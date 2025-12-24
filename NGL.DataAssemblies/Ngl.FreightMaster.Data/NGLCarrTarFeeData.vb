Imports System.Data.Linq
Imports System.ServiceModel
Imports Ngl.FreightMaster.Data.DataTransferObjects
Imports SerilogTracing

Public Class NGLCarrTarFeeData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMASCarrierDataContext(ConnectionString)
        Me.LinqTable = db.CarrierTariffFees
        Me.LinqDB = db
        Me.SourceClass = "NGLCarrTarFeeData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASCarrierDataContext(ConnectionString)
                _LinqTable = db.CarrierTariffFees
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
        Return GetCarrTarFeeFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    Public Function GetCarrTarFeeFiltered(ByVal Control As Integer) As DataTransferObjects.CarrTarFee
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim oRecord As DataTransferObjects.CarrTarFee = (
                        From d In db.CarrierTariffFees
                        Where
                        d.CarrTarFeesControl = Control
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

    Public Function GetCarrTarFeesFiltered(ByVal CarrTarControl As Integer,
                                           Optional ByVal page As Integer = 1,
                                           Optional ByVal pagesize As Integer = 1000) As DataTransferObjects.CarrTarFee()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try


                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1

                'db.Log = New DebugTextWriter

                Dim oQuery = From d In db.CarrierTariffFees
                             Where d.CarrTarFeesCarrTarControl = CarrTarControl
                             Select d

                intRecordCount = oQuery.Count
                If intRecordCount < 1 Then Return Nothing
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize

                Dim oRecords() As DataTransferObjects.CarrTarFee = (
                        From d In oQuery
                        Order By d.CarrTarFeesCaption
                        Select selectDTOData(d, page, intPageCount, intRecordCount, pagesize)
                        ).Skip(intSkip).Take(pagesize).ToArray()

                Return oRecords

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetCarrTarFeesFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Returns a list of Tariff Fees used for costing out the load.  The CarrTarFeesNotSupported
    ''' flag determines if the Tariff matches the profile specific requirements for the lane.
    ''' All fees where CarrTarFeesAccessorialProfileSpecific is false are returned but only
    ''' Fees that match the lane are returned when CarrTarFeesAccessorialProfileSpecific is true.
    ''' </summary>
    ''' <param name="LaneControl"></param>
    ''' <param name="CarrTarControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR on 4/22/2019 for v-8.2 
    '''   Added logic to use records in BookAccessorial table as order specific profile fees
    '''   So we need to pass in a BookControl number
    ''' </remarks>
    Public Function GetCarrTarFeesFilteredByLaneProfile(ByVal LaneControl As Integer, ByVal CarrTarControl As Integer, ByVal BookControl As Integer) As List(Of CarrTarFee)
        Using operation = Logger.StartActivity("GetCarrTarFeesFilteredByLaneProfile for Lane:{LaneControl}, CarrTarControl:{CarrTarControl}, BookControl: {BookControl}", LaneControl, CarrTarControl, BookControl)

            Using db As New NGLMASCarrierDataContext(ConnectionString)
                Try
                    Dim results = (From d In db.spGetTarFeesByLaneProfile(LaneControl, CarrTarControl, BookControl) Select selectDTOData(d)).ToList()
                    operation.Complete()
                    Return results
                Catch ex As Exception
                    operation.Complete()
                    Logger.Error(ex, "Error in GetCarrTarFeesFilteredByLaneProfile")

                End Try
                operation.Complete()
                Return Nothing
            End Using
        End Using
    End Function

    Public Function GetCarrTarFeesWithProfileFees(ByVal ProfileFees As List(Of BookFee),
                                                  ByVal CarrTarControl As Integer) As List(Of CarrTarFee)
        Using operation = Logger.StartActivity("GetCarrTarFeesWithProfileFees(ProfileFees: {@ProfileFees}, CarrTarControl: {CarrTarControl})", ProfileFees, CarrTarControl)

            Dim mergedFees As New List(Of CarrTarFee)
            Using db As New NGLMASCarrierDataContext(ConnectionString)
                Try
                    If Not ProfileFees Is Nothing AndAlso ProfileFees.Count > 0 Then
                        mergedFees = (From d In ProfileFees Select New DataTransferObjects.CarrTarFee With {
                            .CarrTarFeesCarrTarControl = CarrTarControl,
                            .CarrTarFeesMinimum = d.BookFeesMinimum,
                            .CarrTarFeesVariable = d.BookFeesVariable,
                            .CarrTarFeesAccessorialCode = d.BookFeesAccessorialCode,
                            .CarrTarFeesVariableCode = d.BookFeesVariableCode,
                            .CarrTarFeesVisible = d.BookFeesVisible,
                            .CarrTarFeesAutoApprove = d.BookFeesAutoApprove,
                            .CarrTarFeesAllowCarrierUpdates = d.BookFeesAllowCarrierUpdates,
                            .CarrTarFeesCaption = d.BookFeesCaption,
                            .CarrTarFeesEDICode = d.BookFeesEDICode,
                            .CarrTarFeesTaxable = d.BookFeesTaxable,
                            .CarrTarFeesIsTax = d.BookFeesIsTax,
                            .CarrTarFeesTaxSortOrder = d.BookFeesTaxSortOrder,
                            .CarrTarFeesBOLText = d.BookFeesBOLText,
                            .CarrTarFeesBOLPlacement = d.BookFeesBOLPlacement,
                            .CarrTarFeesAccessorialFeeAllocationTypeControl = d.BookFeesAccessorialFeeAllocationTypeControl,
                            .CarrTarFeesTarBracketTypeControl = d.BookFeesTarBracketTypeControl,
                            .CarrTarFeesAccessorialFeeCalcTypeControl = d.BookFeesAccessorialFeeCalcTypeControl,
                            .CarrTarFeesAccessorialProfileSpecific = True,
                            .NotSupported = True,
                            .CarrTarFeesModUser = Me.Parameters.UserName,
                            .CarrTarFeesModDate = Date.Now()}).ToList()
                    End If
                    Dim tarFees As List(Of LTS.CarrierTariffFee) = db.CarrierTariffFees.Where(Function(x) x.CarrTarFeesCarrTarControl = CarrTarControl).ToList()
                    If Not tarFees Is Nothing AndAlso tarFees.Count > 0 Then
                        For Each f In tarFees
                            If f.CarrTarFeesAccessorialProfileSpecific = False Then
                                If Not mergedFees.Any(Function(x) x.CarrTarFeesAccessorialCode = f.CarrTarFeesAccessorialCode) Then
                                    'if a merge fee does not exist for this accessorial code add it.
                                    Logger.Information("Adding new CarrTarFee for Accessorial Code: {AccessorialCode}", f.CarrTarFeesAccessorialCode)
                                    mergedFees.Add(selectDTOData(f))
                                Else
                                    'update it as supported
                                    Dim modFee = mergedFees.Where(Function(x) x.CarrTarFeesAccessorialCode = f.CarrTarFeesAccessorialCode).FirstOrDefault()
                                    If Not modFee Is Nothing Then
                                        copyLTStoDTOData(modFee, f)
                                        With modFee
                                            .NotSupported = False
                                            .CarrTarFeesModDate = Date.Now()
                                            .CarrTarFeesModUser = Me.Parameters.UserName
                                        End With
                                        Logger.Information("Merging CarrTarFee for Accessorial Code: {AccessorialCode}", f.CarrTarFeesAccessorialCode)
                                    End If
                                End If
                            Else
                                If mergedFees.Any(Function(x) x.CarrTarFeesAccessorialCode = f.CarrTarFeesAccessorialCode) Then
                                    'if record exists with the same accessorial code update it with the lts data
                                    Dim modFee = mergedFees.Where(Function(x) x.CarrTarFeesAccessorialCode = f.CarrTarFeesAccessorialCode).FirstOrDefault()
                                    If Not modFee Is Nothing Then
                                        copyLTStoDTOData(modFee, f)
                                        With modFee
                                            .NotSupported = False
                                            .CarrTarFeesModDate = Date.Now()
                                            .CarrTarFeesModUser = Me.Parameters.UserName
                                        End With
                                        Logger.Information("Merging CarrTarFee for Accessorial Code: {AccessorialCode}", f.CarrTarFeesAccessorialCode)
                                    End If
                                End If
                            End If
                        Next
                    End If

                Catch ex As Exception
                    ManageLinqDataExceptions(ex, buildProcedureName("GetCarrTarFeesWithProfileFees"))
                End Try

                Return mergedFees
            End Using

        End Using
    End Function


    Public Function GetCarrTarFeeNodes(ByVal CarrTarControl As Integer, ByVal ParentTreeID As Integer) As List(Of NGLTreeNode)
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'db.Log = New DebugTextWriter
                Dim oNodes As List(Of NGLTreeNode) = (
                        From d In db.CarrierTariffFees
                        Where (d.CarrTarFeesCarrTarControl = CarrTarControl)
                        Order By d.CarrTarFeesCaption
                        Select New DataTransferObjects.NGLTreeNode With {.Control = d.CarrTarFeesControl,
                        .ParentTreeID = ParentTreeID,
                        .Name = d.CarrTarFeesCaption,
                        .Description = d.CarrTarFeesEDICode,
                        .ClassName = "CarrTarFee"}).ToList

                Return oNodes

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                Return Nothing
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetCarrTarFeeNodes"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetCarrTarFeeTree(ByVal CarrTarControl As Integer, ByVal ParentTreeID As Integer, ByRef intNextTreeID As Integer) As List(Of NGLTreeNode)
        Try
            Dim oTreeNodes As List(Of NGLTreeNode) = GetCarrTarFeeNodes(CarrTarControl, ParentTreeID)
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
            throwUnExpectedFaultException(ex, buildProcedureName("GetCarrTarFeeTree"))
        End Try
        Return Nothing
    End Function

    Public Function GetCarrTarFeeTreeFlat(ByVal CarrTarControl As Integer, ByVal ParentTreeID As Integer, ByRef intNextTreeID As Integer) As List(Of NGLTreeNode)
        Try

            Dim oTreeNodes As List(Of NGLTreeNode) = GetCarrTarFeeNodes(CarrTarControl, ParentTreeID)
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
            throwUnExpectedFaultException(ex, buildProcedureName("GetCarrTarFeeTreeFlat"))
        End Try
        Return Nothing
    End Function

#End Region


#Region "LTS carrier tariff fee data"


    ''' <summary>
    ''' Returns the Fee data assoicated with a Contract.
    ''' A CarrTarFeesControl filter  or the  CarrTarControl value must be provided in the filters.ParentControl
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 01/02/2019
    '''   new LTS carrier tariff Fee query  
    ''' </remarks>
    Public Function GetCarrierTariffFees(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vCarrierTariffFee()
        If filters Is Nothing Then Return Nothing
        Dim iCarrTarFeesControl As Integer = 0
        Dim iCarrTarFeesCarrTarControl As Integer = 0
        Dim filterWhere As String = ""
        Dim sFilterSpacer As String = ""
        If Not filters.addParentFilterIfNoRecordControlFilter("CarrTarFeesControl", "CarrTarFeesCarrTarControl", iCarrTarFeesControl, iCarrTarFeesCarrTarControl, filterWhere, sFilterSpacer) Then
            throwNoDataFaultMessage("E_MissingParent") ' "The reference to the parent record is missing. Please select a valid parent record and try again."
        End If
        If iCarrTarFeesCarrTarControl = 0 And iCarrTarFeesControl = 0 Then
            'we do not have a valid filter
            throwNoDataFaultMessage("E_InvalidRecordKey") ' "The reference to the record is missing. Please select another record and try again."
        End If

        Dim oRet() As LTS.vCarrierTariffFee

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                Dim iQuery As IQueryable(Of LTS.vCarrierTariffFee)
                iQuery = db.vCarrierTariffFees
                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "CarrTarFeesCaption"
                    filters.sortDirection = "asc"
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrierTariffFees"), db)
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    '''  Insert or Update the carrier tariff Fees data
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 01/02/2019
    ''' </remarks>
    Public Function SaveCarrierTariffFee(ByVal oData As LTS.vCarrierTariffFee) As Boolean
        Dim blnRet As Boolean = False
        If oData Is Nothing Then Return False 'nothing to do
        Dim iCarrTarControl As Integer = If(oData.CarrTarFeesCarrTarControl, 0)
        Dim iCarrTarFeesAccessorialCode As Integer = If(oData.CarrTarFeesAccessorialCode, 0)
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                'verify the service contract
                If iCarrTarControl = 0 Then
                    Dim lDetails As New List(Of String) From {"Carrier Contract Reference", " was not provided and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                If Not db.CarrierTariffs.Any(Function(x) x.CarrTarControl = iCarrTarControl) Then

                    Dim lDetails As New List(Of String) From {"Carrier Contract Reference", " was not found and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                'check for existing Accessorial Code
                If iCarrTarFeesAccessorialCode = 0 Then
                    Dim lDetails As New List(Of String) From {"Accessorial Code", " is not valid and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                If Not db.tblAccessorialRefCarriers.Any(Function(x) x.AccessorialCode = iCarrTarFeesAccessorialCode) Then

                    Dim lDetails As New List(Of String) From {"Accessorial Code", " is no longer available and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                'check for the break point control
                If oData.CarrTarFeesTarBracketTypeControl = 0 Then
                    'look it up using the Equipment Matrix it must always match
                    oData.CarrTarFeesTarBracketTypeControl = db.CarrierTariffEquipMatrixes.Where(Function(x) x.CarrTarEquipMatCarrTarControl = iCarrTarControl).Select(Function(x) x.CarrTarEquipMatTarBracketTypeControl).FirstOrDefault()
                End If


                If oData.CarrTarFeesControl = 0 Then
                    Dim oNew As New LTS.CarrierTariffFee()
                    Dim strMSG As String = ""
                    Dim skipObjs As New List(Of String) From {"CarrTarFeesModUser", "CarrTarFeesModDate", "CarrTarFeesUpdated"}
                    oNew = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(oNew, oData, skipObjs, strMSG)
                    With oNew
                        .CarrTarFeesModDate = Date.Now
                        .CarrTarFeesModUser = Me.Parameters.UserName
                    End With
                    'this is a new fee so get the default accessorial data not entered by the user
                    Dim oAccessorial = db.tblAccessorialRefCarriers.Where(Function(x) x.AccessorialCode = iCarrTarFeesAccessorialCode).FirstOrDefault()
                    If Not oAccessorial Is Nothing Then
                        With oNew
                            .CarrTarFeesVisible = oAccessorial.AccessorialVisible
                            .CarrTarFeesAutoApprove = oAccessorial.AccessorialAutoApprove
                            .CarrTarFeesAllowCarrierUpdates = oAccessorial.AccessorialAllowCarrierUpdates
                            .CarrTarFeesCaption = oAccessorial.AccessorialCaption
                            .CarrTarFeesTaxable = oAccessorial.AccessorialTaxable
                            .CarrTarFeesIsTax = oAccessorial.AccessorialIsTax
                            .CarrTarFeesTaxSortOrder = oAccessorial.AccessorialTaxSortOrder
                            .CarrTarFeesBOLText = oAccessorial.AccessorialBOLText
                            .CarrTarFeesBOLPlacement = oAccessorial.AccessorialBOLPlacement
                            .CarrTarFeesAccessorialFeeAllocationTypeControl = oAccessorial.AccessorialAccessorialFeeAllocationTypeControl
                            .CarrTarFeesAccessorialFeeCalcTypeControl = oAccessorial.AccessorialAccessorialFeeCalcTypeControl
                            .CarrTarFeesAccessorialProfileSpecific = oAccessorial.AccessorialProfileSpecific
                            .CarrTarFeesTarBracketTypeControl = oAccessorial.AccessorialTarBracketTypeControl
                        End With
                        'check for the break point control
                        If oNew.CarrTarFeesTarBracketTypeControl = 0 Then
                            'look it up using the Equipment Matrix it must always match
                            oNew.CarrTarFeesTarBracketTypeControl = db.CarrierTariffEquipMatrixes.Where(Function(x) x.CarrTarEquipMatCarrTarControl = iCarrTarControl).Select(Function(x) x.CarrTarEquipMatTarBracketTypeControl).FirstOrDefault()
                        End If
                    End If
                    If Debugger.IsAttached AndAlso Not String.IsNullOrWhiteSpace(strMSG) Then
                        System.Diagnostics.Debug.WriteLine(strMSG)
                    End If
                    db.CarrierTariffFees.InsertOnSubmit(oNew)
                Else
                    Dim oExisting = db.CarrierTariffFees.Where(Function(x) x.CarrTarFeesControl = oData.CarrTarFeesControl).FirstOrDefault()
                    If oExisting Is Nothing OrElse oExisting.CarrTarFeesControl = 0 Then
                        throwRecordDeletedFaultException("Cannot save changes for tariff Fee: " & oData.CarrTarFeesCaption)
                    End If
                    Dim strMSG As String = ""
                    Dim skipObjs As New List(Of String) From {"CarrTarFeesModUser", "CarrTarFeesModDate", "CarrTarFeesUpdated"}
                    oExisting = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(oExisting, oData, skipObjs, strMSG)
                    With oExisting
                        .CarrTarFeesModDate = Date.Now
                        .CarrTarFeesModUser = Me.Parameters.UserName
                    End With
                End If
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveCarrierTariffFee"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Delete the provided carrier tariff Fee
    ''' </summary>
    ''' <param name="iControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 01/02/2019
    ''' </remarks>
    Public Function DeleteCarrierTariffFee(ByVal iControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        If iControl = 0 Then Return False 'nothing to do
        'validate before delete

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Delete the Record
                Dim oToDelete = db.CarrierTariffFees.Where(Function(x) x.CarrTarFeesControl = iControl).FirstOrDefault()
                If oToDelete Is Nothing OrElse oToDelete.CarrTarFeesControl = 0 Then Return True 'already deleted
                db.CarrierTariffFees.DeleteOnSubmit(oToDelete)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteCarrierTariffFee"), db)
            End Try
        End Using
        Return blnRet
    End Function


#End Region

#Region "Protected Functions"

    'Modified By LVV on 9/28/16 for v-7.0.5.110 HDM Enhancement
    'Added CarTarFeesMaximum
    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DataTransferObjects.CarrTarFee)
        'Create New Record
        Return New LTS.CarrierTariffFee With {.CarrTarFeesControl = d.CarrTarFeesControl,
            .CarrTarFeesCarrTarControl = d.CarrTarFeesCarrTarControl,
            .CarrTarFeesMinimum = d.CarrTarFeesMinimum,
            .CarrTarFeesMaximum = d.CarrTarFeesMaximum,
            .CarrTarFeesVariable = d.CarrTarFeesVariable,
            .CarrTarFeesAccessorialCode = d.CarrTarFeesAccessorialCode,
            .CarrTarFeesVariableCode = d.CarrTarFeesVariableCode,
            .CarrTarFeesVisible = d.CarrTarFeesVisible,
            .CarrTarFeesAutoApprove = d.CarrTarFeesAutoApprove,
            .CarrTarFeesAllowCarrierUpdates = d.CarrTarFeesAllowCarrierUpdates,
            .CarrTarFeesCaption = d.CarrTarFeesCaption,
            .CarrTarFeesEDICode = d.CarrTarFeesEDICode,
            .CarrTarFeesTaxable = d.CarrTarFeesTaxable,
            .CarrTarFeesIsTax = d.CarrTarFeesIsTax,
            .CarrTarFeesTaxSortOrder = d.CarrTarFeesTaxSortOrder,
            .CarrTarFeesBOLText = d.CarrTarFeesBOLText,
            .CarrTarFeesBOLPlacement = d.CarrTarFeesBOLPlacement,
            .CarrTarFeesAccessorialFeeAllocationTypeControl = d.CarrTarFeesAccessorialFeeAllocationTypeControl,
            .CarrTarFeesTarBracketTypeControl = d.CarrTarFeesTarBracketTypeControl,
            .CarrTarFeesAccessorialFeeCalcTypeControl = d.CarrTarFeesAccessorialFeeCalcTypeControl,
            .CarrTarFeesPreCloneControl = d.CarrTarFeesPreCloneControl,
            .CarrTarFeesAccessorialProfileSpecific = d.CarrTarFeesAccessorialProfileSpecific,
            .CarrTarFeesModUser = Parameters.UserName,
            .CarrTarFeesModDate = Date.Now,
            .CarrTarFeesUpdated = If(d.CarrTarFeesUpdated Is Nothing, New Byte() {}, d.CarrTarFeesUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCarrTarFeeFiltered(Control:=CType(LinqTable, LTS.CarrierTariffFee).CarrTarFeesControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim source As LTS.CarrierTariffFee = TryCast(LinqTable, LTS.CarrierTariffFee)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.CarrierTariffFees
                       Where d.CarrTarFeesControl = source.CarrTarFeesControl
                       Select New DataTransferObjects.QuickSaveResults With {.Control = d.CarrTarFeesControl _
                           , .ModDate = d.CarrTarFeesModDate _
                           , .ModUser = d.CarrTarFeesModUser _
                           , .Updated = d.CarrTarFeesUpdated.ToArray}).First

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

    Private Sub ValidateApproved(ByRef oDB As NGLMASCarrierDataContext, ByRef oData As DataTransferObjects.CarrTarFee)
        DirectCast(NDPBaseClassFactory("NGLCarrTarContractData", False), NGLCarrTarContractData).ValidateApproved(oData.CarrTarFeesCarrTarControl, oDB)
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

    'Friend Function selectDTOData(ByVal d As LTS.CarrierTariffFee, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.CarrTarFee
    '    Return New DTO.CarrTarFee With {.CarrTarFeesControl = d.CarrTarFeesControl, _
    '                                               .CarrTarFeesCarrTarControl = If(d.CarrTarFeesCarrTarControl, 0), _
    '                                               .CarrTarFeesMinimum = If(d.CarrTarFeesMinimum, 0), _
    '                                               .CarrTarFeesVariable = If(d.CarrTarFeesVariable, 0), _
    '                                               .CarrTarFeesAccessorialCode = If(d.CarrTarFeesAccessorialCode, 0), _
    '                                               .CarrTarFeesVariableCode = If(d.CarrTarFeesVariableCode, 0), _
    '                                               .CarrTarFeesVisible = d.CarrTarFeesVisible, _
    '                                               .CarrTarFeesAutoApprove = d.CarrTarFeesAutoApprove, _
    '                                               .CarrTarFeesAllowCarrierUpdates = d.CarrTarFeesAllowCarrierUpdates, _
    '                                               .CarrTarFeesCaption = d.CarrTarFeesCaption, _
    '                                               .CarrTarFeesEDICode = d.CarrTarFeesEDICode, _
    '                                               .CarrTarFeesTaxable = d.CarrTarFeesTaxable, _
    '                                               .CarrTarFeesIsTax = d.CarrTarFeesIsTax, _
    '                                               .CarrTarFeesTaxSortOrder = d.CarrTarFeesTaxSortOrder, _
    '                                               .CarrTarFeesBOLText = d.CarrTarFeesBOLText, _
    '                                               .CarrTarFeesBOLPlacement = d.CarrTarFeesBOLPlacement, _
    '                                               .CarrTarFeesAccessorialFeeAllocationTypeControl = d.CarrTarFeesAccessorialFeeAllocationTypeControl, _
    '                                               .CarrTarFeesTarBracketTypeControl = d.CarrTarFeesTarBracketTypeControl, _
    '                                               .CarrTarFeesAccessorialFeeCalcTypeControl = d.CarrTarFeesAccessorialFeeCalcTypeControl, _
    '                                               .CarrTarFeesPreCloneControl = d.CarrTarFeesPreCloneControl, _
    '                                               .CarrTarFeesAccessorialProfileSpecific = d.CarrTarFeesAccessorialProfileSpecific, _
    '                                               .CarrTarFeesModUser = d.CarrTarFeesModUser, _
    '                                               .CarrTarFeesModDate = d.CarrTarFeesModDate, _
    '                                               .CarrTarFeesUpdated = d.CarrTarFeesUpdated.ToArray(), _
    '                                               .Page = page, _
    '                                               .Pages = pagecount, _
    '                                               .RecordCount = recordcount, _
    '                                               .PageSize = pagesize}
    'End Function

    'Modified By LVV on 9/28/16 for v-7.0.5.110 HDM Enhancement
    'Added CarTarFeesMaximum
    Friend Shared Function selectDTOData(ByVal d As LTS.spGetTarFeesByLaneProfileResult, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.CarrTarFee
        Dim oDTO As New DataTransferObjects.CarrTarFee
        Dim skipObjs As New List(Of String) From {"CarrTarFeesCarrTarControl",
                "CarrTarFeesMinimum",
                "CarrTarFeesMaximum",
                "CarrTarFeesVariable",
                "CarrTarFeesAccessorialCode",
                "CarrTarFeesVariableCode"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .CarrTarFeesCarrTarControl = If(d.CarrTarFeesCarrTarControl, 0)
            .CarrTarFeesMinimum = If(d.CarrTarFeesMinimum, 0)
            .CarrTarFeesMaximum = If(d.CarrTarFeesMaximum, 0)
            .CarrTarFeesVariable = If(d.CarrTarFeesVariable, 0)
            .CarrTarFeesAccessorialCode = If(d.CarrTarFeesAccessorialCode, 0)
            .CarrTarFeesVariableCode = If(d.CarrTarFeesVariableCode, 0)
            .CarrTarFeesUpdated = d.CarrTarFeesUpdated.ToArray()
            .NotSupported = d.CarrTarFeesNotSupported
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oDTO
    End Function

    Friend Shared Function selectDTOData(ByVal d As LTS.CarrierTariffFee, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.CarrTarFee
        Dim oDTO As New DataTransferObjects.CarrTarFee
        copyLTStoDTOData(oDTO, d)
        With oDTO
            .CarrTarFeesUpdated = d.CarrTarFeesUpdated.ToArray()
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oDTO
    End Function

    'Modified By LVV on 9/28/16 for v-7.0.5.110 HDM Enhancement
    'Added CarTarFeesMaximum
    Friend Shared Sub copyLTStoDTOData(ByRef d As DataTransferObjects.CarrTarFee, ByVal f As LTS.CarrierTariffFee)

        Dim skipObjs As New List(Of String) From {"CarrTarFeesCarrTarControl",
                "CarrTarFeesMinimum",
                "CarrTarFeesMaximum",
                "CarrTarFeesVariable",
                "CarrTarFeesAccessorialCode",
                "CarrTarFeesVariableCode"}
        d = CopyMatchingFields(d, f, skipObjs)
        'add custom formatting
        With d
            .CarrTarFeesCarrTarControl = If(f.CarrTarFeesCarrTarControl, 0)
            .CarrTarFeesMinimum = If(f.CarrTarFeesMinimum, 0)
            .CarrTarFeesMaximum = If(f.CarrTarFeesMaximum, 0)
            .CarrTarFeesVariable = If(f.CarrTarFeesVariable, 0)
            .CarrTarFeesAccessorialCode = If(f.CarrTarFeesAccessorialCode, 0)
            .CarrTarFeesVariableCode = If(f.CarrTarFeesVariableCode, 0)
        End With

    End Sub

    'Modified By LVV on 9/28/16 for v-7.0.5.110 HDM Enhancement
    'Added CarTarFeesMaximum
    Friend Shared Sub copyDTOtoLTSData(ByRef f As LTS.CarrierTariffFee, ByVal d As DataTransferObjects.CarrTarFee)

        Dim skipObjs As New List(Of String) From {"CarrTarFeesCarrTarControl",
                "CarrTarFeesMinimum",
                "CarrTarFeesMaximum",
                "CarrTarFeesVariable",
                "CarrTarFeesAccessorialCode",
                "CarrTarFeesVariableCode"}
        f = CopyMatchingFields(f, d, skipObjs)
        'add custom formatting
        With f
            .CarrTarFeesCarrTarControl = d.CarrTarFeesCarrTarControl
            .CarrTarFeesMinimum = d.CarrTarFeesMinimum
            .CarrTarFeesMaximum = d.CarrTarFeesMaximum
            .CarrTarFeesVariable = d.CarrTarFeesVariable
            .CarrTarFeesAccessorialCode = d.CarrTarFeesAccessorialCode
            .CarrTarFeesVariableCode = d.CarrTarFeesVariableCode
        End With

    End Sub

#End Region

End Class