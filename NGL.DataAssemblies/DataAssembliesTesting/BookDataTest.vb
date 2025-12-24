Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports System
Imports DAL = NGL.FreightMaster.Data
Imports DTO = NGL.FreightMaster.Data.DataTransferObjects
Imports System.Data.SqlClient
Imports NGL.Core.ChangeTracker
Imports NGL.FM.BLL

<TestClass()>
Public Class BookDataTest
    Inherits TestBase


    <TestMethod()>
    Public Sub RemoteIntegrationDebugOrderData()



        Dim oBookHeaders As New List(Of NGLBookObjectWebService.clsBookHeaderObject60)
        Dim oBookHeader As New NGLBookObjectWebService.clsBookHeaderObject60()
        Dim oBookItems As New List(Of NGLBookObjectWebService.clsBookDetailObject60)
        Dim oBookItem As New NGLBookObjectWebService.clsBookDetailObject60()

        With oBookHeader
            .PONumber = "RobTestOrder123456"
            .POVendor = "31-C424276-81118775"
            .POdate = "Dec 20 2018 11:06AM"
            .POShipdate = "Jan  4 2019 12:00AM"
            .POFrt = 9
            .POTotalFrt = 1695.7
            .POTotalCost = 0
            .POWgt = 11040
            .POCube = 0
            .POQty = 28
            .POPallets = 7
            .POLines = 0
            .POConfirm = 0
            .PODefaultCustomer = "31"
            .PODefaultCarrier = 0
            .POReqDate = "Jan  9 2019 12:00AM"
            .POCooler = 0
            .POFrozen = 0
            .PODry = 0
            .POTemp = "M"
            .POConsigneeNumber = "0"
            .POCustomerPO = "2018 - 0 - 55429"
            .POOtherCosts = 0
            .POStatusFlag = 0
            .POOrderSequence = 0
            .POInbound = 0
            .POPalletExchange = 0
            .POPalletType = "0"
            .PODefaultRouteSequence = 0
            .PORouteGuideNumber = "0"
        End With
        oBookHeaders.Add(oBookHeader)
        With oBookItem
            .ItemPONumber = "RobTestOrder123456"
            .FixOffInvAllow = 0
            .FixFrtAllow = 0
            .ItemNumber = "NV0ARB10"
            .QtyOrdered = 1
            .FreightCost = 169.57
            .ItemCost = 183.807
            .Weight = 50
            .Cube = 0
            .Pack = 0
            .Description = "ONION JUICE, SPRAY DRIED"
            .LotNumber = "V061/4/A18"
            .CustomerNumber = "31"
            .POOrderSequence = 0
            .POItemLimitedQtyFlag = 0
            .POItemPallets = 1.75
            .POItemTies = 0
            .POItemHighs = 0
            .POItemQtyPalletPercentage = 0
            .POItemQtyLength = 0
            .POItemQtyWidth = 0
            .POItemQtyHeight = 0
            .POItemStackable = 0
            .POItemLevelOfDensity = 0
        End With
        oBookItems.Add(oBookItem)
        oBookItem = New NGLBookObjectWebService.clsBookDetailObject60()
        With oBookItem
            .ItemPONumber = "RobTestOrder123456"
            .FixOffInvAllow = 0
            .FixFrtAllow = 0
            .ItemNumber = "NV0RPD14"
            .QtyOrdered = 12
            .FreightCost = 678.279
            .ItemCost = 4297.56
            .Weight = 5280
            .Cube = 0
            .Pack = 0
            .Description = "ONION PUREE, REFRIGERATED"
            .LotNumber = "V310/10/A18"
            .CustomerNumber = "31"
            .POOrderSequence = 0
            .POItemLimitedQtyFlag = 0
            .POItemPallets = 1.75
            .POItemTies = 0
            .POItemHighs = 0
            .POItemQtyPalletPercentage = 0
            .POItemQtyLength = 0
            .POItemQtyWidth = 0
            .POItemQtyHeight = 0
            .POItemStackable = 0
            .POItemLevelOfDensity = 0
        End With

        oBookItems.Add(oBookItem)
        oBookItem = New NGLBookObjectWebService.clsBookDetailObject60()
        With oBookItem
            .ItemPONumber = "RobTestOrder123456"
            .FixOffInvAllow = 0
            .FixFrtAllow = 0
            .ItemNumber = "NV0RPD14"
            .QtyOrdered = 12
            .FreightCost = 678.279
            .ItemCost = 4297.56
            .Weight = 5280
            .Cube = 0
            .Pack = 0
            .Description = "ONION PUREE, REFRIGERATED"
            .LotNumber = "V310/11/A18"
            .CustomerNumber = "31"
            .POOrderSequence = 0
            .POItemLimitedQtyFlag = 0
            .POItemPallets = 1.75
            .POItemTies = 0
            .POItemHighs = 0
            .POItemQtyPalletPercentage = 0
            .POItemQtyLength = 0
            .POItemQtyWidth = 0
            .POItemQtyHeight = 0
            .POItemStackable = 0
            .POItemLevelOfDensity = 0
        End With

        oBookItems.Add(oBookItem)
        oBookItem = New NGLBookObjectWebService.clsBookDetailObject60()
        With oBookItem
            .ItemPONumber = "RobTestOrder123456"
            .FixOffInvAllow = 0
            .FixFrtAllow = 0
            .ItemNumber = "NV0ARB10"
            .QtyOrdered = 3
            .FreightCost = 169.57
            .ItemCost = 551.42
            .Weight = 150
            .Cube = 0
            .Pack = 0
            .Description = "ONION JUICE, SPRAY DRIED"
            .LotNumber = "V062/1/A18"
            .CustomerNumber = "31"
            .POOrderSequence = 0
            .POItemLimitedQtyFlag = 0
            .POItemPallets = 1.75
            .POItemTies = 0
            .POItemHighs = 0
            .POItemQtyPalletPercentage = 0
            .POItemQtyLength = 0
            .POItemQtyWidth = 0
            .POItemQtyHeight = 0
            .POItemStackable = 0
            .POItemLevelOfDensity = 0
        End With
        oBookItems.Add(oBookItem)
        Dim oBook As New NGLBookObjectWebService.BookObject()
        'prod
        'oBook.Url = "https://ws.nglnextrack.com/BookObject.asmx"
        'Dim WebAuthCode As String = "NGLWSPROD"
        'test
        'oBook.Url = "https://wstest.nglnextrack.com/BookObject.asmx"
        'Dim WebAuthCode As String = "NGLWSTEST"
        'debug
        oBook.Url = "http://localhost:56526/bookobject.asmx"
        Dim WebAuthCode As String = "NGLWSTEST"
        Dim strLastError As String = ""
        Dim intResult = oBook.ProcessData60(WebAuthCode, oBookHeaders.ToArray(), oBookItems.ToArray(), strLastError)
        Select Case intResult
            Case WebServiceReturnValues.nglDataConnectionFailure
                Assert.Fail("Database Connection Failure Error: " & strLastError)
            Case WebServiceReturnValues.nglDataIntegrationFailure
                Assert.Fail("Data Integration Failure Error: " & strLastError)
            Case WebServiceReturnValues.nglDataIntegrationHadErrors
                    'Assert.Fail("Some Errors: " & strLastError)
            Case WebServiceReturnValues.nglDataValidationFailure
                Assert.Fail("Data Validation Failure Error: " & strLastError)
            Case WebServiceReturnValues.nglDataIntegrationComplete
                'Main.insertMessage("Success! Data imported.")
            Case Else
                Assert.Fail("Invalid Return Value.")
        End Select


    End Sub




    <TestMethod()>
    Public Sub GetAllItemsTest()

        Dim proNumber As String = ""
        Dim CNS As String = ""
        Dim PO As String = ""
        Dim OrderNumber As String = ""
        Dim LoadDate As Nullable(Of DateTime) = Nothing
        Dim LoadDateTo As Nullable(Of DateTime) = Nothing
        Dim SCHEDULEDATE As Nullable(Of DateTime) = Nothing
        Dim SCHEDULEDATETo As Nullable(Of DateTime) = Nothing
        Dim LOADDelScheduleDate As Nullable(Of DateTime) = Nothing
        Dim LOADDelScheduleDateTo As Nullable(Of DateTime) = Nothing
        Dim LOADDelScheduleTime As String = ""
        Dim LOADActDelDate As Nullable(Of DateTime) = Nothing
        Dim LOADActDelDateTo As Nullable(Of DateTime) = Nothing
        Dim LOADCARRIERNAME As String = ""
        Dim LOADCARRIERNUMBER As Nullable(Of Integer) = 35
        Dim LOADDESTNAME As String = ""
        Dim LOADDESTCITY As String = ""
        Dim LOADDESTSTATE As String = ""
        Dim LOADBROKERNUMBER As String = Nothing
        Dim LOADCARRIERCONTCONTROL As Nullable(Of Integer) = 20878
        Dim UseCarrierFilters As Nullable(Of Boolean) = True
        Dim DaysOut As Nullable(Of Integer) = Nothing
        Dim DelDaysOut As Nullable(Of Integer) = Nothing
        Dim xmlTransCode As String = "<Root><row id=""N""></row><row id=""P""></row><row id=""PC""></row></Root>"
        Dim xmlLoadCompanyIDs As String = ""
        Dim xmlLoadLanes As String = ""
        Dim p_sortordinal As String = ""
        Dim p_sortdirection As String = ""
        Dim p_datefilterfield As String = ""
        Dim p_datefilterfrom As Nullable(Of DateTime) = Nothing
        Dim p_datefilterto As Nullable(Of DateTime) = Nothing
        Dim page As Integer = 1
        Dim pagesize As Integer = 1000
        Try
            Dim oData As DTO.AllItem()
            'testParameters.UserName = "ngl\rramsey"
            Dim oDataProvider As New DAL.NGLAllItemData(testParameters)
            oData = oDataProvider.GetAllItems(proNumber, _
                                CNS, _
                                PO, _
                                OrderNumber, _
                                LoadDate, _
                                LoadDateTo, _
                                SCHEDULEDATE, _
                                SCHEDULEDATETo, _
                                LOADDelScheduleDate, _
                                LOADDelScheduleDateTo, _
                                LOADDelScheduleTime, _
                                LOADActDelDate, _
                                LOADActDelDateTo, _
                                LOADCARRIERNAME, _
                                LOADCARRIERNUMBER, _
                                LOADDESTNAME, _
                                LOADDESTCITY, _
                                LOADDESTSTATE, _
                                LOADBROKERNUMBER, _
                                LOADCARRIERCONTCONTROL, _
                                UseCarrierFilters, _
                                DaysOut, _
                                DelDaysOut, _
                                xmlTransCode, _
                                xmlLoadCompanyIDs, _
                                xmlLoadLanes, _
                                p_sortordinal, _
                                p_sortdirection, _
                                p_datefilterfield, _
                                p_datefilterfrom, _
                                p_datefilterto, _
                                page, _
                                pagesize)

            If oData Is Nothing OrElse oData.Count() < 1 Then
                Assert.Fail("Read data failure")
            End If

        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("DoRateShopTest Unexpected Error: " & ex.Message)
        Finally

        End Try
    End Sub

    <TestMethod()>
    Public Sub GetBookFilteredNoChildrenTest()
        Try
            Dim oBook As New DAL.NGLBookData(testParameters)
            Dim intBookControl As Integer = 397978
            Dim oData = oBook.GetBookFilteredNoChildren(intBookControl)
            If oData Is Nothing OrElse oData.BookControl <> intBookControl Then
                Assert.Fail("Read data failure")
            End If

        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("DoRateShopTest Unexpected Error: " & ex.Message)
        Finally

        End Try
    End Sub

    <TestMethod()>
    Public Sub GetBookFilteredByProTest()
        Try

            Dim sPro As String = "PVI757593"
            Dim oData = getBookByPro("PVI757593")
            If oData Is Nothing OrElse oData.BookProNumber <> sPro Then
                Assert.Fail("Read data failure")
            End If

        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("DoRateShopTest Unexpected Error: " & ex.Message)
        Finally

        End Try
    End Sub

    '''<summary>
    '''A test for SaveRevenuesWDetails
    '''</summary>
    <TestMethod()> _
    Public Sub SaveRevenuesWDetailsTest()

        Dim target As New DAL.NGLBookRevenueData(testParameters)
        Dim Control As Integer = 804963
        'get some book reve records to work with
        Dim oRecords() As DTO.BookRevenue = target.GetBookRevenuesWDetailsFiltered(Control)
        If oRecords Is Nothing OrElse oRecords.Count < 1 Then
            Assert.Fail("Get Book Revenues With Details Data Failed for Book Control " & Control.ToString)
            Return
        End If
        'save the max revison number
        Dim maxRevison = oRecords.Max(Function(x) x.BookBookRevHistRevision)
        'save the old carrier costs
        Dim oldCarrierCosts As New Dictionary(Of Integer, Decimal)
        Dim oldFees As New Dictionary(Of Integer, Decimal)
        For Each b In oRecords
            oldCarrierCosts.Add(b.BookControl, b.BookRevCarrierCost)
            'add $1.00 to the cost for each record
            b.BookRevCarrierCost += 1

            For Each f In b.BookFees
                'save the book fees
                oldFees.Add(f.BookFeesControl, f.BookFeesMinimum)
                'update the minimum charge
                f.BookFeesMinimum += 1
            Next
        Next

        Dim actual() As DTO.BookRevenue
        actual = target.SaveRevenuesWDetails(oRecords)
        If actual Is Nothing OrElse actual.Count < 1 Then
            Assert.Fail("Get Save Revenues With Details Data Failed for Book Control " & Control.ToString)
            Return
        End If
        'check the revison number is should have been increased
        Dim NewMaxRevison = actual.Max(Function(x) x.BookBookRevHistRevision)
        If NewMaxRevison - maxRevison <> 1 Then
            Assert.Fail("Test Revison Increment failed for Book Control " & Control.ToString)
            Return
        End If
        'check if the costs have been updated
        For Each b In actual
            If oldCarrierCosts(b.BookControl) + 1 <> b.BookRevCarrierCost Then
                Assert.Fail("Test Saved Carrier Cost failed, invalid carrier cost, for Book Control " & b.BookControl.ToString)
                Return
            End If
            For Each f In b.BookFees
                If oldFees(f.BookFeesControl) + 1 <> f.BookFeesMinimum Then
                    Assert.Fail("Test Saved Carrier Cost failed, invalid Fee data, for Book Control " & b.BookControl.ToString)
                    Return
                End If
            Next
        Next

    End Sub

    <TestMethod()> _
    Public Sub ResetToNStatusCompareTest()
        testParameters.DBServer = "NGLRDP06D"
        testParameters.Database = "SHIELDSMASUnitTest"

        Dim oBook As New DAL.NGLBookData(testParameters)
        Dim target As New DAL.NGLBookRevenueData(testParameters)
        Dim oBookBLL As New NGLBookBLL(testParameters)

        'Insert the test records into the db
        Try
            oBook.CreateSampleOrdersOptUnitTest()
        Catch ex As Exception
            'What do I do here?
        End Try

        Dim Filters As String = ""
        Dim getVOptStopVB As DTO.vOptimizationStop()
        Dim getVOptStopSP As DTO.vOptimizationStop()
        Dim getVOptStopSPSHID As DTO.vOptimizationStop()


        Filters = "[bookdateload]>='1/1/2050'AND[bookdateload]<='12/31/2050'AND(isnull([BOOKORIGZIP],0))>='000000000'AND(isnull([BOOKORIGZIP],0))<='999999999'AND(isnull([BOOKDESTZIP],0))>='000000000'AND(isnull([BOOKDESTZIP],0))<='999999999'AND[CompNumber]=1"

        getVOptStopVB = oBook.GetvOptimizationStopData(Filters)

        Dim VBOptStopsNoSHID As New ChangeTrackingCollection(Of DTO.vOptimizationStop)
        Dim VBOptStopsHadSHID As New ChangeTrackingCollection(Of DTO.vOptimizationStop)
        For Each i As DTO.vOptimizationStop In getVOptStopVB
            If Not i.BookControl = 0 Then
                If String.IsNullOrEmpty(i.BookSHID) Then
                    VBOptStopsNoSHID.Add(i)
                    Dim newTranCodeResult = oBookBLL.ProcessNewTransCode(i.BookControl,
                                                             "N",
                                                             i.BookTranCode,
                                                             0)
                    If newTranCodeResult.Success = False Then
                        'we were unable to optimize the data because we could not reset all the records to N status

                        Return
                    End If

                Else
                    VBOptStopsHadSHID.Add(i)
                End If
            End If
        Next


        Filters = "[bookdateload]>='1/1/2050'AND[bookdateload]<='12/31/2050'AND(isnull([BOOKORIGZIP],0))>='000000000'AND(isnull([BOOKORIGZIP],0))<='999999999'AND(isnull([BOOKDESTZIP],0))>='000000000'AND(isnull([BOOKDESTZIP],0))<='999999999'AND[CompNumber]=1"

        getVOptStopVB = oBook.GetvOptimizationStopData(Filters)

        VBOptStopsNoSHID.Clear()
        VBOptStopsHadSHID.Clear()
        For Each i As DTO.vOptimizationStop In getVOptStopVB
            If Not i.BookControl = 0 Then
                If String.IsNullOrEmpty(i.BookSHID) Then
                    VBOptStopsNoSHID.Add(i)
                Else
                    VBOptStopsHadSHID.Add(i)
                End If
            End If
        Next
        'So now the data to compare from the vb method is in VBOptStopsNoSHID and VBOptStopsHadSHID
        'Delete the records and insert them again

        'Delete the test records from the db
        Try
            oBook.DeleteSampleOrdersOptUnitTest()
        Catch ex As Exception
            'What do I do here?
        End Try
        'Insert the test records into the db
        Try
            oBook.CreateSampleOrdersOptUnitTest()
        Catch ex As Exception
            'What do I do here?
        End Try


        'Call the new spResetToNStatus
        getVOptStopSP = oBook.ResetToNStatusSP("NGL\Lauren Van Vleet", Filters, 1)


        'Convert both collections to arrays ordered by BookControl desc
        Dim orderedSP = (From e In getVOptStopSP Order By e.BookControl Descending).ToArray()
        Dim orderedVB = (From e In VBOptStopsNoSHID Order By e.BookControl Descending).ToArray()

        'Compare orderedSP to orderedVB
        Assert.AreEqual(orderedVB.Length(), orderedSP.Length())
        Dim count As Integer = 0
        count = orderedVB.Length()

        'Now compare the results from the VB method to the results of the sp.
        For i = 0 To count - 1

            Dim vb = orderedVB(i)
            Dim sp = orderedSP(i)

            'Assert.AreEqual(vb.BookControl, sp.BookControl)
            Assert.AreEqual(vb.BookDateLoad, sp.BookDateLoad)
            Assert.AreEqual(vb.BookStopNo, sp.BookStopNo)
            Assert.AreEqual(vb.BookDateRequired, sp.BookDateRequired)
            Assert.AreEqual(vb.BookConsPrefix, sp.BookConsPrefix)
            Assert.AreEqual(vb.BookProNumber, sp.BookProNumber)
            Assert.AreEqual(vb.CompControl, sp.CompControl)
            Assert.AreEqual(vb.CompNumber, sp.CompNumber)
            Assert.AreEqual(vb.CompName, sp.CompName)
            Assert.AreEqual(vb.BookDestName, sp.BookDestName)
            Assert.AreEqual(vb.BookDestCity, sp.BookDestCity)
            Assert.AreEqual(vb.BookDestState, sp.BookDestState)
            Assert.AreEqual(vb.BookDestZip, sp.BookDestZip)
            Assert.AreEqual(vb.BookODControl, sp.BookODControl)
            Assert.AreEqual(vb.BookLoadCaseQty, sp.BookLoadCaseQty)
            Assert.AreEqual(vb.BookLoadWgt, sp.BookLoadWgt)
            Assert.AreEqual(vb.BookLoadCube, sp.BookLoadCube)
            Assert.AreEqual(vb.BookLoadPL, sp.BookLoadPL)
            Assert.AreEqual(vb.BookLoadPX, sp.BookLoadPX)
            Assert.AreEqual(vb.BookDestAddress1, sp.BookDestAddress1)
            Assert.AreEqual(vb.BookDestAddress2, sp.BookDestAddress2)
            Assert.AreEqual(vb.LaneLatitude, sp.LaneLatitude)
            Assert.AreEqual(vb.LaneLongitude, sp.LaneLongitude)
            Assert.AreEqual(vb.SpecialCodes, sp.SpecialCodes)
            Assert.AreEqual(vb.LaneFixedTime, sp.LaneFixedTime)
            'Assert.AreEqual(vb.BookLoadControl, sp.BookLoadControl)
            Assert.AreEqual(vb.BookHoldLoad, sp.BookHoldLoad)
            Assert.AreEqual(vb.LaneControl, sp.LaneControl)
            Assert.AreEqual(vb.ActualWgt, sp.ActualWgt)
            Assert.AreEqual(vb.LaneNumber, sp.LaneNumber)
            Assert.AreEqual(vb.BookCarrOrderNumber, sp.BookCarrOrderNumber)
            Assert.AreEqual(vb.BookFinARInvoiceDate, sp.BookFinARInvoiceDate)
            Assert.AreEqual(vb.BookDateOrdered, sp.BookDateOrdered)
            Assert.AreEqual(vb.BookTranCode, sp.BookTranCode)
            Assert.AreEqual(vb.BookSHID, sp.BookSHID)
        Next

        'Call the new spResetToNStatusOptHasSHID
        getVOptStopSPSHID = oBook.ResetToNStatusSPOptHasSHID("NGL\Lauren Van Vleet", Filters, 1)


        'Convert both collections to arrays ordered by BookControl desc
        Dim orderedSPSHID = (From e In getVOptStopSPSHID Order By e.BookControl Descending).ToArray()
        Dim orderedVBSHID = (From e In VBOptStopsHadSHID Order By e.BookControl Descending).ToArray()

        'Compare orderedSP to orderedVB
        Assert.AreEqual(orderedVB.Length(), orderedSP.Length())
        count = 0
        count = orderedVBSHID.Length()

        'Now compare the results from the VB method to the results of the sp.
        For i = 0 To count - 1

            Dim vb = orderedVBSHID(i)
            Dim sp = orderedSPSHID(i)

            'Assert.AreEqual(vb.BookControl, sp.BookControl)
            Assert.AreEqual(vb.BookDateLoad, sp.BookDateLoad)
            Assert.AreEqual(vb.BookStopNo, sp.BookStopNo)
            Assert.AreEqual(vb.BookDateRequired, sp.BookDateRequired)
            Assert.AreEqual(vb.BookConsPrefix, sp.BookConsPrefix)
            Assert.AreEqual(vb.BookProNumber, sp.BookProNumber)
            Assert.AreEqual(vb.CompControl, sp.CompControl)
            Assert.AreEqual(vb.CompNumber, sp.CompNumber)
            Assert.AreEqual(vb.CompName, sp.CompName)
            Assert.AreEqual(vb.BookDestName, sp.BookDestName)
            Assert.AreEqual(vb.BookDestCity, sp.BookDestCity)
            Assert.AreEqual(vb.BookDestState, sp.BookDestState)
            Assert.AreEqual(vb.BookDestZip, sp.BookDestZip)
            Assert.AreEqual(vb.BookODControl, sp.BookODControl)
            Assert.AreEqual(vb.BookLoadCaseQty, sp.BookLoadCaseQty)
            Assert.AreEqual(vb.BookLoadWgt, sp.BookLoadWgt)
            Assert.AreEqual(vb.BookLoadCube, sp.BookLoadCube)
            Assert.AreEqual(vb.BookLoadPL, sp.BookLoadPL)
            Assert.AreEqual(vb.BookLoadPX, sp.BookLoadPX)
            Assert.AreEqual(vb.BookDestAddress1, sp.BookDestAddress1)
            Assert.AreEqual(vb.BookDestAddress2, sp.BookDestAddress2)
            Assert.AreEqual(vb.LaneLatitude, sp.LaneLatitude)
            Assert.AreEqual(vb.LaneLongitude, sp.LaneLongitude)
            Assert.AreEqual(vb.SpecialCodes, sp.SpecialCodes)
            Assert.AreEqual(vb.LaneFixedTime, sp.LaneFixedTime)
            'Assert.AreEqual(vb.BookLoadControl, sp.BookLoadControl)
            Assert.AreEqual(vb.BookHoldLoad, sp.BookHoldLoad)
            Assert.AreEqual(vb.LaneControl, sp.LaneControl)
            Assert.AreEqual(vb.ActualWgt, sp.ActualWgt)
            Assert.AreEqual(vb.LaneNumber, sp.LaneNumber)
            Assert.AreEqual(vb.BookCarrOrderNumber, sp.BookCarrOrderNumber)
            Assert.AreEqual(vb.BookFinARInvoiceDate, sp.BookFinARInvoiceDate)
            Assert.AreEqual(vb.BookDateOrdered, sp.BookDateOrdered)
            Assert.AreEqual(vb.BookTranCode, sp.BookTranCode)
            Assert.AreEqual(vb.BookSHID, sp.BookSHID)
        Next

        'Delete the test records from the db
        Try
            oBook.DeleteSampleOrdersOptUnitTest()
        Catch ex As Exception
            'What do I do here?
        End Try

    End Sub

    '<TestMethod()> _
    'Public Sub ResetToNStatusTest()
    '    testParameters.DBServer = "NGLRDP06D"
    '    testParameters.Database = "SHIELDSMASUnitTest"

    '    Dim oBook As New DAL.NGLBookData(testParameters)
    '    Dim target As New DAL.NGLBookRevenueData(testParameters)

    '    Dim BookControl As Integer = 0
    '    Dim BookLoadControl As Integer = 0
    '    Dim BookItemControl As Integer = 0

    '    Dim BookProBase As String = "12345"
    '    Dim BookProPrefix As String = "YAK"
    '    Dim BookConsPrefix As String = "CNS270088"
    '    Dim BookProNumber As String = ""
    '    Dim BookCarrOrderNumber As String = "LVVUnitTESTPO"
    '    Dim LaneNumber As String = "1-S74600"
    '    Dim BookTranCode As String = "P"
    '    Dim BookSHID As String = ""
    '    Dim ModUser As String = "RTNS Unit Test"

    '    'Create the test records by calling the stored procedure
    '    Dim retControls = oBook.CreateSampleOrderUnitTestRTNS(BookProBase, BookProPrefix, BookConsPrefix, BookProNumber, BookCarrOrderNumber, LaneNumber, BookTranCode, BookSHID, ModUser)
    '    BookControl = retControls.BookControl
    '    BookLoadControl = retControls.BookLoadControl
    '    BookItemControl = retControls.BookItemControl

    '    Dim b = target.GetBookRevenueWDetailsFiltered(BookControl)

    '    'Make sure than each field is populated so we know we have something to test
    '    Assert.AreNotEqual("N", b.BookTranCode)
    '    Assert.AreNotEqual(Nothing, b.BookSHID)
    '    'check Financial Information
    '    Assert.AreNotEqual(False, b.BookLockAllCosts)
    '    Assert.AreNotEqual(0, b.BookSpotRateAllocationFormula)
    '    Assert.AreNotEqual(0, b.BookSpotRateTotalUnallocatedLineHaul)
    '    Assert.AreNotEqual(False, b.BookSpotRateUseCarrierFuelAddendum)
    '    Assert.AreNotEqual(False, b.BookSpotRateUseFuelAddendum)
    '    Assert.AreNotEqual(0, b.BookRevCarrierCost)
    '    Assert.AreNotEqual(0, b.BookRevLineHaul)
    '    Assert.AreNotEqual(0, b.BookRevDiscount)
    '    Assert.AreNotEqual(0, b.BookRevNetCost)
    '    Assert.AreNotEqual(0, b.BookRevFreightTax)
    '    Assert.AreNotEqual(0, b.BookRevTotalCost)
    '    Assert.AreNotEqual(0, b.BookRevOtherCost)
    '    Assert.AreNotEqual(0, b.BookRevLoadSavings)
    '    Assert.AreNotEqual(0, b.BookRevGrossRevenue)
    '    Assert.AreNotEqual(0, b.BookRevCommCost)
    '    Assert.AreNotEqual(0, b.BookFinAPStdCost)
    '    Assert.AreNotEqual(0, b.BookFinARBookFrt)
    '    Assert.AreNotEqual(0, b.BookFinCommStd)
    '    Assert.AreNotEqual(0, b.BookFinServiceFee)

    '    If Not b.BookLockBFCCost And b.CompFinUseImportFrtCost = False Then
    '        Assert.AreNotEqual(0, b.BookRevBilledBFC)
    '        Assert.AreNotEqual(0, b.BookTotalBFC)
    '        Assert.AreNotEqual(True, b.BookSpotRateAutoCalcBFC)
    '        Assert.AreNotEqual(0, b.BookSpotRateBFCAllocationFormula)
    '        Assert.AreNotEqual(0, b.BookSpotRateTotalUnallocatedBFC)
    '    End If
    '    'check BookShip info (assigned carrier info)
    '    Assert.AreNotEqual(Nothing, b.BookShipCarrierDetails)
    '    Assert.AreNotEqual(Nothing, b.BookShipCarrierName)
    '    Assert.AreNotEqual(Nothing, b.BookShipCarrierNumber)
    '    Assert.AreNotEqual(0, b.BookShipCarrierProControl)
    '    Assert.AreNotEqual(Nothing, b.BookShipCarrierProNumber)
    '    Assert.AreNotEqual(Nothing, b.BookShipCarrierProNumberRaw)
    '    'check Carrier Information
    '    Assert.AreNotEqual(0, b.BookCarrierControl)
    '    Assert.AreNotEqual(0, b.BookCarrierContControl)
    '    Assert.AreNotEqual(Nothing, b.BookCarrierContact)
    '    Assert.AreNotEqual(Nothing, b.BookCarrierContactPhone)
    '    'check Tariff Information
    '    Assert.AreNotEqual(0, b.BookCarrTarControl)
    '    Assert.AreNotEqual(0, b.BookCarrTarEquipControl)
    '    Assert.AreNotEqual(0, b.BookCarrTarEquipMatControl)
    '    Assert.AreNotEqual(0, b.BookCarrTarEquipMatDetControl)
    '    Assert.AreNotEqual(0, b.BookCarrTarEquipMatDetID)
    '    Assert.AreNotEqual(0, b.BookCarrTarEquipMatDetValue)
    '    Assert.AreNotEqual(Nothing, b.BookCarrTarEquipMatName)
    '    Assert.AreNotEqual(Nothing, b.BookCarrTarEquipName)
    '    Assert.AreNotEqual(Nothing, b.BookCarrTarName)
    '    Assert.AreNotEqual(0, b.BookCarrTarRevisionNumber)
    '    Assert.AreNotEqual(0, b.BookCarrTruckControl)
    '    Assert.AreNotEqual(Nothing, b.BookMustLeaveByDateTime)
    '    Assert.AreNotEqual(Nothing, b.BookExpDelDateTime)
    '    'tariff engine fields
    '    Assert.AreNotEqual(0, b.BookBestDeficitCost)
    '    Assert.AreNotEqual(0, b.BookBestDeficitWeight)
    '    Assert.AreNotEqual(0, b.BookBestDeficitWeightBreak)
    '    Assert.AreNotEqual(0, b.BookRatedWeightBreak)
    '    Assert.AreNotEqual(0, b.BookWgtAdjCost)
    '    Assert.AreNotEqual(0, b.BookWgtAdjWeight)
    '    Assert.AreNotEqual(0, b.BookWgtAdjWeightBreak)
    '    Assert.AreNotEqual(0, b.BookBilledLoadWeight)
    '    Assert.AreNotEqual(0, b.BookMinAdjustedLoadWeight)
    '    Assert.AreNotEqual(0, b.BookSummedClassWeight)
    '    Assert.AreNotEqual(0, b.BookWgtRoundingVariance)
    '    Assert.AreNotEqual("", b.BookHeaviestClass)
    '    Assert.AreNotEqual(0, b.BookAcutalHeaviestClassWeight)
    '    Assert.AreNotEqual(0, b.BookRevDiscountRate)
    '    Assert.AreNotEqual(0, b.BookRevDiscountMin)

    '    '*******FIGURE OUT HOW TO TEST THIS ****************
    '    ''Clear any Accessorial Fees
    '    'If Not Me.BookFees Is Nothing AndAlso Me.BookFees.Count > 0 Then
    '    '    Dim FeesToRemove As New List(Of BookFee)
    '    '    'bookRevenue.BookFees.RemoveAll(x => x.BookFeesAccessorialFeeTypeControl == (int)feetype); 
    '    '    Me.BookFees.RemoveAll(Function(x) x.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Tariff)
    '    '    For Each f In Me.BookFees
    '    '        'If f.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Tariff Then
    '    '        '    FeesToRemove.Add(f)
    '    '        'Else
    '    '        f.BookFeesValue = 0
    '    '        'End If
    '    '    Next
    '    '    'If Not FeesToRemove Is Nothing AndAlso FeesToRemove.Count > 0 Then
    '    '    '    For Each f In FeesToRemove
    '    '    '        If Me.BookFees.Contains(f) Then Me.BookFees.Remove(f)
    '    '    '    Next
    '    '    'End If
    '    'End If

    '    If Not b.BookLoads Is Nothing AndAlso b.BookLoads.Count > 0 Then
    '        For Each bload In b.BookLoads
    '            Assert.AreNotEqual(0, bload.BookLoadTotCost)
    '            If Not b.BookLockBFCCost And b.CompFinUseImportFrtCost = False Then Assert.AreNotEqual(0, bload.BookLoadBFC)
    '            If Not bload.BookItems Is Nothing AndAlso bload.BookItems.Count > 0 Then
    '                For Each bItem In bload.BookItems
    '                    Assert.AreNotEqual(0, bItem.BookItemCarrTarEquipMatDetValue)
    '                    Assert.AreNotEqual(0, bItem.BookItemCarrTarEquipMatDetID)
    '                    Assert.AreNotEqual(Nothing, bItem.BookItemCarrTarEquipMatName)
    '                    Assert.AreNotEqual(0, bItem.BookItemCarrTarEquipMatControl)
    '                    Assert.AreNotEqual(Nothing, bItem.BookItemRatedFAKClass)
    '                    Assert.AreNotEqual(Nothing, bItem.BookItemRatedNMFCClass)
    '                    Assert.AreNotEqual(Nothing, bItem.BookItemRatedMarineCode)
    '                    Assert.AreNotEqual(Nothing, bItem.BookItemRatedDOTCode)
    '                    Assert.AreNotEqual(Nothing, bItem.BookItemRatedIATACode)
    '                    Assert.AreNotEqual(Nothing, bItem.BookItemRated49CFRCode)
    '                    Assert.AreNotEqual(Nothing, bItem.BookItemDeficitFAKClass)
    '                    Assert.AreNotEqual(Nothing, bItem.BookItemDeficitNMFCClass)
    '                    Assert.AreNotEqual(Nothing, bItem.BookItemDeficitMarineCode)
    '                    Assert.AreNotEqual(Nothing, bItem.BookItemDeficitDOTCode)
    '                    Assert.AreNotEqual(Nothing, bItem.BookItemDeficitIATACode)
    '                    Assert.AreNotEqual(Nothing, bItem.BookItemDeficit49CFRCode)
    '                    Assert.AreNotEqual(0, bItem.BookItemWeightBreak)
    '                    Assert.AreNotEqual(0, bItem.BookItemDeficitWeightAdjustment)
    '                    Assert.AreNotEqual(0, bItem.BookItemDeficitCostAdjustment)
    '                    Assert.AreNotEqual(0, bItem.BookItemNonTaxableFees)
    '                    Assert.AreNotEqual(0, bItem.BookItemTaxes)
    '                    Assert.AreNotEqual(0, bItem.BookItemTaxableFees)
    '                    Assert.AreNotEqual(0, bItem.BookItemLineHaul)
    '                    Assert.AreNotEqual(0, bItem.BookItemDiscount)
    '                    Assert.AreNotEqual(0, bItem.BookItemFreightCost)
    '                    If Not b.BookLockBFCCost And b.CompFinUseImportFrtCost = False Then Assert.AreNotEqual(0, bItem.BookItemBFC)
    '                Next
    '            End If
    '        Next
    '    End If

    '    'Now call the original method ResetToNStatus()
    '    b.ResetToNStatus()
    '    Dim oBookRevs As DTO.BookRevenue()
    '    oBookRevs(0) = b
    '    target.SaveRevenuesNoReturn(oBookRevs, True, True)

    '    'get the updated record back from the data base
    '    b = target.GetBookRevenueWDetailsFiltered(BookControl)

    '    'Verify that this worked
    '    Assert.AreEqual("N", b.BookTranCode)
    '    Assert.AreEqual(Nothing, b.BookSHID)
    '    'check Financial Information
    '    Assert.AreEqual(False, b.BookLockAllCosts)
    '    Assert.AreEqual(0, b.BookSpotRateAllocationFormula)
    '    Assert.AreEqual(0, b.BookSpotRateTotalUnallocatedLineHaul)
    '    Assert.AreEqual(False, b.BookSpotRateUseCarrierFuelAddendum)
    '    Assert.AreEqual(False, b.BookSpotRateUseFuelAddendum)
    '    Assert.AreEqual(0, b.BookRevCarrierCost)
    '    Assert.AreEqual(0, b.BookRevLineHaul)
    '    Assert.AreEqual(0, b.BookRevDiscount)
    '    Assert.AreEqual(0, b.BookRevNetCost)
    '    Assert.AreEqual(0, b.BookRevFreightTax)
    '    Assert.AreEqual(0, b.BookRevTotalCost)
    '    Assert.AreEqual(0, b.BookRevOtherCost)
    '    Assert.AreEqual(0, b.BookRevLoadSavings)
    '    Assert.AreEqual(0, b.BookRevGrossRevenue)
    '    Assert.AreEqual(0, b.BookRevCommCost)
    '    Assert.AreEqual(0, b.BookFinAPStdCost)
    '    Assert.AreEqual(0, b.BookFinARBookFrt)
    '    Assert.AreEqual(0, b.BookFinCommStd)
    '    Assert.AreEqual(0, b.BookFinServiceFee)

    '    If Not b.BookLockBFCCost And b.CompFinUseImportFrtCost = False Then
    '        Assert.AreEqual(0, b.BookRevBilledBFC)
    '        Assert.AreEqual(0, b.BookTotalBFC)
    '        Assert.AreEqual(True, b.BookSpotRateAutoCalcBFC)
    '        Assert.AreEqual(0, b.BookSpotRateBFCAllocationFormula)
    '        Assert.AreEqual(0, b.BookSpotRateTotalUnallocatedBFC)
    '    End If
    '    'check BookShip info (assigned carrier info)
    '    Assert.AreEqual(Nothing, b.BookShipCarrierDetails)
    '    Assert.AreEqual(Nothing, b.BookShipCarrierName)
    '    Assert.AreEqual(Nothing, b.BookShipCarrierNumber)
    '    Assert.AreEqual(0, b.BookShipCarrierProControl)
    '    Assert.AreEqual(Nothing, b.BookShipCarrierProNumber)
    '    Assert.AreEqual(Nothing, b.BookShipCarrierProNumberRaw)
    '    'check Carrier Information
    '    Assert.AreEqual(0, b.BookCarrierControl)
    '    Assert.AreEqual(0, b.BookCarrierContControl)
    '    Assert.AreEqual(Nothing, b.BookCarrierContact)
    '    Assert.AreEqual(Nothing, b.BookCarrierContactPhone)
    '    'check Tariff Information
    '    Assert.AreEqual(0, b.BookCarrTarControl)
    '    Assert.AreEqual(0, b.BookCarrTarEquipControl)
    '    Assert.AreEqual(0, b.BookCarrTarEquipMatControl)
    '    Assert.AreEqual(0, b.BookCarrTarEquipMatDetControl)
    '    Assert.AreEqual(0, b.BookCarrTarEquipMatDetID)
    '    Assert.AreEqual(0, b.BookCarrTarEquipMatDetValue)
    '    Assert.AreEqual(Nothing, b.BookCarrTarEquipMatName)
    '    Assert.AreEqual(Nothing, b.BookCarrTarEquipName)
    '    Assert.AreEqual(Nothing, b.BookCarrTarName)
    '    Assert.AreEqual(0, b.BookCarrTarRevisionNumber)
    '    Assert.AreEqual(0, b.BookCarrTruckControl)
    '    Assert.AreEqual(Nothing, b.BookMustLeaveByDateTime)
    '    Assert.AreEqual(Nothing, b.BookExpDelDateTime)
    '    'tariff engine fields
    '    Assert.AreEqual(0, b.BookBestDeficitCost)
    '    Assert.AreEqual(0, b.BookBestDeficitWeight)
    '    Assert.AreEqual(0, b.BookBestDeficitWeightBreak)
    '    Assert.AreEqual(0, b.BookRatedWeightBreak)
    '    Assert.AreEqual(0, b.BookWgtAdjCost)
    '    Assert.AreEqual(0, b.BookWgtAdjWeight)
    '    Assert.AreEqual(0, b.BookWgtAdjWeightBreak)
    '    Assert.AreEqual(0, b.BookBilledLoadWeight)
    '    Assert.AreEqual(0, b.BookMinAdjustedLoadWeight)
    '    Assert.AreEqual(0, b.BookSummedClassWeight)
    '    Assert.AreEqual(0, b.BookWgtRoundingVariance)
    '    Assert.AreEqual("", b.BookHeaviestClass)
    '    Assert.AreEqual(0, b.BookAcutalHeaviestClassWeight)
    '    Assert.AreEqual(0, b.BookRevDiscountRate)
    '    Assert.AreEqual(0, b.BookRevDiscountMin)

    '    '*******FIGURE OUT HOW TO TEST THIS ****************
    '    ''Clear any Accessorial Fees
    '    'If Not Me.BookFees Is Nothing AndAlso Me.BookFees.Count > 0 Then
    '    '    Dim FeesToRemove As New List(Of BookFee)
    '    '    'bookRevenue.BookFees.RemoveAll(x => x.BookFeesAccessorialFeeTypeControl == (int)feetype); 
    '    '    Me.BookFees.RemoveAll(Function(x) x.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Tariff)
    '    '    For Each f In Me.BookFees
    '    '        'If f.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Tariff Then
    '    '        '    FeesToRemove.Add(f)
    '    '        'Else
    '    '        f.BookFeesValue = 0
    '    '        'End If
    '    '    Next
    '    '    'If Not FeesToRemove Is Nothing AndAlso FeesToRemove.Count > 0 Then
    '    '    '    For Each f In FeesToRemove
    '    '    '        If Me.BookFees.Contains(f) Then Me.BookFees.Remove(f)
    '    '    '    Next
    '    '    'End If
    '    'End If

    '    If Not b.BookLoads Is Nothing AndAlso b.BookLoads.Count > 0 Then
    '        For Each bload In b.BookLoads
    '            Assert.AreEqual(0, bload.BookLoadTotCost)
    '            If Not b.BookLockBFCCost And b.CompFinUseImportFrtCost = False Then Assert.AreEqual(0, bload.BookLoadBFC)
    '            If Not bload.BookItems Is Nothing AndAlso bload.BookItems.Count > 0 Then
    '                For Each bItem In bload.BookItems
    '                    Assert.AreEqual(0, bItem.BookItemCarrTarEquipMatDetValue)
    '                    Assert.AreEqual(0, bItem.BookItemCarrTarEquipMatDetID)
    '                    Assert.AreEqual(Nothing, bItem.BookItemCarrTarEquipMatName)
    '                    Assert.AreEqual(0, bItem.BookItemCarrTarEquipMatControl)
    '                    Assert.AreEqual(Nothing, bItem.BookItemRatedFAKClass)
    '                    Assert.AreEqual(Nothing, bItem.BookItemRatedNMFCClass)
    '                    Assert.AreEqual(Nothing, bItem.BookItemRatedMarineCode)
    '                    Assert.AreEqual(Nothing, bItem.BookItemRatedDOTCode)
    '                    Assert.AreEqual(Nothing, bItem.BookItemRatedIATACode)
    '                    Assert.AreEqual(Nothing, bItem.BookItemRated49CFRCode)
    '                    Assert.AreEqual(Nothing, bItem.BookItemDeficitFAKClass)
    '                    Assert.AreEqual(Nothing, bItem.BookItemDeficitNMFCClass)
    '                    Assert.AreEqual(Nothing, bItem.BookItemDeficitMarineCode)
    '                    Assert.AreEqual(Nothing, bItem.BookItemDeficitDOTCode)
    '                    Assert.AreEqual(Nothing, bItem.BookItemDeficitIATACode)
    '                    Assert.AreEqual(Nothing, bItem.BookItemDeficit49CFRCode)
    '                    Assert.AreEqual(0, bItem.BookItemWeightBreak)
    '                    Assert.AreEqual(0, bItem.BookItemDeficitWeightAdjustment)
    '                    Assert.AreEqual(0, bItem.BookItemDeficitCostAdjustment)
    '                    Assert.AreEqual(0, bItem.BookItemNonTaxableFees)
    '                    Assert.AreEqual(0, bItem.BookItemTaxes)
    '                    Assert.AreEqual(0, bItem.BookItemTaxableFees)
    '                    Assert.AreEqual(0, bItem.BookItemLineHaul)
    '                    Assert.AreEqual(0, bItem.BookItemDiscount)
    '                    Assert.AreEqual(0, bItem.BookItemFreightCost)
    '                    If Not b.BookLockBFCCost And b.CompFinUseImportFrtCost = False Then Assert.AreEqual(0, bItem.BookItemBFC)
    '                Next
    '            End If
    '        Next
    '    End If


    '    '*************** DON'T FORGET TO DELETE THE TEST RECORDS *********************************

    '    oBook.DeleteSampleOrderUnitTest(BookControl, BookLoadControl, BookItemControl)


    '    'call sp to create sample data

    '    'To test sp:
    '    'check to make sure test fields are populated
    '    'call sp and then check to make sure fields are reset to 0 or whatever

    '    'other test:
    '    'call original ResetToNStatus() method and store results
    '    'call sp and store results
    '    'check to make sure they returned the same things
    '    'For Each b In oBookRevs
    '    '    b.ResetToNStatus()
    '    'Next

    'End Sub



    <TestMethod()> _
    Public Sub ManualTenderTest()
        testParameters.DBServer = "NGLRDP07D"
        testParameters.Database = "NGLMASPRODBON"

        Dim oBook As New DAL.NGLBookData(testParameters)
        Dim target As New DAL.NGLBookRevenueData(testParameters)
        Dim oBookBLL As New NGLBookBLL(testParameters)

        Dim newTranCodeResult = oBookBLL.ProcessNewTransCode(13252, "PC", "P", 0)

        If newTranCodeResult.Success = False Then
            'we were unable to optimize the data because we could not reset all the records to N status
            Assert.Fail("TranCode Update Failed")
            Return
        End If


    End Sub


    '''<summary>
    '''A test for AccessorialFeeValidation
    '''</summary>
    <TestMethod()> _
    Public Sub AccessorialFeeValidationTest()
        Dim strTestName As String = "AccessorialFeeValidationTest"
        testParameters.DBServer = "NGLRDP06D"
        testParameters.Database = "HLFCA705"
        testParameters.UserName = "ngl\rramsey"
        Dim target As New DAL.NGLBookRevenueData(testParameters)
        Dim Control As Integer = 575123
        Try
            'get the book reve records 
            Dim oRecords() As DTO.BookRevenue = target.GetBookRevenuesWDetailsFiltered(Control)
            If oRecords Is Nothing OrElse oRecords.Count < 1 Then
                Assert.Fail(strTestName & " get Book Revenues With Details Data Failed for Book Control {0} ", Control)
                Return
            End If
            Dim FeesValidationComplete As Boolean = target.AccessorialFeeValidation(oRecords.ToList())
            System.Diagnostics.Debug.WriteLine("Book Control | Fee Control | FeeAllocationType | OverRidden | FeeTypeControl | Minimum")
            For Each b In oRecords
                For Each f In b.BookFees.Where(Function(x) x.BookFeesAccessorialCode = 10)
                    System.Diagnostics.Debug.WriteLine("{0} | {1} | {2} | {3} | {4} | {5}", b.BookControl, f.BookFeesControl, f.BookFeesAccessorialFeeAllocationTypeControl, f.BookFeesOverRidden, f.BookFeesAccessorialFeeTypeControl, f.BookFeesMinimum)
                Next
            Next

            System.Diagnostics.Debug.WriteLine(strTestName & " Book Revenue Records Found: " & oRecords.Count())
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & strTestName & ": {0} ", ex.Message)
        Finally
            'place clean up code here
        End Try
    End Sub


    '''<summary>
    '''A test for AccessorialFeeOverrideTest
    '''</summary>
    <TestMethod()> _
    Public Sub AccessorialFeeOverrideTest()
        Dim strTestName As String = "AccessorialFeeOverrideTest"
        testParameters.DBServer = "NGLRDP06D"
        testParameters.Database = "HLFCA705"
        testParameters.UserName = "ngl\rramsey"
        Dim DALTarget As New DAL.NGLBookRevenueData(testParameters)
        Dim BLLTarget As New NGLBookFeesBLL(testParameters)
        Dim bookPronumber = "HCS255655"
        Dim Control As Integer = 575123
        Try
            'get the book reve records 
            Dim oRecords() As DTO.BookRevenue = DALTarget.GetBookRevenuesWDetailsFiltered(Control)
            If oRecords Is Nothing OrElse oRecords.Count < 1 Then
                Assert.Fail(strTestName & " get Book Revenues With Details Data Failed for Book Control {0} ", Control)
                Return
            End If

            'Dim FeesValidationComplete As Boolean = DALTarget.AccessorialFeeValidation(oRecords.ToList())
            'System.Diagnostics.Debug.WriteLine(strTestName & " Book Revenue Records Found: " & oRecords.Count())
            Dim oFee As New DTO.BookFee
            Dim book = oRecords.Where(Function(x) x.BookControl = Control).FirstOrDefault()
            oFee = ( _
                From d In book.BookFees _
                Where d.BookFeesAccessorialCode = 10 And d.BookFeesOverRidden = 0 _
                 Order By d.BookFeesModDate Descending _
                Select d).FirstOrDefault()
            oFee.BookFeesMinimum = 0
            oFee.BookFeesAccessorialFeeAllocationTypeControl = 1
            oFee.BookFeesValue = 0

            Dim oQuickReturn = BLLTarget.UpdateBookFeeQuick(oFee)
            System.Diagnostics.Debug.WriteLine(strTestName & " Book Revenue Records Found: " & oRecords.Count())



        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & strTestName & ": {0} ", ex.Message)
        Finally
            'place clean up code here
        End Try
    End Sub

    <TestMethod()>
    Public Sub GetBookAPCheckEntryRecordsTest()
        Try
            Dim oBook As New DAL.NGLBookData(testParameters)
            Dim CarrierControl As Integer = 15489
            Dim DateStart As Nullable(Of Date) = "#12:00:00 AM#"
            Dim DateEnd As Nullable(Of Date) = "#12:00:00 AM#"
            Dim oData = oBook.GetBookAPCheckEntryRecords(CarrierControl, DateStart, DateEnd)
            'If oData Is Nothing OrElse oData.BookControl <> intBookControl Then
            '    Assert.Fail("Read data failure")
            'End If
            Assert.Fail("Read data failure")
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("DoRateShopTest Unexpected Error: " & ex.Message)
        Finally

        End Try
    End Sub

    <TestMethod()>
    Public Sub GetAcceptRejectEmailsFromCompTest()
        Try
            testParameters.Database = "NGLMASDEV7051"
            testParameters.DBServer = "ROBWIN8LAP\SQL2016"
            Dim oBook As New DAL.NGLBookData(testParameters)
            Dim BookControl As Integer = 902599 ' 902698
            Dim dictEmails = oBook.GetAcceptRejectEmailsFromComp(BookControl)
            If dictEmails Is Nothing Then
                Assert.Fail("GetAcceptRejectEmailsFromCompTest Read data failure")
            End If


            For Each d In dictEmails
                System.Diagnostics.Debug.WriteLine("key {0}; value {1}", d.Key, d.Value)
            Next

        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("GetAcceptRejectEmailsFromCompTest Unexpected Error: " & ex.Message)
        Finally

        End Try
    End Sub


    <TestMethod()>
    Public Sub GetUniqueOrdersBySHIDTest()
        testParameters.DBServer = "NGLRDP07D"
        testParameters.Database = "NGLMASPROD"

        Dim oBook As New DAL.NGLBookData(testParameters)

        Dim newTranCodeResult = oBook.GetUniqueOrdersBySHID("CNS-1-1316")

        If newTranCodeResult?.Count < 1 Then
            'we were unable to optimize the data because we could not reset all the records to N status
            Assert.Fail("TranCode Update Failed")
            Return
        End If


    End Sub




    <TestMethod()>
    Public Sub GetFeeAutoApprovedMessageTest()
        testParameters.DBServer = "DESKTOP-0R0EJUB"
        testParameters.Database = "NGLMASBONPROD"

        Dim oBookFeesPending As New DAL.NGLBookFeePendingData(testParameters)

        Dim BookControl As Integer = 184544
        Dim AccessorialCode As Integer = 15
        Dim AllocationTypeControl As Integer = 4
        Dim BookFeePendingControl As Integer = 2045
        Dim PendingAmt As Decimal = 418

        Dim ret = oBookFeesPending.GetFeeAutoApprovedMessage(BookControl, AccessorialCode, PendingAmt, BookFeePendingControl)

        System.Diagnostics.Debug.WriteLine(ret)
        Console.WriteLine(ret)

    End Sub

End Class
