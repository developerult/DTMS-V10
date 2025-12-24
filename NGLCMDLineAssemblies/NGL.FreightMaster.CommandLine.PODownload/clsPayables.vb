Imports ngl.FreightMaster.FMLib.General
Imports ngl.FreightMaster.FMLib.dbUtilities
Imports ngl.FreightMaster.FMLib.PCMiles
Public Class clsPayables : Inherits clsDownload

    Public Sub FileImport(ByRef strHeaderFile As String, ByRef strItemFile As String, ByRef strDataPath As String)

        Dim objImportCon As ADODB.Connection
        Dim shtServerID As Short = 0
        Dim oPCM As New FMLib.PCMiles
        Dim strTitle As String = ""
        Dim strMsg As String = ""
        Dim strHeaderTable As String = "Book"
        Dim strItemTable As String = ""

        'Check if the header file exists
        If Not System.IO.File.Exists(objF.buildPath(strDataPath, strHeaderFile)) Then
            Log("The import header file " & objF.buildPath(strDataPath, strHeaderFile) & "does not exists.")
            Exit Sub
        End If
        'check the active background connection
        If Not FMLib.dbUtilities.OpenConnection() Then
            Log("Cannot Open DB Connection")
            Exit Sub
        End If
        Log("DB Connection Open")
        'set the error date time stamp and other Defaults
        'Header Information
        Dim oFields As New clsImportFields
        If Not buildHeaderCollection(oFields) Then Exit Sub
        If Not openFileDbConnection(objImportCon, strDataPath) Then Exit Sub
        Try
            If useOptimizer(OptimizerTypes.optPCMiler) Then
                shtServerID = oPCM.PCMilerStart()
            End If
            If shtServerID > 0 Then
                Log("PCMiler Started.")
            Else
                Log("PCMiler Not Available.")
            End If
            'Import the Header Records
            importHeaderRecords(objImportCon, oFields, oPCM, strHeaderFile)
            strTitle = "File Import Complete"
            If Me.TotalRecords > 0 Then
                strMsg = "Success!  " & Me.TotalRecords & " " & strHeaderTable & " records were imported."
                If Me.RecordErrors > 0 Then
                    strTitle = "File Import Complete With Errors"
                    strMsg &= vbCrLf & vbCrLf & "ERROR!  " & Me.RecordErrors & " " & strHeaderTable & " records could not be imported.  Please run the File Import Error Report for more information."
                End If
            Else
                strMsg = "No " & strHeaderTable & " records were imported."
            End If
            Log(strMsg)

            SaveSetting("NGL2002", "FileImport", "DataPath", strDataPath)
            SaveSetting("NGL2002", "FileImport", mstrHeaderName & "File", strHeaderFile)
            'SaveSetting("NGL2002", "FileImport", mstrItemName & "File", strItemFile)
        Catch ex As Exception

        Finally
            oPCM.PCMilerEnd()
        End Try

    End Sub


    Private Function importHeaderRecords( _
        ByRef objImportCon As ADODB.Connection, _
        ByRef oFields As clsImportFields, _
        ByRef oPCM As FMLib.PCMiles, _
        ByVal strHeaderFile As String) As Boolean
        Dim Ret As Boolean = False
        Try

            'now get the Payables Header Records
            Dim objImportRS As New ADODB.Recordset
            Dim strSQL As String = "Select * From " & strHeaderFile
            Dim intRetryCt As Integer = 0
            Dim strSource As String = "clsPayables.importHeaderRecords"
            Dim blnValidationError As Boolean = False
            Dim strErrorMessage As String = ""
            Dim blnInsertRecord As Boolean = True

            Do
                intRetryCt += 1
                RecordErrors = 0
                TotalRecords = 0
                Try
                    objImportRS.Open(strSQL, objImportCon, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText)
                    Try
                        Log("Import Records Open")
                        If Not objImportRS.EOF Then objImportRS.MoveLast()
                        If Not objImportRS.BOF Then objImportRS.MoveFirst()
                        Dim lngMax As Long = objImportRS.RecordCount
                        Log("Importing " & lngMax & " Payables Header Records.")
                        Do Until objImportRS.EOF

                            For intct As Integer = 1 To oFields.Count
                                If oFields(intct).Use Then
                                    oFields(intct).Value = validateSQLValue( _
                                        objImportRS.Fields(oFields(intct).Name) _
                                        , CInt(oFields(intct).DataType) _
                                        , strSource _
                                        , "Invalid " & oFields(intct).Name _
                                        , oFields(intct).Null _
                                        , oFields(intct).Length)
                                End If
                            Next

                            'test if the record already exists.
                            Dim intRet As Integer = doesRecordExist(oFields, "Book")
                            Select Case intRet
                                Case -1
                                    blnValidationError = True
                                    strErrorMessage = "Could not check for existing payables information in the book table.  The order number " & oFields("BookCarrOrderNumber").Value.ToString & " has not been downloaded."
                                Case 0 'no records found so we cannot update the payables data
                                    blnValidationError = True
                                    strErrorMessage = "Could import payables data the order number " & oFields("BookCarrOrderNumber").Value.ToString & ", it does not have a valid booking record and has not been downloaded."
                                Case Else
                                    blnInsertRecord = False
                            End Select

                            If blnValidationError Then
                                addToErrorTable(oFields, "[dbo].[FileImportErrorLog]", strErrorMessage, strHeaderFile, mstrHeaderName)
                                RecordErrors += 1
                            Else
                                'Add the GL Data if available (not required)
                                addGL(oFields)
                                'Save the changes to the main table
                                If saveData(oFields, False, "Book", "BookModUser", "BookModDate") Then
                                    TotalRecords += 1
                                End If
                            End If
                            'Debug.Print strSQL
                            blnInsertRecord = False
                            strErrorMessage = ""
                            blnValidationError = False
                            objImportRS.MoveNext()
                        Loop
                        Return True
                    Catch ex As Exception
                        Throw
                    Finally
                        objImportRS.Close()
                    End Try
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        LogError("PODownload Failure", "PODownload.clsPayables.importHeaderRecords: Could not import from file " & strHeaderFile & vbCrLf & ex.ToString & vbCrLf & strSQL, AdminEmail)
                    Else
                        Log("importHeaderRecords Failure Retry = " & intRetryCt.ToString)

                    End If
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.                
        Catch ex As Exception
            LogError("PODownload Failure", "PODownload.clsPayables.importHeaderRecords: Could not import from file " & strHeaderFile & vbCrLf & ex.ToString, AdminEmail)
        End Try
        Return Ret

    End Function



    Private Function buildHeaderCollection(ByRef oFields As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Try
            With oFields
                .Add("BookCarrOrderNumber", "BookCarrOrderNumber", clsImportField.DataTypeID.gcvdtString, 20, False, clsImportField.PKValue.gcPK)
                .Add("BookFinAPPayAmt", "BookFinAPPayAmt", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("BookFinAPActWgt", "BookFinAPActWgt", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("BookFinAPCheck", "BookFinAPCheck", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("BookFinAPPayDate", "BookFinAPPayDate", clsImportField.DataTypeID.gcvdtDate, 20, True)
                .Add("BookFinAPBillNumber", "BookFinAPBillNumber", clsImportField.DataTypeID.gcvdtString, 20, True)
                .Add("BookFinAPBillInvDate", "BookFinAPBillInvDate", clsImportField.DataTypeID.gcvdtDate, 20, True)
                .Add("BookFinAPGLNumber", "BookFinAPGLNumber", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("APGLDescription", "APGLDescription", clsImportField.DataTypeID.gcvdtString, 50, True)
            End With
            Log("Header Field Array Loaded.")
            'get the import field flag values
            For ct As Integer = 1 To oFields.Count
                Dim blnUseField As Boolean = True
                Try
                    blnUseField = CBool(getImportFieldFlag(oFields(ct).Name, ImportFileTypes.ietPayables))
                Catch ex As Exception
                    'throw away any errors in case one or more field names is missing in the Import Field Flag Table
                End Try
                oFields(ct).Use = blnUseField
            Next
            Ret = True
        Catch ex As Exception
            LogError("PODownload Failure", "PODownload.clsPayables.buildHeaderCollection failure" & vbCrLf & ex.ToString, AdminEmail)
        End Try
        Return Ret

    End Function

    Private Function addGL(ByVal oFields As clsImportFields) As Boolean
        Dim blnRet As Boolean = False
        Try

            If oFields("BookFinAPGLNumber").Use Then
                'test for the Chart of Account Record and add it if it does not exist.
                Dim strSQL As String = "Select AcctNo from dbo.ChartOfAccounts where AcctNo = " & oFields("BookFinAPGLNumber").Value
                Dim intRetryCt As Integer = 0
                Do
                    intRetryCt += 1
                    Try
                        'check the active db connection
                        If FMLib.dbUtilities.OpenConnection() Then
                            Dim objTestRS As New ADODB.Recordset
                            With objTestRS
                                .Open(strSQL, gDBCon, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)
                                Try
                                    If .BOF Or .EOF Then
                                        .AddNew()
                                        .Fields("AcctNo").Value = stripQuotes(oFields("BookFinAPGLNumber").Value)
                                    End If
                                    If oFields("APGLDescription").Use Then
                                        .Fields("AcctDescription").Value = stripQuotes(oFields("APGLDescription").Value)
                                    End If
                                    .Update()
                                Catch ex As Exception
                                    Throw
                                Finally
                                    .Close()
                                End Try
                            End With
                            blnRet = True
                            Exit Do
                        Else
                            If intRetryCt > Me.Retry Then
                                LogError("PODownload error import not affected", "PODownload.clsPayables.addGL: Open database connection failure (import not affected); order number " & oFields("BookCarrOrderNumber").Value.ToString & " could not be processed correctly.", GroupEmail)
                            Else
                                Log("addGL Open DB Connection Failure Retry = " & intRetryCt.ToString)
                            End If
                        End If
                    Catch ex As Exception
                        If intRetryCt > Me.Retry Then
                            LogError("PODownload error import not affected", "PODownload.clsPayables.addGL error (import not affected) for order number " & oFields("BookCarrOrderNumber").Value.ToString & vbCrLf & ex.ToString & vbCrLf & strSQL, GroupEmail)
                        Else
                            Log("addGL Failure Retry = " & intRetryCt.ToString)

                        End If
                    End Try
                    'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
                Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
            Else
                blnRet = True
            End If
        Catch ex As Exception
            LogError("PODownload error import not affected", "PODownload.clsPayables.addGL error (import not affected) for order number " & oFields("BookCarrOrderNumber").Value.ToString & vbCrLf & ex.ToString, GroupEmail)
        End Try
        Return blnRet
    End Function

End Class
