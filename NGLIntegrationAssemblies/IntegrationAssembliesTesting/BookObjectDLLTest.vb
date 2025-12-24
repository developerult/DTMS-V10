Imports System
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports DAL = NGL.FreightMaster.Data
Imports NGL.FreightMaster.Integration



<TestClass()> _
Public Class BookObjectDLLTest
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
        oBook.Url = "https://wstest.nglnextrack.com/BookObject.asmx"
        Dim WebAuthCode As String = "NGLWSTEST"
        'debug
        'oBook.Url = "http://localhost:56526/bookobject.asmx"
        'Dim WebAuthCode As String = "NGLWSTEST"
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
    Public Sub NAVIntegrationDebugOrderData()
        Dim sSource As String = "NAVIntegrationDebugOrderData"
        Dim lOrderHeaders As New List(Of NGL.FreightMaster.Integration.clsBookHeaderObject70)
        Dim sOrderNumber As String = "SORobTst001"
        Dim sLaneNumber As String = "NGLS-Customer-02"
        Dim sLaneName As String = "Customer Test Lane 02"
        Dim iCompNumber As Integer = 0
        Dim sCompAlphaCode As String = "NGLSampleComp1"
        Dim sCompLegalEntity As String = "NGL"
        Dim sOrigAddress1 As String = "123 Some Street"
        Dim sOrigCity As String = "Some Town"
        Dim sOrigState As String = "IL"
        Dim sOrigZip As String = "60611"
        Dim sOrigCountry As String = "US"
        Dim sDestAddress1 As String = "1611 Colonial Parkway"
        Dim sDestCity As String = "Inverness"
        Dim sDestState As String = "IL"
        Dim sDestZip As String = "60067"
        Dim sDestCountry As String = "US"
        Dim oHeader As New NGL.FreightMaster.Integration.clsBookHeaderObject70
        With oHeader
            'required key fields
            .PONumber = sOrderNumber
            .POVendor = sLaneNumber
            .PODefaultCustomer = iCompNumber.ToString()
            .POCompAlphaCode = sCompAlphaCode
            .POCompLegalEntity = sCompLegalEntity
            .POdate = System.DateTime.Now.ToShortDateString()
            .POShipdate = System.DateTime.Now.ToShortDateString()
            .POWgt = 14000.0
            .POCube = 100
            .POQty = 1
            .POPallets = 1
            .POReqDate = System.DateTime.Now.ToShortDateString()
            .POCustomerPO = "XXX"
            .POStatusFlag = 5 'create the lane if it does not exist
            'optional fields
            .POComments = "PO Test Comments"
            .POCommentsConfidential = "PO Test Comments Confident"
            .PODestAddress1 = sDestAddress1
            .PODestCity = sDestCity
            .PODestCompNumber = 0
            .PODestContactFax = "1-847-963-0079"
            .PODestContactPhone = "1-847-963-0007"
            .PODestCountry = sDestCountry
            .PODestName = "Test Company 02"
            .PODestState = sDestState
            .PODestZip = sDestZip
            .POInbound = False
            .POModeTypeControl = 3
            .POOrigAddress1 = sOrigAddress1
            .POOrigCity = sOrigCity
            .POOrigCountry = sOrigCountry
            .POOrigState = sOrigState
            .POOrigZip = sOrigZip
            .POShipInstructions = "Test Ship Instructions"
            .POUser1 = "test User 1 Data"
        End With
        lOrderHeaders.add(oHeader)
        'oHeader.
        Dim lOrderDetails As New List(Of NGL.FreightMaster.Integration.clsBookDetailObject70)
        Dim oDetail As NGL.FreightMaster.Integration.clsBookDetailObject70 = NGL.FreightMaster.Integration.clsBookDetailObject70.GenerateSampleObject(sOrderNumber, iCompNumber, sCompAlphaCode, sCompLegalEntity)
        lOrderDetails.Add(oDetail)
        Dim ReturnMessage As String
        Dim result As Integer = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        ReturnMessage = ""

        Dim sDataType As String = "Book"
        Try
            If lOrderHeaders Is Nothing OrElse lOrderHeaders.Count() < 1 Then
                Assert.Fail("Empty Header Nothing To Do!")
                Return
            End If
            Dim oConfig As New Ngl.FreightMaster.Core.UserConfiguration()
            With oConfig
                .AdminEmail = "rramsey@nextgeneration.com"
                .AutoRetry = 0
                .Database = "NGLMASDEV705"
                .DBServer = "NGLRDP06D"
                .Debug = False
                .FromEmail = "system@nextgeneration.com"
                .GroupEmail = "rramsey@nextgeneration.com"
                .LogFile = "C:\Data\TMSLogs\" & sSource & ".log"
                .SMTPServer = "nglmail.ngl.local"
                .UserName = "ngl\rramsey"
                .WSAuthCode = "WSDEV"
                .WCFAuthCode = "WCFDEV"
                .WCFURL = "http://nglwcfdev705.nextgeneration.com"
                .WCFTCPURL = "net.tcp://nglwcfdev705.nextgeneration.com:705"
            End With
            Dim book As New NGL.FreightMaster.Integration.clsBook(oConfig)
            book.OrderNotificationEmail = "rramsey@nextgeneration.com"
            book.ValidateOrderUniqueness = False

            result = book.ProcessObjectData(lOrderHeaders, lOrderDetails, book.ConnectionString)
            ReturnMessage = book.LastError
            Select Case result
                Case NGL.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                    Assert.Fail("Data Connection Failure! could not import Order information:  " & ReturnMessage)
                Case NGL.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                    Assert.Fail("Integration Failure! could not import Order information:  " & ReturnMessage)
                Case NGL.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                    Assert.Fail("Integration Had Errors! Could not import some Order information:  " & ReturnMessage)
                Case NGL.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                    Assert.Fail("Warning!  Data Validation Failure! could not import Order information:  " & ReturnMessage)
                Case Else
                    ReturnMessage = "Success!"
            End Select


        Catch ex As ApplicationException
            Assert.Fail("Application Exception For " & sSource & ": {0} ", ex.Message)
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & sSource & ": {0} ", ex.Message)
        Finally

        End Try
    End Sub



    <TestMethod()>
    Public Sub JuiceIntegrationDebugOrderData()
        Dim sSource As String = "JuiceIntegrationDebugOrderData"
        Dim lOrderHeaders As New List(Of Ngl.FreightMaster.Integration.clsBookHeaderObject70)

        Dim oWCFParameters As New DAL.WCFParameters
        With oWCFParameters
            .UserName = "System Download"
            .Database = "NGLMASJuice"
            .DBServer = "NGLRDP06D\DTMS365"
            .ConnectionString = String.Format("Server={0};Database={1};Integrated Security=SSPI", "NGLRDP06D\DTMS365", "NGLMASJuice")
            .WCFAuthCode = "NGLSystem"
            .ValidateAccess = False
        End With
        Dim LaneCompControl As Integer = 0
        Dim strValMsg As String = ""
        Dim oLaneWCFData = New DAL.NGLLaneData(oWCFParameters)
        'Test the lane company integration issue
        Dim blnisLane = oLaneWCFData.ValidateLaneBeforeInsert(LaneCompControl,
                                                                 0,
                                                                 "201-1TROP-01",
                                                                 "201-1TROP-01",
                                                                 "",
                                                                 "201",
                                                                 strValMsg)

        Dim oHeader As New Ngl.FreightMaster.Integration.clsBookHeaderObject70
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
            .POStatusFlag = "0"
            .POOrderSequence = "0"
            '.POChepGLID/ = "
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
        lOrderHeaders.Add(oHeader)
        'oHeader.
        Dim lOrderDetails As New List(Of Ngl.FreightMaster.Integration.clsBookDetailObject70)
        Dim oDetail As New Ngl.FreightMaster.Integration.clsBookDetailObject70()
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
        lOrderDetails.Add(oDetail)
        Dim ReturnMessage As String
        Dim result As Integer = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        ReturnMessage = ""

        Dim sDataType As String = "Book"
        Try
            If lOrderHeaders Is Nothing OrElse lOrderHeaders.Count() < 1 Then
                Assert.Fail("Empty Header Nothing To Do!")
                Return
            End If
            Dim oConfig As New Ngl.FreightMaster.Core.UserConfiguration()
            With oConfig
                .AdminEmail = "rramsey@nextgeneration.com"
                .AutoRetry = 0
                .Database = "NGLMASJuice"
                .DBServer = "NGLRDP06D\DTMS365"
                .Debug = True
                .FromEmail = "system@nextgeneration.com"
                .GroupEmail = "rramsey@nextgeneration.com"
                .LogFile = "C:\Data\TMSLogs\" & sSource & ".log"
                .SMTPServer = "nglmail.ngl.local"
                .UserName = "ngl\rramsey"
                .WSAuthCode = "WSDEV"
                .WCFAuthCode = "WCFDEV"
                .WCFURL = "http://nglwcfdev705.nextgeneration.com"
                .WCFTCPURL = "net.tcp://nglwcfdev705.nextgeneration.com:705"
            End With
            Dim book As New Ngl.FreightMaster.Integration.clsBook(oConfig)
            book.OrderNotificationEmail = "rramsey@nextgeneration.com"
            book.ValidateOrderUniqueness = False

            result = book.ProcessObjectData(lOrderHeaders, lOrderDetails, book.ConnectionString)
            ReturnMessage = book.LastError
            Select Case result
                Case Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                    Assert.Fail("Data Connection Failure! could not import Order information:  " & ReturnMessage)
                Case Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                    Assert.Fail("Integration Failure! could not import Order information:  " & ReturnMessage)
                Case Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                    Assert.Fail("Integration Had Errors! Could not import some Order information:  " & ReturnMessage)
                Case Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                    Assert.Fail("Warning!  Data Validation Failure! could not import Order information:  " & ReturnMessage)
                Case Else
                    ReturnMessage = "Success!"
            End Select


        Catch ex As ApplicationException
            Assert.Fail("Application Exception For " & sSource & ": {0} ", ex.Message)
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & sSource & ": {0} ", ex.Message)
        Finally

        End Try
    End Sub



    <TestMethod()>
    Public Sub ProcessObjectDataBooking80Test()
        Dim sSource As String = "ProcessObjectDataBooking80Test"
        Dim lOrderHeaders As New List(Of NGL.FreightMaster.Integration.clsBookHeaderObject80)

        Dim oWCFParameters As New DAL.WCFParameters
        With oWCFParameters
            .UserName = "System Download"
            .Database = "NGLMASColorTech"
            .DBServer = "DESKTOP-0R0EJUB"
            .ConnectionString = String.Format("Server={0};Database={1};Integrated Security=SSPI", .DBServer, .Database)
            .WCFAuthCode = "NGLSystem"
            .ValidateAccess = False
        End With

        Dim oHeader As New NGL.FreightMaster.Integration.clsBookHeaderObject80
        With oHeader
            .PONumber = "PO-0000183"
            .POVendor = "60132-2399-1-VU00156-MOA"
            .POdate = "2021-01-12"
            .POShipdate = "2021-02-05"
            .POFrt = "5"
            .POTotalFrt = "0"
            .POTotalCost = "0"
            .POWgt = "42000"
            .POCube = "0"
            .POQty = "42000"
            .POLines = "0"
            .POConfirm = "false"
            .PODefaultCustomer = "3"
            .PODefaultCarrier = "998"
            .POReqDate = "2021-02-05"
            '.POShipInstructions/ = "
            .POCooler = "false"
            .POFrozen = "false"
            .PODry = "true"
            .POTemp = "D"
            '.POCarType/ = "
            .POShipVia = "53"
            .POShipViaType = "ShipViaCode"
            '.POConsigneeNumber/ = "
            .POOtherCosts = "0"
            .POOrderSequence = "0"
            '.POChepGLID/ = "
            .POOrigName = "NOVA CHEMICALS INC."
            .POOrigAddress1 = "58 WEST ROAD"
            .POOrigAddress2 = " "
            .POOrigCity = "Carol Stream"
            .POOrigState = "IL"
            .POOrigCountry = "US"
            .POOrigZip = "60132-2399"
            .PODestName = "CT PLT 1"
            .PODestAddress1 = " "
            .PODestAddress2 = " "
            .PODestAddress3 = " "
            .PODestCity = "Morristown"
            .PODestState = "TN"
            .PODestCountry = "US"
            .PODestZip = "37814"
            .POPallets = "42"
            .POStatusFlag = "0"
            .POInbound = "true"
            .POPalletExchange = "false"
            .PODefaultRouteSequence = "0"
            .POCompAlphaCode = "TN-PLT 1"
            .POModeTypeControl = "3"
            .POCompLegalEntity = "Morristown"
        End With
        lOrderHeaders.Add(oHeader)
        'oHeader.
        Dim lOrderDetails As New List(Of NGL.FreightMaster.Integration.clsBookDetailObject80)
        Dim oDetail As New NGL.FreightMaster.Integration.clsBookDetailObject80()

        With oDetail
            .ItemPONumber = "PO-0000183"
            .FixOffInvAllow = "0"
            .FixFrtAllow = "0"
            .ItemNumber = "RR00101"
            .QtyOrdered = "42000"
            .FreightCost = "0"
            .ItemCost = "34860"
            .Weight = "42000"
            .Cube = "0"
            .Pack = "0"
            .Description = "1 MI HEX (p), TF-0119-F, LF2020"
            .CustomerNumber = "3"
            .POOrderSequence = "0"
            .PalletType = "N"
            .POItemLimitedQtyFlag = "false"
            .POItemPallets = "42"
            .POItemTies = "0"
            .POItemHighs = "0"
            .POItemQtyPalletPercentage = "0"
            .POItemQtyLength = "0"
            .POItemQtyWidth = "0"
            .POItemQtyHeight = "0"
            .POItemStackable = "false"
            .POItemLevelOfDensity = "0"
            .POItemCompAlphaCode = "TN-PLT 1"
            .POItemCompLegalEntity = "Morristown"
            .POItemOrderNumber = "PO-0000183"
        End With
        lOrderDetails.Add(oDetail)
        Dim ReturnMessage As String
        Dim result As Integer = NGL.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        ReturnMessage = ""

        Dim sDataType As String = "Book"
        Try
            If lOrderHeaders Is Nothing OrElse lOrderHeaders.Count() < 1 Then
                Assert.Fail("Empty Header Nothing To Do!")
                Return
            End If
            Dim oConfig As New NGL.FreightMaster.Core.UserConfiguration()
            With oConfig
                .AdminEmail = "rramsey@nextgeneration.com"
                .AutoRetry = 0
                .Database = oWCFParameters.Database
                .DBServer = oWCFParameters.DBServer
                .Debug = True
                .FromEmail = "system@nextgeneration.com"
                .GroupEmail = "rramsey@nextgeneration.com"
                .LogFile = "C:\Data\TMSLogs\" & sSource & ".log"
                .SMTPServer = "nglmail.ngl.local"
                .UserName = "ngl\rramsey"
                .WSAuthCode = "WSPROD"
                .WCFAuthCode = "WCFPROD"
                .WCFURL = "http://localhost:56533"
                .WCFTCPURL = "net.tcp://localhost:56533:808"
            End With
            Dim book As New NGL.FreightMaster.Integration.clsBook(oConfig)
            book.OrderNotificationEmail = "rramsey@nextgeneration.com"
            book.ValidateOrderUniqueness = False

            result = book.ProcessObjectData(lOrderHeaders, lOrderDetails, book.ConnectionString)
            ReturnMessage = book.LastError
            Select Case result
                Case NGL.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                    Assert.Fail("Data Connection Failure! could not import Order information:  " & ReturnMessage)
                Case NGL.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                    Assert.Fail("Integration Failure! could not import Order information:  " & ReturnMessage)
                Case NGL.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                    Assert.Fail("Integration Had Errors! Could not import some Order information:  " & ReturnMessage)
                Case NGL.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                    Assert.Fail("Warning!  Data Validation Failure! could not import Order information:  " & ReturnMessage)
                Case Else
                    ReturnMessage = "Success!"
            End Select


        Catch ex As ApplicationException
            Assert.Fail("Application Exception For " & sSource & ": {0} ", ex.Message)
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & sSource & ": {0} ", ex.Message)
        Finally

        End Try
    End Sub



    <TestMethod()>
    Public Sub ProcessDeleteByOrderNumber()
        Dim sSource As String = "ProcessDeleteByOrderNumber"
        Dim lOrderHeaders As New List(Of NGL.FreightMaster.Integration.clsBookHeaderObject80)

        Dim oWCFParameters As New DAL.WCFParameters
        With oWCFParameters
            .UserName = "System Download"
            .Database = "NGLMASJuice"
            .DBServer = "DESKTOP-0R0EJUB"
            .ConnectionString = String.Format("Server={0};Database={1};Integrated Security=SSPI", "DESKTOP-0R0EJUB", "NGLMASJuice")
            .WCFAuthCode = "NGLSystem"
            .ValidateAccess = False
        End With

        Dim strValMsg As String = ""
        Dim sOrderNumber = "618663"
        Dim ReturnMessage As String
        Dim result As Integer = NGL.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        ReturnMessage = ""

        Dim sDataType As String = "Book"
        Try

            Dim oConfig As New NGL.FreightMaster.Core.UserConfiguration()
            With oConfig
                .AdminEmail = "rramsey@nextgeneration.com"
                .AutoRetry = 0
                .Database = "NGLMASJuice"
                .DBServer = "DESKTOP-0R0EJUB"
                .Debug = True
                .FromEmail = "system@nextgeneration.com"
                .GroupEmail = "rramsey@nextgeneration.com"
                .LogFile = "C:\Data\TMSLogs\" & sSource & ".log"
                .SMTPServer = "nglmail.ngl.local"
                .UserName = "DESKTOP-0R0EJUB\rkrte"
                .WSAuthCode = "WSDEV"
                .WCFAuthCode = "WCFDEV"
                .WCFURL = "http://nglwcfdev705.nextgeneration.com"
                .WCFTCPURL = "net.tcp://nglwcfdev705.nextgeneration.com:705"
                .ConnectionString = oWCFParameters.ConnectionString
            End With
            Dim book As New NGL.FreightMaster.Integration.clsBook(oConfig)
            book.OrderNotificationEmail = "rramsey@nextgeneration.com"
            book.ValidateOrderUniqueness = False

            result = book.ProcessDeleteByOrderNumber(sOrderNumber, book.ConnectionString)
            ReturnMessage = book.LastError
            Select Case result
                Case NGL.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                    Assert.Fail("Data Connection Failure! could not import Order information:  " & ReturnMessage)
                Case NGL.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                    Assert.Fail("Integration Failure! could not import Order information:  " & ReturnMessage)
                Case NGL.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                    Assert.Fail("Integration Had Errors! Could not import some Order information:  " & ReturnMessage)
                Case NGL.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                    Assert.Fail("Warning!  Data Validation Failure! could not import Order information:  " & ReturnMessage)
                Case Else
                    ReturnMessage = "Success!"
            End Select


        Catch ex As ApplicationException
            Assert.Fail("Application Exception For " & sSource & ": {0} ", ex.Message)
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & sSource & ": {0} ", ex.Message)
        Finally

        End Try
    End Sub

    <TestMethod()>
    Public Sub CPUIntegrationAutomationTest()
        Dim sSource As String = "CPUIntegrationAutomationTest"
        Dim lOrderHeaders As New List(Of NGL.FreightMaster.Integration.clsBookHeaderObject70)
        Dim sOrderNumber As String = "CPUTst001"
        Dim sLaneNumber As String = "31- California Custom Fruits & Flavor"
        Dim sLaneName As String = "VJI VJI California Custom Fruits & Flavor"
        Dim iCompNumber As Integer = 31
        Dim sCompAlphaCode As String = ""
        Dim sCompLegalEntity As String = "Veggie"
        Dim oHeader As New NGL.FreightMaster.Integration.clsBookHeaderObject70
        With oHeader
            'required key fields
            .PONumber = sOrderNumber
            .POVendor = sLaneNumber
            .PODefaultCustomer = iCompNumber.ToString()
            .POCompAlphaCode = sCompAlphaCode
            .POCompLegalEntity = sCompLegalEntity
            .POdate = System.DateTime.Now.ToShortDateString()
            .POShipdate = System.DateTime.Now.ToShortDateString()
            .POWgt = 14000.0
            .POCube = 100
            .POQty = 1
            .POPallets = 1
            .POReqDate = System.DateTime.Now.ToShortDateString()
            .POCustomerPO = "XXX"
            .POStatusFlag = 0
            'optional fields
            .POComments = "PO Test Comments"
            .POCommentsConfidential = "PO Test Comments Confident"
            .POModeTypeControl = 3
            .POFrt = 3
            .POShipInstructions = "Test Ship Instructions"
            .POUser1 = "test User 1 Data"
        End With
        lOrderHeaders.Add(oHeader)
        'oHeader.
        Dim lOrderDetails As New List(Of NGL.FreightMaster.Integration.clsBookDetailObject70)
        Dim oDetail As NGL.FreightMaster.Integration.clsBookDetailObject70 = NGL.FreightMaster.Integration.clsBookDetailObject70.GenerateSampleObject(sOrderNumber, iCompNumber, sCompAlphaCode, sCompLegalEntity)
        lOrderDetails.Add(oDetail)
        Dim ReturnMessage As String
        Dim result As Integer = NGL.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        ReturnMessage = ""

        Dim sDataType As String = "Book"
        Try
            If lOrderHeaders Is Nothing OrElse lOrderHeaders.Count() < 1 Then
                Assert.Fail("Empty Header Nothing To Do!")
                Return
            End If
            Dim oConfig As New NGL.FreightMaster.Core.UserConfiguration()
            With oConfig
                .AdminEmail = "rramsey@nextgeneration.com"
                .AutoRetry = 0
                .Database = "NGLMASDEV705"
                .DBServer = "NGLRDP06D"
                .Debug = False
                .FromEmail = "system@nextgeneration.com"
                .GroupEmail = "rramsey@nextgeneration.com"
                .LogFile = "C:\Data\TMSLogs\" & sSource & ".log"
                .SMTPServer = "nglmail.ngl.local"
                .UserName = "ngl\rramsey"
                .WSAuthCode = "WSDEV"
                .WCFAuthCode = "WCFDEV"
                .WCFURL = "http://nglwcfdev705.nextgeneration.com"
                .WCFTCPURL = "net.tcp://nglwcfdev705.nextgeneration.com:705"
            End With
            Dim book As New NGL.FreightMaster.Integration.clsBook(oConfig)
            book.OrderNotificationEmail = "rramsey@nextgeneration.com"
            book.ValidateOrderUniqueness = False
            book.RunSilentTenderAsync = False
            result = book.ProcessObjectData(lOrderHeaders, lOrderDetails, book.ConnectionString)
            ReturnMessage = book.LastError
            Select Case result
                Case NGL.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                    Assert.Fail("Data Connection Failure! could not import Order information:  " & ReturnMessage)
                Case NGL.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                    Assert.Fail("Integration Failure! could not import Order information:  " & ReturnMessage)
                Case NGL.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                    Assert.Fail("Integration Had Errors! Could not import some Order information:  " & ReturnMessage)
                Case NGL.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                    Assert.Fail("Warning!  Data Validation Failure! could not import Order information:  " & ReturnMessage)
                Case Else
                    ReturnMessage = "Success!"
            End Select


        Catch ex As ApplicationException
            Assert.Fail("Application Exception For " & sSource & ": {0} ", ex.Message)
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & sSource & ": {0} ", ex.Message)
        Finally

        End Try
    End Sub

    'ProcessSilentTenders
    <TestMethod()>
    Public Sub CPUProcessSilentTendersTest()
        Dim sSource As String = "CPUProcessSilentTendersTest"
        Dim sOrderNumber As String = "NSO10003"
        Try
            Dim oConfig As New NGL.FreightMaster.Core.UserConfiguration()
            With oConfig
                .AdminEmail = "rramsey@nextgeneration.com"
                .AutoRetry = 0
                .Database = "NGLMASProd"
                .DBServer = "NGLRDP07D"
                .Debug = False
                .FromEmail = "system@nextgeneration.com"
                .GroupEmail = "rramsey@nextgeneration.com"
                .LogFile = "C:\Data\TMSLogs\" & sSource & ".log"
                .SMTPServer = "nglmail.ngl.local"
                .UserName = "ngl\rramsey"
                .WSAuthCode = "WSDEV"
                .WCFAuthCode = "WCFDEV"
                .WCFURL = "http://nglwcfdev705.nextgeneration.com"
                .WCFTCPURL = "net.tcp://nglwcfdev705.nextgeneration.com:705"
            End With
            Dim book As New NGL.FreightMaster.Integration.clsBook(oConfig)
            book.OrderNotificationEmail = "rramsey@nextgeneration.com"
            book.ValidateOrderUniqueness = False
            book.RunSilentTenderAsync = False
            book.mstrOrderNumbers = New List(Of String) From {sOrderNumber}
            book.ProcessSilentTenders()


        Catch ex As ApplicationException
            Assert.Fail("Application Exception For " & sSource & ": {0} ", ex.Message)
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & sSource & ": {0} ", ex.Message)
        Finally

        End Try
    End Sub

    <TestMethod()>
    Public Sub AutoImportProcessTest()
        Dim sSource As String = "AutoImportProcessTestDemo"
        Dim sOrderNumber As String = "PQS013030"
        Try
            Dim oConfig As New NGL.FreightMaster.Core.UserConfiguration()
            With oConfig
                .AdminEmail = "rramsey@nextgeneration.com"
                .AutoRetry = 0
                .Database = "NGLMASPROD"
                .DBServer = "NGLRDP07D"
                .Debug = False
                .FromEmail = "system@nextgeneration.com"
                .GroupEmail = "rramsey@nextgeneration.com"
                .LogFile = "C:\Data\TMSLogs\" & sSource & ".log"
                .SMTPServer = "nglmail.ngl.local"
                .UserName = "ngl\rramsey"
                .WSAuthCode = "WSDEV"
                .WCFAuthCode = "WCFDEV"
                .WCFURL = "http://nglwcfdev706.nextgeneration.com"
                .WCFTCPURL = "net.tcp://nglwcfdev706.nextgeneration.com:706"
            End With
            Dim book As New NGL.FreightMaster.Integration.clsBook(oConfig)
            book.OrderNotificationEmail = "rramsey@nextgeneration.com"
            book.ValidateOrderUniqueness = False
            book.RunSilentTenderAsync = False
            book.mstrOrderNumbers = New List(Of String) From {sOrderNumber}
            book.ProcessSilentTenders()


        Catch ex As ApplicationException
            Assert.Fail("Application Exception For " & sSource & ": {0} ", ex.Message)
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & sSource & ": {0} ", ex.Message)
        Finally

        End Try
    End Sub

    'ProcessSilentTenderJuice
    <TestMethod()>
    Public Sub CPUProcessSilentTenderJuiceTest()
        Dim sSource As String = "CPUProcessSilentTenderJuiceTest"
        Dim sOrderNumber As String = "489355"
        Try
            Dim oConfig As New NGL.FreightMaster.Core.UserConfiguration()
            With oConfig
                .AdminEmail = "rramsey@nextgeneration.com"
                .AutoRetry = 0
                .Database = "NGLMASJuice"
                .DBServer = "NGLRDP06D"
                .Debug = False
                .FromEmail = "system@nextgeneration.com"
                .GroupEmail = "rramsey@nextgeneration.com"
                .LogFile = "C:\Data\TMSLogs\" & sSource & ".log"
                .SMTPServer = "nglmail.ngl.local"
                .UserName = "ngl\rramsey"
                .WSAuthCode = "WSDEV"
                .WCFAuthCode = "WCFDEV"
                .WCFURL = "http://nglwcfdev705.nextgeneration.com"
                .WCFTCPURL = "net.tcp://nglwcfdev705.nextgeneration.com:705"
            End With
            Dim book As New NGL.FreightMaster.Integration.clsBook(oConfig)
            book.OrderNotificationEmail = "rramsey@nextgeneration.com"
            book.ValidateOrderUniqueness = False
            book.RunSilentTenderAsync = False
            book.mstrOrderNumbers = New List(Of String) From {sOrderNumber}
            book.ProcessSilentTenders()


        Catch ex As ApplicationException
            Assert.Fail("Application Exception For " & sSource & ": {0} ", ex.Message)
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & sSource & ": {0} ", ex.Message)
        Finally

        End Try
    End Sub



    <TestMethod()>
    Public Sub DeleteProcessBookJuiceTest()
        Dim sSource As String = "DeleteProcessBookJuiceTest"
        Dim sOrderNumber As String = "489356"
        Try
            Dim oConfig As New NGL.FreightMaster.Core.UserConfiguration()
            With oConfig
                .AdminEmail = "rramsey@nextgeneration.com"
                .AutoRetry = 0
                .Database = "NGLMASJuice"
                .DBServer = "NGLRDP06D"
                .Debug = False
                .FromEmail = "system@nextgeneration.com"
                .GroupEmail = "rramsey@nextgeneration.com"
                .LogFile = "C:\Data\TMSLogs\" & sSource & ".log"
                .SMTPServer = "nglmail.ngl.local"
                .UserName = "ngl\rramsey"
                .WSAuthCode = "WSDEV"
                .WCFAuthCode = "WCFDEV"
                .WCFURL = "http://nglwcfdev705.nextgeneration.com"
                .WCFTCPURL = "net.tcp://nglwcfdev705.nextgeneration.com:705"
            End With
            Dim book As New NGL.FreightMaster.Integration.clsBook(oConfig)
            book.OrderNotificationEmail = "rramsey@nextgeneration.com"
            book.ValidateOrderUniqueness = False
            book.RunSilentTenderAsync = False
            Dim lOrderHeaders As New List(Of clsBookHeaderObject70)
            Dim lOrderDetails As New List(Of clsBookDetailObject70)
            Dim oBook As New clsBookHeaderObject70 With {.POOrderSequence = 0, .PONumber = "489356", .POStatusFlag = 2}
            lOrderHeaders.Add(oBook)
            Dim result = book.ProcessObjectData(lOrderHeaders, lOrderDetails, book.ConnectionString)
            Dim ReturnMessage = book.LastError

            If Not String.IsNullOrWhiteSpace(ReturnMessage) Then
                Assert.Fail(ReturnMessage)
            End If

        Catch ex As ApplicationException
            Assert.Fail("Application Exception For " & sSource & ": {0} ", ex.Message)
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & sSource & ": {0} ", ex.Message)
        Finally

        End Try
    End Sub


End Class
