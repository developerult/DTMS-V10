Imports System.Data.SqlClient
Imports System.Data
Imports DTO = NGL.FreightMaster.Data.DataTransferObjects
Imports NAV = NGL.FM.NAVIntegration
Imports System.Xml.Linq

Module Module1

    Sub Main()
        Try
            processData()
        Catch ex As Exception
            Console.WriteLine("Error: " & ex.ToString)
        Finally
            Console.WriteLine("Press Enter To Continue")
            Console.ReadLine()
        End Try

    End Sub


    Private Function processCompanyData(ByVal TMSSetting As NAV.clsDefaultIntegrationConfiguration) As Boolean
        Dim blnRet As Boolean = False
        If TMSSetting Is Nothing OrElse String.IsNullOrWhiteSpace(TMSSetting.TMSTestServiceURI) OrElse String.IsNullOrWhiteSpace(TMSSetting.ERPTestURI) Then
            Console.WriteLine("Missing TMS Integration settings for Company; nothing to do returning false")
            Return False
        End If
        Try

            Console.WriteLine("Begin Process Company Data ")
            Dim strMsg As String = ""
            Dim ReturnMessage As String = ""
            Dim RetVal As Integer = 0
            Dim oNavCompany = New NAVService.Company
            Dim oNavCompanies As New NAVService.DynamicsTMSCompanies()
            Dim oNAVWebService = New NAVService.DynamicsTMSWebServices()
            oNAVWebService.Url = TMSSetting.ERPTestURI
            oNAVWebService.UseDefaultCredentials = True
            Dim oComp As New Object()
            'oNAVWebService.GetCompanies(oNavCompanies, True, False)
            oNAVWebService.GetCompanies(oComp, True, False)
            If Not oComp Is Nothing Then
                oNavCompanies = CType(oComp, NAVService.DynamicsTMSCompanies)
            Else
                Console.WriteLine("oComp is nothing")
                Return True 'not ready yet so just return true
            End If
            If oNavCompanies Is Nothing OrElse oNavCompanies.Company Is Nothing OrElse oNavCompanies.Company.Count() < 1 Then
                Console.WriteLine("No company Data")
                Return True 'not ready yet so just return true
            End If
            For Each c In oNavCompanies.Company
                If Not c Is Nothing Then
                    Console.WriteLine("Company Found: " & c.CompName)
                    Console.WriteLine("Company Number: " & c.CompNumber)
                    Console.WriteLine("Company Alpha: " & c.CompAlphaCode)
                    Console.WriteLine("Company Abrev: " & c.CompAbrev)
                    Console.WriteLine("Company Legal Entity: " & c.CompLegalEntity)
                Else
                    Console.WriteLine("Missing company Record at 'c'")
                End If
            Next
            blnRet = True
        Catch ex As Exception
            Console.WriteLine("Error!  Cannot Read Company Data: " & ex.ToString())
        End Try

        Return blnRet
    End Function


    Private Function processCarrierData(ByVal TMSSetting As NAV.clsDefaultIntegrationConfiguration) As Boolean
        Dim blnRet As Boolean = False
        If TMSSetting Is Nothing OrElse String.IsNullOrWhiteSpace(TMSSetting.TMSTestServiceURI) OrElse String.IsNullOrWhiteSpace(TMSSetting.ERPTestURI) Then
            Console.WriteLine("Missing TMS Integration settings for Carrier; nothing to do returning false")
            Return False
        End If
        Try

            Console.WriteLine("Begin Process Carrier Data ")
            Dim strMsg As String = ""
            Dim ReturnMessage As String = ""
            Dim RetVal As Integer = 0

            Dim oNAVWebService = New NAVService.DynamicsTMSWebServices()
            oNAVWebService.Url = TMSSetting.ERPTestURI
            oNAVWebService.UseDefaultCredentials = True

            Dim oNavCarriers = New NAVService.DynamicsTMSCarriers()
            oNAVWebService.GetCarriers(oNavCarriers, True, False)
            Dim strSkip As New List(Of String)
            If oNavCarriers Is Nothing OrElse oNavCarriers.Carrier Is Nothing OrElse oNavCarriers.Carrier.Count() < 1 Then
                Console.WriteLine("No on Carrier Data")
                Return True 'not ready yet so just return true
            End If
            For Each c In oNavCarriers.Carrier
                If Not c Is Nothing Then
                    Console.WriteLine("Carrier Found: " & c.CarrierName)
                    Console.WriteLine("Carrier Number: " & c.CarrierNumber)
                    Console.WriteLine("Carrier Alpha: " & c.CarrierAlphaCode)
                    Console.WriteLine("Carrier Legal Entity: " & c.CarrierLegalEntity)
                Else
                    Console.WriteLine("Missing Carrier Record at 'c'")
                End If
            Next

            blnRet = True
        Catch ex As Exception
            Console.WriteLine("Error!  Cannot Read Carrier Data: " & ex.ToString())
        End Try

        Return blnRet
    End Function



    Private Sub processData()
        Dim sSource As String = "NGL.FM.NAVIntegrationTester"
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
        Console.WriteLine(String.Format("Settings: DBName {0} DBServer {1} DBUser {2} DBPass {3}", sDBName, sDBServer, sDBUserName, sDBPass))
        'ProcessERPTestingStatus(oDefaultIntegrationConfiguration)
        'processCompanyData(oDefaultIntegrationConfiguration)
        'processCarrierData(oDefaultIntegrationConfiguration)

        With oApp
            .Source = sSource
            If blnRunTest Then
                Console.WriteLine("Running Integration Test")
                .ProcessDataTest(sSource, oDefaultIntegrationConfiguration)
            Else
                Console.WriteLine("Processing Data")
                .ProcessData(sSource, oDefaultIntegrationConfiguration)
            End If
        End With
        Console.WriteLine("Process Data Complete")
    End Sub


    Private Sub ProcessERPTestingStatus(ByVal TMSSetting As NAV.clsDefaultIntegrationConfiguration)
        'set the defalt to false
        Dim ERPTestingOn As Boolean = False
        If TMSSetting Is Nothing Then Return
        Try
            Console.WriteLine("Begin Read ERP Testing Flag ")
            Dim oNAVWebService = New NAVService.DynamicsTMSWebServices()
            oNAVWebService.Url = TMSSetting.ERPTestURI
            oNAVWebService.UseDefaultCredentials = True

            ERPTestingOn = oNAVWebService.GetTestingStatus()
            Console.WriteLine("ERP Testing Flag = " & ERPTestingOn.ToString())
        Catch ex As Exception
            Console.WriteLine("Error!  Cannot Read ERP Testing Flag: " & ex.ToString())
        End Try
    End Sub


    ''' <summary>
    ''' Returns the settings from the ERPSettingsFile or the Application Settings if the file does not exist
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function readERPSettings() As NAV.clsDefaultIntegrationConfiguration

        Dim xSettings As New NAV.clsDefaultIntegrationConfiguration
        Dim sFile As String = My.Settings.ERPSettingsFile
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


    ''' <summary>
    ''' returns the actual boolean provided using Boolean.TryParse or false
    ''' </summary>
    ''' <param name="s"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function getXMLBool(ByVal s As String) As Boolean
        Dim bVal As Boolean = False
        If Boolean.TryParse(s, bVal) Then
            Return bVal
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' returns the actual double provided using double.tryparse or 0
    ''' </summary>
    ''' <param name="s"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function getXMLDouble(ByVal s As String) As Double
        Dim dVal As Double = 0
        If Double.TryParse(s, dVal) Then
            Return dVal
        Else
            Return 0
        End If
    End Function

    ''' <summary>
    ''' returns the actual Integer provided using Integer.tryparse or 0
    ''' </summary>
    ''' <param name="s"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function getXMLInteger(ByVal s As String) As Double
        Dim iVal As Integer = 0
        If Integer.TryParse(s, iVal) Then
            Return iVal
        Else
            Return 0
        End If
    End Function



End Module
