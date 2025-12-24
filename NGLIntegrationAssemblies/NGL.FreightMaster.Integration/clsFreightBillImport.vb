Option Strict Off
Option Explicit On

Imports ngl.FreightMaster.Integration.Configuration
Imports System.Data.SqlClient

<Serializable()> _
Public Class clsFreightBillImport : Inherits clsDownload
    Public Function getDataSet() As FreightBillData
        Return New FreightBillData
    End Function

    Public Function ProcessObjectData( _
                        ByVal oFreightBills() As clsFreightBillObject, _
                        ByVal strConnection As String) As ProcessDataReturnValues
        Dim intRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationFailure
        Dim oHTable As New FreightBillData.FreightBillDataTable
        Dim dtVal As Date
        Try
            For Each oItem As clsFreightBillObject In oFreightBills
                Dim oRow As FreightBillData.FreightBillRow = oHTable.NewFreightBillRow
                With oRow
                    .APPONumber = Left(oItem.APPONumber, 20)
                    .APPRONumber = Left(oItem.APPRONumber, 20)
                    .APCNSNumber = Left(oItem.APCNSNumber, 20)
                    .APCarrierNumber = Left(oItem.APCarrierNumber, 20)
                    .APBillNumber = Left(oItem.APBillNumber, 50)

                    If validateDateWS(oItem.APBillDate, dtVal) Then
                        .APBillDate = exportDateToString(dtVal.ToString)
                    End If
                    .APCustomerID = Left(oItem.APCustomerID, 50)
                    .APCostCenterNumber = Left(oItem.APCostCenterNumber, 50)
                    .APTotalCost = oItem.APTotalCost
                    .APBLNumber = Left(oItem.APBLNumber, 20)
                    .APBilledWeight = oItem.APBilledWeight
                    .APTotalTax = oItem.APTotalTax
                    .APFee1 = oItem.APFee1
                    .APFee2 = oItem.APFee2
                    .APFee3 = oItem.APFee3
                    .APFee4 = oItem.APFee4
                    .APFee5 = oItem.APFee5
                    .APFee6 = oItem.APFee6
                    .APOtherCost = oItem.APOtherCost
                    .APCarrierCost = oItem.APCarrierCost
                    .APOrderSequence = oItem.APOrderSequence
                End With
                oHTable.AddFreightBillRow(oRow)
            Next

            intRet = ProcessData(oHTable, strConnection)
        Catch ex As Exception
            LogException("Process Object Data Failure", "Order import system error", AdminEmail, ex, "NGL.FreightMaster.Integration.clsFreightBillImport.ProcessObjectData Failure")
        End Try
        Return intRet


    End Function

    Public Function ProcessData( _
                ByVal oFreightBills As FreightBillData.FreightBillDataTable, _
                ByVal strConnection As String) As ProcessDataReturnValues

        Dim intRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strTitle As String = ""
        Dim strMsg As String = ""
        Dim strHeaderTable As String = "APMassEntry"
        Dim strItemTable As String = ""
        Me.HeaderName = "AP Mass Entry"
        Me.ItemName = ""
        Me.ImportTypeKey = IntegrationTypes.FreightBillImport
        Me.CreatedDate = Now.ToString
        Me.CreateUser = "Data Integration DLL"
        Me.Source = "Freight Bill Data Integration"
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
        'If Not buildHeaderCollection(oFields) Then Exit Function

        Try
            'Import the Header Records
            importHeaderRecords(oFreightBills)
            strTitle = "Process Data Complete"
            If GroupEmailMsg.Trim.Length > 0 Then
                LogError("Process Payables Data Warning", "The following errors or warnings were reported some freight bill records may not have been processed correctly." & GroupEmailMsg, GroupEmail)
            End If
            If ITEmailMsg.Trim.Length > 0 Then
                LogError("Process Payables Data Failure", "The following errors or warnings were reported some freight bill records may not have been processed correctly." & ITEmailMsg, AdminEmail)
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
            LogException("Process Payables Data Failure", "Could not process the requested payables data.", AdminEmail, ex, "NGL.FreightMaster.Integration.clsFreightBillImport.ProcessData")
        Finally
            closeConnection
        End Try
        Return intRet
    End Function

    Private Function importHeaderRecords( _
        ByRef oFreightBills As FreightBillData.FreightBillDataTable, _
        ByRef oFields As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Try

            'now get the FreightBill Header Records
            Dim intRetryCt As Integer = 0
            Dim strSource As String = "clsFreightBillImport.importHeaderRecords"
            Dim blnDataValidated As Boolean = False
            Dim strErrorMessage As String = ""
            Dim blnInsertRecord As Boolean = True

            Do
                intRetryCt += 1
                RecordErrors = 0
                TotalRecords = 0
                Try

                    Try
                        Dim lngMax As Long = oFreightBills.Count
                        Log("Importing " & lngMax & " FreightBill Header Records.")
                        For Each oRow As FreightBillData.FreightBillRow In oFreightBills
                            strErrorMessage = ""
                            blnDataValidated = validateFields(oFields, oRow, strErrorMessage, strSource)
                            If Not blnDataValidated Then
                                addToErrorTable(oFields, "[dbo].[FileImportErrorLog]", strErrorMessage, "Data Integration DLL", mstrHeaderName)
                                RecordErrors += 1
                            Else
                                'Save the changes to the database
                                If SaveData(oFields) Then
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
                        ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsFreightBillImport.importHeaderRecords, attempted to import payables header records from Data Integration DLL " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
                        Log("NGL.FreightMaster.Integration.clsFreightBillImport.importHeaderRecords Failed!" & readExceptionMessage(ex))
                    Else
                        Log("importHeaderRecords Failure Retry = " & intRetryCt.ToString)
                    End If
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.                
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsFreightBillImport.importHeaderRecords, Could not import from Data Integration DLL.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsFreightBillImport.importHeaderRecords Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function

    Private Function importHeaderRecords( ByRef oFreightBills As FreightBillData.FreightBillDataTable) As Boolean
        Dim Ret As Boolean = False
        Try

            'now get the FreightBill Header Records
            Dim intRetryCt As Integer = 0
            Dim strSource As String = "clsFreightBillImport.importHeaderRecords"
            Dim blnDataValidated As Boolean = False
            Dim strErrorMessage As String = ""
            Dim blnInsertRecord As Boolean = True

            Do
                intRetryCt += 1
                RecordErrors = 0
                TotalRecords = 0
                Try

                    Try
                        Dim lngMax As Long = oFreightBills.Count
                        Log("Importing " & lngMax & " FreightBill Header Records.")
                        For Each oRow As FreightBillData.FreightBillRow In oFreightBills
                            strErrorMessage = ""
                            'Save the changes to the database
                            If SaveData(oRow) Then
                                TotalRecords += 1
                            End If
                        Next
                        Return True
                    Catch ex As Exception
                        Throw
                    Finally

                    End Try
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsFreightBillImport.importHeaderRecords, attempted to import payables header records from Data Integration DLL " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
                        Log("NGL.FreightMaster.Integration.clsFreightBillImport.importHeaderRecords Failed!" & readExceptionMessage(ex))
                    Else
                        Log("importHeaderRecords Failure Retry = " & intRetryCt.ToString)
                    End If
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.                
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsFreightBillImport.importHeaderRecords, Could not import from Data Integration DLL.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsFreightBillImport.importHeaderRecords Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function

    Private Function buildHeaderCollection(ByRef oFields As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Try
            With oFields
                .Add("APPONumber", "APPONumber", clsImportField.DataTypeID.gcvdtString, 20, True)
                .Add("APPRONumber", "APPRONumber", clsImportField.DataTypeID.gcvdtString, 20, True)
                .Add("APCNSNumber", "APCNSNumber", clsImportField.DataTypeID.gcvdtString, 20, True)
                .Add("APCarrierNumber", "APCarrierNumber", clsImportField.DataTypeID.gcvdtLongInt, 11, False)
                .Add("APBillNumber", "APBillNumber", clsImportField.DataTypeID.gcvdtString, 50, False)
                .Add("APBillDate", "APBillDate", clsImportField.DataTypeID.gcvdtDate, 22, True)
                .Add("APCustomerID", "APCustomerID", clsImportField.DataTypeID.gcvdtString, 50, False)
                .Add("APCostCenterNumber", "APCostCenterNumber", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("APTotalCost", "APTotalCost", clsImportField.DataTypeID.gcvdtMoney, 21, True)
                .Add("APBLNumber", "APBLNumber", clsImportField.DataTypeID.gcvdtString, 20, True)
                .Add("APBilledWeight", "APBilledWeight", clsImportField.DataTypeID.gcvdtLongInt, 11, True)
                .Add("APTotalTax", "APTotalTax", clsImportField.DataTypeID.gcvdtMoney, 21, True)
                .Add("APFee1", "APFee1", clsImportField.DataTypeID.gcvdtMoney, 21, True)
                .Add("APFee2", "APFee2", clsImportField.DataTypeID.gcvdtMoney, 21, True)
                .Add("APFee3", "APFee3", clsImportField.DataTypeID.gcvdtMoney, 21, True)
                .Add("APFee4", "APFee4", clsImportField.DataTypeID.gcvdtMoney, 21, True)
                .Add("APFee5", "APFee5", clsImportField.DataTypeID.gcvdtMoney, 21, True)
                .Add("APFee6", "APFee6", clsImportField.DataTypeID.gcvdtMoney, 21, True)
                .Add("APOtherCost", "APOtherCost", clsImportField.DataTypeID.gcvdtMoney, 21, True)
                .Add("APCarrierCost", "APCarrierCost", clsImportField.DataTypeID.gcvdtMoney, 21, True)
                .Add("APOrderSequence", "APOrderSequence", clsImportField.DataTypeID.gcvdtLongInt, 11, False)
            End With
            Log("Header Field Array Loaded.")
            'get the import field flag values
            For ct As Integer = 1 To oFields.Count
                Dim blnUseField As Boolean = True
                Try
                    If oFields(ct).Name = "APCarrierNumber" Or oFields(ct).Name = "APBillNumber" Then
                        blnUseField = True
                    Else
                        blnUseField = getImportFieldFlag(oFields(ct).Name, IntegrationTypes.FreightBillImport)
                    End If

                Catch ex As Exception
                    'throw away any errors in case one or more field names is missing in the Import Field Flag Table
                End Try
                oFields(ct).Use = blnUseField
            Next
            Ret = True
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsFreightBillImport.buildHeaderCollection, could not build the header collection data.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsFreightBillImport.buildHeaderCollection Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function

    Public Overloads Function SaveData(ByRef oFields As clsImportFields) As Boolean
        Dim blnRet As Boolean = False
        Dim strRetMsg As String = ""
        Dim intErrNumber As Integer = 0
        Try
            Dim intRetryCt As Integer = 0
            Do
                intRetryCt += 1
                Dim cmd As New SqlCommand
                Dim intRecCt As Integer = 0
                Try
                    'check the active db connection
                    If Me.openConnection Then
                        cmd = New SqlClient.SqlCommand()
                        With cmd
                            .Connection = Me.DBCon
                            .CommandTimeout = 600
                            .Parameters.Add("@APPONumber", SqlDbType.NVarChar, 20)
                            If Not oFields("APPONumber").Value.ToUpper = "NULL" Then .Parameters("@APPONumber").Value = Left(oFields("APPONumber").Value, 20)
                            .Parameters.Add("@APPRONumber", SqlDbType.NVarChar, 20)
                            If Not oFields("APPRONumber").Value.ToUpper = "NULL" Then .Parameters("@APPRONumber").Value = Left(oFields("APPRONumber").Value, 20)
                            .Parameters.Add("@APCNSNumber", SqlDbType.NVarChar, 20)
                            If Not oFields("APCNSNumber").Value.ToUpper = "NULL" Then .Parameters("@APCNSNumber").Value = Left(oFields("APCNSNumber").Value, 20)
                            .Parameters.Add("@APCarrierNumber", SqlDbType.Int)
                            If Not oFields("APCarrierNumber").Value.ToUpper = "NULL" Then .Parameters("@APCarrierNumber").Value = oFields("APCarrierNumber").Value
                            .Parameters.Add("@APBillNumber", SqlDbType.NVarChar, 50)
                            If Not oFields("APBillNumber").Value.ToUpper = "NULL" Then .Parameters("@APBillNumber").Value = Left(oFields("APBillNumber").Value, 50)
                            .Parameters.Add("@APBillDate", SqlDbType.DateTime)
                            If Not oFields("APBillDate").Value.ToUpper = "NULL" Then .Parameters("@APBillDate").Value = CDate(Ngl.Core.Utility.DataTransformation.stripQuotes(oFields("APBillDate").Value))
                            .Parameters.Add("@APCustomerID", SqlDbType.NVarChar, 50)
                            If Not oFields("APCustomerID").Value.ToUpper = "NULL" Then .Parameters("@APCustomerID").Value = Left(oFields("APCustomerID").Value, 50)
                            .Parameters.Add("@APCostCenterNumber", SqlDbType.NVarChar, 50)
                            If Not oFields("APCostCenterNumber").Value.ToUpper = "NULL" Then .Parameters("@APCostCenterNumber").Value = Left(oFields("APCostCenterNumber").Value, 50)
                            .Parameters.Add("@APTotalCost", SqlDbType.Money)
                            If Not oFields("APTotalCost").Value.ToUpper = "NULL" Then .Parameters("@APTotalCost").Value = oFields("APTotalCost").Value
                            .Parameters.Add("@APBLNumber", SqlDbType.NVarChar, 20)
                            If Not oFields("APBLNumber").Value.ToUpper = "NULL" Then .Parameters("@APBLNumber").Value = Left(oFields("APBLNumber").Value, 20)
                            .Parameters.Add("@APBilledWeight", SqlDbType.Int)
                            If Not oFields("APBilledWeight").Value.ToUpper = "NULL" Then .Parameters("@APBilledWeight").Value = oFields("APBilledWeight").Value
                            .Parameters.Add("@APReceivedDate", SqlDbType.DateTime)
                            .Parameters("@APReceivedDate").Value = DateTime.Now
                            .Parameters.Add("@APPayCode", SqlDbType.Money)
                            .Parameters("@APPayCode").Value = "N"
                            .Parameters.Add("@APElectronicFlag", SqlDbType.Int)
                            .Parameters("@APElectronicFlag").Value = 1
                            .Parameters.Add("@APTotalTax", SqlDbType.Money)
                            If Not oFields("APTotalTax").Value.ToUpper = "NULL" Then .Parameters("@APTotalTax").Value = oFields("APTotalTax").Value
                            .Parameters.Add("@APFee1", SqlDbType.Money)
                            If Not oFields("APFee1").Value.ToUpper = "NULL" Then .Parameters("@APFee1").Value = oFields("APFee1").Value
                            .Parameters.Add("@APFee2", SqlDbType.Money)
                            If Not oFields("APFee2").Value.ToUpper = "NULL" Then .Parameters("@APFee2").Value = oFields("APFee2").Value
                            .Parameters.Add("@APFee3", SqlDbType.Money)
                            If Not oFields("APFee3").Value.ToUpper = "NULL" Then .Parameters("@APFee3").Value = oFields("APFee3").Value
                            .Parameters.Add("@APFee4", SqlDbType.Money)
                            If Not oFields("APFee4").Value.ToUpper = "NULL" Then .Parameters("@APFee4").Value = oFields("APFee4").Value
                            .Parameters.Add("@APFee5", SqlDbType.Money)
                            If Not oFields("APFee5").Value.ToUpper = "NULL" Then .Parameters("@APFee5").Value = oFields("APFee5").Value
                            .Parameters.Add("@APFee6", SqlDbType.Money)
                            If Not oFields("APFee6").Value.ToUpper = "NULL" Then .Parameters("@APFee6").Value = oFields("APFee6").Value
                            .Parameters.Add("@APOtherCost", SqlDbType.Money)
                            If Not oFields("APOtherCost").Value.ToUpper = "NULL" Then .Parameters("@APOtherCost").Value = oFields("APOtherCost").Value
                            .Parameters.Add("@APCarrierCost", SqlDbType.Money)
                            If Not oFields("APCarrierCost").Value.ToUpper = "NULL" Then .Parameters("@APCarrierCost").Value = oFields("APCarrierCost").Value
                            .Parameters.Add("@APOverwrite", SqlDbType.Bit)
                            .Parameters("@APOverwrite").Value = 0
                            .Parameters.Add("@APOrderSequence", SqlDbType.Int)
                            .Parameters("@APOrderSequence").Value = oFields("APOrderSequence").Value
                            .Parameters.Add("@RetMsg", SqlDbType.NVarChar, 1000)
                            .Parameters("@RetMsg").Direction = ParameterDirection.Output
                            .Parameters.Add("@ErrNumber", SqlDbType.Int)
                            .Parameters("@ErrNumber").Direction = ParameterDirection.Output
                            .CommandText = "dbo.spInsertFreightBillUnique"
                            .CommandType = CommandType.StoredProcedure
                            .ExecuteNonQuery()
                            strRetMsg = Trim(.Parameters("@RetMsg").Value.ToString)
                            If IsDBNull(.Parameters("@ErrNumber").Value) Then
                                intErrNumber = 0
                            Else
                                intErrNumber = .Parameters("@ErrNumber").Value
                            End If
                        End With
                        Try
                            If intErrNumber <> 0 Then
                                If intRetryCt > Me.Retry Then
                                    Me.RecordErrors += 1
                                    ITEmailMsg &= "<br />" & Source & " Failure: clsFreightBillImport.saveData: Procedure spInsertFreightBillUnique output failure: " _
                                        & vbCrLf & "Error # " & intErrNumber & ": " & strRetMsg & ".  Could not import freight bill record.<br />" & vbCrLf
                                    Log("saveData Failed!")
                                    Exit Do
                                Else
                                    Log("NGL.FreightMaster.Integration.clsFreightBillImport.saveData Output Failure Retry = " & intRetryCt.ToString)
                                End If
                            Else
                                'return true to the calling procedrue
                                blnRet = True
                                Exit Do
                            End If
                        Catch ex As Exception
                            Log("NGL.FreightMaster.Integration.clsFreightBillImport.saveData Unexpected Error " & readExceptionMessage(ex) & vbCrLf & " Retry = " & intRetryCt.ToString)
                        End Try
                    Else
                        If intRetryCt > Me.Retry Then
                            Me.RecordErrors += 1
                            ITEmailMsg &= "<br />" & Source & " Failure: clsFreightBillImport.saveData: Open database connection failure, attempted to create a database connection " & intRetryCt.ToString & " times without success.  Could not import freight bill record.<br />" & vbCrLf
                            Log("saveData Failed!")
                            Exit Do
                        Else
                            Log("NGL.FreightMaster.Integration.clsFreightBillImport.saveData Open DB Connection Failure Retry = " & intRetryCt.ToString)
                        End If

                    End If
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        ITEmailMsg &= "<br />" & Source & " Failure: clsFreightBillImport.saveData unexpected error. Could not import freight bill record:<br />" & vbCrLf & readExceptionMessage(ex) & vbCrLf & " Retry = " & intRetryCt.ToString & "<br />" & vbCrLf
                        Log("saveData Failed!" & readExceptionMessage(ex))
                    Else
                        Log("NGL.FreightMaster.Integration.clsFreightBillImport.saveData Unexpected Error " & readExceptionMessage(ex) & vbCrLf & " Retry = " & intRetryCt.ToString)
                    End If
                Finally
                    Try
                        cmd.Cancel()
                        cmd = Nothing
                    Catch ex As Exception

                    End Try
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: clsFreightBillImport.saveData: Could not import freight bill record.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("saveData Failed!" & readExceptionMessage(ex))
        End Try
        Return blnRet

    End Function

    Public Overloads Function SaveData(ByRef oRow As System.Data.DataRow) As Boolean
        Dim blnRet As Boolean = False
        Dim strRetMsg As String = ""
        Dim intErrNumber As Integer = 0
        Try
            Dim intRetryCt As Integer = 0
            Do
                intRetryCt += 1
                Dim cmd As New SqlCommand
                Dim intRecCt As Integer = 0
                Dim oRet As Object = Nothing
                Try
                    'check the active db connection
                    If Me.openConnection Then
                        cmd = New SqlClient.SqlCommand()
                        With cmd
                            .Connection = Me.DBCon
                            .CommandTimeout = 600
                            Ngl.Core.Utility.DataTransformation.addParameterFromDataRow(cmd, "@APPONumber", SqlDbType.NVarChar, oRow, "APPONumber", 20)
                            Ngl.Core.Utility.DataTransformation.addParameterFromDataRow(cmd, "@APPRONumber", SqlDbType.NVarChar, oRow, "APPRONumber", 20)
                            Ngl.Core.Utility.DataTransformation.addParameterFromDataRow(cmd, "@APCNSNumber", SqlDbType.NVarChar, oRow, "APCNSNumber", 20)
                            Ngl.Core.Utility.DataTransformation.addParameterFromDataRow(cmd, "@APSHID", SqlDbType.NVarChar, oRow, "APSHID", 50)
                            Ngl.Core.Utility.DataTransformation.addParameterFromDataRow(cmd, "@APCarrierNumber", SqlDbType.Int, oRow, "APCarrierNumber", 0, 0)
                            Ngl.Core.Utility.DataTransformation.addParameterFromDataRow(cmd, "@APBillNumber", SqlDbType.NVarChar, oRow, "APBillNumber", 50)
                            Ngl.Core.Utility.DataTransformation.addParameterFromDataRow(cmd, "@APBillDate", SqlDbType.DateTime, oRow, "APBillDate", 0, DateTime.Now)
                            Ngl.Core.Utility.DataTransformation.addParameterFromDataRow(cmd, "@APCustomerID", SqlDbType.NVarChar, oRow, "APCustomerID", 50)
                            Ngl.Core.Utility.DataTransformation.addParameterFromDataRow(cmd, "@APCostCenterNumber", SqlDbType.NVarChar, oRow, "APCostCenterNumber", 50)
                            Ngl.Core.Utility.DataTransformation.addParameterFromDataRow(cmd, "@APTotalCost", SqlDbType.Money, oRow, "APTotalCost", 0, 0)
                            Ngl.Core.Utility.DataTransformation.addParameterFromDataRow(cmd, "@APBLNumber", SqlDbType.NVarChar, oRow, "APBLNumber", 20)
                            Ngl.Core.Utility.DataTransformation.addParameterFromDataRow(cmd, "@APBilledWeight", SqlDbType.Int, oRow, "APBilledWeight", 0, 0)
                            .Parameters.Add("@APReceivedDate", SqlDbType.DateTime)
                            .Parameters("@APReceivedDate").Value = DateTime.Now
                            .Parameters.Add("@APPayCode", SqlDbType.NVarChar)
                            .Parameters("@APPayCode").Value = "N"
                            .Parameters.Add("@APElectronicFlag", SqlDbType.Int)
                            .Parameters("@APElectronicFlag").Value = 1
                            Ngl.Core.Utility.DataTransformation.addParameterFromDataRow(cmd, "@APTotalTax", SqlDbType.Money, oRow, "APTotalTax", 0, 0)
                            Ngl.Core.Utility.DataTransformation.addParameterFromDataRow(cmd, "@APFee1", SqlDbType.Money, oRow, "APFee1", 0, 0)
                            Ngl.Core.Utility.DataTransformation.addParameterFromDataRow(cmd, "@APFee2", SqlDbType.Money, oRow, "APFee2", 0, 0)
                            Ngl.Core.Utility.DataTransformation.addParameterFromDataRow(cmd, "@APFee3", SqlDbType.Money, oRow, "APFee3", 0, 0)
                            Ngl.Core.Utility.DataTransformation.addParameterFromDataRow(cmd, "@APFee4", SqlDbType.Money, oRow, "APFee4", 0, 0)
                            Ngl.Core.Utility.DataTransformation.addParameterFromDataRow(cmd, "@APFee5", SqlDbType.Money, oRow, "APFee5", 0, 0)
                            Ngl.Core.Utility.DataTransformation.addParameterFromDataRow(cmd, "@APFee6", SqlDbType.Money, oRow, "APFee6", 0, 0)
                            Ngl.Core.Utility.DataTransformation.addParameterFromDataRow(cmd, "@APOtherCosts", SqlDbType.Money, oRow, "APOtherCost", 0, 0)
                            Ngl.Core.Utility.DataTransformation.addParameterFromDataRow(cmd, "@APCarrierCost", SqlDbType.Money, oRow, "APCarrierCost", 0, 0)
                            .Parameters.Add("@APOverwrite", SqlDbType.Bit)
                            .Parameters("@APOverwrite").Value = 0
                            Ngl.Core.Utility.DataTransformation.addParameterFromDataRow(cmd, "@APOrderSequence", SqlDbType.Int, oRow, "APOrderSequence", 0, 0)
                            .Parameters.Add("@RetMsg", SqlDbType.NVarChar, 1000)
                            .Parameters("@RetMsg").Direction = ParameterDirection.Output
                            .Parameters.Add("@ErrNumber", SqlDbType.Int)
                            .Parameters("@ErrNumber").Direction = ParameterDirection.Output
                            .CommandText = "dbo.spInsertFreightBillUnique"
                            .CommandType = CommandType.StoredProcedure
                            .ExecuteNonQuery()
                            strRetMsg = Trim(.Parameters("@RetMsg").Value.ToString)
                            If IsDBNull(.Parameters("@ErrNumber").Value) Then
                                intErrNumber = 0
                            Else
                                intErrNumber = .Parameters("@ErrNumber").Value
                            End If
                        End With
                        Try
                            If intErrNumber <> 0 Then
                                If intRetryCt > Me.Retry Then
                                    Me.RecordErrors += 1
                                    ITEmailMsg &= "<br />" & Source & " Failure: clsFreightBillImport.saveData: Procedure spInsertFreightBillUnique output failure: " _
                                        & vbCrLf & "Error # " & intErrNumber & ": " & strRetMsg & ".  Could not import freight bill record.<br />" & vbCrLf
                                    Log("saveData Failed!")
                                    Exit Do
                                Else
                                    Log("NGL.FreightMaster.Integration.clsFreightBillImport.saveData Output Failure Retry = " & intRetryCt.ToString)
                                End If
                            Else
                                'return true to the calling procedrue
                                blnRet = True
                                Exit Do
                            End If
                        Catch ex As Exception
                            Log("NGL.FreightMaster.Integration.clsFreightBillImport.saveData Unexpected Error " & readExceptionMessage(ex) & vbCrLf & " Retry = " & intRetryCt.ToString)
                        End Try
                    Else
                        If intRetryCt > Me.Retry Then
                            Me.RecordErrors += 1
                            ITEmailMsg &= "<br />" & Source & " Failure: clsFreightBillImport.saveData: Open database connection failure, attempted to create a database connection " & intRetryCt.ToString & " times without success.  Could not import freight bill record.<br />" & vbCrLf
                            Log("saveData Failed!")
                            Exit Do
                        Else
                            Log("NGL.FreightMaster.Integration.clsFreightBillImport.saveData Open DB Connection Failure Retry = " & intRetryCt.ToString)
                        End If

                    End If
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        ITEmailMsg &= "<br />" & Source & " Failure: clsFreightBillImport.saveData unexpected error. Could not import freight bill record:<br />" & vbCrLf & readExceptionMessage(ex) & vbCrLf & " Retry = " & intRetryCt.ToString & "<br />" & vbCrLf
                        Log("saveData Failed!" & readExceptionMessage(ex))
                    Else
                        Log("NGL.FreightMaster.Integration.clsFreightBillImport.saveData Unexpected Error " & readExceptionMessage(ex) & vbCrLf & " Retry = " & intRetryCt.ToString)
                    End If
                Finally
                    Try
                        cmd.Cancel()
                        cmd = Nothing
                    Catch ex As Exception

                    End Try
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: clsFreightBillImport.saveData: Could not import freight bill record.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("saveData Failed!" & readExceptionMessage(ex))
        End Try
        Return blnRet

    End Function

End Class
