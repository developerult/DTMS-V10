Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.ServiceModel
Imports System.Text.RegularExpressions
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports LTS = Ngl.FreightMaster.Data.LTS
Imports Ngl.Core.ChangeTracker
Imports DTran = Ngl.Core.Utility.DataTransformation
Imports Ngl.FreightMaster.Data.Utilities
Imports System.Linq.Dynamic
Imports NGL.FM.P44

Public Class NGLtblReportListData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        'SprocessParameters(oParameters)

        Me.SourceClass = "NGLtblReportListData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASSecurityDataContext(ConnectionString)
                _LinqTable = db.tblReportLists
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
        Return GettblReportListFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GettblReportListsFiltered()
    End Function

    Public Function GettblReportListFiltered(Optional ByVal Control As Integer = 0, Optional ByVal ReportName As String = "") As DTO.tblReportList
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim oDLO As New DataLoadOptions
                oDLO.LoadWith(Of LTS.tblReportList)(Function(t As LTS.tblReportList) t.tblReportPars)
                db.LoadOptions = oDLO
                'Return all the contacts that match the criteria sorted by name
                Dim tblReportList As DTO.tblReportList = (
                From t In db.tblReportLists
                Order By t.ReportName
                Where
                    (ReportName Is Nothing OrElse String.IsNullOrEmpty(ReportName) OrElse t.ReportName = ReportName) _
                    And
                    (t.ReportControl = If(Control = 0, t.ReportControl, Control))
                Select New DTO.tblReportList With {.ReportControl = t.ReportControl,
                                                   .ReportName = t.ReportName,
                                                   .ReportDescription = t.ReportDescription,
                                                   .ReportServerURL = t.ReportServerURL,
                                                   .ReportURL = t.ReportURL,
                                                   .ReportPrinterName = t.ReportPrinterName,
                                                   .ReportDataSource = t.ReportDataSource,
                                                   .AMSActive = t.AMSActive,
                                                   .ReportReportMenuControl = t.ReportReportMenuControl,
                                                   .ReportMenuSequence = t.ReportMenuSequence,
                                                   .ReportNEXTrackActive = t.ReportNEXTrackActive,
                                                   .ReportShowInTree = t.ReportShowInTree,
                                                   .ReportUpdated = t.ReportUpdated.ToArray(),
                                                   .tblReportPars = (
                                                    From d In t.tblReportPars
                                                    Select New DTO.tblReportPar With {.ReportParControl = d.ReportParControl,
                                                                                    .ReportParReportControl = d.ReportParReportControl,
                                                                                    .ReportParReportParTypeControl = d.ReportParReportParTypeControl,
                                                                                    .ReportParName = d.ReportParName,
                                                                                    .ReportParText = d.ReportParText,
                                                                                    .ReportParSource = d.ReportParSource,
                                                                                    .ReportParValueField = d.ReportParValueField,
                                                                                    .ReportParTextField = d.ReportParTextField,
                                                                                    .ReportParSortOrder = d.ReportParSortOrder,
                                                                                    .ReportParApplyUserName = d.ReportParApplyUserName,
                                                                                    .ReportParDefaultValue = d.ReportParDefaultValue,
                                                                                    .ReportParUpdated = d.ReportParUpdated.ToArray()}).ToList()}).Single()
                Return tblReportList

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
    ''' The AMSActiveFlag is used to determine which records to return 
    ''' 0 = all records
    ''' 1 = only records where AMSActive is true
    ''' Any other value returns records where AMSActive is False
    ''' </summary>
    ''' <param name="AMSActiveFlag"></param>
    ''' <param name="ReportMenuControl">
    ''' Optional if zero or missing all reports are returned except menu item 12 and 13
    ''' 12 = Dashboard reports
    ''' 13 = Fax reports
    ''' </param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GettblReportListsFiltered(Optional ByVal AMSActiveFlag As Integer = 0, Optional ByVal ReportMenuControl As Integer = 0) As DTO.tblReportList()
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim tblReportLists() As DTO.tblReportList = (
                From t In db.tblReportLists
                Where
                    (AMSActiveFlag = 0 OrElse t.AMSActive = True) _
                    And
                    ((ReportMenuControl = 0 And t.ReportReportMenuControl <> 12 And t.ReportReportMenuControl <> 13) _
                    OrElse
                    t.ReportReportMenuControl = ReportMenuControl)
                Order By t.ReportName
                Select New DTO.tblReportList With {.ReportControl = t.ReportControl, .ReportName = t.ReportName, .ReportDescription = t.ReportDescription, .ReportServerURL = t.ReportServerURL, .ReportURL = t.ReportURL, .ReportPrinterName = t.ReportPrinterName, .ReportDataSource = t.ReportDataSource, .AMSActive = t.AMSActive, .ReportReportMenuControl = t.ReportReportMenuControl, .ReportMenuSequence = t.ReportMenuSequence, .ReportNEXTrackActive = t.ReportNEXTrackActive, .ReportShowInTree = t.ReportShowInTree, .ReportUpdated = t.ReportUpdated.ToArray()}).ToArray()
                Return tblReportLists

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

    Public Function GettblReportListsFiltered(ByVal ReportMenuControl As Integer) As DTO.tblReportList()
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim tblReportLists() As DTO.tblReportList = (
                From t In db.tblReportLists
                Where (t.ReportReportMenuControl = ReportMenuControl)
                Order By t.ReportName
                Select New DTO.tblReportList With {.ReportControl = t.ReportControl, .ReportName = t.ReportName, .ReportDescription = t.ReportDescription, .ReportServerURL = t.ReportServerURL, .ReportURL = t.ReportURL, .ReportPrinterName = t.ReportPrinterName, .ReportDataSource = t.ReportDataSource, .AMSActive = t.AMSActive, .ReportReportMenuControl = t.ReportReportMenuControl, .ReportMenuSequence = t.ReportMenuSequence, .ReportNEXTrackActive = t.ReportNEXTrackActive, .ReportShowInTree = t.ReportShowInTree, .ReportUpdated = t.ReportUpdated.ToArray()}).ToArray()
                Return tblReportLists

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


    Public Function GetReportsTreeFlat(ByVal userName As String) As DTO.ReportsTree
        If String.IsNullOrEmpty(userName) Then Return New DTO.ReportsTree

        Dim StandardReportsfiltered As List(Of DTO.tblReportList)
        Dim CustomReportsfiltered As List(Of DTO.tblReportList)
        Dim AllUnRestrictedReports() As DTO.tblReportList
        Using db As New NGLMASSecurityDataContext(ConnectionString)

            Try
                Dim provider As New NGLSecurityDataProvider(Me.Parameters)
                Dim oUser As DTO.tblUserSecurity = provider.GettblUserSecurityByUserName(userName)
                'get menus.
                Dim tblReportMenus() As DTO.tblReportMenu = (
                   From t In db.tblReportMenus
                   Order By t.ReportMenuName
                   Select New DTO.tblReportMenu With {.ReportMenuControl = t.ReportMenuControl, .ReportMenuName = t.ReportMenuName, .ReportMenuDescription = t.ReportMenuDescription, .ReportMenuSequence = t.ReportMenuSequence, .ReportMenuUpdated = t.ReportMenuUpdated.ToArray()}).ToArray()

                AllUnRestrictedReports = provider.GetUnRestrictedReportsByUser(oUser.UserSecurityControl)
                StandardReportsfiltered =
                                        (From report In AllUnRestrictedReports
                                         Where report.ReportControl < Utilities.ONETHOUSAND
                                         Select report).ToList


                CustomReportsfiltered = (From report In AllUnRestrictedReports
                                         Where report.ReportControl >= Utilities.ONETHOUSAND
                                         Select report).ToList


                Dim standardTreeId = 0
                Dim customTreeId = 0
                Dim intNextTreeID As Integer = 0
                Dim intParentID As Integer = 0
                Dim intNestedParentID As Integer = 0
                Dim oTree As New DTO.ReportsTree With {.UserName = userName, .TreeID = incrementID(intNextTreeID), .ParentTreeID = intParentID, .Name = "Reports", .Description = "Reports Tree"}
                intParentID = intNextTreeID

                Dim oStarndard As New DTO.NEXTrackTreeNode With {.TreeID = incrementID(intNextTreeID), .ParentTreeID = intParentID, .Name = "Standard", .Text = "Standard", .Description = "Standard"}
                If oTree.Items Is Nothing Then oTree.Items = New List(Of DTO.NEXTrackTreeNode)
                oTree.HasChildren = True
                oTree.Items.Add(oStarndard)
                standardTreeId = oStarndard.TreeID

                Dim oCustom As New DTO.NEXTrackTreeNode With {.TreeID = incrementID(intNextTreeID), .ParentTreeID = intParentID, .Name = "Custom", .Text = "Custom", .Description = "Custom"}
                If oTree.Items Is Nothing Then oTree.Items = New List(Of DTO.NEXTrackTreeNode)
                oTree.HasChildren = True
                oTree.Items.Add(oCustom)
                customTreeId = oCustom.TreeID

                For Each item In tblReportMenus
                    Dim oCategoryStandard As New DTO.NEXTrackTreeNode With {.TreeID = incrementID(intNextTreeID), .ParentTreeID = standardTreeId, .ClassName = "tblReportMenu", .Name = item.ReportMenuName, .Text = item.ReportMenuName, .Description = item.ReportMenuDescription}
                    If oTree.Items Is Nothing Then oTree.Items = New List(Of DTO.NEXTrackTreeNode)
                    intParentID = oCategoryStandard.TreeID
                    Dim reportsStandardCatfiltered As List(Of DTO.NEXTrackTreeNode)
                    reportsStandardCatfiltered = GetReportMenuTreeFlat(item.ReportMenuControl, StandardReportsfiltered, intParentID, intNextTreeID)
                    If reportsStandardCatfiltered IsNot Nothing AndAlso reportsStandardCatfiltered.Count > 0 Then
                        oStarndard.HasChildren = True
                        oStarndard.Items.Add(oCategoryStandard)
                        oCategoryStandard.HasChildren = True
                        oCategoryStandard.Items.AddRange(reportsStandardCatfiltered)
                    End If

                    Dim oCategoryCustom As New DTO.NEXTrackTreeNode With {.TreeID = incrementID(intNextTreeID), .ParentTreeID = customTreeId, .ClassName = "tblReportMenu", .Name = item.ReportMenuName, .Text = item.ReportMenuName, .Description = item.ReportMenuDescription}
                    If oTree.Items Is Nothing Then oTree.Items = New List(Of DTO.NEXTrackTreeNode)
                    intParentID = oCategoryCustom.TreeID
                    Dim reportsCustomCatfiltered As List(Of DTO.NEXTrackTreeNode) = GetReportMenuTreeFlat(item.ReportMenuControl, CustomReportsfiltered, intParentID, intNextTreeID)
                    If reportsCustomCatfiltered IsNot Nothing AndAlso reportsCustomCatfiltered.Count > 0 Then
                        oCustom.HasChildren = True
                        oCustom.Items.Add(oCategoryCustom)
                        oCategoryCustom.HasChildren = True
                        oCategoryCustom.Items.AddRange(reportsCustomCatfiltered)
                    End If
                Next

                Return oTree
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
        End Using
    End Function

    Private Function GetReportMenuTreeFlat(ByVal MenuControl As Integer,
                                           ByVal reports As List(Of DTO.tblReportList),
                                           ByVal ParentTreeID As Integer,
                                           ByRef intNextTreeID As Integer) As List(Of DTO.NEXTrackTreeNode)
        Try

            Dim oTreeNodes As List(Of DTO.NEXTrackTreeNode) = GetReportTreeNodes(MenuControl, reports, ParentTreeID)
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
            throwUnExpectedFaultException(ex, buildProcedureName("GetCarrTarClassXrefTreeFlat"))
        End Try
        Return Nothing
    End Function

    Private Function GetReportTreeNodes(ByVal MenuControl As Integer,
                                           ByVal reports As List(Of DTO.tblReportList),
                                           ByVal ParentTreeID As Integer) As List(Of DTO.NEXTrackTreeNode)
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'db.Log = New DebugTextWriter
                Dim oNodes As List(Of DTO.NEXTrackTreeNode) = (
                From d In reports
                Where (d.ReportReportMenuControl = MenuControl)
                Order By d.ReportName
                Select New DTO.NEXTrackTreeNode With {.Control = d.ReportControl,
                                                 .ParentTreeID = ParentTreeID,
                                                 .Name = d.ReportName,
                                                 .Text = d.ReportName,
                                                 .Description = d.ReportDescription,
                                                 .ClassName = "tblReportList"}).ToList

                Return oNodes

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                Return Nothing
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetReportTreeNodes"))
            End Try

            Return Nothing

        End Using
    End Function

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.tblReportList)
        'Create New Record
        Return New LTS.tblReportList With {.ReportControl = d.ReportControl,
                                          .ReportName = d.ReportName,
                                          .ReportDescription = d.ReportDescription,
                                          .ReportServerURL = d.ReportServerURL,
                                          .ReportURL = d.ReportURL,
                                          .ReportPrinterName = d.ReportPrinterName,
                                          .ReportDataSource = d.ReportDataSource,
                                          .AMSActive = d.AMSActive,
                                          .ReportReportMenuControl = d.ReportReportMenuControl,
                                          .ReportMenuSequence = d.ReportMenuSequence,
                                          .ReportNEXTrackActive = d.ReportNEXTrackActive,
                                          .ReportShowInTree = d.ReportShowInTree,
                                          .ReportUpdated = If(d.ReportUpdated Is Nothing, New Byte() {}, d.ReportUpdated)}

    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GettblReportListFiltered(Control:=CType(LinqTable, LTS.tblReportList).ReportControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim source As LTS.tblReportList = TryCast(LinqTable, LTS.tblReportList)
                If source Is Nothing Then Return Nothing
                'Note this data source does not have a Mod Date or Mod User data field
                ret = (From d In db.tblReportLists
                       Where d.ReportControl = source.ReportControl
                       Select New DTO.QuickSaveResults With {.Control = d.ReportControl _
                                                            , .ModDate = Date.Now _
                                                            , .ModUser = Parameters.UserName _
                                                            , .Updated = d.ReportUpdated.ToArray}).First

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
        'Check if the data already exists only one allowed
        With CType(oData, DTO.tblReportList)
            Try
                Dim tblReportList As DTO.tblReportList = (
                    From t In CType(oDB, NGLMASSecurityDataContext).tblReportLists
                    Where
                         t.ReportName = .ReportName
                    Select New DTO.tblReportList With {.ReportControl = t.ReportControl}).First

                If Not tblReportList Is Nothing Then
                    Utilities.SaveAppError("Cannot save new tblReportList data.  The tblReportList name, " & .ReportName & " ,  already exist.", Me.Parameters)
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
        With CType(oData, DTO.tblReportList)
            Try
                'Get the newest record that matches the provided criteria
                Dim tblReportList As DTO.tblReportList = (
                    From t In CType(oDB, NGLMASSecurityDataContext).tblReportLists
                    Where
                         t.ReportControl <> .ReportControl And t.ReportName = .ReportName
                    Select New DTO.tblReportList With {.ReportControl = t.ReportControl}).First


                If Not tblReportList Is Nothing Then
                    Utilities.SaveAppError("Cannot save tblReportList changes.  The tblReportList name, " & .ReportName & " ,  already exist.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    Protected Overrides Sub AddDetailsToLinq(ByRef LinqTable As Object, ByRef oData As DTO.DTOBaseClass)

        With CType(LinqTable, LTS.tblReportList)
            'Add tblReportList parameter Records
            .tblReportPars.AddRange(
                From d In CType(oData, DTO.tblReportList).tblReportPars
                Select New LTS.tblReportPar With {.ReportParControl = d.ReportParControl,
                                                .ReportParReportControl = d.ReportParReportControl,
                                                .ReportParReportParTypeControl = d.ReportParReportParTypeControl,
                                                .ReportParName = d.ReportParName,
                                                .ReportParText = d.ReportParText,
                                                .ReportParSource = d.ReportParSource,
                                                .ReportParValueField = d.ReportParValueField,
                                                .ReportParTextField = d.ReportParTextField,
                                                .ReportParSortOrder = d.ReportParSortOrder,
                                                .ReportParApplyUserName = d.ReportParApplyUserName,
                                                .ReportParDefaultValue = d.ReportParDefaultValue,
                                                .ReportParUpdated = If(d.ReportParUpdated Is Nothing, New Byte() {}, d.ReportParUpdated)})


        End With
    End Sub

    Protected Overrides Sub InsertAllDetails(ByRef oDB As System.Data.Linq.DataContext, ByRef LinqTable As Object)
        With CType(oDB, NGLMASSecurityDataContext)
            .tblReportPars.InsertAllOnSubmit(CType(LinqTable, LTS.tblReportList).tblReportPars)
        End With

    End Sub

    Protected Overrides Sub ProcessUpdatedDetails(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        With CType(oDB, NGLMASSecurityDataContext)
            ' Process any inserted contact records 
            .tblReportPars.InsertAllOnSubmit(GettblReportParsChanges(oData, TrackingInfo.Created))
            ' Process any updated contact records
            .tblReportPars.AttachAll(GettblReportParsChanges(oData, TrackingInfo.Updated), True)
            ' Process any deleted contact records
            Dim deletedtblReportPars = GettblReportParsChanges(oData, TrackingInfo.Deleted)
            .tblReportPars.AttachAll(deletedtblReportPars, True)
            .tblReportPars.DeleteAllOnSubmit(deletedtblReportPars)


        End With
    End Sub

    Protected Function GettblReportParsChanges(ByVal source As DTO.tblReportList, ByVal changeType As TrackingInfo) As List(Of LTS.tblReportPar)
        ' Tease out order details with specified change type.
        ' If Updated is null, set to byte[0] (for inserted items).
        Dim details As IEnumerable(Of LTS.tblReportPar) = (
          From d In source.tblReportPars
          Where d.TrackingState = changeType
          Select New LTS.tblReportPar With {.ReportParControl = d.ReportParControl,
                                            .ReportParReportControl = d.ReportParReportControl,
                                            .ReportParReportParTypeControl = d.ReportParReportParTypeControl,
                                            .ReportParName = d.ReportParName,
                                            .ReportParText = d.ReportParText,
                                            .ReportParSource = d.ReportParSource,
                                            .ReportParValueField = d.ReportParValueField,
                                            .ReportParTextField = d.ReportParTextField,
                                            .ReportParSortOrder = d.ReportParSortOrder,
                                            .ReportParApplyUserName = d.ReportParApplyUserName,
                                            .ReportParDefaultValue = d.ReportParDefaultValue,
                                            .ReportParUpdated = If(d.ReportParUpdated Is Nothing, New Byte() {}, d.ReportParUpdated)})
        Return details.ToList()
    End Function
#End Region

End Class

Public Class NGLSecurityDataProvider
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

#Region "NEXTRACk Standard Dashboards"

    Public Function GetCarrierOnTimePerf(ByVal enter_Year As Integer, ByVal from_Carrier As Integer, ByVal to_Carrier As Integer, ByVal userName As String) As DTO.spNGL063SSRSResult

        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Dim record As New DTO.spNGL063SSRSResult
            Try
                'if there are no null values to deal with we can build the array directly
                Dim recordResult As Object = (
                    From t In db.spNGL063SSRS(enter_Year, from_Carrier, to_Carrier, userName))
                Dim sumOnTime As Integer = 0
                Dim sumEarly As Integer = 0
                Dim suumLoads As Integer = 0
                Dim onTimePerf As Decimal = 0


                For Each item As LTS.spNGL063SSRSResult In recordResult
                    sumEarly = sumEarly + item.Early
                    sumOnTime = sumOnTime + item.OnTime
                    suumLoads = suumLoads + item.Loads
                Next
                If suumLoads > 0 Then
                    onTimePerf = ((sumOnTime + sumEarly) / suumLoads) * 100
                    record.ValuePerf = onTimePerf
                    Return record
                End If

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
            record.ValuePerf = 0
            Return record

        End Using

    End Function

    'Public Function GetDOEFuel(ByVal startDate As DateTime) As Object

    '    Using db As New NGLMASCompDataContext(ConnectionString)
    '        Try
    '            'if there are no null values to deal with we can build the array directly
    '            Try
    '                executeSQL("Select NatFuelCost, NatFuelDate From NatFuel Where NatFuelDate >= " & startDate)
    '            Catch ex As FaultException
    '                Throw
    '            Catch ex As Exception
    '                Utilities.SaveAppError(ex.Message, Me.Parameters)
    '                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
    '            End Try
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

#End Region

#Region "tblFormList"

    Public Function GettblFormLists() As DTO.tblFormList()

        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'if there are no null values to deal with we can build the array directly
                Dim tblFormLists() As DTO.tblFormList = (
                    From t In db.tblFormLists
                    Order By t.FormName
                    Select New DTO.tblFormList With {.FormControl = t.FormControl, .FormName = t.FormName, .FormDescription = t.FormDescription, .FormFormMenuControl = t.FormFormMenuControl, .FormMenuSequence = t.FormMenuSequence, .FormUpdated = t.FormUpdated.ToArray()}).ToArray()
                Return tblFormLists
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

    Public Function GettblFormList(ByVal Control As Integer) As DTO.tblFormList

        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'if there are no null values to deal with we can build the array directly
                Dim tblFormList As DTO.tblFormList = (
                From t In db.tblFormLists
                Where t.FormControl = Control
                Select New DTO.tblFormList With {.FormControl = t.FormControl, .FormName = t.FormName, .FormDescription = t.FormDescription, .FormFormMenuControl = t.FormFormMenuControl, .FormMenuSequence = t.FormMenuSequence, .FormUpdated = t.FormUpdated.ToArray()}).Single
                Return tblFormList

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

    Public Function GettblFormListByName(ByVal Name As Integer) As DTO.tblFormList

        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'if there are no null values to deal with we can build the array directly
                Dim tblFormList As DTO.tblFormList = (
                From t In db.tblFormLists
                Where t.FormName = Name
                Select New DTO.tblFormList With {.FormControl = t.FormControl, .FormName = t.FormName, .FormDescription = t.FormDescription, .FormFormMenuControl = t.FormFormMenuControl, .FormMenuSequence = t.FormMenuSequence, .FormUpdated = t.FormUpdated.ToArray()}).FirstOrDefault
                Return tblFormList

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

    Public Function CreatetblFormList(ByVal oData As DTO.tblFormList) As DTO.tblFormList

        Using db As New NGLMASSecurityDataContext(ConnectionString)


            'Create New Record
            Dim ntblFormList As LTS.tblFormList = New LTS.tblFormList With {.FormControl = oData.FormControl, .FormName = oData.FormName, .FormDescription = oData.FormDescription, .FormFormMenuControl = oData.FormFormMenuControl, .FormMenuSequence = oData.FormMenuSequence, .FormUpdated = oData.FormUpdated}

            db.tblFormLists.InsertOnSubmit(ntblFormList)
            Try
                db.SubmitChanges()

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

            Return GettblFormList(ntblFormList.FormControl)

        End Using

    End Function

    Public Sub DeletetblFormList(ByVal oData As DTO.tblFormList)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            ' Delete the record
            Dim ntblFormList As LTS.tblFormList = New LTS.tblFormList With {.FormControl = oData.FormControl, .FormName = oData.FormName, .FormDescription = oData.FormDescription, .FormFormMenuControl = oData.FormFormMenuControl, .FormMenuSequence = oData.FormMenuSequence, .FormUpdated = oData.FormUpdated}

            db.tblFormLists.Attach(ntblFormList, True)
            db.tblFormLists.DeleteOnSubmit(ntblFormList)

            Try
                db.SubmitChanges()
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch conflictEx As ChangeConflictException
                Try
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
        End Using
    End Sub

    Public Function UpdatetblFormList(ByVal oData As DTO.tblFormList) As DTO.tblFormList
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            ' Attach the order
            Dim ntblFormList As LTS.tblFormList = New LTS.tblFormList With {.FormControl = oData.FormControl, .FormName = oData.FormName, .FormDescription = oData.FormDescription, .FormFormMenuControl = oData.FormFormMenuControl, .FormMenuSequence = oData.FormMenuSequence, .FormUpdated = oData.FormUpdated}
            db.tblFormLists.Attach(ntblFormList, True)

            Try
                db.SubmitChanges()
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch conflictEx As ChangeConflictException
                Try
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

            ' Return the updated order
            Return GettblFormList(ntblFormList.FormControl)
        End Using
    End Function

    Public Sub AddFormIfMissing(ByVal FormControl As Integer, ByVal FormName As String, ByVal FormDescription As String)
        'we do not need the return value
        getScalarString("Exec dbo.spAddFormIfMissing " & FormControl & ",'" & FormName & "','" & FormDescription & "'")
    End Sub

#End Region

#Region "tblProcedureList"

    Public Function GettblProcedureLists() As DTO.tblProcedureList()

        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'if there are no null values to deal with we can build the array directly
                Dim tblProcedureLists() As DTO.tblProcedureList = (
                    From t In db.tblProcedureLists
                    Order By t.ProcedureName
                    Select New DTO.tblProcedureList With {.ProcedureControl = t.ProcedureControl, .ProcedureName = t.ProcedureName, .ProcedureDescription = t.ProcedureDescription, .ProcedureUpdated = t.ProcedureUpdated.ToArray()}).ToArray()
                Return tblProcedureLists
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

    Public Function GettblProcedureList(ByVal Control As Integer) As DTO.tblProcedureList

        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'if there are no null values to deal with we can build the array directly
                Dim tblProcedureList As DTO.tblProcedureList = (
                From t In db.tblProcedureLists
                Where t.ProcedureControl = Control
                Select New DTO.tblProcedureList With {.ProcedureControl = t.ProcedureControl, .ProcedureName = t.ProcedureName, .ProcedureDescription = t.ProcedureDescription, .ProcedureUpdated = t.ProcedureUpdated.ToArray()}).Single
                Return tblProcedureList

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

    Public Function CreatetblProcedureList(ByVal oData As DTO.tblProcedureList) As DTO.tblProcedureList

        Using db As New NGLMASSecurityDataContext(ConnectionString)


            'Create New Record
            Dim ntblProcedureList As LTS.tblProcedureList = New LTS.tblProcedureList With {.ProcedureControl = oData.ProcedureControl, .ProcedureName = oData.ProcedureName, .ProcedureDescription = oData.ProcedureDescription, .ProcedureUpdated = oData.ProcedureUpdated}

            db.tblProcedureLists.InsertOnSubmit(ntblProcedureList)
            Try
                db.SubmitChanges()

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

            Return GettblProcedureList(ntblProcedureList.ProcedureControl)

        End Using

    End Function

    Public Sub DeletetblProcedureList(ByVal oData As DTO.tblProcedureList)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            ' Delete the record
            Dim ntblProcedureList As LTS.tblProcedureList = New LTS.tblProcedureList With {.ProcedureControl = oData.ProcedureControl, .ProcedureName = oData.ProcedureName, .ProcedureDescription = oData.ProcedureDescription, .ProcedureUpdated = oData.ProcedureUpdated}

            db.tblProcedureLists.Attach(ntblProcedureList, True)
            db.tblProcedureLists.DeleteOnSubmit(ntblProcedureList)

            Try
                db.SubmitChanges()
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch conflictEx As ChangeConflictException
                Try
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
        End Using
    End Sub

    Public Function UpdatetblProcedureList(ByVal oData As DTO.tblProcedureList) As DTO.tblProcedureList
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            ' Attach the order
            Dim ntblProcedureList As LTS.tblProcedureList = New LTS.tblProcedureList With {.ProcedureControl = oData.ProcedureControl, .ProcedureName = oData.ProcedureName, .ProcedureDescription = oData.ProcedureDescription, .ProcedureUpdated = oData.ProcedureUpdated}
            db.tblProcedureLists.Attach(ntblProcedureList, True)

            Try
                db.SubmitChanges()
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch conflictEx As ChangeConflictException
                Try
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

            ' Return the updated order
            Return GettblProcedureList(ntblProcedureList.ProcedureControl)
        End Using
    End Function

    ''' <summary>
    ''' checks if the current user (identified in the object's parameters settings) has permission to run the specific procedure.
    ''' If ProcedureControl is zero the procedure will attempt to validate using the ProcedureName.
    ''' If the procedure has not been configured in the security table it will be added if 
    ''' procedurename is provided.  New procedures are not restricted until set up in the Role Center.
    ''' </summary>
    ''' <param name="ProcedureControl"></param>
    ''' <param name="ProcedureName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CanUserRunProcedure(ByVal ProcedureControl As Integer, ByVal ProcedureName As String) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim result = db.spNetCheckProcedureSecurityWCF(ProcedureControl, ProcedureName, Me.Parameters.UserName).FirstOrDefault()
                If Not result Is Nothing Then
                    blnRet = If(result.Column1, True)
                End If

            Catch ex As Exception
                'do nothing
            End Try
            Return blnRet

        End Using
    End Function
#End Region

#Region "tblReportList"

    Public Function GettblReportLists() As DTO.tblReportList()

        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'Dim oDLO As New DataLoadOptions
                'oDLO.LoadWith(Of LTS.tblReportList)(Function(t) t.tblReportMenu)
                'db.LoadOptions = oDLO
                'db.Log = New DebugTextWriter
                'if there are no null values to deal with we can build the array directly
                Dim tblReportLists() As DTO.tblReportList = (
                    From t In db.tblReportLists
                    Order By t.ReportName
                    Select New DTO.tblReportList With {.ReportControl = t.ReportControl, .ReportName = t.ReportName, .ReportDescription = t.ReportDescription, .ReportServerURL = t.ReportServerURL, .ReportURL = t.ReportURL, .ReportPrinterName = t.ReportPrinterName, .ReportDataSource = t.ReportDataSource, .AMSActive = t.AMSActive, .ReportReportMenuControl = t.ReportReportMenuControl, .ReportMenuSequence = t.ReportMenuSequence, .ReportNEXTrackActive = t.ReportNEXTrackActive, .ReportShowInTree = t.ReportShowInTree, .ReportUpdated = t.ReportUpdated.ToArray()}).ToArray()
                Return tblReportLists
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

    Public Function GettblReportList(ByVal Control As Integer) As DTO.tblReportList

        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim oDLO As New DataLoadOptions
                oDLO.LoadWith(Of LTS.tblReportList)(Function(t As LTS.tblReportList) t.tblReportPars)
                db.LoadOptions = oDLO
                'if there are no null values to deal with we can build the array directly
                Dim tblReportList As DTO.tblReportList = (
                From t In db.tblReportLists
                Where t.ReportControl = Control
                Select New DTO.tblReportList With {.ReportControl = t.ReportControl,
                                                   .ReportName = t.ReportName,
                                                   .ReportDescription = t.ReportDescription,
                                                   .ReportServerURL = t.ReportServerURL,
                                                   .ReportURL = t.ReportURL,
                                                   .ReportPrinterName = t.ReportPrinterName,
                                                   .ReportDataSource = t.ReportDataSource,
                                                   .AMSActive = t.AMSActive,
                                                   .ReportReportMenuControl = t.ReportReportMenuControl,
                                                   .ReportMenuSequence = t.ReportMenuSequence,
                                                   .ReportNEXTrackActive = t.ReportNEXTrackActive,
                                                   .ReportShowInTree = t.ReportShowInTree,
                                                   .ReportUpdated = t.ReportUpdated.ToArray(),
                                                   .tblReportPars = (
                                                    From d In t.tblReportPars
                                                    Select New DTO.tblReportPar With {.ReportParControl = d.ReportParControl,
                                                                                    .ReportParReportControl = d.ReportParReportControl,
                                                                                    .ReportParReportParTypeControl = d.ReportParReportParTypeControl,
                                                                                    .ReportParName = d.ReportParName,
                                                                                    .ReportParText = d.ReportParText,
                                                                                    .ReportParSource = d.ReportParSource,
                                                                                    .ReportParValueField = d.ReportParValueField,
                                                                                    .ReportParTextField = d.ReportParTextField,
                                                                                    .ReportParSortOrder = d.ReportParSortOrder,
                                                                                    .ReportParApplyUserName = d.ReportParApplyUserName,
                                                                                    .ReportParDefaultValue = d.ReportParDefaultValue,
                                                                                    .ReportParUpdated = d.ReportParUpdated.ToArray()}).ToList()}).Single()
                Return tblReportList

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

    Public Function CreatetblReportList(ByVal oData As DTO.tblReportList) As DTO.tblReportList

        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Dim oResult = db.spAddReportIfMissing(oData.ReportControl, oData.ReportName, oData.ReportDescription, oData.ReportServerURL, oData.ReportURL, oData.ReportPrinterName, oData.ReportDataSource, oData.AMSActive, oData.ReportReportMenuControl, oData.ReportMenuSequence, oData.ReportNEXTrackActive, oData.ReportShowInTree).FirstOrDefault()

            'Create New Record
            'Dim ntblReportList As LTS.tblReportList = New LTS.tblReportList With {.ReportControl = oData.ReportControl, _
            '                                                                      .ReportName = oData.ReportName, _
            '                                                                      .ReportDescription = oData.ReportDescription, _
            '                                                                      .ReportServerURL = oData.ReportServerURL, _
            '                                                                      .ReportURL = oData.ReportURL, _
            '                                                                      .ReportPrinterName = oData.ReportPrinterName, _
            '                                                                      .ReportDataSource = oData.ReportDataSource, _
            '                                                                      .AMSActive = oData.AMSActive, _
            '                                                                      .ReportReportMenuControl = oData.ReportReportMenuControl, _
            '                                                                      .ReportMenuSequence = oData.ReportMenuSequence, _
            '                                                                      .ReportNEXTrackActive = oData.ReportNEXTrackActive, _
            '                                                                      .ReportShowInTree = oData.ReportShowInTree, _
            '                                                                      .ReportUpdated = oData.ReportUpdated}

            Dim ntblReportList As LTS.tblReportList = db.tblReportLists.Where(Function(x) x.ReportControl = oData.ReportControl).FirstOrDefault()

            If ntblReportList Is Nothing OrElse ntblReportList.ReportControl = 0 Then Return Nothing

            'Add Detail Records
            ntblReportList.tblReportPars.AddRange(
                From d In oData.tblReportPars
                Select New LTS.tblReportPar With {.ReportParControl = d.ReportParControl,
                                                .ReportParReportControl = d.ReportParReportControl,
                                                .ReportParReportParTypeControl = d.ReportParReportParTypeControl,
                                                .ReportParName = d.ReportParName,
                                                .ReportParText = d.ReportParText,
                                                .ReportParSource = d.ReportParSource,
                                                .ReportParValueField = d.ReportParValueField,
                                                .ReportParTextField = d.ReportParTextField,
                                                .ReportParSortOrder = d.ReportParSortOrder,
                                                .ReportParApplyUserName = d.ReportParApplyUserName,
                                                .ReportParDefaultValue = d.ReportParDefaultValue,
                                                .ReportParUpdated = d.ReportParUpdated.ToArray()})

            'db.tblReportLists.InsertOnSubmit(ntblReportList)
            db.tblReportPars.InsertAllOnSubmit(ntblReportList.tblReportPars)

            Try
                db.SubmitChanges()

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
            Return GettblReportList(ntblReportList.ReportControl)

        End Using

    End Function

    Public Sub DeletetblReportList(ByVal oData As DTO.tblReportList)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            ' Delete the record
            Dim ntblReportList As LTS.tblReportList = New LTS.tblReportList With {.ReportControl = oData.ReportControl,
                                                                                  .ReportName = oData.ReportName,
                                                                                  .ReportDescription = oData.ReportDescription,
                                                                                  .ReportServerURL = oData.ReportServerURL,
                                                                                  .ReportURL = oData.ReportURL,
                                                                                  .ReportPrinterName = oData.ReportPrinterName,
                                                                                  .ReportDataSource = oData.ReportDataSource,
                                                                                  .AMSActive = oData.AMSActive,
                                                                                  .ReportReportMenuControl = oData.ReportReportMenuControl,
                                                                                  .ReportMenuSequence = oData.ReportMenuSequence,
                                                                                  .ReportNEXTrackActive = oData.ReportNEXTrackActive,
                                                                                  .ReportShowInTree = oData.ReportShowInTree,
                                                                                  .ReportUpdated = oData.ReportUpdated}

            db.tblReportLists.Attach(ntblReportList, True)
            db.tblReportLists.DeleteOnSubmit(ntblReportList)


            Try
                db.SubmitChanges()
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch conflictEx As ChangeConflictException
                Try
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
        End Using
    End Sub

    Public Function UpdatetblReportList(ByVal oData As DTO.tblReportList) As DTO.tblReportList
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            ' Attach the order
            Dim ntblReportList As LTS.tblReportList = New LTS.tblReportList With {.ReportControl = oData.ReportControl,
                                                                                  .ReportName = oData.ReportName,
                                                                                  .ReportDescription = oData.ReportDescription,
                                                                                  .ReportServerURL = oData.ReportServerURL,
                                                                                  .ReportURL = oData.ReportURL,
                                                                                  .ReportPrinterName = oData.ReportPrinterName,
                                                                                  .ReportDataSource = oData.ReportDataSource,
                                                                                  .AMSActive = oData.AMSActive,
                                                                                  .ReportReportMenuControl = oData.ReportReportMenuControl,
                                                                                  .ReportMenuSequence = oData.ReportMenuSequence,
                                                                                  .ReportNEXTrackActive = oData.ReportNEXTrackActive,
                                                                                  .ReportShowInTree = oData.ReportShowInTree,
                                                                                  .ReportUpdated = oData.ReportUpdated}
            db.tblReportLists.Attach(ntblReportList, True)
            ' Process any inserted detail records 
            Dim insertedDetails As List(Of LTS.tblReportPar) = GetReportParChanges(oData, TrackingInfo.Created)
            db.tblReportPars.InsertAllOnSubmit(insertedDetails)
            ' Process any updated detail records
            Dim updatedDetails As List(Of LTS.tblReportPar) = GetReportParChanges(oData, TrackingInfo.Updated)
            db.tblReportPars.AttachAll(updatedDetails, True)
            ' Process any deleted detail records
            Dim deletedDetails As List(Of LTS.tblReportPar) = GetReportParChanges(oData, TrackingInfo.Deleted)
            db.tblReportPars.AttachAll(deletedDetails, True)
            db.tblReportPars.DeleteAllOnSubmit(deletedDetails)

            Try
                db.SubmitChanges()
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch conflictEx As ChangeConflictException
                Try
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

            ' Return the updated order
            Return GettblReportList(ntblReportList.ReportControl)
        End Using
    End Function
#End Region

#Region "tblReportPar"

    Public Function GettblReportPars() As DTO.tblReportPar()

        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim oDLO As New DataLoadOptions
                oDLO.LoadWith(Of LTS.tblReportPar)(Function(t As LTS.tblReportPar) t.tblReportParType)
                oDLO.LoadWith(Of LTS.tblReportPar)(Function(t As LTS.tblReportPar) t.tblReportList)
                db.LoadOptions = oDLO
                'if there are no null values to deal with we can build the array directly
                Dim tblReportPars() As DTO.tblReportPar = (
                    From t In db.tblReportPars
                    Order By t.ReportParName
                    Select New DTO.tblReportPar With {.ReportParControl = t.ReportParControl, .ReportParReportControl = t.ReportParReportControl, .ReportParReportParTypeControl = t.ReportParReportParTypeControl, .ReportParName = t.ReportParName, .ReportParText = t.ReportParText, .ReportParSource = t.ReportParSource, .ReportParValueField = t.ReportParValueField, .ReportParTextField = t.ReportParTextField, .ReportParSortOrder = t.ReportParSortOrder, .ReportParApplyUserName = t.ReportParApplyUserName, .ReportParDefaultValue = t.ReportParDefaultValue, .ReportParReportName = t.tblReportList.ReportName, .ReportParReportParTypeName = t.tblReportParType.ReportParTypeName, .ReportParUpdated = t.ReportParUpdated.ToArray()}).ToArray()
                Return tblReportPars
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

    Public Function GettblReportPar(ByVal Control As Integer) As DTO.tblReportPar

        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim oDLO As New DataLoadOptions
                oDLO.LoadWith(Of LTS.tblReportPar)(Function(t As LTS.tblReportPar) t.tblReportParType)
                oDLO.LoadWith(Of LTS.tblReportPar)(Function(t As LTS.tblReportPar) t.tblReportList)
                db.LoadOptions = oDLO
                'if there are no null values to deal with we can build the array directly
                Dim tblReportPar As DTO.tblReportPar = (
                From t In db.tblReportPars
                Where t.ReportParControl = Control
                Select New DTO.tblReportPar With {.ReportParControl = t.ReportParControl, .ReportParReportControl = t.ReportParReportControl, .ReportParReportParTypeControl = t.ReportParReportParTypeControl, .ReportParName = t.ReportParName, .ReportParText = t.ReportParText, .ReportParSource = t.ReportParSource, .ReportParValueField = t.ReportParValueField, .ReportParTextField = t.ReportParTextField, .ReportParSortOrder = t.ReportParSortOrder, .ReportParApplyUserName = t.ReportParApplyUserName, .ReportParDefaultValue = t.ReportParDefaultValue, .ReportParReportName = t.tblReportList.ReportName, .ReportParReportParTypeName = t.tblReportParType.ReportParTypeName, .ReportParUpdated = t.ReportParUpdated.ToArray()}).Single
                Return tblReportPar

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

    Public Function CreatetblReportPar(ByVal oData As DTO.tblReportPar) As DTO.tblReportPar

        Using db As New NGLMASSecurityDataContext(ConnectionString)


            'Create New Record
            Dim ntblReportPar As LTS.tblReportPar = New LTS.tblReportPar With {.ReportParControl = oData.ReportParControl, .ReportParReportControl = oData.ReportParReportControl, .ReportParReportParTypeControl = oData.ReportParReportParTypeControl, .ReportParName = oData.ReportParName, .ReportParText = oData.ReportParText, .ReportParSource = oData.ReportParSource, .ReportParValueField = oData.ReportParValueField, .ReportParTextField = oData.ReportParTextField, .ReportParSortOrder = oData.ReportParSortOrder, .ReportParApplyUserName = oData.ReportParApplyUserName, .ReportParDefaultValue = oData.ReportParDefaultValue, .ReportParUpdated = oData.ReportParUpdated.ToArray()}

            db.tblReportPars.InsertOnSubmit(ntblReportPar)
            Try
                db.SubmitChanges()

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

            Return GettblReportPar(ntblReportPar.ReportParControl)

        End Using

    End Function

    Public Sub DeletetblReportPar(ByVal oData As DTO.tblReportPar)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            ' Delete the record
            Dim ntblReportPar As LTS.tblReportPar = New LTS.tblReportPar With {.ReportParControl = oData.ReportParControl, .ReportParReportControl = oData.ReportParReportControl, .ReportParReportParTypeControl = oData.ReportParReportParTypeControl, .ReportParName = oData.ReportParName, .ReportParText = oData.ReportParText, .ReportParSource = oData.ReportParSource, .ReportParValueField = oData.ReportParValueField, .ReportParTextField = oData.ReportParTextField, .ReportParSortOrder = oData.ReportParSortOrder, .ReportParApplyUserName = oData.ReportParApplyUserName, .ReportParDefaultValue = oData.ReportParDefaultValue, .ReportParUpdated = oData.ReportParUpdated.ToArray()}

            db.tblReportPars.Attach(ntblReportPar, True)
            db.tblReportPars.DeleteOnSubmit(ntblReportPar)

            Try
                db.SubmitChanges()
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch conflictEx As ChangeConflictException
                Try
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
        End Using
    End Sub

    Public Function UpdatetblReportPar(ByVal oData As DTO.tblReportPar) As DTO.tblReportPar
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            ' Attach the order
            Dim ntblReportPar As LTS.tblReportPar = New LTS.tblReportPar With {.ReportParControl = oData.ReportParControl, .ReportParReportControl = oData.ReportParReportControl, .ReportParReportParTypeControl = oData.ReportParReportParTypeControl, .ReportParName = oData.ReportParName, .ReportParText = oData.ReportParText, .ReportParSource = oData.ReportParSource, .ReportParValueField = oData.ReportParValueField, .ReportParTextField = oData.ReportParTextField, .ReportParSortOrder = oData.ReportParSortOrder, .ReportParApplyUserName = oData.ReportParApplyUserName, .ReportParDefaultValue = oData.ReportParDefaultValue, .ReportParUpdated = oData.ReportParUpdated.ToArray()}
            db.tblReportPars.Attach(ntblReportPar, True)

            Try
                db.SubmitChanges()
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch conflictEx As ChangeConflictException
                Try
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

            ' Return the updated order
            Return GettblReportPar(ntblReportPar.ReportParControl)
        End Using
    End Function
#End Region

#Region "tblReportParType"
    Public Function GettblReportParTypes() As DTO.tblReportParType()

        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'if there are no null values to deal with we can build the array directly
                Dim tblReportParTypes() As DTO.tblReportParType = (
                    From t In db.tblReportParTypes
                    Order By t.ReportParTypeName
                    Select New DTO.tblReportParType With {.ReportParTypeControl = t.ReportParTypeControl,
                                                          .ReportParTypeName = t.ReportParTypeName,
                                                          .ReportParTypeDesc = t.ReportParTypeDesc,
                                                          .ReportParTypeUpdated = t.ReportParTypeUpdated.ToArray()}).ToArray()
                Return tblReportParTypes
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

    Public Function GettblReportParType(ByVal Control As Integer) As DTO.tblReportParType

        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'if there are no null values to deal with we can build the array directly
                Dim tblReportParType As DTO.tblReportParType = (
                From t In db.tblReportParTypes
                Where t.ReportParTypeControl = Control
                Select New DTO.tblReportParType With {.ReportParTypeControl = t.ReportParTypeControl,
                                                      .ReportParTypeName = t.ReportParTypeName,
                                                      .ReportParTypeDesc = t.ReportParTypeDesc,
                                                      .ReportParTypeUpdated = t.ReportParTypeUpdated.ToArray()}).Single
                Return tblReportParType

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

    Public Function CreatetblReportParType(ByVal oData As DTO.tblReportParType) As DTO.tblReportParType

        Using db As New NGLMASSecurityDataContext(ConnectionString)


            'Create New Record
            Dim ntblReportParType As LTS.tblReportParType = New LTS.tblReportParType With {.ReportParTypeControl = oData.ReportParTypeControl,
                                                                                           .ReportParTypeName = oData.ReportParTypeName,
                                                                                           .ReportParTypeDesc = oData.ReportParTypeDesc,
                                                                                           .ReportParTypeUpdated = oData.ReportParTypeUpdated}

            db.tblReportParTypes.InsertOnSubmit(ntblReportParType)
            Try
                db.SubmitChanges()

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

            Return GettblReportParType(ntblReportParType.ReportParTypeControl)

        End Using

    End Function

    Public Sub DeletetblReportParType(ByVal oData As DTO.tblReportParType)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            ' Delete the record
            Dim ntblReportParType As LTS.tblReportParType = New LTS.tblReportParType With {.ReportParTypeControl = oData.ReportParTypeControl,
                                                                                           .ReportParTypeName = oData.ReportParTypeName,
                                                                                           .ReportParTypeDesc = oData.ReportParTypeDesc,
                                                                                           .ReportParTypeUpdated = oData.ReportParTypeUpdated}

            db.tblReportParTypes.Attach(ntblReportParType, True)
            db.tblReportParTypes.DeleteOnSubmit(ntblReportParType)

            Try
                db.SubmitChanges()
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch conflictEx As ChangeConflictException
                Try
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
        End Using
    End Sub

    Public Function UpdatetblReportParType(ByVal oData As DTO.tblReportParType) As DTO.tblReportParType
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            ' Attach the order
            Dim ntblReportParType As LTS.tblReportParType = New LTS.tblReportParType With {.ReportParTypeControl = oData.ReportParTypeControl,
                                                                                           .ReportParTypeName = oData.ReportParTypeName,
                                                                                           .ReportParTypeDesc = oData.ReportParTypeDesc,
                                                                                           .ReportParTypeUpdated = oData.ReportParTypeUpdated}
            db.tblReportParTypes.Attach(ntblReportParType, True)

            Try
                db.SubmitChanges()
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch conflictEx As ChangeConflictException
                Try
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

            ' Return the updated order
            Return GettblReportParType(ntblReportParType.ReportParTypeControl)
        End Using
    End Function
#End Region

#Region "tblFormMenu"
    Public Function GettblFormMenus() As DTO.tblFormMenu()

        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'if there are no null values to deal with we can build the array directly
                Dim tblFormMenus() As DTO.tblFormMenu = (
                    From t In db.tblFormMenus
                    Order By t.FormMenuName
                    Select New DTO.tblFormMenu With {.FormMenuControl = t.FormMenuControl, .FormMenuName = t.FormMenuName, .FormMenuDescription = t.FormMenuDescription, .FormMenuSequence = t.FormMenuSequence, .FormMenuUpdated = t.FormMenuUpdated.ToArray()}).ToArray()
                Return tblFormMenus
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

    Public Function GettblFormMenu(ByVal Control As Integer) As DTO.tblFormMenu

        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'if there are no null values to deal with we can build the array directly
                Dim tblFormMenu As DTO.tblFormMenu = (
                From t In db.tblFormMenus
                Where t.FormMenuControl = Control
                Select New DTO.tblFormMenu With {.FormMenuControl = t.FormMenuControl, .FormMenuName = t.FormMenuName, .FormMenuDescription = t.FormMenuDescription, .FormMenuSequence = t.FormMenuSequence, .FormMenuUpdated = t.FormMenuUpdated.ToArray()}).Single
                Return tblFormMenu

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

    Public Function CreatetblFormMenu(ByVal oData As DTO.tblFormMenu) As DTO.tblFormMenu

        Using db As New NGLMASSecurityDataContext(ConnectionString)


            'Create New Record
            Dim ntblFormMenu As LTS.tblFormMenu = New LTS.tblFormMenu With {.FormMenuControl = oData.FormMenuControl, .FormMenuName = oData.FormMenuName, .FormMenuDescription = oData.FormMenuDescription, .FormMenuSequence = oData.FormMenuSequence, .FormMenuUpdated = oData.FormMenuUpdated}

            db.tblFormMenus.InsertOnSubmit(ntblFormMenu)
            Try
                db.SubmitChanges()

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

            Return GettblFormMenu(ntblFormMenu.FormMenuControl)

        End Using

    End Function

    Public Sub DeletetblFormMenu(ByVal oData As DTO.tblFormMenu)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            ' Delete the record
            Dim ntblFormMenu As LTS.tblFormMenu = New LTS.tblFormMenu With {.FormMenuControl = oData.FormMenuControl, .FormMenuName = oData.FormMenuName, .FormMenuDescription = oData.FormMenuDescription, .FormMenuSequence = oData.FormMenuSequence, .FormMenuUpdated = oData.FormMenuUpdated}

            db.tblFormMenus.Attach(ntblFormMenu, True)
            db.tblFormMenus.DeleteOnSubmit(ntblFormMenu)

            Try
                db.SubmitChanges()
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch conflictEx As ChangeConflictException
                Try
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
        End Using
    End Sub

    Public Function UpdatetblFormMenu(ByVal oData As DTO.tblFormMenu) As DTO.tblFormMenu
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            ' Attach the order
            Dim ntblFormMenu As LTS.tblFormMenu = New LTS.tblFormMenu With {.FormMenuControl = oData.FormMenuControl, .FormMenuName = oData.FormMenuName, .FormMenuDescription = oData.FormMenuDescription, .FormMenuSequence = oData.FormMenuSequence, .FormMenuUpdated = oData.FormMenuUpdated}
            db.tblFormMenus.Attach(ntblFormMenu, True)

            Try
                db.SubmitChanges()
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch conflictEx As ChangeConflictException
                Try
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

            ' Return the updated order
            Return GettblFormMenu(ntblFormMenu.FormMenuControl)
        End Using
    End Function
#End Region

#Region "tblReportMenu"
    Public Function GettblReportMenus() As DTO.tblReportMenu()

        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'if there are no null values to deal with we can build the array directly
                Dim tblReportMenus() As DTO.tblReportMenu = (
                    From t In db.tblReportMenus
                    Order By t.ReportMenuName
                    Select New DTO.tblReportMenu With {.ReportMenuControl = t.ReportMenuControl, .ReportMenuName = t.ReportMenuName, .ReportMenuDescription = t.ReportMenuDescription, .ReportMenuSequence = t.ReportMenuSequence, .ReportMenuUpdated = t.ReportMenuUpdated.ToArray()}).ToArray()
                Return tblReportMenus
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

    Public Function GettblReportMenu(ByVal Control As Integer) As DTO.tblReportMenu

        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'if there are no null values to deal with we can build the array directly
                Dim tblReportMenu As DTO.tblReportMenu = (
                From t In db.tblReportMenus
                Where t.ReportMenuControl = Control
                Select New DTO.tblReportMenu With {.ReportMenuControl = t.ReportMenuControl, .ReportMenuName = t.ReportMenuName, .ReportMenuDescription = t.ReportMenuDescription, .ReportMenuSequence = t.ReportMenuSequence, .ReportMenuUpdated = t.ReportMenuUpdated.ToArray()}).Single
                Return tblReportMenu

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

    Public Function CreatetblReportMenu(ByVal oData As DTO.tblReportMenu) As DTO.tblReportMenu

        Using db As New NGLMASSecurityDataContext(ConnectionString)


            'Create New Record
            Dim ntblReportMenu As LTS.tblReportMenu = New LTS.tblReportMenu With {.ReportMenuControl = oData.ReportMenuControl, .ReportMenuName = oData.ReportMenuName, .ReportMenuDescription = oData.ReportMenuDescription, .ReportMenuSequence = oData.ReportMenuSequence, .ReportMenuUpdated = oData.ReportMenuUpdated}

            db.tblReportMenus.InsertOnSubmit(ntblReportMenu)
            Try
                db.SubmitChanges()

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

            Return GettblReportMenu(ntblReportMenu.ReportMenuControl)

        End Using

    End Function

    Public Sub DeletetblReportMenu(ByVal oData As DTO.tblReportMenu)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            ' Delete the record
            Dim ntblReportMenu As LTS.tblReportMenu = New LTS.tblReportMenu With {.ReportMenuControl = oData.ReportMenuControl, .ReportMenuName = oData.ReportMenuName, .ReportMenuDescription = oData.ReportMenuDescription, .ReportMenuSequence = oData.ReportMenuSequence, .ReportMenuUpdated = oData.ReportMenuUpdated}

            db.tblReportMenus.Attach(ntblReportMenu, True)
            db.tblReportMenus.DeleteOnSubmit(ntblReportMenu)

            Try
                db.SubmitChanges()
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch conflictEx As ChangeConflictException
                Try
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
        End Using
    End Sub

    Public Function UpdatetblReportMenu(ByVal oData As DTO.tblReportMenu) As DTO.tblReportMenu
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            ' Attach the order
            Dim ntblReportMenu As LTS.tblReportMenu = New LTS.tblReportMenu With {.ReportMenuControl = oData.ReportMenuControl, .ReportMenuName = oData.ReportMenuName, .ReportMenuDescription = oData.ReportMenuDescription, .ReportMenuSequence = oData.ReportMenuSequence, .ReportMenuUpdated = oData.ReportMenuUpdated}
            db.tblReportMenus.Attach(ntblReportMenu, True)

            Try
                db.SubmitChanges()
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch conflictEx As ChangeConflictException
                Try
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
            ' Return the updated order
            Return GettblReportMenu(ntblReportMenu.ReportMenuControl)
        End Using
    End Function
#End Region

#Region "Form Security By Group"

    ''' <summary>
    ''' In 365 this has been replaced by the call to NGLUserGroupData.GetRestrictedFormsByGroup() below
    ''' This is because in 365 we need to use the correct base class paging and filtering for grids in cm
    ''' Also LTS is faster and we created a new view
    ''' </summary>
    ''' <param name="GroupControl"></param>
    ''' <returns></returns>
    Public Function GetRestrictedFormsByGroup(ByVal GroupControl As Integer) As DTO.tblFormList()
        'Create a data connection 
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblFormLists As New List(Of DTO.tblFormList)
        Try
            Dim strSQL As String = "SELECT tblFormSecurityGroupXref.FormSecurityGroupXrefControl," _
            & " tblFormList.FormControl, tblFormList.FormName, tblFormList.FormDescription, " _
            & " tblFormList.FormFormMenuControl,tblFormList.FormMenuSequence " _
            & " FROM tblFormSecurityGroupXref INNER JOIN tblFormList ON " _
            & " tblFormSecurityGroupXref.FormControl = tblFormList.FormControl " _
            & " WHERE (tblFormSecurityGroupXref.UserGroupsControl = " _
            & GroupControl & ")" _
            & " Order By tblFormList.FormName"
            Dim oQRet As Ngl.Core.Data.QueryResult = oQuery.ExecuteWithFill(strSQL)
            If Not oQRet.Exception Is Nothing Then
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NotReadScrByGrp"}, New FaultReason("E_DataAccessError"))
            End If
            If Not oQRet.Data Is Nothing AndAlso oQRet.Data.Rows.Count > 0 Then
                For Each oRow As System.Data.DataRow In oQRet.Data.Rows
                    Dim oForm As New DTO.tblFormList
                    With oForm
                        .FormControl = DTran.getDataRowValue(oRow, "FormControl", 0)
                        .FormName = DTran.getDataRowString(oRow, "FormName", "Unnamed")
                        .FormDescription = DTran.getDataRowString(oRow, "FormDescription", "")
                        .FormFormMenuControl = DTran.getDataRowValue(oRow, "FormFormMenuControl", 0)
                        .FormMenuSequence = DTran.getDataRowValue(oRow, "FormMenuSequence", 0)
                        .FormSecurityGroupXrefControl = DTran.getDataRowValue(oRow, "FormSecurityGroupXrefControl", 0)
                    End With
                    tblFormLists.Add(oForm)
                Next
            Else
                tblFormLists.Add(New DTO.tblFormList)
            End If
            Return tblFormLists.ToArray()
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
        Return Nothing
    End Function

    'Used in 365 LEUserFormController/GetUnRestrictedFormsByGroup
    Public Function GetUnRestrictedFormsByGroup(ByVal GroupControl As Integer) As DTO.tblFormList()
        'Create a data connection 
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblFormLists As New List(Of DTO.tblFormList)
        Try
            Dim strSQL As String = " Select " _
                & " tblFormList.FormControl, tblFormList.FormName, tblFormList.FormDescription, " _
                & " tblFormList.FormFormMenuControl,tblFormList.FormMenuSequence " _
                & " FROM tblFormList " _
                & " WHERE (FormControl NOT IN " _
                & " (SELECT FormControl " _
                    & " FROM dbo.tblFormSecurityGroupXref " _
                    & " WHERE tblFormSecurityGroupXref.UserGroupsControl = " _
                    & GroupControl & "))" _
                & " Order By FormName"
            Dim oQRet As Ngl.Core.Data.QueryResult = oQuery.ExecuteWithFill(strSQL)
            If Not oQRet.Exception Is Nothing Then
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NotReadScrByGrp"}, New FaultReason("E_DataAccessError"))
            End If
            If Not oQRet.Data Is Nothing AndAlso oQRet.Data.Rows.Count > 0 Then
                For Each oRow As System.Data.DataRow In oQRet.Data.Rows
                    Dim oForm As New DTO.tblFormList
                    With oForm
                        .FormControl = DTran.getDataRowValue(oRow, "FormControl", 0)
                        .FormName = DTran.getDataRowString(oRow, "FormName", "Unnamed")
                        .FormDescription = DTran.getDataRowString(oRow, "FormDescription", "")
                        .FormFormMenuControl = DTran.getDataRowValue(oRow, "FormFormMenuControl", 0)
                        .FormMenuSequence = DTran.getDataRowValue(oRow, "FormMenuSequence", 0)
                    End With
                    tblFormLists.Add(oForm)
                Next
            Else
                tblFormLists.Add(New DTO.tblFormList)
            End If
            Return tblFormLists.ToArray()
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
        Return Nothing
    End Function

    'Used in 365 LEUserFormController/RemoveFormRestrictionFromGroup
    ''' <summary>
    ''' Unrestricts the Form for the Group
    ''' </summary>
    ''' <param name="FormControl"></param>
    ''' <param name="GroupControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by LVV 2/5/20 v-8.2.1.004
    '''  Added logic so associated procedure security is linked to page (Form) security 
    '''  so when Form security is modified the procedure security is modified to match
    ''' Modified by LVV on 2/6/20 
    '''  Commented out previous change. Wasn't supposed to implement this change until v-8.3 oops
    ''' Used in 365 LEUserFormController/RemoveFormRestrictionFromGroup
    ''' </remarks>
    Public Function RemoveRestrictedFormByGroup(ByVal FormControl As Integer, ByVal GroupControl As Integer) As DTO.tblFormList()
        'Create a data connection 
        Dim oCon As SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Core.Data.Query(ConnectionString)
        Dim tblFormLists As New List(Of DTO.tblFormList)
        Try
            'First remove  
            Dim strSQL As String = "DELETE FROM  tblFormSecurityGroupXref " _
            & " WHERE (tblFormSecurityGroupXref.FormControl = " _
            & FormControl & " AND tblFormSecurityGroupXref.UserGroupsControl = " & GroupControl & ")"

            If oQuery.executeSQLQuery(oCon, strSQL, 1) Then
                'update all the dependent users
                strSQL = "DELETE FROM tblFormSecurityXref " _
                    & " WHERE FormControl = " & FormControl _
                    & " AND UserSecurityControl in " _
                    & " (SELECT UserSecurityControl " _
                    & " FROM tblUserSecurity " _
                    & " WHERE UserUserGroupsControl = " & GroupControl & ")"
                oQuery.executeSQLQuery(oCon, strSQL, 1)
                'Modified by LVV on 2/6/20 - Commented out below change. Wasn't supposed to implement this change until v-8.3 oops
                ''UnrestrictProceduresAssociatedWithForm365(FormControl, GroupControl) 'Added by LVV 2/5/20 Gets all procedures associated with a page (Form) and unrestricts (deletes) them for the Group and all associated users
                Return GetRestrictedFormsByGroup(GroupControl)
            Else
                tblFormLists.Add(New DTO.tblFormList)
                Return tblFormLists.ToArray
            End If
        Catch ex As FaultException
            Throw
        Catch ex As Core.DatabaseRetryExceededException
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Core.DatabaseLogInException
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Core.DatabaseInvalidException
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As SqlException
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception
                'do nothing
            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception
                'do nothing
            End Try
        End Try
        Return Nothing
    End Function

    Public Sub DeletetblFormSecurityGroupXref(ByVal FormSecurityGroupXrefControl As Integer)
        'Create a data connection 
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)

        Try
            Dim strSQL As String = "DELETE FROM  tblFormSecurityGroupXref " _
            & " WHERE (tblFormSecurityGroupXref.FormSecurityGroupXrefControl = " _
            & FormSecurityGroupXrefControl & ")"

            oQuery.executeSQLQuery(oCon, strSQL, 1)

        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try
    End Sub

    ''' <summary>
    ''' There is an updated version of this method for 365 below called NGLUserGroupData.CreateRestrictedFormByGroup() 
    ''' That method should have better performance because it which calls a sp and only accesses the db one time
    ''' and does not waste resources returning DTO objects that are not needed by the caller in 365
    ''' </summary>
    ''' <param name="FormControl"></param>
    ''' <param name="GroupControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by LVV 2/5/20 v-8.2.1.004
    '''  Added logic so associated procedure security is linked to page (Form) security 
    '''  so when Form security is modified the procedure security is modified to match
    ''' Modified by LVV on 2/6/20 
    '''  Commented out previous change. Wasn't supposed to implement this change until v-8.3 oops
    ''' </remarks>
    Public Function CreateRestrictedFormByGroup(ByVal FormControl As Integer, ByVal GroupControl As Integer) As DTO.tblFormList()
        'Create a data connection 
        Dim oCon As SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Core.Data.Query(ConnectionString)
        Dim tblFormLists As New List(Of DTO.tblFormList)
        Try
            Dim strSQL As String = "insert into  dbo.tblFormSecurityGroupXref " _
                & "(UserGroupsControl ,FormControl)" _
                & " values " _
                & " (" & GroupControl & "," & FormControl & " )"

            If oQuery.executeSQLQuery(oCon, strSQL, 1) Then
                'update all the dependent users
                strSQL = "insert into  dbo.tblFormSecurityXref  (UserSecurityControl, FormControl) " _
                    & " Select UserSecurityControl,FormControl " _
                    & " FROM dbo.tblUserSecurity as a " _
                    & " Inner Join dbo.tblFormSecurityGroupXref as b " _
                    & " on a.UserUserGroupsControl = b.UserGroupsControl " _
                    & " Where a.UserUserGroupsControl = " & GroupControl _
                    & " AND b.FormControl = " & FormControl _
                    & " AND b.FormControl not in " _
                    & " (Select c.FormControl " _
                    & " FROM dbo.tblFormSecurityXref as c " _
                    & " Where c.UserSecurityControl = a.UserSecurityControl)"
                oQuery.executeSQLQuery(oCon, strSQL, 1)
                'Modified by LVV on 2/6/20 - Commented out below change. Wasn't supposed to implement this change until v-8.3 oops
                ''RestrictProceduresAssociatedWithForm(FormControl, GroupControl) 'Added by LVV 2/5/20 v-8.2.1.004 Gets all procedures associated with a page (Form) and restricts them for the Group and all associated users
                Return GetRestrictedFormsByGroup(GroupControl)
            Else
                tblFormLists.Add(New DTO.tblFormList)
                Return tblFormLists.ToArray
            End If
        Catch ex As FaultException
            Throw
        Catch ex As Core.DatabaseRetryExceededException
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Core.DatabaseLogInException
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Core.DatabaseInvalidException
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As SqlException
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception
                'do nothing
            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception
                'do nothing
            End Try
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Restricts All Forms For The Group
    ''' </summary>
    ''' <param name="GroupControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by LVV 2/5/20 v-8.2.1.004
    '''  Added logic so associated procedure security is linked to page (Form) security 
    '''  so when Form security is modified the procedure security is modified to match
    ''' Modified by LVV on 2/6/20 
    '''  Commented out previous change. Wasn't supposed to implement this change until v-8.3 oops
    ''' </remarks>
    Public Function RestrictAllFormsByGroup(ByVal GroupControl As Integer) As DTO.tblFormList()
        'Create a data connection 
        Dim oCon As SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblFormLists As New List(Of DTO.tblFormList)
        Try
            Dim strSQL As String = "insert into  dbo.tblFormSecurityGroupXref  (UserGroupsControl, FormControl) " _
               & " Select " & GroupControl & ",FormControl " _
               & " FROM dbo.tblFormList " _
               & " Where FormControl not in " _
               & " (Select FormControl FROM dbo.tblFormSecurityGroupXref Where UserGroupsControl = " & GroupControl & ")"

            If oQuery.executeSQLQuery(oCon, strSQL, 1) Then
                'update all the dependent users
                strSQL = "insert into  dbo.tblFormSecurityXref  (UserSecurityControl, FormControl) " _
                    & " Select UserSecurityControl,FormControl " _
                    & " FROM dbo.tblUserSecurity as a " _
                    & " Inner Join dbo.tblFormSecurityGroupXref as b " _
                    & " on a.UserUserGroupsControl = b.UserGroupsControl " _
                    & " Where a.UserUserGroupsControl = " & GroupControl _
                    & " AND b.FormControl not in " _
                    & " (Select c.FormControl " _
                    & " FROM dbo.tblFormSecurityXref as c " _
                    & " Where c.UserSecurityControl = a.UserSecurityControl)"
                oQuery.executeSQLQuery(oCon, strSQL, 1)
                'Modified by LVV on 2/6/20 - Commented out below change. Wasn't supposed to implement this change until v-8.3 oops
                ''RestrictProceduresAssociatedWithAllForms365(GroupControl) 'Added by LVV 2/5/20 Gets all procedures associated with all pages (Forms) and restricts them for the Group and all associated users
                Return GetRestrictedFormsByGroup(GroupControl)
            Else
                tblFormLists.Add(New DTO.tblFormList)
                Return tblFormLists.ToArray
            End If
        Catch ex As FaultException
            Throw
        Catch ex As Core.DatabaseRetryExceededException
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Core.DatabaseLogInException
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Core.DatabaseInvalidException
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As SqlException
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception
                'do nothing
            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception
                'do nothing
            End Try
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Unrestricts All Forms For The Group
    ''' </summary>
    ''' <param name="GroupControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by LVV 2/5/20 v-8.2.1.004
    '''  Added logic so associated procedure security is linked to page (Form) security 
    '''  so when Form security is modified the procedure security is modified to match
    '''  Modified by LVV on 2/6/20
    '''   Commented out previous change. Wasn't supposed to implement this change until v-8.3 oops
    ''' </remarks>
    Public Function UnRestrictAllFormsByGroup(ByVal GroupControl As Integer) As DTO.tblFormList()
        'Create a data connection 
        Dim oCon As SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Core.Data.Query(ConnectionString)
        Dim tblFormLists As New List(Of DTO.tblFormList)
        Try
            Dim strSQL As String = "Delete from dbo.tblFormSecurityGroupXref where UserGroupsControl = " & GroupControl

            If oQuery.executeSQLQuery(oCon, strSQL, 1) Then
                'remove any dependent records
                strSQL = "DELETE FROM dbo.tblFormSecurityXref " _
                    & " WHERE UserSecurityControl IN " _
                    & " (SELECT UserSecurityControl FROM dbo.tblUserSecurity Where UserUserGroupsControl = " & GroupControl _
                    & " ) AND FormControl IN " _
                    & " (SELECT FormControl FROM dbo.tblFormSecurityGroupXref WHERE UserGroupsControl = " & GroupControl & ")"
                oQuery.executeSQLQuery(oCon, strSQL, 1)
                'Modified by LVV on 2/6/20 - Commented out below change. Wasn't supposed to implement this change until v-8.3 oops
                ''UnrestrictProceduresAssociatedWithAllForms365(GroupControl) 'Added by LVV 2/5/20 Gets all procedures associated with all pages (Forms) and unrestricts (deletes) them for the Group and all associated users
                Return GetUnRestrictedFormsByGroup(GroupControl)
            Else
                tblFormLists.Add(New DTO.tblFormList)
                Return tblFormLists.ToArray
            End If
        Catch ex As FaultException
            Throw
        Catch ex As Core.DatabaseRetryExceededException
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Core.DatabaseLogInException
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Core.DatabaseInvalidException
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As SqlException
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception
                'do nothing
            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception
                'do nothing
            End Try
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Gets all procedures associated with a page (Form) and restricts them for the Group and all associated users.
    ''' Does Not include any procedures that are shared across multiple buttons/pages (Refresh, ResetUserPageSettings, etc)
    ''' </summary>
    ''' <param name="FormControl"></param>
    ''' <param name="GroupControl"></param>
    ''' <remarks>
    ''' Created by LVV 2/5/20 v-8.2.1.004
    ''' </remarks>
    Public Sub RestrictProceduresAssociatedWithForm(ByVal FormControl As Integer, ByVal GroupControl As Integer)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                db.spRestrictProceduresAssociatedWithForm365(FormControl, GroupControl)
            Catch ex As SqlException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Gets all procedures associated with all pages (Forms) and restricts them for the Group and all associated users.
    ''' Does Not include any procedures that are shared across multiple buttons/pages (Refresh, ResetUserPageSettings, etc)
    ''' </summary>
    ''' <param name="GroupControl"></param>
    ''' <remarks>
    ''' Created by LVV 2/5/20 v-8.2.1.004
    ''' </remarks>
    Public Sub RestrictProceduresAssociatedWithAllForms365(ByVal GroupControl As Integer)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                db.spRestrictProceduresAssociatedWithAllForms365(GroupControl)
            Catch ex As SqlException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Gets all procedures associated with a page (Form) and unrestricts (deletes) them for the Group and all associated users.
    ''' Does Not include any procedures that are shared across multiple buttons/pages (Refresh, ResetUserPageSettings, etc)
    ''' </summary>
    ''' <param name="FormControl"></param>
    ''' <param name="GroupControl"></param>
    ''' <remarks>
    ''' Created by LVV 2/5/20 v-8.2.1.004
    ''' </remarks>
    Public Sub UnrestrictProceduresAssociatedWithForm365(ByVal FormControl As Integer, ByVal GroupControl As Integer)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                db.spUnrestrictProceduresAssociatedWithForm365(FormControl, GroupControl)
            Catch ex As SqlException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Gets all procedures associated with all pages (Forms) and unrestricts (deletes) them for the Group and all associated users.
    ''' Does Not include any procedures that are shared across multiple buttons/pages (Refresh, ResetUserPageSettings, etc)
    ''' </summary>
    ''' <param name="GroupControl"></param>
    ''' <remarks>
    ''' Created by LVV 2/5/20 v-8.2.1.004
    ''' </remarks>
    Public Sub UnrestrictProceduresAssociatedWithAllForms365(ByVal GroupControl As Integer)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                db.spUnrestrictProceduresAssociatedWithAllForms365(GroupControl)
            Catch ex As SqlException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
        End Using
    End Sub

#End Region

#Region "Form Security By User"

    Public Function GetRestrictedFormsByUser(ByVal UserSecurityControl As Integer) As DTO.tblFormList()

        'Create a data connection 
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblFormLists As New List(Of DTO.tblFormList)

        Try
            Dim strSQL As String = "SELECT tblFormSecurityXref.FormSecurityXrefControl," _
            & " tblFormList.FormControl, tblFormList.FormName, tblFormList.FormDescription, " _
            & " tblFormList.FormFormMenuControl,tblFormList.FormMenuSequence," _
            & " Case ISNULL(" _
            & "     (Select Top 1 " _
            & "     dbo.tblFormSecurityGroupXref.FormSecurityGroupXrefControl " _
            & "     From " _
            & "         dbo.tblFormSecurityGroupXref" _
            & "     Where" _
            & "         dbo.tblFormSecurityGroupXref.UserGroupsControl = dbo.tblUserSecurity.UserUserGroupsControl" _
            & "         AND" _
            & "         dbo.tblFormSecurityGroupXref.FormControl = dbo.tblFormSecurityXref.FormControl" _
            & "     ) ,0)" _
            & "     When 0 then 1" _
            & "     Else 0" _
            & " End as Override " _
            & " FROM " _
            & "     dbo.tblFormSecurityXref" _
            & "     INNER JOIN dbo.tblFormList " _
            & "         ON dbo.tblFormSecurityXref.FormControl = dbo.tblFormList.FormControl" _
            & "     INNER JOIN  dbo.tblUserSecurity" _
            & "         ON dbo.tblFormSecurityXref.UserSecurityControl = dbo.tblUserSecurity.UserSecurityControl" _
            & " WHERE (dbo.tblFormSecurityXref.UserSecurityControl = " _
            & UserSecurityControl & ")" _
            & " Order By dbo.tblFormList.FormName"
            Dim oQRet As Ngl.Core.Data.QueryResult = oQuery.ExecuteWithFill(strSQL)
            If Not oQRet.Exception Is Nothing Then
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NotReadScrByUser"}, New FaultReason("E_DataAccessError"))
            End If
            If Not oQRet.Data Is Nothing AndAlso oQRet.Data.Rows.Count > 0 Then
                For Each oRow As System.Data.DataRow In oQRet.Data.Rows
                    Dim oForm As New DTO.tblFormList
                    With oForm
                        .FormControl = DTran.getDataRowValue(oRow, "FormControl", 0)
                        .FormName = DTran.getDataRowString(oRow, "FormName", "Unnamed")
                        .FormDescription = DTran.getDataRowString(oRow, "FormDescription", "")
                        .FormFormMenuControl = DTran.getDataRowValue(oRow, "FormFormMenuControl", 0)
                        .FormMenuSequence = DTran.getDataRowValue(oRow, "FormMenuSequence", 0)
                        .FormSecurityXrefControl = DTran.getDataRowValue(oRow, "FormSecurityXrefControl", 0)
                        .FormUserOverrideGroup = If(DTran.getDataRowValue(oRow, "Override", 0) = 0, False, True)
                    End With
                    tblFormLists.Add(oForm)
                Next
            Else
                tblFormLists.Add(New DTO.tblFormList)
            End If
            Return tblFormLists.ToArray()
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
        Return Nothing


    End Function

    Public Function GetRestrictedFormsByUserName(ByVal UserName As String) As DTO.tblFormList()

        'Create a data connection 
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblFormLists As New List(Of DTO.tblFormList)

        Try
            Dim strSQL As String = "SELECT tblFormSecurityXref.FormSecurityXrefControl," _
            & " tblFormList.FormControl, tblFormList.FormName, tblFormList.FormDescription, " _
            & " tblFormList.FormFormMenuControl,tblFormList.FormMenuSequence," _
            & " Case ISNULL(" _
            & "     (Select Top 1 " _
            & "     dbo.tblFormSecurityGroupXref.FormSecurityGroupXrefControl " _
            & "     From " _
            & "         dbo.tblFormSecurityGroupXref" _
            & "     Where" _
            & "         dbo.tblFormSecurityGroupXref.UserGroupsControl = dbo.tblUserSecurity.UserUserGroupsControl" _
            & "         AND" _
            & "         dbo.tblFormSecurityGroupXref.FormControl = dbo.tblFormSecurityXref.FormControl" _
            & "     ) ,0)" _
            & "     When 0 then 1" _
            & "     Else 0" _
            & " End as Override " _
            & " FROM " _
            & "     dbo.tblFormSecurityXref" _
            & "     INNER JOIN dbo.tblFormList " _
            & "         ON dbo.tblFormSecurityXref.FormControl = dbo.tblFormList.FormControl" _
            & "     INNER JOIN  dbo.tblUserSecurity" _
            & "         ON dbo.tblFormSecurityXref.UserSecurityControl = dbo.tblUserSecurity.UserSecurityControl" _
            & " WHERE (dbo.tblFormSecurityXref.UserSecurityControl = " _
            & " (select top 1 u.UserSecurityControl  from dbo.tblUserSecurity as u where u.UserName = '" & UserName & "'))" _
            & " Order By dbo.tblFormList.FormName"
            Dim oQRet As Ngl.Core.Data.QueryResult = oQuery.ExecuteWithFill(strSQL)
            If Not oQRet.Exception Is Nothing Then
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NotReadScrByUser"}, New FaultReason("E_DataAccessError"))
            End If
            If Not oQRet.Data Is Nothing AndAlso oQRet.Data.Rows.Count > 0 Then
                For Each oRow As System.Data.DataRow In oQRet.Data.Rows
                    Dim oForm As New DTO.tblFormList
                    With oForm
                        .FormControl = DTran.getDataRowValue(oRow, "FormControl", 0)
                        .FormName = DTran.getDataRowString(oRow, "FormName", "Unnamed")
                        .FormDescription = DTran.getDataRowString(oRow, "FormDescription", "")
                        .FormFormMenuControl = DTran.getDataRowValue(oRow, "FormFormMenuControl", 0)
                        .FormMenuSequence = DTran.getDataRowValue(oRow, "FormMenuSequence", 0)
                        .FormSecurityXrefControl = DTran.getDataRowValue(oRow, "FormSecurityXrefControl", 0)
                        .FormUserOverrideGroup = If(DTran.getDataRowValue(oRow, "Override", 0) = 0, False, True)
                    End With
                    tblFormLists.Add(oForm)
                Next
            Else
                tblFormLists.Add(New DTO.tblFormList)
            End If
            Return tblFormLists.ToArray()
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
        Return Nothing


    End Function

    Public Function GetUnRestrictedFormsByUserName(ByVal UserName As String, ByVal FromFormControl As Integer, ByVal ToFormControl As Integer) As DTO.tblFormList()

        'Create a data connection 
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblFormLists As New List(Of DTO.tblFormList)

        Try
            Dim oUser As DTO.tblUserSecurity = GettblUserSecurityByUserName(UserName)
            Dim strSQL As String = ""
            If oUser.UserUserGroupsControl > 0 Then
                strSQL = " Select " _
                & " tblFormList.FormControl, tblFormList.FormName, tblFormList.FormDescription, " _
                & " tblFormList.FormFormMenuControl,tblFormList.FormMenuSequence " _
                & " FROM tblFormList " _
                & " WHERE (FormControl NOT IN " _
                & " (SELECT FormControl " _
                    & " FROM dbo.tblFormSecurityGroupXref " _
                    & " WHERE tblFormSecurityGroupXref.UserGroupsControl = " _
                    & oUser.UserUserGroupsControl & "))" _
                    & " AND FormControl Between " & FromFormControl & " AND " & ToFormControl _
                & " Order By FormName"
            Else
                strSQL = " Select " _
                & " tblFormList.FormControl, tblFormList.FormName, tblFormList.FormDescription, " _
                & " tblFormList.FormFormMenuControl,tblFormList.FormMenuSequence " _
                & " FROM tblFormList " _
                & " WHERE (FormControl NOT IN " _
                & " (SELECT FormControl " _
                    & " FROM dbo.tblFormSecurityXref " _
                    & " WHERE tblFormSecurityXref.UserSecurityControl = " _
                    & oUser.UserSecurityControl & "))" _
                    & " AND FormControl Between " & FromFormControl & " AND " & ToFormControl _
                & " Order By FormName"
            End If

            Dim oQRet As Ngl.Core.Data.QueryResult = oQuery.ExecuteWithFill(strSQL)
            If Not oQRet.Exception Is Nothing Then
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NotReadScrByUser"}, New FaultReason("E_DataAccessError"))
            End If
            If Not oQRet.Data Is Nothing AndAlso oQRet.Data.Rows.Count > 0 Then
                For Each oRow As System.Data.DataRow In oQRet.Data.Rows
                    Dim oForm As New DTO.tblFormList
                    With oForm
                        .FormControl = DTran.getDataRowValue(oRow, "FormControl", 0)
                        .FormName = DTran.getDataRowString(oRow, "FormName", "Unnamed")
                        .FormDescription = DTran.getDataRowString(oRow, "FormDescription", "")
                        .FormFormMenuControl = DTran.getDataRowValue(oRow, "FormFormMenuControl", 0)
                        .FormMenuSequence = DTran.getDataRowValue(oRow, "FormMenuSequence", 0)
                    End With
                    tblFormLists.Add(oForm)
                Next
            Else
                tblFormLists.Add(New DTO.tblFormList)
            End If
            Return tblFormLists.ToArray()
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
        Return Nothing


    End Function

    Public Function GetUnRestrictedFormsByUser(ByVal UserSecurityControl As Integer) As DTO.tblFormList()

        'Create a data connection 
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblFormLists As New List(Of DTO.tblFormList)

        Try
            Dim strSQL As String = " Select " _
                & " tblFormList.FormControl, tblFormList.FormName, tblFormList.FormDescription, " _
                & " tblFormList.FormFormMenuControl,tblFormList.FormMenuSequence " _
                & " FROM tblFormList " _
                & " WHERE (FormControl NOT IN " _
                & " (SELECT FormControl " _
                    & " FROM dbo.tblFormSecurityXref " _
                    & " WHERE tblFormSecurityXref.UserSecurityControl = " _
                    & UserSecurityControl & "))" _
                & " Order By FormName"
            Dim oQRet As Ngl.Core.Data.QueryResult = oQuery.ExecuteWithFill(strSQL)
            If Not oQRet.Exception Is Nothing Then
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NotReadScrByUser"}, New FaultReason("E_DataAccessError"))
            End If
            If Not oQRet.Data Is Nothing AndAlso oQRet.Data.Rows.Count > 0 Then
                For Each oRow As System.Data.DataRow In oQRet.Data.Rows
                    Dim oForm As New DTO.tblFormList
                    With oForm
                        .FormControl = DTran.getDataRowValue(oRow, "FormControl", 0)
                        .FormName = DTran.getDataRowString(oRow, "FormName", "Unnamed")
                        .FormDescription = DTran.getDataRowString(oRow, "FormDescription", "")
                        .FormFormMenuControl = DTran.getDataRowValue(oRow, "FormFormMenuControl", 0)
                        .FormMenuSequence = DTran.getDataRowValue(oRow, "FormMenuSequence", 0)
                    End With
                    tblFormLists.Add(oForm)
                Next
            Else
                tblFormLists.Add(New DTO.tblFormList)
            End If
            Return tblFormLists.ToArray()
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

        Return Nothing


    End Function

    Public Function RemoveRestrictedFormByUser(ByVal FormControl As Integer, ByVal UserSecurityControl As Integer) As DTO.tblFormList()
        'Create a data connection 
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblFormLists As New List(Of DTO.tblFormList)

        Try
            Dim strSQL As String = "DELETE FROM  tblFormSecurityXref " _
            & " WHERE (tblFormSecurityXref.FormControl = " _
            & FormControl & " AND tblFormSecurityXref.UserSecurityControl = " & UserSecurityControl & ")"

            If oQuery.executeSQLQuery(oCon, strSQL, 1) Then
                Return GetRestrictedFormsByUser(UserSecurityControl)
            Else
                tblFormLists.Add(New DTO.tblFormList)
                Return tblFormLists.ToArray
            End If

        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try

        Return Nothing

    End Function

    Public Sub DeletetblFormSecurityXref(ByVal FormSecurityXrefControl As Integer)
        'Create a data connection 
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)


        Try
            Dim strSQL As String = "DELETE FROM  tblFormSecurityXref " _
            & " WHERE (tblFormSecurityXref.FormSecurityXrefControl = " _
            & FormSecurityXrefControl & ")"

            oQuery.executeSQLQuery(oCon, strSQL, 1)

        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try
    End Sub

    Public Function CreateRestrictedFormByUser(ByVal FormControl As Integer, ByVal UserSecurityControl As Integer) As DTO.tblFormList()
        'Create a data connection 
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblFormLists As New List(Of DTO.tblFormList)

        Try
            Dim strSQL As String = "insert into  dbo.tblFormSecurityXref " _
                & "(UserSecurityControl ,FormControl)" _
                & " values " _
                & " (" & UserSecurityControl & "," & FormControl & " )"

            If oQuery.executeSQLQuery(oCon, strSQL, 1) Then
                Return GetRestrictedFormsByUser(UserSecurityControl)
            Else
                tblFormLists.Add(New DTO.tblFormList)
                Return tblFormLists.ToArray
            End If

        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try

        Return Nothing

    End Function

    Public Function RestrictAllFormsByUser(ByVal UserSecurityControl As Integer) As DTO.tblFormList()
        'Create a data connection 
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblFormLists As New List(Of DTO.tblFormList)

        Try
            Dim strSQL As String = "insert into  dbo.tblFormSecurityXref  (UserSecurityControl, FormControl) " _
               & " Select " & UserSecurityControl & ",FormControl " _
               & " FROM dbo.tblFormList " _
               & " Where FormControl not in " _
               & " (Select FormControl FROM dbo.tblFormSecurityXref Where UserSecurityControl = " & UserSecurityControl & ")"

            If oQuery.executeSQLQuery(oCon, strSQL, 1) Then
                Return GetRestrictedFormsByUser(UserSecurityControl)
            Else
                tblFormLists.Add(New DTO.tblFormList)
                Return tblFormLists.ToArray
            End If

        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try

        Return Nothing

    End Function

    Public Function UnRestrictAllFormsByUser(ByVal UserSecurityControl As Integer) As DTO.tblFormList()
        'Create a data connection 
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblFormLists As New List(Of DTO.tblFormList)

        Try
            Dim strSQL As String = "Delete from dbo.tblFormSecurityXref where UserSecurityControl = " & UserSecurityControl

            If oQuery.executeSQLQuery(oCon, strSQL, 1) Then
                Return GetUnRestrictedFormsByUser(UserSecurityControl)
            Else
                tblFormLists.Add(New DTO.tblFormList)
                Return tblFormLists.ToArray
            End If

        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try

        Return Nothing

    End Function

#Region "Added for v-8.0 TMS 365"

    ''' <summary>
    ''' Looks up the FormControl based on param FormName and inserts a record into tblFormSecurityXref for the 
    ''' FormControl and UserSecurityControl to restrict the Form from the User
    ''' Returns True on Success and False on Fail (exceptions)
    ''' </summary>
    ''' <param name="FormName"></param>
    ''' <param name="UserSecurityControl"></param>
    ''' <returns>Boolean</returns>
    ''' <remarks>
    ''' Added By LVV on 7/12/17 for v-8.0 TMS 365
    ''' </remarks>
    Public Function RestrictFormForUser365(ByVal FormName As String, ByVal UserSecurityControl As Integer) As Boolean
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim FormControl = db.tblFormLists.Where(Function(x) x.FormName = FormName).Select(Function(x) x.FormControl).FirstOrDefault()

                If Not db.tblFormSecurityXrefs.Any(Function(x) x.FormControl = FormControl And x.UserSecurityControl = UserSecurityControl) Then
                    Dim fsx As New LTS.tblFormSecurityXref
                    With fsx
                        .FormControl = FormControl
                        .UserSecurityControl = UserSecurityControl
                    End With

                    db.tblFormSecurityXrefs.InsertOnSubmit(fsx)
                    db.SubmitChanges()
                End If

                Return True
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
            Return False
        End Using
    End Function


#End Region

#End Region

#Region "Procedure Security By Group"

    ''' <summary>
    ''' In 365 this has been replaced by the call to NGLUserGroupData.GetRestrictedProceduresByGroup() below
    ''' This is because in 365 we need to use the correct base class paging and filtering for grids in cm
    ''' Also LTS is faster and we created a new view
    ''' </summary>
    ''' <param name="GroupControl"></param>
    ''' <returns></returns>
    Public Function GetRestrictedProceduresByGroup(ByVal GroupControl As Integer) As DTO.tblProcedureList()
        'Create a data connection 
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblProcedureLists As New List(Of DTO.tblProcedureList)
        Try
            Dim strSQL As String = "SELECT tblProcedureSecurityGroupXref.ProcedureSecurityGroupXrefControl, " _
            & " tblProcedureList.ProcedureControl, tblProcedureList.ProcedureName, tblProcedureList.ProcedureDescription " _
            & " FROM tblProcedureSecurityGroupXref INNER JOIN tblProcedureList ON " _
            & " tblProcedureSecurityGroupXref.ProcedureControl = tblProcedureList.ProcedureControl " _
            & " WHERE (tblProcedureSecurityGroupXref.UserGroupsControl = " _
            & GroupControl & ")" _
            & " Order By tblProcedureList.ProcedureName"
            Dim oQRet As Ngl.Core.Data.QueryResult = oQuery.ExecuteWithFill(strSQL)
            If Not oQRet.Exception Is Nothing Then
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NotReadProcByGrp"}, New FaultReason("E_DataAccessError"))
            End If
            If Not oQRet.Data Is Nothing AndAlso oQRet.Data.Rows.Count > 0 Then
                For Each oRow As System.Data.DataRow In oQRet.Data.Rows
                    Dim oProcedure As New DTO.tblProcedureList
                    With oProcedure
                        .ProcedureControl = DTran.getDataRowValue(oRow, "ProcedureControl", 0)
                        .ProcedureName = DTran.getDataRowString(oRow, "ProcedureName", "Unnamed")
                        .ProcedureDescription = DTran.getDataRowString(oRow, "ProcedureDescription", "")
                        .ProcedureSecurityGroupXrefControl = DTran.getDataRowValue(oRow, "ProcedureSecurityGroupXrefControl", 0)
                    End With
                    tblProcedureLists.Add(oProcedure)
                Next
            Else
                tblProcedureLists.Add(New DTO.tblProcedureList)
            End If
            Return tblProcedureLists.ToArray()
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
        Return Nothing
    End Function

    'Used in 365 LEUserProcedureController/GetUnRestrictedProceduresByGroup
    Public Function GetUnRestrictedProceduresByGroup(ByVal GroupControl As Integer) As DTO.tblProcedureList()
        'Create a data connection 
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblProcedureLists As New List(Of DTO.tblProcedureList)
        Try
            Dim strSQL As String = " Select " _
                & " tblProcedureList.ProcedureControl, tblProcedureList.ProcedureName, tblProcedureList.ProcedureDescription " _
                & " FROM tblProcedureList " _
                & " WHERE (ProcedureControl NOT IN " _
                & " (SELECT ProcedureControl " _
                    & " FROM dbo.tblProcedureSecurityGroupXref " _
                    & " WHERE tblProcedureSecurityGroupXref.UserGroupsControl = " _
                    & GroupControl & "))" _
                & " Order By ProcedureName"
            Dim oQRet As Ngl.Core.Data.QueryResult = oQuery.ExecuteWithFill(strSQL)
            If Not oQRet.Exception Is Nothing Then
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NotReadProcByGrp"}, New FaultReason("E_DataAccessError"))
            End If
            If Not oQRet.Data Is Nothing AndAlso oQRet.Data.Rows.Count > 0 Then
                For Each oRow As System.Data.DataRow In oQRet.Data.Rows
                    Dim oProcedure As New DTO.tblProcedureList
                    With oProcedure
                        .ProcedureControl = DTran.getDataRowValue(oRow, "ProcedureControl", 0)
                        .ProcedureName = DTran.getDataRowString(oRow, "ProcedureName", "Unnamed")
                        .ProcedureDescription = DTran.getDataRowString(oRow, "ProcedureDescription", "")
                    End With
                    tblProcedureLists.Add(oProcedure)
                Next
            Else
                tblProcedureLists.Add(New DTO.tblProcedureList)
            End If
            Return tblProcedureLists.ToArray()
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
        Return Nothing
    End Function

    'Used in 365 LEUserProcedureController/RemoveProcedureRestrictionFromGroup
    Public Function RemoveRestrictedProcedureByGroup(ByVal ProcedureControl As Integer, ByVal GroupControl As Integer) As DTO.tblProcedureList()
        'Create a data connection 
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblProcedureLists As New List(Of DTO.tblProcedureList)
        Try
            Dim strSQL As String = "DELETE FROM  tblProcedureSecurityGroupXref " _
            & " WHERE (tblProcedureSecurityGroupXref.ProcedureControl = " _
            & ProcedureControl & " AND tblProcedureSecurityGroupXref.UserGroupsControl = " & GroupControl & ")"

            If oQuery.executeSQLQuery(oCon, strSQL, 1) Then
                'update all the dependent users
                strSQL = "DELETE FROM tblProcedureSecurityXref " _
                    & " WHERE ProcedureControl = " & ProcedureControl _
                    & " AND UserSecurityControl in " _
                    & " (SELECT UserSecurityControl " _
                    & " FROM tblUserSecurity " _
                    & " WHERE UserUserGroupsControl = " & GroupControl & ")"
                oQuery.executeSQLQuery(oCon, strSQL, 1)
                Return GetRestrictedProceduresByGroup(GroupControl)
            Else
                tblProcedureLists.Add(New DTO.tblProcedureList)
                Return tblProcedureLists.ToArray
            End If
        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try
        Return Nothing
    End Function

    Public Sub DeletetblProcedureSecurityGroupXref(ByVal ProcedureSecurityGroupXrefControl As Integer)
        'Create a data connection 
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)


        Try
            Dim strSQL As String = "DELETE FROM  tblProcedureSecurityGroupXref " _
            & " WHERE (tblProcedureSecurityGroupXref.ProcedureSecurityGroupXrefControl = " _
            & ProcedureSecurityGroupXrefControl & ")"

            oQuery.executeSQLQuery(oCon, strSQL, 1)

        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try
    End Sub

    ''' <summary>
    ''' There is an updated version of this method for 365 below called NGLUserGroupData.CreateRestrictedProcedureByGroup() 
    ''' That method should have better performance because it which calls a sp and only accesses the db one time
    ''' and does not waste resources returning DTO objects that are not needed by the caller in 365
    ''' </summary>
    ''' <param name="ProcedureControl"></param>
    ''' <param name="GroupControl"></param>
    ''' <returns></returns>
    Public Function CreateRestrictedProcedureByGroup(ByVal ProcedureControl As Integer, ByVal GroupControl As Integer) As DTO.tblProcedureList()
        'Create a data connection 
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblProcedureLists As New List(Of DTO.tblProcedureList)
        Try
            Dim strSQL As String = "insert into  dbo.tblProcedureSecurityGroupXref " _
                & "(UserGroupsControl ,ProcedureControl)" _
                & " values " _
                & " (" & GroupControl & "," & ProcedureControl & " )"

            If oQuery.executeSQLQuery(oCon, strSQL, 1) Then
                'update all the dependent users
                strSQL = "insert into  dbo.tblProcedureSecurityXref  (UserSecurityControl, ProcedureControl) " _
                    & " Select UserSecurityControl,ProcedureControl " _
                    & " FROM dbo.tblUserSecurity as a " _
                    & " Inner Join dbo.tblProcedureSecurityGroupXref as b " _
                    & " on a.UserUserGroupsControl = b.UserGroupsControl " _
                    & " Where a.UserUserGroupsControl = " & GroupControl _
                    & " AND b.ProcedureControl = " & ProcedureControl _
                    & " AND b.ProcedureControl not in " _
                    & " (Select c.ProcedureControl " _
                    & " FROM dbo.tblProcedureSecurityXref as c " _
                    & " Where c.UserSecurityControl = a.UserSecurityControl)"
                oQuery.executeSQLQuery(oCon, strSQL, 1)
                Return GetRestrictedProceduresByGroup(GroupControl)
            Else
                tblProcedureLists.Add(New DTO.tblProcedureList)
                Return tblProcedureLists.ToArray
            End If
        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try
        Return Nothing
    End Function

    Public Function RestrictAllProceduresByGroup(ByVal GroupControl As Integer) As DTO.tblProcedureList()
        'Create a data connection 
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblProcedureList As New List(Of DTO.tblProcedureList)

        Try
            Dim strSQL As String = "insert into  dbo.tblProcedureSecurityGroupXref  (UserGroupsControl, ProcedureControl) " _
               & " Select " & GroupControl & ",ProcedureControl " _
               & " FROM dbo.tblProcedureList " _
               & " Where ProcedureControl not in " _
               & " (Select ProcedureControl FROM dbo.tblProcedureSecurityGroupXref Where UserGroupsControl = " & GroupControl & ")"

            If oQuery.executeSQLQuery(oCon, strSQL, 1) Then
                'update all the dependent users
                strSQL = "insert into  dbo.tblProcedureSecurityXref  (UserSecurityControl, ProcedureControl) " _
                    & " Select UserSecurityControl,ProcedureControl " _
                    & " FROM dbo.tblUserSecurity as a " _
                    & " Inner Join dbo.tblProcedureSecurityGroupXref as b " _
                    & " on a.UserUserGroupsControl = b.UserGroupsControl " _
                    & " Where a.UserUserGroupsControl = " & GroupControl _
                    & " AND b.ProcedureControl not in " _
                    & " (Select c.ProcedureControl " _
                    & " FROM dbo.tblProcedureSecurityXref as c " _
                    & " Where c.UserSecurityControl = a.UserSecurityControl)"
                oQuery.executeSQLQuery(oCon, strSQL, 1)
                Return GetRestrictedProceduresByGroup(GroupControl)
            Else
                tblProcedureList.Add(New DTO.tblProcedureList)
                Return tblProcedureList.ToArray
            End If

        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try

        Return Nothing

    End Function

    Public Function UnRestrictAllProceduresByGroup(ByVal GroupControl As Integer) As DTO.tblProcedureList()
        'Create a data connection 
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblProcedureLists As New List(Of DTO.tblProcedureList)

        Try
            Dim strSQL As String = "Delete from dbo.tblProcedureSecurityGroupXref where UserGroupsControl = " & GroupControl

            If oQuery.executeSQLQuery(oCon, strSQL, 1) Then
                'remove any dependent records
                strSQL = "DELETE FROM dbo.tblProcedureSecurityXref " _
                    & " WHERE UserSecurityControl IN " _
                    & " (SELECT UserSecurityControl FROM dbo.tblUserSecurity Where UserUserGroupsControl = " & GroupControl _
                    & " ) AND ProcedureControl IN " _
                    & " (SELECT ProcedureControl FROM dbo.tblProcedureSecurityGroupXref WHERE UserGroupsControl = " & GroupControl & ")"
                oQuery.executeSQLQuery(oCon, strSQL, 1)
                Return GetUnRestrictedProceduresByGroup(GroupControl)
            Else
                tblProcedureLists.Add(New DTO.tblProcedureList)
                Return tblProcedureLists.ToArray
            End If

        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try

        Return Nothing

    End Function

#End Region

#Region "Procedure Security By User"
    Public Function GetRestrictedProceduresByUser(ByVal UserSecurityControl As Integer) As DTO.tblProcedureList()

        'Create a data connection 
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblProcedureLists As New List(Of DTO.tblProcedureList)

        Try
            Dim strSQL As String = "SELECT " _
                & "     a.ProcedureSecurityXrefControl," _
                & "     b.ProcedureControl, " _
                & "     b.ProcedureName, " _
                & "     b.ProcedureDescription ," _
                & "     Case ISNULL(" _
                & "         (Select Top 1 " _
                & "             d.ProcedureSecurityGroupXrefControl" _
                & "         From" _
                & "             dbo.tblProcedureSecurityGroupXref as d" _
                & "         Where" _
                & "             d.UserGroupsControl = c.UserUserGroupsControl" _
                & "             AND" _
                & "             d.ProcedureControl = a.ProcedureControl" _
                & "         ) ,0)" _
                & "     When 0 then 1" _
                & "     Else 0" _
                & "     End as Override " _
                & " FROM" _
                & "     dbo.tblProcedureSecurityXref as a " _
                & "         INNER JOIN dbo.tblProcedureList as b " _
                & "             ON a.ProcedureControl = b.ProcedureControl " _
                & "         INNER JOIN  dbo.tblUserSecurity as c " _
                & "             ON a.UserSecurityControl = c.UserSecurityControl" _
                & " WHERE" _
                & "     a.UserSecurityControl = " & UserSecurityControl _
                & " Order By " _
                & "     b.ProcedureName"

            Dim oQRet As Ngl.Core.Data.QueryResult = oQuery.ExecuteWithFill(strSQL)
            If Not oQRet.Exception Is Nothing Then
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NotReadProcByUser"}, New FaultReason("E_DataAccessError"))
            End If
            If Not oQRet.Data Is Nothing AndAlso oQRet.Data.Rows.Count > 0 Then
                For Each oRow As System.Data.DataRow In oQRet.Data.Rows
                    Dim oProcedure As New DTO.tblProcedureList
                    With oProcedure
                        .ProcedureControl = DTran.getDataRowValue(oRow, "ProcedureControl", 0)
                        .ProcedureName = DTran.getDataRowString(oRow, "ProcedureName", "Unnamed")
                        .ProcedureDescription = DTran.getDataRowString(oRow, "ProcedureDescription", "")
                        .ProcedureSecurityXrefControl = DTran.getDataRowValue(oRow, "ProcedureSecurityXrefControl", 0)
                        .ProcedureUserOverrideGroup = If(DTran.getDataRowValue(oRow, "Override", 0) = 0, False, True)
                    End With
                    tblProcedureLists.Add(oProcedure)
                Next
            Else
                tblProcedureLists.Add(New DTO.tblProcedureList)
            End If
            Return tblProcedureLists.ToArray()
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

        Return Nothing


    End Function

    Public Function GetUnRestrictedProceduresByUser(ByVal UserSecurityControl As Integer) As DTO.tblProcedureList()

        'Create a data connection 
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblProcedureLists As New List(Of DTO.tblProcedureList)

        Try
            Dim strSQL As String = " Select " _
                & " tblProcedureList.ProcedureControl, tblProcedureList.ProcedureName, tblProcedureList.ProcedureDescription " _
                & " FROM tblProcedureList " _
                & " WHERE (ProcedureControl NOT IN " _
                & " (SELECT ProcedureControl " _
                    & " FROM dbo.tblProcedureSecurityXref " _
                    & " WHERE tblProcedureSecurityXref.UserSecurityControl = " _
                    & UserSecurityControl & "))" _
                & " Order By ProcedureName"
            Dim oQRet As Ngl.Core.Data.QueryResult = oQuery.ExecuteWithFill(strSQL)
            If Not oQRet.Exception Is Nothing Then
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NotReadProcByUser"}, New FaultReason("E_DataAccessError"))
            End If
            If Not oQRet.Data Is Nothing AndAlso oQRet.Data.Rows.Count > 0 Then
                For Each oRow As System.Data.DataRow In oQRet.Data.Rows
                    Dim oProcedure As New DTO.tblProcedureList
                    With oProcedure
                        .ProcedureControl = DTran.getDataRowValue(oRow, "ProcedureControl", 0)
                        .ProcedureName = DTran.getDataRowString(oRow, "ProcedureName", "Unnamed")
                        .ProcedureDescription = DTran.getDataRowString(oRow, "ProcedureDescription", "")
                    End With
                    tblProcedureLists.Add(oProcedure)
                Next
            Else
                tblProcedureLists.Add(New DTO.tblProcedureList)
            End If
            Return tblProcedureLists.ToArray()
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

        Return Nothing


    End Function

    Public Function RemoveRestrictedProcedureByUser(ByVal ProcedureControl As Integer, ByVal UserSecurityControl As Integer) As DTO.tblProcedureList()
        'Create a data connection 
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblProcedureLists As New List(Of DTO.tblProcedureList)

        Try
            Dim strSQL As String = "DELETE FROM  tblProcedureSecurityXref " _
            & " WHERE (tblProcedureSecurityXref.ProcedureControl = " _
            & ProcedureControl & " AND tblProcedureSecurityXref.UserSecurityControl = " & UserSecurityControl & ")"

            If oQuery.executeSQLQuery(oCon, strSQL, 1) Then
                Return GetRestrictedProceduresByUser(UserSecurityControl)
            Else
                tblProcedureLists.Add(New DTO.tblProcedureList)
                Return tblProcedureLists.ToArray
            End If

        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try

        Return Nothing

    End Function

    Public Sub DeletetblProcedureSecurityXref(ByVal ProcedureSecurityXrefControl As Integer)
        'Create a data connection 
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)


        Try
            Dim strSQL As String = "DELETE FROM  tblProcedureSecurityXref " _
            & " WHERE (tblProcedureSecurityXref.ProcedureSecurityXrefControl = " _
            & ProcedureSecurityXrefControl & ")"

            oQuery.executeSQLQuery(oCon, strSQL, 1)

        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try
    End Sub

    Public Function CreateRestrictedProcedureByUser(ByVal ProcedureControl As Integer, ByVal UserSecurityControl As Integer) As DTO.tblProcedureList()
        'Create a data connection 
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblProcedureLists As New List(Of DTO.tblProcedureList)

        Try
            Dim strSQL As String = "insert into  dbo.tblProcedureSecurityXref " _
                & "(UserSecurityControl ,ProcedureControl)" _
                & " values " _
                & " (" & UserSecurityControl & "," & ProcedureControl & " )"

            If oQuery.executeSQLQuery(oCon, strSQL, 0) Then
                'Delete any subscribed alerts
                Try
                    strSQL = "DELETE FROM  tblProcAlertUserXref " _
                    & " WHERE (tblProcAlertUserXref.ProcedureControl = " _
                    & ProcedureControl & " AND tblProcAlertUserXref.UserSecurityControl = " _
                    & UserSecurityControl & ")"
                    oQuery.executeSQLQuery(oCon, strSQL, 0)

                Catch ex As Exception
                    'do nothing
                End Try
                Return GetRestrictedProceduresByUser(UserSecurityControl)
            Else
                tblProcedureLists.Add(New DTO.tblProcedureList)
                Return tblProcedureLists.ToArray
            End If

        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try

        Return Nothing

    End Function

    Public Function RestrictAllProceduresByUser(ByVal UserSecurityControl As Integer) As DTO.tblProcedureList()
        'Create a data connection 
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblProcedureLists As New List(Of DTO.tblProcedureList)

        Try
            Dim strSQL As String = "insert into  dbo.tblProcedureSecurityXref  (UserSecurityControl, ProcedureControl) " _
               & " Select " & UserSecurityControl & ",ProcedureControl " _
               & " FROM dbo.tblProcedureList " _
               & " Where ProcedureControl not in " _
               & " (Select ProcedureControl FROM dbo.tblProcedureSecurityXref Where UserSecurityControl = " & UserSecurityControl & ")"

            If oQuery.executeSQLQuery(oCon, strSQL, 1) Then
                'Delete any subscribed alerts
                Try
                    strSQL = "DELETE FROM  tblProcAlertUserXref " _
                    & " WHERE tblProcAlertUserXref.UserSecurityControl = " _
                    & UserSecurityControl
                    oQuery.executeSQLQuery(oCon, strSQL, 0)

                Catch ex As Exception
                    'do nothing
                End Try
                Return GetRestrictedProceduresByUser(UserSecurityControl)
            Else
                tblProcedureLists.Add(New DTO.tblProcedureList)
                Return tblProcedureLists.ToArray
            End If

        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try

        Return Nothing

    End Function

    Public Function UnRestrictAllProceduresByUser(ByVal UserSecurityControl As Integer) As DTO.tblProcedureList()
        'Create a data connection 
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblProcedureLists As New List(Of DTO.tblProcedureList)

        Try
            Dim strSQL As String = "Delete from dbo.tblProcedureSecurityXref where UserSecurityControl = " & UserSecurityControl

            If oQuery.executeSQLQuery(oCon, strSQL, 1) Then
                Return GetUnRestrictedProceduresByUser(UserSecurityControl)
            Else
                tblProcedureLists.Add(New DTO.tblProcedureList)
                Return tblProcedureLists.ToArray
            End If

        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try

        Return Nothing

    End Function

#End Region

#Region "Report Security By Group"

    ''' <summary>
    ''' In 365 this has been replaced by the call to NGLUserGroupData.GetRestrictedReportsByGroup() below
    ''' This is because in 365 we need to use the correct base class paging and filtering for grids in cm
    ''' Also LTS is faster and we created a new view
    ''' </summary>
    ''' <param name="GroupControl"></param>
    ''' <returns></returns>
    Public Function GetRestrictedReportsByGroup(ByVal GroupControl As Integer) As DTO.tblReportList()
        'Create a data connection 
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblReportLists As New List(Of DTO.tblReportList)
        Try
            Dim strSQL As String = "SELECT tblReportSecurityGroupXref.ReportSecurityGroupXrefControl, " _
            & " tblReportList.ReportControl, tblReportList.ReportName, tblReportList.ReportDescription, " _
            & " tblReportList.ReportReportMenuControl,tblReportList.ReportMenuSequence " _
            & " FROM tblReportSecurityGroupXref INNER JOIN tblReportList ON " _
            & " tblReportSecurityGroupXref.ReportControl = tblReportList.ReportControl " _
            & " WHERE (tblReportSecurityGroupXref.UserGroupsControl = " _
            & GroupControl & ")" _
            & " Order By tblReportList.ReportName"
            Dim oQRet As Ngl.Core.Data.QueryResult = oQuery.ExecuteWithFill(strSQL)
            If Not oQRet.Exception Is Nothing Then
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NotReadRptByGrp"}, New FaultReason("E_DataAccessError"))
            End If
            If Not oQRet.Data Is Nothing AndAlso oQRet.Data.Rows.Count > 0 Then
                For Each oRow As System.Data.DataRow In oQRet.Data.Rows
                    Dim oReport As New DTO.tblReportList
                    With oReport
                        .ReportControl = DTran.getDataRowValue(oRow, "ReportControl", 0)
                        .ReportName = DTran.getDataRowString(oRow, "ReportName", "Unnamed")
                        .ReportDescription = DTran.getDataRowString(oRow, "ReportDescription", "")
                        .ReportReportMenuControl = DTran.getDataRowValue(oRow, "ReportReportMenuControl", 0)
                        .ReportMenuSequence = DTran.getDataRowValue(oRow, "ReportMenuSequence", 0)
                        .ReportSecurityGroupXrefControl = DTran.getDataRowValue(oRow, "ReportSecurityGroupXrefControl", 0)
                    End With
                    tblReportLists.Add(oReport)
                Next
            Else
                tblReportLists.Add(New DTO.tblReportList)
            End If
            Return tblReportLists.ToArray()
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
        Return Nothing
    End Function

    'Used in 365 LEUserReportController/GetUnRestrictedReportsByGroup
    Public Function GetUnRestrictedReportsByGroup(ByVal GroupControl As Integer) As DTO.tblReportList()
        'Create a data connection 
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblReportLists As New List(Of DTO.tblReportList)
        Try
            Dim strSQL As String = " Select " _
                & " tblReportList.ReportControl, tblReportList.ReportName, tblReportList.ReportDescription, " _
                & " tblReportList.ReportReportMenuControl,tblReportList.ReportMenuSequence " _
                & " FROM tblReportList " _
                & " WHERE (ReportControl NOT IN " _
                & " (SELECT ReportControl " _
                    & " FROM dbo.tblReportSecurityGroupXref " _
                    & " WHERE tblReportSecurityGroupXref.UserGroupsControl = " _
                    & GroupControl & "))" _
                & " Order By ReportName"
            Dim oQRet As Ngl.Core.Data.QueryResult = oQuery.ExecuteWithFill(strSQL)
            If Not oQRet.Exception Is Nothing Then
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NotReadRptByGrp"}, New FaultReason("E_DataAccessError"))
            End If
            If Not oQRet.Data Is Nothing AndAlso oQRet.Data.Rows.Count > 0 Then
                For Each oRow As System.Data.DataRow In oQRet.Data.Rows
                    Dim oReport As New DTO.tblReportList
                    With oReport
                        .ReportControl = DTran.getDataRowValue(oRow, "ReportControl", 0)
                        .ReportName = DTran.getDataRowString(oRow, "ReportName", "Unnamed")
                        .ReportDescription = DTran.getDataRowString(oRow, "ReportDescription", "")
                        .ReportReportMenuControl = DTran.getDataRowValue(oRow, "ReportReportMenuControl", 0)
                        .ReportMenuSequence = DTran.getDataRowValue(oRow, "ReportMenuSequence", 0)
                    End With
                    tblReportLists.Add(oReport)
                Next
            Else
                tblReportLists.Add(New DTO.tblReportList)
            End If
            Return tblReportLists.ToArray()
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
        Return Nothing
    End Function

    'Used in 365 LEUserReportController/RemoveReportRestrictionFromGroup
    Public Function RemoveRestrictedReportByGroup(ByVal ReportControl As Integer, ByVal GroupControl As Integer) As DTO.tblReportList()
        'Create a data connection 
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblReportLists As New List(Of DTO.tblReportList)
        Try
            Dim strSQL As String = "DELETE FROM  tblReportSecurityGroupXref " _
            & " WHERE (tblReportSecurityGroupXref.ReportControl = " _
            & ReportControl & " AND tblReportSecurityGroupXref.UserGroupsControl = " & GroupControl & ")"

            If oQuery.executeSQLQuery(oCon, strSQL, 1) Then
                'update all the dependent users
                strSQL = "DELETE FROM tblReportSecurityXref " _
                    & " WHERE ReportControl = " & ReportControl _
                    & " AND UserSecurityControl in " _
                    & " (SELECT UserSecurityControl " _
                    & " FROM tblUserSecurity " _
                    & " WHERE UserUserGroupsControl = " & GroupControl & ")"
                oQuery.executeSQLQuery(oCon, strSQL, 1)
                Return GetRestrictedReportsByGroup(GroupControl)
            Else
                tblReportLists.Add(New DTO.tblReportList)
                Return tblReportLists.ToArray
            End If
        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try
        Return Nothing
    End Function

    Public Sub DeletetblReportSecurityGroupXref(ByVal ReportSecurityGroupXrefControl As Integer)
        'Create a data connection 
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)


        Try
            Dim strSQL As String = "DELETE FROM  tblReportSecurityGroupXref " _
            & " WHERE (tblReportSecurityGroupXref.ReportSecurityGroupXrefControl = " _
            & ReportSecurityGroupXrefControl & ")"

            oQuery.executeSQLQuery(oCon, strSQL, 1)

        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try
    End Sub

    ''' <summary>
    ''' There is an updated version of this method for 365 below called NGLUserGroupData.CreateRestrictedReportByGroup() 
    ''' That method should have better performance because it which calls a sp and only accesses the db one time
    ''' and does not waste resources returning DTO objects that are not needed by the caller in 365
    ''' </summary>
    ''' <param name="ReportControl"></param>
    ''' <param name="GroupControl"></param>
    ''' <returns></returns>
    Public Function CreateRestrictedReportByGroup(ByVal ReportControl As Integer, ByVal GroupControl As Integer) As DTO.tblReportList()
        'Create a data connection 
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblReportLists As New List(Of DTO.tblReportList)
        Try
            Dim strSQL As String = "insert into  dbo.tblReportSecurityGroupXref " _
                & "(UserGroupsControl ,ReportControl)" _
                & " values " _
                & " (" & GroupControl & "," & ReportControl & " )"

            If oQuery.executeSQLQuery(oCon, strSQL, 1) Then
                'update all the dependent users
                strSQL = "insert into  dbo.tblReportSecurityXref  (UserSecurityControl, ReportControl) " _
                    & " Select UserSecurityControl,ReportControl " _
                    & " FROM dbo.tblUserSecurity as a " _
                    & " Inner Join dbo.tblReportSecurityGroupXref as b " _
                    & " on a.UserUserGroupsControl = b.UserGroupsControl " _
                    & " Where a.UserUserGroupsControl = " & GroupControl _
                    & " AND b.ReportControl = " & ReportControl _
                    & " AND b.ReportControl not in " _
                    & " (Select c.ReportControl " _
                    & " FROM dbo.tblReportSecurityXref as c " _
                    & " Where c.UserSecurityControl = a.UserSecurityControl)"
                oQuery.executeSQLQuery(oCon, strSQL, 1)
                Return GetRestrictedReportsByGroup(GroupControl)
            Else
                tblReportLists.Add(New DTO.tblReportList)
                Return tblReportLists.ToArray
            End If
        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try
        Return Nothing
    End Function

    Public Function RestrictAllReportsByGroup(ByVal GroupControl As Integer) As DTO.tblReportList()
        'Create a data connection 
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblReportList As New List(Of DTO.tblReportList)

        Try
            Dim strSQL As String = "insert into  dbo.tblReportSecurityGroupXref  (UserGroupsControl, ReportControl) " _
               & " Select " & GroupControl & ",ReportControl " _
               & " FROM dbo.tblReportList " _
               & " Where ReportControl not in " _
               & " (Select ReportControl FROM dbo.tblReportSecurityGroupXref Where UserGroupsControl = " & GroupControl & ")"

            If oQuery.executeSQLQuery(oCon, strSQL, 1) Then
                'update all the dependent users
                strSQL = "insert into  dbo.tblReportSecurityXref  (UserSecurityControl, ReportControl) " _
                    & " Select UserSecurityControl,ReportControl " _
                    & " FROM dbo.tblUserSecurity as a " _
                    & " Inner Join dbo.tblReportSecurityGroupXref as b " _
                    & " on a.UserUserGroupsControl = b.UserGroupsControl " _
                    & " Where a.UserUserGroupsControl = " & GroupControl _
                    & " AND b.ReportControl not in " _
                    & " (Select c.ReportControl " _
                    & " FROM dbo.tblReportSecurityXref as c " _
                    & " Where c.UserSecurityControl = a.UserSecurityControl)"
                oQuery.executeSQLQuery(oCon, strSQL, 1)
                Return GetRestrictedReportsByGroup(GroupControl)
            Else
                tblReportList.Add(New DTO.tblReportList)
                Return tblReportList.ToArray
            End If

        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try

        Return Nothing

    End Function

    Public Function UnRestrictAllReportsByGroup(ByVal GroupControl As Integer) As DTO.tblReportList()
        'Create a data connection 
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblReportLists As New List(Of DTO.tblReportList)

        Try
            Dim strSQL As String = "Delete from dbo.tblReportSecurityGroupXref where UserGroupsControl = " & GroupControl

            If oQuery.executeSQLQuery(oCon, strSQL, 1) Then
                'remove any dependent records
                strSQL = "DELETE FROM dbo.tblReportSecurityXref " _
                    & " WHERE UserSecurityControl IN " _
                    & " (SELECT UserSecurityControl FROM dbo.tblUserSecurity Where UserUserGroupsControl = " & GroupControl _
                    & " ) AND ReportControl IN " _
                    & " (SELECT ReportControl FROM dbo.tblReportSecurityGroupXref WHERE UserGroupsControl = " & GroupControl & ")"
                oQuery.executeSQLQuery(oCon, strSQL, 1)
                Return GetUnRestrictedReportsByGroup(GroupControl)
            Else
                tblReportLists.Add(New DTO.tblReportList)
                Return tblReportLists.ToArray
            End If

        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try

        Return Nothing

    End Function

#End Region

#Region "Report Security By User"


    Public Function GetRestrictedReportsByUser(ByVal UserSecurityControl As Integer) As DTO.tblReportList()

        'Create a data connection 
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblReportLists As New List(Of DTO.tblReportList)
        'code change - added these fields to the select. pfm 12/6/2013.
        'ReportServerURL
        'ReportURL
        Try
            Dim strSQL As String = "SELECT " _
                & "     a.ReportSecurityXrefControl," _
                & "     b.ReportControl, " _
                & "     b.ReportName, " _
                & "     b.ReportDescription ," _
                & "     b.ReportReportMenuControl," _
                & "     b.ReportMenuSequence, b.ReportServerURL,b.ReportURL, " _
                & "     Case ISNULL(" _
                & "         (Select Top 1 " _
                & "             d.ReportSecurityGroupXrefControl" _
                & "         From" _
                & "             dbo.tblReportSecurityGroupXref as d" _
                & "         Where" _
                & "             d.UserGroupsControl = c.UserUserGroupsControl" _
                & "             AND" _
                & "             d.ReportControl = a.ReportControl" _
                & "         ) ,0)" _
                & "     When 0 then 1" _
                & "     Else 0" _
                & "     End as Override " _
                & " FROM" _
                & "     dbo.tblReportSecurityXref as a " _
                & "         INNER JOIN dbo.tblReportList as b " _
                & "             ON a.ReportControl = b.ReportControl " _
                & "         INNER JOIN  dbo.tblUserSecurity as c " _
                & "             ON a.UserSecurityControl = c.UserSecurityControl" _
                & " WHERE" _
                & "     a.UserSecurityControl = " & UserSecurityControl _
                & " Order By " _
                & "     b.ReportName"

            'Dim strSQL As String = "SELECT tblReportSecurityXref.ReportSecurityXrefControl, " _
            '& " tblReportList.ReportControl, tblReportList.ReportName, tblReportList.ReportDescription, " _
            '& " tblReportList.ReportReportMenuControl,tblReportList.ReportMenuSequence " _
            '& " FROM tblReportSecurityXref INNER JOIN tblReportList ON " _
            '& " tblReportSecurityXref.ReportControl = tblReportList.ReportControl " _
            '& " WHERE (tblReportSecurityXref.UserSecurityControl = " _
            '& UserSecurityControl & ")" _
            '& " Order By tblReportList.ReportName"
            Dim oQRet As Ngl.Core.Data.QueryResult = oQuery.ExecuteWithFill(strSQL)
            If Not oQRet.Exception Is Nothing Then
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NotReadRptByUser"}, New FaultReason("E_DataAccessError"))
            End If
            If Not oQRet.Data Is Nothing AndAlso oQRet.Data.Rows.Count > 0 Then
                For Each oRow As System.Data.DataRow In oQRet.Data.Rows
                    Dim oReport As New DTO.tblReportList
                    With oReport
                        .ReportControl = DTran.getDataRowValue(oRow, "ReportControl", 0)
                        .ReportName = DTran.getDataRowString(oRow, "ReportName", "Unnamed")
                        .ReportDescription = DTran.getDataRowString(oRow, "ReportDescription", "")
                        .ReportReportMenuControl = DTran.getDataRowValue(oRow, "ReportReportMenuControl", 0)
                        .ReportMenuSequence = DTran.getDataRowValue(oRow, "ReportMenuSequence", 0)
                        .ReportSecurityXrefControl = DTran.getDataRowValue(oRow, "ReportSecurityXrefControl", 0)
                        .ReportUserOverrideGroup = If(DTran.getDataRowValue(oRow, "Override", 0) = 0, False, True)
                        .ReportServerURL = DTran.getDataRowValue(oRow, "ReportServerURL", "")
                        .ReportURL = DTran.getDataRowValue(oRow, "ReportURL", "")
                    End With
                    tblReportLists.Add(oReport)
                Next
            Else
                tblReportLists.Add(New DTO.tblReportList)
            End If
            Return tblReportLists.ToArray()
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


        Return Nothing


    End Function

    Public Function GetUnRestrictedReportsByUserName(ByVal UserName As String, ByVal FromReportControl As Integer, ByVal ToReportControl As Integer) As DTO.tblReportList()

        'Create a data connection 
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblReportLists As New List(Of DTO.tblReportList)

        Try
            Dim oUser As DTO.tblUserSecurity = GettblUserSecurityByUserName(UserName)
            Dim strSQL As String = ""
            If oUser.UserUserGroupsControl > 0 Then
                strSQL = " Select " _
                & " tblReportList.ReportControl, tblReportList.ReportName, tblReportList.ReportDescription, " _
                & " tblReportList.ReportReportMenuControl,tblReportList.ReportMenuSequence " _
                & " FROM tblReportList " _
                & " WHERE (ReportControl NOT IN " _
                & " (SELECT ReportControl " _
                    & " FROM dbo.tblReportSecurityGroupXref " _
                    & " WHERE tblReportSecurityGroupXref.UserGroupsControl = " _
                    & oUser.UserUserGroupsControl & "))" _
                    & " AND ReportControl Between " & FromReportControl & " AND " & ToReportControl _
                & " Order By ReportName"
            Else
                strSQL = " Select " _
                & " tblReportList.ReportControl, tblReportList.ReportName, tblReportList.ReportDescription, " _
                & " tblReportList.ReportReportMenuControl,tblReportList.ReportMenuSequence, tblReportList.ReportShowInTree " _
                & " FROM tblReportList " _
                & " WHERE (ReportControl NOT IN " _
                & " (SELECT ReportControl " _
                    & " FROM dbo.tblReportSecurityXref " _
                    & " WHERE tblReportSecurityXref.UserSecurityControl = " _
                    & oUser.UserSecurityControl & "))" _
                    & " AND ReportControl Between " & FromReportControl & " AND " & ToReportControl _
                & " Order By ReportName"
            End If

            Dim oQRet As Ngl.Core.Data.QueryResult = oQuery.ExecuteWithFill(strSQL)
            If Not oQRet.Exception Is Nothing Then
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NotReadRptByUser"}, New FaultReason("E_DataAccessError"))
            End If
            If Not oQRet.Data Is Nothing AndAlso oQRet.Data.Rows.Count > 0 Then
                For Each oRow As System.Data.DataRow In oQRet.Data.Rows
                    Dim oReport As New DTO.tblReportList
                    With oReport
                        .ReportControl = DTran.getDataRowValue(oRow, "ReportControl", 0)
                        .ReportName = DTran.getDataRowString(oRow, "ReportName", "Unnamed")
                        .ReportDescription = DTran.getDataRowString(oRow, "ReportDescription", "")
                        .ReportReportMenuControl = DTran.getDataRowValue(oRow, "ReportReportMenuControl", 0)
                        .ReportMenuSequence = DTran.getDataRowValue(oRow, "ReportMenuSequence", 0)
                        .ReportShowInTree = DTran.getDataRowValue(oRow, "ReportShowInTree", False)
                    End With
                    tblReportLists.Add(oReport)
                Next
            Else
                tblReportLists.Add(New DTO.tblReportList)
            End If
            Return tblReportLists.ToArray()
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


        Return Nothing


    End Function

    Public Function GetUnRestrictedReportsByUser(ByVal UserSecurityControl As Integer) As DTO.tblReportList()

        'Create a data connection 
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblReportLists As New List(Of DTO.tblReportList)
        'code change - added these fields to the select. pfm 12/6/2013.
        'ReportServerURL
        'ReportURL
        Try
            Dim strSQL As String = " Select " _
                & " tblReportList.ReportControl, tblReportList.ReportName, tblReportList.ReportDescription, " _
                & " tblReportList.ReportReportMenuControl,tblReportList.ReportMenuSequence,tblReportList.ReportServerURL,tblReportList.ReportURL, tblReportList.ReportShowInTree " _
                & " FROM tblReportList " _
                & " WHERE (ReportControl NOT IN " _
                & " (SELECT ReportControl " _
                    & " FROM dbo.tblReportSecurityXref " _
                    & " WHERE tblReportSecurityXref.UserSecurityControl = " _
                    & UserSecurityControl & "))" _
                & " Order By ReportName"
            Dim oQRet As Ngl.Core.Data.QueryResult = oQuery.ExecuteWithFill(strSQL)
            If Not oQRet.Exception Is Nothing Then
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NotReadRptByUser"}, New FaultReason("E_DataAccessError"))
            End If
            If Not oQRet.Data Is Nothing AndAlso oQRet.Data.Rows.Count > 0 Then
                For Each oRow As System.Data.DataRow In oQRet.Data.Rows
                    Dim oReport As New DTO.tblReportList
                    With oReport
                        .ReportControl = DTran.getDataRowValue(oRow, "ReportControl", 0)
                        .ReportName = DTran.getDataRowString(oRow, "ReportName", "Unnamed")
                        .ReportDescription = DTran.getDataRowString(oRow, "ReportDescription", "")
                        .ReportReportMenuControl = DTran.getDataRowValue(oRow, "ReportReportMenuControl", 0)
                        .ReportMenuSequence = DTran.getDataRowValue(oRow, "ReportMenuSequence", 0)
                        .ReportServerURL = DTran.getDataRowValue(oRow, "ReportServerURL", "")
                        .ReportURL = DTran.getDataRowValue(oRow, "ReportURL", "")
                        .ReportShowInTree = DTran.getDataRowValue(oRow, "ReportShowInTree", False)
                    End With
                    tblReportLists.Add(oReport)
                Next
            Else
                tblReportLists.Add(New DTO.tblReportList)
            End If
            Return tblReportLists.ToArray()
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

        Return Nothing


    End Function

    Public Function RemoveRestrictedReportByUser(ByVal ReportControl As Integer, ByVal UserSecurityControl As Integer) As DTO.tblReportList()
        'Create a data connection 
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblReportLists As New List(Of DTO.tblReportList)

        Try
            Dim strSQL As String = "DELETE FROM  tblReportSecurityXref " _
            & " WHERE (tblReportSecurityXref.ReportControl = " _
            & ReportControl & " AND tblReportSecurityXref.UserSecurityControl = " & UserSecurityControl & ")"

            If oQuery.executeSQLQuery(oCon, strSQL, 1) Then
                Return GetRestrictedReportsByUser(UserSecurityControl)
            Else
                tblReportLists.Add(New DTO.tblReportList)
                Return tblReportLists.ToArray
            End If

        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try

        Return Nothing

    End Function

    Public Sub DeletetblReportSecurityXref(ByVal ReportSecurityXrefControl As Integer)
        'Create a data connection 
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)


        Try
            Dim strSQL As String = "DELETE FROM  tblReportSecurityXref " _
            & " WHERE (tblReportSecurityXref.ReportSecurityXrefControl = " _
            & ReportSecurityXrefControl & ")"

            oQuery.executeSQLQuery(oCon, strSQL, 1)

        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try
    End Sub

    Public Function CreateRestrictedReportByUser(ByVal ReportControl As Integer, ByVal UserSecurityControl As Integer) As DTO.tblReportList()
        'Create a data connection 
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblReportLists As New List(Of DTO.tblReportList)

        Try
            Dim strSQL As String = "insert into  dbo.tblReportSecurityXref " _
                & "(UserSecurityControl ,ReportControl)" _
                & " values " _
                & " (" & UserSecurityControl & "," & ReportControl & " )"

            If oQuery.executeSQLQuery(oCon, strSQL, 1) Then
                Return GetRestrictedReportsByUser(UserSecurityControl)
            Else
                tblReportLists.Add(New DTO.tblReportList)
                Return tblReportLists.ToArray
            End If

        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try

        Return Nothing

    End Function

    Public Function RestrictReportForAllNEXTRackOnlyUsers(ByVal ReportControl As Integer) As Boolean

        Try
            If ReportControl = 0 Then Return False
            Dim nextrackOnlyUsers() As DTO.tblUserSecurity = GettblUserSecuritysNEXTRackOnly()
            If nextrackOnlyUsers Is Nothing Then Return False
            Return CreateRestrictedReportByUsers(ReportControl, nextrackOnlyUsers)
        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try

            Catch ex As Exception

            End Try
            Try

            Catch ex As Exception

            End Try
        End Try

        Return False

    End Function

    Public Function CreateRestrictedReportByUsers(ByVal ReportControl As Integer, ByVal UserSecuritys() As DTO.tblUserSecurity) As Boolean

        Try
            If UserSecuritys Is Nothing Then Return False
            For Each item In UserSecuritys
                CreateRestrictedReportByUser(ReportControl, item.UserSecurityControl)
            Next
            Return True
        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try

            Catch ex As Exception

            End Try
            Try

            Catch ex As Exception

            End Try
        End Try

        Return False

    End Function

    Public Function RestrictAllReportsByUser(ByVal UserSecurityControl As Integer) As DTO.tblReportList()
        'Create a data connection 
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblReportLists As New List(Of DTO.tblReportList)

        Try
            Dim strSQL As String = "insert into  dbo.tblReportSecurityXref  (UserSecurityControl, ReportControl) " _
               & " Select " & UserSecurityControl & ",ReportControl " _
               & " FROM dbo.tblReportList " _
               & " Where ReportControl not in " _
               & " (Select ReportControl FROM dbo.tblReportSecurityXref Where UserSecurityControl = " & UserSecurityControl & ")"

            If oQuery.executeSQLQuery(oCon, strSQL, 1) Then
                Return GetRestrictedReportsByUser(UserSecurityControl)
            Else
                tblReportLists.Add(New DTO.tblReportList)
                Return tblReportLists.ToArray
            End If

        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try

        Return Nothing

    End Function

    Public Function UnRestrictAllReportsByUser(ByVal UserSecurityControl As Integer) As DTO.tblReportList()
        'Create a data connection 
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblReportLists As New List(Of DTO.tblReportList)

        Try
            Dim strSQL As String = "Delete from dbo.tblReportSecurityXref where UserSecurityControl = " & UserSecurityControl

            If oQuery.executeSQLQuery(oCon, strSQL, 1) Then
                Return GetUnRestrictedReportsByUser(UserSecurityControl)
            Else
                tblReportLists.Add(New DTO.tblReportList)
                Return tblReportLists.ToArray
            End If

        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try

        Return Nothing

    End Function

#End Region

#Region "User Security By Group"

    Public Function RestrictedCarriersForSalesReps() As List(Of Integer)
        Dim lCarriers As New List(Of Integer)
        If Me.Parameters.CatControl = 11 Then
            Using db As New NGLMASSecurityDataContext(ConnectionString)
                Try
                    lCarriers = db.tblUserSecurityCarriers.Where(Function(x) x.USCUserSecurityControl = Me.Parameters.UserControl).Select(Function(y) y.USCCarrierControl).ToList()
                Catch ex As Exception
                    Return lCarriers
                End Try
            End Using
        End If
        Return lCarriers
    End Function

    Public Function isUserRateShopRestricted(ByVal userName As String, ByVal dblCarrierCostUpchargeLimitVisibility As Double) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim oUser As LTS.tblUserSecurity = db.tblUserSecurities.Where(Function(x) x.UserName = userName).FirstOrDefault()
                If oUser Is Nothing Then Return True
                'get the users group category
                Dim oUserGroup As LTS.tblUserGroup = db.tblUserGroups.Where(Function(y) y.UserGroupsControl = oUser.UserUserGroupsControl).FirstOrDefault()
                If oUserGroup Is Nothing Then Return True
                blnRet = (Not NGLLinkDataBaseClass.CanUserSeeCarrierCost(dblCarrierCostUpchargeLimitVisibility, oUserGroup.UserGroupsUGCControl))

            Catch ex As Exception
                Return True
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Determines if a Group has Category NEXTrackOnly or Carrier.
    ''' If GroupControl is 0 then return the default False
    ''' </summary>
    ''' <param name="GroupControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 1/21/20
    ''' Modified By LVV on 2/17/20
    '''  Added logic to include the Inactive Role/Group as NTOnly for licensing purposes
    ''' Modified by RHR for v-8.5.4.001 on 07/03/2023 we now call udfGetNTOnlyBasedOnGroup
    ''' </remarks>
    Public Function IsGroupNEXTrackOnly(ByVal GroupControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                If GroupControl <> 0 Then
                    'Modified by RHR for v-8.5.4.001 on 07/03/2023 we now call udfGetNTOnlyBasedOnGroup
                    Dim bNTOnly As Boolean? = db.udfGetNTOnlyBasedOnGroup(GroupControl)
                    If bNTOnly.HasValue Then blnRet = bNTOnly.Value
                    'Removed by RHR for v-8.5.4.001 
                    'Dim GroupCat = db.tblUserGroups.Where(Function(x) x.UserGroupsControl = GroupControl).Select(Function(y) y.UserGroupsUGCControl).FirstOrDefault()
                    'If GroupCat = 2 OrElse GroupCat = 9 OrElse GroupCat = 8 Then blnRet = True
                End If
            Catch ex As SqlException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Adds the User to the Group.
    ''' Adds the user to the group, and adds security configuration based on the group (only the ones that are missing - also does not delete anything original that does not exist in the new group)
    ''' Note: Does not override all of it - does not delete from table so only adds anything that doesn't already exist
    ''' </summary>
    ''' <param name="GroupControl"></param>
    ''' <param name="UserSecurityControl"></param>
    ''' <remarks>
    ''' Modified By LVV on Added By LVV on 1/17/20 
    '''  Anytime a user is removed from WH/CSR/Carrier Group we have to uncheck the NexTrackOnly checkbox
    '''  Update the User's NEXTrackOnly Flag based on the new Group
    '''  Modified By LVV on 1/21/20
    '''   If the Group we are adding to is a Carrier or NEXTrackOnly Group hijack execution and actually call replace logic
    ''' </remarks>
    Public Sub AddUserToGroup(ByVal GroupControl As Integer, ByVal UserSecurityControl As Integer)
        'Modified By LVV on 1/21/20 - if the Group we are adding to is a Carrier or NEXTrackOnly Group hijack execution and actually call replace logic
        Dim strNTOnly = "0"
        Dim blnNTOnly = IsGroupNEXTrackOnly(GroupControl)
        If blnNTOnly = True Then strNTOnly = "1"
        If blnNTOnly Then
            ReplaceUserSecurityWithGroup(GroupControl, UserSecurityControl)
            Return
        End If
        'Update the GroupControl of the provided user in the user security table
        'Insert records into the tblFormSecurityXref for the user based on the group security config in tblFormSecurityGroupXref (only the ones that are missing) (also does not delete anything original that does not exist in the new group)
        'Do the same thing for procedures and reports
        Dim oCon As SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Core.Data.Query(ConnectionString)
        Dim Tran As SqlTransaction
        Dim blnSuccess As Boolean = False
        Try
            If Not oCon.State = ConnectionState.Open Then
                SaveAppError("Cannot Open Connecton to Database", Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
                Return
            End If
            Tran = oCon.BeginTransaction()
            'Modified By LVV on Added By LVV on 1/17/20 - Update the User's NEXTrackOnly Flag based on the new Group
            'First update the group control (and NEXTrackOnly flag - this way we don't mess up the transaction)
            Dim strSQL As String = "Update tblUserSecurity set UserUserGroupsControl = " & GroupControl.ToString & ", NEXTrackOnly = " & strNTOnly & " Where UserSecurityControl = " & UserSecurityControl.ToString
            If oQuery.executeSQLQuery(oCon, strSQL, 1, Tran) Then
                'update the form level security
                strSQL = "Insert Into dbo.tblFormSecurityXref (UserSecurityControl, FormControl) " _
                    & "Select UserSecurityControl, FormControl " _
                    & "From dbo.tblUserSecurity as a " _
                    & "Inner Join dbo.tblFormSecurityGroupXref as b on a.UserUserGroupsControl = b.UserGroupsControl " _
                    & "Where a.UserUserGroupsControl = " & GroupControl.ToString & " " _
                    & "AND a.UserSecurityControl = " & UserSecurityControl.ToString & " " _
                    & "AND b.FormControl not in (Select c.FormControl From dbo.tblFormSecurityXref as c Where c.UserSecurityControl = a.UserSecurityControl)"
                If oQuery.executeSQLQuery(oCon, strSQL, 1, Tran) Then
                    'update the procedure level security
                    strSQL = "Insert Into dbo.tblProcedureSecurityXref (UserSecurityControl, ProcedureControl) " _
                        & "Select UserSecurityControl, ProcedureControl " _
                        & "From dbo.tblUserSecurity as a " _
                        & "Inner Join dbo.tblProcedureSecurityGroupXref as b on a.UserUserGroupsControl = b.UserGroupsControl " _
                        & "Where a.UserUserGroupsControl = " & GroupControl.ToString & " " _
                        & "AND a.UserSecurityControl = " & UserSecurityControl.ToString & " " _
                        & "AND b.ProcedureControl not in (Select c.ProcedureControl From dbo.tblProcedureSecurityXref as c Where c.UserSecurityControl = a.UserSecurityControl)"
                    If oQuery.executeSQLQuery(oCon, strSQL, 1, Tran) Then
                        'update report level security
                        strSQL = "Insert Into dbo.tblReportSecurityXref (UserSecurityControl, ReportControl) " _
                            & "Select UserSecurityControl, ReportControl " _
                            & "From dbo.tblUserSecurity as a " _
                            & "Inner Join dbo.tblReportSecurityGroupXref as b on a.UserUserGroupsControl = b.UserGroupsControl " _
                            & "Where a.UserUserGroupsControl = " & GroupControl.ToString & " " _
                            & "AND a.UserSecurityControl = " & UserSecurityControl.ToString & " " _
                            & "AND b.ReportControl not in (Select c.ReportControl From dbo.tblReportSecurityXref as c Where c.UserSecurityControl = a.UserSecurityControl)"
                        oQuery.executeSQLQuery(oCon, strSQL, 1, Tran)
                    End If
                End If
            End If
            If Not Tran Is Nothing Then Tran.Commit()
            blnSuccess = True
        Catch ex As FaultException
            Throw
        Catch ex As Core.DatabaseRetryExceededException
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Core.DatabaseLogInException
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Core.DatabaseInvalidException
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As SqlException
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                If Not blnSuccess AndAlso Not Tran Is Nothing Then Tran.Rollback()
            Catch ex As Exception
                'do nothing
            End Try
            Try
                oQuery = Nothing
            Catch ex As Exception
                'do nothing
            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception
                'do nothing
            End Try
        End Try
    End Sub

    ''' <summary>
    ''' Adds all the Users to the Group.
    ''' Adds all the users to the group, and adds security configuration based on the group (only the ones that are missing - also does not delete anything original that does not exist in the new group)
    ''' Note: Does not override all of it - does not delete from table so only adds anything that doesn't already exist
    ''' </summary>
    ''' <param name="GroupControl"></param>
    ''' <remarks>
    '''  Modified By LVV on 1/21/20
    '''   If the Group we are adding to is a Carrier or NEXTrackOnly Group hijack execution and actually call replace logic
    '''   Anytime a user is removed from WH/CSR/Carrier Group we have to uncheck the NexTrackOnly checkbox
    '''   Update the User's NEXTrackOnly Flag based on the new Group
    ''' </remarks>
    Public Sub AddAllUsersToGroup(ByVal GroupControl As Integer)
        'Modified By LVV on 1/21/20 - if the Group we are adding to is a Carrier or NEXTrackOnly Group hijack execution and actually call replace logic
        Dim strNTOnly = "0"
        Dim blnNTOnly = IsGroupNEXTrackOnly(GroupControl)
        If blnNTOnly = True Then strNTOnly = "1"
        If blnNTOnly Then
            ReplaceAllUsersSecurityWithGroup(GroupControl)
            Return
        End If
        'Update the GroupControl of all the records in the user security table
        'Insert records into the tblFormSecurityXref for all users in the provided GroupControl (which is all of them now since we updated the table) based on the group security config in tblFormSecurityGroupXref (only the ones that are missing) (also does not delete anything original that does not exist in the new group)
        'Do the same thing for procedures and reports
        Dim oCon As SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Core.Data.Query(ConnectionString)
        Dim Tran As SqlTransaction
        Dim blnSuccess As Boolean = False
        Try
            If Not oCon.State = ConnectionState.Open Then
                SaveAppError("Cannot Open Connecton to Database", Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
                Return
            End If
            Tran = oCon.BeginTransaction()
            'Modified By LVV on Added By LVV on 1/17/20 - Update the User's NEXTrackOnly Flag based on the new Group
            'First update the group control (and NEXTrackOnly flag - this way we don't mess up the transaction)
            Dim strSQL As String = "Update tblUserSecurity set UserUserGroupsControl = " & GroupControl.ToString & ", NEXTrackOnly = " & strNTOnly ' Modified By LVV on 1/21/20 - reset everything to default NTOnly false (if it is true we don't get this far because we call ReplaceAllUsersSecurityWithGroup)
            If oQuery.executeSQLQuery(oCon, strSQL, 1, Tran) Then
                'update the form level security
                strSQL = "Insert Into dbo.tblFormSecurityXref (UserSecurityControl, FormControl) " _
                    & "Select UserSecurityControl, FormControl " _
                    & "From dbo.tblUserSecurity as a " _
                    & "Inner Join dbo.tblFormSecurityGroupXref as b on a.UserUserGroupsControl = b.UserGroupsControl " _
                    & "Where a.UserUserGroupsControl = " & GroupControl.ToString & " " _
                    & "AND b.FormControl not in (Select c.FormControl From dbo.tblFormSecurityXref as c Where c.UserSecurityControl = a.UserSecurityControl)"
                If oQuery.executeSQLQuery(oCon, strSQL, 1, Tran) Then
                    'update the procedure level security
                    strSQL = "Insert Into dbo.tblProcedureSecurityXref (UserSecurityControl, ProcedureControl) " _
                        & "Select UserSecurityControl, ProcedureControl " _
                        & "From dbo.tblUserSecurity as a " _
                        & "Inner Join dbo.tblProcedureSecurityGroupXref as b on a.UserUserGroupsControl = b.UserGroupsControl " _
                        & "Where a.UserUserGroupsControl = " & GroupControl.ToString & " " _
                        & "AND b.ProcedureControl not in (Select c.ProcedureControl From dbo.tblProcedureSecurityXref as c Where c.UserSecurityControl = a.UserSecurityControl)"
                    If oQuery.executeSQLQuery(oCon, strSQL, 1, Tran) Then
                        'update report level security
                        strSQL = "Insert Into dbo.tblReportSecurityXref (UserSecurityControl, ReportControl) " _
                            & "Select UserSecurityControl, ReportControl " _
                            & "From dbo.tblUserSecurity as a " _
                            & "Inner Join dbo.tblReportSecurityGroupXref as b on a.UserUserGroupsControl = b.UserGroupsControl " _
                            & "Where a.UserUserGroupsControl = " & GroupControl.ToString & " " _
                            & "AND b.ReportControl not in (Select c.ReportControl From dbo.tblReportSecurityXref as c Where c.UserSecurityControl = a.UserSecurityControl)"
                        oQuery.executeSQLQuery(oCon, strSQL, 1, Tran)
                    End If
                End If
            End If
            If Not Tran Is Nothing Then Tran.Commit()
            blnSuccess = True
        Catch ex As FaultException
            Throw
        Catch ex As Core.DatabaseRetryExceededException
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Core.DatabaseLogInException
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Core.DatabaseInvalidException
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As SqlException
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                If Not blnSuccess AndAlso Not Tran Is Nothing Then Tran.Rollback()
            Catch ex As Exception
                'do nothing
            End Try
            Try
                oQuery = Nothing
            Catch ex As Exception
                'do nothing
            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception
                'do nothing
            End Try
        End Try
    End Sub

    ''' <summary>
    ''' Deletes all the security settings associated with the users original group (param GroupControl) and sets the users new GroupControl to 0
    ''' Note: Does not override all of it - only deletes forms/procedures/reports associated with the group, not necessarily everything in the table for the user
    ''' </summary>
    ''' <param name="GroupControl"></param>
    ''' <param name="UserSecurityControl"></param>
    ''' <remarks>
    ''' Modified By LVV on Added By LVV on 1/21/20 
    '''  Anytime a user is removed from WH/CSR/Carrier Group we have to uncheck the NexTrackOnly checkbox
    '''  Update the User's NEXTrackOnly Flag based on the new Group
    ''' </remarks>
    Public Sub RemoveUserFromGroup(ByVal GroupControl As Integer, ByVal UserSecurityControl As Integer)
        'Delete records from the tblFormSecurityXref for the user based on the group security config in tblFormSecurityGroupXref (only the ones that are associated with the group)
        'Do the same thing for procedures and reports
        'Update the GroupControl of the provided user in the user security table
        Dim oCon As SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Core.Data.Query(ConnectionString)
        Dim Tran As SqlTransaction
        Dim blnSuccess As Boolean = False
        Try
            If Not oCon.State = ConnectionState.Open Then
                SaveAppError("Cannot Open Connecton to Database", Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
                Return
            End If
            Tran = oCon.BeginTransaction()
            'update the form level security
            Dim strSQL = "DELETE FROM dbo.tblFormSecurityXref WHERE UserSecurityControl = " & UserSecurityControl & " AND FormControl in (Select FormControl From dbo.tblFormSecurityGroupXref Where UserGroupsControl = " & GroupControl & ")"
            If oQuery.executeSQLQuery(oCon, strSQL, 1, Tran) Then
                'update the procedure level security
                strSQL = "DELETE FROM dbo.tblProcedureSecurityXref WHERE UserSecurityControl = " & UserSecurityControl & " AND ProcedureControl in (Select ProcedureControl From dbo.tblProcedureSecurityGroupXref Where UserGroupsControl = " & GroupControl & ")"
                If oQuery.executeSQLQuery(oCon, strSQL, 1, Tran) Then
                    'update report level security
                    strSQL = "DELETE FROM dbo.tblReportSecurityXref WHERE UserSecurityControl = " & UserSecurityControl & " AND ReportControl in (Select ReportControl From dbo.tblReportSecurityGroupXref Where UserGroupsControl = " & GroupControl & ")"
                    If oQuery.executeSQLQuery(oCon, strSQL, 1, Tran) Then
                        'Modified By LVV on Added By LVV on 1/17/20 - Update the User's NEXTrackOnly Flag based on the new Group
                        Dim strNTOnly = "0"
                        Dim blnNTOnly = IsGroupNEXTrackOnly(0)
                        If blnNTOnly = True Then strNTOnly = "1"
                        'Finally update the group control (and NEXTrackOnly flag - this way we don't mess up the transaction)
                        strSQL = "Update tblUserSecurity set UserUserGroupsControl = 0, NEXTrackOnly = " & strNTOnly & " Where UserSecurityControl = " & UserSecurityControl.ToString
                        oQuery.executeSQLQuery(oCon, strSQL, 1, Tran)
                    End If
                End If
            End If
            If Not Tran Is Nothing Then Tran.Commit()
            blnSuccess = True
        Catch ex As FaultException
            Throw
        Catch ex As Core.DatabaseRetryExceededException
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Core.DatabaseLogInException
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Core.DatabaseInvalidException
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As SqlException
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                If Not blnSuccess AndAlso Not Tran Is Nothing Then Tran.Rollback()
            Catch ex As Exception
                'do nothing
            End Try
            Try
                oQuery = Nothing
            Catch ex As Exception
                'do nothing
            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception
                'do nothing
            End Try
        End Try
    End Sub

    ''' <summary>
    ''' Removes All Users From Group.
    ''' Note: Does not override all of it - only deletes forms/procedures/reports associated with the group, not necessarily everything in the table for the users
    ''' </summary>
    ''' <param name="GroupControl"></param>
    ''' <remarks>
    ''' Modified By LVV on 1/21/20 
    '''  Anytime a user is removed from WH/CSR/Carrier Group we have to uncheck the NexTrackOnly checkbox
    '''  Update the User's NEXTrackOnly Flag based on the new Group
    '''  (also fixed bug with missing Where clause)
    ''' </remarks>
    Public Sub RemoveAllUsersFromGroup(ByVal GroupControl As Integer)
        'Delete records from the tblFormSecurityXref for the users in the group based on the group security config in tblFormSecurityGroupXref (only the ones that are associated with the group)
        'Do the same thing for procedures and reports
        'Update the GroupControl of the provided user in the user security table
        Dim oCon As SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Core.Data.Query(ConnectionString)
        Dim Tran As SqlTransaction
        Dim blnSuccess As Boolean = False
        Try
            If Not oCon.State = ConnectionState.Open Then
                SaveAppError("Cannot Open Connecton to Database", Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
                Return
            End If
            Tran = oCon.BeginTransaction()
            'update the form level security
            Dim strSQL = "DELETE FROM dbo.tblFormSecurityXref " _
                & "WHERE UserSecurityControl in (Select UserSecurityControl From dbo.tblUserSecurity Where UserUserGroupsControl = " & GroupControl & " ) " _
                & "AND FormControl in (Select FormControl From dbo.tblFormSecurityGroupXref Where UserGroupsControl = " & GroupControl & ")"
            If oQuery.executeSQLQuery(oCon, strSQL, 1, Tran) Then
                'update the procedure level security
                strSQL = "DELETE FROM dbo.tblProcedureSecurityXref " _
                    & "WHERE UserSecurityControl in (Select UserSecurityControl From dbo.tblUserSecurity Where UserUserGroupsControl = " & GroupControl & " ) " _
                    & "AND ProcedureControl in (Select ProcedureControl From dbo.tblProcedureSecurityGroupXref Where UserGroupsControl = " & GroupControl & ")"
                If oQuery.executeSQLQuery(oCon, strSQL, 1, Tran) Then
                    'update report level security
                    strSQL = "DELETE FROM dbo.tblReportSecurityXref " _
                        & "WHERE UserSecurityControl in (Select UserSecurityControl From dbo.tblUserSecurity Where UserUserGroupsControl = " & GroupControl & " ) " _
                        & "AND ReportControl in (Select ReportControl From dbo.tblReportSecurityGroupXref Where UserGroupsControl = " & GroupControl & ")"
                    If oQuery.executeSQLQuery(oCon, strSQL, 1, Tran) Then
                        'Modified By LVV on Added By LVV on 1/17/20 - Update the User's NEXTrackOnly Flag based on the new Group
                        Dim strNTOnly = "0"
                        Dim blnNTOnly = IsGroupNEXTrackOnly(0)
                        If blnNTOnly = True Then strNTOnly = "1"
                        'Finally update the group control (and NEXTrackOnly flag - this way we don't mess up the transaction)
                        strSQL = "Update tblUserSecurity set UserUserGroupsControl = 0, NEXTrackOnly = " & strNTOnly & " Where UserSecurityControl in (Select UserSecurityControl From dbo.tblUserSecurity Where UserUserGroupsControl = " & GroupControl & " )" 'Modified By LVV On 1/21/20 - fixed bug with missing Where clause
                        oQuery.executeSQLQuery(oCon, strSQL, 1, Tran)
                    End If
                End If
            End If
            If Not Tran Is Nothing Then Tran.Commit()
            blnSuccess = True
        Catch ex As FaultException
            Throw
        Catch ex As Core.DatabaseRetryExceededException
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Core.DatabaseLogInException
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Core.DatabaseInvalidException
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As SqlException
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                If Not blnSuccess AndAlso Not Tran Is Nothing Then Tran.Rollback()
            Catch ex As Exception
                'do nothing
            End Try
            Try
                oQuery = Nothing
            Catch ex As Exception
                'do nothing
            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception
                'do nothing
            End Try
        End Try
    End Sub

    ''' <summary>
    ''' Replace Security configuration of the User With the Security configuration of the Group
    ''' Adds the user to the group, deletes all previous security configuration and replaces it with security configuration based on the group
    ''' </summary>
    ''' <param name="GroupControl"></param>
    ''' <param name="UserSecurityControl"></param>
    ''' <remarks>
    ''' Modified By LVV on Added By LVV on 1/17/20 
    '''  Anytime a user is added to WH/CSR/Carrier Group we have to check the NexTrackOnly checkbox and if removed from either group uncheck it
    '''  Update the User's NEXTrackOnly Flag based on the new Group
    ''' </remarks>
    Public Sub ReplaceUserSecurityWithGroup(ByVal GroupControl As Integer, ByVal UserSecurityControl As Integer)
        'Update the GroupControl of the provided user in the user security table
        'Delete all the records in tblFormSecurityXref for the provided user
        'Insert records into the tblFormSecurityXref for the user based on the group security config in tblFormSecurityGroupXref
        'Do the same thing for procedures and reports
        Dim oCon As SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Core.Data.Query(ConnectionString)
        Dim Tran As SqlTransaction
        Dim blnSuccess As Boolean = False
        Try
            If Not oCon.State = ConnectionState.Open Then
                SaveAppError("Cannot Open Connecton to Database", Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
                Return
            End If
            Tran = oCon.BeginTransaction()
            'Modified By LVV on Added By LVV on 1/17/20 - Update the User's NEXTrackOnly Flag based on the new Group
            Dim strNTOnly = "0"
            Dim blnNTOnly = IsGroupNEXTrackOnly(GroupControl)
            If blnNTOnly = True Then strNTOnly = "1"
            'First update the group control (and NEXTrackOnly flag - this way we don't mess up the transaction)
            Dim strSQL As String = "Update tblUserSecurity set UserUserGroupsControl = " & GroupControl.ToString & ", NEXTrackOnly = " & strNTOnly & " Where UserSecurityControl = " & UserSecurityControl.ToString
            If oQuery.executeSQLQuery(oCon, strSQL, 1, Tran) Then
                'update the form level security
                strSQL = "Delete From dbo.tblFormSecurityXref Where UserSecurityControl = " & UserSecurityControl & " " _
                    & vbCrLf _
                    & "Insert Into dbo.tblFormSecurityXref (UserSecurityControl, FormControl) " _
                    & "Select a.UserSecurityControl, b.FormControl " _
                    & "From dbo.tblUserSecurity as a " _
                    & "Inner Join dbo.tblFormSecurityGroupXref as b on a.UserUserGroupsControl = b.UserGroupsControl " _
                    & "Where a.UserUserGroupsControl = " & GroupControl.ToString & " AND a.UserSecurityControl = " & UserSecurityControl.ToString
                If oQuery.executeSQLQuery(oCon, strSQL, 1, Tran) Then
                    'update the procedure level security
                    strSQL = "Delete From dbo.tblProcedureSecurityXref Where UserSecurityControl = " & UserSecurityControl & " " _
                        & vbCrLf _
                        & "Insert Into dbo.tblProcedureSecurityXref (UserSecurityControl, ProcedureControl) " _
                        & "Select a.UserSecurityControl, b.ProcedureControl " _
                        & "From dbo.tblUserSecurity as a " _
                        & "Inner Join dbo.tblProcedureSecurityGroupXref as b on a.UserUserGroupsControl = b.UserGroupsControl " _
                        & "Where a.UserUserGroupsControl = " & GroupControl.ToString & " AND a.UserSecurityControl = " & UserSecurityControl.ToString
                    If oQuery.executeSQLQuery(oCon, strSQL, 1, Tran) Then
                        'update report level security
                        strSQL = "Delete From dbo.tblReportSecurityXref Where UserSecurityControl = " & UserSecurityControl & " " _
                            & vbCrLf _
                            & "Insert Into dbo.tblReportSecurityXref (UserSecurityControl, ReportControl) " _
                            & "Select a.UserSecurityControl, b.ReportControl " _
                            & "From dbo.tblUserSecurity as a " _
                            & "Inner Join dbo.tblReportSecurityGroupXref as b on a.UserUserGroupsControl = b.UserGroupsControl " _
                            & "Where a.UserUserGroupsControl = " & GroupControl.ToString & " AND a.UserSecurityControl = " & UserSecurityControl.ToString
                        oQuery.executeSQLQuery(oCon, strSQL, 1, Tran)
                    End If
                End If
            End If
            If Not Tran Is Nothing Then Tran.Commit()
            blnSuccess = True
        Catch ex As FaultException
            Throw
        Catch ex As Core.DatabaseRetryExceededException
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Core.DatabaseLogInException
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Core.DatabaseInvalidException
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As SqlException
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                If Not blnSuccess AndAlso Not Tran Is Nothing Then Tran.Rollback()
            Catch ex As Exception
                'do nothing
            End Try
            Try
                oQuery = Nothing
            Catch ex As Exception
                'do nothing
            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception
                'do nothing
            End Try
        End Try
    End Sub

    ''' <summary>
    ''' Replaces All Users Security With Group.
    ''' Adds all the users to the group, deletes all previous security configuration for each user and replaces it with security configuration based on the group
    ''' </summary>
    ''' <param name="GroupControl"></param>
    ''' <remarks>
    ''' Modified By LVV on Added By LVV on 1/17/20 
    '''  Fixed bug - previously never updated the group control in the user security table so this method didn't actually do anything
    '''  Anytime a user is added to WH/CSR/Carrier Group we have to check the NexTrackOnly checkbox and if removed from either group uncheck it
    '''  Update the User's NEXTrackOnly Flag based on the new Group
    ''' </remarks>
    Public Sub ReplaceAllUsersSecurityWithGroup(ByVal GroupControl As Integer)
        'Update the GroupControl of all the records in the user security table
        'Delete all the records in tblFormSecurityXref for the provided GroupControl (which is all of them now since we updated the table)
        'Insert records into the tblFormSecurityXref for all users in the provided GroupControl (which is all of them now since we updated the table) based on the group security config in tblFormSecurityGroupXref
        'Do the same thing for procedures and reports
        Dim oCon As SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Core.Data.Query(ConnectionString)
        Dim Tran As SqlTransaction
        Dim blnSuccess As Boolean = False
        Try
            If Not oCon.State = ConnectionState.Open Then
                SaveAppError("Cannot Open Connecton to Database", Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
                Return
            End If
            Tran = oCon.BeginTransaction()
            'Modified By LVV on Added By LVV on 1/17/20 - Fixed bug - previously never updated the group control in the user security table so this method didn't actually do anything
            'Modified By LVV on Added By LVV on 1/17/20 - Update the User's NEXTrackOnly Flag based on the new Group
            Dim strNTOnly = "0"
            Dim blnNTOnly = IsGroupNEXTrackOnly(GroupControl)
            If blnNTOnly = True Then strNTOnly = "1"
            'First update the group control (and NEXTrackOnly flag)
            Dim strSQL As String = "Update tblUserSecurity set UserUserGroupsControl = " & GroupControl.ToString & ", NEXTrackOnly = " & strNTOnly
            If oQuery.executeSQLQuery(oCon, strSQL, 1, Tran) Then
                'update the form level security
                strSQL = "Delete From dbo.tblFormSecurityXref Where UserSecurityControl in (Select UserSecurityControl From dbo.tblUserSecurity Where UserUserGroupsControl = " & GroupControl.ToString & ") " _
                    & vbCrLf _
                    & "Insert Into dbo.tblFormSecurityXref (UserSecurityControl, FormControl) " _
                    & "Select a.UserSecurityControl, b.FormControl " _
                    & "From dbo.tblUserSecurity as a " _
                    & "Inner Join dbo.tblFormSecurityGroupXref as b on a.UserUserGroupsControl = b.UserGroupsControl " _
                    & "Where a.UserUserGroupsControl = " & GroupControl.ToString
                If oQuery.executeSQLQuery(oCon, strSQL, 1, Tran) Then
                    'update the procedure level security
                    strSQL = "Delete From dbo.tblProcedureSecurityXref Where UserSecurityControl in (Select UserSecurityControl From dbo.tblUserSecurity Where UserUserGroupsControl = " & GroupControl.ToString & ") " _
                        & vbCrLf _
                        & "Insert Into dbo.tblProcedureSecurityXref (UserSecurityControl, ProcedureControl) " _
                        & "Select a.UserSecurityControl, b.ProcedureControl " _
                        & "From dbo.tblUserSecurity as a " _
                        & "Inner Join dbo.tblProcedureSecurityGroupXref as b on a.UserUserGroupsControl = b.UserGroupsControl " _
                        & "Where a.UserUserGroupsControl = " & GroupControl.ToString
                    If oQuery.executeSQLQuery(oCon, strSQL, 1, Tran) Then
                        'update report level security
                        strSQL = "Delete From dbo.tblReportSecurityXref Where UserSecurityControl in (Select UserSecurityControl From dbo.tblUserSecurity Where UserUserGroupsControl = " & GroupControl.ToString & ") " _
                            & vbCrLf _
                            & "Insert Into dbo.tblReportSecurityXref (UserSecurityControl, ReportControl) " _
                            & "Select a.UserSecurityControl, b.ReportControl " _
                            & "From dbo.tblUserSecurity as a " _
                            & "Inner Join dbo.tblReportSecurityGroupXref as b on a.UserUserGroupsControl = b.UserGroupsControl " _
                            & "Where a.UserUserGroupsControl = " & GroupControl.ToString
                        oQuery.executeSQLQuery(oCon, strSQL, 1, Tran)
                    End If
                End If
            End If
            If Not Tran Is Nothing Then Tran.Commit()
            blnSuccess = True
        Catch ex As FaultException
            Throw
        Catch ex As Core.DatabaseRetryExceededException
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Core.DatabaseLogInException
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Core.DatabaseInvalidException
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As SqlException
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                If Not blnSuccess AndAlso Not Tran Is Nothing Then Tran.Rollback()
            Catch ex As Exception
                'do nothing
            End Try
            Try
                oQuery = Nothing
            Catch ex As Exception
                'do nothing
            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception
                'do nothing
            End Try
        End Try
    End Sub

    Public Sub ReplaceUserFormSecurityWithGroup(ByVal GroupControl As Integer, ByVal UserSecurityControl As Integer)
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim Tran As System.Data.SqlClient.SqlTransaction
        Dim blnSuccess As Boolean = False
        Try
            If Not oCon.State = ConnectionState.Open Then
                Utilities.SaveAppError("Cannot Open Connecton to Database", Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
                Return
            End If
            Tran = oCon.BeginTransaction()
            'First update the group control
            Dim strSQL As String = "Update tblUserSecurity set UserUserGroupsControl = " & GroupControl.ToString _
                                   & " Where UserSecurityControl = " & UserSecurityControl.ToString

            If oQuery.executeSQLQuery(oCon, strSQL, 1, Tran) Then
                'update the form level security
                strSQL = "Delete From dbo.tblFormSecurityXref Where UserSecurityControl = " & UserSecurityControl _
                    & vbCrLf _
                    & " insert into  dbo.tblFormSecurityXref  (UserSecurityControl, FormControl) " _
                    & " Select a.UserSecurityControl,b.FormControl " _
                    & " FROM dbo.tblUserSecurity as a " _
                    & " Inner Join dbo.tblFormSecurityGroupXref as b " _
                    & " on a.UserUserGroupsControl = b.UserGroupsControl " _
                    & " Where a.UserUserGroupsControl = " & GroupControl.ToString _
                    & " AND a.UserSecurityControl = " & UserSecurityControl.ToString
                oQuery.executeSQLQuery(oCon, strSQL, 1, Tran)
            End If
            If Not Tran Is Nothing Then Tran.Commit()
            blnSuccess = True
        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                If Not blnSuccess AndAlso Not Tran Is Nothing Then Tran.Rollback()
            Catch ex As Exception

            End Try
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try

    End Sub

    Public Sub ReplaceUserProcedureSecurityWithGroup(ByVal GroupControl As Integer, ByVal UserSecurityControl As Integer)
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim Tran As System.Data.SqlClient.SqlTransaction
        Dim blnSuccess As Boolean = False
        Try
            If Not oCon.State = ConnectionState.Open Then
                Utilities.SaveAppError("Cannot Open Connecton to Database", Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
                Return
            End If
            Tran = oCon.BeginTransaction()
            'First update the group control
            Dim strSQL As String = "Update tblUserSecurity set UserUserGroupsControl = " & GroupControl.ToString _
                                   & " Where UserSecurityControl = " & UserSecurityControl.ToString

            If oQuery.executeSQLQuery(oCon, strSQL, 1, Tran) Then
                'update the procedure level security
                strSQL = "Delete From dbo.tblProcedureSecurityXref Where UserSecurityControl = " & UserSecurityControl _
                    & vbCrLf _
                    & " insert into  dbo.tblProcedureSecurityXref  (UserSecurityControl, ProcedureControl) " _
                    & " Select a.UserSecurityControl,b.ProcedureControl " _
                    & " FROM dbo.tblUserSecurity as a " _
                    & " Inner Join dbo.tblProcedureSecurityGroupXref as b " _
                    & " on a.UserUserGroupsControl = b.UserGroupsControl " _
                    & " Where a.UserUserGroupsControl = " & GroupControl.ToString _
                    & " AND a.UserSecurityControl = " & UserSecurityControl.ToString
                oQuery.executeSQLQuery(oCon, strSQL, 1, Tran)
            End If
            If Not Tran Is Nothing Then Tran.Commit()
            blnSuccess = True
        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                If Not blnSuccess AndAlso Not Tran Is Nothing Then Tran.Rollback()
            Catch ex As Exception

            End Try
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try

    End Sub

    Public Sub ReplaceUserReportSecurityWithGroup(ByVal GroupControl As Integer, ByVal UserSecurityControl As Integer)
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim Tran As System.Data.SqlClient.SqlTransaction
        Dim blnSuccess As Boolean = False
        Try
            If Not oCon.State = ConnectionState.Open Then
                Utilities.SaveAppError("Cannot Open Connecton to Database", Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
                Return
            End If
            Tran = oCon.BeginTransaction()
            'First update the group control
            Dim strSQL As String = "Update tblUserSecurity set UserUserGroupsControl = " & GroupControl.ToString _
                                   & " Where UserSecurityControl = " & UserSecurityControl.ToString

            If oQuery.executeSQLQuery(oCon, strSQL, 1, Tran) Then
                'update report level security
                strSQL = "Delete From dbo.tblReportSecurityXref Where UserSecurityControl = " & UserSecurityControl _
                    & vbCrLf _
                    & " insert into  dbo.tblReportSecurityXref  (UserSecurityControl, ReportControl) " _
                    & " Select UserSecurityControl,ReportControl " _
                    & " FROM dbo.tblUserSecurity as a " _
                    & " Inner Join dbo.tblReportSecurityGroupXref as b " _
                    & " on a.UserUserGroupsControl = b.UserGroupsControl " _
                    & " Where a.UserUserGroupsControl = " & GroupControl.ToString _
                    & " AND a.UserSecurityControl = " & UserSecurityControl.ToString
                oQuery.executeSQLQuery(oCon, strSQL, 1, Tran)
            End If
            If Not Tran Is Nothing Then Tran.Commit()
            blnSuccess = True
        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                If Not blnSuccess AndAlso Not Tran Is Nothing Then Tran.Rollback()
            Catch ex As Exception

            End Try
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try

    End Sub

    Public Sub ReplaceAllUsersFormSecurityWithGroup(ByVal GroupControl As Integer)
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim Tran As System.Data.SqlClient.SqlTransaction
        Dim blnSuccess As Boolean = False
        Try
            If Not oCon.State = ConnectionState.Open Then
                Utilities.SaveAppError("Cannot Open Connecton to Database", Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
                Return
            End If
            Tran = oCon.BeginTransaction()

            'update the form level security
            Dim strSQL As String = "Delete From dbo.tblFormSecurityXref " _
                & " Where UserSecurityControl in " _
                & " (Select UserSecurityControl From dbo.tblUserSecurity Where UserUserGroupsControl = " & GroupControl.ToString & ") " _
                & vbCrLf _
                & " insert into  dbo.tblFormSecurityXref  (UserSecurityControl, FormControl) " _
                & " Select a.UserSecurityControl,b.FormControl " _
                & " FROM dbo.tblUserSecurity as a " _
                & " Inner Join dbo.tblFormSecurityGroupXref as b " _
                & " on a.UserUserGroupsControl = b.UserGroupsControl " _
                & " Where a.UserUserGroupsControl = " & GroupControl.ToString
            oQuery.executeSQLQuery(oCon, strSQL, 1, Tran)
            If Not Tran Is Nothing Then Tran.Commit()
            blnSuccess = True
        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                If Not blnSuccess AndAlso Not Tran Is Nothing Then Tran.Rollback()
            Catch ex As Exception

            End Try
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try

    End Sub

    Public Sub ReplaceAllUsersProcedureSecurityWithGroup(ByVal GroupControl As Integer)
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim Tran As System.Data.SqlClient.SqlTransaction
        Dim blnSuccess As Boolean = False
        Try
            If Not oCon.State = ConnectionState.Open Then
                Utilities.SaveAppError("Cannot Open Connecton to Database", Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
                Return
            End If
            Tran = oCon.BeginTransaction()

            'update the form level security
            Dim strSQL As String = "Delete From dbo.tblProcedureSecurityXref Where UserSecurityControl in " _
                    & " (Select UserSecurityControl From dbo.tblUserSecurity Where UserUserGroupsControl = " & GroupControl.ToString & ") " _
                    & vbCrLf _
                    & " insert into  dbo.tblProcedureSecurityXref  (UserSecurityControl, ProcedureControl) " _
                    & " Select a.UserSecurityControl,b.ProcedureControl " _
                    & " FROM dbo.tblUserSecurity as a " _
                    & " Inner Join dbo.tblProcedureSecurityGroupXref as b " _
                    & " on a.UserUserGroupsControl = b.UserGroupsControl " _
                    & " Where a.UserUserGroupsControl = " & GroupControl.ToString
            oQuery.executeSQLQuery(oCon, strSQL, 1, Tran)
            If Not Tran Is Nothing Then Tran.Commit()
            blnSuccess = True
        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                If Not blnSuccess AndAlso Not Tran Is Nothing Then Tran.Rollback()
            Catch ex As Exception

            End Try
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try

    End Sub

    Public Sub ReplaceAllUsersReportSecurityWithGroup(ByVal GroupControl As Integer)
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim Tran As System.Data.SqlClient.SqlTransaction
        Dim blnSuccess As Boolean = False
        Try
            If Not oCon.State = ConnectionState.Open Then
                Utilities.SaveAppError("Cannot Open Connecton to Database", Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
                Return
            End If
            Tran = oCon.BeginTransaction()

            'update the form level security
            Dim strSQL As String = "Delete From dbo.tblReportSecurityXref Where UserSecurityControl  " _
                        & " (Select UserSecurityControl From dbo.tblUserSecurity Where UserUserGroupsControl = " & GroupControl.ToString & ") " _
                        & vbCrLf _
                        & " insert into  dbo.tblReportSecurityXref  (UserSecurityControl, ReportControl) " _
                        & " Select UserSecurityControl,ReportControl " _
                        & " FROM dbo.tblUserSecurity as a " _
                        & " Inner Join dbo.tblReportSecurityGroupXref as b " _
                        & " on a.UserUserGroupsControl = b.UserGroupsControl " _
                        & " Where a.UserUserGroupsControl = " & GroupControl.ToString
            oQuery.executeSQLQuery(oCon, strSQL, 1, Tran)

            If Not Tran Is Nothing Then Tran.Commit()
            blnSuccess = True
        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                If Not blnSuccess AndAlso Not Tran Is Nothing Then Tran.Rollback()
            Catch ex As Exception

            End Try
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try

    End Sub

    Public Sub ReplaceGroupFormSecurityWithUser(ByVal GroupControl As Integer, ByVal UserSecurityControl As Integer)
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim Tran As System.Data.SqlClient.SqlTransaction
        Dim blnSuccess As Boolean = False
        Try
            If Not oCon.State = ConnectionState.Open Then
                Utilities.SaveAppError("Cannot Open Connecton to Database", Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
                Return
            End If
            Tran = oCon.BeginTransaction()

            'update Form level security
            Dim strSQL As String = "Delete From dbo.tblFormSecurityGroupXref  Where UserGroupsControl = " & GroupControl _
                & vbCrLf _
                & " insert into  dbo.tblFormSecurityGroupXref  (UserGroupsControl, FormControl) " _
                & " Select " & GroupControl & " ,FormControl FROM dbo.tblFormSecurityXref Where UserSecurityControl = " & UserSecurityControl
            oQuery.executeSQLQuery(oCon, strSQL, 1, Tran)
            If Not Tran Is Nothing Then Tran.Commit()
            blnSuccess = True
        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                If Not blnSuccess AndAlso Not Tran Is Nothing Then Tran.Rollback()
            Catch ex As Exception

            End Try
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try

    End Sub

    Public Sub ReplaceGroupProcedureSecurityWithUser(ByVal GroupControl As Integer, ByVal UserSecurityControl As Integer)
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim Tran As System.Data.SqlClient.SqlTransaction
        Dim blnSuccess As Boolean = False
        Try
            If Not oCon.State = ConnectionState.Open Then
                Utilities.SaveAppError("Cannot Open Connecton to Database", Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
                Return
            End If
            Tran = oCon.BeginTransaction()

            'update Procedure level security
            Dim strSQL As String = "Delete From dbo.tblProcedureSecurityGroupXref  Where UserGroupsControl = " & GroupControl _
                & vbCrLf _
                & " insert into  dbo.tblProcedureSecurityGroupXref  (UserGroupsControl, ProcedureControl) " _
                & " Select " & GroupControl & " ,ProcedureControl FROM dbo.tblProcedureSecurityXref Where UserSecurityControl = " & UserSecurityControl
            oQuery.executeSQLQuery(oCon, strSQL, 1, Tran)
            If Not Tran Is Nothing Then Tran.Commit()
            blnSuccess = True
        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                If Not blnSuccess AndAlso Not Tran Is Nothing Then Tran.Rollback()
            Catch ex As Exception

            End Try
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try

    End Sub

    Public Sub ReplaceGroupReportSecurityWithUser(ByVal GroupControl As Integer, ByVal UserSecurityControl As Integer)
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim Tran As System.Data.SqlClient.SqlTransaction
        Dim blnSuccess As Boolean = False
        Try
            If Not oCon.State = ConnectionState.Open Then
                Utilities.SaveAppError("Cannot Open Connecton to Database", Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
                Return
            End If
            Tran = oCon.BeginTransaction()

            'update report level security
            Dim strSQL As String = "Delete From dbo.tblReportSecurityGroupXref  Where UserGroupsControl = " & GroupControl _
                & vbCrLf _
                & " insert into  dbo.tblReportSecurityGroupXref  (UserGroupsControl, ReportControl) " _
                & " Select " & GroupControl & " ,ReportControl FROM dbo.tblReportSecurityXref Where UserSecurityControl = " & UserSecurityControl
            oQuery.executeSQLQuery(oCon, strSQL, 1, Tran)
            If Not Tran Is Nothing Then Tran.Commit()
            blnSuccess = True
        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                If Not blnSuccess AndAlso Not Tran Is Nothing Then Tran.Rollback()
            Catch ex As Exception

            End Try
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try

    End Sub

    Public Sub ReplaceGroupSecurityWithUser(ByVal GroupControl As Integer, ByVal UserSecurityControl As Integer)
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim Tran As System.Data.SqlClient.SqlTransaction
        Dim blnSuccess As Boolean = False
        Try
            If Not oCon.State = ConnectionState.Open Then
                Utilities.SaveAppError("Cannot Open Connecton to Database", Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
                Return
            End If
            Tran = oCon.BeginTransaction()

            'update the form level security
            Dim strSQL As String = "Delete From dbo.tblFormSecurityGroupXref  Where UserGroupsControl = " & GroupControl _
                & vbCrLf _
                & " insert into  dbo.tblFormSecurityGroupXref  (UserGroupsControl, FormControl) " _
                & " Select " & GroupControl & " ,FormControl FROM dbo.tblFormSecurityXref Where UserSecurityControl = " & UserSecurityControl

            If oQuery.executeSQLQuery(oCon, strSQL, 1, Tran) Then
                'update the procedure level security
                strSQL = "Delete From dbo.tblProcedureSecurityGroupXref  Where UserGroupsControl = " & GroupControl _
                    & vbCrLf _
                    & " insert into  dbo.tblProcedureSecurityGroupXref  (UserGroupsControl, ProcedureControl) " _
                    & " Select " & GroupControl & " ,ProcedureControl FROM dbo.tblProcedureSecurityXref Where UserSecurityControl = " & UserSecurityControl
                If oQuery.executeSQLQuery(oCon, strSQL, 1, Tran) Then
                    'update report level security
                    strSQL = "Delete From dbo.tblReportSecurityGroupXref  Where UserGroupsControl = " & GroupControl _
                        & vbCrLf _
                        & " insert into  dbo.tblReportSecurityGroupXref  (UserGroupsControl, ReportControl) " _
                        & " Select " & GroupControl & " ,ReportControl FROM dbo.tblReportSecurityXref Where UserSecurityControl = " & UserSecurityControl
                    oQuery.executeSQLQuery(oCon, strSQL, 1, Tran)
                End If
            End If
            If Not Tran Is Nothing Then Tran.Commit()
            blnSuccess = True
        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                If Not blnSuccess AndAlso Not Tran Is Nothing Then Tran.Rollback()
            Catch ex As Exception

            End Try
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try

    End Sub

#End Region

#Region "tblUserAdmin"

    'TODO - We can make this much more efficient with a linq query
    Public Function IsNEXTrackOnly(ByVal UserName As String) As Boolean
        Dim tblUserSecurity As DTO.tblUserSecurity = Me.GettblUserSecurityByUserName(UserName)
        If tblUserSecurity Is Nothing Then Return False
        Return tblUserSecurity.NEXTrackOnly
    End Function

    Public Function GettblUserAdmins() As DTO.tblUserAdmin()
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim oDLO As New DataLoadOptions
                oDLO.LoadWith(Of LTS.tblUserAdmin)(Function(t As LTS.tblUserAdmin) t.CompRefSecurity)
                oDLO.LoadWith(Of LTS.tblUserAdmin)(Function(t As LTS.tblUserAdmin) t.tblUserSecurity)
                db.LoadOptions = oDLO
                'if there are no null values to deal with we can build the array directly
                Dim tblUserAdmins() As DTO.tblUserAdmin = (
                    From t In db.tblUserAdmins
                    Order By t.tblUserSecurity.UserName
                    Select New DTO.tblUserAdmin With {.UserAdminControl = t.UserAdminControl,
                                                      .UserSecurityControl = t.UserSecurityControl,
                                                      .UserAdminCompControl = t.UserAdminCompControl,
                                                      .UserName = t.tblUserSecurity.UserName,
                                                      .CompName = t.CompRefSecurity.CompName,
                                                      .CompNumber = If(t.CompRefSecurity.CompNumber.HasValue, t.CompRefSecurity.CompNumber, 0),
                                                      .UserAdminUpdated = t.UserAdminUpdated.ToArray()}).ToArray()
                Return tblUserAdmins
            Catch ex As SqlException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
            Return Nothing
        End Using
    End Function

    Public Function GettblUserAdmin(ByVal Control As Integer) As DTO.tblUserAdmin
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim oDLO As New DataLoadOptions
                oDLO.LoadWith(Of LTS.tblUserAdmin)(Function(t As LTS.tblUserAdmin) t.CompRefSecurity)
                oDLO.LoadWith(Of LTS.tblUserAdmin)(Function(t As LTS.tblUserAdmin) t.tblUserSecurity)
                db.LoadOptions = oDLO
                'if there are no null values to deal with we can build the array directly
                Dim tblUserAdmin As DTO.tblUserAdmin = (
                From t In db.tblUserAdmins
                Where t.UserAdminControl = Control
                Select New DTO.tblUserAdmin With {.UserAdminControl = t.UserAdminControl,
                                                      .UserSecurityControl = t.UserSecurityControl,
                                                      .UserAdminCompControl = t.UserAdminCompControl,
                                                      .UserName = t.tblUserSecurity.UserName,
                                                      .CompName = t.CompRefSecurity.CompName,
                                                      .CompNumber = If(t.CompRefSecurity.CompNumber.HasValue, t.CompRefSecurity.CompNumber, 0),
                                                      .UserAdminUpdated = t.UserAdminUpdated.ToArray()}).Single
                Return tblUserAdmin
            Catch ex As SqlException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
            Return Nothing
        End Using
    End Function

    Public Function GettblUserAdminByUser(ByVal UserSecurityControl As Integer) As DTO.tblUserAdmin
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim oDLO As New DataLoadOptions
                oDLO.LoadWith(Of LTS.tblUserAdmin)(Function(t As LTS.tblUserAdmin) t.CompRefSecurity)
                oDLO.LoadWith(Of LTS.tblUserAdmin)(Function(t As LTS.tblUserAdmin) t.tblUserSecurity)
                db.LoadOptions = oDLO
                'if there are no null values to deal with we can build the array directly
                Dim tblUserAdmin As DTO.tblUserAdmin = (
                From t In db.tblUserAdmins
                Where t.UserSecurityControl = UserSecurityControl
                Order By t.UserAdminControl Descending
                Select New DTO.tblUserAdmin With {.UserAdminControl = t.UserAdminControl,
                                                      .UserSecurityControl = t.UserSecurityControl,
                                                      .UserAdminCompControl = t.UserAdminCompControl,
                                                      .UserName = t.tblUserSecurity.UserName,
                                                      .CompName = t.CompRefSecurity.CompName,
                                                      .CompNumber = If(t.CompRefSecurity.CompNumber.HasValue, t.CompRefSecurity.CompNumber, 0),
                                                      .UserAdminUpdated = t.UserAdminUpdated.ToArray()}).FirstOrDefault
                Return tblUserAdmin
            Catch ex As SqlException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' NOTE: Replaced by NGLUserAdminData.GetvUserAdminsByUser() below for 365 (LEUsers page)
    ''' Hopefully one day we can phase this out completely
    ''' </summary>
    ''' <param name="UserSecurityControl"></param>
    ''' <returns></returns>
    Public Function GettblUserAdminsByUser(ByVal UserSecurityControl As Integer) As DTO.tblUserAdmin()
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'db.Log = New DebugTextWriter
                Dim oDLO As New DataLoadOptions
                oDLO.LoadWith(Of LTS.tblUserAdmin)(Function(t As LTS.tblUserAdmin) t.CompRefSecurity)
                oDLO.LoadWith(Of LTS.tblUserAdmin)(Function(t As LTS.tblUserAdmin) t.tblUserSecurity)
                db.LoadOptions = oDLO
                'if there are no null values to deal with we can build the array directly
                Dim tblUserAdmin As DTO.tblUserAdmin() = (
                From t In db.tblUserAdmins
                Where t.UserSecurityControl = UserSecurityControl
                Order By t.CompRefSecurity.CompNumber Ascending
                Select New DTO.tblUserAdmin With {.UserAdminControl = t.UserAdminControl,
                                                      .UserSecurityControl = t.UserSecurityControl,
                                                      .UserAdminCompControl = t.UserAdminCompControl,
                                                      .UserName = t.tblUserSecurity.UserName,
                                                      .CompName = t.CompRefSecurity.CompName,
                                                      .CompNumber = If(t.CompRefSecurity.CompNumber.HasValue, t.CompRefSecurity.CompNumber, 0),
                                                      .UserAdminUpdated = t.UserAdminUpdated.ToArray()}).ToArray
                Return tblUserAdmin
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

    Public Function GettblUserAdminUnrestrictedByUser(ByVal UserSecurityControl As Integer) As DTO.tblUserAdmin
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim RestrinctedQuery = From useradmin In db.tblUserAdmins Where useradmin.UserSecurityControl = UserSecurityControl Select useradmin.UserAdminCompControl
                Dim tblUserAdmin As DTO.tblUserAdmin = (
                               From t In db.CompRefSecurities
                               Where t.CompActive = True _
                               And Not RestrinctedQuery.Contains(t.CompNumber)
                               Order By t.CompControl
                               Select New DTO.tblUserAdmin With {.UserAdminControl = t.CompControl,
                                                                     .UserSecurityControl = UserSecurityControl,
                                                                     .UserAdminCompControl = t.CompNumber,
                                                                     .CompName = t.CompName,
                                                                     .CompNumber = t.CompNumber}).Single
                Return tblUserAdmin
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
    ''' This method uses the CompControl number as the UserAdminControl number because no tblUserAdmin records actually exist for Unrestricted companies. 
    ''' Note: the UserAdminCompControl actualy represents the company number not the company control number.
    ''' 
    ''' NOTE: Replaced by NGLUserAdminData.GetUnrestrictedCompsByUser() below for 365 (LEUsers page)
    ''' Hopefully one day we can phase this out completely
    ''' </summary>
    ''' <param name="UserSecurityControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GettblUserAdminsUnrestrictedByUser(ByVal UserSecurityControl As Integer) As DTO.tblUserAdmin()
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'db.Log = New DebugTextWriter
                Dim RestrinctedQuery = From useradmin In db.tblUserAdmins Where useradmin.UserSecurityControl = UserSecurityControl Select useradmin.UserAdminCompControl
                Dim tblUserAdmin As DTO.tblUserAdmin() = (
                From t In db.CompRefSecurities
                Where t.CompActive = True _
                And Not RestrinctedQuery.Contains(t.CompNumber)
                Order By t.CompNumber Ascending
                Select New DTO.tblUserAdmin With {.UserAdminControl = t.CompControl,
                                                      .UserSecurityControl = UserSecurityControl,
                                                      .UserAdminCompControl = t.CompNumber,
                                                      .CompName = t.CompName,
                                                      .CompNumber = t.CompNumber}).ToArray
                Return tblUserAdmin
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

    Public Function GettblUserAdminByComp(ByVal CompControl As Integer) As DTO.tblUserAdmin
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim oDLO As New DataLoadOptions
                oDLO.LoadWith(Of LTS.tblUserAdmin)(Function(t As LTS.tblUserAdmin) t.CompRefSecurity)
                oDLO.LoadWith(Of LTS.tblUserAdmin)(Function(t As LTS.tblUserAdmin) t.tblUserSecurity)
                db.LoadOptions = oDLO
                'if there are no null values to deal with we can build the array directly
                Dim tblUserAdmin As DTO.tblUserAdmin = (
                From t In db.tblUserAdmins
                Where t.UserAdminCompControl = CompControl
                Order By t.UserAdminControl Descending
                Select New DTO.tblUserAdmin With {.UserAdminControl = t.UserAdminControl,
                                                      .UserSecurityControl = t.UserSecurityControl,
                                                      .UserAdminCompControl = t.UserAdminCompControl,
                                                      .UserName = t.tblUserSecurity.UserName,
                                                      .CompName = t.CompRefSecurity.CompName,
                                                      .CompNumber = If(t.CompRefSecurity.CompNumber.HasValue, t.CompRefSecurity.CompNumber, 0),
                                                      .UserAdminUpdated = t.UserAdminUpdated.ToArray()}).Single
                Return tblUserAdmin
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

    Public Function GettblUserAdminsByComp(ByVal CompControl As Integer) As DTO.tblUserAdmin()
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim oDLO As New DataLoadOptions
                oDLO.LoadWith(Of LTS.tblUserAdmin)(Function(t As LTS.tblUserAdmin) t.CompRefSecurity)
                oDLO.LoadWith(Of LTS.tblUserAdmin)(Function(t As LTS.tblUserAdmin) t.tblUserSecurity)
                db.LoadOptions = oDLO
                'if there are no null values to deal with we can build the array directly
                Dim tblUserAdmin As DTO.tblUserAdmin() = (
                From t In db.tblUserAdmins
                Where t.UserAdminCompControl = CompControl
                Select New DTO.tblUserAdmin With {.UserAdminControl = t.UserAdminControl,
                                                      .UserSecurityControl = t.UserSecurityControl,
                                                      .UserAdminCompControl = t.UserAdminCompControl,
                                                      .UserName = t.tblUserSecurity.UserName,
                                                      .CompName = t.CompRefSecurity.CompName,
                                                      .CompNumber = If(t.CompRefSecurity.CompNumber.HasValue, t.CompRefSecurity.CompNumber, 0),
                                                      .UserAdminUpdated = t.UserAdminUpdated.ToArray()}).ToArray
                Return tblUserAdmin
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

    Public Function CreatetblUserAdmin(ByVal oData As DTO.tblUserAdmin) As DTO.tblUserAdmin
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            'Create New Record
            Dim ntblUserAdmin As LTS.tblUserAdmin = New LTS.tblUserAdmin With {
                .UserAdminControl = oData.UserAdminControl,
                .UserSecurityControl = oData.UserSecurityControl,
                .UserAdminCompControl = oData.UserAdminCompControl,
                .UserAdminUpdated = oData.UserAdminUpdated
            }
            db.tblUserAdmins.InsertOnSubmit(ntblUserAdmin)
            Try
                db.SubmitChanges()
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
            Return GettblUserAdmin(ntblUserAdmin.UserAdminControl)
        End Using
    End Function

    ''' <summary>
    ''' NOTE: Replaced by NGLUserAdminData.DeleteUserAdmin() below for 365 (LEUsers page)
    ''' Hopefully one day we can phase this out completely
    ''' </summary>
    ''' <param name="oData"></param>
    Public Sub DeletetblUserAdmin(ByVal oData As DTO.tblUserAdmin)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            ' Delete the record
            Dim ntblUserAdmin As LTS.tblUserAdmin = New LTS.tblUserAdmin With {.UserAdminControl = oData.UserAdminControl,
                                                                              .UserSecurityControl = oData.UserSecurityControl,
                                                                              .UserAdminCompControl = oData.UserAdminCompControl,
                                                                              .UserAdminUpdated = oData.UserAdminUpdated}
            db.tblUserAdmins.Attach(ntblUserAdmin, True)
            db.tblUserAdmins.DeleteOnSubmit(ntblUserAdmin)
            Try
                db.SubmitChanges()
            Catch ex As SqlException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch conflictEx As ChangeConflictException
                Try
                    Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(db))
                    conflictInfo.LogConflictDetails(conflictInfo.Message, Me.Parameters)
                    Throw New FaultException(Of ConflictFaultInfo)(conflictInfo, New FaultReason("E_DataConflict"))
                Catch ex As FaultException
                    Throw
                Catch ex As Exception
                    SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                End Try
            Catch ex As InvalidOperationException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
        End Using
    End Sub

    Public Function UpdatetblUserAdmin(ByVal oData As DTO.tblUserAdmin) As DTO.tblUserAdmin
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            ' Attach the order
            Dim ntblUserAdmin As LTS.tblUserAdmin = New LTS.tblUserAdmin With {.UserAdminControl = oData.UserAdminControl,
                                                                              .UserSecurityControl = oData.UserSecurityControl,
                                                                              .UserAdminCompControl = oData.UserAdminCompControl,
                                                                              .UserAdminUpdated = oData.UserAdminUpdated}
            db.tblUserAdmins.Attach(ntblUserAdmin, True)
            Try
                db.SubmitChanges()
            Catch ex As SqlException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch conflictEx As ChangeConflictException
                Try
                    Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(db))
                    conflictInfo.LogConflictDetails(conflictInfo.Message, Me.Parameters)
                    Throw New FaultException(Of ConflictFaultInfo)(conflictInfo, New FaultReason("E_DataConflict"))
                Catch ex As FaultException
                    Throw
                Catch ex As Exception
                    SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                End Try
            Catch ex As InvalidOperationException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
            ' Return the updated order
            Return GettblUserAdmin(ntblUserAdmin.UserAdminControl)
        End Using
    End Function

#End Region

#Region "tblUserSecurity"

    ''' <summary>
    ''' Get all the users that are nextrack only.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GettblUserSecuritysNEXTRackOnly() As DTO.tblUserSecurity()
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'if there are no null values to deal with we can build the array directly
                Dim tblUserSecuritys() As DTO.tblUserSecurity = (
                    From t In db.tblUserSecurities
                    Where t.NEXTrackOnly = True
                    Group Join g In db.tblUserGroups On t.UserUserGroupsControl Equals g.UserGroupsControl Into Group
                    Order By t.UserName
                    From ug In Group.DefaultIfEmpty()
                    Select selecttblUserSecurityDTOData(t, db, ug)).ToArray()
                Return tblUserSecuritys
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

    Public Function GettblUserSecuritys() As DTO.tblUserSecurity()
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'if there are no null values to deal with we can build the array directly
                Dim tblUserSecuritys() As DTO.tblUserSecurity = (
                    From t In db.tblUserSecurities
                    Group Join g In db.tblUserGroups On t.UserUserGroupsControl Equals g.UserGroupsControl Into Group
                    Order By t.UserName
                    From ug In Group.DefaultIfEmpty()
                    Select selecttblUserSecurityDTOData(t, db, ug)).ToArray()
                Return tblUserSecuritys
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

    Public Function GettblUserSecurity(ByVal Control As Integer) As DTO.tblUserSecurity
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                db.Log = New DebugTextWriter
                'if there are no null values to deal with we can build the array directly
                Dim tblUserSecurity As DTO.tblUserSecurity = (
                From t In db.tblUserSecurities
                Group Join g In db.tblUserGroups On t.UserUserGroupsControl Equals g.UserGroupsControl Into Group
                Where t.UserSecurityControl = Control
                From ug In Group.DefaultIfEmpty()
                Select selecttblUserSecurityDTOData(t, db, ug)).Single
                Return tblUserSecurity
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

    Public Function GettblUserSecurityByUserName(ByVal UserName As String) As DTO.tblUserSecurity
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'if there are no null values to deal with we can build the array directly
                Dim tblUserSecurity As DTO.tblUserSecurity = (
                From t In db.tblUserSecurities
                Group Join g In db.tblUserGroups On t.UserUserGroupsControl Equals g.UserGroupsControl Into Group
                Where t.UserName = UserName
                From ug In Group.DefaultIfEmpty()
                Select selecttblUserSecurityDTOData(t, db, ug)).FirstOrDefault()
                Return tblUserSecurity
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

    Public Function GettblUserSecuritysByGroup(ByVal GroupControl As Integer) As DTO.tblUserSecurity()
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'if there are no null values to deal with we can build the array directly
                Dim tblUserSecuritys() As DTO.tblUserSecurity = (
                    From t In db.tblUserSecurities
                    Group Join g In db.tblUserGroups On t.UserUserGroupsControl Equals g.UserGroupsControl Into Group
                    Where t.UserUserGroupsControl = GroupControl
                    Order By t.UserName
                    From ug In Group.DefaultIfEmpty()
                    Select selecttblUserSecurityDTOData(t, db, ug)).ToArray()
                Return tblUserSecuritys
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

    Public Function GettblUserSecuritysNotGroup(ByVal GroupControl As Integer) As DTO.tblUserSecurity()
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'if there are no null values to deal with we can build the array directly
                Dim tblUserSecuritys() As DTO.tblUserSecurity = (
                    From t In db.tblUserSecurities
                    Group Join g In db.tblUserGroups On t.UserUserGroupsControl Equals g.UserGroupsControl Into Group
                    Where t.UserUserGroupsControl <> GroupControl
                    Order By t.UserName
                    From ug In Group.DefaultIfEmpty()
                    Select selecttblUserSecurityDTOData(t, db, ug)).ToArray()
                Return tblUserSecuritys
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
    ''' OLD - CreatetblUserSecurity
    ''' This method now calls the new method CreatetblUserSecurity365. Old code that calls this method should still work.
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified By LVV on 4/5/18 for v-8.1 TMS 365
    ''' This method now calls the new method CreatetblUserSecurity365. Old code that calls this method should still work.
    ''' </remarks>
    Public Function CreatetblUserSecurity(ByVal oData As DTO.tblUserSecurity) As DTO.tblUserSecurity
        'Create New Record
        Dim ntblUserSecurity As LTS.tblUserSecurity = selecttblUserSecurityLTSData(oData, Nothing)
        Dim r = CreatetblUserSecurity365(ntblUserSecurity)
        Return GettblUserSecurity(r)

        'Using db As New NGLMASSecurityDataContext(ConnectionString)
        '    If oData.NEXTrackOnly = False Then
        '        'check if the license allows new users.
        '        Dim intTooManyUsers As Integer = Me.getScalarInteger("Select Case When COUNT(UserSecurityControl) >= (Select top 1 AuthRef from Auth) Then 1 else 0 end from tblUserSecurity Where NEXTrackOnly = 0")
        '        If intTooManyUsers <> 0 Then
        '            Utilities.SaveAppError("Cannot create new user data.  The maximum number of users have already been created.  Please contact support for more information.", Me.Parameters)
        '            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_MaxUsersAssigned"}, New FaultReason("E_AccessDenied"))
        '            Return Nothing
        '        End If
        '    End If
        '    db.tblUserSecurities.InsertOnSubmit(ntblUserSecurity)
        '    Try
        '        db.SubmitChanges()
        '    Catch ex As System.Data.SqlClient.SqlException
        '        Utilities.SaveAppError(ex.Message, Me.Parameters)
        '        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        '    Catch ex As InvalidOperationException
        '        Utilities.SaveAppError(ex.Message, Me.Parameters)
        '        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
        '    Catch ex As Exception
        '        Utilities.SaveAppError(ex.Message, Me.Parameters)
        '        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        '    End Try
        '    Return GettblUserSecurity(ntblUserSecurity.UserSecurityControl)
        'End Using
    End Function

    ''' <summary>
    ''' OLD - DeletetblUserSecurity
    ''' This method now calls the new method DeleteUserSecurity365. Old code that calls this method should still work.
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <remarks>
    ''' Modified By LVV on 4/6/18 for v-8.1 TMS 365
    ''' This method now calls the new method DeleteUserSecurity365. Old code that calls this method should still work.
    ''' </remarks>
    Public Sub DeletetblUserSecurity(ByVal oData As DTO.tblUserSecurity)
        DeleteUserSecurity365(oData.UserSecurityControl)
        'Using db As New NGLMASSecurityDataContext(ConnectionString)
        '    ' Delete the record
        '    Dim ntblUserSecurity As LTS.tblUserSecurity = selecttblUserSecurityLTSData(oData, db)
        '    db.tblUserSecurities.Attach(ntblUserSecurity, True)
        '    db.tblUserSecurities.DeleteOnSubmit(ntblUserSecurity)
        '    Try
        '        db.SubmitChanges()
        '    Catch ex As SqlException
        '        Utilities.SaveAppError(ex.Message, Me.Parameters)
        '        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        '    Catch conflictEx As ChangeConflictException
        '        Try
        '            Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(db))
        '            conflictInfo.LogConflictDetails(conflictInfo.Message, Me.Parameters)
        '            Throw New FaultException(Of ConflictFaultInfo)(conflictInfo, New FaultReason("E_DataConflict"))
        '        Catch ex As FaultException
        '            Throw
        '        Catch ex As Exception
        '            Utilities.SaveAppError(ex.Message, Me.Parameters)
        '            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        '        End Try
        '    Catch ex As InvalidOperationException
        '        Utilities.SaveAppError(ex.Message, Me.Parameters)
        '        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
        '    Catch ex As Exception
        '        Utilities.SaveAppError(ex.Message, Me.Parameters)
        '        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        '    End Try
        'End Using
    End Sub

    Public Function UpdatetblUserSecurity(ByVal oData As DTO.tblUserSecurity) As DTO.tblUserSecurity
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            ' Attach the order
            Dim ntblUserSecurity As LTS.tblUserSecurity = selecttblUserSecurityLTSData(oData, db)
            db.tblUserSecurities.Attach(ntblUserSecurity, True)
            Try
                db.SubmitChanges()
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch conflictEx As ChangeConflictException
                Try
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
            ' Return the updated order
            Return GettblUserSecurity(ntblUserSecurity.UserSecurityControl)
        End Using
    End Function

    ''' <summary>
    ''' Validat that a password meets legal entity security setting 
    ''' </summary>
    ''' <param name="sPassword"></param>
    ''' <param name="sMsg"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.2.006 on 2023-01-25 added new password strength validation by Legal Entity
    ''' </remarks>
    Public Function validatePasswordStrength(ByVal sPassword As String, ByRef sMsg As String) As Boolean
        Dim bRet As Boolean = False
        'Look up Password Strength using tblLegalEntityAdmin.LEAdminSecurityLevel
        Dim oLEDAL As NGLLegalEntityAdminData = New NGLLegalEntityAdminData(Parameters)
        Dim iSecurityVal As Integer = oLEDAL.GetLEAdminSecurityLevel(Parameters.UserLEControl)
        Select Case iSecurityVal
            Case 2
                ' 1 lower case letter
                ' 1 Upper case letter
                ' 1 number
                ' 1 special character like !@#$%^&*()\-__+.
                ' 8 total characters or more
                If Regex.IsMatch(sPassword, "^(?=(.*[a-z]){1,})(?=(.*[A-Z]){1,})(?=(.*[0-9]){1,})(?=(.*[!@#$%^&*()\-__+.]){1,}).{8,}$") Then
                    bRet = True
                Else
                    sMsg = "E_InvalidNewPasswordLevel2"
                End If

            Case 3
                ' 3 lower case letter
                ' 2 Upper case letter
                ' 2 number
                ' 1 special character like !@#$%^&*()\-__+.
                ' 8 total characters or more
                If Regex.IsMatch(sPassword, "^(?=(.*[a-z]){3,})(?=(.*[A-Z]){2,})(?=(.*[0-9]){2,})(?=(.*[!@#$%^&*()\-__+.]){1,}).{8,}$") Then
                    bRet = True
                Else
                    sMsg = "E_InvalidNewPasswordLevel3"
                End If
            Case Else
                If (String.IsNullOrWhiteSpace(sPassword) OrElse sPassword.Length < 6) Then
                    sMsg = "E_InvalidNewPassword"
                Else
                    bRet = True
                End If

        End Select
        Return bRet
    End Function

    ''' <summary>
    ''' Encrypts and save the updated user password to the database 
    ''' </summary>
    ''' <param name="UserControl"></param>
    ''' <param name="sNewPassword"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.0 on 08/30/2017
    '''  encrypts and saves the password to the database for the provided user control
    '''  caller must manage security concerns
    ''' Modified by RHR for v-8.5.2.006 on 2023-01-27 added logic to update UserMustChangePassword flag to false
    ''' </remarks>
    Public Function tblUserSecuritySaveNewPassword(ByVal UserControl As Integer, ByVal sNewPassword As String, Optional ByVal sCurrentPassword As String = "") As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Dim ntblUserSecurity As LTS.tblUserSecurity = db.tblUserSecurities.Where(Function(x) x.UserSecurityControl = UserControl).FirstOrDefault()
            If ntblUserSecurity Is Nothing OrElse ntblUserSecurity.UserSecurityControl = 0 Then
                Return False
            End If
            If Not String.IsNullOrWhiteSpace(sCurrentPassword) Then
                'we need to compare the current passwor with the value provided
                Dim sCurrentEncrypted = DTran.Encrypt(sCurrentPassword, "NGL")
                If ntblUserSecurity.UserRemotePassword <> sCurrentPassword AndAlso ntblUserSecurity.UserRemotePassword <> sCurrentEncrypted Then
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidCurrentPassword"}, New FaultReason("E_AccessDenied"))
                    Return False
                End If
            End If
            'Encrypt the password
            sNewPassword = DTran.Encrypt(sNewPassword, "NGL")
            ntblUserSecurity.UserRemotePassword = sNewPassword
            ntblUserSecurity.UserMustChangePassword = False
            Try
                db.SubmitChanges()
                blnRet = True
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch conflictEx As ChangeConflictException
                Try
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
            ' Return the updated order
            Return blnRet
        End Using
    End Function

    ''' <summary>
    ''' Update the user theme for the provided user control if no theme is provided the default theme, classic-opal, is used
    ''' </summary>
    ''' <param name="UserControl"></param>
    ''' <param name="sTheme"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.4.001 05/24/2034
    ''' updates the users theme for the provided user control
    ''' </remarks>
    Public Function tblUserSecurityChangeUserTheme365(ByVal UserControl As Integer, ByVal sTheme As String) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Dim ntblUserSecurity As LTS.tblUserSecurity = db.tblUserSecurities.Where(Function(x) x.UserSecurityControl = UserControl).FirstOrDefault()
            If ntblUserSecurity Is Nothing OrElse ntblUserSecurity.UserSecurityControl = 0 Then
                Return False
            End If
            If String.IsNullOrWhiteSpace(sTheme) Then
                sTheme = "classic-opal"
            End If
            ntblUserSecurity.UserTheme365 = sTheme
            Try
                db.SubmitChanges()
                blnRet = True
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch conflictEx As ChangeConflictException
                Try
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
            ' Return the updated order
            Return blnRet
        End Using
    End Function

    ''' <summary>
    ''' Copy LTS data to DTO data 
    ''' </summary>
    ''' <param name="d"></param>
    ''' <param name="db"></param>
    ''' <param name="g"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.2.006 added tblUserSecurity.UserMustChangePassword property
    ''' Modified by RHR for v-8.5.4.005 03/28/2024 added timezone and culture
    ''' </remarks>
    Friend Function selecttblUserSecurityDTOData(ByVal d As LTS.tblUserSecurity, ByRef db As NGLMASSecurityDataContext, ByRef g As LTS.tblUserGroup) As DTO.tblUserSecurity
        Return New DTO.tblUserSecurity With {.UserSecurityControl = d.UserSecurityControl,
                                                         .UserName = d.UserName,
                                                         .UserDepartment = d.UserDepartment,
                                                         .UserEmail = d.UserEmail,
                                                         .UserFirstName = d.UserFirstName,
                                                         .UserFriendlyName = d.UserFriendlyName,
                                                         .UserLastName = d.UserLastName,
                                                         .UserMiddleIn = d.UserMiddleIn,
                                                         .UserPhoneCell = d.UserPhoneCell,
                                                         .UserPhoneHome = d.UserPhoneHome,
                                                         .UserPhoneWork = d.UserPhoneWork,
                                                         .UserWorkExt = d.UserWorkExt,
                                                         .UseAuthCode = d.UseAuthCode,
                                                         .UserTitle = d.UserTitle,
                                                         .UserRemotePassword = d.UserRemotePassword,
                                                         .UserUserGroupsControl = d.UserUserGroupsControl,
                                                         .UserGroupsName = If(g Is Nothing, "", g.UserGroupsName),
                                                         .NEXTrackOnly = d.NEXTrackOnly,
                                                         .UserUpdated = d.UserUpdated.ToArray(),
                                                         .UserSSOAControl = d.UserSSOAControl,
                                                         .UserStartFreeTrial = d.UserStartFreeTrial,
                                                         .UserEndFreeTrial = d.UserEndFreeTrial,
                                                         .UserFreeTrialActive = d.UserFreeTrialActive,
                                                         .UserTheme365 = d.UserTheme365,
                                                         .UserMustChangePassword = d.UserMustChangePassword,
                                                         .UserCultureInfo = d.UserCultureInfo,
                                                         .UserTimeZone = d.UserTimeZone}
    End Function

    ''' <summary>
    ''' Copy DTO data to LTS data for tblUserSecurity
    ''' </summary>
    ''' <param name="d"></param>
    ''' <param name="db"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.2.006 added tblUserSecurity.UserMustChangePassword property
    ''' Modified by RHR for v-8.5.4.005 03/28/2024 added timezone and culture
    ''' </remarks>
    Friend Function selecttblUserSecurityLTSData(ByVal d As DTO.tblUserSecurity, ByRef db As NGLMASSecurityDataContext) As LTS.tblUserSecurity
        Return New LTS.tblUserSecurity With {.UserSecurityControl = d.UserSecurityControl,
                                                         .UserName = d.UserName,
                                                         .UserDepartment = d.UserDepartment,
                                                         .UserEmail = d.UserEmail,
                                                         .UserFirstName = d.UserFirstName,
                                                         .UserFriendlyName = d.UserFriendlyName,
                                                         .UserLastName = d.UserLastName,
                                                         .UserMiddleIn = d.UserMiddleIn,
                                                         .UserPhoneCell = d.UserPhoneCell,
                                                         .UserPhoneHome = d.UserPhoneHome,
                                                         .UserPhoneWork = d.UserPhoneWork,
                                                         .UserWorkExt = d.UserWorkExt,
                                                         .UseAuthCode = d.UseAuthCode,
                                                         .UserTitle = d.UserTitle,
                                                         .UserRemotePassword = d.UserRemotePassword,
                                                         .UserUserGroupsControl = d.UserUserGroupsControl,
                                                         .NEXTrackOnly = d.NEXTrackOnly,
                                                         .UserUpdated = If(d.UserUpdated Is Nothing, New Byte() {}, d.UserUpdated),
                                                         .UserSSOAControl = d.UserSSOAControl,
                                                         .UserStartFreeTrial = d.UserStartFreeTrial,
                                                         .UserEndFreeTrial = d.UserEndFreeTrial,
                                                         .UserFreeTrialActive = d.UserFreeTrialActive,
                                                         .UserTheme365 = d.UserTheme365,
                                                         .UserMustChangePassword = d.UserMustChangePassword,
                                                         .UserTimeZone = d.UserTimeZone,
                                                         .UserCultureInfo = d.UserCultureInfo}
    End Function

#Region "TMS 365 (tblUserSecurity)"

    ''' <summary>
    ''' The same as the orginal version CreatetblUserSecurity except this version uses LTS as a parameter instead of DTO. 
    ''' If not NEXTrackOnly we check to see if we are allowed to add more users before we do the insert.
    ''' Returns the UserSecurityControl on success or Nothing on fail (exception thrown or exceeded max users).
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 4/5/18 for v-8.1 TMS 365
    ''' Modified By LVV on 1/20/20 - Modify the code that validates the # of users allowed to exclude the SuperUser Accounts
    ''' </remarks>
    Public Function CreatetblUserSecurity365(ByVal oData As LTS.tblUserSecurity) As Integer
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            If oData.NEXTrackOnly = False Then
                'check if the license allows new users.
                Dim intTooManyUsers As Integer = Me.getScalarInteger("Select Case When COUNT(UserSecurityControl) >= (Select top 1 AuthRef from Auth) Then 1 else 0 end from tblUserSecurity Where NEXTrackOnly = 0 And UserUserGroupsControl <> 9") 'Modified By LVV on 1/20/20 - Modify the code that validates the # of users allowed to exclude the SuperUser Accounts
                If intTooManyUsers <> 0 Then
                    Utilities.SaveAppError("Cannot create new user data. The maximum number of users have already been created. Please contact support for more information.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_MaxUsersAssigned"}, New FaultReason("E_AccessDenied"))
                    Return Nothing
                End If
            End If
            'check if the username already exists
            Dim uName = oData.UserName
            If db.tblUserSecurities.Any(Function(x) x.UserName = uName) Then
                Dim strDet = "This User Name is already assigned to another account." 'TODO - localize this
                Utilities.SaveAppError(strDet, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField", .Details = strDet}, New FaultReason("E_DataValidationFailure"))
            End If
            db.tblUserSecurities.InsertOnSubmit(oData)
            Try
                db.SubmitChanges()
                Return oData.UserSecurityControl
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
    ''' The same as the orginal version DeleteUserSecurity except this version uses the Control as a parameter instead of DTO. 
    ''' Only fails if an exception is thrown else success
    ''' </summary>
    ''' <param name="ControlNumber"></param>
    ''' <remarks>
    ''' Added By LVV on 4/6/18 for v-8.1 TMS 365
    ''' </remarks>
    Public Sub DeleteUserSecurity365(ByVal ControlNumber As Integer)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Dim nObject = db.tblUserSecurities.Where(Function(x) x.UserSecurityControl = ControlNumber).FirstOrDefault()
            If Not nObject Is Nothing AndAlso nObject.UserSecurityControl <> 0 Then
                db.tblUserSecurities.DeleteOnSubmit(nObject)
                Try
                    db.SubmitChanges()
                Catch ex As SqlException
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                Catch conflictEx As ChangeConflictException
                    Try
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
            End If
        End Using
    End Sub

#End Region

#End Region

#Region "tblUserSecuritySettings"

    'Public function getUserSettings() As 
#End Region

#Region "tblUserGroup"

    ''' <summary>
    ''' Modified by LVV on 4/15/19 - Added Where clause to filter out any groups with
    ''' SuperUser permissions (so they do not show up in desktop client - 365 uses new methods in new class NGLUserGroupData)
    ''' </summary>
    ''' <returns></returns>
    Public Function GettblUserGroups() As DTO.tblUserGroup()
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'if there are no null values to deal with we can build the array directly
                Dim tblUserGroups() As DTO.tblUserGroup = (
                    From t In db.tblUserGroups
                    Where t.UserGroupsUGCControl <> 4
                    Order By t.UserGroupsName
                    Select New DTO.tblUserGroup With {.UserGroupsControl = t.UserGroupsControl, .UserGroupsName = t.UserGroupsName, .UserGroupsUpdated = t.UserGroupsUpdated.ToArray()}).ToArray()
                Return tblUserGroups
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

    Public Function GettblUserGroup(ByVal Control As Integer) As DTO.tblUserGroup
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'if there are no null values to deal with we can build the array directly
                Dim tblUserGroup As DTO.tblUserGroup = (
                From t In db.tblUserGroups
                Where t.UserGroupsControl = Control
                Select New DTO.tblUserGroup With {.UserGroupsControl = t.UserGroupsControl, .UserGroupsName = t.UserGroupsName, .UserGroupsUpdated = t.UserGroupsUpdated.ToArray()}).Single
                Return tblUserGroup
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

    Public Function CreatetblUserGroup(ByVal oData As DTO.tblUserGroup) As DTO.tblUserGroup
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            'Create New Record
            Dim ntblUserGroup As LTS.tblUserGroup = New LTS.tblUserGroup With {.UserGroupsControl = oData.UserGroupsControl, .UserGroupsName = oData.UserGroupsName, .UserGroupsUpdated = oData.UserGroupsUpdated}
            db.tblUserGroups.InsertOnSubmit(ntblUserGroup)
            Try
                db.SubmitChanges()
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
            Return GettblUserGroup(ntblUserGroup.UserGroupsControl)
        End Using
    End Function

    Public Sub CreatetblUserGroup(ByVal oData As LTS.tblUserGroup)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            db.tblUserGroups.InsertOnSubmit(oData)
            Try
                db.SubmitChanges()
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
    End Sub


    Public Sub DeletetblUserGroup(ByVal oData As DTO.tblUserGroup)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            ' Delete the record
            Dim ntblUserGroup As LTS.tblUserGroup = New LTS.tblUserGroup With {.UserGroupsControl = oData.UserGroupsControl, .UserGroupsName = oData.UserGroupsName, .UserGroupsUpdated = oData.UserGroupsUpdated}

            db.tblUserGroups.Attach(ntblUserGroup, True)
            db.tblUserGroups.DeleteOnSubmit(ntblUserGroup)

            Try
                db.SubmitChanges()
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch conflictEx As ChangeConflictException
                Try
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
        End Using
    End Sub

    Public Function UpdatetblUserGroup(ByVal oData As DTO.tblUserGroup) As DTO.tblUserGroup
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            ' Attach the order
            Dim ntblUserGroup As LTS.tblUserGroup = New LTS.tblUserGroup With {.UserGroupsControl = oData.UserGroupsControl, .UserGroupsName = oData.UserGroupsName, .UserGroupsUpdated = oData.UserGroupsUpdated}
            db.tblUserGroups.Attach(ntblUserGroup, True)

            Try
                db.SubmitChanges()
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch conflictEx As ChangeConflictException
                Try
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
            ' Return the updated order
            Return GettblUserGroup(ntblUserGroup.UserGroupsControl)
        End Using
    End Function

#Region "TMS 365"

    Public Function GetUserGroupFiltered(ByVal LEAdminCompControl As Integer, ByVal CatControl As Integer) As DTO.tblUserGroup
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'if there are no null values to deal with we can build the array directly
                Dim tblUserGroup As DTO.tblUserGroup = (
                    From t In db.tblUserGroups
                    Where t.UserGroupsLegalEntityCompControl = LEAdminCompControl AndAlso t.UserGroupsUGCControl = CatControl
                    Select New DTO.tblUserGroup With {.UserGroupsControl = t.UserGroupsControl, .UserGroupsName = t.UserGroupsName}).FirstOrDefault()
                Return tblUserGroup
            Catch ex As SqlException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Creates the default UserGroups for a Legal Entity
    ''' and creates all the Form, Procedure, and Report
    ''' security for the new Groups based on the Standard
    ''' Default Groups (LEAControl = 0, UserGroupsControls 1-10) 
    ''' Admin, Operations, Accounting, Carriers, and Carrier Accounting
    ''' Used when a Free Trial User signs up for a full subscription
    ''' </summary>
    ''' <param name="LegalEntity"></param>
    ''' <param name="CompControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 4/13/18 for v-8.1 VSTS Task #327 Ted Page
    ''' </remarks>
    Public Function CreateDefaultUserGroupsForLE(ByVal LegalEntity As String, ByVal CompControl As Integer) As LTS.spCreateDefaultUserGroupsForLEResult
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Return db.spCreateDefaultUserGroupsForLE(LegalEntity, CompControl, Parameters.UserName).FirstOrDefault()
            Catch ex As SqlException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Gets the Equivalent User Group under the provided New Legal Entity for the provided user
    ''' </summary>
    ''' <param name="UserSecurityControl">User account being assigned to a new Legal Entity</param>
    ''' <param name="NewLEAdminControl">Legal Entity the user is being assigned to</param>
    ''' <returns>The UserGroup associated with the new Legal Entity to which the user account should be moved</returns>
    ''' <remarks>Added By LVV on 4/15/20</remarks>
    Public Function GetEquivalentUserGroupForNewLE(ByVal UserSecurityControl As Integer, ByVal NewLEAdminControl As Integer) As Integer
        Dim newGroupControl As Integer = 0
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim spRes = db.spGetEquivalentUserGroupForNewLE(UserSecurityControl, NewLEAdminControl).FirstOrDefault()
                If spRes IsNot Nothing Then newGroupControl = spRes.NewGroupControl
            Catch ex As SqlException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
        End Using
        Return newGroupControl
    End Function

#End Region

#End Region

#Region "System Alert Selection By User"


    ''' <summary>
    ''' Returns a reference to the Unsubscribed subscription alert
    ''' </summary>
    ''' <param name="iProcedureControl"></param>
    ''' <param name="UserSecurityControl"></param>
    ''' <returns>
    ''' Created by RHR for v-8.5.3.007 on 2023-02-02
    ''' </returns>
    Public Function GetUserUnSubscribedSystemAlert(ByVal iProcedureControl As Integer, ByVal UserSecurityControl As Integer) As DTO.tblProcedureList
        'Create a data connection 
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblProcedureLists As New DTO.tblProcedureList()
        Try
            Dim strSQL As String = " Select Top 1 " _
                & " tblProcedureList.ProcedureControl, tblProcedureList.ProcedureName, tblProcedureList.ProcedureDescription " _
                & " FROM tblProcedureList " _
                & " WHERE  ProcedureControl = iProcedureControl And  (ProcedureControl NOT IN " _
                & " (SELECT ProcedureControl " _
                    & " FROM dbo.tblProcedureSecurityXref " _
                    & " WHERE tblProcedureSecurityXref.UserSecurityControl = " _
                    & UserSecurityControl & "))" _
                    & " AND  ProcedureHasAlert = 1" _
                    & " AND (ProcedureControl NOT IN " _
                    & " (SELECT ProcedureControl " _
                    & " FROM dbo.tblProcAlertUserXref " _
                    & " WHERE tblProcAlertUserXref.UserSecurityControl = " _
                    & UserSecurityControl & "))" _
                & " Order By ProcedureName"

            Dim oQRet As Ngl.Core.Data.QueryResult = oQuery.ExecuteWithFill(strSQL)
            If Not oQRet.Exception Is Nothing Then
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NotReadProcByUser"}, New FaultReason("E_DataAccessError"))
            End If
            If Not oQRet.Data Is Nothing AndAlso oQRet.Data.Rows.Count > 0 Then
                Dim oRow As System.Data.DataRow = oQRet.Data.Rows(0)
                With tblProcedureLists
                    .ProcedureControl = DTran.getDataRowValue(oRow, "ProcedureControl", 0)
                    .ProcedureName = DTran.getDataRowString(oRow, "ProcedureName", "Unnamed")
                    .ProcedureDescription = DTran.getDataRowString(oRow, "ProcedureDescription", "")
                End With
            End If
            If Not tblProcedureLists Is Nothing Then
                Return tblProcedureLists
            Else
                Return Nothing
            End If
        Catch ex As FaultException
            Throw
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As InvalidOperationException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            'its ok if there is no data.
            'Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            oQuery = Nothing
        End Try
        Return Nothing
    End Function




    ''' <summary>
    ''' Get a single subscribed alert by user and procedure control
    ''' </summary>
    ''' <param name="iProcedureControl"></param>
    ''' <param name="UserSecurityControl"></param>
    ''' <returns>
    ''' Created by RHR for v-8.5.3.007 on 2023-02-02
    ''' </returns>
    Public Function GetUserSubscribedSystemAlert(ByVal iProcedureControl As Integer, ByVal UserSecurityControl As Integer) As DTO.tblProcedureList
        'Create a data connection 
        Dim oQuery As New Core.Data.Query(ConnectionString)
        Dim tblProcedureLists As New DTO.tblProcedureList
        Try
            Dim strSQL As String = " Select Top 1 " _
               & " tblProcedureList.ProcedureControl, tblProcedureList.ProcedureName, tblProcedureList.ProcedureDescription, tblProcAlertUserXref.ProcAlertUserXrefShowPopup,tblProcAlertUserXref.ProcAlertUserXrefSendEmail " _
               & " FROM tblProcedureList " _
               & " inner join dbo.tblProcAlertUserXref on tblProcedureList.ProcedureControl = dbo.tblProcAlertUserXref.ProcedureControl AND dbo.tblProcAlertUserXref.UserSecurityControl = " & UserSecurityControl & " " _
               & " WHERE procedureControl = iProcedureControl And  (tblProcedureList.ProcedureControl NOT IN " _
               & " (SELECT ProcedureControl " _
                   & " FROM dbo.tblProcedureSecurityXref " _
                   & " WHERE tblProcedureSecurityXref.UserSecurityControl = " _
                   & UserSecurityControl & "))" _
                   & " AND  ProcedureHasAlert = 1" _
                   & " AND (tblProcedureList.ProcedureControl IN " _
                   & " (SELECT ProcedureControl " _
                   & " FROM dbo.tblProcAlertUserXref " _
                   & " WHERE tblProcAlertUserXref.UserSecurityControl = " _
                   & UserSecurityControl & "))" _
               & " Order By ProcedureName"

            Dim oQRet As Core.Data.QueryResult = oQuery.ExecuteWithFill(strSQL)
            If Not oQRet.Exception Is Nothing Then
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NotReadProcByUser"}, New FaultReason("E_DataAccessError"))
            End If
            If Not oQRet.Data Is Nothing AndAlso oQRet.Data.Rows.Count > 0 Then
                Dim oRow As DataRow = oQRet.Data.Rows(0)
                With tblProcedureLists
                    .ProcedureControl = DTran.getDataRowValue(oRow, "ProcedureControl", 0)
                    .ProcedureName = DTran.getDataRowString(oRow, "ProcedureName", "Unnamed")
                    .ProcedureDescription = DTran.getDataRowString(oRow, "ProcedureDescription", "")
                    .ProcAlertUserXrefShowPopup = DTran.getDataRowValue(oRow, "ProcAlertUserXrefShowPopup", 0)
                    .ProcAlertUserXrefSendEmail = DTran.getDataRowValue(oRow, "ProcAlertUserXrefSendEmail", 0)
                    If .ProcAlertUserXrefShowPopup Then .strPopup = "Show Popup" Else .strPopup = "Hide Popup"
                    If .ProcAlertUserXrefSendEmail Then .strEmail = "Send Email" Else .strEmail = "Hide Email"
                End With
            End If
            If Not tblProcedureLists Is Nothing Then
                Return tblProcedureLists
            Else
                Return Nothing
            End If
        Catch ex As FaultException
            Throw
        Catch ex As SqlException
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As InvalidOperationException
            SaveAppError(ex.Message, Me.Parameters)
            'its ok if there is no data.
            'Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
        Catch ex As Exception
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            oQuery = Nothing
        End Try
        Return Nothing
    End Function


    Public Function GetUnSubscribedSystemAlertsByUser(ByVal UserSecurityControl As Integer) As DTO.tblProcedureList()
        'Create a data connection 
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblProcedureLists As New List(Of DTO.tblProcedureList)
        Try
            Dim strSQL As String = " Select " _
                & " tblProcedureList.ProcedureControl, tblProcedureList.ProcedureName, tblProcedureList.ProcedureDescription " _
                & " FROM tblProcedureList " _
                & " WHERE (ProcedureControl NOT IN " _
                & " (SELECT ProcedureControl " _
                    & " FROM dbo.tblProcedureSecurityXref " _
                    & " WHERE tblProcedureSecurityXref.UserSecurityControl = " _
                    & UserSecurityControl & "))" _
                    & " AND  ProcedureHasAlert = 1" _
                    & " AND (ProcedureControl NOT IN " _
                    & " (SELECT ProcedureControl " _
                    & " FROM dbo.tblProcAlertUserXref " _
                    & " WHERE tblProcAlertUserXref.UserSecurityControl = " _
                    & UserSecurityControl & "))" _
                & " Order By ProcedureName"

            Dim oQRet As Ngl.Core.Data.QueryResult = oQuery.ExecuteWithFill(strSQL)
            If Not oQRet.Exception Is Nothing Then
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NotReadProcByUser"}, New FaultReason("E_DataAccessError"))
            End If
            If Not oQRet.Data Is Nothing AndAlso oQRet.Data.Rows.Count > 0 Then
                For Each oRow As System.Data.DataRow In oQRet.Data.Rows
                    Dim oProcedure As New DTO.tblProcedureList
                    With oProcedure
                        .ProcedureControl = DTran.getDataRowValue(oRow, "ProcedureControl", 0)
                        .ProcedureName = DTran.getDataRowString(oRow, "ProcedureName", "Unnamed")
                        .ProcedureDescription = DTran.getDataRowString(oRow, "ProcedureDescription", "")
                    End With
                    tblProcedureLists.Add(oProcedure)
                Next
            End If
            If Not tblProcedureLists Is Nothing AndAlso tblProcedureLists.Count > 0 Then
                Return tblProcedureLists.ToArray()
            Else
                Return Nothing
            End If
        Catch ex As FaultException
            Throw
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As InvalidOperationException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            'its ok if there is no data.
            'Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            oQuery = Nothing
        End Try
        Return Nothing
    End Function

    'Modified By LVV on 11/28/16 for v-7.0.5.110 Subscription Alert Changes
    'Public Function GetSubscribedSystemAlertsByUser(ByVal UserSecurityControl As Integer) As DTO.tblProcedureList()
    '    'Create a data connection 
    '    Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
    '    Dim tblProcedureLists As New List(Of DTO.tblProcedureList)
    '    Try
    '        Dim strSQL As String = " Select " _
    '           & " tblProcedureList.ProcedureControl, tblProcedureList.ProcedureName, tblProcedureList.ProcedureDescription " _
    '           & " FROM tblProcedureList " _
    '           & " WHERE (ProcedureControl NOT IN " _
    '           & " (SELECT ProcedureControl " _
    '               & " FROM dbo.tblProcedureSecurityXref " _
    '               & " WHERE tblProcedureSecurityXref.UserSecurityControl = " _
    '               & UserSecurityControl & "))" _
    '               & " AND  ProcedureHasAlert = 1" _
    '               & " AND (ProcedureControl IN " _
    '               & " (SELECT ProcedureControl " _
    '               & " FROM dbo.tblProcAlertUserXref " _
    '               & " WHERE tblProcAlertUserXref.UserSecurityControl = " _
    '               & UserSecurityControl & "))" _
    '           & " Order By ProcedureName"
    '        Dim oQRet As Ngl.Core.Data.QueryResult = oQuery.ExecuteWithFill(strSQL)
    '        If Not oQRet.Exception Is Nothing Then
    '            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NotReadProcByUser"}, New FaultReason("E_DataAccessError"))
    '        End If
    '        If Not oQRet.Data Is Nothing AndAlso oQRet.Data.Rows.Count > 0 Then
    '            For Each oRow As System.Data.DataRow In oQRet.Data.Rows
    '                Dim oProcedure As New DTO.tblProcedureList
    '                With oProcedure
    '                    .ProcedureControl = DTran.getDataRowValue(oRow, "ProcedureControl", 0)
    '                    .ProcedureName = DTran.getDataRowString(oRow, "ProcedureName", "Unnamed")
    '                    .ProcedureDescription = DTran.getDataRowString(oRow, "ProcedureDescription", "")
    '                End With
    '                tblProcedureLists.Add(oProcedure)
    '            Next
    '        End If
    '        If Not tblProcedureLists Is Nothing AndAlso tblProcedureLists.Count > 0 Then
    '            Return tblProcedureLists.ToArray()
    '        Else
    '            Return Nothing
    '        End If
    '    Catch ex As FaultException
    '        Throw
    '    Catch ex As System.Data.SqlClient.SqlException
    '        Utilities.SaveAppError(ex.Message, Me.Parameters)
    '        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
    '    Catch ex As InvalidOperationException
    '        Utilities.SaveAppError(ex.Message, Me.Parameters)
    '        'its ok if there is no data.
    '        'Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
    '    Catch ex As Exception
    '        Utilities.SaveAppError(ex.Message, Me.Parameters)
    '        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
    '    Finally
    '        oQuery = Nothing
    '    End Try
    '    Return Nothing
    'End Function


    ''' <summary>
    ''' GetSubscribedSystemAlertsByUser
    ''' </summary>
    ''' <param name="UserSecurityControl"></param>
    ''' <returns></returns>
    ''' <remarks>Modified By LVV on 11/28/16 for v-7.0.5.110 Subscription Alert Changes</remarks>
    Public Function GetSubscribedSystemAlertsByUser(ByVal UserSecurityControl As Integer) As DTO.tblProcedureList()
        'Create a data connection 
        Dim oQuery As New Core.Data.Query(ConnectionString)
        Dim tblProcedureLists As New List(Of DTO.tblProcedureList)
        Try
            Dim strSQL As String = " Select " _
               & " tblProcedureList.ProcedureControl, tblProcedureList.ProcedureName, tblProcedureList.ProcedureDescription, tblProcAlertUserXref.ProcAlertUserXrefShowPopup,tblProcAlertUserXref.ProcAlertUserXrefSendEmail " _
               & " FROM tblProcedureList " _
               & " inner join dbo.tblProcAlertUserXref on tblProcedureList.ProcedureControl = dbo.tblProcAlertUserXref.ProcedureControl AND dbo.tblProcAlertUserXref.UserSecurityControl = " & UserSecurityControl & " " _
               & " WHERE (tblProcedureList.ProcedureControl NOT IN " _
               & " (SELECT ProcedureControl " _
                   & " FROM dbo.tblProcedureSecurityXref " _
                   & " WHERE tblProcedureSecurityXref.UserSecurityControl = " _
                   & UserSecurityControl & "))" _
                   & " AND  ProcedureHasAlert = 1" _
                   & " AND (tblProcedureList.ProcedureControl IN " _
                   & " (SELECT ProcedureControl " _
                   & " FROM dbo.tblProcAlertUserXref " _
                   & " WHERE tblProcAlertUserXref.UserSecurityControl = " _
                   & UserSecurityControl & "))" _
               & " Order By ProcedureName"

            Dim oQRet As Core.Data.QueryResult = oQuery.ExecuteWithFill(strSQL)
            If Not oQRet.Exception Is Nothing Then
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NotReadProcByUser"}, New FaultReason("E_DataAccessError"))
            End If
            If Not oQRet.Data Is Nothing AndAlso oQRet.Data.Rows.Count > 0 Then
                For Each oRow As DataRow In oQRet.Data.Rows
                    Dim oProcedure As New DTO.tblProcedureList
                    With oProcedure
                        .ProcedureControl = DTran.getDataRowValue(oRow, "ProcedureControl", 0)
                        .ProcedureName = DTran.getDataRowString(oRow, "ProcedureName", "Unnamed")
                        .ProcedureDescription = DTran.getDataRowString(oRow, "ProcedureDescription", "")
                        .ProcAlertUserXrefShowPopup = DTran.getDataRowValue(oRow, "ProcAlertUserXrefShowPopup", 0)
                        .ProcAlertUserXrefSendEmail = DTran.getDataRowValue(oRow, "ProcAlertUserXrefSendEmail", 0)
                        If .ProcAlertUserXrefShowPopup Then .strPopup = "Show Popup" Else .strPopup = "Hide Popup"
                        If .ProcAlertUserXrefSendEmail Then .strEmail = "Send Email" Else .strEmail = "Hide Email"
                    End With
                    tblProcedureLists.Add(oProcedure)
                Next
            End If
            If Not tblProcedureLists Is Nothing AndAlso tblProcedureLists.Count > 0 Then
                Return tblProcedureLists.ToArray()
            Else
                Return Nothing
            End If
        Catch ex As FaultException
            Throw
        Catch ex As SqlException
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As InvalidOperationException
            SaveAppError(ex.Message, Me.Parameters)
            'its ok if there is no data.
            'Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
        Catch ex As Exception
            SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            oQuery = Nothing
        End Try
        Return Nothing
    End Function

    Public Function IsUserSubscribedToSystemAlert(ByVal UserSecurityControl As Integer, ByVal ProcedureControl As Integer, ByVal compControl As Integer) As Boolean
        'Create a data connection 
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblProcedureLists As New List(Of DTO.tblProcedureList)
        Dim blnRet As Boolean = False
        Try
            Dim strSQL As String = ""
            If compControl <> 0 Then
                'check if this user is allowed to see this company information

            End If
            strSQL = " Select Top 1" _
               & " tblProcedureList.ProcedureControl " _
               & " FROM tblProcedureList " _
               & " WHERE ProcedureControl = " & ProcedureControl _
                   & " AND " _
                   & "(ProcedureControl NOT IN " _
                   & " (SELECT ProcedureControl " _
                   & " FROM dbo.tblProcedureSecurityXref " _
                   & " WHERE tblProcedureSecurityXref.UserSecurityControl = " _
                   & UserSecurityControl & "))" _
                   & " AND  ProcedureHasAlert = 1" _
                   & " AND (ProcedureControl IN " _
                   & " (SELECT ProcedureControl " _
                   & " FROM dbo.tblProcAlertUserXref " _
                   & " WHERE tblProcAlertUserXref.UserSecurityControl = " _
                   & UserSecurityControl & "))" _
                   & " AND (" & compControl & " = 0 " _
                   & " OR " _
                   & " ( " _
                   & " (" _
                   & " isnull((SELECT top 1 isnull(dbo.tblUserAdmin.UserAdminCompControl,0) FROM dbo.tblUserAdmin Where dbo.tblUserAdmin.UserSecurityControl = " & UserSecurityControl & "),0) > 0 " _
                   & " AND " _
                   & compControl & " In (SELECT dbo.tblUserAdmin.UserAdminCompControl FROM dbo.tblUserAdmin	Where dbo.tblUserAdmin.UserSecurityControl = " & UserSecurityControl & ") " _
                   & "  ) " _
                   & "  OR " _
                   & " isnull((SELECT top 1 isnull(dbo.tblUserAdmin.UserAdminCompControl,0) FROM dbo.tblUserAdmin Where dbo.tblUserAdmin.UserSecurityControl = " & UserSecurityControl & "),0) = 0 " _
                   & " ))"

            Dim oQRet As Ngl.Core.Data.QueryResult = oQuery.ExecuteWithFill(strSQL)
            If Not oQRet.Exception Is Nothing Then
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NotReadUsersByAlert"}, New FaultReason("E_DataAccessError"))
            End If
            If Not oQRet.Data Is Nothing AndAlso oQRet.Data.Rows.Count > 0 Then blnRet = True
            Return blnRet
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
        Return blnRet
    End Function

    ''' <summary>
    ''' Retures true if the user users has selected to recieve popup notifications about alerts.  
    ''' Company level restrictions are applied.
    ''' </summary>
    ''' <param name="UserSecurityControl"></param>
    ''' <param name="ProcedureControl"></param>
    ''' <param name="compControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.0 on 12/12/2016
    ''' Modified By LVV on 9/11/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Public Function ShowPopupForAlert(ByVal UserSecurityControl As Integer, ByVal ProcedureControl As Integer, ByVal compControl As Integer) As Boolean
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Return ShowPopupForAlert(db, UserSecurityControl, ProcedureControl, compControl)
        End Using
    End Function

    ''' <summary>
    ''' Retures true if the user users has selected to recieve popup notifications about alerts.  
    ''' Company level restrictions are applied.
    ''' </summary>
    ''' <param name="UserSecurityControl"></param>
    ''' <param name="ProcedureControl"></param>
    ''' <param name="compControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.0 on 12/12/2016
    ''' Modified By LVV on 9/11/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Private Function ShowPopupForAlert(ByRef db As NGLMASSecurityDataContext, ByVal UserSecurityControl As Integer, ByVal ProcedureControl As Integer, ByVal compControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        Try
            Dim results As LTS.spAlertShowPopupMsgResult = db.spAlertShowPopupMsg(ProcedureControl, UserSecurityControl, compControl).FirstOrDefault()
            If Not results Is Nothing Then
                blnRet = results.ShowMsg
            End If
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As InvalidOperationException
            'Utilities.SaveAppError(ex.Message, Me.Parameters)
            'Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Return False
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        End Try
        Return blnRet
    End Function


    ''' <summary>
    ''' Returns a list of User Security records for users that have selected to recieve Email Notificaitons about Alerts.
    ''' Company level restrictions are applied.
    ''' </summary>
    ''' <param name="ProcedureControl"></param>
    ''' <param name="compControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.0 on 12/12/2016
    ''' </remarks>
    Public Function UsersToEmailAlert(ByVal ProcedureControl As Integer, ByVal compControl As Integer) As DTO.tblUserSecurity()
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim tblUserSecuritys() As DTO.tblUserSecurity = (
                    From d In db.spAlertUsersToEmail(ProcedureControl, compControl)
                    Select New DTO.tblUserSecurity With {.UserSecurityControl = d.UserSecurityControl,
                                                         .UserName = d.UserName,
                                                         .UserDepartment = d.UserDepartment,
                                                         .UserEmail = d.UserEmail,
                                                         .UserFirstName = d.UserFirstName,
                                                         .UserFriendlyName = d.UserFriendlyName,
                                                         .UserLastName = d.UserLastName,
                                                         .UserMiddleIn = d.UserMiddleIn,
                                                         .UserPhoneCell = d.UserPhoneCell,
                                                         .UserPhoneHome = d.UserPhoneHome,
                                                         .UserPhoneWork = d.UserPhoneWork,
                                                         .UserWorkExt = d.UserWorkExt,
                                                         .UseAuthCode = d.UseAuthCode,
                                                         .UserTitle = d.UserTitle,
                                                         .UserRemotePassword = d.UserRemotePassword,
                                                         .UserUserGroupsControl = d.UserUserGroupsControl,
                                                         .NEXTrackOnly = d.NEXTrackOnly,
                                                         .UserUpdated = d.UserUpdated.ToArray()}).ToArray()

                Return tblUserSecuritys
            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                'Utilities.SaveAppError(ex.Message, Me.Parameters)
                'Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
                Return Nothing
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
            Return Nothing
        End Using
    End Function


    Public Function UsersSubscribedToSystemAlert(ByVal ProcedureControl As Integer, ByVal compControl As Integer) As DTO.tblUserSecurity()
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim tblUserSecuritys() As DTO.tblUserSecurity = (
                    From d In db.spGetSubscribedUsersForAlert(ProcedureControl, compControl)
                    Select New DTO.tblUserSecurity With {.UserSecurityControl = d.UserSecurityControl,
                                                         .UserName = d.UserName,
                                                         .UserDepartment = d.UserDepartment,
                                                         .UserEmail = d.UserEmail,
                                                         .UserFirstName = d.UserFirstName,
                                                         .UserFriendlyName = d.UserFriendlyName,
                                                         .UserLastName = d.UserLastName,
                                                         .UserMiddleIn = d.UserMiddleIn,
                                                         .UserPhoneCell = d.UserPhoneCell,
                                                         .UserPhoneHome = d.UserPhoneHome,
                                                         .UserPhoneWork = d.UserPhoneWork,
                                                         .UserWorkExt = d.UserWorkExt,
                                                         .UseAuthCode = d.UseAuthCode,
                                                         .UserTitle = d.UserTitle,
                                                         .UserRemotePassword = d.UserRemotePassword,
                                                         .UserUserGroupsControl = d.UserUserGroupsControl,
                                                         .NEXTrackOnly = d.NEXTrackOnly,
                                                         .UserUpdated = d.UserUpdated.ToArray()}).ToArray()
                Return tblUserSecuritys
            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                'Utilities.SaveAppError(ex.Message, Me.Parameters)
                'Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
                Return Nothing
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
            Return Nothing
        End Using
    End Function

    Public Function UnSubscribeSystemAlertByUser(ByVal ProcedureControl As Integer, ByVal UserSecurityControl As Integer) As DTO.tblProcedureList()
        'Create a data connection 
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblProcedureLists As New List(Of DTO.tblProcedureList)
        Try
            Dim strSQL As String = "DELETE FROM  tblProcAlertUserXref " & " WHERE (tblProcAlertUserXref.ProcedureControl = " & ProcedureControl & " AND tblProcAlertUserXref.UserSecurityControl = " & UserSecurityControl & ")"
            If oQuery.executeSQLQuery(oCon, strSQL, 1) Then
                Return GetSubscribedSystemAlertsByUser(UserSecurityControl)
            Else
                tblProcedureLists.Add(New DTO.tblProcedureList)
                Return tblProcedureLists.ToArray
            End If
        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception
                'do nothing
            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception
                'do nothing
            End Try
        End Try
        Return Nothing
    End Function

    Public Sub DeletetblProcAlertUserXref(ByVal ProcAlertUserXrefControl As Integer)
        'Create a data connection 
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Try
            Dim strSQL As String = "DELETE FROM  tblProcAlertUserXref " & " WHERE (tblProcAlertUserXref.ProcAlertUserXrefControl = " & ProcAlertUserXrefControl & ")"
            oQuery.executeSQLQuery(oCon, strSQL, 0)
        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception
                'do nothing
            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception
                'do nothing
            End Try
        End Try
    End Sub

    Public Function SubscribeSystemAlertByUser(ByVal ProcedureControl As Integer, ByVal UserSecurityControl As Integer) As DTO.tblProcedureList()
        'Create a data connection 
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblProcedureLists As New List(Of DTO.tblProcedureList)
        Try
            Dim strSQL As String = "insert into  dbo.tblProcAlertUserXref " _
                & "(UserSecurityControl ,ProcedureControl)" _
                & " values " _
                & " (" & UserSecurityControl & "," & ProcedureControl & " )"

            If oQuery.executeSQLQuery(oCon, strSQL, 1) Then
                Return GetSubscribedSystemAlertsByUser(UserSecurityControl)
            Else
                tblProcedureLists.Add(New DTO.tblProcedureList)
                Return tblProcedureLists.ToArray
            End If
        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try
        Return Nothing
    End Function

    Public Function SubscribeAllSystemAlertsByUser(ByVal UserSecurityControl As Integer) As DTO.tblProcedureList()
        'Create a data connection 
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblProcedureLists As New List(Of DTO.tblProcedureList)
        Try
            Dim strSQL As String = "insert into  dbo.tblProcAlertUserXref  (UserSecurityControl, ProcedureControl) " _
               & " Select " & UserSecurityControl & ",ProcedureControl " _
               & " FROM dbo.tblProcedureList " _
               & " Where (ProcedureControl not in " _
               & " (Select ProcedureControl FROM dbo.tblProcedureSecurityXref Where UserSecurityControl = " & UserSecurityControl & "))" _
               & " AND  ProcedureHasAlert = 1" _
               & " AND (ProcedureControl NOT IN " _
               & " (SELECT ProcedureControl " _
               & " FROM dbo.tblProcAlertUserXref " _
               & " WHERE tblProcAlertUserXref.UserSecurityControl = " _
               & UserSecurityControl & "))"

            If oQuery.executeSQLQuery(oCon, strSQL, 1) Then
                Return GetSubscribedSystemAlertsByUser(UserSecurityControl)
            Else
                tblProcedureLists.Add(New DTO.tblProcedureList)
                Return tblProcedureLists.ToArray
            End If
        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try
        Return Nothing
    End Function

    Public Function UnSubscribeAllSystemAlertsByUser(ByVal UserSecurityControl As Integer) As DTO.tblProcedureList()
        'Create a data connection 
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblProcedureLists As New List(Of DTO.tblProcedureList)
        Try
            Dim strSQL As String = "Delete from dbo.tblProcAlertUserXref where UserSecurityControl = " & UserSecurityControl
            If oQuery.executeSQLQuery(oCon, strSQL, 1) Then
                Return GetSubscribedSystemAlertsByUser(UserSecurityControl)
            Else
                tblProcedureLists.Add(New DTO.tblProcedureList)
                Return tblProcedureLists.ToArray
            End If
        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Sets either ProcAlertUserXrefShowPopup or ProcAlertUserXrefSendEmail to blnValue based on blnIsPopup
    ''' for the provided ProcedureControl
    ''' </summary>
    ''' <param name="ProcedureControl"></param>
    ''' <param name="UserSecurityControl"></param>
    ''' <param name="blnIsPopup"></param>
    ''' <param name="blnValue"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 11/28/16 for v-7.0.5.110 Subscription Alert Changes
    ''' </remarks>
    Public Function SystemAlertSaveUserSettings(ByVal ProcedureControl As Integer, ByVal UserSecurityControl As Integer, ByVal blnIsPopup As Boolean, ByVal blnValue As Boolean) As DTO.tblProcedureList()
        'Create a data connection 
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblProcedureLists As New List(Of DTO.tblProcedureList)
        Try
            Dim strFieldName As String
            If blnIsPopup Then strFieldName = "dbo.tblProcAlertUserXref.ProcAlertUserXrefShowPopup" Else strFieldName = "dbo.tblProcAlertUserXref.ProcAlertUserXrefSendEmail"

            Dim strSQL As String = "update dbo.tblProcAlertUserXref set " _
                & strFieldName & " = " & Convert.ToInt32(blnValue) & " " _
                & "Where dbo.tblProcAlertUserXref.ProcedureControl = " & ProcedureControl & " " _
                & "And dbo.tblProcAlertUserXref.UserSecurityControl = " & UserSecurityControl & " "

            If oQuery.executeSQLQuery(oCon, strSQL, 1) Then
                Return GetSubscribedSystemAlertsByUser(UserSecurityControl)
            Else
                tblProcedureLists.Add(New DTO.tblProcedureList)
                Return tblProcedureLists.ToArray
            End If
        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Sets either ProcAlertUserXrefShowPopup or ProcAlertUserXrefSendEmail to blnValue based on blnIsPopup
    ''' for the provided all ProcedureControls for the user
    ''' </summary>
    ''' <param name="UserSecurityControl"></param>
    ''' <param name="blnIsPopup"></param>
    ''' <param name="blnValue"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 11/28/16 for v-7.0.5.110 Subscription Alert Changes
    ''' </remarks>
    Public Function SystemAlertSaveAllUserSettings(ByVal UserSecurityControl As Integer, ByVal blnIsPopup As Boolean, ByVal blnValue As Boolean) As DTO.tblProcedureList()
        'Create a data connection 
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim tblProcedureLists As New List(Of DTO.tblProcedureList)
        Try
            Dim strFieldName As String
            If blnIsPopup Then strFieldName = "dbo.tblProcAlertUserXref.ProcAlertUserXrefShowPopup" Else strFieldName = "dbo.tblProcAlertUserXref.ProcAlertUserXrefSendEmail"

            Dim strSQL As String = "update dbo.tblProcAlertUserXref set " _
               & strFieldName & " = " & Convert.ToInt32(blnValue) & " " _
               & "Where dbo.tblProcAlertUserXref.UserSecurityControl = " & UserSecurityControl

            If oQuery.executeSQLQuery(oCon, strSQL, 1) Then
                Return GetSubscribedSystemAlertsByUser(UserSecurityControl)
            Else
                tblProcedureLists.Add(New DTO.tblProcedureList)
                Return tblProcedureLists.ToArray
            End If
        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try
        Return Nothing
    End Function



    Public Sub UpdateAlertMessageClientUpdatedFlag(ByRef db As NGLMASSecurityDataContext, ByVal AlertMessageControl As Long)
        Try
            Dim nData = db.tblAlertMessages.Where(Function(x) x.AlertMessageControl = AlertMessageControl).FirstOrDefault()
            If Not nData Is Nothing AndAlso nData.AlertMessageControl = AlertMessageControl Then
                nData.AlertMessageClientUpdated = True
                db.SubmitChanges()
            End If
        Catch ex As Exception
            'ignore any errors when updating the flag (another thread may be updating the same data; this is self correcting) 
        End Try
    End Sub



    Public Function GetClientAlertMessages() As DTO.tblAlertMessage()
        Dim rettblAlertMessages() As DTO.tblAlertMessage
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                ' Attach the order
                Dim ntblAlertMessages As List(Of LTS.tblAlertMessage) = (From d In db.tblAlertMessages Where d.AlertMessageClientUpdated = False Select d).ToList
                rettblAlertMessages = (From d In ntblAlertMessages Where d.AlertMessageModDate > Date.Now.AddMinutes(-10) Select New DTO.tblAlertMessage _
                                                                    With {.AlertMessageControl = d.AlertMessageControl _
                                                                         , .AlertMessageBody = d.AlertMessageBody _
                                                                         , .AlertMessageCarrierControl = d.AlertMessageCarrierControl _
                                                                         , .AlertMessageCarrierNumber = d.AlertMessageCarrierNumber _
                                                                         , .AlertMessageClientUpdated = d.AlertMessageClientUpdated _
                                                                         , .AlertMessageCompControl = d.AlertMessageCompControl _
                                                                         , .AlertMessageCompNumber = d.AlertMessageCompNumber _
                                                                         , .AlertMessageDescription = d.AlertMessageDescription _
                                                                         , .AlertMessageEmailsSent = d.AlertMessageEmailsSent _
                                                                         , .AlertMessageModDate = d.AlertMessageModDate _
                                                                         , .AlertMessageModUser = d.AlertMessageModUser _
                                                                         , .AlertMessageName = d.AlertMessageName _
                                                                         , .AlertMessageNote1 = d.AlertMessageNote1 _
                                                                         , .AlertMessageNote2 = d.AlertMessageNote2 _
                                                                         , .AlertMessageNote3 = d.AlertMessageNote3 _
                                                                         , .AlertMessageNote4 = d.AlertMessageNote4 _
                                                                         , .AlertMessageNote5 = d.AlertMessageNote5 _
                                                                         , .AlertMessageProcedureControl = d.AlertMessageProcedureControl _
                                                                         , .AlertMessageSubject = d.AlertMessageSubject _
                                                                         , .AlertMessageUpdated = d.AlertMessageUpdated.ToArray()}).ToArray()
                For Each m In ntblAlertMessages
                    'Modified by RHR 2/25/14 we call a different sub routine for each message to 
                    'correct data conflict errors from other threads or processes
                    UpdateAlertMessageClientUpdatedFlag(db, m.AlertMessageControl)
                Next
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
            ' Return the selected records
            Return rettblAlertMessages
        End Using
    End Function

    Public Function GetEmailAlertMessages() As DTO.tblAlertMessage()
        Dim rettblAlertMessages() As DTO.tblAlertMessage
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                ' Attach the order
                Dim ntblAlertMessages As List(Of LTS.tblAlertMessage) = (From d In db.tblAlertMessages Where d.AlertMessageEmailsSent = False Select d).ToList
                rettblAlertMessages = (From d In ntblAlertMessages Select New DTO.tblAlertMessage _
                                                                    With {.AlertMessageControl = d.AlertMessageControl _
                                                                         , .AlertMessageBody = d.AlertMessageBody _
                                                                         , .AlertMessageCarrierControl = d.AlertMessageCarrierControl _
                                                                         , .AlertMessageCarrierNumber = d.AlertMessageCarrierNumber _
                                                                         , .AlertMessageClientUpdated = d.AlertMessageClientUpdated _
                                                                         , .AlertMessageCompControl = d.AlertMessageCompControl _
                                                                         , .AlertMessageCompNumber = d.AlertMessageCompNumber _
                                                                         , .AlertMessageDescription = d.AlertMessageDescription _
                                                                         , .AlertMessageEmailsSent = d.AlertMessageEmailsSent _
                                                                         , .AlertMessageModDate = d.AlertMessageModDate _
                                                                         , .AlertMessageModUser = d.AlertMessageModUser _
                                                                         , .AlertMessageName = d.AlertMessageName _
                                                                         , .AlertMessageNote1 = d.AlertMessageNote1 _
                                                                         , .AlertMessageNote2 = d.AlertMessageNote2 _
                                                                         , .AlertMessageNote3 = d.AlertMessageNote3 _
                                                                         , .AlertMessageNote4 = d.AlertMessageNote4 _
                                                                         , .AlertMessageNote5 = d.AlertMessageNote5 _
                                                                         , .AlertMessageProcedureControl = d.AlertMessageProcedureControl _
                                                                         , .AlertMessageSubject = d.AlertMessageSubject _
                                                                         , .AlertMessageUpdated = d.AlertMessageUpdated.ToArray()}).ToArray()
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
            ' Return the selected records
            Return rettblAlertMessages
        End Using
    End Function

    Public Sub ConfirmEmailAlertMessageIsSent(ByVal alertMessageControl As Long)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                ' Attach the order
                Dim ntblAlertMessage As LTS.tblAlertMessage = (From d In db.tblAlertMessages Where d.AlertMessageControl = alertMessageControl Select d).FirstOrDefault()
                If ntblAlertMessage IsNot Nothing Then
                    ntblAlertMessage.AlertMessageEmailsSent = True
                    ntblAlertMessage.AlertMessageModDate = Date.Now()
                    ntblAlertMessage.AlertMessageModUser = If(String.IsNullOrWhiteSpace(Parameters.UserName), Parameters.UserName, "System")
                    Try
                        db.SubmitChanges()
                    Catch ex As SqlException
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                    Catch conflictEx As ChangeConflictException
                        Utilities.SaveAppError(conflictEx.Message, Me.Parameters)
                        Try
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
                End If
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Gets the UserSecurityControl associated with the provided pUserName
    ''' </summary>
    ''' <param name="pUserName"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 11/28/16 for v-7.0.5.110 Subscription Alert Changes
    ''' </remarks>
    Public Function GetUserSecurityControlByUserName(ByVal pUserName As String) As Integer
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim UserSecControl = (
                    From d In db.tblUserSecurities
                    Where d.UserName = pUserName
                    Select d.UserSecurityControl).FirstOrDefault
                Return UserSecControl
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

#Region "Private Methods"

    Private Function GetReportParChanges(ByVal Reports As DTO.tblReportList, ByVal changeType As TrackingInfo) As List(Of LTS.tblReportPar)
        ' Tease out order details with specified change type.
        ' If Updated is null, set to byte[0] (for inserted items).
        Dim details As IEnumerable(Of LTS.tblReportPar) = (
          From d In Reports.tblReportPars
          Where d.TrackingState = changeType
          Select New LTS.tblReportPar With {.ReportParControl = d.ReportParControl,
                                            .ReportParReportControl = d.ReportParReportControl,
                                            .ReportParReportParTypeControl = d.ReportParReportParTypeControl,
                                            .ReportParName = d.ReportParName,
                                            .ReportParText = d.ReportParText,
                                            .ReportParSource = d.ReportParSource,
                                            .ReportParValueField = d.ReportParValueField,
                                            .ReportParTextField = d.ReportParTextField,
                                            .ReportParSortOrder = d.ReportParSortOrder,
                                            .ReportParApplyUserName = d.ReportParApplyUserName,
                                            .ReportParDefaultValue = d.ReportParDefaultValue,
                                            .ReportParUpdated = If(d.ReportParUpdated Is Nothing, New Byte() {}, d.ReportParUpdated)})
        Return details.ToList()
    End Function

#End Region


#Region "Content Management"

    ''' <summary>
    ''' Returns the LTS Records from the cmMenuTree table filtered by
    ''' UserSecurityControl. If UserSecurityControl is 0 then returns all
    ''' records in the cmMenuTree table
    ''' </summary>
    ''' <param name="UserSecControl"></param>
    ''' <returns>LTS.cmMenuTree()</returns>
    ''' <remarks>
    ''' Added By LVV on 12/20/16 for v-8.0 Content Management Tables
    ''' </remarks>
    Public Function getMenuTreeData(ByVal UserSecControl As Integer) As LTS.cmMenuTree()
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                If UserSecControl = 0 Then
                    'TODO return the -1 records
                    'If USC = 0 return only a link to the homepage (this is so it won't delete records)
                    'Return db.cmMenuTrees.Where(Function(x) x.MenuTreeUserSecurityControl = 0 AndAlso x.MenuTreeLinkPageControl = 20).ToArray()
                    Return db.cmMenuTrees.Where(Function(x) x.MenuTreeUserSecurityControl = -1).ToArray()
                End If
                Dim oTable = db.cmMenuTrees
                Dim oMenuTree0 As New List(Of LTS.cmMenuTree)
                Dim oMenuTreeUSC As New List(Of LTS.cmMenuTree)
                Dim oMT0Auth As New List(Of LTS.cmMenuTree)
                Dim ModDate = Date.Now
                Dim ModUser = "System"

                'Get all menu tree objects using 0 as the user security control
                oMenuTree0 = db.cmMenuTrees.Where(Function(x) x.MenuTreeUserSecurityControl = 0).ToList()
                'Get all menu tree objects using USC as filter
                oMenuTreeUSC = db.cmMenuTrees.Where(Function(x) x.MenuTreeUserSecurityControl = UserSecControl).ToList()
                'create a new sp based on spNetCheckFormSecurityWCF but params are USC and the PageControl
                'for each menu object use the provided usersecurity control to call spNetCheckFormSecurityWCF
                'inside for each - if the user has permission to access the page add the menu item to menu tree
                For Each z In oMenuTree0
                    Dim a = db.spNetCheckFormSecurity365(UserSecControl, z.MenuTreeLinkPageControl).FirstOrDefault()
                    If a.Column1 = 0 Then
                        'Not authorized to view that page - remove from collection
                        If Not oMenuTreeUSC Is Nothing AndAlso oMenuTreeUSC.Count > 0 Then
                            If oMenuTreeUSC.Any(Function(x) x.MenuTreeLinkPageControl = z.MenuTreeLinkPageControl) Then
                                Dim r = oMenuTreeUSC.Where(Function(x) x.MenuTreeLinkPageControl = z.MenuTreeLinkPageControl).FirstOrDefault()
                                If Not r Is Nothing Then
                                    oTable.DeleteOnSubmit(r)
                                    oMenuTreeUSC.Remove(r)
                                End If
                            End If
                        End If
                        'oMenuTree0.Remove(z)
                    Else
                        'Authorized so add to new 0 collection (can't modify original because it breaks the for each loop)
                        oMT0Auth.Add(z)
                    End If
                Next
                'if the usc collection is empty start with 0 collection
                If oMenuTreeUSC Is Nothing Then
                    oMenuTreeUSC = New List(Of LTS.cmMenuTree)
                End If

                'oMT0Auth = oMT0Auth.OrderBy(Function(x) x.MenuTreeParentID).ToList()
                For Each z In oMT0Auth
                    If Not oMenuTreeUSC.Any(Function(x) x.MenuTreeLinkPageControl = z.MenuTreeLinkPageControl And x.MenuTreeName = z.MenuTreeName) Then
                        'Dim sq = 0
                        'If Not oMenuTreeUSC Is Nothing AndAlso oMenuTreeUSC.Count > 0 Then
                        '    sq = oMenuTreeUSC.Where(Function(x) x.MenuTreeParentID = z.MenuTreeParentID).Max(Function(x) x.MenuTreeSequenceNo) + 1
                        'Else
                        '    sq = z.MenuTreeSequenceNo
                        'End If

                        'Dim uscParentID = 0
                        'If z.MenuTreeParentID <> 0 Then
                        '    Dim parentName = oMT0Auth.Where(Function(x) x.MenuTreeControl = z.MenuTreeParentID).Select(Function(y) y.MenuTreeName).FirstOrDefault()
                        '    Dim r = oMenuTreeUSC.Where(Function(x) x.MenuTreeName = parentName).FirstOrDefault()
                        '    uscParentID = r.MenuTreeControl
                        'End If

                        Dim lts As New LTS.cmMenuTree
                        With lts
                            .MenuTreeName = z.MenuTreeName
                            .MenuTreeDesc = z.MenuTreeDesc
                            .MenuTreeCaption = z.MenuTreeCaption
                            .MenuTreeCaptionLocal = z.MenuTreeCaptionLocal
                            .MenuTreeLinkTo = z.MenuTreeLinkTo
                            .MenuTreeLinkPageControl = z.MenuTreeLinkPageControl
                            .MenuTreeExpanded = z.MenuTreeExpanded
                            .MenuTreeVisible = z.MenuTreeVisible
                            .MenuTreeUserSecurityControl = UserSecControl
                            .MenuTreeSequenceNo = z.MenuTreeSequenceNo 'sq
                            .MenuTreeParentID = 0
                            .MenuTreeImgSmall = z.MenuTreeImgSmall
                            .MenuTreeImgMedium = z.MenuTreeImgMedium
                            .MenuTreeImgLarge = z.MenuTreeImgLarge
                            .MenuTreeModDate = ModDate
                            .MenuTreeModUser = ModUser
                        End With
                        'oMenuTreeUSC.Add(lts)
                        oTable.InsertOnSubmit(lts)
                    End If
                Next

                db.SubmitChanges()

                Dim records = db.cmMenuTrees.Where(Function(x) x.MenuTreeUserSecurityControl = UserSecControl).ToArray()
                For Each z In oMT0Auth
                    Dim uscParentID = 0
                    'Modifie byr RHR for v-8.5.4.004 on 12/28/2023.
                    ' This code does not work as expected.  the value of r can be nothing
                    ' added code to test for nothing.  this menu will not be visible to 
                    ' the user.
                    '  In the test case dev user warehouse was requesting access to the carrier menu parent 6200
                    '   but the parent name (carrier) did not exist in records.
                    '  User does not have access to "Legal Entity Carrier Maint"
                    '  check the security settings for user warehouse
                    If z.MenuTreeParentID <> 0 Then
                        Dim parentName = oMT0Auth.Where(Function(x) x.MenuTreeControl = z.MenuTreeParentID).Select(Function(y) y.MenuTreeName).FirstOrDefault()
                        Dim r = records.Where(Function(x) x.MenuTreeName = parentName).FirstOrDefault()
                        If (Not r Is Nothing AndAlso r.MenuTreeControl <> 0) Then
                            uscParentID = r.MenuTreeControl
                            Dim lts = records.Where(Function(x) x.MenuTreeName = z.MenuTreeName).FirstOrDefault()
                            lts.MenuTreeParentID = uscParentID
                            'oTable.Attach(lts)
                        End If

                    End If
                Next
                'Modified by RHR for v-8.0 on 12/16/2017
                '  hide any menu parents with no children; parents are  identified by MenuTreeLinkPageControl of zero
                For Each mParent In records.Where(Function(x) x.MenuTreeLinkPageControl = 0)
                    If records.Any(Function(x) x.MenuTreeParentID = mParent.MenuTreeControl) Then
                        mParent.MenuTreeVisible = True
                    Else
                        mParent.MenuTreeVisible = False
                    End If
                Next

                db.SubmitChanges()

                Return db.cmMenuTrees.Where(Function(x) x.MenuTreeUserSecurityControl = UserSecControl).ToArray()
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

    Public Function testNewCode(ByVal index As Integer) As Boolean
        If index = 1 Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Insert new Menu Tree data or Update existing for user.  Should not be used where user control is zero.
    ''' </summary>
    ''' <param name="oRecord"></param>
    ''' <remarks>
    ''' Created by RHR for v-8.5 on 05/10/2021 -- new Insert of Update logic
    ''' </remarks>
    Public Sub InsertOrUpdateMenuTreeData(ByVal oRecord As LTS.cmMenuTree)

        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                If oRecord.MenuTreeControl = 0 Then
                    ' check the user id
                    If (oRecord.MenuTreeUserSecurityControl = 0) Then
                        oRecord.MenuTreeUserSecurityControl = Me.Parameters.UserControl
                    End If
                    'check for the parent ID
                    If (oRecord.MenuTreeParentID = 0) Then
                        oRecord.MenuTreeParentID = getFavoritesMenuForUser(oRecord.MenuTreeUserSecurityControl)
                    End If
                    'Insert
                    db.cmMenuTrees.InsertOnSubmit(oRecord)
                Else
                    'Update
                    db.cmMenuTrees.Attach(oRecord, True)
                End If

                db.SubmitChanges()
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
    End Sub

    Private Function getFavoritesMenuForUser(ByVal iUserID As Integer) As Integer
        Dim iMenueTreeControl As Integer = 0
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim oRecord As New LTS.cmMenuTree()
                Dim sMenuTreeName As String = "Favorites"
                If (db.cmMenuTrees.Any(Function(x) x.MenuTreeName = sMenuTreeName And x.MenuTreeUserSecurityControl = iUserID)) Then
                    iMenueTreeControl = db.cmMenuTrees.Where(Function(x) x.MenuTreeName = sMenuTreeName And x.MenuTreeUserSecurityControl = iUserID).Select(Function(y) y.MenuTreeControl).FirstOrDefault()
                Else
                    With oRecord
                        .MenuTreeName = "Favorites"
                        .MenuTreeDesc = "Favorites"
                        .MenuTreeCaption = "Favorites"
                        .MenuTreeCaptionLocal = "ENU=Favorites"
                        .MenuTreeLinkTo = "../Default.aspx"
                        .MenuTreeLinkPageControl = 20
                        .MenuTreeExpanded = 1
                        .MenuTreeVisible = 1
                        .MenuTreeUserSecurityControl = iUserID
                        .MenuTreeSequenceNo = 1
                        .MenuTreeParentID = 0
                        .MenuTreeImgSmall = ""
                        .MenuTreeImgMedium = ""
                        .MenuTreeImgLarge = ""
                        .MenuTreeModDate = Date.Now()
                        .MenuTreeModUser = Me.Parameters.UserName
                    End With
                    db.cmMenuTrees.InsertOnSubmit(oRecord)
                    db.SubmitChanges()
                    iMenueTreeControl = oRecord.MenuTreeControl
                End If

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
        Return iMenueTreeControl
    End Function

    ''' <summary>
    ''' Retreives MenuTree object by its MenuTreeControl
    ''' </summary>
    ''' <param name="menuTreeControl"></param>
    ''' <remarks>
    ''' Created by CHA on 05/28/2021 -
    ''' </remarks>
    Public Function getMenuTreeDataByMenuTreeControl(ByVal menuTreeControl As Integer) As LTS.cmMenuTree
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim dataEl As LTS.cmMenuTree
                dataEl = db.cmMenuTrees.Where(Function(x) x.MenuTreeControl = menuTreeControl).FirstOrDefault()
                Return dataEl
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
    End Function


    ''' <summary>
    ''' getPageDetailElements
    ''' </summary>
    ''' <param name="PageControl"></param>
    ''' <param name="UserSecControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 12/28/16 for v-8.0 Next Stop
    ''' Modified by RHR for v-8.0 on 11/07/2017
    '''   we now select user control 0 if usercontrol does not have a custom configuraiton for this page
    ''' </remarks>
    Public Function getPageDetailElements(ByVal PageControl As Integer, ByVal UserSecControl As Integer) As LTS.cmPageDetail()
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim oPageDet() As LTS.cmPageDetail
                If db.cmPageDetails.Any(Function(x) x.PageDetPageControl = PageControl AndAlso x.PageDetUserSecurityControl = UserSecControl) Then
                    oPageDet = db.cmPageDetails.Where(Function(x) x.PageDetPageControl = PageControl AndAlso x.PageDetUserSecurityControl = UserSecControl).ToArray()
                Else
                    oPageDet = db.cmPageDetails.Where(Function(x) x.PageDetPageControl = PageControl AndAlso x.PageDetUserSecurityControl = 0).ToArray()
                End If
                Return oPageDet
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
    ''' getElementFields
    ''' </summary>
    ''' <param name="PageDetControl"></param>
    ''' <param name="UserSecControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 12/28/16 for v-8.0 Next Stop
    ''' Modified by RHR for v-8.0 on 11/07/2017
    '''   we now select user control 0 if usercontrol does not have a custom configuraiton for this page
    ''' </remarks>
    Public Function getElementFields(ByVal PageDetControl As Integer, ByVal UserSecControl As Integer) As LTS.cmElementField()
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim oPageDet() As Integer
                If db.cmPageDetails.Any(Function(x) x.PageDetParentID = PageDetControl AndAlso x.PageDetUserSecurityControl = UserSecControl) Then
                    oPageDet = db.cmPageDetails.Where(Function(x) x.PageDetParentID = PageDetControl AndAlso x.PageDetUserSecurityControl = UserSecControl).Select(Function(x) x.PageDetElmtFieldControl).ToArray()
                Else
                    oPageDet = db.cmPageDetails.Where(Function(x) x.PageDetParentID = PageDetControl AndAlso x.PageDetUserSecurityControl = 0).Select(Function(x) x.PageDetElmtFieldControl).ToArray()
                End If
                Dim oElmtFields() As LTS.cmElementField
                oElmtFields = db.cmElementFields.Where(Function(x) oPageDet.Contains(x.ElmtFieldControl)).ToArray()
                Return oElmtFields
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
    ''' getDataElement
    ''' </summary>
    ''' <param name="DataElementControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 1/3/17 for v-8.0 Next Stop
    ''' </remarks>
    Public Function getDataElement(ByVal DataElementControl As Integer) As LTS.cmDataElement
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim oDataElmt As LTS.cmDataElement
                oDataElmt = db.cmDataElements.Where(Function(x) x.DataElmtControl = DataElementControl).FirstOrDefault()
                Return oDataElmt
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
    ''' Returns the Element Field that is the Primary Key for Data Element associated with the provided DataElementControl
    ''' </summary>
    ''' <param name="DataElementControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 1/12/17 for v-8.0 Next Stop
    ''' </remarks>
    Public Function getDataElementPrimaryKey(ByVal DataElementControl As Integer) As LTS.cmElementField
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim oEFPK As LTS.cmElementField
                oEFPK = db.cmElementFields.Where(Function(x) x.ElmtFieldDataElmtControl = DataElementControl And x.ElmtFieldPK = True).FirstOrDefault()
                Return oEFPK
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
    ''' getPageTemplate
    ''' </summary>
    ''' <param name="PageTemplateControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 1/10/17 for v-8.0 Next Stop
    ''' </remarks>
    Public Function getPageTemplate(ByVal PageTemplateControl As Integer) As LTS.cmPageTemplate
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim oPageTemplate As LTS.cmPageTemplate
                oPageTemplate = db.cmPageTemplates.Where(Function(x) x.PageTemplateControl = PageTemplateControl).FirstOrDefault()
                Return oPageTemplate
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
    ''' Returns the cmPageTemplate records assigned to the page in the cmPageTemplateXref table for the provided pagecontrol number
    ''' </summary>
    ''' <param name="PageControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.1 on 02/21/2018
    ''' </remarks>
    Public Function getPageTemplates(ByVal PageControl As Integer) As LTS.cmPageTemplate()
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim oPageTemplateXrefs As LTS.cmPageTemplateXref()
                oPageTemplateXrefs = db.cmPageTemplateXrefs.Where(Function(x) x.PTXPageControl = PageControl).ToArray()
                If oPageTemplateXrefs Is Nothing OrElse oPageTemplateXrefs.Count() < 1 Then
                    Return Nothing
                End If
                Dim intTemplateControls() As Integer = oPageTemplateXrefs.Select(Function(x) x.PTXPageTemplateControl).ToArray()
                Dim oPageTemplates() As LTS.cmPageTemplate
                oPageTemplates = db.cmPageTemplates.Where(Function(x) intTemplateControls.Contains(x.PageTemplateControl)).ToArray()
                Return oPageTemplates
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
    ''' Returns all the records from the cmPageTemplates table
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.1 on 02/21/2018
    ''' </remarks>
    Public Function getAllTemplates() As LTS.cmPageTemplate()
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Return db.cmPageTemplates.ToArray()
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

    Public Sub SaveMenuTreeNodeExpanded(ByVal MTC As Integer, ByVal Expanded As Boolean)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim oRecord = db.cmMenuTrees.Where(Function(x) x.MenuTreeControl = MTC).FirstOrDefault()
                If oRecord Is Nothing OrElse oRecord.MenuTreeControl = 0 Then Return
                oRecord.MenuTreeExpanded = Expanded
                oRecord.MenuTreeModDate = Date.Now
                oRecord.MenuTreeModUser = Parameters.UserName
                db.SubmitChanges()
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
    End Sub

    Public Sub RemoveMenuItem(ByVal MenuTreeControl As Integer)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim record = db.cmMenuTrees.Where(Function(x) x.MenuTreeControl = MenuTreeControl).FirstOrDefault()
                If record Is Nothing Then
                    Return
                End If
                db.cmMenuTrees.DeleteOnSubmit(record)
                record.MenuTreeModDate = Date.Now
                record.MenuTreeModUser = Parameters.UserName
                db.SubmitChanges()
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
    End Sub

    Public Sub ToggleMenuTreeVisibility(ByVal MenuTreeControl As Integer)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim record = db.cmMenuTrees.Where(Function(x) x.MenuTreeControl = MenuTreeControl).FirstOrDefault()
                If record Is Nothing Then
                    Return
                End If
                record.MenuTreeVisible = Not record.MenuTreeVisible
                record.MenuTreeModDate = Date.Now
                record.MenuTreeModUser = Parameters.UserName
                db.SubmitChanges()
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
    End Sub

    Public Sub DeleteMenuTreeByUserSecurityControl(ByVal MenuTreeUserSecurityControl As Integer)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim records = db.cmMenuTrees.Where(Function(x) x.MenuTreeUserSecurityControl = MenuTreeUserSecurityControl).ToArray()
                If records Is Nothing OrElse records.Count() < 1 Then
                    Return
                End If
                For Each item In records
                    If Not item.MenuTreeName.EndsWith("Favorites") Then
                        db.cmMenuTrees.DeleteOnSubmit(item)
                    End If
                Next
                db.SubmitChanges()
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
    End Sub

    Public Sub UpdateRootMenuTreePositionData(ByVal MenuTreeUserSecurityControl As Integer, ByVal MenuTreeControl As Integer, ByVal MenuTreeControlNewPosition As Integer)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim records = db.cmMenuTrees.Where(Function(x) x.MenuTreeParentID = 0 And x.MenuTreeUserSecurityControl = MenuTreeUserSecurityControl).ToArray()
                If records Is Nothing OrElse records.Count() < 1 Then
                    Return
                End If
                Dim menuTreeControlOldPosition = records.Where(Function(x) x.MenuTreeControl = MenuTreeControl).FirstOrDefault()?.MenuTreeSequenceNo
                If menuTreeControlOldPosition Is Nothing Then
                    Return
                End If
                For Each item In records
                    If item.MenuTreeControl = MenuTreeControl Then
                        item.MenuTreeSequenceNo = MenuTreeControlNewPosition
                    Else
                        If item.MenuTreeSequenceNo >= MenuTreeControlNewPosition AndAlso item.MenuTreeSequenceNo < menuTreeControlOldPosition Then
                            item.MenuTreeSequenceNo += 1
                        ElseIf item.MenuTreeSequenceNo <= MenuTreeControlNewPosition AndAlso item.MenuTreeSequenceNo > menuTreeControlOldPosition Then
                            item.MenuTreeSequenceNo -= 1
                        End If
                    End If
                    item.MenuTreeModDate = Date.Now
                    item.MenuTreeModUser = Parameters.UserName
                Next
                db.SubmitChanges()
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
    End Sub

    Public Sub UpdateMenuTreePositionData(ByVal MenuTreeParentID As Integer, ByVal MenuTreeControl As Integer, ByVal MenuTreeControlNewPosition As Integer)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim records = db.cmMenuTrees.Where(Function(x) x.MenuTreeParentID = MenuTreeParentID).ToArray()
                If records Is Nothing OrElse records.Count() < 1 Then
                    Return
                End If
                Dim menuTreeControlOldPosition = records.Where(Function(x) x.MenuTreeControl = MenuTreeControl).FirstOrDefault()?.MenuTreeSequenceNo
                If menuTreeControlOldPosition Is Nothing Then
                    Return
                End If
                For Each item In records
                    If item.MenuTreeControl = MenuTreeControl Then
                        item.MenuTreeSequenceNo = MenuTreeControlNewPosition
                    Else
                        If item.MenuTreeSequenceNo >= MenuTreeControlNewPosition AndAlso item.MenuTreeSequenceNo < menuTreeControlOldPosition Then
                            item.MenuTreeSequenceNo += 1
                        ElseIf item.MenuTreeSequenceNo <= MenuTreeControlNewPosition AndAlso item.MenuTreeSequenceNo > menuTreeControlOldPosition Then
                            item.MenuTreeSequenceNo -= 1
                        End If
                    End If
                    item.MenuTreeModDate = Date.Now
                    item.MenuTreeModUser = Parameters.UserName
                Next
                db.SubmitChanges()
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
    End Sub
#End Region

#Region "Azure Active Directory Authentication"

    Public Function authenticateUser(ByVal oParameters As WCFParameters) As WCFParameters

        Dim blnLogonRequired As Boolean = True
        Dim oUserSecurityAccessToken As NGLtblUserSecurityAccessTokenData
        'steps: 
        '1) Lookup user name  if not return Auth Required = true
        '2) Lookup SSOA account with SSOAName if no record create a new record and return Auth Required = true
        '3) Lookup Token if no token return Auth Required = true
        '4) check Token Expiration Date.  If expired return Auth Required = true
        '5) return Auth Required = false
        'spNetAuthenticateSSOASecurity

        'If Me.SourceClass = "NGLtblUserSecurityAccessTokenData" Then
        '    oUserSecurityAccessToken = Me
        'Else
        '    oUserSecurityAccessToken = New NGLtblUserSecurityAccessTokenData()
        '        oUserSecurityAccessToken.loadParameterSettings(oParameters)
        '    End If
        '    If oParameters.UseToken Then
        '        blnLogonRequired = Not oUserSecurityAccessToken.validateUserToken(oParameters)
        '    End If
        'If blnLogonRequired Then
        '    Dim strRetMsg As String = ""
        '    Dim intErrNbr As Integer
        '    oCmd.Parameters.AddWithValue("@UserName", oParameters.UserName)
        '    'We only provide a password if not using NGL Integrated sucurity, for integrated security the password in passed as NULL
        '    If oParameters.UserRemotePassword <> "@NGL_Integrated_Security_2011!@" Then
        '        oCmd.Parameters.AddWithValue("@UserRemotePassword", DTran.Encrypt(oParameters.UserRemotePassword, "NGL"))
        '    End If
        '    If Not oQuery.execNGLStoredProcedure(oCon, oCmd, "dbo.spNetCheckUserSecurity", 1, True, strRetMsg, intErrNbr) Then
        '        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = strRetMsg}, New FaultReason("E_AccessDenied"))
        '    End If
        '    'Update the token information
        '    oUserSecurityAccessToken.updateNGLToken(oParameters)
        'End If
        Return oParameters
    End Function

#End Region

#Region "TMS 365 Methods"

    Public Sub CreateUserSecuritySetting(ByVal oData As LTS.tblUserSecuritySetting)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            db.tblUserSecuritySettings.InsertOnSubmit(oData)
            Try
                db.SubmitChanges()
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
    End Sub

    Public Function GetUserSecuritySetting(ByVal UserControl As Integer) As LTS.tblUserSecuritySetting
        Dim oRet As LTS.tblUserSecuritySetting
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                oRet = db.tblUserSecuritySettings.Where(Function(x) x.USSUserSecurityControl = UserControl).FirstOrDefault()
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

    ''Public Sub CreateUserSecurityLegalEntity(ByVal oData As LTS.tblUserSecurityLegalEntity)
    ''    Using db As New NGLMASSecurityDataContext(ConnectionString)
    ''        db.tblUserSecurityLegalEntities.InsertOnSubmit(oData)
    ''        Try
    ''            db.SubmitChanges()
    ''        Catch ex As System.Data.SqlClient.SqlException
    ''            Utilities.SaveAppError(ex.Message, Me.Parameters)
    ''            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
    ''        Catch ex As InvalidOperationException
    ''            Utilities.SaveAppError(ex.Message, Me.Parameters)
    ''            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
    ''        Catch ex As Exception
    ''            Utilities.SaveAppError(ex.Message, Me.Parameters)
    ''            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
    ''        End Try
    ''    End Using
    ''End Sub

    ''Public Function UpdateUserSecurityLegalEntity(ByVal oData As LTS.tblUserSecurityLegalEntity) As Boolean
    ''    Using db As New NGLMASSecurityDataContext(ConnectionString)
    ''        Dim blnRet As Boolean = False
    ''        Try
    ''            'Get the record with the specified control number
    ''            Dim ltsUSLE = (From d In db.tblUserSecurityLegalEntities Where d.USLEUserSecurityControl = oData.USLEUserSecurityControl).FirstOrDefault()
    ''            If Not ltsUSLE Is Nothing Then
    ''                Try
    ''                    ltsUSLE.USLELEAdminControl = oData.USLELEAdminControl
    ''                    ltsUSLE.USLELegalEntity = oData.USLELegalEntity
    ''                    ltsUSLE.USLECompControl = oData.USLECompControl
    ''                    ltsUSLE.USLEModDate = oData.USLEModDate
    ''                    ltsUSLE.USLEModUser = oData.USLEModUser
    ''                    db.SubmitChanges()
    ''                    blnRet = True
    ''                Catch ex As Exception
    ''                    'Ignore errors when updating
    ''                    blnRet = False
    ''                End Try
    ''            End If
    ''            Return blnRet
    ''        Catch ex As System.Data.SqlClient.SqlException
    ''            Utilities.SaveAppError(ex.Message, Me.Parameters)
    ''            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
    ''        Catch ex As InvalidOperationException
    ''            Utilities.SaveAppError(ex.Message, Me.Parameters)
    ''            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
    ''        Catch ex As Exception
    ''            Utilities.SaveAppError(ex.Message, Me.Parameters)
    ''            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
    ''        End Try
    ''        Return blnRet
    ''    End Using
    ''End Function


    ''Public Function GetUserSecurityLegalEntity(ByVal UserControl As Integer) As LTS.tblUserSecurityLegalEntity
    ''    Using db As New NGLMASSecurityDataContext(ConnectionString)
    ''        Try
    ''            Return db.tblUserSecurityLegalEntities.Where(Function(x) x.USLEUserSecurityControl = UserControl).FirstOrDefault()
    ''        Catch ex As System.Data.SqlClient.SqlException
    ''            Utilities.SaveAppError(ex.Message, Me.Parameters)
    ''            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
    ''        Catch ex As InvalidOperationException
    ''            Utilities.SaveAppError(ex.Message, Me.Parameters)
    ''            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
    ''        Catch ex As Exception
    ''            Utilities.SaveAppError(ex.Message, Me.Parameters)
    ''            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
    ''        End Try
    ''        Return Nothing
    ''    End Using
    ''End Function

    ''' <summary>
    ''' Get the Single Sign on Account information for user
    ''' </summary>
    ''' <param name="userName"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.2.006 added tblUserSecurity.UserMustChangePassword property
    ''' Modified by RHR for v-8.5.4.005 on 03/28/2024 added timezone and culture
    ''' </remarks>
    Public Function NetGetSSOAccount(ByVal userName As String) As Models.SSOAccount
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim l = (From d In db.spNetGetSSOAccount(userName) Select d).FirstOrDefault()
                Dim acct As New Models.SSOAccount
                If Not l Is Nothing Then
                    With acct
                        .SSOAControl = l.SSOAControl
                        .SSOAName = l.SSOAName
                        .SSOADesc = l.SSOADesc
                        .AllowNonPrimaryComputers = l.AllowNonPrimaryComputers
                        .AllowPublicComputer = l.AllowPublicComputer
                        .SSOAClientID = l.SSOAClientID
                        .SSOALoginURL = l.SSOALoginURL
                        .SSOADataURL = l.SSOADataURL
                        .SSOARedirectURL = l.SSOARedirectURL
                        .SSOAClientSecret = l.SSOAClientSecret
                        .SSOAAuthCode = l.SSOAAuthCode
                        .SSOAAuthenticationRequired = l.SSOAAuthenticationRequired
                        .SSOAUserSecurityControl = l.SSOAUserSecurityControl
                        .SSOAUserName = l.SSOAUserName
                        .SSOAUserEmail = l.SSOAUserEmail
                        .UserFriendlyName = l.UserFriendlyName
                        .UserFirstName = l.UserFirstName
                        .UserLastName = l.UserLastName
                        .AuthenticationErrorCode = l.AuthenticationErrorCode
                        .AuthenticationErrorMessage = l.AuthenticationErrorMessage
                        .IsUserCarrier = l.IsCarrier
                        .UserCarrierControl = l.CarrierControl
                        .UserCarrierContControl = l.CarrierContControl
                        .UserLEControl = l.LEControl
                        .UserTheme365 = l.UserTheme365
                        .CatControl = If(l.CatControl, 0)
                        .UserMustChangePassword = If(l.UserMustChangePassword, False)
                        .UserCultureInfo = l.UserCultureInfo
                        .UserTimeZone = l.UserTimeZone
                    End With
                    Return acct
                End If
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
    ''' Get the single sign on account information for user
    ''' </summary>
    ''' <param name="iUserControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.2.006 added tblUserSecurity.UserMustChangePassword property
    ''' Modified by RHR for v-8.5.4.005 on 03/28/2024 added timezone and culture
    ''' </remarks>
    Public Function NetGetSSOAccount(ByVal iUserControl As Integer) As Models.SSOResults
        Dim ssoRes As New Models.SSOResults()
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'Dim iUserControl As Integer = Parameters.SSOAUserSecurityControl
                Dim userName As String = db.tblUserSecurities.Where(Function(x) x.UserSecurityControl = iUserControl).Select(Function(y) y.UserName).FirstOrDefault()
                Dim l = (From d In db.spNetGetSSOAccount(userName) Select d).FirstOrDefault()
                Dim acct As New Models.SSOAccount
                If Not l Is Nothing Then
                    With ssoRes
                        .SSOAControl = l.SSOAControl
                        .SSOAName = l.SSOAName
                        .SSOAClientID = l.SSOAClientID
                        .SSOALoginURL = l.SSOALoginURL
                        .SSOARedirectURL = l.SSOARedirectURL
                        .SSOAClientSecret = l.SSOAClientSecret
                        .SSOAAuthCode = l.SSOAAuthCode
                        .UserSecurityControl = l.SSOAUserSecurityControl
                        .UserName = l.SSOAUserName
                        .SSOAUserEmail = l.SSOAUserEmail
                        .UserFriendlyName = l.UserFriendlyName
                        .UserFirstName = l.UserFirstName
                        .UserLastName = l.UserLastName
                        .UserTheme365 = l.UserTheme365
                        .CatControl = l.CatControl
                        .UserMustChangePassword = If(l.UserMustChangePassword, False)
                        .UserTimeZone = l.UserTimeZone
                        .UserCultureInfo = l.UserCultureInfo
                    End With
                    Return ssoRes
                End If
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
    ''' Updates users Must Change Password flag.  This must be securred by the caller so that only legal entity administrators can execute this method 
    ''' </summary>
    ''' <param name="Activate"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.2.006 on 2023-02-09
    '''  updates the Must change password flag for all users for this users Legal Entity
    ''' </remarks>
    Public Function AllUsersMustChangePassword(ByVal Activate As Boolean) As Boolean
        'Get the usercontrol and the legal entity control from parameters
        Dim iUserSecurityControl As Integer = Parameters.UserControl
        Dim iLEControl As Integer = Parameters.UserLEControl
        Dim sToken As String
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                ' get the token
                sToken = db.tblUserSecurityAccessTokens.Where(Function(x) x.USATUserSecurityControl = iUserSecurityControl AndAlso x.USATSSOAControl = 1).Select(Function(y) y.USATToken).FirstOrDefault()
                Dim oRet As LTS.spReSecureUsersResult = db.spReSecureUsers(iLEControl, Activate, iUserSecurityControl, sToken).FirstOrDefault()
                If (Not oRet Is Nothing AndAlso oRet.ErrNumber <> 0) Then
                    Dim sDetails As New List(Of String)
                    sDetails.Add("Update All Users; must change password")
                    sDetails.Add(oRet.ErrNumber.ToString())
                    sDetails.Add(oRet.RetMsg)
                    Dim Message = SqlFaultInfo.getFaultMessage(SqlFaultInfo.FaultInfoMsgs.E_AccessDenied)
                    Dim Reason = SqlFaultInfo.getFaultReason(SqlFaultInfo.FaultReasons.E_ProcessProcedureFailure)
                    Dim Details = SqlFaultInfo.getFaultDetailsKey(SqlFaultInfo.FaultDetailsKey.E_SQLWarningDetails)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = Message, .Details = Details, .DetailsList = sDetails}, New FaultReason(Reason))
                End If
                Return True
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
            End Try
            Return False
        End Using
    End Function



    ''' <summary>
    ''' NGLLegacySignIn
    ''' </summary>
    ''' <param name="ssoaKeys"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.1 on 04/07/2018
    '''   Changed the logic used to generate a new token.  a new token is now only generated when a user is forced to log in.
    '''   This corrects the issue where modified tokens on open pages cause some token expired error messages because pages and
    '''   datasources cache the token when the page is build, especially on kendo data sources that do not use nested ajax.
    '''   the pages read localStorage.NGLvar1454 on start up, before the call to validate the user.
    ''' Modified by RHR for v-8.5.2.006 added tblUserSecurity.UserMustChangePassword property
    ''' Modified by RHR for v-8.5.4.005 on 03/28/2024 added timezone and culture
    ''' </remarks>
    Public Function NGLLegacySignIn(ByVal ssoaKeys As Models.NGLClass14) As Models.SSOResults
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim ssoRes As New Models.SSOResults
                'Verify the username
                Dim ssoAct = NetGetSSOAccount(ssoaKeys.NGLvar1455)
                Dim token As String = "" 'Modified by RHR for v-8.1 on 04/07/2018
                If Not ssoAct Is Nothing AndAlso ssoAct.SSOAUserSecurityControl <> 0 Then
                    Dim blnExists = False
                    'if token is not  blank -- write query to check token and if tru set blnExists to true and we are done 
                    If Not String.IsNullOrWhiteSpace(ssoaKeys.NGLvar1454) Then
                        If db.tblUserSecurityAccessTokens.Any(Function(x) x.USATSSOAControl = 1 AndAlso x.USATUserSecurityControl = ssoAct.SSOAUserSecurityControl AndAlso x.USATToken = ssoaKeys.NGLvar1454) Then
                            Dim dtexpires = db.tblUserSecurityAccessTokens.Where(Function(x) x.USATSSOAControl = 1 AndAlso x.USATUserSecurityControl = ssoAct.SSOAUserSecurityControl AndAlso x.USATToken = ssoaKeys.NGLvar1454).Select(Function(y) y.USATExpires).FirstOrDefault()
                            If Date.Now < dtexpires Then
                                blnExists = True
                                token = ssoaKeys.NGLvar1454 'Modified by RHR for v-8.1 on 04/07/2018 use existing token
                            End If
                        End If
                    End If
                    If Not blnExists Then
                        token = Guid.NewGuid().ToString 'now we create a new token on sign in
                        'Check the password
                        Dim sEncrypted As String = DTran.Encrypt(ssoaKeys.NGLvar1450, "NGL")
                        blnExists = db.tblUserSecurities.Any(Function(x) x.UserSecurityControl = ssoAct.SSOAUserSecurityControl AndAlso x.UserRemotePassword = sEncrypted)
                        If Not blnExists Then
                            'check for a legacy non-encrypted password
                            blnExists = db.tblUserSecurities.Any(Function(x) x.UserSecurityControl = ssoAct.SSOAUserSecurityControl AndAlso x.UserRemotePassword = ssoaKeys.NGLvar1450)
                            If blnExists Then
                                'we need to save and encrypt the password 
                                Try
                                    Me.tblUserSecuritySaveNewPassword(ssoAct.SSOAUserSecurityControl, ssoaKeys.NGLvar1450)
                                Catch ex As Exception
                                    'ignore any errors here just continue to login; the tblUserSecuritySaveNewPassword will save to the error to the app error table
                                End Try
                            End If
                        End If
                    End If
                    If blnExists Then
                        'tokens expire after 30 days there are 3600 seconds in an hours 
                        Dim intExpMinutes As Integer = 3600 * 720
                        Dim expDate As Date = Date.Now.AddSeconds(intExpMinutes)
                        'Modified by RHR for v-8.1 on 04/07/2018 old code to get new GUID for token was removed from here

                        With ssoRes
                            '.PrimaryComputer = 
                            '.PublicComputer = 
                            .SSOAControl = ssoAct.SSOAControl
                            .SSOAName = ssoAct.SSOAName
                            .SSOAClientID = ssoAct.SSOAClientID
                            .SSOALoginURL = ssoAct.SSOALoginURL
                            .SSOARedirectURL = ssoAct.SSOARedirectURL
                            .SSOAClientSecret = ssoAct.SSOAClientSecret
                            .SSOAAuthCode = ssoAct.SSOAAuthCode
                            .UserSecurityControl = ssoAct.SSOAUserSecurityControl
                            .UserName = ssoAct.SSOAUserName
                            .SSOAUserEmail = ssoAct.SSOAUserEmail
                            'SSOA missing data
                            'ssoAct.SSOAAuthenticationRequired
                            'ssoAct.SSOADataURL
                            'ssoAct.SSOADesc
                            'ssoAct.AllowNonPrimaryComputers
                            'ssoAct.AllowPublicComputer
                            'ssoAct.AuthenticationErrorCode
                            'ssoAct.AuthenticationErrorMessage
                            '.SSOAExpires = 
                            '.SSOAIssuedAtTime = ssoAct.
                            .USATToken = token 'This is the App Token we update it but is should stay the same for the current session.
                            '.USATUserID =
                            .UserFriendlyName = ssoAct.UserFriendlyName
                            .UserFirstName = ssoAct.UserFirstName
                            .UserLastName = ssoAct.UserLastName
                            .IsUserCarrier = ssoAct.IsUserCarrier
                            .UserCarrierControl = ssoAct.UserCarrierControl
                            .UserCarrierContControl = ssoAct.UserCarrierContControl
                            .UserLEControl = ssoAct.UserLEControl
                            .UserTheme365 = ssoAct.UserTheme365
                            .CatControl = ssoAct.CatControl
                            .UserMustChangePassword = ssoAct.UserMustChangePassword
                            .UserTimeZone = ssoAct.UserTimeZone
                            .UserCultureInfo = ssoAct.UserCultureInfo
                        End With

                        'modified by RHR for v-8.2 on 09/01/2018
                        'added SSOAExpiresMilli property convert to milliseconds to fully qualified javascript time stamp
                        Dim origin As DateTime = New DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                        ssoRes.SSOAExpires = Convert.ToInt32(Math.Floor((expDate.ToUniversalTime() - origin).TotalSeconds))
                        ssoRes.SSOAExpiresMilli = Convert.ToInt64(Math.Floor((expDate.ToUniversalTime() - origin).TotalMilliseconds))
                        'Save this updated App Token to the database
                        Dim oUSAT As New NGLtblUserSecurityAccessTokenData(Parameters)

                        Dim wcf = oUSAT.InsertOrUpdatetblUserSecurityAccessToken(ssoRes.UserSecurityControl, ssoRes.SSOAControl, ssoRes.USATToken, expDate, ssoRes.USATUserID)

                        If Not wcf Is Nothing Then
                            If Not wcf.Warnings Is Nothing AndAlso wcf.Warnings.Count > 0 Then
                                'Dim strMsg = wcf.concatWarnings()
                                'SaveAppError("NGLLegacySignIn Error: " & strMsg)
                            End If
                        End If
                    End If
                Else
                    Return Nothing
                End If
                Return ssoRes
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
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Returns nothing if the username and token are not valid
    ''' </summary>
    ''' <param name="AuthUN"></param>
    ''' <param name="AuthT"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 4/23/2019
    '''     used for Single Sign from Desktop App and other components 
    ''' Modified by RHR for v-8.5.2.006 added tblUserSecurity.UserMustChangePassword property
    ''' Modified by RHR for v-8.5.4.005 on 03/28/2024 added timezone and culture
    ''' </remarks>
    Public Function GetNGLLegacySSOAForUser(ByVal AuthUN As String, ByVal AuthT As String) As Models.SSOResults
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                If String.IsNullOrWhiteSpace(AuthT) Or String.IsNullOrWhiteSpace(AuthUN) Then Return Nothing
                Dim ssoRes As New Models.SSOResults
                'Verify the username
                Dim ssoAct = NetGetSSOAccount(AuthUN)
                Dim token As String = "" 'Modified by RHR for v-8.1 on 04/07/2018
                If ssoAct Is Nothing OrElse ssoAct.SSOAUserSecurityControl = 0 Then Return Nothing
                'Verify token
                If Not db.tblUserSecurityAccessTokens.Any(Function(x) x.USATSSOAControl = 1 AndAlso x.USATUserSecurityControl = ssoAct.SSOAUserSecurityControl AndAlso x.USATToken = AuthT) Then Return Nothing
                'Verify that the token has not expired
                Dim dtexpires = db.tblUserSecurityAccessTokens.Where(Function(x) x.USATSSOAControl = 1 AndAlso x.USATUserSecurityControl = ssoAct.SSOAUserSecurityControl AndAlso x.USATToken = AuthT).Select(Function(y) y.USATExpires).FirstOrDefault()
                If Date.Now >= dtexpires Then Return Nothing
                'tokens expire after 30 days there are 3600 seconds in an hours 
                Dim intExpMinutes As Integer = 3600 * 720
                Dim expDate As Date = Date.Now.AddSeconds(intExpMinutes)
                With ssoRes
                    .SSOAControl = ssoAct.SSOAControl
                    .SSOAName = ssoAct.SSOAName
                    .SSOAClientID = ssoAct.SSOAClientID
                    .SSOALoginURL = ssoAct.SSOALoginURL
                    .SSOARedirectURL = ssoAct.SSOARedirectURL
                    .SSOAClientSecret = ssoAct.SSOAClientSecret
                    .SSOAAuthCode = ssoAct.SSOAAuthCode
                    .UserSecurityControl = ssoAct.SSOAUserSecurityControl
                    .UserName = ssoAct.SSOAUserName
                    .SSOAUserEmail = ssoAct.SSOAUserEmail
                    .USATToken = AuthT
                    .UserFriendlyName = ssoAct.UserFriendlyName
                    .UserFirstName = ssoAct.UserFirstName
                    .UserLastName = ssoAct.UserLastName
                    .IsUserCarrier = ssoAct.IsUserCarrier
                    .UserCarrierControl = ssoAct.UserCarrierControl
                    .UserCarrierContControl = ssoAct.UserCarrierContControl
                    .UserLEControl = ssoAct.UserLEControl
                    .UserTheme365 = ssoAct.UserTheme365
                    .CatControl = ssoAct.CatControl
                    .UserMustChangePassword = ssoAct.UserMustChangePassword
                    .UserTimeZone = ssoAct.UserTimeZone
                    .UserCultureInfo = ssoAct.UserCultureInfo
                End With
                'added SSOAExpiresMilli property convert to milliseconds to fully qualified javascript time stamp
                Dim origin As DateTime = New DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                ssoRes.SSOAExpires = Convert.ToInt32(Math.Floor((expDate.ToUniversalTime() - origin).TotalSeconds))
                ssoRes.SSOAExpiresMilli = Convert.ToInt64(Math.Floor((expDate.ToUniversalTime() - origin).TotalMilliseconds))
                Return ssoRes
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
            End Try
            Return Nothing
        End Using
    End Function


    Public Function NGLLegacyValidateToken(ByVal ssoaKeys As Models.NGLClass14) As Boolean
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                If (Not ssoaKeys Is Nothing) AndAlso (ssoaKeys.NGLvar1452 > 0) AndAlso (Not String.IsNullOrWhiteSpace(ssoaKeys.NGLvar1454)) Then
                    'check if the data is valid so get the current data
                    Dim vSSoA = db.vSSOResults.Where(Function(x) x.UserSecurityControl = ssoaKeys.NGLvar1452).FirstOrDefault()
                    If vSSoA Is Nothing Then Return False
                    If ssoaKeys.NGLvar1454 <> vSSoA.USATToken Then Return False
                    If Not vSSoA.USATExpires.HasValue Then Return False
                    If vSSoA.USATExpires.Value < Date.Now() Then Return False
                    Return True
                Else
                    Return False
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
            End Try
            Return False
        End Using
    End Function

    ''' <summary>
    ''' uses the usercontrol (uc) to read the current values from the SSO Results 
    ''' does not require re-authentication so should be faster.  used to update the 
    ''' Client's GlobalSSOResultsByUser data when the in-memory variable is garbage collected 
    ''' </summary>
    ''' <param name="uc"></param>
    ''' <returns></returns>
    Public Function GetUpdatedSSOResults(ByVal uc As Integer) As LTS.vSSOResult
        Dim oRet As New LTS.vSSOResult
        If uc = 0 Then Return oRet
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                oRet = db.vSSOResults.Where(Function(x) x.UserSecurityControl = uc).FirstOrDefault()
            Catch ex As Exception
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
        End Using
        Return oRet
    End Function

    ''Commented out - DEPRECIATED
    ''Public Function GetUserCarrierControl(ByVal UserControl As Integer, ByRef strMsg As String) As Integer
    ''    Dim CarrierControl As Integer = 0
    ''    Using db As New NGLMASSecurityDataContext(ConnectionString)
    ''        Try
    ''            Dim uSec = db.tblUserSecurities.Where(Function(x) x.UserSecurityControl = UserControl).FirstOrDefault()
    ''            Dim intCat = db.tblUserGroups.Where(Function(x) x.UserGroupsControl = uSec.UserUserGroupsControl).Select(Function(x) x.UserGroupsUGCControl).FirstOrDefault()
    ''            If intCat = 2 OrElse intCat = 4 Then
    ''                'This user is a carrier or superuser
    ''                CarrierControl = db.tblUserSecurityCarriers.Where(Function(x) x.USCUserSecurityControl = UserControl).Select(Function(x) x.USCCarrierControl).FirstOrDefault()
    ''                If CarrierControl = 0 Then strMsg += "No Carrier associated with this User could be found. Make sure the User Settings are configured to link to a Carrier."
    ''            Else
    ''                strMsg += "The User does not have correct group permission to access this data."
    ''            End If
    ''            Return CarrierControl
    ''        Catch ex As System.Data.SqlClient.SqlException
    ''            Utilities.SaveAppError(ex.Message, Me.Parameters)
    ''            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
    ''        Catch ex As InvalidOperationException
    ''            Utilities.SaveAppError(ex.Message, Me.Parameters)
    ''            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
    ''        End Try
    ''        Return CarrierControl
    ''    End Using
    ''End Function

    ''Public Function GetFreeTrialGroupControl() As Integer
    ''    Dim GroupControl As Integer = 0
    ''    Using db As New NGLMASSecurityDataContext(ConnectionString)
    ''        Try
    ''            GroupControl = db.tblUserGroups.Where(Function(x) x.UserGroupsUGCControl = 5).Select(Function(x) x.UserGroupsControl).FirstOrDefault()
    ''            Return GroupControl
    ''        Catch ex As System.Data.SqlClient.SqlException
    ''            Utilities.SaveAppError(ex.Message, Me.Parameters)
    ''            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
    ''        Catch ex As InvalidOperationException
    ''            Utilities.SaveAppError(ex.Message, Me.Parameters)
    ''            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
    ''        Catch ex As Exception
    ''            Utilities.SaveAppError(ex.Message, Me.Parameters)
    ''            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
    ''        End Try
    ''        Return GroupControl
    ''    End Using
    ''End Function

    Public Function GetFreeTrialUsers(ByRef RecordCount As Integer,
                                             ByVal CatControl As Integer,
                                             Optional ByVal filterWhere As String = "",
                                             Optional ByVal sortExpression As String = "UserSecurityControl Desc",
                                             Optional ByVal page As Integer = 1,
                                             Optional ByVal pagesize As Integer = 1000,
                                             Optional ByVal skip As Integer = 0,
                                             Optional ByVal take As Integer = 0) As LTS.tblUserSecurity()
        Dim oRetData As LTS.tblUserSecurity()
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                db.Log = New DebugTextWriter
                Dim intPageCount As Integer = 1
                Dim GroupControl = db.tblUserGroups.Where(Function(x) x.UserGroupsUGCControl = CatControl).Select(Function(x) x.UserGroupsControl).FirstOrDefault()
                Dim oQuery = (From t In db.tblUserSecurities
                              Group Join g In db.tblUserGroups On t.UserUserGroupsControl Equals g.UserGroupsControl Into Group
                              Where t.UserUserGroupsControl = GroupControl
                              Order By t.UserName
                              From ug In Group.DefaultIfEmpty()
                              Select t)
                If oQuery Is Nothing Then Return Nothing
                If Not String.IsNullOrEmpty(filterWhere) Then
                    oQuery = oQuery.Where(Function(x) x.UserName.StartsWith(filterWhere) Or x.UserFriendlyName.StartsWith(filterWhere) Or x.UserFirstName.StartsWith(filterWhere) Or x.UserLastName.StartsWith(filterWhere))
                End If
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

    Public Function getLECompControl(Optional ByVal usercontrol As Integer = 0) As Integer
        Dim intRet As Integer = 0
        If usercontrol = 0 Then usercontrol = Parameters.UserControl
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                intRet = db.tblUserSecurityLegalEntities.Where(Function(x) x.USLEUserSecurityControl = usercontrol).Select(Function(x) x.USLECompControl).FirstOrDefault()

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
        Return intRet
    End Function

    ''Commented out - DEPRECIATED
    ''Public Function GetCategoryForUser(ByVal UserControl As Integer) As Integer
    ''    Dim CategoryControl As Integer = 0
    ''    Using db As New NGLMASSecurityDataContext(ConnectionString)
    ''        Try
    ''            Dim GroupControl = db.tblUserSecurities.Where(Function(x) x.UserSecurityControl = UserControl).Select(Function(x) x.UserUserGroupsControl).FirstOrDefault()
    ''            If GroupControl <> 0 Then
    ''                CategoryControl = db.tblUserGroups.Where(Function(x) x.UserGroupsControl = GroupControl).Select(Function(x) x.UserGroupsUGCControl).FirstOrDefault()
    ''            End If
    ''            Return CategoryControl
    ''        Catch ex As System.Data.SqlClient.SqlException
    ''            Utilities.SaveAppError(ex.Message, Me.Parameters)
    ''            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
    ''        Catch ex As InvalidOperationException
    ''            Utilities.SaveAppError(ex.Message, Me.Parameters)
    ''            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
    ''        Catch ex As Exception
    ''            Utilities.SaveAppError(ex.Message, Me.Parameters)
    ''            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
    ''        End Try
    ''        Return CategoryControl
    ''    End Using
    ''End Function

    Public Function GetUserSecurityLanes(ByVal UserControl As Integer) As LTS.vUserSecurityLane()
        Dim oRet As LTS.vUserSecurityLane()
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                oRet = db.vUserSecurityLanes.Where(Function(x) x.UserSecurityControl = UserControl).ToArray()
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

    ''' <summary>
    ''' Get365Account
    ''' </summary>
    ''' <param name="userName"></param>
    ''' <param name="expires"></param>
    ''' <param name="pUSC"></param>
    ''' <param name="pUSATToken"></param>
    ''' <param name="pUSATUserID"></param>
    ''' <param name="pUserEmail"></param>
    ''' <param name="pUserFirstName"></param>
    ''' <param name="pUserLastName"></param>
    ''' <param name="pSSOAAuthCode"></param>
    ''' <param name="pSSOAControl"></param>
    ''' <param name="pPass"></param>
    ''' <param name="pFriendlyName"></param>
    ''' <param name="pWorkPhone"></param>
    ''' <param name="pWorkExt"></param>
    ''' <param name="P44WebServiceLogin"></param>
    ''' <param name="P44WebServicePassword"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified By LVV on 4/4/18 for v-8.1
    ''' Added optional parameters for P44WebServiceLogin and 
    ''' P44WebServicePassword because these always need to come from
    ''' the WebConfig. (Previously in this method they were hardcoded)
    ''' 
    ''' TODO:  user validation errors and messages returned from  
    ''' are not being processed or properly returned and validation may fail
    ''' validation errors may not be properly returned to the users
    ''' Modified by RHR for v-8.5.2.006 added tblUserSecurity.UserMustChangePassword property
    ''' Modified by RHR for v-8.5.4.005 on 03/28/2024 added timezone and culture
    ''' </remarks>
    Public Function Get365Account(ByVal userName As String,
                                  ByVal expires As Date,
                                  ByVal pUSC As Integer,
                                  Optional ByVal pUSATToken As String = "",
                                  Optional ByVal pUSATUserID As String = "",
                                  Optional ByVal pUserEmail As String = "",
                                  Optional ByVal pUserFirstName As String = "",
                                  Optional ByVal pUserLastName As String = "",
                                  Optional ByVal pSSOAAuthCode As String = "",
                                  Optional ByVal pSSOAControl As Integer = 0,
                                  Optional ByVal pPass As String = "",
                                  Optional ByVal pFriendlyName As String = "",
                                  Optional ByVal pWorkPhone As String = "",
                                  Optional ByVal pWorkExt As String = "",
                                  Optional ByVal P44WebServiceLogin As String = "",
                                  Optional ByVal P44WebServicePassword As String = "") As Models.SSOAccount
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim l = (From d In db.spGet365Account(userName, expires, pUSC, pUSATToken, pUSATUserID, pUserEmail, pUserFirstName, pUserLastName, pSSOAAuthCode, pSSOAControl, pPass, pFriendlyName, pWorkPhone, pWorkExt) Select d).FirstOrDefault()
                Dim acct As New Models.SSOAccount
                If Not l Is Nothing Then
                    'If a new Free Trial User was just created, we need to add a record to tblSSOASecurityXref for this user to authorize them to use P44
                    If l.CreatedFTUser = True Then
                        Dim oSSOA As New NGLtblSingleSignOnAccountData(Parameters)
                        Dim wcf = oSSOA.InsertOrUpdateSSOASecurityXref(l.SSOAUserSecurityControl, 4, P44WebServiceLogin, P44WebServicePassword)
                        If wcf.Success = False AndAlso wcf.Warnings?.Count > 0 Then
                            SaveAppError(wcf.concatWarnings(), Me.Parameters)
                        End If
                    End If
                    With acct
                        .SSOAControl = l.SSOAControl
                        .SSOAName = l.SSOAName
                        .SSOADesc = l.SSOADesc
                        .AllowNonPrimaryComputers = l.AllowNonPrimaryComputers
                        .AllowPublicComputer = l.AllowPublicComputer
                        .SSOAClientID = l.SSOAClientID
                        .SSOALoginURL = l.SSOALoginURL
                        .SSOADataURL = l.SSOADataURL
                        .SSOARedirectURL = l.SSOARedirectURL
                        .SSOAClientSecret = l.SSOAClientSecret
                        .SSOAAuthCode = l.SSOAAuthCode
                        .SSOAAuthenticationRequired = l.SSOAAuthenticationRequired
                        .SSOAUserSecurityControl = l.SSOAUserSecurityControl
                        .SSOAUserName = l.SSOAUserName
                        .SSOAUserEmail = l.SSOAUserEmail
                        .UserFriendlyName = l.UserFriendlyName
                        .UserFirstName = l.UserFirstName
                        .UserLastName = l.UserLastName
                        .AuthenticationErrorCode = l.AuthenticationErrorCode
                        .AuthenticationErrorMessage = l.AuthenticationErrorMessage
                        .IsUserCarrier = l.IsCarrier
                        .UserCarrierControl = l.CarrierControl
                        .UserCarrierContControl = l.CarrierContControl
                        .UserLEControl = l.LEControl
                        .UserTheme365 = l.UserTheme365
                        .CatControl = l.CatControl
                        .UserMustChangePassword = If(l.UserMustChangePassword, False)
                        .UserCultureInfo = l.UserCultureInfo
                        .UserTimeZone = l.UserTimeZone
                    End With
                    Return acct
                End If
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
    ''' Accepts a Legacy Free Trial Request
    ''' </summary>
    ''' <param name="strErrMsg"></param>
    ''' <param name="FTUSC"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified By LVV on 5/2/18 for v-8.1
    ''' Removed unneccessary funtion calls (get user category, get FT group control)
    ''' </remarks>
    Public Function AcceptLegacyFTRequest(ByRef strErrMsg As String, ByVal FTUSC As Integer) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'This method can only be executed by a SuperUser
                If Parameters.CatControl = 4 Then
                    'Dim FTGroupControl = db.tblUserGroups.Where(Function(x) x.UserGroupsUGCControl = 5).Select(Function(x) x.UserGroupsControl).FirstOrDefault()
                    'Free Trial Group Control is now always 4
                    Dim FTGroupControl = 4
                    ReplaceUserSecurityWithGroup(FTGroupControl, FTUSC)
                    Dim lts = (From d In db.tblUserSecurities Where d.UserSecurityControl = FTUSC Select d).FirstOrDefault()
                    lts.UserStartFreeTrial = Date.Now
                    lts.UserEndFreeTrial = Date.Now.AddDays(30)
                    db.SubmitChanges()
                    blnRet = True
                Else
                    '** TODO LVV ** Localize these messages. Also, put all this logic into a sp
                    strErrMsg = "You are not authorized to execute this procedure"
                End If
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
            Return blnRet
        End Using
    End Function

    ''' <summary>
    ''' Get all the User Groups associated with the Companies that are associated with the Legal Entity.
    ''' Filter the results based on the permissions level of the requesting/current/active user
    ''' </summary>
    ''' <param name="LEAdminControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 4/3/18 for v-8.0 TMS 365
    ''' </remarks>
    Public Function GetUserGroupsForLE(ByVal LEAdminControl As Integer) As LTS.spGetUserGroupsForLEResult()
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim results As LTS.spGetUserGroupsForLEResult()
                'Get all the User Groups associated with the Companies that are associated with the Legal Entity.
                Dim spRes = db.spGetUserGroupsForLE(LEAdminControl).ToArray()
                'Filter the results based on the permissions level of the requesting/current/active user
                Select Case Parameters.CatControl
                    Case 4
                        'Super Users can see all groups
                        results = spRes
                    Case 3
                        'LEAdmins can see all groups EXCEPT groups with SuperUser level permissions
                        results = spRes.Where(Function(x) x.UserGroupsUGCControl <> 4).ToArray()
                    Case Else
                        'Normal users can see all groups EXCEPT groups with SuperUser or LEAdmin level permissions
                        results = spRes.Where(Function(x) x.UserGroupsUGCControl <> 4 AndAlso x.UserGroupsUGCControl <> 3).ToArray()
                End Select
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
            Return Nothing
        End Using
    End Function

#Region "365 Alert Methods"

    ''' <summary>
    ''' Gets all alert messages within the last 10 minutes
    ''' </summary>
    ''' <param name="db"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 9/11/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Private Function Get365AlertMessages(ByRef db As NGLMASSecurityDataContext) As LTS.tblAlertMessage()
        Dim ntblAlertMessages() As LTS.tblAlertMessage
        Try
            ntblAlertMessages = db.tblAlertMessages.Where(Function(x) x.AlertMessageModDate > Date.Now.AddMinutes(-10)).ToArray()
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        End Try
        Return ntblAlertMessages
    End Function

    ''' <summary>
    ''' Gets the alerts to show to the 365 User
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 9/11/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Public Function Get365AlertMessagesForUser() As Models.Alert()
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Dim retVals As New List(Of Models.Alert)
            Try
                Dim alerts = Get365AlertMessages(db)
                For Each a In alerts
                    If ShowPopupForAlert(db, Parameters.UserControl, a.AlertMessageProcedureControl, a.AlertMessageCompControl) Then
                        If Not db.tbl365DisplayedAlertMessages.Any(Function(x) x.DAMXUserSecurityControl = Parameters.UserControl AndAlso x.DAMXAlertMessageControl = a.AlertMessageControl) Then
                            'If not exists in xref table --> show the message to user and write record in xref
                            retVals.Add(New Models.Alert With {.Title = a.AlertMessageSubject, .Msg = a.AlertMessageBody})
                            Insert365AlertXrefAsync(Parameters.UserControl, a.AlertMessageControl)
                        End If
                    End If
                Next
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
            Return retVals.ToArray()
        End Using
    End Function

    'Added By LVV on 9/11/18 for v-8.3 TMS365 Scheduler
    Public Delegate Sub Insert365AlertXrefDelegate(ByVal USC As Integer, ByVal AlertControl As Long)

    ''' <summary>
    ''' Asynchronously Inserts a record into tbl365DisplayedAlertMessages
    ''' </summary>
    ''' <param name="USC"></param>
    ''' <param name="AlertControl"></param>
    ''' <remarks>
    ''' Added By LVV on 9/11/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Public Sub Insert365AlertXrefAsync(ByVal USC As Integer, ByVal AlertControl As Long)
        Dim fetcher As New Insert365AlertXrefDelegate(AddressOf Me.Insert365AlertXref)
        ' Launch thread
        fetcher.BeginInvoke(USC, AlertControl, Nothing, Nothing)
    End Sub

    ''' <summary>
    ''' Inserts a record into tbl365DisplayedAlertMessages
    ''' </summary>
    ''' <param name="USC"></param>
    ''' <param name="AlertControl"></param>
    ''' <remarks>
    ''' Added By LVV on 9/11/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Public Sub Insert365AlertXref(ByVal USC As Integer, ByVal AlertControl As Long)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim oRecord As New LTS.tbl365DisplayedAlertMessage
                With oRecord
                    .DAMXUserSecurityControl = USC
                    .DAMXAlertMessageControl = AlertControl
                    .DAMXModDate = Date.Now
                    .DAMXModUser = Parameters.UserName
                End With
                db.tbl365DisplayedAlertMessages.InsertOnSubmit(oRecord)
                db.SubmitChanges()
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
    End Sub

#End Region

#Region "365 Role Configuration Methods"

    '0 = Form, 1 = Procedure, 2 = Report
    Public Enum ConfigType
        Form
        Procedure
        Report
    End Enum

    Public Enum ConfigAction
        AllowAccess
        RestrictAccess
        Lock
        Unlock
    End Enum

    ''' <summary>
    ''' Gets a list of Forms, Procedures, or Reports depending on the value in parameter ItemType
    ''' </summary>
    ''' <param name="ItemType"></param>
    ''' <returns></returns>
    ''' <remarks>Created By LVV on 5/23/20 for v-8.2.1.008 - Task #202005191648 Fix Procedure Level Security Deployment Issues</remarks>
    Public Function GetConfigItems(ByVal ItemType As Integer) As DTO.vLookupList()
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim items As DTO.vLookupList()
                Select Case ItemType
                    Case 0 'Form
                        items = (From f In db.tblFormLists Order By f.FormName Select New DTO.vLookupList With {.Control = f.FormControl, .Name = f.FormName, .Description = f.FormDescription}).ToArray()
                    Case 1 'Procedure (Action)
                        items = (From p In db.tblProcedureLists Order By p.ProcedureName Select New DTO.vLookupList With {.Control = p.ProcedureControl, .Name = p.ProcedureName, .Description = p.ProcedureDescription}).ToArray()
                    Case 2 'Report
                        items = (From r In db.tblReportLists Order By r.ReportName Select New DTO.vLookupList With {.Control = r.ReportControl, .Name = r.ReportName, .Description = r.ReportDescription}).ToArray()
                End Select
                Return items
            Catch ex As SqlException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Restricts or Allows Access to Groups for the Forms/Procedures/Reports
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <remarks>Created By LVV on 5/29/20 for v-8.2.1.008 - Task #202005191648 Fix Procedure Level Security Deployment Issues</remarks>
    Public Sub ManageAccessForGroup(ByVal oData As Models.MultiSelectBatchObjects, ByVal blnAllowAccess As Boolean)
        If oData Is Nothing Then Return 'nothing to do
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim bwTmp As New Core.Utility.BitwiseFlags()
                Select Case oData.ConfigType
                    Case ConfigType.Form
                        For Each f In oData.Controls
                            Dim ltsForm = db.tblFormLists.Where(Function(x) x.FormControl = f).FirstOrDefault()
                            If ltsForm IsNot Nothing Then
                                Dim flagSource = ltsForm.FormSecTypeBits
                                bwTmp = New Core.Utility.BitwiseFlags(flagSource)
                                For Each bit In oData.SelectedBits
                                    If blnAllowAccess Then bwTmp.turnBitFlagOn(bit) Else bwTmp.turnBitFlagOff(bit)
                                Next
                                ltsForm.FormSecTypeBits = bwTmp.FlagSource
                            End If
                            db.SubmitChanges()
                        Next
                    Case ConfigType.Procedure
                        For Each p In oData.Controls
                            Dim ltsForm = db.tblProcedureLists.Where(Function(x) x.ProcedureControl = p).FirstOrDefault()
                            If ltsForm IsNot Nothing Then
                                Dim flagSource = ltsForm.ProcedureSecTypeBits
                                bwTmp = New Core.Utility.BitwiseFlags(flagSource)
                                For Each bit In oData.SelectedBits
                                    If blnAllowAccess Then bwTmp.turnBitFlagOn(bit) Else bwTmp.turnBitFlagOff(bit)
                                Next
                                ltsForm.ProcedureSecTypeBits = bwTmp.FlagSource
                            End If
                            db.SubmitChanges()
                        Next
                    Case ConfigType.Report
                        For Each r In oData.Controls
                            Dim ltsForm = db.tblReportLists.Where(Function(x) x.ReportControl = r).FirstOrDefault()
                            If ltsForm IsNot Nothing Then
                                Dim flagSource = ltsForm.ReportSecTypeBits
                                bwTmp = New Core.Utility.BitwiseFlags(flagSource)
                                For Each bit In oData.SelectedBits
                                    If blnAllowAccess Then bwTmp.turnBitFlagOn(bit) Else bwTmp.turnBitFlagOff(bit)
                                Next
                                ltsForm.ReportSecTypeBits = bwTmp.FlagSource
                            End If
                            db.SubmitChanges()
                        Next
                End Select
            Catch ex As SqlException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Locks or Unlocks settings of Groups for the Forms/Procedures/Reports
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <remarks>Created By LVV on 6/1/20 for v-8.2.1.008 - Task #202005191648 Fix Procedure Level Security Deployment Issues</remarks>
    Public Sub ManageLocksForGroup(ByVal oData As Models.MultiSelectBatchObjects, ByVal blnLocked As Boolean)
        If oData Is Nothing Then Return 'nothing to do
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim bwTmp As New Core.Utility.BitwiseFlags()
                Select Case oData.ConfigType
                    Case ConfigType.Form
                        For Each f In oData.Controls
                            Dim ltsForm = db.tblFormLists.Where(Function(x) x.FormControl = f).FirstOrDefault()
                            If ltsForm IsNot Nothing Then
                                Dim flagSource = ltsForm.FormSecTypeBitLocked
                                bwTmp = New Core.Utility.BitwiseFlags(flagSource)
                                For Each bit In oData.SelectedBits
                                    If blnLocked Then bwTmp.turnBitFlagOn(bit) Else bwTmp.turnBitFlagOff(bit)
                                Next
                                ltsForm.FormSecTypeBitLocked = bwTmp.FlagSource
                            End If
                            db.SubmitChanges()
                        Next
                    Case ConfigType.Procedure
                        For Each p In oData.Controls
                            Dim ltsForm = db.tblProcedureLists.Where(Function(x) x.ProcedureControl = p).FirstOrDefault()
                            If ltsForm IsNot Nothing Then
                                Dim flagSource = ltsForm.ProcedureSecTypeBitLocked
                                bwTmp = New Core.Utility.BitwiseFlags(flagSource)
                                For Each bit In oData.SelectedBits
                                    If blnLocked Then bwTmp.turnBitFlagOn(bit) Else bwTmp.turnBitFlagOff(bit)
                                Next
                                ltsForm.ProcedureSecTypeBitLocked = bwTmp.FlagSource
                            End If
                            db.SubmitChanges()
                        Next
                    Case ConfigType.Report
                        For Each r In oData.Controls
                            Dim ltsForm = db.tblReportLists.Where(Function(x) x.ReportControl = r).FirstOrDefault()
                            If ltsForm IsNot Nothing Then
                                Dim flagSource = ltsForm.ReportSecTypeBitLocked
                                bwTmp = New Core.Utility.BitwiseFlags(flagSource)
                                For Each bit In oData.SelectedBits
                                    If blnLocked Then bwTmp.turnBitFlagOn(bit) Else bwTmp.turnBitFlagOff(bit)
                                Next
                                ltsForm.ReportSecTypeBitLocked = bwTmp.FlagSource
                            End If
                            db.SubmitChanges()
                        Next
                End Select
            Catch ex As SqlException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Returns the Security Type Configuration for each Form.
    ''' Note: Currently only support filtering by Name
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns>Models.SecurityTypeConfig()</returns>
    ''' <remarks>Created By LVV on 6/11/20 for v-8.2.1.008 - Task #202005191648 Fix Procedure Level Security Deployment Issues</remarks>
    Public Function GetFormSecTypeXTabData(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As Models.SecurityTypeConfig()
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim formConfigs As Models.SecurityTypeConfig()
                Dim temp As Models.SecurityTypeConfig() = (
                    From d In db.spGetFormSecTypeXTabData
                    Select New Models.SecurityTypeConfig With {
                        .Control = d.Control,
                        .Name = d.Name,
                        .Desc = d.Desc,
                        .Everyone = d.Everyone,
                        .CarrierDispatch = d.CarrierDispatch,
                        .CarrierAccounting = d.CarrierAccounting,
                        .Warehouse = d.Warehouse,
                        .NEXTrack = d.NEXTrack,
                        .NEXTStop = d.NEXTStop,
                        .LEOperations = d.LEOperations,
                        .LEAccounting = d.LEAccounting,
                        .LEAdmin = d.LEAdmin,
                        .Super = d.Super,
                        .Inactive = d.Inactive}).ToArray()
                'Do the filtering
                formConfigs = temp
                For Each f In filters.FilterValues.OrderBy(Function(x) x.filterName)
                    If f.filterName = "Control" Then
                        Dim control = 0
                        Integer.TryParse(f.filterValueFrom, control)
                        If control <> 0 Then formConfigs = (From d In temp Where d.Control = control).ToArray()
                        Exit For
                    End If
                    If f.filterName = "Name" Then
                        If Not String.IsNullOrWhiteSpace(f.filterValueFrom) Then formConfigs = (From d In temp Where d.Name.ToUpper().Contains(f.filterValueFrom.ToUpper())).ToArray()
                        Exit For
                    End If
                Next
                'Do the paging
                RecordCount = formConfigs.Count()
                If RecordCount < 1 Then RecordCount = 5
                If filters.take < 1 Then filters.take = If(RecordCount > 500, 500, RecordCount)
                If filters.skip >= RecordCount Then filters.skip = (CInt(((RecordCount - 1) / filters.take)) * filters.take) 'adjust for last page if skip beyond last page
                If filters.skip >= RecordCount Or filters.skip < 0 Then filters.skip = 0 'adjust for first page if skip beyond or below first page
                Return formConfigs.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As SqlException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Sets up the Security Type Configuration for the Form based on the parameter values
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <remarks>Created By LVV on 6/11/20 for v-8.2.1.008 - Task #202005191648 Fix Procedure Level Security Deployment Issues</remarks>
    Public Sub ConfigureSecTypesForForm(ByVal oData As Models.SecurityTypeConfig)
        If oData Is Nothing Then Return 'nothing to do
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                db.spConfigureSecTypesForForm(oData.Name, oData.CarrierDispatch, oData.CarrierAccounting, oData.Warehouse, oData.NEXTrack, oData.NEXTStop, oData.LEOperations, oData.LEAccounting, oData.LEAdmin, oData.Super, oData.Inactive)
            Catch ex As SqlException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Returns the Security Type Configuration for each Procedure.
    ''' Note: Currently only support filtering by Name
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns>Models.SecurityTypeConfig()</returns>
    ''' <remarks>Created By LVV on 6/12/20 for v-8.2.1.008 - Task #202005191648 Fix Procedure Level Security Deployment Issues</remarks>
    Public Function GetProcedureSecTypeXTabData(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As Models.SecurityTypeConfig()
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim procedureConfigs As Models.SecurityTypeConfig()
                Dim temp As Models.SecurityTypeConfig() = (
                    From d In db.spGetProcedureSecTypeXTabData
                    Select New Models.SecurityTypeConfig With {
                        .Control = d.Control,
                        .Name = d.Name,
                        .Desc = d.Desc,
                        .Everyone = d.Everyone,
                        .CarrierDispatch = d.CarrierDispatch,
                        .CarrierAccounting = d.CarrierAccounting,
                        .Warehouse = d.Warehouse,
                        .NEXTrack = d.NEXTrack,
                        .NEXTStop = d.NEXTStop,
                        .LEOperations = d.LEOperations,
                        .LEAccounting = d.LEAccounting,
                        .LEAdmin = d.LEAdmin,
                        .Super = d.Super,
                        .Inactive = d.Inactive}).ToArray()
                'Do the filtering
                procedureConfigs = temp
                For Each f In filters.FilterValues.OrderBy(Function(x) x.filterName)
                    If f.filterName = "Control" Then
                        Dim control = 0
                        Integer.TryParse(f.filterValueFrom, control)
                        If control <> 0 Then procedureConfigs = (From d In temp Where d.Control = control).ToArray()
                        Exit For
                    End If
                    If f.filterName = "Name" Then
                        If Not String.IsNullOrWhiteSpace(f.filterValueFrom) Then procedureConfigs = (From d In temp Where d.Name.ToUpper().Contains(f.filterValueFrom.ToUpper())).ToArray()
                        Exit For
                    End If
                Next
                'Do the paging
                RecordCount = procedureConfigs.Count()
                If RecordCount < 1 Then RecordCount = 5
                If filters.take < 1 Then filters.take = If(RecordCount > 500, 500, RecordCount)
                If filters.skip >= RecordCount Then filters.skip = (CInt(((RecordCount - 1) / filters.take)) * filters.take) 'adjust for last page if skip beyond last page
                If filters.skip >= RecordCount Or filters.skip < 0 Then filters.skip = 0 'adjust for first page if skip beyond or below first page
                Return procedureConfigs.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As SqlException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Sets up the Security Type Configuration for the Procedure based on the parameter values
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <remarks>Created By LVV on 6/12/20 for v-8.2.1.008 - Task #202005191648 Fix Procedure Level Security Deployment Issues</remarks>
    Public Sub ConfigureSecTypesForProcedure(ByVal oData As Models.SecurityTypeConfig)
        If oData Is Nothing Then Return 'nothing to do
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                db.spConfigureSecTypesForProcedure(oData.Name, oData.CarrierDispatch, oData.CarrierAccounting, oData.Warehouse, oData.NEXTrack, oData.NEXTStop, oData.LEOperations, oData.LEAccounting, oData.LEAdmin, oData.Super, oData.Inactive)
            Catch ex As SqlException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Returns the Security Type Configuration for each Report.
    ''' Note: Currently only support filtering by Name
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns>Models.SecurityTypeConfig()</returns>
    ''' <remarks>Created By LVV on 6/12/20 for v-8.2.1.008 - Task #202005191648 Fix Procedure Level Security Deployment Issues</remarks>
    Public Function GetReportSecTypeXTabData(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As Models.SecurityTypeConfig()
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim reportConfigs As Models.SecurityTypeConfig()
                Dim temp As Models.SecurityTypeConfig() = (
                    From d In db.spGetReportSecTypeXTabData
                    Select New Models.SecurityTypeConfig With {
                        .Control = d.Control,
                        .Name = d.Name,
                        .Desc = d.Desc,
                        .Everyone = d.Everyone,
                        .CarrierDispatch = d.CarrierDispatch,
                        .CarrierAccounting = d.CarrierAccounting,
                        .Warehouse = d.Warehouse,
                        .NEXTrack = d.NEXTrack,
                        .NEXTStop = d.NEXTStop,
                        .LEOperations = d.LEOperations,
                        .LEAccounting = d.LEAccounting,
                        .LEAdmin = d.LEAdmin,
                        .Super = d.Super,
                        .Inactive = d.Inactive}).ToArray()
                'Do the filtering
                reportConfigs = temp
                For Each f In filters.FilterValues.OrderBy(Function(x) x.filterName)
                    If f.filterName = "Control" Then
                        Dim control = 0
                        Integer.TryParse(f.filterValueFrom, control)
                        If control <> 0 Then reportConfigs = (From d In temp Where d.Control = control).ToArray()
                        Exit For
                    End If
                    If f.filterName = "Name" Then
                        If Not String.IsNullOrWhiteSpace(f.filterValueFrom) Then reportConfigs = (From d In temp Where d.Name.ToUpper().Contains(f.filterValueFrom.ToUpper())).ToArray()
                        Exit For
                    End If
                Next
                'Do the paging
                RecordCount = reportConfigs.Count()
                If RecordCount < 1 Then RecordCount = 5
                If filters.take < 1 Then filters.take = If(RecordCount > 500, 500, RecordCount)
                If filters.skip >= RecordCount Then filters.skip = (CInt(((RecordCount - 1) / filters.take)) * filters.take) 'adjust for last page if skip beyond last page
                If filters.skip >= RecordCount Or filters.skip < 0 Then filters.skip = 0 'adjust for first page if skip beyond or below first page
                Return reportConfigs.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As SqlException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Sets up the Security Type Configuration for the Report based on the parameter values
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <remarks>Created By LVV on 6/12/20 for v-8.2.1.008 - Task #202005191648 Fix Procedure Level Security Deployment Issues</remarks>
    Public Sub ConfigureSecTypesForReport(ByVal oData As Models.SecurityTypeConfig)
        If oData Is Nothing Then Return 'nothing to do
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                db.spConfigureSecTypesForReport(oData.Name, oData.CarrierDispatch, oData.CarrierAccounting, oData.Warehouse, oData.NEXTrack, oData.NEXTStop, oData.LEOperations, oData.LEAccounting, oData.LEAdmin, oData.Super, oData.Inactive)
            Catch ex As SqlException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Reads the group settings and updates pages, actions, and reports to match 
    ''' the tblEnumGroupSecurityType settings based on the current locked or unlocked values.
    ''' Group specific procedure only updates one group at a time
    ''' </summary>
    ''' <param name="GroupControl"></param>
    ''' <remarks>Created By LVV on 6/15/20 for v-8.2.1.008 - Task #202005191648 Fix Procedure Level Security Deployment Issues</remarks>
    Public Sub UpdateGroupBasedOnSecurityType(ByVal GroupControl As Integer)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                db.spUpdateGroupBasedOnSecurityType(GroupControl)
            Catch ex As SqlException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Loops through each group, checks the group security types, and adds or removes the default security settings.
    ''' Updates pages, actions, and reports to match the tblEnumGroupSecurityType settings based on 
    ''' the current locked or unlocked values
    ''' </summary>
    ''' <remarks>Created By LVV on 6/15/20 for v-8.2.1.008 - Task #202005191648 Fix Procedure Level Security Deployment Issues</remarks>
    Public Sub UpdateAllGroupsBasedOnSecurityType()
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                db.spUpdateAllGroupsBasedOnSecurityType()
            Catch ex As SqlException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
        End Using
    End Sub


    ''' <summary>
    ''' Loop through all the users that belong to the Group And call spCMUpdateUserPermissions
    ''' </summary>
    ''' <param name="GroupControl"></param>
    ''' <remarks>Created By LVV on 6/15/20 for v-8.2.1.008 - Task #202005191648 Fix Procedure Level Security Deployment Issues</remarks>
    Public Sub UpdateAllUserPermissionsForGroup(ByVal GroupControl As Integer)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                db.spCMUpdateAllUserPermissions(GroupControl)
            Catch ex As SqlException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
        End Using
    End Sub

#End Region

#End Region

    Public Function NetCheckUserSecurityByControl(ByVal UserSecurityControl As Integer, ByVal UserRemotePassword As String) As LTS.spNetCheckUserSecurityByControlResult
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Return db.spNetCheckUserSecurityByControl(UserSecurityControl, UserRemotePassword).FirstOrDefault()
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

    ''' <summary>
    ''' Checks if the user has permission to view the page.
    ''' If not throws an exception
    ''' </summary>
    ''' <param name="PageControl"></param>
    ''' <param name="USC"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created By LVV on 6/1/20 for v-8.2.1.008 - Task #202005191648 Fix Procedure Level Security Deployment Issues
    ''' Modified by RHR for v-8.4.0.002 on 03-05-2021
    '''   Added logic to disable exception and just return true or false
    '''   we still log the error in app errors but the caller is responsible for
    '''   processing the failure
    ''' Modified by RHR for v-8.5.4.001 on 05/31/2023 added logic to check for the AuthCode.
    '''     ToDo:  Need to add logic to check for the AuthCode by Legal Entity for Multi-Tennant.
    ''' </remarks>
    Public Function CanUserAccessScreen(ByVal PageControl As Integer, ByVal USC As Integer, Optional ByVal blnThrowException As Boolean = True) As Boolean
        Dim blnRet As Boolean = False
        Dim oSystemData As New NGLSystemDataProvider(Me.Parameters)
        oSystemData.CheckAuthCode()
        'CheckAuthCode()
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            'Try
            '0 Access Denied User has been restricted 
            '1 Access Granted
            Dim a = db.spNetCheckFormSecurity365(USC, PageControl).FirstOrDefault()
            If a.Column1 = 0 Then
                'Not authorized to view that page (we return the default value of false
                Dim oLocalize As New NGLcmLocalizeKeyValuePairData(Parameters)
                Dim errMsg = oLocalize.GetLocalizedValueByKey("E_NotAuthScreen", "You are not authorized to access this screen.")
                SaveAppError(errMsg, Me.Parameters)
                If blnThrowException Then
                    Dim reason = oLocalize.GetLocalizedValueByKey("E_AccessDenied", "Access Denied!  Please check that you are logged in with a valid account for this activity.")
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = errMsg, .Details = "", .DetailsList = New List(Of String)}, New FaultReason(reason))
                End If
            Else
                blnRet = True
            End If
            Return blnRet
        End Using
    End Function




End Class

Public Class NGLtblSingleSignOnAccountData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Me.Parameters = oParameters
        'Dim db As New NGLMASSecurityDataContext(ConnectionString)
        'Me.LinqTable = db.tblSingleSignOnAccounts
        'Me.LinqDB = db
        Me.SourceClass = "NGLtblSingleSignOnAccountData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If (_LinqTable Is Nothing) Then
                Dim db As New NGLMASSecurityDataContext(ConnectionString)
                _LinqTable = db.tblSingleSignOnAccounts
                _LinqDB = db

            End If
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Enums"

    'Public Enum SSOAType
    '    NGLLegacy = 1
    '    DAT
    '    NEXTStop
    '    P44
    '    Microsoft
    '    NGLService
    'End Enum

#End Region

#Region "Public Methods"

    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return GettblSingleSignOnAccountFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GettblSingleSignOnAccountsFiltered()
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

        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim tblSingleSignOnAccount As DTO.tblSingleSignOnAccount

                If LowerControl <> 0 Then
                    tblSingleSignOnAccount = (
                   From d In db.tblSingleSignOnAccounts
                   Where d.SSOAControl >= LowerControl
                   Order By d.SSOAControl
                   Select selectDTOData(d)).FirstOrDefault
                Else
                    'Zero indicates that we should get the record with the lowest control number even if it is below zero
                    tblSingleSignOnAccount = (
                   From d In db.tblSingleSignOnAccounts
                   Order By d.SSOAControl
                   Select selectDTOData(d)).FirstOrDefault
                End If

                Return tblSingleSignOnAccount

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetFirstRecord"), sysErrorParameters.sysErrorState.SystemLevelFault, sysErrorParameters.sysErrorSeverity.Unexpected)
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
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try

                'Get the first record that matches the provided criteria
                Dim tblSingleSignOnAccount As DTO.tblSingleSignOnAccount = (
                From d In db.tblSingleSignOnAccounts
                Where d.SSOAControl < CurrentControl
                Order By d.SSOAControl Descending
                Select selectDTOData(d)).FirstOrDefault


                Return tblSingleSignOnAccount

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetPreviousRecord"), sysErrorParameters.sysErrorState.SystemLevelFault, sysErrorParameters.sysErrorSeverity.Unexpected)
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
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try

                'Get the first record that matches the provided criteria
                Dim tblSingleSignOnAccount As DTO.tblSingleSignOnAccount = (
                From d In db.tblSingleSignOnAccounts
                Where d.SSOAControl > CurrentControl
                Order By d.SSOAControl
                Select selectDTOData(d)).FirstOrDefault


                Return tblSingleSignOnAccount

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetNextRecord"), sysErrorParameters.sysErrorState.SystemLevelFault, sysErrorParameters.sysErrorSeverity.Unexpected)
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
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim tblSingleSignOnAccount As DTO.tblSingleSignOnAccount

                If UpperControl <> 0 Then

                    tblSingleSignOnAccount = (
                    From d In db.tblSingleSignOnAccounts
                    Where d.SSOAControl >= UpperControl
                    Order By d.SSOAControl
                    Select selectDTOData(d)).FirstOrDefault
                Else
                    'Zero indicates that we should get the hightest SSOAcontrol record
                    tblSingleSignOnAccount = (
                    From d In db.tblSingleSignOnAccounts
                    Order By d.SSOAControl Descending
                    Select selectDTOData(d)).FirstOrDefault

                End If

                Return tblSingleSignOnAccount

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetLastRecord"), sysErrorParameters.sysErrorState.SystemLevelFault, sysErrorParameters.sysErrorSeverity.Unexpected)
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GettblSingleSignOnAccountFiltered(ByVal Control As Integer) As DTO.tblSingleSignOnAccount
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim tblSingleSignOnAccount As DTO.tblSingleSignOnAccount = (
                From d In db.tblSingleSignOnAccounts
                Where
                    d.SSOAControl = Control
                Select selectDTOData(d)).First

                Return tblSingleSignOnAccount

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GettblSingleSignOnAccountFiltered"), sysErrorParameters.sysErrorState.SystemLevelFault, sysErrorParameters.sysErrorSeverity.Unexpected)
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Read the SSOA data using the SSOAAccount enum
    ''' </summary>
    ''' <param name="SSOAControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.1.001 on 03/07/2022 apply LE Control Filter
    ''' </remarks>
    Public Function GettblSingleSignOnAccount(ByVal SSOAControl As Utilities.SSOAAccount) As DTO.tblSingleSignOnAccount
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try

                Dim iLEControl As Integer = Me.Parameters.UserLEControl

                'Get the newest record that matches the provided criteria
                'Dim tblSingleSignOnAccount As DTO.tblSingleSignOnAccount = (
                'From d In db.tblSingleSignOnAccounts
                'Where d.SSOAControl = SSOAControl And (d.SSOALEAdminControl = iLEControl Or d.SSOALEAdminControl = 0)
                'Order By d.SSOALEAdminControl Descending
                'Select selectDTOData(d)).FirstOrDefault()

                Dim tblSingleSignOnAccount As DTO.tblSingleSignOnAccount = (
                From d In db.tblSingleSignOnAccounts
                Where d.SSOAControl = SSOAControl
                Select selectDTOData(d)).FirstOrDefault()

                Return tblSingleSignOnAccount

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GettblSingleSignOnAccountFiltered"), sysErrorParameters.sysErrorState.SystemLevelFault, sysErrorParameters.sysErrorSeverity.Unexpected)
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Get the Legal Entity Single Sign on Settings by SSOA type
    ''' </summary>
    ''' <param name="SSOAControl"></param>
    ''' <param name="oLEConfig"></param>
    ''' <param name="lCompConfig"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created  by RHR for v-8.5.1.001 on 03/07/2022 apply LE Control Filter and additional config details
    ''' Modified by RHR for v-8.5.3.005 on 08/24/2022 added logic to use bookcontrol to look up Legal Entity because system users are not assigned to a legal entity
    '''       typically used for auto carrier selection
    ''' Modified by RHR for v-8.5.4.002 on 08/04/2023 added logic to filter results by SSOALESSOATypeControl
    '''     each SSOAControl can now support multiple SOATypes  the default is set to None (zero not assigned)
    ''' </remarks>
    Public Function GetSSOAConfig(ByVal SSOAControl As Utilities.SSOAAccount, ByRef oLEConfig As LTS.tblSSOALEConfig, ByRef lCompConfig As List(Of LTS.tblSSOAConfig), Optional ByVal iBookControl As Integer = 0, Optional ByVal eSSOAType As Utilities.SSOAType = Utilities.SSOAType.None) As Boolean
        Dim blnRet As Boolean = False
        If oLEConfig Is Nothing Then oLEConfig = New LTS.tblSSOALEConfig()

        If lCompConfig Is Nothing Then lCompConfig = New List(Of LTS.tblSSOAConfig)
        Dim iCompControl As Integer = 0
        Dim iSSOAType As Integer = CInt(eSSOAType)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                ' Modified by RHR for v-8.5.3.005 on 08/24/2022 added logic to use bookcontrol to look up Legal Entity
                Dim iLEControl As Integer = 0
                If iBookControl > 0 Then
                    Dim strSQL = "Select top 1 LEAdminControl From dbo.tblLegalEntityAdmin as a Where a.LEAdminLegalEntity = (Select top 1 c.CompLegalEntity From dbo.Comp as c inner join dbo.Book as b on c.CompControl = b.BookCustCompControl Where b.BookControl = " & iBookControl.ToString() & ")"
                    iLEControl = getScalarInteger(strSQL)
                End If
                'db.Log = New DebugTextWriter
                If iLEControl = 0 Then
                    iLEControl = Me.Parameters.UserLEControl
                End If




                oLEConfig = (
                From d In db.tblSSOALEConfigs
                Where d.SSOALESSOAControl = SSOAControl And (d.SSOALELEAdminControl = iLEControl Or d.SSOALELEAdminControl = 0) AndAlso d.SSOALESSOATypeControl = iSSOAType
                Order By d.SSOALELEAdminControl Descending
                Select d).FirstOrDefault()

                If Not oLEConfig Is Nothing AndAlso oLEConfig.SSOALEControl <> 0 Then
                    blnRet = True
                    'get the config records
                    Dim iLEConfigControl = oLEConfig.SSOALEControl
                    lCompConfig = db.tblSSOAConfigs.Where(Function(x) x.SSOACSSOALEControl = iLEConfigControl).ToList()
                End If


            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetSSOAConfig"), db)
            End Try

            Return blnRet

        End Using
    End Function

    ''' <summary>
    ''' uses Bid Type to look up Single Sign on Account and returns all the 
    ''' Legal Entity API Setting Headers records that match the SSOAType
    ''' The caller must call GetSSOAConfigs to get the detail records
    ''' </summary>
    ''' <param name="eBidType"></param>
    ''' <param name="iBookControl"></param>
    ''' <param name="eSSOAType"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created  by RHR for v-8.5.4.003 on 10/10/2023 
    '''     Uses new SSOABidTypeControl to get the API settings
    '''     Typically called by Dispatch Bid logic
    ''' </remarks>
    Public Function GetSSOALEConfigsByBidType(ByVal eBidType As DataTransferObjects.tblLoadTender.BidTypeEnum, Optional ByVal iBookControl As Integer = 0, Optional ByVal eSSOAType As Utilities.SSOAType = Utilities.SSOAType.None) As List(Of LTS.tblSSOALEConfig)
        Dim blnRet As Boolean = False
        Dim oLEConfigs As New List(Of LTS.tblSSOALEConfig)

        'If lCompConfig Is Nothing Then lCompConfig = New List(Of LTS.tblSSOAConfig)
        Dim iCompControl As Integer = 0
        Dim iSSOAType As Integer = CInt(eSSOAType)
        Dim iBidType As Integer = CInt(eBidType)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'Get the single sign on account
                Dim iSSOAControl = db.tblSingleSignOnAccounts.Where(Function(x) x.SSOABidTypeControl = iBidType).Select(Function(x) x.SSOAControl).FirstOrDefault()
                ' Modified by RHR for v-8.5.3.005 on 08/24/2022 added logic to use bookcontrol to look up Legal Entity
                Dim iLEControl As Integer = 0
                If iBookControl > 0 Then
                    Dim strSQL = "Select top 1 LEAdminControl From dbo.tblLegalEntityAdmin as a Where a.LEAdminLegalEntity = (Select top 1 c.CompLegalEntity From dbo.Comp as c inner join dbo.Book as b on c.CompControl = b.BookCustCompControl Where b.BookControl = " & iBookControl.ToString() & ")"
                    iLEControl = getScalarInteger(strSQL)
                End If
                'db.Log = New DebugTextWriter
                If iLEControl = 0 Then
                    iLEControl = Me.Parameters.UserLEControl
                End If

                oLEConfigs = db.tblSSOALEConfigs.Where(Function(x) x.SSOALESSOAControl = iSSOAControl AndAlso x.SSOALELEAdminControl = iLEControl AndAlso x.SSOALESSOATypeControl = iSSOAType).ToList()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetSSOAConfigs"), db)
            End Try
        End Using

        Return oLEConfigs
    End Function

    ''' <summary>
    ''' Returns a list of all APIs configured for the specified SSOA Type by Legal Entity
    ''' </summary>
    ''' <param name="eSSOAType"></param>
    ''' <param name="iBookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created for v-8.5.4.004 on 11/29/2023 returns all APIs configured for the specific SSOAType
    '''     filtered by Legal Entity.  The process uses the Bookcontrol to get the legal entity if available
    '''     or it uses the current user to get the default Legal Entity 
    ''' </remarks>
    Public Function GetSSOALEConfigsBySSOAType(ByVal eSSOAType As Utilities.SSOAType, Optional ByVal iBookControl As Integer = 0) As List(Of LTS.tblSSOALEConfig)
        Dim blnRet As Boolean = False
        Dim oLEConfigs As New List(Of LTS.tblSSOALEConfig)

        'If lCompConfig Is Nothing Then lCompConfig = New List(Of LTS.tblSSOAConfig)
        Dim iCompControl As Integer = 0
        Dim iSSOAType As Integer = CInt(eSSOAType)

        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim iLEControl As Integer = 0
                If iBookControl > 0 Then
                    Dim strSQL = "Select top 1 LEAdminControl From dbo.tblLegalEntityAdmin as a Where a.LEAdminLegalEntity = (Select top 1 c.CompLegalEntity From dbo.Comp as c inner join dbo.Book as b on c.CompControl = b.BookCustCompControl Where b.BookControl = " & iBookControl.ToString() & ")"
                    iLEControl = getScalarInteger(strSQL)
                End If
                'db.Log = New DebugTextWriter
                If iLEControl = 0 Then
                    iLEControl = Me.Parameters.UserLEControl
                End If

                oLEConfigs = db.tblSSOALEConfigs.Where(Function(x) x.SSOALELEAdminControl = iLEControl AndAlso x.SSOALESSOATypeControl = iSSOAType).ToList()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetSSOAConfigs"), db)
            End Try
        End Using

        Return oLEConfigs
    End Function

    ''' <summary>
    ''' Get all the child records for the SSOALEConfig header record
    ''' </summary>
    ''' <param name="iLEConfigControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created  by RHR for v-8.5.4.003 on 10/10/2023 
    '''     Uses new SSOALEControl to get all the child API settings
    '''     For the tblSSOALEConfig header record
    ''' </remarks>
    Public Function GetSSOAConfigs(ByVal iLEConfigControl As Integer) As List(Of LTS.tblSSOAConfig)
        Dim blnRet As Boolean = False
        Dim lSSOAConfigs = New List(Of LTS.tblSSOAConfig)

        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try

                lSSOAConfigs = db.tblSSOAConfigs.Where(Function(x) x.SSOACSSOALEControl = iLEConfigControl).ToList()


            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetSSOAConfigs"), db)
            End Try

            Return lSSOAConfigs

        End Using
    End Function


    ''' <summary>
    ''' Get a specific tblSSOALEConfig record by Primary Key (SSOALEControl)
    ''' </summary>
    ''' <param name="iSSOALEControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.4.002 on 08/04/2023
    ''' </remarks>
    Public Function GettblSSOALEConfig(ByVal iSSOALEControl As Integer) As LTS.tblSSOALEConfig

        Dim oRet As LTS.tblSSOALEConfig
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try

                oRet = db.tblSSOALEConfigs.Where(Function(x) x.SSOALEControl = iSSOALEControl).FirstOrDefault()
                Return oRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GettblSSOALEConfig"), db)
            End Try

            Return Nothing

        End Using
    End Function


    ''' <summary>
    ''' Get a specific vtblSSOALEConfig record by Primary Key (SSOALEControl)
    ''' </summary>
    ''' <param name="iSSOALEControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.4.4 on 12/27/2023
    ''' </remarks>
    Public Function GetvtblSSOALEConfig(ByVal iSSOALEControl As Integer) As LTS.vtblSSOALEConfig

        Dim oRet As LTS.vtblSSOALEConfig
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try

                oRet = db.vtblSSOALEConfigs.Where(Function(x) x.SSOALEControl = iSSOALEControl).FirstOrDefault()
                Return oRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetvtblSSOALEConfig"), db)
            End Try

            Return Nothing

        End Using
    End Function



    Public Function GetFirsttblSSOALEAccountNumber(ByVal sSSOALEAuthCode As String, ByVal sSSOALEClientSecret As String) As String

        Dim oRet As String
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try

                Dim oData = db.tblSSOALEConfigs.Where(Function(x) x.SSOALEAuthCode = sSSOALEAuthCode AndAlso x.SSOALEClientSecret = sSSOALEClientSecret).FirstOrDefault()
                If Not oData Is Nothing AndAlso oData.SSOALEControl <> 0 Then
                    oRet = oData.SSOALEClientID
                End If

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetFirsttblSSOALEAccountNumber"), db)
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GettblSSOALEConfigFromToken(ByVal sSSOALEClientID As String, ByVal iSSOALESSOATypeControl As Integer) As LTS.tblSSOALEConfig

        Dim oRet As LTS.tblSSOALEConfig
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try

                oRet = db.tblSSOALEConfigs.Where(Function(x) x.SSOALEClientID = sSSOALEClientID AndAlso x.SSOALESSOATypeControl = iSSOALESSOATypeControl).FirstOrDefault()

                'If Not oData Is Nothing AndAlso oData.SSOALEControl <> 0 Then
                '    oRet = oData.SSOALEClientID
                'End If
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GettblSSOALEConfigFromToken"), db)
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Get the tblSSOALEConfig records by Single Sign on, Legal Entity, and provided filters (typically called from a grid control)
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.4.002 on 08/04/2023
    ''' </remarks>
    Public Function GettblSSOALEConfigs(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.tblSSOALEConfig()
        If filters Is Nothing Then Return Nothing
        Dim iLEAdminControl As Integer = filters.LEAdminControl
        If iLEAdminControl = 0 Then Return Nothing
        Dim oRet() As LTS.tblSSOALEConfig
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'Get the data iqueryable
                Dim iQuery As IQueryable(Of LTS.tblSSOALEConfig)
                iQuery = db.tblSSOALEConfigs.Where(Function(x) x.SSOALELEAdminControl = iLEAdminControl)
                If iQuery Is Nothing Then Return Nothing
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GettblSSOALEConfigs"), db)
            End Try

            Return Nothing

        End Using
    End Function


    ''' <summary>
    ''' Get the vtblSSOALEConfig records by Single Sign on, Legal Entity, and provided filters (typically called from a grid control)
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.4.004 on 12/27/2023
    ''' </remarks>
    Public Function GetvtblSSOALEConfigs(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vtblSSOALEConfig()
        If filters Is Nothing Then Return Nothing
        Dim iLEAdminControl As Integer = filters.LEAdminControl
        If iLEAdminControl = 0 Then Return Nothing
        Dim oRet() As LTS.vtblSSOALEConfig
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'Get the data iqueryable
                Dim iQuery As IQueryable(Of LTS.vtblSSOALEConfig)
                iQuery = db.vtblSSOALEConfigs.Where(Function(x) x.SSOALELEAdminControl = iLEAdminControl)
                If iQuery Is Nothing Then Return Nothing
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetvtblSSOALEConfigs"), db)
            End Try

            Return Nothing

        End Using
    End Function


    ''' <summary>
    ''' Insert a new tblSSOALEConfig when the primary key SSOALEControl is zero or update the existing record with changes
    ''' </summary>
    ''' <param name="oRecord"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.4.002 on 08/04/2023
    ''' </remarks>
    Public Function InsertOrUpdatetblSSOALEConfig(ByVal oRecord As LTS.tblSSOALEConfig) As LTS.tblSSOALEConfig
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                If oRecord.SSOALESSOAControl = 0 OrElse Not db.tblSingleSignOnAccounts.Any(Function(x) x.SSOAControl = oRecord.SSOALESSOAControl) Then
                    throwInvalidKeyParentRequiredException(New List(Of String) From {"Single Sign On Account", "Record"})
                End If

                If oRecord.SSOALELEAdminControl = 0 OrElse Not db.tblLegalEntityAdminRefSecurities.Any(Function(x) x.LEAdminControl = oRecord.SSOALELEAdminControl) Then
                    throwInvalidKeyParentRequiredException(New List(Of String) From {"Legal Entity", "Record"})
                End If

                oRecord.SSOALEModDate = Date.Now()
                oRecord.SSOALEModUser = Me.Parameters.UserName
                If oRecord.SSOALEControl = 0 Then
                    'Insert
                    db.tblSSOALEConfigs.InsertOnSubmit(oRecord)
                Else
                    'Update
                    db.tblSSOALEConfigs.Attach(oRecord, True)
                End If
                db.SubmitChanges()
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                ManageLinqDataExceptions(ex, buildProcedureName("InsertOrUpdatetblSSOALEConfig"), db)
            End Try

            Return oRecord

        End Using
    End Function

    ''' <summary>
    ''' Delete the tblSSOAConfig record using the full record
    ''' </summary>
    ''' <param name="oRecord"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.4.002 on 08/04/2023
    ''' </remarks>
    Public Function DeletetblSSOALEConfig(ByVal oRecord As LTS.tblSSOALEConfig) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                db.tblSSOALEConfigs.Attach(oRecord, True)
                db.tblSSOALEConfigs.DeleteOnSubmit(oRecord)
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeletetblSSOALEConfig"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    '''  Delete the tblSSOALEConfig record using the primary key SSOALEControl
    ''' </summary>
    ''' <param name="Control"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.4.002 on 08/04/2023
    ''' </remarks>
    Public Function DeletetblSSOALEConfig(ByVal Control As Integer) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Dim oTable = db.tblSSOALEConfigs
            Try
                Dim oRecord As LTS.tblSSOALEConfig = db.tblSSOALEConfigs.Where(Function(x) x.SSOALEControl = Control).FirstOrDefault()
                If (oRecord Is Nothing OrElse oRecord.SSOALEControl = 0) Then Return False
                'db.tblSSOALEConfigs.Attach(oRecord, True)
                db.tblSSOALEConfigs.DeleteOnSubmit(oRecord)
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeletetblSSOALEConfig"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Get the specific  tblSSOAConfig record by primary key 
    ''' </summary>
    ''' <param name="Control"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.4.002 on 08/04/2023
    ''' </remarks>
    Public Function GettblSSOAConfig(ByVal Control As Integer) As LTS.tblSSOAConfig
        Dim oRet As LTS.tblSSOAConfig
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try

                oRet = db.tblSSOAConfigs.Where(Function(x) x.SSOACControl = Control).FirstOrDefault()
                Return oRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GettblSSOAConfigs"), db)
            End Try

            Return Nothing

        End Using
    End Function


    ''' <summary>
    ''' Get the specific  vtblSSOAConfig record by primary key 
    ''' </summary>
    ''' <param name="Control"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.4.004  on 12/27/2023
    ''' </remarks>
    Public Function GetvtblSSOAConfig(ByVal Control As Integer) As LTS.vtblSSOAConfig
        Dim oRet As LTS.vtblSSOAConfig
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try

                oRet = db.vtblSSOAConfigs.Where(Function(x) x.SSOACControl = Control).FirstOrDefault()
                Return oRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetvtblSSOAConfigs"), db)
            End Try

            Return Nothing

        End Using
    End Function


    ''' <summary>
    ''' Get the tblSSOAConfigs records by Legal Entity Single Sign Type and provided filters (typically called from a grid control)
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.4.002 on 08/04/2023
    ''' </remarks>
    Public Function GettblSSOAConfigs(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.tblSSOAConfig()
        If filters Is Nothing Then Return Nothing
        Dim iSSOALEControl As Integer = filters.ParentControl
        If iSSOALEControl = 0 Then Return Nothing
        Dim oRet() As LTS.tblSSOAConfig
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'Get the data iqueryable
                Dim iQuery As IQueryable(Of LTS.tblSSOAConfig)
                iQuery = db.tblSSOAConfigs.Where(Function(x) x.SSOACSSOALEControl = iSSOALEControl)
                If iQuery Is Nothing Then Return Nothing
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GettblSSOAConfigs"), db)
            End Try

            Return Nothing

        End Using
    End Function


    ''' <summary>
    ''' Get the vtblSSOAConfigs records by Legal Entity Single Sign Type and provided filters (typically called from a grid control)
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.4.004 on 12/27/2023
    ''' </remarks>
    Public Function GetvtblSSOAConfigs(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vtblSSOAConfig()
        If filters Is Nothing Then Return Nothing
        Dim iSSOALEControl As Integer = filters.ParentControl
        If iSSOALEControl = 0 Then Return Nothing
        Dim oRet() As LTS.vtblSSOAConfig
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'Get the data iqueryable
                Dim iQuery As IQueryable(Of LTS.vtblSSOAConfig)
                iQuery = db.vtblSSOAConfigs.Where(Function(x) x.SSOACSSOALEControl = iSSOALEControl)
                If iQuery Is Nothing Then Return Nothing
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetvtblSSOAConfigs"), db)
            End Try

            Return Nothing

        End Using
    End Function



    ''' <summary>
    ''' Insert a new tblSSOAConfig when the primary key SSOACControl is zero or update the existing record with changes
    ''' </summary>
    ''' <param name="oRecord"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.4.002 on 08/04/2023
    ''' </remarks>
    Public Function InsertOrUpdatetblSSOAConfig(ByVal oRecord As LTS.tblSSOAConfig) As LTS.tblSSOAConfig
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                If oRecord.SSOACSSOALEControl = 0 OrElse Not db.tblSSOALEConfigs.Any(Function(x) x.SSOALEControl = oRecord.SSOACSSOALEControl) Then
                    throwInvalidKeyParentRequiredException(New List(Of String) From {"Legal Entity Single Sign On Account", "Record"})
                End If

                oRecord.SSOACModDate = Date.Now()
                oRecord.SSOACModUser = Me.Parameters.UserName
                If oRecord.SSOACControl = 0 Then
                    'Insert
                    db.tblSSOAConfigs.InsertOnSubmit(oRecord)
                Else
                    'Update
                    db.tblSSOAConfigs.Attach(oRecord, True)
                End If
                db.SubmitChanges()
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                ManageLinqDataExceptions(ex, buildProcedureName("InsertOrUpdatetblSSOAConfig"), db)
            End Try

            Return oRecord

        End Using
    End Function

    ''' <summary>
    ''' Delete the tblSSOAConfig record using the full record
    ''' </summary>
    ''' <param name="oRecord"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.4.002 on 08/04/2023
    ''' </remarks>
    Public Function DeletetblSSOAConfig(ByVal oRecord As LTS.tblSSOAConfig) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                db.tblSSOAConfigs.Attach(oRecord, True)
                db.tblSSOAConfigs.DeleteOnSubmit(oRecord)
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeletetblSSOAConfig"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Delete the tblSSOAConfig record using the primary key SSOACControl
    ''' </summary>
    ''' <param name="Control"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.4.002 on 08/04/2023
    ''' </remarks>
    Public Function DeletetblSSOAConfig(ByVal Control As Integer) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim oRecord As LTS.tblSSOAConfig = db.tblSSOAConfigs.Where(Function(x) x.SSOACControl = Control).FirstOrDefault()
                If (oRecord Is Nothing OrElse oRecord.SSOACControl = 0) Then Return False
                'db.tblSSOAConfigs.Attach(oRecord, True)
                db.tblSSOAConfigs.DeleteOnSubmit(oRecord)
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeletetblSSOAConfig"), db)
            End Try
        End Using
        Return blnRet
    End Function


    Public Function GettblSingleSignOnAccountControl(ByVal Name As String) As Integer
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim oRecords = From d In db.tblSingleSignOnAccounts Where d.SSOAName = Name Select d
                Dim SSOAControl As Integer = oRecords.Select(Function(x) x.SSOAControl).FirstOrDefault()
                Return SSOAControl

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GettblSingleSignOnAccountControl"), sysErrorParameters.sysErrorState.SystemLevelFault, sysErrorParameters.sysErrorSeverity.Unexpected)
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GettblSingleSignOnAccountControl(ByVal Name As String, ByVal ClientSecret As String) As Integer
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim oRecords = From d In db.tblSingleSignOnAccounts Where d.SSOAName = Name And d.SSOAClientSecret = ClientSecret Select d
                Dim SSOAControl As Integer = oRecords.Select(Function(x) x.SSOAControl).FirstOrDefault()
                Return SSOAControl

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GettblSingleSignOnAccountControl"), sysErrorParameters.sysErrorState.SystemLevelFault, sysErrorParameters.sysErrorSeverity.Unexpected)
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GettblSingleSignOnAccountsFiltered(Optional ByVal page As Integer = 1, Optional ByVal pagesize As Integer = 1000) As DTO.tblSingleSignOnAccount()
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1
                'count the record

                Try
                    intRecordCount = getScalarInteger("select COUNT(*) from dbo.tblSingleSignOnAccount")

                Catch ex As Exception
                    'ignore any record count errors
                End Try
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize

                'Return all the contacts that match the criteria sorted by name
                Dim tblSingleSignOnAccounts() As DTO.tblSingleSignOnAccount = (
                From d In db.tblSingleSignOnAccounts
                Order By d.SSOAControl
                Select selectDTOData(d, page, intPageCount, intRecordCount, pagesize)).Skip(intSkip).Take(pagesize).ToArray()
                Return tblSingleSignOnAccounts

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GettblSingleSignOnAccountsFiltered"), sysErrorParameters.sysErrorState.SystemLevelFault, sysErrorParameters.sysErrorSeverity.Unexpected)
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Used by System processes forces delete and bypasses validation 
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <remarks></remarks>
    Public Sub SystemDelete(ByVal oData As DTO.tblSingleSignOnAccount)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            'Create New Record
            Dim nObject = CopyDTOToLinq(oData)
            db.tblSingleSignOnAccounts.Attach(nObject, True)
            db.tblSingleSignOnAccounts.DeleteOnSubmit(nObject)
            Try
                db.SubmitChanges()
            Catch ex As SqlException
                throwSQLFaultException(ex.Message)
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
                    throwUnExpectedFaultException(ex, buildProcedureName("SystemDelete"), sysErrorParameters.sysErrorState.SystemLevelFault, sysErrorParameters.sysErrorSeverity.Unexpected)
                End Try
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("SystemDelete"), sysErrorParameters.sysErrorState.SystemLevelFault, sysErrorParameters.sysErrorSeverity.Unexpected)
            End Try
            DeleteCleanUp(nObject)
        End Using
    End Sub


    ''' <summary>
    ''' Executes spGetSingleSignOnAccountByUser
    ''' Puts the LTS.spResults in a WCFResults object
    ''' Basically breaks down the sp return table results 
    ''' into the DTO.WCFResults object
    ''' Gets all SSOAs (Name, Username, and Password) for the
    ''' specified UserSecurityControl
    ''' To get username/password for the user for a specific
    ''' SSOA pass in the SSOAControl filter, else pass in 0
    ''' </summary>
    ''' <param name="UserSecurityControl"></param>
    ''' <param name="SSOAControl"></param>
    ''' <returns>List(Of DTO.WCFResults)</returns>
    ''' <remarks>
    ''' Added by LVV on 3/17/16 for v-7.0.5.1 DAT
    ''' </remarks>
    Public Function GetSingleSignOnAccountByUser(ByVal UserSecurityControl As Integer, ByVal SSOAControl As Utilities.SSOAAccount) As DTO.WCFResults()
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim oWcfRets As New List(Of DTO.WCFResults)

                Dim ltsRes = (From d In db.spGetSingleSignOnAccountByUser(UserSecurityControl, SSOAControl) Select d).ToArray()

                If Not ltsRes Is Nothing Then
                    For Each res In ltsRes
                        Dim wcfRet As New DTO.WCFResults

                        If res.SSOAControl <> 0 AndAlso Not res.SSOAControl Is Nothing Then
                            wcfRet.updateKeyFields("SSOASecurityXrefControl", res.SSOASecurityXrefControl)
                            wcfRet.updateKeyFields("SSOAName", If(res.SSOAName, ""))
                            wcfRet.updateKeyFields("SSOAControl", res.SSOAControl)
                            wcfRet.updateKeyFields("SSOALoginURL", If(res.SSOALoginURL, ""))
                            wcfRet.updateKeyFields("Username", If(res.Username, ""))
                            Dim dpass = DTran.Decrypt(res.Pass, "NGL")
                            wcfRet.updateKeyFields("Pass", If(dpass, ""))
                            wcfRet.updateKeyFields("RefID", If(res.RefID, ""))
                        End If

                        If res.ErrNumber <> 0 Then
                            Dim p As New List(Of String)
                            p.Add(If(res.SSOAControl, 0))
                            wcfRet.AddMessage(DataTransferObjects.WCFResults.MessageType.Warnings, res.ErrKey, p.ToArray())
                        End If
                        oWcfRets.Add(wcfRet)

                    Next

                    Return oWcfRets.ToArray()
                End If

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetSingleSignOnAccountByUser"))
            End Try

            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Executes spInsertOrUpdateSSOASecurityXref
    ''' Inserts any errors into WCFResults.Warnings
    ''' WCFResults.Success Flag will indicate if the
    ''' sp completed with errors (F) or not (T).
    ''' </summary>
    ''' <param name="UserSecurityControl"></param>
    ''' <param name="SSOAControl"></param>
    ''' <param name="UserName"></param>
    ''' <param name="Password"></param>
    ''' <returns>DTO.WCFResults</returns>
    ''' <remarks>
    ''' Added by LVV on 3/21/16 for v-7.0.5.1 DAT
    ''' </remarks>
    Public Function InsertOrUpdateSSOASecurityXref(ByVal UserSecurityControl As Integer, ByVal SSOAControl As Integer, ByVal UserName As String, ByVal Password As String) As DTO.WCFResults
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim wcfRet As New DTO.WCFResults
                wcfRet.Success = True
                Dim epass = DTran.Encrypt(Password, "NGL")

                Dim ltsRes = (From d In db.spInsertOrUpdateSSOASecurityXref(UserSecurityControl, SSOAControl, UserName, epass) Select d).FirstOrDefault()

                If Not ltsRes Is Nothing Then

                    If ltsRes.ErrNumber <> 0 Then
                        wcfRet.Success = False
                        Dim p(1) As String
                        If Not String.IsNullOrWhiteSpace(ltsRes.ParamName) Then
                            p(0) = ltsRes.ParamName
                            p(1) = If(ltsRes.ParamValue, "")
                        End If

                        wcfRet.AddMessage(DataTransferObjects.WCFResults.MessageType.Warnings, ltsRes.ErrKey, p)
                    End If

                    Return wcfRet
                End If

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertOrUpdateSSOASecurityXref"))
            End Try

            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Executes spDeleteSSOASecurityXref
    ''' Inserts any errors into WCFResults.Warnings
    ''' WCFResults.Success Flag will indicate if the
    ''' sp completed with errors (F) or not (T).
    ''' </summary>
    ''' <param name="SSOASecurityXrefControl"></param>
    ''' <returns>DTO.WCFResults</returns>
    ''' <remarks>
    ''' Added by LVV on 3/25/16 for v-7.0.5.1 DAT
    ''' </remarks>
    Public Function DeleteSSOASecurityXref(ByVal SSOASecurityXrefControl As Integer) As DTO.WCFResults
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim wcfRet As New DTO.WCFResults
                wcfRet.Success = True

                Dim ltsRes = (From d In db.spDeleteSSOASecurityXref(SSOASecurityXrefControl) Select d).FirstOrDefault()

                If Not ltsRes Is Nothing Then

                    If ltsRes.ErrNumber <> 0 Then
                        wcfRet.Success = False
                        Dim p(1) As String
                        If Not String.IsNullOrWhiteSpace(ltsRes.ParamName) Then
                            p(0) = ltsRes.ParamName
                            p(1) = If(ltsRes.ParamValue, "")
                        End If

                        wcfRet.AddMessage(DataTransferObjects.WCFResults.MessageType.Warnings, ltsRes.ErrKey, p)
                    End If

                    Return wcfRet
                End If

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteSSOASecurityXref"))
            End Try

            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Checks to see if a user has an account for the specified SSOA
    ''' Returns true or false
    ''' </summary>
    ''' <param name="UserSecurityControl"></param>
    ''' <param name="SSOAControl"></param>
    ''' <returns>Boolean</returns>
    ''' <remarks>
    ''' Added by LVV 5/23/16 for v-7.0.5.110 DAT
    ''' </remarks>
    Public Function DoesUserHaveSSOAAccount(ByVal UserSecurityControl As Integer, ByVal SSOAControl As Integer) As Boolean
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Dim blnRet As Boolean = False
            Try
                'Get compcontrol number
                Dim blnHasAccount = db.[udfDoesUserHaveSSOA](UserSecurityControl, SSOAControl)

                Return blnHasAccount

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DoesUserHaveSSOAAccount"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Checks to see if a user has an account for the specified SSOA by SSOAName
    ''' Returns true or false
    ''' </summary>
    ''' <param name="UserSecurityControl"></param>
    ''' <param name="SSOAAct"></param>
    ''' <returns>Boolean</returns>
    ''' <remarks>
    ''' Added by LVV on 1/13/17 for v-8.0 Next Stop
    ''' Modified by RHR on 12/11/2018
    '''   we now use an ENUM to ensure future support if the text changes in the database
    ''' </remarks>
    Public Function DoesUserHaveSSOAAccount(ByVal UserSecurityControl As Integer, ByVal SSOAAct As Utilities.SSOAAccount) As Boolean
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Dim blnRet As Boolean = False
            Try

                'Dim SSOACont = (From d In db.tblSingleSignOnAccounts
                '                Where d.SSOAName = SSOAName
                '                Select d.SSOAControl).FirstOrDefault()

                Dim blnHasAccount = db.[udfDoesUserHaveSSOA](UserSecurityControl, SSOAAct)

                Return blnHasAccount

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DoesUserHaveSSOAAccount"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Selects SSOA data by SSOAName Filtered by current user in parameter property
    ''' returns an empty vSSOA record with a zero SSOAControl number if the user does 
    ''' not have an account.
    ''' </summary>
    ''' <param name="SSOAAct"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.0 on 01/31/2017
    '''  returns the first SSOA account for SSOAName for the current user in the parameter table
    ''' </remarks>
    Public Function GetSSOADataForCurrentUser(ByVal SSOAAct As Utilities.SSOAAccount) As LTS.vSSOA
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Dim oRet As New LTS.vSSOA
            Try
                oRet = db.vSSOAs.Where(Function(x) x.UserName = Me.Parameters.UserName And x.SSOAControl = SSOAAct).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetSSOADataForCurrentUser"))
            End Try

            Return oRet

        End Using
    End Function

    ''' <summary>
    ''' Looks up the SSO Account data for the current user using the authenticated user name
    ''' </summary>
    ''' <param name="computerUserName"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.0 on 4/6/2017
    ''' Rules:
    ''' 1. Check if the name exists in tblUserSecurity or tblUserSecurityLocations
    ''' 2. If it does exists check if NGL is the SSOA authenticating authority, and run standard NGL User Authentication procedures
    ''' 3. if authentication fails return an error code and message
    ''' </remarks>
    Public Function getSSOAccount(ByVal computerUserName As String) As Models.SSOAccount
        Dim oSSOAccount As New Models.SSOAccount

    End Function

#Region "TMS 365"

    ''' <summary>
    ''' Returns an array of the SSOA for which the user is currently subscribed. Does not return the password to the caller
    ''' for security reasons. This method is used by the LEUsers page to populate the users child grid for listing/editing
    ''' Single Sign On Accounts by user
    ''' </summary>
    ''' <param name="USC"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added by LVV for v-8.1 on 03/28/2018
    ''' </remarks>
    Public Function GetSSOAListForUser365(ByVal USC As Integer) As Models.SingleSignOn()
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'I return a Model because I want to limit the fields from vSSOA that I return and
                'also I do not want to expose the database field names to the client for security reasons
                '(since these fields are directly related to security and login credentials)
                Dim oRet As New List(Of Models.SingleSignOn)

                Dim ssoa = db.vSSOAs.Where(Function(x) x.UserSecurityControl = USC).ToArray()

                For Each s In ssoa
                    Dim r As New Models.SingleSignOn
                    With r
                        .SSOAXControl = s.SSOASecurityXrefControl
                        .SSOAXUN = s.SSOASecurityXrefUserName
                        .SSOAXRefID = s.SSOASecurityXrefReferenceID
                        .SSOAControl = s.SSOAControl
                        .SSOAName = s.SSOAName
                        .SSOADesc = s.SSOADesc
                        .USC = s.UserSecurityControl
                        .UserName = s.UserName
                    End With
                    oRet.Add(r)
                Next

                Return oRet.ToArray()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetSSOAListForUser365"))
            End Try
            Return Nothing
        End Using
    End Function

    Public Function DeleteSSOAXref(ByVal Control As Integer) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Dim oTable = db.tblSSOASecurityXrefs
            Try
                Dim oRecord As LTS.tblSSOASecurityXref = db.tblSSOASecurityXrefs.Where(Function(x) x.SSOASecurityXrefControl = Control).FirstOrDefault()
                If (oRecord Is Nothing OrElse oRecord.SSOASecurityXrefControl = 0) Then Return False
                'oTable.Attach(oRecord, True)
                oTable.DeleteOnSubmit(oRecord)
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteSSOAXref"))
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' I wrote another version of this specific to 365. This is because I needed a flag to determine if
    ''' we are going to update the password because in 365 we don't send the password through to the client 
    ''' because it is more secure as we do not need to see it in the UI. Also, because we added a new field to 
    ''' tblSSOASecurityXref for RefID and I didn't want to have to make major changes to the Service and FMClient
    ''' and all those solutions which DAT uses the other method InsertOrUpdateSSOASecurityXref.
    ''' </summary>
    ''' <param name="sso"></param>
    ''' <remarks>
    ''' Added By LVV on 4/4/18 for v-8.1 TMS 365
    ''' </remarks>
    Public Sub InsertOrUpdateSSOASecurityXref365(ByVal sso As Models.SingleSignOn)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                If sso.SSOAXControl = 0 Then
                    'Insert
                    Dim epass = DTran.Encrypt(sso.SSOAXPass, "NGL")
                    Dim oRecord As New LTS.tblSSOASecurityXref
                    With oRecord
                        .SSOAControl = sso.SSOAControl
                        .UserSecurityControl = sso.USC
                        .SSOASecurityXrefUserName = sso.SSOAXUN
                        .SSOASecurityXrefPassword = epass
                        .SSOASecurityXrefReferenceID = sso.SSOAXRefID
                        .SSOASecurityXrefModDate = Date.Now
                        .SSOASecurityXrefModUser = Parameters.UserName
                    End With

                    db.tblSSOASecurityXrefs.InsertOnSubmit(oRecord)
                Else
                    'Update
                    Dim oRecord = db.tblSSOASecurityXrefs.Where(Function(x) x.SSOASecurityXrefControl = sso.SSOAXControl).FirstOrDefault()
                    If Not oRecord Is Nothing Then
                        oRecord.SSOASecurityXrefModDate = Date.Now
                        oRecord.SSOASecurityXrefModUser = Parameters.UserName
                        oRecord.SSOASecurityXrefReferenceID = sso.SSOAXRefID
                        oRecord.SSOASecurityXrefUserName = sso.SSOAXUN
                        If sso.UpdateP Then
                            Dim epass = DTran.Encrypt(sso.SSOAXPass, "NGL")
                            oRecord.SSOASecurityXrefPassword = epass
                        End If
                    End If
                    'db.tblSSOASecurityXrefs.Attach(oRecord, True)
                End If

                db.SubmitChanges()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertOrUpdateSSOASecurityXref365"), db)
            End Try
        End Using
    End Sub


    Public Function GettblSingleSignOnAccounts365(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.tblSingleSignOnAccount()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.tblSingleSignOnAccount
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'Get the data iqueryable
                Dim iQuery As IQueryable(Of LTS.tblSingleSignOnAccount)
                iQuery = db.tblSingleSignOnAccounts
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GettblSingleSignOnAccounts365"), db)
            End Try
        End Using
        Return Nothing
    End Function

    'TODO: Need to figure out way to add logic so on add only uses control number larger than 100 (save for system)
    Public Function InsertOrUpdateSingleSignOnAccount(ByVal oData As LTS.tblSingleSignOnAccount) As Boolean
        Dim blnRet As Boolean = False
        If oData Is Nothing Then Return False 'nothing to do
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                oData.SSOAModUser = Parameters.UserName
                oData.SSOAModDate = Date.Now
                If oData.SSOAControl = 0 Then
                    db.tblSingleSignOnAccounts.InsertOnSubmit(oData)
                Else
                    db.tblSingleSignOnAccounts.Attach(oData, True)
                End If
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertOrUpdateSingleSignOnAccount"), db)
            End Try
        End Using
        Return blnRet
    End Function

    'TODO: Need to add logic so can't delete System SSOA records (control < x)
    Public Function DeleteSingleSignOnAccount(ByVal iSSOAControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        If iSSOAControl = 0 Then Return False 'nothing to do
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'verify the record
                Dim oExisting = db.tblSingleSignOnAccounts.Where(Function(x) x.SSOAControl = iSSOAControl).FirstOrDefault()
                If oExisting Is Nothing OrElse oExisting.SSOAControl = 0 Then Return True
                db.tblSingleSignOnAccounts.DeleteOnSubmit(oExisting)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteSingleSignOnAccount"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' For each user in provided Users,
    ''' if NEXTStop is checked - if the user does not already have a NEXTStop ssoa record create one (don't need un or pass for NEXTStop account)
    ''' if P44 is checked - if the user does not already have a P44 ssoa record create one, else update the existing one
    ''' if CopyFromSSOAXCtrl is populated - copy the record with the provided control to all the selected users if not exist, else update existing
    ''' </summary>
    ''' <param name="data"></param>
    ''' <remarks></remarks>
    Public Function MassUpdateSSOASecurityXref365(ByVal data As Models.MassUpdateSingleSignOn) As String
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Dim strMsg As String = ""
            Try
                If data.P44 Then
                    Dim oLocalize As New NGLcmLocalizeKeyValuePairData(Parameters)
                    If Not String.Equals(data.SSOAXPass, data.NewPass) Then
                        strMsg = oLocalize.GetLocalizedValueByKey("PasswordNotMatch", "The passwords do not match")
                        Return strMsg
                    End If
                End If
                Dim blnCopySettings As Boolean = False
                Dim ssoax As LTS.tblSSOASecurityXref = Nothing
                If data.CopyFromSSOAXCtrl <> 0 Then
                    ssoax = db.tblSSOASecurityXrefs.Where(Function(x) x.SSOASecurityXrefControl = data.CopyFromSSOAXCtrl).FirstOrDefault()
                    If Not ssoax Is Nothing Then blnCopySettings = True
                End If
                For Each usc In data.UserControls
                    If data.NEXTStop Then
                        'If the user does not already have a NEXTStop ssoa record create one (don't need un or pass for NEXTStop account)
                        If Not db.tblSSOASecurityXrefs.Any(Function(x) x.UserSecurityControl = usc AndAlso x.SSOAControl = Utilities.SSOAAccount.NextStop) Then
                            Dim oRecord As New LTS.tblSSOASecurityXref
                            populateSSOASecurityXref(oRecord, Utilities.SSOAAccount.NextStop, usc, "", "", "", False)
                            db.tblSSOASecurityXrefs.InsertOnSubmit(oRecord)
                        End If
                    End If
                    If data.P44 Then
                        'If the user does not already have a P44 ssoa record create one, else update the existing one
                        If Not db.tblSSOASecurityXrefs.Any(Function(x) x.UserSecurityControl = usc AndAlso x.SSOAControl = Utilities.SSOAAccount.P44) Then
                            'Insert
                            Dim oRecord As New LTS.tblSSOASecurityXref
                            populateSSOASecurityXref(oRecord, Utilities.SSOAAccount.P44, usc, data.SSOAXUN, data.SSOAXPass, data.SSOAXRefID, True)
                            db.tblSSOASecurityXrefs.InsertOnSubmit(oRecord)
                        Else
                            'Update
                            Dim oRecord = db.tblSSOASecurityXrefs.Where(Function(x) x.UserSecurityControl = usc AndAlso x.SSOAControl = Utilities.SSOAAccount.P44).FirstOrDefault()
                            populateSSOASecurityXref(oRecord, Utilities.SSOAAccount.P44, usc, data.SSOAXUN, data.SSOAXPass, data.SSOAXRefID, True)
                        End If
                    End If
                    If blnCopySettings Then
                        'If the user does not already have a record for the selected copy from ssoa create one, else update the existing one
                        If Not db.tblSSOASecurityXrefs.Any(Function(x) x.UserSecurityControl = usc AndAlso x.SSOAControl = ssoax.SSOAControl) Then
                            'Insert
                            Dim oRecord As New LTS.tblSSOASecurityXref
                            populateSSOASecurityXref(oRecord, ssoax.SSOAControl, usc, ssoax.SSOASecurityXrefUserName, ssoax.SSOASecurityXrefPassword, ssoax.SSOASecurityXrefReferenceID, False)
                            db.tblSSOASecurityXrefs.InsertOnSubmit(oRecord)
                        Else
                            'Update
                            Dim oRecord = db.tblSSOASecurityXrefs.Where(Function(x) x.UserSecurityControl = usc AndAlso x.SSOAControl = ssoax.SSOAControl).FirstOrDefault()
                            populateSSOASecurityXref(oRecord, ssoax.SSOAControl, usc, ssoax.SSOASecurityXrefUserName, ssoax.SSOASecurityXrefPassword, ssoax.SSOASecurityXrefReferenceID, False)
                        End If
                    End If
                Next
                db.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("MassUpdateSSOASecurityXref365"), db)
            End Try
            Return strMsg
        End Using
    End Function

    Private Sub populateSSOASecurityXref(ByRef lts As LTS.tblSSOASecurityXref, ByVal SSOAControl As Integer, ByVal usc As Integer, ByVal username As String, ByVal pass As String, ByVal refID As String, ByVal blnEncryptPass As Boolean)
        Dim epass As String = pass
        If blnEncryptPass AndAlso Not String.IsNullOrWhiteSpace(pass) Then epass = DTran.Encrypt(pass, "NGL")
        If Not lts Is Nothing Then
            With lts
                .SSOAControl = SSOAControl
                .UserSecurityControl = usc
                .SSOASecurityXrefUserName = username
                .SSOASecurityXrefPassword = epass
                .SSOASecurityXrefReferenceID = refID
                .SSOASecurityXrefModDate = Date.Now
                .SSOASecurityXrefModUser = Parameters.UserName
            End With
        End If
    End Sub

#End Region

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.tblSingleSignOnAccount)
        'Create New Record
        Return New LTS.tblSingleSignOnAccount With {.SSOAControl = d.SSOAControl _
                                                   , .SSOAName = d.SSOAName _
                                                   , .SSOADesc = d.SSOADesc _
                                                   , .SSOAClientID = d.SSOAClientID _
                                                   , .SSOALoginURL = d.SSOALoginURL _
                                                   , .SSOADataURL = d.SSOADataURL _
                                                   , .SSOARedirectURL = d.SSOARedirectURL _
                                                   , .SSOAClientSecret = d.SSOAClientSecret _
                                                   , .SSOAAuthCode = d.SSOAAuthCode _
                                                   , .SSOAModDate = Date.Now _
                                                   , .SSOAModUser = Parameters.UserName _
                                                   , .SSOAUpdated = If(d.SSOAUpdated Is Nothing, New Byte() {}, d.SSOAUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GettblSingleSignOnAccountFiltered(Control:=CType(LinqTable, LTS.tblSingleSignOnAccount).SSOAControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim source As LTS.tblSingleSignOnAccount = TryCast(LinqTable, LTS.tblSingleSignOnAccount)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.tblSingleSignOnAccounts
                       Where d.SSOAControl = source.SSOAControl
                       Select New DTO.QuickSaveResults With {.Control = d.SSOAControl _
                                                            , .ModDate = d.SSOAModDate _
                                                            , .ModUser = d.SSOAModUser _
                                                            , .Updated = d.SSOAUpdated.ToArray}).First

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
    ''' 
    ''' </summary>
    ''' <param name="d"></param>
    ''' <param name="page"></param>
    ''' <param name="pagecount"></param>
    ''' <param name="recordcount"></param>
    ''' <param name="pagesize"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.1 on 03/04/2022 added ability to store LEAdminControl with setting
    '''  If Zero(default) setting applies To all legal entities.
    ''' </remarks>
    Friend Function selectDTOData(ByVal d As LTS.tblSingleSignOnAccount, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.tblSingleSignOnAccount
        Return New DTO.tblSingleSignOnAccount With {.SSOAControl = d.SSOAControl _
                                                   , .SSOAName = d.SSOAName _
                                                   , .SSOADesc = d.SSOADesc _
                                                   , .SSOAClientID = d.SSOAClientID _
                                                   , .SSOALoginURL = d.SSOALoginURL _
                                                   , .SSOADataURL = d.SSOADataURL _
                                                   , .SSOARedirectURL = d.SSOARedirectURL _
                                                   , .SSOAClientSecret = d.SSOAClientSecret _
                                                   , .SSOAAuthCode = d.SSOAAuthCode _
                                                   , .SSOAModDate = d.SSOAModDate _
                                                   , .SSOAModUser = d.SSOAModUser _
                                                   , .Page = page _
                                                   , .Pages = pagecount _
                                                   , .RecordCount = recordcount _
                                                   , .PageSize = pagesize _
                                                   , .SSOAUpdated = d.SSOAUpdated.ToArray()}
    End Function

#End Region

End Class

Public Class NGLtblUserSecurityAccessTokenData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        'Dim db As New NGLMASSecurityDataContext(ConnectionString)
        'Me.LinqTable = db.tblUserSecurityAccessTokens
        'Me.LinqDB = db
        Me.SourceClass = "NGLtblUserSecurityAccessTokenData"
    End Sub

    ''' <summary>
    ''' Must call loadParameterSettings manually when using the default constructor
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASSecurityDataContext(ConnectionString)
                _LinqTable = db.tblUserSecurityAccessTokens
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
        Return GettblUserSecurityAccessTokenFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GettblUserSecurityAccessTokensFiltered()
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

        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim tblUserSecurityAccessToken As DTO.tblUserSecurityAccessToken

                If LowerControl <> 0 Then
                    tblUserSecurityAccessToken = (
                   From d In db.tblUserSecurityAccessTokens
                   Where d.USATControl >= LowerControl
                   Order By d.USATControl
                   Select selectDTOData(d)).FirstOrDefault
                Else
                    'Zero indicates that we should get the record with the lowest control number even if it is below zero
                    tblUserSecurityAccessToken = (
                   From d In db.tblUserSecurityAccessTokens
                   Order By d.USATControl
                   Select selectDTOData(d)).FirstOrDefault
                End If



                Return tblUserSecurityAccessToken

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetFirstRecord"), sysErrorParameters.sysErrorState.SystemLevelFault, sysErrorParameters.sysErrorSeverity.Unexpected)
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
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try

                'Get the first record that matches the provided criteria
                Dim tblUserSecurityAccessToken As DTO.tblUserSecurityAccessToken = (
                From d In db.tblUserSecurityAccessTokens
                Where d.USATControl < CurrentControl
                Order By d.USATControl Descending
                Select selectDTOData(d)).FirstOrDefault


                Return tblUserSecurityAccessToken

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetPreviousRecord"), sysErrorParameters.sysErrorState.SystemLevelFault, sysErrorParameters.sysErrorSeverity.Unexpected)
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
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try

                'Get the first record that matches the provided criteria
                Dim tblUserSecurityAccessToken As DTO.tblUserSecurityAccessToken = (
                From d In db.tblUserSecurityAccessTokens
                Where d.USATControl > CurrentControl
                Order By d.USATControl
                Select selectDTOData(d)).FirstOrDefault


                Return tblUserSecurityAccessToken

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetNextRecord"), sysErrorParameters.sysErrorState.SystemLevelFault, sysErrorParameters.sysErrorSeverity.Unexpected)
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
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim tblUserSecurityAccessToken As DTO.tblUserSecurityAccessToken

                If UpperControl <> 0 Then

                    tblUserSecurityAccessToken = (
                    From d In db.tblUserSecurityAccessTokens
                    Where d.USATControl >= UpperControl
                    Order By d.USATControl
                    Select selectDTOData(d)).FirstOrDefault
                Else
                    'Zero indicates that we should get the hightest USATcontrol record
                    tblUserSecurityAccessToken = (
                    From d In db.tblUserSecurityAccessTokens
                    Order By d.USATControl Descending
                    Select selectDTOData(d)).FirstOrDefault

                End If


                Return tblUserSecurityAccessToken

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetLastRecord"), sysErrorParameters.sysErrorState.SystemLevelFault, sysErrorParameters.sysErrorSeverity.Unexpected)
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GettblUserSecurityAccessTokenFiltered(ByVal Control As Integer) As DTO.tblUserSecurityAccessToken
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim tblUserSecurityAccessToken As DTO.tblUserSecurityAccessToken = (
                From d In db.tblUserSecurityAccessTokens
                Where
                    d.USATControl = Control
                Select selectDTOData(d)).First

                Return tblUserSecurityAccessToken

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GettblUserSecurityAccessTokenFiltered"), sysErrorParameters.sysErrorState.SystemLevelFault, sysErrorParameters.sysErrorSeverity.Unexpected)
            End Try

            Return Nothing

        End Using
    End Function


    Public Function GettblUserSecurityAccessTokenFiltered(ByVal UserSecurityControl As Integer, ByVal SSOAAct As Utilities.SSOAAccount) As DTO.tblUserSecurityAccessToken
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim tblUserSecurityAccessToken As DTO.tblUserSecurityAccessToken = (
                From d In db.tblUserSecurityAccessTokens Join s In db.tblSingleSignOnAccounts On d.USATSSOAControl Equals s.SSOAControl
                Where
                    d.USATUserSecurityControl = UserSecurityControl And s.SSOAControl = SSOAAct
                Select selectDTOData(d)).FirstOrDefault

                Return tblUserSecurityAccessToken

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GettblUserSecurityAccessTokenFiltered"), sysErrorParameters.sysErrorState.SystemLevelFault, sysErrorParameters.sysErrorSeverity.Unexpected)
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Query the tblUserSecurityAccessToken and return record or empty object
    ''' </summary>
    ''' <param name="SSOAControl"></param>
    ''' <param name="Token"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR 12/15/2015 for v-7.0.4
    '''   Added FirstOrDefault() logic to minimize E_NoData exceptions
    ''' </remarks>
    Public Function GettblUserSecurityAccessTokenFilteredByToken(ByVal SSOAControl As Integer, ByVal Token As String) As DTO.tblUserSecurityAccessToken
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim tblUserSecurityAccessToken As DTO.tblUserSecurityAccessToken = (
                From d In db.tblUserSecurityAccessTokens
                Where
                    d.USATSSOAControl = SSOAControl And d.USATToken = Token And d.USATExpires > Date.Now()
                Select selectDTOData(d)).FirstOrDefault()


                Return tblUserSecurityAccessToken

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GettblUserSecurityAccessTokenFilteredByToken"), sysErrorParameters.sysErrorState.SystemLevelFault, sysErrorParameters.sysErrorSeverity.Critical)
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GettblUserSecurityAccessTokensFiltered(Optional ByVal page As Integer = 1, Optional ByVal pagesize As Integer = 1000) As DTO.tblUserSecurityAccessToken()
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1
                'count the record
                Dim oRecords = From d In db.tblUserSecurityAccessTokens Select d
                intRecordCount = oRecords.Count
                If intRecordCount < 1 Then Return Nothing
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize

                'Return all the records that match the criteria sorted by name
                Dim tblUserSecurityAccessTokens() As DTO.tblUserSecurityAccessToken = (
                From d In oRecords
                Order By d.USATControl
                Select selectDTOData(d, page, intPageCount, intRecordCount, pagesize)).Skip(intSkip).Take(pagesize).ToArray()

                Return tblUserSecurityAccessTokens

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GettblUserSecurityAccessTokensFiltered"), sysErrorParameters.sysErrorState.SystemLevelFault, sysErrorParameters.sysErrorSeverity.Unexpected)
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Used by System processes forces delete and bypasses validation 
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <remarks></remarks>
    Public Sub SystemDelete(ByVal oData As DTO.tblUserSecurityAccessToken)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            'Create New Record
            Dim nObject = CopyDTOToLinq(oData)
            db.tblUserSecurityAccessTokens.Attach(nObject, True)
            db.tblUserSecurityAccessTokens.DeleteOnSubmit(nObject)
            Try
                db.SubmitChanges()
            Catch ex As SqlException
                throwSQLFaultException(ex.Message)
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
                    throwUnExpectedFaultException(ex, buildProcedureName("SystemDelete"), sysErrorParameters.sysErrorState.SystemLevelFault, sysErrorParameters.sysErrorSeverity.Unexpected)
                End Try
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("SystemDelete"), sysErrorParameters.sysErrorState.SystemLevelFault, sysErrorParameters.sysErrorSeverity.Unexpected)
            End Try
            DeleteCleanUp(nObject)
        End Using
    End Sub

    Public Function InsertOrUpdatetblUserSecurityAccessToken(ByVal UserSecurityControl As Integer, ByVal SSOAControl As Integer, ByVal tokenString As String, ByVal Expires As Date, Optional ByVal USATUserID As String = Nothing) As DTO.WCFResults
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim wcfRet As New DTO.WCFResults
                wcfRet.Success = True

                If SSOAControl = 0 Then
                    Dim temp = db.tblUserSecurities.Where(Function(x) x.UserSecurityControl = UserSecurityControl).Select(Function(x) x.UserSSOAControl).FirstOrDefault()
                    SSOAControl = temp
                End If

                Dim ltsRes = (From d In db.spInsertOrUpdatetblUserSecurityAccessToken(UserSecurityControl, SSOAControl, tokenString, Expires, USATUserID) Select d).FirstOrDefault()

                If Not ltsRes Is Nothing Then

                    If ltsRes.ErrNumber <> 0 Then
                        wcfRet.Success = False
                        Dim p(1) As String
                        If Not String.IsNullOrWhiteSpace(ltsRes.ParamName) Then
                            p(0) = ltsRes.ParamName
                            p(1) = If(ltsRes.ParamValue, "")
                        End If

                        wcfRet.AddMessage(DataTransferObjects.WCFResults.MessageType.Warnings, ltsRes.ErrKey, p)
                    End If

                    Return wcfRet
                End If

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertOrUpdatetblUserSecurityAccessToken"))
            End Try

            Return Nothing
        End Using
    End Function

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.tblUserSecurityAccessToken)
        'Create New Record
        Return New LTS.tblUserSecurityAccessToken With {.USATControl = d.USATControl _
                                                       , .USATUserSecurityControl = d.USATUserSecurityControl _
                                                       , .USATSSOAControl = d.USATSSOAControl _
                                                       , .USATUserID = d.USATUserID _
                                                       , .USATToken = d.USATToken _
                                                       , .USATExpires = d.USATExpires _
                                                       , .USATModDate = Date.Now _
                                                       , .USATModUser = Parameters.UserName _
                                                       , .USATUpdated = If(d.USATUpdated Is Nothing, New Byte() {}, d.USATUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GettblUserSecurityAccessTokenFiltered(Control:=CType(LinqTable, LTS.tblUserSecurityAccessToken).USATControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim source As LTS.tblUserSecurityAccessToken = TryCast(LinqTable, LTS.tblUserSecurityAccessToken)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.tblUserSecurityAccessTokens
                       Where d.USATControl = source.USATControl
                       Select New DTO.QuickSaveResults With {.Control = d.USATControl _
                                                            , .ModDate = d.USATModDate _
                                                            , .ModUser = d.USATModUser _
                                                            , .Updated = d.USATUpdated.ToArray}).First

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

    Friend Function selectDTOData(ByVal d As LTS.tblUserSecurityAccessToken, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.tblUserSecurityAccessToken
        Return New DTO.tblUserSecurityAccessToken With {.USATControl = d.USATControl _
                                                       , .USATUserSecurityControl = d.USATUserSecurityControl _
                                                       , .USATSSOAControl = d.USATSSOAControl _
                                                       , .USATUserID = d.USATUserID _
                                                       , .USATToken = d.USATToken _
                                                       , .USATExpires = d.USATExpires _
                                                       , .USATModDate = d.USATModDate _
                                                       , .USATModUser = d.USATModUser _
                                                       , .Page = page _
                                                       , .Pages = pagecount _
                                                       , .RecordCount = recordcount _
                                                       , .PageSize = pagesize _
                                                       , .USATUpdated = d.USATUpdated.ToArray()}
    End Function

    Protected Function getSignOnControl(ByVal SSOAAct As Utilities.SSOAAccount, ByVal ClientSecret As String) As Integer
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'Get the Sign on Control
                Dim intRet As Integer
                If (Not String.IsNullOrEmpty(ClientSecret)) AndAlso ClientSecret.Trim.Length > 0 Then
                    intRet = (From d In db.tblSingleSignOnAccounts Where d.SSOAControl = SSOAAct And d.SSOAClientSecret = ClientSecret Select d.SSOAControl).FirstOrDefault()
                Else
                    intRet = (From d In db.tblSingleSignOnAccounts Where d.SSOAControl = SSOAAct Select d.SSOAControl).FirstOrDefault()
                End If
                Return intRet

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GettblSingleSignOnAccountControl"), sysErrorParameters.sysErrorState.SystemLevelFault, sysErrorParameters.sysErrorSeverity.Unexpected)
            End Try
            Return 0

        End Using
    End Function

    Protected Function GettblUserSecurity(ByVal Control As Integer) As DTO.tblUserSecurity

        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'if there are no null values to deal with we can build the array directly
                Dim tblUserSecurity As DTO.tblUserSecurity = (
                From d In db.tblUserSecurities
                Where d.UserSecurityControl = Control
                Select New DTO.tblUserSecurity With {.UserSecurityControl = d.UserSecurityControl,
                                                         .UserName = d.UserName,
                                                         .UserDepartment = d.UserDepartment,
                                                         .UserEmail = d.UserEmail,
                                                         .UserFirstName = d.UserFirstName,
                                                         .UserFriendlyName = d.UserFriendlyName,
                                                         .UserLastName = d.UserLastName,
                                                         .UserMiddleIn = d.UserMiddleIn,
                                                         .UserPhoneCell = d.UserPhoneCell,
                                                         .UserPhoneHome = d.UserPhoneHome,
                                                         .UserPhoneWork = d.UserPhoneWork,
                                                         .UserWorkExt = d.UserWorkExt,
                                                         .UseAuthCode = d.UseAuthCode,
                                                         .UserTitle = d.UserTitle,
                                                         .UserRemotePassword = d.UserRemotePassword,
                                                         .UserUserGroupsControl = d.UserUserGroupsControl,
                                                         .NEXTrackOnly = d.NEXTrackOnly,
                                                         .UserUpdated = d.UserUpdated.ToArray()}).FirstOrDefault()
                Return tblUserSecurity

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GettblUserSecurity"), sysErrorParameters.sysErrorState.SystemLevelFault, sysErrorParameters.sysErrorSeverity.Unexpected)
            End Try

            Return Nothing

        End Using

    End Function

    Protected Function GettblUserSecurityByUserName(ByVal UserName As String) As DTO.tblUserSecurity

        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'if there are no null values to deal with we can build the array directly
                Dim tblUserSecurity As DTO.tblUserSecurity = (
                From d In db.tblUserSecurities
                Where d.UserName = UserName
                Select New DTO.tblUserSecurity With {.UserSecurityControl = d.UserSecurityControl,
                                                         .UserName = d.UserName,
                                                         .UserDepartment = d.UserDepartment,
                                                         .UserEmail = d.UserEmail,
                                                         .UserFirstName = d.UserFirstName,
                                                         .UserFriendlyName = d.UserFriendlyName,
                                                         .UserLastName = d.UserLastName,
                                                         .UserMiddleIn = d.UserMiddleIn,
                                                         .UserPhoneCell = d.UserPhoneCell,
                                                         .UserPhoneHome = d.UserPhoneHome,
                                                         .UserPhoneWork = d.UserPhoneWork,
                                                         .UserWorkExt = d.UserWorkExt,
                                                         .UseAuthCode = d.UseAuthCode,
                                                         .UserTitle = d.UserTitle,
                                                         .UserRemotePassword = d.UserRemotePassword,
                                                         .UserUserGroupsControl = d.UserUserGroupsControl,
                                                         .NEXTrackOnly = d.NEXTrackOnly,
                                                         .UserUpdated = d.UserUpdated.ToArray()}).Single
                Return tblUserSecurity

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GettblUserSecurityByUserName"), sysErrorParameters.sysErrorState.SystemLevelFault, sysErrorParameters.sysErrorSeverity.Unexpected)
            End Try
            Return Nothing

        End Using

    End Function


    ''' <summary>
    ''' Checks the current token based on the WCFParameters data
    ''' updates the WCFParameters data with the username associated with 
    ''' the provided USATToken if it exists and if it has not expired
    ''' UseToken must be true, SSOAName must be valid 
    ''' an optional SSOAClientSecret code may be provided. this code is ignored if empty
    ''' If the token is not valid the method returns false
    ''' </summary>
    ''' <param name="oParameters"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR 12/15/2015 v-7.0.4
    '''   Added new logic to minimize E_NoData errors and to test for blank or empty resutls
    ''' </remarks>
    Friend Function validateUserToken(ByRef oParameters As WCFParameters) As Boolean
        Try
            If oParameters.UseToken = False Then Return False
            'Get the Sign on Control
            Dim SSOAControl = getSignOnControl(Utilities.getSSOAccountEnum(oParameters), oParameters.SSOAClientSecret)
            If SSOAControl < 1 OrElse String.IsNullOrWhiteSpace(oParameters.USATToken) Then
                'we do not have enough information to lookup a token so just return false
                Return False
            End If
            Dim oSecurityTokenData As DTO.tblUserSecurityAccessToken = GettblUserSecurityAccessTokenFilteredByToken(SSOAControl, oParameters.USATToken)
            If oSecurityTokenData Is Nothing OrElse oSecurityTokenData.USATUserSecurityControl < 1 Then
                'no token data exists so return false
                Return False
            End If
            'if we get here we have a token
            oParameters.USATToken = oSecurityTokenData.USATToken
            'Get the user information
            Dim oUserData As DTO.tblUserSecurity = GettblUserSecurity(oSecurityTokenData.USATUserSecurityControl)
            oParameters.UserName = oUserData.UserName
            oParameters.UserRemotePassword = oUserData.UserRemotePassword
            Return True
        Catch ex As FaultException(Of SqlFaultInfo)
            If ex.Detail.Message = "E_NoData" Then Return False
            Throw
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("validateUserToken"), sysErrorParameters.sysErrorState.SystemLevelFault, sysErrorParameters.sysErrorSeverity.Critical)
        End Try
        Return False
    End Function

    ''' <summary>
    ''' Creates or updates the users token fron the desktop client
    ''' new token is generated if the old one is expired
    ''' the caller must validate the user  this method does not validate the users token only the expiration date
    ''' </summary>
    ''' <param name="oParameters"></param>
    ''' <remarks>
    ''' Modified by RHR for v-8.4.0.002 on 03/31/2021
    '''     we now only create a new token when it has expired
    ''' </remarks>
    Friend Sub updateNGLToken(ByRef oParameters As WCFParameters)
        'tokens expire after 30 days there are 3600 seconds in an hours 
        Dim intExpMinutes As Integer = 3600 * 720
        Dim expDate As Date = Date.Now.AddSeconds(intExpMinutes)
        Dim token As String = Guid.NewGuid().ToString
        Try
            'Get the user information
            Dim oUserData As DTO.tblUserSecurity = GettblUserSecurityByUserName(oParameters.UserName)
            If oUserData Is Nothing OrElse oUserData.UserSecurityControl = 0 Then Return 'nothing to do no user
            'Dim intSignOnControl As Integer = getSignOnControl(oParameters.SSOAName, oParameters.SSOAClientSecret)
            Dim intSignOnControl As Integer = getSignOnControl(Utilities.SSOAAccount.NGL, oParameters.SSOAClientSecret)
            'Check if a tblUserSecurityAccessToken record exists
            'Modified By LVV 6/27/17 - Add logic to create NGL security token if one does not exist to support backwards compatibility with NEXTrack
            Dim oRecord = GettblUserSecurityAccessTokenFiltered(oUserData.UserSecurityControl, Utilities.SSOAAccount.NGL)
            If oRecord Is Nothing OrElse oRecord.USATControl = 0 Then

                oRecord = New DTO.tblUserSecurityAccessToken With {.USATSSOAControl = intSignOnControl _
                                                                  , .USATUserSecurityControl = oUserData.UserSecurityControl _
                                                                  , .USATExpires = expDate _
                                                                  , .USATToken = token _
                                                                  , .USATUserID = oParameters.UserName _
                                                                  , .TrackingState = TrackingInfo.Created}
                Me.CreateRecord(oRecord)
            Else
                ' if we have a token aleady we only change it if it is about to expire
                ' we always update the expires on existing tokens 

                With oRecord
                    If .USATExpires < Date.Now() Then
                        'the token has expired we need a new one
                        .USATToken = token
                    Else
                        token = .USATToken 'return the current token
                    End If
                    .USATExpires = expDate
                    .USATUserID = oParameters.UserName
                    .TrackingState = TrackingInfo.Updated
                End With
                Me.UpdateRecordNoReturn(oRecord)
            End If
            With oParameters
                .UseToken = True
                .USATToken = token
                .SSOAName = "NGL"
            End With
        Catch ex As FaultException
            'do nothing we just cannot log the token 
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("updateNGLToken"), sysErrorParameters.sysErrorState.UserLevelFault, sysErrorParameters.sysErrorSeverity.Warning)
        End Try

    End Sub

    Friend Sub loadParameterSettings(ByVal oParameters As WCFParameters)

        'Save the parameters object
        Parameters = oParameters
        Dim strWCFAuthCode As String = readConfigSettings("WCFAuthCode").Trim
        Dim strSQLAuthUser As String = readConfigSettings("SQLAuthUser").Trim
        Dim strSQLAuthPass As String = readConfigSettings("SQLAuthPass").Trim
        'clear the connection string to be sure we use the parameter values provided
        Me.ConnectionString = oParameters.ConnectionString
        'Me.ConnectionString = String.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3};TrustServerCertificate=true;", oParameters.DBServer.Trim, oParameters.Database.Trim, strSQLAuthUser, strSQLAuthPass)
        Dim db As New NGLMASSecurityDataContext(ConnectionString)
        Me.LinqTable = db.tblUserSecurityAccessTokens
        Me.LinqDB = db
        Me.SourceClass = "NGLtblUserSecurityAccessTokenData"
    End Sub

#End Region

End Class

Public Class NGLtblAlertMessageData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        'Dim db As New NGLMASSecurityDataContext(ConnectionString)
        'Me.LinqTable = db.tblAlertMessages
        'Me.LinqDB = db
        Me.SourceClass = "NGLtblAlertMessageData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASSecurityDataContext(ConnectionString)
                _LinqTable = db.tblAlertMessages
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

    ''' <summary>
    ''' Not Supported
    ''' </summary>
    ''' <param name="Control"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return Nothing
    End Function

    ''' <summary>
    ''' Not Supported
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    ''' <summary>
    ''' Inserts an alert message into the tblAlertMessage table
    ''' </summary>
    ''' <param name="AlertName"></param>
    ''' <param name="AlertDescription"></param>
    ''' <param name="Subject"></param>
    ''' <param name="Body"></param>
    ''' <param name="CompControl"></param>
    ''' <param name="CompNumber"></param>
    ''' <param name="CarrierControl"></param>
    ''' <param name="CarrierNumber"></param>
    ''' <param name="Note1"></param>
    ''' <param name="Note2"></param>
    ''' <param name="Note3"></param>
    ''' <param name="Note4"></param>
    ''' <param name="Note5"></param>
    ''' <param name="IgnoreErrors"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.104 on 04/18/2017
    '''   added truncation logic to Subject and Notes 1 through 5 to deal with 255 character limit
    ''' </remarks>
    Public Function InsertAlertMessage(ByVal AlertName As String,
                                       ByVal AlertDescription As String,
                                       ByVal Subject As String,
                                       ByVal Body As String,
                                       Optional ByVal CompControl As Integer = 0,
                                       Optional ByVal CompNumber As Integer = 0,
                                       Optional ByVal CarrierControl As Integer = 0,
                                       Optional ByVal CarrierNumber As Integer = 0,
                                       Optional ByVal Note1 As String = "",
                                       Optional ByVal Note2 As String = "",
                                       Optional ByVal Note3 As String = "",
                                       Optional ByVal Note4 As String = "",
                                       Optional ByVal Note5 As String = "",
                                       Optional ByVal IgnoreErrors As Boolean = True) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim oAlert = db.spGetOrCreateSubscriptionAlert(AlertName, AlertDescription).FirstOrDefault()
                If Not oAlert Is Nothing AndAlso oAlert.ProcedureControl <> 0 Then
                    Dim oAlertMessage As New LTS.tblAlertMessage With {
                        .AlertMessageProcedureControl = oAlert.ProcedureControl,
                        .AlertMessageCompControl = CompControl,
                        .AlertMessageCompNumber = CompNumber,
                        .AlertMessageCarrierControl = CarrierControl, 'Bug fixed by LVV 10/3/18 was incorrectly setting CarrierControl to CarrierNumber
                        .AlertMessageCarrierNumber = CarrierNumber,
                        .AlertMessageName = AlertName,
                        .AlertMessageDescription = oAlert.ProcedureDescription,
                        .AlertMessageSubject = Left(Subject, 254),
                        .AlertMessageBody = Body,
                        .AlertMessageNote1 = Left(Note1, 254),
                        .AlertMessageNote2 = Left(Note2, 254),
                        .AlertMessageNote3 = Left(Note3, 254),
                        .AlertMessageNote4 = Left(Note4, 254),
                        .AlertMessageNote5 = Left(Note5, 254),
                        .AlertMessageModDate = Date.Now(),
                        .AlertMessageModUser = Me.Parameters.UserName}
                    db.tblAlertMessages.InsertOnSubmit(oAlertMessage)
                    db.SubmitChanges()
                    blnRet = True
                End If

            Catch ex As Exception
                If Not IgnoreErrors Then ManageLinqDataExceptions(ex, buildProcedureName("InsertAlertMessage"), db)
            End Try

            Return blnRet

        End Using
    End Function

    ''' <summary>
    ''' Inserts an alert message with the send an email option off
    ''' </summary>
    ''' <param name="AlertName"></param>
    ''' <param name="AlertDescription"></param>
    ''' <param name="Subject"></param>
    ''' <param name="Body"></param>
    ''' <param name="CompControl"></param>
    ''' <param name="CompNumber"></param>
    ''' <param name="CarrierControl"></param>
    ''' <param name="CarrierNumber"></param>
    ''' <param name="Note1"></param>
    ''' <param name="Note2"></param>
    ''' <param name="Note3"></param>
    ''' <param name="Note4"></param>
    ''' <param name="Note5"></param>
    ''' <param name="IgnoreErrors"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.0 on 01/15/2018 to merge changes from   v-7.0.6.105
    ''' </remarks>
    Public Function InsertAlertMessageNoEmail(ByVal AlertName As String,
                                       ByVal AlertDescription As String,
                                       ByVal Subject As String,
                                       ByVal Body As String,
                                       Optional ByVal CompControl As Integer = 0,
                                       Optional ByVal CompNumber As Integer = 0,
                                       Optional ByVal CarrierControl As Integer = 0,
                                       Optional ByVal CarrierNumber As Integer = 0,
                                       Optional ByVal Note1 As String = "",
                                       Optional ByVal Note2 As String = "",
                                       Optional ByVal Note3 As String = "",
                                       Optional ByVal Note4 As String = "",
                                       Optional ByVal Note5 As String = "",
                                       Optional ByVal IgnoreErrors As Boolean = True) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim oAlert = db.spGetOrCreateSubscriptionAlert(AlertName, AlertDescription).FirstOrDefault()
                If Not oAlert Is Nothing AndAlso oAlert.ProcedureControl <> 0 Then
                    Dim oAlertMessage As New LTS.tblAlertMessage With {
                        .AlertMessageProcedureControl = oAlert.ProcedureControl,
                        .AlertMessageCompControl = CompControl,
                        .AlertMessageCompNumber = CompNumber,
                        .AlertMessageCarrierControl = CarrierControl, 'Bug fixed by LVV 10/3/18 was incorrectly setting CarrierControl to CarrierNumber
                        .AlertMessageCarrierNumber = CarrierNumber,
                        .AlertMessageName = AlertName,
                        .AlertMessageDescription = oAlert.ProcedureDescription,
                        .AlertMessageSubject = Left(Subject, 254),
                        .AlertMessageBody = Body,
                        .AlertMessageNote1 = Left(Note1, 254),
                        .AlertMessageNote2 = Left(Note2, 254),
                        .AlertMessageNote3 = Left(Note3, 254),
                        .AlertMessageNote4 = Left(Note4, 254),
                        .AlertMessageNote5 = Left(Note5, 254),
                        .AlertMessageEmailsSent = True,
                        .AlertMessageModDate = Date.Now(),
                        .AlertMessageModUser = Me.Parameters.UserName}
                    db.tblAlertMessages.InsertOnSubmit(oAlertMessage)
                    db.SubmitChanges()
                    blnRet = True
                End If

            Catch ex As Exception
                If Not IgnoreErrors Then ManageLinqDataExceptions(ex, buildProcedureName("InsertAlertMessage"), db)
            End Try

            Return blnRet

        End Using
    End Function

    ''' <summary>
    ''' GetSubcriptionAlertsFiltered
    ''' </summary>
    ''' <param name="blnAllProcedures"></param>
    ''' <param name="procControl"></param>
    ''' <param name="blnOnlyMyAlerts"></param>
    ''' <param name="UserSecControl"></param>
    ''' <param name="StartDate"></param>
    ''' <param name="EndDate"></param>
    ''' <param name="page"></param>
    ''' <param name="pagesize"></param>
    ''' <param name="skip"></param>
    ''' <param name="take"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 12/2/16 for v-7.0.5.110 Subscription Alert Changes
    '''   skip and take parameters are to support 
    '''   interface with TMS 365 pages
    ''' </remarks>
    Public Function GetSubcriptionAlertsFiltered(ByVal blnAllProcedures As Boolean,
                                                 ByVal procControl As Integer,
                                                 ByVal blnOnlyMyAlerts As Boolean,
                                                 ByVal UserSecControl As Integer,
                                                 Optional ByVal StartDate As DateTime? = Nothing,
                                                 Optional ByVal EndDate As DateTime? = Nothing,
                                                 Optional ByVal page As Integer = 1,
                                                 Optional ByVal pagesize As Integer = 1000,
                                                 Optional ByVal skip As Integer = 0,
                                                 Optional ByVal take As Integer = 0) As DTO.tblAlertMessage()
        'show only my alerts vs show all alerts
        'dropdown the show a specific alert type (filtered by above)
        'daterange for moddates
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'db.Log = New DebugTextWriter
                Dim datefilterType As Utilities.NGLDateFilterType = Utilities.NGLDateFilterType.DateModified

                If Not StartDate.HasValue AndAlso Not EndDate.HasValue Then
                    'disable the date filter if the dates are null
                    datefilterType = Utilities.NGLDateFilterType.None
                Else
                    'will this fail if a date is nothing?
                    If Not StartDate Is Nothing Then
                        StartDate = DTran.formatStartDateFilter(StartDate.Value)
                    End If
                    If Not EndDate Is Nothing Then
                        EndDate = DTran.formatEndDateFilter(EndDate.Value)
                    End If
                End If

                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1

                Dim myProcedures() As Integer = (From p In db.tblProcAlertUserXrefs
                                                 Where p.UserSecurityControl = UserSecControl
                                                 Select p.ProcedureControl).ToArray()

                Dim oQuery = From t In db.tblAlertMessages
                             Where
                                (
                                 (blnAllProcedures = False And t.AlertMessageProcedureControl = procControl) _
                                 Or
                                 (blnAllProcedures = True And t.AlertMessageProcedureControl = t.AlertMessageProcedureControl)
                                ) _
                            And
                               (
                                   (blnOnlyMyAlerts = False And t.AlertMessageProcedureControl = t.AlertMessageProcedureControl) _
                                   Or
                                   (blnOnlyMyAlerts = True And myProcedures.Contains(t.AlertMessageProcedureControl))
                               ) _
                           And
                               (
                                   datefilterType = Utilities.NGLDateFilterType.None _
                                   Or
                                   (
                                   (StartDate Is Nothing And t.AlertMessageModDate <= EndDate) _
                                    Or
                                   (EndDate Is Nothing And t.AlertMessageModDate >= StartDate) _
                                   Or
                                   (t.AlertMessageModDate >= StartDate AndAlso t.AlertMessageModDate <= EndDate)
                                   )
                               )
                             Order By t.AlertMessageModDate Descending
                             Select t

                intRecordCount = oQuery.Count()
                If intRecordCount < 1 Then Return Nothing

                If take <> 0 Then
                    pagesize = take
                Else
                    'calculate based on page and pagesize
                    If pagesize < 1 Then pagesize = 1
                    If intRecordCount < 1 Then intRecordCount = 1
                    If page < 1 Then page = 1
                    skip = (page - 1) * pagesize
                End If
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1

                Dim alerts() As DTO.tblAlertMessage = (From d In oQuery
                                                       Order By d.AlertMessageModDate Descending
                                                       Select selectDTOData(d, db, page, intPageCount, intRecordCount, pagesize)
                           ).Skip(skip).Take(pagesize).ToArray()

                Return alerts

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetSubcriptionAlertsFiltered"))
            End Try

            Return Nothing

        End Using
    End Function


#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object

        Return Nothing
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return Nothing
    End Function

    Friend Function selectDTOData(ByVal d As LTS.tblAlertMessage, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.tblAlertMessage
        Return Nothing
    End Function

    'Added By LVV on 12/2/16 for v-7.0.5.110 Subscription Alert Changes
    Friend Shared Function selectDTOData(ByVal d As LTS.tblAlertMessage, ByVal db As NGLMASSecurityDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.tblAlertMessage
        Dim oDTO As New DTO.tblAlertMessage
        'the original Carrier DTO object was not designed to support NULL integers
        'so we need to process those values using skipObjs
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

#End Region

End Class


Public Class NGLcmLocalizeKeyValuePairData : Inherits NGLLinkDataBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        'Dim db As New NGLMASSecurityDataContext(ConnectionString)
        'Me.LinqTable = db.cmLocalizeKeyValuePairs
        'Me.LinqDB = db
        Me.SourceClass = "NGLcmLocalizeKeyValuePairData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASSecurityDataContext(ConnectionString)
                _LinqTable = db.cmLocalizeKeyValuePairs
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

    Public Function GetRecord(control As Integer) As LTS.cmLocalizeKeyValuePair
        Dim oRet As New LTS.cmLocalizeKeyValuePair
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                oRet = db.cmLocalizeKeyValuePairs.Where(Function(x) x.cmLocalControl = control).FirstOrDefault()

            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                ManageLinqDataExceptions(ex, buildProcedureName("GetRecord"))
            End Try

            Return oRet

        End Using
    End Function

    Public Function GetAll() As List(Of LTS.cmLocalizeKeyValuePair)
        Dim oRet As New List(Of LTS.cmLocalizeKeyValuePair)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                oRet = db.cmLocalizeKeyValuePairs.ToList()

            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                ManageLinqDataExceptions(ex, buildProcedureName("GetAll"))
            End Try

            Return oRet

        End Using
    End Function

    Public Function GetPage(ByVal skip As Integer, ByVal take As Integer, ByRef RecordCount As Integer) As List(Of LTS.cmLocalizeKeyValuePair)
        Dim oRet As New List(Of LTS.cmLocalizeKeyValuePair)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try

                RecordCount = 0
                Dim intPageCount As Integer = 1
                Dim page As Integer = 1
                Dim pagesize As Integer = 1000
                'db.Log = New DebugTextWriter
                RecordCount = db.cmLocalizeKeyValuePairs.Count()


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
                oRet = db.cmLocalizeKeyValuePairs.Skip(skip).Take(pagesize).ToList()


            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                ManageLinqDataExceptions(ex, buildProcedureName("GetPage"))
            End Try

            Return oRet

        End Using

    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filterWhere">'(String.Equals(cmLocalKey, "BookLoadPO", StringComparison.OrdinalIgnoreCase)) Or String.Equals(cmLocalKey, "BookProNumber", StringComparison.OrdinalIgnoreCase)) "</param>
    ''' <param name="sortExpression"></param>
    ''' <param name="skip"></param>
    ''' <param name="take"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetFiltered(ByRef RecordCount As Integer,
                                ByVal filterWhere As String,
                                Optional ByVal sortExpression As String = "",
                                Optional ByVal skip As Integer = 0,
                                Optional ByVal take As Integer = 1000) As List(Of LTS.cmLocalizeKeyValuePair)
        Dim oRet As New List(Of LTS.cmLocalizeKeyValuePair)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try

                RecordCount = 0
                Dim intPageCount As Integer = 1
                Dim page As Integer = 1
                Dim pagesize As Integer = 1000
                'db.Log = New DebugTextWriter
                'Dim oQuery = db.cmLocalizeKeyValuePairs.Where(Function(x) sKeys.Contains(x.cmLocalKey))
                Dim oQuery = db.cmLocalizeKeyValuePairs
                If Not String.IsNullOrWhiteSpace(filterWhere) Then
                    '(String.Equals(cmLocalKey, "BookLoadPO", StringComparison.OrdinalIgnoreCase)) Or String.Equals(cmLocalKey, "BookProNumber", StringComparison.OrdinalIgnoreCase)) "
                    oQuery = DLinqUtil.filterWhere(oQuery, filterWhere)
                End If
                RecordCount = oQuery.Count()

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
                If Not String.IsNullOrWhiteSpace(sortExpression) Then
                    oRet = oQuery.OrderBy(sortExpression).Skip(skip).Take(pagesize).ToList()
                Else
                    oRet = oQuery.Skip(skip).Take(pagesize).ToList()
                End If


            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                ManageLinqDataExceptions(ex, buildProcedureName("GetFiltered"))
            End Try

            Return oRet

        End Using


    End Function

    Public Function FindRecordByKey(ByVal sKey As String) As LTS.cmLocalizeKeyValuePair
        Dim oRet As New LTS.cmLocalizeKeyValuePair()
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try

                oRet = db.cmLocalizeKeyValuePairs.Where(Function(x) x.cmLocalKey = sKey).FirstOrDefault()

            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                ManageLinqDataExceptions(ex, buildProcedureName("GetFiltered"))
            End Try

            Return oRet

        End Using


    End Function

    Public Function Save(oRecord As LTS.cmLocalizeKeyValuePair) As LTS.cmLocalizeKeyValuePair

        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim oTable = db.cmLocalizeKeyValuePairs
                oRecord.cmLocalModDate = Date.Now()
                oRecord.cmLocalModUser = Me.Parameters.UserName
                oTable.Attach(oRecord, True)
                db.SubmitChanges()
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                ManageLinqDataExceptions(ex, buildProcedureName("Save"))
            End Try

            Return oRecord

        End Using
    End Function

    Public Function Insert(oRecord As LTS.cmLocalizeKeyValuePair) As LTS.cmLocalizeKeyValuePair
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'Perform some validation
                If String.IsNullOrWhiteSpace(oRecord.cmLocalKey) Then
                    'E_FieldRequired = 'The '{0}' is required and cannot be empty.
                    Dim oFaultDetails As New List(Of String) From {"Localization Key"}
                    throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_InvalidKeyFilterMetaData, SqlFaultInfo.FaultDetailsKey.E_FieldRequired, oFaultDetails, SqlFaultInfo.FaultReasons.E_DataValidationFailure, True)

                End If
                If db.cmLocalizeKeyValuePairs.Any(Function(x) x.cmLocalKey = oRecord.cmLocalKey) Then
                    'E_CannotSaveKeyValuesAlreadyExist = 'Cannot save changes to {0}. A record with one or more of the following key values already exist: Keys {1} Values {2}.
                    Dim oFaultDetails As New List(Of String) From {"Localization Data", "Localization Key", oRecord.cmLocalKey}
                    throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_InvalidKeyFilterMetaData, SqlFaultInfo.FaultDetailsKey.E_CannotSaveKeyValuesAlreadyExist, oFaultDetails, SqlFaultInfo.FaultReasons.E_DataValidationFailure, True)

                End If

                Dim oTable = db.cmLocalizeKeyValuePairs
                oRecord.cmLocalModDate = Date.Now()
                oRecord.cmLocalModUser = Me.Parameters.UserName
                oTable.InsertOnSubmit(oRecord)
                db.SubmitChanges()
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                ManageLinqDataExceptions(ex, buildProcedureName("Insert"))
            End Try

            Return oRecord

        End Using
    End Function

    Public Function Delete(oRecord As LTS.cmLocalizeKeyValuePair) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Dim oTable = db.cmLocalizeKeyValuePairs
            Try
                oTable.Attach(oRecord, True)
                oTable.DeleteOnSubmit(oRecord)
                LinqDB.SubmitChanges()
                blnRet = True
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch conflictEx As ChangeConflictException
                Try
                    Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(LinqDB))
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
        End Using
        Return blnRet
    End Function

    Public Function Delete(Control As Integer) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Dim oTable = db.cmLocalizeKeyValuePairs
            Try
                Dim oRecord As LTS.cmLocalizeKeyValuePair = db.cmLocalizeKeyValuePairs.Where(Function(x) x.cmLocalControl = Control).FirstOrDefault()
                If (oRecord Is Nothing OrElse oRecord.cmLocalControl = 0) Then Return False
                oTable.Attach(oRecord, True)
                oTable.DeleteOnSubmit(oRecord)
                LinqDB.SubmitChanges()
                blnRet = True
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch conflictEx As ChangeConflictException
                Try
                    Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(LinqDB))
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
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' This will be the method I will call for Localized Messages from here on out
    ''' This is not finished and does not actually work - it is just a placeholder so I can
    ''' be consistent and only have to change code in one place
    ''' Currently it just returns English value in cmLocalValue
    ''' </summary>
    ''' <param name="sKey"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV On 8/1/18 For v-8.3 TMS365 Scheduler
    ''' Modified by RHR for v-8.2.0.117 on 8/23/19
    '''     added logic to save the default to the table as english if it does not exist
    '''     this allows us to edit the text in the db to change the messages dynamically
    '''     modified error handler,  we do not want to thow any errors this could break 
    '''     other error handlers and throw the wrong message, just return default string
    ''' TODO: add localized user specific keys to read different languages 
    ''' Modified by RHR for v-8.2.1.004 on 12/26/2019
    '''   added logic to store the results into a new dictionary object in memory,  we also now
    '''   store both the value and value local so we can implement language specific logic in the future.
    '''   once a value has been read in the current session it may be referenced again via the dictionary without goin back to the db
    '''   In the future this method should be modified to check the current user's local language preference
    ''' Modified by RHR for v-8.2.1.005 on 02/20/2020
    '''     fixed dictionary bug cannot add key if it already exists
    ''' </remarks>
    Public Function GetLocalizedValueByKey(ByVal sKey As String, ByVal strDefault As String) As String
        Dim strRet As String = strDefault
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim oCMData As New LTS.cmLocalizeKeyValuePair()
                ' check if the data exist in the dictionary
                If Utilities.dicCMLocalizedByKey.ContainsKey(sKey) Then
                    oCMData = Utilities.dicCMLocalizedByKey(sKey)
                Else
                    oCMData = db.cmLocalizeKeyValuePairs.Where(Function(x) x.cmLocalKey = sKey).FirstOrDefault()
                    If oCMData Is Nothing OrElse oCMData.cmLocalControl = 0 OrElse String.IsNullOrWhiteSpace(oCMData.cmLocalValue) Then
                        'save to db it does not exists
                        oCMData = New LTS.cmLocalizeKeyValuePair() With {.cmLocalKey = sKey, .cmLocalValue = strDefault, .cmLocalValueLocal = "ENU=" & strDefault, .cmLocalModDate = Date.Now, .cmLocalModUser = Me.Parameters.UserName}
                        db.cmLocalizeKeyValuePairs.InsertOnSubmit(oCMData)
                        db.SubmitChanges()
                    End If
                    Try
                        'Modified by RHR for v-8.2.1.005 on 02/20/2020 
                        '  fixed dictionary bug cannot add key if it already exists
                        If Utilities.dicCMLocalizedByKey.ContainsKey(sKey) Then
                            Utilities.dicCMLocalizedByKey(sKey) = oCMData
                        Else
                            Utilities.dicCMLocalizedByKey.Add(sKey, oCMData)
                        End If

                    Catch ex As Exception
                        'do nothing if the add to dictionary fails
                    End Try
                End If
                'TODO: add code here to check the users language preference
                'for now we just return the default in cmLocalValue
                strRet = oCMData.cmLocalValue
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                'ignore any errors just return default
                'ManageLinqDataExceptions(ex, buildProcedureName("GetLocalizedValueByKey"))
            End Try
            Return strRet
        End Using
    End Function


    ''' <summary>
    ''' Returns the Result Procedure localized string stored in the Localized Key Value library
    ''' We now use the dicCMLocalizedByKey dictionary and no longer need seperate dictionaries
    ''' for different results data
    ''' </summary>
    ''' <param name="eValue"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2.1.004 on 12/26/2019
    '''   new simplified version from BLL which reads the data from the localization library
    '''   Replaces previous logic in the BLL (wrapper methods still exists for backward compatibility)
    ''' </remarks>
    Public Function readResultProcedure(ByVal eValue As ResultProcedures) As String

        If eValue = ResultProcedures.None Then Return "Procedure"
        Select Case eValue
            Case ResultProcedures.freightbill
                Return GetLocalizedValueByKey("freightbill", "freight bill")
            Case Else
                Return "Procedure"
        End Select
    End Function


    ''' <summary>
    ''' Returns the ResultTitle localized string stored in the Localized Key Value library
    ''' We now use the dicCMLocalizedByKey dictionary and no longer need seperate dictionaries
    ''' </summary>
    ''' <param name="eValue"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2.1.004 on 12/26/2019
    '''   new simplified version from BLL which reads the data from the localization library
    '''   Replaces previous logic in the BLL (wrapper methods still exists for backward compatibility)
    ''' </remarks>
    Public Function readResultTitle(ByVal eValue As ResultTitles) As String
        If eValue = ResultTitles.None Then Return ""

        Select Case eValue
            Case ResultTitles.TitleSaveHistLogFailure
                Return GetLocalizedValueByKey("TitleSaveHistLogFailure", "The system was unable To save historical logs")
            Case ResultTitles.TitleSaveExpectedCost
                Return GetLocalizedValueByKey("TitleSaveExpectedCost", "The system was unable To save expected costs")
            Case ResultTitles.TitlePendingFeeApprovalWarning
                Return GetLocalizedValueByKey("TitlePendingFeeApprovalWarning", "Pending Fee Approval Warning")
            Case ResultTitles.TitlePendingFeeApprovalError
                Return GetLocalizedValueByKey("TitlePendingFeeApprovalError", "Pending Fee Approval Error")
            Case ResultTitles.TitleAuditFreightBillWarning
                Return GetLocalizedValueByKey("TitleAuditFreightBillWarning", "Audit Freight Bill Warning")
            Case Else
                Return ""
        End Select

    End Function

    ''' <summary>
    ''' Returns the Result Prefix localized string stored in the Localized Key Value library
    ''' We now use the dicCMLocalizedByKey dictionary and no longer need seperate dictionaries
    ''' for different results data
    ''' </summary>
    ''' <param name="eValue"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2.1.004 on 12/26/2019
    '''   new simplified version from BLL which reads the data from the localization library
    '''   Replaces previous logic in the BLL (wrapper methods still exists for backward compatibility)
    ''' </remarks>
    Public Function readResultPrefix(ByVal eValue As ResultPrefix) As String
        If eValue = ResultPrefix.None Then Return ""
        Select Case eValue
            Case ResultPrefix.MsgDetails
                Return GetLocalizedValueByKey("MsgDetails", "Details: ")
            Case ResultPrefix.MsgCostComparisonNotAvailable
                Return GetLocalizedValueByKey("MsgCostComparisonNotAvailable", " Cost comparison may not be available for ")
            Case ResultPrefix.MsgUnexpectedFeeValidationIssue
                Return GetLocalizedValueByKey("MsgUnexpectedFeeValidationIssue", " Unexpeced fee validation issue for ")
            Case ResultPrefix.MsgRecalculateCostForFeeFailed
                Return GetLocalizedValueByKey("MsgRecalculateCostForFeeFailed", " Recalculate Costs Failed for ")
            Case Else
                Return ""
        End Select
    End Function

    ''' <summary>
    ''' Returns the Result Suffix localized string stored in the Localized Key Value library
    ''' We now use the dicCMLocalizedByKey dictionary and no longer need seperate dictionaries
    ''' for different results data
    ''' </summary>
    ''' <param name="eValue"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2.1.004 on 12/26/2019
    '''   new simplified version from BLL which reads the data from the localization library
    '''   Replaces previous logic in the BLL (wrapper methods still exists for backward compatibility)
    ''' </remarks>
    Public Function readResultSuffix(ByVal eValue As ResultSuffix) As String
        If eValue = ResultSuffix.None Then Return ""
        Select Case eValue
            Case ResultSuffix.MsgDoesNotEffectProcess
                Return GetLocalizedValueByKey("MsgDoesNotEffectProcess", " This does Not affect the systems ability To process the")
            Case ResultSuffix.MsgCheckAppErrorLogs
                Return GetLocalizedValueByKey("MsgCheckAppErrorLogs", " Check the application error logs for more details.")
            Case ResultSuffix.MsgUpdatedTotalCostManually
                Return GetLocalizedValueByKey("MsgUpdatedTotalCostManually", " Total costs should be updated manually.")
            Case Else
                Return ""
        End Select

    End Function

    Public Function DoesKeyExist(ByVal sKey As String) As Boolean
        Dim oRet As Boolean = False
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                oRet = db.cmLocalizeKeyValuePairs.Any(Function(x) x.cmLocalKey = sKey)
            Catch ex As Exception
                SaveAppError(ex.Message, Me.Parameters)
                ManageLinqDataExceptions(ex, buildProcedureName("DoesKeyExist"))
            End Try
            Return oRet
        End Using
    End Function

#End Region

#Region "Protected Functions"

#End Region

End Class

''' <summary>
''' Base class for Page content management
''' </summary>
''' <remarks>
''' Created by RHR for v-8.0 on 02/15/2017
'''   starting point for UI Content Management of Pages
''' </remarks>
Public Class NGLcmPageData : Inherits NGLLinkDataBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        'Dim db As New NGLMASSecurityDataContext(ConnectionString)
        'Me.LinqTable = db.cmPages
        'Me.LinqDB = db
        Me.SourceClass = "NGLcmPageData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASSecurityDataContext(ConnectionString)
                _LinqTable = db.cmPages
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

    Public Function GetRecord(control As Integer) As LTS.cmPage
        Dim oRet As New LTS.cmPage
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                oRet = db.cmPages.Where(Function(x) x.PageControl = control).FirstOrDefault()

            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                ManageLinqDataExceptions(ex, buildProcedureName("GetRecord"))
            End Try

            Return oRet

        End Using
    End Function

    Public Function GetAll() As List(Of LTS.cmPage)
        Dim oRet As New List(Of LTS.cmPage)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                oRet = db.cmPages.ToList()

            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                ManageLinqDataExceptions(ex, buildProcedureName("GetAll"))
            End Try

            Return oRet

        End Using
    End Function

    Public Function GetPage(ByVal skip As Integer, ByVal take As Integer, ByRef RecordCount As Integer) As List(Of LTS.cmPage)
        Dim oRet As New List(Of LTS.cmPage)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try

                RecordCount = 0
                Dim intPageCount As Integer = 1
                Dim page As Integer = 1
                Dim pagesize As Integer = 1000
                'db.Log = New DebugTextWriter
                RecordCount = db.cmPages.Count()


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
                oRet = db.cmPages.Skip(skip).Take(pagesize).ToList()


            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                ManageLinqDataExceptions(ex, buildProcedureName("GetPage"))
            End Try

            Return oRet

        End Using

    End Function


    ''' <summary>
    ''' Returns the user's Legal Entity Logo and Logo link if available or false if not available
    ''' </summary>
    ''' <param name="PageControl"></param>
    ''' <param name="iUserControl"></param>
    ''' <param name="sHomeTabLogo"></param>
    ''' <param name="sHomeTabHrefURL"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.4.0.002 on 05/14/2021
    '''     Note: PageControl is not currntly used but in the future this may play an important role
    ''' </remarks>
    Public Function ReadUserPageLogo(ByVal PageControl As Integer, ByVal iUserControl As Integer, ByRef sHomeTabLogo As String, ByRef sHomeTabHrefURL As String) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim oUserLEData = db.tblUserSecurityLegalEntities.Where(Function(x) x.USLEUserSecurityControl = iUserControl).FirstOrDefault()
                If Not oUserLEData Is Nothing Then
                    Dim oComp = db.CompRefSecurities.Where(Function(x) x.CompControl = oUserLEData.USLECompControl).FirstOrDefault()
                    If Not oComp Is Nothing Then
                        sHomeTabHrefURL = oComp.CompHeaderLogoLink
                        sHomeTabLogo = oComp.CompHeaderLogo
                        If Not String.IsNullOrWhiteSpace(sHomeTabHrefURL) AndAlso Not String.IsNullOrWhiteSpace(sHomeTabLogo) Then
                            blnRet = True
                        End If
                    End If
                End If
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                ManageLinqDataExceptions(ex, buildProcedureName("ReadUserPageLogo"))
            End Try

            Return blnRet

        End Using

    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filterWhere">'(String.Equals(cmLocalKey, "BookLoadPO", StringComparison.OrdinalIgnoreCase)) Or String.Equals(cmLocalKey, "BookProNumber", StringComparison.OrdinalIgnoreCase)) "</param>
    ''' <param name="sortExpression"></param>
    ''' <param name="skip"></param>
    ''' <param name="take"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetFiltered(ByRef RecordCount As Integer,
                                ByVal filterWhere As String,
                                Optional ByVal sortExpression As String = "",
                                Optional ByVal skip As Integer = 0,
                                Optional ByVal take As Integer = 1000) As List(Of LTS.cmPage)
        Dim oRet As New List(Of LTS.cmPage)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try

                RecordCount = 0
                Dim intPageCount As Integer = 1
                Dim page As Integer = 1
                Dim pagesize As Integer = 1000
                'db.Log = New DebugTextWriter
                'Dim oQuery = db.cmPages.Where(Function(x) sKeys.Contains(x.cmLocalKey))
                Dim oQuery = db.cmPages
                If Not String.IsNullOrWhiteSpace(filterWhere) Then
                    '(String.Equals(cmLocalKey, "BookLoadPO", StringComparison.OrdinalIgnoreCase)) Or String.Equals(cmLocalKey, "BookProNumber", StringComparison.OrdinalIgnoreCase)) "
                    oQuery = DLinqUtil.filterWhere(oQuery, filterWhere)
                End If
                RecordCount = oQuery.Count()

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
                If Not String.IsNullOrWhiteSpace(sortExpression) Then
                    oRet = oQuery.OrderBy(sortExpression).Skip(skip).Take(pagesize).ToList()
                Else
                    oRet = oQuery.Skip(skip).Take(pagesize).ToList()
                End If


            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                ManageLinqDataExceptions(ex, buildProcedureName("GetFiltered"))
            End Try

            Return oRet

        End Using


    End Function

    ''' <summary>
    ''' Returns a list of all pages in the Page table
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns></returns>
    ''' <remarks>Added By LVV On 8/19/20 For v-8.3.0.001 - Task#20200817144456 - Modify the License File Integration Command Line Utility</remarks>
    Public Function GetPages(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.cmPage()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.cmPage
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'Get the data iqueryable
                Dim iQuery As IQueryable(Of LTS.cmPage)
                iQuery = db.cmPages
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetPages"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Saves the PageFooterMsg for the page
    ''' </summary>
    ''' <param name="PageControl"></param>
    ''' <param name="PgFooterMsg"></param>
    ''' <returns></returns>
    ''' <remarks>Added By LVV On 8/19/20 For v-8.3.0.001 - Task#20200817144456 - Modify the License File Integration Command Line Utility</remarks>
    Public Function SavePgFooterMsg(ByVal PageControl As Integer, ByVal PgFooterMsg As String) As Boolean
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Dim blnRet As Boolean = False
            Try
                Dim ltsPg = db.cmPages.Where(Function(x) x.PageControl = PageControl).FirstOrDefault()
                ltsPg.PageFooterMsg = PgFooterMsg
                ltsPg.PageModDate = Date.Now()
                ltsPg.PageModUser = Me.Parameters.UserName
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                SaveAppError(ex.Message, Me.Parameters)
                ManageLinqDataExceptions(ex, buildProcedureName("SavePgFooterMsg"), db)
            End Try
            Return blnRet
        End Using
    End Function


    Public Function Save(oRecord As LTS.cmPage) As LTS.cmPage

        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim oTable = db.cmPages
                oRecord.PageModDate = Date.Now()
                oRecord.PageModUser = Me.Parameters.UserName
                oTable.Attach(oRecord, True)
                db.SubmitChanges()
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                ManageLinqDataExceptions(ex, buildProcedureName("Save"))
            End Try

            Return oRecord

        End Using
    End Function

    Public Function Insert(oRecord As LTS.cmPage) As LTS.cmPage
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'Perform some validation
                If String.IsNullOrWhiteSpace(oRecord.PageName) Then
                    'E_FieldRequired = 'The '{0}' is required and cannot be empty.
                    Dim oFaultDetails As New List(Of String) From {"Page Name"}
                    throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_InvalidKeyFilterMetaData, SqlFaultInfo.FaultDetailsKey.E_FieldRequired, oFaultDetails, SqlFaultInfo.FaultReasons.E_DataValidationFailure, True)

                End If
                'If db.cmPages.Any(Function(x) x.cmLocalKey = oRecord.cmLocalKey) Then
                '    'E_CannotSaveKeyValuesAlreadyExist = 'Cannot save changes to {0}. A record with one or more of the following key values already exist: Keys {1} Values {2}.
                '    Dim oFaultDetails As New List(Of String) From {"Localization Data", "Localization Key", oRecord.cmLocalKey}
                '    throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_InvalidKeyFilterMetaData, SqlFaultInfo.FaultDetailsKey.E_CannotSaveKeyValuesAlreadyExist, oFaultDetails, SqlFaultInfo.FaultReasons.E_DataValidationFailure, True)

                'End If

                Dim oTable = db.cmPages
                oRecord.PageModDate = Date.Now()
                oRecord.PageModUser = Me.Parameters.UserName
                oTable.InsertOnSubmit(oRecord)
                db.SubmitChanges()
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                ManageLinqDataExceptions(ex, buildProcedureName("Insert"))
            End Try

            Return oRecord

        End Using
    End Function

    Public Function Delete(oRecord As LTS.cmPage) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Dim oTable = db.cmPages
            Try
                oTable.Attach(oRecord, True)
                oTable.DeleteOnSubmit(oRecord)
                LinqDB.SubmitChanges()
                blnRet = True
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch conflictEx As ChangeConflictException
                Try
                    Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(LinqDB))
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
        End Using
        Return blnRet
    End Function


    ''' <summary>
    ''' Deletes the Page Detail Record provided
    ''' </summary>
    ''' <param name="oRecord"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.0 on 11/06/2017
    ''' Modified by LVV on 12/12/17 for v-8.0 TMS 365
    '''  Added rule that if the PageDetControl is being 
    '''  referenced by another record as a ParentID
    '''  it cannot be deleted and an exception is thrown
    ''' </remarks>
    Public Function DeleteDetail(oRecord As LTS.cmPageDetail) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Dim oTable = db.cmPageDetails
            Try
                'Delete is not allowed if a child record references the parent ID. 
                If db.cmPageDetails.Any(Function(x) x.PageDetParentID = oRecord.PageDetControl) Then
                    'Cannot delete data the {0} value {1} is being used and cannot be modified.
                    'throwCannotDeleteRecordInUseException("ParentID", oRecord.PageDetControl)
                    throwCannotDeleteRecordInUseException("Selected Page Item", oRecord.PageDetName)
                End If

                oTable.Attach(oRecord, True)
                oTable.DeleteOnSubmit(oRecord)
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteDetail"))
            End Try
        End Using
        Return blnRet
    End Function

    Public Function Delete(Control As Integer) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Dim oTable = db.cmPages
            Try
                Dim oRecord As LTS.cmPage = db.cmPages.Where(Function(x) x.PageControl = Control).FirstOrDefault()
                If (oRecord Is Nothing OrElse oRecord.PageControl = 0) Then Return False
                oTable.Attach(oRecord, True)
                oTable.DeleteOnSubmit(oRecord)
                LinqDB.SubmitChanges()
                blnRet = True
            Catch ex As SqlException

                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch conflictEx As ChangeConflictException
                Try
                    Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(LinqDB))
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
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' create a new DataElement and update the field lists.  The DataElementName must exist in the database and must be a table or a view.
    ''' </summary>
    ''' <param name="DataElementName"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.0 on 09/21/2017
    ''' </remarks>
    Public Function createDataElement(ByVal DataElementName As String) As List(Of LTS.cmElementField)
        Dim oRet As New List(Of LTS.cmElementField)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                db.spCMCreateDataElement(DataElementName)
                Dim iElmtControl As Integer = (From x In db.cmDataElements Where x.DataElmtName = DataElementName Select x.DataElmtControl).FirstOrDefault()
                If iElmtControl <> 0 Then
                    db.spCMUpdateDataElements(DataElementName)
                    If db.cmElementFields.Any(Function(x) x.ElmtFieldDataElmtControl = iElmtControl) Then
                        oRet = (From t In db.cmElementFields Where t.ElmtFieldDataElmtControl = iElmtControl Select t).ToList()
                    End If
                End If

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("createDataElement"), db)
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' create a new DataElement and update the field lists.  The DataElementName must exist in the database and must be a table or a view or the function returns false
    ''' </summary>
    ''' <param name="DataElementName"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.0 on 09/28/2017
    ''' </remarks>
    Public Function createDataElementNoReturn(ByVal DataElementName As String) As Boolean
        Dim blnRet As Boolean = True
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                db.spCMCreateDataElement(DataElementName)
                Dim iElmtControl As Integer = (From x In db.cmDataElements Where x.DataElmtName = DataElementName Select x.DataElmtControl).FirstOrDefault()
                If iElmtControl <> 0 Then
                    db.spCMUpdateDataElements(DataElementName)
                Else
                    blnRet = False
                End If

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("createDataElementNoReturn"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' update the field lists for a dataelement.  The DataElementName must exist in the database and must be a table or a view.
    ''' </summary>
    ''' <param name="DataElementName"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.0 on 09/21/2017
    ''' </remarks>
    Public Function updateDataElementFields(ByVal DataElementName As String) As List(Of LTS.cmElementField)
        Dim oRet As New List(Of LTS.cmElementField)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim iElmtControl As Integer = (From x In db.cmDataElements Where x.DataElmtName = DataElementName Select x.DataElmtControl).FirstOrDefault()
                If iElmtControl <> 0 Then
                    db.spCMUpdateDataElements(DataElementName)
                    If db.cmElementFields.Any(Function(x) x.ElmtFieldDataElmtControl = iElmtControl) Then
                        oRet = (From t In db.cmElementFields Where t.ElmtFieldDataElmtControl = iElmtControl Select t).ToList()
                    End If
                End If

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("updateDataElementFields"), db)
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' update the field lists for a dataelement.  The DataElementName must exist in the database and must be a table or a view or the function returns false
    ''' </summary>
    ''' <param name="DataElementName"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.0 on 09/21/2017
    ''' </remarks>
    Public Function updateDataElementFieldsNoReturn(ByVal DataElementName As String) As Boolean
        Dim blnRet As Boolean = True
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim iElmtControl As Integer = (From x In db.cmDataElements Where x.DataElmtName = DataElementName Select x.DataElmtControl).FirstOrDefault()
                If iElmtControl <> 0 Then
                    db.spCMUpdateDataElements(DataElementName)
                Else
                    blnRet = False
                End If

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("updateDataElementFieldsNoReturn"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Gets a list of Data element fields by DataElementName.  The DataElementName must exist in the database and must be a table or a view.
    ''' </summary>
    ''' <param name="DataElementName"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.0 on 09/21/2017
    ''' </remarks>
    Public Function getDataElementFields(ByVal DataElementName As String) As List(Of LTS.cmElementField)
        Dim oRet As New List(Of LTS.cmElementField)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim iElmtControl As Integer = (From x In db.cmDataElements Where x.DataElmtName = DataElementName Select x.DataElmtControl).FirstOrDefault()
                If iElmtControl <> 0 Then
                    If db.cmElementFields.Any(Function(x) x.ElmtFieldDataElmtControl = iElmtControl) Then
                        oRet = (From t In db.cmElementFields Where t.ElmtFieldDataElmtControl = iElmtControl Select t).ToList()
                    End If
                End If

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("getDataElementFields"), db)
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' Gets a list of Data element fields by DataElmtControl.  The DataElementName referenced by the DataElmtControl must exist in the database and must be a table or a view.
    ''' </summary>
    ''' <param name="DataElmtControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.0 on 09/21/2017
    ''' </remarks>
    Public Function getDataElementFields(ByVal DataElmtControl As Integer) As List(Of LTS.cmElementField)
        Dim oRet As New List(Of LTS.cmElementField)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try

                If DataElmtControl <> 0 Then
                    If db.cmElementFields.Any(Function(x) x.ElmtFieldDataElmtControl = DataElmtControl) Then
                        oRet = (From t In db.cmElementFields Where t.ElmtFieldDataElmtControl = DataElmtControl Select t).ToList()
                    End If
                End If

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("getDataElementFields"), db)
            End Try
        End Using
        Return oRet
    End Function


    Public Function createPage(ByVal PageControl As Integer,
                                ByVal PageName As String,
                                ByVal PageDesc As String,
                                ByVal PageCaption As String,
                                ByVal PageCaptionLocal As String,
                                Optional ByVal PageDataSource As Boolean = False,
                                Optional ByVal PageSortable As Boolean = False,
                                Optional ByVal PagePageable As Boolean = False,
                                Optional ByVal PageGroupable As Boolean = False,
                                Optional ByVal PageEditable As Boolean = False,
                                Optional ByVal PageDataElmtControl As Integer = 0,
                                Optional ByVal PageElmtFieldControl As Integer = 0,
                                Optional ByVal PageAutoRefreshSec As Integer = 0,
                                Optional ByVal FormName As String = "",
                                Optional ByVal FormDescription As String = "",
                                Optional ByVal FormControl As Integer = 0) As LTS.cmPage
        Dim oRet As New LTS.cmPage()
        Dim intPageControl As Integer = 0
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim oPageResutls = db.spCMCreatePage(FormName, PageName, PageDesc, PageCaption, PageCaptionLocal, PageControl, PageDataSource, PageSortable, PagePageable, PageGroupable, PageEditable, PageDataElmtControl, PageElmtFieldControl, PageAutoRefreshSec, FormDescription, FormControl).FirstOrDefault()
                If Not oPageResutls Is Nothing AndAlso oPageResutls.PageControl.HasValue Then
                    intPageControl = oPageResutls.PageControl
                End If
                If intPageControl > 0 Then
                    oRet = db.cmPages.Where(Function(x) x.PageControl = intPageControl).FirstOrDefault()
                End If

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("createPage"), db)
            End Try
        End Using
        If intPageControl < 1 Then
            throwSQLFaultException("Cannot Save Page Information,  Please check the data and try again.")
        End If
        Return oRet
    End Function

    ''' <summary>
    ''' returns a single page detail record
    ''' </summary>
    ''' <param name="PageDetControl"></param>
    ''' <returns></returns>
    Public Function getPageDetailRecord(ByVal PageDetControl As Integer) As LTS.cmPageDetail
        Dim oRet As New LTS.cmPageDetail
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                oRet = db.cmPageDetails.Where(Function(x) x.PageDetControl = PageDetControl).FirstOrDefault()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("getPageDetailRecord"), db)
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' returns all the page details by page sorted by cmpagedetparent and sequence for user zero use NGLSecurityDataProvider.getPageDetailElements to read user settings
    ''' </summary>
    ''' <param name="PageControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR v-8.0 on 7/26/2017
    ''' Modified by RHR for v-8.2 on 09/4/2018
    '''   We now filter the results by user control 0 because we only want to 
    '''   Read the default configuration settings.  User specific settings are not 
    '''   retrieved from this location.
    ''' </remarks>
    Public Function getPageDetailRecords(ByVal PageControl As Integer) As LTS.cmPageDetail()
        Dim oRet() As LTS.cmPageDetail
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'Modified by RHR for v-8.2 on 09/4/2018 added x.PageDetUserSecurityControl = 0
                oRet = db.cmPageDetails.Where(Function(x) x.PageDetPageControl = PageControl And x.PageDetUserSecurityControl = 0).OrderBy(Function(x) x.PageDetParentID).ThenBy(Function(x) x.PageDetSequenceNo).ToArray()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("getPageDetailRecords"), db)
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' updates page details if exists or inserts a new page detail if the PageDetControl is zero
    ''' Throws invalid key value exception if the any of the required key values are missing.
    ''' </summary>
    ''' <param name="pgDetail"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.0 on 8/7/2017
    ''' May be improved with a stored procedure but this method
    ''' is not called often.
    ''' Modified by RHR for v-8.2 on 09/04/2018
    '''   added logic to call spCopycmPageDetailsToAllUsers 
    ''' </remarks>
    Public Function savePageDetailRecord(ByVal pgDetail As LTS.cmPageDetail) As LTS.cmPageDetail
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Dim intUserSecurityControl As Integer = 0
            'validate the data
            Dim pgDetailPageControl As Integer = pgDetail.PageDetPageControl
            If pgDetailPageControl < 1 OrElse Not (db.cmPages.Any(Function(x) x.PageControl = pgDetailPageControl)) Then
                Dim sDetails As New List(Of String)
                sDetails.Add("selected page")
                sDetails.Add(pgDetail.PageDetPageControl & ",cannot be found. A valid page ")
                throwInvalidKeyParentRequiredException(sDetails, False)
            End If
            'check other key values
            Dim sVMsg As String = ""
            Dim blnValidRecord As Boolean = True
            Dim sSpacer As String = ""
            If pgDetail.PageDetGroupTypeControl <= 0 Then
                sVMsg = String.Concat(sVMsg, sSpacer, "Detail Element Group Type")
                sSpacer = ", "
                blnValidRecord = False
            End If
            If pgDetail.PageDetGroupSubTypeControl <= 0 Then
                sVMsg = String.Concat(sVMsg, sSpacer, "Detail Element Type")
                sSpacer = ", "
                blnValidRecord = False
            End If
            If String.IsNullOrWhiteSpace(pgDetail.PageDetName) Then
                sVMsg = String.Concat(sVMsg, sSpacer, "Detail Name")
                sSpacer = ", "
                blnValidRecord = False
            End If
            If Not blnValidRecord Then
                throwInvalidRequiredKeysException("Page Details", sVMsg, False)
            End If
            If pgDetail.PageDetFilterTypeControl < 1 Then pgDetail.PageDetFilterTypeControl = 1

            Try
                pgDetail.PageDetCaption = DTran.replaceQuotes(pgDetail.PageDetCaption)
                pgDetail.PageDetCaptionLocal = DTran.replaceQuotes(pgDetail.PageDetCaptionLocal)
                pgDetail.PageDetModDate = Date.Now()
                pgDetail.PageDetModUser = Me.Parameters.UserName
                If pgDetail.PageDetControl = 0 Then
                    'this is an insert
                    'get the user security control number for the Tag ID
                    If db.tblUserSecurities.Any(Function(x) x.UserName = Parameters.UserName) Then
                        intUserSecurityControl = (From u In db.tblUserSecurities Where u.UserName = Parameters.UserName Select u.UserSecurityControl).FirstOrDefault()
                    End If
                    'build the tag id
                    pgDetail.PageDetTagIDReference = String.Format("id{0}{1}{2}", pgDetail.PageDetPageControl, intUserSecurityControl, Date.Now().ToString("yyyyMMddHHmmssfffffff"))

                    db.cmPageDetails.InsertOnSubmit(pgDetail)
                Else
                    db.cmPageDetails.Attach(pgDetail, True)
                End If
                db.SubmitChanges()
                Try
                    Dim pageDetControl As Integer = pgDetail.PageDetControl
                    If pageDetControl <> 0 Then db.spCopycmPageDetailsToAllUsers(pageDetControl)
                Catch ex As Exception
                    Utilities.SaveAppError("Cannot update user specific cmPageDetail records. Error: " & ex.Message, Me.Parameters)
                End Try
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("savePageDetailRecord"), db)
            End Try
        End Using
        Return pgDetail
    End Function

    Public Function addPageDetailPhoneTemplate(ByVal iPageControl As Integer, ByVal sPageDetName As String) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                If (db.cmPageDetails.Any(Function(x) x.PageDetPageControl = iPageControl And x.PageDetName = sPageDetName) = False) Then Return False
                Dim opds = db.cmPageDetails.Where(Function(x) x.PageDetPageControl = iPageControl And x.PageDetName = sPageDetName).ToArray()
                If opds Is Nothing OrElse opds.Count() < 1 Then Return False
                For Each opd In opds
                    If opd.PageDetControl <> 0 Then
                        opd.PageDetFieldFormatOverride = Chr(34) & "(999) 000-0000" & Chr(34)
                        opd.PageDetFieldTemplateOverride = Chr(34) & "function(dataItem) {  return formatPhoneNumber(dataItem." & opd.PageDetName & ");  }" & Chr(34)
                    End If
                Next
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("addPageDetailPhoneTemplate"), db)
            End Try
        End Using
        Return blnRet
    End Function

    Public Function savePageDetUserVisibility(ByVal PageDetPageControl As Integer, ByVal PageDetControl As Integer, ByVal PageDetVisible As Boolean) As Boolean
        Dim blnRet As Boolean = False
        Dim intUserSecurityControl As Integer = Me.Parameters.UserControl
        If intUserSecurityControl = 0 Then Return False
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim oUserDet As LTS.cmPageDetail = New LTS.cmPageDetail()
                Dim oPageDets() As LTS.cmPageDetail
                Dim newDets As New List(Of LTS.cmPageDetail)

                If (db.cmPageDetails.Any(Function(x) x.PageDetControl = PageDetControl And x.PageDetPageControl = PageDetPageControl And x.PageDetUserSecurityControl = intUserSecurityControl)) Then
                    oUserDet = db.cmPageDetails.Where(Function(x) x.PageDetControl = PageDetControl And x.PageDetPageControl = PageDetPageControl And x.PageDetUserSecurityControl = intUserSecurityControl).FirstOrDefault()
                    With oUserDet
                        .PageDetVisible = PageDetVisible
                        .PageDetModDate = Date.Now
                        .PageDetModUser = Me.Parameters.UserName
                    End With
                Else

                    If (Not db.cmPageDetails.Any(Function(x) x.PageDetControl = PageDetControl And x.PageDetPageControl = PageDetPageControl And x.PageDetUserSecurityControl = 0)) Then
                        Return False
                    End If
                    'lookup the name we may be editing the original source data (updates to control numbers only happen when the screen is refreshed)
                    Dim oPageDetName As String = db.cmPageDetails.Where(Function(x) x.PageDetControl = PageDetControl And x.PageDetPageControl = PageDetPageControl And x.PageDetUserSecurityControl = 0).Select(Function(x) x.PageDetName).FirstOrDefault()
                    If (db.cmPageDetails.Any(Function(x) x.PageDetName = oPageDetName And x.PageDetPageControl = PageDetPageControl And x.PageDetUserSecurityControl = intUserSecurityControl)) Then
                        oUserDet = db.cmPageDetails.Where(Function(x) x.PageDetName = oPageDetName And x.PageDetPageControl = PageDetPageControl And x.PageDetUserSecurityControl = intUserSecurityControl).FirstOrDefault()
                        With oUserDet
                            .PageDetVisible = PageDetVisible
                            .PageDetModDate = Date.Now
                            .PageDetModUser = Me.Parameters.UserName
                        End With
                    Else
                        'create a copy of all page details for this user
                        oPageDets = db.cmPageDetails.Where(Function(x) x.PageDetPageControl = PageDetPageControl And x.PageDetUserSecurityControl = 0).ToArray()
                        If (oPageDets Is Nothing OrElse oPageDets.Count() < 1) Then Return False
                        Dim skipObjs As New List(Of String) From {"PageDetControl", "PageDetUserSecurityControl", "PageDetUpdated", "PageDetModDate", "PageDetModUser", "cmGroupSubType", "cmGroupType", "cmPage"}

                        For Each det In oPageDets.OrderBy(Function(x) x.PageDetParentID)
                            Dim strMSG As String = ""
                            Dim newDet = New LTS.cmPageDetail()
                            newDet = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(newDet, det, skipObjs, strMSG)
                            With newDet
                                If det.PageDetControl = PageDetControl Then
                                    .PageDetVisible = PageDetVisible
                                End If
                                .PageDetModDate = Date.Now
                                .PageDetModUser = Me.Parameters.UserName
                                .PageDetUserSecurityControl = Me.Parameters.UserControl
                            End With
                            If Debugger.IsAttached AndAlso Not String.IsNullOrWhiteSpace(strMSG) Then
                                System.Diagnostics.Debug.WriteLine(strMSG)
                            End If
                            newDets.Add(newDet)
                            db.cmPageDetails.InsertOnSubmit(newDet)
                        Next
                    End If
                End If
                db.SubmitChanges()
                If (Not newDets Is Nothing AndAlso newDets.Count() > 0) AndAlso (Not oPageDets Is Nothing AndAlso oPageDets.Count() > 0) Then
                    'We need to look up the parent IDs
                    Dim dicParentIDXref As New Dictionary(Of Integer, Integer)
                    For Each nDet In newDets
                        If nDet.PageDetParentID > 0 Then
                            If Not dicParentIDXref.ContainsKey(nDet.PageDetParentID) Then
                                'lookup the PageDetParentID
                                Dim sParentName = oPageDets.Where(Function(x) x.PageDetControl = nDet.PageDetParentID).Select(Function(x) x.PageDetName).FirstOrDefault()
                                Dim intNewParentID = newDets.Where(Function(x) x.PageDetName = sParentName).Select(Function(x) x.PageDetControl).FirstOrDefault()
                                dicParentIDXref.Add(nDet.PageDetParentID, intNewParentID)
                            End If
                            nDet.PageDetParentID = dicParentIDXref(nDet.PageDetParentID)
                        End If
                    Next
                    db.SubmitChanges()
                End If
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("PageDetUserVisibility"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="PageDetPageControl"></param>
    ''' <param name="PageDetControl"></param>
    ''' <param name="PageDetWidth"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.4.007 on 06/07/2023
    '''  original loop to create new copy for user could time out
    '''  we now use a stored procedure to create the copy 
    ''' </remarks>
    Public Function savePageDetUserColumnWidth(ByVal PageDetPageControl As Integer, ByVal PageDetControl As Integer, ByVal PageDetWidth As Integer) As Boolean
        Dim blnRet As Boolean = False
        Dim intUserSecurityControl As Integer = Me.Parameters.UserControl
        If intUserSecurityControl = 0 Then Return False
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim oUserDet As LTS.cmPageDetail = New LTS.cmPageDetail()
                Dim oPageDets() As LTS.cmPageDetail
                Dim newDets As New List(Of LTS.cmPageDetail)
                ' Modified we now update using a stored procedure if it does not exist
                If (Not db.cmPageDetails.Any(Function(x) x.PageDetControl = PageDetControl And x.PageDetPageControl = PageDetPageControl And x.PageDetUserSecurityControl = intUserSecurityControl)) Then
                    Dim iRet = db.spCreateUserSpeciicPageDetails(PageDetPageControl, 0, intUserSecurityControl)
                End If

                If (db.cmPageDetails.Any(Function(x) x.PageDetControl = PageDetControl And x.PageDetPageControl = PageDetPageControl And x.PageDetUserSecurityControl = intUserSecurityControl)) Then
                    oUserDet = db.cmPageDetails.Where(Function(x) x.PageDetControl = PageDetControl And x.PageDetPageControl = PageDetPageControl And x.PageDetUserSecurityControl = intUserSecurityControl).FirstOrDefault()
                    With oUserDet
                        .PageDetWidth = PageDetWidth
                        .PageDetModDate = Date.Now
                        .PageDetModUser = Me.Parameters.UserName
                    End With
                Else

                    If (Not db.cmPageDetails.Any(Function(x) x.PageDetControl = PageDetControl And x.PageDetPageControl = PageDetPageControl And x.PageDetUserSecurityControl = 0)) Then
                        Return False
                    End If
                    'lookup the name we may be editing the original source data (updates to control numbers only happen when the screen is refreshed)
                    Dim oPageDetName As String = db.cmPageDetails.Where(Function(x) x.PageDetControl = PageDetControl And x.PageDetPageControl = PageDetPageControl And x.PageDetUserSecurityControl = 0).Select(Function(x) x.PageDetName).FirstOrDefault()
                    If (db.cmPageDetails.Any(Function(x) x.PageDetName = oPageDetName And x.PageDetPageControl = PageDetPageControl And x.PageDetUserSecurityControl = intUserSecurityControl)) Then
                        oUserDet = db.cmPageDetails.Where(Function(x) x.PageDetName = oPageDetName And x.PageDetPageControl = PageDetPageControl And x.PageDetUserSecurityControl = intUserSecurityControl).FirstOrDefault()
                        With oUserDet
                            .PageDetWidth = PageDetWidth
                            .PageDetModDate = Date.Now
                            .PageDetModUser = Me.Parameters.UserName
                        End With
                    Else
                        Return False
                    End If
                End If
                db.SubmitChanges()

                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("savePageDetUserColumnWidth"), db)
            End Try
        End Using
        Return blnRet
    End Function

    Public Function savePageDetUserColumnSequence(ByVal PageDetPageControl As Integer, ByVal PageDetControl As Integer, ByVal PageDetSequenceNo As Integer) As Boolean
        Dim blnRet As Boolean = False
        Dim intUserSecurityControl As Integer = Me.Parameters.UserControl
        If intUserSecurityControl = 0 Then Return False
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim oUserDet As LTS.cmPageDetail = New LTS.cmPageDetail()
                Dim oPageDets() As LTS.cmPageDetail
                Dim newDets As New List(Of LTS.cmPageDetail)

                If (db.cmPageDetails.Any(Function(x) x.PageDetControl = PageDetControl And x.PageDetPageControl = PageDetPageControl And x.PageDetUserSecurityControl = intUserSecurityControl)) Then
                    db.spUpdateGridFieldSequenceNo(PageDetSequenceNo, PageDetControl, PageDetPageControl, intUserSecurityControl, Parameters.UserName)
                Else
                    If (Not db.cmPageDetails.Any(Function(x) x.PageDetControl = PageDetControl And x.PageDetPageControl = PageDetPageControl And x.PageDetUserSecurityControl = 0)) Then
                        Return False
                    End If
                    'lookup the name we may be editing the original source data (updates to control numbers only happen when the screen is refreshed)
                    Dim oPageDetName As String = db.cmPageDetails.Where(Function(x) x.PageDetControl = PageDetControl And x.PageDetPageControl = PageDetPageControl And x.PageDetUserSecurityControl = 0).Select(Function(x) x.PageDetName).FirstOrDefault()
                    If (db.cmPageDetails.Any(Function(x) x.PageDetName = oPageDetName And x.PageDetPageControl = PageDetPageControl And x.PageDetUserSecurityControl = intUserSecurityControl)) Then
                        Dim pgDetControl = db.cmPageDetails.Where(Function(x) x.PageDetName = oPageDetName And x.PageDetPageControl = PageDetPageControl And x.PageDetUserSecurityControl = intUserSecurityControl).Select(Function(y) y.PageDetControl).FirstOrDefault()
                        db.spUpdateGridFieldSequenceNo(PageDetSequenceNo, pgDetControl, PageDetPageControl, intUserSecurityControl, Parameters.UserName)
                    Else
                        'create a copy of all page details for this user
                        oPageDets = db.cmPageDetails.Where(Function(x) x.PageDetPageControl = PageDetPageControl And x.PageDetUserSecurityControl = 0).ToArray()
                        If (oPageDets Is Nothing OrElse oPageDets.Count() < 1) Then Return False
                        Dim skipObjs As New List(Of String) From {"PageDetControl", "PageDetUserSecurityControl", "PageDetUpdated", "PageDetModDate", "PageDetModUser", "cmGroupSubType", "cmGroupType", "cmPage"}

                        For Each det In oPageDets.OrderBy(Function(x) x.PageDetParentID)
                            Dim strMSG As String = ""
                            Dim newDet = New LTS.cmPageDetail()
                            newDet = DTran.CopyMatchingFields(newDet, det, skipObjs, strMSG)
                            With newDet
                                If det.PageDetControl = PageDetControl Then
                                    .PageDetSequenceNo += 100000
                                End If
                                .PageDetModDate = Date.Now
                                .PageDetModUser = Me.Parameters.UserName
                                .PageDetUserSecurityControl = Me.Parameters.UserControl
                            End With
                            If Debugger.IsAttached AndAlso Not String.IsNullOrWhiteSpace(strMSG) Then
                                Debug.WriteLine(strMSG)
                            End If
                            newDets.Add(newDet)
                            db.cmPageDetails.InsertOnSubmit(newDet)
                        Next

                        db.SubmitChanges()
                        If (Not newDets Is Nothing AndAlso newDets.Count() > 0) AndAlso (Not oPageDets Is Nothing AndAlso oPageDets.Count() > 0) Then
                            'We need to look up the parent IDs
                            Dim dicParentIDXref As New Dictionary(Of Integer, Integer)
                            For Each nDet In newDets
                                If nDet.PageDetParentID > 0 Then
                                    If Not dicParentIDXref.ContainsKey(nDet.PageDetParentID) Then
                                        'lookup the PageDetParentID
                                        Dim sParentName = oPageDets.Where(Function(x) x.PageDetControl = nDet.PageDetParentID).Select(Function(x) x.PageDetName).FirstOrDefault()
                                        Dim intNewParentID = newDets.Where(Function(x) x.PageDetName = sParentName).Select(Function(x) x.PageDetControl).FirstOrDefault()
                                        dicParentIDXref.Add(nDet.PageDetParentID, intNewParentID)
                                    End If
                                    nDet.PageDetParentID = dicParentIDXref(nDet.PageDetParentID)
                                End If
                            Next
                            db.SubmitChanges()
                        End If

                        oUserDet = db.cmPageDetails.Where(Function(x) x.PageDetPageControl = PageDetPageControl AndAlso x.PageDetUserSecurityControl = intUserSecurityControl AndAlso x.PageDetSequenceNo > 100000).FirstOrDefault()
                        With oUserDet
                            .PageDetSequenceNo -= 100000
                            .PageDetModDate = Date.Now
                            .PageDetModUser = Me.Parameters.UserName
                        End With
                        db.SubmitChanges()

                        db.spUpdateGridFieldSequenceNo(PageDetSequenceNo, oUserDet.PageDetControl, PageDetPageControl, intUserSecurityControl, Parameters.UserName)

                    End If
                End If

                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("savePageDetUserColumnSequence"), db)
            End Try
        End Using
        Return blnRet
    End Function


    ''Public Function savePageDetUserColumnSequence(ByVal PageDetPageControl As Integer, ByVal PageDetControl As Integer, ByVal PageDetSequenceNo As Integer) As Boolean
    ''    Dim blnRet As Boolean = False
    ''    Dim intUserSecurityControl As Integer = Me.Parameters.UserControl
    ''    If intUserSecurityControl = 0 Then Return False
    ''    Using db As New NGLMASSecurityDataContext(ConnectionString)
    ''        Try
    ''            Dim oUserDet As LTS.cmPageDetail = New LTS.cmPageDetail()
    ''            Dim oPageDets() As LTS.cmPageDetail
    ''            Dim newDets As New List(Of LTS.cmPageDetail)

    ''            If (db.cmPageDetails.Any(Function(x) x.PageDetControl = PageDetControl And x.PageDetPageControl = PageDetPageControl And x.PageDetUserSecurityControl = intUserSecurityControl)) Then
    ''                oUserDet = db.cmPageDetails.Where(Function(x) x.PageDetControl = PageDetControl And x.PageDetPageControl = PageDetPageControl And x.PageDetUserSecurityControl = intUserSecurityControl).FirstOrDefault()
    ''                With oUserDet
    ''                    .PageDetSequenceNo = PageDetSequenceNo
    ''                    .PageDetModDate = Date.Now
    ''                    .PageDetModUser = Me.Parameters.UserName
    ''                End With
    ''            Else

    ''                If (Not db.cmPageDetails.Any(Function(x) x.PageDetControl = PageDetControl And x.PageDetPageControl = PageDetPageControl And x.PageDetUserSecurityControl = 0)) Then
    ''                    Return False
    ''                End If
    ''                'lookup the name we may be editing the original source data (updates to control numbers only happen when the screen is refreshed)
    ''                Dim oPageDetName As String = db.cmPageDetails.Where(Function(x) x.PageDetControl = PageDetControl And x.PageDetPageControl = PageDetPageControl And x.PageDetUserSecurityControl = 0).Select(Function(x) x.PageDetName).FirstOrDefault()
    ''                If (db.cmPageDetails.Any(Function(x) x.PageDetName = oPageDetName And x.PageDetPageControl = PageDetPageControl And x.PageDetUserSecurityControl = intUserSecurityControl)) Then
    ''                    oUserDet = db.cmPageDetails.Where(Function(x) x.PageDetName = oPageDetName And x.PageDetPageControl = PageDetPageControl And x.PageDetUserSecurityControl = intUserSecurityControl).FirstOrDefault()
    ''                    With oUserDet
    ''                        .PageDetSequenceNo = PageDetSequenceNo
    ''                        .PageDetModDate = Date.Now
    ''                        .PageDetModUser = Me.Parameters.UserName
    ''                    End With
    ''                Else
    ''                    'create a copy of all page details for this user
    ''                    oPageDets = db.cmPageDetails.Where(Function(x) x.PageDetPageControl = PageDetPageControl And x.PageDetUserSecurityControl = 0).ToArray()
    ''                    If (oPageDets Is Nothing OrElse oPageDets.Count() < 1) Then Return False
    ''                    Dim skipObjs As New List(Of String) From {"PageDetControl", "PageDetUserSecurityControl", "PageDetUpdated", "PageDetModDate", "PageDetModUser", "cmGroupSubType", "cmGroupType", "cmPage"}

    ''                    For Each det In oPageDets.OrderBy(Function(x) x.PageDetParentID)
    ''                        Dim strMSG As String = ""
    ''                        Dim newDet = New LTS.cmPageDetail()
    ''                        newDet = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(newDet, det, skipObjs, strMSG)
    ''                        With newDet
    ''                            If det.PageDetControl = PageDetControl Then
    ''                                .PageDetSequenceNo = PageDetSequenceNo
    ''                            End If
    ''                            .PageDetModDate = Date.Now
    ''                            .PageDetModUser = Me.Parameters.UserName
    ''                            .PageDetUserSecurityControl = Me.Parameters.UserControl
    ''                        End With
    ''                        If Debugger.IsAttached AndAlso Not String.IsNullOrWhiteSpace(strMSG) Then
    ''                            System.Diagnostics.Debug.WriteLine(strMSG)
    ''                        End If
    ''                        newDets.Add(newDet)
    ''                        db.cmPageDetails.InsertOnSubmit(newDet)
    ''                    Next
    ''                End If
    ''            End If
    ''            db.SubmitChanges()
    ''            If (Not newDets Is Nothing AndAlso newDets.Count() > 0) AndAlso (Not oPageDets Is Nothing AndAlso oPageDets.Count() > 0) Then
    ''                'We need to look up the parent IDs
    ''                Dim dicParentIDXref As New Dictionary(Of Integer, Integer)
    ''                For Each nDet In newDets
    ''                    If nDet.PageDetParentID > 0 Then
    ''                        If Not dicParentIDXref.ContainsKey(nDet.PageDetParentID) Then
    ''                            'lookup the PageDetParentID
    ''                            Dim sParentName = oPageDets.Where(Function(x) x.PageDetControl = nDet.PageDetParentID).Select(Function(x) x.PageDetName).FirstOrDefault()
    ''                            Dim intNewParentID = newDets.Where(Function(x) x.PageDetName = sParentName).Select(Function(x) x.PageDetControl).FirstOrDefault()
    ''                            dicParentIDXref.Add(nDet.PageDetParentID, intNewParentID)
    ''                        End If
    ''                        nDet.PageDetParentID = dicParentIDXref(nDet.PageDetParentID)
    ''                    End If
    ''                Next
    ''                db.SubmitChanges()
    ''            End If
    ''            blnRet = True
    ''        Catch ex As Exception
    ''            ManageLinqDataExceptions(ex, buildProcedureName("PageDetUserVisibility"), db)
    ''        End Try
    ''    End Using
    ''    Return blnRet
    ''End Function

    Public Function createPageDetailFromField(ByVal intPageControl As Integer, ByVal intParentID As Integer, ByVal intElementFieldControl As Integer) As LTS.cmPageDetail
        Dim oRet As New LTS.cmPageDetail
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            'create the data
            Try
                Dim intPageDetControl = db.spCMCreatePageDetailFromField(intPageControl, intParentID, intElementFieldControl)
                If intPageDetControl <> 0 Then
                    oRet = db.cmPageDetails.Where(Function(x) x.PageDetControl = intPageDetControl).FirstOrDefault()
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("createPageDetailFromField"), db)
            End Try
        End Using
        Return oRet
    End Function

    Public Function createAllPageDetailsForDataElement(ByVal intPageControl As Integer, ByVal intParentID As Integer, ByVal intDataElmtControl As Integer) As Boolean
        Dim blnRet As Boolean
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            'call the stored proceure
            Try
                Dim intPageDetControl = db.spCMCreateAllPageDetailsForDataElement(intPageControl, intParentID, intDataElmtControl, Parameters.UserName)
                If intPageDetControl <> 0 Then
                    blnRet = True
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("createAllPageDetailsForElement"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' retrieves the page content management details for container grouptypes 1,9, and 12 
    ''' </summary>
    ''' <param name="PageControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR v-8.0 on 2/16/2017
    '''   retrieves the page content management details for container grouptypes 1,9, and 12 
    ''' </remarks>
    Public Function getPageDetailLayout(ByVal PageControl As Integer) As List(Of LTS.cmPageDetail)
        Dim lRet As New List(Of LTS.cmPageDetail)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim oDetails = db.cmPageDetails.Where(Function(x) x.PageDetPageControl = PageControl).ToList()
                If Not oDetails Is Nothing AndAlso oDetails.Count > 0 AndAlso oDetails.Any(Function(x) x.PageDetParentID = 0) Then
                    For Each detailsection In oDetails.Where(Function(x) x.PageDetParentID = 0)
                        lRet.Add(detailsection)
                        includeDependentPageDetailContainers(lRet, detailsection.PageDetControl, oDetails)
                    Next
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("getPageDetailLayout"), db)
            End Try
        End Using
        Return lRet
    End Function

    ''' <summary>
    ''' called recursively to group page content management details for each container by grouptypes 1,9, and 12 
    ''' </summary>
    ''' <param name="results"></param>
    ''' <param name="parentControl"></param>
    ''' <param name="allDetails"></param>>
    ''' <remarks>
    ''' Created by RHR v-8.0 on 2/16/2017
    '''   groups the page content management details for each container by grouptypes 1,9, and 12 
    ''' </remarks>
    Private Sub includeDependentPageDetailContainers(ByRef results As List(Of LTS.cmPageDetail), ByVal parentControl As Integer, ByRef allDetails As List(Of LTS.cmPageDetail))
        If results Is Nothing Then results = New List(Of LTS.cmPageDetail)
        If parentControl = 0 Then Return
        If allDetails Is Nothing OrElse allDetails.Count() < 2 Then Return

        ' add Layouts first (9) 
        If allDetails.Any(Function(x) x.PageDetParentID = parentControl And x.PageDetGroupTypeControl = 9) Then
            For Each layout In allDetails.Where(Function(x) x.PageDetParentID = parentControl And x.PageDetGroupTypeControl = 9).OrderBy(Function(y) y.PageDetSequenceNo)
                results.Add(layout)
                includeDependentPageDetailContainers(results, layout.PageDetControl, allDetails)
            Next
        End If
        'followed by Interactive (12)
        If allDetails.Any(Function(x) x.PageDetParentID = parentControl And x.PageDetGroupTypeControl = 12) Then
            For Each interactive In allDetails.Where(Function(x) x.PageDetParentID = parentControl And x.PageDetGroupTypeControl = 12).OrderBy(Function(y) y.PageDetSequenceNo)
                results.Add(interactive)
                includeDependentPageDetailContainers(results, interactive.PageDetControl, allDetails)
            Next
        End If
        'followed by Data Management 1
        If allDetails.Any(Function(x) x.PageDetParentID = parentControl And x.PageDetGroupTypeControl = 1) Then
            For Each dataitem In allDetails.Where(Function(x) x.PageDetParentID = parentControl And x.PageDetGroupTypeControl = 1).OrderBy(Function(y) y.PageDetSequenceNo)
                results.Add(dataitem)
                includeDependentPageDetailContainers(results, dataitem.PageDetControl, allDetails)
            Next
        End If

    End Sub

    ''' <summary>
    ''' returns dependent page items Data Elements like text boxes
    ''' </summary>
    ''' <param name="PageDetControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR v-8.0 on 2/16/2017
    '''   returns dependent page items where the grouptype is not 1,9, and 12   typically used to retrieve the Data Elements like text boxes
    ''' </remarks>
    Public Function getPageDetailContainerDependents(ByVal PageDetControl As Integer) As List(Of LTS.cmPageDetail)
        Dim lRet As New List(Of LTS.cmPageDetail)
        Dim lContainers As New List(Of Integer) From {1, 9, 12}
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim oDetails = db.cmPageDetails.Where(Function(x) x.PageDetParentID = PageDetControl And Not lContainers.Contains(x.PageDetGroupTypeControl)).OrderBy(Function(x) x.PageDetSequenceNo).ToList()
                If Not oDetails Is Nothing AndAlso oDetails.Count > 0 Then
                    For Each detailsection In oDetails
                        lRet.Add(detailsection)
                        includeDependentPageElements(lRet, detailsection.PageDetControl, oDetails)
                    Next
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("getPageDetailContainerDependents"), db)
            End Try
        End Using
        Return lRet
    End Function

    ''' <summary>
    ''' includes all dependent page items for any grouptype
    ''' </summary>
    ''' <param name="results"></param>
    ''' <param name="parentControl"></param>
    ''' <param name="allDetails"></param>
    ''' <remarks>
    ''' Created by RHR v-8.0 on 2/16/2017
    '''   includes all dependent page items for any grouptype
    ''' </remarks>
    Private Sub includeDependentPageElements(ByRef results As List(Of LTS.cmPageDetail), ByVal parentControl As Integer, ByRef allDetails As List(Of LTS.cmPageDetail))
        If results Is Nothing Then results = New List(Of LTS.cmPageDetail)
        If parentControl = 0 Then Return
        If allDetails Is Nothing OrElse allDetails.Count() < 2 Then Return
        ' add any dependent items recursively
        If allDetails.Any(Function(x) x.PageDetParentID = parentControl) Then
            For Each item In allDetails.Where(Function(x) x.PageDetParentID = parentControl).OrderBy(Function(y) y.PageDetSequenceNo)
                results.Add(item)
                includeDependentPageElements(results, item.PageDetControl, allDetails)
            Next
        End If
    End Sub

    ''' <summary>
    ''' Gets the NGL, Comp, and User help notes for the Page
    ''' </summary>
    ''' <param name="PageControl"></param>
    ''' <param name="UserSecurityControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 5/15/17 for v-8.0 Help Pages
    ''' 
    '''  ** TODO **
    '''   Currently the only options we have are admin and everyone else
    '''   Need to figure this out plus add in level 2 for comp superuser/admin
    ''' </remarks>
    Public Function GetPageHelpInfo(ByVal PageControl As Integer, ByVal UserSecurityControl As Integer) As Models.HelpInfo
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Dim helpInfo As New Models.HelpInfo
            Try
                Dim oRet = db.spGetPageHelpNotes(PageControl, UserSecurityControl).ToArray()

                If oRet?.Count() > 0 Then
                    helpInfo.HelpWindowTitle = oRet(0).HelpWindowTitle
                    helpInfo.CompTitle = oRet(0).CompTitle
                    helpInfo.UserTitle = oRet(0).UserTitle
                    helpInfo.DefaultTitle = oRet(0).DefaultTitle

                    For Each r In oRet
                        Select Case r.NoteLevel
                            Case 1
                                helpInfo.PHControlL1 = r.PageHelpControl
                                helpInfo.NotesL1 = r.Notes
                                helpInfo.NotesLocalL1 = r.NotesLocal
                            Case 2
                                helpInfo.PHControlL2 = r.PageHelpControl
                                helpInfo.NotesL2 = r.Notes
                                helpInfo.NotesLocalL2 = r.NotesLocal
                            Case 3
                                helpInfo.PHControlL3 = r.PageHelpControl
                                helpInfo.NotesL3 = r.Notes
                                helpInfo.NotesLocalL3 = r.NotesLocal
                            Case 4
                                helpInfo.PHControlL4 = r.PageHelpControl
                                helpInfo.NotesL4 = r.Notes
                                helpInfo.NotesLocalL4 = r.NotesLocal
                        End Select
                    Next
                End If

                'Use the UserSecurityControl to get the users GroupControl from tblUserSecurity
                Dim ugroup = (From t In db.tblUserSecurities
                              Where t.UserSecurityControl = UserSecurityControl
                              Select t.UserUserGroupsControl).FirstOrDefault()
                'Use the GroupControl to get the Category Control from tblUserGroups
                Dim groupcat = (From t In db.tblUserGroups
                                Where t.UserGroupsControl = ugroup
                                Select t.UserGroupsUGCControl).FirstOrDefault()

                Select Case groupcat
                    Case 4
                        'Super Users can access all help files
                        helpInfo.ALevel = 1
                    Case 3
                        'Legal Entity Admins can access the Company and User help files
                        helpInfo.ALevel = 2
                    Case Else
                        'Everyone else can only access User Level notes
                        helpInfo.ALevel = 3
                End Select

                Return helpInfo
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetPageHelpInfo"))
            End Try
        End Using
        Return Nothing
    End Function

    Public Function InsertOrUpdatePageHelpNotes(ByVal h As Models.HelpInfo) As LTS.spInsertOrUpdatePageHelpNotesResult
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim pageHelpControl As Integer = 0
                Dim notes As String = ""

                Select Case h.ALevel
                    Case 1
                        pageHelpControl = h.PHControlL1
                        notes = h.NotesL1
                    Case 2
                        pageHelpControl = h.PHControlL2
                        notes = h.NotesL2
                    Case 3
                        pageHelpControl = h.PHControlL3
                        notes = h.NotesL3
                    Case 4
                        pageHelpControl = h.PHControlL4
                        notes = h.NotesL4
                End Select

                Dim oRet = db.spInsertOrUpdatePageHelpNotes(pageHelpControl, h.Page, h.USec, notes, h.ALevel).FirstOrDefault()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertOrUpdatePageHelpNotes"))
            End Try
        End Using
        Return Nothing
    End Function

    Public Function GetEditorContent(ByVal filter As Models.EditorContent) As Models.EditorContent
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Dim edit As New Models.EditorContent
            Try
                Dim lts As New LTS.cmPageDetail

                If (Not filter Is Nothing) Then
                    'If we have the control number user that, otherwise filter by name and page control
                    If filter.PageDetControl <> 0 Then
                        lts = (From t In db.cmPageDetails
                               Where t.PageDetControl = filter.PageDetControl
                               Select t).FirstOrDefault()
                    Else
                        If (filter.PageControl <> 0) AndAlso (Not String.IsNullOrWhiteSpace(filter.EditorName)) Then
                            lts = (From t In db.cmPageDetails
                                   Where t.PageDetPageControl = filter.PageControl _
                                              And t.PageDetName = filter.EditorName
                                   Select t).FirstOrDefault()
                        End If
                    End If
                End If

                If Not lts Is Nothing AndAlso lts.PageDetControl <> 0 Then
                    With edit
                        .PageControl = lts.PageDetPageControl
                        .USec = lts.PageDetUserSecurityControl
                        .EditorName = lts.PageDetName
                        .Content = lts.PageDetMetaData
                        .PageDetControl = lts.PageDetControl
                    End With
                Else
                    'send back default data
                    With edit
                        .PageControl = filter.PageControl
                        .USec = filter.USec
                        .EditorName = filter.EditorName
                        .Content = ""   '** TODO LVV ** Add options for default content return values. Add way for this to work with localization
                        .PageDetControl = filter.PageDetControl
                    End With
                End If

                Return edit
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEditorContent"))
            End Try
        End Using
        Return Nothing
    End Function

    Public Function SaveEditorContent(ByVal data As Models.EditorContent) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim lts As New LTS.cmPageDetail

                If data.PageDetControl <> 0 Then

                    lts = (From d In db.cmPageDetails Where d.PageDetControl = data.PageDetControl).FirstOrDefault()

                    If Not lts Is Nothing AndAlso lts.PageDetControl > 0 Then
                        With lts
                            .PageDetMetaData = data.Content
                            .PageDetModDate = Date.Now()
                            .PageDetModUser = Me.Parameters.UserName
                        End With
                        db.SubmitChanges()
                        blnRet = True
                    End If

                End If

            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                ManageLinqDataExceptions(ex, buildProcedureName("SaveEditorContent"))
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Gets all records from cmPageMenu for the PageControl that are procedures (PageMenuProcedureControl != 0)
    ''' and only returns the ones the user is authorized to execute
    ''' </summary>
    ''' <param name="PageControl"></param>
    ''' <param name="UserSecurityControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 6/23/17 for v-8.0
    ''' Modified By LVV on 4/24/20 - separated action vs navigation 
    ''' </remarks>
    Public Function GetPageActionData(ByVal PageControl As Integer, ByVal UserSecurityControl As Integer) As LTS.cmPageMenu()
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                If UserSecurityControl = 0 Then Return Nothing
                Dim procs As New List(Of LTS.cmPageMenu)
                Dim actions As New List(Of LTS.cmPageMenu)
                procs = db.cmPageMenus.Where(Function(x) x.PageMenuPageControl = PageControl And x.PageMenuProcedureControl <> 0 AndAlso x.PageMenuMenuTypeControl = 1).ToList()
                For Each p In procs
                    Dim a = db.spNetCheckProcedureSecurity365(p.PageMenuProcedureControl, UserSecurityControl).FirstOrDefault()
                    If a.Column1 Then
                        actions.Add(p) 'Authorized so add to new 0 collection (can't modify original because it breaks the for each loop)
                    End If
                Next
                Return actions.ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetPageActionData"), db)
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Returns true if the user is allowed access to the page item using the page item name.  The default is true
    ''' </summary>
    ''' <param name="PageItemName"></param>
    ''' <param name="UserSecurityControl"></param>
    ''' <param name="blnAddifMissing"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.1.1.1 on 06/06/2018
    '''  call spNetCheckProcedureSecurityByName procedure to check if the user has access to the item by name.
    '''  the procedure can optionally add the page item to the procedure security control but each name must be unique 
    '''  so before adding check the item name for unique identity.  item names like Edit or Button are not a good choice
    '''  if the page item name does not exist in the procedure table access is granted.
    '''  a user security control is required or access is denied.
    ''' </remarks>
    Public Function isPageItemAllowed(ByVal PageItemName As String, ByVal UserSecurityControl As Integer, Optional ByVal blnAddifMissing As Boolean = False) As Boolean
        Dim blnRet As Boolean = True
        If UserSecurityControl = 0 Then
            Return False
        End If
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim oResults = db.spNetCheckProcedureSecurityByName(PageItemName, blnAddifMissing, UserSecurityControl).FirstOrDefault()
                If Not oResults Is Nothing Then blnRet = oResults.AllowAccess
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("isPageItemAllowed"))
            End Try
            Return blnRet
        End Using
    End Function

    ''' <summary>
    ''' Deletes cmPageDetail records by usercontrol and pagecontrol 
    ''' </summary>
    ''' <param name="USC"></param>
    ''' <param name="PageControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.0 on 11/06/2017
    ''' Modified by LVV on 12/12/17 for v-8.0 TMS 365
    '''  Added rule that if the PageDetControl is being 
    '''  referenced by another record as a ParentID
    '''  it cannot be deleted and an exception is thrown
    ''' </remarks>
    Public Function DeleteDetail(ByVal USC As Integer, ByVal PageControl As Integer) As Boolean
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                If USC = 0 Then Return False 'Do not ever delete if usercontrol is zero
                Dim dets As LTS.cmPageDetail() = db.cmPageDetails.Where(Function(x) x.PageDetPageControl = PageControl AndAlso x.PageDetUserSecurityControl = USC).ToArray()
                If (dets Is Nothing OrElse dets.Length < 1) Then Return False
                db.cmPageDetails.DeleteAllOnSubmit(dets)
                db.SubmitChanges()
                Return True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteDetail"))
            End Try
        End Using
        Return False
    End Function

    ''' <summary>
    ''' Gets all records from cmPageMenu for the ReportControl that are reports (PageMenuReportControl != 0)
    ''' and only returns the ones the user is authorized to execute
    ''' </summary>
    ''' <param name="PageControl"></param>
    ''' <param name="UserSecurityControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 6/27/19 for v-8.2
    ''' </remarks>
    Public Function GetPageReportData(ByVal PageControl As Integer, ByVal UserSecurityControl As Integer) As LTS.cmPageMenu()
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                If UserSecurityControl = 0 Then Return Nothing
                Dim rpts As New List(Of LTS.cmPageMenu)
                Dim reports As New List(Of LTS.cmPageMenu)
                rpts = db.cmPageMenus.Where(Function(x) x.PageMenuPageControl = PageControl And x.PageMenuReportControl <> 0).ToList()
                For Each p In rpts
                    Dim a = db.spNetCheckReportSecurity365(p.PageMenuReportControl, UserSecurityControl).FirstOrDefault()
                    If a.Column1 Then
                        reports.Add(p) 'Authorized so add to new 0 collection (can't modify original because it breaks the for each loop)
                    End If
                Next
                Return reports.ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetPageReportData"), db)
            End Try
            Return Nothing
        End Using
    End Function

    Public Function PageMaintHideAllControls(ByVal iPageControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim rVal = db.spCmPageMaintHideAllControls(iPageControl)
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("PageMaintHideAllControls"), db)
            End Try
            Return Nothing
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Gets all records from cmPageMenu for the PageControl that are procedures (PageMenuProcedureControl != 0)
    ''' and only returns the ones the user is authorized to execute. Only returns Menu Type Navigation
    ''' </summary>
    ''' <param name="PageControl"></param>
    ''' <param name="UserSecurityControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 4/24/20
    ''' </remarks>
    Public Function GetPageNavigationData(ByVal PageControl As Integer, ByVal UserSecurityControl As Integer) As LTS.cmPageMenu()
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                If UserSecurityControl = 0 Then Return Nothing
                Dim procs As New List(Of LTS.cmPageMenu)
                Dim actions As New List(Of LTS.cmPageMenu)
                procs = db.cmPageMenus.Where(Function(x) x.PageMenuPageControl = PageControl And x.PageMenuProcedureControl <> 0 And x.PageMenuMenuTypeControl = 2).ToList()
                For Each p In procs
                    Dim a = db.spNetCheckProcedureSecurity365(p.PageMenuProcedureControl, UserSecurityControl).FirstOrDefault()
                    If a.Column1 Then
                        actions.Add(p) 'Authorized so add to new 0 collection (can't modify original because it breaks the for each loop)
                    End If
                Next
                Return actions.ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetPageNavigationData"), db)
            End Try
            Return Nothing
        End Using
    End Function

#End Region

#Region "Protected Functions"

#End Region

End Class

Public Class NGLUserSecurityCarrierData : Inherits NGLLinkDataBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        'Dim db As New NGLMASSecurityDataContext(ConnectionString)
        'Me.LinqTable = db.tblUserSecurityCarriers
        'Me.LinqDB = db
        Me.SourceClass = "NGLUserSecurityCarrierData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASSecurityDataContext(ConnectionString)
                _LinqTable = db.tblUserSecurityCarriers
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

#Region "DEPRECIATED"

    ''' <summary>
    ''' DEPRECIATED by LVV on 3/29/19 - I don't think this is used anywhere
    ''' </summary>
    ''' <param name="USCControl"></param>
    ''' <returns></returns>
    Public Function GetRecord(ByVal USCControl As Integer) As LTS.tblUserSecurityCarrier
        throwDepreciatedException("This version of " & buildProcedureName("GetRecord") & " has been Depreciated")
        Return Nothing
        ''Dim oRet As New LTS.tblUserSecurityCarrier
        ''Using db As New NGLMASSecurityDataContext(ConnectionString)
        ''    Try
        ''        oRet = db.tblUserSecurityCarriers.Where(Function(x) x.USCControl = USCControl).FirstOrDefault()
        ''    Catch ex As Exception
        ''        Utilities.SaveAppError(ex.Message, Me.Parameters)
        ''        ManageLinqDataExceptions(ex, buildProcedureName("GetRecord"))
        ''    End Try
        ''    Return oRet
        ''End Using
    End Function

    ''' <summary>
    ''' DEPRECIATED by LVV on 3/29/19 - I don't think this is used anywhere
    ''' 
    ''' Gets all records in the tblUserSecurityCarrier table
    ''' </summary>
    ''' <returns></returns>
    Public Function GetAllRecords() As List(Of LTS.tblUserSecurityCarrier)
        throwDepreciatedException("This version of " & buildProcedureName("GetAllRecords") & " has been Depreciated")
        Return Nothing
        ''Dim oRet As New List(Of LTS.tblUserSecurityCarrier)
        ''Using db As New NGLMASSecurityDataContext(ConnectionString)
        ''    Try
        ''        oRet = db.tblUserSecurityCarriers.ToList()

        ''    Catch ex As Exception
        ''        Utilities.SaveAppError(ex.Message, Me.Parameters)
        ''        ManageLinqDataExceptions(ex, buildProcedureName("GetAllRecords"))
        ''    End Try
        ''    Return oRet
        ''End Using
    End Function

    ''' <summary>
    ''' DEPRECIATED by LVV on 3/29/19 - We now use InsertOrUpdateUserSecurityCarrier()
    ''' </summary>
    ''' <param name="oRecord"></param>
    ''' <returns></returns>
    Public Function Save(oRecord As LTS.tblUserSecurityCarrier) As LTS.tblUserSecurityCarrier
        throwDepreciatedException("This version of " & buildProcedureName("Save") & " has been Depreciated")
        Return Nothing
        ''Using db As New NGLMASSecurityDataContext(ConnectionString)
        ''    Try
        ''        Dim oTable = db.tblUserSecurityCarriers
        ''        oRecord.USCModDate = Date.Now()
        ''        oRecord.USCModUser = Me.Parameters.UserName
        ''        oTable.Attach(oRecord, True)
        ''        db.SubmitChanges()
        ''    Catch ex As Exception
        ''        Utilities.SaveAppError(ex.Message, Me.Parameters)
        ''        ManageLinqDataExceptions(ex, buildProcedureName("Save"))
        ''    End Try
        ''    Return oRecord
        ''End Using
    End Function

    ''' <summary>
    ''' DEPRECIATED by LVV on 3/29/19 - We now use InsertOrUpdateUserSecurityCarrier()
    ''' </summary>
    ''' <param name="oRecord"></param>
    ''' <returns></returns>
    Public Function Insert(oRecord As LTS.tblUserSecurityCarrier) As LTS.tblUserSecurityCarrier
        throwDepreciatedException("This version of " & buildProcedureName("Insert") & " has been Depreciated")
        Return Nothing
        ''Using db As New NGLMASSecurityDataContext(ConnectionString)
        ''    Try
        ''        ''Perform some validation
        ''        'If String.IsNullOrWhiteSpace(oRecord.PageName) Then
        ''        '    'E_FieldRequired = 'The '{0}' is required and cannot be empty.
        ''        '    Dim oFaultDetails As New List(Of String) From {"Page Name"}
        ''        '    throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_InvalidKeyFilterMetaData, SqlFaultInfo.FaultDetailsKey.E_FieldRequired, oFaultDetails, SqlFaultInfo.FaultReasons.E_DataValidationFailure, True)
        ''        'End If
        ''        ''If db.cmPages.Any(Function(x) x.cmLocalKey = oRecord.cmLocalKey) Then
        ''        ''    'E_CannotSaveKeyValuesAlreadyExist = 'Cannot save changes to {0}. A record with one or more of the following key values already exist: Keys {1} Values {2}.
        ''        ''    Dim oFaultDetails As New List(Of String) From {"Localization Data", "Localization Key", oRecord.cmLocalKey}
        ''        ''    throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_InvalidKeyFilterMetaData, SqlFaultInfo.FaultDetailsKey.E_CannotSaveKeyValuesAlreadyExist, oFaultDetails, SqlFaultInfo.FaultReasons.E_DataValidationFailure, True)
        ''        ''End If
        ''        Dim oTable = db.tblUserSecurityCarriers
        ''        oRecord.USCModDate = Date.Now()
        ''        oRecord.USCModUser = Me.Parameters.UserName
        ''        oTable.InsertOnSubmit(oRecord)
        ''        db.SubmitChanges()
        ''    Catch ex As Exception
        ''        Utilities.SaveAppError(ex.Message, Me.Parameters)
        ''        ManageLinqDataExceptions(ex, buildProcedureName("Insert"))
        ''    End Try
        ''    Return oRecord
        ''End Using
    End Function

    ''' <summary>
    ''' DEPRECIATED by LVV on 3/29/19 - I don't think this is used anywhere
    ''' </summary>
    ''' <param name="oRecord"></param>
    ''' <returns></returns>
    Public Function Delete(oRecord As LTS.tblUserSecurityCarrier) As Boolean
        throwDepreciatedException("This version of " & buildProcedureName("Delete") & " has been Depreciated")
        Return Nothing
        ''Dim blnRet As Boolean = False
        ''Using db As New NGLMASSecurityDataContext(ConnectionString)
        ''    Dim oTable = db.tblUserSecurityCarriers
        ''    Try
        ''        oTable.Attach(oRecord, True)
        ''        oTable.DeleteOnSubmit(oRecord)
        ''        LinqDB.SubmitChanges()
        ''        blnRet = True
        ''    Catch ex As SqlException
        ''        Utilities.SaveAppError(ex.Message, Me.Parameters)
        ''        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        ''    Catch conflictEx As ChangeConflictException
        ''        Try
        ''            Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(LinqDB))
        ''            conflictInfo.LogConflictDetails(conflictInfo.Message, Me.Parameters)
        ''            Throw New FaultException(Of ConflictFaultInfo)(conflictInfo, New FaultReason("E_DataConflict"))
        ''        Catch ex As FaultException
        ''            Throw
        ''        Catch ex As Exception
        ''            Utilities.SaveAppError(ex.Message, Me.Parameters)
        ''            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        ''        End Try
        ''    Catch ex As InvalidOperationException
        ''        Utilities.SaveAppError(ex.Message, Me.Parameters)
        ''        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
        ''    Catch ex As Exception
        ''        Utilities.SaveAppError(ex.Message, Me.Parameters)
        ''        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        ''    End Try
        ''End Using
        ''Return blnRet
    End Function

    ''' <summary>
    ''' DEPRECIATED by LVV on 3/29/19 - I don't think this is used anywhere
    ''' </summary>
    ''' <param name="Control"></param>
    ''' <returns></returns>
    ''Public Function Delete(Control As Integer) As Boolean
    ''    throwDepreciatedException("This version of " & buildProcedureName("Delete") & " has been Depreciated")
    ''    Return Nothing
    ''    ''Dim blnRet As Boolean = False
    ''    ''Using db As New NGLMASSecurityDataContext(ConnectionString)
    ''    ''    Dim oTable = db.tblUserSecurityCarriers
    ''    ''    Try
    ''    ''        Dim oRecord As LTS.tblUserSecurityCarrier = db.tblUserSecurityCarriers.Where(Function(x) x.USCControl = Control).FirstOrDefault()
    ''    ''        If (oRecord Is Nothing OrElse oRecord.USCControl = 0) Then Return False
    ''    ''        oTable.Attach(oRecord, True)
    ''    ''        oTable.DeleteOnSubmit(oRecord)
    ''    ''        LinqDB.SubmitChanges()
    ''    ''        blnRet = True
    ''    ''    Catch ex As SqlException

    ''    ''        Utilities.SaveAppError(ex.Message, Me.Parameters)
    ''    ''        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
    ''    ''    Catch conflictEx As ChangeConflictException
    ''    ''        Try
    ''    ''            Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(LinqDB))
    ''    ''            conflictInfo.LogConflictDetails(conflictInfo.Message, Me.Parameters)
    ''    ''            Throw New FaultException(Of ConflictFaultInfo)(conflictInfo, New FaultReason("E_DataConflict"))
    ''    ''        Catch ex As FaultException
    ''    ''            Throw
    ''    ''        Catch ex As Exception
    ''    ''            Utilities.SaveAppError(ex.Message, Me.Parameters)
    ''    ''            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
    ''    ''        End Try
    ''    ''    Catch ex As InvalidOperationException
    ''    ''        Utilities.SaveAppError(ex.Message, Me.Parameters)
    ''    ''        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
    ''    ''    Catch ex As Exception
    ''    ''        Utilities.SaveAppError(ex.Message, Me.Parameters)
    ''    ''        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
    ''    ''    End Try
    ''    ''End Using
    ''    ''Return blnRet
    ''End Function

#End Region

    ''' <summary>
    ''' Gets the first tblUserSecurityCarrier record filtered by the UserSecurityControl
    ''' </summary>
    ''' <param name="UserSecurityControl"></param>
    ''' <returns></returns>
    Public Function GetFirstRecordByUser(ByVal UserSecurityControl As Integer) As LTS.tblUserSecurityCarrier
        Dim oRet As New LTS.tblUserSecurityCarrier
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                oRet = db.tblUserSecurityCarriers.Where(Function(x) x.USCUserSecurityControl = UserSecurityControl).FirstOrDefault()
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                ManageLinqDataExceptions(ex, buildProcedureName("GetFirstRecordByUser"), db)
            End Try
            Return oRet
        End Using
    End Function

    Public Function GetvUserSecurityCarrier(ByVal USCControl As Integer) As LTS.vUserSecurityCarrier
        Dim oRet As New LTS.vUserSecurityCarrier
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                oRet = db.vUserSecurityCarriers.Where(Function(x) x.USCControl = USCControl).FirstOrDefault()
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                ManageLinqDataExceptions(ex, buildProcedureName("GetvUserSecurityCarrier"), db)
            End Try
            Return oRet
        End Using
    End Function

    ''' <summary>
    ''' Gets the vUserSecurityCarrier records filtered by UserSecurityControl passed in as AllFilters.ParentControl
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns></returns>
    Public Function GetvUserSecurityCarriers(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.vUserSecurityCarrier()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vUserSecurityCarrier
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'Get the data iqueryable
                Dim iQuery As IQueryable(Of LTS.vUserSecurityCarrier)
                iQuery = db.vUserSecurityCarriers.Where(Function(x) x.USCUserSecurityControl = filters.ParentControl)
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetvUserSecurityCarriers"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Adds or updates records in tblUserSecurityCarrier
    ''' Returns "" on success or message on fail
    ''' </summary>
    ''' <param name="oRecord"></param>
    ''' <remarks>
    ''' RULES: 
    ''' CARRIER USER (only allowed one record in tblUserSecurityCarrier)
    ''' TMS (NON-CARRIER) USER (allowed multiple records (associated carriers) in tblUserSecurityCarrier)
    ''' Carrier Users are identified by belonging to "Carriers" or "Carrier Accountants" (GroupControl 7 or 8)
    ''' </remarks>
    Public Function InsertOrUpdateUserSecurityCarrier(ByVal oRecord As LTS.tblUserSecurityCarrier) As String
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Dim strRet As String = ""
            Try
                Dim intUserSecurityControl = oRecord.USCUserSecurityControl
                Dim iUSCControl As Integer = oRecord.USCControl
                oRecord.USCModDate = Date.Now
                oRecord.USCModUser = Parameters.UserName
                If iUSCControl = 0 Then
                    'INSERT
                    Dim groupControl = db.tblUserSecurities.Where(Function(x) x.UserSecurityControl = intUserSecurityControl).Select(Function(y) y.UserUserGroupsControl).FirstOrDefault()
                    If groupControl = 7 OrElse groupControl = 8 Then 'THIS IS A CARRIER USER (only allowed one record in tblUserSecurityCarrier)
                        If db.tblUserSecurityCarriers.Any(Function(x) x.USCUserSecurityControl = intUserSecurityControl) Then
                            Dim oLocalize As New NGLcmLocalizeKeyValuePairData(Parameters)
                            strRet = oLocalize.GetLocalizedValueByKey("E_AddUserAssociatedCarrierFail", "Add Associated Carrier Failure") + " - " + oLocalize.GetLocalizedValueByKey("E_CarrierUserOnlyOneAssociatedCarrier", "A Carrier User Account is only allowed to have one Associated Carrier")
                            Return strRet
                        End If
                    End If
                    db.tblUserSecurityCarriers.InsertOnSubmit(oRecord)
                    db.SubmitChanges()
                Else
                    'UPDATE
                    db.tblUserSecurityCarriers.Attach(oRecord, True)
                    db.SubmitChanges()
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertOrUpdateUserSecurityCarrier"), db)
            End Try
            Return strRet
        End Using
    End Function

    Public Function Delete(ByVal iUSCControl As Integer, Optional ByRef strMsg As String = "") As Boolean
        Dim blnRet As Boolean = False
        If iUSCControl = 0 Then throwInvalidRequiredKeysException("User Security Carrier", "Invalid User Security Carrier, a control number is required and cannot be zero")
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                If db.tblUserSecurityCarriers.Any(Function(x) x.USCControl = iUSCControl) Then
                    Dim oRecord As LTS.tblUserSecurityCarrier = db.tblUserSecurityCarriers.Where(Function(x) x.USCControl = iUSCControl).FirstOrDefault()
                    If Not oRecord Is Nothing AndAlso oRecord.USCControl <> 0 Then

                        'check if the parent of this record a carrier user
                        If db.tblUserSecurities.Any(Function(x) x.UserSecurityControl = oRecord.USCUserSecurityControl And (x.UserUserGroupsControl = 7 Or x.UserUserGroupsControl = 8)) Then
                            'check to see if there are other associated carrier records for this user account (Carrier Users are required to have one and only one associated carrier)
                            If Not db.tblUserSecurityCarriers.Any(Function(x) x.USCUserSecurityControl = oRecord.USCUserSecurityControl And x.USCCarrierControl <> oRecord.USCCarrierControl) Then
                                'cannot delete this associated carrier because carrier users are required to have one associated carrier
                                Dim oLocalize As New NGLcmLocalizeKeyValuePairData(Parameters)
                                strMsg = oLocalize.GetLocalizedValueByKey("E_DeleteAssociatedCarrierFailCarUser", "Cannot delete Associated Carrier Record. An Associated Carrier is required for a Carrier User account.")
                                Return blnRet
                            End If
                        End If


                        db.tblUserSecurityCarriers.DeleteOnSubmit(oRecord)
                        db.SubmitChanges()
                        blnRet = True
                    End If
                Else
                    blnRet = True 'return true if the record does not exist (already deleted)
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("Delete"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' If IsEdit is true then get the records tblLegalEntityCarrier for the provided users LE that have not yet been added to tblUserSecurityCarrier
    ''' If IsEdit is false then get all records in tblLegalEntityCarrier for the provided users LE
    ''' </summary>
    ''' <param name="USC"></param>
    ''' <param name="IsEdit"></param>
    ''' <returns></returns>
    Public Function GetUserAssociatedCarriersList(ByVal USC As Integer, ByVal IsEdit As Boolean) As DTO.vLookupList()
        Dim vList() As DTO.vLookupList
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                vList = (
                    From t In db.spGetUserAssociatedCarriers(USC, IsEdit)
                    Order By t.CarrierName
                    Select New DTO.vLookupList _
                    With {.Control = t.CarrierControl, .Name = t.CarrierName, .Description = t.CarrierNumber.ToString}).ToArray()
                Return vList
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                ManageLinqDataExceptions(ex, buildProcedureName("GetUserAssociatedCarriersList"), db)
            End Try
            Return Nothing
        End Using
    End Function

    Public Function RestrictedCarriersForSalesReps() As List(Of Integer)
        Dim lCarriers As New List(Of Integer)
        If Me.Parameters.CatControl = 11 Then
            Using db As New NGLMASSecurityDataContext(ConnectionString)
                Try
                    lCarriers = db.tblUserSecurityCarriers.Where(Function(x) x.USCUserSecurityControl = Me.Parameters.UserControl).Select(Function(y) y.USCCarrierControl).ToList()
                Catch ex As Exception
                    ManageLinqDataExceptions(ex, buildProcedureName("RestrictedCarriersForSalesReps"), db)
                End Try
            End Using
        End If

        Return lCarriers
    End Function

#End Region

#Region "Protected Functions"

#End Region

End Class

Public Class NGLUserSecurityLegalEntityData : Inherits NGLLinkDataBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        'Dim db As New NGLMASSecurityDataContext(ConnectionString)
        'Me.LinqTable = db.tblUserSecurityLegalEntities
        'Me.LinqDB = db
        Me.SourceClass = "NGLUserSecurityLegalEntityData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASSecurityDataContext(ConnectionString)
                _LinqTable = db.tblUserSecurityLegalEntities
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

    Public Function GetLEUsers365(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.vLEUsers365()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vLEUsers365
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'Get the data iqueryable
                Dim iQuery As IQueryable(Of LTS.vLEUsers365)
                iQuery = db.vLEUsers365s
                Dim filterWhere = " (LEAdminControl = " & filters.LEAdminControl & ") "
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLEUsers365"), db)
            End Try
        End Using
        Return Nothing
    End Function

    Public Function InsertOrUpdateUserSecurityLE(ByVal USC As Integer, ByVal LEAdminControl As Integer) As LTS.spInsertOrUpdateUserSecurityLEResult
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try

                Return db.spInsertOrUpdateUserSecurityLE(USC, LEAdminControl).FirstOrDefault()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertOrUpdateUserSecurityLE"), db)
            End Try
            Return Nothing
        End Using
    End Function

    Public Function GetUsersForLE(ByRef db As NGLMASSecurityDataContext, ByVal LEAControl As Integer) As Integer()
        Try
            Dim retVals = db.tblUserSecurityLegalEntities.Where(Function(x) x.USLELEAdminControl = LEAControl).Select(Function(y) y.USLEUserSecurityControl).ToArray()
            If retVals?.Length < 1 Then retVals = New Integer() {Parameters.UserControl}
        Catch ex As Exception
            ManageLinqDataExceptions(ex, buildProcedureName("GetUsersForLE"), db)
        End Try
        Return Nothing
    End Function

    Public Function GetUsersForLE(ByVal LEAControl As Integer) As Integer()
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Return GetUsersForLE(db, LEAControl)
        End Using
    End Function

    Public Function GetActiveLEUsers365(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.vLEUsers365()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vLEUsers365
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'Get the data iqueryable
                Dim iQuery As IQueryable(Of LTS.vLEUsers365)
                'iQuery = db.vLEUsers365s
                iQuery = db.vLEUsers365s.Where(Function(x) x.UserUserGroupsControl <> 10 AndAlso x.UserUserGroupsControl <> 5 AndAlso x.UserUserGroupsControl <> 6)
                Dim filterWhere = " (LEAdminControl = " & filters.LEAdminControl & ") "
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetActiveLEUsers365"), db)
            End Try
        End Using
        Return Nothing
    End Function


#End Region

#Region "Protected Functions"

#End Region

End Class

Public Class NGLUserPageSettingData : Inherits NGLLinkDataBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        'Dim db As New NGLMASSecurityDataContext(ConnectionString)
        'Me.LinqTable = db.tblUserPageSettings
        'Me.LinqDB = db
        Me.SourceClass = "NGLUserPageSettingData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If (_LinqTable Is Nothing) Then
                Dim db As New NGLMASSecurityDataContext(ConnectionString)
                _LinqTable = db.tblUserPageSettings
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

    Public Function GetRecord(ByVal UPSControl As Integer) As LTS.tblUserPageSetting
        Dim oRet As New LTS.tblUserPageSetting
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                oRet = ReadCurrentUserPageSetting(UPSControl, db)
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetRecord"), db)
            End Try
            Return oRet
        End Using
    End Function

    Public Function GetPageSettingsForCurrentUser(ByVal PageControl As Integer, Optional ByVal UserPSName As String = "") As LTS.tblUserPageSetting()
        Dim oRet As LTS.tblUserPageSetting()
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                If String.IsNullOrWhiteSpace(UserPSName) Then
                    oRet = db.tblUserPageSettings.Where(Function(x) x.UserPSUserSecurityControl = Parameters.UserControl AndAlso x.UserPSPageControl = PageControl).ToArray()
                Else
                    oRet = db.tblUserPageSettings.Where(Function(x) x.UserPSUserSecurityControl = Parameters.UserControl AndAlso x.UserPSPageControl = PageControl AndAlso x.UserPSName.Contains(UserPSName)).ToArray()
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetPageSettingsForCurrentUser"), db)
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' Creates a new record if the PK (UserPSControl) is zero or updates the exiting record
    ''' Throws Field Required Fault Exception if UserPSName is blank
    ''' Throws Key Already Exists Fault Exception if the UserPSName exists for UserPSControl zero 
    ''' and a UserPSPageControl is zero (cannot insert duplicates)    ''' 
    ''' </summary>
    ''' <param name="oRecord"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR For v-8.2 On 08/21/2018
    ''' Modified by RHR for v-8.2 on 09/15/2018
    ''' Modified by RHR for v-8.4.0.002 on 03/01/2021
    '''     added bug fix to support the situaltion where multiple calls to save the filter can be executed
    '''     async when a page loads.  this can cause an invalid key constraint error on insert because the run 
    '''     at the same time.  the initial Any function fails but the insert fails because it is not thread safe
    ''' Modified by RHR for v-8.5.3.007 on 01/13/2023 added logic to skip updates when user control is zero
    ''' Modifie by RHR for v-8.5.4.003 on 10/10/2023 added logic to check for missing page settings keys
    ''' </remarks>
    Public Function InsertOrUpdateCurrentUserPageSetting(ByVal oRecord As LTS.tblUserPageSetting) As LTS.tblUserPageSetting
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim intUserControl = oRecord.UserPSUserSecurityControl
                If intUserControl = 0 Then Return Nothing ' Modified by RHR for v-8.5.3.007 on 01/13/2023
                Dim sName = oRecord.UserPSName
                Dim intPageControl = oRecord.UserPSPageControl
                Dim iUserPSControl As Integer = oRecord.UserPSControl
                If String.IsNullOrWhiteSpace(sName) Then throwFieldRequiredException("UserPSName")

                If iUserPSControl = 0 Then ' we do not have a page setting control
                    If intUserControl <> 0 AndAlso db.tblUserPageSettings.Any(Function(x) x.UserPSUserSecurityControl = intUserControl And x.UserPSName = sName And x.UserPSPageControl = intPageControl) Then
                        Dim oExisting = db.tblUserPageSettings.Where(Function(x) x.UserPSUserSecurityControl = intUserControl And x.UserPSName = sName And x.UserPSPageControl = intPageControl).FirstOrDefault()
                        oExisting.UserPSMetaData = oRecord.UserPSMetaData
                        oExisting.UserPSAPIFilterID = oRecord.UserPSAPIFilterID
                        oExisting.UserPSAPIReference = oRecord.UserPSAPIReference
                        oExisting.UserPSAPISortKey = oRecord.UserPSAPISortKey
                        oExisting.UserPSModel = oRecord.UserPSModel
                        db.SubmitChanges()
                        Return oExisting
                    Else
                        If db.tblUserPageSettings.Any(Function(x) x.UserPSUserSecurityControl = intUserControl And x.UserPSName = sName And x.UserPSPageControl = intPageControl) Then
                            throwInvalidKeyAlreadyExistsException("Page Settings", "UserPSName", sName)
                        End If
                        'check for missing page settings keys
                        If (String.IsNullOrWhiteSpace(oRecord.UserPSName)) Then
                            Return New LTS.tblUserPageSetting()
                        End If
                        If (oRecord.UserPSPageControl = 0) Then
                            Return New LTS.tblUserPageSetting()
                        End If
                        db.tblUserPageSettings.InsertOnSubmit(oRecord)
                        db.SubmitChanges()
                    End If
                Else
                    If intUserControl <> 0 Then
                        db.tblUserPageSettings.Attach(oRecord, True)
                        db.SubmitChanges()
                    End If
                End If

            Catch ex As System.Data.SqlClient.SqlException
                'if we get here another thread or process has attempted to insert records at the same time so just ignore the error and continue
                Utilities.SaveAppError("Unexpected SQL Error in " & buildProcedureName("InsertOrUpdateCurrentUserPageSetting") & ": " & ex.Message, Me.Parameters)
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertOrUpdateCurrentUserPageSetting"), db)
            End Try
        End Using
        If oRecord Is Nothing Then oRecord = New LTS.tblUserPageSetting()
        Return oRecord
    End Function


    ''' <summary>
    ''' Wrapper for InsertOrUpdateCurrentUserPageSetting except return boolean
    ''' Creates a new record if the PK (UserPSControl) is zero or updates the exiting record
    ''' Throws Field Required Fault Exception if UserPSName is blank
    ''' Throws Key Already Exists Fault Exception if the UserPSName exists for UserPSControl zero 
    ''' and a UserPSPageControl is zero (cannot insert duplicates)   
    ''' </summary>
    ''' <param name="oRecord"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR For v-8.2 On 08/21/2018
    ''' Modified by RHR for v-8.2 on 09/15/2018
    '''  we now call InsertOrUpdateCurrentUserPageSetting for code reuse
    ''' </remarks>
    Public Function SaveCurrentUserPageSetting(ByVal oRecord As LTS.tblUserPageSetting) As Boolean
        Dim blnRet As Boolean = False
        Dim oResults = InsertOrUpdateCurrentUserPageSetting(oRecord)
        If (Not oResults Is Nothing AndAlso oResults.UserPSControl <> 0) Then blnRet = True
        Return blnRet
    End Function

    ''' <summary>
    ''' Gets the saved page settings for the current user for a specific page and grid on it
    ''' </summary>
    ''' <remarks>
    ''' Created by CHA On 07/26/2021
    ''' </remarks>
    Public Function GetPageSettingsByGridForCurrentUser(ByVal PageControl As Integer, ByVal Grid As String) As LTS.tblUserPageSetting()
        Dim oRet As LTS.tblUserPageSetting()
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                oRet = db.tblUserPageSettings.Where(Function(x) x.UserPSUserSecurityControl = Parameters.UserControl AndAlso x.UserPSPageControl = PageControl AndAlso x.UserPSAPIFilterID = Grid).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetPageSettingsByGridForCurrentUser"), db)
            End Try
        End Using
        Return oRet
    End Function

    Public Function GetPageSettings(ByVal PageControl As Integer, ByVal UserPSName As String, ByVal UserControl As Integer) As LTS.tblUserPageSetting()
        Dim oRet As LTS.tblUserPageSetting()
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                oRet = db.tblUserPageSettings.Where(Function(x) x.UserPSUserSecurityControl = UserControl AndAlso x.UserPSPageControl = PageControl AndAlso x.UserPSName.Contains(UserPSName)).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetPageSettings"), db)
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' Deletes a single saved used filter by it's name, grid id, page control number and the curent user
    ''' </summary>
    ''' <remarks>
    ''' Created by CHA On 08/31/2021
    ''' </remarks>
    Public Function DeleteFilterByNameAndGridForCurrentUser(ByVal FilterName As String, ByVal PageControl As Integer, ByVal Grid As String) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim oRecord As LTS.tblUserPageSetting = db.tblUserPageSettings.Where(Function(x) x.UserPSUserSecurityControl = Parameters.UserControl AndAlso x.UserPSPageControl = PageControl AndAlso x.UserPSAPIFilterID = Grid AndAlso x.UserPSName = FilterName).FirstOrDefault()
                If (oRecord Is Nothing OrElse oRecord.UserPSControl = 0) Then Return blnRet
                db.tblUserPageSettings.DeleteOnSubmit(oRecord)
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteFilterByNameAndGridForCurrentUser"), db)
            End Try
        End Using
        Return blnRet
    End Function

    Public Sub DeleteUserPageSetting(ByVal UPSControl As Integer)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim oRecord As LTS.tblUserPageSetting = db.tblUserPageSettings.Where(Function(x) x.UserPSControl = UPSControl).FirstOrDefault()
                If (oRecord Is Nothing OrElse oRecord.UserPSControl = 0) Then Return

                db.tblUserPageSettings.DeleteOnSubmit(oRecord)
                db.SubmitChanges()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteUserPageSetting"), db)
            End Try
        End Using
    End Sub

    Public Sub DeleteUserPageSettingFiltered(ByVal USC As Integer, ByVal PageControl As Integer, ByVal PSName As String)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim oRecord As LTS.tblUserPageSetting = db.tblUserPageSettings.Where(Function(x) x.UserPSUserSecurityControl = USC AndAlso x.UserPSPageControl = PageControl AndAlso x.UserPSName = PSName).FirstOrDefault()
                If (oRecord Is Nothing OrElse oRecord.UserPSControl = 0) Then Return
                db.tblUserPageSettings.DeleteOnSubmit(oRecord)
                db.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteUserPageSettingFiltered"), db)
            End Try
        End Using
    End Sub


#End Region

#Region "Protected or Private Functions"

    ''' <summary>
    ''' read record, caller must handle all exceptions
    ''' </summary>
    ''' <param name="UPSControl"></param>
    ''' <param name="db"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 08/21/2018
    '''   Nested method to share db connection and read page settings by control
    '''   Caller must handle all errors 
    ''' </remarks>
    Private Function ReadCurrentUserPageSetting(ByVal UPSControl As Integer, ByRef db As NGLMASSecurityDataContext) As LTS.tblUserPageSetting
        Return db.tblUserPageSettings.Where(Function(x) x.UserPSControl = UPSControl).FirstOrDefault()
    End Function
#End Region

End Class

'Created By LVV on 4/10/19 - I wanted to use the functionality of the new technology
'There are still old methods above that deal with User Groups which will hopefully be phased out
Public Class NGLUserGroupData : Inherits NGLLinkDataBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASSecurityDataContext(ConnectionString)
        Me.LinqTable = db.tblUserGroups
        Me.LinqDB = db
        Me.SourceClass = "NGLUserGroupData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASSecurityDataContext(ConnectionString)
                _LinqTable = db.tblUserGroups
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

    ''' <summary>
    ''' If called by a SuperUser return all UserGroups, else return only UserGroups associated with the Legal Entity aka AllFilters.ParentControl
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns></returns>
    Public Function GetUserGroups(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.vUserGroup()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vUserGroup
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'Get the data iqueryable
                Dim iQuery As IQueryable(Of LTS.vUserGroup)
                If Parameters.CatControl = 4 Then
                    iQuery = db.vUserGroups
                Else
                    iQuery = db.vUserGroups.Where(Function(x) x.LEAdminControl = filters.ParentControl)
                End If
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetUserGroups"), db)
            End Try
        End Using
        Return Nothing
    End Function

    Public Function GetUserSecurityLegalEntity(ByVal UserSecurityControl As Int32) As LTS.tblUserSecurityLegalEntity()
        If UserSecurityControl = 0 Then Return Nothing
        Dim oRet() As LTS.tblUserSecurityLegalEntity
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'Get the data iqueryable
                Dim iQuery As IQueryable(Of LTS.tblUserSecurityLegalEntity)

                iQuery = db.tblUserSecurityLegalEntities.Where(Function(x) x.USLEUserSecurityControl = UserSecurityControl)

                oRet = iQuery.ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetUserSecurityLegalEntity"), db)
            End Try
        End Using
        Return Nothing
    End Function

    Public Function InsertOrUpdateUserGroup(ByVal oData As LTS.tblUserGroup) As Boolean
        Dim blnRet As Boolean = False
        If oData Is Nothing Then Return False 'nothing to do
        Dim iUGCControl = oData.UserGroupsUGCControl
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'verify that a Category record exists
                If iUGCControl = 0 Then
                    Dim lDetails As New List(Of String) From {"Category Reference", " was not provided and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                If Not db.tblUserGroupCategories.Any(Function(x) x.UGCControl = iUGCControl) Then
                    Dim lDetails As New List(Of String) From {"Category Reference", " was not found and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If

                If oData.UserGroupsControl = 0 Then
                    db.tblUserGroups.InsertOnSubmit(oData)
                Else
                    db.tblUserGroups.Attach(oData, True)
                End If
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertOrUpdateUserGroup"), db)
            End Try
        End Using
        Return blnRet
    End Function

    Public Function DeleteUserGroup(ByVal iUserGroupsControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        If iUserGroupsControl = 0 Then
            throwInvalidRequiredKeysException("User Group", "Invalid User Group, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim oGroup As LTS.tblUserGroup = db.tblUserGroups.Where(Function(x) x.UserGroupsControl = iUserGroupsControl).FirstOrDefault()
                If Not oGroup Is Nothing AndAlso oGroup.UserGroupsControl <> 0 Then
                    'Perform validation
                    Try
                        'Cannot delete any of the default groups with controls 1-10
                        If oGroup.UserGroupsControl < 11 Then
                            Dim msg = "Role " & oGroup.UserGroupsName & " is a System Default group and cannot be deleted."
                            Utilities.SaveAppError(msg, Me.Parameters)
                            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DataInUse", .Details = msg}, New FaultReason("E_DataValidationFailure"))
                        End If
                        'Check if this User Group currently has any user accounts associated with it
                        If db.tblUserSecurities.Any(Function(x) x.UserUserGroupsControl = iUserGroupsControl) Then
                            Dim msg = "Role " & oGroup.UserGroupsName & " has users assigned to it and cannot be deleted."
                            Utilities.SaveAppError(msg, Me.Parameters)
                            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DataInUse", .Details = msg}, New FaultReason("E_DataValidationFailure"))
                        End If
                    Catch ex As FaultException
                        Throw
                    Catch ex As InvalidOperationException
                        'do nothing this is the desired result.
                    End Try
                    'Do the Delete
                    db.tblUserGroups.DeleteOnSubmit(oGroup)
                    db.SubmitChanges()
                    blnRet = True
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteUserGroup"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Anytime a user is added to WH/CSR/Carrier Group we have to check the NexTrackOnly checkbox and if removed from either group uncheck it
    ''' </summary>
    ''' <param name="GroupControl"></param>
    ''' <param name="USC"></param>
    Public Sub UpdateNTOnlyBasedOnGroup(ByVal GroupControl As Integer, ByVal USC As Integer)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim res = db.spUpdateNTOnlyBasedOnGroup(GroupControl, USC).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateNTOnlyBasedOnGroup"), db)
            End Try
        End Using
    End Sub

#Region "Form Security By Group"

    ''' <summary>
    ''' Replaces NGLSecurityDataProvider.GetRestrictedFormsByGroup() above
    ''' Used in 365 LEUserFormController/GetByParent
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns></returns>
    Public Function GetRestrictedFormsByGroup(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.vRestrictedFormsByGroup()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vRestrictedFormsByGroup
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.vRestrictedFormsByGroup)
                iQuery = db.vRestrictedFormsByGroups.Where(Function(x) x.UserGroupsControl = filters.ParentControl)
                Dim filterWhere = ""
                'If no sort is added by the user do the default sort
                If filters.SortValues?.Count < 1 OrElse Not filters.SortValues.Any(Function(x) x.sortName.Trim().Length > 0) Then
                    Dim sortArray As Models.SortDetails() = {New Models.SortDetails With {.sortName = "FormName", .sortDirection = "asc"}}
                    filters.SortValues = sortArray
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetRestrictedFormsByGroup"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Replaces NGLSecurityDataProvider.CreateRestrictedFormByGroup() above.
    ''' Creates the form restriction and updates all the dependent users
    ''' </summary>
    ''' <param name="FormControls"></param>
    ''' <param name="GroupControl"></param>
    ''' <remarks>
    ''' Replaces NGLSecurityDataProvider.CreateRestrictedFormByGroup() above
    '''  This version should have better performance because it calls an sp and only accesses the db 1 time
    '''  and does not waste resources returning DTO objects that are not needed by the caller in 365
    '''  Used in 365 LEUserFormController/Post
    ''' Modified by LVV 2/5/20 v-8.2.1.004
    '''  Added logic to sp so associated procedure security is linked to page (Form) security 
    '''  so when Form security is modified the procedure security is modified to match
    ''' Modified by LVV on 2/6/20
    '''  Commented out previous changes to sp. Wasn't supposed to implement this change until v-8.3 oops
    ''' </remarks>
    Public Sub CreateRestrictedFormByGroup(ByVal FormControls() As Integer, ByVal GroupControl As Integer)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                For Each f In FormControls
                    db.spCreateRestrictedFormByGroup(f, GroupControl)
                Next
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("CreateRestrictedFormByGroup"), db)
            End Try
        End Using
    End Sub

#End Region

#Region "Report Security By Group"

    ''' <summary>
    ''' Replaces NGLSecurityDataProvider.GetRestrictedReportsByGroup() above
    ''' Used in 365 LEUserReportController/GetByParent
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns></returns>
    Public Function GetRestrictedReportsByGroup(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.vRestrictedReportsByGroup()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vRestrictedReportsByGroup
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.vRestrictedReportsByGroup)
                iQuery = db.vRestrictedReportsByGroups.Where(Function(x) x.UserGroupsControl = filters.ParentControl)
                Dim filterWhere = ""
                'If no sort is added by the user do the default sort
                If filters.SortValues?.Count < 1 OrElse Not filters.SortValues.Any(Function(x) x.sortName.Trim().Length > 0) Then
                    Dim sortArray As Models.SortDetails() = {New Models.SortDetails With {.sortName = "ReportName", .sortDirection = "asc"}}
                    filters.SortValues = sortArray
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetRestrictedReportsByGroup"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Replaces NGLSecurityDataProvider.CreateRestrictedReportByGroup() above
    ''' This version should have better performance because it calls an sp and only accesses the db 1 time
    ''' and does not waste resources returning DTO objects that are not needed by the caller in 365
    ''' Used in 365 LEUserReportController/Post
    ''' 
    ''' Creates the report restriction and updates all the dependent users
    ''' </summary>
    ''' <param name="ReportControls"></param>
    ''' <param name="GroupControl"></param>
    Public Sub CreateRestrictedReportByGroup(ByVal ReportControls() As Integer, ByVal GroupControl As Integer)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                For Each r In ReportControls
                    db.spCreateRestrictedReportByGroup(r, GroupControl)
                Next
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("CreateRestrictedReportByGroup"), db)
            End Try
        End Using
    End Sub


#End Region

#Region "Procedure Security By Group"

    ''' <summary>
    ''' Replaces NGLSecurityDataProvider.GetRestrictedProceduresByGroup() above
    ''' Used in 365 LEUserProcedureController/GetByParent
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns></returns>
    Public Function GetRestrictedProceduresByGroup(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.vRestrictedProceduresByGroup()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vRestrictedProceduresByGroup
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.vRestrictedProceduresByGroup)
                iQuery = db.vRestrictedProceduresByGroups.Where(Function(x) x.UserGroupsControl = filters.ParentControl)
                Dim filterWhere = ""
                'If no sort is added by the user do the default sort
                If filters.SortValues?.Count < 1 OrElse Not filters.SortValues.Any(Function(x) x.sortName.Trim().Length > 0) Then
                    Dim sortArray As Models.SortDetails() = {New Models.SortDetails With {.sortName = "ProcedureName", .sortDirection = "asc"}}
                    filters.SortValues = sortArray
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetRestrictedProceduresByGroup"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Replaces NGLSecurityDataProvider.CreateRestrictedProcedureByGroup() above
    ''' This version should have better performance because it calls an sp and only accesses the db 1 time
    ''' and does not waste resources returning DTO objects that are not needed by the caller in 365
    ''' Used in 365 LEUserProcedureController/Post
    ''' 
    ''' Creates the procedure restriction and updates all the dependent users
    ''' </summary>
    ''' <param name="ProcedureControls"></param>
    ''' <param name="GroupControl"></param>
    Public Sub CreateRestrictedProcedureByGroup(ByVal ProcedureControls() As Integer, ByVal GroupControl As Integer)
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                For Each p In ProcedureControls
                    db.spCreateRestrictedProcedureByGroup(p, GroupControl)
                Next
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("CreateRestrictedProcedureByGroup"), db)
            End Try
        End Using
    End Sub

#End Region


#Region "tblEnumGroupSecurityType"

    ''' <summary>
    ''' Returns a lookup list of tblEnumGroupSecurityTypes Where vLookupList.Control = EGroupSecTypeBitPosition
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>Created By LVV on 5/20/23 for v-8.2.1.008 - Task #202005191648 Fix Procedure Level Security Deployment Issues</remarks>
    Public Function GetEnumGroupSecurityTypes() As DTO.vLookupList()
        Dim oRet() As DTO.vLookupList
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                oRet = (From r In db.tblEnumGroupSecurityTypes Order By r.EGroupSecTypeDesc Select New DTO.vLookupList With {.Control = r.EGroupSecTypeBitPosition, .Name = r.EGroupSecTypeName, .Description = r.EGroupSecTypeDesc}).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEnumGroupSecurityTypes"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Gets the group data with the security types it belongs to. Used in the security maintenance page
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns></returns>
    ''' <remarks>Created by LVV on 5/27/20 for v-8.2.1.008 - Task #202005191648 Fix Procedure Level Security Deployment Issues</remarks>
    Public Function GetUserGroupsSec365(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.vUserGroupsSec365()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vUserGroupsSec365
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.vUserGroupsSec365)
                iQuery = db.vUserGroupsSec365s
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetUserGroupsSec365"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Saves the security types for the group
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>Created by LVV on 5/28/20 for v-8.2.1.008 - Task #202005191648 Fix Procedure Level Security Deployment Issues</remarks>
    Public Function SaveSecTypesForGroup(ByVal oData As Models.SelectableGridSave) As Boolean
        Dim blnRet As Boolean = False
        If oData Is Nothing Then Return False 'nothing to do
        Dim iGroupControl = oData.Control
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'verify that the Group record exists
                If iGroupControl = 0 Then
                    Dim lDetails As New List(Of String) From {"User Group Reference", " was not provided and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                If Not db.tblUserGroups.Any(Function(x) x.UserGroupsControl = iGroupControl) Then
                    Dim lDetails As New List(Of String) From {"User Group Reference", " was not found and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                Dim bwTmp As New Core.Utility.BitwiseFlags()
                For Each bit In oData.BitPositionsOn
                    bwTmp.turnBitFlagOn(bit)
                Next
                Dim lts = db.tblUserGroups.Where(Function(x) x.UserGroupsControl = iGroupControl).FirstOrDefault()
                lts.UserGroupsSecTypeBits = bwTmp.FlagSource
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveSecTypesForGroup"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Gets the Security Type Configuration for the Group
    ''' </summary>
    ''' <param name="GroupControl"></param>
    ''' <returns></returns>
    ''' <remarks>Created by LVV on 5/28/20 for v-8.2.1.008 - Task #202005191648 Fix Procedure Level Security Deployment Issues</remarks>
    Public Function GetSecTypeConfigForGroup(ByVal GroupControl As Integer) As Models.SelectableGridItem()
        Dim oRet() As Models.SelectableGridItem
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                'verify that the Group record exists
                If GroupControl = 0 Then
                    Dim lDetails As New List(Of String) From {"User Group Reference", " was not provided and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return Nothing
                End If
                If Not db.tblUserGroups.Any(Function(x) x.UserGroupsControl = GroupControl) Then
                    Dim lDetails As New List(Of String) From {"User Group Reference", " was not found and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return Nothing
                End If
                oRet = (From t In db.tblEnumGroupSecurityTypes
                        Order By t.EGroupSecTypeBitPosition
                        Select New Models.SelectableGridItem With {.SGItemBitPos = t.EGroupSecTypeBitPosition, .SGItemCaption = t.EGroupSecTypeDesc, .SGItemOn = False}).ToArray()
                'Get the current integers for the Security Type Config
                Dim flagSource = db.tblUserGroups.Where(Function(x) x.UserGroupsControl = GroupControl).Select(Function(y) y.UserGroupsSecTypeBits).FirstOrDefault()
                Dim bwSecTypes As New Core.Utility.BitwiseFlags(flagSource)
                'Get the current enumns for all selected Security Types
                Dim bitList = bwSecTypes.refreshPositiveBitPositions()
                'If any items in the default return list match the IDs from bitList set those to On
                For Each r In oRet
                    If bitList.Contains(r.SGItemBitPos) Then r.SGItemOn = True
                Next
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetSecTypeConfigForGroup"), db)
            End Try
            Return Nothing
        End Using
    End Function

#End Region


#End Region

#Region "Protected Functions"

#End Region

End Class

'Created By LVV on 5/13/19 - I wanted to use the functionality of the new technology
'There are still old methods above that deal with User Admins which will hopefully be phased out
Public Class NGLUserAdminData : Inherits NGLLinkDataBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        'Dim db As New NGLMASSecurityDataContext(ConnectionString)
        'Me.LinqTable = db.tblUserAdmins
        'Me.LinqDB = db
        Me.SourceClass = "NGLUserAdminData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If (_LinqTable Is Nothing) Then
                Dim db As New NGLMASSecurityDataContext(ConnectionString)
                _LinqTable = db.tblUserAdmins
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

    ''' <summary>
    ''' Replaces NGLSecurityDataProvider.GettblUserAdminsByUser() above
    ''' Caller must pass in UserSecurityControl as AllFilters.ParentControl
    ''' Gets all records in tblUserAdmin by UserControl aka all the restricted companies
    ''' (aka the only companies the user is allowed to access)
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns></returns>
    Public Function GetvUserAdminsByUser(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.vUserAdmin365()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vUserAdmin365
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.vUserAdmin365)
                iQuery = db.vUserAdmin365s.Where(Function(x) x.UserSecurityControl = filters.ParentControl)
                Dim filterWhere = ""
                'If no sort is added by the user do the default sort
                If filters.SortValues?.Count < 1 OrElse Not filters.SortValues.Any(Function(x) x.sortName.Trim().Length > 0) Then
                    Dim sortArray As Models.SortDetails() = {New Models.SortDetails With {.sortName = "CompNumber", .sortDirection = "asc"}}
                    filters.SortValues = sortArray
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetvUserAdminsByUser"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Replaces NGLSecurityDataProvider.DeletetblUserAdmin() above
    ''' </summary>
    ''' <param name="iUserAdminControl"></param>
    ''' <returns></returns>
    Public Function DeleteUserAdmin(ByVal iUserAdminControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        If iUserAdminControl = 0 Then throwInvalidRequiredKeysException("User Admin", "Invalid User Admin, a control number is required and cannot be zero")
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim oAdmin As LTS.tblUserAdmin = db.tblUserAdmins.Where(Function(x) x.UserAdminControl = iUserAdminControl).FirstOrDefault()
                If Not oAdmin Is Nothing AndAlso oAdmin.UserAdminControl <> 0 Then
                    db.tblUserAdmins.DeleteOnSubmit(oAdmin)
                    db.SubmitChanges()
                    blnRet = True
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteUserAdmin"), db)
            End Try
        End Using
        Return blnRet
    End Function

    Public Function InsertUserAdmin(ByVal CompNumbers() As Integer, ByVal USC As Integer) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                For Each compNo In CompNumbers
                    'Create New Record
                    Dim ntblUserAdmin As LTS.tblUserAdmin = New LTS.tblUserAdmin With {.UserSecurityControl = USC, .UserAdminCompControl = compNo}
                    db.tblUserAdmins.InsertOnSubmit(ntblUserAdmin)
                Next
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertUserAdmin"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Replaces NGLSecurityDataProvider.GettblUserAdminsUnrestrictedByUser() above
    ''' Gets the list of companies for the user (standard - Active Comp LE, super - Any Active Comps)
    ''' that are NOT already in tblUserAdmin for this user
    ''' </summary>
    ''' <param name="USC"></param>
    ''' <returns></returns>
    Public Function GetUnrestrictedCompsByUser(ByVal USC As Integer) As LTS.spGetUnrestrictedCompsByUserResult()
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Return db.spGetUnrestrictedCompsByUser(USC).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetUnrestrictedCompsByUser"), db)
            End Try
            Return Nothing
        End Using
    End Function



#End Region

#Region "Protected Functions"

#End Region

End Class

Public Class NGLUserLaneData : Inherits NGLLinkDataBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        'Dim db As New NGLMASSecurityDataContext(ConnectionString)
        'Me.LinqTable = db.tblUserSecurityLanes
        'Me.LinqDB = db
        Me.SourceClass = "NGLUserLaneData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If (_LinqTable Is Nothing) Then
                Dim db As New NGLMASSecurityDataContext(ConnectionString)
                _LinqTable = db.tblUserSecurityLanes
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

    ''' <summary>
    ''' Caller must pass in UserSecurityControl as AllFilters.ParentControl
    ''' Gets all records in tblUserSecurityLane by UserControl aka all the restricted companies
    ''' (aka the only companies the user is allowed to access)
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.4.0.002 on 05/14/2021
    ''' </remarks>
    Public Function GetvUserLanesByUser(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.vUserLane365()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vUserLane365
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.vUserLane365)
                iQuery = db.vUserLane365s.Where(Function(x) x.USLUserSecurityControl = filters.ParentControl)
                Dim filterWhere = ""
                'If no sort is added by the user do the default sort
                If filters.SortValues?.Count < 1 OrElse Not filters.SortValues.Any(Function(x) x.sortName.Trim().Length > 0) Then
                    Dim sortArray As Models.SortDetails() = {New Models.SortDetails With {.sortName = "LaneName", .sortDirection = "asc"}}
                    filters.SortValues = sortArray
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetvUserLanesByUser"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Delete User Lane data
    ''' </summary>
    ''' <param name="iUSLControl"></param>
    ''' <returns></returns>
    Public Function DeleteUserLane(ByVal iUSLControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        If iUSLControl = 0 Then throwInvalidRequiredKeysException("User Lane", "Invalid User Lane, a control number is required and cannot be zero")
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Dim oUserLane As LTS.tblUserSecurityLane = db.tblUserSecurityLanes.Where(Function(x) x.USLControl = iUSLControl).FirstOrDefault()
                If Not oUserLane Is Nothing AndAlso oUserLane.USLControl <> 0 Then
                    db.tblUserSecurityLanes.DeleteOnSubmit(oUserLane)
                    db.SubmitChanges()
                    blnRet = True
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteUserLane"), db)
            End Try
        End Using
        Return blnRet
    End Function

    Public Function InsertUserLane(ByVal LaneControls() As Integer, ByVal USC As Integer) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                For Each iLaneControl In LaneControls
                    'Create New Record
                    Dim ntblUserLane As LTS.tblUserSecurityLane = New LTS.tblUserSecurityLane With {.USLUserSecurityControl = USC, .USLLaneControl = iLaneControl}
                    db.tblUserSecurityLanes.InsertOnSubmit(ntblUserLane)
                Next
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertUserLane"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Gets the list of Lanes for the user 
    ''' that are NOT already in tblUserSecurityLane for this user
    ''' </summary>
    ''' <param name="USC"></param>
    ''' <returns></returns>
    Public Function GetUnrestrictedLanesByUser(ByVal USC As Integer) As LTS.spGetUnrestrictedLanesByUserResult()
        Using db As New NGLMASSecurityDataContext(ConnectionString)
            Try
                Return db.spGetUnrestrictedLanesByUser(USC).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetUnrestrictedLanesByUser"), db)
            End Try
            Return Nothing
        End Using
    End Function



#End Region

#Region "Protected Functions"

#End Region

End Class