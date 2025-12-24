Imports System.ServiceModel

Public Class NGLvBookConsData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        'Dim db As New NGLMasBookDataContext(ConnectionString)
        'Me.LinqTable = db.vBookConsSubForms
        'Me.LinqDB = db
        Me.SourceClass = "NGLvBookConsData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMasBookDataContext(ConnectionString)
                _LinqTable = db.vBookConsSubForms
                _LinqDB = db
            End If

            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

    Private _BookDependencyResult As LTS.spUpdateBookDependenciesResult
    Public Property BookDependencyResult() As LTS.spUpdateBookDependenciesResult
        Get
            Return _BookDependencyResult
        End Get
        Set(ByVal value As LTS.spUpdateBookDependenciesResult)
            _BookDependencyResult = value
        End Set
    End Property

    Private _LastProcedureName As String
    Public Property LastProcedureName() As String
        Get
            Return _LastProcedureName
        End Get
        Set(ByVal value As String)
            _LastProcedureName = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return GetvBookConsRecord(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    Public Function GetvBookConsRecord(ByVal Control As Integer) As DataTransferObjects.vBookCons
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Return (
                    From d In db.vBookConsSubForms
                        Where
                            d.BookControl = Control
                        Select selectDTOData(d, db)).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetvBookConsRecord"))
            End Try
            Return Nothing
        End Using
    End Function

    Public Function GetvBookConsFilteredContains(ByVal strBookConsPrefix As String) As DataTransferObjects.vBookCons()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oSecureComp = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                'convert any * to %
                If Not String.IsNullOrEmpty(strBookConsPrefix) Then strBookConsPrefix = strBookConsPrefix.Replace("*", "%")
                'Get the newest record that matches the provided criteria
                Return (
                    From d In db.vBookConsSubForms
                        Where
                            System.Data.Linq.SqlClient.SqlMethods.Like(d.BookConsPrefix, strBookConsPrefix) _
                            And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.BookCustCompControl))
                        Order By
                            d.BookConsPrefix, d.BookStopNo, d.BookProNumber
                        Select selectDTOData(d, db)).Take(100).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetvBookConsFilteredContains"))
            End Try
            Return Nothing
        End Using
    End Function

    Public Function GetvBookConsFiltered(ByVal strBookConsPrefix As String) As DataTransferObjects.vBookCons()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Return (
                    From d In db.vBookConsSubForms
                        Where
                            d.BookConsPrefix = strBookConsPrefix
                        Order By
                            d.BookConsPrefix, d.BookStopNo, d.BookProNumber
                        Select selectDTOData(d, db)).Take(100).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetvBookConsFiltered"))
            End Try
            Return Nothing
        End Using
    End Function

    Public Overrides Function Update(Of TEntity As Class)(ByVal oData As Object,
                                                          ByVal oLinqTable As System.Data.Linq.Table(Of TEntity)) As Object
        SaveChanges(oData)
        Dim source As DataTransferObjects.vBookCons = TryCast(oData, DataTransferObjects.vBookCons)
        If source Is Nothing Then Return Nothing
        Me.BookDependencyResult = DirectCast(Me.NDPBaseClassFactory("NGLBookData", False), NGLBookData).UpdateBookDependencies(source.BookControl, 0)
        Me.LastProcedureName = "spUpdateBookDependencies"
        Return Nothing
    End Function

    Public Overrides Function UpdateQuick(Of TEntity As Class)(ByVal oData As Object,
                                                               ByVal oLinqTable As System.Data.Linq.Table(Of TEntity)) As DataTransferObjects.QuickSaveResults
        SaveChanges(oData)
        Dim source As DataTransferObjects.vBookCons = TryCast(oData, DataTransferObjects.vBookCons)
        If source Is Nothing Then Return Nothing
        Me.BookDependencyResult = DirectCast(Me.NDPBaseClassFactory("NGLBookData", False), NGLBookData).UpdateBookDependencies(source.BookControl, 0)
        Me.LastProcedureName = "spUpdateBookDependencies"
        Return Nothing

    End Function

    Public Overrides Sub UpdateNoReturn(Of TEntity As Class)(ByVal oData As Object,
                                                             ByVal oLinqTable As System.Data.Linq.Table(Of TEntity))
        SaveChanges(oData)
        Dim source As DataTransferObjects.vBookCons = TryCast(oData, DataTransferObjects.vBookCons)
        If source Is Nothing Then Return
        Me.BookDependencyResult = DirectCast(Me.NDPBaseClassFactory("NGLBookData", False), NGLBookData).UpdateBookDependencies(source.BookControl, 0)
        Me.LastProcedureName = "spUpdateBookDependencies"
    End Sub

    Public Overrides Sub Delete(Of TEntity As Class)(ByVal oData As Object,
                                                     ByVal oLinqTable As System.Data.Linq.Table(Of TEntity))
        Using LinqDB
            'Note: the ValidateData Function must throw a FaultException error on failure
            ValidateDeletedRecord(LinqDB, oData)
            Try
                With CType(oData, DataTransferObjects.BookLoadDetail)

                    'Open the existing Record
                    Dim nObject As LTS.vBookConsSubForm = (From e In CType(LinqDB, NGLMasBookDataContext).vBookConsSubForms Where e.BookControl = .BookControl Select e).First
                    If nObject Is Nothing Then
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_RecordDeleted"}, New FaultReason("E_InvalidOperationException"))
                    Else
                        CType(LinqDB, NGLMasBookDataContext).vBookConsSubForms.DeleteOnSubmit(nObject)
                        LinqDB.SubmitChanges()
                    End If

                End With
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("Delete"))
            End Try
        End Using
    End Sub


#End Region

#Region "Shared Methods"

    Friend Shared Function selectDTOData(ByVal d As LTS.vBookConsSubForm, ByRef db As NGLMasBookDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.vBookCons
        Dim oDTO As New DataTransferObjects.vBookCons
        Dim skipObjs As New List(Of String) From {"LaneNumber",
                "LaneName",
                "LaneBenchMiles",
                "CarrierName",
                "CompanyName",
                "CompanyNumber",
                "CompFinUseImportFrtCost",
                "BookUpdated",
                "Page",
                "Pages",
                "RecordCount",
                "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .LaneNumber = d.LaneNumber
            .LaneName = d.LaneName
            .LaneBenchMiles = d.LaneBenchMiles
            .CarrierName = d.CarrierName
            .CompanyName = d.CompanyName
            .CompanyNumber = d.CompanyNumber
            .CompFinUseImportFrtCost = d.CompFinUseImportFrtCost
            .BookUpdated = d.BookUpdated.ToArray()
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oDTO
    End Function

    Friend Shared Sub updateLTSWithDTO(ByRef d As DataTransferObjects.vBookCons, ByRef l As LTS.vBookConsSubForm, ByVal UserName As String)

        Dim skipObjs As New List(Of String) From {"BookControl",
                "LaneNumber",
                "LaneName",
                "LaneBenchMiles",
                "CarrierName",
                "CompanyName",
                "CompanyNumber",
                "CompFinUseImportFrtCost",
                "BookModDate",
                "BookModUser",
                "BookUpdated",
                "Page",
                "Pages",
                "RecordCount",
                "PageSize"}
        l = CopyMatchingFields(l, d, skipObjs)
        'add custom formatting
        With l
            .BookModDate = Date.Now()
            .BookModUser = UserName
            .BookUpdated = If(d.BookUpdated Is Nothing, New Byte() {}, d.BookUpdated)
        End With
    End Sub

    Friend Shared Function selectLTSData(ByVal d As DataTransferObjects.vBookCons, ByVal UserName As String) As LTS.vBookConsSubForm
        Dim oLTS As New LTS.vBookConsSubForm
        updateLTSWithDTO(d, oLTS, UserName)
        Return oLTS
    End Function

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DataTransferObjects.vBookCons)
        Return selectLTSData(d, Me.Parameters.UserName)
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Dim source As LTS.vBookConsSubForm = TryCast(LinqTable, LTS.vBookConsSubForm)
        If source Is Nothing Then Return Nothing
        Return GetRecordFiltered(Control:=source.BookControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim source As DataTransferObjects.vBookCons = TryCast(LinqTable, DataTransferObjects.vBookCons)
        If source Is Nothing Then Return Nothing
        Dim ret As DataTransferObjects.QuickSaveResults = DirectCast(Me.NDPBaseClassFactory("NGLBookData", False), NGLBookData).QuickSaveResults(source.BookControl)
        Return ret
    End Function

    Private Sub SaveChanges(ByVal oData As DataTransferObjects.vBookCons)
        Dim blnHasStopNbrChanged As Boolean = False
        If Not oData Is Nothing Then
            Dim strCNS As String = oData.BookConsPrefix
            Using LinqDB
                'Note: the ValidateData Function must throw a FaultException error on failure
                ValidateUpdatedRecord(LinqDB, oData)
                Try
                    'Open the existing Record
                    Dim d = (From e In CType(LinqDB, NGLMasBookDataContext).vBookConsSubForms Where e.BookControl = oData.BookControl Select e).FirstOrDefault()
                    If d Is Nothing OrElse d.BookControl = 0 Then
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_RecordDeleted"}, New FaultReason("E_InvalidOperationException"))
                    Else
                        'Check for conflicts
                        'Modified by RHR for v-8.5.2.006 on 12/29/2022 check for seconds because D365 does not save milliseconds
                        Dim iSeconds = DateDiff(DateInterval.Second, oData.BookModDate.Value, d.BookModDate.Value)
                        If iSeconds > 0 Then
                            'the data may have changed so check each field for conflicts
                            'Modified by RHR 10/8/14 we now use reflection via CheckForDataConflicts 
                            Dim ConflictData As New List(Of KeyValuePair(Of String, String))
                            Dim oSkip As New List(Of String) From {"BookControl",
                                    "LaneNumber",
                                    "LaneName",
                                    "LaneBenchMiles",
                                    "CarrierName",
                                    "CompanyName",
                                    "CompanyNumber",
                                    "CompFinUseImportFrtCost",
                                    "BookModDate",
                                    "BookModUser",
                                    "BookUpdated"}
                            Dim blnConflictFound As Boolean = CheckForDataConflicts(oData, d, oSkip, ConflictData)
                            If blnConflictFound Then
                                'We only add the mod date and mod user if one or more other fields have been modified
                                addToConflicts("BookModDate", oData.BookModDate, d.BookModDate, ConflictData, blnConflictFound)
                                addToConflicts("BookModUser", oData.BookModUser, d.BookModUser, ConflictData, blnConflictFound)
                                Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(ConflictData))
                                conflictInfo.LogConflictDetails(conflictInfo.Message, Me.Parameters)
                                Throw New FaultException(Of ConflictFaultInfo)(conflictInfo, New FaultReason("E_DataConflict"))
                            End If
                        End If
                        'Update the table data
                        updateLTSWithDTO(oData, d, Me.Parameters.UserName)
                    End If
                    LinqDB.SubmitChanges()
                Catch ex As Exception
                    ManageLinqDataExceptions(ex, buildProcedureName("SaveChanges"))
                End Try
            End Using
        End If

    End Sub

#End Region

End Class