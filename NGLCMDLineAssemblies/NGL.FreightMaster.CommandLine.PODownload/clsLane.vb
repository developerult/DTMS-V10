Imports ngl.FreightMaster.FMLib.General
Imports ngl.FreightMaster.FMLib.dbUtilities
Imports ngl.FreightMaster.FMLib.PCMiles
Imports NGL.FreightMaster.Integration

Public Class clsLane : Inherits clsDownload

    Private Sub AddFieldToObject(ByRef oLane As clsLaneObject, ByRef oField As clsImportField)
        With oLane
            Select Case oField.Name
                Case "LaneNumber"
                    .LaneNumber = oField.Value
                Case "LaneName"
                    .LaneName = oField.Value
                Case "LaneNumberMaster"
                    .LaneNumberMaster = oField.Value
                Case "LaneNameMaster"
                    .LaneNameMaster = oField.Value
                Case "LaneCompNumber"
                    .LaneCompNumber = oField.Value
                Case "LaneDefaultCarrierUse"
                    .LaneDefaultCarrierUse = oField.Value
                Case "LaneDefaultCarrierNumber"
                    .LaneDefaultCarrierNumber = oField.Value
                Case "LaneOrigCompNumber"
                    .LaneOrigCompNumber = oField.Value
                Case "LaneOrigName"
                    .LaneOrigName = oField.Value
                Case "LaneOrigAddress1"
                    .LaneOrigAddress1 = oField.Value
                Case "LaneOrigAddress2"
                    .LaneOrigAddress2 = oField.Value
                Case "LaneOrigAddress3"
                    .LaneOrigAddress3 = oField.Value
                Case "LaneOrigCity"
                    .LaneOrigCity = oField.Value
                Case "LaneOrigState"
                    .LaneOrigState = oField.Value
                Case "LaneOrigCountry"
                    .LaneOrigCountry = oField.Value
                Case "LaneOrigZip"
                    .LaneOrigZip = oField.Value
                Case "LaneOrigContactPhone"
                    .LaneOrigContactPhone = oField.Value
                Case "LaneOrigContactPhoneExt"
                    .LaneOrigContactPhoneExt = oField.Value
                Case "LaneOrigContactFax"
                    .LaneOrigContactFax = oField.Value
                Case "LaneDestCompNumber"
                    .LaneDestCompNumber = oField.Value
                Case "LaneDestName"
                    .LaneDestName = oField.Value
                Case "LaneDestAddress1"
                    .LaneDestAddress1 = oField.Value
                Case "LaneDestAddress2"
                    .LaneDestAddress2 = oField.Value
                Case "LaneDestAddress3"
                    .LaneDestAddress3 = oField.Value
                Case "LaneDestCity"
                    .LaneDestCity = oField.Value
                Case "LaneDestState"
                    .LaneDestState = oField.Value
                Case "LaneDestCountry"
                    .LaneDestCountry = oField.Value
                Case "LaneDestZip"
                    .LaneDestZip = oField.Value
                Case "LaneDestContactPhone"
                    .LaneDestContactPhone = oField.Value
                Case "LaneDestContactPhoneExt"
                    .LaneDestContactPhoneExt = oField.Value
                Case "LaneDestContactFax"
                    .LaneDestContactFax = oField.Value
                Case "LaneConsigneeNumber"
                    .LaneConsigneeNumber = oField.Value
                Case "LaneRecMinIn"
                    .LaneRecMinIn = oField.Value
                Case "LaneRecMinUnload"
                    .LaneRecMinUnload = oField.Value
                Case "LaneRecMinOut"
                    .LaneRecMinOut = oField.Value
                Case "LaneAppt"
                    .LaneAppt = oField.Value
                Case "LanePalletExchange"
                    .LanePalletExchange = oField.Value
                Case "LanePalletType"
                    .LanePalletType = oField.Value
                Case "LaneBenchMiles"
                    .LaneBenchMiles = oField.Value
                Case "LaneBFC"
                    .LaneBFC = oField.Value
                Case "LaneBFCType"
                    .LaneBFCType = oField.Value
                Case "LaneRecHourStart"
                    .LaneRecHourStart = oField.Value
                Case "LaneRecHourStop"
                    .LaneRecHourStop = oField.Value
                Case "LaneDestHourStart"
                    .LaneDestHourStart = oField.Value
                Case "LaneDestHourStop"
                    .LaneDestHourStop = oField.Value
                Case "LaneComments"
                    .LaneComments = oField.Value
                Case "LaneCommentsConfidential"
                    .LaneCommentsConfidential = oField.Value
                Case "LaneLatitude"
                    .LaneLatitude = oField.Value
                Case "LaneLongitude"
                    .LaneLongitude = oField.Value
                Case "LaneTempType"
                    .LaneTempType = oField.Value
                Case "LaneTransType"
                    .LaneTransType = oField.Value
                Case "LanePrimaryBuyer"
                    .LanePrimaryBuyer = oField.Value
                Case "LaneAptDelivery"
                    .LaneAptDelivery = oField.Value
                Case "BrokerNumber"
                    .BrokerNumber = oField.Value
                Case "BrokerName"
                    .BrokerName = oField.Value
                Case "LaneOriginAddressUse"
                    .LaneOriginAddressUse = oField.Value
            End Select
        End With
    End Sub

    Public Sub FileImport(ByRef strHeaderFile As String, ByRef strItemFile As String, ByRef strDataPath As String)
        Dim intRet As Configuration.ProcessDataReturnValues
        Dim blnOriginUse As Boolean = False
        Dim strMsg As String = ""
        Dim strSQL As String = ""
        Dim objImportCon As ADODB.Connection
        Dim objImportRS As ADODB.Recordset
        Dim blnValidationError As Boolean
        Dim strErrorMessage As String = ""
        Dim lngErrCt As Integer = 0
        Dim lngImportCount As Integer = 0
        Dim lngMax As Integer = 0
        Dim blnImportingHeaders As Boolean = False
        Dim strTitle As String = ""
        Dim blnCreatingErrorLog As Boolean = False
        Dim strSource As String = "clsLane.FileImport"
        Dim blnInsertRecord As Boolean = False
        Dim strConnection As String = ""
        Dim strRecord As String = ""
        Dim intct As Short = 0
        Dim strHeaderTable As String = "Lane"
        Dim strItemTable As String = ""
        Dim blnUseConsigneeNumber As Boolean = False
        Dim intLaneControl As Integer = 0
        Dim shtServerID As Short = 0
        Dim blnFirstField As Boolean = True
        Dim intRowCt As Integer

        On Error Resume Next
        'Check if the header file exists
        If Not System.IO.File.Exists(objF.buildPath(strDataPath, strHeaderFile)) Then
            Log("The import header file " & objF.buildPath(strDataPath, strHeaderFile) & "does not exists.")
            Exit Sub
        End If
        On Error GoTo ErrorHandler

        'Header Information
        Dim intHeaderFieldCt As Integer = 55 'actual number of fields
        Dim oFields As New clsImportFields
        With oFields
            .Add("LaneNumber", "LaneNumber", clsImportField.DataTypeID.gcvdtString, 50, False) '0
            .Add("LaneName", "LaneName", clsImportField.DataTypeID.gcvdtString, 50, False) '1
            .Add("LaneNumberMaster", "LaneNumberMaster", clsImportField.DataTypeID.gcvdtString, 50, True) '2
            .Add("LaneNameMaster", "LaneNameMaster", clsImportField.DataTypeID.gcvdtString, 50, True) '3
            .Add("LaneCompNumber", "LaneCompNumber", clsImportField.DataTypeID.gcvdtString, 50, True) '4
            .Add("LaneDefaultCarrierUse", "LaneDefaultCarrierUse", clsImportField.DataTypeID.gcvdtBit, 2, True) '5
            .Add("LaneDefaultCarrierNumber", "LaneDefaultCarrierNumber", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '6
            .Add("LaneOrigCompNumber", "LaneOrigCompNumber", clsImportField.DataTypeID.gcvdtString, 50, True) '7
            .Add("LaneOrigName", "LaneOrigName", clsImportField.DataTypeID.gcvdtString, 40, True) '8
            .Add("LaneOrigAddress1", "LaneOrigAddress1", clsImportField.DataTypeID.gcvdtString, 40, True) '9
            .Add("LaneOrigAddress2", "LaneOrigAddress2", clsImportField.DataTypeID.gcvdtString, 40, True) '10
            .Add("LaneOrigAddress3", "LaneOrigAddress3", clsImportField.DataTypeID.gcvdtString, 40, True) '11
            .Add("LaneOrigCity", "LaneOrigCity", clsImportField.DataTypeID.gcvdtString, 25, True) '12
            .Add("LaneOrigState", "LaneOrigState", clsImportField.DataTypeID.gcvdtString, 8, True) '13
            .Add("LaneOrigCountry", "LaneOrigCountry", clsImportField.DataTypeID.gcvdtString, 30, True) '14
            .Add("LaneOrigZip", "LaneOrigZip", clsImportField.DataTypeID.gcvdtString, 50, True) '15
            .Add("LaneOrigContactPhone", "LaneOrigContactPhone", clsImportField.DataTypeID.gcvdtString, 15, True) '16
            .Add("LaneOrigContactPhoneExt", "LaneOrigContactPhoneExt", clsImportField.DataTypeID.gcvdtString, 50, True) '17
            .Add("LaneOrigContactFax", "LaneOrigContactFax", clsImportField.DataTypeID.gcvdtString, 15, True) '18
            .Add("LaneDestCompNumber", "LaneDestCompNumber", clsImportField.DataTypeID.gcvdtString, 50, True) '19
            .Add("LaneDestName", "LaneDestName", clsImportField.DataTypeID.gcvdtString, 40, True) '20
            .Add("LaneDestAddress1", "LaneDestAddress1", clsImportField.DataTypeID.gcvdtString, 40, True) '21
            .Add("LaneDestAddress2", "LaneDestAddress2", clsImportField.DataTypeID.gcvdtString, 40, True) '22
            .Add("LaneDestAddress3", "LaneDestAddress3", clsImportField.DataTypeID.gcvdtString, 40, True) '23
            .Add("LaneDestCity", "LaneDestCity", clsImportField.DataTypeID.gcvdtString, 25, True) '24
            .Add("LaneDestState", "LaneDestState", clsImportField.DataTypeID.gcvdtString, 2, True) '25
            .Add("LaneDestCountry", "LaneDestCountry", clsImportField.DataTypeID.gcvdtString, 30, True) '26
            .Add("LaneDestZip", "LaneDestZip", clsImportField.DataTypeID.gcvdtString, 10, True) '27
            .Add("LaneDestContactPhone", "LaneDestContactPhone", clsImportField.DataTypeID.gcvdtString, 15, True) '28
            .Add("LaneDestContactPhoneExt", "LaneDestContactPhoneExt", clsImportField.DataTypeID.gcvdtString, 50, True) '29
            .Add("LaneDestContactFax", "LaneDestContactFax", clsImportField.DataTypeID.gcvdtString, 15, True) '30
            .Add("LaneConsigneeNumber", "LaneConsigneeNumber", clsImportField.DataTypeID.gcvdtString, 50, True, clsImportField.PKValue.gcPK) '31
            .Add("LaneRecMinIn", "LaneRecMinIn", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '32
            .Add("LaneRecMinUnload", "LaneRecMinUnload", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '33
            .Add("LaneRecMinOut", "LaneRecMinOut", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '34
            .Add("LaneAppt", "LaneAppt", clsImportField.DataTypeID.gcvdtBit, 2, True) '35
            .Add("LanePalletExchange", "LanePalletExchange", clsImportField.DataTypeID.gcvdtBit, 2, True) '36
            .Add("LanePalletType", "LanePalletType", clsImportField.DataTypeID.gcvdtString, 1, True) '37
            .Add("LaneBenchMiles", "LaneBenchMiles", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '38
            .Add("LaneBFC", "LaneBFC", clsImportField.DataTypeID.gcvdtFloat, 11, True) '39
            .Add("LaneBFCType", "LaneBFCType", clsImportField.DataTypeID.gcvdtString, 10, True) '40
            .Add("LaneRecHourStart", "LaneRecHourStart", clsImportField.DataTypeID.gcvdtDate, 19, True) '41
            .Add("LaneRecHourStop", "LaneRecHourStop", clsImportField.DataTypeID.gcvdtDate, 19, True) '42
            .Add("LaneDestHourStart", "LaneDestHourStart", clsImportField.DataTypeID.gcvdtDate, 19, True) '43
            .Add("LaneDestHourStop", "LaneDestHourStop", clsImportField.DataTypeID.gcvdtDate, 19, True) '44
            .Add("LaneComments", "LaneComments", clsImportField.DataTypeID.gcvdtString, 255, True) '45
            .Add("LaneCommentsConfidential", "LaneCommentsConfidential", clsImportField.DataTypeID.gcvdtString, 255, True) '46
            .Add("LaneLatitude", "LaneLatitude", clsImportField.DataTypeID.gcvdtFloat, 11, True) '47
            .Add("LaneLongitude", "LaneLongitude", clsImportField.DataTypeID.gcvdtFloat, 11, True) '48
            .Add("LaneTempType", "LaneTempType", clsImportField.DataTypeID.gcvdtSmallInt, 6, True) '49
            .Add("LaneTransType", "LaneTransType", clsImportField.DataTypeID.gcvdtSmallInt, 6, True) '50
            .Add("LanePrimaryBuyer", "LanePrimaryBuyer", clsImportField.DataTypeID.gcvdtString, 50, True) '51
            .Add("LaneAptDelivery", "LaneAptDelivery", clsImportField.DataTypeID.gcvdtBit, 2, True) '52
            .Add("BrokerNumber", "BrokerNumber", clsImportField.DataTypeID.gcvdtString, 50, True) '53
            .Add("BrokerName", "BrokerName", clsImportField.DataTypeID.gcvdtString, 30, True) '54
            .Add("LaneOriginAddressUse", "LaneOriginAddressUse", clsImportField.DataTypeID.gcvdtBit, 2, True) '55
        End With
        Log("Header Field Array Loaded.")


        'build connection string for import file
        strConnection = "Driver={Microsoft Text Driver (*.txt; *.csv)}; Dbq=" & strDataPath & ";Extensions=asc,csv,tab,txt;HDR=YES;Persist Security Info=False"
        'create a new connection
        objImportCon = New ADODB.Connection
        With objImportCon
            .CursorLocation = ADODB.CursorLocationEnum.adUseClient
            .Open(strConnection)
        End With
        Log("Import Record Open")
        'now get the Lane Header Records
        objImportRS = New ADODB.Recordset
        strSQL = "Select * From " & strHeaderFile
        objImportRS.Open(strSQL, objImportCon, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText)
        If Not objImportRS.EOF Then objImportRS.MoveLast()
        If Not objImportRS.BOF Then objImportRS.MoveFirst()
        lngMax = objImportRS.RecordCount
        Log("Importing " & lngMax & " Lane Text Records.")
        Dim oLanes(lngMax - 1) As clsLaneObject
        intRowCt = 0
        Do Until objImportRS.EOF
            blnImportingHeaders = True
            Dim oLane As New clsLaneObject
            'ReDim Preserve oLanes(intRowCt)
            'On Error Resume Next
            For intct = 1 To oFields.Count
                Dim TextFileField As ADODB.Field
                On Error GoTo SKIP_FIELD
                TextFileField = objImportRS.Fields(oFields(intct).Name)
                On Error GoTo ErrorHandler
                oFields(intct).Value = validateObjectValue( _
                    objImportRS.Fields(oFields(intct).Name) _
                    , CInt(oFields(intct).DataType) _
                    , strSource _
                    , "Invalid " & oFields(intct).Name _
                    , oFields(intct).Null _
                    , oFields(intct).Length)
                If intct = 55 Then
                    Dim intval As Integer = 0
                End If
                AddFieldToObject(oLane, oFields(intct))
                GoTo NEXT_FIELD
SKIP_FIELD:
                Resume NEXT_FIELD
NEXT_FIELD:

            Next
            oLanes(intRowCt) = oLane
            Me.TotalRecords += 1
            'On Error GoTo ErrorHandler
            intRowCt += 1
            If blnValidationError Then
LogHeaderImportError:
                On Error GoTo ErrorHandler
                blnCreatingErrorLog = True
                strErrorMessage = "'" & padQuotes(strErrorMessage) & "'"
                lngErrCt = lngErrCt + 1
                'build data record string
                strRecord = ""
                blnFirstField = True
                For intct = 1 To oFields.Count
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
                strSQL = "INSERT INTO [dbo].[FileImportErrorLog]" & " ([ImportRecord], [CreateUser], [ErrorDate], [ErrorMsg], " & " [ImportFileName], [ImportFileType], [ImportName])" & " VALUES(" & strRecord & ", '" & mstrCreateUser & "', '" & mstrCreatedDate & "', " & padSpaces(strErrorMessage) & ", '" & padSpaces(strHeaderFile) & "', " & mintImportTypeKey & ", '" & padSpaces(mstrHeaderName) & "')"
                gDBCon.Execute(strSQL)
                blnCreatingErrorLog = False
            Else
                blnInsertRecord = False
            End If
            'Debug.Print strSQL
            strErrorMessage = ""
            blnValidationError = False
            objImportRS.MoveNext()
        Loop
        blnImportingHeaders = False
        objImportRS.Close()
        'Add the data using ngl.feightmaster.integration
        Dim oLaneBLL As New NGL.FreightMaster.Integration.clsLane
        strConnection = "Data Source=" & Me.DBServer & ";Initial Catalog=" & Me.Database & ";Integrated Security=True"
        With oLaneBLL
            .AdminEmail = Me.AdminEmail
            .FromEmail = Me.FromEmail
            .GroupEmail = Me.GroupEmail
            .Retry = Me.Retry
            .SMTPServer = Me.SMTPServer
            .DBServer = Me.DBServer
            .Database = Me.Database
            .AuthorizationCode = "TEXT_IMPORT"
            .WCFAuthCode = Me.WCFAuthCode
            .WCFTCPURL = Me.WCFTCPURL
            .WCFURL = Me.WCFURL
        End With
        intRet = oLaneBLL.ProcessObjectData(oLanes, strConnection)


CleanExit:
        On Error Resume Next
        strTitle = "File Import Complete"
        mintResults = intRet
        Select Case intRet
            Case Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                strMsg = "Database Connection Failure Error: " & oLaneBLL.LastError
            Case Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                strMsg = "Data Integration Failure Error: " & oLaneBLL.LastError
            Case Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                strMsg = "Some Errors: " & oLaneBLL.LastError
            Case Configuration.ProcessDataReturnValues.nglDataValidationFailure
                strMsg = "Data Validation Failure Error: " & oLaneBLL.LastError
            Case Configuration.ProcessDataReturnValues.nglDataIntegrationComplete
                strMsg = "Success! Data imported."
            Case Else
                strMsg = "Invalid Return Value."
        End Select

        If lngErrCt > 0 Then
            strMsg = strMsg & vbCrLf & vbCrLf & "ERROR!  " & lngErrCt & " " & strHeaderTable & " records could not be imported.  " & "Please run the File Import Error Report for more information."
            strTitle = "File Import Complete With Errors"
        End If

        If Not mblnSilent Then
            MsgBox(strMsg, MsgBoxStyle.Information, strTitle)
        Else
            Log(strMsg)
        End If
        If mblnDebug Then
            Console.WriteLine(strMsg)
        End If

        objImportRS = Nothing
        objImportCon = Nothing
        Exit Sub

ErrorHandler:

        Select Case Err.Number
            Case -2147217865
                If Not mblnSilent Then
                    MsgBox(saveErrMsg(formatErrMsg(Err.Number, Err.Description & vbCrLf & vbCrLf & "Cannot locate data import file in folder " & strDataPath & " !", strSource)), MsgBoxStyle.Exclamation, "Cannot Import Records")
                Else
                    Log(saveErrMsg(formatErrMsg(Err.Number, Err.Description & vbCrLf & vbCrLf & "Cannot locate data import file in folder " & strDataPath & " !", strSource)))
                End If
                Resume CleanExit
            Case -2147467259
                If Not mblnSilent Then
                    MsgBox(saveErrMsg(formatErrMsg(Err.Number, Err.Description & vbCrLf & vbCrLf & "Cannot locate import file folder " & strDataPath & " !", strSource)), MsgBoxStyle.Exclamation, "Cannot Import Records")
                Else
                    Log(saveErrMsg(formatErrMsg(Err.Number, Err.Description & vbCrLf & vbCrLf & "Cannot locate import file folder " & strDataPath & " !", strSource)))
                End If
                Resume CleanExit
            Case -2147217843
                If Not mblnSilent Then
                    MsgBox(saveErrMsg(formatErrMsg(Err.Number, Err.Description & vbCrLf & "The user name or password is not correct.", strSource)), MsgBoxStyle.Exclamation, "Cannot Import Records")
                Else
                    Log(saveErrMsg(formatErrMsg(Err.Number, Err.Description & vbCrLf & "The user name or password is not correct.", strSource)))
                End If
                Resume CleanExit
            Case -2147217873

                If blnCreatingErrorLog Then
                    If Not mblnSilent Then
                        MsgBox(saveErrMsg(formatErrMsg(Err.Number, Err.Description, strSource)), MsgBoxStyle.Exclamation, "Cannot Import Records")
                    Else
                        Log(saveErrMsg(formatErrMsg(Err.Number, Err.Description, strSource)))
                    End If
                    Resume CleanExit
                Else
                    strErrorMessage = "Cannot insert record because the record already exists or another validation rule failed."
                    If blnImportingHeaders Then
                        Resume LogHeaderImportError
                    Else
                        If Not mblnSilent Then
                            MsgBox(saveErrMsg(formatErrMsg(Err.Number, Err.Description, strSource)), MsgBoxStyle.Exclamation, "Cannot Import Records")
                        Else
                            Log(saveErrMsg(formatErrMsg(Err.Number, Err.Description, strSource)))
                        End If
                        Resume CleanExit

                    End If
                End If
            Case -2147217904, -2147217900
                If Not mblnSilent Then
                    MsgBox(saveErrMsg(formatErrMsg(Err.Number, Err.Description & vbCrLf & vbCrLf & "Error in Import File or Schema.ini file.", strSource)), MsgBoxStyle.Exclamation, "Cannot Import Records")
                Else
                    Log(saveErrMsg(formatErrMsg(Err.Number, Err.Description & vbCrLf & vbCrLf & "Error in Import File or Schema.ini file.", strSource)))
                End If
                Resume CleanExit
                'Resume 'used for test

            Case Else
                If Not mblnSilent Then
                    MsgBox(saveErrMsg(formatErrMsg(Err.Number, Err.Description, strSource)), MsgBoxStyle.Exclamation, "Cannot Import Records")
                Else
                    Log(saveErrMsg(formatErrMsg(Err.Number, Err.Description, strSource)))
                End If
                Resume CleanExit
                'Resume 'used for test
        End Select
        Resume CleanExit
        'Resume 'used for test

ValidationError:

        If blnValidationError Then
            strErrorMessage = strErrorMessage & "; "
        Else
            blnValidationError = True
        End If
        strErrorMessage = strErrorMessage & Err.Description

        If Err.Number > gclngErrorNumber2 And Err.Number < gclngErrorNumber4 Then
            'this is a validation error so we just continue with the test
            Resume Next
        Else
            'this is some other error and we should write it to the error log and move to the next record.
            If blnImportingHeaders Then
                Resume LogHeaderImportError
            Else
                If Not mblnSilent Then
                    MsgBox(saveErrMsg(formatErrMsg(Err.Number, Err.Description, strSource)), MsgBoxStyle.Exclamation, "Cannot Import Records")
                Else
                    Log(saveErrMsg(formatErrMsg(Err.Number, Err.Description, strSource)))
                End If
                Resume CleanExit

            End If
        End If

    End Sub

End Class
