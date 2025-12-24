Option Strict Off
Option Explicit On

Imports Ngl.FreightMaster.Integration.Configuration
Imports Ngl.FreightMaster.Integration.FMDataTableAdapters
Imports System.Data.SqlClient
Imports System.ServiceModel
Imports DTran = Ngl.Core.Utility.DataTransformation
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports LTS = Ngl.FreightMaster.Data.LTS
Imports DAL = Ngl.FreightMaster.Data
<Serializable()>
Public Class clsAPExport : Inherits clsUpload
#Region " Class Variables and Properties "


#End Region

#Region "Constructors"

    Sub New()
        MyBase.new()
    End Sub

    Sub New(ByVal config As Ngl.FreightMaster.Core.UserConfiguration)
        MyBase.New(config)
    End Sub

    Sub New(ByVal admin_email As String,
            ByVal from_email As String,
            ByVal group_email As String,
            ByVal auto_retry As Integer,
            ByVal smtp_server As String,
            ByVal db_server As String,
            ByVal database_catalog As String,
            ByVal auth_code As String,
            ByVal debug_mode As Boolean,
            Optional ByVal connection_string As String = "")

        MyBase.New(admin_email, from_email, group_email, auto_retry, smtp_server, db_server, database_catalog, auth_code, debug_mode, connection_string)


    End Sub

#End Region

#Region " Functions "

    Protected Function GetOpenPayables(ByRef oAPET As APExport.APExportDataDataTable,
            ByVal CompNumber As String) As Boolean
        Dim blnRet As Boolean = False
        Dim cmdObj As New System.Data.SqlClient.SqlCommand
        Dim strRet As String = ""
        Dim strSQL As String = ""
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
            strSQL &= "APControl, BookProNumber, CarrierNumber, BookFinAPBillNumber, BookFinAPBillInvDate, BookCarrOrderNumber, LaneNumber, BookItemCostCenterNumber, BookFinAPActCost, BookCarrBLNumber, BookFinAPActWgt, BookFinAPBillNoDate, BookFinAPActTax, BookFinAPExportRetry, BookFinAPExportDate, PrevSentDate, CompanyNumber, BookOrderSequence, CarrierEquipmentCodes, BookCarrierTypeCode, APFee1,APFee2,APFee3,APFee4,APFee5,APFee6,OtherCosts,BookWarehouseNumber,BookShipCarrierProNumber,BookMilesFrom,CompNatNumber,APControl,BookTransType,BookReasonCode,BookShipCarrierNumber FROM dbo.udfGetOpenPayablesWS("
            If Not CompNumber Is Nothing AndAlso CompNumber.Trim.Length > 0 Then
                strSQL &= "'" & CompNumber & "'"
            Else
                strSQL &= "NULL"
            End If
            strSQL &= ")"
            If Debug Then
                Log("Select Open Payable Records: " & strSQL)
            End If
            With cmdObj
                .CommandText = strSQL
                .CommandType = CommandType.Text
                drTemp = .ExecuteReader()
            End With
            If drTemp.HasRows Then
                If oAPET Is Nothing Then
                    oAPET = New APExport.APExportDataDataTable
                End If
                Dim intRecCt As Integer = 0
                Do While drTemp.Read()
                    intRecCt += 1
                    Log("Reading Open Payable Record " & intRecCt.ToString)
                    Dim strOrderNumber As String = ""
                    Dim strProNumber As String = ""
                    Dim intOrderSequence As Integer = 0
                    Dim strCompNumber As String = ""
                    Dim intAPControl As Integer = 0
                    Try
                        intAPControl = getSQLDataReaderValue(drTemp, "APControl", 0)
                        Dim intRetryNbr As Integer = getSQLDataReaderValue(drTemp, "BookFinAPExportRetry", 0)
                        strOrderNumber = getSQLDataReaderValue(drTemp, "BookCarrOrderNumber", "")
                        strProNumber = getSQLDataReaderValue(drTemp, "BookProNumber", "")
                        intOrderSequence = getSQLDataReaderValue(drTemp, "BookOrderSequence", 0)
                        strCompNumber = getSQLDataReaderValue(drTemp, "CompanyNumber", "")
                        Dim oAPER As APExport.APExportDataRow = oAPET.NewAPExportDataRow
                        With oAPER
                            .APControl = intAPControl
                            .BookCarrBLNumber = getSQLDataReaderValue(drTemp, "BookCarrBLNumber", "")
                            .BookCarrOrderNumber = strOrderNumber
                            .BookFinAPActCost = getSQLDataReaderValue(drTemp, "BookFinAPActCost", 0)
                            .BookFinAPActTax = getSQLDataReaderValue(drTemp, "BookFinAPActTax", 0)
                            .BookFinAPActWgt = getSQLDataReaderValue(drTemp, "BookFinAPActWgt", 0)
                            .BookFinAPBillInvDate = getSQLDataReaderValue(drTemp, "BookFinAPBillInvDate")
                            .BookFinAPBillNoDate = getSQLDataReaderValue(drTemp, "BookFinAPBillNoDate")
                            .BookFinAPBillNumber = getSQLDataReaderValue(drTemp, "BookFinAPBillNumber", "")
                            .BookFinAPExportDate = getSQLDataReaderValue(drTemp, "BookFinAPExportDate")
                            .BookFinAPExportRetry = intRetryNbr
                            .BookItemCostCenterNumber = getSQLDataReaderValue(drTemp, "BookItemCostCenterNumber", "")
                            .BookProNumber = strProNumber
                            .CarrierNumber = getSQLDataReaderValue(drTemp, "CarrierNumber", 0)
                            .CompanyNumber = strCompNumber
                            .LaneNumber = getSQLDataReaderValue(drTemp, "LaneNumber", "")
                            .PrevSentDate = getSQLDataReaderValue(drTemp, "PrevSentDate")
                            .BookOrderSequence = intOrderSequence
                            .CarrierEquipmentCodes = getSQLDataReaderValue(drTemp, "CarrierEquipmentCodes", "")
                            .BookCarrierTypeCode = getSQLDataReaderValue(drTemp, "BookCarrierTypeCode", "")
                            .APFee1 = getSQLDataReaderValue(drTemp, "APFee1", 0)
                            .APFee2 = getSQLDataReaderValue(drTemp, "APFee2", 0)
                            .APFee3 = getSQLDataReaderValue(drTemp, "APFee3", 0)
                            .APFee4 = getSQLDataReaderValue(drTemp, "APFee4", 0)
                            .APFee5 = getSQLDataReaderValue(drTemp, "APFee5", 0)
                            .APFee6 = getSQLDataReaderValue(drTemp, "APFee6", 0)
                            .OtherCosts = getSQLDataReaderValue(drTemp, "OtherCosts", 0)
                            .BookWarehouseNumber = getSQLDataReaderValue(drTemp, "BookWarehouseNumber", "")
                            .BookShipCarrierProNumber = getSQLDataReaderValue(drTemp, "BookShipCarrierProNumber", "")
                            .BooKMilesFrom = getSQLDataReaderValue(drTemp, "BooKMilesFrom", 0)
                            .CompNatNumber = getSQLDataReaderValue(drTemp, "CompNatNumber", 0)
                            .APControl = getSQLDataReaderValue(drTemp, "APControl", 0)
                            .BookTransType = getSQLDataReaderValue(drTemp, "BookTransType", 0)
                            .BookReasonCode = getSQLDataReaderValue(drTemp, "BookReasonCode", "")
                            .BookShipCarrierNumber = getSQLDataReaderValue(drTemp, "BookShipCarrierNumber", "")
                        End With
                        oAPET.AddAPExportDataRow(oAPER)
                        oAPET.AcceptChanges()
                    Catch ex As Exception
                        RecordErrors += 1
                        ITEmailMsg &= "<br />Read open payable record error.<br /> " & ex.ToString & "<br />" & vbCrLf
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

    Public Function GetUnpaidFreightBills(ByVal strConnection As String, ByRef LegalEntity As String) As List(Of APUnpaidFreightBills)
        Dim lUnpaid As New List(Of APUnpaidFreightBills)
        If String.IsNullOrWhiteSpace(LegalEntity) Then Return lUnpaid
        Dim cmdObj As New System.Data.SqlClient.SqlCommand
        Dim strRet As String = ""
        Dim strSQL As String = ""
        Dim drTemp As SqlDataReader
        Try
            If Not openConnection() Then
                Return lUnpaid
            End If

            With cmdObj
                .Connection = DBCon
                .CommandTimeout = Me.CommandTimeOut
            End With

            strSQL = "SELECT "
            If Me.MaxRowsReturned > 0 Then
                strSQL &= " TOP " & Me.MaxRowsReturned.ToString & " "
            End If
            strSQL &= "BookFinAPBillNumber,CarrierAlphaCode,CompLegalEntity,CompAlphaCode,ActualCost FROM dbo.udfGetUnpaidFreightBills('" & LegalEntity & "')"

            If Debug Then
                Log("Select Unpaid Freight Bill Records: " & strSQL)
            End If
            With cmdObj
                .CommandText = strSQL
                .CommandType = CommandType.Text
                drTemp = .ExecuteReader()
            End With
            If drTemp.HasRows Then
                Dim intRecCt As Integer = 0
                Do While drTemp.Read()
                    intRecCt += 1
                    Log("Reading Open Payable Record " & intRecCt.ToString)
                    Dim strOrderNumber As String = ""
                    Dim strProNumber As String = ""
                    Dim intOrderSequence As Integer = 0
                    Dim strCompNumber As String = ""
                    Dim intAPControl As Integer = 0
                    Try
                        lUnpaid.Add(New APUnpaidFreightBills(getSQLDataReaderValue(drTemp, "BookFinAPBillNumber", "") _
                                                             , getSQLDataReaderValue(drTemp, "CarrierAlphaCode", "") _
                                                             , getSQLDataReaderValue(drTemp, "CompLegalEntity", "") _
                                                             , getSQLDataReaderValue(drTemp, "CompAlphaCode", "") _
                                                             , getSQLDataReaderValue(drTemp, "ActualCost", 0)))



                    Catch ex As Exception
                        RecordErrors += 1
                        ITEmailMsg &= "<br />Read unpaid freight bill record error.<br /> " & ex.ToString & "<br />" & vbCrLf
                    End Try
                Loop
            End If
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
        Return lUnpaid

    End Function

    Protected Function GetData(ByRef oAPET As APExport.APExportDataDataTable,
            ByRef oAPDetails As APExport.APItemDetailsDataTable,
            ByVal MaxRetryNbr As Nullable(Of Integer),
            ByVal RetryMinutes As Nullable(Of Integer),
            ByVal CompNumber As String,
            ByVal OrderNumber As String,
            ByVal BookOrderSequence As Nullable(Of Integer),
            ByVal BookProNumber As String) As Boolean
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

            strSQL = "Execute dbo.spGetAPExportRecords NULL," 'We pass Null as the value to the @MaxRetryNbr parameter because this function handles the max retry issue after the records are returned
            'If Me.MaxRowsReturned > 0 Then
            '    strSQL &= " TOP " & Me.MaxRowsReturned.ToString & " "
            'End If
            'strSQL &= "APControl, BookProNumber, CarrierNumber, BookFinAPBillNumber, BookFinAPBillInvDate, BookCarrOrderNumber, LaneNumber, BookItemCostCenterNumber, BookFinAPActCost, BookCarrBLNumber, BookFinAPActWgt, BookFinAPBillNoDate, BookFinAPActTax, BookFinAPExportRetry, BookFinAPExportDate, PrevSentDate, CompanyNumber, BookOrderSequence, CarrierEquipmentCodes, BookCarrierTypeCode, APFee1,APFee2,APFee3,APFee4,APFee5,APFee6,OtherCosts,BookWarehouseNumber,BookShipCarrierProNumber,BookMilesFrom,CompNatNumber,BookTransType,BookReasonCode,BookShipCarrierNumber FROM dbo.udfGetAPExportRecordsWS(NULL,"

            If RetryMinutes.HasValue AndAlso RetryMinutes.Value > 0 Then
                strSQL &= RetryMinutes.Value.ToString
            Else
                strSQL &= "NULL"
            End If
            strSQL &= ","
            If Not CompNumber Is Nothing AndAlso CompNumber.Trim.Length > 0 Then
                strSQL &= "'" & CompNumber & "'"
            Else
                strSQL &= "NULL"
            End If
            strSQL &= ","
            If Not OrderNumber Is Nothing AndAlso OrderNumber.Trim.Length > 0 Then
                strSQL &= "'" & OrderNumber & "'"
            Else
                strSQL &= "NULL"
            End If
            strSQL &= ","
            If BookOrderSequence.HasValue Then
                strSQL &= BookOrderSequence.Value.ToString
            Else
                strSQL &= "NULL"
            End If
            strSQL &= ","
            If Me.MaxRowsReturned > 0 Then
                strSQL &= MaxRowsReturned.ToString
            Else
                strSQL &= "NULL"
            End If
            'strSQL &= ")"
            If Debug Then
                Log("Get AP Export Records: " & strSQL)
            End If
            With cmdObj
                .CommandText = strSQL
                .CommandType = CommandType.Text
                drTemp = .ExecuteReader()
            End With
            If drTemp.HasRows Then
                If oAPET Is Nothing Then
                    oAPET = New APExport.APExportDataDataTable
                End If
                Dim intRecCt As Integer = 0
                Do While drTemp.Read()
                    intRecCt += 1
                    Log("Reading AP Export Record " & intRecCt.ToString)
                    Dim strOrderNumber As String = ""
                    Dim strProNumber As String = ""
                    Dim intOrderSequence As Integer = 0
                    Dim strCompNumber As String = ""
                    Dim intAPControl As Integer = 0
                    Dim intCompNatNumber As Integer = 0
                    Try
                        intAPControl = getSQLDataReaderValue(drTemp, "APControl", 0)
                        Dim intRetryNbr As Integer = getSQLDataReaderValue(drTemp, "BookFinAPExportRetry", 0)
                        strOrderNumber = getSQLDataReaderValue(drTemp, "BookCarrOrderNumber", "")
                        strProNumber = getSQLDataReaderValue(drTemp, "BookProNumber", "")
                        intOrderSequence = getSQLDataReaderValue(drTemp, "BookOrderSequence", 0)
                        strCompNumber = getSQLDataReaderValue(drTemp, "CompanyNumber", "")
                        intCompNatNumber = getSQLDataReaderValue(drTemp, "CompNatNumber", 0)

                        'intAPControl = getSQLDataReaderValue(drTemp, "APControl ", 0)
                        If MaxRetryNbr.HasValue AndAlso intRetryNbr > MaxRetryNbr.Value Then
                            'we do not export this record we add this record to the group email error message.
                            GroupEmailMsg &= "<br /> The following AP Export record has exceeded the maximum number or export retries:" _
                                   & "<br /> AP Control: " & intAPControl.ToString _
                                   & "<br /> Book Pro Number: " & strProNumber _
                                   & "<br /> Order Number: " & strOrderNumber _
                                   & "<br /> Order Sequence: " & intOrderSequence.ToString _
                                   & "<br /> Company Number: " & strCompNumber _
                                   & "<br /> Retry # " & intRetryNbr.ToString & " of " & MaxRetryNbr.Value.ToString _
                                   & "<br />" & vbCrLf
                        Else
                            'add the header data
                            Dim oAPER As APExport.APExportDataRow = oAPET.NewAPExportDataRow
                            With oAPER
                                .APControl = intAPControl
                                .BookCarrBLNumber = getSQLDataReaderValue(drTemp, "BookCarrBLNumber", "")
                                .BookCarrOrderNumber = strOrderNumber
                                .BookFinAPActCost = getSQLDataReaderValue(drTemp, "BookFinAPActCost", 0)
                                .BookFinAPActTax = getSQLDataReaderValue(drTemp, "BookFinAPActTax", 0)
                                .BookFinAPActWgt = getSQLDataReaderValue(drTemp, "BookFinAPActWgt", 0)
                                .BookFinAPBillInvDate = getSQLDataReaderValue(drTemp, "BookFinAPBillInvDate")
                                .BookFinAPBillNoDate = getSQLDataReaderValue(drTemp, "BookFinAPBillNoDate")
                                .BookFinAPBillNumber = getSQLDataReaderValue(drTemp, "BookFinAPBillNumber", "")
                                .BookFinAPExportDate = getSQLDataReaderValue(drTemp, "BookFinAPExportDate")
                                .BookFinAPExportRetry = intRetryNbr
                                .BookItemCostCenterNumber = getSQLDataReaderValue(drTemp, "BookItemCostCenterNumber", "")
                                .BookProNumber = strProNumber
                                .CarrierNumber = getSQLDataReaderValue(drTemp, "CarrierNumber", 0)
                                .CompanyNumber = strCompNumber
                                .LaneNumber = getSQLDataReaderValue(drTemp, "LaneNumber", "")
                                .PrevSentDate = getSQLDataReaderValue(drTemp, "PrevSentDate")
                                .BookOrderSequence = intOrderSequence
                                .CarrierEquipmentCodes = getSQLDataReaderValue(drTemp, "CarrierEquipmentCodes", "")
                                .BookCarrierTypeCode = getSQLDataReaderValue(drTemp, "BookCarrierTypeCode", "")
                                .APFee1 = getSQLDataReaderValue(drTemp, "APFee1", 0)
                                .APFee2 = getSQLDataReaderValue(drTemp, "APFee2", 0)
                                .APFee3 = getSQLDataReaderValue(drTemp, "APFee3", 0)
                                .APFee4 = getSQLDataReaderValue(drTemp, "APFee4", 0)
                                .APFee5 = getSQLDataReaderValue(drTemp, "APFee5", 0)
                                .APFee6 = getSQLDataReaderValue(drTemp, "APFee6", 0)
                                .OtherCosts = getSQLDataReaderValue(drTemp, "OtherCosts", 0)
                                .BookWarehouseNumber = getSQLDataReaderValue(drTemp, "BookWarehouseNumber", "")
                                .BookShipCarrierProNumber = getSQLDataReaderValue(drTemp, "BookShipCarrierProNumber", "")
                                .BooKMilesFrom = getSQLDataReaderValue(drTemp, "BooKMilesFrom", 0)
                                .CompNatNumber = intCompNatNumber
                                .APControl = getSQLDataReaderValue(drTemp, "APControl", 0)
                                .BookTransType = getSQLDataReaderValue(drTemp, "BookTransType", 0)
                                .BookReasonCode = getSQLDataReaderValue(drTemp, "BookReasonCode", "")
                                .BookShipCarrierNumber = getSQLDataReaderValue(drTemp, "BookShipCarrierNumber", "")
                                .APTaxDetail1 = getSQLDataReaderValue(drTemp, "APTaxDetail1", 0)
                                .APTaxDetail2 = getSQLDataReaderValue(drTemp, "APTaxDetail2", 0)
                                .APTaxDetail3 = getSQLDataReaderValue(drTemp, "APTaxDetail3", 0)
                                .APTaxDetail4 = getSQLDataReaderValue(drTemp, "APTaxDetail4", 0)
                                .APTaxDetail5 = getSQLDataReaderValue(drTemp, "APTaxDetail5", 0)
                            End With
                            oAPET.AddAPExportDataRow(oAPER)
                            Me.TotalRecords += 1
                            'now add the item details
                            If Not addAPDetailRows(oAPDetails, strProNumber, strOrderNumber, intOrderSequence, strCompNumber, intAPControl, intCompNatNumber) Then
                                oAPET.RejectChanges()
                                ItemErrors += 1
                                ITEmailMsg &= "<br />Unable to read AP item details for:" _
                                   & "<br /> AP Control: " & intAPControl.ToString _
                                   & "<br /> Book Pro Number: " & strProNumber _
                                   & "<br /> Order Number: " & strOrderNumber _
                                   & "<br /> Order Sequence: " & intOrderSequence.ToString _
                                   & "<br /> Company Number: " & strCompNumber _
                                   & "<br />" & vbCrLf

                            Else
                                oAPET.AcceptChanges()
                                Me.TotalRecords += 1
                                Try
                                    If Me.AutoConfirmation Then
                                        If UpdateStatus(Nothing, intAPControl, Nothing, Nothing, Nothing, Nothing, True, intRetryNbr, Now, Nothing) Then
                                            Log("AP Export Auto Confirmation Complete For AP Control " & intAPControl & ".")
                                        End If
                                    Else
                                        'just update the date and retry number
                                        UpdateStatus(Nothing, intAPControl, Nothing, Nothing, Nothing, Nothing, Nothing, intRetryNbr, Now, Nothing)
                                    End If
                                Catch ex As System.ApplicationException
                                    'Log the exception and move on to the next record
                                    LogException("AP Export Update Status Error (duplicate export possible)", "Could not update the export status for AP Control number " & intAPControl.ToString & ".", AdminEmail, ex, "NGL.FreightMaster.Integration.clsAPExport.GetData Failure")
                                Catch ex As Exception
                                    'this is an unexpected error so re throw it
                                    Throw
                                End Try
                            End If
                        End If
                    Catch ex As Exception
                        RecordErrors += 1
                        ITEmailMsg &= "<br />Read AP export record error for order number " & strOrderNumber & " using sequence number " & intOrderSequence.ToString & " <br /> " & ex.ToString & "<br />" & vbCrLf
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

    Public Function UpdateStatus(ByVal BookControl As Nullable(Of Integer),
                                 ByVal APControl As Nullable(Of Integer),
                                 ByVal BookCarrOrderNumber As String,
                                 ByVal BookOrderSequence As Nullable(Of Integer),
                                 ByVal BookFinAPBillNumber As String,
                                 ByVal BookProNumber As String,
                                 ByVal BookFinAPExportFlag As Nullable(Of Boolean),
                                 ByVal BookFinAPExportRetry As Nullable(Of Integer),
                                 ByVal BookFinAPExportDate As Nullable(Of Date),
                                 ByVal CompanyNumber As String) As Boolean

        Dim blnRet As Boolean = False
        Dim strCriteria As String = ""
        Dim oCon As System.Data.SqlClient.SqlConnection
        Dim ExportDate As Date
        Dim Exported As Boolean

        Try
            oCon = getNewConnection()
            If BookControl.HasValue AndAlso BookControl.Value > 0 Then
                strCriteria &= "<br />BookControl = " & BookControl.Value.ToString
            Else
                strCriteria &= "<br />BookControl = NULL"
            End If

            If APControl.HasValue AndAlso APControl.Value > 0 Then
                strCriteria &= "<br />APControl = " & APControl.Value.ToString
            Else
                strCriteria &= "<br />APControl = NULL"
            End If

            If Not BookCarrOrderNumber Is Nothing AndAlso BookCarrOrderNumber.Trim.Length > 0 Then
                strCriteria &= "<br />BookCarrOrderNumber = " & BookCarrOrderNumber
            Else
                strCriteria &= "<br />BookCarrOrderNumber = NULL"
            End If

            If BookOrderSequence.HasValue AndAlso BookOrderSequence.Value > 0 Then
                strCriteria &= "<br />BookOrderSequence = " & BookOrderSequence.Value.ToString
            Else
                strCriteria &= "<br />BookOrderSequence = NULL"
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
                Exported = BookFinAPExportFlag.Value
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
                ExportDate = BookFinAPExportDate.Value
            Else
                strCriteria &= "<br />BookFinAPExportDate = " & Now.ToString
            End If

            If Not CompanyNumber Is Nothing AndAlso CompanyNumber.Trim.Length > 0 Then
                strCriteria &= "<br />CompanyNumber = " & CompanyNumber
            Else
                strCriteria &= "<br />CompanyNumber = NULL"
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

                        .Parameters.Add("@APControl", SqlDbType.Int)
                        If APControl.HasValue AndAlso APControl.Value > 0 Then
                            .Parameters("@APControl").Value = APControl.Value
                        Else
                            .Parameters("@APControl").Value = DBNull.Value
                        End If

                        .Parameters.Add("@BookCarrOrderNumber", SqlDbType.NVarChar, 20)
                        If Not BookCarrOrderNumber Is Nothing AndAlso BookCarrOrderNumber.Trim.Length > 0 Then
                            .Parameters("@BookCarrOrderNumber").Value = BookCarrOrderNumber
                        Else
                            .Parameters("@BookCarrOrderNumber").Value = DBNull.Value
                        End If

                        .Parameters.Add("@BookOrderSequence", SqlDbType.Int)
                        If BookOrderSequence.HasValue AndAlso BookOrderSequence.Value > 0 Then
                            .Parameters("@BookOrderSequence").Value = BookOrderSequence.Value
                        Else
                            .Parameters("@BookOrderSequence").Value = DBNull.Value
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

                        .Parameters.Add("@CompanyNumber", SqlDbType.NVarChar, 50)
                        If Not CompanyNumber Is Nothing AndAlso CompanyNumber.Trim.Length > 0 Then
                            .Parameters("@CompanyNumber").Value = CompanyNumber
                        Else
                            .Parameters("@CompanyNumber").Value = DBNull.Value
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
                        .ExecuteNonQuery()
                        strRetVal = Trim(.Parameters("@RetMsg").Value.ToString)
                        If IsDBNull(.Parameters("@ErrNumber").Value) Then
                            lngErrNumber = 0
                        Else
                            lngErrNumber = .Parameters("@ErrNumber").Value
                        End If
                    End With

                    If lngErrNumber <> 0 Then
                        If intRetryCt > Me.Retry Then
                            Me.StatusUpdateErrors += 1
                            ITEmailMsg &= "<br /> The update AP status command failed " & intRetryCt & " times for the following data:" _
                                & "<br /> Criteria: " & strCriteria _
                                & "<br /> ExportDate: " & ExportDate.ToString _
                                & "<br /> Exported Flag: " & Exported.ToString _
                                & "<br /> Error Number: " & lngErrNumber.ToString _
                                & "<br /> Error Message: " & strRetVal _
                                & "<br />" & vbCrLf
                            Exit Do
                        Else
                            Log("NGL.FreightMaster.Integration.clsAPExport.UpdateStatus Output Failure. Retry # " & intRetryCt.ToString & " of " & Me.Retry.ToString)
                        End If
                    Else
                        Log("An AP Export record status was updated using the following criteria:  " & strCriteria)
                        Return True
                        Exit Do
                    End If
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        Me.StatusUpdateErrors += 1
                        'throw a system application exception and let the calling procedure determine if that application 
                        'should exit or continue processing data
                        Me.LastError = "Could not update the AP status values.  The system attempted to update the status " & intRetryCt.ToString & " times without success.  Duplicate values may be exported on the next cycle for the following criteria " & strCriteria & ". Error: " & Me.readExceptionMessage(ex)
                        Throw New System.ApplicationException(Me.LastError, ex.InnerException)
                        Exit Do
                    Else
                        Log("NGL.FreightMaster.Integration.clsAPExport.UpdateStatus Execution Error. Retry # " & intRetryCt.ToString & " of " & Me.Retry.ToString & ". " & ex.Message)
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


        Catch ex As Exception
            Throw
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

    Protected Function UpdateStatusEx(
            ByVal APControl As Long,
            ByVal ExportDate As Date,
            Optional ByVal Exported As Boolean = True) As Boolean
        Return UpdateStatus(Nothing, APControl, Nothing, Nothing, Nothing, Nothing, Exported, Nothing, ExportDate, Nothing)
    End Function


    Public Function UpdateStatus(ByVal BookSHID As String,
                                 ByVal APBillNumber As String,
                                 ByVal strConnection As String,
                                 Optional ByVal APExportFlag As Boolean = True,
                                 Optional APExportDate As Nullable(Of Date) = Nothing,
                                 Optional APExportRetry As Nullable(Of Integer) = Nothing) As Boolean

        Dim blnRet As Boolean = False
        Dim strCriteria As String = ""
        Dim ExportDate As Date = Date.Now
        Dim Exported As Boolean = True

        Try


            If APExportDate.HasValue Then ExportDate = APExportDate.Value

            Dim strRetry As String = ""
            If APExportRetry.HasValue AndAlso APExportRetry.Value > 0 Then strRetry = "<br />Retry = " & APExportRetry.Value.ToString()


            strCriteria = String.Format("<br />Shipment ID = {0} <br />Freight Bill Number = {1}  <br />Exported = {2} <br />Export Date = {3} {4}", BookSHID, APBillNumber, APExportFlag.ToString, ExportDate.ToString("g"), strRetry)
            Dim oWCFParameters As New DAL.WCFParameters
            With oWCFParameters
                .UserName = "System Download"
                .Database = Me.Database
                .DBServer = Me.DBServer
                .ConnectionString = strConnection
                .WCFAuthCode = "NGLSystem"
                .ValidateAccess = False
            End With

            Dim oAPWCFData = New DAL.NGLAPMassEntryData(oWCFParameters)
            blnRet = oAPWCFData.UpdateStatus(BookSHID, APBillNumber, APExportFlag, ExportDate, APExportRetry)

            If Not blnRet Then
                Me.StatusUpdateErrors += 1
                ITEmailMsg &= "<br /> The update AP status command failed for the following data: " _
                    & "<br /> Criteria: " & strCriteria _
                    & "<br /> ExportDate: " & ExportDate.ToString _
                    & "<br /> Exported Flag: " & Exported.ToString _
                    & "<br />" & vbCrLf
            Else
                Log("An AP Export record status was updated using the following criteria:  " & strCriteria)
            End If
        Catch sqlEx As FaultException(Of DAL.SqlFaultInfo)
            Me.LastError = "Could not update the AP status values.  The system attempted to update the AP  status without success.  Duplicate values may be exported on the next cycle for the following criteria " & strCriteria & ". Error: " & sqlEx.Detail.ToString(sqlEx.Reason.ToString())
            Throw New System.ApplicationException(Me.LastError)
        Catch ex As Exception
            'throw a system application exception and let the calling procedure determine if that application 
            'should exit or continue processing data
            Me.LastError = "Could not update the AP status values.  The system attempted to update the AP  status without success.  Duplicate values may be exported on the next cycle for the following criteria " & strCriteria & ". Error: " & Me.readExceptionMessage(ex)
            Throw New System.ApplicationException(Me.LastError, ex.InnerException)
        Finally

        End Try

        Return blnRet
    End Function


    Public Function getDataSet() As APExport
        Return New APExport
    End Function

    Public Function confirmExport(ByVal strConnection As String,
                    Optional ByVal BookCarrOrderNumber As String = Nothing,
                    Optional ByVal BookFinAPBillNumber As String = Nothing,
                    Optional ByVal BookProNumber As String = Nothing,
                    Optional ByVal CompanyNumber As String = Nothing,
                    Optional ByVal BookOrderSequence As String = Nothing) As Boolean
        Dim blnRet As Boolean = False
        Me.CreatedDate = Now.ToString
        Me.CreateUser = "Data Integration DLL"
        Me.Source = "AP Export Data Confirmation"
        Me.DBConnection = strConnection
        Dim nintBookOrderSequence As Nullable(Of Integer)
        Dim intTest As Integer = 0
        If Not String.IsNullOrEmpty(BookOrderSequence) Then
            If Integer.TryParse(BookOrderSequence, intTest) Then
                nintBookOrderSequence = intTest
            Else
                'this is an invalid order sequence number so return false
                LastError = "The Book Order Sequence Number " & BookOrderSequence & " is not valid."
                Log(LastError)
                ITEmailMsg &= "<br /> The Confirm AP Export command failed with the following data:" _
                    & "<br /> BookCarrOrderNumber: " & BookCarrOrderNumber _
                    & "<br /> BookFinAPBillNumber: " & BookFinAPBillNumber _
                    & "<br /> BookProNumber: " & BookProNumber _
                    & "<br /> CompanyNumber: " & CompanyNumber _
                    & "<br /> BookOrderSequence: " & BookOrderSequence _
                    & "<br /> Error Message: " & LastError _
                    & "<br />" & vbCrLf
                LogError("AP Confirm Export Error Report", ITEmailMsg, AdminEmail)

                Return False
            End If
        End If
        Try
            If Me.UpdateStatus(Nothing, Nothing, BookCarrOrderNumber, nintBookOrderSequence, BookFinAPBillNumber, BookProNumber, True, Nothing, Nothing, CompanyNumber) Then
                Log("AP Export Status Updated")
                Return True
            End If
        Catch ex As Exception
            LogException("AP Export Confirmation Error", "Could not update the export status for the specified AP Export record.", AdminEmail, ex, "NGL.FreightMaster.Integration.clsAPExport.confirmExport Failure")
        End Try

        Return blnRet
    End Function

    Public Function confirmExportEx(ByVal strConnection As String,
                    ByVal APControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        Me.CreatedDate = Now.ToString
        Me.CreateUser = "Data Integration DLL"
        Me.Source = "AP Export Data Confirmation"
        Me.DBConnection = strConnection
        Try
            If Me.UpdateStatus(Nothing, APControl, Nothing, Nothing, Nothing, Nothing, True, Nothing, Nothing, Nothing) Then
                Log("AP Export Status Updated")
                Return True
            End If
        Catch ex As Exception
            LogException("AP Export Confirmation Error", "Could not update the export status for the specified AP Export record.", AdminEmail, ex, "NGL.FreightMaster.Integration.clsAPExport.confirmExportEx Failure")
        End Try

        Return blnRet
    End Function

    'Public Function readObjectData(ByRef oAPExport() As clsAPExportObject, _
    '        ByVal strConnection As String, _
    '        Optional ByVal MaxRetry As Integer = 0, _
    '        Optional ByVal RetryMinutes As Integer = 0, _
    '        Optional ByVal CompNumber As String = Nothing, _
    '        Optional ByVal OrderNumber As String = Nothing) As ProcessDataReturnValues

    '    Dim oAPET As New APExport.APExportDataDataTable
    '    Dim oRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationComplete
    '    Try
    '        oRet = readData(oAPET, _
    '                strConnection, _
    '                MaxRetry, _
    '                RetryMinutes, _
    '                CompNumber, _
    '                OrderNumber)

    '        If Not oAPET Is Nothing Then
    '            If oAPET.Rows.Count > 0 Then
    '                ReDim oAPExport(oAPET.Rows.Count - 1)
    '            End If
    '            Dim intCt As Integer = 0
    '            'For intCt As Integer = 0 To oAPET.Rows.Count - 1
    '            For Each APRow As APExport.APExportDataRow In oAPET

    '                Dim oAP As New clsAPExportObject

    '                oAP.BookCarrBLNumber = APRow.BookFinAPBillNumber
    '                oAP.BookCarrOrderNumber = APRow.BookCarrOrderNumber
    '                oAP.BookFinAPACtCost = APRow.BookFinAPActCost
    '                oAP.BookFinAPActTax = APRow.BookFinAPActTax
    '                oAP.BookFinAPActWgt = APRow.BookFinAPActWgt
    '                oAP.BookFinAPBillInvDate = exportDateToString(APRow.BookFinAPBillInvDate.ToString)
    '                oAP.BookFinAPBillNoDate = exportDateToString(APRow.BookFinAPBillNoDate.ToString)
    '                oAP.BookFinAPBillNumber = APRow.BookFinAPBillNumber
    '                oAP.BookFinAPExportDate = exportDateToString(APRow.BookFinAPExportDate.ToString)
    '                oAP.BookFinAPExportRetry = APRow.BookFinAPExportRetry
    '                oAP.BookItemCostCenterNumber = APRow.BookItemCostCenterNumber
    '                oAP.BookProNumber = APRow.BookProNumber
    '                oAP.CarrierNumber = APRow.CarrierNumber
    '                oAP.LaneNumber = APRow.LaneNumber
    '                oAP.PrevSentDate = exportDateToString(APRow.PrevSentDate.ToString)
    '                oAP.CompanyNumber = APRow.CompanyNumber
    '                oAP.OrderSequence = APRow.OrderSequence
    '                oAP.CarrierEquipmentCodes = APRow.CarrierEquipmentCodes
    '                oAP.CarrierTypeCode = APRow.CarrierTypeCode
    '                oAP.APFee1 = APRow.APFee1
    '                oAP.APFee2 = APRow.APFee2
    '                oAP.APFee3 = APRow.APFee3
    '                oAP.APFee4 = APRow.APFee4
    '                oAP.APFee5 = APRow.APFee5
    '                oAP.APFee6 = APRow.APFee6
    '                oAP.OtherCosts = APRow.OtherCosts
    '                oAP.BookWarehouseNumber = APRow.BookWarehouseNumber
    '                oAP.CarrierProNumber = APRow.CarrierProNumber
    '                oAP.Miles = APRow.Miles
    '                oAP.CompNatNumber = APRow.CompNatNumber
    '                oAP.ReasonCode = APRow.ReasonCode
    '                oAP.TransportationType = APRow.TransportationType
    '                oAPExport(intCt) = oAP
    '                intCt += 1
    '                If intCt > oAPET.Rows.Count Then Exit For
    '            Next
    '        End If

    '    Catch ex As Exception
    '        oRet = ProcessDataReturnValues.nglDataIntegrationFailure
    '        LogException("AP Export Read Object Data Failure", "Could not read AP export records.", AdminEmail, ex, "NGL.FreightMaster.Integration.clsAPExport.readObjectData Failure")
    '    End Try
    '    Return oRet
    'End Function


    ''' <summary>
    ''' Check the database for new AP Export records and convert the data table information 
    ''' to an array of class objects using basic primitive properties
    ''' </summary>
    ''' <param name="oAPExport">
    ''' An output parameter that contains an array of clsAPExportObjects.
    ''' </param>
    ''' <param name="strConnection">
    ''' The required database connection string
    ''' </param>
    ''' <param name="MaxRetry">
    ''' The maximum number or retries before alerts are raised.  
    ''' Works with RetryMinutes to allow delays and error reporting in export confirmations.
    ''' </param>
    ''' <param name="RetryMinutes">
    ''' Enter the number of minutes before resending the AP Record if a confirmaiton has not been received.
    ''' </param>
    ''' <param name="CompNumber">
    ''' Optional company number filter.  
    ''' Select records filtered by company number.
    ''' </param>
    ''' <param name="OrderNumber">
    ''' Optional Order Number.  
    ''' Select records filtered by Order Number.  
    ''' To insure record uniqueness also provide a company number.
    ''' </param>
    ''' <param name="OrderSequence">
    ''' Optional Order Sequence Number.  
    ''' Each order number can have multple legs or sections. 
    ''' Select records filtered by Order Number and Order Sequence.
    ''' Order Number must also be provided or the value is ignored.
    ''' To insuer record uniqueness also provide a company number.
    ''' </param>
    ''' <param name="BookProNumber">
    ''' Optional NGL unique PRO Number.
    ''' Select records filtered by the Booking PRO Number.
    ''' Each BookProNumber is unique.  Always ensures that only one record is selected.
    ''' </param>
    ''' <param name="oAPExportDetails">
    ''' Optional Output parameter that exports item level details as an array of clsAPExportDetailObjects.
    ''' </param>
    ''' <returns>
    ''' nglDataIntegrationComplete value = 0        
    ''' nglDataConnectionFailure value = 1
    ''' nglDataValidationFailure value = 2
    ''' nglDataIntegrationFailure value = 3
    ''' nglDataIntegrationHadErrors value = 4
    ''' </returns>
    ''' <remarks>
    ''' </remarks>
    Public Function readObjectData(ByRef oAPExport() As clsAPExportObject,
                                        ByVal strConnection As String,
                                        Optional ByVal MaxRetry As Integer = 0,
                                        Optional ByVal RetryMinutes As Integer = 0,
                                        Optional ByVal CompNumber As String = Nothing,
                                        Optional ByVal OrderNumber As String = Nothing,
                                        Optional ByVal OrderSequence As String = Nothing,
                                        Optional ByVal BookProNumber As String = Nothing,
                                        Optional ByRef oAPExportDetails() As clsAPExportDetailObject = Nothing) As ProcessDataReturnValues

        Dim oAPET As New APExport.APExportDataDataTable
        Dim oAPETD As New APExport.APItemDetailsDataTable
        Dim oRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationComplete
        Try
            oRet = readData(oAPET,
                    strConnection,
                    MaxRetry,
                    RetryMinutes,
                    CompNumber,
                    OrderNumber,
                    OrderSequence,
                    BookProNumber,
                    oAPETD)

            If Not oAPET Is Nothing Then
    If oAPET.Rows.Count > 0 Then
    ReDim oAPExport(oAPET.Rows.Count - 1)
    End If
    Dim intCt As Integer = 0
    'For intCt As Integer = 0 To oAPET.Rows.Count - 1
    For Each APRow As APExport.APExportDataRow In oAPET

    Dim oAP As New clsAPExportObject

                    oAP.BookCarrBLNumber = DTran.getDataRowString(APRow, "BookFinAPBillNumber", "")
                    oAP.BookCarrOrderNumber = DTran.getDataRowString(APRow, "BookCarrOrderNumber", "")
                    oAP.BookFinAPACtCost = DTran.NZ(APRow, "BookFinAPActCost", 0)
                    oAP.BookFinAPActTax = DTran.NZ(APRow, "BookFinAPActTax", 0)
                    oAP.BookFinAPActWgt = DTran.NZ(APRow, "BookFinAPActWgt", 0)
                    oAP.BookFinAPBillInvDate = exportDateToString(DTran.getDataRowString(APRow, "BookFinAPBillInvDate", ""))
                    oAP.BookFinAPBillNoDate = exportDateToString(DTran.getDataRowString(APRow, "BookFinAPBillNoDate", ""))
                    oAP.BookFinAPBillNumber = DTran.getDataRowString(APRow, "BookFinAPBillNumber", "")
                    oAP.BookFinAPExportDate = exportDateToString(DTran.getDataRowString(APRow, "BookFinAPExportDate", ""))
                    oAP.BookFinAPExportRetry = DTran.NZ(APRow, "BookFinAPExportRetry", 0)
                    oAP.BookItemCostCenterNumber = DTran.getDataRowString(APRow, "BookItemCostCenterNumber", "")
                    oAP.BookProNumber = DTran.getDataRowString(APRow, "BookProNumber", "")
                    oAP.CarrierNumber = DTran.getDataRowString(APRow, "CarrierNumber", "")
                    oAP.LaneNumber = DTran.getDataRowString(APRow, "LaneNumber", "")
                    oAP.PrevSentDate = exportDateToString(DTran.getDataRowString(APRow, "PrevSentDate", ""))
                    oAP.CompanyNumber = DTran.getDataRowString(APRow, "CompanyNumber", "")
                    oAP.BookOrderSequence = DTran.NZ(APRow, "BookOrderSequence", 0)
                    oAP.CarrierEquipmentCodes = DTran.getDataRowString(APRow, "CarrierEquipmentCodes", "")
                    oAP.BookCarrierTypeCode = DTran.getDataRowString(APRow, "BookCarrierTypeCode", "")
                    oAP.APFee1 = DTran.NZ(APRow, "APFee1", 0)
                    oAP.APFee2 = DTran.NZ(APRow, "APFee2", 0)
                    oAP.APFee3 = DTran.NZ(APRow, "APFee3", 0)
                    oAP.APFee4 = DTran.NZ(APRow, "APFee4", 0)
                    oAP.APFee5 = DTran.NZ(APRow, "APFee5", 0)
                    oAP.APFee6 = DTran.NZ(APRow, "APFee6", 0)
                    oAP.OtherCosts = DTran.NZ(APRow, "OtherCosts", 0)
                    oAP.BookWarehouseNumber = DTran.getDataRowString(APRow, "BookWarehouseNumber", "")
                    oAP.BookShipCarrierProNumber = DTran.getDataRowString(APRow, "BookShipCarrierProNumber", "")
                    oAP.BookMilesFrom = DTran.NZ(APRow, "BooKMilesFrom", 0)
                    oAP.CompNatNumber = DTran.NZ(APRow, "CompNatNumber", 0)
                    oAP.BookReasonCode = DTran.getDataRowString(APRow, "BookReasonCode", "")
                    oAP.BookTransType = DTran.getDataRowString(APRow, "BookTransType", "")
                    oAP.BookShipCarrierNumber = DTran.getDataRowString(APRow, "BookShipCarrierNumber", "")
                    oAP.APTaxDetail1 = DTran.NZ(APRow, "APTaxDetail1", 0)
                    oAP.APTaxDetail2 = DTran.NZ(APRow, "APTaxDetail2", 0)
                    oAP.APTaxDetail3 = DTran.NZ(APRow, "APTaxDetail3", 0)
                    oAP.APTaxDetail4 = DTran.NZ(APRow, "APTaxDetail4", 0)
                    oAP.APTaxDetail5 = DTran.NZ(APRow, "APTaxDetail5", 0)
                    oAPExport(intCt) = oAP
                    intCt += 1
                    If intCt > oAPET.Rows.Count Then Exit For
                Next
    End If

    'If Not oAPETD Is Nothing AndAlso Not oAPExportDetails Is Nothing Then
    If Not oAPETD Is Nothing Then
    If oAPETD.Rows.Count > 0 Then
    ReDim oAPExportDetails(oAPETD.Rows.Count - 1)
    End If
    Dim intCt As Integer = 0
    'For intCt As Integer = 0 To oAPET.Rows.Count - 1
    For Each APRow As APExport.APItemDetailsRow In oAPETD

    Dim oAPD As New clsAPExportDetailObject
    With oAPD
    .APControl = DTran.NZ(APRow, "APControl", 0)
    .BFC = DTran.NZ(APRow, "BFC", 0)
    .Brand = DTran.getDataRowString(APRow, "Brand", "")
    .CostCenter = DTran.getDataRowString(APRow, "CostCenter", "")
    .CountryOfOrigin = DTran.getDataRowString(APRow, "CountryOfOrigin", "")
    .Cube = DTran.NZ(APRow, "Cube", 0)
    .CustItemNumber = DTran.getDataRowString(APRow, "CustItemNumber", "")
    .CustomerNumber = DTran.getDataRowString(APRow, "CustomerNumber", "")
    .CustomerPONumber = DTran.getDataRowString(APRow, "CustomerPONumber", "")
    .Description = DTran.getDataRowString(APRow, "Description", "")
    .FreightCost = DTran.NZ(APRow, "FreightCost", 0)
    .GTIN = DTran.getDataRowString(APRow, "GTIN", "")
    .Hazmat = DTran.getDataRowString(APRow, "Hazmat", "")
    .HST = DTran.getDataRowString(APRow, "HST", "")
    .ItemCost = DTran.NZ(APRow, "ItemCost", 0)
    .ItemNumber = DTran.getDataRowString(APRow, "ItemNumber", "")
    .LotExpirationDate = exportDateToString(DTran.getDataRowString(APRow, "LotExpirationDate", ""))
    .LotNumber = DTran.getDataRowString(APRow, "LotNumber", "")
    .OrderNumber = DTran.getDataRowString(APRow, "OrderNumber", "")
    .OrderSequence = DTran.NZ(APRow, "OrderSequence", 0)
    .Pack = DTran.getDataRowString(APRow, "Pack", "")
    .QtyOrdered = DTran.NZ(APRow, "QtyOrdered", 0)
    .Size = DTran.getDataRowString(APRow, "Size", "")
    .Weight = DTran.NZ(APRow, "Weight", 0)
    .BookProNumber = DTran.getDataRowString(APRow, "BookProNumber", "")
    End With

                    oAPExportDetails(intCt) = oAPD
                    intCt += 1
                    If intCt > oAPETD.Rows.Count Then Exit For
                Next
    End If

    Catch ex As Exception
            oRet = ProcessDataReturnValues.nglDataIntegrationFailure
            LogException("AP Export Read Object Data Failure", "Could not read AP export records.", AdminEmail, ex, "NGL.FreightMaster.Integration.clsAPExport.readObjectData Failure")
        End Try
    Return oRet
    End Function

    Public Function readObjectData70(ByRef oAPExport() As clsAPExportObject70,
                                        ByVal strConnection As String,
                                        Optional ByVal MaxRetry As Integer = 0,
                                        Optional ByVal RetryMinutes As Integer = 0,
                                        Optional ByVal CompLegalEntity As String = Nothing,
                                        Optional ByRef oAPExportFees() As clsAPExportFeeObject70 = Nothing,
                                        Optional ByRef oAPExportDetails() As clsAPExportDetailObject70 = Nothing) As ProcessDataReturnValues

        Dim oAPET As New APExport.APExportDataDataTable
        Dim oAPETD As New APExport.APItemDetailsDataTable
        Dim oRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationComplete

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

            Dim oAPWCFData = New DAL.NGLAPMassEntryData(oWCFParameters)
            Dim oAPWCFRets = oAPWCFData.GetAPExportData70(MaxRetry, RetryMinutes, CompLegalEntity, Me.MaxRowsReturned)


            Dim oAPExportDet As New List(Of clsAPExportDetailObject70)
            Dim oAPExportFee As New List(Of clsAPExportFeeObject70)


            If Not oAPWCFRets Is Nothing AndAlso oAPWCFRets.Count > 0 Then
                ReDim oAPExport(oAPWCFRets.Count - 1)

                Dim intCt As Integer = 0

                For Each APRow In oAPWCFRets

                    Dim oAP As New clsAPExportObject70

                    oAP.APControl = APRow.APControl
                    oAP.BookSHID = APRow.BookSHID
                    oAP.CarrierNumber = APRow.CarrierNumber
                    oAP.CarrierAlphaCode = APRow.CarrierAlphaCode
                    oAP.CarrierLegalEntity = APRow.CarrierLegalEntity
                    oAP.CompLegalEntity = APRow.CompLegalEntity
                    oAP.CompanyNumber = APRow.CompanyNumber
                    oAP.CompAlphaCode = APRow.CompAlphaCode
                    oAP.CompNatNumber = APRow.CompNatNumber
                    oAP.LaneLegalEntity = APRow.LaneLegalEntity
                    oAP.LaneNumber = APRow.LaneNumber
                    oAP.BookCarrOrderNumber = APRow.BookCarrOrderNumber
                    oAP.BookOrderSequence = APRow.BookOrderSequence
                    oAP.BookConsPrefix = APRow.BookConsPrefix
                    oAP.BookRouteConsFlag = APRow.BookRouteConsFlag
                    oAP.BookProNumber = APRow.BookProNumber
                    oAP.BookShipCarrierProNumber = APRow.BookShipCarrierProNumber
                    oAP.BookShipCarrierNumber = APRow.BookShipCarrierNumber
                    oAP.BookShipCarrierName = APRow.BookShipCarrierName
                    oAP.BookShipCarrierDetails = APRow.BookShipCarrierDetails
                    oAP.BookFinAPBillNumber = APRow.BookFinAPBillNumber
                    oAP.BookFinAPBillNoDate = exportDateToString(APRow.BookFinAPBillNoDate)
                    oAP.BookFinAPBillInvDate = exportDateToString(APRow.BookFinAPBillInvDate)
                    oAP.BookFinAPActWgt = APRow.BookFinAPActWgt
                    oAP.BookFinAPStdCost = APRow.BookFinAPStdCost
                    oAP.BookFinAPACtCost = APRow.BookFinAPACtCost
                    oAP.BookFinAPActTax = APRow.BookFinAPActTax
                    oAP.BookFinAPTotalTaxableFees = APRow.BookFinAPTotalTaxableFees
                    oAP.BookFinAPTotalTaxes = APRow.BookFinAPTotalTaxes
                    oAP.BookFinAPNonTaxableFees = APRow.BookFinAPNonTaxableFees
                    oAP.BookCarrBLNumber = APRow.BookFinAPBillNumber
                    oAP.BookFinAPExportRetry = APRow.BookFinAPExportRetry
                    oAP.BookItemCostCenterNumber = APRow.BookItemCostCenterNumber
                    oAP.BookFinAPExportDate = exportDateToString(APRow.BookFinAPExportDate)
                    oAP.PrevSentDate = exportDateToString(APRow.PrevSentDate)
                    oAP.CarrierEquipmentCodes = APRow.CarrierEquipmentCodes
                    oAP.BookCarrierTypeCode = APRow.BookCarrierTypeCode
                    oAP.APFee1 = APRow.APFee1
                    oAP.APFee2 = APRow.APFee2
                    oAP.APFee3 = APRow.APFee3
                    oAP.APFee4 = APRow.APFee4
                    oAP.APFee5 = APRow.APFee5
                    oAP.APFee6 = APRow.APFee6
                    oAP.OtherCosts = APRow.OtherCosts
                    oAP.BookWarehouseNumber = APRow.BookWarehouseNumber
                    oAP.BookWhseAuthorizationNo = APRow.BookWhseAuthorizationNo
                    oAP.BookMilesFrom = APRow.BookMilesFrom
                    oAP.BookReasonCode = APRow.BookReasonCode
                    oAP.BookTransType = APRow.BookTransType
                    oAP.APTaxDetail1 = APRow.APTaxDetail1
                    oAP.APTaxDetail2 = APRow.APTaxDetail2
                    oAP.APTaxDetail3 = APRow.APTaxDetail3
                    oAP.APTaxDetail4 = APRow.APTaxDetail4
                    oAP.APTaxDetail5 = APRow.APTaxDetail5
                    oAP.BookFinAPGLNumber = APRow.BookFinAPGLNumber
                    oAPExport(intCt) = oAP

                    Dim BookControl As Integer = APRow.BookControl
                    Dim hdrAPControl As Integer = oAP.APControl
                    Dim hdrCompNumber As Integer = oAP.CompanyNumber
                    Dim hdrCompAlphaCode As String = oAP.CompAlphaCode
                    Dim hdrCompLegalEntity As String = oAP.CompLegalEntity
                    Dim hdrCompNatNumber As Integer = oAP.CompNatNumber

                    Dim oAPWCFDetails = oAPWCFData.GetExportDetailRows70(BookControl)

                    If Not oAPWCFDetails Is Nothing AndAlso oAPWCFDetails.Count > 0 Then
                        For Each d In oAPWCFDetails
                            Dim nDetail As New clsAPExportDetailObject70
                            With nDetail
                                .APControl = hdrAPControl
                                ' from clsIntegrationItemDetailObject'
                                .ItemNumber = d.ItemNumber
                                .QtyOrdered = d.QtyOrdered
                                .FreightCost = d.FreightCost
                                .ItemCost = d.ItemCost
                                .Weight = d.BookItemWeight
                                .Cube = d.BookItemCube
                                .Pack = d.Pack
                                .Size = d.Size
                                .Description = d.BookItemDescription
                                .Hazmat = d.Hazmat
                                .Brand = d.Brand
                                .CostCenter = d.CostCenter
                                .LotNumber = d.LotNumber
                                .LotExpirationDate = d.LotExpirationDate
                                .GTIN = d.GTIN
                                .CustItemNumber = d.CustItemNumber
                                .BFC = d.BFC
                                .CountryOfOrigin = d.CountryOfOrigin
                                .HST = d.HST
                                .PalletType = d.PalletType
                                .HazmatTypeCode = d.HazmatTypeCode
                                .Hazmat49CFRCode = d.Hazmat49CFRCode
                                .IATACode = d.IATACode
                                .DOTCode = d.DOTCode
                                .MarineCode = d.MarineCode
                                .NMFCClass = d.NMFCClass
                                .FAKClass = d.FAKClass
                                .LimitedQtyFlag = d.LimitedQtyFlag
                                .Pallets = d.Pallets
                                .Ties = d.Ties
                                .Highs = d.Highs
                                .QtyPalletPercentage = d.QtyPalletPercentage
                                .QtyLength = d.QtyLength
                                .QtyWidth = d.QtyWidth
                                .QtyHeight = d.QtyHeight
                                .Stackable = d.Stackable
                                .LevelOfDensity = d.LevelOfDensity
                                .CustomerPONumber = d.CustomerPONumber
                                'from clsIntegrationItemDetailObject70'
                                .CompLegalEntity = hdrCompLegalEntity
                                .CustomerNumber = hdrCompNumber
                                .CompAlphaCode = hdrCompAlphaCode
                                .BookCarrOrderNumber = d.BookCarrOrderNumber
                                .OrderSequence = d.BookOrderSequence
                                .BookProNumber = d.BookProNumber
                                .BookItemDiscount = d.BookItemDiscount
                                .BookItemLineHaul = d.BookItemLineHaul
                                .BookItemTaxableFees = d.BookItemTaxableFees
                                .BookItemTaxes = d.BookItemTaxes
                                .BookItemNonTaxableFees = d.BookItemNonTaxableFees
                                .BookItemWeightBreak = d.BookItemWeightBreak
                                .BookItemRated49CFRCode = d.BookItemRated49CFRCode
                                .BookItemRatedIATACode = d.BookItemRatedIATACode
                                .BookItemRatedDOTCode = d.BookItemRatedDOTCode
                                .BookItemRatedMarineCode = d.BookItemRatedMarineCode
                                .BookItemRatedNMFCClass = d.BookItemRatedNMFCClass
                                .BookItemRatedNMFCSubClass = d.BookItemRatedNMFCSubClass
                                .BookItemRatedFAKClass = d.BookItemRatedFAKClass
                                .CompNatNumber = hdrCompNatNumber
                                .OrderNumber = d.BookCarrOrderNumber
                            End With
                            oAPExportDet.Add(nDetail)
                        Next
                    End If

                    Dim oAPWCFFees = oAPWCFData.GetExportFeeRows70(BookControl)

                    If Not oAPWCFFees Is Nothing AndAlso oAPWCFFees.Count > 0 Then

                        For Each f In oAPWCFFees
                            Dim nFee As New clsAPExportFeeObject70
                            With nFee
                                .APControl = hdrAPControl
                                .AccessorialCode = f.AccessorialCode
                                .AccessorialName = f.AccessorialName
                                .AccessorialDescription = f.AccessorialDescription
                                .AccessorialCaption = f.AccessorialCaption
                                .AccessorialAlphaCode = f.AccessorialAlphaCode
                                .AccessorialEDICode = f.AccessorialEDICode
                                .AccessorialTaxable = f.AccessorialTaxable
                                .AccessorialTaxSortOrder = f.AccessorialTaxSortOrder
                                .AccessorialIsTax = f.AccessorialIsTax
                                .AccessorialBOLText = f.AccessorialBOLText
                                .AccessorialBOLPlacement = f.AccessorialBOLPlacement
                                .AccessorialAmount = f.AccessorialAmount
                                .AccessorialGroupType = f.AccessorialGroupType
                            End With
                            oAPExportFee.Add(nFee)
                        Next
                    End If
                    Try
                        If Me.AutoConfirmation Then
                            If UpdateStatus(Nothing, oAP.APControl, Nothing, Nothing, Nothing, Nothing, True, oAP.BookFinAPExportRetry, Now, Nothing) Then
                                Log("AP Export Auto Confirmation Complete For AP Control " & oAP.APControl & ".")
                            End If
                        Else
                            'just update the date and retry number
                            UpdateStatus(Nothing, oAP.APControl, Nothing, Nothing, Nothing, Nothing, Nothing, oAP.BookFinAPExportRetry, Now, Nothing)
                        End If
                    Catch ex As System.ApplicationException
                        'Log the exception and move on to the next record
                        LogException("AP Export Update Status Error (duplicate export possible)", "Could not update the export status for AP Control number " & (oAP.APControl).ToString & ".", AdminEmail, ex, "NGL.FreightMaster.Integration.clsAPExport.readObjectData70 Failure")
                    Catch ex As Exception
                        'this is an unexpected error so re throw it
                        Throw
                    End Try

                    intCt += 1
                    If intCt > oAPWCFRets.Count Then Exit For
                Next
            End If
            'Use the list function toArray to populate the clsAPExportDetailObject70 array object'
            oAPExportDetails = oAPExportDet.ToArray()
            oAPExportFees = oAPExportFee.ToArray()


            'Catch ex As DAL.FaultException(Of SqlFaultInfo)
        Catch ex As Exception
            oRet = ProcessDataReturnValues.nglDataIntegrationFailure
            LogException("AP Export Read Object Data 70 Failure", "Could not read AP export records.", AdminEmail, ex, "NGL.FreightMaster.Integration.clsAPExport.readObjectData70 Failure")
        End Try
        Return oRet
    End Function

    ''' <summary>
    ''' Red the AP Export Data from the database using DAL methods
    ''' </summary>
    ''' <param name="oAPExport"></param>
    ''' <param name="strConnection"></param>
    ''' <param name="MaxRetry"></param>
    ''' <param name="RetryMinutes"></param>
    ''' <param name="CompLegalEntity"></param>
    ''' <param name="oAPExportFees"></param>
    ''' <param name="oAPExportDetails"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR v-8.2.0.117 7/17/2019
    '''   replaces the 70 version Of the data
    '''   includes BookItemOrderNumber
    ''' </remarks>
    Public Function readObjectData80(ByRef oAPExport() As clsAPExportObject80,
                                        ByVal strConnection As String,
                                        Optional ByVal MaxRetry As Integer = 0,
                                        Optional ByVal RetryMinutes As Integer = 0,
                                        Optional ByVal CompLegalEntity As String = Nothing,
                                        Optional ByRef oAPExportFees() As clsAPExportFeeObject80 = Nothing,
                                        Optional ByRef oAPExportDetails() As clsAPExportDetailObject80 = Nothing) As ProcessDataReturnValues

        Dim oAPET As New APExport.APExportDataDataTable
        Dim oAPETD As New APExport.APItemDetailsDataTable
        Dim oRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationComplete

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

            Dim oAPWCFData = New DAL.NGLAPMassEntryData(oWCFParameters)
            Dim oAPWCFRets = oAPWCFData.GetAPExportData70(MaxRetry, RetryMinutes, CompLegalEntity, Me.MaxRowsReturned)


            Dim oAPExportDet As New List(Of clsAPExportDetailObject80)
            Dim oAPExportFee As New List(Of clsAPExportFeeObject80)


            If Not oAPWCFRets Is Nothing AndAlso oAPWCFRets.Count > 0 Then
                ReDim oAPExport(oAPWCFRets.Count - 1)

                Dim intCt As Integer = 0

                For Each APRow In oAPWCFRets

                    Dim oAP As New clsAPExportObject80

                    oAP.APControl = APRow.APControl
                    oAP.BookSHID = APRow.BookSHID
                    oAP.CarrierNumber = APRow.CarrierNumber
                    oAP.CarrierAlphaCode = APRow.CarrierAlphaCode
                    oAP.CarrierLegalEntity = APRow.CarrierLegalEntity
                    oAP.CompLegalEntity = APRow.CompLegalEntity
                    oAP.CompanyNumber = APRow.CompanyNumber
                    oAP.CompAlphaCode = APRow.CompAlphaCode
                    oAP.CompNatNumber = APRow.CompNatNumber
                    oAP.LaneLegalEntity = APRow.LaneLegalEntity
                    oAP.LaneNumber = APRow.LaneNumber
                    oAP.BookCarrOrderNumber = APRow.BookCarrOrderNumber
                    oAP.BookOrderSequence = APRow.BookOrderSequence
                    oAP.BookConsPrefix = APRow.BookConsPrefix
                    oAP.BookRouteConsFlag = APRow.BookRouteConsFlag
                    oAP.BookProNumber = APRow.BookProNumber
                    oAP.BookShipCarrierProNumber = APRow.BookShipCarrierProNumber
                    oAP.BookShipCarrierNumber = APRow.BookShipCarrierNumber
                    oAP.BookShipCarrierName = APRow.BookShipCarrierName
                    oAP.BookShipCarrierDetails = APRow.BookShipCarrierDetails
                    oAP.BookFinAPBillNumber = APRow.BookFinAPBillNumber
                    oAP.BookFinAPBillNoDate = exportDateToString(APRow.BookFinAPBillNoDate)
                    oAP.BookFinAPBillInvDate = exportDateToString(APRow.BookFinAPBillInvDate)
                    oAP.BookFinAPActWgt = APRow.BookFinAPActWgt
                    oAP.BookFinAPStdCost = APRow.BookFinAPStdCost
                    oAP.BookFinAPACtCost = APRow.BookFinAPACtCost
                    oAP.BookFinAPActTax = APRow.BookFinAPActTax
                    oAP.BookFinAPTotalTaxableFees = APRow.BookFinAPTotalTaxableFees
                    oAP.BookFinAPTotalTaxes = APRow.BookFinAPTotalTaxes
                    oAP.BookFinAPNonTaxableFees = APRow.BookFinAPNonTaxableFees
                    oAP.BookCarrBLNumber = APRow.BookFinAPBillNumber
                    oAP.BookFinAPExportRetry = APRow.BookFinAPExportRetry
                    oAP.BookItemCostCenterNumber = APRow.BookItemCostCenterNumber
                    oAP.BookFinAPExportDate = exportDateToString(APRow.BookFinAPExportDate)
                    oAP.PrevSentDate = exportDateToString(APRow.PrevSentDate)
                    oAP.CarrierEquipmentCodes = APRow.CarrierEquipmentCodes
                    oAP.BookCarrierTypeCode = APRow.BookCarrierTypeCode
                    oAP.APFee1 = APRow.APFee1
                    oAP.APFee2 = APRow.APFee2
                    oAP.APFee3 = APRow.APFee3
                    oAP.APFee4 = APRow.APFee4
                    oAP.APFee5 = APRow.APFee5
                    oAP.APFee6 = APRow.APFee6
                    oAP.OtherCosts = APRow.OtherCosts
                    oAP.BookWarehouseNumber = APRow.BookWarehouseNumber
                    oAP.BookWhseAuthorizationNo = APRow.BookWhseAuthorizationNo
                    oAP.BookMilesFrom = APRow.BookMilesFrom
                    oAP.BookReasonCode = APRow.BookReasonCode
                    oAP.BookTransType = APRow.BookTransType
                    oAP.APTaxDetail1 = APRow.APTaxDetail1
                    oAP.APTaxDetail2 = APRow.APTaxDetail2
                    oAP.APTaxDetail3 = APRow.APTaxDetail3
                    oAP.APTaxDetail4 = APRow.APTaxDetail4
                    oAP.APTaxDetail5 = APRow.APTaxDetail5
                    oAP.BookFinAPGLNumber = APRow.BookFinAPGLNumber
                    oAP.APReduction = APRow.APReduction
                    oAP.APReductionReason = APRow.APReductionReason
                    oAP.APReductionAdjustedCost = APRow.APReductionAdjustedCost
                    oAPExport(intCt) = oAP

                    Dim BookControl As Integer = APRow.BookControl
                    Dim hdrAPControl As Integer = oAP.APControl
                    Dim hdrCompNumber As Integer = oAP.CompanyNumber
                    Dim hdrCompAlphaCode As String = oAP.CompAlphaCode
                    Dim hdrCompLegalEntity As String = oAP.CompLegalEntity
                    Dim hdrCompNatNumber As Integer = oAP.CompNatNumber

                    Dim oAPWCFDetails = oAPWCFData.GetExportDetailRows80(BookControl)

                    If Not oAPWCFDetails Is Nothing AndAlso oAPWCFDetails.Count > 0 Then
                        For Each d In oAPWCFDetails
                            Dim nDetail As New clsAPExportDetailObject80
                            With nDetail
                                .APControl = hdrAPControl
                                ' from clsIntegrationItemDetailObject'
                                .ItemNumber = d.ItemNumber
                                .QtyOrdered = d.QtyOrdered
                                .FreightCost = d.FreightCost
                                .ItemCost = d.ItemCost
                                .Weight = d.BookItemWeight
                                .Cube = d.BookItemCube
                                .Pack = d.Pack
                                .Size = d.Size
                                .Description = d.BookItemDescription
                                .Hazmat = d.Hazmat
                                .Brand = d.Brand
                                .CostCenter = d.CostCenter
                                .LotNumber = d.LotNumber
                                .LotExpirationDate = d.LotExpirationDate
                                .GTIN = d.GTIN
                                .CustItemNumber = d.CustItemNumber
                                .BFC = d.BFC
                                .CountryOfOrigin = d.CountryOfOrigin
                                .HST = d.HST
                                .PalletType = d.PalletType
                                .HazmatTypeCode = d.HazmatTypeCode
                                .Hazmat49CFRCode = d.Hazmat49CFRCode
                                .IATACode = d.IATACode
                                .DOTCode = d.DOTCode
                                .MarineCode = d.MarineCode
                                .NMFCClass = d.NMFCClass
                                .FAKClass = d.FAKClass
                                .LimitedQtyFlag = d.LimitedQtyFlag
                                .Pallets = d.Pallets
                                .Ties = d.Ties
                                .Highs = d.Highs
                                .QtyPalletPercentage = d.QtyPalletPercentage
                                .QtyLength = d.QtyLength
                                .QtyWidth = d.QtyWidth
                                .QtyHeight = d.QtyHeight
                                .Stackable = d.Stackable
                                .LevelOfDensity = d.LevelOfDensity
                                .CustomerPONumber = d.CustomerPONumber
                                'from clsIntegrationItemDetailObject70'
                                .CompLegalEntity = hdrCompLegalEntity
                                .CustomerNumber = hdrCompNumber
                                .CompAlphaCode = hdrCompAlphaCode
                                .BookCarrOrderNumber = d.BookCarrOrderNumber
                                .OrderSequence = d.BookOrderSequence
                                .BookProNumber = d.BookProNumber
                                .BookItemDiscount = d.BookItemDiscount
                                .BookItemLineHaul = d.BookItemLineHaul
                                .BookItemTaxableFees = d.BookItemTaxableFees
                                .BookItemTaxes = d.BookItemTaxes
                                .BookItemNonTaxableFees = d.BookItemNonTaxableFees
                                .BookItemWeightBreak = d.BookItemWeightBreak
                                .BookItemRated49CFRCode = d.BookItemRated49CFRCode
                                .BookItemRatedIATACode = d.BookItemRatedIATACode
                                .BookItemRatedDOTCode = d.BookItemRatedDOTCode
                                .BookItemRatedMarineCode = d.BookItemRatedMarineCode
                                .BookItemRatedNMFCClass = d.BookItemRatedNMFCClass
                                .BookItemRatedNMFCSubClass = d.BookItemRatedNMFCSubClass
                                .BookItemRatedFAKClass = d.BookItemRatedFAKClass
                                .CompNatNumber = hdrCompNatNumber
                                .OrderNumber = d.BookCarrOrderNumber
                                .BookItemOrderNumber = d.BookItemOrderNumber
                            End With
                            oAPExportDet.Add(nDetail)
                        Next
                    End If

                    Dim oAPWCFFees = oAPWCFData.GetExportFeeRows70(BookControl)

                    If Not oAPWCFFees Is Nothing AndAlso oAPWCFFees.Count > 0 Then

                        For Each f In oAPWCFFees
                            Dim nFee As New clsAPExportFeeObject80
                            With nFee
                                .APControl = hdrAPControl
                                .AccessorialCode = f.AccessorialCode
                                .AccessorialName = f.AccessorialName
                                .AccessorialDescription = f.AccessorialDescription
                                .AccessorialCaption = f.AccessorialCaption
                                .AccessorialAlphaCode = f.AccessorialAlphaCode
                                .AccessorialEDICode = f.AccessorialEDICode
                                .AccessorialTaxable = f.AccessorialTaxable
                                .AccessorialTaxSortOrder = f.AccessorialTaxSortOrder
                                .AccessorialIsTax = f.AccessorialIsTax
                                .AccessorialBOLText = f.AccessorialBOLText
                                .AccessorialBOLPlacement = f.AccessorialBOLPlacement
                                .AccessorialAmount = f.AccessorialAmount
                                .AccessorialGroupType = f.AccessorialGroupType
                            End With
                            oAPExportFee.Add(nFee)
                        Next
                    End If
                    Try
                        If Me.AutoConfirmation Then
                            If UpdateStatus(Nothing, oAP.APControl, Nothing, Nothing, Nothing, Nothing, True, oAP.BookFinAPExportRetry, Now, Nothing) Then
                                Log("AP Export Auto Confirmation Complete For AP Control " & oAP.APControl & ".")
                            End If
                        Else
                            'just update the date and retry number
                            UpdateStatus(Nothing, oAP.APControl, Nothing, Nothing, Nothing, Nothing, Nothing, oAP.BookFinAPExportRetry, Now, Nothing)
                        End If
                    Catch ex As System.ApplicationException
                        'Log the exception and move on to the next record
                        LogException("AP Export Update Status Error (duplicate export possible)", "Could not update the export status for AP Control number " & (oAP.APControl).ToString & ".", AdminEmail, ex, "NGL.FreightMaster.Integration.clsAPExport.readObjectData80 Failure")
                    Catch ex As Exception
                        'this is an unexpected error so re throw it
                        Throw
                    End Try

                    intCt += 1
                    If intCt > oAPWCFRets.Count Then Exit For
                Next
            End If
            'Use the list function toArray to populate the clsAPExportDetailObject70 array object'
            oAPExportDetails = oAPExportDet.ToArray()
            oAPExportFees = oAPExportFee.ToArray()


            'Catch ex As DAL.FaultException(Of SqlFaultInfo)
        Catch ex As Exception
            oRet = ProcessDataReturnValues.nglDataIntegrationFailure
            LogException("AP Export Read Object Data 80 Failure", "Could not read AP export records.", AdminEmail, ex, "NGL.FreightMaster.Integration.clsAPExport.readObjectData80 Failure")
        End Try
        Return oRet
    End Function



    ''' <summary>
    ''' Read the AP Export Data from the database using DAL methods
    ''' </summary>
    ''' <param name="oAPExport"></param>
    ''' <param name="strConnection"></param>
    ''' <param name="MaxRetry"></param>
    ''' <param name="RetryMinutes"></param>
    ''' <param name="CompLegalEntity"></param>
    ''' <param name="oAPExportFees"></param>
    ''' <param name="oAPExportDetails"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR v-8.5.1.001 03/28/2022
    '''   replaces the 80 version Of the data
    '''   includes new Item Level allocation of cost for
    '''     LineHaul, Fuel and Fees
    ''' </remarks>
    Public Function readObjectData85(ByRef oAPExport() As clsAPExportObject85,
                                        ByVal strConnection As String,
                                        Optional ByVal MaxRetry As Integer = 0,
                                        Optional ByVal RetryMinutes As Integer = 0,
                                        Optional ByVal CompLegalEntity As String = Nothing,
                                        Optional ByRef oAPExportFees() As clsAPExportFeeObject85 = Nothing,
                                        Optional ByRef oAPExportDetails() As clsAPExportDetailObject85 = Nothing) As ProcessDataReturnValues

        Dim oAPET As New APExport.APExportDataDataTable
        Dim oAPETD As New APExport.APItemDetailsDataTable
        Dim oRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationComplete

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

            Dim oAPWCFData = New DAL.NGLAPMassEntryData(oWCFParameters)
            'The fields returned from GetAPExportData70 match the needed field for 8.5 so we can call GetAPExportData70
            Dim oAPWCFRets = oAPWCFData.GetAPExportData70(MaxRetry, RetryMinutes, CompLegalEntity, Me.MaxRowsReturned)


            Dim oAPExportDet As New List(Of clsAPExportDetailObject85)
            Dim oAPExportFee As New List(Of clsAPExportFeeObject85)


            If Not oAPWCFRets Is Nothing AndAlso oAPWCFRets.Count > 0 Then
                ReDim oAPExport(oAPWCFRets.Count - 1)

                Dim intCt As Integer = 0

                For Each APRow In oAPWCFRets

                    Dim oAP As New clsAPExportObject85

                    oAP.APControl = APRow.APControl
                    oAP.BookSHID = APRow.BookSHID
                    oAP.CarrierNumber = APRow.CarrierNumber
                    oAP.CarrierAlphaCode = APRow.CarrierAlphaCode
                    oAP.CarrierLegalEntity = APRow.CarrierLegalEntity
                    oAP.CompLegalEntity = APRow.CompLegalEntity
                    oAP.CompanyNumber = APRow.CompanyNumber
                    oAP.CompAlphaCode = APRow.CompAlphaCode
                    oAP.CompNatNumber = APRow.CompNatNumber
                    oAP.LaneLegalEntity = APRow.LaneLegalEntity
                    oAP.LaneNumber = APRow.LaneNumber
                    oAP.BookCarrOrderNumber = APRow.BookCarrOrderNumber
                    oAP.BookOrderSequence = APRow.BookOrderSequence
                    oAP.BookConsPrefix = APRow.BookConsPrefix
                    oAP.BookRouteConsFlag = APRow.BookRouteConsFlag
                    oAP.BookProNumber = APRow.BookProNumber
                    oAP.BookShipCarrierProNumber = APRow.BookShipCarrierProNumber
                    oAP.BookShipCarrierNumber = APRow.BookShipCarrierNumber
                    oAP.BookShipCarrierName = APRow.BookShipCarrierName
                    oAP.BookShipCarrierDetails = APRow.BookShipCarrierDetails
                    oAP.BookFinAPBillNumber = APRow.BookFinAPBillNumber
                    oAP.BookFinAPBillNoDate = exportDateToString(APRow.BookFinAPBillNoDate)
                    oAP.BookFinAPBillInvDate = exportDateToString(APRow.BookFinAPBillInvDate)
                    oAP.BookFinAPActWgt = APRow.BookFinAPActWgt
                    oAP.BookFinAPStdCost = APRow.BookFinAPStdCost
                    oAP.BookFinAPACtCost = APRow.BookFinAPACtCost
                    oAP.BookFinAPActTax = APRow.BookFinAPActTax
                    oAP.BookFinAPTotalTaxableFees = APRow.BookFinAPTotalTaxableFees
                    oAP.BookFinAPTotalTaxes = APRow.BookFinAPTotalTaxes
                    oAP.BookFinAPNonTaxableFees = APRow.BookFinAPNonTaxableFees
                    oAP.BookCarrBLNumber = APRow.BookFinAPBillNumber
                    oAP.BookFinAPExportRetry = APRow.BookFinAPExportRetry
                    oAP.BookItemCostCenterNumber = APRow.BookItemCostCenterNumber
                    oAP.BookFinAPExportDate = exportDateToString(APRow.BookFinAPExportDate)
                    oAP.PrevSentDate = exportDateToString(APRow.PrevSentDate)
                    oAP.CarrierEquipmentCodes = APRow.CarrierEquipmentCodes
                    oAP.BookCarrierTypeCode = APRow.BookCarrierTypeCode
                    oAP.APFee1 = APRow.APFee1
                    oAP.APFee2 = APRow.APFee2
                    oAP.APFee3 = APRow.APFee3
                    oAP.APFee4 = APRow.APFee4
                    oAP.APFee5 = APRow.APFee5
                    oAP.APFee6 = APRow.APFee6
                    oAP.OtherCosts = APRow.OtherCosts
                    oAP.BookWarehouseNumber = APRow.BookWarehouseNumber
                    oAP.BookWhseAuthorizationNo = APRow.BookWhseAuthorizationNo
                    oAP.BookMilesFrom = APRow.BookMilesFrom
                    oAP.BookReasonCode = APRow.BookReasonCode
                    oAP.BookTransType = APRow.BookTransType
                    oAP.APTaxDetail1 = APRow.APTaxDetail1
                    oAP.APTaxDetail2 = APRow.APTaxDetail2
                    oAP.APTaxDetail3 = APRow.APTaxDetail3
                    oAP.APTaxDetail4 = APRow.APTaxDetail4
                    oAP.APTaxDetail5 = APRow.APTaxDetail5
                    oAP.BookFinAPGLNumber = APRow.BookFinAPGLNumber
                    oAP.APReduction = APRow.APReduction
                    oAP.APReductionReason = APRow.APReductionReason
                    oAP.APReductionAdjustedCost = APRow.APReductionAdjustedCost
                    oAPExport(intCt) = oAP

                    Dim BookControl As Integer = APRow.BookControl
                    Dim hdrAPControl As Integer = oAP.APControl
                    Dim hdrCompNumber As Integer = oAP.CompanyNumber
                    Dim hdrCompAlphaCode As String = oAP.CompAlphaCode
                    Dim hdrCompLegalEntity As String = oAP.CompLegalEntity
                    Dim hdrCompNatNumber As Integer = oAP.CompNatNumber

                    Dim oAPWCFDetails = oAPWCFData.GetExportDetailRows85(BookControl)

                    If Not oAPWCFDetails Is Nothing AndAlso oAPWCFDetails.Count > 0 Then
                        For Each d In oAPWCFDetails
                            Dim nDetail As New clsAPExportDetailObject85
                            With nDetail
                                .APControl = hdrAPControl
                                ' from clsIntegrationItemDetailObject'
                                .ItemNumber = d.ItemNumber
                                .QtyOrdered = d.QtyOrdered
                                .FreightCost = d.FreightCost
                                .LineHaulCost = d.LineHaulCost
                                .FuelCost = d.FuelCost
                                .FeesCost = d.FeesCost
                                .ItemCost = d.ItemCost
                                .Weight = d.BookItemWeight
                                .Cube = d.BookItemCube
                                .Pack = d.Pack
                                .Size = d.Size
                                .Description = d.BookItemDescription
                                .Hazmat = d.Hazmat
                                .Brand = d.Brand
                                .CostCenter = d.CostCenter
                                .LotNumber = d.LotNumber
                                .LotExpirationDate = d.LotExpirationDate
                                .GTIN = d.GTIN
                                .CustItemNumber = d.CustItemNumber
                                .BFC = d.BFC
                                .CountryOfOrigin = d.CountryOfOrigin
                                .HST = d.HST
                                .PalletType = d.PalletType
                                .HazmatTypeCode = d.HazmatTypeCode
                                .Hazmat49CFRCode = d.Hazmat49CFRCode
                                .IATACode = d.IATACode
                                .DOTCode = d.DOTCode
                                .MarineCode = d.MarineCode
                                .NMFCClass = d.NMFCClass
                                .FAKClass = d.FAKClass
                                .LimitedQtyFlag = d.LimitedQtyFlag
                                .Pallets = d.Pallets
                                .Ties = d.Ties
                                .Highs = d.Highs
                                .QtyPalletPercentage = d.QtyPalletPercentage
                                .QtyLength = d.QtyLength
                                .QtyWidth = d.QtyWidth
                                .QtyHeight = d.QtyHeight
                                .Stackable = d.Stackable
                                .LevelOfDensity = d.LevelOfDensity
                                .CustomerPONumber = d.CustomerPONumber
                                'from clsIntegrationItemDetailObject70'
                                .CompLegalEntity = hdrCompLegalEntity
                                .CustomerNumber = hdrCompNumber
                                .CompAlphaCode = hdrCompAlphaCode
                                .BookCarrOrderNumber = d.BookCarrOrderNumber
                                .OrderSequence = d.BookOrderSequence
                                .BookProNumber = d.BookProNumber
                                .BookItemDiscount = d.BookItemDiscount
                                .BookItemLineHaul = d.BookItemLineHaul
                                .BookItemTaxableFees = d.BookItemTaxableFees
                                .BookItemTaxes = d.BookItemTaxes
                                .BookItemNonTaxableFees = d.BookItemNonTaxableFees
                                .BookItemWeightBreak = d.BookItemWeightBreak
                                .BookItemRated49CFRCode = d.BookItemRated49CFRCode
                                .BookItemRatedIATACode = d.BookItemRatedIATACode
                                .BookItemRatedDOTCode = d.BookItemRatedDOTCode
                                .BookItemRatedMarineCode = d.BookItemRatedMarineCode
                                .BookItemRatedNMFCClass = d.BookItemRatedNMFCClass
                                .BookItemRatedNMFCSubClass = d.BookItemRatedNMFCSubClass
                                .BookItemRatedFAKClass = d.BookItemRatedFAKClass
                                .CompNatNumber = hdrCompNatNumber
                                .OrderNumber = d.BookCarrOrderNumber
                                .BookItemOrderNumber = d.BookItemOrderNumber
                            End With
                            oAPExportDet.Add(nDetail)
                        Next
                    End If

                    Dim oAPWCFFees = oAPWCFData.GetExportFeeRows70(BookControl)

                    If Not oAPWCFFees Is Nothing AndAlso oAPWCFFees.Count > 0 Then

                        For Each f In oAPWCFFees
                            Dim nFee As New clsAPExportFeeObject85
                            With nFee
                                .APControl = hdrAPControl
                                .AccessorialCode = f.AccessorialCode
                                .AccessorialName = f.AccessorialName
                                .AccessorialDescription = f.AccessorialDescription
                                .AccessorialCaption = f.AccessorialCaption
                                .AccessorialAlphaCode = f.AccessorialAlphaCode
                                .AccessorialEDICode = f.AccessorialEDICode
                                .AccessorialTaxable = f.AccessorialTaxable
                                .AccessorialTaxSortOrder = f.AccessorialTaxSortOrder
                                .AccessorialIsTax = f.AccessorialIsTax
                                .AccessorialBOLText = f.AccessorialBOLText
                                .AccessorialBOLPlacement = f.AccessorialBOLPlacement
                                .AccessorialAmount = f.AccessorialAmount
                                .AccessorialGroupType = f.AccessorialGroupType
                            End With
                            oAPExportFee.Add(nFee)
                        Next
                    End If
                    Try
                        If Me.AutoConfirmation Then
                            If UpdateStatus(Nothing, oAP.APControl, Nothing, Nothing, Nothing, Nothing, True, oAP.BookFinAPExportRetry, Now, Nothing) Then
                                Log("AP Export Auto Confirmation Complete For AP Control " & oAP.APControl & ".")
                            End If
                        Else
                            'just update the date and retry number
                            UpdateStatus(Nothing, oAP.APControl, Nothing, Nothing, Nothing, Nothing, Nothing, oAP.BookFinAPExportRetry, Now, Nothing)
                        End If
                    Catch ex As System.ApplicationException
                        'Log the exception and move on to the next record
                        LogException("AP Export Update Status Error (duplicate export possible)", "Could not update the export status for AP Control number " & (oAP.APControl).ToString & ".", AdminEmail, ex, "NGL.FreightMaster.Integration.clsAPExport.readObjectData80 Failure")
                    Catch ex As Exception
                        'this is an unexpected error so re throw it
                        Throw
                    End Try

                    intCt += 1
                    If intCt > oAPWCFRets.Count Then Exit For
                Next
            End If
            'Use the list function toArray to populate the clsAPExportDetailObject70 array object'
            oAPExportDetails = oAPExportDet.ToArray()
            oAPExportFees = oAPExportFee.ToArray()


            'Catch ex As DAL.FaultException(Of SqlFaultInfo)
        Catch ex As Exception
            oRet = ProcessDataReturnValues.nglDataIntegrationFailure
            LogException("AP Export Read Object Data 85 Failure", "Could not read AP export records.", AdminEmail, ex, "NGL.FreightMaster.Integration.clsAPExport.readObjectData85 Failure")
        End Try
        Return oRet
    End Function


    ''' <summary>
    ''' Check the database for new AP Export records.
    ''' </summary>
    ''' <param name="oAPET">
    ''' An output parameter of APExport.APExportDataDataTable sql datatable records 
    ''' </param>
    ''' <param name="strConnection">
    ''' The required database connection string</param>
    ''' <param name="MaxRetry">
    ''' The maximum number or retries before alerts are raised.  
    ''' Works with RetryMinutes to allow delays and error reporting in export confirmations.</param>
    ''' <param name="RetryMinutes">
    ''' Enter the number of minutes before resending the AP Record if a confirmaiton has not been received.</param>
    ''' <param name="CompNumber">
    ''' Optional company number filter.  
    ''' Select records filtered by company number.</param>
    ''' <param name="OrderNumber">
    ''' Optional Order Number.  
    ''' Select records filtered by Order Number.  
    ''' To insure record uniqueness also provide a company number.</param>
    ''' <param name="OrderSequence">
    ''' Optional Order Sequence Number.  
    ''' Each order number can have multple legs or sections. 
    ''' Select records filtered by Order Number and Order Sequence.
    ''' Order Number must also be provided or the value is ignored.
    ''' To insuer record uniqueness also provide a company number.</param>
    ''' <param name="BookProNumber">
    ''' Optional NGL unique PRO Number.
    ''' Select records filtered by the Booking PRO Number.
    ''' Each BookProNumber is unique.  Always ensures that only one record is selected.</param>
    ''' <param name="oAPETD">
    ''' Optional Output parameter that exports item level details as APExport.APItemDetailsDataTable sql datatable records.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function readData(ByRef oAPET As APExport.APExportDataDataTable,
            ByVal strConnection As String,
            Optional ByVal MaxRetry As Integer = 0,
            Optional ByVal RetryMinutes As Integer = 0,
            Optional ByVal CompNumber As String = Nothing,
            Optional ByVal OrderNumber As String = Nothing,
            Optional ByVal OrderSequence As String = Nothing,
            Optional ByVal BookProNumber As String = Nothing,
            Optional ByRef oAPETD As APExport.APItemDetailsDataTable = Nothing) As ProcessDataReturnValues

        Dim enmRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strMsg As String = ""
        Dim strTitle As String = ""
        Dim intErrors As Integer = 0
        Dim intCount As Integer = 0
        Dim strSource As String = "clsAPExport.readData"
        Me.ImportTypeKey = IntegrationTypes.APExport
        Me.CreatedDate = Now.ToString
        Me.CreateUser = "Data Integration DLL"
        Me.Source = "AP Export Data Integration"
        Dim MaxRetryNbr As Nullable(Of Integer)
        Dim RetryMinutesNbr As Nullable(Of Integer)
        GroupEmailMsg = ""
        ITEmailMsg = ""
        Dim nintBookOrderSequence As Nullable(Of Integer)
        Dim intTest As Integer = 0
        If Not String.IsNullOrEmpty(OrderSequence) Then
            If Integer.TryParse(OrderSequence, intTest) Then
                nintBookOrderSequence = intTest
            Else
                'this is an invalid order sequence number so return false
                LastError = "The Book Order Sequence Number " & OrderSequence & " is not valid."
                Log(LastError)
                ITEmailMsg &= "<br /> The read AP Export Data command failed with the following data:" _
                    & "<br /> MaxRetry: " & MaxRetry.ToString _
                    & "<br /> RetryMinutes: " & RetryMinutes.ToString _
                    & "<br /> CompNumber: " & CompNumber _
                    & "<br /> OrderNumber: " & OrderNumber _
                    & "<br /> OrderSequence: " & OrderSequence _
                    & "<br /> BookProNumber: " & BookProNumber _
                    & "<br /> Error Message: " & LastError _
                    & "<br />" & vbCrLf
                LogError("Read AP Export Records Error Report", ITEmailMsg, AdminEmail)
                Return ProcessDataReturnValues.nglDataIntegrationFailure
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

            ' Read in the ap export records
            If Not GetData(oAPET, oAPETD, MaxRetryNbr, RetryMinutesNbr, CompNumber, OrderNumber, nintBookOrderSequence, BookProNumber) Then
                Return ProcessDataReturnValues.nglDataIntegrationFailure
            End If

            If GroupEmailMsg.Trim.Length > 0 Then
                LogError("AP Export Data Integration Warning", "The following warnings were reported while running the AP Export data integration routine.  If the maximum number of retries for a record has been reported the AP record was not exported and may only be exported manually by an administrator." & GroupEmailMsg, GroupEmail)
            End If
            If ITEmailMsg.Trim.Length > 0 Then
                LogError("AP Export Error Report", "The following errors were reported while running the AP Export data integration routine:" & ITEmailMsg, AdminEmail)
            End If
            If Me.TotalRecords > 0 Then
                strMsg = "Success! " & Me.TotalRecords & " AP records were exported." & vbCrLf
                enmRet = ProcessDataReturnValues.nglDataIntegrationComplete
                If Me.RecordErrors > 0 Or Me.ItemErrors > 0 Then
                    If Me.RecordErrors > 0 Then
                        strMsg &= " ERROR!  " & Me.RecordErrors & " AP records could not be exported.  Please check the email error report or database error log records for more information.<br />" & vbCrLf
                    End If
                    If Me.ItemErrors > 0 Then
                        strMsg &= " ERROR!  " & Me.ItemErrors & " AP Detail records could not be exported.  Please check the email error report or database error log records for more information." & vbCrLf
                    End If
                    enmRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                End If
            ElseIf StatusUpdateErrors > 0 Then
                strMsg = "WARNING! No AP records were available for export and " _
                    & StatusUpdateErrors & " AP records did not have their status flags updated.  Duplicate transmission of AP records is possible.  Please check the email error report or database error log records for more information."
                enmRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
            Else
                strMsg = "No AP records were available for export."
                enmRet = ProcessDataReturnValues.nglDataIntegrationComplete
            End If
            Log(strMsg)
        Catch ex As Exception
            LogException("AP Export Read Data Failure", "Could not read AP export records.", AdminEmail, ex, "NGL.FreightMaster.Integration.clsAPExport.readData Failure")
        Finally
            closeConnection()
        End Try

        Return enmRet
    End Function

    Public Function readObjectOpenPayables(ByRef oAPExport() As clsAPExportObject,
            ByVal strConnection As String,
            Optional ByVal CompNumber As String = Nothing) As ProcessDataReturnValues
        Dim oAPET As New APExport.APExportDataDataTable
        Dim oRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationComplete
        Try
            oRet = readOpenPayables(oAPET,
                    strConnection,
                    CompNumber)

            If Not oAPET Is Nothing Then
    If oAPET.Rows.Count > 0 Then
    ReDim oAPExport(oAPET.Rows.Count - 1)
    End If
    Dim intCt As Integer = 0
    'For intCt As Integer = 0 To oAPET.Rows.Count - 1
    For Each APRow As APExport.APExportDataRow In oAPET

    Dim oAP As New clsAPExportObject

                    oAP.BookCarrBLNumber = APRow.BookFinAPBillNumber
                    oAP.BookCarrOrderNumber = APRow.BookCarrOrderNumber
                    oAP.BookFinAPACtCost = APRow.BookFinAPActCost
                    oAP.BookFinAPActTax = APRow.BookFinAPActTax
                    oAP.BookFinAPActWgt = APRow.BookFinAPActWgt
                    oAP.BookFinAPBillInvDate = exportDateToString(APRow.BookFinAPBillInvDate.ToString)
                    oAP.BookFinAPBillNoDate = exportDateToString(APRow.BookFinAPBillNoDate.ToString)
                    oAP.BookFinAPBillNumber = APRow.BookFinAPBillNumber
                    oAP.BookFinAPExportDate = exportDateToString(APRow.BookFinAPExportDate.ToString)
                    oAP.BookFinAPExportRetry = APRow.BookFinAPExportRetry
                    oAP.BookItemCostCenterNumber = APRow.BookItemCostCenterNumber
                    oAP.BookProNumber = APRow.BookProNumber
                    oAP.CarrierNumber = APRow.CarrierNumber
                    oAP.LaneNumber = APRow.LaneNumber
                    oAP.PrevSentDate = exportDateToString(APRow.PrevSentDate.ToString)
                    oAPExport(intCt) = oAP
                    intCt += 1
                    If intCt > oAPET.Rows.Count Then Exit For
                Next
    End If

    Catch ex As Exception
            oRet = ProcessDataReturnValues.nglDataIntegrationFailure
            LogException("Read Open Payables Object Data Failure", "Could not export open payable records.", AdminEmail, ex, "NGL.FreightMaster.Integration.clsAPExport.readOObjectOpenPayables Failure")
        End Try
    Return oRet

    End Function

    Public Function readOpenPayables(ByRef oAPET As APExport.APExportDataDataTable,
            ByVal strConnection As String,
            Optional ByVal CompNumber As String = Nothing) As ProcessDataReturnValues
        Dim enmRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strMsg As String = ""
        Dim strTitle As String = ""
        Dim intErrors As Integer = 0
        Dim intCount As Integer = 0
        Dim strSource As String = "clsAPExport.readOpenPayables"
        Me.ImportTypeKey = IntegrationTypes.OpenPayables
        Me.CreatedDate = Now.ToString
        Me.CreateUser = "Data Integration DLL"
        Me.Source = "Open Payables Data Integration"
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
            ' Read in the open payable records
            If Not GetOpenPayables(oAPET, CompNumber) Then
                Return ProcessDataReturnValues.nglDataIntegrationFailure
            End If

            If GroupEmailMsg.Trim.Length > 0 Then
                LogError("Read Open Payables Data Failure", "The following errors were reported while attempting to read the open payables data." & GroupEmailMsg, GroupEmail)
            End If
            If ITEmailMsg.Trim.Length > 0 Then
                LogError("AP Export Error Report", "The following errors were reported while attempting to read the open payables data." & ITEmailMsg, AdminEmail)
            End If
            If Me.TotalRecords > 0 Then
                strMsg = "Success! " & Me.TotalRecords & " Open payable records were exported." & vbCrLf
                enmRet = ProcessDataReturnValues.nglDataIntegrationComplete
                If Me.RecordErrors > 0 Then
                    strMsg &= "<br />ERROR!  " & Me.RecordErrors & " Open payable records could not be exported.  Please check the email error report or database error log records for more information.<br />" & vbCrLf
                    enmRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                End If
            Else
                strMsg = "No open payable records were available for export."
                enmRet = ProcessDataReturnValues.nglDataIntegrationComplete
            End If
            Log(strMsg)
        Catch ex As Exception
            LogException("Read Open Payables Data Failure", "Could not export open payable records.", AdminEmail, ex, "NGL.FreightMaster.Integration.clsAPExport.readOpenPayables Failure")
        Finally
            closeConnection()
        End Try

        Return enmRet

    End Function

    Public Function addAPDetailRows(ByRef oAPDetails As APExport.APItemDetailsDataTable,
                ByVal BookProNumber As String,
                ByVal BookCarrOrderNumber As String,
                ByVal BookOrderSequence As String,
                ByVal CompNumber As String,
                ByVal APControl As Integer,
                ByVal CompNatNumber As Integer) As Boolean

        Dim Ret As Boolean = False
        Dim cmdObj As New System.Data.SqlClient.SqlCommand
        Dim strRet As String = ""
        Dim strSQL As String
        Dim drTemp As SqlDataReader
        Dim oCon As System.Data.SqlClient.SqlConnection

        Try
            oCon = getNewConnection()
            With cmdObj
                .Connection = oCon
                .CommandTimeout = Me.CommandTimeOut
            End With

            strSQL = "SELECT " _
                & " dbo.BookItem.BookItemItemNumber, " _
                & " dbo.BookItem.BookItemQtyOrdered, " _
                & " dbo.BookItem.BookItemFreightCost, " _
                & " dbo.BookItem.BookItemItemCost, " _
                & " dbo.BookItem.BookItemWeight, " _
                & " dbo.BookItem.BookItemCube, " _
                & " dbo.BookItem.BookItemPack, " _
                & " dbo.BookItem.BookItemSize, " _
                & " dbo.BookItem.BookItemDescription, " _
                & " dbo.BookItem.BookCustItemNumber," _
                & " dbo.BookItem.BookItemHazmat," _
                & " dbo.BookItem.BookItemBrand," _
                & " dbo.BookItem.BookItemCostCenter," _
                & " dbo.BookItem.BookItemLotNumber," _
                & " dbo.BookItem.BookItemLotExpirationDate," _
                & " dbo.BookItem.BookItemGTIN," _
                & " dbo.BookItem.BookItemBFC," _
                & " dbo.BookItem.BookItemCountryOfOrigin," _
                & " dbo.BookItem.BookItemHST," _
                & " isnull(dbo.PalletType.PalletType,'NA')" _
                & " FROM  dbo.Book INNER JOIN dbo.BookLoad ON dbo.Book.BookControl = dbo.BookLoad.BookLoadBookControl " _
                & " INNER JOIN dbo.BookItem ON dbo.BookLoad.BookLoadControl = dbo.BookItem.BookItemBookLoadControl " _
                & " LEFT OUTER JOIN dbo.PalletType ON dbo.BookItem.BookItemPalletTypeID = dbo.PalletType.ID " _
                & " WHERE dbo.Book.BookProNumber = '" & BookProNumber & "'"

            'If Debug Then
            '    Log("Select Pick list Details: " & strSQL)
            'End If
            With cmdObj
                .CommandText = strSQL
                .CommandType = CommandType.Text
                drTemp = .ExecuteReader()
            End With
            If drTemp.HasRows Then
                If oAPDetails Is Nothing Then
                    oAPDetails = New APExport.APItemDetailsDataTable
                End If
                Do While drTemp.Read()
                    Dim oAPDR As APExport.APItemDetailsRow = oAPDetails.NewAPItemDetailsRow
                    Dim ItemNumber As String = "0"
                    If Not drTemp.IsDBNull(0) Then
                        ItemNumber = drTemp.GetString(0)
                    End If
                    oAPDR.ItemNumber = ItemNumber
                    oAPDR.OrderNumber = BookCarrOrderNumber
                    If Not drTemp.IsDBNull(1) Then oAPDR.QtyOrdered = drTemp.GetInt32(1).ToString
                    If Not drTemp.IsDBNull(2) Then oAPDR.FreightCost = drTemp.GetSqlMoney(2).ToString
                    If Not drTemp.IsDBNull(3) Then oAPDR.ItemCost = drTemp.GetSqlMoney(3).ToString
                    If Not drTemp.IsDBNull(4) Then oAPDR.Weight = drTemp.GetSqlDouble(4).ToString
                    If Not drTemp.IsDBNull(5) Then oAPDR.Cube = drTemp.GetInt32(5).ToString
                    If Not drTemp.IsDBNull(6) Then oAPDR.Pack = drTemp.GetInt16(6).ToString
                    If Not drTemp.IsDBNull(7) Then oAPDR.Size = drTemp.GetString(7)
                    If Not drTemp.IsDBNull(8) Then oAPDR.Description = drTemp.GetString(8)
                    If Not drTemp.IsDBNull(9) Then oAPDR.CustItemNumber = drTemp.GetString(9)
                    If Not drTemp.IsDBNull(10) Then oAPDR.Hazmat = drTemp.GetString(10)
                    If Not drTemp.IsDBNull(11) Then oAPDR.Brand = drTemp.GetString(11)
                    If Not drTemp.IsDBNull(12) Then oAPDR.CostCenter = drTemp.GetString(12)
                    If Not drTemp.IsDBNull(13) Then oAPDR.LotNumber = drTemp.GetString(13)
                    If Not drTemp.IsDBNull(14) Then oAPDR.LotExpirationDate = drTemp.GetDateTime(14)
                    If Not drTemp.IsDBNull(15) Then oAPDR.GTIN = drTemp.GetString(15)
                    If Not drTemp.IsDBNull(16) Then oAPDR.BFC = drTemp.GetSqlMoney(16).ToString
                    If Not drTemp.IsDBNull(17) Then oAPDR.CountryOfOrigin = drTemp.GetString(17)
                    If Not drTemp.IsDBNull(18) Then oAPDR.HST = drTemp.GetString(18)
                    If Not drTemp.IsDBNull(19) Then oAPDR.PalletType = Left(drTemp.GetString(19), 10) '.Substring(0, 10)
                    oAPDR.CustomerNumber = CompNumber
                    oAPDR.APControl = APControl
                    oAPDR.OrderSequence = BookOrderSequence
                    oAPDR.BookProNumber = BookProNumber
                    oAPDR.CompNatNumber = CompNatNumber
                    oAPDetails.AddAPItemDetailsRow(oAPDR)
                    oAPDetails.AcceptChanges()
                Loop
            End If
            Ret = True
        Catch ex As SqlException
            ItemErrors += 1
            Throw New System.ApplicationException(ex.Errors(0).Number.ToString & " : " & ex.ToString, ex.InnerException)
        Catch ex As ApplicationException
            ItemErrors += 1
            Throw
        Catch ex As Exception
            ItemErrors += 1
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

            Try
#Disable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                oCon.Close()
#Enable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try
        Return Ret
    End Function

#End Region
End Class

Public Class APUnpaidFreightBills

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal sBookFinAPBillNumber As String, ByVal sCarrierAlphaCode As String, ByVal sCompLegalEntity As String, ByVal sCompAlphaCode As String, ByVal dActualCost As Decimal)
        MyBase.New()
        BookFinAPBillNumber = sBookFinAPBillNumber
        CarrierAlphaCode = sCarrierAlphaCode
        CompLegalEntity = sCompLegalEntity
        CompAlphaCode = sCompAlphaCode
        ActualCost = dActualCost
    End Sub

    Private __BookFinAPBillNumber As String
    Public Property BookFinAPBillNumber() As String
        Get
            Return __BookFinAPBillNumber
        End Get
        Set(ByVal value As String)
            __BookFinAPBillNumber = value
        End Set
    End Property


    Private _CarrierAlphaCode As String
    Public Property CarrierAlphaCode() As String
        Get
            Return _CarrierAlphaCode
        End Get
        Set(ByVal value As String)
            _CarrierAlphaCode = value
        End Set
    End Property

    Private _CompLegalEntity As String
    Public Property CompLegalEntity() As String
        Get
            Return _CompLegalEntity
        End Get
        Set(ByVal value As String)
            _CompLegalEntity = value
        End Set
    End Property

    Private _CompAlphaCode As String
    Public Property CompAlphaCode() As String
        Get
            Return _CompAlphaCode
        End Get
        Set(ByVal value As String)
            _CompAlphaCode = value
        End Set
    End Property

    Private _ActualCost As Decimal
    Public Property ActualCost() As Decimal
        Get
            Return _ActualCost
        End Get
        Set(ByVal value As Decimal)
            _ActualCost = value
        End Set
    End Property
End Class

