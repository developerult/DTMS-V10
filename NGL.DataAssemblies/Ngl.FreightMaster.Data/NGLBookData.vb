Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.ServiceModel
Imports System.Text.Json
Imports Ngl.Core.ChangeTracker
Imports Ngl.Core.Utility
Imports Ngl.FreightMaster.Data.DataTransferObjects
Imports Serilog.Events
Imports SerilogTracing

Public Class NGLBookData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        'Dim db As New NGLMasBookDataContext(ConnectionString)
        'Me.LinqTable = db.Books
        'Me.LinqDB = db
        Me.SourceClass = "NGLBookData"
    End Sub

#End Region

#Region " Properties "
    Private _RecalcTotals As Boolean = False
    Private _ItemsChanged As Boolean = False

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMasBookDataContext(ConnectionString)
                _LinqTable = db.Books
                _LinqDB = db
            End If

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

    Private _UpdateBookingRecordResult As LTS.spUpdateBookingRecordResult
    Public Property UpdateBookingRecordResult() As LTS.spUpdateBookingRecordResult
        Get
            Return _UpdateBookingRecordResult
        End Get
        Set(ByVal value As LTS.spUpdateBookingRecordResult)
            _UpdateBookingRecordResult = value
        End Set
    End Property

    Private _WriteNewBookingForBatchResult As LTS.spWriteNewBookingForBatchResult
    Public Property WriteNewBookingForBatchResult() As LTS.spWriteNewBookingForBatchResult
        Get
            Return _WriteNewBookingForBatchResult
        End Get
        Set(ByVal value As LTS.spWriteNewBookingForBatchResult)
            _WriteNewBookingForBatchResult = value
        End Set
    End Property

    Private _SilentTenderFinalizedResult As LTS.spSilentTenderFinalizedResult
    Public Property SilentTenderFinalizedResult() As LTS.spSilentTenderFinalizedResult
        Get
            Return _SilentTenderFinalizedResult
        End Get
        Set(ByVal value As LTS.spSilentTenderFinalizedResult)
            _SilentTenderFinalizedResult = value
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



#End Region


#Region "Public Methods"


#Region " Overridden data methods"

    ''' <summary>
    ''' This override does not call the standard cleanup methods and returns nothing.
    ''' The caller must check the object properties (ReturnBookControl,MustRecalculate,RetMsg, and ErrNumber) 
    ''' , recalculate costs and return the updated booking record
    ''' </summary>
    ''' <typeparam name="TEntity"></typeparam>
    ''' <param name="oData"></param>
    ''' <param name="oLinqTable"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function Add(Of TEntity As Class)(ByVal oData As DataTransferObjects.DTOBaseClass,
                                                       ByVal oLinqTable As System.Data.Linq.Table(Of TEntity)) As DataTransferObjects.DTOBaseClass
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
            Dim source As LTS.Book = TryCast(nObject, LTS.Book)
            If source Is Nothing Then Return Nothing
            UpdateBookDependencies(source.BookControl, 0)
            Return Nothing
        End Using

    End Function

    ''' <summary>
    ''' This override does not call the standard cleanup methods and returns nothing.
    ''' The caller must check the object properties (ReturnBookControl,MustRecalculate,RetMsg, and ErrNumber) 
    ''' , recalculate costs and return the updated booking record
    ''' </summary>
    ''' <typeparam name="TEntity"></typeparam>
    ''' <param name="oData"></param>
    ''' <param name="oLinqTable"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function AddWithDetails(Of TEntity As Class)(ByVal oData As DataTransferObjects.DTOBaseClass,
                                                                  ByVal oLinqTable As System.Data.Linq.Table(Of TEntity)) As DataTransferObjects.DTOBaseClass
        Using LinqDB
            'Note: the ValidateData Function must throw a FaultException error on failure
            ValidateNewRecord(LinqDB, oData)
            'Create New Record 
            Dim nObject = CopyDTOToLinq(oData)
            AddDetailsToLinq(nObject, oData)
            oLinqTable.InsertOnSubmit(nObject)
            InsertAllDetails(LinqDB, nObject)
            Try
                LinqDB.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("AddWithDetails"))
            End Try
            Dim source As LTS.Book = TryCast(nObject, LTS.Book)
            If source Is Nothing Then Return Nothing
            UpdateBookDependencies(source.BookControl, 0)
            Return Nothing
        End Using

    End Function

    ''' <summary>
    ''' Updates the BookWaitingToProcessNewBooking value for the provided BookControl the 
    ''' default value for the optional parameter BookWaitingToProcessNewBooking is 1;  
    ''' this flags the booking record as processed for auto import, routing and carrier assignment
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="BookWaitingToProcessNewBooking"></param>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.110 9/27/16
    '''   added new method to update the BookWaitingToProcessNewBooking value for the provided BookControl
    ''' </remarks>
    Public Sub UpdateBookWaitingToProcessNewBooking(ByVal BookControl As Integer, Optional ByVal BookWaitingToProcessNewBooking As Integer = 0)
        If BookControl = 0 Then Return 'nothing to do
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim obook = db.Books.Where(Function(x) x.BookControl = BookControl).FirstOrDefault()
                If Not obook Is Nothing AndAlso obook.BookControl = BookControl Then
                    obook.BookWaitingToProcessNewBooking = BookWaitingToProcessNewBooking
                    db.SubmitChanges()
                End If

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateBookWaitingToProcessNewBooking"))
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' This override does not call the standard cleanup methods and returns nothing.
    ''' The caller must check the object properties (ReturnBookControl,MustRecalculate,RetMsg, and ErrNumber) 
    ''' , recalculate costs and return the updated booking record
    ''' </summary>
    ''' <typeparam name="TEntity"></typeparam>
    ''' <param name="oData"></param>
    ''' <param name="oLinqTable"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function Update(Of TEntity As Class)(ByVal oData As Object,
                                                          ByVal oLinqTable As System.Data.Linq.Table(Of TEntity)) As Object
        Using LinqDB
            SaveLinqData(oData, oLinqTable)
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' This override does not call the standard cleanup methods and returns nothing.
    ''' The caller must check the object properties (ReturnBookControl,MustRecalculate,RetMsg, and ErrNumber) 
    ''' , recalculate costs and return the updated quick return object
    ''' </summary>
    ''' <typeparam name="TEntity"></typeparam>
    ''' <param name="oData"></param>
    ''' <param name="oLinqTable"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function UpdateQuick(Of TEntity As Class)(ByVal oData As Object,
                                                               ByVal oLinqTable As System.Data.Linq.Table(Of TEntity)) As DataTransferObjects.QuickSaveResults
        Using LinqDB
            SaveLinqData(oData, oLinqTable)
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' This override does not call the standard cleanup methods and returns nothing.
    ''' The caller must check the object properties (ReturnBookControl,MustRecalculate,RetMsg, and ErrNumber) 
    ''' , recalculate costs 
    ''' </summary>
    ''' <typeparam name="TEntity"></typeparam>
    ''' <param name="oData"></param>
    ''' <param name="oLinqTable"></param>
    ''' <remarks></remarks>
    Public Overrides Sub UpdateNoReturn(Of TEntity As Class)(ByVal oData As Object,
                                                             ByVal oLinqTable As System.Data.Linq.Table(Of TEntity))
        Using LinqDB
            SaveLinqData(oData, oLinqTable)
        End Using
    End Sub

    ''' <summary>
    ''' This override does not call the standard cleanup methods and returns nothing.
    ''' The caller must check the object properties (ReturnBookControl,MustRecalculate,RetMsg, and ErrNumber) 
    ''' , recalculate costs 
    ''' </summary>
    ''' <typeparam name="TEntity"></typeparam>
    ''' <param name="oData"></param>
    ''' <param name="oLinqTable"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function UpdateWithDetails(Of TEntity As Class)(ByVal oData As Object,
                                                                     ByVal oLinqTable As System.Data.Linq.Table(Of TEntity)) As Object
        Using LinqDB
            Dim nObject As Object
            Try
                'Note: the ValidateData Function must throw a FaultException error on failure
                ValidateUpdatedRecord(LinqDB, oData)
                'Create New Record 
                nObject = CopyDTOToLinq(oData)
                ' Attach the record 
                oLinqTable.Attach(nObject, True)
                ProcessUpdatedDetails(LinqDB, oData)
                LinqDB.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateWithDetails"))
            End Try
            Dim source As LTS.Book = TryCast(nObject, LTS.Book) 'returns nothing if cast fails
            If Not source Is Nothing Then UpdateBookDependencies(source.BookControl, 0)
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' This override does not call the standard cleanup methods and returns nothing.
    ''' The caller must check the object properties (ReturnBookControl,MustRecalculate,RetMsg, and ErrNumber) 
    ''' , recalculate costs and return the updated booking record
    ''' </summary>
    ''' <typeparam name="TEntity"></typeparam>
    ''' <param name="oData"></param>
    ''' <param name="oLinqTable"></param>
    ''' <remarks></remarks>
    Public Overrides Sub UpdateWithDetailsNoReturn(Of TEntity As Class)(ByVal oData As Object,
                                                                        ByVal oLinqTable As System.Data.Linq.Table(Of TEntity))
        Using LinqDB
            Dim nObject As Object
            Try
                'Note: the ValidateData Function must throw a FaultException error on failure
                ValidateUpdatedRecord(LinqDB, oData)
                'Create New Record 
                nObject = CopyDTOToLinq(oData)
                ' Attach the record 
                oLinqTable.Attach(nObject, True)
                ProcessUpdatedDetails(LinqDB, oData)
                LinqDB.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateWithDetailsNoReturn"))
            End Try
            Dim source As LTS.Book = TryCast(nObject, LTS.Book) 'returns nothing if cast fails
            If Not source Is Nothing Then UpdateBookDependencies(source.BookControl, 0)
        End Using
    End Sub

#End Region

#Region "     Standard Book CRUD Methods"

    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return GetBookFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetBooksFiltered()
    End Function

    Public Function GetBooksByCarrier(ByVal CarrierControl As Integer) As DataTransferObjects.Book()
        If CarrierControl = 0 Then Return Nothing
        Return GetBooksFiltered(CarrierControl:=CarrierControl)
    End Function

    Public Function GetBooksByComp(ByVal CompControl As Integer) As DataTransferObjects.Book()
        If CompControl = 0 Then Return Nothing
        Return GetBooksFiltered(CompControl:=CompControl)
    End Function

    Public Function GetBooksByLane(ByVal LaneControl As Integer) As DataTransferObjects.Book()
        If LaneControl = 0 Then Return Nothing
        Return GetBooksFiltered(LaneControl:=LaneControl)
    End Function

    Public Function GetBookFiltered(Optional ByVal Control As Integer = 0,
                                    Optional ByVal BookProNumber As String = "",
                                    Optional ByVal BookConsPrefix As String = "",
                                    Optional ByVal BookLoadPONumber As String = "",
                                    Optional ByVal BookCarrBLNumber As String = "",
                                    Optional ByVal BookFinAPBillNumber As String = "",
                                    Optional ByVal BookCarrOrderNumber As String = "",
                                    Optional ByVal BookOrderSequence As Integer = 0) As DataTransferObjects.Book
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'db.Log = New DebugTextWriter

                Dim oDLO As New DataLoadOptions
                oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookLoads)
                oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookNotes)
                oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookTracks)
                oDLO.LoadWith(Of LTS.BookLoad)(Function(t As LTS.BookLoad) t.BookItems)
                db.LoadOptions = oDLO
                'Get the newest record that matches the provided criteria                
                Dim Book As DataTransferObjects.Book = (
                        From d In db.Books
                        Where
                        (d.BookControl = If(Control = 0, d.BookControl, Control)) _
                        And
                        (BookProNumber Is Nothing OrElse String.IsNullOrEmpty(BookProNumber) OrElse d.BookProNumber = BookProNumber) _
                        And
                        (BookConsPrefix Is Nothing OrElse String.IsNullOrEmpty(BookConsPrefix) OrElse d.BookConsPrefix = BookConsPrefix) _
                        And
                        (BookCarrBLNumber Is Nothing OrElse String.IsNullOrEmpty(BookCarrBLNumber) OrElse d.BookCarrBLNumber = BookCarrBLNumber) _
                        And
                        (BookFinAPBillNumber Is Nothing OrElse String.IsNullOrEmpty(BookFinAPBillNumber) OrElse d.BookFinAPBillNumber = BookFinAPBillNumber) _
                        And
                        (BookCarrOrderNumber Is Nothing OrElse String.IsNullOrEmpty(BookCarrOrderNumber) OrElse d.BookCarrOrderNumber = BookCarrOrderNumber) _
                        And
                        (BookOrderSequence = 0 OrElse d.BookOrderSequence = BookOrderSequence)
                        Order By d.BookStopNo Ascending
                        Select selectDTODataWDetails(d)).FirstOrDefault()

                Return Book
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetBookFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetBookFilteredNoChildren(ByVal Control As Integer) As DataTransferObjects.Book
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria 
                Dim Book As DataTransferObjects.Book = (
                        From d In db.Books
                        Where
                        (d.BookControl = Control)
                        Select SelectDTOData(d)).FirstOrDefault()
                Return Book

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

    Public Function GetBooksFiltered(Optional ByVal BookConsPrefix As String = "",
                                     Optional ByVal BookCarrBLNumber As String = "",
                                     Optional ByVal BookFinAPBillNumber As String = "",
                                     Optional ByVal BookCarrOrderNumber As String = "",
                                     Optional ByVal CompControl As Integer = 0,
                                     Optional ByVal LaneControl As Integer = 0,
                                     Optional ByVal CarrierControl As Integer = 0) As DataTransferObjects.Book()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'db.Log = New DebugTextWriter

                'Test code for SQL Log
                'Dim oBooks() As DTO.Book = (From d In db.Books _
                'Where _
                '    (BookConsPrefix Is Nothing OrElse String.IsNullOrEmpty(BookConsPrefix) OrElse d.BookConsPrefix = BookConsPrefix) _
                '    And _
                '    (BookCarrBLNumber Is Nothing OrElse String.IsNullOrEmpty(BookCarrBLNumber) OrElse d.BookCarrBLNumber = BookCarrBLNumber) _
                '    And _
                '    (BookFinAPBillNumber Is Nothing OrElse String.IsNullOrEmpty(BookFinAPBillNumber) OrElse d.BookFinAPBillNumber = BookFinAPBillNumber) _
                '    And _
                '    (BookCarrOrderNumber Is Nothing OrElse String.IsNullOrEmpty(BookCarrOrderNumber) OrElse d.BookCarrOrderNumber = BookCarrOrderNumber) _
                '    And _
                '    (CompControl = 0 OrElse d.BookCustCompControl = CompControl) _
                '    And _
                '    (LaneControl = 0 OrElse d.BookODControl = LaneControl) _
                '    And _
                '    (CarrierControl = 0 OrElse d.BookCarrierControl = CarrierControl) _
                'Order By d.BookStopNo Ascending _
                'Select New DTO.Book With {.BookControl = d.BookControl, _
                '                          .BookProNumber = d.BookProNumber, _
                '                          .BookOrigCompControl = If(d.BookOrigCompControl.HasValue, d.BookOrigCompControl, 0) _
                '                         }).Take(10).ToArray()



                Dim oDLO As New DataLoadOptions
                oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookLoads)
                oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookNotes)
                oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookTracks)
                oDLO.LoadWith(Of LTS.BookLoad)(Function(t As LTS.BookLoad) t.BookItems)
                db.LoadOptions = oDLO
                'Get the newest record that matches the provided criteria
                Dim Books() As DataTransferObjects.Book = (
                        From d In db.Books
                        Where
                        (BookConsPrefix Is Nothing OrElse String.IsNullOrEmpty(BookConsPrefix) OrElse d.BookConsPrefix = BookConsPrefix) _
                        And
                        (BookCarrBLNumber Is Nothing OrElse String.IsNullOrEmpty(BookCarrBLNumber) OrElse d.BookCarrBLNumber = BookCarrBLNumber) _
                        And
                        (BookFinAPBillNumber Is Nothing OrElse String.IsNullOrEmpty(BookFinAPBillNumber) OrElse d.BookFinAPBillNumber = BookFinAPBillNumber) _
                        And
                        (BookCarrOrderNumber Is Nothing OrElse String.IsNullOrEmpty(BookCarrOrderNumber) OrElse d.BookCarrOrderNumber = BookCarrOrderNumber) _
                        And
                        (CompControl = 0 OrElse d.BookCustCompControl = CompControl) _
                        And
                        (d.BookODControl = If(LaneControl = 0, d.BookODControl, LaneControl)) _
                        And
                        (d.BookCarrierControl = If(CarrierControl = 0, d.BookCarrierControl, CarrierControl))
                        Order By d.BookStopNo Ascending
                        Select selectDTODataWDetails(d)).Take(20).ToArray()

                Return Books

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
    ''' return an array of book data,  the BookConsPrefix represents the BookSHID first 
    ''' then the BookConsPrefix second if a matching BookSHID is not found
    ''' </summary>
    ''' <param name="BookConsPrefix"></param>
    ''' <param name="BookCarrBLNumber"></param>
    ''' <param name="BookFinAPBillNumber"></param>
    ''' <param name="BookCarrOrderNumber"></param>
    ''' <param name="CompControl"></param>
    ''' <param name="LaneControl"></param>
    ''' <param name="CarrierControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    '''  Modified by RHR for v-8.2.1 on 10/11/2019
    '''   The parameter BookConsPrefix is now used to support  BookConsPrefix Or BookSHID
    '''   new functionality exists where the SHID can be  the carrier pro number.  
    '''   The function had been modified to first look up using 
    '''   SHID and second to lookup using CNS if the SHID does not match.
    ''' </remarks>
    Public Function GetBooksFilteredNoChildren(Optional ByVal BookConsPrefix As String = "",
                                               Optional ByVal BookCarrBLNumber As String = "",
                                               Optional ByVal BookFinAPBillNumber As String = "",
                                               Optional ByVal BookCarrOrderNumber As String = "",
                                               Optional ByVal CompControl As Integer = 0,
                                               Optional ByVal LaneControl As Integer = 0,
                                               Optional ByVal CarrierControl As Integer = 0) As DataTransferObjects.Book()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try

                'Dim oDLO As New DataLoadOptions
                'oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookLoads)
                'oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookNotes)
                'oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookTracks)
                'oDLO.LoadWith(Of LTS.BookLoad)(Function(t As LTS.BookLoad) t.BookItems)
                'db.LoadOptions = oDLO
                'Get the newest record that matches the provided criteria
                'Modified by RHR for v-8.2.1 on 10/11/2019
                'check if we are using the SHID or the CNS 
                Dim blnUseSHID As Boolean = True
                If Not String.IsNullOrWhiteSpace(BookConsPrefix) Then
                    If Not db.Books.Any(Function(x) x.BookSHID = BookConsPrefix) Then blnUseSHID = False
                End If
                Dim Books() As DataTransferObjects.Book = (
                        From d In db.Books
                        Where
                        (BookConsPrefix Is Nothing OrElse String.IsNullOrEmpty(BookConsPrefix) OrElse ((blnUseSHID = True And d.BookSHID = BookConsPrefix) Or (d.BookConsPrefix = BookConsPrefix))) _
                        And
                        (BookCarrBLNumber Is Nothing OrElse String.IsNullOrEmpty(BookCarrBLNumber) OrElse d.BookCarrBLNumber = BookCarrBLNumber) _
                        And
                        (BookFinAPBillNumber Is Nothing OrElse String.IsNullOrEmpty(BookFinAPBillNumber) OrElse d.BookFinAPBillNumber = BookFinAPBillNumber) _
                        And
                        (BookCarrOrderNumber Is Nothing OrElse String.IsNullOrEmpty(BookCarrOrderNumber) OrElse d.BookCarrOrderNumber = BookCarrOrderNumber) _
                        And
                        (CompControl = 0 OrElse d.BookCustCompControl = CompControl) _
                        And
                        (d.BookODControl = If(LaneControl = 0, d.BookODControl, LaneControl)) _
                        And
                        (d.BookCarrierControl = If(CarrierControl = 0, d.BookCarrierControl, CarrierControl))
                        Order By d.BookStopNo Ascending
                        Select SelectDTOData(d)).Take(20).ToArray()

                Return Books

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


    Public Function GetBooksBySHID(ByVal BookSHID As String) As DataTransferObjects.Book()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try

                If String.IsNullOrWhiteSpace(BookSHID) Then Return Nothing

                'Get the newest record that matches the provided criteria
                Dim Books() As DataTransferObjects.Book = (
                        From d In db.Books
                        Where
                        (BookSHID = d.BookSHID)
                        Order By d.BookStopNo Ascending
                        Select SelectDTOData(d)).ToArray()

                Return Books

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetBooksBySHID"))
            End Try

            Return Nothing

        End Using
    End Function


    Public Function GetFirstBookControlBySHID(ByVal BookSHID As String) As Integer
        Dim iBookControl As Integer = 0
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                If String.IsNullOrWhiteSpace(BookSHID) Then Return iBookControl
                iBookControl = db.Books.Where(Function(x) x.BookSHID = BookSHID).OrderBy(Function(x) x.BookStopNo).Select(Function(x) x.BookControl).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetFirstBookControlBySHID"))
            End Try
        End Using
        Return iBookControl
    End Function

    Public Function GetCompanyNameNumberByBookControl(ByVal iBookControl As Integer) As Dictionary(Of String, String)
        Dim dictRet As New Dictionary(Of String, String)
        If iBookControl = 0 Then Return dictRet 'return an empty dictionary not valid
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim iCompControl As Integer = db.Books.Where(Function(x) x.BookControl = iBookControl).Select(Function(x) x.BookCustCompControl).FirstOrDefault()
                If iCompControl = 0 Then Return dictRet
                Dim oCompData = db.CompRefBooks.Where(Function(x) x.CompControl = iCompControl).FirstOrDefault()
                If Not oCompData Is Nothing AndAlso oCompData.CompControl <> 0 Then
                    dictRet.Add("CompName", oCompData.CompName)
                    dictRet.Add("CompNumber", oCompData.CompNumber.ToString())
                    dictRet.Add("CompControl", oCompData.CompControl.ToString())
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCompanyNameNumberByBookControl"))
            End Try
        End Using
        Return dictRet
    End Function

    Public Function GetBooksByPONumber(ByVal BookLoadPONumber As String) As DataTransferObjects.Book()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try

                If BookLoadPONumber Is Nothing OrElse String.IsNullOrEmpty(BookLoadPONumber) Then Return Nothing
                Dim oBLs = From bl In db.BookLoads Where bl.BookLoadPONumber = BookLoadPONumber Select bl.BookLoadBookControl

                Dim oDLO As New DataLoadOptions
                oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookLoads)
                oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookNotes)
                oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookTracks)
                oDLO.LoadWith(Of LTS.BookLoad)(Function(t As LTS.BookLoad) t.BookItems)
                db.LoadOptions = oDLO
                'Get the newest record that matches the provided criteria
                Dim Books() As DataTransferObjects.Book = (
                        From d In db.Books
                        Where oBLs.Contains(d.BookControl)
                        Order By d.BookStopNo Ascending
                        Select selectDTODataWDetails(d)).Take(20).ToArray()

                Return Books

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

    Public Function GetBooksFilteredContains(Optional ByVal BookProNumber As String = "",
                                             Optional ByVal BookConsPrefix As String = "",
                                             Optional ByVal BookCarrBLNumber As String = "",
                                             Optional ByVal BookFinAPBillNumber As String = "",
                                             Optional ByVal BookCarrOrderNumber As String = "") As DataTransferObjects.Book()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oSecureComp = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl

                Dim oDLO As New DataLoadOptions
                oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookLoads)
                oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookNotes)
                oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookTracks)
                oDLO.LoadWith(Of LTS.BookLoad)(Function(t As LTS.BookLoad) t.BookItems)
                db.LoadOptions = oDLO
                'Get the newest record that matches the provided criteria
                Dim Books() As DataTransferObjects.Book = (
                        From d In db.Books
                        Where
                        (BookProNumber Is Nothing OrElse String.IsNullOrEmpty(BookProNumber) OrElse d.BookProNumber.Contains(BookProNumber)) _
                        And
                        (BookConsPrefix Is Nothing OrElse String.IsNullOrEmpty(BookConsPrefix) OrElse d.BookConsPrefix.Contains(BookConsPrefix)) _
                        And
                        (BookCarrBLNumber Is Nothing OrElse String.IsNullOrEmpty(BookCarrBLNumber) OrElse d.BookCarrBLNumber.Contains(BookCarrBLNumber)) _
                        And
                        (BookFinAPBillNumber Is Nothing OrElse String.IsNullOrEmpty(BookFinAPBillNumber) OrElse d.BookFinAPBillNumber.Contains(BookFinAPBillNumber)) _
                        And
                        (BookCarrOrderNumber Is Nothing OrElse String.IsNullOrEmpty(BookCarrOrderNumber) OrElse d.BookCarrOrderNumber.Contains(BookCarrOrderNumber)) _
                        And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.BookCustCompControl))
                        Order By d.BookStopNo Ascending
                        Select selectDTODataWDetails(d)).Take(20).ToArray()

                Return Books

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

    Public Function GetBooksByPONumberContains(ByVal BookLoadPONumber As String) As DataTransferObjects.Book()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oSecureComp = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl

                If BookLoadPONumber Is Nothing OrElse String.IsNullOrEmpty(BookLoadPONumber) Then Return Nothing
                Dim oBLs = From bl In db.BookLoads Where bl.BookLoadPONumber.Contains(BookLoadPONumber) Select bl.BookLoadBookControl

                Dim oDLO As New DataLoadOptions
                oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookLoads)
                oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookNotes)
                oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookTracks)
                oDLO.LoadWith(Of LTS.BookLoad)(Function(t As LTS.BookLoad) t.BookItems)
                db.LoadOptions = oDLO
                'Get the newest record that matches the provided criteria
                Dim Books() As DataTransferObjects.Book = (
                        From d In db.Books
                        Where oBLs.Contains(d.BookControl) _
                              And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.BookCustCompControl))
                        Order By d.BookStopNo Ascending
                        Select selectDTODataWDetails(d)).Take(20).ToArray()

                Return Books

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

    Public Function GetBooksByAppointment(ByVal AMSApptControl As Integer) As DataTransferObjects.Book()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try

                If AMSApptControl = 0 Then Return Nothing

                Dim oDLO As New DataLoadOptions
                oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookLoads)
                db.LoadOptions = oDLO

                Dim Books() As DataTransferObjects.Book = (
                        From d In db.Books
                        Where
                        (d.BookAMSPickupApptControl = AMSApptControl Or d.BookAMSDeliveryApptControl = AMSApptControl)
                        Select selectDTODataWBookLoads(d)).ToArray()

                Return Books

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

    Public Function GetBookAPCheckEntryRecords(ByVal CarrierControl As Integer, ByVal DateStart As Nullable(Of Date), ByVal DateEnd As Nullable(Of Date)) As DataTransferObjects.Book()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oSecureComp = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl

                If DateStart.HasValue Then
                    If Not DateEnd.HasValue Then
                        DateEnd = Date.MaxValue
                    End If
                    DateStart = DataTransformation.formatStartDateFilter(DateStart)
                    If Not DateEnd.Value = Date.MaxValue Then
                        DateEnd = DataTransformation.formatEndDateFilter(DateEnd)
                    End If

                    'Get the AP Check Entry Book Records
                    Dim Books() As DataTransferObjects.Book = (
                            From d In db.Books
                            Where
                            (d.BookPayCode = "P") _
                            And
                            (d.BookCarrierControl = CarrierControl) _
                            And
                            (d.BookDateLoad >= DateStart And d.BookDateLoad <= DateEnd) _
                            And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.BookCustCompControl))
                            Order By d.BookProNumber Ascending
                            Select SelectDTOData(d)).ToArray()

                    Return Books
                Else
                    'Get the AP Check Entry Book Records
                    Dim Books() As DataTransferObjects.Book = (
                            From d In db.Books
                            Where
                            (d.BookPayCode = "P") _
                            And
                            (d.BookCarrierControl = CarrierControl) _
                            And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.BookCustCompControl))
                            Order By d.BookProNumber Ascending
                            Select SelectDTOData(d)).ToArray()

                    Return Books
                End If

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

    Public Function GetBookAPDetailRecords(ByVal CarrierControl As Integer, ByVal BookPayCode As String) As DataTransferObjects.BookAPDetail()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oSecureComp = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl

                'Get the AP Detail Book Records
                Dim BookAPdetails() As DataTransferObjects.BookAPDetail = (
                        From d In db.Books
                        Where
                        (d.BookPayCode = BookPayCode) _
                        And
                        (d.BookCarrierControl = CarrierControl) _
                        And
                        (d.BookFinAPBillNumber.Length > 0) _
                        And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.BookCustCompControl))
                        Order By d.BookDateLoad, d.BookConsPrefix, d.BookProNumber Ascending
                        Select New DataTransferObjects.BookAPDetail With {.BookControl = d.BookControl _
                        , .BookProNumber = d.BookProNumber _
                        , .BookConsPrefix = d.BookConsPrefix _
                        , .BookCarrierControl = d.BookCarrierControl _
                        , .BookDateLoad = d.BookDateLoad _
                        , .BookTranCode = d.BookTranCode _
                        , .BookPayCode = d.BookPayCode _
                        , .BookCarrBLNumber = d.BookCarrBLNumber _
                        , .BookFinAPBillNumber = d.BookFinAPBillNumber _
                        , .BookFinAPBillNoDate = d.BookFinAPBillNoDate _
                        , .BookFinAPBillInvDate = d.BookFinAPBillInvDate _
                        , .BookFinAPActWgt = If(d.BookFinAPActWgt.HasValue, d.BookFinAPActWgt, 0) _
                        , .BookFinAPPayDate = d.BookFinAPPayDate _
                        , .BookFinAPPayAmt = If(d.BookFinAPPayAmt.HasValue, d.BookFinAPPayAmt, 0) _
                        , .BookFinAPCheck = d.BookFinAPCheck _
                        , .BookFinAPGLNumber = d.BookFinAPGLNumber _
                        , .BookRevTotalCost = If(d.BookRevTotalCost.HasValue, d.BookRevTotalCost, 0) _
                        , .BookModDate = d.BookModDate _
                        , .BookModUser = d.BookModUser _
                        , .BookUpdated = d.BookUpdated.ToArray
                        }).Take(2500).ToArray()

                Return BookAPdetails

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

    Public Function GetBookAPDetailRecord(ByVal Control As Integer) As DataTransferObjects.BookAPDetail
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Get the AP Detail Book Record
                Dim BookAPdetail As DataTransferObjects.BookAPDetail = (
                        From d In db.Books
                        Where
                        (d.BookControl = Control)
                        Select New DataTransferObjects.BookAPDetail With {.BookControl = d.BookControl _
                        , .BookProNumber = d.BookProNumber _
                        , .BookConsPrefix = d.BookConsPrefix _
                        , .BookCarrierControl = d.BookCarrierControl _
                        , .BookDateLoad = d.BookDateLoad _
                        , .BookTranCode = d.BookTranCode _
                        , .BookPayCode = d.BookPayCode _
                        , .BookCarrBLNumber = d.BookCarrBLNumber _
                        , .BookFinAPBillNumber = d.BookFinAPBillNumber _
                        , .BookFinAPBillNoDate = d.BookFinAPBillNoDate _
                        , .BookFinAPBillInvDate = d.BookFinAPBillInvDate _
                        , .BookFinAPActWgt = If(d.BookFinAPActWgt.HasValue, d.BookFinAPActWgt, 0) _
                        , .BookFinAPPayDate = d.BookFinAPPayDate _
                        , .BookFinAPPayAmt = If(d.BookFinAPPayAmt.HasValue, d.BookFinAPPayAmt, 0) _
                        , .BookFinAPCheck = d.BookFinAPCheck _
                        , .BookFinAPGLNumber = d.BookFinAPGLNumber _
                        , .BookRevTotalCost = If(d.BookRevTotalCost.HasValue, d.BookRevTotalCost, 0) _
                        , .BookModDate = d.BookModDate _
                        , .BookModUser = d.BookModUser _
                        , .BookUpdated = d.BookUpdated.ToArray
                        }).First

                Return BookAPdetail

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

    Public Function GetBookCriticalLoads(ByVal CarrierControl As Integer) As DataTransferObjects.Book()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'db.Log = New DebugTextWriter
                'Get the Critical Loads Book Records
                Dim oSecureComp = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl

                Dim Books() As DataTransferObjects.Book = (
                        From d In db.Books
                        Where
                        (d.BookCarrierControl = CarrierControl) _
                        And
                        (d.BookDateDelivered.HasValue = False) _
                        And
                        (d.BookTranCode <> "N") _
                        And
                        (d.BookTranCode <> "P") _
                        And
                        (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.BookCustCompControl))
                        Order By d.BookCarrierControl, d.BookCustCompControl
                        Select selectDTODataWBookTracks(d)).Take(200).ToArray()

                Return Books

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

    Public Function GetBookAPCommissionRecords(ByVal BookCommCompControl As Integer) As DataTransferObjects.Book()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'db.Log = New DebugTextWriter
                'Get the AP Commission Book Records
                Dim Books() As DataTransferObjects.Book = (
                        From d In db.Books
                        Where
                        (d.BookCommCompControl = BookCommCompControl) _
                        And
                        (d.BookRevCommCost.HasValue AndAlso d.BookRevCommCost <> 0) _
                        And
                        (d.BookFinCommPayAmt.HasValue = False OrElse d.BookFinCommPayAmt = 0) _
                        And
                        (d.BookFinARPayAmt >= d.BookFinARInvoiceAmt)
                        Order By d.BookFinARInvoiceDate, d.BookProNumber Ascending
                        Select SelectDTOData(d)).ToArray()

                Return Books

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

    Public Sub updateBookRevLoadTenderTypeControl(ByVal BookControl As Integer, ByVal LoadTenderType As Ngl.FreightMaster.Data.DataTransferObjects.tblLoadTender.LoadTenderTypeEnum, Optional ByVal blnOn As Boolean = True, Optional ByVal blnUpdateLTLPool As Boolean = True)
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oFlags = db.udfGetDependentBookRevLoadTenderTypeControl(BookControl, blnUpdateLTLPool)
                If Not oFlags Is Nothing AndAlso oFlags.Count > 0 Then
                    For Each oflag In oFlags
                        If oflag.BookControl.HasValue Then
                            Dim iBookControl = oflag.BookControl.Value
                            Dim bw As Ngl.Core.Utility.BitwiseFlags32
                            If oflag.BookRevLoadTenderTypeControl.HasValue Then
                                bw = New Ngl.Core.Utility.BitwiseFlags32(oflag.BookRevLoadTenderTypeControl.Value)
                            Else
                                bw = New Ngl.Core.Utility.BitwiseFlags32(0)
                            End If

                            If blnOn Then
                                bw.turnBitFlagOn(LoadTenderType)
                            Else
                                bw.turnBitFlagOff(LoadTenderType)
                            End If
                            db.spUpdateBookRevLoadTenderTypeControl(iBookControl, bw.FlagSource)
                        End If
                    Next
                End If


                ' b.BookRevLoadTenderTypeControl = updateBookRevLTTC(b.BookRevLoadTenderTypeControl, LTTypeEnum.NextStop, True)
                'b.BookRevLoadTenderStatusCode = UpdateBookRevLTSCPost(b.BookRevLoadTenderStatusCode, LTTypeEnum.NextStop)
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("updateBookRevLoadTenderTypeControl"))
            End Try
        End Using

    End Sub

    Public Sub updateBookRevLoadTenderStatusCode(ByVal BookControl As Integer, ByVal LoadTenderSC As Ngl.FreightMaster.Data.DataTransferObjects.tblLoadTender.LoadTenderStatusCodeEnum, Optional ByVal blnOn As Boolean = True, Optional ByVal blnUpdateLTLPool As Boolean = True)
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oFlags = db.udfGetDependentBookRevLoadTenderStatusCode(BookControl, blnUpdateLTLPool)
                If Not oFlags Is Nothing AndAlso oFlags.Count > 0 Then
                    For Each fitem In oFlags
                        If fitem.BookControl.HasValue Then
                            Dim iBookControl = fitem.BookControl.Value
                            Dim bw As Ngl.Core.Utility.BitwiseFlags32
                            If fitem.BookRevLoadTenderStatusCode.HasValue Then
                                bw = New Ngl.Core.Utility.BitwiseFlags32(fitem.BookRevLoadTenderStatusCode.Value)
                            Else
                                bw = New Ngl.Core.Utility.BitwiseFlags32(0)
                            End If

                            If blnOn Then
                                bw.turnBitFlagOn(LoadTenderSC)
                            Else
                                bw.turnBitFlagOff(LoadTenderSC)
                            End If
                            db.spUpdateBookRevLoadTenderStatusCode(iBookControl, bw.FlagSource)
                        End If
                    Next
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("updateBookRevLoadTenderStatusCode"))
            End Try
        End Using

    End Sub


    Public Sub updateBookRevLoadTenderStatusCodePost(ByVal BookControl As Integer, ByVal LoadTenderType As Ngl.FreightMaster.Data.DataTransferObjects.tblLoadTender.LoadTenderTypeEnum, Optional ByVal blnUpdateLTLPool As Boolean = True)
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oFlags = db.udfGetDependentBookRevLoadTenderStatusCode(BookControl, blnUpdateLTLPool)
                If Not oFlags Is Nothing AndAlso oFlags.Count > 0 Then
                    For Each oItem In oFlags
                        If oItem.BookControl.HasValue Then
                            Dim iBookControl = oItem.BookControl.Value
                            Dim bw As Ngl.Core.Utility.BitwiseFlags32
                            If oItem.BookRevLoadTenderStatusCode.HasValue Then
                                bw = New Ngl.Core.Utility.BitwiseFlags32(oItem.BookRevLoadTenderStatusCode.Value)
                            Else
                                bw = New Ngl.Core.Utility.BitwiseFlags32(0)
                            End If

                            'DAT
                            If LoadTenderType = DataTransferObjects.tblLoadTender.LoadTenderTypeEnum.DAT Then
                                'Turn on DAT Post
                                bw.turnBitFlagOn(DataTransferObjects.tblLoadTender.LoadTenderStatusCodeEnum.DATPosted)
                                'Make sure Deleted, Expired, and Error are off
                                bw.turnBitFlagOff(DataTransferObjects.tblLoadTender.LoadTenderStatusCodeEnum.DATDeleted)
                                bw.turnBitFlagOff(DataTransferObjects.tblLoadTender.LoadTenderStatusCodeEnum.DATExpired)
                                bw.turnBitFlagOff(DataTransferObjects.tblLoadTender.LoadTenderStatusCodeEnum.DATError)
                                bw.turnBitFlagOff(DataTransferObjects.tblLoadTender.LoadTenderStatusCodeEnum.DATAccepted)
                                bw.turnBitFlagOff(DataTransferObjects.tblLoadTender.LoadTenderStatusCodeEnum.DATUpdated)
                            ElseIf LoadTenderType = DataTransferObjects.tblLoadTender.LoadTenderTypeEnum.NextStop Then
                                'NEXTStop
                                'Turn on NEXTStop Post
                                bw.turnBitFlagOn(DataTransferObjects.tblLoadTender.LoadTenderStatusCodeEnum.NStopPosted)
                                'Make sure Deleted, Expired, and Error are off
                                bw.turnBitFlagOff(DataTransferObjects.tblLoadTender.LoadTenderStatusCodeEnum.NStopDeleted)
                                bw.turnBitFlagOff(DataTransferObjects.tblLoadTender.LoadTenderStatusCodeEnum.NStopExpired)
                                bw.turnBitFlagOff(DataTransferObjects.tblLoadTender.LoadTenderStatusCodeEnum.NStopError)
                                bw.turnBitFlagOff(DataTransferObjects.tblLoadTender.LoadTenderStatusCodeEnum.NStopAccepted)
                                bw.turnBitFlagOff(DataTransferObjects.tblLoadTender.LoadTenderStatusCodeEnum.NStopUpdated)
                            Else
                                'for anything else we just set the flag to zero
                                bw = New Ngl.Core.Utility.BitwiseFlags32(0)
                            End If
                            db.spUpdateBookRevLoadTenderStatusCode(iBookControl, bw.FlagSource)
                        End If
                    Next
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("updateBookRevLoadTenderStatusCode"))
            End Try
        End Using

    End Sub

    Public Function getUnDeliveredCarriersbyUser() As List(Of Integer)
        Dim oRet As New List(Of Integer)
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oSecureComp = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl

                oRet = (From d In db.Books Where d.BookDateDelivered Is Nothing AndAlso (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.BookCustCompControl)) Select d.BookCarrierControl).Distinct().ToList()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("getUnDeliveredCarriersbyUser"), db)
            End Try
        End Using
        Return oRet
    End Function

#End Region

    '
    Public Function GetNextCNSNumber(ByVal compControl As Integer, Optional ByVal compNumber As Integer = 0) As String
        Dim strConsNumber As String = ""
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oResults = db.spGetNextConsNumber(compControl, compNumber).FirstOrDefault()
                If Not oResults Is Nothing Then
                    strConsNumber = oResults.BookConsNumber
                End If
            Catch ex As Exception
                Utilities.SaveAppError(ex, Parameters)
                'do nothing just returns an empty string
            End Try
        End Using
        Return strConsNumber
    End Function



    Public Function GetBookAPPaidRecords(ByVal BookCarrierControl As Integer) As DataTransferObjects.Book()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oSecureComp = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                'Get the AP Check Entry Book Records
                Dim Books() As DataTransferObjects.Book = (
                        From d In db.Books
                        Where
                        (d.BookCarrierControl = BookCarrierControl) _
                        And
                        (d.BookFinAPPayAmt.HasValue AndAlso d.BookFinAPPayAmt.Value <> 0) _
                        And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.BookCustCompControl))
                        Order By d.BookPayCode, d.BookFinAPPayDate Descending
                        Select SelectDTOData(d)).Take(200).ToArray()

                Return Books

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

    Public Function GetBookARMassEntryRecords(ByVal BookCustCompControl As Integer) As DataTransferObjects.Book()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Get the AP Check Entry Book Records
                Dim Books() As DataTransferObjects.Book = (
                        From d In db.Books
                        Where
                        (d.BookCustCompControl = BookCustCompControl) _
                        And
                        (d.BookFinARPayAmt.HasValue = False OrElse d.BookFinARPayAmt = 0) _
                        And
                        (d.BookFinARInvoiceAmt.HasValue AndAlso d.BookFinARInvoiceAmt.Value <> 0)
                        Order By d.BookFinARInvoiceDate, d.BookProNumber, d.BookConsPrefix
                        Select SelectDTOData(d)).ToArray()

                Return Books

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

    Public Function GetBookCheckCalls(ByVal BookConsPrefix As String) As DataTransferObjects.BookCheckCall()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'db.Log = New DebugTextWriter
                Dim BookCheckCalls() As DataTransferObjects.BookCheckCall = (
                        From d In db.Books
                        Where
                        (d.BookConsPrefix = BookConsPrefix)
                        Order By d.BookStopNo Ascending, d.BookProNumber Ascending
                        Select New DataTransferObjects.BookCheckCall With {.BookControl = d.BookControl _
                        , .BookProNumber = d.BookProNumber _
                        , .BookConsPrefix = d.BookConsPrefix _
                        , .BookCustCompControl = d.BookCustCompControl _
                        , .BookODControl = d.BookODControl _
                        , .BookCarrierControl = d.BookCarrierControl _
                        , .BookStopNo = If(d.BookStopNo.HasValue, d.BookStopNo, 0) _
                        , .BookCarrOrderNumber = d.BookCarrOrderNumber _
                        , .BookCarrActDate = d.BookCarrActDate _
                        , .BookCarrActTime = d.BookCarrActTime _
                        , .BookOrderSequence = d.BookOrderSequence}).ToArray()

                Return BookCheckCalls

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
    ''' This method is no longer Accurate and must be modified to support 7.0 changes
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetBookRevFees(ByVal BookControl As Integer) As DataTransferObjects.BookRevFee()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'TODO: v-7.0 Replace logic for spGetBookFees
                Dim Fees() As DataTransferObjects.BookRevFee = (
                        From d In db.spGetBookFees(BookControl)
                        Select New DataTransferObjects.BookRevFee With {.FeeName = d.FeeName, .FeeCost = If(d.FeeCost.HasValue, d.FeeCost, 0)}).ToArray()

                Return Fees

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
    ''' This method is no longer Accurate and must be modified to support 7.0 changes 
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetTotalCost(ByVal BookControl As Integer) As DataTransferObjects.BookRevTotalCost
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'TODO: v-7.0 Replace this logic with new Call to Tariff Costing Module
                'Get the AP Check Entry Book Records
                Dim BookRevTotalCost As DataTransferObjects.BookRevTotalCost = (
                        From d In db.udfCalcTotalCost(BookControl)
                        Select New DataTransferObjects.BookRevTotalCost With {.TotalCost = If(d.TotalCost.HasValue, d.TotalCost, 0), .NetCost = If(d.NetCost.HasValue, d.NetCost, 0), .FreightTax = If(d.FreightTax.HasValue, d.FreightTax, 0)}).First

                Return BookRevTotalCost

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetTotalCost"))
            End Try

            Return Nothing

        End Using

    End Function

    Public Function GetAPExportFileData() As DataTransferObjects.APExportFileData()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try

                Dim oAPExportData() As DataTransferObjects.APExportFileData = (
                        From d In db.udfGetAPExportRecords()
                        Select New DataTransferObjects.APExportFileData With {.BookCarrBLNumber = d.BookCarrBLNumber,
                        .BookCarrOrderNumber = d.BookCarrOrderNumber,
                        .BookFinAPActCost = d.BookFinAPActCost,
                        .BookFinAPActTax = d.BookFinAPActTax,
                        .BookFinAPActWgt = d.BookFinAPActWgt,
                        .BookFinAPBillInvDate = d.BookFinAPBillInvDate,
                        .BookFinAPBillNoDate = d.BookFinAPBillNoDate,
                        .BookFinAPBillNumber = d.BookFinAPBillNumber,
                        .BookItemCostCenterNumber = d.BookItemCostCenterNumber,
                        .CarrierNumber = d.CarrierNumber,
                        .LaneNumber = d.LaneNumber}).ToArray

                Return oAPExportData

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


        Return Nothing
    End Function

    Public Function GetAPExportCSVData() As String
        Dim strRet As String = ""
        'Get the global parameter GlobalExportAPTaxDetailsToCSV
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim ExportTaxDetails = GetSystemParameterValue("GlobalExportAPTaxDetailsToCSV")
                If ExportTaxDetails = 1 Then
                    Dim oData = db.udfGetAPExportWithTaxDetails()
                    strRet = DataTransformation.CreateCSV(oData)
                Else
                    Dim oData = db.udfGetAPExportRecords()
                    strRet = DataTransformation.CreateCSV(oData)
                End If
                Return strRet
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

        Return strRet
    End Function

    ''' <summary>
    ''' Used to calculate the BFC using the TotalFrtCost component
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="TotalFrtCost"></param>
    ''' <returns></returns>
    ''' <remarks>verified to work with 7.0 by RHR 9/16/13</remarks>
    Public Function GetBFCWithTotalCost(ByVal BookControl As Integer, ByVal TotalFrtCost As Decimal) As Decimal

        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oRet = db.udfGetBFCWithTotalCost(BookControl, TotalFrtCost)

                Return If(oRet.HasValue, oRet.Value, 0)

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

            Return 0

        End Using

    End Function

    ''' <summary>
    ''' Calculate the Service Fees based on Lane Settings using the 
    ''' LaneTrans table and the lane's lanetranstype value
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="TotalFrtCost"></param>
    ''' <returns></returns>
    ''' <remarks>verified to work with 7.0 by RHR 9/16/13</remarks>
    Public Function GetServiceFee(ByVal BookControl As Integer, ByVal TotalFrtCost As Decimal) As Decimal
        'This method is ok for 7.0 verified by RHR 5/13/13
        Using Logger.StartActivity("GetServiceFee(BookControl: {BookControl}, TotalFrtCost: {TotalFrtCost})")
            Using db As New NGLMasBookDataContext(ConnectionString)
                Try
                    Dim oRet = db.udfGetServiceFee(BookControl, TotalFrtCost)

                    Return If(oRet.HasValue, oRet.Value, 0)

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

                Return 0
            End Using

        End Using

    End Function

    ''' <summary>
    ''' Calculates any Service Fees Applied to the load
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="BookFinAPPayAmt"></param>
    ''' <param name="BookFinAPActCost"></param>
    ''' <param name="BookRevTotalCost"></param>
    ''' <returns></returns>
    ''' <remarks>verified to work with 7.0 by RHR 9/16/13</remarks>
    Public Function GetServiceFee(ByVal BookControl As Integer,
                                  ByVal BookFinAPPayAmt As Decimal,
                                  ByVal BookFinAPActCost As Decimal,
                                  ByVal BookRevTotalCost As Decimal) As Decimal

        Using Logger.StartActivity("GetServiceFee(BookControl: {BookControl}, BookFinAPPayAmt: {BookFinAPPayAmt}, BookFinAPActCost: {BookFinAPActCost}, BookRevTotalCost: {BookRevTotalCost})",
                                   BookControl,
                                   BookFinAPPayAmt,
                                   BookFinAPActCost,
                                   BookRevTotalCost)

            If BookFinAPPayAmt <> 0 Then
                Return GetServiceFee(BookControl, BookFinAPPayAmt)
            ElseIf BookFinAPActCost <> 0 Then
                Return GetServiceFee(BookControl, BookFinAPActCost)
            Else
                Return GetServiceFee(BookControl, BookRevTotalCost)
            End If
        End Using

    End Function

    Public Function GetBookTruckLoadData(ByVal BookConsPrefix As String) As DataTransferObjects.BookTruckLoadData()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oTruckData() As DataTransferObjects.BookTruckLoadData = (
                        From d In db.udfGetBookTruckLoadData(BookConsPrefix)
                        Select New DataTransferObjects.BookTruckLoadData With {.BookControl = If(d.BookControl.HasValue, d.BookControl, 0),
                        .BookCustCompControl = If(d.BookCustCompControl.HasValue, d.BookCustCompControl, 0),
                        .BookOrigZip = d.BookOrigZip,
                        .BookDestZip = d.BookDestZip,
                        .BookOrigAddress1 = d.BookOrigAddress1,
                        .BookDestAddress1 = d.BookDestAddress1,
                        .BookOrigCity = d.BookOrigCity,
                        .BookDestCity = d.BookDestCity,
                        .BookOrigState = d.BookOrigState,
                        .BookDestState = d.BookDestState,
                        .BookLoadControl = If(d.BookLoadControl.HasValue, d.BookLoadControl, 0),
                        .BookODControl = If(d.BookODControl.HasValue, d.BookODControl, 0),
                        .BookProNumber = d.BookProNumber,
                        .LaneOriginAddressUse = If(d.LaneOriginAddressUse.HasValue, d.LaneOriginAddressUse, False),
                        .BookStopNo = If(d.BookStopNo.HasValue, d.BookStopNo, 0),
                        .BookConsPrefix = d.BookConsPrefix}).ToArray

                Return oTruckData

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

    Public Function GetCriticalALL() As DataTransferObjects.CriticalALL()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Get All Critical Loads
                Dim Critical() As DataTransferObjects.CriticalALL = (
                        From d In db.spCriticalAll50(Me.Parameters.UserName)
                        Select New DataTransferObjects.CriticalALL With {.CarrierControl = d.CarrierControl,
                        .CarrierNumber = If(d.CarrierNumber.HasValue, d.CarrierNumber, 0),
                        .CarrierName = d.CarrierName,
                        .Critical = If(d.Critical.HasValue, d.Critical, 0),
                        .Load_Count = If(d.Load_Count.HasValue, d.Load_Count, 0)}).ToArray()

                Return Critical

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

    Public Function GetOrigDestCost(ByVal OrigState As String, ByVal DestState As String, ByVal FromComp As Integer, ByVal ToComp As Integer) As DataTransferObjects.OrigDestCost()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Get the data
                Dim oList() As DataTransferObjects.OrigDestCost = (
                        From d In db.spOrigDestCost50(OrigState, DestState, FromComp, ToComp, Me.Parameters.UserName)
                        Select New DataTransferObjects.OrigDestCost With {.CarrierNumber = d.CarrierNumber,
                        .CompName = d.CompName,
                        .BookOrigCity = d.BookOrigCity,
                        .BookOrigState = d.BookOrigState,
                        .BookDestCity = d.BookDestCity,
                        .BookDestState = d.BookDestState,
                        .BookDateLoad = d.BookDateLoad,
                        .BookTotalWgt = d.BookTotalWgt,
                        .BookTotalBFC = d.BookTotalBFC,
                        .CarrierName = d.CarrierName,
                        .BookProNumber = d.BookProNumber,
                        .BookControl = d.BookControl,
                        .CompNumber = d.CompNumber}).ToArray()

                Return oList

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

    Public Function GetGetBookControlsWithSameCNS(ByVal CNS As String, ByVal bookControl As Integer) As DataTransferObjects.vBookControl()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Get the data
                Dim oList As DataTransferObjects.vBookControl() = (
                        From d In db.spGetBookControlsWithSameCNS(CNS, bookControl)
                        Select New DataTransferObjects.vBookControl With {.BookControl = d.BookControl}).ToArray()

                Return oList

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

    Public Function GetGetBookControlBySHID(ByVal sBookSHID As String) As Integer
        Dim intRet As Integer = 0
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                intRet = db.Books.Where(Function(x) x.BookSHID = sBookSHID).Select(Function(b) b.BookControl).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetGetBookControlBySHID"), db)
            End Try

            Return intRet

        End Using
    End Function



    Public Function GetGetBookControlByAPBillNumber(ByVal sAPBillNumber As String) As Integer
        Dim intRet As Integer = 0
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                intRet = db.Books.Where(Function(x) x.BookFinAPBillNumber = sAPBillNumber).Select(Function(b) b.BookControl).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetGetBookControlByAPBillNumber"), db)
            End Try

            Return intRet

        End Using
    End Function

    Public Function GetBookControlByOrderNumber(ByVal BookCarrOrderNumber As String) As Integer
        Dim iBookControl As Integer = 0
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Get the data
                iBookControl = db.Books.Where(Function(x) x.BookCarrOrderNumber = BookCarrOrderNumber).Select(Function(y) y.BookControl).FirstOrDefault()


            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetBookControlByOrderNumber"))
            End Try

            Return iBookControl

        End Using
    End Function

    Public Function GetBookControlForStopFee(ByVal BookSHID As String,
                                             ByVal StopNumber As Integer,
                                             ByVal TotalCost As Decimal,
                                             ByVal AccessorialCode As Integer) As LTS.spGetBookControlForFeeResult()
        Dim oRetData As LTS.spGetBookControlForFeeResult()
        Using Logger.StartActivity("GetBookControlForStopFee(BookSHID: {BookSHID}, StopNumber: {StopNumber}, TotalCost: {TotalCost}, AccessorialCode: {AccessorialCode})")
            Using db As New NGLMasBookDataContext(ConnectionString)
                Try

                    oRetData = db.spGetBookControlForFee(BookSHID, StopNumber, TotalCost, AccessorialCode).ToArray()

                Catch ex As Exception
                    Logger.Error(ex, "Error in GetBookControlForStopFee")
                    ManageLinqDataExceptions(ex, buildProcedureName("GetBookControlForStopFee"))
                End Try
            End Using
        End Using

        Return oRetData


    End Function

    Public Function GetGetBookControlsWithSameStopNo(ByVal CNS As String, ByVal StopNumber As String, ByVal bookControl As Integer) As DataTransferObjects.vBookControl()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim stpNumber As Integer = 0
                If Integer.TryParse(StopNumber, stpNumber) Then

                    'Get the data
                    Dim oList As DataTransferObjects.vBookControl() = (
                            From d In db.spGetBookControlsWithSameStopNo(CNS, stpNumber, bookControl)
                            Select New DataTransferObjects.vBookControl With {.BookControl = d.BookControl}).ToArray()

                    Return oList
                End If

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
    ''' Uses AllFilters object differently than GetTenderedOrders in that this version supports users added filters
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by LVV on 4/8/19
    ''' </remarks>
    Public Function GetTenderedOrders365(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vTenderedOrder()
        If filters Is Nothing Then Return Nothing
        Dim CarrierControl As Integer
        Dim CarrierContControl As Integer
        'If ContactControl (BookCarrierContControl) is populated use that, else use the value in CarrierControlFrom (BookCarrierControl)
        If filters.ContactControl > 0 Then CarrierContControl = filters.ContactControl Else CarrierControl = filters.CarrierControlFrom
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oSecureComp = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                Dim iQuery As IQueryable(Of LTS.vTenderedOrder)
                If CarrierContControl > 0 Then
                    iQuery = (From t In db.vTenderedOrders
                              Where (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.BookCustCompControl)) _
                                    And t.BookCarrierContControl = CarrierContControl
                              Select t)
                    'iQuery = db.vTenderedOrders.Where(Function(x) x.BookCarrierContControl = CarrierContControl)
                ElseIf CarrierControl > 0 Then
                    iQuery = (From t In db.vTenderedOrders
                              Where (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.BookCustCompControl)) _
                                    And t.BookCarrierControl = CarrierControl
                              Select t)
                    'iQuery = db.vTenderedOrders.Where(Function(x) x.BookCarrierControl = CarrierControl)
                Else
                    'if we get here this is a non-carrier user so we have the possibility of multiple carriers
                    Dim oSecureCarrier = From s In db.tblUserSecurityCarrierRefBooks Where s.USCUserSecurityControl = Me.Parameters.UserControl Select s.USCCarrierControl
                    iQuery = (From t In db.vTenderedOrders
                              Where (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.BookCustCompControl)) _
                                    And (oSecureCarrier Is Nothing OrElse oSecureCarrier.Count = 0 OrElse oSecureCarrier.Contains(t.BookCarrierControl))
                              Select t)
                End If
                If iQuery Is Nothing Then Return Nothing
                Dim filterWhere = ""

                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                Dim oRet() As LTS.vTenderedOrder = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetTenderedOrders365"))
            End Try
            Return Nothing
        End Using
    End Function

    Public Function GetvBookLoadBoard(ByVal iBookControl As Integer) As LTS.vBookLoadBoard
        Dim oRet As New LTS.vBookLoadBoard()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                oRet = db.vBookLoadBoards.Where(Function(x) x.BookControl = iBookControl).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetvBookLoadBoard"))
            End Try
            Return oRet
        End Using
    End Function

    Public Function GetvBookLoadBoard(ByVal sProNumber As String) As LTS.vBookLoadBoard
        Dim oRet As New LTS.vBookLoadBoard()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                oRet = db.vBookLoadBoards.Where(Function(x) x.BookProNumber = sProNumber).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetvBookLoadBoard"))
            End Try
            Return oRet
        End Using
    End Function


    Public Function GetBooksPendingAcceptanceByCont(ByVal CarrierContControl As Integer,
                                                    ByVal sortOrdinal As String,
                                                    ByVal sortDirection As String,
                                                    ByVal datefilterfield As String,
                                                    ByVal dateFilterFrom As System.Nullable(Of Date),
                                                    ByVal dateFilterTo As System.Nullable(Of Date)) As DataTransferObjects.TenderedItem()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Get the data
                Dim oList() As DataTransferObjects.TenderedItem = (
                        From d In db.spBookRecordsPendingAcceptanceContControl_SORTED(CarrierContControl, sortOrdinal, sortDirection, datefilterfield, dateFilterFrom, dateFilterTo)
                        Select New DataTransferObjects.TenderedItem With {.Control = d.BookControl,
                        .ProNumber = d.BookProNumber,
                        .CnsNumber = d.BookConsPrefix,
                        .CnsIntegrity = d.BookRouteConsFlag,
                        .OrderNumber = d.BookCarrOrderNumber,
                        .PickupName = d.BookOrigName,
                        .PickupAddress = d.BookOrigAddress1,
                        .PickupCity = d.BookOrigCity,
                        .PickupState = d.BookOrigState,
                        .PickupZipCode = d.BookOrigZip,
                        .DestinationName = d.BookDestName,
                        .DestinationAddress = d.BookDestAddress1,
                        .DestinationCity = d.BookDestCity,
                        .DestinationState = d.BookDestState,
                        .DestinationZipCode = d.BookDestZip,
                        .Cases = If(d.BookTotalCases.HasValue, d.BookTotalCases, 0),
                        .Weight = If(d.BookTotalWgt.HasValue, d.BookTotalWgt, 0),
                        .Pallets = If(d.BookTotalPl.HasValue, d.BookTotalPl, 0),
                        .Cubes = If(d.BookTotalCube.HasValue, d.BookTotalCube, 0),
                        .PickupDate = d.BookDateLoad,
                        .DeliveryDate = d.BookDateRequired,
                        .ContractCost = If(d.BookRevTotalCost.HasValue, d.BookRevTotalCost, 0),
                        .AssignedDate = d.BookTrackDate}).ToArray()

                Return oList

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
    ''' 
    ''' </summary>
    ''' <param name="CarrierControl"></param>
    ''' <param name="sortOrdinal"></param>
    ''' <param name="sortDirection"></param>
    ''' <param name="datefilterfield"></param>
    ''' <param name="dateFilterFrom"></param>
    ''' <param name="dateFilterTo"></param>
    ''' <returns></returns>
    Public Function GetBooksRecordsPendingAcceptance(ByVal CarrierControl As Integer,
                                                     ByVal sortOrdinal As String,
                                                     ByVal sortDirection As String,
                                                     ByVal datefilterfield As String,
                                                     ByVal dateFilterFrom As System.Nullable(Of Date),
                                                     ByVal dateFilterTo As System.Nullable(Of Date)) As DataTransferObjects.TenderedItem()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Get the data
                Dim oList() As DataTransferObjects.TenderedItem = (
                        From d In db.BookRecordsPendingAcceptance_SORTED(CarrierControl, sortOrdinal, sortDirection, datefilterfield, dateFilterFrom, dateFilterTo)
                        Select New DataTransferObjects.TenderedItem With {.Control = d.BookControl,
                        .ProNumber = d.BookProNumber,
                        .CnsNumber = d.BookConsPrefix,
                        .CnsIntegrity = d.BookRouteConsFlag,
                        .OrderNumber = d.BookCarrOrderNumber,
                        .PickupName = d.BookOrigName,
                        .PickupAddress = d.BookOrigAddress1,
                        .PickupCity = d.BookOrigCity,
                        .PickupState = d.BookOrigState,
                        .PickupZipCode = d.BookOrigZip,
                        .DestinationName = d.BookDestName,
                        .DestinationAddress = d.BookDestAddress1,
                        .DestinationCity = d.BookDestCity,
                        .DestinationState = d.BookDestState,
                        .DestinationZipCode = d.BookDestZip,
                        .Cases = If(d.BookTotalCases.HasValue, d.BookTotalCases, 0),
                        .Weight = If(d.BookTotalWgt.HasValue, d.BookTotalWgt, 0),
                        .Pallets = If(d.BookTotalPl.HasValue, d.BookTotalPl, 0),
                        .Cubes = If(d.BookTotalCube.HasValue, d.BookTotalCube, 0),
                        .PickupDate = d.BookDateLoad,
                        .DeliveryDate = d.BookDateRequired,
                        .ContractCost = If(d.BookRevTotalCost.HasValue, d.BookRevTotalCost, 0),
                        .AssignedDate = d.BookTrackDate}).ToArray()

                Return oList

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


    Public Function GetWebBillPayRecords(ByVal CarrierControl As System.Nullable(Of Integer),
                                         ByVal CarrierContControl As System.Nullable(Of Integer),
                                         ByVal proNumber As String,
                                         ByVal cns As String,
                                         ByVal bookCarrOrderNumber As String,
                                         ByVal bookDateDelDate As System.Nullable(Of Date),
                                         ByVal bookDateDelDateTo As System.Nullable(Of Date),
                                         ByVal bookPayCode As String,
                                         ByVal bookRevTotalCost As System.Nullable(Of Decimal),
                                         ByVal bookFinAPBillNumber As String,
                                         ByVal bookFinAPActCost As System.Nullable(Of Decimal),
                                         ByVal sortOrdinal As String,
                                         ByVal sortDirection As String,
                                         ByVal datefilterfield As String,
                                         ByVal dateFilterFrom As System.Nullable(Of Date),
                                         ByVal dateFilterTo As System.Nullable(Of Date)) As DataTransferObjects.SettlementItem()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Get the data
                Dim oList() As DataTransferObjects.SettlementItem = (
                        From d In db.spGetWebBillPayRecords_SORTED(CarrierControl, CarrierContControl, proNumber, cns, bookCarrOrderNumber, bookDateDelDate,
                                                                   bookDateDelDateTo, bookPayCode, bookRevTotalCost, bookFinAPBillNumber, bookFinAPActCost, sortOrdinal, sortDirection, datefilterfield, dateFilterFrom, dateFilterTo)
                        Select New DataTransferObjects.SettlementItem With {.Control = If(d.BookControl.HasValue, d.BookControl, 0),
                        .ProNumber = d.BookProNumber,
                        .CnsNumber = d.BookConsPrefix,
                        .OrderNumber = d.BookCarrOrderNumber,
                        .PickupName = d.BookOrigName,
                        .DestinationName = d.BookDestName,
                        .Status = d.BookPayCode,
                        .DeliveredDate = d.BookDateDelivered,
                        .ContractedCost = If(d.BookRevTotalCost.HasValue, d.BookRevTotalCost, 0),
                        .InvoiceNumber = d.BookFinAPBillNumber,
                        .InvoiceAmount = If(d.BookFinAPActCost.HasValue, d.BookFinAPActCost, 0)}).ToArray()

                Return oList

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
    ''' GetWebBillPayRecords7052
    ''' </summary>
    ''' <param name="CarrierControl"></param>
    ''' <param name="CarrierContControl"></param>
    ''' <param name="proNumber"></param>
    ''' <param name="cns"></param>
    ''' <param name="bookCarrOrderNumber"></param>
    ''' <param name="bookDateDelDate"></param>
    ''' <param name="bookDateDelDateTo"></param>
    ''' <param name="bookPayCode"></param>
    ''' <param name="bookRevTotalCost"></param>
    ''' <param name="bookFinAPBillNumber"></param>
    ''' <param name="bookFinAPActCost"></param>
    ''' <param name="bookSHID"></param>
    ''' <param name="sortOrdinal"></param>
    ''' <param name="sortDirection"></param>
    ''' <param name="datefilterfield"></param>
    ''' <param name="dateFilterFrom"></param>
    ''' <param name="dateFilterTo"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added by LVV on 7/28/16 for v-7.0.5.110 Task #14 NxT Search Filters
    ''' </remarks>
    Public Function GetWebBillPayRecords7052(ByVal CarrierControl As System.Nullable(Of Integer),
                                             ByVal CarrierContControl As System.Nullable(Of Integer),
                                             ByVal proNumber As String,
                                             ByVal cns As String,
                                             ByVal bookCarrOrderNumber As String,
                                             ByVal bookDateDelDate As System.Nullable(Of Date),
                                             ByVal bookDateDelDateTo As System.Nullable(Of Date),
                                             ByVal bookPayCode As String,
                                             ByVal bookRevTotalCost As System.Nullable(Of Decimal),
                                             ByVal bookFinAPBillNumber As String,
                                             ByVal bookFinAPActCost As System.Nullable(Of Decimal),
                                             ByVal bookSHID As String,
                                             ByVal bookShipCarrierProNumber As String,
                                             ByVal sortOrdinal As String,
                                             ByVal sortDirection As String,
                                             ByVal datefilterfield As String,
                                             ByVal dateFilterFrom As System.Nullable(Of Date),
                                             ByVal dateFilterTo As System.Nullable(Of Date)) As DataTransferObjects.SettlementItem()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Get the data
                Dim oList() As DataTransferObjects.SettlementItem = (
                        From d In db.spGetWebBillPayRecords_SORTED7052(CarrierControl, CarrierContControl, proNumber, cns, bookCarrOrderNumber, bookDateDelDate,
                                                                       bookDateDelDateTo, bookPayCode, bookRevTotalCost, bookFinAPBillNumber, bookFinAPActCost,
                                                                       bookSHID, bookShipCarrierProNumber, sortOrdinal, sortDirection, datefilterfield, dateFilterFrom, dateFilterTo)
                        Select New DataTransferObjects.SettlementItem With {.Control = If(d.BookControl.HasValue, d.BookControl, 0),
                        .ProNumber = d.BookProNumber,
                        .CnsNumber = d.BookConsPrefix,
                        .OrderNumber = d.BookCarrOrderNumber,
                        .PickupName = d.BookOrigName,
                        .DestinationName = d.BookDestName,
                        .Status = d.BookPayCode,
                        .DeliveredDate = d.BookDateDelivered,
                        .ContractedCost = If(d.BookRevTotalCost.HasValue, d.BookRevTotalCost, 0),
                        .InvoiceNumber = d.BookFinAPBillNumber,
                        .InvoiceAmount = If(d.BookFinAPActCost.HasValue, d.BookFinAPActCost, 0),
                        .SHID = d.BookSHID,
                        .CarrierPro = d.BookShipCarrierProNumber}).ToArray()
                Return oList

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetWebBillPayRecords7052"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetWebBillPayRecordsWPaging(ByVal CarrierControl As System.Nullable(Of Integer),
                                                ByVal CarrierContControl As System.Nullable(Of Integer),
                                                ByVal proNumber As String,
                                                ByVal cns As String,
                                                ByVal bookCarrOrderNumber As String,
                                                ByVal bookDateDelDate As System.Nullable(Of Date),
                                                ByVal bookDateDelDateTo As System.Nullable(Of Date),
                                                ByVal bookPayCode As String,
                                                ByVal bookRevTotalCost As System.Nullable(Of Decimal),
                                                ByVal bookFinAPBillNumber As String,
                                                ByVal bookFinAPActCost As System.Nullable(Of Decimal),
                                                ByVal sortOrdinal As String,
                                                ByVal sortDirection As String,
                                                ByVal datefilterfield As String,
                                                ByVal dateFilterFrom As System.Nullable(Of Date),
                                                ByVal dateFilterTo As System.Nullable(Of Date),
                                                ByVal pageSize As Integer,
                                                ByVal page As Integer) As DataTransferObjects.SettlementItem()
        If pageSize < 1 Then pageSize = 1
        If page < 1 Then page = 1
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'spGetWebBillPayRecords_SORTEDWPages is not complete yet
                Throw New NotImplementedException()
                ''Get the data
                'Dim oList() As DTO.SettlementItem = ( _
                'From d In db.spGetWebBillPayRecords_SORTEDWPages(CarrierControl, CarrierContControl, proNumber, cns, bookCarrOrderNumber, bookDateDelDate, _
                '                                          bookDateDelDateTo, bookPayCode, bookRevTotalCost, bookFinAPBillNumber, bookFinAPActCost, sortOrdinal, sortDirection, datefilterfield, dateFilterFrom, dateFilterTo) _
                'Select New DTO.SettlementItem With {.Control = If(d.BookControl.HasValue, d.BookControl, 0), _
                '                                 .ProNumber = d.BookProNumber, _
                '                                 .CnsNumber = d.BookConsPrefix, _
                '                                 .OrderNumber = d.BookCarrOrderNumber, _
                '                                .PickupName = d.BookOrigName, _
                '                                      .DestinationName = d.BookDestName, _
                '                                 .Status = d.BookPayCode, _
                '                                .DeliveredDate = d.BookDateDelivered, _
                '                                    .ContractedCost = If(d.BookRevTotalCost.HasValue, d.BookRevTotalCost, 0), _
                '                                  .InvoiceNumber = d.BookFinAPBillNumber, _
                '                                   .InvoiceAmount = If(d.BookFinAPActCost.HasValue, d.BookFinAPActCost, 0)}).ToArray()

                'Return oList

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

    Public Function GetWebSettledRecords(ByVal CarrierControl As System.Nullable(Of Integer),
                                         ByVal CarrierContControl As System.Nullable(Of Integer),
                                         ByVal proNumber As String,
                                         ByVal cns As String,
                                         ByVal bookFinAPPayDate As System.Nullable(Of Date),
                                         ByVal bookFinAPPayAmt As System.Nullable(Of Decimal),
                                         ByVal bookFinAPPayCheck As String,
                                         ByVal bookRevTotalCost As System.Nullable(Of Decimal),
                                         ByVal bookFinAPBillNumber As String,
                                         ByVal bookFinAPActCost As System.Nullable(Of Decimal),
                                         ByVal sortOrdinal As String,
                                         ByVal sortDirection As String,
                                         ByVal datefilterfield As String,
                                         ByVal dateFilterFrom As System.Nullable(Of Date),
                                         ByVal dateFilterTo As System.Nullable(Of Date)) As DataTransferObjects.SettledItem()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Get the data
                Dim oList() As DataTransferObjects.SettledItem = (
                        From d In db.spGetWebSettledRecords_SORTED(CarrierControl,
                                                                   CarrierContControl,
                                                                   proNumber,
                                                                   cns,
                                                                   bookFinAPPayDate,
                                                                   bookFinAPPayAmt,
                                                                   bookFinAPPayCheck,
                                                                   bookRevTotalCost,
                                                                   bookFinAPBillNumber,
                                                                   bookFinAPActCost,
                                                                   sortOrdinal,
                                                                   sortDirection,
                                                                   datefilterfield,
                                                                   dateFilterFrom,
                                                                   dateFilterTo)
                        Select New DataTransferObjects.SettledItem With {.Control = If(d.BookControl.HasValue, d.BookControl, 0),
                        .ProNumber = d.BookProNumber,
                        .CnsNumber = d.BookConsPrefix,
                        .InvoiceNumber = d.BookFinAPBillNumber,
                        .ContractedCost = If(d.BookRevTotalCost.HasValue, d.BookRevTotalCost, 0),
                        .PaidCost = If(d.BookFinAPPayAmt.HasValue, d.BookFinAPPayAmt, 0),
                        .InvoiceAmount = If(d.BookFinAPActCost.HasValue, d.BookFinAPActCost, 0),
                        .PaidDate = d.BookFinAPPayDate,
                        .CheckNumber = d.BookFinAPCheck}).ToArray()

                Return oList

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
    ''' GetWebSettledRecords7052
    ''' </summary>
    ''' <param name="CarrierControl"></param>
    ''' <param name="CarrierContControl"></param>
    ''' <param name="proNumber"></param>
    ''' <param name="cns"></param>
    ''' <param name="bookFinAPPayDate"></param>
    ''' <param name="bookFinAPPayAmt"></param>
    ''' <param name="bookFinAPPayCheck"></param>
    ''' <param name="bookRevTotalCost"></param>
    ''' <param name="bookFinAPBillNumber"></param>
    ''' <param name="bookFinAPActCost"></param>
    ''' <param name="sortOrdinal"></param>
    ''' <param name="sortDirection"></param>
    ''' <param name="datefilterfield"></param>
    ''' <param name="dateFilterFrom"></param>
    ''' <param name="dateFilterTo"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added by LVV on 7/28/16 for v-7.0.5.110 Task #14 NxT Search Filters
    ''' </remarks>
    Public Function GetWebSettledRecords7052(ByVal CarrierControl As System.Nullable(Of Integer),
                                             ByVal CarrierContControl As System.Nullable(Of Integer),
                                             ByVal proNumber As String,
                                             ByVal cns As String,
                                             ByVal bookFinAPPayDate As System.Nullable(Of Date),
                                             ByVal bookFinAPPayAmt As System.Nullable(Of Decimal),
                                             ByVal bookFinAPPayCheck As String,
                                             ByVal bookRevTotalCost As System.Nullable(Of Decimal),
                                             ByVal bookFinAPBillNumber As String,
                                             ByVal bookFinAPActCost As System.Nullable(Of Decimal),
                                             ByVal bookSHID As String,
                                             ByVal bookShipCarrierProNumber As String,
                                             ByVal sortOrdinal As String,
                                             ByVal sortDirection As String,
                                             ByVal datefilterfield As String,
                                             ByVal dateFilterFrom As System.Nullable(Of Date),
                                             ByVal dateFilterTo As System.Nullable(Of Date)) As DataTransferObjects.SettledItem()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Get the data
                Dim oList() As DataTransferObjects.SettledItem = (
                        From d In db.spGetWebSettledRecords_SORTED7052(CarrierControl,
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
                                                                       dateFilterTo)
                        Select New DataTransferObjects.SettledItem With {.Control = If(d.BookControl.HasValue, d.BookControl, 0),
                        .ProNumber = d.BookProNumber,
                        .CnsNumber = d.BookConsPrefix,
                        .InvoiceNumber = d.BookFinAPBillNumber,
                        .ContractedCost = If(d.BookRevTotalCost.HasValue, d.BookRevTotalCost, 0),
                        .PaidCost = If(d.BookFinAPPayAmt.HasValue, d.BookFinAPPayAmt, 0),
                        .InvoiceAmount = If(d.BookFinAPActCost.HasValue, d.BookFinAPActCost, 0),
                        .PaidDate = d.BookFinAPPayDate,
                        .CheckNumber = d.BookFinAPCheck,
                        .SHID = d.BookSHID,
                        .CarrierPro = d.BookShipCarrierProNumber}).ToArray()
                Return oList
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetWebSettledRecords7052"))
            End Try
            Return Nothing
        End Using
    End Function

    Public Function GetBookTrackComments(ByVal bookControl As Integer) As DataTransferObjects.vBookTrackXQ()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Get the data
                Dim oList() As DataTransferObjects.vBookTrackXQ = (
                        From d In db.spGetBookTrackComments(bookControl)
                        Select New DataTransferObjects.vBookTrackXQ With {.BookTrackBookControl = If(d.BookTrackBookControl.HasValue, d.BookTrackBookControl, 0),
                        .BookTrackComment = d.BookTrackComment,
                        .BookTrackContact = d.BookTrackContact,
                        .BookTrackControl = If(d.BookTrackControl.HasValue, d.BookTrackControl, 0),
                        .BookTrackDate = d.TrackDate,
                        .BookTrackModDate = d.BookTrackModDate,
                        .BookTrackModUser = d.BookTrackModUser,
                        .BookTrackStatus = If(d.BookTrackStatus.HasValue, d.BookTrackStatus, 0)}).ToArray()

                Return oList

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

    Public Function GetBookLoadDetailReport(ByVal proNumber As String) As DataTransferObjects.BookLoadDetailReport()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Get the data
                Dim oList() As DataTransferObjects.BookLoadDetailReport = (
                        From d In db.spGetLoadDetailReport(proNumber)
                        Select New DataTransferObjects.BookLoadDetailReport With {.BookCarrActDate = d.BookCarrActDate,
                        .BookCarrActTime = d.BookCarrActTime,
                        .BookCarrActualDate = d.BookCarrActualDate,
                        .BookCarrActualTime = d.BookCarrActualTime,
                        .BookCarrApptDate = d.BookCarrApptDate,
                        .BookCarrApptTime = d.BookCarrApptTime,
                        .BookCarrOrderNumber = d.BookCarrOrderNumber,
                        .BookCarrScheduleDate = d.BookCarrScheduleDate,
                        .BookCarrScheduleTime = d.BookCarrScheduleTime,
                        .BookConsPrefix = d.BookConsPrefix,
                        .BookControl = d.BookControl,
                        .BookDateRequired = d.BookDateRequired,
                        .BookDestAddress1 = d.BookDestAddress1,
                        .BookDestCity = d.BookDestCity,
                        .BookDestCountry = d.BookDestCountry,
                        .BookDestName = d.BookDestName,
                        .BookDestState = d.BookDestState,
                        .BookDestZip = d.BookDestZip,
                        .BookLoadPONumber = d.BookLoadPONumber,
                        .BookOrigAddress1 = d.BookOrigAddress1,
                        .BookOrigCity = d.BookOrigCity,
                        .BookOrigCountry = d.BookOrigCountry,
                        .BookOrigName = d.BookOrigName,
                        .BookOrigState = d.BookOrigState,
                        .BookOrigZip = d.BookOrigZip,
                        .BookProNumber = d.BookProNumber,
                        .BookTotalCases = If(d.BookTotalCases.HasValue, d.BookTotalCases, 0),
                        .BookTotalCube = If(d.BookTotalCube.HasValue, d.BookTotalCube, 0),
                        .BookTotalPL = If(d.BookTotalPL.HasValue, d.BookTotalPL, 0),
                        .BookTotalWgt = If(d.BookTotalWgt.HasValue, d.BookTotalWgt, 0),
                        .CarrierName = d.CarrierName,
                        .BookSHID = d.BookSHID,
                        .BookExpDelDateTime = d.BookExpDelDateTime,
                        .BookMustLeaveByDateTime = d.BookMustLeaveByDateTime}).ToArray()

                Return oList

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

    Public Function GetAllItemsLite(ByVal CompanyIDsDelimitedByComma As String) As DataTransferObjects.AllItemLite()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try

                Dim xmlXmlLoadCompanyIDs As XElement = If(String.IsNullOrEmpty(CompanyIDsDelimitedByComma), Nothing, XElement.Parse(CompanyIDsDelimitedByComma))
                'Get the data
                Dim oList() As DataTransferObjects.AllItemLite = (
                        From d In db.spGetNGLLoadSortedLite(xmlXmlLoadCompanyIDs)
                        Select New DataTransferObjects.AllItemLite With {.CarrierControl = d.CarrierControl,
                        .CarrierName = d.CarrierName,
                        .CarrierNumber = If(d.CarrierNumber.HasValue, d.CarrierNumber, 0)}).ToArray()

                Return oList

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

    Public Function GetCarriersSchedDateLite(ByVal CompanyIDsDelimitedByComma As String, ByVal CarrierControl As Integer) As DataTransferObjects.AllItemLite()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try

                Dim xmlXmlLoadCompanyIDs As XElement = If(String.IsNullOrEmpty(CompanyIDsDelimitedByComma), Nothing, XElement.Parse(CompanyIDsDelimitedByComma))
                'Get the data
                Dim oList() As DataTransferObjects.AllItemLite = (
                        From d In db.spGetNGLSchedCarriersByDateLite(xmlXmlLoadCompanyIDs, CarrierControl)
                        Select New DataTransferObjects.AllItemLite With {.ScheduledToLoad = d.BookCarrApptDate,
                        .PickupScheduledAppointmentDate = d.BookCarrScheduleDate}).ToArray()

                Return oList

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

    Public Function GetLoadsByDateLite(ByVal CompanyIDsDelimitedByComma As String, ByVal CarrierControl As Integer, ByVal apptDate As DateTime?) As DataTransferObjects.AllItemLite()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try

                Dim xmlXmlLoadCompanyIDs As XElement = If(String.IsNullOrEmpty(CompanyIDsDelimitedByComma), Nothing, XElement.Parse(CompanyIDsDelimitedByComma))
                'Get the data
                Dim oList() As DataTransferObjects.AllItemLite = (
                        From d In db.spGetNGLLoadsByDateLite(xmlXmlLoadCompanyIDs, CarrierControl, apptDate)
                        Select New DataTransferObjects.AllItemLite With {.PickupScheduledAppointmentDate = If(d.BookCarrApptDate.HasValue, d.BookCarrApptDate, Nothing),
                        .PickupDockPUAssignment = d.BookCarrDockPUAssigment,
                        .PickupFinishLoadingDate = If(d.BookCarrFinishLoadingDate.HasValue, d.BookCarrFinishLoadingDate, Nothing),
                        .PickupFinishLoadingTime = If(d.BookCarrFinishLoadingTime.HasValue, d.BookCarrFinishLoadingTime, Nothing),
                        .DeliveryFinishUnloadingDate = If(d.BookCarrFinishUnloadingDate.HasValue, d.BookCarrFinishUnloadingDate, Nothing),
                        .DeliveryFinishUnloadingTime = If(d.BookCarrFinishUnloadingTime.HasValue, d.BookCarrFinishUnloadingTime, Nothing),
                        .OrderNumber = d.BookCarrOrderNumber,
                        .ScheduledToLoad = If(d.BookCarrScheduleDate.HasValue, d.BookCarrScheduleDate, Nothing),
                        .PickupStartLoadingDate = If(d.BookCarrStartLoadingDate.HasValue, d.BookCarrStartLoadingDate, Nothing),
                        .PickupStartLoadingTime = If(d.BookCarrStartLoadingTime.HasValue, d.BookCarrStartLoadingTime, Nothing),
                        .DeliveryStartUnloadingDate = If(d.BookCarrStartUnloadingDate.HasValue, d.BookCarrStartUnloadingDate, Nothing),
                        .DeliveryStartUnloadingTime = If(d.BookCarrStartUnloadingTime.HasValue, d.BookCarrStartUnloadingTime, Nothing),
                        .CnsNumber = d.BookConsPrefix,
                        .Control = d.BookControl,
                        .DestinationName = d.BookDestName,
                        .DestinationCity = d.BookDestCity,
                        .DestinationState = d.BookDestState,
                        .OrigName = d.BookOrigName,
                        .OrigState = d.BookOrigState,
                        .OrigCity = d.BookOrigCity,
                        .ProNumber = d.BookProNumber,
                        .WhseAuthorizationNumber = d.BookWhseAuthorizationNo,
                        .CarrierName = d.CarrierName}).ToArray()

                Return oList

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

    Public Function GetBookProNumber(ByVal proNumber As String, ByVal CompanyIDs As DataTransferObjects.IDs()) As String
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim arr As List(Of String) = New List(Of String)()
                For Each item In CompanyIDs
                    arr.Add(item.Control.ToString)
                Next

                Dim pro1 As String = ""
                If arr.Count = 0 Then
                    pro1 = (From d In db.Books Where (d.BookProNumber = proNumber Or d.BookCarrOrderNumber = proNumber) Select d).First.BookProNumber
                Else
                    pro1 = (From d In db.Books Where (d.BookProNumber = proNumber Or d.BookCarrOrderNumber = proNumber) And arr.Contains(d.BookCustCompControl.ToString) Select d).First.BookProNumber
                End If

                'Dim pro = ( _
                'From d In db.spGetBookProNumber(proNumber, CompanyIDs) Select d)

                Return pro1

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

    Public Function AssignTruckStopCarrier(ByVal StopName As String,
                                           ByVal ID1 As String,
                                           ByVal ID2 As String,
                                           ByVal TruckID As String,
                                           ByVal SeqNbr As Integer,
                                           ByVal DistToPrev As Double,
                                           ByVal TotalRouteCost As Double,
                                           ByVal ConsNumber As String) As LTS.udfGetTruckStopCarrierInfoResult

        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oTSCarrierInfo = (From d In db.udfGetTruckStopCarrierInfo(StopName, ID1, ID2, TruckID, SeqNbr, DistToPrev, TotalRouteCost, ConsNumber)
                                      Select d).FirstOrDefault()
                Return oTSCarrierInfo
            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("AssignTruckStopCarrier"))
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Replaces all calls to spUpdateBookingRecord returns a new result object
    ''' , callers need to check MustRecalculate and AddToPicklist flags
    ''' </summary>
    ''' <param name="strOrderNumber"></param>
    ''' <param name="intOrderSequence"></param>
    ''' <param name="intDefCompNumber"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateBookingRecord(ByVal strOrderNumber As String,
                                        ByVal intOrderSequence As Integer,
                                        ByVal intDefCompNumber As Integer) As LTS.spUpdateBookingRecordResult

        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oReturnData = (From d In db.spUpdateBookingRecord(strOrderNumber, intOrderSequence, intDefCompNumber) Select d).FirstOrDefault()
                If Not oReturnData Is Nothing Then
                    Me.UpdateBookingRecordResult = oReturnData
                    Me.LastProcedureName = "spUpdateBookingRecord"
                End If
                Return oReturnData
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateBookingRecord"))
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' writes data from the PO Staging tables to the Book table
    ''' and returns a result object.  the strNewPro parameter is 
    ''' updated with the new pro number assigned to the booking 
    ''' record
    ''' </summary>
    ''' <param name="strOrderNumber"></param>
    ''' <param name="intOrderSequence"></param>
    ''' <param name="intDefCompNumber"></param>
    ''' <param name="strNewPro"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.1 on 04/04/2018
    '''   added logic to use the new LTS versions of spGetNextProBase and spGetProAbrev
    '''   includes logic to use the Legal Entity Pro Seed setting
    ''' </remarks>
    Public Function WriteNewBookingForBatch(ByVal strOrderNumber As String,
                                            ByVal intOrderSequence As Integer,
                                            ByVal intDefCompNumber As Integer,
                                            ByRef strNewPro As String) As LTS.spWriteNewBookingForBatchResult
        'Get the Next Pro Number  Modified by RHR for v-8.1 on 04/04/2018
        Dim strProBase As String = "" '= getScalarString("Exec spGetNextProBase")
        Dim strProAbrev As String = "" '= getScalarString("Exec spGetProAbrev 0," & intDefCompNumber.ToString)
        'strNewPro = strProAbrev & strProBase
        'If String.IsNullOrEmpty(strNewPro) OrElse strNewPro.Trim.Length < 1 Then
        '    throwInvalidKeyFilterMetaDataException(SqlFaultInfo.FaultDetailsKey.E_SystemFaliedToGeneratedKeyField, New List(Of String) From {"Book Pro Number"})
        'End If
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Begin Modified by RHR for v-8.1 on 04/04/2018
                Dim oProBase = db.spGetNextProBase(0, intDefCompNumber).FirstOrDefault()
                If Not oProBase Is Nothing Then
                    strProBase = oProBase.NewProNumber
                End If
                Dim oProAbrev = db.spGetProAbrev(0, intDefCompNumber).FirstOrDefault()
                If Not oProAbrev Is Nothing Then
                    strProAbrev = oProAbrev.ProAbrev
                End If
                strNewPro = strProAbrev & strProBase
                If String.IsNullOrWhiteSpace(strNewPro) Then
                    throwInvalidKeyFilterMetaDataException(SqlFaultInfo.FaultDetailsKey.E_SystemFaliedToGeneratedKeyField, New List(Of String) From {"Book Pro Number"})
                End If
                'End Modified by RHR for v-8.1 on 04/04/2018
                Dim strCNSNumber As String = ""
                Dim oCNSData = db.spGetAutoAssignCNSNumber(intDefCompNumber).FirstOrDefault()
                If Not oCNSData Is Nothing Then
                    strCNSNumber = oCNSData.BookConsNumber
                End If

                Dim oReturnData = (From d In db.spWriteNewBookingForBatch(strNewPro, strProBase, strOrderNumber, intOrderSequence, intDefCompNumber, strCNSNumber) Select d).FirstOrDefault()

                If Not oReturnData Is Nothing Then
                    Me.WriteNewBookingForBatchResult = oReturnData
                    Me.LastProcedureName = "spWriteNewBookingForBatch"
                    Dim sDetails As New List(Of String)
                    If oReturnData.ErrNumber > 10 Then
                        sDetails.Add(LastProcedureName)
                        sDetails.Add(oReturnData.ErrNumber.ToString())
                        sDetails.Add(oReturnData.RetMsg)
                        'The procedure, {0}, returned the following warning: number {1} message {2} 
                        throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_AccessDenied, SqlFaultInfo.FaultDetailsKey.E_SQLWarningDetails, sDetails, SqlFaultInfo.FaultReasons.E_ProcessProcedureFailure, True)
                    ElseIf oReturnData.ErrNumber > 0 Then
                        sDetails.Add(oReturnData.RetMsg)
                        'Server Message: {0}
                        throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_AccessDenied, SqlFaultInfo.FaultDetailsKey.E_ServerMsgDetails, sDetails, SqlFaultInfo.FaultReasons.E_ProcessProcedureFailure, True)
                    End If



                End If
                Return oReturnData
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("WriteNewBookingForBatch"))
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Calls spSilentTenderFinalized and returns a result object.  Caller should check MustRecalculate value and recalculate costs and execute spInsertPickList 
    ''' </summary>
    ''' <param name="strOrderNumber"></param>
    ''' <param name="intOrderSequence"></param>
    ''' <param name="intCustNumber"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SilentTenderFinalized(ByVal strOrderNumber As String, ByVal intOrderSequence As Integer, ByVal intCustNumber As Integer) As LTS.spSilentTenderFinalizedResult
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oReturnData = (From d In db.spSilentTenderFinalized(Left(strOrderNumber, 20), intOrderSequence, intCustNumber) Select d).FirstOrDefault()
                If Not oReturnData Is Nothing Then
                    Me.SilentTenderFinalizedResult = oReturnData
                    Me.LastProcedureName = "spSilentTenderFinalized"
                End If
                Return oReturnData
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SilentTenderFinalized"))
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Checks if a record exists in the BookDeleted table where BookDeletedEDIExported = false for the provided load and carrier.
    ''' If a record exists the procedure updates the BookDeletedEDIExported to true and returns true.  If a record does not 
    ''' exist the procedure returns false.
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="CarrierControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateBookDeletedEDIExported(ByVal BookControl As Integer, ByVal CarrierControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oReturnData = (From d In db.spUpdateBookDeletedEDIExported(BookControl, CarrierControl) Select d).FirstOrDefault()
                If Not oReturnData Is Nothing Then
                    blnRet = If(oReturnData.Column1, False)
                End If
                Return blnRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateBookDeletedEDIExported"))
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' To be called when an order is unfinalized and the BookRouteFinalCode = 'CM'
    ''' returns ErrNumber = 1 with RetMsg = 'Invalid BookSHID could not update deleted booking table record.' 
    ''' as the properties of  spInsertBookDeletedForUnfinalizedResult if the BookSHID has not been set.
    ''' return ErrNumber = 0 on success. 
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function InsertBookDeletedForUnfinalized(ByVal BookControl As Integer) As LTS.spInsertBookDeletedForUnfinalizedResult
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Return db.spInsertBookDeletedForUnfinalized(BookControl, Me.Parameters.UserName).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertBookDeletedForUnfinalized"))
            End Try
        End Using
        Return Nothing
    End Function

    Public Function UpdateBookAPDetailRecord(ByVal oData As DataTransferObjects.BookAPDetail) As DataTransferObjects.BookAPDetail
        Using LinqDB
            With oData
                Try

                    'Open the existing Record
                    Dim d = (From e In CType(LinqDB, NGLMasBookDataContext).Books Where e.BookControl = .BookControl Select e).First
                    If d Is Nothing Then
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_RecordDeleted"}, New FaultReason("E_InvalidOperationException"))
                    Else
                        'Check for conflicts
                        'Modified by RHR for v-8.5.2.006 on 12/29/2022 check for seconds because D365 does not save milliseconds
                        Dim iSeconds = DateDiff(DateInterval.Second, .BookModDate.Value, d.BookModDate.Value)
                        If iSeconds > 0 Then
                            'the data may have changed so check each field for conflicts
                            Dim ConflictData As New List(Of KeyValuePair(Of String, String))
                            Dim blnConflictFound As Boolean = False
                            addToConflicts("BookProNumber", .BookProNumber, d.BookProNumber, ConflictData, blnConflictFound)
                            addToConflicts("BookConsPrefix", .BookConsPrefix, d.BookConsPrefix, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrierControl", .BookCarrierControl, d.BookCarrierControl, ConflictData, blnConflictFound)
                            addToConflicts("BookDateLoad", .BookDateLoad, d.BookDateLoad, ConflictData, blnConflictFound)
                            addToConflicts("BookTranCode", .BookTranCode, d.BookTranCode, ConflictData, blnConflictFound)
                            addToConflicts("BookPayCode", .BookPayCode, d.BookPayCode, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrBLNumber", .BookCarrBLNumber, d.BookCarrBLNumber, ConflictData, blnConflictFound)
                            addToConflicts("BookFinAPBillNumber", .BookFinAPBillNumber, d.BookFinAPBillNumber, ConflictData, blnConflictFound)
                            addToConflicts("BookFinAPBillNoDate", .BookFinAPBillNoDate, d.BookFinAPBillNoDate, ConflictData, blnConflictFound)
                            addToConflicts("BookFinAPBillInvDate", .BookFinAPBillInvDate, d.BookFinAPBillInvDate, ConflictData, blnConflictFound)
                            addToConflicts("BookFinAPActWgt", .BookFinAPActWgt, If(d.BookFinAPActWgt.HasValue, d.BookFinAPActWgt, 0), ConflictData, blnConflictFound)
                            addToConflicts("BookFinAPPayDate", .BookFinAPPayDate, d.BookFinAPPayDate, ConflictData, blnConflictFound)
                            addToConflicts("BookFinAPPayAmt", .BookFinAPPayAmt, If(d.BookFinAPPayAmt.HasValue, d.BookFinAPPayAmt, 0), ConflictData, blnConflictFound)
                            addToConflicts("BookFinAPCheck", .BookFinAPCheck, d.BookFinAPCheck, ConflictData, blnConflictFound)
                            addToConflicts("BookFinAPGLNumber", .BookFinAPGLNumber, d.BookFinAPGLNumber, ConflictData, blnConflictFound)
                            addToConflicts("BookRevTotalCost", .BookRevTotalCost, If(d.BookRevTotalCost.HasValue, d.BookRevTotalCost, 0), ConflictData, blnConflictFound)

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
                        d.BookConsPrefix = .BookConsPrefix
                        d.BookCarrierControl = .BookCarrierControl
                        d.BookDateLoad = .BookDateLoad
                        d.BookPayCode = .BookPayCode
                        d.BookModDate = Date.Now
                        d.BookModUser = Me.Parameters.UserName
                        d.BookCarrBLNumber = .BookCarrBLNumber
                        d.BookFinAPBillNumber = .BookFinAPBillNumber
                        d.BookFinAPBillNoDate = .BookFinAPBillNoDate
                        d.BookFinAPBillInvDate = .BookFinAPBillInvDate
                        d.BookFinAPActWgt = .BookFinAPActWgt
                        d.BookFinAPPayDate = .BookFinAPPayDate
                        d.BookFinAPPayAmt = .BookFinAPPayAmt
                        d.BookFinAPCheck = .BookFinAPCheck
                        d.BookFinAPGLNumber = .BookFinAPGLNumber
                        d.BookRevTotalCost = .BookRevTotalCost
                    End If
                    LinqDB.SubmitChanges()
                Catch ex As FaultException
                    Throw
                Catch ex As SqlException
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
                Catch conflictEx As ChangeConflictException
                    Try
                        Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(LinqDB))
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

            End With
        End Using
        ' Return the updated order
        Return GetBookAPDetailRecord(oData.BookControl)

    End Function

    Public Function UpdateBookAPDetailRecordQuick(ByVal oData As DataTransferObjects.BookAPDetail) As DataTransferObjects.QuickSaveResults
        Using LinqDB
            With oData
                Try

                    'Open the existing Record
                    Dim d = (From e In CType(LinqDB, NGLMasBookDataContext).Books Where e.BookControl = .BookControl Select e).First
                    If d Is Nothing Then
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_RecordDeleted"}, New FaultReason("E_InvalidOperationException"))
                    Else
                        'Check for conflicts
                        'Modified by RHR for v-8.5.2.006 on 12/29/2022 check for seconds because D365 does not save milliseconds
                        Dim iSeconds = DateDiff(DateInterval.Second, .BookModDate.Value, d.BookModDate.Value)
                        If iSeconds > 0 Then
                            'the data may have changed so check each field for conflicts
                            Dim ConflictData As New List(Of KeyValuePair(Of String, String))
                            Dim blnConflictFound As Boolean = False
                            addToConflicts("BookProNumber", .BookProNumber, d.BookProNumber, ConflictData, blnConflictFound)
                            addToConflicts("BookConsPrefix", .BookConsPrefix, d.BookConsPrefix, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrierControl", .BookCarrierControl, d.BookCarrierControl, ConflictData, blnConflictFound)
                            addToConflicts("BookDateLoad", .BookDateLoad, d.BookDateLoad, ConflictData, blnConflictFound)
                            addToConflicts("BookTranCode", .BookTranCode, d.BookTranCode, ConflictData, blnConflictFound)
                            addToConflicts("BookPayCode", .BookPayCode, d.BookPayCode, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrBLNumber", .BookCarrBLNumber, d.BookCarrBLNumber, ConflictData, blnConflictFound)
                            addToConflicts("BookFinAPBillNumber", .BookFinAPBillNumber, d.BookFinAPBillNumber, ConflictData, blnConflictFound)
                            addToConflicts("BookFinAPBillNoDate", .BookFinAPBillNoDate, d.BookFinAPBillNoDate, ConflictData, blnConflictFound)
                            addToConflicts("BookFinAPBillInvDate", .BookFinAPBillInvDate, d.BookFinAPBillInvDate, ConflictData, blnConflictFound)
                            addToConflicts("BookFinAPActWgt", .BookFinAPActWgt, If(d.BookFinAPActWgt.HasValue, d.BookFinAPActWgt, 0), ConflictData, blnConflictFound)
                            addToConflicts("BookFinAPPayDate", .BookFinAPPayDate, d.BookFinAPPayDate, ConflictData, blnConflictFound)
                            addToConflicts("BookFinAPPayAmt", .BookFinAPPayAmt, If(d.BookFinAPPayAmt.HasValue, d.BookFinAPPayAmt, 0), ConflictData, blnConflictFound)
                            addToConflicts("BookFinAPCheck", .BookFinAPCheck, d.BookFinAPCheck, ConflictData, blnConflictFound)
                            addToConflicts("BookFinAPGLNumber", .BookFinAPGLNumber, d.BookFinAPGLNumber, ConflictData, blnConflictFound)
                            addToConflicts("BookRevTotalCost", .BookRevTotalCost, If(d.BookRevTotalCost.HasValue, d.BookRevTotalCost, 0), ConflictData, blnConflictFound)

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
                        d.BookConsPrefix = .BookConsPrefix
                        d.BookCarrierControl = .BookCarrierControl
                        d.BookDateLoad = .BookDateLoad
                        d.BookPayCode = .BookPayCode
                        d.BookModDate = Date.Now
                        d.BookModUser = Me.Parameters.UserName
                        d.BookCarrBLNumber = .BookCarrBLNumber
                        d.BookFinAPBillNumber = .BookFinAPBillNumber
                        d.BookFinAPBillNoDate = .BookFinAPBillNoDate
                        d.BookFinAPBillInvDate = .BookFinAPBillInvDate
                        d.BookFinAPActWgt = .BookFinAPActWgt
                        d.BookFinAPPayDate = .BookFinAPPayDate
                        d.BookFinAPPayAmt = .BookFinAPPayAmt
                        d.BookFinAPCheck = .BookFinAPCheck
                        d.BookFinAPGLNumber = .BookFinAPGLNumber
                        d.BookRevTotalCost = .BookRevTotalCost
                    End If
                    LinqDB.SubmitChanges()
                Catch ex As FaultException
                    Throw
                Catch ex As SqlException
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
                Catch conflictEx As ChangeConflictException
                    Try
                        Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(LinqDB))
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

            End With
        End Using
        ' get the updated order
        Dim oUpdated = GetBookAPDetailRecord(oData.BookControl)
        Dim oRet As New DataTransferObjects.QuickSaveResults
        If Not oUpdated Is Nothing Then
            oRet.Control = oUpdated.BookControl
            oRet.ModDate = oUpdated.BookModDate
            oRet.ModUser = oUpdated.BookModUser
        End If
        Return oRet

    End Function

    Public Sub UpdatePayables70(ByVal CompLegalEntity As String,
                                ByVal CompNumber As Integer,
                                ByVal CompAlphaCode As String,
                                ByVal BookCarrOrderNumber As String,
                                ByVal BookOrderSequence As Integer,
                                ByVal BookProNumber As String,
                                ByVal BookFinAPPayAmt As Double,
                                ByVal BookFinAPPayAmtAllowUpdate As Boolean,
                                ByVal BookFinAPActWgt As Double,
                                ByVal BookFinAPActWgtAllowUpdate As Boolean,
                                ByVal BookFinAPCheck As String,
                                ByVal BookFinAPCheckAllowUpdate As Boolean,
                                ByVal BookFinAPPayDate As String,
                                ByVal BookFinAPPayDateAllowUpdate As Boolean,
                                ByVal BookFinAPBillNumber As String,
                                ByVal BookFinAPBillNumberAllowUpdate As Boolean,
                                ByVal BookFinAPBillInvDate As String,
                                ByVal BookFinAPBillInvDateAllowUpdate As Boolean,
                                ByVal BookFinAPGLNumber As String,
                                ByVal BookFinAPGLNumberAllowUpdate As Boolean,
                                ByVal APGLDescription As String,
                                ByVal APGLDescriptionAllowUpdate As Boolean)


        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oRet = db.spUpdatePayables70(CompLegalEntity, CompNumber, CompAlphaCode, BookCarrOrderNumber, BookOrderSequence, BookProNumber, BookFinAPPayAmt, BookFinAPPayAmtAllowUpdate,
                                                 BookFinAPActWgt, BookFinAPActWgtAllowUpdate, BookFinAPCheck, BookFinAPCheckAllowUpdate, BookFinAPPayDate, BookFinAPPayDateAllowUpdate,
                                                 BookFinAPBillNumber, BookFinAPBillNumberAllowUpdate, BookFinAPBillInvDate, BookFinAPBillInvDateAllowUpdate,
                                                 BookFinAPGLNumber, BookFinAPGLNumberAllowUpdate, APGLDescription, APGLDescriptionAllowUpdate).ToList()
                If Not oRet Is Nothing AndAlso oRet.Count() > 0 AndAlso oRet(0).ErrNumber <> 0 Then
                    throwSQLFaultException(oRet(0).RetMsg)
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdatePayables70"))
            End Try
        End Using
    End Sub


    Public Sub UpdatePayablesByFreightBill(ByVal CompLegalEntity As String,
                                           ByVal CompNumber As Integer,
                                           ByVal CompAlphaCode As String,
                                           ByVal CarrierNumber As Integer,
                                           ByVal CarrierAlphaCode As String,
                                           ByVal APPayAmt As Double,
                                           ByVal APPayAmtAllowUpdate As Boolean,
                                           ByVal APCheck As String,
                                           ByVal APCheckAllowUpdate As Boolean,
                                           ByVal APPayDate As Date?,
                                           ByVal APPayDateAllowUpdate As Boolean,
                                           ByVal APBillNumber As String,
                                           ByVal APBillNumberAllowUpdate As Boolean,
                                           ByVal APGLNumber As String,
                                           ByVal APGLNumberAllowUpdate As Boolean,
                                           ByVal APGLDescription As String,
                                           ByVal APGLDescriptionAllowUpdate As Boolean)


        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oRet = db.spUpdatePayablesByFreightBill(CompLegalEntity,
                                                            CompNumber,
                                                            CompAlphaCode,
                                                            CarrierNumber,
                                                            CarrierAlphaCode,
                                                            APPayAmt,
                                                            APPayAmtAllowUpdate,
                                                            APCheck,
                                                            APCheckAllowUpdate,
                                                            APPayDate,
                                                            APPayDateAllowUpdate,
                                                            APBillNumber,
                                                            APGLNumber,
                                                            APGLNumberAllowUpdate,
                                                            APGLDescription,
                                                            APGLDescriptionAllowUpdate).ToList()
                If Not oRet Is Nothing AndAlso oRet.Count() > 0 AndAlso oRet(0).ErrNumber <> 0 Then
                    throwSQLFaultException(oRet(0).RetMsg)
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdatePayablesByFreightBill"))
            End Try
        End Using
    End Sub

    Public Function GetPayablesTestData70(ByVal CompLegalEntity As String) As List(Of LTS.spGetPayablesTestData70Result)
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oReturnData = (From d In db.spGetPayablesTestData70(CompLegalEntity) Select d).ToList()

                Return oReturnData
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("spGetPayablesTestData70"))
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' returns a list of orders in spPendingOrdersExpiredResult object. If the GlobalDefaultLoadAcceptAllowedMinutes
    ''' parameter setting is less than 1 expired processing is turned off no records are processed and an ErrNumber of 
    ''' 1 is returned.  The caller must check for errors by checking the ErrNumber and RetMsg values.  If an error 
    ''' exists only one record is returned.  The CompControl should match the AllowedMinutes parameter.  If CompControl
    '''  = 0 expired records from all companies are returned.  Generally when companies have different AllowedMinutes 
    ''' specific companies are processed first then a value of zero for CompControl is used to return all the remaining 
    ''' orders with default AllowedMinutes.  If AllowedMinuts is less than 1 the system will use the 
    ''' GlobalDefaultLoadAcceptAllowedMinutes parameter setting.
    ''' </summary>
    ''' <param name="AllowedMinutes"></param>
    ''' <param name="CompControl"></param>
    ''' <param name="WebTenderOnly"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPendingExpiredOrders(ByVal AllowedMinutes As Integer, Optional ByVal CompControl As Integer = 0, Optional ByVal WebTenderOnly As Boolean = False) As List(Of LTS.spPendingOrdersExpiredResult)
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Return db.spPendingOrdersExpired(AllowedMinutes, CompControl, WebTenderOnly).ToList()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetPendingExpiredOrders"))
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Gets the last stop data for a load based on the selected booking record.
    ''' </summary>
    ''' <param name="bookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.5.102 on 11/15/2016
    '''   fixed bug where udfGetTariffSelectionKeys was 
    '''   modified in DB but not in DLL.  updated LTS and 
    '''   added 0 for new parameter
    ''' </remarks>
    Public Function GetLastStopData(ByVal bookControl As Integer) As LTS.udfGetTariffSelectionKeysResult
        Using Logger.StartActivity("GetLastStopData(BookControl: {BookControl}", bookControl)
            Using db As New NGLMasBookDataContext(ConnectionString)
                Try
                    Return db.udfGetTariffSelectionKeys(bookControl, 0).FirstOrDefault
                Catch ex As Exception
                    ManageLinqDataExceptions(ex, buildProcedureName("udfGetTariffSelectionKeys"))
                End Try
            End Using
        End Using
        Return Nothing
    End Function

    Public Function GetOrderChanges(ByVal CompNumber As Integer,
                                    ByVal CompAlphaCode As String,
                                    ByVal CompLegalEntity As String,
                                    ByVal OrderNumber As String,
                                    ByVal OrderSequence As Integer,
                                    ByVal LaneNumber As String,
                                    ByVal POStatusFlag As Integer,
                                    ByVal TotalCases As Integer,
                                    ByVal TotalItems As Integer,
                                    ByVal TotalPL As Double,
                                    ByVal TotalWgt As Double,
                                    ByVal DateLoad As Date?,
                                    ByVal DateOrdered As Date?,
                                    ByVal DateRequired As Date?,
                                    ByVal TransType As String,
                                    ByVal CommCodeType As String,
                                    ByVal ModeType As Integer,
                                    Optional ByVal TestTransType As Boolean = 1,
                                    Optional ByVal TestModeType As Boolean = 1,
                                    Optional ByVal OrigAddress1 As String = "",
                                    Optional ByVal OrigCity As String = "",
                                    Optional ByVal OrigState As String = "",
                                    Optional ByVal OrigCountry As String = "",
                                    Optional ByVal OrigZip As String = "",
                                    Optional ByVal DestAddress1 As String = "",
                                    Optional ByVal DestCity As String = "",
                                    Optional ByVal DestState As String = "",
                                    Optional ByVal DestCountry As String = "",
                                    Optional ByVal DestZip As String = "",
                                    Optional ByVal User1 As String = "",
                                    Optional ByVal User2 As String = "",
                                    Optional ByVal User3 As String = "",
                                    Optional ByVal User4 As String = "") As LTS.spGetOrderChangesResult
        Dim oReturn As New LTS.spGetOrderChangesResult
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oList = db.spGetOrderChanges(CompNumber, CompAlphaCode, CompLegalEntity, OrderNumber, OrderSequence, LaneNumber, POStatusFlag, TotalCases, TotalItems, TotalPL, TotalWgt, DateLoad, DateOrdered, DateRequired, TransType, TestTransType, CommCodeType, ModeType, TestModeType, OrigAddress1, OrigCity, OrigState, OrigCountry, OrigZip, DestAddress1, DestCity, DestState, DestCountry, DestZip, User1, User2, User3, User4).ToList()
                If Not oList Is Nothing AndAlso oList.Count() > 0 AndAlso Not oList(0) Is Nothing Then
                    oReturn = oList(0)
                End If

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetOrderChanges"))
            End Try

            Return oReturn

        End Using
    End Function


    Public Function GetOrderPODownloadChanges(ByVal CompNumber As Integer,
                                              ByVal CompAlphaCode As String,
                                              ByVal CompLegalEntity As String,
                                              ByVal OrderNumber As String,
                                              ByVal OrderSequence As Integer,
                                              ByVal LaneNumber As String,
                                              ByVal POStatusFlag As Integer,
                                              ByVal TotalCases As Integer,
                                              ByVal TotalItems As Integer,
                                              ByVal TotalPL As Double,
                                              ByVal TotalWgt As Double,
                                              ByVal DateLoad As Date?,
                                              ByVal DateOrdered As Date?,
                                              ByVal DateRequired As Date?,
                                              ByVal TransType As String,
                                              ByVal CommCodeType As String,
                                              ByVal ModeType As Integer,
                                              Optional ByVal TestTransType As Boolean = 1,
                                              Optional ByVal TestModeType As Boolean = 1,
                                              Optional ByVal OrigAddress1 As String = "",
                                              Optional ByVal OrigCity As String = "",
                                              Optional ByVal OrigState As String = "",
                                              Optional ByVal OrigCountry As String = "",
                                              Optional ByVal OrigZip As String = "",
                                              Optional ByVal DestAddress1 As String = "",
                                              Optional ByVal DestCity As String = "",
                                              Optional ByVal DestState As String = "",
                                              Optional ByVal DestCountry As String = "",
                                              Optional ByVal DestZip As String = "",
                                              Optional ByVal User1 As String = "",
                                              Optional ByVal User2 As String = "",
                                              Optional ByVal User3 As String = "",
                                              Optional ByVal User4 As String = "") As LTS.spGetOrderPODownloadChangesResult
        Dim oReturn As New LTS.spGetOrderPODownloadChangesResult
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oList = db.spGetOrderPODownloadChanges(CompNumber, CompAlphaCode, CompLegalEntity, OrderNumber, OrderSequence, LaneNumber, POStatusFlag, TotalCases, TotalItems, TotalPL, TotalWgt, DateLoad, DateOrdered, DateRequired, TransType, TestTransType, CommCodeType, ModeType, TestModeType, OrigAddress1, OrigCity, OrigState, OrigCountry, OrigZip, DestAddress1, DestCity, DestState, DestCountry, DestZip, User1, User2, User3, User4).ToList()
                If Not oList Is Nothing AndAlso oList.Count() > 0 AndAlso Not oList(0) Is Nothing Then
                    oReturn = oList(0)
                End If

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetOrderPODownloadChanges"))
            End Try

            Return oReturn

        End Using
    End Function

    ''' <summary>
    ''' Returns a list of orders where the required date has been modified by users
    ''' an empty list is returned if nothing has changed
    ''' </summary>
    ''' <param name="OrderNumber"></param>
    ''' <param name="CompNumber"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.0 on 3/13/2017
    '''   new procedure used to return references to orders by company where Required Dates were modified by users
    ''' </remarks>
    Public Function GetTMSModifiedRequiredDate(ByVal OrderNumber As String, ByVal CompNumber As Integer) As List(Of LTS.spGetTMSModifiedRequiredDateResult)
        Dim oResults As New List(Of LTS.spGetTMSModifiedRequiredDateResult)
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                oResults = db.spGetTMSModifiedRequiredDate(OrderNumber, CompNumber).ToList()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetTMSModifiedRequiredDate"))
            End Try
        End Using
        Return oResults
    End Function

    Public Function GetAcceptRejectEmailsFromComp(ByVal BookControl As Integer) As Dictionary(Of String, String)
        Dim dictCompEmails As New Dictionary(Of String, String)

        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'db.Log = New DebugTextWriter
                Dim oDLO As New DataLoadOptions
                oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.CompRefBook)
                Dim oEmails = (From d In db.Books Where d.BookControl = BookControl Select d.CompRefBook.CompRejectedLoadsTo, d.CompRefBook.CompRejectedLoadsCc, d.CompRefBook.CompExpiredLoadsTo, d.CompRefBook.CompExpiredLoadsCc, d.CompRefBook.CompAcceptedLoadsTo, d.CompRefBook.CompAcceptedLoadsCc).FirstOrDefault()
                If Not oEmails Is Nothing Then
                    dictCompEmails.Add("CompRejectedLoadsTo", oEmails.CompRejectedLoadsTo)
                    dictCompEmails.Add("CompRejectedLoadsCc", oEmails.CompRejectedLoadsCc)


                    dictCompEmails.Add("CompExpiredLoadsTo", oEmails.CompExpiredLoadsTo)
                    dictCompEmails.Add("CompExpiredLoadsCc", oEmails.CompExpiredLoadsCc)


                    dictCompEmails.Add("CompAcceptedLoadsTo", oEmails.CompAcceptedLoadsTo)
                    dictCompEmails.Add("CompAcceptedLoadsCc", oEmails.CompAcceptedLoadsCc)


                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAcceptRejectEmailsFromComp"))
            End Try

        End Using
        Return dictCompEmails
    End Function

    Public Sub SaveBookMiles(ByVal BookControl As Integer, ByVal BenchMiles As Double)
        Using operation = Logger.StartActivity("SaveBookMiles(BookControl: {BookControl}, BenchMiles: {BenchMiles}", BookControl, BenchMiles)
            Using db As New NGLMasBookDataContext(ConnectionString)
                Try
                    Dim oBook = db.Books.Where(Function(x) x.BookControl = BookControl).FirstOrDefault()
                    If oBook Is Nothing Then Return
                    oBook.BookMilesFrom = BenchMiles
                    db.SubmitChanges()
                Catch ex As Exception
                    operation.Complete(LogEventLevel.Error, ex)

                    ManageLinqDataExceptions(ex, buildProcedureName("SaveBookMiles"), db)
                End Try
            End Using
        End Using
    End Sub


    Public Function GetBillToForBook(ByVal iBookControl As Integer) As Models.AddressBook
        Dim oRetData As New Models.AddressBook
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Get the comp control for book
                Dim iCompControl As Integer = db.Books.Where(Function(x) x.BookControl = iBookControl).Select(Function(x) x.BookCustCompControl).FirstOrDefault()
                'Get the company Data
                Dim oComp As LTS.CompRefBook = db.CompRefBooks.Where(Function(x) x.CompControl = iCompControl).FirstOrDefault()
                If (oComp.CompFinBillToCompControl IsNot Nothing) Then
                    Dim oBillToComp As LTS.CompRefBook = db.CompRefBooks.Where(Function(x) x.CompControl = oComp.CompFinBillToCompControl).FirstOrDefault()
                    If (oBillToComp IsNot Nothing) Then
                        oRetData.Address1 = oBillToComp.CompStreetAddress1
                        oRetData.Address2 = oBillToComp.CompStreetAddress2
                        oRetData.City = oBillToComp.CompStreetCity
                        oRetData.State = oBillToComp.CompStreetState
                        oRetData.Zip = oBillToComp.CompStreetZip
                        oRetData.Country = oBillToComp.CompStreetCountry
                        oRetData.Name = oBillToComp.CompName
                    End If
                Else
                    oRetData.Address1 = oComp.CompStreetAddress1
                    oRetData.Address2 = oComp.CompStreetAddress2
                    oRetData.City = oComp.CompStreetCity
                    oRetData.State = oComp.CompStreetState
                    oRetData.Zip = oComp.CompStreetZip
                    oRetData.Country = oComp.CompStreetCountry
                    oRetData.Name = oComp.CompName
                End If

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetBillToForBook"))
            End Try

            Return oRetData

        End Using
    End Function


#Region "     Data Validation Methods"

    Public Function IsCarrierContactUsed(ByVal CarrContControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try

                'Check if a record exists
                Dim Book = (
                        From d In db.Books
                        Where
                        (d.BookCarrierContControl = CarrContControl)
                        Select d.BookControl).First

                'If We get here a record exists to return true
                Return True

            Catch ex As System.Data.SqlClient.SqlException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            Catch ex As Exception
                Throw
            End Try

            Return blnRet

        End Using
    End Function

    Public Function DoesLaneExistInBook(ByVal LaneControl As Integer) As Boolean
        Dim blnRet As Boolean = True
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                blnRet = db.Books.Any(Function(x) x.BookODControl = LaneControl)
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DoesLaneExistInBook"), db)
            End Try
            Return blnRet
        End Using
    End Function

#End Region

#Region "     Old NGL Stored Procedure Wrappers Or Direct SQL Queries"

    Public Sub UpdateBookConsPickNumber(ByVal BookConsPrefix As String)
        If BookConsPrefix.Trim.Length > 1 Then
            Dim strProcName As String = "dbo.spUpdateBookConsPickNumber"
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@BookConsPrefix", BookConsPrefix)
            oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
            runNGLStoredProcedure(oCmd, strProcName, 0)
        End If
    End Sub

    Public Sub UpdateOptimizedStopData(ByVal BookLoadControl As Integer,
                                       ByVal SeqNbr As Integer,
                                       ByVal DistToPrev As Double,
                                       ByVal ConsNumber As String)

        Dim strProcName As String = "dbo.spUpdateOptimizedStopData50"
        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@BookLoadControl", BookLoadControl)
        oCmd.Parameters.AddWithValue("@SeqNbr", SeqNbr)
        oCmd.Parameters.AddWithValue("@DistToPrev", DistToPrev)
        oCmd.Parameters.AddWithValue("@ConsNumber", Left(ConsNumber, 20))
        oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        runNGLStoredProcedure(oCmd, strProcName, 0)

    End Sub

    Public Sub UpdateTruckStopData(ByVal StopName As String,
                                   ByVal ID1 As String,
                                   ByVal ID2 As String,
                                   ByVal TruckID As String,
                                   ByVal SeqNbr As Integer,
                                   ByVal DistToPrev As Double,
                                   ByVal TotalRouteCost As Double,
                                   ByVal ConsNumber As String)

        Dim strProcName As String = "dbo.spUpdateTruckStopData50"
        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@StopName", Left(StopName, 20))
        oCmd.Parameters.AddWithValue("@ID1", Left(ID1, 15))
        oCmd.Parameters.AddWithValue("@ID2", Left(ID2, 10))
        oCmd.Parameters.AddWithValue("@TruckID", Left(TruckID, 25))
        oCmd.Parameters.AddWithValue("@SeqNbr", SeqNbr)
        oCmd.Parameters.AddWithValue("@DistToPrev", DistToPrev)
        oCmd.Parameters.AddWithValue("@TotalRouteCost", TotalRouteCost)
        oCmd.Parameters.AddWithValue("@ConsNumber", Left(ConsNumber, 20))
        oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        runNGLStoredProcedure(oCmd, strProcName, 0)

    End Sub

    ''' <summary>
    ''' clear all carrier data and costs for a booking or CNS 
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="ClearCNS"></param>
    ''' <remarks>verified to work with 7.0 by RHR 9/16/13</remarks>
    Public Sub ClearCarrierCons(ByVal BookControl As Integer, ByVal ClearCNS As Boolean)

        Dim strProcName As String = "dbo.spClearCarrierCons"
        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@BookControl", BookControl)
        oCmd.Parameters.AddWithValue("@ClearCNS", ClearCNS)
        runNGLStoredProcedure(oCmd, strProcName, 0)

    End Sub

    Public Function unHoldFiltered(ByVal Filters As String) As Boolean
        Dim blnRet As Boolean = False
        'Create a data connection 
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim oConsolidation As New List(Of vConsolidation)

        Try
            Dim strComp As String = ""
            If Not String.IsNullOrEmpty(Filters) AndAlso Filters.Trim.Length > 1 Then
                strComp = " And "
            End If
            strComp &= "( " _
                       & "(isnull((SELECT top 1 isnull(dbo.UserAdmin.UserAdminCompControl,0) FROM dbo.UserAdmin Where dbo.UserAdmin.UserAdminUserName = '" & Me.Parameters.UserName & "'),0) > 0" _
                       & "	AND " _
                       & " CompNumber In (SELECT dbo.UserAdmin.UserAdminCompControl FROM dbo.UserAdmin	Where dbo.UserAdmin.UserAdminUserName = '" & Me.Parameters.UserName & "')" _
                       & ")" _
                       & " OR " _
                       & " isnull((SELECT top 1 isnull(dbo.UserAdmin.UserAdminCompControl,0) FROM dbo.UserAdmin Where dbo.UserAdmin.UserAdminUserName = '" & Me.Parameters.UserName & "'),0) = 0" _
                       & ") "
            Dim strSQL As String = "Update dbo.Book set BookHoldLoad = 0 from dbo.Book inner join dbo.Comp on dbo.Book.BookCustCompControl = dbo.Comp.CompControl inner join dbo.Lane on dbo.Book.BookODControl = dbo.Lane.LaneControl where " & Filters & strComp
            Dim oQRet As Ngl.Core.Data.QueryResult = oQuery.Execute(strSQL)
            If Not oQRet.Exception Is Nothing Then
                Utilities.SaveAppError(oQRet.Exception.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
            End If
            blnRet = True
        Catch ex As FaultException
            Throw
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As InvalidOperationException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            oQuery = Nothing
        End Try

        Return blnRet

    End Function

    Public Function GetvOptimizationStopData(ByVal Filters As String) As DataTransferObjects.vOptimizationStop()
        'Create a data connection 
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim oStopData As New List(Of vOptimizationStop)

        Try
            Dim strComp As String = ""
            If Not String.IsNullOrEmpty(Filters) AndAlso Filters.Trim.Length > 1 Then
                strComp = " AND "
            End If
            strComp &= "( " _
                       & "(isnull((SELECT top 1 isnull(dbo.UserAdmin.UserAdminCompControl,0) FROM dbo.UserAdmin Where dbo.UserAdmin.UserAdminUserName = '" & Me.Parameters.UserName & "'),0) > 0" _
                       & "	AND " _
                       & " CompNumber In (SELECT dbo.UserAdmin.UserAdminCompControl FROM dbo.UserAdmin	Where dbo.UserAdmin.UserAdminUserName = '" & Me.Parameters.UserName & "')" _
                       & ")" _
                       & " OR " _
                       & " isnull((SELECT top 1 isnull(dbo.UserAdmin.UserAdminCompControl,0) FROM dbo.UserAdmin Where dbo.UserAdmin.UserAdminUserName = '" & Me.Parameters.UserName & "'),0) = 0" _
                       & ") "
            Dim strSQL As String = "Select Top 500 * from Consolidation Where " & Filters & strComp & " And isnull([BookHoldLoad],0) = 0  Order By BookOrigState, BookOrigCity, BookOrigZip, BookDateRequired, BookProNumber "
            Dim oQRet As Ngl.Core.Data.QueryResult = oQuery.ExecuteWithFill(strSQL)
            If Not oQRet.Exception Is Nothing Then
                Utilities.SaveAppError(oQRet.Exception.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
            End If
            If Not oQRet.Data Is Nothing AndAlso oQRet.Data.Rows.Count > 0 Then
                For Each oRow As System.Data.DataRow In oQRet.Data.Rows
                    Dim oItem As New DataTransferObjects.vOptimizationStop
                    With oItem
                        .BookControl = DataTransformation.getDataRowValue(oRow, "BookControl", 0)
                        .BookDateLoad = DataTransformation.getDataRowValue(oRow, "BookDateLoad")
                        .BookStopNo = DataTransformation.getDataRowValue(oRow, "BookStopNo", 0)
                        .BookTranCode = DataTransformation.getDataRowValue(oRow, "BookTranCode", "")
                        .BookDateRequired = DataTransformation.getDataRowValue(oRow, "BookDateRequired")
                        .BookConsPrefix = DataTransformation.getDataRowValue(oRow, "BookConsPrefix", "")
                        .BookProNumber = DataTransformation.getDataRowValue(oRow, "BookProNumber", "")
                        .CompControl = DataTransformation.getDataRowValue(oRow, "CompControl", 0)
                        .CompNumber = DataTransformation.getDataRowValue(oRow, "CompNumber", 0)
                        .CompName = DataTransformation.getDataRowValue(oRow, "CompName", "")
                        .BookDestName = DataTransformation.getDataRowValue(oRow, "BookDestName", "")
                        .BookDestCity = DataTransformation.getDataRowValue(oRow, "BookDestCity", "")
                        .BookDestState = DataTransformation.getDataRowValue(oRow, "BookDestState", "")
                        .BookDestZip = DataTransformation.getDataRowValue(oRow, "BookDestZip", "")
                        .BookODControl = DataTransformation.getDataRowValue(oRow, "BookODControl", 0)
                        .BookLoadCaseQty = DataTransformation.getDataRowValue(oRow, "BookLoadCaseQty", 0)
                        .BookLoadWgt = DataTransformation.getDataRowValue(oRow, "BookLoadWgt", 0)
                        .BookLoadCube = DataTransformation.getDataRowValue(oRow, "BookLoadCube", 0)
                        .BookLoadPL = DataTransformation.getDataRowValue(oRow, "BookLoadPL", 0)
                        .BookLoadPX = DataTransformation.getDataRowValue(oRow, "BookLoadPX", 0)
                        .BookDestAddress1 = DataTransformation.getDataRowValue(oRow, "BookDestAddress1", "")
                        .BookDestAddress2 = DataTransformation.getDataRowValue(oRow, "BookDestAddress2", "")
                        .LaneLatitude = DataTransformation.getDataRowValue(oRow, "LaneLatitude", 0)
                        .LaneLongitude = DataTransformation.getDataRowValue(oRow, "LaneLongitude", 0)
                        .SpecialCodes = DataTransformation.getDataRowValue(oRow, "SpecialCodes", "")
                        .LaneFixedTime = DataTransformation.getDataRowValue(oRow, "LaneFixedTime", "")
                        .BookLoadControl = DataTransformation.getDataRowValue(oRow, "BookLoadControl", 0)
                        .BookHoldLoad = DataTransformation.getDataRowValue(oRow, "BookHoldLoad", 0)
                        .LaneControl = DataTransformation.getDataRowValue(oRow, "LaneControl", 0)
                        .ActualWgt = DataTransformation.getDataRowValue(oRow, "ActualWgt", 0)
                        .LaneNumber = DataTransformation.getDataRowValue(oRow, "LaneNumber", "")
                        .BookCarrOrderNumber = DataTransformation.getDataRowValue(oRow, "BookCarrOrderNumber", "")
                        .BookFinARInvoiceDate = DataTransformation.getDataRowValue(oRow, "BookFinARInvoiceDate")
                        .BookDateOrdered = DataTransformation.getDataRowValue(oRow, "BookDateOrdered")
                        .BookSHID = DataTransformation.getDataRowValue(oRow, "BookSHID")
                    End With
                    oStopData.Add(oItem)
                Next
            Else
                oStopData.Add(New DataTransferObjects.vOptimizationStop)
            End If
            Return oStopData.ToArray()
        Catch ex As FaultException
            Throw
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As InvalidOperationException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            oQuery = Nothing
        End Try

        Return Nothing
    End Function

    Friend Function selectDTOData(ByVal d As LTS.spResetToNStatusResult, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.vOptimizationStop

        Dim oDTO As New DataTransferObjects.vOptimizationStop

        Dim skipObjs As New List(Of String) From {"Page", "Pages", "RecordCount", "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oDTO

    End Function

    Friend Function selectDTOData(ByVal d As LTS.spResetToNStatusOptHasSHIDResult, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.vOptimizationStop

        Dim oDTO As New DataTransferObjects.vOptimizationStop

        Dim skipObjs As New List(Of String) From {"Page", "Pages", "RecordCount", "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oDTO

    End Function



    Public Function ResetToNStatusSP(ByVal UserName As String, ByVal Filters As String, ByVal CompControlFilter As Integer) As DataTransferObjects.vOptimizationStop()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Run the sp to Reset records to N Status and get the results
                Return (From d In db.spResetToNStatus(UserName, Filters, CompControlFilter) Select selectDTOData(d)).ToArray()

                'Return oReturnData
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("ResetToNStatusSP"))
            End Try
        End Using

        Return Nothing

    End Function

    Public Function ResetToNStatusSPOptHasSHID(ByVal UserName As String, ByVal Filters As String, ByVal CompControlFilter As Integer) As DataTransferObjects.vOptimizationStop()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Run the sp to Reset records to N Status and get the results
                Return (From d In db.spResetToNStatusOptHasSHID(UserName, Filters, CompControlFilter) Select selectDTOData(d)).ToArray()

                'Return oReturnData
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("ResetToNStatusSPOptHasSHID"))
            End Try
        End Using

        Return Nothing

    End Function

    Public Sub CreateSampleOrdersOptUnitTest()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oRet = db.spCreateSampleOrdersOptUnitTest().ToList()
                If Not oRet Is Nothing AndAlso oRet.Count() > 0 AndAlso oRet(0).ErrNumber <> 0 Then
                    throwSQLFaultException(oRet(0).RetMsg)
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("CreateSampleOrdersOptUnitTest"))
            End Try
        End Using

    End Sub

    Public Sub DeleteSampleOrdersOptUnitTest()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oRet = db.spDeleteSampleOrdersOptUnitTest().ToList()
                If Not oRet Is Nothing AndAlso oRet.Count() > 0 AndAlso oRet(0).ErrNumber <> 0 Then
                    throwSQLFaultException(oRet(0).RetMsg)
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteSampleOrdersOptUnitTest"))
            End Try
        End Using

    End Sub


    Public Function GetvLoadStatusBoardData(ByVal Filters As String) As DataTransferObjects.vLoadStatusBoard()
        'Create a data connection 
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim oLoadStatusBoard As New List(Of vLoadStatusBoard)

        Try
            Dim strComp As String = ""
            If Not String.IsNullOrEmpty(Filters) AndAlso Filters.Trim.Length > 1 Then
                strComp = " AND "
            End If
            strComp &= "( " _
                       & "(isnull((SELECT top 1 isnull(dbo.UserAdmin.UserAdminCompControl,0) FROM dbo.UserAdmin Where dbo.UserAdmin.UserAdminUserName = '" & Me.Parameters.UserName & "'),0) > 0" _
                       & "	AND " _
                       & " CompNumber In (SELECT dbo.UserAdmin.UserAdminCompControl FROM dbo.UserAdmin	Where dbo.UserAdmin.UserAdminUserName = '" & Me.Parameters.UserName & "')" _
                       & ")" _
                       & " OR " _
                       & " isnull((SELECT top 1 isnull(dbo.UserAdmin.UserAdminCompControl,0) FROM dbo.UserAdmin Where dbo.UserAdmin.UserAdminUserName = '" & Me.Parameters.UserName & "'),0) = 0" _
                       & ") "

            Dim strSQL As String = "SELECT Top 500 BookControl, BookDateLoad, BookDateRequired, BookOrigZip, BookOrigCity, BookOrigState, BookConsPrefix, BookProNumber, " _
                                   & " BookCustCompControl, CompName, BookOrigName, BookDestName, BookDestCity, BookDestState, BookDestZip, BookTotalCases, BookTotalWgt, " _
                                   & " BookTotalPL, BookTotalCube, BookTotalPX, BookTotalBFC, BookCarrActDate, BookFinARInvoiceDate, BookODControl, BookCarrierControl, CarrierName, BookTranCode, " _
                                   & " BookPayCode, BookModDate, BookModUser, BookStopNo, BookRevBilledBFC, BookRevCarrierCost, BookRevOtherCost, BookRevTotalCost, " _
                                   & " BookMilesFrom , BookRouteConsFlag,BookCarrierContact , BookCarrierContactPhone,BookTypeCode,BookCarrOrderNumber,BookModeTypeControl,BookOrderSequence " _
                                   & " FROM Book, Comp, Carrier " _
                                   & " WHERE Book.BookCustCompControl = Comp.CompControl and Book.BookCarrierControl = Carrier.CarrierControl " & strComp & " and " & Filters

            Dim oQRet As Ngl.Core.Data.QueryResult = oQuery.ExecuteWithFill(strSQL)
            If Not oQRet.Exception Is Nothing Then
                Utilities.SaveAppError(oQRet.Exception.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
            End If
            If Not oQRet.Data Is Nothing AndAlso oQRet.Data.Rows.Count > 0 Then
                For Each oRow As System.Data.DataRow In oQRet.Data.Rows
                    Dim oItem As New DataTransferObjects.vLoadStatusBoard
                    With oItem
                        .BookControl = DataTransformation.getDataRowValue(oRow, "BookControl", 0)
                        .BookDateLoad = DataTransformation.getDataRowValue(oRow, "BookDateLoad")
                        .BookDateRequired = DataTransformation.getDataRowValue(oRow, "BookDateRequired")
                        .BookOrigZip = DataTransformation.getDataRowValue(oRow, "BookOrigZip", "")
                        .BookOrigCity = DataTransformation.getDataRowValue(oRow, "BookOrigCity", "")
                        .BookOrigState = DataTransformation.getDataRowValue(oRow, "BookOrigState", "")
                        .BookConsPrefix = DataTransformation.getDataRowValue(oRow, "BookConsPrefix", "")
                        .BookProNumber = DataTransformation.getDataRowValue(oRow, "BookProNumber", "")
                        .BookCustCompControl = DataTransformation.getDataRowValue(oRow, "BookCustCompControl", 0)
                        .CompName = DataTransformation.getDataRowValue(oRow, "CompName", "")
                        .BookOrigName = DataTransformation.getDataRowValue(oRow, "BookOrigName", "")
                        .BookDestName = DataTransformation.getDataRowValue(oRow, "BookDestName", "")
                        .BookDestCity = DataTransformation.getDataRowValue(oRow, "BookDestCity", "")
                        .BookDestState = DataTransformation.getDataRowValue(oRow, "BookDestState", "")
                        .BookDestZip = DataTransformation.getDataRowValue(oRow, "BookDestZip", "")
                        .BookTotalCases = DataTransformation.getDataRowValue(oRow, "BookTotalCases", 0)
                        .BookTotalWgt = DataTransformation.getDataRowValue(oRow, "BookTotalWgt", 0)
                        .BookTotalPL = DataTransformation.getDataRowValue(oRow, "BookTotalPL", 0)
                        .BookTotalCube = DataTransformation.getDataRowValue(oRow, "BookTotalCube", 0)
                        .BookTotalPX = DataTransformation.getDataRowValue(oRow, "BookTotalPX", 0)
                        .BookTotalBFC = DataTransformation.getDataRowValue(oRow, "BookTotalBFC", 0)
                        .BookCarrActDate = DataTransformation.getDataRowValue(oRow, "BookCarrActDate")
                        .BookFinARInvoiceDate = DataTransformation.getDataRowValue(oRow, "BookFinARInvoiceDate")
                        .BookODControl = DataTransformation.getDataRowValue(oRow, "BookODControl", 0)
                        .BookCarrierControl = DataTransformation.getDataRowValue(oRow, "BookCarrierControl", 0)
                        .CarrierName = DataTransformation.getDataRowValue(oRow, "CarrierName", "")
                        .BookTranCode = DataTransformation.getDataRowValue(oRow, "BookTranCode", "")
                        .BookPayCode = DataTransformation.getDataRowValue(oRow, "BookPayCode", "")
                        .BookStopNo = DataTransformation.getDataRowValue(oRow, "BookStopNo", 0)
                        .BookModDate = DataTransformation.getDataRowValue(oRow, "BookModDate")
                        .BookModUser = DataTransformation.getDataRowValue(oRow, "BookModUser", "")
                        .BookRevBilledBFC = DataTransformation.getDataRowValue(oRow, "BookRevBilledBFC", 0)
                        .BookCarrOrderNumber = DataTransformation.getDataRowValue(oRow, "BookCarrOrderNumber", "")
                        .BookRevCarrierCost = DataTransformation.getDataRowValue(oRow, "BookRevCarrierCost", 0)
                        .BookRevOtherCost = DataTransformation.getDataRowValue(oRow, "BookRevOtherCost", 0)
                        .BookRevTotalCost = DataTransformation.getDataRowValue(oRow, "BookRevTotalCost", 0)
                        .BookMilesFrom = DataTransformation.getDataRowValue(oRow, "BookMilesFrom", 0)
                        .BookRouteConsFlag = DataTransformation.getDataRowValue(oRow, "BookRouteConsFlag", 0)
                        .BookCarrierContact = DataTransformation.getDataRowValue(oRow, "BookCarrierContact", "")
                        .BookCarrierContactPhone = DataTransformation.getDataRowValue(oRow, "BookCarrierContactPhone", "")
                        .BookTypeCode = DataTransformation.getDataRowValue(oRow, "BookTypeCode", "")
                        .BookCarrOrderNumber = DataTransformation.getDataRowValue(oRow, "BookCarrOrderNumber", "")
                        .BookModeTypeControl = DataTransformation.getDataRowValue(oRow, "BookModeTypeControl", 0)
                        .BookOrderSequence = DataTransformation.getDataRowValue(oRow, "BookOrderSequence", 0)
                        .BookDateOrdered = DataTransformation.getDataRowValue(oRow, "BookDateOrdered")
                    End With
                    oLoadStatusBoard.Add(oItem)
                Next
            Else
                oLoadStatusBoard.Add(New DataTransferObjects.vLoadStatusBoard)
            End If
            Return oLoadStatusBoard.ToArray()
        Catch ex As FaultException
            Throw
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As InvalidOperationException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            oQuery = Nothing
        End Try

        Return Nothing
    End Function

    Public Function GetvConsolidationData(ByVal Filters As String) As DataTransferObjects.vConsolidation()
        'Create a data connection 
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim oConsolidation As New List(Of vConsolidation)

        Try
            Dim strComp As String = ""
            If Not String.IsNullOrEmpty(Filters) AndAlso Filters.Trim.Length > 1 Then
                strComp = " AND "
            End If
            strComp &= "( " _
                       & "(isnull((SELECT top 1 isnull(dbo.UserAdmin.UserAdminCompControl,0) FROM dbo.UserAdmin Where dbo.UserAdmin.UserAdminUserName = '" & Me.Parameters.UserName & "'),0) > 0" _
                       & "	AND " _
                       & " CompNumber In (SELECT dbo.UserAdmin.UserAdminCompControl FROM dbo.UserAdmin	Where dbo.UserAdmin.UserAdminUserName = '" & Me.Parameters.UserName & "')" _
                       & ")" _
                       & " OR " _
                       & " isnull((SELECT top 1 isnull(dbo.UserAdmin.UserAdminCompControl,0) FROM dbo.UserAdmin Where dbo.UserAdmin.UserAdminUserName = '" & Me.Parameters.UserName & "'),0) = 0" _
                       & ") "
            Dim strSQL As String = "Select Top 500 * from Consolidation Where " & Filters & strComp & " Order By BookOrigState, BookOrigCity, BookOrigZip, BookDateRequired, BookProNumber "
            Dim oQRet As Ngl.Core.Data.QueryResult = oQuery.ExecuteWithFill(strSQL)
            If Not oQRet.Exception Is Nothing Then
                Utilities.SaveAppError(oQRet.Exception.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
            End If
            If Not oQRet.Data Is Nothing AndAlso oQRet.Data.Rows.Count > 0 Then
                For Each oRow As System.Data.DataRow In oQRet.Data.Rows
                    Dim oItem As New DataTransferObjects.vConsolidation
                    With oItem
                        .BookControl = DataTransformation.getDataRowValue(oRow, "BookControl", 0)
                        .BookDateLoad = DataTransformation.getDataRowValue(oRow, "BookDateLoad")
                        .BookStopNo = DataTransformation.getDataRowValue(oRow, "BookStopNo", 0)
                        .BookTranCode = DataTransformation.getDataRowValue(oRow, "BookTranCode", "")
                        .BookOrigState = DataTransformation.getDataRowValue(oRow, "BookOrigState", "")
                        .BookOrigCity = DataTransformation.getDataRowValue(oRow, "BookOrigCity", "")
                        .BookOrigZip = DataTransformation.getDataRowValue(oRow, "BookOrigZip", "")
                        .BookDateRequired = DataTransformation.getDataRowValue(oRow, "BookDateRequired")
                        .BookConsPrefix = DataTransformation.getDataRowValue(oRow, "BookConsPrefix", "")
                        .BookProNumber = DataTransformation.getDataRowValue(oRow, "BookProNumber", "")
                        .CompControl = DataTransformation.getDataRowValue(oRow, "CompControl", 0)
                        .CompNumber = DataTransformation.getDataRowValue(oRow, "CompNumber", 0)
                        .CompName = DataTransformation.getDataRowValue(oRow, "CompName", "")
                        .BookOrigName = DataTransformation.getDataRowValue(oRow, "BookOrigName", "")
                        .BookDestName = DataTransformation.getDataRowValue(oRow, "BookDestName", "")
                        .BookDestCity = DataTransformation.getDataRowValue(oRow, "BookDestCity", "")
                        .BookDestState = DataTransformation.getDataRowValue(oRow, "BookDestState", "")
                        .BookDestZip = DataTransformation.getDataRowValue(oRow, "BookDestZip", "")
                        .BookTotalCases = DataTransformation.getDataRowValue(oRow, "BookTotalCases", 0)
                        .BookTotalWgt = DataTransformation.getDataRowValue(oRow, "BookTotalWgt", 0)
                        .BookTotalPL = DataTransformation.getDataRowValue(oRow, "BookTotalPL", 0)
                        .BookTotalCube = DataTransformation.getDataRowValue(oRow, "BookTotalCube", 0)
                        .BookTotalPX = DataTransformation.getDataRowValue(oRow, "BookTotalPX", 0)
                        .BookTotalBFC = DataTransformation.getDataRowValue(oRow, "BookTotalBFC", 0)
                        .BookDateDelivered = DataTransformation.getDataRowValue(oRow, "BookDateDelivered")
                        .BookDateInvoice = DataTransformation.getDataRowValue(oRow, "BookDateInvoice")
                        .BookODControl = DataTransformation.getDataRowValue(oRow, "BookODControl", 0)
                        .BookCarrierControl = DataTransformation.getDataRowValue(oRow, "BookCarrierControl", 0)
                        .BookPayCode = DataTransformation.getDataRowValue(oRow, "BookPayCode", "")
                        .BookModDate = DataTransformation.getDataRowValue(oRow, "BookModDate")
                        .BookModUser = DataTransformation.getDataRowValue(oRow, "BookModUser", "")
                        .Expr1 = DataTransformation.getDataRowValue(oRow, "Expr1")
                        .BookLoadPONumber = DataTransformation.getDataRowValue(oRow, "BookLoadPONumber", "")
                        .BookLoadVendor = DataTransformation.getDataRowValue(oRow, "BookLoadVendor", "")
                        .BookLoadCaseQty = DataTransformation.getDataRowValue(oRow, "BookLoadCaseQty", 0)
                        .BookLoadWgt = DataTransformation.getDataRowValue(oRow, "BookLoadWgt", 0)
                        .BookLoadCube = DataTransformation.getDataRowValue(oRow, "BookLoadCube", 0)
                        .BookLoadPL = DataTransformation.getDataRowValue(oRow, "BookLoadPL", 0)
                        .BookLoadPX = DataTransformation.getDataRowValue(oRow, "BookLoadPX", 0)
                        .BookLoadCom = DataTransformation.getDataRowValue(oRow, "BookLoadCom", "")
                        .BookLoadPUOrigin = DataTransformation.getDataRowValue(oRow, "BookLoadPUOrigin", "")
                        .BookLoadBFC = DataTransformation.getDataRowValue(oRow, "BookLoadBFC", 0)
                        .BookDestAddress1 = DataTransformation.getDataRowValue(oRow, "BookDestAddress1", "")
                        .BookDestAddress2 = DataTransformation.getDataRowValue(oRow, "BookDestAddress2", "")
                        .LaneLatitude = DataTransformation.getDataRowValue(oRow, "LaneLatitude", 0)
                        .LaneLongitude = DataTransformation.getDataRowValue(oRow, "LaneLongitude", 0)
                        .SpecialCodes = DataTransformation.getDataRowValue(oRow, "SpecialCodes", "")
                        .LaneFixedTime = DataTransformation.getDataRowValue(oRow, "LaneFixedTime", "")
                        .BookLoadControl = DataTransformation.getDataRowValue(oRow, "BookLoadControl", 0)
                        .BookHoldLoad = DataTransformation.getDataRowValue(oRow, "BookHoldLoad", 0)
                        .LaneControl = DataTransformation.getDataRowValue(oRow, "LaneControl", 0)
                        .ActualWgt = DataTransformation.getDataRowValue(oRow, "ActualWgt", 0)
                        .LaneNumber = DataTransformation.getDataRowValue(oRow, "LaneNumber", "")
                        .BookCarrOrderNumber = DataTransformation.getDataRowValue(oRow, "BookCarrOrderNumber", "")
                        .BookTransType = DataTransformation.getDataRowValue(oRow, "BookTransType", "")
                        .BookRevBilledBFC = DataTransformation.getDataRowValue(oRow, "BookRevBilledBFC", 0)
                        .BookCustCompControl = DataTransformation.getDataRowValue(oRow, "BookCustCompControl", 0)
                        .BookRevTotalCost = DataTransformation.getDataRowValue(oRow, "BookRevTotalCost", 0)
                        .BookTypeCode = DataTransformation.getDataRowValue(oRow, "BookTypeCode", "")
                        .CarrierName = DataTransformation.getDataRowValue(oRow, "CarrierName", "")
                        .CarrierNumber = DataTransformation.getDataRowValue(oRow, "CarrierNumber", 0)
                        .CarrierNameNumber = .CarrierNumber.ToString & " " & .CarrierName
                        .BookNotesVisable1 = DataTransformation.getDataRowValue(oRow, "BookNotesVisable1", "")
                        '.BookModeTypeControl = DTran.getDataRowValue(oRow, "BookModeTypeControl", 0)
                        '.BookOrderSequence = DTran.getDataRowValue(oRow, "BookOrderSequence", 0)

                        .BookFinARInvoiceDate = DataTransformation.getDataRowValue(oRow, "BookFinARInvoiceDate")
                        .BookDateOrdered = DataTransformation.getDataRowValue(oRow, "BookDateOrdered")
                        .BookLockAllCosts = DataTransformation.getDataRowValue(oRow, "BookLockAllCosts", 0)
                        .BookLockBFCCost = DataTransformation.getDataRowValue(oRow, "BookLockBFCCost", 0)
                        'New fields for v-6.4
                        .BookSHID = DataTransformation.getDataRowValue(oRow, "BookSHID", "")
                        .BookExpDelDateTime = DataTransformation.getDataRowValue(oRow, "BookExpDelDateTime")
                        .BookMustLeaveByDateTime = DataTransformation.getDataRowValue(oRow, "BookMustLeaveByDateTime")
                        .BookOutOfRouteMiles = DataTransformation.getDataRowValue(oRow, "BookOutOfRouteMiles", 0)
                        .BookSpotRateAllocationFormula = DataTransformation.getDataRowValue(oRow, "BookSpotRateAllocationFormula", 0)
                        .BookSpotRateAutoCalcBFC = DataTransformation.getDataRowValue(oRow, "BookSpotRateAutoCalcBFC", True)
                        .BookSpotRateUseCarrierFuelAddendum = DataTransformation.getDataRowValue(oRow, "BookSpotRateUseCarrierFuelAddendum", False)
                        .BookSpotRateBFCAllocationFormula = DataTransformation.getDataRowValue(oRow, "BookSpotRateBFCAllocationFormula", 0)
                        .BookSpotRateTotalUnallocatedBFC = DataTransformation.getDataRowValue(oRow, "BookSpotRateTotalUnallocatedBFC", 0)
                        .BookSpotRateTotalUnallocatedLineHaul = DataTransformation.getDataRowValue(oRow, "BookSpotRateTotalUnallocatedLineHaul", 0)
                        .BookSpotRateUseFuelAddendum = DataTransformation.getDataRowValue(oRow, "BookSpotRateUseFuelAddendum", False)
                        .BookRevLaneBenchMiles = DataTransformation.getDataRowValue(oRow, "BookRevLaneBenchMiles")
                        .BookRevLoadMiles = DataTransformation.getDataRowValue(oRow, "BookRevLoadMiles")
                        .BookCarrTarControl = DataTransformation.getDataRowValue(oRow, "BookCarrTarControl", 0)
                        .BookCarrTarName = DataTransformation.getDataRowValue(oRow, "BookCarrTarName", "")
                        .BookCarrTarEquipControl = DataTransformation.getDataRowValue(oRow, "BookCarrTarEquipControl", 0)
                        .BookShipCarrierProControl = DataTransformation.getDataRowValue(oRow, "BookShipCarrierProControl")
                    End With
                    oConsolidation.Add(oItem)
                Next
            Else
                oConsolidation.Add(New DataTransferObjects.vConsolidation)
            End If
            Return oConsolidation.ToArray()
        Catch ex As FaultException
            Throw
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As InvalidOperationException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            oQuery = Nothing
        End Try

        Return Nothing
    End Function

#End Region

#Region "     Benchmark Freight Costing"

    ''' <summary>
    ''' calls udfCalcBFCWithFees to calculate the BFC using updated totals 
    ''' </summary>
    ''' <param name="NbrMiles"></param>
    ''' <param name="LoadWgt"></param>
    ''' <param name="NbrPallets"></param>
    ''' <param name="NbrCubes"></param>
    ''' <param name="NbrCases"></param>
    ''' <param name="DefaultBFC"></param>
    ''' <param name="CompControl"></param>
    ''' <param name="LaneControl"></param>
    ''' <param name="TotalFrtCost"></param>
    ''' <param name="TotalFees"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.0 on 10/21/2016 
    '''   we now accept TotalFees typically stored in BookRevOtherCost to provide support for BFC with Fees Calculations 
    ''' </remarks>
    Public Function CalculateBFC(ByVal NbrMiles As Double,
                                 ByVal LoadWgt As Double,
                                 ByVal NbrPallets As Double,
                                 ByVal NbrCubes As Integer,
                                 ByVal NbrCases As Integer,
                                 ByVal DefaultBFC As Decimal,
                                 ByVal CompControl As Integer,
                                 ByVal LaneControl As Integer,
                                 ByVal TotalFrtCost As Decimal,
                                 ByVal TotalFees As Decimal) As Decimal
        Dim decRet As System.Nullable(Of Decimal) = DefaultBFC
        Using operation = Logger.StartActivity("CalculateBFC(NbrMiles: {NbrMiles}, LoadWgt: {LoadWgt}, NbrPallets: {NbrPallets}, NbrCubes: {NbrCubes}, DefaultBFC: {DefaultBFC}, CompControl: {CompControl}, LaneControl: {LaneControl}, TotalFrtCost: {TotalFrtCost}, TotalFees:{TotalFees}", NbrMiles, LoadWgt, NbrPallets, NbrCubes, DefaultBFC, CompControl, LaneControl, TotalFrtCost, TotalFees)
            Using db As New NGLMasBookDataContext(ConnectionString)
                Try
                    decRet = db.udfCalcBFCWithFees(CInt(NbrMiles), LoadWgt, CInt(NbrPallets), NbrCubes, NbrCases, DefaultBFC, CompControl, LaneControl, TotalFrtCost, TotalFees)
                Catch ex As Exception
                    operation.Complete(LogEventLevel.Error, ex)
                    ManageLinqDataExceptions(ex, buildProcedureName("CalculateBFC"))
                End Try
            End Using
        End Using

        Return If(decRet, 0)
    End Function

    ''' <summary>
    ''' This method is no longer Accurate and must be modified to support 7.0 changes
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetBFC(ByVal BookControl As Integer) As Decimal
        Dim decRet As Decimal = 0
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'TODO: v-7.0 Replace this logic with updated BFC Cost Modules
                decRet = db.udfGetBFC(BookControl)

                Return decRet

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

            Return decRet

        End Using

    End Function


#End Region

#Region "     Routing and Transload Methods"

    ''' <summary>
    ''' creates a new order sequence for the provided booking record and returns 
    ''' the following primary keys and result data: BookControl,BookLoadControl,BookConsPrefix,BookRouteConsFlag,RetMsg,ErrNumber; 
    ''' ErrNumber zero indicates success.
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="BookLaneTranXDetControl"></param>
    ''' <param name="BookConsPrefix"></param>
    ''' <param name="BookRouteConsFlag"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SplitOrderForTransload(ByVal BookControl As Integer,
                                           ByVal BookLaneTranXDetControl As Integer,
                                           ByVal BookConsPrefix As String,
                                           ByVal BookRouteConsFlag As Boolean) As LTS.spSplitOrderForTransloadResult
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oSplitKeys = (From d In db.spSplitOrderForTransload(BookControl, BookLaneTranXDetControl, BookConsPrefix, BookRouteConsFlag, Me.Parameters.UserName) Select d).FirstOrDefault()
                Return oSplitKeys
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SplitOrderForTransload"))
            End Try
            Return Nothing
        End Using
    End Function


#End Region

#Region "     Fuel Data Methods"

    ''' <summary>
    ''' Returns a list of bookcontrol numbers that are shipping today or later for the provided carrier 
    ''' </summary>
    ''' <param name="CarrierControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetFuelBookingsForCarrier(ByVal CarrierControl As Integer) As List(Of Integer)
        Dim lRet As New List(Of Integer)
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Return (From d In db.Books Where d.BookCarrierControl = CarrierControl And d.BookDateLoad >= Date.Now().ToShortDateString Select d.BookControl).ToList()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetFuelBookingsForCarrier"))
            End Try
        End Using
        Return lRet
    End Function

    Public Function GetFuelBookingsDictForCarrier(ByVal CarrierControl As Integer) As Dictionary(Of Integer, String)
        Dim oDict As New Dictionary(Of Integer, String)
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                oDict = (From d In db.Books Where d.BookCarrierControl = CarrierControl And d.BookDateLoad >= Date.Now().ToShortDateString Select d.BookControl, d.BookProNumber).AsEnumerable().ToDictionary(Function(kvp) kvp.BookControl, Function(kvp) kvp.BookProNumber)
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetFuelBookingsForCarrier"))
            End Try
        End Using
        Return oDict
    End Function

    Public Function GetFuelBookingReferenceForCarrier(ByVal CarrierControl As Integer) As DataTransferObjects.BookReferenceData()
        Dim oRet() As DataTransferObjects.BookReferenceData
        Using Logger.StartActivity("GetFuelBookingReferenceForCarrier(CarrierControl {CarrierControl})", CarrierControl)
            Using db As New NGLMasBookDataContext(ConnectionString)
                Try
                    oRet = (From d In db.Books Where d.BookCarrierControl = CarrierControl And d.BookDateLoad >= Date.Now().ToShortDateString Select New DataTransferObjects.BookReferenceData With {.BookControl = d.BookControl, .BookCarrOrderNumber = d.BookCarrOrderNumber, .BookConsPrefix = d.BookConsPrefix, .BookCustCompControl = d.BookCustCompControl, .BookOrderSequence = d.BookOrderSequence, .BookProNumber = d.BookProNumber, .BookRouteConsFlag = d.BookRouteConsFlag}).ToArray()

                Catch ex As Exception
                    ManageLinqDataExceptions(ex, buildProcedureName("GetFuelBookingsForCarrier"))
                End Try
            End Using
        End Using

        Return oRet
    End Function

#End Region

#Region "     Book Data Utilities Methods"

    Public Function CreateBookFilters() As DataTransferObjects.BookFilters
        Return New DataTransferObjects.BookFilters
    End Function

    ''' <summary>
    ''' This method replaces all previous update stored procedures to validate CNS and totals for a load.
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="BookLoadControl"></param>
    ''' <remarks></remarks>
    Public Function UpdateBookDependencies(ByVal BookControl As Integer, ByVal BookLoadControl As Integer) As LTS.spUpdateBookDependenciesResult
        Dim oReturnData As LTS.spUpdateBookDependenciesResult
        Using operation = Logger.StartActivity("UpdateBookDependencies(BookControl: {BookControl}, BookLoadControl: {BookLoadControl})", BookControl, BookLoadControl)
            Using db As New NGLMasBookDataContext(ConnectionString)
                Try
                    oReturnData = (From d In db.spUpdateBookDependencies(BookLoadControl, BookControl, Me.Parameters.UserName) Select d).FirstOrDefault()
                    If Not oReturnData Is Nothing Then
                        Me.BookDependencyResult = oReturnData
                        Me.LastProcedureName = "spUpdateBookDependencies"
                    End If
                Catch ex As Exception
                    Logger.Error(ex, "Error in UpdateBookDependencies")
                    operation.Complete(LogEventLevel.Error, ex)
                    ManageLinqDataExceptions(ex, buildProcedureName("UpdateBookDependencies"))
                End Try
            End Using
        End Using
        Return oReturnData
    End Function

    ''' <summary>
    ''' return true if any BookControl values = Control in the book table.
    ''' Throws FaultException on error
    ''' </summary>
    ''' <param name="Control"></param>
    ''' <returns>
    ''' returns false on error or if the query fails
    ''' </returns>
    ''' <remarks></remarks>
    Public Function DoesBookExist(ByVal Control As Integer) As Boolean
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Return db.Books.Any(Function(x) x.BookControl = Control)
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DoesBookExist"))
            End Try
            Return False
        End Using
    End Function

    ''' <summary>
    ''' returns true if any BookConsPrefix values = strCNS in the book table.  
    ''' Throws FaultException on error
    ''' </summary>
    ''' <param name="strCNS"></param>
    ''' <returns>
    ''' returns true on error or if the query fails
    ''' </returns>
    ''' <remarks></remarks>
    Public Function DoesCNSExist(ByVal strCNS As String) As Boolean
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Return db.Books.Any(Function(x) x.BookConsPrefix = strCNS)
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DoesCNSExist"))
            End Try
            Return True
        End Using
    End Function

    ''' <summary>
    ''' returns the first stops booking company and carrier data filterd by SHID
    ''' </summary>
    ''' <param name="sSHID"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 7/11/2019
    '''     created primarily for EDI lookups for carrier and comp data
    '''     replaces call to getCompInfoByCNS
    ''' </remarks>
    Public Function GetBookCarrierCompBySHID(ByVal sSHID As String) As LTS.vBookCarrierComp

        Dim oRet As New LTS.vBookCarrierComp()
        If String.IsNullOrWhiteSpace(sSHID) Then Return oRet
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                oRet = db.vBookCarrierComps.Where(Function(x) x.BookSHID = sSHID).OrderBy(Function(x) x.BookStopNo).FirstOrDefault()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetBookCarrierCompBySHID"))
            End Try
            Return oRet
        End Using
    End Function


#End Region

#Region "    Auto Tender Book Data Methods"

#Region " Old Methods to be Depreciated"

    ''' <summary>
    ''' Used to tender a new order to the assigned carrier based on lane, 
    ''' parameter and system configuraiton settings 
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <remarks></remarks>
    Public Sub AutoTenderLoadCurrentCarrier(ByVal BookControl As Integer)
        Dim strProcName As String = "dbo.spAutoTenderLoadCurrentCarrier"
        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@BookControl", BookControl)
        oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        runNGLStoredProcedure(oCmd, strProcName, 0)
    End Sub

#End Region


    ''' <summary>
    ''' Returns parameters, lane settings, and other configuration stored in the database needed for Accepting and Rejecting loads
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR v-7.0.5.100 05/17/2016
    '''   we now include SuppressEmailWhenLoadsManuallyAccepted and SuppressEmailChangesAfterLoadShipped 
    '''   company level parameters
    ''' </remarks>
    Public Function GetAutoTenderData(ByVal BookControl As Integer) As LTS.spGetAutoTenderDataResult
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Return db.spGetAutoTenderData(BookControl).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAutoTenderData"))
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Inserts a list of BookCarrTend objects into the BookCarrTend table.
    ''' </summary>
    ''' <param name="oList"></param>
    ''' <param name="blnIgnoreErrors"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddBookCarrTends(ByVal oList As List(Of LTS.BookCarrTend), Optional ByVal blnIgnoreErrors As Boolean = True) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                db.BookCarrTends.InsertAllOnSubmit(oList)
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                If Not blnIgnoreErrors Then ManageLinqDataExceptions(ex, buildProcedureName("AddBookCarrTends"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Returns a list of carriers that have already been tendered to this list of book control number (typically a load)
    ''' Used to restrinct which carriers are used to select the next lowest cost carrier.
    ''' </summary>
    ''' <param name="oBookControls"></param>
    ''' <param name="blnIgnoreErrors"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getPreviouslyTenderedCarriers(ByVal oBookControls As List(Of Integer), Optional ByVal blnIgnoreErrors As Boolean = True) As List(Of Integer)
        Dim lCarrierControls As New List(Of Integer)
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                lCarrierControls = (From d In db.BookCarrTends Where oBookControls.Contains(d.BookCarrTendBookControl) Select d.BookCarrTendCarrierControl).Distinct().ToList()
            Catch ex As Exception
                If Not blnIgnoreErrors Then ManageLinqDataExceptions(ex, buildProcedureName("getPreviouslyTenderedCarriers"))
            End Try
        End Using
        Return lCarrierControls
    End Function


#End Region


#Region " Changes for v-7.0.6.101"


    ''' <summary>
    ''' Returns zero or the miles stored in the db for this orig/dest pair
    ''' </summary>
    ''' <param name="ZipFrom"></param>
    ''' <param name="ZipTo"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.101 on 1/20/2017
    ''' Modified By LVV on 9/16/20 for v-8.3.0.001 - Optimizer 365
    '''  Added Null check because when oData.ODIMiles was nothing exceptions were being thrown
    ''' </remarks>
    Public Function GetOptDist(ByVal ZipFrom As String, ByVal ZipTo As String) As Double
        Dim dblRet As Double = 0
        Using operation = Logger.StartActivity("Get Optimal Distance from {ZipFrom} to {ZipTo}", ZipFrom, ZipTo)
            Using db As New NGLMasBookDataContext(ConnectionString)
                Try
                    Dim oData = db.spGetOptDist(ZipFrom, ZipTo).FirstOrDefault()
                    If Not oData Is Nothing Then
                        If Not oData.ODIMiles Is Nothing Then dblRet = oData.ODIMiles 'Modified By LVV on 9/16/20 for v-8.3.0.001 - Optimizer 365
                    End If
                Catch ex As Exception
                    Logger.Error(ex, "Error in GetOptDist")
                    'Utilities.SaveAppError(ex.Message, Me.Parameters)
                    'ManageLinqDataExceptions(ex, buildProcedureName("GetOptDist"))
                End Try
            End Using
        End Using

        Return dblRet

    End Function

    ''' <summary>
    ''' Saves the miles to the db for this orig/dest pair
    ''' </summary>
    ''' <param name="ZipFrom"></param>
    ''' <param name="ZipTo"></param>
    ''' <param name="Miles"></param>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.101 on 1/20/2017
    ''' </remarks>
    Public Sub UpdateOptDist(ByVal ZipFrom As String, ByVal ZipTo As String, Miles As Double)

        Using db As New NGLMasBookDataContext(ConnectionString)
            Try

                db.spUpdateOptDist(ZipFrom, ZipTo, Miles)

            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateOptDist"))
            End Try

        End Using
    End Sub


#End Region

#Region " TMS 365 Specific"

    Public Function GetOrdersFiltered(ByRef RecordCount As Integer,
                                      ByVal filter As Models.OrderFilter,
                                      Optional ByVal sortExpression As String = "",
                                      Optional ByVal skip As Integer = 0,
                                      Optional ByVal take As Integer = 0) As LTS.vBookOrder()
        Dim oRetData As LTS.vBookOrder()
        Using operation = Logger.StartActivity("GetOrdersFiltered(RecordCount: {RecordCount}, Filter: {@OrderFilter}, SortExpression: {sortExpression}, Skip: {Skip}, Take: {Take}", RecordCount, filter, sortExpression, skip, take)
            Using db As New NGLMasBookDataContext(ConnectionString)
                Try
                    If (filter.BookCustCompControl = 0 AndAlso Not String.IsNullOrWhiteSpace(filter.CompNameNumberCode)) Then
                        filter.BookCustCompControl = (From t In db.CompRefBooks
                                                      Where t.CompNumber = filter.CompNameNumberCode _
                                                            Or t.CompName = filter.CompNameNumberCode
                                                      Select t.CompControl).FirstOrDefault()

                    End If

                    If (filter.BookCarrierControl = 0 AndAlso Not String.IsNullOrWhiteSpace(filter.CarrierNameNumberCode)) Then
                        filter.BookCarrierControl = (From t In db.CarrierRefBooks
                                                     Where t.CarrierNumber = filter.CarrierNameNumberCode _
                                                           Or t.CarrierName = filter.CarrierNameNumberCode
                                                     Select t.CarrierControl).FirstOrDefault()

                    End If

                    Dim blnUseFromLoadDateFilter As Boolean = False
                    Dim blnUseToLoadDateFilter As Boolean = False
                    Dim blnUseFromReqDateFilter As Boolean = False
                    Dim blnUseToReqDateFilter As Boolean = False
                    Dim dtFromLoadDateFilter As Date = Date.Now
                    Dim dtToLoadDateFilter As Date = Date.Now
                    Dim dtFromReqDateFilter As Date = Date.Now
                    Dim dtToReqDateFilter As Date = Date.Now
                    If Not String.IsNullOrWhiteSpace(filter.LoadDateFrom) AndAlso Date.TryParse(filter.LoadDateFrom, dtFromLoadDateFilter) Then
                        blnUseFromLoadDateFilter = True
                    End If
                    If Not String.IsNullOrWhiteSpace(filter.LoadDateTo) AndAlso Date.TryParse(filter.LoadDateTo, dtToLoadDateFilter) Then
                        blnUseToLoadDateFilter = True
                    End If

                    If Not String.IsNullOrWhiteSpace(filter.ReqDateFrom) AndAlso Date.TryParse(filter.LoadDateFrom, dtFromReqDateFilter) Then
                        blnUseFromReqDateFilter = True
                    End If
                    If Not String.IsNullOrWhiteSpace(filter.ReqDateTo) AndAlso Date.TryParse(filter.LoadDateTo, dtToReqDateFilter) Then
                        blnUseToReqDateFilter = True
                    End If

                    Dim intPageCount As Integer = 1

                    Dim oQuery = From t In db.vBookOrders
                                 Where
                                 (filter.BookControl = 0 OrElse t.BookControl = filter.BookControl) _
                                 And
                                 (filter.BookCustCompControl = 0 OrElse t.BookCustCompControl = filter.BookCustCompControl) _
                                 And
                                 (filter.BookCarrierControl = 0 OrElse t.BookCarrierControl = filter.BookCarrierControl) _
                                 And
                                 (blnUseFromLoadDateFilter = False OrElse (Not t.BookDateLoad.HasValue OrElse t.BookDateLoad >= dtFromLoadDateFilter)) _
                                 And
                                 (blnUseToLoadDateFilter = False OrElse (Not t.BookDateLoad.HasValue OrElse t.BookDateLoad <= dtFromReqDateFilter)) _
                                 And
                                 (blnUseFromReqDateFilter = False OrElse (Not t.BookDateRequired.HasValue OrElse t.BookDateRequired >= dtFromReqDateFilter)) _
                                 And
                                 (blnUseToReqDateFilter = False OrElse (Not t.BookDateRequired.HasValue OrElse t.BookDateRequired <= dtToReqDateFilter))
                                 Select t

                    RecordCount = oQuery.Count()
                    If RecordCount < 1 Then Return Nothing
                    Dim page As Integer = 1
                    Dim pagesize As Integer = 1000
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


                Catch ex As Exception
                    Logger.Error(ex, "Error in GetOrdersFiltered")
                End Try


            End Using
        End Using
        Return oRetData
    End Function


    Public Function GetUniqueBookingAddressListFiltered(ByRef RecordCount As Integer,
                                                        ByVal filter As Models.OrderFilter,
                                                        Optional ByVal sortExpression As String = "",
                                                        Optional ByVal skip As Integer = 0,
                                                        Optional ByVal take As Integer = 0) As LTS.spGetUniqueBookingAddressListResult()
        Dim oRetData As LTS.spGetUniqueBookingAddressListResult()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                If (filter.BookCustCompControl = 0 AndAlso Not String.IsNullOrWhiteSpace(filter.CompNameNumberCode)) Then
                    filter.BookCustCompControl = (From t In db.CompRefBooks
                                                  Where t.CompNumber = filter.CompNameNumberCode _
                                                        Or t.CompName = filter.CompNameNumberCode
                                                  Select t.CompControl).FirstOrDefault()

                End If

                If (filter.BookCarrierControl = 0 AndAlso Not String.IsNullOrWhiteSpace(filter.CarrierNameNumberCode)) Then
                    filter.BookCarrierControl = (From t In db.CarrierRefBooks
                                                 Where t.CarrierNumber = filter.CarrierNameNumberCode _
                                                       Or t.CarrierName = filter.CarrierNameNumberCode
                                                 Select t.CarrierControl).FirstOrDefault()

                End If

                Dim blnUseFromLoadDateFilter As Boolean = False
                Dim blnUseToLoadDateFilter As Boolean = False
                Dim blnUseFromReqDateFilter As Boolean = False
                Dim blnUseToReqDateFilter As Boolean = False
                Dim dtFromLoadDateFilter As Date = Date.Now
                Dim dtToLoadDateFilter As Date = Date.Now
                Dim dtFromReqDateFilter As Date = Date.Now
                Dim dtToReqDateFilter As Date = Date.Now
                Dim intPageCount As Integer = 1
                Dim dtBookDateLoadFrom As Date?
                Dim dtBookDateLoadTo As Date?
                Dim dtBookDateReqFrom As Date?
                Dim dtBookDateReqTo As Date?
                If Not String.IsNullOrWhiteSpace(filter.LoadDateFrom) AndAlso Date.TryParse(filter.LoadDateFrom, dtFromLoadDateFilter) Then
                    dtBookDateLoadFrom = dtFromLoadDateFilter
                End If
                If Not String.IsNullOrWhiteSpace(filter.LoadDateTo) AndAlso Date.TryParse(filter.LoadDateTo, dtToLoadDateFilter) Then
                    dtBookDateLoadTo = dtToLoadDateFilter
                End If

                If Not String.IsNullOrWhiteSpace(filter.ReqDateFrom) AndAlso Date.TryParse(filter.LoadDateFrom, dtFromReqDateFilter) Then
                    dtBookDateReqFrom = dtFromReqDateFilter
                End If
                If Not String.IsNullOrWhiteSpace(filter.ReqDateTo) AndAlso Date.TryParse(filter.LoadDateTo, dtToReqDateFilter) Then
                    dtBookDateReqTo = dtToReqDateFilter
                End If



                Dim oQuery = (From t In db.spGetUniqueBookingAddressList(dtBookDateLoadFrom, dtBookDateLoadTo, dtBookDateReqFrom, dtBookDateReqTo, filter.BookCustCompControl) Select t).ToList()


                RecordCount = oQuery.Count()
                If RecordCount < 1 Then Return Nothing
                Dim page As Integer = 1
                Dim pagesize As Integer = 1000
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
                oRetData = oQuery.Skip(skip).Take(pagesize).ToArray()

                Return oRetData

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetUniqueBookingAddressListFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Returns an array of LTS.vBookLTL data objects representing LTL orders that have not been consolidated
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="BookCustCompControl"></param>
    ''' <param name="BookCarrierControl"></param>
    ''' <param name="datefilterType"></param>
    ''' <param name="StartDate"></param>
    ''' <param name="EndDate"></param>
    ''' <param name="LaneOriginAddressUse"></param>
    ''' <param name="sortExpression"></param>
    ''' <param name="page"></param>
    ''' <param name="pagesize"></param>
    ''' <param name="skip"></param>
    ''' <param name="take"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR v-7.0.5.110 8/30/2016 for use in v-8.0
    ''' </remarks>
    Public Function GetBookLTLFiltered(ByRef RecordCount As Integer,
                                       Optional ByVal BookCustCompControl As Integer = 0,
                                       Optional ByVal BookCarrierControl As Integer = 0,
                                       Optional ByVal datefilterType As Utilities.NGLDateFilterType = Utilities.NGLDateFilterType.None,
                                       Optional ByVal StartDate As DateTime? = Nothing,
                                       Optional ByVal EndDate As DateTime? = Nothing,
                                       Optional ByVal LaneOriginAddressUse As Boolean = False,
                                       Optional ByVal sortExpression As String = "",
                                       Optional ByVal page As Integer = 1,
                                       Optional ByVal pagesize As Integer = 1000,
                                       Optional ByVal skip As Integer = 0,
                                       Optional ByVal take As Integer = 0) As LTS.vBookLTL()
        Dim oRetData As LTS.vBookLTL()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                If datefilterType <> Utilities.NGLDateFilterType.None Then
                    If StartDate.HasValue And EndDate.HasValue Then
                        StartDate = DataTransformation.formatStartDateFilter(StartDate)
                        EndDate = DataTransformation.formatEndDateFilter(EndDate)
                    Else
                        'disable the date filter if the dates are null
                        datefilterType = Utilities.NGLDateFilterType.None
                    End If
                End If

                Dim intPageCount As Integer = 1

                Dim oQuery = From t In db.vBookLTLs
                             Where
                             (BookCustCompControl = 0 OrElse t.BookCustCompControl = BookCustCompControl) _
                             And
                             (BookCarrierControl = 0 OrElse t.BookCarrierControl = BookCarrierControl) _
                             And
                             (t.LaneOriginAddressUse = LaneOriginAddressUse) _
                             And
                             (
                                 datefilterType = Utilities.NGLDateFilterType.None _
                                 Or
                                 (
                                     (datefilterType = Utilities.NGLDateFilterType.DateOrdered AndAlso t.BookDateOrdered.HasValue AndAlso t.BookDateOrdered >= StartDate AndAlso t.BookDateOrdered <= EndDate) _
                                     Or
                                     (datefilterType = Utilities.NGLDateFilterType.DateLoad AndAlso t.BookDateLoad.HasValue AndAlso t.BookDateLoad >= StartDate AndAlso t.BookDateLoad <= EndDate) _
                                     Or
                                     (datefilterType = Utilities.NGLDateFilterType.DateRequired AndAlso t.BookDateRequired.HasValue AndAlso t.BookDateRequired >= StartDate AndAlso t.BookDateRequired <= EndDate)
                                     )
                                 )
                             Select t

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
                ManageLinqDataExceptions(ex, buildProcedureName("GetBookLTLFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Returns an array of consolidated loads that are not full and are open for additional stops.
    ''' Filters are available to limit the results by date, carrier or shipment ID.  if the ShipKey filter
    ''' is provided it will be compared to the BookSHID first then the BookConsPrefix second 
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="ShipKey"></param>
    ''' <param name="BookCarrierControl"></param>
    ''' <param name="datefilterType"></param>
    ''' <param name="StartDate"></param>
    ''' <param name="EndDate"></param>
    ''' <param name="sortExpression"></param>
    ''' <param name="page"></param>
    ''' <param name="pagesize"></param>
    ''' <param name="skip"></param>
    ''' <param name="take"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR v-7.0.5.110 9/01/2016 for use in v-8.0
    ''' </remarks>
    Public Function GetBookConsolidatedOpenFiltered(ByRef RecordCount As Integer,
                                                    Optional ByVal ShipKey As String = "",
                                                    Optional ByVal BookCarrierControl As Integer = 0,
                                                    Optional ByVal datefilterType As Utilities.NGLDateFilterType = Utilities.NGLDateFilterType.None,
                                                    Optional ByVal StartDate As DateTime? = Nothing,
                                                    Optional ByVal EndDate As DateTime? = Nothing,
                                                    Optional ByVal sortExpression As String = "",
                                                    Optional ByVal page As Integer = 1,
                                                    Optional ByVal pagesize As Integer = 1000,
                                                    Optional ByVal skip As Integer = 0,
                                                    Optional ByVal take As Integer = 0) As LTS.vBookConsolidatedOpen()
        Dim oRetData As LTS.vBookConsolidatedOpen()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                If datefilterType <> Utilities.NGLDateFilterType.None Then
                    If StartDate.HasValue And EndDate.HasValue Then
                        StartDate = DataTransformation.formatStartDateFilter(StartDate)
                        EndDate = DataTransformation.formatEndDateFilter(EndDate)
                    Else
                        'disable the date filter if the dates are null
                        datefilterType = Utilities.NGLDateFilterType.None
                    End If
                End If

                Dim intPageCount As Integer = 1

                Dim oQuery = From t In db.vBookConsolidatedOpens
                             Where
                             (BookCarrierControl = 0 OrElse t.BookCarrierControl = BookCarrierControl) _
                             And
                             (String.IsNullOrWhiteSpace(ShipKey) OrElse (t.BookSHID = ShipKey Or t.BookConsPrefix = ShipKey)) _
                             And
                             (
                                 datefilterType = Utilities.NGLDateFilterType.None _
                                 Or
                                 (
                                     (datefilterType = Utilities.NGLDateFilterType.DateOrdered AndAlso t.OrderDate.HasValue AndAlso t.OrderDate >= StartDate AndAlso t.OrderDate <= EndDate) _
                                     Or
                                     (datefilterType = Utilities.NGLDateFilterType.DateLoad AndAlso t.LoadDate.HasValue AndAlso t.LoadDate >= StartDate AndAlso t.LoadDate <= EndDate) _
                                     Or
                                     (datefilterType = Utilities.NGLDateFilterType.DateRequired AndAlso t.RequiredDate.HasValue AndAlso t.RequiredDate >= StartDate AndAlso t.RequiredDate <= EndDate)
                                     )
                                 )
                             Select t

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
                ManageLinqDataExceptions(ex, buildProcedureName("GetBookConsolidatedOpenFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Returns an array or booking orders assigned to a specific shipping key.  The shipping key represents an BookSHID first then a BookConsPrefix secondBookSHID.
    ''' The caller should send the first value that is not empty.
    ''' </summary>
    ''' <param name="ShipKey"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR v-7.0.5.110 9/01/2016 for use in v-8.0
    ''' </remarks>
    Public Function GetLoadOrders(ByVal ShipKey As String) As LTS.vBookOrder()
        Dim oRetData As LTS.vBookOrder()
        If String.IsNullOrWhiteSpace(ShipKey) Then
            Return Nothing
        End If
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try

                oRetData = (From t In db.vBookOrders
                            Where
                                (String.IsNullOrWhiteSpace(ShipKey) OrElse (t.BookSHID = ShipKey Or t.BookConsPrefix = ShipKey))
                            Select t).ToArray()

                Return oRetData

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLoadOrders"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Returns a single booking order summary record typically used for load planning
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR v-7.0.5.110 9/01/2016 for use in v-8.0
    ''' </remarks>
    Public Function GetLoadOrder(ByVal BookControl As Integer) As LTS.vBookOrder
        Dim oRetData As LTS.vBookOrder
        If BookControl = 0 Then
            Return Nothing
        End If
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try

                oRetData = (From t In db.vBookOrders
                            Where
                                (t.BookControl = BookControl)
                            Select t).FirstOrDefault()

                Return oRetData

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLoadOrder"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Returns an array or booking order details assigned to a specific book control number. 
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR v-7.0.5.110 9/19/2016 for use in v-8.0
    ''' </remarks>
    Public Function GetOrderDetails(ByVal BookControl As Integer) As LTS.vBookOrderDetail()
        Dim oRetData As LTS.vBookOrderDetail()
        If BookControl = 0 Then
            Return Nothing
        End If
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try

                oRetData = (From t In db.vBookOrderDetails
                            Where
                                (t.BookLoadBookControl = BookControl)
                            Select t).ToArray()

                Return oRetData

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLoadOrderDetails"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Returns a single booking order detail record 
    ''' </summary>
    ''' <param name="BookItemControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR v-7.0.5.110 9/19/2016 for use in v-8.0
    ''' </remarks>
    Public Function GetOrderDetail(ByVal BookItemControl As Integer) As LTS.vBookOrderDetail
        Dim oRetData As LTS.vBookOrderDetail
        If BookItemControl = 0 Then
            Return Nothing
        End If
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try

                oRetData = (From t In db.vBookOrderDetails
                            Where
                                (t.BookItemControl = BookItemControl)
                            Select t).FirstOrDefault()

                Return oRetData

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLoadOrder"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' The same as GetWebBillPayRecords365 but with 
    ''' added logic for oSecureComps and oSecureCarriers
    ''' as well as ability for non-carrier users to see records
    ''' for their associated carriers
    ''' Populates the grid on the Settled Screen in 365
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="CarrierControl"></param>
    ''' <param name="CarrierContControl"></param>
    ''' <param name="proNumber"></param>
    ''' <param name="cns"></param>
    ''' <param name="bookCarrOrderNumber"></param>
    ''' <param name="bookDateDelDate"></param>
    ''' <param name="bookDateDelDateTo"></param>
    ''' <param name="bookPayCode"></param>
    ''' <param name="bookRevTotalCost"></param>
    ''' <param name="bookFinAPBillNumber"></param>
    ''' <param name="bookFinAPActCost"></param>
    ''' <param name="bookSHID"></param>
    ''' <param name="bookShipCarrierProNumber"></param>
    ''' <param name="sortOrdinal"></param>
    ''' <param name="sortDirection"></param>
    ''' <param name="datefilterfield"></param>
    ''' <param name="dateFilterFrom"></param>
    ''' <param name="dateFilterTo"></param>
    ''' <param name="carrierName"></param>
    ''' <param name="filters"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 4/9/19
    ''' </remarks>
    Public Function GetSettlementRecords365(ByRef RecordCount As Integer,
                                            ByVal CarrierControl As Integer?,
                                            ByVal CarrierContControl As Integer?,
                                            ByVal proNumber As String,
                                            ByVal cns As String,
                                            ByVal bookCarrOrderNumber As String,
                                            ByVal bookDateDelDate As Date?,
                                            ByVal bookDateDelDateTo As Date?,
                                            ByVal bookPayCode As String,
                                            ByVal bookRevTotalCost As Decimal?,
                                            ByVal bookFinAPBillNumber As String,
                                            ByVal bookFinAPActCost As Decimal?,
                                            ByVal bookSHID As String,
                                            ByVal bookShipCarrierProNumber As String,
                                            ByVal sortOrdinal As String,
                                            ByVal sortDirection As String,
                                            ByVal datefilterfield As String,
                                            ByVal dateFilterFrom As Date?,
                                            ByVal dateFilterTo As Date?,
                                            ByVal carrierName As String,
                                            ByVal filters As Models.AllFilters) As SettlementItem()
        Dim oRetData As SettlementItem()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oQuery = (From d In db.spGetSettlementRecords365(CarrierControl, CarrierContControl, proNumber, cns, bookCarrOrderNumber, bookDateDelDate,
                                                                     bookDateDelDateTo, bookPayCode, bookRevTotalCost, bookFinAPBillNumber, bookFinAPActCost,
                                                                     bookSHID, bookShipCarrierProNumber, sortOrdinal, sortDirection, datefilterfield, dateFilterFrom, dateFilterTo,
                                                                     Parameters.UserName, carrierName, Parameters.UserControl)
                              Select New SettlementItem With {
                              .Control = If(d.BookControl.HasValue, d.BookControl, 0),
                              .ProNumber = d.BookProNumber,
                              .CnsNumber = d.BookConsPrefix,
                              .OrderNumber = d.BookCarrOrderNumber,
                              .PickupName = d.BookOrigName,
                              .DestinationName = d.BookDestName,
                              .Status = d.BookPayCode,
                              .DeliveredDate = d.BookDateDelivered,
                              .ContractedCost = If(d.BookRevTotalCost.HasValue, d.BookRevTotalCost, 0),
                              .InvoiceNumber = d.BookFinAPBillNumber,
                              .InvoiceAmount = If(d.BookFinAPActCost.HasValue, d.BookFinAPActCost, 0),
                              .SHID = d.BookSHID,
                              .CarrierPro = d.BookShipCarrierProNumber,
                              .BookFinAPActWgt = If(d.BookFinAPActWgt.HasValue, d.BookFinAPActWgt, 0),
                              .BookCarrBLNumber = d.BookCarrBLNumber,
                              .CarrierName = d.CarrierName,
                              .BookCarrierControl = d.BookCarrierControl}).ToList()
                If oQuery Is Nothing Then Return Nothing
                '** This logic comes from PrepareQuery() **
                'If we can move the above code to a view or something we can just call PrepareQuery()
                'here -- this will be easier to maintain
                RecordCount = oQuery.Count()
                If RecordCount < 1 Then RecordCount = 1
                If filters.take < 1 Then filters.take = 1
                'adjust for last page if skip beyound last page
                If filters.skip >= RecordCount Then filters.skip = (CInt(((RecordCount - 1) / filters.take)) * filters.take)
                'adjust for first page if skip beyound or below first page
                If filters.skip >= RecordCount Or filters.skip < 0 Then filters.skip = 0
                oRetData = oQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRetData
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetSettlementRecords365"))
            End Try
            Return Nothing
        End Using
    End Function


    ''' <summary>
    ''' The same as GetWebSettledRecords365 but with 
    ''' added logic for oSecureComps and oSecureCarriers
    ''' as well as ability for non-carrier users to see records
    ''' for their associated carriers
    ''' Populates the grid on the Settled Screen in 365
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="CarrierControl"></param>
    ''' <param name="CarrierContControl"></param>
    ''' <param name="proNumber"></param>
    ''' <param name="cns"></param>
    ''' <param name="bookFinAPPayDate"></param>
    ''' <param name="bookFinAPPayAmt"></param>
    ''' <param name="bookFinAPPayCheck"></param>
    ''' <param name="bookRevTotalCost"></param>
    ''' <param name="bookFinAPBillNumber"></param>
    ''' <param name="bookFinAPActCost"></param>
    ''' <param name="bookSHID"></param>
    ''' <param name="bookShipCarrierProNumber"></param>
    ''' <param name="sortOrdinal"></param>
    ''' <param name="sortDirection"></param>
    ''' <param name="datefilterfield"></param>
    ''' <param name="dateFilterFrom"></param>
    ''' <param name="dateFilterTo"></param>
    ''' <param name="carrierName"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 4/5/2019
    ''' </remarks>
    Public Function GetSettledRecords365(ByRef RecordCount As Integer,
                                         ByVal CarrierControl As Integer?,
                                         ByVal CarrierContControl As Integer?,
                                         ByVal proNumber As String,
                                         ByVal cns As String,
                                         ByVal bookFinAPPayDate As Date?,
                                         ByVal bookFinAPPayAmt As Decimal?,
                                         ByVal bookFinAPPayCheck As String,
                                         ByVal bookRevTotalCost As Decimal?,
                                         ByVal bookFinAPBillNumber As String,
                                         ByVal bookFinAPActCost As Decimal?,
                                         ByVal bookSHID As String,
                                         ByVal bookShipCarrierProNumber As String,
                                         ByVal sortOrdinal As String,
                                         ByVal sortDirection As String,
                                         ByVal datefilterfield As String,
                                         ByVal dateFilterFrom As Date?,
                                         ByVal dateFilterTo As Date?,
                                         ByVal carrierName As String,
                                         ByVal filters As Models.AllFilters) As DataTransferObjects.SettledItem()
        Dim oRetData As DataTransferObjects.SettledItem()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oQuery = (From d In db.spGetSettledRecords365(CarrierControl,
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
                                                                  Parameters.UserName,
                                                                  carrierName,
                                                                  Parameters.UserControl)
                              Select New DataTransferObjects.SettledItem With {
                              .Control = If(d.BookControl.HasValue, d.BookControl, 0),
                              .ProNumber = d.BookProNumber,
                              .CnsNumber = d.BookConsPrefix,
                              .InvoiceNumber = d.BookFinAPBillNumber,
                              .ContractedCost = If(d.BookRevTotalCost.HasValue, d.BookRevTotalCost, 0),
                              .PaidCost = If(d.BookFinAPPayAmt.HasValue, d.BookFinAPPayAmt, 0),
                              .InvoiceAmount = If(d.BookFinAPActCost.HasValue, d.BookFinAPActCost, 0),
                              .PaidDate = d.BookFinAPPayDate,
                              .CheckNumber = d.BookFinAPCheck,
                              .SHID = d.BookSHID,
                              .CarrierPro = d.BookShipCarrierProNumber,
                              .BookFinAPActWgt = If(d.BookFinAPActWgt.HasValue, d.BookFinAPActWgt, 0),
                              .BookCarrBLNumber = d.BookCarrBLNumber,
                              .CarrierName = d.CarrierName}).ToList()

                If oQuery Is Nothing Then Return Nothing
                '** This logic comes from PrepareQuery() **
                'If we can move the above code to a view or something we can just call PrepareQuery() 
                'here --this will be easier to maintain
                RecordCount = oQuery.Count()
                If RecordCount < 1 Then RecordCount = 1
                If filters.take < 1 Then filters.take = 1
                If filters.skip >= RecordCount Then filters.skip = (CInt(((RecordCount - 1) / filters.take)) * filters.take) 'adjust for last page if skip beyound last page
                If filters.skip >= RecordCount Or filters.skip < 0 Then filters.skip = 0 'adjust for first page if skip beyound or below first page
                oRetData = oQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRetData
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetSettledRecords365"))
            End Try
            Return Nothing
        End Using
    End Function



    ''' <summary>
    ''' Called from the Carrier Accessorial Approval Screen 365
    ''' Gets summaries of the Line Haul (BookRevCarrierCostSUM)
    ''' and Total Cost (BookRevTotalCostSUM) for the SHID
    ''' Used to create a ToolTip for the Grid (BookFeesPendingGrid)
    ''' </summary>
    ''' <param name="SHID"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 12/4/2017 for v-8.0 TMS365
    ''' </remarks>
    Public Function GetChargesSummaryForSHID(ByVal SHID As String) As LTS.spGetChargesSummaryForSHIDResult
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Return (From d In db.spGetChargesSummaryForSHID(SHID) Select d).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetChargesSummaryForSHID"))
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Gets the header records to be displayed in the Grid on the 
    ''' Freight Bill Detail Entry Window on the Settlement Screen
    ''' (FBSHIDGRID)
    ''' </summary>
    ''' <param name="BookSHID"></param>
    ''' <returns>LTS.vFBSHIDGrid365()</returns>
    ''' <remarks>
    ''' Added By LVV on 12/8/2017 for v-8.0 TMS365
    ''' </remarks>
    Public Function GetFBSHIDGrid365(ByVal BookSHID As String) As LTS.vFBSHIDGrid365()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                If String.IsNullOrWhiteSpace(BookSHID) Then Return Nothing
                Dim Books() = (
                        From d In db.vFBSHIDGrid365s
                        Where
                        (BookSHID = d.BookSHID)
                        Order By d.BookStopNo Ascending
                        Select d).ToArray()
                Return Books
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetFBSHIDGrid365"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Returns a list of Orders from the SHID that are unique by OrderNo-OrderSeq
    ''' </summary>
    ''' <param name="BookSHID"></param>
    ''' <returns></returns>
    Public Function GetUniqueOrdersBySHID(ByVal BookSHID As String) As LTS.vFBSHIDGrid365()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                If String.IsNullOrWhiteSpace(BookSHID) Then Return Nothing
                Dim Books = db.vFBSHIDGrid365s.Where(Function(x) x.BookSHID = BookSHID) _
                        .GroupBy(Function(g) New With {g.BookCarrOrderNumber, g.BookOrderSequence}) _
                        .Select(Function(x) New LTS.vFBSHIDGrid365() With {.BookControl = x.Min(Function(y) y.BookControl), .BookCarrOrderNumber = x.Key.BookCarrOrderNumber, .BookOrderSequence = x.Key.BookOrderSequence}).ToArray()
                Return Books
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetUniqueOrdersBySHID"), db)
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Returns a list of Orders from the SHID that are unique by OrigName
    ''' </summary>
    ''' <param name="BookSHID"></param>
    ''' <returns></returns>
    Public Function GetUniqueOrigBySHID(ByVal BookSHID As String) As LTS.vFBSHIDGrid365()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                If String.IsNullOrWhiteSpace(BookSHID) Then Return Nothing
                Dim Books = db.vFBSHIDGrid365s.Where(Function(x) x.BookSHID = BookSHID) _
                        .GroupBy(Function(g) New With {g.BookOrigName, g.BookOrigZip}) _
                        .Select(Function(x) New LTS.vFBSHIDGrid365() With {.BookControl = x.Min(Function(y) y.BookControl), .BookOrigName = x.Key.BookOrigName, .BookOrigZip = x.Key.BookOrigZip}).ToArray()
                Return Books
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetUniqueOrigBySHID"), db)
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Returns a list of Orders from the SHID that are unique by DestName
    ''' </summary>
    ''' <param name="BookSHID"></param>
    ''' <returns></returns>
    Public Function GetUniqueDestBySHID(ByVal BookSHID As String) As LTS.vFBSHIDGrid365()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                If String.IsNullOrWhiteSpace(BookSHID) Then Return Nothing
                Dim Books = db.vFBSHIDGrid365s.Where(Function(x) x.BookSHID = BookSHID) _
                        .GroupBy(Function(g) New With {g.BookDestName, g.BookDestZip}) _
                        .Select(Function(x) New LTS.vFBSHIDGrid365() With {.BookControl = x.Min(Function(y) y.BookControl), .BookDestName = x.Key.BookDestName, .BookDestZip = x.Key.BookDestZip}).ToArray()
                Return Books
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetUniqueDestBySHID"), db)
            End Try
            Return Nothing
        End Using
    End Function


    Public Function GetBookingMenuInfo(ByVal BookControl As Integer) As Models.BookingMenuInfo
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim retVal As New Models.BookingMenuInfo
                Dim carrierNumber = 0
                Dim carrierName = ""

                Dim b = db.Books.Where(Function(x) x.BookControl = BookControl).FirstOrDefault()
                If b Is Nothing Then Return Nothing

                Dim c = db.CarrierRefBooks.Where(Function(x) x.CarrierControl = b.BookCarrierControl).FirstOrDefault()
                If Not c Is Nothing Then
                    carrierNumber = c.CarrierNumber
                    carrierName = c.CarrierName
                End If

                With retVal
                    .BookControl = BookControl
                    .BookProNumber = b.BookProNumber
                    .CarrierControl = b.BookCarrierControl
                    .CarrierNumber = carrierNumber
                    .CarrierName = carrierName
                End With

                Return retVal
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetBookingMenuInfo"))
            End Try
            Return Nothing
        End Using
    End Function


#Region "Scheduler Methods"

    ''' <summary>
    ''' Scheduler
    ''' Saves EquipmentID for the provided BookControl
    ''' Rules:
    '''     They can only set EquipmentID on Pickups for Outbound Lanes
    '''     They can only set EquipmentID on Delivery for Inbound Lanes
    '''     They can only set EquipmentID on Transfers for the Pickup side
    '''     On Pickup EquipmentID can only be assinged to orders with the same LoadDate
    '''     On Delivery EquipmentID can only be assinged to orders with the same RequiredDate
    ''' Equipment IDs can be repeated And are Not guaranteed Unique. Use these rules:
    '''     1. By carrier
    '''     2. By company 
    '''     3. by inbound Or Outbound 
    '''     4. Within 3 ( company level Parameter) days before Or after the load Or require date.  The equipment ID may go out one day then come back And ship again a few days later.
    ''' </summary>
    ''' <param name="eVal"></param>
    ''' <param name="CompControl"></param>
    ''' <param name="LoadDate"></param>
    ''' <param name="RequiredDate"></param>
    ''' <param name="Inbound"></param>
    ''' <param name="IsTranfer"></param>
    ''' <remarks>
    ''' Added By LVV On 7/31/18 For v-8.3 TMS365 Scheduler
    ''' </remarks>
    Public Sub SaveEquipmentID(ByRef eVal As Models.EquipIDValidation, ByVal CompControl As Integer, ByVal LoadDate As Date, ByVal RequiredDate As Date, ByVal Inbound As Boolean, ByVal IsTranfer As Boolean)
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oLocalize As New NGLcmLocalizeKeyValuePairData(Parameters)
                Dim strDefault = ""
                Dim pBookControl = eVal.BookControl
                Dim EquipmentID = eVal.EquipID

                'Check that the BookControl is valid
                Dim ltsBook = db.Books.Where(Function(x) x.BookControl = pBookControl).FirstOrDefault()
                If ltsBook Is Nothing Then
                    strDefault = "Invalid Parameter: No record exists in the database for {0}: {1}."
                    eVal.ErrMsg = String.Format(oLocalize.GetLocalizedValueByKey("E_InvalidParameterNameValue", strDefault), "BookControl", pBookControl)
                    eVal.Success = False
                    eVal.IsAdd = False
                    Return
                End If

                'What action are we attempting to perform?
                If eVal.IsAdd Then
                    'This means we are saving a new order to an existing appointment
                    If eVal.IsPickup Then
                        ltsBook.BookAMSPickupApptControl = eVal.ApptControl
                    Else
                        ltsBook.BookAMSDeliveryApptControl = eVal.ApptControl
                    End If
                    ltsBook.BookCarrTrailerNo = EquipmentID
                    db.SubmitChanges()
                    eVal.Success = True
                Else

                    'Always let them blank it out if they want
                    If String.IsNullOrWhiteSpace(EquipmentID) Then
                        'Modified By LVV as part of bug fix - must update all orders on CNS
                        Dim ctrl = (From d In db.Books
                                    Join x In db.udfGetDependentBookControls(pBookControl, True)
                                    On d.BookControl Equals x.BookControl
                                    Select d).ToArray()

                        For Each c In ctrl
                            c.BookCarrTrailerNo = EquipmentID 'In the future we may want to optimize this to improve performance (spUpdateBookDependencies)
                        Next
                        ''ltsBook.BookCarrTrailerNo = EquipmentID
                        db.SubmitChanges()
                        eVal.Success = True
                        Return
                    End If

                    'First attempt to Save so validate that we can
                    Dim intRefreshDays = GetParValue("EquipIDRefreshDay", CompControl)

                    If eVal.IsPickup Then
                        'Pickup
                        If Inbound Then
                            strDefault = "Editing EquipmentID is not allowed for Inbound Pickup"
                            eVal.ErrMsg = oLocalize.GetLocalizedValueByKey("SchedSaveEquipIDFailPickIn", strDefault)
                            eVal.Success = False
                            eVal.IsAdd = False
                            Return
                        End If
                        'Pickup EquipID can only be assinged to orders with the same LoadDate
                        'If this EquipmentID is being used on another record with the same Carrier, Company, Inbound/Outbound, and is within intRefreshDays of the LoadDate then we cannot use it
                        Dim blnInUse = (From d In db.Books Join l In db.LaneRefBooks On d.BookODControl Equals l.LaneControl
                                        Where
                                        d.BookCarrTrailerNo = EquipmentID AndAlso d.BookCustCompControl = CompControl AndAlso d.BookCarrierControl = Parameters.UserCarrierControl AndAlso l.LaneOriginAddressUse = Inbound _
                                        AndAlso
                                        ((d.BookDateLoad.Value.Date <= LoadDate.AddDays(intRefreshDays).Date AndAlso d.BookDateLoad.Value.Date > LoadDate.Date) OrElse (d.BookDateLoad.Value.Date >= LoadDate.AddDays(-intRefreshDays).Date AndAlso d.BookDateLoad.Value.Date < LoadDate.Date))
                                        Select d).Any()
                        If blnInUse Then
                            strDefault = "Cannot save EquipmentID for Pickup because this ID is already in use. Enter a different Equipment ID."
                            eVal.ErrMsg = oLocalize.GetLocalizedValueByKey("SchedSaveEquipIDFailPickInUse", strDefault)
                            eVal.Success = False
                            eVal.IsAdd = False
                            Return
                        End If

                        'If the Then carrier enters the same equipment ID after the original appointment Is Set For an additional order we have To 
                        'reevaluate the validation for available time slots - may need to cancel the appointment - give carrier option to cancel appointment And reschedule if validation fails 
                        'check If there Is a pickup Or delivery control - if control ahs already been assigned then we have to revalidate that it can fit - recalculate the minutes needed
                        Dim bookAssigned = (From d In db.Books Join l In db.LaneRefBooks On d.BookODControl Equals l.LaneControl
                                            Where
                                            d.BookCarrTrailerNo = EquipmentID AndAlso d.BookCustCompControl = CompControl _
                                            AndAlso d.BookCarrierControl = Parameters.UserCarrierControl AndAlso l.LaneOriginAddressUse = Inbound _
                                            AndAlso d.BookDateLoad = LoadDate AndAlso d.BookAMSPickupApptControl <> 0
                                            Select d).FirstOrDefault()

                        If Not bookAssigned Is Nothing Then

                            Dim books = GetDependentBooksByEquipID(bookAssigned.BookControl, EquipmentID, CompControl, LoadDate, eVal.IsPickup, Inbound, True) 'This method now calls udfGetAMSDependentBookControls instead of udfGetDependentBookControlsByEquipID 2/13/20 
                            Dim oDockSet As New NGLDockSettingData(Parameters)

                            Dim bList = books.ToList()
                            bList.Add(bookAssigned)
                            Dim strOrders As String = ""
                            Dim blnFits = oDockSet.DoesOrderFitOnAppointment(strOrders, bList.ToArray(), bookAssigned.BookAMSPickupApptControl)

                            If blnFits Then
                                'If it can fit Then show below message
                                strDefault = "Equipment ID {0} has alrady been assigned to an existing appointment with the following Orders: {1}. Would you like to include this order as part of the same appointment?"
                                Dim s = oLocalize.GetLocalizedValueByKey("SchedQAddOrderToEquipID", strDefault)
                                eVal.ErrMsg = String.Format(s, EquipmentID, strOrders)
                                eVal.Success = False
                                eVal.IsAdd = True
                                eVal.ApptControl = bookAssigned.BookAMSPickupApptControl
                                Return
                            Else
                                'If it Then wont fit say it already exists And you have To enter a different equip ID
                                strDefault = "Cannot save EquipmentID for Pickup because this ID is already in use. Enter a different Equipment ID."
                                eVal.ErrMsg = oLocalize.GetLocalizedValueByKey("SchedSaveEquipIDFailPickInUse", strDefault)
                                eVal.Success = False
                                eVal.IsAdd = False
                                Return
                            End If
                        End If
                    Else
                        'Delivery
                        If IsTranfer Then
                            strDefault = "Editing EquipmentID is not allowed for Transfer Delivery"
                            eVal.ErrMsg = oLocalize.GetLocalizedValueByKey("SchedSaveEquipIDFailDelTransfer", strDefault)
                            eVal.Success = False
                            eVal.IsAdd = False
                            Return
                        Else
                            If Not Inbound Then
                                strDefault = "Editing EquipmentID is not allowed for Outbound Delivery"
                                eVal.ErrMsg = oLocalize.GetLocalizedValueByKey("SchedSaveEquipIDFailDelOut", strDefault)
                                eVal.Success = False
                                eVal.IsAdd = False
                                Return
                            End If
                        End If
                        'Delivery EquipID can only be assinged to orders with the same RequiredDate
                        'If this EquipmentID is being used on another record with the same Carrier, Company, Inbound/Outbound, and is within intRefreshDays of the RequiredDate then we cannot use it                    
                        Dim blnInUse = (From d In db.Books Join l In db.LaneRefBooks On d.BookODControl Equals l.LaneControl
                                        Where
                                        d.BookCarrTrailerNo = EquipmentID AndAlso d.BookCustCompControl = CompControl AndAlso d.BookCarrierControl = Parameters.UserCarrierControl AndAlso l.LaneOriginAddressUse = Inbound _
                                        AndAlso
                                        ((d.BookDateRequired.Value.Date <= RequiredDate.AddDays(intRefreshDays).Date AndAlso d.BookDateRequired.Value.Date > RequiredDate.Date) OrElse (d.BookDateRequired.Value.Date >= RequiredDate.AddDays(-intRefreshDays).Date AndAlso d.BookDateRequired.Value.Date < RequiredDate.Date))
                                        Select d).Any()
                        If blnInUse Then
                            strDefault = "Cannot save EquipmentID for Delivery because this ID is already in use. Enter a different Equipment ID."
                            eVal.ErrMsg = oLocalize.GetLocalizedValueByKey("SchedSaveEquipIDFailDelInUse", strDefault)
                            eVal.Success = False
                            eVal.IsAdd = False
                            Return
                        End If

                        'If the Then carrier enters the same equipment ID after the original appointment Is Set For an additional order we have To 
                        'reevaluate the validation for available time slots - may need to cancel the appointment - give carrier option to cancel appointment And reschedule if validation fails 
                        'check If there Is a pickup Or delivery control - if control ahs already been assigned then we have to revalidate that it can fit - recalculate the minutes needed
                        Dim bookAssigned = (From d In db.Books Join l In db.LaneRefBooks On d.BookODControl Equals l.LaneControl
                                            Where
                                            d.BookCarrTrailerNo = EquipmentID AndAlso d.BookCustCompControl = CompControl _
                                            AndAlso d.BookCarrierControl = Parameters.UserCarrierControl AndAlso l.LaneOriginAddressUse = Inbound _
                                            AndAlso d.BookDateRequired = RequiredDate AndAlso d.BookAMSDeliveryApptControl <> 0
                                            Select d).FirstOrDefault()

                        If Not bookAssigned Is Nothing Then

                            Dim books = GetDependentBooksByEquipID(bookAssigned.BookControl, EquipmentID, CompControl, LoadDate, eVal.IsPickup, Inbound, True) 'This method now calls udfGetAMSDependentBookControls instead of udfGetDependentBookControlsByEquipID 2/13/20 
                            Dim oDockSet As New NGLDockSettingData(Parameters)

                            Dim bList = books.ToList()
                            bList.Add(bookAssigned)
                            Dim strOrders As String = ""
                            Dim blnFits = oDockSet.DoesOrderFitOnAppointment(strOrders, bList.ToArray(), bookAssigned.BookAMSDeliveryApptControl)

                            If blnFits Then
                                'If it can fit Then show below message
                                strDefault = "Equipment ID {0} has alrady been assigned to an existing appointment with the following Orders: {1}. Would you like to include this order as part of the same appointment?"
                                Dim s = oLocalize.GetLocalizedValueByKey("SchedQAddOrderToEquipID", strDefault)
                                eVal.ErrMsg = String.Format(s, EquipmentID, strOrders)
                                eVal.Success = False
                                eVal.IsAdd = True
                                eVal.ApptControl = bookAssigned.BookAMSDeliveryApptControl
                                Return
                            Else
                                'If it Then wont fit say it already exists And you have To enter a different equip ID
                                strDefault = "Cannot save EquipmentID for Delivery because this ID is already in use. Enter a different Equipment ID."
                                eVal.ErrMsg = oLocalize.GetLocalizedValueByKey("SchedSaveEquipIDFailDelInUse", strDefault)
                                eVal.Success = False
                                eVal.IsAdd = False
                                Return
                            End If
                        End If
                    End If

                    'Modified By LVV as part of bug fix - must update all orders on CNS
                    Dim ctrls = (From d In db.Books
                                 Join x In db.udfGetDependentBookControls(pBookControl, True)
                                 On d.BookControl Equals x.BookControl
                                 Select d).ToArray()

                    For Each c In ctrls
                        c.BookCarrTrailerNo = EquipmentID 'In the future we may want to optimize this to improve performance (spUpdateBookDependencies)
                    Next

                    ''ltsBook.BookCarrTrailerNo = EquipmentID 'In the future we may want to optimize this to improve performance (spUpdateBookDependencies)

                    db.SubmitChanges()

                    eVal.Success = True
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveEquipmentID"))
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Scheduler - Gets all related Book records by SHID and EquipID groupings
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="EquipID"></param>
    ''' <param name="CompControl"></param>
    ''' <param name="dt"></param>
    ''' <param name="IsPickup"></param>
    ''' <param name="Inbound"></param>
    ''' <param name="IncludeLTLPool"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 8/14/18 for v-8.3 TMS365 Scheduler
    ''' Modified By LVV on 2/13/20
    '''  Rob to said replace all instances of udfGetDependentBookControlsByEquipID with udfGetAMSDependentBookControls
    ''' </remarks>
    Public Function GetDependentBooksByEquipID(ByVal BookControl As Integer,
                                               ByVal EquipID As String,
                                               ByVal CompControl As Integer,
                                               ByVal dt As Date,
                                               ByVal IsPickup As Boolean,
                                               ByVal Inbound As Boolean,
                                               Optional ByVal IncludeLTLPool As Boolean = True) As LTS.Book()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oDLO As New DataLoadOptions
                oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.CompRefBook)
                oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookLoads)
                oDLO.LoadWith(Of LTS.BookLoad)(Function(t As LTS.BookLoad) t.BookItems)
                oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.LaneRefBook)
                db.LoadOptions = oDLO
                'Modified By LVV on 2/13/20
                Dim oRet = (From d In db.Books
                            Join x In db.udfGetAMSDependentBookControls(BookControl, IncludeLTLPool, IsPickup)
                            On d.BookControl Equals x.BookControl
                            Select d).ToArray()
                'Dim oRet = (From d In db.Books
                '            Join x In db.udfGetDependentBookControlsByEquipID(BookControl, IncludeLTLPool, EquipID, CompControl, IsPickup, dt, Inbound)
                '                On d.BookControl Equals x.BookControl
                '            Select d).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetDependentBooksByEquipID"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Scheduler - Gets BookControls by SHID and EquipID groupings
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="EquipID"></param>
    ''' <param name="CompControl"></param>
    ''' <param name="dt"></param>
    ''' <param name="IsPickup"></param>
    ''' <param name="Inbound"></param>
    ''' <param name="IncludeLTLPool"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 8/31/18 for v-8.3 TMS365 Scheduler
    ''' Modified By LVV on 2/13/20
    '''  Rob to said replace all instances of udfGetDependentBookControlsByEquipID with udfGetAMSDependentBookControls
    ''' </remarks>
    Public Function GetDependentBookControlsByEquipID(ByVal BookControl As Integer,
                                                      ByVal EquipID As String,
                                                      ByVal CompControl As Integer,
                                                      ByVal dt As Date,
                                                      ByVal IsPickup As Boolean,
                                                      ByVal Inbound As Boolean,
                                                      Optional ByVal IncludeLTLPool As Boolean = True) As List(Of Integer)
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim bookControls As New List(Of Integer)
                Dim oRet = (From d In db.udfGetAMSDependentBookControls(BookControl, IncludeLTLPool, IsPickup) Select d).ToArray() 'Modified By LVV on 2/13/20
                'Dim oRet = (From d In db.udfGetDependentBookControlsByEquipID(BookControl, IncludeLTLPool, EquipID, CompControl, IsPickup, dt, Inbound) Select d).ToArray()
                For Each r In oRet
                    bookControls.Add(r.BookControl)
                Next
                Return bookControls
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetDependentBookControlsByEquipID"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Returns Books with BookLoad, BookItem, Comp, and Lane for the provided BookControls
    ''' </summary>
    ''' <param name="BookControls"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 8/31/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Public Function GetBookByControls(ByVal BookControls As List(Of Integer)) As LTS.Book()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oDLO As New DataLoadOptions
                oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.CompRefBook)
                oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookLoads)
                oDLO.LoadWith(Of LTS.BookLoad)(Function(t As LTS.BookLoad) t.BookItems)
                oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.LaneRefBook)
                db.LoadOptions = oDLO

                Dim oRet = (From d In db.Books
                            Where BookControls.Contains(d.BookControl)
                            Select d).ToArray()

                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetBookByControls"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Scheduler specific
    ''' Rules:
    '''  1. Only select orders where the SHID matches; if the SHID is blank only add additional orders if consolidation is on and the CNS numbers must match
    '''  2. For Pickup filter by OriginCompControl and BookDateLoad
    '''  3. For Delivery filter by DestCompControl and BookDateRequired
    '''  4. EquipmentID matches (if EquipmentID is not blank)
    '''  5. Carrier must match
    '''  6. Outbound must be true and BookRouteConsFlag must be off for LTL Pool
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="IsPickup"></param>
    ''' <param name="IncludeLTLPool"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 6/5/19
    ''' </remarks>
    Public Function GetAMSDependentBooks(ByVal BookControl As Integer, ByVal IsPickup As Boolean, Optional ByVal IncludeLTLPool As Boolean = True) As LTS.Book()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oDLO As New DataLoadOptions
                oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.CompRefBook)
                oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookLoads)
                oDLO.LoadWith(Of LTS.BookLoad)(Function(t As LTS.BookLoad) t.BookItems)
                oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.LaneRefBook)
                db.LoadOptions = oDLO

                Dim oRet = (From d In db.Books
                            Join x In db.udfGetAMSDependentBookControls(BookControl, IncludeLTLPool, IsPickup)
                            On d.BookControl Equals x.BookControl
                            Select d).ToArray()

                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAMSDependentBooks"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Wrapper method to call GetAMSDependentBooks when the caller doesn't have the IsPickup value.
    ''' Scheduler specific
    ''' Rules:
    '''  1. Only select orders where the SHID matches; if the SHID is blank only add additional orders if consolidation is on and the CNS numbers must match
    '''  2. For Pickup filter by OriginCompControl and BookDateLoad
    '''  3. For Delivery filter by DestCompControl and BookDateRequired
    '''  4. EquipmentID matches (if EquipmentID is not blank)
    '''  5. Carrier must match
    '''  6. Outbound must be true and BookRouteConsFlag must be off for LTL Pool
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="IncludeLTLPool"></param>
    ''' <returns>LTS.Book()</returns>
    ''' <remarks>
    ''' Added By LVV on 8/5/20 for v-8.3.0.001 - Task #202007231830 - Scheduler Concurrency
    ''' </remarks>
    Public Function GetAMSDependentBooks(ByVal BookControl As Integer, Optional ByVal IncludeLTLPool As Boolean = True) As LTS.Book()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim b = db.Books.Where(Function(x) x.BookControl = BookControl).FirstOrDefault()
                Dim IsPickup = IsBookAMSPickup(b.BookOrigCompControl, b.BookAMSPickupApptControl, b.BookCarrActualDate)
                Return GetAMSDependentBooks(BookControl, IsPickup, IncludeLTLPool)
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAMSDependentBooks"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Determines if the record is a Pickup based on the provided parameters
    ''' </summary>
    ''' <param name="BookOrigCompControl"></param>
    ''' <param name="BookAMSPickupApptControl"></param>
    ''' <param name="BookCarrActualDate"></param>
    ''' <returns></returns>
    ''' <remarks>Added By LVV on 8/7/20 for v-8.3.0.001 - Task #202007231830 - Scheduler Concurrency</remarks>
    Public Function IsBookAMSPickup(ByVal BookOrigCompControl As Integer, ByVal BookAMSPickupApptControl As Integer, ByVal BookCarrActualDate As Date?) As Boolean
        Dim IsPickup As Boolean = False
        Try
            If (BookOrigCompControl <> 0 And BookAMSPickupApptControl = 0) Then IsPickup = True 'This is a pickup that needs an appointment (appointment being assigned)
            If (BookOrigCompControl <> 0 And BookAMSPickupApptControl <> 0 And BookCarrActualDate.HasValue = False) Then IsPickup = True 'This is a pickup that already has a scheduled appointment (appointment being modified)
        Catch ex As Exception
            ManageLinqDataExceptions(ex, buildProcedureName("IsBookAMSPickup"))
        End Try
        Return IsPickup
    End Function

    ''' <summary>
    ''' Scheduler specific
    ''' Rules:
    '''  1. Only select orders where the SHID matches; if the SHID is blank only add additional orders if consolidation is on and the CNS numbers must match
    '''  2. For Pickup filter by OriginCompControl and BookDateLoad
    '''  3. For Delivery filter by DestCompControl and BookDateRequired
    '''  4. EquipmentID matches (if EquipmentID is not blank)
    '''  5. Carrier must match
    '''  6. Outbound must be true and BookRouteConsFlag must be off for LTL Pool
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="IsPickup"></param>
    ''' <param name="IncludeLTLPool"></param>
    ''' <returns>LTS.Book()</returns>
    ''' <remarks>
    ''' Added By LVV on 8/5/20 for v-8.3.0.001 - Task #202007231830 - Scheduler Concurrency
    '''  Moved the code that does the work to this method and turned the other ones into wrapper methods
    '''  Did this to minimize the amount of nested calls to the same data context
    ''' </remarks>
    Private Function GetAMSDependentBooks(ByRef db As NGLMasBookDataContext, ByVal BookControl As Integer, ByVal IsPickup As Boolean, Optional ByVal IncludeLTLPool As Boolean = True) As LTS.Book()
        Dim oDLO As New DataLoadOptions
        oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.CompRefBook)
        oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookLoads)
        oDLO.LoadWith(Of LTS.BookLoad)(Function(t As LTS.BookLoad) t.BookItems)
        oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.LaneRefBook)
        db.LoadOptions = oDLO
        Dim oRet = (From d In db.Books
                    Join x In db.udfGetAMSDependentBookControls(BookControl, IncludeLTLPool, IsPickup)
                    On d.BookControl Equals x.BookControl
                    Select d).ToArray()
        Return oRet
    End Function


#End Region

#Region "NGL Accounting Specific Methods"

    Public Function GetvNGLAccounting(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.vNGLAccounting()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vNGLAccounting
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.vNGLAccounting)
                iQuery = db.vNGLAccountings
                Dim filterWhere = ""

                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetvNGLAccounting"), db)
            End Try
        End Using
        Return Nothing
    End Function

    Public Function GetvNGLAccountingConsSum(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.vNGLAccountingConsSum()
        Dim oRet As LTS.vNGLAccountingConsSum()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.vNGLAccountingConsSum)
                iQuery = (From d In db.vNGLAccountingConsSums Order By d.BookConsPrefix, d.BookStopNo, d.BookProNumber)
                Dim filterWhere = ""

                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetvNGLAccountingConsSum"), db)
            End Try
            Return Nothing
        End Using
    End Function

    Public Function GetNGLAcctConsSumTotals(ByVal CNS As String) As LTS.vNGLAccountingConsSum
        Dim oRet As New LTS.vNGLAccountingConsSum
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim books = db.Books.Where(Function(x) x.BookConsPrefix = CNS).ToArray()
                Dim dist = books.Sum(Function(x) x.BookMilesFrom).GetValueOrDefault()
                Dim cases = books.Sum(Function(x) x.BookTotalCases).GetValueOrDefault()
                Dim wgt = books.Sum(Function(x) x.BookTotalWgt).GetValueOrDefault()
                Dim plts = books.Sum(Function(x) x.BookTotalPL).GetValueOrDefault()
                Dim cube = books.Sum(Function(x) x.BookTotalCube).GetValueOrDefault()
                Dim bfc = books.Sum(Function(x) x.BookRevBilledBFC).GetValueOrDefault()
                Dim total = books.Sum(Function(x) x.BookRevTotalCost).GetValueOrDefault()
                Dim savings = books.Sum(Function(x) x.BookRevLoadSavings).GetValueOrDefault()
                With oRet
                    .BookMilesFrom = dist
                    .BookTotalCases = cases
                    .BookTotalWgt = wgt
                    .BookTotalPL = plts
                    .BookTotalCube = cube
                    .BookRevBilledBFC = bfc
                    .BookRevTotalCost = total
                    .BookRevLoadSavings = savings
                End With
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetNGLAcctConsSumTotals"), db)
            End Try
            Return Nothing
        End Using
    End Function


    ''' <summary>
    ''' Creates a new Booking record on the provided CNS by looking up the most recent PPR Pro and
    ''' duplicates fields and inserts other defaults
    ''' </summary>
    ''' <param name="CNS">This will be the CNS of the new record</param>
    ''' <returns></returns>
    Public Function CreateDefaultPPR(ByVal CNS As String) As Models.ResultObject
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim retVal As New Models.ResultObject With {.Control = 0, .Success = False}
                Dim spRes = (From d In db.spCreateDefaultPPR(CNS, Parameters.UserName) Select d).FirstOrDefault()
                If spRes.ErrNumber > 0 Then
                    retVal.ErrMsg = spRes.RetMsg
                Else
                    retVal.Success = True
                    retVal.Control = spRes.BookControl
                End If
                Return retVal
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("CreateDefaultPPR"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Used in the 365 NGL Accounting Screen to update specifically 'PPR' records
    ''' aka special booking records so Bill can pay Paul (Company #5000 PPR Freight Services, Inc.)
    ''' </summary>
    ''' <param name="oRecord"></param>
    ''' <returns></returns>
    ''' <remarks>Added By LVV on 1/3/20</remarks>
    Public Function UpdatePPR(ByVal oRecord As LTS.vNGLAccounting) As Models.ResultObject
        Using db As New NGLMasBookDataContext(ConnectionString)
            Dim oResult As New Models.ResultObject()
            Try
                Dim book = db.Books.Where(Function(x) x.BookControl = oRecord.BookControl).FirstOrDefault()
                With book
                    .BookCarrierControl = oRecord.BookCarrierControl
                    .BookCarrierContControl = oRecord.BookCarrierContControl
                    .BookCarrierContact = oRecord.BookCarrierContact
                    .BookCarrierContactPhone = oRecord.BookCarrierContactPhone
                    .BookDateOrdered = oRecord.BookDateOrdered
                    .BookDateLoad = oRecord.BookDateLoad
                    .BookDateRequired = oRecord.BookDateRequired
                    .BookDateDelivered = oRecord.BookDateDelivered
                    .BookCarrActDate = oRecord.BookCarrActDate
                    .BookTranCode = oRecord.BookTranCode
                    .BookPayCode = oRecord.BookPayCode
                    .BookRouteConsFlag = oRecord.BookRouteConsFlag
                    .BookRevBilledBFC = oRecord.BookRevBilledBFC
                    .BookRevCarrierCost = oRecord.BookRevCarrierCost
                    .BookRevLineHaul = oRecord.BookRevLineHaul
                    .BookRevTotalCost = oRecord.BookRevTotalCost
                    .BookRevLoadSavings = oRecord.BookRevLoadSavings
                    .BookRevCommPercent = oRecord.BookRevCommPercent
                    .BookRevCommCost = oRecord.BookRevCommCost
                    .BookRevGrossRevenue = oRecord.BookRevGrossRevenue
                    .BookFinARInvoiceDate = oRecord.BookFinARInvoiceDate
                    .BookFinARInvoiceAmt = oRecord.BookFinARInvoiceAmt
                    .BookFinARPayDate = oRecord.BookFinARPayDate
                    .BookFinARPayAmt = oRecord.BookFinARPayAmt
                    .BookFinARCheck = oRecord.BookFinARCheck
                    '.BookFinARBalance = oRecord.BookFinARBalance
                    '.BookFinARCurType = oRecord.BookFinARCurType
                    .BookFinAPBillNumber = oRecord.BookFinAPBillNumber
                    .BookFinAPBillNoDate = oRecord.BookFinAPBillNoDate
                    '.BookFinAPBillInvDate = oRecord.BookFinAPBillInvDate
                    .BookFinAPStdCost = oRecord.BookFinAPStdCost
                    .BookFinAPActCost = oRecord.BookFinAPActCost
                    '.BookFinAPPayDate = oRecord.BookFinAPPayDate
                    '.BookFinAPPayAmt = oRecord.BookFinAPPayAmt
                    .BookFinAPCheck = oRecord.BookFinAPCheck
                    .BookFinAPGLNumber = oRecord.BookFinAPGLNumber
                    '.BookFinAPCurType = oRecord.BookFinAPCurType
                    .BookModDate = Date.Now
                    .BookModUser = Parameters.UserName
                End With
                db.SubmitChanges()
                oResult.Success = True
                oResult.SuccessMsg = getLocalizedString("M_Success", "Success!")
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdatePPR"), db)
            End Try
            Return oResult
        End Using
    End Function

    ''' <summary>
    ''' Used in the 365 NGL Accounting Screen to update specifically the other non 'PPR' records on the CNS 
    ''' Part of special process so Bill can pay Paul (Company #5000 PPR Freight Services, Inc.)
    ''' </summary>
    ''' <param name="oRecord"></param>
    ''' <returns></returns>
    ''' <remarks>Added By LVV on 1/3/20</remarks>
    Public Function UpdateNGLAccountingConsSum(ByVal oRecord As LTS.vNGLAccounting) As Models.ResultObject
        Using db As New NGLMasBookDataContext(ConnectionString)
            Dim oResult As New Models.ResultObject()
            Try
                Dim book = db.Books.Where(Function(x) x.BookControl = oRecord.BookControl).FirstOrDefault()
                With book
                    .BookTranCode = oRecord.BookTranCode
                    .BookPayCode = oRecord.BookPayCode
                    .BookRevCarrierCost = oRecord.BookRevCarrierCost
                    .BookFinAPBillNumber = oRecord.BookFinAPBillNumber
                    .BookFinAPBillNoDate = oRecord.BookFinAPBillNoDate
                    .BookFinAPActCost = oRecord.BookFinAPActCost
                    .BookModDate = Date.Now
                    .BookModUser = Parameters.UserName
                End With
                db.SubmitChanges()
                oResult.Success = True
                oResult.SuccessMsg = getLocalizedString("M_Success", "Success!")
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateNGLAccountingConsSum"), db)
            End Try
            Return oResult
        End Using
    End Function



    ''' <summary>
    ''' Creates a new Booking record by duplicating fields from the most recent Book for the Carrier and inserts other defaults
    ''' New Record Costs will be the provided Cost
    ''' </summary>
    ''' <param name="CarrierControl"></param>
    ''' <param name="Amount"></param>
    ''' <returns></returns>
    Public Function CreateNGLExpense(ByVal CarrierControl As Integer, ByVal Amount As Decimal) As Models.ResultObject
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim retVal As New Models.ResultObject With {.Control = 0, .Success = False}
                Dim spRes = (From d In db.spCreateNGLExpense(CarrierControl, Amount, Parameters.UserName) Select d).FirstOrDefault()
                If spRes.ErrNumber > 0 Then
                    retVal.ErrMsg = spRes.RetMsg
                Else
                    retVal.Success = True
                    retVal.Control = spRes.BookControl
                End If
                Return retVal
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("CreateNGLExpense"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Used By 365 NGL Expenses screen
    ''' Save method for changes made to expense records
    ''' </summary>
    ''' <param name="oRecord"></param>
    ''' <returns></returns>
    ''' <remarks>Added By LVV on 12/30/19</remarks>
    Public Function UpdateNGLExpense(ByVal oRecord As LTS.vNGLAccounting) As Models.ResultObject
        Using db As New NGLMasBookDataContext(ConnectionString)
            Dim oResult As New Models.ResultObject()
            Try
                Dim book = db.Books.Where(Function(x) x.BookControl = oRecord.BookControl).FirstOrDefault()
                With book
                    .BookDateLoad = oRecord.BookDateLoad
                    .BookDateRequired = oRecord.BookDateRequired
                    .BookDateOrdered = oRecord.BookDateOrdered
                    .BookDateDelivered = oRecord.BookDateDelivered
                    .BookCarrActDate = oRecord.BookCarrActDate
                    .BookTranCode = oRecord.BookTranCode
                    .BookPayCode = oRecord.BookPayCode
                    '.BookRevBilledBFC = oRecord.BookRevBilledBFC
                    .BookRevCarrierCost = oRecord.BookRevCarrierCost
                    '.BookRevLineHaul = oRecord.BookRevLineHaul
                    '.BookRevTotalCost = oRecord.BookRevTotalCost
                    '.BookRevLoadSavings = oRecord.BookRevLoadSavings
                    '.BookRevCommPercent = oRecord.BookRevCommPercent
                    '.BookRevCommCost = oRecord.BookRevCommCost
                    '.BookRevGrossRevenue = oRecord.BookRevGrossRevenue
                    '.BookFinARInvoiceDate = oRecord.BookFinARInvoiceDate
                    '.BookFinARInvoiceAmt = oRecord.BookFinARInvoiceAmt
                    '.BookFinARPayDate = oRecord.BookFinARPayDate
                    '.BookFinARPayAmt = oRecord.BookFinARPayAmt
                    '.BookFinARCheck = oRecord.BookFinARCheck
                    '.BookFinARBalance = oRecord.BookFinARBalance
                    '.BookFinARCurType = oRecord.BookFinARCurType
                    .BookFinAPBillNumber = oRecord.BookFinAPBillNumber
                    .BookFinAPBillNoDate = oRecord.BookFinAPBillNoDate
                    .BookFinAPBillInvDate = oRecord.BookFinAPBillInvDate
                    .BookFinAPStdCost = oRecord.BookFinAPStdCost
                    .BookFinAPActCost = oRecord.BookFinAPActCost
                    .BookFinAPPayDate = oRecord.BookFinAPPayDate
                    .BookFinAPPayAmt = oRecord.BookFinAPPayAmt
                    .BookFinAPCheck = oRecord.BookFinAPCheck
                    .BookFinAPGLNumber = oRecord.BookFinAPGLNumber
                    .BookFinAPCurType = oRecord.BookFinAPCurType
                    .BookModDate = Date.Now
                    .BookModUser = Parameters.UserName
                End With
                db.SubmitChanges()
                oResult.Success = True
                oResult.SuccessMsg = getLocalizedString("M_Success", "Success!")
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateNGLExpense"), db)
            End Try
            Return oResult
        End Using
    End Function

#End Region

#End Region

    ''' <summary>
    ''' Returns the ModeType and TempType for a Consolidated or Single load by BookControl based on the Order of Precedence.
    ''' Mode OP = Air, Sea, Rail, Road
    ''' Temp OP = Mixed/Any, Frozen, Cooler/Ref, Dry
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="ModeType"></param>
    ''' <param name="TempType"></param>
    ''' <remarks>
    ''' Added By LVV on 10/3/16 for v-7.0.5.110 Mode/Temp Type Precedence
    ''' </remarks>
    Public Sub GetModeTempTypesByPrecedence(ByVal BookControl As Integer, ByRef ModeType As Integer, ByRef TempType As Integer)
        Using operation = Logger.StartActivity("GetModeTempTypesByPrecedence(BookControl: {BookControl}, ModeType: {ModeType}, TempType:{TempType}", BookControl, ModeType, TempType)
            Using db As New NGLMasBookDataContext(ConnectionString)
                Try
                    Dim res = (From d In db.spGetModeTempTypesByPrecedence(BookControl) Select d).FirstOrDefault()
                    ModeType = res.ModeType
                    TempType = res.TempType
                Catch ex As Exception
                    operation.Complete(exception:=ex)
                    Logger.Error(ex, "Error in GetModeTempTypesByPrecedence")
                End Try
            End Using
        End Using
    End Sub

    ''' <summary>
    ''' A wrapper of the above method so the client can call spGetModeTempTypesByPrecedence
    ''' ByRef won't work in the Service layer so I had to put the results in a WCFResults object
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <returns>DTO.WCFResults</returns>
    ''' <remarks>
    ''' Added By LVV on 10/4/16 for v-7.0.5.110 Mode/Temp Type Precedence
    ''' </remarks>
    Public Function GetModeTempTypesByPrecedenceClient(ByVal BookControl As Integer) As DataTransferObjects.WCFResults
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim wcfRet As New DataTransferObjects.WCFResults
                Dim ModeType As Integer = 0
                Dim TempType As Integer = 0
                GetModeTempTypesByPrecedence(BookControl, ModeType, TempType)

                wcfRet.updateKeyFields("ModeType", ModeType)
                wcfRet.updateKeyFields("TempType", TempType)
                wcfRet.Success = True

                Return wcfRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetModeTempTypesByPrecedenceClient"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Calls spCloseOutSpecificLoads which is used to mark 
    ''' loads on CNS associated with @BookControl as PD and IC
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <remarks>
    ''' Added By LVV on 10/24/16 for v-7.0.5.110 Spot Save Action
    ''' </remarks>
    Public Sub CloseOutSpecificLoads(ByVal BookControl As Integer)
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oRet = (From d In db.spCloseOutSpecificLoads(BookControl)
                            Select New DataTransferObjects.GenericResults With {.ErrNumber = d.ErrNumber,
                            .RetMsg = d.RetMsg}).FirstOrDefault
                If Not oRet Is Nothing AndAlso oRet.ErrNumber <> 0 Then
                    throwSQLFaultException(oRet.RetMsg)
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("CloseOutSpecificLoads"))
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Updates CNS for either only the BookControl or all associated loads
    ''' </summary>
    ''' <param name="NewCNS"></param>
    ''' <param name="BookControl"></param>
    ''' <param name="blnUpdateAll"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 11/16/16 for v-7.0.5.110 NextCons Context Menu
    ''' </remarks>
    Public Function UpdateBookConsPrefix(ByVal NewCNS As String, ByVal BookControl As Integer, ByVal blnUpdateAll As Boolean) As DataTransferObjects.GenericResults
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oRet = (From d In db.spUpdateBookConsPrefix(NewCNS, BookControl, blnUpdateAll)
                            Select New DataTransferObjects.GenericResults With {.ErrNumber = d.ErrNumber,
                            .RetMsg = d.RetMsg}).FirstOrDefault

                If Not oRet Is Nothing AndAlso oRet.ErrNumber > 10 Then
                    throwSQLFaultException(oRet.RetMsg)
                End If

                Return oRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateBookConsPrefix"))
            End Try
            Return Nothing
        End Using
    End Function


    ''' <summary>
    ''' Returns the cost per pound for the selected order rounded to 4 decimal points.  Uses Alpha code and legal entity to get the Company Informaiton;
    ''' unless an optional CompNumber is provieded; then it use the compnumber to lookup the order information
    ''' </summary>
    ''' <param name="BookCarrOrderNumber"></param>
    ''' <param name="BookOrderSequence"></param>
    ''' <param name="CompAlphaCode"></param>
    ''' <param name="CompLegalEntity"></param>
    ''' <param name="CompNumber"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR 11/27/2017 for v-7.0.6.105
    ''' Calculate the cost per pound for the provided order informaiton.
    ''' If multiple matching orders are found the most expensive order is used
    ''' If the total weight or total cost are zero or less they are converted to a value of one
    ''' </remarks>
    Public Function GetCostPerPoundForOrder(ByVal BookCarrOrderNumber As String, ByVal BookOrderSequence As Integer, ByVal CompAlphaCode As String, ByVal CompLegalEntity As String, Optional ByVal CompNumber As Integer = 0) As Double
        Dim dblRetVal As Double = 0
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim intCompControl? As Integer = 0
                Dim dbltotalWeight As Double = 1
                Dim dbltotalcost As Double = 1
                If CompNumber <> 0 Then
                    intCompControl = (From c In db.CompRefBooks Where c.CompNumber = CompNumber Select c.CompControl).FirstOrDefault()
                Else
                    intCompControl = (From c In db.CompRefBooks Where c.CompAlphaCode = CompAlphaCode And c.CompLegalEntity = CompLegalEntity Select c.CompControl).FirstOrDefault()
                End If
                'Dim oBookData As New LTS.Book
                If intCompControl.HasValue AndAlso intCompControl.Value <> 0 Then
                    Dim oBookData = (From b In db.Books Where b.BookCarrOrderNumber = BookCarrOrderNumber And b.BookOrderSequence = BookOrderSequence And b.BookCustCompControl = intCompControl.Value Order By b.BookRevTotalCost Descending Select b.BookControl, b.BookTotalWgt, b.BookRevTotalCost).FirstOrDefault()
                    If Not oBookData Is Nothing AndAlso oBookData.BookControl <> 0 Then
                        If oBookData.BookTotalWgt.HasValue AndAlso oBookData.BookTotalWgt > 0 Then
                            dbltotalWeight = oBookData.BookTotalWgt.Value
                        End If
                        If oBookData.BookRevTotalCost.HasValue() AndAlso oBookData.BookRevTotalCost.Value > 0 Then
                            dbltotalcost = System.Convert.ToDouble(oBookData.BookRevTotalCost.Value)
                            If dbltotalcost > 0 Then
                                dblRetVal = Math.Round((dbltotalcost / dbltotalWeight), 4)
                            End If
                        End If
                    End If

                End If
                Return dblRetVal

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCostPerPoundForOrder"), db)
            End Try

            Return dblRetVal

        End Using
    End Function

    ''' <summary>
    ''' Returns a list of orders in spPendingOrdersExpired365Result object.
    ''' The caller must check for errors by checking the ErrNumber and RetMsg values. If an error exists only one record is returned.  
    ''' The CompControl is used to lookup the AllowedMinutes parameter.  
    ''' If CompControl = 0 expired records from all companies are returned. Generally when companies have different AllowedMinutes 
    ''' specific companies are processed first then a value of zero for CompControl is used to return all the remaining 
    ''' orders with default AllowedMinutes. 
    ''' If AllowedMinuts is less than 1 the system will use the GlobalDefaultLoadAcceptAllowedMinutes parameter setting.
    ''' </summary>
    ''' <param name="CompControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPendingExpiredOrders365(ByVal CompControl As Integer) As List(Of LTS.spPendingOrdersExpired365Result)
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Return db.spPendingOrdersExpired365(CompControl).ToList()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetPendingExpiredOrders365"))
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Returns true if this load can go LTL, see new business rules in v-8.2.0.116
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="CarrierControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2.0.117 on 07/18/2019
    '''   Includes logic to check for Multi-Pick LTL Settings by comp And carrier
    '''   Rules Summary:
    '''     1. Check if Multi-Stop (Diffrent Dest Addreses) returns false on all Mult-Stop loads 
    '''     2. Check If multi-pick (Different Pick Up Addresses)
    '''     3. If Not Multi-Pick Or Multi-Stop returns true
    '''     4. if Multi-Pick all carrier and company settings must match
    '''         a) Carrier's Legal Entity Allow LTL Multi-Pick Consolidation flag must be true
    '''         b) All pickup locations (comps) must have the same AllowMultiPickLTLConsolidations parameter setting 
    '''            And the AllowMultiPickLTLConsolidations parameter  must not be zero
    ''' </remarks>
    Public Function CanLoadGoLTL(ByVal BookControl As Integer, Optional ByVal CarrierControl As Integer = 0) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim bVal As Integer? = db.udfCanLoadGoLTL(BookControl, CarrierControl)
                If bVal.HasValue Then blnRet = bVal.Value
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetPendingExpiredOrders365"))
            End Try
        End Using
        Return blnRet
    End Function

#End Region

#Region "Protected Methods"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = TryCast(oData, DataTransferObjects.Book)
        Dim strCompAbrev As String = getScalarString("Select top 1 isnull(dbo.Comp.CompAbrev,'') From dbo.Comp Where dbo.Comp.CompControl = " & d.BookCustCompControl)
        Dim strBookPro As String = strCompAbrev & d.BookProBase
        Dim skipObjs As New List(Of String) From {"BookUpdated", "BookModDate", "BookModUser", "Page", "Pages", "RecordCount", "PageSize"}
        Dim oLTS As New LTS.Book
        oLTS = CopyMatchingFields(oLTS, d, skipObjs)
        'Create New Record
        With oLTS
            .BookModDate = Date.Now
            .BookModUser = Me.Parameters.UserName
            .BookUpdated = If(d.BookUpdated Is Nothing, New Byte() {}, d.BookUpdated)
        End With
        Return oLTS

    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Dim source As LTS.Book = TryCast(LinqTable, LTS.Book)
        If source Is Nothing Then Return Nothing
        Return GetBookFiltered(Control:=source.BookControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim source As LTS.Book = TryCast(LinqTable, LTS.Book)
        If source Is Nothing Then Return Nothing
        Return QuickSaveResults(source.BookControl)
    End Function

    Public Function QuickSaveResults(ByVal BookControl As Integer) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                ret = (From d In db.Books
                       Where d.BookControl = BookControl
                       Select New DataTransferObjects.QuickSaveResults With {.Control = d.BookControl _
                           , .ModDate = d.BookModDate _
                           , .ModUser = d.BookModUser _
                           , .Updated = d.BookUpdated.ToArray}).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("QuickSaveResults"))
            End Try
        End Using
        Return ret
    End Function

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'Check if the data already exists only one allowed      
        With CType(oData, DataTransferObjects.Book)
            Try
                'Check if a company is selected
                If .BookCustCompControl = 0 Then
                    Utilities.SaveAppError("Cannot save new Book data.  A company has not been selected.  Please select a company and try again.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If
                If String.IsNullOrEmpty(.BookProNumber) OrElse .BookProNumber.Trim.Length < 1 Then
                    'We need to add the pro number
                    Dim PROBase As String = getScalarString("SELECT TOP 1 p.ParValue FROM dbo.parameter as p WHERE p.parkey = 'PRONUMBER'")
                    Dim intNextPro As Integer = 0
                    Integer.TryParse(PROBase, intNextPro)
                    intNextPro += 1
                    executeSQL("Update dbo.Parameter Set ParValue = " & intNextPro & " Where ParKey = 'PRONUMBER'")
                    Dim NewPRONumber = Trim(GetCustAbrev(.BookCustCompControl)) & intNextPro.ToString
                    .BookProBase = Left(intNextPro.ToString, 50)
                    .BookProNumber = NewPRONumber
                    .TrackingState = TrackingInfo.Updated
                Else
                    'verify that the bookpronumber is not in use
                    Dim Book As DataTransferObjects.Book = (
                            From t In CType(oDB, NGLMasBookDataContext).Books
                            Where
                            t.BookProNumber = .BookProNumber
                            Select New DataTransferObjects.Book With {.BookControl = t.BookControl}).First
                    If Not Book Is Nothing Then
                        Utilities.SaveAppError("Cannot save new Book data.  The Book Pro number, " & .BookProNumber & " ,  already exist.", Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                    End If
                End If
            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DataTransferObjects.Book)
            Try
                'Get the newest record that matches the provided criteria
                Dim Book As DataTransferObjects.Book = (
                        From t In CType(oDB, NGLMasBookDataContext).Books
                        Where
                        (t.BookControl <> .BookControl) _
                        And
                        (t.BookProNumber = .BookProNumber)
                        Select New DataTransferObjects.Book With {.BookControl = t.BookControl}).FirstOrDefault()

                If Not Book Is Nothing AndAlso Book.BookControl <> 0 Then
                    Utilities.SaveAppError("Cannot save Book changes.  The Book PRO Number, " & .BookProNumber & " already exist.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'Check if the Book is being used by the book data or the lane data
        With CType(oData, DataTransferObjects.Book)
            Try
                ''Add code here to call the Book and Lane data providers when they are created
                'Dim dpBook As New NGLBookData(Me.Parameters)
                'Dim dpLane As New NGLLaneData(Me.Parameters)
                'Dim oBooks() As DTO.Book
                'Dim oLanes() As DTO.Lane
                'Try
                '    oBooks = dpBook.GetBooksByBook(.BookControl)
                'Catch ex As FaultException
                '    If ex.Message <> "E_NoData" Then
                '        Throw
                '    End If
                'End Try
                'Try
                '    oLanes = dpLane.GetLanesByBook(.BookControl)
                'Catch ex As FaultException
                '    If ex.Message <> "E_NoData" Then
                '        Throw
                '    End If
                'End Try
                'If (Not oBooks Is Nothing AndAlso oBooks.Length > 0) OrElse (Not oLanes Is Nothing AndAlso oLanes.Length > 0) Then
                '    Utilities.SaveAppError("Cannot delete Book data.  The Book number, " & .BookNumber & " is being used and cannot be deleted. check the book or lane information.", Me.Parameters)
                '    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DataInUse"}, New FaultReason("E_DataValidationFailure"))
                'End If
            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    Protected Overrides Sub AddDetailsToLinq(ByRef LinqTable As Object, ByRef oData As DataTransferObjects.DTOBaseClass)

        With CType(LinqTable, LTS.Book)
            'Add Book Loads Records
            .BookLoads.AddRange(
                From d In CType(oData, DataTransferObjects.Book).BookLoads
                Select New LTS.BookLoad With {.BookLoadControl = d.BookLoadControl _
                , .BookLoadBookControl = d.BookLoadBookControl _
                , .BookLoadBuy = d.BookLoadBuy _
                , .BookLoadPONumber = d.BookLoadPONumber _
                , .BookLoadVendor = d.BookLoadVendor _
                , .BookLoadCaseQty = d.BookLoadCaseQty _
                , .BookLoadWgt = d.BookLoadWgt _
                , .BookLoadCube = d.BookLoadCube _
                , .BookLoadPL = d.BookLoadPL _
                , .BookLoadPX = d.BookLoadPX _
                , .BookLoadPType = d.BookLoadPType _
                , .BookLoadCom = d.BookLoadCom _
                , .BookLoadPUOrigin = d.BookLoadPUOrigin _
                , .BookLoadBFC = d.BookLoadBFC _
                , .BookLoadTotCost = d.BookLoadTotCost _
                , .BookLoadComments = d.BookLoadComments _
                , .BookLoadStopSeq = d.BookLoadStopSeq _
                , .BookLoadModDate = Date.Now _
                , .BookLoadModUser = Parameters.UserName _
                , .BookLoadUpdated = If(d.BookLoadUpdated Is Nothing, New Byte() {}, d.BookLoadUpdated)})
            'Add Book Notes Records
            .BookNotes.AddRange(
                From d In CType(oData, DataTransferObjects.Book).BookNotes
                Select New LTS.BookNote With {.BookNotesControl = d.BookNotesControl _
                , .BookNotesBookControl = d.BookNotesBookControl _
                , .BookNotesVisable1 = d.BookNotesVisable1 _
                , .BookNotesVisable2 = d.BookNotesVisable2 _
                , .BookNotesVisable3 = d.BookNotesVisable3 _
                , .BookNotesVisable4 = d.BookNotesVisable4 _
                , .BookNotesVisable5 = d.BookNotesVisable5 _
                , .BookNotesConfidential1 = d.BookNotesConfidential1 _
                , .BookNotesConfidential2 = d.BookNotesConfidential2 _
                , .BookNotesConfidential3 = d.BookNotesConfidential3 _
                , .BookNotesConfidential4 = d.BookNotesConfidential4 _
                , .BookNotesConfidential5 = d.BookNotesConfidential5 _
                , .BookNotesModDate = Date.Now _
                , .BookNotesModUser = Parameters.UserName _
                , .BookNotesUpdated = If(d.BookNotesUpdated Is Nothing, New Byte() {}, d.BookNotesUpdated)})
            'Add Book Track Records
            .BookTracks.AddRange(
                From d In CType(oData, DataTransferObjects.Book).BookTracks
                Select New LTS.BookTrack With {.BookTrackControl = d.BookTrackControl _
                , .BookTrackBookControl = d.BookTrackBookControl _
                , .BookTrackDate = d.BookTrackDate _
                , .BookTrackContact = d.BookTrackContact _
                , .BookTrackComment = d.BookTrackComment _
                , .BookTrackStatus = d.BookTrackStatus _
                , .BookTrackCommentLocalized = d.BookTrackCommentLocalized _
                , .BookTrackCommentKeys = d.BookTrackCommentKeys _
                , .BookTrackModUser = Parameters.UserName _
                , .BookTrackModDate = Date.Now _
                , .BookTrackUpdated = If(d.BookTrackUpdated Is Nothing, New Byte() {}, d.BookTrackUpdated)})


        End With
    End Sub

    Protected Overrides Sub InsertAllDetails(ByRef oDB As System.Data.Linq.DataContext, ByRef LinqTable As Object)
        With CType(oDB, NGLMasBookDataContext)
            .BookLoads.InsertAllOnSubmit(CType(LinqTable, LTS.Book).BookLoads)
            .BookNotes.InsertAllOnSubmit(CType(LinqTable, LTS.Book).BookNotes)
            .BookTracks.InsertAllOnSubmit(CType(LinqTable, LTS.Book).BookTracks)
        End With
    End Sub

    Protected Overrides Sub ProcessUpdatedDetails(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)

        With CType(oDB, NGLMasBookDataContext)

            ' Process any inserted bookload records  
            Dim oBookLoadChanges = NGLBookLoadData.GetBookLoadChanges(oData, TrackingInfo.Created, Me.Parameters.UserName)
            If Not oBookLoadChanges Is Nothing AndAlso oBookLoadChanges.Count > 0 Then _RecalcTotals = True
            .BookLoads.InsertAllOnSubmit(oBookLoadChanges)
            ' Process any updated bookload records
            oBookLoadChanges = NGLBookLoadData.GetBookLoadChanges(oData, TrackingInfo.Updated, Me.Parameters.UserName)
            If Not oBookLoadChanges Is Nothing AndAlso oBookLoadChanges.Count > 0 Then _RecalcTotals = True
            .BookLoads.AttachAll(oBookLoadChanges, True)
            ' Process any deleted bookload records
            Dim deletedLoadDetails = NGLBookLoadData.GetBookLoadChanges(oData, TrackingInfo.Deleted, Me.Parameters.UserName)
            If Not deletedLoadDetails Is Nothing AndAlso deletedLoadDetails.Count > 0 Then _RecalcTotals = True
            .BookLoads.AttachAll(deletedLoadDetails, True)
            .BookLoads.DeleteAllOnSubmit(deletedLoadDetails)
            'Process any BookItem Changes 
            For Each oRow As DataTransferObjects.BookLoad In TryCast(oData, DataTransferObjects.Book).BookLoads
                Dim blnItemsChanged As Boolean = False
                If oRow.TrackingState <> TrackingInfo.Deleted AndAlso ((Not oRow.BookItems Is Nothing) AndAlso oRow.BookItems.Count > 0) Then
                    ' Process any inserted bookitem records     
                    Dim oBookItemChanges = NGLBookItemData.GetBookItemChanges(oRow, TrackingInfo.Created, Me.Parameters.UserName)
                    If Not oBookItemChanges Is Nothing AndAlso oBookItemChanges.Count > 0 Then _ItemsChanged = True
                    .BookItems.InsertAllOnSubmit(oBookItemChanges)
                    ' Process any updated bookload records     
                    oBookItemChanges = NGLBookItemData.GetBookItemChanges(oRow, TrackingInfo.Updated, Me.Parameters.UserName)
                    If Not oBookItemChanges Is Nothing AndAlso oBookItemChanges.Count > 0 Then _ItemsChanged = True
                    .BookItems.AttachAll(oBookItemChanges, True)
                    ' Process any deleted bookload records
                    Dim deletedBookItems = NGLBookItemData.GetBookItemChanges(oRow, TrackingInfo.Deleted, Me.Parameters.UserName)
                    If Not deletedBookItems Is Nothing AndAlso deletedBookItems.Count > 0 Then _ItemsChanged = True
                    .BookItems.AttachAll(deletedBookItems, True)
                    .BookItems.DeleteAllOnSubmit(deletedBookItems)
                End If
                If blnItemsChanged Then
                    _RecalcTotals = True
                End If
            Next
            If _ItemsChanged Then _RecalcTotals = True

            ' Process any inserted BookNotes records 
            .BookNotes.InsertAllOnSubmit(GetBookNoteChanges(oData, TrackingInfo.Created))
            ' Process any updated BookNote records
            .BookNotes.AttachAll(GetBookNoteChanges(oData, TrackingInfo.Updated), True)
            ' Process any deleted BookNote records
            Dim deletedBudDetails = GetBookNoteChanges(oData, TrackingInfo.Deleted)
            .BookNotes.AttachAll(deletedBudDetails, True)
            .BookNotes.DeleteAllOnSubmit(deletedBudDetails)
            ' Process any inserted BookTrack records 
            .BookTracks.InsertAllOnSubmit(GetBookTrackChanges(oData, TrackingInfo.Created))
            ' Process any updated BookTrack records
            .BookTracks.AttachAll(GetBookTrackChanges(oData, TrackingInfo.Updated), True)
            ' Process any deleted BookTrack records
            Dim deletedTrackDetails = GetBookTrackChanges(oData, TrackingInfo.Deleted)
            .BookTracks.AttachAll(deletedTrackDetails, True)
            .BookTracks.DeleteAllOnSubmit(deletedTrackDetails)

        End With
    End Sub

    Protected Function GetBookNoteChanges(ByVal source As DataTransferObjects.Book, ByVal changeType As TrackingInfo) As List(Of LTS.BookNote)
        ' Test record details for specified change type.
        ' If Updated is null, set to byte[0] (for inserted items).
        Dim details As IEnumerable(Of LTS.BookNote) = (
                From d In source.BookNotes
                Where d.TrackingState = changeType
                Select New LTS.BookNote With {.BookNotesControl = d.BookNotesControl _
                , .BookNotesBookControl = d.BookNotesBookControl _
                , .BookNotesVisable1 = d.BookNotesVisable1 _
                , .BookNotesVisable2 = d.BookNotesVisable2 _
                , .BookNotesVisable3 = d.BookNotesVisable3 _
                , .BookNotesVisable4 = d.BookNotesVisable4 _
                , .BookNotesVisable5 = d.BookNotesVisable5 _
                , .BookNotesConfidential1 = d.BookNotesConfidential1 _
                , .BookNotesConfidential2 = d.BookNotesConfidential2 _
                , .BookNotesConfidential3 = d.BookNotesConfidential3 _
                , .BookNotesConfidential4 = d.BookNotesConfidential4 _
                , .BookNotesConfidential5 = d.BookNotesConfidential5 _
                , .BookNotesModDate = Date.Now _
                , .BookNotesModUser = Parameters.UserName _
                , .BookNotesUpdated = If(d.BookNotesUpdated Is Nothing, New Byte() {}, d.BookNotesUpdated)})
        Return details.ToList()
    End Function

    Protected Function GetBookTrackChanges(ByVal source As DataTransferObjects.Book, ByVal changeType As TrackingInfo) As List(Of LTS.BookTrack)
        ' Test record details for specified change type.
        ' If Updated is null, set to byte[0] (for inserted items).
        Dim details As IEnumerable(Of LTS.BookTrack) = (
                From d In source.BookTracks
                Where d.TrackingState = changeType
                Select New LTS.BookTrack With {.BookTrackControl = d.BookTrackControl _
                , .BookTrackBookControl = d.BookTrackBookControl _
                , .BookTrackDate = d.BookTrackDate _
                , .BookTrackContact = d.BookTrackContact _
                , .BookTrackComment = d.BookTrackComment _
                , .BookTrackStatus = d.BookTrackStatus _
                , .BookTrackCommentLocalized = d.BookTrackCommentLocalized _
                , .BookTrackCommentKeys = d.BookTrackCommentKeys _
                , .BookTrackModUser = Parameters.UserName _
                , .BookTrackModDate = Date.Now _
                , .BookTrackUpdated = If(d.BookTrackUpdated Is Nothing, New Byte() {}, d.BookTrackUpdated)})
        Return details.ToList()
    End Function

    Private Function getNewBookCodeValues(ByRef intCodeVal1 As Integer, ByRef intCodeVal2 As Integer) As Boolean
        Dim blnRet As Boolean = False
        Try
            intCodeVal1 = getScalarInteger("Select dbo.getLastBookCode1() as RetVal")
            intCodeVal2 = getScalarInteger("Select dbo.getLastBookCode2() as RetVal")
            intCodeVal2 = intCodeVal2 + 1
            If intCodeVal2 >= 255 Then
                intCodeVal2 = 33
                intCodeVal1 = intCodeVal1 + 1
                If intCodeVal1 = 160 Then intCodeVal1 = 161
                If intCodeVal1 = 173 Then intCodeVal1 = 174
            End If
            If intCodeVal1 > 255 Then
                Utilities.SaveAppError("E_MaxNbrOfBooks", Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_MaxNbrOfBooks"}, New FaultReason("E_CreateRecordFailure"))
            End If
            If intCodeVal2 < 40 Then intCodeVal2 = 40
            If intCodeVal2 = 45 Then intCodeVal2 = 46
            If intCodeVal2 > 96 And intCodeVal2 < 123 Then intCodeVal2 = 124
            If intCodeVal2 = 126 Then intCodeVal2 = 128
            If intCodeVal2 = 127 Then intCodeVal2 = 128
            If intCodeVal2 = 160 Then intCodeVal2 = 161
            If intCodeVal1 = 173 Then intCodeVal1 = 174
            blnRet = True
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
        End Try
        Return blnRet
    End Function

    Private Function GetCustAbrev(ByVal Control As Integer,
                                  Optional ByVal UseCompNumber As Boolean = False) As String

        Dim strSQL As String = "Select dbo.comp.compabrev as RetVal " _
                               & " From dbo.comp "
        If UseCompNumber Then
            strSQL &= " Where dbo.comp.compnumber = " & Control
        Else
            strSQL &= " Where dbo.comp.compcontrol = " & Control
        End If
        Return getScalarString(strSQL)

    End Function

    Public Function GetBookAddr(ByVal compNumber As Integer, ByVal OrderNumber As String, ByVal OrderSequenceNo As Integer) As DataTransferObjects.BookAddr
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'first select the comp control
                Dim compDB As New NGLMASCompDataContext(ConnectionString)
                Dim compControl As Integer = 0
                Dim oitemComp As DataTransferObjects.Comp = (
                        From d In compDB.Comps Where d.CompNumber = compNumber
                        Select New DataTransferObjects.Comp With {.CompControl = d.CompControl}).Single

                'now select the address data.

                'Get the data
                Dim oitem As DataTransferObjects.BookAddr = (
                        From d In db.Books Where d.BookCustCompControl = oitemComp.CompControl And d.BookCarrOrderNumber = OrderNumber And d.BookOrderSequence = OrderSequenceNo
                        Select New DataTransferObjects.BookAddr With {.BookControl = d.BookControl,
                        .BookCustCompControl = d.BookCustCompControl,
                        .BookDestAddress1 = d.BookDestAddress1,
                        .BookDestAddress2 = d.BookDestAddress2,
                        .BookDestAddress3 = d.BookDestAddress3,
                        .BookDestCity = d.BookDestCity,
                        .BookDestCompControl = If(d.BookDestCompControl.HasValue, d.BookDestCompControl.Value, 0),
                        .BookDestCountry = d.BookDestCountry,
                        .BookDestName = d.BookDestName,
                        .BookDestState = d.BookDestState,
                        .BookDestZip = d.BookDestZip,
                        .BookODControl = d.BookODControl,
                        .BookOrigAddress1 = d.BookOrigAddress1,
                        .BookOrigAddress2 = d.BookOrigAddress2,
                        .BookOrigAddress3 = d.BookOrigAddress3,
                        .BookOrigCity = d.BookOrigCity,
                        .BookOrigCompControl = If(d.BookOrigCompControl.HasValue, d.BookOrigCompControl.Value, 0),
                        .BookOrigCountry = d.BookOrigCountry,
                        .BookOrigName = d.BookOrigName,
                        .BookOrigState = d.BookOrigState,
                        .BookOrigZip = d.BookOrigZip}).Single

                Return oitem

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                ' Utilities.SaveAppError(ex.Message, Me.Parameters)
                'Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Friend Shared Function SelectDTOData(ByVal d As LTS.Book, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.Book
        Dim oDTO As New DataTransferObjects.Book
        Dim skipObjs As New List(Of String) From {"BookUpdated", "Page", "Pages", "RecordCount", "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .BookUpdated = d.BookUpdated.ToArray()
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oDTO

    End Function

    Friend Shared Function selectDTODataWDetails(ByVal d As LTS.Book, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.Book
        Dim oDTO As DataTransferObjects.Book = SelectDTOData(d, page, pagecount, recordcount, pagesize)
        With oDTO
            .BookLoads = (From bl In d.BookLoads Select NGLBookLoadData.selectDTODataWDetails(bl)).ToList()
            .BookNotes = (From bn In d.BookNotes Select NGLBookNoteData.selectDTOData(bn)).ToList()
            .BookTracks = (From bt In d.BookTracks Select NGLBookTrackData.selectDTOData(bt)).ToList()
        End With
        Return oDTO
    End Function

    Friend Function selectDTODataWBookTracks(ByVal d As LTS.Book, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.Book
        Dim oDTO As DataTransferObjects.Book = SelectDTOData(d, page, pagecount, recordcount, pagesize)
        oDTO.BookTracks = (From bt In d.BookTracks Select NGLBookTrackData.selectDTOData(bt)).ToList()
        Return oDTO
    End Function

    Friend Shared Function selectDTODataWBookLoads(ByVal d As LTS.Book, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.Book
        Dim oDTO As DataTransferObjects.Book = SelectDTOData(d, page, pagecount, recordcount, pagesize)
        oDTO.BookLoads = (From bl In d.BookLoads Select NGLBookLoadData.selectDTOData(bl, Nothing)).ToList()
        Return oDTO
    End Function

    ''' <summary>
    ''' Caller must ecapsulate LinqDB in Using Statement
    ''' </summary>
    ''' <typeparam name="TEntity"></typeparam>
    ''' <param name="oData"></param>
    ''' <param name="oLinqTable"></param>
    ''' <remarks></remarks>
    Private Sub SaveLinqData(Of TEntity As Class)(ByVal oData As Object,
                                                  ByVal oLinqTable As System.Data.Linq.Table(Of TEntity))
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
        Dim source As LTS.Book = TryCast(nObject, LTS.Book)
        If source Is Nothing Then Return
        UpdateBookDependencies(source.BookControl, 0)
    End Sub
#End Region


#Region " SettlementSave From BLL "


#Region "Process Freight Bill Methods"

    ''' <summary>
    ''' Inserts a new Freight Bill history record if one does not exist ore updates the existing history record
    ''' when a change has been made
    ''' </summary>
    ''' <param name="s"></param>
    ''' <param name="blnElectronicFlag"></param>
    ''' <param name="sRetMsg"></param>
    ''' <param name="oResults"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.2.1.004 on 01/02/2020
    '''     added logic to only save billed fees,  non-billed (expected only) or missing fees are not recorded in the history table logs
    ''' Moified by RHR for v-8.5.2.006 on 02/23/2023 added Models.SettlementSave.APBillDate value to APMHBillDate
    '''     if APBillDate is nothing use current date
    ''' </remarks>
    Public Function InsertFreightBillHistory(ByRef s As Models.SettlementSave, ByVal blnElectronicFlag As Boolean, ByRef sRetMsg As String, ByRef oResults As Models.ResultObject) As Boolean
        Logger.Information($"NGLBookData.InsertFreightBillHistory -- ENTRY POINT -- SettlementSave: {JsonSerializer.Serialize(s)} -- blnElectronicFlag: {blnElectronicFlag} -- sRetMsg: {sRetMsg} -- oResults: {JsonSerializer.Serialize(oResults)}")

        ' Log parameter null checks
        Logger.Information($"PARAM CHECK: s Is Nothing: {s Is Nothing}")
        Logger.Information($"PARAM CHECK: oResults Is Nothing: {oResults Is Nothing}")

        Dim blnRet As Boolean = False
        Dim sHistRecord As String = ""

        ' Log initial state
        Logger.Information($"INITIAL STATE: blnRet: {blnRet}, sHistRecord: '{sHistRecord}'")

        If s Is Nothing Then
            Logger.Error("CRITICAL ERROR: s parameter is null")
            Return False
        End If

        If String.IsNullOrWhiteSpace(s.InvoiceNo) Then
            Logger.Error("VALIDATION ERROR: s.InvoiceNo is null or empty")
            'we cannot continue return an exception
            throwFieldRequiredException("Freight Bill Invoice Number")
        End If

        Try
            Dim dtNow As DateTime = Date.Now()
            Logger.Information($"PROCESSING: dtNow: {dtNow}")

            ' Log s properties before any modifications
            Logger.Information($"PROPERTY CHECK: s.CarrierNumber: {s.CarrierNumber}, s.CarrierControl: {s.CarrierControl}")
            Logger.Information($"PROPERTY CHECK: s.BookControl: {s.BookControl}, s.BookSHID: '{s.BookSHID}'")
            Logger.Information($"PROPERTY CHECK: s.APCustomerID: '{s.APCustomerID}'")
            Logger.Information($"PROPERTY CHECK: s.InvoiceAmt: {s.InvoiceAmt}, s.LineHaul: {s.LineHaul}, s.TotalFuel: {s.TotalFuel}")
            Logger.Information($"PROPERTY CHECK: s.APBillDate: {If(s.APBillDate.HasValue, s.APBillDate.Value.ToString(), "NULL")}")
            Logger.Information($"PROPERTY CHECK: s.BookFinAPActWgt: {s.BookFinAPActWgt}, s.BookCarrBLNumber: '{s.BookCarrBLNumber}'")
            Logger.Information($"PROPERTY CHECK: s.Fees Is Nothing: {s.Fees Is Nothing}, s.Fees Count: {If(s.Fees Is Nothing, 0, s.Fees.Count())}")

            Logger.Information($"CALLING: NGLCarrierObjData.getCarrierNameNumber({s.CarrierControl})")

            If s.CarrierNumber = 0 AndAlso s.CarrierControl <> 0 Then
                Dim oCarrierData = NGLCarrierObjData.getCarrierNameNumber(s.CarrierControl)
                Logger.Information($"RESULT: oCarrierData Is Nothing: {oCarrierData Is Nothing}")

                If Not oCarrierData Is Nothing Then
                    Logger.Information($"RESULT: oCarrierData.ContainsKey('CarrierNumber'): {oCarrierData.ContainsKey("CarrierNumber")}")

                    If oCarrierData.ContainsKey("CarrierNumber") Then
                        Dim parseSuccess As Boolean = False
                        Dim tempCarrierNumber As Integer = 0
                        parseSuccess = Integer.TryParse(oCarrierData("CarrierNumber"), tempCarrierNumber)
                        Logger.Information($"PARSING: CarrierNumber parse success: {parseSuccess}, value: {tempCarrierNumber}")

                        If parseSuccess Then
                            s.CarrierNumber = tempCarrierNumber
                        End If
                    End If
                End If

                Logger.Information($"UPDATED: s.CarrierNumber: {s.CarrierNumber}")
            End If

            Logger.Information($"CALLING: GetFirstBookControlBySHID({s.BookSHID})")

            If s.BookControl = 0 AndAlso Not String.IsNullOrWhiteSpace(s.BookSHID) Then
                Dim tempBookControl = GetFirstBookControlBySHID(s.BookSHID)
                Logger.Information($"RESULT: GetFirstBookControlBySHID returned: {tempBookControl}")
                s.BookControl = tempBookControl
            End If

            Logger.Information($"CALLING: GetCompanyNameNumberByBookControl({s.BookControl})")

            If (String.IsNullOrWhiteSpace(s.APCustomerID) OrElse s.APCustomerID = "0") And s.BookControl <> 0 Then
                Dim oCompData = GetCompanyNameNumberByBookControl(s.BookControl)
                Logger.Information($"RESULT: oCompData Is Nothing: {oCompData Is Nothing}")

                If Not oCompData Is Nothing Then
                    Logger.Information($"RESULT: oCompData.ContainsKey('CompNumber'): {oCompData.ContainsKey("CompNumber")}")

                    If oCompData.ContainsKey("CompNumber") Then
                        s.APCustomerID = oCompData("CompNumber")
                        Logger.Information($"UPDATED: s.APCustomerID: '{s.APCustomerID}'")
                    End If
                End If
            End If

            ' Log values before dOtherCost calculation
            Logger.Information($"CALCULATION: About to calculate dOtherCost")
            Logger.Information($"CALCULATION: s.InvoiceAmt type: {s.InvoiceAmt.GetType().FullName}, value: {s.InvoiceAmt}")
            Logger.Information($"CALCULATION: s.LineHaul type: {s.LineHaul.GetType().FullName}, value: {s.LineHaul}")
            Logger.Information($"CALCULATION: s.TotalFuel type: {s.TotalFuel.GetType().FullName}, value: {s.TotalFuel}")

            ' Safely calculate dOtherCost with detailed logging
            Dim invoiceAmt As Decimal = 0
            Dim lineHaul As Decimal = 0
            Dim totalFuel As Decimal = 0

            Try
                invoiceAmt = If(s.InvoiceAmt <> 0, s.InvoiceAmt, 0)
                Logger.Information($"CALCULATION: invoiceAmt assigned: {invoiceAmt}")
            Catch ex As Exception
                Logger.Error(ex, "ERROR: Exception assigning invoiceAmt")
            End Try

            Try
                lineHaul = If(s.LineHaul <> 0, s.LineHaul, 0)
                Logger.Information($"CALCULATION: lineHaul assigned: {lineHaul}")
            Catch ex As Exception
                Logger.Error(ex, "ERROR: Exception assigning lineHaul")
            End Try

            Try
                totalFuel = If(s.TotalFuel <> 0, s.TotalFuel, 0)
                Logger.Information($"CALCULATION: totalFuel assigned: {totalFuel}")
            Catch ex As Exception
                Logger.Error(ex, "ERROR: Exception assigning totalFuel")
            End Try

            Dim dOtherCost As Decimal = 0
            Try
                dOtherCost = (invoiceAmt - lineHaul) - totalFuel
                Logger.Information($"CALCULATION: dOtherCost calculated: {dOtherCost}")
            Catch ex As Exception
                Logger.Error(ex, "ERROR: Exception calculating dOtherCost")
            End Try

            'Modified by RHR for v-8.5.2.006 on 02/23/2023
            Dim dtBillDate As Date = dtNow

            Try
                If s.APBillDate.HasValue Then
                    dtBillDate = s.APBillDate.Value
                    Logger.Information($"PROCESSING: dtBillDate set from s.APBillDate: {dtBillDate}")
                Else
                    Logger.Information($"PROCESSING: dtBillDate using default dtNow: {dtBillDate}")
                End If
            Catch ex As Exception
                Logger.Error(ex, "ERROR: Exception processing APBillDate")
            End Try

            Try
                sHistRecord = String.Format("BillDate = {0}, BilledWeight = {1}, BillNumber = {2}, CarrierCost = {3}, CarrierNumber = {4}, CustomerID = {5}, ElectronicFlag = {6}, ReceivedDate = {7}, SHID = {8}, TotalCost = {9}, BLNumber = {10}, OtherCosts = {11} ",
                                           dtBillDate, s.BookFinAPActWgt, s.InvoiceNo, s.LineHaul, s.CarrierNumber, s.APCustomerID, blnElectronicFlag, dtNow, s.BookSHID, s.InvoiceAmt, s.BookCarrBLNumber, dOtherCost)
                Logger.Information($"PROCESSING: sHistRecord created: '{sHistRecord}'")
            Catch ex As Exception
                Logger.Error(ex, "ERROR: Exception creating sHistRecord")
            End Try

            Try
                Dim oHistory As New LTS.APMassEntryHistory() With {
                    .APMHBillDate = dtBillDate,
                    .APMHBilledWeight = s.BookFinAPActWgt,
                    .APMHBillNumber = s.InvoiceNo,
                    .APMHCarrierCost = s.LineHaul,
                    .APMHCarrierNumber = s.CarrierNumber,
                    .APMHCustomerID = s.APCustomerID,
                    .APMHElectronicFlag = blnElectronicFlag,
                    .APMHReceivedDate = dtNow,
                    .APMHSHID = s.BookSHID,
                    .APMHTotalCost = s.InvoiceAmt,
                    .APMHBLNumber = s.BookCarrBLNumber,
                    .APMHOtherCosts = dOtherCost
                }

                Logger.Information($"OBJECT CREATED: oHistory created successfully")

                ' Log all properties of oHistory to help diagnose null reference issues
                Logger.Information($"OBJECT DETAILS: oHistory.APMHBillDate: {If(oHistory.APMHBillDate = Nothing, "NULL", oHistory.APMHBillDate.ToString())}")
                Logger.Information($"OBJECT DETAILS: oHistory.APMHBilledWeight: {oHistory.APMHBilledWeight}")
                Logger.Information($"OBJECT DETAILS: oHistory.APMHBillNumber: '{oHistory.APMHBillNumber}'")
                Logger.Information($"OBJECT DETAILS: oHistory.APMHCarrierCost: {oHistory.APMHCarrierCost}")
                Logger.Information($"OBJECT DETAILS: oHistory.APMHCarrierNumber: {oHistory.APMHCarrierNumber}")
                Logger.Information($"OBJECT DETAILS: oHistory.APMHCustomerID: '{oHistory.APMHCustomerID}'")
                Logger.Information($"OBJECT DETAILS: oHistory.APMHElectronicFlag: {oHistory.APMHElectronicFlag}")
                Logger.Information($"OBJECT DETAILS: oHistory.APMHReceivedDate: {If(oHistory.APMHReceivedDate = Nothing, "NULL", oHistory.APMHReceivedDate.ToString())}")
                Logger.Information($"OBJECT DETAILS: oHistory.APMHSHID: '{oHistory.APMHSHID}'")
                Logger.Information($"OBJECT DETAILS: oHistory.APMHTotalCost: {oHistory.APMHTotalCost}")
                Logger.Information($"OBJECT DETAILS: oHistory.APMHBLNumber: '{oHistory.APMHBLNumber}'")
                Logger.Information($"OBJECT DETAILS: oHistory.APMHOtherCosts: {oHistory.APMHOtherCosts}")

                ' Check if NGLAPMassEntryHistoriesData is null
                Logger.Information($"CHECKING: NGLAPMassEntryHistoriesData Is Nothing: {NGLAPMassEntryHistoriesData Is Nothing}")
                Logger.Information($"CALLING: NGLAPMassEntryHistoriesData.InsertOrUpdateAPMassEntryHistory")

                If NGLAPMassEntryHistoriesData.InsertOrUpdateAPMassEntryHistory(oHistory) Then
                    Logger.Information($"RESULT: InsertOrUpdateAPMassEntryHistory returned true")
                    Dim iAPMHControl = oHistory.APMHControl
                    Logger.Information($"PROCESSING: iAPMHControl: {iAPMHControl}")

                    Dim iProcessFeesErrors As Integer = 0

                    If iAPMHControl <> 0 AndAlso Not s.Fees Is Nothing AndAlso s.Fees.Count() > 0 Then
                        Logger.Information($"PROCESSING: Processing {s.Fees.Count()} fees")
                        Dim sFeeRecord As String = ""
                        'Modified by RHR for v-8.2.1.004 on 01/02/2020 added x.BilledFee = True filter

                        Try
                            Dim billedFees = s.Fees.Where(Function(x) x.BilledFee = True).ToList()
                            Logger.Information($"PROCESSING: Found {billedFees.Count} billed fees")

                            For i As Integer = 0 To billedFees.Count - 1
                                Dim f = billedFees(i)
                                Logger.Information($"PROCESSING: Processing fee #{i + 1}")
                                Logger.Information($"FEE CHECK: f Is Nothing: {f Is Nothing}")

                                If f Is Nothing Then
                                    Logger.Warning($"FEE WARNING: Fee #{i + 1} is null, skipping")
                                    Continue For
                                End If

                                ' Log fee properties
                                Logger.Information($"FEE PROPERTY: AccessorialCode: {f.AccessorialCode}")
                                Logger.Information($"FEE PROPERTY: BookControl: {f.BookControl}")
                                Logger.Information($"FEE PROPERTY: Caption: '{f.Caption}'")
                                Logger.Information($"FEE PROPERTY: Cost: {f.Cost}")
                                Logger.Information($"FEE PROPERTY: StopSequence: {f.StopSequence}")
                                Logger.Information($"FEE PROPERTY: BookCarrOrderNumber: '{f.BookCarrOrderNumber}'")

                                Try
                                    sFeeRecord = String.Format("AccessorialCode = {0}, APMHControl = {1}, BookControl = {2}, Caption = {3}, Cost = {4}, StopSequence = {5}, OrderNumber = {6}",
                                                              f.AccessorialCode, iAPMHControl, f.BookControl, f.Caption, f.Cost, f.StopSequence, f.BookCarrOrderNumber)
                                    Logger.Information($"FEE PROCESSING: sFeeRecord created: '{sFeeRecord}'")
                                Catch innerE As Exception
                                    Logger.Error(innerE, $"FEE ERROR: Exception creating sFeeRecord for fee #{i + 1}")
                                    'do nothing
                                End Try

                                If (f.BookControl = 0) Then
                                    Logger.Information($"FEE PROCESSING: f.BookControl is 0, setting to s.BookControl: {s.BookControl}")
                                    f.BookControl = s.BookControl
                                End If

                                Try
                                    Logger.Information($"FEE PROCESSING: Creating APMassEntryHistoryFee object")
                                    Dim oFee As New LTS.APMassEntryHistoryFee() With {
                                        .APMHFeesAccessorialCode = f.AccessorialCode,
                                        .APMHFeesAPMHControl = iAPMHControl,
                                        .APMHFeesBookControl = f.BookControl,
                                        .APMHFeesCaption = f.Caption,
                                        .APMHFeesValue = f.Cost,
                                        .APMHFeesStopSequence = f.StopSequence,
                                        .APMHFeesOrderNumber = f.BookCarrOrderNumber
                                    }

                                    Logger.Information($"CALLING: NGLAPMassEntryHistoryFeesData.InsertOrUpdateAPMassEntryHistoryFee")
                                    NGLAPMassEntryHistoryFeesData.InsertOrUpdateAPMassEntryHistoryFee(oFee)
                                    Logger.Information($"RESULT: InsertOrUpdateAPMassEntryHistoryFee completed successfully")
                                Catch ex As Exception
                                    iProcessFeesErrors += 1
                                    Logger.Error(ex, $"FEE ERROR: Exception processing fee #{i + 1}")
                                    'just log any save historical fees data in system error log
                                    logSystemError(ex, "NGLBookData.InsertFreightBillHistory(Fees)", sFeeRecord)
                                End Try
                            Next

                            Logger.Information($"PROCESSING: Completed processing all fees, error count: {iProcessFeesErrors}")
                        Catch feeEx As Exception
                            Logger.Error(feeEx, "ERROR: Exception in fee processing loop")
                        End Try

                        If iProcessFeesErrors > 0 Then
                            Logger.Warning($"PROCESSING: {iProcessFeesErrors} fee processing errors occurred")
                            Dim sDetail = String.Format(" Error Count {0} freight bill number {1}, ", iProcessFeesErrors, s.InvoiceNo)
                            Logger.Information($"CALLING: appendToResultMessage with detail: '{sDetail}'")

                            Try
                                sRetMsg = appendToResultMessage(oResults, sDetail, Models.ResultObject.ResultMsgType.Warning,
                                                              Utilities.ResultProcedures.freightbill,
                                                              Utilities.ResultTitles.TitleSaveHistLogFailure,
                                                              Utilities.ResultPrefix.MsgDetails,
                                                              Utilities.ResultSuffix.MsgDoesNotEffectProcess)
                                Logger.Information($"RESULT: appendToResultMessage returned: '{sRetMsg}'")
                            Catch msgEx As Exception
                                Logger.Error(msgEx, "ERROR: Exception in appendToResultMessage")
                            End Try
                        End If
                    Else
                        Logger.Information($"PROCESSING: Skipping fee processing - conditions not met: iAPMHControl <> 0: {iAPMHControl <> 0}, Not s.Fees Is Nothing: {Not s.Fees Is Nothing}, s.Fees.Count() > 0: {If(s.Fees Is Nothing, False, s.Fees.Count() > 0)}")
                    End If

                    blnRet = True
                    Logger.Information($"PROCESSING: Setting blnRet to true")
                Else
                    Logger.Warning("RESULT: InsertOrUpdateAPMassEntryHistory returned false")
                End If
            Catch historyEx As Exception
                Logger.Error(historyEx, "ERROR: Exception creating or processing oHistory")
            End Try
        Catch ex As Exception
            Logger.Error(ex, "CRITICAL ERROR: Exception in main try block of InsertFreightBillHistory")

            Try
                Dim sDetail = String.Format(" freight bill number {0}, ", s.InvoiceNo)
                Logger.Information($"EXCEPTION HANDLING: Creating error message with detail: '{sDetail}'")
                sRetMsg = appendToResultMessage(oResults, sDetail, Models.ResultObject.ResultMsgType.Err,
                                              Utilities.ResultProcedures.freightbill,
                                              Utilities.ResultTitles.TitleSaveHistLogFailure,
                                              Utilities.ResultPrefix.MsgDetails,
                                              Utilities.ResultSuffix.MsgDoesNotEffectProcess)
                Logger.Information($"EXCEPTION HANDLING: appendToResultMessage returned: '{sRetMsg}'")
            Catch msgEx As Exception
                Logger.Error(msgEx, "ERROR: Exception in exception handling")
            End Try

            'log any save historical fees data in system error log
            Try
                logSystemError(ex, "NGLBookData.InsertFreightBillHistory", sHistRecord)
                Logger.Information("EXCEPTION HANDLING: Called logSystemError")
            Catch logEx As Exception
                Logger.Error(logEx, "ERROR: Exception in logSystemError")
            End Try
        End Try

        Logger.Information($"EXIT POINT: InsertFreightBillHistory returning: {blnRet}")
        Return blnRet
    End Function

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
    Public Sub addAuditMessage(ByVal msg As String, ByRef sbRetMsgs As System.Text.StringBuilder, ByVal APControl As Integer, ByVal eReasonCode As NGLLookupDataProvider.FBLoadStatusCodes, Optional ByVal blnMarkAsResolved As Boolean = False, Optional ByVal sSource As String = "NGLBookData.addAuditMessage")
        Dim sRecord = msg
        Try
            If Not String.IsNullOrWhiteSpace(msg) Then
                If Not sbRetMsgs Is Nothing Then
                    sbRetMsgs.Append(msg)
                End If
                If APControl > 0 Then
                    Dim iLoadStatusControl = 0
                    If Not Utilities.tryGetLoadStatusControl(eReasonCode, iLoadStatusControl) Then iLoadStatusControl = Utilities.storeLoadStatusControl(eReasonCode, NGLLoadStatusCodeObjData.GetLoadStatusControl(eReasonCode, NGLLookupDataProvider.GetFBReasonCodeDesc(eReasonCode), NGLLookupDataProvider.LoadStatusCodeTypes.FreightBill))
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
            logSystemError(ex, "NGLBookData.addAuditMessage", sRecord)
        End Try
    End Sub

    ''' <summary>
    ''' Copies the current booking financial data into Book Revenue History and marks it as The Expected Cost
    ''' </summary>
    ''' <param name="s"></param>
    ''' <param name="sRetMsg"></param>
    ''' <param name="oResults"></param>
    ''' <param name="blnContinueOnFaultException"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2.0.117 on 8/9/19
    '''         wrapper to CreateBookRevenueHistory
    '''         the optional blnContinueOnFaultException parameter allows a true response 
    '''         even on a Fault exception but returns a message instead.
    '''         This allows the caller to continue processing other tasks.  
    '''         Unexpected Exceptions are still re=thrown to the caller.
    '''         if blnContinueOnFaultException is false all exceptions are thrown back to the caller
    ''' </remarks>
    Public Function createExpectedCost(ByRef s As Models.SettlementSave, ByRef sRetMsg As String, ByRef oResults As Models.ResultObject, Optional ByVal blnContinueOnFaultException As Boolean = False) As Boolean
        Try
            Return NGLBookRevObjData.CreateBookRevenueHistory(s.BookControl, True)

        Catch ex As FaultException
            If blnContinueOnFaultException Then
                Dim sDetail = String.Format(" freight bill number {0}. ", s.InvoiceNo)
                sRetMsg = appendToResultMessage(oResults, sDetail, Models.ResultObject.ResultMsgType.Err, Utilities.ResultProcedures.freightbill, Utilities.ResultTitles.TitleSaveExpectedCost, Utilities.ResultPrefix.MsgCostComparisonNotAvailable, Utilities.ResultSuffix.MsgCheckAppErrorLogs)
                'log any save historical fees data in system error log
                Utilities.SaveAppError(String.Format("Unexpected Error in NGLTMS365BLL.createExpectedCost for freight bill {0}. Message: {1}  ", s.InvoiceNo, ex.Message), Me.Parameters)
                Return True
            Else
                Throw
            End If
        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Function createOrUpdateAPMassEntry(ByRef s As Models.SettlementSave, ByVal ElectronicFlag As Boolean, ByRef sRetMsg As String, ByRef oResults As Models.ResultObject) As Boolean
        Dim dalAPME As New NGLAPMassEntryData(Parameters)
        'Re-write InsertFreightBillWeb365 logic to only create the record and not run the audit.
        Dim r = dalAPME.InsertFreightBillWeb365(s.BookControl, s.InvoiceNo, s.APBillDate, s.APReceivedDate, s.InvoiceAmt, s.LineHaul, s.CarrierControl, s.BookFinAPActWgt, s.BookCarrBLNumber, ElectronicFlag)
        If (Not r Is Nothing) Then
            If (r.ErrNumber <> 0) Then
                sRetMsg = appendToResultMessage(oResults, r.RetMsg, Models.ResultObject.ResultMsgType.Warning, Utilities.ResultProcedures.freightbill, Utilities.ResultTitles.TitleSaveHistLogFailure, Utilities.ResultPrefix.MsgDetails, Utilities.ResultSuffix.None)
                oResults.Success = False
                Return False
            Else
                'update the settlementsave data
                s.BookSHID = r.BookSHID
                s.InvoiceNo = r.APBillNumber
                s.APControl = r.APControl
                Return True
            End If

        End If
        Return True
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="s"></param>
    ''' <param name="sRetMsg"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.2.1.004 on 01/10/2020
    '''   added logic to properly assign missing or non-billed fuel
    ''' Modified by RHR for v-8.2.1.006 on 03/19/2020
    '''     added logic to support custom Fee Allocation Types for each load
    ''' </remarks>
    Public Function processBilledFees(ByRef s As Models.SettlementSave, ByRef sRetMsg As String) As Boolean
        Logger.Information("NGLBookData.processBilledFees: SettlementSave: " & JsonSerializer.Serialize(s).ToString())

        'steps for each fee
        Dim oStopFees As New List(Of Models.SettlementFee)
        Dim oBookFees As New List(Of Models.SettlementFee)
        ' Dim dTotalFuel As Decimal = s.TotalFuel
        Dim sFuelFeeCaption As String = ""
        Dim sFuelFeeEDICode As String = ""
        Dim blnFuelFeeMissing As Boolean = False
        Dim sMsg As String = ""
        Dim iErrNumber As Integer
        Dim blnAddBilledFuel As Boolean = False 'Modified by RHR for v-8.2.1.004 on 01/10/2020
        'if one fails we log the issue and continue to the next fee
        '1. Identify if each fee is by stop number or order number
        '       loop through each fee and merge into one fee for each stop or order number as needed.
        '2. Lookup correct EDI Code and map to correct accessorial code this is done by the callers
        '3. Determine if we have Fuel as one of the accessorial fees (typically from EDI)
        '   process TotalFuel from SettlementSave
        '   Append any fuel fees to TotalFuel
        'Note:  
        '       4. 5. and 6. must work together becaue some functionality cannot be completed until all 
        '       fees have been processed atlease once.
        '       Step 1. Determine total cost by stop using allocation rules.
        '       Step 2. Save to APMassEntryFee for each bookcontrol
        '       Step 3. Update Pending Fees
        '       Step 4. Log and process any alerts or issues
        '4. If any fee total does not match total for order create a new pending fee by order.
        '       If order number is empty and stop number is zero this fee allcoates to the entire load.
        '           so we need to merge any fees with blank order numbers and stop number zero into one fee record
        '       If order number is provided we ignore the stop number logic and we ignore the allocation rules.
        '       If total fuel for the load is different than SettlementSave.TotalFuel we create a new pending fee as Spot Rate Fuel
        '           this is allocated accross all stops with Spot Rate Fuel Allocation rules in Accessorial Table (later we may make this configurable by each LE)
        '       Follow all other fee rules
        '5. Update the APMassEntryFee table with the correct allocated amount by bookcontrol
        '       Once all order numbers and stop numbers are process we need to determine if any expected fees are 
        '           are missing,  We use the APMassEntryFee table to compare costs. Costs must match or a pending
        '           must be waiting on approval with a matching cost.  Zeros are allowd in APMassEntryFee this indicates 
        '           that the fee is to be waved or removed.
        '6. Log AP Fee Messages and Alerts (typically return value from 5.)  5. will update Fee Table flags
        '       Special Issues and Alerts:
        '           Alert when an expected fee is missing in the bill
        '           Alert when a non-zero billed fee is not listed as expected 
        '           Alert when a billed fee is not configured for the L.E. Carrier
        If Not s.Fees Is Nothing AndAlso s.Fees.Count() > 0 Then
            'Modified by RHR for v-8.2.1.004 on 01/10/2020
            If s.Fees.Any(Function(y) (y.AccessorialCode = 2 Or y.AccessorialCode = 9 Or y.AccessorialCode = 15) And y.BilledFee = True) Then
                blnAddBilledFuel = True
            End If
            For Each sFee In s.Fees
                If String.IsNullOrWhiteSpace(sFee.BookCarrOrderNumber) Then
                    If sFee.BookControl <> 0 Then
                        Dim strSQL As String = "Select top 1 BookCarrOrderNumber From dbo.Book Where BookControl = " & sFee.BookControl.ToString()
                        Dim sON As String = getScalarString(strSQL)
                        sFee.BookCarrOrderNumber = sON
                    End If
                End If
                If blnAddBilledFuel And (sFee.AccessorialCode = 2 Or sFee.AccessorialCode = 9 Or sFee.AccessorialCode = 15) Then
                    ' when fuel is billed it is expected to be one cost for the load
                    ' so we convert all billed fees to one charge AccessorialCode 15 
                    ' and override any existing fees
                    ' but when billed is false the fuel is allocated by order number so
                    ' we process the fule like any other fee.
                    'For step 3 we combine the total fuel
                    s.TotalFuel += sFee.Cost
                    sFuelFeeCaption = sFee.Caption
                    sFuelFeeEDICode = sFee.EDICode
                    'if any fuel fee is flagged as missing all are missing
                    If sFee.MissingFee And blnFuelFeeMissing = False Then blnFuelFeeMissing = True
                Else
                    Dim blnMatchFound As Boolean = False

                    If String.IsNullOrWhiteSpace(sFee.BookCarrOrderNumber) Then
                        For Each sf In oStopFees
                            If sf.StopSequence = sFee.StopSequence And sf.AccessorialCode = sFee.AccessorialCode Then
                                sf.Cost += sFee.Cost 'add the fees together for this stop
                                blnMatchFound = True
                                Exit For
                            End If
                        Next
                        If Not blnMatchFound Then
                            oStopFees.Add(sFee)
                        End If
                    Else
                        For Each sf In oStopFees
                            If sf.BookCarrOrderNumber = sFee.BookCarrOrderNumber And sf.AccessorialCode = sFee.AccessorialCode Then
                                sf.Cost += sFee.Cost 'in the case where the fee is listed twice for the same order we add them together
                                blnMatchFound = True
                                Exit For
                            End If
                        Next
                        If Not blnMatchFound Then
                            oStopFees.Add(sFee)
                        End If
                    End If
                End If

            Next
            'now that we have the stop specific fees we need to get the booking control numbers and allocate for each fee that does not have an order number
            For Each sf In oStopFees.Where(Function(x) String.IsNullOrWhiteSpace(x.BookCarrOrderNumber) = True)
                Dim oAllocatedBookFees = GetBookControlForStopFee(s.BookSHID, sf.StopSequence, sf.Cost, sf.AccessorialCode)
                If Not oAllocatedBookFees Is Nothing AndAlso oAllocatedBookFees.Count() > 0 Then
                    ' Modified by RHR for v-8.2.1.006 on 03/19/2020 added logic to support custom Fee Allocation Types for each load
                    For Each af In oAllocatedBookFees
                        oBookFees.Add(New Models.SettlementFee() With {
                                         .BookControl = af.BookControl,
                                         .Cost = af.AllocatedCost,
                                         .BookCarrOrderNumber = af.BookCarrOrderNumber,
                                         .AccessorialCode = sf.AccessorialCode,
                                         .AllowCarrierUpdates = sf.AllowCarrierUpdates,
                                         .AutoApprove = sf.AutoApprove,
                                         .BFPControl = sf.BFPControl,
                                         .Caption = sf.Caption,
                                         .Control = sf.Control,
                                         .FeeIndex = sf.FeeIndex,
                                         .Minimum = sf.Minimum,
                                         .Msg = sf.Msg,
                                         .Pending = sf.Pending,
                                         .StopSequence = sf.StopSequence,
                                         .BilledFee = sf.BilledFee,
                                         .MissingFee = sf.MissingFee,
                                         .EDICode = sf.EDICode,
                                         .FeeAllocationTypeControl = sf.FeeAllocationTypeControl,
                                         .FeeAllocationTypeDesc = sf.FeeAllocationTypeDesc,
                                         .FeeAllocationTypeName = sf.FeeAllocationTypeName})
                    Next
                End If
            Next

            For Each sf In oStopFees.Where(Function(x) String.IsNullOrWhiteSpace(x.BookCarrOrderNumber) = False)
                If sf.BookControl = 0 Then sf.BookControl = GetBookControlByOrderNumber(sf.BookCarrOrderNumber)
                ' Modified by RHR for v-8.2.1.006 on 03/19/2020 added logic to support custom Fee Allocation Types for each load
                oBookFees.Add(New Models.SettlementFee() With {
                                 .BookControl = sf.BookControl,
                                 .Cost = sf.Cost,
                                 .BookCarrOrderNumber = sf.BookCarrOrderNumber,
                                 .AccessorialCode = sf.AccessorialCode,
                                 .AllowCarrierUpdates = sf.AllowCarrierUpdates,
                                 .AutoApprove = sf.AutoApprove,
                                 .BFPControl = sf.BFPControl,
                                 .Caption = sf.Caption,
                                 .Control = sf.Control,
                                 .FeeIndex = sf.FeeIndex,
                                 .Minimum = sf.Minimum,
                                 .Msg = sf.Msg,
                                 .Pending = sf.Pending,
                                 .StopSequence = sf.StopSequence,
                                 .BilledFee = sf.BilledFee,
                                 .MissingFee = sf.MissingFee,
                                 .EDICode = sf.EDICode,
                                 .FeeAllocationTypeControl = sf.FeeAllocationTypeControl,
                                 .FeeAllocationTypeDesc = sf.FeeAllocationTypeDesc,
                                 .FeeAllocationTypeName = sf.FeeAllocationTypeName})

            Next
            'Now we consolidate fees that need to be allocated by Load, Origin, or Destination into one stop fee
            Dim oLoadSpecificFees = New List(Of Models.SettlementFee)
            Dim oLoadOrigFees = New List(Of Models.SettlementFee)
            Dim oLoadDestFees = New List(Of Models.SettlementFee)
            Dim oOrderFees = New List(Of Models.SettlementFee)
            'get all of the  accessorials
            Dim oAccessorials = NGLtblAccessorialObjData.GetAllAccessorials()
            'get the booking records so we can check the orig and dest address info
            Dim oBooks = GetBooksBySHID(s.BookSHID)
            For Each bf In oBookFees
                ' Modified by RHR for v-8.2.1.006 on 03/19/2020 added logic to support custom Fee Allocation Types for each load
                Dim iAllocationType = bf.FeeAllocationTypeControl
                If iAllocationType < 1 Or iAllocationType > 4 Then 'if the Allocation Type is not supported use default
                    iAllocationType = oAccessorials.Where(Function(a) a.AccessorialCode = bf.AccessorialCode).Select(Function(i) i.AccessorialAccessorialFeeAllocationTypeControl).FirstOrDefault()
                End If
                '1   None
                '2   Origin
                '3   Destination
                '4   Load
                Select Case iAllocationType
                    Case 4   'Load
                        If oLoadSpecificFees.Any(Function(o) o.AccessorialCode = bf.AccessorialCode) Then
                            Dim item = oLoadSpecificFees.Where(Function(o) o.AccessorialCode = bf.AccessorialCode).FirstOrDefault()
                            item.Cost += bf.Cost
                        Else
                            oLoadSpecificFees.Add(bf)
                        End If
                    Case 2 'Origin
                        If Not oBooks Is Nothing AndAlso oBooks.Any(Function(b) b.BookControl = bf.BookControl) Then
                            If oLoadOrigFees.Any(Function(o) o.AccessorialCode = bf.AccessorialCode) Then
                                Dim items = oLoadOrigFees.Where(Function(o) o.AccessorialCode = bf.AccessorialCode).ToArray()
                                'for each i in items we check the orig informaiton
                                If Not items Is Nothing AndAlso items.Count() > 0 Then
                                    Dim blnOrigFound As Boolean = False
                                    For Each i In items
                                        'get the book data
                                        Dim iBook = oBooks.Where(Function(b) b.BookControl = i.BookControl).FirstOrDefault()
                                        Dim tBook = oBooks.Where(Function(b) b.BookControl = bf.BookControl).FirstOrDefault()
                                        If Not iBook Is Nothing AndAlso Not tBook Is Nothing Then
                                            If (iBook.BookOrigAddress1 = tBook.BookOrigAddress1 _
                                                And iBook.BookOrigCity = tBook.BookOrigCity _
                                                And iBook.BookOrigState = tBook.BookOrigState _
                                                And iBook.BookOrigZip = tBook.BookOrigZip) Then
                                                i.Cost += bf.Cost
                                                blnOrigFound = True
                                                Exit For
                                            End If
                                        End If
                                    Next
                                    If Not blnOrigFound Then
                                        'just add this to the orig fees it has a new address
                                        oLoadOrigFees.Add(bf)
                                    End If
                                Else
                                    'just add this to the orig fees
                                    oLoadOrigFees.Add(bf)
                                End If

                            Else
                                oLoadOrigFees.Add(bf)
                            End If
                        Else
                            'just add this fee as an order specific fee, this should not happen but we dont want to miss any fees
                            oOrderFees.Add(bf)
                        End If

                    Case 3 'Destination
                        If Not oBooks Is Nothing AndAlso oBooks.Any(Function(b) b.BookControl = bf.BookControl) Then
                            If oLoadDestFees.Any(Function(o) o.AccessorialCode = bf.AccessorialCode) Then
                                Dim items = oLoadDestFees.Where(Function(o) o.AccessorialCode = bf.AccessorialCode).ToArray()
                                'for each i in items we check the orig informaiton
                                If Not items Is Nothing AndAlso items.Count() > 0 Then
                                    Dim blnDestFound As Boolean = False
                                    For Each i In items
                                        'get the book data
                                        Dim iBook = oBooks.Where(Function(b) b.BookControl = i.BookControl).FirstOrDefault()
                                        Dim tBook = oBooks.Where(Function(b) b.BookControl = bf.BookControl).FirstOrDefault()
                                        If Not iBook Is Nothing AndAlso Not tBook Is Nothing Then
                                            If (iBook.BookDestAddress1 = tBook.BookDestAddress1 _
                                                And iBook.BookDestCity = tBook.BookDestCity _
                                                And iBook.BookDestState = tBook.BookDestState _
                                                And iBook.BookDestZip = tBook.BookDestZip) Then
                                                i.Cost += bf.Cost
                                                blnDestFound = True
                                                Exit For
                                            End If
                                        End If
                                    Next
                                    If Not blnDestFound Then
                                        'just add this to the orig fees it has a new address
                                        oLoadDestFees.Add(bf)
                                    End If
                                Else
                                    'just add this to the orig fees
                                    oLoadDestFees.Add(bf)
                                End If

                            Else
                                oLoadDestFees.Add(bf)
                            End If
                        Else
                            'just add this fee as an order specific fee, this should not happen but we dont want to miss any fees
                            oOrderFees.Add(bf)
                        End If

                    Case Else
                        oOrderFees.Add(bf)
                End Select
            Next
            'add all the fees together
            oStopFees = New List(Of Models.SettlementFee)
            oStopFees.AddRange(oOrderFees)
            oStopFees.AddRange(oLoadOrigFees)
            oStopFees.AddRange(oLoadDestFees)
            oStopFees.AddRange(oLoadSpecificFees)
            'Now oBookFees should have all the correct data and allocated costs so replae the original fee array
            s.Fees = oStopFees.ToArray()
            'If If(s.APControl, 0) > 0 Then ' we must have an apcontrol to process fuel,  this should always pass if the caller does it's job
            '    NGLAPMassEntryFeesData.UpdateAPMassEntryFees(s.APControl, s.Fees)
            'End If
            'Note: this code may need to be called after we process fuel and if we updae s.Fees with the fuel fees returned
            '      must test with multilple stops 

            Logger.Information("NGLBookData.processBilledFees: Fees: " & JsonSerializer.Serialize(s.Fees).ToString())

            If Not s.Fees Is Nothing AndAlso s.Fees.Count() > 0 Then
                For Each sFee In s.Fees
                    Try
                        ' SaveSettlementFees will insert or update the pending fees and the APMassEntryFeesTable 
                        Dim spRes As LTS.spSaveSettlementFeesResult = NGLBookFeePendingObjData.SaveSettlementFees(sFee, s.CompControl, s.CarrierControl)
                        '8. Update the Fees object with the results
                        sFee.BFPControl = If(spRes.BFPControl, 0)
                        sFee.AutoApprove = If(spRes.BFPAutoApprove, False)
                        If If(spRes.ErrNumber, 0) <> 0 Then
                            'we have an error
                            sRetMsg &= " Process Billed Fee Error; " & sFee.Caption & " failed: " & spRes.RetMsg & " | "
                        End If
                    Catch ex As Exception
                        'we may need to add special exceptions to handle fault exceptions
                        'for now just append to the sRetMsg; also add logic to determine if we should return false?
                        sRetMsg &= " Process Billed Fee Error; " & sFee.Caption & " failed: " & ex.Message & " | "
                    End Try
                Next
            End If

        End If
        ' Modified by RHR for v-8.2.1.004 on 01/10/2020
        '  added logic to check for Fuel From Settlement page or other processes where
        '  fuel is not added as an accessorial fee but as a flat charge.
        '  Rules: 
        '   1. fuel must not exist in fees
        '   2. TotalFuel must be greater than zero
        If blnAddBilledFuel = False AndAlso Not s.Fees.Any(Function(y) (y.AccessorialCode = 2 Or y.AccessorialCode = 9 Or y.AccessorialCode = 15)) AndAlso s.TotalFuel > 0 Then
            blnAddBilledFuel = True
        End If
        ' Modified by RHR for v-8.2.1.004 on 01/10/2020 we do not need to call UpdateFreightBillFuelCosts if not adding billed fuel
        If blnAddBilledFuel = True And (If(s.APControl, 0) > 0) Then ' we must have an apcontrol to process fuel,  this should always pass if the caller does it's job
            'process the fuel charges
            If s.TotalFuel = 0 Then
                s.TotalFuel = s.Fees.Where(Function(x) x.EDICode = "FUE").Sum(Function(x) x.Cost)
            End If
            Dim oFuelFees As Models.SettlementFee() = NGLAPMassEntryFeesData.UpdateFreightBillFuelCosts(s.APControl, s.BookSHID, s.TotalFuel, sFuelFeeCaption, sFuelFeeEDICode, blnFuelFeeMissing, sMsg, iErrNumber)
            If iErrNumber <> 0 And Not String.IsNullOrWhiteSpace(sMsg) Then
                sRetMsg &= " Process Billed Fuel Error: " & sMsg & " | "
            End If
            If Not oFuelFees Is Nothing AndAlso oFuelFees.Count() > 0 Then
                Dim lFees As New List(Of Models.SettlementFee)
                lFees = s.Fees.ToList()
                lFees.AddRange(oFuelFees)
                s.Fees = lFees.ToArray()
                's.Fees.append(oFuelFees)
            End If
        End If
        ' Modified by RHR for v-8.2.1.004 on 01/10/2020
        ' Once all the fees have been processed/added to the AP Mass Entry Fees 
        'we can update the AP Carrier Cost if it Is Not provided by the carrier
        'NGLAPMassEntryObjData.UpdateZeroCarrierCostUsingTotalFees(If(s.APControl, 0))
        Return True
    End Function

    ''' <summary>
    ''' Auto approve pending fees that meet requirements
    ''' </summary>
    ''' <param name="s"></param>
    ''' <param name="sRetMsg"></param>
    ''' <param name="oResults"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Moved from NGLTMS365BLL to DAL for v-8.2.1.004 on 12/26/2019
    '''     so code can be called from existing DAL methods
    '''     The Caller must recalculate costs immediately after calling this method
    '''     References to BLL.NGLBookRevenueBLL have been removed as we cannot call 
    '''     these methods from the DAL directely (circular reference)
    ''' </remarks>
    Public Function runAutoApprovePendingFees(ByVal s As Models.SettlementSave, ByRef sRetMsg As String, ByRef oResults As Models.ResultObject) As Boolean

        ' all updates to fees must have been processed so we need to read the data from the table
        ' this will include any pending fuel costs
        Try
            Dim iBookControls As New List(Of Integer)
            If Not s.Fees Is Nothing AndAlso s.Fees.Count() > 0 Then
                iBookControls = s.Fees.Select(Function(x) x.BookControl).Distinct().ToList()
            End If
            If Not iBookControls.Contains(s.BookControl) Then iBookControls.Add(s.BookControl) 'be sure we have the primary book control
            'now process all the fees for each book control
            For Each iBookControl In iBookControls
                Dim oBFPs = NGLBookFeePendingObjData.GetBookFeePendingsFiltered(iBookControl)
                If Not oBFPs Is Nothing AndAlso oBFPs.Count() > 0 Then
                    'steps for each fee
                    For Each sFee In oBFPs
                        Try
                            'if one fails we log the issue and continue to the next fee
                            '1. Can Fee Be Auto Approved
                            If sFee.BookFeesPendingControl <> 0 Then
                                Dim blnWithinTolerance = NGLBookFeePendingObjData.CanFeeBeAutoApproved(sFee.BookFeesPendingBookControl, sFee.BookFeesPendingAccessorialCode, sFee.BookFeesPendingValue, sFee.BookFeesPendingControl)
                                If (blnWithinTolerance) Then
                                    '2. If true then fee Is AutoApproved - update BFP as approved = true And user = "AutoApprove" And also create record in BF table
                                    Dim spApproveBFPRes As LTS.spAcceptPendingBookFeeResult = NGLBookFeePendingObjData.ApproveBookFeePending(sFee.BookFeesPendingControl, "AutoApprove")
                                    '3. process the approval messages
                                    If (Not spApproveBFPRes Is Nothing) Then
                                        If ((If(spApproveBFPRes.ErrNumber, 0) <> 0) OrElse (If(spApproveBFPRes.BookFeesControl, 0) = 0)) Then
                                            Dim sDetails = spApproveBFPRes.RetMsg & " For " & sFee.BookFeesPendingCaption
                                            sRetMsg &= appendToResultMessage(oResults, sDetails, Models.ResultObject.ResultMsgType.Warning, Utilities.ResultProcedures.freightbill, Utilities.ResultTitles.TitlePendingFeeApprovalWarning, Utilities.ResultPrefix.MsgDetails, Utilities.ResultSuffix.MsgDoesNotEffectProcess)
                                            Continue For
                                        End If
                                    Else
                                        'spApproveBFPRes is nothing so the procedure failed
                                        Dim sDetails = sFee.BookFeesPendingCaption & ". "
                                        sRetMsg &= appendToResultMessage(oResults, sDetails, Models.ResultObject.ResultMsgType.Warning, Utilities.ResultProcedures.freightbill, Utilities.ResultTitles.TitlePendingFeeApprovalWarning, Utilities.ResultPrefix.MsgUnexpectedFeeValidationIssue, Utilities.ResultSuffix.MsgDoesNotEffectProcess)
                                        Continue For
                                    End If

                                End If
                            End If

                        Catch ex As Exception
                            Dim sDetails = String.Format(" SHID {0}, freight bill number{1}, fee {2}, error {3} . ", s.BookSHID, s.InvoiceNo, sFee.BookFeesPendingCaption, ex.Message)
                            sRetMsg = appendToResultMessage(oResults, sDetails, Models.ResultObject.ResultMsgType.Err, Utilities.ResultProcedures.freightbill, Utilities.ResultTitles.TitlePendingFeeApprovalError, Utilities.ResultPrefix.MsgDetails, Utilities.ResultSuffix.None)
                            'add to system error log
                            Dim sRecord = String.Format(" For SHID: {0} , Freight Bill Number: {1}, and Fee: {2}", s.BookSHID, s.InvoiceNo, sFee.BookFeesPendingCaption)
                            logSystemError(ex, "NGLTMS365BLL.runAutoApprovePendingFees", sRecord)
                        End Try
                    Next
                End If
            Next
        Catch ex As Exception
            Dim sDetails = String.Format(" SHID {0}, freight bill number {1}, error {2}. ", s.BookSHID, s.InvoiceNo, ex.Message)
            sRetMsg = appendToResultMessage(oResults, sDetails, Models.ResultObject.ResultMsgType.Err, Utilities.ResultProcedures.freightbill, Utilities.ResultTitles.TitlePendingFeeApprovalError, Utilities.ResultPrefix.MsgDetails, Utilities.ResultSuffix.None)
            'add to system error log
            Dim sRecord = String.Format(" For SHID: {0} , and Freight Bill Number: {1}", s.BookSHID, s.InvoiceNo)
            logSystemError(ex, "NGLTMS365BLL.runAutoApprovePendingFees", sRecord)
        End Try

        Return True
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
    ''' Modified by RHR for 8.2.1.004 on 12/26/2019
    '''   applies new localize formats and result type logic for generating errors, warnings and positive result messages
    '''   new simplified version from BLL which reads the data from the localization library
    '''   Replaces previous logic in the BLL (wrapper methods still exists for backward compatibility)
    ''' </remarks>
    Public Function appendToResultMessage(ByRef oResults As Models.ResultObject,
                                          ByVal sDetails As String,
                                          ByVal eResultType As Models.ResultObject.ResultMsgType,
                                          ByVal eProcedureType As Utilities.ResultProcedures,
                                          ByVal eTitleType As Utilities.ResultTitles,
                                          ByVal ePrefixType As Utilities.ResultPrefix,
                                          ByVal eSuffixType As Utilities.ResultSuffix) As String
        Dim strRet As String = ""
        Try
            Dim sTitle As String = NGLcmLocalizeKeyValuePairObjData.readResultTitle(eTitleType)
            Dim sMsg = String.Format(" {0} {1} {2}", NGLcmLocalizeKeyValuePairObjData.readResultPrefix(ePrefixType), sDetails, NGLcmLocalizeKeyValuePairObjData.readResultSuffix(eSuffixType))
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

    ''' <summary>
    '''  Code called from the following:
    '''  SettlementSave in 365 (Web Tender) 
    '''  EDI (210In ProcessData)
    '''  Desktop AP Mass Entry Screen
    '''  and Settlement Quick Edit in 365
    '''  after calling this procedure the caller must
    '''  execute NGLBookRevenueBLL.RecalculateUsingLineHaul
    '''  then NGLBookData.UpdateAndAuditAPMassEntry.
    ''' </summary>
    ''' <param name="s"></param>
    ''' <param name="ElectronicFlag"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2.1.004 on 12/26/2019
    '''   new DAL version from copied from BLL so methods can be called from DAL directly
    '''   Replaces previous logic in the BLL (wrapper methods still exists for backward compatibility)
    '''   at this point the caller must call NGLBookRevenueBLL.RecalculateUsingLineHaul
    '''   then call NGLBookData.UpdateAndAuditAPMassEntry.  The following are required:
    '''   APMassEntryControl returned in  oResults.Control, 
    '''   BookSHID, 
    '''   BookControl, 
    '''   and the InvoiceNo
    ''' </remarks>
    Public Function SettlementSave(ByVal s As Models.SettlementSave, ByVal ElectronicFlag As Boolean) As Models.ResultObject

        ' NOTES From BLL
        'work flow, the previous workflow was out of step with the updated business requirements
        '     Requirements:
        '        1.  Create a copy of the Expected cost in Book Rev History before updating and freight charges
        '        2.  Save a copy of all freight bill accessorials in the APMassEntryFees table
        '        3.  Allow updates to the APMassEntryFees (including fuel) if not in AA status, could also impact total cost in header, Line Haul cannot be modified once it is created
        '        4.  Change the logic used to populate the AP Fee buckets with approved costs each time costs are recalculated
        '        5.  Log carrier changes to fees that do not match currently approved cost in the pending fees table
        '        6.  Automaticall update the the approved fees based on carrier fee profile settings and tolerances
        '        7.  When an expected fee is not provided by the carrier the audit freight bull routine will go to M and a message will
        '            inform the user that a missing expected fee for x with a value of y was not provided.  Users must approve this before the freight bill will pass audit.
        '        8.  When a missing fee is approved at zero cost the approved fee will be modified with a zero cost order specific fee and the audit will pass
        '        9.  When a missing fee is approved with a non-zero cost it will be added to the AP Fees data, the Pending Fees data, and the and the approved fees data
        '        10. AP Fees and Pending Fees will associated with missing carrier fees will be flagged to allow future reporting
        '        11. The audit cannot pass when pending or missing fees are identified
        '        12. The line haul audit is associated with total cost we do not audit the line haul directly so if all pending fees are approved and the audit fails it
        '            must be the result of an invalid line haul amount.  TMS Users must have the ability to edit the line haul in the pending fee approval table
        '        13. Pending Fee Approval must use allocation rules to generate the correct total amount on the approval screen, so TMS users can edit the total
        '            for example Fuel is allocated across the entier load so on fee should be listed for fuel on the pending fee approval screen
        '    Data Storage:
        '        1.  BookRevenue Data (fields included in the book table)
        '        2.  BookFees 
        '        3.  BookFeesPending
        '        3.  BookRevHistory
        '        4.  BookRevHistoryFees 
        '        5.  APMassEntry
        '        6.  APMassEntryFees
        '        7.  APMassEntryHistory  (it is not clear how this data is used)
        '        8.  APMassEntryHistoryFees (new, it is not clear how this data is used)
        '        9.  APExport (header data, item details and fees are returned at run time from bookitem and bookfees tables)
        '     Process Flow:
        '        1.  Create an APMassEntry record or update the existing, add logic to prevent updates to AA status
        '            Actual data will be written to the APMassEntryHistory table and the APMassEntryHistoryFees table.
        '            The APMassEntryHistoryFees table holds the actual data provided, not the allocated amount, this will be a snapshot of the Freight Bill
        '            before any allocation or other alterations have been made during the audit and approval process.
        '        2.  if creating a new APMassEntry record create a snapshot of Expected Cost in BookRevenueHistory and BookRevHistoryFees
        '        3.  insert the Billed Fees into the APMassEntryFees table costs are allocated by order number or stop as provided by carrier 
        '            costs assigned to stop 0 (pickup) are allocated to all orders using TMS/Carrier specific allocation rules.
        '            if Fuel does not match total fuel for expected cost a spot/flat rate fuel charge will be used and allocation
        '            will follow the spot/flat rate fuel allocation rules.
        '        4.  
        ' Note:  new logic is needed to replace spSaveAndAllocateAPMassEntryFee from EDI
        '         This function is called by the D365 settlement fees controller and by the
        '         clsEDI210.ProcessData method.
        ' Modified by RHR for v-8.2.0.119 on 09/27/19
        '     we now use the new DAL.Utilities.NGLStoredProcError to identify the type of error
        '     and perform the correct actions
        ' Modified by RHR for v-8.2.1.004 on 12/23/2019
        '   Moved all primary functionality and Data Access Code to the DAL so 
        '   Legacy freight bill processing can use the same code base.  some functionality
        '   like; runManualApprovePendingFee can only be performed in the BLL library.
        '   ..
        '   Calls to  bllBookRev.RecalculateUsingLineHaul(s.BookControl) must be called from the BLL so the general
        '   logic to Save The Settlement data is split into two parts.
        '   The legacy components mus be modify to execut this new 
        '   

        Logger.Information("NGLBookData.SettlementSave: Start")
        Dim oResults As New Models.ResultObject() With {.Success = True, .SuccessMsg = "Success!"}
        Dim strErrMsg As String = ""
        Dim sbRetMsgs As New System.Text.StringBuilder()
        Dim iAPMassEntryControl As Integer = 0

        Try
            'process flow:
            '1. Does this Freight Bill Exists
            If String.IsNullOrWhiteSpace(s.InvoiceNo) Then
                'we cannot continue return an exception
                throwFieldRequiredException("Freight Bill Invoice Number")
            End If
            iAPMassEntryControl = NGLAPMassEntryObjData.DoesFreightBillExist(s.InvoiceNo, s.CarrierControl)
            If iAPMassEntryControl = 0 Then
                '2. Insert History on New Freight Bill
                If Not InsertFreightBillHistory(s, ElectronicFlag, strErrMsg, oResults) Then
                    createSubscriptionAlert(strErrMsg) ' We need to identify the correct alert?
                End If
                addAuditMessage(strErrMsg, sbRetMsgs, iAPMassEntryControl, NGLLookupDataProvider.FBLoadStatusCodes.FBUnexpected, True, buildProcedureName("SettlementSave--InsertFreightBillHistoryLogs"))
                strErrMsg = "" 'clear the string
                '3. Create Expected on New Freight Bill
                If Not createExpectedCost(s, strErrMsg, oResults, True) Then
                    createSubscriptionAlert(strErrMsg) ' We need to identify the correct alert? sbRetMsgs.Append(strErrMsg)
                End If
                addAuditMessage(strErrMsg, sbRetMsgs, iAPMassEntryControl, NGLLookupDataProvider.FBLoadStatusCodes.FBUnexpected, True, buildProcedureName("SettlementSave--createExpectedCost"))
                strErrMsg = "" 'clear the string
            End If
            '4. and 5. are now combined into CreateOrUpdateAPMassEntry Insert  or update the freight bill data
            If Not createOrUpdateAPMassEntry(s, ElectronicFlag, strErrMsg, oResults) Then
                createSubscriptionAlert(strErrMsg) ' We need to identify the correct alert?
                addAuditMessage(strErrMsg, sbRetMsgs, iAPMassEntryControl, NGLLookupDataProvider.FBLoadStatusCodes.FBUnexpected, True, buildProcedureName("SettlementSave--createOrUpdateAPMassEntry"))
                'we cannot continue if the update to the ap mass entry fails
                'Return sbRetMsgs.ToString
                Return oResults
            End If
            iAPMassEntryControl = s.APControl
            oResults.BookControl = s.BookControl
            'check for any messages and coninue
            addAuditMessage(strErrMsg, sbRetMsgs, iAPMassEntryControl, NGLLookupDataProvider.FBLoadStatusCodes.FBUnexpected, True, buildProcedureName("SettlementSave--createOrUpdateAPMassEntry"))
            strErrMsg = "" 'clear the string
            'if we get here the freight bill exists so now we process the fees
            '6. Process the Billed Fees (includes logic to add to book fees pending)
            If Not processBilledFees(s, strErrMsg) Then
                createSubscriptionAlert(strErrMsg) ' We need to identify the correct alert?
            End If
            addAuditMessage(strErrMsg, sbRetMsgs, iAPMassEntryControl, NGLLookupDataProvider.FBLoadStatusCodes.FBUnexpected, True, buildProcedureName("SettlementSave--processBilledFees"))
            strErrMsg = "" 'clear the string
            '7. Run Auto Approve function for fees
            If Not runAutoApprovePendingFees(s, strErrMsg, oResults) Then
                createSubscriptionAlert(strErrMsg) ' We need to identify the correct alert? 
            End If
            addAuditMessage(strErrMsg, sbRetMsgs, iAPMassEntryControl, NGLLookupDataProvider.FBLoadStatusCodes.FBUnexpected, True, buildProcedureName("SettlementSave--runAutoApprovePendingFees"))
            strErrMsg = "" 'clear the string
            ' save teh control number
            oResults.Control = iAPMassEntryControl
            'at this point the caller must call NGLBookRevenueBLL.RecalculateUsingLineHaul
            'then call NGLBookData.UpdateAndAuditAPMassEntry with oResults.Control as the APMassEntryControl
            'the caller must also know the following;
            '   BookSHID, BookControl, and the InvoiceNo
        Catch ex As System.ServiceModel.FaultException(Of SqlFaultInfo)
            Dim strMsg = ex.Detail.getMsgForLogs()
            strErrMsg = appendToResultMessage(oResults, strMsg, Models.ResultObject.ResultMsgType.Err, Utilities.ResultProcedures.freightbill, Utilities.ResultTitles.TitleAuditFreightBillWarning, Utilities.ResultPrefix.MsgDetails, Utilities.ResultSuffix.None)
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("SettlementSave"), sysErrorParameters.sysErrorState.SystemLevelFault, sysErrorParameters.sysErrorSeverity.Unexpected)
        End Try
        Return oResults

    End Function

    Public Function UpdateAndAuditAPMassEntry(ByRef oResults As Models.ResultObject, ByVal iAPMassEntryControl As Integer, ByVal sBookSHID As String, ByVal iBookControl As Integer, ByVal sInvoiceNo As String, ByVal ElectronicFlag As Boolean) As Models.ResultObject

        If oResults.Success = False Then
            Return oResults 'cannot continue
        End If
        Dim strErrMsg As String = ""
        Dim sbRetMsgs As New System.Text.StringBuilder()

        Try
            ' we must have an apcontrol to process fuel,  this should always pass if the caller does it's job
            If iAPMassEntryControl > 0 Then
                'update the ap mass entry fees with the approved fees including fuel
                NGLAPMassEntryFeesData.UpdateAPMassEntryFees(iAPMassEntryControl, sBookSHID)
                ' Modified by RHR for v-8.2.1.004 on 01/10/2020
                ' Once all the fees have been processed/added to the AP Mass Entry Fees 
                'we can update the AP Carrier Cost if it Is Not provided by the carrier
                NGLAPMassEntryObjData.UpdateZeroCarrierCostUsingTotalFees(iAPMassEntryControl)

                'Run the FreightBill Audit Routine
                '   a) update fee buckets
                '   b) update ap total costs
                '   c) run audit
                ' Modified by RHR for v-8.2.0.119 on 09/27/19
                Dim oRet = NGLAPMassEntryObjData.AuditFreightBill365(iAPMassEntryControl, iBookControl)
                If Not oRet Is Nothing AndAlso oRet.Count() > 0 AndAlso oRet(0).ErrNumber <> 0 Then
                    Dim sDetails = String.Format(" {0}, freight bill number {1}. ", oRet(0).RetMsg, sInvoiceNo)
                    Select Case oRet(0).ErrNumber.Value
                        Case Utilities.NGLStoredProcError.ActionRequired, Utilities.NGLStoredProcError.InvalidKey
                            'do nothing at this time already logged by stored procedure, 
                            'user does Not need to see this message as an error on Settlement Save
                        Case Is > 9
                            'log the message
                            strErrMsg = appendToResultMessage(oResults, sDetails, Models.ResultObject.ResultMsgType.Warning, Utilities.ResultProcedures.freightbill, Utilities.ResultTitles.TitleAuditFreightBillWarning, Utilities.ResultPrefix.MsgDetails, Utilities.ResultSuffix.None)
                            addAuditMessage(strErrMsg, sbRetMsgs, iAPMassEntryControl, NGLLookupDataProvider.FBLoadStatusCodes.FBUnexpected, True, buildProcedureName("UpdateAndAuditAPMassEntry"))

                        Case Else
                            'the stored procedure already logged the message just update the return value
                            appendToResultMessage(oResults, sDetails, Models.ResultObject.ResultMsgType.Warning, Utilities.ResultProcedures.freightbill, Utilities.ResultTitles.TitleAuditFreightBillWarning, Utilities.ResultPrefix.MsgDetails, Utilities.ResultSuffix.None)

                    End Select
                    strErrMsg = "" 'clear the string
                End If
            End If


        Catch ex As System.ServiceModel.FaultException(Of SqlFaultInfo)
            Dim strMsg = ex.Detail.getMsgForLogs()
            strErrMsg = appendToResultMessage(oResults, strMsg, Models.ResultObject.ResultMsgType.Err, Utilities.ResultProcedures.freightbill, Utilities.ResultTitles.TitleAuditFreightBillWarning, Utilities.ResultPrefix.MsgDetails, Utilities.ResultSuffix.None)
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("UpdateAndAuditAPMassEntry"), sysErrorParameters.sysErrorState.SystemLevelFault, sysErrorParameters.sysErrorSeverity.Unexpected)
        End Try
        Return oResults

    End Function

    ''' <summary>
    ''' Read Fee Data For AP Mass Entry Data Entry
    ''' </summary>
    ''' <param name="BookSHID"></param>
    ''' <param name="CarrierNumber"></param>
    ''' <param name="CompNumber"></param>
    ''' <param name="APFee1"></param>
    ''' <param name="APFee2"></param>
    ''' <param name="APFee3"></param>
    ''' <param name="APFee4"></param>
    ''' <param name="APFee5"></param>
    ''' <param name="APFee6"></param>
    ''' <param name="APOtherCosts"></param>
    ''' <param name="APTaxDetail1"></param>
    ''' <param name="APTaxDetail2"></param>
    ''' <param name="APTaxDetail3"></param>
    ''' <param name="APTaxDetail4"></param>
    ''' <param name="APTaxDetail5"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2.1.004 on 12/23/2019
    '''     Looks up the Fee data For the provided SHID
    '''     based on Fee mappings to EDI codes
    '''     For buckets 1 To 6 And tax buckets 1 To 5
    '''     Other Costs Get mapped To MSC (42)
    '''     If no fees are provided all expected And unapproved 
    '''     pending fees are retuned
    '''     If at least one fee Is provided the system checks For
    '''     any missing expected Or pending fees And marks them
    '''     as missing.  New logic in v-8.2.1.004 requires user
    '''     approval for all missing fees before the audit will pass
    '''     NOTE:  the procedure For saving billed fees must
    '''            have matching modifications To only save
    '''            the fees provided that are Not missing.
    '''            If no fees are provided billed fees are 
    '''            Not included in the historical record
    ''' </remarks>
    Public Function GetDataForAPMassEntryFees(ByVal BookSHID As String,
                                              ByVal CarrierNumber As Integer,
                                              ByVal CompNumber As Integer,
                                              ByVal APFee1 As Decimal,
                                              ByVal APFee2 As Decimal,
                                              ByVal APFee3 As Decimal,
                                              ByVal APFee4 As Decimal,
                                              ByVal APFee5 As Decimal,
                                              ByVal APFee6 As Decimal,
                                              ByVal APOtherCosts As Decimal,
                                              ByVal APTaxDetail1 As Decimal,
                                              ByVal APTaxDetail2 As Decimal,
                                              ByVal APTaxDetail3 As Decimal,
                                              ByVal APTaxDetail4 As Decimal,
                                              ByVal APTaxDetail5 As Decimal) As Models.SettlementFee()
        Dim oRet As Models.SettlementFee()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try

                oRet = (From d In db.spGetDataForAPMassEntryFees(BookSHID, CarrierNumber, CompNumber, APFee1, APFee2, APFee3, APFee4, APFee5, APFee6, APOtherCosts, APTaxDetail1, APTaxDetail2, APTaxDetail3, APTaxDetail4, APTaxDetail5)
                        Select New Models.SettlementFee _
                            With {.AccessorialCode = d.AccessorialCode,
                                .BilledFee = d.FeeBilled,
                                .BookCarrOrderNumber = d.BookCarrOrderNumber,
                                .BookControl = d.BookControl,
                                .BookOrderSequence = d.BookOrderSequence,
                                .Caption = d.Caption,
                                .Control = d.FeeControl,
                                .Cost = d.Cost,
                                .EDICode = d.EDICode,
                                .Minimum = d.Minimum,
                                .MissingFee = d.MissingFee,
                                .Pending = d.Pending,
                                .StopSequence = d.StopSequence
                                }).ToArray()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetDataForAPMassEntryFees"))
            End Try
            Return oRet
        End Using
    End Function



#End Region


End Class