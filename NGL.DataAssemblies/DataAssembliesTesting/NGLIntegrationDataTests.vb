Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports System
Imports DAL = Ngl.FreightMaster.Data
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects

<TestClass()>
Public Class NGLIntegrationDataTests
    Inherits TestBase

    <TestMethod()>
    Public Sub CannotCreateDuplicateImportFieldDataTest()
        Try
            Dim target As New DAL.NGLtblImportFieldData(testParameters)
            'this code should fail because the CompNumber field already exists
            target.CreateRecord(New DTO.tblImportField With {.ImportFieldName = "CompNumber", .ImportFileName = "Company", .ImportFileType = 2, .ImportFlag = True, .TrackingState = NGL.Core.ChangeTracker.TrackingInfo.Created})

        Catch ex As System.ServiceModel.FaultException(Of DAL.SqlFaultInfo)
            If Not ex.Detail.Details = "E_CannotSaveKeyAlreadyExists" Then
                Assert.Fail("CannotCreateDuplicateImportFieldDataTest Datavalidation Failed: " & ex.Message)
            End If
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("CannotCreateDuplicateImportFieldDataTest Unexpected Error: " & ex.Message)
        Finally

        End Try
    End Sub

    <TestMethod()>
    Public Sub CreateImportFieldDataTest()
        Dim target As New DAL.NGLtblImportFieldData(testParameters)
        Dim sFieldName As String = "RobCompNumber"
        Dim sFilename As String = "Company"
        Dim iFileType As Integer = 2
        Try

            'this code should fail because the CompNumber field already exists
            target.CreateRecord(New DTO.tblImportField With {.ImportFieldName = sFieldName, .ImportFileName = sFilename, .ImportFileType = iFileType, .ImportFlag = True, .TrackingState = NGL.Core.ChangeTracker.TrackingInfo.Created})

        Catch ex As System.ServiceModel.FaultException(Of DAL.SqlFaultInfo)
            If Not ex.Detail.Details = "E_CannotSaveKeyAlreadyExists" Then
                Assert.Fail("CannotCreateDuplicateImportFieldDataTest Datavalidation Failed: " & ex.Message)
            End If
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("CannotCreateDuplicateImportFieldDataTest Unexpected Error: " & ex.Message)
        Finally
            Try
                'delete the test record
                Dim toDel = target.GettblImportFieldFiltered(iFileType, sFieldName)
                If Not toDel Is Nothing Then
                    target.DeleteRecord(toDel)
                End If
            Catch ex As Exception
                'ignore all errors durring cleanup
            End Try
        End Try
    End Sub
    'getImportFieldFlagList

    <TestMethod()>
    Public Sub GetImportFieldFlagListTest()
        Try
            Dim target As New DAL.NGLtblImportFieldData(testParameters)
            Dim oFieldnames = target.getImportFieldFlagList(2)
            If oFieldnames Is Nothing OrElse oFieldnames.Count() < 1 Then
                Assert.Fail("GetImportFieldFlagListTest Failed No Data")
            End If
            For Each f In oFieldnames
                System.Diagnostics.Debug.WriteLine("Key: {0} value: {1}", f.ImportFieldName, f.ImportFlag)
            Next
            System.Diagnostics.Debug.WriteLine("Success!")
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("GetImportFieldFlagListTest Unexpected Error: " & ex.Message)
        Finally

        End Try
    End Sub

    <TestMethod()>
    Public Sub GetImportFieldFlagDictionaryDataTest()
        Try
            Dim target As New DAL.NGLtblImportFieldData(testParameters)
            Dim oFieldnames As Dictionary(Of String, Boolean) = target.getImportFieldFlagDictionary(2)
            If oFieldnames Is Nothing OrElse oFieldnames.Count() < 1 Then
                Assert.Fail("GetImportFieldFlagDictionaryDataTest Failed No Data")
            End If
            For Each f In oFieldnames
                System.Diagnostics.Debug.WriteLine("Key: {0} value: {1}", f.Key, f.Value)
            Next
            System.Diagnostics.Debug.WriteLine("Success!")
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("GetImportFieldFlagDictionaryDataTest Unexpected Error: " & ex.Message)
        Finally

        End Try
    End Sub


    <TestMethod()>
    Public Sub InsertFreightBillUniqueTest()

        testParameters.DBServer = "NGLRDP07D"
        testParameters.Database = "NGLMASPROD"
        testParameters.WCFAuthCode = "NGLSystem"
        testParameters.UserName = "ngl\rramsey"
        testParameters.ConnectionString = "Server=" & testParameters.DBServer & ";User ID=nglweb;Password=5529;Database=" & testParameters.Database

        '    <!--<add name="NGLMASPROD" connectionString="Server=" & testParameters.DBServer & ";User ID=nglweb;Password=5529;Database=NGLMASPROD" providerName="System.Data.SqlClient" />-->


        Dim APPONumber As String = ""
        Dim APPRONumber As String = ""
        Dim APCNSNumber As String = ""
        Dim APSHID As String = "CNS-1-1967"
        Dim APCarrierNumber As Integer = 1611
        Dim APBillNumber As String = "INV-1-1967"
        Dim APBillDate As Date = Date.Now()
        Dim APCustomerID As String = ""
        Dim APCostCenterNumber As String = ""
        Dim APTotalCost As Decimal = 3047.91
        Dim APBLNumber As String = ""
        Dim APBilledWeight As Integer = 0
        Dim APReceivedDate As Date = Date.Now()
        Dim APPayCode As String = "N"
        Dim APElectronicFlag As Boolean = False
        Dim APTotalTax As Decimal = 0
        Dim APFee1 As Decimal = 171.77
        Dim APFee2 As Decimal = 100.0
        Dim APFee3 As Decimal = 0
        Dim APFee4 As Decimal = 0
        Dim APFee5 As Decimal = 0
        Dim APFee6 As Decimal = 0
        Dim APOtherCost As Decimal = 0
        Dim APCarrierCost As Decimal = 2776.14
        Dim APOverwrite As Boolean = False
        Dim APOrderSequence As Integer = 0
        Dim APTaxDetail1 As Decimal = 0
        Dim APTaxDetail2 As Decimal = 0
        Dim APTaxDetail3 As Decimal = 0
        Dim APTaxDetail4 As Decimal = 0
        Dim APTaxDetail5 As Decimal = 0
        Dim TwoWay As Boolean = False

        Try
            Dim target As New DAL.NGLBatchProcessDataProvider(testParameters)
            Dim oResults As DAL.Models.ResultObject = target.InsertFreightBillUnique(APPONumber,
                                        APPRONumber,
                                        APCNSNumber,
                                        APSHID,
                                        APCarrierNumber,
                                        APBillNumber,
                                        APBillDate,
                                        APCustomerID,
                                        APCostCenterNumber,
                                        APTotalCost,
                                        APBLNumber,
                                        APBilledWeight,
                                        APReceivedDate,
                                        APPayCode,
                                        APElectronicFlag,
                                        APTotalTax,
                                        APFee1,
                                        APFee2,
                                        APFee3,
                                        APFee4,
                                        APFee5,
                                        APFee6,
                                        APOtherCost,
                                        APCarrierCost,
                                        APOverwrite,
                                        APOrderSequence,
                                         APTaxDetail1,
                                         APTaxDetail2,
                                         APTaxDetail3,
                                         APTaxDetail4,
                                         APTaxDetail5,
                                         TwoWay)
            If oResults.Success = False Then
                System.Diagnostics.Debug.WriteLine("Failed!")
                System.Diagnostics.Debug.WriteLine("Error: " & oResults.ErrMsg)

                System.Diagnostics.Debug.WriteLine("Warning: " & oResults.WarningMsg)

                System.Diagnostics.Debug.WriteLine("Msg : " & oResults.Msg)
            Else
                System.Diagnostics.Debug.WriteLine("Success!")
            End If


        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("InsertFreightBillUniqueTest Unexpected Error: " & ex.Message)
        Finally
            System.Diagnostics.Debug.WriteLine("Test Complete")
        End Try


    End Sub



End Class
