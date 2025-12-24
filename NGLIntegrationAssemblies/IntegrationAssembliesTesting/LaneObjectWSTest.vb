Imports System
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports DAL = Ngl.FreightMaster.Data
Imports Ngl.FreightMaster.Integration

<TestClass()> Public Class LaneObjectWSTest
    Inherits TestBase

    <TestCategory("Nightly")>
    <TestMethod()>
    Public Sub LaneProcessObjectData70Test()
        testParameters.DBServer = "NGLRDP06D"
        testParameters.Database = "SHIELDSMASUnitTest"

        'Always overwrite the web.config when running unit test
        FileCopy("Web.config", "D:\HTTP\WSUnitTest70\Web.config")

        Dim intResult As Integer = 0
        Dim strLastError As String = ""

        Dim oLane As New NGLLaneObjectWebService.LaneObject
        oLane.Url = WSUrl & "LaneObject.ASMX"

        Dim oLaneDB As New DAL.NGLLaneData(testParameters)

        'Test creating a new Lane via web services
        Dim laneObj As New LaneTestObject
        laneObj = createNewLane()
        Dim getLaneDB As New Ngl.FreightMaster.Data.DataTransferObjects.Lane

        '******************** TEST INSERT NEW RECORD **********************
        Try
            'send the new lane data through the web services (70 version)
            Try
                intResult = oLane.ProcessData70(WSAuthCode, laneObj.LaneHeaders(), laneObj.LaneCals(), strLastError)
            Catch ex As Exception
                Assert.Fail("There was a problem with ProcessData70 in LaneObject.asmx: " & ex.Message)
            End Try

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

            'Try to get back the new record from the database
            Try
                getLaneDB = GetLaneFilteredByLaneNumber(laneObj.LaneHeaders(1).LaneNumber)
            Catch ex As Exception
                'If we can't get it back something must have gone wrong with the insert
                Assert.Fail("There was a problem when attempting to read the record back from the database: " & ex.Message)
            End Try

            'Check key fields and fields new to 70; check the original object vs the one read from the db after insert
            Dim exp = laneObj.LaneHeaders(1)
            Assert.AreEqual(exp.LaneName, getLaneDB.LaneName)
            Assert.AreEqual(exp.LaneNumber, getLaneDB.LaneNumber)
            Assert.AreEqual(exp.LaneLegalEntity, getLaneDB.LaneLegalEntity)
            Assert.AreEqual(exp.LaneOrigName, getLaneDB.LaneOrigName)
            Assert.AreEqual(exp.LaneOrigAddress1, getLaneDB.LaneOrigAddress1)
            Assert.AreEqual(exp.LaneOrigCity, getLaneDB.LaneOrigCity)
            Assert.AreEqual(exp.LaneOrigState, getLaneDB.LaneOrigState)
            Assert.AreEqual(exp.LaneOrigZip, getLaneDB.LaneOrigZip)
            Assert.AreEqual(exp.LaneDestName, getLaneDB.LaneDestName)
            Assert.AreEqual(exp.LaneDestAddress1, getLaneDB.LaneDestAddress1)
            Assert.AreEqual(exp.LaneDestCity, getLaneDB.LaneDestCity)
            Assert.AreEqual(exp.LaneDestState, getLaneDB.LaneDestState)
            Assert.AreEqual(exp.LaneDestZip, getLaneDB.LaneDestZip)
            Assert.AreEqual(exp.LaneRequiredOnTimeServiceLevel, getLaneDB.LaneRequiredOnTimeServiceLevel)
            Assert.AreEqual(exp.LaneCalcOnTimeServiceLevel, getLaneDB.LaneCalcOnTimeServiceLevel)
            Assert.AreEqual(exp.LaneCalcOnTimeNoMonthsUsed, getLaneDB.LaneCalcOnTimeNoMonthsUsed)
            Assert.AreEqual(exp.LaneUser1, getLaneDB.LaneUser1)
            Assert.AreEqual(exp.LaneUser2, getLaneDB.LaneUser2)
            Assert.AreEqual(exp.LaneUser3, getLaneDB.LaneUser3)
            Assert.AreEqual(exp.LaneUser4, getLaneDB.LaneUser4)

            '********************* TEST UPDATE RECORD ***********************

            '******* Allow Update Flag FALSE *******

            'First set the AllowUpdate parameter for all fields to false
            setAllowUpdateFlags(oLaneDB, 0)

            'Now try updating the record by changing some fields
            exp.LaneOrigCompNumber = "4"
            exp.LaneOrigName = "Mooresville"
            exp.LaneOrigAddress1 = "207 Talbert Point Dr."
            exp.LaneOrigCity = "Mooresville"
            exp.LaneOrigState = "NC"
            exp.LaneOrigZip = "28117"
            exp.LaneDestCompNumber = "3"
            exp.LaneDestName = "Axis"
            exp.LaneDestAddress1 = "1099 Pratt Blvd."
            exp.LaneDestCity = "Elk Grove Village"
            exp.LaneDestState = "IL"
            exp.LaneDestZip = "60007"
            exp.LaneRequiredOnTimeServiceLevel = 1
            exp.LaneCalcOnTimeServiceLevel = 1
            exp.LaneCalcOnTimeNoMonthsUsed = 1
            exp.LaneUser1 = "UPDATE"
            exp.LaneUser2 = "UPDATE"
            exp.LaneUser3 = "UPDATE"
            exp.LaneUser4 = "UPDATE"

            'send the new lane data through the web services (70 version)
            Try
                intResult = oLane.ProcessData70(WSAuthCode, laneObj.LaneHeaders(), laneObj.LaneCals(), strLastError)
            Catch ex As Exception
                Assert.Fail("There was a problem with ProcessData70 in LaneObject.asmx: " & ex.Message)
            End Try

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

            'Try to get back the updated record from the database
            Try
                getLaneDB = GetLaneFilteredByLaneNumber(laneObj.LaneHeaders(1).LaneNumber)
            Catch ex As Exception
                'If we can't get it back something must have gone wrong with the insert
                Assert.Fail("There was a problem when attempting to read the record back from the database: " & ex.Message)
            End Try

            'check to see if the field was changed -- it should still be the same 
            Assert.AreNotEqual(exp.LaneOrigName, getLaneDB.LaneOrigName)
            Assert.AreNotEqual(exp.LaneOrigAddress1, getLaneDB.LaneOrigAddress1)
            Assert.AreNotEqual(exp.LaneOrigCity, getLaneDB.LaneOrigCity)
            Assert.AreNotEqual(exp.LaneOrigState, getLaneDB.LaneOrigState)
            Assert.AreNotEqual(exp.LaneOrigZip, getLaneDB.LaneOrigZip)
            Assert.AreNotEqual(exp.LaneDestName, getLaneDB.LaneDestName)
            Assert.AreNotEqual(exp.LaneDestAddress1, getLaneDB.LaneDestAddress1)
            Assert.AreNotEqual(exp.LaneDestCity, getLaneDB.LaneDestCity)
            Assert.AreNotEqual(exp.LaneDestState, getLaneDB.LaneDestState)
            Assert.AreNotEqual(exp.LaneDestZip, getLaneDB.LaneDestZip)
            Assert.AreNotEqual(exp.LaneRequiredOnTimeServiceLevel, getLaneDB.LaneRequiredOnTimeServiceLevel)
            Assert.AreNotEqual(exp.LaneCalcOnTimeServiceLevel, getLaneDB.LaneCalcOnTimeServiceLevel)
            Assert.AreNotEqual(exp.LaneCalcOnTimeNoMonthsUsed, getLaneDB.LaneCalcOnTimeNoMonthsUsed)
            Assert.AreNotEqual(exp.LaneUser1, getLaneDB.LaneUser1)
            Assert.AreNotEqual(exp.LaneUser2, getLaneDB.LaneUser2)
            Assert.AreNotEqual(exp.LaneUser3, getLaneDB.LaneUser3)
            Assert.AreNotEqual(exp.LaneUser4, getLaneDB.LaneUser4)

            '******** Allow Update Flag TRUE *******

            'First set the AllowUpdate parameter for all fields to true
            setAllowUpdateFlags(oLaneDB, 1)

            'send the new lane data through the web services (70 version)
            Try
                intResult = oLane.ProcessData70(WSAuthCode, laneObj.LaneHeaders(), laneObj.LaneCals(), strLastError)
            Catch ex As Exception
                Assert.Fail("There was a problem with ProcessData70 in LaneObject.asmx: " & ex.Message)
            End Try

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

            'Try to get back the updated record from the database
            Try
                getLaneDB = GetLaneFilteredByLaneNumber(laneObj.LaneHeaders(1).LaneNumber)
            Catch ex As Exception
                'If we can't get it back something must have gone wrong with the insert
                Assert.Fail("There was a problem when attempting to read the record back from the database: " & ex.Message)
            End Try

            'check to see if the fields were changed
            Dim dec As New Decimal
            dec = 1.0
            Assert.AreEqual("Mooresville", getLaneDB.LaneOrigName)
            Assert.AreEqual("207 Talbert Point Dr.", getLaneDB.LaneOrigAddress1)
            Assert.AreEqual("Mooresville", getLaneDB.LaneOrigCity)
            Assert.AreEqual("NC", getLaneDB.LaneOrigState)
            Assert.AreEqual("28117", getLaneDB.LaneOrigZip)

            Assert.AreEqual("Axis", getLaneDB.LaneDestName)
            Assert.AreEqual("1099 Pratt Blvd.", getLaneDB.LaneDestAddress1)
            Assert.AreEqual("Elk Grove Village", getLaneDB.LaneDestCity)
            Assert.AreEqual("IL", getLaneDB.LaneDestState)
            Assert.AreEqual("60007", getLaneDB.LaneDestZip)
            Assert.AreEqual(dec, getLaneDB.LaneRequiredOnTimeServiceLevel)
            Assert.AreEqual(dec, getLaneDB.LaneCalcOnTimeServiceLevel)
            Assert.AreEqual(dec, getLaneDB.LaneCalcOnTimeNoMonthsUsed)
            Assert.AreEqual("UPDATE", getLaneDB.LaneUser1)
            Assert.AreEqual("UPDATE", getLaneDB.LaneUser2)
            Assert.AreEqual("UPDATE", getLaneDB.LaneUser3)
            Assert.AreEqual("UPDATE", getLaneDB.LaneUser4)

        Catch ex As Exception
            Throw ex
        Finally
            'Restore the data in tblImportFields back to the original values
            restoretblImportFields(oLaneDB)
            Try
                'remove the record we inserted from the database
                getLaneDB = GetLaneFilteredByLaneNumber(laneObj.LaneHeaders(1).LaneNumber)
                If getLaneDB IsNot Nothing Then
                    deleteLaneRecord(getLaneDB)
                End If
            Catch ex As Exception
                Assert.Fail(ex.Message.ToString())
            End Try

        End Try

    End Sub

    Private Function createNewLane() As LaneTestObject
        Dim retObj As New LaneTestObject

        Dim hdrs(2) As NGLLaneObjectWebService.clsLaneObject70
        Dim cals(2) As NGLLaneObjectWebService.clsLaneCalendarObject70

        Dim newLane As New NGLLaneObjectWebService.clsLaneObject70
        With newLane
            .LaneNumber = "27-LVVTEST"
            .LaneName = "LVV TEST LANE WSUT"
            .LaneNumberMaster = ""
            .LaneNameMaster = ""
            .LaneCompNumber = "1"
            .LaneDefaultCarrierUse = True
            .LaneDefaultCarrierNumber = 0
            .LaneOrigCompNumber = "1"
            .LaneOrigName = "Yakima"
            .LaneOrigAddress1 = "1009 Rock Avenue"
            .LaneOrigAddress2 = ""
            .LaneOrigAddress3 = ""
            .LaneOrigCity = "Yakima"
            .LaneOrigState = "WA"
            .LaneOrigCountry = "USA"
            .LaneOrigZip = "98902"
            .LaneOrigContactPhone = ""
            .LaneOrigContactPhoneExt = ""
            .LaneOrigContactFax = ""
            .LaneDestCompNumber = "0"
            .LaneDestName = "California Custom Fruit & Flavors"
            .LaneDestAddress1 = "15800 Tapia St"
            .LaneDestAddress2 = ""
            .LaneDestAddress3 = ""
            .LaneDestCity = "Irwindale"
            .LaneDestState = "CA"
            .LaneDestCountry = "USA"
            .LaneDestZip = "91706"
            .LaneDestContactPhone = ""
            .LaneDestContactPhoneExt = ""
            .LaneDestContactFax = ""
            .LaneConsigneeNumber = ""
            .LaneRecMinIn = 0
            .LaneRecMinUnload = 0
            .LaneRecMinOut = 0
            .LaneAppt = False
            .LanePalletExchange = False
            .LanePalletType = "Normal"
            .LaneBenchMiles = 0
            .LaneBFC = 115
            .LaneBFCType = "PERC"
            .LaneRecHourStart = ""
            .LaneRecHourStop = ""
            .LaneDestHourStart = ""
            .LaneDestHourStop = ""
            .LaneComments = "Lane Comments"
            .LaneCommentsConfidential = "Lane Comments Confidential"
            .LaneLatitude = 0
            .LaneLongitude = 0
            .LaneTempType = 3
            .LaneTransType = 4
            .LanePrimaryBuyer = ""
            .LaneAptDelivery = False
            .BrokerNumber = ""
            .BrokerName = ""
            .LaneOriginAddressUse = False
            .LaneCarrierEquipmentCodes = ""
            .LaneChepGLID = ""
            .LaneCarrierTypeCode = ""
            .LanePickUpMon = True
            .LanePickUpTue = True
            .LanePickUpWed = True
            .LanePickUpThu = True
            .LanePickUpFri = True
            .LanePickUpSat = True
            .LanePickUpSun = True
            .LaneDropOffMon = True
            .LaneDropOffTue = True
            .LaneDropOffWed = True
            .LaneDropOffThu = True
            .LaneDropOffFri = True
            .LaneDropOffSat = True
            .LaneDropOffSun = True
            .LaneDefaultRouteSequence = 0
            .LaneRouteGuideNumber = ""
            .LaneIsCrossDockFacility = False
            'New Fields 70
            .LaneLegalEntity = "TestLaneWS"
            .LaneCompAlphaCode = "LVLANE"
            .LaneRequiredOnTimeServiceLevel = 0
            .LaneCalcOnTimeServiceLevel = 0
            .LaneCalcOnTimeNoMonthsUsed = 0
            .LaneUser1 = "User 1"
            .LaneUser2 = "User 2"
            .LaneUser3 = "User 3"
            .LaneUser4 = "User 4"
            .LaneOrigLegalEntity = ""
            .LaneOrigCompAlphaCode = ""
            .LaneDestLegalEntity = ""
            .LaneDestCompAlphaCode = ""
        End With
        hdrs(1) = newLane

        Dim newCal As New NGLLaneObjectWebService.clsLaneCalendarObject70
        With newCal
            .LaneNumber = "27-LVVTEST"
            .Month = 7
            .Day = 2
            .Open = True
            .StartTime = ""
            .EndTime = ""
            .IsHoliday = False
            .ApplyToOrigin = True
            'New Fields 70
            .LaneLegalEntity = "TestLaneWS"
            .LaneCompAlphaCode = "LVLANE"
        End With
        cals(1) = newCal

        retObj.LaneHeaders = hdrs
        retObj.LaneCals = cals

        Return retObj

    End Function

    <TestMethod()>
    Public Sub LaneProcessObjectWebServiceTest()
        Dim sSource As String = "LaneProcessObjectWebServiceTest"
        Try
            Dim oLane As New NGLLaneObjectWebService.LaneObject
            oLane.Url = "http://nglwstest.nextgeneration.com/LaneObject.asmx"
            'LaneDestName = "Customer 01",
            'LaneDestAddress1 = "123 Any Street",
            'LaneDestCity = "Any Town",
            'LaneDestState = "IL",
            'LaneDestCountry = "US",
            'LaneDestZip = "60611",
            Dim oLaneHeader As New NGLLaneObjectWebService.clsLaneObject60() With { _
                .LaneName = "RobTest Lane", _
                .LaneNumber = "RR-RR-TEST", _
                .LaneCompNumber = "30", _
                .LaneOrigCompNumber = "30", _
                .LaneDestCompNumber = "282", _
                .LaneOriginAddressUse = False, _
                .LanePalletType = "N", _
                .LaneBFC = 100, _
                .LaneBFCType = "PERC", _
                .LaneComments = "comments", _
                .LaneCommentsConfidential = "confidential comments" _
            }

            'add the data to an array
            Dim oLaneHeaders As New List(Of NGLLaneObjectWebService.clsLaneObject60) '= New NGLLaneObjectWebService.clsLaneObject70(1)
            oLaneHeaders.Add(oLaneHeader)
            'create a calendar object or an empty array 
            'NGLLaneData.clsLaneCalendarObject70[] oLaneCalendar = new NGLLaneData.clsLaneCalendarObject70[1];

            Dim ReturnMessage As String = ""
            Dim WebAuthCode As String = "NGLWSTEST"
            Dim NGLRet As Integer = oLane.ProcessData60(WebAuthCode, oLaneHeaders.ToArray(), ReturnMessage)


            Select Case NGLRet
                Case CInt(WebServiceReturnValues.nglDataConnectionFailure)
                    Assert.Fail("Database Connection Failure Error: " & ReturnMessage)
                    Exit Select
                Case CInt(WebServiceReturnValues.nglDataIntegrationFailure)
                    Assert.Fail("Data Integration Failure Error: " & ReturnMessage)
                    Exit Select
                Case CInt(WebServiceReturnValues.nglDataIntegrationHadErrors)
                    Assert.Fail("Some Errors: " & ReturnMessage)
                    Exit Select
                Case CInt(WebServiceReturnValues.nglDataValidationFailure)
                    Assert.Fail("Data Validation Failure Error: " & ReturnMessage)
                    Exit Select

            End Select
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As ApplicationException
            Assert.Fail(sSource & " Failed: {0} ", ex.Message)
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & sSource & ": {0} ", ex.Message)
        Finally
            'place clean up code here

        End Try

    End Sub

End Class


Class LaneTestObject

    Private _LaneHeaders As NGLLaneObjectWebService.clsLaneObject70()
    Public Property LaneHeaders() As NGLLaneObjectWebService.clsLaneObject70()
        Get
            Return _LaneHeaders
        End Get
        Set(ByVal value As NGLLaneObjectWebService.clsLaneObject70())
            _LaneHeaders = value
        End Set
    End Property

    Private _LaneCals As NGLLaneObjectWebService.clsLaneCalendarObject70()
    Public Property LaneCals() As NGLLaneObjectWebService.clsLaneCalendarObject70()
        Get
            Return _LaneCals
        End Get
        Set(ByVal value As NGLLaneObjectWebService.clsLaneCalendarObject70())
            _LaneCals = value
        End Set
    End Property





End Class
