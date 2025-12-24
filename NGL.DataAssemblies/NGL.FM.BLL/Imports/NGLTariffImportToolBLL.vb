Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports DAL = NGL.FreightMaster.Data
Imports DTO = NGL.FreightMaster.Data.DataTransferObjects
Imports NGL.Core.ChangeTracker

'Added By LVV on 9/13/16 for v-7.0.5.110 Tariff Import Tool

Public Class NGLTariffImportToolBLL : Inherits BLLBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As DAL.WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Me.SourceClass = "NGLTariffImportTool"
    End Sub

#End Region

    Public Function ExecCloneContract(ByVal SelectedContractInfo As DTO.RateImportHeader) As DTO.RateImportHeader
        Dim retVal As DTO.RateImportHeader
        retVal = cloneContract(SelectedContractInfo)
        Return retVal
    End Function

    Public Function getLocalAndNetworkPathFolders() As DTO.WCFResults
        Dim wcfRet As New DTO.WCFResults
        Dim localPathFolder As String
        Dim networkPathFolder As String

        NGLParameterData.executeSQL("exec spAddNewParameter N'GlobalBulkInsertSQLServerFolder', 0, N'C:\Data\CSVImport\', N'Path for the local drive. For example, C:\Data\CSVImport\', 1, 1")
        NGLParameterData.executeSQL("exec spAddNewParameter N'GlobalBulkInsertNetworkShare', 0, N'C:\Data\CSVImport\', N'Path for the network share drive. For example, \\NGLSQLServer\CSVImport', 1, 1")

        localPathFolder = NGLParameterData.GetParText("GlobalBulkInsertSQLServerFolder")
        networkPathFolder = NGLParameterData.GetParText("GlobalBulkInsertNetworkShare")

        wcfRet.updateKeyFields("localPathFolder", localPathFolder)
        wcfRet.updateKeyFields("networkPathFolder", networkPathFolder)

        Return wcfRet
    End Function

    Public Sub deleteFromTmpCSVCarrierRates()
        Me.Parameters.ProcedureName = "ImportCSVData"
        'Me.Parameters.ValidateAccess = True

        Dim oSecData As New DAL.NGLSecurityDataProvider(Me.Parameters)

        Using PubsConn As New SqlConnection(oSecData.ConnectionString)
            PubsConn.Open()
            Using cmd As New SqlCommand("delete from tmpCSVCarrierRates", PubsConn)
                cmd.CommandTimeout = 3000
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    ' Added by CHA on 31/08/21 
    Public Sub deleteFromTmpSpreadsheetCarrierRates()
        Me.Parameters.ProcedureName = "ImportSpreadsheetData"

        Dim oSecData As New DAL.NGLSecurityDataProvider(Me.Parameters)

        Using PubsConn As New SqlConnection(oSecData.ConnectionString)
            PubsConn.Open()
            Using cmd As New SqlCommand($"delete from tmpSpreadsheetCarrierRates where UserControl = {Me.Parameters.UserControl}", PubsConn)
                cmd.CommandTimeout = 3000
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Sub ExecSPImportSpreadsheetData(ByVal tempTblName As String, ByRef rates As List(Of DTO.RateImportDetailData))
        ' Create a DataTable matching your target table structure
        Dim dt As New DataTable()
        dt.Columns.Add("UserControl", GetType(Integer))
        dt.Columns.Add("TarID", GetType(String))
        dt.Columns.Add("EquipName", GetType(String))
        dt.Columns.Add("EffDateFrom", GetType(DateTime))
        dt.Columns.Add("EffDateTo", GetType(DateTime))
        dt.Columns.Add("ClassTypeControl", GetType(Integer))
        dt.Columns.Add("RateTypeControl", GetType(Integer))
        dt.Columns.Add("BracketTypeControl", GetType(Integer))
        dt.Columns.Add("Country", GetType(String))
        dt.Columns.Add("State", GetType(String))
        dt.Columns.Add("City", GetType(String))
        dt.Columns.Add("FromZip", GetType(String))
        dt.Columns.Add("ToZip", GetType(String))
        dt.Columns.Add("Lane", GetType(Integer))
        dt.Columns.Add("Class", GetType(String))   ' Changed from TariffClass to Class
        dt.Columns.Add("Min", GetType(Decimal))
        dt.Columns.Add("MaxDays", GetType(Integer))
        dt.Columns.Add("Val1", GetType(Decimal))
        dt.Columns.Add("Val2", GetType(Decimal))
        dt.Columns.Add("Val3", GetType(Decimal))
        dt.Columns.Add("Val4", GetType(Decimal))
        dt.Columns.Add("Val5", GetType(Decimal))
        dt.Columns.Add("Val6", GetType(Decimal))
        dt.Columns.Add("Val7", GetType(Decimal))
        dt.Columns.Add("Val8", GetType(Decimal))
        dt.Columns.Add("Val9", GetType(Decimal))
        dt.Columns.Add("Val10", GetType(Decimal))
        dt.Columns.Add("OrigZip", GetType(String))   ' Changed from CarrTarEquipMatOrigZip to OrigZip

        For Each r As DTO.RateImportDetailData In rates
            Dim dr As DataRow = dt.NewRow()
            dr("UserControl") = Me.Parameters.UserControl  ' Assuming this is never null
            dr("TarID") = If(String.IsNullOrEmpty(r.TarID), DBNull.Value, r.TarID)
            dr("EquipName") = If(String.IsNullOrEmpty(r.EquipName), DBNull.Value, r.EquipName)
            dr("EffDateFrom") = If(r.EffDateFrom.HasValue, r.EffDateFrom.Value, DBNull.Value)
            dr("EffDateTo") = If(r.EffDateTo.HasValue, r.EffDateTo.Value, DBNull.Value)
            dr("ClassTypeControl") = r.ClassTypeControl
            dr("RateTypeControl") = r.RateTypeControl
            dr("BracketTypeControl") = r.BracketTypeControl
            dr("Country") = If(String.IsNullOrEmpty(r.Country), DBNull.Value, r.Country)
            dr("State") = If(String.IsNullOrEmpty(r.State), DBNull.Value, r.State)
            dr("City") = If(String.IsNullOrEmpty(r.City), DBNull.Value, r.City)
            dr("FromZip") = If(String.IsNullOrEmpty(r.FromZip), DBNull.Value, r.FromZip)
            dr("ToZip") = If(String.IsNullOrEmpty(r.ToZip), DBNull.Value, r.ToZip)
            dr("Lane") = r.Lane
            dr("Class") = If(String.IsNullOrEmpty(r.TariffClass), DBNull.Value, r.TariffClass)
            dr("Min") = r.Min
            dr("MaxDays") = r.MaxDays
            dr("Val1") = r.Val1
            dr("Val2") = r.Val2
            dr("Val3") = r.Val3
            dr("Val4") = r.Val4
            dr("Val5") = r.Val5
            dr("Val6") = r.Val6
            dr("Val7") = r.Val7
            dr("Val8") = r.Val8
            dr("Val9") = r.Val9
            dr("Val10") = r.Val10
            dr("OrigZip") = If(String.IsNullOrEmpty(r.CarrTarEquipMatOrigZip), DBNull.Value, r.CarrTarEquipMatOrigZip)
            dt.Rows.Add(dr)
        Next

        ' Insert the data using SqlBulkCopy
        Dim oSecData As New DAL.NGLSecurityDataProvider(Me.Parameters)
        Using conn As New SqlConnection(oSecData.ConnectionString)
            conn.Open()
            Using bulkCopy As New SqlBulkCopy(conn)
                bulkCopy.DestinationTableName = tempTblName

                ' Map the DataTable columns to the destination table columns
                bulkCopy.ColumnMappings.Add("UserControl", "UserControl")
                bulkCopy.ColumnMappings.Add("TarID", "TarID")
                bulkCopy.ColumnMappings.Add("EquipName", "EquipName")
                bulkCopy.ColumnMappings.Add("EffDateFrom", "EffDateFrom")
                bulkCopy.ColumnMappings.Add("EffDateTo", "EffDateTo")
                bulkCopy.ColumnMappings.Add("ClassTypeControl", "ClassTypeControl")
                bulkCopy.ColumnMappings.Add("RateTypeControl", "RateTypeControl")
                bulkCopy.ColumnMappings.Add("BracketTypeControl", "BracketTypeControl")
                bulkCopy.ColumnMappings.Add("Country", "Country")
                bulkCopy.ColumnMappings.Add("State", "State")
                bulkCopy.ColumnMappings.Add("City", "City")
                bulkCopy.ColumnMappings.Add("FromZip", "FromZip")
                bulkCopy.ColumnMappings.Add("ToZip", "ToZip")
                bulkCopy.ColumnMappings.Add("Lane", "Lane")
                bulkCopy.ColumnMappings.Add("Class", "Class")
                bulkCopy.ColumnMappings.Add("Min", "Min")
                bulkCopy.ColumnMappings.Add("MaxDays", "MaxDays")
                bulkCopy.ColumnMappings.Add("Val1", "Val1")
                bulkCopy.ColumnMappings.Add("Val2", "Val2")
                bulkCopy.ColumnMappings.Add("Val3", "Val3")
                bulkCopy.ColumnMappings.Add("Val4", "Val4")
                bulkCopy.ColumnMappings.Add("Val5", "Val5")
                bulkCopy.ColumnMappings.Add("Val6", "Val6")
                bulkCopy.ColumnMappings.Add("Val7", "Val7")
                bulkCopy.ColumnMappings.Add("Val8", "Val8")
                bulkCopy.ColumnMappings.Add("Val9", "Val9")
                bulkCopy.ColumnMappings.Add("Val10", "Val10")
                bulkCopy.ColumnMappings.Add("OrigZip", "OrigZip")

                bulkCopy.WriteToServer(dt)
            End Using
        End Using
    End Sub

    Public Sub ExecSPImportCSVData(ByVal localFilePathName As String, ByVal tempTblName As String)
        'import into temp table.
        Me.Parameters.ProcedureName = "ImportCSVData"
        'Me.Parameters.ValidateAccess = True

        Dim oSecData As New DAL.NGLSecurityDataProvider(Me.Parameters)

        Dim PubsConn As SqlConnection = New SqlConnection(oSecData.ConnectionString)
        Dim cmd As SqlCommand = New SqlCommand("spImportCSVData", PubsConn)
        cmd.CommandType = CommandType.StoredProcedure

        Dim tmpCSVDump As SqlParameter = cmd.Parameters.Add("@TableName", SqlDbType.NVarChar, 4000)
        tmpCSVDump.Value = tempTblName
        tmpCSVDump.Direction = ParameterDirection.Input

        Dim FilePath As SqlParameter = cmd.Parameters.Add("@FilePath", SqlDbType.NVarChar, 4000)
        'Modified by LVV 8/26/16 for v-7.0.5.110 Bulk Import Fix
        FilePath.Value = localFilePathName
        FilePath.Direction = ParameterDirection.Input

        Dim TruncateData As SqlParameter = cmd.Parameters.Add("@TruncateData", SqlDbType.Bit, 1)
        TruncateData.Value = 0
        TruncateData.Direction = ParameterDirection.Input

        Dim SelectData As SqlParameter = cmd.Parameters.Add("@SelectData", SqlDbType.Bit, 1)
        SelectData.Value = 0
        SelectData.Direction = ParameterDirection.Input
        Dim myReader As SqlDataReader = Nothing
        Try
            PubsConn.Open()
            cmd.CommandTimeout = 3000
            myReader = cmd.ExecuteReader()
            Do While myReader.Read
            Loop
            myReader.Close()
            PubsConn.Close()
        Catch ex As Exception
            Throw
        Finally
            Try
                If myReader IsNot Nothing AndAlso Not myReader.IsClosed Then
                    myReader.Close()
                End If
            Catch ex1 As Exception
                'do nothing
            End Try
            Try
                If PubsConn IsNot Nothing AndAlso PubsConn.State = ConnectionState.Open Then
                    PubsConn.Close()
                End If
            Catch ex As Exception
            End Try
        End Try
    End Sub

    Public Function ExecSPImportCSVRatesFromTmpTbl(ByVal ClonedContractInfo As DTO.RateImportHeader) As String
        Dim result As String = ""

        'if we made it this far we can contineu and import from temp table.
        Me.Parameters.ProcedureName = "ImportCSVData"
        'Me.Parameters.ValidateAccess = True

        Dim oSecData As New DAL.NGLSecurityDataProvider(Me.Parameters)

        Dim PubsConn As SqlConnection = New SqlConnection(oSecData.ConnectionString)

        Dim cmd As SqlCommand = New SqlCommand("spImportCSVRatesFromTmpTbl", PubsConn)
        cmd.CommandType = CommandType.StoredProcedure

        Dim CarrTarEquipMatCarrTarEquipControl As SqlParameter = cmd.Parameters.Add("@CarrTarEquipMatCarrTarEquipControl", SqlDbType.Int)
        CarrTarEquipMatCarrTarEquipControl.Value = ClonedContractInfo.CarrTarEquipControl
        CarrTarEquipMatCarrTarEquipControl.Direction = ParameterDirection.Input

        Dim CarrTarEquipMatCarrTarControl As SqlParameter = cmd.Parameters.Add("@CarrTarEquipMatCarrTarControl", SqlDbType.Int)
        CarrTarEquipMatCarrTarControl.Value = ClonedContractInfo.CarrTarControl
        CarrTarEquipMatCarrTarControl.Direction = ParameterDirection.Input

        Dim CarrTarEquipMatCarrTarMatBPControl As SqlParameter = cmd.Parameters.Add("@CarrTarEquipMatCarrTarMatBPControl", SqlDbType.Int)
        CarrTarEquipMatCarrTarMatBPControl.Value = ClonedContractInfo.CarrTarMatBPControl
        CarrTarEquipMatCarrTarMatBPControl.Direction = ParameterDirection.Input

        Dim ModUser As SqlParameter = cmd.Parameters.Add("@CarrTarEquipMatModUser", SqlDbType.NVarChar, 100)
        ModUser.Value = "RatesManualImport"
        ModUser.Direction = ParameterDirection.Input

        Dim CarrTarEquipMatName As SqlParameter = cmd.Parameters.Add("@CarrTarEquipMatName", SqlDbType.NVarChar, 50)
        CarrTarEquipMatName.Value = ClonedContractInfo.CarrTarEquipMatName
        CarrTarEquipMatName.Direction = ParameterDirection.Input

        Dim myReader As SqlDataReader = Nothing
        Try
            PubsConn.Open()
            cmd.CommandTimeout = 3000
            myReader = cmd.ExecuteReader()
            '0	0	Cannot import CSV rates from temp table because of an Invalid Break Point Control Number	1
            Dim success As Boolean = False
            Dim recordsprocessed As Integer = 0
            Dim retMessage As String = ""
            Do While myReader.Read
                success = myReader.GetBoolean(0)
                recordsprocessed = myReader.GetInt64(1)
                retMessage = myReader.GetString(2)
            Loop

            result = "Success: " & success.ToString() & vbCrLf &
                "Records Processed: " & recordsprocessed & vbCrLf &
                "Message: " & retMessage
            'MessageBox.Show(result)
            myReader.Close()
            PubsConn.Close()
        Catch ex As Exception
            Throw
        Finally
            Try
                If myReader IsNot Nothing AndAlso Not myReader.IsClosed Then
                    myReader.Close()
                End If
            Catch ex1 As Exception
                'do nothing
            End Try
            Try
                If PubsConn IsNot Nothing AndAlso PubsConn.State = ConnectionState.Open Then
                    PubsConn.Close()
                End If
            Catch ex As Exception
            End Try
        End Try
        Return result
    End Function

    Public Function ExecSPImportRatesFromSpreadsheetTable(ByVal ClonedContractInfo As DTO.RateImportHeader) As String
        Dim result As String = ""
        Dim success As Boolean = False
        Dim recordsProcessed As Long = 0
        Dim retMessage As String = ""

        Dim oSecData As New DAL.NGLSecurityDataProvider(Me.Parameters)

        Try
            Using PubsConn As New SqlConnection(oSecData.ConnectionString)
                Using cmd As New SqlCommand("spImportRatesFromSpreadsheetTable", PubsConn)
                    cmd.CommandType = CommandType.StoredProcedure

                    ' Add parameters more concisely
                    cmd.Parameters.AddWithValue("@CarrTarEquipMatCarrTarEquipControl", ClonedContractInfo.CarrTarEquipControl)
                    cmd.Parameters.AddWithValue("@CarrTarEquipMatCarrTarControl", ClonedContractInfo.CarrTarControl)
                    cmd.Parameters.AddWithValue("@CarrTarEquipMatCarrTarMatBPControl", ClonedContractInfo.CarrTarMatBPControl)
                    cmd.Parameters.AddWithValue("@UserControlNumber", Me.Parameters.UserControl)
                    cmd.Parameters.AddWithValue("@CarrTarEquipMatModUser", "RatesManualImport")
                    cmd.Parameters.AddWithValue("@CarrTarEquipMatName", ClonedContractInfo.CarrTarEquipMatName)

                    cmd.CommandTimeout = 3000

                    PubsConn.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            success = reader.GetBoolean(0)
                            recordsProcessed = reader.GetInt64(1)
                            retMessage = reader.GetString(2)
                        End If
                    End Using
                End Using
            End Using

            result = $"Success: {success}" & vbCrLf &
                 $"Records Processed: {recordsProcessed}" & vbCrLf &
                 $"Message: {retMessage}"
        Catch ex As Exception
            ' You can log the exception here or handle it as needed.
            Throw
        End Try

        Return result
    End Function

    Public Function GetCarrTarEquipMatPivotsByAltKey(ByVal AltDataKey As String) As DTO.CarrTarEquipMatPivot()
        Dim result As DTO.CarrTarEquipMatPivot() = NGLCarrTarEquipMatData.GetCarrTarEquipMatPivotsByAltKey(AltDataKey, 1, 1000000)
        Return result
    End Function

    Public Function getRatesDDList(ByVal AltDataKey As String,
                                 ByVal contract As DTO.CarrTarContract,
                                 ByVal SelectedContractInfo As DTO.RateImportHeader) As DTO.RateImportDetailData()
        Dim rates As New ChangeTrackingCollection(Of DTO.RateImportDetailData)
        Dim result As DTO.CarrTarEquipMatPivot() = NGLCarrTarEquipMatData.GetCarrTarEquipMatPivotsByAltKey(AltDataKey, 1, 1000000)
        If result Is Nothing Then
            'Modified By LVV on 9/7/16 for v-7.0.5.110 Export Bug Fix
            Dim rate As New DTO.RateImportDetailData
            rate.Success = False
            rate.Message = "Result returned null. " & vbCrLf & vbCrLf & "Make sure you selected the latest revision"
            rates.Add(rate)
            Return rates.ToArray()
        End If

        For Each row In result
            Dim rate As New DTO.RateImportDetailData
            rate.TarID = SelectedContractInfo.CarrTarID
            rate.EquipName = SelectedContractInfo.CarrTarEquipName
            rate.EffDateFrom = contract.CarrTarEffDateFrom
            rate.EffDateTo = contract.CarrTarEffDateTo
            rate.ClassTypeControl = SelectedContractInfo.ClassTypeControl
            rate.RateTypeControl = SelectedContractInfo.RateTypeControl
            rate.BracketTypeControl = SelectedContractInfo.BracketTypeControl
            rate.Country = row.CarrTarEquipMatCountry
            rate.State = row.CarrTarEquipMatState
            rate.City = row.CarrTarEquipMatCity
            rate.FromZip = row.CarrTarEquipMatFromZip
            rate.ToZip = row.CarrTarEquipMatToZip
            rate.Lane = row.CarrTarEquipMatLaneControl
            rate.TariffClass = row.CarrTarEquipMatClass
            'Modified By LVV on 9/7/16 for v-7.0.5.110 Export Bug Fix
            rate.Min = If(row.CarrTarEquipMatMin.HasValue, row.CarrTarEquipMatMin.Value, 0)
            rate.MaxDays = row.CarrTarEquipMatMaxDays
            rate.Val1 = If(row.Val1.HasValue, row.Val1.Value, 0)
            rate.Val2 = If(row.Val2.HasValue, row.Val2.Value, 0)
            rate.Val3 = If(row.Val3.HasValue, row.Val3.Value, 0)
            rate.Val4 = If(row.Val4.HasValue, row.Val4.Value, 0)
            rate.Val5 = If(row.Val5.HasValue, row.Val5.Value, 0)
            rate.Val6 = If(row.Val6.HasValue, row.Val6.Value, 0)
            rate.Val7 = If(row.Val7.HasValue, row.Val7.Value, 0)
            rate.Val8 = If(row.Val8.HasValue, row.Val8.Value, 0)
            rate.Val9 = If(row.Val9.HasValue, row.Val9.Value, 0)
            rate.Val10 = If(row.Val10.HasValue, row.Val10.Value, 0)
            rate.Success = True
            rate.CarrTarEquipMatOrigZip = row.CarrTarEquipMatOrigZip ' Modified by RHR for v-8.5.4.001 on 06/14/2023 new Origin Rate shop logic
            rates.Add(rate)
        Next
        Return rates.ToArray()
    End Function

    Public Function cloneContract(ByVal preClonedInfo As DTO.RateImportHeader) As DTO.RateImportHeader
        Dim results As DTO.GenericResults = Nothing
        Dim clonedRateImportHeader As DTO.RateImportHeader = New DTO.RateImportHeader()
        Dim newCarrTarControl As Integer = 0
        Dim newBPControl As Integer = 0
        Dim newEquipControl As Integer = 0

        results = NGLCarrTarContractData.CloneTariff(preClonedInfo.CarrTarControl,
                                                       preClonedInfo.EffectiveDate,
                                                       preClonedInfo.EffectiveTo,
                                                       True,
                                                       Nothing,
                                                       True,
                                                       True,
                                                       True,
                                                       True,
                                                       True,
                                                       True,
                                                       True,
                                                       True,
                                                       True,
                                                       True,
                                                       False,
                                                       True)
        If (results Is Nothing OrElse results.Control = 0 OrElse results.ErrNumber <> 0) Then
            If (results Is Nothing) Then
                clonedRateImportHeader.Success = False
                clonedRateImportHeader.Message = "Clone Failed, Unknown problem. results= null"
                Return clonedRateImportHeader
            Else
                clonedRateImportHeader.Success = False
                clonedRateImportHeader.Message = results.RetMsg
                Return clonedRateImportHeader
            End If
        Else
            'set new contract control
            newCarrTarControl = results.Control

            'get new equipment control and set it.
            'newEquipControl = NGLCarrTarEquipData.getNewestEquipControlUsingPreClonedValue(preClonedInfo.CarrTarEquipControl)
            newEquipControl = NGLCarrTarEquipData.getNewestEquipControlUsingNewCarrTarControl(newCarrTarControl)

            If (newEquipControl = 0) Then
                clonedRateImportHeader.Success = False
                clonedRateImportHeader.Message = "getNewestEquipControlUsingPreClonedValue Failed: precloneEquipControl: " + newEquipControl.ToString()
                Return clonedRateImportHeader
            End If

            'determine if we need to clone the breakpoints.
            'some type do not need cloning because they use defaults.
            'get the cloned breakpoint. 
            'set the breakpoint control
            Dim RateType As DAL.Utilities.TariffRateType = Me.getRateType(preClonedInfo.RateTypeControl)
            If (RateType = DAL.Utilities.TariffRateType.ClassRate OrElse RateType = DAL.Utilities.TariffRateType.UnitOfMeasure) Then
                If (preClonedInfo.CarrTarMatBPControl > 0) Then
                    newBPControl = NGLCarrTarEquipData.getNewestCarrTarMatBPControlUsingPreClonedValue(preClonedInfo.CarrTarMatBPControl)
                    If (newBPControl = 0) Then
                        clonedRateImportHeader.Success = False
                        clonedRateImportHeader.Message = "getNewestCarrTarMatBPControlUsingPreClonedValue Failed: precloneBPControl: " + preClonedInfo.CarrTarMatBPControl.ToString()
                        Return clonedRateImportHeader
                    End If
                End If
            Else
                'no need to clone distance breakpoints since there arent any break points.
                newBPControl = preClonedInfo.CarrTarMatBPControl
            End If

            clonedRateImportHeader.BracketTypeControl = preClonedInfo.BracketTypeControl
            clonedRateImportHeader.CarrierControl = preClonedInfo.CarrierControl
            clonedRateImportHeader.CarrTarControl = newCarrTarControl
            clonedRateImportHeader.CarrTarEquipControl = newEquipControl
            clonedRateImportHeader.CarrTarEquipMatName = preClonedInfo.CarrTarEquipMatName
            clonedRateImportHeader.CarrTarEquipName = preClonedInfo.CarrTarEquipName
            clonedRateImportHeader.CarrTarID = preClonedInfo.CarrTarID
            clonedRateImportHeader.CarrTarMatBPControl = newBPControl
            clonedRateImportHeader.ClassTypeControl = preClonedInfo.ClassTypeControl
            clonedRateImportHeader.CompanyControl = preClonedInfo.CompanyControl
            clonedRateImportHeader.EffectiveDate = preClonedInfo.EffectiveDate
            clonedRateImportHeader.EffectiveTo = preClonedInfo.EffectiveTo
            clonedRateImportHeader.RateTypeControl = preClonedInfo.RateTypeControl
            clonedRateImportHeader.Success = True
            Return clonedRateImportHeader
        End If

    End Function

    Public Function getRateType(ByVal rateType As Integer) As DAL.Utilities.TariffRateType
        Select Case rateType
            Case DAL.Utilities.TariffRateType.DistanceM
                Return DAL.Utilities.TariffRateType.DistanceM
            Case DAL.Utilities.TariffRateType.ClassRate
                Return DAL.Utilities.TariffRateType.ClassRate
            Case DAL.Utilities.TariffRateType.FlatRate
                Return DAL.Utilities.TariffRateType.FlatRate
            Case DAL.Utilities.TariffRateType.UnitOfMeasure
                Return DAL.Utilities.TariffRateType.UnitOfMeasure
            Case Else
                'should never get here.
                Return DAL.Utilities.TariffRateType.DistanceM
        End Select
    End Function


    ' Added by CHA on 31/08/21 
    Public Function ImportNewRates(ByVal SelectedContractInfo As DTO.RateImportHeader, ByRef rates As List(Of DTO.RateImportDetailData)) As DTO.WCFResults
        Dim oRet As New DTO.WCFResults()
        oRet.Success = False
        Dim str As String = ""
        If rates Is Nothing OrElse rates.Count = 0 Then Return oRet
        If Me.Parameters.UserControl = 0 Then Return oRet

        Try
            If SelectedContractInfo.CarrTarControl = 0 Then
                throwFieldRequiredException("Tariff Control", False, False)
            End If

            If SelectedContractInfo.CarrTarEquipControl = 0 OrElse SelectedContractInfo.RateTypeControl = 0 Then
                'the caller did not provide all the details so use defaults
                'Get the first equipment record
                Dim oEquip = NGLCarrTarEquipData.GetFirstvCarrierTariffService(SelectedContractInfo.CarrTarControl)
                If oEquip Is Nothing OrElse oEquip.CarrTarEquipControl = 0 Then
                    throwFieldRequiredException("Equipment Control", False, False)
                End If
                With SelectedContractInfo
                    .CarrTarEquipControl = oEquip.CarrTarEquipControl
                    .RateTypeControl = oEquip.CarrTarEquipMatTarRateTypeControl
                    .CarrTarEquipMatName = oEquip.CarrTarEquipMatName
                    .CarrTarEquipName = oEquip.CarrTarEquipName
                    .ClassTypeControl = oEquip.CarrTarEquipMatClassTypeControl
                    .BracketTypeControl = oEquip.CarrTarEquipMatTarBracketTypeControl
                    .CarrTarMatBPControl = oEquip.CarrTarEquipMatCarrTarMatBPControl
                End With
            End If
            Dim ClonedContractInfo As DTO.RateImportHeader = ExecCloneContract(SelectedContractInfo)
            If ClonedContractInfo Is Nothing Then
                oRet.AddMessage(DTO.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.E_SaveTariffRevisionFailed, New String() {"Clone Failed unknown problem."})
                Return oRet
            End If
            If ClonedContractInfo.Success = False Then
                oRet.AddMessage(DTO.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.E_SaveTariffRevisionFailed, New String() {ClonedContractInfo.Message})
                Return oRet
            End If
            If ClonedContractInfo.CarrTarControl = SelectedContractInfo.CarrTarControl Then
                oRet.AddMessage(DTO.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.E_SaveTariffRevisionFailed, New String() {"Cloned failed did not create revision."})
                Return oRet
            End If

            ' set key so we can use it for the page primary key
            oRet.Key = ClonedContractInfo.CarrTarControl
            'update the equipment data for each rate
            For Each rItem As DTO.RateImportDetailData In rates
                rItem.RateTypeControl = SelectedContractInfo.RateTypeControl
                rItem.ClassTypeControl = SelectedContractInfo.ClassTypeControl
                rItem.BracketTypeControl = SelectedContractInfo.BracketTypeControl
                rItem.TarID = ClonedContractInfo.CarrTarID
                rItem.EquipName = SelectedContractInfo.CarrTarEquipName
                rItem.EffDateFrom = ClonedContractInfo.EffectiveDate
                rItem.EffDateTo = ClonedContractInfo.EffectiveTo
            Next
            'With ClonedContractInfo
            '    .RateTypeControl = SelectedContractInfo.RateTypeControl
            '    .CarrTarEquipMatName = SelectedContractInfo.CarrTarEquipMatName
            '    .CarrTarEquipName = SelectedContractInfo.CarrTarEquipName
            '    .ClassTypeControl = SelectedContractInfo.ClassTypeControl
            '    .BracketTypeControl = SelectedContractInfo.BracketTypeControl
            '    .CarrTarMatBPControl = SelectedContractInfo.CarrTarMatBPControl
            'End With

            'clear any the previous records from the new user's rates temp table.
            Try
                deleteFromTmpSpreadsheetCarrierRates()
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("DeleteFromTmpSpreadsheet"))
            End Try

            'import new rates into the new user's rates temp table.
            Try
                ExecSPImportSpreadsheetData("tmpSpreadsheetCarrierRates", rates)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("ExecSPImportSpreadsheet"))
            End Try

            'import the rates from the user's rates table using stored procedure
            Try
                Dim result = ExecSPImportRatesFromSpreadsheetTable(ClonedContractInfo)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("ExecSPImportRatesFromSpreadsheetTable"))
            End Try

            oRet.Success = True
        Catch ex As System.ServiceModel.FaultException(Of DAL.SqlFaultInfo)
            Throw
        Catch ex As Exception
            'NGLCarrTarEquipData.Man   ManageLinqDataExceptions(ex, buildProcedureName("getNewestEquipControlUsingNewCarrTarControl"))
            throwUnExpectedFaultException(ex, buildProcedureName("ImportNewRates"))
        End Try
        Return oRet

    End Function

End Class

