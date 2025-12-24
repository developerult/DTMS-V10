Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports System
Imports DAL = NGL.FreightMaster.Data
Imports DTO = NGL.FreightMaster.Data.DataTransferObjects
Imports System.Data.SqlClient
Imports NGL.Core.ChangeTracker
Imports NGL.FM.BLL

<TestClass()> Public Class LaneDataTest
    Inherits TestBase

    <TestMethod()>
    Public Sub TestMethod1()
        Dim target As New DAL.NGLHDMData(testParameters)

        Dim CarrTarControl As Integer = 1634

        Dim res = target.GetTariffHDMs(CarrTarControl)

        res.Count()

    End Sub

    <TestMethod()>
    Public Sub UpdateLanePreferredCarrierTest()
        'connect to  database before getting book records overwrite the defaul
        testParameters.DBServer = "NGLRDP06D\DTMS365"
        testParameters.Database = "NGLMASCABOT"
        testParameters.UserName = "NGL\rramsey"
        Dim strTestName As String = "UpdateLanePreferredCarrierTest"

        Try

            Dim CarrierControl As Integer = 2
            Dim target As New DAL.NGLLaneData(testParameters)
            Dim iLaneControl As Integer = 24705
            Dim iLLTCControl As Integer = 2003
            Dim PreferredCarrier As New DTO.LimitLaneToCarrier()
            Dim oPrefered = target.GetLanePreferredCarriers(iLaneControl)
            Dim oCarrierDetails As New List(Of DTO.LimitLaneToCarrier)
            Dim oCarrierDetail As New DTO.LimitLaneToCarrier()
            If oPrefered.Any(Function(x) x.LLTCControl = iLLTCControl) Then
                oCarrierDetail = oPrefered.Where(Function(x) x.LLTCControl = iLLTCControl).FirstOrDefault()
            Else
                oCarrierDetail.LLTCControl = iLLTCControl
                oCarrierDetail.LimitLaneToCarrierUpdated = Nothing
            End If

            With oCarrierDetail
                .LLTCLaneControl = iLaneControl
                .LLTCCarrierControl = 21
                .LLTCCarrierNumber = 1169
                .LLTCCarrierName = "Customer PIck UP"
                .LLTCModeTypeControl = 0
                .LLTCSequenceNumber = 1
                .LLTCSActive = 1
                .LLTCTempType = 0
                .LLTCMaxCases = 0
                .LLTCMaxWgt = 0
                .LLTCMaxCube = 0
                .LLTCMaxPL = 0
                .LLTCTariffControl = 0
                .LLTCTariffName = "Ignore"
                .LLTCTariffEquip = "None"
                .LLTCMinAllowedCost = 0
                .LLTCMaxAllowedCost = 0
                .LLTCAllowAutoAssignment = 1
                .LLTCIgnoreTariff = 1
                .LLTCCarrierContControl = 332
            End With
            oCarrierDetails.Add(oCarrierDetail)
            Dim results = target.UpdateLanePreferredCarrier(PreferredCarrier)
            If results = False Then
                Assert.Fail(strTestName & " failed,  no records.")
            End If
            'System.Diagnostics.Debug.WriteLine(strTestName & " found " & results.Count() & " records.")
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & strTestName & ": {0} ", ex.Message)
        Finally
            'place clean up code here
        End Try

    End Sub

End Class