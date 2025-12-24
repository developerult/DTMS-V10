Imports System.ServiceModel

Public Class NGLvARMassEntryData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMasBookDataContext(ConnectionString)
        Me.LinqTable = db.vARMassEntries
        Me.LinqDB = db
        Me.SourceClass = "NGLvARMassEntryData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMasBookDataContext(ConnectionString)
            Me.LinqTable = db.vARMassEntries
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
        Return GetvARMassEntryFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    Public Function GetvARMassEntryFiltered(ByVal Control As Integer) As DataTransferObjects.vARMassEntry
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oSecureComp = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl

                'Get the newest record that matches the provided criteria
                Dim ovARMassEntry As DataTransferObjects.vARMassEntry = (
                        From d In db.vARMassEntries
                        Where
                        d.BookControl = Control _
                        And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.BookCustCompControl))
                        Select New DataTransferObjects.vARMassEntry With {.BookControl = d.BookControl _
                        , .BookCustCompControl = d.BookCustCompControl _
                        , .BookFinARGLNumber = d.BookFinARGLNumber _
                        , .BookPayCode = d.BookPayCode _
                        , .BookTypeCode = d.BookTypeCode _
                        , .Check_No = d.Check_No _
                        , .Cons_No = d.Cons_No _
                        , .CurBal = d.CurBal _
                        , .DaysOut = d.DaysOut _
                        , .Invoice_Amt = d.Invoice_Amt _
                        , .Invoiced = d.Invoiced _
                        , .Pay_Amt = d.Pay_Amt _
                        , .Payed = d.Payed _
                        , .Pro_Number = d.Pro_Number _
                        , .BookUpdated = d.BookUpdated.ToArray()}).First


                Return ovARMassEntry

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

    Public Function GetvARMassEntriesFiltered(ByVal CompControl As Integer) As DataTransferObjects.vARMassEntry()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oSecureComp = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName Select Control = s.CompControl

                'Get the newest record that matches the provided criteria
                Dim ovARMassEntry As DataTransferObjects.vARMassEntry() = (
                        From d In db.vARMassEntries
                        Where
                        (d.BookCustCompControl = CompControl) _
                        And
                        (d.Pay_Amt = 0) _
                        And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.BookCustCompControl))
                        Select New DataTransferObjects.vARMassEntry With {.BookControl = d.BookControl _
                        , .BookCustCompControl = d.BookCustCompControl _
                        , .BookFinARGLNumber = d.BookFinARGLNumber _
                        , .BookPayCode = d.BookPayCode _
                        , .BookTypeCode = d.BookTypeCode _
                        , .Check_No = d.Check_No _
                        , .Cons_No = d.Cons_No _
                        , .CurBal = d.CurBal _
                        , .DaysOut = d.DaysOut _
                        , .Invoice_Amt = d.Invoice_Amt _
                        , .Invoiced = d.Invoiced _
                        , .Pay_Amt = d.Pay_Amt _
                        , .Payed = d.Payed _
                        , .Pro_Number = d.Pro_Number _
                        , .BookUpdated = d.BookUpdated.ToArray()}).ToArray


                Return ovARMassEntry

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

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DataTransferObjects.vARMassEntry)
        'Create New Record
        Return New LTS.vARMassEntry With {.BookControl = d.BookControl _
            , .BookCustCompControl = d.BookCustCompControl _
            , .BookFinARGLNumber = d.BookFinARGLNumber _
            , .BookPayCode = d.BookPayCode _
            , .BookTypeCode = d.BookTypeCode _
            , .Check_No = d.Check_No _
            , .Cons_No = d.Cons_No _
            , .CurBal = d.CurBal _
            , .Invoice_Amt = d.Invoice_Amt _
            , .Invoiced = d.Invoiced _
            , .Pay_Amt = d.Pay_Amt _
            , .Payed = d.Payed _
            , .Pro_Number = d.Pro_Number _
            , .BookUpdated = If(d.BookUpdated Is Nothing, New Byte() {}, d.BookUpdated)}

    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetRecordFiltered(Control:=CType(LinqTable, LTS.vARMassEntry).BookControl)
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