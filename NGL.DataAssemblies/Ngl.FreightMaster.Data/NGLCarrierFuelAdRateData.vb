Imports System.ServiceModel

Public Class NGLCarrierFuelAdRateData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMASCarrierDataContext(ConnectionString)
        Me.LinqTable = db.CarrierFuelAdRates
        Me.LinqDB = db
        Me.SourceClass = "NGLCarrierFuelAdRateData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASCarrierDataContext(ConnectionString)
                _LinqTable = db.CarrierFuelAdRates
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
        Return GetCarrierFuelAdRateFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetCarrierFuelAdRatesFiltered()
    End Function

    Public Function GetCarrierFuelAdRateFiltered(ByVal Control As Integer) As DataTransferObjects.CarrierFuelAdRate
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim CarrierFuelAdRate As DataTransferObjects.CarrierFuelAdRate = (
                        From t In db.CarrierFuelAdRates
                        Where
                        (t.CarrFuelAdRatesControl = Control)
                        Select New DataTransferObjects.CarrierFuelAdRate With {.CarrFuelAdRatesControl = t.CarrFuelAdRatesControl _
                        , .CarrFuelAdRatesCarrFuelAdControl = t.CarrFuelAdRatesCarrFuelAdControl _
                        , .CarrFuelAdRatesPriceFrom = t.CarrFuelAdRatesPriceFrom _
                        , .CarrFuelAdRatesPriceTo = t.CarrFuelAdRatesPriceTo _
                        , .CarrFuelAdRatesPerMile = If(t.CarrFuelAdRatesPerMile.HasValue, t.CarrFuelAdRatesPerMile.Value, 0) _
                        , .CarrFuelAdRatesPercent = If(t.CarrFuelAdRatesPercent.HasValue, t.CarrFuelAdRatesPercent.Value, 0) _
                        , .CarrFuelAdRatesEffDate = t.CarrFuelAdRatesEffDate _
                        , .CarrFuelAdRatesModUser = t.CarrFuelAdRatesModUser _
                        , .CarrFuelAdRatesModDate = t.CarrFuelAdRatesModDate _
                        , .CarrFuelAdRatesUpdated = t.CarrFuelAdRatesUpdated.ToArray()}).Single
                Return CarrierFuelAdRate

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

    Public Function GetCarrierFuelAdRatesFiltered(Optional ByVal CarrFuelAdControl As Integer = 0) As DataTransferObjects.CarrierFuelAdRate()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim CarrierFuelAdRates() As DataTransferObjects.CarrierFuelAdRate = (
                        From t In db.CarrierFuelAdRates
                        Where
                        t.CarrFuelAdRatesCarrFuelAdControl = If(CarrFuelAdControl = 0, t.CarrFuelAdRatesCarrFuelAdControl, CarrFuelAdControl)
                        Order By t.CarrFuelAdRatesEffDate
                        Select New DataTransferObjects.CarrierFuelAdRate With {.CarrFuelAdRatesControl = t.CarrFuelAdRatesControl _
                        , .CarrFuelAdRatesCarrFuelAdControl = t.CarrFuelAdRatesCarrFuelAdControl _
                        , .CarrFuelAdRatesPriceFrom = t.CarrFuelAdRatesPriceFrom _
                        , .CarrFuelAdRatesPriceTo = t.CarrFuelAdRatesPriceTo _
                        , .CarrFuelAdRatesPerMile = If(t.CarrFuelAdRatesPerMile.HasValue, t.CarrFuelAdRatesPerMile.Value, 0) _
                        , .CarrFuelAdRatesPercent = If(t.CarrFuelAdRatesPercent.HasValue, t.CarrFuelAdRatesPercent.Value, 0) _
                        , .CarrFuelAdRatesEffDate = t.CarrFuelAdRatesEffDate _
                        , .CarrFuelAdRatesModUser = t.CarrFuelAdRatesModUser _
                        , .CarrFuelAdRatesModDate = t.CarrFuelAdRatesModDate _
                        , .CarrFuelAdRatesUpdated = t.CarrFuelAdRatesUpdated.ToArray()}).ToArray()
                Return CarrierFuelAdRates

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

    Public Function GetCarrierFuelAdRatesWPagingFiltered(ByVal CarrFuelAdControl As Integer,
                                                         Optional ByVal page As Integer = 1,
                                                         Optional ByVal pagesize As Integer = 1000) As DataTransferObjects.CarrierFuelAdRate()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1

                Dim oQuery =
                        From t In db.CarrierFuelAdRates
                        Where
                        t.CarrFuelAdRatesCarrFuelAdControl = If(CarrFuelAdControl = 0, t.CarrFuelAdRatesCarrFuelAdControl, CarrFuelAdControl)
                        Order By t.CarrFuelAdRatesEffDate
                        Select t

                intRecordCount = oQuery.Count
                If intRecordCount < 1 Then Return Nothing
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize

                Dim oRecords() As DataTransferObjects.CarrierFuelAdRate = (
                        From d In oQuery
                        Order By d.CarrFuelAdRatesEffDate
                        Select selectDTOData(d, page, intPageCount, intRecordCount, pagesize)
                        ).Skip(intSkip).Take(pagesize).ToArray()

                Return oRecords

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrierFuelAdRatesWPagingFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

#End Region

#Region "LTS carrier tariff fuel data"


    ''' <summary>
    ''' Returns the  carrierfuel rate data assoicated with a Carrier Fuel Addendum.
    ''' A CarrTarFeesControl filter  or the  CarrTarControl value must be provided in the filters.ParentControl
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 01/25/2019
    ''' </remarks>
    Public Function GetCarrierFuelAdRates(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.CarrierFuelAdRate()
        If filters Is Nothing Then Return Nothing

        '[CarrFuelAdRatesControl]           Int             IDENTITY (1, 1) Not NULL,
        '[CarrFuelAdRatesCarrFuelAdControl] Int             Not NULL,
        '[CarrFuelAdRatesPriceFrom]         Decimal(10, 5) Not NULL,
        '[CarrFuelAdRatesPriceTo]           Decimal(10, 5) Not NULL,
        '[CarrFuelAdRatesPerMile]           Decimal(10, 5)  NULL,
        '[CarrFuelAdRatesPercent]           Decimal(10, 5)  NULL,
        '[CarrFuelAdRatesEffDate]           DateTime        Not NULL,
        '[CarrFuelAdRatesModUser]           NVARCHAR(100)  NULL,
        '[CarrFuelAdRatesModDate]           DateTime        NULL,
        '[CarrFuelAdRatesUpdated]           ROWVERSION      NULL,

        Dim iCarrFuelAdRatesControl As Integer = 0
        Dim iCarrFuelAdRatesCarrFuelAdControl As Integer = 0
        Dim filterWhere As String = ""
        Dim sFilterSpacer As String = ""
        If Not filters.addParentFilterIfNoRecordControlFilter("CarrFuelAdRatesControl", "CarrFuelAdRatesCarrFuelAdControl", iCarrFuelAdRatesControl, iCarrFuelAdRatesCarrFuelAdControl, filterWhere, sFilterSpacer) Then
            throwNoDataFaultMessage("E_MissingParent") ' "The reference to the parent record is missing. Please select a valid parent record and try again."
        End If
        If iCarrFuelAdRatesCarrFuelAdControl = 0 And iCarrFuelAdRatesControl = 0 Then
            'we do not have a valid filter so return nothing
            Return Nothing
            'throwNoDataFaultMessage("E_InvalidRecordKey") ' "The reference to the record is missing. Please select another record and try again."
        End If

        Dim oRet() As LTS.CarrierFuelAdRate

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                Dim iQuery As IQueryable(Of LTS.CarrierFuelAdRate)
                iQuery = db.CarrierFuelAdRates
                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "CarrFuelAdRatesPriceFrom"
                    filters.sortDirection = "asc"
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrierFuelAdRates"), db)
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' Returns the  carrier fuel rate data aassoicated with a Carrier and CarrFuelAdCarrTarControl = 0, it looks up the Fuel Addendum using the carrier control.
    ''' A [CarrFuelAdRatesCarrFuelFuelAdControl filter value must be provided in the filters.ParentControl
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 01/25/2019
    ''' </remarks>
    Public Function GetCarrierFuelAdRatesByCarrier(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.CarrierFuelAdRate()
        If filters Is Nothing Then Return Nothing

        '[CarrFuelAdRatesControl]           Int             IDENTITY (1, 1) Not NULL,
        '[CarrFuelAdRatesCarrFuelAdControl] Int             Not NULL,
        '[CarrFuelAdRatesPriceFrom]         Decimal(10, 5) Not NULL,
        '[CarrFuelAdRatesPriceTo]           Decimal(10, 5) Not NULL,
        '[CarrFuelAdRatesPerMile]           Decimal(10, 5)  NULL,
        '[CarrFuelAdRatesPercent]           Decimal(10, 5)  NULL,
        '[CarrFuelAdRatesEffDate]           DateTime        Not NULL,
        '[CarrFuelAdRatesModUser]           NVARCHAR(100)  NULL,
        '[CarrFuelAdRatesModDate]           DateTime        NULL,
        '[CarrFuelAdRatesUpdated]           ROWVERSION      NULL,

        Dim iCarrFuelAdCarrTarControl As Integer = 0
        Dim iCarrFuelAdRatesControl As Integer = 0
        Dim iCarrFuelAdRatesCarrFuelAdControl As Integer = 0
        Dim filterWhere As String = ""
        Dim sFilterSpacer As String = ""


        Dim oRet() As LTS.CarrierFuelAdRate

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                If (filters.FilterValues Is Nothing OrElse filters.FilterValues.Count() < 1) OrElse (Not filters.FilterValues.Any(Function(x) x.filterName = "CarrFuelAdRatesControl")) Then
                    'The Record Control Filter does not exist so use the parent control fliter
                    If filters.ParentControl = 0 Then
                        throwNoDataFaultMessage("E_MissingParent") ' "The reference to the parent record is missing. Please select a valid parent record and try again."
                    End If
                    iCarrFuelAdRatesCarrFuelAdControl = filters.ParentControl
                    'get the iCarrFuelAdExCarrFuelAdContol using the parentcontrol
                    'iCarrFuelAdRatesCarrFuelAdControl = db.CarrierFuelAddendums.Where(Function(x) x.CarrFuelAdCarrTarControl = iCarrFuelAdCarrTarControl).Select(Function(x) x.CarrFuelAdControl).FirstOrDefault()
                    'If iCarrFuelAdRatesCarrFuelAdControl = 0 Then
                    '    'we do not have a valid filter so return nothing
                    '    Return Nothing
                    '    'throwNoDataFaultMessage("E_InvalidRecordKey") ' "The reference to the record is missing. Please select another record and try again."
                    'End If
                    filterWhere = " (CarrFuelAdRatesCarrFuelAdControl = " & iCarrFuelAdRatesCarrFuelAdControl.ToString() & ") "
                    sFilterSpacer = " And "
                    'Else
                    '    Dim tFilter As Models.FilterDetails = filters.FilterValues.Where(Function(x) x.filterName = "CarrFuelAdRatesControl").FirstOrDefault()
                    '    Integer.TryParse(tFilter.filterValueFrom, iCarrFuelAdRatesControl)
                End If

                If iCarrFuelAdRatesCarrFuelAdControl = 0 Then
                    'we do not have a valid filter
                    throwNoDataFaultMessage("E_InvalidRecordKey") ' "The reference to the record is missing. Please select another record and try again."
                End If
                Dim iQuery As IQueryable(Of LTS.CarrierFuelAdRate)
                iQuery = db.CarrierFuelAdRates
                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "CarrFuelAdRatesPriceFrom"
                    filters.sortDirection = "asc"
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrierFuelAdRatesByCarrier"), db)
            End Try
        End Using
        Return oRet
    End Function


    ''' <summary>
    ''' Returns the  carrier fuel rate data aassoicated with a Carrier Tariff, it looks up the Fuel Addendum using the tariff control.
    ''' A [CarrFuelAdRatesControl filter  or the  CarrFuelAdCarrTarControl value must be provided in the filters.ParentControl
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 01/25/2019
    ''' </remarks>
    Public Function GetCarrierFuelAdRatesByTariff(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.CarrierFuelAdRate()
        If filters Is Nothing Then Return Nothing

        '[CarrFuelAdRatesControl]           Int             IDENTITY (1, 1) Not NULL,
        '[CarrFuelAdRatesCarrFuelAdControl] Int             Not NULL,
        '[CarrFuelAdRatesPriceFrom]         Decimal(10, 5) Not NULL,
        '[CarrFuelAdRatesPriceTo]           Decimal(10, 5) Not NULL,
        '[CarrFuelAdRatesPerMile]           Decimal(10, 5)  NULL,
        '[CarrFuelAdRatesPercent]           Decimal(10, 5)  NULL,
        '[CarrFuelAdRatesEffDate]           DateTime        Not NULL,
        '[CarrFuelAdRatesModUser]           NVARCHAR(100)  NULL,
        '[CarrFuelAdRatesModDate]           DateTime        NULL,
        '[CarrFuelAdRatesUpdated]           ROWVERSION      NULL,

        Dim iCarrFuelAdCarrTarControl As Integer = 0
        Dim iCarrFuelAdRatesControl As Integer = 0
        Dim iCarrFuelAdRatesCarrFuelAdControl As Integer = 0
        Dim filterWhere As String = ""
        Dim sFilterSpacer As String = ""


        Dim oRet() As LTS.CarrierFuelAdRate

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                If (filters.FilterValues Is Nothing OrElse filters.FilterValues.Count() < 1) OrElse (Not filters.FilterValues.Any(Function(x) x.filterName = "CarrFuelAdRatesControl")) Then
                    'The Record Control Filter does not exist so use the parent control fliter
                    If filters.ParentControl = 0 Then
                        throwNoDataFaultMessage("E_MissingParent") ' "The reference to the parent record is missing. Please select a valid parent record and try again."
                    End If
                    iCarrFuelAdCarrTarControl = filters.ParentControl
                    'get the iCarrFuelAdExCarrFuelAdContol using the parentcontrol
                    iCarrFuelAdRatesCarrFuelAdControl = db.CarrierFuelAddendums.Where(Function(x) x.CarrFuelAdCarrTarControl = iCarrFuelAdCarrTarControl).Select(Function(x) x.CarrFuelAdControl).FirstOrDefault()
                    If iCarrFuelAdRatesCarrFuelAdControl = 0 Then
                        'we do not have a valid filter so return nothing
                        Return Nothing
                        'throwNoDataFaultMessage("E_InvalidRecordKey") ' "The reference to the record is missing. Please select another record and try again."
                    End If
                    filterWhere = " (CarrFuelAdRatesCarrFuelAdControl = " & iCarrFuelAdRatesCarrFuelAdControl.ToString() & ") "
                    sFilterSpacer = " And "
                Else
                    Dim tFilter As Models.FilterDetails = filters.FilterValues.Where(Function(x) x.filterName = "CarrFuelAdRatesControl").FirstOrDefault()
                    Integer.TryParse(tFilter.filterValueFrom, iCarrFuelAdRatesControl)
                End If

                If iCarrFuelAdRatesCarrFuelAdControl = 0 And iCarrFuelAdRatesControl = 0 Then
                    'we do not have a valid filter
                    throwNoDataFaultMessage("E_InvalidRecordKey") ' "The reference to the record is missing. Please select another record and try again."
                End If
                Dim iQuery As IQueryable(Of LTS.CarrierFuelAdRate)
                iQuery = db.CarrierFuelAdRates
                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "CarrFuelAdRatesPriceFrom"
                    filters.sortDirection = "asc"
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrierFuelAdRatesByTariff"), db)
            End Try
        End Using
        Return oRet
    End Function


    ''' <summary>
    '''  Insert or Update the carrier tariff Fees data  assoicated with a Carrier Fuel Addendum.
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 01/25/2019
    ''' </remarks>
    Public Function SaveCarrierFuelAdRate(ByVal oData As LTS.CarrierFuelAdRate, Optional ByVal iCarrFuelAdControl As Integer = 0) As Boolean
        Dim blnRet As Boolean = False
        If oData Is Nothing Then Return False 'nothing to do
        Dim CarrFuelAdRatesCarrFuelAdControl As Integer = oData.CarrFuelAdRatesCarrFuelAdControl
        Dim iCarrFuelAdRatesControl As Integer = oData.CarrFuelAdRatesControl
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                If CarrFuelAdRatesCarrFuelAdControl = 0 Then
                    CarrFuelAdRatesCarrFuelAdControl = iCarrFuelAdControl
                    oData.CarrFuelAdRatesCarrFuelAdControl = iCarrFuelAdControl
                End If

                'verify the fuel addendum exist
                If CarrFuelAdRatesCarrFuelAdControl = 0 Then
                    Dim lDetails As New List(Of String) From {"Carrier Contract Fuel Addendum", " was not provided and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                'If Not db.CarrierFuelAddendums.Any(Function(x) x.CarrFuelAdControl = CarrFuelAdRatesCarrFuelAdControl) Then
                '    Dim lDetails As New List(Of String) From {"Carrier Contract Fuel Addendum", " was not found and "}
                '    throwInvalidKeyParentRequiredException(lDetails)
                '    Return False
                'End If
                With oData
                    .CarrFuelAdRatesModDate = Date.Now
                    .CarrFuelAdRatesModUser = Me.Parameters.UserName
                End With
                If iCarrFuelAdRatesControl = 0 Then
                    oData.CarrFuelAdRatesUpdated = New Byte() {}
                    db.CarrierFuelAdRates.InsertOnSubmit(oData)
                Else
                    db.CarrierFuelAdRates.Attach(oData, True)
                End If
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveCarrierFuelAdRate"), db)
            End Try
        End Using
        Return blnRet
    End Function


    ''' <summary>
    '''  Insert or Update the carrier Fees data  assoicated with a Carrier, on insert the system looks up the Fuel Addendum using the iCarrFuelAdRatesCarrFuelAdControl provided.
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 01/25/2019
    ''' </remarks>
    Public Function SaveCarrierFuelAdRateByCarrier(ByVal oData As LTS.CarrierFuelAdRate, Optional ByVal iCarrFuelAdRatesCarrFuelAdControl As Integer = 0) As Boolean
        Dim blnRet As Boolean = False
        If oData Is Nothing Then Return False 'nothing to do
        'Dim iCarrFuelAdRatesCarrFuelAdControl As Integer = oData.CarrFuelAdRatesCarrFuelAdControl
        Dim iCarrFuelAdRatesControl As Integer = oData.CarrFuelAdRatesControl
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'verify the fuel addendum exist
                If iCarrFuelAdRatesCarrFuelAdControl <> 0 Then
                    'verify the fuel addendum exist
                    oData.CarrFuelAdRatesCarrFuelAdControl = iCarrFuelAdRatesCarrFuelAdControl
                Else
                    Dim lDetails As New List(Of String) From {"Carrier Contract Fuel Addendum", " was not provided and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                With oData
                    .CarrFuelAdRatesModDate = Date.Now
                    .CarrFuelAdRatesModUser = Me.Parameters.UserName
                End With
                If iCarrFuelAdRatesControl = 0 Then
                    oData.CarrFuelAdRatesUpdated = New Byte() {}
                    db.CarrierFuelAdRates.InsertOnSubmit(oData)
                Else
                    db.CarrierFuelAdRates.Attach(oData, True)
                End If
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveCarrierFuelAdRateByCarrier"), db)
            End Try
        End Using
        Return blnRet
    End Function


    ''' <summary>
    '''  Insert or Update the carrier tariff Fees data  assoicated with a Carrier Tariff, on insert the system looks up the Fuel Addendum using the iCarrFuelAdCarrTarControl provided.
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 01/25/2019
    ''' </remarks>
    Public Function SaveCarrierFuelAdRateByTariff(ByVal oData As LTS.CarrierFuelAdRate, Optional ByVal iCarrFuelAdCarrTarControl As Integer = 0) As Boolean
        Dim blnRet As Boolean = False
        If oData Is Nothing Then Return False 'nothing to do
        Dim iCarrFuelAdRatesCarrFuelAdControl As Integer = oData.CarrFuelAdRatesCarrFuelAdControl
        Dim iCarrFuelAdRatesControl As Integer = oData.CarrFuelAdRatesControl
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'verify the fuel addendum exist
                If iCarrFuelAdRatesCarrFuelAdControl = 0 Then
                    'look up the fuel addendum if it is missing
                    If iCarrFuelAdCarrTarControl <> 0 Then
                        iCarrFuelAdRatesCarrFuelAdControl = db.CarrierFuelAddendums.Where(Function(x) x.CarrFuelAdCarrTarControl = iCarrFuelAdCarrTarControl).Select(Function(x) x.CarrFuelAdControl).FirstOrDefault()
                        'verify the fuel addendum exist
                        If iCarrFuelAdRatesCarrFuelAdControl = 0 Then
                            Dim lDetails As New List(Of String) From {"Carrier Contract Fuel Addendum", " was not provided and "}
                            throwInvalidKeyParentRequiredException(lDetails)
                            Return False
                        End If
                        oData.CarrFuelAdRatesCarrFuelAdControl = iCarrFuelAdRatesCarrFuelAdControl
                    Else
                        Dim lDetails As New List(Of String) From {"Carrier Contract Fuel Addendum", " was not provided and "}
                        throwInvalidKeyParentRequiredException(lDetails)
                        Return False
                    End If
                ElseIf Not db.CarrierFuelAddendums.Any(Function(x) x.CarrFuelAdControl = iCarrFuelAdRatesCarrFuelAdControl) Then
                    Dim lDetails As New List(Of String) From {"Carrier Contract Fuel Addendum", " was not found and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                With oData
                    .CarrFuelAdRatesModDate = Date.Now
                    .CarrFuelAdRatesModUser = Me.Parameters.UserName
                End With
                If iCarrFuelAdRatesControl = 0 Then
                    oData.CarrFuelAdRatesUpdated = New Byte() {}
                    db.CarrierFuelAdRates.InsertOnSubmit(oData)
                Else
                    db.CarrierFuelAdRates.Attach(oData, True)
                End If
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveCarrierFuelAdRateByTariff"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Delete the provided carrier Fuel Addenum Rate
    ''' </summary>
    ''' <param name="iControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 01/25/2019
    ''' </remarks>
    Public Function DeleteCarrierFuelAdRate(ByVal iControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        If iControl = 0 Then Return False 'nothing to do
        'validate before delete

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Delete the Record
                Dim oToDelete = db.CarrierFuelAdRates.Where(Function(x) x.CarrFuelAdRatesControl = iControl).FirstOrDefault()
                If oToDelete Is Nothing OrElse oToDelete.CarrFuelAdRatesControl = 0 Then Return True 'already deleted
                db.CarrierFuelAdRates.DeleteOnSubmit(oToDelete)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteCarrierFuelAdRate"), db)
            End Try
        End Using
        Return blnRet
    End Function

    Public Function DeleteCarrierFuelAdRateRecords(ByVal iControl As String) As Boolean
        Dim strProcName As String = "dbo.spDeleteCarrierFuelAdRateRecords"
        Dim blnRet As Boolean = False
        If iControl = "" Then Return False 'nothing to do
        'validate before delete

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim oCmd As New System.Data.SqlClient.SqlCommand
                oCmd.Parameters.AddWithValue("@AdRateControl", iControl)
                runNGLStoredProcedure(oCmd, strProcName, 0)
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteCarrierFuelAdRateRecords"), db)
            End Try
        End Using
        Return blnRet
    End Function
#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DataTransferObjects.CarrierFuelAdRate)
        'Create New Record
        Return New LTS.CarrierFuelAdRate With {.CarrFuelAdRatesControl = d.CarrFuelAdRatesControl _
            , .CarrFuelAdRatesCarrFuelAdControl = d.CarrFuelAdRatesCarrFuelAdControl _
            , .CarrFuelAdRatesPriceFrom = d.CarrFuelAdRatesPriceFrom _
            , .CarrFuelAdRatesPriceTo = d.CarrFuelAdRatesPriceTo _
            , .CarrFuelAdRatesPerMile = d.CarrFuelAdRatesPerMile _
            , .CarrFuelAdRatesPercent = d.CarrFuelAdRatesPercent _
            , .CarrFuelAdRatesEffDate = d.CarrFuelAdRatesEffDate _
            , .CarrFuelAdRatesModUser = Parameters.UserName _
            , .CarrFuelAdRatesModDate = Date.Now _
            , .CarrFuelAdRatesUpdated = If(d.CarrFuelAdRatesUpdated Is Nothing, New Byte() {}, d.CarrFuelAdRatesUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCarrierFuelAdRateFiltered(Control:=CType(LinqTable, LTS.CarrierFuelAdRate).CarrFuelAdRatesControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim source As LTS.CarrierFuelAdRate = TryCast(LinqTable, LTS.CarrierFuelAdRate)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.CarrierFuelAdRates
                    Where d.CarrFuelAdRatesControl = source.CarrFuelAdRatesControl
                    Select New DataTransferObjects.QuickSaveResults With {.Control = d.CarrFuelAdRatesControl _
                        , .ModDate = d.CarrFuelAdRatesModDate _
                        , .ModUser = d.CarrFuelAdRatesModUser _
                        , .Updated = d.CarrFuelAdRatesUpdated.ToArray}).First

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

    Friend Function selectDTOData(ByVal t As LTS.CarrierFuelAdRate, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.CarrierFuelAdRate
        Return New DataTransferObjects.CarrierFuelAdRate With {.CarrFuelAdRatesControl = t.CarrFuelAdRatesControl _
            , .CarrFuelAdRatesCarrFuelAdControl = t.CarrFuelAdRatesCarrFuelAdControl _
            , .CarrFuelAdRatesPriceFrom = t.CarrFuelAdRatesPriceFrom _
            , .CarrFuelAdRatesPriceTo = t.CarrFuelAdRatesPriceTo _
            , .CarrFuelAdRatesPerMile = If(t.CarrFuelAdRatesPerMile.HasValue, t.CarrFuelAdRatesPerMile.Value, 0) _
            , .CarrFuelAdRatesPercent = If(t.CarrFuelAdRatesPercent.HasValue, t.CarrFuelAdRatesPercent.Value, 0) _
            , .CarrFuelAdRatesEffDate = t.CarrFuelAdRatesEffDate _
            , .CarrFuelAdRatesModUser = t.CarrFuelAdRatesModUser _
            , .CarrFuelAdRatesModDate = t.CarrFuelAdRatesModDate _
            , .CarrFuelAdRatesUpdated = t.CarrFuelAdRatesUpdated.ToArray(),
            .Page = page,
            .Pages = pagecount,
            .RecordCount = recordcount,
            .PageSize = pagesize}
    End Function

#End Region

End Class