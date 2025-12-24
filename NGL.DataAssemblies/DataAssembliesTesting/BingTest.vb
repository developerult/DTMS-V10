Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports System
Imports DAL = Ngl.FreightMaster.Data
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports BLL = Ngl.FM.BLL
Imports LTS = Ngl.FreightMaster.Data.LTS
Imports Bing = Ngl.FM.Bing

Imports Ngl.Core.Utility

<TestClass()> Public Class BingTest
    Inherits TestBase

    <TestMethod()> Public Sub GeoCodeTest()
        'Bing.Class1.GeoCodeTest()
        'testParameters.DBServer = "NGLRDP07D"
        'testParameters.Database = "NGLMASPROD"
        'testParameters.UserControl = 8
        'Dim oBT = New DAL.NGLBookTrackData(testParameters)
        'oBT.UpdateLatLongFromAddressBing(29, "", "Chicago", "IL", "60601", "US")
    End Sub

    <TestMethod()> Public Sub GeoCodeAddressTest()
        testParameters.DBServer = "NGLRDP07D"
        testParameters.Database = "NGLMASPROD"
        testParameters.UserControl = 8
        Dim oSec = New DAL.NGLtblSingleSignOnAccountData(testParameters)
        Dim oStop = New DAL.NGLtblStopData(testParameters)
        Dim key = ""
        Dim SSOA = oSec.GetSingleSignOnAccountByUser(testParameters.UserControl, DAL.Utilities.SSOAAccount.BingMaps) 'Get the BingMapsKey
        For Each s In SSOA
            Dim sVals = s.TryGetKeys({"RefID"}, {""}) 'Modified by RHR for v-8.2 12/29/2018 to simplify reading of WCFResults keys 
            key = sVals(0)
        Next

        'Dim address1 = "", city = "", state = "", zip = "60601", country = ""
        Dim address1 = "6101 West Gross Point Road", city = "Niles", state = "IL", zip = "60714", country = "US"

        Dim gc = Bing.BingLocationAPI.GeoCodeAddress(address1, city, state, zip, country, key)

        If gc Is Nothing Then Assert.Fail("Fail")

    End Sub

End Class