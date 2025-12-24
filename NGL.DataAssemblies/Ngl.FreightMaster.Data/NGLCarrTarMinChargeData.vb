Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.Linq.Dynamic
Imports System.ServiceModel
Imports Ngl.FreightMaster.Data.DataTransferObjects

Public Class NGLCarrTarMinChargeData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMASCarrierDataContext(ConnectionString)
        Me.LinqTable = db.CarrierTariffMinCharges
        Me.LinqDB = db
        Me.SourceClass = "NGLCarrTarMinChargeData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASCarrierDataContext(ConnectionString)
                _LinqTable = db.CarrierTariffMinCharges
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
        Return GetCarrTarMinChargeFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    Public Function GetCarrTarMinChargeFiltered(ByVal Control As Integer) As DataTransferObjects.CarrTarMinCharge
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim oRecord As DataTransferObjects.CarrTarMinCharge = (
                        From d In db.CarrierTariffMinCharges
                        Where
                        d.CarrTarMinChargeControl = Control
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
    ''' GetCarrTarMinChargesFiltered
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
    Public Function GetCarrTarMinChargesFiltered(ByVal CarrTarControl As Integer,
                                                 Optional ByVal page As Integer = 1,
                                                 Optional ByVal pagesize As Integer = 1000,
                                                 Optional ByVal filterWhere As String = "",
                                                 Optional ByVal sSortKey As String = "") As DataTransferObjects.CarrTarMinCharge()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try


                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1

                'db.Log = New DebugTextWriter

                Dim oQuery = From d In db.CarrierTariffMinCharges
                        Where d.CarrTarMinChargeCarrTarControl = CarrTarControl
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

                Dim oRecords() As DataTransferObjects.CarrTarMinCharge
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
                            Order By d.CarrTarMinChargeCountry, d.CarrTarMinChargeState, d.CarrTarMinChargeCity, d.CarrTarMinChargeZipFrom, d.CarrTarMinChargeZipTo
                            Select selectDTOData(d, page, intPageCount, intRecordCount, pagesize)
                        ).Skip(intSkip).Take(pagesize).ToArray()
                End If

                Return oRecords

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetCarrTarMinChargesFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetCarrTarMinChargeNodes(ByVal CarrTarControl As Integer, ByVal ParentTreeID As Integer) As List(Of NGLTreeNode)
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'db.Log = New DebugTextWriter
                Dim oNodes As List(Of NGLTreeNode) = (
                        From d In db.CarrierTariffMinCharges
                        Where (d.CarrTarMinChargeCarrTarControl = CarrTarControl)
                        Order By d.CarrTarMinChargeCountry, d.CarrTarMinChargeState, d.CarrTarMinChargeCity, d.CarrTarMinChargeZipFrom, d.CarrTarMinChargeZipTo
                        Select New DataTransferObjects.NGLTreeNode With {.Control = d.CarrTarMinChargeControl,
                        .ParentTreeID = ParentTreeID,
                        .Name = d.CarrTarMinChargeName,
                        .Description = " | " & d.CarrTarMinChargeCountry & " | " & d.CarrTarMinChargeState & " | " & d.CarrTarMinChargeCity & " | " & d.CarrTarMinChargeZipFrom & " | " & d.CarrTarMinChargeZipTo & " | ",
                        .ClassName = "CarrTarMinCharge"}).ToList

                Return oNodes

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                Return Nothing
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetCarrTarMinChargeNodes"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetCarrTarMinChargeTree(ByVal CarrTarControl As Integer, ByVal ParentTreeID As Integer, ByRef intNextTreeID As Integer) As List(Of NGLTreeNode)
        Try
            Dim oTreeNodes As List(Of NGLTreeNode) = GetCarrTarMinChargeNodes(CarrTarControl, ParentTreeID)
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
            throwUnExpectedFaultException(ex, buildProcedureName("GetCarrTarMinChargeTree"))
        End Try
        Return Nothing
    End Function

    Public Function GetCarrTarMinChargeTreeFlat(ByVal CarrTarControl As Integer, ByVal ParentTreeID As Integer, ByRef intNextTreeID As Integer) As List(Of NGLTreeNode)
        Try

            Dim oTreeNodes As List(Of NGLTreeNode) = GetCarrTarMinChargeNodes(CarrTarControl, ParentTreeID)
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
            throwUnExpectedFaultException(ex, buildProcedureName("GetCarrTarMinChargeTreeFlat"))
        End Try
        Return Nothing
    End Function


#Region "LTS carrier tariff min charge data"

    ''' <summary>
    ''' Returns the MinCharge data assoicated with a Contract the CarrTarControl value must be provided in the filters.ParentControl parameter
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 08/31/2018
    '''   new LTS carrier tariff MinCharge query  
    ''' </remarks>
    Public Function GetCarrierTariffMinCharges(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vCarrierTariffMinCharge()
        If filters Is Nothing Then Return Nothing
        If filters.ParentControl = 0 Then
            Dim sMsg As String = " The Tariff ID is missing. Please return to the tariff contract page and select a valid carrier contract."
            throwNoDataFaultException(sMsg)
        End If
        Dim oRet() As LTS.vCarrierTariffMinCharge

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                Dim iQuery As IQueryable(Of LTS.vCarrierTariffMinCharge)
                iQuery = db.vCarrierTariffMinCharges
                Dim filterWhere As String = " (CarrTarMinChargeCarrTarControl = " & filters.ParentControl & ") "
                Dim sFilterSpacer As String = " And "

                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "CarrTarMinChargeName"
                    filters.sortDirection = "asc"
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrierTariffMinCharges"), db)
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    '''  Insert or Update the carrier tariff MinCharges data
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 08/31/2018
    ''' </remarks>
    Public Function SaveCarrierTariffMinCharge(ByVal oData As LTS.vCarrierTariffMinCharge) As Boolean
        Dim blnRet As Boolean = False
        If oData Is Nothing Then Return False 'nothing to do

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'verify the service contract
                If oData.CarrTarMinChargeCarrTarControl = 0 Then
                    Dim lDetails As New List(Of String) From {"Carrier Contract Reference", " was not provided and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                If Not db.CarrierTariffs.Any(Function(x) x.CarrTarControl = oData.CarrTarMinChargeCarrTarControl) Then

                    Dim lDetails As New List(Of String) From {"Carrier Contract Reference", " was not found and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                If oData.CarrTarMinChargeControl = 0 Then
                    Dim oNew As New LTS.CarrierTariffMinCharge()
                    Dim strMSG As String = ""
                    Dim skipObjs As New List(Of String) From {"CarrTarMinChargeModUser", "CarrTarMinChargeModDate", "CarrTarMinChargeUpdated"}
                    oNew = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(oNew, oData, skipObjs, strMSG)
                    With oNew
                        .CarrTarMinChargeModDate = Date.Now
                        .CarrTarMinChargeModUser = Me.Parameters.UserName
                    End With
                    If Debugger.IsAttached AndAlso Not String.IsNullOrWhiteSpace(strMSG) Then
                        System.Diagnostics.Debug.WriteLine(strMSG)
                    End If
                    db.CarrierTariffMinCharges.InsertOnSubmit(oNew)
                Else
                    Dim oExisting = db.CarrierTariffMinCharges.Where(Function(x) x.CarrTarMinChargeControl = oData.CarrTarMinChargeControl).FirstOrDefault()
                    If oExisting Is Nothing OrElse oExisting.CarrTarMinChargeControl = 0 Then
                        throwRecordDeletedFaultException("Cannot save changes for tariff MinCharge: " & oData.CarrTarMinChargeName)
                    End If
                    Dim strMSG As String = ""
                    Dim skipObjs As New List(Of String) From {"CarrTarMinChargeModUser", "CarrTarMinChargeModDate", "CarrTarMinChargeUpdated"}
                    oExisting = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(oExisting, oData, skipObjs, strMSG)
                    With oExisting
                        .CarrTarMinChargeModDate = Date.Now
                        .CarrTarMinChargeModUser = Me.Parameters.UserName
                    End With
                End If
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveCarrierTariffMinCharge"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Delete the provided carrier tariff MinCharge
    ''' </summary>
    ''' <param name="iControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 08/31/2018
    ''' </remarks>
    Public Function DeleteCarrierTariffMinCharge(ByVal iControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        If iControl = 0 Then Return False 'nothing to do
        'validate before delete

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Delete the Record
                Dim oToDelete = db.CarrierTariffMinCharges.Where(Function(x) x.CarrTarMinChargeControl = iControl).FirstOrDefault()
                If oToDelete Is Nothing OrElse oToDelete.CarrTarMinChargeControl = 0 Then Return True 'already deleted
                db.CarrierTariffMinCharges.DeleteOnSubmit(oToDelete)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteCarrierTariffMinCharge"), db)
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
        Dim oMinCharge As DataTransferObjects.CarrTarMinCharge = CType(oData, DataTransferObjects.CarrTarMinCharge)
        Using LinqDB
            'Note: the ValidateData Function must throw a FaultException error on failure
            ValidateUpdatedRecord(LinqDB, oMinCharge)
            Dim d = (From e In CType(LinqDB, NGLMASCarrierDataContext).CarrierTariffMinCharges Where e.CarrTarMinChargeControl = oMinCharge.CarrTarMinChargeControl Select e).FirstOrDefault()
            If d Is Nothing OrElse d.CarrTarMinChargeControl = 0 Then Return Nothing
            'Create New Record 
            CopyDTOToLTS(oMinCharge, d)
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
            Return GetCarrTarMinChargeFiltered(Control:=oMinCharge.CarrTarMinChargeControl)
        End Using
    End Function

    ''' <summary>
    ''' Deletes a CarrTarMinCharge record by control number without validation. It can only be called by system only process.
    ''' No validation is neccessary to delete CarrTarMinCharge records because there is no FKey
    ''' </summary>
    ''' <param name="ControlNumber"></param>
    ''' <remarks>
    ''' Added by LVV on 8/19/16 for v-7.0.5.102
    ''' </remarks>
    Public Overloads Sub Delete(ByVal ControlNumber As Integer)
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            If db.CarrierTariffMinCharges.Any(Function(x) x.CarrTarMinChargeControl = ControlNumber) Then
                Dim nObject = db.CarrierTariffMinCharges.Where(Function(x) x.CarrTarMinChargeControl = ControlNumber)
                db.CarrierTariffMinCharges.DeleteAllOnSubmit(nObject)
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
        Dim d = CType(oData, DataTransferObjects.CarrTarMinCharge)
        'Create New Record
        Return New LTS.CarrierTariffMinCharge With {.CarrTarMinChargeControl = d.CarrTarMinChargeControl,
            .CarrTarMinChargeCarrTarControl = d.CarrTarMinChargeCarrTarControl,
            .CarrTarMinChargePointTypeControl = d.CarrTarMinChargePointTypeControl,
            .CarrTarMinChargeName = d.CarrTarMinChargeName,
            .CarrTarMinChargeEffDateFrom = d.CarrTarMinChargeEffDateFrom,
            .CarrTarMinChargeEffDateTo = d.CarrTarMinChargeEffDateTo,
            .CarrTarMinChargeCountry = d.CarrTarMinChargeCountry,
            .CarrTarMinChargeState = d.CarrTarMinChargeState,
            .CarrTarMinChargeCity = d.CarrTarMinChargeCity,
            .CarrTarMinChargeZipFrom = d.CarrTarMinChargeZipFrom,
            .CarrTarMinChargeZipTo = d.CarrTarMinChargeZipTo,
            .CarrTarMinChargeValue = d.CarrTarMinChargeValue,
            .CarrTarMinChargeModUser = Parameters.UserName,
            .CarrTarMinChargeModDate = Date.Now,
            .CarrTarMinChargeUpdated = If(d.CarrTarMinChargeUpdated Is Nothing, New Byte() {}, d.CarrTarMinChargeUpdated)}
    End Function

    ''' <summary>
    ''' CopyDTOToLTS
    ''' </summary>
    ''' <param name="d"></param>
    ''' <param name="l"></param>
    ''' <remarks>
    ''' Added by LVV on 8/19/16 for v-7.0.5.102
    ''' </remarks>
    Protected Sub CopyDTOToLTS(ByRef d As DataTransferObjects.CarrTarMinCharge, ByRef l As LTS.CarrierTariffMinCharge)
        'Create New Record
        With l
            .CarrTarMinChargeName = d.CarrTarMinChargeName
            .CarrTarMinChargeCity = d.CarrTarMinChargeCity
            .CarrTarMinChargeState = d.CarrTarMinChargeState
            .CarrTarMinChargeCountry = d.CarrTarMinChargeCountry
            .CarrTarMinChargeZipTo = d.CarrTarMinChargeZipTo
            .CarrTarMinChargeZipFrom = d.CarrTarMinChargeZipFrom
            .CarrTarMinChargeEffDateFrom = d.CarrTarMinChargeEffDateFrom
            .CarrTarMinChargeEffDateTo = d.CarrTarMinChargeEffDateTo
            .CarrTarMinChargePointTypeControl = d.CarrTarMinChargePointTypeControl
            .CarrTarMinChargeValue = d.CarrTarMinChargeValue
            .CarrTarMinChargeModUser = Parameters.UserName
            .CarrTarMinChargeModDate = Date.Now
        End With

    End Sub


    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCarrTarMinChargeFiltered(Control:=CType(LinqTable, LTS.CarrierTariffMinCharge).CarrTarMinChargeControl)
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
                Dim source As LTS.CarrierTariffMinCharge = TryCast(LinqTable, LTS.CarrierTariffMinCharge)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.CarrierTariffMinCharges
                    Where d.CarrTarMinChargeControl = source.CarrTarMinChargeControl
                    Select New DataTransferObjects.QuickSaveResults With {.Control = d.CarrTarMinChargeControl _
                        , .ModDate = d.CarrTarMinChargeModDate _
                        , .ModUser = d.CarrTarMinChargeModUser _
                        , .Updated = d.CarrTarMinChargeUpdated.ToArray}).First

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

    Private Sub ValidateApproved(ByRef oDB As NGLMASCarrierDataContext, ByRef oData As DataTransferObjects.CarrTarMinCharge)
        DirectCast(NDPBaseClassFactory("NGLCarrTarContractData", False), NGLCarrTarContractData).ValidateApproved(oData.CarrTarMinChargeCarrTarControl, oDB)
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

    Friend Function selectDTOData(ByVal d As LTS.CarrierTariffMinCharge, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.CarrTarMinCharge
        Return New DataTransferObjects.CarrTarMinCharge With {.CarrTarMinChargeControl = d.CarrTarMinChargeControl,
            .CarrTarMinChargeCarrTarControl = d.CarrTarMinChargeCarrTarControl,
            .CarrTarMinChargePointTypeControl = d.CarrTarMinChargePointTypeControl,
            .CarrTarMinChargeName = d.CarrTarMinChargeName,
            .CarrTarMinChargeEffDateFrom = d.CarrTarMinChargeEffDateFrom,
            .CarrTarMinChargeEffDateTo = d.CarrTarMinChargeEffDateTo,
            .CarrTarMinChargeCountry = d.CarrTarMinChargeCountry,
            .CarrTarMinChargeState = d.CarrTarMinChargeState,
            .CarrTarMinChargeCity = d.CarrTarMinChargeCity,
            .CarrTarMinChargeZipFrom = d.CarrTarMinChargeZipFrom,
            .CarrTarMinChargeZipTo = d.CarrTarMinChargeZipTo,
            .CarrTarMinChargeValue = d.CarrTarMinChargeValue,
            .CarrTarMinChargeModUser = d.CarrTarMinChargeModUser,
            .CarrTarMinChargeModDate = d.CarrTarMinChargeModDate,
            .CarrTarMinChargeUpdated = d.CarrTarMinChargeUpdated.ToArray(),
            .Page = page,
            .Pages = pagecount,
            .RecordCount = recordcount,
            .PageSize = pagesize}
    End Function

#End Region

End Class