Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.ServiceModel

Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports LTS = Ngl.FreightMaster.Data.LTS
Imports DTran = Ngl.Core.Utility.DataTransformation
Imports Serilog.Core
Imports Microsoft.VisualBasic.Logging

Public Class NGLDataProviderBaseClass

#Region " Properties"

    Private _ConfigDataRows As DataRow()
    Friend Property ConfigDataRows As DataRow()
        Get
            Return _ConfigDataRows
        End Get
        Set(value As DataRow())
            _ConfigDataRows = value
        End Set
    End Property

    Private _ConnectionString As String = "" ' My.Settings.NGLMAS 'the default value uses the configuration setting NGLMAS
    Public Property ConnectionString() As String
        Get
            If Len(Trim(_ConnectionString)) < 5 Then
                If Len(Trim(_Parameters.ConnectionString)) < 5 Then
                    _ConnectionString = String.Format("Server={0}; Database={1}; Integrated Security=SSPI", _Parameters.DBServer.Trim, _Parameters.Database.Trim)
                Else
                    _ConnectionString = _Parameters.ConnectionString
                End If
            End If
            Return _ConnectionString
        End Get
        Set(ByVal value As String)
            _ConnectionString = value
        End Set
    End Property

    Private _IsAuthorized As Boolean = True
    Public Property IsAuthorized() As Boolean
        Get
            Return _IsAuthorized
        End Get
        Set(ByVal value As Boolean)
            _IsAuthorized = value
        End Set
    End Property

    Private _Parameters As WCFParameters
    Public ReadOnly Property Parameters() As WCFParameters
        Get
            Return _Parameters
        End Get
    End Property

#End Region

#Region " Deligates"
    'Note: oData is a DTO object the function must return an Linq Data Table Object
    Friend Delegate Function CopyToLinq(ByVal oData As Object) As Object
    'Note: the ValidateData Function must throw a FaultException error on failure
    Friend Delegate Sub ValidateData(ByRef oDB As System.Data.Linq.DataContext, ByVal oData As Object)
    'Note: nLink is an LTS link data object and oData is a DTO object
    Friend Delegate Sub AddDetailsToLinq(ByRef nLink As Object, ByVal oData As Object)
    'Note: nLink is an LTS link data object
    Friend Delegate Sub InsertAllDetails(ByRef oDB As System.Data.Linq.DataContext, ByRef nLink As Object)
    'Note: oData is a DTO object
    Friend Delegate Sub ProcessUpdatedDetails(ByRef oDB As System.Data.Linq.DataContext, ByVal oData As Object)

#End Region

#Region " Methods"


    Public Function getNewConnection(Optional ByVal conString As String = "") As System.Data.SqlClient.SqlConnection

        Dim objcon As New System.Data.SqlClient.SqlConnection

        If conString.Trim.Length < 1 Then
            conString = ConnectionString
        End If

        Try

            objcon.ConnectionString = conString
            objcon.Open()

        Catch ex As FaultException
            Throw
        Catch ex As System.Data.SqlClient.SqlException
            Dim strMsg As String = ex.Message
            If ex.Errors.Count > 0 Then
                strMsg = "Login Error Number: " & ex.Errors(0).Class.ToString() & ControlChars.NewLine & strMsg
            End If
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = strMsg}, New FaultReason("E_DBLoginFailure for testing: " & conString & " ---  " & strMsg))
        Catch ex As Exception
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        End Try
        Return objcon

    End Function


    Friend Sub NoDataValidation(ByRef oDB As System.Data.Linq.DataContext, ByVal oData As Object)

    End Sub

    Friend Sub NoDetailsToProcess(ByRef oDB As System.Data.Linq.DataContext, ByVal oData As Object)

    End Sub

    Friend Function CreateRecord(Of TEntity As Class)(ByVal oData As Object,
                                ByVal oLinqTable As System.Data.Linq.Table(Of TEntity),
                                ByVal oDB As System.Data.Linq.DataContext,
                                ByVal LinqCopyMethod As CopyToLinq,
                                ByVal ValidateCreateData As ValidateData) As Object
        Using oDB
            'Note: the ValidateData Function must throw a FaultException error on failure
            ValidateCreateData(oDB, oData)
            'Create New Record 
            Dim nObject = LinqCopyMethod(oData)
            oLinqTable.InsertOnSubmit(nObject)
            Try
                oDB.SubmitChanges()
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
            Return nObject
        End Using

    End Function

    Friend Function CreateRecordWithDetails(Of TEntity As Class)(ByVal oData As Object,
                                ByVal oLinqTable As System.Data.Linq.Table(Of TEntity),
                                ByVal oDB As System.Data.Linq.DataContext,
                                ByVal LinqCopyMethod As CopyToLinq,
                                ByVal ValidateCreateData As ValidateData,
                                ByVal AddDetails As AddDetailsToLinq,
                                ByVal InsertDetails As InsertAllDetails) As Object
        Using oDB
            'Note: the ValidateData Function must throw a FaultException error on failure
            ValidateCreateData(oDB, oData)
            'Create New Record 
            Dim nObject = LinqCopyMethod(oData)
            AddDetails(nObject, oData)
            oLinqTable.InsertOnSubmit(nObject)
            InsertDetails(oDB, nObject)
            Try
                oDB.SubmitChanges()
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
            Return nObject
        End Using

    End Function

    Friend Sub DeleteRecord(Of TEntity As Class)(ByVal oData As Object,
                                ByVal oLinqTable As System.Data.Linq.Table(Of TEntity),
                                ByVal oDB As System.Data.Linq.DataContext,
                                ByVal LinqCopyMethod As CopyToLinq,
                                ByVal ValidateDeleteData As ValidateData)
        Using oDB
            'Note: the ValidateData Function must throw a FaultException error on failure
            ValidateDeleteData(oDB, oData)
            'Create New Record
            Dim nObject = LinqCopyMethod(oData)
            oLinqTable.Attach(nObject, True)
            oLinqTable.DeleteOnSubmit(nObject)
            Try
                oDB.SubmitChanges()
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch conflictEx As ChangeConflictException
                Try
                    Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(oDB))
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
        End Using
    End Sub

    Friend Function UpdateRecord(Of TEntity As Class)(ByVal oData As Object,
                                ByVal oLinqTable As System.Data.Linq.Table(Of TEntity),
                                ByVal oDB As System.Data.Linq.DataContext,
                                ByVal LinqCopyMethod As CopyToLinq,
                                ByVal ValidateChangedData As ValidateData,
                                ByVal ProcessDetails As ProcessUpdatedDetails) As Object
        Using oDB
            'Note: the ValidateData Function must throw a FaultException error on failure
            ValidateChangedData(oDB, oData)
            'Create New Record 
            Dim nObject = LinqCopyMethod(oData)
            ' Attach the record 
            oLinqTable.Attach(nObject, True)
            ProcessDetails(oDB, oData)
            Try
                oDB.SubmitChanges()
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch conflictEx As ChangeConflictException
                Try
                    Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(oDB))
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
            ' Return the updated order
            Return nObject
        End Using
    End Function

    Private processParameterCallCount As Integer = 0
    Friend Sub processParameters(ByVal oParameters As WCFParameters)
        processParameterCallCount += 1
        Try
            If Not oParameters Is Nothing Then
                'clear the connection string to be sure we use the parameter values provided
                Me.ConnectionString = oParameters.ConnectionString
                'Save the parameters object
                _Parameters = oParameters
                'validate the Auth Code
                Serilog.Log.Logger.Verbose("NGLDataProviderBaseClass.processParameters: {0} - {@1}", processParameterCallCount, oParameters)
                'For NGLSystem we use the default connectionstring (provided connectionstring via WCFParameters) or integrated windows security and the WCFAuthCode is not required
                If oParameters.WCFAuthCode <> "NGLSystem" Then
                    Dim strWCFAuthCode As String = readConfigSettings("WCFAuthCode").Trim
                    Dim strSQLAuthUser As String = readConfigSettings("SQLAuthUser").Trim
                    Dim strSQLAuthPass As String = readConfigSettings("SQLAuthPass").Trim
                    If Not oParameters.WCFAuthCode = strWCFAuthCode Then
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NotAuthService"}, New FaultReason("E_AuthCodeFail"))
                    End If
                    If String.IsNullOrEmpty(ConnectionString) Then
                        'Check if we need to build a new connection string and use the config files username and password
                        If (Not String.IsNullOrEmpty(oParameters.DBServer) AndAlso oParameters.DBServer.Trim.Length > 0) AndAlso (Not String.IsNullOrEmpty(oParameters.Database) AndAlso oParameters.Database.Trim.Length > 0) Then
                            ConnectionString = String.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3};TrustServerCertificate=true;", oParameters.DBServer.Trim, oParameters.Database.Trim, strSQLAuthUser, strSQLAuthPass)
                        End If
                    End If

                End If
                    'Create a data connection 
                    Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
                Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
                Dim oCmd As New System.Data.SqlClient.SqlCommand
                'Perform Authentication Procedures
                Try
                    Dim strRet As String = "0"
                    Dim intRet As Integer = 0
                    'Check if we need to authenticate the user
                    If oParameters.ValidateAccess Then
                        Dim strRetMsg As String = ""
                        Dim intErrNbr As Integer
                        oCmd.Parameters.AddWithValue("@UserName", oParameters.UserName)
                        'We only provide a password if not using NGL Integrated sucurity, for integrated security the password in passed as NULL
                        If oParameters.UserRemotePassword <> "@NGL_Integrated_Security_2011!@" Then
                            oCmd.Parameters.AddWithValue("@UserRemotePassword", DTran.Encrypt(oParameters.UserRemotePassword, "NGL"))
                        End If
                        If Not oQuery.execNGLStoredProcedure(oCon, oCmd, "dbo.spNetCheckUserSecurity", 1, True, strRetMsg, intErrNbr) Then
                            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = strRetMsg}, New FaultReason("E_AccessDenied"))
                        End If
                    End If
                    'Check if we need to validate Form level access
                    If oParameters.FormControl > 0 Or (Not String.IsNullOrEmpty(oParameters.FormName) AndAlso oParameters.FormName.Trim.Length > 2) Then
                        'oCon is passed before it has been assigned a value because the funtion will create a connection of it does not exist
                        strRet = oQuery.getScalarValue(oCon, "Exec dbo.spNetCheckFormSecurityWCF " & oParameters.FormControl & ",'" & oParameters.FormName & "','" & oParameters.UserName & "'", 1)
                        If strRet.ToUpper <> "TRUE" Then
                            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NotAuthScreen"}, New FaultReason("E_AccessDenied"))
                        End If
                    End If                    'Check if we need to validata Procedure level access
                    If oParameters.ProcedureControl > 0 Or (Not String.IsNullOrEmpty(oParameters.ProcedureName) AndAlso oParameters.ProcedureName.Trim.Length > 2) Then
                        'oCon is passed before it has been assigned a value because the funtion will create a connection of it does not exist
                        strRet = oQuery.getScalarValue(oCon, "Exec dbo.spNetCheckProcedureSecurityWCF " & oParameters.ProcedureControl & ",'" & oParameters.ProcedureName & "','" & oParameters.UserName & "'", 1)
                        If strRet.ToUpper <> "TRUE" Then
                            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NotAuthProcedure"}, New FaultReason("E_AccessDenied"))
                        End If
                    End If
                    'Check if we need to validata Report level access
                    If oParameters.ReportControl > 0 Or (Not String.IsNullOrEmpty(oParameters.ReportName) AndAlso oParameters.ReportName.Trim.Length > 2) Then
                        'oCon is passed before it has been assigned a value because the funtion will create a connection of it does not exist
                        strRet = oQuery.getScalarValue(oCon, "Exec dbo.spNetCheckReportSecurityWCF " & oParameters.ReportControl & ",'" & oParameters.ReportName & "','" & oParameters.UserName & "'", 1)
                        If strRet.ToUpper <> "TRUE" Then
                            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NotAuthReport"}, New FaultReason("E_AccessDenied"))
                        End If
                    End If
                Catch ex As Core.DatabaseRetryExceededException
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
                Catch ex As Ngl.Core.DatabaseLogInException
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure" & ex.Message.ToString}, New FaultReason("E_DataAccessError"))
                Catch ex As Ngl.Core.DatabaseInvalidException
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
                Catch ex As System.Data.SqlClient.SqlException
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
                Catch ex As ApplicationException
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                Catch ex As FaultException
                    Throw
                Catch ex As Exception
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

            End If
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        End Try
    End Sub

    Public Function executeSQL(ByVal strSQL As String) As Boolean
        Dim blnRet As Boolean = False
        'Create a data connection 
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)

        Try

            If oQuery.executeSQLQuery(oCon, strSQL, 1) Then
                blnRet = True
            End If

        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure" & ex.Message.ToString}, New FaultReason("E_DataAccessError"))
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
                oCon.Close()
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try
        Return blnRet
    End Function

    Friend Function getScalarInteger(ByVal strSQL As String) As Integer
        'Create a data connection 
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim intRet As Integer = 0
        Try
            'oCon is passed before it has been assigned a value because the funtion will create a connection if it does not exist
            Integer.TryParse(oQuery.getScalarValue(oCon, strSQL, 1), intRet)
        Catch ex As Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure" & ex.Message.ToString}, New FaultReason("E_DataAccessError"))
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
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try

        End Try
        Return intRet
    End Function

    Friend Function getScalarString(ByVal strSQL As String) As String
        'Create a data connection 
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim strRet As String = ""
        Try
            'oCon is passed before it has been assigned a value because the funtion will create a connection if it does not exist
            strRet = oQuery.getScalarValue(oCon, strSQL, 1)
        Catch ex As Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure" & ex.Message.ToString}, New FaultReason("E_DataAccessError"))
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
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try

        End Try
        Return strRet
    End Function

    Friend Function readConfigSettings(ByVal Setting As String) As String
        Dim strRet As String = ""
        If Not ConfigDataRows Is Nothing AndAlso ConfigDataRows.Length > 0 Then
            strRet = ConfigDataRows(0)(Setting).ToString
        Else
            Dim dsXML As New System.Data.DataSet ' Data.DataSet
            Dim strPath As String = System.AppDomain.CurrentDomain.BaseDirectory.ToString()
            If Right(strPath, 1) <> "\" Then strPath &= "\"
            strPath &= "bin\config.xml"
            If Not System.IO.File.Exists(strPath) Then
                'try the current path typically used when accessing the DLL directly (not via WCF)
                strPath = System.AppDomain.CurrentDomain.BaseDirectory.ToString()
                If Right(strPath, 1) <> "\" Then strPath &= "\"
                strPath &= "config.xml"
                If Not System.IO.File.Exists(strPath) Then Throw New ApplicationException("Cannot read configuration settings in application folder or application bin folder.")
            End If
            dsXML.ReadXmlSchema(strPath)
            dsXML.ReadXml(strPath)

            Dim oTable As DataTable = dsXML.Tables(0)
            ConfigDataRows = oTable.Select()
            If ConfigDataRows.Length > 0 Then
                strRet = ConfigDataRows(0)(Setting).ToString
            Else
                Throw New ApplicationException("Cannot read configuration settings.")
            End If

        End If
        Return strRet
    End Function

#End Region
End Class

Partial Public MustInherit Class LinqEntityBase

End Class


