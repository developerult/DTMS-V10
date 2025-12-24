Imports NGL.FM.CarTar
Imports DAL = NGL.FreightMaster.Data
Imports DTO = NGL.FreightMaster.Data.DataTransferObjects
Imports BLL = NGL.FM.BLL
Public Class TestBase


    Enum WebServiceReturnValues
        nglDataIntegrationComplete
        nglDataConnectionFailure
        nglDataValidationFailure
        nglDataIntegrationFailure
        nglDataIntegrationHadErrors
    End Enum

    Private _testParameters As DAL.WCFParameters
    Public Property testParameters() As DAL.WCFParameters
        Get
            If _testParameters Is Nothing Then
                _testParameters = New DAL.WCFParameters() With {.Database = "NGLMASDEV7051",
                                                               .DBServer = "MININT-1CJQH8R\SQL2014",
                                                               .WCFAuthCode = "NGLWCFDEV"}
            End If
            Return _testParameters
        End Get
        Set(ByVal value As DAL.WCFParameters)
            _testParameters = value
        End Set
    End Property

    Protected Function getBookByPro(ByVal sProNumber As String) As DTO.Book
        Try
            Dim oBook As New DAL.NGLBookData(testParameters)
            Return oBook.GetBookFiltered(BookProNumber:=sProNumber)
        Catch ex As Exception
            Return Nothing 'no data is available
        End Try
    End Function

    Protected Function getBookRevsByBookControl(ByVal iBookControl As Integer) As DTO.BookRevenue()
        Try
            Dim oBook As New DAL.NGLBookRevenueData(testParameters)
            Return oBook.GetBookRevenuesWDetailsFiltered(iBookControl)
        Catch ex As Exception
            Return Nothing 'no data is available
        End Try
    End Function

    Protected Function getCarrierByName(ByVal sName As String) As DTO.Carrier
        Try
            Dim oCarrier As New DAL.NGLCarrierData(testParameters)
            Return oCarrier.GetCarrierFiltered(Name:=sName)
        Catch ex As Exception
            Return Nothing 'no data is available
        End Try
    End Function

    Protected Function getPOHDRs(ByVal ModVerify As String, ByVal CompNumber As Integer, Optional ByVal Count As Integer = 1) As List(Of DTO.POHdr)

        Try
            Dim oPOHDR As New DAL.NGLPOHdrData(testParameters)
            Return oPOHDR.GetPOHDRsByModVerify(ModVerify, CompNumber, Count)
        Catch ex As Exception
            Return Nothing 'no data is available
        End Try
    End Function

    Protected Function getPOHDR(ByVal OrderNumber As String, ByVal CompNumber As Integer) As DTO.POHdr

        Try
            Dim oPOHDR As New DAL.NGLPOHdrData(testParameters)
            Return oPOHDR.GetPOHdrFiltered(OrderNumber, 0, CompNumber)
        Catch ex As Exception
            Return Nothing 'no data is available
        End Try
    End Function

    Protected Function getLatestContract(ByVal carTarID As String) As DTO.CarrTarContract
        Try
            Dim oContracts As New DAL.NGLCarrTarContractData(testParameters)
            Return oContracts.GetCarrTarContractFiltered(carTarID)
        Catch ex As Exception
            Return Nothing 'no data is available
        End Try
    End Function

    Protected Function getLatestContract(ByVal carrtarcontrol As Integer) As DTO.CarrTarContract
        Try
            Dim oContracts As New DAL.NGLCarrTarContractData(testParameters)
            Return oContracts.GetCarrTarContractFiltered(carrtarcontrol)
        Catch ex As Exception
            Return Nothing 'no data is available
        End Try
    End Function

    Protected Sub deleteContract(ByVal contract As DTO.CarrTarContract)
        Try
            Dim oContracts As New DAL.NGLCarrTarContractData(testParameters)
            oContracts.DeleteRecord(contract)
        Catch ex As Exception

        End Try
    End Sub

    Protected Function getClassXrefData(ByVal cartarcontrol As Integer) As DTO.CarrTarClassXref()
        Try
            Dim data As New DAL.NGLCarrTarClassXrefData(testParameters)
            Return data.GetCarrTarClassXrefsFiltered(cartarcontrol)
        Catch ex As Exception
            Return Nothing 'no data is available
        End Try
    End Function

    Protected Function getDiscountData(ByVal cartarcontrol As Integer) As DTO.CarrTarDiscount()
        Try
            Dim data As New DAL.NGLCarrTarDiscountData(testParameters)
            Return data.GetCarrTarDiscountsFiltered(cartarcontrol)
        Catch ex As Exception
            Return Nothing 'no data is available
        End Try
    End Function

    Protected Function getInterlineData(ByVal cartarcontrol As Integer) As DTO.CarrTarInterline()
        Try
            Dim data As New DAL.NGLCarrTarInterlineData(testParameters)
            Return data.GetCarrTarInterlinesFiltered(cartarcontrol)
        Catch ex As Exception
            Return Nothing 'no data is available
        End Try
    End Function

    Protected Function getMinChargeData(ByVal cartarcontrol As Integer) As DTO.CarrTarMinCharge()
        Try
            Dim data As New DAL.NGLCarrTarMinChargeData(testParameters)
            Return data.GetCarrTarMinChargesFiltered(cartarcontrol)
        Catch ex As Exception
            Return Nothing 'no data is available
        End Try
    End Function

    Protected Function getMinWeightData(ByVal cartarcontrol As Integer) As DTO.CarrTarMinWeight()
        Try
            Dim data As New DAL.NGLCarrTarMinWeightData(testParameters)
            Return data.GetCarrTarMinWeightsFiltered(cartarcontrol)
        Catch ex As Exception
            Return Nothing 'no data is available
        End Try
    End Function

    Protected Function getNonServiceData(ByVal cartarcontrol As Integer) As DTO.CarrTarNonServ()
        Try
            Dim data As New DAL.NGLCarrTarNonServData(testParameters)
            Return data.GetCarrTarNonServsFiltered(cartarcontrol)
        Catch ex As Exception
            Return Nothing 'no data is available
        End Try
    End Function

    Protected Function getTariffEquipmentData(ByVal cartarcontrol As Integer) As DTO.CarrTarEquip()
        Try
            Dim data As New DAL.NGLCarrTarEquipData(testParameters)
            Return data.GetCarrTarEquipsFiltered(cartarcontrol)
        Catch ex As Exception
            Return Nothing 'no data is available
        End Try
    End Function

    Protected Function cloneTariff(ByVal CarrTarControl As Integer, _
                                ByVal EffDateFrom As Date?, _
                                ByVal EffDateTo As Date?, _
                                ByVal AutoApprove As Boolean, _
                                ByVal IssuedDate As Date?, _
                                ByVal CopyClassXrefData As Boolean, _
                                ByVal CopyNoDriveDays As Boolean, _
                                ByVal CopyDiscountData As Boolean, _
                                ByVal CopyFeeData As Boolean, _
                                ByVal CopyInterlinePointData As Boolean, _
                                ByVal CopyMinChargeData As Boolean, _
                                ByVal CopyMinWeightData As Boolean, _
                                ByVal CopyNonServicePointData As Boolean, _
                                ByVal CopyMatrixBPData As Boolean, _
                                ByVal CopyEquipmentData As Boolean, _
                                ByVal CopyEquipmentRateData As Boolean, _
                                ByVal CopyFuelData As Boolean) As DTO.GenericResults
        Try
            Dim data As New DAL.NGLCarrTarContractData(testParameters)
            Return data.CloneTariff(CarrTarControl, _
                                                EffDateFrom, _
                                                EffDateTo, _
                                                AutoApprove, _
                                                IssuedDate, _
                                                CopyClassXrefData, _
                                                CopyNoDriveDays, _
                                                CopyDiscountData, _
                                                CopyFeeData, _
                                                CopyInterlinePointData, _
                                                CopyMinChargeData, _
                                                CopyMinWeightData, _
                                                CopyNonServicePointData, _
                                                CopyMatrixBPData, _
                                                CopyEquipmentData, _
                                                CopyEquipmentRateData, _
                                                CopyFuelData)
        Catch ex As Exception
            Return Nothing 'no data is available
        End Try
    End Function

    Protected Function copyTariff(ByVal CarrTarControl As Integer, _
                                ByVal EffDateFrom As Date?, _
                                ByVal EffDateTo As Date?, _
                                ByVal AutoApprove As Boolean, _
                                ByVal IssuedDate As Date?, _
                                ByVal CopyClassXrefData As Boolean, _
                                ByVal CopyNoDriveDays As Boolean, _
                                ByVal CopyDiscountData As Boolean, _
                                ByVal CopyFeeData As Boolean, _
                                ByVal CopyInterlinePointData As Boolean, _
                                ByVal CopyMinChargeData As Boolean, _
                                ByVal CopyMinWeightData As Boolean, _
                                ByVal CopyNonServicePointData As Boolean, _
                                ByVal CopyMatrixBPData As Boolean, _
                                ByVal CopyEquipmentData As Boolean, _
                                ByVal CopyEquipmentRateData As Boolean, _
                                ByVal CopyFuelData As Boolean, _
                                ByVal newCompControl As Integer, _
                                ByVal newContractName As String) As DTO.GenericResults
        Try
            Dim data As New DAL.NGLCarrTarContractData(testParameters)
            Return data.CopyTariff(CarrTarControl, _
                                                EffDateFrom, _
                                                EffDateTo, _
                                                AutoApprove, _
                                                IssuedDate, _
                                                CopyClassXrefData, _
                                                CopyNoDriveDays, _
                                                CopyDiscountData, _
                                                CopyFeeData, _
                                                CopyInterlinePointData, _
                                                CopyMinChargeData, _
                                                CopyMinWeightData, _
                                                CopyNonServicePointData, _
                                                CopyMatrixBPData, _
                                                CopyEquipmentData, _
                                                CopyEquipmentRateData, _
                                                CopyFuelData,
                                                newCompControl,
                                                newContractName)
        Catch ex As Exception
            Return Nothing 'no data is available
        End Try
    End Function



    '		key	"165-1-0-0"	string 
    Protected Function getRatesData(ByVal altkey As String) As DTO.CarrTarEquipMatPivot()
        Try
            Dim data As New DAL.NGLCarrTarEquipMatData(testParameters)
            Return data.GetCarrTarEquipMatPivotsByAltKey(altkey)
        Catch ex As Exception
            Return Nothing 'no data is available
        End Try
    End Function

    Protected Function gettarifffeesData(ByVal cartarcontrol As Integer) As DTO.CarrTarFee()
        Try
            Dim data As New DAL.NGLCarrTarFeeData(testParameters)
            Return data.GetCarrTarFeesFiltered(cartarcontrol)
        Catch ex As Exception
            Return Nothing 'no data is available
        End Try
    End Function

    Protected Function getTariffFuelAddundumData(ByVal cartarcontrol As Integer) As DTO.CarrierFuelAddendum
        Try
            Dim data As New DAL.NGLCarrierFuelAddendumData(testParameters)
            Return data.GetCarrierFuelAddendumsCarTarFiltered(cartarcontrol)
        Catch ex As Exception
            Return Nothing 'no data is available
        End Try
    End Function

    Protected Function getTariffFuelExceptionsData(ByVal carrAdFuelControl As Integer) As DTO.CarrierFuelAdEx()
        Try
            Dim data As New DAL.NGLCarrierFuelAdExData(testParameters)
            Return data.GetCarrierFuelAdExsWPagingFiltered(carrAdFuelControl)
        Catch ex As Exception
            Return Nothing 'no data is available
        End Try
    End Function

    Protected Function getTariffFuelAdRateData(ByVal carrAdFuelControl As Integer) As DTO.CarrierFuelAdRate()
        Try
            Dim data As New DAL.NGLCarrierFuelAdRateData(testParameters)
            Return data.GetCarrierFuelAdRatesWPagingFiltered(carrAdFuelControl)
        Catch ex As Exception
            Return Nothing 'no data is available
        End Try
    End Function

    Protected Function getNoDriveDaysData(ByVal cartarcontrol As Integer) As DTO.CarrTarNoDriveDays()
        Try
            Dim data As New DAL.NGLCarrierTariffNoDriveDays(testParameters)
            Return data.GetCarrTarNDDsFiltered(cartarcontrol)
        Catch ex As Exception
            Return Nothing 'no data is available
        End Try
    End Function

    Public Sub DeleteTestOrder(ByVal BookProNumber As String)
        Dim oBook As New DAL.NGLBookData(testParameters)
        Dim sbQry As New System.Text.StringBuilder()
        sbQry.Append(String.Format("If Exists(Select * from dbo.book where BookProNumber = {0}) {1}", BookProNumber, vbCrLf))
        sbQry.Append(String.Format(" Delete from dbo.book where BookProNumber = {0} {1}", BookProNumber, vbCrLf))
        oBook.executeSQL(sbQry.ToString())
    End Sub

    Public Sub DeleteTestOrders(ByVal BookProNumbers As List(Of String))
        If BookProNumbers Is Nothing OrElse BookProNumbers.Count < 1 Then Return

        Dim oBook As New DAL.NGLBookData(testParameters)
        Dim sbQry As New System.Text.StringBuilder()
        For Each BookProNumber In BookProNumbers
            sbQry.Append(vbCrLf)
            sbQry.Append(String.Format("If Exists(Select * from dbo.book where BookProNumber = {0}) {1}", BookProNumber, vbCrLf))
            sbQry.Append(String.Format(" Begin Delete from dbo.book where BookProNumber = {0} End {1}", BookProNumber, vbCrLf))
            sbQry.Append(vbCrLf)
        Next
        oBook.executeSQL(sbQry.ToString())
    End Sub

    ''' <summary>
    ''' This method always creates an outbound order for 
    ''' compcontrol -- 9623
    ''' LaneControl -- 56600
    ''' LoadDate is 3 days after the order date
    ''' RequiredDate is 5 days after the order date
    ''' </summary>
    ''' <param name="BookProPrefix"></param>
    ''' <param name="BookProBase"></param>
    ''' <param name="BookConsPrefix"></param>
    ''' <param name="OrderNumber"></param>
    ''' <param name="OrderDate"></param>
    ''' <param name="BookTranCode"></param>
    ''' <param name="CarrierNumber">Default is Veterans Truck Line (80)</param>
    ''' <remarks></remarks>
    Public Sub CreateTestOrder(ByVal BookProPrefix As String, _
                                ByVal BookProBase As String, _
                                ByVal BookConsPrefix As String, _
                                ByVal OrderNumber As String, _
                                ByVal OrderDate As Date, _
                                ByVal BookTranCode As String, _
                                Optional ByVal CarrierNumber As Integer = 80)


        Dim oBook As New DAL.NGLBookData(testParameters)
        Dim dtOrderd = OrderDate
        Dim dtLoad As Date = OrderDate.AddDays(3)
        Dim dtRequired As Date = dtLoad.AddDays(5)
        Dim sbQry As New System.Text.StringBuilder()
        sbQry.Append(String.Format("Declare @BookProBase nvarchar(20) = '{0}' {1}", BookProBase, vbCrLf))
        sbQry.Append(String.Format("Declare @BookProPrefix nvarchar(3) = '{0}' {1}", BookProPrefix, vbCrLf))
        sbQry.Append(String.Format("Declare @BookConsPrefix nvarchar(20) = '{0}' {1}", BookConsPrefix, vbCrLf))
        sbQry.Append(String.Format("Declare @BookProNumber nvarchar(20) = @BookProPrefix + @BookProBase {0}", vbCrLf))
        sbQry.Append(String.Format("Declare @BookCarrOrderNumber nvarchar(20) = '{0}' {1}", OrderNumber, vbCrLf))
        sbQry.Append(String.Format("Declare @BookTranCode nvarchar(3) = '{0}' {1}", BookTranCode, vbCrLf))
        sbQry.Append(String.Format("Delete from dbo.book where BookProNumber = @BookProNumber {0}", vbCrLf))
        sbQry.Append(String.Format("Declare @CarrierNumber int = {0} {1}", CarrierNumber, vbCrLf))
        sbQry.Append(String.Format("Declare @BookCarrierControl int {0}", vbCrLf))
        sbQry.Append(String.Format("Declare @BookCarrierContact nvarchar(30) {0}", vbCrLf))
        sbQry.Append(String.Format("Declare @BookCarrierContactPhone nvarchar(20) {0}", vbCrLf))
        sbQry.Append(String.Format("Declare @BookCarrierContactControl int {0}", vbCrLf))

        sbQry.Append(String.Format("Select @BookCarrierControl = CarrierControl from dbo.Carrier where CarrierNumber = @CarrierNumber {0}", vbCrLf))
        sbQry.Append(String.Format("Select top 1 @BookCarrierContactControl = CarrierContControl,@BookCarrierContact = CarrierContName,@BookCarrierContactPhone = CarrierContactPhone  From dbo.CarrierCont where CarrierContCarrierControl = @BookCarrierControl {0}", vbCrLf))

        sbQry.Append(String.Format("INSERT INTO [dbo].[Book] ([BookProNumber], [BookProBase], [BookConsPrefix], [BookCustCompControl], [BookCommCompControl], [BookODControl], [BookCarrierControl], [BookCarrierContact], [BookCarrierContactPhone], [BookOrigCompControl], [BookOrigName], [BookOrigAddress1], [BookOrigAddress2], [BookOrigAddress3], [BookOrigCity], [BookOrigState], [BookOrigCountry], [BookOrigZip], [BookOrigPhone], [BookOrigFax], [BookOriginStartHrs], [BookOriginStopHrs], [BookOriginApptReq], [BookDestCompControl], [BookDestName], [BookDestAddress1], [BookDestAddress2], [BookDestAddress3], [BookDestCity], [BookDestState], [BookDestCountry], [BookDestZip], [BookDestPhone], [BookDestFax], [BookDestStartHrs], [BookDestStopHrs], [BookDestApptReq], [BookDateOrdered], [BookDateLoad], [BookDateInvoice], [BookDateRequired], [BookDateDelivered], [BookTotalCases], [BookTotalWgt], [BookTotalPL], [BookTotalCube], [BookTotalPX], [BookTotalBFC], [BookTranCode], [BookPayCode], [BookTypeCode], [BookBOLCode], [BookStopNo], [BookModDate], [BookModUser], [BookCarrControl], [BookCarrFBNumber], [BookCarrOrderNumber], [BookCarrBLNumber], [BookCarrBookDate], [BookCarrBookTime], [BookCarrBookContact], [BookCarrScheduleDate], [BookCarrScheduleTime], [BookCarrActualDate], [BookCarrActualTime], [BookCarrActLoadComplete Date], [BookCarrActLoadCompleteTime], [BookCarrDockPUAssigment], [BookCarrPODate], [BookCarrPOTime], [BookCarrApptDate], [BookCarrApptTime], [BookCarrActDate], [BookCarrActTime], [BookCarrActUnloadCompDate], [BookCarrActUnloadCompTime], [BookCarrDockDelAssignment], [BookCarrVarDay], [BookCarrVarHrs], [BookCarrTrailerNo], [BookCarrSealNo], [BookCarrDriverNo], [BookCarrDriverName], [BookCarrRouteNo], [BookCarrTripNo], [BookFinControl], [BookFinARBookFrt], [BookFinARInvoiceDate], [BookFinARInvoiceAmt], [BookFinARPayDate], [BookFinARPayAmt], [BookFinARCheck], [BookFinARGLNumber], [BookFinARBalance], [BookFinARCurType], [BookFinAPBillNumber], [BookFinAPBillNoDate], [BookFinAPBillInvDate], [BookFinAPActWgt], [BookFinAPStdCost], [BookFinAPActCost], [BookFinAPPayDate], [BookFinAPPayAmt], [BookFinAPCheck], [BookFinAPGLNumber], [BookFinAPLastViewed], [BookFinAPCurType], [BookFinCommStd], [BookFinCommAct], [BookFinCommPayDate], [BookFinCommPayAmt], [BookFinCommtCheck], [BookFinCommCreditAmt], [BookFinCommCreditPayDate], [BookFinCommLoadCount], [BookFinCommGLNumber], [BookFinCheckClearedDate], [BookFinCheckClearedNumber], [BookFinCheckClearedAmt], [BookFinCheckClearedDesc], [BookFinCheckClearedAcct], [BookRevControl], [BookRevBilledBFC], [BookRevCarrierCost], [BookRevStopQty], [BookRevStopCost], [BookRevOtherCost], [BookRevTotalCost], [BookRevLoadSavings], [BookRevCommPercent], [BookRevCommCost], [BookRevGrossRevenue], [BookRevNegRevenue], [BookMilesFrom], [BookLaneCarrControl], [BookHoldLoad], [BookRouteFinalDate], [BookRouteFinalCode], [BookRouteFinalFlag], [BookWarehouseNumber], [BookComCode], [BookTransType], [BookRouteConsFlag], [BookWhseAuthorizationNo], [BookHotLoad], [BookFinAPActTax], [BookFinAPExportFlag], [BookFinARFreightTax], [BookRevFreightTax], [BookRevNetCost], [BookFinServiceFee], [BookFinAPExportDate], [BookFinAPExportRetry], [BookCarrierContControl], [BookHotLoadSent], [BookExportDocCreated], [BookDoNotInvoice], [BookCarrStartLoadingDate], [BookCarrStartLoadingTime], [BookCarrFinishLoadingDate], [BookCarrFinishLoadingTime], [BookCarrStartUnloadingDate], [BookCarrStartUnloadingTime], [BookCarrFinishUnloadingDate], [BookCarrFinishUnloadingTime], [BookOrderSequence], [BookChepGLID], [BookCarrierTypeCode], [BookPalletPositions], [BookShipCarrierProNumber], [BookShipCarrierProNumberRaw], [BookShipCarrierProControl], [BookShipCarrierName], [BookShipCarrierNumber], [BookAPAdjReasonControl], [BookDateRequested], [BookCarrierEquipmentCodes], [BookShippedDataExported], [BookLockAllCosts], [BookLockBFCCost], [BookRevNonTaxable], [BookPickupStopNumber], [BookOrigStopNumber], [BookDestStopNumber], [BookOrigMiles], [BookDestMiles], [BookOrigPCMCost], [BookDestPCMCost], [BookOrigPCMTime], [BookDestPCMTime], [BookOrigPCMTolls], [BookDestPCMTolls], [BookOrigPCMESTCHG], [BookDestPCMESTCHG], [BookPickNumber], [BookAMSPickupApptControl], [BookAMSDeliveryApptControl], [BookItemDetailDescription], [BookOrigStopControl], [BookDestStopControl], [BookRouteTypeCode], [BookAlternateAddressLaneControl], [BookAlternateAddressLaneNumber], [BookDefaultRouteSequence], [BookRouteGuideControl], [BookRouteGuideNumber], [BookCustomerApprovalTransmitted], [BookCustomerApprovalRecieved], [BookCarrTruckControl], [BookCarrTarControl], [BookCarrTarRevisionNumber], [BookCarrTarName], [BookCarrTarEquipControl], [BookCarrTarEquipName], [BookCarrTarEquipMatControl], [BookCarrTarEquipMatName], [BookCarrTarEquipMatDetControl], [BookCarrTarEquipMatDetID], [BookCarrTarEquipMatDetValue], [BookBookRevHistRevision], [BookModeTypeControl], [BookAllowInterlinePoints], [BookRevLaneBenchMiles], [BookRevLoadMiles], [BookUser1], [BookUser2], [BookUser3], [BookUser4], [BookRevDiscount], [BookRevLineHaul], [BookMustLeaveByDateTime], [BookMultiMode], [BookOriginalLaneControl], [BookLaneTranXControl], [BookLaneTranXDetControl], [BookSHID], [BookShipCarrierDetails], [BookExpDelDateTime], [BookOutOfRouteMiles], [BookSpotRateAllocationFormula], [BookSpotRateAutoCalcBFC], [BookSpotRateUseCarrierFuelAddendum], [BookSpotRateBFCAllocationFormula], [BookSpotRateTotalUnallocatedBFC], [BookSpotRateTotalUnallocatedLineHaul], [BookSpotRateUseFuelAddendum], [BookCreditHold], [BookBestDeficitCost], [BookBestDeficitWeight], [BookBestDeficitWeightBreak], [BookRatedWeightBreak], [BookWgtAdjCost], [BookWgtAdjWeight], [BookWgtAdjWeightBreak], [BookBilledLoadWeight], [BookMinAdjustedLoadWeight], [BookSummedClassWeight], [BookWgtRoundingVariance], [BookHeaviestClass], [BookAcutalHeaviestClassWeight], [BookRevDiscountRate], [BookRevDiscountMin]) VALUES (@BookProNumber, @BookProBase,@BookConsPrefix, 9623, 9623, 56600, @BookCarrierControl, @BookCarrierContact, @BookCarrierContactPhone, 9623, N'Vegetable Juices, Inc.', N'7400 South Narragansett Avenue', N'', N'', N'Bedford Park', N'IL', N'US', N'60638', N'', N'', NULL, NULL, 1, 0, N'Create A Pack Foods', N'W1344 Industrial Dr', N'', N'', N'Ixonia', N'WI', N'US', N'53036', N'', N'', NULL, NULL, 1, N'{1}', N'{2}', NULL, N'{3}', NULL, 8, 8000, 8, 800, 0, CAST(0.0000 AS Money), @BookTranCode, N'N', N'N/A', 0, 1, GetDate(), N'UnitTest', NULL, N'', @BookCarrOrderNumber, N'', NULL, NULL, N'', NULL, NULL, NULL, NULL, NULL, NULL, N'', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'', 0, 0, N'', N'', N'', N'', N'', N'', NULL, CAST(0.0000 AS Money), NULL, CAST(0.0000 AS Money), NULL, CAST(0.0000 AS Money), N'', N'', CAST(0.0000 AS Money), 1, N'', NULL, NULL, 0, CAST(472.8400 AS Money), CAST(0.0000 AS Money), NULL, CAST(0.0000 AS Money), N'', N'', NULL, 1, CAST(-472.8400 AS Money), CAST(0.0000 AS Money), NULL, CAST(0.0000 AS Money), N'', CAST(0.0000 AS Money), NULL, 0, N'', NULL, N'', CAST(0.0000 AS Money), N'', N'', NULL, CAST(0.0000 AS Money), CAST(242.9000 AS Money), 0, CAST(0.0000 AS Money), CAST(229.9400 AS Money), CAST(472.8400 AS Money), CAST(-472.8400 AS Money), 100, CAST(-472.8400 AS Money), CAST(0.0000 AS Money), 1, 138.8, 0, 1, NULL, N'', 0, N'', N'', N'0', 1, N'', 0, CAST(0.0000 AS Money), 0, CAST(0.0000 AS Money), CAST(0.0000 AS Money), CAST(472.8400 AS Money), CAST(0.0000 AS Money), NULL, 0, @BookCarrierContactControl, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, N'', N'', N'', N'', N'', NULL, N'', N'', 0, N'{3}', N'', 0, 0, 0, CAST(0.0000 AS Money), 0, 0, 0, 0, 0, 0, 0, NULL, NULL, CAST(0.0000 AS Money), CAST(0.0000 AS Money), 0, 0, 0, 0, 0, N'', 0, 0, 6, 0, N'', 0, 0, N'', 0, 0, 0, 880, 1, N'', 1419, N'', 104913, N'', 0, 1, CAST(1.7500 AS Decimal(18, 4)), 1, 3, 1, 138.8, 138.8, N'', N'', N'', N'', CAST(0.0000 AS Decimal(18, 4)), CAST(242.9000 AS Decimal(18, 4)), N'{3}', 0, 56600, 0, 0, NULL, NULL, N'{3}', 0, 0, 0, 0, 0, CAST(0.0000 AS Decimal(18, 4)), CAST(0.0000 AS Decimal(18, 4)), 0, 0, CAST(0.0000 AS Decimal(18, 4)), 0, 0, 0, CAST(0.0000 AS Decimal(18, 4)), 0, 0, 0, 0, 0, 0, NULL, 0, CAST(0.0000 AS Decimal(18, 4)), CAST(0.0000 AS Decimal(18, 4))) {0}", vbCrLf, dtOrderd.ToShortDateString(), dtLoad.ToShortDateString(), dtRequired.ToShortDateString()))
        sbQry.Append(String.Format("--Get the bookcontrol number back {0}", vbCrLf))
        sbQry.Append(String.Format("Declare @BookControl int = SCOPE_IDENTITY() {0}", vbCrLf))
        sbQry.Append(String.Format("INSERT INTO [dbo].[BookLoad] ([BookLoadBookControl], [BookLoadBuy], [BookLoadPONumber], [BookLoadVendor], [BookLoadCaseQty], [BookLoadWgt], [BookLoadCube], [BookLoadPL], [BookLoadPX], [BookLoadPType], [BookLoadCom], [BookLoadPUOrigin], [BookLoadBFC], [BookLoadTotCost], [BookLoadComments], [BookLoadStopSeq], [BookLoadModDate], [BookLoadModUser]) VALUES (@BookControl, NULL, N'SO-TEST1118', N'Vegetable Juices, Inc.', 8, 8000, 800, 8, 0, N'N', N'D', N'Bedford Park', CAST(0.0000 AS Money), CAST(472.8400 AS Money), NULL, 1,getdate(), N'UnitTest') {0}", vbCrLf))
        sbQry.Append(String.Format("--Get the bookloadcontrol number back {0}", vbCrLf))
        sbQry.Append(String.Format("Declare @BookLoadControl int = SCOPE_IDENTITY()  {0}", vbCrLf))
        sbQry.Append(String.Format("INSERT INTO [dbo].[BookItem] ([BookItemBookLoadControl], [BookItemFixOffInvAllow], [BookItemFixFrtAllow], [BookItemItemNumber], [BookItemQtyOrdered], [BookItemFreightCost], [BookItemItemCost], [BookItemWeight], [BookItemCube], [BookItemPack], [BookItemSize], [BookItemDescription], [BookItemHazmat], [BookItemModDate], [BookItemModUser], [BookItemBrand], [BookItemCostCenter], [BookItemLotNumber], [BookItemLotExpirationDate], [BookItemGTIN], [BookCustItemNumber], [BookItemBFC], [BookItemCountryOfOrigin], [BookItemHST], [BookItemPalletTypeID], [BookItemHazmatTypeCode], [BookItem49CFRCode], [BookItemIATACode], [BookItemDOTCode], [BookItemMarineCode], [BookItemNMFCClass], [BookItemFAKClass], [BookItemLimitedQtyFlag], [BookItemPallets], [BookItemTies], [BookItemHighs], [BookItemQtyPalletPercentage], [BookItemQtyLength], [BookItemQtyWidth], [BookItemQtyHeight], [BookItemStackable], [BookItemLevelOfDensity], [BookItemDiscount], [BookItemLineHaul], [BookItemTaxableFees], [BookItemTaxes], [BookItemNonTaxableFees], [BookItemDeficitCostAdjustment], [BookItemDeficitWeightAdjustment], [BookItemWeightBreak], [BookItemDeficit49CFRCode], [BookItemDeficitIATACode], [BookItemDeficitDOTCode], [BookItemDeficitMarineCode], [BookItemDeficitNMFCClass], [BookItemDeficitFAKClass], [BookItemRated49CFRCode], [BookItemRatedIATACode], [BookItemRatedDOTCode], [BookItemRatedMarineCode], [BookItemRatedNMFCClass], [BookItemRatedFAKClass], [BookItemCarrTarEquipMatControl], [BookItemCarrTarEquipMatName], [BookItemCarrTarEquipMatDetID], [BookItemCarrTarEquipMatDetValue], [BookItemUser1], [BookItemUser2], [BookItemUser3], [BookItemUser4], [BookItemUnitOfMeasureControl], [BookItemRatedNMFCSubClass], [BookItemCommCode]) VALUES ( @BookLoadControl, NULL, NULL, N'1234', 8, CAST(472.8400 AS Money), CAST(100.0000 AS Money), 8000, 800, NULL, NULL, N'Test Item', NULL, getDate(), N'UnitTest',  NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, CAST(0.0000 AS Decimal(18, 4)), CAST(0.0000 AS Decimal(18, 4)), CAST(0.0000 AS Decimal(18, 4)), CAST(0.0000 AS Decimal(18, 4)), CAST(0.0000 AS Decimal(18, 4)), CAST(0.0000 AS Decimal(18, 4)), CAST(0.0000 AS Decimal(18, 4)), CAST(0.0000 AS Decimal(18, 4)), NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, 0, CAST(0.0000 AS Decimal(18, 4)), NULL, NULL, NULL, NULL, 0, NULL, N'3') {0}", vbCrLf))
        oBook.executeSQL(sbQry.ToString())
    End Sub


    Public Sub CreateTestPOs(ByVal OrderDate As Date)
        Dim oBook As New DAL.NGLBookData(testParameters)
        Dim dtOrderd = OrderDate
        Dim dtLoad As Date = OrderDate.AddDays(3)
        Dim dtRequired As Date = dtLoad.AddDays(5)
        Dim sbQry As New System.Text.StringBuilder()

        sbQry.Append(String.Format("Declare @OrderNumberPrefix nvarchar(20) = 'SO-TEST' {0}", vbCrLf))
        sbQry.Append(String.Format("Declare @BookCarrOrderNumberSeed int = 21111 {0}", vbCrLf))
        sbQry.Append(String.Format("Declare @BookCarrOrderNumber nvarchar(20) = @OrderNumberPrefix + Cast(@BookCarrOrderNumberSeed as nvarchar(10)) {0}", vbCrLf))
        sbQry.Append(String.Format("Declare @DefaultCustomer nvarchar(20) = '31' {0}", vbCrLf))
        sbQry.Append(String.Format("Declare @POHDRPOdate datetime = '{1}'{0}", vbCrLf, dtOrderd.ToShortDateString()))
        sbQry.Append(String.Format("Declare @POHDRShipdate datetime = '{1}'{0}", vbCrLf, dtLoad.ToShortDateString()))
        sbQry.Append(String.Format("Declare @POHDRReqDate datetime = '{1}'{0}", vbCrLf, dtRequired.ToShortDateString()))
        sbQry.Append(String.Format("Declare @CreateDate datetime = getdate(){0}", vbCrLf))
        sbQry.Append(String.Format("Declare @CreateUser nvarchar(50) = N'UnitTest' {0}", vbCrLf))
        sbQry.Append(String.Format("Delete from POHdr where POHDRCreateUser = @CreateUser{0}", vbCrLf))

        sbQry.Append(String.Format("INSERT INTO [dbo].[POHdr] ([POHDRnumber], [POHDRvendor], [POHDRPOdate], [POHDRShipdate], [POHDRBuyer], [POHDRFrt], [POHDRCreateUser], [POHDRCreateDate], [POHDRModUser], [POHDRTotalFrt], [POHDRTotalCost], [POHDRWgt], [POHDRCube], [POHDRQty], [POHDRLines], [POHDRConfirm], [POHDRDefaultCustomer], [POHDRDefaultCustomerName], [POHDRDefaultCarrier], [POHDRReqDate], [POHDROrderNumber], [POHDRShipInstructions], [POHDRCooler], [POHDRFrozen], [POHDRDry], [POHDRTemp], [POHDRModVerify], [POHDROrigName], [POHDROrigCity], [POHDROrigState], [POHDROrigZip], [POHDRDestName], [POHDRDestCity], [POHDRDestState], [POHDRDestZip], [POHDRCarType], [POHDRShipVia], [POHDRShipViaType], [POHDRPallets], [POHDROtherCost], [POHDRStatusFlag], [POHDRSortOrder], [POHDRPRONumber], [POHDRHoldLoad], [POHDROrderSequence], [POHDRChepGLID], [POHDRCarrierEquipmentCodes], [POHDRCarrierTypeCode], [POHDRPalletPositions], [POHDRSchedulePUDate], [POHDRSchedulePUTime], [POHDRScheduleDelDate], [POHDRScheduleDelTime], [POHDRActPUDate], [POHDRActPUTime], [POHDRActDelDate], [POHDRActDelTime], [POHDROrigCompNumber], [POHDROrigAddress1], [POHDROrigAddress2], [POHDROrigAddress3], [POHDROrigCountry], [POHDROrigContactPhone], [POHDROrigContactPhoneExt], [POHDROrigContactFax], [POHDRDestCompNumber], [POHDRDestAddress1], [POHDRDestAddress2], [POHDRDestAddress3], [POHDRDestCountry], [POHDRDestContactPhone], [POHDRDestContactPhoneExt], [POHDRDestContactFax], [POHDRPalletExchange], [POHDRPalletType], [POHDRComments], [POHDRCommentsConfidential], [POHDRInbound], [POHDRDefaultRouteSequence], [POHDRRouteGuideNumber], [POHDRCompLegalEntity], [POHDRCompAlphaCode], [POHDRModeTypeControl], [POHDRMustLeaveByDateTime], [POHDRUser1], [POHDRUser2], [POHDRUser3], [POHDRUser4], [POHDRAPGLNumber]){0}", vbCrLf))
        sbQry.Append(String.Format("VALUES  {0}", vbCrLf))
        sbQry.Append(String.Format("(@BookCarrOrderNumber, N'31-5STAR-AMERIC', @POHDRPOdate, @POHDRShipdate, NULL, NULL, @CreateUser, @CreateDate, @CreateUser, NULL, NULL, 1, 0, 1, NULL, 0,@DefaultCustomer, N'Vegetable Juices, Inc.', 0, @POHDRReqDate, @BookCarrOrderNumber, N'', 0, 0, 0, N'D',N'No Pro', N'Vegetable Juices, Inc.', N'Bedford Park', N'IL', N'60638', N'AMERICOLD', N'FT. WORTH', N'TX', N'76102', NULL, NULL, NULL, 1, NULL, NULL, 2, N'0', 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL,NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, 0, 0, NULL, NULL, NULL, 3, NULL, NULL, NULL, NULL, NULL, NULL){0}", vbCrLf))
        sbQry.Append(String.Format("set  @BookCarrOrderNumberSeed  = @BookCarrOrderNumberSeed + 1 {0}", vbCrLf))
        sbQry.Append(String.Format("Set @BookCarrOrderNumber  = @OrderNumberPrefix + Cast(@BookCarrOrderNumberSeed as nvarchar(10)) {0}", vbCrLf))

        sbQry.Append(String.Format("INSERT INTO [dbo].[POHdr] ([POHDRnumber], [POHDRvendor], [POHDRPOdate], [POHDRShipdate], [POHDRBuyer], [POHDRFrt], [POHDRCreateUser], [POHDRCreateDate], [POHDRModUser], [POHDRTotalFrt], [POHDRTotalCost], [POHDRWgt], [POHDRCube], [POHDRQty], [POHDRLines], [POHDRConfirm], [POHDRDefaultCustomer], [POHDRDefaultCustomerName], [POHDRDefaultCarrier], [POHDRReqDate], [POHDROrderNumber], [POHDRShipInstructions], [POHDRCooler], [POHDRFrozen], [POHDRDry], [POHDRTemp], [POHDRModVerify], [POHDROrigName], [POHDROrigCity], [POHDROrigState], [POHDROrigZip], [POHDRDestName], [POHDRDestCity], [POHDRDestState], [POHDRDestZip], [POHDRCarType], [POHDRShipVia], [POHDRShipViaType], [POHDRPallets], [POHDROtherCost], [POHDRStatusFlag], [POHDRSortOrder], [POHDRPRONumber], [POHDRHoldLoad], [POHDROrderSequence], [POHDRChepGLID], [POHDRCarrierEquipmentCodes], [POHDRCarrierTypeCode], [POHDRPalletPositions], [POHDRSchedulePUDate], [POHDRSchedulePUTime], [POHDRScheduleDelDate], [POHDRScheduleDelTime], [POHDRActPUDate], [POHDRActPUTime], [POHDRActDelDate], [POHDRActDelTime], [POHDROrigCompNumber], [POHDROrigAddress1], [POHDROrigAddress2], [POHDROrigAddress3], [POHDROrigCountry], [POHDROrigContactPhone], [POHDROrigContactPhoneExt], [POHDROrigContactFax], [POHDRDestCompNumber], [POHDRDestAddress1], [POHDRDestAddress2], [POHDRDestAddress3], [POHDRDestCountry], [POHDRDestContactPhone], [POHDRDestContactPhoneExt], [POHDRDestContactFax], [POHDRPalletExchange], [POHDRPalletType], [POHDRComments], [POHDRCommentsConfidential], [POHDRInbound], [POHDRDefaultRouteSequence], [POHDRRouteGuideNumber], [POHDRCompLegalEntity], [POHDRCompAlphaCode], [POHDRModeTypeControl], [POHDRMustLeaveByDateTime], [POHDRUser1], [POHDRUser2], [POHDRUser3], [POHDRUser4], [POHDRAPGLNumber]) {0}", vbCrLf))
        sbQry.Append(String.Format("VALUES  {0}", vbCrLf))
        sbQry.Append(String.Format("(@BookCarrOrderNumber,  N'31 Create A Pack', @POHDRPOdate, @POHDRShipdate, NULL, NULL, @CreateUser, @CreateDate, @CreateUser, NULL, NULL, 26000, 2600, 26, NULL, 0, @DefaultCustomer, N'Vegetable Juices, Inc.', 0, @POHDRReqDate, @BookCarrOrderNumber, NULL, 0, 0, 1, N'D', N'No Pro', N'Vegetable Juices, Inc.', N'Bedford Park', N'IL', N'60638', N'Create A Pack Foods', N'Ixonia', N'WI', N'53036', NULL, NULL, NULL, 26, NULL, NULL, 2, N'0', 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, 0, 0, NULL, NULL, NULL, 3, NULL, NULL, NULL, NULL, NULL, NULL){0}", vbCrLf))
        sbQry.Append(String.Format("set  @BookCarrOrderNumberSeed  = @BookCarrOrderNumberSeed + 1 {0}", vbCrLf))
        sbQry.Append(String.Format("Set @BookCarrOrderNumber  = @OrderNumberPrefix + Cast(@BookCarrOrderNumberSeed as nvarchar(10)) {0}", vbCrLf))

        sbQry.Append(String.Format("INSERT INTO [dbo].[POHdr] ([POHDRnumber], [POHDRvendor], [POHDRPOdate], [POHDRShipdate], [POHDRBuyer], [POHDRFrt], [POHDRCreateUser], [POHDRCreateDate], [POHDRModUser], [POHDRTotalFrt], [POHDRTotalCost], [POHDRWgt], [POHDRCube], [POHDRQty], [POHDRLines], [POHDRConfirm], [POHDRDefaultCustomer], [POHDRDefaultCustomerName], [POHDRDefaultCarrier], [POHDRReqDate], [POHDROrderNumber], [POHDRShipInstructions], [POHDRCooler], [POHDRFrozen], [POHDRDry], [POHDRTemp], [POHDRModVerify], [POHDROrigName], [POHDROrigCity], [POHDROrigState], [POHDROrigZip], [POHDRDestName], [POHDRDestCity], [POHDRDestState], [POHDRDestZip], [POHDRCarType], [POHDRShipVia], [POHDRShipViaType], [POHDRPallets], [POHDROtherCost], [POHDRStatusFlag], [POHDRSortOrder], [POHDRPRONumber], [POHDRHoldLoad], [POHDROrderSequence], [POHDRChepGLID], [POHDRCarrierEquipmentCodes], [POHDRCarrierTypeCode], [POHDRPalletPositions], [POHDRSchedulePUDate], [POHDRSchedulePUTime], [POHDRScheduleDelDate], [POHDRScheduleDelTime], [POHDRActPUDate], [POHDRActPUTime], [POHDRActDelDate], [POHDRActDelTime], [POHDROrigCompNumber], [POHDROrigAddress1], [POHDROrigAddress2], [POHDROrigAddress3], [POHDROrigCountry], [POHDROrigContactPhone], [POHDROrigContactPhoneExt], [POHDROrigContactFax], [POHDRDestCompNumber], [POHDRDestAddress1], [POHDRDestAddress2], [POHDRDestAddress3], [POHDRDestCountry], [POHDRDestContactPhone], [POHDRDestContactPhoneExt], [POHDRDestContactFax], [POHDRPalletExchange], [POHDRPalletType], [POHDRComments], [POHDRCommentsConfidential], [POHDRInbound], [POHDRDefaultRouteSequence], [POHDRRouteGuideNumber], [POHDRCompLegalEntity], [POHDRCompAlphaCode], [POHDRModeTypeControl], [POHDRMustLeaveByDateTime], [POHDRUser1], [POHDRUser2], [POHDRUser3], [POHDRUser4], [POHDRAPGLNumber]) {0}", vbCrLf))
        sbQry.Append(String.Format("VALUES  {0}", vbCrLf))
        sbQry.Append(String.Format("(@BookCarrOrderNumber,  N'31 Create A Pack', @POHDRPOdate, @POHDRShipdate, NULL, NULL, @CreateUser, @CreateDate, @CreateUser, NULL, NULL, 29000, 2900, 29, NULL, 0, @DefaultCustomer, N'Vegetable Juices, Inc.', 0, @POHDRReqDate, @BookCarrOrderNumber, NULL, 0, 0, 1, N'D', N'No Pro', N'Vegetable Juices, Inc.', N'Bedford Park', N'IL', N'60638', N'Create A Pack Foods', N'Ixonia', N'WI', N'53036', NULL, NULL, NULL, 28, NULL, NULL, 2, N'0', 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, 0, 0, NULL, NULL, NULL, 3, NULL, NULL, NULL, NULL, NULL, NULL){0}", vbCrLf))
        sbQry.Append(String.Format("set  @BookCarrOrderNumberSeed  = @BookCarrOrderNumberSeed + 1 {0}", vbCrLf))
        sbQry.Append(String.Format("Set @BookCarrOrderNumber  = @OrderNumberPrefix + Cast(@BookCarrOrderNumberSeed as nvarchar(10)) {0}", vbCrLf))

        sbQry.Append(String.Format("INSERT INTO [dbo].[POHdr] ([POHDRnumber], [POHDRvendor], [POHDRPOdate], [POHDRShipdate], [POHDRBuyer], [POHDRFrt], [POHDRCreateUser], [POHDRCreateDate], [POHDRModUser], [POHDRTotalFrt], [POHDRTotalCost], [POHDRWgt], [POHDRCube], [POHDRQty], [POHDRLines], [POHDRConfirm], [POHDRDefaultCustomer], [POHDRDefaultCustomerName], [POHDRDefaultCarrier], [POHDRReqDate], [POHDROrderNumber], [POHDRShipInstructions], [POHDRCooler], [POHDRFrozen], [POHDRDry], [POHDRTemp], [POHDRModVerify], [POHDROrigName], [POHDROrigCity], [POHDROrigState], [POHDROrigZip], [POHDRDestName], [POHDRDestCity], [POHDRDestState], [POHDRDestZip], [POHDRCarType], [POHDRShipVia], [POHDRShipViaType], [POHDRPallets], [POHDROtherCost], [POHDRStatusFlag], [POHDRSortOrder], [POHDRPRONumber], [POHDRHoldLoad], [POHDROrderSequence], [POHDRChepGLID], [POHDRCarrierEquipmentCodes], [POHDRCarrierTypeCode], [POHDRPalletPositions], [POHDRSchedulePUDate], [POHDRSchedulePUTime], [POHDRScheduleDelDate], [POHDRScheduleDelTime], [POHDRActPUDate], [POHDRActPUTime], [POHDRActDelDate], [POHDRActDelTime], [POHDROrigCompNumber], [POHDROrigAddress1], [POHDROrigAddress2], [POHDROrigAddress3], [POHDROrigCountry], [POHDROrigContactPhone], [POHDROrigContactPhoneExt], [POHDROrigContactFax], [POHDRDestCompNumber], [POHDRDestAddress1], [POHDRDestAddress2], [POHDRDestAddress3], [POHDRDestCountry], [POHDRDestContactPhone], [POHDRDestContactPhoneExt], [POHDRDestContactFax], [POHDRPalletExchange], [POHDRPalletType], [POHDRComments], [POHDRCommentsConfidential], [POHDRInbound], [POHDRDefaultRouteSequence], [POHDRRouteGuideNumber], [POHDRCompLegalEntity], [POHDRCompAlphaCode], [POHDRModeTypeControl], [POHDRMustLeaveByDateTime], [POHDRUser1], [POHDRUser2], [POHDRUser3], [POHDRUser4], [POHDRAPGLNumber]) {0}", vbCrLf))
        sbQry.Append(String.Format("VALUES  {0}", vbCrLf))
        sbQry.Append(String.Format("(@BookCarrOrderNumber,  N'31 Create A Pack',@POHDRPOdate, @POHDRShipdate, NULL, NULL, @CreateUser, @CreateDate, @CreateUser, NULL, NULL, 31000, 3100, 31, NULL, 0, @DefaultCustomer, N'Vegetable Juices, Inc.', 0, @POHDRReqDate, @BookCarrOrderNumber, NULL, 0, 0, 1, N'D', N'No Pro', N'Vegetable Juices, Inc.', N'Bedford Park', N'IL', N'60638', N'Create A Pack Foods', N'Ixonia', N'WI', N'53036', NULL, NULL, NULL, 28, NULL, NULL, 2, N'0', 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, 0, 0, NULL, NULL, NULL, 3, NULL, NULL, NULL, NULL, NULL, NULL){0}", vbCrLf))
        sbQry.Append(String.Format("set  @BookCarrOrderNumberSeed  = @BookCarrOrderNumberSeed + 1 {0}", vbCrLf))
        sbQry.Append(String.Format("Set @BookCarrOrderNumber  = @OrderNumberPrefix + Cast(@BookCarrOrderNumberSeed as nvarchar(10)) {0}", vbCrLf))

        sbQry.Append(String.Format("INSERT INTO [dbo].[POHdr] ([POHDRnumber], [POHDRvendor], [POHDRPOdate], [POHDRShipdate], [POHDRBuyer], [POHDRFrt], [POHDRCreateUser], [POHDRCreateDate], [POHDRModUser], [POHDRTotalFrt], [POHDRTotalCost], [POHDRWgt], [POHDRCube], [POHDRQty], [POHDRLines], [POHDRConfirm], [POHDRDefaultCustomer], [POHDRDefaultCustomerName], [POHDRDefaultCarrier], [POHDRReqDate], [POHDROrderNumber], [POHDRShipInstructions], [POHDRCooler], [POHDRFrozen], [POHDRDry], [POHDRTemp], [POHDRModVerify], [POHDROrigName], [POHDROrigCity], [POHDROrigState], [POHDROrigZip], [POHDRDestName], [POHDRDestCity], [POHDRDestState], [POHDRDestZip], [POHDRCarType], [POHDRShipVia], [POHDRShipViaType], [POHDRPallets], [POHDROtherCost], [POHDRStatusFlag], [POHDRSortOrder], [POHDRPRONumber], [POHDRHoldLoad], [POHDROrderSequence], [POHDRChepGLID], [POHDRCarrierEquipmentCodes], [POHDRCarrierTypeCode], [POHDRPalletPositions], [POHDRSchedulePUDate], [POHDRSchedulePUTime], [POHDRScheduleDelDate], [POHDRScheduleDelTime], [POHDRActPUDate], [POHDRActPUTime], [POHDRActDelDate], [POHDRActDelTime], [POHDROrigCompNumber], [POHDROrigAddress1], [POHDROrigAddress2], [POHDROrigAddress3], [POHDROrigCountry], [POHDROrigContactPhone], [POHDROrigContactPhoneExt], [POHDROrigContactFax], [POHDRDestCompNumber], [POHDRDestAddress1], [POHDRDestAddress2], [POHDRDestAddress3], [POHDRDestCountry], [POHDRDestContactPhone], [POHDRDestContactPhoneExt], [POHDRDestContactFax], [POHDRPalletExchange], [POHDRPalletType], [POHDRComments], [POHDRCommentsConfidential], [POHDRInbound], [POHDRDefaultRouteSequence], [POHDRRouteGuideNumber], [POHDRCompLegalEntity], [POHDRCompAlphaCode], [POHDRModeTypeControl], [POHDRMustLeaveByDateTime], [POHDRUser1], [POHDRUser2], [POHDRUser3], [POHDRUser4], [POHDRAPGLNumber]) {0}", vbCrLf))
        sbQry.Append(String.Format("VALUES  {0}", vbCrLf))
        sbQry.Append(String.Format("(@BookCarrOrderNumber,  N'31 Create A Pack', @POHDRPOdate, @POHDRShipdate, NULL, NULL, @CreateUser, @CreateDate, @CreateUser, NULL, NULL, 33000, 3300, 33, NULL, 0, @DefaultCustomer, N'Vegetable Juices, Inc.', 0, @POHDRReqDate, @BookCarrOrderNumber, NULL, 0, 0, 1, N'D', N'No Pro', N'Vegetable Juices, Inc.', N'Bedford Park', N'IL', N'60638', N'Create A Pack Foods', N'Ixonia', N'WI', N'53036', NULL, NULL, NULL, 28, NULL, NULL, 2, N'0', 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, 0, 0, NULL, NULL, NULL, 3, NULL, NULL, NULL, NULL, NULL, NULL){0}", vbCrLf))
        sbQry.Append(String.Format("set  @BookCarrOrderNumberSeed  = @BookCarrOrderNumberSeed + 1 {0}", vbCrLf))
        sbQry.Append(String.Format("Set @BookCarrOrderNumber  = @OrderNumberPrefix + Cast(@BookCarrOrderNumberSeed as nvarchar(10)) {0}", vbCrLf))

        sbQry.Append(String.Format("INSERT INTO [dbo].[POHdr] ([POHDRnumber], [POHDRvendor], [POHDRPOdate], [POHDRShipdate], [POHDRBuyer], [POHDRFrt], [POHDRCreateUser], [POHDRCreateDate], [POHDRModUser], [POHDRTotalFrt], [POHDRTotalCost], [POHDRWgt], [POHDRCube], [POHDRQty], [POHDRLines], [POHDRConfirm], [POHDRDefaultCustomer], [POHDRDefaultCustomerName], [POHDRDefaultCarrier], [POHDRReqDate], [POHDROrderNumber], [POHDRShipInstructions], [POHDRCooler], [POHDRFrozen], [POHDRDry], [POHDRTemp], [POHDRModVerify], [POHDROrigName], [POHDROrigCity], [POHDROrigState], [POHDROrigZip], [POHDRDestName], [POHDRDestCity], [POHDRDestState], [POHDRDestZip], [POHDRCarType], [POHDRShipVia], [POHDRShipViaType], [POHDRPallets], [POHDROtherCost], [POHDRStatusFlag], [POHDRSortOrder], [POHDRPRONumber], [POHDRHoldLoad], [POHDROrderSequence], [POHDRChepGLID], [POHDRCarrierEquipmentCodes], [POHDRCarrierTypeCode], [POHDRPalletPositions], [POHDRSchedulePUDate], [POHDRSchedulePUTime], [POHDRScheduleDelDate], [POHDRScheduleDelTime], [POHDRActPUDate], [POHDRActPUTime], [POHDRActDelDate], [POHDRActDelTime], [POHDROrigCompNumber], [POHDROrigAddress1], [POHDROrigAddress2], [POHDROrigAddress3], [POHDROrigCountry], [POHDROrigContactPhone], [POHDROrigContactPhoneExt], [POHDROrigContactFax], [POHDRDestCompNumber], [POHDRDestAddress1], [POHDRDestAddress2], [POHDRDestAddress3], [POHDRDestCountry], [POHDRDestContactPhone], [POHDRDestContactPhoneExt], [POHDRDestContactFax], [POHDRPalletExchange], [POHDRPalletType], [POHDRComments], [POHDRCommentsConfidential], [POHDRInbound], [POHDRDefaultRouteSequence], [POHDRRouteGuideNumber], [POHDRCompLegalEntity], [POHDRCompAlphaCode], [POHDRModeTypeControl], [POHDRMustLeaveByDateTime], [POHDRUser1], [POHDRUser2], [POHDRUser3], [POHDRUser4], [POHDRAPGLNumber]) {0}", vbCrLf))
        sbQry.Append(String.Format("VALUES  {0}", vbCrLf))
        sbQry.Append(String.Format("(@BookCarrOrderNumber,  N'31 Create A Pack', @POHDRPOdate, @POHDRShipdate, NULL, NULL, @CreateUser, @CreateDate, @CreateUser, NULL, NULL, 34000, 3400, 34, NULL, 0, @DefaultCustomer, N'Vegetable Juices, Inc.', 0, @POHDRReqDate, @BookCarrOrderNumber, NULL, 0, 0, 1, N'D', N'No Pro', N'Vegetable Juices, Inc.', N'Bedford Park', N'IL', N'60638', N'Create A Pack Foods', N'Ixonia', N'WI', N'53036', NULL, NULL, NULL, 28, NULL, NULL, 2, N'0', 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, 0, 0, NULL, NULL, NULL, 3, NULL, NULL, NULL, NULL, NULL, NULL){0}", vbCrLf))
        sbQry.Append(String.Format("set  @BookCarrOrderNumberSeed  = @BookCarrOrderNumberSeed + 1 {0}", vbCrLf))
        sbQry.Append(String.Format("Set @BookCarrOrderNumber  = @OrderNumberPrefix + Cast(@BookCarrOrderNumberSeed as nvarchar(10)) {0}", vbCrLf))

        sbQry.Append(String.Format("INSERT INTO [dbo].[POHdr] ([POHDRnumber], [POHDRvendor], [POHDRPOdate], [POHDRShipdate], [POHDRBuyer], [POHDRFrt], [POHDRCreateUser], [POHDRCreateDate], [POHDRModUser], [POHDRTotalFrt], [POHDRTotalCost], [POHDRWgt], [POHDRCube], [POHDRQty], [POHDRLines], [POHDRConfirm], [POHDRDefaultCustomer], [POHDRDefaultCustomerName], [POHDRDefaultCarrier], [POHDRReqDate], [POHDROrderNumber], [POHDRShipInstructions], [POHDRCooler], [POHDRFrozen], [POHDRDry], [POHDRTemp], [POHDRModVerify], [POHDROrigName], [POHDROrigCity], [POHDROrigState], [POHDROrigZip], [POHDRDestName], [POHDRDestCity], [POHDRDestState], [POHDRDestZip], [POHDRCarType], [POHDRShipVia], [POHDRShipViaType], [POHDRPallets], [POHDROtherCost], [POHDRStatusFlag], [POHDRSortOrder], [POHDRPRONumber], [POHDRHoldLoad], [POHDROrderSequence], [POHDRChepGLID], [POHDRCarrierEquipmentCodes], [POHDRCarrierTypeCode], [POHDRPalletPositions], [POHDRSchedulePUDate], [POHDRSchedulePUTime], [POHDRScheduleDelDate], [POHDRScheduleDelTime], [POHDRActPUDate], [POHDRActPUTime], [POHDRActDelDate], [POHDRActDelTime], [POHDROrigCompNumber], [POHDROrigAddress1], [POHDROrigAddress2], [POHDROrigAddress3], [POHDROrigCountry], [POHDROrigContactPhone], [POHDROrigContactPhoneExt], [POHDROrigContactFax], [POHDRDestCompNumber], [POHDRDestAddress1], [POHDRDestAddress2], [POHDRDestAddress3], [POHDRDestCountry], [POHDRDestContactPhone], [POHDRDestContactPhoneExt], [POHDRDestContactFax], [POHDRPalletExchange], [POHDRPalletType], [POHDRComments], [POHDRCommentsConfidential], [POHDRInbound], [POHDRDefaultRouteSequence], [POHDRRouteGuideNumber], [POHDRCompLegalEntity], [POHDRCompAlphaCode], [POHDRModeTypeControl], [POHDRMustLeaveByDateTime], [POHDRUser1], [POHDRUser2], [POHDRUser3], [POHDRUser4], [POHDRAPGLNumber]) {0}", vbCrLf))
        sbQry.Append(String.Format("VALUES  {0}", vbCrLf))
        sbQry.Append(String.Format("(@BookCarrOrderNumber,  N'31 Create A Pack', @POHDRPOdate, @POHDRShipdate, NULL, NULL, @CreateUser, @CreateDate, @CreateUser, NULL, NULL, 35000, 3500, 35, NULL, 0, @DefaultCustomer, N'Vegetable Juices, Inc.', 0, @POHDRReqDate, @BookCarrOrderNumber, NULL, 0, 0, 1, N'D', N'No Pro', N'Vegetable Juices, Inc.', N'Bedford Park', N'IL', N'60638', N'Create A Pack Foods', N'Ixonia', N'WI', N'53036', NULL, NULL, NULL, 28, NULL, NULL, 2, N'0', 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, 0, 0, NULL, NULL, NULL, 3, NULL, NULL, NULL, NULL, NULL, NULL){0}", vbCrLf))
        sbQry.Append(String.Format("set  @BookCarrOrderNumberSeed  = @BookCarrOrderNumberSeed + 1 {0}", vbCrLf))
        sbQry.Append(String.Format("Set @BookCarrOrderNumber  = @OrderNumberPrefix + Cast(@BookCarrOrderNumberSeed as nvarchar(10)) {0}", vbCrLf))

        sbQry.Append(String.Format("INSERT INTO [dbo].[POHdr] ([POHDRnumber], [POHDRvendor], [POHDRPOdate], [POHDRShipdate], [POHDRBuyer], [POHDRFrt], [POHDRCreateUser], [POHDRCreateDate], [POHDRModUser], [POHDRTotalFrt], [POHDRTotalCost], [POHDRWgt], [POHDRCube], [POHDRQty], [POHDRLines], [POHDRConfirm], [POHDRDefaultCustomer], [POHDRDefaultCustomerName], [POHDRDefaultCarrier], [POHDRReqDate], [POHDROrderNumber], [POHDRShipInstructions], [POHDRCooler], [POHDRFrozen], [POHDRDry], [POHDRTemp], [POHDRModVerify], [POHDROrigName], [POHDROrigCity], [POHDROrigState], [POHDROrigZip], [POHDRDestName], [POHDRDestCity], [POHDRDestState], [POHDRDestZip], [POHDRCarType], [POHDRShipVia], [POHDRShipViaType], [POHDRPallets], [POHDROtherCost], [POHDRStatusFlag], [POHDRSortOrder], [POHDRPRONumber], [POHDRHoldLoad], [POHDROrderSequence], [POHDRChepGLID], [POHDRCarrierEquipmentCodes], [POHDRCarrierTypeCode], [POHDRPalletPositions], [POHDRSchedulePUDate], [POHDRSchedulePUTime], [POHDRScheduleDelDate], [POHDRScheduleDelTime], [POHDRActPUDate], [POHDRActPUTime], [POHDRActDelDate], [POHDRActDelTime], [POHDROrigCompNumber], [POHDROrigAddress1], [POHDROrigAddress2], [POHDROrigAddress3], [POHDROrigCountry], [POHDROrigContactPhone], [POHDROrigContactPhoneExt], [POHDROrigContactFax], [POHDRDestCompNumber], [POHDRDestAddress1], [POHDRDestAddress2], [POHDRDestAddress3], [POHDRDestCountry], [POHDRDestContactPhone], [POHDRDestContactPhoneExt], [POHDRDestContactFax], [POHDRPalletExchange], [POHDRPalletType], [POHDRComments], [POHDRCommentsConfidential], [POHDRInbound], [POHDRDefaultRouteSequence], [POHDRRouteGuideNumber], [POHDRCompLegalEntity], [POHDRCompAlphaCode], [POHDRModeTypeControl], [POHDRMustLeaveByDateTime], [POHDRUser1], [POHDRUser2], [POHDRUser3], [POHDRUser4], [POHDRAPGLNumber]) {0}", vbCrLf))
        sbQry.Append(String.Format("VALUES  {0}", vbCrLf))
        sbQry.Append(String.Format("(@BookCarrOrderNumber,  N'31 Create A Pack', @POHDRPOdate, @POHDRShipdate, NULL, NULL, @CreateUser, @CreateDate, @CreateUser, NULL, NULL, 36000, 3500, 36, NULL, 0, @DefaultCustomer, N'Vegetable Juices, Inc.', 0, @POHDRReqDate, @BookCarrOrderNumber, NULL, 0, 0, 1, N'D', N'No Pro', N'Vegetable Juices, Inc.', N'Bedford Park', N'IL', N'60638', N'Create A Pack Foods', N'Ixonia', N'WI', N'53036', NULL, NULL, NULL, 28, NULL, NULL, 2, N'0', 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, 0, 0, NULL, NULL, NULL, 3, NULL, NULL, NULL, NULL, NULL, NULL){0}", vbCrLf))
        sbQry.Append(String.Format("set  @BookCarrOrderNumberSeed  = @BookCarrOrderNumberSeed + 1 {0}", vbCrLf))
        sbQry.Append(String.Format("Set @BookCarrOrderNumber  = @OrderNumberPrefix + Cast(@BookCarrOrderNumberSeed as nvarchar(10)) {0}", vbCrLf))

        sbQry.Append(String.Format("INSERT INTO [dbo].[POHdr] ([POHDRnumber], [POHDRvendor], [POHDRPOdate], [POHDRShipdate], [POHDRBuyer], [POHDRFrt], [POHDRCreateUser], [POHDRCreateDate], [POHDRModUser], [POHDRTotalFrt], [POHDRTotalCost], [POHDRWgt], [POHDRCube], [POHDRQty], [POHDRLines], [POHDRConfirm], [POHDRDefaultCustomer], [POHDRDefaultCustomerName], [POHDRDefaultCarrier], [POHDRReqDate], [POHDROrderNumber], [POHDRShipInstructions], [POHDRCooler], [POHDRFrozen], [POHDRDry], [POHDRTemp], [POHDRModVerify], [POHDROrigName], [POHDROrigCity], [POHDROrigState], [POHDROrigZip], [POHDRDestName], [POHDRDestCity], [POHDRDestState], [POHDRDestZip], [POHDRCarType], [POHDRShipVia], [POHDRShipViaType], [POHDRPallets], [POHDROtherCost], [POHDRStatusFlag], [POHDRSortOrder], [POHDRPRONumber], [POHDRHoldLoad], [POHDROrderSequence], [POHDRChepGLID], [POHDRCarrierEquipmentCodes], [POHDRCarrierTypeCode], [POHDRPalletPositions], [POHDRSchedulePUDate], [POHDRSchedulePUTime], [POHDRScheduleDelDate], [POHDRScheduleDelTime], [POHDRActPUDate], [POHDRActPUTime], [POHDRActDelDate], [POHDRActDelTime], [POHDROrigCompNumber], [POHDROrigAddress1], [POHDROrigAddress2], [POHDROrigAddress3], [POHDROrigCountry], [POHDROrigContactPhone], [POHDROrigContactPhoneExt], [POHDROrigContactFax], [POHDRDestCompNumber], [POHDRDestAddress1], [POHDRDestAddress2], [POHDRDestAddress3], [POHDRDestCountry], [POHDRDestContactPhone], [POHDRDestContactPhoneExt], [POHDRDestContactFax], [POHDRPalletExchange], [POHDRPalletType], [POHDRComments], [POHDRCommentsConfidential], [POHDRInbound], [POHDRDefaultRouteSequence], [POHDRRouteGuideNumber], [POHDRCompLegalEntity], [POHDRCompAlphaCode], [POHDRModeTypeControl], [POHDRMustLeaveByDateTime], [POHDRUser1], [POHDRUser2], [POHDRUser3], [POHDRUser4], [POHDRAPGLNumber]) {0}", vbCrLf))
        sbQry.Append(String.Format("VALUES  {0}", vbCrLf))
        sbQry.Append(String.Format("(@BookCarrOrderNumber,  N'31 Create A Pack', @POHDRPOdate, @POHDRShipdate, NULL, NULL, @CreateUser, @CreateDate, @CreateUser, NULL, NULL, 38000, 3500, 38, NULL, 0, @DefaultCustomer, N'Vegetable Juices, Inc.', 0, @POHDRReqDate, @BookCarrOrderNumber, NULL, 0, 0, 1, N'D', N'No Pro', N'Vegetable Juices, Inc.', N'Bedford Park', N'IL', N'60638', N'Create A Pack Foods', N'Ixonia', N'WI', N'53036', NULL, NULL, NULL, 28, NULL, NULL, 2, N'0', 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, 0, 0, NULL, NULL, NULL, 3, NULL, NULL, NULL, NULL, NULL, NULL){0}", vbCrLf))
        sbQry.Append(String.Format("set  @BookCarrOrderNumberSeed  = @BookCarrOrderNumberSeed + 1 {0}", vbCrLf))
        sbQry.Append(String.Format("Set @BookCarrOrderNumber  = @OrderNumberPrefix + Cast(@BookCarrOrderNumberSeed as nvarchar(10)) {0}", vbCrLf))

        sbQry.Append(String.Format("INSERT INTO [dbo].[POHdr] ([POHDRnumber], [POHDRvendor], [POHDRPOdate], [POHDRShipdate], [POHDRBuyer], [POHDRFrt], [POHDRCreateUser], [POHDRCreateDate], [POHDRModUser], [POHDRTotalFrt], [POHDRTotalCost], [POHDRWgt], [POHDRCube], [POHDRQty], [POHDRLines], [POHDRConfirm], [POHDRDefaultCustomer], [POHDRDefaultCustomerName], [POHDRDefaultCarrier], [POHDRReqDate], [POHDROrderNumber], [POHDRShipInstructions], [POHDRCooler], [POHDRFrozen], [POHDRDry], [POHDRTemp], [POHDRModVerify], [POHDROrigName], [POHDROrigCity], [POHDROrigState], [POHDROrigZip], [POHDRDestName], [POHDRDestCity], [POHDRDestState], [POHDRDestZip], [POHDRCarType], [POHDRShipVia], [POHDRShipViaType], [POHDRPallets], [POHDROtherCost], [POHDRStatusFlag], [POHDRSortOrder], [POHDRPRONumber], [POHDRHoldLoad], [POHDROrderSequence], [POHDRChepGLID], [POHDRCarrierEquipmentCodes], [POHDRCarrierTypeCode], [POHDRPalletPositions], [POHDRSchedulePUDate], [POHDRSchedulePUTime], [POHDRScheduleDelDate], [POHDRScheduleDelTime], [POHDRActPUDate], [POHDRActPUTime], [POHDRActDelDate], [POHDRActDelTime], [POHDROrigCompNumber], [POHDROrigAddress1], [POHDROrigAddress2], [POHDROrigAddress3], [POHDROrigCountry], [POHDROrigContactPhone], [POHDROrigContactPhoneExt], [POHDROrigContactFax], [POHDRDestCompNumber], [POHDRDestAddress1], [POHDRDestAddress2], [POHDRDestAddress3], [POHDRDestCountry], [POHDRDestContactPhone], [POHDRDestContactPhoneExt], [POHDRDestContactFax], [POHDRPalletExchange], [POHDRPalletType], [POHDRComments], [POHDRCommentsConfidential], [POHDRInbound], [POHDRDefaultRouteSequence], [POHDRRouteGuideNumber], [POHDRCompLegalEntity], [POHDRCompAlphaCode], [POHDRModeTypeControl], [POHDRMustLeaveByDateTime], [POHDRUser1], [POHDRUser2], [POHDRUser3], [POHDRUser4], [POHDRAPGLNumber]) {0}", vbCrLf))
        sbQry.Append(String.Format("VALUES  {0}", vbCrLf))
        sbQry.Append(String.Format("(@BookCarrOrderNumber,  N'31 Create A Pack', @POHDRPOdate, @POHDRShipdate, NULL, NULL, @CreateUser, @CreateDate, @CreateUser, NULL, NULL, 39000, 3500, 39, NULL, 0, @DefaultCustomer, N'Vegetable Juices, Inc.', 0, @POHDRReqDate,  @BookCarrOrderNumber, NULL, 0, 0, 1, N'D', N'No Pro', N'Vegetable Juices, Inc.', N'Bedford Park', N'IL', N'60638', N'Create A Pack Foods', N'Ixonia', N'WI', N'53036', NULL, NULL, NULL, 28, NULL, NULL, 2, N'0', 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, 0, 0, NULL, NULL, NULL, 3, NULL, NULL, NULL, NULL, NULL, NULL){0}", vbCrLf))
        oBook.executeSQL(sbQry.ToString())

    End Sub


End Class
