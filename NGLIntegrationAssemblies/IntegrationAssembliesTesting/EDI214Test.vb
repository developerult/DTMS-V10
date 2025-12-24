Imports System
Imports Tar = NGL.FM.CarTar
Imports DAL = NGL.FreightMaster.Data
Imports DTO = NGL.FreightMaster.Data.DataTransferObjects
Imports BLL = NGL.FM.BLL
Imports LTS = Ngl.FreightMaster.Data.LTS
Imports Ngl.FreightMaster.Integration
Imports NDT = Ngl.Core.Utility.DataTransformation

<TestClass()> Public Class EDI214Test
    Inherits TestBase

    ''<TestMethod()> _
    ''Public Sub EDI214UnitTest()
    ''    Try
    ''        Dim BookProPrefix As String = "VJI"
    ''        Dim BookProBase As String = "12345"
    ''        Dim BookProNumber = BookProPrefix & BookProBase
    ''        Dim BookConsPrefix As String = "TST1234567"
    ''        Dim OrderNumberBase As String = "1111"
    ''        Dim BookCarrOrderNumber As String = "SO-UnitTEST1111"
    ''        Dim dtOrder As Date = (Date.Now().AddDays(60))
    ''        Dim dtPickUp As Date = dtOrder.AddDays(3)
    ''        Dim dtEnRoute As Date = dtOrder.AddDays(4).AddHours(10)
    ''        Dim dtDelivery As Date = dtOrder.AddDays(5)
    ''        'Create a Test Order
    ''        CreateTestOrder(BookProPrefix, BookProBase, BookConsPrefix, BookCarrOrderNumber, Date.Now().AddDays(60))
    ''        'Assign an NGL Carrier form a broker that uses the NGL Tariff
    ''        SetNGLAssignedCarrier(BookProNumber, Date.Now())
    ''        Return
    ''        'Assign an interline carrier
    ''        SetAssignedCarrier(BookProNumber, Date.Now())
    ''        Return
    ''        'set pickup appointment for managed facility 
    ''        SetPickupAppointment(BookProNumber, dtPickUp)
    ''        'System.Threading.Thread.Sleep(3000)
    ''        'set delivery appointment for non managed facility
    ''        SetDeliveryAppointment(BookProNumber, dtDelivery)
    ''        'System.Threading.Thread.Sleep(3000)
    ''        'set carrier arrived at pickup for managed facility 
    ''        SetCarrierArrivedAtPickup(BookProNumber, dtPickUp)
    ''        'set carrier loading at pickup for managed facility 
    ''        SetCarrierLoadingAtPickup(BookProNumber, dtPickUp)
    ''        'set carrier finished loading at pickup for managed facility
    ''        SetCarrierFinishedLoadingAtPickup(BookProNumber, dtPickUp)
    ''        'set carrier departed pickup for managed facility
    ''        SetCarrierDepartedPickup(BookProNumber, dtPickUp)
    ''        ' System.Threading.Thread.Sleep(3000)
    ''        'set carrier en route message
    ''        SetEnRouteToDelivery(BookProNumber, dtEnRoute)
    ''        dtEnRoute = dtEnRoute.AddHours(2)
    ''        SetEquipmentBreakDownDelivery(BookProNumber, dtEnRoute)
    ''        dtEnRoute = dtEnRoute.AddHours(2)
    ''        SetShipperRelatedDelay(BookProNumber, dtEnRoute)
    ''        dtEnRoute = dtEnRoute.AddHours(2)
    ''        SetConsigneeRelatedDelay(BookProNumber, dtEnRoute)
    ''        'set carrier arrived at delivery for non managed facility
    ''        SetCarrierArrivedAtDelivery(BookProNumber, dtDelivery)
    ''        'set carrier unloading at delivery for non managed facility
    ''        SetCarrierUnloadingAtDelivery(BookProNumber, dtDelivery)
    ''        'set carrier finished unloading at delivery for non managed facility
    ''        SetCarrierFinishedUnloadingAtDelivery(BookProNumber, dtDelivery)
    ''        'set carrier finished unloading at delivery for non managed facility
    ''        SetCarrierDepartedDelivery(BookProNumber, dtDelivery)
    ''        'System.Threading.Thread.Sleep(3000)


    ''    Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
    ''        Throw
    ''    Catch ex As Exception
    ''        Assert.Fail("SetPickupAppointment Unexpected Error: " & ex.Message)
    ''    Finally

    ''    End Try
    ''End Sub

    ''Private Sub processEDI214CarrierAssignmentMessage(ByVal sCaller As String, ByVal strBookProNumber As String, ByVal dtAT7 As Date, ByVal sB1001Pro As String, ByVal sN102Name As String, ByVal sN104SCAC As String)
    ''    Try
    ''        Dim oEDIInput As New clsEDIInput

    ''        Dim enumResults As Configuration.ProcessDataReturnValues = Configuration.ProcessDataReturnValues.nglDataIntegrationComplete
    ''        testParameters.WCFAuthCode = "NGLSystem"
    ''        Dim osysPar = getSystemParameters()
    ''        Dim strLastError As String = ""
    ''        Dim str997sOut As String = ""
    ''        Dim strEDI As String
    ''        Dim strEDIXAction As String = "214"
    ''        'get the booking records
    ''        Dim oBook = getBookByPro(strBookProNumber)
    ''        If oBook Is Nothing OrElse oBook.BookControl = 0 Then
    ''            Assert.Fail(sCaller & ": cannot read booking records.")
    ''        End If
    ''        Dim intCarrierControl As Integer = oBook.BookCarrierControl
    ''        Dim oCompEDI As DTO.CompEDI = readCompEDI(oBook.BookCustCompControl, strEDIXAction)
    ''        If oCompEDI Is Nothing OrElse oCompEDI.CompEDIControl = 0 Then
    ''            Assert.Fail(sCaller & ": company is not set up for EDI.")
    ''        End If
    ''        Dim oCarrEDI As DTO.CarrierEDI = readCarrierEDI(oBook.BookCarrierControl, strEDIXAction)
    ''        If oCarrEDI Is Nothing OrElse oCarrEDI.CarrierEDIControl = 0 Then
    ''            Assert.Fail(sCaller & ": carrier is not set up for EDI.")
    ''        End If
    ''        Dim dtSend As Date = Date.Now()
    ''        Dim SegmentTerminator As String
    ''        Dim ISASequence As String
    ''        Dim GSSequence As String
    ''        Dim blnInbound As Boolean = True
    ''        Dim strISA = buildISAString(oCompEDI, oCarrEDI, dtSend, SegmentTerminator, ISASequence, blnInbound)
    ''        Dim strGS = buildGSString(oCompEDI, oCarrEDI, dtSend, strEDIXAction, SegmentTerminator, GSSequence, blnInbound)
    ''        Dim intSequence As Integer = 1
    ''        Dim intSegments As Integer = 0
    ''        Dim o214 As New clsEDI214
    ''        With o214
    ''            .ST.ST01 = "214"
    ''            .ST.ST02 = intSequence.ToString()
    ''            intSegments += 1 'increase the segment counter after each segment
    ''            .B10.B1001 = sB1001Pro
    ''            .B10.B1002 = oBook.BookSHID
    ''            .B10.B1003 = oCarrEDI.CarrierEDIPartnerCode
    ''            intSegments += 1 'increase the segment counter after each segment
    ''            .L11.L1101 = "003"
    ''            .L11.L1102 = "19"
    ''            intSegments += 1 'increase the segment counter after each segment
    ''            Dim o100Orig As New clsEDI214Loop100
    ''            o100Orig.N1.N101 = "SF"
    ''            o100Orig.N1.N102 = oBook.BookOrigName
    ''            o100Orig.N1.N103 = "92"
    ''            intSegments += 1 'increase the segment counter after each segment
    ''            o100Orig.N3.N301 = oBook.BookOrigAddress1
    ''            intSegments += 1 'increase the segment counter after each segment
    ''            o100Orig.N4.N401 = oBook.BookOrigCity
    ''            o100Orig.N4.N402 = oBook.BookOrigState
    ''            o100Orig.N4.N403 = oBook.BookOrigZip
    ''            o100Orig.N4.N404 = oBook.BookOrigCountry
    ''            intSegments += 1 'increase the segment counter after each segment
    ''            Dim l100s As New List(Of clsEDI214Loop100)
    ''            l100s.Add(o100Orig)
    ''            Dim o100Dest As New clsEDI214Loop100
    ''            o100Dest.N1.N101 = "ST"
    ''            o100Dest.N1.N102 = oBook.BookDestName
    ''            o100Dest.N1.N103 = "92"
    ''            intSegments += 1 'increase the segment counter after each segment
    ''            o100Dest.N3.N301 = oBook.BookDestAddress1
    ''            intSegments += 1 'increase the segment counter after each segment
    ''            o100Dest.N4.N401 = oBook.BookDestCity
    ''            o100Dest.N4.N402 = oBook.BookDestState
    ''            o100Dest.N4.N403 = oBook.BookDestZip
    ''            o100Dest.N4.N404 = oBook.BookDestCountry
    ''            intSegments += 1 'increase the segment counter after each segment
    ''            l100s.Add(o100Dest)
    ''            Dim o100Carrier As New clsEDI214Loop100
    ''            o100Carrier.N1.N101 = "CA"
    ''            o100Carrier.N1.N102 = sN102Name
    ''            o100Carrier.N1.N103 = "ZZ"
    ''            o100Carrier.N1.N104 = sN104SCAC
    ''            intSegments += 1 'increase the segment counter after each segment
    ''            o100Carrier.N3.N301 = oBook.BookDestAddress1
    ''            intSegments += 1 'increase the segment counter after each segment
    ''            o100Carrier.N4.N401 = oBook.BookDestCity
    ''            o100Carrier.N4.N402 = oBook.BookDestState
    ''            o100Carrier.N4.N403 = oBook.BookDestZip
    ''            o100Carrier.N4.N404 = oBook.BookDestCountry
    ''            intSegments += 1 'increase the segment counter after each segment
    ''            l100s.Add(o100Carrier)
    ''            o214.Loop100 = l100s.ToArray()
    ''            Dim l200s As New List(Of clsEDI214Loop200)
    ''            Dim o200Orig As New clsEDI214Loop200
    ''            o200Orig.LX.LX01 = 1 'Note: stop numbers must start with stop 1.  Stop 0 is not allowed. Stop 1 = Pickup Location
    ''            intSegments += 1 'increase the segment counter after each segment
    ''            Dim l205s As New List(Of clsEDI214Loop205)
    ''            Dim o205Orig As New clsEDI214Loop205
    ''            o205Orig.AT7.AT701 = "XB"
    ''            o205Orig.AT7.AT702 = "NS"
    ''            o205Orig.AT7.AT703 = ""
    ''            o205Orig.AT7.AT704 = ""
    ''            o205Orig.AT7.AT705 = dtAT7.ToString("yyyyMMdd")
    ''            o205Orig.AT7.AT706 = dtAT7.ToString("HHmm")
    ''            o205Orig.AT7.AT707 = "LT"
    ''            intSegments += 1 'increase the segment counter after each segment
    ''            o205Orig.MS1.MS101 = oBook.BookOrigCity
    ''            o205Orig.MS1.MS102 = oBook.BookOrigState
    ''            o205Orig.MS1.MS103 = oBook.BookOrigCountry
    ''            intSegments += 1 'increase the segment counter after each segment
    ''            o205Orig.MS2.MS201 = oCarrEDI.CarrierEDIPartnerCode
    ''            o205Orig.MS2.MS202 = "11111" 'equipment number
    ''            o205Orig.MS2.MS203 = "TL" 'FT – Flatbed; TV – Truck, Van; TW – Reefer;TL – Truck Load
    ''            intSegments += 1 'increase the segment counter after each segment
    ''            l205s.Add(o205Orig)
    ''            o200Orig.Loop205 = l205s.ToArray()
    ''            o200Orig.L11.L1101 = oBook.BookCarrOrderNumber & "-" & oBook.BookOrderSequence
    ''            o200Orig.L11.L1102 = "ON"
    ''            intSegments += 1 'increase the segment counter after each segment
    ''            'add any additional L11 segments to the L11s list if needed
    ''            o200Orig.K1.K101 = "EDI 214 Unit Test"
    ''            o200Orig.K1.K102 = " - " & sCaller & " - "
    ''            intSegments += 1 'increase the segment counter after each segment
    ''            o200Orig.AT8.AT801 = "G"
    ''            o200Orig.AT8.AT802 = "L"
    ''            o200Orig.AT8.AT803 = oBook.BookTotalWgt.ToString()
    ''            o200Orig.AT8.AT804 = oBook.BookTotalPL.ToString()
    ''            o200Orig.AT8.AT805 = oBook.BookTotalCases.ToString()
    ''            o200Orig.AT8.AT806 = "E"
    ''            o200Orig.AT8.AT807 = oBook.BookTotalCube.ToString()
    ''            intSegments += 1 'increase the segment counter after each segment
    ''            l200s.Add(o200Orig)
    ''            .Loop200 = l200s.ToArray()
    ''            .SE.SE01 = intSegments.ToString()
    ''            .SE.SE02 = intSequence.ToString()
    ''        End With
    ''        Dim strGE As String = buildGEString(intSequence, GSSequence, SegmentTerminator)
    ''        Dim strIEA As String = buildIEAString(1, ISASequence, SegmentTerminator)
    ''        strEDI = String.Concat(strISA, strGS, o214.getEDIString(SegmentTerminator), strGE, strIEA)
    ''        With oEDIInput
    ''            .AdminEmail = osysPar.GlobalAdminEmail
    ''            .FromEmail = osysPar.GlobalFromEmail
    ''            .GroupEmail = osysPar.GlobalGroupEmail
    ''            .Retry = osysPar.GlobalAutoRetry
    ''            .SMTPServer = osysPar.GlobalSMTPServer
    ''            .DBServer = testParameters.DBServer
    ''            .Database = testParameters.Database
    ''            .AuthorizationCode = "NGLSystem"
    ''            enumResults = .ProcessData(strEDI, testParameters.ConnectionString, intCarrierControl)
    ''            'check for any 997s to go out
    ''            str997sOut = .EDI997Response
    ''            strLastError = .LastError
    ''        End With

    ''        If enumResults <> Configuration.ProcessDataReturnValues.nglDataIntegrationComplete Then
    ''            Select Case enumResults
    ''                Case Configuration.ProcessDataReturnValues.nglDataConnectionFailure
    ''                    Assert.Fail(sCaller & " Connection Failure: " & strLastError)
    ''                Case Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
    ''                    Assert.Fail(sCaller & " Integration Failure: " & strLastError)
    ''                Case Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
    ''                    Assert.Fail(sCaller & " Had Errors Failure: " & strLastError)
    ''                Case Configuration.ProcessDataReturnValues.nglDataValidationFailure
    ''                    Assert.Fail(sCaller & " Data Validation Failure: " & strLastError)
    ''                Case Else
    ''                    Assert.Fail(sCaller & " Unexpected: " & strLastError)
    ''            End Select
    ''        End If


    ''    Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
    ''        Throw
    ''    Catch ex As Exception
    ''        Assert.Fail(sCaller & ": Unexpected Error: " & ex.Message)
    ''    Finally

    ''    End Try
    ''End Sub


    ''Private Sub processEDI214Message(ByVal sCaller As String, ByVal strBookProNumber As String, ByVal dtAT7 As Date, ByVal sAT701 As String, ByVal sAT702 As String, ByVal sAT703 As String, ByVal sAT704 As String)
    ''    Try
    ''        Dim oEDIInput As New clsEDIInput

    ''        Dim enumResults As Configuration.ProcessDataReturnValues = Configuration.ProcessDataReturnValues.nglDataIntegrationComplete
    ''        testParameters.WCFAuthCode = "NGLSystem"
    ''        Dim osysPar = getSystemParameters()
    ''        Dim strLastError As String = ""
    ''        Dim str997sOut As String = ""
    ''        Dim strEDI As String
    ''        Dim strEDIXAction As String = "214"
    ''        'get the booking records
    ''        Dim oBook = getBookByPro(strBookProNumber)
    ''        If oBook Is Nothing OrElse oBook.BookControl = 0 Then
    ''            Assert.Fail(sCaller & ": cannot read booking records.")
    ''        End If
    ''        Dim intCarrierControl As Integer = oBook.BookCarrierControl
    ''        Dim oCompEDI As DTO.CompEDI = readCompEDI(oBook.BookCustCompControl, strEDIXAction)
    ''        If oCompEDI Is Nothing OrElse oCompEDI.CompEDIControl = 0 Then
    ''            Assert.Fail(sCaller & ": company is not set up for EDI.")
    ''        End If
    ''        Dim oCarrEDI As DTO.CarrierEDI = readCarrierEDI(oBook.BookCarrierControl, strEDIXAction)
    ''        If oCarrEDI Is Nothing OrElse oCarrEDI.CarrierEDIControl = 0 Then
    ''            Assert.Fail(sCaller & ": carrier is not set up for EDI.")
    ''        End If
    ''        Dim dtSend As Date = Date.Now()
    ''        Dim SegmentTerminator As String
    ''        Dim ISASequence As String
    ''        Dim GSSequence As String
    ''        Dim blnInbound As Boolean = True
    ''        Dim strISA = buildISAString(oCompEDI, oCarrEDI, dtSend, SegmentTerminator, ISASequence, blnInbound)
    ''        Dim strGS = buildGSString(oCompEDI, oCarrEDI, dtSend, strEDIXAction, SegmentTerminator, GSSequence, blnInbound)
    ''        Dim intSequence As Integer = 1
    ''        Dim intSegments As Integer = 0
    ''        Dim o214 As New clsEDI214
    ''        With o214
    ''            .ST.ST01 = "214"
    ''            .ST.ST02 = intSequence.ToString()
    ''            intSegments += 1 'increase the segment counter after each segment
    ''            .B10.B1001 = oBook.BookSHID
    ''            .B10.B1002 = oBook.BookSHID
    ''            .B10.B1003 = oCarrEDI.CarrierEDIPartnerCode
    ''            intSegments += 1 'increase the segment counter after each segment
    ''            .L11.L1101 = "003"
    ''            .L11.L1102 = "19"
    ''            intSegments += 1 'increase the segment counter after each segment
    ''            Dim o100Orig As New clsEDI214Loop100
    ''            o100Orig.N1.N101 = "SF"
    ''            o100Orig.N1.N102 = oBook.BookOrigName
    ''            o100Orig.N1.N103 = "92"
    ''            intSegments += 1 'increase the segment counter after each segment
    ''            o100Orig.N3.N301 = oBook.BookOrigAddress1
    ''            intSegments += 1 'increase the segment counter after each segment
    ''            o100Orig.N4.N401 = oBook.BookOrigCity
    ''            o100Orig.N4.N402 = oBook.BookOrigState
    ''            o100Orig.N4.N403 = oBook.BookOrigZip
    ''            o100Orig.N4.N404 = oBook.BookOrigCountry
    ''            intSegments += 1 'increase the segment counter after each segment
    ''            Dim l100s As New List(Of clsEDI214Loop100)
    ''            l100s.Add(o100Orig)
    ''            Dim o100Dest As New clsEDI214Loop100
    ''            o100Dest.N1.N101 = "ST"
    ''            o100Dest.N1.N102 = oBook.BookDestName
    ''            o100Dest.N1.N103 = "92"
    ''            intSegments += 1 'increase the segment counter after each segment
    ''            o100Dest.N3.N301 = oBook.BookDestAddress1
    ''            intSegments += 1 'increase the segment counter after each segment
    ''            o100Dest.N4.N401 = oBook.BookDestCity
    ''            o100Dest.N4.N402 = oBook.BookDestState
    ''            o100Dest.N4.N403 = oBook.BookDestZip
    ''            o100Dest.N4.N404 = oBook.BookDestCountry
    ''            intSegments += 1 'increase the segment counter after each segment
    ''            l100s.Add(o100Dest)
    ''            o214.Loop100 = l100s.ToArray()
    ''            Dim l200s As New List(Of clsEDI214Loop200)
    ''            Dim o200Orig As New clsEDI214Loop200
    ''            o200Orig.LX.LX01 = 1 'Note: stop numbers must start with stop 1.  Stop 0 is not allowed. Stop 1 = Pickup Location
    ''            intSegments += 1 'increase the segment counter after each segment
    ''            Dim l205s As New List(Of clsEDI214Loop205)
    ''            Dim o205Orig As New clsEDI214Loop205
    ''            o205Orig.AT7.AT701 = sAT701
    ''            o205Orig.AT7.AT702 = sAT702
    ''            o205Orig.AT7.AT703 = sAT703
    ''            o205Orig.AT7.AT704 = sAT704
    ''            o205Orig.AT7.AT705 = dtAT7.ToString("yyyyMMdd")
    ''            o205Orig.AT7.AT706 = dtAT7.ToString("HHmm")
    ''            o205Orig.AT7.AT707 = "LT"
    ''            intSegments += 1 'increase the segment counter after each segment
    ''            o205Orig.MS1.MS101 = oBook.BookOrigCity
    ''            o205Orig.MS1.MS102 = oBook.BookOrigState
    ''            o205Orig.MS1.MS103 = oBook.BookOrigCountry
    ''            intSegments += 1 'increase the segment counter after each segment
    ''            o205Orig.MS2.MS201 = oCarrEDI.CarrierEDIPartnerCode
    ''            o205Orig.MS2.MS202 = "11111" 'equipment number
    ''            o205Orig.MS2.MS203 = "TL" 'FT – Flatbed; TV – Truck, Van; TW – Reefer;TL – Truck Load
    ''            intSegments += 1 'increase the segment counter after each segment
    ''            l205s.Add(o205Orig)
    ''            o200Orig.Loop205 = l205s.ToArray()
    ''            o200Orig.L11.L1101 = oBook.BookCarrOrderNumber & "-" & oBook.BookOrderSequence
    ''            o200Orig.L11.L1102 = "ON"
    ''            intSegments += 1 'increase the segment counter after each segment
    ''            'add any additional L11 segments to the L11s list if needed
    ''            o200Orig.K1.K101 = "EDI 214 Unit Test"
    ''            o200Orig.K1.K102 = " - " & sCaller & " - "
    ''            intSegments += 1 'increase the segment counter after each segment
    ''            o200Orig.AT8.AT801 = "G"
    ''            o200Orig.AT8.AT802 = "L"
    ''            o200Orig.AT8.AT803 = oBook.BookTotalWgt.ToString()
    ''            o200Orig.AT8.AT804 = oBook.BookTotalPL.ToString()
    ''            o200Orig.AT8.AT805 = oBook.BookTotalCases.ToString()
    ''            o200Orig.AT8.AT806 = "E"
    ''            o200Orig.AT8.AT807 = oBook.BookTotalCube.ToString()
    ''            intSegments += 1 'increase the segment counter after each segment
    ''            l200s.Add(o200Orig)
    ''            .Loop200 = l200s.ToArray()
    ''            .SE.SE01 = intSegments.ToString()
    ''            .SE.SE02 = intSequence.ToString()
    ''        End With
    ''        Dim strGE As String = buildGEString(intSequence, GSSequence, SegmentTerminator)
    ''        Dim strIEA As String = buildIEAString(1, ISASequence, SegmentTerminator)
    ''        strEDI = String.Concat(strISA, strGS, o214.getEDIString(SegmentTerminator), strGE, strIEA)
    ''        With oEDIInput
    ''            .AdminEmail = osysPar.GlobalAdminEmail
    ''            .FromEmail = osysPar.GlobalFromEmail
    ''            .GroupEmail = osysPar.GlobalGroupEmail
    ''            .Retry = osysPar.GlobalAutoRetry
    ''            .SMTPServer = osysPar.GlobalSMTPServer
    ''            .DBServer = testParameters.DBServer
    ''            .Database = testParameters.Database
    ''            .AuthorizationCode = "NGLSystem"
    ''            enumResults = .ProcessData(strEDI, testParameters.ConnectionString, intCarrierControl)
    ''            'check for any 997s to go out
    ''            str997sOut = .EDI997Response
    ''            strLastError = .LastError
    ''        End With

    ''        If enumResults <> Configuration.ProcessDataReturnValues.nglDataIntegrationComplete Then
    ''            Select Case enumResults
    ''                Case Configuration.ProcessDataReturnValues.nglDataConnectionFailure
    ''                    Assert.Fail(sCaller & " Connection Failure: " & strLastError)
    ''                Case Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
    ''                    Assert.Fail(sCaller & " Integration Failure: " & strLastError)
    ''                Case Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
    ''                    Assert.Fail(sCaller & " Had Errors Failure: " & strLastError)
    ''                Case Configuration.ProcessDataReturnValues.nglDataValidationFailure
    ''                    Assert.Fail(sCaller & " Data Validation Failure: " & strLastError)
    ''                Case Else
    ''                    Assert.Fail(sCaller & " Unexpected: " & strLastError)
    ''            End Select
    ''        End If


    ''    Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
    ''        Throw
    ''    Catch ex As Exception
    ''        Assert.Fail(sCaller & ": Unexpected Error: " & ex.Message)
    ''    Finally

    ''    End Try
    ''End Sub


    ''''' <summary>
    ''''' Set Assigned Carrier 
    ''''' </summary>
    ''''' <param name="BookProNumber"></param>
    ''''' <param name="dtAT7"></param>
    ''''' <remarks></remarks>
    ''Private Sub SetAssignedCarrier(ByVal BookProNumber As String, ByVal dtAT7 As Date)
    ''    processEDI214CarrierAssignmentMessage("SetAssignedCarrier", BookProNumber, dtAT7, "CNT123654", "Contractor", "SCAC")
    ''End Sub

    ''''' <summary>
    ''''' Set Assigned Carrier 
    ''''' </summary>
    ''''' <param name="BookProNumber"></param>
    ''''' <param name="dtAT7"></param>
    ''''' <remarks></remarks>
    ''Private Sub SetNGLAssignedCarrier(ByVal BookProNumber As String, ByVal dtAT7 As Date)
    ''    processEDI214CarrierAssignmentMessage("SetAssignedCarrier", BookProNumber, dtAT7, "CNT123654", "Veterans", "80")
    ''End Sub

    ''''' <summary>
    ''''' Set pickup appointment for load
    ''''' </summary>
    ''''' <param name="BookProNumber"></param>
    ''''' <param name="dtLoad"></param>
    ''''' <remarks></remarks>
    ''Private Sub SetPickupAppointment(ByVal BookProNumber As String, ByVal dtLoad As Date)
    ''    Dim dtAT7 As Date = dtLoad.AddHours(9) '9 am        
    ''    processEDI214Message("SetPickupAppointment", BookProNumber, dtAT7, "", "NS", "AA", "NA")
    ''End Sub

    ''''' <summary>
    ''''' Set delivery appointment for load
    ''''' </summary>
    ''''' <param name="BookProNumber"></param>
    ''''' <param name="dtRequired"></param>
    ''''' <remarks></remarks>
    ''Private Sub SetDeliveryAppointment(ByVal BookProNumber As String, ByVal dtRequired As Date)
    ''    Dim dtAT7 As Date = dtRequired.AddHours(14) '2 pm
    ''    processEDI214Message("SetDeliveryAppointment", BookProNumber, dtAT7, "", "NS", "AB", "NA")
    ''End Sub

    ''Private Sub SetCarrierArrivedAtPickup(ByVal BookProNumber As String, ByVal dtLoad As Date)
    ''    Dim dtAT7 As Date = dtLoad.AddHours(8) '8 am       
    ''    processEDI214Message("SetCarrierArrivedAtPickup", BookProNumber, dtAT7, "X3", "NS", "", "NA")
    ''End Sub


    ''Private Sub SetCarrierLoadingAtPickup(ByVal BookProNumber As String, ByVal dtLoad As Date)
    ''    Dim dtAT7 As Date = dtLoad.AddHours(9) '9 am       
    ''    processEDI214Message("SetCarrierLoadingAtPickup", BookProNumber, dtAT7, "L1", "NS", "", "NA")
    ''End Sub


    ''Private Sub SetCarrierFinishedLoadingAtPickup(ByVal BookProNumber As String, ByVal dtLoad As Date)
    ''    Dim dtAT7 As Date = dtLoad.AddHours(11) '11 am       
    ''    processEDI214Message("SetCarrierFinishedLoadingAtPickup", BookProNumber, dtAT7, "CP", "NS", "", "NA")
    ''End Sub


    ''Private Sub SetCarrierDepartedPickup(ByVal BookProNumber As String, ByVal dtLoad As Date)
    ''    Dim dtAT7 As Date = dtLoad.AddHours(12) '12 noon       
    ''    processEDI214Message("SetCarrierDepartedPickup", BookProNumber, dtAT7, "AF", "NS", "", "NA")
    ''End Sub

    ''Private Sub SetEnRouteToDelivery(ByVal BookProNumber As String, ByVal dtStamp As Date)
    ''    processEDI214Message("SetEnRouteToDelivery", BookProNumber, dtStamp, "X6", "NS", "", "AJ")
    ''End Sub

    ''Private Sub SetEquipmentBreakDownDelivery(ByVal BookProNumber As String, ByVal dtStamp As Date)
    ''    processEDI214Message("SetEquipmentBreakDownDelivery", BookProNumber, dtStamp, "SD", "AI", "", "")
    ''End Sub

    ''Private Sub SetShipperRelatedDelay(ByVal BookProNumber As String, ByVal dtStamp As Date)
    ''    processEDI214Message("SetShipperRelatedDelay", BookProNumber, dtStamp, "SD", "AM", "", "")
    ''End Sub

    ''Private Sub SetConsigneeRelatedDelay(ByVal BookProNumber As String, ByVal dtStamp As Date)
    ''    processEDI214Message("SetConsigneeRelatedDelay", BookProNumber, dtStamp, "SD", "AG", "", "")
    ''End Sub

    ''Private Sub SetCarrierArrivedAtDelivery(ByVal BookProNumber As String, ByVal dtRequired As Date)
    ''    Dim dtAT7 As Date = dtRequired.AddHours(15) '3 pm
    ''    processEDI214Message("SetCarrierArrivedAtDelivery", BookProNumber, dtAT7, "X1", "NS", "", "NA")
    ''End Sub

    ''Private Sub SetCarrierUnloadingAtDelivery(ByVal BookProNumber As String, ByVal dtRequired As Date)
    ''    Dim dtAT7 As Date = dtRequired.AddHours(16) '4 pm
    ''    processEDI214Message("SetCarrierUnloadingAtDelivery", BookProNumber, dtAT7, "X5", "NS", "", "NA")
    ''End Sub

    ''Private Sub SetCarrierFinishedUnloadingAtDelivery(ByVal BookProNumber As String, ByVal dtRequired As Date)
    ''    Dim dtAT7 As Date = dtRequired.AddHours(18) '6 pm
    ''    processEDI214Message("SetCarrierFinishedUnloadingAtDelivery", BookProNumber, dtAT7, "D1", "NS", "", "NA")
    ''End Sub

    ''Private Sub SetCarrierDepartedDelivery(ByVal BookProNumber As String, ByVal dtRequired As Date)
    ''    Dim dtAT7 As Date = dtRequired.AddHours(19) '7 pm
    ''    processEDI214Message("SetCarrierDepartedDelivery", BookProNumber, dtAT7, "CD", "NS", "", "NA")
    ''End Sub


    ''Private Sub CreateTestOrder(ByVal BookProPrefix As String,
    ''                            ByVal BookProBase As String,
    ''                            ByVal BookConsPrefix As String,
    ''                            ByVal OrderNumber As String,
    ''                            ByVal OrderDate As Date)


    ''    Dim oBook As New DAL.NGLBookData(testParameters)
    ''    Dim dtOrderd = OrderDate
    ''    Dim dtLoad As Date = OrderDate.AddDays(3)
    ''    Dim dtRequired As Date = dtLoad.AddDays(5)
    ''    Dim sbQry As New System.Text.StringBuilder()
    ''    sbQry.Append(String.Format("Declare @BookProBase nvarchar(20) = '{0}' {1}", BookProBase, vbCrLf))
    ''    sbQry.Append(String.Format("Declare @BookProPrefix nvarchar(3) = '{0}' {1}", BookProPrefix, vbCrLf))
    ''    sbQry.Append(String.Format("Declare @BookConsPrefix nvarchar(20) = '{0}' {1}", BookConsPrefix, vbCrLf))
    ''    sbQry.Append(String.Format("Declare @BookProNumber nvarchar(20) = @BookProPrefix + @BookProBase {0}", vbCrLf))
    ''    sbQry.Append(String.Format("Declare @BookCarrOrderNumber nvarchar(20) = '{0}' {1}", OrderNumber, vbCrLf))
    ''    sbQry.Append(String.Format("Delete from dbo.book where BookProNumber = @BookProNumber {0}", vbCrLf))

    ''    sbQry.Append(String.Format("INSERT INTO [dbo].[Book] ([BookProNumber], [BookProBase], [BookConsPrefix], [BookCustCompControl], [BookCommCompControl], [BookODControl], [BookCarrierControl], [BookCarrierContact], [BookCarrierContactPhone], [BookOrigCompControl], [BookOrigName], [BookOrigAddress1], [BookOrigAddress2], [BookOrigAddress3], [BookOrigCity], [BookOrigState], [BookOrigCountry], [BookOrigZip], [BookOrigPhone], [BookOrigFax], [BookOriginStartHrs], [BookOriginStopHrs], [BookOriginApptReq], [BookDestCompControl], [BookDestName], [BookDestAddress1], [BookDestAddress2], [BookDestAddress3], [BookDestCity], [BookDestState], [BookDestCountry], [BookDestZip], [BookDestPhone], [BookDestFax], [BookDestStartHrs], [BookDestStopHrs], [BookDestApptReq], [BookDateOrdered], [BookDateLoad], [BookDateInvoice], [BookDateRequired], [BookDateDelivered], [BookTotalCases], [BookTotalWgt], [BookTotalPL], [BookTotalCube], [BookTotalPX], [BookTotalBFC], [BookTranCode], [BookPayCode], [BookTypeCode], [BookBOLCode], [BookStopNo], [BookModDate], [BookModUser], [BookCarrControl], [BookCarrFBNumber], [BookCarrOrderNumber], [BookCarrBLNumber], [BookCarrBookDate], [BookCarrBookTime], [BookCarrBookContact], [BookCarrScheduleDate], [BookCarrScheduleTime], [BookCarrActualDate], [BookCarrActualTime], [BookCarrActLoadComplete Date], [BookCarrActLoadCompleteTime], [BookCarrDockPUAssigment], [BookCarrPODate], [BookCarrPOTime], [BookCarrApptDate], [BookCarrApptTime], [BookCarrActDate], [BookCarrActTime], [BookCarrActUnloadCompDate], [BookCarrActUnloadCompTime], [BookCarrDockDelAssignment], [BookCarrVarDay], [BookCarrVarHrs], [BookCarrTrailerNo], [BookCarrSealNo], [BookCarrDriverNo], [BookCarrDriverName], [BookCarrRouteNo], [BookCarrTripNo], [BookFinControl], [BookFinARBookFrt], [BookFinARInvoiceDate], [BookFinARInvoiceAmt], [BookFinARPayDate], [BookFinARPayAmt], [BookFinARCheck], [BookFinARGLNumber], [BookFinARBalance], [BookFinARCurType], [BookFinAPBillNumber], [BookFinAPBillNoDate], [BookFinAPBillInvDate], [BookFinAPActWgt], [BookFinAPStdCost], [BookFinAPActCost], [BookFinAPPayDate], [BookFinAPPayAmt], [BookFinAPCheck], [BookFinAPGLNumber], [BookFinAPLastViewed], [BookFinAPCurType], [BookFinCommStd], [BookFinCommAct], [BookFinCommPayDate], [BookFinCommPayAmt], [BookFinCommtCheck], [BookFinCommCreditAmt], [BookFinCommCreditPayDate], [BookFinCommLoadCount], [BookFinCommGLNumber], [BookFinCheckClearedDate], [BookFinCheckClearedNumber], [BookFinCheckClearedAmt], [BookFinCheckClearedDesc], [BookFinCheckClearedAcct], [BookRevControl], [BookRevBilledBFC], [BookRevCarrierCost], [BookRevStopQty], [BookRevStopCost], [BookRevOtherCost], [BookRevTotalCost], [BookRevLoadSavings], [BookRevCommPercent], [BookRevCommCost], [BookRevGrossRevenue], [BookRevNegRevenue], [BookMilesFrom], [BookLaneCarrControl], [BookHoldLoad], [BookRouteFinalDate], [BookRouteFinalCode], [BookRouteFinalFlag], [BookWarehouseNumber], [BookComCode], [BookTransType], [BookRouteConsFlag], [BookWhseAuthorizationNo], [BookHotLoad], [BookFinAPActTax], [BookFinAPExportFlag], [BookFinARFreightTax], [BookRevFreightTax], [BookRevNetCost], [BookFinServiceFee], [BookFinAPExportDate], [BookFinAPExportRetry], [BookCarrierContControl], [BookHotLoadSent], [BookExportDocCreated], [BookDoNotInvoice], [BookCarrStartLoadingDate], [BookCarrStartLoadingTime], [BookCarrFinishLoadingDate], [BookCarrFinishLoadingTime], [BookCarrStartUnloadingDate], [BookCarrStartUnloadingTime], [BookCarrFinishUnloadingDate], [BookCarrFinishUnloadingTime], [BookOrderSequence], [BookChepGLID], [BookCarrierTypeCode], [BookPalletPositions], [BookShipCarrierProNumber], [BookShipCarrierProNumberRaw], [BookShipCarrierProControl], [BookShipCarrierName], [BookShipCarrierNumber], [BookAPAdjReasonControl], [BookDateRequested], [BookCarrierEquipmentCodes], [BookShippedDataExported], [BookLockAllCosts], [BookLockBFCCost], [BookRevNonTaxable], [BookPickupStopNumber], [BookOrigStopNumber], [BookDestStopNumber], [BookOrigMiles], [BookDestMiles], [BookOrigPCMCost], [BookDestPCMCost], [BookOrigPCMTime], [BookDestPCMTime], [BookOrigPCMTolls], [BookDestPCMTolls], [BookOrigPCMESTCHG], [BookDestPCMESTCHG], [BookPickNumber], [BookAMSPickupApptControl], [BookAMSDeliveryApptControl], [BookItemDetailDescription], [BookOrigStopControl], [BookDestStopControl], [BookRouteTypeCode], [BookAlternateAddressLaneControl], [BookAlternateAddressLaneNumber], [BookDefaultRouteSequence], [BookRouteGuideControl], [BookRouteGuideNumber], [BookCustomerApprovalTransmitted], [BookCustomerApprovalRecieved], [BookCarrTruckControl], [BookCarrTarControl], [BookCarrTarRevisionNumber], [BookCarrTarName], [BookCarrTarEquipControl], [BookCarrTarEquipName], [BookCarrTarEquipMatControl], [BookCarrTarEquipMatName], [BookCarrTarEquipMatDetControl], [BookCarrTarEquipMatDetID], [BookCarrTarEquipMatDetValue], [BookBookRevHistRevision], [BookModeTypeControl], [BookAllowInterlinePoints], [BookRevLaneBenchMiles], [BookRevLoadMiles], [BookUser1], [BookUser2], [BookUser3], [BookUser4], [BookRevDiscount], [BookRevLineHaul], [BookMustLeaveByDateTime], [BookMultiMode], [BookOriginalLaneControl], [BookLaneTranXControl], [BookLaneTranXDetControl], [BookSHID], [BookShipCarrierDetails], [BookExpDelDateTime], [BookOutOfRouteMiles], [BookSpotRateAllocationFormula], [BookSpotRateAutoCalcBFC], [BookSpotRateUseCarrierFuelAddendum], [BookSpotRateBFCAllocationFormula], [BookSpotRateTotalUnallocatedBFC], [BookSpotRateTotalUnallocatedLineHaul], [BookSpotRateUseFuelAddendum], [BookCreditHold], [BookBestDeficitCost], [BookBestDeficitWeight], [BookBestDeficitWeightBreak], [BookRatedWeightBreak], [BookWgtAdjCost], [BookWgtAdjWeight], [BookWgtAdjWeightBreak], [BookBilledLoadWeight], [BookMinAdjustedLoadWeight], [BookSummedClassWeight], [BookWgtRoundingVariance], [BookHeaviestClass], [BookAcutalHeaviestClassWeight], [BookRevDiscountRate], [BookRevDiscountMin]) VALUES (@BookProNumber, @BookProBase,@BookConsPrefix, 9623, 9623, 56600, 17242, N'TRUCKLOAD BENCHMARK', N'', 9623, N'Vegetable Juices, Inc.', N'7400 South Narragansett Avenue', N'', N'', N'Bedford Park', N'IL', N'US', N'60638', N'', N'', NULL, NULL, 1, 0, N'Create A Pack Foods', N'W1344 Industrial Dr', N'', N'', N'Ixonia', N'WI', N'US', N'53036', N'', N'', NULL, NULL, 1, N'{1}', N'{2}', NULL, N'{3}', NULL, 8, 8000, 8, 800, 0, CAST(0.0000 AS Money), N'PB', N'N', N'N/A', 0, 1, GetDate(), N'UnitTest', NULL, N'', @BookCarrOrderNumber, N'', NULL, NULL, N'', NULL, NULL, NULL, NULL, NULL, NULL, N'', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'', 0, 0, N'', N'', N'', N'', N'', N'', NULL, CAST(0.0000 AS Money), NULL, CAST(0.0000 AS Money), NULL, CAST(0.0000 AS Money), N'', N'', CAST(0.0000 AS Money), 1, N'', NULL, NULL, 0, CAST(472.8400 AS Money), CAST(0.0000 AS Money), NULL, CAST(0.0000 AS Money), N'', N'', NULL, 1, CAST(-472.8400 AS Money), CAST(0.0000 AS Money), NULL, CAST(0.0000 AS Money), N'', CAST(0.0000 AS Money), NULL, 0, N'', NULL, N'', CAST(0.0000 AS Money), N'', N'', NULL, CAST(0.0000 AS Money), CAST(242.9000 AS Money), 0, CAST(0.0000 AS Money), CAST(229.9400 AS Money), CAST(472.8400 AS Money), CAST(-472.8400 AS Money), 100, CAST(-472.8400 AS Money), CAST(0.0000 AS Money), 1, 138.8, 0, 1, NULL, N'', 0, N'', N'', N'0', 1, N'', 0, CAST(0.0000 AS Money), 0, CAST(0.0000 AS Money), CAST(0.0000 AS Money), CAST(472.8400 AS Money), CAST(0.0000 AS Money), NULL, 0, 21934, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, N'', N'', N'', N'', N'', NULL, N'', N'', 0, N'{3}', N'', 0, 0, 0, CAST(0.0000 AS Money), 0, 0, 0, 0, 0, 0, 0, NULL, NULL, CAST(0.0000 AS Money), CAST(0.0000 AS Money), 0, 0, 0, 0, 0, N'', 0, 0, 6, 0, N'', 0, 0, N'', 0, 0, 0, 880, 1, N'', 1419, N'', 104913, N'', 0, 1, CAST(1.7500 AS Decimal(18, 4)), 1, 3, 1, 138.8, 138.8, N'', N'', N'', N'', CAST(0.0000 AS Decimal(18, 4)), CAST(242.9000 AS Decimal(18, 4)), N'{3}', 0, 56600, 0, 0, @BookConsPrefix, NULL, N'{3}', 0, 0, 0, 0, 0, CAST(0.0000 AS Decimal(18, 4)), CAST(0.0000 AS Decimal(18, 4)), 0, 0, CAST(0.0000 AS Decimal(18, 4)), 0, 0, 0, CAST(0.0000 AS Decimal(18, 4)), 0, 0, 0, 0, 0, 0, NULL, 0, CAST(0.0000 AS Decimal(18, 4)), CAST(0.0000 AS Decimal(18, 4))) {0}", vbCrLf, dtOrderd.ToShortDateString(), dtLoad.ToShortDateString(), dtRequired.ToShortDateString()))
    ''    sbQry.Append(String.Format("--Get the bookcontrol number back {0}", vbCrLf))
    ''    sbQry.Append(String.Format("Declare @BookControl int = SCOPE_IDENTITY() {0}", vbCrLf))
    ''    sbQry.Append(String.Format("INSERT INTO [dbo].[BookLoad] ([BookLoadBookControl], [BookLoadBuy], [BookLoadPONumber], [BookLoadVendor], [BookLoadCaseQty], [BookLoadWgt], [BookLoadCube], [BookLoadPL], [BookLoadPX], [BookLoadPType], [BookLoadCom], [BookLoadPUOrigin], [BookLoadBFC], [BookLoadTotCost], [BookLoadComments], [BookLoadStopSeq], [BookLoadModDate], [BookLoadModUser]) VALUES (@BookControl, NULL, N'SO-TEST1118', N'Vegetable Juices, Inc.', 8, 8000, 800, 8, 0, N'N', N'D', N'Bedford Park', CAST(0.0000 AS Money), CAST(472.8400 AS Money), NULL, 1,getdate(), N'UnitTest') {0}", vbCrLf))
    ''    sbQry.Append(String.Format("--Get the bookloadcontrol number back {0}", vbCrLf))
    ''    sbQry.Append(String.Format("Declare @BookLoadControl int = SCOPE_IDENTITY()  {0}", vbCrLf))
    ''    sbQry.Append(String.Format("INSERT INTO [dbo].[BookItem] ([BookItemBookLoadControl], [BookItemFixOffInvAllow], [BookItemFixFrtAllow], [BookItemItemNumber], [BookItemQtyOrdered], [BookItemFreightCost], [BookItemItemCost], [BookItemWeight], [BookItemCube], [BookItemPack], [BookItemSize], [BookItemDescription], [BookItemHazmat], [BookItemModDate], [BookItemModUser], [BookItemBrand], [BookItemCostCenter], [BookItemLotNumber], [BookItemLotExpirationDate], [BookItemGTIN], [BookCustItemNumber], [BookItemBFC], [BookItemCountryOfOrigin], [BookItemHST], [BookItemPalletTypeID], [BookItemHazmatTypeCode], [BookItem49CFRCode], [BookItemIATACode], [BookItemDOTCode], [BookItemMarineCode], [BookItemNMFCClass], [BookItemFAKClass], [BookItemLimitedQtyFlag], [BookItemPallets], [BookItemTies], [BookItemHighs], [BookItemQtyPalletPercentage], [BookItemQtyLength], [BookItemQtyWidth], [BookItemQtyHeight], [BookItemStackable], [BookItemLevelOfDensity], [BookItemDiscount], [BookItemLineHaul], [BookItemTaxableFees], [BookItemTaxes], [BookItemNonTaxableFees], [BookItemDeficitCostAdjustment], [BookItemDeficitWeightAdjustment], [BookItemWeightBreak], [BookItemDeficit49CFRCode], [BookItemDeficitIATACode], [BookItemDeficitDOTCode], [BookItemDeficitMarineCode], [BookItemDeficitNMFCClass], [BookItemDeficitFAKClass], [BookItemRated49CFRCode], [BookItemRatedIATACode], [BookItemRatedDOTCode], [BookItemRatedMarineCode], [BookItemRatedNMFCClass], [BookItemRatedFAKClass], [BookItemCarrTarEquipMatControl], [BookItemCarrTarEquipMatName], [BookItemCarrTarEquipMatDetID], [BookItemCarrTarEquipMatDetValue], [BookItemUser1], [BookItemUser2], [BookItemUser3], [BookItemUser4], [BookItemUnitOfMeasureControl], [BookItemRatedNMFCSubClass], [BookItemCommCode]) VALUES ( @BookLoadControl, NULL, NULL, N'1234', 8, CAST(472.8400 AS Money), CAST(100.0000 AS Money), 8000, 800, NULL, NULL, N'Test Item', NULL, getDate(), N'UnitTest',  NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, CAST(0.0000 AS Decimal(18, 4)), CAST(0.0000 AS Decimal(18, 4)), CAST(0.0000 AS Decimal(18, 4)), CAST(0.0000 AS Decimal(18, 4)), CAST(0.0000 AS Decimal(18, 4)), CAST(0.0000 AS Decimal(18, 4)), CAST(0.0000 AS Decimal(18, 4)), CAST(0.0000 AS Decimal(18, 4)), NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, 0, CAST(0.0000 AS Decimal(18, 4)), NULL, NULL, NULL, NULL, 0, NULL, N'3') {0}", vbCrLf))
    ''    oBook.executeSQL(sbQry.ToString())
    ''    'System.Diagnostics.Debug.WriteLine("*********************************************************************************************")
    ''    'System.Diagnostics.Debug.Write(sbQry.ToString())
    ''    'System.Diagnostics.Debug.WriteLine("*********************************************************************************************")

    ''End Sub

    ''<TestMethod()>
    ''Public Sub TestParseEDI204In()
    ''    Dim oEDIInput As New Ngl.FreightMaster.Integration.clsEDIInput
    ''    With oEDIInput
    ''        .DBServer = "MININT-1CJQH8R\SQL2014"
    ''        .Database = "NGLMASDEV7051"
    ''    End With

    ''    Dim testParams As DAL.WCFParameters = New DAL.WCFParameters() With {.Database = "NGLMASDEV7051",
    ''                                                           .DBServer = "MININT-1CJQH8R\SQL2014",
    ''                                                           .WCFAuthCode = "WCFDEV",
    ''                                                            .UserName = "NGL\Lauren Van Vleet"}
    ''    Dim oSysData As New DAL.NGLSystemDataProvider(testParams)
    ''    testParams.ConnectionString = oSysData.ConnectionString

    ''    'Dim strEDI = "ISA*00*          *00*          *02*BUDT           *12*8479630007     *140717*1643*U*00401*000000040*1*T*>~GS*SM*BUDT*8479630007*20140717*1643*1*X*004010~ST*204*123456789~B2**NGLT**7367849*******FRT~B2A*00~L11*OrderNo*ON~L11*PONum*PO~NTE*DEL*Delivery Comments~NTE*LOI*Load Comments~PLD*24~N1*SF***SFAplhaCode~N3*205 SOUTH AIRPORT DR*SFAddr2~N4*MONTEZUMA*GA*31063*USA~N1*ST*WOLCOTT COLD STORAGE**STAlphaCode~N3*6051 WEST AVE*STAddr2~N4*WOLCOTT*NY*14590*USA~N7*1*Frozen -10*********3~S5*1*CL*41932*L*100*CA*300*E~L11*OrderNo*ON~L11*PONum*PO~G62*02*20170502*Z*1430*LT~G62*10*20170502*Y*1430*LT~G62*69*20170502*U*1430*LT~PLD*24~NTE*LOI*CO 992537~NTE*LOI*TRAILER MUST BE SEALED AND SEAL NUMBER ON ALL COPIES OF BOL~N1*SF***SFAplhaCode~N3*205 SOUTH AIRPORT DR*SFAddr2~N4*MONTEZUMA*GA*31063*USA~G61**SFContName*TE*6148250007~G61**SFContName*EX*1234~G61**SFContName*FX*6148250008~L5*1*Desc1*F**123*15025*12~AT8*G*L*20966*12*50*E*150~S5*2*CU*41932*L*100*CA*300*E~L11*OrderNo*ON~L11*PONum*PO~G62*02*20170502*Z*1430*LT~G62*10*20170502*Y*1430*LT~G62*70*20170503*X*1130*LT~PLD*24~NTE*DEL*CO 992537~NTE*DEL*CONTACT JEFF AT 555-555-5555 FOR DELIVERY APPT~N1*ST*WOLCOTT COLD STORAGE**STAlphaCode~N3*6051 WEST AVE*STAddr2~N4*WOLCOTT*NY*14590*USA~G61**STContName*TE*6148250009~G61**STContName*EX*12345~G61**STContName*FX*6148250010~L5*2*Desc2*F**123*50075*12~AT8*G*L*20966*12*50*E*150~L3*41932*G**LB*651****300*E*100*L~SE*50*123456789~GE*1*1~IEA*1*000000040~"
    ''    Dim strEDI = "ISA*00*          *00*          *02*BUDT           *12*8479630007     *140717*1643*U*00401*000000040*1*T*>~GS*SM*BUDT*8479630007*20140717*1643*1*X*004010~ST*204*123456789~B2**NGLT**7367849*******FRT~B2A*00~L11*OrderNo*ON~L11*PONum*PO~NTE*DEL*Delivery Comments~NTE*LOI*Load Comments~PLD*24~N1*SF***31~N3*7400 South Narragansett Avenue*SFAddr2~N4*Bedford Park*IL*60638*US~N1*ST***18~N3*3301 West Canal Street*STAddr2~N4*Milwaukee*WI*53208*US~N7*1*Frozen -10*********3~S5*1*CL*41932*L*100*CA*300*E~L11*OrderNo*ON~L11*PONum*PO~G62*02*20170502*Z*1430*LT~G62*10*20170502*Y*1430*LT~G62*69*20170502*U*1430*LT~PLD*24~NTE*LOI*CO 992537~NTE*LOI*TRAILER MUST BE SEALED AND SEAL NUMBER ON ALL COPIES OF BOL~N1*SF***31~N3*7400 South Narragansett Avenue*SFAddr2~N4*Bedford Park*IL*60638*US~G61**SFContName*TE*6148250007~G61**SFContName*EX*1234~G61**SFContName*FX*6148250008~L5*1*Desc1*F**123*15025*12~AT8*G*L*20966*12*50*E*150~S5*2*CU*41932*L*100*CA*300*E~L11*OrderNo*ON~L11*PONum*PO~G62*02*20170502*Z*1430*LT~G62*10*20170502*Y*1430*LT~G62*70*20170503*X*1130*LT~PLD*24~NTE*DEL*CO 992537~NTE*DEL*CONTACT JEFF AT 555-555-5555 FOR DELIVERY APPT~N1*ST***18~N3*3301 West Canal Street*STAddr2~N4*Milwaukee*WI*53208*US~G61**STContName*TE*6148250009~G61**STContName*EX*12345~G61**STContName*FX*6148250010~L5*2*Desc2*F**123*50075*12~AT8*G*L*20966*12*50*E*150~L3*41932*G**LB*651****300*E*100*L~SE*50*123456789~GE*1*1~IEA*1*000000040~"
    ''    oEDIInput.ProcessData(strEDI, testParams.ConnectionString, 9613, "filename", Date.Now)

    ''End Sub



    <TestMethod()>
    Public Sub TestParseEDI204In()
        'Dim oEDIInput As New clsEDIInput
        'With oEDIInput
        '    .DBServer = "NGLRDP07D"
        '    .Database = "NGLMASPROD"
        'End With

        'Dim testParams As DAL.WCFParameters = New DAL.WCFParameters() With {.Database = "NGLMASPROD",
        '                                                       .DBServer = "NGLRDP07D",
        '                                                       .WCFAuthCode = "WCFDEV",
        '                                                        .UserName = "NGL\Lauren Van Vleet"}
        'Dim oSysData As New DAL.NGLSystemDataProvider(testParams)
        'testParams.ConnectionString = oSysData.ConnectionString

        'Dim strLogMsg As String = ""

        'Dim strEDI = "ISA*00*          *00*          *02*NGLT           *02*8479630007     *190619*1315*U*00401*000000001*0*T*:~GS*QM*NGLT*8479630007*20190619*1315*1*X*004010~ST*214*0001~B10**CNS-2-1363*NGLT~L11*003*19~N1*ST*Blue Warehouse*ZZ*01*  *00~N3*211 W Farms Mall~N4*FARMINGTON*CT*06032*US~LX*1~AT7*X3*AM*  *  *20190619*1315*LT~MS1*FARMINGTON*CT*US~MS2*TEST*123456789*TL*1~L11*LVV214Testz-0*ON~K1*Pickup Test Msg LVV*Single Order SHID Test~AT8*G*L*5000*5*50*E*4800~SE*104*0010~GE*1*1~IEA*1*000000000~ISA*00*          *00*          *02*NGLT           *02*8479630007     *190619*1316*U*00401*000000001*0*T*:~GS*QM*NGLT*8479630007*20190619*1316*1*X*004010~ST*214*0001~B10**CNS-2-1363*NGLT~L11*003*19~N1*ST*Blue Warehouse*ZZ*01*  *00~N3*211 W Farms Mall~N4*FARMINGTON*CT*06032*US~LX*1~AT7*X1*AM*  *  *20190619*1316*LT~MS1*FARMINGTON*CT*US~MS2*TEST*123456789*TL*1~L11*LVV214Testz-0*ON~K1*Delivery Test MSg LVV*Single Order SHID Test~AT8*G*L*5000*5*50*E*4800~SE*104*0010~GE*1*1~IEA*1*000000000~"
        ''Dim strEDI = "ISA*00*          *00*          *02*NGLT           *02*8479630007     *190619*1315*U*00401*000000001*0*T*:~GS*QM*NGLT*8479630007*20190619*1315*1*X*004010~ST*214*0001~B10**CNS-2-1360*NGLT~L11*003*19~N1*ST*Blue Warehouse*ZZ*01*  *00~N3*211 W Farms Mall~N4*FARMINGTON*CT*06032*US~LX*1~AT7*X3*AM*  *  *20190619*1315*LT~MS1*FARMINGTON*CT*US~MS2*TEST*123456789*TL*1~L11*Yellow-Bluez 1-0*ON~K1*Pickup Test Msg LVV*Multi Order SHID Test~AT8*G*L*5000*5*50*E*4800~SE*104*0010~GE*1*1~IEA*1*000000000~ISA*00*          *00*          *02*NGLT           *02*8479630007     *190619*1316*U*00401*000000001*0*T*:~GS*QM*NGLT*8479630007*20190619*1316*1*X*004010~ST*214*0001~B10**CNS-2-1360*NGLT~L11*003*19~N1*ST*Blue Warehouse*ZZ*01*  *00~N3*211 W Farms Mall~N4*FARMINGTON*CT*06032*US~LX*1~AT7*X1*AM*  *  *20190619*1316*LT~MS1*FARMINGTON*CT*US~MS2*TEST*123456789*TL*1~L11*Yellow-Bluez 1-0*ON~K1*Delivery Test MSg LVV*Multi Order SHID Test~AT8*G*L*5000*5*50*E*4800~SE*104*0010~GE*1*1~IEA*1*000000000~"

        ''Dim strEDI = "ISA*00*          *00*          *02*NGLT           *02*8479630007     *190619*1315*U*00401*000000001*0*T*:~GS*QM*NGLT*8479630007*20190619*1315*1*X*004010~ST*214*0001~B10**CNS-2-1360*NGLT~L11*003*19~N1*ST*Blue Warehouse*ZZ*01*  *00~N3*211 W Farms Mall~N4*FARMINGTON*CT*06032*US~LX*1~AT7*X3*AM*  *  *20190619*1315*LT~MS1*FARMINGTON*CT*US~MS2*TEST*123456789*TL*1~L11*Yellow-Blue 1-0*ON~K1*Pickup Test Msg LVV*Next Pickup Msg LVV~AT8*G*L*5000*5*50*E*4800~SE*104*0010~GE*1*1~IEA*1*000000000~ISA*00*          *00*          *02*NGLT           *02*8479630007     *190619*1316*U*00401*000000001*0*T*:~GS*QM*NGLT*8479630007*20190619*1316*1*X*004010~ST*214*0001~B10**CNS-2-1360*NGLT~L11*003*19~N1*ST*Blue Warehouse*ZZ*01*  *00~N3*211 W Farms Mall~N4*FARMINGTON*CT*06032*US~LX*1~AT7*X1*AM*  *  *20190619*1316*LT~MS1*FARMINGTON*CT*US~MS2*TEST*123456789*TL*1~L11*Yellow-Blue 1-0*ON~K1*Delivery Test MSg LVV*Next Delivery Msg LVV~AT8*G*L*5000*5*50*E*4800~SE*104*0010~GE*1*1~IEA*1*000000000~"
        ''Dim strEDI = "ISA*00*          *00*          *02*NGLT           *02*8479630007     *190618*1730*U*00401*000000001*0*T*:~GS*QM*NGLT*8479630007*20190618*1730*1*X*004010~ST*214*0001~B10**CNS-2-1360*NGLT~L11*003*19~N1*ST*Blue Warehouse*ZZ*01*  *00~N3*211 W Farms Mall~N4*FARMINGTON*CT*06032*US~LX*1~AT7*X1*AM*  *  *20190618*1730*LT~MS1*FARMINGTON*CT*US~MS2*TEST*123456789*TL*1~AT7*X3*AM*  *  *20190618*1730*LT~MS1*FARMINGTON*CT*US~MS2*TEST*123456789*TL*1~L11*Yellow-Blue 1-0*ON~K1*LVV TEST Msg 1*LVV TEST Msg 2~AT8*G*L*5000*5*50*E*4800~SE*134*0010~GE*1*1~IEA*1*000000000~"
        ''Dim strEDI = "ISA*00*          *00*          *02*NGLT           *02*8479630007     *190618*1730*U*00401*000000001*0*T*:~GS*QM*NGLT*8479630007*20190618*1730*1*X*004010~ST*214*0001~B10**CNS-2-1360*NGLT~L11*003*19~N1*ST*Blue Warehouse*ZZ*01*  *00~N3*211 W Farms Mall~N4*FARMINGTON*CT*06032*US~LX*1~AT7*X1*AM*  *  *20190618*1730*LT~MS1*FARMINGTON*CT*US~MS2*TEST*123456789*TL*1~AT7*X3*AM*  *  *20190618*1730*LT~MS1*FARMINGTON*CT*US~MS2*TEST*123456789*TL*1~L11*Yellow-Blue 1-0*ON~K1*Status Message 1*Status Message 2~AT8*G*L*5000*5*50*E*4800~SE*134*0010~GE*1*1~IEA*1*000000000~"
        ''Dim strEDI = "ISA*00*          *00*          *02*BUDT           *12*8479630007     *140827*1606*U*00401*000011252*0*T*>~GS*QM*BUDT*8479630007*20140827*1255*1*X*004010~ST*214*000011252~B10*VJI755907*CNS106559*BUDT~L11*003*19~N1*SF*VEGETABLE JUICE INC~N3*7400 S. NARRAGANSETT~N4*BEDFORD PARK*IL*60638~N1*ST*CONAGRA FOODS INC. FIN. SVC. CENTER~N3*1609 STONE RIDGE DRIVE~N4*STONE MOUNTAIN*GA* 30083~LX*1~AT7*X3*NS***20140827*1430~MS1*BEDFORD PARK*IL*US~MS2*BUDT*53430*TL~L11*1*QN~L11*EDITestM-2*ON~AT8*G*L*1855*19*3*E*0~SE*18*000011252~"
        'oEDIInput.TestparseEDI214(strEDI, strLogMsg)

    End Sub

    <TestMethod()>
    Public Sub parseEDITest()
        Dim oEDIInput As New clsEDIInput
        With oEDIInput
            .DBServer = "DESKTOP-0R0EJUB" ' "NGLRDP07D"
            .Database = "NGLMASTBS" ' "NGLMASPROD"
            .AdminEmail = "rramsey@nextgeneration.com"
            .FromEmail = "rramsey@nextgeneration.com"
            .GroupEmail = "rramsey@nextgeneration.com"
            .Source = "parseEDITest"
            .LogFile = "C:\Data\TMSLog.txt"
            .ConnectionString = "Server=" & .DBServer & ";User ID=nglweb;Password=5529;Database=" & .Database
        End With
        Dim testParams As DAL.WCFParameters = New DAL.WCFParameters() With {.Database = oEDIInput.Database,
                                                               .DBServer = oEDIInput.DBServer,
                                                               .WCFAuthCode = "NGLSystem",
                                                                .UserName = "TEST"}
        Dim oSysData As New DAL.NGLSystemDataProvider(testParams)
        testParams.ConnectionString = oSysData.ConnectionString

        Dim strResults As String = "Fail"

        'Dim strEDI = "ISA*00*          *00*          *02*NGLT           *02*8479630007     *191125*1329*U*00401*000000001*0*T*:~GS*QM*NGLT*8479630007*20191125*1329*1*X*004010~ST*214*0001~B10**CNS-1-1875*NGLT~L11*003*19~N1*ST*TRADER JOE'S (NAZARETH-PA)*ZZ*01*  *00~N3*635 Silver Crest Road~N4*Nazareth*PA*18064*US~LX*1~AT7*X6*NS*  *  *20191125*1329*LT~MS1*Pittsburgh*PA*US~MS2*NGLT*TruckLVV*TL*1~L11*SO-Samp-91114*ON~K1*In Pittsburgh*Go Steelers~AT8*G*L*4000*4*400*E*3000~SE*24*0002~GE*1*1~IEA*1*000000000~"
        'Dim strEDI = "ISA*00*          *00*          *02*NGLT           *02*8479630007     *191125*1329*U*00401*000000001*0*T*:~GS*QM*NGLT*8479630007*20191125*1427*1*X*004010~ST*214*0001~B10**CNS-1-1875*NGLT~L11*003*19~N1*ST*TRADER JOE'S (NAZARETH-PA)*ZZ*01*  *00~N3*635 Silver Crest Road~N4*Nazareth*PA*18064*US~LX*1~AT7*X6*NS*  *  *20191125*1427*LT~MS1*Carlisle*PA*US~MS2*NGLT*TruckLVV1*TL*1~L11*SO-Samp-91114*ON~K1*In Carlisle*Its cold~AT8*G*L*4000*4*400*E*3000~SE*24*0002~GE*1*1~IEA*1*000000000~"
        'Dim strEDI = "ISA*00*          *00*          *02*NGLT           *02*8479630007     *191125*1500*U*00401*000000001*0*T*:~GS*QM*NGLT*8479630007*20191125*1500*1*X*004010~ST*214*0001~B10**CNS-1-1875*NGLT~L11*003*19~N1*ST*TRADER JOE'S (NAZARETH-PA)*ZZ*01*  *00~N3*635 Silver Crest Road~N4*Nazareth*PA*18064*US~LX*1~AT7*X6*NS*  *  *20191125*1500*LT~MS1*Altoona*PA*US~MS2*NGLT*TruckLVV*TL*1~L11*SO-Samp-91114*ON~K1*In Altoona*Testing a thing~AT8*G*L*4000*4*400*E*3000~SE*24*0002~GE*1*1~IEA*1*000000000~"

        'Dim strEDI = "ISA*00*          *00*          *02*NGLT           *02*8479630007     *191010*1228*U*00401*000000001*0*T*:~GS*QM*NGLT*8479630007*20191010*1228*1*X*004010~ST*214*0001~B10**CNS-2-1369*NGLT~L11*003*19~N1*ST*Blue Warehouse*ZZ*01*  *00~N3*211 W Farms Mall~N4*FARMINGTON*CT*06032*US~LX*1~AT7*X6*AM*  *  *20191010*1228*LT~MS1*FARMINGTON*CT*US~MS2*TEST*123456789*TL*1~L11*EDI997Test-0*ON~K1*Status Message 1*Status Message 2~AT8*G*L*15000*15*150*E*3000~SE*24*0002~GE*1*1~IEA*1*000000000~"
        Dim strEDI = "ISA*00*          *00*          *02*NMTF           *12*6083273106     *191227*0736*U*00401*000000007*0*P*:~GS*QM*NMTF*6083273106*20191227*0736*00007*X*004010~ST*214*000070001~B10*107601046*107601046*NMTF~L11**BM~N1*SF*TRACHTE BUILDING SYSTEMS INC~N3*314 WILBURN RD~N4*SUN PRAIRIE*WI*53590~N1*ST*CH INDUSTRIES~N3*18983 WENDOVER~N4*GRANGER*IA*50109~N1*BT*TRACHTE BUILDING SYS %AFS @@~N4*MAULDIN*SC*29662~LX*1~AT7*X3*NS***20191218*0000*LT~MS1*SUN PRAIRIE*WI~AT7*AF*NS***20191218*0000*LT~MS1*SUN PRAIRIE*WI~AT8*G*L*532*1~LX*2~AT7*X1*NS***20191219*1004*LT~MS1*GRANGER*IA~AT7*D1*NS***20191219*1004*LT~MS1*GRANGER*IA~AT7*CD*NS***20191219*1004*LT~MS1*GRANGER*IA~AT8*G*L*532*1~SE*26*000070001~GE*001*00007~IEA*1*000000007~"
        Dim strTestFile As String = "C:\Data\EDI\EDIConfig.txt"


        Dim oRet = oEDIInput.ProcessData(strEDI, testParams.ConnectionString, 0, "", Date.Now(), strTestFile, False)
        If oRet = Configuration.ProcessDataReturnValues.nglDataIntegrationComplete Then
            strResults = "Success"
        End If
        Assert.AreEqual("Success", strResults)
    End Sub




End Class
