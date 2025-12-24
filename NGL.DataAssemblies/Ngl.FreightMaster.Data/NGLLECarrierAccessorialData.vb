Imports Ngl.FreightMaster.Data.LTS
Imports SerilogTracing

Public Class NGLLECarrierAccessorialData : Inherits NGLLinkDataBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMASCarrierDataContext(ConnectionString)
        Me.LinqTable = db.tblLECarrierAccessorials
        Me.LinqDB = db
        Me.SourceClass = "NGLLECarrierAccessorialData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASCarrierDataContext(ConnectionString)
                Me.LinqTable = db.tblLECarrierAccessorials
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

    ''' <summary>
    ''' Replaces CarrierDataProvider.GetCarrierLegalAccessorialXref
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="LEControl"></param>
    ''' <param name="CarrierControl"></param>
    ''' <param name="filterWhere"></param>
    ''' <param name="sortExpression"></param>
    ''' <param name="page"></param>
    ''' <param name="pagesize"></param>
    ''' <param name="skip"></param>
    ''' <param name="take"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 6/18/18 for v-8.2 VSTS #337
    '''  Renamed tblCarrierLegalAccessorialXref to tblLECarrierAccessorial
    '''  Replaces CarrierDataProvider.GetCarrierLegalAccessorialXref
    '''  Changed vCarrierLegalAccessorialXref to vLECarrierAccessorial
    ''' </remarks>
    Public Function GetLECarrierAccessorial(ByRef RecordCount As Integer,
                                            ByVal LEControl As Integer,
                                            ByVal CarrierControl As Integer,
                                            Optional ByVal filterWhere As String = "",
                                            Optional ByVal sortExpression As String = "LECAControl Desc",
                                            Optional ByVal page As Integer = 1,
                                            Optional ByVal pagesize As Integer = 1000,
                                            Optional ByVal skip As Integer = 0,
                                            Optional ByVal take As Integer = 0) As LTS.vLECarrierAccessorial()
        Dim oRetData As LTS.vLECarrierAccessorial()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim intPageCount As Integer = 1
                'Filter By LE and Carrier
                Dim oQuery = (From t In db.vLECarrierAccessorials
                              Where t.LEAdminControl = LEControl And t.CarrierControl = CarrierControl
                              Select t)
                If oQuery Is Nothing Then Return Nothing
                If Not String.IsNullOrEmpty(filterWhere) Then
                    'oQuery = oQuery.Where(Function(x) x.Code.StartsWith(filterWhere) Or x.Name.StartsWith(filterWhere))
                End If
                RecordCount = oQuery.Count()
                If RecordCount < 1 Then Return Nothing
                If take <> 0 Then
                    pagesize = take
                Else
                    'calculate based on page and pagesize
                    If pagesize < 1 Then pagesize = 1
                    If RecordCount < 1 Then RecordCount = 1
                    If page < 1 Then page = 1
                    skip = (page - 1) * pagesize
                End If
                If RecordCount > pagesize Then intPageCount = ((RecordCount - 1) \ pagesize) + 1
                oRetData = oQuery.OrderBy(sortExpression).Skip(skip).Take(pagesize).ToArray()
                Return oRetData
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLECarrierAccessorial"), db)
            End Try
            Return Nothing
        End Using
    End Function
    Public Function GetLECarrierAccessorialsByCarrierControl(CarrierControl As Integer) As tblLECarrierAccessorial()
        Dim results As tblLECarrierAccessorial() = New tblLECarrierAccessorial() {}
        Using Logger.StartActivity("GetLECarrierAccessorialsByCarrierControl(CarrierControl: {CarrierControl})", CarrierControl)

            Using db As New NGLMASCarrierDataContext(ConnectionString)
                Try


                    db.Log = New DebugTextWriter
                    Dim t = db.tblLegalEntityCarriers.FirstOrDefault(Function(x) x.LECarCarrierControl = CarrierControl)


                    results = t.tblLECarrierAccessorials.ToArray()

                    Logger.Information("LECarrierAccessorials {@LECarrierAccessorials}", results)
                Catch ex As Exception
                    Logger.Error(ex, "Error in GetLECarrierAccessorialsByCarrierControl")
                End Try
            End Using
        End Using

        Return results
    End Function

    ''' <summary>
    ''' Caller must set filters.ParentControl to LECALECarControl
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 12/20/18
    ''' Added Get method that could be used by a cm grid
    ''' </remarks>
    Public Function GetLECarrierAccessorials(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.vLECarrierAccessorial()
        Dim oRet As LTS.vLECarrierAccessorial()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Get the data iqueryable
                Dim iQuery As IQueryable(Of LTS.vLECarrierAccessorial)
                iQuery = db.vLECarrierAccessorials
                Dim filterWhere = " (LECALECarControl = " & filters.ParentControl & ") "
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLECarrierAccessorials"), db)
            End Try
            Return Nothing
        End Using
    End Function


    ''' <summary>
    ''' Replaces CarrierDataProvider.DeleteCarrierLegalAccessorialXref()
    ''' </summary>
    ''' <param name="Control"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 6/18/18 for v-8.2 VSTS #337
    '''  Renamed tblCarrierLegalAccessorialXref to tblLECarrierAccessorial
    '''  Replaces CarrierDataProvider.DeleteCarrierLegalAccessorialXref()
    ''' </remarks>
    Public Function DeleteLECarrierAccessorials(ByVal Control As Integer) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Dim oTable = db.tblLECarrierAccessorials
            Try
                Dim oRecord As LTS.tblLECarrierAccessorial = db.tblLECarrierAccessorials.Where(Function(x) x.LECAControl = Control).FirstOrDefault()
                If (oRecord Is Nothing OrElse oRecord.LECAControl = 0) Then Return False
                'oTable.Attach(oRecord, True)
                oTable.DeleteOnSubmit(oRecord)
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteLECarrierAccessorials"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Inserts Or Updates LECarrierAccessorial records
    ''' Replaces InsertOrUpdateCarrierLegalAccessorialXref()
    ''' </summary>
    ''' <param name="oRecord"></param>
    ''' <param name="LEAdminControl"></param>
    ''' <param name="CarrierControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 6/19/18 for v-8.2 VSTS #337
    ''' Renamed tblCarrierLegalAccessorialXref to tblLECarrierAccessorial
    ''' Replaces InsertOrUpdateCarrierLegalAccessorialXref
    ''' </remarks>
    Public Function InsertOrUpdateLECarrierAccessorial(ByVal oRecord As LTS.tblLECarrierAccessorial, ByVal LEAdminControl As Integer, ByVal CarrierControl As Integer) As Models.ResultObject
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Dim oResult As New Models.ResultObject()
            Try
                Dim leCarControl = db.tblLegalEntityCarriers.Where(Function(x) x.LECarLEAdminControl = LEAdminControl AndAlso x.LECarCarrierControl = CarrierControl).Select(Function(y) y.LECarControl).FirstOrDefault()
                oRecord.LECALECarControl = leCarControl
                oResult = InsertOrUpdateLECarrierAccessorial(oRecord)
                'Return db.spInsertOrUpdateLECarrierAccessorial(LEAdminControl, CarrierControl, AccessorialCode, AutoApprove, AllowCarrierUpdates, AccessorialVisible, Caption, EDICode, ApproveToleranceLow, ApproveToleranceHigh, ApproveTolerancePerLow, ApproveTolerancePerHigh, AverageValue, DynamicAverageValue).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertOrUpdateLECarrierAccessorial"), db)
            End Try
            Return oResult
        End Using
    End Function

    ''' <summary>
    ''' Inserts Or Updates a CarrierCont record based on if ContactControl = 0
    ''' </summary>
    ''' <param name="oRecord"></param>
    ''' <remarks>
    ''' Added By LVV on 10/04/18
    ''' Modified by RHR for v-8.2.1.003 on  11/19/2019
    '''     removed duplicate accessorial code validation 
    '''     we now support multiple records with the same accessorial code
    ''' </remarks>
    Public Function InsertOrUpdateLECarrierAccessorial(ByVal oRecord As LTS.tblLECarrierAccessorial) As Models.ResultObject
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Dim oResult As New Models.ResultObject()
            Try
                'Removed by RHR for v-8.2.1.003 on  11/19/2019
                ''Check to make sure a record with the key fields does not already exist
                'If db.tblLECarrierAccessorials.Any(Function(x) x.LECALECarControl = oRecord.LECALECarControl AndAlso x.LECAAccessorialCode = oRecord.LECAAccessorialCode) Then
                '    Dim carrier = "", accessorial = ""
                '    Dim a = db.vLECarrierAccessorials.Where(Function(x) x.LECALECarControl = oRecord.LECALECarControl AndAlso x.AccessorialCode = oRecord.LECAAccessorialCode).FirstOrDefault()
                '    If Not a Is Nothing Then carrier = a.CarrierName : accessorial = a.AccessorialName
                '    oResult.Success = False
                '    oResult.ErrTitle = getLocalizedString("ActionCannotBeCompleted", "Action Cannot Be Completed")
                '    oResult.ErrMsg = formatLocalizedString("W_LECarrierAccssrlAlreadyExists", "Carrier {0} already has a configuration for Accessorial {1}.", New String() {a.CarrierName, a.AccessorialName})
                '    Return oResult
                'End If
                oRecord.LECAModDate = Date.Now
                oRecord.LECAModUser = Parameters.UserName
                If oRecord.LECAControl = 0 Then
                    'Insert
                    db.tblLECarrierAccessorials.InsertOnSubmit(oRecord)
                Else
                    'Update
                    db.tblLECarrierAccessorials.Attach(oRecord, True)
                End If
                db.SubmitChanges()
                oResult.Success = True
                oResult.SuccessMsg = getLocalizedString("M_Success", "Success!")
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertOrUpdateLECarrierAccessorial"), db)
            End Try
            Return oResult
        End Using
    End Function


    ''' <summary>
    ''' DEPRECIATED By LVV on 3/19/20 v-8.2.1.006 - no longer used. Settlement FBDE screen now calls GetLECarFeesByAllocationType()
    ''' Replaces CarrierDataProvider.GetCLAXForSettlement
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="CompLE"></param>
    ''' <param name="CarrierControl"></param>
    ''' <param name="filterWhere"></param>
    ''' <param name="sortExpression"></param>
    ''' <param name="page"></param>
    ''' <param name="pagesize"></param>
    ''' <param name="skip"></param>
    ''' <param name="take"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 6/18/18 for v-8.2 VSTS #337
    '''  Renamed tblCarrierLegalAccessorialXref to tblLECarrierAccessorial
    '''  Replaces CarrierDataProvider.GetCLAXForSettlement()
    ''' Modified by RHR for v-8.2.0.117 we do not include fuel fees in this list
    ''' </remarks>
    Public Function GetLECAForSettlement(ByRef RecordCount As Integer,
                                         ByVal CompLE As String,
                                         ByVal CarrierControl As Integer,
                                         Optional ByVal filterWhere As String = "",
                                         Optional ByVal sortExpression As String = "CLAXControl Desc",
                                         Optional ByVal page As Integer = 1,
                                         Optional ByVal pagesize As Integer = 1000,
                                         Optional ByVal skip As Integer = 0,
                                         Optional ByVal take As Integer = 0) As LTS.vLECarrierAccessorial()
        throwDepreciatedException("This version of " & buildProcedureName("GetLECAForSettlement") & " has been Deprecated")
        ''Dim oRetData As LTS.vLECarrierAccessorial()
        ''Using db As New NGLMASCarrierDataContext(ConnectionString)
        ''    Try
        ''        Dim oLEA As New NGLLegalEntityAdminData(Parameters)
        ''        Dim f As New Models.AllFilters
        ''        Dim rc As Integer = 0
        ''        f.filterName = "LegalEntity"
        ''        f.filterValue = CompLE
        ''        Dim LE = oLEA.GetLEAdminsFiltered(rc, f).FirstOrDefault()
        ''        If LE Is Nothing Then Return Nothing
        ''        Dim intPageCount As Integer = 1
        ''        'Filter By LE and Carrier
        ''        Dim oQuery = (From t In db.vLECarrierAccessorials
        ''                      Where t.LEAdminControl = LE.LEAdminControl And t.CarrierControl = CarrierControl And t.AllowCarrierUpdates = True
        ''                      Select t)
        ''        Dim iQuery As IQueryable(Of LTS.vLECarrierAccessorial)
        ''        iQuery = db.vLECarrierAccessorials
        ''        If Not String.IsNullOrEmpty(filterWhere) Then
        ''            filterWhere &= " AND "
        ''        End If
        ''        filterWhere &= " (LEAdminControl = " & LE.LEAdminControl.ToString & " And CarrierControl = " & CarrierControl.ToString() & "And AllowCarrierUpdates = True )"
        ''        Dim ifilters = New Models.AllFilters()
        ''        If take <> 0 Then
        ''            pagesize = take
        ''        Else
        ''            'calculate based on page and pagesize
        ''            If pagesize < 1 Then pagesize = 1
        ''            If RecordCount < 1 Then RecordCount = 1
        ''            If page < 1 Then page = 1
        ''            skip = (page - 1) * pagesize
        ''        End If
        ''        ifilters.skip = skip
        ''        ifilters.take = take
        ''        ifilters.page = 0
        ''        ifilters.pageSize = pagesize
        ''        ApplyAllFilters(iQuery, ifilters, filterWhere)
        ''        PrepareQuery(iQuery, ifilters, RecordCount)
        ''        db.Log = New DebugTextWriter
        ''        oRetData = iQuery.Skip(ifilters.skip).Take(ifilters.take).ToArray()
        ''        Return oRetData
        ''    Catch ex As Exception
        ''        ManageLinqDataExceptions(ex, buildProcedureName("GetLECAForSettlement"), db)
        ''    End Try
        ''    Return Nothing
        ''End Using
    End Function

    ''' <summary>
    ''' Called by the REST service to populate the Fees ddls on the Settlement Detailed Freight Bill Entry Screen.
    ''' Gets the Fees based on the Legal Entity Carrier Accessorial Configuration as well as the Accessorial Allocation Type.
    ''' Does not include Fuel Accessorials.
    ''' Only returns Accessorials where AllowCarrierUpdates is true.
    ''' Required fields in AllFilters: LEAdminControl, CarrierControlFrom, ParentControl (AllocationType).
    ''' These fields are used to filter the initial IQueryable and come from the backend. The filters in 
    ''' FilterValues are from the user and can be applied in addition on top of the initial query results.
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters">
    ''' Required fields in AllFilters: LEAdminControl, CarrierControlFrom, ParentControl (AllocationType).
    ''' These fields are used to filter the initial IQueryable and come from the backend.
    ''' The filters in FilterValues are from the user and can be applied in addition on top of the initial query results.
    ''' </param>
    ''' <returns>LTS.vLECarrierAccessorial()</returns>
    ''' <remarks>Created By LVV on 3/19/20 for v-8.2.1.006
    ''' Modified by RHR on 5/4/2020 for v-8.2.1.007
    ''' Added new logic to allow carriers to select more fees 
    ''' any fee with Allocation Type 1 can be selected for all types of allocation
    ''' when looking for Order specific fees allocation of 1 we allow all of the other fees not just 1
    ''' </remarks>
    Public Function GetLECarFeesByAllocationType(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.vLECarrierAccessorial()
        Dim oRet As LTS.vLECarrierAccessorial()
        Using operation = Logger.StartActivity("GetLECarFeesByAllocationType(RecordCount: {RecordCount}, filters: {@filters})", RecordCount, filters)
            Using db As New NGLMASCarrierDataContext(ConnectionString)
                Try
                    Dim iQuery As IQueryable(Of LTS.vLECarrierAccessorial)
                    iQuery = (From t In db.vLECarrierAccessorials
                              Where
                                  t.LEAdminControl = filters.LEAdminControl And t.CarrierControl = filters.CarrierControlFrom _
                                  And
                                  t.AllowCarrierUpdates = True _
                                  And
                                  t.AccessorialCode <> 2 And t.AccessorialCode <> 9 And t.AccessorialCode <> 15 _
                                  And
                                  (
                                      t.AccessorialFeeAllocationTypeControl = 1 _
                                      Or
                                      t.AccessorialFeeAllocationTypeControl = filters.ParentControl _
                                      Or
                                      (
                                          filters.ParentControl = 1 _
                                          And
                                          (
                                              t.AccessorialFeeAllocationTypeControl = 2 _
                                              Or
                                              t.AccessorialFeeAllocationTypeControl = 3 _
                                              Or
                                              t.AccessorialFeeAllocationTypeControl = 4
                                              )
                                          )
                                      )
                              Select t)
                    Dim filterWhere = ""
                    db.Log = New DebugTextWriter
                    ApplyAllFilters(iQuery, filters, filterWhere)
                    PrepareQuery(iQuery, filters, RecordCount)
                    oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                    operation.Complete()
                    Return oRet
                Catch ex As Exception
                    Logger.Error(ex, "Error in GetLECarFeesByAllocationType")
                    ManageLinqDataExceptions(ex, buildProcedureName("GetLECarFeesByAllocationType"), db)
                End Try
            End Using

            Return Nothing
        End Using
    End Function


    Public Function GetLECAForSettlement_old(ByRef RecordCount As Integer,
                                             ByVal CompLE As String,
                                             ByVal CarrierControl As Integer,
                                             Optional ByVal filterWhere As String = "",
                                             Optional ByVal sortExpression As String = "CLAXControl Desc",
                                             Optional ByVal page As Integer = 1,
                                             Optional ByVal pagesize As Integer = 1000,
                                             Optional ByVal skip As Integer = 0,
                                             Optional ByVal take As Integer = 0) As LTS.vLECarrierAccessorial()
        Dim oRetData As LTS.vLECarrierAccessorial()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim oLEA As New NGLLegalEntityAdminData(Parameters)
                Dim f As New Models.AllFilters
                Dim rc As Integer = 0
                f.filterName = "LegalEntity"
                f.filterValue = CompLE
                Dim LE = oLEA.GetLEAdminsFiltered(rc, f).FirstOrDefault()
                If LE Is Nothing Then Return Nothing
                Dim intPageCount As Integer = 1
                'Filter By LE and Carrier
                Dim oQuery = (From t In db.vLECarrierAccessorials
                              Where t.LEAdminControl = LE.LEAdminControl And t.CarrierControl = CarrierControl And t.AllowCarrierUpdates = True
                              Select t)
                If oQuery Is Nothing Then Return Nothing
                If Not String.IsNullOrEmpty(filterWhere) Then
                    'oQuery = oQuery.Where(Function(x) x.Code.StartsWith(filterWhere) Or x.Name.StartsWith(filterWhere))
                End If
                RecordCount = oQuery.Count()
                If RecordCount < 1 Then Return Nothing
                If take <> 0 Then
                    pagesize = take
                Else
                    'calculate based on page and pagesize
                    If pagesize < 1 Then pagesize = 1
                    If RecordCount < 1 Then RecordCount = 1
                    If page < 1 Then page = 1
                    skip = (page - 1) * pagesize
                End If
                If RecordCount > pagesize Then intPageCount = ((RecordCount - 1) \ pagesize) + 1
                oRetData = oQuery.OrderBy(sortExpression).Skip(skip).Take(pagesize).ToArray()
                Return oRetData
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLECAForSettlement"), db)
            End Try
            Return Nothing
        End Using
    End Function

    Public Function CopyLECarrierAccessorialConfig(ByVal CopyFromLECarControl As Integer, ByVal CopyToLeCarControls() As Integer) As String
        Dim strRet As String = ""
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'verify that a CopyToLECar record was provided
                If CopyToLeCarControls?.Length < 1 Then
                    Dim lDetails As New List(Of String) From {"Copy To Carrier Record References", " were not provided and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return strRet
                End If
                'verify that a CopyFromLECar record exists
                If CopyFromLECarControl = 0 Then
                    Dim lDetails As New List(Of String) From {"Copy From Carrier Record Reference", " was not provided and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return strRet
                End If
                If Not db.tblLegalEntityCarriers.Any(Function(x) x.LECarControl = CopyFromLECarControl) Then
                    Dim lDetails As New List(Of String) From {"Copy From Carrier Record Reference", " was not found and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return strRet
                End If
                'Copy the configs
                For Each ct In CopyToLeCarControls
                    Dim spRet = db.spCopyLECarrierAccessorialConfig(ct, CopyFromLECarControl).FirstOrDefault()
                    Dim sSep As String = ""
                    If spRet.ErrNumber > 0 AndAlso Not String.IsNullOrWhiteSpace(spRet.RetMsg) Then
                        strRet += (sSep + spRet.RetMsg)
                        sSep = " "
                    End If
                Next
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("CopyLECarrierAccessorialConfig"), db)
            End Try
        End Using
        Return strRet
    End Function

    ''' <summary>
    ''' Copies all visible accessorial codes from tblAccessorial to tblLECarrierAccessorial using the default values from tblAccessorial
    ''' </summary>
    ''' <param name="CopyToLECarControls"></param>
    ''' <returns></returns>
    ''' <remarks>Added By LVV on 12/17/19 for v-8.2.1.004</remarks>
    Public Function CopyDefaultAccessorialsToLECarriers(ByVal CopyToLECarControls() As Integer) As String
        Dim strRet As String = ""
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'verify that a CopyToLECar record was provided
                If CopyToLECarControls?.Length < 1 Then
                    Dim lDetails As New List(Of String) From {"Copy To Carrier Record References", " were not provided and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return strRet
                End If
                'Copy the configs
                For Each ct In CopyToLECarControls
                    Dim spRet = db.spCopyDefaultAccessorialsToLECarrier(ct).FirstOrDefault()
                    Dim sSep As String = ""
                    If spRet.ErrNumber > 0 AndAlso Not String.IsNullOrWhiteSpace(spRet.RetMsg) Then
                        strRet += (sSep + spRet.RetMsg)
                        sSep = " "
                    End If
                Next
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("CopyDefaultAccessorialsToLECarriers"), db)
            End Try
        End Using
        Return strRet
    End Function


#End Region

#Region "Protected Functions"

#End Region

End Class