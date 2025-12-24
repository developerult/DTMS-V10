Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports NAV = NGL.FM.NAVIntegration
Imports System
Imports Tar = NGL.FM.CarTar
Imports DAL = NGL.FreightMaster.Data
Imports DTO = NGL.FreightMaster.Data.DataTransferObjects
Imports BLL = NGL.FM.BLL
Imports LTS = NGL.FreightMaster.Data.LTS
Imports TMS = NGL.FreightMaster.Integration


<TestClass()> Public Class NAVIntegrationTests
    Inherits TestBase


    <TestMethod()>
    Public Sub NAVGetSettingsDLLTest()
        Dim sSource As String = "NGL.Integration.NAVGetSettingsDLLTest"
        Dim oApp As New TMS.clsDTMSIntegration
        Dim sDBName As String = "NGLMASPROD"
        Dim sDBServer As String = "NGLDemoLapTop2" ' "NGLRDP06D"
        Dim sDBPass As String = "5529"
        Dim sDBUserName As String = "nglweb"
        Try
            Dim connectionString = "Data Source=NGLDemoLapTop2;Initial Catalog=NGLMASPROD;User ID=nglweb;Password=5529;"
            Dim oSettings = oApp.getvERPIntegrationSettingsByName("", "NAV", connectionString)
            If oSettings Is Nothing OrElse oSettings.Count() < 1 OrElse String.IsNullOrWhiteSpace(oSettings(0).TMSURI) Then
                Assert.Fail("Invalid Settings Returned ")
            End If
        Catch ex As ApplicationException
            Assert.Fail("Application Exception For " & sSource & ": {0} ", ex.Message)
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & sSource & ": {0} ", ex.Message)
        Finally

        End Try
    End Sub


    <TestMethod()>
    Public Sub NAVIntegrationDebugOrderAPData()

        Dim sSource As String = "NGL.Integration.NAVIntegrationDebugAPData"
        Dim oApp As New NAV.clsApplication()
        Dim blnHadErrors As Boolean = False
        Console.WriteLine("Prepare to Run Integration: Read config settings")

        'Dim oDefaultIntegrationConfiguration As New NAV.clsDefaultIntegrationConfiguration
        'oDefaultIntegrationConfiguration = readERPSettings()
        'Dim strCreatedDate As String = Now.ToString
        'Dim intRetValue As Integer = 0
        'Dim blnRunTest As Boolean = oDefaultIntegrationConfiguration.RunERPTest
        'Dim sDBName As String = oDefaultIntegrationConfiguration.TMSDBName
        'Dim sDBServer As String = oDefaultIntegrationConfiguration.TMSDBServer
        'Dim sDBUserName As String = oDefaultIntegrationConfiguration.TMSDBUser
        'Dim sDBPass As String = oDefaultIntegrationConfiguration.TMSDBPass
        'Dim blnDebug As Boolean = oDefaultIntegrationConfiguration.Debug
        'Dim blnVerbos As Boolean = oDefaultIntegrationConfiguration.Verbos
        'Dim sLegalEntity As String = oDefaultIntegrationConfiguration.TMSRunLegalEntity
        Try
            oApp.processAPExportDataDebug()
        Catch ex As ApplicationException
            Assert.Fail("Application Exception For " & sSource & ": {0} ", ex.Message)
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & sSource & ": {0} ", ex.Message)
        Finally

        End Try
    End Sub

    <TestMethod()>
    Public Sub NAVIntegrationDebugOrderData()
        'Dim sSource As String = "NGL.Integration.NAVIntegrationDebugOrderData"
        'Dim oApp As New NAV.clsApplication
        'Dim oDefaultSettings As New NAV.clsDefaultIntegrationConfiguration
        ''http://NGLNAV2013R2:7557/DynamicsTMS704/WS/CRONUS%20USA%2C%20Inc./Codeunit/DynamicsTMSWebServices
        ''http://07DWS.nextgeneration.com/DTMSERPIntegration.asmx
        ''http://07DWS.nextgeneration.com/DTMSIntegration.asmx
        ''"http://NGLWSDEV705.NEXTGENERATION.COM/DTMSIntegration.asmx"
        'With oDefaultSettings
        '    .TMSDBName = "nglmasprod"
        '    .TMSDBServer = "nglrdp07d"
        '    .TMSDBPass = "5529"
        '    .TMSDBUser = "nglweb"
        '    .TMSRunLegalEntity = "" 'we use all legal entityes and check the config tables in tms for NAV integration settings
        '    .TMSSettingsURI = "http://07DWS.nextgeneration.com/DTMSIntegration.asmx"
        '    .TMSSettingsAuthCode = "WSPROD"
        '    .TMSSettingsAuthPassword = ""
        '    .TMSSettingsAuthUser = ""
        '    .Verbos = True
        '    .Debug = True
        '    .RunERPTest = False
        'End With
        Dim sSource As String = "NGL.Integration.NAVIntegrationDebugOrderData"
        Dim oApp As New NAV.clsApplication()
        Dim blnHadErrors As Boolean = False
        Console.WriteLine("Prepare to Run Integration: Read config settings")

        Dim oDefaultIntegrationConfiguration As New NAV.clsDefaultIntegrationConfiguration
        oDefaultIntegrationConfiguration = readERPSettings()
        Dim strCreatedDate As String = Now.ToString
        Dim intRetValue As Integer = 0
        Dim blnRunTest As Boolean = oDefaultIntegrationConfiguration.RunERPTest
        Dim sDBName As String = oDefaultIntegrationConfiguration.TMSDBName
        Dim sDBServer As String = oDefaultIntegrationConfiguration.TMSDBServer
        Dim sDBUserName As String = oDefaultIntegrationConfiguration.TMSDBUser
        Dim sDBPass As String = oDefaultIntegrationConfiguration.TMSDBPass
        Dim blnDebug As Boolean = oDefaultIntegrationConfiguration.Debug
        Dim blnVerbos As Boolean = oDefaultIntegrationConfiguration.Verbos
        Dim sLegalEntity As String = oDefaultIntegrationConfiguration.TMSRunLegalEntity
        Try
            oApp.DebugOrderData(sSource, oDefaultIntegrationConfiguration)
        Catch ex As ApplicationException
            Assert.Fail("Application Exception For " & sSource & ": {0} ", ex.Message)
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & sSource & ": {0} ", ex.Message)
        Finally

        End Try
    End Sub


    <TestMethod()>
    Public Sub NAVIntegrationRemoteSystemTest()
        Dim sSource As String = "NGL.Integration.NAVIntegrationRemoteSystemTest"
        Dim oApp As New NAV.clsApplication
        Dim oDefaultSettings As New NAV.clsDefaultIntegrationConfiguration
        With oDefaultSettings
            '.TMSDBName = "NGLMASDEVNAV2017" ' "NGLMASPROD"
            '.TMSDBServer = "NGLRDP06D\DTMS365" ' "NGLDemoLapTop2"
            .TMSDBName = "NGLMASD365"
            .TMSDBServer = "nglsql03p"
            .TMSDBPass = "5529"
            .TMSDBUser = "nglweb"
            .TMSRunLegalEntity = "" 'we use all legal entityes and check the config tables in tms for NAV integration settings

            '.TMSSettingsURI = "https://www.dynamicstms365.com/ws/DTMSIntegration.asmx"
            .TMSSettingsURI = "https://www.dynamicstms365.com/ws/DTMSIntegration.asmx"
            .TMSSettingsAuthCode = "WSPROD"
            .TMSSettingsAuthPassword = ""
            .TMSSettingsAuthUser = ""
            .Verbos = True
            .Debug = True
            .RunERPTest = False
            .ERPTypeName = "Dynamics BC"
        End With
        Try
            oApp.ProcessData(sSource, oDefaultSettings)
        Catch ex As ApplicationException
            Assert.Fail("Application Exception For " & sSource & ": {0} ", ex.Message)
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & sSource & ": {0} ", ex.Message)
        Finally

        End Try
    End Sub

    <TestMethod()>
    Public Sub NAVIntegrationNormalTest()
        Dim sSource As String = "NGL.Integration.NAVIntegrationNormalTest"
        Dim oApp As New NAV.clsApplication
        Dim sDBName As String = "NGLMASPROD"
        Dim sDBServer As String = "NGLRDP07D"
        Dim sDBPass As String = "5529"
        Dim sDBUserName As String = "nglweb"
        Try
            'Dim reg As New Ngl.Core.Security.RegistrySettings("DTMS")
            'Try
            '    reg.checkRegistryStatus()
            '    If reg.IsDTMSRegistrysAvailable Then
            '        sDBName = reg.DBName
            '        sDBServer = reg.DBServer
            '        sDBPass = reg.DBPass
            '        sDBUserName = reg.DBUser
            '    Else
            '        Assert.Fail("Database Settings are not availalble For " & sSource & ".  Install the TMS Software. Or run as admin. " & reg.RegisterStatus.Message)
            '    End If
            'Catch ex As Exception
            '    If Not reg Is Nothing AndAlso Not reg.RegisterStatus Is Nothing AndAlso Not String.IsNullOrWhiteSpace(reg.RegisterStatus.Message) Then
            '        Assert.Fail("Unexpected Database Settings ExceptionFor " & sSource & ":" & reg.RegisterStatus.Message & " " & ex.Message)
            '    Else
            '        Assert.Fail("Unexpected Database Settings Exception For " & sSource & ": " & ex.Message)
            '    End If
            'End Try
            Dim oDefaultSettings As New NAV.clsDefaultIntegrationConfiguration
            With oDefaultSettings
                .TMSDBName = sDBName
                .TMSDBServer = sDBServer
                .TMSDBPass = sDBPass
                .TMSDBUser = sDBUserName
                .TMSRunLegalEntity = "" 'we use all legal entityes and check the config tables in tms for NAV integration settings
                .TMSSettingsURI = "http://07dWS.nextgeneration.com/DTMSIntegration.asmx"
                .TMSSettingsAuthCode = "WSPROD"
                .TMSSettingsAuthPassword = ""
                .TMSSettingsAuthUser = ""
                .Verbos = True
                .Debug = True
                .RunERPTest = False
            End With
            oApp.ProcessData(sSource, oDefaultSettings)
        Catch ex As ApplicationException
            Assert.Fail("Application Exception For " & sSource & ": {0} ", ex.Message)
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & sSource & ": {0} ", ex.Message)
        Finally

        End Try
    End Sub


    <TestMethod()>
    Public Sub NAVIntegrationProcessDataUnitTest()
        Dim sSource As String = "NGL.Integration.NAVIntegrationNormalTest"
        Dim oApp As New NAV.clsApplication

        Try

            'Dim oDefaultSettings As New NAV.clsDefaultIntegrationConfiguration
            'With oDefaultSettings
            '    .TMSDBName = sDBName
            '    .TMSDBServer = sDBServer
            '    .TMSDBPass = sDBPass
            '    .TMSDBUser = sDBUserName
            '    .TMSRunLegalEntity = "" 'we use all legal entityes and check the config tables in tms for NAV integration settings
            '    .TMSSettingsURI = "http://07dWS.nextgeneration.com/DTMSIntegration.asmx"
            '    .TMSSettingsAuthCode = "WSPROD"
            '    .TMSSettingsAuthPassword = ""
            '    .TMSSettingsAuthUser = ""
            '    .Verbos = True
            '    .Debug = True
            '    .RunERPTest = False
            'End With
            Dim oDefaultIntegrationConfiguration As New NAV.clsDefaultIntegrationConfiguration
            Dim sFile As String = "C:\Data\IntegrationServicesData\BC Sandbox\NAVServiceSettings.xml"
            oDefaultIntegrationConfiguration = readERPSettings(sFile)
            Dim strCreatedDate As String = Now.ToString
            Dim intRetValue As Integer = 0
            Dim blnRunTest As Boolean = oDefaultIntegrationConfiguration.RunERPTest
            Dim sDBName As String = oDefaultIntegrationConfiguration.TMSDBName
            Dim sDBServer As String = oDefaultIntegrationConfiguration.TMSDBServer
            Dim sDBUserName As String = oDefaultIntegrationConfiguration.TMSDBUser
            Dim sDBPass As String = oDefaultIntegrationConfiguration.TMSDBPass
            Dim blnDebug As Boolean = oDefaultIntegrationConfiguration.Debug
            Dim blnVerbos As Boolean = oDefaultIntegrationConfiguration.Verbos
            Dim sLegalEntity As String = oDefaultIntegrationConfiguration.TMSRunLegalEntity

            oApp.ProcessData(sSource, oDefaultIntegrationConfiguration)
        Catch ex As ApplicationException
            Assert.Fail("Application Exception For " & sSource & ": {0} ", ex.Message)
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & sSource & ": {0} ", ex.Message)
        Finally

        End Try
    End Sub



    <TestMethod()>
    Public Sub NAVIntegrationProcessPickListUnitTest()
        Dim sSource As String = "NGL.Integration.NAVIntegrationTests.NAVIntegrationProcessPickListUnitTest"
        Dim oApp As New NAV.clsApplication
        Dim sDBName As String = "NGLMASPROD"
        Dim sDBServer As String = "NGLRDP07D"
        Dim sDBPass As String = "5529"
        Dim sDBUserName As String = "nglweb"

        Dim sLegalEntity As String = "CRONUS USA, Inc." 'test legal entity for CRONUS DB

        Try
            Dim oTestSettings As New NAV.clsUnitTestKeys With {.LegalEntity = sLegalEntity,
                                                           .CompName = "Yellow Warehouse",
                                                           .CompNumber = 9,
                                                           .CompAlphaCode = "YELLOW",
                                                           .CompAbrev = "YEL",
                                                           .CarrierName = "",
                                                           .CarrierNumber = 0,
                                                           .CarrierAlphaCode = "",
                                                           .LaneName = "",
                                                           .LaneNumber = "",
                                                           .OrderNumber = "",
                                                           .Source = sSource,
                                                           .Verbos = True,
                                                           .Debug = True,
                                                           .WCFAuthCode = "WCFPROD",
                                                           .WCFURL = "http://07dWCF.nextgeneration.com",
                                                           .WCFTCPURL = "net.tcp://07dWCF.nextgeneration.com:908",
                                                           .WSAuthCode = "WSPROD",
                                                           .WSURL = "http://07dWS.nextgeneration.com",
                                                           .DBName = sDBName,
                                                           .DBServer = sDBServer}

            'Dim strCreateUser As String = "NAV Integration Unit Test"
            'Dim strCreatedDate As String = Now.ToString
            'Dim intRetValue As Integer = 0

            oApp.processPicklistUnitTest(oTestSettings)


        Catch ex As ApplicationException
            Assert.Fail("Application Exception For " & sSource & ": {0} ", ex.Message)
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & sSource & ": {0} ", ex.Message)
        Finally


        End Try
    End Sub

    <TestMethod()>
    Public Sub NAVIntegrationUnitTestAlpha()
        Dim sSource As String = "NGL.Integration.NAVIntegrationUnitTestAlpha"
        Dim oApp As New NAV.clsApplication
        Dim sDBName As String = "NGLMAS2013DEV"
        Dim sDBServer As String = "NGLRDP06D"
        Dim sLegalEntity As String = "NAV_Unit_Test_Alpha"
        Dim oTestSettings As New NAV.clsUnitTestKeys With {.LegalEntity = sLegalEntity, _
                                                           .CompName = "NAV_Unit_Test_Company_Alpha", _
                                                           .CompNumber = 0, _
                                                           .CompAlphaCode = "NAV_UTCompA1", _
                                                           .CompAbrev = "NUT", _
                                                           .CarrierName = "NAV_Unit_Test_Carrier_Alpha", _
                                                           .CarrierNumber = 0, _
                                                           .CarrierAlphaCode = "NAV_UTCarrA1", _
                                                           .LaneName = "NAV_Unit_Test_Lane_Alpha", _
                                                           .LaneNumber = "NAVUTA-NAVUTL1A", _
                                                           .OrderNumber = "NAVUTA-1111", _
                                                           .Source = sSource, _
                                                           .Verbos = True, _
                                                           .Debug = True, _
                                                           .WCFAuthCode = "WCFDEV", _
                                                           .WCFURL = "http://nglwcfdev704.nextgeneration.com", _
                                                           .WCFTCPURL = "net.tcp://nglwcfdev704.nextgeneration.com:908", _
                                                           .WSAuthCode = "WSDEV", _
                                                           .WSURL = "http://nglwsdev704.nextgeneration.com", _
                                                           .DBName = sDBName, _
                                                           .DBServer = sDBServer}

        Try

            Dim intRetValue As Integer = 0
            With oApp
                If Not .ProcessDataUnitTest(oTestSettings) Then
                    Assert.Fail("ProcessDataUnitTest Returned False; the following error was returned: " & .LastError)
                End If
            End With
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & sSource & ": {0} ", ex.Message)
        Finally
            'place clean up code here

        End Try
    End Sub

    <TestMethod()>
    Public Sub NAVIntegrationUnitTestNumeric()
        Dim sSource As String = "NGL.Integration.NAVIntegrationUnitTestNumeric"
        Dim oApp As New NAV.clsApplication
        Dim sDBName As String = "NGLMAS2013DEV"
        Dim sDBServer As String = "NGLRDP06D"
        Dim sLegalEntity As String = "NAV_Unit_Test_Numeric"
        Dim oTestSettings As New NAV.clsUnitTestKeys With {.LegalEntity = sLegalEntity,
                                                           .CompName = "NAV_Unit_Test_Company_Numeric",
                                                           .CompNumber = 9999999,
                                                           .CompAlphaCode = "",
                                                           .CompAbrev = "NUT",
                                                           .CarrierName = "NAV_Unit_Test_Carrier_Numeric",
                                                           .CarrierNumber = 9999999,
                                                           .CarrierAlphaCode = "",
                                                           .LaneName = "NAV_Unit_Test_Lane_Numeric",
                                                           .LaneNumber = "NAVUTN-NAVUTL1N",
                                                           .OrderNumber = "NAVUTN-1111",
                                                           .Source = sSource,
                                                           .Verbos = True,
                                                           .Debug = True,
                                                           .WCFAuthCode = "WCFDEV",
                                                           .WCFURL = "http://nglwcfdev704.nextgeneration.com",
                                                           .WCFTCPURL = "net.tcp://nglwcfdev704.nextgeneration.com:908",
                                                           .WSAuthCode = "WSDEV",
                                                           .WSURL = "http://nglwsdev704.nextgeneration.com",
                                                           .DBName = sDBName,
                                                           .DBServer = sDBServer}

        Try

            Dim intRetValue As Integer = 0
            With oApp
                If Not .ProcessDataUnitTest(oTestSettings, True, True) Then
                    Assert.Fail(.LastError)
                End If
            End With
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As ApplicationException
            Assert.Fail(sSource & " Failed: {0} ", ex.Message)
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & sSource & ": {0} ", ex.Message)
        Finally
            'place clean up code here

        End Try
    End Sub


    <TestMethod()>
    Public Sub NAVIntegrationUnitTestNumericcopy()
        Dim sSource As String = "NGL.Integration.NAVIntegrationUnitTestNumeric"
        Dim oApp As New NAV.clsApplication
        Dim sDBName As String = "DESKTOP-0R0EJUB"
        Dim sDBServer As String = "NGLMASPROD"
        Dim sLegalEntity As String = "Demo Nestle"
        Dim wcfServiceURL = "http://localhost:56533"
        Dim webServiceURL = "http://localhost:56526"
        Dim oTestSettings As New NAV.clsUnitTestKeys With {.LegalEntity = sLegalEntity,
                                                           .CompName = "Wholesum Foods",
                                                           .CompNumber = 1000,
                                                           .CompAlphaCode = "",
                                                           .CompAbrev = "DMO",
                                                           .CarrierName = "Unit_Test_Carrier_Numeric",
                                                           .CarrierNumber = 30100,
                                                           .CarrierAlphaCode = "",
                                                           .LaneName = "DEMO-Oakland Container",
                                                           .LaneNumber = "DEMO-WHL-OAK",
                                                           .OrderNumber = "NAVUTN-1111",
                                                           .Source = sSource,
                                                           .Verbos = True,
                                                           .Debug = True,
                                                           .WCFAuthCode = "WCFPROD",
                                                           .WCFURL = wcfServiceURL,
                                                           .WCFTCPURL = "net.tcp://localhost:908",
                                                           .WSAuthCode = "NGLWSTEST",
                                                           .WSURL = webServiceURL,
                                                           .DBName = sDBName,
                                                           .DBServer = sDBServer}

        Try

            Dim intRetValue As Integer = 0
            With oApp
                If Not .ProcessDataUnitTest(oTestSettings, False, True) Then
                    Assert.Fail(.LastError)
                End If
            End With
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As ApplicationException
            Assert.Fail(sSource & " Failed: {0} ", ex.Message)
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & sSource & ": {0} ", ex.Message)
        Finally
            'place clean up code here

        End Try
    End Sub

    Protected Function GetNAVTMSTestSettings(ByVal sDTMSLegalEntity As String,
                                            ByVal sDTMSNAVWebServiceURL As String,
                                            ByVal sDTMSNAVUserName As String,
                                            ByVal sDTMSNAVPassword As String,
                                            Optional ByVal iDTMSPicklistMaxRetry As Integer = 1,
                                            Optional ByVal iDTMSPicklistRetryMinutes As Integer = 30,
                                            Optional ByVal iDTMSPicklistMaxRowsReturned As Integer = 10,
                                            Optional ByVal bDTMSPicklistAutoConfirmation As Boolean = False,
                                            Optional ByVal iDTMSAPExportMaxRetry As Integer = 5,
                                            Optional ByVal iDTMSAPExportRetryMinutes As Integer = 30,
                                            Optional ByVal iDTMSAPExportMaxRowsReturned As Integer = 10,
                                            Optional ByVal bDTMSAPExportAutoConfirmation As Boolean = False,
                                            Optional ByVal bDTMSNAVUseDefaultCredentials As Boolean = True,
                                            Optional ByVal sDTMSWSAuthCode As String = "WSDEV",
                                            Optional ByVal sDTMSWSURL As String = "http://nglwsdev704.nextgeneration.com",
                                            Optional ByVal sDTMSWCFAuthCode As String = "WCFDEV",
                                            Optional ByVal sDTMSWCFURL As String = "http://nglwcfdev704.nextgeneration.com",
                                            Optional ByVal sDTMSWCFTCPURL As String = "net.tcp://nglwcfdev704.nextgeneration.com:908") As DTO.DynamicsTMSSetting



        Return New DTO.DynamicsTMSSetting With {.DTMSLegalEntity = sDTMSLegalEntity _
                                                         , .DTMSPicklistMaxRetry = iDTMSPicklistMaxRetry _
                                                         , .DTMSPicklistRetryMinutes = iDTMSPicklistRetryMinutes _
                                                         , .DTMSPicklistMaxRowsReturned = iDTMSPicklistMaxRowsReturned _
                                                         , .DTMSPicklistAutoConfirmation = bDTMSPicklistAutoConfirmation _
                                                         , .DTMSAPExportMaxRetry = iDTMSAPExportMaxRetry _
                                                         , .DTMSAPExportRetryMinutes = iDTMSAPExportRetryMinutes _
                                                         , .DTMSAPExportMaxRowsReturned = iDTMSAPExportMaxRowsReturned _
                                                         , .DTMSAPExportAutoConfirmation = bDTMSAPExportAutoConfirmation _
                                                         , .DTMSNAVWebServiceURL = sDTMSNAVWebServiceURL _
                                                         , .DTMSNAVUserName = sDTMSNAVUserName _
                                                         , .DTMSNAVPassword = sDTMSNAVPassword _
                                                         , .DTMSNAVUseDefaultCredentials = bDTMSNAVUseDefaultCredentials _
                                                         , .DTMSWSAuthCode = sDTMSWSAuthCode _
                                                         , .DTMSWSURL = sDTMSWSURL _
                                                         , .DTMSWCFAuthCode = sDTMSWCFAuthCode _
                                                         , .DTMSWCFURL = sDTMSWCFURL _
                                                         , .DTMSWCFTCPURL = sDTMSWCFTCPURL}

    End Function



    <TestMethod()>
    Public Sub NAVIntegrationSilentTenderTest()
        Dim sSource As String = "NGL.Integration.NAVIntegrationSilentTenderTest"
        Dim DBServer As String = "NGLRDP07D"
        Dim Database As String = "NGLMASPRODBON"
        Dim oBookIntegration As New TMS.clsBook
        Dim connectionString As String = String.Format("Server={0}; Database={1}; Integrated Security=SSPI", DBServer, Database)

        Try
            With oBookIntegration
                .AdminEmail = "rramsey@nextgeneration.com"
                .FromEmail = "rramsey@nextgeneration.com"
                .GroupEmail = "rramsey@nextgeneration.com"
                .Retry = 1
                .SMTPServer = "nglmail.nextgeneration.com"
                .DBServer = DBServer
                .Database = Database
                .ConnectionString = connectionString
                .Debug = True
                .AuthorizationCode = "WSDEV"
                .WCFAuthCode = "WCFDEV"
                .WCFURL = "http://nglwcfdev704.nextgeneration.com"
                .WCFTCPURL = "net.tcp://nglwcfdev704.nextgeneration.com:908"
                .mstrOrderNumbers = New List(Of String)
                .mstrOrderNumbers.Add("273721")
                .mstrOrderNumbers.Add("273516")
                .mstrOrderNumbers.Add("273752")
                .mstrOrderNumbers.Add("273883")
                .mstrOrderNumbers.Add("274208")
                .mstrOrderNumbers.Add("273631")
                .mstrOrderNumbers.Add("273687")
                .mstrOrderNumbers.Add("273724")
                .mstrOrderNumbers.Add("273755")
                .mstrOrderNumbers.Add("273819")
                .mstrOrderNumbers.Add("273898")
                .mstrOrderNumbers.Add("274077")
                .mstrOrderNumbers.Add("27")
                '.mstrOrderNumbers.Add("P106056")
                .silentImportProcessExecAsync()
                '.ProcessSilentTenders()
            End With

        Catch ex As ApplicationException
            Assert.Fail("Application Exception For " & sSource & ": {0} ", ex.Message)
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & sSource & ": {0} ", ex.Message)
        Finally

        End Try

    End Sub

    'http://10.0.0.185:7047/DynamicsNAV100/WS/CRONUS%20USA%2C%20Inc./Codeunit/DynamicsTMSWebServices


    ''' <summary>
    ''' Returns the settings from the ERPSettingsFile or the Application Settings if the file does not exist
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function readERPSettings(Optional ByVal sFile As String = "C:\Data\IntegrationServicesData\TEST\NAVServiceSettings.xml") As NAV.clsDefaultIntegrationConfiguration

        Dim xSettings As New NAV.clsDefaultIntegrationConfiguration
        'Dim sFile As String = "C:\Data\IntegrationServicesData\TEST\NAVServiceSettings.xml"
        'check if the file exists:
        If System.IO.File.Exists(sFile) Then
            Dim doc As XDocument = XDocument.Load(sFile)
            xSettings = (From s In doc.Root.Elements("Setting")
                         Select New NAV.clsDefaultIntegrationConfiguration With
                                {
                                    .ERPTestURI = s.Element("ERPTestURI"),
                                    .ERPTestAuthUser = s.Element("ERPTestAuthUser"),
                                    .ERPTestAuthPassword = s.Element("ERPTestAuthPassword"),
                                    .ERPTestAuthCode = s.Element("ERPTestAuthCode"),
                                    .ERPTestLegalEntity = s.Element("ERPTestLegalEntity"),
                                    .ERPTestExportMaxRetry = getXMLInteger(s.Element("ERPTestExportMaxRetry")),
                                    .ERPTestExportRetryMinutes = getXMLInteger(s.Element("ERPTestExportRetryMinutes")),
                                    .ERPTestExportMaxRowsReturned = getXMLInteger(s.Element("ERPTestExportMaxRowsReturned")),
                                    .ERPTestExportAutoConfirmation = getXMLBool(s.Element("ERPTestExportAutoConfirmation")),
                                    .ERPTestFreightCost = getXMLDouble(s.Element("ERPTestFreightCost")),
                                    .ERPTestFreightBillNumber = s.Element("ERPTestFreightBillNumber"),
                                    .TMSSettingsURI = s.Element("TMSSettingsURI"),
                                    .TMSSettingsAuthCode = s.Element("TMSSettingsAuthCode"),
                                    .TMSSettingsAuthUser = s.Element("TMSSettingsAuthUser"),
                                    .TMSSettingsAuthPassword = s.Element("TMSSettingsAuthPassword"),
                                    .TMSTestServiceURI = s.Element("TMSTestServiceURI"),
                                    .TMSTestServiceAuthCode = s.Element("TMSTestServiceAuthCode"),
                                    .TMSTestServiceAuthPassword = s.Element("TMSTestServiceAuthPassword"),
                                    .TMSTestServiceAuthUser = s.Element("TMSTestServiceAuthUser"),
                                    .RunERPTest = getXMLBool(s.Element("RunERPTest")),
                                    .Debug = getXMLBool(s.Element("Debug")),
                                    .Verbos = getXMLBool(s.Element("Verbos")),
                                    .TMSDBName = s.Element("TMSDBName"),
                                    .TMSDBServer = s.Element("TMSDBServer"),
                                    .TMSDBUser = s.Element("TMSDBUser"),
                                    .TMSDBPass = s.Element("TMSDBPass"),
                                    .TMSRunLegalEntity = s.Element("TMSRunLegalEntity"),
                                    .ERPTypeName = s.Element("ERPTypeName")
                                }
                                ).FirstOrDefault()


        End If
        With xSettings
            Console.WriteLine("ERPTestURI: " & .ERPTestURI)
            Console.WriteLine("ERPTestAuthUser: " & .ERPTestAuthUser)
            Console.WriteLine("ERPTestAuthPassword: " & .ERPTestAuthPassword)
            Console.WriteLine("ERPTestAuthCode: " & .ERPTestAuthCode)
            Console.WriteLine("ERPTestLegalEntity: " & .ERPTestLegalEntity)
            Console.WriteLine("ERPTestExportMaxRetry: " & .ERPTestExportMaxRetry)
            Console.WriteLine("ERPTestExportRetryMinutes: " & .ERPTestExportRetryMinutes)
            Console.WriteLine("ERPTestExportMaxRowsReturned: " & .ERPTestExportMaxRowsReturned)
            Console.WriteLine("ERPTestExportAutoConfirmation: " & .ERPTestExportAutoConfirmation)
            Console.WriteLine("ERPTestFreightCost: " & .ERPTestFreightCost)
            Console.WriteLine("ERPTestFreightBillNumber: " & .ERPTestFreightBillNumber)
            Console.WriteLine("TMSSettingsURI: " & .TMSSettingsURI)
            Console.WriteLine("TMSSettingsAuthCode: " & .TMSSettingsAuthCode)
            Console.WriteLine("TMSSettingsAuthUser: " & .TMSSettingsAuthUser)
            Console.WriteLine("TMSSettingsAuthPassword: " & .TMSSettingsAuthPassword)
            Console.WriteLine("TMSTestServiceURI: " & .TMSTestServiceURI)
            Console.WriteLine("TMSTestServiceAuthCode: " & .TMSTestServiceAuthCode)
            Console.WriteLine("TMSTestServiceAuthPassword: " & .TMSTestServiceAuthPassword)
            Console.WriteLine("TMSTestServiceAuthUser: " & .TMSTestServiceAuthUser)
            Console.WriteLine("RunERPTest: " & .RunERPTest)
            Console.WriteLine("Debug: " & .Debug)
            Console.WriteLine("Verbos: " & .Verbos)
            Console.WriteLine("TMSDBName: " & .TMSDBName)
            Console.WriteLine("TMSDBServer: " & .TMSDBServer)
            Console.WriteLine("TMSDBUser: " & .TMSDBUser)
            Console.WriteLine("TMSDBPass: " & .TMSDBPass)
            Console.WriteLine("TMSRunLegalEntity: " & .TMSRunLegalEntity)
            Console.WriteLine("ERPTypeName: " & .ERPTypeName)
        End With

        Return xSettings

    End Function


End Class
