Imports System.Data.Linq
Imports Ngl.Core.ChangeTracker
Imports Serilog
Imports SerilogTracing

Public Class NGLBookFeeData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMasBookDataContext(ConnectionString)
        Me.LinqTable = db.BookFees
        Me.LinqDB = db
        Me.SourceClass = "NGLBookFeeData"
        Me.Logger = Logger.ForContext(Of NGLBookFeeData)()

    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMasBookDataContext(ConnectionString)
            _LinqTable = db.BookFees
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
        Return GetBookFeeFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetBookFeesFiltered()
    End Function

    Public Function GetBookFeeFiltered(ByVal Control As Integer) As DataTransferObjects.BookFee
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim BookFee As DataTransferObjects.BookFee = (
                        From d In db.BookFees
                        Where
                        d.BookFeesControl = Control
                        Select selectDTOData(d)).First()

                Return BookFee

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetBookFeesFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetBookFeesFiltered(Optional ByVal BookControl As Integer = 0) As DataTransferObjects.BookFee()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Return all the fees that match the criteria sorted by caption
                Dim BookFees() As DataTransferObjects.BookFee = (
                        From d In db.BookFees
                        Where
                        (d.BookFeesBookControl = BookControl)
                        Order By d.BookFeesCaption
                        Select selectDTOData(d)).ToArray()

                Return BookFees

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetBookFeesFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetBookFeesFiltered(ByVal BookControl As Integer, ByVal FeeType As Utilities.AccessorialFeeType) As DataTransferObjects.BookFee()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Return all the fees that match the criteria sorted by caption
                Dim BookFees() As DataTransferObjects.BookFee = (
                        From d In db.BookFees
                        Where
                        (d.BookFeesBookControl = BookControl) _
                        And d.BookFeesAccessorialFeeTypeControl = FeeType
                        Order By d.BookFeesCaption
                        Select selectDTOData(d)).ToArray()

                Return BookFees

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetBookFeesFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetBookFeesFiltered(ByVal BookControl As Integer, ByVal FeeType As Utilities.AccessorialFeeType, ByVal AccessoraialCode As Integer) As DataTransferObjects.BookFee()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Return all the fees that match the criteria sorted by caption
                Dim BookFees() As DataTransferObjects.BookFee = (
                        From d In db.BookFees
                        Where
                        (d.BookFeesBookControl = BookControl) _
                        And d.BookFeesAccessorialFeeTypeControl = FeeType _
                        And d.BookFeesAccessorialCode = AccessoraialCode
                        Order By d.BookFeesCaption
                        Select selectDTOData(d)).ToArray()

                Return BookFees

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetBookFeesFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetBookFeesFilteredByAccessorialCode(ByVal BookControl As Integer, ByVal AccessoraialCode As Integer) As DataTransferObjects.BookFee()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Return all the fees that match the criteria 
                Dim BookFees() As DataTransferObjects.BookFee = (
                        From d In db.BookFees
                        Where
                        (d.BookFeesBookControl = BookControl) _
                        And d.BookFeesAccessorialCode = AccessoraialCode
                        Order By d.BookFeesAccessorialFeeTypeControl, d.BookFeesAccessorialCode
                        Select selectDTOData(d)).ToArray()

                Return BookFees

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetBookFeesFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function BookFeeExists(ByVal BookControl As Integer, ByVal FeeType As Utilities.AccessorialFeeType, ByVal AccessoraialCode As Integer) As Boolean
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Return (
                    From d In db.BookFees
                    Where
                        (d.BookFeesBookControl = BookControl) _
                        And d.BookFeesAccessorialFeeTypeControl = FeeType _
                        And d.BookFeesAccessorialCode = AccessoraialCode
                    Order By d.BookFeesCaption
                    Select d.BookFeesControl).Any()
            Catch ex As Exception
                Return False
            End Try
            Return False

        End Using
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <typeparam name="TEntity"></typeparam>
    ''' <param name="oData"></param>
    ''' <param name="oLinqTable"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.2 on 7/3/2019
    '''  added New field "BookFeesMissingFee" to support tracking of missing expected carrier fees
    '''  the default Is false.  
    ''' </remarks>
    Public Overrides Function Add(Of TEntity As Class)(ByVal oData As DataTransferObjects.DTOBaseClass,
                                                       ByVal oLinqTable As System.Data.Linq.Table(Of TEntity)) As DataTransferObjects.DTOBaseClass

        Using operation = Logger.StartActivity("NGLBookFeeData.Add<{TEntity}>(oData: {oData}, oLinqTable: {oLinqTable})", GetType(TEntity).Name, oData, oLinqTable)
            Dim oBookDB As NGLMasBookDataContext = TryCast(LinqDB, NGLMasBookDataContext)
            Dim oBookFee As DataTransferObjects.BookFee = TryCast(oData, DataTransferObjects.BookFee)
            Dim oFaultKey As SqlFaultInfo.FaultDetailsKey
            Dim oFaultDetails As New List(Of String)
            Dim intRet As Integer = 0
            Using oBookDB
                'Note: the ValidateData Function must throw a FaultException error on failure
                If Not validateNewRecord(oBookDB, oBookFee, oFaultKey, oFaultDetails) Then
                    throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_InvalidKeyFilterMetaData, oFaultKey, oFaultDetails, SqlFaultInfo.FaultReasons.E_DataValidationFailure, True)
                End If
                Try


                    Dim iQueryBook = oBookDB.Books.Where(Function(x) x.BookControl = oBookFee.BookFeesBookControl)
                    Dim BookPickupStopNumber As Integer = iQueryBook.Select(Function(x) x.BookPickupStopNumber).FirstOrDefault()
                    Dim BookStopNo As Short = iQueryBook.Select(Function(x) x.BookStopNo).FirstOrDefault()

                    With oBookFee
                        intRet = oBookDB.spAddBookFees(BookPickupStopNumber,
                                                       BookStopNo,
                                                       .BookFeesBookControl,
                                                       .BookFeesMinimum,
                                                       .BookFeesValue,
                                                       .BookFeesVariable,
                                                       .BookFeesAccessorialCode,
                                                       .BookFeesAccessorialFeeTypeControl,
                                                       .BookFeesOverRidden,
                                                       .BookFeesVariableCode,
                                                       .BookFeesVisible,
                                                       .BookFeesAutoApprove,
                                                       .BookFeesAllowCarrierUpdates,
                                                       .BookFeesCaption,
                                                       .BookFeesEDICode,
                                                       .BookFeesTaxable,
                                                       .BookFeesIsTax,
                                                       .BookFeesTaxSortOrder,
                                                       .BookFeesBOLText,
                                                       .BookFeesBOLPlacement,
                                                       .BookFeesAccessorialFeeAllocationTypeControl,
                                                       .BookFeesTarBracketTypeControl,
                                                       .BookFeesAccessorialFeeCalcTypeControl,
                                                       .BookFeesAccessorialOverRideReasonControl,
                                                       .BookFeesAccessorialDependencyTypeControl,
                                                       .BookFeesDependencyKey,
                                                       Date.Now(),
                                                       Me.Parameters.UserName,
                                                       .BookFeesMissingFee)
                    End With

                Catch ex As Exception
                    Logger.Error(ex, "Error in NGLBookFeeData.Add")
                    operation.Complete()
                    ManageLinqDataExceptions(ex, buildProcedureName("Add"))
                End Try
                If intRet <> 0 Then
                    Return GetBookFeeFiltered(intRet)
                Else
                    Return New DataTransferObjects.BookFee
                End If
            End Using
        End Using

    End Function


    Public Overrides Function Update(Of TEntity As Class)(ByVal oData As Object,
                                                          ByVal oLinqTable As System.Data.Linq.Table(Of TEntity)) As Object
        Dim oBookFee = TryCast(oData, DataTransferObjects.BookFee)
        If oBookFee Is Nothing OrElse oBookFee.BookFeesControl = 0 Then Return New DataTransferObjects.BookFee()
        Using LinqDB
            SaveLinqData(oBookFee, oLinqTable)
        End Using
        Return GetBookFeeFiltered(oBookFee.BookFeesControl)
    End Function

    Public Overrides Function UpdateQuick(Of TEntity As Class)(ByVal oData As Object,
                                                               ByVal oLinqTable As System.Data.Linq.Table(Of TEntity)) As DataTransferObjects.QuickSaveResults

        Dim oBookFee = TryCast(oData, DataTransferObjects.BookFee)
        If oBookFee Is Nothing OrElse oBookFee.BookFeesControl = 0 Then Return New DataTransferObjects.QuickSaveResults()
        Using LinqDB
            SaveLinqData(oBookFee, oLinqTable)
        End Using
        Return GetQuickSaveResults(oBookFee.BookFeesControl)
    End Function

    Public Overrides Sub UpdateNoReturn(Of TEntity As Class)(ByVal oData As Object,
                                                             ByVal oLinqTable As System.Data.Linq.Table(Of TEntity))
        Using LinqDB
            NoReturnCleanUp(SaveLinqData(oData, oLinqTable))
        End Using
    End Sub


    Public Overrides Sub Delete(Of TEntity As Class)(oData As Object, oLinqTable As Table(Of TEntity))
        Dim oBookDB As NGLMasBookDataContext = TryCast(LinqDB, NGLMasBookDataContext)
        Dim oBookFee As DataTransferObjects.BookFee = TryCast(oData, DataTransferObjects.BookFee)
        Dim oFaultKey As SqlFaultInfo.FaultDetailsKey
        Dim oFaultDetails As New List(Of String)

        Using oBookDB
            'Note: the ValidateData Function must throw a FaultException error on failure
            If Not ValidateDeletedRecord(oBookDB, oBookFee, oFaultKey, oFaultDetails) Then
                throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_InvalidRequest, oFaultKey, oFaultDetails, SqlFaultInfo.FaultReasons.E_DataValidationFailure, True)
            End If
            Try
                Dim intRet = oBookDB.spDeleteBookFees(oBookFee.BookFeesControl, Me.Parameters.UserName)

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("Delete"))
            End Try
        End Using
    End Sub

    Public Function GetvBookFeesFiltered(ByRef RecordCount As Integer,
                                         ByVal BookControl As Integer,
                                         Optional ByVal page As Integer = 1,
                                         Optional ByVal pagesize As Integer = 1000,
                                         Optional ByVal skip As Integer = 0,
                                         Optional ByVal take As Integer = 0) As LTS.vBookFee()
        Dim oRetData As LTS.vBookFee()
        Using operation = Logger.StartActivity("GetvBookFeesFiltered(RecordCount: {RecordCount}, BookControl: {BookControl}, page: {page}, pagesize: {pagesize}, skip: {skip}, take: {take})", RecordCount, BookControl, page, pagesize, skip, take)

            Using db As New NGLMasBookDataContext(ConnectionString)
                Try
                    Dim intPageCount As Integer = 1
                    Dim oQuery As IQueryable(Of LTS.vBookFee)
                    Dim sortExpression As String = "BFControl Desc"

                    oQuery = (From t In db.vBookFees
                              Where t.BFBookControl = BookControl
                              Order By t.BFControl Descending
                              Select t)

                    If oQuery Is Nothing Then Return Nothing

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
                    ManageLinqDataExceptions(ex, buildProcedureName("GetvBookFeesFiltered"))
                End Try

                Return Nothing
            End Using

        End Using
    End Function


    Public Function GetvBookFeesForTariff(ByVal BookControl As Integer) As LTS.vBookFee()
        Dim oRetData As LTS.vBookFee()
        Using operation = Logger.StartActivity("GetvBookFeesForTariff(BookControl: {BookControl})", BookControl)
            Using db As New NGLMasBookDataContext(ConnectionString)
                Try
                    Dim iFeeType As Integer = Utilities.AccessorialFeeType.Tariff
                    oRetData = db.vBookFees.Where(Function(x) x.BFBookControl = BookControl And x.BFAccessorialFeeTypeControl = iFeeType).ToArray()
                Catch ex As Exception
                    ManageLinqDataExceptions(ex, buildProcedureName("GetvBookFeesForTariff"))
                End Try
            End Using
        End Using
        Return oRetData


    End Function


    Public Function GetvBookFeesForLane(ByVal BookControl As Integer) As LTS.vBookFee()
        Dim oRetData As LTS.vBookFee()
        Using operation = Logger.StartActivity("GetvBookFeesForLane(BookControl: {BookControl})", BookControl)
            Using db As New NGLMasBookDataContext(ConnectionString)
                Try
                    Dim iFeeType As Integer = Utilities.AccessorialFeeType.Lane
                    oRetData = db.vBookFees.Where(Function(x) x.BFBookControl = BookControl And x.BFAccessorialFeeTypeControl = iFeeType).ToArray()
                Catch ex As Exception
                    ManageLinqDataExceptions(ex, buildProcedureName("GetvBookFeesForLane"))
                End Try
            End Using
            Return oRetData

        End Using
    End Function

    Public Function GetvBookFeesForOrder(ByVal BookControl As Integer) As LTS.vBookFee()
        Dim oRetData As LTS.vBookFee()
        Using Logger.StartActivity("GetvBookFeesForOrder(BookControl: {BookControl})", BookControl)
            Using db As New NGLMasBookDataContext(ConnectionString)
                Try
                    Dim iFeeType As Integer = Utilities.AccessorialFeeType.Order
                    oRetData = db.vBookFees.Where(Function(x) x.BFBookControl = BookControl And x.BFAccessorialFeeTypeControl = iFeeType).ToArray()
                Catch ex As Exception
                    ManageLinqDataExceptions(ex, buildProcedureName("GetvBookFeesForOrder"))
                End Try

                Return oRetData
            End Using

        End Using
    End Function

    Public Function GetvBookFee(ByVal BookFeeControl As Integer) As LTS.vBookFee
        Dim oRetData As LTS.vBookFee
        Using Logger.StartActivity("GetvBookFee(BookFeeControl: {BookFeeControl})", BookFeeControl)

            Using db As New NGLMasBookDataContext(ConnectionString)
                Try

                    oRetData = db.vBookFees.Where(Function(x) x.BFControl = BookFeeControl).FirstOrDefault()
                Catch ex As Exception
                    ManageLinqDataExceptions(ex, buildProcedureName("GetvBookFee"))
                End Try

                Return oRetData
            End Using

        End Using
    End Function


#End Region

#Region "Protected Functions"

    Friend Overloads Function GetLTSFromDTO(ByVal oData As DataTransferObjects.BookFee) As LTS.BookFee
        Return CopyDTOToLinq(oData)
    End Function

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DataTransferObjects.BookFee)
        Return selectLTSData(d, Me.Parameters.UserName)
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetBookFeeFiltered(Control:=CType(LinqTable, LTS.BookFee).BookFeesControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults

        Dim source As LTS.BookFee = TryCast(LinqTable, LTS.BookFee)
        If source Is Nothing Then Return New DataTransferObjects.QuickSaveResults()
        Return GetQuickSaveResults(source.BookFeesControl)

    End Function

    Protected Overloads Function GetQuickSaveResults(ByVal BookFeesControl As Integer) As DataTransferObjects.QuickSaveResults
        Dim ret As New DataTransferObjects.QuickSaveResults()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                ret = (From d In db.BookFees
                       Where d.BookFeesControl = BookFeesControl
                       Select New DataTransferObjects.QuickSaveResults With {.Control = d.BookFeesControl _
                           , .ModDate = d.BookFeesModDate _
                           , .ModUser = d.BookFeesModUser _
                           , .Updated = d.BookFeesUpdated.ToArray}).FirstOrDefault()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetQuickSaveResults"))
            End Try
        End Using
        Return ret
    End Function

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'Make sure the fee type for the data is Order 
        Dim oBookDB As NGLMasBookDataContext = TryCast(oDB, NGLMasBookDataContext)
        Dim oBookFee As DataTransferObjects.BookFee = TryCast(oData, DataTransferObjects.BookFee)
        Dim oFaultKey As SqlFaultInfo.FaultDetailsKey
        Dim oFaultDetails As New List(Of String)
        If Not validateNewRecord(oBookDB, oBookFee, oFaultKey, oFaultDetails) Then
            throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_InvalidKeyFilterMetaData, oFaultKey, oFaultDetails, SqlFaultInfo.FaultReasons.E_DataValidationFailure, True)
        End If

    End Sub

    Friend Overloads Shared Function validateNewRecord(ByRef oDB As NGLMasBookDataContext, ByRef oBookFee As DataTransferObjects.BookFee, ByRef oFaultKey As SqlFaultInfo.FaultDetailsKey, ByRef oFaultDetails As List(Of String)) As Boolean
        Dim blnRet As Boolean = True
        If oBookFee Is Nothing Or oDB Is Nothing Then Return True
        Dim BookControl As Integer = oBookFee.BookFeesBookControl
        If Not isBookFeeBookControlValid(oDB, oBookFee, oFaultKey, oFaultDetails) Then
            Return False
        End If
        oBookFee.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Order
        Return blnRet
    End Function

    ''' <summary>
    ''' Returns false if the BookFeeBookControl is zero or if it does not exist in the book table
    ''' </summary>
    ''' <param name="oDB"></param>
    ''' <param name="oBookFee"></param>
    ''' <param name="oFaultKey"></param>
    ''' <param name="oFaultDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function isBookFeeBookControlValid(ByRef oDB As NGLMasBookDataContext, ByRef oBookFee As DataTransferObjects.BookFee, ByRef oFaultKey As SqlFaultInfo.FaultDetailsKey, ByRef oFaultDetails As List(Of String)) As Boolean
        Dim blnRet As Boolean = True
        If oBookFee Is Nothing Or oDB Is Nothing Then Return True
        Dim BookControl As Integer = oBookFee.BookFeesBookControl
        If BookControl = 0 Then
            oFaultKey = SqlFaultInfo.FaultDetailsKey.E_CannotSaveParentKeyRequired
            oFaultDetails = New List(Of String) From {"BookFeesBookControl", BookControl.ToString}
            blnRet = False
        ElseIf Not oDB.Books.Any(Function(x) x.BookControl = BookControl) Then
            oFaultKey = SqlFaultInfo.FaultDetailsKey.E_CannotSaveParentKeyRequired
            oFaultDetails = New List(Of String) From {"BookFeesBookControl", BookControl.ToString}
            blnRet = False
        End If
        Return blnRet
    End Function

    ''' <summary>
    ''' Returns True if a copy of the fee is created as an order specific fee.  
    ''' If true the system should only update BookFeesOverRidden flag = true all other 
    ''' changes should be dropped. Returns false if the fee is not overridden.
    ''' Throws exceptions if the data validation rules are violated:
    ''' Example: if fee is not overridden but an existing order specific fee exists 
    ''' throws E_CannotSaveProtectedDataDetails exception
    ''' </summary>
    ''' <param name="oDB"></param>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Overloads Function IsFeeOverRidden(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass) As Boolean
        Dim blnRet As Boolean = False
        Using Logger.StartActivity("IsFeeOverRidden(oData {@oData})", oData)
            Dim oBookDB As NGLMasBookDataContext = TryCast(oDB, NGLMasBookDataContext)
            Dim oBookFee As DataTransferObjects.BookFee = TryCast(oData, DataTransferObjects.BookFee)
            Dim oFaultKey As SqlFaultInfo.FaultDetailsKey
            Dim oFaultDetails As New List(Of String)
            Dim intOverRiddenResults As Integer = NGLBookFeeData.IsFeeOverRidden(oBookDB, oBookFee, oFaultKey, oFaultDetails)


            Logger.Information("Overriden Results: {intOverRiddenResults}", intOverRiddenResults)

            Select Case intOverRiddenResults
                Case 1
                    blnRet = True
                Case -1
                    Select Case oFaultKey
                        Case SqlFaultInfo.FaultDetailsKey.E_CannotSaveParentKeyRequired
                            throwInvalidKeyParentRequiredException(oFaultDetails)
                        Case Else
                            throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_InvalidRequest, oFaultKey, oFaultDetails, SqlFaultInfo.FaultReasons.E_DataValidationFailure)
                    End Select
                Case Else
                    blnRet = False
            End Select
        End Using

        Return blnRet

    End Function

    ''' <summary>
    ''' Returns 1 if a copy of the fee should be created as an order specific fee.  
    ''' If 1 is returned the caller create a copy of the fee and save it as an order 
    ''' specific fee and update the following for the selected record:
    '''     BookFeesOverRidden
    '''     BookFeesAccessorialDependencyTypeControl
    '''     BookFeesAccessorialOverRideReasonControl
    '''     BookFeesDependencyKey
    ''' All other changes should be dropped. 
    ''' Returns 0 if the fee is not overridden.
    ''' Returns -1 and updates the oFaultKey and oFaultDetails if the data validation rules are violated:
    ''' The caller should manage or raise these exceptions as needed
    ''' Example: if fee is not overridden but an existing order specific fee exists 
    ''' oFaultKey will = E_CannotSaveProtectedDataDetails.
    ''' </summary>
    ''' <param name="oDB"></param>
    ''' <param name="oBookFee"></param>
    ''' <param name="oFaultKey"></param>
    ''' <param name="oFaultDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Overloads Shared Function IsFeeOverRidden(ByRef oDB As NGLMasBookDataContext, ByRef oBookFee As DataTransferObjects.BookFee, ByRef oFaultKey As SqlFaultInfo.FaultDetailsKey, ByRef oFaultDetails As List(Of String)) As Integer
        Dim intRet As Integer = 0
        'Check if an update is allowed
        With oBookFee
            If Not isBookFeeBookControlValid(oDB, oBookFee, oFaultKey, oFaultDetails) Then
                Return -1
            End If
            Dim BookControl = .BookFeesBookControl
            Dim AccessoraialCode = .BookFeesAccessorialCode
            Dim sDetails As New List(Of String)
            If .BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Tariff Then
                sDetails = New List(Of String) From {"Fee Type", "Tariff"}
            ElseIf .BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Lane Then
                sDetails = New List(Of String) From {"Fee Type", "Lane"}
            Else
                Return 0 'not overridden ok to save current changes
            End If

            If .BookFeesOverRidden = False Then
                Try
                    If oDB.BookFees.Any(Function(x) x.BookFeesBookControl = BookControl And x.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Order And x.BookFeesAccessorialCode = AccessoraialCode) Then
                        oFaultKey = SqlFaultInfo.FaultDetailsKey.E_CannotSaveProtectedDataDetails
                        oFaultDetails = sDetails
                        Return -1
                    End If
                    intRet = 1
                Catch ex As Exception
                    oFaultKey = SqlFaultInfo.FaultDetailsKey.E_CannotSaveProtectedDataDetails
                    oFaultDetails = sDetails
                    Return -1
                End Try
            Else
                'Cannot update Tariff Booking Fees
                oFaultKey = SqlFaultInfo.FaultDetailsKey.E_CannotSaveProtectedDataDetails
                oFaultDetails = sDetails
                Return -1
            End If
        End With
        Return intRet
    End Function

    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'Make sure the fee type for the data is Order 
        Dim oBookDB As NGLMasBookDataContext = TryCast(oDB, NGLMasBookDataContext)
        Dim oBookFee As DataTransferObjects.BookFee = TryCast(oData, DataTransferObjects.BookFee)
        Dim oFaultKey As SqlFaultInfo.FaultDetailsKey
        Dim oFaultDetails As New List(Of String)
        If Not ValidateDeletedRecord(oBookDB, oBookFee, oFaultKey, oFaultDetails) Then
            throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_InvalidRequest, oFaultKey, oFaultDetails, SqlFaultInfo.FaultReasons.E_DataValidationFailure, True)
        End If
    End Sub

    Friend Overloads Shared Function ValidateDeletedRecord(ByRef oDB As NGLMasBookDataContext, ByRef oBookFee As DataTransferObjects.BookFee, ByRef oFaultKey As SqlFaultInfo.FaultDetailsKey, ByRef oFaultDetails As List(Of String)) As Boolean
        If oDB Is Nothing Then Return False
        If oBookFee Is Nothing Then Return True

        'Check if a delete is allowed
        With oBookFee
            If .BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Tariff Then
                'Cannot delete Tariff Booking Fees
                oFaultKey = SqlFaultInfo.FaultDetailsKey.E_CannotSaveProtectedDataDetails
                oFaultDetails = New List(Of String) From {"Fee Type", "Tariff"}
                Return False
            End If
            If .BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Lane Then
                'Cannot delete Lane Booking Fees
                oFaultKey = SqlFaultInfo.FaultDetailsKey.E_CannotSaveProtectedDataDetails
                oFaultDetails = New List(Of String) From {"Fee Type", "Lane"}
                Return False
            End If
            Return True
        End With
    End Function

    Friend Shared Function selectDTOData(ByVal d As LTS.BookFee, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.BookFee
        Dim oDTO As New DataTransferObjects.BookFee
        Dim skipObjs As New List(Of String) From {"BookFeesBookControl",
                "BookFeesUpdated",
                "BookFeesMinimum",
                "BookFeesVariable",
                "BookFeesAccessorialCode",
                "BookFeesVariableCode",
                "Page",
                "Pages",
                "RecordCount",
                "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .BookFeesBookControl = If(d.BookFeesBookControl.HasValue, d.BookFeesBookControl.Value, 0)
            .BookFeesMinimum = If(d.BookFeesMinimum.HasValue, d.BookFeesMinimum.Value, 0)
            .BookFeesVariable = If(d.BookFeesVariable.HasValue, d.BookFeesVariable.Value, 0)
            .BookFeesAccessorialCode = If(d.BookFeesAccessorialCode.HasValue, d.BookFeesAccessorialCode.Value, 0)
            .BookFeesVariableCode = If(d.BookFeesVariableCode.HasValue, d.BookFeesVariableCode.Value, 0)
            .BookFeesUpdated = d.BookFeesUpdated.ToArray()
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oDTO
        'Return New DTO.BookFee With {.BookFeesControl = d.BookFeesControl _
        '                            , .BookFeesBookControl = If(d.BookFeesBookControl.HasValue, d.BookFeesBookControl.Value, 0) _
        '                            , .BookFeesMinimum = If(d.BookFeesMinimum.HasValue, d.BookFeesMinimum.Value, 0) _
        '                            , .BookFeesValue = d.BookFeesValue _
        '                            , .BookFeesVariable = If(d.BookFeesVariable.HasValue, d.BookFeesVariable.Value, 0) _
        '                            , .BookFeesAccessorialCode = If(d.BookFeesAccessorialCode.HasValue, d.BookFeesAccessorialCode.Value, 0) _
        '                            , .BookFeesAccessorialFeeTypeControl = d.BookFeesAccessorialFeeTypeControl _
        '                            , .BookFeesOverRidden = d.BookFeesOverRidden _
        '                            , .BookFeesVariableCode = If(d.BookFeesVariableCode.HasValue, d.BookFeesVariableCode.Value, 0) _
        '                            , .BookFeesVisible = d.BookFeesVisible _
        '                            , .BookFeesAutoApprove = d.BookFeesAutoApprove _
        '                            , .BookFeesAllowCarrierUpdates = d.BookFeesAllowCarrierUpdates _
        '                            , .BookFeesCaption = d.BookFeesCaption _
        '                            , .BookFeesEDICode = d.BookFeesEDICode _
        '                            , .BookFeesTaxable = d.BookFeesTaxable _
        '                            , .BookFeesIsTax = d.BookFeesIsTax _
        '                            , .BookFeesTaxSortOrder = d.BookFeesTaxSortOrder _
        '                            , .BookFeesBOLText = d.BookFeesBOLText _
        '                            , .BookFeesBOLPlacement = d.BookFeesBOLPlacement _
        '                            , .BookFeesAccessorialFeeAllocationTypeControl = d.BookFeesAccessorialFeeAllocationTypeControl _
        '                            , .BookFeesTarBracketTypeControl = d.BookFeesTarBracketTypeControl _
        '                            , .BookFeesAccessorialFeeCalcTypeControl = d.BookFeesAccessorialFeeCalcTypeControl _
        '                            , .BookFeesModDate = d.BookFeesModDate _
        '                            , .BookFeesModUser = d.BookFeesModUser _
        '                            , .BookFeesUpdated = d.BookFeesUpdated.ToArray(), _
        '                            .Page = page, _
        '                            .Pages = pagecount, _
        '                            .RecordCount = recordcount, _
        '                            .PageSize = pagesize}

    End Function
   
    ''' <summary>
    ''' Caller must encapsulate LinqDB inside using statement
    ''' </summary>
    ''' <typeparam name="TEntity"></typeparam>
    ''' <param name="oData"></param>
    ''' <param name="oLinqTable"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.2 on 7/3/2019
    '''  added New field "BookFeesMissingFee" to support tracking of missing expected carrier fees
    '''  the default Is false.  
    ''' </remarks>
    Private Function SaveLinqData(Of TEntity As Class)(ByVal oData As Object,
                                                       ByVal oLinqTable As System.Data.Linq.Table(Of TEntity)) As Object
        'IsFeeOverRidden validates the data and throws an exception if the fee cannot be processed or overridden
        'the stored procedure spUpdateBookFees will update or insert dependent records and overrides as needed
        'so the return value blnOverridden is not needed by SaveLinqData  
        'but the exceptions will be returned to the caller
        
        Dim nObject As Object
        Using operation = Logger.StartActivity("Wrapper around spUpdateBookFees(oData: {@oData})", oData)
            Dim blnOverridden = IsFeeOverRidden(LinqDB, oData)

            Try
                nObject = CopyDTOToLinq(oData)
                With CType(oData, DataTransferObjects.BookFee)
                    Dim intRet = TryCast(LinqDB, NGLMasBookDataContext).spUpdateBookFees(.BookFeesControl,
                                                                                         .BookFeesBookControl,
                                                                                         .BookFeesMinimum,
                                                                                         .BookFeesValue,
                                                                                         .BookFeesVariable,
                                                                                         .BookFeesAccessorialCode,
                                                                                         .BookFeesAccessorialFeeTypeControl,
                                                                                         .BookFeesOverRidden,
                                                                                         .BookFeesVariableCode,
                                                                                         .BookFeesVisible,
                                                                                         .BookFeesAutoApprove,
                                                                                         .BookFeesAllowCarrierUpdates,
                                                                                         .BookFeesCaption,
                                                                                         .BookFeesEDICode,
                                                                                         .BookFeesTaxable,
                                                                                         .BookFeesIsTax,
                                                                                         .BookFeesTaxSortOrder,
                                                                                         .BookFeesBOLText,
                                                                                         .BookFeesBOLPlacement,
                                                                                         .BookFeesAccessorialFeeAllocationTypeControl,
                                                                                         .BookFeesTarBracketTypeControl,
                                                                                         .BookFeesAccessorialFeeCalcTypeControl,
                                                                                         .BookFeesAccessorialOverRideReasonControl,
                                                                                         .BookFeesAccessorialDependencyTypeControl,
                                                                                         .BookFeesDependencyKey,
                                                                                         Date.Now(),
                                                                                         Me.Parameters.UserName,
                                                                                         .BookFeesMissingFee)

                End With
            Catch ex As Exception
                Logger.Error(ex, "Error in NGLBookFeeData.SaveLinqData")
                ManageLinqDataExceptions(ex, buildProcedureName("SaveLinqData"))
            End Try
        End Using

        Return nObject
    End Function
    Dim shared _getBookFeeChangesLock = New Object()
    Friend Shared Function GetBookFeeChanges(ByVal source As DataTransferObjects.BookRevenue, ByVal changeType As TrackingInfo, ByVal UserName As String) As List(Of LTS.BookFee)
        If source Is Nothing OrElse source.BookFees Is Nothing OrElse source.BookFees.Count < 1 Then Return New List(Of LTS.BookFee)
        ' Test for specified change type.
        Dim details As IEnumerable(Of LTS.BookFee) = (
                From d In source.BookFees
                Where d.TrackingState = changeType
                Select selectLTSData(d, UserName))
        Return details.ToList()
    End Function

    Friend Shared Function selectLTSData(ByVal d As DataTransferObjects.BookFee, ByVal UserName As String) As LTS.BookFee
        Dim oLTS As New LTS.BookFee
        UpdateLTSWithDTO(d, oLTS, UserName)
        Return oLTS
    End Function

    Shared _lockObject As New Object
    Friend Shared Sub UpdateLTSWithDTO(ByRef d As DataTransferObjects.BookFee, ByRef t As LTS.BookFee, ByVal UserName As String)
        SyncLock _lockObject
            Using Serilog.Log.Logger.StartActivity("UpdateLTSWithDTO(d: {@d}, t: {@t}, UserName: {UserName})", d, t, UserName)

                Dim skipObjs As New List(Of String) From {"BookFeesBookControl",
                    "BookFeesMinimum",
                    "BookFeesVariable",
                    "BookFeesAccessorialCode",
                    "BookFeesVariableCode",
                    "BookFeesModDate",
                    "BookFeesModUser",
                    "BookFeesUpdated"}
                t = CopyMatchingFields(t, d, skipObjs)
                With t
                    .BookFeesBookControl = d.BookFeesBookControl
                    .BookFeesMinimum = d.BookFeesMinimum
                    .BookFeesVariable = d.BookFeesVariable
                    .BookFeesAccessorialCode = d.BookFeesAccessorialCode
                    .BookFeesVariableCode = d.BookFeesVariableCode
                    .BookFeesModDate = Date.Now
                    .BookFeesModUser = UserName
                    'Removed by RHR 1/27/2016 seems we get errors on the BookFeesUpdated field when saving some times
                    'it is not required because we do not want to check optimistic concurrencay if we get here anyway
                    '.BookFeesUpdated = If(d.BookFeesUpdated Is Nothing, New Byte() {}, d.BookFeesUpdated)
                End With


            End Using
        End SyncLock
    End Sub

#End Region

End Class