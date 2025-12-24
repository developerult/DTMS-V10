Imports System
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports DAL = NGL.FreightMaster.Data
Imports Ngl.FreightMaster.Integration

<TestClass()> Public Class CompanyObjectWSTest
    Inherits TestBase

    <TestCategory("Nightly")>
    <TestMethod()>
    Public Sub CompProcessData70Test()
        testParameters.DBServer = "NGLRDP06D"
        testParameters.Database = "SHIELDSMASUnitTest"

        'Always overwrite the web.config when running unit test
        FileCopy("Web.config", "D:\HTTP\WSUnitTest70\Web.config")

        Dim intResult As Integer = 0
        Dim strLastError As String = ""

        Dim oCompany As New NGLCompanyObjectWebService.CompanyObject
        oCompany.Url = WSUrl & "CompanyObject.ASMX"

        Dim oComp As New DAL.NGLCompData(testParameters)

        'Test creating a new Company via web services
        Dim compObj As New CompTestObj
        compObj = createNewComp()
        Dim getCompDB As New Ngl.FreightMaster.Data.DataTransferObjects.Comp

        '******************** TEST INSERT NEW RECORD **********************
        Try
            Try
                'send the new comp data through the web services (70 version)           
                intResult = oCompany.ProcessData70(WSAuthCode, compObj.CompHeaders(), compObj.CompContacts.ToArray(), compObj.CompCals.ToArray(), strLastError)
            Catch ex As Exception
                Assert.Fail("There was a problem with ProcessData70 in CompanyObject.asmx: " & ex.Message)
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
                getCompDB = getCompanyFiltered(0, compObj.CompHeaders(1).CompNumber, "")
            Catch ex As Exception
                'If we can't get it back something must have gone wrong with the insert
                Assert.Fail("There was a problem when attempting to read the record back from the database: " & ex.Message)
            End Try

            'Check key fields and fields new to 70; check the original object vs the one read from the db after insert
            Dim exp = compObj.CompHeaders(1)
            'Key fields
            Assert.AreEqual(exp.CompNumber, getCompDB.CompNumber)
            Assert.AreEqual(exp.CompName, getCompDB.CompName)
            'Other fields
            Assert.AreEqual(exp.CompAbrev, getCompDB.CompAbrev)
            Assert.AreEqual(exp.CompStreetAddress1, getCompDB.CompStreetAddress1)
            Assert.AreEqual(exp.CompMailAddress1, getCompDB.CompMailAddress1)
            'New fields 70
            Assert.AreEqual(exp.CompLegalEntity, getCompDB.CompLegalEntity)
            Assert.AreEqual(exp.CompAlphaCode, getCompDB.CompAlphaCode)
            'Assert.AreEqual(exp.CompCurrencyType, getCompDB.CompCurrencyType)
            Assert.AreEqual(exp.CompTimeZone, getCompDB.CompTimeZone)
            Assert.AreEqual(exp.CompRailStationName, getCompDB.CompRailStationName)
            Assert.AreEqual(exp.CompRailSPLC, getCompDB.CompRailSPLC)
            Assert.AreEqual(exp.CompRailFSAC, getCompDB.CompRailFSAC)
            Assert.AreEqual(exp.CompRail333, getCompDB.CompRail333)
            Assert.AreEqual(exp.CompRailR260, getCompDB.CompRailR260)
            Assert.AreEqual(exp.CompIsTransLoad, getCompDB.CompIsTransLoad)
            Assert.AreEqual(exp.CompUser1, getCompDB.CompUser1)
            Assert.AreEqual(exp.CompUser2, getCompDB.CompUser2)
            Assert.AreEqual(exp.CompUser3, getCompDB.CompUser3)
            Assert.AreEqual(exp.CompUser4, getCompDB.CompUser4)

            '********************* TEST UPDATE RECORD ***********************

            '******* Allow Update Flag FALSE *******

            'First set the AllowUpdate parameter for all fields to false
            setAllowUpdateFlags(oComp, 0)

            'Now try updating the record by changing some fields       
            exp.CompStreetAddress1 = "UPDATE TEST"
            exp.CompMailAddress1 = "UPDATE TEST"
            'New fields 70
            exp.CompTimeZone = "UPDATE"
            exp.CompRailStationName = "UPDATE"
            exp.CompRailSPLC = "UPDATE"
            exp.CompRailFSAC = "UPDATE"
            exp.CompRail333 = "UPDATE"
            exp.CompRailR260 = "UPDATE"
            exp.CompIsTransLoad = True
            exp.CompUser1 = "UPDATE"
            exp.CompUser2 = "UPDATE"
            exp.CompUser3 = "UPDATE"
            exp.CompUser4 = "UPDATE"

            'send the new comp data through the web services (70 version)
            Try
                intResult = oCompany.ProcessData70(WSAuthCode, compObj.CompHeaders(), compObj.CompContacts.ToArray(), compObj.CompCals.ToArray(), strLastError)
            Catch ex As Exception
                Assert.Fail("There was a problem with ProcessData70 in CompanyObject.asmx: " & ex.Message)
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
                getCompDB = getCompanyFiltered(0, exp.CompNumber, "")
            Catch ex As Exception
                'If we can't get it back something must have gone wrong with the insert
                Assert.Fail("There was a problem when attempting to read the record back from the database: " & ex.Message)
            End Try

            'check to see if the field was changed -- it should still be the same           
            Assert.AreNotEqual(exp.CompStreetAddress1, getCompDB.CompStreetAddress1)
            Assert.AreNotEqual(exp.CompMailAddress1, getCompDB.CompMailAddress1)
            'New fields 70
            Assert.AreNotEqual(exp.CompTimeZone, getCompDB.CompTimeZone)
            Assert.AreNotEqual(exp.CompRailStationName, getCompDB.CompRailStationName)
            Assert.AreNotEqual(exp.CompRailSPLC, getCompDB.CompRailSPLC)
            Assert.AreNotEqual(exp.CompRailFSAC, getCompDB.CompRailFSAC)
            Assert.AreNotEqual(exp.CompRail333, getCompDB.CompRail333)
            Assert.AreNotEqual(exp.CompRailR260, getCompDB.CompRailR260)
            Assert.AreNotEqual(exp.CompIsTransLoad, getCompDB.CompIsTransLoad)
            Assert.AreNotEqual(exp.CompUser1, getCompDB.CompUser1)
            Assert.AreNotEqual(exp.CompUser2, getCompDB.CompUser2)
            Assert.AreNotEqual(exp.CompUser3, getCompDB.CompUser3)
            Assert.AreNotEqual(exp.CompUser4, getCompDB.CompUser4)

            '******** Allow Update Flag TRUE *******

            'First set the AllowUpdate parameter for all fields to true
            setAllowUpdateFlags(oComp, 1)

            'send the new comp data through the web services (70 version)
            Try
                intResult = oCompany.ProcessData70(WSAuthCode, compObj.CompHeaders(), compObj.CompContacts.ToArray(), compObj.CompCals.ToArray(), strLastError)
            Catch ex As Exception
                Assert.Fail("There was a problem with ProcessData70 in CompanyObject.asmx: " & ex.Message)
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
                getCompDB = getCompanyFiltered(0, exp.CompNumber, "")
            Catch ex As Exception
                'If we can't get it back something must have gone wrong with the insert
                Assert.Fail("There was a problem when attempting to read the record back from the database: " & ex.Message)
            End Try

            'check to see if the field was changed
            Assert.AreEqual("UPDATE TEST", getCompDB.CompStreetAddress1)
            Assert.AreEqual("UPDATE TEST", getCompDB.CompMailAddress1)
            'New fields 70
            Assert.AreEqual("UPDATE", getCompDB.CompTimeZone)
            Assert.AreEqual("UPDATE", getCompDB.CompRailStationName)
            Assert.AreEqual("UPDATE", getCompDB.CompRailSPLC)
            Assert.AreEqual("UPDATE", getCompDB.CompRailFSAC)
            Assert.AreEqual("UPDATE", getCompDB.CompRail333)
            Assert.AreEqual("UPDATE", getCompDB.CompRailR260)
            Assert.AreEqual(True, getCompDB.CompIsTransLoad)
            Assert.AreEqual("UPDATE", getCompDB.CompUser1)
            Assert.AreEqual("UPDATE", getCompDB.CompUser2)
            Assert.AreEqual("UPDATE", getCompDB.CompUser3)
            Assert.AreEqual("UPDATE", getCompDB.CompUser4)


        Catch ex As Exception
            Throw ex
        Finally
            'Restore the data in tblImportFields back to the original values
            restoretblImportFields(oComp)
            Try
                'remove the record we inserted from the database
                getCompDB = getCompanyFiltered(0, compObj.CompHeaders(1).CompNumber, "")
                If getCompDB IsNot Nothing Then
                    deleteCompanyRecord(getCompDB)
                End If
            Catch ex As Exception
                Assert.Fail(ex.Message.ToString())
            End Try

        End Try

    End Sub

    Private Function createNewComp() As CompTestObj
        Dim retObj As New CompTestObj

        Dim hdrs(2) As NGLCompanyObjectWebService.clsCompanyHeaderObject70
        Dim conts(2) As NGLCompanyObjectWebService.clsCompanyContactObject70
        Dim cals(2) As NGLCompanyObjectWebService.clsCompanyCalendarObject70

        Dim newComp As New NGLCompanyObjectWebService.clsCompanyHeaderObject70
        With newComp
            .CompNumber = 1000027
            .CompName = "LVV Test Comp WSUT"
            .CompNatNumber = 0
            .CompNatName = ""
            .CompStreetAddress1 = "7400 South Narragansett Avenue"
            .CompStreetAddress2 = ""
            .CompStreetAddress3 = ""
            .CompStreetCity = "Bedford Park"
            .CompStreetState = "IL"
            .CompStreetZip = "60638"
            .CompStreetCountry = "US"
            .CompMailAddress1 = "7400 South Narragansett Avenue"
            .CompMailAddress2 = ""
            .CompMailAddress3 = ""
            .CompMailCity = "Bedford Park"
            .CompMailState = "IL"
            .CompMailZip = "60638"
            .CompMailCountry = "US"
            .CompWeb = ""
            .CompEmail = "laurenvanvleet@nextgeneration.com"
            .CompDirections = ""
            .CompAbrev = "LVV"
            .CompActive = 1
            .CompNEXTrack = 1
            .CompNEXTStopAcctNo = ""
            .CompNEXTStopPsw = ""
            .CompNextstopSubmitRFP = 0
            .CompFAAShipID = ""
            .CompFAAShipDate = "" 'Date.Now
            .CompFinDuns = "00-512-5935"
            .CompFinTaxID = "36-1909400"
            .CompFinPaymentForm = 1
            .CompFinSIC = ""
            .CompFinPaymentDiscount = 0
            .CompFinPaymentDays = 0
            .CompFinPaymentNetDays = 0
            .CompFinCommTerms = ""
            .CompFinCommTermsPer = 0
            .CompFinCreditLimit = 10000000
            .CompFinCreditUsed = 0
            .CompFinInvPrnCode = 0
            .CompFinInvEMailCode = 1
            .CompFinCurType = 1
            .CompFinFBToleranceHigh = 0
            .CompFinFBToleranceLow = 0
            .CompFinCustomerSince = ""
            .CompFinCardType = "1"
            .CompFinCardName = ""
            .CompFinCardExpires = ""
            .CompFinCardAuthorizor = ""
            .CompFinCardAuthPassword = ""
            .CompFinUseImportFrtCost = 0
            .CompFinBkhlFlatFee = 0
            .CompFinBkhlCostPerc = 0
            .CompLatitude = 41.79
            .CompLongitude = -87.769166666666663
            .CompMailTo = ""
            'New Fields 70
            .CompLegalEntity = "TestCompWS"
            .CompAlphaCode = "LVVWS"
            .CompCurrencyType = ""
            .CompTimeZone = ""
            .CompRailStationName = ""
            .CompRailSPLC = ""
            .CompRailFSAC = ""
            .CompRail333 = ""
            .CompRailR260 = ""
            .CompIsTransLoad = False
            .CompUser1 = ""
            .CompUser2 = ""
            .CompUser3 = ""
            .CompUser4 = ""
        End With
        hdrs(1) = newComp

        'Contacts
        Dim newCompCont As New NGLCompanyObjectWebService.clsCompanyContactObject70
        With newCompCont
            .CompNumber = 1000027
            .CompContName = "Mike O'Hara"
            .CompContTitle = "Director of Operations"
            .CompCont800 = ""
            .CompContPhone = "(708)924-9500"
            .CompContPhoneExt = "324"
            .CompContFax = ""
            .CompContEmail = "laurenvanvleet@nextgeneration.com"
            .CompLegalEntity = "TestCompWS"
            .CompAlphaCode = "LVVWS"
        End With
        conts(1) = newCompCont

        'Calendar
        Dim newCompCal As New NGLCompanyObjectWebService.clsCompanyCalendarObject70
        With newCompCal
            .CompNumber = 1000027
            .Month = 7
            .Day = 2
            .Open = 1
            .StartTime = ""
            .EndTime = ""
            .IsHoliday = 0
            .CompLegalEntity = "TestCompWS"
            .CompAlphaCode = "LVVWS"
        End With
        cals(1) = newCompCal

        retObj.CompHeaders = hdrs
        retObj.CompContacts = conts
        retObj.CompCals = cals

        Return retObj

    End Function


End Class

Class CompTestObj

    Private _CompHeaders As NGLCompanyObjectWebService.clsCompanyHeaderObject70()
    Public Property CompHeaders() As NGLCompanyObjectWebService.clsCompanyHeaderObject70()
        Get
            Return _CompHeaders
        End Get
        Set(ByVal value As NGLCompanyObjectWebService.clsCompanyHeaderObject70())
            _CompHeaders = value
        End Set
    End Property

    Private _CompContacts As NGLCompanyObjectWebService.clsCompanyContactObject70()
    Public Property CompContacts() As NGLCompanyObjectWebService.clsCompanyContactObject70()
        Get
            Return _CompContacts
        End Get
        Set(ByVal value As NGLCompanyObjectWebService.clsCompanyContactObject70())
            _CompContacts = value
        End Set
    End Property

    Private _CompCals As NGLCompanyObjectWebService.clsCompanyCalendarObject70()
    Public Property CompCals() As NGLCompanyObjectWebService.clsCompanyCalendarObject70()
        Get
            Return _CompCals
        End Get
        Set(ByVal value As NGLCompanyObjectWebService.clsCompanyCalendarObject70())
            _CompCals = value
        End Set
    End Property

End Class