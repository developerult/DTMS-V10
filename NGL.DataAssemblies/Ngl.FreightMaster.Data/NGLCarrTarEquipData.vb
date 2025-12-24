Imports System.Data.Linq
Imports System.ServiceModel
Imports Ngl.FreightMaster.Data.DataTransferObjects

Public Class NGLCarrTarEquipData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMASCarrierDataContext(ConnectionString)
        Me.LinqTable = db.CarrierTariffEquipments
        Me.LinqDB = db
        Me.SourceClass = "NGLCarrTarEquipData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASCarrierDataContext(ConnectionString)
                _LinqTable = db.CarrierTariffEquipments
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
        Return GetCarrTarEquipFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    Public Function GetCarrTarEquipFiltered(ByVal Control As Integer) As DataTransferObjects.CarrTarEquip
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                'Dim oDLO As New DataLoadOptions
                'oDLO.LoadWith(Of LTS.CarrierTariffEquipment)(Function(t As LTS.CarrierTariffEquipment) t.CarrierTariffEquipmentBreakPoints)
                'db.LoadOptions = oDLO

                'Get the newest record that matches the provided criteria
                Dim CarrierTariffEquipmentContract As DataTransferObjects.CarrTarEquip = (
                        From d In db.CarrierTariffEquipments
                        Where
                        d.CarrTarEquipControl = Control
                        Select selectDTOData(d, db)).FirstOrDefault()

                Return CarrierTariffEquipmentContract

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

    Public Function GetCarrTarEquipsFiltered(ByVal CarrTarControl As Integer,
                                             Optional ByVal page As Integer = 1,
                                             Optional ByVal pagesize As Integer = 1000) As DataTransferObjects.CarrTarEquip()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1

                'db.Log = New DebugTextWriter

                Dim oRecords = (
                        From d In db.CarrierTariffEquipments
                        Where
                        (d.CarrTarEquipCarrTarControl = CarrTarControl)
                        Select d.CarrTarEquipControl).ToArray()

                If oRecords Is Nothing Then Return Nothing

                intRecordCount = oRecords.Count
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize

                Dim CarrierTariffEquipmentContracts() As DataTransferObjects.CarrTarEquip = (
                        From d In db.CarrierTariffEquipments
                        Where oRecords.Contains(d.CarrTarEquipControl)
                        Order By d.CarrTarEquipControl
                        Select selectDTOData(d, db, page, intPageCount, intRecordCount, pagesize)).Skip(intSkip).Take(pagesize).ToArray()

                Return CarrierTariffEquipmentContracts

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

    Public Function GetCarrTarEquipFiltered(ByVal CarrTarControl As Integer,
                                            ByVal equipName As String) As DataTransferObjects.CarrTarEquip
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                Dim oRecord = (
                        From d In db.CarrierTariffEquipments
                        Where
                        (d.CarrTarEquipCarrTarControl = CarrTarControl) And d.CarrTarEquipName.ToUpper = equipName.ToUpper
                        Select selectDTOData(d, db)).FirstOrDefault()

                Return oRecord

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


    Public Function GetFirstvCarrierTariffService(ByVal CarrTarControl As Integer) As LTS.vCarrierTariffService
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                Dim oRecord = db.vCarrierTariffServices.Where(Function(x) x.CarrTarEquipCarrTarControl = CarrTarControl).FirstOrDefault()

                Return oRecord

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

    ''' <summary>
    ''' Returns the services assoicated with a Contract the CarrTarControl value must be provided in the filters.ParentControl parameter
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 08/17/2018
    '''   new LTS carrier tariff service query  
    ''' </remarks>
    Public Function GetCarrierTariffServices(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vCarrierTariffService()
        If filters Is Nothing Then Return Nothing
        If filters.ParentControl = 0 Then
            Dim sMsg As String = " The Tariff ID is missing. Please return to the tariff contract page and select a valid carrier contract."
            throwNoDataFaultException(sMsg)
        End If
        Dim oRet() As LTS.vCarrierTariffService

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                Dim iQuery As IQueryable(Of LTS.vCarrierTariffService)
                iQuery = db.vCarrierTariffServices
                Dim filterWhere As String = " (CarrTarEquipCarrTarControl = " & filters.ParentControl & ") "
                Dim sFilterSpacer As String = " And "

                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "CarrTarEquipName"
                    filters.sortDirection = "asc"
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                'db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrierTariffServices"), db)
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    '''  Insert or Update the carrier tariff service equipment data
    ''' </summary>
    ''' <param name="service"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 08/16/2018
    ''' </remarks>
    Public Function SaveCarrierTariffService(ByVal service As LTS.vCarrierTariffService) As Boolean
        Dim blnRet As Boolean = False
        If service Is Nothing Then Return False 'nothing to do
        Dim blnNewService As Boolean = False
        Dim oNew As New LTS.CarrierTariffEquipment()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'verify the service contract
                If service.CarrTarEquipCarrTarControl = 0 Then
                    Dim lDetails As New List(Of String) From {"Carrier Contract Reference", " was not provided and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                If Not db.CarrierTariffs.Any(Function(x) x.CarrTarControl = service.CarrTarEquipCarrTarControl) Then
                    Dim lDetails As New List(Of String) From {"Carrier Contract Reference", " was not found and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                If service.CarrTarEquipControl = 0 Then
                    If db.CarrierTariffEquipments.Any(Function(x) x.CarrTarEquipCarrTarControl = service.CarrTarEquipCarrTarControl) Then
                        Dim sTariffName As String = db.CarrierTariffs.Where(Function(x) x.CarrTarControl = service.CarrTarEquipCarrTarControl).Select(Function(x) x.CarrTarName).FirstOrDefault()
                        'Cannot save changes to {0}.  Only one {1} is allowed for each {0}; a {1} of {2} is already configured for this {0}.
                        throwInvalidKeyAlreadyExistsException("Carrier Tariff Service", "Contract Name", sTariffName)
                    End If

                    Dim strMSG As String = ""
                    Dim skipObjs As New List(Of String) From {"CarrTarEquipModUser", "CarrTarEquipModDate", "CarrTarEquipUpdated"}
                    oNew = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(oNew, service, skipObjs, strMSG)
                    With oNew
                        .CarrTarEquipModDate = Date.Now
                        .CarrTarEquipModUser = Me.Parameters.UserName
                    End With
                    If Debugger.IsAttached AndAlso Not String.IsNullOrWhiteSpace(strMSG) Then
                        System.Diagnostics.Debug.WriteLine(strMSG)
                    End If
                    db.CarrierTariffEquipments.InsertOnSubmit(oNew)
                    blnNewService = True
                Else
                    Dim oExisting = db.CarrierTariffEquipments.Where(Function(x) x.CarrTarEquipControl = service.CarrTarEquipControl).FirstOrDefault()
                    If oExisting Is Nothing OrElse oExisting.CarrTarEquipControl = 0 Then
                        throwRecordDeletedFaultException("Cannot save changes for tariff service: " & service.CarrTarEquipName)
                    End If
                    Dim strMSG As String = ""
                    Dim skipObjs As New List(Of String) From {"CarrTarEquipModUser", "CarrTarEquipModDate", "CarrTarEquipUpdated"}
                    oExisting = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(oExisting, service, skipObjs, strMSG)
                    With oExisting
                        .CarrTarEquipModDate = Date.Now
                        .CarrTarEquipModUser = Me.Parameters.UserName
                        .CarrTarEquipUpdated = service.CarrTarEquipUpdated
                    End With
                End If
                db.SubmitChanges()

                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveCarrierTariffService"), db)
            End Try
        End Using
        If blnNewService Then
            'we need to add the break point and the first lines in the equipmatrix tables
            service.CarrTarEquipMatCarrTarMatBPControl = DirectCast(Me.NDPBaseClassFactory("NGLCarrTarMatBPData", False), NGLCarrTarMatBPData).createCarrTarEquipMatBreakPointForTariff(service.CarrTarEquipCarrTarControl, service.CarrTarEquipMatTarRateTypeControl, service.CarrTarEquipMatClassTypeControl, service.CarrTarEquipMatTarBracketTypeControl)
            service.CarrTarEquipControl = oNew.CarrTarEquipControl
            blnRet = DirectCast(Me.NDPBaseClassFactory("NGLCarrTarEquipMatData", False), NGLCarrTarEquipMatData).createNewCarrTarEquipMatrixFromService(service)
        End If
        Return blnRet
    End Function

    ''' <summary>
    ''' Delete the provided carrier tariff service
    ''' </summary>
    ''' <param name="iControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 08/16/2018
    ''' </remarks>
    Public Function DeleteCarrierTariffService(ByVal iControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        If iControl = 0 Then Return False 'nothing to do
        'validate before delete

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Delte the Record
                Dim oToDelete = db.CarrierTariffEquipments.Where(Function(x) x.CarrTarEquipControl = iControl).FirstOrDefault()
                If oToDelete Is Nothing OrElse oToDelete.CarrTarEquipControl = 0 Then Return True 'already deleted
                Using Bookdb As New NGLMasBookDataContext(ConnectionString)
                    If Bookdb.Books.Any(Function(x) x.BookCarrTarEquipControl = iControl) Then
                        'Modified by RHR for v-8.5.2.003 on 08/02/2022 added logic to force delete if no Rates because 
                        'the system becomes unrecoverable
                        If (db.CarrierTariffEquipMatrixes.Any(Function(x) x.CarrTarEquipMatCarrTarEquipControl = iControl)) Then
                            throwCannotDeleteRecordInUseException("Tariff Service", oToDelete.CarrTarEquipName)
                        Else
                            ' force delete by removing dependency in book Table
                            Dim sSQL As String = "Update dbo.Book set BookCarrTarEquipControl = 0 where BookCarrTarEquipControl = " & iControl.ToString()
                            executeSQL(sSQL)
                        End If
                    End If
                End Using
                db.CarrierTariffEquipments.DeleteOnSubmit(oToDelete)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteCarrierTariffService"), db)
            End Try
        End Using
        Return blnRet
    End Function


    Public Function GetCarrTarEquipNodes(ByVal CarrTarControl As Integer, ByVal ParentTreeID As Integer) As List(Of NGLTreeNode)
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'db.Log = New DebugTextWriter
                Dim CarrierTariffEquipmentContracts As List(Of NGLTreeNode) = (
                        From d In db.CarrierTariffEquipments
                        Where (d.CarrTarEquipCarrTarControl = CarrTarControl)
                        Order By d.CarrTarEquipControl
                        Select New DataTransferObjects.NGLTreeNode With {.Control = d.CarrTarEquipControl,
                        .ParentTreeID = ParentTreeID,
                        .Name = d.CarrTarEquipName,
                        .Description = d.CarrTarEquipDescription,
                        .ClassName = "CarrTarEquip"}).ToList

                Return CarrierTariffEquipmentContracts

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                Return Nothing
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetCarrTarEquipNodes"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetCarrTarEquipTree(ByVal CarrTarControl As Integer, ByVal ParentTreeID As Integer, ByRef intNextTreeID As Integer) As List(Of NGLTreeNode)
        Try
            Dim oNGLCarrTarEquipMat As NGLCarrTarEquipMatData = DirectCast(NDPBaseClassFactory("NGLCarrTarEquipMatData", False), NGLCarrTarEquipMatData)
            Dim oEquipTree As List(Of NGLTreeNode) = GetCarrTarEquipNodes(CarrTarControl, ParentTreeID)
            If Not oEquipTree Is Nothing AndAlso oEquipTree.Count > 0 Then
                Dim intNextChildTreeID As Integer = intNextTreeID + oEquipTree.Count
                For Each node In oEquipTree
                    Dim intNodeTreeID = incrementID(intNextTreeID)
                    node.TreeID = intNodeTreeID
                    'load the rates
                    Dim oRates As List(Of NGLTreeNode) = oNGLCarrTarEquipMat.GetCarrTarEquipMatNodes(node.Control, node.TreeID)
                    If Not oRates Is Nothing AndAlso oRates.Count > 0 Then
                        For Each rNode In oRates
                            rNode.TreeID = incrementID(intNextChildTreeID)
                        Next
                        If node.Children Is Nothing Then node.Children = New List(Of NGLTreeNode)
                        node.Children.AddRange(oRates)
                    End If
                Next
                intNextTreeID = intNextChildTreeID
            End If
            Return oEquipTree
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("GetCarrTarEquipTree"))
        End Try
        Return Nothing
    End Function

    Public Function GetCarrTarEquipTreeFlat(ByVal CarrTarControl As Integer, ByVal ParentTreeID As Integer, ByRef intNextTreeID As Integer) As List(Of NGLTreeNode)
        Try
            Dim oNGLCarrTarEquipMat As NGLCarrTarEquipMatData = DirectCast(NDPBaseClassFactory("NGLCarrTarEquipMatData", False), NGLCarrTarEquipMatData)
            Dim oEquipTree As List(Of NGLTreeNode) = GetCarrTarEquipNodes(CarrTarControl, ParentTreeID)
            Dim oEquipRates As New List(Of NGLTreeNode)
            If Not oEquipTree Is Nothing AndAlso oEquipTree.Count > 0 Then
                Dim intNextChildTreeID As Integer = intNextTreeID + oEquipTree.Count
                For Each node In oEquipTree
                    Dim intNodeTreeID = incrementID(intNextTreeID)
                    node.TreeID = intNodeTreeID
                    'load the rates 
                    Dim oRates As List(Of NGLTreeNode) = oNGLCarrTarEquipMat.GetCarrTarEquipMatNodes(node.Control, node.TreeID)
                    If Not oRates Is Nothing AndAlso oRates.Count > 0 Then
                        For Each rNode In oRates
                            rNode.TreeID = incrementID(intNextChildTreeID)
                        Next
                        If oEquipRates Is Nothing Then oEquipRates = New List(Of NGLTreeNode)
                        'PFMCode Change 3/27/2015 no need to add new rate types. only one allowed per tariff.
                        'Dim singeaddnode As DTO.NGLTreeNode = New DTO.NGLTreeNode With {.Control = 0,
                        '                             .ParentTreeID = node.TreeID,
                        '                             .Name = "AddNew",
                        '                             .Description = String.Format("Key: Equip {0}, RateType {1}, BracketType {2}, ClassType {3}", node.Name, 0, 0, 0),
                        '                             .AltDataKey = String.Format("{0}-{1}-{2}-{3}", node.Control, 0, 0, 0),
                        '                             .ClassName = "CarrTarEquipRates"}
                        'singeaddnode.TreeID = incrementID(intNextChildTreeID)
                        'oEquipRates.Add(singeaddnode)
                        oEquipRates.AddRange(oRates)
                    Else
                        'PFM Code Change. 8/28/2013
                        'lets go ahead and add a blank node indicating there are no children so we can add new ones.
                        oRates = New List(Of NGLTreeNode)
                        Dim singeaddnode As DataTransferObjects.NGLTreeNode = New DataTransferObjects.NGLTreeNode With {.Control = 0,
                                .ParentTreeID = node.TreeID,
                                .Name = "AddNew",
                                .Description = String.Format("Key: Equip {0}, RateType {1}, BracketType {2}, ClassType {3}", node.Name, 0, 0, 0),
                                .AltDataKey = String.Format("{0}-{1}-{2}-{3}", node.Control, 0, 0, 0),
                                .ClassName = "CarrTarEquipRates"}
                        singeaddnode.TreeID = incrementID(intNextChildTreeID)
                        oRates.Add(singeaddnode)
                        If oEquipRates Is Nothing Then oEquipRates = New List(Of NGLTreeNode)
                        oEquipRates.AddRange(oRates)
                    End If
                Next
                intNextTreeID = intNextChildTreeID
                If Not oEquipRates Is Nothing Then
                    oEquipTree.AddRange(oEquipRates)
                End If
            End If
            Return oEquipTree
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("GetCarrTarEquipTreeFlat"))
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Get a flat list of Equipment and rate type nodes for a contract.
    ''' </summary>
    ''' <param name="CarrTarControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCarrTarEquipFlatNodes(ByVal CarrTarControl As Integer) As List(Of CarrTarEquipMatNode)
        Try
            Dim oNGLCarrTarEquipMat As NGLCarrTarEquipMatData = DirectCast(NDPBaseClassFactory("NGLCarrTarEquipMatData", False), NGLCarrTarEquipMatData)
            Dim oEquipTree As List(Of NGLTreeNode) = GetCarrTarEquipNodes(CarrTarControl, 0)
            Dim oEquipRates As New List(Of CarrTarEquipMatNode)
            If Not oEquipTree Is Nothing AndAlso oEquipTree.Count > 0 Then
                Dim oRates As List(Of CarrTarEquipMatNode)
                For Each node In oEquipTree
                    'load the rates 
                    oRates = oNGLCarrTarEquipMat.GetCarrTarEquipMatRateNodes(node.Control, CarrTarControl)
                    If oRates IsNot Nothing And oRates.Count > 0 Then
                        oEquipRates.AddRange(oRates)
                    End If
                Next
            End If
            Return oEquipRates
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("GetCarrTarEquipFlatNodes"))
        End Try
        Return Nothing
    End Function

    Public Function GetCarrTarEquipFlatNodesList(ByVal CarrTarControl As ArrayList) As List(Of CarrTarEquipMatNode)
        Try
            Dim oEquipRatesBigList As New List(Of CarrTarEquipMatNode)
            For Each item As Integer In CarrTarControl
                Dim oEquipRates As List(Of CarrTarEquipMatNode) = GetCarrTarEquipFlatNodes(item)
                oEquipRatesBigList.AddRange(oEquipRates)
            Next

            Return oEquipRatesBigList
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("GetCarrTarEquipFlatNodesList"))
        End Try
        Return Nothing
    End Function


    ''' <summary>
    ''' Looks up the most recient carrier tariff equipment control number using the pre cloned control number.
    ''' returns zero if a match cannot be found.
    ''' </summary>
    ''' <param name="CarrTarControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getNewestEquipControlUsingNewCarrTarControl(ByVal CarrTarControl As Integer) As Integer
        Dim intRet As Integer = 0
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                intRet = (From d In db.CarrierTariffEquipments Where d.CarrTarEquipCarrTarControl = CarrTarControl Order By d.CarrTarEquipControl Descending Select d.CarrTarEquipControl).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("getNewestEquipControlUsingNewCarrTarControl"))
            End Try
        End Using
        Return intRet
    End Function

    ''' <summary>
    ''' Looks up the most recient carrier tariff equipment control number using the pre cloned control number.
    ''' returns zero if a match cannot be found.
    ''' </summary>
    ''' <param name="CarrTarEquipPreCloneControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getNewestEquipControlUsingPreClonedValue(ByVal CarrTarEquipPreCloneControl As Integer) As Integer
        Dim intRet As Integer = 0
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                intRet = (From d In db.CarrierTariffEquipments Where d.CarrTarEquipPreCloneControl = CarrTarEquipPreCloneControl Order By d.CarrTarEquipControl Descending Select d.CarrTarEquipControl).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("getNewestEquipControlUsingPreClonedValue"))
            End Try
        End Using
        Return intRet
    End Function

    ''' <summary>
    ''' Looks up the most recent  carrier tariff matrix break point control number using the pre cloned control number.
    ''' returns zero if a match cannot be found.
    ''' </summary>
    ''' <param name="CarrTarMatBPPreCloneControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getNewestCarrTarMatBPControlUsingPreClonedValue(ByVal CarrTarMatBPPreCloneControl As Integer) As Integer
        Dim intRet As Integer = 0
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                intRet = (From d In db.CarrierTariffMatrixBPs Where d.CarrTarMatBPPreCloneControl = CarrTarMatBPPreCloneControl Order By d.CarrTarMatBPControl Descending Select d.CarrTarMatBPControl).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("getNewestEquipControlUsingPreClonedValue"))
            End Try
        End Using
        Return intRet
    End Function

    'Public Function GetTariffCode(ByVal CarrierControl As Integer, _
    '                              ByVal CompControl As Integer, _
    '                              ByVal TariffTempType As Integer, _
    '                              ByVal TariffType As String) As String
    '    Dim strCompNumber As String = "0"
    '    Dim strCarrierNumber As String = "0"
    '    Dim strTempType As String = "0" & TariffTempType.ToString
    '    Using db As New NGLMASCarrierDataContext(ConnectionString)
    '        Try
    '            Dim varComp = From d In db.CompRefCarriers Where d.CompControl = CompControl Select d.CompNumber
    '            Dim varCarr = From c In db.Carriers Where c.CarrierControl = CarrierControl Select c.CarrierNumber

    '            If Not varComp Is Nothing AndAlso varComp.Count > 0 Then strCompNumber = varComp(0).ToString

    '            If Not varCarr Is Nothing AndAlso varCarr.Count > 0 Then strCarrierNumber = varCarr(0).ToString

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

    '        Return strCarrierNumber & "-" & strCompNumber & "-" & strTempType & "-" & TariffTempType

    '    End Using
    'End Function

    ' ''' <summary>
    ' ''' This method will test if an overwrite condition exists.  It does not test if
    ' ''' the users wants to clone an existing tariff and create a new one.  The caller 
    ' ''' must test for the posibility of a clone condition.  If one of the TariffID key
    ' ''' fields changes {CarrierControl, CompControl, TariffTempType or TariffType} the
    ' ''' caller must set the CarrTarControl to zero if a clone operation is desired. They 
    ' ''' should then call AddNew, instead of Update, after calling CanSaveTariff.  It may 
    ' ''' still be necessary to set the AllowOverwrite flag of the CarrierTariffEquipmentHeader object 
    ' ''' to true.
    ' ''' </summary>
    ' ''' <param name="CarrTarControl"></param>
    ' ''' <param name="CarrTarID"></param>
    ' ''' <param name="CarrierControl"></param>
    ' ''' <param name="CompControl"></param>
    ' ''' <param name="TariffTempType"></param>
    ' ''' <param name="TariffType"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function CanSaveTariff(ByVal CarrTarControl As Integer, _
    '                              ByVal CarrTarID As String, _
    '                              ByVal CarrierControl As Integer, _
    '                              ByVal CompControl As Integer, _
    '                              ByVal TariffTempType As Integer, _
    '                              ByVal TariffType As String) As Boolean
    '    Dim blnRet As Boolean = True
    '    Using db As New NGLMASCarrierDataContext(ConnectionString)
    '        Try
    '            Dim strNewTariffID = GetTariffCode(CarrierControl, CompControl, TariffTempType, TariffType)

    '            If CarrTarControl = 0 Then
    '                'this is a new tariff so we need to check if the tariff id already exists
    '                Dim varTariff = From d In db.CarrierTariffEquipments Where d.CarrTarID = strNewTariffID Select d
    '                If Not varTariff Is Nothing AndAlso varTariff.Count > 0 Then Return False
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

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DataTransferObjects.CarrTarEquip)
        'Create New Record
        Return New LTS.CarrierTariffEquipment With {.CarrTarEquipControl = d.CarrTarEquipControl,
            .CarrTarEquipCarrTarControl = d.CarrTarEquipCarrTarControl,
            .CarrTarEquipCarrierEquipControl = d.CarrTarEquipCarrierEquipControl,
            .CarrTarEquipName = d.CarrTarEquipName,
            .CarrTarEquipDescription = d.CarrTarEquipDescription,
            .CarrTarEquipPUMon = d.CarrTarEquipPUMon,
            .CarrTarEquipPUTue = d.CarrTarEquipPUTue,
            .CarrTarEquipPUWed = d.CarrTarEquipPUWed,
            .CarrTarEquipPUThu = d.CarrTarEquipPUThu,
            .CarrTarEquipPUFri = d.CarrTarEquipPUFri,
            .CarrTarEquipPUSat = d.CarrTarEquipPUSat,
            .CarrTarEquipPUSun = d.CarrTarEquipPUSun,
            .CarrTarEquipDLMon = d.CarrTarEquipDLMon,
            .CarrTarEquipDLTue = d.CarrTarEquipDLTue,
            .CarrTarEquipDLWed = d.CarrTarEquipDLWed,
            .CarrTarEquipDLThu = d.CarrTarEquipDLThu,
            .CarrTarEquipDLFri = d.CarrTarEquipDLFri,
            .CarrTarEquipDLSat = d.CarrTarEquipDLSat,
            .CarrTarEquipDLSun = d.CarrTarEquipDLSun,
            .CarrTarEquipCasesMinimum = d.CarrTarEquipCasesMinimum,
            .CarrTarEquipCasesConsiderFull = d.CarrTarEquipCasesConsiderFull,
            .CarrTarEquipCasesMaximum = d.CarrTarEquipCasesMaximum,
            .CarrTarEquipWgtMinimum = d.CarrTarEquipWgtMinimum,
            .CarrTarEquipWgtConsiderFull = d.CarrTarEquipWgtConsiderFull,
            .CarrTarEquipWgtMaximum = d.CarrTarEquipWgtMaximum,
            .CarrTarEquipCubesMinimum = d.CarrTarEquipCubesMinimum,
            .CarrTarEquipCubesConsiderFull = d.CarrTarEquipCubesConsiderFull,
            .CarrTarEquipCubesMaximum = d.CarrTarEquipCubesMaximum,
            .CarrTarEquipPltsMinimum = d.CarrTarEquipPltsMinimum,
            .CarrTarEquipPltsConsiderFull = d.CarrTarEquipPltsConsiderFull,
            .CarrTarEquipPltsMaximum = d.CarrTarEquipPltsMaximum,
            .CarrTarEquipTempType = d.CarrTarEquipTempType,
            .CarrTarEquipCarrProName = d.CarrTarEquipCarrProName,
            .CarrTarEquipModUser = Parameters.UserName,
            .CarrTarEquipModDate = Date.Now,
            .CarrTarEquipUpdated = If(d.CarrTarEquipUpdated Is Nothing, New Byte() {}, d.CarrTarEquipUpdated),
            .CarrTarEquipMultiOrigRating = d.CarrTarEquipMultiOrigRating} 'Modified by RHR for v-8.5.4.001 on 06/14/2023 new Origin Rate shop logic
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCarrTarEquipFiltered(Control:=CType(LinqTable, LTS.CarrierTariffEquipment).CarrTarEquipControl)
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
    '                Dim varTariff = (From d In db.CarrierTariffEquipments Where d.CarrTarID = strNewTariffID Select d).First
    '                If Not varTariff Is Nothing Then
    '                    If Not AllowOverwrite Then Return False
    '                    CarrTarControl = varTariff.CarrTarControl
    '                    CarrTarID = strNewTariffID
    '                    ''Delete all of the matrix details they no longer match.
    '                    'executeSQL("DELETE FROM [dbo].[CarrierTariffEquipmentMatrix] Where CarrTarMatCarrTarControl = " & CarrTarControl)
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
                Dim source As LTS.CarrierTariffEquipment = TryCast(LinqTable, LTS.CarrierTariffEquipment)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.CarrierTariffEquipments
                    Where d.CarrTarEquipControl = source.CarrTarEquipControl
                    Select New DataTransferObjects.QuickSaveResults With {.Control = d.CarrTarEquipControl _
                        , .ModDate = d.CarrTarEquipModDate _
                        , .ModUser = d.CarrTarEquipModUser _
                        , .Updated = d.CarrTarEquipUpdated.ToArray}).First

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

    ''' <summary>
    ''' Checks the Book Table for a reference to the CarrTarEquipControl.
    ''' Records cannot be deleted if a reference exists.  
    ''' Throws E_InvalidRequest FaultException if record exists in Book Table
    ''' </summary>
    ''' <param name="oDB"></param>
    ''' <param name="oData"></param>
    ''' <remarks></remarks>
    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        Dim oCarrTarData As DataTransferObjects.CarrTarEquip = TryCast(oData, DataTransferObjects.CarrTarEquip)
        If oCarrTarData Is Nothing Then Return
        Dim intCarrTarControl = oCarrTarData.CarrTarEquipControl
        If intCarrTarControl = 0 Then Return
        Dim strCarrTarName = oCarrTarData.CarrTarEquipName
        Using db As New NGLMasBookDataContext(ConnectionString)
            If db.Books.Any(Function(x) x.BookCarrTarEquipControl = intCarrTarControl) Then
                throwCannotDeleteRecordInUseException("Tariff Equipment Name", strCarrTarName)
            End If
        End Using
    End Sub

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        ''Check if the data already exists only one allowed
        'With CType(oData, DTO.CarrierTariffEquipment)
        '    Try
        '        Dim CarrierTariffEquipment As DTO.CarrierTariffEquipment = ( _
        '            From t In CType(oDB, NGLMASCarrierDataContext).CarrierTariffEquipments _
        '             Where _
        '                 (t.CarrierTariffEquipmentCarrierControl = .CarrierTariffEquipmentCarrierControl _
        '                 And _
        '                 t.CarrierTariffEquipmentXaction = .CarrierTariffEquipmentXaction) _
        '             Select New DTO.CarrierTariffEquipment With {.CarrierTariffEquipmentControl = t.CarrierTariffEquipmentControl}).First

        '        If Not CarrierTariffEquipment Is Nothing Then
        '            Utilities.SaveAppError("Cannot save new Carrier EDI data.  The Carrier EDI XAction, " & .CarrierTariffEquipmentXaction & ",  is already assigned to the selected carrier.", Me.Parameters)
        '            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
        '        End If

        '    Catch ex As FaultException
        '        Throw
        '    Catch ex As InvalidOperationException
        '        'do nothing this is the desired result.
        '    End Try
        'End With
    End Sub

    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        ''Check if the data already exists only one allowed
        'With CType(oData, DTO.CarrierTariffEquipment)
        '    Try
        '        'Get the newest record that matches the provided criteria
        '        Dim CarrierTariffEquipment As DTO.CarrierTariffEquipment = ( _
        '        From t In CType(oDB, NGLMASCarrierDataContext).CarrierTariffEquipments _
        '         Where _
        '             (t.CarrierTariffEquipmentControl <> .CarrierTariffEquipmentControl) _
        '             And _
        '             (t.CarrierTariffEquipmentCarrierControl = .CarrierTariffEquipmentCarrierControl _
        '             And _
        '             t.CarrierTariffEquipmentXaction = .CarrierTariffEquipmentXaction) _
        '         Select New DTO.CarrierTariffEquipment With {.CarrierTariffEquipmentControl = t.CarrierTariffEquipmentControl}).First

        '        If Not CarrierTariffEquipment Is Nothing Then
        '            Utilities.SaveAppError("Cannot save Carrier EDI changes.  The Carrier EDI XAction, " & .CarrierTariffEquipmentXaction & ",  is already assigned to the selected carrier.", Me.Parameters)
        '            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
        '        End If

        '    Catch ex As FaultException
        '        Throw
        '    Catch ex As InvalidOperationException
        '        'do nothing this is the desired result.
        '    End Try
        'End With
    End Sub

    Friend Function selectDTOData(ByVal d As LTS.CarrierTariffEquipment, ByRef db As NGLMASCarrierDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.CarrTarEquip
        Return New DataTransferObjects.CarrTarEquip With {.CarrTarEquipControl = d.CarrTarEquipControl,
            .CarrTarEquipCarrTarControl = d.CarrTarEquipCarrTarControl,
            .CarrTarEquipCarrierEquipControl = d.CarrTarEquipCarrierEquipControl,
            .CarrTarEquipName = d.CarrTarEquipName,
            .CarrTarEquipDescription = d.CarrTarEquipDescription,
            .CarrTarEquipPUMon = d.CarrTarEquipPUMon,
            .CarrTarEquipPUTue = d.CarrTarEquipPUTue,
            .CarrTarEquipPUWed = d.CarrTarEquipPUWed,
            .CarrTarEquipPUThu = d.CarrTarEquipPUThu,
            .CarrTarEquipPUFri = d.CarrTarEquipPUFri,
            .CarrTarEquipPUSat = d.CarrTarEquipPUSat,
            .CarrTarEquipPUSun = d.CarrTarEquipPUSun,
            .CarrTarEquipDLMon = d.CarrTarEquipDLMon,
            .CarrTarEquipDLTue = d.CarrTarEquipDLTue,
            .CarrTarEquipDLWed = d.CarrTarEquipDLWed,
            .CarrTarEquipDLThu = d.CarrTarEquipDLThu,
            .CarrTarEquipDLFri = d.CarrTarEquipDLFri,
            .CarrTarEquipDLSat = d.CarrTarEquipDLSat,
            .CarrTarEquipDLSun = d.CarrTarEquipDLSun,
            .CarrTarEquipCasesMinimum = d.CarrTarEquipCasesMinimum,
            .CarrTarEquipCasesConsiderFull = d.CarrTarEquipCasesConsiderFull,
            .CarrTarEquipCasesMaximum = d.CarrTarEquipCasesMaximum,
            .CarrTarEquipWgtMinimum = d.CarrTarEquipWgtMinimum,
            .CarrTarEquipWgtConsiderFull = d.CarrTarEquipWgtConsiderFull,
            .CarrTarEquipWgtMaximum = d.CarrTarEquipWgtMaximum,
            .CarrTarEquipCubesMinimum = d.CarrTarEquipCubesMinimum,
            .CarrTarEquipCubesConsiderFull = d.CarrTarEquipCubesConsiderFull,
            .CarrTarEquipCubesMaximum = d.CarrTarEquipCubesMaximum,
            .CarrTarEquipPltsMinimum = d.CarrTarEquipPltsMinimum,
            .CarrTarEquipPltsConsiderFull = d.CarrTarEquipPltsConsiderFull,
            .CarrTarEquipPltsMaximum = d.CarrTarEquipPltsMaximum,
            .CarrTarEquipTempType = d.CarrTarEquipTempType,
            .CarrTarEquipCarrProName = d.CarrTarEquipCarrProName,
            .CarrTarEquipModUser = d.CarrTarEquipModUser,
            .CarrTarEquipModDate = d.CarrTarEquipModDate,
            .CarrTarEquipUpdated = d.CarrTarEquipUpdated.ToArray(),
            .Page = page,
            .Pages = pagecount,
            .RecordCount = recordcount,
            .PageSize = pagesize,
            .CarrTarEquipMultiOrigRating = d.CarrTarEquipMultiOrigRating} 'Modified by RHR for v-8.5.4.001 on 06/14/2023 new Origin Rate shop logic
    End Function

#End Region

End Class