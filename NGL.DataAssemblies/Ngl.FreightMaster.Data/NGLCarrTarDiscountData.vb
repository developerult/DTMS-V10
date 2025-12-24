Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.Linq.Dynamic
Imports System.ServiceModel
Imports Ngl.FreightMaster.Data.DataTransferObjects

Public Class NGLCarrTarDiscountData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMASCarrierDataContext(ConnectionString)
        Me.LinqTable = db.CarrierTariffDiscounts
        Me.LinqDB = db
        Me.SourceClass = "NGLCarrTarDiscountData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASCarrierDataContext(ConnectionString)
                _LinqTable = db.CarrierTariffDiscounts
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
        Return GetCarrTarDiscountFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    Public Function GetCarrTarDiscountFiltered(ByVal Control As Integer) As DataTransferObjects.CarrTarDiscount
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim oRecord As DataTransferObjects.CarrTarDiscount = (
                        From d In db.CarrierTariffDiscounts
                        Where
                        d.CarrTarDiscountControl = Control
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
    ''' GetCarrTarDiscountsFiltered
    ''' </summary>
    ''' <param name="CarrTarControl"></param>
    ''' <param name="page"></param>
    ''' <param name="pagesize"></param>
    ''' <param name="filterWhere"></param>
    ''' <param name="sSortKey"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by LVV on 8/19/16 for v-7.0.5.102
    ''' </remarks>
    Public Function GetCarrTarDiscountsFiltered(ByVal CarrTarControl As Integer,
                                                Optional ByVal page As Integer = 1,
                                                Optional ByVal pagesize As Integer = 1000,
                                                Optional ByVal filterWhere As String = "",
                                                Optional ByVal sSortKey As String = "") As DataTransferObjects.CarrTarDiscount()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try


                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1

                'db.Log = New DebugTextWriter

                Dim oQuery = From d In db.CarrierTariffDiscounts
                        Where d.CarrTarDiscountCarrTarControl = CarrTarControl
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

                Dim oRecords() As DataTransferObjects.CarrTarDiscount
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
                            Order By d.CarrTarDiscountCountry, d.CarrTarDiscountState, d.CarrTarDiscountCity, d.CarrTarDiscountZipFrom, d.CarrTarDiscountZipTo
                            Select selectDTOData(d, page, intPageCount, intRecordCount, pagesize)
                        ).Skip(intSkip).Take(pagesize).ToArray()
                End If

                Return oRecords

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetCarrTarDiscountsFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetCarrTarDiscountNodes(ByVal CarrTarControl As Integer, ByVal ParentTreeID As Integer) As List(Of NGLTreeNode)
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'db.Log = New DebugTextWriter
                Dim oNodes As List(Of NGLTreeNode) = (
                        From d In db.CarrierTariffDiscounts
                        Where (d.CarrTarDiscountCarrTarControl = CarrTarControl)
                        Order By d.CarrTarDiscountCountry, d.CarrTarDiscountState, d.CarrTarDiscountCity, d.CarrTarDiscountZipFrom, d.CarrTarDiscountZipTo
                        Select New DataTransferObjects.NGLTreeNode With {.Control = d.CarrTarDiscountControl,
                        .ParentTreeID = ParentTreeID,
                        .Name = d.CarrTarDiscountName,
                        .Description = " | " & d.CarrTarDiscountCountry & " | " & d.CarrTarDiscountState & " | " & d.CarrTarDiscountCity & " | " & d.CarrTarDiscountZipFrom & " | " & d.CarrTarDiscountZipTo & " | ",
                        .ClassName = "CarrTarDiscount"}).ToList

                Return oNodes

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                Return Nothing
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetCarrTarDiscountNodes"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetCarrTarDiscountTree(ByVal CarrTarControl As Integer, ByVal ParentTreeID As Integer, ByRef intNextTreeID As Integer) As List(Of NGLTreeNode)
        Try
            Dim oTreeNodes As List(Of NGLTreeNode) = GetCarrTarDiscountNodes(CarrTarControl, ParentTreeID)
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
            throwUnExpectedFaultException(ex, buildProcedureName("GetCarrTarDiscountTree"))
        End Try
        Return Nothing
    End Function

    Public Function GetCarrTarDiscountTreeFlat(ByVal CarrTarControl As Integer, ByVal ParentTreeID As Integer, ByRef intNextTreeID As Integer) As List(Of NGLTreeNode)
        Try

            Dim oTreeNodes As List(Of NGLTreeNode) = GetCarrTarDiscountNodes(CarrTarControl, ParentTreeID)
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
            throwUnExpectedFaultException(ex, buildProcedureName("GetCarrTarDiscountTreeFlat"))
        End Try
        Return Nothing
    End Function


#Region "LTS carrier tariff discount data"

    ''' <summary>
    ''' Returns the discount data assoicated with a Contract the CarrTarControl value must be provided in the filters.ParentControl parameter
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 08/31/2018
    '''   new LTS carrier tariff discount query  
    ''' </remarks>
    Public Function GetCarrierTariffDiscounts(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vCarrierTariffDiscount()
        If filters Is Nothing Then Return Nothing
        If filters.ParentControl = 0 Then
            Dim sMsg As String = " The Tariff ID is missing. Please return to the tariff contract page and select a valid carrier contract."
            throwNoDataFaultException(sMsg)
        End If
        Dim oRet() As LTS.vCarrierTariffDiscount

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                Dim iQuery As IQueryable(Of LTS.vCarrierTariffDiscount)
                iQuery = db.vCarrierTariffDiscounts
                Dim filterWhere As String = " (CarrTarDiscountCarrTarControl = " & filters.ParentControl & ") "
                Dim sFilterSpacer As String = " And "

                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "CarrTarDiscountName"
                    filters.sortDirection = "asc"
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrierTariffDiscounts"), db)
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    '''  Insert or Update the carrier tariff discounts data
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 08/31/2018
    ''' </remarks>
    Public Function SaveCarrierTariffDiscount(ByVal oData As LTS.vCarrierTariffDiscount) As Boolean
        Dim blnRet As Boolean = False
        If oData Is Nothing Then Return False 'nothing to do

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'verify the service contract
                If oData.CarrTarDiscountCarrTarControl = 0 Then
                    Dim lDetails As New List(Of String) From {"Carrier Contract Reference", " was not provided and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                If Not db.CarrierTariffs.Any(Function(x) x.CarrTarControl = oData.CarrTarDiscountCarrTarControl) Then

                    Dim lDetails As New List(Of String) From {"Carrier Contract Reference", " was not found and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                If oData.CarrTarDiscountControl = 0 Then
                    Dim oNew As New LTS.CarrierTariffDiscount()
                    Dim strMSG As String = ""
                    Dim skipObjs As New List(Of String) From {"CarrTarDiscountModUser", "CarrTarDiscountModDate", "CarrTarDiscountUpdated"}
                    oNew = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(oNew, oData, skipObjs, strMSG)
                    With oNew
                        .CarrTarDiscountModDate = Date.Now
                        .CarrTarDiscountModUser = Me.Parameters.UserName
                    End With
                    If Debugger.IsAttached AndAlso Not String.IsNullOrWhiteSpace(strMSG) Then
                        System.Diagnostics.Debug.WriteLine(strMSG)
                    End If
                    db.CarrierTariffDiscounts.InsertOnSubmit(oNew)
                Else
                    Dim oExisting = db.CarrierTariffDiscounts.Where(Function(x) x.CarrTarDiscountControl = oData.CarrTarDiscountControl).FirstOrDefault()
                    If oExisting Is Nothing OrElse oExisting.CarrTarDiscountControl = 0 Then
                        throwRecordDeletedFaultException("Cannot save changes for tariff discount: " & oData.CarrTarDiscountName)
                    End If
                    Dim strMSG As String = ""
                    Dim skipObjs As New List(Of String) From {"CarrTarDiscountModUser", "CarrTarDiscountModDate", "CarrTarDiscountUpdated"}
                    oExisting = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(oExisting, oData, skipObjs, strMSG)
                    With oExisting
                        .CarrTarDiscountModDate = Date.Now
                        .CarrTarDiscountModUser = Me.Parameters.UserName
                    End With
                End If
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveCarrierTariffDiscount"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Delete the provided carrier tariff discount
    ''' </summary>
    ''' <param name="iControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 08/31/2018
    ''' </remarks>
    Public Function DeleteCarrierTariffDiscount(ByVal iControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        If iControl = 0 Then Return False 'nothing to do
        'validate before delete

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Delete the Record
                Dim oToDelete = db.CarrierTariffDiscounts.Where(Function(x) x.CarrTarDiscountControl = iControl).FirstOrDefault()
                If oToDelete Is Nothing OrElse oToDelete.CarrTarDiscountControl = 0 Then Return True 'already deleted
                db.CarrierTariffDiscounts.DeleteOnSubmit(oToDelete)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteCarrierTariffDiscount"), db)
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
        Dim oDiscount As DataTransferObjects.CarrTarDiscount = CType(oData, DataTransferObjects.CarrTarDiscount)
        Using LinqDB
            'Note: the ValidateData Function must throw a FaultException error on failure
            ValidateUpdatedRecord(LinqDB, oDiscount)
            Dim d = (From e In CType(LinqDB, NGLMASCarrierDataContext).CarrierTariffDiscounts Where e.CarrTarDiscountControl = oDiscount.CarrTarDiscountControl Select e).FirstOrDefault()
            If d Is Nothing OrElse d.CarrTarDiscountControl = 0 Then Return Nothing
            'Create New Record 
            CopyDTOToLTS(oDiscount, d)
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
            Return GetCarrTarDiscountFiltered(Control:=oDiscount.CarrTarDiscountControl)
        End Using
    End Function

    ''' <summary>
    ''' Deletes a CarrTarDiscount record by control number without validation. It can only be called by system only process.
    ''' No validation is neccessary to delete CarrTarDiscount records because there is no FKey
    ''' </summary>
    ''' <param name="ControlNumber"></param>
    ''' <remarks>
    ''' Added by LVV on 8/19/16 for v-7.0.5.102
    ''' </remarks>
    Public Overloads Sub Delete(ByVal ControlNumber As Integer)
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            If db.CarrierTariffDiscounts.Any(Function(x) x.CarrTarDiscountControl = ControlNumber) Then
                Dim nObject = db.CarrierTariffDiscounts.Where(Function(x) x.CarrTarDiscountControl = ControlNumber)
                db.CarrierTariffDiscounts.DeleteAllOnSubmit(nObject)
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
        Dim d = CType(oData, DataTransferObjects.CarrTarDiscount)
        'Create New Record
        Return New LTS.CarrierTariffDiscount With {.CarrTarDiscountControl = d.CarrTarDiscountControl,
            .CarrTarDiscountCarrTarControl = d.CarrTarDiscountCarrTarControl,
            .CarrTarDiscountPointTypeControl = d.CarrTarDiscountPointTypeControl,
            .CarrTarDiscountName = d.CarrTarDiscountName,
            .CarrTarDiscountEffDateFrom = d.CarrTarDiscountEffDateFrom,
            .CarrTarDiscountEffDateTo = d.CarrTarDiscountEffDateTo,
            .CarrTarDiscountCountry = d.CarrTarDiscountCountry,
            .CarrTarDiscountState = d.CarrTarDiscountState,
            .CarrTarDiscountCity = d.CarrTarDiscountCity,
            .CarrTarDiscountZipFrom = d.CarrTarDiscountZipFrom,
            .CarrTarDiscountZipTo = d.CarrTarDiscountZipTo,
            .CarrTarDiscountMinValue = d.CarrTarDiscountMinValue,
            .CarrTarDiscountRateValue = d.CarrTarDiscountRateValue,
            .CarrTarDiscountWgtLimit = d.CarrTarDiscountWgtLimit,
            .CarrTarDiscountModUser = Parameters.UserName,
            .CarrTarDiscountModDate = Date.Now,
            .CarrTarDiscountUpdated = If(d.CarrTarDiscountUpdated Is Nothing, New Byte() {}, d.CarrTarDiscountUpdated)}
    End Function

    ''' <summary>
    ''' CopyDTOToLTS
    ''' </summary>
    ''' <param name="d"></param>
    ''' <param name="l"></param>
    ''' <remarks>
    ''' Added by LVV on 8/19/16 for v-7.0.5.102
    ''' </remarks>
    Protected Sub CopyDTOToLTS(ByRef d As DataTransferObjects.CarrTarDiscount, ByRef l As LTS.CarrierTariffDiscount)
        'Create New Record
        With l
            .CarrTarDiscountName = d.CarrTarDiscountName
            .CarrTarDiscountCity = d.CarrTarDiscountCity
            .CarrTarDiscountState = d.CarrTarDiscountState
            .CarrTarDiscountCountry = d.CarrTarDiscountCountry
            .CarrTarDiscountZipTo = d.CarrTarDiscountZipTo
            .CarrTarDiscountZipFrom = d.CarrTarDiscountZipFrom
            .CarrTarDiscountEffDateFrom = d.CarrTarDiscountEffDateFrom
            .CarrTarDiscountEffDateTo = d.CarrTarDiscountEffDateTo
            .CarrTarDiscountPointTypeControl = d.CarrTarDiscountPointTypeControl
            .CarrTarDiscountMinValue = d.CarrTarDiscountMinValue
            .CarrTarDiscountRateValue = d.CarrTarDiscountRateValue
            .CarrTarDiscountWgtLimit = d.CarrTarDiscountWgtLimit
            .CarrTarDiscountModUser = Parameters.UserName
            .CarrTarDiscountModDate = Date.Now
        End With

    End Sub

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCarrTarDiscountFiltered(Control:=CType(LinqTable, LTS.CarrierTariffDiscount).CarrTarDiscountControl)
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
                Dim source As LTS.CarrierTariffDiscount = TryCast(LinqTable, LTS.CarrierTariffDiscount)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.CarrierTariffDiscounts
                    Where d.CarrTarDiscountControl = source.CarrTarDiscountControl
                    Select New DataTransferObjects.QuickSaveResults With {.Control = d.CarrTarDiscountControl _
                        , .ModDate = d.CarrTarDiscountModDate _
                        , .ModUser = d.CarrTarDiscountModUser _
                        , .Updated = d.CarrTarDiscountUpdated.ToArray}).First

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

    Private Sub ValidateApproved(ByRef oDB As NGLMASCarrierDataContext, ByRef oData As DataTransferObjects.CarrTarDiscount)
        DirectCast(NDPBaseClassFactory("NGLCarrTarContractData", False), NGLCarrTarContractData).ValidateApproved(oData.CarrTarDiscountCarrTarControl, oDB)
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

    Friend Function selectDTOData(ByVal d As LTS.CarrierTariffDiscount, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.CarrTarDiscount
        Return New DataTransferObjects.CarrTarDiscount With {.CarrTarDiscountControl = d.CarrTarDiscountControl,
            .CarrTarDiscountCarrTarControl = d.CarrTarDiscountCarrTarControl,
            .CarrTarDiscountPointTypeControl = d.CarrTarDiscountPointTypeControl,
            .CarrTarDiscountName = d.CarrTarDiscountName,
            .CarrTarDiscountEffDateFrom = d.CarrTarDiscountEffDateFrom,
            .CarrTarDiscountEffDateTo = d.CarrTarDiscountEffDateTo,
            .CarrTarDiscountCountry = d.CarrTarDiscountCountry,
            .CarrTarDiscountState = d.CarrTarDiscountState,
            .CarrTarDiscountCity = d.CarrTarDiscountCity,
            .CarrTarDiscountZipFrom = d.CarrTarDiscountZipFrom,
            .CarrTarDiscountZipTo = d.CarrTarDiscountZipTo,
            .CarrTarDiscountMinValue = d.CarrTarDiscountMinValue,
            .CarrTarDiscountRateValue = d.CarrTarDiscountRateValue,
            .CarrTarDiscountWgtLimit = d.CarrTarDiscountWgtLimit,
            .CarrTarDiscountModUser = d.CarrTarDiscountModUser,
            .CarrTarDiscountModDate = d.CarrTarDiscountModDate,
            .CarrTarDiscountUpdated = d.CarrTarDiscountUpdated.ToArray(),
            .Page = page,
            .Pages = pagecount,
            .RecordCount = recordcount,
            .PageSize = pagesize}
    End Function

#End Region

End Class