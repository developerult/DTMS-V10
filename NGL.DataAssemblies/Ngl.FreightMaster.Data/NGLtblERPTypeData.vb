Public Class NGLtblERPTypeData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Me.Parameters = oParameters
        Dim db As New NGLMASIntegrationDataContext(ConnectionString)
        Me.LinqTable = db.tblERPTypes
        Me.LinqDB = db
        Me.SourceClass = "NGLtblERPTypeData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASIntegrationDataContext(ConnectionString)
            Me.LinqTable = db.tblERPTypes
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
        Return GettblERPTypeFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GettblERPTypesFiltered()

    End Function

    Public Function GettblERPTypeFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.tblERPType
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim tblERPType As DataTransferObjects.tblERPType = (
                        From d In db.tblERPTypes
                        Where
                        (d.ERPTypeControl = If(Control = 0, d.ERPTypeControl, Control))
                        Select selectDTOData(d, db)).FirstOrDefault()
                Return tblERPType

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GettblERPTypeFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GettblERPTypesFiltered() As DataTransferObjects.tblERPType()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'db.Log = New DebugTextWriter
                'Return all the contacts that match the criteria 
                Dim tblERPTypes() As DataTransferObjects.tblERPType = (
                        From d In db.tblERPTypes
                        Select selectDTOData(d, db)).ToArray()
                Return tblERPTypes

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GettblERPTypesFiltered"))
            End Try

            Return Nothing

        End Using
    End Function



#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DataTransferObjects.tblERPType)
        Return selectLTSData(d, Me.Parameters.UserName)

    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Dim oData As LTS.tblERPType = TryCast(LinqTable, LTS.tblERPType)
        If oData Is Nothing Then Return Nothing
        Return GettblERPTypeFiltered(Control:=oData.ERPTypeControl)
    End Function

    Public Function QuickSaveResults(ByVal ERPTypeControl As Integer) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                ret = (From d In db.tblERPTypes
                    Where d.ERPTypeControl = ERPTypeControl
                    Select New DataTransferObjects.QuickSaveResults With {.Control = d.ERPTypeControl _
                        , .ModDate = d.ERPTypeModDate _
                        , .ModUser = d.ERPTypeModUser _
                        , .Updated = d.ERPTypeUpdated.ToArray}).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("QuickSaveResults"))
            End Try
        End Using
        Return ret
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim source As LTS.tblERPType = TryCast(LinqTable, LTS.tblERPType)
        If source Is Nothing Then Return Nothing
        Return QuickSaveResults(source.ERPTypeControl)
    End Function

    Friend Shared Function selectDTOData(ByVal d As LTS.tblERPType, ByRef db As NGLMASIntegrationDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.tblERPType

        Dim oDTO As New DataTransferObjects.tblERPType
        Dim skipObjs As New List(Of String) From {"ERPTypeUpdated",
                "Page",
                "Pages",
                "RecordCount",
                "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .ERPTypeUpdated = d.ERPTypeUpdated.ToArray()
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
    Friend Shared Function selectLTSData(ByVal d As DataTransferObjects.tblERPType, ByVal UserName As String) As LTS.tblERPType
        ' If Updated is null, set to byte[0] (for inserted items).
        Dim oLTS As New LTS.tblERPType
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
    Friend Shared Sub UpdateLTSWithDTO(ByRef d As DataTransferObjects.tblERPType, ByRef t As LTS.tblERPType, ByVal UserName As String)
        Dim skipObjs As New List(Of String) From {"ERPTypeModDate", "ERPTypeModUser", "ERPTypeUpdated"}
        t = CopyMatchingFields(t, d, skipObjs)
        With t
            .ERPTypeModDate = Date.Now
            .ERPTypeModUser = UserName
            .ERPTypeUpdated = If(d.ERPTypeUpdated Is Nothing, New Byte() {}, d.ERPTypeUpdated)
        End With
    End Sub


#End Region

End Class