Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.ServiceModel
Imports Ngl.Core.ChangeTracker
Imports Ngl.Core.Utility
Imports Ngl.FreightMaster.Data.DataTransferObjects
Imports Ngl.Test.Core
Imports SerilogTracing
Imports System.ComponentModel
Imports Serilog.Events
Imports System.Configuration

Public Class NGLBookRevenueData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        'Dim db As New NGLMasBookDataContext(ConnectionString)
        'Me.LinqTable = db.Books
        'Me.LinqDB = db
        Me.SourceClass = "NGLBookRevenueData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            'If _LinqTable Is Nothing Then
            Dim db As New NGLMasBookDataContext(ConnectionString)
            _LinqTable = db.Books
            Me.LinqDB = db
            'End If

            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

    Private _CreateHistory As Boolean = False
    Public Property CreateHistory() As Boolean
        Get
            Return _CreateHistory
        End Get
        Set(ByVal value As Boolean)
            _CreateHistory = value
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

    'functionality of _RecalcTotals and _ItemsChanged is not clearly identified
    Private _RecalcTotals As Boolean = False
    Private _ItemsChanged As Boolean = False

#End Region

#Region "Public Methods"

    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return GetBookRevenueFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    ''' <summary>
    ''' returns a BookRevenue object using the provided BookControl 
    ''' </summary>
    ''' <param name="Control"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR 9/8/14 created a new stored procedure to select the records to improve
    ''' performance this allowed us to remove the DataLoadOptions.  Runs twice a fast now
    ''' </remarks>
    Public Function GetBookRevenueFiltered(ByVal Control As Integer) As DataTransferObjects.BookRevenue
        Using operation = Logger.StartActivity("GetBookRevenueFiltered(Control: {BookControl})", Control)
            Using db As New NGLMasBookDataContext(ConnectionString)
                Try

                    'Get the newest record that matches the provided criteria
                    Dim oBookRevenue As DataTransferObjects.BookRevenue = (
                        From d In db.spGetBookRevenue(Control)
                        Select selectDTODataSP(d, db)).FirstOrDefault()
                    operation.Complete()
                    Return oBookRevenue

                Catch ex As Exception
                    Logger.Error(ex, "Error in GetBookRevenueFiltered(Control: {BookControl})", Control)
                    operation.Complete()

                    'ManageLinqDataExceptions(ex, buildProcedureName("GetBookRevenueFiltered"))
                End Try
                operation.Complete()
                Return Nothing

            End Using
        End Using
    End Function


    Public Function GetBookRevenueFilteredSpeeTest(ByVal Control As Integer) As DataTransferObjects.BookRevenue()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'db.Log = New DebugTextWriter
                'Dim oDLO As New DataLoadOptions
                'oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.CompRefBook)
                'oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.LaneRefBook)
                'db.LoadOptions = oDLO
                Return (From d In db.spGetBookRevenues(Control, False)
                        Select selectDTODataSP(d, db)).ToArray()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetBookRevenues"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Selects a Book Revenue Object with BookLoad, BookItems and BookFees children
    ''' </summary>
    ''' <param name="Control"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetBookRevenueWDetailsFiltered(ByVal Control As Integer) As DataTransferObjects.BookRevenue
        Using operation = Logger.StartActivity("GetBookRevenueWDetailsFiltered(Control: {BookControl})", Control)
            Using db As New NGLMasBookDataContext(ConnectionString)
                Try
                    Dim oDLO As New DataLoadOptions
                    oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.CompRefBook)
                    oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookLoads)
                    oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookFees)
                    oDLO.LoadWith(Of LTS.BookLoad)(Function(t As LTS.BookLoad) t.BookItems)
                    oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.LaneRefBook)
                    db.LoadOptions = oDLO

                    'Get the newest record that matches the provided criteria
                    Dim oBookRevenue As DataTransferObjects.BookRevenue = (
                        From d In db.Books
                        Where d.BookControl = Control
                        Select selectDTODataWDetails(d, db)).FirstOrDefault()
                    operation.Complete()
                    Return oBookRevenue

                Catch ex As Exception
                    Logger.Error(ex, "Error in GetBookRevenueWDetailsFiltered(Control: {BookControl})", Control)
                Finally
                    operation.Complete()
                End Try

                Return Nothing

            End Using
        End Using
    End Function

    ''' <summary>
    ''' Find the bookrevenue based on order number and sequence.
    ''' </summary>
    ''' <param name="ordernumber"></param>
    ''' <param name="orderSequence"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetBookRevenueWDetailsFiltered(ByVal ordernumber As String, ByVal orderSequence As Integer, ByVal customerControl As Integer) As DataTransferObjects.BookRevenue
        Using operation = Logger.StartActivity("GetBookRevenueWDetailsFiltered(OrderNumber: {OrderNumber}, OrderSequence: {OrderSequence}, CustomerControl: {CustomerControl})", ordernumber, orderSequence, customerControl)
            If (String.IsNullOrEmpty(ordernumber)) Then Return New DataTransferObjects.BookRevenue

            Using db As New NGLMasBookDataContext(ConnectionString)
                Try
                    Dim oDLO As New DataLoadOptions
                    oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.CompRefBook)
                    oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookLoads)
                    oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookFees)
                    oDLO.LoadWith(Of LTS.BookLoad)(Function(t As LTS.BookLoad) t.BookItems)
                    oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.LaneRefBook)
                    db.LoadOptions = oDLO
                    'Get the newest record that matches the provided criteria
                    Dim oBookRevenue As DataTransferObjects.BookRevenue = (
                        From d In db.Books
                        Where d.BookCarrOrderNumber.ToUpper = ordernumber.ToUpper _
                              And d.BookOrderSequence = orderSequence _
                              And d.BookCustCompControl = customerControl
                        Select selectDTODataWDetails(d, db)).FirstOrDefault()
                    operation.Complete()
                    Return oBookRevenue
                Catch ex As Exception
                    Logger.Error(ex, "Error in GetBookRevenueWDetailsFiltered(OrderNumber: {OrderNumber}, OrderSequence: {OrderSequence}, CustomerControl: {CustomerControl})", ordernumber, orderSequence, customerControl)
                Finally
                    operation.Complete()
                End Try

            End Using

        End Using

        Return Nothing

    End Function


    ''' <summary>
    ''' Selects an array of Book Revenue Objects with BookLoad, BookItems and BookFees children
    ''' typically this represents an entire load for the provided BookControl number
    ''' </summary>
    ''' <param name="Control"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetBookRevenuesWDetailsFiltered(ByVal Control As Integer, Optional ByVal IncludeLTLPool As Boolean = False) As DataTransferObjects.BookRevenue()
        Dim oRet As DataTransferObjects.BookRevenue()
        Using operation = Logger.StartActivity("GetBookRevenuesWDetailsFiltered(Control: {Control}, IncludeLTLPool: {IncludeLTLPool}", Control, IncludeLTLPool)
            Using db As New NGLMasBookDataContext(ConnectionString)
                Try

                    Dim oDLO As New DataLoadOptions
                    oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.CompRefBook)
                    oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookLoads)
                    oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookFees)
                    oDLO.LoadWith(Of LTS.BookLoad)(Function(t As LTS.BookLoad) t.BookItems)
                    oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.LaneRefBook)
                    db.LoadOptions = oDLO

                    oRet = (From d In db.Books
                            Join x In db.udfGetDependentBookControls(Control, IncludeLTLPool)
                                On d.BookControl Equals x.BookControl
                            Select selectDTODataWDetails(d, db)).ToArray()

                    'Dim oDepBooks As List(Of Integer) = db.spGetDependentBookControls(Control, IncludeLTLPool).Select(Function(x) x.BookControl).ToList()
                    'oRet = (From d In db.Books Where oDepBooks.Contains(d.BookControl)
                    '                       Select selectDTODataWDetails(d, db)).ToArray()

                    'Dim oDB = db.spGetDependentBookControls(Control, IncludeLTLPool)
                    'oRet = (From d In db.Books Where oDB.Select(Function(x) x.BookControl).ToList().Contains(d.BookControl)
                    '                       Select selectDTODataWDetails(d, db)).ToArray()

                Catch ex As Exception
                    operation.Complete()
                    Logger.Error(ex, "Error in GetBookRevenuesWDetailsFiltered(Control: {Control}, IncludeLTLPool: {IncludeLTLPool}", Control, IncludeLTLPool)
                End Try
            End Using


        End Using
        Return oRet

    End Function


    Public Function GetBookRevenuesWDetailsFilteredFromView(ByVal Control As Integer, Optional ByVal IncludeLTLPool As Boolean = False) As DataTransferObjects.BookRevenue()
        Dim oRet As DataTransferObjects.BookRevenue()
        Using operation = Logger.StartActivity("GetBookRevenuesWDetailsFilteredFromView(Control: {Control}, IncludeLTLPool: {IncludeLTLPool}", Control, IncludeLTLPool)
            Using db As New NGLMasBookDataContext(ConnectionString)
                Try
                    Dim oDLO As New DataLoadOptions
                    oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.CompRefBook)
                    oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookLoads)
                    oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookFees)
                    oDLO.LoadWith(Of LTS.BookLoad)(Function(t As LTS.BookLoad) t.BookItems)
                    oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.LaneRefBook)
                    db.LoadOptions = oDLO

                    Dim oDB = db.spGetDependentBookControls(Control, IncludeLTLPool)
                    oRet = (From d In db.Books Where oDB.Select(Function(x) x.BookControl).ToList().Contains(d.BookControl)
                            Select selectDTODataWDetails(d, db)).ToArray()

                Catch ex As Exception
                    operation.Complete()
                    Logger.Error(ex, "Error in GetBookRevenuesWDetailsFilteredFromView(Control: {Control}, IncludeLTLPool: {IncludeLTLPool}", Control, IncludeLTLPool)


                Finally
                    operation.Complete()
                End Try
            End Using

            Return oRet

        End Using
    End Function

    Public Function GetBookRevenuesLTSWDetailsFilteredFromView(ByVal Control As Integer, Optional ByVal IncludeLTLPool As Boolean = False) As LTS.vBookRevenue()
        Dim oRet As LTS.vBookRevenue()
        Using operation = Logger.StartActivity("GetBookRevenuesLTSWDetailsFilteredFromView(Control: {Control}, IncludeLTLPool: {IncludeLTLPool}", Control, IncludeLTLPool)
            Using db As New NGLMasBookDataContext(ConnectionString)
                Try
                    Dim oDLO As New DataLoadOptions
                    oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.CompRefBook)
                    oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookLoads)
                    oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookFees)
                    oDLO.LoadWith(Of LTS.BookLoad)(Function(t As LTS.BookLoad) t.BookItems)
                    oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.LaneRefBook)
                    db.LoadOptions = oDLO
                    Dim oDB = db.spGetDependentBookControls(Control, IncludeLTLPool)
                    oRet = (From d In db.vBookRevenues Where oDB.Select(Function(x) x.BookControl).ToList().Contains(d.BookControl)
                            Select d).ToArray()
                Catch ex As Exception
                    operation.Complete()
                    Logger.Error(ex, "Error in GetBookRevenuesLTSWDetailsFilteredFromView(Control: {Control}, IncludeLTLPool: {IncludeLTLPool}", Control, IncludeLTLPool)
                Finally
                    operation.Complete()
                End Try
            End Using
        End Using
        Return oRet


    End Function

    ''' <summary>
    ''' Read booking revenue data fields using the provided book control number
    ''' </summary>
    ''' <param name="iBookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2.0.119 on 9/17/19
    '''   typically used to display data on the Load Board Revenue Page
    ''' </remarks>
    Public Function GetvBookRevenue(ByVal iBookControl As Integer) As LTS.vBookRevenue
        Dim oRet As New LTS.vBookRevenue()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                oRet = db.vBookRevenues.Where(Function(x) x.BookControl = iBookControl).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetvBookRevenue"))
            End Try
        End Using
        Return oRet
    End Function


    ''' <summary>
    ''' Typically readonly but this funcion allows developers to save specific fields associated with the Book Revenue Data. 
    ''' Restricted to a limited number of updateable fields
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2.0.119 on 09/8/2019
    '''   Created to allow updates to specific fields (using content management)
    '''   to book revenue data if desired (typically this data is read only)
    '''   No data validation is performed in this version.
    ''' </remarks>
    Public Function SavevBookRevenue(ByVal oData As LTS.vBookRevenue) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'get the book data
                Dim oBook = db.Books.Where(Function(x) x.BookControl = oData.BookControl).FirstOrDefault()
                If Not oBook Is Nothing Then
                    With oBook
                        .BookRevBilledBFC = oData.BookRevBilledBFC
                        .BookRevCommPercent = oData.BookRevCommPercent
                        .BookRevCommCost = oData.BookRevCommCost
                        .BookFinARBookFrt = oData.BookFinARBookFrt
                        .BookFinAPPayAmt = oData.BookFinAPPayAmt
                        .BookFinAPStdCost = oData.BookFinAPStdCost
                        .BookFinAPActCost = oData.BookFinAPActCost
                        .BookFinCommStd = oData.BookFinCommStd
                        .BookFinServiceFee = oData.BookFinServiceFee
                        .BookTotalBFC = oData.BookTotalBFC
                        .BookLockAllCosts = oData.BookLockAllCosts
                        .BookLockBFCCost = oData.BookLockBFCCost
                        .BookModDate = Date.Now()
                        .BookModUser = Me.Parameters.UserName
                    End With
                    db.SubmitChanges()
                    blnRet = True
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SavevBookRevenue"))
            End Try
        End Using
        Return blnRet
    End Function



    Public Function GetLTLvBookRevenues(ByVal iBookControl As Integer) As LTS.vBookRevenue()
        Dim oRet As LTS.vBookRevenue()
        Using operation = Logger.StartActivity("GetLTLvBookRevenues(iBookControl: {BookControl})", iBookControl)
            Using db As New NGLMasBookDataContext(ConnectionString)
                Try

                    Dim oDLO As New DataLoadOptions
                    oDLO.LoadWith(Of LTS.vBookRevenue)(Function(t As LTS.vBookRevenue) t.BookLoads)
                    oDLO.LoadWith(Of LTS.vBookRevenue)(Function(t As LTS.vBookRevenue) t.BookFees)
                    oDLO.LoadWith(Of LTS.BookLoad)(Function(t As LTS.BookLoad) t.BookItems)
                    db.LoadOptions = oDLO

                    Dim oDB = db.spGetDependentLTLBookControls(iBookControl).ToArray()
                    If Not oDB Is Nothing AndAlso oDB.Count() > 0 AndAlso oDB(0).BookControl <> 0 Then
                        oRet = (From d In db.vBookRevenues Where oDB.Select(Function(x) x.BookControl).ToList().Contains(d.BookControl) Select d).ToArray()
                    End If

                Catch ex As Exception
                    Logger.Error(ex, "Error in GetLTLvBookRevenues(iBookControl: {BookControl})", iBookControl)
                Finally
                    operation.Complete()

                End Try
            End Using
        End Using

        Return oRet

    End Function

    Public Sub LockBFCOnImport(ByVal BookControl As Integer, ByVal compcontrol As Integer)
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim dblAutoLockBFCOnImport = GetParValue("AutoLockBFCOnImport", compcontrol)
                If dblAutoLockBFCOnImport = 1 Then
                    Dim oBook = db.Books.Where(Function(x) x.BookControl = BookControl).FirstOrDefault()
                    With oBook
                        If oBook.BookTranCode <> "N" Then
                            oBook.BookLockBFCCost = True
                            db.SubmitChanges()
                        End If
                    End With
                End If
            Catch ex As Exception
                'do nothing
                'ManageLinqDataExceptions(ex, buildProcedureName("LockBFCOnImport"))
            End Try


        End Using
    End Sub


    Public Function GetBookRevenues(ByVal Control As Integer) As DataTransferObjects.BookRevenue()
        Using operation = Logger.StartActivity("GetBookRevenues(Control: {BookControl})", Control)
            Using db As New NGLMasBookDataContext(ConnectionString)
                Try
                    'db.Log = New DebugTextWriter
                    Dim oDLO As New DataLoadOptions
                    oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.CompRefBook)
                    oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookLoads)
                    oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookFees)
                    oDLO.LoadWith(Of LTS.BookLoad)(Function(t As LTS.BookLoad) t.BookItems)
                    oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.LaneRefBook)
                    db.LoadOptions = oDLO
                    Dim oDB = db.udfGetDependentBookControls(Control, False)
                    Dim oRet = (From d In db.Books Where oDB.Select(Function(x) x.BookControl).ToList().Contains(d.BookControl)
                                Select selectDTOData(d, db)).ToArray()
                    operation.Complete()
                    Return oRet
                Catch ex As Exception
                    Logger.Error(ex, "Error in GetBookRevenues(Control: {BookControl})", Control)
                Finally
                    operation.Complete()
                End Try

            End Using
        End Using

        Return Nothing


    End Function

    ''' <summary>
    ''' This overload returns a list of BookRevenue object including company details for the provided CNS 
    ''' and carrier regardless of the consolidation integrity flag; If the same CNS is assigned to a different carrier
    ''' it must be requested seperatly.  Typically used for printing shipping labels.  NOTE: this 
    ''' function limits the results to the first 1000 records.
    ''' </summary>
    ''' <param name="BookConsPrefix"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetBookRevenues(ByVal BookConsPrefix As String, ByVal CarrierControl As Integer) As List(Of BookRevenue)
        Using operation = Logger.StartActivity("GetBookRevenues(BookConsPrefix: {BookConsPrefix}, CarrierControl: {CarrierControl})", BookConsPrefix, CarrierControl)
            Using db As New NGLMasBookDataContext(ConnectionString)
                Try
                    Dim oBookRevenues As New List(Of BookRevenue)
                    If String.IsNullOrWhiteSpace(BookConsPrefix) Or CarrierControl = 0 Then Return oBookRevenues 'return an empty list
                    Dim oDLO As New DataLoadOptions
                    oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.CompRefBook)
                    oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookLoads)
                    oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookFees)
                    oDLO.LoadWith(Of LTS.BookLoad)(Function(t As LTS.BookLoad) t.BookItems)
                    oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.LaneRefBook)
                    db.LoadOptions = oDLO
                    oBookRevenues = (From d In db.Books
                                     Where d.BookConsPrefix = BookConsPrefix And d.BookCarrierControl = CarrierControl
                                     Select selectDTOData(d, db)).Take(1000).ToList() 'limited to the first 1000 record
                    operation.Complete()
                    Return oBookRevenues
                Catch ex As Exception
                    Logger.Error(ex, "Error in GetBookRevenues(BookConsPrefix: {BookConsPrefix}, CarrierControl: {CarrierControl})", BookConsPrefix, CarrierControl)
                Finally
                    operation.Complete()
                End Try
            End Using

        End Using
        Return Nothing

    End Function

    ''' <summary>
    ''' This method has been Depreciated and is no longer used
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <param name="AllocateBFC"></param>
    ''' <param name="UpdateBFC"></param>
    ''' <returns>
    ''' 
    ''' </returns>
    ''' <remarks>
    ''' Removed by RHR v-6.4 no longer supported
    ''' </remarks>
    Public Function SaveAndCalcRevenue(ByVal oData As DataTransferObjects.BookRevenue, Optional ByVal AllocateBFC As Boolean = True, Optional ByVal UpdateBFC As Boolean = True) As DataTransferObjects.BookRevenue
        Logger.Error("SaveAndCalcRevenue is no longer supported")
        throwDepreciatedException(buildProcedureName("SaveAndCalcRevenue"))
        Return Nothing
        'Me.UpdateRecordNoReturn(oData)

        'Dim strProcName As String = "dbo.spCalcBookRevBFC"
        'Dim oCmd As New System.Data.SqlClient.SqlCommand
        'oCmd.Parameters.AddWithValue("@BookControl", oData.BookControl)
        'oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        'If UpdateBFC Then
        '    oCmd.Parameters.AddWithValue("@UpdateBFC", 1)
        'End If
        ''Added by RHR 05/04/11.   
        'oCmd.Parameters.AddWithValue("@IgnoreIC", 1)
        'runNGLStoredProcedure(oCmd, strProcName, 0)
        'Return GetBookRevenueFiltered(oData.BookControl)
    End Function

    Public Function RecalculateFreightCosts(ByVal BookControl As Integer) As DataTransferObjects.BookRevenue
        Dim strProcName As String = "dbo.spRecalculateFreightCosts"
        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@BookControl", BookControl)
        oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        runNGLStoredProcedure(oCmd, strProcName, 0)
        Return GetBookRevenueFiltered(BookControl)
    End Function

    Public Sub RecalculateFreightCostsNoReturn(ByVal BookControl As Integer)
        Dim strProcName As String = "dbo.spRecalculateFreightCosts"
        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@BookControl", BookControl)
        oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        runNGLStoredProcedure(oCmd, strProcName, 0)
    End Sub

    ''' <summary>
    ''' Saves updates to a single Book Revenue Object and replaces the BookFees child data table with changes 
    ''' also Updates the Book Revenue History and revison rumber
    ''' returns the updated Book Revenue Object with the new revision number applied
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' The difference between SaveRevenueWDetails and UpdateWithDetails is:
    ''' 1. UpdateWithDetails checks expects the BookDependencyResult to be populated
    '''    where SaveRevenueWDetails will throw an exception if the update dependency fails.
    ''' 2. UpdateWithDetails only updates book fee record changes where SaveRevenueWDetails
    '''    replaces all fees and removes any that are no longer used by setting the ReplaceFees
    '''    flag to true when the flag is true the system does not run validation on the bookfee data 
    ''' 3. UpdateWithDetails uses the BookDependencyResult to determine if costs should be
    '''    recalculated; SaveRevenueWDetails expects that the costs are recalculated by the caller
    ''' 4. UpdateWithDetails only creates a history record if one or more items or fees have changed but the 
    '''    SaveRevenueWDetails will always create a history record becasue the Replace Fees flag is true 
    ''' </remarks>
    Public Function SaveRevenueWDetails(ByVal oData As DataTransferObjects.BookRevenue) As DataTransferObjects.BookRevenue
        Return SaveRevenue(oData, False, True, True)
    End Function



    ''' <summary>
    ''' Saves updates to a single Book Revenue Object and replaces the BookFees child data table with changes 
    ''' also Updates the Book Revenue History and revison rumber
    ''' returns the updated Book Revenue Object with the new revision number applied
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' The difference between SaveRevenueWDetails and UpdateWithDetails is:
    ''' UpdateWithDetails checks the TrackingInfo SaveRevenueWDetails updates
    ''' all records and does not check TrackingInfo and it does not recalculate 
    ''' costs.  it expectes the costs to have been recalculated by the caller
    ''' </remarks>
    Public Function SaveRevenue(ByVal oData As DataTransferObjects.BookRevenue, ByVal blnUpdateBookDependencyResult As Boolean, ByVal saveDetails As Boolean, ByVal replaceFees As Boolean, Optional ByVal blnReturnChanges As Boolean = True, Optional ByVal blnUpdateDependencies As Boolean = True) As DataTransferObjects.BookRevenue
        Using operation = Logger.StartActivity("SaveRevenue(oData: {BookRevenue}, blnUpdateBookDependencyResult: {UpdateBookDependencyResult}, saveDetails: {SaveDetails}, replaceFees: {ReplaceFees}, blnReturnChanges: {ReturnChanges}, blnUpdateDependencies: {UpdateDependencies})", oData, blnUpdateBookDependencyResult, saveDetails, replaceFees, blnReturnChanges, blnUpdateDependencies)




            If oData Is Nothing Then Return Nothing
            'get a copy of the current data
            Dim BookControl As Integer = oData.BookControl
            Dim oExistings As New List(Of LTS.Book)
            Dim oExisting As LTS.Book
            Dim blnSaved As Boolean = False
            Try
                Using db As New NGLMasBookDataContext(ConnectionString)
                    Dim oDLO As New DataLoadOptions
                    'oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.CompRefBook)
                    oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookLoads)
                    oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookFees)
                    oDLO.LoadWith(Of LTS.BookLoad)(Function(t As LTS.BookLoad) t.BookItems)
                    'oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.LaneRefBook)
                    db.LoadOptions = oDLO
                    Try
                        Dim intLoadBookControls = db.udfGetDependentBookControls(BookControl, False).Select(Function(x) x.BookControl).ToList()
                        If Not intLoadBookControls Is Nothing AndAlso intLoadBookControls.Count > 1 Then
                            oExistings = db.Books.Where(Function(x) intLoadBookControls.Contains(x.BookControl)).ToList()
                        End If
                        If Not oExistings Is Nothing AndAlso oExistings.Count > 0 Then
                            oExisting = oExistings.Where(Function(x) x.BookControl = BookControl).FirstOrDefault()
                        Else
                            oExisting = db.Books.Where(Function(x) x.BookControl = BookControl).FirstOrDefault()
                        End If
                        If Not oExisting Is Nothing AndAlso oExisting.BookControl <> 0 Then CheckBookDataConcurrency(oData, oExisting)
                    Catch ex As Exception
                        ManageLinqDataExceptions(ex, buildProcedureName("SaveRevenue"), db)
                    End Try
                    'Removed by RHR for testing Transaction is timming out
                    'Using trans As New System.Transactions.TransactionScope()
                    blnSaved = False
                    UpdateLTSData(db, oData, oExisting, saveDetails, replaceFees, oExistings)
                    Try
                        db.SubmitChanges()
                    Catch conflictEx As ChangeConflictException
                        Try
                            'improper reference to LinqDB 
                            'changed to db by RHR 10/12/2015
                            Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(db))
                            conflictInfo.LogConflictDetails(conflictInfo.Message, Me.Parameters)
                            Throw New FaultException(Of ConflictFaultInfo)(conflictInfo, New FaultReason("E_DataConflict"))
                        Catch ex As FaultException
                            Logger.Error(ex, "Error in SaveRevenue")
                            Throw
                        Catch ex As Exception
                            Logger.Error(ex, "Error in SaveRevenue")
                            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                        End Try
                    Catch ex As Exception
                        Logger.Error(ex, "Error in SaveRevenue")
                        Throw ex
                    End Try

                    blnSaved = True
                    '    trans.Complete()
                    'End Using
                End Using
            Catch ex As Exception
                Logger.Error(ex, "Error in SaveRevenue")

            End Try
            If blnSaved Then
                Dim oResult As LTS.spUpdateBookDependenciesResult
                If blnUpdateDependencies Then
                    'We call UpdateBookDependencies but we do not recalculate we only check for errors
                    Dim bookData As New NGLBookData(Me.Parameters)
                    oResult = bookData.UpdateBookDependencies(oData.BookControl, 0)

                End If
                If CreateHistory Then CreateBookRevenueHistory(oData.BookControl)
                If blnUpdateBookDependencyResult Then
                    Me.BookDependencyResult = oResult
                Else
                    'Check for errors in oResult
                    If Not oResult Is Nothing AndAlso If(oResult.ErrNumber, 0) <> 0 Then throwFaultException(FreightMaster.Data.SqlFaultInfo.FaultInfoMsgs.E_SQLExceptionMSG, FreightMaster.Data.SqlFaultInfo.FaultDetailsKey.E_SQLWarningDetails, New List(Of String) From {"spUpdateBookDependencies", If(oResult.ErrNumber, 0).ToString, oResult.RetMsg}, FreightMaster.Data.SqlFaultInfo.FaultReasons.E_SQLException)
                End If
            End If
            If blnReturnChanges Then
                ' Return the updated record
                Return GetBookRevenueWDetailsFiltered(oData.BookControl)
            Else
                Return Nothing
            End If
        End Using

    End Function

    ''' <summary>
    ''' Saves an array of Book Revenue Objects and the BookFees child data
    ''' The array is expected to be associated with a single load.
    ''' This method also updates the Book Revenue History
    ''' and revision number.  The updated Book Revenue Objects are returned 
    ''' with the new revision number applied
    ''' All Fees are replaces with the updated fee data.  the caller is expected to
    ''' validate the fee information.
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns>a list of BookRevenue objects with details for the entire load</returns>
    ''' <remarks>does not recalculate 
    ''' costs.  it expectes the costs to have been recalculated by the caller
    ''' Modified by LVV 5/16/16 for v-7.0.5.110 DAT
    ''' Added optional param for call to ResetToNStatus
    ''' </remarks>
    Public Function SaveRevenuesWDetails(ByVal oData As DataTransferObjects.BookRevenue(), Optional ByVal OptimisticConcurrencyOn As Boolean = True, Optional ByVal blnResetToNStatus As Boolean = False) As DataTransferObjects.BookRevenue()
        Dim oResult As LTS.spUpdateBookDependenciesResult
        Using operation = Logger.StartActivity("SaveRevenuesWDetails(oData: {BookRevenue}, OptimisticConcurrencyOn: {OptimisticConcurrencyOn}, blnResetToNStatus: {ResetToNStatus})", oData, OptimisticConcurrencyOn, blnResetToNStatus)
            If oData Is Nothing OrElse oData.Count < 1 Then Return Nothing
            'get a copy of the current data
            Dim BookControl As Integer = 0
            Dim oExistings As New List(Of LTS.Book)
            Dim blnSaved As Boolean = False
            Try
                Using db As New NGLMasBookDataContext(ConnectionString)
                    Dim oDLO As New DataLoadOptions
                    'oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.CompRefBook)
                    oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookLoads)
                    oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookFees)
                    oDLO.LoadWith(Of LTS.BookLoad)(Function(t As LTS.BookLoad) t.BookItems)
                    'oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.LaneRefBook)
                    db.LoadOptions = oDLO
                    Try
                        For Each oItem In oData
                            BookControl = oItem.BookControl
                            Dim oBookLTS = db.Books.Where(Function(x) x.BookControl = BookControl).FirstOrDefault()
                            If Not oBookLTS Is Nothing AndAlso oBookLTS.BookControl <> 0 Then
                                oExistings.Add(oBookLTS)
                                If OptimisticConcurrencyOn Then CheckBookDataConcurrency(oItem, oBookLTS)
                            End If
                        Next
                    Catch ex As Exception
                        ManageLinqDataExceptions(ex, buildProcedureName("SaveRevenuesWDetails"), db)
                    End Try
                    Try
                        Using trans As New System.Transactions.TransactionScope()
                            For Each oItem In oData
                                blnSaved = False
                                'Modified by LVV 5/16/16 for v-7.0.5.110 DAT
                                If blnResetToNStatus Then
                                    oItem.ResetToNStatus()
                                End If
                                BookControl = oItem.BookControl
                                Dim oLTs = oExistings.Where(Function(x) x.BookControl = BookControl).FirstOrDefault()
                                UpdateLTSData(db, oItem, oLTs, True, True, oExistings)
                            Next
                            db.SubmitChanges()
                            blnSaved = True
                            trans.Complete()
                        End Using
                    Catch conflictEx As ChangeConflictException
                        Try
                            'improper reference to LinqDB 
                            'changed to db by RHR 10/12/2015
                            Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(db))
                            conflictInfo.LogConflictDetails(conflictInfo.Message, Me.Parameters)
                            Throw New FaultException(Of ConflictFaultInfo)(conflictInfo, New FaultReason("E_DataConflict"))
                        Catch ex As FaultException
                            Throw
                        Catch ex As Exception
                            Utilities.SaveAppError(ex.Message, Me.Parameters)
                            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                        End Try
                    Catch ex As Exception
                        Throw ex
                    End Try

                End Using
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveRevenuesWDetails"))
            End Try
            If blnSaved Then
                For Each oItem In oData
                    'We call UpdateBookDependencies but we do not recalculate we only check for errors
                    oResult = DirectCast(Me.NDPBaseClassFactory("NGLBookData", False), NGLBookData).UpdateBookDependencies(oItem.BookControl, 0)
                Next
                If CreateHistory Then CreateBookRevenueHistory(oData(0).BookControl)
                'Check for errors in oResult and stop processing on error
                If Not oResult Is Nothing AndAlso If(oResult.ErrNumber, 0) <> 0 Then
                    throwFaultException(FreightMaster.Data.SqlFaultInfo.FaultInfoMsgs.E_SQLExceptionMSG, FreightMaster.Data.SqlFaultInfo.FaultDetailsKey.E_SQLWarningDetails, New List(Of String) From {"spUpdateBookDependencies", If(oResult.ErrNumber, 0).ToString, oResult.RetMsg}, FreightMaster.Data.SqlFaultInfo.FaultReasons.E_SQLException)
                End If
            End If
            'Return the updated records
            Return GetBookRevenuesWDetailsFiltered(oData(0).BookControl)
        End Using
    End Function

    ''' <summary>
    ''' The array is expected to be associated with a single load.
    ''' This method also updates the Book Revenue History
    ''' and revision number.  
    ''' All Fees are replaces with the updated fee data.  the caller is expected to
    ''' validate the fee information.
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <param name="saveDetails"></param>
    ''' <param name="blnUpdateDependencies"></param>
    ''' <remarks></remarks>
    Public Sub SaveRevenuesNoReturn(ByVal BookControl As Integer, ByVal iCarrierControl As Integer, ByVal iBookCarrTarEquipMatControl As Integer, ByVal iBookCarrTarEquipControl As Integer,
                                    Optional ByVal saveDetails As Boolean = True, Optional ByVal blnUpdateDependencies As Boolean = True, Optional ByVal OptimisticConcurrencyOn As Boolean = True)
        Dim oResult As LTS.spUpdateBookDependenciesResult

        'get a copy of the current data
        Dim oExistings As New List(Of LTS.Book)
        Dim blnSaved As Boolean = False
        Dim oData As DataTransferObjects.BookRevenue()
        Try

            Using db As New NGLMasBookDataContext(ConnectionString)
                Dim oDLO As New DataLoadOptions
                'oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.CompRefBook)
                oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookLoads)
                oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookFees)
                oDLO.LoadWith(Of LTS.BookLoad)(Function(t As LTS.BookLoad) t.BookItems)
                'oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.LaneRefBook)
                db.LoadOptions = oDLO
                Try
                    oData = (From d In db.Books
                             Join x In db.udfGetDependentBookControls(BookControl, False)
                                 On d.BookControl Equals x.BookControl
                             Select selectDTODataWDetails(d, db)).ToArray()

                Catch ex As Exception
                    ManageLinqDataExceptions(ex, buildProcedureName("SaveRevenuesNoReturn"), db)
                End Try
                Try
                    If (Not oData Is Nothing AndAlso oData.Count() > 0) Then
                        For Each b In oData
                            b.BookCarrierControl = iCarrierControl
                            b.BookCarrTarEquipMatControl = iBookCarrTarEquipMatControl
                            b.BookCarrTarEquipControl = iBookCarrTarEquipControl
                        Next
                        db.SubmitChanges()
                        blnSaved = True
                    End If


                Catch conflictEx As ChangeConflictException
                    Try
                        'improper reference to LinqDB 
                        'changed to db by RHR 10/12/2015
                        Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(db))
                        conflictInfo.LogConflictDetails(conflictInfo.Message, Me.Parameters)
                        Throw New FaultException(Of ConflictFaultInfo)(conflictInfo, New FaultReason("E_DataConflict"))
                    Catch ex As FaultException
                        Throw
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                Catch ex As SqlException
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                Catch ex As Exception
                    Throw ex
                End Try

            End Using
        Catch ex As Exception
            ManageLinqDataExceptions(ex, buildProcedureName("SaveRevenuesNoReturn"))
        End Try
        If blnSaved Then
            If blnUpdateDependencies Then
                For Each d In oData
                    'We call UpdateBookDependencies but we do not recalculate we only check for errors
                    oResult = DirectCast(Me.NDPBaseClassFactory("NGLBookData", False), NGLBookData).UpdateBookDependencies(d.BookControl, 0)
                Next
            End If
            If CreateHistory Then CreateBookRevenueHistory(oData(0).BookControl)
            'Check for errors in oResult and stop processing on error
            If Not oResult Is Nothing AndAlso If(oResult.ErrNumber, 0) <> 0 Then
                throwFaultException(FreightMaster.Data.SqlFaultInfo.FaultInfoMsgs.E_SQLExceptionMSG, FreightMaster.Data.SqlFaultInfo.FaultDetailsKey.E_SQLWarningDetails, New List(Of String) From {"spUpdateBookDependencies", If(oResult.ErrNumber, 0).ToString, oResult.RetMsg}, FreightMaster.Data.SqlFaultInfo.FaultReasons.E_SQLException)
            End If
        End If
    End Sub

    Public Sub SaveRevenuesNoReturn(ByVal oData As DataTransferObjects.BookRevenue(), Optional ByVal saveDetails As Boolean = True, Optional ByVal blnUpdateDependencies As Boolean = True, Optional ByVal OptimisticConcurrencyOn As Boolean = True)

        Using operation = Logger.StartActivity("SaveRevenuesNoReturn oData:{@oData} saveDetails:{SaveDetails} blnUpdateDependencies:{blnUpdateDependencies} OptConc:{OptConc}", oData, saveDetails, blnUpdateDependencies, OptimisticConcurrencyOn)
            Dim oResult As LTS.spUpdateBookDependenciesResult
            If oData Is Nothing OrElse oData.Count < 1 Then Return
            'get a copy of the current data
            Dim BookControl As Integer = 0
            Dim oExistings As New List(Of LTS.Book)
            Dim blnSaved As Boolean = False

            Try

                Using db As New NGLMasBookDataContext(ConnectionString)
                    Dim oDLO As New DataLoadOptions
                    'oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.CompRefBook)
                    oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookLoads)
                    oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.BookFees)
                    oDLO.LoadWith(Of LTS.BookLoad)(Function(t As LTS.BookLoad) t.BookItems)
                    'oDLO.LoadWith(Of LTS.Book)(Function(t As LTS.Book) t.LaneRefBook)
                    db.LoadOptions = oDLO
                    Try
                        For Each b In oData
                            BookControl = b.BookControl
                            Dim oBookLTS = db.Books.Where(Function(x) x.BookControl = BookControl).FirstOrDefault()
                            If Not oBookLTS Is Nothing AndAlso oBookLTS.BookControl <> 0 Then
                                oExistings.Add(oBookLTS)
                                If OptimisticConcurrencyOn Then CheckBookDataConcurrency(b, oBookLTS)
                            End If
                        Next
                    Catch ex As Exception
                        ManageLinqDataExceptions(ex, buildProcedureName("SaveRevenuesNoReturn"), db)
                    End Try
                    Try
                        Using trans As New System.Transactions.TransactionScope()
                            For Each b In oData
                                blnSaved = False
                                BookControl = b.BookControl
                                Dim oLTs = oExistings.Where(Function(x) x.BookControl = BookControl).FirstOrDefault()
                                UpdateLTSData(db, b, oLTs, saveDetails, True, oExistings)
                            Next
                            db.SubmitChanges()
                            blnSaved = True
                            trans.Complete()
                        End Using
                    Catch conflictEx As ChangeConflictException
                        Try
                            'improper reference to LinqDB 
                            'changed to db by RHR 10/12/2015
                            Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(db))
                            conflictInfo.LogConflictDetails(conflictInfo.Message, Me.Parameters)
                        Catch ex As FaultException
                            Logger.Error(ex, "NGLBookRevenueData.SaveRevenuesNoReturn")
                        Catch ex As Exception
                            Logger.Error(ex, "NGLBookRevenueData.SaveRevenuesNoReturn")
                            'Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                        End Try
                    Catch ex As SqlException
                        Logger.Error(ex, "NGLBookRevenueData.SaveRevenuesNoReturn")
                        'Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As Exception
                        Logger.Error(ex, "NGLBookRevenueData.SaveRevenuesNoReturn")
                    End Try

                End Using
            Catch ex As Exception
                operation.Complete(LogEventLevel.Error, ex)
                ManageLinqDataExceptions(ex, buildProcedureName("SaveRevenuesNoReturn"))
            End Try
            If blnSaved Then
                If blnUpdateDependencies Then
                    For Each d In oData
                        'We call UpdateBookDependencies but we do not recalculate we only check for errors
                        Dim bookData As New NGLBookData(Me.Parameters)
                        oResult = bookData.UpdateBookDependencies(d.BookControl, 0)
                    Next
                End If
                If CreateHistory Then CreateBookRevenueHistory(oData(0).BookControl)
                'Check for errors in oResult and stop processing on error
                If Not oResult Is Nothing AndAlso If(oResult.ErrNumber, 0) <> 0 Then
                    throwFaultException(FreightMaster.Data.SqlFaultInfo.FaultInfoMsgs.E_SQLExceptionMSG, FreightMaster.Data.SqlFaultInfo.FaultDetailsKey.E_SQLWarningDetails, New List(Of String) From {"spUpdateBookDependencies", If(oResult.ErrNumber, 0).ToString, oResult.RetMsg}, FreightMaster.Data.SqlFaultInfo.FaultReasons.E_SQLException)
                End If
            End If
        End Using

    End Sub

    Public Sub UpdateWithDependencies(ByVal source As DataTransferObjects.BookRevenue)

        If source Is Nothing Then Return
        SaveRevenue(source, True, False, False, False)
        Me.LastProcedureName = "spUpdateBookDependencies"
        Return
    End Sub

    ''' <summary>
    ''' This override does not call the standard cleanup methods and returns nothing.
    ''' The caller must check the BookDependencyResult properties (ReturnBookControl,MustRecalculate,RetMsg, and ErrNumber) 
    ''' , recalculate costs and return the updated bookrevenue record
    ''' </summary>
    ''' <typeparam name="TEntity"></typeparam>
    ''' <param name="oData"></param>
    ''' <param name="oLinqTable"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function Update(Of TEntity As Class)(ByVal oData As Object,
                                                          ByVal oLinqTable As System.Data.Linq.Table(Of TEntity)) As Object
        Dim source As DataTransferObjects.BookRevenue = TryCast(oData, DataTransferObjects.BookRevenue)
        If source Is Nothing Then Return Nothing
        SaveRevenue(source, True, False, False, False)
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
        Dim source As DataTransferObjects.BookRevenue = TryCast(oData, DataTransferObjects.BookRevenue)
        If source Is Nothing Then Return
        SaveRevenue(source, True, False, False, False)
        Me.LastProcedureName = "spUpdateBookDependencies"
        Return
    End Sub

    Public Overrides Function UpdateWithDetails(Of TEntity As Class)(ByVal oData As Object,
                                                                     ByVal oLinqTable As System.Data.Linq.Table(Of TEntity)) As Object
        Dim source As DataTransferObjects.BookRevenue = TryCast(oData, DataTransferObjects.BookRevenue)
        If source Is Nothing Then Return Nothing
        SaveRevenue(source, True, True, False, False)
        Me.LastProcedureName = "spUpdateBookDependencies"
        Return Nothing
    End Function

    Public Overrides Sub UpdateWithDetailsNoReturn(Of TEntity As Class)(ByVal oData As Object,
                                                                        ByVal oLinqTable As System.Data.Linq.Table(Of TEntity))
        Dim source As DataTransferObjects.BookRevenue = TryCast(oData, DataTransferObjects.BookRevenue)
        If source Is Nothing Then Return
        SaveRevenue(source, True, True, False, False)
        Me.LastProcedureName = "spUpdateBookDependencies"
    End Sub

    ''' <summary>
    ''' Creates a copy of the book revenue data and generates a new sequence number
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.2.0.116 7/15/19 
    '''   added optional blnExpectedCost flag  this flag is 
    '''   only true the first time a freight bill is recieved
    ''' </remarks>
    Public Function CreateBookRevenueHistory(ByVal BookControl As Integer, Optional ByVal blnExpectedCost As Boolean = False) As Boolean
        Using operation = Logger.StartActivity("CreateBookRevenueHistory(BookControl: {BookControl}, blnExpectedCost: {ExpectedCost})", BookControl, blnExpectedCost)
            Dim strProcName As String = "dbo.spCreateBookRevenueHistory"
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@BookControl", BookControl)
            If blnExpectedCost Then
                oCmd.Parameters.AddWithValue("@ExpectedCost", 1)
            Else
                oCmd.Parameters.AddWithValue("@ExpectedCost", 0)
            End If
            oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
            runNGLStoredProcedure(oCmd, strProcName, 0)
        End Using
        Return True
    End Function

    ''' <summary>
    ''' Updates the Stop, Pick and Fuel fees for each order in BookRevsData
    ''' Returns the sum of each for the load.
    ''' The caller must determine which fees are marked as overridden
    ''' The method only selectes fees where BookFeesOverRidden = False
    ''' </summary>
    ''' <param name="BookRevsData"></param>
    ''' <param name="TotalStopCharges"></param>
    ''' <param name="TotalPickCharges"></param>
    ''' <param name="TotalFuelCharges"></param>
    ''' <param name="CarrierCost"></param>
    ''' <param name="CarrierControl"></param>
    ''' <param name="Taxable"></param>
    ''' <param name="CarrTarControl"></param>
    ''' <param name="Message"></param>
    ''' <remarks></remarks>
    Public Sub getLegacyStopPickandFuelChargesForLoad(ByRef BookRevsData As DataTransferObjects.BookRevenue(),
                                                      ByRef TotalStopCharges As Decimal,
                                                      ByRef TotalPickCharges As Decimal,
                                                      ByRef TotalFuelCharges As Decimal,
                                                      ByVal CarrierCost As Decimal,
                                                      ByVal CarrierControl As Integer,
                                                      ByVal Taxable As Boolean,
                                                      ByVal CarrTarControl As Integer,
                                                      ByVal CarrTarEquipControl As Integer,
                                                      Optional ByRef Message As String = "")

        Using operation = Logger.StartActivity("NGLBookRevenueData.getLegacyStopPickandFuelChargesForLoad(BookRevsData: {BookRevsData}, TotalStopCharges: {TotalStopCharges}, TotalPickCharges: {TotalPickCharges}, TotalFuelCharges: {TotalFuelCharges}, CarrierCost: {CarrierCost}, CarrierControl: {CarrierControl}, Taxable: {Taxable}, CarrTarControl: {CarrTarControl}, CarrTarEquipControl: {CarrTarEquipControl}, Message: {Message})", BookRevsData, TotalStopCharges, TotalPickCharges, TotalFuelCharges, CarrierCost, CarrierControl, Taxable, CarrTarControl, CarrTarEquipControl, Message)


            TotalStopCharges = calculateStopCharge(BookRevsData, CarrierCost, Taxable, Message)
            Logger.Information("Legacy Total Stop Charges:{TotalStopCharges}", TotalStopCharges)
            TotalPickCharges = calculatePickCharge(BookRevsData, CarrierCost, Taxable, Message)
            Logger.Information("TotalPickCharges:{TotalPickCharges}", TotalPickCharges)
            TotalFuelCharges = calculateFuelCharge(BookRevsData, CarrierCost, CarrierControl, Taxable, CarrTarControl, CarrTarEquipControl, Message)
            Logger.Information("TotalFuelCharges:{TotalFuelCharges} for Carrier: {CarrierControl}", TotalFuelCharges, CarrierControl)
        End Using

    End Sub

    ''' <summary>
    ''' Run before costs are calculated, processes missing dependency fees, normalizes allocated fees, checks for missisng allocated fees, and updates the calculation type when needed.
    ''' </summary>
    ''' <param name="BookRevs"></param>
    ''' <param name="PhysicalDeleteMissing"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR v-7.0.5.102 8/11/2016
    '''   added logic to allow manual updates to Order specific fees when the allocation type is Load (or other)
    '''   bypassing the default behavior if the last fee modified was changed in the last 180 seconds (3 minutes)
    '''   Modified Business rules:
    '''   1) if the modified order is set to Allocation  NONE (1) set any fees with the same Accessorial Code as Load (4) to NONE (1)
    '''   2) if the modified order is set to Origin (2) update any fees with the same Accessorial Code and the same Origin to use the same exact settings
    '''   3) if the modified order is set to Destination (3) update any fees with the same Accessorial Code and the same Destination to use the same exact settings 
    '''   4) if the modified order is set to Load (4) update all fees with the same Accessorial Code to use the same exact settings
    ''' Modified by RHR for v-7.0.5.103 on 01/24/2017
    '''  fixed issue where new fees are not being processed because the test check for BookFeesControl
    '''  instead we now test for BookFeesBookControl to be sure the record is not null
    '''  
    ''' </remarks>
    Public Function AccessorialFeeValidation(ByRef BookRevs As List(Of BookRevenue), Optional ByVal PhysicalDeleteMissing As Boolean = True) As Boolean
        Try
            Using Logger.StartActivity("NGLBookRevenueData.AccessorialFeeValidation {BookRevs} - PhysicalDeleteMissing:{PhysicalDeleteMissing}", BookRevs, PhysicalDeleteMissing)

                UpdateOverridesForAccessorialCode(BookRevs)
                ReverseMissingOverrides(BookRevs)
                ProcessMissingRouteDependencies(BookRevs, PhysicalDeleteMissing)
                ProcessMissingLaneDependencies(BookRevs, PhysicalDeleteMissing)
                ProcessMissingOrderDependencies(BookRevs, PhysicalDeleteMissing)

                'get a list of active accessorial codes assigned to this load
                Dim ActiveCodes As List(Of Integer) = (From d In BookRevs From f In d.BookFees Where f.BookFeesOverRidden = False Select f.BookFeesAccessorialCode).Distinct().ToList()

                Logger.Information("NGLBookRevenueData.AccessorialFeeValidation - ActiveCodes:{@0}", ActiveCodes)

                If Not ActiveCodes Is Nothing AndAlso ActiveCodes.Count > 0 Then
                    'Modified by RHR 9/16/14 we now Normalize all allocation types but only Insert Missing where 
                    'the FeeAllocationType is not equal to None  
                    'loop through each code and select the different allocation types that are not None 
                    For Each code In ActiveCodes

                        'If code = 15 Then
                        '    code = 2
                        '    Logger.Information("NGLBookRevenueData.AccessorialFeeValidation - code = 15 so setting it 2")
                        'End If

                        Logger.Information("Querying BookRevs for Fees where AccessorialCode={code} and BookFeesOverridden=False", code)
                        'And f.BookFeesAccessorialFeeAllocationTypeControl <> Utilities.FeeAllocationType.None _
                        Dim AllocationTypes As List(Of Integer) = (From d In BookRevs
                                                                   From f In d.BookFees
                                                                   Where f.BookFeesAccessorialCode = code _
                                                                         And f.BookFeesOverRidden = False
                                                                   Select f.BookFeesAccessorialFeeAllocationTypeControl).Distinct().ToList()


                        Logger.Information("Checking if Allocation Types Count ({AllocationTypesCount}) > 0", AllocationTypes?.Count)
                        If Not AllocationTypes Is Nothing AndAlso AllocationTypes.Count > 0 Then
                            'Modified by RHR v-7.0.5.102 8/11/2016
                            'We have more than one Allocation Type so check if we have a newly modified fee,  
                            'any new fee manually changed takes precedence if this is an order specific fee (AccessorialFeeTypeControl == 3)
                            'changed check if a matching record has been modified in the last 3 minutes
                            'if so we use this record as the key and we apply new business rules (see above)

                            Logger.Information("We have more than one Allocation Type so check if we have a newly modified fee, any new fee manually changed takes precedence if this is an order specific fee (AccessorialFeeTypeControl == 3), changed check if a matching record has been modified in the last 3 minutes")

                            Dim dtCompare As Date = Date.Now()
                            If System.Diagnostics.Debugger.IsAttached Then
                                dtCompare = dtCompare.AddMinutes(-5)
                            Else
                                dtCompare = dtCompare.AddSeconds(-181)
                            End If


                            'If code = 15 Then code = 2

                            Logger.Information("Looking for BookFees matching accessorical code ({code}), FeeTypeControl = 3 and a modification Date > {ModificationDate} while sorting results by BookFeesModDate descing and BookTotalWgt", code, dtCompare)

                            Dim modFee As DataTransferObjects.BookFee = (From d In BookRevs
                                                                         From f In d.BookFees
                                                                         Where f.BookFeesAccessorialCode = code _
                                                                               And f.BookFeesAccessorialFeeTypeControl = 3 _
                                                                               And f.BookFeesModDate > dtCompare
                                                                         Order By f.BookFeesModDate Descending, d.BookTotalWgt Descending
                                                                         Select f).FirstOrDefault()
                            'Modified by RHR for v-7.0.5.103 on 01/24/2017 
                            If Not modFee Is Nothing AndAlso modFee.BookFeesBookControl <> 0 Then

                                Logger.Information("Found BookFee matching accessorical code  ({code}) FeeTypeControl = 3.. Normalizing Fees", code)
                                NormalizeAllocatedFees(modFee, BookRevs, True)

                                Logger.Information("Checking if modFee.BookFeesAccessorialFeeAllocationTypeControl ({BookFeesAccessorialFeeAllocationTypeControl}) <> {FeeAllocationType}", modFee.BookFeesAccessorialFeeAllocationTypeControl, Utilities.FeeAllocationType.None)

                                If modFee.BookFeesAccessorialFeeAllocationTypeControl <> Utilities.FeeAllocationType.None Then

                                    InsertMissingAllocatedFees(modFee, BookRevs)
                                End If

                            Else
                                'run the normal allocation process
                                'If the Allocation Type is not “None” and the allocated fees do not have the same 
                                'allocation or calculation configuration,excluding BookFeesAccessorialFeeCalcTypeControl,
                                'we run the Normalize Allocated Fees procedure.
                                For Each aType In AllocationTypes
                                    Logger.Information("Searching for Max Fee from  BookFeesAccessorialCode: {BookFeesAccessorialCode} AllocationTypes:{AllocationTypes}", code, aType)
                                    Dim maxFee As DataTransferObjects.BookFee = (From d In BookRevs
                                                                                 From f In d.BookFees
                                                                                 Where f.BookFeesAccessorialCode = code _
                                                                                       And f.BookFeesAccessorialFeeAllocationTypeControl = aType
                                                                                 Order By f.BookFeesAccessorialFeeTypeControl Descending, d.BookTotalWgt Descending
                                                                                 Select f).FirstOrDefault()

                                    Logger.Information("Checking if maxFee is nothing ({MaxFeeIsNothing}) AndAlso maxFee.BookFeesBookControl ({MaxFeeBookControl}) <> 0 ",
                                                       (maxFee Is Nothing),
                                                       maxFee?.BookFeesBookControl)

                                    If Not maxFee Is Nothing AndAlso maxFee.BookFeesBookControl <> 0 Then

                                        Logger.Information("Normalizing Fees with maxFee {@maxFee}", maxFee)
                                        NormalizeAllocatedFees(maxFee, BookRevs)

                                        Logger.Information("Checking if maxFee.BookFeesAccessorialFeeAllocationTypeControl ({BookFeesAccessorialFeeAllocationTypeControl}) <> {FeeAllocationType}", maxFee.BookFeesAccessorialFeeAllocationTypeControl, Utilities.FeeAllocationType.None)

                                        If maxFee.BookFeesAccessorialFeeAllocationTypeControl <> Utilities.FeeAllocationType.None Then
                                            Logger.Information("If maxFee AllocationTypeControl ({0}) isn't = to None (1) Inserting Missing Fees", maxFee.BookFeesAccessorialFeeAllocationTypeControl)
                                            InsertMissingAllocatedFees(maxFee, BookRevs)
                                        End If

                                    End If
                                Next
                            End If

                        End If
                    Next
                    Logger.Information("NGLBookRevenueData.AccessorialFeeValidation - Normalizing FeeCalcTypes")
                    NormalizeFeeCalcTypes(BookRevs)
                End If
            End Using
            Return True
        Catch ex As Exception
            Logger.Error(ex, "NGLBookRevenueData.AccessorialFeeValidation")
            Throw 'additional exception handling will be added during unit testing.
        End Try


    End Function

    Public Function GetDependentBookControls(ByVal BookControl As Integer, Optional ByVal bnlIncludeLTLPool As Boolean = False) As List(Of LTS.udfGetDependentBookControlsResult)
        Using Logger.StartActivity("NGLBookRevenueData.GetDependentBookControls {BookControl} - IncludeLTLPool:{IncludeLTLPool}", BookControl, bnlIncludeLTLPool)
            Using db As New NGLMasBookDataContext(ConnectionString)

                Try
                    Return db.udfGetDependentBookControls(BookControl, bnlIncludeLTLPool).ToList()

                Catch ex As Exception
                    ManageLinqDataExceptions(ex, buildProcedureName("GetDependentBookControls"))
                End Try
            End Using
            Return Nothing
        End Using

    End Function

    Public Function DoesBookSHIDExist(ByVal strBookSHID As String) As Boolean
        Using Logger.StartActivity("NGLBookRevenueData.DoesBookSHIDExist {strBookSHID}", strBookSHID)
            Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim intExists = db.spDoesBookSHIDExist(Left(strBookSHID, 50))
                If intExists = 1 Then
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DoesBookSHIDExit"))
            End Try
            Return False
        End Using
            End Using

    End Function

    ''' <summary>
    ''' This is where we call the udf when it is complete -- for now either return Manual or DAT
    ''' </summary>
    ''' <param name="blnIsDAT"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added by LVV 5/17/16 for v-7.0.5.110 DAT
    ''' </remarks>
    Public Function udfGetLoadTenderType(ByRef oBookRevs As DataTransferObjects.BookRevenue(), ByVal blnIsDAT As Boolean) As Integer
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim loadTenderTypeControl As Integer
                'This is where we call the udf when it is complete -- for now either return Manual or DAT
                'Dim BookRevTotalCost As DTO.BookRevTotalCost = ( _
                'From d In db.udfCalcTotalCost(BookControl) _
                'Select New DTO.BookRevTotalCost With {.TotalCost = If(d.TotalCost.HasValue, d.TotalCost, 0), .NetCost = If(d.NetCost.HasValue, d.NetCost, 0), .FreightTax = If(d.FreightTax.HasValue, d.FreightTax, 0)}).First
                If blnIsDAT Then
                    loadTenderTypeControl = DataTransferObjects.tblLoadTender.LoadTenderTypeEnum.DAT
                Else
                    loadTenderTypeControl = DataTransferObjects.tblLoadTender.LoadTenderTypeEnum.Manual
                End If

                For Each b In oBookRevs
                    b.BookRevLoadTenderTypeControl = loadTenderTypeControl
                Next

                Return loadTenderTypeControl

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("udfGetLoadTenderType"))
            End Try

            Return Nothing

        End Using
    End Function

  

    ''' <summary>
    ''' Gets the value of BookRevLoadTenderStatusCode from the Book table for the provided BookControl
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added by LVV 7/20/16 for v-7.0.5.110 DAT
    ''' </remarks>
    Public Function GetLTStatusCode(ByVal BookControl As Integer) As Integer
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim sc As Integer = (
                        From d In db.Books
                        Where
                        d.BookControl = BookControl
                        Select d.BookRevLoadTenderStatusCode).FirstOrDefault()

                Return sc

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLTStatusCode"))
            End Try

            Return Nothing

        End Using
    End Function


    ''' <summary>
    ''' Gets the value of BookRevLoadTenderTypeControl from the Book
    ''' table for the provided BookControl
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added by LVV on 1/13/17 for v-8.0 Next Stop
    ''' </remarks>
    Public Function GetBookLTTypeControl(ByVal BookControl As Integer) As Integer
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim sc As Integer = (
                        From d In db.Books
                        Where
                        d.BookControl = BookControl
                        Select d.BookRevLoadTenderTypeControl).FirstOrDefault()

                Return sc

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetBookLTTypeControl"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    '''
    ''' GetDATData
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added by LVV 5/18/16 for v-7.0.5.110 DAT
    ''' </remarks>
    Public Function GetDATData(ByVal BookControl As Integer, ByVal BookSHID As String) As DataTransferObjects.WCFResults
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim wcfRet As New DataTransferObjects.WCFResults

                Dim ltsRes = (From d In db.spGetDATData(BookControl, BookSHID) Select d).FirstOrDefault()

                If Not ltsRes Is Nothing Then
                    If ltsRes.LoadTenderControl <> 0 AndAlso Not ltsRes.LoadTenderControl Is Nothing Then
                        wcfRet.updateKeyFields("LoadTenderControl", ltsRes.LoadTenderControl)
                        wcfRet.Success = True
                    End If

                    If ltsRes.ErrNumber <> 0 Then
                        Dim p As New List(Of String)
                        If ltsRes.ErrNumber = 1 Then
                            p.Add(ltsRes.ParamName)
                            p.Add(ltsRes.ParamValue)
                            wcfRet.AddMessage(DataTransferObjects.WCFResults.MessageType.Warnings, ltsRes.ErrKey, p.ToArray())
                        End If
                        If ltsRes.ErrNumber = 3 Then
                            wcfRet.AddMessage(DataTransferObjects.WCFResults.MessageType.Warnings, ltsRes.ErrKey)
                        End If
                        wcfRet.Success = False
                    End If

                    Return wcfRet
                End If

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetDATData"))
            End Try

            Return Nothing
        End Using
    End Function

    Public Sub updateBookLoadTenderInfo(ByVal BookControl As Integer, ByVal UserName As String, Optional ByVal LTTypeControl As Integer = 99999, Optional ByVal LTStatusCode As Integer = 99999)
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Try
                    Dim oLTS = (From d In db.Books Where d.BookControl = BookControl).FirstOrDefault()
                    If Not oLTS Is Nothing AndAlso oLTS.BookControl > 0 Then
                        With oLTS
                            If LTStatusCode <> 99999 Then
                                .BookRevLoadTenderStatusCode = LTStatusCode
                            End If
                            If LTTypeControl <> 99999 Then
                                .BookRevLoadTenderTypeControl = LTTypeControl
                            End If
                            .BookModUser = UserName
                            .BookModDate = Date.Now
                        End With

                        db.SubmitChanges()
                    End If
                Catch ex As Exception
                    'Ignore errors when updating
                End Try

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("updateBookLoadTenderInfo"))
            End Try

        End Using
    End Sub

    ''' <summary>
    ''' Saves the Book Revenue data, updates dependencies and prepares the fees for spot rates by 
    ''' changing the fee type to order fee for all fees that have not be marked as overridden.
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <remarks>
    ''' Created by RHR v-7.0.5.100 05/12/2016
    ''' </remarks>
    Public Sub UpdateBeforeSpotRate(ByVal oData As DataTransferObjects.BookRevenue)

        If oData Is Nothing OrElse oData.BookControl = 0 Then Return
        Dim sSHID As String = oData.BookSHID
        SaveRevenue(oData, True, True, True, False)
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

    ''' <summary>
    ''' Gets the LaneBenchMiles from the LaneRefBooks tables based on BookODControl
    ''' </summary>
    ''' <param name="BookODControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 10/20/16 for v-7.0.5.110 Task #35 Accept/Reject Fixes
    '''  Modified by RHR for v-7.0.6.105 on 10/19/2017
    '''   fixed null reference exception bug where LaneBenchMiles could be null but return double does not support null
    '''</remarks>
    Public Function GetLaneBenchMilesByBookODControl(ByVal BookODControl As Integer) As Double
        Using Logger.StartActivity("NGLBookRevenueData.GetLaneBenchMilesByBookODControl(BookODControl: {BookODControl})", BookODControl)
            Using db As New NGLMasBookDataContext(ConnectionString)
                Try
                    'Get the newest record that matches the provided criteria
                    Dim miles = (
                            From d In db.LaneRefBooks
                            Where d.LaneControl = BookODControl
                            Select d.LaneBenchMiles).FirstOrDefault()

                    'Modified by RHR for v-7.0.6.105 on 10/19/2017
                    Return If(miles, 0)

                Catch ex As Exception
                    Logger.Error(ex, "NGLBookRevenueData.GetLaneBenchMilesByBookODControl")
                    ManageLinqDataExceptions(ex, buildProcedureName("GetLaneBenchMilesByBookODControl"))
                End Try
                Return Nothing
            End Using
        End Using

    End Function

    Public Function GetLoadBoardRevFees(ByVal BookControl As Integer) As Models.vCMLoadBoardRevTemplate()
        Dim oRet As New List(Of Models.vCMLoadBoardRevTemplate)
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oResults = db.spGetLoadBoardRevFees(BookControl).ToArray()
                If Not oResults Is Nothing AndAlso oResults.Count > 0 Then
                    Dim skipObjs As New List(Of String)
                    For Each d In oResults
                        Dim oData As New Models.vCMLoadBoardRevTemplate
                        oData = CopyMatchingFields(oData, d, skipObjs)
                        oData.Code = d.EDICode
                        oRet.Add(oData)
                    Next
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLoadBoardRevFees"), db)
            End Try
        End Using
        Return oRet.ToArray()
    End Function

    Public Function GetLoadBoardRevCharges(ByVal BookControl As Integer) As Models.vCMLoadBoardRevTemplate()
        Dim oRet As New List(Of Models.vCMLoadBoardRevTemplate)
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oResults = db.spGetLoadBoardRevCharges(BookControl).ToArray()
                If Not oResults Is Nothing AndAlso oResults.Count > 0 Then
                    Dim skipObjs As New List(Of String) From {"Code", "EDICode"}
                    For Each d In oResults
                        Dim oData As New Models.vCMLoadBoardRevTemplate
                        oData = CopyMatchingFields(oData, d, skipObjs)
                        oData.Code = d.EDICode
                        oRet.Add(oData)
                    Next
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLoadBoardRevCharges"), db)
            End Try
        End Using
        Return oRet.ToArray()
    End Function

    Public Function GetLoadBoardRevCosts(ByVal BookControl As Integer) As Models.vCMLoadBoardRevTemplate()
        Dim oRet As New List(Of Models.vCMLoadBoardRevTemplate)
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oResults = db.spGetLoadBoardRevCosts(BookControl).ToArray()
                If Not oResults Is Nothing AndAlso oResults.Count > 0 Then
                    Dim skipObjs As New List(Of String) From {"Code", "EDICode"}
                    For Each d In oResults
                        Dim oData As New Models.vCMLoadBoardRevTemplate
                        oData = CopyMatchingFields(oData, d, skipObjs)
                        oData.Code = d.EDICode
                        oRet.Add(oData)
                    Next
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLoadBoardRevCosts"), db)
            End Try
        End Using
        Return oRet.ToArray()
    End Function

    ''' <summary>
    ''' Read the editable or calculated revenue fields for the Load Board Revenue page.
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2.1.004 on 11/26/2019
    ''' </remarks>
    Public Function GetvLoadBoardRev(ByVal BookControl As Integer) As LTS.vLoadBoardRev
        Dim oRet As New LTS.vLoadBoardRev
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                oRet = db.vLoadBoardRevs.Where(Function(x) x.BookControl = BookControl).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetvLoadBoardRev"), db)
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' Saves changes based on Load Board Revenue updates.  
    ''' Caller must execute RecalculateBookRevenueFreightCostsNoReturn then call GetvLoadBoardRev with the same bookcontrol. 
    ''' if the booking record was modified by another user or a different time only  BookLockAllCosts or BookLockBFCCost are allowed.
    ''' Any other changes result in a fault conflict exception
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns>
    ''' if no error and no changes are allowed the function returns false else it returns true
    ''' </returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2.1.004 on 11/26/2019
    ''' 
    ''' </remarks>
    Public Function SavevLoadBoardRevChanges(ByVal oData As LTS.vLoadBoardRev) As Boolean
        If oData Is Nothing OrElse oData.BookControl = 0 Then Return False 'nothing to do
        Using Logger.StartActivity("NGLBookRevenueData.SavevLoadBoardRevChanges(oData: {oData})", oData)
            Using db As New NGLMasBookDataContext(ConnectionString)
                Try
                    Dim BookControl As Integer = oData.BookControl
                    Dim oBook = db.Books.Where(Function(b) b.BookControl = BookControl).FirstOrDefault()
                    If oBook Is Nothing Then Return False 'nothing to do

                    With oBook
                        'Modified by RHR for v-8.5.2.006 on 12/29/2022 check for seconds because D365 does not save milliseconds
                        Dim iSeconds = DateDiff(DateInterval.Second, .BookModDate.Value, oData.BookModDate.Value)
                        If .BookModUser <> oData.BookModUser OrElse iSeconds > 0 Then
                            Dim ConflictData As New List(Of KeyValuePair(Of String, String))
                            Dim blnConflictFound As Boolean = False
                            'we only allow the costs to be locked or unlocked when the record was modified by someone else
                            If .BookLockAllCosts <> oData.BookLockAllCosts OrElse .BookLockBFCCost <> oData.BookLockBFCCost Then
                                'test for all conflicts if any exist we cannot save any changes
                                addToConflicts("BookTotalBFC", oData.BookTotalBFC, .BookTotalBFC, ConflictData, blnConflictFound)
                                addToConflicts("BookRevBilledBFC", oData.BookRevBilledBFC, .BookRevBilledBFC, ConflictData, blnConflictFound)
                                addToConflicts("BookRevDiscount", oData.BookRevDiscount, .BookRevDiscount, ConflictData, blnConflictFound)
                                addToConflicts("BookRevLineHaul", oData.BookRevLineHaul, .BookRevLineHaul, ConflictData, blnConflictFound)
                                addToConflicts("BookRevCarrierCost", oData.BookRevCarrierCost, .BookRevCarrierCost, ConflictData, blnConflictFound)
                                addToConflicts("BookRevCommPercent", oData.BookRevCommPercent, .BookRevCommPercent, ConflictData, blnConflictFound)
                                addToConflicts("BookRevCommCost", oData.BookRevCommCost, .BookRevCommCost, ConflictData, blnConflictFound)
                                addToConflicts("BookRevLoadTenderTypeControl", oData.BookRevLoadTenderTypeControl, .BookRevLoadTenderTypeControl, ConflictData, blnConflictFound)
                                addToConflicts("BookRevTotalCost", oData.BookRevTotalCost, .BookRevTotalCost, ConflictData, blnConflictFound)
                                addToConflicts("BookRevNetCost", oData.BookRevNetCost, .BookRevNetCost, ConflictData, blnConflictFound)
                                addToConflicts("BookRevFreightTax", oData.BookRevFreightTax, .BookRevFreightTax, ConflictData, blnConflictFound)
                                addToConflicts("BookRevNonTaxable", oData.BookRevNonTaxable, .BookRevNonTaxable, ConflictData, blnConflictFound)
                                addToConflicts("BookFinAPActCost", oData.BookFinAPActCost, .BookFinAPActCost, ConflictData, blnConflictFound)
                                addToConflicts("BookRevLoadSavings", oData.BookRevLoadSavings, .BookRevLoadSavings, ConflictData, blnConflictFound)
                                addToConflicts("BookFinServiceFee", oData.BookFinServiceFee, .BookFinServiceFee, ConflictData, blnConflictFound)
                                addToConflicts("BookFinCommStd", oData.BookFinCommStd, .BookFinCommStd, ConflictData, blnConflictFound)
                                addToConflicts("BookRevOtherCost", oData.BookRevOtherCost, .BookRevOtherCost, ConflictData, blnConflictFound)
                                addToConflicts("BookRevNegRevenue", oData.BookRevNegRevenue, .BookRevNegRevenue, ConflictData, blnConflictFound)
                                If blnConflictFound Then
                                    'We only add the mod date and mod user if one or more other fields have been modified
                                    addToConflicts("BookModDate", oData.BookModDate, .BookModDate, ConflictData, blnConflictFound)
                                    addToConflicts("BookModUser", oData.BookModUser, .BookModUser, ConflictData, blnConflictFound)
                                    Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(ConflictData))
                                    conflictInfo.LogConflictDetails(conflictInfo.Message, Me.Parameters)
                                    Throw New FaultException(Of ConflictFaultInfo)(conflictInfo, New FaultReason("E_DataConflict"))
                                End If
                                .BookLockAllCosts = oData.BookLockAllCosts
                                .BookLockBFCCost = oData.BookLockBFCCost
                                .BookModDate = Date.Now()
                                .BookModUser = Me.Parameters.UserName
                            Else
                                addToConflicts("BookModDate", oData.BookModDate, .BookModDate, ConflictData, blnConflictFound)
                                addToConflicts("BookModUser", oData.BookModUser, .BookModUser, ConflictData, blnConflictFound)
                                Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(ConflictData))
                                conflictInfo.LogConflictDetails(conflictInfo.Message, Me.Parameters)
                                Throw New FaultException(Of ConflictFaultInfo)(conflictInfo, New FaultReason("E_DataConflict"))
                            End If
                        Else
                            .BookLockAllCosts = oData.BookLockAllCosts
                            .BookLockBFCCost = oData.BookLockBFCCost
                            .BookTotalBFC = oData.BookTotalBFC
                            .BookRevBilledBFC = oData.BookRevBilledBFC
                            'use discount and line haul to get carrier cost if it exists
                            If oData.BookRevDiscount > 0 And oData.BookRevLineHaul <> 0 Then
                                oData.BookRevCarrierCost = oData.BookRevLineHaul - oData.BookRevDiscount
                            ElseIf oData.BookRevDiscount < 0 And oData.BookRevLineHaul <> 0 Then
                                oData.BookRevCarrierCost = oData.BookRevLineHaul + oData.BookRevDiscount
                            ElseIf oData.BookRevLineHaul <> 0 Then
                                oData.BookRevCarrierCost = oData.BookRevLineHaul
                            Else
                                oData.BookRevLineHaul = oData.BookRevCarrierCost
                            End If
                            .BookRevDiscount = oData.BookRevDiscount
                            .BookRevLineHaul = oData.BookRevLineHaul
                            .BookRevCarrierCost = oData.BookRevCarrierCost
                            If .BookRevCommCost <> oData.BookRevCommCost Then
                                'the user has modified the BookRevCommCost as a flat value so set BookRevCommPercent to zero
                                oData.BookRevCommPercent = 0
                            End If
                            .BookRevCommPercent = oData.BookRevCommPercent
                            .BookRevCommCost = oData.BookRevCommCost
                            .BookRevLoadTenderTypeControl = oData.BookRevLoadTenderTypeControl
                            .BookRevNegRevenue = oData.BookRevNegRevenue
                            'The rest are calculated when we call RecalculateBookRevenueFreightCostsNoReturn
                            '.BookRevTotalCost = oData.BookRevTotalCost
                            '.BookRevNetCost = oData.BookRevNetCost
                            '.BookRevFreightTax = oData.BookRevFreightTax
                            '.BookRevNonTaxable = oData.BookRevNonTaxable
                            '.BookFinAPActCost = oData.BookFinAPActCost
                            '.BookRevLoadSavings = oData.BookRevLoadSavings
                            '.BookFinServiceFee = oData.BookFinServiceFee
                            '.BookFinCommStd = oData.BookFinCommStd
                            '.BookRevOtherCost = oData.BookRevOtherCost
                            '.BookRevGrossRevenue = oData.BookRevGrossRevenue
                            .BookModDate = Date.Now()
                            .BookModUser = Me.Parameters.UserName
                        End If
                    End With
                    db.SubmitChanges()
                Catch ex As Exception
                    ManageLinqDataExceptions(ex, buildProcedureName("GetvLoadBoardRev"), db)
                End Try
            End Using
        End Using

        Return True
    End Function


    ''' <summary>
    ''' Set new value for BookLockAllCosts and optionally BookLockBFCCost using the value in blnLockCostFlag
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="blnLockCostFlag"></param>
    ''' <param name="blnIncludeBFC"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2.1.007 on 04/24/2020
    ''' </remarks>
    Public Function LockOrUnlockBooking(ByVal BookControl As Integer, ByVal blnLockCostFlag As Boolean, Optional ByVal blnIncludeBFC As Boolean = False) As Boolean
        If BookControl = 0 Then Return False 'nothing to do
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try

                Dim oBook = db.Books.Where(Function(b) b.BookControl = BookControl).FirstOrDefault()
                If oBook Is Nothing Then Return False 'nothing to do

                With oBook
                    .BookLockAllCosts = blnLockCostFlag
                    If blnIncludeBFC Then
                        .BookLockBFCCost = blnLockCostFlag
                    End If
                End With
                db.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("LockOrUnlockBooking"), db)
            End Try
        End Using
        Return True
    End Function

    ''' <summary>
    ''' Does the same as the Load Board Rev Fees but for the entire Load
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created By LVV on 3/27/20 for v-8.2.1.006
    ''' </remarks>
    Public Function GetLoadBoardRevLoadFees(ByVal BookControl As Integer) As Models.vCMLoadBoardRevTemplate()
        Dim oRet As New List(Of Models.vCMLoadBoardRevTemplate)
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oResults = db.spGetLoadBoardRevLoadFees(BookControl).ToArray()
                If Not oResults Is Nothing AndAlso oResults.Count > 0 Then
                    Dim skipObjs As New List(Of String)
                    For Each d In oResults
                        Dim oData As New Models.vCMLoadBoardRevTemplate
                        oData = CopyMatchingFields(oData, d, skipObjs)
                        oData.Code = d.EDICode
                        oRet.Add(oData)
                    Next
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLoadBoardRevLoadFees"), db)
            End Try
        End Using
        Return oRet.ToArray()
    End Function

    ''' <summary>
    ''' Does the same as the Load Board Rev Charges but for the entire Load
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created By LVV on 3/30/20 for v-8.2.1.006
    ''' </remarks>
    Public Function GetLoadBoardRevLoadCharges(ByVal BookControl As Integer) As Models.vCMLoadBoardRevTemplate()
        Dim oRet As New List(Of Models.vCMLoadBoardRevTemplate)
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oResults = db.spGetLoadBoardRevLoadCharges(BookControl).ToArray()
                If Not oResults Is Nothing AndAlso oResults.Count > 0 Then
                    Dim skipObjs As New List(Of String)
                    For Each d In oResults
                        Dim oData As New Models.vCMLoadBoardRevTemplate
                        oData = CopyMatchingFields(oData, d, skipObjs)
                        oData.Code = d.EDICode
                        oRet.Add(oData)
                    Next
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLoadBoardRevLoadCharges"), db)
            End Try
        End Using
        Return oRet.ToArray()
    End Function

    ''' <summary>
    ''' Does the same as the Load Board Rev Costs but for the entire Load
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created By LVV on 3/30/20 for v-8.2.1.006
    ''' </remarks>
    Public Function GetLoadBoardRevLoadCosts(ByVal BookControl As Integer) As Models.vCMLoadBoardRevTemplate()
        Dim oRet As New List(Of Models.vCMLoadBoardRevTemplate)
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oResults = db.spGetLoadBoardRevLoadCosts(BookControl).ToArray()
                If Not oResults Is Nothing AndAlso oResults.Count > 0 Then
                    Dim skipObjs As New List(Of String)
                    For Each d In oResults
                        Dim oData As New Models.vCMLoadBoardRevTemplate
                        oData = CopyMatchingFields(oData, d, skipObjs)
                        oData.Code = d.EDICode
                        oRet.Add(oData)
                    Next
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLoadBoardRevLoadCosts"), db)
            End Try
        End Using
        Return oRet.ToArray()
    End Function

    Public Function GetLoadBoardRevLoadSummaryData(ByVal BookControl As Integer) As LTS.vCMLoadBoardRevLoadSumTemplate
        Dim oRet As New LTS.vCMLoadBoardRevLoadSumTemplate
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim spRet = db.spGetLoadBoardRevLoadSummary(BookControl).FirstOrDefault()
                If spRet IsNot Nothing Then
                    Dim skipObjs As New List(Of String)
                    oRet = CopyMatchingFields(oRet, spRet, skipObjs)
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLoadBoardRevLoadSummaryData"), db)
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' Calculate the transit time and save the number of hours in BookCarrTransitTime data field 
    ''' </summary>
    ''' <param name="BookLeadTimeAutomationDaysByMile"></param>
    ''' <param name="BookLeadTimeLTLMinimum"></param>
    ''' <param name="BookTotalWgt"></param>
    ''' <param name="NbrMiles"></param>
    ''' <param name="CompControl"></param>
    ''' <param name="LaneControl"></param>
    ''' <param name="LaneTLTBenchmark"></param>
    ''' <param name="APITransit"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.3.006 on 11/29/2022 new logic to update the booking transit time in hours
    ''' </remarks>
    Public Function CalculateTenderTransitTimes(ByRef BookLeadTimeAutomationDaysByMile As Integer?,
                                                ByRef BookLeadTimeLTLMinimum As Integer?,
                                                ByVal BookTotalWgt As Double,
                                                ByVal NbrMiles As Double?,
                                                ByVal CompControl As Integer,
                                                ByVal LaneControl As Integer,
                                                ByVal LaneTLTBenchmark As Integer,
                                                ByVal APITransit As Integer) As Integer
        Dim intTransitTime As Integer = 0

        Using Logger.StartActivity("CalculateTenderTransitTimes(BookLeadTimeAutomationDaysByMile: {BookLeadTimeAutomationDaysByMile}, BookLeadTimeLTLMinimum: {BookLeadTimeLTLMinimum}, BookTotalWgt: {BookTotalWgt}, NbrMiles: {NbrMiles}, CompControl: {CompControl}, LaneControl: {LaneControl}, LaneTLTBenchmark: {LaneTLTBenchmark}, APITransit: {APITransit})")
            'Get Number Of miles traveled In a day For truckload
            If (BookLeadTimeAutomationDaysByMile.HasValue) Then
                If (BookLeadTimeAutomationDaysByMile.Value < 1) Then
                    BookLeadTimeAutomationDaysByMile = GetParValue("LaneLeadTimeAutomationDaysByMile", CompControl)
                End If
            Else
                BookLeadTimeAutomationDaysByMile = GetParValue("LaneLeadTimeAutomationDaysByMile", CompControl)
            End If
            'Get the minimum for loads less than 10,000 lbs
            If (BookLeadTimeLTLMinimum.HasValue) Then
                If (BookLeadTimeLTLMinimum.Value < 1) Then
                    BookLeadTimeLTLMinimum = GetParValue("BookLeadTimeLTLMinimum", CompControl)
                End If
            Else
                BookLeadTimeLTLMinimum = GetParValue("BookLeadTimeLTLMinimum", CompControl)
            End If

            Dim LaneLeadTimeAutomationBufferDays As Double? = GetParValue("LaneLeadTimeAutomationBufferDays", CompControl) 'Buffer To all lead times applied before minimum
            Dim LaneLeadTimeAutomationMinDays As Double? = GetParValue("LaneLeadTimeAutomationMinDays", CompControl) 'Minimum number Of days For all includes any buffers 
            ' set up defaults
            Dim intMinDays As Integer = CInt(If(LaneLeadTimeAutomationMinDays, 1))
            Dim intDaysPerMile As Integer = CInt((((If(NbrMiles, 1) / If(BookLeadTimeAutomationDaysByMile, 350))) + If(LaneLeadTimeAutomationBufferDays, 0)))
            Dim intLTLDays As Integer = CInt(If(BookLeadTimeLTLMinimum, 0) + If(LaneLeadTimeAutomationBufferDays, 0))
            'identify if this order is a truck load
            Dim blnTruckLoad As Boolean = (BookTotalWgt >= 10000)
            'use lane value as minimum if larger
            If intMinDays < LaneTLTBenchmark Then intMinDays = LaneTLTBenchmark
            'use API Transit Time as Minimun
            If intMinDays < APITransit Then intMinDays = APITransit
            'set default to minimun
            intTransitTime = intMinDays * 24 ' Default transit time, In hours, based lane miles/@LaneLeadTimeAutomationDaysByMile Or @BookLeadTimeLTLMinimum, @LaneLeadTimeAutomationBufferDays, @LaneLeadTimeAutomationMinDays And @LaneOLTBenchmark
            If blnTruckLoad And intMinDays < intDaysPerMile Then
                intTransitTime = intDaysPerMile * 24
            ElseIf Not blnTruckLoad And intMinDays < intLTLDays Then
                intTransitTime = intLTLDays * 24
            End If
        End Using

        Return intTransitTime
    End Function


#End Region

#Region "Protected Functions"

    ''' <summary>
    ''' This method is not used by this class so nothing is returned
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Return Nothing
    End Function

    ''' <summary>
    ''' This Method is not used by this class so nothing is returned
    ''' </summary>
    ''' <param name="LinqTable"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return Nothing
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
    ''' Processes inserts, updates and deletes to the book load and book item tables; 
    ''' LinqDB must be set by the caller to a valid NGLMasBookDataContext;
    ''' Caller must encapsulate LinqDB reference inside using statement
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <param name="oTable"></param>
    ''' <remarks></remarks>
    Protected Overloads Sub ProcessUpdatedDetails(ByRef oData As DataTransferObjects.BookRevenue, ByRef oTable As LTS.Book)
        Using Logger.StartActivity("NGLBookRevenueData.ProcessUpdatedDetails(oData: {oData}, oTable: {oTable})", oData, oTable)
            With CType(Me.LinqDB, NGLMasBookDataContext)
            If oData.BookLoads Is Nothing OrElse oData.BookLoads.Count() < 1 Then Return
            For Each l In oData.BookLoads
                'lookup the matching lts object
                Dim olts = oTable.BookLoads.Where(Function(x) x.BookLoadControl = l.BookLoadControl).FirstOrDefault()
                Dim blnProcessItems As Boolean = False
                Select Case l.TrackingState
                    Case TrackingInfo.Deleted
                        If Not olts Is Nothing AndAlso olts.BookLoadControl <> 0 Then
                            .BookLoads.DeleteOnSubmit(olts)
                        End If
                    Case TrackingInfo.Unchanged
                        'do nothing
                    Case Else
                        'we always perform an insert if a match is not found
                        If Not olts Is Nothing AndAlso olts.BookLoadControl <> 0 Then
                            NGLBookLoadData.UpdateLTSWithDTO(l, olts, Me.Parameters.UserName, True)
                            blnProcessItems = True
                        Else
                            olts = NGLBookLoadData.selectLTSData(l, Me.Parameters.UserName)
                            olts.BookLoadControl = 0
                            .BookLoads.InsertOnSubmit(olts)
                        End If
                End Select
                'Process any BookItem Changes 
                If blnProcessItems And Not l.BookItems Is Nothing AndAlso l.BookItems.Count() > 0 Then
                    Dim blnItemsChanged As Boolean = False
                    For Each i As DataTransferObjects.BookItem In l.BookItems
                        'lookup the matching lts object
                        Dim iLTS = olts.BookItems.Where(Function(x) x.BookItemControl = i.BookItemControl).FirstOrDefault()
                        Select Case i.TrackingState
                            Case TrackingInfo.Deleted
                                If Not iLTS Is Nothing AndAlso iLTS.BookItemControl <> 0 Then
                                    .BookItems.DeleteOnSubmit(iLTS)
                                End If
                                _ItemsChanged = True
                            Case TrackingInfo.Unchanged
                                'do nothing
                            Case Else
                                'we always perform an insert of a match is not found
                                If Not iLTS Is Nothing AndAlso iLTS.BookItemControl <> 0 Then
                                    NGLBookItemData.UpdateLTSWithDTO(i, iLTS, Me.Parameters.UserName, True)
                                Else
                                    iLTS = NGLBookItemData.selectLTSData(i, Me.Parameters.UserName)
                                    iLTS.BookItemControl = 0
                                    iLTS.BookItemBookLoadControl = l.BookLoadControl
                                    .BookItems.InsertOnSubmit(iLTS)
                                End If
                                _ItemsChanged = True
                        End Select
                        If _ItemsChanged Then _RecalcTotals = True
                    Next
                End If
            Next

    
        End With
            End Using

    End Sub

    ''' <summary>
    ''' Replaces all fees with changes and sets the CreateHistory flag to true; 
    ''' LinqDB must be set by the caller to a valid NGLMasBookDataContext
    ''' Caller must encapsulate LinqDB reference inside using statement
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <param name="oTable"></param>
    ''' <remarks>
    ''' the difference between ProcessUpdatedFees and ReplaceAllFees is:
    ''' Replace Fees does not run the validate routines associated with fees 
    '''    it assumes that the caller performs validation
    '''    it does not check the TrackingState all fees are 
    '''    inserted or added as needed it deletes all existing 
    '''    fees that do not have a matching BookFeeControl
    '''    It always sets CreateHistory to true
    ''' ProcessUpdatedFees checks the TrackingState and only updates fees
    '''    that have been modified.  It does not remove fees that may have been
    '''    added since editing started.  These remain in the fees table
    '''    It performs  validation on all fees as they are added to ensure they
    '''    meet the business requirements associated with fees.
    '''    CreateHistory is only true if fees have changed.
    ''' </remarks>
    Protected Sub ReplaceAllFees(ByRef oData As DataTransferObjects.BookRevenue, ByRef oTable As LTS.Book)
        Using operation = Logger.StartActivity("ReplaceAllFees(oData: {BookRevenue}, Book: {Book}", oData, oTable)
            If oData.BookFees Is Nothing Then Return
            CreateHistory = True
            Dim BookControl As Integer = oData.BookControl
            'Dim oFaultKey As SqlFaultInfo.FaultDetailsKey
            Dim oFaultDetails As New List(Of String)
            With CType(Me.LinqDB, NGLMasBookDataContext)
                For Each f In oData.BookFees
                    'Bug fix v-8.5.3 07/07/2022 variable scope
                    Dim match As New LTS.BookFee()
                    Try
                        match = (From t In oTable.BookFees Where t.BookFeesControl = f.BookFeesControl Select t).FirstOrDefault()
                    Catch ex As InvalidOperationException
                        match = Nothing
                    Catch ex As Exception
                        operation.Complete(LogEventLevel.Error, ex)
                    End Try

                    'this is an update or an insert.  we always insert if a match is not found
                    If Not match Is Nothing AndAlso match.BookFeesControl <> 0 Then
                        'this is an update
                        Logger.Information("Updating BookFee {BookFee}", f)
                        NGLBookFeeData.UpdateLTSWithDTO(f, match, Me.Parameters.UserName)
                    Else
                        'this is an insert
                        Logger.Information("Inserting BookFee {BookFee}", f)
                        match = NGLBookFeeData.selectLTSData(f, Me.Parameters.UserName)
                        match.BookFeesControl = 0
                        match.BookFeesBookControl = BookControl
                        .BookFees.InsertOnSubmit(match)
                    End If
                Next
                'Remove any existing records that do not exist in the DTO object
                Dim oDTOFeeControls As List(Of Integer) = oData.BookFees.Select(Function(x) x.BookFeesControl).ToList()

                Dim unmatched As List(Of LTS.BookFee) = oTable.BookFees.Where(Function(x) Not oDTOFeeControls.Contains(x.BookFeesControl)).ToList()

                If Not unmatched Is Nothing AndAlso unmatched.Count > 0 Then
                    Logger.Information("Removing unmatched BookFees {BookFees}", unmatched)
                    .BookFees.DeleteAllOnSubmit(unmatched)
                End If

            End With
        End Using

    End Sub

    ''' <summary>
    ''' Caller must ecapsulate LinqDB inside using statement
    ''' </summary>
    ''' <param name="d"></param>
    ''' <param name="t"></param>
    ''' <param name="oExistings"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function processTariffandLaneFeeOverRides(ByRef d As DataTransferObjects.BookFee, ByRef t As LTS.BookFee, ByRef oExistings As List(Of LTS.Book)) As Boolean
        Using operation = Logger.StartActivity("processTariffandLaneFeeOverRides(d: {BookFee}, t: {BookFee}, oExistings: {Book})", d, t, oExistings)
            Dim oBookDB As NGLMasBookDataContext = TryCast(Me.LinqDB, NGLMasBookDataContext)
            If oBookDB Is Nothing OrElse (d Is Nothing AndAlso d.BookFeesControl = 0) OrElse (t Is Nothing AndAlso t.BookFeesControl = 0) Then Return False

            Dim oFaultKey As SqlFaultInfo.FaultDetailsKey
            Dim oFaultDetails As New List(Of String)
            Dim intOverRiddenResults As Integer = NGLBookFeeData.IsFeeOverRidden(oBookDB, d, oFaultKey, oFaultDetails)
            'Modified by RHR 2/2/2016 v-7.0.4.1 
            If d.AllowOverwrite And intOverRiddenResults = 1 Then intOverRiddenResults = 0 'allow updates if the fee is not overridden already
            Dim blnRet As Boolean = False
            Dim BookControl As Integer = d.BookFeesBookControl
            Select Case intOverRiddenResults
                Case 1
                    blnRet = True
                    'create a new order specific fee that overrides the current record
                    Dim oLts = NGLBookFeeData.selectLTSData(d, Me.Parameters.UserName)
                    With oLts
                        .BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Order
                        .BookFeesControl = 0
                        .BookFeesAccessorialFeeCalcTypeControl = Utilities.FeeCalcType.Unique
                    End With
                    oBookDB.BookFees.InsertOnSubmit(oLts)
                    If Not oExistings Is Nothing AndAlso oExistings.Count > 0 Then
                        'add to the bookfees collection so we can find it later to avoid duplicate inserts
                        oExistings.Where(Function(x) x.BookControl = BookControl).First().BookFees.Add(oLts)
                    End If
                    'update the current record as overridden
                    With t
                        .BookFeesAccessorialDependencyTypeControl = Utilities.AccessorialFeeDependencyType.Order
                        .BookFeesAccessorialOverRideReasonControl = Utilities.AccessorialFeeOverRideReasonCode.UserOverRidden
                        .BookFeesDependencyKey = BookControl.ToString()
                        .BookFeesOverRidden = True
                        .BookFeesValue = 0
                        .BookFeesModDate = Date.Now()
                        .BookFeesModUser = Me.Parameters.UserName
                    End With

                Case -1
                    Select Case oFaultKey
                        Case SqlFaultInfo.FaultDetailsKey.E_CannotSaveParentKeyRequired
                            throwInvalidKeyParentRequiredException(oFaultDetails)
                        Case Else
                            throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_InvalidRequest, oFaultKey, oFaultDetails, SqlFaultInfo.FaultReasons.E_DataValidationFailure)
                    End Select
                Case Else
                    blnRet = True
                    NGLBookFeeData.UpdateLTSWithDTO(d, t, Me.Parameters.UserName)
            End Select
            Return blnRet
        End Using

    End Function

    ''' <summary>
    ''' Updates each fee with changes, runs validation on each fee record 
    ''' ?if changes are identified we set the CreateHistory flag to true;?
    ''' LinqDB must be set by the caller to a valid NGLMasBookDataContext
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <param name="oTable"></param>
    ''' <remarks>
    ''' the difference between ProcessUpdatedFees and ReplaceAllFees is:
    ''' Replace Fees does not run the validate routines associated with fees 
    '''    it assumes that the caller performs validation
    '''    it does not check the TrackingState all fees are 
    '''    inserted or added as needed it deletes all existing 
    '''    fees that do not have a matching BookFeeControl
    '''    It always sets CreateHistory to true
    ''' ProcessUpdatedFees checks the TrackingState and only updates fees
    '''    that have been modified.  It does not remove fees that may have been
    '''    added since editing started.  These remain in the fees table
    '''    It performs  validation on all fees as they are added to ensure they
    '''    meet the business requirements associated with fees.
    '''    CreateHistory is only true if fees have changed.
    '''    It assumes that oData is the source of truth for fee validation
    '''    all dependent fees on the load with the same AccessorialCode will
    '''    be updated inserted or marked as overloaded to match changes made 
    '''    in oData.
    ''' </remarks>
    Protected Sub ProcessUpdatedFees(ByRef oData As DataTransferObjects.BookRevenue, ByRef oTable As LTS.Book, ByRef oExistings As List(Of LTS.Book))
        Using Logger.StartActivity("ProcessUpdatedFees(oData: {oData}, oTable: {oTable}, oExistings: {oExistings})", oData, oTable, oExistings)
            If oData.BookFees Is Nothing Then Return
            Dim BookControl As Integer = oData.BookControl
            Dim BookPickupStopNumber As Integer = oData.BookPickupStopNumber
            Dim BookStopNo As Short = oData.BookStopNo
            Dim oFaultKey As SqlFaultInfo.FaultDetailsKey
            Dim oFaultDetails As New List(Of String)
            With CType(Me.LinqDB, NGLMasBookDataContext)
                For Each f In oData.BookFees
                    Dim match As New LTS.BookFee()
                    Try
                        match = (From t In oTable.BookFees Where t.BookFeesControl = f.BookFeesControl Select t).FirstOrDefault()
                    Catch ex As InvalidOperationException
                        match = Nothing
                    Catch ex As Exception
                        Throw
                    End Try

                    Select Case f.TrackingState
                        Case TrackingInfo.Created
                            CreateHistory = True
                            If Not match Is Nothing AndAlso match.BookFeesControl <> 0 Then
                                'this is an update we assume the TrackingInfo is wrong
                                'normal save
                                If processTariffandLaneFeeOverRides(f, match, oExistings) Then
                                    If Not oExistings Is Nothing AndAlso oExistings.Count > 0 AndAlso Not f.AllowOverwrite Then
                                        NormalizeAllocatedFees(f, oExistings, BookPickupStopNumber, BookStopNo)
                                        If oExistings.Count > 1 And f.BookFeesAccessorialFeeAllocationTypeControl <> Utilities.FeeAllocationType.None Then
                                            'this is a consolidated load check for missing fees
                                            InsertMissingAllocatedFees(f, oExistings, BookPickupStopNumber, BookStopNo)
                                        End If
                                    End If
                                    CreateHistory = True
                                End If
                            Else
                                'this is an insert so run the validate new record procedure
                                If Not NGLBookFeeData.validateNewRecord(CType(Me.LinqDB, NGLMasBookDataContext), f, oFaultKey, oFaultDetails) Then
                                    throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_InvalidKeyFilterMetaData, oFaultKey, oFaultDetails, SqlFaultInfo.FaultReasons.E_DataValidationFailure, True)
                                End If
                                match = NGLBookFeeData.selectLTSData(f, Me.Parameters.UserName)
                                match.BookFeesControl = 0
                                match.BookFeesBookControl = BookControl
                                .BookFees.InsertOnSubmit(match)
                                If Not oExistings Is Nothing AndAlso oExistings.Count > 0 AndAlso Not f.AllowOverwrite Then
                                    'add to the bookfees collection so we can find it later to avoid duplicate inserts
                                    oExistings.Where(Function(x) x.BookControl = BookControl).First().BookFees.Add(match)
                                    NormalizeAllocatedFees(f, oExistings, BookPickupStopNumber, BookStopNo)
                                    If oExistings.Count > 1 And f.BookFeesAccessorialFeeAllocationTypeControl <> Utilities.FeeAllocationType.None Then
                                        'this is a consolidated load check for missing fees
                                        InsertMissingAllocatedFees(f, oExistings, BookPickupStopNumber, BookStopNo)
                                    End If
                                End If
                                CreateHistory = True
                            End If

                        Case TrackingInfo.Deleted
                            If Not match Is Nothing AndAlso match.BookFeesControl <> 0 Then
                                If Not NGLBookFeeData.ValidateDeletedRecord(CType(Me.LinqDB, NGLMasBookDataContext), f, oFaultKey, oFaultDetails) Then
                                    throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_InvalidRequest, oFaultKey, oFaultDetails, SqlFaultInfo.FaultReasons.E_DataValidationFailure, True)
                                End If
                                If Not oExistings Is Nothing AndAlso oExistings.Count > 0 Then

                                    If oExistings.Count > 1 And f.BookFeesAccessorialFeeAllocationTypeControl <> Utilities.FeeAllocationType.None Then

                                        DeleteDependentAllocatedFees(f, oExistings, BookPickupStopNumber, BookStopNo)
                                    Else
                                        .BookFees.DeleteOnSubmit(match)
                                    End If
                                    ReverseMissingOverrides(f, oExistings, BookPickupStopNumber, BookStopNo)
                                Else
                                    'just delete the current match
                                    .BookFees.DeleteOnSubmit(match)
                                End If
                                CreateHistory = True
                            End If
                        Case TrackingInfo.Updated
                            'this is an update, if a match is not found we assume this record was deleted buy another 
                            'user so we do nothing
                            If Not match Is Nothing AndAlso match.BookFeesControl <> 0 Then
                                'this is an update we assume the TrackingInfo is wrong
                                'normal save
                                If processTariffandLaneFeeOverRides(f, match, oExistings) Then
                                    If Not oExistings Is Nothing AndAlso oExistings.Count > 0 AndAlso Not f.AllowOverwrite Then
                                        NormalizeAllocatedFees(f, oExistings, BookPickupStopNumber, BookStopNo)
                                        If oExistings.Count > 1 And f.BookFeesAccessorialFeeAllocationTypeControl <> Utilities.FeeAllocationType.None Then
                                            'this is a consolidated load check for missing fees
                                            InsertMissingAllocatedFees(f, oExistings, BookPickupStopNumber, BookStopNo)
                                        End If
                                    End If
                                    CreateHistory = True
                                End If
                            End If
                    End Select
                Next
            End With
        End Using

    End Sub


    ''' <summary>
    ''' Caller must encapsulate db inside using statement
    ''' </summary>
    ''' <param name="db"></param>
    ''' <param name="oData"></param>
    ''' <param name="oExisting"></param>
    ''' <param name="saveDetails"></param>
    ''' <param name="replaceFees"></param>
    ''' <param name="oExistings"></param>
    ''' <remarks></remarks>
    Private Sub UpdateLTSData(ByRef db As NGLMasBookDataContext, ByVal oData As DataTransferObjects.BookRevenue, ByRef oExisting As LTS.Book, Optional ByVal saveDetails As Boolean = False, Optional ByVal replaceFees As Boolean = False, Optional ByRef oExistings As List(Of LTS.Book) = Nothing)
        Using Logger.StartActivity("UpdateLTSData(db: {db}, oData: {oData}, oExisting: {oExisting}, saveDetails: {saveDetails}, replaceFees: {replaceFees}, oExistings: {oExistings})", db, oData, oExisting, saveDetails, replaceFees, oExistings)
            LinqTable = db.Books
            Me.LinqDB = db
            'Note: the ValidateData Function must throw a FaultException error on failure
            ValidateUpdatedRecord(LinqDB, oData)
            With oData

                'Update the table data
                Dim skipObjs As New List(Of String) From {"BookModDate", "BookModUser", "BookUpdated", "Page", "Pages", "RecordCount", "PageSize"}
                oExisting = CopyMatchingFields(oExisting, oData, skipObjs)
                'When saving BFC from BookRevenue Data only BookRevBilledBFC is 100% accurate so update BookTotalBFC with this value.
                If oExisting.BookTotalBFC <> oExisting.BookRevBilledBFC Then oExisting.BookTotalBFC = oExisting.BookRevBilledBFC
                oExisting.BookModDate = Date.Now
                oExisting.BookModUser = Me.Parameters.UserName

                If saveDetails Then
                    If replaceFees Then
                        ReplaceAllFees(oData, oExisting)
                    Else
                        ProcessUpdatedFees(oData, oExisting, oExistings)
                    End If
                    'we always update the bookload and bookitems if saveDetails is true
                    ProcessUpdatedDetails(oData, oExisting)
                End If
            End With
        End Using
    End Sub

    Private Sub CheckBookDataConcurrency(ByVal oData As DataTransferObjects.BookRevenue, ByRef oExisting As LTS.Book)
        With oData
            'Check for conflicts
            'Modified by RHR for v-8.5.2.006 on 12/29/2022 check for seconds because D365 does not save milliseconds
            Dim iSeconds = DateDiff(DateInterval.Second, .BookModDate.Value, oExisting.BookModDate.Value)
            If iSeconds > 0 Then
                'the data may have changed so check each field for conflicts
                Dim ConflictData As New List(Of KeyValuePair(Of String, String))
                Dim blnConflictFound As Boolean = False
                addToConflicts("BookBookRevHistRevision", .BookBookRevHistRevision, oExisting.BookBookRevHistRevision, ConflictData, blnConflictFound)
                addToConflicts("BookRevBilledBFC", .BookRevBilledBFC, If(oExisting.BookRevBilledBFC.HasValue, oExisting.BookRevBilledBFC, 0), ConflictData, blnConflictFound)
                addToConflicts("BookRevCarrierCost", .BookRevCarrierCost, If(oExisting.BookRevCarrierCost.HasValue, oExisting.BookRevCarrierCost, 0), ConflictData, blnConflictFound)
                addToConflicts("BookRevStopQty", .BookRevStopQty, If(oExisting.BookRevStopQty.HasValue, oExisting.BookRevStopQty, 0), ConflictData, blnConflictFound)
                addToConflicts("BookRevStopCost", .BookRevStopCost, If(oExisting.BookRevStopCost.HasValue, oExisting.BookRevStopCost, 0), ConflictData, blnConflictFound)
                addToConflicts("BookRevOtherCost", .BookRevOtherCost, If(oExisting.BookRevOtherCost.HasValue, oExisting.BookRevOtherCost, 0), ConflictData, blnConflictFound)
                addToConflicts("BookRevTotalCost", .BookRevTotalCost, If(oExisting.BookRevTotalCost.HasValue, oExisting.BookRevTotalCost, 0), ConflictData, blnConflictFound)
                addToConflicts("BookRevLoadSavings", .BookRevLoadSavings, If(oExisting.BookRevLoadSavings.HasValue, oExisting.BookRevLoadSavings, 0), ConflictData, blnConflictFound)
                addToConflicts("BookRevCommPercent", .BookRevCommPercent, If(oExisting.BookRevCommPercent.HasValue, oExisting.BookRevCommPercent, 0), ConflictData, blnConflictFound)
                addToConflicts("BookRevCommCost", .BookRevCommCost, If(oExisting.BookRevCommCost.HasValue, oExisting.BookRevCommCost, 0), ConflictData, blnConflictFound)
                addToConflicts("BookRevGrossRevenue", .BookRevGrossRevenue, If(oExisting.BookRevGrossRevenue.HasValue, oExisting.BookRevGrossRevenue, 0), ConflictData, blnConflictFound)
                addToConflicts("BookRevNegRevenue", .BookRevNegRevenue, If(oExisting.BookRevNegRevenue.HasValue, oExisting.BookRevNegRevenue, 0), ConflictData, blnConflictFound)
                addToConflicts("BookRevFreightTax", .BookRevFreightTax, oExisting.BookRevFreightTax, ConflictData, blnConflictFound)
                addToConflicts("BookRevNetCost", .BookRevNetCost, oExisting.BookRevNetCost, ConflictData, blnConflictFound)
                addToConflicts("BookRevNonTaxable", .BookRevNonTaxable, oExisting.BookRevNonTaxable, ConflictData, blnConflictFound)
                addToConflicts("BookFinARBookFrt", .BookFinARBookFrt, If(oExisting.BookFinARBookFrt.HasValue, oExisting.BookFinARBookFrt, 0), ConflictData, blnConflictFound)
                addToConflicts("BookFinAPPayAmt", .BookFinAPPayAmt, If(oExisting.BookFinAPPayAmt.HasValue, oExisting.BookFinAPPayAmt, 0), ConflictData, blnConflictFound)
                addToConflicts("BookFinAPStdCost", .BookFinAPStdCost, If(oExisting.BookFinAPStdCost.HasValue, oExisting.BookFinAPStdCost, 0), ConflictData, blnConflictFound)
                addToConflicts("BookFinAPActCost", .BookFinAPActCost, If(oExisting.BookFinAPActCost.HasValue, oExisting.BookFinAPActCost, 0), ConflictData, blnConflictFound)
                addToConflicts("BookFinCommStd", .BookFinCommStd, If(oExisting.BookFinCommStd.HasValue, oExisting.BookFinCommStd, 0), ConflictData, blnConflictFound)
                addToConflicts("BookFinServiceFee", .BookFinServiceFee, If(oExisting.BookFinServiceFee.HasValue, oExisting.BookFinServiceFee, 0), ConflictData, blnConflictFound)
                addToConflicts("BookTranCode", .BookTranCode, oExisting.BookTranCode, ConflictData, blnConflictFound)
                addToConflicts("BookPayCode", .BookPayCode, oExisting.BookPayCode, ConflictData, blnConflictFound)
                addToConflicts("BookTypeCode", .BookTypeCode, oExisting.BookTypeCode, ConflictData, blnConflictFound)
                addToConflicts("BookStopNo", .BookStopNo, If(oExisting.BookStopNo.HasValue, oExisting.BookStopNo.Value, 0), ConflictData, blnConflictFound)
                addToConflicts("BookConsPrefix", .BookConsPrefix, oExisting.BookConsPrefix, ConflictData, blnConflictFound)
                addToConflicts("BookCustCompControl", .BookCustCompControl, oExisting.BookCustCompControl, ConflictData, blnConflictFound)
                addToConflicts("BookODControl", .BookODControl, oExisting.BookODControl, ConflictData, blnConflictFound)
                addToConflicts("BookCarrierControl", .BookCarrierControl, oExisting.BookCarrierControl, ConflictData, blnConflictFound)
                addToConflicts("BookCarrierContControl", .BookCarrierContControl, oExisting.BookCarrierContControl, ConflictData, blnConflictFound)
                addToConflicts("BookCarrierContact", .BookCarrierContact, oExisting.BookCarrierContact, ConflictData, blnConflictFound)
                addToConflicts("BookCarrierContactPhone", .BookCarrierContactPhone, oExisting.BookCarrierContactPhone, ConflictData, blnConflictFound)
                addToConflicts("BookOrigCompControl", .BookOrigCompControl, If(oExisting.BookOrigCompControl.HasValue, oExisting.BookOrigCompControl, 0), ConflictData, blnConflictFound)
                addToConflicts("BookOrigName", .BookOrigName, oExisting.BookOrigName, ConflictData, blnConflictFound)
                addToConflicts("BookOrigAddress1", .BookOrigAddress1, oExisting.BookOrigAddress1, ConflictData, blnConflictFound)
                addToConflicts("BookOrigAddress2", .BookOrigAddress2, oExisting.BookOrigAddress2, ConflictData, blnConflictFound)
                addToConflicts("BookOrigAddress3", .BookOrigAddress3, oExisting.BookOrigAddress3, ConflictData, blnConflictFound)
                addToConflicts("BookOrigCity", .BookOrigCity, oExisting.BookOrigCity, ConflictData, blnConflictFound)
                addToConflicts("BookOrigState", .BookOrigState, oExisting.BookOrigState, ConflictData, blnConflictFound)
                addToConflicts("BookOrigCountry", .BookOrigCountry, oExisting.BookOrigCountry, ConflictData, blnConflictFound)
                addToConflicts("BookOrigZip", .BookOrigZip, oExisting.BookOrigZip, ConflictData, blnConflictFound)
                addToConflicts("BookDestCompControl", .BookDestCompControl, If(oExisting.BookDestCompControl.HasValue, oExisting.BookDestCompControl, 0), ConflictData, blnConflictFound)
                addToConflicts("BookDestName", .BookDestName, oExisting.BookDestName, ConflictData, blnConflictFound)
                addToConflicts("BookDestAddress1", .BookDestAddress1, oExisting.BookDestAddress1, ConflictData, blnConflictFound)
                addToConflicts("BookDestAddress2", .BookDestAddress2, oExisting.BookDestAddress2, ConflictData, blnConflictFound)
                addToConflicts("BookDestAddress3", .BookDestAddress3, oExisting.BookDestAddress3, ConflictData, blnConflictFound)
                addToConflicts("BookDestCity", .BookDestCity, oExisting.BookDestCity, ConflictData, blnConflictFound)
                addToConflicts("BookDestState", .BookDestState, oExisting.BookDestState, ConflictData, blnConflictFound)
                addToConflicts("BookDestCountry", .BookDestCountry, oExisting.BookDestCountry, ConflictData, blnConflictFound)
                addToConflicts("BookDestZip", .BookDestZip, oExisting.BookDestZip, ConflictData, blnConflictFound)
                addToConflicts("BookDateLoad", .BookDateLoad, oExisting.BookDateLoad, ConflictData, blnConflictFound)
                addToConflicts("BookDateRequired", .BookDateRequired, oExisting.BookDateRequired, ConflictData, blnConflictFound)
                addToConflicts("BookTotalCases", .BookTotalCases, If(oExisting.BookTotalCases.HasValue, oExisting.BookTotalCases, 0), ConflictData, blnConflictFound)
                addToConflicts("BookTotalWgt", .BookTotalWgt, If(oExisting.BookTotalWgt.HasValue, oExisting.BookTotalWgt, 0), ConflictData, blnConflictFound)
                addToConflicts("BookTotalPL", .BookTotalPL, If(oExisting.BookTotalPL.HasValue, oExisting.BookTotalPL, 0), ConflictData, blnConflictFound)
                addToConflicts("BookTotalCube", .BookTotalCube, If(oExisting.BookTotalCube.HasValue, oExisting.BookTotalCube, 0), ConflictData, blnConflictFound)
                addToConflicts("BookTotalPX", .BookTotalPX, If(oExisting.BookTotalPX.HasValue, oExisting.BookTotalPX, 0), ConflictData, blnConflictFound)
                addToConflicts("BookTotalBFC", .BookTotalBFC, If(oExisting.BookTotalBFC.HasValue, oExisting.BookTotalBFC, 0), ConflictData, blnConflictFound)
                addToConflicts("BookRouteFinalDate", .BookRouteFinalDate, oExisting.BookRouteFinalDate, ConflictData, blnConflictFound)
                addToConflicts("BookRouteFinalCode", .BookRouteFinalCode, oExisting.BookRouteFinalCode, ConflictData, blnConflictFound)
                addToConflicts("BookRouteFinalFlag", .BookRouteFinalFlag, oExisting.BookRouteFinalFlag, ConflictData, blnConflictFound)
                addToConflicts("BookRouteConsFlag", .BookRouteConsFlag, oExisting.BookRouteConsFlag, ConflictData, blnConflictFound)
                addToConflicts("BookComCode", .BookComCode, oExisting.BookComCode, ConflictData, blnConflictFound)
                addToConflicts("BookCarrOrderNumber", .BookCarrOrderNumber, oExisting.BookCarrOrderNumber, ConflictData, blnConflictFound)
                addToConflicts("BookOrderSequence", .BookOrderSequence, oExisting.BookOrderSequence, ConflictData, blnConflictFound)
                addToConflicts("BookLockAllCosts", .BookLockAllCosts, oExisting.BookLockAllCosts, ConflictData, blnConflictFound)
                addToConflicts("BookLockBFCCost", .BookLockBFCCost, oExisting.BookLockBFCCost, ConflictData, blnConflictFound)
                addToConflicts("BookShipCarrierProNumber", .BookShipCarrierProNumber, oExisting.BookShipCarrierProNumber, ConflictData, blnConflictFound)
                addToConflicts("BookShipCarrierProNumberRaw", .BookShipCarrierProNumberRaw, oExisting.BookShipCarrierProNumberRaw, ConflictData, blnConflictFound)
                addToConflicts("BookShipCarrierProControl", .BookShipCarrierProControl, oExisting.BookShipCarrierProControl, ConflictData, blnConflictFound)
                addToConflicts("BookShipCarrierName", .BookShipCarrierName, oExisting.BookShipCarrierName, ConflictData, blnConflictFound)
                addToConflicts("BookShipCarrierNumber", .BookShipCarrierNumber, oExisting.BookShipCarrierNumber, ConflictData, blnConflictFound)
                addToConflicts("BookShipCarrierDetails", .BookShipCarrierDetails, oExisting.BookShipCarrierDetails, ConflictData, blnConflictFound)
                addToConflicts("BookRouteTypeCode", .BookRouteTypeCode, oExisting.BookRouteTypeCode, ConflictData, blnConflictFound)
                addToConflicts("BookDefaultRouteSequence", .BookDefaultRouteSequence, oExisting.BookDefaultRouteSequence, ConflictData, blnConflictFound)
                addToConflicts("BookRouteGuideControl", .BookRouteGuideControl, oExisting.BookRouteGuideControl, ConflictData, blnConflictFound)
                addToConflicts("BookCarrTruckControl", .BookCarrTruckControl, oExisting.BookCarrTruckControl, ConflictData, blnConflictFound)
                addToConflicts("BookCarrTarControl", .BookCarrTarControl, oExisting.BookCarrTarControl, ConflictData, blnConflictFound)
                addToConflicts("BookCarrTarRevisionNumber", .BookCarrTarRevisionNumber, oExisting.BookCarrTarRevisionNumber, ConflictData, blnConflictFound)
                addToConflicts("BookCarrTarName", .BookCarrTarName, oExisting.BookCarrTarName, ConflictData, blnConflictFound)
                addToConflicts("BookCarrTarEquipControl", .BookCarrTarEquipControl, oExisting.BookCarrTarEquipControl, ConflictData, blnConflictFound)
                addToConflicts("BookCarrTarEquipName", .BookCarrTarEquipName, oExisting.BookCarrTarEquipName, ConflictData, blnConflictFound)
                addToConflicts("BookCarrTarEquipMatControl", .BookCarrTarEquipMatControl, oExisting.BookCarrTarEquipMatControl, ConflictData, blnConflictFound)
                addToConflicts("BookCarrTarEquipMatName", .BookCarrTarEquipMatName, oExisting.BookCarrTarEquipMatName, ConflictData, blnConflictFound)
                addToConflicts("BookCarrTarEquipMatDetControl", .BookCarrTarEquipMatDetControl, oExisting.BookCarrTarEquipMatDetControl, ConflictData, blnConflictFound)
                addToConflicts("BookCarrTarEquipMatDetID", .BookCarrTarEquipMatDetID, oExisting.BookCarrTarEquipMatDetID, ConflictData, blnConflictFound)
                addToConflicts("BookCarrTarEquipMatDetValue", .BookCarrTarEquipMatDetValue, oExisting.BookCarrTarEquipMatDetValue, ConflictData, blnConflictFound)
                addToConflicts("BookModeTypeControl", .BookModeTypeControl, oExisting.BookModeTypeControl, ConflictData, blnConflictFound)
                addToConflicts("BookAllowInterlinePoints", .BookAllowInterlinePoints, oExisting.BookAllowInterlinePoints, ConflictData, blnConflictFound)
                addToConflicts("BookMilesFrom", .BookMilesFrom, oExisting.BookMilesFrom, ConflictData, blnConflictFound)
                addToConflicts("BookTransType", .BookTransType, oExisting.BookTransType, ConflictData, blnConflictFound)
                addToConflicts("BookRevLaneBenchMiles", .BookRevLaneBenchMiles, oExisting.BookRevLaneBenchMiles, ConflictData, blnConflictFound)
                addToConflicts("BookRevLoadMiles", .BookRevLoadMiles, oExisting.BookRevLoadMiles, ConflictData, blnConflictFound)
                addToConflicts("BookPickupStopNumber", .BookPickupStopNumber, oExisting.BookPickupStopNumber, ConflictData, blnConflictFound)
                addToConflicts("BookRevDiscount", .BookRevDiscount, oExisting.BookRevDiscount, ConflictData, blnConflictFound)
                addToConflicts("BookRevLineHaul", .BookRevLineHaul, oExisting.BookRevLineHaul, ConflictData, blnConflictFound)
                addToConflicts("BookSHID", .BookSHID, oExisting.BookSHID, ConflictData, blnConflictFound)
                addToConflicts("BookMustLeaveByDateTime", .BookMustLeaveByDateTime, oExisting.BookMustLeaveByDateTime, ConflictData, blnConflictFound)
                addToConflicts("BookExpDelDateTime", .BookExpDelDateTime, oExisting.BookExpDelDateTime, ConflictData, blnConflictFound)
                addToConflicts("BookOutOfRouteMiles", .BookOutOfRouteMiles, oExisting.BookOutOfRouteMiles, ConflictData, blnConflictFound)
                addToConflicts("BookCreditHold", .BookCreditHold, oExisting.BookCreditHold, ConflictData, blnConflictFound)
                addToConflicts("BookSpotRateAllocationFormula", .BookSpotRateAllocationFormula, oExisting.BookSpotRateAllocationFormula, ConflictData, blnConflictFound)
                addToConflicts("BookSpotRateAutoCalcBFC", .BookSpotRateAutoCalcBFC, oExisting.BookSpotRateAutoCalcBFC, ConflictData, blnConflictFound)
                addToConflicts("BookSpotRateUseCarrierFuelAddendum", .BookSpotRateUseCarrierFuelAddendum, oExisting.BookSpotRateUseCarrierFuelAddendum, ConflictData, blnConflictFound)
                addToConflicts("BookSpotRateBFCAllocationFormula", .BookSpotRateBFCAllocationFormula, oExisting.BookSpotRateBFCAllocationFormula, ConflictData, blnConflictFound)
                addToConflicts("BookSpotRateTotalUnallocatedBFC", .BookSpotRateTotalUnallocatedBFC, oExisting.BookSpotRateTotalUnallocatedBFC, ConflictData, blnConflictFound)
                addToConflicts("BookSpotRateTotalUnallocatedLineHaul", .BookSpotRateTotalUnallocatedLineHaul, oExisting.BookSpotRateTotalUnallocatedLineHaul, ConflictData, blnConflictFound)
                addToConflicts("BookSpotRateUseFuelAddendum", .BookSpotRateUseFuelAddendum, oExisting.BookSpotRateUseFuelAddendum, ConflictData, blnConflictFound)
                'Added by LVV 6/22/16 for v-7.0.5.110 DAT
                addToConflicts("BookRevLoadTenderTypeControl", .BookRevLoadTenderTypeControl, oExisting.BookRevLoadTenderTypeControl, ConflictData, blnConflictFound)
                addToConflicts("BookRevLoadTenderStatusCode", .BookRevLoadTenderStatusCode, oExisting.BookRevLoadTenderStatusCode, ConflictData, blnConflictFound)

                If blnConflictFound Then
                    'We only add the mod date and mod user if one or more other fields have been modified
                    addToConflicts("BookModDate", .BookModDate, oExisting.BookModDate, ConflictData, blnConflictFound)
                    addToConflicts("BookModUser", .BookModUser, oExisting.BookModUser, ConflictData, blnConflictFound)
                    Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(ConflictData))
                    conflictInfo.LogConflictDetails(conflictInfo.Message, Me.Parameters)
                    Throw New FaultException(Of ConflictFaultInfo)(conflictInfo, New FaultReason("E_DataConflict"))
                End If
            End If
        End With
    End Sub

    Friend Shared Function selectDTOData(ByVal d As LTS.Book, ByRef db As NGLMasBookDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.BookRevenue
        Dim oDTO As New DataTransferObjects.BookRevenue
        Dim skipObjs As New List(Of String) From {"CompFinUseImportFrtCost", "BookUpdated", "Page", "Pages", "RecordCount", "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .CompFinUseImportFrtCost = If(d.CompRefBook.CompFinUseImportFrtCost.HasValue, d.CompRefBook.CompFinUseImportFrtCost.Value, False)
            .LaneOriginAddressUse = d.LaneRefBook.LaneOriginAddressUse
            .CompanyName = d.CompRefBook.CompName
            .CompanyNumber = d.CompRefBook.CompNumber.ToString
            .BookUpdated = d.BookUpdated.ToArray()
            ' Added by LVV 7/20/16 for v-7.0.5.110 DAT
            .BookRevLoadTenderStatusCode = d.BookRevLoadTenderStatusCode
            .BookRevLoadTenderTypeControl = d.BookRevLoadTenderTypeControl
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oDTO

    End Function

    Friend Shared Function selectDTOData(ByVal d As LTS.vBookRevenue, ByRef db As NGLMasBookDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.BookRevenue
        Dim oDTO As New DataTransferObjects.BookRevenue
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

    ''' <summary>
    ''' Copies property information from d into a new DTO.Bookrevenue object
    ''' </summary>
    ''' <param name="d"></param>
    ''' <param name="db"></param>
    ''' <param name="page"></param>
    ''' <param name="pagecount"></param>
    ''' <param name="recordcount"></param>
    ''' <param name="pagesize"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function selectDTODataSP(ByVal d As LTS.spGetBookRevenueResult, ByRef db As NGLMasBookDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.BookRevenue

        Dim oDTO As New DataTransferObjects.BookRevenue
        Dim skipObjs As New List(Of String) From {
                "BookUpdated",
                "Page",
                "Pages",
                "RecordCount",
                "PageSize"}
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



    ''' <summary>
    ''' Copies property information from d into a new DTO.Bookrevenue object
    ''' </summary>
    ''' <param name="d"></param>
    ''' <param name="db"></param>
    ''' <param name="page"></param>
    ''' <param name="pagecount"></param>
    ''' <param name="recordcount"></param>
    ''' <param name="pagesize"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function selectDTODataSP(ByVal d As LTS.spGetBookRevenuesResult, ByRef db As NGLMasBookDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.BookRevenue

        Dim oDTO As New DataTransferObjects.BookRevenue
        Dim skipObjs As New List(Of String) From {
                "BookUpdated",
                "Page",
                "Pages",
                "RecordCount",
                "PageSize"}
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

    Friend Function selectDTODataRobTestSP(ByVal d As LTS.spGetBookRevenueResult, ByRef db As NGLMasBookDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.BookRevenue

        Return New DataTransferObjects.BookRevenue With {.BookControl = d.BookControl,
            .BookBookRevHistRevision = d.BookBookRevHistRevision,
            .BookRevBilledBFC = d.BookRevBilledBFC,
            .BookRevCarrierCost = d.BookRevCarrierCost,
            .BookRevStopQty = d.BookRevStopQty,
            .BookRevStopCost = d.BookRevStopCost,
            .BookRevOtherCost = d.BookRevOtherCost,
            .BookRevTotalCost = d.BookRevTotalCost,
            .BookRevLoadSavings = d.BookRevLoadSavings,
            .BookRevCommPercent = d.BookRevCommPercent,
            .BookRevCommCost = d.BookRevCommCost,
            .BookRevGrossRevenue = d.BookRevGrossRevenue,
            .BookRevNegRevenue = d.BookRevNegRevenue,
            .BookRevFreightTax = d.BookRevFreightTax,
            .BookRevNetCost = d.BookRevNetCost,
            .BookRevNonTaxable = d.BookRevNonTaxable,
            .BookFinARBookFrt = d.BookFinARBookFrt,
            .BookFinAPPayAmt = d.BookFinAPPayAmt,
            .BookFinAPStdCost = d.BookFinAPStdCost,
            .BookFinAPActCost = d.BookFinAPActCost,
            .BookFinCommStd = d.BookFinCommStd,
            .BookFinServiceFee = d.BookFinServiceFee,
            .BookTranCode = d.BookTranCode,
            .BookPayCode = d.BookPayCode,
            .BookTypeCode = d.BookTypeCode,
            .BookStopNo = d.BookStopNo,
            .BookConsPrefix = d.BookConsPrefix,
            .BookCustCompControl = d.BookCustCompControl,
            .BookODControl = d.BookODControl,
            .BookCarrierControl = d.BookCarrierControl,
            .BookCarrierContControl = d.BookCarrierContControl,
            .BookCarrierContact = d.BookCarrierContact,
            .BookCarrierContactPhone = d.BookCarrierContactPhone,
            .BookOrigCompControl = d.BookOrigCompControl,
            .BookOrigName = d.BookOrigName,
            .BookOrigAddress1 = d.BookOrigAddress1,
            .BookOrigAddress2 = d.BookOrigAddress2,
            .BookOrigAddress3 = d.BookOrigAddress3,
            .BookOrigCity = d.BookOrigCity,
            .BookOrigState = d.BookOrigState,
            .BookOrigCountry = d.BookOrigCountry,
            .BookOrigZip = d.BookOrigZip,
            .BookDestCompControl = d.BookDestCompControl,
            .BookDestName = d.BookDestName,
            .BookDestAddress1 = d.BookDestAddress1,
            .BookDestAddress2 = d.BookDestAddress2,
            .BookDestAddress3 = d.BookDestAddress3,
            .BookDestCity = d.BookDestCity,
            .BookDestState = d.BookDestState,
            .BookDestCountry = d.BookDestCountry,
            .BookDestZip = d.BookDestZip,
            .BookDateLoad = d.BookDateLoad,
            .BookDateRequired = d.BookDateRequired,
            .BookTotalCases = d.BookTotalCases,
            .BookTotalWgt = d.BookTotalWgt,
            .BookTotalPL = d.BookTotalPL,
            .BookTotalCube = d.BookTotalCube,
            .BookTotalPX = d.BookTotalPX,
            .BookTotalBFC = d.BookTotalBFC,
            .BookRouteFinalDate = d.BookRouteFinalDate,
            .BookRouteFinalCode = d.BookRouteFinalCode,
            .BookRouteFinalFlag = d.BookRouteFinalFlag,
            .BookRouteConsFlag = d.BookRouteConsFlag,
            .BookComCode = d.BookComCode,
            .BookCarrOrderNumber = d.BookCarrOrderNumber,
            .BookOrderSequence = d.BookOrderSequence,
            .BookLockAllCosts = d.BookLockAllCosts,
            .BookLockBFCCost = d.BookLockBFCCost,
            .BookShipCarrierProNumber = d.BookShipCarrierProNumber,
            .BookShipCarrierProNumberRaw = d.BookShipCarrierProNumberRaw,
            .BookShipCarrierProControl = d.BookShipCarrierProControl,
            .BookShipCarrierName = d.BookShipCarrierName,
            .BookShipCarrierNumber = d.BookShipCarrierNumber,
            .BookShipCarrierDetails = d.BookShipCarrierDetails,
            .BookRouteTypeCode = d.BookRouteTypeCode,
            .BookDefaultRouteSequence = d.BookDefaultRouteSequence,
            .BookRouteGuideControl = d.BookRouteGuideControl,
            .BookCarrTruckControl = d.BookCarrTruckControl,
            .BookCarrTarControl = d.BookCarrTarControl,
            .BookCarrTarRevisionNumber = d.BookCarrTarRevisionNumber,
            .BookCarrTarName = d.BookCarrTarName,
            .BookCarrTarEquipControl = d.BookCarrTarEquipControl,
            .BookCarrTarEquipName = d.BookCarrTarEquipName,
            .BookCarrTarEquipMatControl = d.BookCarrTarEquipMatControl,
            .BookCarrTarEquipMatName = d.BookCarrTarEquipMatName,
            .BookCarrTarEquipMatDetControl = d.BookCarrTarEquipMatDetControl,
            .BookCarrTarEquipMatDetID = d.BookCarrTarEquipMatDetID,
            .BookCarrTarEquipMatDetValue = d.BookCarrTarEquipMatDetValue,
            .BookModeTypeControl = d.BookModeTypeControl,
            .BookAllowInterlinePoints = d.BookAllowInterlinePoints,
            .BookMilesFrom = d.BookMilesFrom,
            .BookTransType = d.BookTransType,
            .BookProNumber = d.BookProNumber,
            .BookRevLaneBenchMiles = d.BookRevLaneBenchMiles,
            .BookRevLoadMiles = d.BookRevLoadMiles,
            .BookPickupStopNumber = d.BookPickupStopNumber,
            .BookRevDiscount = d.BookRevDiscount,
            .BookRevLineHaul = d.BookRevLineHaul,
            .BookSHID = d.BookSHID,
            .BookMustLeaveByDateTime = d.BookMustLeaveByDateTime,
            .BookExpDelDateTime = d.BookExpDelDateTime,
            .BookOutOfRouteMiles = d.BookOutOfRouteMiles,
            .CompFinUseImportFrtCost = d.CompFinUseImportFrtCost,
            .CompanyName = d.CompanyName,
            .CompanyNumber = d.CompanyNumber,
            .LaneOriginAddressUse = d.LaneOriginAddressUse,
            .BookCreditHold = d.BookCreditHold,
            .BookSpotRateAllocationFormula = d.BookSpotRateAllocationFormula,
            .BookSpotRateAutoCalcBFC = d.BookSpotRateAutoCalcBFC,
            .BookSpotRateUseCarrierFuelAddendum = d.BookSpotRateUseCarrierFuelAddendum,
            .BookSpotRateBFCAllocationFormula = d.BookSpotRateBFCAllocationFormula,
            .BookSpotRateTotalUnallocatedBFC = d.BookSpotRateTotalUnallocatedBFC,
            .BookSpotRateTotalUnallocatedLineHaul = d.BookSpotRateTotalUnallocatedLineHaul,
            .BookSpotRateUseFuelAddendum = d.BookSpotRateUseFuelAddendum,
            .BookModUser = d.BookModUser,
            .BookModDate = d.BookModDate,
            .BookUpdated = d.BookUpdated.ToArray(),
            .Page = page,
            .Pages = pagecount,
            .RecordCount = recordcount,
            .PageSize = pagesize}

    End Function


    Friend Function selectDTODataWDetails(ByVal d As LTS.Book, ByRef db As NGLMasBookDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.BookRevenue

        Dim oDTO As New DataTransferObjects.BookRevenue
        Dim oLoadData As NGLBookLoadData = New NGLBookLoadData(Parameters)

        oDTO = selectDTOData(d, db, page, pagecount, recordcount, pagesize)
        If Not oDTO Is Nothing Then
            With oDTO
                .BookLoads = (From bl In d.BookLoads Select NGLBookLoadData.selectDTODataWDetails(bl)).ToList()
                .BookFees = (From f In d.BookFees Select NGLBookFeeData.selectDTOData(f)).ToList()
            End With
        End If
        Return oDTO

        'Return New DTO.BookRevenue With {.BookControl = d.BookControl, _
        '                                .BookBookRevHistRevision = d.BookBookRevHistRevision, _
        '                                .BookRevBilledBFC = If(d.BookRevBilledBFC.HasValue, d.BookRevBilledBFC, 0), _
        '                                .BookRevCarrierCost = If(d.BookRevCarrierCost.HasValue, d.BookRevCarrierCost, 0), _
        '                                .BookRevStopQty = If(d.BookRevStopQty.HasValue, d.BookRevStopQty, 0), _
        '                                .BookRevStopCost = If(d.BookRevStopCost.HasValue, d.BookRevStopCost, 0), _
        '                                .BookRevOtherCost = If(d.BookRevOtherCost.HasValue, d.BookRevOtherCost, 0), _
        '                                .BookRevTotalCost = If(d.BookRevTotalCost.HasValue, d.BookRevTotalCost, 0), _
        '                                .BookRevLoadSavings = If(d.BookRevLoadSavings.HasValue, d.BookRevLoadSavings, 0), _
        '                                .BookRevCommPercent = If(d.BookRevCommPercent.HasValue, d.BookRevCommPercent, 0), _
        '                                .BookRevCommCost = If(d.BookRevCommCost.HasValue, d.BookRevCommCost, 0), _
        '                                .BookRevGrossRevenue = If(d.BookRevGrossRevenue.HasValue, d.BookRevGrossRevenue, 0), _
        '                                .BookRevNegRevenue = If(d.BookRevNegRevenue.HasValue, d.BookRevNegRevenue, 0), _
        '                                .BookRevFreightTax = d.BookRevFreightTax, _
        '                                .BookRevNetCost = d.BookRevNetCost, _
        '                                .BookRevNonTaxable = d.BookRevNonTaxable, _
        '                                .BookFinARBookFrt = If(d.BookFinARBookFrt.HasValue, d.BookFinARBookFrt, 0), _
        '                                .BookFinAPPayAmt = If(d.BookFinAPPayAmt.HasValue, d.BookFinAPPayAmt, 0), _
        '                                .BookFinAPStdCost = If(d.BookFinAPStdCost.HasValue, d.BookFinAPStdCost, 0), _
        '                                .BookFinAPActCost = If(d.BookFinAPActCost.HasValue, d.BookFinAPActCost, 0), _
        '                                .BookFinCommStd = If(d.BookFinCommStd.HasValue, d.BookFinCommStd, 0), _
        '                                .BookFinServiceFee = If(d.BookFinServiceFee.HasValue, d.BookFinServiceFee, 0), _
        '                                .BookTranCode = d.BookTranCode, _
        '                                .BookPayCode = d.BookPayCode, _
        '                                .BookTypeCode = d.BookTypeCode, _
        '                                .BookStopNo = If(d.BookStopNo.HasValue, d.BookStopNo.Value, 0), _
        '                                .BookConsPrefix = d.BookConsPrefix, _
        '                                .BookCustCompControl = d.BookCustCompControl, _
        '                                .BookODControl = d.BookODControl, _
        '                                .BookCarrierControl = d.BookCarrierControl, _
        '                                .BookCarrierContControl = d.BookCarrierContControl, _
        '                                .BookCarrierContact = d.BookCarrierContact, _
        '                                .BookCarrierContactPhone = d.BookCarrierContactPhone, _
        '                                .BookOrigCompControl = If(d.BookOrigCompControl.HasValue, d.BookOrigCompControl, 0), _
        '                                .BookOrigName = d.BookOrigName, _
        '                                .BookOrigAddress1 = d.BookOrigAddress1, _
        '                                .BookOrigAddress2 = d.BookOrigAddress2, _
        '                                .BookOrigAddress3 = d.BookOrigAddress3, _
        '                                .BookOrigCity = d.BookOrigCity, _
        '                                .BookOrigState = d.BookOrigState, _
        '                                .BookOrigCountry = d.BookOrigCountry, _
        '                                .BookOrigZip = d.BookOrigZip, _
        '                                .BookDestCompControl = If(d.BookDestCompControl.HasValue, d.BookDestCompControl, 0), _
        '                                .BookDestName = d.BookDestName, _
        '                                .BookDestAddress1 = d.BookDestAddress1, _
        '                                .BookDestAddress2 = d.BookDestAddress2, _
        '                                .BookDestAddress3 = d.BookDestAddress3, _
        '                                .BookDestCity = d.BookDestCity, _
        '                                .BookDestState = d.BookDestState, _
        '                                .BookDestCountry = d.BookDestCountry, _
        '                                .BookDestZip = d.BookDestZip, _
        '                                .BookDateLoad = d.BookDateLoad, _
        '                                .BookDateRequired = d.BookDateRequired, _
        '                                .BookTotalCases = If(d.BookTotalCases.HasValue, d.BookTotalCases, 0), _
        '                                .BookTotalWgt = If(d.BookTotalWgt.HasValue, d.BookTotalWgt, 0), _
        '                                .BookTotalPL = If(d.BookTotalPL.HasValue, d.BookTotalPL, 0), _
        '                                .BookTotalCube = If(d.BookTotalCube.HasValue, d.BookTotalCube, 0), _
        '                                .BookTotalPX = If(d.BookTotalPX.HasValue, d.BookTotalPX, 0), _
        '                                .BookTotalBFC = If(d.BookTotalBFC.HasValue, d.BookTotalBFC, 0), _
        '                                .BookRouteFinalDate = d.BookRouteFinalDate, _
        '                                .BookRouteFinalCode = d.BookRouteFinalCode, _
        '                                .BookRouteFinalFlag = d.BookRouteFinalFlag, _
        '                                .BookRouteConsFlag = d.BookRouteConsFlag, _
        '                                .BookComCode = d.BookComCode, _
        '                                .BookCarrOrderNumber = d.BookCarrOrderNumber, _
        '                                .BookOrderSequence = d.BookOrderSequence, _
        '                                .BookLockAllCosts = d.BookLockAllCosts, _
        '                                .BookLockBFCCost = d.BookLockBFCCost, _
        '                                .BookShipCarrierProNumber = d.BookShipCarrierProNumber, _
        '                                .BookShipCarrierProNumberRaw = d.BookShipCarrierProNumberRaw, _
        '                                .BookShipCarrierProControl = d.BookShipCarrierProControl, _
        '                                .BookShipCarrierName = d.BookShipCarrierName, _
        '                                .BookShipCarrierNumber = d.BookShipCarrierNumber, _
        '                                .BookShipCarrierDetails = d.BookShipCarrierDetails, _
        '                                .BookRouteTypeCode = d.BookRouteTypeCode, _
        '                                .BookDefaultRouteSequence = d.BookDefaultRouteSequence, _
        '                                .BookRouteGuideControl = d.BookRouteGuideControl, _
        '                                .BookCarrTruckControl = d.BookCarrTruckControl, _
        '                                .BookCarrTarControl = d.BookCarrTarControl, _
        '                                .BookCarrTarRevisionNumber = d.BookCarrTarRevisionNumber, _
        '                                .BookCarrTarName = d.BookCarrTarName, _
        '                                .BookCarrTarEquipControl = d.BookCarrTarEquipControl, _
        '                                .BookCarrTarEquipName = d.BookCarrTarEquipName, _
        '                                .BookCarrTarEquipMatControl = d.BookCarrTarEquipMatControl, _
        '                                .BookCarrTarEquipMatName = d.BookCarrTarEquipMatName, _
        '                                .BookCarrTarEquipMatDetControl = d.BookCarrTarEquipMatDetControl, _
        '                                .BookCarrTarEquipMatDetID = d.BookCarrTarEquipMatDetID, _
        '                                .BookCarrTarEquipMatDetValue = d.BookCarrTarEquipMatDetValue, _
        '                                .BookModeTypeControl = d.BookModeTypeControl, _
        '                                .BookAllowInterlinePoints = d.BookAllowInterlinePoints, _
        '                                .BookMilesFrom = d.BookMilesFrom, _
        '                                .BookTransType = d.BookTransType, _
        '                                .BookProNumber = d.BookProNumber, _
        '                                .BookRevLaneBenchMiles = d.BookRevLaneBenchMiles, _
        '                                .BookRevLoadMiles = d.BookRevLoadMiles, _
        '                                .CompFinUseImportFrtCost = If(d.CompRefBook.CompFinUseImportFrtCost.HasValue, d.CompRefBook.CompFinUseImportFrtCost.Value, False), _
        '                                .CompanyName = d.CompRefBook.CompName, _
        '                                .CompanyNumber = d.CompRefBook.CompNumber.ToString, _
        '                                .BookPickupStopNumber = d.BookPickupStopNumber, _
        '                                .BookRevDiscount = d.BookRevDiscount, _
        '                                .BookRevLineHaul = d.BookRevLineHaul, _
        '                                .BookSHID = d.BookSHID, _
        '                                .BookMustLeaveByDateTime = d.BookMustLeaveByDateTime, _
        '                                .BookExpDelDateTime = d.BookExpDelDateTime, _
        '                                .BookOutOfRouteMiles = d.BookOutOfRouteMiles, _
        '                                .BookModUser = d.BookModUser,
        '                                .BookModDate = d.BookModDate,
        '                                .BookUpdated = d.BookUpdated.ToArray(), _
        '                                .Page = page,
        '                                .Pages = pagecount,
        '                                .RecordCount = recordcount,
        '                                .PageSize = pagesize,
        '                                 .BookLoads = (From bl In d.BookLoads Select oLoadData.selectDTODataWDetails(bl)).ToList(),
        '                                 .BookFees = (From f In d.BookFees Select oFees.selectDTOData(f)).ToList(),
        '                                 .LaneOriginAddressUse = d.LaneRefBook.LaneOriginAddressUse}

    End Function


    Friend Function selectDTODataWDetails(ByVal d As LTS.vBookRevenue, ByRef db As NGLMasBookDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.BookRevenue

        Dim oDTO As New DataTransferObjects.BookRevenue
        Dim oLoadData As NGLBookLoadData = Me.NDPBaseClassFactory("NGLBookLoadData", False)

        oDTO = selectDTOData(d, db, page, pagecount, recordcount, pagesize)

        If Not oDTO Is Nothing Then
            With oDTO
                .BookLoads = (From bl In d.BookLoads Select NGLBookLoadData.selectDTODataWDetails(bl)).ToList()
                .BookFees = (From f In d.BookFees Select NGLBookFeeData.selectDTOData(f)).ToList()
            End With
        End If
        Return oDTO


    End Function

    ''' <summary>
    ''' Calculates the total stop charge for the load
    ''' The caller must determine which fees are marked as overridden
    ''' The method only selectes fees where the BookFeesOverRidden = False
    ''' </summary>
    ''' <param name="oBookRevs"></param>
    ''' <param name="CarrierCost"></param>
    ''' <param name="Taxable"></param>
    ''' <param name="Message"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function calculateStopCharge(ByRef oBookRevs As DataTransferObjects.BookRevenue(), ByVal CarrierCost As Decimal, ByVal Taxable As Boolean, ByRef Message As String) As Decimal
        Dim decTotalStopChargesForLoad As Decimal = 0
        Using Logger.StartActivity("calculateStopCharge")

            'Get the Stop Fee Variable Rate
            'Calculation Business Rules
            'Select Stop Fees based on Accessorial Codes 20 to 28
            'Historical Business Rules Compatibility
            'NOTE: we use enumerators for Variable Code and other values the numbers are for example an are subject to change
            '---------------------------------------------------------
            'Determine Calculation Formula based on FeeVariableCode Enum
            '   Legacy Stop codes are between Stop1Flat (7) and Stop10Flat (16) which represent stop 1 to 10
            '   If the BookStopNo value + (Stop1Flat - 1) is an exact match with a configured Variable Code use that Variable Code to determine the formula
            '   If the BookStopNo value + (Stop1Flat - 1) is greater than the highest configured Variable Code use the highest Variable Code to determine the formula
            '   If the BookStopNo value + (Stop1Flat - 1) is less than the highest configured Variable Code use the next highest Variable Code to determine the formula
            '---------------------------------------------------------
            'New 7.0 Business Rules Accessorial Codes 20 to 28 are reserved for Legacy calculations 
            '   to use new stop fee calcuations a custom Accessorial code must be used.
            '---------------------------------------------------------
            'Legacy Stop Fees are always allocated by Destination so the FeeAllocationType is ignored
            '---------------------------------------------------------
            'Legacy Stop Fees are always Unique so FeeCalcType must always be Unique = 2 (Override Duplicate Fees At Order Level)
            'The caller must determine if the Lane or carrier fees are to be marked as overridden
            'This method only selects fees where the BookFeesOverRidden = False


            Dim intMinAccessorialCode As Integer = 20
            Dim intMaxAccessorialCode As Integer = 28
            Dim lProcessed As New List(Of Integer)
            Try
                For Each oBRev In oBookRevs.OrderBy(Function(x) x.BookStopNo)
                    Dim intStopBookStopNo As Short = oBRev.BookStopNo
                    Dim intStopBookControl As Integer = oBRev.BookControl
                    If lProcessed.Contains(intStopBookControl) Then Continue For 'Skip to the next book record
                    Dim oStopSummaryData = oBookRevs.Where(Function(x) x.BookStopNo = intStopBookStopNo).ToArray()
                    lProcessed.AddRange(oStopSummaryData.Select(Function(X) X.BookControl))
                    Dim oBookFeesByStop As List(Of BookFee) = (From d In oStopSummaryData From f In d.BookFees Where (f.BookFeesAccessorialCode >= intMinAccessorialCode And f.BookFeesAccessorialCode <= intMaxAccessorialCode) And (f.BookFeesVariableCode >= Utilities.FeeVariableCode.Stop1Flat And f.BookFeesVariableCode <= Utilities.FeeVariableCode.Stop10Flat) And f.BookFeesTaxable = Taxable Select f).ToList()
                    If oBookFeesByStop Is Nothing OrElse oBookFeesByStop.Count < 1 Then Continue For 'Skip to the next book record
                    'count the number of shared stops
                    Dim intStopSharedStops As Integer = oStopSummaryData.Count()
                    'Sum the total weight for this stop (default = 1)
                    Dim dblStopTotalWeightByStop As Double = oStopSummaryData.Sum(Function(x) x.BookTotalWgt)
                    If dblStopTotalWeightByStop = 0 Then dblStopTotalWeightByStop = 1
                    Dim dblStopMaxBookStopWeight As Double = oStopSummaryData.Max(Function(x) x.BookTotalWgt)
                    Dim intStopMaxWeightBookControl As Integer = (From d In oStopSummaryData Where d.BookTotalWgt = dblStopMaxBookStopWeight Select d.BookControl).FirstOrDefault()
                    If dblStopMaxBookStopWeight = 0 Then dblStopMaxBookStopWeight = 1
                    'Get the Stop Fee Variable Rate using the current stop number
                    Dim intStopVariableCode As Integer = If(intStopBookStopNo = 0, Utilities.FeeVariableCode.Stop1Flat, intStopBookStopNo + (Utilities.FeeVariableCode.Stop1Flat - 1))
                    'Calculate the highest possible stop charge
                    Dim decStopMaxStopFeesCharged As Decimal
                    'Bug fix v-8.5.3 07/07/2022 variable scope
                    decStopMaxStopFeesCharged = 0
                    'get the lowest variable that requires a stop charge
                    Dim intMinVariableCode As Integer = oBookFeesByStop.Select(Function(x) x.BookFeesVariableCode).Min()
                    'if this stop is lower than the smallest stop charge variable we use zero for the stop charge
                    If intStopVariableCode >= intMinVariableCode Then
                        'Get the first fee with an exact match to this StopVariableCode (stop number)
                        Dim oStopFees As List(Of BookFee) = (From d In oBookFeesByStop Where d.BookFeesVariableCode = intStopVariableCode And d.BookFeesOverRidden = False Select d).ToList()
                        Dim oStopFeeUsed As New DataTransferObjects.BookFee
                        If Not oStopFees Is Nothing AndAlso oStopFees.Count > 0 Then
                            oStopFeeUsed = oStopFees.OrderByDescending(Function(x) x.BookFeesMinimum).FirstOrDefault()
                        Else
                            'Try to use the next highest variable code to get the maximum charge
                            oStopFees = (From d In oBookFeesByStop Where
                            (d.BookFeesVariableCode > intStopVariableCode) _
                            And d.BookFeesOverRidden = False Order By d.BookFeesVariableCode Select d).ToList()
                            If Not oStopFees Is Nothing AndAlso oStopFees.Count > 0 Then
                                oStopFeeUsed = oStopFees.OrderBy(Function(x) x.BookFeesVariableCode).ThenByDescending(Function(x) x.BookFeesMinimum).FirstOrDefault()
                            Else
                                'Use the maximum charge for the highest variable code
                                oStopFees = (From d In oBookFeesByStop Where
                                d.BookFeesVariableCode >= Utilities.FeeVariableCode.Stop1Flat _
                                And d.BookFeesVariableCode <= Utilities.FeeVariableCode.Stop10Flat _
                                And d.BookFeesOverRidden = False Select d).ToList()
                                If Not oStopFees Is Nothing AndAlso oStopFees.Count > 0 Then
                                    oStopFeeUsed = oStopFees.OrderByDescending(Function(x) x.BookFeesVariableCode).ThenByDescending(Function(x) x.BookFeesMinimum).FirstOrDefault()
                                End If
                            End If
                        End If
                        If Not oStopFeeUsed Is Nothing Then
                            decStopMaxStopFeesCharged = oStopFeeUsed.BookFeesMinimum
                            Dim actualAccessorialCode As Integer = oStopFeeUsed.BookFeesAccessorialCode
                            Dim actualVariableCode As Integer = oStopFeeUsed.BookFeesVariableCode
                            If decStopMaxStopFeesCharged <= 0 Then Continue For 'skip to next record
                            Dim decStopFee As Decimal = 0
                            Dim decTotalStopCharges As Decimal = 0
                            'Loop through each item in the stops collection 
                            'calculate the stop fee allocted by weight then add the allocated amount to the total for this stop
                            Dim oThisBookFee As New DataTransferObjects.BookFee
                            Dim oMaxBookWgtFee As DataTransferObjects.BookFee
                            For Each s In oStopSummaryData
                                decStopFee = Math.Round(decStopMaxStopFeesCharged * (If(s.BookTotalWgt = 0, 1, s.BookTotalWgt) / If(dblStopTotalWeightByStop = 0, 1, dblStopTotalWeightByStop)), 2)
                                decTotalStopCharges += decStopFee
                                Dim oBookStopFees = (From f In s.BookFees
                                                     Where f.BookFeesAccessorialCode = actualAccessorialCode _
                                                       And f.BookFeesVariableCode = actualVariableCode _
                                                       And f.BookFeesTaxable = Taxable
                                                     Order By
                                                 f.BookFeesAccessorialFeeTypeControl Descending,
                                                 f.BookFeesMinimum Descending
                                                     Select f).ToList()
                                'Update the FeeValue with the calculated cost
                                If Not oBookStopFees Is Nothing AndAlso oBookStopFees.Count > 0 Then
                                    Dim blnMatchFound As Boolean = False
                                    For Each f In oBookStopFees
                                        If Not blnMatchFound And f.BookFeesAccessorialCode = actualAccessorialCode Then
                                            blnMatchFound = True
                                            With f
                                                'Update the first matching record with the selected book fee data 
                                                'all book fees on the stop should match
                                                .BookFeesValue = decStopFee
                                                .BookFeesAccessorialFeeAllocationTypeControl = Utilities.FeeAllocationType.Destination 'Always Destination for legacy stop fees
                                                .BookFeesAccessorialFeeCalcTypeControl = Utilities.FeeCalcType.Unique 'Always Unique for legacy stop fees
                                                .BookFeesMinimum = oStopFeeUsed.BookFeesMinimum
                                                .BookFeesTarBracketTypeControl = oStopFeeUsed.BookFeesTarBracketTypeControl
                                                .BookFeesVariable = oStopFeeUsed.BookFeesVariable
                                                .BookFeesVariableCode = oStopFeeUsed.BookFeesVariableCode
                                                .TrackingState = TrackingInfo.Updated
                                            End With
                                            oThisBookFee = f
                                        Else
                                            f.BookFeesOverRidden = True
                                            f.BookFeesValue = 0
                                            f.TrackingState = TrackingInfo.Updated
                                        End If
                                    Next
                                Else
                                    'add the fee
                                    oThisBookFee = oStopFeeUsed.Clone()
                                    With oThisBookFee
                                        .BookFeesBookControl = s.BookControl
                                        .BookFeesControl = 0
                                        .BookFeesValue = decStopFee
                                        .BookFeesAccessorialFeeAllocationTypeControl = Utilities.FeeAllocationType.Destination
                                        .BookFeesAccessorialFeeCalcTypeControl = Utilities.FeeCalcType.Unique
                                        .BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Order
                                        .BookFeesTaxable = Taxable
                                        .TrackingState = TrackingInfo.Created
                                    End With
                                    s.BookFees.Add(oThisBookFee)
                                End If
                                If s.BookControl = intStopMaxWeightBookControl Then oMaxBookWgtFee = oThisBookFee
                            Next
                            If decTotalStopCharges <> decStopMaxStopFeesCharged AndAlso Not oMaxBookWgtFee Is Nothing Then
                                'Adjust for Rounding Errors
                                oMaxBookWgtFee.BookFeesValue += (decStopMaxStopFeesCharged - decTotalStopCharges)
                            End If
                        End If
                    End If
                    decTotalStopChargesForLoad += decStopMaxStopFeesCharged
                Next
            Catch ex As System.Data.SqlClient.SqlException
                Logger.Error(ex, "calculateStopCharge")
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("calculateStopCharge"), sysErrorParameters.sysErrorState.SystemLevelFault, sysErrorParameters.sysErrorSeverity.Critical)
            End Try
        End Using

        Return decTotalStopChargesForLoad
    End Function

    ''' <summary>
    ''' Calculates the total pick charge for the load
    ''' The caller must determine which fees are marked as overridden. 
    ''' This method only selects fees where the BookFeesOverRidden = False.
    ''' </summary>
    ''' <param name="oBookRevs"></param>
    ''' <param name="CarrierCost"></param>
    ''' <param name="Taxable"></param>
    ''' <param name="Message"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function calculatePickCharge(ByRef oBookRevs As DataTransferObjects.BookRevenue(), ByVal CarrierCost As Decimal, ByVal Taxable As Boolean, ByRef Message As String) As Decimal
        Dim decTotalPickChargesForLoad As Decimal = 0
        Using Logger.StartActivity("calculatePickCharge(BookRevs: {BookRevs}, CarrierCost: {CarrierCost}, Taxable: {Taxable}, Message: {Message}", oBookRevs, CarrierCost, Taxable, Message)



            'Calculation Business Rules
            'Select Pick Fees based on Accessorial Codes 3 to 8
            'Historical Business Rules Compatibility
            'NOTE: we use enumerators for Variable Code and other values the numbers are for example an are subject to change
            '---------------------------------------------------------
            'Determine Calculation Formula based on FeeVariableCode Enum
            '   Legacy Pick codes are between Pick1Flat (17) and Pick6Flat(22) which represent pick 1 to 6
            '   We now also support upto Pick10Flat (26)
            '   If the BookPickupStopNumber value + (Pick1Flat - 1) is an exact match with a configured Variable Code use that Variable Code to determine the formula
            '   If the BookStopNo value + (Pick1Flat - 1) is greater than the highest configured Variable Code use the highest Variable Code to determine the formula
            '   If the BookStopNo value + (Pick1Flat - 1) is less than the highest configured Variable Code use the next highest Variable Code to determien the formula
            '---------------------------------------------------------
            'New 7.0 Business Rules Accessorial Codes 3 to 8 are reserved for Legacy calculations 
            '   to use new pick fee calcuations a custome Accessorial code must be used.
            '---------------------------------------------------------
            'Legacy Pick Fees are always allocated by Origin ination so the FeeAllocationType is ignored
            '---------------------------------------------------------
            'Legacy Pick Fees are always Unique so FeeCalcType must always be Unique = 2 'Override Duplicate Fees At Order Level
            'The caller must determine if the Lane or carrire fees are to be marked as overridden
            'This method only slects fees where the BookFeesOverRidden = False


            Dim intMinAccessorialCode As Integer = 3
            Dim intMaxAccessorialCode As Integer = 8
            Dim lProcessed As New List(Of Integer)
            Try
                For Each oBRev In oBookRevs.OrderBy(Function(x) x.BookPickupStopNumber)
                    Dim inBookPickNo As Short = oBRev.BookPickupStopNumber
                    Dim intPickBookControl As Integer = oBRev.BookControl
                    If lProcessed.Contains(intPickBookControl) Then Continue For 'Skip to the next book record 
                    Dim oStopSummaryData = oBookRevs.Where(Function(x) x.BookPickupStopNumber = inBookPickNo).ToArray()
                    lProcessed.AddRange(oStopSummaryData.Select(Function(X) X.BookControl))
                    Dim oBookFeesByPick As List(Of BookFee) = (From d In oStopSummaryData From f In d.BookFees Where (f.BookFeesAccessorialCode >= intMinAccessorialCode And f.BookFeesAccessorialCode <= intMaxAccessorialCode) And (f.BookFeesVariableCode >= Utilities.FeeVariableCode.Pick1Flat And f.BookFeesVariableCode <= Utilities.FeeVariableCode.Pick10Flat) And f.BookFeesTaxable = Taxable Select f).ToList()
                    If oBookFeesByPick Is Nothing OrElse oBookFeesByPick.Count < 1 Then Continue For 'Skip to the next book record
                    'count the number of shared stops
                    Dim intStopSharedStops As Integer = oStopSummaryData.Count()
                    'Sum the total weight for this stop (default = 1)
                    Dim dblTotalWeightByPick As Double = oStopSummaryData.Sum(Function(x) x.BookTotalWgt)
                    If dblTotalWeightByPick = 0 Then dblTotalWeightByPick = 1
                    Dim dblMaxBookPickWeight As Double = oStopSummaryData.Max(Function(x) x.BookTotalWgt)
                    Dim intPickMaxWeightBookControl As Integer = (From d In oStopSummaryData Where d.BookTotalWgt = dblMaxBookPickWeight Select d.BookControl).FirstOrDefault()
                    If dblMaxBookPickWeight = 0 Then dblMaxBookPickWeight = 1
                    'Get the Stop Fee Variable Rate
                    Dim intPickVariableCode As Integer = If(inBookPickNo = 0, Utilities.FeeVariableCode.Pick1Flat, inBookPickNo + (Utilities.FeeVariableCode.Pick1Flat - 1))
                    'Calculate the highest possible stop charge
                    Dim decMaxPickFeesCharged As Decimal
                    'get the lowest variable that requires a stop charge
                    Dim intMinVariableCode As Integer = oBookFeesByPick.Select(Function(x) x.BookFeesVariableCode).Min()
                    'if this stop is lower than the smallest stop charge variable we use zero for the stop charge
                    If intPickVariableCode >= intMinVariableCode Then
                        Dim oPickFees As List(Of BookFee) = (From d In oBookFeesByPick Where d.BookFeesVariableCode = intPickVariableCode And d.BookFeesOverRidden = False Select d).ToList()
                        Dim oPickFeeUsed As New DataTransferObjects.BookFee
                        If Not oPickFees Is Nothing AndAlso oPickFees.Count > 0 Then
                            oPickFeeUsed = oPickFees.OrderByDescending(Function(x) x.BookFeesMinimum).FirstOrDefault()
                        Else
                            'Try to use the next highest variable code to get the maximum charge
                            oPickFees = (From d In oBookFeesByPick Where
                            (d.BookFeesVariableCode > intPickVariableCode) _
                            And d.BookFeesOverRidden = False Order By d.BookFeesVariableCode Select d).ToList()
                            If Not oPickFees Is Nothing AndAlso oPickFees.Count > 0 Then
                                oPickFeeUsed = oPickFees.OrderBy(Function(x) x.BookFeesVariableCode).ThenByDescending(Function(x) x.BookFeesMinimum).FirstOrDefault()
                            Else
                                'Use the maximum charge for the highest variable code
                                oPickFees = (From d In oBookFeesByPick Where
                                d.BookFeesVariableCode >= Utilities.FeeVariableCode.Pick1Flat _
                                And d.BookFeesVariableCode <= Utilities.FeeVariableCode.Pick10Flat _
                                And d.BookFeesOverRidden = False Select d).ToList()
                                If Not oPickFees Is Nothing AndAlso oPickFees.Count > 0 Then
                                    oPickFeeUsed = oPickFees.OrderByDescending(Function(x) x.BookFeesVariableCode).ThenByDescending(Function(x) x.BookFeesMinimum).FirstOrDefault()
                                End If
                            End If
                        End If
                        If Not oPickFeeUsed Is Nothing Then
                            decMaxPickFeesCharged = oPickFeeUsed.BookFeesMinimum
                            Dim actualAccessorialCode As Integer = oPickFeeUsed.BookFeesAccessorialCode
                            Dim actualVariableCode As Integer = oPickFeeUsed.BookFeesVariableCode
                            If decMaxPickFeesCharged <= 0 Then Return 0 'no charge
                            Dim decPickFee As Decimal = 0
                            Dim decTotalPickCharges As Decimal = 0
                            'Loop through each item in the stops collection 
                            'calculate the pick fee and add them together to determine the total pick charges for this pickup location
                            Dim oThisBookFee As New DataTransferObjects.BookFee
                            Dim oMaxBookWgtFee As DataTransferObjects.BookFee
                            For Each s In oStopSummaryData
                                decPickFee = Math.Round(decMaxPickFeesCharged * (If(s.BookTotalWgt = 0, 1, s.BookTotalWgt) / If(dblTotalWeightByPick = 0, 1, dblTotalWeightByPick)), 2)
                                decTotalPickCharges += decPickFee
                                Dim oBookStopFees = (From f In s.BookFees
                                                     Where f.BookFeesAccessorialCode = actualAccessorialCode _
                                                       And f.BookFeesVariableCode = actualVariableCode _
                                                       And f.BookFeesTaxable = Taxable
                                                     Order By
                                                 f.BookFeesAccessorialFeeTypeControl Descending,
                                                 f.BookFeesMinimum Descending
                                                     Select f).ToList()
                                'Update the FeeValue with the calculated cost
                                If Not oBookStopFees Is Nothing AndAlso oBookStopFees.Count > 0 Then
                                    Dim blnMatchFound As Boolean = False
                                    For Each f In oBookStopFees
                                        If Not blnMatchFound And f.BookFeesAccessorialCode = actualAccessorialCode Then
                                            blnMatchFound = True
                                            With f
                                                'Update the first matching record with the selected book fee data 
                                                'all book fees on the pick should match
                                                .BookFeesValue = decPickFee
                                                .BookFeesAccessorialFeeAllocationTypeControl = Utilities.FeeAllocationType.Origin 'Always Origin for legacy pick fees
                                                .BookFeesAccessorialFeeCalcTypeControl = Utilities.FeeCalcType.Unique 'Always Unique for legacy pick fees
                                                .BookFeesMinimum = oPickFeeUsed.BookFeesMinimum
                                                .BookFeesTarBracketTypeControl = oPickFeeUsed.BookFeesTarBracketTypeControl
                                                .BookFeesVariable = oPickFeeUsed.BookFeesVariable
                                                .BookFeesVariableCode = oPickFeeUsed.BookFeesVariableCode
                                                .TrackingState = TrackingInfo.Updated
                                            End With
                                            oThisBookFee = f
                                        Else
                                            f.BookFeesOverRidden = True
                                            f.BookFeesValue = 0
                                            f.TrackingState = TrackingInfo.Updated
                                        End If
                                    Next
                                Else
                                    'add the fee
                                    oThisBookFee = oPickFeeUsed.Clone()
                                    With oThisBookFee
                                        .BookFeesBookControl = s.BookControl
                                        .BookFeesControl = 0
                                        .BookFeesValue = decPickFee
                                        .BookFeesAccessorialFeeAllocationTypeControl = Utilities.FeeAllocationType.Origin
                                        .BookFeesAccessorialFeeCalcTypeControl = Utilities.FeeCalcType.Unique
                                        .BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Order
                                        .BookFeesTaxable = Taxable
                                        .BookFeesUpdated = New Byte() {}
                                        .TrackingState = TrackingInfo.Created
                                    End With
                                    s.BookFees.Add(oThisBookFee)
                                End If
                                If s.BookControl = intPickMaxWeightBookControl Then oMaxBookWgtFee = oThisBookFee
                            Next
                            If decTotalPickCharges <> decMaxPickFeesCharged AndAlso Not oMaxBookWgtFee Is Nothing Then
                                'Adjust for Rounding Errors
                                oMaxBookWgtFee.BookFeesValue += (decMaxPickFeesCharged - decTotalPickCharges)
                            End If
                        End If
                    End If
                    decTotalPickChargesForLoad += decMaxPickFeesCharged
                Next
            Catch ex As System.Data.SqlClient.SqlException
                Logger.Error(ex, "calculatePickCharge")
                Throw
            Catch ex As InvalidOperationException
                Logger.Error(ex, "calculatePickCharge")

            Catch ex As Exception
                Logger.Error(ex, "calculatePickCharge")
                throwUnExpectedFaultException(ex, buildProcedureName("calculatePickCharge"), sysErrorParameters.sysErrorState.SystemLevelFault, sysErrorParameters.sysErrorSeverity.Critical)
            End Try
        End Using
        Return decTotalPickChargesForLoad
    End Function

    ''' <summary>
    ''' Calculates the total fuel charge for the load
    ''' The caller must determine which fees are marked as overridden
    ''' The method only selectes fees where the BookFeesOverRidden = False
    ''' </summary>
    ''' <param name="oBookRevs"></param>
    ''' <param name="CarrierCost"></param>
    ''' <param name="CarrierControl"></param>
    ''' <param name="Taxable"></param>
    ''' <param name="CarrTarControl"></param>
    ''' <param name="Message"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.2.003 on 07/08/2022 fix microsoft issues with default values of variables
    ''' Modified by RHR for v-8.5.2.005 on 08/09/2022 added logic to prevent updates to fuel after delivered except when the
    '''         Fuel is modified by the carrier (Pending Fees logic)
    '''         Note: oBookRevs is not always the database copy it normally has been modified by the carrier tariff
    '''             for this reason we must read the correct data from the bookfees table when applying existing fuel rates
    ''' </remarks>
    Protected Function calculateFuelCharge(ByRef oBookRevs As DataTransferObjects.BookRevenue(), ByVal CarrierCost As Decimal, ByVal CarrierControl As Integer, ByVal Taxable As Boolean, ByVal CarrTarControl As Integer, ByVal CarrTarEquipControl As Integer, ByRef Message As String) As Decimal
        Dim decTotalFuelCharges As Decimal = 0
        Using Logger.StartActivity("NGLBookRevenueData.calculateFuelCharge - CarrierCost: {CarrierCost}, CarrierControl:{CarrierControl}, Taxable: {Taxable}, CarTarControl: {CarrTarControl}, CarrTarEquipControl: {CarrTarEquipControl}", CarrierCost, CarrierControl, Taxable, CarrTarControl, CarrTarEquipControl)        'Calculation Business Rules
            'Select Fees based on Accessorial Codes 2 or 9
            'Historical Business Rules Compatibility
            'NOTE: we use enumerators for Variable Code and other values the numbers are for example an are subject to change
            '---------------------------------------------------------
            'Determine Calculation Formula based on FeeVariableCode Enum
            '   Because the allocation type is by load we always use the totals for the entire load.
            '   LoadWeight = 1 'Multiply Variable Value By Total Weight For Load
            '   NumberPallets = 2 'Multiply Variable Value By Number of Pallets For Load
            '   CarrierCost = 3 'Multiply Variable Value By Total Carrier Cost For Load
            '   PerMile = 4 'Multiply Variable Value By Total Number of Miles For Load
            '   ByVolume = 5 'Multiply Variable Value By Total Volume or Cubes For Load
            '   ByQuantity = 6 'Multiply Variable Value By Total Quantity or Cases For Load

            '---------------------------------------------------------
            'New 7.0 Business Rules Accessorial Codes 2 and 9 are reserved for Legacy calculations 
            '   to use new  fee calcuation logic a custome Accessorial code must be used.
            '---------------------------------------------------------
            'Legacy Fuel Fees are always allocated by Load(4) so the FeeAllocationType is ignored
            '   We are using the BracketType to determine how the allocation is calculated:
            '       Pallets = 1 'Break or Allocate by number of pallets
            '       Volume = 2 'Break or Allocate by number of Cubes
            '       Quantity = 3 'Break or Allocate by number of Cases
            '       Lbs = 4 'Break or Allocate by number of Lbs
            '       Cwt = 5 'Break or Allocate by number of Cwt
            '       Distance = 6 'Break or Allocate by Distance like Per Kilometer
            '---------------------------------------------------------
            'Legacy Fuel Fees are always Unique so FeeCalcType must always be Unique = 2 'Override Duplicate Fees At Order Level
            'The caller must determine if the Lane or carrire fees are to be marked as overridden
            'This method only slects fees where the BookFeesOverRidden = False

            Dim intLowAccessorialCode As Integer = 2
            Dim intHighAccessorialCode As Integer = 9
            Try
                Dim dblTotalWgt As Double = oBookRevs.Sum(Function(x) x.BookTotalWgt)
                If dblTotalWgt = 0 Then
                    dblTotalWgt = 1
                    Logger.Information("[{BookCarrTarName} v{BookCarrTarRevisionNumber}]({BookCarrTarEquipName}) calculateFuelCharge - Total Weight is 0, setting to 1", oBookRevs(0).BookCarrTarName, oBookRevs(0).BookCarrTarRevisionNumber, oBookRevs(0).BookCarrTarEquipName)
                End If
                Dim dblTotalMiles As Double = oBookRevs.Sum(Function(x) x.BookMilesFrom)
                If dblTotalMiles = 0 Then
                    dblTotalMiles = 1
                    Logger.Information("[{BookCarrTarName} v{BookCarrTarRevisionNumber}]({BookCarrTarEquipName}) calculateFuelCharge -  Total Miles is 0, setting to 1", oBookRevs(0).BookCarrTarName, oBookRevs(0).BookCarrTarRevisionNumber, oBookRevs(0).BookCarrTarEquipName)
                End If
                Dim dblTotalPlts As Double = oBookRevs.Sum(Function(x) x.BookTotalPL)
                If dblTotalPlts = 0 Then
                    dblTotalPlts = 1
                    Logger.Information("[{BookCarrTarName} v{BookCarrTarRevisionNumber}]({BookCarrTarEquipName}) calculateFuelCharge -  Total Pallets is 0, setting to 1", oBookRevs(0).BookCarrTarName, oBookRevs(0).BookCarrTarRevisionNumber, oBookRevs(0).BookCarrTarEquipName)
                End If
                Dim dblTotalQty As Double = oBookRevs.Sum(Function(x) x.BookTotalCases)
                If dblTotalQty = 0 Then
                    dblTotalQty = 1
                    Logger.Information("[{BookCarrTarName} v{BookCarrTarRevisionNumber}]({BookCarrTarEquipName}) calculateFuelCharge -  Total Quantity is 0, setting to 1", oBookRevs(0).BookCarrTarName, oBookRevs(0).BookCarrTarRevisionNumber, oBookRevs(0).BookCarrTarEquipName)
                End If
                Dim dblTotalVol As Double = oBookRevs.Sum(Function(x) x.BookTotalCube)
                If dblTotalVol = 0 Then
                    dblTotalVol = 1
                    Logger.Information("[{BookCarrTarName} v{BookCarrTarRevisionNumber}]({BookCarrTarEquipName}) calculateFuelCharge -  Total Volume is 0, setting to 1", oBookRevs(0).BookCarrTarName, oBookRevs(0).BookCarrTarRevisionNumber, oBookRevs(0).BookCarrTarEquipName)
                End If
                Logger.Information("calculateTotalFuel - determineTotalsResults TotalWgt: {TotalWgt}, TotalMiles: {TotalMiles}, TotalPlts: {TotalPlts}, TotalQty: {TotalQty}, TotalVol: {TotalVol} ", dblTotalWgt, dblTotalMiles, dblTotalPlts, dblTotalQty, dblTotalVol)

                Dim dblMaxBookStopWeight As Double = oBookRevs.Max(Function(x) x.BookTotalWgt)
                Dim intMaxWeightBookControl As Integer = (From d In oBookRevs Where d.BookTotalWgt = dblMaxBookStopWeight Select d.BookControl).FirstOrDefault()

                Logger.Information("Searching BookRevs where d.BookTotalWgt = {dblMaxBookStopWeight} resulting in BookControl: {BookControl} ", dblMaxBookStopWeight, intMaxWeightBookControl)


                If dblMaxBookStopWeight = 0 Then dblMaxBookStopWeight = 1
                Dim LastStopBookControl As Integer = (From d In oBookRevs Order By d.BookStopNo Descending Select d.BookControl).FirstOrDefault()
                Dim STATE As String = (From d In oBookRevs Order By d.BookStopNo Descending Select d.BookDestState).FirstOrDefault()
                Dim EffectiveDate As Date? = (From d In oBookRevs Order By d.BookStopNo Descending Select d.BookDateLoad).FirstOrDefault()
                If (Not EffectiveDate.HasValue) Then EffectiveDate = Date.Now()
                Dim FeesVariable As Decimal
                Dim AccessorialCode As Integer
                Dim sBookSHID As String = oBookRevs(0).BookSHID
                Dim sCNS As String = oBookRevs(0).BookConsPrefix
                ' * Begin Modified by RHR for v-8.5.2.005 on 08/09/2022 added logic to prevent updates to fuel after delivered except when the *
                Dim existingBooks As New List(Of LTS.Book)
                Dim existingBookFees As New List(Of LTS.BookFee)

                Using db As New NGLMasBookDataContext(ConnectionString)
                    Try
                        If Not String.IsNullOrWhiteSpace(sBookSHID) Then
                            'Use Injector for easy testing. Override essentially a static DB
                            'existingBooks = Injector.Instance.GetObject(Of List(Of LTS.Book))("calc_existingBooks",
                            '                                                                  Function() db.Books.Where(Function(x) x.BookSHID = sBookSHID AndAlso x.BookCarrierControl = CarrierControl).ToList())

                            existingBooks = db.Books.Where(Function(x) x.BookSHID = sBookSHID AndAlso x.BookCarrierControl = CarrierControl).ToList()      'Old line
                            Logger.Information("NGLBookRevenueData.calculateFuelCharge(SHID) - Tariff: {BookCarrTarName}, Revision: {BookCarrTarRevisionNumber}, Equipment: {BookCarrTarEquipName} Find Existing Books {existingBooks}", oBookRevs(0).BookCarrTarName, oBookRevs(0).BookCarrTarRevisionNumber, oBookRevs(0).BookCarrTarEquipName, existingBooks.Count)
                        End If

                        If existingBooks Is Nothing OrElse existingBooks.Count() <> oBookRevs.Count() Then
                            existingBooks = db.Books.Where(Function(x) x.BookConsPrefix = sCNS AndAlso x.BookCarrierControl = CarrierControl).ToList()
                            Logger.Information("NGLBookRevenueData.calculateFuelCharge - Found ({BookCount}) Existing Books Byte CNS Prefix ({CNS}) and Carrier: {CarrierControl} ", existingBooks?.Count, sCNS, CarrierControl)
                        End If
                        Dim blnDelivered As Boolean = False

                        If Not existingBooks Is Nothing AndAlso existingBooks.Count() = oBookRevs.Count() Then
                            Logger.Information("NGLBookRevenueData.calculateFuelCharge - Found ({BookCount}) Existing Books By SHID ({SHID}) or ConsPrefix ({CNS}) and Carrier: {CarrierControl} ", existingBooks?.Count, sBookSHID, sCNS, CarrierControl)
                            For Each iBook As LTS.Book In existingBooks
                                If iBook.BookCarrActDate.HasValue Then
                                    blnDelivered = True
                                    Exit For
                                End If
                            Next
                            If blnDelivered Then
                                Logger.Information("NGLBookRevenueData.calculateFuelCharge - Book {BookControl} has been delivered on {BookCarrActDate} and setting blnDelivered to {blnDelivered}, iterating through bookRev objects", existingBooks(0).BookControl, existingBooks(0).BookCarrActDate, blnDelivered)
                                decTotalFuelCharges = 0
                                For Each oBRev In oBookRevs
                                    Dim intBookControl As Integer = oBRev.BookControl
                                    'Get the active fuel fees if they exist
                                    'existingBookFees = Injector.Instance.GetObject(Of List(Of LTS.BookFee))("calc_existingBookFees",
                                    '                                                                        Function() db.BookFees.Where(Function(x) x.BookFeesBookControl = intBookControl And (x.BookFeesAccessorialCode = intLowAccessorialCode Or x.BookFeesAccessorialCode = intHighAccessorialCode) And (x.BookFeesOverRidden = 0)).ToList())

                                    Logger.Information("NGLBookRevenueData.calculateFuelCharge [DELIVERED] - Found ({BookFeeCount}) Existing Book Fees for BookControl: {BookControl} and Accessorial Codes {LowAccessorialCode} and {HighAccessorialCode} and Overridden = 0 on {@BookRev}", existingBookFees?.Count, intBookControl, intLowAccessorialCode, intHighAccessorialCode, oBRev)

                                    existingBookFees = db.BookFees.Where(Function(x) x.BookFeesBookControl = intBookControl And (x.BookFeesAccessorialCode = intLowAccessorialCode Or x.BookFeesAccessorialCode = intHighAccessorialCode) And (x.BookFeesOverRidden = 0)).ToList()

                                    decTotalFuelCharges = decTotalFuelCharges + existingBookFees.Sum(Function(x) x.BookFeesValue)
                                    Logger.Information("NGLBookRevenueData.calculateFuelCharge [DELIVERED] - Total Fuel Charges for BookControl: {BookControl} is {TotalFuelCharges} based on summing existing book fees from the databse", intBookControl, decTotalFuelCharges)

                                    If Not existingBookFees Is Nothing AndAlso existingBookFees.Count() > 0 Then
                                        Logger.Information("NGLBookRevenueData.calculateFuelCharge [DELIVERED] - if exsitingBookFees not nothing and count > 0")

                                        ' update the data with the active fuel fees
                                        'Get the processing data's Fuel Fees
                                        Dim oFuelFees = (From f In oBRev.BookFees
                                                         Where (f.BookFeesAccessorialCode = intLowAccessorialCode Or f.BookFeesAccessorialCode = intHighAccessorialCode) _
                                                               And f.BookFeesTaxable = Taxable _
                                                               And f.BookFeesOverRidden = False
                                                         Order By
                                                         f.BookFeesAccessorialFeeTypeControl Descending,
                                                         f.BookFeesAccessorialCode Descending,
                                                         f.BookFeesVariableCode Descending,
                                                         f.BookFeesVariable Descending,
                                                         f.BookFeesMinimum Descending
                                                         Select f).ToList()
                                        Logger.Information("NGLBookRevenueData.calculateFuelCharge [DELIVERED] - Found ({FuelFeeCount}) Fuel Fees for BookControl: {BookControl} and Accessorial Codes {LowAccessorialCode} and {HighAccessorialCode} and Taxable = {Taxable} and Overridden = 0", oFuelFees?.Count, intBookControl, intLowAccessorialCode, intHighAccessorialCode, Taxable)

                                        If Not oFuelFees Is Nothing AndAlso oFuelFees.Count > 0 Then
                                            Dim blnMatchFound As Boolean = False
                                            Logger.Information("NGLBookRevenueData.calculateFuelCharge [DELIVERED] - if oFuelFees not nothing and count > 0")
                                            For Each f In oFuelFees
                                                Logger.Information("NGLBookRevenueData.calculateFuelCharge - Find existing book fee for BookControl: {BookControl} and Accessorial Code: {AccessorialCode}", intBookControl, f.BookFeesAccessorialCode)
                                                Dim matchingBookFee = existingBookFees.Where(Function(x) x.BookFeesAccessorialCode = f.BookFeesAccessorialCode).FirstOrDefault()
                                                If Not blnMatchFound AndAlso matchingBookFee IsNot Nothing Then
                                                    blnMatchFound = True
                                                    Logger.Information("NGLBookRevenueData.calculateFuelCharge [DELIVERED] - Found matching book fee for BookControl: {BookControl} and Accessorial Code: {AccessorialCode}", intBookControl, f.BookFeesAccessorialCode)
                                                    With f
                                                        'Update the first matching record with the selected fuel fee data 
                                                        'all fuel fees on the load should match
                                                        .BookFeesValue = matchingBookFee.BookFeesValue
                                                        .BookFeesAccessorialFeeAllocationTypeControl = matchingBookFee.BookFeesAccessorialFeeAllocationTypeControl
                                                        .BookFeesAccessorialFeeCalcTypeControl = matchingBookFee.BookFeesAccessorialFeeCalcTypeControl
                                                        .BookFeesMinimum = matchingBookFee.BookFeesMinimum
                                                        .BookFeesTarBracketTypeControl = matchingBookFee.BookFeesTarBracketTypeControl
                                                        .BookFeesVariable = matchingBookFee.BookFeesVariable
                                                        .BookFeesVariableCode = matchingBookFee.BookFeesVariableCode
                                                        .TrackingState = TrackingInfo.Updated
                                                    End With
                                                Else

                                                    f.BookFeesOverRidden = True
                                                    f.BookFeesValue = 0
                                                    f.TrackingState = TrackingInfo.Updated

                                                    Logger.Information("NGLBookRevenueData.calculateFuelCharge [DELIVERED] - No Matching BookFee Found Set BookFee Overridden to True and Value to 0 for BookControl: {BookControl} and Accessorial Code: {AccessorialCode}", intBookControl, f.BookFeesAccessorialCode)
                                                End If
                                            Next
                                        Else
                                            'add the fees
                                            Logger.Information("NGLBookRevenueData.calculateFuelCharge [DELIVERED] - if oFuelFees is nothing or count = 0")
                                            For Each existf As LTS.BookFee In existingBookFees
                                                Dim oThisBookFee As DataTransferObjects.BookFee = NGLBookFeeData.selectDTOData(existf)
                                                oThisBookFee.TrackingState = TrackingInfo.Updated
                                                oBRev.BookFees.Add(oThisBookFee)
                                            Next
                                        End If
                                    End If

                                Next
                                Return decTotalFuelCharges
                            End If
                        End If
                    Catch ex As Exception
                        'do nothing
                        Logger.Error(ex, "Exception in get Fuel Charges")
                    End Try
                End Using
                ' * End Modified by RHR for v-8.5.2.005 on 08/09/2022 added logic to prevent updates to fuel after delivered except when the *

                Dim iCode15 = 15
                'In the previous version (6.0.x) the fuel fees were always the same for all orders on the load.
                'Now it is possible to overload the carrier fees on one or more orders. For this reason we always
                'select the fee with the following sort order: 
                '   1. Highest Accessorial Fee Type Control [BookFeesAccessorialFeeTypeControl] (1 = Tariff, 2 = Lane, 3 = Order)
                '   2. Highest Accessorial Code [BookFeesAccessorialCode] (2 = Fuel SurCharge Percent or 9 = Fuel SurCharge Per Mile) the rate per mile is selected over percent.
                '   3. Highest Variable Code [BookFeesVariableCode] it is possible for the users to change how the costs are calculated so we start with the highest number to support the previous business rule.
                '   4. Highest Variable Value [BookFeesVariable] value multiplied by the order metrix defined by [BookFeesVariableCode]
                '   5. Highest Minimum value [BookFeesMinimum]

                Dim oFuelFee As DataTransferObjects.BookFee = (From d In oBookRevs
                                                               From f In d.BookFees
                                                               Where (f.BookFeesAccessorialCode = intHighAccessorialCode Or f.BookFeesAccessorialCode = intLowAccessorialCode Or f.BookFeesAccessorialCode = intHighAccessorialCode) _
                                                                     And f.BookFeesTaxable = Taxable _
                                                                     And f.BookFeesOverRidden = False
                                                               Order By
                                                               f.BookFeesAccessorialFeeTypeControl Descending,
                                                               f.BookFeesAccessorialCode Descending,
                                                               f.BookFeesVariableCode Descending,
                                                               f.BookFeesVariable Descending,
                                                               f.BookFeesMinimum Descending Select f).FirstOrDefault()



                Logger.Information("Check if oFuelFee is nothing or oFuelFee.BookFeesVariableCode ({BookFeesVariableCode}) is greater than Quantity({1})", oFuelFee?.BookFeesVariableCode, 6) 'Utilities.FeeVariableCode.ByQuantity as integer
                If oFuelFee Is Nothing OrElse oFuelFee.BookFeesVariableCode > Utilities.FeeVariableCode.ByQuantity Then
                    Logger.Information("No oFuelFee found Or oFuelFee.BookFeesVariableCode ({BookFeesVariableCode}) Is greater than {1}", oFuelFee?.BookFeesVariableCode, Utilities.FeeVariableCode.ByQuantity)

                    Return 0 'no fuel
                End If
                'If this is a Tariff fee we check if the Fuel Addendum has changed and if the formula needs to 
                'be updated.
                Logger.Information("Check If oFuelFee.BookFeesAccessorialFeeTypeControl ({BookFeesAccessorialFeeTypeControl}) = Utilities.AccessorialFeeType.Tariff ({1})", oFuelFee.BookFeesAccessorialFeeTypeControl, Utilities.AccessorialFeeType.Tariff)
                If oFuelFee.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Tariff Then
                    'update the accessorial data using the fuel addendum we do not adjust the minimum
                    FeesVariable = oFuelFee.BookFeesVariable
                    AccessorialCode = oFuelFee.BookFeesAccessorialCode
                    Logger.Information("BookFeeAccessorialType Is Tariff, setting FeesVariable = {FeesVariable}, AccessorialCode = {AccessorialCode}", FeesVariable, AccessorialCode)
                    If (LastStopBookControl = 0) Then
                        Logger.Information("NGLBookRevenueData.calculateFuelCharge - LastBookStopControl = 0 so searching CarrierFuelAddendum With CarrierControl: {CarrierControl}", CarrierControl)
                        Dim oResults As LTS.spGetFuelSurchargeResult = NGLCarrierFuelAddendumObjData.GetFuelSurCharge(CarrierControl,
                                                                                                                      CarrTarControl,
                                                                                                                      CarrTarEquipControl,
                                                                                                                      STATE,
                                                                                                                      EffectiveDate)

                        FeesVariable = If(oResults.FuelSurcharge, FeesVariable)
                        Logger.Information("FeesVariable = {FeesVariable} because oResults.FuelSurchage = {FuelSurcharge}", FeesVariable, oResults.FuelSurcharge)
                    Else
                        Logger.Information("LastStopBookControl <> 0, GetFuelChargeByLastStop with LastStopBookControl: {LastStopBookControl}", LastStopBookControl)
                        GetFuelChargeByLastStop(LastStopBookControl,
                                                CarrierControl,
                                                CarrTarControl,
                                                CarrTarEquipControl,
                                                FeesVariable,
                                                AccessorialCode,
                                                Message)

                    End If

                    oFuelFee.BookFeesVariable = FeesVariable
                    oFuelFee.BookFeesAccessorialCode = AccessorialCode
                    Logger.Information("oFuelFee.BookFeesVariable = {BookFeesVariable}, oFuelFee.BookFeesAccessorialCode = {BookFeesAccessorialCode}", oFuelFee.BookFeesVariable, oFuelFee.BookFeesAccessorialCode)
                    If AccessorialCode = 2 Then
                        Logger.Information("AccessorialCode = 2, Setting BookFeesVariableCode to CarrierCost")
                        oFuelFee.BookFeesVariableCode = Utilities.FeeVariableCode.CarrierCost
                    Else
                        Logger.Information("AccessorialCode <> 2,Setting BookFeesVariableCode to PerMile")
                        oFuelFee.BookFeesVariableCode = Utilities.FeeVariableCode.PerMile
                    End If
                End If
                Logger.Information("Checking TypeOf Of bookfeesvariablecode {BookFeesVariableCode} to a utility value ", oFuelFee.BookFeesVariableCode)
                Select Case oFuelFee.BookFeesVariableCode
                    Case Utilities.FeeVariableCode.LoadWeight
                        decTotalFuelCharges = Math.Round(oFuelFee.BookFeesVariable * dblTotalWgt, 2)
                        Logger.Information("Setting decTotalFuelCharges to {BookFeesVariable} * {dblTotalWgt} = {decTotalFuelCharges}", oFuelFee.BookFeesVariable, dblTotalWgt, decTotalFuelCharges)
                    Case Utilities.FeeVariableCode.NumberPallets
                        decTotalFuelCharges = Math.Round(oFuelFee.BookFeesVariable * dblTotalPlts, 2)
                        Logger.Information("Setting decTotalFuelCharges to {BookFeesVariable} * {dblTotalPlts} = {decTotalFuelCharges}", oFuelFee.BookFeesVariable, dblTotalPlts, decTotalFuelCharges)
                    Case Utilities.FeeVariableCode.CarrierCost
                        decTotalFuelCharges = Math.Round(oFuelFee.BookFeesVariable * CarrierCost, 2)
                        Logger.Information("Setting decTotalFuelCharges to {BookFeesVariable} * {CarrierCost} = {decTotalFuelCharges}", oFuelFee.BookFeesVariable, CarrierCost, decTotalFuelCharges)
                    Case Utilities.FeeVariableCode.PerMile
                        decTotalFuelCharges = Math.Round(oFuelFee.BookFeesVariable * dblTotalMiles, 2)
                        Logger.Information("Setting decTotalFuelCharges to {BookFeesVariable} * {dblTotalMiles} = {decTotalFuelCharges}", oFuelFee.BookFeesVariable, dblTotalMiles, decTotalFuelCharges)
                    Case Utilities.FeeVariableCode.ByVolume
                        decTotalFuelCharges = Math.Round(oFuelFee.BookFeesVariable * dblTotalVol, 2)
                        Logger.Information("Setting decTotalFuelCharges to {BookFeesVariable} * {dblTotalVol} = {decTotalFuelCharges}", oFuelFee.BookFeesVariable, dblTotalVol, decTotalFuelCharges)
                    Case Utilities.FeeVariableCode.ByQuantity
                        decTotalFuelCharges = Math.Round(oFuelFee.BookFeesVariable * dblTotalQty, 2)
                        Logger.Information("Setting decTotalFuelCharges to {BookFeesVariable} * {dblTotalQty} = {decTotalFuelCharges}", oFuelFee.BookFeesVariable, dblTotalQty, decTotalFuelCharges)
                    Case Else
                        Logger.Information("Setting decTotalFuelCharges to 0 because BookFeesVariableCode is not a valid value")
                        Return 0 'no fuel
                End Select
                'this corrects the bug where previous code assigned the minimum fuel charge to
                'the order after allocation when it should be assinged to the whole load.
                Logger.Information("Checking If decTotalFuelCharges ({decTotalFuelCharges}) < oFuelFee.BookFeesMinimum ({BookFeesMinimum})", decTotalFuelCharges, oFuelFee.BookFeesMinimum)


                If decTotalFuelCharges < oFuelFee.BookFeesMinimum Then
                    Logger.Information("Setting decTotalFuelCharges ({decTotalFuelCharges} to {BookFeesMinimum} because it is less than the minimum", decTotalFuelCharges, oFuelFee.BookFeesMinimum)
                    decTotalFuelCharges = oFuelFee.BookFeesMinimum

                End If
                Dim decFuelAllocatedTotal As Decimal
                Dim oMaxBookWgtFee As DataTransferObjects.BookFee
                'Modified by RHR for v-8.5.2.003 on 07/08/2022 fix microsoft issues with default values of variables
                'Bug fix v-8.5.3 07/07/2022 variable scope
                Dim decOrderFuelCost As Decimal
                For Each oBRev In oBookRevs
                    Dim intBookControl As Integer = oBRev.BookControl
                    'Calculate the allocated fuel cost for this order
                    decOrderFuelCost = 0
                    Select Case oFuelFee.BookFeesTarBracketTypeControl
                        Case Utilities.BracketType.Pallets
                            decOrderFuelCost = DataTransformation.AllocateCostByPercentage(oBRev.BookTotalPL, dblTotalPlts, decTotalFuelCharges) 'Math.Round((If(oBRev.BookTotalPL = 0, 1, oBRev.BookTotalPL) / dblTotalPlts) * decTotalFuelCharges, 2)
                            Logger.Information("Setting decOrderFuelCost to {AllocateCostByPercentage} based on BookTotalPL {BookTotalPL} and dblTotalPlts {dblTotalPlts} and decTotalFuelCharges {decTotalFuelCharges}", decOrderFuelCost, oBRev.BookTotalPL, dblTotalPlts, decTotalFuelCharges)
                        Case Utilities.BracketType.FlatPallet
                            'Modified by RHR for v-8.5.4.002 on 08/25/2023 added FlatPallet logic
                            decOrderFuelCost = DataTransformation.AllocateCostByPercentage(oBRev.BookTotalPL, dblTotalPlts, decTotalFuelCharges) 'Math.Round((If(oBRev.BookTotalPL = 0, 1, oBRev.BookTotalPL) / dblTotalPlts) * decTotalFuelCharges, 2)
                            Logger.Information("Setting decOrderFuelCost to {AllocateCostByPercentage} based on BookTotalPL {BookTotalPL} and dblTotalPlts {dblTotalPlts} and decTotalFuelCharges {decTotalFuelCharges}", decOrderFuelCost, oBRev.BookTotalPL, dblTotalPlts, decTotalFuelCharges)
                        Case Utilities.BracketType.Volume
                            decOrderFuelCost = DataTransformation.AllocateCostByPercentage(oBRev.BookTotalCube, dblTotalVol, decTotalFuelCharges)  'Math.Round((If(oBRev.BookTotalCube = 0, 1, oBRev.BookTotalCube) / dblTotalVol) * decTotalFuelCharges, 2)
                            Logger.Information("Setting decOrderFuelCost to {AllocateCostByPercentage} based on BookTotalCube {BookTotalCube} and dblTotalVol {dblTotalVol} and decTotalFuelCharges {decTotalFuelCharges}", decOrderFuelCost, oBRev.BookTotalCube, dblTotalVol, decTotalFuelCharges)
                        Case Utilities.BracketType.Quantity
                            decOrderFuelCost = DataTransformation.AllocateCostByPercentage(oBRev.BookTotalCases, dblTotalQty, decTotalFuelCharges) 'Math.Round((If(oBRev.BookTotalCases = 0, 1, oBRev.BookTotalCases) / dblTotalQty) * decTotalFuelCharges, 2)
                            Logger.Information("Setting decOrderFuelCost to {AllocateCostByPercentage} based on BookTotalCases {BookTotalCases} and dblTotalQty {dblTotalQty} and decTotalFuelCharges {decTotalFuelCharges}", decOrderFuelCost, oBRev.BookTotalCases, dblTotalQty, decTotalFuelCharges)
                        Case Utilities.BracketType.Lbs
                            decOrderFuelCost = DataTransformation.AllocateCostByPercentage(oBRev.BookTotalWgt, dblTotalWgt, decTotalFuelCharges) 'Math.Round((If(oBRev.BookTotalWgt = 0, 1, oBRev.BookTotalWgt) / dblTotalWgt) * decTotalFuelCharges, 2)
                            Logger.Information("Setting decOrderFuelCost to {AllocateCostByPercentage} based on BookTotalWgt {BookTotalWgt} and dblTotalWgt {dblTotalWgt} and decTotalFuelCharges {decTotalFuelCharges}", decOrderFuelCost, oBRev.BookTotalWgt, dblTotalWgt, decTotalFuelCharges)
                        Case Utilities.BracketType.Cwt
                            decOrderFuelCost = DataTransformation.AllocateCostByPercentage(oBRev.BookTotalWgt, dblTotalWgt, decTotalFuelCharges) 'Math.Round((If(oBRev.BookTotalWgt = 0, 1, oBRev.BookTotalWgt) / dblTotalWgt) * decTotalFuelCharges, 2)
                            Logger.Information("Setting decOrderFuelCost to {AllocateCostByPercentage} based on BookTotalWgt {BookTotalWgt} and dblTotalWgt {dblTotalWgt} and decTotalFuelCharges {decTotalFuelCharges}", decOrderFuelCost, oBRev.BookTotalWgt, dblTotalWgt, decTotalFuelCharges)
                        Case Utilities.BracketType.Distance
                            If oBRev.BookMilesFrom.HasValue AndAlso oBRev.BookMilesFrom > 0 Then

                                decOrderFuelCost = DataTransformation.AllocateCostByPercentage(oBRev.BookMilesFrom.Value, dblTotalMiles, decTotalFuelCharges) 'Math.Round((oBRev.BookMilesFrom.Value / dblTotalMiles) * decTotalFuelCharges, 2)
                                Logger.Information("Setting decOrderFuelCost to {AllocateCostByPercentage} based on BookMilesFrom {BookMilesFrom} and dblTotalMiles {dblTotalMiles} and decTotalFuelCharges {decTotalFuelCharges}", decOrderFuelCost, oBRev.BookMilesFrom, dblTotalMiles, decTotalFuelCharges)
                            End If
                        Case Utilities.BracketType.Even
                            decOrderFuelCost = Math.Round(decTotalFuelCharges / oBookRevs.Length(), 2)
                            Logger.Information("Setting decOrderFuelCost to {decTotalFuelCharges} / {BookRevLength} = {decOrderFuelCost}", decTotalFuelCharges, oBookRevs?.Length(), decOrderFuelCost)
                    End Select
                    decFuelAllocatedTotal += decOrderFuelCost
                    Dim actualAccessorialCode = oFuelFee.BookFeesAccessorialCode
                    Logger.Information("Setting actualAccessorialCode to {BookFeesAccessorialCode} and decFuelAllocatedTotal += decOrderFuelCost ({decOrderFuelCost}", actualAccessorialCode, decOrderFuelCost)
                    'Update the FeeValue with the calculated cost
                    'NOTE:  this logic does not work.  it does not always update the correct fee.
                    'if the fee exists but all the BookFeesOverRidden values are True we need to change one of them to False
                    'This should match the  oFuelFee settings.
                    Dim oThisBookFee As New DataTransferObjects.BookFee
                    'Get the Fuel Fees
                    Dim oFuelFees = (From f In oBRev.BookFees
                                     Where (f.BookFeesAccessorialCode = intLowAccessorialCode Or f.BookFeesAccessorialCode = intHighAccessorialCode) _
                                           And f.BookFeesTaxable = Taxable _
                                           And f.BookFeesOverRidden = False
                                     Order By
                                     f.BookFeesAccessorialFeeTypeControl Descending,
                                     f.BookFeesAccessorialCode Descending,
                                     f.BookFeesVariableCode Descending,
                                     f.BookFeesVariable Descending,
                                     f.BookFeesMinimum Descending
                                     Select f).ToList()
                    Logger.Information("Find existing fees in oBRev.BookFees where matches lowor high accessorial code ")

                    If Not oFuelFees Is Nothing AndAlso oFuelFees.Count > 0 Then
                        'Bug fix v-8.5.3 07/07/2022 variable scope
                        Logger.Information("Checking If oFuelFees Is Not Nothing AndAlso oFuelFees.Count > 0")
                        Dim blnMatchFound As Boolean = False
                        For Each f In oFuelFees
                            Logger.Information("Checking If Not blnMatchFound ({0}) And f.BookFeesAccessorialCode ({1}) = actualAccessorialCode ({2})", blnMatchFound, f.BookFeesAccessorialCode, actualAccessorialCode)
                            If Not blnMatchFound And f.BookFeesAccessorialCode = actualAccessorialCode Then
                                blnMatchFound = True
                                With f
                                    'Update the first matching record with the selected fuel fee data 
                                    'all fuel fees on the load should match
                                    .BookFeesValue = decOrderFuelCost
                                    .BookFeesAccessorialFeeAllocationTypeControl = Utilities.FeeAllocationType.Load 'Always Load For legacy fuel fees
                                    .BookFeesAccessorialFeeCalcTypeControl = Utilities.FeeCalcType.Unique 'Always Unique for legacy fuel fees
                                    .BookFeesMinimum = oFuelFee.BookFeesMinimum
                                    .BookFeesTarBracketTypeControl = oFuelFee.BookFeesTarBracketTypeControl
                                    .BookFeesVariable = oFuelFee.BookFeesVariable
                                    .BookFeesVariableCode = oFuelFee.BookFeesVariableCode
                                    .TrackingState = TrackingInfo.Updated
                                End With
                                oThisBookFee = f
                                Logger.Information("Book Fee Match Found, setting BookFeesAllocationTypeControl = Load, and FeeCalcType to Unique {@BookFee}", oThisBookFee)
                            Else
                                f.BookFeesOverRidden = True
                                f.BookFeesValue = 0
                                f.TrackingState = TrackingInfo.Updated
                                Logger.Information("Book Fee Match Not Found, setting BookFeesOverRidden = True and Value to 0 {@BookFee}", f)
                            End If
                        Next
                    Else
                        'add the fee
                        oThisBookFee = oFuelFee.Clone()
                        With oThisBookFee
                            .BookFeesBookControl = intBookControl
                            .BookFeesControl = 0
                            .BookFeesValue = decOrderFuelCost
                            .BookFeesAccessorialFeeAllocationTypeControl = Utilities.FeeAllocationType.Load
                            .BookFeesAccessorialFeeCalcTypeControl = Utilities.FeeCalcType.Unique
                            .BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Order
                            .BookFeesTaxable = Taxable
                            .BookFeesUpdated = New Byte() {}
                            .TrackingState = TrackingInfo.Created
                        End With
                        Logger.Information("Book Fee Not Found, setting BookFeesAllocationTypeControl = Load, and FeeCalcType to Unique and FeeType to Order {@BookFee}", oThisBookFee)
                        oBRev.BookFees.Add(oThisBookFee)
                    End If
                    If oBRev.BookControl = intMaxWeightBookControl Then
                        oMaxBookWgtFee = oThisBookFee
                        Logger.Information("Setting oMaxBookWgtFee to oThisBookFee {@BookFee}", oMaxBookWgtFee)
                    End If
                Next

                If Not oMaxBookWgtFee Is Nothing Then
                    oMaxBookWgtFee.BookFeesValue = DataTransformation.AllocationVarianceAdjustment(decFuelAllocatedTotal, decTotalFuelCharges, oMaxBookWgtFee.BookFeesValue)
                    Logger.Information("Setting oMaxBookWgtFee.BookFeesValue to AllocationVarianceAdjustment {decFuelAllocatedTotal}, {decTotalFuelCharges}, {BookFeesValue}", decFuelAllocatedTotal, decTotalFuelCharges, oMaxBookWgtFee.BookFeesValue)
                End If

            Catch ex As System.Data.SqlClient.SqlException
                Logger.Error(ex, "SqlException in calculateFuelCharge")
                Throw
            Catch ex As InvalidOperationException
                Logger.Error(ex, "InvalidOperationException in calculateFuelCharge")
                ' throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                Logger.Error(ex, "Exception in calculateFuelCharge")
                'throwUnExpectedFaultException(ex, buildProcedureName("calculateFuelCharge"), sysErrorParameters.sysErrorState.SystemLevelFault, sysErrorParameters.sysErrorSeverity.Critical)
            End Try
            Logger.Information("Returning decTotalFuelCharges {decTotalFuelCharges}", decTotalFuelCharges)
        End Using

        Return decTotalFuelCharges
    End Function

    Protected Sub GetFuelChargeByLastStop(ByVal LastStopBookControl As Integer,
                                          ByVal CarrierControl As Integer,
                                          ByVal CarrTarControl As Integer,
                                          ByVal CarrTarEquipControl As Integer,
                                          ByRef FeesVariable As Decimal,
                                          ByRef AccessorialCode As Integer,
                                          ByVal Message As String)


        Using db As New NGLMasBookDataContext(ConnectionString)
            Try

                Logger.Information("calling udfGetFuelChargeByLastStop with LastStopBookControl: {LastStopBookControl}, CarTarControl: {CarrTarControl}, CarrTarEquipControl: {CarTarrEquipControl}, FeesVariable: {FeesVariable}, AccessorialCode: {AccessorialCode}", LastStopBookControl, CarrTarControl, CarrTarEquipControl, FeesVariable, AccessorialCode)
                Dim oFuelData = (From d In db.udfGetFuelChargeByLastStop(LastStopBookControl, CarrierControl, CarrTarControl, CarrTarEquipControl, FeesVariable, AccessorialCode)
                                 Select d).FirstOrDefault()

                If Not oFuelData Is Nothing Then

                    FeesVariable = If(oFuelData.FeesVariable, FeesVariable)
                    AccessorialCode = If(oFuelData.AccessorialCode, AccessorialCode)
                    Logger.Information("Fuel Data Found with FeesVariable: {FeesVariable} and AccessorialCode: {AccessorialCode}", FeesVariable, AccessorialCode)

                    If oFuelData.Msg.Trim.Length > 0 Then Message &= oFuelData.Msg
                End If

            Catch ex As System.Data.SqlClient.SqlException
                Logger.Error(ex, "SqlException in GetFuelChargeByLastStop")
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                Logger.Error(ex, "InvalidOperationException in GetFuelChargeByLastStop")
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                Logger.Error(ex, "Exception in GetFuelChargeByLastStop")
                throwUnExpectedFaultException(ex, buildProcedureName("GetFuelChargeByLastStop"))
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' If the tariff fees or lane fees are refreshed we need to check for an order specific fee that overrides 
    ''' the tariff or lane fee
    ''' </summary>
    ''' <param name="BookRevs"></param>
    ''' <remarks>
    ''' Modified by RHR v-7.0.5.102 7/12/2016
    ''' fixed bug in LINQ query where Boolean where condition needed parentheses
    ''' Modified by RHR for v-8.5.4.006 on 04/15/24 added logic for Booking Profile fees
    '''     Rules:
    '''     1. fee must be a profile fee
    '''     2. BookFeeValue/Minimum must be zero to override Lane or Tariff Fee
    '''     3.
    ''' </remarks>
    Protected Sub UpdateOverridesForAccessorialCode(ByRef BookRevs As List(Of BookRevenue))
        'Begin Modified by RHR v-7.0.5.102 7/12/2016
        ' get a list of all Lane and Tariff Fees assigned to the load
        Using operation = Logger.StartActivity("UpdateOverridesForAccessorialCode(BookRevs: {@BookRevs})", BookRevs)
            Dim oToOverride As List(Of BookFee) = (From d In BookRevs
                                                   From f In d.BookFees
                                                   Where
                                                   (f.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Lane _
                                                    Or
                                                    f.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Tariff
                                                       ) _
                                                   And f.BookFeesOverRidden = False
                                                   Select f).ToList()

            Logger.Information("Found {BookFeeCount} Lane Or Tariff Fees To Override", oToOverride?.Count)

            'old query
            'Dim oToOverride As List(Of DTO.BookFee) = (From d In BookRevs _
            '               From f In d.BookFees _
            '               Where f.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Lane _
            '               Or f.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Tariff _
            '               And f.BookFeesOverRidden = False _
            '               Select f).ToList()
            'End Modified
            'Modified by RHR for v-8.5.4.006 on 04/15/24 added logic for Booking Profile fees
            If Not oToOverride Is Nothing AndAlso oToOverride.Count > 0 Then
                For Each fee In oToOverride
                    Dim AccessorialCode = fee.BookFeesAccessorialCode
                    Dim BookControl = fee.BookFeesBookControl
                    Logger.Information("Checking For Order Specific Fee To Override Lane Or Tariff Fee For AccessorialCode {AccessorialCode} And BookControl: {BookControl}", AccessorialCode, BookControl)

                    'Check if an order specific fee exists
                    Dim oOverRides As DataTransferObjects.BookFee = (From d In BookRevs
                                                                     From f In d.BookFees
                                                                     Where f.BookFeesAccessorialCode = AccessorialCode _
                                                                           And f.BookFeesBookControl = BookControl _
                                                                           And f.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Order _
                                                                           And f.BookFeesOverRidden = False _
                                                                           And (f.BookFeesAccessorialProfileSpecific = False And (f.BookFeesMinimum > 0 Or f.BookFeesValue > 0))
                                                                     Select f).FirstOrDefault()
                    If Not oOverRides Is Nothing AndAlso oOverRides.BookFeesControl <> 0 Then
                        'a record exist so update the fee as overridden
                        fee.BookFeesOverRidden = True
                        fee.BookFeesAccessorialDependencyTypeControl = Utilities.AccessorialFeeDependencyType.Order
                        fee.BookFeesAccessorialOverRideReasonControl = Utilities.AccessorialFeeOverRideReasonCode.SystemOverRidden
                        fee.BookFeesDependencyKey = BookControl.ToString
                        fee.BookFeesValue = 0
                        fee.TrackingState = TrackingInfo.Updated
                        Logger.Information("Overriding Lane Or Tariff Fee for AccessorialCode {AccessorialCode} And BookControl: {BookControl}", AccessorialCode, BookControl)
                    End If
                Next
            End If
        End Using

    End Sub

    ''' <summary>
    ''' If a Lane or Tariff fee is marked as Overridden and an order specific fee 
    ''' no longer exists for the order the OverRidden flag will be turned off
    ''' with a reason code of “Missing Order Dependency”
    ''' </summary>
    ''' <param name="BookRevs"></param>
    ''' <remarks>
    ''' Modified by RHR v-7.0.5.102 7/12/2016
    ''' fixed bug in LINQ query where Boolean where condition needed parentheses 
    ''' </remarks>
    Protected Sub ReverseMissingOverrides(ByRef BookRevs As List(Of BookRevenue))
        'Begin Modified by RHR v-7.0.5.102 7/12/2016
        Using Logger.StartActivity("ReverseMissingOverrides(BookRevs {BookRevs})")
            Dim oOverridden As List(Of BookFee) = (From d In BookRevs
                                                   From f In d.BookFees
                                                   Where
                                                   (
                                                       f.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Lane _
                                                       Or f.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Tariff
                                                       ) _
                                                   And f.BookFeesOverRidden = True
                                                   Select f).ToList()
            Logger.Information("Found {Count} overridden fees related to Lane Or Tariff", oOverridden?.Count)
            If Not oOverridden Is Nothing AndAlso oOverridden.Count > 0 Then
                For Each fee In oOverridden
                    Logger.Information("Found Override Fee for {AccessorialCode}", fee.BookFeesAccessorialCode)
                    Dim AccessorialCode = fee.BookFeesAccessorialCode
                    Logger.Information("Checking for Order Specific Fee to Override Lane Or Tariff Fee for AccessorialCode {AccessorialCode} And BookControl {BookControl}", AccessorialCode, fee.BookFeesBookControl)
                    Dim oNotOverRidden As DataTransferObjects.BookFee = (From d In BookRevs
                                                                         From f In d.BookFees
                                                                         Where f.BookFeesAccessorialCode = AccessorialCode _
                                                                               And f.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Order _
                                                                               And f.BookFeesOverRidden = False _
                                                                               And d.BookControl = fee.BookFeesBookControl
                                                                         Select f).FirstOrDefault()
                    If oNotOverRidden Is Nothing OrElse oNotOverRidden.BookFeesControl = 0 Then
                        'no records exist so update the fee as nolonger overridden
                        Logger.Information("No Order Specific Fee found for AccessorialCode {AccessorialCode} And BookControl: {BookControl}", AccessorialCode, fee.BookFeesBookControl)
                        fee.BookFeesOverRidden = False
                        fee.BookFeesAccessorialDependencyTypeControl = 0
                        fee.BookFeesAccessorialOverRideReasonControl = 0
                        fee.BookFeesDependencyKey = ""
                        fee.TrackingState = TrackingInfo.Updated
                    End If
                Next
            End If
        End Using

    End Sub

    ''' <summary>
    ''' If a Lane or Tariff fee is marked as Overridden for the specified AccessorialCode  
    ''' OverRidden flag will be turned off;  the caller must be sure all order specific fees
    ''' are removed from the fees table for this AccessorialCode
    ''' </summary>
    ''' <param name="sourceFee"></param>
    ''' <param name="Books"></param>
    ''' <param name="BookPickupStopNumber"></param>
    ''' <param name="BookStopNo"></param>
    ''' <remarks></remarks>
    Protected Sub ReverseMissingOverrides(ByRef sourceFee As DataTransferObjects.BookFee, ByRef Books As List(Of LTS.Book), ByVal BookPickupStopNumber As Integer, ByVal BookStopNo As Short)
        Using operation = Logger.StartActivity("ReverseMissingOVerrides(sourceFee {SourceFee}, Books: {Books}, BookPickupStopNumber: {BookPickupStopNumber}, BookStopNo: {BookStopNo})", sourceFee, Books, BookPickupStopNumber, BookStopNo)

            If Books Is Nothing OrElse Books.Count < 1 Then Return
            If sourceFee Is Nothing OrElse sourceFee.BookFeesBookControl = 0 Then Return

            Dim BookControl As Integer = sourceFee.BookFeesBookControl
            Dim bookFeesControl As Integer = sourceFee.BookFeesControl
            Dim code As Integer = sourceFee.BookFeesAccessorialCode
            Dim aType As Integer = sourceFee.BookFeesAccessorialFeeAllocationTypeControl

            Logger.Information("{SourceFee} - Checking for Order Specific Fee to Override Lane Or Tariff Fee for AccessorialCode {AccessorialCode} And BookControl {BookControl} And if AllocationType ({AllocationType}) = Origin And bookStopPickupNumber matches",
            sourceFee,
            code,
                               BookControl,
                               aType)

            Dim oOverridden As List(Of LTS.BookFee) = (From d In Books
                                                       From f In d.BookFees
                                                       Where
                                                       f.BookFeesAccessorialCode = code _
                                                       And (
                                                           f.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Lane _
                                                           Or f.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Tariff
                                                           ) _
                                                       And (
                                                           d.BookControl = BookControl _
                                                           Or (
                                                               (aType = Utilities.FeeAllocationType.Origin And d.BookPickupStopNumber = BookPickupStopNumber) _
                                                               Or
                                                               (aType = Utilities.FeeAllocationType.Destination And d.BookStopNo = BookStopNo) _
                                                               Or
                                                               (aType = Utilities.FeeAllocationType.Load) _
                                                               Or
                                                               (aType = Utilities.FeeAllocationType.None And d.BookControl = BookControl)
                                                               )
                                                           ) _
                                                       And f.BookFeesOverRidden = True Select f).ToList()

            If Not oOverridden Is Nothing AndAlso oOverridden.Count > 0 Then

                For Each fee In oOverridden
                    Logger.Information("{OverriddenFee} - Found {Count} overridden fees related to Lane Or Tariff, setting AccessorialDependencyType = 0 And AccessorialOverrideReason = 0",
                                       fee,
                                       oOverridden?.Count)
                    fee.BookFeesOverRidden = False
                    fee.BookFeesAccessorialDependencyTypeControl = 0
                    fee.BookFeesAccessorialOverRideReasonControl = 0
                    fee.BookFeesDependencyKey = ""
                    fee.BookFeesModDate = Date.Now()
                    fee.BookFeesModUser = Me.Parameters.UserName
                Next
            End If
        End Using

    End Sub

    ''' <summary>
    ''' If a fee is marked as Route Dependent and the stored Dependent key (CNS number)
    ''' value does not match the current value for the load the fee must be flagged as 
    ''' OverRidden with a reason code of “Missing Route Dependency”.
    ''' </summary>
    ''' <param name="BookRevs"></param>
    ''' <param name="PhysicalDeleteMissing"></param>
    ''' <remarks></remarks>
    Protected Sub ProcessMissingRouteDependencies(ByRef BookRevs As List(Of BookRevenue), Optional ByVal PhysicalDeleteMissing As Boolean = True)
        Using operation = Logger.StartActivity("ProcessMissingRouteDependencies(BookRevs {@BookRevs}, PhysicalDeleteMissing: {PhysicalDeleteMissing}", BookRevs, PhysicalDeleteMissing)
            Dim FeesToDeleteControls As New List(Of Integer)
            Dim oMissing As List(Of BookFee) = (From d In BookRevs
                                                From f In d.BookFees
                                                Where f.BookFeesAccessorialDependencyTypeControl = Utilities.AccessorialFeeDependencyType.Route _
                                                  And d.BookConsPrefix <> f.BookFeesDependencyKey _
                                                  And f.BookFeesOverRidden = False
                                                Select f).ToList()

            Logger.Information("Found {Count} Route Dependent Fees to Process where AccessorialDependencyTypeControl={Route} ", oMissing?.Count, Utilities.AccessorialFeeDependencyType.Route)

            If Not oMissing Is Nothing AndAlso oMissing.Count > 0 Then
                For Each fee In oMissing

                    Logger.Information("{BookFee} - Is Missing, Checking if it is Order specific", fee)

                    If fee.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Order Then

                        Dim NonOrderFee As DataTransferObjects.BookFee = (From d In BookRevs
                                                                          From f In d.BookFees
                                                                          Where f.BookFeesBookControl = fee.BookFeesBookControl _
                                                                            And f.BookFeesAccessorialFeeTypeControl <> Utilities.AccessorialFeeType.Order _
                                                                            And f.BookFeesAccessorialCode = fee.BookFeesAccessorialCode _
                                                                            And f.BookFeesAccessorialFeeAllocationTypeControl = fee.BookFeesAccessorialFeeAllocationTypeControl _
                                                                            And f.BookFeesOverRidden = True
                                                                          Order By f.BookFeesAccessorialFeeTypeControl Descending
                                                                          Select f).FirstOrDefault()

                        Logger.Information("{NonOrderFee} - Checking If NonOrderFee exists where FeeTypeControl <> {OrderFeeType} And AccessorialCode = {FeeAccessorialCode} And FeeAllocationType = {FeeAllocationType}",
                                           NonOrderFee,
                                           Utilities.AccessorialFeeType.Order,
                                           fee.BookFeesAccessorialCode,
                                           fee.BookFeesAccessorialFeeAllocationTypeControl)

                        If Not NonOrderFee Is Nothing AndAlso NonOrderFee.BookFeesControl <> 0 Then
                            NonOrderFee.BookFeesOverRidden = False
                            NonOrderFee.BookFeesAccessorialOverRideReasonControl = Utilities.AccessorialFeeOverRideReasonCode.SystemReversed
                            NonOrderFee.TrackingState = TrackingInfo.Updated

                            Logger.Information("{NonOrderFee} found, setting BookFeesOverRidden = False And BookFeesAccessorialOverRideReasonControl = {SystemReversed}",
                                               NonOrderFee.BookFeesCaption,
                                               Utilities.AccessorialFeeOverRideReasonCode.SystemReversed)

                            If PhysicalDeleteMissing Then
                                Logger.Information("{NonOrderFee} - Adding Fee to FeesToDeleteControls", fee)
                                FeesToDeleteControls.Add(fee.BookFeesControl)
                            Else
                                Logger.Information("{NonOrderFee} marking as Overridden And setting Value to 0 so that it will not be used in further calculations until saved into the database", fee)
                                fee.BookFeesOverRidden = True
                                fee.BookFeesValue = 0
                                fee.TrackingState = TrackingInfo.Deleted
                            End If
                            Continue For
                        End If
                    End If
                    fee.BookFeesOverRidden = True
                    fee.BookFeesValue = 0
                    fee.BookFeesAccessorialOverRideReasonControl = Utilities.AccessorialFeeOverRideReasonCode.MissingRouteDependency
                    fee.TrackingState = TrackingInfo.Updated

                    Logger.Information("{NonOrderFee} - Marking as Overridden And setting Value to 0 And AccessorialOverRideReasonControl to {MissingRouteDependency}",
                                       fee.BookFeesCaption,
                                       Utilities.AccessorialFeeOverRideReasonCode.MissingRouteDependency)
                Next
            End If

            'now remove the physical deletes
            For Each control In FeesToDeleteControls
                Logger.Information("Removing Fee {Control} from oMissing", control)
                oMissing.RemoveAll(Function(x) x.BookFeesControl = control)
            Next
        End Using

    End Sub

    ''' <summary>
    ''' If a fee is marked as Lane Dependent and the stored Dependent Lane Value no longer exists 
    ''' for any orders on the load the fee will be flagged as OverRidden with a reason code of “Missing Lane Dependency”
    ''' </summary>
    ''' <param name="BookRevs"></param>
    ''' <param name="PhysicalDeleteMissing"></param>
    ''' <remarks></remarks>
    Protected Sub ProcessMissingLaneDependencies(ByRef BookRevs As List(Of BookRevenue), Optional ByVal PhysicalDeleteMissing As Boolean = True)
        Using operation = Logger.StartActivity("ProcessMissingLaneDependencies(BookRevs {BookRevs}, PhysicalDeleteMissing: {PhysicalDeleteMissing}", BookRevs, PhysicalDeleteMissing)

            Dim oActual As List(Of String) = (From d In BookRevs Select Convert.ToString(d.BookODControl)).Distinct().ToList()

            Logger.Information("Found {Count} distinct BookODControl values", oActual?.Count)

            Dim oMissing As List(Of BookFee) = (From d In BookRevs
                                                From f In d.BookFees
                                                Where f.BookFeesAccessorialDependencyTypeControl = Utilities.AccessorialFeeDependencyType.Lane _
                                                  And (f.BookFeesDependencyKey.Trim.Length > 0 AndAlso Not oActual.Contains(f.BookFeesDependencyKey)) _
                                                  And f.BookFeesOverRidden = False
                                                Select f).ToList()
            Logger.Information("Found {Count} Lane Dependent Fees to Process where AccessorialDependencyTypeControl={Lane} ", oMissing?.Count, Utilities.AccessorialFeeDependencyType.Lane)


            Dim FeesToDeleteControls As New List(Of Integer)
            If Not oMissing Is Nothing AndAlso oMissing.Count > 0 Then
                For Each fee In oMissing
                    'check if this is an order specific fee
                    Logger.Information("Checking If Missing Fee {Fee} Is Order Specific", fee.BookFeesCaption)
                    If fee.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Order Then
                        'check for a matching lane or tariff specific fee
                        Logger.Information("Checking for NonOrderFee where BookControl = {BookControl} And AccessorialCode = {AccessorialCode} And FeeAllocationType = {FeeAllocationType} And OverRidden = True", fee.BookFeesBookControl, fee.BookFeesAccessorialCode, fee.BookFeesAccessorialFeeAllocationTypeControl)
                        Dim NonOrderFee As DataTransferObjects.BookFee = (From d In BookRevs
                                                                          From f In d.BookFees
                                                                          Where f.BookFeesBookControl = fee.BookFeesBookControl _
                                                                                And f.BookFeesAccessorialFeeTypeControl <> Utilities.AccessorialFeeType.Order _
                                                                                And f.BookFeesAccessorialCode = fee.BookFeesAccessorialCode _
                                                                                And f.BookFeesAccessorialFeeAllocationTypeControl = fee.BookFeesAccessorialFeeAllocationTypeControl _
                                                                                And f.BookFeesOverRidden = True
                                                                          Order By f.BookFeesAccessorialFeeTypeControl Descending
                                                                          Select f).FirstOrDefault()
                        'if a lane or tariff fee that was previously overridden exists 
                        'we reverse the overridden status and delete the order specific fee
                        If Not NonOrderFee Is Nothing AndAlso NonOrderFee.BookFeesControl <> 0 Then
                            Logger.Information("NonOrderFee {NonOrderFee} found, setting BookFeesOverRidden = False And BookFeesAccessorialOverRideReasonControl = {SystemReversed}", NonOrderFee.BookFeesCaption, Utilities.AccessorialFeeOverRideReasonCode.SystemReversed)
                            NonOrderFee.BookFeesOverRidden = False
                            NonOrderFee.BookFeesAccessorialOverRideReasonControl = Utilities.AccessorialFeeOverRideReasonCode.SystemReversed
                            NonOrderFee.TrackingState = TrackingInfo.Updated
                            If PhysicalDeleteMissing Then
                                FeesToDeleteControls.Add(fee.BookFeesControl)
                            Else
                                Logger.Information("Marking Fee {Fee} as Overridden And setting Value to 0", fee.BookFeesCaption)
                                fee.BookFeesOverRidden = True 'mark as overridden so it will not be used in further calculations until saved to the database.
                                fee.BookFeesValue = 0
                                fee.TrackingState = TrackingInfo.Deleted  'the save routine will remove this item from the database when using the default updatewithdetails method
                            End If
                            Continue For
                        End If
                    End If
                    'if we get here we simply mark the fee as overridden and update the reason code
                    Logger.Information("Marking Fee {Fee} as Overridden And setting Value to 0 And AccessorialOverRideReasonControl to {MissingLaneDependency}", fee.BookFeesCaption)
                    fee.BookFeesOverRidden = True
                    fee.BookFeesValue = 0
                    fee.BookFeesAccessorialOverRideReasonControl = Utilities.AccessorialFeeOverRideReasonCode.MissingLaneDependency
                    fee.TrackingState = TrackingInfo.Updated
                Next
            End If
            'now remove the physical deletes
            For Each control In FeesToDeleteControls
                oMissing.RemoveAll(Function(x) x.BookFeesControl = control)
            Next
        End Using

    End Sub

    ''' <summary>
    ''' If a fee is marked as Order Dependent and the stored Dependent Book Control Value 
    ''' no longer exists for any orders on the load the fee will be flagged as OverRidden 
    ''' with a reason code of “Missing Order Dependency”
    ''' </summary>
    ''' <param name="BookRevs"></param>
    ''' <param name="PhysicalDeleteMissing"></param>
    ''' <remarks></remarks>
    Protected Sub ProcessMissingOrderDependencies(ByRef BookRevs As List(Of BookRevenue), Optional ByVal PhysicalDeleteMissing As Boolean = True)
        Using operation = Logger.StartActivity("ProcessMissingOrderDependencies(BookRevs {BookRevs}, PhysicalDeleteMissing: {PhysicalDeleteMissing}", BookRevs, PhysicalDeleteMissing)
            Dim oActual As List(Of String) = (From d In BookRevs Select Convert.ToString(d.BookControl)).Distinct().ToList()

            Logger.Information("Found {Count} distinct BookControl values", oActual?.Count)

            Dim oMissing As List(Of BookFee) = (From d In BookRevs
                                                From f In d.BookFees
                                                Where f.BookFeesAccessorialDependencyTypeControl = Utilities.AccessorialFeeDependencyType.Order _
                                                  And (f.BookFeesDependencyKey.Trim.Length > 0 AndAlso Not oActual.Contains(f.BookFeesDependencyKey)) _
                                                  And f.BookFeesOverRidden = False
                                                Select f).ToList()

            Logger.Information("Found {Count} Order Dependent Fees to Process where AccessorialDependencyTypeControl={Order} ", oMissing?.Count, Utilities.AccessorialFeeDependencyType.Order)

            Dim FeesToDeleteControls As New List(Of Integer)
            If Not oMissing Is Nothing AndAlso oMissing.Count > 0 Then
                For Each fee In oMissing
                    'check if this is an order specific fee
                    Logger.Information("Checking If Missing Fee {Fee} Is Order Specific", fee.BookFeesCaption)
                    If fee.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Order Then
                        'check for a matching lane or tariff specific fee
                        Dim NonOrderFee As DataTransferObjects.BookFee = (From d In BookRevs
                                                                          From f In d.BookFees
                                                                          Where f.BookFeesBookControl = fee.BookFeesBookControl _
                                                                            And f.BookFeesAccessorialFeeTypeControl <> Utilities.AccessorialFeeType.Order _
                                                                            And f.BookFeesAccessorialCode = fee.BookFeesAccessorialCode _
                                                                            And f.BookFeesAccessorialFeeAllocationTypeControl = fee.BookFeesAccessorialFeeAllocationTypeControl _
                                                                            And f.BookFeesOverRidden = True
                                                                          Order By f.BookFeesAccessorialFeeTypeControl Descending
                                                                          Select f).FirstOrDefault()
                        Logger.Information("Checking for NonOrderFee where BookControl = {BookControl} And AccessorialCode = {AccessorialCode} And FeeAllocationType = {FeeAllocationType} And OverRidden = True", fee.BookFeesBookControl, fee.BookFeesAccessorialCode, fee.BookFeesAccessorialFeeAllocationTypeControl)
                        'if a lane or tariff fee that was previously overridden exists 
                        'we reverse the overridden status and delete the order specific fee
                        If Not NonOrderFee Is Nothing AndAlso NonOrderFee.BookFeesControl <> 0 Then
                            Logger.Information("NonOrderFee {NonOrderFee} found, setting BookFeesOverRidden = False And BookFeesAccessorialOverRideReasonControl = {SystemReversed}", NonOrderFee.BookFeesCaption, Utilities.AccessorialFeeOverRideReasonCode.SystemReversed)
                            NonOrderFee.BookFeesOverRidden = False
                            NonOrderFee.BookFeesAccessorialOverRideReasonControl = Utilities.AccessorialFeeOverRideReasonCode.SystemReversed
                            NonOrderFee.TrackingState = TrackingInfo.Updated
                            If PhysicalDeleteMissing Then
                                Logger.Information("Adding Fee {Fee} to FeesToDeleteControls", fee.BookFeesCaption)
                                FeesToDeleteControls.Add(fee.BookFeesControl)
                            Else
                                Logger.Information("Marking Fee {Fee} as Overridden And setting Value to 0", fee.BookFeesCaption)
                                fee.BookFeesOverRidden = True 'mark as overridden so it will not be used in further calculations until saved to the database.
                                fee.BookFeesValue = 0
                                fee.TrackingState = TrackingInfo.Deleted  'the save routine will remove this item from the database when using the default updatewithdetails method
                            End If
                            Continue For
                        End If
                    End If
                    'if we get here we simply mark the fee as overridden and update the reason code
                    Logger.Information("Marking Fee {Fee} as Overridden And setting Value to 0 And AccessorialOverRideReasonControl to {MissingOrderDependency}", fee.BookFeesCaption)
                    fee.BookFeesOverRidden = True
                    fee.BookFeesValue = 0
                    fee.BookFeesAccessorialOverRideReasonControl = Utilities.AccessorialFeeOverRideReasonCode.MissingLaneDependency
                    fee.TrackingState = TrackingInfo.Updated
                Next
            End If
            'now remove the physical deletes
            For Each control In FeesToDeleteControls
                oMissing.RemoveAll(Function(x) x.BookFeesControl = control)
            Next
        End Using

    End Sub

    ''' <summary>
    ''' Performs normalization on allocated order specific fees to ensure they meet current business requirements
    ''' typically called from NormalizeAllocatedFees and should not be called directly.  This overload requires a
    ''' LTS version of the Book Table list.  It assumes that sourceFee is the most current version of the data
    ''' </summary>
    ''' <param name="sourceFee"></param>
    ''' <param name="BookRevs"></param>
    ''' <param name="BookPickupStopNumber"></param>
    ''' <param name="BookStopNo"></param>
    ''' <remarks>
    ''' Created by RHR v-7.0.5.110 7/13/2016
    ''' New business rule changes for order specific fees.  Previously it was hard if not impossible
    ''' to change the allocation rules and calculation formula on Load specific fees previously assigned because 
    ''' it was all reset to Load specific.
    ''' Here are the new business rules being applied.
    ''' 1. For this overload the souce fee provided is always expected to be the most recent record changed
    ''' 2. For this overload the source fee’s modified date has not been changed yet so we always apply the changes to other similar feeson the load
    ''' 3. If the modified fee’s allocation method is set to None apply the following rules
    '''    (a)	Any similar fees previously set to use Origin Allocation that has the same Origin Location
    '''         or similare fees previously set to use Destination Allocaiton that has the same Destination location
    '''         will be modified to match the current changes
    '''    (b)	Any other similar fees currently set to use Load Allocation will be set to use None for Allocation.
    '''    (c)	All other fees will not be modified
    ''' 4. If the modified fee’s allocation method is set to Origin apply the following rules
    '''    (a)	All similar fees picking up at the same origin will be modified to match the current changes
    '''    (b)	All other similar fees not picking up at the same origin that have an allocation method of Load will be set to None
    '''    (c)	Any other similar fees with an allocation method of Destination or None will remain unchanged
    ''' 5. If the modified fee’s allocation method is set to Destination apply the following rules
    '''    (a)	All similar fees delivering to the same destination will be modified to match the current changes
    '''    (b)	All other similar fees not going to the same destination that have an allocation method of Load will be set to None
    '''    (c)	Any other similar fees with an allocation method of Origin or None will remain unchanged
    ''' 6. If the modified fee’s allocation method is set to Load all other similar fees will also be set to Load and modified with the same configuration
    ''' Modified by RHR for v-8.2 on 7/3/2019
    '''  added New field "BookFeesMissingFee" to support tracking of missing expected carrier fees
    '''  the default Is false.  
    ''' </remarks>
    Friend Sub NormalizeAllocatedOrderSpecificFees(ByRef sourceFee As DataTransferObjects.BookFee, ByRef BookRevs As List(Of LTS.Book), ByVal BookPickupStopNumber As Integer, ByVal BookStopNo As Short)
        Using operation = Logger.StartActivity("NormalizeAllocatedOrderSpecificFees(sourceFee {@SourceFee}, BookRevs: {BookRevs}, BookPickupStopNumber: {BookPickupStopNumber}, BookStopNo: {BookStopNo})", sourceFee, BookRevs, BookPickupStopNumber, BookStopNo)


            If BookRevs Is Nothing OrElse BookRevs.Count < 1 Then
                Logger.Warning("No BookRevs found, returning")
                Return
            End If
            'check for fee differences       
            If sourceFee Is Nothing OrElse sourceFee.BookFeesBookControl = 0 Then
                Logger.Warning("No sourceFee found, returning")
                Return
            End If
            'check if this is an order specific fee,  this should always be true except for a programming error we only show an error if the Debugger.IsAttached
            Logger.Information("Checking if sourceFee {Fee}({FeeType}) Is an Order Specific Fee ({OrderFeeType})", sourceFee.BookFeesCaption, sourceFee.BookFeesAccessorialFeeTypeControl, Utilities.AccessorialFeeType.Order)
            If sourceFee.BookFeesAccessorialFeeTypeControl <> Utilities.AccessorialFeeType.Order Then
                Logger.Warning("sourceFee {Fee} Is Not an Order Specific Fee", sourceFee.BookFeesCaption)
                If System.Diagnostics.Debugger.IsAttached Then
                    throwUnExpectedFaultException("Debug Message NormalizeAllocatedOrderSpecificFees Is Not allowed when the sourceFee's AccessorialFeeType is not Order ")
                End If
                Return
            End If


            'select the key fields from sourceFee
            'Rule 1 & 2
            Dim code As Integer = sourceFee.BookFeesAccessorialCode
            Dim aType As Integer = sourceFee.BookFeesAccessorialFeeAllocationTypeControl
            Dim FeeType As Integer = sourceFee.BookFeesAccessorialFeeTypeControl
            Dim BookFeesMinimum As Decimal = sourceFee.BookFeesMinimum
            Dim BookFeesVariable As Double = sourceFee.BookFeesVariable
            Dim BookFeesVariableCode As Integer = sourceFee.BookFeesVariableCode
            Dim BookFeesTaxable As Boolean = sourceFee.BookFeesTaxable
            Dim BookFeesIsTax As Boolean = sourceFee.BookFeesIsTax
            Dim BookFeesTaxSortOrder As Integer = sourceFee.BookFeesTaxSortOrder
            Dim BookFeesTarBracketTypeControl As Integer = sourceFee.BookFeesTarBracketTypeControl
            Dim BookFeesAccessorialFeeCalcTypeControl = sourceFee.BookFeesAccessorialFeeCalcTypeControl
            Dim BookFeesBookControl As Integer = sourceFee.BookFeesBookControl
            Dim BookFeesControl As Integer = sourceFee.BookFeesControl
            'Modified by RHR for v-8.2 on 7/3/2019
            Dim BookFeesMissingFee As Boolean = sourceFee.BookFeesMissingFee
            Dim OriginStreet As String = ""
            Dim OriginState As String = ""
            Dim DestStreet As String = ""
            Dim DestState As String = ""
            'get the BookRev object associated with the sourceFee if it is available
            Dim sourceRev As LTS.Book = BookRevs.Where(Function(x) x.BookControl = BookFeesBookControl).FirstOrDefault()
            If Not sourceRev Is Nothing AndAlso sourceRev.BookControl <> 0 Then
                OriginStreet = sourceRev.BookOrigAddress1
                OriginState = sourceRev.BookOrigState
                DestStreet = sourceRev.BookDestAddress1
                DestState = sourceRev.BookDestState
            End If
            Dim FeesDiff As New List(Of LTS.BookFee)
            Dim FeesToNone As New List(Of LTS.BookFee)
            Logger.Information("Checking for Fees with AllocationType = {AllocationType} and AccessorialCode = {AccessorialCode} and BookControl = {BookControl}", aType, code, BookFeesBookControl)
            If aType = Utilities.FeeAllocationType.None Then
                'Rule 3 a Origin and Destination Order Fees Test
                Logger.Information("Checking for BookFees with AccessorialCode: {AccessorialCode} of Type {FeeTypeOrder} and BookFeesControl <> {BookFeesControl}, AND (BookFeeAllocationType <> {AllocationType} or BookFeeMinimum <> {BookFeesMinimum} or BookFeesVariable <> {BookFeesVariable} or BookFeesVariableCode <> {BookFeesVariableCode} or BookFeesTaxable <> {BookFeesTaxable} or BookFeesIsTax <> {BookFeesIsTax} or FeeBracket <> {BookFeesBracket} and AccessorialFeeCalcType <> {AccessorialFeeCalcType}", code, Utilities.AccessorialFeeType.Order, BookFeesControl, aType, BookFeesMinimum, BookFeesVariable, BookFeesVariableCode, BookFeesTaxable, BookFeesIsTax, BookFeesTarBracketTypeControl, BookFeesAccessorialFeeCalcTypeControl)
                FeesDiff = (From d In BookRevs
                            From f In d.BookFees
                            Where f.BookFeesOverRidden = False _
                              And f.BookFeesAccessorialCode = code _
                              And f.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Order _
                              And f.BookFeesControl <> BookFeesControl _
                              And
                              (
                                  f.BookFeesAccessorialFeeAllocationTypeControl <> aType _
                                  Or
                                  f.BookFeesMinimum <> BookFeesMinimum _
                                  Or
                                  f.BookFeesVariable <> BookFeesVariable _
                                  Or
                                  f.BookFeesVariableCode <> BookFeesVariableCode _
                                  Or
                                  f.BookFeesTaxable <> BookFeesTaxable _
                                  Or
                                  f.BookFeesIsTax <> BookFeesIsTax _
                                  Or
                                  f.BookFeesTaxSortOrder <> BookFeesTaxSortOrder _
                                  Or
                                  f.BookFeesTarBracketTypeControl <> BookFeesTarBracketTypeControl _
                                  Or
                                  f.BookFeesAccessorialFeeCalcTypeControl <> BookFeesAccessorialFeeCalcTypeControl
                                  ) _
                              And
                              (
                                  (
                                      f.BookFeesAccessorialFeeAllocationTypeControl = Utilities.FeeAllocationType.Origin _
                                      And
                                      (
                                          If(String.IsNullOrWhiteSpace(OriginStreet), d.BookPickupStopNumber = BookPickupStopNumber, d.BookOrigAddress1 = OriginStreet And d.BookOrigState = OriginState)
                                          )
                                      ) _
                                  Or
                                  (
                                      f.BookFeesAccessorialFeeAllocationTypeControl = Utilities.FeeAllocationType.Destination _
                                      And
                                      (
                                          If(String.IsNullOrWhiteSpace(DestStreet), d.BookStopNo = BookStopNo, d.BookDestAddress1 = DestStreet And d.BookDestState = DestState)
                                          )
                                      )
                                  )
                            Select f).ToList()
                Logger.Information("Found {Count} Fees to update", FeesDiff?.Count)

                Logger.Information("Checking for BookFees with AccessorialCode: {AccessorialCode} of Type {FeeTypeOrder} and BookFeesControl <> {BookFeesControl} and AccessorialFeeAllocationType = {AllocationType} and BookFeesControl <> {BookFeesControl}", code, Utilities.AccessorialFeeType.Order, BookFeesControl, Utilities.FeeAllocationType.Load, BookFeesControl)
                'Rule 3 b Load to None Order Fees
                FeesToNone = (From d In BookRevs
                              From f In d.BookFees
                              Where f.BookFeesOverRidden = False _
                                And f.BookFeesAccessorialCode = code _
                                And f.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Order _
                                And f.BookFeesControl <> BookFeesControl _
                                And f.BookFeesAccessorialFeeAllocationTypeControl = Utilities.FeeAllocationType.Load
                              Select f).ToList()

                Logger.Information("Found {Count} Fees to set to None", FeesToNone?.Count)
            ElseIf aType = Utilities.FeeAllocationType.Origin Then
                'Rule 4 a 
                Logger.Information("Checking for BookFees with AccessorialCode: {AccessorialCode} of Type {FeeTypeOrder} and BookFeesControl <> {BookFeesControl}, AND (BookFeeAllocationType <> {AllocationType} or BookFeeMinimum <> {BookFeesMinimum} or BookFeesVariable <> {BookFeesVariable} or BookFeesVariableCode <> {BookFeesVariableCode} or BookFeesTaxable <> {BookFeesTaxable} or BookFeesIsTax <> {BookFeesIsTax} or FeeBracket <> {BookFeesBracket} and AccessorialFeeCalcType <> {AccessorialFeeCalcType}", code, Utilities.AccessorialFeeType.Order, BookFeesControl, aType, BookFeesMinimum, BookFeesVariable, BookFeesVariableCode, BookFeesTaxable, BookFeesIsTax, BookFeesTarBracketTypeControl, BookFeesAccessorialFeeCalcTypeControl)
                FeesDiff = (From d In BookRevs
                            From f In d.BookFees
                            Where f.BookFeesOverRidden = False _
                                  And f.BookFeesAccessorialCode = code _
                                  And f.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Order _
                                  And f.BookFeesControl <> BookFeesControl _
                                  And (If(String.IsNullOrWhiteSpace(OriginStreet), d.BookPickupStopNumber = BookPickupStopNumber, d.BookOrigAddress1 = OriginStreet And d.BookOrigState = OriginState)) _
                                  And
                                  (
                                      f.BookFeesAccessorialFeeAllocationTypeControl <> aType _
                                      Or
                                      f.BookFeesMinimum <> BookFeesMinimum _
                                      Or
                                      f.BookFeesVariable <> BookFeesVariable _
                                      Or
                                      f.BookFeesVariableCode <> BookFeesVariableCode _
                                      Or
                                      f.BookFeesTaxable <> BookFeesTaxable _
                                      Or
                                      f.BookFeesIsTax <> BookFeesIsTax _
                                      Or
                                      f.BookFeesTaxSortOrder <> BookFeesTaxSortOrder _
                                      Or
                                      f.BookFeesTarBracketTypeControl <> BookFeesTarBracketTypeControl _
                                      Or
                                      f.BookFeesAccessorialFeeCalcTypeControl <> BookFeesAccessorialFeeCalcTypeControl
                                      )
                            Select f).ToList()
                'Rule 4 b 
                Logger.Information("Found {Count} Fees to update", FeesDiff?.Count)

                Logger.Information("Checking for BookFees with AccessorialCode: {AccessorialCode} of Type {FeeTypeOrder} and BookFeesControl <> {BookFeesControl} and AccessorialFeeAllocationType = {AllocationType} and BookFeesControl <> {BookFeesControl}", code, Utilities.AccessorialFeeType.Order, BookFeesControl, Utilities.FeeAllocationType.Load, BookFeesControl)
                FeesToNone = (From d In BookRevs
                              From f In d.BookFees
                              Where f.BookFeesOverRidden = False _
                                    And f.BookFeesAccessorialCode = code _
                                    And f.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Order _
                                    And f.BookFeesControl <> BookFeesControl _
                                    And f.BookFeesAccessorialFeeAllocationTypeControl = Utilities.FeeAllocationType.Load _
                                    And (If(String.IsNullOrWhiteSpace(OriginStreet), d.BookPickupStopNumber <> BookPickupStopNumber, d.BookOrigAddress1 <> OriginStreet And d.BookOrigState <> OriginState))
                              Select f).ToList()
                Logger.Information("Found {Count} Fees to None", FeesDiff?.Count)
            ElseIf aType = Utilities.FeeAllocationType.Destination Then
                'Rule 5 a 
                Logger.Information("Checking for BookFees with AccessorialCode: {AccessorialCode} of Type {FeeTypeOrder} and BookFeesControl <> {BookFeesControl}, AND (BookFeeAllocationType <> {AllocationType} or BookFeeMinimum <> {BookFeesMinimum} or BookFeesVariable <> {BookFeesVariable} or BookFeesVariableCode <> {BookFeesVariableCode} or BookFeesTaxable <> {BookFeesTaxable} or BookFeesIsTax <> {BookFeesIsTax} or FeeBracket <> {BookFeesBracket} and AccessorialFeeCalcType <> {AccessorialFeeCalcType}", code, Utilities.AccessorialFeeType.Order, BookFeesControl, aType, BookFeesMinimum, BookFeesVariable, BookFeesVariableCode, BookFeesTaxable, BookFeesIsTax, BookFeesTarBracketTypeControl, BookFeesAccessorialFeeCalcTypeControl)
                FeesDiff = (From d In BookRevs
                            From f In d.BookFees
                            Where f.BookFeesOverRidden = False _
                              And f.BookFeesAccessorialCode = code _
                              And f.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Order _
                              And f.BookFeesControl <> BookFeesControl _
                              And (If(String.IsNullOrWhiteSpace(DestStreet), d.BookStopNo = BookStopNo, d.BookDestAddress1 = DestStreet And d.BookDestState = DestState)) _
                              And
                              (
                                  f.BookFeesAccessorialFeeAllocationTypeControl <> aType _
                                  Or
                                  f.BookFeesMinimum <> BookFeesMinimum _
                                  Or
                                  f.BookFeesVariable <> BookFeesVariable _
                                  Or
                                  f.BookFeesVariableCode <> BookFeesVariableCode _
                                  Or
                                  f.BookFeesTaxable <> BookFeesTaxable _
                                  Or
                                  f.BookFeesIsTax <> BookFeesIsTax _
                                  Or
                                  f.BookFeesTaxSortOrder <> BookFeesTaxSortOrder _
                                  Or
                                  f.BookFeesTarBracketTypeControl <> BookFeesTarBracketTypeControl _
                                  Or
                                  f.BookFeesAccessorialFeeCalcTypeControl <> BookFeesAccessorialFeeCalcTypeControl
                                  )
                            Select f).ToList()
                Logger.Information("Found {Count} Fees to update", FeesDiff?.Count)

                'Rule 5 b 

                Logger.Information("Checking for BookFees with AccessorialCode: {AccessorialCode} of Type {FeeTypeOrder} and BookFeesControl <> {BookFeesControl} and AccessorialFeeAllocationType = {AllocationType} and BookFeesControl <> {BookFeesControl}", code, Utilities.AccessorialFeeType.Order, BookFeesControl, Utilities.FeeAllocationType.Load, BookFeesControl)
                FeesToNone = (From d In BookRevs
                              From f In d.BookFees
                              Where f.BookFeesOverRidden = False _
                                And f.BookFeesAccessorialCode = code _
                                And f.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Order _
                                And f.BookFeesAccessorialFeeAllocationTypeControl = Utilities.FeeAllocationType.Load _
                                And f.BookFeesControl <> BookFeesControl _
                                And (If(String.IsNullOrWhiteSpace(DestStreet), d.BookStopNo <> BookStopNo, d.BookDestAddress1 <> DestStreet And d.BookDestState <> DestState))
                              Select f).ToList()
                Logger.Information("Found {Count} Fees to None", FeesDiff?.Count)
            ElseIf aType = Utilities.FeeAllocationType.Load Then
                'Rule 6
                Logger.Information("[FeeAllocationType.Load] - Checking for BookFees with AccessorialCode: {AccessorialCode} of Type {FeeTypeOrder} and BookFeesControl <> {BookFeesControl}, AND (BookFeeAllocationType <> {AllocationType} or BookFeeMinimum <> {BookFeesMinimum} or BookFeesVariable <> {BookFeesVariable} or BookFeesVariableCode <> {BookFeesVariableCode} or BookFeesTaxable <> {BookFeesTaxable} or BookFeesIsTax <> {BookFeesIsTax} or FeeBracket <> {BookFeesBracket} and AccessorialFeeCalcType <> {AccessorialFeeCalcType}", code, Utilities.AccessorialFeeType.Order, BookFeesControl, aType, BookFeesMinimum, BookFeesVariable, BookFeesVariableCode, BookFeesTaxable, BookFeesIsTax, BookFeesTarBracketTypeControl, BookFeesAccessorialFeeCalcTypeControl)
                FeesDiff = (From d In BookRevs
                            From f In d.BookFees
                            Where f.BookFeesOverRidden = False _
                                  And f.BookFeesAccessorialCode = code _
                                  And f.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Order _
                                  And f.BookFeesControl <> BookFeesControl _
                                  And
                                  (
                                      f.BookFeesAccessorialFeeAllocationTypeControl <> aType _
                                      Or
                                      f.BookFeesMinimum <> BookFeesMinimum _
                                      Or
                                      f.BookFeesVariable <> BookFeesVariable _
                                      Or
                                      f.BookFeesVariableCode <> BookFeesVariableCode _
                                      Or
                                      f.BookFeesTaxable <> BookFeesTaxable _
                                      Or
                                      f.BookFeesIsTax <> BookFeesIsTax _
                                      Or
                                      f.BookFeesTaxSortOrder <> BookFeesTaxSortOrder _
                                      Or
                                      f.BookFeesTarBracketTypeControl <> BookFeesTarBracketTypeControl _
                                      Or
                                      f.BookFeesAccessorialFeeCalcTypeControl <> BookFeesAccessorialFeeCalcTypeControl
                                      )
                            Select f).ToList()
            End If
            If Not FeesToNone Is Nothing AndAlso FeesToNone.Count() > 0 Then
                For Each fee In FeesToNone
                    Logger.Information("Setting Fee {Fee} to AllocationType = {AllocationType}", fee.BookFeesCaption, Utilities.FeeAllocationType.None)
                    fee.BookFeesAccessorialFeeAllocationTypeControl = Utilities.FeeAllocationType.None
                Next
            End If
            If Not FeesDiff Is Nothing AndAlso FeesDiff.Count() > 0 Then
                For Each fee In FeesDiff
                    Logger.Information("Updating Fee {Fee} with new values {@FeeValues}", fee.BookFeesCaption, fee)
                    With fee
                        .BookFeesAccessorialFeeAllocationTypeControl = aType
                        .BookFeesMinimum = BookFeesMinimum
                        .BookFeesVariable = BookFeesVariable
                        .BookFeesVariableCode = BookFeesVariableCode
                        .BookFeesTaxable = BookFeesTaxable
                        .BookFeesIsTax = BookFeesIsTax
                        .BookFeesTaxSortOrder = BookFeesTaxSortOrder
                        .BookFeesTarBracketTypeControl = BookFeesTarBracketTypeControl
                        .BookFeesAccessorialDependencyTypeControl = Utilities.AccessorialFeeDependencyType.Order
                        .BookFeesAccessorialOverRideReasonControl = Utilities.AccessorialFeeOverRideReasonCode.SystemOverRidden
                        .BookFeesAccessorialFeeCalcTypeControl = BookFeesAccessorialFeeCalcTypeControl
                        .BookFeesDependencyKey = BookFeesBookControl
                        .BookFeesMissingFee = BookFeesMissingFee
                    End With
                Next
            End If
        End Using

    End Sub

    ''' <summary>
    ''' Performs normalization on allocated order specific fees to ensure they meet current business requirements
    ''' typically called from NormalizeAllocatedFees and should not be called directly.  this overload requires a 
    ''' DTO version of the BookRevenue list it uses the Modified Date to select the most current version of the data
    ''' </summary>
    ''' <param name="sourceFee"></param>
    ''' <param name="BookRevs"></param>
    ''' <remarks>
    ''' Created by RHR v-7.0.5.110 7/13/2016
    ''' New business rule changes for order specific fees.  Previously it was hard if not impossible
    ''' to change the allocation rules and calculation formula on Load specific fees previously assigned because 
    ''' it was all reset to Load specific.
    ''' Here are the new business rules being applied.
    ''' 1. For all order specific fees get the bookfee record with the most recent modified date
    ''' 2. If the modified fee’s modified date is before today do not make any changes to other fees on the load
    ''' 3. If the modified fee’s allocation method is set to None apply the following rules
    '''    (a)	Any similar fees previously set to use Origin Allocation that has the same Origin Location
    '''         or similare fees previously set to use Destination Allocaiton that has the same Destination location
    '''         will be modified to match the current changes
    '''    (b)	Any other similar fees currently set to use Load Allocation will be set to use None for Allocation.
    '''    (c)	All other fees will not be modified
    ''' 4. If the modified fee’s allocation method is set to Origin apply the following rules
    '''    (a)	All similar fees picking up at the same origin will be modified to match the current changes
    '''    (b)	All other similar fees not picking up at the same origin that have an allocation method of Load will be set to None
    '''    (c)	Any other similar fees with an allocation method of Destination or None will remain unchanged
    ''' 5. If the modified fee’s allocation method is set to Destination apply the following rules
    '''    (a)	All similar fees delivering to the same destination will be modified to match the current changes
    '''    (b)	All other similar fees not going to the same destination that have an allocation method of Load will be set to None
    '''    (c)	Any other similar fees with an allocation method of Origin or None will remain unchanged
    ''' 6. If the modified fee’s allocation method is set to Load all other similar fees will also be set to Load and modified with the same configuration
    ''' Modified by RHR for v-8.2 on 7/3/2019
    '''  added New field "BookFeesMissingFee" to support tracking of missing expected carrier fees
    '''  the default Is false.  
    ''' </remarks>
    Friend Sub NormalizeAllocatedOrderSpecificFees(ByRef sourceFee As DataTransferObjects.BookFee, ByRef BookRevs As List(Of BookRevenue))
        If BookRevs Is Nothing OrElse BookRevs.Count < 1 Then Return
        'check for fee differences       
        If sourceFee Is Nothing OrElse sourceFee.BookFeesBookControl = 0 Then Return
        'check if this is an order specific fee,  this should always be true except for a programming error we only show an error if the Debugger.IsAttached
        If sourceFee.BookFeesAccessorialFeeTypeControl <> Utilities.AccessorialFeeType.Order Then
            If System.Diagnostics.Debugger.IsAttached Then
                throwUnExpectedFaultException("Debug Message: NormalizeAllocatedOrderSpecificFees is not allowed when the sourceFee's AccessorialFeeType is not Order ")
            End If
            Return
        End If

        'select the key fields from sourceFee
        Dim code As Integer = sourceFee.BookFeesAccessorialCode
        Dim FeeType As Integer = sourceFee.BookFeesAccessorialFeeTypeControl
        'Rule 1 get the fee with the most recient modified date for this type and code
        Dim modFee As DataTransferObjects.BookFee = (From d In BookRevs
                                                     From f In d.BookFees
                                                     Where f.BookFeesAccessorialCode = code _
                                                           And f.BookFeesAccessorialFeeTypeControl = FeeType
                                                     Order By f.BookFeesModDate Descending
                                                     Select f).FirstOrDefault()
        If modFee Is Nothing OrElse modFee.BookFeesControl = 0 Then Return 'nothing to do
        'Rule 2 If the modified fee’s modified date is before today do not make any changes to other fees on the load
        If modFee.BookFeesModDate < Date.Now.Date() Then Return 'nothing to do because no changes were made today so just return 

        Dim aType As Integer = modFee.BookFeesAccessorialFeeAllocationTypeControl
        Dim BookFeesMinimum As Decimal = modFee.BookFeesMinimum
        Dim BookFeesVariable As Double = modFee.BookFeesVariable
        Dim BookFeesVariableCode As Integer = modFee.BookFeesVariableCode
        Dim BookFeesTaxable As Boolean = modFee.BookFeesTaxable
        Dim BookFeesIsTax As Boolean = modFee.BookFeesIsTax
        Dim BookFeesTaxSortOrder As Integer = modFee.BookFeesTaxSortOrder
        Dim BookFeesTarBracketTypeControl As Integer = modFee.BookFeesTarBracketTypeControl
        Dim BookFeesBookControl As Integer = modFee.BookFeesBookControl
        Dim BookFeesControl As Integer = modFee.BookFeesControl
        Dim BookFeesAccessorialFeeCalcTypeControl = modFee.BookFeesAccessorialFeeCalcTypeControl
        'Modified by RHR for v-8.2 on 7/3/2019
        Dim BookFeesMissingFee As Boolean = modFee.BookFeesMissingFee
        'get the BookRev object associated with the modFee
        Dim sourceRev As DataTransferObjects.BookRevenue = BookRevs.Where(Function(x) x.BookControl = BookFeesBookControl).FirstOrDefault()
        If sourceRev Is Nothing OrElse sourceRev.BookControl = 0 Then Return 'nothing to do
        Dim OriginStreet As String = sourceRev.BookOrigAddress1
        Dim OriginState As String = sourceRev.BookOrigState
        Dim DestStreet As String = sourceRev.BookDestAddress1
        Dim DestState As String = sourceRev.BookDestState
        Dim FeesDiff As New List(Of BookFee)
        Dim FeesToNone As New List(Of BookFee)
        If aType = Utilities.FeeAllocationType.None Then

            'Rule 3 a Origin and Destination Order Fees Test
            FeesDiff = (From d In BookRevs
                        From f In d.BookFees
                        Where f.BookFeesOverRidden = False _
                              And f.BookFeesAccessorialCode = code _
                              And f.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Order _
                              And f.BookFeesControl <> BookFeesControl _
                              And
                              (
                                  f.BookFeesAccessorialFeeAllocationTypeControl <> aType _
                                  Or
                                  f.BookFeesMinimum <> BookFeesMinimum _
                                  Or
                                  f.BookFeesVariable <> BookFeesVariable _
                                  Or
                                  f.BookFeesVariableCode <> BookFeesVariableCode _
                                  Or
                                  f.BookFeesTaxable <> BookFeesTaxable _
                                  Or
                                  f.BookFeesIsTax <> BookFeesIsTax _
                                  Or
                                  f.BookFeesTaxSortOrder <> BookFeesTaxSortOrder _
                                  Or
                                  f.BookFeesTarBracketTypeControl <> BookFeesTarBracketTypeControl _
                                  Or
                                  f.BookFeesAccessorialFeeCalcTypeControl <> BookFeesAccessorialFeeCalcTypeControl
                                  ) _
                              And
                              (
                                  (
                                      f.BookFeesAccessorialFeeAllocationTypeControl = Utilities.FeeAllocationType.Origin _
                                      And
                                      (d.BookOrigAddress1 = OriginStreet And d.BookOrigState = OriginState)
                                      ) _
                                  Or
                                  (
                                      f.BookFeesAccessorialFeeAllocationTypeControl = Utilities.FeeAllocationType.Destination _
                                      And
                                      (d.BookDestAddress1 = DestStreet And d.BookDestState = DestState)
                                      )
                                  )
                        Select f).ToList()

            'Rule 3 b Load to None Order Fees 
            FeesToNone = (From d In BookRevs
                          From f In d.BookFees
                          Where f.BookFeesOverRidden = False _
                                And f.BookFeesAccessorialCode = code _
                                And f.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Order _
                                And f.BookFeesControl <> BookFeesControl _
                                And f.BookFeesAccessorialFeeAllocationTypeControl = Utilities.FeeAllocationType.Load
                          Select f).ToList()

        ElseIf aType = Utilities.FeeAllocationType.Origin Then
            'Rule 4 a 
            FeesDiff = (From d In BookRevs
                        From f In d.BookFees
                        Where f.BookFeesOverRidden = False _
                              And f.BookFeesAccessorialCode = code _
                              And f.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Order _
                              And f.BookFeesControl <> BookFeesControl _
                              And d.BookOrigAddress1 = OriginStreet _
                              And d.BookOrigState = OriginState _
                              And
                              (
                                  f.BookFeesAccessorialFeeAllocationTypeControl <> aType _
                                  Or
                                  f.BookFeesMinimum <> BookFeesMinimum _
                                  Or
                                  f.BookFeesVariable <> BookFeesVariable _
                                  Or
                                  f.BookFeesVariableCode <> BookFeesVariableCode _
                                  Or
                                  f.BookFeesTaxable <> BookFeesTaxable _
                                  Or
                                  f.BookFeesIsTax <> BookFeesIsTax _
                                  Or
                                  f.BookFeesTaxSortOrder <> BookFeesTaxSortOrder _
                                  Or
                                  f.BookFeesTarBracketTypeControl <> BookFeesTarBracketTypeControl _
                                  Or
                                  f.BookFeesAccessorialFeeCalcTypeControl <> BookFeesAccessorialFeeCalcTypeControl
                                  )
                        Select f).ToList()
            'Rule 4 b 
            FeesToNone = (From d In BookRevs
                          From f In d.BookFees
                          Where f.BookFeesOverRidden = False _
                                And f.BookFeesAccessorialCode = code _
                                And f.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Order _
                                And f.BookFeesControl <> BookFeesControl _
                                And f.BookFeesAccessorialFeeAllocationTypeControl = Utilities.FeeAllocationType.Load _
                                And d.BookOrigAddress1 <> OriginStreet _
                                And d.BookOrigState <> OriginState
                          Select f).ToList()

        ElseIf aType = Utilities.FeeAllocationType.Destination Then
            'Rule 5 a 
            FeesDiff = (From d In BookRevs
                        From f In d.BookFees
                        Where f.BookFeesOverRidden = False _
                              And f.BookFeesAccessorialCode = code _
                              And f.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Order _
                              And f.BookFeesControl <> BookFeesControl _
                              And d.BookDestAddress1 = DestStreet _
                              And d.BookDestState = DestState _
                              And
                              (
                                  f.BookFeesAccessorialFeeAllocationTypeControl <> aType _
                                  Or
                                  f.BookFeesMinimum <> BookFeesMinimum _
                                  Or
                                  f.BookFeesVariable <> BookFeesVariable _
                                  Or
                                  f.BookFeesVariableCode <> BookFeesVariableCode _
                                  Or
                                  f.BookFeesTaxable <> BookFeesTaxable _
                                  Or
                                  f.BookFeesIsTax <> BookFeesIsTax _
                                  Or
                                  f.BookFeesTaxSortOrder <> BookFeesTaxSortOrder _
                                  Or
                                  f.BookFeesTarBracketTypeControl <> BookFeesTarBracketTypeControl _
                                  Or
                                  f.BookFeesAccessorialFeeCalcTypeControl <> BookFeesAccessorialFeeCalcTypeControl
                                  )
                        Select f).ToList()
            'Rule 5 b 
            FeesToNone = (From d In BookRevs
                          From f In d.BookFees
                          Where f.BookFeesOverRidden = False _
                                And f.BookFeesAccessorialCode = code _
                                And f.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Order _
                                And f.BookFeesControl <> BookFeesControl _
                                And f.BookFeesAccessorialFeeAllocationTypeControl = Utilities.FeeAllocationType.Load _
                                And d.BookDestAddress1 <> DestStreet _
                                And d.BookDestState <> DestState
                          Select f).ToList()
        ElseIf aType = Utilities.FeeAllocationType.Load Then
            'Rule 6
            FeesDiff = (From d In BookRevs
                        From f In d.BookFees
                        Where f.BookFeesOverRidden = False _
                              And f.BookFeesAccessorialCode = code _
                              And f.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Order _
                              And f.BookFeesControl <> BookFeesControl _
                              And
                              (
                                  f.BookFeesAccessorialFeeAllocationTypeControl <> aType _
                                  Or
                                  f.BookFeesMinimum <> BookFeesMinimum _
                                  Or
                                  f.BookFeesVariable <> BookFeesVariable _
                                  Or
                                  f.BookFeesVariableCode <> BookFeesVariableCode _
                                  Or
                                  f.BookFeesTaxable <> BookFeesTaxable _
                                  Or
                                  f.BookFeesIsTax <> BookFeesIsTax _
                                  Or
                                  f.BookFeesTaxSortOrder <> BookFeesTaxSortOrder _
                                  Or
                                  f.BookFeesTarBracketTypeControl <> BookFeesTarBracketTypeControl _
                                  Or
                                  f.BookFeesAccessorialFeeCalcTypeControl <> BookFeesAccessorialFeeCalcTypeControl
                                  )
                        Select f).ToList()
        End If
        If Not FeesToNone Is Nothing AndAlso FeesToNone.Count() > 0 Then
            For Each fee In FeesToNone
                fee.BookFeesAccessorialFeeAllocationTypeControl = Utilities.FeeAllocationType.None
                fee.TrackingState = TrackingInfo.Updated
            Next
        End If
        If Not FeesDiff Is Nothing AndAlso FeesDiff.Count() > 0 Then
            For Each fee In FeesDiff
                With fee
                    .BookFeesAccessorialFeeAllocationTypeControl = aType
                    .BookFeesMinimum = BookFeesMinimum
                    .BookFeesVariable = BookFeesVariable
                    .BookFeesVariableCode = BookFeesVariableCode
                    .BookFeesTaxable = BookFeesTaxable
                    .BookFeesIsTax = BookFeesIsTax
                    .BookFeesTaxSortOrder = BookFeesTaxSortOrder
                    .BookFeesTarBracketTypeControl = BookFeesTarBracketTypeControl
                    .BookFeesAccessorialDependencyTypeControl = Utilities.AccessorialFeeDependencyType.Order
                    .BookFeesAccessorialOverRideReasonControl = Utilities.AccessorialFeeOverRideReasonCode.SystemOverRidden
                    .BookFeesAccessorialFeeCalcTypeControl = BookFeesAccessorialFeeCalcTypeControl
                    .BookFeesDependencyKey = BookFeesBookControl
                    .BookFeesMissingFee = BookFeesMissingFee
                    fee.TrackingState = TrackingInfo.Updated
                End With
            Next
        End If
    End Sub


    ''' <summary>
    ''' Updates existing allocted fees that do not match sourceFee configuration to match sourceFee configuration
    ''' the blnManualOverride flag determins if this fee should take precedence over other fees and determines 
    ''' if the new business rules for 7.0.5.102 are in effect
    ''' </summary>
    ''' <param name="sourceFee"></param>
    ''' <param name="BookRevs"></param>
    ''' <remarks>
    ''' Modified by RHR 9/15/14 changed key fields query to use Or instead of And
    ''' Modified by RHR v-7.0.5.102 8/11/2016
    '''   added logic to process manual updates to Order specific fees when the allocation type is Load (or other)
    '''   bypassing the default behavior if the last fee modified was changed in the last 180 seconds (3 minutes)
    '''   Modified Business rules:
    '''   1) if the modified order is set to Allocation  NONE (1) set any fees with the same Accessorial Code as Load (4) to NONE (1)
    '''   2) if the modified order is set to Origin (2) update any fees with the same Accessorial Code and the same Origin to use the same exact settings 
    '''      Set any other with the same Accessorial Code to use Allocation of NONE (1) {LOAD is not allowed if any one fee is set to Origin}
    '''   3) if the modified order is set to Destination (3) update any fees with the same Accessorial Code and the same Destination to use the same exact settings 
    '''      Set any other with the same Accessorial Code to use Allocation of NONE (1) {LOAD is not allowed if any one fee is set to Destination}
    '''   4) if the modified order is set to Load (4) update all fees with the same Accessorial Code to use the same exact settings
    ''' Modified by RHR v-7.0.5.110 7/13/2016
    ''' if the source fee is an order specific fee we now call NormalizeAllocatedOrderSpecificFees to update similare fees using new business rules 
    ''' Modified by RHR for v-8.2 on 7/3/2019
    '''  added New field "BookFeesMissingFee" to support tracking of missing expected carrier fees
    '''  the default Is false.  
    ''' </remarks>
    Friend Sub NormalizeAllocatedFees(ByRef sourceFee As DataTransferObjects.BookFee, ByRef BookRevs As List(Of BookRevenue), Optional ByVal blnManualOverride As Boolean = False)

        Using operation = Logger.StartActivity("NormalizeAllocatedFees for {SourceFee} with blnManualOverride {ManualOverride}", sourceFee.BookFeesCaption, blnManualOverride)
            'check for nulls")
            If BookRevs Is Nothing OrElse BookRevs.Count < 1 Then Return
            'check for fee differences       
            If sourceFee Is Nothing OrElse sourceFee.BookFeesBookControl = 0 Then Return
            'Begin Modified by RHR v-7.0.5.110 7/13/2016
            If sourceFee.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Order Then
                NormalizeAllocatedOrderSpecificFees(sourceFee, BookRevs)
                Return
            End If
            'End Modified by RHR v-7.0.5.110 7/13/2016
            'select the key fields from maxfee
            Dim code As Integer = sourceFee.BookFeesAccessorialCode
            Dim aType As Integer = sourceFee.BookFeesAccessorialFeeAllocationTypeControl
            Dim FeeType As Integer = sourceFee.BookFeesAccessorialFeeTypeControl
            Dim BookFeesMinimum As Decimal = sourceFee.BookFeesMinimum
            Dim BookFeesVariable As Double = sourceFee.BookFeesVariable
            Dim BookFeesVariableCode As Integer = sourceFee.BookFeesVariableCode
            Dim BookFeesTaxable As Boolean = sourceFee.BookFeesTaxable
            Dim BookFeesIsTax As Boolean = sourceFee.BookFeesIsTax
            Dim BookFeesTaxSortOrder As Integer = sourceFee.BookFeesTaxSortOrder
            Dim BookFeesTarBracketTypeControl As Integer = sourceFee.BookFeesTarBracketTypeControl
            Dim BookFeesBookControl As Integer = sourceFee.BookFeesBookControl
            ' Modified by RHR for v-8.2 on 7/3/2019
            Dim BookFeesMissingFee = sourceFee.BookFeesMissingFee
            'get the BookRev object associated with the sourceFee
            Dim sourceRev As DataTransferObjects.BookRevenue = BookRevs.Where(Function(x) x.BookControl = BookFeesBookControl).FirstOrDefault()
            If sourceRev Is Nothing OrElse sourceRev.BookControl = 0 Then Return 'nothing to do
            Dim BookPickupStopNumber As Integer = sourceRev.BookPickupStopNumber
            Dim BookStopNo As Integer = sourceRev.BookStopNo
            Dim BookOrigAddress1 As String = If(String.IsNullOrWhiteSpace(sourceRev.BookOrigAddress1), "*NA*", sourceRev.BookOrigAddress1)
            Dim BookOrigCity As String = If(String.IsNullOrWhiteSpace(sourceRev.BookOrigCity), "*NA*", sourceRev.BookOrigCity)
            Dim BookOrigState As String = If(String.IsNullOrWhiteSpace(sourceRev.BookOrigState), "*NA*", sourceRev.BookOrigState)
            Dim BookDestAddress1 As String = If(String.IsNullOrWhiteSpace(sourceRev.BookDestAddress1), "*NA*", sourceRev.BookDestAddress1)
            Dim BookDestCity As String = If(String.IsNullOrWhiteSpace(sourceRev.BookDestCity), "*NA*", sourceRev.BookDestCity)
            Dim BookDestState As String = If(String.IsNullOrWhiteSpace(sourceRev.BookDestState), "*NA*", sourceRev.BookDestState)
            Dim NewAccessorialFeeAllocationTypeControl = 0

            'get a list of fees that have a different configuration for this code 
            Dim FeesDiff As List(Of BookFee) = (From d In BookRevs
                                                From f In d.BookFees
                                                Where f.BookFeesOverRidden = False _
                                                      And f.BookFeesAccessorialCode = code _
                                                      And
                                                      (
                                                          f.BookFeesAccessorialFeeTypeControl <> FeeType _
                                                          Or
                                                          f.BookFeesAccessorialFeeAllocationTypeControl <> aType _
                                                          Or
                                                          f.BookFeesMinimum <> BookFeesMinimum _
                                                          Or
                                                          f.BookFeesVariable <> BookFeesVariable _
                                                          Or
                                                          f.BookFeesVariableCode <> BookFeesVariableCode _
                                                          Or
                                                          f.BookFeesTaxable <> BookFeesTaxable _
                                                          Or
                                                          f.BookFeesIsTax <> BookFeesIsTax _
                                                          Or
                                                          f.BookFeesTaxSortOrder <> BookFeesTaxSortOrder _
                                                          Or
                                                          f.BookFeesTarBracketTypeControl <> BookFeesTarBracketTypeControl
                                                          ) _
                                                      And
                                                      (
                                                          (blnManualOverride = True) _
                                                          Or
                                                          (
                                                              (aType = Utilities.FeeAllocationType.Origin And d.BookPickupStopNumber = BookPickupStopNumber) _
                                                              Or
                                                              (aType = Utilities.FeeAllocationType.Destination And d.BookStopNo = BookStopNo) _
                                                              Or
                                                              (aType = Utilities.FeeAllocationType.Load) _
                                                              Or
                                                              (aType = Utilities.FeeAllocationType.None And f.BookFeesBookControl = BookFeesBookControl)
                                                              )
                                                          )
                                                Order By f.BookFeesBookControl, f.BookFeesAccessorialFeeTypeControl Descending, d.BookTotalWgt Descending
                                                Select f).ToList()
            Dim intPreviousBookControl As Integer = 0
            Dim intPreviousAccessorialFeeTypeControl As Integer = 0
            If Not FeesDiff Is Nothing AndAlso FeesDiff.Count > 0 Then
                'we need to normalize the allocated fees
                For Each fee In FeesDiff
                    If fee.BookFeesBookControl = intPreviousBookControl And fee.BookFeesAccessorialFeeTypeControl = intPreviousAccessorialFeeTypeControl Then
                        'this is a duplcate so just mark it as overridden
                        fee.BookFeesOverRidden = True
                        fee.BookFeesValue = 0
                        fee.BookFeesAccessorialOverRideReasonControl = Utilities.AccessorialFeeOverRideReasonCode.SystemOverRidden
                        fee.TrackingState = TrackingInfo.Updated
                        'Begin Modified by RHR v-7.0.5.110 7/13/2016
                    ElseIf fee.BookFeesAccessorialFeeTypeControl <> Utilities.AccessorialFeeType.Order Then
                        'If the fee is a tariff or lane specific fee mark it as OverRidden with a reason code 
                        'of “System OverRidden” 
                        'create removed by RHR 9/15/14 (not sure why we would create an order specific fee when we mark the lane or tariff fee overridden?)
                        ''then create a new order specific fee using the correct 
                        ''configuration settings; marked as Order Dependent, using the maxFee BookControl 
                        ''number as the key, with a reason code of “System OverRidden”
                        fee.BookFeesOverRidden = True
                        fee.BookFeesValue = 0
                        fee.BookFeesAccessorialOverRideReasonControl = Utilities.AccessorialFeeOverRideReasonCode.SystemOverRidden
                        fee.TrackingState = TrackingInfo.Updated
               
                    Else
                        Dim blnUpdateThisFee As Boolean = False
                        Dim blnUpdateAllocationTypeOnly As Boolean = False
                        NewAccessorialFeeAllocationTypeControl = sourceFee.BookFeesAccessorialFeeAllocationTypeControl
                        If blnManualOverride Then
                            'rule one
                            Select Case sourceFee.BookFeesAccessorialFeeAllocationTypeControl
                                Case 1 'rule one
                                    If fee.BookFeesAccessorialFeeAllocationTypeControl = 4 Then
                                        blnUpdateAllocationTypeOnly = True
                                        NewAccessorialFeeAllocationTypeControl = 1
                                    End If
                                Case 2 'rule two
                                    Dim feeRev As DataTransferObjects.BookRevenue = BookRevs.Where(Function(x) x.BookControl = fee.BookFeesBookControl).FirstOrDefault()
                                    If Not feeRev Is Nothing AndAlso feeRev.BookControl <> 0 Then
                                        If feeRev.BookOrigAddress1 = BookOrigAddress1 _
                                           And feeRev.BookOrigCity = BookOrigCity _
                                           And feeRev.BookOrigState = BookOrigState Then blnUpdateThisFee = True
                                    End If
                                    If Not blnUpdateThisFee Then
                                        blnUpdateAllocationTypeOnly = True
                                        NewAccessorialFeeAllocationTypeControl = 1
                                    End If
                                Case 3 'rule three
                                    Dim feeRev As DataTransferObjects.BookRevenue = BookRevs.Where(Function(x) x.BookControl = fee.BookFeesBookControl).FirstOrDefault()
                                    If Not feeRev Is Nothing AndAlso feeRev.BookControl <> 0 Then
                                        If feeRev.BookDestAddress1 = BookDestAddress1 _
                                           And feeRev.BookDestCity = BookDestCity _
                                           And feeRev.BookDestState = BookDestState Then blnUpdateThisFee = True
                                    End If
                                    If Not blnUpdateThisFee Then
                                        blnUpdateAllocationTypeOnly = True
                                        NewAccessorialFeeAllocationTypeControl = 1
                                    End If
                                Case Else
                                    'rule four
                                    'standard rule
                                    blnUpdateThisFee = True
                            End Select


                        Else
                            'apply standard rules and just update it
                            'this is an order specific fee so just update it.
                            blnUpdateThisFee = True
                        End If
                        If blnUpdateThisFee Then
                            With fee
                                intPreviousBookControl = .BookFeesBookControl
                                .BookFeesAccessorialFeeAllocationTypeControl = aType
                                .BookFeesMinimum = BookFeesMinimum
                                .BookFeesVariable = BookFeesVariable
                                .BookFeesVariableCode = BookFeesVariableCode
                                .BookFeesTaxable = BookFeesTaxable
                                .BookFeesIsTax = BookFeesIsTax
                                .BookFeesTaxSortOrder = BookFeesTaxSortOrder
                                .BookFeesTarBracketTypeControl = BookFeesTarBracketTypeControl
                                .BookFeesAccessorialDependencyTypeControl = Utilities.AccessorialFeeDependencyType.Order
                                .BookFeesAccessorialOverRideReasonControl = Utilities.AccessorialFeeOverRideReasonCode.SystemOverRidden
                                .BookFeesAccessorialFeeCalcTypeControl = Utilities.FeeCalcType.Unique
                                .BookFeesDependencyKey = sourceFee.BookFeesBookControl
                                .BookFeesMissingFee = BookFeesMissingFee
                                fee.TrackingState = TrackingInfo.Updated
                            End With
                        ElseIf blnUpdateAllocationTypeOnly Then
                            With fee
                                .BookFeesAccessorialFeeAllocationTypeControl = NewAccessorialFeeAllocationTypeControl
                                fee.TrackingState = TrackingInfo.Updated
                            End With
                        End If
                    End If
                    intPreviousBookControl = fee.BookFeesBookControl
                    intPreviousAccessorialFeeTypeControl = fee.BookFeesAccessorialFeeTypeControl
                Next
            End If
        End Using

    End Sub

    ''' <summary>
    ''' Updates existing allocted fees that do not match sourceFee configuration to match sourceFee configuration
    ''' Note uses instance specific LinqDB datacontext and the caller must submit the changes.
    ''' </summary>
    ''' <param name="sourceFee"></param>
    ''' <param name="BookRevs"></param>
    ''' <remarks>
    ''' Modified by RHR v-7.0.5.110 7/13/2016
    ''' if the source fee is an order specific fee we now call NormalizeAllocatedOrderSpecificFees to update similare fees using new business rules 
    ''' </remarks>
    Friend Sub NormalizeAllocatedFees(ByRef sourceFee As DataTransferObjects.BookFee, ByRef BookRevs As List(Of LTS.Book), ByVal BookPickupStopNumber As Integer, ByVal BookStopNo As Short)
        Using operation = Logger.StartActivity("NormalizeAllocatedFees for {SourceFee} with BookPickupStopNumber {PickupStopNumber} and BookStopNo {StopNo}", sourceFee.BookFeesCaption, BookPickupStopNumber, BookStopNo)


            If BookRevs Is Nothing OrElse BookRevs.Count < 1 Then Return
            'check for fee differences       
            If sourceFee Is Nothing OrElse sourceFee.BookFeesBookControl = 0 Then Return
            'Begin Modified by RHR v-7.0.5.110 7/13/2016
            Logger.Information("Checking if the sourceFee ({FeeType}) is an {OrderFeeType} Specific Fee", sourceFee.BookFeesAccessorialFeeTypeControl, Utilities.AccessorialFeeType.Order)


            If sourceFee.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Order Then
                NormalizeAllocatedOrderSpecificFees(sourceFee, BookRevs, BookPickupStopNumber, BookStopNo)
                operation.Complete()
                Return
            End If
            'End Modified by RHR v-7.0.5.110 7/13/2016
            'select the key fields from maxfee
            Dim code As Integer = sourceFee.BookFeesAccessorialCode
            Dim aType As Integer = sourceFee.BookFeesAccessorialFeeAllocationTypeControl
            Dim FeeType As Integer = sourceFee.BookFeesAccessorialFeeTypeControl
            Dim BookFeesMinimum As Decimal = sourceFee.BookFeesMinimum
            Dim BookFeesVariable As Double = sourceFee.BookFeesVariable
            Dim BookFeesVariableCode As Integer = sourceFee.BookFeesVariableCode
            Dim BookFeesTaxable As Boolean = sourceFee.BookFeesTaxable
            Dim BookFeesIsTax As Boolean = sourceFee.BookFeesIsTax
            Dim BookFeesTaxSortOrder As Integer = sourceFee.BookFeesTaxSortOrder
            Dim BookFeesTarBracketTypeControl As Integer = sourceFee.BookFeesTarBracketTypeControl
            Dim BookFeesBookControl As Integer = sourceFee.BookFeesBookControl

            'get a list of fees that have a different configuration for this code 
            Dim FeesDiff As List(Of LTS.BookFee) = (From d In BookRevs
                                                    From f In d.BookFees
                                                    Where f.BookFeesOverRidden = False _
                                                          And f.BookFeesAccessorialCode = code _
                                                          And
                                                          (
                                                              f.BookFeesAccessorialFeeTypeControl <> FeeType _
                                                              Or
                                                              f.BookFeesAccessorialFeeAllocationTypeControl <> aType _
                                                              Or
                                                              f.BookFeesMinimum <> BookFeesMinimum _
                                                              Or
                                                              f.BookFeesVariable <> BookFeesVariable _
                                                              Or
                                                              f.BookFeesVariableCode <> BookFeesVariableCode _
                                                              Or
                                                              f.BookFeesTaxable <> BookFeesTaxable _
                                                              Or
                                                              f.BookFeesIsTax <> BookFeesIsTax _
                                                              Or
                                                              f.BookFeesTaxSortOrder <> BookFeesTaxSortOrder _
                                                              Or
                                                              f.BookFeesTarBracketTypeControl <> BookFeesTarBracketTypeControl
                                                              ) _
                                                          And
                                                          (
                                                              (aType = Utilities.FeeAllocationType.Origin And d.BookPickupStopNumber = BookPickupStopNumber) _
                                                              Or
                                                              (aType = Utilities.FeeAllocationType.Destination And d.BookStopNo = BookStopNo) _
                                                              Or
                                                              (aType = Utilities.FeeAllocationType.Load) _
                                                              Or
                                                              (aType = Utilities.FeeAllocationType.None And f.BookFeesBookControl = BookFeesBookControl)
                                                              )
                                                    Order By f.BookFeesBookControl, f.BookFeesAccessorialFeeTypeControl Descending, d.BookTotalWgt Descending
                                                    Select f).ToList()
            Dim intPreviousBookControl As Integer = 0
            Dim intPreviousAccessorialFeeTypeControl As Integer = 0
            If Not FeesDiff Is Nothing AndAlso FeesDiff.Count > 0 Then
                'we need to normalize the allocated fees
                For Each fee In FeesDiff
                    Logger.Information("Checking if the fee is a duplicate of the previous fee")
                    If fee.BookFeesBookControl = intPreviousBookControl And fee.BookFeesAccessorialFeeTypeControl = intPreviousAccessorialFeeTypeControl Then
                        'this is a duplcate so just mark it as overridden
                        fee.BookFeesOverRidden = True
                        fee.BookFeesAccessorialOverRideReasonControl = Utilities.AccessorialFeeOverRideReasonCode.SystemOverRidden
                        fee.BookFeesValue = 0
                        'Begin Modified by RHR v-7.0.5.110 7/13/2016
                    ElseIf fee.BookFeesAccessorialFeeTypeControl <> Utilities.AccessorialFeeType.Order Then
                        Logger.Information("Checking if the fee is a tariff or lane specific fee")
                        fee.BookFeesOverRidden = True
                        fee.BookFeesAccessorialOverRideReasonControl = Utilities.AccessorialFeeOverRideReasonCode.SystemOverRidden
                        fee.BookFeesValue = 0
                        'End Modified by RHR v-7.0.5.110 7/13/2016                    
                    End If

                    intPreviousBookControl = fee.BookFeesBookControl
                    intPreviousAccessorialFeeTypeControl = fee.BookFeesAccessorialFeeTypeControl
                Next
            End If
        End Using

    End Sub

    ''' <summary>
    ''' Called after fees are normalized. inserts any missing fees for each matching allocated fee
    ''' </summary>
    ''' <param name="sourceFee"></param>
    ''' <param name="BookRevs"></param>
    ''' <remarks>
    ''' Modified by RHR 9/15/14.  we do not compare BookFeesAccessorialFeeAllocationTypeControl when searching for a match
    ''' we assume that the NormalizeAllocatedFees corrects this issue. 
    ''' All fees witht he same Accessorial Code on a consolidation must have the same allocation type
    ''' Modified by RHR for v-7.0.5.103 on 01/24/2017 
    '''   Added logic to only add additional fees if the street address matches when costs are allocated by orig or dest.
    ''' Modified by RHR for v-8.2 on 7/3/2019
    '''  added New field "BookFeesMissingFee" to support tracking of missing expected carrier fees
    '''  the default Is false.  
    ''' </remarks>
    Friend Sub InsertMissingAllocatedFees(ByRef sourceFee As DataTransferObjects.BookFee, ByRef BookRevs As List(Of BookRevenue))
        'Now that the exiting fees are normalized we need to insert any missing fees for each allocated order; 
        'so, select a list of orders associated with any Allocation Type that is not “None” and that does not 
        'have a valid fee (Non-OverRidden fee that matches the configuration settings)
        Using operation = Logger.StartActivity("InsertMissingAllocatedFees for {SourceFee}", sourceFee.BookFeesCaption)

            If BookRevs Is Nothing OrElse BookRevs.Count < 1 Then Return
            If sourceFee Is Nothing OrElse sourceFee.BookFeesBookControl = 0 Then Return
            'select the key fields from maxfee
            Dim BookControl As Integer = sourceFee.BookFeesBookControl
            Dim bookFeesControl As Integer = sourceFee.BookFeesControl
            Dim code As Integer = sourceFee.BookFeesAccessorialCode
            Dim aType As Integer = sourceFee.BookFeesAccessorialFeeAllocationTypeControl
            'Dim BookFeesMinimum As Decimal = sourceFee.BookFeesMinimum
            'Dim BookFeesVariable As Double = sourceFee.BookFeesVariable
            'Dim BookFeesVariableCode As Integer = sourceFee.BookFeesVariableCode
            'Dim BookFeesTaxable As Boolean = sourceFee.BookFeesTaxable
            'Dim BookFeesIsTax As Boolean = sourceFee.BookFeesIsTax
            'Dim BookFeesTaxSortOrder As Integer = sourceFee.BookFeesTaxSortOrder
            'Dim BookFeesTarBracketTypeControl As Integer = sourceFee.BookFeesTarBracketTypeControl
            ' Modified by RHR for v-8.2 on 7/3/2019
            Dim BookFeesMissingFee = sourceFee.BookFeesMissingFee
            'get the BookRev object associated with the sourceFee
            Dim sourceRev As DataTransferObjects.BookRevenue = BookRevs.Where(Function(x) x.BookControl = BookControl).FirstOrDefault()
            If sourceRev Is Nothing OrElse sourceRev.BookControl = 0 Then Return 'nothing to do
            Dim BookPickupStopNumber As Integer = sourceRev.BookPickupStopNumber
            Dim BookStopNo As Integer = sourceRev.BookStopNo
            'Modified by RHR for v-7.0.5.103 on 01/24/2017
            'Begin new variables for address validation
            Dim BookOrigAddress1 As String = If(String.IsNullOrWhiteSpace(sourceRev.BookOrigAddress1), "*NA*", sourceRev.BookOrigAddress1)
            Dim BookOrigCity As String = If(String.IsNullOrWhiteSpace(sourceRev.BookOrigCity), "*NA*", sourceRev.BookOrigCity)
            Dim BookOrigState As String = If(String.IsNullOrWhiteSpace(sourceRev.BookOrigState), "*NA*", sourceRev.BookOrigState)
            Dim BookDestAddress1 As String = If(String.IsNullOrWhiteSpace(sourceRev.BookDestAddress1), "*NA*", sourceRev.BookDestAddress1)
            Dim BookDestCity As String = If(String.IsNullOrWhiteSpace(sourceRev.BookDestCity), "*NA*", sourceRev.BookDestCity)
            Dim BookDestState As String = If(String.IsNullOrWhiteSpace(sourceRev.BookDestState), "*NA*", sourceRev.BookDestState)
            'End changes for v-7.0.5.103

            Dim MissingFees As New List(Of BookRevenue)
            Select Case aType
                Case Utilities.FeeAllocationType.Origin
                    'get a list of BookRevs that have the same BookPickupStopNumber as sourceRev but does not 
                    'have a matching accessorial code and is not overwritten
                    MissingFees = BookRevs.Where(Function(t1) t1.BookPickupStopNumber = BookPickupStopNumber And t1.BookControl <> BookControl And Not t1.BookFees.Any(Function(t2) t2.BookFeesAccessorialCode = code And t2.BookFeesOverRidden = False)).ToList()
                Case Utilities.FeeAllocationType.Destination
                    MissingFees = BookRevs.Where(Function(t1) t1.BookStopNo = BookStopNo And t1.BookControl <> BookControl And Not t1.BookFees.Any(Function(t2) t2.BookFeesAccessorialCode = code And t2.BookFeesOverRidden = False)).ToList()
                Case Utilities.FeeAllocationType.Load
                    MissingFees = BookRevs.Where(Function(t1) t1.BookControl <> BookControl And Not t1.BookFees.Any(Function(t2) t2.BookFeesAccessorialCode = code And t2.BookFeesOverRidden = False)).ToList()
            End Select
            If MissingFees Is Nothing OrElse MissingFees.Count < 1 Then Return 'nothing to do
            Dim intPreviousBookControl As Integer = 0

            For Each rev In MissingFees
                'make sure we only add the fee once for each order
                Logger.Information("Checking if the fee is a duplicate of the previous fee")
                If rev.BookControl <> intPreviousBookControl Then
                    'Modified by RHR for v-7.0.5.103 on 01/24/2017
                    'add logic to be sure the address matches even if the stop or pickup numbers are the same
                    'stop and pickup numbers may not have been assigned properly yet.
                    Dim blnCloneFee As Boolean = True

                    If aType = Utilities.FeeAllocationType.Origin Then
                        If Not (BookOrigAddress1 = If(String.IsNullOrWhiteSpace(rev.BookOrigAddress1), "*SKIP*", rev.BookOrigAddress1) _
                                And BookOrigCity = If(String.IsNullOrWhiteSpace(rev.BookOrigCity), "*SKIP*", rev.BookOrigCity) _
                                And BookOrigState = If(String.IsNullOrWhiteSpace(rev.BookOrigState), "*SKIP*", rev.BookOrigState)) Then
                            blnCloneFee = False
                        End If
                    End If

                    If aType = Utilities.FeeAllocationType.Destination Then
                        If Not (BookDestAddress1 = If(String.IsNullOrWhiteSpace(rev.BookDestAddress1), "*SKIP*", rev.BookDestAddress1) _
                                And BookDestCity = If(String.IsNullOrWhiteSpace(rev.BookDestCity), "*SKIP*", rev.BookDestCity) _
                                And BookDestState = If(String.IsNullOrWhiteSpace(rev.BookDestState), "*SKIP*", rev.BookDestState)) Then
                            blnCloneFee = False
                        End If
                    End If
                    If blnCloneFee Then


                        'add an order specific fee that matches the sourceFee
                        Dim orderFee As DataTransferObjects.BookFee = sourceFee.Clone
                        With orderFee
                            .BookFeesBookControl = rev.BookControl
                            .BookFeesControl = 0
                            .BookFeesUpdated = New Byte() {}
                            .BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Order
                            .BookFeesAccessorialDependencyTypeControl = Utilities.AccessorialFeeDependencyType.Order
                            .BookFeesAccessorialOverRideReasonControl = Utilities.AccessorialFeeOverRideReasonCode.SystemOverRidden
                            .BookFeesDependencyKey = sourceFee.BookFeesBookControl.ToString()
                            .BookFeesMissingFee = BookFeesMissingFee
                            .TrackingState = TrackingInfo.Created
                        End With
                        rev.BookFees.Add(orderFee)
                    End If
                End If
                intPreviousBookControl = rev.BookControl
            Next
        End Using

    End Sub

    ''' <summary>
    ''' Called after fees are normalized. inserts any missing fees for each matching allocated fee
    ''' Note uses instance specific LinqDB datacontext and the caller must submit the changes.
    ''' </summary>
    ''' <param name="sourceFee"></param>
    ''' <param name="BookRevs"></param>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.5.103 on 01/24/2017 
    '''   Added logic to only add additional fees if the street address matches when costs are allocated by orig or dest.
    ''' Modified by RHR for v-8.0 on 01/15/2018 merge changes with v-7.0.6.105
    ''' Modified by RHR for v-8.2 on 7/3/2019
    '''  added New field "BookFeesMissingFee" to support tracking of missing expected carrier fees
    '''  the default Is false.  
    ''' </remarks>
    Friend Sub InsertMissingAllocatedFees(ByRef sourceFee As DataTransferObjects.BookFee, ByRef BookRevs As List(Of LTS.Book), ByVal BookPickupStopNumber As Integer, ByVal BookStopNo As Short)
        'Now that the exiting fees are normalized we need to insert any missing fees for each allocated order; 
        'so, select a list of orders associated with any Allocation Type that is not “None” and that does not 
        'have a valid fee (Non-OverRidden fee that matches the configuration settings)
        Using operation = Logger.StartActivity("InsertMissingAllocatedFees for {SourceFee} with BookPickupStopNumber {PickupStopNumber} and BookStopNo {StopNo}", sourceFee.BookFeesCaption, BookPickupStopNumber, BookStopNo)
            If BookRevs Is Nothing OrElse BookRevs.Count < 1 Then Return
            If sourceFee Is Nothing OrElse sourceFee.BookFeesBookControl = 0 Then Return
            'select the key fields from maxfee
            Dim BookControl As Integer = sourceFee.BookFeesBookControl
            Dim bookFeesControl As Integer = sourceFee.BookFeesControl
            Dim code As Integer = sourceFee.BookFeesAccessorialCode
            Dim aType As Integer = sourceFee.BookFeesAccessorialFeeAllocationTypeControl
            Dim BookFeesMinimum As Decimal = sourceFee.BookFeesMinimum
            Dim BookFeesVariable As Double = sourceFee.BookFeesVariable
            Dim BookFeesVariableCode As Integer = sourceFee.BookFeesVariableCode
            Dim BookFeesTaxable As Boolean = sourceFee.BookFeesTaxable
            Dim BookFeesIsTax As Boolean = sourceFee.BookFeesIsTax
            Dim BookFeesTaxSortOrder As Integer = sourceFee.BookFeesTaxSortOrder
            'Modified by RHR for v-8.0 on 01/15/2018 merge changes with v-7.0.6.105
            Dim BookFeesTarBracketTypeControl As Integer = sourceFee.BookFeesTarBracketTypeControl
            ' Modified by RHR for v-8.2 on 7/3/2019
            Dim BookFeesMissingFee = sourceFee.BookFeesMissingFee
            'Modified by RHR for v-7.0.5.103 on 01/24/2017
            'Begin new variables for address validation
            Dim sourceRev As LTS.Book = BookRevs.Where(Function(x) x.BookControl = BookControl).FirstOrDefault()
            If sourceRev Is Nothing OrElse sourceRev.BookControl = 0 Then Return 'nothing to do
            Dim BookOrigAddress1 As String = If(String.IsNullOrWhiteSpace(sourceRev.BookOrigAddress1), "*NA*", sourceRev.BookOrigAddress1)
            Dim BookOrigCity As String = If(String.IsNullOrWhiteSpace(sourceRev.BookOrigCity), "*NA*", sourceRev.BookOrigCity)
            Dim BookOrigState As String = If(String.IsNullOrWhiteSpace(sourceRev.BookOrigState), "*NA*", sourceRev.BookOrigState)
            Dim BookDestAddress1 As String = If(String.IsNullOrWhiteSpace(sourceRev.BookDestAddress1), "*NA*", sourceRev.BookDestAddress1)
            Dim BookDestCity As String = If(String.IsNullOrWhiteSpace(sourceRev.BookDestCity), "*NA*", sourceRev.BookDestCity)
            Dim BookDestState As String = If(String.IsNullOrWhiteSpace(sourceRev.BookDestState), "*NA*", sourceRev.BookDestState)


            Dim MissingFees As New List(Of LTS.Book)
            Select Case aType
                Case Utilities.FeeAllocationType.Origin
                    'get a list of BookRevs that have the same BookPickupStopNumber as sourceRev but does not 
                    'have a matching accessorial code and is not overwritten
                    MissingFees = BookRevs.Where(Function(t1) t1.BookPickupStopNumber = BookPickupStopNumber And t1.BookControl <> BookControl And Not t1.BookFees.Any(Function(t2) t2.BookFeesAccessorialCode = code And t2.BookFeesOverRidden = False)).ToList()
                Case Utilities.FeeAllocationType.Destination
                    MissingFees = BookRevs.Where(Function(t1) t1.BookStopNo = BookStopNo And t1.BookControl <> BookControl And Not t1.BookFees.Any(Function(t2) t2.BookFeesAccessorialCode = code And t2.BookFeesOverRidden = False)).ToList()
                Case Utilities.FeeAllocationType.Load
                    MissingFees = BookRevs.Where(Function(t1) t1.BookControl <> BookControl And Not t1.BookFees.Any(Function(t2) t2.BookFeesAccessorialCode = code And t2.BookFeesOverRidden = False)).ToList()
            End Select
            If MissingFees Is Nothing OrElse MissingFees.Count < 1 Then Return 'nothing to do
            Dim intPreviousBookControl As Integer = 0
            For Each rev In MissingFees
                'make sure we only add the fee once for each order
                If rev.BookControl <> intPreviousBookControl Then
                    'Modified by RHR for v-7.0.5.103 on 01/24/2017
                    'add logic to be sure the address matches even if the stop or pickup numbers are the same
                    'stop and pickup numbers may not have been assigned properly yet.
                    Dim blnCloneFee As Boolean = True

                    If aType = Utilities.FeeAllocationType.Origin Then
                        If Not (BookOrigAddress1 = If(String.IsNullOrWhiteSpace(rev.BookOrigAddress1), "*SKIP*", rev.BookOrigAddress1) _
                                And BookOrigCity = If(String.IsNullOrWhiteSpace(rev.BookOrigCity), "*SKIP*", rev.BookOrigCity) _
                                And BookOrigState = If(String.IsNullOrWhiteSpace(rev.BookOrigState), "*SKIP*", rev.BookOrigState)) Then
                            blnCloneFee = False
                        End If
                    End If

                    If aType = Utilities.FeeAllocationType.Destination Then
                        If Not (BookDestAddress1 = If(String.IsNullOrWhiteSpace(rev.BookDestAddress1), "*SKIP*", rev.BookDestAddress1) _
                                And BookDestCity = If(String.IsNullOrWhiteSpace(rev.BookDestCity), "*SKIP*", rev.BookDestCity) _
                                And BookDestState = If(String.IsNullOrWhiteSpace(rev.BookDestState), "*SKIP*", rev.BookDestState)) Then
                            blnCloneFee = False
                        End If
                    End If

                    If blnCloneFee Then
                        'add an order specific fee that matches the sourceFee
                        Dim orderFee As LTS.BookFee = NGLBookFeeData.selectLTSData(sourceFee, Me.Parameters.UserName)
                        With orderFee
                            .BookFeesBookControl = rev.BookControl
                            .BookFeesControl = 0
                            .BookFeesUpdated = New Byte() {}
                            .BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Order
                            .BookFeesAccessorialDependencyTypeControl = Utilities.AccessorialFeeDependencyType.Order
                            .BookFeesAccessorialOverRideReasonControl = Utilities.AccessorialFeeOverRideReasonCode.SystemOverRidden
                            .BookFeesDependencyKey = sourceFee.BookFeesBookControl.ToString()
                            .BookFeesMissingFee = BookFeesMissingFee
                        End With
                        rev.BookFees.Add(orderFee)
                        'be sure this fee gets inserted to the db on submit
                        CType(Me.LinqDB, NGLMasBookDataContext).BookFees.InsertOnSubmit(orderFee)
                    End If
                End If
                intPreviousBookControl = rev.BookControl
            Next
        End Using

    End Sub


    ''' <summary>
    ''' Called after a fee is marked as deleted. 
    ''' Note uses instance specific LinqDB datacontext and the caller must submit the changes.
    ''' </summary>
    ''' <param name="sourceFee"></param>
    ''' <param name="Books"></param>
    ''' <param name="BookPickupStopNumber"></param>
    ''' <param name="BookStopNo"></param>
    ''' <remarks></remarks>
    Friend Sub DeleteDependentAllocatedFees(ByRef sourceFee As DataTransferObjects.BookFee, ByRef Books As List(Of LTS.Book), ByVal BookPickupStopNumber As Integer, ByVal BookStopNo As Short)
        'select a list of orders associated with any Allocation Type that is not “None” and mark it as delete on submit

        Using Logger.StartActivity("DeleteDependentAllocatedFees for {SourceFee} with BookPickupStopNumber {PickupStopNumber} and BookStopNo {StopNo}", sourceFee.BookFeesCaption, BookPickupStopNumber, BookStopNo)
            If Books Is Nothing OrElse Books.Count < 1 Then Return
            If sourceFee Is Nothing OrElse sourceFee.BookFeesBookControl = 0 Then Return
            'select the key fields from maxfee
            Dim BookControl As Integer = sourceFee.BookFeesBookControl
            Dim bookFeesControl As Integer = sourceFee.BookFeesControl
            Dim code As Integer = sourceFee.BookFeesAccessorialCode
            Dim aType As Integer = sourceFee.BookFeesAccessorialFeeAllocationTypeControl
            Dim DependentFees As New List(Of LTS.BookFee)
            Select Case aType
                Case Utilities.FeeAllocationType.Origin
                    'get a list of BookRevs that have the same BookPickupStopNumber as sourceRev but does not 
                    'have a matching accessorial code and is not overwritten
                    'DependentFees = Books.Where(Function(t1) t1.BookPickupStopNumber = BookPickupStopNumber And (t1.BookFees.Where(Function(t2) t2.BookFeesAccessorialCode = code And t2.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Order))).ToList()
                    DependentFees = (From d In Books From f In d.BookFees Where d.BookPickupStopNumber = BookPickupStopNumber And f.BookFeesAccessorialCode = code And f.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Order Select f).ToList()
                Case Utilities.FeeAllocationType.Destination
                    'DependentFees = Books.Where(Function(t1) t1.BookStopNo = BookStopNo And t1.BookFees.Any(Function(t2) t2.BookFeesAccessorialCode = code And t2.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Order)).ToList()
                    DependentFees = (From d In Books From f In d.BookFees Where d.BookStopNo = BookStopNo And f.BookFeesAccessorialCode = code And f.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Order Select f).ToList()
                Case Utilities.FeeAllocationType.Load
                    'DependentFees = Books.Where(Function(t1) t1.BookFees.Any(Function(t2) t2.BookFeesAccessorialCode = code And t2.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Order)).ToList()
                    DependentFees = (From d In Books From f In d.BookFees Where f.BookFeesAccessorialCode = code And f.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Order Select f).ToList()
            End Select
            If DependentFees Is Nothing OrElse DependentFees.Count < 1 Then Return 'nothing to do
            For Each f In DependentFees
                If Not f Is Nothing AndAlso f.BookFeesControl <> 0 Then
                    CType(Me.LinqDB, NGLMasBookDataContext).BookFees.DeleteOnSubmit(f)
                End If
            Next
        End Using

    End Sub



    Friend Sub NormalizeFeeCalcTypes(ByRef BookRevs As List(Of BookRevenue))
        Using Logger.StartActivity("NormalizeFeeCalcTypes")

            If BookRevs Is Nothing OrElse BookRevs.Count < 1 Then Return
            For Each rev In BookRevs
                Logger.Information("Check for overriden Fees")
                Dim ActiveCodes As List(Of Integer) = (From f In rev.BookFees Where f.BookFeesOverRidden = False Select f.BookFeesAccessorialCode).Distinct().ToList()
                If Not ActiveCodes Is Nothing AndAlso ActiveCodes.Count > 0 Then
                    'loop through each code and check for duplicates 
                    For Each code In ActiveCodes
                        'Check for duplicate fees and if they exist we need to mark them as FeeCalcType of ALL
                        Logger.Information("Check for overriden Fees for code: {code} ", code)
                        Dim FeesDup As List(Of BookFee) = (From f In rev.BookFees
                                                           Where f.BookFeesOverRidden = False _
                                                                 And f.BookFeesAccessorialCode = code
                                                           Select f).ToList()
                        If Not FeesDup Is Nothing AndAlso FeesDup.Count > 1 Then
                            Logger.Information("Found duplicate fees for code: {code} ", code)
                            For Each fee In FeesDup.Where(Function(x) x.BookFeesAccessorialFeeCalcTypeControl <> Utilities.FeeCalcType.All)
                                fee.BookFeesAccessorialFeeCalcTypeControl = Utilities.FeeCalcType.All
                                fee.TrackingState = TrackingInfo.Updated
                                Logger.Information("Updated FeeCalcType for code: {code} where BookFeesAccessorialFeeCalcTypeControl <> All", code)
                            Next
                        End If
                    Next
                End If
            Next
        End Using

    End Sub

#End Region

End Class