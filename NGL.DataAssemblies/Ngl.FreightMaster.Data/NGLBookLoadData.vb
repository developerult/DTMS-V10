Imports System.ServiceModel
Imports NGL.Core.ChangeTracker
Imports Ngl.FreightMaster.Data.DataTransferObjects

Public Class NGLBookLoadData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        'Dim db As New NGLMasBookDataContext(ConnectionString)
        'Me.LinqTable = db.BookLoads
        'Me.LinqDB = db
        Me.SourceClass = "NGLBookLoadData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get

            'If _LinqTable Is Nothing Then
            Dim db As New NGLMasBookDataContext(ConnectionString)
            _LinqTable = db.BookLoads
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
            Dim source As LTS.BookLoad = TryCast(nObject, LTS.BookLoad)
            If source Is Nothing Then Return Nothing
            Me.BookDependencyResult = DirectCast(Me.NDPBaseClassFactory("NGLBookData", False), NGLBookData).UpdateBookDependencies(0, source.BookLoadControl)
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
        Dim intBookControl As Integer = 0
        Dim dtoObject As DataTransferObjects.BookLoad = TryCast(oData, DataTransferObjects.BookLoad)
        If Not dtoObject Is Nothing Then intBookControl = dtoObject.BookLoadBookControl
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
            If intBookControl <> 0 Then
                Me.BookDependencyResult = DirectCast(Me.NDPBaseClassFactory("NGLBookData", False), NGLBookData).UpdateBookDependencies(intBookControl, 0)
                Me.LastProcedureName = "spUpdateBookDependencies"
            Else
                Me.BookDependencyResult = Nothing
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
            Dim source As LTS.BookLoad = TryCast(nObject, LTS.BookLoad)
            If source Is Nothing Then Return Nothing
            Me.BookDependencyResult = DirectCast(Me.NDPBaseClassFactory("NGLBookData", False), NGLBookData).UpdateBookDependencies(0, source.BookLoadControl)
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
            Dim source As LTS.BookLoad = TryCast(nObject, LTS.BookLoad)
            If source Is Nothing Then Return Nothing
            Me.BookDependencyResult = DirectCast(Me.NDPBaseClassFactory("NGLBookData", False), NGLBookData).UpdateBookDependencies(0, source.BookLoadControl)
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
    Public Overrides Sub UpdateNoReturn(Of TEntity As Class)(ByVal oData As Object,
                                                             ByVal oLinqTable As System.Data.Linq.Table(Of TEntity))
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
            Dim source As LTS.BookLoad = TryCast(nObject, LTS.BookLoad)
            If source Is Nothing Then Return
            Me.BookDependencyResult = DirectCast(Me.NDPBaseClassFactory("NGLBookData", False), NGLBookData).UpdateBookDependencies(0, source.BookLoadControl)
            Me.LastProcedureName = "spUpdateBookDependencies"
            Return
        End Using
    End Sub

#End Region


    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return GetBookLoadFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetBookLoadsFiltered()
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Control"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR v-5.1.3 7/1/11 removed BookItems data.  We now leave this list empty
    ''' because the maximum allowed durring a save is a little over 50 records (WCF Error 400 issue)
    ''' </remarks>
    Public Function GetBookLoadFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.BookLoad
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Dim oDLO As New DataLoadOptions
                'oDLO.LoadWith(Of LTS.BookLoad)(Function(t As LTS.BookLoad) t.BookItems)
                'db.LoadOptions = oDLO


                Dim BookLoad As DataTransferObjects.BookLoad = (
                        From d In db.BookLoads
                        Where
                        (d.BookLoadControl = If(Control = 0, d.BookLoadControl, Control))
                        Order By d.BookLoadControl Descending
                        Select selectDTOData(d, db)).First

                Return BookLoad

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
    ''' <param name="BookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR v-5.1.3 7/1/11 removed BookItems data.  We now leave this list empty
    ''' because the maximum allowed durring a save is a little over 50 records (WCF Error 400 issue)
    ''' </remarks>
    Public Function GetBookLoadsFiltered(Optional ByVal BookControl As Integer = 0) As DataTransferObjects.BookLoad()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Dim oDLO As New DataLoadOptions
                'oDLO.LoadWith(Of LTS.BookLoad)(Function(t As LTS.BookLoad) t.BookItems)
                'db.LoadOptions = oDLO

                Dim BookLoads() As DataTransferObjects.BookLoad = (
                        From d In db.BookLoads
                        Where
                        (d.BookLoadBookControl = If(BookControl = 0, d.BookLoadBookControl, BookControl))
                        Order By d.BookLoadControl
                        Select selectDTOData(d, db)).ToArray()

                Return BookLoads

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

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DataTransferObjects.BookLoad)
        Return selectLTSData(d, Me.Parameters.UserName)

    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Dim oData As LTS.BookLoad = TryCast(LinqTable, LTS.BookLoad)
        If oData Is Nothing Then
            Return Nothing
        End If
        Return GetBookLoadFiltered(Control:=oData.BookLoadControl)
    End Function

    Public Function QuickSaveResults(ByVal BookLoadControl As Integer) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                ret = (From d In db.BookLoads
                    Where d.BookLoadControl = BookLoadControl
                    Select New DataTransferObjects.QuickSaveResults With {.Control = d.BookLoadControl _
                        , .ModDate = d.BookLoadModDate _
                        , .ModUser = d.BookLoadModUser _
                        , .Updated = d.BookLoadUpdated.ToArray}).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("QuickSaveResults"))
            End Try
        End Using
        Return ret
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim source As LTS.BookLoad = TryCast(LinqTable, LTS.BookLoad)
        If source Is Nothing Then Return Nothing
        Return QuickSaveResults(source.BookLoadControl)
    End Function

    ' ''' <summary>
    ' ''' 
    ' ''' </summary>
    ' ''' <param name="LinqTable"></param>
    ' ''' <param name="oData"></param>
    ' ''' <remarks>
    ' ''' Modified by RHR v-5.1.3 7/1/11 removed BookItems data.  We now leave this list empty
    ' ''' because the maximum allowed durring a save is a little over 50 records (WCF Error 400 issue)
    ' ''' </remarks>
    'Protected Overrides Sub AddDetailsToLinq(ByRef LinqTable As Object, ByRef oData As DTO.DTOBaseClass)

    '    With CType(LinqTable, LTS.BookLoad)
    '        'Add Book Loads Records
    '        .BookItems.AddRange( _
    '                 From d In CType(oData, DTO.BookLoad).BookItems _
    '                 Select New LTS.BookItem With {.BookItemControl = d.BookItemControl _
    '                                   , .BookItemBookLoadControl = d.BookItemBookLoadControl _
    '                                   , .BookItemFixOffInvAllow = d.BookItemFixOffInvAllow _
    '                                   , .BookItemFixFrtAllow = d.BookItemFixFrtAllow _
    '                                   , .BookItemItemNumber = d.BookItemItemNumber _
    '                                   , .BookItemQtyOrdered = d.BookItemQtyOrdered _
    '                                   , .BookItemFreightCost = d.BookItemFreightCost _
    '                                   , .BookItemItemCost = d.BookItemItemCost _
    '                                   , .BookItemWeight = d.BookItemWeight _
    '                                   , .BookItemCube = d.BookItemCube _
    '                                   , .BookItemPack = d.BookItemPack _
    '                                   , .BookItemSize = d.BookItemSize _
    '                                   , .BookItemDescription = d.BookItemDescription _
    '                                   , .BookItemHazmat = d.BookItemHazmat _
    '                                   , .BookItemModDate = Date.Now _
    '                                   , .BookItemModUser = Parameters.UserName _
    '                                   , .BookItemBrand = d.BookItemBrand _
    '                                   , .BookItemCostCenter = d.BookItemCostCenter _
    '                                   , .BookItemLotNumber = d.BookItemLotNumber _
    '                                   , .BookItemLotExpirationDate = d.BookItemLotExpirationDate _
    '                                   , .BookItemGTIN = d.BookItemGTIN _
    '                                   , .BookCustItemNumber = d.BookCustItemNumber _
    '                                   , .BookItemBFC = d.BookItemBFC _
    '                                   , .BookItemCountryOfOrigin = d.BookItemCountryOfOrigin _
    '                                   , .BookItemHST = d.BookItemHST _
    '                                   , .BookItemPalletTypeID = d.BookItemPalletTypeID _
    '                                   , .BookItemUpdated = If(d.BookItemUpdated Is Nothing, New Byte() {}, d.BookItemUpdated)})

    '    End With
    'End Sub

    ' ''' <summary>
    ' ''' 
    ' ''' </summary>
    ' ''' <param name="oDB"></param>
    ' ''' <param name="LinqTable"></param>
    ' ''' <remarks>
    ' ''' Modified by RHR v-5.1.3 7/1/11 removed BookItems data.  We now leave this list empty
    ' ''' because the maximum allowed durring a save is a little over 50 records (WCF Error 400 issue)
    ' ''' </remarks>
    'Protected Overrides Sub InsertAllDetails(ByRef oDB As System.Data.Linq.DataContext, ByRef LinqTable As Object)
    '    With CType(oDB, NGLMasBookDataContext)
    '        .BookItems.InsertAllOnSubmit(CType(LinqTable, LTS.BookLoad).BookItems)
    '    End With
    'End Sub

    ' ''' <summary>
    ' ''' 
    ' ''' </summary>
    ' ''' <param name="oDB"></param>
    ' ''' <param name="oData"></param>
    ' ''' <remarks>
    ' ''' Modified by RHR v-5.1.3 7/1/11 removed BookItems data.  We now leave this list empty
    ' ''' because the maximum allowed durring a save is a little over 50 records (WCF Error 400 issue)
    ' ''' </remarks>
    'Protected Overrides Sub ProcessUpdatedDetails(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
    '    With CType(oDB, NGLMasBookDataContext)
    '        If ((Not .BookItems Is Nothing) AndAlso .BookItems.Count > 0) Then
    '            ' Process any inserted item records 
    '            .BookItems.InsertAllOnSubmit(GetBookItemChanges(oData, TrackingInfo.Created))
    '            ' Process any updated contact records
    '            .BookItems.AttachAll(GetBookItemChanges(oData, TrackingInfo.Updated), True)
    '            ' Process any deleted contact records
    '            Dim deletedDetails = GetBookItemChanges(oData, TrackingInfo.Deleted)
    '            .BookItems.AttachAll(deletedDetails, True)
    '            .BookItems.DeleteAllOnSubmit(deletedDetails)
    '        End If
    '    End With
    'End Sub

    ' ''' <summary>
    ' ''' 
    ' ''' </summary>
    ' ''' <param name="source"></param>
    ' ''' <param name="changeType"></param>
    ' ''' <returns></returns>
    ' ''' <remarks>
    ' ''' Modified by RHR v-5.1.3 7/1/11 removed BookItems data.  We now leave this list empty
    ' ''' because the maximum allowed durring a save is a little over 50 records (WCF Error 400 issue)
    ' ''' </remarks>
    'Protected Function GetBookItemChanges(ByVal source As DTO.BookLoad, ByVal changeType As TrackingInfo) As List(Of LTS.BookItem)
    '    ' Test record details for specified change type.
    '    ' If Updated is null, set to byte[0] (for inserted items).
    '    Dim details As IEnumerable(Of LTS.BookItem) = ( _
    '      From d In source.BookItems _
    '      Where d.TrackingState = changeType _
    '      Select New LTS.BookItem With {.BookItemControl = d.BookItemControl _
    '                                   , .BookItemBookLoadControl = d.BookItemBookLoadControl _
    '                                   , .BookItemFixOffInvAllow = d.BookItemFixOffInvAllow _
    '                                   , .BookItemFixFrtAllow = d.BookItemFixFrtAllow _
    '                                   , .BookItemItemNumber = d.BookItemItemNumber _
    '                                   , .BookItemQtyOrdered = d.BookItemQtyOrdered _
    '                                   , .BookItemFreightCost = d.BookItemFreightCost _
    '                                   , .BookItemItemCost = d.BookItemItemCost _
    '                                   , .BookItemWeight = d.BookItemWeight _
    '                                   , .BookItemCube = d.BookItemCube _
    '                                   , .BookItemPack = d.BookItemPack _
    '                                   , .BookItemSize = d.BookItemSize _
    '                                   , .BookItemDescription = d.BookItemDescription _
    '                                   , .BookItemHazmat = d.BookItemHazmat _
    '                                   , .BookItemModDate = Date.Now _
    '                                   , .BookItemModUser = Parameters.UserName _
    '                                   , .BookItemBrand = d.BookItemBrand _
    '                                   , .BookItemCostCenter = d.BookItemCostCenter _
    '                                   , .BookItemLotNumber = d.BookItemLotNumber _
    '                                   , .BookItemLotExpirationDate = d.BookItemLotExpirationDate _
    '                                   , .BookItemGTIN = d.BookItemGTIN _
    '                                   , .BookCustItemNumber = d.BookCustItemNumber _
    '                                   , .BookItemBFC = d.BookItemBFC _
    '                                   , .BookItemCountryOfOrigin = d.BookItemCountryOfOrigin _
    '                                   , .BookItemHST = d.BookItemHST _
    '                                   , .BookItemPalletTypeID = d.BookItemPalletTypeID _
    '                                   , .BookItemUpdated = If(d.BookItemUpdated Is Nothing, New Byte() {}, d.BookItemUpdated)})
    '    Return details.ToList()
    'End Function

    Friend Shared Function selectDTOData(ByVal d As LTS.BookLoad, ByRef db As NGLMasBookDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.BookLoad
        Dim oDTO As New DataTransferObjects.BookLoad
        Dim skipObjs As New List(Of String) From {"BookLoadUpdated",
                "Page",
                "Pages",
                "RecordCount",
                "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .BookLoadUpdated = d.BookLoadUpdated.ToArray()
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oDTO


    End Function

    Friend Shared Function selectDTODataWDetails(ByVal d As LTS.BookLoad, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.BookLoad
        'Dim oBookItem As NGLBookItemData = Me.NDPBaseClassFactory("NGLBookItemData", False)
        Dim oDTO As New DataTransferObjects.BookLoad
        Dim skipObjs As New List(Of String) From {"BookLoadUpdated",
                "Page",
                "Pages",
                "RecordCount",
                "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .BookLoadUpdated = d.BookLoadUpdated.ToArray()
            .BookItems = (From i In d.BookItems Select NGLBookItemData.selectDTOData(i, Nothing)).ToList()
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oDTO


    End Function

    Friend Shared Function selectLTSData(ByVal d As DataTransferObjects.BookLoad, ByVal UserName As String) As LTS.BookLoad
        ' If Updated is null, set to byte[0] (for inserted items).
        Dim oLTS As New LTS.BookLoad
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
    Friend Shared Sub UpdateLTSWithDTO(ByRef d As DataTransferObjects.BookLoad, ByRef t As LTS.BookLoad, ByVal UserName As String, Optional ByVal isLTSAttached As Boolean = False)
        Dim blnNewLTS As Boolean = False 'used to determine if we allow the BookLoadData to be set, existing LTS objects cannot update the BookLoadUpdated value.
        If t.BookLoadControl = 0 Then blnNewLTS = True 'in this case we use a new Byte or the current value in d
        Dim skipObjs As New List(Of String) From {"BookLoadCaseQty",
                "BookLoadWgt",
                "BookLoadCube",
                "BookLoadPL",
                "BookLoadPX",
                "BookLoadBFC",
                "BookLoadTotCost",
                "BookLoadStopSeq",
                "BookLoadModDate",
                "BookLoadModUser",
                "BookLoadUpdated"}
        Dim strMSG As String = ""
        t = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(t, d, skipObjs, strMSG)
        't = CopyMatchingFields(t, d, skipObjs)
        With t
            .BookLoadCaseQty = d.BookLoadCaseQty
            .BookLoadWgt = d.BookLoadWgt
            .BookLoadCube = d.BookLoadCube
            .BookLoadPL = d.BookLoadPL
            .BookLoadPX = d.BookLoadPX
            .BookLoadBFC = d.BookLoadBFC
            .BookLoadTotCost = d.BookLoadTotCost
            .BookLoadStopSeq = d.BookLoadStopSeq
            .BookLoadModDate = Date.Now
            .BookLoadModUser = UserName
            If (Not isLTSAttached) AndAlso blnNewLTS Then .BookLoadUpdated = If(d.BookLoadUpdated Is Nothing, New Byte() {}, d.BookLoadUpdated)
        End With
    End Sub

    Friend Shared Function GetBookLoadChanges(ByVal source As DataTransferObjects.DTOBaseClass, ByVal changeType As TrackingInfo, ByVal UserName As String) As List(Of LTS.BookLoad)
        If source Is Nothing Then Return New List(Of LTS.BookLoad)
        Dim lBookLoads As List(Of BookLoad)
        If TypeOf source Is DataTransferObjects.Book Then
            lBookLoads = CType(source, DataTransferObjects.Book).BookLoads
        ElseIf TypeOf source Is DataTransferObjects.BookRevenue Then
            lBookLoads = CType(source, DataTransferObjects.BookRevenue).BookLoads
        Else
            Return New List(Of LTS.BookLoad)
        End If
        If lBookLoads Is Nothing OrElse lBookLoads.Count < 1 Then Return New List(Of LTS.BookLoad)
        ' Test record details for specified change type.
        Dim details As IEnumerable(Of LTS.BookLoad) = (
                From d In lBookLoads
                Where d.TrackingState = changeType
                Select selectLTSData(d, UserName))
        Return details.ToList()
    End Function



#End Region

End Class