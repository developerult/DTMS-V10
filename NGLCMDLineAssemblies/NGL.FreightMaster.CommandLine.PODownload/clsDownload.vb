Imports ngl.FreightMaster.FMLib.General
Imports ngl.FreightMaster.FMLib.dbUtilities
Imports ngl.FreightMaster.FMLib.PCMiles
Public Class clsDownload : Inherits clsImportExport
    Protected mstrItemName As String = ""

    Private mintTotalItems As Integer = 0
    Public Property TotalItems() As Integer
        Get
            TotalItems = mintTotalItems
        End Get
        Protected Set(ByVal Value As Integer)
            mintTotalItems = Value
        End Set
    End Property

    Private mintItemErrors As Integer = 0
    Public Property ItemErrors() As Integer
        Get
            ItemErrors = mintItemErrors
        End Get
        Protected Set(ByVal Value As Integer)
            mintItemErrors = Value
        End Set
    End Property

    Public Property ItemName() As String
        Get
            Return mstrItemName
        End Get
        Set(ByVal Value As String)
            mstrItemName = Value
        End Set
    End Property


    Protected Function calcMiles( _
        ByRef oPCM As FMLib.PCMiles, _
        ByRef oOrig As FMLib.clsAddress, _
        ByRef oDest As FMLib.clsAddress, _
        ByVal blnOriginUse As Boolean, _
        ByVal blnUseKilometers As Boolean) As Single

        Try
            Dim sglmiles As Single = 0

            If blnOriginUse Then
                sglmiles = oPCM.getPracticalMiles(oDest, oOrig, FMLib.PCMiles.PCMEX_Route_Type.CALCEX_TYPE_PRACTICAL, PCMEX_Opt_Flags.OPT_NATIONAL, PCMEX_Veh_Type.CALCEX_VEH_TRUCK, blnUseKilometers)
            Else
                sglmiles = oPCM.getPracticalMiles(oOrig, oDest, FMLib.PCMiles.PCMEX_Route_Type.CALCEX_TYPE_PRACTICAL, PCMEX_Opt_Flags.OPT_NATIONAL, PCMEX_Veh_Type.CALCEX_VEH_TRUCK, blnUseKilometers)
            End If
            Return sglmiles

        Catch ex As Exception
            LogError(Source & " error import not affected", "clsDownload.calcMiles: Calculate miles error but import not affected" & vbCrLf & ex.ToString, Me.AdminEmail)
        End Try


    End Function

    Protected Function lookupCompControlByAlphaCode(ByRef oField As clsImportField, _
        ByVal strNewFieldName As String) As Boolean
        Dim blnRet As Boolean = False
        Try
            If oField.Value <> "''" And oField.Value.ToUpper <> "NULL" Then
                'we have a valid company number so check th alpha x-ref table
                Dim strSQL As String = "Select dbo.udfGetCompControlByAlpha(" & oField.Value & ") as CompControl"
                Dim intRetryCt As Integer = 0
                Do
                    intRetryCt += 1
                    Try
                        'check the active db connection
                        If FMLib.dbUtilities.OpenConnection() Then
                            Dim objTestRS As New ADODB.Recordset
                            With objTestRS
                                .Open(strSQL, gDBCon, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)
                                Try
                                    If .RecordCount > 0 Then
                                        If nz(.Fields("CompControl"), 0) > 0 Then
                                            oField.DataType = clsImportField.DataTypeID.gcvdtLongInt
                                            oField.Length = 11
                                            oField.Name = strNewFieldName
                                            oField.Value = nz(.Fields("CompControl"), 0)
                                            blnRet = True
                                        End If
                                    End If
                                Catch ex As Exception
                                    Throw
                                Finally
                                    .Close()
                                End Try
                            End With
                            Exit Do
                        Else
                            If intRetryCt > Me.Retry Then
                                LogError(Source & " Failure", "clsDownload.lookupCompControlByAlphaCode: Open database connection failure; Company number " & oField.Value & " could not be processed correctly.", AdminEmail)
                            Else
                                Log("lookupCompControlByAlphaCode Open DB Connection Failure Retry = " & intRetryCt.ToString)
                            End If
                        End If
                    Catch ex As Exception
                        If intRetryCt > Me.Retry Then
                            LogError(Source & " Failure", "clsDownload.lookupCompControlByAlphaCode failure for Company number " & oField.Value & vbCrLf & ex.ToString & vbCrLf & strSQL, AdminEmail)
                        Else
                            Log("lookupCompControlByAlphaCode Failure Retry = " & intRetryCt.ToString)
                        End If
                    End Try
                    'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
                Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
            Else
                oField.Name = strNewFieldName
                oField.Value = 0
                oField.DataType = clsImportField.DataTypeID.gcvdtLongInt
                oField.Length = 11
                blnRet = True
            End If
        Catch ex As Exception
            LogError(Source & " Failure", "clsDownload.lookupCompControlByAlphaCode failure for Company number " & oField.Value & vbCrLf & ex.ToString, AdminEmail)
        End Try
        Return blnRet
    End Function

    Protected Function lookupCompNumberByAlphaCode(ByRef oField As clsImportField) As Boolean
        Dim blnRet As Boolean = False
        Try
            If oField.Value <> "''" And oField.Value.ToUpper <> "NULL" Then
                'we have a valid company number so check the alpha x-ref table
                Dim strSQL As String = "Select dbo.udfGetCompNumberByAlpha(" & oField.Value & ") as CompNumber"
                Dim intRetryCt As Integer = 0
                Do
                    intRetryCt += 1
                    Try
                        'check the active db connection
                        If FMLib.dbUtilities.OpenConnection() Then
                            Dim objTestRS As New ADODB.Recordset
                            With objTestRS
                                .Open(strSQL, gDBCon, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)
                                Try
                                    If .RecordCount > 0 Then
                                        oField.Value = nz(.Fields("CompNumber"), "0")
                                        blnRet = True
                                    End If
                                Catch ex As Exception
                                    Throw
                                Finally
                                    .Close()
                                End Try
                            End With
                            Exit Do
                        Else
                            If intRetryCt > Me.Retry Then
                                LogError(Source & " Failure", "clsDownload.lookupCompNumberByAlphaCode Open database connection failure; Company number " & oField.Value & " could not be processed correctly.", AdminEmail)
                            Else
                                Log("lookupCompNumberByAlphaCode Open DB Connection Failure Retry = " & intRetryCt.ToString)
                            End If
                        End If
                    Catch ex As Exception
                        If intRetryCt > Me.Retry Then
                            LogError(Source & " Failure", "clsDownload.lookupCompNumberByAlphaCode failure for Company number " & oField.Value & vbCrLf & ex.ToString & vbCrLf & strSQL, AdminEmail)
                        Else
                            Log("lookupCompNumberByAlphaCode Failure Retry = " & intRetryCt.ToString)
                        End If
                    End Try
                    'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
                Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
            Else
                oField.Value = "0"
                blnRet = True
            End If
        Catch ex As Exception
            LogError(Source & " Failure", "clsDownload.lookupCompNumberByAlphaCode failure for Company number " & oField.Value & vbCrLf & ex.ToString, AdminEmail)
        End Try
        Return blnRet
    End Function

    Protected Function lookupDefaultCarrier(ByRef oField As clsImportField, ByVal strNewFieldName As String) As Boolean
        Dim blnRet As Boolean = False
        Try
            If Val(oField.Value) > 0 Then
                'we have a valid carrier number so check the carrier table
                Dim strSQL As String = "Select CarrierControl From Carrier Where CarrierNumber = " & Val(oField.Value)
                Dim intRetryCt As Integer = 0
                Do
                    intRetryCt += 1
                    Try
                        'check the active db connection
                        If FMLib.dbUtilities.OpenConnection() Then
                            Dim objTestRS As New ADODB.Recordset
                            With objTestRS
                                .Open(strSQL, gDBCon, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)
                                Try
                                    If .RecordCount > 0 Then
                                        If nz(.Fields("CarrierControl"), 0) > 0 Then
                                            oField.Name = strNewFieldName
                                            oField.Value = nz(.Fields("CarrierControl"), 0)
                                        End If
                                    End If
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
                                LogError(Source & " Failure", "clsDownload.lookupDefaultCarrier: Open database connection failure; Carrier number " & oField.Value & " could not be processed correctly.", AdminEmail)
                            Else
                                Log("lookupDefaultCarrier Open DB Connection Failure Retry = " & intRetryCt.ToString)
                            End If
                        End If
                    Catch ex As Exception
                        If intRetryCt > Me.Retry Then
                            LogError(Source & " Failure", "clsDownload.lookupDefaultCarrier failure for Carrier number " & oField.Value & vbCrLf & ex.ToString & vbCrLf & strSQL, AdminEmail)
                        Else
                            Log("lookupDefaultCarrier Failure Retry = " & intRetryCt.ToString)

                        End If
                    End Try
                    'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
                Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
            Else
                oField.Name = strNewFieldName
                oField.Value = "0"
                blnRet = True
            End If
        Catch ex As Exception
            LogError(Source & " Failure", "clsDownload.lookupDefaultCarrier failure for Carrier number " & oField.Value & vbCrLf & ex.ToString, AdminEmail)
        End Try
        Return blnRet
    End Function

    Protected Function lookupCompAddress(ByRef oCompField As clsImportField, _
                                        ByRef oNameField As clsImportField, _
                                        ByRef oAdd1Field As clsImportField, _
                                        ByRef oAdd2Field As clsImportField, _
                                        ByRef oAdd3Field As clsImportField, _
                                        ByRef oCityField As clsImportField, _
                                        ByRef oStateField As clsImportField, _
                                        ByRef oCountryField As clsImportField, _
                                        ByRef oZipField As clsImportField, _
                                        ByRef oPhoneField As clsImportField, _
                                        ByRef oFaxField As clsImportField, _
                                        ByVal strNewFieldName As String) As Boolean
        Dim blnRet As Boolean = False
        Try
            If oCompField.Value <> "''" And oCompField.Value.ToUpper <> "NULL" Then
                'we have a valid company number so check th alpha x-ref table
                If Not lookupCompControlByAlphaCode(oCompField, strNewFieldName) Then
                    Return False
                End If
                If Val(oCompField.Value) < 1 Then Return False
                'Use the new CompControl Number to get the address information
                Dim strSQL As String = "Select CompControl, CompName,CompStreetAddress1,CompStreetAddress2,CompStreetAddress3,CompStreetCity,CompStreetState,CompStreetCountry,CompStreetZip From Comp Where CompControl = " & Val(oCompField.Value)
                Dim intRetryCt As Integer = 0
                Do
                    intRetryCt += 1
                    Try
                        'check the active db connection
                        If FMLib.dbUtilities.OpenConnection() Then
                            Dim objTestRS As New ADODB.Recordset
                            With objTestRS
                                .Open(strSQL, gDBCon, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)
                                Try
                                    If .RecordCount > 0 Then
                                        oNameField.Value = "'" & padQuotes(nz(.Fields("CompName"), "")) & "'"
                                        oAdd1Field.Value = "'" & padQuotes(nz(.Fields("CompStreetAddress1"), "")) & "'"
                                        oAdd2Field.Value = "'" & padQuotes(nz(.Fields("CompStreetAddress2"), "")) & "'"
                                        oAdd3Field.Value = "'" & padQuotes(nz(.Fields("CompStreetAddress3"), "")) & "'"
                                        oCityField.Value = "'" & padQuotes(nz(.Fields("CompStreetCity"), "")) & "'"
                                        oStateField.Value = "'" & padQuotes(nz(.Fields("CompStreetState"), "")) & "'"
                                        oCountryField.Value = "'" & padQuotes(nz(.Fields("CompStreetCountry"), "")) & "'"
                                        oZipField.Value = "'" & padQuotes(nz(.Fields("CompStreetZip"), "")) & "'"
                                    End If
                                Catch ex As Exception
                                    Throw
                                Finally
                                    .Close()
                                End Try
                            End With
                            'now get the contact information
                            strSQL = "Select top 1 CompContPhone,CompContFax from Compcont Where compcontCompcontrol = " & Val(oCompField.Value)
                            With objTestRS
                                .Open(strSQL, gDBCon, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)
                                Try
                                    If .RecordCount > 0 Then
                                        oPhoneField.Value = "'" & padQuotes(nz(.Fields("CompContPhone"), "")) & "'"
                                        oFaxField.Value = "'" & padQuotes(nz(.Fields("CompContFax"), "")) & "'"
                                    End If
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
                                LogError(Source & " Failure", "clsDownload.lookupCompAddress: Open database connection failure; Company number " & oCompField.Value & " could not be processed correctly.", AdminEmail)
                            Else
                                Log("lookupCompAddress Open DB Connection Failure Retry = " & intRetryCt.ToString)
                            End If
                        End If
                    Catch ex As Exception
                        If intRetryCt > Me.Retry Then
                            LogError(Source & " Failure", "clsDownload.lookupCompAddress failure for Company number " & oCompField.Value & vbCrLf & ex.ToString & vbCrLf & strSQL, AdminEmail)
                        Else
                            Log("lookupCompAddress Failure Retry = " & intRetryCt.ToString)
                        End If
                    End Try
                    'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
                Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
            Else
                oCompField.Name = strNewFieldName
                oCompField.Value = "0"
                blnRet = True
            End If
        Catch ex As Exception
            LogError(Source & " Failure", "clsDownload.lookupCompAddress failure for Company number " & oCompField.Value & vbCrLf & ex.ToString, AdminEmail)
        End Try
        Return blnRet
    End Function

    Protected Function getLaneControl(ByRef oKeyField As clsImportField) As Integer
        Dim Ret As Integer = 0

        Try
            Dim strSQL As String = "Select LaneControl From Lane Where LaneNumber = " & oKeyField.Value
            Dim intRetryCt As Integer = 0
            Do
                intRetryCt += 1
                Try
                    'check the active db connection
                    If FMLib.dbUtilities.OpenConnection() Then
                        Dim objTestRS As New ADODB.Recordset
                        With objTestRS
                            .Open(strSQL, gDBCon, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)
                            Try
                                If .RecordCount > 0 Then
                                    Ret = nz(.Fields("LaneControl"), 0)
                                End If
                            Catch ex As Exception
                                Throw
                            Finally
                                .Close()
                            End Try
                        End With
                        Exit Do
                    Else
                        If intRetryCt > Me.Retry Then
                            LogError(Source & " Failure", "clsDownload.getLaneControl: Open database connection failure; Lane number " & oKeyField.Value & " could not be processed correctly.", AdminEmail)
                        Else
                            Log("getLaneControl Open DB Connection Failure Retry = " & intRetryCt.ToString)
                        End If
                    End If
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        LogError(Source & " Failure", "clsDownload.getLaneControl failure for Lane number " & oKeyField.Value & vbCrLf & ex.ToString & vbCrLf & strSQL, AdminEmail)
                    Else
                        Log("getLaneControl Failure Retry = " & intRetryCt.ToString)

                    End If
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
        Catch ex As Exception
            LogError(Source & " Failure", "clsDownload.getLaneControl failure for Lane number " & oKeyField.Value & vbCrLf & ex.ToString, AdminEmail)
        End Try
        Return Ret
    End Function

    Protected Function doesRecordExist(ByRef oFields As clsImportFields, _
                                        ByVal strTableName As String) As Integer
        Dim intRet As Integer = -1

        Try
            Dim strSQL As String = "Select "
            Dim strValues As String = " Where "
            Dim blnKeyFound As Boolean = False
            For intct As Integer = 1 To oFields.Count
                If oFields(intct).Use Then
                    If Val(oFields(intct).PK) = gcPK Then
                        If blnKeyFound Then
                            strSQL &= " , "
                            strValues &= " AND "
                        Else
                            blnKeyFound = True
                        End If
                        strSQL &= oFields(intct).Name
                        strValues &= oFields(intct).Name & " = " & oFields(intct).Value
                    End If
                End If
            Next
            strSQL &= " From " & strTableName & strValues
            Dim intRetryCt As Integer = 0
            Do
                intRetryCt += 1
                Try
                    'check the active db connection
                    If FMLib.dbUtilities.OpenConnection() Then
                        Dim objTestRS As New ADODB.Recordset
                        With objTestRS
                            .Open(strSQL, gDBCon, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)
                            Try
                                If .RecordCount > 0 Then
                                    intRet = .RecordCount
                                Else
                                    intRet = 0
                                End If
                            Catch ex As Exception
                                Throw
                            Finally
                                .Close()
                            End Try
                        End With
                        Exit Do
                    Else
                        If intRetryCt > Me.Retry Then
                            LogError(Source & " Failure", "clsDownload.doesRecordExist: Open database connection failure; could not read from  " & strTableName & " table.", AdminEmail)
                        Else
                            Log("doesRecordExist Open DB Connection Failure Retry = " & intRetryCt.ToString)
                        End If
                    End If
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        LogError(Source & " Failure", "clsDownload.doesRecordExist Could not read from  " & strTableName & " table" & vbCrLf & ex.ToString & vbCrLf & strSQL, AdminEmail)
                    Else
                        Log("doesRecordExist Failure Retry = " & intRetryCt.ToString)

                    End If
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
        Catch ex As Exception
            LogError(Source & " Failure", "clsDownload.doesRecordExist: Could not read from  " & strTableName & " table" & vbCrLf & ex.ToString, AdminEmail)
        End Try
        Return intRet
    End Function

    Protected Function saveData(ByRef oFields As clsImportFields, _
                                        ByVal blnInsertRecord As Boolean, _
                                        ByVal strTableName As String, _
                                        ByVal strModUserField As String, _
                                        ByVal strModDateField As String) As Boolean
        Dim Ret As Boolean = False
        Dim strSQL As String = ""
        Dim strValues As String = ""
        Dim blnFirstField As Boolean = True
        Try
            If blnInsertRecord Then
                'build execute string to insert record into Header table
                strSQL = "Insert Into " & strTableName & " ("
                strValues = " Values ( "
                blnFirstField = True
                For intct As Integer = 1 To oFields.Count
                    If oFields(intct).Use And CInt(oFields(intct).PK) <> gcHK Then
                        'we do not add hidden fields they are used for look up only
                        If blnFirstField Then
                            blnFirstField = False
                        Else
                            strSQL &= " , "
                            strValues &= " , "
                        End If
                        strSQL &= oFields(intct).Name
                        strValues &= oFields(intct).Value
                    End If
                Next
                If strModUserField.Trim.Length > 0 Then
                    strSQL &= " , " & strModUserField
                    strValues &= ",'" & mstrCreateUser & "'"
                End If
                If strModDateField.Trim.Length > 0 Then
                    strSQL &= " , " & strModDateField
                    strValues &= ",'" & mstrCreatedDate & "'"
                End If
                strSQL = strSQL & " ) " & strValues & " ) "
                'debug.print " Insert Header SQL = " & strSQL
            Else
                'build sql string to update current record
                strSQL = "Update " & strTableName & " Set "
                strValues = " Where "
                blnFirstField = True
                Dim blnKeyFound As Boolean = False
                For intct As Integer = 1 To oFields.Count
                    If oFields(intct).Use And CInt(oFields(intct).PK) <> gcHK Then
                        If blnFirstField Then
                            blnFirstField = False
                        Else
                            strSQL &= " , "
                        End If
                        If CInt(oFields(intct).PK) = gcPK Or CInt(oFields(intct).PK) = gcFK Then
                            If blnKeyFound Then
                                strValues &= " AND "
                            Else
                                blnKeyFound = True
                            End If
                            strValues &= oFields(intct).Name & " = " & oFields(intct).Value
                        End If
                        strSQL &= oFields(intct).Name & " = " & oFields(intct).Value
                    End If
                Next
                If strModUserField.Trim.Length > 0 Then
                    strSQL &= " , " & strModUserField & " = '" & mstrCreateUser & "'"
                End If
                If strModDateField.Trim.Length > 0 Then
                    strSQL &= " , " & strModDateField & " = '" & mstrCreatedDate & "'"
                End If
                strSQL &= " From " & strTableName & strValues
                'Debug.Print " Update Header SQL = " & strSQL
            End If
            Dim intRetryCt As Integer = 0
            Do
                intRetryCt += 1
                Try
                    'check the active db connection
                    If FMLib.dbUtilities.OpenConnection() Then
                        gDBCon.Execute(strSQL)
                        Ret = True
                        Exit Do
                    Else
                        If intRetryCt > Me.Retry Then
                            LogError(Source & " Failure", "clsDownload.saveData: Open database connection failure; could not write to  " & strTableName & " table.", AdminEmail)
                        Else
                            Log("saveData Open DB Connection Failure Retry = " & intRetryCt.ToString)
                        End If
                    End If
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        LogError(Source & " Failure", "clsDownload.saveData: Could not write to  " & strTableName & " table" & vbCrLf & ex.ToString & vbCrLf & strSQL, AdminEmail)
                    Else
                        Log("saveData Failure Retry = " & intRetryCt.ToString)

                    End If
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
        Catch ex As Exception
            LogError(Source & " Failure", "clsDownload.saveData: Could not write to  " & strTableName & " table" & vbCrLf & ex.ToString, AdminEmail)
        End Try
        Return Ret
    End Function

    Protected Function lookupFKValue( _
                    ByRef oField As clsImportField, _
                    ByRef oKeyField As clsImportField, _
                    ByVal strHeaderTable As String, _
                    ByVal strSource As String) As Boolean
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
                        strSQL = "Select Top 1 " & oField.Parent_Field & " as RetVal " _
                            & " From " & strHeaderTable _
                            & " Where " & oKeyField.Name & " = " & oKeyField.Value
                        Dim objTestRS As New ADODB.Recordset
                        With objTestRS
                            .Open(strSQL, gDBCon, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)
                            Try
                                If .RecordCount > 0 Then
                                    oField.Value = validateSQLValue( _
                                        .Fields("RetVal"), _
                                        Val(oField.DataType), _
                                        strSource, _
                                        "Invalid " & oField.Name, _
                                        oField.Null, _
                                        oField.Length)
                                    Ret = True
                                End If
                            Catch ex As Exception
                                Throw
                            Finally
                                .Close()
                            End Try
                        End With
                        Exit Do
                    Else
                        If intRetryCt > Me.Retry Then
                            LogError(Source & " Failure", "clsDownload.lookupFKValue: Open database connection failure; could not read to from " & strHeaderTable & " table.", AdminEmail)
                        Else
                            Log("lookupFKValue Open DB Connection Failure Retry = " & intRetryCt.ToString)
                        End If
                    End If
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        LogError(Source & " Failure", "clsDownload.lookupFKValue: Could not read to from " & strHeaderTable & " table" & vbCrLf & ex.ToString & vbCrLf & strSQL, AdminEmail)
                    Else
                        Log("lookupFKValue Failure Retry = " & intRetryCt.ToString)

                    End If
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
        Catch ex As Exception
            LogError(Source & " Failure", "clsDownload.lookupFKValue: Could not read to from " & strHeaderTable & " table" & vbCrLf & ex.ToString, AdminEmail)
        End Try
        Return Ret
    End Function

    Protected Function addToErrorTable( _
            ByRef oFields As clsImportFields, _
            ByVal strTableName As String, _
            ByVal strErrorMessage As String, _
            ByVal strFile As String, _
            ByVal strName As String) As Boolean
        Dim Ret As Boolean = False
        Dim strSQL As String = ""
        Try
            Dim intRetryCt As Integer = 0
            Do
                intRetryCt += 1
                Try
                    'check the active db connection
                    If FMLib.dbUtilities.OpenConnection() Then
                        strErrorMessage = "'" & padQuotes(strErrorMessage) & "'"
                        'build data record string
                        Dim strRecord As String = ""
                        Dim blnFirstField As Boolean = True
                        For intct As Integer = 1 To oFields.Count
                            If oFields(intct).Use Then
                                If blnFirstField Then
                                    blnFirstField = False
                                Else
                                    strRecord &= ","
                                End If
                                strRecord &= oFields(intct).Name & " = " & oFields(intct).Value
                            End If
                        Next

                        strRecord = "'" & padQuotes(strRecord) & "'"
                        'build execute string into error
                        strSQL = "INSERT INTO " & strTableName _
                            & " ([ImportRecord], [CreateUser], [ErrorDate], [ErrorMsg], [ImportFileName], [ImportFileType], [ImportName]) VALUES(" _
                            & strRecord & ", '" _
                            & mstrCreateUser & "', '" _
                            & mstrCreatedDate & "', " _
                            & padSpaces(strErrorMessage) & ", '" _
                            & padSpaces(strFile) & "', " _
                            & mintImportTypeKey & ", '" _
                            & padSpaces(strName) & "')"
                        gDBCon.Execute(strSQL)
                        Ret = True
                        Exit Do
                    Else
                        If intRetryCt > Me.Retry Then
                            LogError(Source & " Failure", "clsDownload.addToErrorTable: Open database connection failure; could not write to " & strTableName & " table.", AdminEmail)
                        Else
                            Log("addToErrorTable Open DB Connection Failure Retry = " & intRetryCt.ToString)
                        End If
                    End If
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        LogError(Source & " Failure", "clsDownload.addToErrorTable: Could not write to " & strTableName & " table" & vbCrLf & ex.ToString & vbCrLf & strSQL, AdminEmail)
                    Else
                        Log("addToErrorTable Failure Retry = " & intRetryCt.ToString)

                    End If
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
        Catch ex As Exception
            LogError(Source & " Failure", "clsDownload.addToErrorTable: Could not write to " & strTableName & " table" & vbCrLf & ex.ToString, AdminEmail)
        End Try
        Return Ret
    End Function

End Class
