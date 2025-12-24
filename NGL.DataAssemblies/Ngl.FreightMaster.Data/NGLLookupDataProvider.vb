Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.ServiceModel

Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports LTS = Ngl.FreightMaster.Data.LTS
Imports Ngl.Core.ChangeTracker
Imports DTran = Ngl.Core.Utility.DataTransformation
Imports System.Linq.Dynamic
Imports Ngl.FreightMaster.Data.DataTransferObjects
Imports Microsoft.ApplicationInsights
Imports Ngl.FM.P44
Imports SerilogTracing
Imports Ngl.FreightMaster.Data.LTS

Public Class NGLLoadStatusCodeData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLookupDataContext(ConnectionString)
        Me.LinqTable = db.LoadStatusCodes
        Me.LinqDB = db
        Me.SourceClass = "NGLLoadStatusCodeData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLookupDataContext(ConnectionString)
            Me.LinqTable = db.LoadStatusCodes
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
        Return Nothing
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetLoadStatusCodesFiltered()
    End Function

    Public Function GetLoadStatusCodesFiltered(ByVal Code As Integer) As DTO.LoadStatusCode
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim LoadStatusCode As DTO.LoadStatusCode = (
                From d In db.LoadStatusCodes
                Where
                    (d.LoadStatusCode = Code)
                Order By d.LoadStatusCode
                Select selectDTOData(d)).First

                Return LoadStatusCode
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLoadStatusCodesFiltered"))
            End Try
            Return Nothing
        End Using
    End Function

    Public Function GetLoadStatusCodesFiltered() As DTO.LoadStatusCode()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'Return all the records that match the criteria
                Dim LoadStatusCodes() As DTO.LoadStatusCode = (
                From d In db.LoadStatusCodes
                Order By d.LoadStatusCode
                Select selectDTOData(d)).ToArray()

                Return LoadStatusCodes
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
    ''' Wrapper method for spGetLoadStatusControl,  creates the load status record if it does not exist 
    ''' and returns the control number for the provided Load Status Code; use default or set iDefaultSequence 
    ''' to zero to use existing or to get next highest sequence number by type 
    ''' </summary>
    ''' <param name="LoadStatusCode"></param>
    ''' <param name="DefaultLoadStatusDesc"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.2.0.117 on 8/11/2019
    '''     caller must now provide a type code All previous references have been updated
    '''     the sequence number is optional, if using default (0) or set to (0) the procedure
    '''     will use the existing value or get next highest sequence number, by @LoadStatusLSCTControl, 
    '''     When performing an insert
    ''' </remarks>
    Public Function GetLoadStatusControl(ByVal LoadStatusCode As Integer, ByVal DefaultLoadStatusDesc As String, ByVal eLoadStatusCodeType As NGLLookupDataProvider.LoadStatusCodeTypes, Optional ByVal iDefaultSequence As Integer = 0) As Integer
        Dim intRet As Integer = 0
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim oRet = db.spGetLoadStatusControl(LoadStatusCode, DefaultLoadStatusDesc, eLoadStatusCodeType, iDefaultSequence).FirstOrDefault()
                If Not oRet Is Nothing Then
                    intRet = oRet.LoadStatusCode
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLoadStatusControl"))
            End Try
            Return intRet
        End Using
    End Function



#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.LoadStatusCode)
        'Create New Record
        Return New LTS.LoadStatusCode With {.LoadStatusCode = d.LoadStatusCode _
                                              , .LoadStatusDesc = d.LoadStatusCodeDesc _
                                              , .LoadStatusControl = d.LoadStatusCodeControl _
                                              , .LoadStatusLSCTControl = d.LoadStatusLSCTControl _
                                              , .LoadStatusSequenceNo = d.LoadStatusSequenceNo _
                                              , .LoadStatusCodesUpdated = If(d.LoadStatusCodesUpdated Is Nothing, New Byte() {}, d.LoadStatusCodesUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetLoadStatusCodesFiltered(CType(LinqTable, LTS.LoadStatusCode).LoadStatusCode)
    End Function

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DTO.LoadStatusCode)
            Try
                Dim oExists As DTO.LoadStatusCode = (
                    From t In CType(oDB, NGLMASLookupDataContext).LoadStatusCodes
                    Where
                     t.LoadStatusCode = .LoadStatusCode
                    Select New DTO.LoadStatusCode With {.LoadStatusCode = t.LoadStatusCode}).First

                If Not oExists Is Nothing Then
                    Utilities.SaveAppError("Cannot save new LoadStatusCode data. The LoadStatusCode, " & .LoadStatusCode & ", already exists.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'this is not needed. updates are ok.

        ''Check if the data already exists only one allowed
        'With CType(oData, DTO.LoadStatusCode)
        '    'First we need to determine if the LoadStatusCode exists (if not the user is trying to update the LoadStatusCode and this is not allowed).
        '    Try
        '        Dim oExists As DTO.LoadStatusCode = ( _
        '            From t In CType(oDB, NGLMASLookupDataContext).LoadStatusCodes _
        '             Where _
        '             t.LoadStatusCode = .LoadStatusCode _
        '             Select New DTO.LoadStatusCode With {.LoadStatusCode = t.LoadStatusCode}).First

        '        If oExists Is Nothing Then
        '            'Cannot update record because CommCodetype (PK) does not exist
        '            Utilities.SaveAppError("Cannot save LoadStatusCode changes.  The LoadStatusCode value, " & .LoadStatusCode & ", is the primary key and could not be found in the database.  Please use add to create a new record (updates to the CommCodeType are not allowed).", Me.Parameters)
        '            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UpdateNotAllowed"}, New FaultReason("E_AccessDenied"))
        '        End If

        '    Catch ex As FaultException
        '        Throw
        '    Catch ex As InvalidOperationException
        '        'Cannot update record because CommCodetype (PK) does not exist
        '        Utilities.SaveAppError("Cannot save TempType changes.  The CommCodetype value, " & .CommCodeType & ", is the primary key and could not be found in the database.  Please use add to create a new record (updates to the CommCodeType are not allowed).", Me.Parameters)
        '        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UpdateNotAllowed"}, New FaultReason("E_AccessDenied"))

        '    End Try


        'End With
    End Sub


    ''' <summary>
    ''' NOTE: This is a special way of doing the update in order to be backwards compatible as well as handle special functionality new to 365
    ''' I added 2 new fields to the LoadStatusCodes table and we want to make sure that on update from the DTO these fields do not get wiped out in the db
    ''' I did not add these fields to the DTO (LoadStatusLSCTControl and LoadStatusSequenceNo)
    ''' </summary>
    ''' <typeparam name="TEntity"></typeparam>
    ''' <param name="oData"></param>
    ''' <param name="oLinqTable"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 3/1/19
    ''' This is a special way of doing the update in order to be backwards compatible as well as handle special functionality new to 365
    ''' I added 2 new fields to the LoadStatusCodes table and we want to make sure that on update from the DTO these fields do not get wiped out in the db
    ''' I did not add these fields to the DTO (LoadStatusLSCTControl and LoadStatusSequenceNo)
    ''' </remarks>
    Public Overrides Function Update(Of TEntity As Class)(ByVal oData As Object, ByVal oLinqTable As System.Data.Linq.Table(Of TEntity)) As Object
        Using LinqDB
            'Note: the ValidateData Function must throw a FaultException error on failure
            ValidateUpdatedRecord(LinqDB, oData)
            Dim d = CType(oData, DTO.LoadStatusCode)
            Try
                'Get the LTS
                Dim lts = (From t In CType(LinqDB, NGLMASLookupDataContext).LoadStatusCodes Where t.LoadStatusControl = d.LoadStatusCodeControl Select t).FirstOrDefault()
                'Update LTS from DTO
                lts.LoadStatusCode = d.LoadStatusCode
                lts.LoadStatusDesc = d.LoadStatusCodeDesc
                lts.LoadStatusLSCTControl = d.LoadStatusLSCTControl
                lts.LoadStatusSequenceNo = d.LoadStatusSequenceNo
                lts.LoadStatusCodesUpdated = If(d.LoadStatusCodesUpdated Is Nothing, New Byte() {}, d.LoadStatusCodesUpdated)
                'Submit Changes
                LinqDB.SubmitChanges()
            Catch ex As SqlException
                ManageLinqDataExceptions(ex, buildProcedureName("Update"), LinqDB)
            End Try
            PostUpdate(LinqDB, oData) 'This method optionally performs any additional functions or cleanup needed after a save
            Return GetLoadStatusCodesFiltered(d.LoadStatusCode) 'Return the updated order
        End Using
    End Function

    ''' <summary>
    ''' Calls Update (with special logic)
    ''' </summary>
    ''' <typeparam name="TEntity"></typeparam>
    ''' <param name="oData"></param>
    ''' <param name="oLinqTable"></param>
    ''' <remarks>
    ''' Added By LVV on 3/1/19
    ''' This is a special way of doing the update in order to be backwards compatible as well as handle special functionality new to 365
    ''' I added 2 new fields to the LoadStatusCodes table and we want to make sure that on update from the DTO these fields do not get wiped out in the db
    ''' I did not add these fields to the DTO (LoadStatusLSCTControl and LoadStatusSequenceNo)
    ''' </remarks>
    Public Overrides Sub UpdateNoReturn(Of TEntity As Class)(ByVal oData As Object, ByVal oLinqTable As System.Data.Linq.Table(Of TEntity))
        Update(oData, oLinqTable)
    End Sub

    ''' <summary>
    ''' Calls Update (with special logic)
    ''' </summary>
    ''' <typeparam name="TEntity"></typeparam>
    ''' <param name="oData"></param>
    ''' <param name="oLinqTable"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 3/1/19
    ''' This is a special way of doing the update in order to be backwards compatible as well as handle special functionality new to 365
    ''' I added 2 new fields to the LoadStatusCodes table and we want to make sure that on update from the DTO these fields do not get wiped out in the db
    ''' I did not add these fields to the DTO (LoadStatusLSCTControl and LoadStatusSequenceNo)
    ''' </remarks>
    Public Overrides Function UpdateWithDetails(Of TEntity As Class)(ByVal oData As Object, ByVal oLinqTable As System.Data.Linq.Table(Of TEntity)) As Object
        Return Update(oData, oLinqTable)
    End Function

    ''' <summary>
    ''' Calls Update (with special logic)
    ''' </summary>
    ''' <typeparam name="TEntity"></typeparam>
    ''' <param name="oData"></param>
    ''' <param name="oLinqTable"></param>
    ''' <remarks>
    ''' Added By LVV on 3/1/19
    ''' This is a special way of doing the update in order to be backwards compatible as well as handle special functionality new to 365
    ''' I added 2 new fields to the LoadStatusCodes table and we want to make sure that on update from the DTO these fields do not get wiped out in the db
    ''' I did not add these fields to the DTO (LoadStatusLSCTControl and LoadStatusSequenceNo)
    ''' </remarks>
    Public Overrides Sub UpdateWithDetailsNoReturn(Of TEntity As Class)(ByVal oData As Object, ByVal oLinqTable As System.Data.Linq.Table(Of TEntity))
        Update(oData, oLinqTable)
    End Sub


    Friend Shared Function selectDTOData(ByVal d As LTS.LoadStatusCode, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.LoadStatusCode
        Dim oDTO As New DTO.LoadStatusCode
        Dim skipObjs As New List(Of String) From {"LoadStatusCodesUpdated", "rowguid", "Page", "Pages", "RecordCount", "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .LoadStatusCodesUpdated = If(d.LoadStatusCodesUpdated Is Nothing, New Byte() {}, d.LoadStatusCodesUpdated.ToArray())
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oDTO

    End Function


#End Region

End Class

Public Class NGLLoadTypeData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLookupDataContext(ConnectionString)
        Me.LinqTable = db.LoadTypes
        Me.LinqDB = db
        Me.SourceClass = "NGLLoadTypeData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLookupDataContext(ConnectionString)
            Me.LinqTable = db.LoadTypes
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
        Return GetLoadTypeFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetLoadTypesFiltered()
    End Function

    Public Function GetLoadTypeFiltered(Optional ByVal Control As Integer = 0, Optional ByVal LoadTypeName As String = "") As DTO.LoadType
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim LoadType As DTO.LoadType = (
                From d In db.LoadTypes
                Where
                    (d.ID = If(Control = 0, d.ID, Control)) _
                     And
                     (LoadTypeName Is Nothing OrElse String.IsNullOrEmpty(LoadTypeName) OrElse d.LoadType = LoadTypeName)
                Select New DTO.LoadType With {.ID = d.ID _
                                              , .LoadTypeName = d.LoadType _
                                              , .LoadTypeGroup = d.LoadTypeGroup _
                                              , .LoadTypeUpdated = d.LoadTypeUpdated.ToArray()}).First


                Return LoadType

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

    Public Function GetLoadTypesFiltered(Optional ByVal LoadTypeGroup As String = "") As DTO.LoadType()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim LoadTypes() As DTO.LoadType = (
                From d In db.LoadTypes
                Where
                    d.LoadTypeGroup.Contains(LoadTypeGroup)
                Order By d.LoadType
                Select New DTO.LoadType With {.ID = d.ID _
                                              , .LoadTypeName = d.LoadType _
                                              , .LoadTypeGroup = d.LoadTypeGroup _
                                              , .LoadTypeUpdated = d.LoadTypeUpdated.ToArray()}).ToArray()
                Return LoadTypes

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

    Public Function GetLoadTypebyFiltered(ByVal LaneFeesControl As Integer) As LTS.LoadType
        Dim oRet As LTS.LoadType 'return the table or view
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                oRet = db.LoadTypes.Where(Function(x) x.ID = LaneFeesControl).FirstOrDefault() 'added firstordefault to fix edit error Manorama' 16-Jun-2020
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLoadTypebyFiltered"), db)
            End Try
            Return oRet
        End Using
    End Function

    ''' <summary>
    ''' Method to Get LoadType Records 
    ''' </summary>
    ''' <param name="filters">filters</param>
    ''' <param name="RecordCount">No Of records</param>
    ''' <remarks>
    ''' Added By Suhas On 05-SEP-2020
    ''' 
    ''' </remarks>
    ''' <returns>LTS LaneTrans</returns>
    Public Function GetLoadTypeFiltered(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.LoadType()
        If filters Is Nothing Then Return Nothing
        Dim itransControl As Integer = 0 'PK Control Number
        Dim filterWhere As String = ""
        Dim sFilterSpacer As String = ""

        Dim oRet() As LTS.LoadType 'return the table or view
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                If (filters.FilterValues Is Nothing OrElse filters.FilterValues.Count() < 1) OrElse (Not filters.FilterValues.Any(Function(x) x.filterName = "ID")) Then
                    'The Record Control Filter does not exist so use the parent control fliter

                Else
                    Dim tFilter As Models.FilterDetails = filters.FilterValues.Where(Function(x) x.filterName = "ID").FirstOrDefault()
                    Integer.TryParse(tFilter.filterValueFrom, itransControl)
                End If

                Dim iQuery As IQueryable(Of LTS.LoadType)
                iQuery = db.LoadTypes
                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "ID"
                    filters.sortDirection = "desc"
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLoadTypeFiltered"), db)
            End Try
            Return oRet
        End Using
    End Function

    Public Function SaveOrCreateLoadType(ByVal oData As LTS.LoadType) As Boolean
        Dim blnRet As Boolean = False
        Dim blnvalue As LTS.LoadType
        If oData Is Nothing Then Return False 'nothing to do

        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'Dim iLaneTranID = oData.ID
                'If oData.LaneFeesLaneControl = 0 Then
                'If iLaneTranID = 0 Then
                '    Dim sMsg As String = "E_MissingParent" ' "  The reference to the parent record is missing. Please select a valid parent record and try again."
                '    throwNoDataFaultException(sMsg)
                'End If

                'End If

                '    Dim blnProcessed As Boolean = False
                'oData.LaneFeesModDate = Date.Now()
                'oData.LaneFeesModUser = Me.Parameters.UserName

                'If oData.ID = 0 Then
                '    db.LoadTypes.InsertOnSubmit(oData)
                'Else
                '    db.LoadTypes.Attach(oData, True)
                'End If

                If Not oData.ID = 0 Then
                    blnvalue = GetLoadTypebyFiltered(oData.ID)
                    If blnvalue Is Nothing Then
                        db.LoadTypes.InsertOnSubmit(oData)
                    Else
                        db.LoadTypes.Attach(oData, True)
                    End If
                End If
                db.SubmitChanges()

                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveOrCreateLoadType"), db)
            End Try
        End Using
        Return blnRet
    End Function

    Public Function DeleteLoadType(ByVal iLaneTranID As Integer) As Boolean
        Dim blnRet As Boolean = False

        If iLaneTranID = 0 Then Return False 'nothing to do
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'verify the service contract
                Dim oExisting = db.LoadTypes.Where(Function(x) x.ID = iLaneTranID).FirstOrDefault()
                If oExisting Is Nothing OrElse oExisting.ID = 0 Then Return True
                db.LoadTypes.DeleteOnSubmit(oExisting)
                db.SubmitChanges()

                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteLoadType"), db)
            End Try
        End Using
        Return blnRet
    End Function

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.LoadType)
        'Create New Record
        Return New LTS.LoadType With {.ID = d.ID _
                                              , .LoadType = d.LoadTypeName _
                                              , .LoadTypeGroup = d.LoadTypeGroup _
                                              , .LoadTypeUpdated = If(d.LoadTypeUpdated Is Nothing, New Byte() {}, d.LoadTypeUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetLoadTypeFiltered(Control:=CType(LinqTable, LTS.LoadType).ID)
    End Function

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DTO.LoadType)
            Try
                Dim oExists As DTO.LoadType = (
                    From t In CType(oDB, NGLMASLookupDataContext).LoadTypes
                    Where
                     t.ID = .ID
                    Select New DTO.LoadType With {.ID = t.ID}).First

                If Not oExists Is Nothing Then
                    Utilities.SaveAppError("Cannot save new LoadTyoe data.  The LoadType, " & .LoadTypeName & ", already exist.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub
#End Region

End Class

Public Class NGLPalletTypeData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLookupDataContext(ConnectionString)
        Me.LinqTable = db.PalletTypes
        Me.LinqDB = db
        Me.SourceClass = "NGLPalletTypeData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLookupDataContext(ConnectionString)
            Me.LinqTable = db.PalletTypes
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"
    Public Overrides Function GetRecordFiltered(Optional Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return GetPalletTypeFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetPalletTypesFiltered()
    End Function

    Public Function GetPalletTypeFiltered(Optional ByVal Control As Integer = 0, Optional ByVal PalletTypeName As String = "") As DTO.PalletType
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim PalletType As DTO.PalletType = (
                From d In db.PalletTypes
                Where
                    (d.ID = If(Control = 0, d.ID, Control)) _
                     And
                     (PalletTypeName Is Nothing OrElse String.IsNullOrEmpty(PalletTypeName) OrElse d.PalletType = PalletTypeName)
                Select New DTO.PalletType With {.ID = d.ID _
                                              , .PalletTypeName = d.PalletType _
                                              , .PalletTypeContainer = d.PalletTypeContainer _
                                              , .PalletTypeDepth = d.PalletTypeDepth _
                                              , .PalletTypeDescription = d.PalletTypeDescription _
                                              , .PalletTypeHeight = d.PalletTypeHeight _
                                              , .PalletTypeVolume = d.PalletTypeVolume _
                                              , .PalletTypeWeight = d.PalletTypeWeight _
                                              , .PalletTypeWidth = d.PalletTypeWidth _
                                              , .PalletTypeUpdated = d.PalletTypeUpdated.ToArray()}).First


                Return PalletType

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

    Public Function GetPalletTypesFiltered() As DTO.PalletType()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim PalletTypes() As DTO.PalletType = (
                    From d In db.PalletTypes
                    Order By d.PalletTypeBitPos
                    Select New DTO.PalletType With {.ID = d.ID _
                                                  , .PalletTypeName = d.PalletType _
                                                  , .PalletTypeContainer = d.PalletTypeContainer _
                                                  , .PalletTypeDepth = d.PalletTypeDepth _
                                                  , .PalletTypeDescription = d.PalletTypeDescription _
                                                  , .PalletTypeHeight = d.PalletTypeHeight _
                                                  , .PalletTypeVolume = d.PalletTypeVolume _
                                                  , .PalletTypeWeight = d.PalletTypeWeight _
                                                  , .PalletTypeWidth = d.PalletTypeWidth _
                                                  , .PalletTypeUpdated = d.PalletTypeUpdated.ToArray()}).ToArray()
                Return PalletTypes

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

    Public Function GetPalletTypebyFiltered(ByVal LaneFeesControl As Integer) As LTS.PalletType

        Dim oRet As LTS.PalletType 'return the table or view
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                oRet = db.PalletTypes.Where(Function(x) x.ID = LaneFeesControl).FirstOrDefault() 'added firstordefault to fix edit error Manorama' 16-Jun-2020
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetPalletTypebyFiltered"), db)
            End Try
            Return oRet
        End Using
    End Function

    ''' <summary>
    ''' Method to Get PalletType Records 
    ''' </summary>
    ''' <param name="filters">filters</param>
    ''' <param name="RecordCount">No Of records</param>
    ''' <remarks>
    ''' Added By Suhas On 07-SEP-2020
    ''' 
    ''' </remarks>
    ''' <returns>LTS LaneTrans</returns>
    Public Function GetPalletTypeFiltered(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.PalletType()
        If filters Is Nothing Then Return Nothing
        Dim itransControl As Integer = 0 'PK Control Number
        Dim filterWhere As String = ""
        Dim sFilterSpacer As String = ""

        Dim oRet() As LTS.PalletType 'return the table or view
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                If (filters.FilterValues Is Nothing OrElse filters.FilterValues.Count() < 1) OrElse (Not filters.FilterValues.Any(Function(x) x.filterName = "ID")) Then
                    'The Record Control Filter does not exist so use the parent control fliter

                Else
                    Dim tFilter As Models.FilterDetails = filters.FilterValues.Where(Function(x) x.filterName = "ID").FirstOrDefault()
                    Integer.TryParse(tFilter.filterValueFrom, itransControl)
                End If

                Dim iQuery As IQueryable(Of LTS.PalletType)
                iQuery = db.PalletTypes
                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "ID"
                    filters.sortDirection = "desc"
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetPalletTypeFiltered"), db)
            End Try
            Return oRet
        End Using
    End Function

    Public Function SaveOrCreatePalletType(ByVal oData As LTS.PalletType) As Boolean
        Dim blnRet As Boolean = False
        If oData Is Nothing Then Return False 'nothing to do

        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'Dim iLaneTranID = oData.ID
                'If oData.LaneFeesLaneControl = 0 Then
                'If iLaneTranID = 0 Then
                '    Dim sMsg As String = "E_MissingParent" ' " The reference to the parent record is missing. Please select a valid parent record and try again."
                '    throwNoDataFaultException(sMsg)
                'End If

                'End If

                '    Dim blnProcessed As Boolean = False
                'oData.LaneFeesModDate = Date.Now()
                'oData.LaneFeesModUser = Me.Parameters.UserName

                If oData.ID = 0 Then
                    db.PalletTypes.InsertOnSubmit(oData)
                Else
                    db.PalletTypes.Attach(oData, True)
                End If
                db.SubmitChanges()

                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveOrCreatePalletType"), db)
            End Try
        End Using
        Return blnRet
    End Function

    Public Function DeletePalletType(ByVal iLaneTranID As Integer) As Boolean
        Dim blnRet As Boolean = False

        If iLaneTranID = 0 Then Return False 'nothing to do
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'verify the service contract
                Dim oExisting = db.PalletTypes.Where(Function(x) x.ID = iLaneTranID).FirstOrDefault()
                If oExisting Is Nothing OrElse oExisting.ID = 0 Then Return True
                db.PalletTypes.DeleteOnSubmit(oExisting)
                db.SubmitChanges()

                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeletePalletType"), db)
            End Try
        End Using
        Return blnRet
    End Function
#End Region

#Region "Protected Functions"
    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.PalletType)
        'Create New Record
        Return New LTS.PalletType With {.ID = d.ID _
                                              , .PalletType = d.PalletTypeName _
                                              , .PalletTypeBitPos = d.PalletTypeName _
                                              , .PalletTypeContainer = d.PalletTypeContainer _
                                              , .PalletTypeDepth = d.PalletTypeDepth _
                                              , .PalletTypeDescription = d.PalletTypeDescription _
                                              , .PalletTypeHeight = d.PalletTypeHeight _
                                              , .PalletTypeVolume = d.PalletTypeVolume _
                                              , .PalletTypeWeight = d.PalletTypeWeight _
                                              , .PalletTypeWidth = d.PalletTypeWidth _
                                              , .PalletTypeUpdated = If(d.PalletTypeUpdated Is Nothing, New Byte() {}, d.PalletTypeUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetPalletTypeFiltered(Control:=CType(LinqTable, LTS.PalletType).ID)
    End Function

#End Region

End Class

Public Class NGLNegativeRevenueReasonData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMASLookupDataContext(ConnectionString)
        Me.LinqTable = db.NegativeRevenueReasons
        Me.LinqDB = db
        Me.SourceClass = "NGLNegativeRevenueReasonData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLookupDataContext(ConnectionString)
            Me.LinqTable = db.NegativeRevenueReasons
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
        Return GetNegativeRevenueReasonFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetNegativeRevenueReasonsFiltered()
    End Function

    Public Function GetNegativeRevenueReasonFiltered(Optional ByVal Control As Integer = 0, Optional ByVal Reason As String = "") As DTO.NegativeRevenueReason
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim NegativeRevenueReason As DTO.NegativeRevenueReason = (
                From d In db.NegativeRevenueReasons
                Where
                    (Control = 0 OrElse d.NegativeRevenueCode = Control) _
                     And
                     (Reason Is Nothing OrElse String.IsNullOrEmpty(Reason) OrElse d.NegativeRevenueReason = Reason)
                Select New DTO.NegativeRevenueReason With {.NegativeRevenueCode = d.NegativeRevenueCode _
                                              , .NegativeRevenueReason = d.NegativeRevenueReason _
                                              , .NegativeRevenueUpdated = d.NegativeRevenueUpdated.ToArray()}).First


                Return NegativeRevenueReason

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

    Public Function GetNegativeRevenueReasonsFiltered(Optional ByVal Reason As String = "") As DTO.NegativeRevenueReason()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'Return all the records that match the criteria 
                Dim NegativeRevenueReasons() As DTO.NegativeRevenueReason = (
                From d In db.NegativeRevenueReasons
                Where
                    (Reason Is Nothing OrElse String.IsNullOrEmpty(Reason) OrElse d.NegativeRevenueReason = Reason)
                Order By d.NegativeRevenueCode
                Select New DTO.NegativeRevenueReason With {.NegativeRevenueCode = d.NegativeRevenueCode _
                                              , .NegativeRevenueReason = d.NegativeRevenueReason _
                                              , .NegativeRevenueUpdated = d.NegativeRevenueUpdated.ToArray()}).ToArray()
                Return NegativeRevenueReasons

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

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.NegativeRevenueReason)
        'Create New Record
        Return New LTS.NegativeRevenueReason With {.NegativeRevenueCode = d.NegativeRevenueCode _
                                              , .NegativeRevenueReason = d.NegativeRevenueReason _
                                              , .NegativeRevenueUpdated = If(d.NegativeRevenueUpdated Is Nothing, New Byte() {}, d.NegativeRevenueUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetNegativeRevenueReasonFiltered(Control:=CType(LinqTable, LTS.NegativeRevenueReason).NegativeRevenueCode)
    End Function

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DTO.NegativeRevenueReason)
            Try

                'Check required fields
                Dim blnValidationFailed As Boolean = False
                Dim strMsg As String = ""
                Dim strSpacer As String = ""


                If String.IsNullOrEmpty(.NegativeRevenueReason) OrElse .NegativeRevenueReason.Trim.Length < 1 Then
                    blnValidationFailed = True
                    strMsg = "NegativeRevenueReason"
                    strSpacer = ", "
                End If
                If blnValidationFailed Then
                    Utilities.SaveAppError("Cannot save Negative Revenue Reason Code changes because the following fields are required: " & strMsg & ".", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If
                If .NegativeRevenueCode = 0 Then
                    'Get the next available code
                    .NegativeRevenueCode = CType(oDB, NGLMASLookupDataContext).udfGetNextNegRevReasonCode()
                End If


                'check if the data already exists
                Dim oExists As DTO.NegativeRevenueReason = (
                From t In CType(oDB, NGLMASLookupDataContext).NegativeRevenueReasons
                Where
                     (t.NegativeRevenueCode = .NegativeRevenueCode) _
                     Or
                     (t.NegativeRevenueReason = .NegativeRevenueReason)
                Select New DTO.NegativeRevenueReason With {.NegativeRevenueCode = t.NegativeRevenueCode, .NegativeRevenueReason = t.NegativeRevenueReason}).First

                If Not oExists Is Nothing Then
                    Utilities.SaveAppError("Cannot save new Negative Revenue Reason Code.  The data already exists.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If


            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DTO.NegativeRevenueReason)
            Try
                'Check required fields
                Dim blnValidationFailed As Boolean = False
                Dim strMsg As String = ""
                Dim strSpacer As String = ""

                If .NegativeRevenueCode = 0 Then
                    blnValidationFailed = True
                    strMsg = "NegativeRevenueCode"
                    strSpacer = ", "
                End If

                If String.IsNullOrEmpty(.NegativeRevenueReason) OrElse .NegativeRevenueReason.Trim.Length < 1 Then
                    blnValidationFailed = True
                    strMsg = "NegativeRevenueReason"
                    strSpacer = ", "
                End If
                If blnValidationFailed Then
                    Utilities.SaveAppError("Cannot save Negative Revenue Reason Code changes because the following fields are required: " & strMsg & ".", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If
                'check if the data already exists
                Dim oExists As DTO.NegativeRevenueReason = (
                From t In CType(oDB, NGLMASLookupDataContext).NegativeRevenueReasons
                Where
                     (t.NegativeRevenueCode <> .NegativeRevenueCode) _
                     And
                     (t.NegativeRevenueReason = .NegativeRevenueReason)
                Select New DTO.NegativeRevenueReason With {.NegativeRevenueCode = t.NegativeRevenueCode, .NegativeRevenueReason = t.NegativeRevenueReason}).First

                If Not oExists Is Nothing Then
                    Utilities.SaveAppError("Cannot save Negative Revenue Reason Code changes.  The Code , " & .NegativeRevenueReason & " already exist.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the Book is being used by the book data or the lane data
        With CType(oData, DTO.NegativeRevenueReason)
            Try
                Dim blnInUse As Boolean = CType(oDB, NGLMASLookupDataContext).udfIsNegRevReasonInUse(.NegativeRevenueCode)
                If blnInUse Then
                    Utilities.SaveAppError("Cannot delete Negative Revenue Reason Code.  The Code , " & .NegativeRevenueReason & " is being used and cannot be deleted. check the book information.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DataInUse"}, New FaultReason("E_DataValidationFailure"))
                End If
            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            End Try
        End With
    End Sub

#End Region

End Class

Public Class NGLCurrencyData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLookupDataContext(ConnectionString)
        Me.LinqTable = db.Currencies
        Me.LinqDB = db
        Me.SourceClass = "NGLCurrencyData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLookupDataContext(ConnectionString)
            Me.LinqTable = db.Currencies
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
        Return GetCurrencyFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetCurrenciesFiltered()
    End Function

    Public Function GetCurrencyFiltered(Optional ByVal Control As Integer = 0, Optional ByVal CurrencyName As String = "") As DTO.Currency
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim Currency As DTO.Currency = (
                From d In db.Currencies
                Where
                    (d.ID = If(Control = 0, d.ID, Control)) _
                     And
                     (CurrencyName Is Nothing OrElse String.IsNullOrEmpty(CurrencyName) OrElse d.CurrencyName = CurrencyName)
                Select New DTO.Currency With {.ID = d.ID _
                                              , .CurrencyType = d.CurrencyType _
                                              , .CurrencyName = d.CurrencyName _
                                              , .CurrencyCountry = d.CurrencyCountry _
                                              , .CurrencyUpdated = d.CurrencyUpdated.ToArray()}).First


                Return Currency

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

    Public Function GetCurrenciesFiltered(Optional ByVal CurrencyType As String = "", Optional ByVal CurrencyCountry As String = "") As DTO.Currency()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim Currencies() As DTO.Currency = (
                From d In db.Currencies
                Where
                    (CurrencyType Is Nothing OrElse String.IsNullOrEmpty(CurrencyType) OrElse d.CurrencyType = CurrencyType) _
                    And
                    (CurrencyCountry Is Nothing OrElse String.IsNullOrEmpty(CurrencyCountry) OrElse d.CurrencyCountry = CurrencyCountry)
                Order By d.CurrencyName
                Select New DTO.Currency With {.ID = d.ID _
                                              , .CurrencyType = d.CurrencyType _
                                              , .CurrencyName = d.CurrencyName _
                                              , .CurrencyCountry = d.CurrencyCountry _
                                              , .CurrencyUpdated = d.CurrencyUpdated.ToArray()}).ToArray()
                Return Currencies

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


    Public Function GetCurrenciesbyFiltered(ByVal LaneFeesControl As Integer) As LTS.Currency
        Dim oRet As LTS.Currency 'return the table or view
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                oRet = db.Currencies.Where(Function(x) x.ID = LaneFeesControl).FirstOrDefault() 'added firstordefault to fix edit error Manorama' 16-Jun-2020
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCurrenciesbyFiltered"), db)
            End Try
            Return oRet
        End Using
    End Function

    ''' <summary>
    ''' Method to Get Currency Records 
    ''' </summary>
    ''' <param name="filters">filters</param>
    ''' <param name="RecordCount">No Of records</param>
    ''' <remarks>
    ''' Added By Suhas On 05-SEP-2020
    ''' 
    ''' </remarks>
    ''' <returns>LTS Currency</returns>
    Public Function GetCurrencyFiltered(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.Currency()
        If filters Is Nothing Then Return Nothing
        Dim itransControl As Integer = 0 'PK Control Number
        Dim filterWhere As String = ""
        Dim sFilterSpacer As String = ""

        Dim oRet() As LTS.Currency 'return the table or view
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                If (filters.FilterValues Is Nothing OrElse filters.FilterValues.Count() < 1) OrElse (Not filters.FilterValues.Any(Function(x) x.filterName = "ID")) Then
                    'The Record Control Filter does not exist so use the parent control fliter

                Else
                    Dim tFilter As Models.FilterDetails = filters.FilterValues.Where(Function(x) x.filterName = "ID").FirstOrDefault()
                    Integer.TryParse(tFilter.filterValueFrom, itransControl)
                End If

                Dim iQuery As IQueryable(Of LTS.Currency)
                iQuery = db.Currencies
                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "ID"
                    filters.sortDirection = "desc"
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCurrenciesFiltered"), db)
            End Try
            Return oRet
        End Using
    End Function

    Public Function SaveOrCreateCurrency(ByVal oData As LTS.Currency) As Boolean
        Dim blnRet As Boolean = False
        If oData Is Nothing Then Return False 'nothing to do

        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'Dim iLaneTranID = oData.ID
                'If oData.LaneFeesLaneControl = 0 Then
                'If iLaneTranID = 0 Then
                '    Dim sMsg As String = "E_MissingParent" ' "The reference to the parent record is missing. Please select a valid parent record and try again."
                '    throwNoDataFaultException(sMsg)
                'End If

                'End If

                '    Dim blnProcessed As Boolean = False
                'oData.LaneFeesModDate = Date.Now()
                'oData.LaneFeesModUser = Me.Parameters.UserName

                If oData.ID = 0 Then
                    db.Currencies.InsertOnSubmit(oData)
                Else
                    db.Currencies.Attach(oData, True)
                End If
                db.SubmitChanges()

                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveOrCreateCurrency"), db)
            End Try
        End Using
        Return blnRet
    End Function

    Public Function DeleteCurrency(ByVal iLaneTranID As Integer) As Boolean
        Dim blnRet As Boolean = False

        If iLaneTranID = 0 Then Return False 'nothing to do
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'verify the service contract
                Dim oExisting = db.Currencies.Where(Function(x) x.ID = iLaneTranID).FirstOrDefault()
                If oExisting Is Nothing OrElse oExisting.ID = 0 Then Return True
                db.Currencies.DeleteOnSubmit(oExisting)
                db.SubmitChanges()

                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteCurrency"), db)
            End Try
        End Using
        Return blnRet
    End Function

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.Currency)
        'Create New Record
        Return New LTS.Currency With {.ID = d.ID _
                                              , .CurrencyType = d.CurrencyType _
                                              , .CurrencyName = d.CurrencyName _
                                              , .CurrencyCountry = d.CurrencyCountry _
                                              , .CurrencyUpdated = If(d.CurrencyUpdated Is Nothing, New Byte() {}, d.CurrencyUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCurrencyFiltered(Control:=CType(LinqTable, LTS.Currency).ID)
    End Function

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DTO.Currency)
            Try
                Dim oExists As DTO.Currency = (
                    From t In CType(oDB, NGLMASLookupDataContext).Currencies
                    Where
                     t.CurrencyType = .CurrencyType
                    Select New DTO.Currency With {.ID = t.ID}).First

                If Not oExists Is Nothing Then
                    Utilities.SaveAppError("Cannot save new Currency data.  The Currency Type, " & .CurrencyType & ", already exist.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DTO.Currency)
            Try
                'Get the newest record that matches the provided criteria
                Dim oExists As DTO.Currency = (
                    From t In CType(oDB, NGLMASLookupDataContext).Currencies
                    Where
                     t.ID <> .ID _
                     And
                     t.CurrencyType = .CurrencyType
                    Select New DTO.Currency With {.ID = t.ID}).First


                If Not oExists Is Nothing Then
                    Utilities.SaveAppError("Cannot save Currency changes.  The Currency Type, " & .CurrencyType & ", already exist.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

#End Region

End Class

Public Class NGLPaymentFormData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLookupDataContext(ConnectionString)
        Me.LinqTable = db.PaymentForms
        Me.LinqDB = db
        Me.SourceClass = "NGLPaymentFormData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLookupDataContext(ConnectionString)
            Me.LinqTable = db.PaymentForms
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
        Return GetPaymentFormFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetPaymentFormsFiltered()
    End Function

    Public Function GetPaymentFormFiltered(Optional ByVal Control As Integer = 0) As DTO.PaymentForm
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim PaymentForm As DTO.PaymentForm = (
                From d In db.PaymentForms
                Where
                    (d.PaymentFormNumber = If(Control = 0, d.PaymentFormNumber, Control))
                Select New DTO.PaymentForm With {.PaymentFormNumber = d.PaymentFormNumber _
                                              , .PaymentFormType = d.PaymentFormType _
                                              , .PaymentFormUpdated = d.PaymentFormUpdated.ToArray()}).First


                Return PaymentForm

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

    Public Function GetPaymentFormsFiltered() As DTO.PaymentForm()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim PaymentForms() As DTO.PaymentForm = (
                From d In db.PaymentForms
                Order By d.PaymentFormType
                Select New DTO.PaymentForm With {.PaymentFormNumber = d.PaymentFormNumber _
                                              , .PaymentFormType = d.PaymentFormType _
                                              , .PaymentFormUpdated = d.PaymentFormUpdated.ToArray()}).ToArray()
                Return PaymentForms

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

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.PaymentForm)
        'Create New Record
        Return New LTS.PaymentForm With {.PaymentFormNumber = d.PaymentFormNumber _
                                              , .PaymentFormType = d.PaymentFormType _
                                              , .PaymentFormUpdated = If(d.PaymentFormUpdated Is Nothing, New Byte() {}, d.PaymentFormUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetPaymentFormFiltered(Control:=CType(LinqTable, LTS.PaymentForm).PaymentFormNumber)
    End Function

#End Region

End Class

Public Class NGLCommodityCodeData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLookupDataContext(ConnectionString)
        Me.LinqTable = db.CommodityCodes
        Me.LinqDB = db
        Me.SourceClass = "NGLCommodityCodeData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLookupDataContext(ConnectionString)
            Me.LinqTable = db.CommodityCodes
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
        Return Nothing
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetCommodityCodesFiltered()
    End Function

    ''' <summary>
    ''' Reads a Commodity record from the Commodity table
    ''' Used instead of the TempType table which has been Depricated
    ''' </summary>
    ''' <param name="type"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' follows standard design patterns and throws SQLFaultExceptions on error
    ''' Modified 1/09/2016 by RHR v-7.0.5
    '''   we now use FirstOrDefault() instead of throwing a fault excepton if no data is availble
    '''   will return an empty Commodity object if no data exists.  the caller must check the value of ID
    ''' </remarks>
    Public Function GetCommodityCodeFiltered(Optional ByVal type As String = "") As DTO.Commodity
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim CommodityType As DTO.Commodity = (
                From d In db.CommodityCodes
                Where
                    (d.CommCodeType = If(type = "", d.CommCodeType, type))
                Select New DTO.Commodity With {.CommCodeType = d.CommCodeType _
                                              , .CommCodeDescription = d.CommCodeDescription _
                                              , .ID = d.ID _
                                              , .TempType = d.TempType _
                                              , .CommCodeUpdated = d.CommCodeUpdated.ToArray()}).FirstOrDefault()


                Return CommodityType

            Catch ex As Exception
                ManageLinqDataExceptions(ex, "GetCommodityCodeFiltered", db)
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetCommodityCodesFiltered() As DTO.Commodity()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim CommodityTypes() As DTO.Commodity = (
                From d In db.CommodityCodes
                Order By d.CommCodeType
                Select New DTO.Commodity With {.CommCodeType = d.CommCodeType _
                                              , .CommCodeDescription = d.CommCodeDescription _
                                              , .ID = d.ID _
                                              , .TempType = d.TempType _
                                              , .CommCodeUpdated = d.CommCodeUpdated.ToArray()}).ToArray()
                Return CommodityTypes

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

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.Commodity)
        'Create New Record
        Return New LTS.CommodityCode With {.CommCodeType = d.CommCodeType _
                                              , .CommCodeDescription = d.CommCodeDescription _
                                          , .TempType = d.TempType _
                                          , .ID = d.ID _
                                             , .CommCodeUpdated = If(d.CommCodeUpdated Is Nothing, New Byte() {}, d.CommCodeUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCommodityCodeFiltered(type:=(CType(LinqTable, LTS.CommodityCode).CommCodeType))
    End Function

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DTO.Commodity)
            Try

                'Check required fields
                Dim blnValidationFailed As Boolean = False
                Dim strMsg As String = ""
                Dim strSpacer As String = ""


                If String.IsNullOrEmpty(.CommCodeType) OrElse .CommCodeType.Trim.Length < 1 Then
                    blnValidationFailed = True
                    strMsg = "CommodityCode"
                    strSpacer = ", "
                End If
                If blnValidationFailed Then
                    Utilities.SaveAppError("Cannot save Commodity Code changes because the following fields are required: " & strMsg & ".", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

                'check if the data already exists
                Dim oExists As DTO.Commodity = (
                From t In CType(oDB, NGLMASLookupDataContext).CommodityCodes
                Where
                     (t.CommCodeType = .CommCodeType)
                Select New DTO.Commodity With {.CommCodeType = t.CommCodeType}).First

                If Not oExists Is Nothing Then
                    Utilities.SaveAppError("Cannot save new Commodity Code.  The data already exists.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If


            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DTO.Commodity)
            Try
                'Check required fields
                Dim blnValidationFailed As Boolean = False
                Dim strMsg As String = ""
                Dim strSpacer As String = ""

                If .CommCodeType = "" Then
                    blnValidationFailed = True
                    strMsg = "CommodityCode"
                    strSpacer = ", "
                End If

                If String.IsNullOrEmpty(.CommCodeType) OrElse .CommCodeType.Trim.Length < 1 Then
                    blnValidationFailed = True
                    strMsg = "CommodityCode"
                    strSpacer = ", "
                End If
                If blnValidationFailed Then
                    Utilities.SaveAppError("Cannot save Commodity Code changes because the following fields are required: " & strMsg & ".", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If
                'check if the data already exists
                Dim oExists As DTO.Commodity = (
                From t In CType(oDB, NGLMASLookupDataContext).CommodityCodes
                Where
                     (t.CommCodeType <> .CommCodeType)
                Select New DTO.Commodity With {.CommCodeType = t.CommCodeType}).First

                If Not oExists Is Nothing Then
                    Utilities.SaveAppError("Cannot save Commodity Code changes.  The Code , " & .CommCodeType & " already exist.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

#End Region

End Class

Public Class NGLCreditCardTypeData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLookupDataContext(ConnectionString)
        Me.LinqTable = db.CreditCardTypes
        Me.LinqDB = db
        Me.SourceClass = "NGLCreditCardTypeData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLookupDataContext(ConnectionString)
            Me.LinqTable = db.CreditCardTypes
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
        Return GetCreditCardTypeFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetCreditCardTypesFiltered()
    End Function

    Public Function GetCreditCardTypeFiltered(Optional ByVal Control As Integer = 0) As DTO.CreditCardType
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim CreditCardType As DTO.CreditCardType = (
                From d In db.CreditCardTypes
                Where
                    (d.CreditCardControlNumber = If(Control = 0, d.CreditCardControlNumber, Control))
                Select New DTO.CreditCardType With {.CreditCardControlNumber = d.CreditCardControlNumber _
                                              , .CreditCardTypeName = d.CreditCardType _
                                              , .CreditCardTypesUpdated = d.CreditCardTypesUpdated.ToArray()}).First


                Return CreditCardType

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

    Public Function GetCreditCardTypesFiltered() As DTO.CreditCardType()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim CreditCardTypes() As DTO.CreditCardType = (
                From d In db.CreditCardTypes
                Order By d.CreditCardType
                Select New DTO.CreditCardType With {.CreditCardControlNumber = d.CreditCardControlNumber _
                                              , .CreditCardTypeName = d.CreditCardType _
                                              , .CreditCardTypesUpdated = d.CreditCardTypesUpdated.ToArray()}).ToArray()
                Return CreditCardTypes

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

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.CreditCardType)
        'Create New Record
        Return New LTS.CreditCardType With {.CreditCardControlNumber = d.CreditCardControlNumber _
                                              , .CreditCardType = d.CreditCardTypeName _
                                              , .CreditCardTypesUpdated = If(d.CreditCardTypesUpdated Is Nothing, New Byte() {}, d.CreditCardTypesUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCreditCardTypeFiltered(Control:=CType(LinqTable, LTS.CreditCardType).CreditCardControlNumber)
    End Function

#End Region

End Class

Public Class NGLSeasonalityData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLookupDataContext(ConnectionString)
        Me.LinqTable = db.Seasonalities
        Me.LinqDB = db
        Me.SourceClass = "NGLSeasonalityData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLookupDataContext(ConnectionString)
            Me.LinqTable = db.Seasonalities
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
        Return GetSeasonalityFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetSeasonalitiesFiltered()
    End Function

    Public Function GetSeasonalityFiltered(Optional ByVal Control As Integer = 0) As DTO.Seasonality
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim Seasonality As DTO.Seasonality = (
                From d In db.Seasonalities
                Where
                    (d.SeasControl = If(Control = 0, d.SeasControl, Control))
                Select New DTO.Seasonality With {.SeasControl = d.SeasControl _
                                          , .SeasProfileNo = d.SeasProfileNo _
                                          , .SeasDescription = d.SeasDescription _
                                          , .SeasMo1 = If(d.SeasMo1.HasValue, d.SeasMo1.Value, 0) _
                                          , .SeasMo2 = If(d.SeasMo2.HasValue, d.SeasMo2.Value, 0) _
                                          , .SeasMo3 = If(d.SeasMo3.HasValue, d.SeasMo3.Value, 0) _
                                          , .SeasMo4 = If(d.SeasMo4.HasValue, d.SeasMo4.Value, 0) _
                                          , .SeasMo5 = If(d.SeasMo5.HasValue, d.SeasMo5.Value, 0) _
                                          , .SeasMo6 = If(d.SeasMo6.HasValue, d.SeasMo6.Value, 0) _
                                          , .SeasMo7 = If(d.SeasMo7.HasValue, d.SeasMo7.Value, 0) _
                                          , .SeasMo8 = If(d.SeasMo8.HasValue, d.SeasMo8.Value, 0) _
                                          , .SeasMo9 = If(d.SeasMo9.HasValue, d.SeasMo9.Value, 0) _
                                          , .SeasMo10 = If(d.SeasMo10.HasValue, d.SeasMo10.Value, 0) _
                                          , .SeasMo11 = If(d.SeasMo11.HasValue, d.SeasMo11.Value, 0) _
                                          , .SeasMo12 = If(d.SeasMo12.HasValue, d.SeasMo12.Value, 0) _
                                          , .SeasModUser = d.SeasModUser _
                                          , .SeasModDate = d.SeasModDate _
                                          , .SeasUpdated = d.SeasUpdated.ToArray()}).First


                Return Seasonality

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

    Public Function GetSeasonalitiesFiltered() As DTO.Seasonality()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim Seasonalities() As DTO.Seasonality = (
                From d In db.Seasonalities
                Order By d.SeasControl
                Select New DTO.Seasonality With {.SeasControl = d.SeasControl _
                                          , .SeasProfileNo = d.SeasProfileNo _
                                          , .SeasDescription = d.SeasDescription _
                                          , .SeasMo1 = If(d.SeasMo1.HasValue, d.SeasMo1.Value, 0) _
                                          , .SeasMo2 = If(d.SeasMo2.HasValue, d.SeasMo2.Value, 0) _
                                          , .SeasMo3 = If(d.SeasMo3.HasValue, d.SeasMo3.Value, 0) _
                                          , .SeasMo4 = If(d.SeasMo4.HasValue, d.SeasMo4.Value, 0) _
                                          , .SeasMo5 = If(d.SeasMo5.HasValue, d.SeasMo5.Value, 0) _
                                          , .SeasMo6 = If(d.SeasMo6.HasValue, d.SeasMo6.Value, 0) _
                                          , .SeasMo7 = If(d.SeasMo7.HasValue, d.SeasMo7.Value, 0) _
                                          , .SeasMo8 = If(d.SeasMo8.HasValue, d.SeasMo8.Value, 0) _
                                          , .SeasMo9 = If(d.SeasMo9.HasValue, d.SeasMo9.Value, 0) _
                                          , .SeasMo10 = If(d.SeasMo10.HasValue, d.SeasMo10.Value, 0) _
                                          , .SeasMo11 = If(d.SeasMo11.HasValue, d.SeasMo11.Value, 0) _
                                          , .SeasMo12 = If(d.SeasMo12.HasValue, d.SeasMo12.Value, 0) _
                                          , .SeasModUser = d.SeasModUser _
                                          , .SeasModDate = d.SeasModDate _
                                          , .SeasUpdated = d.SeasUpdated.ToArray()}).ToArray()
                Return Seasonalities

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

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.Seasonality)
        'Create New Record
        Return New LTS.Seasonality With {.SeasControl = d.SeasControl _
                                          , .SeasProfileNo = d.SeasProfileNo _
                                          , .SeasDescription = d.SeasDescription _
                                          , .SeasMo1 = d.SeasMo1 _
                                          , .SeasMo2 = d.SeasMo2 _
                                          , .SeasMo3 = d.SeasMo3 _
                                          , .SeasMo4 = d.SeasMo4 _
                                          , .SeasMo5 = d.SeasMo5 _
                                          , .SeasMo6 = d.SeasMo6 _
                                          , .SeasMo7 = d.SeasMo7 _
                                          , .SeasMo8 = d.SeasMo8 _
                                          , .SeasMo9 = d.SeasMo9 _
                                          , .SeasMo10 = d.SeasMo10 _
                                          , .SeasMo11 = d.SeasMo11 _
                                          , .SeasMo12 = d.SeasMo12 _
                                          , .SeasModUser = Parameters.UserName _
                                          , .SeasModDate = Date.Now _
                                          , .SeasUpdated = If(d.SeasUpdated Is Nothing, New Byte() {}, d.SeasUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetSeasonalityFiltered(Control:=CType(LinqTable, LTS.Seasonality).SeasControl)
    End Function

#End Region

End Class

Public Class NGLStateData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLookupDataContext(ConnectionString)
        Me.LinqTable = db.States
        Me.LinqDB = db
        Me.SourceClass = "NGLStateData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLookupDataContext(ConnectionString)
            Me.LinqTable = db.States
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
        Return GetStateFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetStatesFiltered()
    End Function

    Public Function GetStateFiltered(Optional ByVal Control As Integer = 0, Optional ByVal StateAbv As String = "", Optional ByVal Statename As String = "") As DTO.State
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim State As DTO.State = (
                From d In db.States
                Where
                    (d.StateControl = If(Control = 0, d.StateControl, Control)) _
                    And
                    (StateAbv Is Nothing OrElse String.IsNullOrEmpty(StateAbv) OrElse d.STATE = StateAbv) _
                    And
                    (Statename Is Nothing OrElse String.IsNullOrEmpty(Statename) OrElse d.STATENAME = Statename)
                Select New DTO.State With {.StateControl = d.StateControl _
                                              , .STATEABV = d.STATE _
                                              , .STATENAME = d.STATENAME _
                                              , .StatesUpdated = d.StatesUpdated.ToArray()}).First


                Return State

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

    Public Function GetStatesFiltered() As DTO.State()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim States() As DTO.State = (
                From d In db.States
                Order By d.STATE
                Select New DTO.State With {.StateControl = d.StateControl _
                                              , .STATEABV = d.STATE _
                                              , .STATENAME = d.STATENAME _
                                              , .StatesUpdated = d.StatesUpdated.ToArray()}).ToArray()
                Return States

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
    ''' Check if the value of sState is an exact match in the States table.  optional parameter sCountry is for Future, US is the Default
    ''' </summary>
    ''' <param name="sState"></param>
    ''' <param name="sCountry"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.4.0.002 on 04/14/2021
    '''     logic used to validate the shipping state information for Carrier API dispatching
    ''' </remarks>
    Public Function ValidateStateShort(ByVal sState As String, Optional ByVal sCountry As String = "US") As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim oState = db.States.Where(Function(x) x.STATE = sState).FirstOrDefault()
                'In the future we should add a country code to the States Table and use the code
                'For now we just check the list for a match
                If Not oState Is Nothing AndAlso oState.StateControl <> 0 Then blnRet = True 'we have a match


            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("ValidateStateShort"), db)
            End Try

            Return Nothing

        End Using
    End Function

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.State)
        'Create New Record
        Return New LTS.State With {.StateControl = d.StateControl _
                                              , .STATE = d.STATEABV _
                                              , .STATENAME = d.STATENAME _
                                              , .StatesUpdated = If(d.StatesUpdated Is Nothing, New Byte() {}, d.StatesUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetStateFiltered(Control:=CType(LinqTable, LTS.State).StateControl)
    End Function

#End Region

End Class

Public Class NGLtblCountries : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLookupDataContext(ConnectionString)
        Me.LinqTable = db.tblCountries
        Me.LinqDB = db
        Me.SourceClass = "NGLtblCountries"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLookupDataContext(ConnectionString)
            Me.LinqTable = db.tblCountries
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
        Return GetCountryFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetCountriesFiltered()
    End Function

    Public Function GetCountryFiltered(Optional ByVal Control As Integer = 0, Optional ByVal CountryISO As String = "", Optional ByVal CountryName As String = "") As DTO.tblCountries
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim Country As DTO.tblCountries = (
                From d In db.tblCountries
                Where
                    (d.CountryControl = If(Control = 0, d.CountryControl, Control)) _
                    And
                    (CountryISO Is Nothing OrElse String.IsNullOrEmpty(CountryISO) OrElse d.CountryISO = CountryISO) _
                    And
                    (CountryName Is Nothing OrElse String.IsNullOrEmpty(CountryName) OrElse d.CountryName = CountryName)
                Select New DTO.tblCountries With {.CountryControl = d.CountryControl _
                                              , .CountryISO = d.CountryISO _
                                              , .CountryName = d.CountryName _
                                              , .CountryUpdated = d.CountryUpdated.ToArray()}).First


                Return Country

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

    Public Function GetCountriesFiltered() As DTO.tblCountries()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim Countries() As DTO.tblCountries = (
                From d In db.tblCountries
                Order By d.CountryISO
                Select New DTO.tblCountries With {.CountryControl = d.CountryControl _
                                              , .CountryISO = d.CountryISO _
                                              , .CountryName = d.CountryName _
                                              , .CountryUpdated = d.CountryUpdated.ToArray()}).ToArray()
                Return Countries

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

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.tblCountries)
        'Create New Record
        Return New LTS.tblCountry With {.CountryControl = d.CountryControl _
                                              , .CountryISO = d.CountryISO _
                                              , .CountryName = d.CountryName _
                                              , .CountryUpdated = If(d.CountryUpdated Is Nothing, New Byte() {}, d.CountryUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCountryFiltered(Control:=CType(LinqTable, LTS.tblCountry).CountryControl)
    End Function

#End Region

End Class

Public Class NGLLaneTranData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLookupDataContext(ConnectionString)
        Me.LinqTable = db.LaneTrans
        Me.LinqDB = db
        Me.SourceClass = "NGLLaneTranData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLookupDataContext(ConnectionString)
            Me.LinqTable = db.LaneTrans
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
        Return GetLaneTranFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetLaneTransFiltered()
    End Function

    Public Function GetLaneTranFiltered(Optional ByVal Control As Integer = 0, Optional ByVal LaneTransNumber As Integer = 0) As DTO.LaneTran
        Using operation = Logger.StartActivity("GetLaneTranFiltered(Control: {Control}, LaneTransNumber: {LaneTransNumber}", Control, LaneTransNumber)
            Using db As New NGLMASLookupDataContext(ConnectionString)
                Try

                    'Get the newest record that matches the provided criteria
                    Dim LaneTran As DTO.LaneTran = (
                    From d In db.LaneTrans
                    Where
                        (d.ID = If(Control = 0, d.ID, Control)) _
                         And
                         (d.LaneTransNumber = If(LaneTransNumber = 0, d.LaneTransNumber, LaneTransNumber))
                    Select New DTO.LaneTran With {.ID = d.ID _
                                                  , .LaneTransNumber = d.LaneTransNumber _
                                                  , .LaneTransTypeDesc = d.LaneTransTypeDesc _
                                                  , .LaneTransServiceFeeMin = If(d.LaneTransServiceFeeMin.HasValue, d.LaneTransServiceFeeMin.Value, 0) _
                                                  , .LaneTransServiceFeeMax = If(d.LaneTransServiceFeeMax.HasValue, d.LaneTransServiceFeeMax.Value, 0) _
                                                  , .LaneTransServiceFeeFlat = If(d.LaneTransServiceFeeFlat.HasValue, d.LaneTransServiceFeeFlat.Value, 0) _
                                                  , .LaneTransServiceFeePerc = If(d.LaneTransServiceFeePerc.HasValue, d.LaneTransServiceFeePerc.Value, 0) _
                                                  , .LaneTransUpdated = d.LaneTransUpdated.ToArray()}).First


                    Return LaneTran

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

        End Using
    End Function

    Public Function GetLaneTransFiltered() As DTO.LaneTran()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim LaneTrans() As DTO.LaneTran = (
                From d In db.LaneTrans
                Order By d.LaneTransNumber
                Select New DTO.LaneTran With {.ID = d.ID _
                                              , .LaneTransNumber = d.LaneTransNumber _
                                              , .LaneTransTypeDesc = d.LaneTransTypeDesc _
                                              , .LaneTransServiceFeeMin = If(d.LaneTransServiceFeeMin.HasValue, d.LaneTransServiceFeeMin.Value, 0) _
                                              , .LaneTransServiceFeeMax = If(d.LaneTransServiceFeeMax.HasValue, d.LaneTransServiceFeeMax.Value, 0) _
                                              , .LaneTransServiceFeeFlat = If(d.LaneTransServiceFeeFlat.HasValue, d.LaneTransServiceFeeFlat.Value, 0) _
                                              , .LaneTransServiceFeePerc = If(d.LaneTransServiceFeePerc.HasValue, d.LaneTransServiceFeePerc.Value, 0) _
                                              , .LaneTransUpdated = d.LaneTransUpdated.ToArray()}).ToArray()
                Return LaneTrans

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

    Public Function GetLaneTranbyFiltered(ByVal LanetransControl As Integer) As LTS.LaneTran

        Dim oRet As LTS.LaneTran 'return the table or view
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                oRet = db.LaneTrans.Where(Function(x) x.ID = LanetransControl).FirstOrDefault() 'added firstordefault to fix edit error Manorama' 16-Jun-2020
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLaneTranbyFiltered"), db)
            End Try
            Return oRet
        End Using
    End Function
    ''' <summary>
    ''' Method to Get LaneTrans Records 
    ''' </summary>
    ''' <param name="filters">filters</param>
    ''' <param name="RecordCount">No Of records</param>
    ''' <remarks>
    ''' Added By Manorama On 04-SEP-2020
    ''' 
    ''' </remarks>
    ''' <returns>LTS LaneTrans</returns>
    Public Function GetLaneTranFiltered(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.LaneTran()
        If filters Is Nothing Then Return Nothing
        Dim itransControl As Integer = 0 'PK Control Number
        Dim filterWhere As String = ""
        Dim sFilterSpacer As String = ""

        Dim oRet() As LTS.LaneTran 'return the table or view
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                If (filters.FilterValues Is Nothing OrElse filters.FilterValues.Count() < 1) OrElse (Not filters.FilterValues.Any(Function(x) x.filterName = "ID")) Then
                    'The Record Control Filter does not exist so use the parent control fliter

                Else
                    Dim tFilter As Models.FilterDetails = filters.FilterValues.Where(Function(x) x.filterName = "ID").FirstOrDefault()
                    Integer.TryParse(tFilter.filterValueFrom, itransControl)
                End If

                Dim iQuery As IQueryable(Of LTS.LaneTran)
                iQuery = db.LaneTrans
                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "ID"
                    filters.sortDirection = "desc"
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLaneTranFiltered"), db)
            End Try
            Return oRet
        End Using
    End Function

    Public Function SaveOrCreateLaneTran(ByVal oData As LTS.LaneTran) As Boolean
        Dim blnRet As Boolean = False
        If oData Is Nothing Then Return False 'nothing to do

        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'Dim iLaneTranID = oData.ID
                'If oData.LaneFeesLaneControl = 0 Then
                'If iLaneTranID = 0 Then
                '    Dim sMsg As String = "E_MissingParent" ' "  The reference to the parent record is missing. Please select a valid parent record and try again."
                '    throwNoDataFaultException(sMsg)
                'End If

                'End If

                '    Dim blnProcessed As Boolean = False
                'oData.LaneFeesModDate = Date.Now()
                'oData.LaneFeesModUser = Me.Parameters.UserName

                If oData.ID = 0 Then
                    db.LaneTrans.InsertOnSubmit(oData)
                Else
                    db.LaneTrans.Attach(oData, True)
                End If
                db.SubmitChanges()

                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveOrCreateLaneTran"), db)
            End Try
        End Using
        Return blnRet
    End Function

    Public Function DeleteLaneTran(ByVal iLaneTranID As Integer) As Boolean
        Dim blnRet As Boolean = False

        If iLaneTranID = 0 Then Return False 'nothing to do
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'verify the service contract
                Dim oExisting = db.LaneTrans.Where(Function(x) x.ID = iLaneTranID).FirstOrDefault()
                If oExisting Is Nothing OrElse oExisting.ID = 0 Then Return True
                db.LaneTrans.DeleteOnSubmit(oExisting)
                db.SubmitChanges()

                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteLaneTran"), db)
            End Try
        End Using
        Return blnRet
    End Function
#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.LaneTran)
        'Create New Record
        Return New LTS.LaneTran With {.ID = d.ID _
                                      , .LaneTransNumber = d.LaneTransNumber _
                                      , .LaneTransTypeDesc = d.LaneTransTypeDesc _
                                      , .LaneTransServiceFeeMin = d.LaneTransServiceFeeMin _
                                      , .LaneTransServiceFeeMax = d.LaneTransServiceFeeMax _
                                      , .LaneTransServiceFeeFlat = d.LaneTransServiceFeeFlat _
                                      , .LaneTransServiceFeePerc = d.LaneTransServiceFeePerc _
                                      , .LaneTransUpdated = If(d.LaneTransUpdated Is Nothing, New Byte() {}, d.LaneTransUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetLaneTranFiltered(Control:=CType(LinqTable, LTS.LaneTran).ID)
    End Function

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DTO.LaneTran)
            Try
                Dim oExists As DTO.LaneTran = (
                    From t In CType(oDB, NGLMASLookupDataContext).LaneTrans
                    Where
                    t.LaneTransNumber = .LaneTransNumber _
                    Or
                    t.LaneTransTypeDesc = .LaneTransTypeDesc
                    Select New DTO.LaneTran With {.ID = t.ID}).First

                If Not oExists Is Nothing Then
                    Utilities.SaveAppError("Cannot save New LaneTrans data.  The LaneTransNumber, " & .LaneTransNumber & ", Or the LaneTransTypeDesc, " & .LaneTransTypeDesc & ", already exist.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DTO.LaneTran)
            Try
                'Get the newest record that matches the provided criteria
                Dim oExists As DTO.LaneTran = (
                    From t In CType(oDB, NGLMASLookupDataContext).LaneTrans
                    Where
                    t.ID <> .ID _
                    And
                    (t.LaneTransNumber = .LaneTransNumber _
                    Or
                    t.LaneTransTypeDesc = .LaneTransTypeDesc)
                    Select New DTO.LaneTran With {.ID = t.ID}).First


                If Not oExists Is Nothing Then
                    Utilities.SaveAppError("Cannot save LaneTrans changes.  The LaneTransNumber, " & .LaneTransNumber & ", Or the LaneTransTypeDesc, " & .LaneTransTypeDesc & ", already exist.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

#End Region

End Class

Public Class NGLTempTypeData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLookupDataContext(ConnectionString)
        Me.LinqTable = db.TempTypes
        Me.LinqDB = db
        Me.SourceClass = "NGLTempTypeData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLookupDataContext(ConnectionString)
            Me.LinqTable = db.TempTypes
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
        Return Nothing
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetTempTypesFiltered()
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="CommCodeType"></param>
    ''' <param name="ID"></param>
    ''' <param name="TariffTempType"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by LVV 5/4/16 for v-7.0.5.1 DAT
    ''' Added DATEquipTypeControl
    ''' </remarks>
    Public Function GetTempTypeFiltered(Optional ByVal CommCodeType As String = "", Optional ByVal ID As String = "", Optional ByVal TariffTempType As Integer = 0) As DTO.TempType
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim TempType As DTO.TempType = (
                From d In db.TempTypes
                Where
                    (d.CommCodeType = If(CommCodeType.Trim = "", d.CommCodeType, Left(CommCodeType.Trim, 1))) _
                    And
                    (ID Is Nothing OrElse String.IsNullOrEmpty(ID) OrElse d.ID = ID) _
                     And
                     (d.TariffTempType = If(TariffTempType = 0, d.TariffTempType, TariffTempType))
                Order By d.CommCodeType
                Select New DTO.TempType With {.CommCodeType = d.CommCodeType _
                                              , .CommCodeDescription = d.CommCodeDescription _
                                              , .ID = d.ID _
                                              , .TempType = d.TempType _
                                              , .TariffTempType = d.TariffTempType _
                                              , .DATEquipTypeControl = d.DATEquipTypeControl _
                                              , .TempTypeUpdated = d.TempTypeUpdated.ToArray()}).First


                Return TempType

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
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by LVV 5/4/16 for v-7.0.5.1 DAT
    ''' Added DATEquipTypeControl
    ''' </remarks>
    Public Function GetTempTypesFiltered() As DTO.TempType()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'Return all the records that match the criteria
                Dim TempTypes() As DTO.TempType = (
                From d In db.TempTypes
                Order By d.CommCodeType
                Select New DTO.TempType With {.CommCodeType = d.CommCodeType _
                                              , .CommCodeDescription = d.CommCodeDescription _
                                              , .ID = d.ID _
                                              , .TempType = d.TempType _
                                              , .TariffTempType = d.TariffTempType _
                                              , .DATEquipTypeControl = d.DATEquipTypeControl _
                                              , .TempTypeUpdated = d.TempTypeUpdated.ToArray()}).ToArray()
                Return TempTypes

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

    Public Function ConvertTempTypeIDToTariffTempType(ByVal ID As String) As Integer
        Dim intRet As Integer = 0
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                intRet = (From d In db.TempTypes Where d.ID = ID Select d.TariffTempType).FirstOrDefault()
            Catch ex As Exception
                'ignore any errors and just return 0 = Any
            End Try
            Return intRet
        End Using
    End Function

    '********************** TMS 365 Methods **********************
    ''' <summary>
    ''' Gets a list of TempTypes where the TempTypeBitPos is in the parameter array
    ''' </summary>
    ''' <param name="tmpList"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 6/19/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Public Function GetTempTypesByEnumIDs(ByVal tmpList() As Integer) As LTS.TempType()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                If tmpList Is Nothing OrElse tmpList.Count < 1 Then Return Nothing

                Return db.TempTypes.Where(Function(x) tmpList.Contains(x.TempTypeBitPos)).ToArray()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetTempTypesByEnumIDs"), db)
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Gets all PalletTypes and returns them as Models.DockPTType
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 6/19/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Public Function GetAllTempTypes() As Models.DockPTType()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                Dim tmps() As Models.DockPTType = (
                From t In db.TempTypes
                Order By t.TempType
                Select New Models.DockPTType With {.PTBitPos = t.TempTypeBitPos, .PTCaption = t.TempType, .PTOn = False}).ToArray()
                Return tmps

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAllTempTypes"), db)
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Deprecated by RHR for v-8.5.2.007 on 04/24/2023 
    ''' </summary>
    ''' <param name="LaneFeesControl"></param>
    ''' <returns></returns>
    Public Function GetTempTypebyFiltered(ByVal LaneFeesControl As Integer) As LTS.vTempType
        ' Deprecated by RHR for v-8.5.2.007 on 04/24/2023 
        ' Use GetTempTypebyFiltered365
        throwDepreciatedException("Deprecated for v-8.5.2.007 on 04/24/2023 -- vTempType is not valid ")
        Return Nothing
        'Dim oRet As LTS.vTempType 'return the table or view
        'Using db As New NGLMASLookupDataContext(ConnectionString)
        '    Try
        '        oRet = db.vTempTypes.Where(Function(x) x.UniqueID = LaneFeesControl).FirstOrDefault() 'added firstordefault to fix edit error Manorama' 16-Jun-2020
        '    Catch ex As Exception
        '        ManageLinqDataExceptions(ex, buildProcedureName("GetTempTypebyFiltered"), db)
        '    End Try
        '    Return oRet
        'End Using
    End Function

    ''' <summary>
    ''' Check if the ComCode is being used
    ''' </summary>
    ''' <param name="sCommCodeType"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.2.007 on o4/21/2023
    '''     renamed parameter because the old name did not match the data
    ''' </remarks>
    Public Function GetTempTypebyCommcode(ByVal sCommCodeType As String) As LTS.TempType

        Dim oRet As LTS.TempType 'return the table or view
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                oRet = db.TempTypes.Where(Function(x) x.CommCodeType = sCommCodeType).FirstOrDefault() 'added firstordefault to fix edit error Manorama' 16-Jun-2020
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetTempTypebyCommcode"), db)
            End Try
            Return oRet
        End Using
    End Function


    ''' <summary>
    ''' Deprecated by RHR for v-8.5.2.007 on 04/24/2023  Use GetTempTypeFilter365
    ''' </summary>
    ''' <param name="filters">filters</param>
    ''' <param name="RecordCount">No Of records</param>
    ''' <remarks>
    ''' Added By Suhas On 08-SEP-2020
    ''' Deprecated by RHR for v-8.5.2.007 on 04/24/2023 
    ''' </remarks>
    ''' <returns>LTS TempType</returns>
    Public Function GetTempTypeFilter(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vTempType()
        ' Deprecated by RHR for v-8.5.2.007 on 04/24/2023 
        ' Use GetTempTypeFilter365
        throwDepreciatedException("Deprecated for v-8.5.2.007 on 04/24/2023 -- vTempType is not valid ")
        Return Nothing

        'If filters Is Nothing Then Return Nothing
        'Dim itransControl As Integer = 0 'PK Control Number
        'Dim filterWhere As String = ""
        'Dim sFilterSpacer As String = ""

        'Dim oRet() As LTS.vTempType 'return the table or view
        'Using db As New NGLMASLookupDataContext(ConnectionString)
        '    Try
        '        If (filters.FilterValues Is Nothing OrElse filters.FilterValues.Count() < 1) OrElse (Not filters.FilterValues.Any(Function(x) x.filterName = "")) Then
        '            'The Record Control Filter does not exist so use the parent control fliter

        '        Else
        '            Dim tFilter As Models.FilterDetails = filters.FilterValues.Where(Function(x) x.filterName = "CommCodeType").FirstOrDefault()
        '            Integer.TryParse(tFilter.filterValueFrom, itransControl)
        '        End If

        '        Dim iQuery As IQueryable(Of LTS.vTempType)
        '        iQuery = db.vTempTypes
        '        If String.IsNullOrWhiteSpace(filters.sortName) Then
        '            filters.sortName = "CommCodeType"
        '            filters.sortDirection = "desc"
        '        End If
        '        ApplyAllFilters(iQuery, filters, filterWhere)
        '        PrepareQuery(iQuery, filters, RecordCount)
        '        db.Log = New DebugTextWriter
        '        oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
        '    Catch ex As Exception
        '        ManageLinqDataExceptions(ex, buildProcedureName("GetTempTypeFilter"), db)
        '    End Try
        '    Return oRet
        'End Using
    End Function

    ''' <summary>
    ''' Get a vTempType365 record by CommCodeType
    ''' </summary>
    ''' <param name="sCommCodeType"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.2.007 on 04/21/2023
    '''   fixed bug where the original view vTempType was not valid
    '''   and the LTS query PK was the wrong datafield
    ''' </remarks>
    Public Function GetTempTypebyFiltered(ByVal sCommCodeType As String) As LTS.vTempType365

        Dim oRet As LTS.vTempType365 'return the table or view
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                oRet = db.vTempType365s.Where(Function(x) x.CommCodeType = sCommCodeType).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetTempTypebyFiltered"), db)
            End Try
            Return oRet
        End Using
    End Function



    ''' <summary>
    ''' Method to Get Temp Type Records 
    ''' </summary>
    ''' <param name="filters">filters</param>
    ''' <param name="RecordCount">No Of records</param>
    ''' <remarks>
    ''' Added By Suhas On 08-SEP-2020
    ''' Modified by RHR for v-8.5.2.007 on 04/21/2023
    '''     fixed bug where view was missing
    '''     updated to use new view vTempType365
    ''' </remarks>
    ''' <returns>LTS TempType</returns>
    Public Function GetTempTypeFilter365(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vTempType365()
        If filters Is Nothing Then Return Nothing
        Dim itransControl As Integer = 0 'PK Control Number
        Dim filterWhere As String = ""
        Dim sFilterSpacer As String = ""

        Dim oRet() As LTS.vTempType365 'return the table or view
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                If (filters.FilterValues Is Nothing OrElse filters.FilterValues.Count() < 1) OrElse (Not filters.FilterValues.Any(Function(x) x.filterName = "")) Then
                    'The Record Control Filter does not exist so use the parent control fliter

                Else
                    Dim tFilter As Models.FilterDetails = filters.FilterValues.Where(Function(x) x.filterName = "CommCodeType").FirstOrDefault()
                    Integer.TryParse(tFilter.filterValueFrom, itransControl)
                End If

                Dim iQuery As IQueryable(Of LTS.vTempType365)
                iQuery = db.vTempType365s
                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "CommCodeType"
                    filters.sortDirection = "desc"
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetTempTypeFilter365"), db)
            End Try
            Return oRet
        End Using
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.2.007 on 04/24/2023
    '''     added new and update validation logic
    '''     CommCodeType must be unique for each ID 
    '''     
    ''' </remarks>
    Public Function SaveOrCreateTempType(ByVal oData As LTS.TempType) As Boolean
        Dim blnRet As Boolean = False
        Dim oExistingTempType As LTS.TempType
        If oData Is Nothing Then Return False 'nothing to do

        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'Dim iLaneTranID = oData.ID
                'If oData.LaneFeesLaneControl = 0 Then
                'If iLaneTranID = 0 Then
                '    Dim sMsg As String = "E_MissingParent" ' "  The reference to the parent record is missing. Please select a valid parent record and try again."
                '    throwNoDataFaultException(sMsg)
                'End If

                'End If

                '    Dim blnProcessed As Boolean = False
                'oData.LaneFeesModDate = Date.Now()
                'oData.LaneFeesModUser = Me.Parameters.UserName

                oExistingTempType = GetTempTypebyCommcode(oData.CommCodeType)
                Dim sID As String = ""
                If oExistingTempType Is Nothing Then
                    Dim iID As Integer = 0

                    If (Integer.TryParse(oData.ID, iID) = False) Then
                        sID = db.TempTypes.Max(Function(x) x.ID)
                        If (Integer.TryParse(sID, iID) = False) Then
                            iID = 0
                        End If
                        'oData.ID = iID.ToString()
                    Else
                        If iID = 0 Then
                            sID = db.TempTypes.Max(Function(x) x.ID)
                            If (Integer.TryParse(sID, iID) = False) Then
                                iID = 0
                            Else
                                iID += 1
                            End If
                            'oData.ID = iID.ToString()
                        End If
                    End If
                    sID = iID.ToString()
                    Do While (db.TempTypes.Any(Function(x) x.ID = sID))
                        iID += 1
                        sID = iID.ToString()
                    Loop
                    oData.ID = sID
                    If oData.TempTypeBitPos = 0 Then
                        oData.TempTypeBitPos = iID
                    End If
                    db.TempTypes.InsertOnSubmit(oData)
                Else
                    'we have a match for the current CommCodeType so check if the ID matches
                    If oData.ID <> oExistingTempType.ID Then
                        'the ID may have changed so verify that the new ID is not being used by another CommCodeType
                        If db.TempTypes.Any(Function(x) x.ID = oData.ID AndAlso x.CommCodeType <> oData.CommCodeType) Then
                            throwInvalidKeysAlreadyExistsException("Temp Type", "ID", oData.ID)
                        End If
                    End If
                    db.TempTypes.Attach(oData, True)
                End If
                db.SubmitChanges()

                blnRet = True
            Catch ex As FaultException
                Throw
                'Catch ex As System.Data.Linq.ChangeConflictException
                '    If ex.Message = "Row not found or changed." Then
                '        throwInvalidKeysAlreadyExistsException("Temp Type", "CommCodeType", oData.CommCodeType)
                '    Else
                '        ManageLinqDataExceptions(ex, buildProcedureName("SaveOrCreateTempType"), db)
                '    End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveOrCreateTempType"), db)
            End Try
        End Using
        Return blnRet
    End Function

    Public Function DeleteTempType(ByVal iLaneTranID As Integer, ByVal CommID As String) As Boolean
        Dim blnRet As Boolean = False
        Dim blnvalue As LTS.TempType
        If iLaneTranID = 0 Then Return False 'nothing to do
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'verify the service contract
                Dim oExisting = db.vTempTypes.Where(Function(x) x.UniqueID = iLaneTranID).FirstOrDefault()
                'If oExisting Is Nothing OrElse oExisting.UniqueID = 0 Then Return True
                'db.vTempTypes.DeleteOnSubmit(oExisting)
                Dim existingvalue = db.TempTypes.Where(Function(x) x.CommCodeType = CommID).FirstOrDefault()
                If existingvalue Is Nothing OrElse existingvalue.CommCodeType = "" Then Return True
                db.TempTypes.DeleteOnSubmit(existingvalue)
                db.SubmitChanges()

                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteTempType"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Delete the selected temp type record by CommCodeType
    ''' </summary>
    ''' <param name="sCommCodeType"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.2.007
    '''     Overload uses CommCodeType.
    '''     NOTE: 365 Rest Services may not support 
    '''         direct calling this method from API due to 
    '''         missing integer primary key in the table.
    ''' </remarks>
    Public Function DeleteTempType365(ByVal sCommCodeType As String) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'verify the service contract
                Dim oExisting As LTS.TempType = db.TempTypes.Where(Function(x) x.CommCodeType = sCommCodeType).FirstOrDefault()
                If oExisting Is Nothing OrElse String.IsNullOrWhiteSpace(oExisting.CommCodeType) Then Return True
                db.TempTypes.DeleteOnSubmit(oExisting)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteTempType"), db)
            End Try
        End Using
        Return blnRet
    End Function

#End Region

#Region "Protected Functions"

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by LVV 5/4/16 for v-7.0.5.1 DAT
    ''' Added DATEquipTypeControl
    ''' </remarks>
    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.TempType)
        'Create New Record
        Return New LTS.TempType With {.CommCodeType = d.CommCodeType _
                                              , .CommCodeDescription = d.CommCodeDescription _
                                              , .ID = d.ID _
                                              , .TempType = d.TempType _
                                              , .TariffTempType = d.TariffTempType _
                                              , .DATEquipTypeControl = d.DATEquipTypeControl _
                                              , .TempTypeUpdated = If(d.TempTypeUpdated Is Nothing, New Byte() {}, d.TempTypeUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetTempTypeFiltered(CommCodeType:=CType(LinqTable, LTS.TempType).CommCodeType)
    End Function

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DTO.TempType)
            Try
                Dim oExists As DTO.TempType = (
                    From t In CType(oDB, NGLMASLookupDataContext).TempTypes
                    Where
                    t.CommCodeType = .CommCodeType
                    Select New DTO.TempType With {.CommCodeType = t.CommCodeType}).First

                If Not oExists Is Nothing Then
                    Utilities.SaveAppError("Cannot save New TempType data.  The CommCodeType, " & .CommCodeType & ", already exist.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DTO.TempType)
            'First we need to determine if the CommCodeType exists (if not the user is trying to update the CommCodeType and this is not allowed).
            Try
                Dim oExists As DTO.TempType = (
                    From t In CType(oDB, NGLMASLookupDataContext).TempTypes
                    Where
                    t.CommCodeType = .CommCodeType
                    Select New DTO.TempType With {.CommCodeType = t.CommCodeType}).First

                If oExists Is Nothing Then
                    'Cannot update record because CommCodetype (PK) does not exist
                    Utilities.SaveAppError("Cannot save TempType changes.  The CommCodetype value, " & .CommCodeType & ", Is the primary key And could Not be found In the database.  Please use add To create a New record (updates To the CommCodeType are Not allowed).", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UpdateNotAllowed"}, New FaultReason("E_AccessDenied"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'Cannot update record because CommCodetype (PK) does not exist
                Utilities.SaveAppError("Cannot save TempType changes.  The CommCodetype value, " & .CommCodeType & ", Is the primary key And could Not be found In the database.  Please use add To create a New record (updates To the CommCodeType are Not allowed).", Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UpdateNotAllowed"}, New FaultReason("E_AccessDenied"))

            End Try


        End With
    End Sub

#End Region

End Class

Public Class NGLUOMData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Parameters = oParameters
        Dim db As New NGLMASLookupDataContext(ConnectionString)
        Me.LinqTable = db.UOMs
        Me.LinqDB = db
        Me.SourceClass = "NGLUOMData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLookupDataContext(ConnectionString)
            Me.LinqTable = db.UOMs
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
        Return Nothing
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetUOMsFiltered()
    End Function

    Public Function GetUOMFiltered(Optional ByVal UOMKey As String = "") As DTO.UOM
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim UOM As DTO.UOM = (
                From d In db.UOMs
                Where
                    (d.UOM.Contains(UOMKey))
                Select New DTO.UOM With {.UOMKey = d.UOM _
                                        , .UOMUpdated = d.UOMUpdated.ToArray()}).First


                Return UOM

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

    Public Function GetUOMsFiltered() As DTO.UOM()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'Return all the records that match the criteria
                Dim UOMs() As DTO.UOM = (
                From d In db.UOMs
                Order By d.UOM
                Select New DTO.UOM With {.UOMKey = d.UOM _
                                        , .UOMUpdated = d.UOMUpdated.ToArray()}).ToArray()
                Return UOMs

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

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.UOM)
        'Create New Record
        Return New LTS.UOM With {.UOM = d.UOMKey _
                                 , .UOMUpdated = If(d.UOMUpdated Is Nothing, New Byte() {}, d.UOMUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetUOMFiltered(UOMKey:=CType(LinqTable, LTS.UOM).UOM)
    End Function

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DTO.UOM)
            Try
                Dim oExists As DTO.UOM = (
                    From t In CType(oDB, NGLMASLookupDataContext).UOMs
                    Where
                    t.UOM = .UOMKey
                    Select New DTO.UOM With {.UOMKey = t.UOM}).First

                If Not oExists Is Nothing Then
                    Utilities.SaveAppError("Cannot save new UOM data.  The UOM value, " & .UOMKey & ",  already exist.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Updates to the UOM data are not allowed.
        Utilities.SaveAppError("Cannot save UOM changes.  Each UOM value is a unique key and updates are not allowed; only new records may be added.", Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UpdateNotAllowed"}, New FaultReason("E_AccessDenied"))
    End Sub

#End Region

End Class

Public Class NGLCodeData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLookupDataContext(ConnectionString)
        Me.LinqTable = db.Codes
        Me.LinqDB = db
        Me.SourceClass = "NGLCodeData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLookupDataContext(ConnectionString)
            Me.LinqTable = db.Codes
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
        Return Nothing
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    Public Function GetCodeFiltered(ByVal CodeKey As String, ByVal CodeType As String) As DTO.Code
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim Code As DTO.Code = (
                From d In db.Codes
                Where
                    (d.Code = CodeKey) _
                    And
                    (d.CodeType = CodeType)
                Select New DTO.Code With {.CodeKey = d.Code _
                                         , .CodeType = d.CodeType _
                                         , .CodeDescription = d.CodeDescription _
                                        , .CodeUpdated = d.CodeUpdated.ToArray()}).First

                Return Code

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

    Public Function GetCodeFiltered365(ByVal CodeKey As String, ByVal CodeType As String) As LTS.Code
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim Code As LTS.Code = (
                From d In db.Codes
                Where
                    (d.Code = CodeKey) _
                    And
                    (d.CodeType = CodeType)).FirstOrDefault()

                Return Code

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

    Public Function GetCodesFiltered(ByVal CodeKey As String, ByVal CodeType As String) As DTO.Code()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'Return all the records that match the criteria
                Dim Codes() As DTO.Code = (
                From d In db.Codes
                Where
                    (d.Code = CodeKey) _
                    And
                    (d.CodeType = CodeType)
                Order By d.Code
                Select New DTO.Code With {.CodeKey = d.Code _
                                         , .CodeType = d.CodeType _
                                         , .CodeDescription = d.CodeDescription _
                                        , .CodeUpdated = d.CodeUpdated.ToArray()}).ToArray()
                Return Codes

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

#End Region

#Region "LTS365 CRUD Methods"
    ''' <summary>
    ''' Method to List of Pay Codes. 
    ''' </summary>
    ''' <param name="filters">Page filters</param>
    ''' <param name="RecordCount">No Of Records</param>
    ''' <param name="CodeType">Type of Code(PAY)</param>
    ''' <remarks>
    ''' Added By ManoRama for Static Lookup List Maintenance On 08-SEP-2020
    ''' </remarks>
    ''' <returns>VLookupCode</returns>
    Public Function GetCodesFiltered(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer, ByVal CodeType As String) As LTS.vLookUpCode()
        If filters Is Nothing Then Return Nothing
        Dim filterWhere As String = ""
        Dim sFilterSpacer As String = ""
        Dim icodefilter As String = ""

        Dim oRet() As LTS.vLookUpCode 'return the table or view
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                If (filters.FilterValues Is Nothing OrElse filters.FilterValues.Count() < 1) OrElse (Not filters.FilterValues.Any(Function(x) x.filterName = "CodeType")) Then
                    'The Record Control Filter does not exist so use the parent control fliter

                Else
                    Dim tFilter As Models.FilterDetails = filters.FilterValues.Where(Function(x) x.filterName = "CodeType").FirstOrDefault()
                    Integer.TryParse(tFilter.filterValueFrom, icodefilter)
                End If

                Dim iQuery As IQueryable(Of LTS.vLookUpCode)
                iQuery = db.vLookUpCodes.Where(Function(x) x.CodeType = CodeType)

                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "CodeType"
                    filters.sortDirection = "desc"
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCodesFiltered"), db)
            End Try
            Return oRet
        End Using
    End Function
    ''' <summary>
    ''' Method to Get the List of Com Codes
    ''' </summary>
    ''' <param name="filters">Page Filters</param>
    ''' <param name="RecordCount">No Of Records</param>
    ''' <param name="CodeType">Type of Code(COM)</param>
    ''' <remarks>
    ''' Added By ManoRama for Static Lookup List Maintenance On 08-SEP-2020
    ''' </remarks>
    ''' <returns>VLookupComCode</returns>
    Public Function GetComCodesFiltered(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer, ByVal CodeType As String) As LTS.vLookUpComCode()
        If filters Is Nothing Then Return Nothing
        Dim filterWhere As String = ""
        Dim sFilterSpacer As String = ""
        Dim icodefilter As String = ""

        Dim oRet() As LTS.vLookUpComCode 'return the table or view
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                If (filters.FilterValues Is Nothing OrElse filters.FilterValues.Count() < 1) OrElse (Not filters.FilterValues.Any(Function(x) x.filterName = "CodeType")) Then
                    'The Record Control Filter does not exist so use the parent control fliter

                Else
                    Dim tFilter As Models.FilterDetails = filters.FilterValues.Where(Function(x) x.filterName = "CodeType").FirstOrDefault()
                    Integer.TryParse(tFilter.filterValueFrom, icodefilter)
                End If

                Dim iQuery As IQueryable(Of LTS.vLookUpComCode)
                iQuery = db.vLookUpComCodes.Where(Function(x) x.CodeType = CodeType)

                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "CodeType"
                    filters.sortDirection = "desc"
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCodesFiltered"), db)
            End Try
            Return oRet
        End Using
    End Function
    ''' <summary>
    ''' Method to Get the List of Tran Codes
    ''' </summary>
    ''' <param name="filters">Page Filters</param>
    ''' <param name="RecordCount">No Of Records</param>
    ''' <param name="CodeType">Type of Code(TRAN)</param>
    ''' <remarks>
    ''' Added By ManoRama for Static Lookup List Maintenance On 08-SEP-2020
    ''' </remarks>
    ''' <returns>VLookupTranCode </returns>
    Public Function GetTranCodesFiltered(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer, ByVal CodeType As String) As LTS.vLookupTranCode()
        If filters Is Nothing Then Return Nothing
        Dim filterWhere As String = ""
        Dim sFilterSpacer As String = ""
        Dim icodefilter As String = ""

        Dim oRet() As LTS.vLookupTranCode 'return the table or view
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                If (filters.FilterValues Is Nothing OrElse filters.FilterValues.Count() < 1) OrElse (Not filters.FilterValues.Any(Function(x) x.filterName = "CodeType")) Then
                    'The Record Control Filter does not exist so use the parent control fliter

                Else
                    Dim tFilter As Models.FilterDetails = filters.FilterValues.Where(Function(x) x.filterName = "CodeType").FirstOrDefault()
                    Integer.TryParse(tFilter.filterValueFrom, icodefilter)
                End If

                Dim iQuery As IQueryable(Of LTS.vLookupTranCode)
                iQuery = db.vLookupTranCodes.Where(Function(x) x.CodeType = CodeType)

                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "CodeType"
                    filters.sortDirection = "desc"
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCodesFiltered"), db)
            End Try
            Return oRet
        End Using
    End Function

    ''' <summary>
    ''' Method to Save or Insert Code Records in the Code Table.
    ''' </summary>
    ''' <param name="oData">Code LTS Object</param>
    ''' <param name="iCodeUpdate">boolean value weather it is update/Insert</param>
    ''' <remarks>
    ''' Added By ManoRama for Static Lookup List Maintenance On 10-SEP-2020
    ''' </remarks>
    ''' <returns>Array of spInsertOrUpdateCodesResult</returns>
    Public Function SaveOrCreateCode365(ByVal oData As LTS.Code, ByVal iCodeUpdate As Boolean) As LTS.spInsertOrUpdateCodesResult()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim CodeUpdated = False
                CodeUpdated = iCodeUpdate
                Dim res = db.spInsertOrUpdateCodes(oData.Code.Trim(), oData.CodeType, oData.CodeDescription.Trim(), CodeUpdated).ToArray()
                Return res
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveOrCreateCode365"), db)
            End Try
        End Using
        Return Nothing
    End Function
    ''' <summary>
    ''' Method to Delete the Code Table Record.
    ''' </summary>
    ''' <param name="rowguid">rowguid for the Code table.</param>
    ''' <remarks>
    ''' Added By ManoRama for Static Lookup List Maintenance On 10-SEP-2020
    ''' </remarks>
    ''' <returns>Array of spDeleteCodeResult</returns>
    Public Function DeleteCode365(ByVal rowguid As Guid) As LTS.spDeleteCodeResult()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'verify the service contract
                Dim oExisting = db.Codes.Where(Function(x) x.rowguid = rowguid).FirstOrDefault()
                If oExisting Is Nothing OrElse oExisting.rowguid = Guid.Empty Then Return Nothing
                Dim res = db.spDeleteCode(rowguid).ToArray()
                Return res
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteCode365"), db)
            End Try
        End Using
        Return Nothing
    End Function
#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.Code)
        'Create New Record
        Return New LTS.Code With {.Code = d.CodeKey _
                                         , .CodeType = d.CodeType _
                                         , .CodeDescription = d.CodeDescription _
                                 , .CodeUpdated = If(d.CodeUpdated Is Nothing, New Byte() {}, d.CodeUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCodeFiltered(CodeKey:=CType(LinqTable, LTS.Code).Code, CodeType:=CType(LinqTable, LTS.Code).CodeType)
    End Function

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DTO.Code)
            Try
                Dim oExists As DTO.Code = (
                    From t In CType(oDB, NGLMASLookupDataContext).Codes
                    Where
                    t.Code = .CodeKey _
                    And
                    t.CodeType = .CodeType
                    Select New DTO.Code With {.CodeKey = t.Code, .CodeType = t.CodeType}).First

                If Not oExists Is Nothing Then
                    Utilities.SaveAppError("Cannot save new Code data.  The Code value, " & .CodeKey & ",  already exist for Code Type " & .CodeType, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

#End Region

End Class

Public Class NGLSpecialCodeData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLookupDataContext(ConnectionString)
        Me.LinqTable = db.SpecialCodes
        Me.LinqDB = db
        Me.SourceClass = "NGLSpecialCodeData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLookupDataContext(ConnectionString)
            Me.LinqTable = db.SpecialCodes
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
        Return GetSpecialCodeFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    Public Function GetSpecialCodeFiltered(Optional ByVal Control As Integer = 0, Optional ByVal Code As String = "") As DTO.SpecialCode
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim SpecialCode As DTO.SpecialCode = (
                From d In db.SpecialCodes
                Where
                    (Control = 0 OrElse d.SpecialCodesControl = Control) _
                    And
                    (String.IsNullOrEmpty(Code) OrElse Code.Trim.Length < 1 OrElse d.Code = Code)
                Select New DTO.SpecialCode With {.SpecialCodesControl = d.SpecialCodesControl _
                                         , .Code = d.Code _
                                         , .Description = d.Description _
                                        , .SpecialCodesUpdated = d.SpecialCodesUpdated.ToArray()}).First

                Return SpecialCode

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

    Public Function GetSpecialCodesFiltered(Optional ByVal Control As Integer = 0, Optional ByVal Code As String = "") As DTO.SpecialCode()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'Return all the records that match the criteria
                Dim SpecialCodes() As DTO.SpecialCode = (
                From d In db.SpecialCodes
                Where
                    (Control = 0 OrElse d.SpecialCodesControl = Control) _
                    And
                    (String.IsNullOrEmpty(Code) OrElse Code.Trim.Length < 1 OrElse d.Code = Code)
                Order By d.Code
                Select New DTO.SpecialCode With {.SpecialCodesControl = d.SpecialCodesControl _
                                         , .Code = d.Code _
                                         , .Description = d.Description _
                                        , .SpecialCodesUpdated = d.SpecialCodesUpdated.ToArray()}).ToArray()
                Return SpecialCodes

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

    'Book Special Codes Methods
    Public Function GetSelectedBookEquipCodes(ByVal BookControl As Integer) As DTO.vLookupList()
        Dim vList() As DTO.vLookupList
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                vList = (
                    From t In db.spGetSelectedBookEquipCodes(BookControl)
                    Order By t.Code
                    Select New DTO.vLookupList _
                    With {.Control = t.Control, .Name = t.Code, .Description = t.Description}).ToArray()

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
        Return vList
    End Function

    Public Function GetAvailBookEquipCodes(ByVal BookControl As Integer) As DTO.vLookupList()
        Dim vList() As DTO.vLookupList
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                vList = (
                    From t In db.spGetAvailBookEquipCodes(BookControl)
                    Order By t.Code
                    Select New DTO.vLookupList _
                    With {.Control = t.Control, .Name = t.Code, .Description = t.Description}).ToArray()

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
        Return vList
    End Function

    Public Sub InsertBookEquipCode(ByVal BookControl As Integer, ByVal BookCodesControl As Integer)
        Dim strProcName As String = "dbo.spInsertBookEqupCode"
        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@BookControl", BookControl)
        oCmd.Parameters.AddWithValue("@BookCodesControl ", BookCodesControl)
        oCmd.Parameters.AddWithValue("@UserName", Me.Parameters.UserName)
        runNGLStoredProcedure(oCmd, strProcName, 2)
    End Sub

    Public Sub DeleteBookEquipCode(ByVal BookControl As Integer, ByVal BookCodesControl As Integer)
        Dim strProcName As String = "dbo.spDeleteBookEqupCode"
        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@BookControl", BookControl)
        oCmd.Parameters.AddWithValue("@BookCodesControl ", BookCodesControl)
        oCmd.Parameters.AddWithValue("@UserName", Me.Parameters.UserName)
        runNGLStoredProcedure(oCmd, strProcName, 2)
    End Sub

    'Lane Special Codes Methods
    Public Function GetSelectedLaneEquipCodes(ByVal LaneControl As Integer) As DTO.vLookupList()
        Dim vList() As DTO.vLookupList
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                vList = (
                    From t In db.spGetSelectedLaneEquipCodes(LaneControl)
                    Order By t.Code
                    Select New DTO.vLookupList _
                    With {.Control = t.Control, .Name = t.Code, .Description = t.Description}).ToArray()

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
        Return vList
    End Function

    Public Function GetAvailLaneEquipCodes(ByVal LaneControl As Integer) As DTO.vLookupList()
        Dim vList() As DTO.vLookupList
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                vList = (
                    From t In db.spGetAvailLaneEquipCodes(LaneControl)
                    Order By t.Code
                    Select New DTO.vLookupList _
                    With {.Control = t.Control, .Name = t.Code, .Description = t.Description}).ToArray()

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
        Return vList
    End Function

    Public Sub InsertLaneEquipCode(ByVal LaneControl As Integer, ByVal StopCodesSpecialCodesControl As Integer)
        Dim strProcName As String = "dbo.spInsertLaneEqupCode"
        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@LaneControl", LaneControl)
        oCmd.Parameters.AddWithValue("@StopCodesSpecialCodesControl ", StopCodesSpecialCodesControl)
        oCmd.Parameters.AddWithValue("@UserName", Me.Parameters.UserName)
        runNGLStoredProcedure(oCmd, strProcName, 2)
    End Sub

    Public Sub DeleteLaneEquipCode(ByVal LaneControl As Integer, ByVal StopCodesSpecialCodesControl As Integer)
        Dim strProcName As String = "dbo.spDeleteLaneEqupCode"
        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@LaneControl", LaneControl)
        oCmd.Parameters.AddWithValue("@StopCodesSpecialCodesControl ", StopCodesSpecialCodesControl)
        oCmd.Parameters.AddWithValue("@UserName", Me.Parameters.UserName)
        runNGLStoredProcedure(oCmd, strProcName, 2)
    End Sub

    'MasterTruck Special Codes Methods
    Public Function GetSelectedMasterTruckEquipCodes(ByVal MasterTruckCodesCarrierTruckControl As Integer) As DTO.vLookupList()
        Dim vList() As DTO.vLookupList
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                vList = (
                    From t In db.spGetSelectedMasterTruckEquipCodes(MasterTruckCodesCarrierTruckControl)
                    Order By t.Code
                    Select New DTO.vLookupList _
                    With {.Control = t.Control, .Name = t.Code, .Description = t.Description}).ToArray()

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
        Return vList
    End Function

    Public Function GetAvailMasterTruckEquipCodes(ByVal MasterTruckCodesCarrierTruckControl As Integer) As DTO.vLookupList()
        Dim vList() As DTO.vLookupList
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                vList = (
                    From t In db.spGetAvailMasterTruckEquipCodes(MasterTruckCodesCarrierTruckControl)
                    Order By t.Code
                    Select New DTO.vLookupList _
                    With {.Control = t.Control, .Name = t.Code, .Description = t.Description}).ToArray()

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
        Return vList
    End Function

    Public Sub InsertMasterTruckEquipCode(ByVal MasterTruckCodesCarrierTruckControl As Integer, ByVal MasterTruckCodesSpecialCodesControl As Integer)
        Dim strProcName As String = "dbo.spInsertMasterTruckEqupCode"
        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@MasterTruckCodesCarrierTruckControl", MasterTruckCodesCarrierTruckControl)
        oCmd.Parameters.AddWithValue("@MasterTruckCodesSpecialCodesControl", MasterTruckCodesSpecialCodesControl)
        oCmd.Parameters.AddWithValue("@UserName", Me.Parameters.UserName)
        runNGLStoredProcedure(oCmd, strProcName, 2)
    End Sub

    Public Sub DeleteMasterTruckEquipCode(ByVal MasterTruckCodesCarrierTruckControl As Integer, ByVal MasterTruckCodesControl As Integer)
        Dim strProcName As String = "dbo.spDeleteMasterTruckEqupCode"
        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@MasterTruckCodesCarrierTruckControl", MasterTruckCodesCarrierTruckControl)
        oCmd.Parameters.AddWithValue("@MasterTruckCodesControl", MasterTruckCodesControl)
        oCmd.Parameters.AddWithValue("@UserName", Me.Parameters.UserName)
        runNGLStoredProcedure(oCmd, strProcName, 2)
    End Sub

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.SpecialCode)
        'Create New Record
        Return New LTS.SpecialCode With {.SpecialCodesControl = d.SpecialCodesControl _
                                         , .Code = d.Code _
                                         , .Description = d.Description _
                                 , .SpecialCodesUpdated = If(d.SpecialCodesUpdated Is Nothing, New Byte() {}, d.SpecialCodesUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetSpecialCodeFiltered(Control:=CType(LinqTable, LTS.SpecialCode).SpecialCodesControl, Code:=CType(LinqTable, LTS.SpecialCode).Code)
    End Function

    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DTO.SpecialCode)
            Try
                Dim oExists As DTO.SpecialCode = (
                    From t In CType(oDB, NGLMASLookupDataContext).SpecialCodes
                    Where
                    t.SpecialCodesControl <> .SpecialCodesControl _
                    And
                    t.Code = .Code
                    Select New DTO.SpecialCode With {.SpecialCodesControl = t.SpecialCodesControl, .Code = t.Code}).First

                If Not oExists Is Nothing Then
                    Utilities.SaveAppError("Cannot save new SpecialCode data.  The SpecialCode value, " & .Code & ",  already exist.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DTO.SpecialCode)
            Try
                Dim oExists As DTO.SpecialCode = (
                    From t In CType(oDB, NGLMASLookupDataContext).SpecialCodes
                    Where
                    t.Code = .Code
                    Select New DTO.SpecialCode With {.SpecialCodesControl = t.SpecialCodesControl, .Code = t.Code}).First

                If Not oExists Is Nothing Then
                    Utilities.SaveAppError("Cannot save new SpecialCode data.  The SpecialCode value, " & .Code & ",  already exist.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

#End Region

End Class

Public Class NGLAlphaCompanyXrefData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLookupDataContext(ConnectionString)
        Me.LinqTable = db.AlphaCompanyXrefs
        Me.LinqDB = db
        Me.SourceClass = "NGLAlphaCompanyXrefData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLookupDataContext(ConnectionString)
            Me.LinqTable = db.AlphaCompanyXrefs
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
        Return GetAlphaCompanyXrefFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetAlphaCompanyXrefs()
    End Function

    Public Function GetAlphaCompanyXrefFiltered(ByVal ACXControl As Integer) As DTO.AlphaCompanyXref
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim AlphaCompanyXref As DTO.AlphaCompanyXref = (
                From d In db.AlphaCompanyXrefs
                Where
                    (d.ACXControl = ACXControl)
                Select New DTO.AlphaCompanyXref With {.ACXControl = d.ACXControl _
                                         , .ACXCompNumber = d.ACXCompNumber _
                                         , .ACXAlphaNumber = d.ACXAlphaNumber _
                                        , .ACXUpdated = d.ACXUpdated.ToArray()}).First

                Return AlphaCompanyXref

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

    Public Function GetAlphaCompanyXrefs() As DTO.AlphaCompanyXref()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'Return all the records that match the criteria
                Dim AlphaCompanyXrefs() As DTO.AlphaCompanyXref = (
                From d In db.AlphaCompanyXrefs
                Order By d.ACXCompNumber
                Select New DTO.AlphaCompanyXref With {.ACXControl = d.ACXControl _
                                         , .ACXCompNumber = d.ACXCompNumber _
                                         , .ACXAlphaNumber = d.ACXAlphaNumber _
                                        , .ACXUpdated = d.ACXUpdated.ToArray()}).ToArray()
                Return AlphaCompanyXrefs

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


#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.AlphaCompanyXref)
        'Create New Record
        Return New LTS.AlphaCompanyXref With {.ACXControl = d.ACXControl _
                                         , .ACXCompNumber = d.ACXCompNumber _
                                         , .ACXAlphaNumber = d.ACXAlphaNumber _
                                        , .ACXUpdated = If(d.ACXUpdated Is Nothing, New Byte() {}, d.ACXUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetAlphaCompanyXrefFiltered(ACXControl:=CType(LinqTable, LTS.AlphaCompanyXref).ACXControl)
    End Function

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DTO.AlphaCompanyXref)
            Try
                Dim oExists As DTO.AlphaCompanyXref = (
                    From t In CType(oDB, NGLMASLookupDataContext).AlphaCompanyXrefs
                    Where
                    t.ACXCompNumber = .ACXCompNumber _
                    Or
                    t.ACXAlphaNumber = .ACXAlphaNumber
                    Select New DTO.AlphaCompanyXref With {.ACXControl = t.ACXControl}).First

                If Not oExists Is Nothing Then
                    Utilities.SaveAppError("Cannot save new AlphaCompanyXref data.  The AlphaCompanyXref Company Number, " & .ACXCompNumber & ", or Alpha Number, " & .ACXAlphaNumber & ", already exist!", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    Protected Overrides Sub ValidateupdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DTO.AlphaCompanyXref)
            Try
                Dim oExists As DTO.AlphaCompanyXref = (
                    From t In CType(oDB, NGLMASLookupDataContext).AlphaCompanyXrefs
                    Where
                    t.ACXCompNumber = .ACXCompNumber _
                    Or
                    t.ACXAlphaNumber = .ACXAlphaNumber _
                    And
                    t.ACXControl <> .ACXControl
                    Select New DTO.AlphaCompanyXref With {.ACXControl = t.ACXControl}).First

                If Not oExists Is Nothing Then
                    Utilities.SaveAppError("Cannot save updated AlphaCompanyXref data.  The AlphaCompanyXref Company Number, " & .ACXCompNumber & ", or Alpha Number, " & .ACXAlphaNumber & ", already exist!", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

#End Region

End Class

Public Class NGLNatFuelZoneData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        'processParameters(oParameters)
        Parameters = oParameters
        Dim db As New NGLMASLookupDataContext(ConnectionString)
        Me.LinqTable = db.NatFuelZones
        Me.LinqDB = db
        Me.SourceClass = "NGLNatFuelZoneData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLookupDataContext(ConnectionString)
            Me.LinqTable = db.NatFuelZones
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
        Return GetNatFuelZoneFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetNatFuelZones()
    End Function

    Public Function GetNatFuelZoneFiltered(ByVal ID As Integer) As DTO.NatFuelZone
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim zone As DTO.NatFuelZone = (
                From d In db.NatFuelZones
                Where
                    (d.NatFuelZoneID = ID)
                Select New DTO.NatFuelZone With {.NatFuelZoneID = d.NatFuelZoneID _
                                         , .NatFuelZoneDesc = d.NatFuelZoneDesc _
                                         , .NatFuelZoneIndex = d.NatFuelZoneIndex _
                                        , .NatFuelZoneModDate = d.NatFuelZoneModDate _
                                          , .NatFuelZoneModUser = d.NatFuelZoneModUser _
                                         , .NatFuelZoneName = d.NatFuelZoneName _
                                        , .NatFuelZoneUpdated = d.NatFuelZoneUpdated.ToArray()}).First

                Return zone

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

    Public Function GetNatFuelZones() As DTO.NatFuelZone()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'Return all the records that match the criteria
                Dim zoness() As DTO.NatFuelZone = (
                From d In db.NatFuelZones
                Order By d.NatFuelZoneID
                Select New DTO.NatFuelZone With {.NatFuelZoneID = d.NatFuelZoneID _
                                         , .NatFuelZoneDesc = d.NatFuelZoneDesc _
                                         , .NatFuelZoneIndex = d.NatFuelZoneIndex _
                                                , .NatFuelZoneModDate = d.NatFuelZoneModDate _
                                                , .NatFuelZoneModUser = d.NatFuelZoneModUser _
                                                , .NatFuelZoneName = d.NatFuelZoneName _
                                        , .NatFuelZoneUpdated = d.NatFuelZoneUpdated.ToArray()}).ToArray()
                Return zoness

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


#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.NatFuelZone)
        'Create New Record   
        Return New LTS.NatFuelZone With {.NatFuelZoneID = d.NatFuelZoneID _
                                         , .NatFuelZoneIndex = d.NatFuelZoneIndex _
                                        , .NatFuelZoneDesc = d.NatFuelZoneDesc _
                                        , .NatFuelZoneName = d.NatFuelZoneName _
                                        , .NatFuelZoneModDate = Date.Now _
                                         , .NatFuelZoneModUser = Parameters.UserName _
                                        , .NatFuelZoneUpdated = If(d.NatFuelZoneUpdated Is Nothing, New Byte() {}, d.NatFuelZoneUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetNatFuelZoneFiltered(ID:=CType(LinqTable, LTS.NatFuelZone).NatFuelZoneID)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim source As LTS.NatFuelZone = TryCast(LinqTable, LTS.NatFuelZone)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.NatFuelZones
                       Where d.NatFuelZoneID = source.NatFuelZoneID
                       Select New DTO.QuickSaveResults With {.Control = d.NatFuelZoneID _
                                                            , .ModDate = d.NatFuelZoneModDate _
                                                            , .ModUser = d.NatFuelZoneModUser _
                                                            , .Updated = d.NatFuelZoneUpdated.ToArray}).First

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

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'no validation

        ''Check if the data already exists only one allowed
        'With CType(oData, DTO.NatFuelZone)
        '    Try
        '        Dim oExists As DTO.NatFuelZone = ( _
        '            From t In CType(oDB, NGLMASLookupDataContext).NatFuelZones _
        '             Where _
        '             t.NatFuelZoneID = .NatFuelZoneID _
        '             Or _
        '             t.NatFuelZoneIndex = .NatFuelZoneIndex _
        '             Select New DTO.NatFuelZone With {.NatFuelZoneID = t.NatFuelZoneID}).First

        '        If Not oExists Is Nothing Then
        '            Utilities.SaveAppError("Cannot save new NatFuelZone data.  The NatFuelZone ID, " & .NatFuelZoneID & ", or Index, " & .NatFuelZoneIndex & ", already exist!", Me.Parameters)
        '            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
        '        End If

        '    Catch ex As FaultException
        '        Throw
        '    Catch ex As InvalidOperationException
        '        'do nothing this is the desired result.
        '    End Try
        'End With
    End Sub

    Protected Overrides Sub ValidateupdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'no validation
        ''Check if the data already exists only one allowed
        'With CType(oData, DTO.AlphaCompanyXref)
        '    Try
        '        Dim oExists As DTO.AlphaCompanyXref = ( _
        '            From t In CType(oDB, NGLMASLookupDataContext).AlphaCompanyXrefs _
        '             Where _
        '             t.ACXCompNumber = .ACXCompNumber _
        '             Or _
        '             t.ACXAlphaNumber = .ACXAlphaNumber _
        '             And _
        '             t.ACXControl <> .ACXControl _
        '             Select New DTO.AlphaCompanyXref With {.ACXControl = t.ACXControl}).First

        '        If Not oExists Is Nothing Then
        '            Utilities.SaveAppError("Cannot save updated AlphaCompanyXref data.  The AlphaCompanyXref Company Number, " & .ACXCompNumber & ", or Alpha Number, " & .ACXAlphaNumber & ", already exist!", Me.Parameters)
        '            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
        '        End If

        '    Catch ex As FaultException
        '        Throw
        '    Catch ex As InvalidOperationException
        '        'do nothing this is the desired result.
        '    End Try
        'End With
    End Sub

#End Region

End Class

Public Class NGLNatFuelZoneStateData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLookupDataContext(ConnectionString)
        Me.LinqTable = db.NatFuelZoneStates
        Me.LinqDB = db
        Me.SourceClass = "NGLNatFuelZoneStateData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLookupDataContext(ConnectionString)
            Me.LinqTable = db.NatFuelZoneStates
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
        Return GetNatFuelZoneStateFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetNatFuelZoneStates()
    End Function

    Public Function GetNatFuelZoneStateFiltered(ByVal zoneID As Integer) As DTO.NatFuelZoneState
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim zoneState As DTO.NatFuelZoneState = (
                From d In db.NatFuelZoneStates
                Where
                    (d.NatFuelZoneStatesNatFuelZoneID = zoneID)
                Select New DTO.NatFuelZoneState With {.NatFuelZoneStatesNatFuelZoneID = d.NatFuelZoneStatesNatFuelZoneID _
                                         , .NatFuelZoneStatesState = d.NatFuelZoneStatesState _
                                         , .NatFuelZoneStatesModDate = d.NatFuelZoneStatesModDate _
                                        , .NatFuelZoneStatesModUser = d.NatFuelZoneStatesModUser _
                                        , .NatFuelZoneStatesUpdated = d.NatFuelZoneStatesUpdated.ToArray()}).First

                Return zoneState

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

    Public Function GetNatFuelZoneStates(ByVal zoneID As Integer) As DTO.NatFuelZoneState()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'Return all the records that match the criteria
                Dim zonesStates() As DTO.NatFuelZoneState = (
                From d In db.NatFuelZoneStates
                Where d.NatFuelZoneStatesNatFuelZoneID = zoneID
                Order By d.NatFuelZoneStatesState
                Select New DTO.NatFuelZoneState With {.NatFuelZoneStatesState = d.NatFuelZoneStatesState _
                                         , .NatFuelZoneStatesNatFuelZoneID = d.NatFuelZoneStatesNatFuelZoneID _
                                         , .NatFuelZoneStatesModDate = d.NatFuelZoneStatesModDate _
                                                , .NatFuelZoneStatesModUser = d.NatFuelZoneStatesModUser _
                                        , .NatFuelZoneStatesUpdated = d.NatFuelZoneStatesUpdated.ToArray()}).ToArray()
                Return zonesStates

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

    Public Function GetNatFuelZoneStates() As DTO.NatFuelZoneState()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'Return all the records that match the criteria
                Dim zonesStates() As DTO.NatFuelZoneState = (
                From d In db.NatFuelZoneStates
                Order By d.NatFuelZoneStatesState
                Select New DTO.NatFuelZoneState With {.NatFuelZoneStatesState = d.NatFuelZoneStatesState _
                                         , .NatFuelZoneStatesNatFuelZoneID = d.NatFuelZoneStatesNatFuelZoneID _
                                         , .NatFuelZoneStatesModDate = d.NatFuelZoneStatesModDate _
                                                , .NatFuelZoneStatesModUser = d.NatFuelZoneStatesModUser _
                                        , .NatFuelZoneStatesUpdated = d.NatFuelZoneStatesUpdated.ToArray()}).ToArray()
                Return zonesStates

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
    ''' Method to List of NatfuelZone States
    ''' </summary>
    ''' <param name="filters">Filter Settings</param>
    ''' <param name="RecordCount">No Of Records</param>
    ''' <remarks>
    ''' Added by ManoRama On 27-AUG-2020,This is to list out natfuelzonestates using view vNatFuelZoneStates
    ''' </remarks>
    ''' <returns>View vNatFuelZoneState</returns>
    Public Function GetNatFuelZoneStates365(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vNatFuelZoneState()
        If filters Is Nothing Then Return Nothing
        Dim filterWhere As String = ""
        Dim sFilterSpacer As String = ""

        Dim oRet() As LTS.vNatFuelZoneState 'return the table or view
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                If (filters.FilterValues Is Nothing OrElse filters.FilterValues.Count() < 1) OrElse (Not filters.FilterValues.Any(Function(x) x.filterName = "LaneFeesControl")) Then
                    'The Record Control Filter does not exist so use the parent control fliter

                Else
                    ''   Dim tFilter As Models.FilterDetails = filters.FilterValues.Where(Function(x) x.filterName = "LaneFeesControl").FirstOrDefault()
                    ''  Integer.TryParse(tFilter.filterValueFrom, iLaneFeesControl)
                End If

                Dim iQuery As IQueryable(Of LTS.vNatFuelZoneState)
                iQuery = db.vNatFuelZoneStates
                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "NatFuelZoneModDate"
                    filters.sortDirection = "DESC"
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetNatFuelZoneStates365"), db)
            End Try
            Return oRet
        End Using
    End Function
    ''' <summary>
    ''' Method to Insert the record in NatFuelZoneState table
    ''' </summary>
    ''' <param name="zone">LTS.NatFuelZoneState</param>
    ''' <remarks>
    ''' Added by ManoRama On 27-AUG-2020 ,to map zone to state in NatFuelZoneState using SP:SPInsertNatFuelZoneStates
    ''' </remarks>
    ''' <returns></returns>
    Public Function ValidateupdatedRecord365(ByVal zone As LTS.NatFuelZoneState) As Boolean
        Dim blnRet As Boolean = False
        If zone Is Nothing Then Return False 'nothing to do
        Dim blnNewCarrier As Boolean = False
        Dim oNew As New LTS.NatFuelZoneState()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'verify the Carrier contract


                If zone.NatFuelZoneStatesNatFuelZoneID Then
                    'If db.vNatFuelZoneStates.Any(Function(x) x.NatFuelZoneID = carrier.NatFuelZoneStatesNatFuelZoneID) Then
                    '    Dim sTariffName As String = db.NatFuelZoneStates.AsEnumerable().Where(Function(x) x.NatFuelZoneStatesNatFuelZoneID = carrier.NatFuelZoneStatesNatFuelZoneID & x.NatFuelZoneStatesState = carrier.NatFuelZoneStatesState).Select(Function(x) x.NatFuelZoneStatesState).FirstOrDefault()
                    '    'Cannot save changes to {0}.  Only one {1} is allowed for each {0}; a {1} of {2} is already configured for this {0}.
                    '    throwInvalidKeyAlreadyExistsException("State Already Exists", "State", sTariffName)
                    'End If

                    Dim strMSG As String = ""
                    Dim strBatchName As String = "ValidateupdatedRecord365"
                    Dim strProcName As String = "dbo.SPInsertNatFuelZoneStates"
                    '  Dim blnRet As Boolean = False

                    Dim oCmd As New System.Data.SqlClient.SqlCommand
                    oCmd.Parameters.AddWithValue("@StateID", Convert.ToInt32(zone.NatFuelZoneStatesState))
                    oCmd.Parameters.AddWithValue("@ZoneID", zone.NatFuelZoneStatesNatFuelZoneID)
                    oCmd.Parameters.AddWithValue("@ModUser", Left(Me.Parameters.UserName, 200))
                    runNGLStoredProcedure(oCmd, strProcName, 0)
                    blnRet = True
                    Return blnRet
                    If Debugger.IsAttached AndAlso Not String.IsNullOrWhiteSpace(strMSG) Then
                        System.Diagnostics.Debug.WriteLine(strMSG)
                    End If
                    ''db.NatFuelZoneStates.InsertOnSubmit(oNew)
                    blnNewCarrier = True

                End If
                '' db.SubmitChanges()
                ''blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("ValidateupdatedRecord365"), db)
            End Try
        End Using

        Return blnRet
    End Function
    ''' <summary>
    ''' Method to Delete the record in NatFuelZoneStates table.
    ''' </summary>
    ''' <param name="iNatStateControl">NatFuelZoneStatesState(pk)</param>
    ''' <remarks>
    ''' Added by ManoRama On 27-AUG-2020 ,for Natfuel Delete changes.
    ''' </remarks>
    ''' <returns>True/False</returns>
    Public Function DeleteNatFuelZone365(ByVal iNatStateControl As String) As Boolean
        Dim blnRet As Boolean = False

        If String.IsNullOrEmpty(iNatStateControl) Then Return False 'nothing to do
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'verify the service contract
                Dim oExisting = db.NatFuelZoneStates.AsEnumerable().Where(Function(x) x.NatFuelZoneStatesState = iNatStateControl).FirstOrDefault()
                If oExisting Is Nothing OrElse String.IsNullOrEmpty(oExisting.NatFuelZoneStatesState) Then Return True
                db.NatFuelZoneStates.DeleteOnSubmit(oExisting)
                db.SubmitChanges()

                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteNatFuelZone365"), db)
            End Try
        End Using
        Return blnRet
    End Function
#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.NatFuelZoneState)
        'Create New Record   
        Return New LTS.NatFuelZoneState With {.NatFuelZoneStatesState = d.NatFuelZoneStatesState _
                                         , .NatFuelZoneStatesNatFuelZoneID = d.NatFuelZoneStatesNatFuelZoneID _
                                        , .NatFuelZoneStatesModDate = Date.Now _
                                        , .NatFuelZoneStatesModUser = Parameters.UserName _
                                        , .NatFuelZoneStatesUpdated = If(d.NatFuelZoneStatesUpdated Is Nothing, New Byte() {}, d.NatFuelZoneStatesUpdated)}

    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetNatFuelZoneStateFiltered(zoneID:=CType(LinqTable, LTS.NatFuelZoneState).NatFuelZoneStatesNatFuelZoneID)
    End Function

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)

        'Check if the data already exists only one allowed
        With CType(oData, DTO.NatFuelZoneState)
            Try
                Dim oExists As DTO.NatFuelZoneState = (
                    From t In CType(oDB, NGLMASLookupDataContext).NatFuelZoneStates
                    Where
                    t.NatFuelZoneStatesState = .NatFuelZoneStatesState
                    Select New DTO.NatFuelZoneState With {.NatFuelZoneStatesState = t.NatFuelZoneStatesState}).First

                If Not oExists Is Nothing Then
                    Utilities.SaveAppError("Cannot save new NatFuelZoneState data.  The NatFuelZoneState, " & .NatFuelZoneStatesState & ", already exist!", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    Protected Overrides Sub ValidateupdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DTO.NatFuelZoneState)
            Try
                Dim oExists As DTO.NatFuelZoneState = (
                    From t In CType(oDB, NGLMASLookupDataContext).NatFuelZoneStates
                    Where
                    t.NatFuelZoneStatesState = .NatFuelZoneStatesState _
                    And
                    t.NatFuelZoneStatesNatFuelZoneID <> .NatFuelZoneStatesNatFuelZoneID
                    Select New DTO.NatFuelZoneState With {.NatFuelZoneStatesState = t.NatFuelZoneStatesState}).First

                If Not oExists Is Nothing Then
                    Utilities.SaveAppError("Cannot save updated NatFuelZoneStatesState data.  The NatFuelZoneStates State, " & .NatFuelZoneStatesState & ", already exist!", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub



    'Public Function ValidateupdatedRecord365(ByVal carrier As LTS.NatFuelZoneState) As Boolean
    '    Dim blnRet As Boolean = False
    '    If carrier Is Nothing Then Return False 'nothing to do
    '    Dim blnNewCarrier As Boolean = False
    '    Dim oNew As New LTS.NatFuelZoneState()
    '    Using db As New NGLMASLookupDataContext(ConnectionString)
    '        Try
    '            'verify the Carrier contract


    '            If carrier. = 0 Then
    '                If db.CarrierEDIs.Any(Function(x) x.CarrierEDICarrierControl = carrier.CarrierEDICarrierControl) Then
    '                    Dim sTariffName As String = db.CarrierEDIs.Where(Function(x) x.CarrierEDICarrierControl = carrier.CarrierEDICarrierControl).Select(Function(x) x.CarrierEDIXaction).FirstOrDefault()
    '                    'Cannot save changes to {0}.  Only one {1} is allowed for each {0}; a {1} of {2} is already configured for this {0}.
    '                    throwInvalidKeyAlreadyExistsException("Carrier Tariff Carrier", "Contract Name", sTariffName)
    '                End If

    '                Dim strMSG As String = ""
    '                Dim skipObjs As New List(Of String) From {"CarrierEDICarrierControl", "rowguid", "CarrierEDIModDate", "CarrierEDIModUser", "CarrierEDIUpdated", "Carrier"}
    '                oNew = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(oNew, carrier, skipObjs, strMSG)
    '                With oNew
    '                    .CarrierEDIModDate = Date.Now
    '                    .CarrierEDIModUser = Me.Parameters.UserName
    '                    .CarrierEDICarrierControl = carrier.CarrierEDICarrierControl
    '                End With
    '                If Debugger.IsAttached AndAlso Not String.IsNullOrWhiteSpace(strMSG) Then
    '                    System.Diagnostics.Debug.WriteLine(strMSG)
    '                End If
    '                db.CarrierEDIs.InsertOnSubmit(oNew)
    '                blnNewCarrier = True
    '            Else
    '                Dim oExisting = db.CarrierEDIs.Where(Function(x) x.CarrierEDICarrierControl = carrier.CarrierEDICarrierControl).FirstOrDefault()
    '                If oExisting Is Nothing OrElse oExisting.CarrierEDICarrierControl = 0 Then
    '                    throwRecordDeletedFaultException("Cannot save changes for CarrierEDI: " & carrier.CarrierEDIXaction)
    '                End If
    '                Dim strMSG As String = ""
    '                Dim skipObjs As New List(Of String) From {"CarrierEDIModUser", "CarrierEDIModDate", "Carrier", "CarrierEDIUpdated"}
    '                oExisting = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(oExisting, carrier, skipObjs, strMSG)
    '                With oExisting
    '                    .CarrierEDIModDate = Date.Now
    '                    .CarrierEDIModUser = Me.Parameters.UserName
    '                    .CarrierEDIUpdated = carrier.CarrierEDIUpdated
    '                End With
    '            End If
    '            db.SubmitChanges()
    '            blnRet = True
    '        Catch ex As FaultException
    '            Throw
    '        Catch ex As Exception
    '            ManageLinqDataExceptions(ex, buildProcedureName("SaveCarrierEDI"), db)
    '        End Try
    '    End Using

    '    Return blnRet
    'End Function
#End Region

End Class

Public Class NGLtblAccessorialVariableCodeData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMASLookupDataContext(ConnectionString)
        Me.LinqTable = db.tblAccessorialVariableCodes
        Me.LinqDB = db
        Me.SourceClass = "NGLtblAccessorialVariableCodeData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLookupDataContext(ConnectionString)
            _LinqTable = db.tblAccessorialVariableCodes
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
        Return GettblAccessorialVariableCodeFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GettblAccessorialVariableCodesFiltered()
    End Function

    Public Function GettblAccessorialVariableCodeFiltered(Optional ByVal Control As Integer = 0, Optional ByVal AccessorialVariableCodesName As String = "") As DTO.tblAccessorialVariableCode
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim tblAccessorialVariableCode As DTO.tblAccessorialVariableCode = (
                From d In db.tblAccessorialVariableCodes
                Where
                    (Control = 0 OrElse d.AccessorialVariableCodesControl = Control) _
                     And
                     (AccessorialVariableCodesName Is Nothing OrElse String.IsNullOrEmpty(AccessorialVariableCodesName) OrElse d.AccessorialVariableCodesName = AccessorialVariableCodesName)
                Select selectDTOData(d)).First


                Return tblAccessorialVariableCode

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GettblAccessorialVariableCodeFiltered"), sysErrorParameters.sysErrorState.ServerLevelFault, sysErrorParameters.sysErrorSeverity.Critical)
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GettblAccessorialVariableCodesFiltered() As DTO.tblAccessorialVariableCode()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'Return all records sorted by name
                Dim tblAccessorialVariableCodes() As DTO.tblAccessorialVariableCode = (
                From d In db.tblAccessorialVariableCodes
                Order By d.AccessorialVariableCodesName
                Select selectDTOData(d)).ToArray()
                Return tblAccessorialVariableCodes

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GettblAccessorialVariableCodesFiltered"), sysErrorParameters.sysErrorState.ServerLevelFault, sysErrorParameters.sysErrorSeverity.Critical)
            End Try

            Return Nothing

        End Using
    End Function

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.tblAccessorialVariableCode)
        'Create New Record
        Return New LTS.tblAccessorialVariableCode With {.AccessorialVariableCodesControl = d.AccessorialVariableCodesControl _
                                              , .AccessorialVariableCodesName = d.AccessorialVariableCodesName _
                                              , .AccessorialVariableCodesDescription = d.AccessorialVariableCodesDescription _
                                              , .AccessorialVariableModDate = Date.Now _
                                              , .AccessorialVariableModUser = Me.Parameters.UserName _
                                              , .AccessorialVariableUpdated = If(d.AccessorialVariableUpdated Is Nothing, New Byte() {}, d.AccessorialVariableUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GettblAccessorialVariableCodeFiltered(Control:=CType(LinqTable, LTS.tblAccessorialVariableCode).AccessorialVariableCodesControl)
    End Function

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DTO.tblAccessorialVariableCode)
            Try
                Dim oExists As DTO.tblAccessorialVariableCode = (
                    From t In CType(oDB, NGLMASLookupDataContext).tblAccessorialVariableCodes
                    Where
                     t.AccessorialVariableCodesName = .AccessorialVariableCodesName
                    Select New DTO.tblAccessorialVariableCode With {.AccessorialVariableCodesControl = t.AccessorialVariableCodesControl}).First

                If Not oExists Is Nothing Then
                    Utilities.SaveAppError("Cannot save new tblAccessorialVariableCode data.  The tblAccessorialVariableCode Name, " & .AccessorialVariableCodesName & ", already exist.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DTO.tblAccessorialVariableCode)
            Try
                'Get the newest record that matches the provided criteria
                Dim oExists As DTO.tblAccessorialVariableCode = (
                    From t In CType(oDB, NGLMASLookupDataContext).tblAccessorialVariableCodes
                    Where
                    t.AccessorialVariableCodesControl <> .AccessorialVariableCodesControl _
                    And
                    t.AccessorialVariableCodesName = .AccessorialVariableCodesName
                    Select New DTO.tblAccessorialVariableCode With {.AccessorialVariableCodesControl = t.AccessorialVariableCodesControl}).First


                If Not oExists Is Nothing Then
                    Utilities.SaveAppError("Cannot save tblAccessorialVariableCode changes.  The tblAccessorialVariableCode Name, " & .AccessorialVariableCodesName & ", already exist.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'We do not allow tblAccessorialVariableCodes records to be deleted from this class
        Utilities.SaveAppError("Cannot delete data.  Records cannot be deleted using this interface!", Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))

    End Sub

    Friend Function selectDTOData(ByVal d As LTS.tblAccessorialVariableCode, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.tblAccessorialVariableCode
        Return New DTO.tblAccessorialVariableCode With {.AccessorialVariableCodesControl = d.AccessorialVariableCodesControl _
                                                       , .AccessorialVariableCodesName = d.AccessorialVariableCodesName _
                                                       , .AccessorialVariableCodesDescription = d.AccessorialVariableCodesDescription _
                                                       , .AccessorialVariableModDate = d.AccessorialVariableModDate _
                                                       , .AccessorialVariableModUser = d.AccessorialVariableModUser _
                                                       , .AccessorialVariableUpdated = d.AccessorialVariableUpdated.ToArray() _
                                                       , .Page = page _
                                                       , .Pages = pagecount _
                                                       , .RecordCount = recordcount _
                                                       , .PageSize = pagesize}
    End Function

#End Region

End Class

Public Class NGLtblAccessorialData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        'processParameters(oParameters)
        Me.Parameters = oParameters
        Dim db As New NGLMASLookupDataContext(ConnectionString)
        Me.LinqTable = db.tblAccessorials
        Me.LinqDB = db
        Me.SourceClass = "NGLtblAccessorialData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLookupDataContext(ConnectionString)
            _LinqTable = db.tblAccessorials
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
        Return GettblAccessorialFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GettblAccessorialsFiltered()
    End Function

    Public Function GettblAccessorialFiltered(Optional ByVal Control As Integer = 0, Optional ByVal AccessorialName As String = "") As DTO.tblAccessorial
        Using Logger.StartActivity("GettblAccessorialFiltered(Control: {Control}, AccessorialName: {AccessorialName})", Control, AccessorialName)
            Using db As New NGLMASLookupDataContext(ConnectionString)
                Try

                    'Get the newest record that matches the provided criteria
                    Dim tblAccessorial As DTO.tblAccessorial = (
                    From d In db.tblAccessorials
                    Where
                        (Control = 0 OrElse d.AccessorialCode = Control) _
                         And
                         (AccessorialName Is Nothing OrElse String.IsNullOrEmpty(AccessorialName) OrElse d.AccessorialName = AccessorialName)
                    Select selectDTOData(d)).FirstOrDefault()


                    Return tblAccessorial

                Catch ex As System.Data.SqlClient.SqlException
                    Logger.Error(ex, "Error in GetTblAccessorialsFiltered")
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                Catch ex As InvalidOperationException
                    Logger.Error(ex, "Error in GetTblAccessorialsFiltered")
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
                Catch ex As Exception
                    Logger.Error(ex, "Error in GetTblAccessorialsFiltered")
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                End Try

                Return Nothing
            End Using

        End Using
    End Function

    Public Function GettblAccessorialsFiltered(Optional ByVal AccessorialVariableCode As Integer = 0) As DTO.tblAccessorial()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'Return all records sorted by name
                Dim tblAccessorials() As DTO.tblAccessorial = (
                From d In db.tblAccessorials
                Where
                    (AccessorialVariableCode = 0 OrElse d.AccessorialVariableCode = AccessorialVariableCode)
                Order By d.AccessorialName
                Select selectDTOData(d)).ToArray()
                Return tblAccessorials

            Catch ex As System.Data.SqlClient.SqlException
                Logger.Error(ex, "Error in GetTblAccessorialsFiltered")
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Logger.Error(ex, "Error in GetTblAccessorialsFiltered")
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Logger.Error(ex, "Error in GetTblAccessorialsFiltered")
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GettblAccessorialsFilteredByProfileSpec(ByVal profileSpecific As Boolean) As DTO.tblAccessorial()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'Return all records sorted by name
                Dim tblAccessorials() As DTO.tblAccessorial = (
                From d In db.tblAccessorials
                Where
                    (d.AccessorialProfileSpecific = profileSpecific)
                Order By d.AccessorialName
                Select selectDTOData(d)).ToArray()
                Return tblAccessorials

            Catch ex As System.Data.SqlClient.SqlException
                Logger.Error(ex, "Error in GettblAccessorialsFilteredByProfileSpec")
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Logger.Error(ex, "Error in GettblAccessorialsFilteredByProfileSpec")
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Logger.Error(ex, "Error in GettblAccessorialsFilteredByProfileSpec")
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetAvailableAccessorialFeesByBook(ByVal BookControl As Integer) As DTO.tblAccessorial()
        Dim retVal As New List(Of DTO.tblAccessorial)
        Using Logger.StartActivity("GetAvailableAccessorialFeesByBook(BookControl: {BookControl})", BookControl)
            Using db As New NGLMASLookupDataContext(ConnectionString)
                Try
                    'spNetGetAvailableBookFees50 was modified 7/8/13 to only filter where BookFeesAccessorialFeeTypeControl = 3 [Orders]
                    Dim oFeesAvailable = (From d In db.spNetGetAvailableBookFees50(BookControl) Select d.AccessorialCode).ToArray()
                    'Return all fees that are not being used by this Order we have to requery because  spNetGetAvailableBookFees50 does not return all fields
                    retVal = (
                    From d In db.tblAccessorials Where oFeesAvailable.Contains(d.AccessorialCode)
                    Select selectDTOData(d)).ToList()

                Catch ex As System.Data.SqlClient.SqlException
                    Logger.Error(ex, "Error in GetAvailableAccessorialFeesByBook")
                    throwSQLFaultException(ex.Message)
                Catch ex As InvalidOperationException
                    'do nothing
                    Logger.Error(ex, "Error in GetAvailableAccessorialFeesByBook")
                Catch ex As Exception
                    Logger.Error(ex, "Error in GetAvailableAccessorialFeesByBook")
                    throwUnExpectedFaultException(ex, buildProcedureName("GetAvailableAccessorialFeesByBook"))
                End Try
            End Using
            Return retVal.ToArray()

        End Using
    End Function

    ''' <summary>
    ''' This method has been depreciated and is no longer being used.  Use GetAvailableAccessorialFeesByCarrTarControl instead
    ''' </summary>
    ''' <param name="CarrierControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAvailableAccessorialFeesByCarrier(ByVal CarrierControl As Integer) As DTO.tblAccessorial()
        Dim tblAccessorials() As DTO.tblAccessorial
        Using Logger.StartActivity("GetAvailableAccessorialFeesByCarrier(CarrierControl: {CarrierControl})", CarrierControl)
            Using db As New NGLMASLookupDataContext(ConnectionString)
                Try
                    'Return all records sorted by name
                    tblAccessorials = (
                   From d In db.spNetGetAvailableCarrierFees50(CarrierControl)
                   Select New DTO.tblAccessorial With {.AccessorialCode = d.AccessorialCode _
                                                 , .AccessorialName = d.AccessorialName _
                                                 , .AccessorialDescription = d.AccessorialDescription _
                                                 , .AccessorialModDate = d.AccessorialModDate _
                                                 , .AccessorialModUser = d.AccessorialModUser _
                                                 , .AccessorialUpdated = d.AccessorialUpdated.ToArray()}).ToArray()
                    Return tblAccessorials

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

                Return tblAccessorials
            End Using

        End Using
    End Function

    Public Function GetAvailableAccessorialFeesByLane(ByVal LaneControl As Integer) As DTO.tblAccessorial()
        Dim retVal As New List(Of DTO.tblAccessorial)
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim oFeesAvailable = (From d In db.spNetGetAvailableLaneFees50(LaneControl) Select d.AccessorialCode).ToArray()
                'Return all fees that are not being used by this lane 
                retVal = (
                From d In db.tblAccessorials Where oFeesAvailable.Contains(d.AccessorialCode)
                Select selectDTOData(d)).ToList()

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                'do nothing
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetAvailableAccessorialFeesByLane"))
            End Try

            Return retVal.ToArray()

        End Using
    End Function

    Public Function GetAvailableAccessorialFeesByCarrTarControl(ByVal CarrTarControl As Integer) As DTO.tblAccessorial()
        Dim retVal As New List(Of DTO.tblAccessorial)
        Using Logger.StartActivity("GetAvailableAccessorialFeesByCarrTarControl(CarrTarControl: {CarrTarControl})", CarrTarControl)
            Using db As New NGLMASLookupDataContext(ConnectionString)
                Try
                    'Return all fees that are not being used by this tariff 
                    Dim oCarrTarFees = From c In db.CarrierTariffFeeRefLookups Where c.CarrTarFeesCarrTarControl = CarrTarControl Select c.CarrTarFeesAccessorialCode

                    retVal = (
                    From d In db.tblAccessorials Where d.AccessorialVisible = True And Not oCarrTarFees.Contains(d.AccessorialCode)
                    Select selectDTOData(d)).ToList()

                Catch ex As System.Data.SqlClient.SqlException
                    throwSQLFaultException(ex.Message)
                Catch ex As InvalidOperationException
                    'do nothing
                Catch ex As Exception
                    throwUnExpectedFaultException(ex, buildProcedureName("GetAvailableAccessorialFeesByCarrTarControl"))
                End Try
            End Using
        End Using
        Return retVal.ToArray()


    End Function

    ''' <summary>
    ''' Converts tblaccessorial to a bookfee object
    ''' </summary>
    ''' <param name="d"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ConvertToBookFee(ByVal d As DTO.tblAccessorial, ByVal BookFeesAccessorialFeeTypeControl As Integer) As DTO.BookFee
        Dim newFee As New DTO.BookFee
        If d Is Nothing Then Return Nothing
        newFee.BookFeesAccessorialCode = d.AccessorialCode
        newFee.BookFeesCaption = d.AccessorialCaption
        newFee.BookFeesVariableCode = d.AccessorialVariableCode
        newFee.BookFeesVisible = d.AccessorialVisible
        newFee.BookFeesAutoApprove = d.AccessorialAutoApprove
        newFee.BookFeesAllowCarrierUpdates = d.AccessorialAllowCarrierUpdates
        newFee.BookFeesEDICode = d.AccessorialEDICode
        newFee.BookFeesTaxable = d.AccessorialTaxable
        newFee.BookFeesIsTax = d.AccessorialIsTax
        newFee.BookFeesTaxSortOrder = d.AccessorialTaxSortOrder
        newFee.BookFeesBOLText = d.AccessorialBOLText
        newFee.BookFeesBOLPlacement = d.AccessorialBOLPlacement
        newFee.BookFeesAccessorialFeeAllocationTypeControl = d.AccessorialAccessorialFeeAllocationTypeControl
        newFee.BookFeesTarBracketTypeControl = d.AccessorialTarBracketTypeControl
        newFee.BookFeesAccessorialFeeCalcTypeControl = d.AccessorialAccessorialFeeCalcTypeControl
        newFee.BookFeesAccessorialFeeTypeControl = BookFeesAccessorialFeeTypeControl
        Return newFee
    End Function

    'Added By LVV on 11/15/16 for v-7.0.5.110 Rate Shop HDM Enhancement 
    Public Function GetAccessorialsForRateShop(ByVal DestZip As String, ByVal DestCity As String, ByVal DestState As String, ByVal DestCountry As String) As DTO.tblAccessorial()
        Using Logger.StartActivity("GetAccessorialsForRateShop(DestZip: {DestZip}, DestCity: {DestCity}, DestState: {DestState}, DestCountry: {DestCountry})", DestZip, DestCity, DestState, DestCountry)
            Using db As New NGLMASLookupDataContext(ConnectionString)
                Try
                    Dim results As New List(Of DTO.tblAccessorial)

                    Dim oRet = (From d In db.spGetHDMsForRateShop(DestZip, DestCity, DestState, DestCountry)).ToList()

                    If Not oRet Is Nothing AndAlso oRet.Count > 0 Then
                        If oRet(0).AccessorialCode = 0 Then Return Nothing
                        For Each r In oRet
                            Dim a As New DTO.tblAccessorial
                            With a
                                .AccessorialCode = r.AccessorialCode
                                .AccessorialName = r.AccessorialName
                            End With
                            results.Add(a)
                        Next
                    End If

                    Return results.ToArray()
                    'If Not oRet Is Nothing AndAlso oRet.ErrNumber <> 0 Then
                    '    throwSQLFaultException(oRet.RetMsg)
                    'End If
                Catch ex As Exception
                    ManageLinqDataExceptions(ex, buildProcedureName("GetAccessorialsForRateShop"))
                End Try
            End Using
        End Using

        Return Nothing
    End Function

    Public Function GetAccessorialsFilteredByEDICode(ByVal Code As String) As DTO.tblAccessorial()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim tblAccessorial() As DTO.tblAccessorial = (
                From d In db.tblAccessorials
                Where d.AccessorialEDICode = Code
                Select selectDTOData(d)).ToArray()

                Return tblAccessorial

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

#Region "TMS 365"

    ''' <summary>
    ''' Returns an array of LTS.vAccessorial data objects
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 10/29/18 for v-8.3
    ''' </remarks>
    Public Function GetAccessorials(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.vAccessorial()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vAccessorial
        Using Logger.StartActivity("GetAccessorials(RecordCount: {RecordCount}, filters: {filters})", RecordCount, filters)
            Using db As New NGLMASLookupDataContext(ConnectionString)
                Try
                    'Get the data iqueryable
                    Dim iQuery As IQueryable(Of LTS.vAccessorial)
                    iQuery = db.vAccessorials
                    Dim filterWhere = ""
                    ApplyAllFilters(iQuery, filters, filterWhere)
                    PrepareQuery(iQuery, filters, RecordCount)
                    oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                    Return oRet
                Catch ex As Exception
                    ManageLinqDataExceptions(ex, buildProcedureName("GetAccessorials"), db)
                End Try
            End Using
        End Using

        Return Nothing
    End Function

    Public Function GetAllAccessorials() As LTS.tblAccessorial()

        Dim oRet() As LTS.tblAccessorial
        Using operation = Logger.StartActivity("GetAllAccessorials")
            Using db As New NGLMASLookupDataContext(ConnectionString)
                Try
                    oRet = db.tblAccessorials.ToArray()

                Catch ex As Exception
                    ManageLinqDataExceptions(ex, buildProcedureName("GetAllAccessorials"), db)
                End Try
            End Using
        End Using

        Return oRet
    End Function

    Public Function SaveAccessorial(ByVal oData As LTS.tblAccessorial) As Boolean
        Dim blnRet As Boolean = False
        Using Logger.StartActivity("SaveAccessorial(oData: {oData}", oData)
            If oData Is Nothing Then Return False 'nothing to do
            Using db As New NGLMASLookupDataContext(ConnectionString)
                Try
                    Dim name = oData.AccessorialName
                    Dim code = oData.AccessorialCode
                    Dim blnProfileSpecific = oData.AccessorialProfileSpecific
                    oData.AccessorialModUser = Parameters.UserName
                    oData.AccessorialModDate = Date.Now
                    If oData.AccessorialCode = 0 Then
                        'Logic from ValidateNewRecord
                        If db.tblAccessorials.Any(Function(x) x.AccessorialName = name) Then
                            Logger.Error("Cannot save New tblAccessorial data. The tblAccessorial Name, " & name & ", already exist.")
                            Utilities.SaveAppError("Cannot save New tblAccessorial data. The tblAccessorial Name, " & name & ", already exist.", Me.Parameters)
                            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                        End If
                        'Insert
                        db.tblAccessorials.InsertOnSubmit(oData)
                    Else
                        'Logic from ValidateUpdatedRecord
                        Try
                            If db.tblAccessorials.Any(Function(x) x.AccessorialCode <> code And x.AccessorialName = name) Then
                                Logger.Error("Cannot save tblAccessorial changes. The tblAccessorial Name, " & name & ", already exists.")
                                Utilities.SaveAppError("Cannot save tblAccessorial changes. The tblAccessorial Name, " & name & ", already exists.", Me.Parameters)
                                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                            End If
                        Catch ex As FaultException
                            Logger.Error(ex, "Error in SaveAccessorial")
                            Throw
                        Catch ex As InvalidOperationException
                            Logger.Error(ex, "Error in SaveAccessorial")
                            'do nothing this is the desired result.
                        End Try
                        If blnProfileSpecific = True Then
                            Dim CodesNotSpecific = New Integer() {2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28}
                            For Each f In CodesNotSpecific
                                If f = code Then
                                    Utilities.SaveAppError("Cannot save tblAccessorial changes. The following fee, " & name & ", cannot be made profile specific.", Me.Parameters)
                                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NotProfileSpecific"}, New FaultReason("E_DataValidationFailure"))
                                End If
                            Next
                        End If
                        'Update                   
                        db.tblAccessorials.Attach(oData, True)
                    End If
                    db.SubmitChanges()
                    blnRet = True
                Catch ex As Exception
                    ManageLinqDataExceptions(ex, buildProcedureName("SaveAccessorial"), db)
                End Try
            End Using
        End Using

        Return blnRet
    End Function

    Public Function DeleteAccessorial(ByVal iAccessorialCode As Integer) As Boolean
        Dim blnRet As Boolean = False
        If iAccessorialCode = 0 Then Return False 'nothing to do
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'verify the accessorial exists
                Dim oExisting = db.tblAccessorials.Where(Function(x) x.AccessorialCode = iAccessorialCode).FirstOrDefault()
                If oExisting Is Nothing OrElse oExisting.AccessorialCode = 0 Then Return True
                Dim code = oExisting.AccessorialCode
                'Logic from ValidateDeletedRecord
                Try
                    'Check if the AccessorialCod can be deleted - anything less than 50 not allowed to delete.
                    If code < 50 Then
                        'This is a system code and cannot be deleted
                        Utilities.SaveAppError("Cannot delete data. The selected accessorial fee Is required by the system And cannot be deleted!", Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))
                    End If
                Catch ex As FaultException
                    Throw
                Catch ex As InvalidOperationException
                    'do nothing this is the desired result.
                End Try

                db.tblAccessorials.DeleteOnSubmit(oExisting)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteAccessorial"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Shows all the distinct records by Code/Desc in tblNGLAPICodes and the accossorials in tblAccessorial that each maps to
    ''' Note: 
    '''  Selects distinct records by Code/Desc In tblNGLAPICodes because sometimes codes are repeated between codetypes 1,2,3 And codetype 4
    '''  Instead of making the user map each individual P44 accessorial record (500+) in the UI we group them by distinct code/desc
    '''  There is a one to many relationship between tblAccessorial --> tblNGLAPICodes 
    '''  This view (for grid ds) shows each P44 accessorial And which NGL accessorial it maps to (if any)
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created By LVV on 2/4/19
    ''' </remarks>
    Public Function GetvP44NGLFeeMapXref(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.vP44NGLFeeMapXref()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vP44NGLFeeMapXref
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'Get the data iqueryable
                Dim iQuery As IQueryable(Of LTS.vP44NGLFeeMapXref)
                iQuery = db.vP44NGLFeeMapXrefs
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetvP44NGLFeeMapXref"), db)
            End Try
        End Using
        Return Nothing
    End Function

    Public Sub SaveP44NGLFeeMapXref(ByVal NACControl As Integer, ByVal AccessorialCode As Integer)
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                db.spSaveP44NGLFeeMapXref(NACControl, AccessorialCode)
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveP44NGLFeeMapXref"), db)
            End Try
        End Using
    End Sub


    ''' <summary>
    ''' Returns the accessorial values from tblAccessorial assoicated with the Provided NACCode
    ''' Return code 42 "MSC API Fees" data if the NACCode is not found
    ''' </summary>
    ''' <param name="sNACCode"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Create by RHR for v-8.2.0.117 on 09/04/19
    '''     Primarily used for API Accessorial code lookup
    ''' </remarks>
    Public Function GetAccessorialForNACCode(ByVal sNACCode As String) As LTS.spGetAccessorialForNACCodeResult
        '
        Dim oRet As New LTS.spGetAccessorialForNACCodeResult()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                oRet = db.spGetAccessorialForNACCode(sNACCode).FirstOrDefault()
                If oRet Is Nothing OrElse oRet.AccessorialCode < 1 Then
                    oRet = New LTS.spGetAccessorialForNACCodeResult() With {
                    .AccessorialCode = 42,
                    .AccessorialName = "MSC API Fees",
                    .AccessorialDescription = "Total Of all NGL API Unmapped Fees",
                    .AccessorialVariableCode = 1,
                    .AccessorialVisible = True,
                    .AccessorialAutoApprove = False,
                    .AccessorialAllowCarrierUpdates = False,
                    .AccessorialCaption = "MSC API Fees",
                    .AccessorialEDICode = "MSC",
                    .AccessorialTaxable = True,
                    .AccessorialIsTax = False,
                    .AccessorialTaxSortOrder = 0,
                    .AccessorialBOLText = Nothing,
                    .AccessorialBOLPlacement = Nothing,
                    .AccessorialAccessorialFeeAllocationTypeControl = 1,
                    .AccessorialTarBracketTypeControl = 4,
                    .AccessorialAccessorialFeeCalcTypeControl = 1,
                    .AccessorialProfileSpecific = False,
                    .AccessorialHDMControl = Nothing,
                    .AccessorialMinimum = 0,
                    .AccessorialVariable = 0}
                End If

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAccessorialForNACCode"), db)
            End Try
        End Using
        Return oRet
    End Function

#End Region


#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = TryCast(oData, DTO.tblAccessorial)
        Dim skipObjs As New List(Of String) From {"AccessorialUpdated", "AccessorialModDate", "AccessorialModUser", "Page", "Pages", "RecordCount", "PageSize"}
        Dim oLTS As New LTS.tblAccessorial
        oLTS = CopyMatchingFields(oLTS, d, skipObjs)
        'Create New Record
        With oLTS
            .AccessorialModDate = Date.Now
            .AccessorialModUser = Me.Parameters.UserName
            .AccessorialUpdated = If(d.AccessorialUpdated Is Nothing, New Byte() {}, d.AccessorialUpdated)
        End With
        Return oLTS

    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GettblAccessorialFiltered(Control:=CType(LinqTable, LTS.tblAccessorial).AccessorialCode)
    End Function

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DTO.tblAccessorial)
            Try
                Dim oExists As DTO.tblAccessorial = (
                    From t In CType(oDB, NGLMASLookupDataContext).tblAccessorials
                    Where
                    t.AccessorialName = .AccessorialName
                    Select New DTO.tblAccessorial With {.AccessorialCode = t.AccessorialCode}).First

                If Not oExists Is Nothing Then
                    Utilities.SaveAppError("Cannot save New tblAccessorial data.  The tblAccessorial Name, " & .AccessorialName & ", already exist.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    ''' <summary>
    ''' runs validation on the Accessorial Table before updates
    ''' Rules: (1) the same code must not already exist for this Name 
    ''' (2) validate profile specific requirements some Core fees cannot be set as profile specific
    ''' </summary>
    ''' <param name="oDB"></param>
    ''' <param name="oData"></param>
    ''' <remarks>
    ''' 'Modified by RHR v-7.0.5.102 8/5/2016
    '''   Fixed bug where InvalidOperationException during oExits because we call .First instead of .FirstOrDefault
    '''   Added logic to check the AccessorialCode if oExists is populated. We ignore zero
    '''   Fixed Bug where the AccessorialProfileSpecific validation was running even when the AccessorialProfileSpecific flag was not true
    ''' </remarks>
    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DTO.tblAccessorial)
            Try
                'Get the newest record that matches the provided criteria
                Dim oExists As DTO.tblAccessorial = (
                    From t In CType(oDB, NGLMASLookupDataContext).tblAccessorials
                    Where
                    t.AccessorialCode <> .AccessorialCode _
                    And
                    t.AccessorialName = .AccessorialName
                    Select New DTO.tblAccessorial With {.AccessorialCode = t.AccessorialCode}).FirstOrDefault()  'Modified by RHR v-7.0.5.102 8/5/2016

                'Modified by RHR v-7.0.5.102 8/5/2016
                If Not oExists Is Nothing AndAlso oExists.AccessorialCode <> 0 Then
                    Utilities.SaveAppError("Cannot save tblAccessorial changes.  The tblAccessorial Name, " & .AccessorialName & ", already exists.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
            'Modified by RHR v-7.0.5.102 8/5/2016
            If .AccessorialProfileSpecific = True Then
                Dim CodesNotSpecific = New Integer() {2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28}
                For Each f In CodesNotSpecific
                    If f = .AccessorialCode Then
                        Utilities.SaveAppError("Cannot save tblAccessorial changes.  The following fee, " & .AccessorialName & ", cannot be made profile specific.", Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NotProfileSpecific"}, New FaultReason("E_DataValidationFailure"))
                    End If
                Next
            End If

        End With
    End Sub

    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DTO.tblAccessorial)
            Try
                'Check if the AccessorialCod can be deleted
                'Code Change PFM Approved by Rob for 6.4 - anything less than 50 not allowed to delete.
                If .AccessorialCode < 50 Then
                    'This is a system code and cannot be deleted
                    Utilities.SaveAppError("Cannot delete data.  The selected accessorial fee Is required by the system And cannot be deleted!", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With

    End Sub

    Friend Function selectDTOData(ByVal d As LTS.tblAccessorial, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.tblAccessorial

        Dim oDTO As New DTO.tblAccessorial
        Dim skipObjs As New List(Of String) From {"AccessorialUpdated", "Page", "Pages", "RecordCount", "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .AccessorialUpdated = d.AccessorialUpdated.ToArray()
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oDTO
    End Function

#End Region

End Class

'Added by LVV 5/2/16 for v-7.0.5.1 DAT

Public Class NGLtblPointTypeData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLookupDataContext(ConnectionString)
        Me.LinqTable = db.tblPointTypes
        Me.LinqDB = db
        Me.SourceClass = "NGLtblPointTypeData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLookupDataContext(ConnectionString)
            _LinqTable = db.tblPointTypes
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
        Return GettblPointTypeFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GettblPointTypesFiltered()
    End Function

    ''' <summary>
    ''' Enter zero for LowerControl to return the record with the lowest control PK
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="LowerControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetFirstRecord(ByVal LowerControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass

        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim tblPointType As DTO.tblPointType

                If LowerControl <> 0 Then
                    tblPointType = (
                   From d In db.tblPointTypes
                   Where d.PointTypeControl >= LowerControl
                   Order By d.PointTypeControl
                   Select selectDTOData(d, db)).FirstOrDefault
                Else
                    'Zero indicates that we should get the record with the lowest control number even if it is below zero
                    tblPointType = (
                   From d In db.tblPointTypes
                   Order By d.PointTypeControl
                   Select selectDTOData(d, db)).FirstOrDefault
                End If



                Return tblPointType

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
    ''' Enter the CurrentControl number to use as the reference to the previous record
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetPreviousRecord(ByVal CurrentControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the first record that matches the provided criteria
                Dim tblPointType As DTO.tblPointType = (
                From d In db.tblPointTypes
                Where d.PointTypeControl < CurrentControl
                Order By d.PointTypeControl Descending
                Select selectDTOData(d, db)).FirstOrDefault


                Return tblPointType

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
    ''' Enter the CurrentControl number to use as the reference to the next record
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetNextRecord(ByVal CurrentControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the first record that matches the provided criteria
                Dim tblPointType As DTO.tblPointType = (
                From d In db.tblPointTypes
                Where d.PointTypeControl > CurrentControl
                Order By d.PointTypeControl
                Select selectDTOData(d, db)).FirstOrDefault


                Return tblPointType

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
    ''' Enter zero as the UpperControl number return the record with the highest control PK
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="UpperControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetLastRecord(ByVal UpperControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim tblPointType As DTO.tblPointType

                If UpperControl <> 0 Then

                    tblPointType = (
                    From d In db.tblPointTypes
                    Where d.PointTypeControl >= UpperControl
                    Order By d.PointTypeControl
                    Select selectDTOData(d, db)).FirstOrDefault
                Else
                    'Zero indicates that we should get the hightest PointTypecontrol record
                    tblPointType = (
                    From d In db.tblPointTypes
                    Order By d.PointTypeControl Descending
                    Select selectDTOData(d, db)).FirstOrDefault

                End If


                Return tblPointType

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

    Public Function GettblPointTypeFiltered(ByVal Control As Integer) As DTO.tblPointType
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim tblPointType As DTO.tblPointType = (
                From d In db.tblPointTypes
                Where
                    d.PointTypeControl = Control
                Select selectDTOData(d, db)).First

                Return tblPointType

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

    Public Function GettblPointTypesFiltered(Optional ByVal page As Integer = 1, Optional ByVal pagesize As Integer = 1000) As DTO.tblPointType()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1
                'count the record

                Try
                    intRecordCount = getScalarInteger("Select COUNT(*) from dbo.tblPointType")

                Catch ex As Exception
                    'ignore any record count errors
                End Try
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize

                'Return all the contacts that match the criteria sorted by name
                Dim tblPointTypes() As DTO.tblPointType = (
                From d In db.tblPointTypes
                Order By d.PointTypeControl
                Select selectDTOData(d, db, page, intPageCount, intRecordCount, pagesize)).Skip(intSkip).Take(pagesize).ToArray()
                Return tblPointTypes

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
    ''' Used by System processes forces delete and bypasses validation 
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <remarks></remarks>
    Public Sub SystemDelete(ByVal oData As DTO.tblPointType)
        Using db As New NGLMASLookupDataContext(ConnectionString)
            'Create New Record
            Dim nObject = CopyDTOToLinq(oData)
            db.tblPointTypes.Attach(nObject, True)
            db.tblPointTypes.DeleteOnSubmit(nObject)
            Try
                db.SubmitChanges()
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch conflictEx As ChangeConflictException
                Try
                    'Improper Reference to LinqDB Fixed by RHR 2/6/15
                    'Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(LinqDB))
                    Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(db))
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
            DeleteCleanUp(nObject)
        End Using
    End Sub

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.tblPointType)
        'Create New Record
        Return New LTS.tblPointType With {.PointTypeControl = d.PointTypeControl _
                                    , .PointTypeName = d.PointTypeName _
                                    , .PointTypeDesc = d.PointTypeDesc _
                                    , .PointTypeModDate = Date.Now _
                                    , .PointTypeModUser = Parameters.UserName _
                                    , .PointTypeUpdated = If(d.PointTypeUpdated Is Nothing, New Byte() {}, d.PointTypeUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GettblPointTypeFiltered(Control:=CType(LinqTable, LTS.tblPointType).PointTypeControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim source As LTS.tblPointType = TryCast(LinqTable, LTS.tblPointType)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.tblPointTypes
                       Where d.PointTypeControl = source.PointTypeControl
                       Select New DTO.QuickSaveResults With {.Control = d.PointTypeControl _
                                                            , .ModDate = d.PointTypeModDate _
                                                            , .ModUser = d.PointTypeModUser _
                                                            , .Updated = d.PointTypeUpdated.ToArray}).First

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

    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'We do not allow Class Type records to be deleted 
        Utilities.SaveAppError("Cannot delete data.  Records cannot be deleted Using this interface!", Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))
    End Sub

    Friend Function selectDTOData(ByVal d As LTS.tblPointType, ByRef db As NGLMASLookupDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.tblPointType
        Return New DTO.tblPointType With {.PointTypeControl = d.PointTypeControl _
                                                      , .PointTypeName = d.PointTypeName _
                                                      , .PointTypeDesc = d.PointTypeDesc _
                                                      , .PointTypeModDate = d.PointTypeModDate _
                                                      , .PointTypeModUser = d.PointTypeModUser,
                                                       .Page = page,
                                                       .Pages = pagecount,
                                                       .RecordCount = recordcount,
                                                       .PageSize = pagesize _
                                                      , .PointTypeUpdated = d.PointTypeUpdated.ToArray()}
    End Function

#End Region

End Class

Public Class NGLtblTarAgentData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLookupDataContext(ConnectionString)
        Me.LinqTable = db.tblTarAgents
        Me.LinqDB = db
        Me.SourceClass = "NGLtblTarAgentData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLookupDataContext(ConnectionString)
            _LinqTable = db.tblTarAgents
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
        Return GettblTarAgentFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GettblTarAgentsFiltered()
    End Function

    ''' <summary>
    ''' Enter zero for LowerControl to return the record with the lowest control PK
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="LowerControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetFirstRecord(ByVal LowerControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass

        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim tblTarAgent As DTO.tblTarAgent

                If LowerControl <> 0 Then
                    tblTarAgent = (
                   From d In db.tblTarAgents
                   Where d.TarAgentControl >= LowerControl
                   Order By d.TarAgentControl
                   Select selectDTOData(d, db)).FirstOrDefault
                Else
                    'Zero indicates that we should get the record with the lowest control number even if it is below zero
                    tblTarAgent = (
                   From d In db.tblTarAgents
                   Order By d.TarAgentControl
                   Select selectDTOData(d, db)).FirstOrDefault
                End If



                Return tblTarAgent

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
    ''' Enter the CurrentControl number to use as the reference to the previous record
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetPreviousRecord(ByVal CurrentControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the first record that matches the provided criteria
                Dim tblTarAgent As DTO.tblTarAgent = (
                From d In db.tblTarAgents
                Where d.TarAgentControl < CurrentControl
                Order By d.TarAgentControl Descending
                Select selectDTOData(d, db)).FirstOrDefault


                Return tblTarAgent

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
    ''' Enter the CurrentControl number to use as the reference to the next record
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetNextRecord(ByVal CurrentControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the first record that matches the provided criteria
                Dim tblTarAgent As DTO.tblTarAgent = (
                From d In db.tblTarAgents
                Where d.TarAgentControl > CurrentControl
                Order By d.TarAgentControl
                Select selectDTOData(d, db)).FirstOrDefault


                Return tblTarAgent

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
    ''' Enter zero as the UpperControl number return the record with the highest control PK
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="UpperControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetLastRecord(ByVal UpperControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim tblTarAgent As DTO.tblTarAgent

                If UpperControl <> 0 Then

                    tblTarAgent = (
                    From d In db.tblTarAgents
                    Where d.TarAgentControl >= UpperControl
                    Order By d.TarAgentControl
                    Select selectDTOData(d, db)).FirstOrDefault
                Else
                    'Zero indicates that we should get the hightest TarAgentcontrol record
                    tblTarAgent = (
                    From d In db.tblTarAgents
                    Order By d.TarAgentControl Descending
                    Select selectDTOData(d, db)).FirstOrDefault

                End If


                Return tblTarAgent

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

    Public Function GettblTarAgentFiltered(ByVal Control As Integer) As DTO.tblTarAgent
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim tblTarAgent As DTO.tblTarAgent = (
                From d In db.tblTarAgents
                Where
                    d.TarAgentControl = Control
                Select selectDTOData(d, db)).First

                Return tblTarAgent

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

    Public Function GettblTarAgentsFiltered(Optional ByVal page As Integer = 1, Optional ByVal pagesize As Integer = 1000) As DTO.tblTarAgent()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1
                'count the record

                Try
                    intRecordCount = getScalarInteger("Select COUNT(*) from dbo.tblTarAgent")

                Catch ex As Exception
                    'ignore any record count errors
                End Try
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize

                'Return all the contacts that match the criteria sorted by name
                Dim tblTarAgents() As DTO.tblTarAgent = (
                From d In db.tblTarAgents
                Order By d.TarAgentControl
                Select selectDTOData(d, db, page, intPageCount, intRecordCount, pagesize)).Skip(intSkip).Take(pagesize).ToArray()
                Return tblTarAgents

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

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.tblTarAgent)
        'Create New Record
        Return New LTS.tblTarAgent With {.TarAgentControl = d.TarAgentControl _
                                    , .TarAgentName = d.TarAgentName _
                                    , .TarAgentDesc = d.TarAgentDesc _
                                    , .TarAgentModDate = Date.Now _
                                    , .TarAgentModUser = Parameters.UserName _
                                    , .TarAgentUpdated = If(d.TarAgentUpdated Is Nothing, New Byte() {}, d.TarAgentUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GettblTarAgentFiltered(Control:=CType(LinqTable, LTS.tblTarAgent).TarAgentControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim source As LTS.tblTarAgent = TryCast(LinqTable, LTS.tblTarAgent)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.tblTarAgents
                       Where d.TarAgentControl = source.TarAgentControl
                       Select New DTO.QuickSaveResults With {.Control = d.TarAgentControl _
                                                            , .ModDate = d.TarAgentModDate _
                                                            , .ModUser = d.TarAgentModUser _
                                                            , .Updated = d.TarAgentUpdated.ToArray}).First

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

    Friend Function selectDTOData(ByVal d As LTS.tblTarAgent, ByRef db As NGLMASLookupDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.tblTarAgent
        Return New DTO.tblTarAgent With {.TarAgentControl = d.TarAgentControl _
                                                      , .TarAgentName = d.TarAgentName _
                                                      , .TarAgentDesc = d.TarAgentDesc _
                                                      , .TarAgentModDate = d.TarAgentModDate _
                                                      , .TarAgentModUser = d.TarAgentModUser,
                                                       .Page = page,
                                                       .Pages = pagecount,
                                                       .RecordCount = recordcount,
                                                       .PageSize = pagesize _
                                                      , .TarAgentUpdated = d.TarAgentUpdated.ToArray()}
    End Function

#End Region

End Class

Public Class NGLtblTarBracketTypeData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLookupDataContext(ConnectionString)
        Me.LinqTable = db.tblTarBracketTypes
        Me.LinqDB = db
        Me.SourceClass = "NGLtblTarBracketTypeData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLookupDataContext(ConnectionString)
            _LinqTable = db.tblTarBracketTypes
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
        Return GettblTarBracketTypeFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GettblTarBracketTypesFiltered()
    End Function

    ''' <summary>
    ''' Enter zero for LowerControl to return the record with the lowest control PK
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="LowerControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetFirstRecord(ByVal LowerControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass

        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim tblTarBracketType As DTO.tblTarBracketType

                If LowerControl <> 0 Then
                    tblTarBracketType = (
                   From d In db.tblTarBracketTypes
                   Where d.TarBracketTypeControl >= LowerControl
                   Order By d.TarBracketTypeControl
                   Select selectDTOData(d, db)).FirstOrDefault
                Else
                    'Zero indicates that we should get the record with the lowest control number even if it is below zero
                    tblTarBracketType = (
                   From d In db.tblTarBracketTypes
                   Order By d.TarBracketTypeControl
                   Select selectDTOData(d, db)).FirstOrDefault
                End If



                Return tblTarBracketType

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
    ''' Enter the CurrentControl number to use as the reference to the previous record
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetPreviousRecord(ByVal CurrentControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the first record that matches the provided criteria
                Dim tblTarBracketType As DTO.tblTarBracketType = (
                From d In db.tblTarBracketTypes
                Where d.TarBracketTypeControl < CurrentControl
                Order By d.TarBracketTypeControl Descending
                Select selectDTOData(d, db)).FirstOrDefault


                Return tblTarBracketType

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
    ''' Enter the CurrentControl number to use as the reference to the next record
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetNextRecord(ByVal CurrentControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the first record that matches the provided criteria
                Dim tblTarBracketType As DTO.tblTarBracketType = (
                From d In db.tblTarBracketTypes
                Where d.TarBracketTypeControl > CurrentControl
                Order By d.TarBracketTypeControl
                Select selectDTOData(d, db)).FirstOrDefault


                Return tblTarBracketType

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
    ''' Enter zero as the UpperControl number return the record with the highest control PK
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="UpperControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetLastRecord(ByVal UpperControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim tblTarBracketType As DTO.tblTarBracketType

                If UpperControl <> 0 Then

                    tblTarBracketType = (
                    From d In db.tblTarBracketTypes
                    Where d.TarBracketTypeControl >= UpperControl
                    Order By d.TarBracketTypeControl
                    Select selectDTOData(d, db)).FirstOrDefault
                Else
                    'Zero indicates that we should get the hightest TarBracketTypecontrol record
                    tblTarBracketType = (
                    From d In db.tblTarBracketTypes
                    Order By d.TarBracketTypeControl Descending
                    Select selectDTOData(d, db)).FirstOrDefault

                End If


                Return tblTarBracketType

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

    Public Function GettblTarBracketTypeFiltered(ByVal Control As Integer) As DTO.tblTarBracketType
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim tblTarBracketType As DTO.tblTarBracketType = (
                From d In db.tblTarBracketTypes
                Where
                    d.TarBracketTypeControl = Control
                Select selectDTOData(d, db)).First

                Return tblTarBracketType

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

    Public Function GettblTarBracketTypesFiltered(Optional ByVal page As Integer = 1, Optional ByVal pagesize As Integer = 1000) As DTO.tblTarBracketType()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1
                'count the record

                Try
                    intRecordCount = getScalarInteger("Select COUNT(*) from dbo.tblTarBracketType")

                Catch ex As Exception
                    'ignore any record count errors
                End Try
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize

                'Return all the contacts that match the criteria sorted by name
                Dim tblTarBracketTypes() As DTO.tblTarBracketType = (
                From d In db.tblTarBracketTypes
                Order By d.TarBracketTypeControl
                Select selectDTOData(d, db, page, intPageCount, intRecordCount, pagesize)).Skip(intSkip).Take(pagesize).ToArray()
                Return tblTarBracketTypes

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
    ''' Used by System processes forces delete and bypasses validation 
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <remarks></remarks>
    Public Sub SystemDelete(ByVal oData As DTO.tblTarBracketType)
        Using db As New NGLMASLookupDataContext(ConnectionString)
            'Create New Record
            Dim nObject = CopyDTOToLinq(oData)
            db.tblTarBracketTypes.Attach(nObject, True)
            db.tblTarBracketTypes.DeleteOnSubmit(nObject)
            Try
                db.SubmitChanges()
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch conflictEx As ChangeConflictException
                Try
                    'Improper Reference to LinqDB Fixed by RHR 2/6/15
                    'Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(LinqDB))
                    Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(db))
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
            DeleteCleanUp(nObject)
        End Using
    End Sub

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.tblTarBracketType)
        'Create New Record
        Return New LTS.tblTarBracketType With {.TarBracketTypeControl = d.TarBracketTypeControl _
                                    , .TarBracketTypeName = d.TarBracketTypeName _
                                    , .TarBracketTypeDesc = d.TarBracketTypeDesc _
                                    , .TarBracketTypeModDate = Date.Now _
                                    , .TarBracketTypeModUser = Parameters.UserName _
                                    , .TarBracketTypeUpdated = If(d.TarBracketTypeUpdated Is Nothing, New Byte() {}, d.TarBracketTypeUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GettblTarBracketTypeFiltered(Control:=CType(LinqTable, LTS.tblTarBracketType).TarBracketTypeControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim source As LTS.tblTarBracketType = TryCast(LinqTable, LTS.tblTarBracketType)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.tblTarBracketTypes
                       Where d.TarBracketTypeControl = source.TarBracketTypeControl
                       Select New DTO.QuickSaveResults With {.Control = d.TarBracketTypeControl _
                                                            , .ModDate = d.TarBracketTypeModDate _
                                                            , .ModUser = d.TarBracketTypeModUser _
                                                            , .Updated = d.TarBracketTypeUpdated.ToArray}).First

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

    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'Check if the data already exists only one allowed
        With DirectCast(oData, DTO.tblTarBracketType)
            Try
                'the first 5 are protected
                If .TarBracketTypeControl < 6 Then
                    throwInvalidSaveRequestException("tblTarBracketType.TarBracketTypeName", .TarBracketTypeName)
                End If
                'Check if the name is in use by another bracket type control number
                Dim oReturn As DTO.tblTarBracketType = (
                From t In DirectCast(oDB, NGLMASLookupDataContext).tblTarBracketTypes
                Where
                    (t.TarBracketTypeControl <> .TarBracketTypeControl) _
                    And
                    (t.TarBracketTypeName = .TarBracketTypeName)
                Select New DTO.tblTarBracketType With {.TarBracketTypeControl = t.TarBracketTypeControl}).First

                If Not oReturn Is Nothing Then
                    throwInvalidKeyAlreadyExistsException("tblTarBracketType", "TarBracketTypeName", .TarBracketTypeName)
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'Check if the data already exists only one allowed
        With DirectCast(oData, DTO.tblTarBracketType)
            Try
                'check if the bracket type already exists
                Dim oReturn As DTO.tblTarBracketType = (
                From t In DirectCast(oDB, NGLMASLookupDataContext).tblTarBracketTypes
                Where
                    (t.TarBracketTypeName = .TarBracketTypeName)
                Select New DTO.tblTarBracketType With {.TarBracketTypeControl = t.TarBracketTypeControl}).First

                If Not oReturn Is Nothing Then
                    throwInvalidKeyAlreadyExistsException("tblTarBracketType", "TarBracketTypeName", .TarBracketTypeName)
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'We do not allow Class Type records to be deleted 
        With DirectCast(oData, DTO.tblTarBracketType)
            'the first 5 are protected
            If .TarBracketTypeControl < 6 Then
                throwInvalidDeleteRequestException("tblTarBracketType.TarBracketTypeName", .TarBracketTypeName)
            End If
        End With
    End Sub

    Friend Function selectDTOData(ByVal d As LTS.tblTarBracketType, ByRef db As NGLMASLookupDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.tblTarBracketType
        Return New DTO.tblTarBracketType With {.TarBracketTypeControl = d.TarBracketTypeControl _
                                                      , .TarBracketTypeName = d.TarBracketTypeName _
                                                      , .TarBracketTypeDesc = d.TarBracketTypeDesc _
                                                      , .TarBracketTypeModDate = d.TarBracketTypeModDate _
                                                      , .TarBracketTypeModUser = d.TarBracketTypeModUser,
                                                       .Page = page,
                                                       .Pages = pagecount,
                                                       .RecordCount = recordcount,
                                                       .PageSize = pagesize _
                                                      , .TarBracketTypeUpdated = d.TarBracketTypeUpdated.ToArray()}
    End Function

#End Region

End Class

Public Class NGLtblTariffTypeData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLookupDataContext(ConnectionString)
        Me.LinqTable = db.tblTariffTypes
        Me.LinqDB = db
        Me.SourceClass = "NGLtblTariffTypeData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLookupDataContext(ConnectionString)
            _LinqTable = db.tblTariffTypes
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
        Return GettblTariffTypeFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GettblTariffTypesFiltered()
    End Function

    ''' <summary>
    ''' Enter zero for LowerControl to return the record with the lowest control PK
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="LowerControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetFirstRecord(ByVal LowerControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass

        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim tblTariffType As DTO.tblTariffType

                If LowerControl <> 0 Then
                    tblTariffType = (
                   From d In db.tblTariffTypes
                   Where d.TariffTypeControl >= LowerControl
                   Order By d.TariffTypeControl
                   Select selectDTOData(d, db)).FirstOrDefault
                Else
                    'Zero indicates that we should get the record with the lowest control number even if it is below zero
                    tblTariffType = (
                   From d In db.tblTariffTypes
                   Order By d.TariffTypeControl
                   Select selectDTOData(d, db)).FirstOrDefault
                End If



                Return tblTariffType

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
    ''' Enter the CurrentControl number to use as the reference to the previous record
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetPreviousRecord(ByVal CurrentControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the first record that matches the provided criteria
                Dim tblTariffType As DTO.tblTariffType = (
                From d In db.tblTariffTypes
                Where d.TariffTypeControl < CurrentControl
                Order By d.TariffTypeControl Descending
                Select selectDTOData(d, db)).FirstOrDefault


                Return tblTariffType

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
    ''' Enter the CurrentControl number to use as the reference to the next record
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetNextRecord(ByVal CurrentControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the first record that matches the provided criteria
                Dim tblTariffType As DTO.tblTariffType = (
                From d In db.tblTariffTypes
                Where d.TariffTypeControl > CurrentControl
                Order By d.TariffTypeControl
                Select selectDTOData(d, db)).FirstOrDefault


                Return tblTariffType

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
    ''' Enter zero as the UpperControl number return the record with the highest control PK
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="UpperControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetLastRecord(ByVal UpperControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim tblTariffType As DTO.tblTariffType

                If UpperControl <> 0 Then

                    tblTariffType = (
                    From d In db.tblTariffTypes
                    Where d.TariffTypeControl >= UpperControl
                    Order By d.TariffTypeControl
                    Select selectDTOData(d, db)).FirstOrDefault
                Else
                    'Zero indicates that we should get the hightest TariffTypecontrol record
                    tblTariffType = (
                    From d In db.tblTariffTypes
                    Order By d.TariffTypeControl Descending
                    Select selectDTOData(d, db)).FirstOrDefault

                End If


                Return tblTariffType

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

    Public Function GettblTariffTypeFiltered(ByVal Control As Integer) As DTO.tblTariffType
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim tblTariffType As DTO.tblTariffType = (
                From d In db.tblTariffTypes
                Where
                    d.TariffTypeControl = Control
                Select selectDTOData(d, db)).First

                Return tblTariffType

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

    Public Function GettblTariffTypesFiltered(Optional ByVal page As Integer = 1, Optional ByVal pagesize As Integer = 1000) As DTO.tblTariffType()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1
                'count the record

                Try
                    intRecordCount = getScalarInteger("Select COUNT(*) from dbo.tblTariffType")

                Catch ex As Exception
                    'ignore any record count errors
                End Try
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize

                'Return all the contacts that match the criteria sorted by name
                Dim tblTariffTypes() As DTO.tblTariffType = (
                From d In db.tblTariffTypes
                Order By d.TariffTypeControl
                Select selectDTOData(d, db, page, intPageCount, intRecordCount, pagesize)).Skip(intSkip).Take(pagesize).ToArray()
                Return tblTariffTypes

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
    ''' Used by System processes forces delete and bypasses validation 
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <remarks></remarks>
    Public Sub SystemDelete(ByVal oData As DTO.tblTariffType)
        Using db As New NGLMASLookupDataContext(ConnectionString)
            'Create New Record
            Dim nObject = CopyDTOToLinq(oData)
            db.tblTariffTypes.Attach(nObject, True)
            db.tblTariffTypes.DeleteOnSubmit(nObject)
            Try
                db.SubmitChanges()
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch conflictEx As ChangeConflictException
                Try
                    'Improper Reference to LinqDB Fixed by RHR 2/6/15
                    'Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(LinqDB))
                    Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(db))
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
            DeleteCleanUp(nObject)
        End Using
    End Sub

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.tblTariffType)
        'Create New Record
        Return New LTS.tblTariffType With {.TariffTypeControl = d.TariffTypeControl _
                                    , .TariffTypeName = d.TariffTypeName _
                                    , .TariffTypeDesc = d.TariffTypeDesc _
                                    , .TariffTypeModDate = Date.Now _
                                    , .TariffTypeModUser = Parameters.UserName _
                                    , .TariffTypeUpdated = If(d.TariffTypeUpdated Is Nothing, New Byte() {}, d.TariffTypeUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GettblTariffTypeFiltered(Control:=CType(LinqTable, LTS.tblTariffType).TariffTypeControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim source As LTS.tblTariffType = TryCast(LinqTable, LTS.tblTariffType)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.tblTariffTypes
                       Where d.TariffTypeControl = source.TariffTypeControl
                       Select New DTO.QuickSaveResults With {.Control = d.TariffTypeControl _
                                                            , .ModDate = d.TariffTypeModDate _
                                                            , .ModUser = d.TariffTypeModUser _
                                                            , .Updated = d.TariffTypeUpdated.ToArray}).First

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

    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'We do not allow Class Type records to be deleted 
        Utilities.SaveAppError("Cannot delete data.  Records cannot be deleted Using this interface!", Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))
    End Sub

    Friend Function selectDTOData(ByVal d As LTS.tblTariffType, ByRef db As NGLMASLookupDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.tblTariffType
        Return New DTO.tblTariffType With {.TariffTypeControl = d.TariffTypeControl _
                                                      , .TariffTypeName = d.TariffTypeName _
                                                      , .TariffTypeDesc = d.TariffTypeDesc _
                                                      , .TariffTypeModDate = d.TariffTypeModDate _
                                                      , .TariffTypeModUser = d.TariffTypeModUser,
                                                       .Page = page,
                                                       .Pages = pagecount,
                                                       .RecordCount = recordcount,
                                                       .PageSize = pagesize _
                                                      , .TariffTypeUpdated = d.TariffTypeUpdated.ToArray()}
    End Function

#End Region

End Class

Public Class NGLtblTarRateTypeData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLookupDataContext(ConnectionString)
        Me.LinqTable = db.tblTarRateTypes
        Me.LinqDB = db
        Me.SourceClass = "NGLtblTarRateTypeData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLookupDataContext(ConnectionString)
            _LinqTable = db.tblTarRateTypes
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
        Return GettblTarRateTypeFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GettblTarRateTypesFiltered()
    End Function

    ''' <summary>
    ''' Enter zero for LowerControl to return the record with the lowest control PK
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="LowerControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetFirstRecord(ByVal LowerControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass

        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim tblTarRateType As DTO.tblTarRateType

                If LowerControl <> 0 Then
                    tblTarRateType = (
                   From d In db.tblTarRateTypes
                   Where d.TarRateTypeControl >= LowerControl
                   Order By d.TarRateTypeControl
                   Select selectDTOData(d, db)).FirstOrDefault
                Else
                    'Zero indicates that we should get the record with the lowest control number even if it is below zero
                    tblTarRateType = (
                   From d In db.tblTarRateTypes
                   Order By d.TarRateTypeControl
                   Select selectDTOData(d, db)).FirstOrDefault
                End If



                Return tblTarRateType

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
    ''' Enter the CurrentControl number to use as the reference to the previous record
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetPreviousRecord(ByVal CurrentControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the first record that matches the provided criteria
                Dim tblTarRateType As DTO.tblTarRateType = (
                From d In db.tblTarRateTypes
                Where d.TarRateTypeControl < CurrentControl
                Order By d.TarRateTypeControl Descending
                Select selectDTOData(d, db)).FirstOrDefault


                Return tblTarRateType

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
    ''' Enter the CurrentControl number to use as the reference to the next record
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetNextRecord(ByVal CurrentControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the first record that matches the provided criteria
                Dim tblTarRateType As DTO.tblTarRateType = (
                From d In db.tblTarRateTypes
                Where d.TarRateTypeControl > CurrentControl
                Order By d.TarRateTypeControl
                Select selectDTOData(d, db)).FirstOrDefault


                Return tblTarRateType

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
    ''' Enter zero as the UpperControl number return the record with the highest control PK
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="UpperControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetLastRecord(ByVal UpperControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim tblTarRateType As DTO.tblTarRateType

                If UpperControl <> 0 Then

                    tblTarRateType = (
                    From d In db.tblTarRateTypes
                    Where d.TarRateTypeControl >= UpperControl
                    Order By d.TarRateTypeControl
                    Select selectDTOData(d, db)).FirstOrDefault
                Else
                    'Zero indicates that we should get the hightest TarRateTypecontrol record
                    tblTarRateType = (
                    From d In db.tblTarRateTypes
                    Order By d.TarRateTypeControl Descending
                    Select selectDTOData(d, db)).FirstOrDefault

                End If


                Return tblTarRateType

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

    Public Function GettblTarRateTypeFiltered(ByVal Control As Integer) As DTO.tblTarRateType
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim tblTarRateType As DTO.tblTarRateType = (
                From d In db.tblTarRateTypes
                Where
                    d.TarRateTypeControl = Control
                Select selectDTOData(d, db)).First

                Return tblTarRateType

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

    Public Function GettblTarRateTypesFiltered(Optional ByVal page As Integer = 1, Optional ByVal pagesize As Integer = 1000) As DTO.tblTarRateType()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1
                'count the record

                Try
                    intRecordCount = getScalarInteger("Select COUNT(*) from dbo.tblTarRateType")

                Catch ex As Exception
                    'ignore any record count errors
                End Try
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize

                'Return all the contacts that match the criteria sorted by name
                Dim tblTarRateTypes() As DTO.tblTarRateType = (
                From d In db.tblTarRateTypes
                Order By d.TarRateTypeControl
                Select selectDTOData(d, db, page, intPageCount, intRecordCount, pagesize)).Skip(intSkip).Take(pagesize).ToArray()
                Return tblTarRateTypes

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
    ''' Used by System processes forces delete and bypasses validation 
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <remarks></remarks>
    Public Sub SystemDelete(ByVal oData As DTO.tblTarRateType)
        Using db As New NGLMASLookupDataContext(ConnectionString)
            'Create New Record
            Dim nObject = CopyDTOToLinq(oData)
            db.tblTarRateTypes.Attach(nObject, True)
            db.tblTarRateTypes.DeleteOnSubmit(nObject)
            Try
                db.SubmitChanges()
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch conflictEx As ChangeConflictException
                Try
                    'Improper Reference to LinqDB Fixed by RHR 2/6/15
                    'Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(LinqDB))
                    Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(db))
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
            DeleteCleanUp(nObject)
        End Using
    End Sub

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.tblTarRateType)
        'Create New Record
        Return New LTS.tblTarRateType With {.TarRateTypeControl = d.TarRateTypeControl _
                                    , .TarRateTypeName = d.TarRateTypeName _
                                    , .TarRateTypeDesc = d.TarRateTypeDesc _
                                    , .TarRateTypeModDate = Date.Now _
                                    , .TarRateTypeModUser = Parameters.UserName _
                                    , .TarRateTypeUpdated = If(d.TarRateTypeUpdated Is Nothing, New Byte() {}, d.TarRateTypeUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GettblTarRateTypeFiltered(Control:=CType(LinqTable, LTS.tblTarRateType).TarRateTypeControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim source As LTS.tblTarRateType = TryCast(LinqTable, LTS.tblTarRateType)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.tblTarRateTypes
                       Where d.TarRateTypeControl = source.TarRateTypeControl
                       Select New DTO.QuickSaveResults With {.Control = d.TarRateTypeControl _
                                                            , .ModDate = d.TarRateTypeModDate _
                                                            , .ModUser = d.TarRateTypeModUser _
                                                            , .Updated = d.TarRateTypeUpdated.ToArray}).First

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

    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'We do not allow Class Type records to be deleted 
        Utilities.SaveAppError("Cannot delete data.  Records cannot be deleted Using this interface!", Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))
    End Sub

    Friend Function selectDTOData(ByVal d As LTS.tblTarRateType, ByRef db As NGLMASLookupDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.tblTarRateType
        Return New DTO.tblTarRateType With {.TarRateTypeControl = d.TarRateTypeControl _
                                                      , .TarRateTypeName = d.TarRateTypeName _
                                                      , .TarRateTypeDesc = d.TarRateTypeDesc _
                                                      , .TarRateTypeModDate = d.TarRateTypeModDate _
                                                      , .TarRateTypeModUser = d.TarRateTypeModUser,
                                                       .Page = page,
                                                       .Pages = pagecount,
                                                       .RecordCount = recordcount,
                                                       .PageSize = pagesize _
                                                      , .TarRateTypeUpdated = d.TarRateTypeUpdated.ToArray()}
    End Function

#End Region

End Class

Public Class NGLtblCheckDigitAlgorithmData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLookupDataContext(ConnectionString)
        Me.LinqTable = db.tblCheckDigitAlgorithms
        Me.LinqDB = db
        Me.SourceClass = "NGLtblCheckDigitAlgorithmData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLookupDataContext(ConnectionString)
            _LinqTable = db.tblCheckDigitAlgorithms
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
        Return GettblCheckDigitAlgorithmFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GettblCheckDigitAlgorithmsFiltered()
    End Function

    ''' <summary>
    ''' Enter zero for LowerControl to return the record with the lowest control PK
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="LowerControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetFirstRecord(ByVal LowerControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass

        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim tblCheckDigitAlgorithm As DTO.tblCheckDigitAlgorithm

                If LowerControl <> 0 Then
                    tblCheckDigitAlgorithm = (
                   From d In db.tblCheckDigitAlgorithms
                   Where d.ChkDigAlgControl >= LowerControl
                   Order By d.ChkDigAlgControl
                   Select selectDTOData(d, db)).FirstOrDefault
                Else
                    'Zero indicates that we should get the record with the lowest control number even if it is below zero
                    tblCheckDigitAlgorithm = (
                   From d In db.tblCheckDigitAlgorithms
                   Order By d.ChkDigAlgControl
                   Select selectDTOData(d, db)).FirstOrDefault
                End If

                Return tblCheckDigitAlgorithm

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetFirstRecord"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Enter the CurrentControl number to use as the reference to the previous record
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetPreviousRecord(ByVal CurrentControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'Get the first record that matches the provided criteria
                Dim tblCheckDigitAlgorithm As DTO.tblCheckDigitAlgorithm = (
                From d In db.tblCheckDigitAlgorithms
                Where d.ChkDigAlgControl < CurrentControl
                Order By d.ChkDigAlgControl Descending
                Select selectDTOData(d, db)).FirstOrDefault

                Return tblCheckDigitAlgorithm
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetPreviousRecord"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Enter the CurrentControl number to use as the reference to the next record
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetNextRecord(ByVal CurrentControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the first record that matches the provided criteria
                Dim tblCheckDigitAlgorithm As DTO.tblCheckDigitAlgorithm = (
                From d In db.tblCheckDigitAlgorithms
                Where d.ChkDigAlgControl > CurrentControl
                Order By d.ChkDigAlgControl
                Select selectDTOData(d, db)).FirstOrDefault

                Return tblCheckDigitAlgorithm
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetNextRecord"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Enter zero as the UpperControl number return the record with the highest control PK
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="UpperControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetLastRecord(ByVal UpperControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim tblCheckDigitAlgorithm As DTO.tblCheckDigitAlgorithm

                If UpperControl <> 0 Then

                    tblCheckDigitAlgorithm = (
                    From d In db.tblCheckDigitAlgorithms
                    Where d.ChkDigAlgControl >= UpperControl
                    Order By d.ChkDigAlgControl
                    Select selectDTOData(d, db)).FirstOrDefault
                Else
                    'Zero indicates that we should get the hightest ChkDigAlgcontrol record
                    tblCheckDigitAlgorithm = (
                    From d In db.tblCheckDigitAlgorithms
                    Order By d.ChkDigAlgControl Descending
                    Select selectDTOData(d, db)).FirstOrDefault

                End If

                Return tblCheckDigitAlgorithm
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLastRecord"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GettblCheckDigitAlgorithmFiltered(ByVal Control As Integer) As DTO.tblCheckDigitAlgorithm
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim tblCheckDigitAlgorithm As DTO.tblCheckDigitAlgorithm = (
                From d In db.tblCheckDigitAlgorithms
                Where
                    d.ChkDigAlgControl = Control
                Select selectDTOData(d, db)).First

                Return tblCheckDigitAlgorithm
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GettblCheckDigitAlgorithmFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GettblCheckDigitAlgorithmsFiltered(Optional ByVal page As Integer = 1, Optional ByVal pagesize As Integer = 1000) As DTO.tblCheckDigitAlgorithm()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1
                'count the record
                Try
                    intRecordCount = getScalarInteger("Select COUNT(*) from dbo.tblCheckDigitAlgorithm")
                Catch ex As Exception
                    'ignore any record count errors
                End Try
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize

                'Return all the contacts that match the criteria sorted by name
                Dim tblCheckDigitAlgorithms() As DTO.tblCheckDigitAlgorithm = (
                From d In db.tblCheckDigitAlgorithms
                Order By d.ChkDigAlgControl
                Select selectDTOData(d, db, page, intPageCount, intRecordCount, pagesize)).Skip(intSkip).Take(pagesize).ToArray()
                Return tblCheckDigitAlgorithms
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GettblCheckDigitAlgorithmsFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Used by System processes forces delete and bypasses validation 
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <remarks></remarks>
    Public Sub SystemDelete(ByVal oData As DTO.tblCheckDigitAlgorithm)
        Using db As New NGLMASLookupDataContext(ConnectionString)
            'Create New Record
            Dim nObject = CopyDTOToLinq(oData)
            db.tblCheckDigitAlgorithms.Attach(nObject, True)
            db.tblCheckDigitAlgorithms.DeleteOnSubmit(nObject)
            Try
                db.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SystemDelete"), db)
            End Try
            DeleteCleanUp(nObject)
        End Using
    End Sub

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.tblCheckDigitAlgorithm)
        'Create New Record
        Return New LTS.tblCheckDigitAlgorithm With {.ChkDigAlgControl = d.ChkDigAlgControl _
                                                   , .ChkDigAlgName = d.ChkDigAlgName _
                                                   , .ChkDigAlgDesc = d.ChkDigAlgDesc _
                                                   , .ChkDigAlgActive = d.ChkDigAlgActive _
                                                   , .ChkDigAlgAllowWeightFactor = d.ChkDigAlgAllowWeightFactor _
                                                   , .ChkDigAlgAllowErrorCode = d.ChkDigAlgAllowErrorCode _
                                                   , .ChkDigAlgAllow10DigitCode = d.ChkDigAlgAllow10DigitCode _
                                                   , .ChkDigAlgAllowOver10DigitCode = d.ChkDigAlgAllowOver10DigitCode _
                                                   , .ChkDigAlgAllowZeroDigitCode = d.ChkDigAlgAllowZeroDigitCode _
                                                   , .ChkDigAlgAllowWeightFactorDigitSplitting = d.ChkDigAlgAllowWeightFactorDigitSplitting _
                                                   , .ChkDigAlgAllowUseIndexForWeightFactor = d.ChkDigAlgAllowUseIndexForWeightFactor _
                                                   , .ChkDigAlgExp1 = d.ChkDigAlgExp1 _
                                                   , .ChkDigAlgExp2 = d.ChkDigAlgExp2 _
                                                   , .ChkDigAlgExp3 = d.ChkDigAlgExp3 _
                                                   , .ChkDigAlgExp4 = d.ChkDigAlgExp4 _
                                                   , .ChkDigAlgAllowUseSubtractionFactor = d.ChkDigAlgAllowUseSubtractionFactor _
                                                   , .ChkDigAlgModDate = Date.Now _
                                                   , .ChkDigAlgModUser = Parameters.UserName _
                                                   , .ChkDigAlgUpdated = If(d.ChkDigAlgUpdated Is Nothing, New Byte() {}, d.ChkDigAlgUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GettblCheckDigitAlgorithmFiltered(Control:=CType(LinqTable, LTS.tblCheckDigitAlgorithm).ChkDigAlgControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim source As LTS.tblCheckDigitAlgorithm = TryCast(LinqTable, LTS.tblCheckDigitAlgorithm)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.tblCheckDigitAlgorithms
                       Where d.ChkDigAlgControl = source.ChkDigAlgControl
                       Select New DTO.QuickSaveResults With {.Control = d.ChkDigAlgControl _
                                                            , .ModDate = d.ChkDigAlgModDate _
                                                            , .ModUser = d.ChkDigAlgModUser _
                                                            , .Updated = d.ChkDigAlgUpdated.ToArray}).First
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetQuickSaveResults"))
            End Try

        End Using
        Return ret
    End Function

    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'We do not allow Check Digit Algorithm records to be deleted 
        Utilities.SaveAppError("Cannot delete data.  Records cannot be deleted Using this interface!", Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))
    End Sub

    Friend Function selectDTOData(ByVal d As LTS.tblCheckDigitAlgorithm, ByRef db As NGLMASLookupDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.tblCheckDigitAlgorithm
        Return New DTO.tblCheckDigitAlgorithm With {.ChkDigAlgControl = d.ChkDigAlgControl _
                                                      , .ChkDigAlgName = d.ChkDigAlgName _
                                                      , .ChkDigAlgDesc = d.ChkDigAlgDesc _
                                                      , .ChkDigAlgActive = d.ChkDigAlgActive _
                                                      , .ChkDigAlgAllowWeightFactor = d.ChkDigAlgAllowWeightFactor _
                                                      , .ChkDigAlgAllowErrorCode = d.ChkDigAlgAllowErrorCode _
                                                      , .ChkDigAlgAllow10DigitCode = d.ChkDigAlgAllow10DigitCode _
                                                      , .ChkDigAlgAllowOver10DigitCode = d.ChkDigAlgAllowOver10DigitCode _
                                                      , .ChkDigAlgAllowZeroDigitCode = d.ChkDigAlgAllowZeroDigitCode _
                                                      , .ChkDigAlgAllowWeightFactorDigitSplitting = d.ChkDigAlgAllowWeightFactorDigitSplitting _
                                                      , .ChkDigAlgAllowUseIndexForWeightFactor = d.ChkDigAlgAllowUseIndexForWeightFactor _
                                                      , .ChkDigAlgAllowUseSubtractionFactor = d.ChkDigAlgAllowUseSubtractionFactor _
                                                      , .ChkDigAlgExp1 = d.ChkDigAlgExp1 _
                                                      , .ChkDigAlgExp2 = d.ChkDigAlgExp2 _
                                                      , .ChkDigAlgExp3 = d.ChkDigAlgExp3 _
                                                      , .ChkDigAlgExp4 = d.ChkDigAlgExp4 _
                                                      , .ChkDigAlgModDate = d.ChkDigAlgModDate _
                                                      , .ChkDigAlgModUser = d.ChkDigAlgModUser,
                                                       .Page = page,
                                                       .Pages = pagecount,
                                                       .RecordCount = recordcount,
                                                       .PageSize = pagesize _
                                                      , .ChkDigAlgUpdated = d.ChkDigAlgUpdated.ToArray()}
    End Function

#End Region

End Class

Public Class NGLLookupDataProvider
    Inherits NGLDataProviderBaseClass


#Region " Constructors "
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strConnection As String)
        MyBase.New()
        ConnectionString = strConnection
    End Sub

    Public Sub New(ByVal oParameters As WCFParameters)
        processParameters(oParameters)
    End Sub

#End Region


#Region " Enum "

    ''' <summary>
    ''' tblBidCostAdj.BidCostAdjTypeControl values default is zero
    ''' </summary>
    ''' <remarks>
    ''' created by RHR for v-8.5.4.001
    ''' must map to tblCostAdjType and NGLAPI.CostAdjType
    ''' </remarks>
    Public Enum CostAdjType
        None = 0
        CarrierTotalCost = 1 ' Carrier Total Cost
        CustomerTotalCost = 2 ' Customer Total Cost
        CarrierLineHaul = 3 ' Carrier Line Haul
        CustomerLineHaul = 4 ' Customer Line Haul
        Fuel = 5 ' Fuel
        Accessorial = 6 ' Accessorial
        Discount = 7 ' Discount
        Service = 8 ' Service
    End Enum


    ''' <summary>
    ''' Values for CarrierCostUpchargeLimitVisibility parameter When value matches tblUserGroupCategory
    ''' we hide the carrier cost and only show the Customer Cost (upcharge amounts)
    ''' Use CanUserSeeCarrierCost Static function in NGLLinkDataBaseClass
    '''Sales =  0 ' 11  SalesRep
    '''Customer = 1 ' 11  SalesRep and 10  Customers
    '''NEXTrack = 2 ' 11  SalesRep and 10  Customers and 9 NEXTrackUsers
    '''All others can see the raw carrier cost
    ''' </summary>
    ''' <remarks>
    ''' created by RHR for v-8.5.4.001 
    ''' </remarks>
    Public Enum UpchargeLimitVisibility
        Sales = 0 ' 11  SalesRep
        Customer = 1 ' 11  SalesRep and 10  Customers
        NEXTrack = 2 ' 11  SalesRep and 10  Customers and 9 NEXTrackUsers
    End Enum

    ''' <summary>
    ''' Used to populate tblParProcessOption.ParProcOptParKeyValueType and tblParProcessOption.ParProcOptParKeyTextType
    ''' lookup list
    ''' </summary>
    ''' <remarks>
    '''  Created by RHR For v-8.5.3.006 On 10/19/2022 added Parameter Process Option Lookup Type Enum
    ''' </remarks>
    Public Enum ParProcOptType
        None = 0
        TrueFalse = 1
        List = 2
        EnumBit = 3
        Numeric = 4
        DatePicker = 5
        TimePicker = 6
        Text = 7
    End Enum

    ''' <summary>
    ''' Used for Lane Maintenance and Transit Time Statistics
    ''' </summary>
    ''' <remarks>
    ''' Created by RHR for v-8.4.0.003 on 07/21/21 Colortech Enhancement Transit Time
    '''   0 = None, 1 = Compare By State, 2 = Compare By City , 3 = Compare By 3 digit postal code range
    ''' </remarks>
    Public Enum TransLeadTimeLocationOption
        None = 0
        CompareByState = 1
        CompareByCity = 2
        CompareBy3DigitPostalCodeRange = 3
    End Enum

    ''' <summary>
    ''' Used for Lane Maintenance and Transit Time Statistics
    ''' </summary>
    ''' <remarks>
    ''' Created by RHR for v-8.4.0.003 on 07/21/21 Colortech Enhancement Transit Time
    '''   0 = None, 1 = Calculate Ship Date, 2 = Calculate Required Date
    ''' </remarks>
    Public Enum TransLeadTimeCalcType
        None = 0
        CalculateShipDate = 1
        CalculateRequiredDate = 2
    End Enum

    ''' <summary>
    ''' List of Status Code Types linked to LoadStatusCodes 
    ''' </summary>
    Public Enum LoadStatusCodeTypes
        None = 0
        CARRIER = 1
        EDI = 2
        AMS = 3
        CONSIGNEE = 4
        LoadBoard = 5
        Manual = 6
        MISC = 7
        NEXTrack = 8
        SHIPPER = 9
        System = 10
        WEATHER = 11
        SchedOverride = 12
        FreightBill = 13
        Reduction = 14 'Added By LVV on 4/7/20 for v-8.2.1.006
    End Enum

    Public Enum FBLoadStatusCodes
        FBApproved = 70
        FBLoadNotFinalized = 71
        FBAlreadyPaid = 72
        FBOutsideOfTolerances = 73
        FBInvalidEstimatedFreightCost = 74
        FBWrongCarrier = 75
        FBInvalidShipmentID = 76
        FBCheckAccessorialFees = 77
        FBUnexpected = 78
    End Enum

    Public Enum ActionType
        StoredProcedure = 0
        Service
        Executable
    End Enum

    Public Enum StaticListsType
        NoModDate = 0
        NoModDateSystem
        ModDate
        NoModDateEnum
        ModDateSystem
    End Enum

    ''' <summary>
    '''  Maps API Rate Adjustments UOM to BidCostAdjUOM 
    ''' </summary>
    ''' <remarks>
    ''' Created by RHR for v-8.5.4.004 on 01/21/2024
    ''' Modified by RHR for v-8.5.4.005 on 02/17/2024 added new codes
    ''' </remarks>
    Public Enum APIUOMCodes
        None = 0
        Cases = 1
        Plts = 2
        Cubes = 3
        Boxs = 4
        Other = 5
        Lbs = 6
        Kgs = 7
        Flat = 8
        Perc = 9
    End Enum

    ' Note changes to this enum should also be applied to the same js enum in the client core.js file
    Public Enum StaticLists
        UOM = 0  ' No Mod DAte -- System
        LaneTran ' No Mod DAte
        TempType ' No Mod DAte
        State    ' No Mod DAte -- System
        Seasonality ' ModDate 
        CreditCardType ' No Mod DAte -- System
        PaymentForm ' No Mod DAte -- System
        Currency ' No Mod DAte -- System
        LoadType ' No Mod DAte
        PalletType ' No Mod DAte
        tblFormMenu ' No Mod DAte -- System
        tblReportMenu ' No Mod DAte -- System
        tblReportPar  ' No Mod DAte  -- System
        tblReportParType ' No Mod DAte   -- System
        APAdjReason  ' No Mod DAte
        ComCodes  ' No Mod DAte   -- System
        PayCodes  ' No Mod DAte   -- Complex ?System?
        TranCodes ' No Mod DAte   -- Complex System
        LoadStatusCodes ' No Mod DAte
        NegativeRevenueReason ' No Mod DAte   -- System
        tblBracketType  ' No Mod DAte   -- System
        AccessorialVariableCodes ' ModDate 
        tblParCategories ' No Mod DAte   -- System
        TariffTempType ' No Mod Date Enum
        TariffType ' No Mod Date Enum
        ImportFileType ' No Mod Date Enum
        tblRouteTypes ' No Mod DAte   -- System
        tblStaticRoutes ' ModDate 
        CapacityPreference ' No Mod Date Enum
        ActionType ' No Mod Date Enum
        tblAttribute ' ModDate 
        tblAttributeType ' ModDate 
        tblAction ' ModDate
        ColorCodeType ' No Mod Date Enum
        ApptStatusColorCodeKey ' ModDate  -- System
        ApptTypeColorCodeKey ' ModDate  -- System
        tblPointType ' ModDate  -- System
        tblClassType ' ModDate  -- System
        tblClassTypeAnyAll ' ModDate  -- System
        tblModeType ' ModDate  -- System
        tblTariffType ' ModDate  -- System
        tblTarAgent ' ModDate
        tblTarRateType ' ModDate  -- System
        tblTarBracketType ' ModDate  -- System
        AccessorialFeeCalcType ' ModDate  -- System
        AccessorialFeeAllocationType ' ModDate  -- System
        AccessorialFeeType ' ModDate  -- System
        tblCountries    ' No Mod DAte   -- System
        tblEdiActions ' No Mod DAte   -- System
        tblEdiElements ' No Mod DAte   -- System
        tblEdiTypes ' ModDate  -- System
        tblHDM ' ModDate
        tblERPType ' ModDate  -- System
        tblIntegrationType ' ModDate  -- System
        DATEquipType ' ModDate  -- System
        tblFilterType ' ModDate  -- System
        cmGroupType ' ModDate  -- System
        cmGroupSubType ' ModDate  -- System
        cmDataType ' ModDate  -- System
        cmMenuType ' ModDate  -- System
        tblBidStatusCode ' ModDate  -- System
        tblLanguageCode ' ModDate  -- System
        tblDispatchType ' ModDate  -- System
        tblLoadTenderTransType ' ModDate  -- System
        vNGLAPIPalletTypes ' No Mod DAte
        CalculationFactorType  ' ModDate  -- System 'Added By LVV on 6/11/18 for v-8.3 TMS365 Scheduler
        'Added By LVV on 6/19/18 for v-8.3 TMS365 Scheduler - Merge EDI Tool Changes
        tblListType  ' ModDate  -- System
        tblEDIDataType ' ModDate  -- System
        tblEDIFormattingFunctions ' ModDate
        tblEDIValidationTypes ' ModDate
        tblEDITransformationTypes ' ModDate
        SystemTimeZones  ' No Mod Date Enum --  'Added By LVV on 7/10/18 for v-8.3 TMS365 Scheduler
        SchedulerReasonCodes  ' No Mod DAte   -- System --  'Added By LVV on 7/10/18 for v-8.3 TMS365 Scheduler
        BookingAcssCodes  ' No Mod DAte Complex  -- ?System? -- 'Added by RHR for v-8.2 on 10/15/2018 Static list from vLookupAcssCode used to select NAC  Accessorial Codes that are mapped to NGL Accessorial Codes via tblAccessorialNGLAPIFeesXref
        APIFrtClasses ' No Mod Date Enum
        APIWeightUnit ' No Mod Date Enum
        APILengthUnit ' No Mod Date Enum
        BookTransType   ' No Mod DAte Complex  -- ?System? 
        AllCarriers  ' ModDate  -- 'Added by LVV on 4/4/19 - I needed a list that would return all carriers in the database without apply any filters like user settings or LE
        LegalEntities ' ModDate  -- 'Added by LVV on 4/10/19 - I needed a list that would return all LEAdmins in the database plus "None"
        UserGroupCat  ' ModDate -- User Permission Specific -- 'Added by LVV on 4/10/19
        LECompControls ' ModDate -- 'Added by LVV on 4/12/19 - I needed a list that would return all LEAdmins in the database plus "None" and the control is LEAdminCompControl instead of LEAdminControl (some tables are stupid and use CompControl to link to LE instead of the LEAControl)
        CMVisibility ' No Mod Date Enum -- 'Added by LVV on 7/26/19
        APAuditFltrs ' No Mod Date Enum -- 'Added by LVV on 8/1/19
        FeatureType ' ModDate  -- System -- 'Added by LVV on 2/20/20
        Version ' No Mod Date -- System -- 'Added by LVV on 2/20/20
        APReductionReasonCodes  ' No Mod -- Complex -- 'Added By LVV on 4/6/20 for v-8.2.1.006
        NatFuelZones ' ModDate  -- System -- 'Added By ManoRama On 25-AUG-2020 for Carrier Fuel Index Maint Changes
        TransLeadTimeLocationOptions 'Created by RHR for v-8.4.0.003 on 07/21/21 Colortech Enhancement Transit Time
        TransLeadTimeCalcTypes ' Created by RHR for v-8.4.0.003 on 07/21/21 Colortech Enhancement Transit Time
        'added by RHR for v-8.5 Task Manager Logic
        TaskTypes
        TaskMinutes
        TaskHours
        TaskDays
        TaskMonths
        TaskWeekDays
        'added by RHR for v-8.5.3.006 on 10/19/2022
        ParProcOptType
        'added by RHR for v-8.5.3.006 on 01/05/2023
        ChkDigAlgName
        'added by RHR for v-8.5.4.001 
        CostAdjType
        'added by RHR for v-8.5.4.002
        SSOAType
        'added by RHR for v-8.5.4.004
        APIUOMCodes
    End Enum


    ''' <summary>
    ''' User specific lookup lists typically filtered using the current user settings and role center configuration
    ''' </summary>
    ''' <remarks>
    ''' Created by RHR for v-8.0 on 2/20/2017
    ''' Note changes to this enum should also be applied to the same js enum in the client core.js file
    ''' </remarks>
    Public Enum UserDynamicLists
        Lane = 0
        LaneActive
        LaneNonRestrictedCarriers
        LaneRestrictedCarriers
        LaneCrossDock
        LaneByWarehouse
        LaneActiveByWarehouse
        LaneCrossDockLists
        LaneCarrierTariff
        APCarrier
        APActiveCarrier
        APCarrierPaid
        APCarrierAmtPaid
        Carrier
        CarrierActive
        CarrAdHoc
        CarrierProName
        NatAcctNumber
        ARCompany
        APCompany
        Comp
        CompActive
        CompNEXTrack
        ChartOfAcounts
        TariffShipper
        CarrierQualValidated
        SingleSignOnAccountName
        SubscriptionAlerts
        tblFormList
        tblProcedureList
        tblReportList
        cmPage
        cmPageDetail
        AvailSSOAByUser 'Added by LVV for v-8.1 on 03/28/2018
        LaneTariff 'Added by RHR for v-8.2 on 09/13/2018
        CarrierTariffProName 'Added By RHR on 09/25/2018 for v-8.2 Tariff Changes
        LEAcssCodes 'Added by RHR for v-8.2 on 10/15/2018 User Dynamic list LEAcssCodes uses vLookupAcssCodeByLegalEntity to query all of the NAC codes mapped to NGL codes where the LE Admin Control exists in vAccByLegalEntityCarrier
        PackageDescription 'Added by RHR for v-8.2 on 04/30/2019  User Dynamic list PackageDescription uses vLookupAcssCodeByLegalEntity to query all of the NAC codes mapped to NGL codes where the LE Admin Control exists in vAccByLegalEntityCarrier
        UserLEComps 'Added By LVV on 132/13/19 - Gets a list of Companies associated with the logged in user's Legal Entity and filters using Role Center security (company restrictions)
        NGLExpenseCarrier 'Added By LVV on 12/27/19 - Gets a list of all NGL "Expense Carriers" aka how Mickey And Bill pay utilities etc.
        SSOALEConfigs 'Created by RHR for v-8.5.4.004 on 12/27/2023
    End Enum

    ''' <summary>
    ''' global dynamic lists that are shared by all users and have logic to check for changes
    ''' </summary>
    ''' <remarks>
    ''' Created by RHR for v-8.0 on 2/20/2017
    ''' Modified by RHR for v-8.5.4.003 on 10/24/2023 added tblImageTypeCode lookup
    ''' Note changes to this enum should also be applied to the same js enum in the client core.js file
    ''' </remarks>
    Public Enum GlobalDynamicLists
        CarrierEquipCode
        CarrierEquipment
        vLookupERPSettings
        tblUserSecurity
        cmDataElement
        cmElementField
        cmPageDetailDataElement 'Returns a vlookup list of PageDetails where Control = PageDetControl, Name = PageDetName, and Description/Number = the PageDetDataElmtControl filtered by pageControl
        cmPageDetailElementField 'Returns a vlookup list of Element Fields where Control = ElmtFieldControl, Name = ElmtFieldName, and Description/Number = ElmtFieldDesc filtered by the parent cmPageDetailControl
        CalcFactorTypeUOM 'Added By LVV on 6/19/18 for v-8.3 TMS365 Scheduler --> Returns a vlookup list of UOM for the ResourceControl and CalcType. Only returns the UOM if it is not already in use in the tblDock/TimeCalcFactors table for the Dock and CalcType
        CalcFactorTypeForDock 'Added By LVV on 6/26/18 for v-8.3 TMS365 Scheduler --> Returns a vlookup list of CalculationFactorTypes filtered for the ResourceControl
        Accessorials
        AvailLECarrier 'Added By LVV on 1/21/19 --> Returns list of Carriers that have not yet been assigned to the LECarrier table for the provided LE
        LECarrier 'Added By LVV on 1/21/19 --> Returns list of Carriers that have been assigned to the LECarrier table for the provided LE
        AvailableZoneStates 'Added By ManoRama On 25-AUG-2020 for Carrier Fuel Index Maint Changes
        LECarrierOrAny 'Added By RHR for v-8.5.0.001 on 11/01/2021 --> Returns Any for zero and a list of Carriers that have been assigned to the LECarrier table for the provided LE
        TariffAvailableHDM 'Added By RHR for v-8.5.0.001 on 11/08/2021 -->  Used to select the available HDM Fees not already assigned to the calling tariff ; the caller must filter by CarrTarControl
        tblImageTypeCode ' Modified by RHR for v-8.5.4.003 on 10/24/2023
    End Enum

    Public Enum ListSortType
        Control = 0
        Name = 1
        DescOrNbr = 2
        SeqNo = 4 'Modified By LVV on 3/1/19 - Added Order By SequenceNo
    End Enum

    Public Enum NGLAPICodeTypes
        None = 0
        PickUpAccessorials
        DeliveryAccessorials
        NonSpecificAccessorials
        ChargeCodes
        PackageTypes
        CountryCodes
        CurrencyCodes
        ServiceLevelCodes
        TrackingStatusCodes
    End Enum

    Public Enum LoadTenderTransType
        None = 0
        Outbound = 1
        Transfer = 2
        Inbound = 3
    End Enum

    Public Enum CMVisibility
        SyncWithGrid = 0
        ShowAlways = 1
        HideAlways = 2
    End Enum

    Public Enum APAuditFltrs
        Normal = 0
        Matched = 1
        Approved = 2
        Electronic = 3
        AllErrors = 4
        PA = 5
    End Enum

#End Region

#Region " Functions "

    Public Function GetAttributeTypeControl(ByVal strName As String, ByVal strDescription As String) As Integer
        Dim intRet As Integer = 0
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                Dim results = db.spGetorCreateAttributeTypeControl(Left(strName, 100),
                                                                   Left(strDescription, 4000),
                                                                   Me.Parameters.UserName,
                                                                   intRet)

                Return results

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

            Return intRet

        End Using
    End Function

    Public Function GetAttributeControl(ByVal strName As String,
                                        ByVal intAttTypeControl As Integer,
                                        ByVal strMsg As String,
                                        Optional ByVal intActionControl As Integer = 0) As Integer
        Dim intRet As Integer = 0
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim results = db.spGetorCreateAttributeControl(Left(strName, 100),
                                                               intAttTypeControl,
                                                               intActionControl,
                                                               Left(strMsg, 4000),
                                                               Me.Parameters.UserName,
                                                               intRet)

                Return results

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

            Return intRet

        End Using
    End Function

    Public Function GetActionControl(ByVal strName As String,
                                     ByVal strDescription As String,
                                     ByVal intType As ActionType,
                                     ByVal strCommand As String,
                                     Optional ByVal intArgCount As Integer = 0,
                                     Optional ByVal strArgString As String = "",
                                     Optional ByVal strURI As String = "") As Integer
        Dim intRet As Integer = 0
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim results = db.spGetorCreateActionControl(Left(strName, 100),
                                                            intType,
                                                            intArgCount,
                                                            Left(strArgString, 4000),
                                                            Left(strCommand, 500),
                                                            Left(strURI, 4000),
                                                            Left(strDescription, 4000),
                                                            Me.Parameters.UserName,
                                                            intRet)

                Return results

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

            Return intRet

        End Using
    End Function

    Public Shared Function GetFBReasonCodeDesc(ByVal eFBReasonCode As FBLoadStatusCodes) As String
        Dim sRet As String = ""
        Select Case eFBReasonCode
            Case FBLoadStatusCodes.FBApproved
                sRet = "FB Approved"
            Case FBLoadStatusCodes.FBLoadNotFinalized
                sRet = "FB Load Not finalized"
            Case FBLoadStatusCodes.FBAlreadyPaid
                sRet = "FB Already Paid"
            Case FBLoadStatusCodes.FBOutsideOfTolerances
                sRet = "FB Outside Of Tolerances"
            Case FBLoadStatusCodes.FBInvalidEstimatedFreightCost
                sRet = "FB Invalid Estimated Freight Cost"
            Case FBLoadStatusCodes.FBWrongCarrier
                sRet = "FB Wrong Carrier"
            Case FBLoadStatusCodes.FBInvalidShipmentID
                sRet = "FB Invalid Shipment ID"
            Case FBLoadStatusCodes.FBCheckAccessorialFees
                sRet = "FB Check Accessorial Fees"
            Case FBLoadStatusCodes.FBUnexpected
                sRet = "FB Unexpected"
        End Select
    End Function

#End Region

#Region " Views"

    Public Function GetBookMaintFilters(ByVal CompControl As Integer, ByVal DateFrom As Date, ByVal DaysBack As Integer, Optional ByVal ModDate As Date? = Nothing) As DTO.vBookMaintLookup()
        Dim vBookMaintLookups As New List(Of DTO.vBookMaintLookup)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Try

            Dim strSQL As String = "Select * From(Select top 60000 BookControl,BookProNumber,isnull(BookConsPrefix,'') as BookConsPrefix,isnull(BookSHID,'') as BookSHID,isnull(BookShipCarrierProNumber,'') as BookShipCarrierProNumber,BookCarrOrderNumber,BookOrderSequence, " _
                & " BookLoadPONumber,BookCarrBLNumber,BookFinAPBillNumber,BookCustCompControl,CompNumber,isnull(BookDestName,'') as BookDestName, " _
                & " isnull(BookDestCity,'') as BookDestCity,isnull(BookOrigName,'') as BookOrigName,isnull(BookOrigCity,'') as BookOrigCity  " _
                & " From dbo.Book left outer join dbo.BookLoad on dbo.Book.BookControl = dbo.BookLoad.BookLoadBookControl inner join dbo.Comp on dbo.Book.BookCustCompControl = dbo.Comp.CompControl " _
                & " Where BookDateOrdered between DATEADD ( d , " & DaysBack.ToString & ", '" & DateFrom.ToString("d") & "')  and DateAdd(d, 1, '" & DateFrom.ToString("d") & "')"
            If ModDate.HasValue Then
                strSQL &= " AND (BookModDate > '" & ModDate.Value.ToString() & "') "
            End If
            If CompControl <> 0 Then
                strSQL &= " AND BookCustCompControl = ISNULL(" & CompControl.ToString & ",BookCustCompControl)"
            End If
            strSQL &= " AND ( " _
                & "      isnull((SELECT top 1 isnull(dbo.UserAdmin.UserAdminCompControl,0) FROM dbo.UserAdmin Where dbo.UserAdmin.UserAdminUserName = '" & Parameters.UserName & "'),0) = 0 " _
                & "      OR " _
                & "      ( " _
                & "       isnull((SELECT top 1 isnull(dbo.UserAdmin.UserAdminCompControl,0) FROM dbo.UserAdmin Where dbo.UserAdmin.UserAdminUserName = '" & Parameters.UserName & "'),0) > 0 " _
                & "       AND " _
                & "       CompNumber In (SELECT dbo.UserAdmin.UserAdminCompControl FROM dbo.UserAdmin	Where dbo.UserAdmin.UserAdminUserName = '" & Parameters.UserName & "') " _
                & "      )" _
                & "     )" _
                & " Order by BookDateOrdered desc " _
                & " ) as x" _
                & " Order by BookProNumber desc "

            Dim oQRet As Ngl.Core.Data.QueryResult = oQuery.ExecuteWithFill(strSQL)
            If Not oQRet.Exception Is Nothing Then
                Utilities.SaveAppError(oQRet.Exception.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
            End If
            If Not oQRet.Data Is Nothing AndAlso oQRet.Data.Rows.Count > 0 Then
                For Each oRow As System.Data.DataRow In oQRet.Data.Rows
                    Dim oBookLookup As New DTO.vBookMaintLookup

                    With oBookLookup
                        .BookControl = DTran.getDataRowValue(oRow, "BookControl", 0)
                        .BookProNumber = DTran.getDataRowString(oRow, "BookProNumber", "")
                        .BookConsPrefix = DTran.getDataRowString(oRow, "BookConsPrefix", "")
                        .BookOrderSequence = DTran.getDataRowValue(oRow, "BookOrderSequence", 0)
                        .BookCarrOrderNumber = DTran.getDataRowString(oRow, "BookCarrOrderNumber", "")
                        .BookCarrOrderNumberSeq = .BookCarrOrderNumber & "-" & .BookOrderSequence.ToString
                        .BookLoadPONumber = DTran.getDataRowString(oRow, "BookLoadPONumber", "")
                        .BookCarrBLNumber = DTran.getDataRowString(oRow, "BookCarrBLNumber", "")
                        .BookFinAPBillNumber = DTran.getDataRowString(oRow, "BookFinAPBillNumber", "")
                        .BookCustCompControl = DTran.getDataRowValue(oRow, "BookCustCompControl", 0)
                        .CompNumber = DTran.getDataRowValue(oRow, "CompNumber", 0)
                        .BookDestCity = DTran.getDataRowString(oRow, "BookDestCity", "")
                        .BookDestName = DTran.getDataRowString(oRow, "BookDestName", "")
                        .BookOrigCity = DTran.getDataRowString(oRow, "BookOrigCity", "")
                        .BookOrigName = DTran.getDataRowString(oRow, "BookOrigName", "")
                        .BookSHID = DTran.getDataRowString(oRow, "BookSHID", "")
                        .BookShipCarrierProNumber = DTran.getDataRowString(oRow, "BookShipCarrierProNumber", "")
                    End With
                    vBookMaintLookups.Add(oBookLookup)
                Next
            Else
                vBookMaintLookups.Add(New DTO.vBookMaintLookup)
            End If

        Catch ex As FaultException
            Throw
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As InvalidOperationException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            oQuery = Nothing
        End Try
        If Not vBookMaintLookups Is Nothing AndAlso vBookMaintLookups.Count > 0 Then
            Return vBookMaintLookups.ToArray()
        Else
            Return Nothing
        End If


    End Function

    Public Function GetBookMaintItemByProNumber(ByVal BookProNumber As String, Optional ByVal ModDate As Date? = Nothing) As DTO.vBookMaintLookup
        Dim vBookMaintLookup As New DTO.vBookMaintLookup
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Try

            Dim strSQL As String = "Select top 1 BookControl,BookProNumber,isnull(BookConsPrefix,'') as BookConsPrefix,isnull(BookSHID,'') as BookSHID,isnull(BookShipCarrierProNumber,'') as BookShipCarrierProNumber,BookCarrOrderNumber,BookOrderSequence, " _
                & " BookLoadPONumber,BookCarrBLNumber,BookFinAPBillNumber,BookCustCompControl,CompNumber,isnull(BookDestName,'') as BookDestName, " _
                & " isnull(BookDestCity,'') as BookDestCity,isnull(BookOrigName,'') as BookOrigName,isnull(BookOrigCity,'') as BookOrigCity  " _
                & " From dbo.Book left outer join dbo.BookLoad on dbo.Book.BookControl = dbo.BookLoad.BookLoadBookControl inner join dbo.Comp on dbo.Book.BookCustCompControl = dbo.Comp.CompControl " _
                & " Where BookProNumber = '" & BookProNumber & "'"
            If ModDate.HasValue Then
                strSQL &= " AND (BookModDate > '" & ModDate.Value.ToString() & "') "
            End If
            strSQL &= " AND ( " _
                & "      isnull((SELECT top 1 isnull(dbo.UserAdmin.UserAdminCompControl,0) FROM dbo.UserAdmin Where dbo.UserAdmin.UserAdminUserName = '" & Parameters.UserName & "'),0) = 0 " _
                & "      OR " _
                & "      ( " _
                & "       isnull((SELECT top 1 isnull(dbo.UserAdmin.UserAdminCompControl,0) FROM dbo.UserAdmin Where dbo.UserAdmin.UserAdminUserName = '" & Parameters.UserName & "'),0) > 0 " _
                & "       AND " _
                & "       CompNumber In (SELECT dbo.UserAdmin.UserAdminCompControl FROM dbo.UserAdmin	Where dbo.UserAdmin.UserAdminUserName = '" & Parameters.UserName & "') " _
                & "      )" _
                & "     )"

            Dim oQRet As Ngl.Core.Data.QueryResult = oQuery.ExecuteWithFill(strSQL)
            If Not oQRet.Exception Is Nothing Then
                Utilities.SaveAppError(oQRet.Exception.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
            End If
            If Not oQRet.Data Is Nothing AndAlso oQRet.Data.Rows.Count > 0 Then
                Dim orow As System.Data.DataRow = oQRet.Data.Rows(0)
                'For Each oRow As System.Data.DataRow In oQRet.Data.Rows
                Dim oBookLookup As New DTO.vBookMaintLookup
                With vBookMaintLookup
                    .BookControl = DTran.getDataRowValue(orow, "BookControl", 0)
                    .BookProNumber = DTran.getDataRowString(orow, "BookProNumber", "")
                    .BookConsPrefix = DTran.getDataRowString(orow, "BookConsPrefix", "")
                    .BookOrderSequence = DTran.getDataRowValue(orow, "BookOrderSequence", 0)
                    .BookCarrOrderNumber = DTran.getDataRowString(orow, "BookCarrOrderNumber", "")
                    .BookCarrOrderNumberSeq = .BookCarrOrderNumber & "-" & .BookOrderSequence.ToString
                    .BookLoadPONumber = DTran.getDataRowString(orow, "BookLoadPONumber", "")
                    .BookCarrBLNumber = DTran.getDataRowString(orow, "BookCarrBLNumber", "")
                    .BookFinAPBillNumber = DTran.getDataRowString(orow, "BookFinAPBillNumber", "")
                    .BookCustCompControl = DTran.getDataRowValue(orow, "BookCustCompControl", 0)
                    .CompNumber = DTran.getDataRowValue(orow, "CompNumber", 0)
                    .BookDestCity = DTran.getDataRowString(orow, "BookDestCity", "")
                    .BookDestName = DTran.getDataRowString(orow, "BookDestName", "")
                    .BookOrigCity = DTran.getDataRowString(orow, "BookOrigCity", "")
                    .BookOrigName = DTran.getDataRowString(orow, "BookOrigName", "")
                    .BookSHID = DTran.getDataRowString(orow, "BookSHID", "")
                    .BookShipCarrierProNumber = DTran.getDataRowString(orow, "BookShipCarrierProNumber", "")
                End With

            End If

        Catch ex As FaultException
            Throw
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As InvalidOperationException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            oQuery = Nothing
        End Try
        Return vBookMaintLookup

    End Function

    Public Function GetBookMaintItemByOrderNumber(ByVal OrderNumber As String, ByVal OrderSequence As Integer, ByVal CompNumber As Integer, Optional ByVal ModDate As Date? = Nothing) As DTO.vBookMaintLookup
        Dim vBookMaintLookup As New DTO.vBookMaintLookup
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Try

            Dim strSQL As String = "Select top 1 BookControl,BookProNumber,isnull(BookConsPrefix,'') as BookConsPrefix,isnull(BookSHID,'') as BookSHID,isnull(BookShipCarrierProNumber,'') as BookShipCarrierProNumber,BookCarrOrderNumber,BookOrderSequence, " _
                & " BookLoadPONumber,BookCarrBLNumber,BookFinAPBillNumber,BookCustCompControl,CompNumber,isnull(BookDestName,'') as BookDestName, " _
                & " isnull(BookDestCity,'') as BookDestCity,isnull(BookOrigName,'') as BookOrigName,isnull(BookOrigCity,'') as BookOrigCity  " _
                & " From dbo.Book left outer join dbo.BookLoad on dbo.Book.BookControl = dbo.BookLoad.BookLoadBookControl inner join dbo.Comp on dbo.Book.BookCustCompControl = dbo.Comp.CompControl " _
                & " Where BookCarrOrderNumber = '" & OrderNumber _
                & "' AND BookOrderSequence = " & OrderSequence _
                & " AND CompNumber = " & CompNumber

            If ModDate.HasValue Then
                strSQL &= " AND (BookModDate > '" & ModDate.Value.ToString() & "') "
            End If

            strSQL &= " AND ( " _
                & "      isnull((SELECT top 1 isnull(dbo.UserAdmin.UserAdminCompControl,0) FROM dbo.UserAdmin Where dbo.UserAdmin.UserAdminUserName = '" & Parameters.UserName & "'),0) = 0 " _
                & "      OR " _
                & "      ( " _
                & "       isnull((SELECT top 1 isnull(dbo.UserAdmin.UserAdminCompControl,0) FROM dbo.UserAdmin Where dbo.UserAdmin.UserAdminUserName = '" & Parameters.UserName & "'),0) > 0 " _
                & "       AND " _
                & "       CompNumber In (SELECT dbo.UserAdmin.UserAdminCompControl FROM dbo.UserAdmin	Where dbo.UserAdmin.UserAdminUserName = '" & Parameters.UserName & "') " _
                & "      )" _
                & "     )"

            Dim oQRet As Ngl.Core.Data.QueryResult = oQuery.ExecuteWithFill(strSQL)
            If Not oQRet.Exception Is Nothing Then
                Utilities.SaveAppError(oQRet.Exception.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
            End If
            If Not oQRet.Data Is Nothing AndAlso oQRet.Data.Rows.Count > 0 Then
                Dim orow As System.Data.DataRow = oQRet.Data.Rows(0)
                'For Each oRow As System.Data.DataRow In oQRet.Data.Rows
                Dim oBookLookup As New DTO.vBookMaintLookup
                With vBookMaintLookup
                    .BookControl = DTran.getDataRowValue(orow, "BookControl", 0)
                    .BookProNumber = DTran.getDataRowString(orow, "BookProNumber", "")
                    .BookConsPrefix = DTran.getDataRowString(orow, "BookConsPrefix", "")
                    .BookOrderSequence = DTran.getDataRowValue(orow, "BookOrderSequence", 0)
                    .BookCarrOrderNumber = DTran.getDataRowString(orow, "BookCarrOrderNumber", "")
                    .BookCarrOrderNumberSeq = .BookCarrOrderNumber & "-" & .BookOrderSequence.ToString
                    .BookLoadPONumber = DTran.getDataRowString(orow, "BookLoadPONumber", "")
                    .BookCarrBLNumber = DTran.getDataRowString(orow, "BookCarrBLNumber", "")
                    .BookFinAPBillNumber = DTran.getDataRowString(orow, "BookFinAPBillNumber", "")
                    .BookCustCompControl = DTran.getDataRowValue(orow, "BookCustCompControl", 0)
                    .CompNumber = DTran.getDataRowValue(orow, "CompNumber", 0)
                    .BookDestCity = DTran.getDataRowString(orow, "BookDestCity", "")
                    .BookDestName = DTran.getDataRowString(orow, "BookDestName", "")
                    .BookOrigCity = DTran.getDataRowString(orow, "BookOrigCity", "")
                    .BookOrigName = DTran.getDataRowString(orow, "BookOrigName", "")
                    .BookSHID = DTran.getDataRowString(orow, "BookSHID", "")
                    .BookShipCarrierProNumber = DTran.getDataRowString(orow, "BookShipCarrierProNumber", "")
                End With

            End If

        Catch ex As FaultException
            Throw
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As InvalidOperationException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            oQuery = Nothing
        End Try
        Return vBookMaintLookup

    End Function



    Public Function GetViewLookupStaticList(ByVal enmList As StaticLists, ByVal sortKey As ListSortType, Optional ByVal Criteria As Object = Nothing, Optional ByVal ModDate As Date? = Nothing) As DTO.vLookupList()
        Dim Enumerator As Type = GetType(StaticLists)
        Dim strListName = [Enum].GetName(Enumerator, enmList)
        Return GetViewLookupList(strListName, sortKey, Criteria, ModDate)
    End Function

    Public Function GetViewLookupUserDynamicList(ByVal enmList As UserDynamicLists, ByVal sortKey As ListSortType, Optional ByVal Criteria As Object = Nothing, Optional ByVal ModDate As Date? = Nothing) As DTO.vLookupList()
        Dim Enumerator As Type = GetType(UserDynamicLists)
        Dim strListName = [Enum].GetName(Enumerator, enmList)
        Return GetViewLookupList(strListName, sortKey, Criteria, ModDate)
    End Function

    Public Function GetViewLookupGlobalDynamicList(ByVal enmList As GlobalDynamicLists, ByVal sortKey As ListSortType, Optional ByVal Criteria As Object = Nothing, Optional ByVal ModDate As Date? = Nothing) As DTO.vLookupList()
        Dim Enumerator As Type = GetType(GlobalDynamicLists)
        Dim strListName = [Enum].GetName(Enumerator, enmList)
        Return GetViewLookupList(strListName, sortKey, Criteria, ModDate)
    End Function

    ''' <summary>
    ''' Returns a lookup list based on the listName paramter, Criteria can be used to filter data but each list 
    ''' applies the Criteria differently,  typically this is a FK to the parent table.  ModDate can be used to 
    ''' return only those records that have changed since the date provided.  It is only supported by some lists
    ''' specifically dynaimc lists,  static lists normally return all records.  
    ''' The caller is responsible for merging 
    ''' changes when the ModDate paramter is provided
    ''' </summary>
    ''' <param name="listName"></param>
    ''' <param name="sortKey"></param>
    ''' <param name="Criteria"></param>
    ''' <param name="ModDate"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.0 on 2/20/2017
    '''   Additional logic is implemented for dynamic lists where the ModDate parameter
    '''   can be used to return only records that have changed after a certain date.
    ''' </remarks>
    Public Function GetViewLookupList(ByVal listName As String, ByVal sortKey As Integer, Optional ByVal Criteria As Object = Nothing, Optional ByVal ModDate As Date? = Nothing) As DTO.vLookupList()
        Dim vList() As DTO.vLookupList

        Select Case listName
            Case "UOM"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        vList = (
                            From t In db.UOMs
                            Order By t.UOM
                            Select New DTO.vLookupList _
                            With {.Name = t.UOM}).ToArray()
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "TempType"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.TempTypes
                                    Order By t.TempType
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ID, .Name = t.CommCodeType, .Description = t.TempType}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.TempTypes
                                    Order By t.TempType
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ID, .Name = t.CommCodeType, .Description = t.TempType}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.TempTypes
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ID, .Name = t.CommCodeType, .Description = t.TempType}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "BookTransType"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Dim lvList As New List(Of DTO.vLookupList)
                        Select Case sortKey
                            Case 1
                                lvList = (
                                    From t In db.LaneTrans
                                    Order By t.LaneTransNumber
                                    Select New DTO.vLookupList _
                                    With {.Control = t.LaneTransNumber, .Name = t.LaneTransTypeDesc, .Description = t.LaneTransTypeDesc}).ToList()
                            Case 2
                                lvList = (
                                    From t In db.LaneTrans
                                    Order By t.LaneTransTypeDesc
                                    Select New DTO.vLookupList _
                                    With {.Control = t.LaneTransNumber, .Name = t.LaneTransTypeDesc, .Description = t.LaneTransTypeDesc}).ToList()
                            Case Else
                                lvList = (
                                    From t In db.LaneTrans
                                    Select New DTO.vLookupList _
                                    With {.Control = t.LaneTransNumber, .Name = t.LaneTransTypeDesc, .Description = t.LaneTransTypeDesc}).ToList()
                        End Select
                        lvList.Insert(0, New DTO.vLookupList() With {.Control = 0, .Name = "None or N/A", .Description = "None or N/A"})
                        vList = lvList.ToArray()
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using

            Case "LaneTran"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.LaneTrans
                                    Order By t.LaneTransNumber
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ID, .Name = t.LaneTransTypeDesc, .Description = t.LaneTransNumber.ToString}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.LaneTrans
                                    Order By t.LaneTransTypeDesc
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ID, .Name = t.LaneTransTypeDesc, .Description = t.LaneTransNumber.ToString}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.LaneTrans
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ID, .Name = t.LaneTransTypeDesc, .Description = t.LaneTransNumber.ToString}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using

            Case "State"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Dim lookupList As New List(Of DTO.vLookupList)
                        Select Case sortKey
                            Case 1
                                lookupList = (
                                    From t In db.States
                                    Order By t.STATE
                                    Select New DTO.vLookupList _
                                    With {.Control = t.StateControl, .Name = t.STATE, .Description = t.STATENAME}).ToList()
                            Case 2
                                lookupList = (
                                    From t In db.States
                                    Order By t.STATENAME
                                    Select New DTO.vLookupList _
                                    With {.Control = t.StateControl, .Name = t.STATE, .Description = t.STATENAME}).ToList()
                            Case Else
                                lookupList = (
                                    From t In db.States
                                    Select New DTO.vLookupList _
                                    With {.Control = t.StateControl, .Name = t.STATE, .Description = t.STATENAME}).ToList()
                        End Select

                        Dim itemDefault As New DTO.vLookupList(0, "", "")
                        If lookupList Is Nothing Then lookupList = New List(Of DTO.vLookupList)
                        lookupList.Insert(0, itemDefault)
                        vList = lookupList.ToArray()

                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using

            Case "AvailableZoneStates" 'Added By Manorama for NatFuelStateZones chnages
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Dim lookupList As New List(Of DTO.vLookupList)
                        Dim exstate As New List(Of String) 'getting list of strings for states which are not mapped in NatFuelZoneStates'
                        exstate = (
                                (From t In db.States
                                 Select t.STATE).Distinct().Except((From natst In db.NatFuelZoneStates
                                                                    Select natst.NatFuelZoneStatesState)).Distinct()).ToList()
                        Select Case sortKey
                            Case 1
                                lookupList = (
                                            From t In db.States
                                            Where (exstate Is Nothing OrElse exstate.Count = 0 OrElse exstate.Contains(t.STATE))
                                            Order By t.STATENAME
                                            Select New DTO.vLookupList _
                                            With {.Control = t.StateControl, .Name = t.STATE, .Description = t.STATENAME}).ToList()
                            Case 2
                                lookupList = (
                                            From t In db.States
                                            Order By t.STATENAME
                                            Select New DTO.vLookupList _
                                            With {.Control = t.StateControl, .Name = t.STATE, .Description = t.STATENAME}).ToList()
                            Case Else
                                lookupList = (
                                            From t In db.States
                                            Select New DTO.vLookupList _
                                            With {.Control = t.StateControl, .Name = t.STATE, .Description = t.STATENAME}).ToList()
                        End Select

                        Dim itemDefault As New DTO.vLookupList(0, "", "")
                        If lookupList Is Nothing Then lookupList = New List(Of DTO.vLookupList)
                        lookupList.Insert(0, itemDefault)
                        vList = lookupList.ToArray()

                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "CarrierEquipCode" 'Modified by RHR for v-8.0 on 2/20/2017
                Using db As New NGLMASCarrierDataContext(ConnectionString)
                    Try
                        'Logic to test the ModDate Parameter
                        'Dim blnUseModDate As Boolean = False
                        'Dim dtStartDate As Date = Date.Now()
                        'If ModDate.HasValue Then
                        '    blnUseModDate = True
                        '    dtStartDate = ModDate.Value
                        'End If
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.CarrierEquipCodes
                                    Order By t.CarrierEquipCode
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrierEquipControl, .Name = t.CarrierEquipCode, .Description = t.CarrierEquipDescription}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.CarrierEquipCodes
                                    Order By t.CarrierEquipDescription
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrierEquipControl, .Name = t.CarrierEquipCode, .Description = t.CarrierEquipDescription}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.CarrierEquipCodes
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrierEquipControl, .Name = t.CarrierEquipCode, .Description = t.CarrierEquipDescription}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using

            Case "Seasonality" ' Modified By RHR for v-8.3.0.002 on 12/1/2020 added ModDate Logic
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.Seasonalities
                                    Where (ModDate.HasValue = False OrElse t.SeasModDate > ModDate)
                                    Order By t.SeasProfileNo
                                    Select New DTO.vLookupList _
                                    With {.Control = t.SeasControl, .Name = t.SeasProfileNo, .Description = t.SeasDescription}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.Seasonalities
                                    Where (ModDate.HasValue = False OrElse t.SeasModDate > ModDate)
                                    Order By t.SeasDescription
                                    Select New DTO.vLookupList _
                                    With {.Control = t.SeasControl, .Name = t.SeasProfileNo, .Description = t.SeasDescription}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.Seasonalities
                                    Where (ModDate.HasValue = False OrElse t.SeasModDate > ModDate)
                                    Select New DTO.vLookupList _
                                    With {.Control = t.SeasControl, .Name = t.SeasProfileNo, .Description = t.SeasDescription}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using

            Case "CreditCardType"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.CreditCardTypes
                                    Order By t.CreditCardType
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CreditCardControlNumber, .Name = t.CreditCardType}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.CreditCardTypes
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CreditCardControlNumber, .Name = t.CreditCardType}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "PaymentForm"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.PaymentForms
                                    Order By t.PaymentFormType
                                    Select New DTO.vLookupList _
                                    With {.Control = t.PaymentFormNumber, .Name = t.PaymentFormType}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.PaymentForms
                                    Select New DTO.vLookupList _
                                    With {.Control = t.PaymentFormNumber, .Name = t.PaymentFormType}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "Currency"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.Currencies
                                    Order By t.CurrencyName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ID, .Name = t.CurrencyName, .Description = t.CurrencyCountry.ToString}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.Currencies
                                    Order By t.CurrencyCountry
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ID, .Name = t.CurrencyName, .Description = t.CurrencyCountry.ToString}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.Currencies
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ID, .Name = t.CurrencyName, .Description = t.CurrencyCountry.ToString}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "LoadType"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.LoadTypes
                                    Order By t.LoadType
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ID, .Name = t.LoadType, .Description = t.LoadTypeGroup.ToString}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.LoadTypes
                                    Order By t.LoadTypeGroup
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ID, .Name = t.LoadType, .Description = t.LoadTypeGroup.ToString}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.LoadTypes
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ID, .Name = t.LoadType, .Description = t.LoadTypeGroup.ToString}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "Lane" 'Modified by RHR for v-8.0 on 2/20/2017
                Using db As New NGLMASLaneDataContext(ConnectionString)
                    Try
                        'Logic to test the ModDate Parameter
                        'Dim blnUseModDate As Boolean = False
                        'Dim dtStartDate As Date = Date.Now()
                        'If ModDate.HasValue Then
                        '    blnUseModDate = True
                        '    dtStartDate = ModDate.Value
                        'End If
                        Dim oSecureComp = From s In db.vUserAdminWithCompControlRefLanes Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.Lanes
                                    Where (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.LaneCompControl)) _
                                        And
                                        (ModDate.HasValue = False OrElse t.LaneModDate > ModDate)
                                    Order By t.LaneName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.LaneControl, .Name = t.LaneName, .Description = t.LaneNumber.ToString}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.Lanes
                                    Where (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.LaneCompControl)) _
                                        And
                                        (ModDate.HasValue = False OrElse t.LaneModDate > ModDate)
                                    Order By t.LaneNumber
                                    Select New DTO.vLookupList _
                                    With {.Control = t.LaneControl, .Name = t.LaneName, .Description = t.LaneNumber.ToString}).ToArray()
                                'With {.Control = t.LaneControl, .Name = t.LaneName, .Description = t.LaneNumber.ToString}).Take(12000).ToArray()
                            Case Else
                                vList = (
                                    From t In db.Lanes
                                    Where (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.LaneCompControl)) _
                                        And
                                        (ModDate.HasValue = False OrElse t.LaneModDate > ModDate)
                                    Select New DTO.vLookupList _
                                    With {.Control = t.LaneControl, .Name = t.LaneName, .Description = t.LaneNumber.ToString}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using

            Case "Carrier" 'Modified by RHR for v-8.0 on 2/20/2017
                Using db As New NGLMASCarrierDataContext(ConnectionString)
                    Try
                        'Logic to test the ModDate Parameter
                        'Dim blnUseModDate As Boolean = False
                        'Dim dtStartDate As Date = Date.Now()
                        'If ModDate.HasValue Then
                        '    blnUseModDate = True
                        '    dtStartDate = ModDate.Value
                        'End If
                        'Dim oSecureComp = From s In db.vUserAdminWithCompControlRefCarriers Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                        'db.Log = New DebugTextWriter
                        'Dim oSecureTariffs = From d In db.spGetSecureCarriers(Me.Parameters.UserName) Select d.CarrTarCarrierControl

                        Dim oSecureTariffs = From d In db.vSecureCarriers Where d.UserAdminUserName = Me.Parameters.UserName Select d.CarrTarCarrierControl


                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.Carriers
                                    Where (oSecureTariffs Is Nothing OrElse oSecureTariffs.Count = 0 OrElse oSecureTariffs.Contains(t.CarrierControl)) _
                                    And
                                    (ModDate.HasValue = False OrElse t.CarrierModDate > ModDate)
                                    Order By t.CarrierName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrierControl, .Name = t.CarrierName, .Description = t.CarrierNumber.ToString}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.Carriers
                                    Where (oSecureTariffs Is Nothing OrElse oSecureTariffs.Count = 0 OrElse oSecureTariffs.Contains(t.CarrierControl)) _
                                    And
                                    (ModDate.HasValue = False OrElse t.CarrierModDate > ModDate)
                                    Order By t.CarrierNumber
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrierControl, .Name = t.CarrierName, .Description = t.CarrierNumber.ToString}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.Carriers
                                    Where (oSecureTariffs Is Nothing OrElse oSecureTariffs.Count = 0 OrElse oSecureTariffs.Contains(t.CarrierControl)) _
                                    And
                                    (ModDate.HasValue = False OrElse t.CarrierModDate > ModDate)
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrierControl, .Name = t.CarrierName, .Description = t.CarrierNumber.ToString}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using

            Case "Comp" 'Modified by RHR for v-8.0 on 2/20/2017
                Using db As New NGLMASCompDataContext(ConnectionString)
                    Try
                        'Logic to test the ModDate Parameter
                        'Dim blnUseModDate As Boolean = False
                        'Dim dtStartDate As Date = Date.Now()
                        'If ModDate.HasValue Then
                        '    blnUseModDate = True
                        '    dtStartDate = ModDate.Value
                        'End If
                        'db.Log = New DebugTextWriter
                        Dim oSecureComp = From s In db.vUserAdminWithCompControlRefComps Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.Comps
                                    Where (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.CompControl)) _
                                    And
                                    (ModDate.HasValue = False OrElse t.CompModDate > ModDate)
                                    Order By t.CompName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CompControl, .Name = t.CompName, .Description = t.CompNumber.ToString}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.Comps
                                    Where (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.CompControl)) _
                                    And
                                    (ModDate.HasValue = False OrElse t.CompModDate > ModDate)
                                    Order By t.CompNumber
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CompControl, .Name = t.CompName, .Description = t.CompNumber.ToString}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.Comps
                                    Where (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.CompControl)) _
                                    And
                                    (ModDate.HasValue = False OrElse t.CompModDate > ModDate)
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CompControl, .Name = t.CompName, .Description = t.CompNumber.ToString}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "NatAcctNumber" 'Modified by RHR for v-8.0 on 2/20/2017
                Using db As New NGLMASCompDataContext(ConnectionString)
                    Try
                        'Logic to test the ModDate Parameter
                        'Dim blnUseModDate As Boolean = False
                        'Dim dtStartDate As Date = Date.Now()
                        'If ModDate.HasValue Then
                        '    blnUseModDate = True
                        '    dtStartDate = ModDate.Value
                        'End If

                        Dim oSecureComp = From s In db.vUserAdminWithCompControlRefComps Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                        Dim oNatAccount As List(Of DTO.Comp) = (From t In db.Comps
                                                                Where (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.CompControl)) _
                            And
                            If(t.CompNatNumber, 0) > 0
                                                                Select New DTO.Comp _
                            With {.CompNatName = t.CompNatName, .CompNatNumber = t.CompNatNumber}).Distinct.ToList()

                        Select Case sortKey
                            Case 1

                                vList = (
                                        From d In oNatAccount
                                        Order By d.CompNatName
                                        Select New DTO.vLookupList _
                                        With {.Control = d.CompNatNumber, .Name = d.CompNatName, .Description = d.CompNatNumber}).ToArray

                            Case Else

                                vList = (
                                        From d In oNatAccount
                                        Order By d.CompNatNumber
                                        Select New DTO.vLookupList _
                                        With {.Control = d.CompNatNumber, .Name = d.CompNatName, .Description = d.CompNatNumber}).ToArray

                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "PalletType"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.PalletTypes
                                    Order By t.PalletType
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ID, .Name = t.PalletType, .Description = t.PalletTypeDescription}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.PalletTypes
                                    Order By t.PalletTypeDescription
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ID, .Name = t.PalletType, .Description = t.PalletTypeDescription}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.PalletTypes
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ID, .Name = t.PalletType, .Description = t.PalletTypeDescription}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "tblFormList" 'Modified by RHR for v-8.0 on 2/20/2017
                Using db As New NGLMASSecurityDataContext(ConnectionString)
                    Try

                        'Logic to check user security settings
                        Dim blnUseFormFilter As Boolean = False
                        Dim lRestricted As List(Of Integer) = (From x In db.tblFormSecurityXrefs Join u In db.tblUserSecurities On x.UserSecurityControl Equals u.UserSecurityControl Where u.UserName = Me.Parameters.UserName Select x.FormControl).ToList()
                        If Not lRestricted Is Nothing AndAlso lRestricted.Count() > 0 Then blnUseFormFilter = True

                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.tblFormLists
                                    Where (blnUseFormFilter = False OrElse Not lRestricted.Contains(t.FormControl))
                                    Order By t.FormName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.FormControl, .Name = t.FormName, .Description = t.FormDescription}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.tblFormLists
                                    Where (blnUseFormFilter = False OrElse Not lRestricted.Contains(t.FormControl))
                                    Order By t.FormDescription
                                    Select New DTO.vLookupList _
                                    With {.Control = t.FormControl, .Name = t.FormName, .Description = t.FormDescription}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.tblFormLists
                                    Where (blnUseFormFilter = False OrElse Not lRestricted.Contains(t.FormControl))
                                    Select New DTO.vLookupList _
                                    With {.Control = t.FormControl, .Name = t.FormName, .Description = t.FormDescription}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "tblFormMenu"
                Using db As New NGLMASSecurityDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.tblFormMenus
                                    Order By t.FormMenuName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.FormMenuControl, .Name = t.FormMenuName, .Description = t.FormMenuDescription}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.tblFormMenus
                                    Order By t.FormMenuDescription
                                    Select New DTO.vLookupList _
                                    With {.Control = t.FormMenuControl, .Name = t.FormMenuName, .Description = t.FormMenuDescription}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.tblFormMenus
                                    Select New DTO.vLookupList _
                                    With {.Control = t.FormMenuControl, .Name = t.FormMenuName, .Description = t.FormMenuDescription}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "tblProcedureList"
                Using db As New NGLMASSecurityDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.tblProcedureLists
                                    Order By t.ProcedureName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ProcedureControl, .Name = t.ProcedureName, .Description = t.ProcedureDescription}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.tblProcedureLists
                                    Order By t.ProcedureDescription
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ProcedureControl, .Name = t.ProcedureName, .Description = t.ProcedureDescription}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.tblProcedureLists
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ProcedureControl, .Name = t.ProcedureName, .Description = t.ProcedureDescription}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "tblReportList"
                Using db As New NGLMASSecurityDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.tblReportLists
                                    Order By t.ReportName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ReportControl, .Name = t.ReportName, .Description = t.ReportDescription}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.tblReportLists
                                    Order By t.ReportDescription
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ReportControl, .Name = t.ReportName, .Description = t.ReportDescription}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.tblReportLists
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ReportControl, .Name = t.ReportName, .Description = t.ReportDescription}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "tblReportPar"
                Using db As New NGLMASSecurityDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.tblReportPars
                                    Order By t.ReportParName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ReportParControl, .Name = t.ReportParName, .Description = t.ReportParText}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.tblReportPars
                                    Order By t.ReportParText
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ReportParControl, .Name = t.ReportParName, .Description = t.ReportParText}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.tblReportPars
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ReportParControl, .Name = t.ReportParName, .Description = t.ReportParText}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "tblReportMenu"
                Using db As New NGLMASSecurityDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.tblReportMenus
                                    Order By t.ReportMenuName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ReportMenuControl, .Name = t.ReportMenuName, .Description = t.ReportMenuDescription}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.tblReportMenus
                                    Order By t.ReportMenuDescription
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ReportMenuControl, .Name = t.ReportMenuName, .Description = t.ReportMenuDescription}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.tblReportMenus
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ReportMenuControl, .Name = t.ReportMenuName, .Description = t.ReportMenuDescription}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "tblReportParType"
                Using db As New NGLMASSecurityDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.tblReportParTypes
                                    Order By t.ReportParTypeName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ReportParTypeControl, .Name = t.ReportParTypeName, .Description = t.ReportParTypeDesc}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.tblReportParTypes
                                    Order By t.ReportParTypeDesc
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ReportParTypeControl, .Name = t.ReportParTypeName, .Description = t.ReportParTypeDesc}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.tblReportParTypes
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ReportParTypeControl, .Name = t.ReportParTypeName, .Description = t.ReportParTypeDesc}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "tblUserSecurity"
                Using db As New NGLMASSecurityDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.tblUserSecurities
                                    Order By t.UserName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.UserSecurityControl, .Name = t.UserName, .Description = ""}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.tblUserSecurities
                                    Select New DTO.vLookupList _
                                    With {.Control = t.UserSecurityControl, .Name = t.UserName, .Description = ""}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "CarrierActive" 'Modified By LVV on 12/11/19 for v-8.2.1.004 - Change LINQ code to call sp to solve 'maximum number of parameters' error with .Contains()
                Using db As New NGLMASCarrierDataContext(ConnectionString)
                    Try
                        vList = (
                            From t In db.spGetCarrierActiveLookupList(sortKey, ModDate, Me.Parameters.UserName)
                            Select New DTO.vLookupList With {.Control = t.CarrierControl, .Name = t.CarrierName, .Description = t.CarrierNumber.ToString}
                        ).ToArray()
                    Catch ex As SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "CompActive"
                Using db As New NGLMASCompDataContext(ConnectionString)
                    Try

                        Dim oSecureComp = From s In db.vUserAdminWithCompControlRefComps Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.Comps
                                    Where t.CompActive = True _
                                   And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.CompControl)) _
                                   And
                                   (ModDate.HasValue = False OrElse t.CompModDate > ModDate)
                                    Order By t.CompName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CompControl, .Name = t.CompName, .Description = t.CompNumber.ToString}).ToArray()
                            Case 2
                                vList = (
                                   From t In db.Comps
                                   Where t.CompActive = True _
                                  And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.CompControl)) _
                                  And
                                  (ModDate.HasValue = False OrElse t.CompModDate > ModDate)
                                   Order By t.CompNumber
                                   Select New DTO.vLookupList _
                                   With {.Control = t.CompControl, .Name = t.CompName, .Description = t.CompNumber.ToString}).ToArray()
                            Case Else
                                vList = (
                                   From t In db.Comps
                                   Where t.CompActive = True _
                                  And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.CompControl)) _
                                  And
                                  (ModDate.HasValue = False OrElse t.CompModDate > ModDate)
                                   Select New DTO.vLookupList _
                                   With {.Control = t.CompControl, .Name = t.CompName, .Description = t.CompNumber.ToString}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using

            Case "CompNEXTrack"
                Using db As New NGLMASCompDataContext(ConnectionString)
                    Try

                        Dim oSecureComp = From s In db.vUserAdminWithCompControlRefComps Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.Comps
                                    Where t.CompNEXTrack = True _
                                   And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.CompControl)) _
                                   And
                                   (ModDate.HasValue = False OrElse t.CompModDate > ModDate)
                                    Order By t.CompName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CompControl, .Name = t.CompName, .Description = t.CompNumber.ToString}).ToArray()
                            Case 2
                                vList = (
                                   From t In db.Comps
                                   Where t.CompNEXTrack = True _
                                  And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.CompControl)) _
                                  And
                                  (ModDate.HasValue = False OrElse t.CompModDate > ModDate)
                                   Order By t.CompNumber
                                   Select New DTO.vLookupList _
                                   With {.Control = t.CompControl, .Name = t.CompName, .Description = t.CompNumber.ToString}).ToArray()
                            Case Else
                                vList = (
                                   From t In db.Comps
                                   Where t.CompNEXTrack = True _
                                  And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.CompControl)) _
                                  And
                                  (ModDate.HasValue = False OrElse t.CompModDate > ModDate)
                                   Select New DTO.vLookupList _
                                   With {.Control = t.CompControl, .Name = t.CompName, .Description = t.CompNumber.ToString}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using

            Case "APAdjReason"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.vLookupAPAdjReasons
                                    Order By t.APAdjReasonCode
                                    Select New DTO.vLookupList _
                                    With {.Control = t.APAdjReasonControl, .Name = t.APAdjReasonCode, .Description = t.APAdjReasonDescription.ToString}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.vLookupAPAdjReasons
                                    Order By t.APAdjReasonDescription
                                    Select New DTO.vLookupList _
                                    With {.Control = t.APAdjReasonControl, .Name = t.APAdjReasonCode, .Description = t.APAdjReasonDescription.ToString}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.vLookupAPAdjReasons
                                    Select New DTO.vLookupList _
                                    With {.Control = t.APAdjReasonControl, .Name = t.APAdjReasonCode, .Description = t.APAdjReasonDescription.ToString}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using

            Case "ComCodes"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try

                        vList = (
                          From t In db.vLookUpComCodes
                          Select New DTO.vLookupList _
                          With {.Name = t.Code, .Description = t.CodeDescription}).ToArray()
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using

            Case "PayCodes"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try

                        vList = (
                          From t In db.vLookUpPayCodes
                          Select New DTO.vLookupList _
                          With {.Name = t.Code, .Description = t.CodeDescription}).ToArray()
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using

            Case "TranCodes"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try

                        vList = (
                          From t In db.vLookupTranCodes
                          Select New DTO.vLookupList _
                          With {.Name = t.Code, .Description = t.CodeDescription}).ToArray()
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "ChartOfAcounts"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try

                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.ChartOfAccounts
                                    Order By t.AcctNo
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ID, .Name = t.AcctDescription, .Description = t.AcctNo}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.ChartOfAccounts
                                    Order By t.AcctDescription
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ID, .Name = t.AcctDescription, .Description = t.AcctNo}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.ChartOfAccounts
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ID, .Name = t.AcctDescription, .Description = t.AcctNo}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "LoadStatusCodes"
                'Modified By LVV on 3/1/19 - Added Order By LoadStatusSequenceNo (Also added ListSortType "SeqNo" = 4 to 365 enum
                'Modified by RHR for v-7.0.6.103 on 3/15/2017
                '  we now sort by control nunber instead of code so that the
                '  No Issues Reported item show up in the list first.
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try

                        Select Case sortKey
                            Case 1
                                'Modified by RHR for v-7.0.6.103 on 3/15/2017
                                vList = (
                                    From t In db.LoadStatusCodes
                                    Order By t.LoadStatusControl
                                    Select New DTO.vLookupList _
                                    With {.Control = t.LoadStatusControl, .Name = t.LoadStatusDesc, .Description = t.LoadStatusCode}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.LoadStatusCodes
                                    Order By t.LoadStatusDesc
                                    Select New DTO.vLookupList _
                                    With {.Control = t.LoadStatusControl, .Name = t.LoadStatusDesc, .Description = t.LoadStatusCode}).ToArray()
                            Case 4 'Modified By LVV on 3/1/19 - Added Order By LoadStatusSequenceNo (Also added ListSortType "SeqNo" = 4 to 365 enum
                                vList = (
                                    From t In db.LoadStatusCodes
                                    Order By t.LoadStatusSequenceNo
                                    Select New DTO.vLookupList _
                                    With {.Control = t.LoadStatusControl, .Name = t.LoadStatusDesc, .Description = t.LoadStatusCode}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.LoadStatusCodes
                                    Select New DTO.vLookupList _
                                    With {.Control = t.LoadStatusControl, .Name = t.LoadStatusDesc, .Description = t.LoadStatusCode}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "NegativeRevenueReason"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try

                        vList = (
                            From t In db.NegativeRevenueReasons
                            Order By t.NegativeRevenueReason
                            Select New DTO.vLookupList _
                            With {.Control = t.NegativeRevenueCode, .Name = t.NegativeRevenueReason}).ToArray()

                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "ARCompany"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Dim oSecureComp = From s In db.vUserAdminWithCompControlRefVlookups Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.vLookUpARCompanies
                                    Where (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.CompControl))
                                    Order By t.CompName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CompControl, .Name = t.CompName, .Description = t.CompNumber.ToString}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.vLookUpARCompanies
                                    Where (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.CompControl))
                                    Order By t.CompNumber
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CompControl, .Name = t.CompName, .Description = t.CompNumber.ToString}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.vLookUpARCompanies
                                    Where (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.CompControl))
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CompControl, .Name = t.CompName, .Description = t.CompNumber.ToString}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "APCarrier"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Dim oSecureTariffs = From d In db.vSecureCarrierRefLookups Where d.UserAdminUserName = Me.Parameters.UserName Select d.CarrTarCarrierControl
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.vLookupAPCarriers
                                    Where (oSecureTariffs Is Nothing OrElse oSecureTariffs.Count = 0 OrElse oSecureTariffs.Contains(t.CarrierControl))
                                    Order By t.CarrierName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrierControl, .Name = t.CarrierName, .Description = t.CarrierNumber.ToString}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.vLookupAPCarriers
                                    Where (oSecureTariffs Is Nothing OrElse oSecureTariffs.Count = 0 OrElse oSecureTariffs.Contains(t.CarrierControl))
                                    Order By t.CarrierNumber
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrierControl, .Name = t.CarrierName, .Description = t.CarrierNumber.ToString}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.vLookupAPCarriers
                                    Where (oSecureTariffs Is Nothing OrElse oSecureTariffs.Count = 0 OrElse oSecureTariffs.Contains(t.CarrierControl))
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrierControl, .Name = t.CarrierName, .Description = t.CarrierNumber.ToString}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "APCompany"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Dim oSecureComp = From s In db.vUserAdminWithCompControlRefVlookups Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.vLookupAPCompanies
                                    Where (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.CompControl))
                                    Order By t.CompName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CompControl, .Name = t.CompName, .Description = t.CompNumber.ToString}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.vLookupAPCompanies
                                    Where (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.CompControl))
                                    Order By t.CompNumber
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CompControl, .Name = t.CompName, .Description = t.CompNumber.ToString}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.vLookupAPCompanies
                                    Where (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.CompControl))
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CompControl, .Name = t.CompName, .Description = t.CompNumber.ToString}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "APActiveCarrier"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Dim oSecureTariffs = From d In db.vSecureCarrierRefLookups Where d.UserAdminUserName = Me.Parameters.UserName Select d.CarrTarCarrierControl
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.vLookupActiveAPCarriers
                                    Where (oSecureTariffs Is Nothing OrElse oSecureTariffs.Count = 0 OrElse oSecureTariffs.Contains(t.CarrierControl))
                                    Order By t.CarrierName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrierControl, .Name = t.CarrierName, .Description = t.CarrierNumber.ToString}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.vLookupActiveAPCarriers
                                    Where (oSecureTariffs Is Nothing OrElse oSecureTariffs.Count = 0 OrElse oSecureTariffs.Contains(t.CarrierControl))
                                    Order By t.CarrierNumber
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrierControl, .Name = t.CarrierName, .Description = t.CarrierNumber.ToString}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.vLookupActiveAPCarriers
                                    Where (oSecureTariffs Is Nothing OrElse oSecureTariffs.Count = 0 OrElse oSecureTariffs.Contains(t.CarrierControl))
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrierControl, .Name = t.CarrierName, .Description = t.CarrierNumber.ToString}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "APCarrierPaid"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Dim oSecureTariffs = From d In db.vSecureCarrierRefLookups Where d.UserAdminUserName = Me.Parameters.UserName Select d.CarrTarCarrierControl

                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.vLookupAPCarrierPaids
                                    Where (oSecureTariffs Is Nothing OrElse oSecureTariffs.Count = 0 OrElse oSecureTariffs.Contains(t.CarrierControl))
                                    Order By t.CarrierName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrierControl, .Name = t.CarrierName, .Description = t.CarrierNumber.ToString}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.vLookupAPCarrierPaids
                                    Where (oSecureTariffs Is Nothing OrElse oSecureTariffs.Count = 0 OrElse oSecureTariffs.Contains(t.CarrierControl))
                                    Order By t.CarrierNumber
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrierControl, .Name = t.CarrierName, .Description = t.CarrierNumber.ToString}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.vLookupAPCarrierPaids
                                    Where (oSecureTariffs Is Nothing OrElse oSecureTariffs.Count = 0 OrElse oSecureTariffs.Contains(t.CarrierControl))
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrierControl, .Name = t.CarrierName, .Description = t.CarrierNumber.ToString}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "APCarrierAmtPaid"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Dim oSecureTariffs = From d In db.vSecureCarrierRefLookups Where d.UserAdminUserName = Me.Parameters.UserName Select d.CarrTarCarrierControl
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.vLookupAPCarrierAmtPaids
                                    Where (oSecureTariffs Is Nothing OrElse oSecureTariffs.Count = 0 OrElse oSecureTariffs.Contains(t.CarrierControl))
                                    Order By t.CarrierName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrierControl, .Name = t.CarrierName, .Description = t.CarrierNumber.ToString}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.vLookupAPCarrierAmtPaids
                                    Where (oSecureTariffs Is Nothing OrElse oSecureTariffs.Count = 0 OrElse oSecureTariffs.Contains(t.CarrierControl))
                                    Order By t.CarrierNumber
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrierControl, .Name = t.CarrierName, .Description = t.CarrierNumber.ToString}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.vLookupAPCarrierAmtPaids
                                    Where (oSecureTariffs Is Nothing OrElse oSecureTariffs.Count = 0 OrElse oSecureTariffs.Contains(t.CarrierControl))
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrierControl, .Name = t.CarrierName, .Description = t.CarrierNumber.ToString}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "tblBracketType"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.vLookuptblBracketTypes
                                    Order By t.BTName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.BTControl, .Name = t.BTName, .Description = t.BTDescription}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.vLookuptblBracketTypes
                                    Order By t.BTDescription
                                    Select New DTO.vLookupList _
                                    With {.Control = t.BTControl, .Name = t.BTName, .Description = t.BTDescription}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.vLookuptblBracketTypes
                                    Select New DTO.vLookupList _
                                    With {.Control = t.BTControl, .Name = t.BTName, .Description = t.BTDescription}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "AccessorialVariableCodes" ' Modified By RHR for v-8.3.0.002 on 12/1/2020 added ModDate Logic 
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.tblAccessorialVariableCodes
                                    Order By t.AccessorialVariableCodesName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.AccessorialVariableCodesControl, .Name = t.AccessorialVariableCodesName, .Description = t.AccessorialVariableCodesDescription}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.tblAccessorialVariableCodes
                                    Order By t.AccessorialVariableCodesDescription
                                    Select New DTO.vLookupList _
                                    With {.Control = t.AccessorialVariableCodesControl, .Name = t.AccessorialVariableCodesName, .Description = t.AccessorialVariableCodesDescription}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.tblAccessorialVariableCodes
                                    Select New DTO.vLookupList _
                                    With {.Control = t.AccessorialVariableCodesControl, .Name = t.AccessorialVariableCodesName, .Description = t.AccessorialVariableCodesDescription}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "LaneNonRestrictedCarriers"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        If Criteria Is Nothing Then Return Nothing
                        Dim Control As Integer
                        If (Not Integer.TryParse(Criteria.ToString(), Control)) Then Throw New InvalidOperationException("Cannot get the non restricted carriers for the lane because the lane control number criteria is not valid.")

                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.spGetLaneNonRestrictedCarriers(Criteria)
                                    Order By t.CarrierName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrierControl, .Name = t.CarrierName, .Description = t.CarrierNumber}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.spGetLaneNonRestrictedCarriers(Criteria)
                                    Order By t.CarrierNumber
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrierControl, .Name = t.CarrierName, .Description = t.CarrierNumber}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.spGetLaneNonRestrictedCarriers(Criteria)
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrierControl, .Name = t.CarrierName, .Description = t.CarrierNumber}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "LaneRestrictedCarriers"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        If Criteria Is Nothing Then Return Nothing
                        Dim Control As Integer
                        If (Not Integer.TryParse(Criteria.ToString(), Control)) Then Throw New InvalidOperationException("Cannot get the restricted carriers for the lane because the lane control number criteria is not valid.")

                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.spGetLaneCarrierRestrictions(Criteria)
                                    Order By t.CarrierName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.LLTCControl, .Name = t.CarrierName, .Description = t.CarrierNumber}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.spGetLaneCarrierRestrictions(Criteria)
                                    Order By t.CarrierNumber
                                    Select New DTO.vLookupList _
                                    With {.Control = t.LLTCControl, .Name = t.CarrierName, .Description = t.CarrierNumber}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.spGetLaneCarrierRestrictions(Criteria)
                                    Select New DTO.vLookupList _
                                    With {.Control = t.LLTCControl, .Name = t.CarrierName, .Description = t.CarrierNumber}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "CarrAdHoc"
                Using db As New NGLMASCarrierDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.CarrAdHocs
                                    Order By t.CarrAdHocName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrAdHocControl, .Name = t.CarrAdHocName, .Description = t.CarrAdHocNumber.ToString}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.CarrAdHocs
                                    Order By t.CarrAdHocNumber
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrAdHocControl, .Name = t.CarrAdHocName, .Description = t.CarrAdHocNumber.ToString}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.CarrAdHocs
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrAdHocControl, .Name = t.CarrAdHocName, .Description = t.CarrAdHocNumber.ToString}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "LaneActive"
                Using db As New NGLMASLaneDataContext(ConnectionString)
                    Try
                        Dim oSecureComp = From s In db.vUserAdminWithCompControlRefLanes Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.Lanes
                                    Where (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.LaneCompControl)) _
                                    And (t.LaneActive.HasValue = False OrElse t.LaneActive = True) _
                                    And
                                    (ModDate.HasValue = False OrElse t.LaneModDate > ModDate)
                                    Order By t.LaneName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.LaneControl, .Name = t.LaneName, .Description = t.LaneNumber.ToString}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.Lanes
                                    Where (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.LaneCompControl)) _
                                    And (t.LaneActive.HasValue = False OrElse t.LaneActive = True) _
                                    And
                                    (ModDate.HasValue = False OrElse t.LaneModDate > ModDate)
                                    Order By t.LaneNumber
                                    Select New DTO.vLookupList _
                                    With {.Control = t.LaneControl, .Name = t.LaneName, .Description = t.LaneNumber.ToString}).ToArray()
                                'With {.Control = t.LaneControl, .Name = t.LaneName, .Description = t.LaneNumber.ToString}).Take(12000).ToArray()
                            Case Else
                                vList = (
                                    From t In db.Lanes
                                    Where (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.LaneCompControl)) _
                                    And (t.LaneActive.HasValue = False OrElse t.LaneActive = True) _
                                    And
                                    (ModDate.HasValue = False OrElse t.LaneModDate > ModDate)
                                    Select New DTO.vLookupList _
                                    With {.Control = t.LaneControl, .Name = t.LaneName, .Description = t.LaneNumber.ToString}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "tblParCategories"
                Using db As New NGLMASSYSDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.tblParCategories
                                    Order By t.ParCatName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ParCatControl, .Name = t.ParCatName, .Description = t.ParCatDescription.ToString}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.tblParCategories
                                    Order By t.ParCatDescription
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ParCatControl, .Name = t.ParCatName, .Description = t.ParCatDescription.ToString}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.tblParCategories
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ParCatControl, .Name = t.ParCatName, .Description = t.ParCatDescription.ToString}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "TariffTempType"

                Try
                    Dim ListItemList As New List(Of DTO.vLookupList)

                    ListItemList.Add(New DTO.vLookupList(0, "Any", "Any"))
                    ListItemList.Add(New DTO.vLookupList(1, "Dry", "Dry"))
                    ListItemList.Add(New DTO.vLookupList(2, "Frozen", "Frozen"))
                    ListItemList.Add(New DTO.vLookupList(3, "Cooler", "Cooler"))
                    Select Case sortKey
                        Case 1


                            vList = (
                                From t In ListItemList
                                Order By t.Name
                                Select New DTO.vLookupList _
                                With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                        Case 2
                            vList = (
                                 From t In ListItemList
                                 Order By t.Description
                                 Select New DTO.vLookupList _
                                 With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                        Case Else
                            vList = (
                                 From t In ListItemList
                                 Select New DTO.vLookupList _
                                 With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                    End Select
                Catch ex As System.Data.SqlClient.SqlException
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                Catch ex As InvalidOperationException
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                Catch ex As Exception
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                End Try
            Case "TariffType"
                Try
                    Dim ListItemList As New List(Of DTO.vLookupList)

                    ListItemList.Add(New DTO.vLookupList(0, "  - I -", "I"))
                    ListItemList.Add(New DTO.vLookupList(1, "  - O -", "O"))
                    Select Case sortKey
                        Case 1


                            vList = (
                                From t In ListItemList
                                Order By t.Name
                                Select New DTO.vLookupList _
                                With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                        Case 2
                            vList = (
                                 From t In ListItemList
                                 Order By t.Description
                                 Select New DTO.vLookupList _
                                 With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                        Case Else
                            vList = (
                                 From t In ListItemList
                                 Select New DTO.vLookupList _
                                 With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                    End Select
                Catch ex As System.Data.SqlClient.SqlException
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                Catch ex As InvalidOperationException
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                Catch ex As Exception
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                End Try
            Case "ImportFileType"
                Try
                    Dim ListItemList As New List(Of DTO.vLookupList)

                    ListItemList.Add(New DTO.vLookupList(0, "gcImportCarrier", "Carrier"))
                    ListItemList.Add(New DTO.vLookupList(1, "gcImportLane", "Lane"))
                    ListItemList.Add(New DTO.vLookupList(2, "gcImportComp", "Company"))
                    ListItemList.Add(New DTO.vLookupList(3, "gcImportPayables", "Payables"))
                    ListItemList.Add(New DTO.vLookupList(4, "gcImportSchedule", "Schedule"))
                    ListItemList.Add(New DTO.vLookupList(5, "gcimportFrtBill", "Freight Bill"))
                    ListItemList.Add(New DTO.vLookupList(6, "gcimportBook", "Book"))
                    Select Case sortKey
                        Case 1
                            vList = (
                                From t In ListItemList
                                Order By t.Name
                                Select New DTO.vLookupList _
                                With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                        Case 2
                            vList = (
                                 From t In ListItemList
                                 Order By t.Description
                                 Select New DTO.vLookupList _
                                 With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                        Case Else
                            vList = (
                                 From t In ListItemList
                                 Select New DTO.vLookupList _
                                 With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                    End Select
                Catch ex As System.Data.SqlClient.SqlException
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                Catch ex As InvalidOperationException
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                Catch ex As Exception
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                End Try
            Case "tblRouteTypes"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.tblRouteTypes
                                    Order By t.RouteTypeName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.RouteTypeControl, .Name = t.RouteTypeName, .Description = t.RouteTypeDesc}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.tblRouteTypes
                                    Order By t.RouteTypeDesc
                                    Select New DTO.vLookupList _
                                    With {.Control = t.RouteTypeControl, .Name = t.RouteTypeName, .Description = t.RouteTypeDesc}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.tblRouteTypes
                                    Select New DTO.vLookupList _
                                    With {.Control = t.RouteTypeControl, .Name = t.RouteTypeName, .Description = t.RouteTypeDesc}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "tblStaticRoutes"
                'Criteria
                Using db As New NGLMASLaneDataContext(ConnectionString)
                    Try
                        Dim lookupList As New List(Of DTO.vLookupList)
                        Select Case sortKey
                            Case 1
                                lookupList = (
                                    From t In db.tblStaticRoutes
                                    Order By t.StaticRouteName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.StaticRouteControl, .Name = t.StaticRouteName, .Description = t.StaticRouteNumber}).ToList

                            Case 2
                                lookupList = (
                                    From t In db.tblStaticRoutes
                                    Order By t.StaticRouteCompControl
                                    Select New DTO.vLookupList _
                                    With {.Control = t.StaticRouteControl, .Name = t.StaticRouteName, .Description = t.StaticRouteNumber}).ToList
                            Case Else
                                lookupList = (
                                    From t In db.tblStaticRoutes
                                    Select New DTO.vLookupList _
                                    With {.Control = t.StaticRouteControl, .Name = t.StaticRouteName, .Description = t.StaticRouteNumber}).ToList
                        End Select
                        Dim itemDefault As New DTO.vLookupList(0, "NONE", "0")
                        If lookupList Is Nothing Then lookupList = New List(Of DTO.vLookupList)
                        lookupList.Add(itemDefault)
                        vList = lookupList.ToArray()

                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "CarrierEquipment"
                Dim CarrierControl As Integer = 0
                If Not Object.ReferenceEquals(CarrierControl.GetType, Criteria.GetType) Then Return Nothing
                CarrierControl = Criteria
                Using db As New NGLMASCarrierDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.CarrierTrucks
                                    Where t.CarrierTruckCarrierControl = CarrierControl
                                    Order By t.CarrierTruckEquipment
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrierTruckControl, .Name = t.CarrierTruckEquipment, .Description = t.CarrierTruckDescription}).ToArray()

                            Case 2
                                vList = (
                                    From t In db.CarrierTrucks
                                    Where t.CarrierTruckCarrierControl = CarrierControl
                                    Order By t.CarrierTruckDescription
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrierTruckControl, .Name = t.CarrierTruckEquipment, .Description = t.CarrierTruckDescription}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.CarrierTrucks
                                    Where t.CarrierTruckCarrierControl = CarrierControl
                                    Order By t.CarrierTruckControl
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrierTruckControl, .Name = t.CarrierTruckEquipment, .Description = t.CarrierTruckDescription}).ToArray()
                        End Select

                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        ' Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                        'its ok if there isnt any equipment
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using

            Case "CapacityPreference"
                Try
                    Dim ListItemList As New List(Of DTO.vLookupList)

                    ListItemList.Add(New DTO.vLookupList(0, "Sequence", "Routing Sequence Number"))
                    ListItemList.Add(New DTO.vLookupList(1, "Quantity", "Typically Cases"))
                    ListItemList.Add(New DTO.vLookupList(2, "Weight", "Weight"))
                    ListItemList.Add(New DTO.vLookupList(3, "Pallets", "Pallets"))
                    ListItemList.Add(New DTO.vLookupList(4, "Cubes", "Cubes"))
                    Select Case sortKey
                        Case 1
                            vList = (
                                From t In ListItemList
                                Order By t.Name
                                Select New DTO.vLookupList _
                                With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                        Case 2
                            vList = (
                                 From t In ListItemList
                                 Order By t.Description
                                 Select New DTO.vLookupList _
                                 With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                        Case Else
                            vList = (
                                 From t In ListItemList
                                 Select New DTO.vLookupList _
                                 With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                    End Select
                Catch ex As System.Data.SqlClient.SqlException
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                Catch ex As InvalidOperationException
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                Catch ex As Exception
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                End Try

            Case "ActionType"
                Try
                    Dim ListItemList As New List(Of DTO.vLookupList)

                    ListItemList.Add(New DTO.vLookupList(0, "stored procedure", "Execute a Stored Procedure"))
                    ListItemList.Add(New DTO.vLookupList(1, "Service", "Used to Execute a Service like WCF"))
                    ListItemList.Add(New DTO.vLookupList(2, "Executable", "Used to Launch an Executable Application"))
                    Select Case sortKey
                        Case 1


                            vList = (
                                From t In ListItemList
                                Order By t.Name
                                Select New DTO.vLookupList _
                                With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                        Case 2
                            vList = (
                                 From t In ListItemList
                                 Order By t.Description
                                 Select New DTO.vLookupList _
                                 With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                        Case Else
                            vList = (
                                 From t In ListItemList
                                 Select New DTO.vLookupList _
                                 With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                    End Select
                Catch ex As System.Data.SqlClient.SqlException
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                Catch ex As InvalidOperationException
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                Catch ex As Exception
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                End Try

            Case "tblAttribute"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Dim AttributeType As Integer = 0
                        If Not Criteria Is Nothing Then
                            If Not Object.ReferenceEquals(AttributeType.GetType, Criteria.GetType) Then Return Nothing
                            AttributeType = Criteria
                            Select Case sortKey
                                Case 1
                                    vList = (
                                        From t In db.vLookuptblAttributes
                                        Where t.AttributeAttributeTypeControl = AttributeType
                                        Order By t.Name
                                        Select New DTO.vLookupList _
                                        With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                                Case 2
                                    vList = (
                                        From t In db.vLookuptblAttributes
                                        Where t.AttributeAttributeTypeControl = AttributeType
                                        Order By t.Description
                                        Select New DTO.vLookupList _
                                        With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                                Case Else
                                    vList = (
                                        From t In db.vLookuptblAttributes
                                        Where t.AttributeAttributeTypeControl = AttributeType
                                        Order By t.Control
                                        Select New DTO.vLookupList _
                                        With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                            End Select
                        Else
                            'Get all items
                            Select Case sortKey
                                Case 1
                                    vList = (
                                        From t In db.vLookuptblAttributes
                                        Order By t.Name
                                        Select New DTO.vLookupList _
                                        With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                                Case 2
                                    vList = (
                                        From t In db.vLookuptblAttributes
                                        Order By t.Description
                                        Select New DTO.vLookupList _
                                        With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                                Case Else
                                    vList = (
                                        From t In db.vLookuptblAttributes
                                        Order By t.Control
                                        Select New DTO.vLookupList _
                                        With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                            End Select

                        End If


                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        ' Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                        'its ok if there isnt any equipment
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using

            Case "tblAttributeType"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try

                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.tblAttributeTypes
                                    Order By t.AttributeTypeName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.AttributeTypeControl, .Name = t.AttributeTypeName, .Description = t.AttributeTypeDescription}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.tblAttributeTypes
                                    Order By t.AttributeTypeDescription
                                    Select New DTO.vLookupList _
                                    With {.Control = t.AttributeTypeControl, .Name = t.AttributeTypeName, .Description = t.AttributeTypeDescription}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.tblAttributeTypes
                                    Order By t.AttributeTypeControl
                                    Select New DTO.vLookupList _
                                    With {.Control = t.AttributeTypeControl, .Name = t.AttributeTypeName, .Description = t.AttributeTypeDescription}).ToArray()
                        End Select

                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        ' Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                        'its ok if there isnt any equipment
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using

            Case "tblAction"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try

                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.tblActions
                                    Order By t.ActionName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ActionControl, .Name = t.ActionName, .Description = t.ActionDescription}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.tblActions
                                    Order By t.ActionDescription
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ActionControl, .Name = t.ActionName, .Description = t.ActionDescription}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.tblActions
                                    Order By t.ActionControl
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ActionControl, .Name = t.ActionName, .Description = t.ActionDescription}).ToArray()
                        End Select


                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        ' Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                        'its ok if there isnt any equipment
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "LaneCrossDock"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Dim blnFlag As Boolean
                        If Criteria Is Nothing Then
                            'return all
                            Select Case sortKey
                                Case 1
                                    vList = (
                                        From t In db.spGetLaneCrossDockList(Nothing, Nothing)
                                        Order By t.LaneName
                                        Select New DTO.vLookupList _
                                        With {.Control = t.LaneControl, .Name = t.LaneName, .Description = t.LaneNumber}).ToArray()
                                Case 2
                                    vList = (
                                        From t In db.spGetLaneCrossDockList(Nothing, Nothing)
                                        Order By t.LaneNumber
                                        Select New DTO.vLookupList _
                                        With {.Control = t.LaneControl, .Name = t.LaneName, .Description = t.LaneNumber}).ToArray()
                                Case Else
                                    vList = (
                                        From t In db.spGetLaneCrossDockList(Nothing, Nothing)
                                        Select New DTO.vLookupList _
                                        With {.Control = t.LaneControl, .Name = t.LaneName, .Description = t.LaneNumber}).ToArray()
                            End Select

                        ElseIf Boolean.TryParse(Criteria.ToString, blnFlag) Then
                            'return only cross dock facilities
                            Select Case sortKey
                                Case 1
                                    vList = (
                                        From t In db.spGetLaneCrossDockList(Nothing, blnFlag)
                                        Order By t.LaneName
                                        Select New DTO.vLookupList _
                                        With {.Control = t.LaneControl, .Name = t.LaneName, .Description = t.LaneNumber}).ToArray()
                                Case 2
                                    vList = (
                                        From t In db.spGetLaneCrossDockList(Nothing, blnFlag)
                                        Order By t.LaneNumber
                                        Select New DTO.vLookupList _
                                        With {.Control = t.LaneControl, .Name = t.LaneName, .Description = t.LaneNumber}).ToArray()
                                Case Else
                                    vList = (
                                        From t In db.spGetLaneCrossDockList(Nothing, blnFlag)
                                        Select New DTO.vLookupList _
                                        With {.Control = t.LaneControl, .Name = t.LaneName, .Description = t.LaneNumber}).ToArray()
                            End Select

                        Else
                            'filter my lane number master
                            Select Case sortKey
                                Case 1
                                    vList = (
                                        From t In db.spGetLaneCrossDockList(Criteria.ToString, False)
                                        Order By t.LaneName
                                        Select New DTO.vLookupList _
                                        With {.Control = t.LaneControl, .Name = t.LaneName, .Description = t.LaneNumber}).ToArray()
                                Case 2
                                    vList = (
                                        From t In db.spGetLaneCrossDockList(Criteria.ToString, False)
                                        Order By t.LaneNumber
                                        Select New DTO.vLookupList _
                                        With {.Control = t.LaneControl, .Name = t.LaneName, .Description = t.LaneNumber}).ToArray()
                                Case Else
                                    vList = (
                                        From t In db.spGetLaneCrossDockList(Criteria.ToString, False)
                                        Select New DTO.vLookupList _
                                        With {.Control = t.LaneControl, .Name = t.LaneName, .Description = t.LaneNumber}).ToArray()
                            End Select

                        End If

                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "ColorCodeType"
                Try
                    Dim ListItemList As New List(Of DTO.vLookupList)

                    ListItemList.Add(New DTO.vLookupList(0, "Appt Status", "Appointment Calendar Color Code Type"))
                    ListItemList.Add(New DTO.vLookupList(1, "Appt Type", "Order List Catagory Color Code Type"))
                    Select Case sortKey
                        Case 1

                            vList = (
                                From t In ListItemList
                                Order By t.Name
                                Select New DTO.vLookupList _
                                With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                        Case 2
                            vList = (
                                 From t In ListItemList
                                 Order By t.Description
                                 Select New DTO.vLookupList _
                                 With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                        Case Else
                            vList = (
                                 From t In ListItemList
                                 Select New DTO.vLookupList _
                                 With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                    End Select
                Catch ex As System.Data.SqlClient.SqlException
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                Catch ex As InvalidOperationException
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                Catch ex As Exception
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                End Try
            Case "ApptStatusColorCodeKey" 'Modifed By LVV on 6/28/18 for v-8.3 TMS365 Scheduler
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.tblAMSApptStatus
                                    Order By t.AMSApptStatusName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.AMSApptStatusControl, .Name = t.AMSApptStatusName, .Description = t.AMSApptStatusDesc}).ToArray()
                            Case 2
                                vList = (
                                     From t In db.tblAMSApptStatus
                                     Order By t.AMSApptStatusDesc
                                     Select New DTO.vLookupList _
                                     With {.Control = t.AMSApptStatusControl, .Name = t.AMSApptStatusName, .Description = t.AMSApptStatusDesc}).ToArray()
                            Case Else
                                vList = (
                                     From t In db.tblAMSApptStatus
                                     Select New DTO.vLookupList _
                                     With {.Control = t.AMSApptStatusControl, .Name = t.AMSApptStatusName, .Description = t.AMSApptStatusDesc}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "ApptTypeColorCodeKey" 'Modifed By LVV on 6/28/18 for v-8.3 TMS365 Scheduler
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.tblAMSApptTypes
                                    Order By t.AMSApptTypeName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.AMSApptTypeControl, .Name = t.AMSApptTypeName, .Description = t.AMSApptTypeDesc}).ToArray()
                            Case 2
                                vList = (
                                     From t In db.tblAMSApptTypes
                                     Order By t.AMSApptTypeDesc
                                     Select New DTO.vLookupList _
                                     With {.Control = t.AMSApptTypeControl, .Name = t.AMSApptTypeName, .Description = t.AMSApptTypeDesc}).ToArray()
                            Case Else
                                vList = (
                                     From t In db.tblAMSApptTypes
                                     Select New DTO.vLookupList _
                                     With {.Control = t.AMSApptTypeControl, .Name = t.AMSApptTypeName, .Description = t.AMSApptTypeDesc}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "tblPointType"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Dim ListItemList As New List(Of DTO.vLookupList)
                        Select Case sortKey
                            Case 1


                                ListItemList = (
                                    From t In db.tblPointTypes
                                    Order By t.PointTypeName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.PointTypeControl, .Name = t.PointTypeName, .Description = t.PointTypeDesc}).ToList()
                            Case 2
                                ListItemList = (
                                     From t In db.tblPointTypes
                                     Order By t.PointTypeDesc
                                     Select New DTO.vLookupList _
                                     With {.Control = t.PointTypeControl, .Name = t.PointTypeName, .Description = t.PointTypeDesc}).ToList()
                            Case Else
                                ListItemList = (
                                     From t In db.tblPointTypes
                                     Select New DTO.vLookupList _
                                     With {.Control = t.PointTypeControl, .Name = t.PointTypeName, .Description = t.PointTypeDesc}).ToList()
                        End Select
                        If Not ListItemList Is Nothing Then
                            ListItemList.Add(New DTO.vLookupList(0, "Any", "Default value if nothing is selected"))
                            vList = ListItemList.ToArray()
                        End If

                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "tblClassType"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Dim ListItemList As New List(Of DTO.vLookupList)
                        Select Case sortKey
                            Case 1
                                ListItemList = (
                                    From t In db.tblClassTypes
                                    Order By t.ClassTypeName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ClassTypeControl, .Name = t.ClassTypeName, .Description = t.ClassTypeDesc}).ToList()
                            Case 2
                                ListItemList = (
                                     From t In db.tblClassTypes
                                     Order By t.ClassTypeDesc
                                     Select New DTO.vLookupList _
                                     With {.Control = t.ClassTypeControl, .Name = t.ClassTypeName, .Description = t.ClassTypeDesc}).ToList()
                            Case Else
                                ListItemList = (
                                     From t In db.tblClassTypes
                                     Select New DTO.vLookupList _
                                     With {.Control = t.ClassTypeControl, .Name = t.ClassTypeName, .Description = t.ClassTypeDesc}).ToList()
                        End Select
                        If Not ListItemList Is Nothing Then
                            ListItemList.Add(New DTO.vLookupList(0, "None", "Default value if nothing is selected"))
                            vList = ListItemList.ToArray()
                        End If
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "tblClassTypeAnyAll"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Dim ListItemList As New List(Of DTO.vLookupList)
                        Select Case sortKey
                            Case 1
                                ListItemList = (
                                    From t In db.tblClassTypes
                                    Order By t.ClassTypeName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ClassTypeControl, .Name = t.ClassTypeName, .Description = t.ClassTypeDesc}).ToList()
                            Case 2
                                ListItemList = (
                                     From t In db.tblClassTypes
                                     Order By t.ClassTypeDesc
                                     Select New DTO.vLookupList _
                                     With {.Control = t.ClassTypeControl, .Name = t.ClassTypeName, .Description = t.ClassTypeDesc}).ToList()
                            Case Else
                                ListItemList = (
                                     From t In db.tblClassTypes
                                     Select New DTO.vLookupList _
                                     With {.Control = t.ClassTypeControl, .Name = t.ClassTypeName, .Description = t.ClassTypeDesc}).ToList()
                        End Select
                        If Not ListItemList Is Nothing Then
                            ListItemList.Add(New DTO.vLookupList(0, "Any/All", "Default value if nothing is selected"))
                            vList = ListItemList.ToArray()
                        End If
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "tblModeType"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Dim ListItemList As New List(Of DTO.vLookupList)
                        Select Case sortKey
                            Case 1
                                ListItemList = (
                                    From t In db.tblModeTypes
                                    Order By t.ModeTypeName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ModeTypeControl, .Name = t.ModeTypeName, .Description = t.ModeTypeDesc}).ToList()
                            Case 2
                                ListItemList = (
                                     From t In db.tblModeTypes
                                     Order By t.ModeTypeDesc
                                     Select New DTO.vLookupList _
                                     With {.Control = t.ModeTypeControl, .Name = t.ModeTypeName, .Description = t.ModeTypeDesc}).ToList()
                            Case Else
                                ListItemList = (
                                     From t In db.tblModeTypes
                                     Order By t.ModeTypeName
                                     Select New DTO.vLookupList _
                                     With {.Control = t.ModeTypeControl, .Name = t.ModeTypeName, .Description = t.ModeTypeDesc}).ToList()
                        End Select
                        If sortKey > 2 Then
                            If Not ListItemList Is Nothing Then
                                ListItemList.Add(New DTO.vLookupList(0, "Any", "Default value if nothing is selected"))
                            End If
                        End If
                        vList = ListItemList.ToArray()
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "tblTariffType"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Dim ListItemList As New List(Of DTO.vLookupList)
                        Select Case sortKey
                            Case 1
                                ListItemList = (
                                    From t In db.tblTariffTypes
                                    Order By t.TariffTypeName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.TariffTypeControl, .Name = t.TariffTypeName, .Description = t.TariffTypeDesc}).ToList()
                            Case 2
                                ListItemList = (
                                     From t In db.tblTariffTypes
                                     Order By t.TariffTypeDesc
                                     Select New DTO.vLookupList _
                                     With {.Control = t.TariffTypeControl, .Name = t.TariffTypeName, .Description = t.TariffTypeDesc}).ToList()
                            Case Else
                                ListItemList = (
                                     From t In db.tblTariffTypes
                                     Select New DTO.vLookupList _
                                     With {.Control = t.TariffTypeControl, .Name = t.TariffTypeName, .Description = t.TariffTypeDesc}).ToList()
                        End Select
                        If Not ListItemList Is Nothing Then
                            ListItemList.Add(New DTO.vLookupList(0, "Any", "Default value if nothing is selected"))
                            vList = ListItemList.ToArray()
                        End If
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "tblTarAgent"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Dim ListItemList As New List(Of DTO.vLookupList)
                        Select Case sortKey
                            Case 1


                                ListItemList = (
                                    From t In db.tblTarAgents
                                    Order By t.TarAgentName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.TarAgentControl, .Name = t.TarAgentName, .Description = t.TarAgentDesc}).ToList()
                            Case 2
                                ListItemList = (
                                     From t In db.tblTarAgents
                                     Order By t.TarAgentDesc
                                     Select New DTO.vLookupList _
                                     With {.Control = t.TarAgentControl, .Name = t.TarAgentName, .Description = t.TarAgentDesc}).ToList()
                            Case Else
                                ListItemList = (
                                     From t In db.tblTarAgents
                                     Select New DTO.vLookupList _
                                     With {.Control = t.TarAgentControl, .Name = t.TarAgentName, .Description = t.TarAgentDesc}).ToList()
                        End Select
                        If Not ListItemList Is Nothing Then
                            ListItemList.Add(New DTO.vLookupList(0, "Any", "Default value if nothing is selected"))
                            vList = ListItemList.ToArray()
                        End If
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "tblTarRateType"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Dim ListItemList As New List(Of DTO.vLookupList)
                        Select Case sortKey
                            Case 1
                                ListItemList = (
                                    From t In db.tblTarRateTypes
                                    Order By t.TarRateTypeName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.TarRateTypeControl, .Name = t.TarRateTypeName, .Description = t.TarRateTypeDesc}).ToList()
                            Case 2
                                ListItemList = (
                                     From t In db.tblTarRateTypes
                                     Order By t.TarRateTypeDesc
                                     Select New DTO.vLookupList _
                                     With {.Control = t.TarRateTypeControl, .Name = t.TarRateTypeName, .Description = t.TarRateTypeDesc}).ToList()
                            Case Else
                                ListItemList = (
                                     From t In db.tblTarRateTypes
                                     Select New DTO.vLookupList _
                                     With {.Control = t.TarRateTypeControl, .Name = t.TarRateTypeName, .Description = t.TarRateTypeDesc}).ToList()
                        End Select
                        If Not ListItemList Is Nothing Then
                            ListItemList.Add(New DTO.vLookupList(0, "Any", "Default value if nothing is selected"))
                            vList = ListItemList.ToArray()
                        End If
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "tblTarBracketType"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Dim ListItemList As New List(Of DTO.vLookupList)
                        Select Case sortKey
                            Case 1
                                ListItemList = (
                                    From t In db.tblTarBracketTypes
                                    Order By t.TarBracketTypeName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.TarBracketTypeControl, .Name = t.TarBracketTypeName, .Description = t.TarBracketTypeDesc}).ToList()
                            Case 2
                                ListItemList = (
                                     From t In db.tblTarBracketTypes
                                     Order By t.TarBracketTypeDesc
                                     Select New DTO.vLookupList _
                                     With {.Control = t.TarBracketTypeControl, .Name = t.TarBracketTypeName, .Description = t.TarBracketTypeDesc}).ToList()
                            Case Else
                                ListItemList = (
                                     From t In db.tblTarBracketTypes
                                     Select New DTO.vLookupList _
                                     With {.Control = t.TarBracketTypeControl, .Name = t.TarBracketTypeName, .Description = t.TarBracketTypeDesc}).ToList()
                        End Select
                        If Not ListItemList Is Nothing Then
                            ListItemList.Add(New DTO.vLookupList(0, "None", "Default value if nothing is selected"))
                            vList = ListItemList.ToArray()
                        End If
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "TariffShipper"
                Using db As New NGLMASCompDataContext(ConnectionString)
                    Try

                        Dim oSecureComp = From s In db.vUserAdminWithCompControlRefComps Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.Comps
                                    Where t.CompActive = True _
                                   And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.CompControl))
                                    Order By t.CompName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CompControl, .Name = t.CompName, .Description = t.CompStreetAddress1 & " " & t.CompStreetCity & " " & t.CompStreetState & " " & t.CompStreetCountry}).ToArray()
                            Case 2
                                vList = (
                                   From t In db.Comps
                                   Where t.CompActive = True _
                                  And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.CompControl))
                                   Order By t.CompStreetCountry, t.CompStreetState, t.CompStreetCity, t.CompStreetAddress1
                                   Select New DTO.vLookupList _
                                   With {.Control = t.CompControl, .Name = t.CompName, .Description = t.CompStreetAddress1 & " " & t.CompStreetCity & " " & t.CompStreetState & " " & t.CompStreetCountry}).ToArray()
                            Case Else
                                vList = (
                                   From t In db.Comps
                                   Where t.CompActive = True _
                                  And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.CompControl))
                                   Select New DTO.vLookupList _
                                   With {.Control = t.CompControl, .Name = t.CompName, .Description = t.CompStreetAddress1 & " " & t.CompStreetCity & " " & t.CompStreetState & " " & t.CompStreetCountry}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "AccessorialFeeCalcType"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.tblAccessorialFeeCalcTypes
                                    Order By t.AccessorialFeeCalcTypeName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.AccessorialFeeCalcTypeControl, .Name = t.AccessorialFeeCalcTypeName, .Description = t.AccessorialFeeCalcTypeDesc}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.tblAccessorialFeeCalcTypes
                                    Order By t.AccessorialFeeCalcTypeDesc
                                    Select New DTO.vLookupList _
                                    With {.Control = t.AccessorialFeeCalcTypeControl, .Name = t.AccessorialFeeCalcTypeName, .Description = t.AccessorialFeeCalcTypeDesc}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.tblAccessorialFeeCalcTypes
                                    Select New DTO.vLookupList _
                                    With {.Control = t.AccessorialFeeCalcTypeControl, .Name = t.AccessorialFeeCalcTypeName, .Description = t.AccessorialFeeCalcTypeDesc}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using

            Case "AccessorialFeeAllocationType"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.tblAccessorialFeeAllocationTypes
                                    Order By t.AccessorialFeeAllocationTypeName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.AccessorialFeeAllocationTypeControl, .Name = t.AccessorialFeeAllocationTypeName, .Description = t.AccessorialFeeAllocationTypeDesc}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.tblAccessorialFeeAllocationTypes
                                    Order By t.AccessorialFeeAllocationTypeDesc
                                    Select New DTO.vLookupList _
                                    With {.Control = t.AccessorialFeeAllocationTypeControl, .Name = t.AccessorialFeeAllocationTypeName, .Description = t.AccessorialFeeAllocationTypeDesc}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.tblAccessorialFeeAllocationTypes
                                    Select New DTO.vLookupList _
                                    With {.Control = t.AccessorialFeeAllocationTypeControl, .Name = t.AccessorialFeeAllocationTypeName, .Description = t.AccessorialFeeAllocationTypeDesc}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using

            Case "AccessorialFeeType"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.tblAccessorialFeeTypes
                                    Order By t.AccessorialFeeTypeName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.AccessorialFeeTypeControl, .Name = t.AccessorialFeeTypeName, .Description = t.AccessorialFeeTypeDesc}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.tblAccessorialFeeTypes
                                    Order By t.AccessorialFeeTypeDesc
                                    Select New DTO.vLookupList _
                                    With {.Control = t.AccessorialFeeTypeControl, .Name = t.AccessorialFeeTypeName, .Description = t.AccessorialFeeTypeDesc}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.tblAccessorialFeeTypes
                                    Select New DTO.vLookupList _
                                    With {.Control = t.AccessorialFeeTypeControl, .Name = t.AccessorialFeeTypeName, .Description = t.AccessorialFeeTypeDesc}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "LaneByWarehouse"
                Dim oLookup As New List(Of DTO.vLookupList)
                Using db As New NGLMASLaneDataContext(ConnectionString)
                    Try
                        If Criteria Is Nothing Then Return Nothing
                        Dim Control As Integer
                        If (Not Integer.TryParse(Criteria.ToString(), Control)) Then Throw New InvalidOperationException("E_InvalidLaneWarehouse")

                        Dim oSecureComp = From s In db.vUserAdminWithCompControlRefLanes Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                        Select Case sortKey
                            Case 1
                                oLookup = (
                                    From t In db.Lanes
                                    Where t.LaneCompControl = Control _
                                    And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.LaneCompControl)) _
                                    And
                                    (ModDate.HasValue = False OrElse t.LaneModDate > ModDate)
                                    Order By t.LaneName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.LaneControl, .Name = t.LaneName, .Description = t.LaneNumber.ToString}).ToList()
                            Case 2
                                oLookup = (
                                    From t In db.Lanes
                                    Where t.LaneCompControl = Control _
                                    And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.LaneCompControl)) _
                                    And
                                    (ModDate.HasValue = False OrElse t.LaneModDate > ModDate)
                                    Order By t.LaneNumber
                                    Select New DTO.vLookupList _
                                    With {.Control = t.LaneControl, .Name = t.LaneName, .Description = t.LaneNumber.ToString}).ToList()
                                'With {.Control = t.LaneControl, .Name = t.LaneName, .Description = t.LaneNumber.ToString}).Take(12000).ToArray()
                            Case Else
                                oLookup = (
                                    From t In db.Lanes
                                    Where t.LaneCompControl = Control _
                                    And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.LaneCompControl)) _
                                    And
                                    (ModDate.HasValue = False OrElse t.LaneModDate > ModDate)
                                    Select New DTO.vLookupList _
                                    With {.Control = t.LaneControl, .Name = t.LaneName, .Description = t.LaneNumber.ToString}).ToList()
                        End Select

                        oLookup.Insert(0, New DTO.vLookupList With {.Control = 0, .Name = "ALL", .Description = "Any Lane"})
                        vList = oLookup.ToArray()

                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try

                End Using
            Case "LaneActiveByWarehouse"
                Dim oLookup As New List(Of DTO.vLookupList)
                Using db As New NGLMASLaneDataContext(ConnectionString)
                    Try
                        If Criteria Is Nothing Then Return Nothing
                        Dim Control As Integer
                        If (Not Integer.TryParse(Criteria.ToString(), Control)) Then Throw New InvalidOperationException("E_InvalidLaneWarehouse")

                        Dim oSecureComp = From s In db.vUserAdminWithCompControlRefLanes Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                        Select Case sortKey
                            Case 1
                                oLookup = (
                                    From t In db.Lanes
                                    Where t.LaneCompControl = Control _
                                    And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.LaneCompControl)) _
                                    And (t.LaneActive.HasValue = False OrElse t.LaneActive = True) _
                                    And
                                    (ModDate.HasValue = False OrElse t.LaneModDate > ModDate)
                                    Order By t.LaneName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.LaneControl, .Name = t.LaneName, .Description = t.LaneNumber.ToString}).ToList()
                            Case 2
                                oLookup = (
                                    From t In db.Lanes
                                    Where t.LaneCompControl = Control _
                                    And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.LaneCompControl)) _
                                    And (t.LaneActive.HasValue = False OrElse t.LaneActive = True) _
                                    And
                                    (ModDate.HasValue = False OrElse t.LaneModDate > ModDate)
                                    Order By t.LaneNumber
                                    Select New DTO.vLookupList _
                                    With {.Control = t.LaneControl, .Name = t.LaneName, .Description = t.LaneNumber.ToString}).ToList()
                            Case Else
                                oLookup = (
                                    From t In db.Lanes
                                    Where t.LaneCompControl = Control _
                                    And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.LaneCompControl)) _
                                    And (t.LaneActive.HasValue = False OrElse t.LaneActive = True) _
                                    And
                                    (ModDate.HasValue = False OrElse t.LaneModDate > ModDate)
                                    Select New DTO.vLookupList _
                                    With {.Control = t.LaneControl, .Name = t.LaneName, .Description = t.LaneNumber.ToString}).ToList()
                        End Select

                        oLookup.Insert(0, New DTO.vLookupList With {.Control = 0, .Name = "ALL", .Description = "Any Lane"})
                        vList = oLookup.ToArray()

                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "LaneCrossDockLists"
                Dim oLookup As New List(Of DTO.vLookupList)
                Using db As New NGLMASLaneDataContext(ConnectionString)
                    Try
                        Dim oSecureComp = From s In db.vUserAdminWithCompControlRefLanes Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                        Select Case sortKey
                            Case 1
                                oLookup = (
                                    From t In db.Lanes
                                    Where t.LaneIsCrossDockFacility = True _
                                    And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.LaneCompControl)) _
                                    And
                                    (ModDate.HasValue = False OrElse t.LaneModDate > ModDate)
                                    Order By t.LaneName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.LaneControl, .Name = t.LaneName, .Description = t.LaneNumber.ToString}).ToList()
                            Case 2
                                oLookup = (
                                    From t In db.Lanes
                                    Where t.LaneIsCrossDockFacility = True _
                                     And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.LaneCompControl)) _
                                     And
                                     (ModDate.HasValue = False OrElse t.LaneModDate > ModDate)
                                    Order By t.LaneNumber
                                    Select New DTO.vLookupList _
                                    With {.Control = t.LaneControl, .Name = t.LaneName, .Description = t.LaneNumber.ToString}).ToList()
                            Case Else
                                oLookup = (
                                    From t In db.Lanes
                                    Where t.LaneIsCrossDockFacility = True _
                                    And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.LaneCompControl)) _
                                    And
                                    (ModDate.HasValue = False OrElse t.LaneModDate > ModDate)
                                    Select New DTO.vLookupList _
                                    With {.Control = t.LaneControl, .Name = t.LaneName, .Description = t.LaneNumber.ToString}).ToList()
                        End Select

                        oLookup.Insert(0, New DTO.vLookupList With {.Control = 0, .Name = "ALL", .Description = "Any Lane"})
                        vList = oLookup.ToArray()

                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try

                End Using
            Case "LaneCarrierTariff"
                Dim oLookup As New List(Of DTO.vLookupList)
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        If Criteria Is Nothing Then Return Nothing
                        Dim Control As Integer
                        If (Not Integer.TryParse(Criteria.ToString(), Control)) Then Throw New InvalidOperationException("E_InvalidLaneWarehouse")

                        '    Dim oSecureComp = From s In db.vUserAdminWithCompControlRefLanes Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                        Select Case sortKey
                            Case 1
                                oLookup = (
                                    From t In db.udfLookupLaneDefaultCarrier(Control, True)
                                    Order By t.CarrierName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrierControl, .Name = t.CarrierName, .Description = t.CarrierNumber.ToString}).ToList()
                            Case 2
                                oLookup = (
                                    From t In db.udfLookupLaneDefaultCarrier(Control, True)
                                    Order By t.CarrierNumber
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrierControl, .Name = t.CarrierName, .Description = t.CarrierNumber.ToString}).ToList()
                            Case Else
                                oLookup = (
                                    From t In db.udfLookupLaneDefaultCarrier(Control, True)
                                    Order By t.CarrierControl
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrierControl, .Name = t.CarrierName, .Description = t.CarrierNumber.ToString}).ToList()
                        End Select

                        oLookup.Insert(0, New DTO.vLookupList With {.Control = 0, .Name = "**Optimal Provider**", .Description = "**Optimal Provider**"})
                        vList = oLookup.ToArray()

                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try

                End Using
            Case "CarrierProName"
                Dim intCarrierControl As Integer = 0
                Dim intCompControl As Integer = 0

                'Dim t As Type = Criteria.GetType()
                'If Not t.IsArray Then Return vList
                'Dim arrCriteria As Integer() = DirectCast(Criteria, Integer())
                'If arrCriteria Is Nothing OrElse arrCriteria.Count < 2 Then Return vList
                'intCarrierControl = arrCriteria(0)
                'intCompControl = arrCriteria(1)
                Dim sArr = Criteria.ToString().Split(",")
                If sArr Is Nothing OrElse sArr.Count < 2 Then Return vList
                Integer.TryParse(sArr(0), intCarrierControl)
                Integer.TryParse(sArr(1), intCompControl)

                If intCarrierControl = 0 Or intCompControl = 0 Then Return vList
                'And d.CarrProActive = True _
                Using db As New NGLMASCarrierDataContext(ConnectionString)
                    Try
                        Dim lookupList As New List(Of DTO.vLookupList)
                        lookupList = (
                            From d In db.CarrierProNumbers
                            Where d.CarrProCarrierControl = intCarrierControl _
                            And d.CarrProCompControl = intCompControl
                            Order By d.CarrProName
                            Select New DTO.vLookupList _
                            With {.Name = d.CarrProName, .Description = If(d.CarrProActive, "Active", "Inactive")}).Distinct().ToList()

                        Dim itemDefault As New DTO.vLookupList(0, "", "")
                        If lookupList Is Nothing Then lookupList = New List(Of DTO.vLookupList)
                        lookupList.Insert(0, itemDefault)
                        vList = lookupList.ToArray()

                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "CarrierTariffProName"
                'Added By RHR on 09/25/2018 for v-8.2 Tariff Changes
                Dim intCarrTarControl As Integer = 0
                Integer.TryParse(Criteria.ToString(), intCarrTarControl)
                Dim oLookup As New List(Of DTO.vLookupList)
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        If intCarrTarControl = 0 Then Return Nothing

                        oLookup = (
                                    From t In db.spGetCarrierTariffProNames(intCarrTarControl)
                                    Select New DTO.vLookupList _
                                    With {.Control = 0, .Name = t.CarrProName, .Description = t.Description}).ToList()


                        oLookup.Insert(0, New DTO.vLookupList With {.Control = 0, .Name = "None", .Description = "No Carrier Pro Fromula"})
                        vList = oLookup.ToArray()

                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using


            Case "tblCountries"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Dim lookupList As New List(Of DTO.vLookupList)
                        Select Case sortKey
                            Case 1
                                lookupList = (
                                    From t In db.tblCountries
                                    Order By t.CountryISO
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CountryControl, .Name = t.CountryISO, .Description = t.CountryName}).ToList()
                            Case 2
                                lookupList = (
                                    From t In db.tblCountries
                                    Order By t.CountryName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CountryControl, .Name = t.CountryISO, .Description = t.CountryName}).ToList()
                            Case Else
                                lookupList = (
                                    From t In db.tblCountries
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CountryControl, .Name = t.CountryISO, .Description = t.CountryName}).ToList()
                        End Select

                        Dim itemDefault As New DTO.vLookupList(0, "", "")
                        If lookupList Is Nothing Then lookupList = New List(Of DTO.vLookupList)
                        lookupList.Insert(0, itemDefault)
                        vList = lookupList.ToArray()

                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "CarrierQualValidated"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        'db.Log = New DebugTextWriter
                        Dim oSecureTariffs = From d In db.vSecureCarrierRefLookups Where d.UserAdminUserName = Me.Parameters.UserName Select d.CarrTarCarrierControl

                        Select Case sortKey
                            Case 1

                                vList = (
                                    From t In db.vCarrierQualValidateds
                                    Where
                                    (oSecureTariffs Is Nothing OrElse oSecureTariffs.Count = 0 OrElse oSecureTariffs.Contains(t.CarrierControl))
                                    Order By t.CarrierName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrierControl, .Name = t.CarrierName, .Description = t.CarrierNumber.ToString}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.vCarrierQualValidateds
                                    Where
                                    (oSecureTariffs Is Nothing OrElse oSecureTariffs.Count = 0 OrElse oSecureTariffs.Contains(t.CarrierControl))
                                    Order By t.CarrierNumber
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrierControl, .Name = t.CarrierName, .Description = t.CarrierNumber.ToString}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.vCarrierQualValidateds
                                    Where
                                    (oSecureTariffs Is Nothing OrElse oSecureTariffs.Count = 0 OrElse oSecureTariffs.Contains(t.CarrierControl))
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrierControl, .Name = t.CarrierName, .Description = t.CarrierNumber.ToString}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "tblEdiActions"
                Using db As New NGLMASCarrierDataContext(ConnectionString)
                    Try
                        Dim lookupList As New List(Of DTO.vLookupList)
                        Select Case sortKey
                            Case 1
                                lookupList = (
                                    From t In db.tblEDIActions
                                    Order By t.EDIAFactoryName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.EDIAControl, .Name = t.EDIAFactoryName, .Description = t.EDIADescription}).ToList()
                            Case 2
                                lookupList = (
                                    From t In db.tblEDIActions
                                    Order By t.EDIADescription
                                    Select New DTO.vLookupList _
                                    With {.Control = t.EDIAControl, .Name = t.EDIAFactoryName, .Description = t.EDIADescription}).ToList()
                            Case Else
                                lookupList = (
                                    From t In db.tblEDIActions
                                    Order By t.EDIAFactoryName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.EDIAControl, .Name = t.EDIAFactoryName, .Description = t.EDIADescription}).ToList()
                        End Select

                        vList = lookupList.ToArray()

                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "tblEdiElements"
                Using db As New NGLMASCarrierDataContext(ConnectionString)
                    Try
                        Dim lookupList As New List(Of DTO.vLookupList)
                        Select Case sortKey
                            Case 1
                                lookupList = (
                                    From t In db.tblEDIElements
                                    Order By t.EDIEName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.EDIEControl, .Name = t.EDIEName, .Description = t.EDIEDescription}).ToList()
                            Case 2
                                lookupList = (
                                   From t In db.tblEDIElements
                                   Order By t.EDIEDescription
                                   Select New DTO.vLookupList _
                                   With {.Control = t.EDIEControl, .Name = t.EDIEName, .Description = t.EDIEDescription}).ToList()
                            Case Else
                                lookupList = (
                                    From t In db.tblEDIElements
                                    Order By t.EDIEName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.EDIEControl, .Name = t.EDIEName, .Description = t.EDIEDescription}).ToList()
                        End Select
                        vList = lookupList.ToArray()

                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "tblEdiTypes"
                Using db As New NGLMASEDIMaintDataContext(ConnectionString)
                    Try
                        Dim lookupList As New List(Of DTO.vLookupList)
                        Select Case sortKey
                            Case 1
                                lookupList = (
                                    From t In db.tblEDITypes
                                    Order By t.EDITName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.EDITControl, .Name = t.EDITName, .Description = t.EDITDescription}).ToList()
                            Case 2
                                lookupList = (
                                    From t In db.tblEDITypes
                                    Order By t.EDITDescription
                                    Select New DTO.vLookupList _
                                    With {.Control = t.EDITControl, .Name = t.EDITName, .Description = t.EDITDescription}).ToList()
                            Case Else
                                lookupList = (
                                    From t In db.tblEDITypes
                                    Order By t.EDITName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.EDITControl, .Name = t.EDITName, .Description = t.EDITDescription}).ToList()
                        End Select
                        vList = lookupList.ToArray()

                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "tblHDM"
                Using db As New NGLMASLaneDataContext(ConnectionString)
                    Try
                        Dim lookupList As New List(Of DTO.vLookupList)
                        Select Case sortKey
                            Case 1
                                lookupList = (
                                    From t In db.tblHDMs
                                    Order By t.HDMName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.HDMControl, .Name = t.HDMName, .Description = t.HDMDesc}).ToList()
                            Case 2
                                lookupList = (
                                    From t In db.tblHDMs
                                    Order By t.HDMDesc
                                    Select New DTO.vLookupList _
                                    With {.Control = t.HDMControl, .Name = t.HDMName, .Description = t.HDMDesc}).ToList()
                            Case Else
                                lookupList = (
                                   From t In db.tblHDMs
                                   Order By t.HDMName
                                   Select New DTO.vLookupList _
                                   With {.Control = t.HDMControl, .Name = t.HDMName, .Description = t.HDMDesc}).ToList()
                        End Select
                        Dim itemDefault As New DTO.vLookupList(0, "None", "No HDM")
                        If lookupList Is Nothing Then lookupList = New List(Of DTO.vLookupList)
                        lookupList.Insert(0, itemDefault)
                        vList = lookupList.ToArray()

                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "tblERPType"
                Using db As New NGLMASIntegrationDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.tblERPTypes
                                    Order By t.Name
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ERPTypeControl, .Name = t.Name, .Description = t.Description}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.tblERPTypes
                                    Order By t.Description
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ERPTypeControl, .Name = t.Name, .Description = t.Description}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.tblERPTypes
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ERPTypeControl, .Name = t.Name, .Description = t.Description}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "tblIntegrationType"
                Using db As New NGLMASIntegrationDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.tblIntegrationTypes
                                    Order By t.Name
                                    Select New DTO.vLookupList _
                                    With {.Control = t.IntegrationTypeControl, .Name = t.Name, .Description = t.Description}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.tblIntegrationTypes
                                    Order By t.Description
                                    Select New DTO.vLookupList _
                                    With {.Control = t.IntegrationTypeControl, .Name = t.Name, .Description = t.Description}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.tblIntegrationTypes
                                    Select New DTO.vLookupList _
                                    With {.Control = t.IntegrationTypeControl, .Name = t.Name, .Description = t.Description}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "vLookupERPSettings"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.vLookupERPSettings
                                    Order By t.Name
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ERPSettingControl, .Name = t.Name & "-" & t.LegalEntity, .Description = t.Description}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.vLookupERPSettings
                                    Order By t.Description
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ERPSettingControl, .Name = t.Name & "-" & t.LegalEntity, .Description = t.Description}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.vLookupERPSettings
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ERPSettingControl, .Name = t.Name & "-" & t.LegalEntity, .Description = t.Description}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "SingleSignOnAccountName"
                ' by LVV 3/22/16 for v-7.0.5.1 DAT
                Using db As New NGLMASSecurityDataContext(ConnectionString)
                    Try

                        vList = (
                            From t In db.tblSingleSignOnAccounts
                            Order By t.SSOAControl
                            Select New DTO.vLookupList _
                            With {.Control = t.SSOAControl, .Name = t.SSOAName, .Description = t.SSOADesc}).ToArray()


                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "DATEquipType"
                ' Added by LVV 5/3/16 for v-7.0.5.1 DAT
                Dim oLookup As New List(Of DTO.vLookupList)
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try

                        oLookup = (
                            From t In db.tblDATEquipTypes
                            Order By t.DATEquipTypeControl
                            Select New DTO.vLookupList _
                            With {.Control = t.DATEquipTypeControl, .Name = t.DATEquipTypeName, .Description = t.DATEquipTypeDesc}).ToList()

                        oLookup.Insert(0, New DTO.vLookupList With {.Control = 0, .Name = "None", .Description = "None"})
                        vList = oLookup.ToArray()

                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "SubscriptionAlerts"
                'Added By LVV on 12/1/16 for v-7.0.5.110 Subscription Alert Changes
                Dim oLookup As New List(Of DTO.vLookupList)
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        If Criteria Is Nothing Then Return Nothing
                        Dim Control As Integer
                        If (Not Integer.TryParse(Criteria.ToString(), Control)) Then Throw New InvalidOperationException("Cannot get subscription alerts because the user control number criteria is not valid.")

                        Select Case sortKey
                            Case 1
                                oLookup = (
                                    From t In db.spGetSubscriptionAlerts(Criteria)
                                    Order By t.ProcedureControl
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ProcedureControl, .Name = t.ProcedureName, .Description = t.ProcedureDescription}).ToList()
                            Case 2
                                oLookup = (
                                    From t In db.spGetSubscriptionAlerts(Criteria)
                                    Order By t.ProcedureDescription
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ProcedureControl, .Name = t.ProcedureName, .Description = t.ProcedureDescription}).ToList()
                            Case Else
                                oLookup = (
                                    From t In db.spGetSubscriptionAlerts(Criteria)
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ProcedureControl, .Name = t.ProcedureName, .Description = t.ProcedureDescription}).ToList()
                        End Select

                        oLookup.Insert(0, New DTO.vLookupList With {.Control = 0, .Name = "Show All", .Description = "Show All"})
                        vList = oLookup.ToArray()

                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "tblFilterType"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1


                                vList = (
                                    From t In db.tblFilterTypes
                                    Order By t.FilterTypeName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.FilterTypeControl, .Name = t.FilterTypeName, .Description = t.FilterTypeDesc}).ToArray()
                            Case 2
                                vList = (
                                     From t In db.tblFilterTypes
                                     Order By t.FilterTypeDesc
                                     Select New DTO.vLookupList _
                                     With {.Control = t.FilterTypeControl, .Name = t.FilterTypeName, .Description = t.FilterTypeDesc}).ToArray()
                            Case Else
                                vList = (
                                     From t In db.tblFilterTypes
                                     Select New DTO.vLookupList _
                                     With {.Control = t.FilterTypeControl, .Name = t.FilterTypeName, .Description = t.FilterTypeDesc}).ToArray()
                        End Select

                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using

            Case "cmGroupType" 'Modified by RHR for v-8.0 on 2/20/2017
                Using db As New NGLMASSecurityDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.cmGroupTypes
                                    Order By t.GroupTypeName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.GroupTypeControl, .Name = t.GroupTypeName, .Description = t.GroupTypeDesc}).ToArray()
                            Case 2
                                vList = (
                                     From t In db.cmGroupTypes
                                     Order By t.GroupTypeDesc
                                     Select New DTO.vLookupList _
                                     With {.Control = t.GroupTypeControl, .Name = t.GroupTypeName, .Description = t.GroupTypeDesc}).ToArray()
                            Case Else
                                vList = (
                                     From t In db.cmGroupTypes
                                     Select New DTO.vLookupList _
                                     With {.Control = t.GroupTypeControl, .Name = t.GroupTypeName, .Description = t.GroupTypeDesc}).ToArray()
                        End Select

                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "cmGroupSubType" 'Modified by RHR for v-8.0 on 2/20/2017
                Using db As New NGLMASSecurityDataContext(ConnectionString)
                    Try
                        Dim Control As Integer = 0
                        If (Not Integer.TryParse(Criteria.ToString(), Control)) Then Throw New InvalidOperationException("Cannot get the Group SubType Lookup list because the Group Type control number criteria is not valid.")

                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.cmGroupSubTypes Where (Control = 0 OrElse t.GroupSubTypeGroupTypeControl = Control)
                                    Order By t.GroupSubTypeName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.GroupSubTypeControl, .Name = t.GroupSubTypeName, .Description = t.GroupSubTypeDesc}).ToArray()
                            Case 2
                                vList = (
                                     From t In db.cmGroupSubTypes Where (Control = 0 OrElse t.GroupSubTypeGroupTypeControl = Control)
                                     Order By t.GroupSubTypeDesc
                                     Select New DTO.vLookupList _
                                     With {.Control = t.GroupSubTypeControl, .Name = t.GroupSubTypeName, .Description = t.GroupSubTypeDesc}).ToArray()
                            Case Else
                                vList = (
                                     From t In db.cmGroupSubTypes Where (Control = 0 OrElse t.GroupSubTypeGroupTypeControl = Control)
                                     Select New DTO.vLookupList _
                                     With {.Control = t.GroupSubTypeControl, .Name = t.GroupSubTypeName, .Description = t.GroupSubTypeDesc}).ToArray()
                        End Select

                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "cmDataType" 'Modified by RHR for v-8.0 on 2/20/2017
                Using db As New NGLMASSecurityDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.cmDataTypes
                                    Order By t.DataTypeName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.DataTypeControl, .Name = t.DataTypeName, .Description = t.DataTypeDesc}).ToArray()
                            Case 2
                                vList = (
                                     From t In db.cmDataTypes
                                     Order By t.DataTypeDesc
                                     Select New DTO.vLookupList _
                                     With {.Control = t.DataTypeControl, .Name = t.DataTypeName, .Description = t.DataTypeDesc}).ToArray()
                            Case Else
                                vList = (
                                     From t In db.cmDataTypes
                                     Select New DTO.vLookupList _
                                     With {.Control = t.DataTypeControl, .Name = t.DataTypeName, .Description = t.DataTypeDesc}).ToArray()
                        End Select

                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using

            Case "cmMenuType" 'Modified by RHR for v-8.0 on 2/20/2017
                Using db As New NGLMASSecurityDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.cmMenuTypes
                                    Order By t.MenuTypeName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.MenuTypeControl, .Name = t.MenuTypeName, .Description = t.MenuTypeDesc}).ToArray()
                            Case 2
                                vList = (
                                     From t In db.cmMenuTypes
                                     Order By t.MenuTypeDesc
                                     Select New DTO.vLookupList _
                                     With {.Control = t.MenuTypeControl, .Name = t.MenuTypeName, .Description = t.MenuTypeDesc}).ToArray()
                            Case Else
                                vList = (
                                     From t In db.cmMenuTypes
                                     Select New DTO.vLookupList _
                                     With {.Control = t.MenuTypeControl, .Name = t.MenuTypeName, .Description = t.MenuTypeDesc}).ToArray()
                        End Select

                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "cmPage" 'Modified by RHR for v-8.0 on 2/20/2017
                Using db As New NGLMASSecurityDataContext(ConnectionString)
                    'Notes: 
                    '   Page List Is filtered by Role Center security for user using FormControl
                    '   This is a dynamic list so we check the ModDate parameter and if provided 
                    '   we only return records that have changed.  The caller must merge the changes.

                    Try
                        'Logic to test the ModDate Parameter
                        Dim blnUseModDate As Boolean = False
                        Dim dtStartDate As Date = Date.Now()
                        If ModDate.HasValue Then
                            blnUseModDate = True
                            dtStartDate = ModDate.Value
                        End If

                        'Logic to check user security settings
                        Dim blnUseFormFilter As Boolean = False
                        Dim lRestricted As List(Of Integer) = (From x In db.tblFormSecurityXrefs Join u In db.tblUserSecurities On x.UserSecurityControl Equals u.UserSecurityControl Where u.UserName = Me.Parameters.UserName Select x.FormControl).ToList()
                        If Not lRestricted Is Nothing AndAlso lRestricted.Count() > 0 Then blnUseFormFilter = True

                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.cmPages
                                    Where (blnUseFormFilter = False OrElse Not lRestricted.Contains(t.PageFormControl)) And (blnUseModDate = False OrElse If(t.PageModDate, dtStartDate) >= dtStartDate)
                                    Order By t.PageName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.PageControl, .Name = t.PageName, .Description = t.PageDesc}).ToArray()
                            Case 2
                                vList = (
                                     From t In db.cmPages
                                     Where (blnUseFormFilter = False OrElse Not lRestricted.Contains(t.PageFormControl)) And (blnUseModDate = False OrElse If(t.PageModDate, dtStartDate) >= dtStartDate)
                                     Order By t.PageDesc
                                     Select New DTO.vLookupList _
                                     With {.Control = t.PageControl, .Name = t.PageName, .Description = t.PageDesc}).ToArray()
                            Case Else
                                vList = (
                                     From t In db.cmPages
                                     Where (blnUseFormFilter = False OrElse Not lRestricted.Contains(t.PageFormControl)) And (blnUseModDate = False OrElse If(t.PageModDate, dtStartDate) >= dtStartDate)
                                     Select New DTO.vLookupList _
                                     With {.Control = t.PageControl, .Name = t.PageName, .Description = t.PageDesc}).ToArray()
                        End Select

                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "cmPageDetail" 'Modified by RHR for v-8.0 on 2/20/2017
                Using db As New NGLMASSecurityDataContext(ConnectionString)
                    Try

                        'Security on Page Details is controled by the Page.  Users should only be able to select details from a page or list of pages where security is already applied.
                        If Criteria Is Nothing Then Return Nothing
                        Dim Control As Integer = 0
                        If (Not Integer.TryParse(Criteria.ToString(), Control)) Then Throw New InvalidOperationException("Cannot get the Page Detail Lookup list because the Page control number criteria is not valid.")

                        'Logic to test the ModDate Parameter
                        Dim blnUseModDate As Boolean = False
                        Dim dtStartDate As Date = Date.Now()
                        If ModDate.HasValue Then
                            blnUseModDate = True
                            dtStartDate = ModDate.Value
                        End If


                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.cmPageDetails
                                    Where (t.PageDetPageControl = Control) And (blnUseModDate = False OrElse If(t.PageDetModDate, dtStartDate) >= dtStartDate)
                                    Order By t.PageDetName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.PageDetControl, .Name = t.PageDetName, .Description = t.PageDetDesc}).ToArray()
                            Case 2
                                vList = (
                                     From t In db.cmPageDetails
                                     Where (t.PageDetPageControl = Control) And (blnUseModDate = False OrElse If(t.PageDetModDate, dtStartDate) >= dtStartDate)
                                     Order By t.PageDetDesc
                                     Select New DTO.vLookupList _
                                     With {.Control = t.PageDetControl, .Name = t.PageDetName, .Description = t.PageDetDesc}).ToArray()
                            Case Else
                                vList = (
                                     From t In db.cmPageDetails
                                     Where (t.PageDetPageControl = Control) And (blnUseModDate = False OrElse If(t.PageDetModDate, dtStartDate) >= dtStartDate)
                                     Select New DTO.vLookupList _
                                     With {.Control = t.PageDetControl, .Name = t.PageDetName, .Description = t.PageDetDesc}).ToArray()
                        End Select

                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "cmDataElement" 'Modified by RHR for v-8.0 on 2/20/2017
                Using db As New NGLMASSecurityDataContext(ConnectionString)
                    'Notes: 
                    '   This is a dynamic list so we check the ModDate parameter and if provided 
                    '   we only return records that have changed.  The caller must merge the changes.

                    Try
                        'Logic to test the ModDate Parameter
                        Dim blnUseModDate As Boolean = False
                        Dim dtStartDate As Date = Date.Now()
                        If ModDate.HasValue Then
                            blnUseModDate = True
                            dtStartDate = ModDate.Value
                        End If

                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.cmDataElements
                                    Where (blnUseModDate = False OrElse If(t.DataElmtModDate, dtStartDate) >= dtStartDate)
                                    Order By t.DataElmtName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.DataElmtControl, .Name = t.DataElmtName, .Description = t.DataElmtDesc}).ToArray()
                            Case 2
                                vList = (
                                     From t In db.cmDataElements
                                     Where (blnUseModDate = False OrElse If(t.DataElmtModDate, dtStartDate) >= dtStartDate)
                                     Order By t.DataElmtDesc
                                     Select New DTO.vLookupList _
                                     With {.Control = t.DataElmtControl, .Name = t.DataElmtName, .Description = t.DataElmtDesc}).ToArray()
                            Case Else
                                vList = (
                                     From t In db.cmDataElements
                                     Where (blnUseModDate = False OrElse If(t.DataElmtModDate, dtStartDate) >= dtStartDate)
                                     Select New DTO.vLookupList _
                                     With {.Control = t.DataElmtControl, .Name = t.DataElmtName, .Description = t.DataElmtDesc}).ToArray()
                        End Select

                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "cmElementField" 'Modified by RHR for v-8.0 on 2/20/2017
                Using db As New NGLMASSecurityDataContext(ConnectionString)
                    Try

                        'Security on Element Fields is controled by the Data Element.  
                        If Criteria Is Nothing Then Return Nothing
                        Dim Control As Integer = 0
                        If (Not Integer.TryParse(Criteria.ToString(), Control)) Then Throw New InvalidOperationException("Cannot get the Element Field Lookup list because the Data Element control number criteria is not valid.")

                        'Logic to test the ModDate Parameter
                        Dim blnUseModDate As Boolean = False
                        Dim dtStartDate As Date = Date.Now()
                        If ModDate.HasValue Then
                            blnUseModDate = True
                            dtStartDate = ModDate.Value
                        End If


                        Select Case sortKey
                            Case 1
                                vList = (
                                        From t In db.cmElementFields
                                        Where (t.ElmtFieldDataElmtControl = Control) And (blnUseModDate = False OrElse If(t.ElmtFieldModDate, dtStartDate) >= dtStartDate)
                                        Order By t.ElmtFieldName
                                        Select New DTO.vLookupList _
                                        With {.Control = t.ElmtFieldControl, .Name = t.ElmtFieldName, .Description = t.ElmtFieldDesc}).ToArray()
                            Case 2
                                vList = (
                                         From t In db.cmElementFields
                                         Where (t.ElmtFieldDataElmtControl = Control) And (blnUseModDate = False OrElse If(t.ElmtFieldModDate, dtStartDate) >= dtStartDate)
                                         Order By t.ElmtFieldDesc
                                         Select New DTO.vLookupList _
                                         With {.Control = t.ElmtFieldControl, .Name = t.ElmtFieldName, .Description = t.ElmtFieldDesc}).ToArray()
                            Case Else
                                vList = (
                                         From t In db.cmElementFields
                                         Where (t.ElmtFieldDataElmtControl = Control) And (blnUseModDate = False OrElse If(t.ElmtFieldModDate, dtStartDate) >= dtStartDate)
                                         Select New DTO.vLookupList _
                                         With {.Control = t.ElmtFieldControl, .Name = t.ElmtFieldName, .Description = t.ElmtFieldDesc}).ToArray()
                        End Select

                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "tblBidStatusCode" 'Modified by LVV for v-8.0 on 4/3/2017
                Using db As New NGLMASIntegrationDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.tblBidStatusCodes
                                    Order By t.BSCName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.BSCControl, .Name = t.BSCName, .Description = t.BSCDesc}).ToArray()
                            Case 2
                                vList = (
                                     From t In db.tblBidStatusCodes
                                     Order By t.BSCDesc
                                     Select New DTO.vLookupList _
                                     With {.Control = t.BSCControl, .Name = t.BSCName, .Description = t.BSCDesc}).ToArray()
                            Case Else
                                vList = (
                                     From t In db.tblBidStatusCodes
                                     Select New DTO.vLookupList _
                                     With {.Control = t.BSCControl, .Name = t.BSCName, .Description = t.BSCDesc}).ToArray()
                        End Select

                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "tblLanguageCode" 'Modified by RHR for v-8.0 on 08/23/2017
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.tblLanguageCodes
                                    Order By t.LCName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.LCControl, .Name = t.LCName, .Description = t.LCDesc}).ToArray()
                            Case 2
                                vList = (
                                     From t In db.tblLanguageCodes
                                     Order By t.LCDesc
                                     Select New DTO.vLookupList _
                                     With {.Control = t.LCControl, .Name = t.LCName, .Description = t.LCDesc}).ToArray()
                            Case Else
                                vList = (
                                     From t In db.tblLanguageCodes
                                     Select New DTO.vLookupList _
                                     With {.Control = t.LCControl, .Name = t.LCName, .Description = t.LCDesc}).ToArray()
                        End Select

                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "cmPageDetailDataElement" 'Modified by RHR for v-8.0 on 10/01/2017  Returns a vlookup list of PageDetails where Control = PageDetControl, Name = PageDetName, and Description/Number = the PageDetDataElmtControl filtered by pageControl
                Using db As New NGLMASSecurityDataContext(ConnectionString)
                    Try

                        'Security on Page Details is controled by the Page.  Users should only be able to select details from a page or list of pages where security is already applied.
                        If Criteria Is Nothing Then Return Nothing
                        Dim Control As Integer = 0
                        If (Not Integer.TryParse(Criteria.ToString(), Control)) Then Throw New InvalidOperationException("Cannot get the Page Detail Data Element Lookup list because the Page control number criteria is not valid.")

                        'Logic to test the ModDate Parameter
                        Dim blnUseModDate As Boolean = False
                        Dim dtStartDate As Date = Date.Now()
                        If ModDate.HasValue Then
                            blnUseModDate = True
                            dtStartDate = ModDate.Value
                        End If


                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.cmPageDetails
                                    Where (t.PageDetPageControl = Control) And (blnUseModDate = False OrElse If(t.PageDetModDate, dtStartDate) >= dtStartDate)
                                    Order By t.PageDetName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.PageDetControl, .Name = t.PageDetName, .Description = t.PageDetDataElmtControl}).ToArray()
                            Case 2
                                vList = (
                                     From t In db.cmPageDetails
                                     Where (t.PageDetPageControl = Control) And (blnUseModDate = False OrElse If(t.PageDetModDate, dtStartDate) >= dtStartDate)
                                     Order By t.PageDetDesc
                                     Select New DTO.vLookupList _
                                     With {.Control = t.PageDetControl, .Name = t.PageDetName, .Description = t.PageDetDataElmtControl}).ToArray()
                            Case Else
                                vList = (
                                     From t In db.cmPageDetails
                                     Where (t.PageDetPageControl = Control) And (blnUseModDate = False OrElse If(t.PageDetModDate, dtStartDate) >= dtStartDate)
                                     Select New DTO.vLookupList _
                                     With {.Control = t.PageDetControl, .Name = t.PageDetName, .Description = t.PageDetDataElmtControl}).ToArray()
                        End Select

                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "cmPageDetailElementField" 'Modified by RHR for v-8.0 on 10/01/2017  Returns a vlookup list of Element Fields where Control = ElmtFieldControl, Name = ElmtFieldName, and Description/Number = ElmtFieldDesc filtered by the parent cmPageDetailControl
                Using db As New NGLMASSecurityDataContext(ConnectionString)
                    Try
                        'Security on Element Fields is controled by the Data Element.  
                        If Criteria Is Nothing Then Return Nothing
                        Dim Control As Integer = 0
                        If (Not Integer.TryParse(Criteria.ToString(), Control)) Then Throw New InvalidOperationException("Cannot get the Page Detail Element Field Lookup list because the Parent Page Detail control number criteria is not valid.")

                        Select Case sortKey
                            Case 1
                                vList = (
                                        From t In db.cmElementFields Join d In db.cmPageDetails On t.ElmtFieldDataElmtControl Equals d.PageDetDataElmtControl
                                        Where (d.PageDetControl = Control)
                                        Order By t.ElmtFieldName
                                        Select New DTO.vLookupList _
                                        With {.Control = t.ElmtFieldControl, .Name = t.ElmtFieldName, .Description = t.ElmtFieldDesc}).ToArray()
                            Case 2
                                vList = (
                                         From t In db.cmElementFields Join d In db.cmPageDetails On t.ElmtFieldDataElmtControl Equals d.PageDetDataElmtControl
                                         Where (d.PageDetControl = Control)
                                         Order By t.ElmtFieldDesc
                                         Select New DTO.vLookupList _
                                         With {.Control = t.ElmtFieldControl, .Name = t.ElmtFieldName, .Description = t.ElmtFieldDesc}).ToArray()
                            Case Else
                                vList = (
                                         From t In db.cmElementFields Join d In db.cmPageDetails On t.ElmtFieldDataElmtControl Equals d.PageDetDataElmtControl
                                         Where (d.PageDetControl = Control)
                                         Select New DTO.vLookupList _
                                         With {.Control = t.ElmtFieldControl, .Name = t.ElmtFieldName, .Description = t.ElmtFieldDesc}).ToArray()
                        End Select

                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "tblDispatchType" 'Modified by LVV for v-8.0 on 10/23/2017
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.tblDispatchTypes
                                    Order By t.DispatchTypeName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.DispatchTypeControl, .Name = t.DispatchTypeName, .Description = t.DispatchTypeDesc}).ToArray()
                            Case 2
                                vList = (
                                     From t In db.tblDispatchTypes
                                     Order By t.DispatchTypeDesc
                                     Select New DTO.vLookupList _
                                     With {.Control = t.DispatchTypeControl, .Name = t.DispatchTypeName, .Description = t.DispatchTypeDesc}).ToArray()
                            Case Else
                                vList = (
                                     From t In db.tblDispatchTypes
                                     Select New DTO.vLookupList _
                                     With {.Control = t.DispatchTypeControl, .Name = t.DispatchTypeName, .Description = t.DispatchTypeDesc}).ToArray()
                        End Select

                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using

            Case "tblLoadTenderTransType" 'Modified by RHR for v-8.1 on 03/28/2018
                Using db As New NGLMASIntegrationDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.tblLoadTenderTransTypes
                                    Order By t.LTTTName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.LTTTControl, .Name = t.LTTTName, .Description = t.LTTTDesc}).ToArray()
                            Case 2
                                vList = (
                                     From t In db.tblLoadTenderTransTypes
                                     Order By t.LTTTDesc
                                     Select New DTO.vLookupList _
                                     With {.Control = t.LTTTControl, .Name = t.LTTTName, .Description = t.LTTTDesc}).ToArray()
                            Case Else
                                vList = (
                                     From t In db.tblLoadTenderTransTypes
                                     Select New DTO.vLookupList _
                                     With {.Control = t.LTTTControl, .Name = t.LTTTName, .Description = t.LTTTDesc}).ToArray()
                        End Select

                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using

            Case "vNGLAPIPalletTypes" 'Modified by RHR for v-8.1 on 03/28/2018
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.vNGLAPIPalletTypes
                                    Order By t.PalletType
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ID, .Name = t.PalletType, .Description = t.PalletTypeDescription}).ToArray()
                            Case 2
                                vList = (
                                     From t In db.vNGLAPIPalletTypes
                                     Order By t.PalletTypeDescription
                                     Select New DTO.vLookupList _
                                     With {.Control = t.ID, .Name = t.PalletType, .Description = t.PalletTypeDescription}).ToArray()
                            Case Else
                                vList = (
                                     From t In db.vNGLAPIPalletTypes
                                     Select New DTO.vLookupList _
                                     With {.Control = t.ID, .Name = t.PalletType, .Description = t.PalletTypeDescription}).ToArray()
                        End Select

                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "AvailSSOAByUser" 'Modified by LVV for v-8.1 on 03/28/2018
                Using db As New NGLMASSecurityDataContext(ConnectionString)
                    Try

                        If Criteria Is Nothing Then Return Nothing
                        Dim Control As Integer = 0
                        If (Not Integer.TryParse(Criteria.ToString(), Control)) Then Throw New InvalidOperationException("Cannot get the Available Single Sign On Accounts By User Lookup list because the user control number criteria is not valid.")

                        ''Logic to test the ModDate Parameter
                        'Dim blnUseModDate As Boolean = False
                        'Dim dtStartDate As Date = Date.Now()
                        'If ModDate.HasValue Then
                        '    blnUseModDate = True
                        '    dtStartDate = ModDate.Value
                        'End If

                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.tblSingleSignOnAccounts
                                    Where Not db.tblSSOASecurityXrefs.Any(Function(x) x.SSOAControl = t.SSOAControl And x.UserSecurityControl = Control)
                                    Order By t.SSOAName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.SSOAControl, .Name = t.SSOAName, .Description = t.SSOADesc}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.tblSingleSignOnAccounts
                                    Where Not db.tblSSOASecurityXrefs.Any(Function(x) x.SSOAControl = t.SSOAControl And x.UserSecurityControl = Control)
                                    Order By t.SSOADesc
                                    Select New DTO.vLookupList _
                                    With {.Control = t.SSOAControl, .Name = t.SSOAName, .Description = t.SSOADesc}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.tblSingleSignOnAccounts
                                    Where Not db.tblSSOASecurityXrefs.Any(Function(x) x.SSOAControl = t.SSOAControl And x.UserSecurityControl = Control)
                                    Select New DTO.vLookupList _
                                    With {.Control = t.SSOAControl, .Name = t.SSOAName, .Description = t.SSOADesc}).ToArray()
                        End Select

                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "CalculationFactorType" 'Added By LVV on 6/11/18 for v-8.3 TMS365 Scheduler
                'Returns the Static List of all Calculation Factor Types 
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.tblCalculationFactorTypes
                                    Order By t.CalcFactorTypeName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CalcFactorTypeControl, .Name = t.CalcFactorTypeName, .Description = t.CalcFactorTypeDesc}).ToArray()
                            Case 2
                                vList = (
                                     From t In db.tblCalculationFactorTypes
                                     Order By t.CalcFactorTypeDesc
                                     Select New DTO.vLookupList _
                                     With {.Control = t.CalcFactorTypeControl, .Name = t.CalcFactorTypeName, .Description = t.CalcFactorTypeDesc}).ToArray()
                            Case Else
                                vList = (
                                     From t In db.tblCalculationFactorTypes
                                     Select New DTO.vLookupList _
                                     With {.Control = t.CalcFactorTypeControl, .Name = t.CalcFactorTypeName, .Description = t.CalcFactorTypeDesc}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "CalcFactorTypeForDock" 'Added By LVV on 6/26/18 for v-8.3 TMS365 Scheduler
                'Returns a vlookup list of CalculationFactorTypes filtered for the DockControl
                'sortKey = 1 --> get list for Add (only CalcFactorTypes still available for Dock)
                'sortKey = 2 --> get list for Edit (all CalcFactorTypes)
                'Caller has to check to make sure if nothing is returned then we can't add a new DockTimeCalcFactor record - only can edit existing 
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                'get list for Add (only CalcFactorTypes still available for Dock)
                                Dim oDockTime As New NGLDockTimeCalcFactorData(Parameters)
                                Dim excludeList As New List(Of Integer)
                                excludeList.Add(0)

                                If Criteria Is Nothing Then Return Nothing
                                Dim Control As Integer = 0
                                If (Not Integer.TryParse(Criteria.ToString(), Control)) Then Throw New InvalidOperationException("E_InvalidCalculationFactor")

                                'Check if we can add anymore of the type of config for this dock
                                If oDockTime.DockHasQuantityConfig(Control) Then excludeList.Add(1)
                                If oDockTime.DockHasAllWgtConfigs(Control) Then excludeList.Add(2)
                                If oDockTime.DockHasAllPkgConfigs(Control) Then excludeList.Add(3)
                                If oDockTime.DockHasAllCubeConfigs(Control) Then excludeList.Add(4)

                                vList = (From t In db.tblCalculationFactorTypes
                                         Where Not excludeList.Contains(t.CalcFactorTypeControl)
                                         Select New DTO.vLookupList _
                                             With {.Control = t.CalcFactorTypeControl, .Name = t.CalcFactorTypeName, .Description = t.CalcFactorTypeDesc}).ToArray()
                            Case 2
                                'get list for Edit (all CalcFactorTypes)
                                vList = (
                                     From t In db.tblCalculationFactorTypes
                                     Select New DTO.vLookupList _
                                     With {.Control = t.CalcFactorTypeControl, .Name = t.CalcFactorTypeName, .Description = t.CalcFactorTypeDesc}).ToArray()
                            Case Else
                                'default return is Edit list (return all)
                                vList = (
                                     From t In db.tblCalculationFactorTypes
                                     Select New DTO.vLookupList _
                                     With {.Control = t.CalcFactorTypeControl, .Name = t.CalcFactorTypeName, .Description = t.CalcFactorTypeDesc}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "CalcFactorTypeUOM" 'Added By LVV on 6/19/18 for v-8.3 TMS365 Scheduler
                'Returns a vlookup list of UOM for the ResourceControl And CalcType. Only returns the UOM if it Is Not already in use in the tblDock/TimeCalcFactors table for the Dock And CalcType
                'Criteria should be a string (csv) in the format "intDockControl,intCalcFactorTypeControl"
                'sortKey = 1 --> get list for Add (only UOM still available for Dock)
                'sortKey = 2 --> get list for Edit (all UOM)
                Using db As New NGLMASCompDataContext(ConnectionString)
                    Try
                        Dim oDockTime As New NGLDockTimeCalcFactorData(Parameters)
                        Dim intDockControl As Integer = 0
                        Dim intCalcFactorTypeControl As Integer = 0
                        'Get the control values from the Criteria string
                        Dim sArr = Criteria.ToString().Split(",")
                        If sArr Is Nothing OrElse sArr.Count < 2 Then Return vList
                        Integer.TryParse(sArr(0), intDockControl)
                        Integer.TryParse(sArr(1), intCalcFactorTypeControl)

                        If intDockControl = 0 Or intCalcFactorTypeControl = 0 Then Return vList

                        Select Case intCalcFactorTypeControl
                            Case 1 'Quantity
                                'Quantity does not have any associated UOMs so return blank string
                                Dim QtyUOMList As New List(Of DTO.vLookupList)
                                QtyUOMList.Add(New DTO.vLookupList(0, "", ""))
                                vList = (
                                     From t In QtyUOMList
                                     Select New DTO.vLookupList _
                                     With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                            Case 2 'Weight
                                Dim WgtUOMList As New List(Of DTO.vLookupList)
                                If sortKey = 1 Then
                                    'Add (Only return UOM that do not already have a config record for this Dock)
                                    If Not oDockTime.DoesCalcUOMConfigExist(db, intDockControl, intCalcFactorTypeControl, "LBS") Then WgtUOMList.Add(New DTO.vLookupList(1, "LBS", "Pounds"))
                                    If Not oDockTime.DoesCalcUOMConfigExist(db, intDockControl, intCalcFactorTypeControl, "KG") Then WgtUOMList.Add(New DTO.vLookupList(2, "KG", "Kilograms"))
                                Else
                                    'Edit (all UOM)
                                    WgtUOMList.Add(New DTO.vLookupList(1, "LBS", "Pounds"))
                                    WgtUOMList.Add(New DTO.vLookupList(2, "KG", "Kilograms"))
                                End If
                                vList = (
                                     From t In WgtUOMList
                                     Select New DTO.vLookupList _
                                     With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                            Case 3 'Package
                                If sortKey = 1 Then
                                    'Add (Only return UOM that do not already have a config record for this Dock)
                                    vList = (
                                         From t In db.udfGetPkgUOMAvailForDockTCF(intDockControl)
                                         Select New DTO.vLookupList _
                                         With {.Control = t.ID, .Name = t.PalletType, .Description = t.PalletTypeDescription}).ToArray()
                                Else
                                    'Edit (all UOM)
                                    vList = GetPalletTypesList()
                                End If
                            Case 4 'Cube
                                Dim CubeUOMList As New List(Of DTO.vLookupList)
                                If sortKey = 1 Then
                                    'Add (Only return UOM that do not already have a config record for this Dock)
                                    If Not oDockTime.DoesCalcUOMConfigExist(db, intDockControl, intCalcFactorTypeControl, "CUI") Then CubeUOMList.Add(New DTO.vLookupList(1, "CUI", "Cubic Inches"))
                                    If Not oDockTime.DoesCalcUOMConfigExist(db, intDockControl, intCalcFactorTypeControl, "CUFT") Then CubeUOMList.Add(New DTO.vLookupList(2, "CUFT", "Cubic Feet"))
                                    If Not oDockTime.DoesCalcUOMConfigExist(db, intDockControl, intCalcFactorTypeControl, "CCM") Then CubeUOMList.Add(New DTO.vLookupList(3, "CCM", "Cubic Centimeters"))
                                    If Not oDockTime.DoesCalcUOMConfigExist(db, intDockControl, intCalcFactorTypeControl, "CBM") Then CubeUOMList.Add(New DTO.vLookupList(4, "CBM", "Cubic Meters"))
                                Else
                                    'Edit (all UOM)
                                    CubeUOMList.Add(New DTO.vLookupList(1, "CUI", "Cubic Inches"))
                                    CubeUOMList.Add(New DTO.vLookupList(2, "CUFT", "Cubic Feet"))
                                    CubeUOMList.Add(New DTO.vLookupList(3, "CCM", "Cubic Centimeters"))
                                    CubeUOMList.Add(New DTO.vLookupList(4, "CBM", "Cubic Meters"))
                                End If
                                vList = (
                                     From t In CubeUOMList
                                     Select New DTO.vLookupList _
                                     With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "tblListType" 'Added By LVV on 6/19/18 for v-8.3 TMS365 Scheduler - Merge EDI Tool Changes
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.tblListTypes
                                    Order By t.ListTypeName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ListTypeControl, .Name = t.ListTypeName, .Description = t.ListTypeDesc.ToString}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.tblListTypes
                                    Order By t.ListTypeDesc
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ListTypeControl, .Name = t.ListTypeName, .Description = t.ListTypeDesc.ToString}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.tblListTypes
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ListTypeControl, .Name = t.ListTypeName, .Description = t.ListTypeDesc.ToString}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "tblEDIDataType" 'Added By LVV on 6/19/18 for v-8.3 TMS365 Scheduler - Merge EDI Tool Changes
                Using db As New NGLMASEDIMaintDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.tblEDIDataTypes
                                    Order By t.EDIDataTypeName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.EDIDataTypeControl, .Name = t.EDIDataTypeName, .Description = t.EDIDataTypeDesc.ToString}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.tblEDIDataTypes
                                    Order By t.EDIDataTypeDesc
                                    Select New DTO.vLookupList _
                                    With {.Control = t.EDIDataTypeControl, .Name = t.EDIDataTypeName, .Description = t.EDIDataTypeDesc.ToString}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.tblEDIDataTypes
                                    Select New DTO.vLookupList _
                                    With {.Control = t.EDIDataTypeControl, .Name = t.EDIDataTypeName, .Description = t.EDIDataTypeDesc.ToString}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "tblEDIFormattingFunctions" 'Added By LVV on 6/19/18 for v-8.3 TMS365 Scheduler - Merge EDI Tool Changes
                Using db As New NGLMASEDIMaintDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.tblEDIFormattingFunctions
                                    Order By t.FormattingFnName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.FormattingFnControl, .Name = t.FormattingFnName, .Description = t.FormattingFnDesc.ToString}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.tblEDIFormattingFunctions
                                    Order By t.FormattingFnDesc
                                    Select New DTO.vLookupList _
                                    With {.Control = t.FormattingFnControl, .Name = t.FormattingFnName, .Description = t.FormattingFnDesc.ToString}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.tblEDIFormattingFunctions
                                    Select New DTO.vLookupList _
                                    With {.Control = t.FormattingFnControl, .Name = t.FormattingFnName, .Description = t.FormattingFnDesc.ToString}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "tblEDIValidationTypes" 'Added By LVV on 6/19/18 for v-8.3 TMS365 Scheduler - Merge EDI Tool Changes
                Using db As New NGLMASEDIMaintDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.tblEDIValidationTypes
                                    Order By t.ValidationTypeName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ValidationTypeControl, .Name = t.ValidationTypeName, .Description = t.ValidationTypeDesc.ToString}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.tblEDIValidationTypes
                                    Order By t.ValidationTypeDesc
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ValidationTypeControl, .Name = t.ValidationTypeName, .Description = t.ValidationTypeDesc.ToString}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.tblEDIValidationTypes
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ValidationTypeControl, .Name = t.ValidationTypeName, .Description = t.ValidationTypeDesc.ToString}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "tblEDITransformationTypes" 'Added By LVV on 6/19/18 for v-8.3 TMS365 Scheduler - Merge EDI Tool Changes
                Using db As New NGLMASEDIMaintDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.tblEDITransformationTypes
                                    Order By t.TransformationTypeName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.TransformationTypeControl, .Name = t.TransformationTypeName, .Description = t.TransformationTypeDesc.ToString}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.tblEDITransformationTypes
                                    Order By t.TransformationTypeDesc
                                    Select New DTO.vLookupList _
                                    With {.Control = t.TransformationTypeControl, .Name = t.TransformationTypeName, .Description = t.TransformationTypeDesc.ToString}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.tblEDITransformationTypes
                                    Select New DTO.vLookupList _
                                    With {.Control = t.TransformationTypeControl, .Name = t.TransformationTypeName, .Description = t.TransformationTypeDesc.ToString}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "SystemTimeZones" 'Added By LVV on 7/10/18 for v-8.3 TMS365 Scheduler
                Using db As New NGLMASEDIMaintDataContext(ConnectionString)
                    Try
                        Dim timeZones = TimeZoneInfo.GetSystemTimeZones()

                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In timeZones
                                    Order By t.DisplayName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.Id, .Name = t.DisplayName, .Description = t.DisplayName}).ToArray()
                                'Case 2
                                '    vList = (
                                '        From t In timeZones
                                '        Order By t.DisplayName
                                '        Select New DTO.vLookupList _
                                '        With {.Control = t.Id, .Name = t.DisplayName, .Description = t.DisplayName}).ToArray()
                            Case Else
                                vList = (
                                    From t In timeZones
                                    Select New DTO.vLookupList _
                                    With {.Control = t.Id, .Name = t.DisplayName, .Description = t.DisplayName}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "SchedulerReasonCodes" 'Added By LVV on 8/2/18 for v-8.3 TMS365 Scheduler
                'Modified by RHR for v-8.2.0.117 on 8/11/2019 added sort by sequence no option
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.LoadStatusCodes
                                    Where t.LoadStatusLSCTControl = LoadStatusCodeTypes.SchedOverride
                                    Order By t.LoadStatusControl
                                    Select New DTO.vLookupList _
                                    With {.Control = t.LoadStatusControl, .Name = t.LoadStatusCode, .Description = t.LoadStatusDesc}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.LoadStatusCodes
                                    Where t.LoadStatusLSCTControl = LoadStatusCodeTypes.SchedOverride
                                    Order By t.LoadStatusDesc
                                    Select New DTO.vLookupList _
                                    With {.Control = t.LoadStatusControl, .Name = t.LoadStatusCode, .Description = t.LoadStatusDesc}).ToArray()
                            Case 4 'Modified by RHR for v-8.2.0.117 on 8/11/2019 - Added Order By LoadStatusSequenceNo (Also added ListSortType "SeqNo" = 4 to 365 enum
                                vList = (
                                    From t In db.LoadStatusCodes
                                    Where t.LoadStatusLSCTControl = LoadStatusCodeTypes.SchedOverride
                                    Order By t.LoadStatusSequenceNo
                                    Select New DTO.vLookupList _
                                    With {.Control = t.LoadStatusControl, .Name = t.LoadStatusCode, .Description = t.LoadStatusDesc}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.LoadStatusCodes
                                    Where t.LoadStatusLSCTControl = LoadStatusCodeTypes.SchedOverride
                                    Select New DTO.vLookupList _
                                    With {.Control = t.LoadStatusControl, .Name = t.LoadStatusCode, .Description = t.LoadStatusDesc}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "LaneTariff"
                Dim oLookup As New List(Of DTO.vLookupList)
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        If Criteria Is Nothing Then Return Nothing
                        Dim Control As Integer
                        If (Not Integer.TryParse(Criteria.ToString(), Control)) Then Throw New InvalidOperationException("E_InvalidLaneTariff")
                        'we assume the the user has access to the tariff so additional user admin filters are not required
                        ' Dim oSecureComp = From s In db.vUserAdminWithCompControlRefLanes Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                        Select Case sortKey
                            Case 1
                                oLookup = (
                                    From t In db.vLookupLaneTariffs
                                    Where t.CarrTarControl = Control And (ModDate.HasValue = False OrElse t.LaneModDate > ModDate)
                                    Order By t.LaneName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.LaneControl, .Name = t.LaneName, .Description = t.LaneNumber.ToString}).ToList()
                            Case 2
                                oLookup = (
                                    From t In db.vLookupLaneTariffs
                                    Where t.CarrTarControl = Control And (ModDate.HasValue = False OrElse t.LaneModDate > ModDate)
                                    Order By t.LaneNumber
                                    Select New DTO.vLookupList _
                                    With {.Control = t.LaneControl, .Name = t.LaneName, .Description = t.LaneNumber.ToString}).ToList()
                            Case Else
                                oLookup = (
                                    From t In db.vLookupLaneTariffs
                                    Where t.CarrTarControl = Control And (ModDate.HasValue = False OrElse t.LaneModDate > ModDate)
                                    Select New DTO.vLookupList _
                                    With {.Control = t.LaneControl, .Name = t.LaneName, .Description = t.LaneNumber.ToString}).ToList()
                        End Select
                        oLookup.Insert(0, New DTO.vLookupList With {.Control = 0, .Name = "ALL", .Description = "Any Lane"})
                        vList = oLookup.ToArray()

                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "BookingAcssCodes"
                Dim oLookup As New List(Of DTO.vLookupList)
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                oLookup = (
                                    From t In db.vLookupAcssCodes
                                    Order By t.NACCode
                                    Select New DTO.vLookupList _
                                    With {.Control = t.NACControl, .Name = t.NACCode, .Description = t.NACName}).ToList()
                            Case 2
                                oLookup = (
                                    From t In db.vLookupAcssCodes
                                    Order By t.NACName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.NACControl, .Name = t.NACCode, .Description = t.NACName}).ToList()
                            Case Else
                                oLookup = (
                                    From t In db.vLookupAcssCodes
                                    Select New DTO.vLookupList _
                                    With {.Control = t.NACControl, .Name = t.NACCode, .Description = t.NACName}).ToList()
                        End Select
                        vList = oLookup.ToArray()

                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "LEAcssCodes"
                Dim oLookup As New List(Of DTO.vLookupList)
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        ' Modified by RHR for v-8.2.0.109 on 2/20/2019
                        ' We no longer filter codes by carrier or legal entity.
                        ' All accessorials are available to all users so Criteria is not used
                        'If Criteria Is Nothing Then Return Nothing
                        'Dim Control As Integer
                        'If (Not Integer.TryParse(Criteria.ToString(), Control)) Then Throw New InvalidOperationException("E_InvalidLegalEntityForAccessorial")
                        'we assume the the user has access to the tariff so additional user admin filters are not required
                        ' Dim oSecureComp = From s In db.vUserAdminWithCompControlRefLanes Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl

                        'old querys like this were removed.
                        'oLookup = (
                        '            From t In db.vLookupAcssCodeByLegalEntities
                        '            Where t.LEAdminControl = Control
                        '            Order By t.NACCode
                        '            Select New DTO.vLookupList _
                        '            With {.Control = t.NACControl, .Name = t.NACName, .Description = t.NACCode}).ToList()
                        Select Case sortKey
                            Case 1
                                oLookup = (
                                    From t In db.vLookupAcssCodes
                                    Order By t.NACCode
                                    Select New DTO.vLookupList _
                                    With {.Control = t.NACControl, .Name = t.NACName, .Description = t.NACCode}).ToList()
                            Case 2
                                oLookup = (
                                    From t In db.vLookupAcssCodes
                                    Order By t.NACName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.NACControl, .Name = t.NACName, .Description = t.NACCode}).ToList()
                            Case Else
                                oLookup = (
                                    From t In db.vLookupAcssCodes
                                    Select New DTO.vLookupList _
                                    With {.Control = t.NACControl, .Name = t.NACName, .Description = t.NACCode}).ToList()
                        End Select
                        vList = oLookup.ToArray()
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "APIFrtClasses"
                Dim oLookup As New List(Of DTO.vLookupList)

                oLookup.Insert(0, New DTO.vLookupList With {.Control = 1, .Name = "50", .Description = "FAK Class 50"})
                oLookup.Insert(0, New DTO.vLookupList With {.Control = 2, .Name = "55", .Description = "FAK Class 55"})
                oLookup.Insert(0, New DTO.vLookupList With {.Control = 3, .Name = "60", .Description = "FAK Class 60"})
                oLookup.Insert(0, New DTO.vLookupList With {.Control = 4, .Name = "65", .Description = "FAK Class 65"})
                oLookup.Insert(0, New DTO.vLookupList With {.Control = 5, .Name = "70", .Description = "FAK Class 70"})
                oLookup.Insert(0, New DTO.vLookupList With {.Control = 6, .Name = "77.5", .Description = "FAK Class 77.5"})
                oLookup.Insert(0, New DTO.vLookupList With {.Control = 7, .Name = "85", .Description = "FAK Class 85"})
                oLookup.Insert(0, New DTO.vLookupList With {.Control = 8, .Name = "92.5", .Description = "FAK Class 92.5"})
                oLookup.Insert(0, New DTO.vLookupList With {.Control = 9, .Name = "100", .Description = "FAK Class 100"})
                oLookup.Insert(0, New DTO.vLookupList With {.Control = 10, .Name = "110", .Description = "FAK Class 110"})
                oLookup.Insert(0, New DTO.vLookupList With {.Control = 11, .Name = "125", .Description = "FAK Class 125"})
                oLookup.Insert(0, New DTO.vLookupList With {.Control = 12, .Name = "150", .Description = "FAK Class 150"})
                oLookup.Insert(0, New DTO.vLookupList With {.Control = 13, .Name = "175", .Description = "FAK Class 175"})
                oLookup.Insert(0, New DTO.vLookupList With {.Control = 14, .Name = "200", .Description = "FAK Class 200"})
                oLookup.Insert(0, New DTO.vLookupList With {.Control = 15, .Name = "250", .Description = "FAK Class 250"})
                oLookup.Insert(0, New DTO.vLookupList With {.Control = 16, .Name = "300", .Description = "FAK Class 300"})
                oLookup.Insert(0, New DTO.vLookupList With {.Control = 17, .Name = "400", .Description = "FAK Class 400"})
                oLookup.Insert(0, New DTO.vLookupList With {.Control = 18, .Name = "500", .Description = "FAK Class 500"})

                vList = oLookup.ToArray()
            Case "APIWeightUnit"
                Dim oLookup As New List(Of DTO.vLookupList)
                oLookup.Insert(0, New DTO.vLookupList With {.Control = 1, .Name = "LB", .Description = "Shipping Weight in Pounds"})
                oLookup.Insert(0, New DTO.vLookupList With {.Control = 2, .Name = "KG", .Description = "Shipping Weight in Kilograms"})
                vList = oLookup.ToArray()
            Case "APILengthUnit"
                Dim oLookup As New List(Of DTO.vLookupList)
                oLookup.Insert(0, New DTO.vLookupList With {.Control = 1, .Name = "IN", .Description = "Shipping Dimensions in Inches"})
                oLookup.Insert(0, New DTO.vLookupList With {.Control = 2, .Name = "CM", .Description = "Shipping Dimensions in Centimeters"})
                vList = oLookup.ToArray()

            Case "Accessorials"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.tblAccessorials
                                    Order By t.AccessorialName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.AccessorialCode, .Name = t.AccessorialName, .Description = t.AccessorialDescription}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.tblAccessorials
                                    Order By t.AccessorialDescription
                                    Select New DTO.vLookupList _
                                    With {.Control = t.AccessorialCode, .Name = t.AccessorialName, .Description = t.AccessorialDescription}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.tblAccessorials
                                    Select New DTO.vLookupList _
                                    With {.Control = t.AccessorialCode, .Name = t.AccessorialName, .Description = t.AccessorialDescription}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using

            Case "AvailLECarrier" 'Added By LVV on 1/21/19 --> Returns list of Carriers that have not yet been assigned to the LECarrier table for the provided LE
                Using db As New NGLMASCarrierDataContext(ConnectionString)
                    Try
                        If Criteria Is Nothing Then Return Nothing
                        Dim LEAControl As Integer
                        If (Not Integer.TryParse(Criteria.ToString(), LEAControl)) Then Throw New InvalidOperationException("E_InvalidLegalEntityForCarrier")

                        Dim assigned = db.tblLegalEntityCarriers.Where(Function(x) x.LECarLEAdminControl = LEAControl).Select(Function(y) y.LECarCarrierControl).ToList()
                        'Modified by RHR for v-8.5.4.004 on 01/23/2024 we now limit to Active Carriers only
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.Carriers
                                    Where (Not assigned.Contains(t.CarrierControl)) _
                                    And
                                    (ModDate.HasValue = False OrElse t.CarrierModDate > ModDate) _
                                    And t.CarrierActive = True
                                    Order By t.CarrierName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrierControl, .Name = t.CarrierName, .Description = t.CarrierNumber.ToString}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.Carriers
                                    Where (Not assigned.Contains(t.CarrierControl)) _
                                    And
                                    (ModDate.HasValue = False OrElse t.CarrierModDate > ModDate) _
                                    And t.CarrierActive = True
                                    Order By t.CarrierNumber
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrierControl, .Name = t.CarrierName, .Description = t.CarrierNumber.ToString}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.Carriers
                                    Where (Not assigned.Contains(t.CarrierControl)) _
                                    And
                                    (ModDate.HasValue = False OrElse t.CarrierModDate > ModDate) _
                                    And t.CarrierActive = True
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrierControl, .Name = t.CarrierName, .Description = t.CarrierNumber.ToString}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "LECarrier" 'Added By LVV on 1/21/19 --> Returns list of Carriers that have been assigned to the LECarrier table for the provided LE
                Using db As New NGLMASCarrierDataContext(ConnectionString)
                    Try
                        If Criteria Is Nothing Then Return Nothing
                        Dim LEAControl As Integer
                        If (Not Integer.TryParse(Criteria.ToString(), LEAControl)) Then Throw New InvalidOperationException("E_InvalidLegalEntityForCarrier")

                        Dim assigned = db.tblLegalEntityCarriers.Where(Function(x) x.LECarLEAdminControl = LEAControl).Select(Function(y) y.LECarCarrierControl).ToList()

                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.Carriers
                                    Where (assigned.Contains(t.CarrierControl)) _
                                    And
                                    (ModDate.HasValue = False OrElse t.CarrierModDate > ModDate)
                                    Order By t.CarrierName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrierControl, .Name = t.CarrierName, .Description = t.CarrierNumber.ToString}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.Carriers
                                    Where (assigned.Contains(t.CarrierControl)) _
                                    And
                                    (ModDate.HasValue = False OrElse t.CarrierModDate > ModDate)
                                    Order By t.CarrierNumber
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrierControl, .Name = t.CarrierName, .Description = t.CarrierNumber.ToString}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.Carriers
                                    Where (assigned.Contains(t.CarrierControl)) _
                                    And
                                    (ModDate.HasValue = False OrElse t.CarrierModDate > ModDate)
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrierControl, .Name = t.CarrierName, .Description = t.CarrierNumber.ToString}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "LECarrierOrAny" 'Added By LVV on 1/21/19 --> Returns Any for zero and a list of Carriers that have been assigned to the LECarrier table for the provided LE
                Using db As New NGLMASCarrierDataContext(ConnectionString)
                    Try
                        If Criteria Is Nothing Then Return Nothing
                        Dim LEAControl As Integer
                        If (Not Integer.TryParse(Criteria.ToString(), LEAControl)) Then Throw New InvalidOperationException("E_InvalidLegalEntityForCarrier")

                        Dim assigned = db.tblLegalEntityCarriers.Where(Function(x) x.LECarLEAdminControl = LEAControl).Select(Function(y) y.LECarCarrierControl).ToList()
                        Dim lookupList As List(Of DTO.vLookupList)
                        Select Case sortKey
                            Case 1
                                lookupList = (
                                    From t In db.Carriers
                                    Where (assigned.Contains(t.CarrierControl)) _
                                    And
                                    (ModDate.HasValue = False OrElse t.CarrierModDate > ModDate)
                                    Order By t.CarrierName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrierControl, .Name = t.CarrierName, .Description = t.CarrierNumber.ToString}).ToList()
                            Case 2
                                lookupList = (
                                    From t In db.Carriers
                                    Where (assigned.Contains(t.CarrierControl)) _
                                    And
                                    (ModDate.HasValue = False OrElse t.CarrierModDate > ModDate)
                                    Order By t.CarrierNumber
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrierControl, .Name = t.CarrierName, .Description = t.CarrierNumber.ToString}).ToList()
                            Case Else
                                lookupList = (
                                    From t In db.Carriers
                                    Where (assigned.Contains(t.CarrierControl)) _
                                    And
                                    (ModDate.HasValue = False OrElse t.CarrierModDate > ModDate)
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrierControl, .Name = t.CarrierName, .Description = t.CarrierNumber.ToString}).ToList()
                        End Select
                        Dim itemDefault As New DTO.vLookupList(0, "Any", "Any or All")
                        If lookupList Is Nothing Then lookupList = New List(Of DTO.vLookupList)
                        lookupList.Insert(0, itemDefault)
                        vList = lookupList.ToArray()

                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "AllCarriers" 'Added by LVV on 4/4/19 - I needed a list that would return all carriers in the database without apply any filters like user settings or LE
                Using db As New NGLMASCarrierDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.Carriers
                                    Order By t.CarrierName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrierControl, .Name = t.CarrierName, .Description = t.CarrierNumber.ToString}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.Carriers
                                    Order By t.CarrierNumber
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrierControl, .Name = t.CarrierName, .Description = t.CarrierNumber.ToString}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.Carriers
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CarrierControl, .Name = t.CarrierName, .Description = t.CarrierNumber.ToString}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "NatFuelZones" 'Added by ManoRama On 25-AUG-2020 for Carrier Fuel Index Maint Changes
                Using db As New NGLMASLookupDataContext(ConnectionString)

                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.NatFuelZones
                                    Order By t.NatFuelZoneName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.NatFuelZoneID, .Name = t.NatFuelZoneName, .Description = t.NatFuelZoneDesc.ToString}).ToArray()
                            Case 2
                                vList = (
                                   From t In db.NatFuelZones
                                   Order By t.NatFuelZoneName
                                   Select New DTO.vLookupList _
                                    With {.Control = t.NatFuelZoneID, .Name = t.NatFuelZoneName, .Description = t.NatFuelZoneDesc.ToString}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.NatFuelZones
                                    Select New DTO.vLookupList _
                                    With {.Control = t.NatFuelZoneID, .Name = t.NatFuelZoneName, .Description = t.NatFuelZoneDesc.ToString}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "LegalEntities" 'Added by LVV on 4/10/19 - I needed a list that would return all LEAdmins in the database plus "None"
                Using db As New NGLMASCompDataContext(ConnectionString)
                    Try
                        Dim lookupList As New List(Of DTO.vLookupList)
                        Select Case sortKey
                            Case 1
                                lookupList = (
                                    From t In db.vLegalEntityAdmins
                                    Order By t.LegalEntity
                                    Select New DTO.vLookupList _
                                    With {.Control = t.LEAdminControl, .Name = t.LegalEntity, .Description = t.LECompControl.ToString}).ToList()
                            Case 2
                                lookupList = (
                                    From t In db.vLegalEntityAdmins
                                    Order By t.CompName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.LEAdminControl, .Name = t.LegalEntity, .Description = t.LECompControl.ToString}).ToList()
                            Case Else
                                lookupList = (
                                    From t In db.vLegalEntityAdmins
                                    Select New DTO.vLookupList _
                                    With {.Control = t.LEAdminControl, .Name = t.LegalEntity, .Description = t.LECompControl.ToString}).ToList()
                        End Select
                        Dim itemDefault As New DTO.vLookupList(0, "Carrier Only", "0")
                        If lookupList Is Nothing Then lookupList = New List(Of DTO.vLookupList)
                        'lookupList.Add(itemDefault)
                        lookupList.Insert(0, itemDefault)
                        vList = lookupList.ToArray()
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "UserGroupCat" 'Added by LVV on 4/10/19
                Using db As New NGLMASSecurityDataContext(ConnectionString)
                    Try
                        If Parameters.CatControl = 4 Then
                            'SuperUsers can see all cats
                            Select Case sortKey
                                Case 1
                                    vList = (
                                        From t In db.tblUserGroupCategories
                                        Order By t.UGCName
                                        Select New DTO.vLookupList _
                                        With {.Control = t.UGCControl, .Name = t.UGCName, .Description = t.UGCDesc.ToString}).ToArray()
                                Case 2
                                    vList = (
                                        From t In db.tblUserGroupCategories
                                        Order By t.UGCDesc
                                        Select New DTO.vLookupList _
                                        With {.Control = t.UGCControl, .Name = t.UGCName, .Description = t.UGCDesc.ToString}).ToArray()
                                Case Else
                                    vList = (
                                        From t In db.tblUserGroupCategories
                                        Select New DTO.vLookupList _
                                        With {.Control = t.UGCControl, .Name = t.UGCName, .Description = t.UGCDesc.ToString}).ToArray()
                            End Select
                        Else
                            'everyone else can only see limited data
                            Dim allowedCats As New List(Of Integer) From {1, 3}
                            Select Case sortKey
                                Case 1
                                    vList = (
                                        From t In db.tblUserGroupCategories
                                        Where allowedCats.Contains(t.UGCControl)
                                        Order By t.UGCName
                                        Select New DTO.vLookupList _
                                        With {.Control = t.UGCControl, .Name = t.UGCName, .Description = t.UGCDesc.ToString}).ToArray()
                                Case 2
                                    vList = (
                                        From t In db.tblUserGroupCategories
                                        Where allowedCats.Contains(t.UGCControl)
                                        Order By t.UGCDesc
                                        Select New DTO.vLookupList _
                                        With {.Control = t.UGCControl, .Name = t.UGCName, .Description = t.UGCDesc.ToString}).ToArray()
                                Case Else
                                    vList = (
                                        From t In db.tblUserGroupCategories
                                        Where allowedCats.Contains(t.UGCControl)
                                        Select New DTO.vLookupList _
                                        With {.Control = t.UGCControl, .Name = t.UGCName, .Description = t.UGCDesc.ToString}).ToArray()
                            End Select
                        End If
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "LECompControls" 'Added by LVV on 4/12/19 - I needed a list that would return all LEAdmins in the database plus "None" and the control is LEAdminCompControl instead of LEAdminControl (some tables are stupid and use CompControl to link to LE instead of the LEAControl)
                Using db As New NGLMASCompDataContext(ConnectionString)
                    Try
                        Dim lookupList As New List(Of DTO.vLookupList)
                        Select Case sortKey
                            Case 1
                                lookupList = (
                                    From t In db.vLegalEntityAdmins
                                    Order By t.LegalEntity
                                    Select New DTO.vLookupList _
                                    With {.Control = t.LECompControl, .Name = t.LegalEntity, .Description = t.LEAdminControl.ToString}).ToList()
                            Case 2
                                lookupList = (
                                    From t In db.vLegalEntityAdmins
                                    Order By t.CompName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.LECompControl, .Name = t.LegalEntity, .Description = t.LEAdminControl.ToString}).ToList()
                            Case Else
                                lookupList = (
                                    From t In db.vLegalEntityAdmins
                                    Select New DTO.vLookupList _
                                    With {.Control = t.LECompControl, .Name = t.LegalEntity, .Description = t.LEAdminControl.ToString}).ToList()
                        End Select
                        Dim itemDefault As New DTO.vLookupList(0, "None", "0")
                        If lookupList Is Nothing Then lookupList = New List(Of DTO.vLookupList)
                        'lookupList.Add(itemDefault)
                        lookupList.Insert(0, itemDefault)
                        vList = lookupList.ToArray()
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "PackageDescription"
                Dim oLookup As New List(Of DTO.vLookupList)
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Dim qLookup As New List(Of DTO.vLookupList)
                        ' Created by RHR for v-8.2. on 04/30/2019
                        Dim iPkgDescLEAdminControl As Integer = 0
                        iPkgDescLEAdminControl = Me.Parameters.UserLEControl
                        If iPkgDescLEAdminControl <> 0 Then
                            Select Case sortKey
                                Case 1
                                    qLookup = (
                                    From t In db.tblPackageDescriptions
                                    Where t.PkgDescLEAdminControl = iPkgDescLEAdminControl
                                    Order By t.PkgDescName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.PkgDescControl, .Name = t.PkgDescName, .Description = t.PkgDescDesc}).ToList()
                                Case 2
                                    qLookup = (
                                    From t In db.tblPackageDescriptions
                                    Where t.PkgDescLEAdminControl = iPkgDescLEAdminControl
                                    Order By t.PkgDescDesc
                                    Select New DTO.vLookupList _
                                    With {.Control = t.PkgDescControl, .Name = t.PkgDescName, .Description = t.PkgDescDesc}).ToList()
                                Case Else
                                    qLookup = (
                                    From t In db.tblPackageDescriptions
                                    Where t.PkgDescLEAdminControl = iPkgDescLEAdminControl
                                    Order By t.PkgDescControl
                                    Select New DTO.vLookupList _
                                    With {.Control = t.PkgDescControl, .Name = t.PkgDescName, .Description = t.PkgDescDesc}).ToList()
                            End Select
                        End If
                        oLookup.Add(New DTO.vLookupList(0, "None", "No package description has been selected"))
                        If Not qLookup Is Nothing AndAlso qLookup.Count() > 0 Then oLookup.AddRange(qLookup)
                        vList = oLookup.ToArray()
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "CMVisibility" 'Added by LVV on 7/26/19
                Dim oLookup As New List(Of DTO.vLookupList)
                oLookup.Insert(0, New DTO.vLookupList With {.Control = 0, .Name = "SyncWithGrid", .Description = "Sync With Grid"})
                oLookup.Insert(1, New DTO.vLookupList With {.Control = 1, .Name = "ShowAlways", .Description = "Always Show"})
                oLookup.Insert(2, New DTO.vLookupList With {.Control = 2, .Name = "HideAlways", .Description = "Always Hide"})
                vList = oLookup.ToArray()
            Case "APAuditFltrs" 'Added by LVV on 8/1/19
                Dim oLookup As New List(Of DTO.vLookupList)
                oLookup.Insert(0, New DTO.vLookupList With {.Control = 0, .Name = "Normal", .Description = "Normal"})
                oLookup.Insert(1, New DTO.vLookupList With {.Control = 1, .Name = "Matched", .Description = "Matched"})
                oLookup.Insert(2, New DTO.vLookupList With {.Control = 2, .Name = "Approved", .Description = "Approved"})
                oLookup.Insert(3, New DTO.vLookupList With {.Control = 3, .Name = "Electronic", .Description = "Electronic"})
                oLookup.Insert(4, New DTO.vLookupList With {.Control = 4, .Name = "AllErrors", .Description = "All Errors"})
                oLookup.Insert(5, New DTO.vLookupList With {.Control = 5, .Name = "PA", .Description = "Pending Approval"})
                vList = oLookup.ToArray()
            Case "UserLEComps" 'Added By LVV on 132/13/19 - Gets a list of Companies associated with the logged in user's Legal Entity and filters using Role Center security (company restrictions)
                'Modified by RHR or v-8.4.0.003 on 06/01/2021 added comp restrictions via new UserSecurityControl in vLEComp365s 
                Using db As New NGLMASCompDataContext(ConnectionString)
                    Try
                        Dim list As New List(Of DTO.vLookupList)
                        Select Case sortKey
                            Case 1
                                list = (
                                    From t In db.vLEComp365s
                                    Where
                                        t.LEAdminControl = Parameters.UserLEControl _
                                        And t.UserSecurityControl = Me.Parameters.UserControl _
                                        And (ModDate.HasValue = False OrElse t.CompModDate > ModDate)
                                    Order By t.CompName
                                    Select New DTO.vLookupList With {.Control = t.CompControl, .Name = t.CompName, .Description = t.CompNumber.ToString}).ToList()
                            Case 2
                                list = (
                                    From t In db.vLEComp365s
                                    Where
                                        t.LEAdminControl = Parameters.UserLEControl _
                                        And t.UserSecurityControl = Me.Parameters.UserControl _
                                        And (ModDate.HasValue = False OrElse t.CompModDate > ModDate)
                                    Order By t.CompNumber
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CompControl, .Name = t.CompName, .Description = t.CompNumber.ToString}).ToList()
                            Case Else
                                list = (
                                    From t In db.vLEComp365s
                                    Where
                                        t.LEAdminControl = Parameters.UserLEControl _
                                        And t.UserSecurityControl = Me.Parameters.UserControl _
                                        And (ModDate.HasValue = False OrElse t.CompModDate > ModDate)
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CompControl, .Name = t.CompName, .Description = t.CompNumber.ToString}).ToList()
                        End Select
                        list.Insert(0, New DTO.vLookupList With {.Control = 0, .Name = "None", .Description = "NA"})
                        vList = list.ToArray()
                    Catch ex As SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "NGLExpenseCarrier" 'Added By LVV on 12/27/19 - Gets a list of all NGL "Expense Carriers" aka how Mickey And Bill pay utilities etc.
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        vList = (
                            From t In db.spGetLookupListNGLExpenseCarrier(sortKey, Me.Parameters.UserName)
                            Select New DTO.vLookupList With {.Control = t.CarrierControl, .Name = t.CarrierName, .Description = t.CarrierNumber.ToString}
                        ).ToArray()
                    Catch ex As SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "FeatureType" 'Added By LVV on 2/20/20
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.tblFeatureTypes
                                    Order By t.FeatureTypeName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.FeatureTypeControl, .Name = t.FeatureTypeName, .Description = t.FeatureTypeDesc}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.tblFeatureTypes
                                    Order By t.FeatureTypeDesc
                                    Select New DTO.vLookupList _
                                    With {.Control = t.FeatureTypeControl, .Name = t.FeatureTypeName, .Description = t.FeatureTypeDesc}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.tblFeatureTypes
                                    Select New DTO.vLookupList _
                                    With {.Control = t.FeatureTypeControl, .Name = t.FeatureTypeName, .Description = t.FeatureTypeDesc}).ToArray()
                        End Select
                    Catch ex As SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "Version" 'Added By LVV on 2/20/20
                Using db As New NGLMASSYSDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.Versions
                                    Order By t.Version
                                    Select New DTO.vLookupList _
                                    With {.Control = 0, .Name = t.Version, .Description = t.VersionDescription}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.Versions
                                    Order By t.VersionDescription
                                    Select New DTO.vLookupList _
                                    With {.Control = 0, .Name = t.Version, .Description = t.VersionDescription}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.Versions
                                    Select New DTO.vLookupList _
                                    With {.Control = 0, .Name = t.Version, .Description = t.VersionDescription}).ToArray()
                        End Select
                    Catch ex As SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "APReductionReasonCodes"   'Added By LVV on 4/6/20 for v-8.2.1.006
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Dim list As New List(Of DTO.vLookupList)
                        Select Case sortKey
                            Case 1
                                list = (
                                    From t In db.LoadStatusCodes
                                    Where t.LoadStatusLSCTControl = LoadStatusCodeTypes.Reduction
                                    Order By t.LoadStatusControl
                                    Select New DTO.vLookupList With {.Control = t.LoadStatusControl, .Name = t.LoadStatusCode, .Description = t.LoadStatusDesc}).ToList()
                            Case 2
                                list = (
                                    From t In db.LoadStatusCodes
                                    Where t.LoadStatusLSCTControl = LoadStatusCodeTypes.Reduction
                                    Order By t.LoadStatusDesc
                                    Select New DTO.vLookupList With {.Control = t.LoadStatusControl, .Name = t.LoadStatusCode, .Description = t.LoadStatusDesc}).ToList()
                            Case 4
                                list = (
                                    From t In db.LoadStatusCodes
                                    Where t.LoadStatusLSCTControl = LoadStatusCodeTypes.Reduction
                                    Order By t.LoadStatusSequenceNo
                                    Select New DTO.vLookupList With {.Control = t.LoadStatusControl, .Name = t.LoadStatusCode, .Description = t.LoadStatusDesc}).ToList()
                            Case Else
                                list = (
                                    From t In db.LoadStatusCodes
                                    Where t.LoadStatusLSCTControl = LoadStatusCodeTypes.Reduction
                                    Select New DTO.vLookupList With {.Control = t.LoadStatusControl, .Name = t.LoadStatusCode, .Description = t.LoadStatusDesc}).ToList()
                        End Select
                        list.Insert(0, New DTO.vLookupList With {.Control = 0, .Name = "", .Description = "None"})
                        vList = list.ToArray()
                    Catch ex As SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "TransLeadTimeCalcTypes" 'Created by RHR for v-8.4.0.003 on 07/21/21 Colortech Enhancement Transit Time 
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Dim ListItemList As New List(Of DTO.vLookupList)

                        ListItemList.Add(New DTO.vLookupList(0, "None", "Do not calculate Lane Lead Times on Import"))
                        ListItemList.Add(New DTO.vLookupList(1, "Calculate Ship Date", "Calculate Ship Date on Import using Lead Time"))
                        ListItemList.Add(New DTO.vLookupList(2, "Calculate Required Date", "Calculate Required Date on Import using Lead Time"))
                        Select Case sortKey
                            Case 1
                                vList = (
                                From t In ListItemList
                                Order By t.Name
                                Select New DTO.vLookupList _
                                With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                            Case 2
                                vList = (
                                 From t In ListItemList
                                 Order By t.Description
                                 Select New DTO.vLookupList _
                                 With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                            Case Else
                                vList = (
                                 From t In ListItemList
                                 Select New DTO.vLookupList _
                                 With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "TransLeadTimeLocationOptions" 'Created by RHR for v-8.4.0.003 on 07/21/21 Colortech Enhancement Transit Time 
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Dim ListItemList As New List(Of DTO.vLookupList)
                        ListItemList.Add(New DTO.vLookupList(0, "None", "Do not compare similar locations for statistical analysis"))
                        ListItemList.Add(New DTO.vLookupList(1, "Compare By State", "Compare similar locations by state for statistical analysis"))
                        ListItemList.Add(New DTO.vLookupList(2, "Compare By City", "Compare similar locations by city for statistical analysis"))
                        ListItemList.Add(New DTO.vLookupList(2, "Compare By Postal Code", "Compare similar locations by 3 digit postal code for statistical analysis"))
                        Select Case sortKey
                            Case 1
                                vList = (
                                From t In ListItemList
                                Order By t.Name
                                Select New DTO.vLookupList _
                                With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                            Case 2
                                vList = (
                                 From t In ListItemList
                                 Order By t.Description
                                 Select New DTO.vLookupList _
                                 With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                            Case Else
                                vList = (
                                 From t In ListItemList
                                 Select New DTO.vLookupList _
                                 With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "TaskTypes" 'Created by RHR for v-8.5.0.001 on 09/22/21 
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Dim ListItemList As New List(Of DTO.vLookupList)
                        ListItemList.Add(New DTO.vLookupList(0, "ExecutableFile", "Run a command line executable on the application server"))
                        ListItemList.Add(New DTO.vLookupList(1, "StoredProcedure", "Execute a SQL Stored procedure that does not return any data"))
                        ListItemList.Add(New DTO.vLookupList(2, "AzureAppService", "Run an AzureAppService command"))
                        Select Case sortKey
                            Case 1
                                vList = (
                                From t In ListItemList
                                Order By t.Name
                                Select New DTO.vLookupList _
                                With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                            Case 2
                                vList = (
                                 From t In ListItemList
                                 Order By t.Description
                                 Select New DTO.vLookupList _
                                 With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                            Case Else
                                vList = (
                                 From t In ListItemList
                                 Select New DTO.vLookupList _
                                 With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "TaskMinutes" 'Created by RHR for v-8.5.0.001 on 09/22/21 
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Dim ListItemList As New List(Of DTO.vLookupList)
                        For intCt As Integer = 0 To 59
                            ListItemList.Add(New DTO.vLookupList(intCt, intCt.ToString(), intCt.ToString() & " Minutes past the hour"))
                        Next
                        ListItemList.Add(New DTO.vLookupList(60, "Every 5 Minutes", "Every 5 minutes all day"))
                        ListItemList.Add(New DTO.vLookupList(61, "Every 10 Minutes", "Every 10 minutes all day"))
                        ListItemList.Add(New DTO.vLookupList(62, "Every 15 Minutes", "Every 15 minutes all day"))
                        Select Case sortKey
                            Case 1
                                vList = (
                                From t In ListItemList
                                Order By t.Name
                                Select New DTO.vLookupList _
                                With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                            Case 2
                                vList = (
                                 From t In ListItemList
                                 Order By t.Description
                                 Select New DTO.vLookupList _
                                 With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                            Case Else
                                vList = (
                                 From t In ListItemList
                                 Select New DTO.vLookupList _
                                 With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "TaskHours" 'Created by RHR for v-8.5.0.001 on 09/22/21 
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Dim ListItemList As New List(Of DTO.vLookupList)
                        Dim strHour As String = ""
                        For intCt As Integer = 0 To 23

                            If intCt < 12 Then
                                strHour = intCt & " am"
                            Else
                                strHour = intCt - 12 & " pm"
                            End If
                            ListItemList.Add(New DTO.vLookupList(intCt, strHour, strHour))
                        Next
                        ListItemList.Add(New DTO.vLookupList(24, "Every Hour", "Every hour all day"))
                        ListItemList.Add(New DTO.vLookupList(25, "Every 2 Hours", "Every 2 hours all day"))
                        ListItemList.Add(New DTO.vLookupList(26, "Every 4 Hours", "Every 4 hours all day"))
                        ListItemList.Add(New DTO.vLookupList(27, "Every 6 Hours", "Every 6 hours all day"))
                        ListItemList.Add(New DTO.vLookupList(28, "Every 12 Hours", "Every 12 hours all day"))
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In ListItemList
                                    Order By t.Name
                                    Select New DTO.vLookupList _
                                    With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                            Case 2
                                vList = (
                                     From t In ListItemList
                                     Order By t.Description
                                     Select New DTO.vLookupList _
                                     With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                            Case Else
                                vList = (
                                     From t In ListItemList
                                     Select New DTO.vLookupList _
                                     With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "TaskDays" 'Created by RHR for v-8.5.0.001 on 09/22/21 
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Dim ListItemList As New List(Of DTO.vLookupList)
                        ListItemList.Add(New DTO.vLookupList(0, "Every Day", "Every day"))
                        For intCt As Integer = 1 To 31
                            ListItemList.Add(New DTO.vLookupList(intCt, intCt.ToString(), "On day " & intCt.ToString() & " of the month"))
                        Next
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In ListItemList
                                    Order By t.Name
                                    Select New DTO.vLookupList _
                                    With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                            Case 2
                                vList = (
                                     From t In ListItemList
                                     Order By t.Description
                                     Select New DTO.vLookupList _
                                     With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                            Case Else
                                vList = (
                                     From t In ListItemList
                                     Select New DTO.vLookupList _
                                     With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "TaskWeekDays" 'Created by RHR for v-8.5.0.001 on 09/22/21 
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Dim ListItemList As New List(Of DTO.vLookupList)
                        ListItemList.Add(New DTO.vLookupList(0, "Every Sunday", "Every Sunday"))
                        ListItemList.Add(New DTO.vLookupList(1, "Every Monday", "Every Monday"))
                        ListItemList.Add(New DTO.vLookupList(2, "Every Tuesday", "Every Tuesday"))
                        ListItemList.Add(New DTO.vLookupList(3, "Every Wednesday", "Every Wednesday"))
                        ListItemList.Add(New DTO.vLookupList(4, "Every Thursday", "Every Thursday"))
                        ListItemList.Add(New DTO.vLookupList(5, "Every Friday", "Every Friday"))
                        ListItemList.Add(New DTO.vLookupList(6, "Every Saturday", "Every Saturday"))
                        ListItemList.Add(New DTO.vLookupList(7, "Use Days Only", "Use Days Only"))

                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In ListItemList
                                    Order By t.Name
                                    Select New DTO.vLookupList _
                                    With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                            Case 2
                                vList = (
                                     From t In ListItemList
                                     Order By t.Description
                                     Select New DTO.vLookupList _
                                     With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                            Case Else
                                vList = (
                                     From t In ListItemList
                                     Select New DTO.vLookupList _
                                     With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "TaskMonths" 'Created by RHR for v-8.5.0.001 on 09/22/21 
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Dim ListItemList As New List(Of DTO.vLookupList)
                        ListItemList.Add(New DTO.vLookupList(0, "Every Month", "Every Month"))
                        For intCt As Integer = 1 To 12
                            ListItemList.Add(New DTO.vLookupList(intCt, intCt.ToString(), intCt.ToString()))
                        Next
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In ListItemList
                                    Order By t.Name
                                    Select New DTO.vLookupList _
                                    With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                            Case 2
                                vList = (
                                     From t In ListItemList
                                     Order By t.Description
                                     Select New DTO.vLookupList _
                                     With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                            Case Else
                                vList = (
                                     From t In ListItemList
                                     Select New DTO.vLookupList _
                                     With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "TariffAvailableHDM" 'Added By RHR for v-8.5.0.001 on 11/08/2021 -->  Used to select the available HDM Fees not already assigned to the calling tariff ; the caller must filter by CarrTarControl
                Using db As New NGLMASLaneDataContext(ConnectionString)
                    Try
                        If Criteria Is Nothing Then Return Nothing
                        Dim iTariffControl As Integer
                        If (Not Integer.TryParse(Criteria.ToString(), iTariffControl)) Then Throw New InvalidOperationException("E_InvalidTariffKeyForAvailHDM")

                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.vtblHDMTariffAvailables
                                    Where (t.CarrTarControl = iTariffControl)
                                    Order By t.HDMName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.HDMControl, .Name = t.HDMName, .Description = t.HDMDesc}).ToArray()
                            Case 2
                                vList = (
                                   From t In db.vtblHDMTariffAvailables
                                   Where (t.CarrTarControl = iTariffControl)
                                   Order By t.HDMDesc
                                   Select New DTO.vLookupList _
                                   With {.Control = t.HDMControl, .Name = t.HDMName, .Description = t.HDMDesc}).ToArray()
                            Case Else
                                vList = (
                                   From t In db.vtblHDMTariffAvailables
                                   Where (t.CarrTarControl = iTariffControl)
                                   Select New DTO.vLookupList _
                                   With {.Control = t.HDMControl, .Name = t.HDMName, .Description = t.HDMDesc}).ToArray()
                        End Select

                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "ParProcOptType"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Dim ListItemList As New List(Of DTO.vLookupList)
                        For Each iVal As ParProcOptType In [Enum].GetValues(GetType(ParProcOptType))
                            ListItemList.Add(New DTO.vLookupList(CInt(iVal), iVal.ToString(), iVal.ToString()))
                        Next
                        Select Case sortKey
                            Case 1
                                vList = (
                                From t In ListItemList
                                Order By t.Name
                                Select New DTO.vLookupList _
                                With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                            Case 2
                                vList = (
                                 From t In ListItemList
                                 Order By t.Description
                                 Select New DTO.vLookupList _
                                 With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                            Case Else
                                vList = (
                                 From t In ListItemList
                                 Select New DTO.vLookupList _
                                 With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using                '
            Case "ChkDigAlgName"
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.tblCheckDigitAlgorithms
                                    Order By t.ChkDigAlgName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ChkDigAlgControl, .Name = t.ChkDigAlgName, .Description = t.ChkDigAlgDesc}).ToArray()
                            Case 2
                                vList = (
                                   From t In db.tblCheckDigitAlgorithms
                                   Order By t.ChkDigAlgDesc
                                   Select New DTO.vLookupList _
                                   With {.Control = t.ChkDigAlgControl, .Name = t.ChkDigAlgName, .Description = t.ChkDigAlgDesc}).ToArray()
                            Case Else
                                vList = (
                                   From t In db.tblCheckDigitAlgorithms
                                   Select New DTO.vLookupList _
                                   With {.Control = t.ChkDigAlgControl, .Name = t.ChkDigAlgName, .Description = t.ChkDigAlgDesc}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "CostAdjType" 'added by RHR for v-8.5.4.001 
                Using db As New NGLMASIntegrationDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.tblCostAdjTypes
                                    Order By t.CostAdjTypeName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.CostAdjTypeControl, .Name = t.CostAdjTypeName, .Description = t.CostAdjTypeDesc}).ToArray()
                            Case 2
                                vList = (
                                     From t In db.tblCostAdjTypes
                                     Order By t.CostAdjTypeDesc
                                     Select New DTO.vLookupList _
                                     With {.Control = t.CostAdjTypeControl, .Name = t.CostAdjTypeName, .Description = t.CostAdjTypeDesc}).ToArray()
                            Case Else
                                vList = (
                                     From t In db.tblCostAdjTypes
                                     Select New DTO.vLookupList _
                                     With {.Control = t.CostAdjTypeControl, .Name = t.CostAdjTypeName, .Description = t.CostAdjTypeDesc}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using 'SSOAType
            Case "SSOAType" 'added by RHR for v-8.5.4.002 note: changes to database need to be updated in DAL Utilities SSOAAccount Enum
                Using db As New NGLMASSecurityDataContext(ConnectionString)
                    Dim list As New List(Of DTO.vLookupList)

                    Try
                        Select Case sortKey
                            Case 1
                                list = (
                                    From t In db.tblSSOATypes
                                    Order By t.SSOATypeName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.SSOATypeControl, .Name = t.SSOATypeName, .Description = t.SSOATypeDesc}).ToList()
                            Case 2
                                list = (
                                     From t In db.tblSSOATypes
                                     Order By t.SSOATypeDesc
                                     Select New DTO.vLookupList _
                                     With {.Control = t.SSOATypeControl, .Name = t.SSOATypeName, .Description = t.SSOATypeDesc}).ToList()
                            Case Else
                                list = (
                                     From t In db.tblSSOATypes
                                     Select New DTO.vLookupList _
                                     With {.Control = t.SSOATypeControl, .Name = t.SSOATypeName, .Description = t.SSOATypeDesc}).ToList()
                        End Select
                        list.Insert(0, New DTO.vLookupList With {.Control = 0, .Name = "None", .Description = "None"})
                        vList = list.ToArray()
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "tblImageTypeCode" 'Added by RHR for v-8.5.4.003 on 10/24/2023
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Select Case sortKey
                            Case 1
                                vList = (
                                    From t In db.tblImageTypeCodes
                                    Where (ModDate.HasValue = False OrElse t.ImageTypeCodeModDate > ModDate)
                                    Order By t.ImageTypeCodeName
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ImageTypeCodeControl, .Name = t.ImageTypeCodeName, .Description = t.ImageTypeCodeDesc}).ToArray()
                            Case 2
                                vList = (
                                    From t In db.tblImageTypeCodes
                                    Where (ModDate.HasValue = False OrElse t.ImageTypeCodeModDate > ModDate)
                                    Order By t.ImageTypeCodeDesc
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ImageTypeCodeControl, .Name = t.ImageTypeCodeName, .Description = t.ImageTypeCodeDesc}).ToArray()
                            Case Else
                                vList = (
                                    From t In db.tblImageTypeCodes
                                    Where (ModDate.HasValue = False OrElse t.ImageTypeCodeModDate > ModDate)
                                    Select New DTO.vLookupList _
                                    With {.Control = t.ImageTypeCodeControl, .Name = t.ImageTypeCodeName, .Description = t.ImageTypeCodeDesc}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "SSOALEConfigs" 'Created by RHR for v-8.5.4.004 on 12/27/2023 Gets API configurations by Legal Entity
                Using db As New NGLMASSecurityDataContext(ConnectionString)
                    Try
                        Dim list As New List(Of DTO.vLookupList)
                        Select Case sortKey
                            Case 1
                                list = (
                                    From t In db.vtblSSOALEConfigs
                                    Where
                                        t.SSOALELEAdminControl = Parameters.UserLEControl _
                                        And (ModDate.HasValue = False OrElse t.SSOALEModDate > ModDate)
                                    Order By t.SSOAName
                                    Select New DTO.vLookupList With {.Control = t.SSOALEControl, .Name = t.SSOAName, .Description = t.SSOATypeDesc}).ToList()
                            Case 2
                                list = (
                                    From t In db.vtblSSOALEConfigs
                                    Where
                                        t.SSOALELEAdminControl = Parameters.UserLEControl _
                                        And (ModDate.HasValue = False OrElse t.SSOALEModDate > ModDate)
                                    Order By t.SSOATypeDesc
                                    Select New DTO.vLookupList With {.Control = t.SSOALEControl, .Name = t.SSOAName, .Description = t.SSOATypeDesc}).ToList()
                            Case Else
                                list = (
                                    From t In db.vtblSSOALEConfigs
                                    Where
                                        t.SSOALELEAdminControl = Parameters.UserLEControl _
                                        And (ModDate.HasValue = False OrElse t.SSOALEModDate > ModDate)
                                    Select New DTO.vLookupList With {.Control = t.SSOALEControl, .Name = t.SSOAName, .Description = t.SSOATypeDesc}).ToList()
                        End Select
                        list.Insert(0, New DTO.vLookupList With {.Control = 0, .Name = "None", .Description = "NA"})
                        vList = list.ToArray()
                    Catch ex As SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case "APIUOMCodes" 'Created by RHR for v-8.5.4.004 on 01/21/2024 maps from API Rate Adjustments to BidCostAdjUOM 
                Using db As New NGLMASLookupDataContext(ConnectionString)
                    Try
                        Dim ListItemList As New List(Of DTO.vLookupList)

                        ListItemList.Add(New DTO.vLookupList(0, "None", "Not used"))
                        ListItemList.Add(New DTO.vLookupList(1, "Cases", "Eaches or number of cases"))
                        ListItemList.Add(New DTO.vLookupList(2, "Plts", "Number of pallets or containers"))
                        ListItemList.Add(New DTO.vLookupList(3, "Cubes", "Volume or cubic feet"))
                        ListItemList.Add(New DTO.vLookupList(4, "Boxs", "Number of boxes"))
                        ListItemList.Add(New DTO.vLookupList(5, "Other", "Not provided"))
                        ListItemList.Add(New DTO.vLookupList(6, "Lbs", "Pounds"))
                        ListItemList.Add(New DTO.vLookupList(7, "Kgs", "Kilograms"))
                        Select Case sortKey
                            Case 1
                                vList = (
                                From t In ListItemList
                                Order By t.Name
                                Select New DTO.vLookupList _
                                With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                            Case 2
                                vList = (
                                 From t In ListItemList
                                 Order By t.Description
                                 Select New DTO.vLookupList _
                                 With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                            Case Else
                                vList = (
                                 From t In ListItemList
                                 Select New DTO.vLookupList _
                                 With {.Control = t.Control, .Name = t.Name, .Description = t.Description}).ToArray()
                        End Select
                    Catch ex As System.Data.SqlClient.SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch ex As InvalidOperationException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                End Using
            Case Else
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoLookUpAvailable"}, New FaultReason("E_InvalidRequest"))
        End Select
        Return vList
    End Function

    Public Function GetHazmatDetails(ByVal BookItemControl As Integer) As DTO.Hazmat()

        Try
            Using db As New NGLMASLookupDataContext(ConnectionString)
                'Get the newest record that matches the provided criteria
                Dim HazmatList() As DTO.Hazmat = (
               From h In db.spGetHazmatDetails(BookItemControl)
               Select New DTO.Hazmat With {.HazRegulation = h.HazRegulation,
                                          .HazItem = h.HazItem,
                                          .HazClass = h.HazClass,
                                          .HazID = h.HazID,
                                          .HazDesc01 = h.HazDesc01,
                                          .HazDesc02 = h.HazDesc02,
                                          .HazDesc03 = h.HazDesc03,
                                          .HazUnit = h.HazUnit,
                                          .HazPackingGroup = h.HazPackingGroup,
                                          .HazPackingDesc = h.HazPackingDesc,
                                          .HazShipInst = h.HazShipInst,
                                          .HazLtdQ = h.HazLtdQ,
                                          .HazMarPoll = h.HazMarPoll,
                                          .HazMarStorCat = h.HazMarStorCat,
                                          .HazNMFCSub = h.HazNMFCSub,
                                          .HazNMFC = h.HazNMFC,
                                          .HazFrtClass = h.HazFrtClass,
                                          .HazFdxGndOK = h.HazFdxGndOK,
                                          .HazFdxAirOK = h.HazFdxAirOK,
                                          .HazUPSgndOK = h.HazUPSgndOK,
                                          .HazUPSAirOK = h.HazUPSAirOK,
                                          .HazModUser = h.HazModUser,
                                          .HazModDate = h.HazModDate}).ToArray()



                Return HazmatList
            End Using
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
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified By LVV 2/29/16 for v-7.0.5.1 EDI Migration
    ''' Now uses vEmailInvoiceComps210Offs
    ''' </remarks>
    Public Function GetCompsWithTransCodeI() As DTO.EmailInvoiceComps()

        Try
            Using db As New NGLMASLookupDataContext(ConnectionString)
                'Get the newest record that matches the provided criteria
                'Modified By LVV 2/29/16 for v-7.0.5.1 EDI Migration
                Dim CompsList() As DTO.EmailInvoiceComps = (
               From c In db.vEmailInvoiceComps210Offs
               Order By c.CompControl Ascending
               Select New DTO.EmailInvoiceComps With {.CompControl = c.CompControl,
                                                .CompNumber = c.CompNumber,
                                                .CompName = c.CompName,
                                                .CompFinInvEMailCode = c.CompFinInvEMailCode,
                                                .CompFinInvPrnCode = c.CompFinInvPrnCode,
                                                .CompMailTo = c.CompMailTo
                                                }).ToArray()


                Return CompsList
            End Using
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

    End Function

    Public Function GetCurrentCNSNumbers(Optional ByVal DaysOut As Integer = 30, Optional ModDate As Date? = Nothing) As DTO.TrackableString()


        Try
            Using db As New NGLMASLookupDataContext(ConnectionString)
                Dim CNSList() As DTO.TrackableString = (
                    From d In db.spGetCurrentCNSNumbers(DaysOut, Me.Parameters.UserName, ModDate)
                    Select New DTO.TrackableString With {.Text = d.BookConsPrefix}).ToArray

                Return CNSList
            End Using
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

    End Function

    Public Function GetCarrierTariffEquipmentList(ByVal CompControl As Integer, ByVal ModeTypeControl As Integer, ByVal TarTempType As Integer) As DTO.vCarrierTariffEquipment()
        Dim oRet As DTO.vCarrierTariffEquipment()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                oRet = (From d In db.vCarrierTariffEquipments
                        Where d.CarrTarCompControl = CompControl _
                        And d.CarrTarTariffModeTypeControl = ModeTypeControl _
                        And d.CarrTarTempType = TarTempType
                        Select New DTO.vCarrierTariffEquipment _
                        With {.CarrierName = d.CarrierName,
                        .ModeTypeName = d.ModeTypeName,
                        .TariffTemp = d.TariffTemp,
                        .CarrTarName = d.CarrTarName,
                        .CarrTarEffDateFrom = d.CarrTarEffDateFrom,
                        .CarrTarRevisionNumber = d.CarrTarRevisionNumber,
                        .CarrTarEquipName = d.CarrTarEquipName,
                        .CarrierControl = d.CarrierControl,
                        .CarrierNumber = d.CarrierNumber,
                        .CarrTarControl = d.CarrTarControl,
                        .CarrTarCompControl = d.CarrTarCompControl,
                        .CarrTarID = d.CarrTarID,
                        .CarrTarTempType = d.CarrTarTempType,
                        .CarrTarEffDateTo = d.CarrTarEffDateTo,
                        .CarrTarApproved = d.CarrTarApproved,
                        .CarrTarTariffModeTypeControl = d.CarrTarTariffModeTypeControl,
                        .CarrTarEquipControl = d.CarrTarEquipControl,
                        .CarrTarEquipDescription = d.CarrTarEquipDescription,
                        .CarrTarEquipCasesMinimum = d.CarrTarEquipCasesMinimum,
                        .CarrTarEquipCasesMaximum = d.CarrTarEquipCasesMaximum,
                        .CarrTarEquipWgtMinimum = d.CarrTarEquipWgtMinimum,
                        .CarrTarEquipWgtMaximum = d.CarrTarEquipWgtMaximum,
                        .CarrTarEquipCubesMinimum = d.CarrTarEquipCubesMinimum,
                        .CarrTarEquipCubesMaximum = d.CarrTarEquipCubesMaximum,
                        .CarrTarEquipPltsMinimum = d.CarrTarEquipPltsMinimum,
                        .CarrTarEquipPltsMaximum = d.CarrTarEquipPltsMaximum,
                        .CarrTarEquipMultiOrigRating = d.CarrTarEquipMultiOrigRating}).ToArray() ' Modified by RHR for v-8.5.4.001 on 06/14/2023 new Origin Rate shop logic


                Return oRet



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

            Return oRet

        End Using
    End Function


#Region "TMS 365"


    '''' <summary>
    '''' Gets the Zip codes from tblZipCode filtered by ZipCode.StartsWith(filterWhere)
    '''' </summary>
    '''' <param name="RecordCount"></param>
    '''' <param name="filterWhere"></param>
    '''' <param name="sortExpression"></param>
    '''' <param name="page"></param>
    '''' <param name="pagesize"></param>
    '''' <param name="skip"></param>
    '''' <param name="take"></param>
    '''' <returns></returns>
    '''' <remarks>
    '''' Added By LVV on 5/11/17 for v-8.0 Rate Shop Free Trial
    '''' </remarks>
    'Public Function GetZips(ByRef RecordCount As Integer,
    '                        Optional ByVal filterWhere As String = "",
    '                        Optional ByVal sortExpression As String = "",
    '                        Optional ByVal page As Integer = 1,
    '                        Optional ByVal pagesize As Integer = 1000,
    '                        Optional ByVal skip As Integer = 0,
    '                        Optional ByVal take As Integer = 0) As Models.ZipCode()
    '    Dim oRetData As Models.ZipCode()
    '    Using db As New NGLMASLookupDataContext(ConnectionString)
    '        Try
    '            Dim intPageCount As Integer = 1

    '            'Filter By Zip
    '            Dim oQuery = (From t In db.tblZipCodes
    '                          Where t.ZipCode.StartsWith(filterWhere)
    '                          Select New Models.ZipCode With {
    '                                 .ZipCodeControl = t.ZipCodeControl,
    '                                 .ZipCode = t.ZipCode,
    '                                 .City = t.City,
    '                                 .State = t.State}).GroupBy(Function(g) New With {g.ZipCode, g.City, g.State}).Select(Function(x) New Models.ZipCode() With
    '                                    {.ZipCode = x.Key.ZipCode, .City = x.Key.City, .State = x.Key.State})

    '            If oQuery Is Nothing Then Return Nothing

    '            RecordCount = oQuery.Count()
    '            If RecordCount < 1 Then Return Nothing

    '            If take <> 0 Then
    '                pagesize = take
    '            Else
    '                'calculate based on page and pagesize
    '                If pagesize < 1 Then pagesize = 1
    '                If RecordCount < 1 Then RecordCount = 1
    '                If page < 1 Then page = 1
    '                skip = (page - 1) * pagesize
    '            End If
    '            If RecordCount > pagesize Then intPageCount = ((RecordCount - 1) \ pagesize) + 1
    '            oRetData = oQuery.OrderBy(sortExpression).Skip(skip).Take(pagesize).ToArray()

    '            Return oRetData

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

    '        Return Nothing

    '    End Using
    'End Function


    ''' <summary>
    ''' Gets the Zip codes from tblZipCode filtered by ZipCode.StartsWith(filterWhere)
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filterWhere"></param>
    ''' <param name="sortExpression"></param>
    ''' <param name="page"></param>
    ''' <param name="pagesize"></param>
    ''' <param name="skip"></param>
    ''' <param name="take"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 5/11/17 for v-8.0 Rate Shop Free Trial
    ''' </remarks>
    Public Function GetZips(ByRef RecordCount As Integer,
                            Optional ByVal filterWhere As String = "",
                            Optional ByVal sortExpression As String = "",
                            Optional ByVal page As Integer = 1,
                            Optional ByVal pagesize As Integer = 1000,
                            Optional ByVal skip As Integer = 0,
                            Optional ByVal take As Integer = 0) As Models.ZipCode()
        Dim oRetData As Models.ZipCode()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim intPageCount As Integer = 1

                'Filter By Zip
                Dim oQuery = (From t In db.tblZipCodes
                              Where t.ZipCode.StartsWith(filterWhere)
                              Select New Models.ZipCode With {
                                     .ZipCodeControl = t.ZipCodeControl,
                                     .ZipCode = t.ZipCode,
                                     .City = t.City,
                                     .State = t.State}).GroupBy(Function(g) New With {g.ZipCode, g.City, g.State}).Select(Function(x) New Models.ZipCode() With
                                        {.ZipCode = x.Key.ZipCode, .City = x.Key.City, .State = x.Key.State})

                If oQuery Is Nothing Then Return Nothing

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
    ''' Gets the Zip codes from tblZipCode filtered by City.StartsWith(filterWhere)
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filterWhere"></param>
    ''' <param name="sortExpression"></param>
    ''' <param name="page"></param>
    ''' <param name="pagesize"></param>
    ''' <param name="skip"></param>
    ''' <param name="take"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 5/11/17 for v-8.0 Rate Shop Free Trial
    ''' </remarks>
    Public Function GetZipsByCity(ByRef RecordCount As Integer,
                            Optional ByVal filterWhere As String = "",
                            Optional ByVal sortExpression As String = "",
                            Optional ByVal page As Integer = 1,
                            Optional ByVal pagesize As Integer = 1000,
                            Optional ByVal skip As Integer = 0,
                            Optional ByVal take As Integer = 0) As Models.ZipCode()
        Dim oRetData As Models.ZipCode()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim intPageCount As Integer = 1

                'Filter By City
                Dim oQuery = (From t In db.tblZipCodes
                              Where t.City.StartsWith(filterWhere)
                              Select New Models.ZipCode With {
                                     .ZipCodeControl = t.ZipCodeControl,
                                     .ZipCode = t.ZipCode,
                                     .City = t.City,
                                     .State = t.State}).GroupBy(Function(g) New With {g.City, g.State}).Select(Function(x) New Models.ZipCode() With
                                        {.City = x.Key.City, .State = x.Key.State})

                If oQuery Is Nothing Then Return Nothing

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
    ''' Gets the Zip codes from tblZipCode filtered by State.StartsWith(filterWhere)
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filterWhere"></param>
    ''' <param name="sortExpression"></param>
    ''' <param name="page"></param>
    ''' <param name="pagesize"></param>
    ''' <param name="skip"></param>
    ''' <param name="take"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 5/11/17 for v-8.0 Rate Shop Free Trial
    ''' </remarks>
    Public Function GetZipsByState(ByRef RecordCount As Integer,
                            Optional ByVal filterWhere As String = "",
                            Optional ByVal sortExpression As String = "",
                            Optional ByVal page As Integer = 1,
                            Optional ByVal pagesize As Integer = 1000,
                            Optional ByVal skip As Integer = 0,
                            Optional ByVal take As Integer = 0) As Models.ZipCode()
        Dim oRetData As Models.ZipCode()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim intPageCount As Integer = 1

                'Filter By State
                Dim oQuery = (From t In db.tblZipCodes
                              Where t.State.StartsWith(filterWhere)
                              Select New Models.ZipCode With {
                                     .ZipCodeControl = t.ZipCodeControl,
                                     .ZipCode = t.ZipCode,
                                     .City = t.City,
                                     .State = t.State}).GroupBy(Function(g) g.State).Select(Function(x) x.FirstOrDefault())

                If oQuery Is Nothing Then Return Nothing

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
    ''' Gets an array of ZipCodes for the provided City and State
    ''' </summary>
    ''' <param name="City"></param>
    ''' <param name="State"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 5/11/17 for v-8.0 Rate Shop Free Trial
    ''' </remarks>
    Public Function GetZipsForCityState(ByVal City As String, ByVal State As String) As Models.ZipCode()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                Dim oRetData = (From t In db.tblZipCodes
                                Where (t.City = City AndAlso t.State = State)
                                Order By t.ZipCode
                                Select New Models.ZipCode With {
                                     .ZipCode = t.ZipCode,
                                     .City = t.City,
                                     .State = t.State}).Distinct().ToArray()
                Return oRetData

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

    Public Function GetCarriersWAccessorialMaintConfigsToCopy(ByVal LEAControl As Integer) As LTS.spGetCarriersWAccessorialMaintConfigsToCopyResult()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                Return db.spGetCarriersWAccessorialMaintConfigsToCopy(LEAControl).ToArray()

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
        Return Nothing
    End Function

    Public Function GetLEWithCAMConfigsToCopy() As LTS.vGetLEWithCAMConfigsToCopy()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                Return (From t In db.vGetLEWithCAMConfigsToCopies Select t).ToArray()

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
        Return Nothing
    End Function


    Public Function CopyCarrierAccessorialMaintConfig(ByVal CopyToCarrier As Integer, ByVal CopyFromCarrier As Integer, ByVal LEAControl As Integer) As LTS.spCopyCarrierAccessorialMaintConfigResult
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                Return db.spCopyCarrierAccessorialMaintConfig(CopyToCarrier, CopyFromCarrier, LEAControl).FirstOrDefault()

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
        Return Nothing
    End Function

    Public Function CopyLECAMConfig(ByVal CopyToLE As Integer, ByVal CopyFromLE As Integer) As LTS.spCopyLECAMConfigResult
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                Return db.spCopyLECAMConfig(CopyToLE, CopyFromLE).FirstOrDefault()

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
        Return Nothing
    End Function

    Public Function CheckSettlementAuditMessageVisibility(ByVal CompControl As Integer,
                                                          ByVal CarrierControl As Integer,
                                                          ByVal SHID As String) As LTS.spCheckSettlementAuditMessageVisibilityResult
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                Return db.spCheckSettlementAuditMessageVisibility(CompControl, CarrierControl, SHID).FirstOrDefault()

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
        Return Nothing
    End Function


    Public Function GetvBFPApprovalCarriers(ByRef RecordCount As Integer,
                                        Optional ByVal page As Integer = 1,
                                        Optional ByVal pagesize As Integer = 1000,
                                        Optional ByVal skip As Integer = 0,
                                        Optional ByVal take As Integer = 0) As DTO.vLookupList()
        Dim oRetData As DTO.vLookupList()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim intPageCount As Integer = 1
                Dim oQuery As IQueryable(Of DTO.vLookupList)
                Dim sortExpression As String = "Name Asc"

                oQuery = (From t In db.vBookFeesPendingRefs
                          Where t.BFPApproved = 0
                          Order By t.CarrierName Ascending
                          Select New DTO.vLookupList _
                                    With {.Control = t.CarrierControl, .Name = t.CarrierName, .Description = t.CarrierSCAC}).Distinct()

                If oQuery Is Nothing Then Return Nothing

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


#End Region

#End Region

#Region " Pallet Type"

    Public Function GetPalletTypes() As DTO.PalletType()

        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'if there are no null values to deal with we can build the array directly
                Dim palletTypes() As DTO.PalletType = (
                From t In db.PalletTypes
                Order By t.PalletType
                Select New DTO.PalletType With {.ID = t.ID, .PalletTypeName = t.PalletType, .PalletTypeDescription = t.PalletTypeDescription, .PalletTypeWeight = t.PalletTypeWeight, .PalletTypeHeight = t.PalletTypeHeight, .PalletTypeWidth = t.PalletTypeWidth, .PalletTypeDepth = t.PalletTypeDepth, .PalletTypeVolume = t.PalletTypeVolume, .PalletTypeContainer = t.PalletTypeContainer, .PalletTypeUpdated = t.PalletTypeUpdated.ToArray()}).ToArray()
                Return palletTypes

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

    Public Function GetPalletType(ByVal ID As Integer) As DTO.PalletType

        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'if there are no null values to deal with we can build the array directly
                Dim palletType As DTO.PalletType = (
                From t In db.PalletTypes
                Where t.ID = ID
                Select New DTO.PalletType With {.ID = t.ID, .PalletTypeName = t.PalletType, .PalletTypeDescription = t.PalletTypeDescription, .PalletTypeWeight = t.PalletTypeWeight, .PalletTypeHeight = t.PalletTypeHeight, .PalletTypeWidth = t.PalletTypeWidth, .PalletTypeDepth = t.PalletTypeDepth, .PalletTypeVolume = t.PalletTypeVolume, .PalletTypeContainer = t.PalletTypeContainer, .PalletTypeUpdated = t.PalletTypeUpdated.ToArray()}).Single
                Return palletType

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

    Public Function CreatePalletType(ByVal oData As DTO.PalletType) As DTO.PalletType
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Return GetPalletType(CType(CreateRecord(oData, db.PalletTypes, db, AddressOf CopyPalletTypeToLinq, AddressOf ValidateNewPalletType), LTS.PalletType).ID)
        End Using

    End Function

    Public Sub DeletePalletType(ByVal oData As DTO.PalletType)
        Using db As New NGLMASLookupDataContext(ConnectionString)
            DeleteRecord(oData, db.PalletTypes, db, AddressOf CopyPalletTypeToLinq, AddressOf NoDataValidation)
        End Using
    End Sub

    Public Function UpdatePalletType(ByVal oData As DTO.PalletType) As DTO.PalletType
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Return GetPalletType(CType(UpdateRecord(oData, db.PalletTypes, db, AddressOf CopyPalletTypeToLinq, AddressOf ValidateUpdatePalletType, AddressOf NoDetailsToProcess), LTS.PalletType).ID)
        End Using
    End Function

    '********************** TMS 365 Methods **********************
    ''' <summary>
    ''' Gets a list of PalletTypes where the PalletTypeBitPos is in the parameter array
    ''' </summary>
    ''' <param name="pkgList"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 6/19/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Public Function GetPalletTypesByEnumIDs(ByVal pkgList() As Integer) As LTS.PalletType()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                If pkgList Is Nothing OrElse pkgList.Count < 1 Then Return Nothing

                Return db.PalletTypes.Where(Function(x) pkgList.Contains(x.PalletTypeBitPos)).ToArray()

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
    ''' Gets all PalletTypes and returns them as Models.DockPTType
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 6/19/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Public Function GetAllPackageTypes() As Models.DockPTType()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                Dim pkgs() As Models.DockPTType = (
                From t In db.PalletTypes
                Order By t.PalletTypeDescription
                Select New Models.DockPTType With {.PTBitPos = t.PalletTypeBitPos, .PTCaption = t.PalletTypeDescription, .PTOn = False}).ToArray()
                Return pkgs

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

    Public Function GetPalletTypesList() As DTO.vLookupList()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'if there are no null values to deal with we can build the array directly
                Dim palletTypes() As DTO.vLookupList = (
                From t In db.PalletTypes
                Order By t.PalletType
                Select New DTO.vLookupList With {.Control = t.ID, .Name = t.PalletType, .Description = t.PalletTypeDescription}).ToArray()
                Return palletTypes

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

#Region " Private Methods"

    Private Function CopyPalletTypeToLinq(ByVal oData As DTO.PalletType) As LTS.PalletType
        'Create New Record
        Return New LTS.PalletType With {.ID = oData.ID,
                                        .PalletType = oData.PalletTypeName,
                                        .PalletTypeDescription = oData.PalletTypeDescription,
                                        .PalletTypeWeight = oData.PalletTypeWeight,
                                        .PalletTypeHeight = oData.PalletTypeHeight,
                                        .PalletTypeWidth = oData.PalletTypeWidth,
                                        .PalletTypeDepth = oData.PalletTypeDepth,
                                        .PalletTypeVolume = oData.PalletTypeVolume,
                                        .PalletTypeContainer = oData.PalletTypeContainer,
                                        .PalletTypeUpdated = oData.PalletTypeUpdated}
    End Function

    Private Sub ValidateNewPalletType(ByRef oDB As NGLMASLookupDataContext, ByVal oData As DTO.PalletType)
        'Check if the Pallet Type already exists only one allowed
        Try
            Dim palletType As DTO.PalletType = (
            From t In oDB.PalletTypes
            Where
                (t.PalletType = oData.PalletTypeName _
                Or
                t.PalletTypeDescription = oData.PalletTypeDescription)
            Select New DTO.PalletType With {.ID = t.ID}).First

            If Not palletType Is Nothing Then
                Utilities.SaveAppError("Cannot add new PalletType.  The Name, " & oData.PalletTypeName & " or the Description, " & oData.PalletTypeDescription & ",  already exist.", Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
            End If

        Catch ex As FaultException
            Throw
        Catch ex As InvalidOperationException
            'do nothing this is the desired result.
        End Try

    End Sub

    Private Sub ValidateUpdatePalletType(ByRef oDB As NGLMASLookupDataContext, ByVal oData As DTO.PalletType)
        'Check if the Pallet Type already exists
        Try
            'Get the newest record that matches the provided criteria
            Dim palletType As DTO.PalletType = (
            From t In oDB.PalletTypes
            Where
                (t.ID <> oData.ID) _
                And
                (t.PalletType = oData.PalletTypeName _
                Or
                t.PalletTypeDescription = oData.PalletTypeDescription)
            Select New DTO.PalletType With {.ID = t.ID}).First

            If Not palletType Is Nothing Then
                Utilities.SaveAppError("Cannot update PalletType.  The Name, " & oData.PalletTypeName & " or the Description, " & oData.PalletTypeDescription & ",  already exist.", Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
            End If
        Catch ex As FaultException
            Throw
        Catch ex As InvalidOperationException
            'do nothing this is the desired result.
        End Try
    End Sub
#End Region

#End Region

#Region "AMS Appointment Detail Fields 365"

    ''' <summary>
    ''' Gets all AMSApptDetailFields and returns them as Models.DockPTType
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 7/10/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Public Function GetAllAMSApptDetailFields() As Models.DockPTType()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                Dim dets() As Models.DockPTType = (
                From t In db.tblAMSApptDetailFields
                Order By t.ApptDetailFieldDesc
                Select New Models.DockPTType With {.PTBitPos = t.ApptDetailFieldBitPos, .PTCaption = t.ApptDetailFieldDesc, .PTOn = False}).ToArray()
                Return dets

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

#End Region

End Class


Public Class NGLtblServiceTypeData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLookupDataContext(ConnectionString)
        Me.LinqTable = db.tblServiceTypes
        Me.LinqDB = db
        Me.SourceClass = "NGLtblServiceTypeData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLookupDataContext(ConnectionString)
            _LinqTable = db.tblServiceTypes
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
        Return GettblServiceTypeFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GettblServiceTypesFiltered()
    End Function

    ''' <summary>
    ''' Enter zero for LowerControl to return the record with the lowest control PK
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="LowerControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetFirstRecord(ByVal LowerControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass

        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim tblServiceType As DTO.tblServiceType

                If LowerControl <> 0 Then
                    tblServiceType = (
                   From d In db.tblServiceTypes
                   Where d.ServiceTypeControl >= LowerControl
                   Order By d.ServiceTypeControl
                   Select selectDTOData(d, db)).FirstOrDefault
                Else
                    'Zero indicates that we should get the record with the lowest control number even if it is below zero
                    tblServiceType = (
                   From d In db.tblServiceTypes
                   Order By d.ServiceTypeControl
                   Select selectDTOData(d, db)).FirstOrDefault
                End If



                Return tblServiceType

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
    ''' Enter the CurrentControl number to use as the reference to the previous record
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetPreviousRecord(ByVal CurrentControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the first record that matches the provided criteria
                Dim tblServiceType As DTO.tblServiceType = (
                From d In db.tblServiceTypes
                Where d.ServiceTypeControl < CurrentControl
                Order By d.ServiceTypeControl Descending
                Select selectDTOData(d, db)).FirstOrDefault


                Return tblServiceType

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
    ''' Enter the CurrentControl number to use as the reference to the next record
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetNextRecord(ByVal CurrentControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the first record that matches the provided criteria
                Dim tblServiceType As DTO.tblServiceType = (
                From d In db.tblServiceTypes
                Where d.ServiceTypeControl > CurrentControl
                Order By d.ServiceTypeControl
                Select selectDTOData(d, db)).FirstOrDefault


                Return tblServiceType

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
    ''' Enter zero as the UpperControl number return the record with the highest control PK
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="UpperControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetLastRecord(ByVal UpperControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim tblServiceType As DTO.tblServiceType

                If UpperControl <> 0 Then

                    tblServiceType = (
                    From d In db.tblServiceTypes
                    Where d.ServiceTypeControl >= UpperControl
                    Order By d.ServiceTypeControl
                    Select selectDTOData(d, db)).FirstOrDefault
                Else
                    'Zero indicates that we should get the hightest ServiceTypecontrol record
                    tblServiceType = (
                    From d In db.tblServiceTypes
                    Order By d.ServiceTypeControl Descending
                    Select selectDTOData(d, db)).FirstOrDefault

                End If


                Return tblServiceType

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

    Public Function GettblServiceTypeFiltered(ByVal Control As Integer) As DTO.tblServiceType
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim tblServiceType As DTO.tblServiceType = (
                From d In db.tblServiceTypes
                Where
                    d.ServiceTypeControl = Control
                Select selectDTOData(d, db)).First

                Return tblServiceType

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

    Public Function GettblServiceTypesFiltered(Optional ByVal page As Integer = 1, Optional ByVal pagesize As Integer = 1000) As DTO.tblServiceType()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1
                'count the record

                Try
                    intRecordCount = getScalarInteger("select COUNT(*) from dbo.tblServiceType")

                Catch ex As Exception
                    'ignore any record count errors
                End Try
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize

                'Return all the contacts that match the criteria sorted by name
                Dim tblServiceTypes() As DTO.tblServiceType = (
                From d In db.tblServiceTypes
                Order By d.ServiceTypeControl
                Select selectDTOData(d, db, page, intPageCount, intRecordCount, pagesize)).Skip(intSkip).Take(pagesize).ToArray()
                Return tblServiceTypes

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
    ''' Used by System processes forces delete and bypasses validation 
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <remarks></remarks>
    Public Sub SystemDelete(ByVal oData As DTO.tblServiceType)
        Using db As New NGLMASLookupDataContext(ConnectionString)
            'Create New Record
            Dim nObject = CopyDTOToLinq(oData)
            db.tblServiceTypes.Attach(nObject, True)
            db.tblServiceTypes.DeleteOnSubmit(nObject)
            Try
                db.SubmitChanges()
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch conflictEx As ChangeConflictException
                Try
                    'Improper Reference to LinqDB Fixed by RHR 2/6/15
                    'Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(LinqDB))
                    Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(db))
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
            DeleteCleanUp(nObject)
        End Using
    End Sub

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.tblServiceType)
        Return selectLTSData(d, Me.Parameters.UserName)
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GettblServiceTypeFiltered(Control:=CType(LinqTable, LTS.tblServiceType).ServiceTypeControl)
    End Function

    Public Function QuickSaveResults(ByVal ServiceTypeControl As Integer) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                ret = (From d In db.tblServiceTypes
                       Where d.ServiceTypeControl = ServiceTypeControl
                       Select New DTO.QuickSaveResults With {.Control = d.ServiceTypeControl _
                                                            , .ModDate = d.ServiceTypeModDate _
                                                            , .ModUser = d.ServiceTypeModUser _
                                                            , .Updated = d.ServiceTypeUpdated.ToArray}).FirstOrDefault()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("QuickSaveResults"))
            End Try

        End Using
        Return ret
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim source As LTS.tblServiceType = TryCast(LinqTable, LTS.tblServiceType)
        If source Is Nothing Then Return Nothing
        Return QuickSaveResults(source.ServiceTypeControl)
    End Function

    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'We do not allow Service Type records to be deleted 
        Utilities.SaveAppError("Cannot delete data.  Records cannot be deleted using this interface!", Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))
    End Sub

    Friend Shared Function selectDTOData(ByVal d As LTS.tblServiceType, ByRef db As NGLMASLookupDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.tblServiceType
        Dim oDTO As New DTO.tblServiceType()
        Dim skipObjs As New List(Of String) From {"ServiceTypeUpdated",
                                                  "Page",
                                                  "Pages",
                                                  "RecordCount",
                                                  "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .ServiceTypeUpdated = d.ServiceTypeUpdated.ToArray()
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oDTO

    End Function

    ''' <summary>
    ''' Typically used when we want to insert a new LTS object in the DB
    ''' </summary>
    ''' <param name="d"></param>
    ''' <param name="UserName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function selectLTSData(ByVal d As DTO.tblServiceType, ByVal UserName As String) As LTS.tblServiceType
        ' If Updated is null, set to byte[0] (for inserted items).
        Dim oLTS As New LTS.tblServiceType
        UpdateLTSWithDTO(d, oLTS, UserName)
        Return oLTS

    End Function

    Friend Shared Sub UpdateLTSWithDTO(ByRef d As DTO.tblServiceType, ByRef t As LTS.tblServiceType, ByVal UserName As String)
        Dim blnNewLTS As Boolean = False 'used to determine if we allow the BookItemData to be set,  existing LTS objects cannot update the BookItemUpdated value.
        If t.ServiceTypeControl = 0 Then blnNewLTS = True 'in this case we use a new Byte or the current value in d

        Dim skipObjs As New List(Of String) From {"ServiceTypeModDate", "ServiceTypeModUser", "ServiceTypeUpdated"}
        t = CopyMatchingFields(t, d, skipObjs)
        With t
            .ServiceTypeModDate = Date.Now
            .ServiceTypeModUser = UserName
            If blnNewLTS Then .ServiceTypeUpdated = If(d.ServiceTypeUpdated Is Nothing, New Byte() {}, d.ServiceTypeUpdated)
        End With
    End Sub


#End Region

End Class


''' <summary>
''' DTO and LTS library for the existing tblServiceToken Data 
''' New functionality created for Token Automation 
''' </summary>
''' <remarks>
''' Created by RHR for v-8.4 on 04/22/2021
'''     all new code.  the DTO objectd was created but no logic was found 
'''     to manage the database integration
'''     additional LTS logic was created
''' </remarks>
Public Class NGLtblServiceTokenData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLookupDataContext(ConnectionString)
        Me.LinqTable = db.tblServiceTokens
        Me.LinqDB = db
        Me.SourceClass = "NGLtblServiceTokenData"
    End Sub

#End Region

#Region " Enus"

    ''' <summary>
    ''' Maps to ServiceTokenServiceTypeControl
    ''' </summary>
    ''' <remarks>
    ''' Created by RHR for v-8.4 on 05/04/2021 
    ''' </remarks>
    Public Enum TokenServiceType
        None = 0
        CarrierAcceptLoadWithToken = 1  '	Accept Load Tendered to Carrier via Token Link in Load Tender Email
        CarrierBookApptWithToken = 2    '	Allow Carrier to Book And Appoinyment for a Load via Token Link in Load Tender Email
    End Enum
#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLookupDataContext(ConnectionString)
            _LinqTable = db.tblServiceTokens
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
        Return GettblServiceTokenFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GettblServiceTokensFiltered()
    End Function

    ''' <summary>
    ''' Enter zero for LowerControl to return the record with the lowest control PK
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="LowerControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetFirstRecord(ByVal LowerControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass

        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim tblServiceToken As DTO.tblServiceToken

                If LowerControl <> 0 Then
                    tblServiceToken = (
                   From d In db.tblServiceTokens
                   Where d.ServiceTokenControl >= LowerControl
                   Order By d.ServiceTokenControl
                   Select selectDTOData(d, db)).FirstOrDefault
                Else
                    'Zero indicates that we should get the record with the lowest control number even if it is below zero
                    tblServiceToken = (
                   From d In db.tblServiceTokens
                   Order By d.ServiceTokenControl
                   Select selectDTOData(d, db)).FirstOrDefault()
                End If



                Return tblServiceToken

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetFirstRecord"), db)
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Enter the CurrentControl number to use as the reference to the previous record
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetPreviousRecord(ByVal CurrentControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the first record that matches the provided criteria
                Dim tblServiceToken As DTO.tblServiceToken = (
                From d In db.tblServiceTokens
                Where d.ServiceTokenControl < CurrentControl
                Order By d.ServiceTokenControl Descending
                Select selectDTOData(d, db)).FirstOrDefault()


                Return tblServiceToken

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetPreviousRecord"), db)
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Enter the CurrentControl number to use as the reference to the next record
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetNextRecord(ByVal CurrentControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the first record that matches the provided criteria
                Dim tblServiceToken As DTO.tblServiceToken = (
                From d In db.tblServiceTokens
                Where d.ServiceTokenControl > CurrentControl
                Order By d.ServiceTokenControl
                Select selectDTOData(d, db)).FirstOrDefault()


                Return tblServiceToken

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetNextRecord"), db)
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Enter zero as the UpperControl number return the record with the highest control PK
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="UpperControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetLastRecord(ByVal UpperControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim tblServiceToken As DTO.tblServiceToken

                If UpperControl <> 0 Then

                    tblServiceToken = (
                    From d In db.tblServiceTokens
                    Where d.ServiceTokenControl >= UpperControl
                    Order By d.ServiceTokenControl
                    Select selectDTOData(d, db)).FirstOrDefault
                Else
                    'Zero indicates that we should get the hightest ServiceTokenControl record
                    tblServiceToken = (
                    From d In db.tblServiceTokens
                    Order By d.ServiceTokenControl Descending
                    Select selectDTOData(d, db)).FirstOrDefault

                End If


                Return tblServiceToken

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLastRecord"), db)
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GettblServiceTokenFiltered(ByVal Control As Integer) As DTO.tblServiceToken
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim tblServiceToken As DTO.tblServiceToken = (
                From d In db.tblServiceTokens
                Where
                    d.ServiceTokenControl = Control
                Select selectDTOData(d, db)).First

                Return tblServiceToken

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GettblServiceTokenFiltered"), db)
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GettblServiceTokensFiltered(Optional ByVal page As Integer = 1, Optional ByVal pagesize As Integer = 1000) As DTO.tblServiceToken()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1
                'count the record

                Try
                    intRecordCount = getScalarInteger("select COUNT(*) from dbo.tblServiceToken")

                Catch ex As Exception
                    'ignore any record count errors
                End Try
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize

                'Return all the contacts that match the criteria sorted by name
                Dim tblServiceTokens() As DTO.tblServiceToken = (
                From d In db.tblServiceTokens
                Order By d.ServiceTokenControl
                Select selectDTOData(d, db, page, intPageCount, intRecordCount, pagesize)).Skip(intSkip).Take(pagesize).ToArray()
                Return tblServiceTokens

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GettblServiceTokensFiltered"), db)
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Used by System processes forces delete and bypasses validation 
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <remarks></remarks>
    Public Sub SystemDelete(ByVal oData As DTO.tblServiceToken)
        Using db As New NGLMASLookupDataContext(ConnectionString)
            'Create New Record
            Dim nObject = CopyDTOToLinq(oData)
            db.tblServiceTokens.Attach(nObject, True)
            db.tblServiceTokens.DeleteOnSubmit(nObject)
            Try
                db.SubmitChanges()
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch conflictEx As ChangeConflictException
                Try
                    'Improper Reference to LinqDB Fixed by RHR 2/6/15
                    'Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(LinqDB))
                    Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(db))
                    conflictInfo.LogConflictDetails(conflictInfo.Message, Me.Parameters)
                    Throw New FaultException(Of ConflictFaultInfo)(conflictInfo, New FaultReason("E_DataConflict"))
                Catch ex As FaultException
                    Throw
                Catch ex As Exception
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                End Try
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SystemDelete"), db)
            End Try
            DeleteCleanUp(nObject)
        End Using
    End Sub

#End Region


#Region "365 LTS Methods"
    'All new LTS methods Modified by RHR for v-8.4 on 05/03/2021 

    Public Function GettblServiceTokenByControl(ByVal iServiceTokenControl As Integer) As LTS.tblServiceToken
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                If iServiceTokenControl = 0 OrElse Not db.tblServiceTokens.Any(Function(x) x.ServiceTokenControl = iServiceTokenControl) Then throwNoDataFaultMessage("E_InvalidRecordKey") 'we do not have a valid filter - "The reference to the record is missing. Please select another record and try again."
                Return db.tblServiceTokens.Where(Function(x) x.ServiceTokenControl = iServiceTokenControl).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GettblServiceTokenByControl"), db)
            End Try
            Return Nothing
        End Using
    End Function

    Public Function GettblServiceTokenByToken(ByVal strServiceToken As String, ByVal iServiceTypeControl As Integer) As LTS.tblServiceToken
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                If String.IsNullOrWhiteSpace(strServiceToken) OrElse Not db.tblServiceTokens.Any(Function(x) x.ServiceToken = strServiceToken And x.ServiceTokenServiceTypeControl = iServiceTypeControl) Then throwNoDataFaultMessage("E_InvalidRecordKey") 'we do not have a valid filter - "The reference to the record is missing. Please select another record and try again."
                Return db.tblServiceTokens.Where(Function(x) x.ServiceToken = strServiceToken And x.ServiceTokenServiceTypeControl = iServiceTypeControl).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GettblServiceTokenByToken"), db)
            End Try
            Return Nothing
        End Using
    End Function


    Public Function InsertOrUpdatetblServiceToken(ByVal oRecord As LTS.tblServiceToken) As LTS.tblServiceToken
        Dim oTokenUpdated As LTS.tblServiceToken = Nothing
        'token = Guid.NewGuid().ToString 'now we create a new token on sign in
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                oRecord.ServiceTokenModDate = Date.Now
                oRecord.ServiceTokenModUser = Parameters.UserName
                If String.IsNullOrWhiteSpace(oRecord.ServiceToken) Then
                    oRecord.ServiceToken = Guid.NewGuid().ToString()
                End If

                If oRecord.ServiceTokenControl = 0 Then
                    'check if this record exists.
                    If db.tblServiceTokens.Any(Function(x) x.ServiceToken = oRecord.ServiceToken) Then
                        'We need a new Token the old token is already used.
                        oRecord.ServiceToken = Guid.NewGuid().ToString()
                    End If
                    'Insert
                    db.tblServiceTokens.InsertOnSubmit(oRecord)
                Else
                    'Update
                    db.tblServiceTokens.Attach(oRecord, True)
                End If
                db.SubmitChanges()
                oTokenUpdated = oRecord
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertOrUpdatetblServiceToken"), db)
            End Try
        End Using
        Return oTokenUpdated
    End Function

    'Modified by RHR for v-8.4.0.003 on 11/2/2021
    Public Function CarrierBookApptWithTokenData(ByVal strServiceToken As String) As Models.CarrierBookApptWithTokenData
        Dim oRet As New Models.CarrierBookApptWithTokenData()
        Try
            Dim oTokenData As LTS.tblServiceToken = GettblServiceTokenByToken(strServiceToken, 2)
            If Not oTokenData Is Nothing Then
                Using db As New NGLMasBookDataContext(ConnectionString)
                    Try
                        'get the book data 
                        Dim oBook As LTS.Book = db.Books.Where(Function(x) x.BookControl = oTokenData.ServiceTokenBookControl).FirstOrDefault()
                        If oBook Is Nothing OrElse oBook.BookControl = 0 Then
                            throwNoDataFaultMessage("E_InvalidRecordKey")
                        End If

                        Dim sCompName As String = db.CompRefBooks.Where(Function(x) x.CompControl = oBook.BookCustCompControl).Select(Function(Y) Y.CompName).FirstOrDefault()
                        Dim oLane As LTS.LaneRefBook = db.LaneRefBooks.Where(Function(x) x.LaneControl = oBook.BookODControl).FirstOrDefault()
                        With oRet
                            .BookControl = oBook.BookControl
                            .BookDateLoad = oBook.BookDateLoad
                            .BookSHID = oBook.BookSHID
                            .CarrierControl = oBook.BookCarrierControl
                            .CompControl = oBook.BookCustCompControl
                            .CompName = sCompName
                            .BookConsPrefix = oBook.BookConsPrefix
                            .BookDateRequired = oBook.BookDateRequired
                            .ExpirationDate = oTokenData.ServiceTokenExpirationDate
                            .LaneControl = oBook.BookODControl
                            .LaneAllowCarrierBookApptByEmail = If(oLane.LaneAllowCarrierBookApptByEmail, False)
                            .LaneCarrierBookApptviaTokenEmail = If(oLane.LaneCarrierBookApptviaTokenEmail, False)
                            .LaneCarrierBookApptviaTokenFailEmail = If(oLane.LaneCarrierBookApptviaTokenFailEmail, "support@nextgeneration.com")
                            .LaneCarrierBookApptviaTokenFailPhone = If(oLane.LaneCarrierBookApptviaTokenFailPhone, "847-963-0007")
                            .BookCarrOrderNumber = oBook.BookCarrOrderNumber 'Modified by RHR for v-8.4.0.003 on 11/2/2021
                        End With

                    Catch ex As Exception
                        ManageLinqDataExceptions(ex, buildProcedureName("GettblServiceTokenByToken"), db)
                    End Try
                End Using
            End If
        Catch ex As Exception
            ManageLinqDataExceptions(ex, buildProcedureName("CarrierBookApptWithTokenData"))
        End Try
        Return oRet
    End Function

    Public Function CarrierAcceptLoadWithTokenData(ByVal strServiceToken As String) As Models.CarrierAcceptLoadWithTokenData
        Dim oRet As New Models.CarrierAcceptLoadWithTokenData()
        Try
            Dim oTokenData As LTS.tblServiceToken = GettblServiceTokenByToken(strServiceToken, 1)
            If Not oTokenData Is Nothing Then
                Using db As New NGLMasBookDataContext(ConnectionString)
                    Try
                        'get the book data 
                        Dim oBook As LTS.Book = db.Books.Where(Function(x) x.BookControl = oTokenData.ServiceTokenBookControl).FirstOrDefault()
                        If oBook Is Nothing OrElse oBook.BookControl = 0 Then
                            throwNoDataFaultMessage("E_InvalidRecordKey")
                        End If
                        Dim oComp As LTS.CompRefBook = db.CompRefBooks.Where(Function(x) x.CompControl = oBook.BookCustCompControl).FirstOrDefault()
                        Dim iLEAdminControl = getLEAdminControlByLegalEntityName(oComp.CompLegalEntity)
                        Dim oLECarData As LTS.vLegalEntityCarrierByLERefBook = db.vLegalEntityCarrierByLERefBooks.Where(Function(x) x.LEAdminControl = iLEAdminControl And x.CarrierControl = oBook.BookCarrierControl).FirstOrDefault()
                        If oLECarData Is Nothing Then throwNoDataFaultMessage("E_InvalidRecordKey")
                        Dim sCarrierName = db.CarrierRefBooks.Where(Function(x) x.CarrierControl = oBook.BookCarrierControl).Select(Function(y) y.CarrierName).FirstOrDefault()
                        With oRet
                            .BookControl = oBook.BookControl
                            .BookDateLoad = oBook.BookDateLoad
                            .BookSHID = oBook.BookSHID
                            .CarrierControl = oBook.BookCarrierControl
                            .CompControl = oBook.BookCustCompControl
                            .CompName = oComp.CompName
                            .BookConsPrefix = oBook.BookConsPrefix
                            .BookDateRequired = oBook.BookDateRequired
                            .ExpirationDate = oTokenData.ServiceTokenExpirationDate
                            .LaneControl = oBook.BookODControl
                            .LECarAllowCarrierAcceptRejectByEmail = If(oLECarData.LECarAllowCarrierAcceptRejectByEmail, False)
                            .LECarCarrierAuthCarrierAcceptRejectByEmail = If(oLECarData.LECarCarrierAuthCarrierAcceptRejectByEmail, False)
                            .LECarCarrierAuthCarrierAcceptRejectExpMin = If(oLECarData.LECarCarrierAuthCarrierAcceptRejectExpMin, 1444)
                            .OriginNameAddressCSZ = String.Format("{0}{1}{3}{0}{1}{4}{0}{1}{5},{2}{6}{2}{7}", "", "<br //>", "&nbsp;", oBook.BookOrigName, oBook.BookOrigAddress1, oBook.BookOrigCity, oBook.BookOrigState, oBook.BookOrigZip)
                            .DestNameAddressCSZ = String.Format("{0}{1}{3}{0}{1}{4}{0}{1}{5},{2}{6}{2}{7}", "", "<br //>", "&nbsp;", oBook.BookDestName, oBook.BookDestAddress1, oBook.BookDestCity, oBook.BookDestState, oBook.BookDestZip)
                            .TokenSupportEmail = GetParText("SupportEmail", oBook.BookCustCompControl)
                            .TokenSupportPhone = GetParText("SupportPhoneNumber", oBook.BookCustCompControl)
                            .CarrierContControl = oBook.BookCarrierContControl
                            .CarrierName = sCarrierName
                        End With

                    Catch ex As Exception
                        ManageLinqDataExceptions(ex, buildProcedureName("CarrierAcceptLoadWithTokenData"), db)
                    End Try
                End Using
            End If
        Catch ex As Exception
            ManageLinqDataExceptions(ex, buildProcedureName("CarrierAcceptLoadWithTokenData"))
        End Try
        Return oRet
    End Function

    Public Function DeleteToken(ByVal iControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        If iControl = 0 Then Return False 'nothing to do
        'validate before delete

        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'Delete the Record
                Dim oToDelete = db.tblServiceTokens.Where(Function(x) x.ServiceTokenControl = iControl).FirstOrDefault()
                If oToDelete Is Nothing OrElse oToDelete.ServiceTokenControl = 0 Then Return True 'already deleted

                db.tblServiceTokens.DeleteOnSubmit(oToDelete)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteToken"), db)
            End Try
        End Using
        Return blnRet
    End Function

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.tblServiceToken)
        Return selectLTSData(d, Me.Parameters.UserName)
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GettblServiceTokenFiltered(Control:=CType(LinqTable, LTS.tblServiceToken).ServiceTokenControl)
    End Function

    Public Function QuickSaveResults(ByVal ServiceTokenControl As Integer) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                ret = (From d In db.tblServiceTokens
                       Where d.ServiceTokenControl = ServiceTokenControl
                       Select New DTO.QuickSaveResults With {.Control = d.ServiceTokenControl _
                                                            , .ModDate = d.ServiceTokenModDate _
                                                            , .ModUser = d.ServiceTokenModUser _
                                                            , .Updated = d.ServiceTokenUpdated.ToArray}).FirstOrDefault()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("QuickSaveResults"))
            End Try

        End Using
        Return ret
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim source As LTS.tblServiceToken = TryCast(LinqTable, LTS.tblServiceToken)
        If source Is Nothing Then Return Nothing
        Return QuickSaveResults(source.ServiceTokenControl)
    End Function

    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'We do not allow Service Type records to be deleted 
        Utilities.SaveAppError("Cannot delete data.  Records cannot be deleted using this interface!", Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))
    End Sub

    Friend Shared Function selectDTOData(ByVal d As LTS.tblServiceToken, ByRef db As NGLMASLookupDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.tblServiceToken
        Dim oDTO As New DTO.tblServiceToken()
        Dim skipObjs As New List(Of String) From {"ServiceTokenUpdated",
                                                  "Page",
                                                  "Pages",
                                                  "RecordCount",
                                                  "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .ServiceTokenUpdated = d.ServiceTokenUpdated.ToArray()
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oDTO

    End Function

    ''' <summary>
    ''' Typically used when we want to insert a new LTS object in the DB
    ''' </summary>
    ''' <param name="d"></param>
    ''' <param name="UserName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function selectLTSData(ByVal d As DTO.tblServiceToken, ByVal UserName As String) As LTS.tblServiceToken
        ' If Updated is null, set to byte[0] (for inserted items).
        Dim oLTS As New LTS.tblServiceToken
        UpdateLTSWithDTO(d, oLTS, UserName)
        Return oLTS

    End Function

    Friend Shared Sub UpdateLTSWithDTO(ByRef d As DTO.tblServiceToken, ByRef t As LTS.tblServiceToken, ByVal UserName As String)
        Dim blnNewLTS As Boolean = False 'used to determine if we allow the BookItemData to be set,  existing LTS objects cannot update the BookItemUpdated value.
        If t.ServiceTokenControl = 0 Then blnNewLTS = True 'in this case we use a new Byte or the current value in d

        Dim skipObjs As New List(Of String) From {"ServiceTokenModDate", "ServiceTokenModUser", "ServiceTokenUpdated"}
        t = CopyMatchingFields(t, d, skipObjs)
        With t
            .ServiceTokenModDate = Date.Now
            .ServiceTokenModUser = UserName
            If blnNewLTS Then .ServiceTokenUpdated = If(d.ServiceTokenUpdated Is Nothing, New Byte() {}, d.ServiceTokenUpdated)
        End With
    End Sub


#End Region

End Class

Public Class NGLChartOfAccountData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLookupDataContext(ConnectionString)
        Me.LinqTable = db.ChartOfAccounts
        Me.LinqDB = db
        Me.SourceClass = "NGLChartOfAccountData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLookupDataContext(ConnectionString)
            Me.LinqTable = db.ChartOfAccounts
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
        Return GetChartOfAccountFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetChartOfAccountsFiltered()
    End Function

    Public Function GetChartOfAccountFiltered(Optional ByVal Control As Integer = 0, Optional ByVal AcctNo As String = "") As DTO.ChartOfAccount
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim ChartOfAccount As DTO.ChartOfAccount = (
                From d In db.ChartOfAccounts
                Where
                    (d.ID = If(Control = 0, d.ID, Control)) _
                     And
                     (AcctNo Is Nothing OrElse String.IsNullOrEmpty(AcctNo) OrElse d.AcctNo = AcctNo)
                Select New DTO.ChartOfAccount With {.ID = d.ID _
                                              , .AcctNo = d.AcctNo _
                                              , .AcctDescription = d.AcctDescription _
                                              , .AcctType = d.AcctType _
                                              , .AcctLine = d.AcctLine _
                                              , .AcctLineNumber = d.AcctLineNumber _
                                              , .AcctLinNumberSub = d.AcctLinNumberSub _
                                              , .AcctUpdated = d.AcctUpdated.ToArray()}).First


                Return ChartOfAccount

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

    Public Function GetChartOfAccountsFiltered(Optional ByVal AcctType As String = "") As DTO.ChartOfAccount()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim ChartOfAccounts() As DTO.ChartOfAccount = (
                From d In db.ChartOfAccounts
                Where
                    (AcctType Is Nothing OrElse String.IsNullOrEmpty(AcctType) OrElse d.AcctType = AcctType)
                Order By d.ID
                Select New DTO.ChartOfAccount With {.ID = d.ID _
                                              , .AcctNo = d.AcctNo _
                                              , .AcctDescription = d.AcctDescription _
                                              , .AcctType = d.AcctType _
                                              , .AcctLine = d.AcctLine _
                                              , .AcctLineNumber = d.AcctLineNumber _
                                              , .AcctLinNumberSub = d.AcctLinNumberSub _
                                              , .AcctUpdated = d.AcctUpdated.ToArray()}).ToArray()
                Return ChartOfAccounts

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
    ''' Get all ChartOfAccounts using AllFilters object
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.2.007 on 04/18/2023
    ''' </remarks>
    Public Function GetChartOfAccounts(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As DTO.ChartOfAccount()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As DTO.ChartOfAccount = New DTO.ChartOfAccount() {}
        Dim oLTS() As LTS.ChartOfAccount
        'Dim intCompNumberFrom As Integer = 0
        'Dim intCompNumberTo As Integer = 0
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.ChartOfAccount)
                iQuery = db.ChartOfAccounts
                Dim filterWhere As String = ""
                Dim sFilterSpacer As String = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                db.Log = New DebugTextWriter
                oLTS = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                oRet = (From d In oLTS Select selectDTOData(d)).ToArray()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrierEquipCodes"), db)
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' Delete ChartOfAccount by ID
    ''' </summary>
    ''' <param name="ID"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.2.007 on 04/18/2023
    ''' </remarks>
    Public Function DeleteChartOfAccount(ByVal ID As Integer) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Dim nObject = db.ChartOfAccounts.Where(Function(x) x.ID = ID).FirstOrDefault()
            If Not nObject Is Nothing AndAlso nObject.ID <> 0 Then
                Try

                    db.ChartOfAccounts.DeleteOnSubmit(nObject)
                    db.SubmitChanges()
                    blnRet = True
                Catch ex As Exception
                    ManageLinqDataExceptions(ex, buildProcedureName("DeleteChartOfAccount"), db)
                End Try
            End If
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Convert LTS ChartOfAccount to DTO ChartOfAccount object
    ''' </summary>
    ''' <param name="d"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.2.007 on 04/18/2023
    ''' </remarks>
    Public Function selectDTOData(ByVal d As LTS.ChartOfAccount) As DTO.ChartOfAccount

        Dim dtoRecord As New DTO.ChartOfAccount()
        If (d IsNot Nothing) Then
            Dim skipObjs As New List(Of String)({"AcctUpdated", "rowguid"})
            Dim sMsg As String = ""
            dtoRecord = DTran.CopyMatchingFields(dtoRecord, d, skipObjs, sMsg)
        End If
        If (dtoRecord IsNot Nothing) Then
            'dtoRecord.AcctUpdated = d.AcctUpdated
            dtoRecord.AcctUpdated = If(d.AcctUpdated Is Nothing, New Byte() {}, d.AcctUpdated.ToArray())
        End If
        Return dtoRecord
    End Function


#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.ChartOfAccount)
        'Create New Record
        Return New LTS.ChartOfAccount With {.ID = d.ID _
                                              , .AcctNo = d.AcctNo _
                                              , .AcctDescription = d.AcctDescription _
                                              , .AcctType = d.AcctType _
                                              , .AcctLine = d.AcctLine _
                                              , .AcctLineNumber = d.AcctLineNumber _
                                              , .AcctLinNumberSub = d.AcctLinNumberSub _
                                              , .AcctUpdated = If(d.AcctUpdated Is Nothing, New Byte() {}, d.AcctUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetChartOfAccountFiltered(Control:=CType(LinqTable, LTS.ChartOfAccount).ID)
    End Function

#End Region

End Class



Public Class NGLtblLanguageCodeData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLookupDataContext(ConnectionString)
        Me.LinqTable = db.tblLanguageCodes
        Me.LinqDB = db
        Me.SourceClass = "NGLtblLanguageCodeData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLookupDataContext(ConnectionString)
            _LinqTable = db.tblLanguageCodes
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
        Return GettblLanguageCodeFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GettblLanguageCodesFiltered()
    End Function

    ''' <summary>
    ''' Enter zero for LowerControl to return the record with the lowest control PK
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="LowerControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetFirstRecord(ByVal LowerControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass

        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim tblLanguageCode As DTO.tblLanguageCode

                If LowerControl <> 0 Then
                    tblLanguageCode = (
                   From d In db.tblLanguageCodes
                   Where d.LCControl >= LowerControl
                   Order By d.LCControl
                   Select selectDTOData(d, db)).FirstOrDefault
                Else
                    'Zero indicates that we should get the record with the lowest control number even if it is below zero
                    tblLanguageCode = (
                   From d In db.tblLanguageCodes
                   Order By d.LCControl
                   Select selectDTOData(d, db)).FirstOrDefault
                End If



                Return tblLanguageCode

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
    ''' Enter the CurrentControl number to use as the reference to the previous record
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetPreviousRecord(ByVal CurrentControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the first record that matches the provided criteria
                Dim tblLanguageCode As DTO.tblLanguageCode = (
                From d In db.tblLanguageCodes
                Where d.LCControl < CurrentControl
                Order By d.LCControl Descending
                Select selectDTOData(d, db)).FirstOrDefault()


                Return tblLanguageCode

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
    ''' Enter the CurrentControl number to use as the reference to the next record
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetNextRecord(ByVal CurrentControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the first record that matches the provided criteria
                Dim tblLanguageCode As DTO.tblLanguageCode = (
                From d In db.tblLanguageCodes
                Where d.LCControl > CurrentControl
                Order By d.LCControl
                Select selectDTOData(d, db)).FirstOrDefault


                Return tblLanguageCode

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
    ''' Enter zero as the UpperControl number return the record with the highest control PK
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="UpperControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetLastRecord(ByVal UpperControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim tblLanguageCode As DTO.tblLanguageCode

                If UpperControl <> 0 Then

                    tblLanguageCode = (
                    From d In db.tblLanguageCodes
                    Where d.LCControl >= UpperControl
                    Order By d.LCControl
                    Select selectDTOData(d, db)).FirstOrDefault
                Else
                    'Zero indicates that we should get the hightest LCcontrol record
                    tblLanguageCode = (
                    From d In db.tblLanguageCodes
                    Order By d.LCControl Descending
                    Select selectDTOData(d, db)).FirstOrDefault

                End If


                Return tblLanguageCode

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

    Public Function GettblLanguageCodeFiltered(ByVal Control As Integer) As DTO.tblLanguageCode
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim tblLanguageCode As DTO.tblLanguageCode = (
                From d In db.tblLanguageCodes
                Where
                    d.LCControl = Control
                Select selectDTOData(d, db)).First

                Return tblLanguageCode

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

    Public Function GettblLanguageCodesFiltered(Optional ByVal page As Integer = 1, Optional ByVal pagesize As Integer = 1000) As DTO.tblLanguageCode()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1
                'count the record

                Try
                    intRecordCount = getScalarInteger("select COUNT(*) from dbo.tblLanguageCode")

                Catch ex As Exception
                    'ignore any record count errors
                End Try
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize

                'Return all the contacts that match the criteria sorted by name
                Dim tblLanguageCodes() As DTO.tblLanguageCode = (
                From d In db.tblLanguageCodes
                Order By d.LCControl
                Select selectDTOData(d, db, page, intPageCount, intRecordCount, pagesize)).Skip(intSkip).Take(pagesize).ToArray()
                Return tblLanguageCodes

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
    ''' Used by System processes forces delete and bypasses validation 
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <remarks></remarks>
    Public Sub SystemDelete(ByVal oData As DTO.tblLanguageCode)
        Using db As New NGLMASLookupDataContext(ConnectionString)
            'Create New Record
            Dim nObject = CopyDTOToLinq(oData)
            db.tblLanguageCodes.Attach(nObject, True)
            db.tblLanguageCodes.DeleteOnSubmit(nObject)
            Try
                db.SubmitChanges()
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch conflictEx As ChangeConflictException
                Try
                    'Improper Reference to LinqDB Fixed by RHR 2/6/15
                    'Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(LinqDB))
                    Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(db))
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
            DeleteCleanUp(nObject)
        End Using
    End Sub

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.tblLanguageCode)
        'Create New Record
        Return New LTS.tblLanguageCode With {.LCControl = d.LCControl _
                                    , .LCName = d.LCName _
                                    , .LCDesc = d.LCDesc _
                                    , .LCCode = d.LCCode _
                                    , .LCModDate = Date.Now _
                                    , .LCModUser = Parameters.UserName _
                                    , .LCUpdated = If(d.LCUpdated Is Nothing, New Byte() {}, d.LCUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GettblLanguageCodeFiltered(Control:=CType(LinqTable, LTS.tblLanguageCode).LCControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim source As LTS.tblLanguageCode = TryCast(LinqTable, LTS.tblLanguageCode)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.tblLanguageCodes
                       Where d.LCControl = source.LCControl
                       Select New DTO.QuickSaveResults With {.Control = d.LCControl _
                                                            , .ModDate = d.LCModDate _
                                                            , .ModUser = d.LCModUser _
                                                            , .Updated = d.LCUpdated.ToArray}).First

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

    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'We do not allow Class Type records to be deleted 
        Utilities.SaveAppError("Cannot delete data.  Records cannot be deleted using this interface!", Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))
    End Sub

    Friend Function selectDTOData(ByVal d As LTS.tblLanguageCode, ByRef db As NGLMASLookupDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.tblLanguageCode
        Return New DTO.tblLanguageCode With {.LCControl = d.LCControl _
                                                      , .LCName = d.LCName _
                                                      , .LCDesc = d.LCDesc _
                                                      , .LCCode = d.LCCode _
                                                      , .LCModDate = d.LCModDate _
                                                      , .LCModUser = d.LCModUser,
                                                       .Page = page,
                                                       .Pages = pagecount,
                                                       .RecordCount = recordcount,
                                                       .PageSize = pagesize _
                                                      , .LCUpdated = d.LCUpdated.ToArray()}
    End Function

#End Region

End Class




Public Class NGLLookupGroupTypeData : Inherits NGLLinkDataBaseClass

#Region "Constructors"

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLookupDataContext(ConnectionString)
        Me.LinqTable = db.tblLookupGroupTypes
        Me.LinqDB = db
        Me.SourceClass = "NGLLookupGroupTypeData"
    End Sub

#End Region

#Region "Properties"

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLookupDataContext(ConnectionString)
            _LinqTable = db.tblLookupGroupTypes
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Returns LGT groupings by referencing table
    ''' </summary>
    ''' <param name="referencingTable"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by LVV for v-8.1 on 1/24/18 LookupLists
    ''' </remarks>
    Public Function GetLookupGroupingsByTable(ByVal referencingTable As String) As DTO.vLookupList()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim vList = (
                        From t In db.tblLookupGroupTypes
                        Where t.LGTDesc = referencingTable
                        Order By t.LGTName
                        Select New DTO.vLookupList _
                        With {.Control = t.LGTControl, .Name = t.LGTName, .Description = t.LGTDesc.ToString}).ToArray()

                Return vList

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
        End Using
        Return Nothing
    End Function

#End Region

#Region "Protected Methods"


#End Region

End Class



Public Class NGLtblPackageDescriptionData : Inherits NGLLinkDataBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLookupDataContext(ConnectionString)
        Me.LinqTable = db.tblPackageDescriptions
        Me.LinqDB = db
        Me.SourceClass = "NGLtblPackageDescriptionData"
    End Sub

#End Region

#Region " Properties "


    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLookupDataContext(ConnectionString)
            _LinqTable = db.tblPackageDescriptions
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Returns the Package descriptions assoicated with a specific package description record, PkgDescControl, 
    ''' or for the current users Legal Entity when a PkgDescControl is not provided as a filter or as    Booking Record the BookControl value must be provided in the filters.ParentControl parameter
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 04/30/2019
    '''     reads one or more Package Discrption records.  
    ''' </remarks>
    Public Function GettblPackageDescriptions(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer, Optional ByVal blnUseParameterForOverride As Boolean = True) As LTS.tblPackageDescription()
        If filters Is Nothing Then Return Nothing
        Dim filterWhere As String = ""
        Dim sFilterSpacer As String = ""
        Dim iPkgDescControl As Integer = 0
        Dim iPkgDescLEAdminControl As Integer = 0
        Dim oRet() As LTS.tblPackageDescription
        iPkgDescLEAdminControl = Me.Parameters.UserLEControl
        If iPkgDescLEAdminControl = 0 Then Return Nothing 'Not allowed
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'Get the data iqueryable
                Dim iQuery As IQueryable(Of LTS.tblPackageDescription)
                iQuery = db.tblPackageDescriptions
                filterWhere = " (PkgDescLEAdminControl = " & iPkgDescLEAdminControl & ") "
                sFilterSpacer = " And "
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GettblPackageDescriptions"), db)
            End Try
        End Using
        Return oRet
    End Function



    ''' <summary>
    ''' Saves or Inserts a Book Package Record.  
    ''' The BookPkgPalletTypeID is required and cannot be zero or empty if missing the system will use 
    ''' PalletTypeID 19 for Pallets
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 10/07/2018
    '''     save dispatching Package records.  This data is not the actual pallet counts assigned to the load 
    '''     only the Packages/pallets that are needed for dispatching and rating
    ''' </remarks>
    Public Function InsertOrUpdatetblPackageDescription(ByVal oData As LTS.tblPackageDescription) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                If oData Is Nothing Then throwNoDataFaultException()
                Dim iPkgDescLEAdminControl As Integer = Me.Parameters.UserLEControl
                If iPkgDescLEAdminControl = 0 Then throwInvalidRequiredKeysException("Package Description", "Legal Entity")
                oData.PkgDescLEAdminControl = iPkgDescLEAdminControl
                If String.IsNullOrWhiteSpace(oData.PkgDescName) Then
                    throwInvalidRequiredKeysException("Package Description", "Package Description Name")
                End If
                Dim sPkgName = oData.PkgDescName
                Dim sPkgNameSource = sPkgName
                Dim iPkgDescControl = oData.PkgDescControl
                Dim iCt As Integer = 0
                While db.tblPackageDescriptions.Any(Function(x) x.PkgDescName = sPkgName And x.PkgDescLEAdminControl = iPkgDescLEAdminControl And (iPkgDescControl = 0 OrElse x.PkgDescControl <> iPkgDescControl))
                    'sPkgNames must be unique
                    iCt += 1
                    sPkgName = sPkgNameSource & "-" & iCt.ToString()
                    oData.PkgDescName = sPkgName
                End While
                oData.PkgDescModDate = Date.Now()
                oData.PkgDescModUser = Me.Parameters.UserName
                If oData.PkgDescControl = 0 Then
                    'get the legal entity
                    'Insert
                    db.tblPackageDescriptions.InsertOnSubmit(oData)
                Else
                    'Update
                    db.tblPackageDescriptions.Attach(oData, True)
                End If
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertOrUpdatetblPackageDescription"), db)
            End Try
        End Using
        Return blnRet
    End Function

    Public Function DeletetblPackageDescription(ByVal iPkgDescControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        If iPkgDescControl = 0 Then Return False 'nothing to do
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'verify the service contract
                Dim oExisting = db.tblPackageDescriptions.Where(Function(x) x.PkgDescControl = iPkgDescControl).FirstOrDefault()
                If oExisting Is Nothing OrElse oExisting.PkgDescControl = 0 Then Return True
                db.tblPackageDescriptions.DeleteOnSubmit(oExisting)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeletetblPackageDescription"), db)
            End Try
        End Using
        Return blnRet
    End Function


#End Region

#Region "Protected Functions"


#End Region

End Class


Public Class NGLtblImageTypeCodeData : Inherits NGLLinkDataBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLookupDataContext(ConnectionString)
        Me.LinqTable = db.tblImageTypeCodes
        Me.LinqDB = db
        Me.SourceClass = "NGLtblImageTypeCodeData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLookupDataContext(ConnectionString)
            Me.LinqTable = db.tblImageTypeCodes
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"


    ''' <summary>
    ''' Return the image type code record by primary key
    ''' </summary>
    ''' <param name="iImageTypeCodeControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.4.003 on 10/24/2023
    ''' </remarks>
    Public Function GettblImageTypeCode(ByVal iImageTypeCodeControl As Integer) As LTS.tblImageTypeCode
        Dim oRet As New LTS.tblImageTypeCode()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                oRet = db.tblImageTypeCodes.Where(Function(x) x.ImageTypeCodeControl = iImageTypeCodeControl).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GettblImageTypeCode"), db)
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' get all of the filtered image type code records
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.4.003 on 10/24/2023
    ''' </remarks>
    Public Function GettblImageTypeCodes(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.tblImageTypeCode()
        If filters Is Nothing Then Return Nothing
        Dim iControl As Integer = 0 'PK Control Number
        Dim filterWhere As String = ""
        Dim sFilterSpacer As String = ""

        Dim oRet() As LTS.tblImageTypeCode
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.tblImageTypeCode)
                iQuery = db.tblImageTypeCodes
                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "ImageTypeCodeName"
                    filters.sortDirection = "asc"
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                'db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GettblImageTypeCodes"), db)
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' Insert a new record or updates an existing record in the tblImageTypeCode table using the ImageTypeCodeControl as the key,  if zero an insert is performed.  
    ''' Note: the Name, ImageTypeCodeName, must be unique for the table or an exception is thrown
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.4.003 on 10/24/2023
    ''' </remarks>
    Public Function InsertOrUpdatetblImageTypeCode(ByVal oData As LTS.tblImageTypeCode) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                If oData Is Nothing Then throwNoDataFaultException()
                Dim iControl As Integer = oData.ImageTypeCodeControl
                Dim sImageTypeCodeName As String = oData.ImageTypeCodeName
                If (String.IsNullOrWhiteSpace(sImageTypeCodeName)) Then
                    throwInvalidRequiredKeysException(" Image Type Code", " Code Name ", False)
                End If
                If (iControl = 0) Then
                    If (db.tblImageTypeCodes.Any(Function(x) x.ImageTypeCodeName = sImageTypeCodeName)) Then
                        throwInvalidKeyAlreadyExistsException(" Image Type Code", " Code Name", sImageTypeCodeName, False)
                    End If
                    'Insert
                    db.tblImageTypeCodes.InsertOnSubmit(oData)
                Else
                    If (db.tblImageTypeCodes.Any(Function(x) x.ImageTypeCodeName = sImageTypeCodeName AndAlso x.ImageTypeCodeControl <> iControl)) Then
                        throwInvalidKeyAlreadyExistsException(" Image Type Code", " Code Name", sImageTypeCodeName, False)
                    End If
                    'Update
                    db.tblImageTypeCodes.Attach(oData, True)
                End If
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertOrUpdatetblImageTypeCode"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Delete a record from the tblImageTypeCode table using the primary key value in iImageTypeCodeControl
    ''' </summary>
    ''' <param name="iImageTypeCodeControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.4.003 on 10/24/2023
    ''' </remarks>
    Public Function DeletetblImageTypeCode(ByVal iImageTypeCodeControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        If iImageTypeCodeControl = 0 Then Return False 'nothing to do
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                'verify the service contract
                Dim oExisting = db.tblImageTypeCodes.Where(Function(x) x.ImageTypeCodeControl = iImageTypeCodeControl).FirstOrDefault()
                If oExisting Is Nothing OrElse oExisting.ImageTypeCodeControl = 0 Then Return True
                db.tblImageTypeCodes.DeleteOnSubmit(oExisting)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeletetblImageTypeCode"), db)
            End Try
        End Using
        Return blnRet
    End Function

#End Region

#Region "Protected Functions"

#End Region

End Class





