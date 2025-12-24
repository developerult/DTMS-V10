
Imports TMS = Ngl.FreightMaster.Integration

Module Module1

    Private _AdminEmail As String = "rramsey@nextgeneration.com"
    Public Property AdminEmail() As String
        Get
            Return _AdminEmail
        End Get
        Set(ByVal value As String)
            _AdminEmail = value
        End Set
    End Property

    Private _Source As String = "TestNAVIntegration"
    Public Property Source() As String
        Get
            Return _Source
        End Get
        Set(ByVal value As String)
            _Source = value
        End Set
    End Property

    Private _LegalEntity As String
    Public Property LegalEntity() As String
        Get
            Return _LegalEntity
        End Get
        Set(ByVal value As String)
            _LegalEntity = value
        End Set
    End Property



    Sub Main()

        Try
            Dim oRes As TMS.clsIntegrationUpdateResults = processCarrierDataDLLDirect()
            Return
            'If Not ConfigureInstance(sSource, c) Then Return
            Dim oSettings As TMSIntegrationSettings.vERPIntegrationSetting()
            If Not getTMSIntegrationSettings(oSettings) Then Return
            If Not oSettings Is Nothing AndAlso oSettings.Count > 0 Then
                Dim sLegals As List(Of String) = oSettings.Select(Function(x) x.LegalEntity).Distinct().ToList()
                If Not sLegals Is Nothing AndAlso sLegals.Count() > 0 Then
                    For Each legal In sLegals
                        Dim lLegalSettings As TMSIntegrationSettings.vERPIntegrationSetting() = oSettings.Where(Function(x) x.LegalEntity = legal).ToArray()
                        If Not lLegalSettings Is Nothing AndAlso lLegalSettings.Count() > 0 Then
                            LegalEntity = legal
                            Dim Processed As New List(Of Integer)
                            Dim Orders As New List(Of String)
                            Dim StandardSetting = getSpecificTMSSetting("Standard", lLegalSettings, Nothing)

                            processCarrierDataSample(getSpecificTMSSetting("Carrier", lLegalSettings, StandardSetting), Processed)
                            Return
                            'update the ERP Testing Status Flag using web services if available
                            Dim sKey As String = "N"
                            Console.WriteLine("Test Company: press Y or N")
                            Dim ctKey = Console.ReadKey()
                            If ctKey.KeyChar.ToString.ToUpper = "Y" Then

                                processCompanyData(getSpecificTMSSetting("Company", lLegalSettings, StandardSetting), Processed)
                            End If
                            Processed = New List(Of Integer)
                            Console.WriteLine("Test Carrier: press Y or N")
                            ctKey = Console.ReadKey()
                            If ctKey.KeyChar.ToString.ToUpper = "Y" Then

                                processCarrierData(getSpecificTMSSetting("Carrier", lLegalSettings, StandardSetting), Processed)
                            End If
                        End If
                    Next
                End If

            End If

        Catch ex As Exception

        Finally
            Console.WriteLine("Press Enter To Continue")
            Console.ReadLine()

        End Try

    End Sub

    Private Function getSpecificTMSSetting(ByVal n As String, ByRef a As TMSIntegrationSettings.vERPIntegrationSetting(), ByRef s As TMSIntegrationSettings.vERPIntegrationSetting) As TMSIntegrationSettings.vERPIntegrationSetting
        If a Is Nothing OrElse a.Count() < 1 Then Return s
        If a.Any(Function(x) x.IntegrationTypeName = n) Then
            Return a.Where(Function(x) x.IntegrationTypeName = n).FirstOrDefault()
        Else
            Return s
        End If
    End Function

    Private Sub LogError(ByVal msg As String, Optional ByVal email As String = "")
        Console.WriteLine("-----------------------------")
        Console.WriteLine(msg)
        Console.WriteLine("-----------------------------")
    End Sub

    Private Sub Log(ByVal msg As String)
        LogError(msg)
    End Sub

    Private Function getTMSIntegrationSettings(ByRef oSettings As TMSIntegrationSettings.vERPIntegrationSetting()) As Boolean
        Dim blnRet As Boolean = False
        Try
            Dim oSettingObject As New TMSIntegrationSettings.DTMSIntegration()
            oSettingObject.Url = My.Settings.TestNAVIntegrationCMDLine_TMSIntegrationSettings_DTMSIntegration

            Dim ReturnMessage As String
            Dim ERPTypeName As String = "NAV"
            Dim RetVal As Integer
#Disable Warning BC42030 ' Variable 'ReturnMessage' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            oSettings = oSettingObject.getvERPIntegrationSettingsByName(My.Settings.TMSSettingsAuthCode, My.Settings.TMSRunLegalEntity, ERPTypeName, RetVal, ReturnMessage)
#Enable Warning BC42030 ' Variable 'ReturnMessage' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            If RetVal <> TMS.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete Then
                Select Case RetVal
                    Case TMS.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        LogError("Error Data Connection Failure! could not read Integration Settings information:  " & ReturnMessage)
                        Return False
                    Case TMS.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                        LogError("Error Data Validation Failure! could notread Integration Settings information:  " & ReturnMessage, AdminEmail)
                        Return False
                    Case Else
                        LogError("Error Integration Failure! could not read Integration Settings information:  " & ReturnMessage, AdminEmail)
                        Return False
                End Select
            Else
                blnRet = True
            End If

        Catch ex As Exception
            LogError(Source & " Unexpected Read Integration Settings Error! Could not process any integration requests; the actual error is:  " & ex.Message, AdminEmail)
            Throw
        End Try
        Return blnRet

    End Function



    Private Function processCompanyData(ByVal TMSSetting As TMSIntegrationSettings.vERPIntegrationSetting, Optional ByRef Processed As List(Of Integer) = Nothing) As Boolean
        Dim blnRet As Boolean = False
        If Processed Is Nothing Then Processed = New List(Of Integer)
        If TMSSetting Is Nothing OrElse String.IsNullOrWhiteSpace(TMSSetting.TMSURI) OrElse String.IsNullOrWhiteSpace(TMSSetting.ERPURI) Then
            LogError("Missing TMS Integration settings for Company; nothing to do returning false")
            Return False
        End If
        Try
            Log("Begin Process Company Data ")
            Dim strMsg As String = ""
            Dim ReturnMessage As String = ""
            Dim RetVal As Integer = 0
            Dim oCompIntegration As New TMSIntegrationServices.DTMSERPIntegration()
            oCompIntegration.Url = TMSSetting.TMSURI
            If (String.IsNullOrWhiteSpace(TMSSetting.TMSAuthUser)) Then
                oCompIntegration.UseDefaultCredentials = True
            Else
                oCompIntegration.Credentials() = New System.Net.NetworkCredential(TMSSetting.TMSAuthUser, TMSSetting.TMSAuthPassword)
            End If
            Dim oCompHeaders As New List(Of TMSIntegrationServices.clsCompanyHeaderObject70)
            Dim oCompConts As New List(Of TMSIntegrationServices.clsCompanyContactObject70)
            Dim oCompCalendars As New List(Of TMSIntegrationServices.clsCompanyCalendarObject70)
            Dim oNavCompany = New NAVService.Company
            Dim oNavCompanies As New NAVService.DynamicsTMSCompanies
            Dim oNAVWebService = New NAVService.DynamicsTMSWebServices()
            oNAVWebService.Url = TMSSetting.ERPURI
            If (String.IsNullOrWhiteSpace(TMSSetting.ERPAuthUser)) Then
                oNAVWebService.UseDefaultCredentials = True
            Else
                oNAVWebService.Credentials() = New System.Net.NetworkCredential(TMSSetting.ERPAuthUser, TMSSetting.ERPAuthPassword)
            End If
            oNAVWebService.GetCompanies(oNavCompanies, True, False)
            If oNavCompanies Is Nothing OrElse oNavCompanies.Company Is Nothing OrElse oNavCompanies.Company.Count() < 1 Then
                Log("No company Data")
                Return True 'not ready yet so just return true
            End If
            For Each c In oNavCompanies.Company
                If Not c Is Nothing Then
                    Log(String.Concat("Name: ", c.CompName, " LE: ", c.CompLegalEntity, " Alpha: ", c.CompAlphaCode))
                End If
            Next
            Log("Process Company Data Complete")
        Catch ex As Exception
            LogError(Source & " Unexpected Integration Error! Could not import Company information:  " & ex.Message, AdminEmail)
        End Try

        Return blnRet
    End Function

    Private Function processCarrierDataDLLDirect() As TMS.clsIntegrationUpdateResults
        Dim strMsg As String = ""
        Dim ReturnMessage As String = ""
        Dim RetVal As Integer = 0


        Dim oCarrierHeaders As New List(Of TMS.clsCarrierHeaderObject70)
        Dim oCarrierConts As New List(Of TMS.clsCarrierContactObject70)

        Dim oHeader = New TMS.clsCarrierHeaderObject70
        With oHeader
            .CarrierAlphaCode = "E205"
            .CarrierCurrencyType = ""
            .CarrierEmail = ""
            .CarrierLegalEntity = "TRACHTE-LIVE"
            .CarrierMailAddress1 = "PO BOX 25612"
            .CarrierMailAddress2 = ""
            .CarrierMailAddress3 = ""
            .CarrierMailCity = "RICHMOND"
            .CarrierMailCountry = "USA"
            .CarrierMailState = "VA"
            .CarrierMailZip = "23260-5612"
            .CarrierName = "ESTES EXPRESS LINES"
            .CarrierNumber = "0"
            .CarrierQualAuthority = ""
            .CarrierQualContract = False
            .CarrierQualContractExpiresDate = ""
            .CarrierQualInsuranceDate = ""
            .CarrierQualQualified = True
            .CarrierQualSignedDate = ""
            .CarrierSCAC = ""
            .CarrierStreetAddress1 = "PO BOX 25612"
            .CarrierStreetAddress2 = ""
            .CarrierStreetAddress3 = ""
            .CarrierStreetCity = "RICHMOND"
            .CarrierStreetCountry = "USA"
            .CarrierStreetState = "VA"
            .CarrierStreetZip = "23260-5612"
            .CarrierTypeCode = ""
            .CarrierWebSite = ""
        End With

        oCarrierHeaders.Add(oHeader)
        Dim CarrierHeaders As TMS.clsCarrierHeaderObject70() = oCarrierHeaders.ToArray()
        Dim CarrierContacts As TMS.clsCarrierContactObject70()
        If Not oCarrierConts Is Nothing AndAlso oCarrierConts.Count() > 0 Then CarrierContacts = oCarrierConts.ToArray()

        Dim oRes As New TMS.clsIntegrationUpdateResults
        oRes.ReturnValue = TMS.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        ReturnMessage = ""
        Dim sSource As String = "DTMSERPIntegration.ProcessCarrierData70"
        Dim sDataType As String = "Company"
        Try
            If CarrierHeaders Is Nothing OrElse CarrierHeaders.Length = 0 Then
                ReturnMessage = "Empty Header"
                oRes.ReturnValue = TMS.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete
                Return oRes
            End If

            Dim lCarrierHeaders As List(Of Ngl.FreightMaster.Integration.clsCarrierHeaderObject70) = CarrierHeaders.ToList()
            Dim lCarrierContacts As New List(Of Ngl.FreightMaster.Integration.clsCarrierContactObject70)
#Disable Warning BC42104 ' Variable 'CarrierContacts' is used before it has been assigned a value. A null reference exception could result at runtime.
            If Not CarrierContacts Is Nothing AndAlso CarrierContacts.Count() > 0 Then
#Enable Warning BC42104 ' Variable 'CarrierContacts' is used before it has been assigned a value. A null reference exception could result at runtime.
                lCarrierContacts = CarrierContacts.ToList()
            End If
            Dim carrier As New Ngl.FreightMaster.Integration.clsCarrier

            populateIntegrationObjectParameters(carrier)
            oRes = carrier.ProcessObjectData70(lCarrierHeaders, lCarrierContacts, carrier.ConnectionString)
            ReturnMessage = carrier.LastError
        Catch ex As Exception
            ReturnMessage = ex.Message
            oRes.ReturnValue = TMS.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Finally

        End Try
        Return oRes
    End Function

    Private Function processCarrierDataSample(ByVal TMSSetting As TMSIntegrationSettings.vERPIntegrationSetting, Optional ByRef Processed As List(Of Integer) = Nothing) As Boolean

        Dim blnRet As Boolean = False
        If Processed Is Nothing Then Processed = New List(Of Integer)
        If TMSSetting Is Nothing OrElse String.IsNullOrWhiteSpace(TMSSetting.TMSURI) OrElse String.IsNullOrWhiteSpace(TMSSetting.ERPURI) Then
            LogError("Missing TMS Integration settings for Carrier; nothing to do returning false")
            Return False
        End If
        Try
            Log("Begin Process Carrier Data ")
            Dim strMsg As String = ""
            Dim ReturnMessage As String = ""
            Dim RetVal As Integer = 0
            Dim oCarrierIntegration As New TMSIntegrationServices.DTMSERPIntegration()
            oCarrierIntegration.Url = TMSSetting.TMSURI
            If (String.IsNullOrWhiteSpace(TMSSetting.TMSAuthUser)) Then
                oCarrierIntegration.UseDefaultCredentials = True
            Else
                oCarrierIntegration.Credentials() = New System.Net.NetworkCredential(TMSSetting.TMSAuthUser, TMSSetting.TMSAuthPassword)
            End If
            Dim oCarrierHeaders As New List(Of TMSIntegrationServices.clsCarrierHeaderObject70)
            Dim oCarrierConts As New List(Of TMSIntegrationServices.clsCarrierContactObject70)
            Dim oNAVWebService = New NAVService.DynamicsTMSWebServices()
            oNAVWebService.Url = TMSSetting.ERPURI

            Dim oHeader = New TMSIntegrationServices.clsCarrierHeaderObject70
            With oHeader
                .CarrierAlphaCode = "E205"
                .CarrierCurrencyType = ""
                .CarrierEmail = ""
                .CarrierLegalEntity = "TRACHTE-LIVE"
                .CarrierMailAddress1 = "PO BOX 25612"
                .CarrierMailAddress2 = ""
                .CarrierMailAddress3 = ""
                .CarrierMailCity = "RICHMOND"
                .CarrierMailCountry = "USA"
                .CarrierMailState = "VA"
                .CarrierMailZip = "23260-5612"
                .CarrierName = "ESTES EXPRESS LINES"
                .CarrierNumber = "0"
                .CarrierQualAuthority = ""
                .CarrierQualContract = False
                .CarrierQualContractExpiresDate = ""
                .CarrierQualInsuranceDate = ""
                .CarrierQualQualified = True
                .CarrierQualSignedDate = ""
                .CarrierSCAC = ""
                .CarrierStreetAddress1 = "PO BOX 25612"
                .CarrierStreetAddress2 = ""
                .CarrierStreetAddress3 = ""
                .CarrierStreetCity = "RICHMOND"
                .CarrierStreetCountry = "USA"
                .CarrierStreetState = "VA"
                .CarrierStreetZip = "23260-5612"
                .CarrierTypeCode = ""
                .CarrierWebSite = ""
            End With

            oCarrierHeaders.Add(oHeader)
            Dim aCarrierHeaders As TMSIntegrationServices.clsCarrierHeaderObject70() = oCarrierHeaders.ToArray()
            Dim aCarrierConts As TMSIntegrationServices.clsCarrierContactObject70()
            If Not oCarrierConts Is Nothing AndAlso oCarrierConts.Count() > 0 Then aCarrierConts = oCarrierConts.ToArray()
#Disable Warning BC42104 ' Variable 'aCarrierConts' is used before it has been assigned a value. A null reference exception could result at runtime.
            Dim oResults As TMSIntegrationServices.clsIntegrationUpdateResults = oCarrierIntegration.ProcessCarrierData70(TMSSetting.TMSAuthCode, aCarrierHeaders, aCarrierConts, ReturnMessage)
#Enable Warning BC42104 ' Variable 'aCarrierConts' is used before it has been assigned a value. A null reference exception could result at runtime.

            Select Case oResults.ReturnValue
                Case TMS.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                    Log("Error Data Connection Failure! could not import Carrier information:  " & ReturnMessage)
                Case TMS.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                    Log("DataIntegrationFailure Failure! could not import Carrier information:  " & ReturnMessage)
                Case TMS.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                    Log("DataValidationFailure Failure! could not import Carrier information:  " & ReturnMessage)

                Case Else
                    Log("success")
                    blnRet = True
            End Select

            Log("Process Carrier Data Complete")
        Catch ex As Exception
            LogError(Source & " Unexpected Integration Error! Could not import Carrier information:  " & ex.Message, AdminEmail)
        End Try

        Return blnRet
    End Function

    Private Function processCarrierData(ByVal TMSSetting As TMSIntegrationSettings.vERPIntegrationSetting, Optional ByRef Processed As List(Of Integer) = Nothing) As Boolean
        Dim blnRet As Boolean = False
        If Processed Is Nothing Then Processed = New List(Of Integer)
        If TMSSetting Is Nothing OrElse String.IsNullOrWhiteSpace(TMSSetting.TMSURI) OrElse String.IsNullOrWhiteSpace(TMSSetting.ERPURI) Then
            LogError("Missing TMS Integration settings for Carrier; nothing to do returning false")
            Return False
        End If
        Try
            Log("Begin Process Carrier Data ")
            Dim strMsg As String = ""
            Dim ReturnMessage As String = ""
            Dim RetVal As Integer = 0
            Dim oCarrierIntegration As New TMSIntegrationServices.DTMSERPIntegration()
            oCarrierIntegration.Url = TMSSetting.TMSURI
            If (String.IsNullOrWhiteSpace(TMSSetting.TMSAuthUser)) Then
                oCarrierIntegration.UseDefaultCredentials = True
            Else
                oCarrierIntegration.Credentials() = New System.Net.NetworkCredential(TMSSetting.TMSAuthUser, TMSSetting.TMSAuthPassword)
            End If
            Dim oCarrierHeaders As New List(Of TMSIntegrationServices.clsCarrierHeaderObject70)
            Dim oCarrierConts As New List(Of TMSIntegrationServices.clsCarrierContactObject70)
            Dim oNAVWebService = New NAVService.DynamicsTMSWebServices()
            oNAVWebService.Url = TMSSetting.ERPURI
            If (String.IsNullOrWhiteSpace(TMSSetting.ERPAuthUser)) Then
                oNAVWebService.UseDefaultCredentials = True
            Else
                oNAVWebService.Credentials() = New System.Net.NetworkCredential(TMSSetting.ERPAuthUser, TMSSetting.ERPAuthPassword)
            End If
            Dim oNavCarriers = New NAVService.DynamicsTMSCarriers()
            oNAVWebService.GetCarriers(oNavCarriers, True, False)
            Dim strSkip As New List(Of String)
            If oNavCarriers Is Nothing OrElse oNavCarriers.Carrier Is Nothing OrElse oNavCarriers.Carrier.Count() < 1 Then
                Log("NoCarrier Data")
                Return True 'not ready yet so just return true
            End If
            For Each c In oNavCarriers.Carrier
                If Not c Is Nothing Then
                    Log(String.Concat("Name: ", c.CarrierName, " Nbr: ", c.CarrierNumber, " Alpha: ", c.CarrierAlphaCode))
                End If
            Next
            Log("Process Carrier Data Complete")
        Catch ex As Exception
            LogError(Source & " Unexpected Integration Error! Could not import Carrier information:  " & ex.Message, AdminEmail)
        End Try

        Return blnRet
    End Function

    Private Sub populateIntegrationObjectParameters(ByRef oImportExport As TMS.clsImportExport)

        Dim connectionString As String = "Data Source=TBS-TMSdB;Initial Catalog=NGLMASPROD;User ID=nglweb;Password=nglweb"
        With oImportExport
            .AdminEmail = AdminEmail
            .FromEmail = AdminEmail
            .GroupEmail = AdminEmail
            .Retry = 0
            .SMTPServer = "nglmail.nextgeneration.com"
            .DBServer = My.Settings.DBServer
            .Database = My.Settings.Database
            .ConnectionString = connectionString
            .AuthorizationCode = "WCFProd"
            .Debug = True
            .WCFAuthCode = "WCFProd"
            .WCFURL = "http://nglwcfprod.trachte.com/"
            .WCFTCPURL = "net.tcp://NGLWCFPROD.trachte.com:808"
        End With

    End Sub





End Module
