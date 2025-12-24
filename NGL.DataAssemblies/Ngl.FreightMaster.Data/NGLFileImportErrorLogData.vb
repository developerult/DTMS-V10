Imports System.ServiceModel

Public Class NGLFileImportErrorLogData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Me.Parameters = oParameters
        Dim db As New NGLMASIntegrationDataContext(ConnectionString)
        Me.LinqTable = db.FileImportErrorLogs
        Me.LinqDB = db
        Me.SourceClass = "NGLFileImportErrorLogData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASIntegrationDataContext(ConnectionString)
            _LinqTable = db.FileImportErrorLogs
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
        Return GetFileImportErrorLogFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetFileImportErrorLogsFiltered(0)
    End Function

    Public Function GetFileImportErrorLogFiltered(ByVal Control As Long) As DataTransferObjects.FileImportErrorLog
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'not supported

                'Get the newest record that matches the provided criteria
                'Dim fileImportErrorLog As DTO.FileImportErrorLog = ( _
                'From d In db.FileImportErrorLogs _
                'Where _
                '    d.ImportFileType = Control _
                'Select New DTO.tblImportField With {.ImportControl = d.ImportControl _
                '                                , .ImportFieldName = d.ImportFieldName _
                '                                , .ImportFileName = d.ImportFileName _
                '                                , .ImportFileType = d.ImportFileType _
                '                                , .ImportFlag = d.ImportFlag _
                '                                , .ImportUpdated = d.ImportUpdated.ToArray()}).First


                Return Nothing

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

    Public Function GetFileImportErrorLogsFiltered(ByVal FileType As Integer) As DataTransferObjects.FileImportErrorLog()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim fileImportErrorLogArr() As DataTransferObjects.FileImportErrorLog = (
                        From d In db.FileImportErrorLogs
                        Where
                        (d.ImportFileType = FileType)
                        Order By d.ErrorDate Descending
                        Select New DataTransferObjects.FileImportErrorLog With {.CreateUser = d.CreateUser _
                        , .ErrorDate = If(d.ErrorDate.HasValue, d.ErrorDate.Value, Date.Now) _
                        , .ErrorMsg = d.ErrorMsg _
                        , .ImportFileName = d.ImportFileName _
                        , .ImportFileType = d.ImportFileType _
                        , .ImportName = d.ImportName _
                        , .ImportRecord = d.ImportRecord}).Take(100).ToArray()
                Return fileImportErrorLogArr

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

    Public Function GetFileImportErrorLogsFiltered(ByVal FileType As Integer, ByVal HowManyToSelect As Integer) As DataTransferObjects.FileImportErrorLog()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim fileImportErrorLogArr() As DataTransferObjects.FileImportErrorLog = (
                        From d In db.FileImportErrorLogs
                        Where
                        (d.ImportFileType = FileType)
                        Order By d.ErrorDate Descending
                        Select New DataTransferObjects.FileImportErrorLog With {.CreateUser = d.CreateUser _
                        , .ErrorDate = If(d.ErrorDate.HasValue, d.ErrorDate.Value, Date.Now) _
                        , .ErrorMsg = d.ErrorMsg _
                        , .ImportFileName = d.ImportFileName _
                        , .ImportFileType = d.ImportFileType _
                        , .ImportName = d.ImportName _
                        , .ImportRecord = d.ImportRecord}).Take(HowManyToSelect).ToArray()
                Return fileImportErrorLogArr

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
        Dim d = CType(oData, DataTransferObjects.FileImportErrorLog)
        'Create New Record
        Return New LTS.FileImportErrorLog With {.CreateUser = d.CreateUser _
            , .ErrorDate = If(d.ErrorDate.HasValue, d.ErrorDate.Value, Date.Now) _
            , .ErrorMsg = d.ErrorMsg _
            , .ImportFileName = d.ImportFileName _
            , .ImportFileType = d.ImportFileType _
            , .ImportName = d.ImportName _
            , .ImportRecord = d.ImportRecord}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetFileImportErrorLogFiltered(Control:=CType(LinqTable, LTS.FileImportErrorLog).ImportFileType)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim source As LTS.FileImportErrorLog = TryCast(LinqTable, LTS.FileImportErrorLog)
                If source Is Nothing Then Return Nothing
                'not supported
                ret = Nothing

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

#End Region

End Class