Public Class NGLtblIntegrationTypeData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Me.Parameters = oParameters
        Dim db As New NGLMASIntegrationDataContext(ConnectionString)
        Me.LinqTable = db.tblIntegrationTypes
        Me.LinqDB = db
        Me.SourceClass = "NGLtblIntegrationTypeData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If (_LinqTable Is Nothing) Then
                Dim db As New NGLMASIntegrationDataContext(ConnectionString)
                _LinqTable = db.tblIntegrationTypes
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
        Return GettblIntegrationTypeFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GettblIntegrationTypesFiltered()
    End Function

    Public Function GettblIntegrationTypeFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.tblIntegrationType
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim tblIntegrationType As DataTransferObjects.tblIntegrationType = (
                        From d In db.tblIntegrationTypes
                        Where
                        (d.IntegrationTypeControl = If(Control = 0, d.IntegrationTypeControl, Control))
                        Select selectDTOData(d, db)).FirstOrDefault()
                Return tblIntegrationType

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GettblIntegrationTypeFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GettblIntegrationTypesFiltered() As DataTransferObjects.tblIntegrationType()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'db.Log = New DebugTextWriter
                'Return all the contacts that match the criteria 
                Dim tblIntegrationTypes() As DataTransferObjects.tblIntegrationType = (
                        From d In db.tblIntegrationTypes
                        Select selectDTOData(d, db)).ToArray()
                Return tblIntegrationTypes

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GettblIntegrationTypesFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DataTransferObjects.tblIntegrationType)
        Return selectLTSData(d, Me.Parameters.UserName)

    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Dim oData As LTS.tblIntegrationType = TryCast(LinqTable, LTS.tblIntegrationType)
        If oData Is Nothing Then Return Nothing
        Return GettblIntegrationTypeFiltered(Control:=oData.IntegrationTypeControl)
    End Function

    Public Function QuickSaveResults(ByVal IntegrationTypeControl As Integer) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                ret = (From d In db.tblIntegrationTypes
                    Where d.IntegrationTypeControl = IntegrationTypeControl
                    Select New DataTransferObjects.QuickSaveResults With {.Control = d.IntegrationTypeControl _
                        , .ModDate = d.IntegrationTypeModDate _
                        , .ModUser = d.IntegrationTypeModUser _
                        , .Updated = d.IntegrationTypeUpdated.ToArray}).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("QuickSaveResults"))
            End Try
        End Using
        Return ret
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim source As LTS.tblIntegrationType = TryCast(LinqTable, LTS.tblIntegrationType)
        If source Is Nothing Then Return Nothing
        Return QuickSaveResults(source.IntegrationTypeControl)
    End Function

    Friend Shared Function selectDTOData(ByVal d As LTS.tblIntegrationType, ByRef db As NGLMASIntegrationDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.tblIntegrationType

        Dim oDTO As New DataTransferObjects.tblIntegrationType
        Dim skipObjs As New List(Of String) From {"IntegrationTypeUpdated",
                "Page",
                "Pages",
                "RecordCount",
                "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .IntegrationTypeUpdated = d.IntegrationTypeUpdated.ToArray()
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
    Friend Shared Function selectLTSData(ByVal d As DataTransferObjects.tblIntegrationType, ByVal UserName As String) As LTS.tblIntegrationType
        ' If Updated is null, set to byte[0] (for inserted items).
        Dim oLTS As New LTS.tblIntegrationType
        UpdateLTSWithDTO(d, oLTS, UserName)
        Return oLTS

    End Function

    ''' <summary>
    ''' Typically used to update an existing LTS object
    ''' </summary>
    ''' <param name="d"></param>
    ''' <param name="t"></param>
    ''' <param name="UserName"></param>
    ''' <remarks></remarks>
    Friend Shared Sub UpdateLTSWithDTO(ByRef d As DataTransferObjects.tblIntegrationType, ByRef t As LTS.tblIntegrationType, ByVal UserName As String)
        Dim skipObjs As New List(Of String) From {"IntegrationTypeModDate", "IntegrationTypeModUser", "IntegrationTypeUpdated"}
        t = CopyMatchingFields(t, d, skipObjs)
        With t
            .IntegrationTypeModDate = Date.Now
            .IntegrationTypeModUser = UserName
            .IntegrationTypeUpdated = If(d.IntegrationTypeUpdated Is Nothing, New Byte() {}, d.IntegrationTypeUpdated)
        End With
    End Sub


#End Region

End Class