Option Strict Off
Option Explicit On

Imports Ngl.FreightMaster.Integration.Configuration
Imports Ngl.FreightMaster.Integration.FMDataTableAdapters
Imports System.Data.SqlClient
Imports DTran = Ngl.Core.Utility.DataTransformation
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports LTS = Ngl.FreightMaster.Data.LTS
Imports DAL = Ngl.FreightMaster.Data

<Serializable()> _
<System.ComponentModel.DataObject()> _
Public Class clsPickList : Inherits clsUpload


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

#Region " Class Variables and Properties "

    Private _Adapter As tblPickListTableAdapter
    Protected ReadOnly Property Adapter() As tblPickListTableAdapter
        Get
            If _Adapter Is Nothing Then
                _Adapter = New tblPickListTableAdapter
                _Adapter.SetConnectionString(Me.DBConnection)
            End If

            Return _Adapter
        End Get
    End Property

#End Region

#Region " Constructors "



#End Region

#Region " Functions "

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Protected Function GetData() As FMData.tblPickListDataTable
        Adapter.SetCommandTimeOut(Me.CommandTimeOut)
        Return (Adapter.GetData())
    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Protected Function GetDataByControl(ByVal Control As Long) As FMData.tblPickListDataTable
        Adapter.SetCommandTimeOut(Me.CommandTimeOut)
        Return (Adapter.GetDataByControl(Control))
    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Protected Function GetDataByKeyValues(ByVal Control As Nullable(Of Long), _
                                          ByVal BookCarrOrderNumber As String, _
                                          ByVal CompNumber As String, _
                                          ByVal BookOrderSequence As Integer) As FMData.tblPickListDataTable
        Adapter.SetCommandTimeOut(Me.CommandTimeOut)
        Return (Adapter.GetDataByKeyValues(Control, BookCarrOrderNumber, CompNumber, BookOrderSequence))

    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Protected Function GetDataByControl(ByVal Control As Nullable(Of Long), _
                                        ByVal BookCarrOrderNumber As String, _
                                        ByVal CompNumber As String, _
                                        ByVal BookOrderSequence As Nullable(Of Integer)) As FMData.tblPickListDataTable
        Adapter.SetCommandTimeOut(Me.CommandTimeOut)
        Return (Adapter.GetDataByKeyValues(Control, BookCarrOrderNumber, CompNumber, BookOrderSequence))

    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Protected Function GetDataByExportStatus(ByVal TestDate As Nullable(Of Date), _
                ByVal MaxRetry As Nullable(Of Integer), _
                ByVal CompNumber As String, _
                ByVal OrderNumber As String, _
                ByVal OrderSequence As Nullable(Of Integer)) As FMData.tblPickListDataTable
        Adapter.SetCommandTimeOut(Me.CommandTimeOut)
        Return (Adapter.GetDataByExportStatus(TestDate, MaxRetry, CompNumber, OrderNumber, OrderSequence))
    End Function
    ''' <summary>
    ''' Update the Pick List Status
    ''' </summary>
    ''' <param name="PLControl"></param>
    ''' <param name="ExportDate"></param>
    ''' <param name="Exported"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.4.0.003 on 07/15/2021 added logic to connect to DAL DLL directly using LTS object
    ''' </remarks>
    Public Function UpdateStatus( _
            ByVal PLControl As Long, _
            ByVal ExportDate As Date, _
            Optional ByVal Exported As Boolean = True) As Boolean
        Dim blnRet As Boolean = False
        Dim oWCFParameters As New DAL.WCFParameters
        Dim strConnection = Me.ConnectionString
        With oWCFParameters
            .UserName = "System Download"
            .Database = Me.Database
            .DBServer = Me.DBServer
            .ConnectionString = strConnection
            .WCFAuthCode = "NGLSystem"
            .ValidateAccess = False
        End With

        Try
            Dim oPLDAL As New DAL.NGLtblPickListData(oWCFParameters)

            blnRet = oPLDAL.UpdatePickListExportStatus(PLControl, ExportDate, Exported)


        Catch ex As Exception
            LogException("Update PickList Export Failure", "Could update Pick List records as exported duplicate export is possible.", AdminEmail, ex, "NGL.FreightMaster.Integration.clsPickList.UpdateStatus Failure")
        Finally

        End Try


        Return blnRet
    End Function

    Public Function getDataSet() As PickListData
        Return New PickListData
    End Function
    ''' <summary>
    ''' NOTE: the BookCarrOrderNumber and CompNumber parameters are no longer used 
    ''' they are left for backward compatibility of the contract
    ''' </summary>
    ''' <param name="strConnection"></param>
    ''' <param name="PLControl"></param>
    ''' <param name="BookCarrOrderNumber"></param>
    ''' <param name="CompNumber"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function confirmExport(ByVal strConnection As String, _
                ByVal PLControl As Nullable(Of Long), _
                Optional ByVal BookCarrOrderNumber As String = Nothing, _
                Optional ByVal CompNumber As String = Nothing) As Boolean
        Dim blnRet As Boolean = False
        Me.CreatedDate = Now.ToString
        Me.CreateUser = "Data Integration DLL"
        Me.Source = "Pick List Data Confirmation"

        Me.DBConnection = strConnection
        'try the connection
        If Not Me.openConnection Then
            Return ProcessDataReturnValues.nglDataConnectionFailure
        End If

        Try
            If Me.UpdateStatus(PLControl, Now, True) Then
                Log("Pick List Export Status Updated")
                Return True
            End If
        Catch ex As Exception
            LogException("Pick List Export Confirmation Error", "Could not update the export status for pick list control number " & PLControl.ToString & ".", AdminEmail, ex, "NGL.FreightMaster.Integration.clsPickList.confirmExport Failure")
        Finally
            closeConnection()
        End Try

        Return blnRet
    End Function

    Public Function readObjectData(ByRef oPickList() As clsPickListObject, _
            ByRef oPickDetails() As clsPickDetailObject, _
            ByVal strConnection As String, _
            Optional ByVal MaxRetry As Integer = 0, _
            Optional ByVal RetryMinutes As Integer = 0, _
            Optional ByVal CompNumber As String = Nothing, _
            Optional ByVal OrderNumber As String = Nothing, _
            Optional ByVal OrderSequence As String = Nothing) As ProcessDataReturnValues

        Dim oPTable As New PickListData.PickListDataTable
        Dim oDTable As New PickListData.PickDetailDataTable
        Dim oRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationComplete
        Try
            oRet = readData(oPTable, _
                    oDTable, _
                    strConnection, _
                    MaxRetry, _
                    RetryMinutes, _
                    CompNumber, _
                    OrderNumber, _
                    OrderSequence)

            If Not oPTable Is Nothing Then
                If oPTable.Rows.Count > 0 Then
                    ReDim oPickList(oPTable.Rows.Count - 1)
                End If
                Dim intCt As Integer = 0

                For Each oRow As PickListData.PickListRow In oPTable

                    Dim oObject As New clsPickListObject
                    With oObject
                        .BookCarrOrderNumber = DTran.getDataRowString(oRow, "BookCarrOrderNumber", "")
                        .BookCommCompControl = DTran.NZ(oRow, "BookCommCompControl", 0)
                        .BookConsPrefix = DTran.getDataRowString(oRow, "BookConsPrefix", "")
                        .BookDateLoad = exportDateToString(DTran.getDataRowString(oRow, "BookDateLoad", ""))
                        .BookDateOrdered = exportDateToString(DTran.getDataRowString(oRow, "BookDateOrdered", ""))
                        .BookDateRequired = exportDateToString(DTran.getDataRowString(oRow, "BookDateRequired", ""))
                        .BookDestAddress1 = DTran.getDataRowString(oRow, "BookDestAddress1", "")
                        .BookDestAddress2 = DTran.getDataRowString(oRow, "BookDestAddress2", "")
                        .BookDestAddress3 = DTran.getDataRowString(oRow, "BookDestAddress3", "")
                        .BookDestCity = DTran.getDataRowString(oRow, "BookDestCity", "")
                        .BookDestCountry = DTran.getDataRowString(oRow, "BookDestCountry", "")
                        .BookDestName = DTran.getDataRowString(oRow, "BookDestName", "")
                        .BookDestState = DTran.getDataRowString(oRow, "BookDestState", "")
                        .BookDestZip = DTran.getDataRowString(oRow, "BookDestZip", "")
                        .BookFinCommStd = DTran.NZ(oRow, "BookFinCommStd", 0)
                        .BookLoadCom = DTran.getDataRowString(oRow, "BookLoadCom", "")
                        .BookLoadPONumber = DTran.getDataRowString(oRow, "BookLoadPONumber", "")
                        .BookMilesFrom = DTran.getDataRowString(oRow, "BookMilesFrom", "0")
                        .BookOrigAddress1 = DTran.getDataRowString(oRow, "BookOrigAddress1", "")
                        .BookOrigAddress2 = DTran.getDataRowString(oRow, "BookOrigAddress2", "")
                        .BookOrigAddress3 = DTran.getDataRowString(oRow, "BookOrigAddress3", "")
                        .BookOrigCity = DTran.getDataRowString(oRow, "BookOrigCity", "")
                        .BookOrigCountry = DTran.getDataRowString(oRow, "BookOrigCountry", "")
                        .BookOrigName = DTran.getDataRowString(oRow, "BookOrigName", "")
                        .BookOrigState = DTran.getDataRowString(oRow, "BookOrigState", "")
                        .BookOrigZip = DTran.getDataRowString(oRow, "BookOrigZip", "")
                        .BookProNumber = DTran.getDataRowString(oRow, "BookProNumber", "")
                        .BookRevCommCost = DTran.NZ(oRow, "BookRevCommCost", 0)
                        .BookRevGrossRevenue = DTran.NZ(oRow, "BookRevGrossRevenue", 0)
                        .BookRevTotalCost = DTran.getDataRowString(oRow, "BookRevTotalCost", "0")
                        .BookRouteFinalCode = DTran.getDataRowString(oRow, "BookRouteFinalCode", "")
                        .BookRouteFinalDate = exportDateToString(DTran.getDataRowString(oRow, "BookRouteFinalDate", ""))
                        .BookStopNo = DTran.getDataRowString(oRow, "BookStopNo", "0")
                        .BookTotalBFC = DTran.getDataRowString(oRow, "BookTotalBFC", "0")
                        .BookTotalCases = DTran.getDataRowString(oRow, "BookTotalCases", "0")
                        .BookTotalCube = DTran.getDataRowString(oRow, "BookTotalCube", "0")
                        .BookTotalPL = DTran.getDataRowString(oRow, "BookTotalPL", "0")
                        .BookTotalWgt = DTran.getDataRowString(oRow, "BookTotalWgt", "0")
                        .BookTypeCode = DTran.getDataRowString(oRow, "BookTypeCode", "")
                        .CarrierName = DTran.getDataRowString(oRow, "CarrierName", "")
                        .CarrierNumber = DTran.getDataRowString(oRow, "CarrierNumber", "0")
                        .CommCodeDescription = DTran.getDataRowString(oRow, "CommCodeDescription", "")
                        .CompName = DTran.getDataRowString(oRow, "CompName", "")
                        .CompNumber = DTran.getDataRowString(oRow, "CompNumber", "")
                        .LaneNumber = DTran.getDataRowString(oRow, "LaneNumber", "")
                        .LoadOrder = DTran.getDataRowString(oRow, "LoadOrder", "")
                        .PLControl = DTran.NZ(oRow, "PLControl", 0)
                        .BookCommCompControl = DTran.NZ(oRow, "BookCommCompControl", 0)
                        .BookRevCommCost = DTran.NZ(oRow, "BookRevCommCost", 0)
                        .BookRevGrossRevenue = DTran.NZ(oRow, "BookRevGrossRevenue", 0)
                        .BookFinCommStd = DTran.NZ(oRow, "BookFinCommStd", 0)
                        .BookDoNotInvoice = DTran.NZ(oRow, "BookDoNotInvoice", 0)
                        .BookOrderSequence = DTran.NZ(oRow, "BookOrderSequence", 0)
                        .CarrierEquipmentCodes = DTran.getDataRowString(oRow, "CarrierEquipmentCodes", "")
                        .BookCarrierTypeCode = DTran.getDataRowString(oRow, "BookCarrierTypeCode", "")
                        .BookWarehouseNumber = DTran.getDataRowString(oRow, "BookWarehouseNumber", "")
                        .BookShipCarrierProNumber = DTran.getDataRowString(oRow, "BookShipCarrierProNumber", "")
                        .CompNatNumber = DTran.NZ(oRow, "CompNatNumber", 0)
                        .BookTransType = DTran.getDataRowString(oRow, "BookTransType", "")
                        .BookShipCarrierNumber = DTran.getDataRowString(oRow, "BookShipCarrierNumber", "")
                        .LaneComments = DTran.getDataRowString(oRow, "LaneComments", "")
                        .FuelSurCharge = DTran.NZ(oRow, "FuelSurCharge", 0)
                        .BookRevCarrierCost = DTran.NZ(oRow, "BookRevCarrierCost", 0)
                        .BookRevOtherCost = DTran.NZ(oRow, "BookRevOtherCost", 0)
                        .BookRevNetCost = DTran.NZ(oRow, "BookRevNetCost", 0)
                        .BookRevFreightTax = DTran.NZ(oRow, "BookRevFreightTax", 0)
                        .BookFinServiceFee = DTran.NZ(oRow, "BookFinServiceFee", 0)
                        .BookRevLoadSavings = DTran.NZ(oRow, "BookRevLoadSavings", 0)
                        .TotalNonFuelFees = DTran.NZ(oRow, "TotalNonFuelFees", 0)
                        .BookPickNumber = DTran.NZ(oRow, "BookPickNumber", 0)
                        .BookPickupStopNumber = DTran.NZ(oRow, "BookPickupStopNumber", 0)
                    End With
                    oPickList(intCt) = oObject
                    intCt += 1
                    If intCt > oPTable.Rows.Count Then Exit For
                Next
                'Now add all the detail records
                If intCt > 0 Then
                    intCt = 0
                    If oDTable.Rows.Count > 0 Then
                        ReDim oPickDetails(oDTable.Rows.Count - 1)
                    End If


                    For Each oRow As PickListData.PickDetailRow In oDTable

                        Dim oObject As New clsPickDetailObject
                        With oObject
                            .BookCarrOrderNumber = DTran.getDataRowString(oRow, "BookCarrOrderNumber", "")
                            .Cube = DTran.getDataRowString(oRow, "Cube", "0")
                            .CustItemNumber = DTran.getDataRowString(oRow, "CustItemNumber", "")
                            .CustomerNumber = DTran.getDataRowString(oRow, "CustomerNumber", "")
                            .Description = DTran.getDataRowString(oRow, "Description", "")
                            .FreightCost = DTran.getDataRowString(oRow, "FreightCost", "0")
                            .ItemCost = DTran.getDataRowString(oRow, "ItemCost", "0")
                            .ItemNumber = DTran.getDataRowString(oRow, "ItemNumber", "")
                            .Pack = DTran.getDataRowString(oRow, "Pack", "")
                            .PLControl = DTran.NZ(oRow, "PLControl", 0)
                            .QtyOrdered = DTran.getDataRowString(oRow, "QtyOrdered", "0")
                            .Size = DTran.getDataRowString(oRow, "Size", "")
                            .Weight = DTran.getDataRowString(oRow, "Weight", "0")
                            .OrderSequence = DTran.NZ(oRow, "OrderSequence", 0)
                            .Hazmat = DTran.getDataRowString(oRow, "Hazmat", "")
                            .Brand = DTran.getDataRowString(oRow, "Brand", "")
                            .CostCenter = DTran.getDataRowString(oRow, "CostCenter", "")
                            .LotNumber = DTran.getDataRowString(oRow, "LotNumber", "")
                            .LotExpirationDate = exportDateToString(DTran.getDataRowString(oRow, "LotExpirationDate", ""))
                            .GTIN = DTran.getDataRowString(oRow, "GTIN", "")
                            .BFC = DTran.getDataRowString(oRow, "BFC", "0")
                            .CountryOfOrigin = DTran.getDataRowString(oRow, "CountryOfOrigin", "")
                            .CustomerPONumber = DTran.getDataRowString(oRow, "CustomerPONumber", "")
                            .HST = DTran.getDataRowString(oRow, "HST", "")
                            .BookProNumber = DTran.getDataRowString(oRow, "BookProNumber", "")
                            .PalletType = DTran.getDataRowString(oRow, "PalletType", "")
                            .CompNatNumber = DTran.NZ(oRow, "CompNatNumber", 0)
                        End With
                        oPickDetails(intCt) = oObject
                        intCt += 1
                        If intCt > oDTable.Rows.Count Then Exit For
                    Next
                End If
            End If

        Catch ex As Exception
            oRet = ProcessDataReturnValues.nglDataIntegrationFailure
            LogException("Pick List Read Object Data Failure", "Could not read pick list data records.", AdminEmail, ex, "NGL.FreightMaster.Integration.clsPickList.readObjectData Failure")
        End Try
        Return oRet
    End Function

    Public Function readData(ByRef oPickList As PickListData.PickListDataTable, _
            ByRef oPickDetails As PickListData.PickDetailDataTable, _
            ByVal strConnection As String, _
            Optional ByVal MaxRetry As Integer = 0, _
            Optional ByVal RetryMinutes As Integer = 0, _
            Optional ByVal CompNumber As String = Nothing, _
            Optional ByVal OrderNumber As String = Nothing, _
            Optional ByVal OrderSequence As String = Nothing) As ProcessDataReturnValues

        Dim enmRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationFailure
        Dim TestDate As Nullable(Of Date)
        Dim strMsg As String = ""
        Dim strTitle As String = ""
        Dim intErrors As Integer = 0
        Dim intCount As Integer = 0
        Dim strSource As String = "clsPickList.readData"
        GroupEmailMsg = ""
        ITEmailMsg = ""
        Me.ImportTypeKey = IntegrationTypes.PickList
        Me.CreatedDate = Now.ToString
        Me.CreateUser = "Data Integration DLL"
        Me.Source = "Pick List Data Integration"

        Me.DBConnection = strConnection
        'try the connection
        If Not Me.openConnection Then
            Return ProcessDataReturnValues.nglDataConnectionFailure
        End If


        If RetryMinutes > 0 Then
            TestDate = Date.Now.AddMinutes(-RetryMinutes)
        End If
        Try
            RecordErrors = 0
            TotalRecords = 0
            ItemErrors = 0
            'check for empty strings being passed as parameters
            If String.IsNullOrEmpty(CompNumber) OrElse CompNumber.Trim.Length < 1 Then CompNumber = Nothing 'empty strings are not allowed
            If String.IsNullOrEmpty(OrderNumber) OrElse OrderNumber.Trim.Length < 1 Then OrderNumber = Nothing 'empty strings are not allowed

            Dim nintBookOrderSequence As Nullable(Of Integer) = 0
            Dim intTest As Integer = 0
            If Not String.IsNullOrEmpty(OrderSequence) Then
                If Integer.TryParse(OrderSequence, intTest) Then
                    nintBookOrderSequence = intTest
                Else
                    'this is an invalid order sequence number so return false
                    LastError = "The Book Order Sequence Number " & OrderSequence & " is not valid."
                    Log(LastError)
                    ITEmailMsg &= "<br /> The read Pick List Data command failed with the following data:" _
                        & "<br /> MaxRetry: " & MaxRetry.ToString _
                        & "<br /> RetryMinutes: " & RetryMinutes.ToString _
                        & "<br /> CompNumber: " & CompNumber _
                        & "<br /> OrderNumber: " & OrderNumber _
                        & "<br /> OrderSequence: " & OrderSequence _
                        & "<br /> Error Message: " & LastError _
                        & "<br />" & vbCrLf
                    LogError("Read Pick List Records Error Report", ITEmailMsg, AdminEmail)
                    Return ProcessDataReturnValues.nglDataIntegrationFailure
                End If
            End If

            ' Read in the pick list records
            Dim oTable As FMData.tblPickListDataTable = Me.GetDataByExportStatus(TestDate, Nothing, CompNumber, OrderNumber, nintBookOrderSequence)
            Dim intRowCount As Integer = 0
            'Loop through all of the records checking for maxrowsreturned
            For Each oRow As FMData.tblPickListRow In oTable
                Try
                    'check if the max retry number has been exceeded.
                    If MaxRetry > 0 AndAlso DTran.NZ(oRow, "PLExportRetry", 0) > MaxRetry Then
                        'we do not export this record we add this record to the group email error message.
                        GroupEmailMsg &= "<br /> The following pick list record has exceeded the maximum number or export retries:" _
                                    & "<br /> PL Control: " & DTran.getDataRowString(oRow, "PLControl", "0") _
                                    & "<br /> Book Pro Number: " & DTran.getDataRowString(oRow, "BookProNumber", "") _
                                    & "<br /> Order Number: " & DTran.getDataRowString(oRow, "BookCarrOrderNumber", "") _
                                    & "<br /> Order Sequence: " & DTran.getDataRowString(oRow, "BookOrderSequence", "0") _
                                    & "<br /> Company Number: " & DTran.getDataRowString(oRow, "CompNumber", "0") _
                                    & "<br /> Retry # " & DTran.getDataRowString(oRow, "PLExportRetry", "0") & " of " & MaxRetry.ToString _
                                    & "<br />" & vbCrLf
                    Else
                        intRowCount += 1
                        If Not (Me.MaxRowsReturned > 0 AndAlso intRowCount > Me.MaxRowsReturned) Then
                            'add the data to the export dataset
                            If Not addPickListExportRow(oPickList, oRow) Then
                                RecordErrors += 1
                                ITEmailMsg &= "<br /> Unable to create pick list export record for:" _
                                    & "<br /> PL Control: " & DTran.getDataRowString(oRow, "PLControl", "0") _
                                    & "<br /> Book Pro Number: " & DTran.getDataRowString(oRow, "BookProNumber", "") _
                                    & "<br /> Order Number: " & DTran.getDataRowString(oRow, "BookCarrOrderNumber", "") _
                                    & "<br /> Order Sequence: " & DTran.getDataRowString(oRow, "BookOrderSequence", "0") _
                                    & "<br /> Company Number: " & DTran.getDataRowString(oRow, "CompNumber", "0") _
                                    & "<br />" & vbCrLf
                            Else
                                'add the details
                                If Not addPickDetailRows(oPickDetails, DTran.getDataRowString(oRow, "BookProNumber", "NA"), DTran.getDataRowString(oRow, "BookCarrOrderNumber", "NA"), DTran.NZ(oRow, "BookOrderSequence", 0), DTran.getDataRowString(oRow, "CompNumber", "0"), DTran.NZ(oRow, "PLControl", 0), DTran.NZ(oRow, "CompNatNumber", 0)) Then
                                    oPickList.RejectChanges()
                                    ItemErrors += 1
                                    ITEmailMsg &= "<br />Unable to read Pick List item details for:" _
                                       & "<br /> PL Control: " & DTran.getDataRowString(oRow, "PLControl", "0") _
                                       & "<br /> Book Pro Number: " & DTran.getDataRowString(oRow, "BookProNumber", "") _
                                       & "<br /> Order Number: " & DTran.getDataRowString(oRow, "BookCarrOrderNumber", "") _
                                       & "<br /> Order Sequence: " & DTran.getDataRowString(oRow, "BookOrderSequence", "0") _
                                       & "<br /> Company Number: " & DTran.getDataRowString(oRow, "CompNumber", "0") _
                                       & "<br />" & vbCrLf
                                Else
                                    'commit the changes to the header record
                                    oPickList.AcceptChanges()
                                    TotalRecords += 1
                                    Try
                                        If Me.AutoConfirmation Then
                                            If Me.UpdateStatus(DTran.NZ(oRow, "PLControl", 0), Now, True) Then
                                                Log("Pick List Auto Confirmation Complete For PL Control " & DTran.getDataRowString(oRow, "PLControl", "0") & ".")
                                            End If
                                        Else
                                            Me.UpdateStatus(DTran.NZ(oRow, "PLControl", 0), Now, False)
                                        End If
                                    Catch ex As System.ApplicationException
                                        'Log the exception and move on to the next record
                                        LogException("Pick List Export Update Status Error (duplicate export possible)", "Could not update the export status for pick list control number " & DTran.getDataRowString(oRow, "PLControl", "0") & ".", AdminEmail, ex, "NGL.FreightMaster.Integration.clsPickList.readData Failure")
                                    Catch ex As Exception
                                        'this is an unexpected error so re throw it
                                        Throw
                                    End Try
                                End If
                            End If
                        End If
                    End If
                Catch ex As System.ApplicationException
                    ITEmailMsg &= "<br /> Application Error: " & ex.ToString & " on order number " & DTran.getDataRowString(oRow, "BookCarrOrderNumber", "") & "<br />" & vbCrLf
                Catch ex As Exception
                    Throw
                End Try
            Next
            If GroupEmailMsg.Trim.Length > 0 Then
                LogError("Pick List Export Data Integration Warning", "The following warnings were reported while running the Pick List Export data integration routine.  If the maximum number of retries for a record has been reported the Pick List record was not exported and may only be exported manually by an administrator." & GroupEmailMsg, GroupEmail)
            End If
            If ITEmailMsg.Trim.Length > 0 Then
                LogError("Pick List Export Error Report", "The following errors were reported while running the Pick List oExport data integration routine:" & ITEmailMsg, AdminEmail)
            End If
            If Me.TotalRecords > 0 Then
                strMsg = "Success! " & Me.TotalRecords & " Pick List records were exported." & vbCrLf
                enmRet = ProcessDataReturnValues.nglDataIntegrationComplete
                If Me.RecordErrors > 0 Or Me.ItemErrors > 0 Then
                    If Me.RecordErrors > 0 Then
                        strMsg &= " ERROR!  " & Me.RecordErrors & " Pick List records could not be exported.  Please check the email error report or database error log records for more information." & vbCrLf
                    End If
                    If Me.ItemErrors > 0 Then
                        strMsg &= " ERROR!  " & Me.ItemErrors & " Pick List Detail records could not be exported.  Please check the email error report or database error log records for more information." & vbCrLf
                    End If
                    enmRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                End If
            ElseIf StatusUpdateErrors > 0 Then
                strMsg = "WARNING! No Pick List records were available for export and " _
                    & StatusUpdateErrors & " Pick List records did not have their status flags updated.  Duplicate transmission of pick list records is possible.  Please check the email error report or database error log records for more information."
                enmRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
            Else
                strMsg = "No Pick List records were available for export."
                enmRet = ProcessDataReturnValues.nglDataIntegrationComplete
            End If
            Log(strMsg)
        Catch ex As Exception
            LogException("Pick List Read Data Failure", "Could not read pick list data records", AdminEmail, ex, "NGL.FreightMaster.Integration.clsPickList.readData Failure")
        Finally
            closeConnection()
        End Try

        Return enmRet
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oPLT"></param>
    ''' <param name="oRow"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR on 02/21/2020 for v-8.2.1.005
    '''   Added logic to check for empty string on Origin State because the 
    '''   Substring function can throw a System.ArgumentOutOfRangeException when the string is less
    '''   than 2 characters long.
    ''' </remarks>
    Protected Function addPickListExportRow(ByRef oPLT As PickListData.PickListDataTable, ByVal oRow As FMData.tblPickListRow) As Boolean
        Dim blnRet As Boolean = False
        Try
            If oPLT Is Nothing Then
                oPLT = New PickListData.PickListDataTable
            End If
            Dim drPickList As PickListData.PickListRow = oPLT.NewPickListRow
            With drPickList
                .BookCarrOrderNumber = getDataRowString(oRow, "BookCarrOrderNumber")
                .BookConsPrefix = getDataRowString(oRow, "BookConsPrefix")
                .BookDateLoad = exportDateToString(getDataRowString(oRow, "BookDateLoad"))
                .BookDateOrdered = exportDateToString(getDataRowString(oRow, "BookDateOrdered"))
                .BookDateRequired = exportDateToString(getDataRowString(oRow, "BookDateRequired"))
                .BookDestAddress1 = getDataRowString(oRow, "BookDestAddress1")
                .BookDestAddress2 = getDataRowString(oRow, "BookDestAddress2")
                .BookDestAddress3 = getDataRowString(oRow, "BookDestAddress3")
                .BookDestCity = getDataRowString(oRow, "BookDestCity")
                .BookDestCountry = getDataRowString(oRow, "BookDestCountry")
                .BookDestName = getDataRowString(oRow, "BookDestName")
                .BookDestState = getDataRowString(oRow, "BookDestState")
                .BookDestZip = getDataRowString(oRow, "BookDestZip")
                .BookLoadCom = getDataRowString(oRow, "BookLoadCom")
                .BookLoadPONumber = getDataRowString(oRow, "BookLoadPONumber")
                .BookMilesFrom = getDataRowString(oRow, "BookMilesFrom")
                .BookOrigAddress1 = getDataRowString(oRow, "BookOrigAddress1")
                .BookOrigAddress2 = getDataRowString(oRow, "BookOrigAddress2")
                .BookOrigAddress3 = getDataRowString(oRow, "BookOrigAddress3")
                .BookOrigCity = getDataRowString(oRow, "BookOrigCity")
                .BookOrigCountry = getDataRowString(oRow, "BookOrigCountry")
                .BookOrigName = getDataRowString(oRow, "BookOrigName")
                ' Modified by RHR on 02/21/2020 for v-8.2.1.005
                'old code modified below: .BookOrigState = getDataRowString(oRow, "BookOrigState").Substring(0, 2)
                Dim strOrigState = getDataRowString(oRow, "BookOrigState")
                If Not String.IsNullOrWhiteSpace(strOrigState) AndAlso strOrigState.Length() > 2 Then
                    strOrigState = strOrigState.Substring(0, 2) 'a max of 2 characters is allowed in the table
                End If
                .BookOrigState = strOrigState
                .BookOrigZip = getDataRowString(oRow, "BookOrigZip")
                .BookProNumber = getDataRowString(oRow, "BookProNumber")
                .BookRevTotalCost = getDataRowString(oRow, "BookRevTotalCost")
                .BookRouteFinalCode = getDataRowString(oRow, "BookRouteFinalCode")
                .BookRouteFinalDate = exportDateToString(getDataRowString(oRow, "BookRouteFinalDate"))
                .BookStopNo = getDataRowString(oRow, "BookStopNo")
                .BookTotalBFC = getDataRowString(oRow, "BookTotalBFC")
                .BookTotalCases = getDataRowString(oRow, "BookTotalCases")
                .BookTotalCube = getDataRowString(oRow, "BookTotalCube")
                .BookTotalPL = getDataRowString(oRow, "BookTotalPL")
                .BookTotalWgt = getDataRowString(oRow, "BookTotalWgt")
                .BookTypeCode = getDataRowString(oRow, "BookTypeCode")
                .CarrierName = getDataRowString(oRow, "CarrierName")
                .CarrierNumber = getDataRowString(oRow, "CarrierNumber")
                .CommCodeDescription = getDataRowString(oRow, "CommCodeDescription")
                .CompName = getDataRowString(oRow, "CompName")
                .CompNumber = getDataRowString(oRow, "CompNumber")
                .LaneNumber = getDataRowString(oRow, "LaneNumber")
                .LoadOrder = getDataRowString(oRow, "LoadOrder")
                .PLControl = getDataRowString(oRow, "PLControl")
                .BookCommCompControl = nz(getDataRowString(oRow, "BookCommCompControl"), 0)
                .BookRevCommCost = nz(getDataRowString(oRow, "BookRevCommCost"), 0)
                .BookRevGrossRevenue = nz(getDataRowString(oRow, "BookRevGrossRevenue"), 0)
                .BookFinCommStd = nz(getDataRowString(oRow, "BookFinCommStd"), 0)
                .BookOrderSequence = nz(getDataRowString(oRow, "BookOrderSequence"), 0)
                .CarrierEquipmentCodes = lookupCarrierEquipmentCodesByPro(.BookProNumber) 'Note: Pro number is previously assigned
                .BookCarrierTypeCode = getDataRowString(oRow, "BookCarrierTypeCode")
                .BookWarehouseNumber = getDataRowString(oRow, "BookWarehouseNumber")
                .BookShipCarrierProNumber = getDataRowString(oRow, "BookShipCarrierProNumber")
                .CompNatNumber = nz(getDataRowString(oRow, "CompNatNumber"), 0)
                .BookTransType = nz(getDataRowString(oRow, "BookTransType"), 0)
                .BookDoNotInvoice = nz(getDataRowString(oRow, "BookDoNotInvoice"), 0)
                .BookShipCarrierNumber = getDataRowString(oRow, "BookShipCarrierNumber")
                .LaneComments = DTran.getDataRowString(oRow, "LaneComments", "")
                .FuelSurCharge = DTran.NZ(oRow, "FuelSurCharge", 0)
                .BookRevCarrierCost = DTran.NZ(oRow, "BookRevCarrierCost", 0)
                .BookRevOtherCost = DTran.NZ(oRow, "BookRevOtherCost", 0)
                .BookRevNetCost = DTran.NZ(oRow, "BookRevNetCost", 0)
                .BookRevFreightTax = DTran.NZ(oRow, "BookRevFreightTax", 0)
                .BookFinServiceFee = DTran.NZ(oRow, "BookFinServiceFee", 0)
                .BookRevLoadSavings = DTran.NZ(oRow, "BookRevLoadSavings", 0)
                .TotalNonFuelFees = DTran.NZ(oRow, "TotalNonFuelFees", 0)
                .BookPickNumber = DTran.NZ(oRow, "BookPickNumber", 0)
                .BookPickupStopNumber = DTran.NZ(oRow, "BookPickupStopNumber", 0)
            End With
            oPLT.AddPickListRow(drPickList)
            blnRet = True
        Catch ex As System.ApplicationException
            RecordErrors += 1
            Throw
        Catch ex As Exception
            RecordErrors += 1
            Throw New System.ApplicationException(ex.ToString, ex.InnerException)
        End Try
        Return blnRet
    End Function

    Public Function addPickDetailRows(ByRef oPDT As PickListData.PickDetailDataTable, _
                ByVal BookProNumber As String, _
                ByVal BookCarrOrderNumber As String, _
                ByVal BookOrderSequence As String, _
                ByVal CompNumber As String, _
                ByVal PLControl As Long, _
                ByVal CompNatNumber As Integer) As Boolean
        Dim Ret As Boolean = False
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
                & " isnull(dbo.PalletType.PalletType,'NA') as PalletType" _
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
                If oPDT Is Nothing Then
                    oPDT = New PickListData.PickDetailDataTable
                End If
                Do While drTemp.Read()
                    Dim oPDR As PickListData.PickDetailRow = oPDT.NewPickDetailRow
                    Dim ItemNumber As String = "0"
                    If Not drTemp.IsDBNull(0) Then
                        ItemNumber = drTemp.GetString(0)
                    End If
                    oPDR.ItemNumber = ItemNumber
                    oPDR.BookCarrOrderNumber = BookCarrOrderNumber
                    If Not drTemp.IsDBNull(1) Then oPDR.QtyOrdered = drTemp.GetInt32(1).ToString
                    If Not drTemp.IsDBNull(2) Then oPDR.FreightCost = drTemp.GetSqlMoney(2).ToString
                    If Not drTemp.IsDBNull(3) Then oPDR.ItemCost = drTemp.GetSqlMoney(3).ToString
                    If Not drTemp.IsDBNull(4) Then oPDR.Weight = drTemp.GetSqlDouble(4).ToString
                    If Not drTemp.IsDBNull(5) Then oPDR.Cube = drTemp.GetInt32(5).ToString
                    If Not drTemp.IsDBNull(6) Then oPDR.Pack = drTemp.GetInt16(6).ToString
                    If Not drTemp.IsDBNull(7) Then oPDR.Size = drTemp.GetString(7)
                    If Not drTemp.IsDBNull(8) Then oPDR.Description = drTemp.GetString(8)
                    If Not drTemp.IsDBNull(9) Then oPDR.CustItemNumber = drTemp.GetString(9)
                    If Not drTemp.IsDBNull(10) Then oPDR.Hazmat = drTemp.GetString(10)
                    If Not drTemp.IsDBNull(11) Then oPDR.Brand = drTemp.GetString(11)
                    If Not drTemp.IsDBNull(12) Then oPDR.CostCenter = drTemp.GetString(12)
                    If Not drTemp.IsDBNull(13) Then oPDR.LotNumber = drTemp.GetString(13)
                    If Not drTemp.IsDBNull(14) Then oPDR.LotExpirationDate = drTemp.GetDateTime(14)
                    If Not drTemp.IsDBNull(15) Then oPDR.GTIN = drTemp.GetString(15)
                    If Not drTemp.IsDBNull(16) Then oPDR.BFC = drTemp.GetSqlMoney(16).ToString
                    If Not drTemp.IsDBNull(17) Then oPDR.CountryOfOrigin = drTemp.GetString(17)
                    If Not drTemp.IsDBNull(18) Then oPDR.HST = drTemp.GetString(18)
                    If Not drTemp.IsDBNull(19) Then oPDR.PalletType = Left(drTemp.GetString(19), 10)
                    oPDR.CustomerNumber = CompNumber
                    oPDR.PLControl = PLControl
                    oPDR.OrderSequence = BookOrderSequence
                    oPDR.BookProNumber = BookProNumber
                    oPDR.CompNatNumber = CompNatNumber
                    oPDT.AddPickDetailRow(oPDR)
                    oPDT.AcceptChanges()
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
        End Try
        Return Ret
    End Function

    Public Function readObjectData60(ByRef oPickList As List(Of clsPickListObject60), _
            ByRef oPickDetails As List(Of clsPickDetailObject60), _
            ByVal strConnection As String, _
            Optional ByVal MaxRetry As Integer = 0, _
            Optional ByVal RetryMinutes As Integer = 0, _
            Optional ByVal CompNumber As String = Nothing, _
            Optional ByVal OrderNumber As String = Nothing, _
            Optional ByVal OrderSequence As String = Nothing) As ProcessDataReturnValues
        Dim TestDate As Nullable(Of Date)
        Dim strMsg As String = ""
        Dim strTitle As String = ""
        Dim intErrors As Integer = 0
        Dim intCount As Integer = 0
        Dim strSource As String = "clsPickList.readData"
        GroupEmailMsg = ""
        ITEmailMsg = ""
        Me.ImportTypeKey = IntegrationTypes.PickList
        Me.CreatedDate = Now.ToString
        Me.CreateUser = "Data Integration DLL"
        Me.Source = "Pick List Data Integration"
        Dim oRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationComplete
        Me.DBConnection = strConnection
        'try the connection
        If Not Me.openConnection Then
            Return ProcessDataReturnValues.nglDataConnectionFailure
        End If


        If RetryMinutes > 0 Then
            TestDate = Date.Now.AddMinutes(-RetryMinutes)
        End If
        Try
            RecordErrors = 0
            TotalRecords = 0
            ItemErrors = 0
            'check for empty strings being passed as parameters
            If String.IsNullOrEmpty(CompNumber) OrElse CompNumber.Trim.Length < 1 Then CompNumber = Nothing 'empty strings are not allowed
            If String.IsNullOrEmpty(OrderNumber) OrElse OrderNumber.Trim.Length < 1 Then OrderNumber = Nothing 'empty strings are not allowed
            Dim nintBookOrderSequence As Nullable(Of Integer) = 0
            Dim intTest As Integer = 0

            If Not String.IsNullOrEmpty(OrderSequence) Then
                If Integer.TryParse(OrderSequence, intTest) Then
                    nintBookOrderSequence = intTest
                Else
                    'this is an invalid order sequence number so return false
                    LastError = "The Book Order Sequence Number " & OrderSequence & " is not valid."
                    Log(LastError)
                    ITEmailMsg &= "<br /> The read Pick List Object Data 60 command failed with the following data:" _
                        & "<br /> MaxRetry: " & MaxRetry.ToString _
                        & "<br /> RetryMinutes: " & RetryMinutes.ToString _
                        & "<br /> CompNumber: " & CompNumber _
                        & "<br /> OrderNumber: " & OrderNumber _
                        & "<br /> OrderSequence: " & OrderSequence _
                        & "<br /> Error Message: " & LastError _
                        & "<br />" & vbCrLf
                    LogError("Read Pick List Object Data 60 Error Report", ITEmailMsg, AdminEmail)
                    Return ProcessDataReturnValues.nglDataIntegrationFailure
                End If
            End If
            Dim rowsToTake As Integer = 1000
            If MaxRowsReturned > 0 Then rowsToTake = MaxRowsReturned

            Using db As New LTSIntegrationDataDataContext(strConnection)
                Dim oPData As List(Of clsPickListObject60) = (From p In db.tblPickLists _
                             Where _
                             (p.PLExported = 0) _
                             And _
                             (p.PLExportDate.HasValue = False OrElse TestDate.HasValue = False OrElse p.PLExportDate.Value < TestDate.Value) _
                             And _
                             (String.IsNullOrEmpty(CompNumber) OrElse p.CompNumber = CompNumber) _
                             And _
                             ( _
                                String.IsNullOrEmpty(OrderNumber) _
                                OrElse _
                                ( _
                                    p.BookCarrOrderNumber = OrderNumber _
                                    AndAlso _
                                    ( _
                                        nintBookOrderSequence.HasValue = False _
                                        OrElse _
                                        p.BookOrderSequence = nintBookOrderSequence.Value _
                                    ) _
                                ) _
                             ) _
                             Order By p.PLControl _
                             Select New clsPickListObject60 With {.PLControl = p.PLControl _
                                                                 , .PLExportRetry = p.PLExportRetry _
                                                                 , .PLExportDate = p.PLExportDate _
                                                                 , .PLExported = p.PLExported _
                                                                 , .BookAlternateAddressLaneNumber = p.BookAlternateAddressLaneNumber _
                                                                 , .BookCarrierTypeCode = p.BookCarrierTypeCode _
                                                                 , .BookCarrOrderNumber = p.BookCarrOrderNumber _
                                                                 , .BookCommCompControl = p.BookCommCompControl _
                                                                 , .BookConsPrefix = p.BookConsPrefix _
                                                                 , .BookDateLoad = p.BookDateLoad _
                                                                 , .BookDateOrdered = p.BookDateOrdered _
                                                                 , .BookDateRequired = p.BookDateRequired _
                                                                 , .BookDestAddress1 = p.BookDestAddress1 _
                                                                 , .BookDestAddress2 = p.BookDestAddress2 _
                                                                 , .BookDestAddress3 = p.BookDestAddress3 _
                                                                 , .BookDestCity = p.BookDestCity _
                                                                 , .BookDestCountry = p.BookDestCountry _
                                                                 , .BookDestName = p.BookDestName _
                                                                 , .BookDestState = p.BookDestState _
                                                                 , .BookDestZip = p.BookDestZip _
                                                                 , .BookDoNotInvoice = p.BookDoNotInvoice _
                                                                 , .BookFinCommStd = If(p.BookFinCommStd, 0) _
                                                                 , .BookFinServiceFee = p.BookFinServiceFee _
                                                                 , .BookLoadCom = p.BookLoadCom _
                                                                 , .BookLoadPONumber = p.BookLoadPONumber _
                                                                 , .BookMilesFrom = p.BookMilesFrom _
                                                                 , .BookOrderSequence = p.BookOrderSequence _
                                                                 , .BookOrigAddress1 = p.BookOrigAddress1 _
                                                                 , .BookOrigAddress2 = p.BookOrigAddress2 _
                                                                 , .BookOrigAddress3 = p.BookOrigAddress3 _
                                                                 , .BookOrigCity = p.BookOrigCity _
                                                                 , .BookOrigCountry = p.BookOrigCountry _
                                                                 , .BookOrigName = p.BookOrigName _
                                                                 , .BookOrigState = p.BookOrigState _
                                                                 , .BookOrigZip = p.BookOrigZip _
                                                                 , .BookPickNumber = p.BookPickNumber _
                                                                 , .BookPickupStopNumber = p.BookPickupStopNumber _
                                                                 , .BookProNumber = p.BookProNumber _
                                                                 , .BookRevCarrierCost = p.BookRevCarrierCost _
                                                                 , .BookRevCommCost = If(p.BookRevCommCost, 0) _
                                                                 , .BookRevFreightTax = p.BookRevFreightTax _
                                                                 , .BookRevGrossRevenue = If(p.BookRevGrossRevenue, 0) _
                                                                 , .BookRevLoadSavings = p.BookRevLoadSavings _
                                                                 , .BookRevNetCost = p.BookRevNetCost _
                                                                 , .BookRevOtherCost = p.BookRevOtherCost _
                                                                 , .BookRevTotalCost = p.BookRevTotalCost _
                                                                 , .BookRouteConsFlag = p.BookRouteConsFlag _
                                                                 , .BookRouteFinalCode = p.BookRouteFinalCode _
                                                                 , .BookRouteFinalDate = p.BookRouteFinalDate _
                                                                 , .BookShipCarrierNumber = p.BookShipCarrierNumber _
                                                                 , .BookShipCarrierProNumber = p.BookShipCarrierProNumber _
                                                                 , .BookStopNo = p.BookStopNo _
                                                                 , .BookTotalBFC = p.BookTotalBFC _
                                                                 , .BookTotalCases = p.BookTotalCases _
                                                                 , .BookTotalCube = p.BookTotalCube _
                                                                 , .BookTotalPL = p.BookTotalPL _
                                                                 , .BookTotalWgt = p.BookTotalWgt _
                                                                 , .BookTransType = p.BookTransType _
                                                                 , .BookTypeCode = p.BookTypeCode _
                                                                 , .BookWarehouseNumber = p.BookWarehouseNumber _
                                                                 , .CarrierEquipmentCodes = p.CarrierEquipmentCodes _
                                                                 , .CarrierName = p.CarrierName _
                                                                 , .CarrierNumber = p.CarrierNumber _
                                                                 , .CommCodeDescription = p.CommCodeDescription _
                                                                 , .CompName = p.CompName _
                                                                 , .CompNatNumber = p.CompNatNumber _
                                                                 , .CompNumber = p.CompNumber _
                                                                 , .FuelSurCharge = p.FuelSurCharge _
                                                                 , .LaneComments = p.LaneComments _
                                                                 , .LaneNumber = p.LaneNumber _
                                                                 , .LoadOrder = p.LoadOrder _
                                                                 , .TotalNonFuelFees = p.TotalNonFuelFees}).Take(rowsToTake).ToList
                If oPData Is Nothing OrElse oPData.Count < 1 Then
                    Return ProcessDataReturnValues.nglDataIntegrationComplete 'no data processs complete
                End If
                Dim intRowCount As Integer = 0

                'Loop through all of the records checking for maxrowsreturned
                For Each oRow In oPData
                    Try
                        If MaxRetry > 0 AndAlso oRow.PLExportRetry.HasValue AndAlso oRow.PLExportRetry > MaxRetry Then
                            'we do not export this record we add this record to the group email error message.
                            GroupEmailMsg &= "<br /> The following pick list record has exceeded the maximum number or export retries: " _
                                        & "<br /> PL Control: " & oRow.PLControl.ToString _
                                        & "<br /> Book Pro Number: " & oRow.BookProNumber _
                                        & "<br /> Order Number: " & oRow.BookCarrOrderNumber _
                                        & "<br /> Order Sequence: " & oRow.BookOrderSequence _
                                        & "<br /> Company Number: " & oRow.CompNumber _
                                        & "<br /> Retry # " & oRow.PLExportRetry & " of " & MaxRetry.ToString _
                                        & "<br />" & vbCrLf

                        Else
                            TotalRecords += 1
                            'add the data to the export list
                            oPickList.Add(oRow)
                            'add the details
                            Dim thisCompNumber As String = oRow.CompNumber
                            Dim thisPLControl As Int64 = oRow.PLControl
                            Dim thisOrderSequence = oRow.BookOrderSequence
                            Dim thisBookProNumber = oRow.BookProNumber
                            Dim thisCompNatNumber = oRow.CompNatNumber
                            Dim thisOrderNumber = oRow.BookCarrOrderNumber
                            Dim opd As List(Of clsPickDetailObject60) = (From d In db.vIntegrationItemDetails _
                                      Where d.BookProNumber = thisBookProNumber _
                                      Select New clsPickDetailObject60 With {.BFC = If(d.BookItemBFC, 0) _
                                                                             , .BookCarrOrderNumber = thisOrderNumber _
                                                                            , .BookProNumber = thisBookProNumber _
                                                                            , .Brand = d.BookItemBrand _
                                                                            , .CompNatNumber = thisCompNatNumber _
                                                                            , .CostCenter = d.BookItemCostCenter _
                                                                            , .CountryOfOrigin = d.BookItemCountryOfOrigin _
                                                                            , .Cube = If(d.BookItemCube, 0) _
                                                                            , .CustItemNumber = d.BookCustItemNumber _
                                                                            , .CustomerNumber = thisCompNumber _
                                                                            , .CustomerPONumber = d.BookLoadPONumber _
                                                                            , .Description = d.BookItemDescription _
                                                                            , .DOTCode = d.BookItemDOTCode _
                                                                            , .FAKClass = d.BookItemFAKClass _
                                                                            , .FreightCost = If(d.BookItemFreightCost, 0) _
                                                                            , .GTIN = d.BookItemGTIN _
                                                                            , .Hazmat = d.BookItemHazmat _
                                                                            , .Hazmat49CFRCode = d.BookItem49CFRCode _
                                                                            , .HazmatTypeCode = d.BookItemHazmatTypeCode _
                                                                            , .Highs = d.BookItemHighs _
                                                                            , .HST = d.BookItemHST _
                                                                            , .IATACode = d.BookItemIATACode _
                                                                            , .ItemCost = If(d.BookItemItemCost, 0) _
                                                                            , .ItemNumber = d.BookItemItemNumber _
                                                                            , .LevelOfDensity = d.BookItemLevelOfDensity _
                                                                            , .LimitedQtyFlag = d.BookItemLimitedQtyFlag _
                                                                            , .LotExpirationDate = If(d.BookItemLotExpirationDate, "") _
                                                                            , .LotNumber = d.BookItemLotNumber _
                                                                            , .MarineCode = d.BookItemMarineCode _
                                                                            , .NMFCClass = d.BookItemNMFCClass _
                                                                            , .OrderSequence = thisOrderSequence _
                                                                            , .Pack = If(d.BookItemPack, 0) _
                                                                            , .Pallets = d.BookItemPallets _
                                                                            , .PalletType = d.PalletType _
                                                                            , .QtyHeight = d.BookItemQtyHeight _
                                                                            , .QtyLength = d.BookItemQtyLength _
                                                                            , .QtyOrdered = If(d.BookItemQtyOrdered, 0) _
                                                                            , .QtyPalletPercentage = d.BookItemQtyPalletPercentage _
                                                                            , .QtyWidth = d.BookItemQtyWidth _
                                                                            , .Size = d.BookItemSize _
                                                                            , .Stackable = d.BookItemStackable _
                                                                            , .Ties = d.BookItemTies _
                                                                            , .Weight = If(d.BookItemWeight, 0)}).ToList

                            If Not opd Is Nothing AndAlso opd.Count > 0 Then
                                For Each d In opd
                                    oPickDetails.Add(d)
                                Next
                            End If

                            Try
                                If Me.AutoConfirmation Then
                                    If Me.UpdateStatus(oRow.PLControl, Now, True) Then
                                        Log("Pick List Auto Confirmation Complete For PL Control " & oRow.PLControl.ToString & ".")
                                    End If
                                Else
                                    Me.UpdateStatus(oRow.PLControl, Now, False)
                                End If
                            Catch ex As System.ApplicationException
                                'Log the exception and move on to the next record
                                LogException("Pick List Export Update Status Error (duplicate export possible)", "Could not update the export status for pick list control number " & oRow.PLControl.ToString & ".", AdminEmail, ex, "NGL.FreightMaster.Integration.clsPickList.readObjectData60 Failure")
                            Catch ex As Exception
                                'this is an unexpected error so re throw it
                                Throw
                            End Try
                        End If
                    Catch ex As System.Data.SqlClient.SqlException
                        ItemErrors += 1
                        ITEmailMsg &= "<br />SQL Data Exception while reading Pick List Details: " & ex.ToString & " on order number " & oRow.BookCarrOrderNumber & "<br />" & vbCrLf
                    Catch ex As InvalidOperationException
                        ItemErrors += 1
                        ITEmailMsg &= "<br /> No Data or Invalid Operation Exception while reading Pick List Detail: " & ex.ToString & " on order number " & oRow.BookCarrOrderNumber & "<br />" & vbCrLf

                    Catch ex As System.ApplicationException
                        ItemErrors += 1
                        ITEmailMsg &= "<br /> Application Error: " & ex.ToString & " on order number " & oRow.BookCarrOrderNumber & "<br />" & vbCrLf
                    Catch ex As Exception
                        Throw
                    End Try
                Next
            End Using
            If GroupEmailMsg.Trim.Length > 0 Then
                LogError("Pick List Export Data Integration Warning", "The following warnings were reported while running the Pick List Export data integration routine.  If the maximum number of retries for a record has been reported the Pick List record was not exported and may only be exported manually by an administrator." & GroupEmailMsg, GroupEmail)
            End If

            If ITEmailMsg.Trim.Length > 0 Then
                LogError("Pick List Export Error Report", "The following errors were reported while running the Pick List oExport data integration routine:" & ITEmailMsg, AdminEmail)
            End If
            If Me.TotalRecords > 0 Then
                strMsg = "Success! " & Me.TotalRecords & " Pick List records were exported." & vbCrLf
                oRet = ProcessDataReturnValues.nglDataIntegrationComplete
                If Me.RecordErrors > 0 Or Me.ItemErrors > 0 Then
                    If Me.RecordErrors > 0 Then
                        strMsg &= " ERROR!  " & Me.RecordErrors & " Pick List records could not be exported.  Please check the email error report or database error log records for more information." & vbCrLf
                    End If
                    If Me.ItemErrors > 0 Then
                        strMsg &= " ERROR!  " & Me.ItemErrors & " Pick List Detail records could not be exported.  Please check the email error report or database error log records for more information." & vbCrLf
                    End If
                    oRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                End If
            ElseIf StatusUpdateErrors > 0 Then
                strMsg = "WARNING! No Pick List records were available for export and " _
                    & StatusUpdateErrors & " Pick List records did not have their status flags updated.  Duplicate transmission of pick list records is possible.  Please check the email error report or database error log records for more information."
                oRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
            Else
                strMsg = "No Pick List records were available for export."
                oRet = ProcessDataReturnValues.nglDataIntegrationComplete
            End If
            Log(strMsg)
        Catch ex As System.Data.SqlClient.SqlException
            oRet = ProcessDataReturnValues.nglDataIntegrationFailure
            LogException("SQL Data Exception while reading Pick List Data", "Could not read pick list data records.", AdminEmail, ex, "NGL.FreightMaster.Integration.clsPickList.readObjectData60 Failure")
        Catch ex As InvalidOperationException
            oRet = ProcessDataReturnValues.nglDataIntegrationFailure
            LogException("No Data or Invalid Operation Exception while reading Pick List Data", "Could not read pick list data records.", AdminEmail, ex, "NGL.FreightMaster.Integration.clsPickList.readObjectData60 Failure")

        Catch ex As Exception
            oRet = ProcessDataReturnValues.nglDataIntegrationFailure
            LogException("Pick List Read Object Data Failure", "Could not read pick list data records.", AdminEmail, ex, "NGL.FreightMaster.Integration.clsPickList.readObjectData60 Failure")
        Finally
            Try
                closeConnection()
            Catch ex As Exception

            End Try
        End Try
        Return oRet
    End Function

    ''' <summary>
    ''' Reads Object Data For v-7.0 for Pick List information from the database
    ''' </summary>
    ''' <param name="oPickList"></param>
    ''' <param name="strConnection"></param>
    ''' <param name="MaxRetry"></param>
    ''' <param name="RetryMinutes"></param>
    ''' <param name="CompLegalEntity"></param>
    ''' <param name="oPickListFees"></param>
    ''' <param name="oPickListDetails"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR v-7.0.5.100 5/12/2016
    '''   We now call a new stored procedure to get the item details
    '''   So we can support Billed Cost for AP and Contracted Cost for Pick List
    ''' </remarks>
    Public Function readObjectData70(ByRef oPickList() As clsPickListObject70, _
                                     ByVal strConnection As String, _
                                     Optional ByVal MaxRetry As Integer = 0, _
                                     Optional ByVal RetryMinutes As Integer = 0, _
                                     Optional ByVal CompLegalEntity As String = Nothing, _
                                     Optional ByRef oPickListFees() As clsPickListFeeObject70 = Nothing, _
                                     Optional ByRef oPickListDetails() As clsPickDetailObject70 = Nothing) As ProcessDataReturnValues
#Disable Warning BC42024 ' Unused local variable: 'TestDate'.
        Dim TestDate As Nullable(Of Date)
#Enable Warning BC42024 ' Unused local variable: 'TestDate'.
        Dim strMsg As String = ""
        Dim strTitle As String = ""
        Dim intErrors As Integer = 0
        Dim intCount As Integer = 0
        Dim strSource As String = "clsPickList.readData"
        GroupEmailMsg = ""
        ITEmailMsg = ""
        Me.ImportTypeKey = IntegrationTypes.PickList
        Me.CreatedDate = Now.ToString
        Me.CreateUser = "Data Integration DLL"
        Me.Source = "Pick List Data Integration"
        Dim oRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationComplete
        Me.DBConnection = strConnection
        'try the connection
        If Not Me.openConnection Then
            Return ProcessDataReturnValues.nglDataConnectionFailure
        End If
        Dim oPLET As New PickListData.PickListDataTable
        Dim oPLETD As New PickListData.PickDetailDataTable

        'Dim TestDate As Nullable(Of Date)
        'If RetryMinutes > 0 Then TestDate = Date.Now.AddMinutes(-RetryMinutes)

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

            Dim oPLWCFData = New DAL.NGLAPMassEntryData(oWCFParameters)
            'Dim oPLWCFData = New DAL.NGLtblPickListData(oWCFParameters)
            Dim oPLWCFRets = oPLWCFData.GetPickListData70(MaxRetry, RetryMinutes, CompLegalEntity, Me.MaxRowsReturned)

            Dim oPLExportDet As New List(Of clsPickDetailObject70)
            Dim oPLExportFee As New List(Of clsPickListFeeObject70)

            If Not oPLWCFRets Is Nothing AndAlso oPLWCFRets.Count > 0 Then
                ReDim oPickList(oPLWCFRets.Count - 1)

                Dim intCt As Integer = 0

                For Each PLRow In oPLWCFRets

                    Dim oPL As New clsPickListObject70

                    oPL.PLControl = PLRow.PLControl
                    oPL.PLExportRetry = PLRow.PLExportRetry
                    oPL.PLExportDate = PLRow.PLExportDate
                    oPL.PLExported = PLRow.PLExported
                    oPL.BookSHID = If(PLRow.BookSHID, 0)
                    oPL.CarrierNumber = PLRow.CarrierNumber
                    oPL.CarrierAlphaCode = PLRow.CarrierAlphaCode
                    oPL.CarrierLegalEntity = PLRow.CarrierLegalEntity
                    oPL.CarrierName = PLRow.CarrierName
                    oPL.CompLegalEntity = PLRow.CompLegalEntity
                    oPL.CompNumber = PLRow.CompNumber
                    oPL.CompName = PLRow.CompName
                    oPL.CompAlphaCode = PLRow.CompAlphaCode
                    oPL.CompNatNumber = PLRow.CompNatNumber
                    oPL.LaneLegalEntity = PLRow.LaneLegalEntity
                    oPL.LaneNumber = PLRow.LaneNumber
                    oPL.BookOriginalLaneNumber = PLRow.BookOriginalLaneNumber
                    oPL.BookOriginalLaneLegalEntity = PLRow.BookOriginalLaneLegalEntity
                    oPL.BookAlternateAddressLaneNumber = PLRow.BookAlternateAddressLaneNumber
                    oPL.BookCarrOrderNumber = PLRow.BookCarrOrderNumber
                    oPL.BookOrderSequence = PLRow.BookOrderSequence
                    oPL.BookConsPrefix = PLRow.BookConsPrefix
                    oPL.BookRouteConsFlag = PLRow.BookRouteConsFlag
                    oPL.LoadOrder = PLRow.LoadOrder
                    oPL.BookDateLoad = PLRow.BookDateLoad
                    oPL.BookDateRequired = PLRow.BookDateRequired
                    oPL.BookLoadCom = PLRow.BookLoadCom
                    oPL.BookProNumber = PLRow.BookProNumber
                    oPL.BookRouteFinalCode = PLRow.BookRouteFinalCode
                    oPL.BookRouteFinalDate = PLRow.BookRouteFinalDate
                    oPL.BookTotalCases = PLRow.BookTotalCases
                    oPL.BookTotalWgt = PLRow.BookTotalWgt
                    oPL.BookTotalPL = PLRow.BookTotalPL
                    oPL.BookTotalCube = PLRow.BookTotalCube
                    oPL.BookStopNo = PLRow.BookStopNo
                    oPL.BookTypeCode = PLRow.BookTypeCode
                    oPL.BookDateOrdered = PLRow.BookDateOrdered
                    oPL.BookOrigName = PLRow.BookOrigName
                    oPL.BookOrigAddress1 = PLRow.BookOrigAddress1
                    oPL.BookOrigAddress2 = PLRow.BookOrigAddress2
                    oPL.BookOrigAddress3 = PLRow.BookOrigAddress3
                    oPL.BookOrigCity = PLRow.BookOrigCity
                    oPL.BookOrigState = PLRow.BookOrigState
                    oPL.BookOrigCountry = PLRow.BookOrigCountry
                    oPL.BookOrigZip = PLRow.BookOrigZip
                    oPL.BookDestName = PLRow.BookDestName
                    oPL.BookDestAddress1 = PLRow.BookDestAddress1
                    oPL.BookDestAddress2 = PLRow.BookDestAddress2
                    oPL.BookDestAddress3 = PLRow.BookDestAddress3
                    oPL.BookDestCity = PLRow.BookDestCity
                    oPL.BookDestState = PLRow.BookDestState
                    oPL.BookDestCountry = PLRow.BookDestCountry
                    oPL.BookDestZip = PLRow.BookDestZip
                    oPL.BookLoadPONumber = PLRow.BookLoadPONumber
                    oPL.CommCodeDescription = PLRow.CommCodeDescription
                    oPL.BookMilesFrom = PLRow.BookMilesFrom
                    oPL.BookCommCompControl = If(PLRow.BookCommCompControl, 0)
                    oPL.BookFinCommStd = If(PLRow.BookFinCommStd, 0)
                    oPL.BookDoNotInvoice = PLRow.BookDoNotInvoice
                    oPL.CarrierEquipmentCodes = PLRow.CarrierEquipmentCodes
                    oPL.BookCarrierTypeCode = PLRow.BookCarrierTypeCode
                    oPL.BookWarehouseNumber = PLRow.BookWarehouseNumber
                    oPL.BookWhseAuthorizationNo = PLRow.BookWhseAuthorizationNo
                    oPL.BookTransType = PLRow.BookTransType
                    oPL.BookShipCarrierProNumber = PLRow.BookShipCarrierProNumber
                    oPL.BookShipCarrierNumber = PLRow.BookShipCarrierNumber
                    oPL.BookShipCarrierName = PLRow.BookShipCarrierName
                    oPL.BookShipCarrierDetails = PLRow.BookShipCarrierDetails
                    oPL.LaneComments = PLRow.LaneComments
                    oPL.FuelSurCharge = PLRow.FuelSurCharge
                    oPL.BookTotalBFC = PLRow.BookTotalBFC
                    oPL.BookRevTotalCost = PLRow.BookRevTotalCost
                    oPL.BookRevCommCost = If(PLRow.BookRevCommCost, 0)
                    oPL.BookRevGrossRevenue = If(PLRow.BookRevGrossRevenue, 0)
                    oPL.BookRevCarrierCost = PLRow.BookRevCarrierCost
                    oPL.BookRevOtherCost = PLRow.BookRevOtherCost
                    oPL.BookRevNetCost = PLRow.BookRevNetCost
                    oPL.BookRevNonTaxable = If(PLRow.BookRevNonTaxable, 0)
                    oPL.BookRevFreightTax = PLRow.BookRevFreightTax
                    oPL.BookFinServiceFee = PLRow.BookFinServiceFee
                    oPL.BookRevLoadSavings = PLRow.BookRevLoadSavings
                    oPL.TotalNonFuelFees = PLRow.TotalNonFuelFees
                    oPL.BookPickNumber = PLRow.BookPickNumber
                    oPL.BookPickupStopNumber = PLRow.BookPickupStopNumber
                    oPL.BookFinAPGLNumber = PLRow.BookFinAPGLNumber
                    oPickList(intCt) = oPL

                    Dim BookControl As Integer = PLRow.BookControl
                    Dim hdrPLControl As Integer = oPL.PLControl
                    Dim hdrCompNumber As Integer = oPL.CompNumber
                    Dim hdrCompAlphaCode As String = oPL.CompAlphaCode
                    Dim hdrCompLegalEntity As String = oPL.CompLegalEntity
                    Dim hdrCompNatNumber As Integer = oPL.CompNatNumber

                    Dim oPLWCFDetails = oPLWCFData.GetExportPickDetailRows70(BookControl)

                    If Not oPLWCFDetails Is Nothing AndAlso oPLWCFDetails.Count > 0 Then
                        For Each d In oPLWCFDetails
                            Dim nDetail As New clsPickDetailObject70
                            With nDetail
                                .PLControl = hdrPLControl
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
                            oPLExportDet.Add(nDetail)
                        Next
                    End If

                    Dim oPLWCFFees = oPLWCFData.GetExportFeeRows70(BookControl)

                    If Not oPLWCFFees Is Nothing AndAlso oPLWCFFees.Count > 0 Then
                        For Each f In oPLWCFFees
                            Dim nFee As New clsPickListFeeObject70
                            With nFee
                                .PLControl = hdrPLControl
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
                            oPLExportFee.Add(nFee)
                        Next
                    End If
                    Try
                        If Me.AutoConfirmation Then
                            If Me.UpdateStatus(oPL.PLControl, Now, True) Then
                                Log("Pick List Auto Confirmation Complete For PL Control " & oPL.PLControl.ToString & ".")
                            End If
                        Else
                            Me.UpdateStatus(oPL.PLControl, Now, False)
                        End If
                    Catch ex As System.ApplicationException
                        'Log the exception and move on to the next record
                        LogException("Pick List Export Update Status Error (duplicate export possible)", "Could not update the export status for pick list control number " & oPL.PLControl.ToString & ".", AdminEmail, ex, "NGL.FreightMaster.Integration.clsPickList.readObjectData70 Failure")
                    Catch ex As Exception
                        'this is an unexpected error so re throw it
                        Throw
                    End Try

                    intCt += 1
                    If intCt > oPLWCFRets.Count Then Exit For
                Next

            End If
            oPickListDetails = oPLExportDet.ToArray()
            oPickListFees = oPLExportFee.ToArray()

            'Catch ex As DAL.FaultException(Of SqlFaultInfo)'
        Catch ex As Exception
            oRet = ProcessDataReturnValues.nglDataIntegrationFailure
            LogException("PickList Read Object Data 70 Failure", "Could not read Pick List records.", AdminEmail, ex, "NGL.FreightMaster.Integeration.clsPickList.readObjectData70 Failure")
        Finally
            Try
                closeConnection()
            Catch ex As Exception

            End Try
        End Try
        Return oRet
    End Function


    ''' <summary>
    ''' Reads Object Data For v-8.0 for Pick List information from the database
    ''' </summary>
    ''' <param name="oPickList"></param>
    ''' <param name="strConnection"></param>
    ''' <param name="MaxRetry"></param>
    ''' <param name="RetryMinutes"></param>
    ''' <param name="CompLegalEntity"></param>
    ''' <param name="oPickListFees"></param>
    ''' <param name="oPickListDetails"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR v-8.2.0.117 7/17/2019
    '''   replaces the 70 version Of the data
    '''   includes BookItemOrderNumber
    ''' </remarks>
    Public Function readObjectData80(ByRef oPickList() As clsPickListObject80,
                                     ByVal strConnection As String,
                                     Optional ByVal MaxRetry As Integer = 0,
                                     Optional ByVal RetryMinutes As Integer = 0,
                                     Optional ByVal CompLegalEntity As String = Nothing,
                                     Optional ByRef oPickListFees() As clsPickListFeeObject80 = Nothing,
                                     Optional ByRef oPickListDetails() As clsPickDetailObject80 = Nothing) As ProcessDataReturnValues
#Disable Warning BC42024 ' Unused local variable: 'TestDate'.
        Dim TestDate As Nullable(Of Date)
#Enable Warning BC42024 ' Unused local variable: 'TestDate'.
        Dim strMsg As String = ""
        Dim strTitle As String = ""
        Dim intErrors As Integer = 0
        Dim intCount As Integer = 0
        Dim strSource As String = "clsPickList.readData"
        GroupEmailMsg = ""
        ITEmailMsg = ""
        Me.ImportTypeKey = IntegrationTypes.PickList
        Me.CreatedDate = Now.ToString
        Me.CreateUser = "Data Integration DLL"
        Me.Source = "Pick List Data Integration"
        Dim oRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationComplete
        Me.DBConnection = strConnection
        'try the connection
        If Not Me.openConnection Then
            Return ProcessDataReturnValues.nglDataConnectionFailure
        End If
        Dim oPLET As New PickListData.PickListDataTable
        Dim oPLETD As New PickListData.PickDetailDataTable

        'Dim TestDate As Nullable(Of Date)
        'If RetryMinutes > 0 Then TestDate = Date.Now.AddMinutes(-RetryMinutes)

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

            Dim oPLWCFData = New DAL.NGLAPMassEntryData(oWCFParameters)
            'Dim oPLWCFData = New DAL.NGLtblPickListData(oWCFParameters)
            Dim oPLWCFRets = oPLWCFData.GetPickListData70(MaxRetry, RetryMinutes, CompLegalEntity, Me.MaxRowsReturned)

            Dim oPLExportDet As New List(Of clsPickDetailObject80)
            Dim oPLExportFee As New List(Of clsPickListFeeObject80)

            If Not oPLWCFRets Is Nothing AndAlso oPLWCFRets.Count > 0 Then
                ReDim oPickList(oPLWCFRets.Count - 1)

                Dim intCt As Integer = 0

                For Each PLRow In oPLWCFRets

                    Dim oPL As New clsPickListObject80

                    oPL.PLControl = PLRow.PLControl
                    oPL.PLExportRetry = PLRow.PLExportRetry
                    oPL.PLExportDate = PLRow.PLExportDate
                    oPL.PLExported = PLRow.PLExported
                    oPL.BookSHID = If(PLRow.BookSHID, 0)
                    oPL.CarrierNumber = PLRow.CarrierNumber
                    oPL.CarrierAlphaCode = PLRow.CarrierAlphaCode
                    oPL.CarrierLegalEntity = PLRow.CarrierLegalEntity
                    oPL.CarrierName = PLRow.CarrierName
                    oPL.CompLegalEntity = PLRow.CompLegalEntity
                    oPL.CompNumber = PLRow.CompNumber
                    oPL.CompName = PLRow.CompName
                    oPL.CompAlphaCode = PLRow.CompAlphaCode
                    oPL.CompNatNumber = PLRow.CompNatNumber
                    oPL.LaneLegalEntity = PLRow.LaneLegalEntity
                    oPL.LaneNumber = PLRow.LaneNumber
                    oPL.BookOriginalLaneNumber = PLRow.BookOriginalLaneNumber
                    oPL.BookOriginalLaneLegalEntity = PLRow.BookOriginalLaneLegalEntity
                    oPL.BookAlternateAddressLaneNumber = PLRow.BookAlternateAddressLaneNumber
                    oPL.BookCarrOrderNumber = PLRow.BookCarrOrderNumber
                    oPL.BookOrderSequence = PLRow.BookOrderSequence
                    oPL.BookConsPrefix = PLRow.BookConsPrefix
                    oPL.BookRouteConsFlag = PLRow.BookRouteConsFlag
                    oPL.LoadOrder = PLRow.LoadOrder
                    oPL.BookDateLoad = PLRow.BookDateLoad
                    oPL.BookDateRequired = PLRow.BookDateRequired
                    oPL.BookLoadCom = PLRow.BookLoadCom
                    oPL.BookProNumber = PLRow.BookProNumber
                    oPL.BookRouteFinalCode = PLRow.BookRouteFinalCode
                    oPL.BookRouteFinalDate = PLRow.BookRouteFinalDate
                    oPL.BookTotalCases = PLRow.BookTotalCases
                    oPL.BookTotalWgt = PLRow.BookTotalWgt
                    oPL.BookTotalPL = PLRow.BookTotalPL
                    oPL.BookTotalCube = PLRow.BookTotalCube
                    oPL.BookStopNo = PLRow.BookStopNo
                    oPL.BookTypeCode = PLRow.BookTypeCode
                    oPL.BookDateOrdered = PLRow.BookDateOrdered
                    oPL.BookOrigName = PLRow.BookOrigName
                    oPL.BookOrigAddress1 = PLRow.BookOrigAddress1
                    oPL.BookOrigAddress2 = PLRow.BookOrigAddress2
                    oPL.BookOrigAddress3 = PLRow.BookOrigAddress3
                    oPL.BookOrigCity = PLRow.BookOrigCity
                    oPL.BookOrigState = PLRow.BookOrigState
                    oPL.BookOrigCountry = PLRow.BookOrigCountry
                    oPL.BookOrigZip = PLRow.BookOrigZip
                    oPL.BookDestName = PLRow.BookDestName
                    oPL.BookDestAddress1 = PLRow.BookDestAddress1
                    oPL.BookDestAddress2 = PLRow.BookDestAddress2
                    oPL.BookDestAddress3 = PLRow.BookDestAddress3
                    oPL.BookDestCity = PLRow.BookDestCity
                    oPL.BookDestState = PLRow.BookDestState
                    oPL.BookDestCountry = PLRow.BookDestCountry
                    oPL.BookDestZip = PLRow.BookDestZip
                    oPL.BookLoadPONumber = PLRow.BookLoadPONumber
                    oPL.CommCodeDescription = PLRow.CommCodeDescription
                    oPL.BookMilesFrom = PLRow.BookMilesFrom
                    oPL.BookCommCompControl = If(PLRow.BookCommCompControl, 0)
                    oPL.BookFinCommStd = If(PLRow.BookFinCommStd, 0)
                    oPL.BookDoNotInvoice = PLRow.BookDoNotInvoice
                    oPL.CarrierEquipmentCodes = PLRow.CarrierEquipmentCodes
                    oPL.BookCarrierTypeCode = PLRow.BookCarrierTypeCode
                    oPL.BookWarehouseNumber = PLRow.BookWarehouseNumber
                    oPL.BookWhseAuthorizationNo = PLRow.BookWhseAuthorizationNo
                    oPL.BookTransType = PLRow.BookTransType
                    oPL.BookShipCarrierProNumber = PLRow.BookShipCarrierProNumber
                    oPL.BookShipCarrierNumber = PLRow.BookShipCarrierNumber
                    oPL.BookShipCarrierName = PLRow.BookShipCarrierName
                    oPL.BookShipCarrierDetails = PLRow.BookShipCarrierDetails
                    oPL.LaneComments = PLRow.LaneComments
                    oPL.FuelSurCharge = PLRow.FuelSurCharge
                    oPL.BookTotalBFC = PLRow.BookTotalBFC
                    oPL.BookRevTotalCost = PLRow.BookRevTotalCost
                    oPL.BookRevCommCost = If(PLRow.BookRevCommCost, 0)
                    oPL.BookRevGrossRevenue = If(PLRow.BookRevGrossRevenue, 0)
                    oPL.BookRevCarrierCost = PLRow.BookRevCarrierCost
                    oPL.BookRevOtherCost = PLRow.BookRevOtherCost
                    oPL.BookRevNetCost = PLRow.BookRevNetCost
                    oPL.BookRevNonTaxable = If(PLRow.BookRevNonTaxable, 0)
                    oPL.BookRevFreightTax = PLRow.BookRevFreightTax
                    oPL.BookFinServiceFee = PLRow.BookFinServiceFee
                    oPL.BookRevLoadSavings = PLRow.BookRevLoadSavings
                    oPL.TotalNonFuelFees = PLRow.TotalNonFuelFees
                    oPL.BookPickNumber = PLRow.BookPickNumber
                    oPL.BookPickupStopNumber = PLRow.BookPickupStopNumber
                    oPL.BookFinAPGLNumber = PLRow.BookFinAPGLNumber
                    oPickList(intCt) = oPL

                    Dim BookControl As Integer = PLRow.BookControl
                    Dim hdrPLControl As Integer = oPL.PLControl
                    Dim hdrCompNumber As Integer = oPL.CompNumber
                    Dim hdrCompAlphaCode As String = oPL.CompAlphaCode
                    Dim hdrCompLegalEntity As String = oPL.CompLegalEntity
                    Dim hdrCompNatNumber As Integer = oPL.CompNatNumber

                    Dim oPLWCFDetails = oPLWCFData.GetExportPickDetailRows80(BookControl)

                    If Not oPLWCFDetails Is Nothing AndAlso oPLWCFDetails.Count > 0 Then
                        For Each d In oPLWCFDetails
                            Dim nDetail As New clsPickDetailObject80
                            With nDetail
                                .PLControl = hdrPLControl
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
                            oPLExportDet.Add(nDetail)
                        Next
                    End If

                    Dim oPLWCFFees = oPLWCFData.GetExportFeeRows70(BookControl)

                    If Not oPLWCFFees Is Nothing AndAlso oPLWCFFees.Count > 0 Then
                        For Each f In oPLWCFFees
                            Dim nFee As New clsPickListFeeObject80
                            With nFee
                                .PLControl = hdrPLControl
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
                            oPLExportFee.Add(nFee)
                        Next
                    End If
                    Try
                        If Me.AutoConfirmation Then
                            If Me.UpdateStatus(oPL.PLControl, Now, True) Then
                                Log("Pick List Auto Confirmation Complete For PL Control " & oPL.PLControl.ToString & ".")
                            End If
                        Else
                            Me.UpdateStatus(oPL.PLControl, Now, False)
                        End If
                    Catch ex As System.ApplicationException
                        'Log the exception and move on to the next record
                        LogException("Pick List Export Update Status Error (duplicate export possible)", "Could not update the export status for pick list control number " & oPL.PLControl.ToString & ".", AdminEmail, ex, "NGL.FreightMaster.Integration.clsPickList.readObjectData70 Failure")
                    Catch ex As Exception
                        'this is an unexpected error so re throw it
                        Throw
                    End Try

                    intCt += 1
                    If intCt > oPLWCFRets.Count Then Exit For
                Next

            End If
            oPickListDetails = oPLExportDet.ToArray()
            oPickListFees = oPLExportFee.ToArray()

            'Catch ex As DAL.FaultException(Of SqlFaultInfo)'
        Catch ex As Exception
            oRet = ProcessDataReturnValues.nglDataIntegrationFailure
            LogException("PickList Read Object Data 80 Failure", "Could not read Pick List records.", AdminEmail, ex, "NGL.FreightMaster.Integeration.clsPickList.readObjectData80 Failure")
        Finally
            Try
                closeConnection()
            Catch ex As Exception

            End Try
        End Try
        Return oRet
    End Function



    ''' <summary>
    ''' Reads Object Data For v-8.5 for Pick List information from the database
    ''' </summary>
    ''' <param name="oPickList"></param>
    ''' <param name="strConnection"></param>
    ''' <param name="MaxRetry"></param>
    ''' <param name="RetryMinutes"></param>
    ''' <param name="CompLegalEntity"></param>
    ''' <param name="oPickListFees"></param>
    ''' <param name="oPickListDetails"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR v-8.5.0.002 12/13/2021
    ''' </remarks>
    Public Function readObjectData85(ByRef oPickList() As clsPickListObject85,
                                     ByVal strConnection As String,
                                     Optional ByVal MaxRetry As Integer = 0,
                                     Optional ByVal RetryMinutes As Integer = 0,
                                     Optional ByVal CompLegalEntity As String = Nothing,
                                     Optional ByRef oPickListFees() As clsPickListFeeObject85 = Nothing,
                                     Optional ByRef oPickListDetails() As clsPickDetailObject85 = Nothing) As ProcessDataReturnValues
#Disable Warning BC42024 ' Unused local variable: 'TestDate'.
        Dim TestDate As Nullable(Of Date)
#Enable Warning BC42024 ' Unused local variable: 'TestDate'.
        Dim strMsg As String = ""
        Dim strTitle As String = ""
        Dim intErrors As Integer = 0
        Dim intCount As Integer = 0
        Dim strSource As String = "clsPickList.readData"
        GroupEmailMsg = ""
        ITEmailMsg = ""
        Me.ImportTypeKey = IntegrationTypes.PickList
        Me.CreatedDate = Now.ToString
        Me.CreateUser = "Data Integration DLL"
        Me.Source = "Pick List Data Integration"
        Dim oRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationComplete
        Me.DBConnection = strConnection
        'try the connection
        If Not Me.openConnection Then
            Return ProcessDataReturnValues.nglDataConnectionFailure
        End If
        Dim oPLET As New PickListData.PickListDataTable
        Dim oPLETD As New PickListData.PickDetailDataTable

        'Dim TestDate As Nullable(Of Date)
        'If RetryMinutes > 0 Then TestDate = Date.Now.AddMinutes(-RetryMinutes)

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

            Dim oPLWCFData = New DAL.NGLAPMassEntryData(oWCFParameters)
            'Dim oPLWCFData = New DAL.NGLtblPickListData(oWCFParameters)
            Dim oPLWCFRets = oPLWCFData.GetPickListData70(MaxRetry, RetryMinutes, CompLegalEntity, Me.MaxRowsReturned)

            Dim oPLExportDet As New List(Of clsPickDetailObject85)
            Dim oPLExportFee As New List(Of clsPickListFeeObject85)

            If Not oPLWCFRets Is Nothing AndAlso oPLWCFRets.Count > 0 Then
                ReDim oPickList(oPLWCFRets.Count - 1)

                Dim intCt As Integer = 0

                For Each PLRow In oPLWCFRets

                    Dim oPL As New clsPickListObject85

                    oPL.PLControl = PLRow.PLControl
                    oPL.PLExportRetry = PLRow.PLExportRetry
                    oPL.PLExportDate = PLRow.PLExportDate
                    oPL.PLExported = PLRow.PLExported
                    oPL.BookSHID = If(PLRow.BookSHID, 0)
                    oPL.CarrierNumber = PLRow.CarrierNumber
                    oPL.CarrierAlphaCode = PLRow.CarrierAlphaCode
                    oPL.CarrierLegalEntity = PLRow.CarrierLegalEntity
                    oPL.CarrierName = PLRow.CarrierName
                    oPL.CompLegalEntity = PLRow.CompLegalEntity
                    oPL.CompNumber = PLRow.CompNumber
                    oPL.CompName = PLRow.CompName
                    oPL.CompAlphaCode = PLRow.CompAlphaCode
                    oPL.CompNatNumber = PLRow.CompNatNumber
                    oPL.LaneLegalEntity = PLRow.LaneLegalEntity
                    oPL.LaneNumber = PLRow.LaneNumber
                    oPL.BookOriginalLaneNumber = PLRow.BookOriginalLaneNumber
                    oPL.BookOriginalLaneLegalEntity = PLRow.BookOriginalLaneLegalEntity
                    oPL.BookAlternateAddressLaneNumber = PLRow.BookAlternateAddressLaneNumber
                    oPL.BookCarrOrderNumber = PLRow.BookCarrOrderNumber
                    oPL.BookOrderSequence = PLRow.BookOrderSequence
                    oPL.BookConsPrefix = PLRow.BookConsPrefix
                    oPL.BookRouteConsFlag = PLRow.BookRouteConsFlag
                    oPL.LoadOrder = PLRow.LoadOrder
                    oPL.BookDateLoad = PLRow.BookDateLoad
                    oPL.BookDateRequired = PLRow.BookDateRequired
                    oPL.BookLoadCom = PLRow.BookLoadCom
                    oPL.BookProNumber = PLRow.BookProNumber
                    oPL.BookRouteFinalCode = PLRow.BookRouteFinalCode
                    oPL.BookRouteFinalDate = PLRow.BookRouteFinalDate
                    oPL.BookTotalCases = PLRow.BookTotalCases
                    oPL.BookTotalWgt = PLRow.BookTotalWgt
                    oPL.BookTotalPL = PLRow.BookTotalPL
                    oPL.BookTotalCube = PLRow.BookTotalCube
                    oPL.BookStopNo = PLRow.BookStopNo
                    oPL.BookTypeCode = PLRow.BookTypeCode
                    oPL.BookDateOrdered = PLRow.BookDateOrdered
                    oPL.BookOrigName = PLRow.BookOrigName
                    oPL.BookOrigAddress1 = PLRow.BookOrigAddress1
                    oPL.BookOrigAddress2 = PLRow.BookOrigAddress2
                    oPL.BookOrigAddress3 = PLRow.BookOrigAddress3
                    oPL.BookOrigCity = PLRow.BookOrigCity
                    oPL.BookOrigState = PLRow.BookOrigState
                    oPL.BookOrigCountry = PLRow.BookOrigCountry
                    oPL.BookOrigZip = PLRow.BookOrigZip
                    oPL.BookDestName = PLRow.BookDestName
                    oPL.BookDestAddress1 = PLRow.BookDestAddress1
                    oPL.BookDestAddress2 = PLRow.BookDestAddress2
                    oPL.BookDestAddress3 = PLRow.BookDestAddress3
                    oPL.BookDestCity = PLRow.BookDestCity
                    oPL.BookDestState = PLRow.BookDestState
                    oPL.BookDestCountry = PLRow.BookDestCountry
                    oPL.BookDestZip = PLRow.BookDestZip
                    oPL.BookLoadPONumber = PLRow.BookLoadPONumber
                    oPL.CommCodeDescription = PLRow.CommCodeDescription
                    oPL.BookMilesFrom = PLRow.BookMilesFrom
                    oPL.BookCommCompControl = If(PLRow.BookCommCompControl, 0)
                    oPL.BookFinCommStd = If(PLRow.BookFinCommStd, 0)
                    oPL.BookDoNotInvoice = PLRow.BookDoNotInvoice
                    oPL.CarrierEquipmentCodes = PLRow.CarrierEquipmentCodes
                    oPL.BookCarrierTypeCode = PLRow.BookCarrierTypeCode
                    oPL.BookWarehouseNumber = PLRow.BookWarehouseNumber
                    oPL.BookWhseAuthorizationNo = PLRow.BookWhseAuthorizationNo
                    oPL.BookTransType = PLRow.BookTransType
                    oPL.BookShipCarrierProNumber = PLRow.BookShipCarrierProNumber
                    oPL.BookShipCarrierNumber = PLRow.BookShipCarrierNumber
                    oPL.BookShipCarrierName = PLRow.BookShipCarrierName
                    oPL.BookShipCarrierDetails = PLRow.BookShipCarrierDetails
                    oPL.LaneComments = PLRow.LaneComments
                    oPL.FuelSurCharge = PLRow.FuelSurCharge
                    oPL.BookTotalBFC = PLRow.BookTotalBFC
                    oPL.BookRevTotalCost = PLRow.BookRevTotalCost
                    oPL.BookRevCommCost = If(PLRow.BookRevCommCost, 0)
                    oPL.BookRevGrossRevenue = If(PLRow.BookRevGrossRevenue, 0)
                    oPL.BookRevCarrierCost = PLRow.BookRevCarrierCost
                    oPL.BookRevOtherCost = PLRow.BookRevOtherCost
                    oPL.BookRevNetCost = PLRow.BookRevNetCost
                    oPL.BookRevNonTaxable = If(PLRow.BookRevNonTaxable, 0)
                    oPL.BookRevFreightTax = PLRow.BookRevFreightTax
                    oPL.BookFinServiceFee = PLRow.BookFinServiceFee
                    oPL.BookRevLoadSavings = PLRow.BookRevLoadSavings
                    oPL.TotalNonFuelFees = PLRow.TotalNonFuelFees
                    oPL.BookPickNumber = PLRow.BookPickNumber
                    oPL.BookPickupStopNumber = PLRow.BookPickupStopNumber
                    oPL.BookFinAPGLNumber = PLRow.BookFinAPGLNumber
                    'Modified by RHR for v-8.5.0.002 on 12/03/2021 added Scheduler Fields
                    oPL.BookCarrTrailerNo = PLRow.BookCarrTrailerNo
                    oPL.BookCarrSealNo = PLRow.BookCarrSealNo
                    oPL.BookCarrDriverNo = PLRow.BookCarrDriverNo
                    oPL.BookCarrDriverName = PLRow.BookCarrDriverName
                    oPL.BookCarrRouteNo = PLRow.BookCarrRouteNo
                    oPL.BookCarrTripNo = PLRow.BookCarrTripNo
                    oPL.BookCarrApptDate = PLRow.BookCarrApptDate
                    oPL.BookCarrApptTime = PLRow.BookCarrApptTime
                    oPL.BookCarrActDate = PLRow.BookCarrActDate
                    oPL.BookCarrActTime = PLRow.BookCarrActTime
                    oPL.BookCarrStartUnloadingDate = PLRow.BookCarrStartUnloadingDate
                    oPL.BookCarrStartUnloadingTime = PLRow.BookcarrStartUnloadingTime
                    oPL.BookCarrFinishUnloadingDate = PLRow.BookCarrFinishUnloadingDate
                    oPL.BookCarrFinishUnloadingTime = PLRow.BookCarrfinishUnloadingTime
                    oPL.BookCarrActUnloadCompDate = PLRow.BookCarrActUnloadCompDate
                    oPL.BookCarrActUnloadCompTime = PLRow.BookCarrActUnloadCompTime
                    oPL.BookCarrScheduleDate = PLRow.BookCarrScheduleDate
                    oPL.BookCarrScheduleTime = PLRow.BookCarrScheduleTime
                    oPL.BookCarrActualDate = PLRow.BookCarrActualDate
                    oPL.BookCarrActualTime = PLRow.BookCarrActualtime
                    oPL.BookCarrStartLoadingDate = PLRow.BookCarrStartLoadingDate
                    oPL.BookCarrStartLoadingTime = PLRow.BookCarrStartLoadingTime
                    oPL.BookCarrFinishLoadingDate = PLRow.BookCarrFinishLoadingDate
                    oPL.BookCarrFinishLoadingTime = PLRow.BookCarrFinishLoadingTime
                    oPL.BookCarrActLoadComplete_Date = PLRow.BookCarrActLoadComplete_Date
                    oPL.BookCarrActLoadCompleteTime = PLRow.BookCarrActLoadCompleteTime

                    oPickList(intCt) = oPL

                    Dim BookControl As Integer = PLRow.BookControl
                    Dim hdrPLControl As Integer = oPL.PLControl
                    Dim hdrCompNumber As Integer = oPL.CompNumber
                    Dim hdrCompAlphaCode As String = oPL.CompAlphaCode
                    Dim hdrCompLegalEntity As String = oPL.CompLegalEntity
                    Dim hdrCompNatNumber As Integer = oPL.CompNatNumber

                    Dim oPLWCFDetails = oPLWCFData.GetExportPickDetailRows85(BookControl)

                    If Not oPLWCFDetails Is Nothing AndAlso oPLWCFDetails.Count > 0 Then
                        For Each d In oPLWCFDetails
                            Dim nDetail As New clsPickDetailObject85
                            With nDetail
                                .PLControl = hdrPLControl
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
                            oPLExportDet.Add(nDetail)
                        Next
                    End If

                    Dim oPLWCFFees = oPLWCFData.GetExportFeeRows70(BookControl)

                    If Not oPLWCFFees Is Nothing AndAlso oPLWCFFees.Count > 0 Then
                        For Each f In oPLWCFFees
                            Dim nFee As New clsPickListFeeObject85
                            With nFee
                                .PLControl = hdrPLControl
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
                            oPLExportFee.Add(nFee)
                        Next
                    End If
                    Try
                        If Me.AutoConfirmation Then
                            If Me.UpdateStatus(oPL.PLControl, Now, True) Then
                                Log("Pick List Auto Confirmation Complete For PL Control " & oPL.PLControl.ToString & ".")
                            End If
                        Else
                            Me.UpdateStatus(oPL.PLControl, Now, False)
                        End If
                    Catch ex As System.ApplicationException
                        'Log the exception and move on to the next record
                        LogException("Pick List Export Update Status Error (duplicate export possible)", "Could not update the export status for pick list control number " & oPL.PLControl.ToString & ".", AdminEmail, ex, "NGL.FreightMaster.Integration.clsPickList.readObjectData70 Failure")
                    Catch ex As Exception
                        'this is an unexpected error so re throw it
                        Throw
                    End Try

                    intCt += 1
                    If intCt > oPLWCFRets.Count Then Exit For
                Next

            End If
            oPickListDetails = oPLExportDet.ToArray()
            oPickListFees = oPLExportFee.ToArray()

            'Catch ex As DAL.FaultException(Of SqlFaultInfo)'
        Catch ex As Exception
            oRet = ProcessDataReturnValues.nglDataIntegrationFailure
            LogException("PickList Read Object Data 85 Failure", "Could not read Pick List records.", AdminEmail, ex, "NGL.FreightMaster.Integeration.clsPickList.readObjectData85 Failure")
        Finally
            Try
                closeConnection()
            Catch ex As Exception

            End Try
        End Try
        Return oRet
    End Function


#End Region
End Class
