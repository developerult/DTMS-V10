Imports System.Data.Linq
Imports System.ServiceModel
Imports Ngl.FreightMaster.Data.DataTransferObjects

Public Class NGLCarrierTariffNoDriveDays : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMASCarrierDataContext(ConnectionString)
        Me.LinqTable = db.CarrierTariffNoDriveDays
        Me.LinqDB = db
        Me.SourceClass = "NGLCarrierTariffNoDriveDays"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASCarrierDataContext(ConnectionString)
                _LinqTable = db.CarrierTariffNoDriveDays
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
        Return GetCarrTarNDDFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    Public Function GetCarrTarNDDFiltered(ByVal Control As Integer) As DataTransferObjects.CarrTarNoDriveDays
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim oRecord As DataTransferObjects.CarrTarNoDriveDays = (
                        From d In db.CarrierTariffNoDriveDays
                        Where
                        d.CarrTarNDDControl = Control
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

    Public Function GetCarrTarNDDsFiltered(ByVal CarrTarControl As Integer,
                                           Optional ByVal page As Integer = 1,
                                           Optional ByVal pagesize As Integer = 1000) As DataTransferObjects.CarrTarNoDriveDays()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try


                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1

                'db.Log = New DebugTextWriter

                Dim oQuery = From d In db.CarrierTariffNoDriveDays
                        Where d.CarrTarNDDCarrTarControl = CarrTarControl
                        Select d

                intRecordCount = oQuery.Count
                If intRecordCount < 1 Then Return Nothing
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize

                Dim oRecords() As DataTransferObjects.CarrTarNoDriveDays = (
                        From d In oQuery
                        Order By d.CarrTarNDDNoDrivingDate Descending
                        Select selectDTOData(d, page, intPageCount, intRecordCount, pagesize)
                        ).Skip(intSkip).Take(pagesize).ToArray()

                Return oRecords

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetCarrTarNDDsFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetCarrTarNDDNodes(ByVal CarrTarControl As Integer, ByVal ParentTreeID As Integer) As List(Of NGLTreeNode)
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'db.Log = New DebugTextWriter
                Dim oNodes As List(Of NGLTreeNode) = (
                        From d In db.CarrierTariffNoDriveDays
                        Where (d.CarrTarNDDCarrTarControl = CarrTarControl)
                        Order By d.CarrTarNDDNoDrivingDate Descending
                        Select New DataTransferObjects.NGLTreeNode With {.Control = d.CarrTarNDDControl,
                        .ParentTreeID = ParentTreeID,
                        .Name = d.CarrTarNDDNoDrivingDate.ToShortDateString,
                        .Description = " | CarrTarNoDriveDays | ",
                        .ClassName = "CarrTarNoDriveDays"}).ToList

                Return oNodes

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                Return Nothing
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetCarrTarNDDNodes"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetCarrTarNDDTree(ByVal CarrTarControl As Integer, ByVal ParentTreeID As Integer, ByRef intNextTreeID As Integer) As List(Of NGLTreeNode)
        Try
            Dim oTreeNodes As List(Of NGLTreeNode) = GetCarrTarNDDNodes(CarrTarControl, ParentTreeID)
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
            throwUnExpectedFaultException(ex, buildProcedureName("GetCarrTarNDDTree"))
        End Try
        Return Nothing
    End Function

    Public Function GetCarrTarNDDTreeFlat(ByVal CarrTarControl As Integer, ByVal ParentTreeID As Integer, ByRef intNextTreeID As Integer) As List(Of NGLTreeNode)
        Try

            Dim oTreeNodes As List(Of NGLTreeNode) = GetCarrTarNDDNodes(CarrTarControl, ParentTreeID)
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
            throwUnExpectedFaultException(ex, buildProcedureName("GetCarrTarNDDTreeFlat"))
        End Try
        Return Nothing
    End Function

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim t = CType(oData, DataTransferObjects.CarrTarNoDriveDays)
        'Create New Record
        Return New LTS.CarrierTariffNoDriveDay With {.CarrTarNDDControl = t.CarrTarNDDControl _
            , .CarrTarNDDCarrTarControl = t.CarrTarNDDCarrTarControl _
            , .CarrTarNDDNoDrivingDate = t.CarrTarNDDNoDrivingDate _
            , .CarrTarNDDModDate = Date.Now _
            , .CarrTarNDDModUser = Parameters.UserName _
            , .CarrTarNDDUpdated = If(t.CarrTarNDDUpdated Is Nothing, New Byte() {}, t.CarrTarNDDUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCarrTarNDDFiltered(Control:=CType(LinqTable, LTS.CarrierTariffNoDriveDay).CarrTarNDDControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim source As LTS.CarrierTariffNoDriveDay = TryCast(LinqTable, LTS.CarrierTariffNoDriveDay)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.CarrierTariffNoDriveDays
                    Where d.CarrTarNDDControl = source.CarrTarNDDControl
                    Select New DataTransferObjects.QuickSaveResults With {.Control = d.CarrTarNDDControl _
                        , .ModDate = d.CarrTarNDDModDate _
                        , .ModUser = d.CarrTarNDDModUser _
                        , .Updated = d.CarrTarNDDUpdated.ToArray}).First

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

    Private Sub ValidateApproved(ByRef oDB As NGLMASCarrierDataContext, ByRef oData As DataTransferObjects.CarrTarNoDriveDays)
        DirectCast(NDPBaseClassFactory("NGLCarrTarContractData", False), NGLCarrTarContractData).ValidateApproved(oData.CarrTarNDDCarrTarControl, oDB)
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

    Friend Function selectDTOData(ByVal d As LTS.CarrierTariffNoDriveDay, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.CarrTarNoDriveDays
        Return New DataTransferObjects.CarrTarNoDriveDays With {.CarrTarNDDControl = d.CarrTarNDDControl _
            , .CarrTarNDDCarrTarControl = d.CarrTarNDDCarrTarControl _
            , .CarrTarNDDNoDrivingDate = d.CarrTarNDDNoDrivingDate _
            , .CarrTarNDDModDate = d.CarrTarNDDModDate _
            , .CarrTarNDDModUser = d.CarrTarNDDModUser _
            , .CarrTarNDDUpdated = d.CarrTarNDDUpdated.ToArray() _
            , .Page = page _
            , .Pages = pagecount _
            , .RecordCount = recordcount _
            , .PageSize = pagesize}
    End Function



#End Region

End Class