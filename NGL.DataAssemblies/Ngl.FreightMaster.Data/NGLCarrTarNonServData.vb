Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.Linq.Dynamic
Imports System.ServiceModel
Imports Ngl.FreightMaster.Data.DataTransferObjects

Public Class NGLCarrTarNonServData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMASCarrierDataContext(ConnectionString)
        Me.LinqTable = db.CarrierTariffNonServices
        Me.LinqDB = db
        Me.SourceClass = "NGLCarrTarNonServData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASCarrierDataContext(ConnectionString)
            _LinqTable = db.CarrierTariffNonServices
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
        Return GetCarrTarNonServFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    Public Function GetCarrTarNonServFiltered(ByVal Control As Integer) As DataTransferObjects.CarrTarNonServ
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim oRecord As DataTransferObjects.CarrTarNonServ = (
                        From d In db.CarrierTariffNonServices
                        Where
                        d.CarrTarNonServControl = Control
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
    ''' Returns true or false based on the tariffs non-service point configuration
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
    ''' we now query the db directly instead of storing the Non-Service Point information in memory 
    ''' there are too many records and the system performance would be affected.
    ''' </remarks>
    Public Function IsNonServicePoint(ByVal CarrTarControl As Integer, ByVal Country As String, ByVal State As String, ByVal City As String, ByVal FromDate As Date?, ByVal ToDate As Date?, ByVal Zip As String) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Country = Country.ToUpper()
                State = State.ToUpper()
                City = City.ToUpper()
                Zip = Zip.ToUpper()
                'db.Log = New DebugTextWriter
                blnRet = (From d In db.CarrierTariffNonServices
                    Where
                        d.CarrTarNonServCarrTarControl = CarrTarControl _
                        And
                        ((d.CarrTarNonServCountry Is Nothing OrElse d.CarrTarNonServCountry.Trim() = String.Empty) OrElse d.CarrTarNonServCountry.ToUpper() = Country) _
                        And
                        ((d.CarrTarNonServState Is Nothing OrElse d.CarrTarNonServState.Trim() = String.Empty) OrElse d.CarrTarNonServState.ToUpper() = State) _
                        And
                        ((d.CarrTarNonServCity Is Nothing OrElse d.CarrTarNonServCity.Trim() = String.Empty) OrElse d.CarrTarNonServCity.ToUpper() = City) _
                        And
                        (Not FromDate.HasValue OrElse (Not d.CarrTarNonServEffDateFrom.HasValue OrElse d.CarrTarNonServEffDateFrom.Value.Date <= FromDate)) _
                        And
                        (Not ToDate.HasValue OrElse (Not d.CarrTarNonServEffDateTo.HasValue OrElse d.CarrTarNonServEffDateTo.Value.Date >= ToDate)) _
                        And
                        ((d.CarrTarNonServZip Is Nothing OrElse d.CarrTarNonServZip.Trim() = String.Empty) OrElse (Zip Is Nothing OrElse Zip.Trim() = String.Empty) OrElse Zip.Trim().Substring(0, d.CarrTarNonServZip.Trim().Length).ToUpper().CompareTo(d.CarrTarNonServZip.Trim().ToUpper()) = 0) Select d.CarrTarNonServControl).Any()


            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("IsNonServicePoint"), db)
            End Try

            Return blnRet

        End Using
    End Function


    ''' <summary>
    ''' GetCarrTarNonServsFiltered
    ''' </summary>
    ''' <param name="CarrTarControl"></param>
    ''' <param name="page"></param>
    ''' <param name="pagesize"></param>
    ''' <param name="filterWhere"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added by LVV on 8/18/16 for v-7.0.5.102 NonService Points Tariff System
    ''' </remarks>
    Public Function GetCarrTarNonServsFiltered(ByVal CarrTarControl As Integer,
                                               Optional ByVal page As Integer = 1,
                                               Optional ByVal pagesize As Integer = 5000,
                                               Optional ByVal filterWhere As String = "",
                                               Optional ByVal sSortKey As String = "") As DataTransferObjects.CarrTarNonServ()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1

                'db.Log = New DebugTextWriter

                Dim oQuery = From d In db.CarrierTariffNonServices
                        Where d.CarrTarNonServCarrTarControl = CarrTarControl
                        Select d

                'Added by LVV on 8/18/16 for v-7.0.5.102 NonService Points Tariff System
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
                Dim oRecords() As DataTransferObjects.CarrTarNonServ
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
                            Order By d.CarrTarNonServCountry, d.CarrTarNonServState, d.CarrTarNonServCity, d.CarrTarNonServZip
                            Select selectDTOData(d, page, intPageCount, intRecordCount, pagesize)
                        ).Skip(intSkip).Take(pagesize).ToArray()
                End If

                Return oRecords

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetCarrTarNonServsFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetCarrTarNonServNodes(ByVal CarrTarControl As Integer, ByVal ParentTreeID As Integer) As List(Of NGLTreeNode)
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'db.Log = New DebugTextWriter
                Dim oNodes As List(Of NGLTreeNode) = (
                        From d In db.CarrierTariffNonServices
                        Where (d.CarrTarNonServCarrTarControl = CarrTarControl)
                        Order By d.CarrTarNonServCountry, d.CarrTarNonServState, d.CarrTarNonServCity, d.CarrTarNonServZip
                        Select New DataTransferObjects.NGLTreeNode With {.Control = d.CarrTarNonServControl,
                        .ParentTreeID = ParentTreeID,
                        .Name = d.CarrTarNonServName,
                        .Description = " | " & d.CarrTarNonServCountry & " | " & d.CarrTarNonServState & " | " & d.CarrTarNonServCity & " | " & d.CarrTarNonServZip & " | ",
                        .ClassName = "CarrTarNonServ"}).ToList

                Return oNodes

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                Return Nothing
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetCarrTarNonServNodes"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetCarrTarNonServTree(ByVal CarrTarControl As Integer, ByVal ParentTreeID As Integer, ByRef intNextTreeID As Integer) As List(Of NGLTreeNode)
        Try
            Dim oTreeNodes As List(Of NGLTreeNode) = GetCarrTarNonServNodes(CarrTarControl, ParentTreeID)
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
            throwUnExpectedFaultException(ex, buildProcedureName("GetCarrTarNonServTree"))
        End Try
        Return Nothing
    End Function

    Public Function GetCarrTarNonServTreeFlat(ByVal CarrTarControl As Integer, ByVal ParentTreeID As Integer, ByRef intNextTreeID As Integer) As List(Of NGLTreeNode)
        Try

            Dim oTreeNodes As List(Of NGLTreeNode) = GetCarrTarNonServNodes(CarrTarControl, ParentTreeID)
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
            throwUnExpectedFaultException(ex, buildProcedureName("GetCarrTarNonServTreeFlat"))
        End Try
        Return Nothing
    End Function


#Region "LTS carrier tariff NonService data"

    ''' <summary>
    ''' Returns the NonService data assoicated with a Contract the CarrTarControl value must be provided in the filters.ParentControl parameter
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 08/31/2018
    '''   new LTS carrier tariff NonService query  
    ''' </remarks>
    Public Function GetCarrierTariffNonServices(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vCarrierTariffNonService()
        If filters Is Nothing Then Return Nothing
        If filters.ParentControl = 0 Then
            Dim sMsg As String = " The Tariff ID is missing. Please return to the tariff contract page and select a valid carrier contract."
            throwNoDataFaultException(sMsg)
        End If
        Dim oRet() As LTS.vCarrierTariffNonService

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                Dim iQuery As IQueryable(Of LTS.vCarrierTariffNonService)
                iQuery = db.vCarrierTariffNonServices
                Dim filterWhere As String = " (CarrTarNonServCarrTarControl = " & filters.ParentControl & ") "
                Dim sFilterSpacer As String = " And "

                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "CarrTarNonServName"
                    filters.sortDirection = "asc"
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrierTariffNonServices"), db)
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    '''  Insert or Update the carrier tariff NonServices data
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 08/31/2018
    ''' </remarks>
    Public Function SaveCarrierTariffNonService(ByVal oData As LTS.vCarrierTariffNonService) As Boolean
        Dim blnRet As Boolean = False
        If oData Is Nothing Then Return False 'nothing to do

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'verify the service contract
                If oData.CarrTarNonServCarrTarControl = 0 Then
                    Dim lDetails As New List(Of String) From {"Carrier Contract Reference", " was not provided and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                If Not db.CarrierTariffs.Any(Function(x) x.CarrTarControl = oData.CarrTarNonServCarrTarControl) Then

                    Dim lDetails As New List(Of String) From {"Carrier Contract Reference", " was not found and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                If oData.CarrTarNonServControl = 0 Then
                    Dim oNew As New LTS.CarrierTariffNonService()
                    Dim strMSG As String = ""
                    Dim skipObjs As New List(Of String) From {"CarrTarNonServModUser", "CarrTarNonServModDate", "CarrTarNonServUpdated"}
                    oNew = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(oNew, oData, skipObjs, strMSG)
                    With oNew
                        .CarrTarNonServModDate = Date.Now
                        .CarrTarNonServModUser = Me.Parameters.UserName
                    End With
                    If Debugger.IsAttached AndAlso Not String.IsNullOrWhiteSpace(strMSG) Then
                        System.Diagnostics.Debug.WriteLine(strMSG)
                    End If
                    db.CarrierTariffNonServices.InsertOnSubmit(oNew)
                Else
                    Dim oExisting = db.CarrierTariffNonServices.Where(Function(x) x.CarrTarNonServControl = oData.CarrTarNonServControl).FirstOrDefault()
                    If oExisting Is Nothing OrElse oExisting.CarrTarNonServControl = 0 Then
                        throwRecordDeletedFaultException("Cannot save changes for tariff NonService: " & oData.CarrTarNonServName)
                    End If
                    Dim strMSG As String = ""
                    Dim skipObjs As New List(Of String) From {"CarrTarNonServModUser", "CarrTarNonServModDate", "CarrTarNonServUpdated"}
                    oExisting = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(oExisting, oData, skipObjs, strMSG)
                    With oExisting
                        .CarrTarNonServModDate = Date.Now
                        .CarrTarNonServModUser = Me.Parameters.UserName
                    End With
                End If
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveCarrierTariffNonService"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Delete the provided carrier tariff NonService
    ''' </summary>
    ''' <param name="iControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 08/31/2018
    ''' </remarks>
    Public Function DeleteCarrierTariffNonService(ByVal iControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        If iControl = 0 Then Return False 'nothing to do
        'validate before delete

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Delete the Record
                Dim oToDelete = db.CarrierTariffNonServices.Where(Function(x) x.CarrTarNonServControl = iControl).FirstOrDefault()
                If oToDelete Is Nothing OrElse oToDelete.CarrTarNonServControl = 0 Then Return True 'already deleted
                db.CarrierTariffNonServices.DeleteOnSubmit(oToDelete)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteCarrierTariffNonService"), db)
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
    ''' Added by LVV on 8/18/16 for v-7.0.5.102 NonService Points Tariff System
    ''' </remarks>
    Public Overrides Function Update(Of TEntity As Class)(ByVal oData As Object,
                                                          ByVal oLinqTable As System.Data.Linq.Table(Of TEntity)) As Object
        Dim oNonServ As DataTransferObjects.CarrTarNonServ = CType(oData, DataTransferObjects.CarrTarNonServ)
        Using LinqDB
            'Note: the ValidateData Function must throw a FaultException error on failure
            ValidateUpdatedRecord(LinqDB, oNonServ)
            Dim d = (From e In CType(LinqDB, NGLMASCarrierDataContext).CarrierTariffNonServices Where e.CarrTarNonServControl = oNonServ.CarrTarNonServControl Select e).FirstOrDefault()
            If d Is Nothing OrElse d.CarrTarNonServControl = 0 Then Return Nothing
            'Create New Record 
            CopyDTOToLTS(oNonServ, d)
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
            Return GetCarrTarNonServFiltered(Control:=oNonServ.CarrTarNonServControl)
        End Using
    End Function

    ''' <summary>
    ''' Deletes a CarrTarNonServ record by control number without validation. It can only be called by system only process.
    ''' No validation is neccessary to delete CarrTarNonServ records because there is no FKey
    ''' </summary>
    ''' <param name="ControlNumber"></param>
    ''' <remarks>
    ''' Added by LVV on 8/18/16 for v-7.0.5.102 NonService Points Tariff System
    ''' </remarks>
    Public Overloads Sub Delete(ByVal ControlNumber As Integer)
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            If db.CarrierTariffNonServices.Any(Function(x) x.CarrTarNonServControl = ControlNumber) Then
                Dim nObject = db.CarrierTariffNonServices.Where(Function(x) x.CarrTarNonServControl = ControlNumber)
                db.CarrierTariffNonServices.DeleteAllOnSubmit(nObject)
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
        Dim d = CType(oData, DataTransferObjects.CarrTarNonServ)
        'Create New Record
        Return New LTS.CarrierTariffNonService With {.CarrTarNonServControl = d.CarrTarNonServControl,
            .CarrTarNonServCarrTarControl = d.CarrTarNonServCarrTarControl,
            .CarrTarNonServName = d.CarrTarNonServName,
            .CarrTarNonServEffDateFrom = d.CarrTarNonServEffDateFrom,
            .CarrTarNonServEffDateTo = d.CarrTarNonServEffDateTo,
            .CarrTarNonServCountry = d.CarrTarNonServCountry,
            .CarrTarNonServState = d.CarrTarNonServState,
            .CarrTarNonServCity = d.CarrTarNonServCity,
            .CarrTarNonServZip = d.CarrTarNonServZip,
            .CarrTarNonServLaneControl = d.CarrTarNonServLaneControl,
            .CarrTarNonServModUser = Parameters.UserName,
            .CarrTarNonServModDate = Date.Now,
            .CarrTarNonServUpdated = If(d.CarrTarNonServUpdated Is Nothing, New Byte() {}, d.CarrTarNonServUpdated)}
    End Function

    ''' <summary>
    ''' CopyDTOToLTS
    ''' </summary>
    ''' <param name="d"></param>
    ''' <param name="l"></param>
    ''' <remarks>
    ''' Added by LVV on 8/18/16 for v-7.0.5.102 NonService Points Tariff System
    ''' </remarks>
    Protected Sub CopyDTOToLTS(ByRef d As DataTransferObjects.CarrTarNonServ, ByRef l As LTS.CarrierTariffNonService)
        'Create New Record
        With l
            .CarrTarNonServName = d.CarrTarNonServName
            .CarrTarNonServEffDateFrom = d.CarrTarNonServEffDateFrom
            .CarrTarNonServEffDateTo = d.CarrTarNonServEffDateTo
            .CarrTarNonServCountry = d.CarrTarNonServCountry
            .CarrTarNonServState = d.CarrTarNonServState
            .CarrTarNonServCity = d.CarrTarNonServCity
            .CarrTarNonServZip = d.CarrTarNonServZip
            .CarrTarNonServLaneControl = d.CarrTarNonServLaneControl
            .CarrTarNonServModUser = Parameters.UserName
            .CarrTarNonServModDate = Date.Now
        End With

    End Sub


    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCarrTarNonServFiltered(Control:=CType(LinqTable, LTS.CarrierTariffNonService).CarrTarNonServControl)
    End Function

    'Protected Function validateSaveTariff(ByRef CarrTarControl As Integer, _
    '                              ByRef CarrTarID As String, _
    '                              ByVal CarrierControl As Integer, _
    '                              ByVal CompControl As Integer, _
    '                              ByVal TariffTempType As Integer, _
    '                              ByVal TariffType As String, _
    '                              ByVal AllowOverwrite As Boolean) As Boolean
    '    Dim blnRet As Boolean = True
    '    Dim intOldCarTarControl As Integer = CarrTarControl
    '    Dim strOldCarrTarID As String = CarrTarID
    '    Using db As New NGLMASCarrierDataContext(ConnectionString)
    '        Try
    '            Dim strNewTariffID = GetTariffCode(CarrierControl, CompControl, TariffTempType, TariffType)

    '            If CarrTarControl = 0 Then
    '                'this is a new tariff so we need to check if the tariff id already exists
    '                Dim varTariff = (From d In db.CarrierTariffs Where d.CarrTarID = strNewTariffID Select d).First
    '                If Not varTariff Is Nothing Then
    '                    If Not AllowOverwrite Then Return False
    '                    CarrTarControl = varTariff.CarrTarControl
    '                    CarrTarID = strNewTariffID
    '                    ''Delete all of the matrix details they no longer match.
    '                    'executeSQL("DELETE FROM [dbo].[CarrierTariffMatrix] Where CarrTarMatCarrTarControl = " & CarrTarControl)
    '                End If
    '            ElseIf CarrTarID <> strNewTariffID Then
    '                'if they change the TariffID we alwys return false and force the UI to ask for permission
    '                Return False
    '            Else
    '                Return True
    '            End If

    '        Catch ex As FaultException
    '            Throw
    '        Catch ex As System.Data.SqlClient.SqlException
    '            Utilities.SaveAppError(ex.Message, Me.Parameters)
    '            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
    '        Catch ex As InvalidOperationException
    '            Utilities.SaveAppError(ex.Message, Me.Parameters)
    '            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
    '        Catch ex As Exception
    '            Utilities.SaveAppError(ex.Message, Me.Parameters)
    '            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
    '        End Try

    '        Return blnRet

    '    End Using
    'End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim source As LTS.CarrierTariffNonService = TryCast(LinqTable, LTS.CarrierTariffNonService)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.CarrierTariffNonServices
                    Where d.CarrTarNonServControl = source.CarrTarNonServControl
                    Select New DataTransferObjects.QuickSaveResults With {.Control = d.CarrTarNonServControl _
                        , .ModDate = d.CarrTarNonServModDate _
                        , .ModUser = d.CarrTarNonServModUser _
                        , .Updated = d.CarrTarNonServUpdated.ToArray}).First

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

    Private Sub ValidateApproved(ByRef oDB As NGLMASCarrierDataContext, ByRef oData As DataTransferObjects.CarrTarNonServ)
        DirectCast(NDPBaseClassFactory("NGLCarrTarContractData", False), NGLCarrTarContractData).ValidateApproved(oData.CarrTarNonServCarrTarControl, oDB)
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

    Friend Function selectDTOData(ByVal d As LTS.CarrierTariffNonService, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.CarrTarNonServ
        Return New DataTransferObjects.CarrTarNonServ With {.CarrTarNonServControl = d.CarrTarNonServControl,
            .CarrTarNonServCarrTarControl = d.CarrTarNonServCarrTarControl,
            .CarrTarNonServName = d.CarrTarNonServName,
            .CarrTarNonServEffDateFrom = d.CarrTarNonServEffDateFrom,
            .CarrTarNonServEffDateTo = d.CarrTarNonServEffDateTo,
            .CarrTarNonServCountry = d.CarrTarNonServCountry,
            .CarrTarNonServState = d.CarrTarNonServState,
            .CarrTarNonServCity = d.CarrTarNonServCity,
            .CarrTarNonServZip = d.CarrTarNonServZip,
            .CarrTarNonServLaneControl = d.CarrTarNonServLaneControl,
            .CarrTarNonServModUser = d.CarrTarNonServModUser,
            .CarrTarNonServModDate = d.CarrTarNonServModDate,
            .CarrTarNonServUpdated = d.CarrTarNonServUpdated.ToArray(),
            .Page = page,
            .Pages = pagecount,
            .RecordCount = recordcount,
            .PageSize = pagesize}
    End Function

#End Region

End Class