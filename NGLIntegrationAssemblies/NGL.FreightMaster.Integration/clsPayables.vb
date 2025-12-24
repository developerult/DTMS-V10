Imports System.Data.SqlClient
Imports Ngl.FreightMaster.Integration.Configuration
Imports DTran = Ngl.Core.Utility.DataTransformation
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports LTS = Ngl.FreightMaster.Data.LTS
Imports DAL = Ngl.FreightMaster.Data
Imports System.ServiceModel

<Serializable()> _
Public Class clsPayables : Inherits clsDownload


#Region "Constructors"

    Sub New()
        MyBase.new()
    End Sub

    Sub New(ByVal config As Ngl.FreightMaster.Core.UserConfiguration)
        MyBase.New(config)
    End Sub

    Sub New(ByVal admin_email As String, _
            ByVal from_email As String, _
            ByVal group_email As String, _
            ByVal auto_retry As Integer, _
            ByVal smtp_server As String, _
            ByVal db_server As String, _
            ByVal database_catalog As String, _
            ByVal auth_code As String, _
            ByVal debug_mode As Boolean,
            Optional ByVal connection_string As String = "")

        MyBase.New(admin_email, from_email, group_email, auto_retry, smtp_server, db_server, database_catalog, auth_code, debug_mode, connection_string)


    End Sub

#End Region

    Public Function getDataSet() As PayablesData
        Return New PayablesData
    End Function


    Public Function UpdatePayablesByFreightBill(ByVal oPayables As List(Of clsPayablesObject705), ByVal strConnection As String) As ProcessDataReturnValues
        Dim intRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationComplete
        Try
            Dim oWCFParameters As New DAL.WCFParameters
            With oWCFParameters
                .UserName = "System Download"
                .Database = Me.Database
                .DBServer = Me.DBServer
                .ConnectionString = strConnection
                .WCFAuthCode = "NGLSystem"
                .ValidateAccess = False
            End With

            Dim strValMsg As String = ""
            Dim strDatakey As String = ""
            Dim oPayablesWCFData = New DAL.NGLBookData(oWCFParameters)
            Dim oAllowUpdatePar As New AllowUpdateParameters With {.WCFParameters = oWCFParameters, .ImportType = IntegrationTypes.Payables}
            Dim CompanyReference As String = "Not Available"
            For Each oItem As clsPayablesObject705 In oPayables
                'If Not oItem Is Nothing Then
                Try
                    If oItem.CompNumber <> 0 Then
                        CompanyReference = oItem.CompNumber.ToString()
                    Else
                        CompanyReference = oItem.CompAlphaCode
                    End If
                    Dim insertFlag As Boolean
                    oAllowUpdatePar.insertFlag = insertFlag

                    oPayablesWCFData.UpdatePayablesByFreightBill(oItem.CompLegalEntity,
                                                                oItem.CompNumber,
                                                                oItem.CompAlphaCode,
                                                                oItem.CarrierNumber,
                                                                oItem.CarrierAlphaCode,
                                                                oItem.BookFinAPPayAmt,
                                                                AllowUpdate("BookFinAPPayAmt", oAllowUpdatePar),
                                                                oItem.BookFinAPCheck,
                                                                AllowUpdate("BookFinAPCheck", oAllowUpdatePar),
                                                                convertStringDateToNullableDate(oItem.BookFinAPPayDate),
                                                                AllowUpdate("BookFinAPPayDate", oAllowUpdatePar),
                                                                oItem.BookFinAPBillNumber,
                                                                AllowUpdate("BookFinAPBillNumber", oAllowUpdatePar),
                                                                oItem.BookFinAPGLNumber,
                                                                AllowUpdate("BookFinAPGLNumber", oAllowUpdatePar),
                                                                oItem.APGLDescription,
                                                                AllowUpdate("APGLDescription", oAllowUpdatePar))

                Catch ex As FaultException(Of DAL.SqlFaultInfo)
                    Me.LastError = ex.Detail.getMsgForLogs(ex.Reason.ToString())
                    AddToGroupEmailMsg(String.Format("Update Payables information failure for company {0} and Freight Bill #: {1}; {2}", CompanyReference, oItem.BookFinAPBillNumber, Me.LastError))
                    intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                Catch ex As Exception
                    Me.LastError = ex.Message
                    AddToITEmailMsg(String.Format("Unexpected Error; Could not import Payables for company {0} and Freight Bill #: {1}; Server Message: {2}", CompanyReference, oItem.BookFinAPBillNumber, Me.LastError))
                    intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                End Try
            Next

        Catch ex As Exception
            Me.LastError = ex.Message
            AddToITEmailMsg("Unexpected Payables Import system error: " & ex.ToString)
            intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
        Finally
            If GroupEmailMsg.Trim.Length > 0 Then
                LogGroupEmailError("Process Payables Data Warning", "The following errors or warnings were reported some payables records may not have been processed correctly." & GroupEmailMsg)
            End If
            If ITEmailMsg.Trim.Length > 0 Then
                LogAdminEmailError("Process Payables Data Failure", "The following errors or warnings were reported some payables records may not have been processed correctly." & ITEmailMsg)
            End If
        End Try

        Return intRet

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oPayables"></param>
    ''' <param name="strConnection"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.2.001 on 05/16/2022 removed Throw in exception to allow multiple updates if one fails
    ''' Modified by RHR for v-8.xxx on 05/18/2022 added more message details to errors
    ''' </remarks>
    Public Function ProcessObjectData70(ByVal oPayables As List(Of clsPayablesObject70), ByVal strConnection As String) As ProcessDataReturnValues
        Dim intRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationComplete
        Dim dtVal As Date
        Try
            Dim oWCFParameters As New DAL.WCFParameters
            With oWCFParameters
                .UserName = "System Download"
                .Database = Me.Database
                .DBServer = Me.DBServer
                .ConnectionString = strConnection
                .WCFAuthCode = "NGLSystem"
                .ValidateAccess = False
            End With

            Dim strValMsg As String = ""
            Dim strDatakey As String = ""
            Dim oPayablesWCFData = New DAL.NGLBookData(oWCFParameters)
            Dim oAllowUpdatePar As New AllowUpdateParameters With {.WCFParameters = oWCFParameters, .ImportType = IntegrationTypes.Payables}

            Dim sRecordDetails As String = "" 'Modified by RHR for v-8.xxx on 05/18/2022 added more message details to errors
            For Each oItem As clsPayablesObject70 In oPayables
                'If Not oItem Is Nothing Then
                'Modified by RHR for v-8.xxx on 05/18/2022 added more message details to errors
                sRecordDetails = String.Format("Legal Entity: {0}, TMS Comp Number: {1}, Order Number: {2}, Order Sequence: {3}, TMS Pro Number: {4}, Invoice Number: {5}, Pay Amount: {6}, Pay Date: {7}, Check Number: {8} ", oItem.CompLegalEntity, oItem.CompNumber, oItem.BookCarrOrderNumber, oItem.BookOrderSequence, oItem.BookProNumber, oItem.BookFinAPBillNumber, oItem.BookFinAPPayAmt, oItem.BookFinAPPayDate, oItem.BookFinAPCheck)
                Try
                    Dim insertFlag As Boolean
                    oAllowUpdatePar.insertFlag = insertFlag

                    If validateDateWS(oItem.BookFinAPPayDate, dtVal) Then
                        oItem.BookFinAPPayDate = exportDateToString(dtVal.ToString)
                    Else
                        oItem.BookFinAPPayDate = ""
                    End If

                    oPayablesWCFData.UpdatePayables70(oItem.CompLegalEntity, oItem.CompNumber, oItem.CompAlphaCode, oItem.BookCarrOrderNumber,
                                                      oItem.BookOrderSequence, oItem.BookProNumber, oItem.BookFinAPPayAmt, AllowUpdate("BookFinAPPayAmt", oAllowUpdatePar),
                                                      oItem.BookFinAPActWgt, AllowUpdate("BookFinAPActWgt", oAllowUpdatePar), oItem.BookFinAPCheck, AllowUpdate("BookFinAPCheck", oAllowUpdatePar),
                                                      oItem.BookFinAPPayDate, AllowUpdate("BookFinAPPayDate", oAllowUpdatePar), oItem.BookFinAPBillNumber, AllowUpdate("BookFinAPBillNumber", oAllowUpdatePar),
                                                      oItem.BookFinAPBillInvDate, AllowUpdate("BookFinAPBillInvDate", oAllowUpdatePar), oItem.BookFinAPGLNumber, AllowUpdate("BookFinAPGLNumber", oAllowUpdatePar),
                                                      oItem.APGLDescription, AllowUpdate("APGLDescription", oAllowUpdatePar))

                    'Modified by RHR for v-8.xxx on 05/18/2022 added more message details to errors
                Catch ex As FaultException(Of DAL.SqlFaultInfo)
                    If ex.Detail.Message = "E_ServerDetails" AndAlso Not ex.Detail.DetailsList Is Nothing AndAlso ex.Detail.DetailsList.Count > 0 Then
                        AddToGroupEmailMsg(String.Format("Update Payables information error. Server Message: {0} {1}Record Details: {2}", ex.Detail.DetailsList, vbCrLf, sRecordDetails))
                        intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                    Else
                        AddToGroupEmailMsg(String.Format("Invalid or missing payables information {0}Record Details: {1}", vbCrLf, sRecordDetails))
                        intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                    End If
                    'Modified by RHR for v-8.xxx on 05/16/2022 removed Throw in exception to allow multiple updates if one fails
                    'Throw
                Catch ex As Exception
                    AddToITEmailMsg(String.Format("Could not import Payables informaiton error. Server Message: {0} {1}Record Details: {2}", ex.Message, vbCrLf, sRecordDetails))
                    intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                End Try
                'Else
                '    AddToGroupEmailMsg("One of the payables records was null or empty and could not be processed.")
                '    intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                'End If
            Next

        Catch ex As Exception
            AddToITEmailMsg("Order import system error")
            intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
        Finally
            If GroupEmailMsg.Trim.Length > 0 Then
                LogGroupEmailError("Process Payables Data Warning", "The following errors or warnings were reported some payables records may not have been processed correctly." & GroupEmailMsg)
            End If
            If ITEmailMsg.Trim.Length > 0 Then
                LogAdminEmailError("Process Payables Data Failure", "The following errors or warnings were reported some payables records may not have been processed correctly." & ITEmailMsg)
            End If
        End Try

        Return intRet

    End Function


    Public Function ProcessObjectData( _
                        ByVal oPayables() As clsPayablesObject, _
                        ByVal strConnection As String) As ProcessDataReturnValues
        Dim intRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationFailure
        Dim oHTable As New PayablesData.PayablesHeaderDataTable
        Dim dtVal As Date

        Try
            For Each oItem As clsPayablesObject In oPayables
                Dim oRow As PayablesData.PayablesHeaderRow = oHTable.NewPayablesHeaderRow
                With oRow
                    .BookCarrOrderNumber = Left(oItem.BookCarrOrderNumber, 20)
                    .BookFinAPPayAmt = oItem.BookFinAPPayAmt
                    .BookFinAPActWgt = oItem.BookFinAPActWgt
                    .BookFinAPCheck = Left(oItem.BookFinAPCheck, 50)
                    If validateDateWS(oItem.BookFinAPPayDate, dtVal) Then
                        .BookFinAPPayDate = exportDateToString(dtVal.ToString)
                    End If
                    .BookFinAPBillNumber = Left(oItem.BookFinAPBillNumber, 20)
                    .BookFinAPBillInvDate = Left(oItem.BookFinAPBillInvDate, 20)
                    .BookFinAPGLNumber = Left(oItem.BookFinAPGLNumber, 50)
                    .APGLDescription = Left(oItem.APGLDescription, 50)
                    .CompNumber = Left(oItem.CompNumber, 50)
                    .BookPRONumber = Left(oItem.BookProNumber, 20)
                    .BookOrderSequence = oItem.BookOrderSequence
                End With
                oHTable.AddPayablesHeaderRow(oRow)
            Next

            intRet = ProcessData(oHTable, strConnection)
        Catch ex As Exception
            LogException("Process Object Data Failure", "Order import system error", AdminEmail, ex, "NGL.FreightMaster.Integration.clsPayables.ProcessObjectData Failure")
        End Try
        Return intRet


    End Function

    Public Function ProcessData( _
                ByVal oPayables As PayablesData.PayablesHeaderDataTable, _
                ByVal strConnection As String) As ProcessDataReturnValues

        Dim intRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strTitle As String = ""
        Dim strMsg As String = ""
        Dim strHeaderTable As String = "Book"
        Dim strItemTable As String = ""
        Me.HeaderName = "Book Payables"
        Me.ItemName = ""
        Me.ImportTypeKey = IntegrationTypes.Payables
        Me.CreatedDate = Now.ToString
        Me.CreateUser = "Data Integration DLL"
        Me.Source = "Payables Data Integration"
        Me.DBConnection = strConnection
        'GroupEmailMsg = ""
        'ITEmailMsg = ""
        'try the connection
        If Not Me.openConnection Then
            Return ProcessDataReturnValues.nglDataConnectionFailure
        End If


        'set the error date time stamp and other Defaults
        'Header Information
        Dim oFields As New clsImportFields
        If Not buildHeaderCollection(oFields) Then Exit Function

        Try
            'Import the Header Records
            importHeaderRecords(oPayables, oFields)
            strTitle = "Process Data Complete"
            If GroupEmailMsg.Trim.Length > 0 Then
                LogError("Process Payables Data Warning", "The following errors or warnings were reported some payables records may not have been processed correctly." & GroupEmailMsg, GroupEmail)
            End If
            If ITEmailMsg.Trim.Length > 0 Then
                LogError("Process Payables Data Failure", "The following errors or warnings were reported some payables records may not have been processed correctly." & ITEmailMsg, AdminEmail)
            End If
            If Me.TotalRecords > 0 Then
                strMsg = "Success!  " & Me.TotalRecords & " " & Me.HeaderName & " records were imported."
                intRet = ProcessDataReturnValues.nglDataIntegrationComplete
                If Me.RecordErrors > 0 Then
                    strTitle = "Process Data Complete With Errors"
                    strMsg &= vbCrLf & vbCrLf & "ERROR!  " & Me.RecordErrors & " " & Me.HeaderName & " records could not be imported.  Please run the Import Error Report for more information."
                    intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                End If

            Else
                strMsg = "No " & Me.HeaderName & " records were imported."
                intRet = ProcessDataReturnValues.nglDataIntegrationFailure
            End If
            Log(strMsg)
        Catch ex As Exception
            LogException("Process Payables Data Failure", "Could not process the requested payables data.", AdminEmail, ex, "NGL.FreightMaster.Integration.clsPayables.ProcessData")
        Finally
            closeConnection()
        End Try
        Return intRet
    End Function


    Private Function importHeaderRecords( _
        ByRef oPayables As PayablesData.PayablesHeaderDataTable, _
        ByRef oFields As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Try

            'now get the Payables Header Records
            Dim intRetryCt As Integer = 0
            Dim strSource As String = "clsPayables.importHeaderRecords"
            Dim blnDataValidated As Boolean = False
            Dim strErrorMessage As String = ""
            Dim blnInsertRecord As Boolean = True

            Do
                intRetryCt += 1
                RecordErrors = 0
                TotalRecords = 0
                Try

                    Try
                        Dim lngMax As Long = oPayables.Count
                        Log("Importing " & lngMax & " Payables Header Records.")
                        For Each oRow As PayablesData.PayablesHeaderRow In oPayables
                            'Reset the data types and values to defaults for the following fields 
                            'at the top of each loop to handle alpha vs numeric data changes
                            oFields("CompNumber").DataType = clsImportField.DataTypeID.gcvdtString
                            oFields("CompNumber").Length = 50
                            oFields("CompNumber").Null = True
                            strErrorMessage = ""
                            blnDataValidated = validateFields(oFields, oRow, strErrorMessage, strSource)
                            'Check for alpha company compatibility (note the company field tye will be changed to an integer on success)
                            If blnDataValidated Then blnDataValidated = validateCompany(oFields("CompNumber"), _
                                                                                                strErrorMessage, _
                                                                                                oCX, _
                                                                                                strSource)
                            'Only import records by Pro Number so the getProNumber check for an empty Pro and gets it based on Order Number and Company Number if needed.
                            If blnDataValidated AndAlso getProNumber(oFields) Then
                                'test if the record already exists.
                                If blnDataValidated Then blnDataValidated = doesRecordExist(oFields, _
                                                                                                    strErrorMessage, _
                                                                                                    blnInsertRecord, _
                                                                                                    "Book Pro Number " & oFields("BookProNumber").Value, _
                                                                                                    "Book")
                            End If
                            If Not blnDataValidated Then
                                addToErrorTable(oFields, "[dbo].[FileImportErrorLog]", strErrorMessage, "Data Integration DLL", mstrHeaderName)
                                RecordErrors += 1
                            Else
                                'Add the GL Data if available (not required)
                                addGL(oFields)

                                'make a copy of the fields array so the next record will maintain the default settings
                                Dim oUpdateFields As New clsImportFields
                                For Each ofield As clsImportField In oFields
                                    oUpdateFields.Add(ofield)
                                Next
                                oUpdateFields.Add("BookPayCode", "BookPayCode", clsImportField.DataTypeID.gcvdtString, 3, False)
                                oUpdateFields("BookPayCode").Value = "'PD'"
                                oUpdateFields("BookPayCode").Use = True
                                oUpdateFields("APGLDescription").Use = False 'We do not want to save this field in the book table
                                'The saveData method save data to one table only, in this case the Book Table
                                If saveData(oUpdateFields, False, "Book", "BookModUser", "BookModDate") Then
                                    TotalRecords += 1
                                End If
                            End If

                        Next
                        Return True
                    Catch ex As Exception
                        Throw
                    Finally

                    End Try
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsPayables.importHeaderRecords, attempted to import payables header records from Data Integration DLL " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
                        Log("NGL.FreightMaster.Integration.clsPayables.importHeaderRecords Failed!" & readExceptionMessage(ex))
                    Else
                        Log("importHeaderRecords Failure Retry = " & intRetryCt.ToString)
                    End If
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.                
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsPayables.importHeaderRecords, Could not import from Data Integration DLL.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsPayables.importHeaderRecords Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function



    Private Function buildHeaderCollection(ByRef oFields As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Try
            With oFields
                .Add("BookCarrOrderNumber", "BookCarrOrderNumber", clsImportField.DataTypeID.gcvdtString, 20, True, clsImportField.PKValue.gcHK)
                .Add("BookFinAPPayAmt", "BookFinAPPayAmt", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("BookFinAPActWgt", "BookFinAPActWgt", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("BookFinAPCheck", "BookFinAPCheck", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("BookFinAPPayDate", "BookFinAPPayDate", clsImportField.DataTypeID.gcvdtDate, 22, True)
                .Add("BookFinAPBillNumber", "BookFinAPBillNumber", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("BookFinAPBillInvDate", "BookFinAPBillInvDate", clsImportField.DataTypeID.gcvdtDate, 22, True)
                .Add("BookFinAPGLNumber", "BookFinAPGLNumber", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("APGLDescription", "APGLDescription", clsImportField.DataTypeID.gcvdtString, 50, True, clsImportField.PKValue.gcHK)
                .Add("BookProNumber", "BookProNumber", clsImportField.DataTypeID.gcvdtString, 20, True, clsImportField.PKValue.gcPK)
                .Add("CompNumber", "CompNumber", clsImportField.DataTypeID.gcvdtString, 50, True, clsImportField.PKValue.gcHK)
                .Add("BookOrderSequence", "BookOrderSequence", clsImportField.DataTypeID.gcvdtLongInt, 11, True, clsImportField.PKValue.gcHK)
            End With
            Log("Header Field Array Loaded.")
            'get the import field flag values
            For ct As Integer = 1 To oFields.Count
                Dim blnUseField As Boolean = True
                Try
                    If oFields(ct).Name = "BookProNumber" Or oFields(ct).Name = "CompNumber" Or oFields(ct).Name = "BookCarrOrderNumber" Or oFields(ct).Name = "BookOrderSequence" Then
                        blnUseField = True
                    Else
                        blnUseField = getImportFieldFlag(oFields(ct).Name, IntegrationTypes.Payables)
                    End If

                Catch ex As Exception
                    'throw away any errors in case one or more field names is missing in the Import Field Flag Table
                End Try
                oFields(ct).Use = blnUseField
            Next
            Ret = True
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsPayables.buildHeaderCollection, could not build the header collection data.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsPayables.buildHeaderCollection Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function

    Private Function addGL(ByVal oFields As clsImportFields) As Boolean
        Dim blnRet As Boolean = False
        Try

            If oFields("BookFinAPGLNumber").Use AndAlso Not oFields("BookFinAPGLNumber").Value Is Nothing AndAlso oFields("BookFinAPGLNumber").Value.ToUpper <> "NULL" AndAlso oFields("BookFinAPGLNumber").Value > "" Then
                'test for the Chart of Account Record and add it if it does not exist.
                Dim strSQL As String = "Select AcctNo from dbo.ChartOfAccounts where AcctNo = " & oFields("BookFinAPGLNumber").Value
                Dim intRetryCt As Integer = 0
                Do
                    intRetryCt += 1
                    Dim cmdObj As New System.Data.SqlClient.SqlCommand
                    Dim drTemp As SqlDataReader
                    Try
                        'check the active db connection
                        If Me.openConnection Then
                            With cmdObj
                                .Connection = DBCon
                                .CommandTimeout = 300
                                .CommandText = strSQL
                                .CommandType = CommandType.Text
                                drTemp = .ExecuteReader()
                            End With
                            Dim blnHasRows As Boolean = drTemp.HasRows
                            Try
                                drTemp.Close()
                            Catch ex As Exception

                            End Try
                            If blnHasRows AndAlso oFields("APGLDescription").Use Then
                                strSQL = "Update dbo.ChartOfAccounts Set AcctDescription = " & oFields("APGLDescription").Value & " Where AcctNo = " & oFields("BookFinAPGLNumber").Value
                                cmdObj.CommandText = strSQL
                                cmdObj.ExecuteScalar()
                            ElseIf oFields("APGLDescription").Use Then
                                strSQL = "Insert Into dbo.ChartOfAccounts (AcctNo, AcctDescription) Values (" & oFields("BookFinAPGLNumber").Value & "," & oFields("APGLDescription").Value & ")"
                                cmdObj.CommandText = strSQL
                                cmdObj.ExecuteScalar()
                            Else
                                strSQL = "Insert Into dbo.ChartOfAccounts (AcctNo) Values (" & oFields("BookFinAPGLNumber").Value & ")"
                                cmdObj.CommandText = strSQL
                                cmdObj.ExecuteScalar()
                            End If
                            blnRet = True
                            Exit Do
                        Else
                            If intRetryCt > Me.Retry Then
                                ITEmailMsg &= "<br />" & Source & " Warning: NGL.FreightMaster.Integration.clsPayables.addGL, Open database connection failure (import not affected),  attempted to add GL data for order number " & oFields("BookCarrOrderNumber").Value.ToString & " " & intRetryCt.ToString & " times without success." & "<br />" & vbCrLf
                                Log("NGL.FreightMaster.Integration.clsPayables.addGL Warning!")
                            Else
                                Log("addGL Open DB Connection Failure Retry = " & intRetryCt.ToString)
                            End If
                        End If
                    Catch ex As Exception
                        If intRetryCt > Me.Retry Then
                            ITEmailMsg &= "<br />" & Source & " Warning: NGL.FreightMaster.Integration.clsPayables.addGL import not affected), attempted to add GL data for order number " & oFields("BookCarrOrderNumber").Value.ToString & " " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<hr \>" & vbCrLf & strSQL & "<hr \>" & vbCrLf
                            Log("NGL.FreightMaster.Integration.clsPayables.addGL Warning!")
                        Else
                            Log("addGL Failure Retry = " & intRetryCt.ToString)
                        End If
                    Finally
                        Try
#Disable Warning BC42104 ' Variable 'drTemp' is used before it has been assigned a value. A null reference exception could result at runtime.
                            drTemp.Close()
#Enable Warning BC42104 ' Variable 'drTemp' is used before it has been assigned a value. A null reference exception could result at runtime.
                        Catch ex As Exception

                        End Try
                        Try
                            cmdObj.Cancel()
                        Catch ex As Exception

                        End Try

                    End Try
                    'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
                Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
            Else
                blnRet = True
            End If
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Warning: NGL.FreightMaster.Integration.clsPayables.addGL (import not affected), could not add GL data for order number " & oFields("BookCarrOrderNumber").Value.ToString & "<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsPayables.addGL Warning!")
        End Try
        Return blnRet
    End Function

End Class
