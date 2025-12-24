Imports System
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports DAL = Ngl.FreightMaster.Data
Imports Ngl.FreightMaster.Integration

<TestClass()> Public Class BookObjectWSTest
    Inherits TestBase

    <TestCategory("Nightly")>
    <TestMethod()>
    Public Sub BookProcessData70Test()
        testParameters.DBServer = "NGLRDP06D"
        testParameters.Database = "SHIELDSMASUnitTest"

        'Always overwrite the web.config when running unit test
        FileCopy("Web.config", "D:\HTTP\WSUnitTest70\Web.config")

        Dim intResult As Integer = 0
        Dim strLastError As String = ""

        Dim oBook As New NGLBookObjectWebService.BookObject
        oBook.Url = WSUrl & "BookObject.ASMX"

        Dim oBookDB As New DAL.NGLBookData(testParameters)

        'Test creating a new Lane via web services
        Dim bookObj As New BookTestObject
        bookObj = createNewBook()
        Dim getPOHdrDB As New Ngl.FreightMaster.Data.DataTransferObjects.POHdr

        '******************** TEST INSERT NEW RECORD **********************
        Try
            'send the new POHdr data through the web services (70 version)
            Try
                intResult = oBook.ProcessData70(WSAuthCode, bookObj.BookHeaders(), bookObj.BookDetails(), strLastError)
            Catch ex As Exception
                Assert.Fail("There was a problem with ProcessData70 in BookObject.asmx: " & ex.Message)
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
                getPOHdrDB = GetPOHdrFiltered(bookObj.BookHeaders(1).PONumber)
            Catch ex As Exception
                'If we can't get it back something must have gone wrong with the insert
                Assert.Fail("There was a problem when attempting to read the record back from the database: " & ex.Message)
            End Try

            'Check key fields and fields new to 70; check the original object vs the one read from the db after insert
            Dim exp = bookObj.BookHeaders(1)
            Assert.AreEqual(exp.PONumber, getPOHdrDB.POHDRnumber)
            Assert.AreEqual(exp.POVendor, getPOHdrDB.POHDRvendor)
            Assert.AreEqual(exp.POCompLegalEntity, getPOHdrDB.POHDRCompLegalEntity)
            Assert.AreEqual(exp.POCompAlphaCode, getPOHdrDB.POHDRCompAlphaCode)
            Assert.AreEqual(exp.POModeTypeControl, getPOHdrDB.POHDRModeTypeControl)
            Assert.AreEqual(exp.POMustLeaveByDateTime, getPOHdrDB.POHDRMustLeaveByDateTime)
            Assert.AreEqual(exp.POUser1, getPOHdrDB.POHDRUser1)
            Assert.AreEqual(exp.POUser2, getPOHdrDB.POHDRUser2)
            Assert.AreEqual(exp.POUser3, getPOHdrDB.POHDRUser3)
            Assert.AreEqual(exp.POUser4, getPOHdrDB.POHDRUser4)
            'Assert.AreEqual(exp.POAPGLNumber, getPOHdrDB.POHDRAPGLNumber) 'APGLNumber is not a member of DTO.POHdr
            Assert.AreEqual(exp.POdate, getPOHdrDB.POHDRPOdate)
            Assert.AreEqual(exp.POShipdate, getPOHdrDB.POHDRShipdate)
            Assert.AreEqual(exp.POFrt, getPOHdrDB.POHDRFrt)
            Assert.AreEqual(exp.POWgt, getPOHdrDB.POHDRWgt)
            Assert.AreEqual(exp.POShipInstructions, getPOHdrDB.POHDRShipInstructions)
            Assert.AreEqual(exp.POComments, getPOHdrDB.POHDRComments)
            Assert.AreEqual(exp.POCommentsConfidential, getPOHdrDB.POHDRCommentsConfidential)

            '********************* TEST UPDATE RECORD ***********************

            '******* Allow Update Flag FALSE *******

            'First set the AllowUpdate parameter for all fields to false
            setAllowUpdateFlags(oBookDB, 0)

            'Now try updating the record by changing some fields
            exp.PONumber = "LVVTESTUpdate"
            exp.POVendor = "1-S14540"
            exp.POCompLegalEntity = "NEWLE"
            exp.POCompAlphaCode = "NEWAC"
            exp.POModeTypeControl = 2
            exp.POMustLeaveByDateTime = "2015-05-14 08:02:00.000"
            exp.POUser1 = "UPDATE"
            exp.POUser2 = "UPDATE"
            exp.POUser3 = "UPDATE"
            exp.POUser4 = "UPDATE"
            exp.POdate = "2015-05-09 08:05:31.000"
            exp.POShipdate = "2015-05-09 08:05:31.000"
            exp.POFrt = 0
            exp.POWgt = 1270
            exp.POShipInstructions = "UPDATE"
            exp.POComments = "UPDATE"
            exp.POCommentsConfidential = "UPDATE"

            'send the new POHdr data through the web services (70 version)
            Try
                intResult = oBook.ProcessData70(WSAuthCode, bookObj.BookHeaders(), bookObj.BookDetails(), strLastError)
            Catch ex As Exception
                Assert.Fail("There was a problem with ProcessData70 in BookObject.asmx: " & ex.Message)
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
                getPOHdrDB = GetPOHdrFiltered(bookObj.BookHeaders(1).PONumber)
            Catch ex As Exception
                'If we can't get it back something must have gone wrong with the insert
                Assert.Fail("There was a problem when attempting to read the record back from the database: " & ex.Message)
            End Try

            'check to see if the field was changed -- it should still be the same 
            Assert.AreNotEqual(exp.PONumber, getPOHdrDB.POHDRnumber)
            Assert.AreNotEqual(exp.POVendor, getPOHdrDB.POHDRvendor)
            Assert.AreNotEqual(exp.POCompLegalEntity, getPOHdrDB.POHDRCompLegalEntity)
            Assert.AreNotEqual(exp.POCompAlphaCode, getPOHdrDB.POHDRCompAlphaCode)
            Assert.AreNotEqual(exp.POModeTypeControl, getPOHdrDB.POHDRModeTypeControl)
            Assert.AreNotEqual(exp.POMustLeaveByDateTime, getPOHdrDB.POHDRMustLeaveByDateTime)
            Assert.AreNotEqual(exp.POUser1, getPOHdrDB.POHDRUser1)
            Assert.AreNotEqual(exp.POUser2, getPOHdrDB.POHDRUser2)
            Assert.AreNotEqual(exp.POUser3, getPOHdrDB.POHDRUser3)
            Assert.AreNotEqual(exp.POUser4, getPOHdrDB.POHDRUser4)
            'Assert.AreNotEqual(exp.POAPGLNumber, getPOHdrDB.POHDRAPGLNumber) 'APGLNumber is not a member of DTO.POHdr
            Assert.AreNotEqual(exp.POdate, getPOHdrDB.POHDRPOdate)
            Assert.AreNotEqual(exp.POShipdate, getPOHdrDB.POHDRShipdate)
            Assert.AreNotEqual(exp.POFrt, getPOHdrDB.POHDRFrt)
            Assert.AreNotEqual(exp.POWgt, getPOHdrDB.POHDRWgt)
            Assert.AreNotEqual(exp.POShipInstructions, getPOHdrDB.POHDRShipInstructions)
            Assert.AreNotEqual(exp.POComments, getPOHdrDB.POHDRComments)
            Assert.AreNotEqual(exp.POCommentsConfidential, getPOHdrDB.POHDRCommentsConfidential)

            '******** Allow Update Flag TRUE *******

            'First set the AllowUpdate parameter for all fields to true
            setAllowUpdateFlags(oBookDB, 1)

            'send the new POHdr data through the web services (70 version)
            Try
                intResult = oBook.ProcessData70(WSAuthCode, bookObj.BookHeaders(), bookObj.BookDetails(), strLastError)
            Catch ex As Exception
                Assert.Fail("There was a problem with ProcessData70 in BookObject.asmx: " & ex.Message)
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
                getPOHdrDB = GetPOHdrFiltered(bookObj.BookHeaders(1).PONumber)
            Catch ex As Exception
                'If we can't get it back something must have gone wrong with the insert
                Assert.Fail("There was a problem when attempting to read the record back from the database: " & ex.Message)
            End Try

            'check to see if the fields were changed
            Assert.AreEqual("LVVTESTUpdate", getPOHdrDB.POHDRnumber)
            Assert.AreEqual("1-S14540", getPOHdrDB.POHDRvendor)
            Assert.AreEqual("NEWLE", getPOHdrDB.POHDRCompLegalEntity)
            Assert.AreEqual("NEWAC", getPOHdrDB.POHDRCompAlphaCode)
            Assert.AreEqual(2, getPOHdrDB.POHDRModeTypeControl)
            Assert.AreEqual("2015-05-14 08:02:00.000", getPOHdrDB.POHDRMustLeaveByDateTime)
            Assert.AreEqual("UPDATE", getPOHdrDB.POHDRUser1)
            Assert.AreEqual("UPDATE", getPOHdrDB.POHDRUser2)
            Assert.AreEqual("UPDATE", getPOHdrDB.POHDRUser3)
            Assert.AreEqual("UPDATE", getPOHdrDB.POHDRUser4)
            Assert.AreEqual("2015-05-09 08:05:31.000", getPOHdrDB.POHDRPOdate)
            Assert.AreEqual("2015-05-09 08:05:31.000", getPOHdrDB.POHDRShipdate)
            Assert.AreEqual(0, getPOHdrDB.POHDRFrt)
            Assert.AreEqual(1270, getPOHdrDB.POHDRWgt)
            Assert.AreEqual("UPDATE", getPOHdrDB.POHDRShipInstructions)
            Assert.AreEqual("UPDATE", getPOHdrDB.POHDRComments)
            Assert.AreEqual("UPDATE", getPOHdrDB.POHDRCommentsConfidential)

        Catch ex As Exception
            Throw ex
        Finally
            'Restore the data in tblImportFields back to the original values
            restoretblImportFields(oBookDB)
            Try
                'remove the record we inserted from the database
                getPOHdrDB = GetPOHdrFiltered(bookObj.BookHeaders(1).PONumber)
                If getPOHdrDB IsNot Nothing Then
                    deletePOHdrRecord(getPOHdrDB)
                End If
            Catch ex As Exception
                Assert.Fail(ex.Message.ToString())
            End Try

        End Try


    End Sub

    Private Function createNewBook() As BookTestObject
        Dim retObj As New BookTestObject

        Dim hdrs(2) As NGLBookObjectWebService.clsBookHeaderObject70
        Dim dets(2) As NGLBookObjectWebService.clsBookDetailObject70

        Dim newPOHdr As New NGLBookObjectWebService.clsBookHeaderObject70
        With newPOHdr
            .PONumber = "LVVTESTPO"
            .POVendor = "1-S74600"
            .POdate = "2015-05-08 08:05:31.000"
            .POShipdate = "2015-05-08 08:05:31.000"
            .POBuyer = ""
            .POFrt = 4
            .POTotalFrt = 0
            .POTotalCost = 0
            .POWgt = 1025
            .POCube = 0
            .POQty = 1
            .POLines = 0
            .POConfirm = False
            .PODefaultCustomer = "1"
            .PODefaultCarrier = 0
            .POReqDate = "2015-05-14 00:00:00.000"
            .POShipInstructions = "Ship Instructions"
            .POCooler = False
            .POFrozen = False
            .PODry = True
            .POTemp = "D"
            .POCarType = ""
            .POShipVia = ""
            .POShipViaType = ""
            .POConsigneeNumber = ""
            .POCustomerPO = ""
            .POOtherCosts = 0
            .POStatusFlag = 0
            .POOrderSequence = 0
            .POChepGLID = ""
            .POCarrierEquipmentCodes = ""
            .POCarrierTypeCode = ""
            .POPalletPositions = ""
            .POSchedulePUDate = ""
            .POSchedulePUTime = ""
            .POScheduleDelDate = ""
            .POSCheduleDelTime = ""
            .POActPUDate = ""
            .POActPUTime = ""
            .POActDelDate = ""
            .POActDelTime = ""
            .POOrigCompNumber = ""
            .POOrigName = ""
            .POOrigAddress1 = ""
            .POOrigAddress2 = ""
            .POOrigAddress3 = ""
            .POOrigCity = ""
            .POOrigState = ""
            .POOrigZip = ""
            .POOrigCountry = ""
            .POOrigContactPhone = ""
            .POOrigContactPhoneExt = ""
            .POOrigContactFax = ""
            .PODestCompNumber = ""
            .PODestName = ""
            .PODestAddress1 = ""
            .PODestAddress2 = ""
            .PODestAddress3 = ""
            .PODestCity = ""
            .PODestState = ""
            .PODestZip = ""
            .PODestCountry = ""
            .PODestContactPhone = ""
            .PODestContactPhoneExt = ""
            .PODestContactFax = ""
            .POPalletExchange = False
            .POPalletType = ""
            .POComments = "Comments"
            .POCommentsConfidential = "Comments Confidential"
            .POInbound = False
            .PODefaultRouteSequence = 0
            .PORouteGuideNumber = ""
            'New Fields 70
            .POCompLegalEntity = "LVVLE"
            .POCompAlphaCode = "LVVAC"
            .POModeTypeControl = 3
            .POMustLeaveByDateTime = "2015-05-13 08:02:00.000"
            .POUser1 = "User 1"
            .POUser2 = "User 2"
            .POUser3 = "User 3"
            .POUser4 = "User 4"
            .POAPGLNumber = "APGLNumber"
        End With
        hdrs(1) = newPOHdr

        Dim newPOItem As New NGLBookObjectWebService.clsBookDetailObject70
        With newPOItem
            .ItemPONumber = "LVVTESTPO"
            .FixOffInvAllow = 0
            .FixFrtAllow = 0
            .ItemNumber = "12345"
            .QtyOrdered = 90
            .FreightCost =
            .ItemCost = 11232.0
            .Weight = 2610
            .Cube = 101
            .Pack = 25
            .Size = "Packages per case"
            .Description = "HL 25 x 454 g Wild Pac PinkSal"
            .Hazmat = ""
            .Brand = "10"
            .CostCenter = "445"
            .LotNumber = ""
            .LotExpirationDate = ""
            .GTIN = "10622062075467"
            .CustItemNumber = "536515"
            .CustomerNumber = "1"
            .POOrderSequence = 0
            .PalletType = ""
            .POItemHazmatTypeCode = ""
            .POItem49CFRCode = ""
            .POItemIATACode = ""
            .POItemDOTCode = ""
            .POItemMarineCode = ""
            .POItemNMFCClass = ""
            .POItemFAKClass = ""
            .POItemLimitedQtyFlag = False
            .POItemPallets = 0
            .POItemTies = 0
            .POItemHighs = 0
            .POItemQtyPalletPercentage = 0
            .POItemQtyLength = 0
            .POItemQtyWidth = 0
            .POItemQtyHeight = 0
            .POItemStackable = True
            .POItemLevelOfDensity = 0
            ' New Fields 70
            .POItemCompLegalEntity = "LVVLE"
            .POItemCompAlphaCode = "LVVAC"
            .POItemNMFCSubClass = "NMFCSubClass"
            .POItemUser1 = "User 1"
            .POItemUser2 = "User 2"
            .POItemUser3 = "User 3"
            .POItemUser4 = "User 4"
        End With
        dets(1) = newPOItem

        retObj.BookHeaders = hdrs
        retObj.BookDetails = dets

        Return retObj

    End Function

    Private Function createNewIngomarTestOrder() As BookTestObject80

        Dim retObj As New BookTestObject80

        Dim hdrs(2) As DTMSERPIntegration.clsBookHeaderObject80
        Dim dets(2) As DTMSERPIntegration.clsBookDetailObject80

        Dim newPOHdr As New DTMSERPIntegration.clsBookHeaderObject80
        With newPOHdr
            .POWhseAuthorizationNo = " "
            .POOrigContactEmail = "IPCCustomerServiceDest@ingomar.com"
            .PODestContactEmail = "IPCCustomerServiceDest@ingomar.com"
            .POWhseReleaseNo = ""
            .POOrigLegalEntity = Nothing
            .POOrigCompAlphaCode = Nothing
            .PODestLegalEntity = Nothing
            .PODestCompAlphaCode = Nothing
            .ChangeNo = Nothing
            .POCompLegalEntity = "IPC"
            .POCompAlphaCode = "Ingomar"
            .POModeTypeControl = 2
            .POMustLeaveByDateTime = Nothing
            .POUser1 = ""
            .POUser2 = ""
            .POUser3 = ""
            .POUser4 = ""
            .POAPGLNumber = "64200-50-1"
            .PORecMinIn = 0
            .PORecMinUnload = 0
            .PORecMinOut = 0
            .POAppt = False
            .POBFC = 100
            .POBFCType = "PERC"
            .POCarrBLNumber = Nothing
            .PONumber = "211477"
            .POVendor = "IPC-936-19-0"
            .POdate = "1/12/2022 75849 AM"
            .POShipdate = "1/4/2022 12:00:00 AM"
            .POBuyer = Nothing
            .POFrt = 4
            .POTotalFrt = 0
            .POTotalCost = 0
            .POWgt = 0
            .POCube = 0
            .POQty = 0
            .POPallets = 0
            .POLines = 0
            .POConfirm = False
            .PODefaultCustomer = "1"
            .PODefaultCarrier = 0
            .POReqDate = "1/4/2022 12:00:00 AM"
            .POShipInstructions = " " ' "EMAIL:agrifoods@jcipelli.com; , E - MAIL: EFI \\ u0026 PETER.STRATTON@ECS-SHIPPING.COM, CUT OFF:   02/11"
            .POCooler = False
            .POFrozen = False
            .PODry = False
            .POTemp = "D"
            .POCarType = Nothing
            .POShipVia = Nothing
            .POShipViaType = Nothing
            .POConsigneeNumber = Nothing
            .POCustomerPO = "4736(5)"
            .POOtherCosts = 0
            .POStatusFlag = 5
            .POOrderSequence = 0
            .POChepGLID = Nothing
            .POCarrierEquipmentCodes = Nothing
            .POCarrierTypeCode = Nothing
            .POPalletPositions = Nothing
            .POSchedulePUDate = Nothing
            .POSchedulePUTime = Nothing
            .POScheduleDelDate = Nothing
            .POSCheduleDelTime = Nothing
            .POActPUDate = Nothing
            .POActPUTime = Nothing
            .POActDelDate = Nothing
            .POActDelTime = Nothing
            .POOrigCompNumber = "1"
            .POOrigName = Nothing
            .POOrigAddress1 = Nothing
            .POOrigAddress2 = Nothing
            .POOrigAddress3 = Nothing
            .POOrigCity = Nothing
            .POOrigState = Nothing
            .POOrigCountry = Nothing
            .POOrigZip = Nothing
            .POOrigContactPhone = Nothing
            .POOrigContactPhoneExt = Nothing
            .POOrigContactFax = Nothing
            .PODestCompNumber = Nothing
            .PODestName = "NATIONAL CANNERS LTD"
            .PODestAddress1 = ""
            .PODestAddress2 = "PALLET CHARGES"
            .PODestAddress3 = Nothing
            .PODestCity = ""
            .PODestState = ""
            .PODestCountry = "US"
            .PODestZip = ""
            .PODestContactPhone = Nothing
            .PODestContactPhoneExt = Nothing
            .PODestContactFax = Nothing
            .POInbound = False
            .POPalletExchange = False
            .POPalletType = Nothing
            .POComments = Nothing
            .POCommentsConfidential = Nothing
            .PODefaultRouteSequence = 0
            .PORouteGuideNumber = Nothing
        End With

        hdrs(1) = newPOHdr

        Dim newPOItem As New DTMSERPIntegration.clsBookDetailObject80()
        With newPOItem
            .ItemPONumber = "LVVTESTPO"
            .FixOffInvAllow = 0
            .FixFrtAllow = 0
            .ItemNumber = "12345"
            .QtyOrdered = 90
            .FreightCost =
            .ItemCost = 11232.0
            .Weight = 2610
            .Cube = 101
            .Pack = 25
            .Size = "Packages per case"
            .Description = "HL 25 x 454 g Wild Pac PinkSal"
            .Hazmat = ""
            .Brand = "10"
            .CostCenter = "445"
            .LotNumber = ""
            .LotExpirationDate = ""
            .GTIN = "10622062075467"
            .CustItemNumber = "536515"
            .CustomerNumber = "1"
            .POOrderSequence = 0
            .PalletType = ""
            .POItemHazmatTypeCode = ""
            .POItem49CFRCode = ""
            .POItemIATACode = ""
            .POItemDOTCode = ""
            .POItemMarineCode = ""
            .POItemNMFCClass = ""
            .POItemFAKClass = ""
            .POItemLimitedQtyFlag = False
            .POItemPallets = 0
            .POItemTies = 0
            .POItemHighs = 0
            .POItemQtyPalletPercentage = 0
            .POItemQtyLength = 0
            .POItemQtyWidth = 0
            .POItemQtyHeight = 0
            .POItemStackable = True
            .POItemLevelOfDensity = 0
            ' New Fields 70
            .POItemCompLegalEntity = "LVVLE"
            .POItemCompAlphaCode = "LVVAC"
            .POItemNMFCSubClass = "NMFCSubClass"
            .POItemUser1 = "User 1"
            .POItemUser2 = "User 2"
            .POItemUser3 = "User 3"
            .POItemUser4 = "User 4"
        End With
        dets(1) = newPOItem

        retObj.BookHeaders = hdrs
        retObj.BookDetails = dets

        Return retObj


    End Function

    Private Function createNewBook80() As BookTestObject80
        Dim retObj As New BookTestObject80

        Dim hdrs(2) As DTMSERPIntegration.clsBookHeaderObject80
        Dim dets(2) As DTMSERPIntegration.clsBookDetailObject80

        Dim newPOHdr As New DTMSERPIntegration.clsBookHeaderObject80
        With newPOHdr
            .PONumber = "LVVTESTPO"
            .POVendor = "1-S74600"
            .POdate = "2022-05-08 08:05:31.000"
            .POShipdate = "2022-05-08 08:05:31.000"
            .POBuyer = ""
            .POFrt = 4
            .POTotalFrt = 0
            .POTotalCost = 0
            .POWgt = 1025
            .POCube = 0
            .POQty = 1
            .POLines = 0
            .POConfirm = False
            .PODefaultCustomer = "1"
            .PODefaultCarrier = 0
            .POReqDate = "2022-05-14 00:00:00.000"
            .POShipInstructions = "Ship Instructions"
            .POCooler = False
            .POFrozen = False
            .PODry = True
            .POTemp = "D"
            .POCarType = ""
            .POShipVia = ""
            .POShipViaType = ""
            .POConsigneeNumber = ""
            .POCustomerPO = ""
            .POOtherCosts = 0
            .POStatusFlag = 0
            .POOrderSequence = 0
            .POChepGLID = ""
            .POCarrierEquipmentCodes = ""
            .POCarrierTypeCode = ""
            .POPalletPositions = ""
            .POSchedulePUDate = ""
            .POSchedulePUTime = ""
            .POScheduleDelDate = ""
            .POSCheduleDelTime = ""
            .POActPUDate = ""
            .POActPUTime = ""
            .POActDelDate = ""
            .POActDelTime = ""
            .POOrigCompNumber = 1
            .POOrigName = "Ingomar"
            .POOrigAddress1 = ""
            .POOrigAddress2 = " "
            .POOrigCity = ""
            .POOrigState = ""
            .POOrigCountry = ""
            .POOrigZip = ""
            .POOrigContactPhone = ""
            .POOrigContactPhoneExt = ""
            .POOrigContactFax = ""
            .PODestName = "TROPICANA DOLE BEVERAGE, N.A."
            .PODestName = "NATURAL COUNTRY FARMS"
            .POOrigAddress1 = "58 WEST ROAD"
            .POOrigAddress2 = " "
            .POOrigCity = "ELLINGTON"
            .POOrigState = "CT"
            .POOrigCountry = "US"
            .POOrigZip = "06029"
            .PODestCountry = ""
            .PODestContactPhone = ""
            .PODestContactPhoneExt = ""
            .PODestContactFax = ""
            .POPalletExchange = False
            .POPalletType = ""
            .POComments = "Comments"
            .POCommentsConfidential = "Comments Confidential"
            .POInbound = False
            .PODefaultRouteSequence = 0
            .PORouteGuideNumber = ""
            'New Fields 70
            .POCompLegalEntity = "IPC"
            .POCompAlphaCode = "Ingomar"
            .POModeTypeControl = 3
            .POMustLeaveByDateTime = "2022-05-13 08:02:00.000"
            .POUser1 = "User 1"
            .POUser2 = "User 2"
            .POUser3 = "User 3"
            .POUser4 = "User 4"
            .POAPGLNumber = "APGLNumber"
        End With
        hdrs(1) = newPOHdr

        Dim newPOItem As New DTMSERPIntegration.clsBookDetailObject80
        With newPOItem
            .ItemPONumber = "LVVTESTPO"
            .FixOffInvAllow = 0
            .FixFrtAllow = 0
            .ItemNumber = "12345"
            .QtyOrdered = 90
            .FreightCost =
            .ItemCost = 11232.0
            .Weight = 2610
            .Cube = 101
            .Pack = 25
            .Size = "Packages per case"
            .Description = "HL 25 x 454 g Wild Pac PinkSal"
            .Hazmat = ""
            .Brand = "10"
            .CostCenter = "445"
            .LotNumber = ""
            .LotExpirationDate = ""
            .GTIN = "10622062075467"
            .CustItemNumber = "536515"
            .CustomerNumber = "1"
            .POOrderSequence = 0
            .PalletType = ""
            .POItemHazmatTypeCode = ""
            .POItem49CFRCode = ""
            .POItemIATACode = ""
            .POItemDOTCode = ""
            .POItemMarineCode = ""
            .POItemNMFCClass = ""
            .POItemFAKClass = ""
            .POItemLimitedQtyFlag = False
            .POItemPallets = 0
            .POItemTies = 0
            .POItemHighs = 0
            .POItemQtyPalletPercentage = 0
            .POItemQtyLength = 0
            .POItemQtyWidth = 0
            .POItemQtyHeight = 0
            .POItemStackable = True
            .POItemLevelOfDensity = 0
            ' New Fields 70
            .POItemCompLegalEntity = "LVVLE"
            .POItemCompAlphaCode = "LVVAC"
            .POItemNMFCSubClass = "NMFCSubClass"
            .POItemUser1 = "User 1"
            .POItemUser2 = "User 2"
            .POItemUser3 = "User 3"
            .POItemUser4 = "User 4"
        End With
        dets(1) = newPOItem

        retObj.BookHeaders = hdrs
        retObj.BookDetails = dets

        Return retObj

    End Function

    <TestMethod()>
    Public Sub BookProcessData70JuiceTest()
        Dim sSource As String = "BookProcessData70JuiceTest"
        testParameters.DBServer = "NGLRDP06D\DTMS365"
        testParameters.Database = "NGLMASJuice"

        Dim intResult As Integer = 0
        Dim strLastError As String = ""

        Dim oBook As New NGLBookObjectWebService.BookObject
        oBook.Url = "http://nglwsdev70.nextgeneration.com/BookObject.ASMX"
        oBook.Timeout = -1

        'Test creating a new Lane via web services
        Dim bookObj As New BookTestObject
        'bookObj = createJuiceBookRecord(6, False, False)
        bookObj = getJuiceBookTestData()

        Try
            'send the new POHdr data through the web services (70 version)
            Try
                intResult = oBook.ProcessData70("WSTEST", bookObj.BookHeaders(), bookObj.BookDetails(), strLastError)
            Catch ex As Exception
                Assert.Fail("There was a problem with ProcessData70 in BookObject.asmx: " & ex.Message)
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

        Catch ex As ApplicationException
            Assert.Fail("Application Exception For {0}: {1} ", sSource, ex.Message)
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For {0}: {1} ", sSource, ex.Message)
        Finally

        End Try


    End Sub

    <TestMethod()>
    Public Sub BookProcessData80IngomarTest()
        Dim sSource As String = "BookProcessData80IngomarTest"
        testParameters.DBServer = "ip-sqlsvr"
        testParameters.Database = "NGLMASPROD"

        Dim intResult As Integer = 0
        Dim strLastError As String = ""

        Dim oBook As New DTMSERPIntegration.DTMSERPIntegration()  ' NGLBookObjectWebService.BookObject
        oBook.Url = "http://d365prod.ngl-server.ingomar.local/ws/DTMSERPIntegration.asmx"
        'oBook.Url = "http://localhost:44320/WS/DTMSERPIntegration.asmx"
        oBook.Timeout = -1

        'Test creating a new Lane via web services
        Dim bookObj As New BookTestObject80
        'bookObj = createJuiceBookRecord(6, False, False)
        'bookObj = createNewBook80()
        bookObj = createNewIngomarTestOrder()

        Try
            'send the new POHdr data through the web services (70 version)
            Try
                intResult = oBook.ProcessBookData80("WSPROD", bookObj.BookHeaders(), bookObj.BookDetails(), strLastError)
            Catch ex As Exception
                Assert.Fail("There was a problem with ProcessData70 in BookObject.asmx: " & ex.Message)
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

        Catch ex As ApplicationException
            Assert.Fail("Application Exception For {0}: {1} ", sSource, ex.Message)
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For {0}: {1} ", sSource, ex.Message)
        Finally

        End Try


    End Sub

    Private Function getJuiceBookTestData() As BookTestObject
        Dim retObj As New BookTestObject

        Dim hdrs As New List(Of NGLBookObjectWebService.clsBookHeaderObject70)
        Dim dets As New List(Of NGLBookObjectWebService.clsBookDetailObject70)
        Dim oHeader As New NGLBookObjectWebService.clsBookHeaderObject70
        With oHeader
            .PONumber = "569502"
            .POVendor = "201-1TROP-01"
            .POdate = "04/01/2019 00:00:00"
            .POShipdate = "03/30/2019 00:00:00"
            .POFrt = "4"
            .POTotalFrt = "0"
            .POTotalCost = "385.37"
            .POWgt = "0"
            .POCube = "0"
            .POQty = "433"
            .POPallets = "0"
            .POLines = "1"
            .POConfirm = "false"
            .PODefaultCustomer = "0"
            .PODefaultCarrier = "0"
            .POReqDate = "03/30/2019 00:00:00"
            '.POShipInstructions/ = "
            .POCooler = "true"
            .POFrozen = "false"
            .PODry = "false"
            .POTemp = "C"
            '.POCarType/ = "
            .POShipVia = "53"
            .POShipViaType = "ShipViaCode"
            '.POConsigneeNumber/ = "
            .POCustomerPO = "4520649624"
            .POOtherCosts = "0"
            .POStatusFlag = "5"
            .POOrderSequence = "0"
            '.POChepGLID/ = "
            .POOrigCompNumber = 1
            .POOrigName = "NATURAL COUNTRY FARMS"
            .POOrigAddress1 = "58 WEST ROAD"
            .POOrigAddress2 = " "
            .POOrigCity = "ELLINGTON"
            .POOrigState = "CT"
            .POOrigCountry = "US"
            .POOrigZip = "06029"
            .PODestName = "TROPICANA DOLE BEVERAGE, N.A."
            .PODestAddress1 = " "
            .PODestAddress2 = " "
            .PODestAddress3 = " "
            .PODestCity = " "
            .PODestState = "CT"
            .PODestCountry = "US"
            .PODestZip = " "
            .POInbound = "false"
            .POPalletExchange = "false"
            .PODefaultRouteSequence = "0"
            .POCompAlphaCode = "201"
            .POModeTypeControl = "0"
        End With
        hdrs.Add(oHeader)
        Dim oDetail As New NGLBookObjectWebService.clsBookDetailObject70()
        With oDetail
            .ItemPONumber = "569502"
            .FixOffInvAllow = "0"
            .FixFrtAllow = "0"
            .ItemNumber = "BXPALLETPADS"
            .QtyOrdered = "433"
            .FreightCost = "0"
            .ItemCost = "0.89"
            .Weight = "0"
            .Cube = "0"
            .Pack = "0"
            .Description = "6-64  SLIP SHEETS"
            .CustomerNumber = "201"
            .POOrderSequence = "0"
            .PalletType = "N"
            .POItemLimitedQtyFlag = "false"
            .POItemPallets = "0"
            .POItemTies = "0"
            .POItemHighs = "0"
            .POItemQtyPalletPercentage = "0"
            .POItemQtyLength = "0"
            .POItemQtyWidth = "0"
            .POItemQtyHeight = "0"
            .POItemStackable = "false"
            .POItemLevelOfDensity = "0"
            .POItemCompAlphaCode = "201"
        End With
        dets.Add(oDetail)
        retObj.BookHeaders = hdrs.ToArray()
        retObj.BookDetails = dets.ToArray()

        Return retObj
    End Function

    Private Function createJuiceBookRecord(Optional ByVal QtyFactor As Integer = 3, Optional ByVal blnUseAltDest As Boolean = False, Optional ByVal blnUseAltOrig As Boolean = False) As BookTestObject
        Dim retObj As New BookTestObject

        Dim hdrs As New List(Of NGLBookObjectWebService.clsBookHeaderObject70)
        Dim dets As New List(Of NGLBookObjectWebService.clsBookDetailObject70)

        Dim newPOHdr As New NGLBookObjectWebService.clsBookHeaderObject70
        Dim strCSVHeader As String = "TEST ORDER,101-WELCHF-CPU,2016-03-16 00:00:00.000,2016-03-18 00:00:00.000,,3,Data Integration DLL,2016-03-16 14:58:51.000,NULL,0,0,1000.5,0,29,2,0,101,Ardmore Farms,998,2016-03-18 00:00:00.000,489355,TEST ORDER,1,0,0,C,ADF772824,ARDMORE FARMS,DELAND,FL,32720,RICKS COMPUTER SALES,AKRON,OH,44314,,P,ShipViaCod,1,0,4,0,ADF772824,0,0,,,,,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,-8999999999999962231,0x00000000005EA59E,0,1915 N. WOODLAND BLVD.,,,US,,,,,ATTN RICK HOUP,,,US,,,,0,,,,0,0,,,101,0,NULL,,,,,"
        Dim sArray As String() = strCSVHeader.Split(",")
        With newPOHdr
            .POCustomerPO = convertNULLToDefault(sArray(0))
            .POVendor = convertNULLToDefault(sArray(1))
            .POdate = convertNULLToDefault(sArray(2))
            .POShipdate = convertNULLToDefault(sArray(3))
            .POBuyer = convertNULLToDefault(sArray(4))
            .POFrt = convertNULLToDefault(sArray(5), True)
            '.POCreateUser = sArray(6)
            '.POCreateDate = sArray(7)
            '.POModUser = sArray(8)
            .POTotalFrt = convertNULLToDefault(sArray(9), True)
            .POTotalCost = convertNULLToDefault(sArray(10), True)
            .POWgt = convertNULLToDefault(sArray(11), True)
            .POCube = convertNULLToDefault(sArray(12), True)
            .POQty = convertNULLToDefault(sArray(13), True) + (QtyFactor * 2)
            .POLines = convertNULLToDefault(sArray(14), True)
            .POConfirm = convertNULLToDefault(sArray(15), False, True)
            .PODefaultCustomer = convertNULLToDefault(sArray(16))
            '.PODefaultCustomerName = sArray(17)
            .PODefaultCarrier = convertNULLToDefault(sArray(18), True)
            .POReqDate = convertNULLToDefault(sArray(19))
            .PONumber = convertNULLToDefault(sArray(20))
            .POShipInstructions = convertNULLToDefault(sArray(21))
            .POCooler = convertNULLToDefault(sArray(22), False, True)
            .POFrozen = convertNULLToDefault(sArray(23), False, True)
            .PODry = convertNULLToDefault(sArray(24), False, True)
            .POTemp = convertNULLToDefault(sArray(25))
            '.POModVerify = sArray(26)
            .POOrigName = convertNULLToDefault(sArray(27))
            .POOrigCity = convertNULLToDefault(sArray(28))
            .POOrigState = convertNULLToDefault(sArray(29))
            .POOrigZip = convertNULLToDefault(sArray(30))

            If blnUseAltOrig Then
                .POOrigName = ""
                .POOrigCity = "London"
                .POOrigState = "LN"
                .POOrigZip = "LN456"
            End If

            .PODestName = convertNULLToDefault(sArray(31))
            .PODestCity = convertNULLToDefault(sArray(32))
            .PODestState = convertNULLToDefault(sArray(33))
            .PODestZip = convertNULLToDefault(sArray(34))

            If blnUseAltDest Then
                .PODestName = "" 'convertNULLToDefault(sArray(31))
                .PODestCity = "Oldam" 'convertNULLToDefault(sArray(32))
                .PODestState = "MA" 'convertNULLToDefault(sArray(33))
                .PODestZip = "L11234" 'convertNULLToDefault(sArray(34))

            End If

            .POCarType = convertNULLToDefault(sArray(35))
            .POShipVia = convertNULLToDefault(sArray(36))
            .POShipViaType = convertNULLToDefault(sArray(37))
            .POPallets = convertNULLToDefault(sArray(38), True)
            '.POOtherCost = sArray(39)
            .POStatusFlag = convertNULLToDefault(sArray(40), True)
            '.POSortOrder = sArray(41)
            '.POPRONumber = sArray(42)
            '.POHoldLoad = sArray(43)
            .POOrderSequence = convertNULLToDefault(sArray(44), True)
            .POChepGLID = convertNULLToDefault(sArray(45))
            .POCarrierEquipmentCodes = convertNULLToDefault(sArray(46))
            .POCarrierTypeCode = convertNULLToDefault(sArray(47))
            .POPalletPositions = convertNULLToDefault(sArray(48))
            .POSchedulePUDate = convertNULLToDefault(sArray(49))
            .POSchedulePUTime = convertNULLToDefault(sArray(50))
            .POScheduleDelDate = convertNULLToDefault(sArray(51))
            .POSCheduleDelTime = convertNULLToDefault(sArray(52))
            .POActPUDate = convertNULLToDefault(sArray(53))
            .POActPUTime = convertNULLToDefault(sArray(54))
            .POActDelDate = convertNULLToDefault(sArray(55))
            .POActDelTime = convertNULLToDefault(sArray(56))
            '.POControl = sArray(57)
            '.POUpdated = sArray(58)
            .POOrigCompNumber = convertNULLToDefault(sArray(59))
            .POOrigAddress1 = convertNULLToDefault(sArray(60))
            .POOrigAddress2 = convertNULLToDefault(sArray(61))
            .POOrigAddress3 = convertNULLToDefault(sArray(62))
            .POOrigCountry = convertNULLToDefault(sArray(63))
            .POOrigContactPhone = convertNULLToDefault(sArray(64))
            .POOrigContactPhoneExt = convertNULLToDefault(sArray(65))
            .POOrigContactFax = convertNULLToDefault(sArray(66))

            If blnUseAltOrig Then
                .POOrigCompNumber = 0
                .POOrigAddress1 = "11 Abby Road"
                .POOrigAddress2 = "Not Here"
                .POOrigAddress3 = "Another Note"
                .POOrigCountry = "GB"
                .POOrigContactPhone = "111-111-1111"
                .POOrigContactPhoneExt = "333"
                .POOrigContactFax = "111-111-1112"
            End If

            .PODestCompNumber = convertNULLToDefault(sArray(67))
            .PODestAddress1 = convertNULLToDefault(sArray(68))
            .PODestAddress2 = convertNULLToDefault(sArray(69))
            .PODestAddress3 = convertNULLToDefault(sArray(70))
            .PODestCountry = convertNULLToDefault(sArray(71))
            .PODestContactPhone = convertNULLToDefault(sArray(72))
            .PODestContactPhoneExt = convertNULLToDefault(sArray(73))
            .PODestContactFax = convertNULLToDefault(sArray(74))

            If blnUseAltDest Then
                .PODestCompNumber = 0
                .PODestAddress1 = "123 Some Street"
                .PODestAddress2 = "Some Notes"
                .PODestAddress3 = "more Notes"
                .PODestCountry = "GB"
                .PODestContactPhone = "111-222-5555"
                .PODestContactPhoneExt = "333"
                .PODestContactFax = "111-222=5554" '
            End If
            .POPalletExchange = convertNULLToDefault(sArray(75), False, True)
            .POPalletType = convertNULLToDefault(sArray(76))
            .POComments = convertNULLToDefault(sArray(77))
            .POCommentsConfidential = convertNULLToDefault(sArray(78))
            .POInbound = convertNULLToDefault(sArray(79), False, True)
            .PODefaultRouteSequence = convertNULLToDefault(sArray(80), True)
            .PORouteGuideNumber = convertNULLToDefault(sArray(81))
            .POCompLegalEntity = convertNULLToDefault(sArray(82))
            .POCompAlphaCode = convertNULLToDefault(sArray(83))
            .POModeTypeControl = convertNULLToDefault(sArray(84), True)
            .POMustLeaveByDateTime = convertNULLToDefault(sArray(85))

            .POUser1 = "User 1 Msg" 'convertNULLToDefault(sArray(86))
            .POUser2 = "User 1 Msg" 'convertNULLToDefault(sArray(87))
            .POUser3 = "User 1 Msg" 'convertNULLToDefault(sArray(88))
            .POUser4 = "User 1 Msg" 'convertNULLToDefault(sArray(89))

            '.POUser1 = convertNULLToDefault(sArray(86))
            '.POUser2 = convertNULLToDefault(sArray(87))
            '.POUser3 = convertNULLToDefault(sArray(88))
            '.POUser4 = convertNULLToDefault(sArray(89))

            .POAPGLNumber = convertNULLToDefault(sArray(90))
        End With
        hdrs.Add(newPOHdr)
        Dim sItems As New List(Of Array)
        Dim strCSVItem As String = "489355,0,0.00,WPD40126,24,0.00,0.00,828,0,0,,8/59 Welch’s Purple Grape,,Data Integration DLL,2016-03-16 14:58:51.000,,,,NULL,,,101,0,N,-8999999999999735283,0x00000000005EA5A0,,,,,,,,0,0,0,0,0,0,0,0,0,0,,101,,,,,"
        sItems.Add(strCSVItem.Split(","))
        strCSVItem = "489355,0,0.00,WPD40127,5,0.00,0.00,172.5,0,0,,8/59 Welch’s Passionfruit,,Data Integration DLL,2016-03-16 14:58:51.000,,,,NULL,,,101,0,N,-8999999999999735282,0x00000000005EA5A1,,,,,,,,0,0,0,0,0,0,0,0,0,0,,101,,,,,"
        sItems.Add(strCSVItem.Split(","))
        For Each item In sItems
            Dim newPOItem As New NGLBookObjectWebService.clsBookDetailObject70
            With newPOItem
                .ItemPONumber = convertNULLToDefault(item(0))
                .FixOffInvAllow = convertNULLToDefault(item(1), True)
                .FixFrtAllow = convertNULLToDefault(item(2), True)
                .ItemNumber = convertNULLToDefault(item(3))
                .QtyOrdered = convertNULLToDefault(item(4), True) + QtyFactor
                .FreightCost = convertNULLToDefault(item(5), True)
                .ItemCost = convertNULLToDefault(item(6), True)
                .Weight = convertNULLToDefault(item(7), True)
                .Cube = convertNULLToDefault(item(8), True)
                .Pack = convertNULLToDefault(item(9), True)
                .Size = convertNULLToDefault(item(10))
                .Description = convertNULLToDefault(item(11))
                .Hazmat = convertNULLToDefault(item(12))
                '.CreatedUser = convertNULLToDefault(item(13))
                '.CreatedDate = convertNULLToDefault(item(14))
                .Brand = convertNULLToDefault(item(15))
                .CostCenter = convertNULLToDefault(item(16))
                .LotNumber = convertNULLToDefault(item(17))
                .LotExpirationDate = convertNULLToDefault(item(18))
                .GTIN = convertNULLToDefault(item(19))
                .CustItemNumber = convertNULLToDefault(item(20))
                .CustomerNumber = convertNULLToDefault(item(21))
                .POOrderSequence = convertNULLToDefault(item(22), True)
                .PalletType = convertNULLToDefault(item(23))
                '.POItemControl = convertNULLToDefault(item(24), False, True)
                '.POItemUpdated = convertNULLToDefault(item(25))
                .POItemHazmatTypeCode = convertNULLToDefault(item(26))
                .POItem49CFRCode = convertNULLToDefault(item(27))
                .POItemIATACode = convertNULLToDefault(item(28))
                .POItemDOTCode = convertNULLToDefault(item(29))
                .POItemMarineCode = convertNULLToDefault(item(30))
                .POItemNMFCClass = convertNULLToDefault(item(31))
                .POItemFAKClass = convertNULLToDefault(item(32))
                .POItemLimitedQtyFlag = convertNULLToDefault(item(33), False, True)
                .POItemPallets = convertNULLToDefault(item(34), True)
                .POItemTies = convertNULLToDefault(item(35), True)
                .POItemHighs = convertNULLToDefault(item(36), True)
                .POItemQtyPalletPercentage = convertNULLToDefault(item(37), True)
                .POItemQtyLength = convertNULLToDefault(item(38), True)
                .POItemQtyWidth = convertNULLToDefault(item(39), True)
                .POItemQtyHeight = convertNULLToDefault(item(40), True)
                .POItemStackable = convertNULLToDefault(item(41), False, True)
                .POItemLevelOfDensity = convertNULLToDefault(item(42), True)
                .POItemCompLegalEntity = convertNULLToDefault(item(43))
                .POItemCompAlphaCode = convertNULLToDefault(item(44))
                .POItemNMFCSubClass = convertNULLToDefault(item(45))
                .POItemUser1 = convertNULLToDefault(item(46))
                .POItemUser2 = convertNULLToDefault(item(47))
                .POItemUser3 = convertNULLToDefault(item(48))
                .POItemUser4 = convertNULLToDefault(item(49))
            End With
            dets.Add(newPOItem)
        Next

        retObj.BookHeaders = hdrs.ToArray()
        retObj.BookDetails = dets.ToArray()

        Return retObj
    End Function

    Private Function convertNULLToDefault(ByVal sVal As String, Optional ByVal blnIsNumeric As Boolean = False, Optional blnIsBoolean As Boolean = False) As Object
        If sVal.ToUpper = "NULL" Then
            If blnIsNumeric Then
                Return 0
            Else
                Return ""
            End If
        Else
            If blnIsNumeric Then
                If Not IsNumeric(sVal) Then
                    Return 0
                End If
            ElseIf blnIsBoolean Then
                Dim blnVal As Boolean = False
                Boolean.TryParse(sVal, blnVal)
                Return blnVal
            End If
        End If
        Return sVal
    End Function

End Class


Class BookTestObject

    Private _BookHeaders As NGLBookObjectWebService.clsBookHeaderObject70()
    Public Property BookHeaders() As NGLBookObjectWebService.clsBookHeaderObject70()
        Get
            Return _BookHeaders
        End Get
        Set(ByVal value As NGLBookObjectWebService.clsBookHeaderObject70())
            _BookHeaders = value
        End Set
    End Property

    Private _BookDetails As NGLBookObjectWebService.clsBookDetailObject70()
    Public Property BookDetails() As NGLBookObjectWebService.clsBookDetailObject70()
        Get
            Return _BookDetails
        End Get
        Set(ByVal value As NGLBookObjectWebService.clsBookDetailObject70())
            _BookDetails = value
        End Set
    End Property




End Class

Class BookTestObject80

    Private _BookHeaders As DTMSERPIntegration.clsBookHeaderObject80()
    Public Property BookHeaders() As DTMSERPIntegration.clsBookHeaderObject80()
        Get
            Return _BookHeaders
        End Get
        Set(ByVal value As DTMSERPIntegration.clsBookHeaderObject80())
            _BookHeaders = value
        End Set
    End Property

    Private _BookDetails As DTMSERPIntegration.clsBookDetailObject80()
    Public Property BookDetails() As DTMSERPIntegration.clsBookDetailObject80()
        Get
            Return _BookDetails
        End Get
        Set(ByVal value As DTMSERPIntegration.clsBookDetailObject80())
            _BookDetails = value
        End Set
    End Property




End Class
