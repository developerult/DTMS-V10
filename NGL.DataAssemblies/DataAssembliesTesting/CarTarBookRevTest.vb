Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports System
Imports Ngl.FM.CarTar
Imports DAL = Ngl.FreightMaster.Data
Imports DTO = NGL.FreightMaster.Data.DataTransferObjects
Imports BLL = NGL.FM.BLL

<TestClass()> Public Class CarTarBookRevTest
    Inherits TestBase

    <TestMethod> _
    Public Sub EstimateCarriersByCostShieldsTest()
        Try
            'connect to  database before getting book records overwrite the defaul
            testParameters.DBServer = "NGLRDP05D"
            testParameters.Database = "SHIELDSMASPROD"
            Dim BookProNumber As String = "YAK768132" ' "YAK756769" 'YAK756769
            Dim oBook As DTO.Book = getBookByPro(BookProNumber)
            If oBook Is Nothing Then
                Assert.Fail("no booking records found for Book Pro Number: {0}", BookProNumber)
                Return
            End If
            Dim BookControl As Integer = oBook.BookControl
            Dim target As New Ngl.FM.CarTar.BookRev(testParameters)
            Dim carriercontrol As Integer = 1
            Dim results As DTO.CarrierCostResults = target.estimatedCarriersByCost(BookControl, carriercontrol)

            If results Is Nothing OrElse results.CarriersByCost Is Nothing Then
                Assert.Fail("Call to estimatedCarriersByCost Failure for BookControl: {0}", BookControl)
                Return
            End If

            For Each l As DTO.NGLMessage In results.Log
                System.Diagnostics.Debug.WriteLine(l.Message)
                If l.Control <> 0 Then
                    System.Diagnostics.Debug.WriteLine("Link to " + l.ControlReferenceName + " Control # " + l.Control.ToString())
                End If
            Next

            System.Diagnostics.Debug.WriteLine("Provider{0}| Min{0}| Est{0}| Rate{0}| Type ", vbTab)
            For Each c In results.CarriersByCost
                System.Diagnostics.Debug.WriteLine("{1}{0}| {2}{0}| {3}{0}| {4}{0}| {5} ", vbTab, c.CarrierName, c.CarrierMinCost, c.CarrierCost, c.CarrierRate, c.RateTypeName)
            Next

            'Assert.AreEqual(oData.BookCarrierControl, 9837);
            'Assert.AreEqual(oData.BookTotalBFC, (decimal)7.41);
            Dim ct As Integer = results.CarriersByCost.Count

        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For AssignCarrierCons. Test: {0} ", ex.Message)
            'place clean up code here
        Finally
        End Try
    End Sub


    <TestMethod> _
    Public Sub EstimateCarriersByCostDevTest()
        Try
            'connect to  database before getting book records overwrite the defaul
            testParameters.DBServer = "NGLRDP06D"
            testParameters.Database = "NGLMASDEV7051"
            Dim BookProNumber As String = "GPB757881" 'Rob1120" ' "VJI757625"
            Dim oBook As DTO.Book = getBookByPro(BookProNumber)
            If oBook Is Nothing Then
                Assert.Fail("no booking records found for Book Pro Number: {0}", BookProNumber)
                Return
            End If
            Dim BookControl As Integer = oBook.BookControl
            Dim target As New Ngl.FM.CarTar.BookRev(testParameters)

            Dim results As DTO.CarrierCostResults = target.estimatedCarriersByCost(BookControl, 0)

            If results Is Nothing OrElse results.CarriersByCost Is Nothing Then
                Assert.Fail("Call to estimatedCarriersByCost Failure for BookControl: {0}", BookControl)
                Return
            End If

            For Each l As DTO.NGLMessage In results.Log
                System.Diagnostics.Debug.WriteLine(l.Message)
                If l.Control <> 0 Then
                    System.Diagnostics.Debug.WriteLine("Link to " + l.ControlReferenceName + " Control # " + l.Control.ToString())
                End If
            Next

            System.Diagnostics.Debug.WriteLine("Provider{0}| Min{0}| Est{0}| Rate{0}| Type ", vbTab)
            For Each c In results.CarriersByCost
                System.Diagnostics.Debug.WriteLine("{1}{0}| {2}{0}| {3}{0}| {4}{0}| {5} ", vbTab, c.CarrierName, c.CarrierMinCost, c.CarrierCost, c.CarrierRate, c.RateTypeName)
            Next

            'Assert.AreEqual(oData.BookCarrierControl, 9837);
            'Assert.AreEqual(oData.BookTotalBFC, (decimal)7.41);
            Dim ct As Integer = results.CarriersByCost.Count

        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For AssignCarrierCons. Test: {0} ", ex.Message)
            'place clean up code here
        Finally
        End Try
    End Sub

    ''' <summary>
    ''' will only work if purchase order P106029 exists
    ''' used to debug tariff issues where line haul = $0.10
    ''' </summary>
    ''' <remarks></remarks>
    <TestMethod> _
    Public Sub EstimateCarriersByCostTest07D()
        Try
            'connect to  database before getting book records overwrite the defaul
            testParameters.DBServer = "NGLRDP07D"
            testParameters.Database = "NGLMASPROD"
            testParameters.ConnectionString = "Server=NGLRDP07D;User ID=nglweb;Password=5529;Database=NGLMASPROD"
            testParameters.UserName = "NGL\rramsey"

            Dim target As New Ngl.FM.CarTar.BookRev(testParameters)
            'test for Purchase order P106029 -- 351
            'test for ProNumber BLU756218  -- 
            Dim bookcontrol As Integer = 4769 '4699
            Dim results As DTO.CarrierCostResults = target.estimatedCarriersByCost(bookcontrol, 0, True, False, False, True, 0, 0, 0, "", 0, 0, 0, 1, 1000)

            If results Is Nothing OrElse results.CarriersByCost Is Nothing Then
                Assert.Fail("Call to EstimateCarriersByCostTest07D Failure for Purchase Order: P106029")
                Return
            End If

            For Each l As DTO.NGLMessage In results.Log
                System.Diagnostics.Debug.WriteLine(l.Message)
                If l.Control <> 0 Then
                    System.Diagnostics.Debug.WriteLine("Link to " + l.ControlReferenceName + " Control # " + l.Control.ToString())
                End If
            Next

            System.Diagnostics.Debug.WriteLine("Provider{0}| Min{0}| Est{0}| Rate{0}| Type ", vbTab)
            For Each c In results.CarriersByCost
                System.Diagnostics.Debug.WriteLine("{1}{0}| {2}{0}| {3}{0}| {4}{0}| {5} ", vbTab, c.CarrierName, c.CarrierMinCost, c.CarrierCost, c.CarrierRate, c.RateTypeName)
            Next

            'Assert.AreEqual(oData.BookCarrierControl, 9837);
            'Assert.AreEqual(oData.BookTotalBFC, (decimal)7.41);
            Dim ct As Integer = results.CarriersByCost.Count

        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For AssignCarrierCons. Test: {0} ", ex.Message)
            'place clean up code here
        Finally
        End Try
    End Sub


    <TestMethod> _
    Public Sub AssignSelectedCarrierShieldsTest()
        Try
            'connect to  database before getting book records overwrite the defaul
            testParameters.DBServer = "NGLRDP05D"
            testParameters.Database = "SHIELDSMASPROD"
            Dim BookProNumber As String = "YAK756769" 'YAK756769
            Dim oBook As DTO.Book = getBookByPro(BookProNumber)
            If oBook Is Nothing Then
                Assert.Fail("no booking records found for Book Pro Number: {0}", BookProNumber)
                Return
            End If
            Dim BookControl As Integer = oBook.BookControl
            Dim CarrierControl As Integer = 6 ' = Conway
            Dim CarrTarEquipMatControl As Integer = 0
            Dim CarrTarEquipControl As Integer = 0
            'Test for assign with existing carrier "Normal"
            Dim CalculationType As DAL.Utilities.AssignCarrierCalculationType = DAL.Utilities.AssignCarrierCalculationType.Normal
            Dim prefered As Boolean = True
            Dim noLateDelivery As Boolean = False
            Dim validated As Boolean = False
            Dim optimizeByCapacity As Boolean = True
            Dim modeTypeControl As Integer = 3
            Dim tempType As Integer = 0
            Dim tariffTypeControl As Integer = 0
            Dim carrTarEquipMatClass As String = Nothing
            Dim carrTarEquipMatClassTypeControl As Integer = 0
            Dim carrTarEquipMatTarRateTypeControl As Integer = 0
            Dim agentControl As Integer = 0
            Dim target As New NGL.FM.CarTar.BookRev(testParameters)

            Dim results As DTO.CarrierCostResults = target.assignCarrier(BookControl,
                                        CarrierControl,
                                        CalculationType,
                                        CarrTarEquipMatControl,
                                        CarrTarEquipControl,
                                        prefered,
                                        noLateDelivery,
                                        validated,
                                        optimizeByCapacity,
                                        modeTypeControl,
                                        tempType,
                                        tariffTypeControl,
                                        carrTarEquipMatClass,
                                        carrTarEquipMatClassTypeControl,
                                        carrTarEquipMatTarRateTypeControl,
                                        agentControl,
                                        1,
                                        1000,
                                        Nothing)

            If results Is Nothing OrElse results.Success = False Then
                Assert.Fail("Call to assignCarrier Failed for BookPro: {0}", BookProNumber)
                Return
            End If

            For Each l As DTO.NGLMessage In results.Log
                System.Diagnostics.Debug.WriteLine(l.Message)
                If l.Control <> 0 Then
                    System.Diagnostics.Debug.WriteLine("Link to " + l.ControlReferenceName + " Control # " + l.Control.ToString())
                End If
            Next


            Dim ct As Integer = results.CarriersByCost.Count

        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For AssignSelectedCarrierTest. Test: {0} ", ex.Message)
            'place clean up code here
        Finally
        End Try
    End Sub

    <TestMethod> _
    Public Sub RecalculateCostsInImport()

        'connect to  database before getting book records overwrite the defaul
        testParameters.DBServer = "NGLRDP07D"
        testParameters.Database = "NGLMASPRODBON"
        Dim bookPronumber = "PER762809"
        Dim BookControl As Integer = 8885
        Try
            Dim target As New NGL.FM.BLL.NGLBookRevenueBLL(testParameters)
            Dim results = target.UpdateCarrierCost(BookControl)
            'Dim results = target.AssignCarrier(BookControl, NGL.FreightMaster.Data.Utilities.AssignCarrierCalculationType.UpdateCarrier)
            'AssignOrUpdateCarrier(ByVal BookControl As Integer, Optional ByVal OptmisticConcurrencyOn As Boolean = True)
            If results Is Nothing OrElse results.Success = False Then
                Assert.Fail("RecalculateFreightCostsTest Failed ")
                Return
            End If

            For Each l As DTO.NGLMessage In results.Log
                System.Diagnostics.Debug.WriteLine(l.Message)
                If l.Control <> 0 Then
                    System.Diagnostics.Debug.WriteLine("Link to " + l.ControlReferenceName + " Control # " + l.Control.ToString())
                End If
            Next


            Dim ct As Integer = results.CarriersByCost.Count

        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For RecalculateFreightCostsTest. Test: {0} ", ex.Message)
            'place clean up code here
        Finally
        End Try

    End Sub


    <TestMethod> _
    Public Sub RecalculateCostsUsingLineHaulTest()
        Dim strSource As String = "RecalculateCostsUsingLineHaulTest"
        'connect to  database before getting book records overwrite the defaul
        testParameters.DBServer = "nglrdp07d"
        testParameters.Database = "NGLMASprod"
        Dim bookPronumber = "WAU-1-1801"
        Dim BookControl As Integer = 5133
        Try
            Dim target As New Ngl.FM.BLL.NGLBookRevenueBLL(testParameters)
            Dim results = target.RecalculateBookRevenueFreightCosts(BookControl)
            'Dim results = target.AssignCarrier(BookControl, NGL.FreightMaster.Data.Utilities.AssignCarrierCalculationType.UpdateCarrier)
            'AssignOrUpdateCarrier(ByVal BookControl As Integer, Optional ByVal OptmisticConcurrencyOn As Boolean = True)
            If results Is Nothing Then
                Assert.Fail(strSource & " Failed ")
                Return
            End If



        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & strSource & " . Test: {0} ", ex.Message)
            'place clean up code here
        Finally
        End Try

    End Sub


    <TestMethod> _
    Public Sub RecalculateFreightCostsTest()
        Try
            'connect to  database before getting book records overwrite the defaul
            testParameters.DBServer = "NGLRDP07D"
            testParameters.Database = "NGLMASPROD"
            Dim bookPronumber = "BLU756792"
            Dim obookrevs = getBookRevsByBookControl(950)
            If obookrevs Is Nothing OrElse obookrevs.Count() < 1 Then
                Assert.Fail("no booking records found ")
                Return
            End If

            Dim CalculationType As DAL.Utilities.AssignCarrierCalculationType = DAL.Utilities.AssignCarrierCalculationType.Recalculate

            Dim target As New NGL.FM.BLL.NGLBookRevenueBLL(testParameters)

            Dim results As DTO.CarrierCostResults = target.AssignCarrier(obookrevs(0), CalculationType)

            If results Is Nothing OrElse results.Success = False Then
                Assert.Fail("RecalculateFreightCostsTest Failed ")
                Return
            End If

            For Each l As DTO.NGLMessage In results.Log
                System.Diagnostics.Debug.WriteLine(l.Message)
                If l.Control <> 0 Then
                    System.Diagnostics.Debug.WriteLine("Link to " + l.ControlReferenceName + " Control # " + l.Control.ToString())
                End If
            Next


            Dim ct As Integer = results.CarriersByCost.Count

        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For RecalculateFreightCostsTest. Test: {0} ", ex.Message)
            'place clean up code here
        Finally
        End Try
    End Sub



    <TestMethod> _
    Public Sub CanadianCarrierDiscountShieldsTest()
        Try
            'connect to  database before getting book records overwrite the defaul
            testParameters.DBServer = "NGLRDP05D"
            testParameters.Database = "SHIELDSMASPROD"
            Dim BookProNumber As String = "YAK756007"
            Dim oBook As DTO.Book = getBookByPro(BookProNumber)
            If oBook Is Nothing Then
                Assert.Fail("no booking records found for Book Pro Number: {0}", BookProNumber)
                Return
            End If
            Dim BookControl As Integer = oBook.BookControl
            Dim CarrierControl As Integer = oBook.BookCarrierControl
            Dim CarrTarEquipMatControl As Integer = oBook.BookCarrTarEquipMatControl
            Dim CarrTarEquipControl As Integer = oBook.BookCarrTarEquipControl
            'Test for assign with existing carrier "Normal"
            Dim CalculationType As DAL.Utilities.AssignCarrierCalculationType = DAL.Utilities.AssignCarrierCalculationType.UpdateCarrier
            Dim prefered As Boolean = True
            Dim noLateDelivery As Boolean = False
            Dim validated As Boolean = False
            Dim optimizeByCapacity As Boolean = False
            Dim modeTypeControl As Integer = oBook.BookModeTypeControl
            Dim tempType As Integer = 0
            Dim tariffTypeControl As Integer = 0
            Dim carrTarEquipMatClass As String = Nothing
            Dim carrTarEquipMatClassTypeControl As Integer = 0
            Dim carrTarEquipMatTarRateTypeControl As Integer = 0
            Dim agentControl As Integer = 0
            Dim target As New NGL.FM.CarTar.BookRev(testParameters)

            Dim results As DTO.CarrierCostResults = target.assignCarrier(BookControl,
                                        CarrierControl,
                                        CalculationType,
                                        CarrTarEquipMatControl,
                                        CarrTarEquipControl,
                                        prefered,
                                        noLateDelivery,
                                        validated,
                                        optimizeByCapacity,
                                        modeTypeControl,
                                        tempType,
                                        tariffTypeControl,
                                        carrTarEquipMatClass,
                                        carrTarEquipMatClassTypeControl,
                                        carrTarEquipMatTarRateTypeControl,
                                        agentControl,
                                        1,
                                        1000,
                                        Nothing)

            If results Is Nothing OrElse results.Success = False Then
                Assert.Fail("Call to assignCarrier Failed for BookPro: {0}", BookProNumber)
                Return
            End If

            For Each l As DTO.NGLMessage In results.Log
                System.Diagnostics.Debug.WriteLine(l.Message)
                If l.Control <> 0 Then
                    System.Diagnostics.Debug.WriteLine("Link to " + l.ControlReferenceName + " Control # " + l.Control.ToString())
                End If
            Next


            Dim ct As Integer = results.CarriersByCost.Count

        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For AssignSelectedCarrierTest. Test: {0} ", ex.Message)
            'place clean up code here
        Finally
        End Try
    End Sub


    <TestMethod> _
    Public Sub AssignSelectedCarrierDevTest()
        Try
            'connect to  database before getting book records overwrite the defaul

            testParameters.DBServer = "172.16.151.56"
            testParameters.Database = "NGLMASDEV"
            testParameters.ConnectionString = "Server=172.16.151.56;User ID=nglweb;Password=5529;Database=NGLMASDEV"
            testParameters.UserName = "colortech1\tmsservice"

            Dim BookProNumber As String = "MOB-12-1172"
            Dim oBook As DTO.Book = getBookByPro(BookProNumber)
            If oBook Is Nothing Then
                Assert.Fail("no booking records found for Book Pro Number: {0}", BookProNumber)
                Return
            End If
            Dim BookControl As Integer = oBook.BookControl
            Dim CarrierControl As Integer = 0
            Dim CarrTarEquipMatControl As Integer = 0
            Dim CarrTarEquipControl As Integer = 0
            'Test for assign with existing carrier "Normal"
            Dim CalculationType As DAL.Utilities.AssignCarrierCalculationType = DAL.Utilities.AssignCarrierCalculationType.Normal
            'Dim prefered As Boolean = True
            'Dim noLateDelivery As Boolean = False
            'Dim validated As Boolean = False
            'Dim optimizeByCapacity As Boolean = True
            'Dim modeTypeControl As Integer = 3
            'Dim tempType As Integer = 0
            'Dim tariffTypeControl As Integer = 0
            'Dim carrTarEquipMatClass As String = Nothing
            'Dim carrTarEquipMatClassTypeControl As Integer = 0
            'Dim carrTarEquipMatTarRateTypeControl As Integer = 0
            'Dim agentControl As Integer = 0

            Dim prefered As Boolean = False
            Dim noLateDelivery As Boolean = False
            Dim validated As Boolean = False
            Dim optimizeByCapacity As Boolean = False
            Dim modeTypeControl As Integer = 3
            Dim tempType As Integer = 0
            Dim tariffTypeControl As Integer = 0
            Dim carrTarEquipMatClass As String = Nothing
            Dim carrTarEquipMatClassTypeControl As Integer = 0
            Dim carrTarEquipMatTarRateTypeControl As Integer = 0
            Dim agentControl As Integer = 0

            Dim target As New NGL.FM.CarTar.BookRev(testParameters)

            Dim results As DTO.CarrierCostResults = target.assignCarrier(BookControl,
                                        CarrierControl,
                                        CalculationType,
                                        CarrTarEquipMatControl,
                                        CarrTarEquipControl,
                                        prefered,
                                        noLateDelivery,
                                        validated,
                                        optimizeByCapacity,
                                        modeTypeControl,
                                        tempType,
                                        tariffTypeControl,
                                        carrTarEquipMatClass,
                                        carrTarEquipMatClassTypeControl,
                                        carrTarEquipMatTarRateTypeControl,
                                        agentControl,
                                        1,
                                        1000,
                                        Nothing)

            If results Is Nothing OrElse results.Success = False Then
                Assert.Fail("Call to assignCarrier Failed for BookPro: {0}", BookProNumber)
                Return
            End If

            For Each l As DTO.NGLMessage In results.Log
                System.Diagnostics.Debug.WriteLine(l.Message)
                If l.Control <> 0 Then
                    System.Diagnostics.Debug.WriteLine("Link to " + l.ControlReferenceName + " Control # " + l.Control.ToString())
                End If
            Next


            Dim ct As Integer = results.CarriersByCost.Count

        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For AssignSelectedCarrierTest. Test: {0} ", ex.Message)
            'place clean up code here
        Finally
        End Try
    End Sub


    <TestMethod> _
    Public Sub AssignAssignedCarrierTest()
        Try
            'connect to  database before getting book records overwrite the defaul
            testParameters.DBServer = "NGLRDP06D"
            testParameters.Database = "NGLMASDEV7051"
            Dim BookProNumber As String = "GPB757881"
            Dim oBook As DTO.Book = getBookByPro(BookProNumber)
            If oBook Is Nothing Then
                Assert.Fail("no booking records found for Book Pro Number: {0}", BookProNumber)
                Return
            End If
            Dim BookControl As Integer = oBook.BookControl
            Dim CarrierControl As Integer = 9658
            Dim CarrTarEquipMatControl As Integer = 0
            Dim CarrTarEquipControl As Integer = 0
            'Test for assign with existing carrier "Normal"
            Dim CalculationType As DAL.Utilities.AssignCarrierCalculationType = DAL.Utilities.AssignCarrierCalculationType.Normal
            Dim prefered As Boolean = True
            Dim noLateDelivery As Boolean = False
            Dim validated As Boolean = False
            Dim optimizeByCapacity As Boolean = True
            Dim modeTypeControl As Integer = 3
            Dim tempType As Integer = 0
            Dim tariffTypeControl As Integer = 0
            Dim carrTarEquipMatClass As String = Nothing
            Dim carrTarEquipMatClassTypeControl As Integer = 0
            Dim carrTarEquipMatTarRateTypeControl As Integer = 0
            Dim agentControl As Integer = 0
            Dim target As New Ngl.FM.CarTar.BookRev(testParameters)

            Dim results As DTO.CarrierCostResults = target.assignShipCarrier(BookControl,
                                        CarrierControl,
                                        CalculationType,
                                        CarrTarEquipMatControl,
                                        CarrTarEquipControl,
                                        prefered,
                                        noLateDelivery,
                                        validated,
                                        optimizeByCapacity,
                                        modeTypeControl,
                                        tempType,
                                        tariffTypeControl,
                                        carrTarEquipMatClass,
                                        carrTarEquipMatClassTypeControl,
                                        carrTarEquipMatTarRateTypeControl,
                                        agentControl,
                                        1,
                                        1000,
                                        Nothing)

            If results Is Nothing OrElse results.Success = False Then
                Assert.Fail("Call to assignCarrier Failed for BookPro: {0}", BookProNumber)
                Return
            End If

            For Each l As DTO.NGLMessage In results.Log
                System.Diagnostics.Debug.WriteLine(l.Message)
                If l.Control <> 0 Then
                    System.Diagnostics.Debug.WriteLine("Link to " + l.ControlReferenceName + " Control # " + l.Control.ToString())
                End If
            Next


            Dim ct As Integer = results.CarriersByCost.Count

        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For AssignSelectedCarrierTest. Test: {0} ", ex.Message)
            'place clean up code here
        Finally
        End Try
    End Sub


    <TestMethod> _
    Public Sub UpdateOrAssignCarrierTest()
        Try
            'connect to  database before getting book records overwrite the defaul
            testParameters.DBServer = "NGLRDP06D"
            testParameters.Database = "NGLMAS2013DEV"
            Dim BookProNumber As String = "VJI757618"
            Dim oBook As DTO.Book = getBookByPro(BookProNumber)
            If oBook Is Nothing Then
                Assert.Fail("no booking records found for Book Pro Number: {0}", BookProNumber)
                Return
            End If
            Dim BookControl As Integer = oBook.BookControl
            Dim CarrierControl As Integer = oBook.BookCarrierControl
            Dim CarrTarEquipMatControl As Integer = oBook.BookCarrTarEquipMatControl
            Dim CarrTarEquipControl As Integer = oBook.BookCarrTarEquipControl
            'Test for assign with existing carrier "Normal"
            Dim CalculationType As DAL.Utilities.AssignCarrierCalculationType = DAL.Utilities.AssignCarrierCalculationType.UpdateCarrier
            Dim prefered As Boolean = True
            Dim noLateDelivery As Boolean = True
            Dim validated As Boolean = True
            Dim optimizeByCapacity As Boolean = True
            Dim modeTypeControl As Integer = 3
            Dim tempType As Integer = 0
            Dim tariffTypeControl As Integer = 0
            Dim carrTarEquipMatClass As String = Nothing
            Dim carrTarEquipMatClassTypeControl As Integer = 0
            Dim carrTarEquipMatTarRateTypeControl As Integer = 0
            Dim agentControl As Integer = 0
            Dim target As New NGL.FM.CarTar.BookRev(testParameters)

            Dim results As DTO.CarrierCostResults = target.assignCarrier(BookControl,
                                            CarrierControl,
                                            CalculationType,
                                            CarrTarEquipMatControl,
                                            CarrTarEquipControl,
                                            prefered,
                                            noLateDelivery,
                                            validated,
                                            optimizeByCapacity,
                                            modeTypeControl,
                                            tempType,
                                            tariffTypeControl,
                                            carrTarEquipMatClass,
                                            carrTarEquipMatClassTypeControl,
                                            carrTarEquipMatTarRateTypeControl,
                                            agentControl,
                                            1,
                                            1000,
                                            Nothing)

            If results Is Nothing OrElse results.Success = False Then
                Assert.Fail("Call to assignCarrier Failed for BookPro: {0}", BookProNumber)
                Return
            End If

            For Each l As DTO.NGLMessage In results.Log
                System.Diagnostics.Debug.WriteLine(l.Message)
                If l.Control <> 0 Then
                    System.Diagnostics.Debug.WriteLine("Link to " + l.ControlReferenceName + " Control # " + l.Control.ToString())
                End If
            Next


            Dim ct As Integer = results.CarriersByCost.Count

        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For AssignSelectedCarrierTest. Test: {0} ", ex.Message)
            'place clean up code here
        Finally
        End Try
    End Sub

    <TestMethod> _
    Public Sub EstimateCarrierCostTestFiltered()
        'connect to  database before getting book records overwrite the defaul
        testParameters.DBServer = "NGLRDP07D"
        testParameters.Database = "NGLMASPRODBon"

        Dim strTestName As String = "EstimateCarrierCostTestFiltered"
        Try
            Dim bookControl As Integer = 7167
            Dim carrierControl As Integer = 0
            Dim prefered As Boolean = True
            Dim noLateDelivery As Boolean = False
            Dim validated As Boolean = False
            Dim optimizeByCapacity As Boolean = True
            Dim modeTypeControl As Integer = 0
            Dim tempType As Integer = 0
            Dim tariffTypeControl As Integer = 0
            Dim carrTarEquipMatClass As String = Nothing
            Dim carrTarEquipMatClassTypeControl As Integer = 0
            Dim carrTarEquipMatTarRateTypeControl As Integer = 0
            Dim agentControl As Integer = 0
            Dim page As Integer = 1
            Dim pagesize As Integer = 1000

            Dim target As New Ngl.FM.CarTar.BookRev(testParameters)
            Dim results As DTO.CarrierCostResults = target.estimatedCarriersByCost(bookControl,
                                            carrierControl,
                                            prefered,
                                            noLateDelivery,
                                            validated,
                                            optimizeByCapacity,
                                            modeTypeControl,
                                            tempType,
                                            tariffTypeControl,
                                            carrTarEquipMatClass,
                                            carrTarEquipMatClassTypeControl,
                                            carrTarEquipMatTarRateTypeControl,
                                            agentControl,
                                            page,
                                            pagesize)

            If results Is Nothing OrElse results.CarriersByCost Is Nothing Then
                Assert.Fail(strTestName & " failed for BookControl: {0}", bookControl)
                Return
            End If

            For Each l As DTO.NGLMessage In results.Log
                System.Diagnostics.Debug.WriteLine(l.Message)
                If l.Control <> 0 Then
                    System.Diagnostics.Debug.WriteLine("Link to " + l.ControlReferenceName + " Control # " + l.Control.ToString())
                End If
            Next

            'Assert.AreEqual(oData.BookCarrierControl, 9837);
            'Assert.AreEqual(oData.BookTotalBFC, (decimal)7.41);
            Dim ct As Integer = results.CarriersByCost.Count


        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail(strTestName & " unexpected Error: {0} ", ex.Message)
            'place clean up code here
        Finally
        End Try
    End Sub


    <TestMethod> _
    Public Sub EstimateCarrierCostTest()
        Try
            'connect to  database before getting book records overwrite the defaul
            testParameters.DBServer = "NGLRDP05D"
            testParameters.Database = "SHIELDSMASPROD"
            Dim oBook As DTO.Book = getBookByPro("YAK757267")
            If oBook Is Nothing Then
                Assert.Fail("no booking record found")
                Return
            End If
            Dim BookControl As Integer = oBook.BookControl
            'Get the Carrier Data
            Dim CarrierControl As Integer = 0
            Dim oCarrier As DTO.Carrier = getCarrierByName("Old Dominion")
            If oCarrier Is Nothing Then
                Assert.Fail("no carrier data found")
                Return
            End If
            CarrierControl = oCarrier.CarrierControl
            Dim target As New NGL.FM.CarTar.BookRev(testParameters)

            Dim results As DTO.CarrierCostResults = target.estimatedCarriersByCost(BookControl, CarrierControl)

            If results Is Nothing OrElse results.CarriersByCost Is Nothing Then
                Assert.Fail("Call to estimatedCarriersByCost Failure for BookControl: {0}", BookControl)
                Return
            End If

            For Each l As DTO.NGLMessage In results.Log
                System.Diagnostics.Debug.WriteLine(l.Message)
                If l.Control <> 0 Then
                    System.Diagnostics.Debug.WriteLine("Link to " + l.ControlReferenceName + " Control # " + l.Control.ToString())
                End If
            Next

            'Assert.AreEqual(oData.BookCarrierControl, 9837);
            'Assert.AreEqual(oData.BookTotalBFC, (decimal)7.41);
            Dim ct As Integer = results.CarriersByCost.Count
        Catch ex As Exception
            Assert.Fail("Unexpected Error For AssignCarrierCons. Test: {0} ", ex.Message)
            'place clean up code here
        Finally
        End Try
    End Sub


    <TestMethod> _
    Public Sub GetSampleCarrierProNumberTest()
        Try
            'connect to  database before getting book records overwrite the defaul
            testParameters.DBServer = "NGLRDP05D"
            testParameters.Database = "SHIELDSMASPROD"
            Dim CarrProControl = 1 '=	29	YRC
            Dim oCarrierbll As New BLL.NGLCarrierBLL(testParameters)

            For intSeedFactor As Integer = 0 To 9
                If intSeedFactor = 4 Then
                    Console.WriteLine("Testing 5th seed")
                End If
                Dim results As DTO.CarrierPRONumberResult = oCarrierbll.GetCarrierSampleProNumber(CarrProControl, intSeedFactor)



                If results Is Nothing OrElse String.IsNullOrWhiteSpace(results.CarrierProNumber) Then
                    Assert.Fail("Cannot get sample carrier pro number")
                    Return
                Else
                    System.Diagnostics.Debug.WriteLine("Carrier Pro Number: {0}", results.CarrierProNumber)
                End If
            Next

            System.Diagnostics.Debug.WriteLine("Finished")



        Catch ex As Exception
            Assert.Fail("Unexpected Error For GetSampleCarrierProNumberTest: {0} ", ex.Message)
            'place clean up code here
        Finally
        End Try
    End Sub



End Class
