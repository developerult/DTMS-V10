Imports System.ServiceModel

Public Class NGLCarrierProNumberBlockData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMASCarrierDataContext(ConnectionString)
        Me.LinqTable = db.CarrierProNumberBlocks
        Me.LinqDB = db
        Me.SourceClass = "NGLCarrierProNumberBlockData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then


                Dim db As New NGLMASCarrierDataContext(ConnectionString)
                Me.LinqTable = db.CarrierProNumberBlocks
                Me.LinqDB = db
            End If
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"

#Region " Overridden data methods"


#End Region

    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return GetCarrierProNumberBlockFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetCarrierProNumberBlocksFiltered(0)
    End Function

    Public Function GetCarrierProNumberBlockFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.CarrierProNumberBlock
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim CarrierProNumberBlock As DataTransferObjects.CarrierProNumberBlock = (
                        From d In db.CarrierProNumberBlocks
                        Where
                        (d.CarrProNbrBlockControl = If(Control = 0, d.CarrProNbrBlockControl, Control))
                        Order By d.CarrProNbrBlockCarrierControl Descending
                        Select selectDTOData(d, db)).FirstOrDefault()
                Return CarrierProNumberBlock

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrierProNumberBlockFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetCarrierProNumberBlocksFiltered(ByVal CarrProControl As Integer,
                                                      Optional ByVal AvailableOnly As Boolean = True,
                                                      Optional ByVal page As Integer = 1,
                                                      Optional ByVal pagesize As Integer = 1000) As DataTransferObjects.CarrierProNumberBlock()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                Dim intPageCount As Integer = 1
                If pagesize < 1 Then pagesize = 1
                If page < 1 Then page = 1
                Dim intSkip As Integer = (page - 1) * pagesize
                Dim intRecordCount As Integer = db.CarrierProNumberBlocks.Where(Function(x) x.CarrProNbrBlockCarrProControl = CarrProControl And (AvailableOnly = False OrElse x.CarrProNbrBlockAvailable = True)).Count()
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                'Return all the contacts that match the criteria sorted by name
                Dim CarrierProNumberBlocks() As DataTransferObjects.CarrierProNumberBlock = (
                        From d In db.CarrierProNumberBlocks
                        Where
                        (d.CarrProNbrBlockCarrProControl = CarrProControl) _
                        And
                        (AvailableOnly = False OrElse d.CarrProNbrBlockAvailable = True)
                        Order By d.CarrProNbrBlockSortOrder
                        Select selectDTOData(d, db, page, intPageCount, intRecordCount, pagesize)).Skip(intSkip).Take(pagesize).ToArray()
                Return CarrierProNumberBlocks

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrierProNumberBlocksFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DataTransferObjects.CarrierProNumberBlock)
        Return selectLTSData(d, Me.Parameters.UserName)

    End Function

    Protected Function CompareUpdatedWithDB(ByVal oData As DataTransferObjects.CarrierProNumberBlock) As Boolean
        Dim DBCarrierProNumberBlockUpdated As Byte()
        Dim db As NGLMASCarrierDataContext = CType(Me.LinqDB, NGLMASCarrierDataContext)
        Dim intControl As Integer = oData.CarrProNbrBlockControl
        DBCarrierProNumberBlockUpdated = (From d In db.CarrierProNumberBlocks Where d.CarrProNbrBlockControl = intControl Select d.CarrProNbrBlockUpdated).FirstOrDefault().ToArray()
        Return StructuralComparisons.StructuralEqualityComparer.Equals(DBCarrierProNumberBlockUpdated, oData.CarrProNbrBlockUpdated)
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Dim oData As LTS.CarrierProNumberBlock = TryCast(LinqTable, LTS.CarrierProNumberBlock)
        If oData Is Nothing Then Return Nothing
        Return GetCarrierProNumberBlockFiltered(Control:=oData.CarrProNbrBlockControl)
    End Function

    Public Function QuickSaveResults(ByVal CarrProNbrBlockControl As Integer) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                ret = (From d In db.CarrierProNumberBlocks
                    Where d.CarrProNbrBlockControl = CarrProNbrBlockControl
                    Select New DataTransferObjects.QuickSaveResults With {.Control = d.CarrProNbrBlockControl _
                        , .ModDate = d.CarrProNbrBlockModDate _
                        , .ModUser = d.CarrProNbrBlockModUser _
                        , .Updated = d.CarrProNbrBlockUpdated.ToArray()}).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("QuickSaveResults"))
            End Try
        End Using
        Return ret
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim source As LTS.CarrierProNumberBlock = TryCast(LinqTable, LTS.CarrierProNumberBlock)
        If source Is Nothing Then Return Nothing
        Return QuickSaveResults(source.CarrProNbrBlockControl)
    End Function

    Friend Shared Function selectDTOData(ByVal d As LTS.CarrierProNumberBlock, ByRef db As NGLMASCarrierDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.CarrierProNumberBlock

        Dim oDTO As New DataTransferObjects.CarrierProNumberBlock

        Dim skipObjs As New List(Of String) From {"CarrProNbrBlockUpdated",
                "Page",
                "Pages",
                "RecordCount",
                "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .CarrProNbrBlockUpdated = d.CarrProNbrBlockUpdated.ToArray()
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
    Friend Shared Function selectLTSData(ByVal d As DataTransferObjects.CarrierProNumberBlock, ByVal UserName As String) As LTS.CarrierProNumberBlock
        ' If Updated is null, set to byte[0] (for inserted items).
        Dim oLTS As New LTS.CarrierProNumberBlock
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
    Friend Shared Sub UpdateLTSWithDTO(ByRef d As DataTransferObjects.CarrierProNumberBlock, ByRef t As LTS.CarrierProNumberBlock, ByVal UserName As String)
        Dim blnNewLTS As Boolean = False 'used to determine if we allow the CarrierProNumberBlockData to be set,  existing LTS objects cannot update the CarrierProNumberBlockUpdated value.
        If t.CarrProNbrBlockControl = 0 Then blnNewLTS = True 'in this case we use a new Byte or the current value in d
        Dim strMSG As String = ""
        Dim skipObjs As New List(Of String) From {"CarrProNbrBlockModDate", "CarrProNbrBlockModUser", "CarrProNbrBlockUpdated"}
        t = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(t, d, skipObjs, strMSG)
        't = CopyMatchingFields(t, d, skipObjs)
        With t
            .CarrProNbrBlockModDate = Date.Now
            .CarrProNbrBlockModUser = UserName
            If blnNewLTS Then .CarrProNbrBlockUpdated = If(d.CarrProNbrBlockUpdated Is Nothing, New Byte() {}, d.CarrProNbrBlockUpdated)
        End With
        If Debugger.IsAttached AndAlso Not String.IsNullOrWhiteSpace(strMSG) Then
            System.Diagnostics.Debug.WriteLine(strMSG)
        End If
    End Sub

    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'We do not allow booking records to be deleted from this class
        Utilities.SaveAppError("Cannot delete data.  Records cannot be deleted using this interface!", Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))

    End Sub

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        If oData Is Nothing Then Return 'if no data the caller will manage any exceptions
        With CType(oData, DataTransferObjects.CarrierProNumberBlock)
            Dim strBadKeyFields As New List(Of String)
            Dim intProControl = .CarrProNbrBlockCarrProControl
            If intProControl = 0 Then strBadKeyFields.Add("Pro Number Control")
            Dim intCarrierControl = .CarrProNbrBlockCarrierControl
            If intCarrierControl = 0 Then strBadKeyFields.Add("Carrier Control")
            Dim strBlockNumber As String = .CarrProNbrBlockNumber
            If String.IsNullOrWhiteSpace(strBlockNumber) Then strBadKeyFields.Add("Block Number")
            Dim blnFKsValid As Boolean = True
            If intProControl <> 0 Then
                If Not CType(oDB, NGLMASCarrierDataContext).CarrierProNumbers.Any(Function(x) x.CarrProControl = intProControl) Then strBadKeyFields.Add("Pro Number Control")
                blnFKsValid = False
            End If
            If intCarrierControl <> 0 Then
                If Not CType(oDB, NGLMASCarrierDataContext).Carriers.Any(Function(x) x.CarrierControl = intCarrierControl) Then strBadKeyFields.Add("Carrier Control")
                blnFKsValid = False
            End If
            If blnFKsValid And Not String.IsNullOrWhiteSpace(strBlockNumber) Then
                If CType(oDB, NGLMASCarrierDataContext).CarrierProNumberBlocks.Any(Function(x) x.CarrProNbrBlockCarrProControl = intProControl And x.CarrProNbrBlockCarrierControl = intCarrierControl And x.CarrProNbrBlockNumber = strBlockNumber) Then
                    throwInvalidKeyAlreadyExistsException("Carrier_Pro_Numbe_Block", "Block Number", strBlockNumber)
                    Return
                End If
            End If

            If Not strBadKeyFields Is Nothing AndAlso strBadKeyFields.Count() > 0 Then
                'Cannot save changes to {0}.  The following key fields are required: {1}. 
                Dim lDetails As New List(Of String) From {"Carrier_Pro_Numbe_Block", String.Join(" | ", strBadKeyFields)}
                throwInvalidKeyFaultException(SqlFaultInfo.FaultDetailsKey.E_CannotSaveKeyFieldsRequired, lDetails)
                Return
            End If
            'if we get here we are good so update the date added key
            .CarrProNbrBlockAddedDate = Date.Now()
        End With
    End Sub


    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'validate the key fields
        If oData Is Nothing Then Return 'if no data the caller will manage any exceptions
        With CType(oData, DataTransferObjects.CarrierProNumberBlock)
            Dim strBadKeyFields As New List(Of String)
            Dim intProControl = .CarrProNbrBlockCarrProControl
            If intProControl = 0 Then strBadKeyFields.Add("Pro Number Control")
            Dim intCarrierControl = .CarrProNbrBlockCarrierControl
            If intCarrierControl = 0 Then strBadKeyFields.Add("Carrier Control")
            Dim strBlockNumber As String = .CarrProNbrBlockNumber
            If String.IsNullOrWhiteSpace(strBlockNumber) Then strBadKeyFields.Add("Block Number")
            Dim intProBlockControl = .CarrProNbrBlockControl
            If intProBlockControl = 0 Then
                throwInvalidKeyAlreadyExistsException("Carrier_Pro_Numbe_Block", "Block Control Number", intProBlockControl)
                Return
            End If
            Dim blnFKsValid As Boolean = True
            If intProControl <> 0 Then
                If Not CType(oDB, NGLMASCarrierDataContext).CarrierProNumbers.Any(Function(x) x.CarrProControl = intProControl) Then strBadKeyFields.Add("Pro Number Control")
                blnFKsValid = False
            End If
            If intCarrierControl <> 0 Then
                If Not CType(oDB, NGLMASCarrierDataContext).Carriers.Any(Function(x) x.CarrierControl = intCarrierControl) Then strBadKeyFields.Add("Carrier Control")
                blnFKsValid = False
            End If
            If blnFKsValid And Not String.IsNullOrWhiteSpace(strBlockNumber) Then
                If CType(oDB, NGLMASCarrierDataContext).CarrierProNumberBlocks.Any(Function(x) x.CarrProNbrBlockCarrProControl = intProControl And x.CarrProNbrBlockCarrierControl = intCarrierControl And x.CarrProNbrBlockNumber = strBlockNumber And x.CarrProNbrBlockControl <> intProBlockControl) Then
                    throwInvalidKeyAlreadyExistsException("Carrier_Pro_Numbe_Block", "Block Number", strBlockNumber)
                    Return
                End If
            End If

            If Not strBadKeyFields Is Nothing AndAlso strBadKeyFields.Count() > 0 Then
                'Cannot save changes to {0}.  The following key fields are required: {1}. 
                Dim lDetails As New List(Of String) From {"Carrier_Pro_Numbe_Block", String.Join(" | ", strBadKeyFields)}
                throwInvalidKeyFaultException(SqlFaultInfo.FaultDetailsKey.E_CannotSaveKeyFieldsRequired, lDetails)
                Return
            End If
        End With
    End Sub


#End Region

End Class