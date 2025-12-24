Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports System
Imports Ngl.FM.CarTar
Imports DAL = Ngl.FreightMaster.Data
Imports DTO = NGL.FreightMaster.Data.DataTransferObjects
Imports BLL = NGL.FM.BLL

<TestClass()> Public Class BookRevenueBLLTest
    Inherits TestBase


    '''<summary>
    '''A test for DoRateShop
    '''</summary>
    <TestMethod()> _
    Public Sub SheildsDoRateShopTest()
        'connect to  database before getting book records overwrite the defaul
        testParameters.DBServer = "NGLRDP06D"
        testParameters.Database = "SHIELDSMASPROD"


        Dim target As BLL.NGLBookRevenueBLL = New BLL.NGLBookRevenueBLL(testParameters)
        Dim oBookRevs As New List(Of DTO.BookRevenue)
        'Test Route
        '98903 Yakima, WA, Yakima to 28117 Mooresville, NC, Iredell  miles = 2685.4
        Dim oBookrev As New DTO.BookRevenue With {.BookDateLoad = Date.Now(), _
                                              .BookDateRequired = Date.Now(), _
                                              .BookDestCompControl = 0,
                                              .BookDestCity = "Kansas City", _
                                              .BookDestCountry = "US", _
                                              .BookDestState = "KS", _
                                              .BookDestZip = "66101", _
                                              .BookLockBFCCost = False, _
                                              .BookMilesFrom = 1000, _
                                              .BookModeTypeControl = 0, _
                                              .BookOrigCompControl = 1,
                                              .BookOrigCity = "Yakima", _
                                              .BookOrigCountry = "US", _
                                              .BookOrigState = "WA", _
                                              .BookOrigZip = "98902", _
                                              .BookTotalBFC = 0, _
                                              .BookTotalCases = 0, _
                                              .BookTotalCube = 0, _
                                              .BookTotalPL = 1, _
                                              .BookTotalPX = 0, _
                                              .BookTotalWgt = 35000, _
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


        Try
            Dim Results As DTO.CarrierCostResults = target.DoRateShop(rateShop)

            If Results Is Nothing OrElse Results.CarriersByCost Is Nothing Then
                Assert.Fail("Call to DoRateShop Failed no results")
                Return
            End If

            For Each l As DTO.NGLMessage In Results.Log
                System.Diagnostics.Debug.WriteLine(l.Message)
                If l.Control <> 0 Then
                    System.Diagnostics.Debug.WriteLine("Link to " + l.ControlReferenceName + " Control # " + l.Control.ToString())
                End If
            Next
            Dim ct As Integer = Results.CarriersByCost.Count



        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("DoRateShopTest Unexpected Error: " & ex.Message)
        Finally

        End Try
    End Sub


    '''<summary>
    '''A test for DoRateShop
    '''</summary>
    <TestMethod()> _
    Public Sub BonDoRateShopTest()
        'connect to  database before getting book records overwrite the defaul
        testParameters.DBServer = "NGLRDP07D"
        testParameters.Database = "NGLMASPRODBON"


        Dim target As BLL.NGLBookRevenueBLL = New BLL.NGLBookRevenueBLL(testParameters)
        Dim oBookRevs As New List(Of DTO.BookRevenue)
        'Test Route
        '98903 Yakima, WA, Yakima to 28117 Mooresville, NC, Iredell  miles = 2685.4
        Dim oBookrev As New DTO.BookRevenue With {.BookDateLoad = Date.Now(), _
                                              .BookDateRequired = Date.Now(), _
                                              .BookDestCompControl = 0,
                                              .BookDestCity = "CHESTER DEPOT", _
                                              .BookDestCountry = "US", _
                                              .BookDestState = "VT", _
                                              .BookDestZip = "05144", _
                                              .BookLockBFCCost = False, _
                                              .BookMilesFrom = Nothing, _
                                              .BookModeTypeControl = 3, _
                                              .BookOrigCompControl = 1002,
                                              .BookOrigCity = "PERHAM", _
                                              .BookOrigCountry = "US", _
                                              .BookOrigState = "MN", _
                                              .BookOrigZip = "56573", _
                                              .BookTotalBFC = 0, _
                                              .BookTotalCases = 0, _
                                              .BookTotalCube = 0, _
                                              .BookTotalPL = 1, _
                                              .BookTotalPX = 0, _
                                              .BookTotalWgt = 1000, _
                                              .BookTranCode = Nothing, _
                                              .BookTypeCode = Nothing, _
                                              .BookTransType = Nothing}


        oBookRevs.Add(oBookrev)
        Dim rateShop As New DTO.RateShop With {.Page = 1,
                                            .PageSize = 100,
                                            .AgentControl = 0,
                                            .BookRevs = oBookRevs,
                                            .CarrierControl = 0,
                                            .CarrTarEquipMatClass = "60",
                                            .CarrTarEquipMatClassTypeControl = 6,
                                            .CarrTarEquipMatTarRateTypeControl = 0,
                                            .ModeTypeControl = 3,
                                            .NoLateDelivery = False,
                                            .OptimizeByCapacity = False,
                                            .Outbound = True,
                                            .Prefered = False,
                                            .TariffTypeControl = 0,
                                            .TempType = 1,
                                            .UsePCM = True,
                                            .Validated = False}


        Try
            Dim Results As DTO.CarrierCostResults = target.DoRateShop(rateShop)

            If Results Is Nothing OrElse Results.CarriersByCost Is Nothing Then
                Assert.Fail("Call to DoRateShop Failed no results")
                Return
            End If

            For Each l As DTO.NGLMessage In Results.Log
                System.Diagnostics.Debug.WriteLine(l.Message)
                If l.Control <> 0 Then
                    System.Diagnostics.Debug.WriteLine("Link to " + l.ControlReferenceName + " Control # " + l.Control.ToString())
                End If
            Next
            Dim ct As Integer = Results.CarriersByCost.Count



        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As System.ServiceModel.FaultException(Of DAL.SqlFaultInfo)
            Dim strMsg = ex.Detail.getMsgForLogs()
            'Dim strMsg = ex.Detail.getMsgForLogs(ex.Reason.ToString())
            Assert.Fail("DoRateShopTest SQL Fault Error: " & strMsg)
        Catch ex As Exception
            Assert.Fail("DoRateShopTest Unexpected Error: " & ex.Message)
        Finally

        End Try
    End Sub


    '''<summary>
    '''
    '''</summary>
    <TestMethod()> _
    Public Sub OverCapacityLoadCalculateCostDevTest()
        'connect to  database before getting book records overwrite the defaul
        testParameters.DBServer = "NGLRDP06D"
        testParameters.Database = "NGLMAS2013DEV"
        'Note:  this book control number will need to be checked before running this test
        'in the automated version of the test we will build loads automatically
        Dim BookControl As Integer = 900611
        Dim strTestName As String = "OverCapacityLoadCalculateCostDevTest"
        Try
            Dim target As BLL.NGLBookRevenueBLL = New BLL.NGLBookRevenueBLL(testParameters)
            Dim Messages As New Dictionary(Of String, List(Of DTO.NGLMessage))
            Dim oCCResults = target.AssignOrUpdateCarrier(BookControl)
            If Not oCCResults.Success Then
                If Not oCCResults.Messages Is Nothing AndAlso oCCResults.Messages.Count() > 0 Then
                    For Each m In oCCResults.Messages
                        If Not Messages.ContainsKey(m.Key) Then
                            Messages(m.Key) = m.Value
                        Else
                            Messages.Add(m.Key, m.Value)
                        End If
                    Next
                Else
                    If Not Not Messages.ContainsKey("MSGRecalcCostFailedWarning") Then
                        Messages.Add("MSGRecalcCostFailedWarning", Nothing)
                    End If
                End If
                If Not oCCResults.Log Is Nothing AndAlso oCCResults.Log.Count() > 0 Then
                    For Each l As DTO.NGLMessage In oCCResults.Log
                        System.Diagnostics.Debug.WriteLine(l.Message)
                        If l.Control <> 0 Then
                            System.Diagnostics.Debug.WriteLine("Link to " + l.ControlReferenceName + " Control # " + l.Control.ToString())
                        End If
                    Next
                End If
            End If
            'print out the messages

            'get the record back
            Dim oRevs = getBookRevsByBookControl(BookControl)
            If oRevs Is Nothing OrElse oRevs.Count() < 1 Then
                Assert.Fail(strTestName & " no book revenue records found.")
            End If

            System.Diagnostics.Debug.WriteLine("Book Revenue Records Found: " & oRevs.Count())

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
    <TestMethod()>
    Public Sub RemoveCurrentContactBugTest()
        'connect to  database before getting book records overwrite the defaul
        testParameters.DBServer = "NGLRDP07D"
        testParameters.Database = "NGLMASPROD"
        'Note:  this book control number will need to be checked before running this test
        'in the automated version of the test we will build loads automatically
        Dim BookControl As Integer = 5117
        Dim strTestName As String = "RemoveCurrentContactBugTest"
        Try
            Dim target As BLL.NGLBookRevenueBLL = New BLL.NGLBookRevenueBLL(testParameters)
            Dim Messages As New Dictionary(Of String, List(Of DTO.NGLMessage))
            Dim oCCResults = target.AssignOrUpdateCarrier(BookControl)
            If Not oCCResults.Success Then
                If Not oCCResults.Messages Is Nothing AndAlso oCCResults.Messages.Count() > 0 Then
                    For Each m In oCCResults.Messages
                        If Not Messages.ContainsKey(m.Key) Then
                            Messages(m.Key) = m.Value
                        Else
                            Messages.Add(m.Key, m.Value)
                        End If
                    Next
                Else
                    If Not Not Messages.ContainsKey("MSGRecalcCostFailedWarning") Then
                        Messages.Add("MSGRecalcCostFailedWarning", Nothing)
                    End If
                End If
                If Not oCCResults.Log Is Nothing AndAlso oCCResults.Log.Count() > 0 Then
                    For Each l As DTO.NGLMessage In oCCResults.Log
                        System.Diagnostics.Debug.WriteLine(l.Message)
                        If l.Control <> 0 Then
                            System.Diagnostics.Debug.WriteLine("Link to " + l.ControlReferenceName + " Control # " + l.Control.ToString())
                        End If
                    Next
                End If
            End If
            'print out the messages

            'get the record back
            Dim oRevs = getBookRevsByBookControl(BookControl)
            If oRevs Is Nothing OrElse oRevs.Count() < 1 Then
                Assert.Fail(strTestName & " no book revenue records found.")
            End If

            System.Diagnostics.Debug.WriteLine("Book Revenue Records Found: " & oRevs.Count())

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
    Public Sub SolutionTruckDragDropDevTest()
        'connect to  database before getting book records overwrite the defaul
        testParameters.DBServer = "NGLRDP06D"
        testParameters.Database = "NGLMAS2013DEV"

        Dim strTestName As String = "SolutionTruckDragDropDevTest"
        Try
            Dim intCompControl As Integer = 9658
            Dim sTruckKey1 As String = "9658-CNS108381-1-6-0"
            Dim sTruckKey2 As String = "9658-CNS108503-1-6-0"
            Dim sOrderNumber As String = "SO-OPTTEST-1136-0".ToUpper()
            Dim target As BLL.NGLSolutionDetailBLL = New BLL.NGLSolutionDetailBLL(testParameters)
            Dim oLoadPlanningTruckData As New DAL.NGLLoadPlanningTruckData(testParameters)
            Dim lpTruck1 As DTO.tblSolutionTruck = oLoadPlanningTruckData.GetLoadPlanningTruckFiltered(intCompControl, sTruckKey1, False)
            If lpTruck1 Is Nothing Then
                Assert.Fail(strTestName & " cannot read truck key {0}", sTruckKey1)
            End If
            Dim lpTruck2 As DTO.tblSolutionTruck = oLoadPlanningTruckData.GetLoadPlanningTruckFiltered(intCompControl, sTruckKey2, False)
            If lpTruck2 Is Nothing Then
                Assert.Fail(strTestName & " cannot read truck key {0}", sTruckKey2)
            End If
            If lpTruck2.SolutionDetails Is Nothing OrElse lpTruck2.SolutionDetails.Count() < 1 Then
                Assert.Fail(strTestName & " cannot read truck key {0} details", sTruckKey2)
            End If
            Dim detail As DTO.tblSolutionDetail
            For Each d In lpTruck2.SolutionDetails
                If d.OrderNumber.ToUpper() = sOrderNumber Then
                    detail = d
                    Exit For
                End If
            Next
            If detail Is Nothing Then
                Assert.Fail(strTestName & " no details for order number {0}", sOrderNumber)
            End If
            'drop detail onto truck 1
            target.LoadPlanningItemDropped(lpTruck1, detail, 2)
            'call the drop update method
            Dim result = target.LoadPlanningRecalculateTruck(lpTruck1, True, True, True)
            With result.UpdatedLPTruck
                System.Diagnostics.Debug.WriteLine("******************** Returned to Client ***********************")
                System.Diagnostics.Debug.WriteLine("CNS {6}: | Cases {0} | Wgt {1} | Plt {2} | Cube {3} | Cost {4} | Miles {5} |", .SolutionTruckTotalCases, .SolutionTruckTotalWgt, .SolutionTruckTotalPL, .SolutionTruckTotalCube, .SolutionTruckTotalCost, .SolutionTruckTotalMiles, .SolutionTruckConsPrefix)
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

            result = target.LoadPlanningRecalculateTruck(lpTruck2, True, True, True)
            With result.UpdatedLPTruck
                System.Diagnostics.Debug.WriteLine("******************** Returned to Client ***********************")
                System.Diagnostics.Debug.WriteLine("CNS {6}: | Cases {0} | Wgt {1} | Plt {2} | Cube {3} | Cost {4} | Miles {5} |", .SolutionTruckTotalCases, .SolutionTruckTotalWgt, .SolutionTruckTotalPL, .SolutionTruckTotalCube, .SolutionTruckTotalCost, .SolutionTruckTotalMiles, .SolutionTruckConsPrefix)
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
            'print out the messages

            'get the record back
            Dim oRevs = Nothing
            '  oRevs = getBookRevsByBookControl(BookControl)
            If oRevs Is Nothing OrElse oRevs.Count() < 1 Then
                Assert.Fail(strTestName & " no book revenue records found.")
            End If

            System.Diagnostics.Debug.WriteLine("Book Revenue Records Found: " & oRevs.Count())

        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & strTestName & ": {0} ", ex.Message)
        Finally
            'place clean up code here
        End Try

    End Sub

    'VJI754576
    '''<summary>
    '''A test for Carrier Accept Or Reject from Web Test
    '''</summary>
    <TestMethod()> _
    Public Sub CarrierAcceptOrRejectLoadWebTest()
        Dim strTestName As String = "CarrierAcceptOrRejectLoadWebTest"
        'connect to  database before getting book records overwrite the defaul
        testParameters.DBServer = "NGLRDP06D"
        testParameters.Database = "NGLMAS2013DEV"


        Dim bookControl As Integer = 898800
        Dim responseCode As Integer = 1
        Dim carrierContControl As Integer = 21984
        Dim carrierControl As Integer = 17253
        Dim sendEmail As Boolean = True
        Dim trucComment As String = "Cannot take Load"
        Dim UserName As String = "18Wheel"
        Dim emailTo As String = ""
        Dim emailCc As String = ""

        Try

            Dim target As BLL.NGLBookBLL = New BLL.NGLBookBLL(testParameters)
            Dim blnRet As Boolean = target.CarrierAcceptOrRejectLoadWeb(bookControl, responseCode, carrierContControl, carrierControl, sendEmail, trucComment, UserName, emailTo, emailCc)

            If Not blnRet Then
                Assert.Fail("Call to " & strTestName & " Failed")
                Return
            End If

        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail(strTestName & " Unexpected Error: " & ex.Message)
        Finally

        End Try
    End Sub

End Class