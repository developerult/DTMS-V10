Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Ngl.FM
Imports System.ServiceProcess
Imports System.Data.SqlClient
Imports System.Data
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports GP = Ngl.FM.GPIntegration
Imports System.Xml.Linq


<TestClass()> Public Class GPIntegrationTests

    '<TestMethod()> Public Sub TestGPCompany()
    '    'Dim oGP As New NGL.FM.GPIntegration.clsApplication()
    '    'Dim ocomp = oGP.GetCompanyList()
    '    'Dim GPApp As New GPIntegration.clsApplication
    '    'Dim GPCompany As New Ngl.FM.GPIntegration.GPDataIntegrationSTructure.GPCompanies

    '    ' Need to work with Rob to set ERP server
    '    'GPCompany.GPDatabase = "TWO"

    '    'GPDynamics(c.TMSDBServer.ToString, Me.LegalEntity.ToString
    '    'Dim SuccessFlag As Boolean

    '    'SuccessFlag = GPApp.GPDynamics("NGLGP2013R2", GPCompany)

    '    'MsgBox(SuccessFlag)

    '    'If Not ocomp Is Nothing AndAlso ocomp.Count() > 0 Then
    '    'For Each c In ocomp
    '    'System.Diagnostics.Debug.WriteLine(c.Name)
    '    'Next

    '    'End If


    '    System.Diagnostics.Debug.WriteLine("Success!")

    'End Sub

    '<TestMethod()> Public Sub TestGPWarehouses()
    '    Dim oGP As New Ngl.FM.GPIntegration.clsApplication()
    '    'Dim ocomp = oGP.GetCompanyList()
    '    'If Not ocomp Is Nothing AndAlso ocomp.Count() > 0 Then
    '    '        For Each c In ocomp
    '    'Dim compkey = c.Key
    '    'Dim compID = c.Key.Id
    '    'Dim LegalEntity = Left(c.Name, 3)
    '    'System.Diagnostics.Debug.WriteLine(c.Name)
    '    'Dim oWarehouses = oGP.getWarehouses("", compID)
    '    'If Not oWarehouses Is Nothing AndAlso oWarehouses.Count() > 0 Then
    '    '    For Each w In oWarehouses
    '    '        System.Diagnostics.Debug.WriteLine("State: {0}  Description: {1}", w.State, w.Description)
    '    '    Next
    '    'End If
    '    'Next
    '    'End If


    'End Sub


    '<TestMethod()>
    'Public Sub GPIntegrationXMLConfigurationTest()

    '    Dim sSource As String = "GPIntegrationTests.GPIntegrationXMLConfigurationTest"
    '    Dim strFile As String = "C:\Data\IntegrationServicesData\GPServiceSettings.xml"
    '    Dim xSettings As New GP.clsDefaultIntegrationConfiguration


    '    'check if the file exists:
    '    If System.IO.File.Exists(strFile) Then
    '        Dim doc As XDocument = XDocument.Load(strFile)
    '        'For testing we read one setting at a time
    '        xSettings.ERPTestURI = (From s In doc.Root.Elements("Setting") Select s.Element("ERPTestURI")).FirstOrDefault()
    '        xSettings.ERPTestAuthUser = (From s In doc.Root.Elements("Setting") Select s.Element("ERPTestAuthUser")).FirstOrDefault()
    '        xSettings.ERPTestAuthPassword = (From s In doc.Root.Elements("Setting") Select s.Element("ERPTestAuthPassword")).FirstOrDefault()
    '        xSettings.ERPTestAuthCode = (From s In doc.Root.Elements("Setting") Select s.Element("ERPTestAuthCode")).FirstOrDefault()
    '        xSettings.ERPTestLegalEntity = (From s In doc.Root.Elements("Setting") Select s.Element("ERPTestLegalEntity")).FirstOrDefault()
    '        xSettings.ERPTestExportMaxRetry = getXMLInteger((From s In doc.Root.Elements("Setting") Select s.Element("ERPTestExportMaxRetry")).FirstOrDefault())
    '        xSettings.ERPTestExportRetryMinutes = getXMLInteger((From s In doc.Root.Elements("Setting") Select s.Element("ERPTestExportRetryMinutes")).FirstOrDefault())
    '        xSettings.ERPTestExportMaxRowsReturned = getXMLInteger((From s In doc.Root.Elements("Setting") Select s.Element("ERPTestExportMaxRowsReturned")).FirstOrDefault())
    '        xSettings.ERPTestExportAutoConfirmation = getXMLBool((From s In doc.Root.Elements("Setting") Select s.Element("ERPTestExportAutoConfirmation")).FirstOrDefault())
    '        xSettings.ERPTestFreightCost = getXMLDouble((From s In doc.Root.Elements("Setting") Select s.Element("ERPTestFreightCost")).FirstOrDefault())
    '        xSettings.ERPTestFreightBillNumber = (From s In doc.Root.Elements("Setting") Select s.Element("ERPTestFreightBillNumber")).FirstOrDefault()
    '        xSettings.ERPFrieghtAccountIndex = (From s In doc.Root.Elements("Setting") Select s.Element("ERPFrieghtAccountIndex")).FirstOrDefault()
    '        xSettings.TMSSettingsURI = (From s In doc.Root.Elements("Setting") Select s.Element("TMSSettingsURI")).FirstOrDefault()
    '        xSettings.TMSSettingsAuthCode = (From s In doc.Root.Elements("Setting") Select s.Element("TMSSettingsAuthCode")).FirstOrDefault()
    '        xSettings.TMSSettingsAuthUser = (From s In doc.Root.Elements("Setting") Select s.Element("TMSSettingsAuthUser")).FirstOrDefault()
    '        xSettings.TMSSettingsAuthPassword = (From s In doc.Root.Elements("Setting") Select s.Element("TMSSettingsAuthPassword")).FirstOrDefault()
    '        xSettings.TMSTestServiceURI = (From s In doc.Root.Elements("Setting") Select s.Element("TMSTestServiceURI")).FirstOrDefault()
    '        xSettings.TMSTestServiceAuthCode = (From s In doc.Root.Elements("Setting") Select s.Element("TMSTestServiceAuthCode")).FirstOrDefault()
    '        xSettings.TMSTestServiceAuthPassword = (From s In doc.Root.Elements("Setting") Select s.Element("TMSTestServiceAuthPassword")).FirstOrDefault()
    '        xSettings.TMSTestServiceAuthUser = (From s In doc.Root.Elements("Setting") Select s.Element("TMSTestServiceAuthUser")).FirstOrDefault()
    '        xSettings.RunERPTest = getXMLBool((From s In doc.Root.Elements("Setting") Select s.Element("RunERPTest")).FirstOrDefault())
    '        xSettings.Debug = getXMLBool((From s In doc.Root.Elements("Setting") Select s.Element("Debug")).FirstOrDefault())
    '        xSettings.Verbos = getXMLBool((From s In doc.Root.Elements("Setting") Select s.Element("Verbos")).FirstOrDefault())
    '        xSettings.TMSDBName = (From s In doc.Root.Elements("Setting") Select s.Element("TMSDBName")).FirstOrDefault()
    '        xSettings.TMSDBServer = (From s In doc.Root.Elements("Setting") Select s.Element("TMSDBServer")).FirstOrDefault()
    '        xSettings.TMSDBUser = (From s In doc.Root.Elements("Setting") Select s.Element("TMSDBUser")).FirstOrDefault()
    '        xSettings.TMSDBPass = (From s In doc.Root.Elements("Setting") Select s.Element("TMSDBPass")).FirstOrDefault()
    '        xSettings.TMSRunLegalEntity = (From s In doc.Root.Elements("Setting") Select s.Element("TMSRunLegalEntity")).FirstOrDefault()
    '        xSettings.GPFunctionsTotalSOWeight = (From s In doc.Root.Elements("Setting") Select s.Element("GPFunctionsTotalSOWeight")).FirstOrDefault()
    '        xSettings.GPFunctionsTotalSOConWeight = (From s In doc.Root.Elements("Setting") Select s.Element("GPFunctionsTotalSOConWeight")).FirstOrDefault()
    '        xSettings.GPFunctionsTotalPOWeight = (From s In doc.Root.Elements("Setting") Select s.Element("GPFunctionsTotalPOWeight")).FirstOrDefault()
    '        xSettings.GPFunctionsTotalSOQuantity = (From s In doc.Root.Elements("Setting") Select s.Element("GPFunctionsTotalSOQuantity")).FirstOrDefault()
    '        xSettings.GPFunctionsTotalSOConQuantity = (From s In doc.Root.Elements("Setting") Select s.Element("GPFunctionsTotalSOConQuantity")).FirstOrDefault()
    '        xSettings.GPFunctionsTotalPOQuantity = (From s In doc.Root.Elements("Setting") Select s.Element("GPFunctionsTotalPOQuantity")).FirstOrDefault()
    '        xSettings.GPFunctionsTotalSOPallets = (From s In doc.Root.Elements("Setting") Select s.Element("GPFunctionsTotalSOPallets")).FirstOrDefault()
    '        xSettings.GPFunctionsTotalPOPallets = (From s In doc.Root.Elements("Setting") Select s.Element("GPFunctionsTotalPOPallets")).FirstOrDefault()
    '        xSettings.GPFunctionsSOItemDetails = (From s In doc.Root.Elements("Setting") Select s.Element("GPFunctionsSOItemDetails")).FirstOrDefault()
    '        xSettings.GPFunctionsSOConItemDetails = (From s In doc.Root.Elements("Setting") Select s.Element("GPFunctionsSOConItemDetails")).FirstOrDefault()
    '        xSettings.GPFunctionsPOItemDetails = (From s In doc.Root.Elements("Setting") Select s.Element("GPFunctionsPOItemDetails")).FirstOrDefault()
    '        xSettings.GPFunctionsForceDefaultCountry = (From s In doc.Root.Elements("Setting") Select s.Element("GPFunctionsForceDefaultCountry")).FirstOrDefault()
    '        xSettings.APExportOn = (From s In doc.Root.Elements("Setting") Select s.Element("APExportOn")).FirstOrDefault()
    '        xSettings.PayablesOn = (From s In doc.Root.Elements("Setting") Select s.Element("PayablesOn")).FirstOrDefault()
    '        xSettings.GPFunctionsGetSOShippingMethod = (From s In doc.Root.Elements("Setting") Select s.Element("GPFunctionsGetSOShippingMethod")).FirstOrDefault()
    '        xSettings.GPFunctionsGetSOConShippingMethod = (From s In doc.Root.Elements("Setting") Select s.Element("GPFunctionsGetSOConShippingMethod")).FirstOrDefault()
    '        xSettings.GPFunctionsGetPOShippingMethod = (From s In doc.Root.Elements("Setting") Select s.Element("GPFunctionsGetPOShippingMethod")).FirstOrDefault()
    '        xSettings.GPFunctionsGetSOTemp = (From s In doc.Root.Elements("Setting") Select s.Element("GPFunctionsGetSOTemp")).FirstOrDefault()
    '        xSettings.GPFunctionsGetPOTemp = (From s In doc.Root.Elements("Setting") Select s.Element("GPFunctionsGetPOTemp")).FirstOrDefault()
    '        xSettings.GPFunctionsGetPayableCreditGL = (From s In doc.Root.Elements("Setting") Select s.Element("GPFunctionsGetPayableCreditGL")).FirstOrDefault()
    '        xSettings.GPFunctionsGetCurrencyID = (From s In doc.Root.Elements("Setting") Select s.Element("GPFunctionsGetCurrencyID")).FirstOrDefault()
    '        xSettings.GPFunctionsDefaultFreightGLAccount = (From s In doc.Root.Elements("Setting") Select s.Element("GPFunctionsDefaultFreightGLAccount")).FirstOrDefault()
    '        'xSettings.GPFunctionGetNextSOPInvDexID = (From s In doc.Root.Elements("Setting") Select s.Element("GPFunctionGetNextSOPInvDexID")).FirstOrDefault()
    '        'xSettings.GPFunctionGetShipConfirmedInv = (From s In doc.Root.Elements("Setting") Select s.Element("GPFunctionGetShipConfirmedInv")).FirstOrDefault()
    '        xSettings.GPPMBatchPrefix = (From s In doc.Root.Elements("Setting") Select s.Element("GPPMBatchPrefix")).FirstOrDefault()
    '        xSettings.GPBatchDateToAdd = getXMLInteger((From s In doc.Root.Elements("Setting") Select s.Element("GPBatchDateToAdd")).FirstOrDefault())
    '        xSettings.GPFunctionGetItemTemp = (From s In doc.Root.Elements("Setting") Select s.Element("GPFunctionGetItemTemp")).FirstOrDefault()
    '        xSettings.GPFunctionSOPCostUpdateOn = (From s In doc.Root.Elements("Setting") Select s.Element("GPFunctionSOPCostUpdateOn")).FirstOrDefault()
    '        xSettings.FrightBillCostsOn = (From s In doc.Root.Elements("Setting") Select s.Element("FrightBillCostsOn")).FirstOrDefault()
    '        xSettings.GPFunctionCheckForSOPUserDefinedRecord = (From s In doc.Root.Elements("Setting") Select s.Element("GPFunctionCheckForSOPUserDefinedRecord")).FirstOrDefault()
    '        xSettings.GPFunctionUpdateUserDefinedRecord = (From s In doc.Root.Elements("Setting") Select s.Element("GPFunctionUpdateUserDefinedRecord")).FirstOrDefault()
    '        xSettings.GPFunctionShipConfirmationOn = getXMLBool((From s In doc.Root.Elements("Setting") Select s.Element("GPFunctionShipConfirmationOn")).FirstOrDefault())
    '        xSettings.GPPMPOFieldValue = (From s In doc.Root.Elements("Setting") Select s.Element("GPPMPOFieldValue")).FirstOrDefault()
    '        xSettings.GPPMDescriptionFieldValue = (From s In doc.Root.Elements("Setting") Select s.Element("GPPMDescriptionFieldValue")).FirstOrDefault()
    '        xSettings.GPFunctionGetSOPOrdersOnFreightBill = (From s In doc.Root.Elements("Setting") Select s.Element("GPFunctionGetSOPOrdersOnFreightBill")).FirstOrDefault()
    '        xSettings.GPFunctionsGetTOShippingMethod = (From s In doc.Root.Elements("Setting") Select s.Element("GPFunctionsGetTOShippingMethod")).FirstOrDefault()
    '        xSettings.GPFunctionsGetTOTemp = (From s In doc.Root.Elements("Setting") Select s.Element("GPFunctionsGetTOTemp")).FirstOrDefault()
    '        xSettings.GPFunctionsPOComment = (From s In doc.Root.Elements("Setting") Select s.Element("GPFunctionsPOComment")).FirstOrDefault()
    '        xSettings.GPFunctionsPONotes = (From s In doc.Root.Elements("Setting") Select s.Element("GPFunctionsPONotes")).FirstOrDefault()
    '        xSettings.GPFunctionsSOPComment = (From s In doc.Root.Elements("Setting") Select s.Element("GPFunctionsSOPComment")).FirstOrDefault()
    '        xSettings.GPFunctionsSOPNotes = (From s In doc.Root.Elements("Setting") Select s.Element("GPFunctionsSOPNotes")).FirstOrDefault()
    '        xSettings.GPFunctionsTOItemDetails = (From s In doc.Root.Elements("Setting") Select s.Element("GPFunctionsTOItemDetails")).FirstOrDefault()
    '        xSettings.GPFunctionsTotalTOPallets = (From s In doc.Root.Elements("Setting") Select s.Element("GPFunctionsTotalTOPallets")).FirstOrDefault()
    '        xSettings.GPFunctionsTotalTOWeight = (From s In doc.Root.Elements("Setting") Select s.Element("GPFunctionsTotalTOWeight")).FirstOrDefault()



    '    End If
    '    Dim results = xSettings

    'End Sub



    '<TestMethod()>
    'Public Sub GPIntegrationRemoteSystemTest()
    '    'Testing company for legal entity 'Two' = Control: 12693 Number: 20000  Name: Main Site   
    '    'Sample Order Numbers
    '    'ORDST2230, ORDST2229, ORDST2232, ORDST2231, ORDST2226, ORDST2234, ORDST2235
    '    'pohdr orders
    '    'ORDST2230, ORDST2231, ORDST2232, ORDST2226, ORDST2227, ORDST2228, ORDST2233, ORDST2225, ORDST2234, ORDST2235
    '    Dim sSource As String = "NGL.Integration.GPIntegrationRemoteSystemTest"
    '    Dim oApp As New GP.clsApplication
    '    Dim oDefaultSettings As New GP.clsDefaultIntegrationConfiguration
    '    With oDefaultSettings
    '        .ERPTestURI = ""
    '        .ERPTestAuthUser = ""
    '        .ERPTestAuthPassword = ""
    '        .ERPTestAuthCode = ""
    '        .ERPTestLegalEntity = "PQ|PQR"

    '        .ERPTestExportMaxRetry = 1
    '        .ERPTestExportRetryMinutes = 15
    '        .ERPTestExportMaxRowsReturned = 100
    '        .ERPTestExportAutoConfirmation = False
    '        .ERPTestFreightCost = 100
    '        .ERPTestFreightBillNumber = "FB Unit Test"
    '        .ERPFrieghtAccountIndex = "520, 521"

    '        .TMSSettingsURI = "http://WSTEST.POLYQUEST.COM/DTMSIntegration.asmx"
    '        .TMSSettingsAuthCode = "WSTEST"
    '        .TMSSettingsAuthUser = ""
    '        .TMSSettingsAuthPassword = ""
    '        .TMSTestServiceURI = "http://WSTEST.POLYQUEST.COM/DTMSERPIntegration.asmx"
    '        .TMSTestServiceAuthCode = "WSTEST"
    '        .TMSTestServiceAuthPassword = ""
    '        .TMSTestServiceAuthUser = ""

    '        .RunERPTest = False
    '        .Debug = False
    '        .Verbos = False


    '        .TMSDBName = "NGLMASPROD"
    '        .TMSDBServer = "TESTTMS"
    '        .TMSDBPass = "sq16"
    '        .TMSDBUser = "nglweb"
    '        .TMSRunLegalEntity = "PQ|PQR" 'we use all legal entityes and check the config tables in tms for NAV integration settings
    '        .PayablesOn = False
    '        .APExportOn = True
    '        .GPFunctionsTotalSOWeight = "Select sum((sopln.QUANTITY - sopln.QTYCANCE) * iv.ITEMSHWT) As soptotalweight from SOP10200 sopln inner join IV00101 iv On iv.ITEMNMBR = sopln.ITEMNMBR where sopln.SOPNUMBE = '{0}'"
    '        .GPFunctionsTotalTOWeight = "select sum((invtrn.TRNSFQTY * iv.ITEMSHWT)) As soptotalweight from SVC00701 invtrn inner join IV00101 iv On iv.ITEMNMBR = invtrn.ITEMNMBR where invtrn.ORDDOCID = '{0}'"
    '        .GPFunctionsTotalPOWeight = "Select sum((sopln.QTYORDER - sopln.QTYCANCE) * iv.ITEMSHWT) As soptotalweight from POP10110 sopln inner join IV00101 iv On iv.ITEMNMBR = sopln.ITEMNMBR where sopln.PONUMBER = '{0}'"
    '        .GPFunctionsTotalSOQuantity = "select sum(sopln.QUANTITY - sopln.QTYCANCE) As soptotalquantity from SOP10200 sopln where sopln.SOPNUMBE = '{0}'"
    '        .GPFunctionsTotalPOQuantity = "Select sum(sopln.QTYORDER - sopln.QTYCANCE) As soptotalquantity from POP10110 sopln where sopln.ponumber = '{0}'"
    '        .GPFunctionsTotalSOPallets = "select sum(sopln.QUANTITY - sopln.QTYCANCE) As soptotalquantity from SOP10200 sopln where sopln.SOPNUMBE = '{0}'"
    '        .GPFunctionsTotalTOPallets = "Select sum(invtrnln.TRNSFQTY) As invtrntotalquantity from SVC00701 invtrnln where invtrnln.ORDDOCID = '{0}'"
    '        .GPFunctionsTotalPOPallets = "select sum(QTYORDER - QTYCANCE) As total_qty from POP10110 where POLNESTA In (1, 2, 3) And PONUMBER = '{0}'"
    '        .GPFunctionsSOItemDetails = "Select Itemnmbr As ItemNumber, isnull(UnitCost, 0) As ItemCost, isnull(QUANTITY, 0) - isnull(QTYCANCE, 0) As QtyOrdered, isnull((Select top 1 iv.ITEMSHWT from IV00101 As iv where iv.ITEMNMBR = Itemnmbr), 1) As [Weight],0 As FixOffInvAllow,0 As FixFrtAllow,0 As FreightCost,isnull(EXTDCOST,0) As ItemCost,0 As [Cube],0 As [Pack],'' as [Size],ITEMDESC as [Description],'N' as Hazmat,'' as Brand,'' as CostCenter,'' as LotNumber,'' as LotExpirationDate,'' as GTIN,ITEMNMBR as CustItemNumber,'N' as PalletType,'' as POItemHazmatTypeCode,'' as POItem49CFRCode,'' as POItemIATACode,'' as POItemDOTCode,'' as POItemMarineCode,'' as POItemNMFCClass,'' as POItemFAKClass,'' as POItemLimitedQtyFlag,isnull(QUANTITY,0) - isnull(QTYCANCE,0) as POItemPallets,0 as POItemTies,0 as POItemHighs,0 as POItemQtyPalletPercentage,0 as POItemQtyLength,0 as POItemQtyWidth,0 as POItemQtyHeight,0 as POItemStackable,0 as POItemLevelOfDensity,'' as POItemNMFCSubClass,'' as POItemUser1,'' as POItemUser2,'' as POItemUser3,'' as POItemUser4,UOFM as POItemWeightUnitOfMeasure,UOFM as POItemCubeUnitOfMeasure ,UOFM as POItemDimensionUnitOfMeasure FROM sop10200 WHERE SOPTYPE = 2  and SOPNUMBE = '{0}'"
    '        .GPFunctionsTOItemDetails = "select Itemnmbr As ItemNumber, 0 As ItemCost, isnull(TRNSFQTY, 0) As QtyOrdered, isnull((select top 1 iv.ITEMSHWT from IV00101 As iv where iv.ITEMNMBR = Itemnmbr), 1) As [Weight],0 As FixOffInvAllow,0 As FixFrtAllow,0 As FreightCost,0 As ItemCost,0 As [Cube],0 As [Pack],'' as [Size],DSCRIPTN as [Description],'N' as Hazmat,'' as Brand,'' as CostCenter,'' as LotNumber,'' as LotExpirationDate,'' as GTIN,ITEMNMBR as CustItemNumber,'N' as PalletType,'' as POItemHazmatTypeCode,'' as POItem49CFRCode,'' as POItemIATACode,'' as POItemDOTCode,'' as POItemMarineCode,'' as POItemNMFCClass,'' as POItemFAKClass,'' as POItemLimitedQtyFlag,isnull(TRNSFQTY,0) as POItemPallets,0 as POItemTies,0 as POItemHighs,0 as POItemQtyPalletPercentage,0 as POItemQtyLength,0 as POItemQtyWidth,0 as POItemQtyHeight,0 as POItemStackable,0 as POItemLevelOfDensity,'' as POItemNMFCSubClass,'' as POItemUser1,'' as POItemUser2,'' as POItemUser3,'' as POItemUser4,UOFM as POItemWeightUnitOfMeasure,UOFM as POItemCubeUnitOfMeasure ,UOFM as POItemDimensionUnitOfMeasure FROM SVC00701 WHERE ORDDOCID = '{0}'"
    '        .GPFunctionsPOItemDetails = "Select Itemnmbr As ItemNumber, isnull(UnitCost, 0) As ItemCost, isnull(QTYORDER, 0) - isnull(QTYCANCE, 0) As QtyOrdered, isnull((Select top 1 iv.ITEMSHWT from IV00101 As iv where iv.ITEMNMBR = Itemnmbr), 1) As [Weight],0 As FixOffInvAllow,0 As FixFrtAllow,0 As FreightCost,isnull(EXTDCOST,0) As ItemCost,0 As [Cube],0 As [Pack],'' as [Size],ITEMDESC as [Description],'N' as Hazmat,'' as Brand,'' as CostCenter,'' as LotNumber,'' as LotExpirationDate,'' as GTIN,ITEMNMBR as CustItemNumber,'N' as PalletType,'' as POItemHazmatTypeCode,'' as POItem49CFRCode,'' as POItemIATACode,'' as POItemDOTCode,'' as POItemMarineCode,'' as POItemNMFCClass,'' as POItemFAKClass,'' as POItemLimitedQtyFlag,isnull(QTYORDER,0) - isnull(QTYCANCE,0) as POItemPallets,0 as POItemTies,0 as POItemHighs,0 as POItemQtyPalletPercentage,0 as POItemQtyLength,0 as POItemQtyWidth,0 as POItemQtyHeight,0 as POItemStackable,0 as POItemLevelOfDensity,'' as POItemNMFCSubClass,'' as POItemUser1,'' as POItemUser2,'' as POItemUser3,'' as POItemUser4,UOFM as POItemWeightUnitOfMeasure,UOFM as POItemCubeUnitOfMeasure ,UOFM as POItemDimensionUnitOfMeasure from pop10110 where POLNESTA in (1, 2, 3) and PONUMBER = '{0}'"
    '        .GPFunctionsForceDefaultCountry = "US"
    '        .GPFunctionsGetSOShippingMethod = "Select sopln.SHIPMTHD, sh.SHMTHDSC, sh.SHIPTYPE, Case sh.SHIPTYPE When 0 Then 3 Else 4 End As TransType from SOP10200 sopln inner join SY03000 sh On sh.SHIPMTHD = sopln.SHIPMTHD  where sopln.SOPTYPE = 2 And sopln.SOPNUMBE = '{0}'"
    '        .GPFunctionsGetTOShippingMethod = "select 3 As TransType from SVC00701 invtrnln where invtrnln.ORDDOCID = '{0}'"
    '        .GPFunctionsGetPOShippingMethod = "Select 'Inbound' as SHIPMTHD,'Purchase Order' as SHMTHDSC, 8 as TransType from POP10110 where POLNESTA in (1, 2, 3) and PONUMBER = '{0}'"
    '        .GPFunctionsGetSOTemp = "select 'D' as SOTemp from SOP10200 sopln inner join SY03000 sh on sh.SHIPMTHD = sopln.SHIPMTHD where sopln.SOPTYPE = 2 and sopln.SOPNUMBE = '{0}'"
    '        .GPFunctionsGetTOTemp = "Select 'D' as TOTemp from SVC00701 invtrnln where invtrnln.ORDDOCID = '{0}'"
    '        .GPFunctionsGetPOTemp = "select 'D' as POTemp from POP10110 where POLNESTA in (1, 2, 3) and PONUMBER = '{0}'"
    '        .GPFunctionsGetPayableCreditGL = "Select rtrim(gl.ACTNUMST) As gl_code from SY01100 post inner join gl00105  gl On gl.ACTINDX = post.ACTINDX where SERIES = 4 And SEQNUMBR = 200"
    '        .GPFunctionsGetCurrencyID = "US$"
    '        .GPFunctionsDefaultFreightGLAccount = "01-000-100-5050-00|21-600-700-4100-25"

    '    End With
    '    Try
    '        oApp.ProcessData(sSource, oDefaultSettings)
    '    Catch ex As ApplicationException
    '        Assert.Fail("Application Exception For " & sSource & ": {0} ", ex.Message)
    '    Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
    '        Throw
    '    Catch ex As Exception
    '        Assert.Fail("Unexpected Error For " & sSource & ": {0} ", ex.Message)
    '    Finally

    '    End Try
    'End Sub

    '<TestMethod()>
    'Public Sub GPIntegrationNormalTest()

    '    Dim sSource As String = "NGL.Integration.NAVIntegrationNormalTest"
    '    Dim oApp As New GP.clsApplication
    '    Dim sDBName As String = "NGLMAS2013DEV"
    '    Dim sDBServer As String = "NGLRDP06D"
    '    Dim sDBPass As String = "5529"
    '    Dim sDBUserName As String = "nglweb"
    '    Try
    '        Dim reg As New Ngl.Core.Security.RegistrySettings("DTMS")
    '        Try
    '            reg.checkRegistryStatus()
    '            If reg.IsDTMSRegistrysAvailable Then
    '                sDBName = reg.DBName
    '                sDBServer = reg.DBServer
    '                sDBPass = reg.DBPass
    '                sDBUserName = reg.DBUser
    '            Else
    '                Assert.Fail("Database Settings are not availalble For " & sSource & ".  Install the TMS Software. Or run as admin. " & reg.RegisterStatus.Message)
    '            End If
    '        Catch ex As Exception
    '            If Not reg Is Nothing AndAlso Not reg.RegisterStatus Is Nothing AndAlso Not String.IsNullOrWhiteSpace(reg.RegisterStatus.Message) Then
    '                Assert.Fail("Unexpected Database Settings ExceptionFor " & sSource & ":" & reg.RegisterStatus.Message & " " & ex.Message)
    '            Else
    '                Assert.Fail("Unexpected Database Settings Exception For " & sSource & ": " & ex.Message)
    '            End If
    '        End Try
    '        Dim oDefaultSettings As New GP.clsDefaultIntegrationConfiguration
    '        With oDefaultSettings
    '            .TMSDBName = sDBName
    '            .TMSDBServer = sDBServer
    '            .TMSDBPass = sDBPass
    '            .TMSDBUser = sDBUserName
    '            .TMSRunLegalEntity = "" 'we use all legal entityes and check the config tables in tms for NAV integration settings
    '            .TMSSettingsURI = "http://NGLWSDEV704.NEXTGENERATION.COM/DTMSIntegration.asmx"
    '            .TMSSettingsAuthCode = "WSDEV"
    '            .TMSSettingsAuthPassword = ""
    '            .TMSSettingsAuthUser = ""
    '            .Verbos = True
    '            .Debug = True
    '            .RunERPTest = False
    '        End With
    '        oApp.ProcessData(sSource, oDefaultSettings)
    '    Catch ex As ApplicationException
    '        Assert.Fail("Application Exception For " & sSource & ": {0} ", ex.Message)
    '    Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
    '        Throw
    '    Catch ex As Exception
    '        Assert.Fail("Unexpected Error For " & sSource & ": {0} ", ex.Message)
    '    Finally

    '    End Try
    'End Sub


    '<TestMethod()> Public Sub TestGPCustomers()
    '    Dim oGP As New Ngl.FM.GPIntegration.clsApplication()
    '    'Dim ocomp = oGP.GetCompanyList()
    '    'If Not ocomp Is Nothing AndAlso ocomp.Count() > 0 Then
    '    'For Each c In ocomp
    '    'Dim compkey = c.Key
    '    'Dim compID = c.Key.Id
    '    'Dim LegalEntity = Left(c.Name, 3)
    '    'System.Diagnostics.Debug.WriteLine(c.Name)
    '    'Dim ocustomers = oGP.GetCustomerList(companyKeyId:=compID)
    '    'If Not ocustomers Is Nothing AndAlso ocustomers.Count() > 0 Then
    '    '    For Each cust In ocustomers
    '    '        Dim laneNumber As String = String.Concat(LegalEntity, "-", cust.CorporateAccountKey, "-", cust.Name)
    '    '        System.Diagnostics.Debug.WriteLine("Lane Number {0}  ", laneNumber, "")
    '    '        'System.Diagnostics.Debug.WriteLine("State: {0} Name {1} ", cust.State, cust.Name)
    '    '    Next

    '    'End If
    '    'Next

    '    'End If

    '    System.Diagnostics.Debug.WriteLine("Success!")
    'End Sub

    '<TestMethod()> Public Sub TestGPSalesOrders()
    '    Dim oGP As New Ngl.FM.GPIntegration.clsApplication()
    '    'Dim ocomp = oGP.GetCompanyList()
    '    'If Not ocomp Is Nothing AndAlso ocomp.Count() > 0 Then
    '    'For Each c In ocomp
    '    'Dim compkey = c.Key
    '    'Dim compID = c.Key.Id
    '    'Dim LegalEntity = Left(c.Name, 3)
    '    'System.Diagnostics.Debug.WriteLine(c.Name)
    '    'Dim ocustomers = oGP.GetCustomerList(companyKeyId:=compID)
    '    'If Not ocustomers Is Nothing AndAlso ocustomers.Count() > 0 Then
    '    '    For Each cust In ocustomers
    '    '        Dim custKeyID = cust.Key.Id
    '    '        Dim laneNumber As String = String.Concat(LegalEntity, "-", custKeyID, "-", cust.Name)
    '    '        System.Diagnostics.Debug.WriteLine("Lane Number {0}  ", laneNumber, "")
    '    '        'Dim oOrders = oGP.getSalesOrders(custKeyID, compID)
    '    '        'If Not oOrders Is Nothing AndAlso oOrders.Count() > 0 Then
    '    '        '    For Each o In oOrders
    '    '        '        System.Diagnostics.Debug.WriteLine("Order number: {0} Order amount: {1} ", o.Key.Id, o.TotalAmount.Value.ToString("C"))
    '    '        '    Next
    '    '        'End If
    '    '    Next

    '    'End If
    '    'Next

    '    'End If

    '    System.Diagnostics.Debug.WriteLine("Success!")

    'End Sub


    '<TestMethod()>
    'Public Sub TestGPSaleDocuments()
    '    Dim oGP As New Ngl.FM.GPIntegration.clsApplication()
    '    'Dim ocomp = oGP.GetCompanyList()
    '    'If Not ocomp Is Nothing AndAlso ocomp.Count() > 0 Then
    '    'For Each c In ocomp
    '    'Dim compkey = c.Key
    '    'Dim compID = c.Key.Id
    '    'Dim LegalEntity = Left(c.Name, 3)
    '    'System.Diagnostics.Debug.WriteLine(c.Name)
    '    'Dim ocustomers = oGP.GetCustomerList(companyKeyId:=compID)
    '    'If Not ocustomers Is Nothing AndAlso ocustomers.Count() > 0 Then
    '    '    For Each cust In ocustomers
    '    '        Dim custKeyID = cust.Key.Id
    '    '        Dim laneNumber As String = String.Concat(LegalEntity, "-", custKeyID, "-", cust.Name)
    '    '        System.Diagnostics.Debug.WriteLine("Lane Number {0}  ", laneNumber, "")
    '    '        Dim oOrders = oGP.getSalesDocuments(custKeyID, compID)
    '    '        If Not oOrders Is Nothing AndAlso oOrders.Count() > 0 Then
    '    '            For Each o In oOrders
    '    '                'If o.Type.ToString() = "Order" Then
    '    '                '    Dim oData = o.CustomerPONumber
    '    '                'End If
    '    '                System.Diagnostics.Debug.WriteLine("Order number: {0} Order amount: {1} Document Type: {2} ", o.Key.Id, o.TotalAmount.Value.ToString("C"), o.Type.ToString())
    '    '            Next
    '    '        End If
    '    '    Next
    '    'End If
    '    'Next

    '    'End If

    '    System.Diagnostics.Debug.WriteLine("Success!")

    'End Sub

    '<TestMethod()>
    'Public Sub TestPipeDelimiter()
    '    Dim sTestData As String = ""
    '    'added logic for multiple legal entities using a | (pipe) delimiter
    '    Dim sConfigLegals As String() = sTestData.Split("|")
    '    If sConfigLegals Is Nothing OrElse sConfigLegals.Count() < 1 Then
    '        sConfigLegals = New String() {sTestData}
    '    End If
    '    If Not sConfigLegals Is Nothing AndAlso sConfigLegals.Count() > 0 Then
    '        For Each sLegal In sConfigLegals
    '            Dim sthisLegal = sLegal
    '        Next
    '    End If
    'End Sub


    '<TestMethod()>
    'Public Sub GPIntegrationByFileTest()

    '    Dim sSource As String = "NGL.Integration.GPIntegrationByFileTest"
    '    Dim oApp As New GP.clsApplication
    '    Dim strFile = "C:\Data\IntegrationServicesData\GPServiceSettings.xml"

    '    Try
    '        Dim oDefaultSettings As GP.clsDefaultIntegrationConfiguration = readERPSettings(strFile)
    '        'change the ERPPayablesLastRunDate to determine if Payables will run.
    '        'leave at default for payables to not run
    '        oDefaultSettings.ERPPayablesLastRunDate = "12-19-2017" ' Date.Now.AddDays(-1)
    '        oApp.ProcessData(sSource, oDefaultSettings)
    '    Catch ex As ApplicationException
    '        Assert.Fail("Application Exception For " & sSource & ": {0} ", ex.Message)
    '    Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
    '        Throw
    '    Catch ex As Exception
    '        Assert.Fail("Unexpected Error For " & sSource & ": {0} ", ex.Message)
    '    Finally

    '    End Try
    'End Sub


    'Private Function readERPSettings(strFile) As GP.clsDefaultIntegrationConfiguration

    '    Dim xSettings As New GP.clsDefaultIntegrationConfiguration
    '    'use application configuration as default

    '    Dim sFile As String = strFile
    '    'check if the file exists:
    '    If System.IO.File.Exists(sFile) Then
    '        Dim doc As XDocument = XDocument.Load(sFile)
    '        xSettings = (From s In doc.Root.Elements("Setting")
    '                     Select New GP.clsDefaultIntegrationConfiguration With
    '                                    {
    '                                        .ERPTestURI = s.Element("ERPTestURI"),
    '                                        .ERPTestAuthUser = s.Element("ERPTestAuthUser"),
    '                                        .ERPTestAuthPassword = s.Element("ERPTestAuthPassword"),
    '                                        .ERPTestAuthCode = s.Element("ERPTestAuthCode"),
    '                                        .ERPTestLegalEntity = s.Element("ERPTestLegalEntity"),
    '                                        .ERPTestExportMaxRetry = getXMLInteger(s.Element("ERPTestExportMaxRetry")),
    '                                        .ERPTestExportRetryMinutes = getXMLInteger(s.Element("ERPTestExportRetryMinutes")),
    '                                        .ERPTestExportMaxRowsReturned = getXMLInteger(s.Element("ERPTestExportMaxRowsReturned")),
    '                                        .ERPTestExportAutoConfirmation = getXMLBool(s.Element("ERPTestExportAutoConfirmation")),
    '                                        .ERPTestFreightCost = getXMLDouble(s.Element("ERPTestFreightCost")),
    '                                        .ERPTestFreightBillNumber = s.Element("ERPTestFreightBillNumber"),
    '                                        .ERPFrieghtAccountIndex = s.Element("ERPFrieghtAccountIndex"),
    '                                        .TMSSettingsURI = s.Element("TMSSettingsURI"),
    '                                        .TMSSettingsAuthCode = s.Element("TMSSettingsAuthCode"),
    '                                        .TMSSettingsAuthUser = s.Element("TMSSettingsAuthUser"),
    '                                        .TMSSettingsAuthPassword = s.Element("TMSSettingsAuthPassword"),
    '                                        .TMSTestServiceURI = s.Element("TMSTestServiceURI"),
    '                                        .TMSTestServiceAuthCode = s.Element("TMSTestServiceAuthCode"),
    '                                        .TMSTestServiceAuthPassword = s.Element("TMSTestServiceAuthPassword"),
    '                                        .TMSTestServiceAuthUser = s.Element("TMSTestServiceAuthUser"),
    '                                        .RunERPTest = getXMLBool(s.Element("RunERPTest")),
    '                                        .Debug = getXMLBool(s.Element("Debug")),
    '                                        .Verbos = getXMLBool(s.Element("Verbos")),
    '                                        .TMSDBName = s.Element("TMSDBName"),
    '                                        .TMSDBServer = s.Element("TMSDBServer"),
    '                                        .TMSDBUser = s.Element("TMSDBUser"),
    '                                        .TMSDBPass = s.Element("TMSDBPass"),
    '                                        .TMSRunLegalEntity = s.Element("TMSRunLegalEntity"),
    '                                        .PayablesOn = getXMLBool(s.Element("PayablesOn")),
    '                                        .APExportOn = getXMLBool(s.Element("APExportOn")),
    '                                        .FrightBillCostsOn = getXMLBool(s.Element("FrightBillCostsOn")),
    '                                        .PurchaseOrderOn = getXMLBool(s.Element("PurchaseOrderOn")),
    '                                        .SalesOrderOn = getXMLBool(s.Element("SalesOrderOn")),
    '                                        .TransferOrderOn = getXMLBool(s.Element("TransferOrderOn")),
    '                                        .GPFunctionSOPCostUpdateOn = getXMLBool(s.Element("GPFunctionSOPCostUpdateOn")),
    '                                        .GPFunctionShipConfirmationOn = getXMLBool(s.Element("GPFunctionShipConfirmationOn")),
    '                                        .IntegrationTimerMS = getXMLInteger(s.Element("IntegrationTimerMS")),
    '                                        .GPFunctionsSOsToProcess = s.Element("GPFunctionsSOsToProcess"),
    '                                        .GPFunctionsSOConsToProcess = s.Element("GPFunctionsSOConsToProcess"),
    '                                        .GPFunctionsPOsToProcess = s.Element("GPFunctionsPOsToProcess"),
    '                                        .GPFunctionsSOHeaders = s.Element("GPFunctionsSOHeaders"),
    '                                        .GPFunctionsSOConHeaders = s.Element("GPFunctionsSOConHeaders"),
    '                                        .GPFunctionsPOHeaders = s.Element("GPFunctionsPOHeaders"),
    '                                        .GPFunctionsPOLocationCode = s.Element("GPFunctionsPOLocationCode"),
    '                                        .GPFunctionsTotalSOWeight = s.Element("GPFunctionsTotalSOWeight"),
    '                                        .GPFunctionsTotalSOConWeight = s.Element("GPFunctionsTotalSOConWeight"),
    '                                        .GPFunctionsTotalTOWeight = s.Element("GPFunctionsTotalTOWeight"),
    '                                        .GPFunctionsTotalPOWeight = s.Element("GPFunctionsTotalPOWeight"),
    '                                        .GPFunctionsTotalSOQuantity = s.Element("GPFunctionsTotalSOQuantity"),
    '                                        .GPFunctionsTotalSOConQuantity = s.Element("GPFunctionsTotalSOConQuantity"),
    '                                        .GPFunctionsTotalTOQuantity = s.Element("GPFunctionsTotalTOQuantity"),
    '                                        .GPFunctionsTotalPOQuantity = s.Element("GPFunctionsTotalPOQuantity"),
    '                                        .GPFunctionsTotalSOPallets = s.Element("GPFunctionsTotalSOPallets"),
    '                                        .GPFunctionsTotalTOPallets = s.Element("GPFunctionsTotalTOPallets"),
    '                                        .GPFunctionsTotalPOPallets = s.Element("GPFunctionsTotalPOPallets"),
    '                                        .GPFunctionsSOItemDetails = s.Element("GPFunctionsSOItemDetails"),
    '                                        .GPFunctionsSOConItemDetails = s.Element("GPFunctionsSOConItemDetails"),
    '                                        .GPFunctionsTOItemDetails = s.Element("GPFunctionsTOItemDetails"),
    '                                        .GPFunctionsPOItemDetails = s.Element("GPFunctionsPOItemDetails"),
    '                                        .GPFunctionsForceDefaultCountry = s.Element("GPFunctionsForceDefaultCountry"),
    '                                        .GPFunctionsGetSOShippingMethod = s.Element("GPFunctionsGetSOShippingMethod"),
    '                                        .GPFunctionsGetSOConShippingMethod = s.Element("GPFunctionsGetSOConShippingMethod"),
    '                                        .GPFunctionsGetTOShippingMethod = s.Element("GPFunctionsGetTOShippingMethod"),
    '                                        .GPFunctionsGetPOShippingMethod = s.Element("GPFunctionsGetPOShippingMethod"),
    '                                        .GPFunctionsGetSOTemp = s.Element("GPFunctionsGetSOTemp"),
    '                                        .GPFunctionsGetSOConTemp = s.Element("GPFunctionsGetSOConTemp"),
    '                                        .GPFunctionsGetTOTemp = s.Element("GPFunctionsGetTOTemp"),
    '                                        .GPFunctionsGetPOTemp = s.Element("GPFunctionsGetPOTemp"),
    '                                        .GPFunctionsPOComment = s.Element("GPFunctionsPOComment"),
    '                                        .GPFunctionsPONotes = s.Element("GPFunctionsPONotes"),
    '                                        .GPFunctionsSOPComment = s.Element("GPFunctionsSOPComment"),
    '                                        .GPFunctionsSOPNotes = s.Element("GPFunctionsSOPNotes"),
    '                                        .GPFunctionsGetPayableCreditGL = s.Element("GPFunctionsGetPayableCreditGL"),
    '                                        .GPFunctionsGetCurrencyID = s.Element("GPFunctionsGetCurrencyID"),
    '                                        .GPFunctionGetSOPOrdersOnFreightBill = s.Element("GPFunctionGetSOPOrdersOnFreightBill"),
    '                                        .GPFunctionsDefaultFreightGLAccount = s.Element("GPFunctionsDefaultFreightGLAccount"),
    '                                        .GPPMBatchPrefix = s.Element("GPPMBatchPrefix"),
    '                                        .GPBatchDateToAdd = getXMLInteger(s.Element("GPBatchDateToAdd")),
    '                                        .GPFunctionGetItemTemp = s.Element("GPFunctionGetItemTemp"),
    '                                        .GPFunctionCheckForSOPUserDefinedRecord = s.Element("GPFunctionCheckForSOPUserDefinedRecord"),
    '                                        .GPFunctionUpdateUserDefinedRecord = s.Element("GPFunctionUpdateUserDefinedRecord"),
    '                                        .GPPMPOFieldValue = s.Element("GPPMPOFieldValue"),
    '                                        .GPPMDescriptionFieldValue = s.Element("GPPMDescriptionFieldValue"),
    '                                        .GPFunctionInsertUserDefinedRecord = s.Element("GPFunctionInsertUserDefinedRecord")
    '                                    }
    '                                ).FirstOrDefault()


    '    End If
    '    Return xSettings

    'End Function


    '''' <summary>
    '''' returns the actual boolean provided using Boolean.TryParse or false
    '''' </summary>
    '''' <param name="s"></param>
    '''' <returns></returns>
    '''' <remarks></remarks>
    'Private Function getXMLBool(ByVal s As String) As Boolean
    '    Dim bVal As Boolean = False
    '    If Boolean.TryParse(s, bVal) Then
    '        Return bVal
    '    Else
    '        Return False
    '    End If
    'End Function

    '''' <summary>
    '''' returns the actual double provided using double.tryparse or 0
    '''' </summary>
    '''' <param name="s"></param>
    '''' <returns></returns>
    '''' <remarks></remarks>
    'Private Function getXMLDouble(ByVal s As String) As Double
    '    Dim dVal As Double = 0
    '    If Double.TryParse(s, dVal) Then
    '        Return dVal
    '    Else
    '        Return 0
    '    End If
    'End Function

    '''' <summary>
    '''' returns the actual Integer provided using Integer.tryparse or 0
    '''' </summary>
    '''' <param name="s"></param>
    '''' <returns></returns>
    '''' <remarks></remarks>
    'Private Function getXMLInteger(ByVal s As String) As Double
    '    Dim iVal As Integer = 0
    '    If Integer.TryParse(s, iVal) Then
    '        Return iVal
    '    Else
    '        Return 0
    '    End If
    'End Function

End Class