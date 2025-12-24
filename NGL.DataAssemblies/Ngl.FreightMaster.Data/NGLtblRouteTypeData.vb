Imports System.ServiceModel
Imports Ngl.FreightMaster.Data.LTS

Public Class NGLtblRouteTypeData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLookupDataContext(ConnectionString)
        Me.LinqTable = db.tblRouteTypes
        Me.LinqDB = db
        Me.SourceClass = "NGLtblRouteTypeData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLookupDataContext(ConnectionString)
            _LinqTable = db.tblRouteTypes
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
        Return GettblRouteTypeFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GettblRouteTypesFiltered()
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
                Dim tblRouteType As DataTransferObjects.tblRouteType

                If LowerControl <> 0 Then
                    tblRouteType = (
                        From d In db.tblRouteTypes
                        Where d.RouteTypeControl >= LowerControl
                        Order By d.RouteTypeControl
                        Select New DataTransferObjects.tblRouteType With {.RouteTypeControl = d.RouteTypeControl _
                            , .RouteTypeName = d.RouteTypeName _
                            , .RouteTypeDesc = d.RouteTypeDesc _
                            , .RouteTypeUpdated = d.RouteTypeUpdated.ToArray()}).FirstOrDefault
                Else
                    'Zero indicates that we should get the record with the lowest control number even if it is below zero
                    tblRouteType = (
                        From d In db.tblRouteTypes
                        Order By d.RouteTypeControl
                        Select New DataTransferObjects.tblRouteType With {.RouteTypeControl = d.RouteTypeControl _
                            , .RouteTypeName = d.RouteTypeName _
                            , .RouteTypeDesc = d.RouteTypeDesc _
                            , .RouteTypeUpdated = d.RouteTypeUpdated.ToArray()}).FirstOrDefault
                End If



                Return tblRouteType

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
                Dim tblRouteType As DataTransferObjects.tblRouteType = (
                        From d In db.tblRouteTypes
                        Where d.RouteTypeControl < CurrentControl
                        Order By d.RouteTypeControl Descending
                        Select New DataTransferObjects.tblRouteType With {.RouteTypeControl = d.RouteTypeControl _
                        , .RouteTypeName = d.RouteTypeName _
                        , .RouteTypeDesc = d.RouteTypeDesc _
                        , .RouteTypeUpdated = d.RouteTypeUpdated.ToArray()}).FirstOrDefault


                Return tblRouteType

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
                Dim tblRouteType As DataTransferObjects.tblRouteType = (
                        From d In db.tblRouteTypes
                        Where d.RouteTypeControl > CurrentControl
                        Order By d.RouteTypeControl
                        Select New DataTransferObjects.tblRouteType With {.RouteTypeControl = d.RouteTypeControl _
                        , .RouteTypeName = d.RouteTypeName _
                        , .RouteTypeDesc = d.RouteTypeDesc _
                        , .RouteTypeUpdated = d.RouteTypeUpdated.ToArray()}).FirstOrDefault


                Return tblRouteType

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
                Dim tblRouteType As DataTransferObjects.tblRouteType

                If UpperControl <> 0 Then

                    tblRouteType = (
                        From d In db.tblRouteTypes
                        Where d.RouteTypeControl >= UpperControl
                        Order By d.RouteTypeControl
                        Select New DataTransferObjects.tblRouteType With {.RouteTypeControl = d.RouteTypeControl _
                            , .RouteTypeName = d.RouteTypeName _
                            , .RouteTypeDesc = d.RouteTypeDesc _
                            , .RouteTypeUpdated = d.RouteTypeUpdated.ToArray()}).FirstOrDefault
                Else
                    'Zero indicates that we should get the hightest RouteTypecontrol record
                    tblRouteType = (
                        From d In db.tblRouteTypes
                        Order By d.RouteTypeControl Descending
                        Select New DataTransferObjects.tblRouteType With {.RouteTypeControl = d.RouteTypeControl _
                            , .RouteTypeName = d.RouteTypeName _
                            , .RouteTypeDesc = d.RouteTypeDesc _
                            , .RouteTypeUpdated = d.RouteTypeUpdated.ToArray()}).FirstOrDefault

                End If


                Return tblRouteType

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

    Public Function GettblRouteTypeFiltered(ByVal Control As Integer) As DataTransferObjects.tblRouteType
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim tblRouteType As DataTransferObjects.tblRouteType = (
                        From d In db.tblRouteTypes
                        Where
                        d.RouteTypeControl = Control
                        Select New DataTransferObjects.tblRouteType With {.RouteTypeControl = d.RouteTypeControl _
                        , .RouteTypeName = d.RouteTypeName _
                        , .RouteTypeDesc = d.RouteTypeDesc _
                        , .RouteTypeUpdated = d.RouteTypeUpdated.ToArray()}).First


                Return tblRouteType

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

    Public Function GettblRouteTypesFiltered(Optional ByVal page As Integer = 1, Optional ByVal pagesize As Integer = 1000) As DataTransferObjects.tblRouteType()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1
                'count the record

                Try
                    intRecordCount = getScalarInteger("Select COUNT(*) from dbo.tblRouteType")

                Catch ex As Exception
                    'ignore any record count errors
                End Try
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize

                'Return all the contacts that match the criteria sorted by name
                Dim tblRouteTypes() As DataTransferObjects.tblRouteType = (
                        From d In db.tblRouteTypes
                        Order By d.RouteTypeControl
                        Select New DataTransferObjects.tblRouteType With {.RouteTypeControl = d.RouteTypeControl _
                        , .RouteTypeName = d.RouteTypeName _
                        , .RouteTypeDesc = d.RouteTypeDesc _
                        , .Page = page _
                        , .Pages = intPageCount _
                        , .RecordCount = intRecordCount _
                        , .PageSize = pagesize _
                        , .RouteTypeUpdated = d.RouteTypeUpdated.ToArray()}).Skip(intSkip).Take(pagesize).ToArray()
                Return tblRouteTypes

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

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DataTransferObjects.tblRouteType)
        'Create New Record
        Return New LTS.tblRouteType With {.RouteTypeControl = d.RouteTypeControl _
            , .RouteTypeName = d.RouteTypeName _
            , .RouteTypeDesc = d.RouteTypeDesc _
            , .RouteTypeUpdated = If(d.RouteTypeUpdated Is Nothing, New Byte() {}, d.RouteTypeUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GettblRouteTypeFiltered(Control:=CType(LinqTable, LTS.tblRouteType).RouteTypeControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim source As LTS.tblRouteType = TryCast(LinqTable, LTS.tblRouteType)
                If source Is Nothing Then Return Nothing
                'Note this data source does not have a Mod Date or Mod User data field
                ret = (From d In db.tblRouteTypes
                       Where d.RouteTypeControl = source.RouteTypeControl
                       Select New DataTransferObjects.QuickSaveResults With {.Control = d.RouteTypeControl _
                           , .ModDate = Date.Now _
                           , .ModUser = Me.Parameters.UserName _
                           , .Updated = d.RouteTypeUpdated.ToArray}).First

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
        'We do not allow Route Type records to be deleted 
        Utilities.SaveAppError("Cannot delete data.  Records cannot be deleted Using this interface!", Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))
    End Sub

#End Region

End Class