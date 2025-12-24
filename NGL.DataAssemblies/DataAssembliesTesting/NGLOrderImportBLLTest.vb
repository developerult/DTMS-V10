Imports System.Text
Imports System.ServiceModel
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports NGL.FreightMaster.Data.DataTransferObjects
Imports NGL.FM.BLL

<TestClass()> Public Class NGLOrderImportBLLTest
    Inherits TestBase

    
    <TestMethod()>
    Public Sub TestImportPOHdrRecordNOPRO()
        Dim strTestName As String = "ImportPOHdrRecordTest"
        Dim strPOHDRModVerify As String = "No Pro"
        Dim strPOHDROrdernumber As String = "277958"
        Dim intDefCompNumber As Integer = 1


        Dim blnRet As Boolean = False
        Dim strNewPro As String = ""
        testParameters.DBServer = "NGLRDP05D"
        testParameters.Database = "ShieldsMASPROD"
        Try
            'Get the first pohdr record available
            Dim oPohdr As POHdr = getPOHDR(strPOHDROrdernumber, intDefCompNumber)
            If oPohdr Is Nothing Then
                Assert.Fail(strTestName & " Failed: no PO records found")
                Return
            End If
            'Dim oBLL As New NGLOrderImportBLL(testParameters)


        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & strTestName & ": {0} ", ex.Message)
        Finally
            'place clean up code here
        End Try

    End Sub

    <TestMethod>
    Public Sub ImportPOHDRNoProTest()
        Dim strPOHDRModVerify As String = "NEW LANE"
        Dim strOrderNumber As String = "SO-0000070" ' "RR-SO0007"
        Dim intOrderSequence As Integer = 0
        Dim intDefCompNumber As Integer = 5
        Dim strVendorNumber As String = "CAL-00-CU00514-95965-5858-1"
        Dim strBookProNumber As String = "0"
        Dim blnAsync As Boolean = False
        Dim strTestName As String = "ImportPOHDRNoProTest"
        testParameters.DBServer = "DESKTOP-0R0EJUB"
        testParameters.Database = "NGLMASTEST"
        'NEW TRAN','SO1085',0,1,'BLU-10000-10019-1','0',true
        Try
            Dim target As NGLOrderImportBLL = New NGLOrderImportBLL(testParameters)

            Dim blnRet = target.ImportPOHdrRecord(strPOHDRModVerify, strOrderNumber, intOrderSequence, intDefCompNumber, strVendorNumber, strBookProNumber, blnAsync)
            If Not blnRet Then Assert.Fail(strTestName & " test failed.  Cound not import order number: " & strOrderNumber)
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As FaultException(Of NGL.FreightMaster.Data.SqlFaultInfo)
            Dim strMsg As String = ex.Detail.ToString(ex.Reason.ToString())
            Assert.Fail(strTestName & " test failed: {0} ", strMsg)
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & strTestName & ": {0} ", ex.Message)
        Finally
            'place clean up code here
        End Try
       
    End Sub

    <TestMethod()>
    Public Sub TestGetImportChanges()
        Dim strTestName As String = "GetImportChangesTest"
        Dim intCustomerNumber As Integer = 18
        Dim strOrderNumber As String = "TO065429"
        Dim intSequenceNumber As Integer = 0


        Dim blnRet As Boolean = False
        Dim strNewPro As String = ""
        testParameters.DBServer = "NGLRDP06D"
        testParameters.Database = "NGLMAS2013DEV"

        Dim target As NGLOrderImportBLL = New NGLOrderImportBLL(testParameters)

        Try
            Dim bookChanged = target.GetImportChanges(strOrderNumber, intSequenceNumber, intCustomerNumber)

            If bookChanged Is Nothing Then
                Assert.Fail(strTestName & " Failed: no PO records found")
                Return
            End If
            'Dim oBLL As New NGLOrderImportBLL(testParameters)


        Catch ex As Exception

        End Try

    End Sub


    <TestMethod>
    Public Sub ProcessNewBookWaitingTest()
        
        Dim strTestName As String = "ProcessNewBookWaitingTest"
        'testParameters.DBServer = "NGLRDP06D"
        'testParameters.Database = "NGLMASDEV7051"
        'testParameters.DBServer = "RFDBPROD2" ' "DESKTOP-0R0EJUB" ' "DESKTOP-0R0EJUB" ' "RFDBPROD2" '  ' "NGLRDP07D" '"NGLRDP06D\DTMS365" '"DESKTOP-0R0EJUB" '"NGLRDP07D"
        'testParameters.Database = "NGLMASPROD" '"NGLMASReily" '  "NGLMASColorTech" ' "NgLMASCabotTest" ' "NGLMASPRODBON"
        testParameters.DBServer = "DESKTOP-0R0EJUB\SQL2022" ' "DESKTOP-0R0EJUB" ' "RFDBPROD2" '  ' "NGLRDP07D" '"NGLRDP06D\DTMS365" '"DESKTOP-0R0EJUB" '"NGLRDP07D"
        testParameters.Database = "NGLMASAppvion" '  "NGLMASColorTech" ' "NgLMASCabotTest" ' "NGLMASPRODBON"

        testParameters.UserName = "ngl\rramsey"  '"DESKTOP-0R0EJUB\rkrte" '   "ngl\rramsey" ' "ngl\rramsey"
        'testParameters.ConnectionString = "Server=RFDBPROD2;User ID=nglweb;Password=Jj5MnzpDukCbYHFx4s8ZE2TX;Database=NGLMASPROD"
        testParameters.ConnectionString = "Server=" & testParameters.DBServer & ";User ID=nglweb;Password=5529;Database=" & testParameters.Database

        'testParameters.WCFAuthCode = "WCFTEST"
        testParameters.UserName = "FM Task Server"
        testParameters.WCFAuthCode = "NGLSystem"
        testParameters.ValidateAccess = False
        'testParameters.ConnectionString = "Server=" & testParameters.DBServer & ";User ID=nglweb;Password=Jj5MnzpDukCbYHFx4s8ZE2TX;Database=" & testParameters.Database
        'testParameters.ConnectionString = "Server=" & testParameters.DBServer & ";User ID=nglweb;Password=5529;Database=" & testParameters.Database

        'testParameters.ConnectionString = "Server=" & testParameters.DBServer & ";User ID=nglweb;Password=5529;Database=" & testParameters.Database

        Try
            Dim target As NGLOrderImportBLL = New NGLOrderImportBLL(testParameters)

            'target.ProcessNewBookWaiting()



            Dim iResults = target.ProcessNewBookWaitingWithResults(True, True)
            Dim strMsg = "capture log data"
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As FaultException(Of Ngl.FreightMaster.Data.SqlFaultInfo)
            Dim strMsg As String = ex.Detail.ToString(ex.Reason.ToString())
            Assert.Fail(strTestName & " test failed: {0} ", strMsg)
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & strTestName & ": {0} ", ex.Message)
        Finally
            'place clean up code here
        End Try
        Dim blnEndOfTestBreak As Boolean = False
    End Sub


    <TestMethod>
    Public Sub UpdateDefaultCarrierForUnRoutedTest()
        Dim strTestName As String = "UpdateDefaultCarrierForUnRoutedTest"
        'testParameters.DBServer = "NGLRDP06D"
        'testParameters.Database = "NGLMASDEV7051"
        testParameters.DBServer = "DESKTOP-0R0EJUB" ' "NGLRDP07D"
        'testParameters.Database = "NGLMASPROD"

        testParameters.Database = "NGLMASPROD" ' "NGLMASPROD"
        testParameters.UserName = "ngl\rramsey" ' "ngl\rramsey"
        testParameters.ConnectionString = "Data Source=" & testParameters.DBServer & ";Initial Catalog=" & testParameters.Database & " ;User ID=nglweb;Password=5529"
        Dim BookControl As Integer = 5958
        Try
            Dim target As NGLOrderImportBLL = New NGLOrderImportBLL(testParameters)

            Dim oResults = target.UpdateDefaultCarrierForUnRouted(BookControl)
            If oResults.Success = False Then
                Assert.Fail(String.Concat(oResults.Messages))
            End If

        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As FaultException(Of NGL.FreightMaster.Data.SqlFaultInfo)
            Dim strMsg As String = ex.Detail.ToString(ex.Reason.ToString())
            Assert.Fail(strTestName & " test failed: {0} ", strMsg)
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & strTestName & ": {0} ", ex.Message)
        Finally
            'place clean up code here
        End Try
        Dim blnEndOfTestBreak As Boolean = False
    End Sub


End Class