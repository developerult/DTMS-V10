Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports DAL = NGL.FreightMaster.Data
Imports DTO = NGL.FreightMaster.Data.DataTransferObjects
Imports BLL = NGL.FM.BLL 

<TestClass()> Public Class tblHdmTest
    Inherits TestBase

    <TestMethod()>
    Public Sub TestAddHdmRecord()


        Dim strTestName As String = "TestAddHdmRecord"
        Dim target As New DAL.NGLHDMData(testParameters)
        Dim actual As DTO.tblHdm = Nothing
        Try
            Dim expected As New DTO.tblHdm()
            expected.HDMCarrierControl = 2
            expected.HDMDesc = "test Desc"
            expected.HDMName = "test Name"
            actual = target.CreateRecord(expected)
            Assert.IsTrue(actual.HDMControl > 0)
            Assert.AreEqual(expected.HDMName, actual.HDMName)
        Catch ex As Exception

            Assert.Fail("TestAddHdmRecord: add failed")
        Finally
            Try
                If actual IsNot Nothing AndAlso actual.HDMControl > 0 Then
                    target.DeleteRecord(actual)
                End If
            Catch ex As Exception

            End Try
        End Try

    End Sub

    ''' <summary>
    ''' Test rate shopping
    ''' </summary>
    ''' <remarks></remarks> 
    <TestMethod()>
    Public Sub TesRateShop()
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
                                                .CarrTarEquipMatClass = "125",
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
                Return
            End If

            For Each l As DTO.NGLMessage In Results.Log
                System.Diagnostics.Debug.WriteLine(l.Message)
                If l.Control <> 0 Then
                    System.Diagnostics.Debug.WriteLine("Link to " + l.ControlReferenceName + " Control # " + l.Control.ToString())
                End If
            Next

            If Results.CarriersByCost.Count() < 1 Then
                Assert.Fail(strTestName & " Failed: No Carriers Returned")
                Return
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
        Return
    End Sub

    <TestMethod()> _
    Public Sub TestUpdateHdmRecord()
       

        Dim strTestName As String = "TestAddHdmRecord"
        Dim target As New DAL.NGLHDMData(testParameters)
        Dim actual As DTO.tblHdm = Nothing
        Dim actualUpdated As DTO.tblHdm = Nothing
        Try
            Dim expected As New DTO.tblHdm()
            expected.HDMCarrierControl = 2
            expected.HDMDesc = "test Desc"
            expected.HDMName = "test Name"
            actual = target.CreateRecord(expected)
            Assert.IsTrue(actual.HDMControl > 0)
            Assert.AreEqual(expected.HDMName, actual.HDMName)
            actual.HDMName = "test name test 2"
            actualUpdated = target.UpdateRecord(actual)
            Assert.IsTrue(actual.HDMControl > 0)
            Assert.AreEqual(actual.HDMName, actualUpdated.HDMName)
        Catch ex As Exception

            Assert.Fail("TestAddHdmRecord: add failed")
        Finally
            Try
                If actualUpdated IsNot Nothing AndAlso actualUpdated.HDMControl > 0 Then
                    target.DeleteRecord(actualUpdated)
                End If
            Catch ex As Exception

            End Try
        End Try

    End Sub

End Class