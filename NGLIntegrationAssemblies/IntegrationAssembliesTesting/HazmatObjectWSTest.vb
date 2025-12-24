Imports System
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports DAL = Ngl.FreightMaster.Data
Imports Ngl.FreightMaster.Integration

<TestClass()> Public Class HazmatObjectWSTest
    Inherits TestBase

    <TestMethod()>
    Public Sub HazmatProcessDataTest()
        testParameters.DBServer = "NGLRDP06D"
        testParameters.Database = "SHIELDSMASUnitTest"

        'Always overwrite the web.config when running unit test
        FileCopy("Web.config", "D:\HTTP\WSUnitTest70\Web.config")

        Dim intResult As Integer = 0
        Dim strLastError As String = ""

        Dim oHazmat As New NGLHazmatObjectWebService.HazmatObject
        oHazmat.Url = WSUrl & "HazmatObject.ASMX"

        Dim oHaz As New DAL.NGLLookupDataProvider(testParameters)

        'Test creating a new Hazmat via web services
        Dim hazmats(2) As NGLHazmatObjectWebService.clsHazmatObject
        hazmats = createNewHazmat()
        Dim getHazDB As New Ngl.FreightMaster.Data.DataTransferObjects.Hazmat

        '******************** TEST INSERT NEW RECORD **********************
        Try
            Try
                'send the new hazmat data through the web services (70 version)
                intResult = oHazmat.ProcessData(WSAuthCode, hazmats, strLastError)
            Catch ex As Exception
                Assert.Fail("There was a problem with ProcessData in HazmatObject.asmx: " & ex.Message)
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

            ''Try to get back the new record from the database
            'Try
            '    getHazDB = GetHazmatDetails(hazmats(1).)
            'Catch ex As Exception

            'End Try






        Catch ex As Exception
            Throw ex
        Finally
            'Restore the data in tblImportFields back to the original values
            'restoretblImportFields(oHaz)
            Try
                'remove the record we inserted from the database
                'getCarrierDB = getCarrierByName(carObj.CarrierHeaders(1).CarrierName)
                'If getCarrierDB IsNot Nothing Then
                '    deleteCarrierRecord(getCarrierDB)
                'End If
            Catch ex As Exception
                Assert.Fail(ex.Message.ToString())
            End Try

        End Try

    End Sub

    Private Function createNewHazmat() As NGLHazmatObjectWebService.clsHazmatObject()
        Dim hazArray(2) As NGLHazmatObjectWebService.clsHazmatObject
        Dim newHaz As New NGLHazmatObjectWebService.clsHazmatObject

        With newHaz
            .HazRegulation = ""
            .HazItem = ""
            .HazClass = ""
            .HazID = ""
            .HazDesc01 = ""
            .HazDesc02 = ""
            .HazDesc03 = ""
            .HazUnit = ""
            .HazPackingGroup = ""
            .HazPackingDesc = ""
            .HazShipInst = ""
            .HazLtdQ = False
            .HazMarPoll = False
            .HazMarStorCat = ""
            .HazNMFCSub = 0
            .HazNMFC = 0
            .HazFrtClass = 0
            .HazFdxGndOK = False
            .HazFdxAirOK = False
            .HazUPSgndOK = False
            .HazUPSAirOK = False
        End With
        hazArray(1) = newHaz

        Return hazArray

    End Function

End Class