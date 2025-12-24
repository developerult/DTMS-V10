Imports System.ServiceModel

Public Class NGLBookNoteData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        'Dim db As New NGLMasBookDataContext(ConnectionString)
        'Me.LinqTable = db.BookNotes
        'Me.LinqDB = db
        Me.SourceClass = "NGLBookNoteData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMasBookDataContext(ConnectionString)
                _LinqTable = db.BookNotes
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
        Return GetBookNoteFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetBookNotesFiltered()
    End Function

    Public Function GetBookNoteFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.BookNote
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim BookNote As DataTransferObjects.BookNote = (
                        From d In db.BookNotes
                        Where
                        (d.BookNotesControl = If(Control = 0, d.BookNotesControl, Control))
                        Order By d.BookNotesControl Descending
                        Select selectDTOData(d)).First
                Return BookNote

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetBookNotesFiltered(Optional ByVal BookControl As Integer = 0) As DataTransferObjects.BookNote()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim BookNotes() As DataTransferObjects.BookNote = (
                        From d In db.BookNotes
                        Where
                        (d.BookNotesBookControl = If(BookControl = 0, d.BookNotesBookControl, BookControl))
                        Order By d.BookNotesControl
                        Select selectDTOData(d)).ToArray()
                Return BookNotes

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

#Region "365 Methods"

    ''' <summary>
    ''' Returns an array of BookNotes associated with the provided BookControl
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <returns></returns>
    ''' <remarks>Added By LVV on 6/23/20 for v-8.2.1.008 Task #20200609162832 - Create Book Notes page and add Navigation item to the Load Board</remarks>
    Public Function GetvBookNotes(ByVal BookControl As Integer) As LTS.vBookNote()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                If BookControl = 0 OrElse Not db.Books.Any(Function(x) x.BookControl = BookControl) Then throwNoDataFaultMessage("E_InvalidRecordKey") 'we do not have a valid filter - "The reference to the record is missing. Please select another record and try again."
                Return db.vBookNotes.Where(Function(x) x.BookNotesBookControl = BookControl).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetvBookNotes"), db)
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Inserts or Updates a BookNote record
    ''' </summary>
    ''' <param name="oRecord"></param>
    ''' <returns></returns>
    ''' <remarks>Added By LVV on 6/23/20 for v-8.2.1.008 Task #20200609162832 - Create Book Notes page and add Navigation item to the Load Board</remarks>
    Public Function InsertOrUpdateBookNote(ByVal oRecord As LTS.BookNote) As Boolean
        Dim blnRet = False
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                oRecord.BookNotesModDate = Date.Now
                oRecord.BookNotesModUser = Parameters.UserName
                If oRecord.BookNotesControl = 0 Then
                    'Insert
                    db.BookNotes.InsertOnSubmit(oRecord)
                Else
                    'Update
                    db.BookNotes.Attach(oRecord, True)
                End If
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertOrUpdateBookNote"), db)
            End Try
        End Using
        Return blnRet
    End Function

#End Region

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DataTransferObjects.BookNote)
        'Create New Record
        Return New LTS.BookNote With {.BookNotesControl = d.BookNotesControl _
            , .BookNotesBookControl = d.BookNotesBookControl _
            , .BookNotesVisable1 = d.BookNotesVisable1 _
            , .BookNotesVisable2 = d.BookNotesVisable2 _
            , .BookNotesVisable3 = d.BookNotesVisable3 _
            , .BookNotesVisable4 = d.BookNotesVisable4 _
            , .BookNotesVisable5 = d.BookNotesVisable5 _
            , .BookNotesConfidential1 = d.BookNotesConfidential1 _
            , .BookNotesConfidential2 = d.BookNotesConfidential2 _
            , .BookNotesConfidential3 = d.BookNotesConfidential3 _
            , .BookNotesConfidential4 = d.BookNotesConfidential4 _
            , .BookNotesConfidential5 = d.BookNotesConfidential5 _
            , .BookNotesBookUser1 = d.BookNotesBookUser1 _
            , .BookNotesBookUser2 = d.BookNotesBookUser2 _
            , .BookNotesBookUser3 = d.BookNotesBookUser3 _
            , .BookNotesBookUser4 = d.BookNotesBookUser4 _
            , .BookNotesModDate = Date.Now _
            , .BookNotesModUser = Parameters.UserName _
            , .BookNotesUpdated = If(d.BookNotesUpdated Is Nothing, New Byte() {}, d.BookNotesUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetBookNoteFiltered(Control:=CType(LinqTable, LTS.BookNote).BookNotesControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim source As LTS.BookNote = TryCast(LinqTable, LTS.BookNote)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.BookNotes
                    Where d.BookNotesControl = source.BookNotesControl
                    Select New DataTransferObjects.QuickSaveResults With {.Control = d.BookNotesControl _
                        , .ModDate = d.BookNotesModDate _
                        , .ModUser = d.BookNotesModUser _
                        , .Updated = d.BookNotesUpdated.ToArray}).First

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

        End Using
        Return ret
    End Function

    Friend Shared Function selectDTOData(ByVal d As LTS.BookNote, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.BookNote

        Dim oDTO As New DataTransferObjects.BookNote
        Dim skipObjs As New List(Of String) From {"BookNotesUpdated",
                "Page",
                "Pages",
                "RecordCount",
                "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .BookNotesUpdated = d.BookNotesUpdated.ToArray()
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oDTO

    End Function

#End Region

End Class