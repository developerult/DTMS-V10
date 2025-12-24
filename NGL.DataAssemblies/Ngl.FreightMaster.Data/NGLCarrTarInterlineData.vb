Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.Linq.Dynamic
Imports System.ServiceModel
Imports Ngl.FreightMaster.Data.DataTransferObjects

Public Class NGLCarrTarInterlineData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMASCarrierDataContext(ConnectionString)
        Me.LinqTable = db.CarrierTariffInterlines
        Me.LinqDB = db
        Me.SourceClass = "NGLCarrTarInterlineData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASCarrierDataContext(ConnectionString)
                _LinqTable = db.CarrierTariffInterlines
                Me.LinqDB = db
            End If

            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return GetCarrTarInterlineFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    Public Function GetCarrTarInterlineFiltered(ByVal Control As Integer) As DataTransferObjects.CarrTarInterline
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim oRecord As DataTransferObjects.CarrTarInterline = (
                        From d In db.CarrierTariffInterlines
                        Where
                        d.CarrTarInterlineControl = Control
                        Select selectDTOData(d)).First

                Return oRecord

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException()
            Catch ex As Exception
                throwUnExpectedFaultException(ex.Message)
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Returns true or false based on the tariffs interline configuration
    ''' </summary>
    ''' <param name="CarrTarControl"></param>
    ''' <param name="Country"></param>
    ''' <param name="State"></param>
    ''' <param name="City"></param>
    ''' <param name="FromDate"></param>
    ''' <param name="ToDate"></param>
    ''' <param name="Zip"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR v-7.0.5.100 6/2/2016  
    ''' we now query the db directly instead of storing the interline information in memory 
    ''' there are too many records and the system performance would be affected.
    ''' </remarks>
    Public Function IsInterline(ByVal CarrTarControl As Integer, ByVal Country As String, ByVal State As String, ByVal City As String, ByVal FromDate As Date?, ByVal ToDate As Date?, ByVal Zip As String) As Boolean
        Dim blnIsInterline As Boolean = False
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Country = Country.ToUpper()
                State = State.ToUpper()
                City = City.ToUpper()
                Zip = Zip.ToUpper()
                'db.Log = New DebugTextWriter
                blnIsInterline = (From d In db.CarrierTariffInterlines
                    Where
                        d.CarrTarInterlineCarrTarControl = CarrTarControl _
                        And
                        ((d.CarrTarInterlineCountry Is Nothing OrElse d.CarrTarInterlineCountry.Trim() = String.Empty) OrElse d.CarrTarInterlineCountry.ToUpper() = Country) _
                        And
                        ((d.CarrTarInterlineState Is Nothing OrElse d.CarrTarInterlineState.Trim() = String.Empty) OrElse d.CarrTarInterlineState.ToUpper() = State) _
                        And
                        ((d.CarrTarInterlineCity Is Nothing OrElse d.CarrTarInterlineCity.Trim() = String.Empty) OrElse d.CarrTarInterlineCity.ToUpper() = City) _
                        And
                        (Not FromDate.HasValue OrElse (Not d.CarrTarInterlineEffDateFrom.HasValue OrElse d.CarrTarInterlineEffDateFrom.Value.Date <= FromDate)) _
                        And
                        (Not ToDate.HasValue OrElse (Not d.CarrTarInterlineEffDateTo.HasValue OrElse d.CarrTarInterlineEffDateTo.Value.Date >= ToDate)) _
                        And
                        ((d.CarrTarInterlineZip Is Nothing OrElse d.CarrTarInterlineZip.Trim() = String.Empty) OrElse (Zip Is Nothing OrElse Zip.Trim() = String.Empty) OrElse Zip.Trim().Substring(0, d.CarrTarInterlineZip.Trim().Length).ToUpper().CompareTo(d.CarrTarInterlineZip.Trim().ToUpper()) = 0) Select d.CarrTarInterlineControl).Any()


            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("IsInterline"), db)
            End Try

            Return blnIsInterline

        End Using
    End Function

    ''' <summary>
    ''' GetCarrTarInterlinesFiltered
    ''' </summary>
    ''' <param name="CarrTarControl"></param>
    ''' <param name="page"></param>
    ''' <param name="pagesize"></param>
    ''' <param name="filterWhere"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by LVV on 8/15/16 for v-7.0.5.102 Interline Points Tariff System 
    ''' </remarks>
    Public Function GetCarrTarInterlinesFiltered(ByVal CarrTarControl As Integer,
                                                 Optional ByVal page As Integer = 1,
                                                 Optional ByVal pagesize As Integer = 1000,
                                                 Optional ByVal filterWhere As String = "",
                                                 Optional ByVal sSortKey As String = "") As DataTransferObjects.CarrTarInterline()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1

                'db.Log = New DebugTextWriter

                Dim oQuery = From d In db.CarrierTariffInterlines
                        Where d.CarrTarInterlineCarrTarControl = CarrTarControl
                        Select d

                'Modified by LVV on 8/15/16 for v-7.0.5.102 Interline Points Tariff System 
                If oQuery Is Nothing Then Return Nothing
                If Not String.IsNullOrEmpty(filterWhere) Then
                    oQuery = DLinqUtil.filterWhere(oQuery, filterWhere)
                End If

                intRecordCount = oQuery.Count
                If intRecordCount < 1 Then Return Nothing
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize

                Dim oRecords() As DataTransferObjects.CarrTarInterline
                If Not String.IsNullOrWhiteSpace(sSortKey) Then
                    'example sort key "CarrTarNonServCountry ASC,CarrTarNonServState DESC,CarrTarNonServCity ASC"
                    Dim nQuery = oQuery.OrderBy(sSortKey)
                    oRecords = (
                        From d In nQuery
                            Select selectDTOData(d, page, intPageCount, intRecordCount, pagesize)
                        ).Skip(intSkip).Take(pagesize).ToArray()
                Else
                    'sort using default values
                    oRecords = (
                        From d In oQuery
                            Order By d.CarrTarInterlineCountry, d.CarrTarInterlineState, d.CarrTarInterlineCity, d.CarrTarInterlineZip
                            Select selectDTOData(d, page, intPageCount, intRecordCount, pagesize)
                        ).Skip(intSkip).Take(pagesize).ToArray()
                End If

                Return oRecords

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetCarrTarInterlinesFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetCarrTarInterlineNodes(ByVal CarrTarControl As Integer, ByVal ParentTreeID As Integer) As List(Of NGLTreeNode)
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'db.Log = New DebugTextWriter
                Dim oNodes As List(Of NGLTreeNode) = (
                        From d In db.CarrierTariffInterlines
                        Where (d.CarrTarInterlineCarrTarControl = CarrTarControl)
                        Order By d.CarrTarInterlineCountry, d.CarrTarInterlineState, d.CarrTarInterlineCity, d.CarrTarInterlineZip
                        Select New DataTransferObjects.NGLTreeNode With {.Control = d.CarrTarInterlineControl,
                        .ParentTreeID = ParentTreeID,
                        .Name = d.CarrTarInterlineName,
                        .Description = " | " & d.CarrTarInterlineCountry & " | " & d.CarrTarInterlineState & " | " & d.CarrTarInterlineCity & " | " & d.CarrTarInterlineZip & " | ",
                        .ClassName = "CarrTarInterline"}).ToList

                Return oNodes

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                Return Nothing
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetCarrTarInterlineNodes"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetCarrTarInterlineTree(ByVal CarrTarControl As Integer, ByVal ParentTreeID As Integer, ByRef intNextTreeID As Integer) As List(Of NGLTreeNode)
        Try
            Dim oTreeNodes As List(Of NGLTreeNode) = GetCarrTarInterlineNodes(CarrTarControl, ParentTreeID)
            If Not oTreeNodes Is Nothing AndAlso oTreeNodes.Count > 0 Then
                Dim intNextChildTreeID As Integer = intNextTreeID + oTreeNodes.Count
                For Each node In oTreeNodes
                    Dim intNodeTreeID = incrementID(intNextTreeID)
                    node.TreeID = intNodeTreeID
                Next
                intNextTreeID = intNextChildTreeID
            End If
            Return oTreeNodes
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("GetCarrTarInterlineTree"))
        End Try
        Return Nothing
    End Function

    Public Function GetCarrTarInterlineTreeFlat(ByVal CarrTarControl As Integer, ByVal ParentTreeID As Integer, ByRef intNextTreeID As Integer) As List(Of NGLTreeNode)
        Try

            Dim oTreeNodes As List(Of NGLTreeNode) = GetCarrTarInterlineNodes(CarrTarControl, ParentTreeID)
            If Not oTreeNodes Is Nothing AndAlso oTreeNodes.Count > 0 Then
                Dim intNextChildTreeID As Integer = intNextTreeID + oTreeNodes.Count
                For Each node In oTreeNodes
                    Dim intNodeTreeID = incrementID(intNextTreeID)
                    node.TreeID = intNodeTreeID
                Next
                intNextTreeID = intNextChildTreeID
            End If
            Return oTreeNodes
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("GetCarrTarInterlineTreeFlat"))
        End Try
        Return Nothing
    End Function


#Region "LTS carrier tariff Interline data"

    ''' <summary>
    ''' Returns the Interline data assoicated with a Contract the CarrTarControl value must be provided in the filters.ParentControl parameter
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 08/31/2018
    '''   new carrier tariff Interline query  using LTS and Models 
    ''' </remarks>
    Public Function GetCarrierTariffInterlines(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As Models.mCarrierTariffInterline()
        If filters Is Nothing Then Return Nothing
        If filters.ParentControl = 0 Then
            Dim sMsg As String = " The Tariff ID is missing. Please return to the tariff contract page and select a valid carrier contract."
            throwNoDataFaultException(sMsg)
        End If
        Dim oRet As New List(Of Models.mCarrierTariffInterline)

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                Dim iQuery As IQueryable(Of LTS.CarrierTariffInterline)
                iQuery = db.CarrierTariffInterlines
                Dim filterWhere As String = " (CarrTarInterlineCarrTarControl = " & filters.ParentControl & ") "
                Dim sFilterSpacer As String = " And "

                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "CarrTarInterlineName"
                    filters.sortDirection = "asc"
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                db.Log = New DebugTextWriter
                Dim oData = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                If Not oData Is Nothing AndAlso oData.Count() > 0 Then
                    Dim strMSG As String = ""
                    Dim skipObjs As New List(Of String) From {"CarrTarInterlineUpdated"}
                    For Each d In oData
                        Dim oRecord = New Models.mCarrierTariffInterline()
                        oRet.Add(Ngl.Core.Utility.DataTransformation.CopyMatchingFields(oRecord, d, skipObjs, strMSG))
                    Next
                    If Debugger.IsAttached AndAlso Not String.IsNullOrWhiteSpace(strMSG) Then
                        System.Diagnostics.Debug.WriteLine(strMSG)
                    End If
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrierTariffInterlines"), db)
            End Try
        End Using
        Return oRet.ToArray()
    End Function

    ''' <summary>
    '''  Insert or Update the carrier tariff Interlines data
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 08/31/2018
    ''' </remarks>
    Public Function SaveCarrierTariffInterline(ByVal oData As Models.mCarrierTariffInterline) As Boolean
        Dim blnRet As Boolean = False
        If oData Is Nothing Then Return False 'nothing to do

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'verify the tariff contract
                If oData.CarrTarInterlineCarrTarControl = 0 Then
                    Dim lDetails As New List(Of String) From {"Carrier Contract Reference", " was not provided and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                If Not db.CarrierTariffs.Any(Function(x) x.CarrTarControl = oData.CarrTarInterlineCarrTarControl) Then

                    Dim lDetails As New List(Of String) From {"Carrier Contract Reference", " was not found and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                If oData.CarrTarInterlineControl = 0 Then
                    Dim oNew As New LTS.CarrierTariffInterline()
                    Dim strMSG As String = ""
                    Dim skipObjs As New List(Of String) From {"CarrTarInterlineModUser", "CarrTarInterlineModDate", "CarrTarInterlineUpdated"}
                    oNew = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(oNew, oData, skipObjs, strMSG)
                    With oNew
                        .CarrTarInterlineModDate = Date.Now
                        .CarrTarInterlineModUser = Me.Parameters.UserName
                    End With
                    If Debugger.IsAttached AndAlso Not String.IsNullOrWhiteSpace(strMSG) Then
                        System.Diagnostics.Debug.WriteLine(strMSG)
                    End If
                    db.CarrierTariffInterlines.InsertOnSubmit(oNew)
                Else
                    Dim oExisting = db.CarrierTariffInterlines.Where(Function(x) x.CarrTarInterlineControl = oData.CarrTarInterlineControl).FirstOrDefault()
                    If oExisting Is Nothing OrElse oExisting.CarrTarInterlineControl = 0 Then
                        throwRecordDeletedFaultException("Cannot save changes for tariff Interline: " & oData.CarrTarInterlineName)
                    End If
                    Dim strMSG As String = ""
                    Dim skipObjs As New List(Of String) From {"CarrTarInterlineModUser", "CarrTarInterlineModDate", "CarrTarInterlineUpdated"}
                    oExisting = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(oExisting, oData, skipObjs, strMSG)
                    With oExisting
                        .CarrTarInterlineModDate = Date.Now
                        .CarrTarInterlineModUser = Me.Parameters.UserName
                    End With
                End If
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveCarrierTariffInterline"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Delete the provided carrier tariff Interline
    ''' </summary>
    ''' <param name="iControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 08/31/2018
    ''' </remarks>
    Public Function DeleteCarrierTariffInterline(ByVal iControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        If iControl = 0 Then Return False 'nothing to do
        'validate before delete

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Delete the Record
                Dim oToDelete = db.CarrierTariffInterlines.Where(Function(x) x.CarrTarInterlineControl = iControl).FirstOrDefault()
                If oToDelete Is Nothing OrElse oToDelete.CarrTarInterlineControl = 0 Then Return True 'already deleted
                db.CarrierTariffInterlines.DeleteOnSubmit(oToDelete)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteCarrierTariffInterline"), db)
            End Try
        End Using
        Return blnRet
    End Function

#End Region

    ''' <summary>
    ''' Update
    ''' </summary>
    ''' <typeparam name="TEntity"></typeparam>
    ''' <param name="oData"></param>
    ''' <param name="oLinqTable"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by LVV on 8/15/16 for v-7.0.5.102 Interline Points Tariff System
    ''' </remarks>
    Public Overrides Function Update(Of TEntity As Class)(ByVal oData As Object,
                                                          ByVal oLinqTable As System.Data.Linq.Table(Of TEntity)) As Object
        Dim oInterline As DataTransferObjects.CarrTarInterline = CType(oData, DataTransferObjects.CarrTarInterline)
        Using LinqDB
            'Note: the ValidateData Function must throw a FaultException error on failure
            ValidateUpdatedRecord(LinqDB, oInterline)
            Dim d = (From e In CType(LinqDB, NGLMASCarrierDataContext).CarrierTariffInterlines Where e.CarrTarInterlineControl = oInterline.CarrTarInterlineControl Select e).FirstOrDefault()
            If d Is Nothing OrElse d.CarrTarInterlineControl = 0 Then Return Nothing
            'Create New Record 
            CopyDTOToLTS(oInterline, d)
            Try
                LinqDB.SubmitChanges()
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
            ' Return the updated order
            Return GetCarrTarInterlineFiltered(Control:=oInterline.CarrTarInterlineControl)
        End Using
    End Function

    ''' <summary>
    ''' Deletes a CarrTarInterline record by control number without validation. It can only be called by system only process.
    ''' No validation is neccessary to delete CarrTarInterline records because there is no FKey
    ''' </summary>
    ''' <param name="ControlNumber"></param>
    ''' <remarks>
    ''' Modified by LVV on 8/15/16 for v-7.0.5.102 Interline Points Tariff System
    ''' </remarks>
    Public Overloads Sub Delete(ByVal ControlNumber As Integer)
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            If db.CarrierTariffInterlines.Any(Function(x) x.CarrTarInterlineControl = ControlNumber) Then
                Dim nObject = db.CarrierTariffInterlines.Where(Function(x) x.CarrTarInterlineControl = ControlNumber)
                db.CarrierTariffInterlines.DeleteAllOnSubmit(nObject)
                Try
                    db.SubmitChanges()
                Catch ex As Exception
                    ManageLinqDataExceptions(ex, buildProcedureName("Delete"), db)
                End Try
            End If
        End Using
    End Sub

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DataTransferObjects.CarrTarInterline)
        'Create New Record
        Return New LTS.CarrierTariffInterline With {.CarrTarInterlineControl = d.CarrTarInterlineControl,
            .CarrTarInterlineCarrTarControl = d.CarrTarInterlineCarrTarControl,
            .CarrTarInterlineName = d.CarrTarInterlineName,
            .CarrTarInterlineEffDateFrom = d.CarrTarInterlineEffDateFrom,
            .CarrTarInterlineEffDateTo = d.CarrTarInterlineEffDateTo,
            .CarrTarInterlineCountry = d.CarrTarInterlineCountry,
            .CarrTarInterlineState = d.CarrTarInterlineState,
            .CarrTarInterlineCity = d.CarrTarInterlineCity,
            .CarrTarInterlineZip = d.CarrTarInterlineZip,
            .CarrTarInterlineModUser = Parameters.UserName,
            .CarrTarInterlineModDate = Date.Now,
            .CarrTarInterlineUpdated = If(d.CarrTarInterlineUpdated Is Nothing, New Byte() {}, d.CarrTarInterlineUpdated)}
    End Function

    ''' <summary>
    ''' CopyDTOToLTS
    ''' </summary>
    ''' <param name="d"></param>
    ''' <param name="l"></param>
    ''' <remarks>
    ''' Modified by LVV on 8/15/16 for v-7.0.5.102 Interline Points Tariff System
    ''' </remarks>
    Protected Sub CopyDTOToLTS(ByRef d As DataTransferObjects.CarrTarInterline, ByRef l As LTS.CarrierTariffInterline)
        'Create New Record
        With l
            .CarrTarInterlineName = d.CarrTarInterlineName
            .CarrTarInterlineEffDateFrom = d.CarrTarInterlineEffDateFrom
            .CarrTarInterlineEffDateTo = d.CarrTarInterlineEffDateTo
            .CarrTarInterlineCountry = d.CarrTarInterlineCountry
            .CarrTarInterlineState = d.CarrTarInterlineState
            .CarrTarInterlineCity = d.CarrTarInterlineCity
            .CarrTarInterlineZip = d.CarrTarInterlineZip
            .CarrTarInterlineModUser = Parameters.UserName
            .CarrTarInterlineModDate = Date.Now
        End With

    End Sub


    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCarrTarInterlineFiltered(Control:=CType(LinqTable, LTS.CarrierTariffInterline).CarrTarInterlineControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim source As LTS.CarrierTariffInterline = TryCast(LinqTable, LTS.CarrierTariffInterline)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.CarrierTariffInterlines
                    Where d.CarrTarInterlineControl = source.CarrTarInterlineControl
                    Select New DataTransferObjects.QuickSaveResults With {.Control = d.CarrTarInterlineControl _
                        , .ModDate = d.CarrTarInterlineModDate _
                        , .ModUser = d.CarrTarInterlineModUser _
                        , .Updated = d.CarrTarInterlineUpdated.ToArray}).First

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.ToString)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetQuickSaveResults"))
            End Try

        End Using
        Return ret
    End Function

    Private Sub ValidateApproved(ByRef oDB As NGLMASCarrierDataContext, ByRef oData As DataTransferObjects.CarrTarInterline)
        DirectCast(NDPBaseClassFactory("NGLCarrTarContractData", False), NGLCarrTarContractData).ValidateApproved(oData.CarrTarInterlineCarrTarControl, oDB)
    End Sub

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        ValidateApproved(oDB, oData)
    End Sub

    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        ValidateApproved(oDB, oData)
    End Sub

    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        ValidateApproved(oDB, oData)
    End Sub

    Friend Function selectDTOData(ByVal d As LTS.CarrierTariffInterline, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.CarrTarInterline
        Return New DataTransferObjects.CarrTarInterline With {.CarrTarInterlineControl = d.CarrTarInterlineControl,
            .CarrTarInterlineCarrTarControl = d.CarrTarInterlineCarrTarControl,
            .CarrTarInterlineName = d.CarrTarInterlineName,
            .CarrTarInterlineEffDateFrom = d.CarrTarInterlineEffDateFrom,
            .CarrTarInterlineEffDateTo = d.CarrTarInterlineEffDateTo,
            .CarrTarInterlineCountry = d.CarrTarInterlineCountry,
            .CarrTarInterlineState = d.CarrTarInterlineState,
            .CarrTarInterlineCity = d.CarrTarInterlineCity,
            .CarrTarInterlineZip = d.CarrTarInterlineZip,
            .CarrTarInterlineModUser = d.CarrTarInterlineModUser,
            .CarrTarInterlineModDate = d.CarrTarInterlineModDate,
            .CarrTarInterlineUpdated = d.CarrTarInterlineUpdated.ToArray(),
            .Page = page,
            .Pages = pagecount,
            .RecordCount = recordcount,
            .PageSize = pagesize}
    End Function

#End Region

End Class