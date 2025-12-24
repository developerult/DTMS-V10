Imports System
Imports System.Xml
Imports System.Xml.XPath
Imports System.Xml.Linq
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports System.Text
Imports System.Web
Imports System.IO
Imports System.ServiceModel
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Ngl.FreightMaster.Data.DataTransferObjects
Imports P44 = Ngl.FM.P44




<TestClass()> Public Class P44Tests
    Inherits TestBase


    <TestMethod()>
    Public Sub TestProcessResponse()
        Dim oP44Proxy As New P44.P44Proxy("https://cloud.p-44.com", "rramsey@nextgeneration.com", "NGL2016!")
        Dim strTestName As String = "TestProcessResponse"
        Dim xDoc As New XDocument()
        Dim sb As New StringBuilder()
        Try

            sb.Append("<RateQuotes>")
            sb.Append("<rateQuote>")
            sb.Append("<mode>LTL</mode>")
            sb.Append("<scac>AACT</scac>")
            sb.Append("<vendor>AACT</vendor>")
            sb.Append("<originTerminal>")
            sb.Append("<country>US</country>")
            sb.Append("<phoneNumber>6303501688</phoneNumber>")
            sb.Append("<terminalId>DSP</terminalId>")
            sb.Append("</originTerminal>")
            sb.Append("<destinationTerminal>")
            sb.Append("<country>US</country>")
            sb.Append("<phoneNumber>8655227647</phoneNumber>")
            sb.Append("<terminalId>KNO</terminalId>")
            sb.Append("</destinationTerminal>")
            sb.Append("<errors/>")
            sb.Append("<warnings/>")
            sb.Append("<infos/>")
            sb.Append("<interLine>DIRECT</interLine>")
            sb.Append("<quoteNumber>003512315</quoteNumber>")
            sb.Append("<rateDetail>")
            sb.Append("<currency>USD</currency>")
            sb.Append("<rateAdjustments>")
            sb.Append("<rateAdjustment>")
            sb.Append("<description>Item Charge</description>")
            sb.Append("<descriptionCode>ITEM</descriptionCode>")
            sb.Append("<amount>1220.30</amount>")
            sb.Append("<rate>1.22</rate>")
            sb.Append("</rateAdjustment>")
            sb.Append("<rateAdjustment>")
            sb.Append("<description>* LESS 91.40 PCT DISCOUNT *</description>")
            sb.Append("<descriptionCode>DSC</descriptionCode>")
            sb.Append("<amount>-1115.35</amount>")
            sb.Append("<rate>1115.35</rate>")
            sb.Append("</rateAdjustment>")
            sb.Append("<rateAdjustment>")
            sb.Append("<description>Fuel Surcharge 19.88 PCT</description>")
            sb.Append("<descriptionCode>FSC</descriptionCode>")
            sb.Append("<amount>20.86</amount>")
            sb.Append("<rate>20.86</rate>")
            sb.Append("</rateAdjustment>")
            sb.Append("</rateAdjustments>")
            sb.Append("<total>125.81</total>")
            sb.Append("<transitTime>3</transitTime>")
            sb.Append("</rateDetail>")
            sb.Append("<serviceTypeCode>STD</serviceTypeCode>")
            sb.Append("<serviceType>Standard Rate</serviceType>")
            sb.Append("<totalPallets>1</totalPallets>")
            sb.Append("<totalWeight>1000</totalWeight>")
            sb.Append("<totalWeightUnit>lbs</totalWeightUnit>")
            sb.Append("<transitTime>3</transitTime>")
            sb.Append("<alternateRates>")
            sb.Append("<alternateRate>")
            sb.Append("<currency>USD</currency>")
            sb.Append("<rateAdjustments>")
            sb.Append("<rateAdjustment>")
            sb.Append("<description>Action Guaranteed Day (AGD)</description>")
            sb.Append("<descriptionCode>GUR</descriptionCode>")
            sb.Append("<amount>179.31</amount>")
            sb.Append("<rate>179.31</rate>")
            sb.Append("</rateAdjustment>")
            sb.Append("</rateAdjustments>")
            sb.Append("<total>179.31</total>")
            sb.Append("</alternateRate>")
            sb.Append("</alternateRates>")
            sb.Append("<accessorialsNotPriced/>")
            sb.Append("<missingChargeCodes/>")
            sb.Append("</rateQuote>")
            'sb.Append("<loginGroupKey>NGLOPS</loginGroupKey>")
            sb.Append("</RateQuotes>")
            xDoc = XDocument.Parse(sb.ToString())

            oP44Proxy.ProcessResponse(xDoc)


        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & strTestName & ": {0} ", ex.Message)
        Finally
            'place clean up code here
        End Try

    End Sub


    <TestMethod()>
    Public Sub Test_1ProcessResponse()
        Dim oP44Proxy As New P44.P44Proxy("https://cloud.p-44.com", "rramsey@nextgeneration.com", "NGL2016!")
        Dim strTestName As String = "TestProcessResponse"
        Dim xDoc As New XDocument()
        Dim sb As New StringBuilder()
        Try

            sb.Append("<RateQuotes>")
            sb.Append("<rateQuote>")
            sb.Append("<mode>LTL</mode>")
            sb.Append("<scac>AACT</scac>")
            sb.Append("<vendor>AACT</vendor>")
            sb.Append("<originTerminal>")
            sb.Append("<country>US</country>")
            sb.Append("<phoneNumber>6303501688</phoneNumber>")
            sb.Append("<terminalId>DSP</terminalId>")
            sb.Append("</originTerminal>")
            sb.Append("<destinationTerminal>")
            sb.Append("<country>US</country>")
            sb.Append("<phoneNumber>8655227647</phoneNumber>")
            sb.Append("<terminalId>KNO</terminalId>")
            sb.Append("</destinationTerminal>")
            sb.Append("<errors/>")
            sb.Append("<warnings/>")
            sb.Append("<infos/>")
            sb.Append("<interLine>DIRECT</interLine>")
            sb.Append("<quoteNumber>003512315</quoteNumber>")
            sb.Append("<rateDetail>")
            sb.Append("<currency>USD</currency>")
            sb.Append("<rateAdjustments>")
            sb.Append("<rateAdjustment>")
            sb.Append("<description>Item Charge</description>")
            sb.Append("<descriptionCode>ITEM</descriptionCode>")
            sb.Append("<amount>1220.30</amount>")
            sb.Append("<rate>1.22</rate>")
            sb.Append("</rateAdjustment>")
            sb.Append("<rateAdjustment>")
            sb.Append("<description>* LESS 91.40 PCT DISCOUNT *</description>")
            sb.Append("<descriptionCode>DSC</descriptionCode>")
            sb.Append("<amount>-1115.35</amount>")
            sb.Append("<rate>1115.35</rate>")
            sb.Append("</rateAdjustment>")
            sb.Append("<rateAdjustment>")
            sb.Append("<description>Fuel Surcharge 19.88 PCT</description>")
            sb.Append("<descriptionCode>FSC</descriptionCode>")
            sb.Append("<amount>20.86</amount>")
            sb.Append("<rate>20.86</rate>")
            sb.Append("</rateAdjustment>")
            sb.Append("</rateAdjustments>")
            sb.Append("<total>125.81</total>")
            sb.Append("<transitTime>3</transitTime>")
            sb.Append("</rateDetail>")
            sb.Append("<serviceTypeCode>STD</serviceTypeCode>")
            sb.Append("<serviceType>Standard Rate</serviceType>")
            sb.Append("<totalPallets>1</totalPallets>")
            sb.Append("<totalWeight>1000</totalWeight>")
            sb.Append("<totalWeightUnit>lbs</totalWeightUnit>")
            sb.Append("<transitTime>3</transitTime>")
            'sb.Append("<alternateRates />")
            sb.Append("<alternateRates>")
            sb.Append("<alternateRate>")
            sb.Append("<currency>USD</currency>")
            sb.Append("<rateAdjustments>")
            sb.Append("<rateAdjustment>")
            sb.Append("<description>Action Guaranteed Day (AGD)</description>")
            sb.Append("<descriptionCode>GUR</descriptionCode>")
            sb.Append("<amount>179.31</amount>")
            sb.Append("<rate>179.31</rate>")
            sb.Append("</rateAdjustment>")
            sb.Append("</rateAdjustments>")
            sb.Append("<total>179.31</total>")
            sb.Append("</alternateRate>")
            sb.Append("</alternateRates>")
            sb.Append("<accessorialsNotPriced/>")
            sb.Append("<missingChargeCodes/>")
            sb.Append("</rateQuote>")
            'sb.Append("<loginGroupKey>NGLOPS</loginGroupKey>")
            sb.Append("</RateQuotes>")
            xDoc = XDocument.Parse(sb.ToString())

            oP44Proxy.ProcessResponse(xDoc)


        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & strTestName & ": {0} ", ex.Message)
        Finally
            'place clean up code here
        End Try

    End Sub

End Class
