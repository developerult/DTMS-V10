Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.ServiceModel

Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports LTS = Ngl.FreightMaster.Data.LTS
Imports Ngl.Core.ChangeTracker
Imports DTran = Ngl.Core.Utility.DataTransformation

Public Class NGLBatchProcessDataProvider : Inherits NDPBaseClass


#Region " Constructors "
    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASSYSDataContext(ConnectionString)
        Me.LinqTable = db.tblBatchProcessRunnings
        Me.LinqDB = db
        Me.SourceClass = "NGLBatchProcessDataProvider"
    End Sub


#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASSYSDataContext(ConnectionString)
            _LinqTable = db.tblBatchProcessRunnings
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
        Return Nothing
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    Public Function ValidateAlertsBeforeFinalize(ByVal BookControl As Integer) As DTO.DoFinalizeAlerts
        Dim oSysData As New NGLSystemDataProvider(Me.Parameters)
        Return oSysData.GetDoFinalizeAlerts(BookControl)
    End Function



    Public Function ValidateCarrierQualBeforeFinalize(ByVal BookControl As Integer) As LTS.spValidateCarrierQualBeforeFinalizeResult
        Dim oSysData As New NGLSystemDataProvider(Me.Parameters)
        Return oSysData.ValidateCarrierQualBeforeFinalize(BookControl)
    End Function

    Public Function IsBatchProcessRunning(ByVal ProcessName As String) As Boolean
        Dim oSysData As New NGLSystemDataProvider(Me.Parameters)
        Return oSysData.IsBatchProcessRunning(Me.Parameters.UserName, ProcessName)
    End Function

    Public Function DLookup(ByVal FieldName As String, ByVal TableName As String, ByVal Filter As String) As String
        Dim strSQL As String = "Select top 1 " & FieldName & " From " & TableName & " Where " & Filter
        Return getScalarString(strSQL)
    End Function

    Public Function getCompFilterByControl(ByVal FieldName As String) As String
        Dim strSQL As String = "Select dbo.getCompFilterByControl('" & FieldName & "') as RetVal"
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim result As Ngl.Core.Data.QueryResult = oQuery.ExecuteWithFill(strSQL)
        If result.Data.Rows.Count = 0 Then Return ""
        Return result.Data.Rows(0).Item(0).ToString
    End Function

    Public Sub SendEmailToCarrier(ByVal CompControl As Integer, ByVal CarrierControl As Integer, ByVal CarrierContControl As Integer, ByVal Subject As String, ByVal Body As String)
        Dim strBatchName As String = "SendEmailToCarrier"
        Dim strProcName As String = "dbo.spSendEmailToCarrier"
        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@CompControl", CompControl)
        oCmd.Parameters.AddWithValue("@CarrierControl", CarrierControl)
        oCmd.Parameters.AddWithValue("@CarrierContControl", CarrierContControl)
        oCmd.Parameters.AddWithValue("@Subject", Left(Subject, 100))
        oCmd.Parameters.AddWithValue("@Body", Left(Body, 4000))
        runNGLStoredProcedure(oCmd, strProcName, 2)
    End Sub

    Public Sub CreateServerEmailMsg(ByVal EmailFrom As String, ByVal EmailTo As String, ByVal CCEmail As String, ByVal Subject As String, ByVal Body As String)
        Dim strBatchName As String = "CreateServerEmailMsg"
        Dim strProcName As String = "dbo.spCreateServerEmailMsg"
        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@EmailFrom", Left(EmailFrom, 100))
        oCmd.Parameters.AddWithValue("@EmailTo", Left(EmailTo, 500))
        oCmd.Parameters.AddWithValue("@CCEmail", Left(CCEmail, 500))
        oCmd.Parameters.AddWithValue("@Subject", Left(Subject, 100))
        oCmd.Parameters.AddWithValue("@Body", Left(Body, 4000))
        runNGLStoredProcedure(oCmd, strProcName, 2)
    End Sub

    Public Sub CreateServerEmailMsgWithReport(ByVal EmailFrom As String,
                                              ByVal EmailTo As String,
                                              ByVal CCEmail As String,
                                              ByVal Subject As String,
                                              ByVal Body As String,
                                              ByVal Path As String,
                                              ByVal Format As String,
                                              ByVal Parameters As String,
                                              ByVal FileName As String)
        Dim strBatchName As String = "CreateServerEmailMsg"
        Dim strProcName As String = "dbo.spCreateServerEmailMsg"
        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@EmailFrom", Left(EmailFrom, 100))
        oCmd.Parameters.AddWithValue("@EmailTo", Left(EmailTo, 500))
        oCmd.Parameters.AddWithValue("@CCEmail", Left(CCEmail, 500))
        oCmd.Parameters.AddWithValue("@Subject", Left(Subject, 100))
        oCmd.Parameters.AddWithValue("@Body", Left(Body, 4000))
        oCmd.Parameters.AddWithValue("@Path", Left(Path, 1000))
        oCmd.Parameters.AddWithValue("@Format", Left(Format, 50))
        oCmd.Parameters.AddWithValue("@Parameters", Left(Parameters, 1000))
        oCmd.Parameters.AddWithValue("@FileName", Left(FileName, 50))
        runNGLStoredProcedure(oCmd, strProcName, 2)
    End Sub

    Public Function GetNextConsNumber(ByVal CompControl As Integer) As String
        Dim strSQL As String = "Exec dbo.spGetNextConsNumber " & CompControl
        Return getScalarString(strSQL)
    End Function

    Public Function updateBookingTranCode(ByVal TranCode As String, ByVal BookControl As Integer) As Boolean
        Return executeSQL("update Book set BookTranCode = '" & TranCode & "' " _
             & " where BookControl = " & BookControl)

    End Function

    Public Function GetCustAbrev(ByVal Control As Integer,
                                 Optional ByVal UseCompNumber As Boolean = False) As String

        Dim strSQL As String = "Select dbo.comp.compabrev as RetVal " _
            & " From dbo.comp "
        If UseCompNumber Then
            strSQL &= " Where dbo.comp.compnumber = " & Control
        Else
            strSQL &= " Where dbo.comp.compcontrol = " & Control
        End If
        Return getScalarString(strSQL)

    End Function

    ''' <summary>
    ''' Returns the next Pro Number for Legal Entity and optional returns the new Pro Base and/or the new Pro Abrev
    ''' </summary>
    ''' <param name="CompControl"></param>
    ''' <param name="sNewProBase"></param>
    ''' <param name="sNewProAbrev"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.2.0.003 on 1/30/2021
    '''   added logic to use new stored procedures that look up Legal Entity data
    '''   optionally returns the new pro base and new pro abr
    ''' </remarks>
    Public Function GetNextProNumber(ByVal CompControl As Integer,
                                     Optional ByRef sNewProBase As String = "",
                                     Optional ByRef sNewProAbrev As String = "") As String
        Dim sNewPro = ""
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Begin Modified by RHR for v-8.1 on 04/04/2018
                Dim oProBase = db.spGetNextProBase(CompControl, Nothing).FirstOrDefault()
                If Not oProBase Is Nothing Then
                    sNewProBase = oProBase.NewProNumber
                End If
                Dim oProAbrev = db.spGetProAbrev(CompControl, Nothing).FirstOrDefault()
                If Not oProAbrev Is Nothing Then
                    sNewProAbrev = oProAbrev.ProAbrev
                End If
                sNewPro = sNewProAbrev & sNewProBase
                If String.IsNullOrWhiteSpace(sNewPro) Then
                    throwInvalidKeyFilterMetaDataException(SqlFaultInfo.FaultDetailsKey.E_SystemFaliedToGeneratedKeyField, New List(Of String) From {"Book Pro Number"})
                End If
                ' Legacy code removed 01/30/2021 by RHR
                'Dim strPro As String = getScalarString("SELECT TOP 1 p.ParValue FROM dbo.parameter as p WHERE p.parkey = 'PRONUMBER'")
                'Dim intNextPro As Integer = 0
                'Integer.TryParse(strPro, intNextPro)
                'intNextPro += 1
                'executeSQL("Update dbo.Parameter Set ParValue = " & intNextPro & " Where ParKey = 'PRONUMBER'")
                'strPro = Trim(GetCustAbrev(CompControl)) & intNextPro.ToString
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetNextProNumber"))
            End Try
        End Using
        Return sNewPro
    End Function

    ''' <summary>
    ''' Compies all booking and booking items to a new records with the same order number and a new sequence
    ''' , typically used for cross dock or trans load logic
    ''' </summary>
    ''' <param name="CompControl"></param>
    ''' <param name="OldPR0Number"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.1 on 04/04/2018
    '''   added logic to use the new LTS versions of spGetNextProBase and spGetProAbrev 
    '''   includes logic to use the Legal Entity Pro Seed and CNS seed setting
    ''' TODO: Add logic to use localization on erros based on  Utilities.NGLSPErrorCodes
    ''' Modified by RHR for v-8.3.0.003 on 01/30/2021
    '''   added new logic to call the updated GetNextProNumber function
    '''   so all code flows through the same code base logic
    ''' Modifie by RHR for v-8.5.0.002 on 12/21/2021 fixed bug where sNewProBase was not being passed to GetNextProNumber correcly
    ''' </remarks>
    Public Function CreateBookingOrderSequence(ByVal CompControl As Integer, ByVal OldPR0Number As String) As String

        Dim sNewProBase As String = ""
        Dim sNewProAbrev As String = ""
        Dim sNewPRONumber = ""
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                sNewPRONumber = GetNextProNumber(CompControl, sNewProBase, sNewProAbrev)
                Dim oResults = db.spCreateBookingOrderSequence70(sNewPRONumber, sNewProBase, OldPR0Number, Parameters.UserName, Nothing, Nothing).FirstOrDefault()
                If Not oResults Is Nothing Then
                    If oResults.ErrNumber <> 0 Then
                        Dim strDetails As String = oResults.RetMsg
                        'TODO: Add logic to use localization on erros based on  Utilities.NGLSPErrorCodes
                        If oResults.ErrNumber > Utilities.NGLSPErrorCodes.Invalid_Value Then
                            strDetails = "Cannot Create Booking Order Sequence due to the following Error # " & oResults.ErrNumber & ": " & oResults.RetMsg & ". "
                        End If
                        Dim strMessage = "E_DataValidationFailure"
                        Utilities.SaveAppError(strDetails, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = strMessage, .Details = strDetails}, New FaultReason("E_ProcessProcedureFailure"))
                    End If
                End If

            Catch ex As FaultException
    Throw
    Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("CreateBookingOrderSequence"))
            End Try
    End Using

        Return sNewPRONumber


    End Function

    Public Function CreateBookingOrderSequenceRetvBookMaintLookup(ByVal CompControl As Integer, ByVal OldPR0Number As String) As DTO.vBookMaintLookup
        Return New NGLLookupDataProvider(Me.Parameters).GetBookMaintItemByProNumber(CreateBookingOrderSequence(CompControl, OldPR0Number))
    End Function


#End Region

#Region "Protected Methods"
    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Return Nothing
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return Nothing
    End Function

    'Protected Sub RunBatchStoredProcedure(ByVal strProcName As String, ByVal strBatchName As String, ByRef oCmd As System.Data.SqlClient.SqlCommand, ByRef oSystem As NGLSystemDataProvider)

    '    Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
    '    Try
    '        'Update the batch processing tracking table
    '        oSystem.StarttblBatchProcessRunning(Me.Parameters.UserName, strBatchName)
    '        'Create a data connection 
    '        openConnection()
    '        'If DBCon is passed before it has been opened the funtion will create a new connection.
    '        oQuery.execNGLStoredProcedure(DBCon, oCmd, strProcName, 2)
    '        oSystem.EndtblBatchProcessRunning(Me.Parameters.UserName, strBatchName)
    '    Catch ex As FaultException(Of SqlFaultInfo)
    '        Try
    '            oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Detail.Message, ex.Reason.ToString)
    '        Catch e As Exception
    '            'just log the error and continue
    '            Utilities.SaveAppError(e.Message, Me.Parameters)
    '        End Try
    '    Catch ex As Ngl.Core.DatabaseDataValidationException
    '        Utilities.SaveAppError(ex.Message, Me.Parameters)
    '        oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_DataValidationFailure")
    '    Catch ex As Core.DatabaseRetryExceededException
    '        Utilities.SaveAppError(ex.Message, Me.Parameters)
    '        Dim inner As System.Exception = ex.InnerException
    '        If Not inner Is Nothing Then
    '            Utilities.SaveAppError(inner.Message, Me.Parameters)
    '            oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, inner.Message, "E_FailedToExecute")
    '        Else
    '            oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_FailedToExecute")
    '        End If
    '    Catch ex As Ngl.Core.DatabaseLogInException
    '        Utilities.SaveAppError(ex.Message, Me.Parameters)
    '        oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_DBLoginFailure")
    '    Catch ex As Ngl.Core.DatabaseInvalidException
    '        Utilities.SaveAppError(ex.Message, Me.Parameters)
    '        oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_DBConnectionFailure")
    '    Catch ex As System.Data.SqlClient.SqlException
    '        Utilities.SaveAppError(ex.Message, Me.Parameters)
    '        oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_SQLException")
    '    Catch ex As Exception
    '        Utilities.SaveAppError(ex.Message, Me.Parameters)
    '        oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
    '    Finally
    '        closeConnection()
    '        Try
    '            oQuery = Nothing
    '        Catch ex As Exception

    '        End Try

    '    End Try
    'End Sub

    Protected Sub RunBatchCLRStoredProcedure(ByVal strProcName As String, ByVal strBatchName As String, ByRef oCmd As System.Data.SqlClient.SqlCommand, ByRef oSystem As NGLSystemDataProvider)

        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Try
            'Update the batch processing tracking table
            oSystem.StarttblBatchProcessRunning(Me.Parameters.UserName, strBatchName)


            'If oCon is passed before it has been opened the funtion will create a new connection.
            oQuery.execNGLCLRStoredProcedure(oCon, oCmd, strProcName, 2)
            oSystem.EndtblBatchProcessRunning(Me.Parameters.UserName, strBatchName)
        Catch ex As FaultException(Of SqlFaultInfo)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Detail.Message, ex.Reason.ToString)
            Catch e As Exception
                'just log the error and continue
                Utilities.SaveAppError(e.Message, Me.Parameters)
            End Try
        Catch ex As Ngl.Core.DatabaseDataValidationException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_DataValidationFailure")
        Catch ex As Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_FailedToExecute")
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_DBLoginFailure")
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_DBConnectionFailure")
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_SQLException")
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                oCmd.Cancel()
                oCmd = Nothing
            Catch ex As Exception

            End Try

            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try

        End Try
    End Sub

    Protected Sub RunCLRStoredProcedure(ByRef oCmd As System.Data.SqlClient.SqlCommand, ByVal strProcName As String, Optional ByVal intMaxRetry As Integer = 3)

        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Try
            'If oCon is passed before it has been opened the funtion will create a new connection.
            oQuery.execNGLCLRStoredProcedure(oCon, oCmd, strProcName, intMaxRetry)
        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseDataValidationException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DataValidationFailure"}, New FaultReason("E_ProcessProcedureFailure"))
        Catch ex As Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_ProcessProcedureFailure"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                oCmd.Cancel()
                oCmd = Nothing
            Catch ex As Exception

            End Try

            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try

        End Try
    End Sub

#End Region

#Region "Test Methods"
    Public Sub TestBatchProcessing(ByVal intTimeToWait As Integer)
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        oSystem.StarttblBatchProcessRunning(Me.Parameters.UserName, "TestBatchProcess")
        Threading.Thread.Sleep(intTimeToWait)
        oSystem.EndtblBatchProcessRunning(Me.Parameters.UserName, "TestBatchProcess")
    End Sub

    Public Sub TestBatchProcessingWithError(ByVal intTimeToWait As Integer)
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        oSystem.StarttblBatchProcessRunning(Me.Parameters.UserName, "TestBatchProcess")
        Threading.Thread.Sleep(intTimeToWait)
        oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, "TestBatchProcess", "Your Test of Batch Processing Returned an Error", "E_UnExpected")
    End Sub

#End Region

#Region "Batch Processing Stored Procedures"

    ''' <summary>
    ''' Inserts an auto approved freight bill for each order assocated with the provided LegalEntity.
    ''' Typically used for testing. 
    ''' </summary>
    ''' <param name="LegalEntity"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UtilityMassUpdateAllTestFreightBills(ByVal LegalEntity As String) As List(Of LTS.spUtilityMassUpdateAllTestFreightBillsResult)
        Dim oRet As New List(Of LTS.spUtilityMassUpdateAllTestFreightBillsResult)
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                oRet = db.spUtilityMassUpdateAllTestFreightBills(LegalEntity).ToList()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UtilityMassUpdateAllTestFreightBills"))
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' set the global parameter GlobalAllowSilentTendering to 1 and sets
    ''' all CompLegalEntity flags to true for each company assigned to LegalEntity
    ''' Typically used for testing. 
    ''' </summary>
    ''' <param name="LegalEntity"></param>
    ''' <remarks></remarks>
    Public Sub UtilityAllowSilentTender(ByVal LegalEntity As String)
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim intRet = db.spUtilityAllowSilentTender(LegalEntity)
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UtilityAllowSilentTender"))
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Deletes all records created by test data related to the LegalEntity.
    ''' Typically used for testing.  
    ''' </summary>
    ''' <param name="LegalEntity"></param>
    ''' <remarks></remarks>
    Public Sub UtilityRemoveAllTestDataByLegalEntity(ByVal LegalEntity As String)
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim intRet = db.spUtilityRemoveAllTestDataByLegalEntity(LegalEntity)
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UtilityRemoveAllTestDataByLegalEntity"))
            End Try
        End Using
    End Sub

    Public Function UpdateBudgetActSales2Way() As Boolean
        Dim strBatchName As String = "UpdateBudgetActSales"
        Dim strProcName As String = "dbo.spUpdateBudgetActSales"
        Dim blnRet As Boolean = False

        Dim oCmd As New System.Data.SqlClient.SqlCommand
        runNGLStoredProcedure(oCmd, strProcName, 0)
        blnRet = True
        Return blnRet
    End Function

    Public Function UpdateCreditRoutine2Way() As Boolean
        Dim strBatchName As String = "UpdateCreditRoutine"
        Dim strProcName As String = "dbo.spUpdateCreditRoutine"
        Dim blnRet As Boolean = False

        Dim oCmd As New System.Data.SqlClient.SqlCommand
        runNGLStoredProcedure(oCmd, strProcName, 0)
        blnRet = True
        Return blnRet
    End Function

    Public Function UpdateYTD2Way() As Boolean
        Dim strBatchName As String = "UpdateYTD"
        Dim strProcName As String = "dbo.spUpdateYTD"
        Dim blnRet As Boolean = False

        Dim oCmd As New System.Data.SqlClient.SqlCommand
        runNGLStoredProcedure(oCmd, strProcName, 0)
        blnRet = True
        Return blnRet
    End Function

    Public Function UpdateLaneAddressWithCompanyByCompNumber2Way(ByVal CompNumber As Integer) As Boolean
        Dim strBatchName As String = "UpdateLaneAddressWithCompanyByCompNumber"
        Dim strProcName As String = "dbo.spUpdateLaneAddressWithCompAddress50"
        Dim blnRet As Boolean = False

        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@CompControl", 0)
        oCmd.Parameters.AddWithValue("@CompNumber", CompNumber)
        oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        runNGLStoredProcedure(oCmd, strProcName, 0)
        blnRet = True
        Return blnRet
    End Function

    'User Name Parameter Not Needed
    Public Sub addCompanyParameters(ByVal CompControl As Integer)
        Dim strBatchName As String = "addCompanyParameters"
        Dim strProcName As String = "dbo.spInsertDefaultCompanyParameters"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            'Validate the parameter data
            If CompControl = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The company control number is not valid. Please select a valid company record to run this procedure.", "E_DataValidationFailure")
                Return
            End If
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@CompControl", CompControl)
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub
    'User Name Parameter Not Needed
    Public Sub AllocateBFCtoDetailByWgt(ByVal BookControl As Integer)
        Dim strBatchName As String = "AllocateBFCtoDetailByWgt"
        Dim strProcName As String = "dbo.spAllocateLoadBFCByWgt"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            'Validate the parameter data
            If BookControl = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The book control number is not valid. Please select a valid booking record to run this procedure.", "E_DataValidationFailure")
                Return
            End If
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@BookControl", BookControl)
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub

    Public Function AllocateBFCtoDetailByWgt2Way(ByVal BookControl As Integer) As Boolean
        Dim strBatchName As String = "AllocateBFCtoDetailByWgt"
        Dim strProcName As String = "dbo.spAllocateLoadBFCByWgt"
        Dim blnRet As Boolean = False
        'Validate the parameter data
        If BookControl = 0 Then Return False

        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@BookControl", BookControl)
        runNGLStoredProcedure(oCmd, strProcName, 0)
        blnRet = True
        Return blnRet
    End Function

    'User Name Parameter Not Needed
    Public Sub AllocTotalCostToDetailByWgt(ByVal BookControl As Integer)
        Dim strBatchName As String = "AllocTotalCostToDetailByWgt"
        Dim strProcName As String = "dbo.spAllocateLoadTotalCostByWgt"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            'Validate the parameter data
            If BookControl = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The book control number is not valid. Please select a valid booking record to run this procedure.", "E_DataValidationFailure")
                Return
            End If
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@BookControl", BookControl)
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub

    Public Function AllocTotalCostToDetailByWgt2Way(ByVal BookControl As Integer) As Boolean
        Dim strBatchName As String = "AllocTotalCostToDetailByWgt"
        Dim strProcName As String = "dbo.spAllocateLoadTotalCostByWgt"
        Dim blnRet As Boolean = False
        'Validate the parameter data
        If BookControl = 0 Then Return False
        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@BookControl", BookControl)
        runNGLStoredProcedure(oCmd, strProcName, 0)
        blnRet = True
        Return blnRet
    End Function

    'This function uses the lane carr table and while it uses the current user to update the LaneCarrModUser it is obsolete and rarely used
    'The stored procedure will not be modified to use the actual user name.
    Public Sub deleteAllLaneTrucks(ByVal CarrierTruckControl As Integer)
        Dim strBatchName As String = "deleteAllLaneTrucks"
        Dim strProcName As String = "dbo.spdeleteAllLaneTrucks"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            'Validate the parameter data
            If CarrierTruckControl = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The carrier truck control number is not valid. Please select a valid record to run this procedure.", "E_DataValidationFailure")
                Return
            End If
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@CarrierTruckControl", CarrierTruckControl)
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub
    'User Name Parameter Not Needed
    Public Sub UpdateMtoAA()
        Dim strBatchName As String = "UpdateMtoAA"
        Dim strProcName As String = "dbo.spSetMToAA"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub
    'New @UserName parameter added
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="CarrierNumber"></param>
    ''' <param name="RecDateFrom"></param>
    ''' <param name="RecDateTo"></param>
    ''' <param name="Electronic"></param>
    ''' <remarks>
    '''  Modified by RHR for v-8.2.1.004 on 01/09/2020
    '''     we no longer call stored procedure
    '''     now we call FreightBillAudit2Way which calls NGLBookData.UpdateAndAuditAPMassEntry
    ''' </remarks>
    Public Sub FreightBillAudit(ByVal CarrierNumber As Integer, ByVal RecDateFrom As Date, ByVal RecDateTo As Date, ByVal Electronic As Boolean)

        Dim strBatchName As String = "FreightBillAudit"
        Dim strProcName As String = "dbo.spAuditAPFreightBills50"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            FreightBillAudit2Way(CarrierNumber, RecDateFrom, RecDateTo, Electronic)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub

    ''' <summary>
    ''' Audit any freight bills not in AA status using the parameters provided as filters
    ''' </summary>
    ''' <param name="CarrierNumber"></param>
    ''' <param name="RecDateFrom"></param>
    ''' <param name="RecDateTo"></param>
    ''' <param name="Electronic"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.2.1.004 on 01/09/2020
    '''     we no longer call stored procedure
    '''     now we call NGLBookData.UpdateAndAuditAPMassEntry
    ''' </remarks>
    Public Function FreightBillAudit2Way(ByVal CarrierNumber As Integer, ByVal RecDateFrom As Date, ByVal RecDateTo As Date, ByVal Electronic As Boolean) As Boolean

        Dim strErrMsg As String = ""
        Dim sbRetMsgs As New System.Text.StringBuilder()
        Dim blnRet As Boolean = True
        If CarrierNumber = 0 Then Return False
        Try
            Dim oFBsToAudit = NGLAPMassEntryObjData.GetAPFreightBillsTotAuditByCarrierAndDate(CarrierNumber, RecDateFrom, RecDateTo, Electronic)
            If oFBsToAudit Is Nothing OrElse oFBsToAudit.Count() < 1 Then
                Return True 'nothing to audit
            Else
                If oFBsToAudit(0).ErrNumber <> 0 Then
                    blnRet = False
                    Utilities.SaveAppError(oFBsToAudit(0).RetMsg, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = oFBsToAudit(0).RetMsg}, New FaultReason("E_BatchRunAPFreightBillAudit"))
                Else
                    Try
                        For Each oFB In oFBsToAudit
                            Dim oResults As New Models.ResultObject() With {.Success = True, .SuccessMsg = "Success!"}
                            oResults = NGLBookObjData.UpdateAndAuditAPMassEntry(oResults, oFB.APControl, oFB.APSHID, oFB.BookControl, oFB.APBillNumber, Electronic)
                            If Not oResults Is Nothing AndAlso oResults.Success = False Then
                                If Not String.IsNullOrWhiteSpace(oResults.Msg) Then sbRetMsgs.Append(oResults.Msg)
                                If Not String.IsNullOrWhiteSpace(oResults.WarningMsg) Then sbRetMsgs.Append(oResults.WarningMsg)
                                If Not String.IsNullOrWhiteSpace(oResults.ErrMsg) Then sbRetMsgs.Append(oResults.ErrMsg)
                            End If
                        Next
                    Catch ex As Exception
                        sbRetMsgs.Append(ex.Message)
                    End Try
                End If
            End If

        Catch ex As Exception
            blnRet = False
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_BatchRunAPFreightBillAudit"))
        End Try
        strErrMsg = sbRetMsgs.ToString()
        If Not String.IsNullOrWhiteSpace(strErrMsg) Then
            blnRet = False
            Utilities.SaveAppError(strErrMsg, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = strErrMsg}, New FaultReason("E_BatchRunAPFreightBillAudit"))
        End If
        Return blnRet
    End Function


    ''' <summary>
    ''' Audit All Matched Loads
    ''' Do not Confuse with AuditAPFreightBills routine
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function APFreightBillAudit2Way() As Boolean
        'This batch is actually the: "Audit All Matched Loads" routine.
        Dim strBatchName As String = "APFreightBillAudit"
        Dim strProcName As String = "dbo.spAPFreightBillAudit"
        Dim blnRet As Boolean = False
        'Validate the parameter data

        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        runNGLStoredProcedure(oCmd, strProcName, 0)
        blnRet = True
        Return blnRet
    End Function

    'New @UserName parameter added
    Public Sub insertBookStatusMsg(ByVal BookControl As Integer, ByVal BookTrackContact As String, ByVal BookTrackComments As String, ByVal BookTrackStatus As Integer)
        Dim strBatchName As String = "insertBookStatusMsg"
        Dim strProcName As String = "dbo.spUpdateBookTrackingRecord50"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            'Validate the parameter data
            If BookControl = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The book control number is not valid. Please select a valid booking record to run this procedure.", "E_DataValidationFailure")
                Return
            End If

            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@BookControl", BookControl)
            oCmd.Parameters.AddWithValue("@BookTrackContact", Left(BookTrackContact, 50))
            oCmd.Parameters.AddWithValue("@BookTrackComments", Left(BookTrackComments, 255))
            oCmd.Parameters.AddWithValue("@BookTrackStatus", BookTrackStatus)
            oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub
    'User Name Parameter Not Needed
    Public Sub InsertSalesHistoryDetails()
        Dim strBatchName As String = "InsertSalesHistoryDetails"
        Dim strProcName As String = "dbo.spInsertSalesHistoryDetails"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub
    'User Name Parameter Not Needed
    Public Sub UpdateYTD()
        Dim strBatchName As String = "UpdateYTD"
        Dim strProcName As String = "dbo.spUpdateYTD"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub
    'New @UserName parameter added
    Public Sub UpdateLaneAddressWithCompany(ByVal CompControl As Integer)
        Dim strBatchName As String = "UpdateLaneAddressWithCompany"
        Dim strProcName As String = "dbo.spUpdateLaneAddressWithCompAddress50"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            'Validate the parameter data
            If CompControl = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The company control number is not valid. Please select a valid company record to run this procedure.", "E_DataValidationFailure")
                Return
            End If
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@CompControl", CompControl)
            oCmd.Parameters.AddWithValue("@CompNumber", 0)
            oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub
    'New @UserName parameter added
    Public Sub UpdateLaneAddressWithCompanyByCompNumber(ByVal CompNumber As Integer)
        Dim strBatchName As String = "UpdateLaneAddressWithCompanyByCompNumber"
        Dim strProcName As String = "dbo.spUpdateLaneAddressWithCompAddress50"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            'Validate the parameter data
            If CompNumber = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The company number is not valid. Please select a valid company record to run this procedure.", "E_DataValidationFailure")
                Return
            End If
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@CompControl", 0)
            oCmd.Parameters.AddWithValue("@CompNumber", CompNumber)
            oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub
    'User Name Parameter Not Needed
    Public Sub UpdateLaneActualMovement()
        Dim strBatchName As String = "UpdateLaneActualMovement"
        Dim strProcName As String = "dbo.spUpdateLaneActualMovement"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub
    'New @UserName parameter added
    Public Sub UpdateLaneTruck(ByVal CarrierTruckControl As Integer, ByVal LaneCarrControl As Integer)
        Dim strBatchName As String = "UpdateLaneTruck"
        Dim strProcName As String = "dbo.spUpdateLaneTruck50"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            'Validate the parameter data
            If CarrierTruckControl = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The carrier truck control number is not valid. Please select a valid record to run this procedure.", "E_DataValidationFailure")
                Return
            End If
            If LaneCarrControl = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The lane carrier control number is not valid. Please select a valid record to run this procedure.", "E_DataValidationFailure")
                Return
            End If
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@CarrierTruckControl", CarrierTruckControl)
            oCmd.Parameters.AddWithValue("@LaneCarrControl", LaneCarrControl)
            oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub
    'User Name Parameter Not Needed
    Public Sub ScrubAddress(ByVal Type As Integer)
        Dim strBatchName As String = "ScrubAddress"
        Dim strProcName As String = "nglspScrub.Net"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Dim sMsg As String = ""
        Try
            nglspScrub(Type, sMsg)
            oSystem.EndtblBatchProcessRunning(Me.Parameters.UserName, strBatchName)
        Catch ex As FaultException
            oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "Fault Exception")
            Throw
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try

        End Try
    End Sub

    Public Function ScrubAddress2Way(ByVal Type As Integer) As Boolean
        Dim strBatchName As String = "ScrubAddress2Way"
        Dim strProcName As String = "nglspScrub.Net"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Dim sMsg As String = ""
        Dim blnRet As Boolean = False
        Try
            nglspScrub(Type, sMsg)
            oSystem.EndtblBatchProcessRunning(Me.Parameters.UserName, strBatchName)
            blnRet = True
        Catch ex As FaultException
            oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "Fault Exception")
            Throw
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try

        Return blnRet
    End Function


    Public Sub nglspScrub(ByVal Type As Integer, ByRef Message As String, Optional ByVal iCompControl As Integer = 0)
        'Note Type values:
        '   0 = Company Street Address
        '   1 = Lane Origin Address
        '   2 = Lane Destination Address
        '   3 = Company Street and Lane Origin Address
        '   4 = Company Street and Lane Origin Address and Lane Destination Address

        Dim strMsg As String = ""
        Dim strSpacer As String = ""
        Dim lngLanesProcessed As Integer = 0
        Dim lngCompsProcessed As Integer = 0
        Dim lngLanesChanged As Integer = 0
        Dim lngCompsChanged As Integer = 0
        Try
            Message = "Success! Data was Scrubbed."
            'Scrub the company street address
            If Type = 0 Or Type = 3 Or Type = 4 Then
                Dim oCompDAL = New NGLCompData(Me.Parameters)
                lngCompsProcessed = oCompDAL.Scrub(iCompControl)
            End If
            'Scrub the lane address
            If Type = 1 Or Type = 2 Or Type = 3 Or Type = 4 Then
                Dim oLaneDAL = New NGLLaneData(Me.Parameters)
                lngLanesProcessed = oLaneDAL.Scrub(iCompControl)
            End If
            strMsg = "Success! "
            strSpacer = ""
            If lngCompsProcessed > 0 Then
                strMsg &= lngCompsProcessed.ToString & " companies were processed"
                strSpacer = ", "
                If lngCompsChanged > 0 Then
                    strMsg &= strSpacer & lngCompsChanged.ToString & " companies changed"
                Else
                    strMsg &= " And no company changes were needed."
                End If
            End If
            If lngLanesProcessed > 0 Then
                strMsg &= strSpacer & lngLanesProcessed.ToString & " lanes were processed"
                strSpacer = ", "
                If lngLanesChanged > 0 Then
                    strMsg &= strSpacer & lngLanesChanged.ToString & " lanes changed"
                Else
                    strMsg &= " And no lane changes were needed."
                End If
            End If
            strMsg &= "."
            Message = strMsg
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            'do nothing
            Message = ex.Message
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Type"></param>
    ''' <param name="Message"></param>
    ''' <param name="iCompControl"></param>
    ''' <remarks>
    ''' Modified by RHR for v-8.2 on 02/24/2021
    '''     Fixed bug where Type integer value of 1 does not match the 
    '''     Enum value in desktop client where Company is zero
    '''      Public Enum AddressCaseTypes
    '''         Company
    '''         Lane
    '''      End Enum
    ''' </remarks>
    Public Sub nglspCaseType(ByVal Type As Integer, ByRef Message As String, Optional ByVal iCompControl As Integer = 0)
        'Note Type values:
        '   0 = Company Address
        '   1 = Lane Address

        Dim strMsg As String = ""
        Dim strSpacer As String = ""
        Dim lngLanesProcessed As Integer = 0
        Dim lngCompsProcessed As Integer = 0
        Dim lngLanesChanged As Integer = 0
        Dim lngCompsChanged As Integer = 0
        Try
            Message = "Success! Data was Scrubbed."

            'Scrub the lane address
            If Type = 1 Then
                ' Modified by RHR for v-8.2 on 02/24/2021
                Dim oLaneDAL = New NGLLaneData(Me.Parameters)
                lngLanesProcessed = oLaneDAL.CaseType(iCompControl)
                'Dim oCompDAL = New NGLCompData(Me.Parameters)
                'lngCompsProcessed = oCompDAL.CaseType(iCompControl)
                'sqlCmd = New SqlCommand("Select LaneControl,isnull(LaneOrigAddress1,'') as LaneOrigAddress1,isnull(LaneDestAddress1,'') as LaneDestAddress1  From dbo.Lane Where LaneControl = 371", sqlCon)
            Else
                ' Modified by RHR for v-8.2 on 02/24/2021
                Dim oCompDAL = New NGLCompData(Me.Parameters)
                lngCompsProcessed = oCompDAL.CaseType(iCompControl)
                'Dim oLaneDAL = New NGLLaneData(Me.Parameters)
                'lngLanesProcessed = oLaneDAL.CaseType(iCompControl)
            End If

            strMsg = "Success! "
            strSpacer = ""
            If lngCompsProcessed > 0 Then
                strMsg &= lngCompsProcessed.ToString & " companies were processed"
                strSpacer = ", "
            End If
            If lngLanesProcessed > 0 Then
                strMsg &= strSpacer & lngLanesProcessed.ToString & " lanes were processed"
            End If
            strMsg &= "."
            Message = strMsg
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            'do nothing
            Message = ex.Message
        End Try
    End Sub


    Public Function updateAllLaneTrucks2Way(ByVal CarrierTruckControl As Integer) As Boolean
        Dim strBatchName As String = "updateAllLaneTrucks"
        Dim strProcName As String = "dbo.spupdateAllLaneTrucks50"
        Dim blnRet As Boolean = False
        'Validate the parameter data
        If CarrierTruckControl = 0 Then Return False

        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@CarrierTruckControl", CarrierTruckControl)
        oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        runNGLStoredProcedure(oCmd, strProcName, 0)
        blnRet = True
        Return blnRet
    End Function

    'New @UserName parameter added
    Public Sub updateAllLaneTrucks(ByVal CarrierTruckControl As Integer)
        Dim strBatchName As String = "updateAllLaneTrucks"
        Dim strProcName As String = "dbo.spupdateAllLaneTrucks50"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            'Validate the parameter data
            If CarrierTruckControl = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The carrier truck control number Is Not valid. Please Select a valid record To run this procedure.", "E_DataValidationFailure")
                Return
            End If
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@CarrierTruckControl", CarrierTruckControl)
            oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub
    'User Name Parameter Not Needed
    Public Sub UpdateBudgetActSales()
        Dim strBatchName As String = "UpdateBudgetActSales"
        Dim strProcName As String = "dbo.spUpdateBudgetActSales"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub

    ''' <summary>
    ''' As of v-6.4 this method has been depreciated.  We now use a BLL overload
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <remarks></remarks>
    Public Sub UpdateBookFuelFee(ByVal BookControl As Integer)
        throwDepreciatedException(buildProcedureName("UpdateBookFuelFee"))
        'Dim strBatchName As String = "UpdateBookFuelFee"
        'Dim strProcName As String = "dbo.spUpdateBookFuelFee50"
        'Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        'Try
        '    'Validate the parameter data
        '    If BookControl = 0 Then
        '        oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The book control number Is Not valid. Please Select a valid booking record To run this procedure.", "E_DataValidationFailure")
        '        Return
        '    End If
        '    Dim oCmd As New System.Data.SqlClient.SqlCommand
        '    oCmd.Parameters.AddWithValue("@BookControl", BookControl)
        '    oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        '    RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        'Catch ex As Exception
        '    Utilities.SaveAppError(ex.Message, Me.Parameters)
        '    Try
        '        oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
        '    Catch e As Exception

        '    End Try
        'End Try
    End Sub

    ''' <summary>
    ''' As of v-6.4 this method has been depreciated.  We now use a BLL overload
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateBookFuelFee2Way(ByVal BookControl As Integer) As Boolean
        throwDepreciatedException(buildProcedureName("UpdateBookFuelFee2Way"))
        Return False
        'Dim strBatchName As String = "UpdateBookFuelFee"
        'Dim strProcName As String = "dbo.spUpdateBookFuelFee50"
        'Dim blnRet As Boolean = False
        ''Validate the parameter data
        'If BookControl = 0 Then Return False

        'Dim oCmd As New System.Data.SqlClient.SqlCommand
        'oCmd.Parameters.AddWithValue("@BookControl", BookControl)
        'oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        'runNGLStoredProcedure(oCmd, strProcName, 0)
        'blnRet = True
        'Return blnRet
    End Function

    ''' <summary>
    ''' As of v-6.4 this method has been depreciated.  We now use a BLL overload
    ''' </summary>
    ''' <param name="CarrierControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateBookFuelFees2Way(ByVal CarrierControl As Integer) As Boolean
        throwDepreciatedException(buildProcedureName("UpdateBookFuelFees2Way"))
        Return False
        'Dim strBatchName As String = "UpdateBookFuelFees"
        'Dim strProcName As String = "dbo.spUpdateBookFuelFees50"
        'Dim blnRet As Boolean = False
        ''Validate the parameter data
        'If CarrierControl = 0 Then Return False

        'Dim oCmd As New System.Data.SqlClient.SqlCommand
        'oCmd.Parameters.AddWithValue("@CarrierControl", CarrierControl)
        'oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        'runNGLStoredProcedure(oCmd, strProcName, 0)
        'blnRet = True
        'Return blnRet
    End Function

    ''' <summary>
    ''' As of v-6.4 this method has been depreciated.  We now use a BLL overload
    ''' </summary>
    ''' <param name="CarrierControl"></param>
    ''' <remarks></remarks>
    Public Sub UpdateBookFuelFees(ByVal CarrierControl As Integer)
        throwDepreciatedException(buildProcedureName("UpdateBookFuelFees"))
        'Dim strBatchName As String = "UpdateBookFuelFees"
        'Dim strProcName As String = "dbo.spUpdateBookFuelFees50"
        'Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        'Try
        '    'Validate the parameter data
        '    If CarrierControl = 0 Then
        '        oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The carrier control number Is Not valid. Please Select a valid carrier record To run this procedure.", "E_DataValidationFailure")
        '        Return
        '    End If
        '    Dim oCmd As New System.Data.SqlClient.SqlCommand
        '    oCmd.Parameters.AddWithValue("@CarrierControl", CarrierControl)
        '    oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        '    RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        'Catch ex As Exception
        '    Utilities.SaveAppError(ex.Message, Me.Parameters)
        '    Try
        '        oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
        '    Catch e As Exception

        '    End Try
        'End Try
    End Sub
    'New @UserName parameter added
    Public Sub updateBookCarrContacts(ByVal CarrierContControl As Integer)
        Dim strBatchName As String = "updateBookCarrContacts"
        Dim strProcName As String = "dbo.spUpdateBookCarrContacts50"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            'Validate the parameter data
            If CarrierContControl = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The carrier contact control number Is Not valid. Please Select a valid carrier contact record To run this procedure.", "E_DataValidationFailure")
                Return
            End If
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@CarrierContControl", CarrierContControl)
            oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub
    'New @UserName parameter added
    Public Sub updateCarrContact(ByVal BookControl As Integer, ByVal CarrierContControl As Integer)
        Dim strBatchName As String = "updateCarrContact"
        Dim strProcName As String = "dbo.spUpdateCarrContact50"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            'Validate the parameter data
            If BookControl = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The book control number Is Not valid. Please Select a valid booking record To run this procedure.", "E_DataValidationFailure")
                Return
            End If
            If CarrierContControl = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The carrier contact control number Is Not valid. Please Select a valid carrier record To run this procedure.", "E_DataValidationFailure")
                Return
            End If
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@BookControl", BookControl)
            oCmd.Parameters.AddWithValue("@CarrierContControl", CarrierContControl)
            oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub

    Public Function updateCarrContact2Way(ByVal BookControl As Integer, ByVal CarrierContControl As Integer) As Boolean
        Dim strBatchName As String = "updateCarrContact"
        Dim strProcName As String = "dbo.spUpdateCarrContact50"
        Dim blnRet As Boolean = False

        'Validate the parameter data
        If BookControl = 0 Then Return False

        If CarrierContControl = 0 Then Return False

        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@BookControl", BookControl)
        oCmd.Parameters.AddWithValue("@CarrierContControl", CarrierContControl)
        oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        runNGLStoredProcedure(oCmd, strProcName, 0)
        blnRet = True
        Return blnRet
    End Function

    'New @UserName parameter added
    Public Sub updateConsCarrier(ByVal BookControl As Integer, ByVal CarrierControl As Integer)
        Dim strBatchName As String = "updateConsCarrier"
        Dim strProcName As String = "dbo.spAssignCarrierCons50"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            'Validate the parameter data
            If BookControl = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The book control number Is Not valid. Please Select a valid booking record To run this procedure.", "E_DataValidationFailure")
                Return
            End If
            If CarrierControl = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The carrier control number Is Not valid. Please Select a valid carrier record To run this procedure.", "E_DataValidationFailure")
                Return
            End If
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@BookControl", BookControl)
            oCmd.Parameters.AddWithValue("@CarrierControl", CarrierControl)
            oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub

    Public Function updateConsCarrier2Way(ByVal BookControl As Integer, ByVal CarrierControl As Integer) As Boolean
        Dim strBatchName As String = "updateConsCarrier"
        Dim strProcName As String = "dbo.spAssignCarrierCons50"
        Dim blnRet As Boolean = False
        'Validate the parameter data
        If BookControl = 0 Then Return False

        If CarrierControl = 0 Then Return False

        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@BookControl", BookControl)
        oCmd.Parameters.AddWithValue("@CarrierControl", CarrierControl)
        oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        runNGLStoredProcedure(oCmd, strProcName, 0)
        blnRet = True
        Return blnRet
    End Function

    'New @UserName parameter added
    Public Sub CalcBookRev(ByVal BookControl As Integer)
        Dim strBatchName As String = "CalcBookRev"
        Dim strProcName As String = "dbo.spCalcBookRev50"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            'Validate the parameter data
            If BookControl = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The book control number Is Not valid. Please Select a valid booking record To run this procedure.", "E_DataValidationFailure")
                Return
            End If
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@BookControl", BookControl)
            oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub
    'New @UserName parameter added
    Public Sub updateConsTranCode(ByVal BookControl As Integer, ByVal TransCode As String)
        Dim strBatchName As String = "updateConsTranCode"
        Dim strProcName As String = "dbo.spUpdateConsTransCode50"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            'Validate the parameter data
            If BookControl = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The booking control number Is Not valid. Please Select a valid booking record To run this procedure.", "E_DataValidationFailure")
                Return
            End If
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@BookControl", BookControl)
            oCmd.Parameters.AddWithValue("@TransCode", Left(TransCode, 3))
            oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub
    'New @UserName parameter added
    Public Sub UpdateCarrierFuelFees(ByVal CarrierControl As Integer)
        Dim strBatchName As String = "UpdateCarrierFuelFees"
        Dim strProcName As String = "dbo.spUpdateCarrierFuelFees50"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            'Validate the parameter data
            If CarrierControl = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The carrier control number Is Not valid. Please Select a valid carrier record To run this procedure.", "E_DataValidationFailure")
                Return
            End If
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@CarrierControl", CarrierControl)
            oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub
    'New @UserName parameter added
    Public Function UpdateCarrierFuelFees2Way(ByVal CarrierControl As Integer) As Boolean
        Dim strBatchName As String = "UpdateCarrierFuelFees"
        Dim strProcName As String = "dbo.spUpdateCarrierFuelFees50"
        Dim blnRet As Boolean = False

        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@CarrierControl", CarrierControl)
        oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        runNGLStoredProcedure(oCmd, strProcName, 0)
        blnRet = True
        Return blnRet

    End Function

    ''' <summary>
    ''' This method has been Depreciated and is no longer used.  
    ''' It has been replaced by NGL.FM.BLL.NGLCarrierBLL.UpdateAllCarrierFuelFees
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Removed by RHR 11/8/13
    ''' </remarks>
    Public Function UpdateAllCarrierFuelFees2Way() As Boolean
        throwDepreciatedException(buildProcedureName("UpdateAllCarrierFuelFees2Way"))
        Return False
        'Dim strBatchName As String = "UpdateAllCarrierFuelFees"
        'Dim strProcName As String = "dbo.spUpdateAllCarrierFuelFees50"
        'Dim blnRet As Boolean = False

        'Dim oCmd As New System.Data.SqlClient.SqlCommand
        'oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        'runNGLStoredProcedure(oCmd, strProcName, 0)
        'blnRet = True
        'Return blnRet

    End Function

    ''' <summary>
    ''' This method has been Depreciated and is no longer used.  
    ''' It has been replaced by NGL.FM.BLL.NGLCarrierBLL.UpdateAllCarrierFuelFees
    ''' </summary>
    ''' <remarks>
    ''' Removed by RHR 11/8/13
    ''' </remarks>
    Public Sub UpdateAllCarrierFuelFees()
        throwDepreciatedException(buildProcedureName("UpdateAllCarrierFuelFees"))
        Return
        'Dim strBatchName As String = "UpdateAllCarrierFuelFees"
        'Dim strProcName As String = "dbo.spUpdateAllCarrierFuelFees50"
        'Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        'Try
        '    Dim oCmd As New System.Data.SqlClient.SqlCommand
        '    oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        '    RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        'Catch ex As Exception
        '    Utilities.SaveAppError(ex.Message, Me.Parameters)
        '    Try
        '        oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
        '    Catch e As Exception

        '    End Try
        'End Try
    End Sub
    'User Name Parameter Not Needed
    Public Sub UpdateCreditRoutine()
        Dim strBatchName As String = "UpdateCreditRoutine"
        Dim strProcName As String = "dbo.spUpdateCreditRoutine"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub
    'New @UserName parameter added
    Public Sub UpdateFrtBillNumber(ByVal BookControl As Integer, ByVal BookFinAPBillNumber As String, ByVal BookFinAPBillNoDate As Date)
        Dim strBatchName As String = "UpdateFrtBillNumber"
        Dim strProcName As String = "dbo.spUpdateFrtBillNumber50"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            'Validate the parameter data
            If BookControl = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The booking control number Is Not valid. Please Select a valid booking record To run this procedure.", "E_DataValidationFailure")
                Return
            End If
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@BookControl", BookControl)
            oCmd.Parameters.AddWithValue("@BookFinAPBillNumber", Left(BookFinAPBillNumber, 50))
            oCmd.Parameters.AddWithValue("@BookFinAPBillNoDate", BookFinAPBillNoDate)
            oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub

    Public Function UpdateFrtBillNumber2Way(ByVal BookControl As Integer, ByVal BookFinAPBillNumber As String, ByVal BookFinAPBillNoDate As Date) As Boolean
        Dim strBatchName As String = "UpdateFrtBillNumber"
        Dim strProcName As String = "dbo.spUpdateFrtBillNumber50"
        Dim blnRet As Boolean = False
        'Validate the parameter data
        If BookControl = 0 Then Return False

        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@BookControl", BookControl)
        oCmd.Parameters.AddWithValue("@BookFinAPBillNumber", Left(BookFinAPBillNumber, 50))
        oCmd.Parameters.AddWithValue("@BookFinAPBillNoDate", BookFinAPBillNoDate)
        oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        runNGLStoredProcedure(oCmd, strProcName, 0)
        blnRet = True
        Return blnRet
    End Function

    'New @UserName parameter added
    Public Function InvoiceReversal(ByVal InvoiceDate As Date, Optional ByVal OneWay As Boolean = True) As Boolean
        Dim blnRet As Boolean = False
        Dim strBatchName As String = "InvoiceReversal"
        Dim strProcName As String = "dbo.InvoiceReverse50"
        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@InvoiceDate", InvoiceDate)
        oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        If OneWay Then
            blnRet = True
            Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
            Try
                RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Try
                    oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
                Catch e As Exception

                End Try
            End Try
        Else
            runNGLStoredProcedure(oCmd, strProcName, 0)
            blnRet = True
        End If
        Return blnRet
    End Function
    'New @UserName parameter added
    Public Function InvoiceUpdateNToIC(ByVal FromCompNumber As Integer, ByVal ToCompNumber As Integer, Optional ByVal OneWay As Boolean = True) As Boolean
        Dim blnRet As Boolean = False
        Dim strBatchName As String = "InvoiceUpdateNToIC"
        Dim strProcName As String = "dbo.InvoiceUpdateNToIC50"
        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@FromCompNumber", FromCompNumber)
        oCmd.Parameters.AddWithValue("@ToCompNumber", ToCompNumber)
        oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        If OneWay Then
            blnRet = True
            Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
            Try
                'Validate the parameter data
                If FromCompNumber = 0 Then
                    oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The from company number Is Not valid. Please Select a valid company record To run this procedure.", "E_DataValidationFailure")
                    Return False
                End If
                If ToCompNumber = 0 Then
                    oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The To company number Is Not valid. Please Select a valid company record To run this procedure.", "E_DataValidationFailure")
                    Return False
                End If
                RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Try
                    oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
                Catch e As Exception

                End Try
            End Try
        Else
            'Validate the parameter data
            If FromCompNumber = 0 Then Return False
            If ToCompNumber = 0 Then Return False
            runNGLStoredProcedure(oCmd, strProcName, 0)
            blnRet = True
        End If
        Return blnRet
    End Function
    'New @UserName parameter added
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="InvoiceDate"></param>
    ''' <param name="OneWay"></param>
    ''' <returns>Boolean</returns>
    ''' <remarks>
    ''' Modified By LVV 2/29/16 for v-7.0.5.1 EDI Migration
    ''' Now calls spInvoiceUpdate50210Off
    ''' </remarks>
    Public Function UpdateInvoice(ByVal InvoiceDate As Date, Optional ByVal OneWay As Boolean = True) As Boolean
        Dim blnRet As Boolean = False
        Dim strBatchName As String = "UpdateInvoice"
        Dim strProcName As String = "dbo.spInvoiceUpdate50210Off" 'Modified By LVV 2/29/16 for v-7.0.5.1 EDI Migration
        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@InvoiceDate", InvoiceDate)
        oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        If OneWay Then
            blnRet = True
            Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
            Try
                RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Try
                    oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
                Catch e As Exception

                End Try
            End Try
        Else
            runNGLStoredProcedure(oCmd, strProcName, 0)
            blnRet = True
        End If
        Return blnRet
    End Function

    'New @UserName parameter added
    Public Sub LoadsNToPCBackhaulsOnly(ByVal FromLoadDate As Date, ByVal ToLoadDate As Date, ByVal FromCompNumber As Integer, ByVal ToCompNumber As Integer)
        Dim strBatchName As String = "LoadsNToPCBackhaulsOnly"
        Dim strProcName As String = "dbo.LoadsNToPCBackhaulsOnly50"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            'Validate the parameter data
            If FromCompNumber = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The from company number Is Not valid. Please Select a valid company record To run this procedure.", "E_DataValidationFailure")
                Return
            End If
            If ToCompNumber = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The To company number Is Not valid. Please Select a valid company record To run this procedure.", "E_DataValidationFailure")
                Return
            End If

            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@FromLoadDate", FromLoadDate)
            oCmd.Parameters.AddWithValue("@ToLoadDate", ToLoadDate)
            oCmd.Parameters.AddWithValue("@FromCompNumber", FromCompNumber)
            oCmd.Parameters.AddWithValue("@ToCompNumber", ToCompNumber)
            oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub
    'New @UserName parameter added
    Public Sub LoadsNToIBackhauls(ByVal FromCompNumber As Integer, ByVal ToCompNumber As Integer)
        Dim strBatchName As String = "LoadsNToIBackhauls"
        Dim strProcName As String = "dbo.InvoiceUpdateNToIBackhauls50"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            'Validate the parameter data
            If FromCompNumber = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The from company number Is Not valid. Please Select a valid company record To run this procedure.", "E_DataValidationFailure")
                Return
            End If
            If ToCompNumber = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The To company number Is Not valid. Please Select a valid company record To run this procedure.", "E_DataValidationFailure")
                Return
            End If

            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@FromCompNumber", FromCompNumber)
            oCmd.Parameters.AddWithValue("@ToCompNumber", ToCompNumber)
            oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub
    'New @UserName parameter added
    Public Sub InvoiceUpdateNToICBackhauls(ByVal FromCompNumber As Integer, ByVal ToCompNumber As Integer)
        Dim strBatchName As String = "InvoiceUpdateNToICBackhauls"
        Dim strProcName As String = "dbo.InvoiceUpdateNToICBackhauls50"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            'Validate the parameter data
            If FromCompNumber = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The from company number Is Not valid. Please Select a valid company record To run this procedure.", "E_DataValidationFailure")
                Return
            End If
            If ToCompNumber = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The To company number Is Not valid. Please Select a valid company record To run this procedure.", "E_DataValidationFailure")
                Return
            End If

            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@FromCompNumber", FromCompNumber)
            oCmd.Parameters.AddWithValue("@ToCompNumber", ToCompNumber)
            oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub
    'New @UserName parameter added
    Public Function LoadsNToPCNoBackhauls(ByVal ToLoadDate As Date,
                                          ByVal FromCompNumber As Integer,
                                          ByVal ToCompNumber As Integer,
                                          Optional ByVal OneWay As Boolean = True) As Boolean
        Dim blnRet As Boolean = False
        Dim strBatchName As String = "LoadsNToPCNoBackhauls"
        Dim strProcName As String = "dbo.LoadsNToPCNoBackhauls50"
        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@ToLoadDate", ToLoadDate)
        oCmd.Parameters.AddWithValue("@FromCompNumber", FromCompNumber)
        oCmd.Parameters.AddWithValue("@ToCompNumber", ToCompNumber)
        oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        If OneWay Then
            blnRet = True
            Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
            Try
                'Validate the parameter data
                If FromCompNumber = 0 Then
                    oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The from company number Is Not valid. Please Select a valid company record To run this procedure.", "E_DataValidationFailure")
                    Return False
                End If
                If ToCompNumber = 0 Then
                    oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The To company number Is Not valid. Please Select a valid company record To run this procedure.", "E_DataValidationFailure")
                    Return False
                End If
                RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Try
                    oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
                Catch e As Exception

                End Try
            End Try
        Else
            'Validate the parameter data
            If FromCompNumber = 0 Then Return False
            If ToCompNumber = 0 Then Return False
            runNGLStoredProcedure(oCmd, strProcName, 0)
            blnRet = True
        End If
        Return blnRet
    End Function
    'New @UserName parameter added
    Public Function LoadsPrinted(Optional ByVal OneWay As Boolean = True) As Boolean
        Dim blnRet As Boolean = False
        Dim strBatchName As String = "LoadsPrinted"
        Dim strProcName As String = "dbo.LoadsPrinted50"
        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        If OneWay Then
            blnRet = True
            Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
            Try
                RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Try
                    oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
                Catch e As Exception

                End Try
            End Try
        Else
            runNGLStoredProcedure(oCmd, strProcName, 0)
            blnRet = True
        End If
        Return blnRet
    End Function
    'New @UserName parameter added
    Public Function LoadsReadyToInvoice(ByVal FromLoadDate As Date, ByVal ToLoadDate As Date, Optional ByVal OneWay As Boolean = True) As Boolean
        Dim blnRet As Boolean = False
        Dim strBatchName As String = "LoadsReadyToInvoice"
        Dim strProcName As String = "dbo.LoadsReadyToInvoice50"
        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@FromLoadDate", FromLoadDate)
        oCmd.Parameters.AddWithValue("@ToLoadDate", ToLoadDate)
        oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        If OneWay Then
            blnRet = True
            Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
            Try
                RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Try
                    oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
                Catch e As Exception

                End Try
            End Try
        Else
            blnRet = False
            runNGLStoredProcedure(oCmd, strProcName, 0)
            blnRet = True
        End If
        Return blnRet
    End Function
    'User Name Parameter Not Needed
    Public Sub CaseTypeAddress(ByVal Type As Integer)
        Dim strBatchName As String = "CaseTypeAddress"
        Dim strProcName As String = "dbo.nglspCaseType"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Dim sMsg As String = ""
        Try
            nglspCaseType(Type, sMsg)
            oSystem.EndtblBatchProcessRunning(Me.Parameters.UserName, strBatchName)
        Catch ex As FaultException
            oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "Fault Exception")
            Throw
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub

    Public Function CaseTypeAddress2Way(ByVal Type As Integer) As Boolean
        Dim strBatchName As String = "CaseTypeAddress2Way"
        Dim strProcName As String = "nglspCaseType.Net"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Dim sMsg As String = ""
        Dim blnRet As Boolean = False
        Try
            nglspCaseType(Type, sMsg)
            oSystem.EndtblBatchProcessRunning(Me.Parameters.UserName, strBatchName)
            blnRet = True
        Catch ex As FaultException
            oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "Fault Exception")
            Throw
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try

        Return blnRet
    End Function

    'User Name Parameter Not Needed
    Public Sub clearBookingCarrier(ByVal BookControl As Integer, ByVal ClearCNS As Boolean)
        Dim strBatchName As String = "clearBookingCarrier"
        Dim strProcName As String = "dbo.spClearCarrierCons"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            'Validate the parameter data
            If BookControl = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The booking control number Is Not valid. Please Select a valid booking record To run this procedure.", "E_DataValidationFailure")
                Return
            End If
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@BookControl", BookControl)
            oCmd.Parameters.AddWithValue("@ClearCNS", ClearCNS)
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub

    ''' <summary>
    ''' This method has been Depreciated and is no longer used
    ''' </summary>
    ''' <param name="BookLoadControl"></param>
    ''' <remarks>
    ''' Removed by RHR 10/3/13 for v-6.3
    ''' </remarks>
    Public Sub UpdateBookItemTotals(ByVal BookLoadControl As Integer)
        throwDepreciatedException(buildProcedureName("UpdateBookItemTotals"))
        Return
    End Sub

    ''' <summary>
    ''' This method has been Depreciated and is no longer used
    ''' </summary>
    ''' <param name="BookLoadControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Removed by RHR 10/3/13 for v-6.3
    ''' </remarks>
    Public Function UpdateBookItemTotals2Way(ByVal BookLoadControl As Integer) As Boolean
        throwDepreciatedException(buildProcedureName("UpdateBookItemTotals2Way"))
        Return False
    End Function

    ''' <summary>
    ''' This method has been Depreciated and is no longer used
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Removed by RHR 10/3/13 for v-6.3
    ''' </remarks>
    Public Function UpdateBookCapacityTotals2Way(ByVal BookControl As Integer) As Boolean
        throwDepreciatedException(buildProcedureName("UpdateBookCapacityTotals2Way"))
        Return False
    End Function

    Public Sub UpdateLaneGeoData2Way(ByVal LaneControl As Integer, ByVal dblLat As Double, ByVal dblLong As Double, ByVal Miles As Double)
        'we only update data if the values are non zero
        Dim strSQL As String = "Update dbo.Lane Set "
        Dim strSpacer As String = ""
        Dim blnUpdateValid As Boolean = False
        If dblLat <> 0 Then
            blnUpdateValid = True
            strSQL &= "LaneLatitude = " & dblLat
            strSpacer = ", "
        End If
        If dblLong <> 0 Then
            blnUpdateValid = True
            strSQL &= strSpacer & "LaneLongitude = " & dblLong
            strSpacer = ", "
        Else
            strSpacer = " "
        End If
        If Miles <> 0 Then
            blnUpdateValid = True
            strSQL &= strSpacer & "LaneBenchMiles = " & Miles
        End If
        If blnUpdateValid Then
            executeSQL(strSQL & " Where LaneControl = " & LaneControl)
        End If

    End Sub

#End Region

#Region "General Processing Stored Procedures"
    'New @UserName parameter added
    Public Sub updateLaneSpecialCodes(ByVal LaneControl As Integer)
        Dim strBatchName As String = "updateLaneSpecialCodes"
        Dim strProcName As String = "dbo.spUpdateLaneSpecialCodes50"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            'Validate the parameter data
            If LaneControl = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The lane control number Is Not valid. Please Select a valid lane record To run this procedure.", "E_DataValidationFailure")
                Return
            End If
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@LaneControl", LaneControl)
            oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub

    Public Function finalizeBooking(ByVal BookControl As Integer, ByVal BookConsPrefix As String, Optional ByVal TwoWay As Boolean = False) As Boolean
        Dim spConfig As New clsNGLSPConfig("finalizeBooking", "dbo.spFinalizeBooking50", TwoWay)
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        With spConfig
            If BookControl = 0 Then
                If Not TwoWay Then oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, .strBatchName, "The book control number Is Not valid. Please Select a valid book record To run this procedure.", "E_DataValidationFailure")
                Return False
            End If
            With .oCmd
                .Parameters.AddWithValue("@BookControl", BookControl)
                .Parameters.AddWithValue("@BookConsPrefix", Left(BookConsPrefix, 20))
                .Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))

            End With
        End With
        Return processNGLStoredProcedure(spConfig, oSystem)

    End Function

    Public Sub generatePickListRecord(ByVal BookControl As Integer, ByVal BookConsPrefix As String)
        Dim strBatchName As String = "generatePickListRecord"
        Dim strProcName As String = "dbo.spInsertPickList50"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            'Validate the parameter data
            If BookControl = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The book control number Is Not valid. Please Select a valid book record To run this procedure.", "E_DataValidationFailure")
                Return
            End If
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@BookControl", BookControl)
            oCmd.Parameters.AddWithValue("@BookConsPrefix", Left(BookConsPrefix, 20))
            oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub

    Public Function generatePickListRecord2Way(ByVal BookControl As Integer, ByVal BookConsPrefix As String) As Boolean
        Dim strBatchName As String = "generatePickListRecord"
        Dim strProcName As String = "dbo.spInsertPickList50"
        Dim blnRet As Boolean = False
        'Validate the parameter data
        If BookControl = 0 Then Return False

        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@BookControl", BookControl)
        oCmd.Parameters.AddWithValue("@BookConsPrefix", Left(BookConsPrefix, 20))
        oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        runNGLStoredProcedure(oCmd, strProcName, 0)
        blnRet = True
        Return blnRet
    End Function

    'User Name Parameter Not Needed
    Public Sub executeGenerateTenderEmailProcedure(ByVal BookControl As Integer,
                                                   ByVal CarrierControl As Integer,
                                                   ByVal BookProNumber As String,
                                                   ByVal BookConsPrefix As String,
                                                   ByVal EmailTo As String,
                                                   ByVal CCEmail As String,
                                                   ByVal Subject As String,
                                                   ByVal Body As String)
        Dim strBatchName As String = "executeGenerateTenderEmailProcedure"
        Dim strProcName As String = "dbo.spGenerateTenderEmail"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            'Validate the parameter data
            If BookControl = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The book control number Is Not valid. Please Select a valid book record To run this procedure.", "E_DataValidationFailure")
                Return
            End If
            If CarrierControl = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The carrier control number Is Not valid. Please Select a valid carrier record To run this procedure.", "E_DataValidationFailure")
                Return
            End If
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@BookControl", BookControl)
            oCmd.Parameters.AddWithValue("@CarrierControl", CarrierControl)
            oCmd.Parameters.AddWithValue("@BookProNumber", Left(BookProNumber, 20))
            oCmd.Parameters.AddWithValue("@BookConsPrefix", Left(BookConsPrefix, 20))
            oCmd.Parameters.AddWithValue("@EmailTo", Left(EmailTo, 500))
            oCmd.Parameters.AddWithValue("@CCEmail", Left(CCEmail, 500))
            oCmd.Parameters.AddWithValue("@Subject", Left(Subject, 100))
            oCmd.Parameters.AddWithValue("@Body", Left(Body, 4000))
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub

    Public Function executeGenerateTenderEmailProcedure2Way(ByVal BookControl As Integer,
                                                   ByVal CarrierControl As Integer,
                                                   ByVal BookProNumber As String,
                                                   ByVal BookConsPrefix As String,
                                                   ByVal EmailTo As String,
                                                   ByVal CCEmail As String,
                                                   ByVal Subject As String,
                                                   ByVal Body As String) As Boolean
        Dim strBatchName As String = "executeGenerateTenderEmailProcedure"
        Dim strProcName As String = "dbo.spGenerateTenderEmail"
        Dim blnRet As Boolean = False
        'Validate the parameter data
        If BookControl = 0 Then Return False

        If CarrierControl = 0 Then Return False

        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@BookControl", BookControl)
        oCmd.Parameters.AddWithValue("@CarrierControl", CarrierControl)
        oCmd.Parameters.AddWithValue("@BookProNumber", Left(BookProNumber, 20))
        oCmd.Parameters.AddWithValue("@BookConsPrefix", Left(BookConsPrefix, 20))
        oCmd.Parameters.AddWithValue("@EmailTo", Left(EmailTo, 500))
        oCmd.Parameters.AddWithValue("@CCEmail", Left(CCEmail, 500))
        oCmd.Parameters.AddWithValue("@Subject", Left(Subject, 100))
        oCmd.Parameters.AddWithValue("@Body", Left(Body, 4000))
        runNGLStoredProcedure(oCmd, strProcName, 0)
        blnRet = True
        Return blnRet
    End Function

    ''' <summary> 
    ''' Sample Parameter Data:
    ''' @MailFrom = dbo.getParText ('SysEmailNoticeFrom')
    ''' @EmailTo = 'system@nextgeneration.com; support@nextgeneration.com'
    ''' @CCEmail = 'system@nextgeneration.com; support@nextgeneration.com',
    ''' @Subject = 'Limit to 100 characters'
    ''' @Body = 'Limit to 4000 characters'
    ''' @ReportPath = '/FMStdReports/ReportName_limited_to_255_Characters'
    ''' @ReportParameters = 'Enter_Pro_Number^VJI123456|Enter_Consolidation_Number^CNS456' NOTE: limited to 1000 characters
    ''' @ReportFileName = 'Limit_To_500_Characters.pdf'  Note should be unique some how.
    ''' </summary>
    ''' <param name="MailFrom"></param>
    ''' <param name="EmailTo"></param>
    ''' <param name="CCEmail"></param>
    ''' <param name="Subject"></param>
    ''' <param name="Body"></param>
    ''' <param name="ReportPath"></param>
    ''' <param name="ReportParameters"></param>
    ''' <param name="ReportFileName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function executeGenerateEmailWReport2Way(ByVal MailFrom As String,
                                                   ByVal EmailTo As String,
                                                   ByVal CCEmail As String,
                                                   ByVal Subject As String,
                                                   ByVal Body As String,
                                                   ByVal ReportPath As String,
                                                   ByVal ReportParameters As String,
                                                   ByVal ReportFileName As String) As Boolean


        Dim strBatchName As String = "executeGenerateEmailWReport"
        Dim strProcName As String = "dbo.spGenerateEmailWReport"
        Dim blnRet As Boolean = False
        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@MailFrom", Left(MailFrom, 255))
        oCmd.Parameters.AddWithValue("@EmailTo", Left(EmailTo, 500))
        oCmd.Parameters.AddWithValue("@CCEmail", Left(CCEmail, 500))
        oCmd.Parameters.AddWithValue("@Subject", Left(Subject, 100))
        oCmd.Parameters.AddWithValue("@Body", Left(Body, 4000))
        oCmd.Parameters.AddWithValue("@ReportPath", Left(ReportPath, 255))
        oCmd.Parameters.AddWithValue("@ReportParameters", Left(ReportParameters, 1000))
        oCmd.Parameters.AddWithValue("@ReportFileName", Left(ReportFileName, 500))
        oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        runNGLStoredProcedure(oCmd, strProcName, 0)
        blnRet = True
        Return blnRet
    End Function

    ''' <summary>
    ''' Sample Parameter Data:
    ''' @MailFrom = dbo.getParText ('SysEmailNoticeFrom')
    ''' @EmailTo = 'system@nextgeneration.com; support@nextgeneration.com'
    ''' @CCEmail = 'system@nextgeneration.com; support@nextgeneration.com',
    ''' @Subject = 'Limit to 100 characters'
    ''' @Body = 'Limit to 4000 characters'
    ''' </summary>
    ''' <param name="MailFrom"></param>
    ''' <param name="EmailTo"></param>
    ''' <param name="CCEmail"></param>
    ''' <param name="Subject"></param>
    ''' <param name="Body"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function executeGenerateEmail2Way(ByVal MailFrom As String,
                                                   ByVal EmailTo As String,
                                                   ByVal CCEmail As String,
                                                   ByVal Subject As String,
                                                   ByVal Body As String) As Boolean


        Dim strBatchName As String = "executeGenerateEmail"
        Dim strProcName As String = "dbo.spGenerateEmail"
        Dim blnRet As Boolean = False
        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@MailFrom", Left(MailFrom, 255))
        oCmd.Parameters.AddWithValue("@EmailTo", Left(EmailTo, 500))
        oCmd.Parameters.AddWithValue("@CCEmail", Left(CCEmail, 500))
        oCmd.Parameters.AddWithValue("@Subject", Left(Subject, 100))
        oCmd.Parameters.AddWithValue("@Body", Left(Body, 4000))
        oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        runNGLStoredProcedure(oCmd, strProcName, 0)
        blnRet = True
        Return blnRet
    End Function


    ''' <summary>
    ''' Used to send an alert message to the client systems
    ''' </summary>
    ''' <param name="ProcedureName"></param>
    ''' <param name="CompControl"></param>
    ''' <param name="CompNumber"></param>
    ''' <param name="CarrierControl"></param>
    ''' <param name="CarrierNumber"></param>
    ''' <param name="Subject"></param>
    ''' <param name="Body"></param>
    ''' <param name="Note1"></param>
    ''' <param name="Note2"></param>
    ''' <param name="Note3"></param>
    ''' <param name="Note4"></param>
    ''' <param name="Note5"></param>
    ''' <param name="TwoWay"></param>
    ''' <returns></returns>
    ''' <remarks>
    '''  Modified by RHR for v-7.0.6.104 on 04/18/2017
    '''   added truncation logic to Subject and Notes 1 through 5 to deal with 255 character limit
    ''' Modified by RHR for v-8.0 on 01/15/2018
    '''   we now call NGLtblAlertMessageData.InsertAlertMessageNoEmail method merged with v-7.0.6.105 changes
    ''' </remarks>
    Public Function executeInsertAlertMessageNoEmail(ByVal ProcedureName As String,
                                                     ByVal CompControl As Integer,
                                                     ByVal CompNumber As Integer,
                                                     ByVal CarrierControl As Integer,
                                                     ByVal CarrierNumber As Integer,
                                                     ByVal Subject As String,
                                                     ByVal Body As String,
                                                     ByVal Note1 As String,
                                                     ByVal Note2 As String,
                                                     ByVal Note3 As String,
                                                     ByVal Note4 As String,
                                                     ByVal Note5 As String,
                                                     Optional ByVal TwoWay As Boolean = False) As Boolean

        Dim target As New NGLtblAlertMessageData(Me.Parameters)
        Return target.InsertAlertMessageNoEmail(ProcedureName, ProcedureName, Left(Subject, 254), Body, CompControl, CompNumber, CarrierControl, CarrierNumber, Left(Note1, 254), Left(Note2, 254), Left(Note3, 254), Left(Note4, 254), Left(Note5, 254), True)

    End Function


    ''' <summary>
    ''' Used to send an alert message to the client systems
    ''' </summary>
    ''' <param name="ProcedureName"></param>
    ''' <param name="CompControl"></param>
    ''' <param name="CompNumber"></param>
    ''' <param name="CarrierControl"></param>
    ''' <param name="CarrierNumber"></param>
    ''' <param name="Subject"></param>
    ''' <param name="Body"></param>
    ''' <param name="Note1"></param>
    ''' <param name="Note2"></param>
    ''' <param name="Note3"></param>
    ''' <param name="Note4"></param>
    ''' <param name="Note5"></param>
    ''' <param name="TwoWay"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.104 on 04/18/2017
    '''   added truncation logic to Subject and Notes 1 through 5 to deal with 255 character limit
    '''   modified query to use LTS object instead of the NGL stored procedure logic
    '''   because the spInsertAlertMessage is not configured as an NGL stored procedure
    ''' Modified by RHR for v-8.0 on 01/15/2018
    '''   we now call NGLtblAlertMessageData.InsertAlertMessageNoEmail method merged with v-7.0.6.105 changes
    ''' </remarks>
    Public Function executeInsertAlertMessage(ByVal ProcedureName As String,
                                                     ByVal CompControl As Integer,
                                                     ByVal CompNumber As Integer,
                                                     ByVal CarrierControl As Integer,
                                                     ByVal CarrierNumber As Integer,
                                                     ByVal Subject As String,
                                                     ByVal Body As String,
                                                     ByVal Note1 As String,
                                                     ByVal Note2 As String,
                                                     ByVal Note3 As String,
                                                     ByVal Note4 As String,
                                                     ByVal Note5 As String,
                                                     Optional ByVal TwoWay As Boolean = False) As Boolean

        Dim target As New NGLtblAlertMessageData(Me.Parameters)
        Return target.InsertAlertMessage(ProcedureName, ProcedureName, Left(Subject, 254), Body, CompControl, CompNumber, CarrierControl, CarrierNumber, Left(Note1, 254), Left(Note2, 254), Left(Note3, 254), Left(Note4, 254), Left(Note5, 254), True)

    End Function

    'New @UserName parameter added
    Public Function executeReLockProcedure(ByVal BookControl As Integer, ByVal BookConsPrefix As String, Optional ByVal TwoWay As Boolean = False) As Boolean
        Dim spConfig As New clsNGLSPConfig("executeReLockProcedure", "dbo.spReLockBooking50", TwoWay)
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        With spConfig
            'Validate the parameter data
            If BookControl = 0 Then
                If Not TwoWay Then oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, .strBatchName, "The book control number Is Not valid. Please Select a valid book record To run this procedure.", "E_DataValidationFailure")
                Return False
            End If
            With .oCmd
                .Parameters.AddWithValue("@BookControl", BookControl)
                .Parameters.AddWithValue("@BookConsPrefix", Left(BookConsPrefix, 20))
                .Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
            End With
        End With
        Return processNGLStoredProcedure(spConfig, oSystem)
    End Function

    'New @UserName parameter added
    Public Function executeUnLockProcedure(ByVal BookControl As Integer, ByVal BookConsPrefix As String, Optional ByVal TwoWay As Boolean = False) As Boolean
        Dim spConfig As New clsNGLSPConfig("executeUnLockProcedure", "dbo.spUnLockBooking50", TwoWay)
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        With spConfig
            'Validate the parameter data
            If BookControl = 0 Then
                If Not TwoWay Then oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, .strBatchName, "The book control number Is Not valid. Please Select a valid book record To run this procedure.", "E_DataValidationFailure")
                Return False
            End If
            With .oCmd
                .Parameters.AddWithValue("@BookControl", BookControl)
                .Parameters.AddWithValue("@BookConsPrefix", Left(BookConsPrefix, 20))
                .Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
            End With
        End With
        Return processNGLStoredProcedure(spConfig, oSystem)
    End Function

    Public Function undofinalizeBooking(ByVal BookControl As Integer, ByVal BookConsPrefix As String, Optional ByVal TwoWay As Boolean = False) As Boolean
        Dim spConfig As New clsNGLSPConfig("undofinalizeBooking", "dbo.spUnFinalizeBooking50", TwoWay)
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        With spConfig
            If BookControl = 0 Then
                If Not TwoWay Then oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, .strBatchName, "The book control number Is Not valid. Please Select a valid book record To run this procedure.", "E_DataValidationFailure")
                Return False
            End If
            With .oCmd
                .Parameters.AddWithValue("@BookControl", BookControl)
                .Parameters.AddWithValue("@BookConsPrefix", Left(BookConsPrefix, 20))
                .Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))

            End With
        End With
        Return processNGLStoredProcedure(spConfig, oSystem)
    End Function

    Public Sub UpdateTruckStopData(ByVal StopName As String,
                                   ByVal ID1 As String,
                                   ByVal ID2 As String,
                                   ByVal TruckID As String,
                                   ByVal SeqNbr As Integer,
                                   ByVal DistToPrev As Double,
                                   ByVal TotalRouteCost As Double,
                                   ByVal ConsNumber As String)
        'ID1 = BookLoadControl
        Dim strBatchName As String = "UpdateTruckStopData"
        Dim strProcName As String = "dbo.spUpdateTruckStopData50"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try

            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@StopName", Left(StopName, 20))
            oCmd.Parameters.AddWithValue("@ID1", Left(ID1, 15))
            oCmd.Parameters.AddWithValue("@ID2", Left(ID2, 10))
            oCmd.Parameters.AddWithValue("@TruckID", Left(TruckID, 25))
            oCmd.Parameters.AddWithValue("@SeqNbr", SeqNbr)
            oCmd.Parameters.AddWithValue("@DistToPrev", DistToPrev)
            oCmd.Parameters.AddWithValue("@TotalRouteCost", TotalRouteCost)
            oCmd.Parameters.AddWithValue("@ConsNumber", Left(ConsNumber, 20))
            oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub

    'New @UserName parameter added
    Public Sub updateBookingSpecialCodes(ByVal BookControl As Integer)
        Dim strBatchName As String = "updateBookingSpecialCodes"
        Dim strProcName As String = "dbo.spUpdateBookSpecialCodes50"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            'Validate the parameter data
            If BookControl = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The book control number Is Not valid. Please Select a valid book record To run this procedure.", "E_DataValidationFailure")
                Return
            End If
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@BookControl", BookControl)
            oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub

    ''' <summary>
    ''' This method has been Depreciated and is no longer used
    ''' </summary>
    ''' <param name="StopName"></param>
    ''' <param name="ID1"></param>
    ''' <param name="ID2"></param>
    ''' <param name="TruckID"></param>
    ''' <param name="SeqNbr"></param>
    ''' <param name="DistToPrev"></param>
    ''' <param name="TotalRouteCost"></param>
    ''' <param name="ConsNumber"></param>
    ''' <remarks>Removed by RHR v-6.3 no longer supported</remarks>
    Public Sub AssignTruckStopCarrier(ByVal StopName As String,
                                   ByVal ID1 As String,
                                   ByVal ID2 As String,
                                   ByVal TruckID As String,
                                   ByVal SeqNbr As Integer,
                                   ByVal DistToPrev As Double,
                                   ByVal TotalRouteCost As Double,
                                   ByVal ConsNumber As String)
        throwDepreciatedException(buildProcedureName("AssignTruckStopCarrier"))
    End Sub
    '
    'New @UserName parameter added
    Public Sub UpdateBookConsDataPCMiler(ByVal StopName As String,
                                   ByVal ID1 As String,
                                   ByVal ID2 As String,
                                   ByVal TruckID As String,
                                   ByVal SeqNbr As Integer,
                                   ByVal DistToPrev As Double,
                                   ByVal TotalRouteCost As Double,
                                   ByVal ConsNumber As String)

        Dim strBatchName As String = "UpdateBookConsDataPCMiler"
        Dim strProcName As String = "dbo.spUpdateBookConsDataPCMiler50"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try

            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@StopName", Left(StopName, 20))
            oCmd.Parameters.AddWithValue("@ID1", Left(ID1, 15))
            oCmd.Parameters.AddWithValue("@ID2", Left(ID2, 10))
            oCmd.Parameters.AddWithValue("@TruckID", Left(TruckID, 25))
            oCmd.Parameters.AddWithValue("@SeqNbr", SeqNbr)
            oCmd.Parameters.AddWithValue("@TotalRouteCost", TotalRouteCost)
            oCmd.Parameters.AddWithValue("@DistToPrev", DistToPrev)
            oCmd.Parameters.AddWithValue("@ConsNumber", Left(ConsNumber, 20))
            oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub

    Public Function UpdateBookConsDataPCMiler2Way(ByVal StopName As String,
                                   ByVal ID1 As String,
                                   ByVal ID2 As String,
                                   ByVal TruckID As String,
                                   ByVal SeqNbr As Integer,
                                   ByVal DistToPrev As Double,
                                   ByVal TotalRouteCost As Double,
                                   ByVal ConsNumber As String) As Boolean

        Dim strBatchName As String = "UpdateBookConsDataPCMiler"
        Dim strProcName As String = "dbo.spUpdateBookConsDataPCMiler50"
        Dim blnRet As Boolean = False

        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@StopName", Left(StopName, 20))
        oCmd.Parameters.AddWithValue("@ID1", Left(ID1, 15))
        oCmd.Parameters.AddWithValue("@ID2", Left(ID2, 10))
        oCmd.Parameters.AddWithValue("@TruckID", Left(TruckID, 25))
        oCmd.Parameters.AddWithValue("@SeqNbr", SeqNbr)
        oCmd.Parameters.AddWithValue("@TotalRouteCost", TotalRouteCost)
        oCmd.Parameters.AddWithValue("@DistToPrev", DistToPrev)
        oCmd.Parameters.AddWithValue("@ConsNumber", Left(ConsNumber, 20))
        oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        runNGLStoredProcedure(oCmd, strProcName, 0)
        blnRet = True
        Return blnRet
    End Function

    Public Function UpdateBookConsMultiPickPCMiler(ByVal BookControl As Integer,
                                              ByVal LocationIsOrigin As Boolean,
                                              ByVal StopNumber As Integer,
                                              ByVal Miles As Double,
                                              ByVal PCMCost As Double,
                                              ByVal PCMTime As String,
                                              ByVal PCMTolls As Decimal,
                                              ByVal PCMESTCHG As Double,
                                              Optional ByVal TwoWay As Boolean = False) As Boolean

        Dim spConfig As New clsNGLSPConfig("UpdateBookConsMultiPickPCMiler", "dbo.spUpdateBookConsMultiPickPCMiler", TwoWay)
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        With spConfig.oCmd.Parameters
            .AddWithValue("@BookControl", BookControl)
            .AddWithValue("@LocationIsOrigin", LocationIsOrigin)
            .AddWithValue("@StopNumber", StopNumber)
            .AddWithValue("@Miles", Miles)
            .AddWithValue("@PCMCost", PCMCost)
            .AddWithValue("@PCMTime", Left(PCMTime, 40))
            .AddWithValue("@PCMTolls", PCMTolls)
            .AddWithValue("@PCMESTCHG", PCMESTCHG)
            .AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        End With
        Return processNGLStoredProcedure(spConfig, oSystem)
    End Function

    Public Function UpdateBookConsPickNumber(ByVal BookConsPrefix As String,
                                             Optional ByVal TwoWay As Boolean = False) As Boolean

        Dim spConfig As New clsNGLSPConfig("UpdateBookConsPickNumber", "dbo.spUpdateBookConsPickNumber", TwoWay)
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        With spConfig.oCmd.Parameters
            .AddWithValue("@BookConsPrefix", Left(BookConsPrefix, 20))
            .AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        End With
        Return processNGLStoredProcedure(spConfig, oSystem)
    End Function

    'New @UserName parameter added
    Public Sub UpdateOptimizedStopData(ByVal BookLoadControl As Integer,
                                   ByVal SeqNbr As Integer,
                                   ByVal DistToPrev As Double,
                                   ByVal ConsNumber As String)

        Dim strBatchName As String = "UpdateOptimizedStopData"
        Dim strProcName As String = "dbo.spUpdateOptimizedStopData50"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            'Validate the parameter data
            If BookLoadControl = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The book load control number Is Not valid. Please Select a valid book Load record To run this procedure.", "E_DataValidationFailure")
                Return
            End If
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@BookLoadControl", BookLoadControl)
            oCmd.Parameters.AddWithValue("@SeqNbr", SeqNbr)
            oCmd.Parameters.AddWithValue("@DistToPrev", DistToPrev)
            oCmd.Parameters.AddWithValue("@ConsNumber", Left(ConsNumber, 20))
            oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub


#End Region

#Region "Form And Screen Processing Stored Procedures"

    Public Sub AddFormIfMissing(ByVal FormControl As Integer, ByVal FormName As String, ByVal FormDescription As String)
        'we do not need the return value
        getScalarString("Exec dbo.spAddFormIfMissing " & FormControl & ",'" & FormName & "','" & FormDescription & "'")
    End Sub

    'User Name Parameter Not Needed
    Public Sub UpdateGLAPMassEntry(ByVal APGLNumber As String, ByVal BookPayCode As String, ByVal CarrierControl As Integer)
        Dim strBatchName As String = "UpdateGLAPMassEntry"
        Dim strProcName As String = "dbo.spUpdateGLAPMassEntry"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            'Validate the parameter data
            If CarrierControl = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The carrier control number is not valid. Please select a valid carrier record to run this procedure.", "E_DataValidationFailure")
                Return
            End If
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@APGLNumber", Left(APGLNumber, 50))
            oCmd.Parameters.AddWithValue("@BookPayCode", Left(BookPayCode, 3))
            oCmd.Parameters.AddWithValue("@CarrierNo", CarrierControl)
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub

    Public Function UpdateGLAPMassEntry2Way(ByVal APGLNumber As String, ByVal BookPayCode As String, ByVal CarrierControl As Integer) As Boolean
        Dim strBatchName As String = "UpdateGLAPMassEntry"
        Dim strProcName As String = "dbo.spUpdateGLAPMassEntry"
        Dim blnRet As Boolean = False
        'Validate the parameter data
        If CarrierControl = 0 Then Return False

        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@APGLNumber", Left(APGLNumber, 50))
        oCmd.Parameters.AddWithValue("@BookPayCode", Left(BookPayCode, 3))
        oCmd.Parameters.AddWithValue("@CarrierNo", CarrierControl)
        runNGLStoredProcedure(oCmd, strProcName, 0)
        blnRet = True
        Return blnRet
    End Function


    'User Name Parameter Not Needed
    Public Sub UpdateGLAPCommissions(ByVal APGLNumber As String, ByVal CompControl As Integer)
        Dim strBatchName As String = "UpdateGLAPCommissions"
        Dim strProcName As String = "dbo.spUpdateGLAPCommissions"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            'Validate the parameter data
            If CompControl = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The company control number is not valid. Please select a valid company record to run this procedure.", "E_DataValidationFailure")
                Return
            End If
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@NewGLNumber", Left(APGLNumber, 50))
            oCmd.Parameters.AddWithValue("@CompControlNumber", CompControl)
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub

    'User Name Parameter Not Needed
    Public Function UpdateGLAPCommissions2Way(ByVal APGLNumber As String, ByVal CompControl As Integer) As Boolean
        Dim strBatchName As String = "UpdateGLAPCommissions"
        Dim strProcName As String = "dbo.spUpdateGLAPCommissions"
        Dim blnRet As Boolean = False
        'Validate the parameter data
        If CompControl = 0 Then Return False
        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@NewGLNumber", Left(APGLNumber, 50))
        oCmd.Parameters.AddWithValue("@CompControlNumber", CompControl)
        runNGLStoredProcedure(oCmd, strProcName, 0)
        blnRet = True
        Return blnRet
    End Function
    'User Name Parameter Not Needed
    Public Sub UpdateGLAPPaid(ByVal APGLNumber As String, ByVal BookPayCode As String, ByVal CarrierControl As Integer)
        Dim strBatchName As String = "UpdateGLAPPaid"
        Dim strProcName As String = "dbo.spUpdateGLAPPaid"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            'Validate the parameter data
            If CarrierControl = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The carrier control number is not valid. Please select a valid carrier record to run this procedure.", "E_DataValidationFailure")
                Return
            End If
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@APGLNumber", Left(APGLNumber, 50))
            oCmd.Parameters.AddWithValue("@BookPayCode", Left(BookPayCode, 3))
            oCmd.Parameters.AddWithValue("@CarrierNo", CarrierControl)
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub

    Public Function UpdateGLAPPaid2Way(ByVal APGLNumber As String, ByVal BookPayCode As String, ByVal CarrierControl As Integer) As Boolean
        Dim strBatchName As String = "UpdateGLAPPaid"
        Dim strProcName As String = "dbo.spUpdateGLAPPaid"
        Dim blnRet As Boolean = False
        'Validate the parameter data
        If CarrierControl = 0 Then Return False

        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@APGLNumber", Left(APGLNumber, 50))
        oCmd.Parameters.AddWithValue("@BookPayCode", Left(BookPayCode, 3))
        oCmd.Parameters.AddWithValue("@CarrierNo", CarrierControl)
        runNGLStoredProcedure(oCmd, strProcName, 0)
        blnRet = True
        Return blnRet
    End Function


    ''' <summary>
    ''' Old function for entering freight bills  new functions should fillow the SettlementSave process flow
    ''' this function has been modified for backward compatibility but only executes part of the procedure    ''' 
    ''' </summary>
    ''' <param name="APPONumber"></param>
    ''' <param name="APPRONumber"></param>
    ''' <param name="APCNSNumber"></param>
    ''' <param name="APSHID"></param>
    ''' <param name="APCarrierNumber"></param>
    ''' <param name="APBillNumber"></param>
    ''' <param name="APBillDate"></param>
    ''' <param name="APCustomerID"></param>
    ''' <param name="APCostCenterNumber"></param>
    ''' <param name="APTotalCost"></param>
    ''' <param name="APBLNumber"></param>
    ''' <param name="APBilledWeight"></param>
    ''' <param name="APReceivedDate"></param>
    ''' <param name="APPayCode"></param>
    ''' <param name="APElectronicFlag"></param>
    ''' <param name="APTotalTax"></param>
    ''' <param name="APFee1"></param>
    ''' <param name="APFee2"></param>
    ''' <param name="APFee3"></param>
    ''' <param name="APFee4"></param>
    ''' <param name="APFee5"></param>
    ''' <param name="APFee6"></param>
    ''' <param name="APOtherCost"></param>
    ''' <param name="APCarrierCost"></param>
    ''' <param name="APOverwrite"></param>
    ''' <param name="APOrderSequence"></param>
    ''' <param name="APTaxDetail1"></param>
    ''' <param name="APTaxDetail2"></param>
    ''' <param name="APTaxDetail3"></param>
    ''' <param name="APTaxDetail4"></param>
    ''' <param name="APTaxDetail5"></param>
    ''' <param name="TwoWay"></param>
    ''' <returns>
    '''  Models.ResultObject
    ''' </returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.2.1.004 on 12/26/2019
    '''   replaces the old spInsertFreightBillUnique50 logic with new 365 Settlement Save functionality
    '''   the caller must also be modified to call NGLBookRevenueBLL.RecalculateUsingLineHaul
    '''   then call NGLBookData.UpdateAndAuditAPMassEntry.  The following are required:
    '''   APMassEntryControl returned in  oResults.Control, 
    '''   BookSHID, 
    '''   BookControl, 
    '''   and the InvoiceNo
    '''   version from BLL which reads the data from the localization library
    '''   Replaces previous logic in the BLL (wrapper methods still exists for backward compatibility)
    '''   the return data has also been modified with more details
    ''' </remarks>
    Public Function InsertFreightBillUnique(ByVal APPONumber As String,
                                       ByVal APPRONumber As String,
                                       ByVal APCNSNumber As String,
                                       ByVal APSHID As String,
                                       ByVal APCarrierNumber As Integer,
                                       ByVal APBillNumber As String,
                                       ByVal APBillDate As Date,
                                       ByVal APCustomerID As String,
                                       ByVal APCostCenterNumber As String,
                                       ByVal APTotalCost As Decimal,
                                       ByVal APBLNumber As String,
                                       ByVal APBilledWeight As Integer,
                                       ByVal APReceivedDate As Date,
                                       ByVal APPayCode As String,
                                       ByVal APElectronicFlag As Boolean,
                                       ByVal APTotalTax As Decimal,
                                       ByVal APFee1 As Decimal,
                                       ByVal APFee2 As Decimal,
                                       ByVal APFee3 As Decimal,
                                       ByVal APFee4 As Decimal,
                                       ByVal APFee5 As Decimal,
                                       ByVal APFee6 As Decimal,
                                       ByVal APOtherCost As Decimal,
                                       ByVal APCarrierCost As Decimal,
                                       ByVal APOverwrite As Boolean,
                                       ByVal APOrderSequence As Integer,
                                       Optional ByVal APTaxDetail1 As Decimal = 0,
                                       Optional ByVal APTaxDetail2 As Decimal = 0,
                                       Optional ByVal APTaxDetail3 As Decimal = 0,
                                       Optional ByVal APTaxDetail4 As Decimal = 0,
                                       Optional ByVal APTaxDetail5 As Decimal = 0,
                                       Optional ByVal TwoWay As Boolean = False) As Models.ResultObject
        Dim oResults As New Models.ResultObject() With {.Success = True, .SuccessMsg = "Success!"}
        Dim strErrMsg As String = ""
        Dim sbRetMsgs As New System.Text.StringBuilder()
        Try
            Dim s As New Models.SettlementSave

            If String.IsNullOrWhiteSpace(APSHID) Then
                throwFieldRequiredException("Booking Shipment ID")
            End If
            s.BookSHID = APSHID
            Dim iBookControl As Integer = 0
            Dim iCompControl As Integer = 0
            Dim iCarrierControl As Integer = 0
            Dim icompNo = 0
            Using db As New NGLMasBookDataContext(ConnectionString)
                Try
                    'iBookControl = db.Books.Where(Function(x) x.BookSHID = APSHID).Select(Function(y) y.BookControl).FirstOrDefault()
                    Dim oBookData As DTO.Book = (From b In db.Books Where b.BookSHID = APSHID Select New DTO.Book() With {.BookControl = b.BookControl, .BookCustCompControl = b.BookCustCompControl, .BookCarrierControl = b.BookCarrierControl}).FirstOrDefault()
                    If Not oBookData Is Nothing Then
                        iBookControl = oBookData.BookControl
                        iCompControl = oBookData.BookCustCompControl
                        iCarrierControl = oBookData.BookCarrierControl
                    End If
                Catch ex As Exception
                    'do nothing here we test the results below
                End Try
            End Using
            'Dim compData = NGLBookObjData.GetCompanyNameNumberByBookControl(iBookControl)

            If iBookControl = 0 Then
                throwFieldRequiredException("Booking Control Number")
            End If
            s.BookControl = iBookControl
            'process the fees
            s.Fees = NGLBookObjData.GetDataForAPMassEntryFees(APSHID, APCarrierNumber, icompNo, APFee1, APFee2, APFee3, APFee4, APFee5, APFee6, APOtherCost, APTaxDetail1, APTaxDetail2, APTaxDetail3, APTaxDetail4, APTaxDetail5)
            s.CompControl = iCompControl
            s.APCustomerID = icompNo
            s.CarrierControl = iCarrierControl
            s.CarrierNumber = APCarrierNumber
            's.APCustomerID = compData("CompNumber")
            s.InvoiceNo = APBillNumber
            s.InvoiceAmt = APTotalCost
            'Modified by RHR 01/10/2020 for ap mass update we do not adjust the Line Haul 
            'we save whatever is entered
            s.LineHaul = APCarrierCost
            'If APCarrierCost = 0 Then
            '    'calculate line haul using totalcost minus total billed fees (we exclude any missing fees as this line haul will be used for the historical record)
            '    Dim totalfees = s.Fees.Where(Function(x) x.BilledFee = True).Sum(Function(x) x.Cost)
            '    s.LineHaul = APTotalCost - totalfees
            'Else
            '    s.LineHaul = APCarrierCost
            'End If
            s.BookFinAPActWgt = APBilledWeight
            s.BookCarrBLNumber = APBLNumber
            s.APBillDate = APBillDate
            s.APReceivedDate = APReceivedDate
            oResults = NGLBookObjData.SettlementSave(s, False)
            oResults.BookControl = iBookControl 'we save the bookcontrol so the caller can recalculate costs and continue auditing the freight bill

        Catch ex As System.ServiceModel.FaultException(Of SqlFaultInfo)
            oResults.Success = False
            Dim strMsg = ex.Detail.getMsgForLogs()
            strErrMsg = NGLBookObjData.appendToResultMessage(oResults, strMsg, Models.ResultObject.ResultMsgType.Err, Utilities.ResultProcedures.freightbill, Utilities.ResultTitles.TitleAuditFreightBillWarning, Utilities.ResultPrefix.MsgDetails, Utilities.ResultSuffix.None)
        Catch ex As FaultException
            oResults.Success = False
            Throw
        Catch ex As Exception
            oResults.Success = False
            throwUnExpectedFaultException(ex, buildProcedureName("InsertFreightBillUnique"), sysErrorParameters.sysErrorState.SystemLevelFault, sysErrorParameters.sysErrorSeverity.Unexpected)
        End Try

        'Return sbRetMsgs.ToString
        Return oResults
    End Function

    Public Function InsertFreightBillUnique(ByVal oData As DTO.FreightBill,
                                       Optional ByVal TwoWay As Boolean = False) As Models.ResultObject
        With oData
            Return InsertFreightBillUnique(.APPONumber,
                                      .APPRONumber,
                                      .APCNSNumber,
                                      .APSHID,
                                      .APCarrierNumber,
                                      .APBillNumber,
                                      .APBillDate,
                                      .APCustomerID,
                                      .APCostCenterNumber,
                                      .APTotalCost,
                                      .APBLNumber,
                                      .APBilledWeight,
                                      .APReceivedDate,
                                      .APPayCode,
                                      .APElectronicFlag,
                                      .APTotalTax,
                                      .APFee1,
                                      .APFee2,
                                      .APFee3,
                                      .APFee4,
                                      .APFee5,
                                      .APFee6,
                                      .APOtherCost,
                                      .APCarrierCost,
                                      .APOverwrite,
                                      .APOrderSequence,
                                      .APTaxDetail1,
                                      .APTaxDetail2,
                                      .APTaxDetail3,
                                      .APTaxDetail4,
                                      .APTaxDetail5,
                                      TwoWay)
        End With

    End Function

    Public Function InsertFreightBillUnique2Way(ByVal APPONumber As String,
                                       ByVal APPRONumber As String,
                                       ByVal APCNSNumber As String,
                                       ByVal APSHID As String,
                                       ByVal APCarrierNumber As Integer,
                                       ByVal APBillNumber As String,
                                       ByVal APBillDate As Date,
                                       ByVal APCustomerID As String,
                                       ByVal APCostCenterNumber As String,
                                       ByVal APTotalCost As Decimal,
                                       ByVal APBLNumber As String,
                                       ByVal APBilledWeight As Integer,
                                       ByVal APReceivedDate As Date,
                                       ByVal APPayCode As String,
                                       ByVal APElectronicFlag As Boolean,
                                       ByVal APTotalTax As Decimal,
                                       ByVal APFee1 As Decimal,
                                       ByVal APFee2 As Decimal,
                                       ByVal APFee3 As Decimal,
                                       ByVal APFee4 As Decimal,
                                       ByVal APFee5 As Decimal,
                                       ByVal APFee6 As Decimal,
                                       ByVal APOtherCost As Decimal,
                                       ByVal APCarrierCost As Decimal,
                                       ByVal APOverwrite As Boolean,
                                       ByVal APOrderSequence As Integer) As Models.ResultObject

        Return InsertFreightBillUnique(APPONumber,
                                     APPRONumber,
                                     APCNSNumber,
                                     APSHID,
                                     APCarrierNumber,
                                     APBillNumber,
                                     APBillDate,
                                     APCustomerID,
                                     APCostCenterNumber,
                                     APTotalCost,
                                     APBLNumber,
                                     APBilledWeight,
                                     APReceivedDate,
                                     APPayCode,
                                     APElectronicFlag,
                                     APTotalTax,
                                     APFee1,
                                     APFee2,
                                     APFee3,
                                     APFee4,
                                     APFee5,
                                     APFee6,
                                     APOtherCost,
                                     APCarrierCost,
                                     APOverwrite,
                                     APOrderSequence,
                                     0,
                                     0,
                                     0,
                                     0,
                                     0,
                                     True)

    End Function

    'New @UserName parameter added
    Public Sub UpdateConsLoadStatusWTime(ByVal Contact As String,
                                       ByVal Message As String,
                                       ByVal ProNumber As String,
                                       ByVal DeliveredDate As Date,
                                       ByVal DeliveredTime As Date,
                                       ByVal blnUseDate As Boolean,
                                       ByVal blnUseTime As Boolean)
        Dim strBatchName As String = "UpdateConsLoadStatusWTime"
        Dim strProcName As String = "dbo.spUpdateConsLoadStatusWTime50"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@Contact", Left(Contact, 50))
            oCmd.Parameters.AddWithValue("@Message", Left(Message, 255))
            oCmd.Parameters.AddWithValue("@ProNumber", Left(ProNumber, 20))
            If blnUseDate Then oCmd.Parameters.AddWithValue("@DeliveredDate", DeliveredDate)
            If blnUseTime Then oCmd.Parameters.AddWithValue("@DeliveredTime", DeliveredTime)
            oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub
    'New @UserName parameter added
    Public Function UpdateConsLoadStatusWTime2Way(ByVal Contact As String,
                                       ByVal Message As String,
                                       ByVal ProNumber As String,
                                       ByVal DeliveredDate As Date,
                                       ByVal DeliveredTime As Date,
                                       ByVal blnUseDate As Boolean,
                                       ByVal blnUseTime As Boolean) As Boolean
        Dim strBatchName As String = "UpdateConsLoadStatusWTime"
        Dim strProcName As String = "dbo.spUpdateConsLoadStatusWTime50"
        Dim blnRet As Boolean = False

        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@Contact", Left(Contact, 50))
        oCmd.Parameters.AddWithValue("@Message", Left(Message, 255))
        oCmd.Parameters.AddWithValue("@ProNumber", Left(ProNumber, 20))
        If blnUseDate Then oCmd.Parameters.AddWithValue("@DeliveredDate", DeliveredDate)
        If blnUseTime Then oCmd.Parameters.AddWithValue("@DeliveredTime", DeliveredTime)
        oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        runNGLStoredProcedure(oCmd, strProcName, 0)
        blnRet = True
        Return blnRet
    End Function
    'New @UserName parameter added
    Public Sub InsertNatFuelIndex(ByVal NatFuelDate As Date,
                                     ByVal NatFuelNatAvg As Double,
                                     ByVal NatFuelZone1Avg As Double,
                                     ByVal NatFuelZone2Avg As Double,
                                     ByVal NatFuelZone3Avg As Double,
                                     ByVal NatFuelZone4Avg As Double,
                                     ByVal NatFuelZone5Avg As Double,
                                     ByVal NatFuelZone6Avg As Double,
                                     ByVal NatFuelZone7Avg As Double,
                                     ByVal NatFuelZone8Avg As Double,
                                     ByVal NatFuelZone9Avg As Double)
        Dim strBatchName As String = "InsertNatFuelIndex"
        Dim strProcName As String = "dbo.spInsertNatFuelIndex50"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@NatFuelDate", NatFuelDate)
            oCmd.Parameters.AddWithValue("@NatFuelNatAvg", NatFuelNatAvg)
            oCmd.Parameters.AddWithValue("@NatFuelZone1Avg", NatFuelZone1Avg)
            oCmd.Parameters.AddWithValue("@NatFuelZone2Avg", NatFuelZone2Avg)
            oCmd.Parameters.AddWithValue("@NatFuelZone3Avg", NatFuelZone3Avg)
            oCmd.Parameters.AddWithValue("@NatFuelZone4Avg", NatFuelZone4Avg)
            oCmd.Parameters.AddWithValue("@NatFuelZone5Avg", NatFuelZone5Avg)
            oCmd.Parameters.AddWithValue("@NatFuelZone6Avg", NatFuelZone6Avg)
            oCmd.Parameters.AddWithValue("@NatFuelZone7Avg", NatFuelZone7Avg)
            oCmd.Parameters.AddWithValue("@NatFuelZone8Avg", NatFuelZone8Avg)
            oCmd.Parameters.AddWithValue("@NatFuelZone9Avg", NatFuelZone9Avg)
            oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try

    End Sub

    ''' <summary>
    ''' This procedure should be modified to call
    ''' spUpdateLoadPlanningCarrier  but we are missing
    ''' the carrier number?  This method is typically call 
    ''' from the Load Board Summary screen. 
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="NewConsNumber"></param>
    ''' <param name="BookConsPrefix"></param>
    ''' <remarks></remarks>
    Public Sub UpdateConsolidationNumber(ByVal BookControl As Integer, ByVal NewConsNumber As String, ByVal BookConsPrefix As String)
        Dim strBatchName As String = "UpdateConsolidationNumber"
        Dim strProcName As String = "dbo.spUpdateConsolidationNumber50"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            'Validate the parameter data
            If BookControl = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The book control number is not valid. Please select a valid book record to run this procedure.", "E_DataValidationFailure")
                Return
            End If
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@BookControl", BookControl)
            oCmd.Parameters.AddWithValue("@BookConsPrefix", Left(BookConsPrefix, 20))
            oCmd.Parameters.AddWithValue("@NewConsNumber", Left(NewConsNumber, 20))
            oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub


    Public Function UpdateConsolidationNumber365(ByVal BookControl As Integer, ByVal NewConsNumber As String) As Boolean
        ' to remove the CNS number (set to null in db) both BookConsPrefix and NewConsNumber are the same as current
        ' to change the CNS number provide NewConsNumber  but leave BookConsPrefix blank
        ' so if NewConsNumber is Null
        Dim BookConsPrefix As String = ""
        If (String.IsNullOrWhiteSpace(NewConsNumber)) Then
            'we need to look up the existing CNS Number
            Using db As New NGLMasBookDataContext(ConnectionString)
                Try
                    BookConsPrefix = db.Books.Where(Function(x) x.BookControl = BookControl).Select(Function(y) y.BookConsPrefix).FirstOrDefault()
                    NewConsNumber = BookConsPrefix
                Catch ex As Exception
                    ManageLinqDataExceptions(ex, buildProcedureName("UpdateConsolidationNumber365"))
                End Try
            End Using
        End If
        Return UpdateConsolidationNumber2Way(BookControl, NewConsNumber, BookConsPrefix)
    End Function


    Public Function UpdateConsolidationNumber2Way(ByVal BookControl As Integer, ByVal NewConsNumber As String, ByVal BookConsPrefix As String) As Boolean
        Dim strBatchName As String = "UpdateConsolidationNumber"
        Dim strProcName As String = "dbo.spUpdateConsolidationNumber50"
        Dim blnRet As Boolean = False

        'Validate the parameter data
        If BookControl = 0 Then Return False

        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@BookControl", BookControl)
        oCmd.Parameters.AddWithValue("@BookConsPrefix", Left(BookConsPrefix, 20))
        oCmd.Parameters.AddWithValue("@NewConsNumber", Left(NewConsNumber, 20))
        oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        runNGLStoredProcedure(oCmd, strProcName, 0)
        blnRet = True
        Return blnRet
    End Function

    'New @UserName parameter added
    Public Sub UpdateCarrierBudgetAllocation(ByVal BudgetAllocation As Decimal, ByVal CarrierBudControl As Integer)
        Dim strBatchName As String = "UpdateCarrierBudgetAllocation"
        Dim strProcName As String = "dbo.spUpdateCarrierBudgetAllocation50"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@BudgetAllocation", BudgetAllocation)
            oCmd.Parameters.AddWithValue("@CarrierBudControl", CarrierBudControl)
            oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub
    Public Function UpdateLaneAddressFromPCMiler2Way(ByVal BLAControl As Integer, ByVal BLALaneControl As Integer, ByVal OrigDestBoth As Integer) As Boolean
        Dim blnRet As Boolean = False
        Dim strBatchName As String = "UpdateLaneAddressFromPCMiler"
        Dim strProcName As String = "dbo.spUpdateLaneAddressFromPCMiler50"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)

        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@BLAControl", BLAControl)
        oCmd.Parameters.AddWithValue("@BLALaneControl", BLALaneControl)
        oCmd.Parameters.AddWithValue("@OrigDestBoth", OrigDestBoth)
        oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        blnRet = True
        Return blnRet
    End Function

    Public Function UpdateBookAddressFromPCMiler2Way(ByVal BLAControl As Integer, ByVal BLABookProNumber As String, ByVal OrigDestBoth As Integer) As Boolean
        Dim blnRet As Boolean = False
        Dim strBatchName As String = "UpdateBookAddressFromPCMiler"
        Dim strProcName As String = "dbo.spUpdateBookAddressFromPCMiler50"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)

        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@BLAControl", BLAControl)
        oCmd.Parameters.AddWithValue("@BLABookProNumber", Left(BLABookProNumber, 20))
        oCmd.Parameters.AddWithValue("@OrigDestBoth", OrigDestBoth)
        oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        blnRet = True
        Return blnRet
    End Function

    'New @UserName parameter added
    Public Sub UpdateLaneAddressFromPCMiler(ByVal BLAControl As Integer, ByVal BLALaneControl As Integer, ByVal OrigDestBoth As Integer)
        Dim strBatchName As String = "UpdateLaneAddressFromPCMiler"
        Dim strProcName As String = "dbo.spUpdateLaneAddressFromPCMiler50"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@BLAControl", BLAControl)
            oCmd.Parameters.AddWithValue("@BLALaneControl", BLALaneControl)
            oCmd.Parameters.AddWithValue("@OrigDestBoth", OrigDestBoth)
            oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub
    'New @UserName parameter added
    Public Sub UpdateBookAddressFromPCMiler(ByVal BLAControl As Integer, ByVal BLABookProNumber As String, ByVal OrigDestBoth As Integer)
        Dim strBatchName As String = "UpdateBookAddressFromPCMiler"
        Dim strProcName As String = "dbo.spUpdateBookAddressFromPCMiler50"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@BLAControl", BLAControl)
            oCmd.Parameters.AddWithValue("@BLABookProNumber", Left(BLABookProNumber, 20))
            oCmd.Parameters.AddWithValue("@OrigDestBoth", OrigDestBoth)
            oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub
    'New @UserName parameter added
    Public Sub AllocateCostByWgt(ByVal BookControl As Integer, ByVal BookConsPrefix As String)
        Dim strBatchName As String = "AllocateCostByWgt"
        Dim strProcName As String = "dbo.spAllocateCostByWgt50"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            'Validate the parameter data
            If BookControl = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The book control number is not valid. Please select a valid book record to run this procedure.", "E_DataValidationFailure")
                Return
            End If
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@BookControl", BookControl)
            oCmd.Parameters.AddWithValue("@BookConsPrefix", Left(BookConsPrefix, 20))
            oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub
    'New @UserName parameter added
    Public Sub AllocateLoadWgt(ByVal BookControl As Integer)
        Dim strBatchName As String = "AllocateLoadWgt"
        Dim strProcName As String = "dbo.spAllocateLoadWgt50"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            'Validate the parameter data
            If BookControl = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The book control number is not valid. Please select a valid book record to run this procedure.", "E_DataValidationFailure")
                Return
            End If
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@BookControl", BookControl)
            oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub
    'New @UserName parameter added
    Public Sub GenerateCarrierTariff(ByVal CarrierControl As Integer,
                                     ByVal CompControl As Integer,
                                     ByVal TempType As Integer,
                                     ByVal TariffType As String,
                                     ByVal BPBracket As Integer,
                                     ByVal TLCapacity As Integer)
        Dim strBatchName As String = "GenerateCarrierTariff"
        Dim strProcName As String = "dbo.spGenerateCarrierTariff50"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            'Validate the parameter data
            If CarrierControl = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The carrier control number is not valid. Please select a valid carrier record to run this procedure.", "E_DataValidationFailure")
                Return
            End If
            'Validate the parameter data
            If CompControl = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The company control number is not valid. Please select a valid company record to run this procedure.", "E_DataValidationFailure")
                Return
            End If
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@CarrierControl", CarrierControl)
            oCmd.Parameters.AddWithValue("@CompControl", CompControl)
            oCmd.Parameters.AddWithValue("@TempType", TempType)
            oCmd.Parameters.AddWithValue("@TariffType", Left(TariffType, 1))
            oCmd.Parameters.AddWithValue("@BPBracket", BPBracket)
            oCmd.Parameters.AddWithValue("@TLCapacity", TLCapacity)
            oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub
    'User Name Parameter Not Needed
    Public Sub InsertCarrierFuelStates(ByVal CarrierControl As Integer)
        Dim strBatchName As String = "InsertCarrierFuelStates"
        Dim strProcName As String = "dbo.spInsertCarrierFuelStates"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            'Validate the parameter data
            If CarrierControl = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The carrier control number is not valid. Please select a valid carrier record to run this procedure.", "E_DataValidationFailure")
                Return
            End If
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@CarrierControl", CarrierControl)
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub
    'User Name Parameter Not Needed
    Public Sub UpdateCompBudSeasonality(ByVal CompControl As Integer,
                                        ByVal Mo1 As Decimal,
                                        ByVal Mo2 As Decimal,
                                        ByVal Mo3 As Decimal,
                                        ByVal Mo4 As Decimal,
                                        ByVal Mo5 As Decimal,
                                        ByVal Mo6 As Decimal,
                                        ByVal Mo7 As Decimal,
                                        ByVal Mo8 As Decimal,
                                        ByVal Mo9 As Decimal,
                                        ByVal Mo10 As Decimal,
                                        ByVal Mo11 As Decimal,
                                        ByVal Mo12 As Decimal)
        Dim strBatchName As String = "UpdateCompBudSeasonality"
        Dim strProcName As String = "dbo.spUpdateCompBudSeasonality"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            'Validate the parameter data
            If CompControl = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The company control number is not valid. Please select a valid company record to run this procedure.", "E_DataValidationFailure")
                Return
            End If
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@CompControl", CompControl)
            oCmd.Parameters.AddWithValue("@Mo1", Mo1)
            oCmd.Parameters.AddWithValue("@Mo2", Mo2)
            oCmd.Parameters.AddWithValue("@Mo3", Mo3)
            oCmd.Parameters.AddWithValue("@Mo4", Mo4)
            oCmd.Parameters.AddWithValue("@Mo5", Mo5)
            oCmd.Parameters.AddWithValue("@Mo6", Mo6)
            oCmd.Parameters.AddWithValue("@Mo7", Mo7)
            oCmd.Parameters.AddWithValue("@Mo8", Mo8)
            oCmd.Parameters.AddWithValue("@Mo9", Mo9)
            oCmd.Parameters.AddWithValue("@Mo10", Mo10)
            oCmd.Parameters.AddWithValue("@Mo11", Mo11)
            oCmd.Parameters.AddWithValue("@Mo12", Mo12)
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub
    'User Name Parameter Not Needed
    Public Sub UpdateCompSeasProfile(ByVal CompControl As Integer, ByVal SeasControl As Integer)
        Dim strBatchName As String = "UpdateCompSeasProfile"
        Dim strProcName As String = "dbo.spUpdateCompSeasProfile"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            'Validate the parameter data
            If CompControl = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The company control number is not valid. Please select a valid company record to run this procedure.", "E_DataValidationFailure")
                Return
            End If
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@CompControl", CompControl)
            oCmd.Parameters.AddWithValue("@SeasControl", SeasControl)
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub
    'User Name Parameter Not Needed
    Public Sub UpdateCompBudAllocation(ByVal CompControl As Integer, ByVal AllocVal As Decimal)
        Dim strBatchName As String = "UpdateCompBudAllocation"
        Dim strProcName As String = "dbo.spUpdateCompBudAllocation"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            'Validate the parameter data
            If CompControl = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The company control number is not valid. Please select a valid company record to run this procedure.", "E_DataValidationFailure")
                Return
            End If
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@CompControl", CompControl)
            oCmd.Parameters.AddWithValue("@AllocVal", AllocVal)
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub
    Public Function UpdateCompBudAllocation2Way(ByVal CompControl As Integer, ByVal AllocVal As Decimal) As Boolean
        Dim strBatchName As String = "UpdateCompBudAllocation"
        Dim strProcName As String = "dbo.spUpdateCompBudAllocation"
        Dim blnRet As Boolean = False
        'Validate the parameter data
        If CompControl = 0 Then Return False
        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@CompControl", CompControl)
        oCmd.Parameters.AddWithValue("@AllocVal", AllocVal)
        runNGLStoredProcedure(oCmd, strProcName, 0)
        blnRet = True
        Return blnRet
    End Function

    Public Function UpdateCompCOGS2Way(ByVal CompControl As Integer,
                                        ByVal Mo1 As Decimal,
                                        ByVal Mo2 As Decimal,
                                        ByVal Mo3 As Decimal,
                                        ByVal Mo4 As Decimal,
                                        ByVal Mo5 As Decimal,
                                        ByVal Mo6 As Decimal,
                                        ByVal Mo7 As Decimal,
                                        ByVal Mo8 As Decimal,
                                        ByVal Mo9 As Decimal,
                                        ByVal Mo10 As Decimal,
                                        ByVal Mo11 As Decimal,
                                        ByVal Mo12 As Decimal,
                                        ByVal CogsMarginBudget As Double,
                                        ByVal CogsBudgetTotal As Decimal) As Boolean
        Dim strBatchName As String = "UpdateCompCOGS"
        Dim strProcName As String = "dbo.spUpdateCompCOGS"
        Dim blnRet As Boolean = False
        'Validate the parameter data
        If CompControl = 0 Then Return False
        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@CompControl", CompControl)
        oCmd.Parameters.AddWithValue("@Mo1", Mo1)
        oCmd.Parameters.AddWithValue("@Mo2", Mo2)
        oCmd.Parameters.AddWithValue("@Mo3", Mo3)
        oCmd.Parameters.AddWithValue("@Mo4", Mo4)
        oCmd.Parameters.AddWithValue("@Mo5", Mo5)
        oCmd.Parameters.AddWithValue("@Mo6", Mo6)
        oCmd.Parameters.AddWithValue("@Mo7", Mo7)
        oCmd.Parameters.AddWithValue("@Mo8", Mo8)
        oCmd.Parameters.AddWithValue("@Mo9", Mo9)
        oCmd.Parameters.AddWithValue("@Mo10", Mo10)
        oCmd.Parameters.AddWithValue("@Mo11", Mo11)
        oCmd.Parameters.AddWithValue("@Mo12", Mo12)
        oCmd.Parameters.AddWithValue("@CogsMarginBudget", CogsMarginBudget)
        oCmd.Parameters.AddWithValue("@CogsBudgetTotal", CogsBudgetTotal)
        runNGLStoredProcedure(oCmd, strProcName, 0)
        blnRet = True
        Return blnRet
    End Function

    'User Name Parameter Not Needed
    Public Sub UpdateCompCOGS(ByVal CompControl As Integer,
                                        ByVal Mo1 As Decimal,
                                        ByVal Mo2 As Decimal,
                                        ByVal Mo3 As Decimal,
                                        ByVal Mo4 As Decimal,
                                        ByVal Mo5 As Decimal,
                                        ByVal Mo6 As Decimal,
                                        ByVal Mo7 As Decimal,
                                        ByVal Mo8 As Decimal,
                                        ByVal Mo9 As Decimal,
                                        ByVal Mo10 As Decimal,
                                        ByVal Mo11 As Decimal,
                                        ByVal Mo12 As Decimal,
                                        ByVal CogsMarginBudget As Double,
                                        ByVal CogsBudgetTotal As Decimal)
        Dim strBatchName As String = "UpdateCompCOGS"
        Dim strProcName As String = "dbo.spUpdateCompCOGS"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            'Validate the parameter data
            If CompControl = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The company control number is not valid. Please select a valid company record to run this procedure.", "E_DataValidationFailure")
                Return
            End If
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@CompControl", CompControl)
            oCmd.Parameters.AddWithValue("@Mo1", Mo1)
            oCmd.Parameters.AddWithValue("@Mo2", Mo2)
            oCmd.Parameters.AddWithValue("@Mo3", Mo3)
            oCmd.Parameters.AddWithValue("@Mo4", Mo4)
            oCmd.Parameters.AddWithValue("@Mo5", Mo5)
            oCmd.Parameters.AddWithValue("@Mo6", Mo6)
            oCmd.Parameters.AddWithValue("@Mo7", Mo7)
            oCmd.Parameters.AddWithValue("@Mo8", Mo8)
            oCmd.Parameters.AddWithValue("@Mo9", Mo9)
            oCmd.Parameters.AddWithValue("@Mo10", Mo10)
            oCmd.Parameters.AddWithValue("@Mo11", Mo11)
            oCmd.Parameters.AddWithValue("@Mo12", Mo12)
            oCmd.Parameters.AddWithValue("@CogsMarginBudget", CogsMarginBudget)
            oCmd.Parameters.AddWithValue("@CogsBudgetTotal", CogsBudgetTotal)
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub

    Public Function UpdateCompProfit2Way(ByVal CompControl As Integer,
                                        ByVal Mo1 As Decimal,
                                        ByVal Mo2 As Decimal,
                                        ByVal Mo3 As Decimal,
                                        ByVal Mo4 As Decimal,
                                        ByVal Mo5 As Decimal,
                                        ByVal Mo6 As Decimal,
                                        ByVal Mo7 As Decimal,
                                        ByVal Mo8 As Decimal,
                                        ByVal Mo9 As Decimal,
                                        ByVal Mo10 As Decimal,
                                        ByVal Mo11 As Decimal,
                                        ByVal Mo12 As Decimal,
                                        ByVal ProfitMarginBudget As Double,
                                        ByVal ProfitBudgetTotal As Decimal) As Boolean
        Dim strBatchName As String = "UpdateCompProfit"
        Dim strProcName As String = "dbo.spUpdateCompProfit"
        Dim blnRet As Boolean = False
        'Validate the parameter data
        If CompControl = 0 Then Return False
        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@CompControl", CompControl)
        oCmd.Parameters.AddWithValue("@Mo1", Mo1)
        oCmd.Parameters.AddWithValue("@Mo2", Mo2)
        oCmd.Parameters.AddWithValue("@Mo3", Mo3)
        oCmd.Parameters.AddWithValue("@Mo4", Mo4)
        oCmd.Parameters.AddWithValue("@Mo5", Mo5)
        oCmd.Parameters.AddWithValue("@Mo6", Mo6)
        oCmd.Parameters.AddWithValue("@Mo7", Mo7)
        oCmd.Parameters.AddWithValue("@Mo8", Mo8)
        oCmd.Parameters.AddWithValue("@Mo9", Mo9)
        oCmd.Parameters.AddWithValue("@Mo10", Mo10)
        oCmd.Parameters.AddWithValue("@Mo11", Mo11)
        oCmd.Parameters.AddWithValue("@Mo12", Mo12)
        oCmd.Parameters.AddWithValue("@ProfitMarginBudget", ProfitMarginBudget)
        oCmd.Parameters.AddWithValue("@ProfitBudgetTotal", ProfitBudgetTotal)
        runNGLStoredProcedure(oCmd, strProcName, 0)
        blnRet = True
        Return blnRet
    End Function

    'User Name Parameter Not Needed
    Public Sub UpdateCompProfit(ByVal CompControl As Integer,
                                        ByVal Mo1 As Decimal,
                                        ByVal Mo2 As Decimal,
                                        ByVal Mo3 As Decimal,
                                        ByVal Mo4 As Decimal,
                                        ByVal Mo5 As Decimal,
                                        ByVal Mo6 As Decimal,
                                        ByVal Mo7 As Decimal,
                                        ByVal Mo8 As Decimal,
                                        ByVal Mo9 As Decimal,
                                        ByVal Mo10 As Decimal,
                                        ByVal Mo11 As Decimal,
                                        ByVal Mo12 As Decimal,
                                        ByVal ProfitMarginBudget As Double,
                                        ByVal ProfitBudgetTotal As Decimal)
        Dim strBatchName As String = "UpdateCompProfit"
        Dim strProcName As String = "dbo.spUpdateCompProfit"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            'Validate the parameter data
            If CompControl = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The company control number is not valid. Please select a valid company record to run this procedure.", "E_DataValidationFailure")
                Return
            End If
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@CompControl", CompControl)
            oCmd.Parameters.AddWithValue("@Mo1", Mo1)
            oCmd.Parameters.AddWithValue("@Mo2", Mo2)
            oCmd.Parameters.AddWithValue("@Mo3", Mo3)
            oCmd.Parameters.AddWithValue("@Mo4", Mo4)
            oCmd.Parameters.AddWithValue("@Mo5", Mo5)
            oCmd.Parameters.AddWithValue("@Mo6", Mo6)
            oCmd.Parameters.AddWithValue("@Mo7", Mo7)
            oCmd.Parameters.AddWithValue("@Mo8", Mo8)
            oCmd.Parameters.AddWithValue("@Mo9", Mo9)
            oCmd.Parameters.AddWithValue("@Mo10", Mo10)
            oCmd.Parameters.AddWithValue("@Mo11", Mo11)
            oCmd.Parameters.AddWithValue("@Mo12", Mo12)
            oCmd.Parameters.AddWithValue("@ProfitMarginBudget", ProfitMarginBudget)
            oCmd.Parameters.AddWithValue("@ProfitBudgetTotal", ProfitBudgetTotal)
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub
    'User Name Parameter Not Needed
    Public Sub UpdateFuelDefault(ByVal CarrierControl As Integer, ByVal EffDate As Date, ByVal DefPercentage As Double)
        Dim strBatchName As String = "UpdateFuelDefault"
        Dim strProcName As String = "dbo.spUpdateFuelDefault"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            'Validate the parameter data
            If CarrierControl = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The carrier control number is not valid. Please select a valid carrier record to run this procedure.", "E_DataValidationFailure")
                Return
            End If
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@CarrierControl", CarrierControl)
            oCmd.Parameters.AddWithValue("@EffDate", EffDate)
            oCmd.Parameters.AddWithValue("@DefPercentage", DefPercentage)
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub

    ''' <summary>
    ''' Insert a new duplicate copy of the current order.  Caller must manually enter the order number.
    ''' Returns the new TMS Pro Number
    ''' </summary>
    ''' <param name="CompControl"></param>
    ''' <param name="OldPRONumber"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.3.0.003 on 01/30/2021
    '''     Added logic to use the updated GetNextProNumber logic so all code paths 
    '''     work the same.
    ''' </remarks>
    Public Function InsertNewBookingDuplicate(ByVal CompControl As Integer, ByVal OldPRONumber As String) As String
        Dim strBatchName As String = "InsertNewBookingDuplicate"
        Dim strProcName As String = "dbo.spInsertNewBookingDuplicate70"
        Dim sNewPROBase As String = getScalarString("SELECT TOP 1 p.ParValue FROM dbo.parameter as p WHERE p.parkey = 'PRONUMBER'")
        Dim intNextPro As Integer = 0
        Integer.TryParse(sNewPROBase, intNextPro)
        intNextPro += 1
        executeSQL("Update dbo.Parameter Set ParValue = " & intNextPro & " Where ParKey = 'PRONUMBER'")
        Dim sNewPRONumber = GetNextProNumber(CompControl, sNewPROBase)

        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@NewPRONumber", Left(sNewPRONumber, 20))
        oCmd.Parameters.AddWithValue("@PROBase", Left(intNextPro.ToString, 50))
        oCmd.Parameters.AddWithValue("@OldPRONumber", Left(OldPRONumber, 20))
        oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        runNGLStoredProcedure(oCmd, strProcName, 2)

        Return sNewPRONumber
    End Function
    'New @UserName parameter added
    Public Sub UpdateBookLoadCosting(ByVal BookControl As Integer,
                                     ByVal BookTotalCases As Integer,
                                     ByVal BookTotalWgt As Double,
                                     ByVal BookTotalPL As Double,
                                     ByVal BookTotalPX As Integer,
                                     ByVal BookTotalCube As Integer,
                                     ByVal BookTotalBFC As Decimal)
        Dim strBatchName As String = "UpdateBookLoadCosting"
        Dim strProcName As String = "dbo.spUpdateBookLoadCosting50"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            'Validate the parameter data
            If BookControl = 0 Then
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, "The book control number is not valid. Please select a valid book record to run this procedure.", "E_DataValidationFailure")
                Return
            End If
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@BookControl", BookControl)
            oCmd.Parameters.AddWithValue("@BookTotalCases", BookTotalCases)
            oCmd.Parameters.AddWithValue("@BookTotalWgt", BookTotalWgt)
            oCmd.Parameters.AddWithValue("@BookTotalPL", BookTotalPL)
            oCmd.Parameters.AddWithValue("@BookTotalPX", BookTotalPX)
            oCmd.Parameters.AddWithValue("@BookTotalCube", BookTotalCube)
            oCmd.Parameters.AddWithValue("@BookTotalBFC", BookTotalBFC)
            oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub
    'New @UserName parameter added
    Public Sub AllocateLoadCostWeight(ByVal TotalBFC As Decimal, ByVal BookConsPrefix As String)
        Dim strBatchName As String = "AllocateLoadCostWeight"
        Dim strProcName As String = "dbo.spAllocateLoadCostWeight50"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@TotalBFC", TotalBFC)
            oCmd.Parameters.AddWithValue("@BookConsPrefix", Left(BookConsPrefix, 20))
            oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub

    Public Function AllocateLoadCostWeight2Way(ByVal TotalBFC As Decimal, ByVal BookConsPrefix As String) As Boolean
        Dim strBatchName As String = "AllocateLoadCostWeight"
        Dim strProcName As String = "dbo.spAllocateLoadCostWeight50"
        Dim blnRet As Boolean = False
        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@TotalBFC", TotalBFC)
        oCmd.Parameters.AddWithValue("@BookConsPrefix", Left(BookConsPrefix, 20))
        oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        runNGLStoredProcedure(oCmd, strProcName, 0)
        blnRet = True
        Return blnRet
    End Function

    'New @UserName parameter added
    Public Sub AllocateCarrierCostByWeightOnly(ByVal CarrierCost As Decimal, ByVal BookConsPrefix As String)
        Dim strBatchName As String = "AllocateCarrierCostByWeightOnly"
        Dim strProcName As String = "dbo.spAllocateCarrierCostByWeightOnly50"
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@CarrierCost", CarrierCost)
            oCmd.Parameters.AddWithValue("@BookConsPrefix", Left(BookConsPrefix, 20))
            oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
            RunBatchStoredProcedure(strProcName, strBatchName, oCmd, oSystem)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Try
                oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, strBatchName, ex.Message, "E_UnExpected")
            Catch e As Exception

            End Try
        End Try
    End Sub

    Public Function AllocateCarrierCostByWeightOnly2Way(ByVal CarrierCost As Decimal, ByVal BookConsPrefix As String) As Boolean
        Dim strBatchName As String = "AllocateCarrierCostByWeightOnly"
        Dim strProcName As String = "dbo.spAllocateCarrierCostByWeightOnly50"
        Dim blnRet As Boolean = False
        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@CarrierCost", CarrierCost)
        oCmd.Parameters.AddWithValue("@BookConsPrefix", Left(BookConsPrefix, 20))
        oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        runNGLStoredProcedure(oCmd, strProcName, 0)
        blnRet = True
        Return blnRet
    End Function

    Public Function executeNGLStoredProcedure(ByVal BatchName As String, ByVal ProcName As String, ByVal ProcPars As DTO.NGLStoredProcedureParameter(), Optional ByVal TwoWay As Boolean = False, Optional ByVal MaxRetry As Integer = 1) As Boolean
        Dim spConfig As New clsNGLSPConfig(BatchName, ProcName, TwoWay)
        Dim oSystem As New NGLSystemDataProvider(Me.Parameters)
        Try
            With spConfig
                .intMaxRetry = MaxRetry
                If Not ProcPars Is Nothing AndAlso ProcPars.Count > 0 Then
                    'add the parameters if they exist
                    With .oCmd
                        For Each ProcPar In ProcPars
                            .Parameters.AddWithValue(ProcPar.ParName, ProcPar.ParValue)
                        Next
                        .Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
                    End With
                End If
            End With
        Catch ex As Exception

            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        End Try
        Return processNGLStoredProcedure(spConfig, oSystem)
    End Function

    Public Function hasOrderChanged(ByVal OrderNumber As String,
                                    ByVal LaneNumber As String,
                                    ByVal BookDateLoad As String,
                                    ByVal BookTotalWgt As Double,
                                    ByVal BookTotalCases As Integer,
                                    ByVal BookTotalPL As Integer,
                                    ByVal PONumber As String,
                                    ByVal BookItemNumbers As String,
                                    ByVal NumberOfItems As Integer) As Boolean
        Dim blnRet As Boolean = False

        'build a sql string to check if the order has changed
        'build a sql string to check if the order has changed
        Dim strSQL As String = ""
        If NumberOfItems > 0 Then
            strSQL = String.Format("Select Count(BookControl) From dbo.book inner join dbo.BookLoad on dbo.book.BookControl = dbo.bookload.BookLoadBookControl inner join dbo.BookItem on dbo.BookLoad.BookLoadControl = dbo.BookItem.BookItemBookLoadControl inner join dbo.Lane on dbo.book.BookODControl = dbo.lane.LaneControl inner join dbo.Comp on dbo.book.BookCustCompControl = dbo.Comp.CompControl where BookCarrOrderNumber = '{0}' AND LaneNumber = '{1}' AND dbo.DateNoTime(BookDateLoad)  = '{2}' AND BookTotalWgt = {3} AND BookTotalCases = {4} AND BookTotalPL = {5} AND CompNumber = '31' AND dbo.BookLoad.BookLoadPONumber = '{6}'	AND dbo.BookItem.BookItemItemNumber in ({7}) AND (Select COUNT(BookItemControl) From dbo.BookItem Where dbo.BookItem.BookItemBookLoadControl = dbo.BookLoad.BookLoadControl) = {8}", OrderNumber, LaneNumber, BookDateLoad, BookTotalWgt, BookTotalCases, BookTotalPL, PONumber, BookItemNumbers, NumberOfItems)
        Else
            strSQL = String.Format("Select Count(BookControl) From dbo.book inner join dbo.BookLoad on dbo.book.BookControl = dbo.bookload.BookLoadBookControl inner join dbo.Lane on dbo.book.BookODControl = dbo.lane.LaneControl inner join dbo.Comp on dbo.book.BookCustCompControl = dbo.Comp.CompControl where BookCarrOrderNumber = '{0}' AND LaneNumber = '{1}' AND dbo.DateNoTime(BookDateLoad)  = '{2}' AND BookTotalWgt = {3} AND BookTotalCases = {4} AND BookTotalPL = {5} AND CompNumber = '31' AND dbo.BookLoad.BookLoadPONumber = '{6}'	AND (Select COUNT(BookItemControl) From dbo.BookItem Where dbo.BookItem.BookItemBookLoadControl = dbo.BookLoad.BookLoadControl) = 0", OrderNumber, LaneNumber, BookDateLoad, BookTotalWgt, BookTotalCases, BookTotalPL, PONumber)
        End If
        Dim intRet As Integer = 0
        intRet = getScalarInteger(strSQL)
        If intRet < 1 Then blnRet = True
        Return blnRet

    End Function


    Public Function returnScalarInteger(ByVal strSQL As String) As Integer
        Dim intRet As Integer = 0
        intRet = getScalarInteger(strSQL)
        Return intRet
    End Function

    Public Function returnScalarString(ByVal strSQL As String) As String
        Dim strRet As String = ""
        strRet = getScalarString(strSQL)
        Return strRet
    End Function

    Public Function getXMLDataFromStoredProcedure(ByVal ProcName As String, ByVal ProcPars As DTO.NGLStoredProcedureParameter()) As System.xml.xmlelement

        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim oXMLDDoc As System.Xml.XmlDocument
        Try

            oQuery.ConnectionString = Me.ConnectionString

            Dim cmdObj As New System.Data.SqlClient.SqlCommand
            With cmdObj
                For Each ProcPar In ProcPars
                    .Parameters.AddWithValue(ProcPar.ParName, ProcPar.ParValue)
                Next
                .CommandText = ProcName
                .CommandType = CommandType.StoredProcedure
            End With


            Dim oResult As Ngl.Core.Data.QueryResult = oQuery.ExecuteWithFill(cmdObj)
            If Not oResult.Exception Is Nothing Then
                Utilities.SaveAppError(oResult.Exception.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = oResult.Exception.Message}, New FaultReason("E_SQLException"))
            End If
            If Not oResult.Data Is Nothing Then
                Dim oTable As System.Data.DataTable = oResult.Data
                oTable.TableName = "blank"
                'oXMLDDoc = DTran.GetXmlDoc(GetType(System.Data.DataTable), oTable)
                'Dim strXML As String = DTran.ConvertDataTableToXML(oTable)
                'oXMLDDoc = DTran.ConvertDataTableToXML(oTable)
                'Dim strXML As String = DTran.ConvertDataTableToXML(oTable, oXMLDDoc)
                oXMLDDoc = DTran.ConvertDataTableToXML(oTable)
            End If
        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseDataValidationException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DataValidationFailure"}, New FaultReason("E_ProcessProcedureFailure"))
        Catch ex As Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_ProcessProcedureFailure"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try

        End Try
        If Not oXMLDDoc Is Nothing Then
            Return oXMLDDoc.DocumentElement
        Else
            Return Nothing
        End If

    End Function

#End Region


End Class
