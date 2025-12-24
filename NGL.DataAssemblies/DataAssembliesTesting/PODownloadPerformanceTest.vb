Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports System
Imports NGL.FM.CarTar
Imports DAL = NGL.FreightMaster.Data
Imports DTO = NGL.FreightMaster.Data.DataTransferObjects
Imports BLL = NGL.FM.BLL

<TestClass()> Public Class PODownloadPerformanceTest
    Inherits TestBase



    <TestMethod> _
    Public Sub ImportPOHdrNoProTest()
        Dim strTestName As String = "ImportPOHdrRecordTest"
        Dim strPOHDRModVerify As String = "No Pro"
        Dim intDefCompNumber As Integer = 1
       

        Dim blnRet As Boolean = False
        Dim strNewPro As String = ""
        testParameters.DBServer = "NGLRDP05D"
        testParameters.Database = "ShieldsOrderPreviewTest"
        Try
            'Get the first pohdr record available
            Dim oPohdrs As List(Of DTO.POHdr) = getPOHDRs("No Pro", 1, 10)
            If oPohdrs Is Nothing OrElse oPohdrs.Count < 1 Then
                Assert.Fail(strTestName & " Failed: no PO records found")
                Return
            End If
            For Each p In oPohdrs
                'populate the variables
                Dim strOrderNumber As String = p.POHDROrderNumber
                Dim intOrderSequence As Integer = p.POHDROrderSequence
                Dim strVendorNumber As String = p.POHDRvendor
                Dim strBookProNumber As String = p.POHDRPRONumber
                Dim oBook As New DAL.NGLBookData(testParameters)
                'process the data
                Dim oResult = oBook.WriteNewBookingForBatch(strOrderNumber, intOrderSequence, intDefCompNumber, strNewPro)
                Dim target As New BLL.NGLOrderImportBLL(testParameters)
                target.TryToRouteLoad(strNewPro)
                'target.TryToRouteLoadAsync(strNewPro)
            Next

            blnRet = True

        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail(strTestName & "Unexpected Error: {0} ", ex.Message)
        End Try
    End Sub




    <TestMethod> _
    Public Sub ImportPOHdrNoLaneTest()
        Dim strTestName As String = "ImportPOHdrNoLaneTest"
        Dim strPOHDRModVerify As String = "No Pro"
        Dim intDefCompNumber As Integer = 31


        Dim blnRet As Boolean = False
        Dim strNewPro As String = ""
        testParameters.DBServer = "NGLRDP06D"
        testParameters.Database = "NGLMAS2013DEV"
        Try
            'Get the first pohdr record available
            Dim oPohdr As DTO.POHdr = getPOHDR("SO-TEST1136", 31)
            If oPohdr Is Nothing Then
                Assert.Fail(strTestName & " Failed: no PO record found")
                Return
            End If
                'populate the variables
            Dim strOrderNumber As String = oPohdr.POHDROrderNumber
            Dim intOrderSequence As Integer = oPohdr.POHDROrderSequence
            Dim strVendorNumber As String = oPohdr.POHDRvendor
            Dim strBookProNumber As String = oPohdr.POHDRPRONumber
            Dim oBook As New DAL.NGLBookData(testParameters)
            'process the data
            Dim oResult = oBook.WriteNewBookingForBatch(strOrderNumber, intOrderSequence, intDefCompNumber, strNewPro)
            Dim target As New BLL.NGLOrderImportBLL(testParameters)
            target.TryToRouteLoad(strNewPro)
                'target.TryToRouteLoadAsync(strNewPro)


            blnRet = True
        Catch ex As System.ServiceModel.FaultException(Of DAL.SqlFaultInfo)

            Dim sReason As String = ex.Reason.ToString()
            Dim oDetail = ex.Detail
            Dim sDetails = ex.Detail.DetailsList
            Dim sMessage As String = ex.Detail.Message
            Dim sDMessage = DAL.SqlFaultInfo.getFaultDetailsKeyNotLocalizedString(DAL.SqlFaultInfo.getFaultDetailsKeyEnumFromString(ex.Detail.Details), ex.Detail.Details)
            Dim sFailMsg As String = strTestName & " Reason: " & sReason & " Message: " & sMessage
            System.Diagnostics.Debug.WriteLine(sFailMsg)
            If Not sDetails Is Nothing Then
                System.Diagnostics.Debug.WriteLine(String.Format(sDMessage, sDetails.ToArray()))
            Else
                System.Diagnostics.Debug.WriteLine(sDMessage)
            End If
            Assert.Fail(strTestName & " Reason: " & sReason & " Message: " & sMessage)
            'Dim sDetailResult
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail(strTestName & "Unexpected Error: {0} ", ex.Message)
        End Try
    End Sub



End Class
