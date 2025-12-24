Imports System.Data.Linq
Imports System.Linq.Dynamic
Imports System.ServiceModel
Imports NGL.Core.ChangeTracker
Imports Ngl.FreightMaster.Data.DataTransferObjects

Public Class NGLCarrTarEquipMatData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMASCarrierDataContext(ConnectionString)
        Me.LinqTable = db.CarrierTariffEquipMatrixes
        Me.LinqDB = db
        Me.SourceClass = "NGLCarrTarEquipMatData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASCarrierDataContext(ConnectionString)
                _LinqTable = db.CarrierTariffEquipMatrixes
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
        Return GetCarrTarEquipMatFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    Public Function GetCarrTarEquipMatFiltered(ByVal Control As Integer) As DataTransferObjects.CarrTarEquipMat
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                Dim oDLO As New DataLoadOptions
                oDLO.LoadWith(Of LTS.CarrierTariffEquipMatrix)(Function(t As LTS.CarrierTariffEquipMatrix) t.CarrierTariffEquipMatrixDetails)
                db.LoadOptions = oDLO

                'Get the newest record that matches the provided criteria
                Dim CarrierTariffEquipMatrixContract As DataTransferObjects.CarrTarEquipMat = (
                        From d In db.CarrierTariffEquipMatrixes
                        Where
                        d.CarrTarEquipMatControl = Control
                        Select selectDTOData(d, db)).First

                Return CarrierTariffEquipMatrixContract

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException()
            Catch ex As Exception
                throwUnExpectedFaultException(ex, getSourceCaller("GetCarrTarEquipMatFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetCarrTarEquipMatsFiltered(ByVal CarrTarEquipControl As Integer,
                                                Optional ByVal page As Integer = 1,
                                                Optional ByVal pagesize As Integer = 1000) As DataTransferObjects.CarrTarEquipMat()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1

                'db.Log = New DebugTextWriter

                Dim oRecords = (
                        From d In db.CarrierTariffEquipMatrixes
                        Where
                        (d.CarrTarEquipMatCarrTarEquipControl = CarrTarEquipControl)
                        Select d.CarrTarEquipMatControl).ToArray()

                If oRecords Is Nothing Then Return Nothing

                intRecordCount = oRecords.Count
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize

                Dim oDLO As New DataLoadOptions
                oDLO.LoadWith(Of LTS.CarrierTariffEquipMatrix)(Function(t As LTS.CarrierTariffEquipMatrix) t.CarrierTariffEquipMatrixDetails)
                db.LoadOptions = oDLO

                Dim CarrierTariffEquipMatrixes() As DataTransferObjects.CarrTarEquipMat = (
                        From d In db.CarrierTariffEquipMatrixes
                        Where oRecords.Contains(d.CarrTarEquipMatControl)
                        Order By d.CarrTarEquipMatControl
                        Select selectDTOData(d, db, page, intPageCount, intRecordCount, pagesize)).Skip(intSkip).Take(pagesize).ToArray()

                Return CarrierTariffEquipMatrixes

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException()
            Catch ex As Exception
                throwUnExpectedFaultException(ex, getSourceCaller("GetCarrTarEquipMatsFiltered"))
            End Try

            Return Nothing

        End Using
    End Function


    Public Function GetCarrTarEquipMatLTLTestRecord() As DataTransferObjects.CarrTarEquipMat
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                Return (From d In db.CarrierTariffEquipMatrixes
                    Where d.CarrTarEquipMatTarRateTypeControl = 3
                    Order By d.CarrTarEquipMatControl
                    Select selectDTOData(d, db)).FirstOrDefault()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrTarEquipMatLTLTestRecord"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Get the Equipment Matrix Details for Import/Export tariff logic
    ''' </summary>
    ''' <param name="CarrTarEquipControl"></param>
    ''' <param name="carrtarcontrol"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.3.0.001 on 08/19/2020
    '''     fixed bug where import could fail if the CarrTarEquipMatName is null or empty
    ''' </remarks>
    Public Function GetCarrTarEquipMatRateNodes(ByVal CarrTarEquipControl As Integer, ByVal carrtarcontrol As Integer) As List(Of CarrTarEquipMatNode)
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'first select the distinct records
                'then select the fields.
                Dim oRecords As List(Of CarrTarEquipMatNode) = (
                        From d In db.CarrierTariffEquipMatrixes
                        Where (d.CarrTarEquipMatCarrTarEquipControl = CarrTarEquipControl)
                        Order By d.CarrTarEquipMatName
                        Select New DataTransferObjects.CarrTarEquipMatNode _
                        With {.CarrTarEquipMatName = d.CarrTarEquipMatName,
                        .CarrTarEquipMatCarrTarEquipControl = d.CarrTarEquipMatCarrTarEquipControl,
                        .CarrTarEquipMatTarRateTypeControl = d.CarrTarEquipMatTarRateTypeControl,
                        .CarrTarEquipMatTarBracketTypeControl = d.CarrTarEquipMatTarBracketTypeControl,
                        .CarrTarEquipMatClassTypeControl = d.CarrTarEquipMatClassTypeControl}).Distinct().ToList()

                For Each item As DataTransferObjects.CarrTarEquipMatNode In oRecords
                    item.CarrTarEquipMatCarrTarControl = carrtarcontrol
                    ' Modified by RHR for v-8.3.0.001 on 08/19/2020
                    If String.IsNullOrWhiteSpace(item.CarrTarEquipMatName) Then item.CarrTarEquipMatName = "Undefined"
                Next

                Return oRecords

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                Return Nothing
            Catch ex As Exception
                throwUnExpectedFaultException(ex, getSourceCaller("GetCarrTarEquipMatRateNodes"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetCarrTarEquipMatNodes(ByVal CarrTarEquipControl As Integer, ByVal ParentTreeID As Integer) As List(Of NGLTreeNode)
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim CarrTarEquipMatrixNodes As New List(Of NGLTreeNode)
                'db.Log = New DebugTextWriter
                Dim oRecords As List(Of CarrTarEquipMat) = (
                        From d In db.CarrierTariffEquipMatrixes
                        Where (d.CarrTarEquipMatCarrTarEquipControl = CarrTarEquipControl)
                        Order By d.CarrTarEquipMatName
                        Select New DataTransferObjects.CarrTarEquipMat _
                        With {.CarrTarEquipMatName = d.CarrTarEquipMatName,
                        .CarrTarEquipMatCarrTarEquipControl = d.CarrTarEquipMatCarrTarEquipControl,
                        .CarrTarEquipMatTarRateTypeControl = d.CarrTarEquipMatTarRateTypeControl,
                        .CarrTarEquipMatTarBracketTypeControl = d.CarrTarEquipMatTarBracketTypeControl,
                        .CarrTarEquipMatClassTypeControl = d.CarrTarEquipMatClassTypeControl}).Distinct().ToList()

                If Not oRecords Is Nothing AndAlso oRecords.Count > 0 Then
                    CarrTarEquipMatrixNodes = (
                        From d In oRecords
                            Select New DataTransferObjects.NGLTreeNode With {.Control = 0,
                                .ParentTreeID = ParentTreeID,
                                .Name = d.CarrTarEquipMatName,
                                .Description = String.Format("Key: Equip {0}, RateType {1}, BracketType {2}, ClassType {3}", d.CarrTarEquipMatName, d.CarrTarEquipMatTarRateTypeControl, d.CarrTarEquipMatTarBracketTypeControl, d.CarrTarEquipMatClassTypeControl),
                                .AltDataKey = String.Format("{0}-{1}-{2}-{3}", d.CarrTarEquipMatCarrTarEquipControl, d.CarrTarEquipMatTarRateTypeControl, d.CarrTarEquipMatTarBracketTypeControl, d.CarrTarEquipMatClassTypeControl),
                                .ClassName = "CarrTarEquipRates"}).ToList()
                End If

                Return CarrTarEquipMatrixNodes


            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                Return Nothing
            Catch ex As Exception
                throwUnExpectedFaultException(ex, getSourceCaller("GetCarrTarEquipMatNodes"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetCarrTarEquipMatPivot(ByVal Control As Integer) As DataTransferObjects.CarrTarEquipMatPivot
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                'db.Log = New DebugTextWriter

                Dim oRecord = (
                        From d In db.vCarrTarEquipMatPivots
                        Where d.CarrTarEquipMatControl = Control
                        Select d.CarrTarEquipMatControl, d.CarrTarEquipMatCarrTarMatBPControl).First()

                If oRecord Is Nothing Then Return Nothing

                'The breakpoint piviot is the same for each matrix record in this result set so we only need to read it once.
                Dim BPPivot As DataTransferObjects.CarrTarMatBPPivot = New NGLCarrTarMatBPData(Me.Parameters).GetCarrTarMatBPPivot(oRecord.CarrTarEquipMatCarrTarMatBPControl)

                Dim CarrierTariffEquipMatPivot As DataTransferObjects.CarrTarEquipMatPivot = (
                        From d In db.vCarrTarEquipMatPivots
                        Where d.CarrTarEquipMatControl = Control
                        Select selectDTOCarrTarEquipMatPivotData(BPPivot, d, db)).First()

                Return CarrierTariffEquipMatPivot

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException()
            Catch ex As Exception
                throwUnExpectedFaultException(ex, getSourceCaller("GetCarrTarEquipMatPivot"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetTariffRates(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vTariffRate()
        '[CarrTarEquipMatCarrTarEquipControl]
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vTariffRate
        If Not filters.FilterValues.Any(Function(x) x.filterName = "CarrTarEquipMatCarrTarEquipControl" And x.filterValueFrom > 0) Then
            Return oRet
        End If
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.vTariffRate)
                iQuery = db.vTariffRates
                Dim filterWhere As String = ""
                Dim sFilterSpacer As String = ""
                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    Dim sorting = New List(Of Models.SortDetails)
                    sorting.Add(New Models.SortDetails() With {.sortName = "CarrTarEquipMatCountry", .sortDirection = "asc"})
                    sorting.Add(New Models.SortDetails() With {.sortName = "CarrTarEquipMatState", .sortDirection = "asc"})
                    sorting.Add(New Models.SortDetails() With {.sortName = "CarrTarEquipMatCity", .sortDirection = "asc"})
                    sorting.Add(New Models.SortDetails() With {.sortName = "CarrTarEquipMatFromZip", .sortDirection = "asc"})
                    filters.SortValues = sorting.ToArray()
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetTariffRates"))
            End Try
        End Using
        Return oRet
    End Function
    Public Function GetCarrTarEquipMatPivotsByAltKeyOld(ByVal AltDataKey As String,
                                                        Optional ByVal page As Integer = 1,
                                                        Optional ByVal pagesize As Integer = 1000,
                                                        Optional ByVal filterWhere As String = "") As DataTransferObjects.CarrTarEquipMatPivot()

        If String.IsNullOrEmpty(AltDataKey) Then Return Nothing

        Dim CarrTarEquipControl As Integer
        Dim TarRateTypeControl As Integer
        Dim ClassTypeControl As Integer
        Dim TarBracketTypeControl As Integer
        'We need to split the key data and pass it to the pivot filter

        'AltDataKey = String.Format("{0}-{1}-{2}-{3}", d.CarrTarEquipMatCarrTarEquipControl, d.CarrTarEquipMatTarRateTypeControl, d.CarrTarEquipMatTarBracketTypeControl, d.CarrTarEquipMatClassTypeControl),
        Dim sFs = AltDataKey.Split("-")
        If Not ValidateEqupMatPivotKeyFields(sFs, CarrTarEquipControl, TarRateTypeControl, ClassTypeControl, TarBracketTypeControl) Then Return Nothing

        Return GetCarrTarEquipMatPivotsOld(CarrTarEquipControl,
                                           TarRateTypeControl,
                                           ClassTypeControl,
                                           TarBracketTypeControl,
                                           page,
                                           pagesize,
                                           filterWhere)

    End Function




    Public Function GetCarrTarEquipMatPivotsByAltKey(ByVal AltDataKey As String,
                                                     Optional ByVal page As Integer = 1,
                                                     Optional ByVal pagesize As Integer = 1000,
                                                     Optional ByVal filterWhere As String = "") As DataTransferObjects.CarrTarEquipMatPivot()

        If String.IsNullOrEmpty(AltDataKey) Then Return Nothing

        Dim CarrTarEquipControl As Integer
        Dim TarRateTypeControl As Integer
        Dim ClassTypeControl As Integer
        Dim TarBracketTypeControl As Integer
        'We need to split the key data and pass it to the pivot filter

        'AltDataKey = String.Format("{0}-{1}-{2}-{3}", d.CarrTarEquipMatCarrTarEquipControl, d.CarrTarEquipMatTarRateTypeControl, d.CarrTarEquipMatTarBracketTypeControl, d.CarrTarEquipMatClassTypeControl),
        Dim sFs = AltDataKey.Split("-")
        If Not ValidateEqupMatPivotKeyFields(sFs, CarrTarEquipControl, TarRateTypeControl, ClassTypeControl, TarBracketTypeControl) Then Return Nothing

        Return GetCarrTarEquipMatPivots(CarrTarEquipControl,
                                        TarRateTypeControl,
                                        ClassTypeControl,
                                        TarBracketTypeControl,
                                        page,
                                        pagesize,
                                        filterWhere)

    End Function

    Public Function GetCarrTarEquipMatPivots(ByVal node As DataTransferObjects.NGLTreeNode,
                                             Optional ByVal page As Integer = 1,
                                             Optional ByVal pagesize As Integer = 1000) As DataTransferObjects.CarrTarEquipMatPivot()

        If node Is Nothing Then Return Nothing

        Dim CarrTarEquipControl As Integer
        Dim TarRateTypeControl As Integer
        Dim ClassTypeControl As Integer
        Dim TarBracketTypeControl As Integer
        'We need to split the key data and pass it to the pivot filter

        'AltDataKey = String.Format("{0}-{1}-{2}-{3}", d.CarrTarEquipMatCarrTarEquipControl, d.CarrTarEquipMatTarRateTypeControl, d.CarrTarEquipMatTarBracketTypeControl, d.CarrTarEquipMatClassTypeControl),
        Dim sFs = node.AltDataKey.Split("-")
        If Not ValidateEqupMatPivotKeyFields(sFs, CarrTarEquipControl, TarRateTypeControl, ClassTypeControl, TarBracketTypeControl) Then Return Nothing

        Return GetCarrTarEquipMatPivots(CarrTarEquipControl,
                                        TarRateTypeControl,
                                        ClassTypeControl,
                                        TarBracketTypeControl,
                                        page,
                                        pagesize)

    End Function

    Public Function GetCarrTarEquipMatPivotsOld(ByVal CarrTarEquipControl As Integer,
                                                ByVal TarRateTypeControl As Integer,
                                                ByVal ClassTypeControl As Integer,
                                                ByVal TarBracketTypeControl As Integer,
                                                Optional ByVal page As Integer = 1,
                                                Optional ByVal pagesize As Integer = 1000,
                                                Optional ByVal filterWhere As String = "") As DataTransferObjects.CarrTarEquipMatPivot()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1

                'db.Log = New DebugTextWriter

                Dim oRecords = From d In db.vCarrTarEquipMatPivots
                        Where
                        (d.CarrTarEquipMatCarrTarEquipControl = CarrTarEquipControl) _
                        And
                        d.CarrTarEquipMatTarRateTypeControl = TarRateTypeControl _
                        And
                        d.CarrTarEquipMatClassTypeControl = ClassTypeControl _
                        And
                        d.CarrTarEquipMatTarBracketTypeControl = TarBracketTypeControl
                        Select d

                If Not String.IsNullOrEmpty(filterWhere) Then
                    oRecords = DLinqUtil.filterWhere(oRecords, filterWhere)
                End If

                intRecordCount = oRecords.Count
                If intRecordCount < 1 Then Return Nothing
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize

                Dim CarrTarEquipMatCarrTarMatBPControl As Integer = oRecords.Select(Function(x) x.CarrTarEquipMatCarrTarMatBPControl).FirstOrDefault()

                'The breakpoint piviot is the same for each matrix record in this result set so we only need to read it once.
                Dim BPPivot As DataTransferObjects.CarrTarMatBPPivot = New NGLCarrTarMatBPData(Me.Parameters).GetCarrTarMatBPPivot(CarrTarEquipMatCarrTarMatBPControl)

                'Dim oFilterd = oRecords.Select(Function(x) x.CarrTarEquipMatControl)

                Dim CarrierTariffEquipMatrixePivot() As DataTransferObjects.CarrTarEquipMatPivot = (
                        From d In oRecords
                        Order By
                        d.CarrTarEquipMatCarrTarControl,
                        d.CarrTarEquipMatCarrTarEquipControl,
                        d.CarrTarEquipMatTarRateTypeControl,
                        d.CarrTarEquipMatTarBracketTypeControl,
                        d.CarrTarEquipMatClassTypeControl,
                        d.CarrTarEquipMatClass,
                        d.CarrTarEquipMatCountry,
                        d.CarrTarEquipMatState,
                        d.CarrTarEquipMatCity,
                        d.CarrTarEquipMatFromZip,
                        d.CarrTarEquipMatToZip,
                        d.CarrTarEquipMatLaneControl
                        Select selectDTOCarrTarEquipMatPivotData(BPPivot, d, db, page, intPageCount, intRecordCount, pagesize)).Skip(intSkip).Take(pagesize).ToArray()

                Return CarrierTariffEquipMatrixePivot

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException()
            Catch ex As Exception
                throwUnExpectedFaultException(ex, getSourceCaller("GetCarrTarEquipMatPivots"))
            End Try

            Return Nothing

        End Using
    End Function


    Public Function GetCarrTarEquipMatPivots(ByVal CarrTarEquipControl As Integer,
                                             ByVal TarRateTypeControl As Integer,
                                             ByVal ClassTypeControl As Integer,
                                             ByVal TarBracketTypeControl As Integer,
                                             Optional ByVal page As Integer = 1,
                                             Optional ByVal pagesize As Integer = 1000,
                                             Optional ByVal filterWhere As String = "") As DataTransferObjects.CarrTarEquipMatPivot()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1
                Dim CarrTarEquipMatCarrTarMatBPControl As Integer
                Dim CarrierTariffEquipMatrixePivot() As DataTransferObjects.CarrTarEquipMatPivot
                Dim BPPivot As DataTransferObjects.CarrTarMatBPPivot
                'db.Log = New DebugTextWriter
                If String.IsNullOrWhiteSpace(filterWhere) Then

                    Dim oPage = db.spGetCarrTarEquipMatPivotPage(CarrTarEquipControl, TarRateTypeControl, ClassTypeControl, TarBracketTypeControl, page, pagesize).ToArray()
                    If oPage Is Nothing OrElse oPage.Length = 0 Then
                        Return Nothing
                    End If
                    CarrTarEquipMatCarrTarMatBPControl = oPage(0).CarrTarEquipMatCarrTarMatBPControl


                    BPPivot = New NGLCarrTarMatBPData(Me.Parameters).GetCarrTarMatBPPivot(CarrTarEquipMatCarrTarMatBPControl)


                    CarrierTariffEquipMatrixePivot = (
                        From d In oPage
                            Select selectDTOCarrTarEquipMatPivotData(BPPivot, d)).ToArray()

                Else

                    Dim oRecords = From d In db.vCarrTarEquipMatPivots
                            Where
                            (d.CarrTarEquipMatCarrTarEquipControl = CarrTarEquipControl) _
                            And
                            d.CarrTarEquipMatTarRateTypeControl = TarRateTypeControl _
                            And
                            d.CarrTarEquipMatClassTypeControl = ClassTypeControl _
                            And
                            d.CarrTarEquipMatTarBracketTypeControl = TarBracketTypeControl
                            Select d

                    oRecords = DLinqUtil.filterWhere(oRecords, filterWhere)


                    intRecordCount = oRecords.Count
                    If intRecordCount < 1 Then Return Nothing
                    If pagesize < 1 Then pagesize = 1
                    If intRecordCount < 1 Then intRecordCount = 1
                    If page < 1 Then page = 1
                    If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                    Dim intSkip As Integer = (page - 1) * pagesize

                    CarrTarEquipMatCarrTarMatBPControl = oRecords.Select(Function(x) x.CarrTarEquipMatCarrTarMatBPControl).FirstOrDefault()

                    BPPivot = New NGLCarrTarMatBPData(Me.Parameters).GetCarrTarMatBPPivot(CarrTarEquipMatCarrTarMatBPControl)

                    CarrierTariffEquipMatrixePivot = (
                        From d In oRecords
                            Order By
                                d.CarrTarEquipMatCarrTarControl,
                                d.CarrTarEquipMatCarrTarEquipControl,
                                d.CarrTarEquipMatTarRateTypeControl,
                                d.CarrTarEquipMatTarBracketTypeControl,
                                d.CarrTarEquipMatClassTypeControl,
                                d.CarrTarEquipMatClass,
                                d.CarrTarEquipMatCountry,
                                d.CarrTarEquipMatState,
                                d.CarrTarEquipMatCity,
                                d.CarrTarEquipMatFromZip,
                                d.CarrTarEquipMatToZip,
                                d.CarrTarEquipMatLaneControl
                            Select selectDTOCarrTarEquipMatPivotData(BPPivot, d, db, page, intPageCount, intRecordCount, pagesize)).Skip(intSkip).Take(pagesize).ToArray()

                End If


                'The breakpoint piviot is the same for each matrix record in this result set so we only need to read it once.

                Return CarrierTariffEquipMatrixePivot

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException()
            Catch ex As Exception
                throwUnExpectedFaultException(ex, getSourceCaller("GetCarrTarEquipMatPivots"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Returns the Rate Type for this contract: 1 - Distance, 3	- Class, 4 - Flat, 5 - Unit of Measure.  
    ''' The Rate Type is used to select the correct layout for the Tariff Rates Page.
    ''' 0 indicates a matrix does not exist so we must add one
    ''' </summary>
    ''' <param name="CarrTarControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 09/06/2018
    ''' </remarks>
    Public Function CarrTarEquipMatTarRateType(ByVal CarrTarControl As Integer) As Integer
        Dim iRet As Integer = 0 'indicates a matrix does not exist so we must add one
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                iRet = db.CarrierTariffEquipMatrixes.Where(Function(x) x.CarrTarEquipMatCarrTarControl = CarrTarControl).Select(Function(x) x.CarrTarEquipMatTarRateTypeControl).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, getSourceCaller("CarrTarEquipMatTarRateType"), db)
            End Try

        End Using
        Return iRet
    End Function

    ''' <summary>
    ''' Returns the first vCarrTarEquipMatPivotTruckLoad record for the active contract
    ''' This method requires a pk setting for the tariff page (39) for the active user in the parameter settings
    ''' if usercontrol is zero an empty record is returned
    ''' </summary>
    ''' <returns>LTS.vCarrTarEquipMatPivotTruckLoad</returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 09/06/2018
    '''     Modified by RHR for v-8.5.4.001 on 06/15/2023
    '''         Changed Return value to LTS.vCarrTarEquipMatPivotTruckLoad so we can process Origin Rate shop requests
    ''' </remarks>
    Public Function GetCarrTarEquipMatTarRateTypeForActiveTariff() As LTS.vCarrTarEquipMatPivotTruckLoad
        Dim oRet As New LTS.vCarrTarEquipMatPivotTruckLoad()
        If Parameters.UserControl = 0 Then Return oRet
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim sDaL As New NGLUserPageSettingData(Parameters)
                'read the primary key from page 39 (tariff)
                Dim sSettings = sDaL.GetPageSettingsForCurrentUser(39, "pk")
                If Not sSettings Is Nothing AndAlso sSettings.Count() > 0 Then
                    Dim sCarrTarControl = sSettings(0).UserPSMetaData
                    Dim iCarrTarControl As Integer = 0
                    Integer.TryParse(sCarrTarControl, iCarrTarControl)
                    If iCarrTarControl > 0 Then
                        oRet = db.vCarrTarEquipMatPivotTruckLoads.Where(Function(x) x.CarrTarEquipMatCarrTarControl = iCarrTarControl).FirstOrDefault()
                    End If
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, getSourceCaller("GetCarrTarEquipMatTarRateTypeForActiveTariff"), db)
            End Try

        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' Returns the rates, displayed in Distance Format, assoicated with a Contract the CarrTarControl value which must be provided in the filters.ParentControl parameter
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 09/06/2018
    '''   new LTS carrier tariff matrix query  
    '''   for now Flat duplicates Distance but this may change in the future 
    ''' </remarks>
    Public Function GetCarrTarEquipMatDistanceRates(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vCarrTarEquipMatPivotTruckLoad()
        If filters Is Nothing Then Return Nothing
        If filters.ParentControl = 0 Then
            Dim sMsg As String = " The Tariff ID is missing. Please return to the tariff contract page and select a valid carrier contract."
            throwNoDataFaultException(sMsg)
        End If
        Dim oRet() As LTS.vCarrTarEquipMatPivotTruckLoad
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.vCarrTarEquipMatPivotTruckLoad)
                iQuery = db.vCarrTarEquipMatPivotTruckLoads
                Dim filterWhere As String = " (CarrTarEquipMatCarrTarControl = " & filters.ParentControl & ") "
                Dim sFilterSpacer As String = " And "

                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "CarrTarEquipMatName"
                    filters.sortDirection = "asc"
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                'db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, getSourceCaller("GetCarrTarEquipMatDistanceRates"), db)
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' Returns the rates, displayed in Flat Format, assoicated with a Contract the CarrTarControl value which must be provided in the filters.ParentControl parameter
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 09/06/2018
    '''   new LTS carrier tariff matrix query  
    '''   for now Flat duplicates Distance but this may change in the future
    ''' </remarks>
    Public Function GetCarrTarEquipMatFlatRates(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vCarrTarEquipMatPivotTruckLoad()
        If filters Is Nothing Then Return Nothing
        If filters.ParentControl = 0 Then
            Dim sMsg As String = " The Tariff ID is missing. Please return to the tariff contract page and select a valid carrier contract."
            throwNoDataFaultException(sMsg)
        End If
        Dim oRet() As LTS.vCarrTarEquipMatPivotTruckLoad
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.vCarrTarEquipMatPivotTruckLoad)
                iQuery = db.vCarrTarEquipMatPivotTruckLoads
                Dim filterWhere As String = " (CarrTarEquipMatCarrTarControl = " & filters.ParentControl & ") "
                Dim sFilterSpacer As String = " And "

                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "CarrTarEquipMatName"
                    filters.sortDirection = "asc"
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                'db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, getSourceCaller("GetCarrTarEquipMatFlatRates"), db)
            End Try
        End Using
        Return oRet
    End Function


    ''' <summary>
    ''' Returns the rates, displayed in Class Format, assoicated with a Contract the CarrTarControl value which must be provided in the filters.ParentControl parameter
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 09/06/2018
    '''   new LTS carrier tariff matrix query  
    '''   for now Class duplicates UOM but this may change in the future
    ''' </remarks>
    Public Function GetCarrTarEquipMatClassRates(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vCarrTarEquipMatPivotDetail()
        If filters Is Nothing Then Return Nothing
        If filters.ParentControl = 0 Then
            Dim sMsg As String = " The Tariff ID is missing. Please return to the tariff contract page and select a valid carrier contract."
            throwNoDataFaultException(sMsg)
        End If
        Dim oRet() As LTS.vCarrTarEquipMatPivotDetail
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.vCarrTarEquipMatPivotDetail)
                iQuery = db.vCarrTarEquipMatPivotDetails
                Dim filterWhere As String = " (CarrTarEquipMatCarrTarControl = " & filters.ParentControl & ") "
                Dim sFilterSpacer As String = " And "

                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "CarrTarEquipMatName"
                    filters.sortDirection = "asc"
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                'db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, getSourceCaller("GetCarrTarEquipMatClassRates"), db)
            End Try
        End Using
        Return oRet
    End Function


    ''' <summary>
    ''' Returns the rates, displayed in Unit of Measure Format, assoicated with a Contract the CarrTarControl value which must be provided in the filters.ParentControl parameter
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 09/06/2018
    '''   new LTS carrier tariff matrix query  
    '''   for now Class duplicates UOM but this may change in the future
    ''' </remarks>
    Public Function GetCarrTarEquipMatUOMRates(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vCarrTarEquipMatPivotDetail()
        If filters Is Nothing Then Return Nothing
        If filters.ParentControl = 0 Then
            Dim sMsg As String = " The Tariff ID is missing. Please return to the tariff contract page and select a valid carrier contract."
            throwNoDataFaultException(sMsg)
        End If
        Dim oRet() As LTS.vCarrTarEquipMatPivotDetail
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.vCarrTarEquipMatPivotDetail)
                iQuery = db.vCarrTarEquipMatPivotDetails
                Dim filterWhere As String = " (CarrTarEquipMatCarrTarControl = " & filters.ParentControl & ") "
                Dim sFilterSpacer As String = " And "

                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "CarrTarEquipMatName"
                    filters.sortDirection = "asc"
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                'db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, getSourceCaller("GetCarrTarEquipMatUOMRates"), db)
            End Try
        End Using
        Return oRet
    End Function


    ''' <summary>
    ''' Returns the Break Point header record associated with the selected Matrix 
    ''' </summary>
    ''' <param name="BPControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 09/06/2018
    '''   new LTS carrier tariff Break Point Headers
    '''   looks up record using CarrTarMatBPDetCarrTarMatBPControl
    ''' </remarks>
    Public Function GetCarrTarEquipMatBreakPoint(ByVal BPControl As Integer) As LTS.vCarrTarEquipMatBreakPoint
        Dim oRet As LTS.vCarrTarEquipMatBreakPoint
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                oRet = db.vCarrTarEquipMatBreakPoints.Where(Function(x) x.CarrTarMatBPDetCarrTarMatBPControl = BPControl).FirstOrDefault()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, getSourceCaller("GetCarrTarEquipMatBreakPont"), db)
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' Save the distance rate data
    ''' </summary>
    ''' <param name="rate"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 09/06/2018
    '''   new LTS carrier tariff matrix view  
    '''   for now Flat duplicates Distance but this may change in the future 
    ''' Modified by RHR for v-8.3.0.001 on 09/21/2020
    '''     added logic to insert a default rate name when blank
    ''' </remarks>
    Public Function SaveCarrTarEquipMatDistanceRate(ByVal rate As LTS.vCarrTarEquipMatPivotTruckLoad) As Boolean
        Dim blnRet As Boolean = False
        If rate Is Nothing Then Return False 'nothing to do
        Dim oNew As New LTS.CarrierTariffEquipMatrix()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Modified by RHR for v-8.3.0.001 on 09/21/2020
                If String.IsNullOrWhiteSpace(rate.CarrTarEquipMatName) Then rate.CarrTarEquipMatName = "Rates"
                Dim blnAddingNewMatrix As Boolean = False
                'verify rate contract
                If rate.CarrTarEquipMatCarrTarControl = 0 Then
                    Dim lDetails As New List(Of String) From {"Carrier Contract Reference", " was not provided and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                If Not db.CarrierTariffs.Any(Function(x) x.CarrTarControl = rate.CarrTarEquipMatCarrTarControl) Then
                    Dim lDetails As New List(Of String) From {"Carrier Contract Reference", " was not found and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                'verify rate service/equipment
                If rate.CarrTarEquipMatCarrTarEquipControl = 0 Then
                    Dim lDetails As New List(Of String) From {"Carrier Contract Service Reference", " was not provided and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                If Not db.CarrierTariffEquipments.Any(Function(x) x.CarrTarEquipControl = rate.CarrTarEquipMatCarrTarEquipControl) Then
                    Dim lDetails As New List(Of String) From {"Carrier Contract Service Reference", " was not found and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                'For Distance Rates we do not need to verify rate break point 
                'If rate.CarrTarEquipMatCarrTarMatBPControl = 0 Then
                '    Dim lDetails As New List(Of String) From {"Carrier Contract Break Point Reference", " was not provided and "}
                '    throwInvalidKeyParentRequiredException(lDetails)
                '    Return False
                'End If
                'If Not db.CarrierTariffMatrixBPs.Any(Function(x) x.CarrTarMatBPControl = rate.CarrTarEquipMatCarrTarMatBPControl) Then
                '    Dim lDetails As New List(Of String) From {"Carrier Contract Break Point Reference", " was not found and "}
                '    throwInvalidKeyParentRequiredException(lDetails)
                '    Return False
                'End If
                'check for adding new
                If rate.CarrTarEquipMatControl = 0 Then
                    blnAddingNewMatrix = True
                    Dim strMSG As String = ""
                    Dim skipObjs As New List(Of String) From {"Rate", "CarrTarEquipMatModUser", "CarrTarEquipMatModDate", "CarrTarEquipMatUpdated", "CarrierTariffEquipMatrixDetails", "CarrierTariffEquipment", "LaneNumber"}
                    oNew = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(oNew, rate, skipObjs, strMSG)
                    With oNew
                        .CarrTarEquipMatModDate = Date.Now
                        .CarrTarEquipMatModUser = Me.Parameters.UserName
                    End With
                    If Debugger.IsAttached AndAlso Not String.IsNullOrWhiteSpace(strMSG) Then
                        System.Diagnostics.Debug.WriteLine(strMSG)
                    End If
                    db.CarrierTariffEquipMatrixes.InsertOnSubmit(oNew)
                Else
                    'update the existing matrix
                    Dim oExisting As LTS.CarrierTariffEquipMatrix = db.CarrierTariffEquipMatrixes.Where(Function(x) x.CarrTarEquipMatControl = rate.CarrTarEquipMatControl).FirstOrDefault()
                    If oExisting Is Nothing OrElse oExisting.CarrTarEquipMatControl = 0 Then
                        throwRecordDeletedFaultException("Cannot save changes for tariff distance rate: " & rate.CarrTarEquipMatName)
                    End If
                    Dim strMSG As String = ""
                    Dim skipObjs As New List(Of String) From {"Rate", "CarrTarEquipMatModUser", "CarrTarEquipMatModDate", "CarrTarEquipMatUpdated", "CarrierTariffEquipMatrixDetails", "CarrierTariffEquipment", "LaneNumber"}
                    oExisting = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(oExisting, rate, skipObjs, strMSG)
                    With oExisting
                        .CarrTarEquipMatModDate = Date.Now
                        .CarrTarEquipMatModUser = Me.Parameters.UserName
                    End With
                    'now update the Matrix Details for Distance Rates we only have one
                    SaveCarrTarEquipMatDet(oExisting.CarrTarEquipMatControl, rate.Rate, 1, db)
                End If
                If Not String.IsNullOrWhiteSpace(rate.CarrTarEquipMatFromZip) AndAlso rate.CarrTarEquipMatFromZip.Trim.Length = 4 Then
                    rate.CarrTarEquipMatFromZip = "0" + rate.CarrTarEquipMatFromZip
                End If

                If Not String.IsNullOrWhiteSpace(rate.CarrTarEquipMatToZip) AndAlso rate.CarrTarEquipMatToZip.Trim.Length = 4 Then
                    rate.CarrTarEquipMatToZip = "0" + rate.CarrTarEquipMatToZip
                End If
                ' Modified by RHR for v-8.5.4.001 on 06/14/2023 new Origin Rate shop logic
                If Not String.IsNullOrWhiteSpace(rate.CarrTarEquipMatOrigZip) AndAlso rate.CarrTarEquipMatOrigZip.Trim.Length = 4 Then
                    rate.CarrTarEquipMatOrigZip = "0" + rate.CarrTarEquipMatOrigZip
                End If

                db.SubmitChanges()
                If blnAddingNewMatrix Then
                    'we need to add the new matrix detail record
                    SaveCarrTarEquipMatDet(oNew.CarrTarEquipMatControl, rate.Rate, 1, db, True)
                    'we need to save the matrix details
                    db.SubmitChanges()
                End If
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, getSourceCaller("SaveCarrTarEquipMatDistanceRate"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Save the flat rate data
    ''' </summary>
    ''' <param name="rate"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 09/06/2018
    '''   new LTS carrier tariff matrix view  
    '''   for now Flat duplicates Distance but this may change in the future 
    ''' Modified by RHR for v-8.3.0.001 on 09/21/2020
    '''     added logic to insert a default rate name when blank
    ''' </remarks>
    Public Function SaveCarrTarEquipMatFlatRate(ByVal rate As LTS.vCarrTarEquipMatPivotTruckLoad) As Boolean
        Dim blnRet As Boolean = False
        If rate Is Nothing Then Return False 'nothing to do
        Dim oNew As New LTS.CarrierTariffEquipMatrix()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Modified by RHR for v-8.3.0.001 on 09/21/2020
                If String.IsNullOrWhiteSpace(rate.CarrTarEquipMatName) Then rate.CarrTarEquipMatName = "Rates"
                Dim blnAddingNewMatrix As Boolean = False
                'verify rate contract
                If rate.CarrTarEquipMatCarrTarControl = 0 Then
                    Dim lDetails As New List(Of String) From {"Carrier Contract Reference", " was not provided and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                If Not db.CarrierTariffs.Any(Function(x) x.CarrTarControl = rate.CarrTarEquipMatCarrTarControl) Then
                    Dim lDetails As New List(Of String) From {"Carrier Contract Reference", " was not found and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                'verify rate service/equipment
                If rate.CarrTarEquipMatCarrTarEquipControl = 0 Then
                    Dim lDetails As New List(Of String) From {"Carrier Contract Service Reference", " was not provided and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                If Not db.CarrierTariffEquipments.Any(Function(x) x.CarrTarEquipControl = rate.CarrTarEquipMatCarrTarEquipControl) Then
                    Dim lDetails As New List(Of String) From {"Carrier Contract Service Reference", " was not found and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                'For Distance Rates we do not need to verify rate break point 
                'If rate.CarrTarEquipMatCarrTarMatBPControl = 0 Then
                '    Dim lDetails As New List(Of String) From {"Carrier Contract Break Point Reference", " was not provided and "}
                '    throwInvalidKeyParentRequiredException(lDetails)
                '    Return False
                'End If
                'If Not db.CarrierTariffMatrixBPs.Any(Function(x) x.CarrTarMatBPControl = rate.CarrTarEquipMatCarrTarMatBPControl) Then
                '    Dim lDetails As New List(Of String) From {"Carrier Contract Break Point Reference", " was not found and "}
                '    throwInvalidKeyParentRequiredException(lDetails)
                '    Return False
                'End If
                'check for adding new
                If rate.CarrTarEquipMatControl = 0 Then
                    blnAddingNewMatrix = True
                    Dim strMSG As String = ""
                    Dim skipObjs As New List(Of String) From {"Rate", "CarrTarEquipMatModUser", "CarrTarEquipMatModDate", "CarrTarEquipMatUpdated", "CarrierTariffEquipMatrixDetails", "CarrierTariffEquipment", "LaneNumber"}
                    oNew = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(oNew, rate, skipObjs, strMSG)
                    With oNew
                        .CarrTarEquipMatModDate = Date.Now
                        .CarrTarEquipMatModUser = Me.Parameters.UserName
                    End With
                    If Debugger.IsAttached AndAlso Not String.IsNullOrWhiteSpace(strMSG) Then
                        System.Diagnostics.Debug.WriteLine(strMSG)
                    End If
                    db.CarrierTariffEquipMatrixes.InsertOnSubmit(oNew)
                Else
                    'update the existing matrix
                    Dim oExisting As LTS.CarrierTariffEquipMatrix = db.CarrierTariffEquipMatrixes.Where(Function(x) x.CarrTarEquipMatControl = rate.CarrTarEquipMatControl).FirstOrDefault()
                    If oExisting Is Nothing OrElse oExisting.CarrTarEquipMatControl = 0 Then
                        throwRecordDeletedFaultException("Cannot save changes for tariff distance rate: " & rate.CarrTarEquipMatName)
                    End If
                    Dim strMSG As String = ""
                    Dim skipObjs As New List(Of String) From {"Rate", "CarrTarEquipMatModUser", "CarrTarEquipMatModDate", "CarrTarEquipMatUpdated", "CarrierTariffEquipMatrixDetails", "CarrierTariffEquipment", "LaneNumber"}
                    oExisting = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(oExisting, rate, skipObjs, strMSG)
                    With oExisting
                        .CarrTarEquipMatModDate = Date.Now
                        .CarrTarEquipMatModUser = Me.Parameters.UserName
                    End With
                    'now update the Matrix Details for Distance Rates we only have one
                    SaveCarrTarEquipMatDet(oExisting.CarrTarEquipMatControl, rate.Rate, 1, db)
                End If
                db.SubmitChanges()
                If blnAddingNewMatrix Then
                    'we need to add the new matrix detail record
                    SaveCarrTarEquipMatDet(oNew.CarrTarEquipMatControl, rate.Rate, 1, db, True)
                    'we need to save the matrix details
                    db.SubmitChanges()
                End If
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, getSourceCaller("SaveCarrTarEquipMatFlatRate"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Create new Rate Matrix from Service (vCarrierTariffService)
    ''' </summary>
    ''' <param name="service"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.4.001 on 06/14/2023 new Origin Rate shop logic
    '''     convert vCarrierTariffService to vCarrTarEquipMatPivotDetail or vCarrTarEquipMatPivotTruckLoad then create new rate matrix
    ''' </remarks>
    Public Function createNewCarrTarEquipMatrixFromService(ByVal service As LTS.vCarrierTariffService) As Boolean
        Dim blnRet As Boolean = False
        If service.CarrTarEquipMatTarRateTypeControl = Utilities.TariffRateType.ClassRate Or service.CarrTarEquipMatTarRateTypeControl = Utilities.TariffRateType.UnitOfMeasure Then
            Dim rate As New LTS.vCarrTarEquipMatPivotDetail()
            With rate
                .CarrTarEquipMatCarrTarControl = service.CarrTarEquipCarrTarControl
                .CarrTarEquipMatCarrTarEquipControl = service.CarrTarEquipControl
                .CarrTarEquipMatCarrTarMatBPControl = service.CarrTarEquipMatCarrTarMatBPControl
                .CarrTarEquipMatCity = service.CarrTarEquipMatCity
                .CarrTarEquipMatClassTypeControl = service.CarrTarEquipMatClassTypeControl
                .CarrTarEquipMatCountry = service.CarrTarEquipMatCountry
                .CarrTarEquipMatFromZip = service.CarrTarEquipMatFromZip
                .CarrTarEquipMatLaneControl = 0
                .CarrTarEquipMatMaxDays = service.CarrTarEquipMatMaxDays
                .CarrTarEquipMatMin = service.CarrTarEquipMatMin
                .CarrTarEquipMatModDate = Date.Now()
                .CarrTarEquipMatModUser = Parameters.UserName
                .CarrTarEquipMatName = service.CarrTarEquipMatName
                .CarrTarEquipMatState = service.CarrTarEquipMatState
                .CarrTarEquipMatTarBracketTypeControl = service.CarrTarEquipMatTarBracketTypeControl
                .CarrTarEquipMatTarRateTypeControl = service.CarrTarEquipMatTarRateTypeControl
                .CarrTarEquipMatToZip = service.CarrTarEquipMatToZip
                .CarrTarEquipMultiOrigRating = service.CarrTarEquipMultiOrigRating ' Modified by RHR for v-8.5.4.001 on 06/14/2023 new Origin Rate shop logic
            End With
            If service.CarrTarEquipMatTarRateTypeControl = Utilities.TariffRateType.ClassRate Then
                Return SaveCarrTarEquipMatClassRate(rate)
            Else
                Return SaveCarrTarEquipMatUOMRate(rate)
            End If
        Else
            Dim rate As New LTS.vCarrTarEquipMatPivotTruckLoad()
            With rate
                .CarrTarEquipMatCarrTarControl = service.CarrTarEquipCarrTarControl
                .CarrTarEquipMatCarrTarEquipControl = service.CarrTarEquipControl
                .CarrTarEquipMatCarrTarMatBPControl = service.CarrTarEquipMatCarrTarMatBPControl
                .CarrTarEquipMatCity = service.CarrTarEquipMatCity
                .CarrTarEquipMatClassTypeControl = service.CarrTarEquipMatClassTypeControl
                .CarrTarEquipMatCountry = service.CarrTarEquipMatCountry
                .CarrTarEquipMatFromZip = service.CarrTarEquipMatFromZip
                .CarrTarEquipMatLaneControl = 0
                .CarrTarEquipMatMaxDays = service.CarrTarEquipMatMaxDays
                .CarrTarEquipMatMin = service.CarrTarEquipMatMin
                .CarrTarEquipMatModDate = Date.Now()
                .CarrTarEquipMatModUser = Parameters.UserName
                .CarrTarEquipMatName = service.CarrTarEquipMatName
                .CarrTarEquipMatState = service.CarrTarEquipMatState
                .CarrTarEquipMatTarBracketTypeControl = service.CarrTarEquipMatTarBracketTypeControl
                .CarrTarEquipMatTarRateTypeControl = service.CarrTarEquipMatTarRateTypeControl
                .CarrTarEquipMatToZip = service.CarrTarEquipMatToZip
                .CarrTarEquipMultiOrigRating = service.CarrTarEquipMultiOrigRating ' Modified by RHR for v-8.5.4.001 on 06/14/2023 new Origin Rate shop logic
            End With
            If service.CarrTarEquipMatTarRateTypeControl = Utilities.TariffRateType.DistanceK Or service.CarrTarEquipMatTarRateTypeControl = Utilities.TariffRateType.DistanceK Then
                Return SaveCarrTarEquipMatDistanceRate(rate)
            Else
                Return SaveCarrTarEquipMatFlatRate(rate)
            End If
        End If
    End Function

    ''' <summary>
    ''' Save the flat rate data.  The caller must save any Break Point Data first and populate the break point reference control
    ''' </summary>
    ''' <param name="rate"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 09/06/2018
    '''   new LTS carrier tariff matrix view  
    '''   for now Class duplicates UOM but this may change in the future  
    ''' Modified by RHR for v-8.3.0.001 on 09/21/2020
    '''     added logic to insert a default rate name when blank
    ''' </remarks>
    Public Function SaveCarrTarEquipMatClassRate(ByVal rate As LTS.vCarrTarEquipMatPivotDetail) As Boolean
        Dim blnRet As Boolean = False
        If rate Is Nothing Then Return False 'nothing to do
        Dim oNew As New LTS.CarrierTariffEquipMatrix()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Modified by RHR for v-8.3.0.001 on 09/21/2020
                If String.IsNullOrWhiteSpace(rate.CarrTarEquipMatName) Then rate.CarrTarEquipMatName = "Rates"
                Dim blnAddingNewMatrix As Boolean = False
                'verify rate contract
                If rate.CarrTarEquipMatCarrTarControl = 0 Then
                    Dim lDetails As New List(Of String) From {"Carrier Contract Reference", " was not provided and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                If Not db.CarrierTariffs.Any(Function(x) x.CarrTarControl = rate.CarrTarEquipMatCarrTarControl) Then
                    Dim lDetails As New List(Of String) From {"Carrier Contract Reference", " was not found and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                'verify rate service/equipment
                If rate.CarrTarEquipMatCarrTarEquipControl = 0 Then
                    Dim lDetails As New List(Of String) From {"Carrier Contract Service Reference", " was not provided and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                If Not db.CarrierTariffEquipments.Any(Function(x) x.CarrTarEquipControl = rate.CarrTarEquipMatCarrTarEquipControl) Then
                    Dim lDetails As New List(Of String) From {"Carrier Contract Service Reference", " was not found and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                'For verify rate break point 
                If rate.CarrTarEquipMatCarrTarMatBPControl = 0 Then
                    Dim lDetails As New List(Of String) From {"Carrier Contract Break Point Reference", " was not provided and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                If Not db.CarrierTariffMatrixBPs.Any(Function(x) x.CarrTarMatBPControl = rate.CarrTarEquipMatCarrTarMatBPControl) Then
                    Dim lDetails As New List(Of String) From {"Carrier Contract Break Point Reference", " was not found and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                'check for adding new
                If rate.CarrTarEquipMatControl = 0 Then
                    blnAddingNewMatrix = True
                    Dim strMSG As String = ""
                    Dim skipObjs As New List(Of String) From {"Val1", "Val2", "Val3", "Val4", "Val5", "Val6", "Val7", "Val8", "Val9", "Val10", "CarrTarEquipMatModUser", "CarrTarEquipMatModDate", "CarrTarEquipMatUpdated", "CarrierTariffEquipMatrixDetails", "CarrierTariffEquipment", "LaneNumber"}
                    oNew = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(oNew, rate, skipObjs, strMSG)
                    With oNew
                        .CarrTarEquipMatModDate = Date.Now
                        .CarrTarEquipMatModUser = Me.Parameters.UserName
                    End With
                    If Debugger.IsAttached AndAlso Not String.IsNullOrWhiteSpace(strMSG) Then
                        System.Diagnostics.Debug.WriteLine(strMSG)
                    End If
                    db.CarrierTariffEquipMatrixes.InsertOnSubmit(oNew)
                Else
                    'update the existing matrix
                    Dim oExisting As LTS.CarrierTariffEquipMatrix = db.CarrierTariffEquipMatrixes.Where(Function(x) x.CarrTarEquipMatControl = rate.CarrTarEquipMatControl).FirstOrDefault()
                    If oExisting Is Nothing OrElse oExisting.CarrTarEquipMatControl = 0 Then
                        throwRecordDeletedFaultException("Cannot save changes for tariff class rate: " & rate.CarrTarEquipMatName)
                    End If
                    Dim strMSG As String = ""
                    Dim skipObjs As New List(Of String) From {"Val1", "Val2", "Val3", "Val4", "Val5", "Val6", "Val7", "Val8", "Val9", "Val10", "CarrTarEquipMatModUser", "CarrTarEquipMatModDate", "CarrTarEquipMatUpdated", "CarrierTariffEquipMatrixDetails", "CarrierTariffEquipment", "LaneNumber"}
                    oExisting = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(oExisting, rate, skipObjs, strMSG)
                    With oExisting
                        .CarrTarEquipMatModDate = Date.Now
                        .CarrTarEquipMatModUser = Me.Parameters.UserName
                    End With
                    'now update the Matrix Details for Distance Rates we only have one
                    For i As Integer = 1 To 10
                        Dim decRate As Decimal? = 0
                        Select Case i
                            Case 1
                                decRate = rate.Val1
                            Case 2
                                decRate = rate.Val2
                            Case 3
                                decRate = rate.Val3
                            Case 4
                                decRate = rate.Val4
                            Case 5
                                decRate = rate.Val5
                            Case 6
                                decRate = rate.Val6
                            Case 7
                                decRate = rate.Val7
                            Case 8
                                decRate = rate.Val8
                            Case 9
                                decRate = rate.Val9
                            Case 10
                                decRate = rate.Val10
                        End Select
                        SaveCarrTarEquipMatDet(rate.CarrTarEquipMatControl, decRate, i, db)
                    Next
                End If
                db.SubmitChanges()
                If blnAddingNewMatrix Then
                    'we need to add the new matrix detail record
                    For i As Integer = 1 To 10
                        Dim decRate As Decimal? = 0
                        Select Case i
                            Case 1
                                decRate = rate.Val1
                            Case 2
                                decRate = rate.Val2
                            Case 3
                                decRate = rate.Val3
                            Case 4
                                decRate = rate.Val4
                            Case 5
                                decRate = rate.Val5
                            Case 6
                                decRate = rate.Val6
                            Case 7
                                decRate = rate.Val7
                            Case 8
                                decRate = rate.Val8
                            Case 9
                                decRate = rate.Val9
                            Case 10
                                decRate = rate.Val10
                        End Select
                        SaveCarrTarEquipMatDet(oNew.CarrTarEquipMatControl, decRate, i, db, True)
                    Next
                    'we need to save the matrix details
                    db.SubmitChanges()
                End If
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, getSourceCaller("SaveCarrTarEquipMatClassRate"), db)
            End Try
        End Using
        Return blnRet
    End Function


    ''' <summary>
    ''' Save the unit of measure rate data.  The caller must save any Break Point Data first and populate the break point reference control
    ''' </summary>
    ''' <param name="rate"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 09/06/2018
    '''   new LTS carrier tariff matrix view  
    '''   for now Class duplicates UOM but this may change in the future 
    ''' Modified by RHR for v-8.3.0.001 on 09/21/2020
    '''     added logic to insert a default rate name when blank
    ''' </remarks>
    Public Function SaveCarrTarEquipMatUOMRate(ByVal rate As LTS.vCarrTarEquipMatPivotDetail) As Boolean
        Dim blnRet As Boolean = False
        If rate Is Nothing Then Return False 'nothing to do
        Dim oNew As New LTS.CarrierTariffEquipMatrix()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Modified by RHR for v-8.3.0.001 on 09/21/2020
                If String.IsNullOrWhiteSpace(rate.CarrTarEquipMatName) Then rate.CarrTarEquipMatName = "Rates"
                Dim blnAddingNewMatrix As Boolean = False
                'verify rate contract
                If rate.CarrTarEquipMatCarrTarControl = 0 Then
                    Dim lDetails As New List(Of String) From {"Carrier Contract Reference", " was not provided and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                If Not db.CarrierTariffs.Any(Function(x) x.CarrTarControl = rate.CarrTarEquipMatCarrTarControl) Then
                    Dim lDetails As New List(Of String) From {"Carrier Contract Reference", " was not found and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                'verify rate service/equipment
                If rate.CarrTarEquipMatCarrTarEquipControl = 0 Then
                    Dim lDetails As New List(Of String) From {"Carrier Contract Service Reference", " was not provided and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                If Not db.CarrierTariffEquipments.Any(Function(x) x.CarrTarEquipControl = rate.CarrTarEquipMatCarrTarEquipControl) Then
                    Dim lDetails As New List(Of String) From {"Carrier Contract Service Reference", " was not found and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                'For verify rate break point 
                If rate.CarrTarEquipMatCarrTarMatBPControl = 0 Then
                    Dim lDetails As New List(Of String) From {"Carrier Contract Break Point Reference", " was not provided and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                If Not db.CarrierTariffMatrixBPs.Any(Function(x) x.CarrTarMatBPControl = rate.CarrTarEquipMatCarrTarMatBPControl) Then
                    Dim lDetails As New List(Of String) From {"Carrier Contract Break Point Reference", " was not found and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                'check for adding new
                If rate.CarrTarEquipMatControl = 0 Then
                    blnAddingNewMatrix = True
                    Dim strMSG As String = ""
                    Dim skipObjs As New List(Of String) From {"Val1", "Val2", "Val3", "Val4", "Val5", "Val6", "Val7", "Val8", "Val9", "Val10", "CarrTarEquipMatModUser", "CarrTarEquipMatModDate", "CarrTarEquipMatUpdated", "CarrierTariffEquipMatrixDetails", "CarrierTariffEquipment", "LaneNumber"}
                    oNew = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(oNew, rate, skipObjs, strMSG)
                    With oNew
                        .CarrTarEquipMatModDate = Date.Now
                        .CarrTarEquipMatModUser = Me.Parameters.UserName
                    End With
                    If Debugger.IsAttached AndAlso Not String.IsNullOrWhiteSpace(strMSG) Then
                        System.Diagnostics.Debug.WriteLine(strMSG)
                    End If
                    db.CarrierTariffEquipMatrixes.InsertOnSubmit(oNew)
                Else
                    'update the existing matrix
                    Dim oExisting As LTS.CarrierTariffEquipMatrix = db.CarrierTariffEquipMatrixes.Where(Function(x) x.CarrTarEquipMatControl = rate.CarrTarEquipMatControl).FirstOrDefault()
                    If oExisting Is Nothing OrElse oExisting.CarrTarEquipMatControl = 0 Then
                        throwRecordDeletedFaultException("Cannot save changes for tariff uom rate: " & rate.CarrTarEquipMatName)
                    End If
                    Dim strMSG As String = ""
                    Dim skipObjs As New List(Of String) From {"Val1", "Val2", "Val3", "Val4", "Val5", "Val6", "Val7", "Val8", "Val9", "Val10", "CarrTarEquipMatModUser", "CarrTarEquipMatModDate", "CarrTarEquipMatUpdated", "CarrierTariffEquipMatrixDetails", "CarrierTariffEquipment", "LaneNumber"}
                    oExisting = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(oExisting, rate, skipObjs, strMSG)
                    With oExisting
                        .CarrTarEquipMatModDate = Date.Now
                        .CarrTarEquipMatModUser = Me.Parameters.UserName
                    End With
                    'now update the Matrix Details for Distance Rates we only have one
                    For i As Integer = 1 To 10
                        Dim decRate As Decimal? = 0
                        Select Case i
                            Case 1
                                decRate = rate.Val1
                            Case 2
                                decRate = rate.Val2
                            Case 3
                                decRate = rate.Val3
                            Case 4
                                decRate = rate.Val4
                            Case 5
                                decRate = rate.Val5
                            Case 6
                                decRate = rate.Val6
                            Case 7
                                decRate = rate.Val7
                            Case 8
                                decRate = rate.Val8
                            Case 9
                                decRate = rate.Val9
                            Case 10
                                decRate = rate.Val10
                        End Select
                        SaveCarrTarEquipMatDet(rate.CarrTarEquipMatControl, decRate, i, db)
                    Next
                End If
                db.SubmitChanges()
                If blnAddingNewMatrix Then
                    'we need to add the new matrix detail record
                    For i As Integer = 1 To 10
                        Dim decRate As Decimal? = 0
                        Select Case i
                            Case 1
                                decRate = rate.Val1
                            Case 2
                                decRate = rate.Val2
                            Case 3
                                decRate = rate.Val3
                            Case 4
                                decRate = rate.Val4
                            Case 5
                                decRate = rate.Val5
                            Case 6
                                decRate = rate.Val6
                            Case 7
                                decRate = rate.Val7
                            Case 8
                                decRate = rate.Val8
                            Case 9
                                decRate = rate.Val9
                            Case 10
                                decRate = rate.Val10
                        End Select
                        SaveCarrTarEquipMatDet(oNew.CarrTarEquipMatControl, decRate, i, db, True)
                    Next
                    'we need to save the matrix details
                    db.SubmitChanges()
                End If
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, getSourceCaller("SaveCarrTarEquipMatUOMRate"), db)
            End Try
        End Using
        Return blnRet
    End Function


    ''' <summary>
    ''' Inserts or updates a CarrierTariffEquipMatrixDetail record as needed
    ''' Caller must manage all exceptions
    ''' </summary>
    ''' <param name="CarrTarEquipMatControl"></param>
    ''' <param name="decRate"></param>
    ''' <param name="ID"></param>
    ''' <param name="db"></param>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 09/06/2018
    ''' </remarks>
    Private Sub SaveCarrTarEquipMatDet(ByVal CarrTarEquipMatControl As Integer, ByVal decRate As Decimal?, ByVal ID As Integer, ByRef db As NGLMASCarrierDataContext, Optional ByVal blnAlwaysInsert As Boolean = False)
        Dim oExistingDetail As LTS.CarrierTariffEquipMatrixDetail
        If Not blnAlwaysInsert Then oExistingDetail = db.CarrierTariffEquipMatrixDetails.Where(Function(x) x.CarrTarEquipMatDetCarrTarEquipMatControl = CarrTarEquipMatControl And x.CarrTarEquipMatDetID = ID).FirstOrDefault()
        If blnAlwaysInsert OrElse (oExistingDetail Is Nothing OrElse oExistingDetail.CarrTarEquipMatDetControl = 0) Then
            'we need to add a new one
            oExistingDetail = New LTS.CarrierTariffEquipMatrixDetail()
            With oExistingDetail
                .CarrTarEquipMatDetCarrTarEquipMatControl = CarrTarEquipMatControl
                .CarrTarEquipMatDetID = ID
                .CarrTarEquipMatDetValue = decRate
                .CarrTarEquipMatDetModDate = Date.Now
                .CarrTarEquipMatDetModUser = Me.Parameters.UserName
            End With
            db.CarrierTariffEquipMatrixDetails.InsertOnSubmit(oExistingDetail)
        Else
            'update the detail record
            oExistingDetail.CarrTarEquipMatDetValue = decRate
            oExistingDetail.CarrTarEquipMatDetModDate = Date.Now
            oExistingDetail.CarrTarEquipMatDetModUser = Me.Parameters.UserName
        End If

    End Sub



    Public Function SaveCarrTarEquipMatPivot(ByVal Pivot As DataTransferObjects.CarrTarEquipMatPivot) As DataTransferObjects.CarrTarEquipMatPivot

        Using Me.LinqDB
            Try
                If Pivot Is Nothing Then Return Nothing
                'check if we need to create a break point pivot record
                If Pivot.CarrTarEquipMatCarrTarMatBPControl = 0 Then
                    'Try
                    Dim BPPivot As DataTransferObjects.CarrTarMatBPPivot
                    If Pivot.BPPivot Is Nothing Then
                        BPPivot = New NGLCarrTarMatBPData(Me.Parameters).GetOrCreateCarrTarMatBPPivot(Pivot.CarrTarEquipMatCarrTarControl, Pivot.CarrTarEquipMatCarrTarEquipControl, Pivot.CarrTarEquipMatClassTypeControl, Pivot.CarrTarEquipMatTarBracketTypeControl, Pivot.CarrTarEquipMatTarRateTypeControl, Pivot.CarrTarEquipMatName & " Break Points")
                    ElseIf Pivot.BPPivot.CarrTarMatBPControl = 0 Then
                        'this is used when a BPPivot Template is selected by the UI
                        'we just make sure the correct FK fields are updated to match the current matrix record
                        With Pivot.BPPivot
                            .CarrTarMatBPCarrTarControl = Pivot.CarrTarEquipMatCarrTarControl
                            .CarrTarMatBPName = Pivot.CarrTarEquipMatCarrTarEquipControl.ToString
                            .CarrTarMatBPClassTypeControl = Pivot.CarrTarEquipMatClassTypeControl
                            .CarrTarMatBPTarBracketTypeControl = Pivot.CarrTarEquipMatTarBracketTypeControl
                            .CarrTarMatBPTarRateTypeControl = Pivot.CarrTarEquipMatTarRateTypeControl
                            .CarrTarMatBPModDate = Pivot.CarrTarEquipMatModDate
                            .CarrTarMatBPModUser = Pivot.CarrTarEquipMatModUser
                        End With
                        BPPivot = New NGLCarrTarMatBPData(Me.Parameters).GetOrCreateCarrTarMatBPPivot(Pivot.BPPivot)
                    Else
                        BPPivot = Pivot.BPPivot
                    End If

                    If Not BPPivot Is Nothing AndAlso BPPivot.CarrTarMatBPControl <> 0 Then
                        Pivot.CarrTarEquipMatCarrTarMatBPControl = BPPivot.CarrTarMatBPControl
                    End If
                    'Catch ex As Exception
                    '    'ignore any errors when creating break point data the BP exception handler will log the errors
                    'End Try
                End If

                'get a copy of the data
                Dim oData = GetCarrTarEquipMatFiltered(Pivot.CarrTarEquipMatControl)
                With oData
                    .CarrTarEquipMatCarrTarMatBPControl = Pivot.CarrTarEquipMatCarrTarMatBPControl
                    .CarrTarEquipMatFromZip = Pivot.CarrTarEquipMatFromZip
                    .CarrTarEquipMatToZip = Pivot.CarrTarEquipMatToZip
                    .CarrTarEquipMatMin = Pivot.CarrTarEquipMatMin
                    .CarrTarEquipMatMaxDays = Pivot.CarrTarEquipMatMaxDays
                    'We set moduser, moddate and updated properties of the copy 
                    'equal to the pivot data for change management logic.
                    .CarrTarEquipMatModUser = Pivot.CarrTarEquipMatModUser
                    .CarrTarEquipMatModDate = Pivot.CarrTarEquipMatModDate
                    .CarrTarEquipMatUpdated = Pivot.CarrTarEquipMatUpdated
                    .CarrTarEquipMatName = Pivot.CarrTarEquipMatName
                    .CarrTarEquipMatClass = Pivot.CarrTarEquipMatClass
                    .CarrTarEquipMatCountry = Pivot.CarrTarEquipMatCountry
                    .CarrTarEquipMatState = Pivot.CarrTarEquipMatState
                    .CarrTarEquipMatCity = Pivot.CarrTarEquipMatCity
                    .CarrTarEquipMatClassTypeControl = Pivot.CarrTarEquipMatClassTypeControl
                    .CarrTarEquipMatTarRateTypeControl = Pivot.CarrTarEquipMatTarRateTypeControl
                    .CarrTarEquipMatLaneControl = Pivot.CarrTarEquipMatLaneControl
                    .CarrTarEquipMatTarBracketTypeControl = Pivot.CarrTarEquipMatTarBracketTypeControl
                    .CarrTarEquipMatOrigZip = Pivot.CarrTarEquipMatOrigZip ' Modified by RHR for v-8.5.4.001 on 06/14/2023 new Origin Rate shop logic
                End With
                'now update the details we do not perform change management because the pivot data is treated as a single element
                If Not oData.CarrierTariffEquipMatrixDetails Is Nothing AndAlso oData.CarrierTariffEquipMatrixDetails.Count > 0 Then
                    For Each item In oData.CarrierTariffEquipMatrixDetails
                        Select Case item.CarrTarEquipMatDetID
                            Case 1
                                If item.CarrTarEquipMatDetValue <> Pivot.Val1 Then
                                    item.TrackingState = TrackingInfo.Updated
                                    item.CarrTarEquipMatDetValue = Pivot.Val1
                                End If
                            Case 2
                                If item.CarrTarEquipMatDetValue <> Pivot.Val2 Then
                                    item.TrackingState = TrackingInfo.Updated
                                    item.CarrTarEquipMatDetValue = Pivot.Val2
                                End If
                            Case 3
                                If item.CarrTarEquipMatDetValue <> Pivot.Val3 Then
                                    item.TrackingState = TrackingInfo.Updated
                                    item.CarrTarEquipMatDetValue = Pivot.Val3
                                End If
                            Case 4
                                If item.CarrTarEquipMatDetValue <> Pivot.Val4 Then
                                    item.TrackingState = TrackingInfo.Updated
                                    item.CarrTarEquipMatDetValue = Pivot.Val4
                                End If
                            Case 5
                                If item.CarrTarEquipMatDetValue <> Pivot.Val5 Then
                                    item.TrackingState = TrackingInfo.Updated
                                    item.CarrTarEquipMatDetValue = Pivot.Val5
                                End If
                            Case 6
                                If item.CarrTarEquipMatDetValue <> Pivot.Val6 Then
                                    item.TrackingState = TrackingInfo.Updated
                                    item.CarrTarEquipMatDetValue = Pivot.Val6
                                End If
                            Case 7
                                If item.CarrTarEquipMatDetValue <> Pivot.Val7 Then
                                    item.TrackingState = TrackingInfo.Updated
                                    item.CarrTarEquipMatDetValue = Pivot.Val7
                                End If
                            Case 8
                                If item.CarrTarEquipMatDetValue <> Pivot.Val8 Then
                                    item.TrackingState = TrackingInfo.Updated
                                    item.CarrTarEquipMatDetValue = Pivot.Val8
                                End If
                            Case 9
                                If item.CarrTarEquipMatDetValue <> Pivot.Val9 Then
                                    item.TrackingState = TrackingInfo.Updated
                                    item.CarrTarEquipMatDetValue = Pivot.Val9
                                End If
                            Case 10
                                If item.CarrTarEquipMatDetValue <> Pivot.Val10 Then
                                    item.TrackingState = TrackingInfo.Updated
                                    item.CarrTarEquipMatDetValue = Pivot.Val10
                                End If
                        End Select
                    Next
                End If

                UpdateRecordWithDetails(oData)
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                throwUnExpectedFaultException(ex, getSourceCaller("SaveCarrTarEquipMatPivot"), Pivot.ToString, sysErrorParameters.sysErrorSeverity.Unexpected, sysErrorParameters.sysErrorState.ServerLevelFault)
            End Try

        End Using
        Return GetCarrTarEquipMatPivot(Pivot.CarrTarEquipMatControl)
    End Function

    Public Function CreateCarrTarEquipMatPivot(ByVal Pivot As DataTransferObjects.CarrTarEquipMatPivot) As DataTransferObjects.CarrTarEquipMatPivot
        Dim intCarrTarEquipMatControl As Integer = 0
        Using Me.LinqDB
            Try
                If Pivot Is Nothing Then Return Nothing
                'check if we need to create a break point pivot record
                If Pivot.CarrTarEquipMatCarrTarMatBPControl = 0 Then
                    'Try
                    Dim BPPivot As DataTransferObjects.CarrTarMatBPPivot
                    If Pivot.BPPivot Is Nothing Then
                        BPPivot = New NGLCarrTarMatBPData(Me.Parameters).GetOrCreateCarrTarMatBPPivot(Pivot.CarrTarEquipMatCarrTarControl, Pivot.CarrTarEquipMatCarrTarEquipControl, Pivot.CarrTarEquipMatClassTypeControl, Pivot.CarrTarEquipMatTarBracketTypeControl, Pivot.CarrTarEquipMatTarRateTypeControl, Pivot.CarrTarEquipMatName & " Break Points")
                    ElseIf Pivot.BPPivot.CarrTarMatBPControl = 0 Then
                        'this is used when a BPPivot Template is selected by the UI
                        'we just make sure the correct FK fields are updated to match the current matrix record
                        With Pivot.BPPivot
                            .CarrTarMatBPCarrTarControl = Pivot.CarrTarEquipMatCarrTarControl
                            .CarrTarMatBPName = Pivot.CarrTarEquipMatCarrTarEquipControl.ToString
                            .CarrTarMatBPClassTypeControl = Pivot.CarrTarEquipMatClassTypeControl
                            .CarrTarMatBPTarBracketTypeControl = Pivot.CarrTarEquipMatTarBracketTypeControl
                            .CarrTarMatBPTarRateTypeControl = Pivot.CarrTarEquipMatTarRateTypeControl
                            .CarrTarMatBPModDate = Pivot.CarrTarEquipMatModDate
                            .CarrTarMatBPModUser = Pivot.CarrTarEquipMatModUser
                        End With
                        BPPivot = New NGLCarrTarMatBPData(Me.Parameters).GetOrCreateCarrTarMatBPPivot(Pivot.BPPivot)
                    Else
                        BPPivot = Pivot.BPPivot
                    End If

                    If Not BPPivot Is Nothing AndAlso BPPivot.CarrTarMatBPControl <> 0 Then
                        Pivot.CarrTarEquipMatCarrTarMatBPControl = BPPivot.CarrTarMatBPControl
                    End If
                    'Catch ex As Exception
                    '    'ignore any errors when creating break point data the BP exception handler will log the errors
                    'End Try
                End If
                'We set moduser, moddate and updated properties of the copy 
                'equal to the pivot data for change management logic.
                ' Modified by RHR for v-8.5.4.001 on 06/14/2023 new Origin Rate shop logic
                Dim oData As New DataTransferObjects.CarrTarEquipMat With
                        {
                        .CarrTarEquipMatCarrTarControl = Pivot.CarrTarEquipMatCarrTarControl,
                        .CarrTarEquipMatCarrTarEquipControl = Pivot.CarrTarEquipMatCarrTarEquipControl,
                        .CarrTarEquipMatCarrTarMatBPControl = Pivot.CarrTarEquipMatCarrTarMatBPControl,
                        .CarrTarEquipMatFromZip = Pivot.CarrTarEquipMatFromZip,
                        .CarrTarEquipMatToZip = Pivot.CarrTarEquipMatToZip,
                        .CarrTarEquipMatMin = Pivot.CarrTarEquipMatMin,
                        .CarrTarEquipMatMaxDays = Pivot.CarrTarEquipMatMaxDays,
                        .CarrTarEquipMatModUser = Pivot.CarrTarEquipMatModUser,
                        .CarrTarEquipMatModDate = Pivot.CarrTarEquipMatModDate,
                        .CarrTarEquipMatUpdated = Pivot.CarrTarEquipMatUpdated,
                        .CarrTarEquipMatName = Pivot.CarrTarEquipMatName,
                        .CarrTarEquipMatClass = Pivot.CarrTarEquipMatClass,
                        .CarrTarEquipMatCountry = Pivot.CarrTarEquipMatCountry,
                        .CarrTarEquipMatState = Pivot.CarrTarEquipMatState,
                        .CarrTarEquipMatCity = Pivot.CarrTarEquipMatCity,
                        .CarrTarEquipMatClassTypeControl = Pivot.CarrTarEquipMatClassTypeControl,
                        .CarrTarEquipMatTarRateTypeControl = Pivot.CarrTarEquipMatTarRateTypeControl,
                        .CarrTarEquipMatLaneControl = Pivot.CarrTarEquipMatLaneControl,
                        .CarrTarEquipMatTarBracketTypeControl = Pivot.CarrTarEquipMatTarBracketTypeControl,
                        .CarrTarEquipMatOrigZip = Pivot.CarrTarEquipMatOrigZip
                        }
                Dim oNewData As DataTransferObjects.CarrTarEquipMat = Me.Add(oData, LinqTable)
                'now insert the details we do not perform change management because the pivot data is treated as a single element
                If Not oNewData Is Nothing AndAlso oNewData.CarrTarEquipMatControl <> 0 Then
                    intCarrTarEquipMatControl = oNewData.CarrTarEquipMatControl
                    Dim oMatDet As NGLCarrTarEquipMatDetData = Me.NDPBaseClassFactory("NGLCarrTarEquipMatDetData", False)
                    Dim intPivotVal As System.Nullable(Of Decimal)() = {Pivot.Val1, Pivot.Val2, Pivot.Val3, Pivot.Val4, Pivot.Val5, Pivot.Val6, Pivot.Val7, Pivot.Val8, Pivot.Val9, Pivot.Val10}
                    Dim val As Nullable(Of Decimal) = Nothing
                    For i As Integer = 0 To 9
                        val = IIf(intPivotVal(i) Is Nothing, Nothing, intPivotVal(i))
                        If val IsNot Nothing Then
                            'create a new detail record
                            Dim oDetail As New DataTransferObjects.CarrTarEquipMatDet With {.CarrTarEquipMatDetCarrTarEquipMatControl = intCarrTarEquipMatControl, .CarrTarEquipMatDetID = (i + 1), .CarrTarEquipMatDetValue = val, .TrackingState = TrackingInfo.Created}
                            oMatDet.CreateRecord(oDetail)
                        End If

                    Next
                End If
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                throwUnExpectedFaultException(ex, getSourceCaller("SaveCarrTarEquipMatPivot"), Pivot.ToString, sysErrorParameters.sysErrorSeverity.Unexpected, sysErrorParameters.sysErrorState.ServerLevelFault)
            End Try

        End Using
        Return GetCarrTarEquipMatPivot(intCarrTarEquipMatControl)
    End Function

    ''' <summary>
    ''' Call this method in the CSV import
    ''' 'return a BPControl as integer
    ''' </summary>
    ''' <param name="Pivot"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCarrTarEquipBPControl(ByVal Pivot As DataTransferObjects.CarrTarEquipMatPivot) As Integer
        Dim intCarrTarEquipMatControl As Integer = 0
        Using Me.LinqDB
            Try
                If Pivot Is Nothing Then Return Nothing
                'check if we need to create a break point pivot record
                Dim bpControl As Integer = 0
                If Pivot.CarrTarEquipMatCarrTarMatBPControl = 0 Then

                    Dim BPPivot As DataTransferObjects.CarrTarMatBPPivot
                    If Pivot.BPPivot Is Nothing Then
                        BPPivot = New NGLCarrTarMatBPData(Me.Parameters).GetOrCreateCarrTarMatBPPivot(Pivot.CarrTarEquipMatCarrTarControl, Pivot.CarrTarEquipMatCarrTarEquipControl, Pivot.CarrTarEquipMatClassTypeControl, Pivot.CarrTarEquipMatTarBracketTypeControl, Pivot.CarrTarEquipMatTarRateTypeControl, Pivot.CarrTarEquipMatName & " Break Points")
                    ElseIf Pivot.BPPivot.CarrTarMatBPControl = 0 Then
                        'this is used when a BPPivot Template is selected by the UI
                        'we just make sure the correct FK fields are updated to match the current matrix record
                        With Pivot.BPPivot
                            .CarrTarMatBPCarrTarControl = Pivot.CarrTarEquipMatCarrTarControl
                            .CarrTarMatBPName = Pivot.CarrTarEquipMatCarrTarEquipControl.ToString
                            .CarrTarMatBPClassTypeControl = Pivot.CarrTarEquipMatClassTypeControl
                            .CarrTarMatBPTarBracketTypeControl = Pivot.CarrTarEquipMatTarBracketTypeControl
                            .CarrTarMatBPTarRateTypeControl = Pivot.CarrTarEquipMatTarRateTypeControl
                            .CarrTarMatBPModDate = Pivot.CarrTarEquipMatModDate
                            .CarrTarMatBPModUser = Pivot.CarrTarEquipMatModUser
                        End With
                        BPPivot = New NGLCarrTarMatBPData(Me.Parameters).GetOrCreateCarrTarMatBPPivot(Pivot.BPPivot)
                    Else
                        BPPivot = Pivot.BPPivot
                    End If

                    If Not BPPivot Is Nothing AndAlso BPPivot.CarrTarMatBPControl <> 0 Then
                        Pivot.CarrTarEquipMatCarrTarMatBPControl = BPPivot.CarrTarMatBPControl
                        bpControl = BPPivot.CarrTarMatBPControl
                    End If
                    Return bpControl
                End If
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                throwUnExpectedFaultException(ex, getSourceCaller("SaveCarrTarEquipMatPivot"), Pivot.ToString, sysErrorParameters.sysErrorSeverity.Unexpected, sysErrorParameters.sysErrorState.ServerLevelFault)
            End Try
        End Using

    End Function

    Public Sub DeletCarrTarEquipMatByControl(ByVal Control As Integer)
        If Control <> 0 Then DeleteRecord(GetCarrTarEquipMatFiltered(Control))
    End Sub

    Public Function DeleteCarrTarEquipMatRecords(ByVal sControls As String) As Boolean
        Dim strProcName As String = "dbo.spDeleteCarrTarEquipMatRecords"
        Dim blnRet As Boolean = False
        If sControls = "" Then Return False 'nothing to do'
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim oCmd As New System.Data.SqlClient.SqlCommand
                oCmd.Parameters.AddWithValue("@CarrTarEquipMatControl", sControls)
                runNGLStoredProcedure(oCmd, strProcName, 0)
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteCarrTarEquipMatRecords"), db)
            End Try
        End Using
        Return blnRet
    End Function

    Public Function DeleteCarrTarEquipMatRates(ByVal CarrTarEquipControl As Integer, Optional ByVal blnAll As Boolean = False) As Boolean
        Dim strProcName As String = "dbo.spDeleteCarrTarEquipMatRates"
        Dim blnRet As Boolean = False
        If CarrTarEquipControl = 0 Then Return False 'nothing to do'
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                If blnAll Then
                    db.spDeleteAllCarrTarEquipMatRates(CarrTarEquipControl)
                Else
                    db.spDeleteCarrTarEquipMatRates(CarrTarEquipControl)
                End If

                blnRet = True

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteCarrTarEquipMatRates"), db)
            End Try
        End Using
        Return blnRet
    End Function


    ''' <summary>
    ''' Returns true if CarrTarEquipMatName can be used or false if this equipment configuration already has a matching named configuraiton
    ''' </summary>
    ''' <param name="EquipControl"></param>
    ''' <param name="RateTypeControl"></param>
    ''' <param name="BracketTypeControl"></param>
    ''' <param name="ClassTypeControl"></param>
    ''' <param name="CarrTarEquipMatName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function isAltDataKeyAvailable(ByVal EquipControl As Integer,
                                          ByVal RateTypeControl As Integer,
                                          ByVal BracketTypeControl As Integer,
                                          ByVal ClassTypeControl As Integer,
                                          ByVal CarrTarEquipMatName As String,
                                          ByVal CarrTarEquipMatCarrTarControl As Integer) As Boolean
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Return Not doesAltDataKeyExist(db, EquipControl, RateTypeControl, BracketTypeControl, ClassTypeControl, CarrTarEquipMatName, CarrTarEquipMatCarrTarControl)
        End Using
    End Function

    ''' <summary>
    ''' Uses logic from NGLTariffImportToolBLL.getRatesDDList()
    ''' </summary>
    ''' <param name="iCarrTarControl"></param>
    ''' <returns></returns>
    Public Function GetTariffExport365(ByVal iCarrTarControl As Integer) As Models.TariffExcelExport()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Get the filters for the contract
                Dim iRet = db.CarrierTariffEquipMatrixes.Where(Function(x) x.CarrTarEquipMatCarrTarControl = iCarrTarControl).FirstOrDefault()

                If iRet Is Nothing OrElse iRet.CarrTarEquipMatControl = 0 Then Return Nothing

                Dim CarrTarEquipControl As Integer = iRet.CarrTarEquipMatCarrTarEquipControl
                Dim TarRateTypeControl As Integer = iRet.CarrTarEquipMatTarRateTypeControl
                Dim ClassTypeControl As Integer = iRet.CarrTarEquipMatClassTypeControl
                Dim TarBracketTypeControl As Integer = iRet.CarrTarEquipMatTarBracketTypeControl

                Dim results = GetCarrTarEquipMatPivots(CarrTarEquipControl, TarRateTypeControl, ClassTypeControl, TarBracketTypeControl, 1, 1000000, "")

                Dim rates As New List(Of Models.TariffExcelExport)

                For Each row In results
                    Dim rate As New Models.TariffExcelExport
                    rate.Country = row.CarrTarEquipMatCountry
                    rate.State = row.CarrTarEquipMatState
                    rate.City = row.CarrTarEquipMatCity
                    rate.FromZip = row.CarrTarEquipMatFromZip
                    rate.ToZip = row.CarrTarEquipMatToZip
                    rate.Lane = row.CarrTarEquipMatLaneControl
                    rate.TariffClass = row.CarrTarEquipMatClass
                    rate.Min = If(row.CarrTarEquipMatMin.HasValue, row.CarrTarEquipMatMin.Value, 0)
                    rate.MaxDays = row.CarrTarEquipMatMaxDays
                    rate.Val1 = If(row.Val1.HasValue, row.Val1.Value, 0)
                    rate.Val2 = If(row.Val2.HasValue, row.Val2.Value, 0)
                    rate.Val3 = If(row.Val3.HasValue, row.Val3.Value, 0)
                    rate.Val4 = If(row.Val4.HasValue, row.Val4.Value, 0)
                    rate.Val5 = If(row.Val5.HasValue, row.Val5.Value, 0)
                    rate.Val6 = If(row.Val6.HasValue, row.Val6.Value, 0)
                    rate.Val7 = If(row.Val7.HasValue, row.Val7.Value, 0)
                    rate.Val8 = If(row.Val8.HasValue, row.Val8.Value, 0)
                    rate.Val9 = If(row.Val9.HasValue, row.Val9.Value, 0)
                    rate.Val10 = If(row.Val10.HasValue, row.Val10.Value, 0)
                    rates.Add(rate)
                Next
                Return rates.ToArray()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, getSourceCaller("GetTariffExport365"), db)
            End Try
            Return Nothing
        End Using
    End Function



#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DataTransferObjects.CarrTarEquipMat)
        'Create New Record
        ' Modified by RHR for v-8.5.4.001 on 06/14/2023 new Origin Rate shop logic
        Return New LTS.CarrierTariffEquipMatrix With {.CarrTarEquipMatControl = d.CarrTarEquipMatControl,
            .CarrTarEquipMatCarrTarEquipControl = d.CarrTarEquipMatCarrTarEquipControl,
            .CarrTarEquipMatCarrTarControl = d.CarrTarEquipMatCarrTarControl,
            .CarrTarEquipMatCarrTarMatControl = d.CarrTarEquipMatCarrTarMatControl,
            .CarrTarEquipMatCarrTarMatBPControl = d.CarrTarEquipMatCarrTarMatBPControl,
            .CarrTarEquipMatFromZip = d.CarrTarEquipMatFromZip,
            .CarrTarEquipMatToZip = d.CarrTarEquipMatToZip,
            .CarrTarEquipMatExptFlag = d.CarrTarEquipMatExptFlag,
            .CarrTarEquipMatMin = d.CarrTarEquipMatMin,
            .CarrTarEquipMatMaxDays = d.CarrTarEquipMatMaxDays,
            .CarrTarEquipMatName = d.CarrTarEquipMatName,
            .CarrTarEquipMatClass = d.CarrTarEquipMatClass,
            .CarrTarEquipMatCountry = d.CarrTarEquipMatCountry,
            .CarrTarEquipMatState = d.CarrTarEquipMatState,
            .CarrTarEquipMatCity = d.CarrTarEquipMatCity,
            .CarrTarEquipMatClassTypeControl = d.CarrTarEquipMatClassTypeControl,
            .CarrTarEquipMatTarRateTypeControl = d.CarrTarEquipMatTarRateTypeControl,
            .CarrTarEquipMatLaneControl = d.CarrTarEquipMatLaneControl,
            .CarrTarEquipMatTarBracketTypeControl = d.CarrTarEquipMatTarBracketTypeControl,
            .CarrTarEquipMatModUser = Parameters.UserName,
            .CarrTarEquipMatModDate = Date.Now,
            .CarrTarEquipMatUpdated = If(d.CarrTarEquipMatUpdated Is Nothing, New Byte() {}, d.CarrTarEquipMatUpdated),
            .CarrTarEquipMatOrigZip = d.CarrTarEquipMatOrigZip}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCarrTarEquipMatFiltered(Control:=CType(LinqTable, LTS.CarrierTariffEquipMatrix).CarrTarEquipMatControl)
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
    '                Dim varTariff = (From d In db.CarrierTariffEquipMatrixes Where d.CarrTarID = strNewTariffID Select d).First
    '                If Not varTariff Is Nothing Then
    '                    If Not AllowOverwrite Then Return False
    '                    CarrTarControl = varTariff.CarrTarControl
    '                    CarrTarID = strNewTariffID
    '                    ''Delete all of the matrix details they no longer match.
    '                    'executeSQL("DELETE FROM [dbo].[CarrierTariffEquipMatrixMatrix] Where CarrTarMatCarrTarControl = " & CarrTarControl)
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
                Dim source As LTS.CarrierTariffEquipMatrix = TryCast(LinqTable, LTS.CarrierTariffEquipMatrix)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.CarrierTariffEquipMatrixes
                    Where d.CarrTarEquipMatControl = source.CarrTarEquipMatControl
                    Select New DataTransferObjects.QuickSaveResults With {.Control = d.CarrTarEquipMatControl _
                        , .ModDate = d.CarrTarEquipMatModDate _
                        , .ModUser = d.CarrTarEquipMatModUser _
                        , .Updated = d.CarrTarEquipMatUpdated.ToArray}).First

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException()
            Catch ex As Exception
                throwUnExpectedFaultException(ex, getSourceCaller("GetQuickSaveResult"))
            End Try

        End Using
        Return ret
    End Function

    Private Sub ValidateApproved(ByRef oDB As NGLMASCarrierDataContext, ByRef oData As DataTransferObjects.CarrTarEquipMat)
        DirectCast(NDPBaseClassFactory("NGLCarrTarContractData", False), NGLCarrTarContractData).ValidateApproved(oData.CarrTarEquipMatCarrTarControl, oDB)
    End Sub

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        With CType(oData, DataTransferObjects.CarrTarEquipMat)
            'If doesAltDataKeyExist(oDB, .CarrTarEquipMatCarrTarEquipControl, .CarrTarEquipMatTarRateTypeControl, .CarrTarEquipMatTarBracketTypeControl, .CarrTarEquipMatClassTypeControl, .CarrTarEquipMatName, .CarrTarEquipMatCarrTarControl) = False Then
            '    Dim strKeValues As String = String.Format("Rate Name: {4} | Equipment: {0} | Rate Type: {1} | Bracket Type: {2} | Class Type: {3} ", .CarrTarEquipMatCarrTarEquipControl, .CarrTarEquipMatTarRateTypeControl, .CarrTarEquipMatTarBracketTypeControl, .CarrTarEquipMatClassTypeControl, .CarrTarEquipMatName)
            '    throwInvalidKeyFaultException(SqlFaultInfo.FaultDetailsKey.E_CannotSaveKeyAlreadyExists, New List(Of String) From {"CarrierTariff", "Rating Configuration Key", strKeValues})
            '    Return
            'End If
        End With
        ValidateApproved(oDB, oData)
    End Sub

    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        With CType(oData, DataTransferObjects.CarrTarEquipMat)
            'If doesAltDataKeyExist(oDB, .CarrTarEquipMatCarrTarEquipControl, .CarrTarEquipMatTarRateTypeControl, .CarrTarEquipMatTarBracketTypeControl, .CarrTarEquipMatClassTypeControl, .CarrTarEquipMatControl, .CarrTarEquipMatCarrTarControl) Then
            '    Dim strKeValues As String = String.Format("Rate Name: {4} | Equipment: {0} | Rate Type: {1} | Bracket Type: {2} | Class Type: {3} ", .CarrTarEquipMatCarrTarEquipControl, .CarrTarEquipMatTarRateTypeControl, .CarrTarEquipMatTarBracketTypeControl, .CarrTarEquipMatClassTypeControl, .CarrTarEquipMatName)
            '    throwInvalidKeyFaultException(SqlFaultInfo.FaultDetailsKey.E_CannotSaveKeyAlreadyExists, New List(Of String) From {"CarrierTariff", "Rating Configuration Key", strKeValues})
            '    Return
            'End If
        End With
        ValidateApproved(oDB, oData)
    End Sub

    ''' <summary>
    ''' Checks the Book Table for a reference to the CarrTarEquipControl.
    ''' Records cannot be deleted if a reference exists.
    ''' Throws E_InvalidRequest FaultException if record exists in Book Table 
    ''' </summary>
    ''' <param name="oDB"></param>
    ''' <param name="oData"></param>
    ''' <remarks></remarks>
    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'this method is not being used for v-6.4 Validation is not currently required
        ValidateApproved(oDB, oData)
        Dim oCarrTarData As DataTransferObjects.CarrTarEquipMat = TryCast(oData, DataTransferObjects.CarrTarEquipMat)
        If oCarrTarData Is Nothing Then Return
        Dim intCarrTarControl = oCarrTarData.CarrTarEquipMatControl
        If intCarrTarControl = 0 Then Return
        Dim iCarrTarEquipMatCarrTarEquipControl As Integer = DirectCast(oData, DataTransferObjects.CarrTarEquipMat).CarrTarEquipMatCarrTarEquipControl
        Dim strCarrTarName = String.Format("City {0} State {1} Country {2} From Zip {3} To Zip {4}", oCarrTarData.CarrTarEquipMatCity, oCarrTarData.CarrTarEquipMatState, oCarrTarData.CarrTarEquipMatCountry, oCarrTarData.CarrTarEquipMatFromZip, oCarrTarData.CarrTarEquipMatToZip)

        'check if this is the last Rate,  we cannot delete the last rate for any equipment
        If DirectCast(oDB, NGLMASCarrierDataContext).CarrierTariffEquipMatrixes.Any(Function(x) x.CarrTarEquipMatCarrTarEquipControl = iCarrTarEquipMatCarrTarEquipControl And x.CarrTarEquipMatControl <> intCarrTarControl) Then
            Using db As New NGLMasBookDataContext(ConnectionString)
                If db.Books.Any(Function(x) x.BookCarrTarEquipControl = intCarrTarControl) Then
                    throwCannotDeleteRecordInUseException("Tariff Rates", strCarrTarName)
                End If
            End Using
        Else
            throwCannotDeleteRecordInUseException("Tariff Rates. One is always required.", strCarrTarName)
        End If



    End Sub

    Friend Function selectDTOData(ByVal d As LTS.CarrierTariffEquipMatrix, ByRef db As NGLMASCarrierDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.CarrTarEquipMat

        ' Modified by RHR for v-8.5.4.001 on 06/14/2023 new Origin Rate shop logic
        Return New DataTransferObjects.CarrTarEquipMat With {.CarrTarEquipMatControl = d.CarrTarEquipMatControl,
            .CarrTarEquipMatCarrTarEquipControl = d.CarrTarEquipMatCarrTarEquipControl,
            .CarrTarEquipMatCarrTarControl = d.CarrTarEquipMatCarrTarControl,
            .CarrTarEquipMatCarrTarMatControl = d.CarrTarEquipMatCarrTarMatControl,
            .CarrTarEquipMatCarrTarMatBPControl = d.CarrTarEquipMatCarrTarMatBPControl,
            .CarrTarEquipMatFromZip = d.CarrTarEquipMatFromZip,
            .CarrTarEquipMatToZip = d.CarrTarEquipMatToZip,
            .CarrTarEquipMatExptFlag = d.CarrTarEquipMatExptFlag,
            .CarrTarEquipMatMin = d.CarrTarEquipMatMin,
            .CarrTarEquipMatMaxDays = d.CarrTarEquipMatMaxDays,
            .CarrTarEquipMatName = d.CarrTarEquipMatName,
            .CarrTarEquipMatClass = d.CarrTarEquipMatClass,
            .CarrTarEquipMatCountry = d.CarrTarEquipMatCountry,
            .CarrTarEquipMatState = d.CarrTarEquipMatState,
            .CarrTarEquipMatCity = d.CarrTarEquipMatCity,
            .CarrTarEquipMatClassTypeControl = d.CarrTarEquipMatClassTypeControl,
            .CarrTarEquipMatTarRateTypeControl = d.CarrTarEquipMatTarRateTypeControl,
            .CarrTarEquipMatLaneControl = d.CarrTarEquipMatLaneControl,
            .CarrTarEquipMatTarBracketTypeControl = d.CarrTarEquipMatTarBracketTypeControl,
            .CarrTarEquipMatModUser = d.CarrTarEquipMatModUser,
            .CarrTarEquipMatModDate = d.CarrTarEquipMatModDate,
            .CarrTarEquipMatUpdated = d.CarrTarEquipMatUpdated.ToArray(),
            .CarrierTariffEquipMatrixDetails = (
                From c In d.CarrierTariffEquipMatrixDetails
                    Select New DataTransferObjects.CarrTarEquipMatDet _
                        With {.CarrTarEquipMatDetControl = c.CarrTarEquipMatDetControl _
                            , .CarrTarEquipMatDetCarrTarEquipMatControl = c.CarrTarEquipMatDetCarrTarEquipMatControl _
                            , .CarrTarEquipMatDetID = c.CarrTarEquipMatDetID _
                            , .CarrTarEquipMatDetValue = c.CarrTarEquipMatDetValue _
                            , .CarrTarEquipMatDetModDate = c.CarrTarEquipMatDetModDate _
                            , .CarrTarEquipMatDetModUser = c.CarrTarEquipMatDetModUser _
                            , .CarrTarEquipMatDetUpdated = c.CarrTarEquipMatDetUpdated.ToArray()}).ToList(),
            .Page = page,
            .Pages = pagecount,
            .RecordCount = recordcount,
            .PageSize = pagesize,
            .CarrTarEquipMatOrigZip = d.CarrTarEquipMatOrigZip}

    End Function

    Friend Function selectDTOCarrTarEquipMatPivotData(ByVal BPPivot As DataTransferObjects.CarrTarMatBPPivot, ByVal d As LTS.vCarrTarEquipMatPivot, ByRef db As NGLMASCarrierDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.CarrTarEquipMatPivot
        ' Modified by RHR for v-8.5.4.001 on 06/14/2023 new Origin Rate shop logic
        Return New DataTransferObjects.CarrTarEquipMatPivot With {.CarrTarEquipMatControl = d.CarrTarEquipMatControl,
            .CarrTarEquipMatCarrTarEquipControl = d.CarrTarEquipMatCarrTarEquipControl,
            .CarrTarEquipMatCarrTarControl = d.CarrTarEquipMatCarrTarControl,
            .CarrTarEquipMatCarrTarMatBPControl = d.CarrTarEquipMatCarrTarMatBPControl,
            .CarrTarEquipMatFromZip = d.CarrTarEquipMatFromZip,
            .CarrTarEquipMatToZip = d.CarrTarEquipMatToZip,
            .CarrTarEquipMatMin = d.CarrTarEquipMatMin,
            .CarrTarEquipMatMaxDays = d.CarrTarEquipMatMaxDays,
            .CarrTarEquipMatName = d.CarrTarEquipMatName,
            .CarrTarEquipMatClass = d.CarrTarEquipMatClass,
            .CarrTarEquipMatCountry = d.CarrTarEquipMatCountry,
            .CarrTarEquipMatState = d.CarrTarEquipMatState,
            .CarrTarEquipMatCity = d.CarrTarEquipMatCity,
            .CarrTarEquipMatClassTypeControl = d.CarrTarEquipMatClassTypeControl,
            .CarrTarEquipMatTarRateTypeControl = d.CarrTarEquipMatTarRateTypeControl,
            .CarrTarEquipMatLaneControl = d.CarrTarEquipMatLaneControl,
            .CarrTarEquipMatTarBracketTypeControl = d.CarrTarEquipMatTarBracketTypeControl,
            .Val1 = d.Val1,
            .Val2 = d.Val2,
            .Val3 = d.Val3,
            .Val4 = d.Val4,
            .Val5 = d.Val5,
            .Val6 = d.Val6,
            .Val7 = d.Val7,
            .Val8 = d.Val8,
            .Val9 = d.Val9,
            .Val10 = d.Val10,
            .CarrTarEquipMatModUser = d.CarrTarEquipMatModUser,
            .CarrTarEquipMatModDate = d.CarrTarEquipMatModDate,
            .BPPivot = BPPivot,
            .CarrTarEquipMatUpdated = d.CarrTarEquipMatUpdated.ToArray(),
            .Page = page,
            .Pages = pagecount,
            .RecordCount = recordcount,
            .PageSize = pagesize,
            .CarrTarEquipMatOrigZip = d.CarrTarEquipMatOrigZip,
            .CarrTarEquipMultiOrigRating = d.CarrTarEquipMultiOrigRating} ' Modified by RHR for v-8.5.4.001 on 06/14/2023 new Origin Rate shop logic
    End Function


    Friend Function selectDTOCarrTarEquipMatPivotData(ByVal BPPivot As DataTransferObjects.CarrTarMatBPPivot, ByVal d As LTS.spGetCarrTarEquipMatPivotPageResult) As DataTransferObjects.CarrTarEquipMatPivot
        ' Modified by RHR for v-8.5.4.001 on 06/14/2023 new Origin Rate shop logic
        Return New DataTransferObjects.CarrTarEquipMatPivot With {.CarrTarEquipMatControl = d.CarrTarEquipMatControl,
            .CarrTarEquipMatCarrTarEquipControl = d.CarrTarEquipMatCarrTarEquipControl,
            .CarrTarEquipMatCarrTarControl = d.CarrTarEquipMatCarrTarControl,
            .CarrTarEquipMatCarrTarMatBPControl = d.CarrTarEquipMatCarrTarMatBPControl,
            .CarrTarEquipMatFromZip = d.CarrTarEquipMatFromZip,
            .CarrTarEquipMatToZip = d.CarrTarEquipMatToZip,
            .CarrTarEquipMatMin = d.CarrTarEquipMatMin,
            .CarrTarEquipMatMaxDays = d.CarrTarEquipMatMaxDays,
            .CarrTarEquipMatName = d.CarrTarEquipMatName,
            .CarrTarEquipMatClass = d.CarrTarEquipMatClass,
            .CarrTarEquipMatCountry = d.CarrTarEquipMatCountry,
            .CarrTarEquipMatState = d.CarrTarEquipMatState,
            .CarrTarEquipMatCity = d.CarrTarEquipMatCity,
            .CarrTarEquipMatClassTypeControl = d.CarrTarEquipMatClassTypeControl,
            .CarrTarEquipMatTarRateTypeControl = d.CarrTarEquipMatTarRateTypeControl,
            .CarrTarEquipMatLaneControl = d.CarrTarEquipMatLaneControl,
            .CarrTarEquipMatTarBracketTypeControl = d.CarrTarEquipMatTarBracketTypeControl,
            .Val1 = d.Val1,
            .Val2 = d.Val2,
            .Val3 = d.Val3,
            .Val4 = d.Val4,
            .Val5 = d.Val5,
            .Val6 = d.Val6,
            .Val7 = d.Val7,
            .Val8 = d.Val8,
            .Val9 = d.Val9,
            .Val10 = d.Val10,
            .CarrTarEquipMatModUser = d.CarrTarEquipMatModUser,
            .CarrTarEquipMatModDate = d.CarrTarEquipMatModDate,
            .BPPivot = BPPivot,
            .CarrTarEquipMatUpdated = d.CarrTarEquipMatUpdated.ToArray(),
            .Page = d.Page,
            .Pages = d.Pages,
            .RecordCount = d.RecordCount,
            .PageSize = d.PageSize,
            .CarrTarEquipMatOrigZip = d.CarrTarEquipMatOrigZip}
    End Function

    Protected Overrides Sub AddDetailsToLinq(ByRef LinqTable As Object, ByRef oData As DataTransferObjects.DTOBaseClass)

        With CType(LinqTable, LTS.CarrierTariffEquipMatrix)
            'Add Detail Records
            .CarrierTariffEquipMatrixDetails.AddRange(
                From c In CType(oData, DataTransferObjects.CarrTarEquipMat).CarrierTariffEquipMatrixDetails
                                                         Select New LTS.CarrierTariffEquipMatrixDetail With {.CarrTarEquipMatDetControl = c.CarrTarEquipMatDetControl _
                                                         , .CarrTarEquipMatDetCarrTarEquipMatControl = c.CarrTarEquipMatDetCarrTarEquipMatControl _
                                                         , .CarrTarEquipMatDetID = c.CarrTarEquipMatDetID _
                                                         , .CarrTarEquipMatDetValue = c.CarrTarEquipMatDetValue _
                                                         , .CarrTarEquipMatDetModDate = c.CarrTarEquipMatDetModDate _
                                                         , .CarrTarEquipMatDetModUser = c.CarrTarEquipMatDetModUser _
                                                         , .CarrTarEquipMatDetUpdated = If(c.CarrTarEquipMatDetUpdated Is Nothing, New Byte() {}, c.CarrTarEquipMatDetUpdated)})

        End With
    End Sub

    Protected Overrides Sub InsertAllDetails(ByRef oDB As System.Data.Linq.DataContext, ByRef LinqTable As Object)
        With CType(oDB, NGLMASCarrierDataContext)
            .CarrierTariffEquipMatrixDetails.InsertAllOnSubmit(CType(LinqTable, LTS.CarrierTariffEquipMatrix).CarrierTariffEquipMatrixDetails)
        End With
    End Sub

    Protected Overrides Sub ProcessUpdatedDetails(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        With CType(oDB, NGLMASCarrierDataContext)
            ' Process any inserted detail records  
            .CarrierTariffEquipMatrixDetails.InsertAllOnSubmit(GetCarrTarEquipMatDetChanges(oData, TrackingInfo.Created))
            ' Process any updated bookload records
            .CarrierTariffEquipMatrixDetails.AttachAll(GetCarrTarEquipMatDetChanges(oData, TrackingInfo.Updated), True)
        End With
    End Sub

    Protected Function GetCarrTarEquipMatDetChanges(ByVal source As DataTransferObjects.CarrTarEquipMat, ByVal changeType As TrackingInfo) As List(Of LTS.CarrierTariffEquipMatrixDetail)
        ' Test record details for specified change type.
        ' If Updated is null, set to byte[0] (for inserted items).
        Dim details As IEnumerable(Of LTS.CarrierTariffEquipMatrixDetail) = (
                From d In source.CarrierTariffEquipMatrixDetails
                Where d.TrackingState = changeType
                Select New LTS.CarrierTariffEquipMatrixDetail With {.CarrTarEquipMatDetControl = d.CarrTarEquipMatDetControl _
                , .CarrTarEquipMatDetCarrTarEquipMatControl = d.CarrTarEquipMatDetCarrTarEquipMatControl _
                , .CarrTarEquipMatDetID = d.CarrTarEquipMatDetID _
                , .CarrTarEquipMatDetValue = d.CarrTarEquipMatDetValue _
                , .CarrTarEquipMatDetModDate = d.CarrTarEquipMatDetModDate _
                , .CarrTarEquipMatDetModUser = d.CarrTarEquipMatDetModUser _
                , .CarrTarEquipMatDetUpdated = If(d.CarrTarEquipMatDetUpdated Is Nothing, New Byte() {}, d.CarrTarEquipMatDetUpdated)})
        Return details.ToList()
    End Function

    Private Function ValidateEqupMatPivotKeyFields(ByRef sFs As String(),
                                                   ByRef CarrTarEquipControl As Integer,
                                                   ByRef TarRateTypeControl As Integer,
                                                   ByRef ClassTypeControl As Integer,
                                                   ByRef TarBracketTypeControl As Integer) As Boolean

        If sFs Is Nothing OrElse sFs.Count < 4 Then 'E_EquipMatPivotAllKeyFilterDetails
            throwInvalidKeyFilterMetaDataException(SqlFaultInfo.FaultDetailsKey.E_EquipMatPivotAllKeyFilterDetails, New List(Of String))
            Return False
        End If
        If Not Integer.TryParse(sFs(0), CarrTarEquipControl) Then
            throwInvalidKeyFilterMetaDataException(SqlFaultInfo.FaultDetailsKey.E_EquipMatPivotKeyFilterDetails, New List(Of String) From {"Equipment Control", sFs(0)})
            Return False
        End If
        If Not Integer.TryParse(sFs(1), TarRateTypeControl) Then
            throwInvalidKeyFilterMetaDataException(SqlFaultInfo.FaultDetailsKey.E_EquipMatPivotKeyFilterDetails, New List(Of String) From {"Rate Type Control", sFs(1)})
            Return False
        End If
        If Not Integer.TryParse(sFs(2), TarBracketTypeControl) Then
            throwInvalidKeyFilterMetaDataException(SqlFaultInfo.FaultDetailsKey.E_EquipMatPivotKeyFilterDetails, New List(Of String) From {"Bracket Type Control", sFs(2)})
            Return False
        End If
        If Not Integer.TryParse(sFs(3), ClassTypeControl) Then
            throwInvalidKeyFilterMetaDataException(SqlFaultInfo.FaultDetailsKey.E_EquipMatPivotKeyFilterDetails, New List(Of String) From {"Class Type Control", sFs(3)})
            Return False
        End If

        Return True
    End Function

    ''' <summary>
    ''' does the alt data key already exist.  if this is the first record, then it is always valid.
    ''' </summary>
    ''' <param name="db"></param>
    ''' <param name="EquipControl"></param>
    ''' <param name="RateTypeControl"></param>
    ''' <param name="BracketTypeControl"></param>
    ''' <param name="ClassTypeControl"></param>
    ''' <param name="CarrTarEquipMatName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function validateAltDataKey(ByRef db As NGLMASCarrierDataContext,
                                       ByVal EquipControl As Integer,
                                       ByVal RateTypeControl As Integer,
                                       ByVal BracketTypeControl As Integer,
                                       ByVal ClassTypeControl As Integer,
                                       ByVal CarrTarEquipMatName As String,
                                       ByVal CarrTarEquipMatCarrTarControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        Try
            If RateTypeControl = 1 Then 'distance rate
                Dim intExistsDist1 As Integer = (From d In db.CarrierTariffEquipMatrixes
                        Where d.CarrTarEquipMatCarrTarEquipControl = EquipControl _
                              And d.CarrTarEquipMatCarrTarControl = CarrTarEquipMatCarrTarControl _
                              And d.CarrTarEquipMatTarRateTypeControl = RateTypeControl
                        Select d).Count
                If intExistsDist1 > 0 Then ' this must be the first one, anything goes.
                    Return True
                End If
            Else
                Dim intExistsDist1 As Integer = (From d In db.CarrierTariffEquipMatrixes
                        Where d.CarrTarEquipMatCarrTarEquipControl = EquipControl _
                              And d.CarrTarEquipMatCarrTarControl = CarrTarEquipMatCarrTarControl _
                              And d.CarrTarEquipMatTarRateTypeControl = RateTypeControl _
                              And d.CarrTarEquipMatTarBracketTypeControl = BracketTypeControl _
                              And d.CarrTarEquipMatClassTypeControl = ClassTypeControl
                        Select d).Count
                If intExistsDist1 > 0 Then ' this must be the first one, anything goes.
                    Return True
                End If
            End If
            Return False

        Catch ex As Exception
            ManageLinqDataExceptions(ex, buildProcedureName("validateAltDataKey"))
        End Try
        Return blnRet
    End Function

    Public Function doesAltDataKeyExist(ByRef db As NGLMASCarrierDataContext,
                                        ByVal EquipControl As Integer,
                                        ByVal RateTypeControl As Integer,
                                        ByVal BracketTypeControl As Integer,
                                        ByVal ClassTypeControl As Integer,
                                        ByVal CarrTarEquipMatName As String,
                                        ByVal CarrTarEquipMatCarrTarControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        Try
            ' Using db As New NGLMASCarrierDataContext(ConnectionString)
            If RateTypeControl = 1 Then 'distance rate
                Dim intExistsDist1 As Integer = (From d In db.CarrierTariffEquipMatrixes
                        Where d.CarrTarEquipMatCarrTarEquipControl = EquipControl _
                              And d.CarrTarEquipMatCarrTarControl = CarrTarEquipMatCarrTarControl _
                              And d.CarrTarEquipMatTarRateTypeControl = RateTypeControl
                        Select d).Count
                If intExistsDist1 > 0 Then ' this must be the first one, anything goes.
                    Return True
                End If
            Else
                Dim intExistsDist1 As Integer = (From d In db.CarrierTariffEquipMatrixes
                        Where d.CarrTarEquipMatCarrTarEquipControl = EquipControl _
                              And d.CarrTarEquipMatCarrTarControl = CarrTarEquipMatCarrTarControl _
                              And d.CarrTarEquipMatTarRateTypeControl = RateTypeControl _
                              And d.CarrTarEquipMatTarBracketTypeControl = BracketTypeControl _
                              And d.CarrTarEquipMatClassTypeControl = ClassTypeControl
                        Select d).Count
                If intExistsDist1 > 0 Then ' this must be the first one, anything goes.
                    Return True
                End If
            End If
            Return False
            ''all of the others
            'Dim intExists1 As Integer = (From d In db.CarrierTariffEquipMatrixes _
            '                                        Where d.CarrTarEquipMatCarrTarEquipControl = EquipControl _
            '                                        And d.CarrTarEquipMatCarrTarControl = CarrTarEquipMatCarrTarControl _
            '                                          And d.CarrTarEquipMatTarRateTypeControl = RateTypeControl _
            '                                        And d.CarrTarEquipMatTarBracketTypeControl = BracketTypeControl _
            '                                        And d.CarrTarEquipMatClassTypeControl = ClassTypeControl _
            '                                        Select d).Count
            'If intExists1 = 0 Then ' this must be the first one, anything goes.
            '    Return True
            'End If
            ''db.Log = New DebugTextWriter
            ''Return a record with the same configuration but a different name
            ''only one named configuration is allowed per equipment
            'Dim intExists As Integer = (From d In db.CarrierTariffEquipMatrixes _
            '                                        Where d.CarrTarEquipMatCarrTarEquipControl = EquipControl _
            '                                        And d.CarrTarEquipMatCarrTarControl = CarrTarEquipMatCarrTarControl _
            '                                        And d.CarrTarEquipMatTarRateTypeControl = RateTypeControl _
            '                                        And d.CarrTarEquipMatTarBracketTypeControl = BracketTypeControl _
            '                                        And d.CarrTarEquipMatClassTypeControl = ClassTypeControl _
            '                                        And d.CarrTarEquipMatName.ToUpper <> CarrTarEquipMatName.ToUpper _
            '                                        Select d).Distinct.Count

            'If intExists = 0 Then blnRet = True
            '  End Using
        Catch ex As Exception
            ManageLinqDataExceptions(ex, buildProcedureName("validateAltDataKey"))
        End Try
        Return blnRet
    End Function

#End Region

End Class