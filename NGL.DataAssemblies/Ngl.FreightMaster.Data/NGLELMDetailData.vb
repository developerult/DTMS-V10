Imports System.ServiceModel

Public Class NGLELMDetailData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Me.Parameters = oParameters
        Dim db As New NGLMASIntegrationDataContext(ConnectionString)
        Me.LinqTable = db.ELMDetails
        Me.LinqDB = db
        Me.SourceClass = "NGLELMDetailData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASIntegrationDataContext(ConnectionString)
            _LinqTable = db.ELMDetails
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
        Return GetELMDetailFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetELMDetails()
    End Function

    Public Function GetELMDetailFiltered(Optional ByVal Control As Long = 0) As DataTransferObjects.ELMDetail
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try

                Dim ELMDetail As DataTransferObjects.ELMDetail = (
                        From d In db.ELMDetails
                        Where
                        (d.ELMControl = If(Control = 0, d.ELMControl, Control))
                        Select selectDTOData(d, db)).First

                Return ELMDetail

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


    Public Function GetELMDetails(Optional ByVal page As Integer = 1, Optional ByVal pagesize As Integer = 1000) As DataTransferObjects.ELMDetail()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try

                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1
                'count the record

                Try
                    intRecordCount = getScalarInteger("select COUNT(dbo.ELMDetail.ELMControl) from dbo.ELMDetail")

                Catch ex As Exception
                    'ignore any record count errors
                End Try
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize
                'Return all the contacts that match the criteria sorted by name
                Dim ELMDetail() As DataTransferObjects.ELMDetail = (
                        From d In db.ELMDetails
                        Order By d.ELMControl
                        Select selectDTOData(d, db, page, intPageCount, intRecordCount, pagesize)).Skip(intSkip).Take(pagesize).ToArray()

                Return ELMDetail

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
        Dim d = CType(oData, DataTransferObjects.ELMDetail)
        'Create New Record
        Return New LTS.ELMDetail With {.ELMControl = d.ELMControl _
            , .ELMInitial = d.ELMInitial _
            , .ELMNumber = d.ELMNumber _
            , .ELMSentDateTime = If(d.ELMSentDateTime.HasValue, d.ELMSentDateTime, Nothing) _
            , .ELMSightingCity = d.ELMSightingCity _
            , .ELMSightingStateProvinceCountry = d.ELMSightingStateProvinceCountry _
            , .ELMSightingDate = If(d.ELMSightingDate.HasValue, d.ELMSightingDate, Nothing) _
            , .ELMSightingHour = d.ELMSightingHour _
            , .ELMSightingMinute = d.ELMSightingMinute _
            , .ELMSightingSPLC = d.ELMSightingSPLC _
            , .ELMStatus = d.ELMStatus _
            , .ELMSightingEvent = d.ELMSightingEvent _
            , .ELMDestinationCity = d.ELMDestinationCity _
            , .ELMDestinationStateProvinceCountry = d.ELMDestinationStateProvinceCountry _
            , .ELMIDCode = d.ELMIDCode _
            , .ELMReportingSCAC = d.ELMReportingSCAC _
            , .ELMAEIReidIndicator = d.ELMAEIReidIndicator _
            , .ELMETADestinationCity = d.ELMETADestinationCity _
            , .ELMETADestinationStateProvinceCountry = d.ELMETADestinationStateProvinceCountry _
            , .ELMETADateTime = If(d.ELMETADateTime.HasValue, d.ELMETADateTime, Nothing) _
            , .ELMEventTypeCode = d.ELMEventTypeCode _
            , .ELMGrossWeight = d.ELMGrossWeight _
            , .ELMTareWeight = d.ELMTareWeight _
            , .ELMNetWeight = d.ELMNETWeight _
            , .ELMWeightCode = d.ELMWeightCode _
            , .ELMWeightDateTime = If(d.ELMWeightDateTime.HasValue, d.ELMWeightDateTime, Nothing) _
            , .ELMWeightLocationCity = d.ELMWeightLocationCity _
            , .ELMWeightLocationStateProvinceCountry = d.ELMWeightLocationStateProvinceCountry _
            , .ELMWayBillDateTime = If(d.ELMWayBillDateTime.HasValue, d.ELMWayBillDateTime, Nothing) _
            , .ELMWeightIndicator = d.ELMWeightIndicator _
            , .ELMAllowance = d.ELMAllowance _
            , .ELMBookControl = d.ELMBookControl _
            , .ActionModDate = If(d.ELMActionModDate.HasValue, d.ELMActionModDate, Nothing) _
            , .ActionModUser = d.ELMActionModUser _
            , .ActionUpdated = If(d.ELMActionUpdated Is Nothing, New Byte() {}, d.ELMActionUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetELMDetailFiltered(Control:=CType(LinqTable, LTS.ELMDetail).ELMControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim source As LTS.ELMDetail = TryCast(LinqTable, LTS.ELMDetail)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.ELMDetails
                    Where d.ELMControl = source.ELMControl
                    Select New DataTransferObjects.QuickSaveResults With {.Control = d.ELMControl _
                        , .ModDate = d.ActionModDate _
                        , .ModUser = d.ActionModUser _
                        , .Updated = d.ActionUpdated.ToArray}).First

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

    Friend Function selectDTOData(ByVal d As LTS.ELMDetail, ByRef db As NGLMASIntegrationDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.ELMDetail
        Return New DataTransferObjects.ELMDetail With {.ELMControl = d.ELMControl _
            , .ELMInitial = d.ELMInitial _
            , .ELMNumber = d.ELMNumber _
            , .ELMSentDateTime = If(d.ELMSentDateTime.HasValue, d.ELMSentDateTime, Nothing) _
            , .ELMSightingCity = d.ELMSightingCity _
            , .ELMSightingStateProvinceCountry = d.ELMSightingStateProvinceCountry _
            , .ELMSightingDate = If(d.ELMSightingDate.HasValue, d.ELMSightingDate, Nothing) _
            , .ELMSightingHour = d.ELMSightingHour _
            , .ELMSightingMinute = d.ELMSightingMinute _
            , .ELMSightingSPLC = d.ELMSightingSPLC _
            , .ELMStatus = d.ELMStatus _
            , .ELMSightingEvent = d.ELMSightingEvent _
            , .ELMDestinationCity = d.ELMDestinationCity _
            , .ELMDestinationStateProvinceCountry = d.ELMDestinationStateProvinceCountry _
            , .ELMIDCode = d.ELMIDCode _
            , .ELMReportingSCAC = d.ELMReportingSCAC _
            , .ELMAEIReidIndicator = d.ELMAEIReidIndicator _
            , .ELMETADestinationCity = d.ELMETADestinationCity _
            , .ELMETADestinationStateProvinceCountry = d.ELMETADestinationStateProvinceCountry _
            , .ELMETADateTime = If(d.ELMETADateTime.HasValue, d.ELMETADateTime, Nothing) _
            , .ELMEventTypeCode = d.ELMEventTypeCode _
            , .ELMGrossWeight = d.ELMGrossWeight _
            , .ELMTareWeight = d.ELMTareWeight _
            , .ELMNETWeight = d.ELMNetWeight _
            , .ELMWeightCode = d.ELMWeightCode _
            , .ELMWeightDateTime = If(d.ELMWeightDateTime.HasValue, d.ELMWeightDateTime, Nothing) _
            , .ELMWeightLocationCity = d.ELMWeightLocationCity _
            , .ELMWeightLocationStateProvinceCountry = d.ELMWeightLocationStateProvinceCountry _
            , .ELMWayBillDateTime = If(d.ELMWayBillDateTime.HasValue, d.ELMWayBillDateTime, Nothing) _
            , .ELMWeightIndicator = d.ELMWeightIndicator _
            , .ELMAllowance = d.ELMAllowance _
            , .ELMBookControl = d.ELMBookControl _
            , .ELMActionModDate = If(d.ActionModDate.HasValue, d.ActionModDate, Nothing) _
            , .ELMActionModUser = d.ActionModUser,
            .Page = page,
            .Pages = pagecount,
            .RecordCount = recordcount,
            .PageSize = pagesize _
            , .ELMActionUpdated = d.ActionUpdated.ToArray()}
    End Function

#End Region

End Class