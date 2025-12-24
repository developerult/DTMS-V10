Imports System.ServiceModel

Public Class NGLCarrierFuelAdExData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters

        Me.SourceClass = "NGLCarrierFuelAdExData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASCarrierDataContext(ConnectionString)
                _LinqTable = db.CarrierFuelAdExes
                _LinqDB = db
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
        Return GetCarrierFuelAdExFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetCarrierFuelAdExsFiltered()
    End Function

    Public Function GetCarrierFuelAdExFiltered(ByVal Control As Integer) As DataTransferObjects.CarrierFuelAdEx
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim CarrierFuelAdEx As DataTransferObjects.CarrierFuelAdEx = (
                        From t In db.CarrierFuelAdExes
                        Where
                        (t.CarrFuelAdExControl = Control)
                        Select selectDTOData(t)).FirstOrDefault()
                Return CarrierFuelAdEx

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetCarrierFuelAdExsFiltered(Optional ByVal CarrFuelAdControl As Integer = 0) As DataTransferObjects.CarrierFuelAdEx()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim CarrierFuelAdExs() As DataTransferObjects.CarrierFuelAdEx = (
                        From t In db.CarrierFuelAdExes
                        Where
                        t.CarrFuelAdExCarrFuelAdContol = If(CarrFuelAdControl = 0, t.CarrFuelAdExCarrFuelAdContol, CarrFuelAdControl)
                        Order By t.CarrFuelAdExEffDate
                        Select selectDTOData(t)).ToArray()
                Return CarrierFuelAdExs

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetCarrierFuelAdExsWPagingFiltered(ByVal CarrFuelAdControl As Integer,
                                                       Optional ByVal page As Integer = 1,
                                                       Optional ByVal pagesize As Integer = 1000) As DataTransferObjects.CarrierFuelAdEx()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1

                Dim oQuery =
                        From t In db.CarrierFuelAdExes
                        Where
                        t.CarrFuelAdExCarrFuelAdContol = If(CarrFuelAdControl = 0, t.CarrFuelAdExCarrFuelAdContol, CarrFuelAdControl)
                        Order By t.CarrFuelAdExEffDate
                        Select t

                intRecordCount = oQuery.Count
                If intRecordCount < 1 Then Return Nothing
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize

                Dim oRecords() As DataTransferObjects.CarrierFuelAdEx = (
                        From d In oQuery
                        Order By d.CarrFuelAdExEffDate
                        Select selectDTOData(d, page, intPageCount, intRecordCount, pagesize)
                        ).Skip(intSkip).Take(pagesize).ToArray()

                Return oRecords


            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrierFuelAdExsWPagingFiltered"))
            End Try

            Return Nothing

        End Using
    End Function



    ''' <summary>
    ''' Copies the LTS data into a new DTO object
    ''' </summary>
    ''' <param name="d"></param>
    ''' <param name="page"></param>
    ''' <param name="pagecount"></param>
    ''' <param name="recordcount"></param>
    ''' <param name="pagesize"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR v-8.5.4.005 on 03/27/2024
    ''' </remarks> 
    Friend Shared Function selectDTOData(ByVal d As LTS.CarrierFuelAdEx, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.CarrierFuelAdEx
        Dim oDTO As New DataTransferObjects.CarrierFuelAdEx
        Dim skipObjs As New List(Of String) From {"CarrFuelAdExUpdated",
                "Page",
                "Pages",
                "RecordCount",
                "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .CarrFuelAdExUpdated = d.CarrFuelAdExUpdated.ToArray()
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With

        Return oDTO

    End Function

    ''' <summary>
    '''  Copies the LTS data into a new LTS object
    ''' </summary>
    ''' <param name="d"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR v-8.5.4.005 on 03/27/2024
    ''' </remarks>
    Friend Shared Function selectLTSData(ByVal d As LTS.CarrierFuelAdEx) As LTS.CarrierFuelAdEx
        Dim oLTS As New LTS.CarrierFuelAdEx
        'the original Carrier DTO object was not designed to support NULL integers
        'so we need to process those values using skipObjs
        Dim skipObjs As New List(Of String) From {"CarrFuelAdExUpdated",
                "Page",
                "Pages",
                "RecordCount",
                "PageSize"}
        oLTS = CopyMatchingFields(oLTS, d, skipObjs)
        'add custom formatting
        With oLTS
            .CarrFuelAdExUpdated = If(d.CarrFuelAdExUpdated Is Nothing, New Byte() {}, d.CarrFuelAdExUpdated)
        End With

        Return oLTS

    End Function

    ''' <summary>
    ''' Copies the DTO data into a new LTS object
    ''' </summary>
    ''' <param name="d"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR v-8.5.4.005 on 03/27/2024
    ''' </remarks>
    Friend Shared Function selectLTSData(ByVal d As DataTransferObjects.CarrierFuelAdEx, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As LTS.CarrierFuelAdEx
        Dim oLTS As New LTS.CarrierFuelAdEx
        'the original Carrier DTO object was not designed to support NULL integers
        'so we need to process those values using skipObjs
        Dim skipObjs As New List(Of String) From {"CarrFuelAdExUpdated",
                "Page",
                "Pages",
                "RecordCount",
                "PageSize"}
        oLTS = CopyMatchingFields(oLTS, d, skipObjs)
        'add custom formatting
        With oLTS
            .CarrFuelAdExUpdated = If(d.CarrFuelAdExUpdated Is Nothing, New Byte() {}, d.CarrFuelAdExUpdated)
        End With

        Return oLTS

    End Function


#End Region

#Region "LTS carrier tariff fuel exception data"


    ''' <summary>
    ''' Returns the view for  carrier fuel Ex data assoicated with a Carrier Fuel Addendum.
    ''' A CarrFuelAdExControl filter  or the  CarrFuelAdExCarrFuelAdContol value must be provided in the filters.ParentControl
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.4.005 on 01/25/201903/27/2024
    ''' </remarks>
    Public Function GetvCarrierFuelAdEx(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vCarrierFuelAdEx()
        If filters Is Nothing Then Return Nothing

        Dim iCarrFuelAdExControl As Integer = 0
        Dim iCarrFuelAdExCarrFuelAdContol As Integer = 0
        Dim filterWhere As String = ""
        Dim sFilterSpacer As String = ""
        If Not filters.addParentFilterIfNoRecordControlFilter("CarrFuelAdExControl", "CarrFuelAdExCarrFuelAdContol", iCarrFuelAdExControl, iCarrFuelAdExCarrFuelAdContol, filterWhere, sFilterSpacer) Then
            throwNoDataFaultMessage("E_MissingParent") ' "The reference to the parent record is missing. Please select a valid parent record and try again."
        End If
        If iCarrFuelAdExCarrFuelAdContol = 0 And iCarrFuelAdExControl = 0 Then
            'we do not have a valid filter so return nothing
            Return Nothing
            'throwNoDataFaultMessage("E_InvalidRecordKey") ' "The reference to the record is missing. Please select another record and try again."
        End If

        Dim oRet() As LTS.vCarrierFuelAdEx

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                Dim iQuery As IQueryable(Of LTS.vCarrierFuelAdEx)
                iQuery = db.vCarrierFuelAdExes
                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "CarrFuelAdExState"
                    filters.sortDirection = "ASC"
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                'db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetvCarrierFuelAdEx"), db)
            End Try
        End Using
        Return oRet
    End Function


    ''' <summary>
    ''' Returns the  carrier fuel Ex data assoicated with a Carrier Fuel Addendum.
    ''' A CarrFuelAdExControl filter  or the  CarrFuelAdExCarrFuelAdContol value must be provided in the filters.ParentControl
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 01/25/2019
    ''' </remarks>
    Public Function GetCarrierFuelAdEx(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.CarrierFuelAdEx()
        If filters Is Nothing Then Return Nothing

        '  [CarrFuelAdExControl]
        ',[CarrFuelAdExCarrFuelAdContol]
        ',[CarrFuelAdExState]
        ',[CarrFuelAdExRatePerMile]
        ',[CarrFuelAdExPercent]
        ',[CarrFuelAdExEffDate]
        ',[CarrFuelAdExModUser]
        ',[CarrFuelAdExModDate]
        ',[CarrFuelAdExUpdated]


        Dim iCarrFuelAdExControl As Integer = 0
        Dim iCarrFuelAdExCarrFuelAdContol As Integer = 0
        Dim filterWhere As String = ""
        Dim sFilterSpacer As String = ""
        If Not filters.addParentFilterIfNoRecordControlFilter("CarrFuelAdExControl", "CarrFuelAdExCarrFuelAdContol", iCarrFuelAdExControl, iCarrFuelAdExCarrFuelAdContol, filterWhere, sFilterSpacer) Then
            throwNoDataFaultMessage("E_MissingParent") ' "The reference to the parent record is missing. Please select a valid parent record and try again."
        End If
        If iCarrFuelAdExCarrFuelAdContol = 0 And iCarrFuelAdExControl = 0 Then
            'we do not have a valid filter so return nothing
            Return Nothing
            'throwNoDataFaultMessage("E_InvalidRecordKey") ' "The reference to the record is missing. Please select another record and try again."
        End If

        Dim oRet() As LTS.CarrierFuelAdEx

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                Dim iQuery As IQueryable(Of LTS.CarrierFuelAdEx)
                iQuery = db.CarrierFuelAdExes
                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "CarrFuelAdExState"
                    filters.sortDirection = "ASC"
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                'db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrierFuelAdEx"), db)
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' Returns the  carrier fuel Ex data assoicated with a Carrier Tariff, it looks up the Fuel Addendum using the tariff control.
    ''' A CarrFuelAdExControl filter  or the  CarrFuelAdCarrTarControl value must be provided in the filters.ParentControl
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 01/25/2019
    ''' </remarks>
    Public Function GetCarrierFuelAdExByTariff(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.CarrierFuelAdEx()
        If filters Is Nothing Then Return Nothing

        '  [CarrFuelAdExControl]
        ',[CarrFuelAdExCarrFuelAdContol]
        ',[CarrFuelAdExState]
        ',[CarrFuelAdExRatePerMile]
        ',[CarrFuelAdExPercent]
        ',[CarrFuelAdExEffDate]
        ',[CarrFuelAdExModUser]
        ',[CarrFuelAdExModDate]
        ',[CarrFuelAdExUpdated]

        Dim iCarrFuelAdCarrTarControl As Integer = 0
        Dim iCarrFuelAdExControl As Integer = 0
        Dim iCarrFuelAdExCarrFuelAdContol As Integer = 0
        Dim filterWhere As String = ""
        Dim sFilterSpacer As String = ""

        Dim oRet() As LTS.CarrierFuelAdEx



        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                If (filters.FilterValues Is Nothing OrElse filters.FilterValues.Count() < 1) OrElse (Not filters.FilterValues.Any(Function(x) x.filterName = "CarrFuelAdExControl")) Then
                    'The Record Control Filter does not exist so use the parent control fliter
                    If filters.ParentControl = 0 Then
                        throwNoDataFaultMessage("E_MissingParent") ' "The reference to the parent record is missing. Please select a valid parent record and try again."
                    End If
                    iCarrFuelAdCarrTarControl = filters.ParentControl
                    'get the iCarrFuelAdExCarrFuelAdContol using the parentcontrol
                    iCarrFuelAdExCarrFuelAdContol = db.CarrierFuelAddendums.Where(Function(x) x.CarrFuelAdCarrTarControl = iCarrFuelAdCarrTarControl).Select(Function(x) x.CarrFuelAdControl).FirstOrDefault()
                    If iCarrFuelAdExCarrFuelAdContol = 0 Then
                        'we do not have a valid filter so return nothing
                        Return Nothing
                        'throwNoDataFaultMessage("E_InvalidRecordKey") ' "The reference to the record is missing. Please select another record and try again."
                    End If
                    filterWhere = " (CarrFuelAdExCarrFuelAdContol = " & iCarrFuelAdExCarrFuelAdContol.ToString() & ") "
                    sFilterSpacer = " And "
                Else
                    Dim tFilter As Models.FilterDetails = filters.FilterValues.Where(Function(x) x.filterName = "CarrFuelAdExControl").FirstOrDefault()
                    Integer.TryParse(tFilter.filterValueFrom, iCarrFuelAdExControl)
                End If

                If iCarrFuelAdExCarrFuelAdContol = 0 And iCarrFuelAdExControl = 0 Then
                    'we do not have a valid filter
                    throwNoDataFaultMessage("E_InvalidRecordKey") ' "The reference to the record is missing. Please select another record and try again."
                End If

                Dim iQuery As IQueryable(Of LTS.CarrierFuelAdEx)
                iQuery = db.CarrierFuelAdExes
                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "CarrFuelAdExState"
                    filters.sortDirection = "ASC"
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                'db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrierFuelAdExByTariff"), db)
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    '''  Insert or Update the carrier tariff fuel exception data  assoicated with a Carrier Fuel Addendum.
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 01/25/2019
    ''' </remarks>
    Public Function SaveCarrierFuelAdEx(ByVal oData As LTS.CarrierFuelAdEx, Optional ByVal iCarrFuelAdControl As Integer = 0) As Boolean
        Dim blnRet As Boolean = False
        If oData Is Nothing Then Return False 'nothing to do
        Dim iCarrFuelAdExCarrFuelAdContol As Integer = oData.CarrFuelAdExCarrFuelAdContol
        Dim iCarrFuelAdExControl As Integer = oData.CarrFuelAdExControl
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                If iCarrFuelAdExCarrFuelAdContol = 0 Then
                    iCarrFuelAdExCarrFuelAdContol = iCarrFuelAdControl
                    oData.CarrFuelAdExCarrFuelAdContol = iCarrFuelAdControl
                End If

                'verify the fuel addendum exist
                If iCarrFuelAdExCarrFuelAdContol = 0 Then
                    Dim lDetails As New List(Of String) From {"Carrier Contract Fuel Addendum", " was not provided and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                'If Not db.CarrierFuelAddendums.Any(Function(x) x.CarrFuelAdControl = iCarrFuelAdExCarrFuelAdContol) Then
                '    Dim lDetails As New List(Of String) From {"Carrier Contract Fuel Addendum", " was not found and "}
                '    throwInvalidKeyParentRequiredException(lDetails)
                '    Return False
                'End If
                With oData
                    .CarrFuelAdExModDate = Date.Now
                    .CarrFuelAdExModUser = Me.Parameters.UserName
                End With
                If iCarrFuelAdExControl = 0 Then
                    oData.CarrFuelAdExUpdated = New Byte() {}
                    db.CarrierFuelAdExes.InsertOnSubmit(oData)
                Else
                    db.CarrierFuelAdExes.Attach(oData, True)
                End If
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveCarrierFuelAdEx"), db)
            End Try
        End Using
        Return blnRet
    End Function



    ''' <summary>
    '''  Insert or Update the carrier tariff fuel exception data  assoicated with a Carrier Tariff, on insert the system looks up the Fuel Addendum using the iCarrFuelAdCarrTarControl provided.
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 01/25/2019
    ''' </remarks>
    Public Function SaveCarrierFuelAdExByTariff(ByVal oData As LTS.CarrierFuelAdEx, Optional ByVal iCarrFuelAdCarrTarControl As Integer = 0) As Boolean
        Dim blnRet As Boolean = False
        If oData Is Nothing Then Return False 'nothing to do
        Dim iCarrFuelAdExCarrFuelAdContol As Integer = oData.CarrFuelAdExCarrFuelAdContol
        Dim iCarrFuelAdExControl As Integer = oData.CarrFuelAdExControl
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'verify the fuel addendum exist
                If iCarrFuelAdExCarrFuelAdContol = 0 Then
                    'look up the fuel addendum if it is missing
                    If iCarrFuelAdCarrTarControl <> 0 Then
                        iCarrFuelAdExCarrFuelAdContol = db.CarrierFuelAddendums.Where(Function(x) x.CarrFuelAdCarrTarControl = iCarrFuelAdCarrTarControl).Select(Function(x) x.CarrFuelAdControl).FirstOrDefault()
                        'verify the fuel addendum exist
                        If iCarrFuelAdExCarrFuelAdContol = 0 Then
                            Dim lDetails As New List(Of String) From {"Carrier Contract Fuel Addendum", " was not provided and "}
                            throwInvalidKeyParentRequiredException(lDetails)
                            Return False
                        End If
                        oData.CarrFuelAdExCarrFuelAdContol = iCarrFuelAdExCarrFuelAdContol
                    Else
                        Dim lDetails As New List(Of String) From {"Carrier Contract Fuel Addendum", " was not provided and "}
                        throwInvalidKeyParentRequiredException(lDetails)
                        Return False
                    End If
                ElseIf Not db.CarrierFuelAddendums.Any(Function(x) x.CarrFuelAdControl = iCarrFuelAdExCarrFuelAdContol) Then
                    Dim lDetails As New List(Of String) From {"Carrier Contract Fuel Addendum", " was not found and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                With oData
                    .CarrFuelAdExModDate = Date.Now
                    .CarrFuelAdExModUser = Me.Parameters.UserName
                End With
                If iCarrFuelAdExControl = 0 Then
                    oData.CarrFuelAdExUpdated = New Byte() {}
                    db.CarrierFuelAdExes.InsertOnSubmit(oData)
                Else
                    db.CarrierFuelAdExes.Attach(oData, True)
                End If
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveCarrierFuelAdExByTariff"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Delete the provided carrier Fuel Addenum Ex
    ''' </summary>
    ''' <param name="iControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 01/25/2019
    ''' </remarks>
    Public Function DeleteCarrierFuelAdEx(ByVal iControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        If iControl = 0 Then Return False 'nothing to do
        'validate before delete

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Delete the Record
                Dim oToDelete = db.CarrierFuelAdExes.Where(Function(x) x.CarrFuelAdExControl = iControl).FirstOrDefault()
                If oToDelete Is Nothing OrElse oToDelete.CarrFuelAdExControl = 0 Then Return True 'already deleted
                db.CarrierFuelAdExes.DeleteOnSubmit(oToDelete)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteCarrierFuelAdEx"), db)
            End Try
        End Using
        Return blnRet
    End Function


#End Region


#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DataTransferObjects.CarrierFuelAdEx)
        'Create New Record
        Return New LTS.CarrierFuelAdEx With {.CarrFuelAdExControl = d.CarrFuelAdExControl _
            , .CarrFuelAdExCarrFuelAdContol = d.CarrFuelAdExCarrFuelAdContol _
            , .CarrFuelAdExState = d.CarrFuelAdExState _
            , .CarrFuelAdExRatePerMile = d.CarrFuelAdExRatePerMile _
            , .CarrFuelAdExPercent = d.CarrFuelAdExPercent _
            , .CarrFuelAdExEffDate = d.CarrFuelAdExEffDate _
            , .CarrFuelAdExModUser = Parameters.UserName _
            , .CarrFuelAdExModDate = Date.Now _
            , .CarrFuelAdExUpdated = If(d.CarrFuelAdExUpdated Is Nothing, New Byte() {}, d.CarrFuelAdExUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCarrierFuelAdExFiltered(Control:=CType(LinqTable, LTS.CarrierFuelAdEx).CarrFuelAdExControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim source As LTS.CarrierFuelAdEx = TryCast(LinqTable, LTS.CarrierFuelAdEx)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.CarrierFuelAdExes
                    Where d.CarrFuelAdExControl = source.CarrFuelAdExControl
                    Select New DataTransferObjects.QuickSaveResults With {.Control = d.CarrFuelAdExControl _
                        , .ModDate = d.CarrFuelAdExModDate _
                        , .ModUser = d.CarrFuelAdExModUser _
                        , .Updated = d.CarrFuelAdExUpdated.ToArray}).First

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

        End Using
        Return ret
    End Function

    'Friend Function selectDTOData(ByVal t As LTS.CarrierFuelAdEx, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.CarrierFuelAdEx
    '    Return New DTO.CarrierFuelAdEx With {.CarrFuelAdExControl = t.CarrFuelAdExControl _
    '                                    , .CarrFuelAdExCarrFuelAdContol = t.CarrFuelAdExCarrFuelAdContol _
    '                                    , .CarrFuelAdExState = t.CarrFuelAdExState _
    '                                    , .CarrFuelAdExRatePerMile = If(t.CarrFuelAdExRatePerMile.HasValue, t.CarrFuelAdExRatePerMile.Value, 0) _
    '                                    , .CarrFuelAdExPercent = If(t.CarrFuelAdExPercent.HasValue, t.CarrFuelAdExPercent.Value, 0) _
    '                                    , .CarrFuelAdExEffDate = t.CarrFuelAdExEffDate _
    '                                    , .CarrFuelAdExModUser = t.CarrFuelAdExModUser _
    '                                    , .CarrFuelAdExModDate = t.CarrFuelAdExModDate _
    '                                    , .CarrFuelAdExUpdated = t.CarrFuelAdExUpdated.ToArray(),
    '                                               .Page = page,
    '                                               .Pages = pagecount,
    '                                               .RecordCount = recordcount,
    '                                               .PageSize = pagesize}
    'End Function

#End Region

End Class