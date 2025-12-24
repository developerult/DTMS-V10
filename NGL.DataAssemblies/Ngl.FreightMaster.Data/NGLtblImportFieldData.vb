Imports System.ServiceModel

Public Class NGLtblImportFieldData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        'Me.Parameters = oParameters
        Me.Parameters = oParameters
        Dim db As New NGLMASIntegrationDataContext(ConnectionString)
        Me.LinqTable = db.tblImportFields
        Me.LinqDB = db
        Me.SourceClass = "NGLtblImportFieldData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASIntegrationDataContext(ConnectionString)
            _LinqTable = db.tblImportFields
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
        Return GettblImportFieldFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GettblImportFieldsFiltered(0)
    End Function

    Public Function GettblImportFieldFiltered(ByVal Control As Long) As DataTransferObjects.tblImportField
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim tblImportField As DataTransferObjects.tblImportField = (
                        From d In db.tblImportFields
                        Where
                        d.ImportControl = Control
                        Select New DataTransferObjects.tblImportField With {.ImportControl = d.ImportControl _
                        , .ImportFieldName = d.ImportFieldName _
                        , .ImportFileName = d.ImportFileName _
                        , .ImportFileType = d.ImportFileType _
                        , .ImportFlag = d.ImportFlag _
                        , .ImportUpdated = d.ImportUpdated.ToArray()}).First


                Return tblImportField

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

    Public Function GettblImportFieldFiltered(ByVal FileType As Integer, ByVal FieldName As String) As DataTransferObjects.tblImportField
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim tblImportField As DataTransferObjects.tblImportField = (
                        From d In db.tblImportFields
                        Where
                        d.ImportFileType = FileType And d.ImportFieldName = FieldName
                        Select New DataTransferObjects.tblImportField With {.ImportControl = d.ImportControl _
                        , .ImportFieldName = d.ImportFieldName _
                        , .ImportFileName = d.ImportFileName _
                        , .ImportFileType = d.ImportFileType _
                        , .ImportFlag = d.ImportFlag _
                        , .ImportUpdated = d.ImportUpdated.ToArray()}).FirstOrDefault()


                Return tblImportField

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GettblImportFieldFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GettblImportFieldsFiltered(ByVal FileType As Integer) As DataTransferObjects.tblImportField()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim tblImportFields() As DataTransferObjects.tblImportField = (
                        From d In db.tblImportFields
                        Where
                        (d.ImportFileType = FileType)
                        Order By d.ImportControl
                        Select New DataTransferObjects.tblImportField With {.ImportControl = d.ImportControl _
                        , .ImportFieldName = d.ImportFieldName _
                        , .ImportFileName = d.ImportFileName _
                        , .ImportFileType = d.ImportFileType _
                        , .ImportFlag = d.ImportFlag _
                        , .ImportUpdated = d.ImportUpdated.ToArray()}).ToArray()
                Return tblImportFields

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
    ''' returns a dictionary object of FieldName, ImportFlag for each field listed for this file type.
    ''' Caller must handle all exceptions.  may throw FaultException(Of SqlFaultInfo).
    ''' </summary>
    ''' <param name="ImportFileType"></param>
    ''' <param name="ImportFileName"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR 6/27/15 we now use sp
    ''' ImportFileName has been defined as optional and  is no longer needed and will be ignored,
    ''' </remarks>
    Public Function getImportFieldFlagDictionary(ByVal ImportFileType As Integer, Optional ByVal ImportFileName As String = "") As Dictionary(Of String, Boolean)
        Dim oFlags As New Dictionary(Of String, Boolean)
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                oFlags = db.spGetImportFieldFlagDictionaryData(ImportFileType).ToDictionary(Function(t) t.ImportFieldName, Function(y) y.ImportFlag)
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("getImportFieldFlagDictionary"))
            End Try

        End Using

        Return oFlags

    End Function

    Public Function getImportFieldFlagList(ByVal ImportFileType As Integer, Optional ByVal ImportFileName As String = "") As List(Of LTS.spGetImportFieldFlagDictionaryDataResult)

        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Return db.spGetImportFieldFlagDictionaryData(ImportFileType).ToList()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("getImportFieldFlagList"))
            End Try

        End Using

        Return Nothing

    End Function

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DataTransferObjects.tblImportField)
        'Create New Record
        Return New LTS.tblImportField With {.ImportControl = d.ImportControl _
            , .ImportFieldName = d.ImportFieldName _
            , .ImportFileName = d.ImportFileName _
            , .ImportFileType = d.ImportFileType _
            , .ImportFlag = d.ImportFlag _
            , .ImportUpdated = If(d.ImportUpdated Is Nothing, New Byte() {}, d.ImportUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GettblImportFieldFiltered(Control:=CType(LinqTable, LTS.tblImportField).ImportControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim source As LTS.tblImportField = TryCast(LinqTable, LTS.tblImportField)
                If source Is Nothing Then Return Nothing
                'Note this data source does not have a Mod Date or Mod User data field
                ret = (From d In db.tblImportFields
                    Where d.ImportControl = source.ImportControl
                    Select New DataTransferObjects.QuickSaveResults With {.Control = d.ImportControl _
                        , .ModDate = Date.Now _
                        , .ModUser = Parameters.UserName _
                        , .Updated = d.ImportUpdated.ToArray}).First

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


    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'Check if the data already exists only one allowed       
        With CType(oData, DataTransferObjects.tblImportField)
            Try
                Dim sFieldName As String = .ImportFieldName
                Dim iFileType As Integer = .ImportFileType

                'verify that the ImportFieldName is not in use for the provided ImportFileType
                If CType(oDB, NGLMASIntegrationDataContext).tblImportFields.Any(Function(x) x.ImportFieldName = sFieldName And x.ImportFileType = iFileType) Then
                    throwInvalidKeyAlreadyExistsException("tblImportFields", "ImportFieldName", sFieldName, False)
                End If

            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("ValidateNewRecord"))
            End Try
        End With
    End Sub


#End Region

End Class