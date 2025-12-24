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
Imports CHR = Ngl.FM.CHRAPI
Imports JTS = Ngl.FM.JTSAPI
Imports UPS = Ngl.FM.UPSAPI


<TestClass()> Public Class CHRAPITest
    Inherits TestBase


    <TestMethod()>
    Public Sub TestGetTokenResponse()
        'Dim dShipDate = Date.Now.AddDays(5)
        'Dim sShipDate = String.Format("{0:yyyy-MM-ddTHH:mm:ss.FFFZ}", dShipDate.ToUniversalTime())
        'System.Diagnostics.Debug.WriteLine(sShipDate)
        'Return

        Dim oTestChrAPI As New CHR.CHRAPI()
        Dim oRes As CHR.CHRTokenData = oTestChrAPI.getToken("0oa9kmcecjIeC1Rgn357", "Wy9DFX_AQRoVlC4rMIcqUdgVcFJ0SpgcvoRnolKY", "https://inavisphere.chrobinson.com", "client_credentials")
        System.Diagnostics.Debug.WriteLine("Token: " + oRes.access_token)

    End Sub
    'getRateRequest



    <TestMethod()>
    Public Sub TestGetRateRequest()


        Dim oTestChrAPI As New CHR.CHRAPI(True)
        Dim oRes As CHR.CHRTokenData = oTestChrAPI.getToken("0oa9kmcecjIeC1Rgn357", "Wy9DFX_AQRoVlC4rMIcqUdgVcFJ0SpgcvoRnolKY", "https://inavisphere.chrobinson.com", "client_credentials")
        If Not oRes Is Nothing AndAlso Not String.IsNullOrWhiteSpace(oRes.access_token) Then
            'Dim oRet = oTestChrAPI.getRateRequest(oRes.access_token)
            'Dim oRet = oTestChrAPI.getRateRequestTest(oRes.access_token)
            Dim sCCCode As String = "C6953660" ' "C377465"
            Dim oRet = oTestChrAPI.getTestHTTPRateRequest(oRes.access_token,sCCCode)
            'System.Diagnostics.Debug.WriteLine(oRet)
        End If

    End Sub


    <TestMethod()>
    Public Sub TestJTSGetRateRequest()
        Dim oTestJTSAPI As New JTS.JTSAPI(True)
        ' Dim sToken = "d711d36b-f01b-40fd-8445-5f0730fe4db6" ' D711D36B-F01B-40FD-8445-5F0730FE4DB6
        Dim sToken = "D711D36B-F01B-40FD-8445-5F0730FE4DB6"
        'Dim sToken = "5933f1ba-1609-4b44-9619-25899efeeb77"
        'Dim oRet = oTestChrAPI.getRateRequest(oRes.access_token)
        'Dim oRet = oTestChrAPI.getRateRequestTest(oRes.access_token)
        Dim oRet = oTestJTSAPI.getTestHTTPRateRequest(sToken)
        System.Diagnostics.Debug.WriteLine(oRet)


    End Sub


    <TestMethod()>
    Public Sub TestUPsGetRateRequest()
        Dim oTestUPSAPI As New UPS.UPSAPI(True)
        'Dim sToken = "d711d36b-f01b-40fd-8445-5f0730fe4db6"
        'Dim sToken = "5933f1ba-1609-4b44-9619-25899efeeb77"
        'Dim oRet = oTestChrAPI.getRateRequest(oRes.access_token)
        'Dim oRet = oTestChrAPI.getRateRequestTest(oRes.access_token)
        Dim oRet = oTestUPSAPI.getTestHTTPRateRequest()
        System.Diagnostics.Debug.WriteLine(oRet)


    End Sub


End Class
