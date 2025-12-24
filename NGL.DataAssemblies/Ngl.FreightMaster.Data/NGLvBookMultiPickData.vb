Imports System.ServiceModel

Public Class NGLvBookMultiPickData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMasBookDataContext(ConnectionString)
        Me.LinqTable = Nothing
        Me.LinqDB = db
        Me.SourceClass = "NGLvBookMultipPickData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMasBookDataContext(ConnectionString)
            _LinqTable = Nothing
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

    Public Function GetvBookMultiPickRecord(ByVal MultiPickConsPrefix As String,
                                            ByVal MultiPickBookControl As Integer,
                                            ByVal MultiPickLocationisOrigin As Boolean) As DataTransferObjects.vBookMultiPick
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Return (
                    From d In db.spGetBookConsMultiPickRecord(MultiPickConsPrefix, MultiPickBookControl, MultiPickLocationisOrigin, Me.Parameters.UserName)
                        Select selectDTOData(d, db)).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetvBookMultiPickRecord"))
            End Try
            Return Nothing
        End Using
    End Function

    Public Function GetvBookMultiPickRecords(ByVal MultiPickConsPrefix As String) As DataTransferObjects.vBookMultiPick()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Return (
                    From d In db.spGetBookConsMultiPick(MultiPickConsPrefix, Me.Parameters.UserName)
                        Select selectDTOData(d, db)).Take(100).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetvBookMultiPickRecords"))
            End Try
            Return Nothing
        End Using
    End Function

    Public Function GetvBookMultiPickRecordsByStopNumber(ByVal MultiPickConsPrefix As String) As DataTransferObjects.vBookMultiPick()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Return (
                    From d In db.spGetBookConsMultiPickByStopNumber(MultiPickConsPrefix, Me.Parameters.UserName)
                        Select selectDTOData(d, db)).Take(100).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetvBookMultiPickRecordsByStopNumber"))
            End Try
            Return Nothing
        End Using
    End Function

    Public Overrides Function Update(Of TEntity As Class)(ByVal oData As Object,
                                                          ByVal oLinqTable As System.Data.Linq.Table(Of TEntity)) As Object
        Dim oVbookMultiPick As DataTransferObjects.vBookMultiPick = TryCast(oData, DataTransferObjects.vBookMultiPick)
        If Not oVbookMultiPick Is Nothing Then
            SaveChanges(oVbookMultiPick)
            Dim updatedMultiPick As DataTransferObjects.vBookMultiPick = GetvBookMultiPickRecord(oVbookMultiPick.MultiPickConsPrefix, oVbookMultiPick.MultiPickBookControl, oVbookMultiPick.MultiPickLocationisOrigin)
            'we set the MultiPickControl number to the previous value because it is dynamic
            updatedMultiPick.MultiPickControl = oVbookMultiPick.MultiPickControl
            Return updatedMultiPick
        End If
        Return Nothing

    End Function

    Public Overrides Sub UpdateNoReturn(Of TEntity As Class)(ByVal oData As Object,
                                                             ByVal oLinqTable As System.Data.Linq.Table(Of TEntity))

        Dim oVbookMultiPick As DataTransferObjects.vBookMultiPick = TryCast(oData, DataTransferObjects.vBookMultiPick)
        If Not oVbookMultiPick Is Nothing Then
            SaveChanges(oVbookMultiPick)
        End If

    End Sub

    Public Function UpdateSequence(ByVal oData As DataTransferObjects.vBookMultiPick) As DataTransferObjects.vBookMultiPick

        Dim oVbookMultiPick As DataTransferObjects.vBookMultiPick = TryCast(oData, DataTransferObjects.vBookMultiPick)
        If Not oVbookMultiPick Is Nothing Then
            SaveStopSequenceChanges(oVbookMultiPick)
            Dim updatedMultiPick As DataTransferObjects.vBookMultiPick = GetvBookMultiPickRecord(oVbookMultiPick.MultiPickConsPrefix, oVbookMultiPick.MultiPickBookControl, oVbookMultiPick.MultiPickLocationisOrigin)
            'we set the MultiPickControl number to the previous value because it is dynamic
            updatedMultiPick.MultiPickControl = oVbookMultiPick.MultiPickControl
            Return updatedMultiPick
        End If
        Return Nothing
    End Function

    Public Overrides Sub Delete(Of TEntity As Class)(ByVal oData As Object,
                                                     ByVal oLinqTable As System.Data.Linq.Table(Of TEntity))
        'We do not allow booking records to be added from this class
        Utilities.SaveAppError("Cannot delete data.  Records cannot be deleted using this interface!", Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))
    End Sub



#End Region

#Region "LTS Methods"


#End Region

#Region "Shared Methods"


    Friend Shared Function selectDTOData(ByVal d As LTS.spGetBookConsMultiPickRecordResult, ByRef db As NGLMasBookDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.vBookMultiPick
        Dim oDTO As New DataTransferObjects.vBookMultiPick
        Dim skipObjs As New List(Of String) From {"Page", "Pages", "RecordCount", "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oDTO
    End Function

    Friend Shared Function selectDTOData(ByVal d As LTS.spGetBookConsMultiPickResult, ByRef db As NGLMasBookDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.vBookMultiPick
        Dim oDTO As New DataTransferObjects.vBookMultiPick
        Dim skipObjs As New List(Of String) From {"Page", "Pages", "RecordCount", "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oDTO
    End Function

    Friend Shared Function selectDTOData(ByVal d As LTS.spGetBookConsMultiPickByStopNumberResult, ByRef db As NGLMasBookDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.vBookMultiPick
        Dim oDTO As New DataTransferObjects.vBookMultiPick
        Dim skipObjs As New List(Of String) From {"Page", "Pages", "RecordCount", "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oDTO
    End Function


#End Region


#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Return Nothing

    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return Nothing
    End Function

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'We do not allow booking records to be added from this class
        Utilities.SaveAppError("Cannot add data.  Records cannot be inserted using this interface!", Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))
    End Sub

    Private Sub SaveChanges(ByVal oData As DataTransferObjects.vBookMultiPick)
        Dim spConfig As New clsNGLSPConfig("UpdateBookConsMultiPick", "dbo.spUpdateBookConsMultiPick", True)
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        With spConfig.oCmd.Parameters
            .AddWithValue("@MultiPickBookControl", oData.MultiPickBookControl)
            .AddWithValue("@MultiPickProNumber", oData.MultiPickProNumber)
            .AddWithValue("@MultiPickConsPrefix", oData.MultiPickConsPrefix)
            .AddWithValue("@MultiPickLocationisOrigin", oData.MultiPickLocationisOrigin)
            .AddWithValue("@MultiPickName", oData.MultiPickName)
            .AddWithValue("@MultiPickAddress1", oData.MultiPickAddress1)
            .AddWithValue("@MultiPickAddress2", oData.MultiPickAddress2)
            .AddWithValue("@MultiPickAddress3", oData.MultiPickAddress3)
            .AddWithValue("@MultiPickCity", oData.MultiPickCity)
            .AddWithValue("@MultiPickState", oData.MultiPickState)
            .AddWithValue("@MultiPickCountry", oData.MultiPickCountry)
            .AddWithValue("@MultiPickZip", oData.MultiPickZip)
            .AddWithValue("@MultiPickPhone", oData.MultiPickPhone)
            .AddWithValue("@MultiPickFax", oData.MultiPickFax)
            .AddWithValue("@MultiPickStopNumber", oData.MultiPickStopNumber)
            .AddWithValue("@MultiPickMiles", oData.MultiPickMiles)
            .AddWithValue("@MultiPickPickNumber", oData.MultiPickPickNumber)
            .AddWithValue("@MultiPickDeliveryStopNumber", oData.MultiPickDeliveryStopNumber)
            .AddWithValue("@MultiPickOrderNumber", oData.MultiPickOrderNumber)
            .AddWithValue("@MultiPickOrderSequence", oData.MultiPickOrderSequence)
            .AddWithValue("@MultiPickTotalOrderMiles", oData.MultiPickTotalOrderMiles)
            .AddWithValue("@MultiPickRouteMiles", oData.MultiPickRouteMiles)
            .AddWithValue("@MultiPickLockAllCosts", oData.MultiPickLockAllCosts)
            .AddWithValue("@MultiPickLockBFCCost", oData.MultiPickLockBFCCost)
            .AddWithValue("@MultiPickBookSHID", oData.MultiPickBookSHID)
            .AddWithValue("@MultiPickBookExpDelDateTime", oData.MultiPickBookExpDelDateTime)
            .AddWithValue("@MultiPickBookMustLeaveByDateTime", oData.MultiPickBookMustLeaveByDateTime)
            .AddWithValue("@MultiPickBookOutOfRouteMiles", oData.MultiPickBookOutOfRouteMiles)
            .AddWithValue("@MultiPickBookSpotRateAllocationFormula", oData.MultiPickBookSpotRateAllocationFormula)
            .AddWithValue("@MultiPickBookSpotRateAutoCalcBFC", oData.MultiPickBookSpotRateAutoCalcBFC)
            .AddWithValue("@MultiPickBookSpotRateUseCarrierFuelAddendum", oData.MultiPickBookSpotRateUseCarrierFuelAddendum)
            .AddWithValue("@MultiPickBookSpotRateBFCAllocationFormula", oData.MultiPickBookSpotRateBFCAllocationFormula)
            .AddWithValue("@MultiPickBookSpotRateTotalUnallocatedBFC", oData.MultiPickBookSpotRateTotalUnallocatedBFC)
            .AddWithValue("@MultiPickBookSpotRateTotalUnallocatedLineHaul", oData.MultiPickBookSpotRateTotalUnallocatedLineHaul)
            .AddWithValue("@MultiPickBookSpotRateUseFuelAddendum", oData.MultiPickBookSpotRateUseFuelAddendum)
            .AddWithValue("@MultiPickBookRevLaneBenchMiles", oData.MultiPickBookRevLaneBenchMiles)
            .AddWithValue("@MultiPickBookRevLoadMiles", oData.MultiPickBookRevLoadMiles)
            .AddWithValue("@MultiPickBookCarrTarControl", oData.MultiPickBookCarrTarControl)
            .AddWithValue("@MultiPickBookCarrTarName", oData.MultiPickBookCarrTarName)
            .AddWithValue("@MultiPickBookCarrTarEquipControl", oData.MultiPickBookCarrTarEquipControl)
            .AddWithValue("@MultiPickBookShipCarrierProControl", oData.MultiPickBookShipCarrierProControl)
            .AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        End With
        processNGLStoredProcedure(spConfig, oSystem)

    End Sub

    Private Sub SaveStopSequenceChanges(ByVal oData As DataTransferObjects.vBookMultiPick)
        Dim spConfig As New clsNGLSPConfig("UpdateBookConsMultiPickStopSequence", "dbo.spUpdateBookConsMultiPickStopSequence", True)
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        With spConfig.oCmd.Parameters
            .AddWithValue("@MultiPickBookControl", oData.MultiPickBookControl)
            .AddWithValue("@MultiPickProNumber", oData.MultiPickProNumber)
            .AddWithValue("@MultiPickConsPrefix", oData.MultiPickConsPrefix)
            .AddWithValue("@MultiPickLocationisOrigin", oData.MultiPickLocationisOrigin)
            .AddWithValue("@MultiPickStopNumber", oData.MultiPickStopNumber)
            .AddWithValue("@MultiPickMiles", oData.MultiPickMiles)
            .AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        End With
        processNGLStoredProcedure(spConfig, oSystem)
    End Sub

#End Region

End Class