Imports NGL.Core.Utility.DataTransformation
Imports NGL.Core.Communication.Email
Imports NGL.Core.Communication.General
Imports GP = NGL.FM.GPIntegration

Public Class RunGPShipConf
    Private Shared EvtLog As New System.Diagnostics.EventLog
    Private Shared Debug As Boolean = False
    Private Shared oDefaultIntegrationConfiguration As New GP.clsDefaultIntegrationConfiguration()

    Public Shared Sub Main()
        Dim sSource As String = "NGL.FM.CMDLine.MailServer"
        Dim oApp As New GP.clsApplication

        EvtLog.Log = "Application"
        EvtLog.Source = sSource
        Try
            With oApp
                .Source = sSource
                If .readCommandLineArgs("NGL.FM.CMDLine.RunGPShipConf") = 0 Then
                    Exit Sub
                End If
                'we set the Debug flag based on the command line arguments not the EDI integration debug flag
                'so this must be called before readERPSettings
                Debug = .Debug
                'the GP integration uses web services so we do not need the database connection
                'If Not .validateDatabase() Then
                '    Exit Sub
                'End If
                'If Not .getTaskParameters Then
                '    Exit Sub
                'End If
                Try
                    If Debug Then
                        EvtLog.WriteEntry("Application Start", EventLogEntryType.Information)
                    End If
                Catch ex As Exception
                    'ignore any errors while writing to the event log
                End Try
                oDefaultIntegrationConfiguration = readERPSettings()
                If oDefaultIntegrationConfiguration Is Nothing Then Exit Sub
                'get the last run date
                'Dim dtLastRun As Date = My.Settings.LastRunDate
                'Modified by RHR for v-7.0.6.105 on 12/14/2017 because My.Settings.LastRunDate is lost durring upgrades or re-install we now save the setting in C:\ProgramData using Environment special folder settings
                'set the default to yesterday
                Dim dtLastRun As Date = Date.Now.AddDays(-1).ToShortDateString()
                'get the file and path
                Dim fSCSetting = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) & "\TMSRunGPSCSettings.dat"
                'encapsulate the read code in try catch to avoid file access errors
                Try

                    If System.IO.File.Exists(fSCSetting) Then
                        Dim sLine As String = ""
                        'all the settings are stored on one line
                        Using reader As System.IO.StreamReader = New System.IO.StreamReader(fSCSetting)
                            ' Read one line from file
                            sLine = reader.ReadLine
                        End Using
                        If Not String.IsNullOrWhiteSpace(sLine) Then
                            Date.TryParse(sLine, dtLastRun)
                        End If
                    End If
                Catch ex As Exception
                    'do nothing just log the results
                    If .Debug Then .Log("Read SC Settings Error: " & ex.ToString())
                End Try
                .Log("Running SC From: " & dtLastRun.ToString("yyyy-MM-dd HH:mm:ss"))
                'this procedure has not been written yet
                dtLastRun = .processSOPShipConfirmationCommandLine(sSource, oDefaultIntegrationConfiguration, dtLastRun)
                'update the last run date with the time stamp returned from ProcessGPSCOrders but check for bad dates
                If (dtLastRun = Date.MinValue Or dtLastRun = Date.MaxValue Or dtLastRun < Date.Now.ToShortDateString) Then
                    dtLastRun = Date.Now()
                End If
                If .Debug Then
                    .Log("Next SC Run Date: " & dtLastRun.ToString("yyyy-MM-dd HH:mm:ss"))
                End If
                'save the settings to the file
                Try
                    Using writer As System.IO.StreamWriter = New System.IO.StreamWriter(fSCSetting, False)
                        writer.WriteLine(dtLastRun)
                    End Using
                Catch ex As Exception
                    'do nothing
                    If .Debug Then .Log("Write SC Settings Error: " & ex.ToString())
                End Try

                My.Settings.LastRunDate = dtLastRun
            End With

        Catch ex As ApplicationException
            If Debug Then
                Console.WriteLine("Error! " & ex.ToString)
            End If
            EvtLog.WriteEntry(formatErrMsg(ex, sSource, Debug), EventLogEntryType.Error)

        Catch ex As Exception
            If Debug Then
                Console.WriteLine("Error! " & ex.ToString)
            End If
            EvtLog.WriteEntry(formatErrMsg(ex, sSource, Debug), EventLogEntryType.Error)
        Finally
            If Debug Then
                Console.WriteLine("Press Enter To Continue")
                Console.ReadLine()
            End If
        End Try
    End Sub

    Private Shared Function readERPSettings() As GP.clsDefaultIntegrationConfiguration

        Dim xSettings As New GP.clsDefaultIntegrationConfiguration
        Dim sFile As String = My.Settings.ERPSettingsFile
        Try
            'check if the file exists:
            If System.IO.File.Exists(sFile) Then
                Dim doc As XDocument = XDocument.Load(sFile)
                xSettings = (From s In doc.Root.Elements("Setting")
                             Select New GP.clsDefaultIntegrationConfiguration With
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
                                            .ERPFrieghtAccountIndex = s.Element("ERPFrieghtAccountIndex"),
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
                                            .PayablesOn = getXMLBool(s.Element("PayablesOn")),
                                            .APExportOn = getXMLBool(s.Element("APExportOn")),
                                            .FrightBillCostsOn = getXMLBool(s.Element("FrightBillCostsOn")),
                                            .PurchaseOrderOn = getXMLBool(s.Element("PurchaseOrderOn")),
                                            .SalesOrderOn = getXMLBool(s.Element("SalesOrderOn")),
                                            .TransferOrderOn = getXMLBool(s.Element("TransferOrderOn")),
                                            .GPFunctionSOPCostUpdateOn = getXMLBool(s.Element("GPFunctionSOPCostUpdateOn")),
                                            .GPFunctionShipConfirmationOn = getXMLBool(s.Element("GPFunctionShipConfirmationOn")),
                                            .IntegrationTimerMS = getXMLInteger(s.Element("IntegrationTimerMS")),
                                            .GPFunctionsSOsToProcess = s.Element("GPFunctionsSOsToProcess"),
                                            .GPFunctionsSOConsToProcess = s.Element("GPFunctionsSOConsToProcess"),
                                            .GPFunctionsPOsToProcess = s.Element("GPFunctionsPOsToProcess"),
                                            .GPFunctionsSOHeaders = s.Element("GPFunctionsSOHeaders"),
                                            .GPFunctionsSOConHeaders = s.Element("GPFunctionsSOConHeaders"),
                                            .GPFunctionsPOHeaders = s.Element("GPFunctionsPOHeaders"),
                                            .GPFunctionsPOLocationCode = s.Element("GPFunctionsPOLocationCode"),
                                            .GPFunctionsTotalSOWeight = s.Element("GPFunctionsTotalSOWeight"),
                                            .GPFunctionsTotalSOConWeight = s.Element("GPFunctionsTotalSOConWeight"),
                                            .GPFunctionsTotalTOWeight = s.Element("GPFunctionsTotalTOWeight"),
                                            .GPFunctionsTotalPOWeight = s.Element("GPFunctionsTotalPOWeight"),
                                            .GPFunctionsTotalSOQuantity = s.Element("GPFunctionsTotalSOQuantity"),
                                            .GPFunctionsTotalSOConQuantity = s.Element("GPFunctionsTotalSOConQuantity"),
                                            .GPFunctionsTotalTOQuantity = s.Element("GPFunctionsTotalTOQuantity"),
                                            .GPFunctionsTotalPOQuantity = s.Element("GPFunctionsTotalPOQuantity"),
                                            .GPFunctionsTotalSOPallets = s.Element("GPFunctionsTotalSOPallets"),
                                            .GPFunctionsTotalTOPallets = s.Element("GPFunctionsTotalTOPallets"),
                                            .GPFunctionsTotalPOPallets = s.Element("GPFunctionsTotalPOPallets"),
                                            .GPFunctionsSOItemDetails = s.Element("GPFunctionsSOItemDetails"),
                                            .GPFunctionsSOConItemDetails = s.Element("GPFunctionsSOConItemDetails"),
                                            .GPFunctionsTOItemDetails = s.Element("GPFunctionsTOItemDetails"),
                                            .GPFunctionsPOItemDetails = s.Element("GPFunctionsPOItemDetails"),
                                            .GPFunctionsForceDefaultCountry = s.Element("GPFunctionsForceDefaultCountry"),
                                            .GPFunctionsGetSOShippingMethod = s.Element("GPFunctionsGetSOShippingMethod"),
                                            .GPFunctionsGetSOConShippingMethod = s.Element("GPFunctionsGetSOConShippingMethod"),
                                            .GPFunctionsGetTOShippingMethod = s.Element("GPFunctionsGetTOShippingMethod"),
                                            .GPFunctionsGetPOShippingMethod = s.Element("GPFunctionsGetPOShippingMethod"),
                                            .GPFunctionsGetSOTemp = s.Element("GPFunctionsGetSOTemp"),
                                            .GPFunctionsGetSOConTemp = s.Element("GPFunctionsGetSOConTemp"),
                                            .GPFunctionsGetTOTemp = s.Element("GPFunctionsGetTOTemp"),
                                            .GPFunctionsGetPOTemp = s.Element("GPFunctionsGetPOTemp"),
                                            .GPFunctionsPOComment = s.Element("GPFunctionsPOComment"),
                                            .GPFunctionsPONotes = s.Element("GPFunctionsPONotes"),
                                            .GPFunctionsSOPComment = s.Element("GPFunctionsSOPComment"),
                                            .GPFunctionsSOPNotes = s.Element("GPFunctionsSOPNotes"),
                                            .GPFunctionsGetPayableCreditGL = s.Element("GPFunctionsGetPayableCreditGL"),
                                            .GPFunctionsGetCurrencyID = s.Element("GPFunctionsGetCurrencyID"),
                                            .GPFunctionGetSOPOrdersOnFreightBill = s.Element("GPFunctionGetSOPOrdersOnFreightBill"),
                                            .GPFunctionsDefaultFreightGLAccount = s.Element("GPFunctionsDefaultFreightGLAccount"),
                                            .GPPMBatchPrefix = s.Element("GPPMBatchPrefix"),
                                            .GPBatchDateToAdd = getXMLInteger(s.Element("GPBatchDateToAdd")),
                                            .GPFunctionGetItemTemp = s.Element("GPFunctionGetItemTemp"),
                                            .GPFunctionCheckForSOPUserDefinedRecord = s.Element("GPFunctionCheckForSOPUserDefinedRecord"),
                                            .GPFunctionUpdateUserDefinedRecord = s.Element("GPFunctionUpdateUserDefinedRecord"),
                                            .GPPMPOFieldValue = s.Element("GPPMPOFieldValue"),
                                            .GPPMDescriptionFieldValue = s.Element("GPPMDescriptionFieldValue"),
                                            .GPFunctionInsertUserDefinedRecord = s.Element("GPFunctionInsertUserDefinedRecord"),
                                            .GPAPAmountField = s.Element("GPAPAmountField"),
                                            .GPSOPOrdRequiredShipDate = s.Element("GPSOPOrdRequiredShipDate"),
                                            .GPSOPShipConfirmRequiredShipDate = s.Element("GPSOPShipConfirmRequiredShipDate")
                                        }
                                    ).FirstOrDefault()

            Else
                EvtLog.WriteEntry("RunGPShipConf.readERPSettings Failure  Config File Not Found: " & sFile, EventLogEntryType.Error, 10)
                Return Nothing
            End If
        Catch ex As Exception
            EvtLog.WriteEntry(formatErrMsg(ex, "RunGPShipConf.readERPSettings", Debug), EventLogEntryType.Error, 10)
        End Try
        Return xSettings

    End Function

    ''' <summary>
    ''' returns the actual boolean provided using Boolean.TryParse or false
    ''' </summary>
    ''' <param name="s"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function getXMLBool(ByVal s As String) As Boolean
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
    Private Shared Function getXMLDouble(ByVal s As String) As Double
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
    Private Shared Function getXMLInteger(ByVal s As String) As Double
        Dim iVal As Integer = 0
        If Integer.TryParse(s, iVal) Then
            Return iVal
        Else
            Return 0
        End If
    End Function

End Class
