Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports System
Imports NGL.FM.CarTar
Imports DAL = NGL.FreightMaster.Data
Imports DTO = NGL.FreightMaster.Data.DataTransferObjects
Imports BLL = NGL.FM.BLL

<TestClass()> Public Class SolutionDataTest
    Inherits TestBase

    <TestMethod> _
    Public Sub TestSolutionData()
        'connect to  database before getting any records overwrite the defaul
        testParameters.DBServer = "NGLRDP06D"
        testParameters.Database = "NGLMAS2013Dev"
        Dim blnRunNextTest As Boolean = True
        blnRunNextTest = TestLoadPlanningTruckData()
        If blnRunNextTest Then blnRunNextTest = TestNewBookingsForSolutionData()
        If blnRunNextTest Then blnRunNextTest = TestNewPOsForSolutionData()
    End Sub

    Private Function TestLoadPlanningTruckData() As Boolean
        Dim blnRet As Boolean = True

        Dim strTestName As String = "TestLoadPlanningTruckData"
        Try
            Dim target As New DAL.NGLLoadPlanningTruckData(testParameters)

            Dim CompControl As Integer = 9623
            Dim StartDateFilter As Date = "4/1/2016"
            Dim SampleOrderDate As Date = "4/15/2016"
            Dim StopDateFilter As Date = "4/30/2016"
            Dim UseLoadDateFilter As Boolean = True
            'Create some sample orders
            CreateTestOrder("LOD", 1111, "LOAD1234567", "SO-TstLoad2220", SampleOrderDate, "P")
            CreateTestOrder("LOD", 1112, "LOAD1234568", "SO-TstLoad2221", SampleOrderDate, "P")
            CreateTestOrder("LOD", 1113, "LOAD1234569", "SO-TstLoad2222", SampleOrderDate, "P")
            CreateTestOrder("LOD", 1114, "LOAD1234561", "SO-TstLoad2223", SampleOrderDate, "P")
            CreateTestOrder("LOD", 1115, "LOAD1234562", "SO-TstLoad2224", SampleOrderDate, "P")
            CreateTestOrder("LOD", 1116, "LOAD1234563", "SO-TstLoad2225", SampleOrderDate, "P")
            CreateTestOrder("LOD", 1117, "LOAD1234564", "SO-TstLoad2226", SampleOrderDate, "P")
            CreateTestOrder("LOD", 1118, "LOAD1234565", "SO-TstLoad2227", SampleOrderDate, "P")
            CreateTestOrder("LOD", 1119, "LOAD1234566", "SO-TstLoad2228", SampleOrderDate, "P")
            CreateTestOrder("LOD", 1120, "LOAD1234560", "SO-TstLoad2229", SampleOrderDate, "P")

            Dim TruckFilter As New DTO.LoadPlanningTruckDataFilter()
            With TruckFilter
                .CompControlFilter = CompControl
                .CarrierControlFilter = 0
                .StartDateFilter = StartDateFilter
                .StopDateFilter = StopDateFilter
                .OrigStartZipFilter = "0000000000"
                .OrigStopZipFilter = "9999999999"
                .DestStartZipFilter = "0000000000"
                .DestStopZipFilter = "9999999999"
                .OrigCityFilter = ""
                .DestCityFilter = ""
                .OrigSt1Filter = ""
                .OrigSt2Filter = ""
                .OrigSt3Filter = ""
                .OrigSt4Filter = ""
                .DestSt1Filter = ""
                .DestSt2Filter = ""
                .DestSt3Filter = ""
                .DestSt4Filter = ""
                .UseLoadDateFilter = UseLoadDateFilter
                .BookTransTypeFilter = ""
                .BookConsPrefixFilter = ""
                .LaneNumberFilter = ""
                .BookTranCodeFilter = ""
                .Page = 1
                .PageSize = 100
            End With
            Dim results As DTO.tblSolutionTruck() = target.GetLoadPlanningTrucksFiltered(TruckFilter)

            If results Is Nothing OrElse results.Count() < 1 Then
                Assert.Fail(strTestName & " Failed: No Trucks")
                Return False
            End If

            For Each r As DTO.tblSolutionTruck In results
                System.Diagnostics.Debug.WriteLine("Key: {0} Cases: {1} Wgt: {2} PL: {3} Cube: {4} PX: {5} BFC: {6} Key: {7} Lanes: {8} Lane Numbers: {9} Notes: {10}", r.SolutionTruckKey, r.TotalCases, r.TotalWgt, r.TotalPlts, r.TotalCubes, r.SolutionTruckTotalPX, r.SolutionTruckTotalBFC, r.Stops, r.SolutionTruckLaneNames, r.SolutionTruckLaneNumbers, r.SolutionTruckBookNotes)
            Next
            Dim skey As String = results(results.Count() - 1).SolutionTruckKey
            Dim result As DTO.tblSolutionTruck = target.GetLoadPlanningTruckFiltered(TruckFilter.CompControlFilter, skey)

            If result Is Nothing Then
                Assert.Fail("{0} Failed: No Truck for Key: {1}", strTestName, skey)
                Return False
            End If

            System.Diagnostics.Debug.WriteLine("Key: {0} Cases: {1} Wgt: {2} PL: {3} Cube: {4} PX: {5} BFC: {6} Key: {7} Lanes: {8} Lane Numbers: {9} Notes: {10}", result.SolutionTruckKey, result.TotalCases, result.TotalWgt, result.TotalPlts, result.TotalCubes, result.SolutionTruckTotalPX, result.SolutionTruckTotalBFC, result.Stops, result.SolutionTruckLaneNames, result.SolutionTruckLaneNumbers, result.SolutionTruckBookNotes)
            Dim BookDateLoadFilter As Date = SampleOrderDate.AddDays(3) '2016-04-15 00:00:00.000
            Dim BookRouteGuideControlFilter As Integer = 0
            Dim BookRouteTypeCodeFilter As Integer = 6
            results = target.GetMatchingRoutedLoads(TruckFilter.CompControlFilter, 0, BookDateLoadFilter, "", "P", BookRouteGuideControlFilter, BookRouteTypeCodeFilter)
            If results Is Nothing OrElse results.Count() < 1 Then
                Assert.Fail(strTestName & " Failed: No  Matching Trucks")
                Return False
            End If

            For Each r As DTO.tblSolutionTruck In results
                System.Diagnostics.Debug.WriteLine("Key: {0} Cases: {1} Wgt: {2} PL: {3} Cube: {4} PX: {5} BFC: {6} Key: {7} Lanes: {8} Lane Numbers: {9} Notes: {10}", r.SolutionTruckKey, r.TotalCases, r.TotalWgt, r.TotalPlts, r.TotalCubes, r.SolutionTruckTotalPX, r.SolutionTruckTotalBFC, r.Stops, r.SolutionTruckLaneNames, r.SolutionTruckLaneNumbers, r.SolutionTruckBookNotes)
            Next

            System.Diagnostics.Debug.WriteLine("Success!")

        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & strTestName & ": {0} ", ex.Message)
        Finally
            'place clean up code here
        End Try
        Return blnRet
    End Function


    Private Function TestNewBookingsForSolutionData() As Boolean
        Dim blnRet As Boolean = True

        Dim strTestName As String = "TestNewBookingsForSolutionData"
        Try
            Dim target As New DAL.NGLNewBookingsForSolutionData(testParameters)


            Dim CompControl As Integer = 9623
            Dim StartDateFilter As Date = "4/1/2016"
            Dim SampleOrderDate As Date = "4/12/2016"
            Dim StopDateFilter As Date = "4/30/2016"
            Dim UseLoadDateFilter As Boolean = True
            'Create some sample orders
            CreateTestOrder("Rob", 1111, "TST1234567", "SO-TstNew2220", SampleOrderDate, "N")
            CreateTestOrder("Rob", 1112, "TST1234568", "SO-TstNew2221", SampleOrderDate, "N")
            CreateTestOrder("Rob", 1113, "TST1234569", "SO-TstNew2222", SampleOrderDate, "N")
            CreateTestOrder("Rob", 1114, "TST1234561", "SO-TstNew2223", SampleOrderDate, "N")
            CreateTestOrder("Rob", 1115, "TST1234562", "SO-TstNew2224", SampleOrderDate, "N")
            CreateTestOrder("Rob", 1116, "TST1234563", "SO-TstNew2225", SampleOrderDate, "N")
            CreateTestOrder("Rob", 1117, "TST1234564", "SO-TstNew2226", SampleOrderDate, "N")
            CreateTestOrder("Rob", 1118, "TST1234565", "SO-TstNew2227", SampleOrderDate, "N")
            CreateTestOrder("Rob", 1119, "TST1234566", "SO-TstNew2228", SampleOrderDate, "N")
            CreateTestOrder("Rob", 1120, "TST1234560", "SO-TstNew2229", SampleOrderDate, "N")

            Dim result As DTO.tblSolutionDetail = target.GetFirstRecord(0, CompControl, 0, StartDateFilter, StopDateFilter, UseLoadDateFilter, 0, 0)
            If result Is Nothing Then
                Assert.Fail(strTestName & " Get First Failed: No New Orders")
                Return False
            End If
            result = target.GetNextRecord(result.SolutionDetailBookControl, CompControl, 0, StartDateFilter, StopDateFilter, UseLoadDateFilter, 0, 0)
            If result Is Nothing Then
                Assert.Fail(strTestName & " Get Next Failed: Not Enough New Orders")
                Return False
            End If
            result = target.GetLastRecord(0, CompControl, 0, StartDateFilter, StopDateFilter, UseLoadDateFilter, 0, 0)

            If result Is Nothing Then
                Assert.Fail(strTestName & " Get Last Failed: Not Enough New Orders")
                Return False
            End If
            result = target.GetPreviousRecord(result.SolutionDetailBookControl, CompControl, 0, StartDateFilter, StopDateFilter, UseLoadDateFilter, 0, 0)

            If result Is Nothing Then
                Assert.Fail(strTestName & " Get Previous Failed: Not Enough New Orders")
                Return False
            End If

            result = target.GetNewBookingFiltered(result.SolutionDetailBookControl)

            If result Is Nothing Then
                Assert.Fail(strTestName & " Get New Booking Failed: Book Control Missing")
                Return False
            End If

            result = target.GetNewBookingFilteredByPro(result.SolutionDetailProNumber)

            If result Is Nothing Then
                Assert.Fail(strTestName & " Get New Booking by Pro Failed: Pro Number Missing")
                Return False
            End If


            Dim TruckFilter As New DTO.LoadPlanningTruckDataFilter()
            With TruckFilter
                .CompControlFilter = CompControl
                .CarrierControlFilter = 0
                .StartDateFilter = StartDateFilter
                .StopDateFilter = StopDateFilter
                .OrigStartZipFilter = "0000000000"
                .OrigStopZipFilter = "9999999999"
                .DestStartZipFilter = "0000000000"
                .DestStopZipFilter = "9999999999"
                .OrigCityFilter = ""
                .DestCityFilter = ""
                .OrigSt1Filter = ""
                .OrigSt2Filter = ""
                .OrigSt3Filter = ""
                .OrigSt4Filter = ""
                .DestSt1Filter = ""
                .DestSt2Filter = ""
                .DestSt3Filter = ""
                .DestSt4Filter = ""
                .UseLoadDateFilter = UseLoadDateFilter
                .BookTransTypeFilter = ""
                .BookConsPrefixFilter = ""
                .LaneNumberFilter = ""
                .BookTranCodeFilter = ""
                .Page = 1
                .PageSize = 100
            End With
            Dim results As DTO.tblSolutionDetail() = target.GetNewBookingsFiltered(TruckFilter)

            If results Is Nothing OrElse results.Count() < 1 Then
                Assert.Fail(strTestName & " Get New Bookings Failed: Not Records for Truck Filter")
                Return False
            End If
            For Each r As DTO.tblSolutionDetail In results
                System.Diagnostics.Debug.WriteLine("Comp {0} Cases: {1} Wgt: {2} PL: {3} Cube: {4} PX: {5} BFC: {6} Key: {7} Lanes: {8} Lane Numbers: {9} Notes: {10}", r.SolutionDetailCompNumber, r.SolutionDetailTotalCases, r.SolutionDetailTotalWgt, r.SolutionDetailTotalPL, r.SolutionDetailTotalCube, r.SolutionDetailTotalPX, r.SolutionDetailTotalBFC, r.SolutionDetailStopNo, r.SolutionDetailLaneNumber, r.SolutionDetailLaneNumber, r.SolutionDetailBookNotes)
            Next

            System.Diagnostics.Debug.WriteLine("Success!")

        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & strTestName & ": {0} ", ex.Message)
        Finally
            'place clean up code here
        End Try
        Return blnRet
    End Function


    Private Function TestNewPOsForSolutionData() As Boolean
        Dim blnRet As Boolean = True

        Dim strTestName As String = "TestNewPOsForSolutionData"
        Try
            Dim target As New DAL.NGLNewPOsForSolutionData(testParameters)


            Dim CompControl As Integer = 9623
            Dim StartDateFilter As Date = "4/1/2016"
            Dim SampleOrderDate As Date = "4/12/2016"
            Dim StopDateFilter As Date = "4/30/2016"
            Dim UseLoadDateFilter As Boolean = True
            'Create some sample orders
            CreateTestPOs(SampleOrderDate)

            Dim result As DTO.tblSolutionDetail = target.GetFirstRecord(0, CompControl, StartDateFilter, StopDateFilter, UseLoadDateFilter, 0, 0)
            If result Is Nothing Then
                Assert.Fail(strTestName & " Get First Failed: No New POs")
                Return False
            End If
            result = target.GetNextRecord(result.SolutionDetailPOHdrControl, CompControl, StartDateFilter, StopDateFilter, UseLoadDateFilter, 0, 0)
            If result Is Nothing Then
                Assert.Fail(strTestName & " Get Next Failed: Not Enough New POs")
                Return False
            End If
            result = target.GetLastRecord(0, CompControl, StartDateFilter, StopDateFilter, UseLoadDateFilter, 0, 0)

            If result Is Nothing Then
                Assert.Fail(strTestName & " Get Last Failed: Not Enough New POs")
                Return False
            End If
            result = target.GetPreviousRecord(result.SolutionDetailPOHdrControl, CompControl, StartDateFilter, StopDateFilter, UseLoadDateFilter, 0, 0)

            If result Is Nothing Then
                Assert.Fail(strTestName & " Get Previous Failed: Not Enough New POs")
                Return False
            End If

            result = target.GetNewPOFiltered(result.SolutionDetailPOHdrControl)

            If result Is Nothing Then
                Assert.Fail(strTestName & " Get New PO Filtered Failed: POHdr Control Missing")
                Return False
            End If

            Dim TruckFilter As New DTO.LoadPlanningTruckDataFilter()
            With TruckFilter
                .CompControlFilter = CompControl
                .CarrierControlFilter = 0
                .StartDateFilter = StartDateFilter
                .StopDateFilter = StopDateFilter
                .OrigStartZipFilter = "0000000000"
                .OrigStopZipFilter = "9999999999"
                .DestStartZipFilter = "0000000000"
                .DestStopZipFilter = "9999999999"
                .OrigCityFilter = ""
                .DestCityFilter = ""
                .OrigSt1Filter = ""
                .OrigSt2Filter = ""
                .OrigSt3Filter = ""
                .OrigSt4Filter = ""
                .DestSt1Filter = ""
                .DestSt2Filter = ""
                .DestSt3Filter = ""
                .DestSt4Filter = ""
                .UseLoadDateFilter = UseLoadDateFilter
                .BookTransTypeFilter = ""
                .BookConsPrefixFilter = ""
                .LaneNumberFilter = ""
                .BookTranCodeFilter = ""
                .Page = 1
                .PageSize = 100
            End With
            Dim results As DTO.tblSolutionDetail() = target.GetNewPOsFiltered(TruckFilter)

            If results Is Nothing OrElse results.Count() < 1 Then
                Assert.Fail(strTestName & " Get New POs Filtered Failed: Not Records for Truck Filter")
                Return False
            End If
            For Each r As DTO.tblSolutionDetail In results
                System.Diagnostics.Debug.WriteLine("Comp {0} Cases: {1} Wgt: {2} PL: {3} Cube: {4} PX: {5} BFC: {6} Key: {7} Lanes: {8} Lane Numbers: {9} Notes: {10}", r.SolutionDetailCompNumber, r.SolutionDetailTotalCases, r.SolutionDetailTotalWgt, r.SolutionDetailTotalPL, r.SolutionDetailTotalCube, r.SolutionDetailTotalPX, r.SolutionDetailTotalBFC, r.SolutionDetailStopNo, r.SolutionDetailLaneNumber, r.SolutionDetailLaneNumber, r.SolutionDetailBookNotes)
            Next

            System.Diagnostics.Debug.WriteLine("Success!")

        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & strTestName & ": {0} ", ex.Message)
        Finally
            'place clean up code here
        End Try
        Return blnRet
    End Function



    '''<summary>
    '''
    '''</summary>
    <TestMethod()> _
    Public Sub SolutionTruckDragDropDevTest()
        'connect to  database before getting book records overwrite the defaul
        testParameters.DBServer = "NGLRDP06D"
        testParameters.Database = "NGLMAS2013DEV"

        Dim strTestName As String = "SolutionTruckDragDropDevTest"
        Dim sTestPros As New List(Of String)
        Try

            Dim intCompControl As Integer = 9623
            Dim StartDateFilter As Date = Date.Now.AddYears(2).ToShortDateString() ' "4/1/2017"
            Dim SampleOrderDate As Date = Date.Now.AddYears(2).AddDays(15).ToShortDateString() '"4/15/2017"
            Dim StopDateFilter As Date = Date.Now.AddYears(2).AddDays(30).ToShortDateString() '"4/30/2017"
            Dim UseLoadDateFilter As Boolean = True
            Dim intSeed = CInt(Date.Now.ToString("yymmdd"))
            'Create some sample orders
            Dim sCNSKey As String = "TSTRT"
            Dim sOrderKey As String = "SO-TstLoad"
            For i As Integer = 1111 To 1120
                Dim sCNS = sCNSKey & intSeed.ToString()
                Dim sOrder = sOrderKey & intSeed.ToString()
                intSeed = intSeed + 1
                CreateTestOrder("VJI", i, sCNS, sOrder, SampleOrderDate, "P", 80)
                sTestPros.Add("VJI" & i)
            Next
            'get the trucks 
            Dim TruckFilter As New DTO.LoadPlanningTruckDataFilter()
            With TruckFilter
                .CompControlFilter = intCompControl
                .CarrierControlFilter = 0
                .StartDateFilter = StartDateFilter
                .StopDateFilter = StopDateFilter
                .OrigStartZipFilter = "0000000000"
                .OrigStopZipFilter = "9999999999"
                .DestStartZipFilter = "0000000000"
                .DestStopZipFilter = "9999999999"
                .OrigCityFilter = ""
                .DestCityFilter = ""
                .OrigSt1Filter = ""
                .OrigSt2Filter = ""
                .OrigSt3Filter = ""
                .OrigSt4Filter = ""
                .DestSt1Filter = ""
                .DestSt2Filter = ""
                .DestSt3Filter = ""
                .DestSt4Filter = ""
                .UseLoadDateFilter = UseLoadDateFilter
                .BookTransTypeFilter = ""
                .BookConsPrefixFilter = ""
                .LaneNumberFilter = ""
                .BookTranCodeFilter = ""
                .Page = 1
                .PageSize = 100
            End With
            Dim oDALTruck As New DAL.NGLLoadPlanningTruckData(testParameters)
            Dim oTrucks As DTO.tblSolutionTruck() = oDALTruck.GetLoadPlanningTrucksFiltered(TruckFilter)

            If oTrucks Is Nothing OrElse oTrucks.Count() < 10 Then
                Assert.Fail(strTestName & " Failed: Not enough Trucks")
                Return
            End If
            Dim sTruckKeys() As String = oTrucks.Select(Function(d) d.SolutionTruckKey).ToArray()
            If sTruckKeys Is Nothing OrElse sTruckKeys.Count() < 10 Then
                Assert.Fail(strTestName & " Failed: Not enough Truck Keys")
                Return
            End If
            'move all the orders from the even trucks onto the next odd trucks

            Dim oBLLTruck As BLL.NGLSolutionDetailBLL = New BLL.NGLSolutionDetailBLL(testParameters)
            Dim intNewTrucks As Integer = 0
            For i As Integer = 0 To (sTruckKeys.Count() - 2)
                Dim sTruckKey1 As String = sTruckKeys(i)
                i += 1
                Dim sTruckKey2 As String = sTruckKeys(i)
                'get the trucks
                Dim lpTruck1 = oTrucks.Where(Function(d) d.SolutionTruckKey = sTruckKey1).FirstOrDefault()
                If lpTruck1 Is Nothing Then
                    Assert.Fail(strTestName & " cannot read truck key {0}", sTruckKey1)
                    Return
                End If
                If lpTruck1.SolutionDetails Is Nothing OrElse lpTruck1.SolutionDetails.Count() < 1 Then
                    Assert.Fail(strTestName & " cannot read details for truck key {0}", sTruckKey1)
                    Return
                End If
                Dim lpTruck2 = oTrucks.Where(Function(d) d.SolutionTruckKey = sTruckKey2).FirstOrDefault()
                If lpTruck2 Is Nothing Then
                    Assert.Fail(strTestName & " cannot read truck key {0}", sTruckKey2)
                End If
                If lpTruck2.SolutionDetails Is Nothing OrElse lpTruck2.SolutionDetails.Count() < 1 Then
                    Assert.Fail(strTestName & " cannot read details for truck key {0}", sTruckKey2)
                    Return
                End If
                Dim oDetail1 = lpTruck1.SolutionDetails(0)
                'drop detail1 onto truck 2 
                oBLLTruck.LoadPlanningItemDropped(lpTruck2, oDetail1, 2)
                'call the drop update method
                Dim oUpdateResults = oBLLTruck.LoadPlanningRecalculateTruck(lpTruck2, True, True, True)
                If oUpdateResults Is Nothing OrElse oUpdateResults.Success = False OrElse oUpdateResults.UpdatedLPTruck Is Nothing Then
                    If Not oUpdateResults.Messages Is Nothing AndAlso oUpdateResults.Messages.Count() > 0 Then
                        For Each m In oUpdateResults.Messages
                            System.Diagnostics.Debug.WriteLine("Resut Message Not Localized {0}:", m.Key, "")
                            If Not m.Value Is Nothing AndAlso m.Value.Count() > 0 Then
                                For Each n In m.Value
                                    System.Diagnostics.Debug.WriteLine("------------ Detail {0}", n.Message, "")
                                Next
                            End If
                        Next
                    End If
                    Assert.Fail(strTestName & " cannot Recalculate Truck with truck key {0}", sTruckKey2)
                    Return
                End If
                With oUpdateResults.UpdatedLPTruck
                    System.Diagnostics.Debug.WriteLine("******************** Returned to Client ***********************")
                    System.Diagnostics.Debug.WriteLine("CNS {6}: | Cases {0} | Wgt {1} | Plt {2} | Cube {3} | Cost {4} | Miles {5} | NbrOfOrder {7}", .SolutionTruckTotalCases, .SolutionTruckTotalWgt, .SolutionTruckTotalPL, .SolutionTruckTotalCube, .SolutionTruckTotalCost, .SolutionTruckTotalMiles, .SolutionTruckConsPrefix, .SolutionDetails.Count())
                    If Not .Messages Is Nothing AndAlso .Messages.Count() > 0 Then
                        For Each m In .Messages
                            System.Diagnostics.Debug.WriteLine("Message Not Localized {0}:", m.Key, "")
                            If Not m.Value Is Nothing AndAlso m.Value.Count() > 0 Then
                                For Each n In m.Value
                                    System.Diagnostics.Debug.WriteLine("------------ Detail {0}", n.Message, "")
                                Next
                            End If
                        Next
                    End If
                    System.Diagnostics.Debug.WriteLine("***************************************************************")
                End With
                intNewTrucks += 1
            Next
            'get the trucks back and count the number or records
            oTrucks = oDALTruck.GetLoadPlanningTrucksFiltered(TruckFilter)
            If oTrucks.Count() <> intNewTrucks Then
                Assert.Fail(strTestName & " Failed: Not enough new Trucks return")
                Return
            End If

            
            System.Diagnostics.Debug.WriteLine("Found {0} new trucks.", oTrucks.Count(), "")

            Try
                'delete the test order and ignore any errors
                DeleteTestOrders(sTestPros)
            Catch ex As Exception

            End Try
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & strTestName & ": {0} ", ex.Message)
        Finally
            'place clean up code here
        End Try

    End Sub

    <TestMethod()> _
    Public Sub SolutionTruckCalculatePrepareSpeedTest()
        'connect to  database before getting book records overwrite the defaul
        testParameters.DBServer = "NGLRDP06D"
        testParameters.Database = "NGLMAS2013DEV"

        Dim strTestName As String = "SolutionTruckDragDropDevTest"
        Dim sTestPros As New List(Of String)
        Try

            Dim intCompControl As Integer = 9623
            Dim StartDateFilter As Date = Date.Now.AddYears(2).ToShortDateString() ' "4/1/2017"
            Dim SampleOrderDate As Date = Date.Now.AddYears(2).AddDays(15).ToShortDateString() '"4/15/2017"
            Dim StopDateFilter As Date = Date.Now.AddYears(2).AddDays(30).ToShortDateString() '"4/30/2017"
            Dim UseLoadDateFilter As Boolean = True
            Dim intSeed = CInt(Date.Now.ToString("yymmdd"))
            'Create some sample orders
            Dim sCNSKey As String = "TSTRT"
            Dim sOrderKey As String = "SO-TstLoad"
            For i As Integer = 1111 To 1120
                Dim sCNS = sCNSKey & intSeed.ToString()
                Dim sOrder = sOrderKey & intSeed.ToString()
                intSeed = intSeed + 1
                CreateTestOrder("VJI", i, sCNS, sOrder, SampleOrderDate, "P", 80)
                sTestPros.Add("VJI" & i)
            Next
            'get the trucks 
            Dim TruckFilter As New DTO.LoadPlanningTruckDataFilter()
            With TruckFilter
                .CompControlFilter = intCompControl
                .CarrierControlFilter = 0
                .StartDateFilter = StartDateFilter
                .StopDateFilter = StopDateFilter
                .OrigStartZipFilter = "0000000000"
                .OrigStopZipFilter = "9999999999"
                .DestStartZipFilter = "0000000000"
                .DestStopZipFilter = "9999999999"
                .OrigCityFilter = ""
                .DestCityFilter = ""
                .OrigSt1Filter = ""
                .OrigSt2Filter = ""
                .OrigSt3Filter = ""
                .OrigSt4Filter = ""
                .DestSt1Filter = ""
                .DestSt2Filter = ""
                .DestSt3Filter = ""
                .DestSt4Filter = ""
                .UseLoadDateFilter = UseLoadDateFilter
                .BookTransTypeFilter = ""
                .BookConsPrefixFilter = ""
                .LaneNumberFilter = ""
                .BookTranCodeFilter = ""
                .Page = 1
                .PageSize = 100
            End With
            Dim oDALTruck As New DAL.NGLLoadPlanningTruckData(testParameters)
            Dim oTrucks As DTO.tblSolutionTruck() = oDALTruck.GetLoadPlanningTrucksFiltered(TruckFilter)

            If oTrucks Is Nothing OrElse oTrucks.Count() < 10 Then
                Assert.Fail(strTestName & " Failed: Not enough Trucks")
                Return
            End If
            Dim sTruckKeys() As String = oTrucks.Select(Function(d) d.SolutionTruckKey).ToArray()
            If sTruckKeys Is Nothing OrElse sTruckKeys.Count() < 10 Then
                Assert.Fail(strTestName & " Failed: Not enough Truck Keys")
                Return
            End If
            'move all the orders from the even trucks onto the next odd trucks

            Dim oBLLTruck As BLL.NGLSolutionDetailBLL = New BLL.NGLSolutionDetailBLL(testParameters)
            Dim intNewTrucks As Integer = 0
            For i As Integer = 0 To (sTruckKeys.Count() - 2)
                Dim sTruckKey1 As String = sTruckKeys(i)
                i += 1
                Dim sTruckKey2 As String = sTruckKeys(i)
                'get the trucks
                Dim lpTruck1 = oTrucks.Where(Function(d) d.SolutionTruckKey = sTruckKey1).FirstOrDefault()
                If lpTruck1 Is Nothing Then
                    Assert.Fail(strTestName & " cannot read truck key {0}", sTruckKey1)
                    Return
                End If
                If lpTruck1.SolutionDetails Is Nothing OrElse lpTruck1.SolutionDetails.Count() < 1 Then
                    Assert.Fail(strTestName & " cannot read details for truck key {0}", sTruckKey1)
                    Return
                End If
                Dim lpTruck2 = oTrucks.Where(Function(d) d.SolutionTruckKey = sTruckKey2).FirstOrDefault()
                If lpTruck2 Is Nothing Then
                    Assert.Fail(strTestName & " cannot read truck key {0}", sTruckKey2)
                End If
                If lpTruck2.SolutionDetails Is Nothing OrElse lpTruck2.SolutionDetails.Count() < 1 Then
                    Assert.Fail(strTestName & " cannot read details for truck key {0}", sTruckKey2)
                    Return
                End If
                Dim oDetail1 = lpTruck1.SolutionDetails(0)
                'drop detail1 onto truck 2 
                oBLLTruck.LoadPlanningItemDropped(lpTruck2, oDetail1, 2)
                'call the drop update method
                Dim oUpdateResults = oBLLTruck.LoadPlanningRecalculateTruck(lpTruck2, True, True, True)
                If oUpdateResults Is Nothing OrElse oUpdateResults.Success = False OrElse oUpdateResults.UpdatedLPTruck Is Nothing Then
                    If Not oUpdateResults.Messages Is Nothing AndAlso oUpdateResults.Messages.Count() > 0 Then
                        For Each m In oUpdateResults.Messages
                            System.Diagnostics.Debug.WriteLine("Resut Message Not Localized {0}:", m.Key, "")
                            If Not m.Value Is Nothing AndAlso m.Value.Count() > 0 Then
                                For Each n In m.Value
                                    System.Diagnostics.Debug.WriteLine("------------ Detail {0}", n.Message, "")
                                Next
                            End If
                        Next
                    End If
                    Assert.Fail(strTestName & " cannot Recalculate Truck with truck key {0}", sTruckKey2)
                    Return
                End If
                With oUpdateResults.UpdatedLPTruck
                    System.Diagnostics.Debug.WriteLine("******************** Returned to Client ***********************")
                    System.Diagnostics.Debug.WriteLine("CNS {6}: | Cases {0} | Wgt {1} | Plt {2} | Cube {3} | Cost {4} | Miles {5} | NbrOfOrder {7}", .SolutionTruckTotalCases, .SolutionTruckTotalWgt, .SolutionTruckTotalPL, .SolutionTruckTotalCube, .SolutionTruckTotalCost, .SolutionTruckTotalMiles, .SolutionTruckConsPrefix, .SolutionDetails.Count())
                    If Not .Messages Is Nothing AndAlso .Messages.Count() > 0 Then
                        For Each m In .Messages
                            System.Diagnostics.Debug.WriteLine("Message Not Localized {0}:", m.Key, "")
                            If Not m.Value Is Nothing AndAlso m.Value.Count() > 0 Then
                                For Each n In m.Value
                                    System.Diagnostics.Debug.WriteLine("------------ Detail {0}", n.Message, "")
                                Next
                            End If
                        Next
                    End If
                    System.Diagnostics.Debug.WriteLine("***************************************************************")
                End With
                intNewTrucks += 1
            Next
            'get the trucks back and count the number or records
            oTrucks = oDALTruck.GetLoadPlanningTrucksFiltered(TruckFilter)
            If oTrucks.Count() <> intNewTrucks Then
                Assert.Fail(strTestName & " Failed: Not enough new Trucks return")
                Return
            End If


            System.Diagnostics.Debug.WriteLine("Found {0} new trucks.", oTrucks.Count(), "")

        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & strTestName & ": {0} ", ex.Message)
        Finally
            'place clean up code here
        End Try

    End Sub

    '''<summary>
    '''
    '''</summary>
    <TestMethod()> _
    Public Sub SolutionTruckCalculateSpeedTest()
        'connect to  database before getting book records overwrite the defaul
        testParameters.DBServer = "NGLRDP06D"
        testParameters.Database = "NGLMAS2013DEV"

        Dim strTestName As String = "SolutionTruckCalculateSpeedTest"
        Dim sTestControls As New List(Of Integer)
        Try

            Dim intCompControl As Integer = 9623
            Dim StartDateFilter As Date = Date.Now.AddYears(2).ToShortDateString() ' "4/1/2017"
            Dim SampleOrderDate As Date = Date.Now.AddYears(2).AddDays(15).ToShortDateString() '"4/15/2017"
            Dim StopDateFilter As Date = Date.Now.AddYears(2).AddDays(30).ToShortDateString() '"4/30/2017"
            Dim UseLoadDateFilter As Boolean = True
            
            'get the trucks 
            Dim TruckFilter As New DTO.LoadPlanningTruckDataFilter()
            With TruckFilter
                .CompControlFilter = intCompControl
                .CarrierControlFilter = 0
                .StartDateFilter = StartDateFilter
                .StopDateFilter = StopDateFilter
                .OrigStartZipFilter = "0000000000"
                .OrigStopZipFilter = "9999999999"
                .DestStartZipFilter = "0000000000"
                .DestStopZipFilter = "9999999999"
                .OrigCityFilter = ""
                .DestCityFilter = ""
                .OrigSt1Filter = ""
                .OrigSt2Filter = ""
                .OrigSt3Filter = ""
                .OrigSt4Filter = ""
                .DestSt1Filter = ""
                .DestSt2Filter = ""
                .DestSt3Filter = ""
                .DestSt4Filter = ""
                .UseLoadDateFilter = UseLoadDateFilter
                .BookTransTypeFilter = ""
                .BookConsPrefixFilter = ""
                .LaneNumberFilter = ""
                .BookTranCodeFilter = ""
                .Page = 1
                .PageSize = 100
            End With
            Dim oDALTruck As New DAL.NGLLoadPlanningTruckData(testParameters)
            Dim oTrucks As DTO.tblSolutionTruck() = oDALTruck.GetLoadPlanningTrucksFiltered(TruckFilter)
            If oTrucks Is Nothing Then
                Assert.Fail(strTestName & " Failed: No Trucks")
                Return
            End If
            'read the bookrevs back in 
            Dim oBookRevDAL As New DAL.NGLBookRevenueData(testParameters)
            For Each t In oTrucks
                If Not t.SolutionDetails Is Nothing AndAlso t.SolutionDetails.Count() > 0 Then
                    'Dim oBookRevs = oBookRevDAL.GetBookRevenuesWDetailsFiltered(t.SolutionDetails(0).SolutionDetailBookControl)
                    'Dim oBookRevs = oBookRevDAL.GetBookRevenuesWDetailsFilteredFromView(t.SolutionDetails(0).SolutionDetailBookControl)
                    Dim oLTSRevs = oBookRevDAL.GetBookRevenuesLTSWDetailsFilteredFromView(t.SolutionDetails(0).SolutionDetailBookControl)
                End If
            Next
           
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & strTestName & ": {0} ", ex.Message)
        Finally
            'place clean up code here
        End Try

    End Sub


End Class
