Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports System
Imports Ngl.FM.CarTar
Imports DAL = Ngl.FreightMaster.Data
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports BLL = Ngl.FM.BLL

<TestClass()> Public Class NGLCarrierDataProvierTests
    Inherits TestBase

    <TestMethod()> _
    Public Sub UpdateCarrierFuelFeesTest()
        'connect to  database before getting book records overwrite the defaul
        testParameters.DBServer = "NGLRDP07D"
        testParameters.Database = "NGLMASPROD"
        testParameters.UserName = "NGL\rramsey"
        Dim strTestName As String = "UpdateCarrierFuelFeesTest"

        Try

            Dim CarrierControl As Integer = 2
            Dim target As New Ngl.FM.BLL.NGLCarrierBLL(testParameters)
            Dim results = target.UpdateCarrierFuelFeesSync(CarrierControl)
            If results Is Nothing Then
                Assert.Fail(strTestName & " failed,  no records.")
            End If
            'System.Diagnostics.Debug.WriteLine(strTestName & " found " & results.Count() & " records.")
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & strTestName & ": {0} ", ex.Message)
        Finally
            'place clean up code here
        End Try

    End Sub

    <TestMethod()> _
    Public Sub GetCarrTarContractsFilteredSecurityTest()
        'connect to  database before getting book records overwrite the defaul
        testParameters.DBServer = "NGLRDP06D"
        testParameters.Database = "NGLMAS2013DEV"
        testParameters.UserName = "NGL\rramsey"

        Dim strTestName As String = "GetCarrTarContractsFilteredSecurityTest"

        Try

            Dim CarrierControl As Integer = 0
            Dim CompControl As Integer = 0
            Dim TempType As Integer = 0
            Dim EffStartDateFrom As System.Nullable(Of Date) = Nothing
            Dim EffStartDateTo As System.Nullable(Of Date) = Nothing
            Dim EffEndDateFrom As System.Nullable(Of Date) = Nothing
            Dim EffEndDateTo As System.Nullable(Of Date) = Nothing
            Dim TariffType As Integer = 0
            Dim ModeType As Integer = 0
            Dim Revision As Integer = 0
            Dim ShowActiveContracts As Boolean = True
            Dim Approved As Boolean = True
            Dim Rejected As Boolean = False
            Dim Outbound As Boolean = True
            Dim AgentControl As Integer = 0
            Dim page As Integer = 1
            Dim pagesize As Integer = 1000

            Dim target As New DAL.NGLCarrTarContractData(testParameters)
            Dim results As DTO.CarrTarContract() = target.GetCarrTarContractsFiltered(CarrierControl, _
                                                              CompControl, _
                                                              TempType, _
                                                              EffStartDateFrom, _
                                                              EffStartDateTo, _
                                                              EffEndDateFrom, _
                                                              EffEndDateTo, _
                                                              TariffType, _
                                                              ModeType, _
                                                              Revision, _
                                                              ShowActiveContracts, _
                                                              Approved, _
                                                              Rejected, _
                                                              Outbound, _
                                                              AgentControl, _
                                                              page, _
                                                              pagesize)
            If results Is Nothing OrElse results.Count() < 1 Then
                Assert.Fail(strTestName & " failed,  no records.")
            End If
            System.Diagnostics.Debug.WriteLine(strTestName & " found " & results.Count() & " records.")
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & strTestName & ": {0} ", ex.Message)
        Finally
            'place clean up code here
        End Try

    End Sub


    <TestMethod()> _
    Public Sub GetCityStateZipLookupBLLTest()
        'connect to  database before getting book records overwrite the defaul
        testParameters.DBServer = "NGLRDP06D"
        testParameters.Database = "NGLMAS2013DEV"
        testParameters.UserName = "NGL\rramsey"

        Dim strTestName As String = "GetCityStateZipLookupBLLTest"

        Try

            Dim CarrierControl As Integer = 0
            Dim CompControl As Integer = 0
            Dim TempType As Integer = 0
            Dim EffStartDateFrom As System.Nullable(Of Date) = Nothing
            Dim EffStartDateTo As System.Nullable(Of Date) = Nothing
            Dim EffEndDateFrom As System.Nullable(Of Date) = Nothing
            Dim EffEndDateTo As System.Nullable(Of Date) = Nothing
            Dim TariffType As Integer = 0
            Dim ModeType As Integer = 0
            Dim Revision As Integer = 0
            Dim ShowActiveContracts As Boolean = True
            Dim Approved As Boolean = True
            Dim Rejected As Boolean = False
            Dim Outbound As Boolean = True
            Dim AgentControl As Integer = 0
            Dim page As Integer = 1
            Dim pagesize As Integer = 1000
            Dim postalCode As String = "606"


            'NGL.FM.BLL.NGLLookupBLL oBLL = new NGL.FM.BLL.NGLLookupBLL(parameters);
            '       Ngl.FreightMaster.Data.DataTransferObjects.CityStateZipTariffLookup[] oZips = oBLL.GetCityStateZipLookup(sortKey, carrierControl, postalCode, getLists, 10);

            Dim target As New Ngl.FM.BLL.NGLLookupBLL(testParameters)


            Dim results = target.GetCityStateZipLookup(3, 0, postalCode, 1, 20)
            'Dim tariffShippers = (From ts In target.GetTariffShippersByCarrier(2, 0)
            '                                                   Where ts.CompStreetZip.ToUpper.StartsWith(postalCode.ToUpper)
            '                                                  Select ts)
            ' Dim tariffShippers = target.GetTariffShippersByCarrier(2, 0).Where(Function(ts) ts.CompStreetZip.ToUpper.StartsWith(postalCode.ToUpper)).ToList()
            'Dim tariffShippers = target.GetTariffShippersByCarrier(2, 0).Where(Function(ts) ts.CompStreetZip.ToUpper = postalCode.ToUpper)).ToList()

            If results Is Nothing OrElse results.Count() < 1 Then
                Assert.Fail(strTestName & " failed,  no records.")
            End If
            System.Diagnostics.Debug.WriteLine(strTestName & " found " & results.Count() & " records.")
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & strTestName & ": {0} ", ex.Message)
        Finally
            'place clean up code here
        End Try

    End Sub


    <TestMethod()> _
    Public Sub GetTariffShippersByCarrierTest()
        'connect to  database before getting book records overwrite the defaul
        testParameters.DBServer = "NGLRDP06D"
        testParameters.Database = "NGLMAS2013DEV"
        testParameters.UserName = "NGL\rramsey"

        Dim strTestName As String = "GetTariffShippersByCarrierTest"

        Try

            Dim CarrierControl As Integer = 0
            Dim CompControl As Integer = 0
            Dim TempType As Integer = 0
            Dim EffStartDateFrom As System.Nullable(Of Date) = Nothing
            Dim EffStartDateTo As System.Nullable(Of Date) = Nothing
            Dim EffEndDateFrom As System.Nullable(Of Date) = Nothing
            Dim EffEndDateTo As System.Nullable(Of Date) = Nothing
            Dim TariffType As Integer = 0
            Dim ModeType As Integer = 0
            Dim Revision As Integer = 0
            Dim ShowActiveContracts As Boolean = True
            Dim Approved As Boolean = True
            Dim Rejected As Boolean = False
            Dim Outbound As Boolean = True
            Dim AgentControl As Integer = 0
            Dim page As Integer = 1
            Dim pagesize As Integer = 1000
            Dim postalCode As String = "606"
            Dim target As New DAL.NGLCarrTarContractData(testParameters)
            'GetTariffShippersByCarrier(ByVal sortKey As Integer, ByVal CarrierControl As Integer) As DTO.vComp()
            Dim results = target.GetTariffShippersByCarrierAndZip(3, 0, postalCode, 20)
            'Dim tariffShippers = (From ts In target.GetTariffShippersByCarrier(2, 0)
            '                                                   Where ts.CompStreetZip.ToUpper.StartsWith(postalCode.ToUpper)
            '                                                  Select ts)
            ' Dim tariffShippers = target.GetTariffShippersByCarrier(2, 0).Where(Function(ts) ts.CompStreetZip.ToUpper.StartsWith(postalCode.ToUpper)).ToList()
            'Dim tariffShippers = target.GetTariffShippersByCarrier(2, 0).Where(Function(ts) ts.CompStreetZip.ToUpper = postalCode.ToUpper)).ToList()

            If results Is Nothing OrElse results.Count() < 1 Then
                Assert.Fail(strTestName & " failed,  no records.")
            End If
            System.Diagnostics.Debug.WriteLine(strTestName & " found " & results.Count() & " records.")
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & strTestName & ": {0} ", ex.Message)
        Finally
            'place clean up code here
        End Try

    End Sub

    <TestMethod()> _
    Public Sub GetTariffShippersByCarrierAndZip()
        'connect to  database before getting book records overwrite the defaul
        testParameters.DBServer = "NGLRDP07D"
        testParameters.Database = "NGLMASPROD"
        testParameters.WCFAuthCode = "NGLSystem"
        testParameters.ConnectionString = "Server=NGLRDP07D;User ID=nglweb;Password=5529;Database=NGLMASPROD"
        testParameters.UserName = "NGL\John Riske"

        Dim strTestName As String = "GetTariffShippersByCarrierAndZip"
        Dim sortKey As Integer = 2
        Dim CarrierControl As Integer = 0
        Dim PostalCode As String = "060"
        Dim Take As Integer = 20
        Try
            Dim target As New DAL.NGLCarrTarContractData(testParameters)

            Dim results As DTO.CityStateZipTariffLookup() = target.GetTariffShippersByCarrierAndZip(sortKey, CarrierControl, PostalCode, Take)
            If results Is Nothing OrElse results.Count() < 1 Then
                Assert.Fail(strTestName & " failed,  no records.")
            End If
            System.Diagnostics.Debug.WriteLine(strTestName & " found " & results.Count() & " records.")
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & strTestName & ": {0} ", ex.Message)
        Finally
            'place clean up code here
        End Try
    End Sub


    <TestMethod()> _
    Public Sub GetCarrierLiteTest()
        'connect to  database before getting book records overwrite the defaul
        testParameters.DBServer = "NGLRDP06D"
        testParameters.Database = "NGLMASDEV7051"
        testParameters.UserName = "NGL\rramsey"
        Dim strTestName As String = "GetCarrierLiteTest"

        Try

            Dim CarrierControl As Integer = 9613 'budreck 35
            Dim target As New NGL.FreightMaster.Data.NGLCarrierData(testParameters)
            Dim CarrierArray = target.GetCarriersFiltered(0, Nothing, True, 1, 0, 0, 10)
            System.Diagnostics.Debug.WriteLine("")
            Dim lCarrierControls = target.GetCarrierControls()
            If Not lCarrierControls Is Nothing AndAlso lCarrierControls.Count() <> 0 Then
                For Each c In lCarrierControls
                    System.Diagnostics.Debug.Write("Carrier Control: " & c.ToString())
                    Dim results = target.GetCarrier(c)
                    If results Is Nothing Then
                        Assert.Fail(strTestName & " failed,  no records.")
                        Return
                    End If
                    System.Diagnostics.Debug.Write("Carrier Number: " & results.CarrierNumber.ToString())
                    System.Diagnostics.Debug.WriteLine("")
                Next
            End If
            
            System.Diagnostics.Debug.WriteLine("Test Complete")
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & strTestName & ": {0} ", ex.Message)
        Finally
            'place clean up code here
        End Try

    End Sub
End Class
