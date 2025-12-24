Imports System
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports DAL = NGL.FreightMaster.Data
Imports NGL.FreightMaster.Integration

<TestClass()> Public Class PickListObjectWSTest
    Inherits TestBase


    <TestMethod()>
    Public Sub GetPickListData85Test()
        Dim DBServer = "NGLSQL03P"
        Dim Database = "NGLMASD365"

        Dim AuthorizationCode As String = "WSPROD"
        Dim MaxRetry As Integer = 100
        Dim RetryMinutes As Integer = 1
        Dim CompLegalEntity As String = "USMF"
        Dim MaxRowsReturned As Integer = 100
        Dim AutoConfirmation As Boolean = False
        Dim RetVal As Integer = 0
        Dim ReturnMessage As String = ""



        If CompLegalEntity = "" Then CompLegalEntity = Nothing

        Dim picklist As New NGL.FreightMaster.Integration.clsPickList
        Dim Headers() As NGL.FreightMaster.Integration.clsPickListObject85
        Dim Details() As NGL.FreightMaster.Integration.clsPickDetailObject85
        Dim Fees() As NGL.FreightMaster.Integration.clsPickListFeeObject85

        Dim strConnection = "Server=" & DBServer & ";User ID=nglweb;Password=5529;Database=" & Database


        'set the default value to false
        RetVal = NGL.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        ReturnMessage = ""
        Dim sSource As String = "DTMSERPIntegration.GetPickListData85"
        Dim sDataType As String = "Pick List"
        Dim strCriteria As String = ""
        strCriteria &= " MaxRetry = " & MaxRetry.ToString
        strCriteria &= " RetryMinutes = " & RetryMinutes.ToString
        strCriteria &= " MaxRowsReturned = " & MaxRowsReturned.ToString
        strCriteria &= " AutoConfirmation = " & AutoConfirmation.ToString
        If Not String.IsNullOrEmpty(CompLegalEntity) Then strCriteria &= " CompLegalEntity = " & CompLegalEntity

        Try

            picklist.MaxRowsReturned = MaxRowsReturned
            picklist.AutoConfirmation = AutoConfirmation
            RetVal = picklist.readObjectData85(Headers, strConnection, MaxRetry, RetryMinutes, CompLegalEntity, Fees, Details)
            ReturnMessage = picklist.LastError

        Catch ex As ApplicationException
            Assert.Fail("Application Exception For " & sSource & ": {0} ", ex.Message)
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & sSource & ": {0} ", ex.Message)
        Finally

        End Try



    End Sub


    <TestMethod()> Public Sub readObjectData70Test()
        testParameters.DBServer = "NGLRDP06D"
        testParameters.Database = "SHIELDSMASUnitTest"

        'Always overwrite the web.config when running unit test
        FileCopy("Web.config", "D:\HTTP\WSUnitTest70\Web.config")

        Dim intResult As Integer = 0
        Dim strLastError As String = ""

        Dim oBook As New NGLBookObjectWebService.BookObject
        oBook.Url = WSUrl & "BookObject.ASMX"

        Dim oBookDB As New DAL.NGLBookData(testParameters)


        Dim BookProPrefix As String = "YAK"
        Dim BookProBase As String = "12345"
        Dim BookProNumber = BookProPrefix & BookProBase
        Dim BookConsPrefix As String = "TSTLVVWS27"
        'Dim OrderNumberBase As String = "1111"
        Dim BookCarrOrderNumber As String = "WS-UnitTEST2222"
        Dim dtOrder As Date = (Date.Now().AddDays(60))
        Dim dtPickUp As Date = dtOrder.AddDays(3)
        Dim dtEnRoute As Date = dtOrder.AddDays(4).AddHours(10)
        Dim dtDelivery As Date = dtOrder.AddDays(5)
        'Create a Test Order
        CreateTestOrder(oBookDB, BookProPrefix, BookProBase, BookConsPrefix, BookCarrOrderNumber, Date.Now().AddDays(60))



        'First create new record in the db and then try to get it back
        'using the web services

        'BookCarrOrderNumber()
        'BookConsPrefix()
        'CarrierNumber()
        'BookRevTotalCost()
        'LoadOrder()
        'BookDateLoad()
        'BookDateRequired()
        'BookLoadCom()
        'BookProNumber()
        'BookRouteFinalCode()
        'BookRouteFinalDate()
        'BookTotalCases()
        'BookTotalWgt()
        'BookTotalPL()
        'BookTotalCube()
        'BookTotalBFC()
        'BookStopNo()
        'CompName()
        'CompNumber()
        'BookTypeCode()
        'BookDateOrdered()
        'BookOrigName()
        'BookOrigAddress1()
        'BookOrigAddress2()
        'BookOrigAddress3()
        'BookOrigCity()
        'BookOrigState()
        'BookOrigCountry()
        'BookOrigZip()
        'BookDestName()
        'BookDestAddress1()
        'BookDestAddress2()
        'BookDestAddress3()
        'BookDestCity()
        'BookDestState()
        'BookDestCountry()
        'BookDestZip()
        'BookLoadPONumber()
        'CarrierName()
        'LaneNumber()
        'CommCodeDescription()
        'BookMilesFrom()
        'PLControl()
        'BookCommCompControl()
        'BookRevCommCost()
        'BookRevGrossRevenue()
        'BookFinCommStd()
        'BookDoNotInvoice()
        'BookOrderSequence()
        'CarrierEquipmentCodes()
        'BookCarrierTypeCode()
        'BookWarehouseNumber()
        'CompNatNumber()
        'BookTransType()
        'BookShipCarrierProNumber()
        'BookShipCarrierNumber()
        'LaneComments()
        'FuelSurCharge()
        'BookRevCarrierCost()
        'BookRevOtherCost()
        'BookRevNetCost()
        'BookRevFreightTax()
        'BookFinServiceFee()
        'BookRevLoadSavings()
        'TotalNonFuelFees()
        'BookPickNumber()
        'BookPickupStopNumber()
        'New Fields 70
        'PLExportRetry()
        'PLExportDate()
        'PLExported()
        'CarrierAlphaCode()
        'CarrierLegalEntity()
        'CompLegalEntity()
        'CompAlphaCode()
        'LaneLegalEntity()
        'BookOriginalLaneNumber()
        'BookAlternateAddressLaneNumber()
        'BookSHID()
        'BookOriginalLaneLegalEntity()
        'BookWhseAuthorizationNo()
        'BookShipCarrierName()
        'BookShipCarrierDetails()
        'BookRevNonTaxable()
        'BookFinAPGLNumber()

        '        BookCarrOrderNumber()
        '        ItemNumber()
        '        QtyOrdered()
        '        FreightCost()
        '        ItemCost()
        '        Weight()
        '        Cube()
        '        Pack()
        '        Size()
        '        Description()
        '        CustomerItemNumber()
        '        CustomerNumber()
        '        OrderSequence()
        '        Hazmat()
        '        Brand()
        '        CostCenter()
        '        LotNumber()
        '        LotExpirationDate()
        '        GTIN()
        '        BFC()
        '        CountryOfOrgin()
        '        CustomerPONumber()
        '        HST()
        '        BookProNumber()
        '        PalletType()
        '        PLControl()
        '        CompNatNumber()
        'New Fields 70
        '        BookRouteConsFlag()
        '        BookAlternateAddressLaneNumber()
        '        CompLegalEntity()
        '        CompAlphaCode()
        '        BookItemDiscount()
        '        BookItemLineHaul()
        '        BookItemTaxableFees()
        '        BookItemTaxes()
        '        BookItemNonTaxableFees()
        '        BookItemWeightBreak()
        '        BookItemRated49CFRCode()
        '        BookItemRatedIATACode()
        '        BookItemRatedDOTCode()
        '        BookItemRatedMarineCode()
        '        BookItemRatedNMFCClass()
        '        BookItemRatedNMFCSubClass()
        '        BookItemRatedFAKClass()
        '        HazmatTypeCode()
        '        Hazmat49CFRCode()
        '        IATACode()
        '        DOTCode()
        '        MarineCode()
        '        NMFCClass()
        '        FAKClass()
        '        LimitedQtyFlag()
        '        Pallets()
        '        Ties()
        '        Highs()
        '        QtyPalletPercentage()
        '        QtyLength()
        '        QtyWidth()
        '        QtyHeight()
        '        Stackable()
        '        LevelOfDensity()








    End Sub

    Private Sub CreateTestOrder(ByVal oBook As DAL.NGLBookData, _
                                ByVal BookProPrefix As String, _
                                ByVal BookProBase As String, _
                                ByVal BookConsPrefix As String, _
                                ByVal OrderNumber As String, _
                                ByVal OrderDate As Date)

        Dim dtOrderd = OrderDate
        Dim dtLoad As Date = OrderDate.AddDays(3)
        Dim dtRequired As Date = dtLoad.AddDays(5)
        Dim sbQry As New System.Text.StringBuilder()
        sbQry.Append(String.Format("Declare @BookProBase nvarchar(20) = '{0}' {1}", BookProBase, vbCrLf))
        sbQry.Append(String.Format("Declare @BookProPrefix nvarchar(3) = '{0}' {1}", BookProPrefix, vbCrLf))
        sbQry.Append(String.Format("Declare @BookConsPrefix nvarchar(20) = '{0}' {1}", BookConsPrefix, vbCrLf))
        sbQry.Append(String.Format("Declare @BookProNumber nvarchar(20) = @BookProPrefix + @BookProBase {0}", vbCrLf))
        sbQry.Append(String.Format("Declare @BookCarrOrderNumber nvarchar(20) = '{0}' {1}", OrderNumber, vbCrLf))
        sbQry.Append(String.Format("Delete from dbo.book where BookProNumber = @BookProNumber {0}", vbCrLf))

        sbQry.Append(String.Format("INSERT INTO [dbo].[Book] ([BookProNumber], [BookProBase], [BookConsPrefix], [BookCustCompControl], [BookCommCompControl], [BookODControl], [BookCarrierControl], [BookCarrierContact], [BookCarrierContactPhone], [BookOrigCompControl], [BookOrigName], [BookOrigAddress1], [BookOrigAddress2], [BookOrigAddress3], [BookOrigCity], [BookOrigState], [BookOrigCountry], [BookOrigZip], [BookOrigPhone], [BookOrigFax], [BookOriginStartHrs], [BookOriginStopHrs], [BookOriginApptReq], [BookDestCompControl], [BookDestName], [BookDestAddress1], [BookDestAddress2], [BookDestAddress3], [BookDestCity], [BookDestState], [BookDestCountry], [BookDestZip], [BookDestPhone], [BookDestFax], [BookDestStartHrs], [BookDestStopHrs], [BookDestApptReq], [BookDateOrdered], [BookDateLoad], [BookDateInvoice], [BookDateRequired], [BookDateDelivered], [BookTotalCases], [BookTotalWgt], [BookTotalPL], [BookTotalCube], [BookTotalPX], [BookTotalBFC], [BookTranCode], [BookPayCode], [BookTypeCode], [BookBOLCode], [BookStopNo], [BookModDate], [BookModUser], [BookCarrControl], [BookCarrFBNumber], [BookCarrOrderNumber], [BookCarrBLNumber], [BookCarrBookDate], [BookCarrBookTime], [BookCarrBookContact], [BookCarrScheduleDate], [BookCarrScheduleTime], [BookCarrActualDate], [BookCarrActualTime], [BookCarrActLoadComplete Date], [BookCarrActLoadCompleteTime], [BookCarrDockPUAssigment], [BookCarrPODate], [BookCarrPOTime], [BookCarrApptDate], [BookCarrApptTime], [BookCarrActDate], [BookCarrActTime], [BookCarrActUnloadCompDate], [BookCarrActUnloadCompTime], [BookCarrDockDelAssignment], [BookCarrVarDay], [BookCarrVarHrs], [BookCarrTrailerNo], [BookCarrSealNo], [BookCarrDriverNo], [BookCarrDriverName], [BookCarrRouteNo], [BookCarrTripNo], [BookFinControl], [BookFinARBookFrt], [BookFinARInvoiceDate], [BookFinARInvoiceAmt], [BookFinARPayDate], [BookFinARPayAmt], [BookFinARCheck], [BookFinARGLNumber], [BookFinARBalance], [BookFinARCurType], [BookFinAPBillNumber], [BookFinAPBillNoDate], [BookFinAPBillInvDate], [BookFinAPActWgt], [BookFinAPStdCost], [BookFinAPActCost], [BookFinAPPayDate], [BookFinAPPayAmt], [BookFinAPCheck], [BookFinAPGLNumber], [BookFinAPLastViewed], [BookFinAPCurType], [BookFinCommStd], [BookFinCommAct], [BookFinCommPayDate], [BookFinCommPayAmt], [BookFinCommtCheck], [BookFinCommCreditAmt], [BookFinCommCreditPayDate], [BookFinCommLoadCount], [BookFinCommGLNumber], [BookFinCheckClearedDate], [BookFinCheckClearedNumber], [BookFinCheckClearedAmt], [BookFinCheckClearedDesc], [BookFinCheckClearedAcct], [BookRevControl], [BookRevBilledBFC], [BookRevCarrierCost], [BookRevStopQty], [BookRevStopCost], [BookRevOtherCost], [BookRevTotalCost], [BookRevLoadSavings], [BookRevCommPercent], [BookRevCommCost], [BookRevGrossRevenue], [BookRevNegRevenue], [BookMilesFrom], [BookLaneCarrControl], [BookHoldLoad], [BookRouteFinalDate], [BookRouteFinalCode], [BookRouteFinalFlag], [BookWarehouseNumber], [BookComCode], [BookTransType], [BookRouteConsFlag], [BookWhseAuthorizationNo], [BookHotLoad], [BookFinAPActTax], [BookFinAPExportFlag], [BookFinARFreightTax], [BookRevFreightTax], [BookRevNetCost], [BookFinServiceFee], [BookFinAPExportDate], [BookFinAPExportRetry], [BookCarrierContControl], [BookHotLoadSent], [BookExportDocCreated], [BookDoNotInvoice], [BookCarrStartLoadingDate], [BookCarrStartLoadingTime], [BookCarrFinishLoadingDate], [BookCarrFinishLoadingTime], [BookCarrStartUnloadingDate], [BookCarrStartUnloadingTime], [BookCarrFinishUnloadingDate], [BookCarrFinishUnloadingTime], [BookOrderSequence], [BookChepGLID], [BookCarrierTypeCode], [BookPalletPositions], [BookShipCarrierProNumber], [BookShipCarrierProNumberRaw], [BookShipCarrierProControl], [BookShipCarrierName], [BookShipCarrierNumber], [BookAPAdjReasonControl], [BookDateRequested], [BookCarrierEquipmentCodes], [BookShippedDataExported], [BookLockAllCosts], [BookLockBFCCost], [BookRevNonTaxable], [BookPickupStopNumber], [BookOrigStopNumber], [BookDestStopNumber], [BookOrigMiles], [BookDestMiles], [BookOrigPCMCost], [BookDestPCMCost], [BookOrigPCMTime], [BookDestPCMTime], [BookOrigPCMTolls], [BookDestPCMTolls], [BookOrigPCMESTCHG], [BookDestPCMESTCHG], [BookPickNumber], [BookAMSPickupApptControl], [BookAMSDeliveryApptControl], [BookItemDetailDescription], [BookOrigStopControl], [BookDestStopControl], [BookRouteTypeCode], [BookAlternateAddressLaneControl], [BookAlternateAddressLaneNumber], [BookDefaultRouteSequence], [BookRouteGuideControl], [BookRouteGuideNumber], [BookCustomerApprovalTransmitted], [BookCustomerApprovalRecieved], [BookCarrTruckControl], [BookCarrTarControl], [BookCarrTarRevisionNumber], [BookCarrTarName], [BookCarrTarEquipControl], [BookCarrTarEquipName], [BookCarrTarEquipMatControl], [BookCarrTarEquipMatName], [BookCarrTarEquipMatDetControl], [BookCarrTarEquipMatDetID], [BookCarrTarEquipMatDetValue], [BookBookRevHistRevision], [BookModeTypeControl], [BookAllowInterlinePoints], [BookRevLaneBenchMiles], [BookRevLoadMiles], [BookUser1], [BookUser2], [BookUser3], [BookUser4], [BookRevDiscount], [BookRevLineHaul], [BookMustLeaveByDateTime], [BookMultiMode], [BookOriginalLaneControl], [BookLaneTranXControl], [BookLaneTranXDetControl], [BookSHID], [BookShipCarrierDetails], [BookExpDelDateTime], [BookOutOfRouteMiles], [BookSpotRateAllocationFormula], [BookSpotRateAutoCalcBFC], [BookSpotRateUseCarrierFuelAddendum], [BookSpotRateBFCAllocationFormula], [BookSpotRateTotalUnallocatedBFC], [BookSpotRateTotalUnallocatedLineHaul], [BookSpotRateUseFuelAddendum], [BookCreditHold], [BookBestDeficitCost], [BookBestDeficitWeight], [BookBestDeficitWeightBreak], [BookRatedWeightBreak], [BookWgtAdjCost], [BookWgtAdjWeight], [BookWgtAdjWeightBreak], [BookBilledLoadWeight], [BookMinAdjustedLoadWeight], [BookSummedClassWeight], [BookWgtRoundingVariance], [BookHeaviestClass], [BookAcutalHeaviestClassWeight], [BookRevDiscountRate], [BookRevDiscountMin]) VALUES (@BookProNumber, @BookProBase,@BookConsPrefix, 9623, 9623, 56600, 17242, N'TRUCKLOAD BENCHMARK', N'', 1, N'Yakima', N'1009 Rock Avenue', N'', N'', N'Yakima', N'WA', N'US', N'98902', N'', N'', NULL, NULL, 1, 4, N'Mooresville', N'207 Talbert Point Dr.', N'', N'', N'Mooresville', N'NC', N'US', N'28117', N'', N'', NULL, NULL, 1, N'{1}', N'{2}', NULL, N'{3}', NULL, 8, 8000, 8, 800, 0, CAST(0.0000 AS Money), N'PB', N'N', N'N/A', 0, 1, GetDate(), N'UnitTest', NULL, N'', @BookCarrOrderNumber, N'', NULL, NULL, N'', NULL, NULL, NULL, NULL, NULL, NULL, N'', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'', 0, 0, N'', N'', N'', N'', N'', N'', NULL, CAST(0.0000 AS Money), NULL, CAST(0.0000 AS Money), NULL, CAST(0.0000 AS Money), N'', N'', CAST(0.0000 AS Money), 1, N'', NULL, NULL, 0, CAST(472.8400 AS Money), CAST(0.0000 AS Money), NULL, CAST(0.0000 AS Money), N'', N'', NULL, 1, CAST(-472.8400 AS Money), CAST(0.0000 AS Money), NULL, CAST(0.0000 AS Money), N'', CAST(0.0000 AS Money), NULL, 0, N'', NULL, N'', CAST(0.0000 AS Money), N'', N'', NULL, CAST(0.0000 AS Money), CAST(242.9000 AS Money), 0, CAST(0.0000 AS Money), CAST(229.9400 AS Money), CAST(472.8400 AS Money), CAST(-472.8400 AS Money), 100, CAST(-472.8400 AS Money), CAST(0.0000 AS Money), 1, 138.8, 0, 1, NULL, N'', 0, N'', N'', N'0', 1, N'', 0, CAST(0.0000 AS Money), 0, CAST(0.0000 AS Money), CAST(0.0000 AS Money), CAST(472.8400 AS Money), CAST(0.0000 AS Money), NULL, 0, 21934, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, N'', N'', N'', N'', N'', NULL, N'', N'', 0, N'{3}', N'', 0, 0, 0, CAST(0.0000 AS Money), 0, 0, 0, 0, 0, 0, 0, NULL, NULL, CAST(0.0000 AS Money), CAST(0.0000 AS Money), 0, 0, 0, 0, 0, N'', 0, 0, 6, 0, N'', 0, 0, N'', 0, 0, 0, 880, 1, N'', 1419, N'', 104913, N'', 0, 1, CAST(1.7500 AS Decimal(18, 4)), 1, 3, 1, 138.8, 138.8, N'', N'', N'', N'', CAST(0.0000 AS Decimal(18, 4)), CAST(242.9000 AS Decimal(18, 4)), N'{3}', 0, 56600, 0, 0, @BookConsPrefix, NULL, N'{3}', 0, 0, 0, 0, 0, CAST(0.0000 AS Decimal(18, 4)), CAST(0.0000 AS Decimal(18, 4)), 0, 0, CAST(0.0000 AS Decimal(18, 4)), 0, 0, 0, CAST(0.0000 AS Decimal(18, 4)), 0, 0, 0, 0, 0, 0, NULL, 0, CAST(0.0000 AS Decimal(18, 4)), CAST(0.0000 AS Decimal(18, 4))) {0}", vbCrLf, dtOrderd.ToShortDateString(), dtLoad.ToShortDateString(), dtRequired.ToShortDateString()))
        sbQry.Append(String.Format("--Get the bookcontrol number back {0}", vbCrLf))
        sbQry.Append(String.Format("Declare @BookControl int = SCOPE_IDENTITY() {0}", vbCrLf))
        sbQry.Append(String.Format("INSERT INTO [dbo].[BookLoad] ([BookLoadBookControl], [BookLoadBuy], [BookLoadPONumber], [BookLoadVendor], [BookLoadCaseQty], [BookLoadWgt], [BookLoadCube], [BookLoadPL], [BookLoadPX], [BookLoadPType], [BookLoadCom], [BookLoadPUOrigin], [BookLoadBFC], [BookLoadTotCost], [BookLoadComments], [BookLoadStopSeq], [BookLoadModDate], [BookLoadModUser]) VALUES (@BookControl, NULL, N'WS-TEST2228', N'Yakima', 8, 8000, 800, 8, 0, N'N', N'D', N'Yakima', CAST(0.0000 AS Money), CAST(472.8400 AS Money), NULL, 1,getdate(), N'UnitTest') {0}", vbCrLf))
        sbQry.Append(String.Format("--Get the bookloadcontrol number back {0}", vbCrLf))
        sbQry.Append(String.Format("Declare @BookLoadControl int = SCOPE_IDENTITY()  {0}", vbCrLf))
        sbQry.Append(String.Format("INSERT INTO [dbo].[BookItem] ([BookItemBookLoadControl], [BookItemFixOffInvAllow], [BookItemFixFrtAllow], [BookItemItemNumber], [BookItemQtyOrdered], [BookItemFreightCost], [BookItemItemCost], [BookItemWeight], [BookItemCube], [BookItemPack], [BookItemSize], [BookItemDescription], [BookItemHazmat], [BookItemModDate], [BookItemModUser], [BookItemBrand], [BookItemCostCenter], [BookItemLotNumber], [BookItemLotExpirationDate], [BookItemGTIN], [BookCustItemNumber], [BookItemBFC], [BookItemCountryOfOrigin], [BookItemHST], [BookItemPalletTypeID], [BookItemHazmatTypeCode], [BookItem49CFRCode], [BookItemIATACode], [BookItemDOTCode], [BookItemMarineCode], [BookItemNMFCClass], [BookItemFAKClass], [BookItemLimitedQtyFlag], [BookItemPallets], [BookItemTies], [BookItemHighs], [BookItemQtyPalletPercentage], [BookItemQtyLength], [BookItemQtyWidth], [BookItemQtyHeight], [BookItemStackable], [BookItemLevelOfDensity], [BookItemDiscount], [BookItemLineHaul], [BookItemTaxableFees], [BookItemTaxes], [BookItemNonTaxableFees], [BookItemDeficitCostAdjustment], [BookItemDeficitWeightAdjustment], [BookItemWeightBreak], [BookItemDeficit49CFRCode], [BookItemDeficitIATACode], [BookItemDeficitDOTCode], [BookItemDeficitMarineCode], [BookItemDeficitNMFCClass], [BookItemDeficitFAKClass], [BookItemRated49CFRCode], [BookItemRatedIATACode], [BookItemRatedDOTCode], [BookItemRatedMarineCode], [BookItemRatedNMFCClass], [BookItemRatedFAKClass], [BookItemCarrTarEquipMatControl], [BookItemCarrTarEquipMatName], [BookItemCarrTarEquipMatDetID], [BookItemCarrTarEquipMatDetValue], [BookItemUser1], [BookItemUser2], [BookItemUser3], [BookItemUser4], [BookItemUnitOfMeasureControl], [BookItemRatedNMFCSubClass], [BookItemCommCode]) VALUES ( @BookLoadControl, NULL, NULL, N'1234', 8, CAST(472.8400 AS Money), CAST(100.0000 AS Money), 8000, 800, NULL, NULL, N'Test Item', NULL, getDate(), N'UnitTest',  NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, CAST(0.0000 AS Decimal(18, 4)), CAST(0.0000 AS Decimal(18, 4)), CAST(0.0000 AS Decimal(18, 4)), CAST(0.0000 AS Decimal(18, 4)), CAST(0.0000 AS Decimal(18, 4)), CAST(0.0000 AS Decimal(18, 4)), CAST(0.0000 AS Decimal(18, 4)), CAST(0.0000 AS Decimal(18, 4)), NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, 0, CAST(0.0000 AS Decimal(18, 4)), N'User 1', N'User 2', N'User 3', N'User 4', 0, NULL, N'3') {0}", vbCrLf))
        oBook.executeSQL(sbQry.ToString())
        'System.Diagnostics.Debug.WriteLine("*********************************************************************************************")
        'System.Diagnostics.Debug.Write(sbQry.ToString())
        'System.Diagnostics.Debug.WriteLine("*********************************************************************************************")

    End Sub

End Class