Imports System.Data.SqlClient
Imports System.ServiceModel
Imports Ngl.Core.Utility

Public Class NGLAPMassEntryData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        'Dim db As New NGLMasBookDataContext(ConnectionString)
        'Me.LinqTable = db.APMassEntries
        'Me.LinqDB = db
        Me.SourceClass = "NGLAPMassEntryData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMasBookDataContext(ConnectionString)
                _LinqTable = db.APMassEntries
                _LinqDB = db
            End If
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return GetAPMassEntryFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        'this function cannot be used to return records
        Return Nothing
    End Function

    ''' <summary>
    ''' GetAPMassEntryFiltered
    ''' </summary>
    ''' <param name="Control"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified By LVV 3/24/20 v-8.2.1.006
    '''  Added fields APReduction, APReductionReason, and APReductionAdjustedCost
    '''  Also added APShipCarrierProNumber because I noticed it was missing
    ''' </remarks>
    Public Function GetAPMassEntryFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.APMassEntry
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'db.Log = New DebugTextWriter
                Dim oSecureComp = From s In db.UserAdminAsNvarcharRefBooks Where s.UserAdminUserName = Me.Parameters.UserName Select s.UserAdminCompControl
                'Get the newest record that matches the provided criteria
                Dim APMassEntry As DataTransferObjects.APMassEntry = (
                        From d In db.APMassEntries
                        Where
                        (Control = 0 Or d.APControl = Control) _
                        And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.APCustomerID))
                        Order By d.APControl Descending
                        Select New DataTransferObjects.APMassEntry With {.APControl = d.APControl _
                        , .APCarrierNumber = If(d.APCarrierNumber.HasValue, d.APCarrierNumber, 0) _
                        , .APBillNumber = d.APBillNumber _
                        , .APBillDate = d.APBillDate _
                        , .APPONumber = d.APPONumber _
                        , .APCustomerID = d.APCustomerID _
                        , .APCostCenterNumber = d.APCostCenterNumber _
                        , .APTotalCost = d.APTotalCost _
                        , .APPRONumber = d.APPRONumber _
                        , .APBLNumber = d.APBLNumber _
                        , .APBilledWeight = If(d.APBilledWeight.HasValue, d.APBilledWeight, 0) _
                        , .APCNSNumber = d.APCNSNumber _
                        , .APReceivedDate = d.APReceivedDate _
                        , .APPayCode = d.APPayCode _
                        , .APElectronicFlag = d.APElectronicFlag _
                        , .APApprovedFlag = d.APApprovedFlag _
                        , .APMessage = d.APMessage _
                        , .APTotalTax = If(d.APTotalTax.HasValue, d.APTotalTax, 0) _
                        , .APFee1 = If(d.APFee1.HasValue, d.APFee1, 0) _
                        , .APFee2 = If(d.APFee2.HasValue, d.APFee2, 0) _
                        , .APFee3 = If(d.APFee3.HasValue, d.APFee3, 0) _
                        , .APFee4 = If(d.APFee4.HasValue, d.APFee4, 0) _
                        , .APFee5 = If(d.APFee5.HasValue, d.APFee5, 0) _
                        , .APFee6 = If(d.APFee6.HasValue, d.APFee6, 0) _
                        , .APOtherCosts = If(d.APOtherCosts.HasValue, d.APOtherCosts, 0) _
                        , .APCarrierCost = If(d.APCarrierCost.HasValue, d.APCarrierCost, 0) _
                        , .APExportFlag = If(d.APExportFlag.HasValue, d.APExportFlag, False) _
                        , .APOrderSequence = d.APOrderSequence _
                        , .APTaxDetail1 = d.APTaxDetail1 _
                        , .APTaxDetail2 = d.APTaxDetail2 _
                        , .APTaxDetail3 = d.APTaxDetail3 _
                        , .APTaxDetail4 = d.APTaxDetail4 _
                        , .APTaxDetail5 = d.APTaxDetail5 _
                        , .APSHID = d.APSHID _
                        , .APShipCarrierProNumber = d.APShipCarrierProNumber _
                        , .APReduction = d.APReduction _
                        , .APReductionReason = d.APReductionReason _
                        , .APReductionAdjustedCost = d.APReductionAdjustedCost _
                        , .APModUser = d.APModUser _
                        , .APModDate = d.APModDate _
                        , .APUpdated = d.APUpdated.ToArray()}).First
                Return APMassEntry
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' GetAPMassEntrysFiltered
    ''' </summary>
    ''' <param name="APCarrierNumber"></param>
    ''' <param name="APReceivedDateFrom"></param>
    ''' <param name="APReceivedDateTo"></param>
    ''' <param name="APApprovedFlag"></param>
    ''' <param name="APElectronicFlag"></param>
    ''' <param name="ShowMatched"></param>
    ''' <param name="ShowAllErrors"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified By LVV 3/24/20 v-8.2.1.006
    '''  Added fields APReduction, APReductionReason, and APReductionAdjustedCost
    '''  Also added APShipCarrierProNumber because I noticed it was missing
    ''' </remarks>
    Public Function GetAPMassEntrysFiltered(ByVal APCarrierNumber As Integer,
                                            ByVal APReceivedDateFrom As Date,
                                            ByVal APReceivedDateTo As Date,
                                            ByVal APApprovedFlag As Boolean,
                                            ByVal APElectronicFlag As Boolean,
                                            ByVal ShowMatched As Boolean,
                                            ByVal ShowAllErrors As Boolean) As DataTransferObjects.APMassEntry()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'the show all errors flag overrides other the APApprovedFlag
                If ShowAllErrors = True Then APApprovedFlag = False
                Dim dtFrom As Date = DataTransformation.formatStartDateFilter(APReceivedDateFrom)
                Dim dtTo As Date = DataTransformation.formatEndDateFilter(APReceivedDateTo)

                Dim blnIgnoreElectronicFlag = False
                If ShowAllErrors = True Or ShowMatched = True Or APApprovedFlag = True Then blnIgnoreElectronicFlag = True

                'If APElectronicFlag = True Or (ShowMatched = False And APApprovedFlag = False) Then blnIgnoreElectronicFlag = False
                'db.Log = New DebugTextWriter
                Dim oSecureComp = From s In db.UserAdminAsNvarcharRefBooks Where s.UserAdminUserName = Me.Parameters.UserName Select s.UserAdminCompControl
                'Return all the contacts that match the criteria sorted by name
                Dim APMassEntrys() As DataTransferObjects.APMassEntry = (
                        From d In db.APMassEntries
                        Where
                        (APCarrierNumber = 0 Or d.APCarrierNumber = APCarrierNumber) _
                        And
                        (d.APReceivedDate >= dtFrom And d.APReceivedDate <= dtTo) _
                        And
                        (d.APApprovedFlag = APApprovedFlag) _
                        And
                        (blnIgnoreElectronicFlag = True OrElse d.APElectronicFlag = APElectronicFlag) _
                        And
                        (ShowMatched = False OrElse d.APPayCode = "M") _
                        And
                        (ShowAllErrors = False OrElse d.APMessage.Length > 1) _
                        And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.APCustomerID))
                        Order By d.APCarrierNumber
                        Select New DataTransferObjects.APMassEntry With {.APControl = d.APControl _
                        , .APCarrierNumber = If(d.APCarrierNumber.HasValue, d.APCarrierNumber, 0) _
                        , .APBillNumber = d.APBillNumber _
                        , .APBillDate = d.APBillDate _
                        , .APPONumber = d.APPONumber _
                        , .APCustomerID = d.APCustomerID _
                        , .APCostCenterNumber = d.APCostCenterNumber _
                        , .APTotalCost = d.APTotalCost _
                        , .APPRONumber = d.APPRONumber _
                        , .APBLNumber = d.APBLNumber _
                        , .APBilledWeight = If(d.APBilledWeight.HasValue, d.APBilledWeight, 0) _
                        , .APCNSNumber = d.APCNSNumber _
                        , .APReceivedDate = d.APReceivedDate _
                        , .APPayCode = d.APPayCode _
                        , .APElectronicFlag = d.APElectronicFlag _
                        , .APApprovedFlag = d.APApprovedFlag _
                        , .APMessage = d.APMessage _
                        , .APTotalTax = If(d.APTotalTax.HasValue, d.APTotalTax, 0) _
                        , .APFee1 = If(d.APFee1.HasValue, d.APFee1, 0) _
                        , .APFee2 = If(d.APFee2.HasValue, d.APFee2, 0) _
                        , .APFee3 = If(d.APFee3.HasValue, d.APFee3, 0) _
                        , .APFee4 = If(d.APFee4.HasValue, d.APFee4, 0) _
                        , .APFee5 = If(d.APFee5.HasValue, d.APFee5, 0) _
                        , .APFee6 = If(d.APFee6.HasValue, d.APFee6, 0) _
                        , .APOtherCosts = If(d.APOtherCosts.HasValue, d.APOtherCosts, 0) _
                        , .APCarrierCost = If(d.APCarrierCost.HasValue, d.APCarrierCost, 0) _
                        , .APExportFlag = If(d.APExportFlag.HasValue, d.APExportFlag, False) _
                        , .APOrderSequence = d.APOrderSequence _
                        , .APTaxDetail1 = d.APTaxDetail1 _
                        , .APTaxDetail2 = d.APTaxDetail2 _
                        , .APTaxDetail3 = d.APTaxDetail3 _
                        , .APTaxDetail4 = d.APTaxDetail4 _
                        , .APTaxDetail5 = d.APTaxDetail5 _
                        , .APSHID = d.APSHID _
                        , .APShipCarrierProNumber = d.APShipCarrierProNumber _
                        , .APReduction = d.APReduction _
                        , .APReductionReason = d.APReductionReason _
                        , .APReductionAdjustedCost = d.APReductionAdjustedCost _
                        , .APModUser = d.APModUser _
                        , .APModDate = d.APModDate _
                        , .APUpdated = d.APUpdated.ToArray()}).ToArray()
                Return APMassEntrys
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
            Return Nothing
        End Using
    End Function

    Public Function GetAPExportData70(ByVal MaxRetryNbr As Integer, ByVal RetryMinutes As Integer, ByVal CompLegalEntity As String, ByVal MaxRowsReturned As Integer) As List(Of LTS.spGetAPExportRecords70Result)
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oReturnData = (From d In db.spGetAPExportRecords70(MaxRetryNbr, RetryMinutes, CompLegalEntity, MaxRowsReturned) Select d).ToList()
                Return oReturnData
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAPExportData70"), db)
            End Try
        End Using
        Return Nothing
    End Function

    Public Function GetAPExportTestData70(ByVal CompLegalEntity As String, ByVal RowsReturned As Integer) As List(Of LTS.spGetAPExportTestRecords70Result)
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oReturnData = (From d In db.spGetAPExportTestRecords70(CompLegalEntity, RowsReturned) Select d).ToList()
                Return oReturnData
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAPExportTestData70"), db)
            End Try
        End Using
        Return Nothing
    End Function

    Public Function GetPickListData70(ByVal MaxRetryNbr As Integer, ByVal RetryMinutes As Integer, ByVal CompLegalEntity As String, ByVal MaxRowsReturned As Integer) As List(Of LTS.spGetPickListRecords70Result)
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oReturnData = (From d In db.spGetPickListRecords70(MaxRetryNbr, RetryMinutes, CompLegalEntity, MaxRowsReturned) Select d).ToList()
                Return oReturnData
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetPickListData70"), db)
            End Try
        End Using
        Return Nothing
    End Function

    Public Function GetPickListTestData70(ByVal CompLegalEntity As String, ByVal RowsReturned As Integer) As List(Of LTS.spGetPickListTestRecords70Result)
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oReturnData = (From d In db.spGetPickListTestRecords70(CompLegalEntity, RowsReturned) Select d).ToList()
                Return oReturnData
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetPickListTestData70"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Returns the item details need for Pick List Status Update Web Services
    ''' We now call spGetExportPickDetailRows70 instead of spGetExportDetailRows70
    ''' because Pick Data needs Contracted Cost and AP Data needs Billed Cost
    ''' AP export procedures should still use GetExportDetailRows70
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR v-7.0.5.100
    ''' Note: a duplicate instance of this method is available in thw NGLtblPickListData library
    ''' </remarks>
    Public Function GetExportPickDetailRows70(ByVal BookControl As Integer) As List(Of LTS.spGetExportPickDetailRows70Result)
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oReturnData = (From d In db.spGetExportPickDetailRows70(BookControl) Select d).ToList()
                Return oReturnData
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetExportPickDetailRows70"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Book Item Details for the Pick Worksheet Export web service 
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR v-8.2.0.117 7/17/2019
    '''   replaces the 70 version Of the data
    '''   includes BookItemOrderNumber
    ''' </remarks>
    Public Function GetExportPickDetailRows80(ByVal BookControl As Integer) As List(Of LTS.spGetExportPickDetailRows80Result)
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oReturnData = (From d In db.spGetExportPickDetailRows80(BookControl) Select d).ToList()
                Return oReturnData
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetExportPickDetailRows80"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Book Item Details for the Pick Worksheet Export web service 
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR v-8.5.1.001 3/21/2022
    '''     replaces the 80 version Of the data
    '''     added New logic for LineHaulCost, FuelCost, And FeesCost
    ''' </remarks>
    Public Function GetExportPickDetailRows85(ByVal BookControl As Integer) As List(Of LTS.spGetExportPickDetailRows85Result)
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oReturnData = (From d In db.spGetExportPickDetailRows85(BookControl) Select d).ToList()
                Return oReturnData
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetExportPickDetailRows85"), db)
            End Try
        End Using
        Return Nothing
    End Function



    Public Function GetExportDetailRows70(ByVal BookControl As Integer) As List(Of LTS.spGetExportDetailRows70Result)
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oReturnData = (From d In db.spGetExportDetailRows70(BookControl) Select d).ToList()
                Return oReturnData
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetExportDetailRows70"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Book Item Details for the AP Export web service 
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR v-8.2.0.117 7/17/2019
    '''   replaces the 70 version Of the data
    '''   includes BookItemOrderNumber
    ''' </remarks>
    Public Function GetExportDetailRows80(ByVal BookControl As Integer) As List(Of LTS.spGetExportDetailRows80Result)
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oReturnData = (From d In db.spGetExportDetailRows80(BookControl) Select d).ToList()
                Return oReturnData
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetExportDetailRows80"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Book Item Details for the AP Export web service 
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR v-8.5.1.001 3/21/2022
    '''     replaces the 80 version Of the data
    '''     added New logic for LineHaulCost, FuelCost, And FeesCost
    ''' </remarks>
    Public Function GetExportDetailRows85(ByVal BookControl As Integer) As List(Of LTS.spGetExportDetailRows85Result)
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oReturnData = (From d In db.spGetExportDetailRows85(BookControl) Select d).ToList()
                Return oReturnData
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetExportDetailRows85"), db)
            End Try
        End Using
        Return Nothing
    End Function

    Public Function GetExportFeeRows70(BookControl As Integer) As List(Of LTS.spGetExportFeeRows70Result)
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oReturnData = (From d In db.spGetExportFeeRows70(BookControl) Select d).ToList()
                Return oReturnData
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetExportFeeRows70"), db)
            End Try
        End Using
        Return Nothing
    End Function

    Public Function GetAPExportRecordsAggregated(ByVal MaxRetryNbr As Integer, ByVal RetryMinutes As Integer, ByVal CompLegalEntity As String, ByVal MaxRowsReturned As Integer) As List(Of LTS.spGetAPExportRecordsAggregatedResult)
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oReturnData = (From d In db.spGetAPExportRecordsAggregated(MaxRetryNbr, RetryMinutes, CompLegalEntity, MaxRowsReturned) Select d).ToList()
                Return oReturnData
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAPExportRecordsAggregated"), db)
            End Try
        End Using
        Return Nothing
    End Function

    Public Function UpdateStatus(ByVal BookSHID As String,
                                 ByVal APBillNumber As String,
                                 Optional ByVal APExportFlag As Boolean = True,
                                 Optional ByVal APExportDate As Nullable(Of Date) = Nothing,
                                 Optional ByVal APExportRetry As Nullable(Of Integer) = Nothing) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim intRet = db.spUpdateAPExportStatusBySHIDAndBillNo(BookSHID, APBillNumber, APExportFlag, APExportRetry, APExportDate, Me.Parameters.UserName)
                blnRet = True 'if there are no errors we just return true
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateStatus"), db)
            End Try
        End Using
        Return blnRet
    End Function

#Region "TMS 365"

    ''' <summary>
    ''' Inserts or Updates the current Freight Bill Invoice informaiton provided;
    ''' When PendingFeeApprovalFlag Is true the system looks up the APActCost and APCarrierCost from the existing data; 
    ''' the APActCost is recalculated based on changes to the approced accessorial fees
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="APBillNumber"></param>
    ''' <param name="APTotalCost"></param>
    ''' <param name="APCarrierCost"></param>
    ''' <param name="CarrierControl"></param>
    ''' <param name="APBilledWeight"></param>
    ''' <param name="APBLNumber"></param>
    ''' <param name="PendingFeeApprovalFlag"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.0 on 01/12/2018
    ''' add new parameter mapping to stored procedure
    ''' @BookControl INT
    ''' @APBillNumber NVARCHAR (50)
    ''' @APActCost MONEY -- Total Billed Cost (Line Haul + Fees)  When @PendingFeeApprovalFlag Is true we need to lookup from AP Mass Entry Table
    ''' @APCarrierCost MONEY  --Line Haul -- Modified by RHR for v-8.0.0 on 01/09/2018
    ''' @CarrierControl INT
    ''' @APBilledWeight int
    ''' @APBLNumber nvarchar(20)
    ''' @PendingFeeApprovalFlag bit, -- Modified by RHR for v-8.0.0 on 01/09/2018
    ''' 
    ''' Modified by LVV on 02/26/2018 for v-8.1 PQ EDI
    ''' Added parameter ElectronicFlag because even though the sp has 'Web' 
    ''' in the name we are calling this sp from both WebTender And EDI
    ''' Modified by RHR for v-8.2.0.117 on 8/19/19
    '''   added billed date and received date and new reference to stored procedure
    '''   this function is only used to insert or update the APMassEntry table
    ''' </remarks>
    Public Function InsertFreightBillWeb365(ByVal BookControl As Integer,
                                            ByVal APBillNumber As String,
                                            ByVal APBillDate As Date?,
                                            ByVal APReceivedDate As Date?,
                                            ByVal APTotalCost As Decimal,
                                            ByVal APCarrierCost As Decimal,
                                            ByVal CarrierControl As Integer,
                                            ByVal APBilledWeight As Integer,
                                            ByVal APBLNumber As String,
                                            ByVal ElectronicFlag As Boolean?,
                                            Optional ByVal PendingFeeApprovalFlag As Boolean = False) As LTS.spInsertFreightBillWeb365Result
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Return db.spInsertFreightBillWeb365(BookControl, APBillNumber, APBillDate, APReceivedDate, APTotalCost, APCarrierCost, CarrierControl, APBilledWeight, APBLNumber, PendingFeeApprovalFlag, ElectronicFlag, Parameters.UserName).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertFreightBillWeb365"), db)
            End Try
        End Using
        Return Nothing
    End Function

    Public Function GetAPFreightBillsTotAuditByCarrierAndDate(ByVal CarrierNumber As Integer, ByVal RecDateFrom As Date, ByVal RecDateTo As Date, ByVal Electronic As Boolean) As LTS.spGetAPFreightBillsTotAuditByCarrierAndDateResult()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Return db.spGetAPFreightBillsTotAuditByCarrierAndDate(CarrierNumber, RecDateFrom, RecDateTo, Electronic, Me.Parameters.UserName).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAPFreightBillsTotAuditByCarrierAndDate"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' returns the line haul (carrier cost) if it exits in the AP mass entry table for the provided SHID number
    ''' </summary>
    ''' <param name="sBookSHID"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' created by RHR on 05/23/2018 for v-8.1.1   
    ''' Modified by RHR for v-8.2.0.117 on 08/21/19
    '''     we now call spGetAPCarrierCostBySHID
    '''     If a freight bill does not exist we look up the line haul 
    '''     from the booking records 
    '''     Also the new logic can get the ap data by any key (like order number)
    '''     to support legacy freight bills that are not forced to use the SHID
    '''     as the key
    '''     The optional parameter APControl can be use to identify the freight bill being used if available
    ''' </remarks>
    Public Function GetAPCarrierCost(ByVal sBookSHID As String, Optional ByRef APControl As Integer = 0) As Decimal
        'select APCarrierCost, * from dbo.APMassEntry where APMassEntry.APBillNumber = 'FB-1-1192'
        Dim oRet As Decimal? = 0
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oRetData = db.spGetAPCarrierCostBySHID(sBookSHID).FirstOrDefault()
                If Not oRetData Is Nothing Then
                    oRet = oRetData.CarrierCost
                    APControl = If(oRetData.APControl, 0)
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAPCarrierCost"), db)
            End Try
        End Using
        Return If(oRet.HasValue, oRet.Value, 0)
    End Function

    Public Function GetBookFinAPActWgtBySHID(ByVal sBookSHID As String) As Double
        'select APCarrierCost, * from dbo.APMassEntry where APMassEntry.APBillNumber = 'FB-1-1192'
        Dim dRet As Double = 0
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim iBilledWgt = db.Books.Where(Function(x) x.BookSHID = sBookSHID).Sum(Function(s) s.BookFinAPActWgt)
                If If(iBilledWgt, 0) < 1 Then
                    Dim dBilledWgt = db.Books.Where(Function(x) x.BookSHID = sBookSHID).Sum(Function(s) s.BookTotalWgt)
                    dRet = CDbl(If(dBilledWgt, 0))
                Else
                    dRet = CDbl(If(iBilledWgt, 0))
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAPBilledWeightBySHID"), db)
            End Try
        End Using
        Return dRet
    End Function

    Public Function GetSettlementBookTotalMilesBySHID(ByVal sBookSHID As String) As Double
        Dim dRet As Double = 0
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                dRet = If(db.Books.Where(Function(x) x.BookSHID = sBookSHID).Sum(Function(s) s.BookMilesFrom), 0)

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetSettlementBookTotalMilesBySHID"), db)
            End Try
        End Using
        Return dRet
    End Function

    ''' <summary>
    ''' Get total fuel cost for SHID
    ''' </summary>
    ''' <param name="BookSHID"></param>
    ''' <param name="ReturnVisibleFeesOnly"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 6/10/2019
    ''' Get all Fuel Fees and Pending Fuel Fees for the SHID.  
    ''' Fuel Is identified by AccessorialCodes 15,9, And 2	
    ''' We do Not include zero costs Fuel fees
    ''' </remarks>
    Public Function GetSettlementFuelForSHID(ByVal BookSHID As String, Optional ByVal ReturnVisibleFeesOnly As Boolean = False) As Decimal
        Dim decRet As Decimal = 0
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                If String.IsNullOrWhiteSpace(BookSHID) Then Return decRet
                Dim spRes = db.spGetSettlementFuelForSHID(BookSHID, ReturnVisibleFeesOnly).FirstOrDefault()
                If Not spRes Is Nothing AndAlso spRes.TotalFuel.HasValue Then decRet = spRes.TotalFuel.Value
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetSettlementFuelForSHID"), db)
            End Try
        End Using
        Return decRet
    End Function

    ''' <summary>
    ''' NOT CURRENTLY USED
    ''' This is the one for when we use the grid AllFilters for all the filtering
    ''' We are using a different version of this method but I left it in case we want to switch later, or else just as an example
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns></returns>
    Public Function GetAPMassEntrysFiltered365(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.APMassEntry()
        If filters Is Nothing Then Return Nothing
        Dim filterWhere As String = ""
        Dim sFilterSpacer As String = ""
        Dim oRet() As LTS.APMassEntry
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Have to deal with the bit filters
                If filters.FilterValues.Any(Function(x) x.filterName = "APElectronicFlag") Then
                    Dim sActiveFilter = filters.FilterValues.Where(Function(x) x.filterName = "APElectronicFlag").FirstOrDefault()
                    If (Not sActiveFilter Is Nothing) Then
                        If Not String.IsNullOrEmpty(sActiveFilter.filterValueFrom) AndAlso (sActiveFilter.filterValueFrom.ToUpper().Contains("FALSE") Or sActiveFilter.filterValueFrom.ToUpper().Contains("0")) Then
                            filterWhere &= sFilterSpacer & " ( APElectronicFlag = false  )  "
                            sFilterSpacer = " And "
                        ElseIf Not String.IsNullOrEmpty(sActiveFilter.filterValueTo) AndAlso (sActiveFilter.filterValueTo.ToUpper().Contains("FALSE") Or sActiveFilter.filterValueTo.ToUpper().Contains("0")) Then
                            filterWhere &= sFilterSpacer & " ( APElectronicFlag = false  )  "
                            sFilterSpacer = " And "
                        Else
                            filterWhere &= sFilterSpacer & " ( APElectronicFlag = true  )  "
                            sFilterSpacer = " And "
                        End If
                    End If
                End If
                If filters.FilterValues.Any(Function(x) x.filterName = "APApprovedFlag") Then
                    Dim sActiveFilter = filters.FilterValues.Where(Function(x) x.filterName = "APApprovedFlag").FirstOrDefault()
                    If (Not sActiveFilter Is Nothing) Then
                        If Not String.IsNullOrEmpty(sActiveFilter.filterValueFrom) AndAlso (sActiveFilter.filterValueFrom.ToUpper().Contains("FALSE") Or sActiveFilter.filterValueFrom.ToUpper().Contains("0")) Then
                            filterWhere &= sFilterSpacer & " ( APApprovedFlag = false  )  "
                            sFilterSpacer = " And "
                        ElseIf Not String.IsNullOrEmpty(sActiveFilter.filterValueTo) AndAlso (sActiveFilter.filterValueTo.ToUpper().Contains("FALSE") Or sActiveFilter.filterValueTo.ToUpper().Contains("0")) Then
                            filterWhere &= sFilterSpacer & " ( APApprovedFlag = false  )  "
                            sFilterSpacer = " And "
                        Else
                            filterWhere &= sFilterSpacer & " ( APApprovedFlag = true  )  "
                            sFilterSpacer = " And "
                        End If
                    End If
                End If
                If filters.FilterValues.Any(Function(x) x.filterName = "APExportFlag") Then
                    Dim sActiveFilter = filters.FilterValues.Where(Function(x) x.filterName = "APExportFlag").FirstOrDefault()
                    If (Not sActiveFilter Is Nothing) Then
                        If Not String.IsNullOrEmpty(sActiveFilter.filterValueFrom) AndAlso (sActiveFilter.filterValueFrom.ToUpper().Contains("FALSE") Or sActiveFilter.filterValueFrom.ToUpper().Contains("0")) Then
                            filterWhere &= sFilterSpacer & " ( APExportFlag = false  )  "
                            sFilterSpacer = " And "
                        ElseIf Not String.IsNullOrEmpty(sActiveFilter.filterValueTo) AndAlso (sActiveFilter.filterValueTo.ToUpper().Contains("FALSE") Or sActiveFilter.filterValueTo.ToUpper().Contains("0")) Then
                            filterWhere &= sFilterSpacer & " ( APExportFlag = false  )  "
                            sFilterSpacer = " And "
                        Else
                            filterWhere &= sFilterSpacer & " ( APExportFlag = true  )  "
                            sFilterSpacer = " And "
                        End If
                    End If
                End If
                Dim oSecureComp = From s In db.UserAdminAsNvarcharRefBooks Where s.UserAdminUserName = Me.Parameters.UserName Select s.UserAdminCompControl
                Dim iQuery As IQueryable(Of LTS.APMassEntry)
                iQuery = (From d In db.APMassEntries
                    Where (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse d.APCustomerID = String.Empty OrElse d.APCustomerID = "0" OrElse oSecureComp.Contains(d.APCustomerID))
                    Order By d.APCarrierNumber
                    Select d)
                db.Log = New DebugTextWriter
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAPMassEntrysFiltered365"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' This is the one where we use filters on the page that are not from the grid. Custom page level filters
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <param name="APCarrierNumber"></param>
    ''' <param name="APReceivedDateFrom"></param>
    ''' <param name="APReceivedDateTo"></param>
    ''' <param name="APApprovedFlag"></param>
    ''' <param name="APElectronicFlag"></param>
    ''' <param name="ShowMatched"></param>
    ''' <param name="ShowAllErrors"></param>
    ''' <returns></returns>
    Public Function GetAPMassEntrysFiltered365(ByRef RecordCount As Integer,
                                               ByVal filters As Models.AllFilters,
                                               ByVal APCarrierNumber As Integer,
                                               ByVal APReceivedDateFrom As Date,
                                               ByVal APReceivedDateTo As Date,
                                               ByVal APApprovedFlag As Boolean,
                                               ByVal APElectronicFlag As Boolean,
                                               ByVal ShowMatched As Boolean,
                                               ByVal ShowAllErrors As Boolean,
                                               ByVal ShowPA As Boolean) As LTS.APMassEntry()
        If filters Is Nothing Then Return Nothing
        Dim filterWhere As String = ""
        Dim oRet() As LTS.APMassEntry
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                If ShowAllErrors = True Then APApprovedFlag = False 'the show all errors flag overrides other the APApprovedFlag
                Dim dtFrom As Date = DataTransformation.formatStartDateFilter(APReceivedDateFrom)
                Dim dtTo As Date = DataTransformation.formatEndDateFilter(APReceivedDateTo)
                Dim blnIgnoreElectronicFlag = False
                If ShowAllErrors = True Or ShowMatched = True Or APApprovedFlag = True Or ShowPA = True Then blnIgnoreElectronicFlag = True
                'If APElectronicFlag = True Or (ShowMatched = False And APApprovedFlag = False) Then blnIgnoreElectronicFlag = False
                Dim oSecureComp = From s In db.UserAdminAsNvarcharRefBooks Where s.UserAdminUserName = Me.Parameters.UserName Select s.UserAdminCompControl
                Dim iQuery As IQueryable(Of LTS.APMassEntry)
                iQuery = (From d In db.APMassEntries
                    Where
                        (APCarrierNumber = 0 Or d.APCarrierNumber = APCarrierNumber) _
                        And
                        (d.APReceivedDate >= dtFrom And d.APReceivedDate <= dtTo) _
                        And
                        (d.APApprovedFlag = APApprovedFlag) _
                        And
                        (blnIgnoreElectronicFlag = True OrElse d.APElectronicFlag = APElectronicFlag) _
                        And
                        (ShowMatched = False OrElse d.APPayCode = "M") _
                        And
                        (ShowAllErrors = False OrElse d.APMessage.Length > 1) _
                        And
                        (ShowPA = False OrElse d.APPayCode = "PA") _
                        And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse d.APCustomerID = String.Empty OrElse d.APCustomerID = "0" OrElse oSecureComp.Contains(d.APCustomerID))
                    Order By d.APCarrierNumber
                    Select d)
                db.Log = New DebugTextWriter
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAPMassEntrysFiltered365"), db)
            End Try
        End Using
        Return Nothing
    End Function

    Public Function GetAPMassEntry(ByVal iAPControl As Integer) As LTS.APMassEntry
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                If iAPControl <> 0 Then
                    Return db.APMassEntries.Where(Function(x) x.APControl = iAPControl).FirstOrDefault()
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAPMassEntry"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Inserts Or Updates a record in the APMassEntry table
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified By LVV on 4/6/20 for v-8.2.1.006
    '''  Added fields APReduction, APReductionAdjustedCost, and APReductionReason to the table
    '''  Added formula to calculate APReductionAdjustedCost using on APTotalCost and APReduction
    ''' </remarks>
    Public Function InsertOrUpdateAPMassEntry(ByVal oData As LTS.APMassEntry) As Boolean
        Dim blnRet As Boolean = False
        If oData Is Nothing Then Return False 'nothing to do
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                If oData.APReduction <> 0 Then
                    oData.APReductionAdjustedCost = oData.APTotalCost - oData.APReduction 'Modified By LVV on 4/6/20 for v-8.2.1.006
                Else
                    oData.APReductionAdjustedCost = 0
                End If
                oData.APModUser = Parameters.UserName
                oData.APModDate = Date.Now
                If oData.APControl = 0 Then
                    db.APMassEntries.InsertOnSubmit(oData)
                Else
                    db.APMassEntries.Attach(oData, True)
                End If
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertOrUpdateAPMassEntry"), db)
            End Try
        End Using
        Return blnRet
    End Function


    Public Function DeleteAPMassEntry365(ByVal iAPControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        If iAPControl = 0 Then Return False 'nothing to do
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oExisting = db.APMassEntries.Where(Function(x) x.APControl = iAPControl AndAlso x.APPayCode.ToUpper() = "N").FirstOrDefault()
                If oExisting Is Nothing OrElse oExisting.APControl = 0 Then Return True
                db.APMassEntries.DeleteOnSubmit(oExisting)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteAPMassEntry365"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Returns zero if the freight bill does not exist or the ap control number if the freight bill is found
    ''' </summary>
    ''' <param name="sAPBillNumber"></param>
    ''' <param name="iCarrierControl"></param>
    ''' <returns></returns>
    Public Function DoesFreightBillExist(ByVal sAPBillNumber As String, ByVal iCarrierControl As Integer) As Integer
        Dim iAPControl As Integer = 0
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim iRet? As Integer = db.udfDoesFreightBillExist(sAPBillNumber, iCarrierControl)
                If iRet.HasValue Then iAPControl = iRet.Value
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DoesFreightBillExist"), db)
            End Try
        End Using
        Return iAPControl
    End Function


    ''' <summary>
    ''' Audit the Freight Bills using D365 procedures
    ''' </summary>
    ''' <param name="APControl"></param>
    ''' <param name="BookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.3.0.003 on 02/18/2021
    '''     spUpdateBookDependencies must be called for each booking to 
    '''     ensure that the BookItemActFreightCost is properly allocated 
    ''' </remarks>
    Public Function AuditFreightBill365(ByVal APControl As Integer, Optional ByVal BookControl As Integer = 0) As LTS.spAuditFreightBill365Result()
        Dim oReturnData As LTS.spAuditFreightBill365Result()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                oReturnData = db.spAuditFreightBill365(APControl, Parameters.UserName).ToArray()
                Try
                    If (Not oReturnData Is Nothing) AndAlso (oReturnData.Count() > 0) AndAlso (Not String.IsNullOrWhiteSpace(oReturnData(0).APBillNumber)) Then
                        Dim lBookControls As List(Of Integer) = db.Books.Where(Function(x) x.BookFinAPBillNumber = oReturnData(0).APBillNumber).Select(Function(b) b.BookControl).ToList()
                        If Not lBookControls Is Nothing AndAlso lBookControls.Count() > 0 Then
                            ' Modified by RHR for v-8.3.0.003 on 02/18/2021
                            ' spUpdateBookDependencies must be called for each booking to 
                            ' ensure that the BookItemActFreightCost is properly allocated 
                            For Each ditem In lBookControls
                                db.spUpdateBookDependencies(0, ditem, Parameters.UserName)
                            Next
                        Else
                            If BookControl > 0 Then
                                db.spUpdateBookDependencies(0, BookControl, Parameters.UserName)
                            End If
                        End If
                    Else
                        If BookControl > 0 Then
                            db.spUpdateBookDependencies(0, BookControl, Parameters.UserName)
                        End If
                    End If

                    'Removed by RHR for v-8.3.0.003 on 02/18/2021
                    'If BookControl = 0 AndAlso (Not oReturnData Is Nothing) AndAlso (oReturnData.Count() > 0) AndAlso (Not String.IsNullOrWhiteSpace(oReturnData(0).APBillNumber)) Then
                    '    BookControl = db.Books.Where(Function(x) x.BookFinAPBillNumber = oReturnData(0).APBillNumber).Select(Function(b) b.BookControl).FirstOrDefault()
                    'End If
                    'If BookControl > 0 Then
                    '    db.spUpdateBookDependencies(0, BookControl, Parameters.UserName)
                    'End If
                Catch ex As Exception
                    'do nothing
                End Try
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("AuditFreightBill365"), db)
            End Try
        End Using
        Return oReturnData
    End Function

    Public Function GetvAPMESummaryFields(ByVal iAPControl As Integer) As LTS.vAPMESummaryField
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                If iAPControl <> 0 Then
                    Return db.vAPMESummaryFields.Where(Function(x) x.APControl = iAPControl).FirstOrDefault()
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetvAPMESummaryFields"), db)
            End Try
        End Using
        Return Nothing
    End Function


    ''' <summary>
    ''' updates the the zero carrier cost in the AP Mass Entry table by subtracting the fees from the total
    ''' typically used when carrier cost is not probided by the carrier or by users in AP Mass Entry
    ''' </summary>
    ''' <param name="iAPControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2.1.004 on 01/10/2020
    ''' </remarks>
    Public Function UpdateZeroCarrierCostUsingTotalFees(ByVal iAPControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                If iAPControl <> 0 Then
                    Dim oAPData = db.APMassEntries.Where(Function(x) x.APControl = iAPControl).FirstOrDefault()
                    If Not oAPData Is Nothing AndAlso oAPData.APControl = iAPControl AndAlso oAPData.APCarrierCost = 0 Then
                        Dim dTotalFees As Decimal? = 0
                        If db.APMassEntryFees.Any(Function(x) x.APMFeesAPControl = iAPControl) Then
                            dTotalFees = db.APMassEntryFees.Where(Function(x) x.APMFeesAPControl = iAPControl).Sum(Function(x) x.APMFeesValue)
                        End If
                        If Not dTotalFees.HasValue Then dTotalFees = 0
                        oAPData.APCarrierCost = oAPData.APTotalCost - dTotalFees
                        db.SubmitChanges()
                    End If
                End If
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateZeroCarrierCostUsingTotalFees"), db)
            End Try
        End Using
        Return blnRet
    End Function



#End Region


#End Region

#Region "Protected Functions"

    ''' <summary>
    ''' CopyDTOToLinq APMassEntry
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns>LTS.APMassEntry</returns>
    ''' <remarks>
    ''' Modified By LVV 3/24/20 v-8.2.1.006
    '''  Added fields APReduction, APReductionReason, and APReductionAdjustedCost
    ''' </remarks>
    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DataTransferObjects.APMassEntry)
        'Create New Record
        Return New LTS.APMassEntry With {.APControl = d.APControl _
            , .APCarrierNumber = d.APCarrierNumber _
            , .APBillNumber = d.APBillNumber _
            , .APBillDate = d.APBillDate _
            , .APPONumber = d.APPONumber _
            , .APCustomerID = d.APCustomerID _
            , .APCostCenterNumber = d.APCostCenterNumber _
            , .APTotalCost = d.APTotalCost _
            , .APPRONumber = d.APPRONumber _
            , .APBLNumber = d.APBLNumber _
            , .APBilledWeight = d.APBilledWeight _
            , .APCNSNumber = d.APCNSNumber _
            , .APReceivedDate = d.APReceivedDate _
            , .APPayCode = d.APPayCode _
            , .APElectronicFlag = d.APElectronicFlag _
            , .APApprovedFlag = d.APApprovedFlag _
            , .APMessage = d.APMessage _
            , .APTotalTax = d.APTotalTax _
            , .APFee1 = d.APFee1 _
            , .APFee2 = d.APFee2 _
            , .APFee3 = d.APFee3 _
            , .APFee4 = d.APFee4 _
            , .APFee5 = d.APFee5 _
            , .APFee6 = d.APFee6 _
            , .APOtherCosts = d.APOtherCosts _
            , .APCarrierCost = d.APCarrierCost _
            , .APExportFlag = d.APExportFlag _
            , .APOrderSequence = d.APOrderSequence _
            , .APTaxDetail1 = d.APTaxDetail1 _
            , .APTaxDetail2 = d.APTaxDetail2 _
            , .APTaxDetail3 = d.APTaxDetail3 _
            , .APTaxDetail4 = d.APTaxDetail4 _
            , .APTaxDetail5 = d.APTaxDetail5 _
            , .APSHID = d.APSHID _
            , .APShipCarrierProNumber = d.APShipCarrierProNumber _
            , .APReduction = d.APReduction _
            , .APReductionReason = d.APReductionReason _
            , .APReductionAdjustedCost = d.APReductionAdjustedCost _
            , .APModUser = Parameters.UserName _
            , .APModDate = Date.Now _
            , .APUpdated = If(d.APUpdated Is Nothing, New Byte() {}, d.APUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetAPMassEntryFiltered(Control:=CType(LinqTable, LTS.APMassEntry).APControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim source As LTS.APMassEntry = TryCast(LinqTable, LTS.APMassEntry)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.APMassEntries
                    Where d.APControl = source.APControl
                    Select New DataTransferObjects.QuickSaveResults With {.Control = d.APControl _
                        , .ModDate = d.APModDate _
                        , .ModUser = d.APModUser _
                        , .Updated = d.APUpdated.ToArray}).First

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

        End Using
        Return ret
    End Function

#End Region

End Class