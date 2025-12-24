
Imports ngl.FreightMaster.FMLib.General
Imports ngl.FreightMaster.FMLib.dbUtilities
Imports ngl.FreightMaster.FMLib.PCMiles

Public Class clsCompany : Inherits clsDownload

    Public Sub FileImport(ByRef strHeaderFile As String, ByRef strItemFile As String, ByRef strDataPath As String)

        Dim objImportCon As ADODB.Connection
        Dim shtServerID As Short = 0
        Dim oPCM As New FMLib.PCMiles
        Dim strTitle As String = ""
        Dim strMsg As String = ""
        Dim strHeaderTable As String = "Comp"
        Dim strItemTable As String = "CompCont"

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
        'set the default for the use LaneLatitude and LaneLongitude values
        Dim blnUseLaneLat As Boolean = oFields("CompLatitude").Use
        Dim blnUseLaneLong As Boolean = oFields("CompLongitude").Use
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



    Private Sub calcLatLong( _
        ByRef oPCM As FMLib.PCMiles, _
        ByRef oFields As clsImportFields)

        Dim blnGetGeoCode As Boolean = False
        Dim strlocation As String = ""
        Try
            'Test if a zip code is possible and the 
            'the import flags for lat and long are false
            'we only modify if the import flag is false else
            'the source controls the Lat Long
            If oFields("CompStreetZip").Use _
                AndAlso oFields("CompLatitude").Use = False _
                AndAlso oFields("CompLongitude").Use = False Then
                strlocation = zipClean(oFields("CompStreetZip").Value)
                Dim dblLat As Double = 0
                Dim dblLong As Double = 0

                If oPCM.getGeoCode(strlocation, dblLat, dblLong) Then
                    oFields("CompLatitude").Value = CStr(dblLat)
                    oFields("CompLatitude").Use = True
                    oFields("CompLongitude").Value = CStr(dblLong)
                    oFields("CompLongitude").Use = True
                    Log("Lat/Long = " & dblLat.ToString & "/" & dblLong.ToString)
                Else
                    LogError("PODownload error import not affected", "PODownload.clsCarrier.calcLatLong: Calc Lat/Long error but import not affected" & vbCrLf & oPCM.LastError, Me.AdminEmail)
                End If
            End If
        Catch ex As Exception
            LogError("PODownload error import not affected", "PODownload.clsCarrier.calcLatLong: Calc Lat/Long error but import not affected" & vbCrLf & ex.ToString, Me.AdminEmail)
        End Try

    End Sub

    Private Function createNewCompNumber(ByVal oNumber As clsImportField) As Boolean

        Dim Ret As Boolean = False
        Dim strSQL As String = ""
        Dim strValues As String = ""
        Dim blnFirstField As Boolean = True
        Dim intCompNumber As Integer = 0
        Try
            Dim intRetryCt As Integer = 0
            Do
                intRetryCt += 1
                Try
                    'check the active db connection
                    If FMLib.dbUtilities.OpenConnection() Then
                        'Get the current highest company number available
                        strSQL = "Select top 1 CompNumber from Comp Order By CompNumber Desc"
                        Dim objTestRS As New ADODB.Recordset
                        With objTestRS
                            .Open(strSQL, gDBCon, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)
                            Try
                                If .RecordCount > 0 Then
                                    intCompNumber = Val(nz(.Fields("CompNumber"), 0))
                                End If
                            Catch ex As Exception
                                Throw
                            Finally
                                .Close()
                            End Try
                        End With
                        'add 1 to the value
                        intCompNumber += 1
                        'build execute string to insert record into dbo.AlphaCompanyXref
                        strSQL = "Insert Into dbo.AlphaCompanyXref (ACXCompNumber,ACXAlphaNumber)"
                        strSQL &= " Values ( " & intCompNumber & " , '" & oNumber.Value & "')"
                        'debug.print " Insert AlphaCompanyXref SQL = " & strSQL
                        gDBCon.Execute(strSQL)
                        LogError("PODownload notification download not affected", "PODownload.clsCompany.createNewCompNumber: A new company number was created (Downloads were not affected)" & vbCrLf & "The alpha company number " & oNumber.Value.ToString & " did not exist in the AlphaCompanyXref table.  A new cross reference was created using numeric company number " & intCompNumber.ToString & ".", GroupEmail)
                        oNumber.Value = intCompNumber
                        Ret = True
                        Exit Do
                    Else
                        If intRetryCt > Me.Retry Then
                            LogError("PODownload Failure", "PODownload.clsCompany.createNewCompNumber: Open database connection failure" & vbCrLf & "Could not write to AlphaCompanyXref table.", AdminEmail)
                        Else
                            Log("createNewCompNumber Open DB Connection Failure Retry = " & intRetryCt.ToString)
                        End If
                    End If
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        LogError("PODownload Failure", "PODownload.clsCompany.createNewCompNumber: Could not write to  AlphaCompanyXref table" & vbCrLf & ex.ToString & vbCrLf & strSQL, AdminEmail)
                    Else
                        Log("createNewCompNumber Failure Retry = " & intRetryCt.ToString)

                    End If
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
        Catch ex As Exception
            LogError("PODownload Failure", "PODownload.clsCompany.createNewCompNumber: Could not write to  AlphaCompanyXref table" & vbCrLf & ex.ToString, AdminEmail)
        End Try
        Return Ret
    End Function

    Private Function importHeaderRecords( _
        ByRef objImportCon As ADODB.Connection, _
        ByRef oFields As clsImportFields, _
        ByRef oPCM As FMLib.PCMiles, _
        ByVal strHeaderFile As String) As Boolean
        Dim Ret As Boolean = False
        Try

            'now get the Comp Header Records
            Dim objImportRS As New ADODB.Recordset
            Dim strSQL As String = "Select * From " & strHeaderFile
            Dim intRetryCt As Integer = 0
            Dim blnUseLaneLat As Boolean = oFields("CompLatitude").Use
            Dim blnUseLaneLong As Boolean = oFields("CompLongitude").Use
            Dim strSource As String = "clsCompany.importHeaderRecords"
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
                        Log("Importing " & lngMax & " Company Header Records.")
                        Do Until objImportRS.EOF
                            'Reset the data types and values to defaults for the following fields 
                            'at the top of each loop to handle alpha vs numeric data changes
                            oFields("CompNumber").Name = "CompNumber"
                            oFields("CompNumber").DataType = clsImportField.DataTypeID.gcvdtString
                            oFields("CompNumber").Length = 50
                            oFields("CompNumber").Null = True
                            'we need to reset the Use Lat and Long values
                            oFields("CompLatitude").Use = blnUseLaneLat
                            oFields("CompLongitude").Use = blnUseLaneLong

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
                            'Check the alpha Xref table for a Company Number
                            If Not lookupCompNumberByAlphaCode(oFields("CompNumber")) Then
                                blnValidationError = True
                                strErrorMessage = "Could not validate Company Number in Alpha Xref Table  " & oFields("CompNumber").Value
                            End If

                            Dim blnNewCompNumber As Boolean = False
                            If Not blnValidationError And Not IsNumeric(oFields("CompNumber").Value) Or oFields("CompNumber").Value = "0" Then
                                'we do not have a valid company number so we need to create one. and send an email to all about the new value
                                If Not createNewCompNumber(oFields("CompNumber")) Then
                                    blnValidationError = True
                                    strErrorMessage = "Could not create a new Company Number in Alpha Xref Table For " & oFields("CompNumber").Value
                                End If
                                blnNewCompNumber = True
                            End If

                            If Not blnValidationError Then
                                oFields("CompNumber").DataType = clsImportField.DataTypeID.gcvdtLongInt
                                oFields("CompNumber").Length = 11
                                'test if the record already exists.
                                Dim blnReadyToSave As Boolean = False
                                intRetryCt = 0
                                Do
                                    intRetryCt += 1
                                    Dim intRet As Integer = doesRecordExist(oFields, "Comp")
                                    Select Case intRet
                                        Case -1
                                            blnValidationError = True
                                            strErrorMessage = "Could not check for existing company record.  The company number " & oFields("CompNumber").Value & " has not been downloaded."
                                            Exit Do
                                        Case 0 'no records found so insert a new one
                                            blnInsertRecord = True
                                            blnReadyToSave = True
                                            Exit Do
                                        Case Else
                                            If blnNewCompNumber Then
                                                'Something went wrong and the selected new company number is no longer available
                                                'add one to the number and try again
                                                If intRetryCt > Retry Then
                                                    blnValidationError = True
                                                    strErrorMessage = "Could not use new company number value already in use.  The company number " & oFields("CompNumber").Value & " has not been downloaded."
                                                    Exit Do
                                                Else
                                                    oFields("CompNumber").Value += 1
                                                End If
                                            Else
                                                blnInsertRecord = False
                                                blnReadyToSave = True
                                                Exit Do
                                            End If
                                    End Select
                                Loop Until blnReadyToSave
                            End If
                            If blnValidationError Then
                                addToErrorTable(oFields, "[dbo].[FileImportErrorLog]", strErrorMessage, strHeaderFile, mstrHeaderName)
                                RecordErrors += 1
                            Else
                                'Get the Lat Long and miles if needed
                                calcLatLong(oPCM, oFields)
                                'Save the changes to the main table
                                If saveData(oFields, blnInsertRecord, "Comp", "CompModUser", "CompModDate") Then
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
                        LogError("PODownload Failure", "PODownload.clsCompany.importHeaderRecords: Could not import from file " & strHeaderFile & vbCrLf & ex.ToString & vbCrLf & strSQL, AdminEmail)
                    Else
                        Log("importHeaderRecords Failure Retry = " & intRetryCt.ToString)

                    End If
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.                
        Catch ex As Exception
            LogError("PODownload Failure", "PODownload.clsCompany.importHeaderRecords: Could not import from file " & strHeaderFile & vbCrLf & ex.ToString, AdminEmail)
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
            'now get the Comp Contact Records
            Dim objImportRS As New ADODB.Recordset
            Dim strSQL As String = "Select * From " & strFile
            Dim intRetryCt As Integer = 0
            Dim strSource As String = "clsCompany.importItemRecords"
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
                        Log("Importing " & lngMax & " Company Contact Records.")
                        Do Until objImportRS.EOF
                            'Reset the data types and values to defaults for the following fields 
                            'at the top of each loop to handle alpha vs numeric data changes
                            'Reset the data types and values to defaults for the following fields 
                            'at the top of each loop to handle alpha vs numeric data changes
                            oItems("CompNumber").Name = "CompNumber"
                            oItems("CompNumber").DataType = clsImportField.DataTypeID.gcvdtString
                            oItems("CompNumber").Length = 50
                            oItems("CompNumber").Null = True

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
                            'Get the correct company number from the alpha Xref table
                            If lookupCompNumberByAlphaCode(oItems("CompNumber")) Then
                                'Now get any lookup data
                                For intct As Integer = 1 To oItems.Count
                                    If CInt(oItems(intct).PK) <> gcFK Then
                                        'we need to lookup the value in the parent table
                                        If Not lookupFKValue(oItems(intct), oItems(oItems(intct).FK_Key), "Comp", strSource) Then
                                            blnValidationError = True
                                            strErrorMessage &= "Invalid reference to Comp " _
                                                & " table for " & oItems(intct).Parent_Field & ". "
                                        End If
                                    End If
                                Next
                            Else
                                blnValidationError = True
                                strErrorMessage = "Could not validate Company Number in Alpha Xref Table  " & oItems("CompNumber").Value
                            End If
                            If Not blnValidationError Then
                                oItems("CompNumber").DataType = clsImportField.DataTypeID.gcvdtLongInt
                                oItems("CompNumber").Length = 11
                                'test if the record already exists.
                                Dim intRet As Integer = doesRecordExist(oItems, "CompCont")
                                Select Case intRet
                                    Case -1
                                        blnValidationError = True
                                        strErrorMessage = "Could not check for existing company contact record.  The company contact for company number " & oItems("CompNumber").Value & " has not been downloaded."
                                        Exit Do
                                    Case 0 'no records found so insert a new one
                                        blnInsertRecord = True
                                    Case Else
                                        blnInsertRecord = False
                                End Select
                            End If

                            If blnValidationError Then
                                addToErrorTable(oItems, "[dbo].[FileDetailsErrorLog]", strErrorMessage, strFile, mstrHeaderName)
                                ItemErrors += 1
                            Else
                                'Save the changes
                                If saveData(oItems, blnInsertRecord, "CompCont", "", "") Then
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
                        LogError("PODownload Failure", "PODownload.clsCompany.importItemRecords: Could not import from file " & strFile & vbCrLf & ex.ToString & vbCrLf & strSQL, AdminEmail)
                    Else
                        Log("importItemRecords Failure Retry = " & intRetryCt.ToString)

                    End If
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.                
        Catch ex As Exception
            LogError("PODownload Failure", "PODownload.clsCompany.importItemRecords: Could not import from file " & strFile & vbCrLf & ex.ToString, AdminEmail)
        End Try
        Return Ret

    End Function

    Private Function buildHeaderCollection(ByRef oFields As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Try
            With oFields
                .Add("CompNumber", "CompNumber", clsImportField.DataTypeID.gcvdtString, 50, False, clsImportField.PKValue.gcPK)
                .Add("CompName", "CompName", clsImportField.DataTypeID.gcvdtString, 40, False)
                .Add("CompNatNumber", "CompNatNumber", clsImportField.DataTypeID.gcvdtLongInt, 11, True)
                .Add("CompNatName", "CompNatName", clsImportField.DataTypeID.gcvdtString, 40, True)
                .Add("CompStreetAddress1", "CompStreetAddress1", clsImportField.DataTypeID.gcvdtString, 40, True)
                .Add("CompStreetAddress2", "CompStreetAddress2", clsImportField.DataTypeID.gcvdtString, 40, True)
                .Add("CompStreetAddress3", "CompStreetAddress3", clsImportField.DataTypeID.gcvdtString, 40, True)
                .Add("CompStreetCity", "CompStreetCity", clsImportField.DataTypeID.gcvdtString, 25, True)
                .Add("CompStreetState", "CompStreetState", clsImportField.DataTypeID.gcvdtString, 8, True)
                .Add("CompStreetCountry", "CompStreetCountry", clsImportField.DataTypeID.gcvdtString, 30, True)
                .Add("CompStreetZip", "CompStreetZip", clsImportField.DataTypeID.gcvdtString, 10, True)
                .Add("CompMailAddress1", "CompMailAddress1", clsImportField.DataTypeID.gcvdtString, 40, True)
                .Add("CompMailAddress2", "CompMailAddress2", clsImportField.DataTypeID.gcvdtString, 40, True)
                .Add("CompMailAddress3", "CompMailAddress3", clsImportField.DataTypeID.gcvdtString, 40, True)
                .Add("CompMailCity", "CompMailCity", clsImportField.DataTypeID.gcvdtString, 25, True)
                .Add("CompMailState", "CompMailState", clsImportField.DataTypeID.gcvdtString, 8, True)
                .Add("CompMailCountry", "CompMailCountry", clsImportField.DataTypeID.gcvdtString, 30, True)
                .Add("CompMailZip", "CompMailZip", clsImportField.DataTypeID.gcvdtString, 10, True)
                .Add("CompWeb", "CompWeb", clsImportField.DataTypeID.gcvdtString, 255, True)
                .Add("CompEmail", "CompEmail", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("CompDirections", "CompDirections", clsImportField.DataTypeID.gcvdtString, 500, True)
                .Add("CompAbrev", "CompAbrev", clsImportField.DataTypeID.gcvdtString, 3, True)
                .Add("CompActive", "CompActive", clsImportField.DataTypeID.gcvdtBit, 2, True)
                .Add("CompNEXTrack", "CompNEXTrack", clsImportField.DataTypeID.gcvdtBit, 2, True)
                .Add("CompNEXTStopAcctNo", "CompNEXTStopAcctNo", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("CompNEXTStopPsw", "CompNEXTStopPsw", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("CompNextstopSubmitRFP", "CompNextstopSubmitRFP", clsImportField.DataTypeID.gcvdtBit, 2, True)
                .Add("CompFAAShipID", "CompFAAShipID", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("CompFAAShipDate", "CompFAAShipDate", clsImportField.DataTypeID.gcvdtDate, 19, True)
                .Add("CompFinDuns", "CompFinDuns", clsImportField.DataTypeID.gcvdtString, 11, True)
                .Add("CompFinTaxID", "CompFinTaxID", clsImportField.DataTypeID.gcvdtString, 20, True)
                .Add("CompFinPaymentForm", "CompFinPaymentForm", clsImportField.DataTypeID.gcvdtSmallInt, 11, True)
                .Add("CompFinSIC", "CompFinSIC", clsImportField.DataTypeID.gcvdtString, 8, True)
                .Add("CompFinPaymentDiscount", "CompFinPaymentDiscount", clsImportField.DataTypeID.gcvdtSmallInt, 6, True)
                .Add("CompFinPaymentDays", "CompFinPaymentDays", clsImportField.DataTypeID.gcvdtSmallInt, 6, True)
                .Add("CompFinPaymentNetDays", "CompFinPaymentNetDays", clsImportField.DataTypeID.gcvdtSmallInt, 6, True)
                .Add("CompFinCommTerms", "CompFinCommTerms", clsImportField.DataTypeID.gcvdtString, 15, True)
                .Add("CompFinCommTermsPer", "CompFinCommTermsPer", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("CompFinCreditLimit", "CompFinCreditLimit", clsImportField.DataTypeID.gcvdtLongInt, 11, True)
                .Add("CompFinCreditUsed", "CompFinCreditUsed", clsImportField.DataTypeID.gcvdtLongInt, 6, True)
                .Add("CompFinInvPrnCode", "CompFinInvPrnCode", clsImportField.DataTypeID.gcvdtBit, 2, True)
                .Add("CompFinInvEMailCode", "CompFinInvEMailCode", clsImportField.DataTypeID.gcvdtBit, 2, True)
                .Add("CompFinCurType", "CompFinCurType", clsImportField.DataTypeID.gcvdtLongInt, 11, True)
                .Add("CompFinFBToleranceHigh", "CompFinFBToleranceHigh", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("CompFinFBToleranceLow", "CompFinFBToleranceLow", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("CompFinCustomerSince", "CompFinCustomerSince", clsImportField.DataTypeID.gcvdtDate, 19, True)
                .Add("CompFinCardType", "CompFinCardType", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("CompFinCardName", "CompFinCardName", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("CompFinCardExpires", "CompFinCardExpires", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("CompFinCardAuthorizor", "CompFinCardAuthorizor", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("CompFinCardAuthPassword", "CompFinCardAuthPassword", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("CompFinUseImportFrtCost", "CompFinUseImportFrtCost", clsImportField.DataTypeID.gcvdtBit, 2, True)
                .Add("CompFinBkhlFlatFee", "CompFinBkhlFlatFee", clsImportField.DataTypeID.gcvdtMoney, 21, True)
                .Add("CompFinBkhlCostPerc", "CompFinBkhlCostPerc", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("CompLatitude", "CompLatitude", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("CompLongitude", "CompLongitude", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("CompMailTo", "CompMailTo", clsImportField.DataTypeID.gcvdtString, 500, True)
            End With
            Log("Header Field Array Loaded.")
            'get the import field flag values
            For ct As Integer = 1 To oFields.Count
                Dim blnUseField As Boolean = True
                Try
                    blnUseField = CBool(getImportFieldFlag(oFields(ct).Name, ImportFileTypes.ietComp))
                Catch ex As Exception
                    'throw away any errors in case one or more field names is missing in the Import Field Flag Table
                End Try
                oFields(ct).Use = blnUseField
            Next
            Ret = True
        Catch ex As Exception
            LogError("PODownload Failure", "PODownload.clsCompany.buildHeaderCollection failure" & vbCrLf & ex.ToString, AdminEmail)
        End Try
        Return Ret

    End Function

    Private Function buildItemCollection(ByRef oItems As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Try
            With oItems
                .Add("CompContCompControl", "CompContCompControl", clsImportField.DataTypeID.gcvdtLongInt, 11, False, clsImportField.PKValue.gcFK, 2, "CompNumber", "CompControl")
                .Add("CompNumber", "CompNumber", clsImportField.DataTypeID.gcvdtString, 50, False, clsImportField.PKValue.gcHK)
                .Add("CompContName", "CompContName", clsImportField.DataTypeID.gcvdtString, 25, True, clsImportField.PKValue.gcPK)
                .Add("CompContTitle", "CompContTitle", clsImportField.DataTypeID.gcvdtString, 25, True)
                .Add("CompCont800", "CompCont800", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("CompContPhone", "CompContPhone", clsImportField.DataTypeID.gcvdtString, 15, True)
                .Add("CompContFax", "CompContFax", clsImportField.DataTypeID.gcvdtString, 15, True)
                .Add("CompContEMail", "CompContEMail", clsImportField.DataTypeID.gcvdtString, 50, True)
            End With
            'get the item  field flag values
            For ct As Integer = 1 To oItems.Count
                Dim blnUseField As Boolean = True
                Try
                    blnUseField = CBool(getImportFieldFlag(oItems(ct).Name, ImportFileTypes.ietComp))
                Catch ex As Exception
                    'throw away any errors in case one or more field names is missing in the Import Field Flag Table
                End Try
                oItems(ct).Use = blnUseField
            Next
            Ret = True
        Catch ex As Exception
            LogError("PODownload Failure", "PODownload.clsCompany.buildItemCollection failure" & vbCrLf & ex.ToString, AdminEmail)
        End Try
        Return Ret

    End Function

End Class
