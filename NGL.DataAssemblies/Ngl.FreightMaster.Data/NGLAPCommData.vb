Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.ServiceModel

Public Class NGLAPCommData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMasBookDataContext(ConnectionString)
        Me.LinqTable = db.Books
        Me.LinqDB = db
        Me.SourceClass = "NGLAPCommData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMasBookDataContext(ConnectionString)
            Me.LinqTable = db.Books
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
        Return GetAPCommFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    Public Function GetAPCommFiltered(ByVal Control As Integer) As DataTransferObjects.APComm
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oSecureComp = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                'db.Log = New DebugTextWriter
                'Get the AP Commission Book Record
                Dim Result As DataTransferObjects.APComm = (
                        From d In db.Books
                        Where
                        (d.BookControl = Control) _
                        And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.BookCustCompControl))
                        Select New DataTransferObjects.APComm With {.BookControl = d.BookControl _
                        , .BookProNumber = d.BookProNumber _
                        , .BookConsPrefix = d.BookConsPrefix _
                        , .BookCommCompControl = d.BookCommCompControl _
                        , .BookFinARInvoiceDate = d.BookFinARInvoiceDate _
                        , .BookFinARPayAmt = If(d.BookFinARPayAmt.HasValue, d.BookFinARPayAmt, 0) _
                        , .BookFinCommStd = If(d.BookFinCommStd.HasValue, d.BookFinCommStd, 0) _
                        , .BookFinCommAct = If(d.BookFinCommAct.HasValue, d.BookFinCommAct, 0) _
                        , .BookFinCommPayDate = d.BookFinCommPayDate _
                        , .BookFinCommPayAmt = If(d.BookFinCommPayAmt.HasValue, d.BookFinCommPayAmt, 0) _
                        , .BookFinCommtCheck = d.BookFinCommtCheck _
                        , .BookFinCommCreditAmt = If(d.BookFinCommCreditAmt.HasValue, d.BookFinCommCreditAmt, 0) _
                        , .BookFinCommCreditPayDate = d.BookFinCommCreditPayDate _
                        , .BookFinCommLoadCount = If(d.BookFinCommLoadCount.HasValue, d.BookFinCommLoadCount, 0) _
                        , .BookFinCommGLNumber = d.BookFinCommGLNumber _
                        , .BookRevCommCost = If(d.BookRevCommCost.HasValue, d.BookRevCommCost, 0) _
                        , .BookModDate = d.BookModDate _
                        , .BookModUser = d.BookModUser _
                        , .BookUpdated = d.BookUpdated.ToArray
                        }).First()

                Return Result

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

    Public Function GetAPCommsFiltered(ByVal BookCommCompControl As Integer) As DataTransferObjects.APComm()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oSecureComp = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                'db.Log = New DebugTextWriter
                'Get the AP Commission Book Records
                Dim Results() As DataTransferObjects.APComm = (
                        From d In db.Books
                        Where
                        (d.BookCommCompControl = BookCommCompControl) _
                        And
                        d.BookTranCode = "IC" _
                        And
                        d.BookFinARPayDate.HasValue _
                        And
                        d.BookFinAPPayDate.HasValue _
                        And
                        d.BookFinARCheck.Trim.Length > 0 _
                        And
                        d.BookFinAPCheck.Trim.Length > 0 _
                        And
                        (d.BookFinAPPayAmt.HasValue AndAlso d.BookFinAPPayAmt > 0) _
                        And
                        (d.BookRevCommCost.HasValue AndAlso d.BookRevCommCost <> 0) _
                        And
                        (d.BookFinCommPayAmt.HasValue = False OrElse d.BookFinCommPayAmt = 0) _
                        And
                        ((d.BookFinARPayAmt.HasValue AndAlso d.BookFinARPayAmt > 0) _
                         And
                         (d.BookFinARPayAmt >= d.BookFinARInvoiceAmt)) _
                        And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.BookCustCompControl))
                        Order By d.BookFinARInvoiceDate, d.BookProNumber Ascending
                        Select New DataTransferObjects.APComm With {.BookControl = d.BookControl _
                        , .BookProNumber = d.BookProNumber _
                        , .BookConsPrefix = d.BookConsPrefix _
                        , .BookCommCompControl = d.BookCommCompControl _
                        , .BookFinARInvoiceDate = d.BookFinARInvoiceDate _
                        , .BookFinARPayAmt = If(d.BookFinARPayAmt.HasValue, d.BookFinARPayAmt, 0) _
                        , .BookFinCommStd = If(d.BookFinCommStd.HasValue, d.BookFinCommStd, 0) _
                        , .BookFinCommAct = If(d.BookFinCommAct.HasValue, d.BookFinCommAct, 0) _
                        , .BookFinCommPayDate = d.BookFinCommPayDate _
                        , .BookFinCommPayAmt = If(d.BookFinCommPayAmt.HasValue, d.BookFinCommPayAmt, 0) _
                        , .BookFinCommtCheck = d.BookFinCommtCheck _
                        , .BookFinCommCreditAmt = If(d.BookFinCommCreditAmt.HasValue, d.BookFinCommCreditAmt, 0) _
                        , .BookFinCommCreditPayDate = d.BookFinCommCreditPayDate _
                        , .BookFinCommLoadCount = If(d.BookFinCommLoadCount.HasValue, d.BookFinCommLoadCount, 0) _
                        , .BookFinCommGLNumber = d.BookFinCommGLNumber _
                        , .BookRevCommCost = If(d.BookRevCommCost.HasValue, d.BookRevCommCost, 0) _
                        , .BookModDate = d.BookModDate _
                        , .BookModUser = d.BookModUser _
                        , .BookUpdated = d.BookUpdated.ToArray
                        }).Take(2000).ToArray()

                Return Results

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

    ''' <summary> 
    ''' This method overfides the parent function it does not 
    ''' need to Attach the data object because this version of  
    ''' CopyDTOToLinq does not return a detached data object
    ''' </summary>
    ''' <typeparam name="TEntity"></typeparam>
    ''' <param name="oData"></param>
    ''' <param name="oLinqTable"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function UpdateQuick(Of TEntity As Class)(ByVal oData As Object,
                                                               ByVal oLinqTable As System.Data.Linq.Table(Of TEntity)) As DataTransferObjects.QuickSaveResults
        Using LinqDB
            'Note: the ValidateData Function must throw a FaultException error on failure
            ValidateUpdatedRecord(LinqDB, oData)
            Dim nObject = CopyDTOToLinq(oData)
            ' Return the quick results object
            Return GetQuickSaveResults(nObject)
        End Using

    End Function

    ''' <summary>
    ''' This method overfides the parent function it does not 
    ''' need to Attach the data object because this version of  
    ''' CopyDTOToLinq does not return a detached data object
    ''' </summary>
    ''' <typeparam name="TEntity"></typeparam>
    ''' <param name="oData"></param>
    ''' <param name="oLinqTable"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function Update(Of TEntity As Class)(ByVal oData As Object,
                                                          ByVal oLinqTable As System.Data.Linq.Table(Of TEntity)) As Object
        Using LinqDB
            'Note: the ValidateData Function must throw a FaultException error on failure
            ValidateUpdatedRecord(LinqDB, oData)
            Dim nObject = CopyDTOToLinq(oData)
            ' Return the updated order
            Return GetDTOUsingLinqTable(nObject)
        End Using
    End Function

    ''' <summary>
    ''' This method overfides the parent function it does not 
    ''' need to Attach the data object because this version of  
    ''' CopyDTOToLinq does not return a detached data object
    ''' </summary>
    ''' <typeparam name="TEntity"></typeparam>
    ''' <param name="oData"></param>
    ''' <param name="oLinqTable"></param>
    ''' <remarks></remarks>
    Public Overrides Sub UpdateNoReturn(Of TEntity As Class)(ByVal oData As Object,
                                                             ByVal oLinqTable As System.Data.Linq.Table(Of TEntity))
        Using LinqDB
            'Note: the ValidateData Function must throw a FaultException error on failure
            ValidateUpdatedRecord(LinqDB, oData)
            Dim nObject = CopyDTOToLinq(oData)
            NoReturnCleanUp(nObject)
        End Using
    End Sub

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = TryCast(oData, DataTransferObjects.APComm)
        'Create Existing Record
        Dim oLTS As New LTS.Book
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                If d Is Nothing OrElse d.BookControl = 0 Then
                    Utilities.SaveAppError("Cannot process data because the source record is empty or has an invalid control number", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = "Cannot process data because the source record is empty or has an invalid control number"}, New FaultReason("E_InvalidOperationException"))
                End If
                Dim source = (From b In db.Books
                        Where b.BookControl = d.BookControl
                        Select b).First()

                If source Is Nothing Then
                    Utilities.SaveAppError("Cannot process data because the source record cannot be found in the database", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = "Cannot process data because the source record cannot be found in the database"}, New FaultReason("E_InvalidOperationException"))
                End If

                Dim skipObjs As New List(Of String) From {"BookItemModDate",
                        "BookItemModUser",
                        "BookItemUpdated"}
                oLTS = CopyMatchingFields(oLTS, source, skipObjs)

                'Dim oBook As New LTS.Book
                'Dim type As Type = source.GetType()
                'Dim properties As System.Reflection.PropertyInfo() = type.GetProperties()
                'For Each p As System.Reflection.PropertyInfo In properties
                '    p.SetValue(oBook, p.GetValue(source, Nothing), Nothing)
                'Next



                With oLTS
                    .BookConsPrefix = d.BookConsPrefix
                    .BookCommCompControl = d.BookCommCompControl
                    .BookFinARInvoiceDate = d.BookFinARInvoiceDate
                    .BookFinARPayAmt = d.BookFinARPayAmt
                    .BookFinCommStd = d.BookFinCommStd
                    .BookFinCommAct = d.BookFinCommAct
                    .BookFinCommPayDate = d.BookFinCommPayDate
                    .BookFinCommPayAmt = d.BookFinCommPayAmt
                    .BookFinCommtCheck = d.BookFinCommtCheck
                    .BookFinCommCreditAmt = d.BookFinCommCreditAmt
                    .BookFinCommCreditPayDate = d.BookFinCommCreditPayDate
                    .BookFinCommLoadCount = d.BookFinCommLoadCount
                    .BookFinCommGLNumber = d.BookFinCommGLNumber
                    .BookRevCommCost = d.BookRevCommCost
                    .BookModDate = Date.Now
                    .BookModUser = Me.Parameters.UserName
                    .BookUpdated = If(d.BookUpdated Is Nothing, New Byte() {}, d.BookUpdated)
                End With
                db.Books.Attach(oLTS, True)
                Try
                    db.SubmitChanges()
                Catch ex As SqlException
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                Catch conflictEx As ChangeConflictException
                    Try
                        'improper reference to LinqDB 
                        'changed to db by RHR 2/6/15
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

                Return oLTS



            Catch ex As FaultException
                Throw
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

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetAPCommFiltered(Control:=CType(LinqTable, LTS.Book).BookControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim source As LTS.Book = TryCast(LinqTable, LTS.Book)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.Books
                    Where d.BookControl = source.BookControl
                    Select New DataTransferObjects.QuickSaveResults With {.Control = d.BookControl _
                        , .ModDate = d.BookModDate _
                        , .ModUser = d.BookModUser _
                        , .Updated = d.BookUpdated.ToArray}).First

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

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'We do not allow booking records to be inserted from this class
        Utilities.SaveAppError("Cannot add data.  Records cannot be inserted using this interface!", Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))
    End Sub

    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'We do not allow booking records to be deleted from this class
        Utilities.SaveAppError("Cannot delete data.  Records cannot be deleted using this interface!", Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))

    End Sub

#End Region

End Class