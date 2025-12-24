Option Strict Off
Option Explicit On

Imports ngl.FreightMaster.Integration.Configuration
Imports Ngl.FreightMaster.Integration.FMDataTableAdapters
Imports System.Data.SqlClient

<Serializable()> _
Public Class clsAPFreightBillExport : Inherits clsUpload
#Region " Class Variables and Properties "


#End Region

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

#Region " Functions "

    Protected Function GetOpenPayableData(ByRef oAPFBT As APFreightBill.APFreightBillDataDataTable, _
            ByVal CompNumber As Nullable(Of Integer)) As Boolean
        Dim blnRet As Boolean = False
        Dim cmdObj As New System.Data.SqlClient.SqlCommand
        Dim strRet As String = ""
        Dim strSQL As String
        Dim drTemp As SqlDataReader
        Try
            If Not openConnection() Then
                Return False
            End If

            With cmdObj
                .Connection = DBCon
                .CommandTimeout = Me.CommandTimeOut
            End With

            strSQL = "SELECT "
            If Me.MaxRowsReturned > 0 Then
                strSQL &= " TOP " & Me.MaxRowsReturned.ToString & " "
            End If
            strSQL &= "APCarrierNumber, APBillNumber, APBillDate, APPONumber, APCustomerID, APCostCenterNumber, APTotalCost, APPRONumber, APBLNumber, APBilledWeight, APCNSNumber, APReceivedDate, APTotalTax, APFee1, APFee2, APFee3, APFee4, APFee5, APFee6, APOtherCosts, APCarrierCost, GLTotalCost, GLTotalTax, GLFee1, GLFee2, GLFee3, GLFee4, GLFee5, GLFee6, GLOtherCosts, GLCarrierCost, BookFinAPExportRetry, BookFinAPExportDate, PrevSentDate, CompanyNumber FROM dbo.udfGetOpenAPFreightBillRecordsWS("

            If CompNumber.HasValue AndAlso CompNumber.Value > 0 Then
                strSQL &= CompNumber.Value.ToString
            Else
                strSQL &= "NULL"
            End If
            
            strSQL &= ")"
            If Debug Then
                Log("Select Open AP Freight Bill  Records: " & strSQL)
            End If
            With cmdObj
                .CommandText = strSQL
                .CommandType = CommandType.Text
                drTemp = .ExecuteReader()
            End With
            If drTemp.HasRows Then

                If oAPFBT Is Nothing Then
                    oAPFBT = New APFreightBill.APFreightBillDataDataTable
                End If
                Dim intRecCt As Integer = 0
                Do While drTemp.Read()
                    intRecCt += 1
                    Log("Reading Open AP Freight Bill Record " & intRecCt.ToString)
                    Dim strFreightBillNumber As String = ""
                    Try
                        Dim intRetryNbr As Integer = getSQLDataReaderValue(drTemp, "BookFinAPExportRetry")
                        strFreightBillNumber = getSQLDataReaderValue(drTemp, "APBillNumber")
                        Dim oAPFBR As APFreightBill.APFreightBillDataRow = oAPFBT.NewAPFreightBillDataRow
                        With oAPFBR
                            .APCarrierNumber = getSQLDataReaderValue(drTemp, "APCarrierNumber")
                            .APBillNumber = strFreightBillNumber
                            .APBillDate = getSQLDataReaderValue(drTemp, "APBillDate")
                            .APPONumber = getSQLDataReaderValue(drTemp, "APPONumber")
                            .APCustomerID = getSQLDataReaderValue(drTemp, "APCustomerID")
                            .APCostCenterNumber = getSQLDataReaderValue(drTemp, "APCostCenterNumber")
                            .APTotalCost = getSQLDataReaderValue(drTemp, "APTotalCost")
                            .APPRONumber = getSQLDataReaderValue(drTemp, "APPRONumber")
                            .APBLNumber = getSQLDataReaderValue(drTemp, "APBLNumber")
                            .APBilledWeight = getSQLDataReaderValue(drTemp, "APBilledWeight")
                            .APCNSNumber = getSQLDataReaderValue(drTemp, "APCNSNumber")
                            .APReceivedDate = getSQLDataReaderValue(drTemp, "APReceivedDate")
                            .APTotalTax = getSQLDataReaderValue(drTemp, "APTotalTax")
                            .APFee1 = getSQLDataReaderValue(drTemp, "APFee1")
                            .APFee2 = getSQLDataReaderValue(drTemp, "APFee2")
                            .APFee3 = getSQLDataReaderValue(drTemp, "APFee3")
                            .APFee4 = getSQLDataReaderValue(drTemp, "APFee4")
                            .APFee5 = getSQLDataReaderValue(drTemp, "APFee5")
                            .APFee6 = getSQLDataReaderValue(drTemp, "APFee6")
                            .APOtherCosts = getSQLDataReaderValue(drTemp, "APOtherCosts")
                            .APCarrierCost = getSQLDataReaderValue(drTemp, "APCarrierCost")
                            .GLTotalCost = getSQLDataReaderValue(drTemp, "GLTotalCost")
                            .GLTotalTax = getSQLDataReaderValue(drTemp, "GLTotalTax")
                            .GLFee1 = getSQLDataReaderValue(drTemp, "GLFee1")
                            .GLFee2 = getSQLDataReaderValue(drTemp, "GLFee2")
                            .GLFee3 = getSQLDataReaderValue(drTemp, "GLFee3")
                            .GLFee4 = getSQLDataReaderValue(drTemp, "GLFee4")
                            .GLFee5 = getSQLDataReaderValue(drTemp, "GLFee5")
                            .GLFee6 = getSQLDataReaderValue(drTemp, "GLFee6")
                            .GLOtherCosts = getSQLDataReaderValue(drTemp, "GLOtherCosts")
                            .GLCarrierCost = getSQLDataReaderValue(drTemp, "GLCarrierCost")
                            .BookFinAPExportRetry = intRetryNbr
                            .BookFinAPExportDate = getSQLDataReaderValue(drTemp, "BookFinAPExportDate")
                            .PrevSentDate = getSQLDataReaderValue(drTemp, "PrevSentDate")
                            .CompanyNumber = getSQLDataReaderValue(drTemp, "CompanyNumber")
                        End With
                        oAPFBT.AddAPFreightBillDataRow(oAPFBR)
                        oAPFBT.AcceptChanges()
                        Me.TotalRecords += 1
                    Catch ex As Exception
                        RecordErrors += 1
                        ITEmailMsg &= "<br />Read Open AP freight bill record error<br /> " & ex.ToString & "<br />" & vbCrLf
                    End Try
                Loop
            End If
            blnRet = True
        Catch ex As System.ApplicationException
            Throw
        Catch ex As Exception
            RecordErrors += 1
            Throw New System.ApplicationException(ex.ToString, ex.InnerException)
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
        Return blnRet

    End Function

    Protected Function GetData(ByRef oAPFBT As APFreightBill.APFreightBillDataDataTable, _
            ByVal MaxRetryNbr As Nullable(Of Integer), _
            ByVal RetryMinutes As Nullable(Of Integer), _
            ByVal CompNumber As Nullable(Of Integer), _
            ByVal FreightBillNumber As String) As Boolean
        Dim blnRet As Boolean = False
        Dim cmdObj As New System.Data.SqlClient.SqlCommand
        Dim strRet As String = ""
        Dim strSQL As String
        Dim drTemp As SqlDataReader
        Try
            If Not openConnection() Then
                Return False
            End If

            With cmdObj
                .Connection = DBCon
                .CommandTimeout = Me.CommandTimeOut
            End With

            strSQL = "SELECT "
            If Me.MaxRowsReturned > 0 Then
                strSQL &= " TOP " & Me.MaxRowsReturned.ToString & " "
            End If
            strSQL &= "APCarrierNumber, APBillNumber, APBillDate, APPONumber, APCustomerID, APCostCenterNumber, APTotalCost, APPRONumber, APBLNumber, APBilledWeight, APCNSNumber, APReceivedDate, APTotalTax, APFee1, APFee2, APFee3, APFee4, APFee5, APFee6, APOtherCosts, APCarrierCost, GLTotalCost, GLTotalTax, GLFee1, GLFee2, GLFee3, GLFee4, GLFee5, GLFee6, GLOtherCosts, GLCarrierCost, BookFinAPExportRetry, BookFinAPExportDate, PrevSentDate, CompanyNumber, APTaxDetail1, APTaxDetail2, APTaxDetail3, APTaxDetail4, APTaxDetail5, GLAPTaxDetail1, GLAPTaxDetail2, GLAPTaxDetail3, GLAPTaxDetail4, GLAPTaxDetail5 FROM dbo.udfGetAPFreightBillRecordsWS(NULL,"

            If RetryMinutes.HasValue AndAlso RetryMinutes.Value > 0 Then
                strSQL &= RetryMinutes.Value.ToString
            Else
                strSQL &= "NULL"
            End If
            strSQL &= ","
            If CompNumber.HasValue AndAlso CompNumber.Value > 0 Then
                strSQL &= CompNumber.Value.ToString
            Else
                strSQL &= "NULL"
            End If
            strSQL &= ","
            If Not FreightBillNumber Is Nothing AndAlso FreightBillNumber.Trim.Length > 0 Then
                strSQL &= "'" & FreightBillNumber & "'"
            Else
                strSQL &= "NULL"
            End If
            strSQL &= ",0)"
            If Debug Then
                Log("Select AP Freight Bill  Records: " & strSQL)
            End If
            With cmdObj
                .CommandText = strSQL
                .CommandType = CommandType.Text
                drTemp = .ExecuteReader()
            End With
            If drTemp.HasRows Then

                If oAPFBT Is Nothing Then
                    oAPFBT = New APFreightBill.APFreightBillDataDataTable
                End If
                Dim intRecCt As Integer = 0
                Do While drTemp.Read()
                    intRecCt += 1
                    Log("Reading AP Freight Bill Record " & intRecCt.ToString)
                    Dim strFreightBillNumber As String = ""
                    Try
                        Dim intRetryNbr As Integer = getSQLDataReaderValue(drTemp, "BookFinAPExportRetry")
                        strFreightBillNumber = getSQLDataReaderValue(drTemp, "APBillNumber")
                        If MaxRetryNbr.HasValue AndAlso intRetryNbr > MaxRetryNbr.Value Then
                            'we do not export this record we add this record to the group email error message.
                            GroupEmailMsg &= "<br />" & strFreightBillNumber & vbCrLf
                            Me.UpdateStatus(Nothing, Nothing, strFreightBillNumber, Nothing, Nothing, intRetryNbr, Now)
                        Else
                            Dim oAPFBR As APFreightBill.APFreightBillDataRow = oAPFBT.NewAPFreightBillDataRow
                            With oAPFBR
                                .APCarrierNumber = getSQLDataReaderValue(drTemp, "APCarrierNumber")
                                .APBillNumber = strFreightBillNumber
                                .APBillDate = getSQLDataReaderValue(drTemp, "APBillDate")
                                .APPONumber = getSQLDataReaderValue(drTemp, "APPONumber")
                                .APCustomerID = getSQLDataReaderValue(drTemp, "APCustomerID")
                                .APCostCenterNumber = getSQLDataReaderValue(drTemp, "APCostCenterNumber")
                                .APTotalCost = getSQLDataReaderValue(drTemp, "APTotalCost")
                                .APPRONumber = getSQLDataReaderValue(drTemp, "APPRONumber")
                                .APBLNumber = getSQLDataReaderValue(drTemp, "APBLNumber")
                                .APBilledWeight = getSQLDataReaderValue(drTemp, "APBilledWeight")
                                .APCNSNumber = getSQLDataReaderValue(drTemp, "APCNSNumber")
                                .APReceivedDate = getSQLDataReaderValue(drTemp, "APReceivedDate")
                                .APTotalTax = getSQLDataReaderValue(drTemp, "APTotalTax")
                                .APFee1 = getSQLDataReaderValue(drTemp, "APFee1")
                                .APFee2 = getSQLDataReaderValue(drTemp, "APFee2")
                                .APFee3 = getSQLDataReaderValue(drTemp, "APFee3")
                                .APFee4 = getSQLDataReaderValue(drTemp, "APFee4")
                                .APFee5 = getSQLDataReaderValue(drTemp, "APFee5")
                                .APFee6 = getSQLDataReaderValue(drTemp, "APFee6")
                                .APOtherCosts = getSQLDataReaderValue(drTemp, "APOtherCosts")
                                .APCarrierCost = getSQLDataReaderValue(drTemp, "APCarrierCost")
                                .GLTotalCost = getSQLDataReaderValue(drTemp, "GLTotalCost")
                                .GLTotalTax = getSQLDataReaderValue(drTemp, "GLTotalTax")
                                .GLFee1 = getSQLDataReaderValue(drTemp, "GLFee1")
                                .GLFee2 = getSQLDataReaderValue(drTemp, "GLFee2")
                                .GLFee3 = getSQLDataReaderValue(drTemp, "GLFee3")
                                .GLFee4 = getSQLDataReaderValue(drTemp, "GLFee4")
                                .GLFee5 = getSQLDataReaderValue(drTemp, "GLFee5")
                                .GLFee6 = getSQLDataReaderValue(drTemp, "GLFee6")
                                .GLOtherCosts = getSQLDataReaderValue(drTemp, "GLOtherCosts")
                                .GLCarrierCost = getSQLDataReaderValue(drTemp, "GLCarrierCost")
                                .BookFinAPExportRetry = intRetryNbr
                                .BookFinAPExportDate = getSQLDataReaderValue(drTemp, "BookFinAPExportDate")
                                .PrevSentDate = getSQLDataReaderValue(drTemp, "PrevSentDate")
                                .CompanyNumber = getSQLDataReaderValue(drTemp, "CompanyNumber")
                                .APTaxDetail1 = getSQLDataReaderValue(drTemp, "APTaxDetail1")
                                .APTaxDetail2 = getSQLDataReaderValue(drTemp, "APTaxDetail2")
                                .APTaxDetail3 = getSQLDataReaderValue(drTemp, "APTaxDetail3")
                                .APTaxDetail4 = getSQLDataReaderValue(drTemp, "APTaxDetail4")
                                .APTaxDetail5 = getSQLDataReaderValue(drTemp, "APTaxDetail5")
                                .GLAPTaxDetail1 = getSQLDataReaderValue(drTemp, "GLAPTaxDetail1")
                                .GLAPTaxDetail2 = getSQLDataReaderValue(drTemp, "GLAPTaxDetail2")
                                .GLAPTaxDetail3 = getSQLDataReaderValue(drTemp, "GLAPTaxDetail3")
                                .GLAPTaxDetail4 = getSQLDataReaderValue(drTemp, "GLAPTaxDetail4")
                                .GLAPTaxDetail5 = getSQLDataReaderValue(drTemp, "GLAPTaxDetail5")
                            End With
                            oAPFBT.AddAPFreightBillDataRow(oAPFBR)
                            oAPFBT.AcceptChanges()
                            If Me.AutoConfirmation Then
                                If Not Me.UpdateStatus(Nothing, Nothing, strFreightBillNumber, Nothing, True, Nothing, Nothing) Then
                                    oAPFBT.RejectChanges()
                                Else
                                    Me.TotalRecords += 1
                                End If
                            Else
                                If Not Me.UpdateStatus(Nothing, Nothing, strFreightBillNumber, Nothing, Nothing, intRetryNbr, Now) Then
                                    oAPFBT.RejectChanges()
                                Else
                                    Me.TotalRecords += 1
                                End If
                            End If
                        End If
                    Catch ex As Exception
                        RecordErrors += 1
                        ITEmailMsg &= "<br />Read AP freight bill record error for freight bill number " & strFreightBillNumber & "<br /> " & ex.ToString & "<br />" & vbCrLf
                    End Try
                Loop
            End If
            blnRet = True
        Catch ex As System.ApplicationException
            Throw
        Catch ex As Exception
            RecordErrors += 1
            Throw New System.ApplicationException(ex.ToString, ex.InnerException)
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
        Return blnRet

    End Function

    Protected Function UpdateStatus(ByVal BookControl As Nullable(Of Integer), _
         ByVal BookCarrOrderNumber As String, _
         ByVal BookFinAPBillNumber As String, _
         ByVal BookProNumber As String, _
         ByVal BookFinAPExportFlag As Nullable(Of Boolean), _
         ByVal BookFinAPExportRetry As Nullable(Of Integer), _
         ByVal BookFinAPExportDate As Nullable(Of Date)) As Boolean


        Dim blnRet As Boolean = False
        Dim strCriteria As String = ""
        Dim oCon As System.Data.SqlClient.SqlConnection
        Try
            oCon = getNewConnection()
            If BookControl.HasValue AndAlso BookControl.Value > 0 Then
                strCriteria &= "<br />BookControl = " & BookControl.Value.ToString
            Else
                strCriteria &= "<br />BookControl = NULL"
            End If
            If Not BookCarrOrderNumber Is Nothing AndAlso BookCarrOrderNumber.Trim.Length > 0 Then
                strCriteria &= "<br />BookCarrOrderNumber = " & BookCarrOrderNumber
            Else
                strCriteria &= "<br />BookCarrOrderNumber = NULL"
            End If
            If Not BookFinAPBillNumber Is Nothing AndAlso BookFinAPBillNumber.Trim.Length > 0 Then
                strCriteria &= "<br />BookFinAPBillNumber = " & BookFinAPBillNumber
            Else
                strCriteria &= "<br />BookFinAPBillNumber = NULL"
            End If
            If Not BookProNumber Is Nothing AndAlso BookProNumber.Trim.Length > 0 Then
                strCriteria &= "<br />BookProNumber = " & BookProNumber
            Else
                strCriteria &= "<br />BookProNumber = NULL"
            End If
            If BookFinAPExportFlag.HasValue AndAlso BookFinAPExportFlag.Value > 0 Then
                strCriteria &= "<br />BookFinAPExportFlag = " & BookFinAPExportFlag.Value.ToString
            Else
                strCriteria &= "<br />BookFinAPExportFlag = NULL"
            End If
            If BookFinAPExportRetry.HasValue AndAlso BookFinAPExportRetry.Value > 0 Then
                strCriteria &= "<br />BookFinAPExportRetry = " & BookFinAPExportRetry.Value.ToString
            Else
                strCriteria &= "<br />BookFinAPExportRetry = NULL"
            End If
            If BookFinAPExportDate.HasValue Then
                strCriteria &= "<br />BookFinAPExportDate = " & BookFinAPExportDate.Value.ToString
            Else
                strCriteria &= "<br />BookFinAPExportDate = " & Now.ToString
            End If

            Dim intRetryCt As Integer = 0
            Do
                intRetryCt += 1
                Dim objCom As New SqlCommand
                Try
                    Dim lngErrNumber As Long
                    Dim strRetVal As String = ""
                    With objCom
                        .Connection = oCon
                        .CommandTimeout = Me.CommandTimeOut
                        .Parameters.Add("@BookControl", SqlDbType.Int)
                        If BookControl.HasValue AndAlso BookControl.Value > 0 Then
                            .Parameters("@BookControl").Value = BookControl.Value
                        Else
                            .Parameters("@BookControl").Value = DBNull.Value
                        End If
                        .Parameters.Add("@BookCarrOrderNumber", SqlDbType.NVarChar, 20)
                        If Not BookCarrOrderNumber Is Nothing AndAlso BookCarrOrderNumber.Trim.Length > 0 Then
                            .Parameters("@BookCarrOrderNumber").Value = BookCarrOrderNumber
                        Else
                            .Parameters("@BookCarrOrderNumber").Value = DBNull.Value
                        End If
                        .Parameters.Add("@BookFinAPBillNumber", SqlDbType.NVarChar, 50)
                        If Not BookFinAPBillNumber Is Nothing AndAlso BookFinAPBillNumber.Trim.Length > 0 Then
                            .Parameters("@BookFinAPBillNumber").Value = BookFinAPBillNumber
                        Else
                            .Parameters("@BookFinAPBillNumber").Value = DBNull.Value
                        End If
                        .Parameters.Add("@BookProNumber", SqlDbType.NVarChar, 20)
                        If Not BookProNumber Is Nothing AndAlso BookProNumber.Trim.Length > 0 Then
                            .Parameters("@BookProNumber").Value = BookProNumber
                        Else
                            .Parameters("@BookProNumber").Value = DBNull.Value
                        End If
                        .Parameters.Add("@BookFinAPExportFlag", SqlDbType.Bit)
                        If BookFinAPExportFlag.HasValue Then
                            .Parameters("@BookFinAPExportFlag").Value = BookFinAPExportFlag.Value
                        Else
                            .Parameters("@BookFinAPExportFlag").Value = DBNull.Value
                        End If
                        .Parameters.Add("@BookFinAPExportRetry", SqlDbType.Int)
                        If BookFinAPExportRetry.HasValue AndAlso BookFinAPExportRetry.Value > 0 Then
                            .Parameters("@BookFinAPExportRetry").Value = BookFinAPExportRetry.Value
                        Else
                            .Parameters("@BookFinAPExportRetry").Value = DBNull.Value
                        End If
                        .Parameters.Add("@BookFinAPExportDate", SqlDbType.DateTime)
                        If BookFinAPExportDate.HasValue Then
                            .Parameters("@BookFinAPExportDate").Value = BookFinAPExportDate.Value
                        Else
                            .Parameters("@BookFinAPExportDate").Value = Now
                        End If
                        .Parameters.Add("@ModUser", SqlDbType.NVarChar, 25)
                        If Not Me.CreateUser Is Nothing AndAlso Me.CreateUser.Trim.Length > 0 Then
                            .Parameters("@ModUser").Value = Me.CreateUser
                        Else
                            .Parameters("@ModUser").Value = DBNull.Value
                        End If
                        .Parameters.Add("@RetMsg", SqlDbType.NVarChar, 500)
                        .Parameters("@RetMsg").Direction = ParameterDirection.Output
                        .Parameters.Add("@ErrNumber", SqlDbType.Int)
                        .Parameters("@ErrNumber").Direction = ParameterDirection.Output
                        .CommandText = "spUpdateAPExportStatusWS"
                        .CommandType = CommandType.StoredProcedure
                        .ExecuteScalar()
                        strRetVal = Trim(.Parameters("@RetMsg").Value.ToString)
                        If IsDBNull(.Parameters("@ErrNumber").Value) Then
                            lngErrNumber = 0
                        Else
                            lngErrNumber = .Parameters("@ErrNumber").Value
                        End If
                    End With

                    Try
                        If lngErrNumber <> 0 Then
                            If intRetryCt > Me.Retry Then
                                Me.RecordErrors += 1
                                LogError("Data Export Failure", "NGL.FreightMaster.Integration.clsAPFreightBillExport.UpdateStatus: Procedure spUpdateAPExportStatusWS output failure for criteria " _
                                    & strCriteria & vbCrLf & "Error # " & lngErrNumber & ": " & strRetVal, AdminEmail)
                            Else
                                Log("NGL.FreightMaster.Integration.clsAPFreightBillExport.UpdateStatus Output Failure Retry = " & intRetryCt.ToString)

                            End If
                        Else
                            Log("An AP Freight Bill record status was updated using the following criteria:  " & strCriteria)
                            blnRet = True
                            Exit Do
                        End If
                    Catch ex As Exception
                        Log("NGL.FreightMaster.Integration.clsAPFreightBillExport.UpdateStatus Unexpected Error " & ex.ToString & vbCrLf & " Retry = " & intRetryCt.ToString)
                    End Try
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        LogException("Data Export Failure", "Could not update the ap freight bill export status values. Records may be exported again on the next cycle for criteria " & strCriteria, AdminEmail, ex, "NGL.FreightMaster.Integration.clsAPFreightBillExport.UpdateStatus: Procedure spUpdateAPExportStatusWS Output Failure")
                    Else
                        Log("NGL.FreightMaster.Integration.clsAPFreightBillExport.UpdateStatus Execution Failure Retry = " & intRetryCt.ToString)

                    End If
                Finally
                    Try
                        objCom.Cancel()
                        objCom = Nothing
                    Catch ex As Exception

                    End Try
                End Try
                'We only get here if an exception is thrown and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
        Catch ex As System.ApplicationException
            Throw
        Catch ex As Exception
            Me.LastError = ex.Message
            Throw New System.ApplicationException(Me.LastError, ex.InnerException)
        Finally
            Try
#Disable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                oCon.Close()
#Enable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try
        Return blnRet
    End Function

    Public Function getDataSet() As APFreightBill
        Return New APFreightBill
    End Function

    Public Function confirmExport(ByVal strConnection As String, _
                 ByVal BookFinAPBillNumber As String) As Boolean
        Dim blnRet As Boolean = False
        Me.CreatedDate = Now.ToString
        Me.CreateUser = "Data Integration DLL"
        Me.Source = "AP Freight Bill Export Data Confirmation"
        Me.DBConnection = strConnection
        Try
            If Me.UpdateStatus(Nothing, Nothing, BookFinAPBillNumber, Nothing, True, Nothing, Nothing) Then
                Log("AP Freight Bill Status Updated")
                Return True
            End If
        Catch ex As Exception
            LogException("AP Freight Bill Confirmation Error", "Could not update the export status for the specified AP Freight Bill record.", AdminEmail, ex, "NGL.FreightMaster.Integration.clsAPFreightBillExport.confirmExport Failure")
        End Try
        Return blnRet
    End Function

    Public Function readObjectData(ByRef oAPFreightBill() As clsAPFreightBillObject, _
            ByVal strConnection As String, _
            Optional ByVal MaxRetry As Integer = 0, _
            Optional ByVal RetryMinutes As Integer = 0, _
            Optional ByVal CompNumber As String = Nothing, _
            Optional ByVal FreightBillNumber As String = Nothing) As ProcessDataReturnValues

        Dim oTable As New APFreightBill.APFreightBillDataDataTable
        Dim oRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationComplete
        Try
            oRet = readData(oTable, _
                    strConnection, _
                    MaxRetry, _
                    RetryMinutes, _
                    CompNumber, _
                    FreightBillNumber)

            If Not oTable Is Nothing Then
                If oTable.Rows.Count > 0 Then
                    ReDim oAPFreightBill(oTable.Rows.Count - 1)
                End If
                Dim intCt As Integer = 0

                For Each oRow As APFreightBill.APFreightBillDataRow In oTable

                    Dim oObject As New clsAPFreightBillObject
                    With oObject
                        .APBillDate = exportDateToString(oRow.APBillDate.ToString)
                        .APBilledWeight = oRow.APBilledWeight
                        .APBillNumber = oRow.APBillNumber
                        .APBLNumber = oRow.APBLNumber
                        .APCarrierCost = oRow.APCarrierCost
                        .APCarrierNumber = oRow.APCarrierNumber
                        .APCNSNumber = oRow.APCNSNumber
                        .APCostCenterNumber = oRow.APCostCenterNumber
                        .APCustomerID = oRow.APCustomerID
                        .APFee1 = oRow.APFee1
                        .APFee2 = oRow.APFee2
                        .APFee3 = oRow.APFee3
                        .APFee4 = oRow.APFee4
                        .APFee5 = oRow.APFee5
                        .APFee6 = oRow.APFee6
                        .APOtherCosts = oRow.APOtherCosts
                        .APPONumber = oRow.APPONumber
                        .APPRONumber = oRow.APPRONumber
                        .APReceivedDate = oRow.APReceivedDate
                        .APTotalCost = oRow.APTotalCost
                        .APTotalTax = oRow.APTotalTax
                        .BookFinAPExportDate = exportDateToString(oRow.BookFinAPExportDate.ToString)
                        .BookFinAPExportRetry = oRow.BookFinAPExportRetry
                        .CompanyNumber = oRow.CompanyNumber
                        .GLCarrierCost = oRow.GLCarrierCost
                        .GLFee1 = oRow.GLFee1
                        .GLFee2 = oRow.GLFee2
                        .GLFee3 = oRow.GLFee3
                        .GLFee4 = oRow.GLFee4
                        .GLFee5 = oRow.GLFee5
                        .GLFee6 = oRow.GLFee6
                        .GLOtherCosts = oRow.GLOtherCosts
                        .GLTotalCost = oRow.GLTotalCost
                        .GLTotalTax = oRow.GLTotalTax
                        .PrevSentDate = exportDateToString(oRow.PrevSentDate.ToString)
                    End With
                    oAPFreightBill(intCt) = oObject
                    intCt += 1
                    If intCt > oTable.Rows.Count Then Exit For
                Next
            End If

        Catch ex As Exception
            oRet = ProcessDataReturnValues.nglDataIntegrationFailure
            LogException("AP Freight Bill Export Read Object Data Failure", "Could not read AP freight bill records.", AdminEmail, ex, "NGL.FreightMaster.Integration.clsAPFreightBillExport.readObjectData Failure")
        End Try
        Return oRet
    End Function

    Public Function readData(ByRef oAPFBT As APFreightBill.APFreightBillDataDataTable, _
            ByVal strConnection As String, _
            Optional ByVal MaxRetry As Integer = 0, _
            Optional ByVal RetryMinutes As Integer = 0, _
            Optional ByVal CompNumber As String = Nothing, _
            Optional ByVal FreightBillNumber As String = Nothing) As ProcessDataReturnValues

        Dim enmRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strMsg As String = ""
        Dim strTitle As String = ""
        Dim intErrors As Integer = 0
        Dim intCount As Integer = 0
        Dim strSource As String = "clsAPFreightBillExport.readData"
        Me.ImportTypeKey = IntegrationTypes.FreightBillExport
        Me.CreatedDate = Now.ToString
        Me.CreateUser = "Data Integration DLL"
        Me.Source = "AP Freight Bill  Data Integration"
        Dim MaxRetryNbr As Nullable(Of Integer)
        Dim RetryMinutesNbr As Nullable(Of Integer)
        GroupEmailMsg = ""
        ITEmailMsg = ""
        'TODO: modify code to allow alpha company numbers
        'Validate the the CompNumber is a number. 
        Dim nintCompNumber As Nullable(Of Integer)
        Dim intTest As Integer = 0
        If Not String.IsNullOrEmpty(CompNumber) Then
            If Integer.TryParse(CompNumber, intTest) Then
                nintCompNumber = intTest
            Else
                'this is an invalid order sequence number so return false
                LastError = "The Company Number " & CompNumber & " is not valid.  Only numbers are allowed for AP Freight Bill Export method"
                Log(LastError)
                ITEmailMsg &= "<br /> The Read AP Freight Bill command failed with the following data:" _
                    & "<br /> MaxRetry: " & MaxRetry.ToString _
                    & "<br /> RetryMinutes: " & RetryMinutes.ToString _
                    & "<br /> CompanyNumber: " & CompNumber _
                    & "<br /> FreightBillNumber: " & FreightBillNumber _
                    & "<br /> Error Message: " & LastError _
                    & "<br />" & vbCrLf
                LogError("Read AP Freight Bill Data Failure Report", ITEmailMsg, AdminEmail)
                Return ProcessDataReturnValues.nglDataValidationFailure
            End If
        End If
        Me.DBConnection = strConnection
        'try the connection
        If Not Me.openConnection Then
            Return ProcessDataReturnValues.nglDataConnectionFailure
        End If


        If MaxRetry > 0 Then
            MaxRetryNbr = MaxRetry
        End If
        If RetryMinutes > 0 Then
            RetryMinutesNbr = RetryMinutes
        End If

        Try
            RecordErrors = 0
            TotalRecords = 0

            ' Read in the AP Freight Bill records
            If Not GetData(oAPFBT, MaxRetryNbr, RetryMinutesNbr, nintCompNumber, FreightBillNumber) Then
                Return ProcessDataReturnValues.nglDataIntegrationFailure
            End If

            If GroupEmailMsg.Trim.Length > 0 Then
                LogError("AP Freight Bill Max Retry Exceeded", "The following freight bills have exceeded the maximum number of retries for the AP Freight Bill Export process and may only be exported manually by an administrator." & GroupEmailMsg, GroupEmail)
            End If
            If ITEmailMsg.Trim.Length > 0 Then
                LogError("AP Freight Bill  Error Report", "The following freight bills had errors and could not be included in the AP Freight Bill Export records." & ITEmailMsg, AdminEmail)
            End If
            If Me.TotalRecords > 0 Then
                strMsg = "Success! " & Me.TotalRecords & " AP freight bill records were exported." & vbCrLf
                enmRet = ProcessDataReturnValues.nglDataIntegrationComplete
                If Me.RecordErrors > 0 Then
                    strMsg &= "<br />ERROR!  " & Me.RecordErrors & " AP freight bill records could not be exported.  Please check the email error report or database error log records for more information.<br />" & vbCrLf
                    enmRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                End If
            Else
                strMsg = "No AP freight bill records were available for export."
                enmRet = ProcessDataReturnValues.nglDataIntegrationComplete
            End If
            Log(strMsg)
        Catch ex As Exception
            LogException("AP Freight Bill Read Data Failure", "Could not read the AP freight bill data", AdminEmail, ex, "NGL.FreightMaster.Integration.clsAPFreightBillExport.readData Failure")
        Finally
            closeConnection
        End Try

        Return enmRet
    End Function

    Public Function readObjectOpenPayables(ByRef oAPFreightBill() As clsAPFreightBillObject, _
            ByVal strConnection As String, _
            Optional ByVal CompNumber As String = Nothing) As ProcessDataReturnValues

        Dim oTable As New APFreightBill.APFreightBillDataDataTable
        Dim oRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationComplete
        Try
            oRet = readOpenPayables(oTable, _
                    strConnection, _
                    CompNumber)

            If Not oTable Is Nothing Then
                If oTable.Rows.Count > 0 Then
                    ReDim oAPFreightBill(oTable.Rows.Count - 1)
                End If
                Dim intCt As Integer = 0

                For Each oRow As APFreightBill.APFreightBillDataRow In oTable

                    Dim oObject As New clsAPFreightBillObject
                    With oObject
                        .APBillDate = exportDateToString(oRow.APBillDate.ToString)
                        .APBilledWeight = oRow.APBilledWeight
                        .APBillNumber = oRow.APBillNumber
                        .APBLNumber = oRow.APBLNumber
                        .APCarrierCost = oRow.APCarrierCost
                        .APCarrierNumber = oRow.APCarrierNumber
                        .APCNSNumber = oRow.APCNSNumber
                        .APCostCenterNumber = oRow.APCostCenterNumber
                        .APCustomerID = oRow.APCustomerID
                        .APFee1 = oRow.APFee1
                        .APFee2 = oRow.APFee2
                        .APFee3 = oRow.APFee3
                        .APFee4 = oRow.APFee4
                        .APFee5 = oRow.APFee5
                        .APFee6 = oRow.APFee6
                        .APOtherCosts = oRow.APOtherCosts
                        .APPONumber = oRow.APPONumber
                        .APPRONumber = oRow.APPRONumber
                        .APReceivedDate = oRow.APReceivedDate
                        .APTotalCost = oRow.APTotalCost
                        .APTotalTax = oRow.APTotalTax
                        .BookFinAPExportDate = exportDateToString(oRow.BookFinAPExportDate.ToString)
                        .BookFinAPExportRetry = oRow.BookFinAPExportRetry
                        .CompanyNumber = oRow.CompanyNumber
                        .GLCarrierCost = oRow.GLCarrierCost
                        .GLFee1 = oRow.GLFee1
                        .GLFee2 = oRow.GLFee2
                        .GLFee3 = oRow.GLFee3
                        .GLFee4 = oRow.GLFee4
                        .GLFee5 = oRow.GLFee5
                        .GLFee6 = oRow.GLFee6
                        .GLOtherCosts = oRow.GLOtherCosts
                        .GLTotalCost = oRow.GLTotalCost
                        .GLTotalTax = oRow.GLTotalTax
                        .PrevSentDate = exportDateToString(oRow.PrevSentDate.ToString)
                    End With
                    oAPFreightBill(intCt) = oObject
                    intCt += 1
                    If intCt > oTable.Rows.Count Then Exit For
                Next
            End If

        Catch ex As Exception
            oRet = ProcessDataReturnValues.nglDataIntegrationFailure
            LogException("AP Open Payables Read Data Failure", "Could not read the AP Open Payables data", AdminEmail, ex, "NGL.FreightMaster.Integration.clsAPFreightBillExport.readObjectOpenPayables Failure")
        End Try
        Return oRet
    End Function

    Public Function readOpenPayables(ByRef oAPFBT As APFreightBill.APFreightBillDataDataTable, _
            ByVal strConnection As String, _
            Optional ByVal CompNumber As String = Nothing) As ProcessDataReturnValues

        Dim enmRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strMsg As String = ""
        Dim strTitle As String = ""
        Dim intErrors As Integer = 0
        Dim intCount As Integer = 0
        Dim strSource As String = "clsAPFreightBillExport.readOpenPayables"
        Me.ImportTypeKey = IntegrationTypes.OpenPayables
        Me.CreatedDate = Now.ToString
        Me.CreateUser = "Data Integration DLL"
        Me.Source = "AP Open Payables Data Integration"
        GroupEmailMsg = ""
        ITEmailMsg = ""

        Me.DBConnection = strConnection
        'try the connection
        If Not Me.openConnection Then
            Return ProcessDataReturnValues.nglDataConnectionFailure
        End If

        Try
            RecordErrors = 0
            TotalRecords = 0

            ' Read in the Open AP Freight Bill records
            If Not GetOpenPayableData(oAPFBT, CompNumber) Then
                Return ProcessDataReturnValues.nglDataIntegrationFailure
            End If

            If ITEmailMsg.Trim.Length > 0 Then
                LogError("AP Open Payables Error Report", "The following freight bills had errors and could not be included in the AP Open Payables Export records." & ITEmailMsg, AdminEmail)
            End If
            If Me.TotalRecords > 0 Then
                strMsg = "Success! " & Me.TotalRecords & " AP Open Payables records were exported." & vbCrLf
                enmRet = ProcessDataReturnValues.nglDataIntegrationComplete
                If Me.RecordErrors > 0 Then
                    strMsg &= "<br />ERROR!  " & Me.RecordErrors & " AP Open Payables records could not be exported.  Please check the email error report or database error log records for more information.<br />" & vbCrLf
                    enmRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                End If
            Else
                strMsg = "No AP Open Payables records were available for export."
                enmRet = ProcessDataReturnValues.nglDataIntegrationComplete
            End If
            Log(strMsg)
        Catch ex As Exception
            LogException("AP Open Payables Read Data Failure", "Could not read the AP Open Payables data", AdminEmail, ex, "NGL.FreightMaster.Integration.clsAPFreightBillExport.readOpenPayables Failure")
        Finally
            closeConnection
        End Try

        Return enmRet
    End Function

#End Region

End Class
