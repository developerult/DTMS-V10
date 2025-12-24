Imports System.ServiceModel
Imports NGL.Core.ChangeTracker
Imports Ngl.FreightMaster.Data.DataTransferObjects
Imports Serilog

Public Class NGLBookItemData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        'Dim db As New NGLMasBookDataContext(ConnectionString)
        'Me.LinqTable = db.BookItems
        'Me.LinqDB = db
        Me.SourceClass = "NGLBookItemData"
        Logger = Me.Logger.ForContext(Of NGLBookItemData)

    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            'If _LinqTable Is Nothing Then
            Dim db As New NGLMasBookDataContext(ConnectionString)
            _LinqTable = db.BookItems
            _LinqDB = db
            'End If
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property


    Private _BookDependencyResult As LTS.spUpdateBookDependenciesResult
    Public Property BookDependencyResult() As LTS.spUpdateBookDependenciesResult
        Get
            Return _BookDependencyResult
        End Get
        Set(ByVal value As LTS.spUpdateBookDependenciesResult)
            _BookDependencyResult = value
        End Set
    End Property

    Private _LastProcedureName As String
    Public Property LastProcedureName() As String
        Get
            Return _LastProcedureName
        End Get
        Set(ByVal value As String)
            _LastProcedureName = value
        End Set
    End Property


    Private _LastBookItemControl As Integer
    Public Property LastBookItemControl() As Integer
        Get
            Return _LastBookItemControl
        End Get
        Set(ByVal value As Integer)
            _LastBookItemControl = value
        End Set
    End Property

#End Region

#Region "Public Methods"

#Region " Overridden data methods"

    ''' <summary>
    ''' This override does not call the standard cleanup methods and returns nothing.
    ''' The caller must check the BookDependencyResult properties (ReturnBookControl,MustRecalculate,RetMsg, and ErrNumber) 
    ''' , recalculate costs and return the updated bookitem record
    ''' </summary>
    ''' <typeparam name="TEntity"></typeparam>
    ''' <param name="oData"></param>
    ''' <param name="oLinqTable"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function Add(Of TEntity As Class)(ByVal oData As DataTransferObjects.DTOBaseClass,
                                                       ByVal oLinqTable As System.Data.Linq.Table(Of TEntity)) As DataTransferObjects.DTOBaseClass
        LastBookItemControl = 0
        Using LinqDB
            'Note: the ValidateData Function must throw a FaultException error on failure
            ValidateNewRecord(LinqDB, oData)
            'Create New Record 
            Dim nObject = CopyDTOToLinq(oData)
            oLinqTable.InsertOnSubmit(nObject)
            Try
                LinqDB.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("Add"))
            End Try
            Dim source As LTS.BookItem = TryCast(nObject, LTS.BookItem)
            If source Is Nothing Then Return Nothing
            LastBookItemControl = source.BookItemControl
            Me.BookDependencyResult = DirectCast(Me.NDPBaseClassFactory("NGLBookData", False), NGLBookData).UpdateBookDependencies(0, source.BookItemBookLoadControl)
            Me.LastProcedureName = "spUpdateBookDependencies"
            Return Nothing
        End Using

    End Function

    ''' <summary>
    ''' This override does not call the standard cleanup methods and returns nothing.
    ''' The caller must check the BookDependencyResult properties (ReturnBookControl,MustRecalculate,RetMsg, and ErrNumber) 
    ''' , recalculate costs 
    ''' </summary>
    ''' <typeparam name="TEntity"></typeparam>
    ''' <param name="oData"></param>
    ''' <param name="oLinqTable"></param>
    ''' <remarks></remarks>
    Public Overrides Sub Delete(Of TEntity As Class)(ByVal oData As Object,
                                                     ByVal oLinqTable As System.Data.Linq.Table(Of TEntity))
        Dim intBookLoadControl As Integer = 0
        Dim dtoObject As DataTransferObjects.BookItem = TryCast(oData, DataTransferObjects.BookItem)
        If Not dtoObject Is Nothing Then intBookLoadControl = dtoObject.BookItemBookLoadControl
        Using LinqDB
            'Note: the ValidateData Function must throw a FaultException error on failure
            ValidateDeletedRecord(LinqDB, oData)
            'Create New Record
            Dim nObject = CopyDTOToLinq(oData)
            oLinqTable.Attach(nObject, True)
            oLinqTable.DeleteOnSubmit(nObject)
            Try
                LinqDB.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("Delete"))
            End Try
            If intBookLoadControl <> 0 Then
                Me.BookDependencyResult = DirectCast(Me.NDPBaseClassFactory("NGLBookData", False), NGLBookData).UpdateBookDependencies(0, intBookLoadControl)
                Me.LastProcedureName = "spUpdateBookDependencies"
            End If
        End Using
    End Sub

    ''' <summary>
    ''' This override does not call the standard cleanup methods and returns nothing.
    ''' The caller must check the BookDependencyResult properties (ReturnBookControl,MustRecalculate,RetMsg, and ErrNumber) 
    ''' , recalculate costs and return the updated bookitem record
    ''' </summary>
    ''' <typeparam name="TEntity"></typeparam>
    ''' <param name="oData"></param>
    ''' <param name="oLinqTable"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function Update(Of TEntity As Class)(ByVal oData As Object,
                                                          ByVal oLinqTable As System.Data.Linq.Table(Of TEntity)) As Object
        LastBookItemControl = 0
        Using LinqDB
            'Note: the ValidateData Function must throw a FaultException error on failure
            ValidateUpdatedRecord(LinqDB, oData)
            'Create New Record 
            Dim nObject = CopyDTOToLinq(oData)
            ' Attach the record 
            oLinqTable.Attach(nObject, True)
            Try
                LinqDB.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("Update"))
            End Try
            Dim source As LTS.BookItem = TryCast(nObject, LTS.BookItem)
            If source Is Nothing Then Return Nothing
            LastBookItemControl = source.BookItemControl
            Me.BookDependencyResult = DirectCast(Me.NDPBaseClassFactory("NGLBookData", False), NGLBookData).UpdateBookDependencies(0, source.BookItemBookLoadControl)
            Me.LastProcedureName = "spUpdateBookDependencies"
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' This override does not call the standard cleanup methods and returns nothing.
    ''' The caller must check the BookDependencyResult properties (ReturnBookControl,MustRecalculate,RetMsg, and ErrNumber) 
    ''' , recalculate costs and return the updated bookitem quick details
    ''' </summary>
    ''' <typeparam name="TEntity"></typeparam>
    ''' <param name="oData"></param>
    ''' <param name="oLinqTable"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function UpdateQuick(Of TEntity As Class)(ByVal oData As Object,
                                                               ByVal oLinqTable As System.Data.Linq.Table(Of TEntity)) As DataTransferObjects.QuickSaveResults
        LastBookItemControl = 0
        Using LinqDB
            Dim blnchanged As Boolean = CompareUpdatedWithDB(oData)
            'Note: the ValidateData Function must throw a FaultException error on failure
            ValidateUpdatedRecord(LinqDB, oData)
            'Create New Record 
            Dim nObject = CopyDTOToLinq(oData)
            ' Attach the record 
            oLinqTable.Attach(nObject, True)
            Try
                LinqDB.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("Update"))
            End Try
            Dim source As LTS.BookItem = TryCast(nObject, LTS.BookItem)
            If source Is Nothing Then Return Nothing
            LastBookItemControl = source.BookItemControl
            Me.BookDependencyResult = DirectCast(Me.NDPBaseClassFactory("NGLBookData", False), NGLBookData).UpdateBookDependencies(0, source.BookItemBookLoadControl)
            Me.LastProcedureName = "spUpdateBookDependencies"
            'Return GetQuickSaveResults(nObject)
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' This override does not call the standard cleanup methods and returns nothing.
    ''' The caller must check the BookDependencyResult properties (ReturnBookControl,MustRecalculate,RetMsg, and ErrNumber) 
    ''' , recalculate costs 
    ''' </summary>
    ''' <typeparam name="TEntity"></typeparam>
    ''' <param name="oData"></param>
    ''' <param name="oLinqTable"></param>
    ''' <remarks></remarks>
    Public Overrides Sub UpdateNoReturn(Of TEntity As Class)(ByVal oData As Object,
                                                             ByVal oLinqTable As System.Data.Linq.Table(Of TEntity))
        LastBookItemControl = 0
        Using LinqDB

            'Note: the ValidateData Function must throw a FaultException error on failure
            ValidateUpdatedRecord(LinqDB, oData)
            'Create New Record 
            Dim nObject = CopyDTOToLinq(oData)
            ' Attach the record 
            oLinqTable.Attach(nObject, True)
            Try
                LinqDB.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateNoReturn"))
            End Try
            Dim source As LTS.BookItem = TryCast(nObject, LTS.BookItem)
            If source Is Nothing Then Return
            LastBookItemControl = source.BookItemControl
            Me.BookDependencyResult = DirectCast(Me.NDPBaseClassFactory("NGLBookData", False), NGLBookData).UpdateBookDependencies(0, source.BookItemBookLoadControl)
            Me.LastProcedureName = "spUpdateBookDependencies"
            Return
        End Using
    End Sub

    ''' <summary>
    ''' Save all changes in the Batch to the database and recalculate costs if needed, like when the pallet count changes.
    ''' </summary>
    ''' <param name="oRecords"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR v-7.0.5.102 8/16/2016
    '''   Checks all records in the batch for optimistic conncurrance before saving and recalculating costs
    ''' </remarks>
    Public Function UpdateBookItemBatch(ByVal oRecords As DataTransferObjects.BookItem()) As Boolean
        If oRecords Is Nothing OrElse oRecords.Count() < 1 Then Return False
        LastBookItemControl = 0
        Dim intBookItemBookLoadControl As Integer = 0
        Using db As New NGLMasBookDataContext(ConnectionString)
            For Each item In oRecords
                'Note: the ValidateData Function must throw a FaultException error on failure
                ValidateUpdatedRecord(db, item)
                'Create New Record 
                Dim nObject = CopyDTOToLinq(item)
                ' Attach the record 
                db.BookItems.Attach(nObject, True)
                LastBookItemControl = item.BookItemControl
                intBookItemBookLoadControl = item.BookItemBookLoadControl
            Next

            Try
                db.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateBookItemBatch"))
            End Try
            Me.LastProcedureName = "spUpdateBookDependencies"
            If intBookItemBookLoadControl = 0 Then Return True
            Me.BookDependencyResult = DirectCast(Me.NDPBaseClassFactory("NGLBookData", False), NGLBookData).UpdateBookDependencies(0, intBookItemBookLoadControl)

            Return True

        End Using
    End Function

#End Region

    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return GetBookItemFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetBookItemsFiltered()
    End Function

    Public Function GetBookItemFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.BookItem
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim BookItem As DataTransferObjects.BookItem = (
                        From d In db.BookItems
                        Where
                        (d.BookItemControl = If(Control = 0, d.BookItemControl, Control))
                        Order By d.BookItemControl Descending
                        Select selectDTOData(d, db)).First
                Return BookItem

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

    '***** CALLED BY GetAMSOrdersDetailByBookLoadControl() IN AMS.cs *****
    Public Function GetBookItemsFiltered(Optional ByVal BookLoadControl As Integer = 0) As DataTransferObjects.BookItem()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'db.Log = New DebugTextWriter
                'If(d.BookItemFixOffInvAllow.HasValue, d.BookItemFixOffInvAllow, 0)
                'Return all the contacts that match the criteria sorted by name
                Dim BookItems() As DataTransferObjects.BookItem = (
                        From d In db.BookItems
                        Where
                        (d.BookItemBookLoadControl = BookLoadControl)
                        Order By d.BookItemControl
                        Select selectDTOData(d, db)).ToArray()
                Return BookItems

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

    Public Function GetBookItemsFiltered(ByVal oBookLoadControls As List(Of Integer)) As List(Of BookItem)
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim BookItems As List(Of BookItem) = (
                        From d In db.BookItems
                        Where
                        (oBookLoadControls.Contains(d.BookItemBookLoadControl))
                        Order By d.BookItemControl
                        Select selectDTOData(d, db)).ToList()
                Return BookItems

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

    ''' <summary>
    ''' Get all the book items for the provided book control
    ''' </summary>
    ''' <param name="iBookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2.0.119 on 9/17/19
    '''   typically used to display data on the Load Board Items Page
    ''' </remarks>
    Public Function GetBookItemsByBookControl(ByVal iBookControl) As LTS.BookItem()
        Dim oRet As LTS.BookItem()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                If iBookControl > 0 Then
                    'get a list of bookload control numbers
                    Dim iBookLoadControls = db.BookLoads.Where(Function(x) x.BookLoadBookControl = iBookControl).Select(Function(y) y.BookLoadControl).ToArray()
                    If Not iBookLoadControls Is Nothing AndAlso iBookLoadControls.Count() > 0 Then
                        oRet = db.BookItems.Where(Function(x) iBookLoadControls.Contains(x.BookItemBookLoadControl)).ToArray()
                    End If
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetBookItemsByBookControl"), db)
            End Try

        End Using

        Return oRet

    End Function

    ''' <summary>
    ''' Provide one of the following: 
    ''' BookItemBookLoadContro in  Models.AllFilters.ParentControl, 
    ''' Models.AllFilters.BookControl, or 
    ''' Models.AllFilters.FilterValues.FilterName = BookItemControl
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2.0.119 on 09/18/2019
    '''   new all filter model for Load Board Item page and others
    ''' </remarks>
    Public Function GetBookItems(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.BookItem()
        Dim oRet As LTS.BookItem()
        If filters Is Nothing Then Return Nothing
        Dim filterWhere As String = ""
        Dim sFilterSpacer As String = ""
        Dim iBookItemControl As Integer = 0
        Dim iBookControl As Integer = 0
        Dim iBookLoadControl As Integer = 0

        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.BookItem)
                If Not filters.FilterValues.Any(Function(x) x.filterName = "BookItemControl") Then
                    'we need a BookItemControl fliter a parent control number or a Book Control number
                    Dim iBookLoadControls(1) As Integer
                    If filters.BookControl <> 0 Then
                        iBookControl = filters.BookControl
                        iBookLoadControls = db.BookLoads.Where(Function(x) x.BookLoadBookControl = iBookControl).Select(Function(y) y.BookLoadControl).ToArray()
                    Else
                        If filters.ParentControl = 0 Then
                            Dim sMsg As String = "E_MissingBookingParent" ' "  The reference to the parent booking record is missing. Please select a valid booking record from the load planning page and try again."
                            throwNoDataFaultException(sMsg)
                        Else
                            iBookLoadControls(0) = filters.ParentControl
                        End If
                    End If
                    If Not iBookLoadControls Is Nothing AndAlso iBookLoadControls.Count() > 0 Then
                        iQuery = db.BookItems.Where(Function(x) iBookLoadControls.Contains(x.BookItemBookLoadControl))
                    End If
                Else
                    Dim sBookItemControl = filters.FilterValues.Where(Function(x) x.filterName = "BookItemControl").Select(Function(y) y.filterValueFrom).FirstOrDefault()
                    Integer.TryParse(sBookItemControl, iBookItemControl)
                    If iBookItemControl > 0 Then
                        oRet = db.BookItems.Where(Function(x) x.BookItemControl = iBookItemControl).ToArray()
                    End If
                    Return oRet
                End If
                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "BookItemItemNumber"
                    filters.sortDirection = "asc"
                End If
                If Not iQuery Is Nothing Then


                    ApplyAllFilters(iQuery, filters, filterWhere)
                    PrepareQuery(iQuery, filters, RecordCount)
                    oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetBookItems"), db)
            End Try
        End Using

        Return oRet
    End Function

    Public Function SaveOrCreateBookItem(ByVal oData As LTS.BookItem, Optional ByVal iBookControl As Integer = 0) As Boolean
        Dim blnRet As Boolean = False
        If oData Is Nothing Then Return False 'nothing to do

        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim iBookLoadControl = oData.BookItemBookLoadControl
                If oData.BookItemBookLoadControl = 0 Then
                    If iBookControl = 0 Then
                        Dim sMsg As String = "E_MissingBookingParent" ' "  The reference to the parent booking record is missing. Please select a valid booking record from the load planning page and try again."
                        throwNoDataFaultException(sMsg)
                    End If
                    If Not db.BookLoads.Any(Function(x) x.BookLoadBookControl = iBookControl) Then
                        Dim oNewBookLoad = (From d In db.Books Where d.BookControl = iBookControl
                                Select New LTS.BookLoad With {
                                .BookLoadBookControl = iBookControl,
                                .BookLoadPONumber = d.BookCarrOrderNumber,
                                .BookLoadCaseQty = d.BookTotalCases,
                                .BookLoadWgt = d.BookTotalWgt,
                                .BookLoadCube = d.BookTotalCube,
                                .BookLoadPL = d.BookTotalPL,
                                .BookLoadPX = d.BookTotalPX,
                                .BookLoadPType = "N",
                                .BookLoadCom = "D",
                                .BookLoadModDate = Date.Now(),
                                .BookLoadModUser = "System"}).FirstOrDefault()
                        db.BookLoads.InsertOnSubmit(oNewBookLoad)
                        db.SubmitChanges()
                        oData.BookItemBookLoadControl = oNewBookLoad.BookLoadControl
                    Else
                        oData.BookItemBookLoadControl = db.BookLoads.Where(Function(x) x.BookLoadBookControl = iBookControl).Select(Function(y) y.BookLoadControl).FirstOrDefault()
                    End If
                End If

                '    Dim blnProcessed As Boolean = False
                oData.BookItemModDate = Date.Now()
                oData.BookItemModUser = Me.Parameters.UserName

                If oData.BookItemControl = 0 Then
                    db.BookItems.InsertOnSubmit(oData)
                Else
                    db.BookItems.Attach(oData, True)
                End If
                db.SubmitChanges()
                Try
                    'this will update all dependent data an recalculate booking total cases, weight, pallets and cubes
                    Me.BookDependencyResult = DirectCast(Me.NDPBaseClassFactory("NGLBookData", False), NGLBookData).UpdateBookDependencies(0, oData.BookItemBookLoadControl)
                Catch ex As Exception
                    'do nothing here
                End Try
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveOrCreateBookItem"), db)
            End Try
        End Using
        Return blnRet
    End Function

    Public Function DeleteBookItem(ByVal iBookItemControl As Integer) As Boolean
        Dim blnRet As Boolean = False

        If iBookItemControl = 0 Then Return False 'nothing to do
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'verify the service contract
                Dim oExisting = db.BookItems.Where(Function(x) x.BookItemControl = iBookItemControl).FirstOrDefault()
                If oExisting Is Nothing OrElse oExisting.BookItemControl = 0 Then Return True
                Dim iBookItemBookLoadControl = oExisting.BookItemBookLoadControl
                db.BookItems.DeleteOnSubmit(oExisting)
                db.SubmitChanges()
                Try
                    'this will update all dependent data an recalculate booking total cases, weight, pallets and cubes
                    Me.BookDependencyResult = DirectCast(Me.NDPBaseClassFactory("NGLBookData", False), NGLBookData).UpdateBookDependencies(0, iBookItemBookLoadControl)
                Catch ex As Exception
                    'do nothing here
                End Try
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteBookItem"), db)
            End Try
        End Using
        Return blnRet
    End Function

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DataTransferObjects.BookItem)
        Return selectLTSData(d, Me.Parameters.UserName)

    End Function

    Protected Function CompareUpdatedWithDB(ByVal oData As DataTransferObjects.BookItem) As Boolean
        Dim DBBookItemUpdated As Byte()
        Dim db As NGLMasBookDataContext = CType(Me.LinqDB, NGLMasBookDataContext)
        Dim intControl As Integer = oData.BookItemControl
        DBBookItemUpdated = (From d In db.BookItems Where d.BookItemControl = intControl Select d.BookItemUpdated).FirstOrDefault().ToArray()
        Return StructuralComparisons.StructuralEqualityComparer.Equals(DBBookItemUpdated, oData.BookItemUpdated)
    End Function


    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Dim oData As LTS.BookItem = TryCast(LinqTable, LTS.BookItem)
        If oData Is Nothing Then Return Nothing
        Return GetBookItemFiltered(Control:=oData.BookItemControl)
    End Function

    Public Function QuickSaveResults(ByVal BookItemControl As Integer) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                ret = (From d In db.BookItems
                    Where d.BookItemControl = BookItemControl
                    Select New DataTransferObjects.QuickSaveResults With {.Control = d.BookItemControl _
                        , .ModDate = d.BookItemModDate _
                        , .ModUser = d.BookItemModUser _
                        , .Updated = d.BookItemUpdated.ToArray}).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("QuickSaveResults"))
            End Try
        End Using
        Return ret
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim source As LTS.BookItem = TryCast(LinqTable, LTS.BookItem)
        If source Is Nothing Then Return Nothing
        Return QuickSaveResults(source.BookItemControl)
    End Function

    Friend Shared Function selectDTOData(ByVal d As LTS.BookItem, ByRef db As NGLMasBookDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.BookItem

        Dim oDTO As New DataTransferObjects.BookItem

        Dim skipObjs As New List(Of String) From {"BookItemUpdated",
                "Page",
                "Pages",
                "RecordCount",
                "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .BookItemUpdated = d.BookItemUpdated.ToArray()
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oDTO


    End Function

    ''' <summary>
    ''' Typically used when we want to insert a new LTS object in the DB
    ''' </summary>
    ''' <param name="d"></param>
    ''' <param name="UserName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function selectLTSData(ByVal d As DataTransferObjects.BookItem, ByVal UserName As String) As LTS.BookItem
        ' If Updated is null, set to byte[0] (for inserted items).
        Dim oLTS As New LTS.BookItem
        UpdateLTSWithDTO(d, oLTS, UserName)
        Return oLTS

    End Function

    ''' <summary>
    ''' Typically used to update an existing LTS object.  If the caller sends an attached LTS object when the isLTSAttached = false 
    ''' the save will fail and an exception will be thrown when SubmitChanges is called
    ''' </summary>
    ''' <param name="d"></param>
    ''' <param name="t"></param>
    ''' <param name="UserName"></param>
    ''' <param name="isLTSAttached"></param>
    ''' <remarks>
    ''' Note: this method depends on the caller to use an existing LTS object or to Attach a new LTS object to the data context
    ''' The main difference has to do with BookItemUpdated used for optimistic concurrency checks.
    ''' We can only use the existing BookItemUpdated values before the LTS object has been attached
    ''' The default value for isLTSAttached is False. If the caller sends an attached LTS object when this flag false 
    ''' the save will fail and an exception will be thrown when SubmitChanges is called
    ''' </remarks>
    Friend Shared Sub UpdateLTSWithDTO(ByRef d As DataTransferObjects.BookItem, ByRef t As LTS.BookItem, ByVal UserName As String, Optional ByVal isLTSAttached As Boolean = False)
        Dim blnNewLTS As Boolean = False 'used to determine if we allow the BookItemUpdated to be set,  existing LTS objects cannot update the BookItemUpdated value.
        If t.BookItemControl = 0 Then blnNewLTS = True 'in this case we use a new Byte or the current value in d
        Dim strMSG As String = ""
        Dim skipObjs As New List(Of String) From {"BookItemModDate", "BookItemModUser", "BookItemUpdated", "rowguid"}
        t = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(t, d, skipObjs, strMSG)
        't = CopyMatchingFields(t, d, skipObjs)
        With t
            .BookItemModDate = Date.Now
            .BookItemModUser = UserName
            If (Not isLTSAttached) AndAlso blnNewLTS Then .BookItemUpdated = If(d.BookItemUpdated Is Nothing, New Byte() {}, d.BookItemUpdated)
        End With
        If Debugger.IsAttached AndAlso Not String.IsNullOrWhiteSpace(strMSG) Then
            System.Diagnostics.Debug.WriteLine(strMSG)
        End If
    End Sub

    Friend Shared Function GetBookItemChanges(ByVal source As DataTransferObjects.BookLoad, ByVal changeType As TrackingInfo, ByVal UserName As String) As List(Of LTS.BookItem)
        If source Is Nothing OrElse source.BookItems Is Nothing OrElse source.BookItems.Count < 1 Then Return New List(Of LTS.BookItem)

        ' Test record details for specified change type.
        Dim details As IEnumerable(Of LTS.BookItem) = (
                From d In source.BookItems
                Where d.TrackingState = changeType
                Select NGLBookItemData.selectLTSData(d, UserName))
        Return details.ToList()

    End Function


#End Region

End Class