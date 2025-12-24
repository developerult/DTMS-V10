Imports System.ServiceModel

Public Class NGLIntegrationData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Me.Parameters = oParameters
        Dim db As New NGLMASIntegrationDataContext(ConnectionString)
        Me.LinqTable = db.Integrations
        Me.LinqDB = db
        Me.SourceClass = "NGLIntegrationData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASIntegrationDataContext(ConnectionString)
            Me.LinqTable = db.Integrations
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
        Return GetIntegrationFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'db.Log = New DebugTextWriter
                'Return all the contacts that match the criteria 
                Dim Integrations() As DataTransferObjects.Integration = (
                        From d In db.Integrations
                        Select selectDTOData(d, db)).ToArray()
                Return Integrations

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetRecordsFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetIntegrationFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.Integration
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim Integration As DataTransferObjects.Integration = (
                        From d In db.Integrations
                        Where
                        (d.IntegrationControl = If(Control = 0, d.IntegrationControl, Control))
                        Order By d.IntegrationControl Descending
                        Select selectDTOData(d, db)).FirstOrDefault()
                Return Integration

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetIntegrationFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Returns a view of the ERP Integration settings by primary key
    ''' </summary>
    ''' <param name="Control"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created for v-8.5.4.004 on 11/13/2023
    ''' </remarks>
    Public Function GetvIntegrationFiltered(ByVal Control As Integer) As LTS.vIntegration
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim Integration As LTS.vIntegration = (
                        From d In db.vIntegrations
                        Where
                        (d.IntegrationControl = If(Control = 0, d.IntegrationControl, Control))
                        Order By d.IntegrationControl Descending
                        Select d).FirstOrDefault()
                Return Integration

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetIntegrationFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Returns a view of all the ERP Integration settings  assigned to a ERP Setting Header
    ''' </summary>
    ''' <param name="ERPSettingControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created for v-8.5.4.004 on 11/13/2023
    ''' </remarks>
    Public Function GetvIntegrationsFiltered(ByVal ERPSettingControl As Integer) As LTS.vIntegration()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'db.Log = New DebugTextWriter
                'Return all the contacts that match the criteria 
                Dim Integrations() As LTS.vIntegration = (
                        From d In db.vIntegrations
                        Where (d.ERPSettingControl = ERPSettingControl)
                        Select d).ToArray()
                Return Integrations

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetIntegrationsFiltered"))
            End Try

            Return Nothing

        End Using
    End Function


    Public Function GetIntegrationsFiltered(ByVal ERPSettingControl As Integer) As DataTransferObjects.Integration()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'db.Log = New DebugTextWriter
                'Return all the contacts that match the criteria 
                Dim Integrations() As DataTransferObjects.Integration = (
                        From d In db.Integrations
                        Where (d.ERPSettingControl = ERPSettingControl)
                        Select selectDTOData(d, db)).ToArray()
                Return Integrations

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetIntegrationsFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetIntegrationsFiltered(ByVal ERPSettingControl As Integer, ByVal IntegrationTypeControl As Integer) As DataTransferObjects.Integration()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'db.Log = New DebugTextWriter
                'Return all the contacts that match the criteria 
                Dim Integrations() As DataTransferObjects.Integration = (
                        From d In db.Integrations
                        Where (d.ERPSettingControl = ERPSettingControl) And (d.IntegrationTypeControl = IntegrationTypeControl)
                        Select selectDTOData(d, db)).ToArray()
                Return Integrations

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetIntegrationsFiltered"))
            End Try

            Return Nothing

        End Using
    End Function



    Public Function DeleteIntegration(ByVal iIntegrationControl As Integer) As Boolean
        Dim blnRet As Boolean = False

        If iIntegrationControl = 0 Then Return False 'nothing to do
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'verify that the data exists
                Dim oExisting = db.Integrations.Where(Function(x) x.IntegrationControl = iIntegrationControl).FirstOrDefault()
                If oExisting Is Nothing OrElse oExisting.ERPSettingControl = 0 Then Return True
                db.Integrations.DeleteOnSubmit(oExisting)
                db.SubmitChanges()

                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteERPSetting"), db)
            End Try
        End Using
        Return blnRet
    End Function


#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DataTransferObjects.Integration)
        Return selectLTSData(d, Me.Parameters.UserName)

    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Dim oData As LTS.Integration = TryCast(LinqTable, LTS.Integration)
        If oData Is Nothing Then Return Nothing
        Return GetIntegrationFiltered(Control:=oData.IntegrationControl)
    End Function

    Public Function QuickSaveResults(ByVal IntegrationControl As Integer) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                ret = (From d In db.Integrations
                    Where d.IntegrationControl = IntegrationControl
                    Select New DataTransferObjects.QuickSaveResults With {.Control = d.IntegrationControl _
                        , .ModDate = d.IntegrationModDate _
                        , .ModUser = d.IntegrationModUser _
                        , .Updated = d.IntegrationUpdated.ToArray}).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("QuickSaveResults"))
            End Try
        End Using
        Return ret
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim source As LTS.Integration = TryCast(LinqTable, LTS.Integration)
        If source Is Nothing Then Return Nothing
        Return QuickSaveResults(source.IntegrationControl)
    End Function

    Friend Shared Function selectDTOData(ByVal d As LTS.Integration, ByRef db As NGLMASIntegrationDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.Integration

        Dim oDTO As New DataTransferObjects.Integration
        Dim skipObjs As New List(Of String) From {"IntegrationUpdated",
                "Page",
                "Pages",
                "RecordCount",
                "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .IntegrationUpdated = d.IntegrationUpdated.ToArray()
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
    Friend Shared Function selectLTSData(ByVal d As DataTransferObjects.Integration, ByVal UserName As String) As LTS.Integration
        ' If Updated is null, set to byte[0] (for inserted items).
        Dim oLTS As New LTS.Integration
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
    Friend Shared Sub UpdateLTSWithDTO(ByRef d As DataTransferObjects.Integration, ByRef t As LTS.Integration, ByVal UserName As String)
        Dim skipObjs As New List(Of String) From {"IntegrationModDate", "IntegrationModUser", "IntegrationUpdated"}
        t = CopyMatchingFields(t, d, skipObjs)
        With t
            .IntegrationModDate = Date.Now
            .IntegrationModUser = UserName
            .IntegrationUpdated = If(d.IntegrationUpdated Is Nothing, New Byte() {}, d.IntegrationUpdated)
        End With
    End Sub


#End Region

End Class