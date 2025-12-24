Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.ServiceModel
Imports Ngl.FreightMaster.Data.LTS

Public Class NGLtblDATEquipTypeData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLookupDataContext(ConnectionString)
        Me.LinqTable = db.tblDATEquipTypes
        Me.LinqDB = db
        Me.SourceClass = "NGLtblDATEquipTypeData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLookupDataContext(ConnectionString)
            _LinqTable = db.tblDATEquipTypes
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
        Return GetDATEquipTypeFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetDATEquipTypesFiltered()
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
                Dim tblDATEquipType As DataTransferObjects.tblDATEquipType

                If LowerControl <> 0 Then
                    tblDATEquipType = (
                        From d In db.tblDATEquipTypes
                        Where d.DATEquipTypeControl >= LowerControl
                        Order By d.DATEquipTypeControl
                        Select selectDTOData(d, db)).FirstOrDefault
                Else
                    'Zero indicates that we should get the record with the lowest control number even if it is below zero
                    tblDATEquipType = (
                        From d In db.tblDATEquipTypes
                        Order By d.DATEquipTypeControl
                        Select selectDTOData(d, db)).FirstOrDefault
                End If



                Return tblDATEquipType

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetFirstRecord"))
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
                Dim tblDATEquipType As DataTransferObjects.tblDATEquipType = (
                        From d In db.tblDATEquipTypes
                        Where d.DATEquipTypeControl < CurrentControl
                        Order By d.DATEquipTypeControl Descending
                        Select selectDTOData(d, db)).FirstOrDefault


                Return tblDATEquipType

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetPreviousRecord"))
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
                Dim tblDATEquipType As DataTransferObjects.tblDATEquipType = (
                        From d In db.tblDATEquipTypes
                        Where d.DATEquipTypeControl > CurrentControl
                        Order By d.DATEquipTypeControl
                        Select selectDTOData(d, db)).FirstOrDefault


                Return tblDATEquipType

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetNextRecord"))
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
                Dim tblDATEquipType As DataTransferObjects.tblDATEquipType

                If UpperControl <> 0 Then

                    tblDATEquipType = (
                        From d In db.tblDATEquipTypes
                        Where d.DATEquipTypeControl >= UpperControl
                        Order By d.DATEquipTypeControl
                        Select selectDTOData(d, db)).FirstOrDefault
                Else
                    'Zero indicates that we should get the hightest ClassTypecontrol record
                    tblDATEquipType = (
                        From d In db.tblDATEquipTypes
                        Order By d.DATEquipTypeControl Descending
                        Select selectDTOData(d, db)).FirstOrDefault

                End If


                Return tblDATEquipType

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLastRecord"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetDATEquipTypeFiltered(ByVal Control As Integer) As DataTransferObjects.tblDATEquipType
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim tblDATEquipType As DataTransferObjects.tblDATEquipType = (
                        From d In db.tblDATEquipTypes
                        Where
                        d.DATEquipTypeControl = Control
                        Select selectDTOData(d, db)).First

                Return tblDATEquipType

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetDATEquipTypeFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetDATEquipTypesFiltered(Optional ByVal page As Integer = 1, Optional ByVal pagesize As Integer = 1000) As DataTransferObjects.tblDATEquipType()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1
                'count the record

                Try
                    intRecordCount = getScalarInteger("Select COUNT(*) from dbo.tblDATEquipType")

                Catch ex As Exception
                    'ignore any record count errors
                End Try
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize

                'Return all the contacts that match the criteria sorted by name
                Dim tblDATEquipTypes() As DataTransferObjects.tblDATEquipType = (
                        From d In db.tblDATEquipTypes
                        Order By d.DATEquipTypeControl
                        Select selectDTOData(d, db, page, intPageCount, intRecordCount, pagesize)).Skip(intSkip).Take(pagesize).ToArray()
                Return tblDATEquipTypes

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetDATEquipTypesFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Used by System processes forces delete and bypasses validation 
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <remarks></remarks>
    Public Sub SystemDelete(ByVal oData As DataTransferObjects.tblDATEquipType)
        Using db As New NGLMASLookupDataContext(ConnectionString)
            'Create New Record
            Dim nObject = CopyDTOToLinq(oData)
            db.tblDATEquipTypes.Attach(nObject, True)
            db.tblDATEquipTypes.DeleteOnSubmit(nObject)
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
        Dim d = CType(oData, DataTransferObjects.tblDATEquipType)
        'Create New Record
        Return New LTS.tblDATEquipType With {.DATEquipTypeControl = d.DATEquipTypeControl _
            , .DATEquipTypeName = d.DATEquipTypeName _
            , .DATEquipTypeDesc = d.DATEquipTypeDesc _
            , .DATEquipTypeModDate = Date.Now _
            , .DATEquipTypeModUser = Parameters.UserName _
            , .DATEquipTypeUpdated = If(d.DATEquipTypeUpdated Is Nothing, New Byte() {}, d.DATEquipTypeUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetDATEquipTypeFiltered(Control:=CType(LinqTable, LTS.tblDATEquipType).DATEquipTypeControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim source As LTS.tblDATEquipType = TryCast(LinqTable, LTS.tblDATEquipType)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.tblDATEquipTypes
                       Where d.DATEquipTypeControl = source.DATEquipTypeControl
                       Select New DataTransferObjects.QuickSaveResults With {.Control = d.DATEquipTypeControl _
                           , .ModDate = d.DATEquipTypeModDate _
                           , .ModUser = d.DATEquipTypeModUser _
                           , .Updated = d.DATEquipTypeUpdated.ToArray}).First

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

    Friend Function selectDTOData(ByVal d As LTS.tblDATEquipType, ByRef db As NGLMASLookupDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.tblDATEquipType
        Return New DataTransferObjects.tblDATEquipType With {.DATEquipTypeControl = d.DATEquipTypeControl _
            , .DATEquipTypeName = d.DATEquipTypeName _
            , .DATEquipTypeDesc = d.DATEquipTypeDesc _
            , .DATEquipTypeModDate = d.DATEquipTypeModDate _
            , .DATEquipTypeModUser = d.DATEquipTypeModUser,
            .Page = page,
            .Pages = pagecount,
            .RecordCount = recordcount,
            .PageSize = pagesize _
            , .DATEquipTypeUpdated = d.DATEquipTypeUpdated.ToArray()}
    End Function

#End Region

End Class