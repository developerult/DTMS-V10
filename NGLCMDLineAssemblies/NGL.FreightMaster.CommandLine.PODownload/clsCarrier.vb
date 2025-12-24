
Imports ngl.FreightMaster.FMLib.General
Imports ngl.FreightMaster.FMLib.dbUtilities
Imports ngl.FreightMaster.FMLib.PCMiles

Public Class clsCarrier : Inherits clsDownload

    Public Sub FileImport(ByRef strHeaderFile As String, ByRef strItemFile As String, ByRef strDataPath As String)

        Dim objImportCon As ADODB.Connection
        Dim shtServerID As Short = 0
        Dim oPCM As New FMLib.PCMiles
        Dim strTitle As String = ""
        Dim strMsg As String = ""
        Dim strHeaderTable As String = "Carrier"
        Dim strItemTable As String = "CarrierCont"

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
        'Item/Detail Information
        Dim oItems As New clsImportFields
        If Not buildItemCollection(oItems) Then Exit Sub
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
            If importHeaderRecords(objImportCon, oFields, oPCM, strHeaderFile) Then
                'Now Import the Details
                'Check if the detail file exists
                If Not System.IO.File.Exists(objF.buildPath(strDataPath, strItemFile)) Then
                    Log("The import item file " & objF.buildPath(strDataPath, strItemFile) & "does not exists.")
                Else
                    importItemRecords(objImportCon, oItems, oPCM, strItemFile)
                End If
            End If
            strTitle = "File Import Complete"
            If Me.TotalRecords > 0 Then
                strMsg = "Success!  " & Me.TotalRecords & " " & strHeaderTable & " records were imported."
                If Me.TotalItems > 0 Then
                    strMsg &= vbCrLf & vbCrLf & " And " & Me.TotalItems & " " & strItemTable & " records were imported."

                End If
                If Me.RecordErrors > 0 Or Me.ItemErrors > 0 Then
                    strTitle = "File Import Complete With Errors"
                    If Me.RecordErrors > 0 Then
                        strMsg &= vbCrLf & vbCrLf & "ERROR!  " & Me.RecordErrors & " " & strHeaderTable & " records could not be imported.  Please run the File Import Error Report for more information."
                    End If
                    If Me.ItemErrors > 0 Then
                        strMsg &= vbCrLf & vbCrLf & "ERROR!  " & Me.ItemErrors & " " & strItemTable & " records could not be imported.  Please run the File Import Error Report for more information."
                    End If
                End If
            Else
                strMsg = "No " & strHeaderTable & " records were imported."
            End If
            Log(strMsg)

            SaveSetting("NGL2002", "FileImport", "DataPath", strDataPath)
            SaveSetting("NGL2002", "FileImport", mstrHeaderName & "File", strHeaderFile)
            SaveSetting("NGL2002", "FileImport", mstrItemName & "File", strItemFile)
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

            'now get the Carrier Header Records
            Dim objImportRS As New ADODB.Recordset
            Dim strSQL As String = "Select * From " & strHeaderFile
            Dim intRetryCt As Integer = 0
            Dim strSource As String = "clsCarrier.importHeaderRecords"
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
                        Log("Importing " & lngMax & " Carrier Header Records.")
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
                            Dim intRet As Integer = doesRecordExist(oFields, "Carrier")
                            Select Case intRet
                                Case -1
                                    blnValidationError = True
                                    strErrorMessage = "Could not check for existing Carrier record.  The Carrier number " & oFields("CarrierNumber").Value & " has not been downloaded."
                                Case 0 'no records found so insert a new one
                                    blnInsertRecord = True
                                Case Else
                                    blnInsertRecord = False
                            End Select

                            If blnValidationError Then
                                addToErrorTable(oFields, "[dbo].[FileImportErrorLog]", strErrorMessage, strHeaderFile, mstrHeaderName)
                                RecordErrors += 1
                            Else
                                'Save the changes to the main table
                                If saveData(oFields, blnInsertRecord, "Carrier", "CarrierModUser", "CarrierModDate") Then
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
                        LogError("PODownload Failure", "PODownload.clsCarrier.importHeaderRecords: Could import from file " & strHeaderFile & vbCrLf & ex.ToString & vbCrLf & strSQL, AdminEmail)
                    Else
                        Log("importHeaderRecords Failure Retry = " & intRetryCt.ToString)

                    End If
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.                
        Catch ex As Exception
            LogError("PODownload Failure", "PODownload.clsCarrier.importHeaderRecords: Could import from file " & strHeaderFile & vbCrLf & ex.ToString, AdminEmail)
        End Try
        Return Ret

    End Function

    Private Function importItemRecords( _
        ByRef objImportCon As ADODB.Connection, _
        ByRef oItems As clsImportFields, _
        ByRef oPCM As FMLib.PCMiles, _
        ByVal strFile As String) As Boolean
        Dim Ret As Boolean = False
        Try
            'now get the Carrier Contact Records
            Dim objImportRS As New ADODB.Recordset
            Dim strSQL As String = "Select * From " & strFile
            Dim intRetryCt As Integer = 0
            Dim strSource As String = "clsCarrier.importItemRecords"
            Dim blnValidationError As Boolean = False
            Dim strErrorMessage As String = ""
            Dim blnInsertRecord As Boolean = True

            Do
                intRetryCt += 1
                ItemErrors = 0
                TotalItems = 0
                Try
                    objImportRS.Open(strSQL, objImportCon, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText)
                    Try
                        Log("Import Records Open")
                        If Not objImportRS.EOF Then objImportRS.MoveLast()
                        If Not objImportRS.BOF Then objImportRS.MoveFirst()
                        Dim lngMax As Long = objImportRS.RecordCount
                        Log("Importing " & lngMax & " Carrier Contact Records.")
                        Do Until objImportRS.EOF

                            For intct As Integer = 1 To oItems.Count
                                If CInt(oItems(intct).PK) <> gcFK And oItems(intct).Use Then
                                    oItems(intct).Value = validateSQLValue( _
                                        objImportRS.Fields(oItems(intct).Name) _
                                        , CInt(oItems(intct).DataType) _
                                        , strSource _
                                        , "Invalid " & oItems(intct).Name _
                                        , oItems(intct).Null _
                                        , oItems(intct).Length)
                                End If
                            Next
                            'test if the record already exists.
                            Dim intRet As Integer = doesRecordExist(oItems, "CarrierCont")
                            Select Case intRet
                                Case -1
                                    blnValidationError = True
                                    strErrorMessage = "Could not check for existing Carrier contact record.  The Carrier contact for Carrier number " & oItems("CarrierNumber").Value & " has not been downloaded."
                                Case 0 'no records found so insert a new one
                                    blnInsertRecord = True
                                Case Else
                                    blnInsertRecord = False
                            End Select

                            If blnValidationError Then
                                addToErrorTable(oItems, "[dbo].[FileDetailsErrorLog]", strErrorMessage, strFile, mstrHeaderName)
                                ItemErrors += 1
                            Else
                                'Save the changes
                                If saveData(oItems, blnInsertRecord, "CarrierCont", "", "") Then
                                    TotalItems += 1
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
                        LogError("PODownload Failure", "PODownload.clsCarrier.importItemRecords: Could import from file " & strFile & vbCrLf & ex.ToString & vbCrLf & strSQL, AdminEmail)
                    Else
                        Log("importItemRecords Failure Retry = " & intRetryCt.ToString)

                    End If
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.                
        Catch ex As Exception
            LogError("PODownload Failure", "PODownload.clsCarrier.importItemRecords: Could import from file " & strFile & vbCrLf & ex.ToString, AdminEmail)
        End Try
        Return Ret

    End Function

    Private Function buildHeaderCollection(ByRef oFields As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Try
            With oFields
                .Add("CarrierNumber", "CarrierNumber", clsImportField.DataTypeID.gcvdtLongInt, 11, False, clsImportField.PKValue.gcPK)
                .Add("CarrierName", "CarrierName", clsImportField.DataTypeID.gcvdtString, 40, False)
                .Add("CarrierStreetAddress1", "CarrierStreetAddress1", clsImportField.DataTypeID.gcvdtString, 40, True)
                .Add("CarrierStreetAddress2", "CarrierStreetAddress2", clsImportField.DataTypeID.gcvdtString, 40, True)
                .Add("CarrierStreetAddress3", "CarrierStreetAddress3", clsImportField.DataTypeID.gcvdtString, 40, True)
                .Add("CarrierStreetCity", "CarrierStreetCity", clsImportField.DataTypeID.gcvdtString, 25, True)
                .Add("CarrierStreetState", "CarrierStreetState", clsImportField.DataTypeID.gcvdtString, 8, True)
                .Add("CarrierStreetCountry", "CarrierStreetCountry", clsImportField.DataTypeID.gcvdtString, 30, True)
                .Add("CarrierStreetZip", "CarrierStreetZip", clsImportField.DataTypeID.gcvdtString, 10, True)
                .Add("CarrierMailAddress1", "CarrierMailAddress1", clsImportField.DataTypeID.gcvdtString, 40, True)
                .Add("CarrierMailAddress2", "CarrierMailAddress2", clsImportField.DataTypeID.gcvdtString, 40, True)
                .Add("CarrierMailAddress3", "CarrierMailAddress3", clsImportField.DataTypeID.gcvdtString, 40, True)
                .Add("CarrierMailCity", "CarrierMailCity", clsImportField.DataTypeID.gcvdtString, 25, True)
                .Add("CarrierMailState", "CarrierMailState", clsImportField.DataTypeID.gcvdtString, 8, True)
                .Add("CarrierMailCountry", "CarrierMailCountry", clsImportField.DataTypeID.gcvdtString, 30, True)
                .Add("CarrierMailZip", "CarrierMailZip", clsImportField.DataTypeID.gcvdtString, 10, True)
                .Add("CarrierTypeCode", "CarrierTypeCode", clsImportField.DataTypeID.gcvdtString, 1, True)
                .Add("CarrierSCAC", "CarrierSCAC", clsImportField.DataTypeID.gcvdtString, 4, True)
                .Add("CarrierWebSite", "CarrierWeb", clsImportField.DataTypeID.gcvdtString, 255, True)
                .Add("CarrierEmail", "CarrierEmail", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("CarrierQualInsuranceDate", "CarrierQualInsuranceDate", clsImportField.DataTypeID.gcvdtDate, 19, True)
                .Add("CarrierQualQualified", "CarrierQualQualified", clsImportField.DataTypeID.gcvdtBit, 2, True)
                .Add("CarrierQualAuthority", "CarrierQualAuthority", clsImportField.DataTypeID.gcvdtString, 15, True)
                .Add("CarrierQualContract", "CarrierQualContract", clsImportField.DataTypeID.gcvdtBit, 2, True)
                .Add("CarrierQualSignedDate", "CarrierQualSignedDate", clsImportField.DataTypeID.gcvdtDate, 19, True)
                .Add("CarrierQualContractExpiresDate", "CarrierQualContractExpiresDate", clsImportField.DataTypeID.gcvdtDate, 19, True)
            End With
            Log("Header Field Array Loaded.")
            'get the import field flag values
            For ct As Integer = 1 To oFields.Count
                Dim blnUseField As Boolean = True
                Try
                    blnUseField = CBool(getImportFieldFlag(oFields(ct).Name, ImportFileTypes.ietCarrier))
                Catch ex As Exception
                    'throw away any errors in case one or more field names is missing in the Import Field Flag Table
                End Try
                oFields(ct).Use = blnUseField
            Next
            Ret = True
        Catch ex As Exception
            LogError("PODownload Failure", "PODownload.clsCarrier.buildHeaderCollection failure" & vbCrLf & ex.ToString, AdminEmail)
        End Try
        Return Ret

    End Function

    Private Function buildItemCollection(ByRef oItems As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Try
            With oItems
                .Add("CarrierContCarrierControl", "CarrierContCarrierControl", clsImportField.DataTypeID.gcvdtLongInt, 11, False, clsImportField.PKValue.gcFK, 2, "CarrierNumber", "CarrierControl")
                .Add("CarrierNumber", "CarrierNumber", clsImportField.DataTypeID.gcvdtString, 50, False, clsImportField.PKValue.gcHK)
                .Add("CarrierContName", "CarrierContName", clsImportField.DataTypeID.gcvdtString, 25, True, clsImportField.PKValue.gcPK)
                .Add("CarrierContTitle", "CarrierContTitle", clsImportField.DataTypeID.gcvdtString, 25, True)
                .Add("CarrierCont800", "CarrierCont800", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("CarrierContPhone", "CarrierContPhone", clsImportField.DataTypeID.gcvdtString, 15, True)
                .Add("CarrierContFax", "CarrierContFax", clsImportField.DataTypeID.gcvdtString, 15, True)
                .Add("CarrierContEMail", "CarrierContEMail", clsImportField.DataTypeID.gcvdtString, 50, True)
            End With
            'get the item  field flag values
            For ct As Integer = 1 To oItems.Count
                Dim blnUseField As Boolean = True
                Try
                    blnUseField = CBool(getImportFieldFlag(oItems(ct).Name, ImportFileTypes.ietCarrier))
                Catch ex As Exception
                    'throw away any errors in case one or more field names is missing in the Import Field Flag Table
                End Try
                oItems(ct).Use = blnUseField
            Next
            Ret = True
        Catch ex As Exception
            LogError("PODownload Failure", "PODownload.clsCarrier.buildItemCollection failure" & vbCrLf & ex.ToString, AdminEmail)
        End Try
        Return Ret

    End Function
End Class
