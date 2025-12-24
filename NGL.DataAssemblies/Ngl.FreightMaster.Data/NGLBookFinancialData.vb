Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.ServiceModel

Public Class NGLBookFinancialData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMasBookDataContext(ConnectionString)
        Me.LinqTable = db.vBookFinancials
        Me.LinqDB = db
        Me.SourceClass = "NGLBookFinancialData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMasBookDataContext(ConnectionString)
            Me.LinqTable = db.vBookFinancials
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
        Return GetBookFinancialFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    Public Function GetBookFinancialFiltered(ByVal Control As Integer) As DataTransferObjects.BookFinancial
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim oBookFinancial As DataTransferObjects.BookFinancial = (
                        From d In db.vBookFinancials
                        Where
                        d.BookControl = Control
                        Select New DataTransferObjects.BookFinancial With {.BookControl = d.BookControl _
                        , .BookCarrierControl = d.BookCarrierControl _
                        , .BookFinARBookFrt = If(d.BookFinARBookFrt.HasValue, d.BookFinARBookFrt, 0) _
                        , .BookFinARInvoiceDate = d.BookFinARInvoiceDate _
                        , .BookFinARInvoiceAmt = If(d.BookFinARInvoiceAmt.HasValue, d.BookFinARInvoiceAmt, 0) _
                        , .BookFinARPayDate = d.BookFinARPayDate _
                        , .BookFinARPayAmt = If(d.BookFinARPayAmt.HasValue, d.BookFinARPayAmt, 0) _
                        , .BookFinARCheck = d.BookFinARCheck _
                        , .BookFinARGLNumber = d.BookFinARGLNumber _
                        , .BookFinARBalance = If(d.BookFinARBalance.HasValue, d.BookFinARBalance, 0) _
                        , .BookFinARCurType = If(d.BookFinARCurType.HasValue, d.BookFinARCurType, 0) _
                        , .BookFinAPBillNumber = d.BookFinAPBillNumber _
                        , .BookFinAPBillNoDate = d.BookFinAPBillNoDate _
                        , .BookFinAPBillInvDate = d.BookFinAPBillInvDate _
                        , .BookFinAPActWgt = If(d.BookFinAPActWgt.HasValue, d.BookFinAPActWgt, 0) _
                        , .BookFinAPStdCost = If(d.BookFinAPStdCost.HasValue, d.BookFinAPStdCost, 0) _
                        , .BookFinAPActCost = If(d.BookFinAPActCost.HasValue, d.BookFinAPActCost, 0) _
                        , .BookFinAPPayDate = d.BookFinAPPayDate _
                        , .BookFinAPPayAmt = If(d.BookFinAPPayAmt.HasValue, d.BookFinAPPayAmt, 0) _
                        , .BookFinAPCheck = d.BookFinAPCheck _
                        , .BookFinAPGLNumber = d.BookFinAPGLNumber _
                        , .BookFinAPLastViewed = d.BookFinAPLastViewed _
                        , .BookFinAPCurType = If(d.BookFinAPCurType.HasValue, d.BookFinAPCurType, 0) _
                        , .BookFinCommStd = If(d.BookFinCommStd.HasValue, d.BookFinCommStd, 0) _
                        , .BookFinCommAct = If(d.BookFinCommAct.HasValue, d.BookFinCommAct, 0) _
                        , .BookFinCommPayDate = d.BookFinCommPayDate _
                        , .BookFinCommPayAmt = If(d.BookFinCommPayAmt.HasValue, d.BookFinCommPayAmt, 0) _
                        , .BookFinCommtCheck = d.BookFinCommtCheck _
                        , .BookFinCommCreditAmt = If(d.BookFinCommCreditAmt.HasValue, d.BookFinCommCreditAmt, 0) _
                        , .BookFinCommCreditPayDate = d.BookFinCommCreditPayDate _
                        , .BookFinCommLoadCount = If(d.BookFinCommLoadCount.HasValue, d.BookFinCommLoadCount, 0) _
                        , .BookFinCommGLNumber = d.BookFinCommGLNumber _
                        , .BookFinCheckClearedDate = d.BookFinCheckClearedDate _
                        , .BookFinCheckClearedNumber = d.BookFinCheckClearedNumber _
                        , .BookFinCheckClearedAmt = If(d.BookFinCheckClearedAmt.HasValue, d.BookFinCheckClearedAmt, 0) _
                        , .BookFinCheckClearedDesc = d.BookFinCheckClearedDesc _
                        , .BookFinCheckClearedAcct = d.BookFinCheckClearedAcct _
                        , .BookModDate = d.BookModDate _
                        , .BookModUser = d.BookModUser _
                        , .BookFinAPExportFlag = d.BookFinAPExportFlag _
                        , .BookFinServiceFee = If(d.BookFinServiceFee.HasValue, d.BookFinServiceFee, 0) _
                        , .BookFinAPExportDate = d.BookFinAPExportDate _
                        , .BookFinAPExportRetry = If(d.BookFinAPExportRetry.HasValue, d.BookFinAPExportRetry, 0) _
                        , .BookDoNotInvoice = d.BookDoNotInvoice _
                        , .BookDateLoad = d.BookDateLoad _
                        , .BookRouteFinalCode = d.BookRouteFinalCode _
                        , .BookConsPrefix = d.BookConsPrefix _
                        , .BookSHID = d.BookSHID _
                        , .ARCustomerText = d.ARCustomerText _
                        , .APCarrierText = d.APCarrierText _
                        , .APCommissionsText = d.APCommissionsText}).First

                Return oBookFinancial

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

            Return Nothing

        End Using
    End Function

    Public Overrides Function Update(Of TEntity As Class)(ByVal oData As Object,
                                                          ByVal oLinqTable As System.Data.Linq.Table(Of TEntity)) As Object
        SaveChanges(oData)
        ' Return the updated order
        Return GetBookFinancialFiltered(Control:=oData.BookControl)

    End Function

    Public Overrides Sub UpdateNoReturn(Of TEntity As Class)(ByVal oData As Object,
                                                             ByVal oLinqTable As System.Data.Linq.Table(Of TEntity))
        SaveChanges(oData)
    End Sub

#End Region


#Region "365 Methods"

    ''' <summary>
    ''' Read the LTS.vBookFinancial from the book table
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.2.006 on 12/16/2022 added new LTS methods for DTMS 365
    ''' </remarks>
    Public Function GetvBookFinancial(ByVal BookControl As Integer) As LTS.vBookFinancial
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                If BookControl = 0 OrElse Not db.Books.Any(Function(x) x.BookControl = BookControl) Then throwNoDataFaultMessage("E_InvalidRecordKey") 'we do not have a valid filter - "The reference to the record is missing. Please select another record and try again."
                Return db.vBookFinancials.Where(Function(x) x.BookControl = BookControl).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetvBookNotes"), db)
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Save changes to the LTS vBookFinancial data
    ''' </summary>
    ''' <param name="oRecord"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.2.006 on 12/16/2022 added new LTS methods for DTMS 365
    ''' Note: SaveChanges will update the ModUser and ModDate information
    ''' </remarks>
    Public Function UpdatevBookFinancial(ByVal oRecord As LTS.vBookFinancial) As Boolean
        Dim db As New NGLMasBookDataContext(ConnectionString)
        SaveChanges(oRecord, db)
        Return True 'true when no error 
    End Function



#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DataTransferObjects.BookFinancial)
        'Create New Record
        Return New LTS.vBookFinancial With {.BookControl = d.BookControl _
            , .BookCarrierControl = d.BookCarrierControl _
            , .BookFinARBookFrt = d.BookFinARBookFrt _
            , .BookFinARInvoiceDate = d.BookFinARInvoiceDate _
            , .BookFinARInvoiceAmt = d.BookFinARInvoiceAmt _
            , .BookFinARPayDate = d.BookFinARPayDate _
            , .BookFinARPayAmt = d.BookFinARPayAmt _
            , .BookFinARCheck = d.BookFinARCheck _
            , .BookFinARGLNumber = d.BookFinARGLNumber _
            , .BookFinARBalance = d.BookFinARBalance _
            , .BookFinARCurType = d.BookFinARCurType _
            , .BookFinAPBillNumber = d.BookFinAPBillNumber _
            , .BookFinAPBillNoDate = d.BookFinAPBillNoDate _
            , .BookFinAPBillInvDate = d.BookFinAPBillInvDate _
            , .BookFinAPActWgt = d.BookFinAPActWgt _
            , .BookFinAPStdCost = d.BookFinAPStdCost _
            , .BookFinAPActCost = d.BookFinAPActCost _
            , .BookFinAPPayDate = d.BookFinAPPayDate _
            , .BookFinAPPayAmt = d.BookFinAPPayAmt _
            , .BookFinAPCheck = d.BookFinAPCheck _
            , .BookFinAPGLNumber = d.BookFinAPGLNumber _
            , .BookFinAPLastViewed = d.BookFinAPLastViewed _
            , .BookFinAPCurType = d.BookFinAPCurType _
            , .BookFinCommStd = d.BookFinCommStd _
            , .BookFinCommAct = d.BookFinCommAct _
            , .BookFinCommPayDate = d.BookFinCommPayDate _
            , .BookFinCommPayAmt = d.BookFinCommPayAmt _
            , .BookFinCommtCheck = d.BookFinCommtCheck _
            , .BookFinCommCreditAmt = d.BookFinCommCreditAmt _
            , .BookFinCommCreditPayDate = d.BookFinCommCreditPayDate _
            , .BookFinCommLoadCount = d.BookFinCommLoadCount _
            , .BookFinCommGLNumber = d.BookFinCommGLNumber _
            , .BookFinCheckClearedDate = d.BookFinCheckClearedDate _
            , .BookFinCheckClearedNumber = d.BookFinCheckClearedNumber _
            , .BookFinCheckClearedAmt = d.BookFinCheckClearedAmt _
            , .BookFinCheckClearedDesc = d.BookFinCheckClearedDesc _
            , .BookFinCheckClearedAcct = d.BookFinCheckClearedAcct _
            , .BookModDate = Date.Now _
            , .BookModUser = Me.Parameters.UserName _
            , .BookFinAPExportFlag = d.BookFinAPExportFlag _
            , .BookFinServiceFee = d.BookFinServiceFee _
            , .BookFinAPExportDate = d.BookFinAPExportDate _
            , .BookFinAPExportRetry = d.BookFinAPExportRetry _
            , .BookDoNotInvoice = d.BookDoNotInvoice _
            , .BookDateLoad = d.BookDateLoad _
            , .BookRouteFinalCode = d.BookRouteFinalCode _
            , .BookSHID = d.BookSHID _
            , .BookConsPrefix = d.BookConsPrefix}

    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetRecordFiltered(Control:=CType(LinqTable, LTS.vBookFinancial).BookControl)
    End Function

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'We do not allow booking records to be inserted from this class
        Utilities.SaveAppError("Cannot add data.  Records cannot be inserted using this interface!", Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))
    End Sub

    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'We do not allow booking records to be deleted from this class
        Utilities.SaveAppError("Cannot delete data.  Records cannot be deleted using this interface!", Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))

    End Sub

    ''' <summary>
    ''' Save DTO data to book table using addToConflicts optimistic concurrancy
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.2.006 on 12/16/2022 calls new SaveChanges overload 
    ''' </remarks>
    Private Sub SaveChanges(ByVal oData As DataTransferObjects.BookFinancial)

        'Note: the ValidateData Function must throw a FaultException error on failure
        ValidateUpdatedRecord(LinqDB, oData)
        Dim lTSData As LTS.vBookFinancial = CopyDTOToLinq(oData)
        SaveChanges(lTSData, DirectCast(LinqDB, NGLMasBookDataContext))

    End Sub

    ''' <summary>
    ''' Save LTS vBookFinancial to book table using addToConflicts optimistic concurrancy
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <param name="db"></param>
    ''' <remarks>
    ''' Created by RHR for v-8.5.2.006 on 12/16/2022 added override to allow for LTL D365 save
    ''' Modified by RHR on 11/08/2023 for v-8.5.4.003 the vBookFinancials view is not saving,  switched to book table
    ''' </remarks>
    Private Sub SaveChanges(ByVal oData As LTS.vBookFinancial, ByRef db As NGLMasBookDataContext)
        Using db
            With oData
                Try
                    'LinqDB.Log = New DebugTextWriter
                    'Open the existing Record
                    ' Modified by RHR on 11/08/2023 for v-8.5.4.003
                    'Dim d = (From e In db.vBookFinancials Where e.BookControl = .BookControl Select e).First
                    Dim d = (From e In db.Books Where e.BookControl = .BookControl Select e).First
                    If d Is Nothing Then
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_RecordDeleted"}, New FaultReason("E_InvalidOperationException"))
                    Else
                        'Check for conflicts
                        'Modified by RHR for v-8.5.2.006 on 12/29/2022 check for seconds because D365 does not save milliseconds
                        Dim iSeconds = DateDiff(DateInterval.Second, .BookModDate.Value, d.BookModDate.Value)
                        If iSeconds > 0 Then
                            'the data has changed so check each field for conflicts
                            Dim ConflictData As New List(Of KeyValuePair(Of String, String))
                            Dim blnConflictFound As Boolean = False
                            addToConflicts("BookCarrierControl", .BookCarrierControl, d.BookCarrierControl, ConflictData, blnConflictFound)
                            addToConflicts("BookFinARBookFrt", .BookFinARBookFrt, If(d.BookFinARBookFrt.HasValue, d.BookFinARBookFrt, 0), ConflictData, blnConflictFound)
                            addToConflicts("BookFinARInvoiceDate", .BookFinARInvoiceDate, d.BookFinARInvoiceDate, ConflictData, blnConflictFound)
                            addToConflicts("BookFinARInvoiceAmt", .BookFinARInvoiceAmt, If(d.BookFinARInvoiceAmt.HasValue, d.BookFinARInvoiceAmt, 0), ConflictData, blnConflictFound)
                            addToConflicts("BookFinARPayDate", .BookFinARPayDate, d.BookFinARPayDate, ConflictData, blnConflictFound)
                            addToConflicts("BookFinARPayAmt", .BookFinARPayAmt, If(d.BookFinARPayAmt.HasValue, d.BookFinARPayAmt, 0), ConflictData, blnConflictFound)
                            addToConflicts("BookFinARCheck", .BookFinARCheck, d.BookFinARCheck, ConflictData, blnConflictFound)
                            addToConflicts("BookFinARGLNumber", .BookFinARGLNumber, d.BookFinARGLNumber, ConflictData, blnConflictFound)
                            addToConflicts("BookFinARBalance", .BookFinARBalance, d.BookFinARBalance, ConflictData, blnConflictFound)
                            addToConflicts("BookFinARCurType", .BookFinARCurType, d.BookFinARCurType, ConflictData, blnConflictFound)
                            addToConflicts("BookFinAPBillNumber", .BookFinAPBillNumber, d.BookFinAPBillNumber, ConflictData, blnConflictFound)
                            addToConflicts("BookFinAPBillNoDate", .BookFinAPBillNoDate, d.BookFinAPBillNoDate, ConflictData, blnConflictFound)
                            addToConflicts("BookFinAPBillInvDate", .BookFinAPBillInvDate, d.BookFinAPBillInvDate, ConflictData, blnConflictFound)
                            addToConflicts("BookFinAPActWgt", .BookFinAPActWgt, If(d.BookFinAPActWgt.HasValue, d.BookFinAPActWgt, 0), ConflictData, blnConflictFound)
                            addToConflicts("BookFinAPStdCost", .BookFinAPStdCost, If(d.BookFinAPStdCost.HasValue, d.BookFinAPStdCost, 0), ConflictData, blnConflictFound)
                            addToConflicts("BookFinAPActCost", .BookFinAPActCost, If(d.BookFinAPActCost.HasValue, d.BookFinAPActCost, 0), ConflictData, blnConflictFound)
                            addToConflicts("BookFinAPPayDate", .BookFinAPPayDate, d.BookFinAPPayDate, ConflictData, blnConflictFound)
                            addToConflicts("BookFinAPPayAmt", .BookFinAPPayAmt, If(d.BookFinAPPayAmt.HasValue, d.BookFinAPPayAmt, 0), ConflictData, blnConflictFound)
                            addToConflicts("BookFinAPCheck", .BookFinAPCheck, d.BookFinAPCheck, ConflictData, blnConflictFound)
                            addToConflicts("BookFinAPGLNumber", .BookFinAPGLNumber, d.BookFinAPGLNumber, ConflictData, blnConflictFound)
                            addToConflicts("BookFinAPLastViewed", .BookFinAPLastViewed, d.BookFinAPLastViewed, ConflictData, blnConflictFound)
                            addToConflicts("BookFinAPCurType", .BookFinAPCurType, If(d.BookFinAPCurType.HasValue, d.BookFinAPCurType, 0), ConflictData, blnConflictFound)
                            addToConflicts("BookFinCommStd", .BookFinCommStd, If(d.BookFinCommStd.HasValue, d.BookFinCommStd, 0), ConflictData, blnConflictFound)
                            addToConflicts("BookFinCommAct", .BookFinCommAct, If(d.BookFinCommAct.HasValue, d.BookFinCommAct, 0), ConflictData, blnConflictFound)
                            addToConflicts("BookFinCommPayDate", .BookFinCommPayDate, d.BookFinCommPayDate, ConflictData, blnConflictFound)
                            addToConflicts("BookFinCommPayAmt", .BookFinCommPayAmt, If(d.BookFinCommPayAmt.HasValue, d.BookFinCommPayAmt, 0), ConflictData, blnConflictFound)
                            addToConflicts("BookFinCommtCheck", .BookFinCommtCheck, d.BookFinCommtCheck, ConflictData, blnConflictFound)
                            addToConflicts("BookFinCommCreditAmt", .BookFinCommCreditAmt, If(d.BookFinCommCreditAmt.HasValue, d.BookFinCommCreditAmt, 0), ConflictData, blnConflictFound)
                            addToConflicts("BookFinCommCreditPayDate", .BookFinCommCreditPayDate, d.BookFinCommCreditPayDate, ConflictData, blnConflictFound)
                            addToConflicts("BookFinCommLoadCount", .BookFinCommLoadCount, If(d.BookFinCommLoadCount.HasValue, d.BookFinCommLoadCount, 0), ConflictData, blnConflictFound)
                            addToConflicts("BookFinCommGLNumber", .BookFinCommGLNumber, d.BookFinCommGLNumber, ConflictData, blnConflictFound)
                            addToConflicts("BookFinCheckClearedDate", .BookFinCheckClearedDate, d.BookFinCheckClearedDate, ConflictData, blnConflictFound)
                            addToConflicts("BookFinCheckClearedNumber", .BookFinCheckClearedNumber, d.BookFinCheckClearedNumber, ConflictData, blnConflictFound)
                            addToConflicts("BookFinCheckClearedAmt", .BookFinCheckClearedAmt, d.BookFinCheckClearedAmt, ConflictData, blnConflictFound)
                            addToConflicts("BookFinCheckClearedDesc", .BookFinCheckClearedDesc, d.BookFinCheckClearedDesc, ConflictData, blnConflictFound)
                            addToConflicts("BookFinCheckClearedAcct", .BookFinCheckClearedAcct, d.BookFinCheckClearedAcct, ConflictData, blnConflictFound)
                            addToConflicts("BookFinAPExportFlag", .BookFinAPExportFlag, d.BookFinAPExportFlag, ConflictData, blnConflictFound)
                            addToConflicts("BookFinServiceFee", .BookFinServiceFee, If(d.BookFinServiceFee.HasValue, d.BookFinServiceFee, 0), ConflictData, blnConflictFound)
                            addToConflicts("BookFinAPExportDate", .BookFinAPExportDate, d.BookFinAPExportDate, ConflictData, blnConflictFound)
                            addToConflicts("BookFinAPExportRetry", .BookFinAPExportRetry, If(d.BookFinAPExportRetry.HasValue, d.BookFinAPExportRetry, 0), ConflictData, blnConflictFound)
                            addToConflicts("BookDoNotInvoice", .BookDoNotInvoice, d.BookDoNotInvoice, ConflictData, blnConflictFound)
                            addToConflicts("BookDateLoad", .BookDateLoad, d.BookDateLoad, ConflictData, blnConflictFound)
                            addToConflicts("BookRouteFinalCode", .BookRouteFinalCode, d.BookRouteFinalCode, ConflictData, blnConflictFound)
                            addToConflicts("BookConsPrefix", .BookConsPrefix, d.BookConsPrefix, ConflictData, blnConflictFound)
                            addToConflicts("BookSHID", .BookSHID, d.BookSHID, ConflictData, blnConflictFound)
                            If blnConflictFound Then
                                'We only add the mod date and mod user if one or more other fields have been modified
                                addToConflicts("BookModDate", .BookModDate, d.BookModDate, ConflictData, blnConflictFound)
                                addToConflicts("BookModUser", .BookModUser, d.BookModUser, ConflictData, blnConflictFound)
                                Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(ConflictData))
                                conflictInfo.LogConflictDetails(conflictInfo.Message, Me.Parameters)
                                Throw New FaultException(Of ConflictFaultInfo)(conflictInfo, New FaultReason("E_DataConflict"))
                            End If
                        End If
                        'Update the table data
                        d.BookCarrierControl = .BookCarrierControl
                        d.BookFinARBookFrt = .BookFinARBookFrt
                        d.BookFinARInvoiceDate = .BookFinARInvoiceDate
                        d.BookFinARInvoiceAmt = .BookFinARInvoiceAmt
                        d.BookFinARPayDate = .BookFinARPayDate
                        d.BookFinARPayAmt = .BookFinARPayAmt
                        d.BookFinARCheck = .BookFinARCheck
                        d.BookFinARGLNumber = .BookFinARGLNumber
                        d.BookFinARBalance = .BookFinARBalance
                        d.BookFinARCurType = .BookFinARCurType
                        d.BookFinAPBillNumber = .BookFinAPBillNumber
                        d.BookFinAPBillNoDate = .BookFinAPBillNoDate
                        d.BookFinAPBillInvDate = .BookFinAPBillInvDate
                        d.BookFinAPActWgt = .BookFinAPActWgt
                        d.BookFinAPStdCost = .BookFinAPStdCost
                        d.BookFinAPActCost = .BookFinAPActCost
                        d.BookFinAPPayDate = .BookFinAPPayDate
                        d.BookFinAPPayAmt = .BookFinAPPayAmt
                        d.BookFinAPCheck = .BookFinAPCheck
                        d.BookFinAPGLNumber = .BookFinAPGLNumber
                        d.BookFinAPLastViewed = .BookFinAPLastViewed
                        d.BookFinAPCurType = .BookFinAPCurType
                        d.BookFinCommStd = .BookFinCommStd
                        d.BookFinCommAct = .BookFinCommAct
                        d.BookFinCommPayDate = .BookFinCommPayDate
                        d.BookFinCommPayAmt = .BookFinCommPayAmt
                        d.BookFinCommtCheck = .BookFinCommtCheck
                        d.BookFinCommCreditAmt = .BookFinCommCreditAmt
                        d.BookFinCommCreditPayDate = .BookFinCommCreditPayDate
                        d.BookFinCommLoadCount = .BookFinCommLoadCount
                        d.BookFinCommGLNumber = .BookFinCommGLNumber
                        d.BookFinCheckClearedDate = .BookFinCheckClearedDate
                        d.BookFinCheckClearedNumber = .BookFinCheckClearedNumber
                        d.BookFinCheckClearedAmt = .BookFinCheckClearedAmt
                        d.BookFinCheckClearedDesc = .BookFinCheckClearedDesc
                        d.BookFinCheckClearedAcct = .BookFinCheckClearedAcct
                        d.BookModDate = Date.Now
                        d.BookModUser = Me.Parameters.UserName
                        d.BookFinAPExportFlag = .BookFinAPExportFlag
                        d.BookFinServiceFee = .BookFinServiceFee
                        d.BookFinAPExportDate = .BookFinAPExportDate
                        d.BookFinAPExportRetry = .BookFinAPExportRetry
                        d.BookDoNotInvoice = .BookDoNotInvoice
                        d.BookDateLoad = .BookDateLoad
                        d.BookRouteFinalCode = .BookRouteFinalCode
                        d.BookSHID = .BookSHID
                        d.BookConsPrefix = .BookConsPrefix
                    End If
                    db.SubmitChanges()
                Catch ex As FaultException
                    Throw
                Catch ex As SqlException
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
                Catch conflictEx As ChangeConflictException
                    Try
                        Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(db))
                        conflictInfo.LogConflictDetails(conflictInfo.Message, Me.Parameters)
                        Throw New FaultException(Of ConflictFaultInfo)(conflictInfo, New FaultReason("E_DataConflict"))
                    Catch ex As FaultException
                        Throw
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                Catch ex As InvalidOperationException
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_RecordDeleted"}, New FaultReason("E_InvalidOperationException"))
                Catch ex As Exception
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
                End Try
                'We call UpdateBookDependencies but we do not recalculate we only check for errors
                DirectCast(Me.NDPBaseClassFactory("NGLBookData", False), NGLBookData).UpdateBookDependencies(oData.BookControl, 0)
            End With
        End Using

    End Sub

#End Region

End Class