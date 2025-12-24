Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports System
Imports NGL.FM.CarTar
Imports DAL = NGL.FreightMaster.Data
Imports DTO = NGL.FreightMaster.Data.DataTransferObjects
Imports BLL = NGL.FM.BLL

<TestClass()> Public Class TariffUnitTest
    Inherits TestBase

#Region "Test Importing Tariff Data"

    <TestMethod> _
    Public Sub ImportRatesCSVTest()

        testParameters.DBServer = "NGLRDP07D"
        testParameters.Database = "NGLMASPROD"
        Dim fileName As String = "scnn_tl_rates_ngl.csv"
        Dim Ratename As String = "Distance"
        Dim bll As New BLL.NGLCarrTarBLL(testParameters)
        bll.importRatesCSV(fileName, False, "ngl\paul molenda", "ImportRatesCSVTest", Ratename)
    End Sub

#End Region

#Region "Test Cloning"


    ''' <summary>
    ''' Test the stored procedure that clones the tariff.
    ''' </summary> 
    ''' <remarks></remarks>
    <TestCategory("Nightly")>
    <TestMethod> _
    Public Sub CloneTariffTruckLoadTest()
        testParameters.DBServer = "NGLRDP06D"
        testParameters.Database = "SHIELDSMASUnitTest"

        Dim contractID As String = "TestCloning"
        Dim expectedRevisionNumber = 0
        Dim actualRevisionNumber = 0
        Dim firstContract As DTO.CarrTarContract
        Dim clonedContract As DTO.CarrTarContract
        Try
            firstContract = TestGetContractTestClone(contractID)
        Catch
            'if TestGetContract failed no need to move forward.
            Return
        End Try
        expectedRevisionNumber = firstContract.CarrTarRevisionNumber + 1

        Dim result As DTO.GenericResults = cloneTariff(firstContract.CarrTarControl,
                    firstContract.CarrTarEffDateFrom,
                    firstContract.CarrTarEffDateTo,
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
                    True,
                    True)
        If result Is Nothing OrElse result.Control = 0 OrElse Not result.ErrNumber = 0 Then
            Assert.Fail("TestCloneTariff - clone failed" & result.RetMsg)
        End If

        Try
            clonedContract = TestGetContractTestClone(contractID)
        Catch
            'if TestGetContract failed no need to move forward.
            Return
        End Try

        actualRevisionNumber = clonedContract.CarrTarRevisionNumber
        If clonedContract.CarrTarControl = firstContract.CarrTarControl Then
            Assert.Fail("testClonedContract - cloned control is the same.")
        End If
        If Not (firstContract.CarrTarRevisionNumber + 1) = actualRevisionNumber Then
            Assert.Fail("testClonedContract - the revised number is not the next number.")
        End If

    End Sub

    <TestMethod> _
    Public Sub CopyTariffTruckLoadTest()
        testParameters.DBServer = "NGLRDP06D"
        testParameters.Database = "NGLMAS2013Dev"

        Dim contractID As String = "8005-31"
        Dim newName As String = "Name_" & contractID
        Dim newCompControl = 11107 'King Kold, LLC comp number 567
        Dim oldCompControl = 9623 'VJI comp number 31
        Dim firstContract As DTO.CarrTarContract
        Dim copiedContract As DTO.CarrTarContract
        Try
            firstContract = getLatestContract(contractID)
        Catch
            'if TestGetContract failed no need to move forward.
            Return
        End Try

        Dim cdal As New DAL.NGLCarrTarContractData(testParameters)
        Dim genResult As String = cdal.generateNewCarrTarID(newCompControl, firstContract.CarrTarCarrierControl, firstContract.CarrTarTariffModeTypeControl, firstContract.CarrTarTariffTypeControl, firstContract.CarrTarTempType, firstContract.CarrTarEffDateFrom, firstContract.CarrTarOutbound)
        If String.IsNullOrEmpty(genResult) Then
            Assert.Fail("Genereate ID failed.")
        End If

        Dim result As DTO.GenericResults = copyTariff(firstContract.CarrTarControl,
                    firstContract.CarrTarEffDateFrom,
                    firstContract.CarrTarEffDateTo,
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
                    True,
                    True,
                    newCompControl,
                    newName)
        If result Is Nothing OrElse result.Control = 0 OrElse Not result.ErrNumber = 0 Then
            Assert.Fail("TestCloneTariff - clone failed" & result.RetMsg)
        End If

        Try
            copiedContract = getLatestContract(result.Control)
        Catch
            'if TestGetContract failed no need to move forward.
            Return
        End Try

        If copiedContract.CarrTarControl = firstContract.CarrTarControl Then
            Assert.Fail("testClonedContract - cloned control is the same.")
        End If
        If copiedContract.CarrTarCompControl = firstContract.CarrTarCompControl Then
            Assert.Fail("testClonedContract - cloned comp control is the same.")
        End If

        deleteContract(copiedContract)
    End Sub

    Private Function TestGetContractTestClone(ByVal contractID As String)

        Dim contract As DTO.CarrTarContract
        Dim classXRefs As DTO.CarrTarClassXref()
        Dim discounts As DTO.CarrTarDiscount()
        Dim interlines As DTO.CarrTarInterline()
        Dim minCharges As DTO.CarrTarMinCharge()
        Dim minWeights As DTO.CarrTarMinWeight()
        Dim nonServices As DTO.CarrTarNonServ()
        Dim equipment As DTO.CarrTarEquip()
        Dim rates As DTO.CarrTarEquipMatPivot()
        Dim fees As DTO.CarrTarFee()
        Dim fuelAddendum As DTO.CarrierFuelAddendum
        Dim fuelEx As DTO.CarrierFuelAdEx()
        Dim fuelAdRate As DTO.CarrierFuelAdRate()
        Dim noDriveDays As DTO.CarrTarNoDriveDays()

        contract = getLatestContract(contractID)
        If contract Is Nothing OrElse contract.CarrTarControl = 0 Then
            Assert.Fail("TestCloneTariff - could not find lastest contract")
        End If

        classXRefs = getClassXrefData(contract.CarrTarControl)
        If classXRefs Is Nothing OrElse classXRefs.Length <> 2 Then
            Assert.Fail("TestCloneTariff - classXRefs was null or not correct amount.")
        End If

        discounts = getDiscountData(contract.CarrTarControl)
        If discounts Is Nothing OrElse discounts.Length <> 1 Then
            Assert.Fail("TestCloneTariff - discounts was null or not correct amount.")
        End If

        interlines = getInterlineData(contract.CarrTarControl)
        If interlines Is Nothing OrElse interlines.Length <> 1 Then
            Assert.Fail("TestCloneTariff - interlines was null or not correct amount.")
        End If

        minCharges = getMinChargeData(contract.CarrTarControl)
        If minCharges Is Nothing OrElse minCharges.Length <> 1 Then
            Assert.Fail("TestCloneTariff - minCharges was null or not correct amount.")
        End If

        minWeights = getMinWeightData(contract.CarrTarControl)
        If minWeights Is Nothing OrElse minWeights.Length <> 2 Then
            Assert.Fail("TestCloneTariff - minWeights was null or not correct amount.")
        End If

        nonServices = getNonServiceData(contract.CarrTarControl)
        If nonServices Is Nothing OrElse nonServices.Length <> 1 Then
            Assert.Fail("TestCloneTariff - nonServices was null or not correct amount.")
        End If

        equipment = getTariffEquipmentData(contract.CarrTarControl)
        If equipment Is Nothing OrElse equipment.Length <> 1 Then
            Assert.Fail("TestCloneTariff - equipment was null or not correct amount.")
        End If

        '		key	"165-1-0-0"	string'
        rates = getRatesData(equipment(0).CarrTarEquipControl & "-1-0-0")
        If rates Is Nothing OrElse rates.Length <> 1 Then
            Assert.Fail("TestCloneTariff - rates was null or not correct amount.")
        End If

        fees = gettarifffeesData(contract.CarrTarControl)
        If fees Is Nothing OrElse fees.Length <> 2 Then
            Assert.Fail("TestCloneTariff - fees was null or not correct amount.")
        End If

        fuelAddendum = getTariffFuelAddundumData(contract.CarrTarControl)
        If fuelAddendum Is Nothing OrElse fuelAddendum.CarrFuelAdCarrTarControl <> contract.CarrTarControl Then
            Assert.Fail("TestCloneTariff - fuelAddendum.")
        End If

        fuelEx = getTariffFuelExceptionsData(fuelAddendum.CarrFuelAdControl)
        If fuelEx Is Nothing OrElse fuelEx.Length <> 1 Then
            Assert.Fail("TestCloneTariff - fuelEx was null or not correct amount.")
        End If

        fuelAdRate = getTariffFuelAdRateData(fuelAddendum.CarrFuelAdControl)
        If fuelAdRate Is Nothing OrElse fuelAdRate.Length <> 2 Then
            Assert.Fail("TestCloneTariff - fuelAdRate was null or not correct amount.")
        End If

        noDriveDays = getNoDriveDaysData(contract.CarrTarControl)
        If noDriveDays Is Nothing OrElse noDriveDays.Length <> 2 Then
            Assert.Fail("TestCloneTariff - noDriveDays was null or not correct amount.")
        End If

        Return contract
    End Function

#End Region

    <TestCategory("Nightly")>
    <TestMethod> _
    Public Sub TestAllTariffs()
        'connect to  database before getting any records overwrite the defaul
        testParameters.DBServer = "NGLRDP06D"
        testParameters.Database = "SHIELDSMASUnitTest"
        Dim blnRunNextTest As Boolean = True
        '****************  Begin Rate Shopping Tests *************************
        If blnRunNextTest Then blnRunNextTest = TestCanadianRateShop()
        If blnRunNextTest Then blnRunNextTest = TestUniqueTruckLoadRateShop()
        If blnRunNextTest Then blnRunNextTest = TestLargeFEDEXLTLRateShop()
        '****************  Begin Estimated Carrier Costing Tests *************************
        'test for duplicate rates and verify intermodal flat rate for JBHunt
        If blnRunNextTest Then blnRunNextTest = TestUniqueEstimateJBHuntLoad()
        'Test minimum weight requirements on multiple pallets
        If blnRunNextTest Then blnRunNextTest = TestEstMinWeightMultiPLTYRCLTLLoad()
        'Test Minimum weight requirements on single pallet
        If blnRunNextTest Then blnRunNextTest = TestEstMinWeightYRCLTLLoad()
        'Test Minimum rate for small load
        If blnRunNextTest Then blnRunNextTest = TestEstMinRateFEDEXLTLLoad()
        'Test LTL master billing consolidation with multiple classes and multiple orders to same address
        If blnRunNextTest Then blnRunNextTest = TestEstMasterBillMultiClassFEDEXLTLLoad()
        'Test LTL Deficit Weight (multiple class codes included)
        If blnRunNextTest Then blnRunNextTest = TestEstMultiClassDeficitFEDEXLTLLoad()
        'Test Large Load above highet break point
        If blnRunNextTest Then blnRunNextTest = TestEstimateLargeReddawayLTLLoad()
        'Test Medium LTL rating with default class code (no Item details)
        If blnRunNextTest Then blnRunNextTest = TestEstimateMediumFEDEXLTLLoad()
        'Test Medium LTL rating for multiple class codes with item details and no deficit weight needed
        If blnRunNextTest Then blnRunNextTest = TestEstimateMultiClassFEDEXLTLLoad()




    End Sub

#Region " Test All Estimate Cost Tests"
    ''' <summary>
    ''' Test Minimum weight restrictions for multiple pallet load with no item details using default Class (parameter setting) 
    '''  
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' actual weight = 1000
    ''' minimumn weight per pallet = 500
    ''' 3 pallets
    ''' adjusted weight = 1500 lbs
    ''' bracket 1000
    ''' rate 1.1546 
    ''' line haul cost = 1.1546 * 1500 =  1731.9
    ''' BookRevDiscount	 1382.06D
    ''' BookRevLineHaul	 1731.9D
    ''' CarrierCost	 391.82D
    ''' TotalAccessorial 41.98D
    ''' </remarks>
    Private Function TestEstMinWeightMultiPLTYRCLTLLoad() As Boolean
        Dim blnRet As Boolean = True
        Try
            Dim BookProNumber As String = "YAK757263"
            Dim oBook As DTO.Book = getBookByPro(BookProNumber)
            If oBook Is Nothing Then
                Assert.Fail("TestEstMinWeightYRCLTLLoad Failed: no booking record for BookProNumber {0}", BookProNumber)
                Return False
            End If
            Dim BookControl As Integer = oBook.BookControl
            'Get the Carrier Data
            Dim CarrierControl As Integer = 0
            Dim oCarrier As DTO.Carrier = getCarrierByName("YRC")
            If oCarrier Is Nothing Then
                Assert.Fail("TestEstMinWeightYRCLTLLoad Failed: no carrier data found")
                Return False
            End If
            CarrierControl = oCarrier.CarrierControl
            Dim target As New NGL.FM.CarTar.BookRev(testParameters)

            Dim results As DTO.CarrierCostResults = target.estimatedCarriersByCost(BookControl, CarrierControl)

            If results Is Nothing OrElse results.CarriersByCost Is Nothing Then
                Assert.Fail("TestEstMinWeightYRCLTLLoad Failed: No estimatedCarriersByCost results for BookProNumber {0}", BookProNumber)
                Return False
            End If

            For Each l As DTO.NGLMessage In results.Log
                System.Diagnostics.Debug.WriteLine(l.Message)
                If l.Control <> 0 Then
                    System.Diagnostics.Debug.WriteLine("Link to " + l.ControlReferenceName + " Control # " + l.Control.ToString())
                End If
            Next

            'test the results only one carrier should be returned so just test the first one
            With results.CarriersByCost(0)
                If .CarrierCost <> 391.82D Then
                    Assert.Fail("TestEstMinWeightYRCLTLLoad Failed: Carrier Cost is not correct {0}", .CarrierCost)
                    Return False
                End If
                If .BookRevDiscount <> 1382.06D Then
                    Assert.Fail("TestEstMinWeightYRCLTLLoad Failed: Discount is not correct {0}", .BookRevDiscount)
                    Return False
                End If
                If .BookRevLineHaul <> 1731.9D Then
                    Assert.Fail("TestEstMinWeightYRCLTLLoad Failed: Line Haul is not correct {0}", .BookRevLineHaul)
                    Return False
                End If
                'Cannot always test fees because fuel changes (uncomment to test fees manually)
                'If .TotalAccessorial <> 41.98D Then
                '    Assert.Fail("TestEstMinWeightYRCLTLLoad Failed: Accessorial is not correct {0}", .TotalAccessorial)
                '    Return False
                'End If
            End With


            Dim ct As Integer = results.CarriersByCost.Count

        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For TestEstMinWeightYRCLTLLoad: {0} ", ex.Message)
        Finally
            'place clean up code here
        End Try
        Return blnRet
    End Function

    ''' <summary>
    ''' Test Minimum weight restrictions for small load with no item details using default Class (parameter setting) 
    '''  
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Actual weight = 100
    ''' 1 pallet
    ''' Adjusted weight = 500
    ''' break point bracket = 500
    ''' rate = 1.3929
    ''' Line Haul Cost = 696.45 = 1.3929 * 500
    ''' BookRevDiscount	555.77D	Decimal
    ''' BookRevLineHaul	696.45D	Decimal
    ''' CarrierCost	157.56D	Decimal
    ''' TotalAccessorial	16.88D	Decimal
    ''' 
    ''' 
    ''' </remarks>
    Private Function TestEstMinWeightYRCLTLLoad() As Boolean
        Dim blnRet As Boolean = True
        Try
            Dim BookProNumber As String = "YAK757262"
            Dim oBook As DTO.Book = getBookByPro(BookProNumber)
            If oBook Is Nothing Then
                Assert.Fail("TestEstMinWeightYRCLTLLoad Failed: no booking record for BookProNumber {0}", BookProNumber)
                Return False
            End If
            Dim BookControl As Integer = oBook.BookControl
            'Get the Carrier Data
            Dim CarrierControl As Integer = 0
            Dim oCarrier As DTO.Carrier = getCarrierByName("YRC")
            If oCarrier Is Nothing Then
                Assert.Fail("TestEstMinWeightYRCLTLLoad Failed: no carrier data found")
                Return False
            End If
            CarrierControl = oCarrier.CarrierControl
            Dim target As New NGL.FM.CarTar.BookRev(testParameters)

            Dim results As DTO.CarrierCostResults = target.estimatedCarriersByCost(BookControl, CarrierControl)

            If results Is Nothing OrElse results.CarriersByCost Is Nothing Then
                Assert.Fail("TestEstMinWeightYRCLTLLoad Failed: No estimatedCarriersByCost results for BookProNumber {0}", BookProNumber)
                Return False
            End If

            For Each l As DTO.NGLMessage In results.Log
                System.Diagnostics.Debug.WriteLine(l.Message)
                If l.Control <> 0 Then
                    System.Diagnostics.Debug.WriteLine("Link to " + l.ControlReferenceName + " Control # " + l.Control.ToString())
                End If
            Next

            'test the results only one carrier should be returned so just test the first one
            With results.CarriersByCost(0)
                If .CarrierCost <> 157.56D Then
                    Assert.Fail("TestEstMinWeightYRCLTLLoad Failed: Carrier Cost is not correct {0}", .CarrierCost)
                    Return False
                End If
                If .BookRevDiscount <> 555.77D Then
                    Assert.Fail("TestEstMinWeightYRCLTLLoad Failed: Discount is not correct {0}", .BookRevDiscount)
                    Return False
                End If
                If .BookRevLineHaul <> 696.45D Then
                    Assert.Fail("TestEstMinWeightYRCLTLLoad Failed: Line Haul is not correct {0}", .BookRevLineHaul)
                    Return False
                End If
                'Cannot always test fees because fuel changes (uncomment to test fees manually)
                'If .TotalAccessorial <> 16.88D Then
                '    Assert.Fail("TestEstMinWeightYRCLTLLoad Failed: Accessorial is not correct {0}", .TotalAccessorial)
                '    Return False
                'End If
            End With

            Dim ct As Integer = results.CarriersByCost.Count

        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For TestEstMinWeightYRCLTLLoad: {0} ", ex.Message)
        Finally
            'place clean up code here
        End Try
        Return blnRet
    End Function

    ''' <summary>
    ''' Test Minimum rate for small load with no item details using default Class (parameter setting) of 50
    ''' No minimum weight; deficit weight does not apply.  
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function TestEstMinRateFEDEXLTLLoad() As Boolean
        Dim blnRet As Boolean = True
        Try
            Dim BookProNumber As String = "YAK757262"
            Dim oBook As DTO.Book = getBookByPro(BookProNumber)
            If oBook Is Nothing Then
                Assert.Fail("TestEstMinRateFEDEXLTLLoad Failed: no booking record for BookProNumber {0}", BookProNumber)
                Return False
            End If
            Dim BookControl As Integer = oBook.BookControl
            'Get the Carrier Data
            Dim CarrierControl As Integer = 0
            Dim oCarrier As DTO.Carrier = getCarrierByName("FEDEX")
            If oCarrier Is Nothing Then
                Assert.Fail("TestEstMinRateFEDEXLTLLoad Failed: no carrier data found")
                Return False
            End If
            CarrierControl = oCarrier.CarrierControl
            Dim target As New NGL.FM.CarTar.BookRev(testParameters)

            Dim results As DTO.CarrierCostResults = target.estimatedCarriersByCost(BookControl, CarrierControl)

            If results Is Nothing OrElse results.CarriersByCost Is Nothing Then
                Assert.Fail("TestEstMinRateFEDEXLTLLoad Failed: No estimatedCarriersByCost results for BookProNumber {0}", BookProNumber)
                Return False
            End If

            For Each l As DTO.NGLMessage In results.Log
                System.Diagnostics.Debug.WriteLine(l.Message)
                If l.Control <> 0 Then
                    System.Diagnostics.Debug.WriteLine("Link to " + l.ControlReferenceName + " Control # " + l.Control.ToString())
                End If
            Next

            'test the results only one carrier should be returned so just test the first one
            With results.CarriersByCost(0)
                If .CarrierCost <> 112.54D Then
                    Assert.Fail("TestEstMinRateFEDEXLTLLoad Failed: Carrier Cost is not correct {0}", .CarrierCost)
                    Return False
                End If
                If .BookRevDiscount <> 0D Then
                    Assert.Fail("TestEstMinRateFEDEXLTLLoad Failed: Discount is not correct {0}", .BookRevDiscount)
                    Return False
                End If
                If .BookRevLineHaul <> 103.25D Then
                    Assert.Fail("TestEstMinRateFEDEXLTLLoad Failed: Line Haul is not correct {0}", .BookRevLineHaul)
                    Return False
                End If
                'Cannot always test fees because fuel changes (uncomment to test fees manually)
                'If .TotalAccessorial <> 9.29D Then
                '    Assert.Fail("TestEstMinRateFEDEXLTLLoad Failed: Accessorial is not correct {0}", .TotalAccessorial)
                '    Return False
                'End If
            End With

            Dim ct As Integer = results.CarriersByCost.Count

        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For TestEstMinRateFEDEXLTLLoad: {0} ", ex.Message)
        Finally
            'place clean up code here
        End Try
        Return blnRet
    End Function

    ''' <summary>
    ''' Test LTL master billing consolidation with multiple classes and multiple orders to same address
    ''' Uses a multii-line order with class 50, 100 and 150 items
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' RT757260 pro numbers (YAK757261 and YAK757260)
    ''' --------------------------------------------
    ''' BookRevDiscount 13343.38D
    ''' BookRevLineHaul 18105D
    ''' CarrierCost 5190.17D
    ''' TotalAccessorial 428.55D
    ''' -------------------------------------------
    ''' 10,000 lb rate
    ''' -------------------------------------------
    ''' 50  = 3382.2 (0.5637 per lb) @ 6000 lbs
    ''' 100 = 5889   (0.9815 per lb) @ 6000 lbs
    ''' 150 = 8833.8 (1.4723 per lb) @ 6000 lbs
    ''' Total = 18105
    ''' </remarks>
    Private Function TestEstMasterBillMultiClassFEDEXLTLLoad() As Boolean
        Dim blnRet As Boolean = True
        Try
            Dim BookProNumber As String = "YAK757261"
            Dim oBook As DTO.Book = getBookByPro(BookProNumber)
            If oBook Is Nothing Then
                Assert.Fail("TestEstMasterBillMultiClassFEDEXLTLLoad Failed: no booking records")
                Return False
            End If
            Dim BookControl As Integer = oBook.BookControl
            'Get the Carrier Data
            Dim CarrierControl As Integer = 0
            Dim oCarrier As DTO.Carrier = getCarrierByName("FEDEX")
            If oCarrier Is Nothing Then
                Assert.Fail("TestEstMasterBillMultiClassFEDEXLTLLoad Failed: no carrier data")
                Return False
            End If
            CarrierControl = oCarrier.CarrierControl
            Dim target As New NGL.FM.CarTar.BookRev(testParameters)

            Dim results As DTO.CarrierCostResults = target.estimatedCarriersByCost(BookControl, CarrierControl)

            If results Is Nothing OrElse results.CarriersByCost Is Nothing Then
                Assert.Fail("TestEstMasterBillMultiClassFEDEXLTLLoad Failed: No estimatedCarriersByCost results for BookProNumber: {0}", BookProNumber)
                Return False
            End If

            For Each l As DTO.NGLMessage In results.Log
                System.Diagnostics.Debug.WriteLine(l.Message)
                If l.Control <> 0 Then
                    System.Diagnostics.Debug.WriteLine("Link to " + l.ControlReferenceName + " Control # " + l.Control.ToString())
                End If
            Next

            'test the results only one carrier should be returned so just test the first one
            With results.CarriersByCost(0)
                If .CarrierCost <> 5190.17D Then
                    Assert.Fail("TestEstMasterBillMultiClassFEDEXLTLLoad Failed: Carrier Cost is not correct {0}", .CarrierCost)
                    Return False
                End If
                If .BookRevDiscount <> 13343.38D Then
                    Assert.Fail("TestEstMasterBillMultiClassFEDEXLTLLoad Failed: Discount is not correct {0}", .BookRevDiscount)
                    Return False
                End If
                If .BookRevLineHaul <> 18105D Then
                    Assert.Fail("TestEstMasterBillMultiClassFEDEXLTLLoad Failed: Line Haul is not correct {0}", .BookRevLineHaul)
                    Return False
                End If
                'Cannot always test fees because fuel changes (uncomment to test fees manually)
                'If .TotalAccessorial <> 428.55D Then
                '    Assert.Fail("TestEstMasterBillMultiClassFEDEXLTLLoad Failed: Accessorial is not correct {0}", .TotalAccessorial)
                '    Return False
                'End If
            End With


            Dim ct As Integer = results.CarriersByCost.Count

        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error for TestEstMasterBillMultiClassFEDEXLTLLoad: {0} ", ex.Message)
        Finally
            'place clean up code here
        End Try
        Return blnRet
    End Function

    ''' <summary>
    ''' Uses a multii-line order with class 50, 100 and 150 items for a total weight of 19550 lbs 
    ''' should rate at 20000 lbs rate
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' *************************************************
    ''' Using Defict 10000 lb rate
    ''' -------------------------------------------
    ''' 50  = 1691.1 (0.5637 per lb) @ 3000 lbs
    ''' 100 = 2944.5 (0.9815 per lb) @ 3000 lbs
    ''' 150 = 4416.9 (1.4723 per lb) @ 3000 lbs
    ''' sub total 9052.5
    ''' Deficit Weight Adjustment 
    ''' 50  = 563.7  (0.5637 per lb) @ 1000 lbs
    ''' Total 9616.2
    ''' *******************************************************
    ''' Without deficit 5,000 lb rate
    ''' --------------------------------------------
    ''' 50  = 1978.8 (0.6596 per lb) @ 3000 lbs
    ''' 100 = 3445.8 (1.1486 per lb) @ 3000 lbs
    ''' 150 = 5168.7 (1.7229 per lb) @ 3000 lbs
    ''' Total 10593.3
    ''' *******************************************************
    ''' </remarks>
    Private Function TestEstMultiClassDeficitFEDEXLTLLoad() As Boolean
        Dim blnRet As Boolean = True
        Try
            Dim BookProNumber As String = "YAK757259"
            Dim oBook As DTO.Book = getBookByPro(BookProNumber)
            If oBook Is Nothing Then
                Assert.Fail("TestEstMultiClassDeficitFEDEXLTLLoad Failed: no booking records")
                Return False
            End If
            Dim BookControl As Integer = oBook.BookControl
            'Get the Carrier Data
            Dim CarrierControl As Integer = 0
            Dim oCarrier As DTO.Carrier = getCarrierByName("FEDEX")
            If oCarrier Is Nothing Then
                Assert.Fail("TestEstMultiClassDeficitFEDEXLTLLoad Failed: no carrier data")
                Return False
            End If
            CarrierControl = oCarrier.CarrierControl
            Dim target As New NGL.FM.CarTar.BookRev(testParameters)

            Dim results As DTO.CarrierCostResults = target.estimatedCarriersByCost(BookControl, CarrierControl)

            If results Is Nothing OrElse results.CarriersByCost Is Nothing Then
                Assert.Fail("TestEstMultiClassDeficitFEDEXLTLLoad Failed: No estimatedCarriersByCost results for BookProNumber: {0}", BookProNumber)
                Return False
            End If

            For Each l As DTO.NGLMessage In results.Log
                System.Diagnostics.Debug.WriteLine(l.Message)
                If l.Control <> 0 Then
                    System.Diagnostics.Debug.WriteLine("Link to " + l.ControlReferenceName + " Control # " + l.Control.ToString())
                End If
            Next

            'test the results only one carrier should be returned so just test the first one
            With results.CarriersByCost(0)
                If .CarrierCost <> 2756.68D Then
                    Assert.Fail("TestEstMultiClassDeficitFEDEXLTLLoad Failed: Carrier Cost is not correct {0}", .CarrierCost)
                    Return False
                End If
                If .BookRevDiscount <> 7087.14D Then
                    Assert.Fail("TestEstMultiClassDeficitFEDEXLTLLoad Failed: Discount is not correct {0}", .BookRevDiscount)
                    Return False
                End If
                If .BookRevLineHaul <> 9616.2D Then
                    Assert.Fail("TestEstMultiClassDeficitFEDEXLTLLoad Failed: Line Haul is not correct {0}", .BookRevLineHaul)
                    Return False
                End If
                'Cannot always test fees because fuel changes (uncomment to test fees manually)
                'If .TotalAccessorial <> 227.62D Then
                '    Assert.Fail("TestEstMultiClassDeficitFEDEXLTLLoad Failed: Accessorial is not correct {0}", .TotalAccessorial)
                '    Return False
                'End If
            End With


            Dim ct As Integer = results.CarriersByCost.Count

        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error for TestEstimateMultiClassFEDEXLTLLoad: {0} ", ex.Message)
        Finally
            'place clean up code here
        End Try
        Return blnRet
    End Function

    ''' <summary>
    ''' Uses a multii-line order with class 50, 100 and 150 items
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function TestEstimateMultiClassFEDEXLTLLoad() As Boolean
        Dim blnRet As Boolean = True
        Try
            Dim BookProNumber As String = "YAK757258"
            Dim oBook As DTO.Book = getBookByPro(BookProNumber)
            If oBook Is Nothing Then
                Assert.Fail("TestEstimateMultiClassFEDEXLTLLoad Failed: no booking records")
                Return False
            End If
            Dim BookControl As Integer = oBook.BookControl
            'Get the Carrier Data
            Dim CarrierControl As Integer = 0
            Dim oCarrier As DTO.Carrier = getCarrierByName("FEDEX")
            If oCarrier Is Nothing Then
                Assert.Fail("TestEstimateMultiClassFEDEXLTLLoad Failed: no carrier data")
                Return False
            End If
            CarrierControl = oCarrier.CarrierControl
            Dim target As New NGL.FM.CarTar.BookRev(testParameters)

            Dim results As DTO.CarrierCostResults = target.estimatedCarriersByCost(BookControl, CarrierControl)

            If results Is Nothing OrElse results.CarriersByCost Is Nothing Then
                Assert.Fail("TestEstimateMultiClassFEDEXLTLLoad Failed: No estimatedCarriersByCost results for BookProNumber: {0}", BookProNumber)
                Return False
            End If

            For Each l As DTO.NGLMessage In results.Log
                System.Diagnostics.Debug.WriteLine(l.Message)
                If l.Control <> 0 Then
                    System.Diagnostics.Debug.WriteLine("Link to " + l.ControlReferenceName + " Control # " + l.Control.ToString())
                End If
            Next

            'test the results only one carrier should be returned so just test the first one
            With results.CarriersByCost(0)
                If .CarrierCost <> 5190.17D Then
                    Assert.Fail("TestEstimateMultiClassFEDEXLTLLoad Failed: Carrier Cost is not correct {0}", .CarrierCost)
                    Return False
                End If
                If .BookRevDiscount <> 13343.38D Then
                    Assert.Fail("TestEstimateMultiClassFEDEXLTLLoad Failed: Discount is not correct {0}", .BookRevDiscount)
                    Return False
                End If
                If .BookRevLineHaul <> 18105D Then
                    Assert.Fail("TestEstimateMultiClassFEDEXLTLLoad Failed: Line Haul is not correct {0}", .BookRevLineHaul)
                    Return False
                End If
                'Cannot always test fees because fuel changes (uncomment to test fees manually)
                'If .TotalAccessorial <> 428.55D Then
                '    Assert.Fail("TestEstimateMultiClassFEDEXLTLLoad Failed: Accessorial is not correct {0}", .TotalAccessorial)
                '    Return False
                'End If
            End With

            Dim ct As Integer = results.CarriersByCost.Count

        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error for TestEstimateMultiClassFEDEXLTLLoad: {0} ", ex.Message)
        Finally
            'place clean up code here
        End Try
        Return blnRet
    End Function

    ''' <summary>
    ''' Tests a single LTL load with no item details using default Class (parameter setting) of 50
    ''' No minimum weight; deficit weight does not apply.  
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function TestEstimateMediumFEDEXLTLLoad() As Boolean
        Dim blnRet As Boolean = True
        Try
            Dim BookProNumber As String = "YAK757228"
            Dim oBook As DTO.Book = getBookByPro(BookProNumber)
            If oBook Is Nothing Then
                Assert.Fail("TestEstimateMediumFEDEXLTLLoad Failed: no booking record for BookProNumber {0}", BookProNumber)
                Return False
            End If
            Dim BookControl As Integer = oBook.BookControl
            'Get the Carrier Data
            Dim CarrierControl As Integer = 0
            Dim oCarrier As DTO.Carrier = getCarrierByName("FEDEX")
            If oCarrier Is Nothing Then
                Assert.Fail("TestEstimateMediumFEDEXLTLLoad Failed: no carrier data found")
                Return False
            End If
            CarrierControl = oCarrier.CarrierControl
            Dim target As New NGL.FM.CarTar.BookRev(testParameters)

            Dim results As DTO.CarrierCostResults = target.estimatedCarriersByCost(BookControl, CarrierControl)

            If results Is Nothing OrElse results.CarriersByCost Is Nothing Then
                Assert.Fail("TestEstimateMediumFEDEXLTLLoad Failed: No estimatedCarriersByCost results for BookProNumber {0}", BookProNumber)
                Return False
            End If

            For Each l As DTO.NGLMessage In results.Log
                System.Diagnostics.Debug.WriteLine(l.Message)
                If l.Control <> 0 Then
                    System.Diagnostics.Debug.WriteLine("Link to " + l.ControlReferenceName + " Control # " + l.Control.ToString())
                End If
            Next

            'test the results only one carrier should be returned so just test the first one
            With results.CarriersByCost(0)
                If .CarrierCost <> 3030.08D Then
                    Assert.Fail("TestEstimateMediumFEDEXLTLLoad Failed: Carrier Cost is not correct {0}", .CarrierCost)
                    Return False
                End If
                If .BookRevDiscount <> 7790.05D Then
                    Assert.Fail("TestEstimateMediumFEDEXLTLLoad Failed: Discount is not correct {0}", .BookRevDiscount)
                    Return False
                End If
                If .BookRevLineHaul <> 10569.94D Then
                    Assert.Fail("TestEstimateMediumFEDEXLTLLoad Failed: Line Haul is not correct {0}", .BookRevLineHaul)
                    Return False
                End If
                'Cannot always test fees because fuel changes (uncomment to test fees manually)
                'If .TotalAccessorial <> 250.19D Then
                '    Assert.Fail("TestEstimateMediumFEDEXLTLLoad Failed: Accessorial is not correct {0}", .TotalAccessorial)
                '    Return False
                'End If
            End With


            Dim ct As Integer = results.CarriersByCost.Count

        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For TestEstimateMediumFEDEXLTLLoad: {0} ", ex.Message)
        Finally
            'place clean up code here
        End Try
        Return blnRet
    End Function

    ''' <summary>
    ''' Test the ability to go above the largest configured break point of 30000 lbs using 32235 lbs
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function TestEstimateLargeReddawayLTLLoad() As Boolean
        Dim blnRet As Boolean = True
        Dim strTestName As String = "TestEstimateLargeReddawayLTLLoad"
        Try
            Dim BookProNumber As String = "YAK757226"
            Dim oBook As DTO.Book = getBookByPro(BookProNumber)
            If oBook Is Nothing Then
                Assert.Fail(strTestName & " Failed: no booking record found for BookProNumber {0}", BookProNumber)
                Return False
            End If
            Dim BookControl As Integer = oBook.BookControl
            'Get the Carrier Data
            Dim CarrierControl As Integer = 0
            Dim oCarrier As DTO.Carrier = getCarrierByName("Reddaway")
            If oCarrier Is Nothing Then
                Assert.Fail(strTestName & " Failed: no carrier data found")
                Return False
            End If
            CarrierControl = oCarrier.CarrierControl
            Dim target As New NGL.FM.CarTar.BookRev(testParameters)

            Dim results As DTO.CarrierCostResults = target.estimatedCarriersByCost(BookControl, CarrierControl)

            If results Is Nothing OrElse results.CarriersByCost Is Nothing Then
                Assert.Fail(strTestName & " Failed: No estimatedCarriersByCost results for BookProNumber {0}", BookProNumber)
                Return False
            End If

            For Each l As DTO.NGLMessage In results.Log
                System.Diagnostics.Debug.WriteLine(l.Message)
                If l.Control <> 0 Then
                    System.Diagnostics.Debug.WriteLine("Link to " + l.ControlReferenceName + " Control # " + l.Control.ToString())
                End If
            Next

            'test the results only one carrier should be returned so just test the first one
            With results.CarriersByCost(0)
                If .CarrierCost <> 461.28D Then
                    Assert.Fail(strTestName & " Failed: Carrier Cost is not correct {0}", .CarrierCost)
                    Return False
                End If
                If .BookRevDiscount <> 0D Then
                    Assert.Fail(strTestName & " Failed: Discount is not correct {0}", .BookRevDiscount)
                    Return False
                End If
                If .BookRevLineHaul <> 435.17D Then
                    Assert.Fail(strTestName & " Failed: Line Haul is not correct {0}", .BookRevLineHaul)
                    Return False
                End If
                'Cannot always test fees because fuel changes (uncomment to test fees manually)
                'If .TotalAccessorial <> 26.11D Then
                '    Assert.Fail(strTestName & " Failed: Accessorial is not correct {0}", .TotalAccessorial)
                '    Return False
                'End If
            End With

            Dim ct As Integer = results.CarriersByCost.Count
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & strTestName & ": {0} ", ex.Message)
        Finally
            'place clean up code here
        End Try
        Return blnRet
    End Function

    ''' <summary>
    ''' test for duplicate rates and verify intermodal flat rate for JBHunt
    ''' No minimum weight; deficit weight does not apply; no item details.  
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function TestUniqueEstimateJBHuntLoad() As Boolean
        Dim blnRet As Boolean = True
        Dim strTestName As String = "TestUniqueEstimateJBHuntLoad"
        Try
            Dim BookProNumber As String = "YAK755867"
            Dim oBook As DTO.Book = getBookByPro(BookProNumber)
            If oBook Is Nothing Then
                Assert.Fail(strTestName & " Failed: no booking record for BookProNumber {0}", BookProNumber)
                Return False
            End If
            Dim BookControl As Integer = oBook.BookControl
            'Get the Carrier Data
            Dim CarrierControl As Integer = 11

            Dim target As New NGL.FM.CarTar.BookRev(testParameters)

            Dim results As DTO.CarrierCostResults = target.estimatedCarriersByCost(BookControl, CarrierControl)

            If results Is Nothing OrElse results.CarriersByCost Is Nothing Then
                Assert.Fail(strTestName & " Failed: No estimatedCarriersByCost results for BookProNumber {0}", BookProNumber)
                Return False
            End If

            If results.CarriersByCost.Count() > 1 Then
                Assert.Fail(strTestName & " Failed: Duplicate Rate Found {0} for tariff {1}", results.CarriersByCost(0).BookCarrTarEquipName, results.CarriersByCost(0).BookCarrTarName)
                Return False
            End If

            For Each l As DTO.NGLMessage In results.Log
                System.Diagnostics.Debug.WriteLine(l.Message)
                If l.Control <> 0 Then
                    System.Diagnostics.Debug.WriteLine("Link to " + l.ControlReferenceName + " Control # " + l.Control.ToString())
                End If
            Next

            'test the results only one carrier should be returned so just test the first one
            With results.CarriersByCost(0)
                If .CarrierCost <> 2319.44D Then
                    Assert.Fail(strTestName & " Failed: Carrier Cost is not correct {0}", .CarrierCost)
                    Return False
                End If
                If .BookRevDiscount <> 0D Then
                    Assert.Fail(strTestName & " Failed: Discount is not correct {0}", .BookRevDiscount)
                    Return False
                End If
                If .BookRevLineHaul <> 1909D Then
                    Assert.Fail(strTestName & " Failed: Line Haul is not correct {0}", .BookRevLineHaul)
                    Return False
                End If
                'Cannot always test fees because fuel changes (uncomment to test fees manually)
                'If .TotalAccessorial <> 410.44D Then
                '    Assert.Fail(strTestName & " Failed: Accessorial is not correct {0}", .TotalAccessorial)
                '    Return False
                'End If
            End With


            Dim ct As Integer = results.CarriersByCost.Count

        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & strTestName & ": {0} ", ex.Message)
        Finally
            'place clean up code here
        End Try
        Return blnRet
    End Function

#End Region

#Region "Test All RateShop Tests"

    ''' <summary>
    ''' Test to ensure that each rate is unique for spot rate.
    ''' should select most precise rate for each tariff
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function TestUniqueTruckLoadRateShop() As Boolean
        Dim blnRet As Boolean = True
        Dim strTestName As String = "TestUniqueTruckLoadRateShop"
        Try
            Dim target As BLL.NGLBookRevenueBLL = New BLL.NGLBookRevenueBLL(testParameters)
            Dim oBookRevs As New List(Of DTO.BookRevenue)
            'we used a fixed date for the load date because we want the tariffs to be static
            Dim dtLoad As Date = "3/26/15"
            Dim dtRequired As Date = "3/29/15"
            'Test Route
            '98903 Yakima, WA, Yakima to 64101 Kansas City, Mo US miles = 1707.5
            Dim oBookrev As New DTO.BookRevenue With {.BookDateLoad = dtLoad, _
                                                  .BookDateRequired = dtRequired, _
                                                  .BookDestCompControl = 0,
                                                  .BookDestCity = "Kansas City", _
                                                  .BookDestCountry = "US", _
                                                  .BookDestState = "MO", _
                                                  .BookDestZip = "64101", _
                                                  .BookLockBFCCost = False, _
                                                  .BookMilesFrom = 1707.5, _
                                                  .BookModeTypeControl = 0, _
                                                  .BookOrigCompControl = 1,
                                                  .BookOrigCity = "Yakima", _
                                                  .BookOrigCountry = "US", _
                                                  .BookOrigState = "WA", _
                                                  .BookOrigZip = "98902", _
                                                  .BookTotalBFC = 0, _
                                                  .BookTotalCases = 0, _
                                                  .BookTotalCube = 0, _
                                                  .BookTotalPL = 14, _
                                                  .BookTotalPX = 0, _
                                                  .BookTotalWgt = 40000, _
                                                  .BookTranCode = Nothing, _
                                                  .BookTypeCode = Nothing, _
                                                  .BookTransType = Nothing}


            oBookRevs.Add(oBookrev)
            Dim rateShop As New DTO.RateShop With {.Page = 1,
                                                .PageSize = 100,
                                                .AgentControl = 0,
                                                .BookRevs = oBookRevs,
                                                .CarrierControl = 0,
                                                .CarrTarEquipMatClass = "",
                                                .CarrTarEquipMatClassTypeControl = 0,
                                                .CarrTarEquipMatTarRateTypeControl = 0,
                                                .ModeTypeControl = 0,
                                                .NoLateDelivery = False,
                                                .OptimizeByCapacity = False,
                                                .Outbound = True,
                                                .Prefered = False,
                                                .TariffTypeControl = 0,
                                                .TempType = 0,
                                                .UsePCM = False,
                                                .Validated = False}


            Dim Results As DTO.CarrierCostResults = target.DoRateShop(rateShop)

            If Results Is Nothing OrElse Results.CarriersByCost Is Nothing Then
                Assert.Fail(strTestName & " Failed: Call to DoRateShop Failed no results")
                Return False
            End If

            For Each l As DTO.NGLMessage In Results.Log
                System.Diagnostics.Debug.WriteLine(l.Message)
                If l.Control <> 0 Then
                    System.Diagnostics.Debug.WriteLine("Link to " + l.ControlReferenceName + " Control # " + l.Control.ToString())
                End If
            Next

            If Results.CarriersByCost.Count() < 1 Then
                Assert.Fail(strTestName & " Failed: No Carriers Returned")
                Return False
            End If
            Dim lEquipment As New List(Of Integer)
            System.Diagnostics.Debug.WriteLine("Name           | Provider           | Cost       | Fuel ")
            'check that each carrier tariff is unique
            For Each carr In Results.CarriersByCost
                If Not lEquipment.Contains(carr.BookCarrTarEquipControl) Then
                    lEquipment.Add(carr.BookCarrTarEquipControl)
                Else
                    Assert.Fail(strTestName & " Failed: Duplicate Rate Found {0} for tariff {1}", carr.BookCarrTarEquipName, carr.BookCarrTarName)
                    Return False
                End If

                System.Diagnostics.Debug.WriteLine("{0} | {1} | {2} | {3}", carr.BookCarrTarName, carr.CarrierName, carr.CarrierCost, carr.FuelCost)
            Next
            Dim ct As Integer = Results.CarriersByCost.Count



        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & strTestName & ": {0} ", ex.Message)
        Finally

        End Try
        Return blnRet
    End Function


    ''' <summary>
    ''' Tests a large LTL Rate Shopping using FEDEX 
    ''' No minimum weight; deficit weight does not apply. 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function TestLargeFEDEXLTLRateShop() As Boolean
        Dim blnRet As Boolean = True
        Dim strTestName As String = "TestLargeFEDEXLTLRateShop"
        Try
            Dim target As BLL.NGLBookRevenueBLL = New BLL.NGLBookRevenueBLL(testParameters)
            Dim oBookRevs As New List(Of DTO.BookRevenue)
            'we used a fixed date for the load date because we want the tariffs to be static
            Dim dtLoad As Date = "3/26/15"
            Dim dtRequired As Date = "3/29/15"
            'Test Route
            '98903 Yakima, WA, Yakima to 64101 Kansas City, Mo US miles = 1707.5
            Dim oBookrev As New DTO.BookRevenue With {.BookDateLoad = dtLoad, _
                                                  .BookDateRequired = dtRequired, _
                                                  .BookDestCompControl = 0,
                                                  .BookDestCity = "Kansas City", _
                                                  .BookDestCountry = "US", _
                                                  .BookDestState = "MO", _
                                                  .BookDestZip = "64101", _
                                                  .BookLockBFCCost = False, _
                                                  .BookMilesFrom = 1707.5, _
                                                  .BookModeTypeControl = 0, _
                                                  .BookOrigCompControl = 1,
                                                  .BookOrigCity = "Yakima", _
                                                  .BookOrigCountry = "US", _
                                                  .BookOrigState = "WA", _
                                                  .BookOrigZip = "98902", _
                                                  .BookTotalBFC = 0, _
                                                  .BookTotalCases = 0, _
                                                  .BookTotalCube = 0, _
                                                  .BookTotalPL = 14, _
                                                  .BookTotalPX = 0, _
                                                  .BookTotalWgt = 40000, _
                                                  .BookTranCode = Nothing, _
                                                  .BookTypeCode = Nothing, _
                                                  .BookTransType = Nothing}


            oBookRevs.Add(oBookrev)
            'Get the Carrier Data
            Dim CarrierControl As Integer = 0
            Dim oCarrier As DTO.Carrier = getCarrierByName("FEDEX")
            If oCarrier Is Nothing Then
                Assert.Fail(strTestName & " Failed: no carrier data found")
                Return False
            End If
            CarrierControl = oCarrier.CarrierControl

            Dim rateShop As New DTO.RateShop With {.Page = 1,
                                                .PageSize = 100,
                                                .AgentControl = 0,
                                                .BookRevs = oBookRevs,
                                                .CarrierControl = CarrierControl,
                                                .CarrTarEquipMatClass = "",
                                                .CarrTarEquipMatClassTypeControl = 0,
                                                .CarrTarEquipMatTarRateTypeControl = 0,
                                                .ModeTypeControl = 0,
                                                .NoLateDelivery = False,
                                                .OptimizeByCapacity = False,
                                                .Outbound = True,
                                                .Prefered = False,
                                                .TariffTypeControl = 0,
                                                .TempType = 0,
                                                .UsePCM = True,
                                                .Validated = False}


            Dim Results As DTO.CarrierCostResults = target.DoRateShop(rateShop)

            If Results Is Nothing OrElse Results.CarriersByCost Is Nothing Then
                Assert.Fail(strTestName & " Failed: No Spot Rates Returned")
                Return False
            End If

            For Each l As DTO.NGLMessage In Results.Log
                System.Diagnostics.Debug.WriteLine(l.Message)
                If l.Control <> 0 Then
                    System.Diagnostics.Debug.WriteLine("Link to " + l.ControlReferenceName + " Control # " + l.Control.ToString())
                End If
            Next

            'test the results only one carrier should be returned so just test the first one
            If Results.CarriersByCost.Count = 0 Then
                Assert.Fail("No tariffs returned.")
            End If

            With Results.CarriersByCost(0)
                If .CarrierCost <> 5233.45D Then
                    Assert.Fail(strTestName & " Failed: Carrier Cost is not correct {0}", .CarrierCost)
                    Return False
                End If
                If .BookRevDiscount <> 13454.67D Then
                    Assert.Fail(strTestName & " Failed: Discount is not correct {0}", .BookRevDiscount)
                    Return False
                End If
                If .BookRevLineHaul <> 18256D Then
                    Assert.Fail(strTestName & " Failed: Line Haul is not correct {0}", .BookRevLineHaul)
                    Return False
                End If
                'Cannot always test fees because fuel changes (uncomment to test fees manually)
                'If .TotalAccessorial <> 432.12D Then
                '    Assert.Fail("TestEstimateMediumFEDEXLTLLoad Failed: Accessorial is not correct {0}", .TotalAccessorial)
                '    Return False
                'End If
            End With

            Dim ct As Integer = Results.CarriersByCost.Count

        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & strTestName & ": {0} ", ex.Message)
        Finally
            'place clean up code here
        End Try
        Return blnRet
    End Function


    ''' <summary>
    ''' Test to ensure that each rate is unique for spot rate.
    ''' should select most precise rate for each tariff
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function TestCanadianRateShop() As Boolean
        Dim blnRet As Boolean = True
        Dim strTestName As String = "TestCanadianRateShop"
        Try
            Dim target As BLL.NGLBookRevenueBLL = New BLL.NGLBookRevenueBLL(testParameters)
            Dim oBookRevs As New List(Of DTO.BookRevenue)
            'we used a fixed date for the load date because we want the tariffs to be static
            Dim dtLoad As Date = "3/26/15"
            Dim dtRequired As Date = "3/29/15"
            'Test Route
            '98903 Yakima, WA, Yakima to 64101 Kansas City, Mo US miles = 1707.5
            Dim oBookrev As New DTO.BookRevenue With {.BookDateLoad = dtLoad, _
                                                  .BookDateRequired = dtRequired, _
                                                  .BookDestCompControl = 0,
                                                  .BookDestCity = "Vancouver", _
                                                  .BookDestCountry = "CA", _
                                                  .BookDestState = "BC", _
                                                  .BookDestZip = "V6A 2A2", _
                                                  .BookLockBFCCost = False, _
                                                  .BookMilesFrom = 1707.5, _
                                                  .BookModeTypeControl = 0, _
                                                  .BookOrigCompControl = 1,
                                                  .BookOrigCity = "Yakima", _
                                                  .BookOrigCountry = "US", _
                                                  .BookOrigState = "WA", _
                                                  .BookOrigZip = "98902", _
                                                  .BookTotalBFC = 0, _
                                                  .BookTotalCases = 0, _
                                                  .BookTotalCube = 0, _
                                                  .BookTotalPL = 4, _
                                                  .BookTotalPX = 0, _
                                                  .BookTotalWgt = 5000, _
                                                  .BookTranCode = Nothing, _
                                                  .BookTypeCode = Nothing, _
                                                  .BookTransType = Nothing}


            oBookRevs.Add(oBookrev)
            Dim rateShop As New DTO.RateShop With {.Page = 1,
                                                .PageSize = 100,
                                                .AgentControl = 0,
                                                .BookRevs = oBookRevs,
                                                .CarrierControl = 0,
                                                .CarrTarEquipMatClass = "",
                                                .CarrTarEquipMatClassTypeControl = 0,
                                                .CarrTarEquipMatTarRateTypeControl = 0,
                                                .ModeTypeControl = 0,
                                                .NoLateDelivery = False,
                                                .OptimizeByCapacity = False,
                                                .Outbound = True,
                                                .Prefered = False,
                                                .TariffTypeControl = 0,
                                                .TempType = 0,
                                                .UsePCM = False,
                                                .Validated = False}


            Dim Results As DTO.CarrierCostResults = target.DoRateShop(rateShop)

            If Results Is Nothing OrElse Results.CarriersByCost Is Nothing Then
                Assert.Fail(strTestName & " Failed: Call to DoRateShop Failed no results")
                Return False
            End If

            For Each l As DTO.NGLMessage In Results.Log
                System.Diagnostics.Debug.WriteLine(l.Message)
                If l.Control <> 0 Then
                    System.Diagnostics.Debug.WriteLine("Link to " + l.ControlReferenceName + " Control # " + l.Control.ToString())
                End If
            Next

            If Results.CarriersByCost.Count() < 1 Then
                Assert.Fail(strTestName & " Failed: No Carriers Returned")
                Return False
            End If
            System.Diagnostics.Debug.WriteLine("Name           | Provider           | Cost       | Fuel ")
            'print the results
            For Each carr In Results.CarriersByCost
                System.Diagnostics.Debug.WriteLine("{0} | {1} | {2} | {3}", carr.BookCarrTarName, carr.CarrierName, carr.CarrierCost, carr.FuelCost)
            Next
            Dim ct As Integer = Results.CarriersByCost.Count



        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & strTestName & ": {0} ", ex.Message)
        Finally

        End Try
        Return blnRet
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    <TestMethod>
    Public Sub TestFEDEXRateShop()

        Dim strTestName As String = "TestFEDEXRateShop"
        Try
            testParameters.DBServer = "NGLRDP05D"
            testParameters.Database = "ShieldsMasPROD"
            Dim target As BLL.NGLBookRevenueBLL = New BLL.NGLBookRevenueBLL(testParameters)
            Dim oBookRevs As New List(Of DTO.BookRevenue)
            'we used a fixed date for the load date because we want the tariffs to be static
            Dim dtLoad As Date = Date.Now.ToShortDateString() ' "3/26/15"
            Dim dtRequired As Date = Date.Now.AddDays(3).ToShortDateString() '"3/29/15"
            'Test Route
            '98903 Yakima, WA, Yakima to 64101 Kansas City, Mo US miles = 1707.5
            Dim oBookrev As New DTO.BookRevenue With {.BookDateLoad = dtLoad,
                                                  .BookDateRequired = dtRequired,
                                                  .BookDestCompControl = 0,
                                                  .BookDestCity = "Kansas City",
                                                  .BookDestCountry = "US",
                                                  .BookDestState = "MO",
                                                  .BookDestZip = "64101",
                                                  .BookLockBFCCost = False,
                                                  .BookMilesFrom = 1707.5,
                                                  .BookModeTypeControl = 0,
                                                  .BookOrigCompControl = 1,
                                                  .BookOrigCity = "Yakima",
                                                  .BookOrigCountry = "US",
                                                  .BookOrigState = "WA",
                                                  .BookOrigZip = "98902",
                                                  .BookTotalBFC = 0,
                                                  .BookTotalCases = 0,
                                                  .BookTotalCube = 0,
                                                  .BookTotalPL = 5,
                                                  .BookTotalPX = 0,
                                                  .BookTotalWgt = 9000,
                                                  .BookTranCode = Nothing,
                                                  .BookTypeCode = Nothing,
                                                  .BookTransType = Nothing}


            oBookRevs.Add(oBookrev)
            'Get the Carrier Data
            Dim CarrierControl As Integer = 29
            'Dim oCarrier As DTO.Carrier = getCarrierByName("FEDEX")
            'If oCarrier Is Nothing Then
            '    Assert.Fail(strTestName & " Failed: no carrier data found")
            '    Return
            'End If
            'CarrierControl = oCarrier.CarrierControl

            Dim rateShop As New DTO.RateShop With {.Page = 1,
                                                .PageSize = 100,
                                                .AgentControl = 0,
                                                .BookRevs = oBookRevs,
                                                .CarrierControl = CarrierControl,
                                                .CarrTarEquipMatClass = "",
                                                .CarrTarEquipMatClassTypeControl = 0,
                                                .CarrTarEquipMatTarRateTypeControl = 0,
                                                .ModeTypeControl = 0,
                                                .NoLateDelivery = False,
                                                .OptimizeByCapacity = False,
                                                .Outbound = True,
                                                .Prefered = False,
                                                .TariffTypeControl = 0,
                                                .TempType = 0,
                                                .UsePCM = True,
                                                .Validated = False}


            Dim Results As DTO.CarrierCostResults = target.DoRateShop(rateShop)

            If Results Is Nothing OrElse Results.CarriersByCost Is Nothing Then
                Assert.Fail(strTestName & " Failed: No Rates Returned")
                Return
            End If

            'For Each l As DTO.NGLMessage In Results.Log
            '    System.Diagnostics.Debug.WriteLine(l.Message)
            '    If l.Control <> 0 Then
            '        System.Diagnostics.Debug.WriteLine("Link to " + l.ControlReferenceName + " Control # " + l.Control.ToString())
            '    End If
            'Next

            'test the results only one carrier should be returned so just test the first one
            If Results.CarriersByCost.Count = 0 Then
                Assert.Fail("No tariffs returned.")
            End If

            'With Results.CarriersByCost(0)

            'End With

            Dim ct As Integer = Results.CarriersByCost.Count
            System.Diagnostics.Debug.WriteLine("Total Rates: " & ct.ToString())
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & strTestName & ": {0} ", ex.Message)
        Finally
            'place clean up code here
        End Try
    End Sub


    ''' <summary>
    ''' Test rate shopping
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks> 
    <TestMethod()>
    Public Function TesRateShop() As Boolean
        Dim blnRet As Boolean = True
        Dim strTestName As String = "TesRateShop"
        Try
            testParameters.DBServer = "NGLRDP07D"
            testParameters.Database = "NGLMASPROD"
            Dim target As BLL.NGLBookRevenueBLL = New BLL.NGLBookRevenueBLL(testParameters)
            Dim oBookRevs As New List(Of DTO.BookRevenue)
            'we used a fixed date for the load date because we want the tariffs to be static
            Dim dtLoad As Date = "12/14/2018"
            Dim dtRequired As Date = "12/18/2018"
            'Test Route
            'from company 600001 Waukesha 53188 to Alexandria, OH 43001
            '19	Waukesha	2101 Delafield Street	Waukesha	WI	53188	US
            Dim oBookrev As New DTO.BookRevenue With {.BookDateLoad = dtLoad,
                                                  .BookDateRequired = dtRequired,
                                                  .BookDestCompControl = 0,
                                                  .BookDestCity = "Alexandria",
                                                  .BookDestCountry = "US",
                                                  .BookDestState = "OH",
                                                  .BookDestZip = "43001",
                                                  .BookLockBFCCost = False,
                                                  .BookMilesFrom = 500,
                                                  .BookModeTypeControl = 0,
                                                  .BookOrigCompControl = 19,
                                                  .BookOrigCity = "Waukesha",
                                                  .BookOrigCountry = "US",
                                                  .BookOrigState = "WI",
                                                  .BookOrigZip = "53188",
                                                  .BookTotalBFC = 0,
                                                  .BookTotalCases = 0,
                                                  .BookTotalCube = 0,
                                                  .BookTotalPL = 1,
                                                  .BookTotalPX = 0,
                                                  .BookTotalWgt = 500,
                                                  .BookTranCode = Nothing,
                                                  .BookTypeCode = Nothing,
                                                  .BookTransType = Nothing}


            oBookRevs.Add(oBookrev)
            Dim rateShop As New DTO.RateShop With {.Page = 1,
                                                .PageSize = 100,
                                                .AgentControl = 0,
                                                .BookRevs = oBookRevs,
                                                .CarrierControl = 0,
                                                .CarrTarEquipMatClass = "",
                                                .CarrTarEquipMatClassTypeControl = 0,
                                                .CarrTarEquipMatTarRateTypeControl = 0,
                                                .ModeTypeControl = 0,
                                                .NoLateDelivery = False,
                                                .OptimizeByCapacity = False,
                                                .Outbound = True,
                                                .Prefered = False,
                                                .TariffTypeControl = 0,
                                                .TempType = 0,
                                                .UsePCM = False,
                                                .Validated = False}


            Dim Results As DTO.CarrierCostResults = target.DoRateShop(rateShop)

            If Results Is Nothing OrElse Results.CarriersByCost Is Nothing Then
                Assert.Fail(strTestName & " Failed: Call to DoRateShop Failed no results")
                Return False
            End If

            For Each l As DTO.NGLMessage In Results.Log
                System.Diagnostics.Debug.WriteLine(l.Message)
                If l.Control <> 0 Then
                    System.Diagnostics.Debug.WriteLine("Link to " + l.ControlReferenceName + " Control # " + l.Control.ToString())
                End If
            Next

            If Results.CarriersByCost.Count() < 1 Then
                Assert.Fail(strTestName & " Failed: No Carriers Returned")
                Return False
            End If
            System.Diagnostics.Debug.WriteLine("Name           | Provider           | Cost       | Fuel ")
            'print the results
            For Each carr In Results.CarriersByCost
                System.Diagnostics.Debug.WriteLine("{0} | {1} | {2} | {3}", carr.BookCarrTarName, carr.CarrierName, carr.CarrierCost, carr.FuelCost)
            Next
            Dim ct As Integer = Results.CarriersByCost.Count



        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & strTestName & ": {0} ", ex.Message)
        Finally

        End Try
        Return blnRet
    End Function

#End Region

End Class
