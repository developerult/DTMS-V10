Imports System.ServiceModel
Imports NGL.Core.ChangeTracker

Public Class NGLBookLoadDetailData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMasBookDataContext(ConnectionString)
        Me.LinqTable = db.vBookLoadMaintenances
        Me.LinqDB = db
        Me.SourceClass = "NGLBookLoadDetailData"
    End Sub

#End Region

#Region " Properties "

    Private _RecalcTotals As Boolean = False

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMasBookDataContext(ConnectionString)
            Me.LinqTable = db.vBookLoadMaintenances
            Me.LinqDB = db
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


#End Region

#Region "Public Methods"

    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return GetBookLoadDetailFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetBookLoadDetailsFiltered()
    End Function

    Public Function GetBookLoadDetailsByCarrier(ByVal CarrierControl As Integer) As DataTransferObjects.BookLoadDetail()
        If CarrierControl = 0 Then Return Nothing
        Return GetBookLoadDetailsFiltered(CarrierControl:=CarrierControl)
    End Function

    Public Function GetBookLoadDetailsByComp(ByVal CompControl As Integer) As DataTransferObjects.BookLoadDetail()
        If CompControl = 0 Then Return Nothing
        Return GetBookLoadDetailsFiltered(CompControl:=CompControl)
    End Function

    Public Function GetBookLoadDetailsByLane(ByVal LaneControl As Integer) As DataTransferObjects.BookLoadDetail()
        If LaneControl = 0 Then Return Nothing
        Return GetBookLoadDetailsFiltered(LaneControl:=LaneControl)
    End Function

    Public Function GetBookLoadDetailFiltered(Optional ByVal Control As Integer = 0,
                                              Optional ByVal BookProNumber As String = "",
                                              Optional ByVal BookConsPrefix As String = "",
                                              Optional ByVal BookLoadPONumber As String = "",
                                              Optional ByVal BookCarrBLNumber As String = "",
                                              Optional ByVal BookFinAPBillNumber As String = "",
                                              Optional ByVal BookCarrOrderNumber As String = "",
                                              Optional ByVal BookOrderSequence As Integer = 0,
                                              Optional ByVal BookSHID As String = "") As DataTransferObjects.BookLoadDetail
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria                
                Dim BookLoadDetail As DataTransferObjects.BookLoadDetail = (
                        From d In db.vBookLoadMaintenances
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
                        (BookSHID Is Nothing OrElse String.IsNullOrEmpty(BookSHID) OrElse d.BookSHID = BookSHID) _
                        And
                        (BookOrderSequence = 0 OrElse d.BookOrderSequence = BookOrderSequence)
                        Order By d.BookStopNo Ascending
                        Select SelectDTOData(d)).FirstOrDefault()

                Return BookLoadDetail

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetBookLoadDetailFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetBookLoadDetailsFiltered(Optional ByVal BookProNumber As String = "",
                                               Optional ByVal BookConsPrefix As String = "",
                                               Optional ByVal BookCarrBLNumber As String = "",
                                               Optional ByVal BookFinAPBillNumber As String = "",
                                               Optional ByVal BookCarrOrderNumber As String = "",
                                               Optional ByVal CompControl As Integer = 0,
                                               Optional ByVal LaneControl As Integer = 0,
                                               Optional ByVal CarrierControl As Integer = 0,
                                               Optional ByVal BookSHID As String = "") As DataTransferObjects.BookLoadDetail()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'db.Log = New DebugTextWriter
                'Get the newest record that matches the provided criteria
                Dim BookLoadDetails() As DataTransferObjects.BookLoadDetail = (
                        From d In db.vBookLoadMaintenances
                        Where
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
                        (BookSHID Is Nothing OrElse String.IsNullOrEmpty(BookSHID) OrElse d.BookSHID = BookSHID) _
                        And
                        (CompControl = 0 OrElse d.BookCustCompControl = CompControl) _
                        And
                        (d.BookODControl = If(LaneControl = 0, d.BookODControl, LaneControl)) _
                        And
                        (d.BookCarrierControl = If(CarrierControl = 0, d.BookCarrierControl, CarrierControl))
                        Order By d.BookProNumber Descending
                        Select SelectDTOData(d, 1, 1, 0, 20)).Take(20).ToArray()

                If Not String.IsNullOrWhiteSpace(BookConsPrefix) Then
                    Return BookLoadDetails.OrderBy(Function(x) x.BookStopNo).ToArray()
                Else
                    Return BookLoadDetails
                End If

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetBookLoadDetailsFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetBookLoadDetailsByPONumber(ByVal BookLoadPONumber As String) As DataTransferObjects.BookLoadDetail()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try

                If BookLoadPONumber Is Nothing OrElse String.IsNullOrEmpty(BookLoadPONumber) Then Return Nothing
                Dim oBLs = From bl In db.BookLoads Where bl.BookLoadPONumber = BookLoadPONumber Select bl.BookLoadBookControl

                'Get the newest record that matches the provided criteria
                Dim BookLoadDetails() As DataTransferObjects.BookLoadDetail = (
                        From d In db.vBookLoadMaintenances
                        Where oBLs.Contains(d.BookControl)
                        Order By d.BookStopNo Ascending
                        Select SelectDTOData(d, 1, 1, 0, 20)).Take(20).ToArray()

                Return BookLoadDetails

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetBookLoadDetailsByPONumber"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetBookLoadDetailsFilteredContains(Optional ByVal BookProNumber As String = "",
                                                       Optional ByVal BookConsPrefix As String = "",
                                                       Optional ByVal BookCarrBLNumber As String = "",
                                                       Optional ByVal BookFinAPBillNumber As String = "",
                                                       Optional ByVal BookCarrOrderNumber As String = "",
                                                       Optional ByVal BookSHID As String = "") As DataTransferObjects.BookLoadDetail()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oSecureComp = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                'convert any * to %
                If Not String.IsNullOrEmpty(BookProNumber) Then BookProNumber = BookProNumber.Replace("*", "%")
                If Not String.IsNullOrEmpty(BookConsPrefix) Then BookConsPrefix = BookConsPrefix.Replace("*", "%")
                If Not String.IsNullOrEmpty(BookCarrBLNumber) Then BookCarrBLNumber = BookCarrBLNumber.Replace("*", "%")
                If Not String.IsNullOrEmpty(BookFinAPBillNumber) Then BookFinAPBillNumber = BookFinAPBillNumber.Replace("*", "%")
                If Not String.IsNullOrEmpty(BookCarrOrderNumber) Then BookCarrOrderNumber = BookCarrOrderNumber.Replace("*", "%")
                If Not String.IsNullOrEmpty(BookSHID) Then BookSHID = BookSHID.Replace("*", "%")
                'db.Log = New DebugTextWriter
                'Get the newest record that matches the provided criteria
                Dim BookLoadDetails() As DataTransferObjects.BookLoadDetail = (
                        From d In db.vBookLoadMaintenances
                        Where
                        (BookProNumber Is Nothing OrElse String.IsNullOrEmpty(BookProNumber) OrElse System.Data.Linq.SqlClient.SqlMethods.Like(d.BookProNumber, BookProNumber)) _
                        And
                        (BookConsPrefix Is Nothing OrElse String.IsNullOrEmpty(BookConsPrefix) OrElse System.Data.Linq.SqlClient.SqlMethods.Like(d.BookConsPrefix, BookConsPrefix)) _
                        And
                        (BookCarrBLNumber Is Nothing OrElse String.IsNullOrEmpty(BookCarrBLNumber) OrElse System.Data.Linq.SqlClient.SqlMethods.Like(d.BookCarrBLNumber, BookCarrBLNumber)) _
                        And
                        (BookFinAPBillNumber Is Nothing OrElse String.IsNullOrEmpty(BookFinAPBillNumber) OrElse System.Data.Linq.SqlClient.SqlMethods.Like(d.BookFinAPBillNumber, BookFinAPBillNumber)) _
                        And
                        (BookCarrOrderNumber Is Nothing OrElse String.IsNullOrEmpty(BookCarrOrderNumber) OrElse System.Data.Linq.SqlClient.SqlMethods.Like(d.BookCarrOrderNumber, BookCarrOrderNumber)) _
                        And
                        (BookSHID Is Nothing OrElse String.IsNullOrEmpty(BookSHID) OrElse System.Data.Linq.SqlClient.SqlMethods.Like(d.BookSHID, BookSHID)) _
                        And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.BookCustCompControl))
                        Order By d.BookProNumber Descending
                        Select SelectDTOData(d, 1, 1, 0, 20)).Take(20).ToArray()

                Return BookLoadDetails

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetBookLoadDetailsFilteredContains"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetBookLoadDetailsByPONumberContains(ByVal BookLoadPONumber As String) As DataTransferObjects.BookLoadDetail()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oSecureComp = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                'convert any * to %
                If Not String.IsNullOrEmpty(BookLoadPONumber) Then BookLoadPONumber = BookLoadPONumber.Replace("*", "%")
                If BookLoadPONumber Is Nothing OrElse String.IsNullOrEmpty(BookLoadPONumber) Then Return Nothing
                Dim oBLs = From bl In db.BookLoads Where System.Data.Linq.SqlClient.SqlMethods.Like(bl.BookLoadPONumber, BookLoadPONumber) Select bl.BookLoadBookControl
                'Get the newest record that matches the provided criteria
                Dim BookLoadDetails() As DataTransferObjects.BookLoadDetail = (
                        From d In db.vBookLoadMaintenances
                        Where oBLs.Contains(d.BookControl) _
                              And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.BookCustCompControl))
                        Order By d.BookStopNo Ascending
                        Select SelectDTOData(d, 1, 1, 0, 20)).Take(20).ToArray()

                Return BookLoadDetails

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetBookLoadDetailsByPONumberContains"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' This override does not call the standard cleanup methods and returns nothing.
    ''' The caller must check the BookDependencyResult properties (ReturnBookControl,MustRecalculate,RetMsg, and ErrNumber) 
    ''' , recalculate costs the caller must select and return the updated book record
    ''' </summary>
    ''' <typeparam name="TEntity"></typeparam>
    ''' <param name="oData"></param>
    ''' <param name="oLinqTable"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function Update(Of TEntity As Class)(ByVal oData As Object,
                                                          ByVal oLinqTable As System.Data.Linq.Table(Of TEntity)) As Object
        Dim source As DataTransferObjects.BookLoadDetail = TryCast(oData, DataTransferObjects.BookLoadDetail)
        If source Is Nothing Then Return Nothing
        SaveChanges(oData)
        Me.BookDependencyResult = DirectCast(Me.NDPBaseClassFactory("NGLBookData", False), NGLBookData).UpdateBookDependencies(source.BookControl, 0)
        Me.LastProcedureName = "spUpdateBookDependencies"
        Return Nothing

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
        Dim source As DataTransferObjects.BookLoadDetail = TryCast(oData, DataTransferObjects.BookLoadDetail)
        If source Is Nothing Then Return
        SaveChanges(oData)
        Me.BookDependencyResult = DirectCast(Me.NDPBaseClassFactory("NGLBookData", False), NGLBookData).UpdateBookDependencies(source.BookControl, 0)
        Me.LastProcedureName = "spUpdateBookDependencies"
    End Sub

    ''' <summary>
    ''' Saves the load details data, updates dependencies and prepares the fees for spot rates by 
    ''' changing the fee type to order fee for all fees that have not be marked as overridden.
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <remarks>
    ''' Created by RHR v-7.0.5.100 05/06/2016
    ''' </remarks>
    Public Sub UpdateBeforeSpotRate(ByVal oData As DataTransferObjects.BookLoadDetail)
        Dim source As DataTransferObjects.BookLoadDetail = TryCast(oData, DataTransferObjects.BookLoadDetail)
        If source Is Nothing Then Return
        Dim sSHID As String = oData.BookSHID
        SaveChanges(oData)
        Me.BookDependencyResult = DirectCast(Me.NDPBaseClassFactory("NGLBookData", False), NGLBookData).UpdateBookDependencies(source.BookControl, 0)
        If Not String.IsNullOrWhiteSpace(sSHID) Then
            Using db As New NGLMasBookDataContext(ConnectionString)
                Try
                    Dim oReturnData = db.spPrepareBookFeesForSpotRate(sSHID)
                Catch ex As Exception
                    'do nothing here
                End Try
            End Using
        End If
        Me.LastProcedureName = "spUpdateBookDependencies"
    End Sub



    Public Overrides Sub Delete(Of TEntity As Class)(ByVal oData As Object,
                                                     ByVal oLinqTable As System.Data.Linq.Table(Of TEntity))
        Using LinqDB
            'Note: the ValidateData Function must throw a FaultException error on failure
            ValidateDeletedRecord(LinqDB, oData)
            Try
                With CType(oData, DataTransferObjects.BookLoadDetail)

                    'Open the existing Record
                    Dim nObject As LTS.Book = (From e In CType(LinqDB, NGLMasBookDataContext).Books Where e.BookControl = .BookControl Select e).First
                    If nObject Is Nothing Then
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_RecordDeleted"}, New FaultReason("E_InvalidOperationException"))
                    Else
                        CType(LinqDB, NGLMasBookDataContext).Books.DeleteOnSubmit(nObject)
                        LinqDB.SubmitChanges()
                    End If

                End With
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("Delete"))
            End Try
        End Using
    End Sub

#End Region

#Region "Protected Methods"

    ''' <summary>
    ''' CopyDTOToLinq
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by LVV 6/22/16 for v-7.0.5.110 DAT
    ''' Added fields BookRevLoadTenderTypeControl
    ''' and BookRevLoadTenderStatusCode
    ''' Modified by LVV 10/25/16 for v-7.0.5.110 Add Book Interline
    ''' Added field BookCarrTarInterlinePoint
    ''' Modified By LVV on 11/1/16 for v-7.0.5.110 Lane Default Carrier Enhancements
    ''' Added field BookRevPreferredCarrier
    ''' </remarks>
    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = TryCast(oData, DataTransferObjects.BookLoadDetail)
        Dim strCompAbrev As String = getScalarString("Select top 1 isnull(dbo.Comp.CompAbrev,'') From dbo.Comp Where dbo.Comp.CompControl = " & d.BookCustCompControl)
        Dim strBookPro As String = strCompAbrev & d.BookProBase

        'Create New Record
        Return New LTS.vBookLoadMaintenance With {.BookControl = d.BookControl _
            , .BookProNumber = d.BookProNumber _
            , .BookProBase = d.BookProBase _
            , .BookConsPrefix = d.BookConsPrefix _
            , .BookCustCompControl = d.BookCustCompControl _
            , .BookCommCompControl = d.BookCommCompControl _
            , .BookODControl = d.BookODControl _
            , .BookCarrierControl = d.BookCarrierControl _
            , .BookCarrierContact = d.BookCarrierContact _
            , .BookCarrierContactPhone = d.BookCarrierContactPhone _
            , .BookOrigCompControl = d.BookOrigCompControl _
            , .BookOrigName = d.BookOrigName _
            , .BookOrigAddress1 = d.BookOrigAddress1 _
            , .BookOrigAddress2 = d.BookOrigAddress2 _
            , .BookOrigAddress3 = d.BookOrigAddress3 _
            , .BookOrigCity = d.BookOrigCity _
            , .BookOrigState = d.BookOrigState _
            , .BookOrigCountry = d.BookOrigCountry _
            , .BookOrigZip = d.BookOrigZip _
            , .BookOrigPhone = d.BookOrigPhone _
            , .BookOrigFax = d.BookOrigFax _
            , .BookOriginStartHrs = d.BookOriginStartHrs _
            , .BookOriginStopHrs = d.BookOriginStopHrs _
            , .BookOriginApptReq = d.BookOriginApptReq _
            , .BookDestCompControl = d.BookDestCompControl _
            , .BookDestName = d.BookDestName _
            , .BookDestAddress1 = d.BookDestAddress1 _
            , .BookDestAddress2 = d.BookDestAddress2 _
            , .BookDestAddress3 = d.BookDestAddress3 _
            , .BookDestCity = d.BookDestCity _
            , .BookDestState = d.BookDestState _
            , .BookDestCountry = d.BookDestCountry _
            , .BookDestZip = d.BookDestZip _
            , .BookDestPhone = d.BookDestPhone _
            , .BookDestFax = d.BookDestFax _
            , .BookDestStartHrs = d.BookDestStartHrs _
            , .BookDestStopHrs = d.BookDestStopHrs _
            , .BookDestApptReq = d.BookDestApptReq _
            , .BookDateOrdered = d.BookDateOrdered _
            , .BookDateLoad = d.BookDateLoad _
            , .BookDateInvoice = d.BookDateInvoice _
            , .BookFinARInvoiceDate = d.BookFinARInvoiceDate _
            , .BookDateRequired = d.BookDateRequired _
            , .BookDateDelivered = d.BookDateDelivered _
            , .BookTotalCases = d.BookTotalCases _
            , .BookTotalWgt = d.BookTotalWgt _
            , .BookTotalPL = d.BookTotalPL _
            , .BookTotalCube = d.BookTotalCube _
            , .BookTotalPX = d.BookTotalPX _
            , .BookTotalBFC = d.BookTotalBFC _
            , .BookTranCode = d.BookTranCode _
            , .BookPayCode = d.BookPayCode _
            , .BookTypeCode = d.BookTypeCode _
            , .BookBOLCode = d.BookBOLCode _
            , .BookStopNo = d.BookStopNo _
            , .BookModDate = Date.Now _
            , .BookModUser = Me.Parameters.UserName _
            , .BookCarrBLNumber = d.BookCarrBLNumber _
            , .BookCarrOrderNumber = d.BookCarrOrderNumber _
            , .BookFinAPBillNumber = d.BookFinAPBillNumber _
            , .BookRevBilledBFC = d.BookRevBilledBFC _
            , .BookRevTotalCost = d.BookRevTotalCost _
            , .BookRouteFinalDate = d.BookRouteFinalDate _
            , .BookRouteFinalCode = d.BookRouteFinalCode _
            , .BookRouteFinalFlag = d.BookRouteFinalFlag _
            , .BookWarehouseNumber = d.BookWarehouseNumber _
            , .BookComCode = d.BookComCode _
            , .BookTransType = d.BookTransType _
            , .BookCarrActDate = d.BookCarrActDate _
            , .BookCarrActTime = d.BookCarrActTime _
            , .BookRouteConsFlag = d.BookRouteConsFlag _
            , .BookHotLoad = d.BookHotLoad _
            , .BookMilesFrom = d.BookMilesFrom _
            , .BookCarrierContControl = d.BookCarrierContControl _
            , .BookExportDocCreated = d.BookExportDocCreated _
            , .BookDoNotInvoice = d.BookDoNotInvoice _
            , .BookOrderSequence = d.BookOrderSequence _
            , .BookCarrierEquipmentCodes = d.BookCarrierEquipmentCodes _
            , .BookDateRequested = d.BookDateRequested _
            , .BookShipCarrierProNumber = d.BookShipCarrierProNumber _
            , .BookShipCarrierProNumberRaw = d.BookShipCarrierProNumberRaw _
            , .BookShipCarrierProControl = d.BookShipCarrierProControl _
            , .BookShipCarrierName = d.BookShipCarrierName _
            , .BookShipCarrierNumber = d.BookShipCarrierNumber _
            , .BookAPAdjReasonControl = d.BookAPAdjReasonControl _
            , .BookLockAllCosts = d.BookLockAllCosts _
            , .BookLockBFCCost = d.BookLockBFCCost _
            , .BookDestStopNumber = d.BookDestStopNumber _
            , .BookOrigStopNumber = d.BookOrigStopNumber _
            , .BookDestStopControl = d.BookDestStopControl _
            , .BookOrigStopControl = d.BookOrigStopControl _
            , .BookRouteTypeCode = d.BookRouteTypeCode _
            , .BookAlternateAddressLaneControl = d.BookAlternateAddressLaneControl _
            , .BookAlternateAddressLaneNumber = d.BookAlternateAddressLaneNumber _
            , .BookDefaultRouteSequence = d.BookDefaultRouteSequence _
            , .BookRouteGuideControl = d.BookRouteGuideControl _
            , .BookRouteGuideNumber = d.BookRouteGuideNumber _
            , .BookCustomerApprovalRecieved = d.BookCustomerApprovalRecieved _
            , .BookCustomerApprovalTransmitted = d.BookCustomerApprovalTransmitted _
            , .BookCarrTruckControl = d.BookCarrTruckControl _
            , .BookCarrTarControl = d.BookCarrTarControl _
            , .BookCarrTarRevisionNumber = d.BookCarrTarRevisionNumber _
            , .BookCarrTarName = d.BookCarrTarName _
            , .BookCarrTarEquipControl = d.BookCarrTarEquipControl _
            , .BookCarrTarEquipName = d.BookCarrTarEquipName _
            , .BookCarrTarEquipMatControl = d.BookCarrTarEquipMatControl _
            , .BookCarrTarEquipMatName = d.BookCarrTarEquipMatName _
            , .BookCarrTarEquipMatDetControl = d.BookCarrTarEquipMatDetControl _
            , .BookCarrTarEquipMatDetID = d.BookCarrTarEquipMatDetID _
            , .BookCarrTarEquipMatDetValue = d.BookCarrTarEquipMatDetValue _
            , .BookBookRevHistRevision = d.BookBookRevHistRevision _
            , .BookModeTypeControl = d.BookModeTypeControl _
            , .BookAllowInterlinePoints = d.BookAllowInterlinePoints _
            , .BookRevLaneBenchMiles = d.BookRevLaneBenchMiles _
            , .BookRevLoadMiles = d.BookRevLoadMiles _
            , .BookUser1 = d.BookUser1 _
            , .BookUser2 = d.BookUser2 _
            , .BookUser3 = d.BookUser3 _
            , .BookUser4 = d.BookUser4 _
            , .BookMustLeaveByDateTime = d.BookMustLeaveByDateTime _
            , .BookExpDelDateTime = d.BookExpDelDateTime _
            , .BookMultiMode = d.BookMultiMode _
            , .BookOriginalLaneControl = d.BookOriginalLaneControl _
            , .BookLaneTranXControl = d.BookLaneTranXControl _
            , .BookLaneTranXDetControl = d.BookLaneTranXDetControl _
            , .BookCreditHold = d.BookCreditHold _
            , .BookSHID = d.BookSHID _
            , .BookRevLoadTenderTypeControl = d.BookRevLoadTenderTypeControl _
            , .BookRevLoadTenderStatusCode = d.BookRevLoadTenderStatusCode _
            , .BookCarrTarInterlinePoint = d.BookCarrTarInterlinePoint _
            , .BookRevPreferredCarrier = d.BookRevPreferredCarrier}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Dim source As LTS.vBookLoadMaintenance = TryCast(LinqTable, LTS.vBookLoadMaintenance)
        If source Is Nothing Then Return Nothing
        Return GetBookLoadDetailFiltered(Control:=source.BookControl)
    End Function

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'Check if the data already exists only one allowed      
        With CType(oData, DataTransferObjects.BookLoadDetail)
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
                    'New code added by RHR 8/11/13
                    If CType(oDB, NGLMasBookDataContext).Books.Where(Function(x) x.BookProNumber = .BookProNumber).Count > 0 Then throwInvalidKeyAlreadyExistsException("Book", "Book Pro number", .BookProNumber)
                    'Old code removed by RHR 8/11/13 (trying new logic for validation)
                    '' ''verify that the bookpronumber is not in use
                    ' ''Dim BookLoadDetail As DTO.BookLoadDetail = ( _
                    ' ''    From t In CType(oDB, NGLMasBookDataContext).Books _
                    ' ''     Where _
                    ' ''         t.BookProNumber = .BookProNumber _
                    ' ''     Select New DTO.BookLoadDetail With {.BookControl = t.BookControl}).First
                    ' ''If Not BookLoadDetail Is Nothing Then
                    ' ''    Utilities.SaveAppError("Cannot save new Book data.  The Book Pro number, " & .BookProNumber & " ,  already exist.", Me.Parameters)
                    ' ''    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                    ' ''End If
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
        With CType(oData, DataTransferObjects.BookLoadDetail)
            Try
                'New code added by RHR 8/11/13
                If CType(oDB, NGLMasBookDataContext).Books.Where(Function(x) x.BookControl <> .BookControl And x.BookProNumber = .BookProNumber).Count > 0 Then throwInvalidKeyAlreadyExistsException("Book", "Book Pro number", .BookProNumber)
                'Old code removed by RHR 8/11/13 (trying new logic for validation)
                '' ''Get the newest record that matches the provided criteria
                ' ''Dim BookLoadDetail As DTO.BookLoadDetail = ( _
                ' ''From t In CType(oDB, NGLMasBookDataContext).Books _
                ' '' Where _
                ' ''     (t.BookControl <> .BookControl) _
                ' ''     And _
                ' ''     (t.BookProNumber = .BookProNumber) _
                ' '' Select New DTO.BookLoadDetail With {.BookControl = t.BookControl}).First

                ' ''If Not BookLoadDetail Is Nothing Then
                ' ''    Utilities.SaveAppError("Cannot save Book changes.  The Book PRO Number, " & .BookProNumber & " already exist.", Me.Parameters)
                ' ''    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                ' ''End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'Check if the BookLoadDetail is being used by the BookLoadDetail data or the lane data
        With CType(oData, DataTransferObjects.BookLoadDetail)
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

    ''' <summary>
    ''' SaveChanges
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <remarks>
    ''' Modified by LVV 6/22/16 for v-7.0.5.110 DAT
    ''' Added fields BookRevLoadTenderTypeControl
    ''' and BookRevLoadTenderStatusCode
    ''' Modified by LVV 10/25/16 for v-7.0.5.110 Add Book Interline
    ''' Added field BookCarrTarInterlinePoint
    ''' Modified By LVV on 11/1/16 for v-7.0.5.110 Lane Default Carrier Enhancements
    ''' Added field BookRevPreferredCarrier
    ''' </remarks>
    Private Sub SaveChanges(ByVal oData As DataTransferObjects.BookLoadDetail)
        Using LinqDB
            'Note: the ValidateData Function must throw a FaultException error on failure
            ValidateUpdatedRecord(LinqDB, oData)
            Dim blnBookTotalPalletsChanged = False
            Dim dblBookTotalPallets As Double = 0
            Dim iBookControl As Integer = 0
            With oData
                Try
                    Dim strCompAbrev As String = getScalarString("Select top 1 isnull(dbo.Comp.CompAbrev,'') From dbo.Comp Where dbo.Comp.CompControl = " & .BookCustCompControl)
                    Dim strBookPro As String = strCompAbrev & .BookProBase
                    'Open the existing Record
                    Dim d = (From e In CType(LinqDB, NGLMasBookDataContext).Books Where e.BookControl = .BookControl Select e).FirstOrDefault()
                    If d Is Nothing OrElse d.BookControl = 0 Then
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
                            addToConflicts("BookProBase", .BookProBase, d.BookProBase, ConflictData, blnConflictFound)
                            addToConflicts("BookConsPrefix", .BookConsPrefix, d.BookConsPrefix, ConflictData, blnConflictFound)
                            addToConflicts("BookCustCompControl", .BookCustCompControl, d.BookCustCompControl, ConflictData, blnConflictFound)
                            addToConflicts("BookCommCompControl", .BookCommCompControl, d.BookCommCompControl, ConflictData, blnConflictFound)
                            addToConflicts("BookODControl", .BookODControl, d.BookODControl, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrierControl", .BookCarrierControl, d.BookCarrierControl, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrierContact", .BookCarrierContact, d.BookCarrierContact, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrierContactPhone", .BookCarrierContactPhone, d.BookCarrierContactPhone, ConflictData, blnConflictFound)
                            addToConflicts("BookOrigCompControl", .BookOrigCompControl, If(d.BookOrigCompControl.HasValue, d.BookOrigCompControl, 0), ConflictData, blnConflictFound)
                            addToConflicts("BookOrigName", .BookOrigName, d.BookOrigName, ConflictData, blnConflictFound)
                            addToConflicts("BookOrigAddress1", .BookOrigAddress1, d.BookOrigAddress1, ConflictData, blnConflictFound)
                            addToConflicts("BookOrigAddress2", .BookOrigAddress2, d.BookOrigAddress2, ConflictData, blnConflictFound)
                            addToConflicts("BookOrigAddress3", .BookOrigAddress3, d.BookOrigAddress3, ConflictData, blnConflictFound)
                            addToConflicts("BookOrigCity", .BookOrigCity, d.BookOrigCity, ConflictData, blnConflictFound)
                            addToConflicts("BookOrigState", .BookOrigState, d.BookOrigState, ConflictData, blnConflictFound)
                            addToConflicts("BookOrigCountry", .BookOrigCountry, d.BookOrigCountry, ConflictData, blnConflictFound)
                            addToConflicts("BookOrigZip", .BookOrigZip, d.BookOrigZip, ConflictData, blnConflictFound)
                            addToConflicts("BookOrigPhone", .BookOrigPhone, d.BookOrigPhone, ConflictData, blnConflictFound)
                            addToConflicts("BookOrigFax", .BookOrigFax, d.BookOrigFax, ConflictData, blnConflictFound)
                            addToConflicts("BookOriginStartHrs", .BookOriginStartHrs, d.BookOriginStartHrs, ConflictData, blnConflictFound)
                            addToConflicts("BookOriginStopHrs", .BookOriginStopHrs, d.BookOriginStopHrs, ConflictData, blnConflictFound)
                            addToConflicts("BookOriginApptReq", .BookOriginApptReq, If(d.BookOriginApptReq.HasValue, d.BookOriginApptReq, False), ConflictData, blnConflictFound)
                            addToConflicts("BookDestCompControl", .BookDestCompControl, If(d.BookDestCompControl.HasValue, d.BookDestCompControl, 0), ConflictData, blnConflictFound)
                            addToConflicts("BookDestAddress1", .BookDestAddress1, d.BookDestAddress1, ConflictData, blnConflictFound)
                            addToConflicts("BookDestAddress2", .BookDestAddress2, d.BookDestAddress2, ConflictData, blnConflictFound)
                            addToConflicts("BookDestAddress3", .BookDestAddress3, d.BookDestAddress3, ConflictData, blnConflictFound)
                            addToConflicts("BookDestCity", .BookDestCity, d.BookDestCity, ConflictData, blnConflictFound)
                            addToConflicts("BookDestState", .BookDestState, d.BookDestState, ConflictData, blnConflictFound)
                            addToConflicts("BookDestCountry", .BookDestCountry, d.BookDestCountry, ConflictData, blnConflictFound)
                            addToConflicts("BookDestZip", .BookDestZip, d.BookDestZip, ConflictData, blnConflictFound)
                            addToConflicts("BookDestPhone", .BookDestPhone, d.BookDestPhone, ConflictData, blnConflictFound)
                            addToConflicts("BookDestFax", .BookDestFax, d.BookDestFax, ConflictData, blnConflictFound)
                            addToConflicts("BookDestStartHrs", .BookDestStartHrs, d.BookDestStartHrs, ConflictData, blnConflictFound)
                            addToConflicts("BookDestStopHrs", .BookDestStopHrs, d.BookDestStopHrs, ConflictData, blnConflictFound)
                            addToConflicts("BookDestApptReq", .BookDestApptReq, If(d.BookDestApptReq.HasValue, d.BookDestApptReq, False), ConflictData, blnConflictFound)
                            addToConflicts("BookDateOrdered", .BookDateOrdered, d.BookDateOrdered, ConflictData, blnConflictFound)
                            addToConflicts("BookDateLoad", .BookDateLoad, d.BookDateLoad, ConflictData, blnConflictFound)
                            addToConflicts("BookDateInvoice", .BookDateInvoice, d.BookDateInvoice, ConflictData, blnConflictFound)
                            addToConflicts("BookFinARInvoiceDate", .BookFinARInvoiceDate, d.BookFinARInvoiceDate, ConflictData, blnConflictFound)
                            addToConflicts("BookDateRequired", .BookDateRequired, d.BookDateRequired, ConflictData, blnConflictFound)
                            addToConflicts("BookDateDelivered", .BookDateDelivered, d.BookDateDelivered, ConflictData, blnConflictFound)
                            addToConflicts("BookTotalCases", .BookTotalCases, If(d.BookTotalCases.HasValue, d.BookTotalCases, 0), ConflictData, blnConflictFound)
                            addToConflicts("BookTotalWgt", .BookTotalWgt, If(d.BookTotalWgt.HasValue, d.BookTotalWgt, 0), ConflictData, blnConflictFound)
                            addToConflicts("BookTotalPL", .BookTotalPL, If(d.BookTotalPL.HasValue, d.BookTotalPL, 0), ConflictData, blnConflictFound)
                            addToConflicts("BookTotalCube", .BookTotalCube, If(d.BookTotalCube.HasValue, d.BookTotalCube, 0), ConflictData, blnConflictFound)
                            addToConflicts("BookTotalPX", .BookTotalPX, If(d.BookTotalPX.HasValue, d.BookTotalPX, 0), ConflictData, blnConflictFound)
                            addToConflicts("BookTotalBFC", .BookTotalBFC, If(d.BookTotalBFC.HasValue, d.BookTotalBFC, 0), ConflictData, blnConflictFound)
                            addToConflicts("BookTranCode", .BookTranCode, d.BookTranCode, ConflictData, blnConflictFound)
                            addToConflicts("BookPayCode", .BookPayCode, d.BookPayCode, ConflictData, blnConflictFound)
                            addToConflicts("BookTypeCode", .BookTypeCode, d.BookTypeCode, ConflictData, blnConflictFound)
                            addToConflicts("BookBOLCode", .BookBOLCode, d.BookBOLCode, ConflictData, blnConflictFound)
                            addToConflicts("BookStopNo", .BookStopNo, If(d.BookStopNo.HasValue, d.BookStopNo, 0), ConflictData, blnConflictFound)
                            addToConflicts("BookCarrBLNumber", .BookCarrBLNumber, d.BookCarrBLNumber, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrOrderNumber", .BookCarrOrderNumber, d.BookCarrOrderNumber, ConflictData, blnConflictFound)
                            addToConflicts("BookFinAPBillNumber", .BookFinAPBillNumber, d.BookFinAPBillNumber, ConflictData, blnConflictFound)
                            addToConflicts("BookRevBilledBFC", .BookRevBilledBFC, If(d.BookRevBilledBFC.HasValue, d.BookRevBilledBFC, 0), ConflictData, blnConflictFound)
                            addToConflicts("BookRevTotalCost", .BookRevTotalCost, If(d.BookRevTotalCost.HasValue, d.BookRevTotalCost, 0), ConflictData, blnConflictFound)
                            addToConflicts("BookRouteFinalDate", .BookRouteFinalDate, d.BookRouteFinalDate, ConflictData, blnConflictFound)
                            addToConflicts("BookRouteFinalCode", .BookRouteFinalCode, d.BookRouteFinalCode, ConflictData, blnConflictFound)
                            addToConflicts("BookRouteFinalFlag", .BookRouteFinalFlag, d.BookRouteFinalFlag, ConflictData, blnConflictFound)
                            addToConflicts("BookWarehouseNumber", .BookWarehouseNumber, d.BookWarehouseNumber, ConflictData, blnConflictFound)
                            addToConflicts("BookComCode", .BookComCode, d.BookComCode, ConflictData, blnConflictFound)
                            addToConflicts("BookTransType", .BookTransType, d.BookTransType, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrActDate", .BookCarrActDate, d.BookCarrActDate, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrActTime", .BookCarrActTime, d.BookCarrActTime, ConflictData, blnConflictFound)
                            addToConflicts("BookRouteConsFlag", .BookRouteConsFlag, d.BookRouteConsFlag, ConflictData, blnConflictFound)
                            addToConflicts("BookHotLoad", .BookHotLoad, If(d.BookHotLoad.HasValue, d.BookHotLoad, False), ConflictData, blnConflictFound)
                            addToConflicts("BookMilesFrom", .BookMilesFrom, If(d.BookMilesFrom.HasValue, d.BookMilesFrom, 0), ConflictData, blnConflictFound)
                            addToConflicts("BookCarrierContControl", .BookCarrierContControl, If(d.BookCarrierContControl.HasValue, d.BookCarrierContControl, 0), ConflictData, blnConflictFound)
                            addToConflicts("BookExportDocCreated", .BookExportDocCreated, If(d.BookExportDocCreated.HasValue, d.BookExportDocCreated, False), ConflictData, blnConflictFound)
                            addToConflicts("BookDoNotInvoice", .BookDoNotInvoice, d.BookDoNotInvoice, ConflictData, blnConflictFound)
                            addToConflicts("BookOrderSequence", .BookOrderSequence, d.BookOrderSequence, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrierEquipmentCodes", .BookCarrierEquipmentCodes, d.BookCarrierEquipmentCodes, ConflictData, blnConflictFound)
                            addToConflicts("BookDateRequested", .BookDateRequested, d.BookDateRequested, ConflictData, blnConflictFound)
                            addToConflicts("BookShipCarrierProNumber", .BookShipCarrierProNumber, d.BookShipCarrierProNumber, ConflictData, blnConflictFound)
                            addToConflicts("BookShipCarrierProNumberRaw", .BookShipCarrierProNumberRaw, d.BookShipCarrierProNumberRaw, ConflictData, blnConflictFound)
                            addToConflicts("BookShipCarrierProControl", .BookShipCarrierProControl, d.BookShipCarrierProControl, ConflictData, blnConflictFound)
                            addToConflicts("BookShipCarrierName", .BookShipCarrierName, d.BookShipCarrierName, ConflictData, blnConflictFound)
                            addToConflicts("BookShipCarrierNumber", .BookShipCarrierNumber, d.BookShipCarrierNumber, ConflictData, blnConflictFound)
                            addToConflicts("BookAPAdjReasonControl", .BookAPAdjReasonControl, d.BookAPAdjReasonControl, ConflictData, blnConflictFound)
                            addToConflicts("BookLockAllCosts", .BookLockAllCosts, d.BookLockAllCosts, ConflictData, blnConflictFound)
                            addToConflicts("BookLockBFCCost", .BookLockBFCCost, d.BookLockBFCCost, ConflictData, blnConflictFound)
                            addToConflicts("BookDestStopNumber", .BookDestStopNumber, d.BookDestStopNumber, ConflictData, blnConflictFound)
                            addToConflicts("BookOrigStopNumber", .BookOrigStopNumber, d.BookOrigStopNumber, ConflictData, blnConflictFound)
                            addToConflicts("BookDestStopControl", .BookDestStopControl, d.BookDestStopControl, ConflictData, blnConflictFound)
                            addToConflicts("BookOrigStopControl", .BookOrigStopControl, d.BookOrigStopControl, ConflictData, blnConflictFound)
                            addToConflicts("BookRouteTypeCode", .BookRouteTypeCode, d.BookRouteTypeCode, ConflictData, blnConflictFound)
                            addToConflicts("BookAlternateAddressLaneControl", .BookAlternateAddressLaneControl, d.BookAlternateAddressLaneControl, ConflictData, blnConflictFound)
                            addToConflicts("BookDefaultRouteSequence", .BookDefaultRouteSequence, d.BookDefaultRouteSequence, ConflictData, blnConflictFound)
                            addToConflicts("BookRouteGuideControl", .BookRouteGuideControl, d.BookRouteGuideControl, ConflictData, blnConflictFound)
                            addToConflicts("BookCustomerApprovalRecieved", .BookCustomerApprovalRecieved, d.BookCustomerApprovalRecieved, ConflictData, blnConflictFound)
                            addToConflicts("BookCustomerApprovalTransmitted", .BookCustomerApprovalTransmitted, d.BookCustomerApprovalTransmitted, ConflictData, blnConflictFound)

                            addToConflicts("BookCarrTruckControl", .BookCarrTruckControl, d.BookCarrTruckControl, ConflictData, blnConflictFound)
                            addToConflicts("BookModeTypeControl", .BookModeTypeControl, d.BookModeTypeControl, ConflictData, blnConflictFound)
                            addToConflicts("BookAllowInterlinePoints", .BookAllowInterlinePoints, d.BookAllowInterlinePoints, ConflictData, blnConflictFound)
                            addToConflicts("BookUser1", .BookUser1, d.BookUser1, ConflictData, blnConflictFound)
                            addToConflicts("BookUser2", .BookUser2, d.BookUser2, ConflictData, blnConflictFound)
                            addToConflicts("BookUser3", .BookUser3, d.BookUser3, ConflictData, blnConflictFound)
                            addToConflicts("BookUser4", .BookUser4, d.BookUser4, ConflictData, blnConflictFound)
                            addToConflicts("BookMustLeaveByDateTime", .BookMustLeaveByDateTime, d.BookMustLeaveByDateTime, ConflictData, blnConflictFound)
                            addToConflicts("BookExpDelDateTime", .BookExpDelDateTime, d.BookExpDelDateTime, ConflictData, blnConflictFound)
                            addToConflicts("BookMultiMode", .BookMultiMode, d.BookMultiMode, ConflictData, blnConflictFound)
                            addToConflicts("BookOriginalLaneControl", .BookOriginalLaneControl, d.BookOriginalLaneControl, ConflictData, blnConflictFound)
                            addToConflicts("BookCreditHold", .BookCreditHold, d.BookCreditHold, ConflictData, blnConflictFound)
                            addToConflicts("BookSHID", .BookSHID, d.BookSHID, ConflictData, blnConflictFound)
                            'Added by LVV 6/22/16 for v-7.0.5.110 DAT
                            addToConflicts("BookRevLoadTenderTypeControl", .BookRevLoadTenderTypeControl, d.BookRevLoadTenderTypeControl, ConflictData, blnConflictFound)
                            addToConflicts("BookRevLoadTenderStatusCode", .BookRevLoadTenderStatusCode, d.BookRevLoadTenderStatusCode, ConflictData, blnConflictFound)
                            'Added by LVV 10/25/16 for v-7.0.5.110 Add Book Interline
                            addToConflicts("BookCarrTarInterlinePoint", .BookCarrTarInterlinePoint, d.BookCarrTarInterlinePoint, ConflictData, blnConflictFound)
                            'Added By LVV on 11/1/16 for v-7.0.5.110 Lane Default Carrier Enhancements
                            addToConflicts("BookRevPreferredCarrier", .BookRevPreferredCarrier, d.BookRevPreferredCarrier, ConflictData, blnConflictFound)

                            If blnConflictFound Then
                                'We only add the mod date and mod user if one or more other fields have been modified
                                addToConflicts("BookModDate", .BookModDate, d.BookModDate, ConflictData, blnConflictFound)
                                addToConflicts("BookModUser", .BookModUser, d.BookModUser, ConflictData, blnConflictFound)
                                Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(ConflictData))
                                conflictInfo.LogConflictDetails(conflictInfo.Message, Me.Parameters)
                                Throw New FaultException(Of ConflictFaultInfo)(conflictInfo, New FaultReason("E_DataConflict"))
                            End If
                        End If
                        If .BookTotalPL <> If(d.BookTotalPL, 0) Then
                            blnBookTotalPalletsChanged = True
                            dblBookTotalPallets = .BookTotalPL
                            iBookControl = .BookControl
                        End If
                        'Update the table data with non-readonly data fields
                        d.BookProNumber = strBookPro
                        d.BookProBase = .BookProBase
                        d.BookConsPrefix = .BookConsPrefix
                        d.BookCustCompControl = .BookCustCompControl
                        d.BookCommCompControl = .BookCommCompControl
                        d.BookODControl = .BookODControl
                        d.BookCarrierControl = .BookCarrierControl
                        d.BookCarrierContact = .BookCarrierContact
                        d.BookCarrierContactPhone = .BookCarrierContactPhone
                        d.BookOrigCompControl = .BookOrigCompControl
                        d.BookOrigName = .BookOrigName
                        d.BookOrigAddress1 = .BookOrigAddress1
                        d.BookOrigAddress2 = .BookOrigAddress2
                        d.BookOrigAddress3 = .BookOrigAddress3
                        d.BookOrigCity = .BookOrigCity
                        d.BookOrigState = .BookOrigState
                        d.BookOrigCountry = .BookOrigCountry
                        d.BookOrigZip = .BookOrigZip
                        d.BookOrigPhone = .BookOrigPhone
                        d.BookOrigFax = .BookOrigFax
                        d.BookOriginStartHrs = .BookOriginStartHrs
                        d.BookOriginStopHrs = .BookOriginStopHrs
                        d.BookOriginApptReq = .BookOriginApptReq
                        d.BookDestCompControl = .BookDestCompControl
                        d.BookDestName = .BookDestName
                        d.BookDestAddress1 = .BookDestAddress1
                        d.BookDestAddress2 = .BookDestAddress2
                        d.BookDestAddress3 = .BookDestAddress3
                        d.BookDestCity = .BookDestCity
                        d.BookDestState = .BookDestState
                        d.BookDestCountry = .BookDestCountry
                        d.BookDestZip = .BookDestZip
                        d.BookDestPhone = .BookDestPhone
                        d.BookDestFax = .BookDestFax
                        d.BookDestStartHrs = .BookDestStartHrs
                        d.BookDestStopHrs = .BookDestStopHrs
                        d.BookDestApptReq = .BookDestApptReq
                        d.BookDateOrdered = .BookDateOrdered
                        d.BookDateLoad = .BookDateLoad
                        d.BookDateInvoice = .BookDateInvoice
                        d.BookFinARInvoiceDate = .BookFinARInvoiceDate
                        d.BookDateRequired = .BookDateRequired
                        d.BookDateDelivered = .BookDateDelivered
                        d.BookTotalCases = .BookTotalCases
                        d.BookTotalWgt = .BookTotalWgt
                        d.BookTotalPL = .BookTotalPL
                        d.BookTotalCube = .BookTotalCube
                        d.BookTotalPX = .BookTotalPX
                        d.BookTotalBFC = .BookTotalBFC
                        d.BookTranCode = .BookTranCode
                        d.BookPayCode = .BookPayCode
                        d.BookTypeCode = .BookTypeCode
                        d.BookBOLCode = .BookBOLCode
                        d.BookOrigStopNumber = If(.BookOrigStopNumber < 1, 1, .BookOrigStopNumber)
                        d.BookDestStopNumber = calculateBookDestStopNumber(d.BookStopNo, .BookStopNo, d.BookDestStopNumber)
                        d.BookStopNo = If(.BookStopNo < 1, 1, .BookStopNo)
                        d.BookModDate = Date.Now
                        d.BookModUser = Me.Parameters.UserName
                        d.BookCarrBLNumber = .BookCarrBLNumber
                        d.BookCarrOrderNumber = .BookCarrOrderNumber
                        d.BookFinAPBillNumber = .BookFinAPBillNumber
                        d.BookRevBilledBFC = .BookRevBilledBFC
                        d.BookRevTotalCost = .BookRevTotalCost
                        d.BookRouteFinalDate = .BookRouteFinalDate
                        d.BookRouteFinalCode = .BookRouteFinalCode
                        d.BookRouteFinalFlag = .BookRouteFinalFlag
                        d.BookWarehouseNumber = .BookWarehouseNumber
                        d.BookComCode = .BookComCode
                        d.BookTransType = .BookTransType
                        d.BookCarrActDate = .BookCarrActDate
                        d.BookCarrActTime = .BookCarrActTime
                        d.BookRouteConsFlag = .BookRouteConsFlag
                        d.BookHotLoad = .BookHotLoad
                        d.BookMilesFrom = .BookMilesFrom
                        d.BookCarrierContControl = .BookCarrierContControl
                        d.BookExportDocCreated = .BookExportDocCreated
                        d.BookDoNotInvoice = .BookDoNotInvoice
                        d.BookOrderSequence = .BookOrderSequence
                        d.BookCarrierEquipmentCodes = .BookCarrierEquipmentCodes
                        d.BookDateRequested = .BookDateRequested
                        d.BookShipCarrierProNumber = .BookShipCarrierProNumber
                        d.BookShipCarrierProNumberRaw = .BookShipCarrierProNumberRaw
                        d.BookShipCarrierProControl = .BookShipCarrierProControl
                        d.BookShipCarrierName = .BookShipCarrierName
                        d.BookShipCarrierNumber = .BookShipCarrierNumber
                        d.BookAPAdjReasonControl = .BookAPAdjReasonControl
                        d.BookLockAllCosts = .BookLockAllCosts
                        d.BookLockBFCCost = .BookLockBFCCost
                        d.BookDestStopControl = .BookDestStopControl
                        d.BookOrigStopControl = .BookOrigStopControl
                        d.BookRouteTypeCode = .BookRouteTypeCode
                        d.BookAlternateAddressLaneControl = .BookAlternateAddressLaneControl
                        d.BookAlternateAddressLaneNumber = .BookAlternateAddressLaneNumber
                        d.BookDefaultRouteSequence = .BookDefaultRouteSequence
                        d.BookRouteGuideControl = .BookRouteGuideControl
                        d.BookRouteGuideNumber = .BookRouteGuideNumber
                        d.BookCustomerApprovalRecieved = .BookCustomerApprovalRecieved
                        d.BookCustomerApprovalTransmitted = .BookCustomerApprovalTransmitted
                        d.BookCarrTruckControl = .BookCarrTruckControl
                        d.BookModeTypeControl = .BookModeTypeControl
                        d.BookAllowInterlinePoints = .BookAllowInterlinePoints
                        d.BookUser1 = .BookUser1
                        d.BookUser2 = .BookUser2
                        d.BookUser3 = .BookUser3
                        d.BookUser4 = .BookUser4
                        d.BookMustLeaveByDateTime = .BookMustLeaveByDateTime
                        d.BookExpDelDateTime = .BookExpDelDateTime
                        d.BookMultiMode = .BookMultiMode
                        d.BookOriginalLaneControl = .BookOriginalLaneControl
                        d.BookCreditHold = .BookCreditHold
                        d.BookSHID = .BookSHID
                        'Added by LVV 6/22/16 for v-7.0.5.110 DAT
                        d.BookRevLoadTenderTypeControl = .BookRevLoadTenderTypeControl
                        d.BookRevLoadTenderStatusCode = .BookRevLoadTenderStatusCode
                        'Added by LVV 10/25/16 for v-7.0.5.110 Add Book Interline
                        d.BookCarrTarInterlinePoint = .BookCarrTarInterlinePoint
                        'Added By LVV on 11/1/16 for v-7.0.5.110 Lane Default Carrier Enhancements
                        d.BookRevPreferredCarrier = .BookRevPreferredCarrier
                    End If
                    LinqDB.SubmitChanges()
                Catch ex As Exception
                    ManageLinqDataExceptions(ex, buildProcedureName("SaveChanges"))
                End Try
                If blnBookTotalPalletsChanged Then
                    Dim oREt = CType(LinqDB, NGLMasBookDataContext).spAllocateBookTotalPalletsToItems(iBookControl, dblBookTotalPallets, Me.Parameters.UserName)
                End If

            End With
        End Using
    End Sub

    ''' <summary>
    ''' SelectDTOData
    ''' </summary>
    ''' <param name="d"></param>
    ''' <param name="page"></param>
    ''' <param name="pagecount"></param>
    ''' <param name="recordcount"></param>
    ''' <param name="pagesize"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by LVV 6/22/16 for v-7.0.5.110 DAT
    ''' Added fields BookRevLoadTenderTypeControl
    ''' and BookRevLoadTenderStatusCode
    ''' Modified by LVV 10/25/16 for v-7.0.5.110 Add Book Interline
    ''' Added field BookCarrTarInterlinePoint
    ''' Modified By LVV on 11/1/16 for v-7.0.5.110 Lane Default Carrier Enhancements
    ''' Added field BookRevPreferredCarrier
    ''' </remarks>
    Friend Function SelectDTOData(ByVal d As LTS.vBookLoadMaintenance, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.BookLoadDetail
        Return New DataTransferObjects.BookLoadDetail With {.BookControl = d.BookControl _
            , .BookProNumber = d.BookProNumber _
            , .BookProBase = d.BookProBase _
            , .BookConsPrefix = d.BookConsPrefix _
            , .BookCustCompControl = d.BookCustCompControl _
            , .BookCommCompControl = d.BookCommCompControl _
            , .BookODControl = d.BookODControl _
            , .BookCarrierControl = d.BookCarrierControl _
            , .BookCarrierContact = d.BookCarrierContact _
            , .BookCarrierContactPhone = d.BookCarrierContactPhone _
            , .BookOrigCompControl = If(d.BookOrigCompControl.HasValue, d.BookOrigCompControl, 0) _
            , .BookOrigName = d.BookOrigName _
            , .BookOrigAddress1 = d.BookOrigAddress1 _
            , .BookOrigAddress2 = d.BookOrigAddress2 _
            , .BookOrigAddress3 = d.BookOrigAddress3 _
            , .BookOrigCity = d.BookOrigCity _
            , .BookOrigState = d.BookOrigState _
            , .BookOrigCountry = d.BookOrigCountry _
            , .BookOrigZip = d.BookOrigZip _
            , .BookOrigPhone = d.BookOrigPhone _
            , .BookOrigFax = d.BookOrigFax _
            , .BookOriginStartHrs = d.BookOriginStartHrs _
            , .BookOriginStopHrs = d.BookOriginStopHrs _
            , .BookOriginApptReq = If(d.BookOriginApptReq.HasValue, d.BookOriginApptReq, False) _
            , .BookDestCompControl = If(d.BookDestCompControl.HasValue, d.BookDestCompControl, 0) _
            , .BookDestName = d.BookDestName _
            , .BookDestAddress1 = d.BookDestAddress1 _
            , .BookDestAddress2 = d.BookDestAddress2 _
            , .BookDestAddress3 = d.BookDestAddress3 _
            , .BookDestCity = d.BookDestCity _
            , .BookDestState = d.BookDestState _
            , .BookDestCountry = d.BookDestCountry _
            , .BookDestZip = d.BookDestZip _
            , .BookDestPhone = d.BookDestPhone _
            , .BookDestFax = d.BookDestFax _
            , .BookDestStartHrs = d.BookDestStartHrs _
            , .BookDestStopHrs = d.BookDestStopHrs _
            , .BookDestApptReq = If(d.BookDestApptReq.HasValue, d.BookDestApptReq, False) _
            , .BookDateOrdered = d.BookDateOrdered _
            , .BookDateLoad = d.BookDateLoad _
            , .BookDateInvoice = d.BookDateInvoice _
            , .BookDateRequired = d.BookDateRequired _
            , .BookDateDelivered = d.BookDateDelivered _
            , .BookTotalCases = If(d.BookTotalCases.HasValue, d.BookTotalCases, 0) _
            , .BookTotalWgt = If(d.BookTotalWgt.HasValue, d.BookTotalWgt, 0) _
            , .BookTotalPL = If(d.BookTotalPL.HasValue, d.BookTotalPL, 0) _
            , .BookTotalCube = If(d.BookTotalCube.HasValue, d.BookTotalCube, 0) _
            , .BookTotalPX = If(d.BookTotalPX.HasValue, d.BookTotalPX, 0) _
            , .BookTotalBFC = If(d.BookTotalBFC.HasValue, d.BookTotalBFC, 0) _
            , .BookTranCode = d.BookTranCode _
            , .BookPayCode = d.BookPayCode _
            , .BookTypeCode = d.BookTypeCode _
            , .BookBOLCode = d.BookBOLCode _
            , .BookStopNo = If(d.BookStopNo.HasValue, d.BookStopNo, 0) _
            , .BookCarrOrderNumber = d.BookCarrOrderNumber _
            , .BookCarrBLNumber = d.BookCarrBLNumber _
            , .BookCarrActDate = d.BookCarrActDate _
            , .BookCarrActTime = d.BookCarrActTime _
            , .BookFinARInvoiceDate = d.BookFinARInvoiceDate _
            , .BookFinAPBillNumber = d.BookFinAPBillNumber _
            , .BookRevBilledBFC = If(d.BookRevBilledBFC.HasValue, d.BookRevBilledBFC, 0) _
            , .BookRevTotalCost = If(d.BookRevTotalCost.HasValue, d.BookRevTotalCost, 0) _
            , .BookMilesFrom = If(d.BookMilesFrom.HasValue, d.BookMilesFrom, 0) _
            , .BookRouteFinalDate = d.BookRouteFinalDate _
            , .BookRouteFinalCode = d.BookRouteFinalCode _
            , .BookRouteFinalFlag = d.BookRouteFinalFlag _
            , .BookWarehouseNumber = d.BookWarehouseNumber _
            , .BookComCode = d.BookComCode _
            , .BookTransType = d.BookTransType _
            , .BookRouteConsFlag = d.BookRouteConsFlag _
            , .BookHotLoad = If(d.BookHotLoad.HasValue, d.BookHotLoad, False) _
            , .BookCarrierContControl = If(d.BookCarrierContControl.HasValue, d.BookCarrierContControl, 0) _
            , .BookExportDocCreated = If(d.BookExportDocCreated.HasValue, d.BookExportDocCreated, False) _
            , .BookDoNotInvoice = d.BookDoNotInvoice _
            , .BookOrderSequence = d.BookOrderSequence _
            , .BookShipCarrierProNumber = d.BookShipCarrierProNumber _
            , .BookShipCarrierProNumberRaw = d.BookShipCarrierProNumberRaw _
            , .BookShipCarrierProControl = d.BookShipCarrierProControl _
            , .BookShipCarrierName = d.BookShipCarrierName _
            , .BookShipCarrierNumber = d.BookShipCarrierNumber _
            , .BookAPAdjReasonControl = d.BookAPAdjReasonControl _
            , .BookDateRequested = d.BookDateRequested _
            , .BookCarrierEquipmentCodes = d.BookCarrierEquipmentCodes _
            , .BookLockAllCosts = d.BookLockAllCosts _
            , .BookLockBFCCost = d.BookLockBFCCost _
            , .BookDestStopNumber = d.BookDestStopNumber _
            , .BookOrigStopNumber = d.BookOrigStopNumber _
            , .BookDestStopControl = d.BookDestStopControl _
            , .BookOrigStopControl = d.BookOrigStopControl _
            , .BookRouteTypeCode = d.BookRouteTypeCode _
            , .BookAlternateAddressLaneControl = d.BookAlternateAddressLaneControl _
            , .BookAlternateAddressLaneNumber = d.BookAlternateAddressLaneNumber _
            , .BookDefaultRouteSequence = d.BookDefaultRouteSequence _
            , .BookRouteGuideControl = d.BookRouteGuideControl _
            , .BookRouteGuideNumber = d.BookRouteGuideNumber _
            , .BookCustomerApprovalRecieved = d.BookCustomerApprovalRecieved _
            , .BookCustomerApprovalTransmitted = d.BookCustomerApprovalTransmitted _
            , .BookCarrTruckControl = d.BookCarrTruckControl _
            , .BookCarrTarControl = d.BookCarrTarControl _
            , .BookCarrTarRevisionNumber = d.BookCarrTarRevisionNumber _
            , .BookCarrTarName = d.BookCarrTarName _
            , .BookCarrTarEquipControl = d.BookCarrTarEquipControl _
            , .BookCarrTarEquipName = d.BookCarrTarEquipName _
            , .BookCarrTarEquipMatControl = d.BookCarrTarEquipMatControl _
            , .BookCarrTarEquipMatName = d.BookCarrTarEquipMatName _
            , .BookCarrTarEquipMatDetControl = d.BookCarrTarEquipMatDetControl _
            , .BookCarrTarEquipMatDetID = d.BookCarrTarEquipMatDetID _
            , .BookCarrTarEquipMatDetValue = d.BookCarrTarEquipMatDetValue _
            , .BookBookRevHistRevision = d.BookBookRevHistRevision _
            , .BookModeTypeControl = d.BookModeTypeControl _
            , .BookAllowInterlinePoints = d.BookAllowInterlinePoints _
            , .BookRevLaneBenchMiles = d.BookRevLaneBenchMiles _
            , .BookRevLoadMiles = d.BookRevLoadMiles _
            , .BookUser1 = d.BookUser1 _
            , .BookUser2 = d.BookUser2 _
            , .BookUser3 = d.BookUser3 _
            , .BookUser4 = d.BookUser4 _
            , .BookMustLeaveByDateTime = d.BookMustLeaveByDateTime _
            , .BookExpDelDateTime = d.BookExpDelDateTime _
            , .BookMultiMode = d.BookMultiMode _
            , .BookOriginalLaneControl = d.BookOriginalLaneControl _
            , .BookLaneTranXControl = d.BookLaneTranXControl _
            , .BookLaneTranXDetControl = d.BookLaneTranXDetControl _
            , .BookModDate = d.BookModDate _
            , .BookModUser = d.BookModUser _
            , .CompanyName = d.CompanyName _
            , .CompanyNumber = d.CompanyNumber _
            , .CarrierName = d.CarrierName _
            , .CarrierNumber = Convert.ToString(d.CarrierNumber) _
            , .CommissionsName = d.CommissionsName _
            , .LaneName = d.LaneName _
            , .LaneOriginAddressUse = d.LaneOriginAddressUse _
            , .CompFinUseImportFrtCost = d.CompFinUseImportFrtCost _
            , .BookCreditHold = d.BookCreditHold _
            , .BookSHID = d.BookSHID _
            , .BookRevLoadTenderTypeControl = d.BookRevLoadTenderTypeControl _
            , .BookRevLoadTenderStatusCode = d.BookRevLoadTenderStatusCode _
            , .BookCarrTarInterlinePoint = d.BookCarrTarInterlinePoint _
            , .BookRevPreferredCarrier = d.BookRevPreferredCarrier _
            , .Page = page _
            , .Pages = pagecount _
            , .RecordCount = recordcount _
            , .PageSize = pagesize}

    End Function

#End Region

End Class