Imports System.ServiceModel

Public Class NGLERPSettingData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Me.Parameters = oParameters
        Dim db As New NGLMASIntegrationDataContext(ConnectionString)
        Me.LinqTable = db.ERPSettings
        Me.LinqDB = db
        Me.SourceClass = "NGLERPSettingData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASIntegrationDataContext(ConnectionString)
            Me.LinqTable = db.ERPSettings
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
        Return GetERPSettingFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'db.Log = New DebugTextWriter
                'Return all the contacts that match the criteria 
                Dim ERPSettings() As DataTransferObjects.ERPSetting = (
                        From d In db.ERPSettings
                        Select selectDTOData(d, db)).ToArray()
                Return ERPSettings

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetRecordsFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetERPSettingFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.ERPSetting
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim ERPSetting As DataTransferObjects.ERPSetting = (
                        From d In db.ERPSettings
                        Where
                        (d.ERPSettingControl = If(Control = 0, d.ERPSettingControl, Control))
                        Order By d.ERPSettingControl Descending
                        Select selectDTOData(d, db)).FirstOrDefault()
                Return ERPSetting

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetERPSettingFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Returns the view of the ERP Setting by primary key
    ''' </summary>
    ''' <param name="Control"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created for v-8.5.4.004 on 11/13/2023
    ''' </remarks>
    Public Function GetvERPSettingFiltered(ByVal Control As Integer) As LTS.vERPSetting
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim vERPSetting As LTS.vERPSetting = (
                        From d In db.vERPSettings
                        Where (d.ERPSettingControl = Control)
                        Select d).FirstOrDefault()
                Return vERPSetting

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetvERPSettingFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetERPSettingsFiltered(ByVal LegalEntity As String) As DataTransferObjects.ERPSetting()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'db.Log = New DebugTextWriter
                'Return all the contacts that match the criteria 
                Dim ERPSettings() As DataTransferObjects.ERPSetting = (
                        From d In db.ERPSettings
                        Where (d.LegalEntity = LegalEntity)
                        Select selectDTOData(d, db)).ToArray()
                Return ERPSettings

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetERPSettingsFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Returns all of the ERP Settings assigned to a specific Legal Entity
    ''' </summary>
    ''' <param name="iLEControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created for v-8.5.4.004 on 11/13/2023
    ''' </remarks>
    Public Function GetvERPSettingsFiltered(ByVal iLEControl As Integer) As LTS.vERPSetting()
        If iLEControl = 0 Then iLEControl = Me.Parameters.UserLEControl
        If iLEControl = 0 Then Return Nothing
        Dim oData As LTS.vERPSetting() = Nothing

        Try
            Dim sLegalEntity As String
            Using db As New NGLMASCompDataContext(ConnectionString)
                sLegalEntity = db.tblLegalEntityAdmins.Where(Function(x) x.LEAdminControl = iLEControl).Select(Function(y) y.LEAdminLegalEntity).FirstOrDefault()
            End Using
            If String.IsNullOrWhiteSpace(sLegalEntity) Then
                Return Nothing
            Else
                Using db As New NGLMASIntegrationDataContext(ConnectionString)
                    oData = db.vERPSettings.Where(Function(x) x.LegalEntity = sLegalEntity).ToArray()
                End Using
            End If
        Catch ex As Exception
            ManageLinqDataExceptions(ex, buildProcedureName("GetERPSettingsFiltered"))
        End Try

        Return oData


    End Function


    Public Function DeleteERPSetting(ByVal iERPSettingControl As Integer) As Boolean
        Dim blnRet As Boolean = False

        If iERPSettingControl = 0 Then Return False 'nothing to do
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'verify that the data exists
                Dim oExisting = db.ERPSettings.Where(Function(x) x.ERPSettingControl = iERPSettingControl).FirstOrDefault()
                If oExisting Is Nothing OrElse oExisting.ERPSettingControl = 0 Then Return True
                db.ERPSettings.DeleteOnSubmit(oExisting)
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



    Public Function GetERPSettingsFiltered(ByVal LegalEntity As String, ByVal ERPTypeControl As Integer) As DataTransferObjects.ERPSetting()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'db.Log = New DebugTextWriter
                'Return all the contacts that match the criteria 
                Dim ERPSettings() As DataTransferObjects.ERPSetting = (
                        From d In db.ERPSettings
                        Where (d.LegalEntity = LegalEntity) And (d.ERPTypeControl = ERPTypeControl)
                        Select selectDTOData(d, db)).ToArray()
                Return ERPSettings

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetERPSettingsFiltered"))
            End Try

            Return Nothing

        End Using
    End Function


    ''' <summary>
    ''' Returns an array of integration settings denormalized into a view filtered by Legal Entity and ERP Type
    ''' This over load returns all Integration Types
    ''' </summary>
    ''' <param name="LegalEntity"></param>
    ''' <param name="ERPTypeControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR 8/4/2015 v-7.0.4
    ''' </remarks>
    Public Function getvERPIntegrationSettings(ByVal LegalEntity As String, ByVal ERPTypeControl As Integer) As DataTransferObjects.vERPIntegrationSetting()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim vERPSettings() As DataTransferObjects.vERPIntegrationSetting = (
                        From d In db.vERPIntegrationSettings
                        Where (d.LegalEntity = LegalEntity) And (d.ERPTypeControl = ERPTypeControl)
                        Select selectvERPSettingDTOData(d, db)).ToArray()
                Return vERPSettings

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("getvERPIntegrationSettings"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Returns an array of integration settings denormalized into a view filteed by Legal Entity, ERP Type and Integration Type
    ''' This Overload returns all Integration Types and filters by ERP Type Name instead of control numbers
    ''' </summary>
    ''' <param name="LegalEntity"></param>
    ''' <param name="ERPTypeName"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR 8/4/2015 v-7.0.4
    ''' </remarks>
    Public Function getvERPIntegrationSettings(ByVal LegalEntity As String, ByVal ERPTypeName As String) As DataTransferObjects.vERPIntegrationSetting()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim vERPSettings() As DataTransferObjects.vERPIntegrationSetting = (
                        From d In db.vERPIntegrationSettings
                        Where (String.IsNullOrEmpty(LegalEntity) OrElse d.LegalEntity = LegalEntity) And (d.ERPTypeName = ERPTypeName)
                        Select selectvERPSettingDTOData(d, db)).ToArray()
                Return vERPSettings

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("getvERPIntegrationSettings"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Returns one integration setting record denormalized into a view filtered by Legal Entity, ERP Type and Integration Type
    ''' This overload will return one record where the  IntegrationTypeControl matches. The Caller must test for nothing/null
    ''' and for a blank record by testing the IntegrationControl for zero in case the type does not exist.
    ''' </summary>
    ''' <param name="LegalEntity"></param>
    ''' <param name="ERPTypeControl"></param>
    ''' <param name="IntegrationTypeControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR 8/4/2015 v-7.0.4
    ''' </remarks>
    Public Function getvERPIntegrationSetting(ByVal LegalEntity As String, ByVal ERPTypeControl As Integer, ByVal IntegrationTypeControl As Integer) As DataTransferObjects.vERPIntegrationSetting
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim vERPSetting As DataTransferObjects.vERPIntegrationSetting = (
                        From d In db.vERPIntegrationSettings
                        Where (d.LegalEntity = LegalEntity) And (d.ERPTypeControl = ERPTypeControl) And (d.IntegrationTypeControl = IntegrationTypeControl)
                        Select selectvERPSettingDTOData(d, db)).FirstOrDefault()
                Return vERPSetting

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("getvERPIntegrationSetting"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Returns one integration setting record denormalized into a view filteed by Legal Entity, ERP Type and Integration Type
    ''' This will sort the results by the last(largest) IntegrationControl entered for the filters provided
    ''' and return only one record (the last one entered) if more than one match is found for IntegrationTypeName.
    ''' The Caller must test for nothing/null and for a blank record by testing the IntegrationControl for zero 
    ''' in case the type does not exist.
    ''' </summary>
    ''' <param name="LegalEntity"></param>
    ''' <param name="ERPTypeName"></param>
    ''' <param name="IntegrationTypeName"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR 8/4/2015 v-7.0.4
    ''' </remarks>
    Public Function getvERPIntegrationSetting(ByVal LegalEntity As String, ByVal ERPTypeName As String, ByVal IntegrationTypeName As String) As DataTransferObjects.vERPIntegrationSetting
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim vERPSetting As DataTransferObjects.vERPIntegrationSetting = (
                        From d In db.vERPIntegrationSettings
                        Where (d.LegalEntity = LegalEntity) And (d.ERPTypeName = ERPTypeName) And (d.IntegrationTypeName = IntegrationTypeName)
                        Order By d.IntegrationControl Descending
                        Select selectvERPSettingDTOData(d, db)).FirstOrDefault()
                Return vERPSetting

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("getvERPIntegrationSetting"))
            End Try

            Return Nothing

        End Using
    End Function

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DataTransferObjects.ERPSetting)
        Return selectLTSData(d, Me.Parameters.UserName)

    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Dim oData As LTS.ERPSetting = TryCast(LinqTable, LTS.ERPSetting)
        If oData Is Nothing Then Return Nothing
        Return GetERPSettingFiltered(Control:=oData.ERPSettingControl)
    End Function

    Public Function QuickSaveResults(ByVal ERPSettingControl As Integer) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                ret = (From d In db.ERPSettings
                    Where d.ERPSettingControl = ERPSettingControl
                    Select New DataTransferObjects.QuickSaveResults With {.Control = d.ERPSettingControl _
                        , .ModDate = d.ERPSettingModDate _
                        , .ModUser = d.ERPSettingModUser _
                        , .Updated = d.ERPSettingUpdated.ToArray}).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("QuickSaveResults"))
            End Try
        End Using
        Return ret
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim source As LTS.ERPSetting = TryCast(LinqTable, LTS.ERPSetting)
        If source Is Nothing Then Return Nothing
        Return QuickSaveResults(source.ERPSettingControl)
    End Function

    Friend Shared Function selectDTOData(ByVal d As LTS.ERPSetting, ByRef db As NGLMASIntegrationDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.ERPSetting

        Dim oDTO As New DataTransferObjects.ERPSetting
        Dim skipObjs As New List(Of String) From {"ERPSettingUpdated",
                "Page",
                "Pages",
                "RecordCount",
                "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .ERPSettingUpdated = d.ERPSettingUpdated.ToArray()
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oDTO

    End Function


    Friend Shared Function selectvERPSettingDTOData(ByVal d As LTS.vERPIntegrationSetting, ByRef db As NGLMASIntegrationDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.vERPIntegrationSetting

        Dim oDTO As New DataTransferObjects.vERPIntegrationSetting
        Dim skipObjs As New List(Of String) From {"Page",
                "Pages",
                "RecordCount",
                "PageSize"}
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

    ''' <summary>
    ''' Typically used when we want to insert a new LTS object in the DB
    ''' </summary>
    ''' <param name="d"></param>
    ''' <param name="UserName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function selectLTSData(ByVal d As DataTransferObjects.ERPSetting, ByVal UserName As String) As LTS.ERPSetting
        ' If Updated is null, set to byte[0] (for inserted items).
        Dim oLTS As New LTS.ERPSetting
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
    Friend Shared Sub UpdateLTSWithDTO(ByRef d As DataTransferObjects.ERPSetting, ByRef t As LTS.ERPSetting, ByVal UserName As String)
        Dim skipObjs As New List(Of String) From {"ERPSettingModDate", "ERPSettingModUser", "ERPSettingUpdated"}
        t = CopyMatchingFields(t, d, skipObjs)
        With t
            .ERPSettingModDate = Date.Now
            .ERPSettingModUser = UserName
            .ERPSettingUpdated = If(d.ERPSettingUpdated Is Nothing, New Byte() {}, d.ERPSettingUpdated)
        End With
    End Sub


#End Region

End Class