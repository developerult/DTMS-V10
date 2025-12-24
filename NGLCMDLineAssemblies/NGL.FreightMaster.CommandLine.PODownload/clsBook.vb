Option Strict Off
Option Explicit On

Imports ngl.FreightMaster.FMLib.General
Imports ngl.FreightMaster.FMLib.dbUtilities
Imports NGL.FreightMaster.Integration

Public Class clsBook : Inherits clsDownload
    Private mintTotalOrders As Integer = 0
    Private mdblHashTotalOrders As Double = 0
    Private mintTotalDetails As Integer = 0
    Private mintTotalQty As Integer = 0
    Private mdblTotalWeight As Double = 0
    Private mdblHashTotalDetails As Double = 0
    Private mintErrCt As Integer = 0
    'Private objF As New FMLib.clsStandardFunctions


    Public ReadOnly Property TotalWeight() As Double
        Get
            TotalWeight = mdblTotalWeight
        End Get
    End Property

    Public ReadOnly Property TotalQty() As Integer
        Get
            TotalQty = mintTotalQty
        End Get
    End Property

    Public ReadOnly Property HashTotalDetails() As Double
        Get
            HashTotalDetails = mdblHashTotalDetails
        End Get
    End Property

    Public ReadOnly Property TotalDetails() As Integer
        Get
            TotalDetails = mintTotalDetails
        End Get
    End Property



    Public ReadOnly Property HashTotalOrders() As Double
        Get
            HashTotalOrders = mdblHashTotalOrders
        End Get
    End Property

    Public ReadOnly Property TotalOrders() As Integer
        Get
            TotalOrders = mintTotalOrders
        End Get
    End Property


    Public Sub FileImport(ByRef strHeaderFile As String, ByRef strItemFile As String, ByRef strDataPath As String)
        Dim intRet As Configuration.ProcessDataReturnValues
        Dim strMsg As String = ""
        Dim strSQL As String = ""
        Dim objImportCon As ADODB.Connection
        Dim objImportRS As New ADODB.Recordset
        Dim blnValidationError As Boolean = False
        Dim strErrorMessage As String = ""
        Dim lngErrCt As Integer = 0
        Dim lngImportCount As Integer = 0
        Dim lngMax As Integer = 0
        Dim lngCurrent As Long = 0
        Dim lngImportItemCount As Integer = 0
        Dim lngImportItemErrors As Long = 0
        Dim blnImportingHeaders As Boolean = False
        Dim blnImportingItems As Boolean = False
        Dim strSource As String = "clsBook.FileImport"
        Dim blnInsertRecord As Boolean = False
        Dim strConnection As String = ""
        Dim objCom As ADODB.Command
        Dim intRetryCt As Integer = 0
        Dim intRowCt As Integer


        Try
            'Check if the header file exists
            If Not System.IO.File.Exists(objF.buildPath(strDataPath, strHeaderFile)) Then
                Log("The import header file " & objF.buildPath(strDataPath, strHeaderFile) & "does not exists.")
                Exit Sub
            End If
        Catch ex As Exception
            LogError("PODownload System Error", "PODownload.clsBook.FileImport: Read POHeader record failure" & vbCrLf & ex.ToString, GroupEmail)
            Exit Sub
        End Try

        strConnection = "Driver={Microsoft Text Driver (*.txt; *.csv)}; Dbq=" _
            & strDataPath & ";Extensions=asc,csv,tab,txt;HDR=YES;Persist Security Info=False"
        strSource = "clsBook.FileImport"
        intRetryCt = 0
        Try
            Do
                intRetryCt += 1
                Try
                    'create a new connection
                    objImportCon = New ADODB.Connection
                    With objImportCon
                        .CursorLocation = ADODB.CursorLocationEnum.adUseClient
                        .Open(strConnection)
                    End With
                    'now get the PO Header Records
                    objImportRS = New ADODB.Recordset

                    strSQL = "Select * From " & strHeaderFile
                    objImportRS.Open(strSQL, objImportCon, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText)
                    Exit Do
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        LogError("PODownload Failure", "PODownload.clsBook.FileImport: Order download open POHeader file error" & vbCrLf & ex.ToString, GroupEmail)
                        Exit Sub
                    Else
                        Log("FileImport Open POHeader File Failure Retry = " & intRetryCt.ToString)

                    End If
                End Try
                'We only get here if an exception is thrown and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
            'Now we import the Header Records
            Try
                If Not objImportRS.EOF Then objImportRS.MoveLast()
                If Not objImportRS.BOF Then objImportRS.MoveFirst()
                lngMax = objImportRS.RecordCount
            Catch ex As Exception
                LogError("PODownload Failure", "PODownload.clsBook.FileImport: Order download access POHeader records failure" & vbCrLf & ex.ToString, GroupEmail)
                Exit Sub
            End Try
            lngCurrent = 0
            Log("Importing " & lngMax & " Header Records.")
            Dim oPOHeaders(lngMax - 1) As clsBookHeaderObject
            intRowCt = 0
            Do Until objImportRS.EOF
                blnImportingHeaders = True
                lngCurrent = lngCurrent + 1
                'validate all the fields in the current record
                'if any errors exist in the data we write the
                'information to an error table where a report
                'can be run.  The error information is stored
                'in blnVError and strErrorMessage
                Dim PO As New clsBookHeaderObject
                Dim blnVError As Boolean = False
                strErrorMessage = ""
                If FillHeaderData(PO, objImportRS, blnVError, strErrorMessage, strSource) Then
                    If blnVError Then
                        LogHeaderImportError(PO, strErrorMessage)
                    Else
                        oPOHeaders(intRowCt) = PO
                        intRowCt += 1
                        lngImportCount = lngImportCount + 1
                        mintTotalOrders += 1
                        mdblHashTotalOrders += Val(stripQuotes(PO.PONumber))
                    End If
                End If
                strErrorMessage = ""
                blnValidationError = False
                objImportRS.MoveNext()
            Loop
            blnImportingHeaders = False
            Try
                objImportRS.Close()
            Catch ex As Exception

            End Try
            'End of importing Header Records  
            Try
                objImportRS = Nothing
            Catch e As Exception

            End Try
            Try
                objImportCon = Nothing
            Catch e As Exception

            End Try
            Dim oPODetails() As clsBookDetailObject
            Try
                'Check if the detail file exists
                If Not System.IO.File.Exists(objF.buildPath(strDataPath, strItemFile)) Then
                    'LogError("PODownload Failure", "PODownload.clsBook.FileImport: Order detail download failure" & vbCrLf _
                    '    & "The import detail file " & objF.buildPath(strDataPath, strItemFile) & " does not exists.", _
                    '    AdminEmail)
                    Log("The import detail file " & objF.buildPath(strDataPath, strItemFile) & "does not exists.")
                    'Exit Sub
                Else
                    intRetryCt = 0
                    Do
                        intRetryCt += 1
                        Try
                            'create a new connection
                            objImportCon = New ADODB.Connection
                            With objImportCon
                                .CursorLocation = ADODB.CursorLocationEnum.adUseClient
                                .Open(strConnection)
                            End With
                            'now get the PO Item Records
                            objImportRS = New ADODB.Recordset
                            strSQL = "Select * From " & strItemFile
                            objImportRS.Open(strSQL, objImportCon, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText)
                            Exit Do
                        Catch ex As Exception
                            If intRetryCt > Me.Retry Then
                                LogError("PODownload Failure", "PODownload.clsBook.FileImport: Order download open POItem file error" & vbCrLf & ex.ToString, AdminEmail)
                                Exit Sub
                            Else
                                Log("FileImport Open POItem File Failure Retry = " & intRetryCt.ToString)
                            End If
                        End Try
                        'We only get here if an exception is thrown and intRetryCt <= 3
                    Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
                    Try
                        If Not objImportRS.EOF Then objImportRS.MoveLast()
                        If Not objImportRS.BOF Then objImportRS.MoveFirst()
                        lngMax = objImportRS.RecordCount
                    Catch ex As Exception
                        LogError("PODownload Failure", "PODownload.clsBook.FileImport: Order download access POItem record failure" & vbCrLf & ex.ToString, AdminEmail)
                        Exit Sub
                    End Try
                    lngCurrent = 0
                    Log("Importing " & lngMax & " Detail Records.")
                    ReDim oPODetails(lngMax - 1)
                    intRowCt = 0
                    Do Until objImportRS.EOF
                        blnImportingItems = True
                        lngCurrent = lngCurrent + 1
                        'validate all the fields in the current record
                        'if any errors exist in the data we write the
                        'information to an error table where a report
                        'can be run.  The error information is stored
                        'in blnVError and strErrorMessage
                        Dim PI As New clsBookDetailObject
                        Dim blnVError As Boolean = False
                        strErrorMessage = ""
                        If FillItemData(PI, objImportRS, blnVError, strErrorMessage, strSource) Then
                            If blnVError Then
                                LogItemImportError(PI, strErrorMessage)
                            Else
                                oPODetails(intRowCt) = PI
                                intRowCt += 1
                                lngImportItemCount += 1
                                mintTotalDetails += 1
                                mintTotalQty += Val(PI.QtyOrdered)
                                mdblTotalWeight += Val(PI.Weight)
                                mdblHashTotalDetails += Val(stripQuotes(PI.ItemNumber))
                            End If
                        End If
                        strErrorMessage = ""
                        blnValidationError = False
                        objImportRS.MoveNext()
                    Loop
                    blnImportingItems = False
                    Try
                        objImportRS.Close()
                    Catch ex As Exception

                    End Try

                End If
            Catch ex As Exception
                LogError("PODownload Failure", "PODownload.clsBook.FileImport: Order download system error" & vbCrLf & ex.ToString, AdminEmail)
                Exit Sub
            End Try


            'Add the data using ngl.freightmaster.integration
            Dim oBookBLL As New NGL.FreightMaster.Integration.clsBook
            strConnection = "Data Source=" & Me.DBServer & ";Initial Catalog=" & Me.Database & ";Integrated Security=True"
            With oBookBLL
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
                .RunSilentTenderAsync = False 'new property added RHR 3/18/15 
            End With
            intRet = oBookBLL.ProcessObjectData(oPOHeaders, oPODetails, strConnection)
            Select Case intRet
                Case Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                    lngErrCt += 1
                    'set all counts and hash totals to zero
                    mintTotalOrders = 0
                    mdblHashTotalOrders = 0
                    lngImportItemCount = 0
                    mintTotalDetails = 0
                    mintTotalQty = 0
                    mdblTotalWeight = 0
                    mdblHashTotalDetails = 0
                    strMsg = "Database Connection Failure Error: " & oBookBLL.LastError
                Case Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                    lngErrCt += 1
                    'set all counts and hash totals to zero
                    mintTotalOrders = 0
                    mdblHashTotalOrders = 0
                    lngImportItemCount = 0
                    mintTotalDetails = 0
                    mintTotalQty = 0
                    mdblTotalWeight = 0
                    mdblHashTotalDetails = 0
                    'set all counts and hash totals to zero
                    mintTotalOrders = 0
                    mdblHashTotalOrders = 0
                    lngImportItemCount = 0
                    mintTotalDetails = 0
                    mintTotalQty = 0
                    mdblTotalWeight = 0
                    mdblHashTotalDetails = 0
                    strMsg = "Data Integration Failure Error: " & oBookBLL.LastError
                Case Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                    lngErrCt += 1
                    'set all counts and hash totals to zero
                    mintTotalOrders = 0
                    mdblHashTotalOrders = 0
                    lngImportItemCount = 0
                    mintTotalDetails = 0
                    mintTotalQty = 0
                    mdblTotalWeight = 0
                    mdblHashTotalDetails = 0
                    strMsg = "Some Errors: " & oBookBLL.LastError
                Case Configuration.ProcessDataReturnValues.nglDataValidationFailure
                    lngErrCt += 1
                    'set all counts and hash totals to zero
                    mintTotalOrders = 0
                    mdblHashTotalOrders = 0
                    lngImportItemCount = 0
                    mintTotalDetails = 0
                    mintTotalQty = 0
                    mdblTotalWeight = 0
                    mdblHashTotalDetails = 0
                    strMsg = "Data Validation Failure Error: " & oBookBLL.LastError
                Case Configuration.ProcessDataReturnValues.nglDataIntegrationComplete
                    Results = lngImportCount
                    If lngImportCount > 0 Then
                        strMsg = "Success!  " & lngImportCount & " PO Header records were imported."
                    Else
                        strMsg = "No PO Header records were imported."
                    End If
                    If lngImportItemCount > 0 Then
                        strMsg = strMsg & vbCrLf & vbCrLf & "Success!  " & lngImportItemCount _
                            & " PO Item detail records were imported."
                    Else
                        strMsg = strMsg & vbCrLf & vbCrLf & "No Item detail records were imported."
                    End If
                Case Else
                    lngErrCt += 1
                    'set all counts and hash totals to zero
                    mintTotalOrders = 0
                    mdblHashTotalOrders = 0
                    lngImportItemCount = 0
                    mintTotalDetails = 0
                    mintTotalQty = 0
                    mdblTotalWeight = 0
                    mdblHashTotalDetails = 0
                    strMsg = "Invalid Return Value."
            End Select
            Log(strMsg)

        Catch ex As Exception
            LogError("PODownload Failure", "PODownload.clsBook.FileImport: File import system error" & vbCrLf & ex.ToString, AdminEmail)
            Exit Sub
        Finally
            Try
                objImportRS = Nothing
            Catch e As Exception
            End Try
            Try
                objImportCon = Nothing
            Catch e As Exception
            End Try
        End Try

    End Sub


    Public Sub LogHeaderImportError(ByVal PO As clsBookHeaderObject, ByVal strErrorMessage As String)

        Dim strSQL As String = ""
        Try
            strErrorMessage = "'" & padQuotes(strErrorMessage) & "'"
            mintErrCt += 1
            'build execute string into error table
            strSQL = "Insert Into POHDRImportErrorLog" _
                & "(POnumber" _
                & ", POvendor" _
                & ", POdate" _
                & ", POShipdate" _
                & ", POBuyer" _
                & ", POFrt" _
                & ", POCreateUser" _
                & ", POTotalFrt" _
                & ", POTotalCost" _
                & ", POWgt" _
                & ", POCube" _
                & ", POQty" _
                & ", POLines" _
                & ", POConfirm" _
                & ", PODefaultCustomer, POErrorDate " _
                & ", POReqDate" _
                & ", POShipInstructions" _
                & ", POCooler" _
                & ", POFrozen" _
                & ", PODry" _
                & ", POTemp" _
                & ", POCarType" _
                & ", POShipVia, POShipViaType" _
                & ", POErrorMsg, POPallets, POOtherCosts,POStatusFlag)"

            strSQL = strSQL & " Values " _
                & "(" & padSpaces(PO.PONumber) _
                & "," & padSpaces(PO.POVendor) _
                & "," & padSpaces(PO.POdate) _
                & "," & padSpaces(PO.POShipdate) _
                & "," & padSpaces(PO.POBuyer) _
                & "," & padSpaces(PO.POFrt) _
                & ",'" & CreateUser & "'" _
                & "," & padSpaces(PO.POTotalFrt) _
                & "," & padSpaces(PO.POTotalCost) _
                & "," & padSpaces(PO.POWgt) _
                & "," & padSpaces(PO.POCube) _
                & "," & padSpaces(PO.POQty) _
                & "," & padSpaces(PO.POLines) _
                & "," & padSpaces(PO.POConfirm) _
                & "," & padSpaces(PO.PODefaultCustomer) & ",'" & CreatedDate & "'" _
                & "," & padSpaces(PO.POReqDate) _
                & "," & padSpaces(PO.POShipInstructions) _
                & "," & padSpaces(PO.POCooler) _
                & "," & padSpaces(PO.POFrozen) _
                & "," & padSpaces(PO.PODry) _
                & "," & padSpaces(PO.POTemp) _
                & "," & padSpaces(PO.POCarType) _
                & "," & padSpaces(PO.POShipVia) & "," & padSpaces(PO.POShipViaType) _
                & "," & padSpaces(strErrorMessage) _
                & "," & padSpaces(PO.POPallets) _
                & "," & padSpaces(PO.POOtherCosts) _
                & "," & padSpaces(PO.POStatusFlag) & ")"
            gDBCon.Execute(strSQL)
            LogError("PODownload Failure", "PODownload.clsBook.LogHeaderImportError: POHeader validation error" & vbCrLf & strErrorMessage & vbCrLf & strSQL & vbCrLf & "Run the PO Header Import Error report for more information.", AdminEmail)
        Catch ex As Exception
            LogError("PODownload Failure", "PODownload.clsBook.LogHeaderImportError: Log header import error failure" & vbCrLf & ex.ToString & vbCrLf & strErrorMessage & vbCrLf & strSQL, AdminEmail)
        End Try



    End Sub

    Public Sub LogItemImportError(ByVal PI As clsBookDetailObject, ByVal strErrorMessage As String)

        Dim strSQL As String = ""
        Try
            strErrorMessage = "'" & padQuotes(strErrorMessage) & "'"
            mintErrCt += 1
            'build execute string into error table
            strSQL = "Insert Into POItemImportErrorLog" _
                        & "(ItemPONumber" _
                        & ", FixOffInvAllow" _
                        & ", FixFrtAllow" _
                        & ", ItemNumber" _
                        & ", QtyOrdered" _
                        & ", FreightCost" _
                        & ", ItemCost" _
                        & ", Weight" _
                        & ", Cube" _
                        & ", Pack" _
                        & ", Size" _
                        & ", Description" _
                        & ", Hazmat" _
                        & ", CreatedUser" _
                        & ", ItemErrorDate" _
                        & ", ItemErrorMsg" _
                        & ", Brand" _
                        & ", CostCenter" _
                        & ", LotNumber" _
                        & ", LotExpirationDate" _
                        & ", GTIN" _
                        & ", CustItemNumber" _
                        & ", CustomerNumber)"

            strSQL = strSQL & " Values " _
                        & "(" & padSpaces(PI.ItemPONumber) _
                        & "," & padSpaces(PI.FixOffInvAllow) _
                        & "," & padSpaces(PI.FixFrtAllow) _
                        & "," & padSpaces(PI.ItemNumber) _
                        & "," & padSpaces(PI.QtyOrdered) _
                        & "," & padSpaces(PI.FreightCost) _
                        & "," & padSpaces(PI.ItemCost) _
                        & "," & padSpaces(PI.Weight) _
                        & "," & padSpaces(PI.Cube) _
                        & "," & padSpaces(PI.Pack) _
                        & "," & padSpaces(PI.Size) _
                        & "," & padSpaces(PI.Description) _
                        & "," & padSpaces(PI.Hazmat) _
                        & ",'" & CreateUser & "'" _
                        & ",'" & CreatedDate & "'" _
                        & "," & padSpaces(strErrorMessage) _
                        & "," & padSpaces(PI.Brand) _
                        & "," & padSpaces(PI.CostCenter) _
                        & "," & padSpaces(PI.LotNumber) _
                        & "," & padSpaces(PI.LotExpirationDate) _
                        & "," & padSpaces(PI.GTIN) _
                        & "," & padSpaces(PI.CustItemNumber) _
                        & "," & padSpaces(PI.CustomerNumber) & ")"
            gDBCon.Execute(strSQL)
            LogError("PODownload Failure", "PODownload.clsBook.LogItemImportError: POItem validation error" & vbCrLf & strErrorMessage & vbCrLf & strSQL & vbCrLf & "Run the PO Item Import Error report for more information.", AdminEmail)
        Catch ex As Exception
            LogError("PODownload Failure", "PODownload.clsBook.LogItemImportError: Log POItem import error failure" & vbCrLf & ex.ToString & vbCrLf & strErrorMessage & vbCrLf & strSQL, AdminEmail)
        End Try
    End Sub

    Public Function validateFieldData(ByRef blnVError As Boolean, _
        ByRef strErrorMessage As String, _
        ByRef objrs As ADODB.Field, _
        ByVal enm As FMLib.General.ValidateDataType, _
        ByVal strSource As String, _
        ByVal strErrMsg As String, _
        Optional ByVal blnAllowNull As Boolean = False, _
        Optional ByVal intLength As Integer = 1) As String
        Dim strRetVal As String = ""
        Try
            strRetVal = validateObjectValue(objrs _
                , enm _
                , strSource _
                , strErrMsg _
                , blnAllowNull _
                , intLength)
        Catch ex As Exception
            Try
                blnVError = True
                strErrorMessage &= ex.ToString
            Catch e As Exception
            End Try
        End Try
        Return strRetVal

    End Function

    Public Function validatePossibleFieldData(ByVal strDefault As String, _
        ByRef objrs As ADODB.Field, _
        ByVal enm As FMLib.General.ValidateDataType, _
        ByVal strSource As String, _
        ByVal strErrMsg As String, _
        Optional ByVal blnAllowNull As Boolean = False, _
        Optional ByVal intLength As Integer = 1) As String
        Dim strRetVal As String = strDefault
        Try
            strRetVal = validateObjectValue(objrs _
                , enm _
                , strSource _
                , strErrMsg _
                , blnAllowNull _
                , intLength)
        Catch ex As Exception
        End Try
        Return strRetVal

    End Function

    Public Function FillHeaderData(ByRef PO As clsBookHeaderObject _
        , ByRef objImportRS As ADODB.Recordset _
        , ByRef blnVError As Boolean _
        , ByRef strErrorMessage As String _
        , ByRef strSource As String) As Boolean
        Try
            ' Field # 1
            PO.PONumber = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("POnumber") _
                , ValidateDataType.vdtString _
                , strSource _
                , "Invalid PONumber" _
                , False _
                , 20)
            ' Field # 2
            'Try
            '    PO.NewLaneNumber = nz(objImportRS.Fields("POvendor"), "")
            'Catch ex As Exception
            'End Try

            PO.POvendor = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("POvendor") _
                , ValidateDataType.vdtString _
                , strSource _
                , "Invalid POvendor" _
                , False _
                , 50)
            ' Field # 3
            PO.POdate = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("POdate") _
                , ValidateDataType.vdtDate _
                , strSource _
                , "Invalid POdate" _
                , True)
            ' Field # 4
            PO.POShipdate = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("POShipdate") _
                , ValidateDataType.vdtDate _
                , strSource _
                , "Invalid POShipdate" _
                , True)
            ' Field # 5
            PO.POBuyer = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("POBuyer") _
                , ValidateDataType.vdtString _
                , strSource _
                , "Invalid POBuyer" _
                , True _
                , 10)
            ' Field # 6
            PO.POFrt = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("POFrt") _
                , ValidateDataType.vdtTinyInt _
                , strSource _
                , "Invalid POFrt" _
                , True)
            ' Field # 7
            PO.POTotalFrt = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("POTotalFrt") _
                , ValidateDataType.vdtFloat _
                , strSource _
                , "Invalid POTotalFrt" _
                , True)
            ' Field # 8
            PO.POTotalCost = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("POTotalCost") _
                , ValidateDataType.vdtFloat _
                , strSource _
                , "Invalid POTotalCost" _
                , True)
            ' Field # 9
            PO.POWgt = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("POWgt") _
                , ValidateDataType.vdtFloat _
                , strSource _
                , "Invalid POWgt" _
                , True)
            ' Field # 10
            PO.POCube = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("POCube") _
                , ValidateDataType.vdtLongInt _
                , strSource _
                , "Invalid POCube" _
                , True)
            ' Field # 11
            PO.POQty = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("POQty") _
                , ValidateDataType.vdtLongInt _
                , strSource _
                , "Invalid POQty" _
                , True)
            ' Field # 12
            PO.POPallets = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("POPallets") _
                , ValidateDataType.vdtLongInt _
                , strSource _
                , "Invalid POPallets" _
                , True)
            ' Field # 13
            PO.POLines = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("POLines") _
                , ValidateDataType.vdtFloat _
                , strSource _
                , "Invalid POLines" _
                , True)
            ' Field # 14
            PO.POConfirm = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("POConfirm") _
                , ValidateDataType.vdtBit _
                , strSource _
                , "Invalid POConfirm" _
                , True)
            ' Field # 15
            PO.PODefaultCustomer = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("PODefaultCustomer") _
                , ValidateDataType.vdtString _
                , strSource _
                , "Invalid PODefaultCustomer" _
                , True _
                , 50)
            '            Dim intRetryCt As Integer = 0
            'GetCompNumberByAlpha:
            '            Try
            '                ' test if a cross reference record exists for the DefaultCustomer
            '                Dim objTestRS As New ADODB.Recordset
            '                Dim strSQL As String = "Select dbo.udfGetCompNumberByAlpha(" & PO.PODefaultCustomer & ") as CompNumber"
            '                With objTestRS
            '                    .Open(strSQL, gDBCon, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)
            '                    If .RecordCount > 0 Then
            '                        ' Validate Field # 15 Again (with new cross reference data)
            '                        PO.PODefaultCustomer = validateFieldData(blnVError _
            '                            , strErrorMessage _
            '                            , objTestRS.Fields("CompNumber") _
            '                            , ValidateDataType.vdtLongInt _
            '                            , strSource _
            '                            , "Invalid PODefaultCustomer" _
            '                            , True)
            '                    Else
            '                        ' Validate Field # 15 Again (check if customer number is int: No Cross Reference Available)
            '                        PO.PODefaultCustomer = validateFieldData(blnVError _
            '                            , strErrorMessage _
            '                            , objImportRS.Fields("PODefaultCustomer") _
            '                            , ValidateDataType.vdtLongInt _
            '                            , strSource _
            '                            , "Invalid PODefaultCustomer" _
            '                            , True)
            '                    End If
            '                    .Close()
            '                End With
            '            Catch ex As Exception
            '                intRetryCt += 1
            '                If intRetryCt > Me.Retry Then
            '                    LogError("PODownload Failure", "PODownload.clsBook.FillHeaderData: Order download failure call udfGetCompNumberByAlpha error; order number " & PO.POOrderNumber & " failed to import" & vbCrLf & ex.ToString, AdminEmail)
            '                    Return False
            '                Else
            '                    Log("FillHeaderData udfGetCompNumberByAlpha Failure Retry = " & intRetryCt.ToString)

            '                    GoTo GetCompNumberByAlpha
            '                End If
            '            End Try
            ' Field # 16
            PO.PODefaultCarrier = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("PODefaultCarrier") _
                , ValidateDataType.vdtLongInt _
                , strSource _
                , "Invalid PODefaultCarrier" _
                , True)
            '  Field # 17
            PO.POReqDate = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("POReqDate") _
                , ValidateDataType.vdtDate _
                , strSource _
                , "Invalid POReqDate" _
                , True)
            ' Field # 18
            PO.POShipInstructions = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("POShipInstructions") _
                , ValidateDataType.vdtString _
                , strSource _
                , "Invalid POShipInstructions" _
                , True _
                , 255)
            ' Field # 19
            PO.POCooler = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("POCooler") _
                , ValidateDataType.vdtBit _
                , strSource _
                , "Invalid POCooler" _
                , True)
            ' Field # 20
            PO.POFrozen = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("POFrozen") _
                , ValidateDataType.vdtBit _
                , strSource _
                , "Invalid POFrozen" _
                , True)
            ' Field # 21
            PO.PODry = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("PODry") _
                , ValidateDataType.vdtLongInt _
                , strSource _
                , "Invalid PODry" _
                , True)
            ' Field # 22
            PO.POTemp = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("POTemp") _
                , ValidateDataType.vdtString _
                , strSource _
                , "Invalid POTemp" _
                , True _
                , 1)
            ' Field # 23
            PO.POCarType = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("POCarType") _
                , ValidateDataType.vdtString _
                , strSource _
                , "Invalid POCarType" _
                , True _
                , 15)
            ' Field # 24
            PO.POShipVia = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("POShipVia") _
                , ValidateDataType.vdtString _
                , strSource _
                , "Invalid POShipVia" _
                , True _
                , 10)
            ' Field # 25
            PO.POShipViaType = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("POShipViaType") _
                , ValidateDataType.vdtString _
                , strSource _
                , "Invalid POShipViaType" _
                , True _
                , 10)
            ' Field # 26
            PO.POConsigneeNumber = ""
            Try
                PO.POConsigneeNumber = nz(objImportRS.Fields("POConsigneeNumber"), "0")
            Catch ex As Exception

            End Try
            ' Field # 27
            PO.POCustomerPO = validatePossibleFieldData("", objImportRS.Fields("POCustomerPO") _
                , ValidateDataType.vdtString _
                , strSource _
                , "Invalid POCustomerPO" _
                , True _
                , 20)
            ' Field # 28
            PO.POOtherCosts = validatePossibleFieldData("0", objImportRS.Fields("POOtherCosts") _
                , ValidateDataType.vdtFloat _
                , strSource _
                , "Invalid POOtherCosts" _
                , True)
            ' Field # 29
            PO.POStatusFlag = validatePossibleFieldData("0", objImportRS.Fields("POStatusFlag") _
                , ValidateDataType.vdtLongInt _
                , strSource _
                , "Invalid POStatusFlag" _
                , True)
            Try
                If PO.PONumber.Length < 1 And PO.POCustomerPO.Length > 0 Then
                    PO.PONumber = PO.POCustomerPO
                End If

            Catch ex As Exception

            End Try
            Return True

        Catch ex As Exception
            LogError("PODownload Failure", "PODownload.clsBook.FillHeaderData: Order download fill POHeader data failure; order number " & PO.PONumber & " failed to import" & vbCrLf & ex.ToString, AdminEmail)
            Return False
        End Try
    End Function

    Public Function FillItemData(ByRef PI As clsBookDetailObject _
        , ByRef objImportRS As ADODB.Recordset _
        , ByRef blnVError As Boolean _
        , ByRef strErrorMessage As String _
        , ByRef strSource As String) As Boolean
        Try
            ' Field # 1
            PI.ItemPONumber = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("ItemPONumber") _
                , ValidateDataType.vdtString _
                , strSource _
                , "Invalid ItemPONumber" _
                , False _
                , 20)
            ' Field # 2           
            PI.FixOffInvAllow = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("FixOffInvAllow") _
                , ValidateDataType.vdtMoney _
                , strSource _
                , "Invalid FixOffInvAllow" _
                , True)
            ' Field # 3
            PI.FixFrtAllow = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("FixFrtAllow") _
                , ValidateDataType.vdtMoney _
                , strSource _
                , "Invalid FixFrtAllow" _
                , True)
            ' Field # 4
            PI.ItemNumber = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("ItemNumber") _
                , ValidateDataType.vdtString _
                , strSource _
                , "Invalid ItemNumber" _
                , True _
                , 50)
            ' Field # 5
            PI.QtyOrdered = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("QtyOrdered") _
                , ValidateDataType.vdtLongInt _
                , strSource _
                , "Invalid QtyOrdered" _
                , True)
            ' Field # 6
            PI.FreightCost = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("FreightCost") _
                , ValidateDataType.vdtMoney _
                , strSource _
                , "Invalid FreightCost" _
                , True)
            ' Field # 7
            PI.ItemCost = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("ItemCost") _
                , ValidateDataType.vdtMoney _
                , strSource _
                , "Invalid ItemCost" _
                , True)
            ' Field # 8
            PI.Weight = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("Weight") _
                , ValidateDataType.vdtFloat _
                , strSource _
                , "Invalid Weight" _
                , True)
            ' Field # 9
            PI.Cube = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("Cube") _
                , ValidateDataType.vdtLongInt _
                , strSource _
                , "Invalid Cube" _
                , True)
            ' Field # 10
            PI.Pack = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("Pack") _
                , ValidateDataType.vdtSmallInt _
                , strSource _
                , "Invalid Pack" _
                , True)
            ' Field # 11
            PI.Size = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("Size") _
                , ValidateDataType.vdtString _
                , strSource _
                , "Invalid Size" _
                , True _
                , 255)
            ' Field # 12
            PI.Description = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("Description") _
                , ValidateDataType.vdtString _
                , strSource _
                , "Invalid Description" _
                , True _
                , 255)
            ' Field # 13
            PI.Hazmat = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("Hazmat") _
                , ValidateDataType.vdtString _
                , strSource _
                , "Invalid Hazmat" _
                , True _
                , 1)
            ' Field # 14
            PI.Brand = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("Brand") _
                , ValidateDataType.vdtString _
                , strSource _
                , "Invalid Brand" _
                , True _
                , 255)
            ' Field # 15
            PI.CostCenter = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("CostCenter") _
                , ValidateDataType.vdtString _
                , strSource _
                , "Invalid Cost Center" _
                , True _
                , 50)
            ' Field # 16
            PI.LotNumber = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("LotNumber") _
                , ValidateDataType.vdtString _
                , strSource _
                , "Invalid Lot Number" _
                , True _
                , 50)
            '  Field # 17
            PI.LotExpirationDate = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("LotExpirationDate") _
                , ValidateDataType.vdtDate _
                , strSource _
                , "Invalid Lot Experation Date" _
                , True)
            ' Field # 18
            PI.GTIN = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("GTIN") _
                , ValidateDataType.vdtString _
                , strSource _
                , "Invalid GTIN" _
                , True _
                , 50)
            ' Field # 19
            PI.CustItemNumber = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("CustItemNumber") _
                , ValidateDataType.vdtString _
                , strSource _
                , "Invalid Customer Item Number" _
                , True _
                , 50)
            ' Field # 20
            PI.CustomerNumber = validateFieldData(blnVError _
                , strErrorMessage _
                , objImportRS.Fields("CustomerNumber") _
                , ValidateDataType.vdtString _
                , strSource _
                , "Invalid Item Customer Number" _
                , True _
                , 50)


            Return True

        Catch ex As Exception
            LogError("PODownload Failure", "PODownload.clsBook.FillItemData: Order detail download failure fill POItem data failure; order item number " & PI.ItemNumber & " failed to import" & vbCrLf & ex.ToString, AdminEmail)
            Return False
        End Try
    End Function

    Public Sub UpdateLaneNumber(ByRef PO As clsPOHeader)
        Dim objTestRS As New ADODB.Recordset
        Dim strSQL As String = ""
        Try
            With objTestRS
                strSQL = "Select LaneNumber From Lane Where LaneConsigneeNumber = '" _
                    & PO.POConsigneeNumber & "'"
                .Open(strSQL, gDBCon, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)
                If .RecordCount > 0 Then
                    'Edit the record
                    .Fields("LaneNumber").Value = PO.NewLaneNumber
                    .Update()
                End If
                .Close()
            End With
        Catch ex As Exception
            LogError("PODownload error download not affected", "PODownload.clsBook.UpdateLaneNumber: Update lane number error order download not affected; could not update the booking lane number for order number " & PO.POOrderNumber & ".  The order was downloaded successfully: " & vbCrLf & ex.ToString, Me.AdminEmail)
        Finally
            objTestRS = Nothing
        End Try

    End Sub

    Public Function searchForExistingHeader(ByRef PO As clsPOHeader, ByRef blnInsertRecord As Boolean) As Boolean
        'test if the record already exists.
        Dim objTestRS As New ADODB.Recordset
        Dim blnRetVal As Boolean = False
        If PO Is Nothing Then Return False

        Try
            With objTestRS
                'reset the counter
                Dim intRetryCt As Integer = 0
                Do

                    intRetryCt += 1
                    Try
                        Dim strSQL As String = "Select POHDRCreateUser, POHDRCreateDate From POHDR Where POHDRDefaultCustomer = " _
                            & PO.PODefaultCustomer & " And POHDROrderNumber = " & PO.POOrderNumber
                        .Open(strSQL, gDBCon, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)
                        If .RecordCount > 0 Then
                            'set the insert flag to false
                            blnInsertRecord = False
                        Else
                            'set the insert flat to true
                            blnInsertRecord = True
                        End If
                        .Close()
                        blnRetVal = True
                        Exit Do
                    Catch ex As Exception
                        If intRetryCt > Me.Retry Then
                            LogError("PODownload Failure", "PODownload.clsBook.searchForExistingHeader: Order download check for existing POHeader record failure for order number " & PO.POOrderNumber & vbCrLf & ex.ToString, AdminEmail)

                        Else
                            Log("searchForExistingHeader Failure Retry = " & intRetryCt.ToString)

                        End If
                    End Try
                    'We only get here if an exception is thrown and intRetryCt <= 3
                Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
            End With
        Catch ex As Exception
            LogError("PODownload Failure", "PODownload.clsBook.searchForExistingHeader: Order download check for existing POHeader record failure for order number " & PO.POOrderNumber & vbCrLf & ex.ToString, AdminEmail)
        Finally
            objTestRS = Nothing

        End Try
        Return blnRetVal
    End Function

    Public Function searchForExistingItem(ByRef PI As clsPOItem, ByRef blnInsertRecord As Boolean) As Boolean
        'test if the record already exists.
        Dim objTestRS As New ADODB.Recordset
        Dim blnRetVal As Boolean = False
        Dim strSQL As String = ""
        If PI Is Nothing Then Return False

        Try
            With objTestRS
                'reset the counter
                Dim intRetryCt As Integer = 0
                Do
                    intRetryCt += 1
                    Try
                        If Val(PI.ItemCustomer) < 1 Then
                            strSQL = "Select CreatedUser, CreatedDate From POItem Where ItemPONumber = " _
                                & PI.ItemPONumber & " And ItemNumber = " & PI.ItemNumber
                        Else
                            strSQL = "Select CreatedUser, CreatedDate From POItem Where ItemPONumber = " _
                                & PI.ItemPONumber & " And ItemNumber = " & PI.ItemNumber & " And CustomerNumber = " _
                                & Val(PI.ItemCustomer)
                        End If
                        .Open(strSQL, gDBCon, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)
                        If .RecordCount > 0 Then
                            'set the insert flag to false
                            blnInsertRecord = False
                        Else
                            'set the insert flat to true
                            blnInsertRecord = True
                        End If
                        .Close()
                        blnRetVal = True
                        Exit Do
                    Catch ex As Exception
                        If intRetryCt > Me.Retry Then
                            LogError("PODownload Failure", "PODownload.clsBook.searchForExistingItem: Check for existing POItem record failure for order item number " & PI.ItemNumber & vbCrLf & ex.ToString, AdminEmail)
                        Else
                            Log("searchForExistingItem Failure Retry = " & intRetryCt.ToString)

                        End If
                    End Try
                    'We only get here if an exception is thrown and intRetryCt <= 3
                Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
            End With
        Catch ex As Exception
            LogError("PODownload Failure", "PODownload.clsBook.searchForExistingItem: Check for existing POItem record failure for order item number " & PI.ItemNumber & vbCrLf & ex.ToString, AdminEmail)
        Finally
            objTestRS = Nothing
        End Try
        Return blnRetVal
    End Function

    Public Function updatePOHDRDefaults(ByRef PO As clsPOHeader) As Boolean
        Dim blnRetVal As Boolean = False

        If PO Is Nothing Then Return False

        Try
            Dim intRetryCt As Integer = 0


            Do
                intRetryCt += 1

                Try
                    Dim objCom As New ADODB.Command

                    With objCom
                        .ActiveConnection = gDBCon
                        .CommandTimeout = 3600
                        .Parameters.Append(.CreateParameter("@OrderNumber" _
                                                                , ADODB.DataTypeEnum.adVarChar _
                                                                , ADODB.ParameterDirectionEnum.adParamInput _
                                                                , 20 _
                                                                , stripQuotes(PO.POOrderNumber)))
                        .Parameters.Append(.CreateParameter("@CustomerNumber" _
                                                                , ADODB.DataTypeEnum.adBigInt _
                                                                , ADODB.ParameterDirectionEnum.adParamInput _
                                                                , _
                                                                , PO.PODefaultCustomer))
                        .Parameters.Append(.CreateParameter("@RetMsg" _
                                                                , ADODB.DataTypeEnum.adVarChar _
                                                                , ADODB.ParameterDirectionEnum.adParamOutput _
                                                                , 2500))
                        .Parameters.Append(.CreateParameter("@ErrNumber" _
                                                                , ADODB.DataTypeEnum.adBigInt _
                                                                , ADODB.ParameterDirectionEnum.adParamOutput))
                        .CommandText = "spUpdatePOHDRDefaults"
                        .CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
                    End With
                    objCom.Execute(, , ADODB.ExecuteOptionEnum.adAsyncExecute)
                    Do While (objCom.State And ADODB.ObjectStateEnum.adStateExecuting) = ADODB.ObjectStateEnum.adStateExecuting

                    Loop
                    Try

                        Dim strRetVal As String = Trim(objCom.Parameters("@RetMsg").Value)
                        Dim lngErrNumber As Long
                        If IsDBNull(objCom.Parameters("@ErrNumber").Value) Then
                            lngErrNumber = 0
                        Else
                            lngErrNumber = objCom.Parameters("@ErrNumber").Value
                        End If
                        If lngErrNumber <> 0 Then
                            If intRetryCt > Me.Retry Then
                                LogError("PODownload Failure", "PODownload.clsBook.updatePOHDRDefaults: Procedure spUpdatePOHDRDefaults output failure for order number " _
                                    & PO.POOrderNumber & vbCrLf & saveErrMsg(formatErrMsg(lngErrNumber _
                                    , strRetVal _
                                    , "spUpdatePOHDRDefaults" _
                                    & "; POHeader.clsBook.updatePOHDRDefaults")), AdminEmail)
                            Else
                                Log("updatePOHDRDefaults Output Failure Retry = " & intRetryCt.ToString)

                            End If
                        Else
                            blnRetVal = True
                            Exit Do
                        End If
                    Catch ex As Exception

                    End Try
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        LogError("PODownload Failure", "PODownload.clsBook.updatePOHDRDefaults: Procedure spUpdatePOHDRDefaults execution failure for order number " & PO.POOrderNumber & vbCrLf & ex.ToString, AdminEmail)
                    Else
                        Log("spUpdatePOHDRDefaults Execution Failure Retry = " & intRetryCt.ToString)

                    End If
                End Try
                'We only get here if an exception is thrown and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.

        Catch ex As Exception
            Try
                LogError("PODownload Failure", "PODownload.clsBook.updatePOHDRDefaults: Procedure spUpdatePOHDRDefaults general failure for order number " & PO.POOrderNumber & vbCrLf & ex.ToString, AdminEmail)
            Catch e As Exception
            End Try
        Finally

        End Try
        Return blnRetVal
    End Function

    Public Function updateHeaderRecord(ByRef PO As clsPOHeader) As Boolean
        Dim blnRetVal As Boolean = False
        If PO Is Nothing Then Return False

        Try
            Dim intRetryCt As Integer = 0
            Dim strSQL As String = "Update dbo.POHDR Set " _
                & " POHDRnumber = " & PO.POCustomerPO & ", POHDRPOdate = " & PO.POdate _
                & ", POHDRvendor = " & PO.POvendor & ", POHDRShipdate = " & PO.POShipdate _
                & ", POHDRBuyer = " & PO.POBuyer _
                & ", POHDRFrt = " & PO.POFrt _
                & ", POHDRCreateUser = '" & CreateUser & "', POHDRCreateDate = '" & CreatedDate & "'" _
                & ", POHDRTotalFrt = " & PO.POTotalFrt _
                & ", POHDRTotalCost = " & PO.POTotalCost _
                & ", POHDRWgt = " & PO.POWgt _
                & ", POHDRCube = " & PO.POCube _
                & ", POHDRQty = " & PO.POQty _
                & ", POHDRLines = " & PO.POLines _
                & ", POHDRConfirm = " & PO.POConfirm _
                & ", POHDRDefaultCarrier = " & PO.PODefaultCarrier _
                & ", POHDRReqDate = " & PO.POReqDate _
                & ", POHDRShipInstructions = " & PO.POShipInstructions _
                & ", POHDRCooler = " & PO.POCooler _
                & ", POHDRFrozen = " & PO.POFrozen _
                & ", POHDRDry = " & PO.PODry _
                & ", POHDRTemp = " & PO.POTemp _
                & ", POHDRCarType = " & PO.POCarType _
                & ", POHDRShipVia = " & PO.POShipVia _
                & ", POHDRShipViaType = " & PO.POShipViaType & ", POHDRPallets = " & PO.POPallets _
                & ", POHDROtherCost = " & PO.OtherCosts & ", POHDRStatusFlag = " & PO.StatusFlag & " "

            strSQL = strSQL & " From dbo.POHDR " _
                & " Where POHDRDefaultCustomer = '" _
                & PO.PODefaultCustomer & "' " _
                & " And POHDROrderNumber  = " & PO.POOrderNumber

            Do
                intRetryCt += 1
                Try
                    gDBCon.Execute(strSQL)
                    blnRetVal = True
                    Exit Do
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        LogError("PODownload Failure", "PODownload.clsBook.updateHeaderRecord: Update modified POHeader record failure for Order number " & PO.POOrderNumber & vbCrLf & ex.ToString & vbCrLf & strSQL, AdminEmail)
                    Else
                        Log("updateHeaderRecord Failure Retry = " & intRetryCt.ToString)

                    End If
                End Try
                'We only get here if an exception is thrown and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.

        Catch ex As Exception
            LogError("PODownload Failure", "PODownload.clsBook.updateHeaderRecord failure for order number " & PO.POOrderNumber & vbCrLf & ex.ToString, AdminEmail)
        Finally

        End Try
        Return blnRetVal
    End Function

    Public Function updateItemRecord(ByRef PI As clsPOItem) As Boolean
        Dim blnRetVal As Boolean = False
        If PI Is Nothing Then Return False
        Try
            Dim intRetryCt As Integer = 0
            Dim strSQL As String = "Update dbo.POItem Set " _
                            & " ItemPONumber = " & PI.ItemPONumber _
                            & ", FixOffInvAllow = " & PI.FixOffInvAllow _
                            & ", FixFrtAllow = " & PI.FixFrtAllow _
                            & ", ItemNumber = " & PI.ItemNumber _
                            & ", QtyOrdered = " & PI.QtyOrdered _
                            & ", FreightCost = " & PI.FreightCost _
                            & ", ItemCost = " & PI.ItemCost _
                            & ", Weight = " & PI.Weight _
                            & ", Cube = " & PI.Cube _
                            & ", Pack = " & PI.Pack _
                            & ", Size = " & PI.Size _
                            & ", Description = " & PI.Description _
                            & ", Hazmat = " & PI.Hazmat _
                            & ", Brand = " & PI.Brand _
                            & ", CostCenter = " & PI.CostCenter _
                            & ", LotNumber = " & PI.LotNumber _
                            & ", LotExpirationDate = " & PI.LotExpirationDate _
                            & ", GTIN = " & PI.GTIN _
                            & ", CustItemNumber = " & PI.CustItemNumber _
                            & ", CreatedUser = '" & CreateUser & "'" _
                            & ", CreatedDate = '" & CreatedDate & "'"
            If Val(PI.ItemCustomer) < 1 Then
                strSQL = strSQL & " From dbo.POItem " _
                    & " Where ItemPONumber = " & PI.ItemPONumber _
                    & " And ItemNumber = " & PI.ItemNumber
            Else
                strSQL = strSQL & " From dbo.POItem " _
                    & " Where ItemPONumber = " & PI.ItemPONumber _
                    & " And ItemNumber = " & PI.ItemNumber _
                    & " And CustomerNumber = " & Val(PI.ItemCustomer)
            End If

            Do
                intRetryCt += 1
                Try
                    gDBCon.Execute(strSQL)
                    blnRetVal = True
                    Exit Do
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        LogError("PODownload Failure", "PODownload.clsBook.updateItemRecord: Update modified POItem record failure for order item number " & PI.ItemNumber & vbCrLf & ex.ToString & vbCrLf & strSQL, AdminEmail)
                    Else
                        Log("updateItemRecord Failure Retry = " & intRetryCt.ToString)

                    End If
                End Try
                'We only get here if an exception is thrown and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.

        Catch ex As Exception
            LogError("PODownload Failure", "PODownload.clsBook.updateItemRecord for order item number " & PI.ItemNumber & vbCrLf & ex.ToString, AdminEmail)
        Finally

        End Try
        Return blnRetVal
    End Function

    Public Function insertHeaderRecord(ByRef PO As clsPOHeader) As Boolean
        Dim blnRetVal As Boolean = False
        If PO Is Nothing Then Return False
        Try
            Dim intRetryCt As Integer = 0
            Dim strSQL As String = "Insert Into dbo.POHDR" _
                & "(POHDROrderNumber" & ", POHDRnumber " _
                & ", POHDRvendor" _
                & ", POHDRPOdate" _
                & ", POHDRShipdate" _
                & ", POHDRBuyer" _
                & ", POHDRFrt" _
                & ", POHDRCreateUser, POHDRCreateDate" _
                & ", POHDRTotalFrt" _
                & ", POHDRTotalCost" _
                & ", POHDRWgt" _
                & ", POHDRCube" _
                & ", POHDRQty" _
                & ", POHDRLines" _
                & ", POHDRConfirm" _
                & ", POHDRDefaultCustomer , POHDRDefaultCarrier " _
                & ", POHDRReqDate" _
                & ", POHDRShipInstructions" _
                & ", POHDRCooler" _
                & ", POHDRFrozen" _
                & ", POHDRDry" _
                & ", POHDRTemp" _
                & ", POHDRCarType" _
                & ", POHDRShipVia" _
                & ", POHDRShipViaType, POHDRPallets,POHDROtherCost,POHDRStatusFlag)"

            strSQL = strSQL & " Values " _
                & "(" & PO.POOrderNumber & "," & PO.POCustomerPO _
                & "," & PO.POvendor _
                & "," & PO.POdate _
                & "," & PO.POShipdate _
                & "," & PO.POBuyer _
                & "," & PO.POFrt _
                & ",'" & CreateUser & "','" & CreatedDate & "'" _
                & "," & PO.POTotalFrt _
                & "," & PO.POTotalCost _
                & "," & PO.POWgt _
                & "," & PO.POCube _
                & "," & PO.POQty _
                & "," & PO.POLines _
                & "," & PO.POConfirm _
                & "," & PO.PODefaultCustomer & " , " & PO.PODefaultCarrier _
                & "," & PO.POReqDate _
                & "," & PO.POShipInstructions _
                & "," & PO.POCooler _
                & "," & PO.POFrozen _
                & "," & PO.PODry _
                & "," & PO.POTemp _
                & "," & PO.POCarType _
                & "," & PO.POShipVia _
                & "," & PO.POShipViaType & "," & PO.POPallets & "," & PO.OtherCosts & "," & PO.StatusFlag & " )"

            Do
                intRetryCt += 1
                Try
                    gDBCon.Execute(strSQL)
                    blnRetVal = True
                    Exit Do
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        LogError("PODownload Failure", "PODownload.clsBook.insertHeaderRecord: Order download insert new POHeader record failure for order number " & PO.POOrderNumber & vbCrLf & ex.ToString & vbCrLf & strSQL, AdminEmail)
                    Else
                        Log("insertHeaderRecord Failure Retry = " & intRetryCt.ToString)
                    End If
                End Try
                'We only get here if an exception is thrown and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.

        Catch ex As Exception
            LogError("PODownload Failure", "PODownload.clsBook.insertHeaderRecord for order number " & PO.POOrderNumber & vbCrLf & ex.ToString, AdminEmail)
        Finally

        End Try
        Return blnRetVal
    End Function

    Public Function insertItemRecord(ByRef PI As clsPOItem) As Boolean
        Dim blnRetVal As Boolean = False
        If PI Is Nothing Then Return False

        Try
            Dim intRetryCt As Integer = 0
            Dim strSQL As String = "Insert Into dbo.POItem " _
                            & "(ItemPONumber" _
                            & ", FixOffInvAllow" _
                            & ", FixFrtAllow" _
                            & ", ItemNumber" _
                            & ", QtyOrdered" _
                            & ", FreightCost" _
                            & ", ItemCost" _
                            & ", Weight" _
                            & ", Cube" _
                            & ", Pack" _
                            & ", Size" _
                            & ", Description" _
                            & ", Hazmat" _
                            & ", Brand" _
                            & ", CostCenter" _
                            & ", LotNumber" _
                            & ", LotExpirationDate" _
                            & ", GTIN" _
                            & ", CustItemNumber" _
                            & ", CreatedUser" _
                            & ", CreatedDate" _
                            & ", CustomerNumber)"

            strSQL = strSQL & " Values " _
                            & "(" & PI.ItemPONumber _
                            & "," & PI.FixOffInvAllow _
                            & "," & PI.FixFrtAllow _
                            & "," & PI.ItemNumber _
                            & "," & PI.QtyOrdered _
                            & "," & PI.FreightCost _
                            & "," & PI.ItemCost _
                            & "," & PI.Weight _
                            & "," & PI.Cube _
                            & "," & PI.Pack _
                            & "," & PI.Size _
                            & "," & PI.Description _
                            & "," & PI.Hazmat _
                            & "," & PI.Brand _
                            & "," & PI.CostCenter _
                            & "," & PI.LotNumber _
                            & "," & PI.LotExpirationDate _
                            & "," & PI.GTIN _
                            & "," & PI.CustItemNumber _
                            & ",'" & CreateUser & "'" _
                            & ",'" & CreatedDate & "'" _
                            & "," & PI.ItemCustomer & ")"

            Do
                intRetryCt += 1
                Try
                    gDBCon.Execute(strSQL)
                    blnRetVal = True
                    Exit Do
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        LogError("PODownload Failure", "PODownload.clsBook.insertItemRecord: Insert new POItem record failure for order item number " & PI.ItemNumber & vbCrLf & ex.ToString & vbCrLf & strSQL, AdminEmail)
                    Else
                        Log("insertItemRecord Failure Retry = " & intRetryCt.ToString)

                    End If
                End Try
                'We only get here if an exception is thrown and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.

        Catch ex As Exception
            LogError("PODownload Failure", "PODownload.clsBook.insertItemRecord for order item number " & PI.ItemNumber & vbCrLf & ex.ToString, AdminEmail)
        Finally

        End Try
        Return blnRetVal
    End Function

    Public Function processOrder(ByVal blnInsertRecord As Boolean, ByRef PO As clsPOHeader) As Boolean
        Dim blnRet As Boolean = False
        Try

            If blnInsertRecord Then
                blnRet = insertHeaderRecord(PO)
            Else
                blnRet = updateHeaderRecord(PO)
            End If
        Catch ex As Exception

        End Try
        Return blnRet

    End Function

    Public Function processItems(ByVal blnInsertRecord As Boolean, ByRef PI As clsPOItem) As Boolean
        Dim blnRet As Boolean = False
        Try

            If blnInsertRecord Then
                blnRet = insertItemRecord(PI)
            Else
                blnRet = updateItemRecord(PI)
            End If
        Catch ex As Exception

        End Try
        Return blnRet

    End Function

End Class

Public Class clsPOHeader
    'POHeader Variables
    Public POOrderNumber As String = ""
    Public POCustomerPO As String = ""
    Public POvendor As String = ""
    Public POdate As String = ""
    Public POShipdate As String = ""
    Public POBuyer As String = ""
    Public POFrt As String = ""
    Public POTotalFrt As String = ""
    Public POTotalCost As String = ""
    Public POWgt As String = ""
    Public POCube As String = ""
    Public POQty As String = ""
    Public POLines As String = ""
    Public POConfirm As String = ""
    Public PODefaultCustomer As String = ""
    Public POReqDate As String = ""
    Public POShipInstructions As String = ""
    Public POCooler As String = ""
    Public POFrozen As String = ""
    Public PODry As String = ""
    Public POTemp As String = ""
    Public POCarType As String = ""
    Public POShipVia As String = ""
    Public POShipViaType As String = ""
    Public POPallets As String = ""
    Public POConsigneeNumber As String = ""
    Public PODefaultCarrier As String = ""
    Public OtherCosts As String = ""
    Public StatusFlag As String = ""
    Public NewLaneNumber As String = ""



End Class

Public Class clsPOItem
    'POItem Variables
    Public ItemPONumber As String = ""
    Public FixOffInvAllow As String = ""
    Public FixFrtAllow As String = ""
    Public ItemNumber As String = ""
    Public QtyOrdered As String = ""
    Public FreightCost As String = ""
    Public ItemCost As String = ""
    Public Weight As String = ""
    Public Cube As String = ""
    Public Pack As String = ""
    Public Size As String = ""
    Public Description As String = ""
    Public Hazmat As String = ""
    Public Brand As String = ""
    Public CostCenter As String = ""
    Public LotNumber As String = ""
    Public LotExpirationDate As String = ""
    Public GTIN As String = ""
    Public CustItemNumber As String = ""
    Public ItemCustomer As String = ""
End Class

