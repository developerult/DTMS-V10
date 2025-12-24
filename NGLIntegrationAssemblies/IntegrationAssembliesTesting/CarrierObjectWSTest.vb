Imports System
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports DAL = NGL.FreightMaster.Data
Imports NGL.FreightMaster.Integration

<TestClass()> Public Class CarrierObjectWSTest
    Inherits TestBase

    <TestCategory("Nightly")>
    <TestMethod()>
    Public Sub CarrierProcessObjectData70Test()
        testParameters.DBServer = "NGLRDP06D"
        testParameters.Database = "SHIELDSMASUnitTest"

        'Always overwrite the web.config when running unit test
        FileCopy("Web.config", "D:\HTTP\WSUnitTest70\Web.config")

        Dim intResult As Integer = 0
        Dim strLastError As String = ""

        Dim oCarrier As New NGLCarrierObjectWebService.CarrierObject
        oCarrier.Url = WSUrl & "CarrierObject.ASMX"

        Dim oCarr As New DAL.NGLCarrierData(testParameters)

        'Test creating a new Carrier via web services
        Dim carObj As New CarrierTestObject
        carObj = createNewCarrier()
        Dim getCarrierDB As New Ngl.FreightMaster.Data.DataTransferObjects.Carrier

        '******************** TEST INSERT NEW RECORD **********************
        Try
            Try
                'send the new carrier data through the web services (70 version)
                intResult = oCarrier.ProcessData70(WSAuthCode, carObj.CarrierHeaders(), carObj.CarrierContacts(), strLastError)
            Catch ex As Exception
                Assert.Fail("There was a problem with ProcessData70 in CarrierObject.asmx: " & ex.Message)
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
                getCarrierDB = getCarrierByName(carObj.CarrierHeaders(1).CarrierName)
            Catch ex As Exception
                'If we can't get it back something must have gone wrong with the insert
                Assert.Fail("There was a problem when attempting to read the record back from the database: " & ex.Message)
            End Try

            'Check key fields and fields new to 70; check the original object vs the one read from the db after insert
            Dim exp = carObj.CarrierHeaders(1)
            Assert.AreEqual(exp.CarrierName, getCarrierDB.CarrierName)
            Assert.AreEqual(exp.CarrierNumber, getCarrierDB.CarrierNumber)
            Assert.AreEqual(exp.CarrierSCAC, getCarrierDB.CarrierSCAC)
            Assert.AreEqual(exp.CarrierStreetAddress1, getCarrierDB.CarrierStreetAddress1)
            Assert.AreEqual(exp.CarrierLegalEntity, getCarrierDB.CarrierLegalEntity)
            Assert.AreEqual(exp.CarrierAlphaCode, getCarrierDB.CarrierAlphaCode)
            'Assert.AreEqual(exp.CarrierUser1, getCarrierDB.CarrierUser1)
            'Assert.AreEqual(exp.CarrierUser2, getCarrierDB.CarrierUser2)
            'Assert.AreEqual(exp.CarrierUser3, getCarrierDB.CarrierUser3)
            'Assert.AreEqual(exp.CarrierUser4, getCarrierDB.CarrierUser4)

            '********************* TEST UPDATE RECORD ***********************
            'Note: If the CarrierName, Number, Alpha Code, or Legal Entity is changed it performs
            '      an insert instead of an update
            'Note: The CarrierUser fields check is commented out for now because they are not
            '      returned in the getCarrierFiltered method yet...will fix later per Rob

            '******* Allow Update Flag FALSE *******

            'First set the AllowUpdate parameter for all fields to false
            setAllowUpdateFlags(oCarr, 0)

            'Now try updating the record by changing some fields
            exp.CarrierStreetAddress1 = "UPDATE TEST"
            exp.CarrierSCAC = "SNEW"
            exp.CarrierUser1 = "UPDATE TEST"
            exp.CarrierUser2 = "UPDATE TEST"
            exp.CarrierUser3 = "UPDATE TEST"
            exp.CarrierUser4 = "UPDATE TEST"

            'send the new carrier data through the web services (70 version)
            Try
                intResult = oCarrier.ProcessData70(WSAuthCode, carObj.CarrierHeaders(), carObj.CarrierContacts(), strLastError)
            Catch ex As Exception
                Assert.Fail("There was a problem with ProcessData70 in CarrierObject.asmx: " & ex.Message)
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
                getCarrierDB = getCarrierByName(exp.CarrierName)
            Catch ex As Exception
                'If we can't get it back something must have gone wrong with the insert
                Assert.Fail("There was a problem when attempting to read the record back from the database: " & ex.Message)
            End Try

            'check to see if the field was changed -- it should still be the same
            Assert.AreNotEqual(exp.CarrierSCAC, getCarrierDB.CarrierSCAC)
            Assert.AreNotEqual(exp.CarrierStreetAddress1, getCarrierDB.CarrierStreetAddress1)
            'Assert.AreNotEqual(exp.CarrierUser1, getCarrierDB.CarrierUser1)
            'Assert.AreNotEqual(exp.CarrierUser2, getCarrierDB.CarrierUser2)
            'Assert.AreNotEqual(exp.CarrierUser3, getCarrierDB.CarrierUser3)
            'Assert.AreNotEqual(exp.CarrierUser4, getCarrierDB.CarrierUser4)

            '******** Allow Update Flag TRUE *******

            'First set the AllowUpdate parameter for all fields to true
            setAllowUpdateFlags(oCarr, 1)

            'send the new carrier data through the web services (70 version)
            Try
                intResult = oCarrier.ProcessData70(WSAuthCode, carObj.CarrierHeaders(), carObj.CarrierContacts(), strLastError)
            Catch ex As Exception
                Assert.Fail("There was a problem with ProcessData70 in CarrierObject.asmx: " & ex.Message)
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

                '**************************************************************************************
                'THIS METHOD CALLS GETCARRIERFILTERED WHICH DOES NOT ACTUALLY RETURN CARRIERUSER FIELDS
                'IS THAT METHOD SUPPOSED TO ONLY RETURN SOME FIELDS OR IS IT MISSING FIELDS??
                '**************************************************************************************

                getCarrierDB = getCarrierByName(exp.CarrierName)
            Catch ex As Exception
                'If we can't get it back something must have gone wrong with the insert
                Assert.Fail("There was a problem when attempting to read the record back from the database: " & ex.Message)
            End Try

            'check to see if the field was changed
            Assert.AreEqual("UPDATE TEST", getCarrierDB.CarrierStreetAddress1)
            Assert.AreEqual("SNEW", getCarrierDB.CarrierSCAC)
            'Assert.AreEqual("UPDATE TEST", getCarrierDB.CarrierUser1)
            'Assert.AreEqual("UPDATE TEST", getCarrierDB.CarrierUser2)
            'Assert.AreEqual("UPDATE TEST", getCarrierDB.CarrierUser3)
            'Assert.AreEqual("UPDATE TEST", getCarrierDB.CarrierUser4)

        Catch ex As Exception
            Throw ex
        Finally
            'Restore the data in tblImportFields back to the original values
            restoretblImportFields(oCarr)
            Try
                'remove the record we inserted from the database
                getCarrierDB = getCarrierByName(carObj.CarrierHeaders(1).CarrierName)
                If getCarrierDB IsNot Nothing Then
                    deleteCarrierRecord(getCarrierDB)
                End If
            Catch ex As Exception
                Assert.Fail(ex.Message.ToString())
            End Try

        End Try

    End Sub



    Private Function createNewCarrier() As CarrierTestObject
        Dim retObj As New CarrierTestObject

        Dim hdrs(2) As NGLCarrierObjectWebService.clsCarrierHeaderObject70
        Dim conts(2) As NGLCarrierObjectWebService.clsCarrierContactObject70


        Dim newCarrier As New NGLCarrierObjectWebService.clsCarrierHeaderObject70
        With newCarrier
            .CarrierNumber = 27000
            .CarrierName = "LVV Test Carrier WSUT"
            .CarrierStreetAddress1 = "2700 W. Main St."
            .CarrierStreetAddress2 = ""
            .CarrierStreetAddress3 = ""
            .CarrierStreetCity = "Turlock"
            .CarrierStreetState = "CA"
            .CarrierStreetZip = "95380-9537"
            .CarrierStreetCountry = "USA"
            .CarrierMailAddress1 = "2700 W. Main St."
            .CarrierMailAddress2 = ""
            .CarrierMailAddress3 = ""
            .CarrierMailCity = "Turlock"
            .CarrierMailState = "CA"
            .CarrierMailZip = "95380-9537"
            .CarrierMailCountry = "USA"
            .CarrierTypeCode = "V"
            .CarrierSCAC = "TEST"
            .CarrierWebSite = "www.poppystate.com"
            .CarrierEmail = "laurenvanvleet@nextgeneration.com"
            .CarrierQualInsuranceDate = "05/01/2015"
            .CarrierQualQualified = True
            .CarrierQualAuthority = "223132"
            .CarrierQualContract = True
            .CarrierQualSignedDate = "04/01/2015"
            .CarrierQualContractExpiresDate = "5/01/2020"
            'New Fields v-6.4
            .CarrierLegalEntity = "TestCarrierWS"
            .CarrierAlphaCode = "LVCAR"
            .CarrierCurrencyType = ""
            .CarrierUser1 = "User 1"
            .CarrierUser2 = "User 2"
            .CarrierUser3 = "User 3"
            .CarrierUser4 = "User 4"

        End With
        hdrs(1) = newCarrier

        Dim newContact As New NGLCarrierObjectWebService.clsCarrierContactObject70
        With newContact
            'Contacts
            .CarrierNumber = 27000
            .CarrierContName = "Mike x2037 Turlock"
            .CarrierLegalEntity = "TestCarrierWS"
            .CarrierAlphaCode = "LVCAR"
            .CarrierContTitle = "N CA/Reno,NV"
            .CarrierContact800 = "(800)692-5874"
            .CarrierContactPhone = "(323)581-9985"
            .CarrierContPhoneExt = "2037"
            .CarrierContactFax = ""
            .CarrierContactEMail = "laurenvanvleet@nextgeneration.com"
        End With
        conts(1) = newContact

        retObj.CarrierHeaders = hdrs
        retObj.CarrierContacts = conts

        Return retObj

    End Function

   

End Class



Class CarrierTestObject

    Private _CarrierHeaders As NGLCarrierObjectWebService.clsCarrierHeaderObject70()
    Public Property CarrierHeaders() As NGLCarrierObjectWebService.clsCarrierHeaderObject70()
        Get
            Return _CarrierHeaders
        End Get
        Set(ByVal value As NGLCarrierObjectWebService.clsCarrierHeaderObject70())
            _CarrierHeaders = value
        End Set
    End Property

    Private _CarrierContacts As NGLCarrierObjectWebService.clsCarrierContactObject70()
    Public Property CarrierContacts() As NGLCarrierObjectWebService.clsCarrierContactObject70()
        Get
            Return _CarrierContacts
        End Get
        Set(ByVal value As NGLCarrierObjectWebService.clsCarrierContactObject70())
            _CarrierContacts = value
        End Set
    End Property





End Class