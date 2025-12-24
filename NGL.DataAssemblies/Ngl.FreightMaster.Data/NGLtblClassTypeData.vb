Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.ServiceModel
Imports Ngl.FreightMaster.Data.LTS

Public Class NGLtblClassTypeData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLookupDataContext(ConnectionString)
        Me.LinqTable = db.tblClassTypes
        Me.LinqDB = db
        Me.SourceClass = "NGLtblClassTypeData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLookupDataContext(ConnectionString)
            _LinqTable = db.tblClassTypes
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
        Return GettblClassTypeFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GettblClassTypesFiltered()
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

        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim tblClassType As DataTransferObjects.tblClassType

                If LowerControl <> 0 Then
                    tblClassType = (
                        From d In db.tblClassTypes
                        Where d.ClassTypeControl >= LowerControl
                        Order By d.ClassTypeControl
                        Select selectDTOData(d, db)).FirstOrDefault
                Else
                    'Zero indicates that we should get the record with the lowest control number even if it is below zero
                    tblClassType = (
                        From d In db.tblClassTypes
                        Order By d.ClassTypeControl
                        Select selectDTOData(d, db)).FirstOrDefault
                End If



                Return tblClassType

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
    ''' Enter the CurrentControl number to use as the reference to the previous record
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetPreviousRecord(ByVal CurrentControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the first record that matches the provided criteria
                Dim tblClassType As DataTransferObjects.tblClassType = (
                        From d In db.tblClassTypes
                        Where d.ClassTypeControl < CurrentControl
                        Order By d.ClassTypeControl Descending
                        Select selectDTOData(d, db)).FirstOrDefault


                Return tblClassType

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
    ''' Enter the CurrentControl number to use as the reference to the next record
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetNextRecord(ByVal CurrentControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the first record that matches the provided criteria
                Dim tblClassType As DataTransferObjects.tblClassType = (
                        From d In db.tblClassTypes
                        Where d.ClassTypeControl > CurrentControl
                        Order By d.ClassTypeControl
                        Select selectDTOData(d, db)).FirstOrDefault


                Return tblClassType

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
    ''' Enter zero as the UpperControl number return the record with the highest control PK
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="UpperControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetLastRecord(ByVal UpperControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim tblClassType As DataTransferObjects.tblClassType

                If UpperControl <> 0 Then

                    tblClassType = (
                        From d In db.tblClassTypes
                        Where d.ClassTypeControl >= UpperControl
                        Order By d.ClassTypeControl
                        Select selectDTOData(d, db)).FirstOrDefault
                Else
                    'Zero indicates that we should get the hightest ClassTypecontrol record
                    tblClassType = (
                        From d In db.tblClassTypes
                        Order By d.ClassTypeControl Descending
                        Select selectDTOData(d, db)).FirstOrDefault

                End If


                Return tblClassType

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

    Public Function GettblClassTypeFiltered(ByVal Control As Integer) As DataTransferObjects.tblClassType
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim tblClassType As DataTransferObjects.tblClassType = (
                        From d In db.tblClassTypes
                        Where
                        d.ClassTypeControl = Control
                        Select selectDTOData(d, db)).First

                Return tblClassType

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

    Public Function GettblClassTypesFiltered(Optional ByVal page As Integer = 1, Optional ByVal pagesize As Integer = 1000) As DataTransferObjects.tblClassType()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1
                'count the record

                Try
                    intRecordCount = getScalarInteger("Select COUNT(*) from dbo.tblClassType")

                Catch ex As Exception
                    'ignore any record count errors
                End Try
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize

                'Return all the contacts that match the criteria sorted by name
                Dim tblClassTypes() As DataTransferObjects.tblClassType = (
                        From d In db.tblClassTypes
                        Order By d.ClassTypeControl
                        Select selectDTOData(d, db, page, intPageCount, intRecordCount, pagesize)).Skip(intSkip).Take(pagesize).ToArray()
                Return tblClassTypes

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
    ''' Used by System processes forces delete and bypasses validation 
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <remarks></remarks>
    Public Sub SystemDelete(ByVal oData As DataTransferObjects.tblClassType)
        Using db As New NGLMASLookupDataContext(ConnectionString)
            'Create New Record
            Dim nObject = CopyDTOToLinq(oData)
            db.tblClassTypes.Attach(nObject, True)
            db.tblClassTypes.DeleteOnSubmit(nObject)
            Try
                db.SubmitChanges()
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
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
            DeleteCleanUp(nObject)
        End Using
    End Sub

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DataTransferObjects.tblClassType)
        'Create New Record
        Return New LTS.tblClassType With {.ClassTypeControl = d.ClassTypeControl _
            , .ClassTypeName = d.ClassTypeName _
            , .ClassTypeDesc = d.ClassTypeDesc _
            , .ClassTypeModDate = Date.Now _
            , .ClassTypeModUser = Parameters.UserName _
            , .ClassTypeUpdated = If(d.ClassTypeUpdated Is Nothing, New Byte() {}, d.ClassTypeUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GettblClassTypeFiltered(Control:=CType(LinqTable, LTS.tblClassType).ClassTypeControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim source As LTS.tblClassType = TryCast(LinqTable, LTS.tblClassType)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.tblClassTypes
                       Where d.ClassTypeControl = source.ClassTypeControl
                       Select New DataTransferObjects.QuickSaveResults With {.Control = d.ClassTypeControl _
                           , .ModDate = d.ClassTypeModDate _
                           , .ModUser = d.ClassTypeModUser _
                           , .Updated = d.ClassTypeUpdated.ToArray}).First

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

    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'We do not allow Class Type records to be deleted 
        Utilities.SaveAppError("Cannot delete data.  Records cannot be deleted Using this interface!", Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))
    End Sub

    Friend Function selectDTOData(ByVal d As LTS.tblClassType, ByRef db As NGLMASLookupDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.tblClassType
        Return New DataTransferObjects.tblClassType With {.ClassTypeControl = d.ClassTypeControl _
            , .ClassTypeName = d.ClassTypeName _
            , .ClassTypeDesc = d.ClassTypeDesc _
            , .ClassTypeModDate = d.ClassTypeModDate _
            , .ClassTypeModUser = d.ClassTypeModUser,
            .Page = page,
            .Pages = pagecount,
            .RecordCount = recordcount,
            .PageSize = pagesize _
            , .ClassTypeUpdated = d.ClassTypeUpdated.ToArray()}
    End Function

#End Region

End Class