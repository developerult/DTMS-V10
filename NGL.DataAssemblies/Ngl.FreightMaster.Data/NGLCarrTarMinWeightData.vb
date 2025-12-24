Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.Linq.Dynamic
Imports System.ServiceModel
Imports Ngl.FreightMaster.Data.DataTransferObjects

Public Class NGLCarrTarMinWeightData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMASCarrierDataContext(ConnectionString)
        Me.LinqTable = db.CarrierTariffMinWeights
        Me.LinqDB = db
        Me.SourceClass = "NGLCarrTarMinWeightData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASCarrierDataContext(ConnectionString)
                _LinqTable = db.CarrierTariffMinWeights
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
        Return GetCarrTarMinWeightFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    Public Function GetCarrTarMinWeightFiltered(ByVal Control As Integer) As DataTransferObjects.CarrTarMinWeight
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim oRecord As DataTransferObjects.CarrTarMinWeight = (
                        From d In db.CarrierTariffMinWeights
                        Where
                        d.CarrTarMinWeightControl = Control
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

    Public Function GetCarrTarMinWeightsFiltered(ByVal CarrTarControl As Integer,
                                                 Optional ByVal page As Integer = 1,
                                                 Optional ByVal pagesize As Integer = 1000,
                                                 Optional ByVal filterWhere As String = "",
                                                 Optional ByVal sSortKey As String = "") As DataTransferObjects.CarrTarMinWeight()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try


                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1

                'db.Log = New DebugTextWriter

                Dim oQuery = From d In db.CarrierTariffMinWeights
                        Where d.CarrTarMinWeightCarrTarControl = CarrTarControl
                        Select d

                'Added by LVV on 8/19/16 for v-7.0.5.102
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

                Dim oRecords() As DataTransferObjects.CarrTarMinWeight
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
                            Order By d.CarrTarMinWeightCountry, d.CarrTarMinWeightState, d.CarrTarMinWeightCity, d.CarrTarMinWeightZipFrom, d.CarrTarMinWeightZipTo
                            Select selectDTOData(d, page, intPageCount, intRecordCount, pagesize)
                        ).Skip(intSkip).Take(pagesize).ToArray()
                End If

                Return oRecords

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetCarrTarMinWeightsFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetCarrTarMinWeightNodes(ByVal CarrTarControl As Integer, ByVal ParentTreeID As Integer) As List(Of NGLTreeNode)
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'db.Log = New DebugTextWriter
                Dim oNodes As List(Of NGLTreeNode) = (
                        From d In db.CarrierTariffMinWeights
                        Where (d.CarrTarMinWeightCarrTarControl = CarrTarControl)
                        Order By d.CarrTarMinWeightCountry, d.CarrTarMinWeightState, d.CarrTarMinWeightCity, d.CarrTarMinWeightZipFrom, d.CarrTarMinWeightZipTo
                        Select New DataTransferObjects.NGLTreeNode With {.Control = d.CarrTarMinWeightControl,
                        .ParentTreeID = ParentTreeID,
                        .Name = d.CarrTarMinWeightName,
                        .Description = " | " & d.CarrTarMinWeightCountry & " | " & d.CarrTarMinWeightState & " | " & d.CarrTarMinWeightCity & " | " & d.CarrTarMinWeightZipFrom & " | " & d.CarrTarMinWeightZipTo & " | ",
                        .ClassName = "CarrTarMinWeight"}).ToList

                Return oNodes

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                Return Nothing
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetCarrTarMinWeightNodes"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetCarrTarMinWeightTree(ByVal CarrTarControl As Integer, ByVal ParentTreeID As Integer, ByRef intNextTreeID As Integer) As List(Of NGLTreeNode)
        Try
            Dim oTreeNodes As List(Of NGLTreeNode) = GetCarrTarMinWeightNodes(CarrTarControl, ParentTreeID)
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
            throwUnExpectedFaultException(ex, buildProcedureName("GetCarrTarMinWeightTree"))
        End Try
        Return Nothing
    End Function

    Public Function GetCarrTarMinWeightTreeFlat(ByVal CarrTarControl As Integer, ByVal ParentTreeID As Integer, ByRef intNextTreeID As Integer) As List(Of NGLTreeNode)
        Try

            Dim oTreeNodes As List(Of NGLTreeNode) = GetCarrTarMinWeightNodes(CarrTarControl, ParentTreeID)
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
            throwUnExpectedFaultException(ex, buildProcedureName("GetCarrTarMinWeightTreeFlat"))
        End Try
        Return Nothing
    End Function


#Region "LTS carrier tariff MinWeight data"

    ''' <summary>
    ''' Returns the MinWeight data assoicated with a Contract the CarrTarControl value must be provided in the filters.ParentControl parameter
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 08/31/2018
    '''   new LTS carrier tariff MinWeight query  
    ''' </remarks>
    Public Function GetCarrierTariffMinWeights(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vCarrierTariffMinWeight()
        If filters Is Nothing Then Return Nothing
        If filters.ParentControl = 0 Then
            Dim sMsg As String = " The Tariff ID is missing. Please return to the tariff contract page and select a valid carrier contract."
            throwNoDataFaultException(sMsg)
        End If
        Dim oRet() As LTS.vCarrierTariffMinWeight

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                Dim iQuery As IQueryable(Of LTS.vCarrierTariffMinWeight)
                iQuery = db.vCarrierTariffMinWeights
                Dim filterWhere As String = " (CarrTarMinWeightCarrTarControl = " & filters.ParentControl & ") "
                Dim sFilterSpacer As String = " And "

                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "CarrTarMinWeightName"
                    filters.sortDirection = "asc"
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrierTariffMinWeights"), db)
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    '''  Insert or Update the carrier tariff MinWeights data
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 08/31/2018
    ''' </remarks>
    Public Function SaveCarrierTariffMinWeight(ByVal oData As LTS.vCarrierTariffMinWeight) As Boolean
        Dim blnRet As Boolean = False
        If oData Is Nothing Then Return False 'nothing to do

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'verify the service contract
                If oData.CarrTarMinWeightCarrTarControl = 0 Then
                    Dim lDetails As New List(Of String) From {"Carrier Contract Reference", " was not provided and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                If Not db.CarrierTariffs.Any(Function(x) x.CarrTarControl = oData.CarrTarMinWeightCarrTarControl) Then

                    Dim lDetails As New List(Of String) From {"Carrier Contract Reference", " was not found and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                If oData.CarrTarMinWeightControl = 0 Then
                    Dim oNew As New LTS.CarrierTariffMinWeight()
                    Dim strMSG As String = ""
                    Dim skipObjs As New List(Of String) From {"CarrTarMinWeightModUser", "CarrTarMinWeightModDate", "CarrTarMinWeightUpdated"}
                    oNew = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(oNew, oData, skipObjs, strMSG)
                    With oNew
                        .CarrTarMinWeightModDate = Date.Now
                        .CarrTarMinWeightModUser = Me.Parameters.UserName
                    End With
                    If Debugger.IsAttached AndAlso Not String.IsNullOrWhiteSpace(strMSG) Then
                        System.Diagnostics.Debug.WriteLine(strMSG)
                    End If
                    db.CarrierTariffMinWeights.InsertOnSubmit(oNew)
                Else
                    Dim oExisting = db.CarrierTariffMinWeights.Where(Function(x) x.CarrTarMinWeightControl = oData.CarrTarMinWeightControl).FirstOrDefault()
                    If oExisting Is Nothing OrElse oExisting.CarrTarMinWeightControl = 0 Then
                        throwRecordDeletedFaultException("Cannot save changes for tariff MinWeight: " & oData.CarrTarMinWeightName)
                    End If
                    Dim strMSG As String = ""
                    Dim skipObjs As New List(Of String) From {"CarrTarMinWeightModUser", "CarrTarMinWeightModDate", "CarrTarMinWeightUpdated"}
                    oExisting = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(oExisting, oData, skipObjs, strMSG)
                    With oExisting
                        .CarrTarMinWeightModDate = Date.Now
                        .CarrTarMinWeightModUser = Me.Parameters.UserName
                    End With
                End If
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveCarrierTariffMinWeight"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Delete the provided carrier tariff MinWeight
    ''' </summary>
    ''' <param name="iControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 08/31/2018
    ''' </remarks>
    Public Function DeleteCarrierTariffMinWeight(ByVal iControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        If iControl = 0 Then Return False 'nothing to do
        'validate before delete

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Delete the Record
                Dim oToDelete = db.CarrierTariffMinWeights.Where(Function(x) x.CarrTarMinWeightControl = iControl).FirstOrDefault()
                If oToDelete Is Nothing OrElse oToDelete.CarrTarMinWeightControl = 0 Then Return True 'already deleted
                db.CarrierTariffMinWeights.DeleteOnSubmit(oToDelete)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteCarrierTariffMinWeight"), db)
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
    ''' Added by LVV on 8/19/16 for v-7.0.5.102
    ''' </remarks>
    Public Overrides Function Update(Of TEntity As Class)(ByVal oData As Object,
                                                          ByVal oLinqTable As System.Data.Linq.Table(Of TEntity)) As Object
        Dim oMinWeight As DataTransferObjects.CarrTarMinWeight = CType(oData, DataTransferObjects.CarrTarMinWeight)
        Using LinqDB
            'Note: the ValidateData Function must throw a FaultException error on failure
            ValidateUpdatedRecord(LinqDB, oMinWeight)
            Dim d = (From e In CType(LinqDB, NGLMASCarrierDataContext).CarrierTariffMinWeights Where e.CarrTarMinWeightControl = oMinWeight.CarrTarMinWeightControl Select e).FirstOrDefault()
            If d Is Nothing OrElse d.CarrTarMinWeightControl = 0 Then Return Nothing
            'Create New Record 
            CopyDTOToLTS(oMinWeight, d)
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
            Return GetCarrTarMinWeightFiltered(Control:=oMinWeight.CarrTarMinWeightControl)
        End Using
    End Function

    ''' <summary>
    ''' Deletes a CarrTarMinWeight record by control number without validation. It can only be called by system only process.
    ''' No validation is neccessary to delete CarrTarMinWeight records because there is no FKey
    ''' </summary>
    ''' <param name="ControlNumber"></param>
    ''' <remarks>
    ''' Added by LVV on 8/19/16 for v-7.0.5.102
    ''' </remarks>
    Public Overloads Sub Delete(ByVal ControlNumber As Integer)
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            If db.CarrierTariffMinWeights.Any(Function(x) x.CarrTarMinWeightControl = ControlNumber) Then
                Dim nObject = db.CarrierTariffMinWeights.Where(Function(x) x.CarrTarMinWeightControl = ControlNumber)
                db.CarrierTariffMinWeights.DeleteAllOnSubmit(nObject)
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
        Dim d = CType(oData, DataTransferObjects.CarrTarMinWeight)
        'Create New Record
        Return New LTS.CarrierTariffMinWeight With {.CarrTarMinWeightControl = d.CarrTarMinWeightControl,
            .CarrTarMinWeightCarrTarControl = d.CarrTarMinWeightCarrTarControl,
            .CarrTarMinWeightCity = d.CarrTarMinWeightCity,
            .CarrTarMinWeightClassFrom = d.CarrTarMinWeightClassFrom,
            .CarrTarMinWeightClassTo = d.CarrTarMinWeightClassTo,
            .CarrTarMinWeightClassTypeControl = d.CarrTarMinWeightClassTypeControl,
            .CarrTarMinWeightCountry = d.CarrTarMinWeightCountry,
            .CarrTarMinWeightEffDateFrom = d.CarrTarMinWeightEffDateFrom,
            .CarrTarMinWeightEffDateTo = d.CarrTarMinWeightEffDateTo,
            .CarrTarMinWeightLaneControl = d.CarrTarMinWeightLaneControl,
            .CarrTarMinWeightName = d.CarrTarMinWeightName,
            .CarrTarMinWeightPerLoad = d.CarrTarMinWeightPerLoad,
            .CarrTarMinWeightPerPallet = d.CarrTarMinWeightPerPallet,
            .CarrTarMinWeightPointTypeControl = d.CarrTarMinWeightPointTypeControl,
            .CarrTarMinWeightState = d.CarrTarMinWeightState,
            .CarrTarMinWeightZipFrom = d.CarrTarMinWeightZipFrom,
            .CarrTarMinWeightZipTo = d.CarrTarMinWeightZipTo,
            .CarrTarMinWeightModUser = Parameters.UserName,
            .CarrTarMinWeightModDate = Date.Now,
            .CarrTarMinWeightUpdated = If(d.CarrTarMinWeightUpdated Is Nothing, New Byte() {}, d.CarrTarMinWeightUpdated)}
    End Function

    ''' <summary>
    ''' CopyDTOToLTS
    ''' </summary>
    ''' <param name="d"></param>
    ''' <param name="l"></param>
    ''' <remarks>
    ''' Added by LVV on 8/19/16 for v-7.0.5.102
    ''' </remarks>
    Protected Sub CopyDTOToLTS(ByRef d As DataTransferObjects.CarrTarMinWeight, ByRef l As LTS.CarrierTariffMinWeight)
        'Create New Record
        With l
            .CarrTarMinWeightName = d.CarrTarMinWeightName
            .CarrTarMinWeightCity = d.CarrTarMinWeightCity
            .CarrTarMinWeightState = d.CarrTarMinWeightState
            .CarrTarMinWeightCountry = d.CarrTarMinWeightCountry
            .CarrTarMinWeightZipTo = d.CarrTarMinWeightZipTo
            .CarrTarMinWeightZipFrom = d.CarrTarMinWeightZipFrom
            .CarrTarMinWeightEffDateFrom = d.CarrTarMinWeightEffDateFrom
            .CarrTarMinWeightEffDateTo = d.CarrTarMinWeightEffDateTo
            .CarrTarMinWeightPointTypeControl = d.CarrTarMinWeightPointTypeControl
            .CarrTarMinWeightPerLoad = d.CarrTarMinWeightPerLoad
            .CarrTarMinWeightPerPallet = d.CarrTarMinWeightPerPallet
            .CarrTarMinWeightClassTypeControl = d.CarrTarMinWeightClassTypeControl
            .CarrTarMinWeightLaneControl = d.CarrTarMinWeightLaneControl
            .CarrTarMinWeightClassFrom = d.CarrTarMinWeightClassFrom
            .CarrTarMinWeightClassTo = d.CarrTarMinWeightClassTo
            .CarrTarMinWeightModUser = Parameters.UserName
            .CarrTarMinWeightModDate = Date.Now
        End With

    End Sub

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCarrTarMinWeightFiltered(Control:=CType(LinqTable, LTS.CarrierTariffMinWeight).CarrTarMinWeightControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim source As LTS.CarrierTariffMinWeight = TryCast(LinqTable, LTS.CarrierTariffMinWeight)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.CarrierTariffMinWeights
                    Where d.CarrTarMinWeightControl = source.CarrTarMinWeightControl
                    Select New DataTransferObjects.QuickSaveResults With {.Control = d.CarrTarMinWeightControl _
                        , .ModDate = d.CarrTarMinWeightModDate _
                        , .ModUser = d.CarrTarMinWeightModUser _
                        , .Updated = d.CarrTarMinWeightUpdated.ToArray}).First

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

    Private Sub ValidateApproved(ByRef oDB As NGLMASCarrierDataContext, ByRef oData As DataTransferObjects.CarrTarMinWeight)
        DirectCast(NDPBaseClassFactory("NGLCarrTarContractData", False), NGLCarrTarContractData).ValidateApproved(oData.CarrTarMinWeightCarrTarControl, oDB)
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

    Friend Function selectDTOData(ByVal d As LTS.CarrierTariffMinWeight, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.CarrTarMinWeight
        Return New DataTransferObjects.CarrTarMinWeight With {.CarrTarMinWeightControl = d.CarrTarMinWeightControl,
            .CarrTarMinWeightCarrTarControl = d.CarrTarMinWeightCarrTarControl,
            .CarrTarMinWeightCity = d.CarrTarMinWeightCity,
            .CarrTarMinWeightClassFrom = d.CarrTarMinWeightClassFrom,
            .CarrTarMinWeightClassTo = d.CarrTarMinWeightClassTo,
            .CarrTarMinWeightClassTypeControl = d.CarrTarMinWeightClassTypeControl,
            .CarrTarMinWeightCountry = d.CarrTarMinWeightCountry,
            .CarrTarMinWeightEffDateFrom = d.CarrTarMinWeightEffDateFrom,
            .CarrTarMinWeightEffDateTo = d.CarrTarMinWeightEffDateTo,
            .CarrTarMinWeightLaneControl = d.CarrTarMinWeightLaneControl,
            .CarrTarMinWeightName = d.CarrTarMinWeightName,
            .CarrTarMinWeightPerLoad = d.CarrTarMinWeightPerLoad,
            .CarrTarMinWeightPerPallet = d.CarrTarMinWeightPerPallet,
            .CarrTarMinWeightPointTypeControl = d.CarrTarMinWeightPointTypeControl,
            .CarrTarMinWeightState = d.CarrTarMinWeightState,
            .CarrTarMinWeightZipFrom = d.CarrTarMinWeightZipFrom,
            .CarrTarMinWeightZipTo = d.CarrTarMinWeightZipTo,
            .CarrTarMinWeightModUser = d.CarrTarMinWeightModUser,
            .CarrTarMinWeightModDate = d.CarrTarMinWeightModDate,
            .CarrTarMinWeightUpdated = d.CarrTarMinWeightUpdated.ToArray(),
            .Page = page,
            .Pages = pagecount,
            .RecordCount = recordcount,
            .PageSize = pagesize}
    End Function

#End Region

End Class