Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.ServiceModel
Imports DAL = NGL.FreightMaster.Data
Imports DTO = NGL.FreightMaster.Data.DataTransferObjects
Imports LTS = NGL.FreightMaster.Data.LTS
Imports DTran = NGL.Core.Utility.DataTransformation
Imports Map = NGL.API.Mapping
Imports NGL.FM.CHRAPI
Imports System.Runtime.ConstrainedExecution
Imports NGL.FreightMaster.Data
Imports NGL.FreightMaster.Data.DataTransferObjects.tblLoadTender
Imports BSCEnum = NGL.FreightMaster.Data.DataTransferObjects.tblLoadTender.BidStatusCodeEnum
Imports Microsoft.Office.Interop.Excel
Imports NGL.API.Mapping.NGLAPI
Imports NGL.FreightMaster.Data.DataTransferObjects
Imports System.Threading.Tasks
Imports NGL.API.Mapping
Imports Microsoft.ApplicationInsights
Imports Newtonsoft.Json
Imports NGL.FreightMaster.Core.Model
Imports Microsoft.VisualBasic.Logging
Imports Serilog
Imports SerilogTracing

Public Class NGLAPIBLL : Inherits BLLBaseClass
#Region " Constructors "

    Public Sub New(ByVal oParameters As DAL.WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Me.SourceClass = "NGLAPIBLL"
        Logger = Logger.ForContext(Of NGLAPIBLL)
    End Sub

#End Region

#Region " Properties "



#End Region


#Region "Begin API Processing Logic"

    ''' <summary>
    ''' Standard Carrier API Rate Request Process for Rate Shopping
    ''' </summary>
    ''' <param name="order"></param>
    ''' <param name="LoadTenderControl"></param>
    ''' <param name="lAPICarrierConfigs"></param>
    ''' <param name="oResults"></param>
    ''' <param name="strMsg"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.4.004 on 01/19/2024
    ''' </remarks>
    Public Function ProcessAPIRateRequest(ByVal order As DAL.Models.RateRequestOrder,
                                      ByVal LoadTenderControl As Integer,
                                      ByVal lAPICarrierConfigs As List(Of LTS.tblSSOALEConfig),
                                      ByRef oResults As DTO.WCFResults,
                                      ByRef strMsg As String,
                                      Optional ByVal BookControl As Integer = 0) As Boolean
        ''' Block this on 09/28/2025 as part of performance tuning
        'Activity.Current.SetStatus(ActivityStatusCode.Ok, "ProcessAPIRateRequest_Start")

        '''Added this check to avoid null reference exception in case Activity.Current is Nothing
        ''' Added by Ayman on 09/28/2025
        If Activity.Current IsNot Nothing Then
            Activity.Current.SetStatus(ActivityStatusCode.Ok, "ProcessAPIRateRequest_Start")
        Else
            Logger.Information("ProcessAPIRateRequest_Start (Activity.Current is Nothing)")
        End If

        'telemetry.TrackTrace("ProcessAPIRateRequest_Start", DataContracts.SeverityLevel.Information, New Dictionary(Of String, String) From {
        '                        {"order", JsonConvert.SerializeObject(order)},
        '                        {"LoadTenderControl", LoadTenderControl},
        '                        {"lAPICarrierConfigs", JsonConvert.SerializeObject(lAPICarrierConfigs)}
        '                    })

        Dim oResponse As New Map.QuoteResponse()
        Using operation = Logger.StartActivity("ProcessAPIRequest(Order: {@Order}, LoadTenderControl: {LoadTenderControl}, APICarrierConfigs: {APICarrierConfigs}, results: {Results}, strMsg: {strMsg}, BookControl: {BookControl}", order, LoadTenderControl, lAPICarrierConfigs, oResults, strMsg, BookControl)




            ' Initialize common variables
            Dim lAccessorial As New List(Of DTO.BookFee)
            Dim origCompControl As Integer = 0
            Dim destCompControl As Integer = 0
            Dim iCompControl As Integer = 0
            Dim iDefaultFreightClass As Integer = 100
            Dim oMapRateRequestOrder As New Map.RateRequestOrder()
            Dim oRateRequest As New Map.RateRequest()
            Dim sMsg As String = ""
            Dim skipObjs As New List(Of String) From {"Packages", "Pickup", "Stops"}
            Dim blnContinue As Boolean = True
            Dim intVersion As Integer = 0
            Dim bUseTLS12 As Boolean = True
            Dim dtShipDate = Date.Today
            Dim sCarrierNumber As String = "0" ' Moved declaration outside of Parallel.ForEach

            ' Prepare data depending on BookControl
            If BookControl <> 0 Then
                Logger.Information("NGLAPIBLL.ProcessAPIRateRequest NGLBookRevenueData.GetLTLvBookRevenues: {BookControl}", BookControl)
                Dim oLTLs As LTS.vBookRevenue() = NGLBookRevenueData.GetLTLvBookRevenues(BookControl)
                If oLTLs Is Nothing OrElse oLTLs.Count() < 1 Then

                    ''' Check Activity null exception. Added on 28-09-2025
                    If Activity.Current IsNot Nothing Then
                        Activity.Current.AddEvent(New ActivityEvent("No LTL Book Revenue data found for BookControl: " & BookControl))
                    End If
                    Logger.Information("NGLAPIBLL.ProcessAPIRateRequest No LTL Book Revenue data found for BookControl: {BookControl}", BookControl)
                    Return True ' Nothing to do but we did not fail
                End If
                iCompControl = oLTLs(0).BookCustCompControl

                intVersion = NGLLoadTenderData.GetParValue("APIRateQuoteVersion", iCompControl)
                bUseTLS12 = If(NGLLoadTenderData.GetParValue("GlobalTurnOnTLs12ForAPIs", iCompControl) = 0, False, True)
                Integer.TryParse(NGLLoadTenderData.GetParText("UseFAKDefault", iCompControl), iDefaultFreightClass)
                Logger.Information("UseFAKDefault: {iDefaultFreightClass}", iDefaultFreightClass)
                Dim oPickup As New LTS.vBookRevenue()
                Logger.Information("NGLAPIBLL.ProcessAPIRateRequest CopyBookDataToAPIData: {BookControl}, oLTLs: {@oLTLs}, oPickup: {@oPickup}, defaultFreightClass: {iDefaultFreightClass}", BookControl, oLTLs, oPickup, iDefaultFreightClass)
                oRateRequest = CopyBookDataToAPIData(oLTLs, oPickup, iDefaultFreightClass)

                If oRateRequest Is Nothing Then

                    ''' Check Activity null exception. Added on 28-09-2025
                    If Activity.Current IsNot Nothing Then
                        Activity.Current.AddEvent(New ActivityEvent("CopyBookDataToAPIData returned null for BookControl: " & BookControl))
                    End If
                    Logger.Information("NGLAPIBLL.ProcessAPIRateRequest CopyBookDataToAPIData returned null for BookControl: {BookControl}", BookControl)
                    Return True ' Nothing to do but we did not fail
                End If

            Else
                If order.Accessorials Is Nothing Then
                    order.Accessorials = New String() {}
                Else
                    Dim obookAcc = New NGLBookAccessorial(Me.Parameters)
                    Dim lNewFees = New List(Of String)
                    For Each sVal In order.Accessorials
                        Logger.Information("NGLAPIBLL.ProcessAPIRateRequest Finding GetAccessorialEDICodeFromNACCode: {sVal}", sVal)
                        lNewFees.Add(obookAcc.GetAccessorialEDICodeFromNACCode(sVal, sVal))
                    Next
                    order.Accessorials = lNewFees.ToArray()
                End If
                Logger.Information("NGLAPIBLL.ProcessAPIRateRequest GetInfoForLTRateQuoteTariffBids: {LoadTenderControl}, Order.Accessorials: {@Accessorials}, origCompControl: {origCompControl}, lAccessorial: {@lAccessorial}, Order.AccessorialValues: {@AccessorialValues}", LoadTenderControl, order.Accessorials, order.Accessorials, origCompControl, lAccessorial, order.AccessorialValues)
                blnContinue = NGLLoadTenderData.GetInfoForLTRateQuoteTariffBids(LoadTenderControl, order.Accessorials, origCompControl, destCompControl, lAccessorial, order.AccessorialValues)

                If Not blnContinue Then
                    Logger.Information("NGLAPIBLL.ProcessAPIRateRequest GetInfoForLTRateQuoteTariffBids returned false for LoadTenderControl: {LoadTenderControl}", LoadTenderControl)
                    Return False ' Nothing to do
                End If
                Logger.Information("NGLAPIBLL.ProcessAPIRateRequest GetInfoForLTRateQuoteTariffBids returned true for LoadTenderControl: {LoadTenderControl}", LoadTenderControl)
                intVersion = NGLLoadTenderData.GetParValue("APIRateQuoteVersion", origCompControl)
                bUseTLS12 = If(NGLLoadTenderData.GetParValue("GlobalTurnOnTLs12ForAPIs", origCompControl) = 0, False, True)

                If Not Date.TryParse(order.ShipDate, dtShipDate) Then
                    order.ShipDate = dtShipDate.ToString()
                End If
                If dtShipDate < Date.Today Then Return False ' Invalid ship date

                If order Is Nothing OrElse order.Stops Is Nothing OrElse order.Stops.Count() < 1 Then
                    'Throw New InvalidOperationException("Shipping information is missing stop data")
                    Logger.Warning("Order is invalid or no Stops were found in the order")
                    operation.Complete(Events.LogEventLevel.Warning)

                    Return False
                End If

                iCompControl = order.BookCustCompControl
                Logger.Information("NGLAPIBLL.ProcessAPIRateRequest GetParText: {UseFAKDefault}, iCompControl: {iCompControl}", "UseFAKDefault", iCompControl)
                Integer.TryParse(NGLLoadTenderData.GetParText("UseFAKDefault", iCompControl), iDefaultFreightClass)
                Logger.Information("Mapping RateRequestOrder: {@order}", order)
                oMapRateRequestOrder = order.MapNGLAPIRateRequestOrder()
                ' Map temperature to item details for RateShop
                Logger.Information("Setting temperature to {CommCodeType}", order.CommCodeType)
                Dim sOrderTemperature As String = order.CommCodeType
                oMapRateRequestOrder.CommCodeType = sOrderTemperature
                Logger.Information("NGLAPIBLL.ProcessAPIRateRequest - Mapping RateRequestOrder: {@oMapRateRequestOrder}", oMapRateRequestOrder)

                If oMapRateRequestOrder.Pickup IsNot Nothing AndAlso oMapRateRequestOrder.Pickup.Items IsNot Nothing Then
                    For Each oItem In oMapRateRequestOrder.Pickup.Items

                        Logger.Information("Mapping RateRequestOrder Pickup Items: {sOrderTemperature}", sOrderTemperature)
                        oItem.ItemTemperature = Map.RateRequestItem.GetTemperatureCode(sOrderTemperature)
                    Next
                End If

                If oMapRateRequestOrder.Stops IsNot Nothing Then
                    For Each oStop In oMapRateRequestOrder.Stops
                        If oStop.Items IsNot Nothing Then
                            For Each oItem In oStop.Items

                                oItem.ItemTemperature = Map.RateRequestItem.GetTemperatureCode(sOrderTemperature)
                                Logger.Information("Mapping RateRequestOrder Stop Items: {sOrderTemperature} - {ItemTemperature}", sOrderTemperature, oItem.ItemTemperature)
                            Next
                        End If
                    Next
                End If
            End If

            ' Parallel processing for each carrier configuration
            Dim localStrMsg As String = strMsg
            Dim localResults As DTO.WCFResults = oResults

            Parallel.ForEach(lAPICarrierConfigs, Sub(oLEConfig)
                                                     Try
                                                         Dim sCarrierNumberLocal As String = "0"
                                                         Dim lCompConfigs As List(Of LTS.tblSSOAConfig) = NGLtblSingleSignOnAccountData.GetSSOAConfigs(oLEConfig.SSOALEControl)

                                                         If lCompConfigs IsNot Nothing AndAlso lCompConfigs.Count() > 0 Then
                                                             If lCompConfigs.Any(Function(x) x.SSOACName = "CarrierNumber") Then
                                                                 sCarrierNumberLocal = lCompConfigs.Where(Function(x) x.SSOACName = "CarrierNumber").Select(Function(d) d.SSOACValue).FirstOrDefault()
                                                             End If
                                                         End If

                                                         Dim oMapLEConfig As New Map.SSOALEConfig(oLEConfig.SSOALEClientID, oLEConfig.SSOALEClientSecret, oLEConfig.SSOALELoginURL, oLEConfig.SSOALEAuthCode, oLEConfig.SSOALEDataURL)
                                                         Dim sRetMsg As String = ""
                                                         Logger.Information("NGLAPIBLL.ProcessAPIRateRequest [{sCarrierNumber}] - Mapping SSOALEConfig: {@oMapLEConfig}", oMapLEConfig, sCarrierNumber)
                                                         Dim lMapSSOAConfigs As List(Of Map.SSOAConfig) = (From e In lCompConfigs Select Map.SSOAConfig.selectMapData(e, sRetMsg)).ToList()
                                                         Logger.Information("NGLAPIBLL.ProcessAPIRateRequest [{sCarrierNumber}] - Mapping SSOAConfigs: {@lMapSSOAConfigs}", lMapSSOAConfigs, sCarrierNumber)
                                                         Select Case oLEConfig.SSOALESSOAControl
                                                             Case 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25
                                                                 Dim oAPI As NGLAPI = Nothing
                                                                 Select Case oLEConfig.SSOALESSOAControl
                                                                     Case 10 : oAPI = New NGL.FM.CHRAPI.CHRAPI(bUseTLS12)
                                                                     Case 16 : oAPI = New DTMS.API.HMBay.HMBayAPI()
                                                                     Case 17 : oAPI = New DTMS.API.FFE.FFEAPI()
                                                                     Case 24 : oAPI = New DTMS.API.GTZ.GTZAPI()
                                                                     Case 25 : oAPI = New NGL.FM.Estes.EstesApi()
                                                                         ' Add other cases as needed
                                                                 End Select

                                                                 If oAPI IsNot Nothing Then
                                                                     Dim lSpecialFees As New List(Of Map.RateRequest.SpecialRequirement)
                                                                     Dim oAPIResponses As New List(Of Map.QuoteResponse)

                                                                     If BookControl <> 0 Then
                                                                         oAPIResponses = oAPI.GetBid(oRateRequest, LoadTenderControl, oMapLEConfig, lMapSSOAConfigs, localStrMsg, iDefaultFreightClass)
                                                                     Else
                                                                         oAPIResponses = oAPI.ProcessRateRequest(oMapRateRequestOrder, LoadTenderControl, oMapLEConfig, lMapSSOAConfigs, lSpecialFees, localStrMsg, iDefaultFreightClass)
                                                                     End If

                                                                     If oAPIResponses IsNot Nothing AndAlso oAPIResponses.Count() > 0 Then
                                                                         SyncLock localResults
                                                                             For Each oAPIResp As Map.QuoteResponse In oAPIResponses
                                                                                 If oAPIResp.success Then

                                                                                     Logger.Information("NGLAPIBLL.ProcessAPIRateRequest [{sCarrierNumber}] - LoadTenderControl: {LoadTenderControl}, InsertAPIRateQuoteBids: {@oAPIResp}", sCarrierNumberLocal, LoadTenderControl, oAPIResp)
                                                                                     InsertAPIRateQuoteBids(LoadTenderControl, oAPIResp, sCarrierNumberLocal, localResults)


                                                                                 End If
                                                                             Next
                                                                         End SyncLock
                                                                     End If
                                                                 End If

                                                             Case Else
                                                                 ' Handle any other specific cases if necessary
                                                         End Select

                                                     Catch ex As Exception
                                                         SyncLock localResults
                                                             Logger.Error(ex, "NGLAPIBLL.ProcessAPIRateRequest Error: {@ex}", ex)
                                                             'telemetry.TrackTrace("ProcessAPIRateRequest_Error", DataContracts.SeverityLevel.Error, New Dictionary(Of String, String) From {
                                                             '                            {"ex", ex.ToString()}
                                                             '                        })
                                                             Dim sParms As New List(Of String)
                                                             sParms.Add(LoadTenderControl.ToString())
                                                             sParms.Add("Process API rates failed for carrier number:  " & sCarrierNumber)
                                                             localResults.AddMessage(DTO.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.E_SaveRateFailure, sParms.ToArray())
                                                         End SyncLock
                                                     End Try

                                                 End Sub)

            strMsg = localStrMsg
            oResults = localResults


        End Using

        Return oResponse.success

    End Function




    Public Function CreateAPIBid(ByVal BookControl As Integer,
                                 ByVal LoadTenderControl As Integer,
                                 ByVal SHID As String,
                                 ByVal lAPICarrierConfigs As List(Of LTS.tblSSOALEConfig),
                                 ByRef oResults As DTO.WCFResults,
                                 ByRef strMsg As String) As Boolean

        Dim oLTLs As LTS.vBookRevenue() = NGLBookRevenueData.GetLTLvBookRevenues(BookControl)
        If oLTLs Is Nothing OrElse oLTLs.Count() < 1 Then Return True 'nothing to do but we did not fail
        Dim origST = oLTLs(0).BookOrigState
        Dim destST = oLTLs(0).BookDestState
        Dim dtNow = Date.Now

        Dim lCompConfig As List(Of LTS.tblSSOAConfig)
        Dim oResponse As New Map.QuoteResponse()
        Dim iCompControl As Integer = 0
        Dim iDefaultFreightClass As Integer = 100
        iCompControl = oLTLs(0).BookCustCompControl
        Dim intVersion = NGLLoadTenderData.GetParValue("APIRateQuoteVersion", iCompControl)
        Dim bUseTLS12 As Boolean = If(NGLLoadTenderData.GetParValue("GlobalTurnOnTLs12ForAPIs", iCompControl) = 0, False, True)
        'Dim oChrAPI As New CHR.CHRAPI(bUseTLS12)
        'get a default FAK class
        Integer.TryParse(NGLLoadTenderData.GetParText("UseFAKDefault", iCompControl), iDefaultFreightClass)

        Dim oPickup As New LTS.vBookRevenue()
        Dim oRateRequest As Map.RateRequest = CopyBookDataToAPIData(oLTLs, oPickup, iDefaultFreightClass)

        If oRateRequest Is Nothing Then Return True 'nothing to do but we did not fail


        Dim sCarrierNumber As String = "0"
        For Each oLEConfig In lAPICarrierConfigs
            Select Case oLEConfig.SSOALEControl
                Case 10 'CHR API
                    Dim oChrAPI As New NGL.FM.CHRAPI.CHRAPI(bUseTLS12)
                    Dim lSpecialFees As New List(Of Map.RateRequest.SpecialRequirement)
                    'Dim oAPIResponse = oChrAPI.ProcessRateRequest(oMapRateRequestOrder, LoadTenderControl, oMapLEConfig, lMapSSOAConfigs, lSpecialFees, strMsg, iDefaultFreightClass)
                    'If oAPIResponse.success Then
                    '    ' we now insert a quote with error messages even if one is not available 
                    '    ' using the postMessagesOnly flag with a zero cost.  This logic will help
                    '    ' users track issues with API rating
                    '    InsertAPIRateQuoteBids(LoadTenderControl, oAPIResponse, sCarrierNumber)
                    'End If
                    'Imports CHR = NGL.FM.CHRAPI
                    'Imports UPS = NGL.FM.UPSAPI
                    'Imports JTS = NGL.FM.JTSAPI
                    'Imports Map = NGL.API.Mapping
                Case 11  'UPS API
                Case 12  'YRC API
                Case 13  'JTS API
                Case 14  'FedX API
                Case 15  'Engage Lane API
                Case 16  'HMBay API
                Case 17  'FFE API
                Case 18  'EVANSTS API
                Case 19  'FROZENLOG API
                Case 20  'HUDSON API
                Case 21  'JBPARTNERS API
                Case 22  'LANTER API
                Case 23  'TQL API
                Case 24  'GTZ API
                Case Else
                    'the two lines of code need to be replace they are just to get past compile errors

                    If Not lCompConfig Is Nothing AndAlso lCompConfig.Count() > 0 Then
                        If (lCompConfig.Any(Function(x) x.SSOACName = "CarrierNumber")) Then sCarrierNumber = lCompConfig.Where(Function(x) x.SSOACName = "CarrierNumber").Select(Function(d) d.SSOACValue).FirstOrDefault()
                    End If
                    Dim ChrLEConfig As Map.SSOALEConfig = New Map.SSOALEConfig(oLEConfig.SSOALEClientID, oLEConfig.SSOALEClientSecret, oLEConfig.SSOALELoginURL, oLEConfig.SSOALEAuthCode, oLEConfig.SSOALEDataURL)
                    Dim sRetMsg As String = ""
                    Dim ChrSSOAConfigs As List(Of Map.SSOAConfig) = (From e In lCompConfig Select Map.SSOAConfig.selectMapData(e, sRetMsg)).ToList()
                    'the line of code below needs to be replace they are just to get past compile errors

                    'Dim oAPIResponse = oChrAPI.GetBid(oRateRequest, LoadTenderControl, ChrLEConfig, ChrSSOAConfigs, strMsg, iDefaultFreightClass)
                    'If oResponse.success Then
                    '    ' we now insert a quote with error messages even if one is not available 
                    '    ' using the postMessagesOnly flag with a zero cost.  This logic will help
                    '    ' users track issues with API rating
                    '    InsertCHRRateQuoteBids(LoadTenderControl, oResponse, sCarrierNumber)
                    'End If
            End Select


        Next
        Return oResponse.success

    End Function



    ''' <summary>
    ''' Add the quotes to the bid table
    ''' </summary>
    ''' <param name="LoadTenderControl"></param>
    ''' <param name="oResponse"></param>
    ''' <remarks>
    ''' TODO add logic to reference fees based on standard mapping 
    ''' this procedure uses CHR defaults
    ''' Modified by RHR for v-8.5.4.005 on 02/14/2024 added Flat for UOM
    ''' Modified by RHR for v-8.5.4.005 on 02/19/2024 added logic to use new API specific Rate mapping values
    ''' Modified by RHR for v-8.5.4.005 on 02/21/2024
    '''     added logic to save the JSON Rate Request data
    '''     And the JSON Rate Response Data
    '''     the caller/receiver will store the data in the
    '''     BidInfos for outbound And BidWarnings for inbound data
    ''' Modified by RHR for v-8.5.4.005 on 02/23/2024 added oResults As DTO.WCFResults data
    '''     we never trow and exception from this method because other API data is still generated
    '''     instead we update the error messges in the WCFResults object
    ''' </remarks>
    Public Function InsertAPIRateQuoteBids(ByVal LoadTenderControl As Integer, ByVal oResponse As Map.QuoteResponse, ByVal sCarrierNumber As String, ByRef oResults As DTO.WCFResults, Optional sFreightClass As String = "Truck") As Boolean
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            
            Using insertActivityOperation = Logger.StartActivity("InsertAPIRateQuoteBids(LoadTenderControl: {LoadTenderControl}, oResponse: {@oResponse}, sCarrierNumber: {sCarrierNumber}, oResults: {@oResults}, sFreightClass: {sFreightClass})", LoadTenderControl, oResponse, sCarrierNumber, oResults, sFreightClass)

                Dim oLoadTender = db.tblLoadTenders.Where(Function(x) x.LoadTenderControl = LoadTenderControl).FirstOrDefault()
                Dim sRetMSg As String = ""
                'If Not CreateNGLAPIBid(oResponse, oLoadTender.LoadTenderControl, oLoadTender.LTBookSHID, oLoadTender.LTBookOrigState, oLoadTender.LTBookDestState, 0, sRetMSg, 0, oLoadTender.LTBookDateRequired, BSCEnum.Quoted) Then
                '    throwUnExpectedFaultException(sRetMSg)
                'End If
                'Begin 

                Dim SHID As String = oLoadTender.LTBookSHID
                Dim origST As String = oLoadTender.LTBookOrigState
                Dim destST As String = oLoadTender.LTBookDestState
                insertActivityOperation.AddProperty("SHID", SHID)
                
                Dim intCompControl As Integer = 0
                Dim strMsg As String = ""
                Dim BookControl As Integer = 0
                Dim DefaultRequiredDate As Date? = oLoadTender.LTBookDateRequired
                Dim bStatusCode As BSCEnum = BSCEnum.Active

                Dim sCarrierInfo As String = " Carrier number: " & sCarrierNumber
                If (Not oResponse Is Nothing AndAlso Not oResponse.quoteSummaries Is Nothing AndAlso oResponse.quoteSummaries.Count() > 0 AndAlso Not oResponse.quoteSummaries(0).carrier Is Nothing) Then
                    'sCarrierInfo =
                    If Not String.IsNullOrWhiteSpace(oResponse.quoteSummaries(0).carrier.carrierName) Then
                        sCarrierInfo = " Carrier name: " & oResponse.quoteSummaries(0).carrier.carrierName
                    Else
                        If Not String.IsNullOrWhiteSpace(oResponse.quoteSummaries(0).carrier.carrierNumber) Then
                            sCarrierInfo = " Carrier number: " & oResponse.quoteSummaries(0).carrier.carrierNumber
                        End If
                    End If
                End If

                If LoadTenderControl = 0 Then
                    Dim sParms As New List(Of String)
                    sParms.Add("0")
                    sParms.Add("Load Tendered Reference cannot be found for  " & sCarrierInfo)
                    oResults.AddMessage(DTO.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.E_SaveRateFailure, sParms.ToArray())
                    Return False
                End If

                Dim sSCAC = oLoadTender.LTCarrierSCAC
                Dim iCarrierControl As Integer = 0
                Dim iCarrierNumber As Integer = 0
                Dim sCNS = oLoadTender.LTBookConsPrefix
                Dim sOrderNbr = oLoadTender.LTBookCarrOrderNumber
                Dim blnInvalidData As Boolean = False
                Dim sMsgDetails = String.Format("Using Carrier Number {0},  LoadTenderControl {1}, CNS {2}, SCAC {3}, Order No. {4}", sCarrierNumber, LoadTenderControl, sCNS, sSCAC, sOrderNbr)
                insertActivityOperation.AddProperty("CNS", sCNS)
                insertActivityOperation.AddProperty("OrderNumber", sOrderNbr)
                If Not Integer.TryParse(sCarrierNumber, iCarrierNumber) Then
                    Dim sParms As New List(Of String)
                    sParms.Add(LoadTenderControl.ToString())
                    sParms.Add("Invalid Carrier for " & sMsgDetails)
                    Logger.Warning("Invalid Carrier for {sMsgDetails}", sMsgDetails)
                    oResults.AddMessage(DTO.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.E_SaveRateFailure, sParms.ToArray())
                    Return False
                End If
                Dim oCarrier = db.CarrierRefIntegrations.FirstOrDefault(Function(x) x.CarrierNumber = iCarrierNumber)
                If oCarrier Is Nothing OrElse oCarrier.CarrierControl = 0 Then
                    Dim sParms As New List(Of String)
                    sParms.Add(LoadTenderControl.ToString())
                    sParms.Add("Invalid Carrier for " & sMsgDetails)
                    Logger.Warning("Invalid Carrier - Was not found ini CarrierRefIntegrations Table for {CarrierNumber}",iCarrierNumber)
                    oResults.AddMessage(DTO.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.E_SaveRateFailure, sParms.ToArray())
                    Return False
                End If
                Dim sCarrierName = oCarrier.CarrierName
                sSCAC = oCarrier.CarrierSCAC
                iCarrierControl = oCarrier.CarrierControl

                insertActivityOperation.AddProperty("CarrierName", sCarrierName)
                insertActivityOperation.AddProperty("SCAC", sSCAC)


                Dim sLoadDetails = String.Format("Using Carrier {0},  LoadTenderControl {1}, CNS {2}, SCAC {3}, Order No. {4}", sCarrierName, LoadTenderControl, sCNS, sSCAC, sOrderNbr)
                Dim dtNow As Date = Date.Now
                Dim dtDefaultRequiredDate As Date = If(DefaultRequiredDate, dtNow.AddDays(3)) ' TODO: add transit time

                If oResponse Is Nothing OrElse oResponse.quoteSummaries Is Nothing OrElse oResponse.quoteSummaries.Count() < 1 Then
                    If oResponse Is Nothing Then oResponse = New Map.QuoteResponse()
                    oResponse.AddMessage(Map.APIMessage.MessageEnum.E_NoRatesFound, sLoadDetails, "", "")
                    blnInvalidData = True
                    Logger.Warning("Response object was null or no quote summary objects existed...{oResponse}", oResponse)
                End If


                Try
                    Dim Sec As New NGLSecurityDataProvider(Parameters)
                    If intCompControl = 0 Then

                        intCompControl = Sec.getLECompControl()
                    End If
                    Logger.Information("check RestrictedCarriersForSalesReps")
                    Dim lCarrier As List(Of Integer) = Sec.RestrictedCarriersForSalesReps()
                    If ((Not lCarrier Is Nothing) AndAlso (lCarrier.Count() > 0) AndAlso (Not lCarrier.Contains(iCarrierControl))) Then
                        Dim sParms As New List(Of String)
                        sParms.Add(LoadTenderControl.ToString())
                        sParms.Add("Rates are restricted  for: " & sMsgDetails & ".  Not a preferred carrier.")
                        Logger.Warning("Rates are restricted for {sMsgDetails}.  Not a preferred carrier.", sMsgDetails)
                        oResults.AddMessage(DTO.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.E_SaveRateFailure, sParms.ToArray())
                        Return False
                    End If
                    If Not blnInvalidData And Not oResponse.postMessagesOnly Then
                        For Each q As Map.QuoteSummary In oResponse.quoteSummaries
                            Logger.Information("Processing QuoteSummary: {@QuoteSummary}", q)
                            sLoadDetails = String.Format("Using Carrier {0},  LoadTenderControl {1}, CNS {2}, SCAC {3}, Order No. {4}", oCarrier.CarrierName, LoadTenderControl, sCNS, sSCAC, sOrderNbr)
                            Logger.Information("Using Carrier {CarrierName}, LoadTenderControl: {LoadTenderControl}, CNS: {CNS}, SCAC: {SCAC}, Order No: {OrderNumber}", oCarrier.CarrierName, LoadTenderControl, sCNS, sSCAC, sOrderNbr)
                            Dim oCarrierData As New LTS.spGetBidCarrierBySCACUsingCompLegalEntityResult()
                            'Modified by RHR for v-8.5.4.005 on 03/19/2024 added multi-carrier lookup logic
                            Dim soCarrierDataName As String = "Unknown"
                            If q.carrier Is Nothing Then
                                Logger.Warning("Carrier is missing for {sLoadDetails}.  Using default carrier. {SCAC}", sLoadDetails, oCarrier.CarrierSCAC)
                                sSCAC = oCarrier.CarrierSCAC
                            Else
                                sSCAC = If(String.IsNullOrWhiteSpace(q.carrier.scac), oCarrier.CarrierSCAC, q.carrier.scac)
                                soCarrierDataName = If(String.IsNullOrWhiteSpace(q.carrier.carrierName), soCarrierDataName, q.carrier.carrierName)
                            End If
                            If Not String.IsNullOrEmpty(sSCAC) Then
                                'get the carrier data
                                Logger.Information("spGetBidCarrierBySCACUsingCompLegalEntity: {intCompControl}, {SCAC}", intCompControl, sSCAC)
                                oCarrierData = db.spGetBidCarrierBySCACUsingCompLegalEntity(intCompControl, sSCAC).FirstOrDefault()
                            End If
                            If oCarrierData Is Nothing OrElse oCarrierData.CarrierControl = 0 Then
                                Logger.Warning("Carrier data is missing for {sLoadDetails}.  Using default carrier. {SCAC}", sLoadDetails, sSCAC)
                                InsertAPIRateQuoteBidError(soCarrierDataName, sSCAC, oCarrier.CarrierSCAC, oLoadTender, oResults, Map.APIMessage.MessageEnum.E_InvalidCarrierNumber, sLoadDetails, "Invalid Carrier Configuration for API", "")
                                Continue For 'We still want to try to create any other bids -- the strMsg results should be logged by the caller
                            End If
                            'Modified by RHR for v-8.5.4.005 on 03/19/2024 added preferred carrier restrictions
                            If ((Not lCarrier Is Nothing) AndAlso (lCarrier.Count() > 0) AndAlso (Not lCarrier.Contains(oCarrierData.CarrierControl))) Then
                                Logger.Warning("Carrier is restricted for {sLoadDetails}.  Not a preferred carrier.", sLoadDetails)
                                InsertAPIRateQuoteBidError(soCarrierDataName, sSCAC, oCarrier.CarrierSCAC, oLoadTender, oResults, Map.APIMessage.MessageEnum.E_CarrierRestricted, sLoadDetails, "This provider is not one of the preferred transportation providers assigned to the lane.", "")
                                Continue For
                            End If

                            Dim fuelTotal As Decimal = 0
                            Dim fuelVar As Decimal = 0
                            Dim dLIneHaul As Decimal = 0
                            Dim Adjs As New List(Of LTS.tblBidCostAdj)
                            Dim iBidAdjustmentCount As Integer = 0
                            Dim dblCarrierCostUpcharge As Decimal = NGLLegalEntityCarrierObjData.GetCarrierUpliftValue(iCarrierControl, intCompControl)
                            Logger.Information("dblCarrierCostUpcharge: {dblCarrierCostUpcharge}", dblCarrierCostUpcharge)
                            Dim decBidCustAdjustments As Decimal = 0
                            '-------------------
                            'Hack override [[INSERT_TARIFF_API]]
                            'Dim oAccOverrider = New NGL.Test.Core.AccessorialOverrider()
                            'Dim oAccApiSource = sSCAC
                            'Dim oNewAccList = oAccOverrider.HackIt(oAccApiSource)
                            'Dim updatedRates As New List(Of Rate)(q.rates) ' Convert array to a list for modification
                            'If (Not oNewAccList Is Nothing And oNewAccList.Count() > 0) Then
                            '    Logger.Information($"Calling AccessorialOverrider to overide the accessorials. API={oAccApiSource}, OverrideCount={oNewAccList.Count()}")
                            '    For Each oAcc In oNewAccList
                            '        ' Check if the rateCode from oAcc is not in q.rates
                            '        'Hard override by removing it
                            '        If Not q.rates.Any(Function(rate) rate.rateCodeValue = oAcc.EdiCode) Then
                            '            ' Create a new Rate object and populate its properties
                            '            Dim newRate As New Rate() With {
                            '                .rateCodeValue = oAcc.EdiCode,
                            '                .totalRate = oAcc.NumericValue, ' Assuming oAcc has a Value property
                            '                .description = oAcc.Description,
                            '                .quantity = 1,   ' Assuming oAcc has a Quantity property
                            '                .unitRate = 0,   ' Assuming oAcc has a UnitRate property
                            '                .adjustmentType = NGLAPI.CostAdjType.Accessorial
                            '            }
                            '            ' Add the new Rate to q.rates
                            '            updatedRates.Add(newRate)
                            '        Else
                            '            'OVERRIDE the numeric value to whatever we hardcode
                            '            ' If the rateCode from oAcc is in q.rates, update the totalRate
                            '            Dim rateToUpdate = q.rates.First(Function(rate) rate.rateCodeValue = oAcc.EdiCode)
                            '            rateToUpdate.totalRate = oAcc.NumericValue
                            '        End If
                            '    Next
                            '    q.rates = updatedRates.ToArray()
                            'End If
                            '-------------------
                            If Not q Is Nothing AndAlso Not q.rates Is Nothing AndAlso q.rates.Count() > 0 Then
                                dLIneHaul = q.totalFreightCharge
                                iBidAdjustmentCount = q.rates.Count()
                                For Each a As Map.Rate In q.rates

                                    If a.isOptional = True Then
                                        Logger.Warning("OPTIONAL [NOT INSERTED] tblBidCostAdj: {CarrierName} {SCAC} with FreightClass {FreightClass}, Weight: {Weight}, AdjustmentType: {AdjustmentType}, DescCode: {RateCode}, Description: {Description}, RateCodeValue: {RateCodeValue}, UnitOfMeasure: {UnitOfMeasure} ", sCarrierName, sSCAC, sFreightClass, a.quantity, a.adjustmentType, a.rateCode, a.description, a.rateCodeValue, a.unitOfMeasure)
                                        oResponse.AddMessage(Map.APIMessage.MessageEnum.E_OptionalCharge, a.rateCodeValue & " " & a.totalRate.ToString("c"), "NA", a.description)
                                    Else

                                        decBidCustAdjustments = decBidCustAdjustments + a.totalRate
                                        Adjs.Add(New LTS.tblBidCostAdj With {.BidCostAdjFreightClass = sFreightClass,
                                                                                   .BidCostAdjWeight = a.quantity,
                                                                                   .BidCostAdjAmount = a.totalRate,
                                                                                   .BidCostAdjRate = a.unitRate,
                                                                                   .BidCostAdjDescCode = a.rateCodeValue,
                                                                                   .BidCostAdjTypeControl = a.adjustmentType,
                                                                                   .BidCostAdjDesc = a.description,
                                                                                   .BidCostAdjUOM = a.unitOfMeasure,
                                                                                   .BidCostAdjModDate = dtNow,
                                                                                   .BidCostAdjModUser = Me.Parameters.UserName})
                                        Logger.Information("Insert tblBidCostAdj: {CarrierName} {SCAC} with FreightClass {FreightClass}, Weight: {Weight}, AdjustmentType: {AdjustmentType}, TotalRate: {TotalRate}, UnitRate: {UnitRate}, DescCode: {RateCode}, Description: {Description}, RateCodeValue: {RateCodeValue}, UnitOfMeasure: {UnitOfMeasure} ", sCarrierName, sSCAC, sFreightClass, a.quantity, a.adjustmentType, a.totalRate, a.unitRate, a.rateCode, a.description, a.rateCodeValue, a.unitOfMeasure)
                                    End If

                                Next

                            End If
                            Dim strErrs = "" 
                           
                            Dim blnInterline As Boolean = False
                            Dim bArchived = 0
                            'TODO: add logic for minimum transit days and max and Min delivery date
                            Dim dtLoad As Date = If(oLoadTender.LTBookDateLoad, Date.Now.AddDays(1))
                            Dim dtBidDeliveryDate As Date? = If(oLoadTender.LTBookDateRequired, Date.Now.AddDays(5))
                            Dim dblTransitDays As Integer = 5
                            If Not q.transit Is Nothing AndAlso q.transit.maximumTransitDays > 0 Then
                                dblTransitDays = q.transit.maximumTransitDays
                            End If
                            dtBidDeliveryDate = dtLoad.AddDays(dblTransitDays)
                            Dim dtBidQuoteDate As Date? = dtNow
                            Dim dtBidExpirationDate As Date = dtNow
                            Dim decBidCustLineHaul As Decimal = dLIneHaul * (1 + dblCarrierCostUpcharge)
                            Dim decBidCustTotalCost As Decimal = q.totalCharge * (1 + dblCarrierCostUpcharge)
                            'Modified by RHR for v-8.5.4.5 on 02/02/2024 added left function to bid comments .BidComments = Left(q.comments, 254), 

                            ''Modified by AYman for v-10 on 16/10/2025. No need to UPLFT for API bids as the line haul and total cost already include the uplift
                            'Adjs.Add(New LTS.tblBidCostAdj With {.BidCostAdjFreightClass = sFreightClass,
                            '                                                       .BidCostAdjWeight = 1,
                            '                                                       .BidCostAdjAmount = decBidCustLineHaul,
                            '                                                       .BidCostAdjRate = 1,
                            '                                                       .BidCostAdjDescCode = "UPLF",
                            '                                                       .BidCostAdjDesc = "Customer Line Haul Charges",
                            '                                                       .BidCostAdjTypeControl = NGLLookupDataProvider.CostAdjType.CustomerLineHaul,
                            '                                                       .BidCostAdjModDate = dtNow,
                            '                                                       .BidCostAdjModUser = Me.Parameters.UserName,
                            '                                                       .BidCostAdjUOM = "Flat"})

                            Logger.Information("{CarrierName} - Adding UPLFT - decBidCustLineHaul: {decBidCustLineHaul}, decBidCustTotalCost: {decBidCustTotalCost}", sCarrierName, decBidCustLineHaul, decBidCustTotalCost)

                            Dim oBid As New LTS.tblBid With {.BidLoadTenderControl = LoadTenderControl,
                                                                .BidBidTypeControl = BidTypeEnum.CHRAPI,
                                                                .BidCarrierControl = oCarrierData.CarrierControl,
                                                                .BidCarrierNumber = If(oCarrierData.CarrierNumber, 0),
                                                                .BidCarrierName = Left(oCarrierData.CarrierName, 40),
                                                                .BidCarrierSCAC = Left(sSCAC, 4),
                                                                .BidSHID = SHID,
                                                                .BidTotalCost = q.totalCharge,
                                                                .BidLineHaul = q.totalFreightCharge,
                                                                .BidFuelTotal = q.totalFuelCharge,
                                                                .BidFuelVariable = fuelVar,
                                                                .BidFuelUOM = "Flat Rate",
                                                                .BidOrigState = Left(origST, 2),
                                                                .BidDestState = Left(destST, 2),
                                                                .BidPosted = dtNow,
                                                                .BidStatusCode = bStatusCode,
                                                                .BidArchived = bArchived,
                                                                .BidMode = q.transportModeType,
                                                                .BidErrorCount = 0,
                                                                .BidErrors = Left(strErrs, 3999),
                                                                .BidWarnings = Left(oResponse.responseString, 3999), 'Modified by RHR v-8.5.4.005 on 02/21/2024 Logic changed to store inbound API Rest data in the BidWarnings field
                                                                .BidInfos = Left(oResponse.requestString, 3999), 'Modified by RHR v-8.5.4.005 on 02/21/2024 Logic changed to store outbound API Rest data in the Bidinfos field
                                                                .BidInterLine = blnInterline,
                                                                .BidQuoteNumber = If(String.IsNullOrEmpty(q.sQuoteId), Left(q.quoteId, 100), Left(q.sQuoteId, 100)),
                                                                .BidTransitTime = dblTransitDays,
                                                                .BidDeliveryDate = dtBidDeliveryDate,
                                                                .BidQuoteDate = dtBidQuoteDate,
                                                                .BidTotalWeight = oLoadTender.LTBookTotalWgt,
                                                                .BidDetailTotal = 0,
                                                                .BidDetailTransitTime = 0,
                                                                .BidAdjustments = q.totalCharge - dLIneHaul, 'difference between line haul and total cost 
                                                                .BidAdjustmentCount = iBidAdjustmentCount,
                                                                .BidVendor = Left(sSCAC, 20),
                                                                .BidContractID = If(String.IsNullOrEmpty(q.sQuoteId), Left(q.quoteId, 100), Left(q.sQuoteId, 100)),
                                                                .BidServiceType = Left(q.transportModeType, 50),
                                                                .BidTotalPlts = oLoadTender.LTBookTotalPL,
                                                                .BidTotalQty = oLoadTender.LTBookTotalCases,
                                                                .BidComments = Left(q.comments, 254),
                                                                .BidExpires = dtBidExpirationDate,
                                                                .BidCustLineHaul = decBidCustLineHaul,
                                                                .BidCustTotalCost = decBidCustTotalCost,
                                                                .BidModDate = dtNow,
                                                                .BidModUser = Me.Parameters.UserName}
                            ' End Modified by RHR for v-8.5.4.001 on 07/07/2023

                            Logger.Information("Inserting oBid: {@oBid}", oBid)

                            Dim oTable = db.tblBids
                            oTable.InsertOnSubmit(oBid)
                            db.SubmitChanges()
                            Dim bidCtrl = oBid.BidControl

                            Dim oT = db.tblBidCostAdjs
                            For Each adj In Adjs
                                adj.BidCostAdjBidControl = bidCtrl
                                oT.InsertOnSubmit(adj)
                            Next

                            Dim lMessages As List(Of Map.APIMessage) = oResponse.GetMessages().Where(Function(x) x.bLogged = False).ToList()
                            If (Not lMessages Is Nothing AndAlso lMessages.Count > 0) Then
                                'Reset blnInvalidData flag to false because we have already logged the messages
                                blnInvalidData = False
                                Dim oTbl = db.tblBidSvcErrs
                                For Each msg In lMessages
                                    msg.bLogged = True
                                    Dim oBidErr As New LTS.tblBidSvcErr With {
                                        .BidSvcErrBidControl = bidCtrl,
                                        .BidSvcErrErrorMessage = NGLLoadTenderData.getLocalizedString(msg.MessageLocalCode, msg.Message),
                                        .BidSvcErrMessage = msg.Details,
                                        .BidSvcErrFieldName = msg.FieldName,
                                        .BidSvcErrModDate = dtNow,
                                        .BidSvcErrModUser = Me.Parameters.UserName}
                                    oTbl.InsertOnSubmit(oBidErr)
                                Next
                            End If

                            db.SubmitChanges()
                        Next
                    End If

                    If blnInvalidData Or oResponse.postMessagesOnly Then
                        Dim lMessages As List(Of Map.APIMessage) = oResponse.GetMessages().Where(Function(x) x.bLogged = False).ToList()
                        If Not lMessages Is Nothing AndAlso lMessages.Count() > 0 Then
                            Dim oBid As New LTS.tblBid With {.BidLoadTenderControl = LoadTenderControl,
                                                                    .BidBidTypeControl = BidTypeEnum.CHRAPI,
                                                                    .BidCarrierControl = iCarrierControl,
                                                                    .BidCarrierNumber = iCarrierNumber,
                                                                    .BidCarrierName = Left(sCarrierName, 40),
                                                                    .BidCarrierSCAC = Left(sSCAC, 4),
                                                                    .BidSHID = SHID,
                                                                    .BidTotalCost = 0,
                                                                    .BidLineHaul = 0,
                                                                    .BidFuelTotal = 0,
                                                                    .BidFuelVariable = 0,
                                                                    .BidFuelUOM = "NA",
                                                                    .BidOrigState = Left(origST, 2),
                                                                    .BidDestState = Left(destST, 2),
                                                                    .BidPosted = dtNow,
                                                                    .BidStatusCode = bStatusCode,
                                                                    .BidArchived = False,
                                                                    .BidMode = "NA",
                                                                    .BidErrorCount = lMessages.Count(),
                                                                    .BidErrors = Left(oResponse.concateMessages, 3999),
                                                                    .BidWarnings = Left(oResponse.responseString, 3999), 'Modified by RHR v-8.5.4.005 on 02/21/2024 Logic changed to store inbound API Rest data in the BidWarnings field
                                                                    .BidInfos = Left(oResponse.requestString, 3999), 'Modified by RHR v-8.5.4.005 on 02/21/2024 Logic changed to store outbound API Rest data in the Bidinfos field
                                                                    .BidVendor = Left(sSCAC, 20),
                                                                    .BidTotalPlts = oLoadTender.LTBookTotalPL,
                                                                    .BidTotalQty = oLoadTender.LTBookTotalCases,
                                                                    .BidCustLineHaul = 0,
                                                                    .BidCustTotalCost = 0,
                                                                    .BidModDate = dtNow,
                                                                    .BidModUser = Me.Parameters.UserName}

                            Dim oTable = db.tblBids
                            oTable.InsertOnSubmit(oBid)
                            db.SubmitChanges()
                            Dim bidCtrl = oBid.BidControl

                            If (Not lMessages Is Nothing AndAlso lMessages.Count > 0) Then
                                Dim oTbl = db.tblBidSvcErrs
                                For Each msg In lMessages
                                    msg.bLogged = True
                                    'Modified by RHR for v-8.5.4.006 on 04/24/2024 added logic to truncte the message details in the BidSvcErrVendorErrorMessage field to 499 characters
                                    Dim oBidErr As New LTS.tblBidSvcErr With {
                                            .BidSvcErrBidControl = bidCtrl,
                                            .BidSvcErrErrorMessage = NGLLoadTenderData.getLocalizedString(msg.MessageLocalCode, msg.Message),
                                            .BidSvcErrVendorErrorMessage = Left(msg.Details, 499),
                                            .BidSvcErrFieldName = msg.FieldName,
                                            .BidSvcErrModDate = dtNow,
                                            .BidSvcErrModUser = Me.Parameters.UserName}
                                    oTbl.InsertOnSubmit(oBidErr)
                                Next
                            End If

                            db.SubmitChanges()
                        End If

                    End If

                    Return True
                Catch ex As Exception
                    Logger.Error(ex, "Exception in NGLAPIBLL.InsertAPIRateQuoteBids")
                    Dim sParms As New List(Of String)
                    sParms.Add(LoadTenderControl.ToString())
                    sParms.Add("Unexpected Error: " & ex.Message)
                    oResults.AddMessage(DTO.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.E_SaveRateFailure, sParms.ToArray())
                    Return False
                End Try
                Return False
            End Using
        End Using
    End Function


    Public Function InsertAPIRateQuoteBidError(ByVal sCarrierName As String,
                                               ByVal sSCAC As String,
                                               ByVal sVendor As String,
                                               ByVal oLoadTender As LTS.tblLoadTender,
                                               ByRef oResults As DTO.WCFResults,
                                               ByVal key As Map.APIMessage.MessageEnum,
                                               ByVal sDetails As String,
                                               ByVal sDefault As String,
                                               ByVal sFieldName As String) As Boolean

        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Dim aPIMessage = New Map.APIMessage()
            aPIMessage.MessageLocalCode = key.ToString()
            aPIMessage.Message = Map.APIMessage.getMessageNotLocalized(key, key.ToString())
            aPIMessage.Details = sDetails
            aPIMessage.FieldName = sFieldName
            aPIMessage.bLogged = False

            Dim sRetMSg As String = ""

            Dim SHID As String = oLoadTender.LTBookSHID
            Dim origST As String = oLoadTender.LTBookOrigState
            Dim destST As String = oLoadTender.LTBookDestState
            Dim strMsg As String = ""
            Dim bStatusCode As BSCEnum = BSCEnum.Active

            Dim iCarrierControl As Integer = 0
            Dim iCarrierNumber As Integer = 0

            Try

                Dim oBid As New LTS.tblBid With {.BidLoadTenderControl = oLoadTender.LoadTenderControl,
                                                            .BidBidTypeControl = BidTypeEnum.CHRAPI,
                                                            .BidCarrierControl = iCarrierControl,
                                                            .BidCarrierNumber = iCarrierNumber,
                                                            .BidCarrierName = Left(sCarrierName, 40),
                                                            .BidCarrierSCAC = Left(sSCAC, 4),
                                                            .BidSHID = SHID,
                                                            .BidTotalCost = 0,
                                                            .BidLineHaul = 0,
                                                            .BidFuelTotal = 0,
                                                            .BidFuelVariable = 0,
                                                            .BidFuelUOM = "NA",
                                                            .BidOrigState = Left(origST, 2),
                                                            .BidDestState = Left(destST, 2),
                                                            .BidPosted = Date.Now,
                                                            .BidStatusCode = bStatusCode,
                                                            .BidArchived = False,
                                                            .BidMode = "NA",
                                                            .BidErrorCount = 1,
                                                            .BidErrors = "",
                                                            .BidWarnings = "",
                                                            .BidInfos = "",
                                                            .BidVendor = Left(sVendor, 20),
                                                            .BidTotalPlts = oLoadTender.LTBookTotalPL,
                                                            .BidTotalQty = oLoadTender.LTBookTotalCases,
                                                            .BidCustLineHaul = 0,
                                                            .BidCustTotalCost = 0,
                                                            .BidModDate = Date.Now,
                                                            .BidModUser = Me.Parameters.UserName}

                Dim oTable = db.tblBids
                oTable.InsertOnSubmit(oBid)
                db.SubmitChanges()
                Dim bidCtrl = oBid.BidControl

                Dim oTbl = db.tblBidSvcErrs
                Dim oBidErr As New LTS.tblBidSvcErr With {
                                        .BidSvcErrBidControl = bidCtrl,
                                        .BidSvcErrErrorMessage = NGLLoadTenderData.getLocalizedString(aPIMessage.MessageLocalCode, aPIMessage.Message),
                                        .BidSvcErrVendorErrorMessage = aPIMessage.Details,
                                        .BidSvcErrFieldName = aPIMessage.FieldName,
                                        .BidSvcErrModDate = Date.Now,
                                        .BidSvcErrModUser = Me.Parameters.UserName}
                oTbl.InsertOnSubmit(oBidErr)

                db.SubmitChanges()
                Return True
            Catch ex As Exception
                Dim sParms As New List(Of String)
                sParms.Add(oLoadTender.LoadTenderControl.ToString())
                sParms.Add("Unexpected Error: " & ex.Message)
                oResults.AddMessage(DTO.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.E_SaveRateFailure, sParms.ToArray())
                Return False
            End Try
            Return False
        End Using
    End Function


    Public Function CopyBookDataToAPIData(ByRef oBooks() As LTS.vBookRevenue, ByRef oPickup As LTS.vBookRevenue, ByVal iDefaultFreightClass As Integer) As Map.RateRequest
        Dim oRet As New Map.RateRequest()
        If ((oBooks Is Nothing) OrElse oBooks.Count() < 1 OrElse (oBooks(0).BookControl = 0)) Then Return Nothing
        oPickup = oBooks.OrderBy(Function(x) x.BookPickupStopNumber).ThenBy(Function(y) y.BookStopNo).FirstOrDefault()
        Dim oResponse As New Map.QuoteResponse()

        Dim origCompControl As Integer = 0
        Dim destCompControl As Integer = 0
        'set default values for load date and required date
        ' Modified by RHR for v-8.5.4.006 on 05/24/2024 moved setShipDate logic to use book data logic to set the ship date 
        Dim dtShip As Nullable(Of Date) = Nothing ' Removed this does not work here Date.Now.AddDays(2) 'CHR rates need 2 days 
        Dim dtRequired As Nullable(Of Date) = Nothing ' Removed ths does not work here dtShip.AddDays(4)
        Dim oBookPkgDAL = New NGLBookPackage(Me.Parameters)
        Dim oItems As New List(Of LTS.vBookPackage)
        Dim oBookAccDAL = New NGLBookAccessorial(Me.Parameters)
        Dim oBookAccessorial As New List(Of LTS.vBookAccessorial)
        Dim sAccessorial As New List(Of String)
        Dim oAccs As LTS.vBookAccessorial()

        Dim sCNS As String = If(String.IsNullOrWhiteSpace(oPickup.BookConsPrefix), oPickup.BookProNumber, oPickup.BookConsPrefix)
        Dim sSHID As String = If(String.IsNullOrWhiteSpace(oPickup.BookSHID), sCNS, oPickup.BookSHID)
        Dim sDel As String = If(String.IsNullOrWhiteSpace(oPickup.BookLoads(0).BookLoadPONumber), sCNS, oPickup.BookLoads(0).BookLoadPONumber)
        Dim oLaneData As DTO.Lane = NGLLaneData.GetLaneFiltered(oPickup.BookODControl)
        Dim lOriginRefNbrs As New List(Of Map.ReferenceNumber)
        Dim lOriginItems As New List(Of Map.RateRequestItem)
        Dim iTmp As Integer = 0
        If Not oPickup.BookLoads Is Nothing AndAlso oPickup.BookLoads.Any() AndAlso Not oPickup.BookLoads(0).BookItems Is Nothing AndAlso oPickup.BookLoads(0).BookItems.Any() Then
            lOriginItems = (From i In oPickup.BookLoads(0).BookItems Select New Map.RateRequestItem() With {
                .Description = If(String.IsNullOrWhiteSpace(i.BookItemDescription), "misc products", i.BookItemDescription),
                .FreightClass = If(Integer.TryParse(i.BookItemFAKClass, iTmp), iTmp, iDefaultFreightClass),
                .Weight = If(CInt(i.BookItemWeight) > 0, CInt(i.BookItemWeight), 100),
                .WeightUnit = oLaneData.LaneWeightUnit,
                .LengthUnit = oLaneData.LaneLengthUnit,
                .Length = i.BookItemQtyLength,
                .Width = i.BookItemQtyWidth,
                .Height = i.BookItemQtyHeight,
                .PalletCount = Math.Ceiling(i.BookItemPallets),
                .Pieces = i.BookItemQtyOrdered,
                .PackageType = i.BookItemPalletTypeID.ToString(),
                .ItemNumber = i.BookItemItemNumber,
                .ItemCost = If(i.BookItemItemCost.HasValue, i.BookItemItemCost.Value, 10.0),
                .Stackable = i.BookItemStackable,
                .ItemTemperature = Map.RateRequestItem.GetTemperatureCode(i.BookItemCommCode)
            }).ToList()
        End If

        'Get the package details from the first stop
        'This information is managed on the Rate IT Package or Pallets Details Grid
        Dim filters = New Models.AllFilters With {.ParentControl = oPickup.BookControl} 'we must clear the filter to be sure we have good data
        Dim oPkgs As LTS.vBookPackage() = oBookPkgDAL.GetBookPackages(filters, 500, False)
        Dim iCompControl As Integer = 0
        With oRet
            .customerCode = oPickup.BookOrigName 'default, each API may have specific mapping requirements CHR comes from Company SSOA account data.
            .lStops = New List(Of Map.AddressBook)
            .lItems = lOriginItems
            ' Modified by RHR fo rv-8.5.4.006 on 05/02/2024 moved setShipDate logic to use book data logic to set the ship date to the earliest load date


            If lOriginItems Is Nothing AndAlso Not oItems Is Nothing Then
                lOriginItems = (From i In oItems Select New Map.RateRequestItem() With {
                  .Description = If(String.IsNullOrWhiteSpace(i.BookPkgDescription), "misc products", i.BookPkgDescription),
                  .FreightClass = If(Integer.TryParse(i.BookPkgFAKClass, iTmp), iTmp, iDefaultFreightClass),
                  .Weight = If(CInt(i.BookPkgWeight) > 0, CInt(i.BookPkgWeight), 100),
                  .WeightUnit = "Pounds",
                  .Length = i.BookPkgLength,
                  .Width = i.BookPkgWidth,
                  .Height = i.BookPkgHeight,
                  .PalletCount = i.BookPkgCount,
                  .Pieces = i.BookPkgCount,
                  .PackageType = If(String.IsNullOrWhiteSpace(i.PackageType), "PLT", i.PackageType),
                  .ItemNumber = "goods",
                  .ItemCost = 1000,
                  .Stackable = i.BookPkgStackable,
                  .ItemTemperature = Map.RateRequestItem.GetTemperatureCode("D")
              }).ToList()
                .lItems = lOriginItems
            End If

            'default, each API may have special mapping requirements for Reference numbers
            lOriginRefNbrs.Add(New Map.ReferenceNumber(Map.ReferenceNumber.RefNumber.SHID, sSHID))
            lOriginRefNbrs.Add(New Map.ReferenceNumber(Map.ReferenceNumber.RefNumber.CNS, sCNS))
            lOriginRefNbrs.Add(New Map.ReferenceNumber(Map.ReferenceNumber.RefNumber.PU, sCNS))
            lOriginRefNbrs.Add(New Map.ReferenceNumber(Map.ReferenceNumber.RefNumber.DEL, sDel))

            If (Not String.IsNullOrWhiteSpace(oPickup.BookCarrOrderNumber)) Then
                lOriginRefNbrs.Add(New Map.ReferenceNumber(Map.ReferenceNumber.RefNumber.ORD, oPickup.BookCarrOrderNumber))
            End If
            If (Not String.IsNullOrWhiteSpace(oPickup.BookProNumber)) Then
                lOriginRefNbrs.Add(New Map.ReferenceNumber(Map.ReferenceNumber.RefNumber.TMSPRO, oPickup.BookProNumber))
            End If
            .lReferencesNumbers = lOriginRefNbrs
            .oOrigin = New Map.AddressBook() With
                {
                    .Name = oPickup.BookOrigName,
                    .Address1 = oPickup.BookOrigAddress1,
                    .City = oPickup.BookOrigCity,
                    .State = oPickup.BookOrigState,
                    .Country = oPickup.BookOrigCountry,
                    .Zip = oPickup.BookOrigZip,
                    .LocationCode = oPickup.BookOrigName,
                    .lReferencesNumbers = lOriginRefNbrs,
                    .lItems = lOriginItems
                }
            .declaredValue = lOriginItems.Sum(Function(x) x.ItemCost)
        End With

        For Each book As LTS.vBookRevenue In oBooks

            If book Is Nothing OrElse book.BookControl = 0 OrElse book.BookLoads Is Nothing OrElse book.BookLoads.Count() = 0 Then
                Continue For
            End If

            oRet.totalCases = oRet.totalCases + book.BookTotalCases
            oRet.totalWeight = oRet.totalWeight + book.BookTotalWgt
            oRet.totalPL = oRet.totalPL + book.BookTotalPL
            oRet.totalCube = oRet.totalCube + book.BookTotalCube

            iCompControl = book.BookCustCompControl
            'get the earliest load date from the booking records
            ' Modified by RHR for v-8.5.4.006 on 05/02/2024 
            If book.BookDateLoad.HasValue Then
                If dtShip.HasValue Then
                    If book.BookDateLoad.Value < dtShip.Value Then dtShip = book.BookDateLoad.Value
                Else
                    'the first time we always use the BookDateLoad
                    dtShip = book.BookDateLoad.Value
                End If
            Else
                dtShip = Date.Now.AddDays(2) 'API rates need 2 days
            End If
            ' Modified by RHR for v-8.5.4.006 on 05/02/2024 added logic to set the ship date to the default or the earliest load date on the truck
            oRet.setShipDate(dtShip.Value.ToShortDateString)
            'get the last delivery date
            ' Modified by RHR for v-8.5.4.006 on 05/02/2024 
            If book.BookDateRequired.HasValue Then
                If dtRequired.HasValue Then
                    If book.BookDateRequired.Value > dtRequired.Value Then dtRequired = book.BookDateRequired.Value
                Else
                    'the first time we always use the BookDateRequired
                    dtRequired = book.BookDateRequired.Value
                End If
            Else
                dtRequired = Date.Now.AddDays(5) 'set to 5 days from now
            End If
            Dim ct As Integer = 0
            filters = New Models.AllFilters With {.ParentControl = book.BookControl} 'we must clear the filter to be sure we have good data
            Logger.Information("NGLAPIBLL.CopyBookDataToAPIData - GetBookAccessorials(filters,ct): {@filters} {@ct}", filters, ct)
            oAccs = oBookAccDAL.GetBookAccessorials(filters, ct)
            Dim oTMSFees() As LTS.tblAccessorial = NGLtblAccessorialData.GetAllAccessorials()
            
            Dim lAPIFees As New List(Of Map.Fees)
            If Not oAccs Is Nothing AndAlso oAccs.Count() > 0 Then
                For Each fee As LTS.vBookAccessorial In oAccs
                    Logger.Information("NGLAPIBLL.CopyBookDataToAPIData - fee: {@fee}", fee)
                    Dim foundCode = oTMSFees.Where(Function(x) x.AccessorialCode = fee.AccessorialCode).Select(Function(y) y.AccessorialEDICode).FirstOrDefault()
                    Logger.Information("NGLAPIBLL.CopyBookDataToAPIData - foundCode: {foundCode}", foundCode)
                    lAPIFees.Add(
                        New Map.Fees With {
                                    .BookAcssControl = fee.BookAcssControl,
                                    .BookAcssNACControl = fee.BookAcssNACControl,
                                    .BookAcssValue = fee.BookAcssValue,
                                    .NACCode = fee.NACCode,
                                    .NACName = fee.NACName,
                                    .AccessorialCode = fee.AccessorialCode,
                                    .AccessorialName = fee.AccessorialName,
                                    .AccessorialEDICode = foundCode
                                    })
                Next
                oRet.oFees = lAPIFees
            End If
            Dim lStopItems As List(Of Map.RateRequestItem) = lOriginItems
            If Not book.BookLoads Is Nothing AndAlso book.BookLoads.Count() > 0 AndAlso Not book.BookLoads(0).BookItems Is Nothing AndAlso book.BookLoads(0).BookItems.Count() > 0 Then
                Logger.Information("NGLAPIBLL.CopyBookDataToAPIData - book.BookLoads(0).BookItems: {@book.BookLoads(0).BookItems}", book.BookLoads(0).BookItems)
                lStopItems = (From i In book.BookLoads(0).BookItems Select New Map.RateRequestItem() With {
                    .Description = If(String.IsNullOrWhiteSpace(i.BookItemDescription), "misc products", i.BookItemDescription),
                    .FreightClass = If(Integer.TryParse(i.BookItemFAKClass, iTmp), iTmp, iDefaultFreightClass),
                    .Weight = If(CInt(i.BookItemWeight) > 0, CInt(i.BookItemWeight), 100),
                    .WeightUnit = oLaneData.LaneWeightUnit,
                    .LengthUnit = oLaneData.LaneLengthUnit,
                    .Length = i.BookItemQtyLength,
                    .Width = i.BookItemQtyWidth,
                    .Height = i.BookItemQtyHeight,
                    .PalletCount = Math.Ceiling(i.BookItemPallets),
                    .Pieces = i.BookItemQtyOrdered,
                    .PackageType = i.BookItemPalletTypeID.ToString(),
                    .ItemNumber = i.BookItemItemNumber,
                    .ItemCost = If(i.BookItemItemCost.HasValue, i.BookItemItemCost.Value, 10.0),
                    .Stackable = i.BookItemStackable,
                    .ItemTemperature = Map.RateRequestItem.GetTemperatureCode(i.BookItemCommCode)
                    }).ToList()
            Else
                Logger.Information("NGLAPIBLL.CopyBookDataToAPIData - book.BookLoads(0).BookItems is null or empty")
                oPkgs = oBookPkgDAL.GetBookPackages(filters, ct, False)
                If Not oPkgs Is Nothing AndAlso oPkgs.Count() > 0 Then
                    Logger.Information("NGLAPIBLL.CopyBookDataToAPIData - oPkgs: {@oPkgs}", oPkgs)
                    lStopItems = (From i In oPkgs Select New Map.RateRequestItem() With {
                                .Description = If(String.IsNullOrWhiteSpace(i.BookPkgDescription), "misc products", i.BookPkgDescription),
                                .FreightClass = If(Integer.TryParse(i.BookPkgFAKClass, iTmp), iTmp, iDefaultFreightClass),
                                .Weight = If(CInt(i.BookPkgWeight) > 0, CInt(i.BookPkgWeight), 100),
                                .WeightUnit = "Pounds",
                                .Length = i.BookPkgLength,
                                .Width = i.BookPkgWidth,
                                .Height = i.BookPkgHeight,
                                .PalletCount = i.BookPkgCount,
                                .Pieces = i.BookPkgCount,
                                .PackageType = If(String.IsNullOrWhiteSpace(i.PackageType), "PLT", i.PackageType),
                                .ItemNumber = "goods",
                                .ItemCost = 1000,
                                .Stackable = i.BookPkgStackable,
                                .ItemTemperature = Map.RateRequestItem.GetTemperatureCode("D")
                                }).ToList()
                    Logger.Warning("NGLAPIBLL.CopyBookDataToAPIData [temp code set to d???] - lStopItems: {@lStopItems}", lStopItems)
                End If
            End If
            'Removed by RHR for v-8.5.4.006 on 05/24 /2024 we assign this value earlier
            'If dtRequired = Date.MinValue Then dtRequired = Date.Now.AddDays(5) 'set to 5 days from now

            If book.BookOrigName <> oPickup.BookOrigName AndAlso Not oRet.lStops.Any(Function(x) x.Name = book.BookOrigName) Then
                'we have a multi-pick and need to add the stop information.
            End If

            Dim oStop = New Map.AddressBook() With {
            .Name = book.BookDestName,
            .Address1 = book.BookDestAddress1,
            .City = book.BookDestCity,
            .State = book.BookDestState,
            .Country = book.BookDestCountry,
            .Zip = book.BookDestZip,
            .LocationCode = book.BookDestName,
            .lReferencesNumbers = lOriginRefNbrs,
            .lItems = lStopItems
            }

            oRet.lStops.Add(oStop)
        Next


        Return oRet
    End Function




    Public Function DispatchToAPI(ByRef d As Models.Dispatch,
                                   ByVal oLEConfig As LTS.tblSSOALEConfig,
                                   Optional ByRef strMsg As String = "") As DTO.WCFResults

        Dim oRet As New DTO.WCFResults()
        oRet.Success = False
        Dim bUseTLS12 As Boolean = True
        bUseTLS12 = If(NGLLoadTenderData.GetParValue("GlobalTurnOnTLs12ForAPIs", 0) = 0, False, True)

        'Get the accessorial fees
        Dim dTotalOtherCosts As Decimal = NGLDATBLL.ProcessAPIOtherCosts(d.BookControl, d.BidControl, d.Accessorials, d.BookFees)
        'Get the bill to address
        d.BillTo = NGLBookData.GetBillToForBook(d.BookControl)
        'map the dispatch to the API Dispatch
        Dim oMapDispatch = d.MapNGLAPIDispatch
        Dim lCompConfigs As List(Of LTS.tblSSOAConfig) = NGLtblSingleSignOnAccountData.GetSSOAConfigs(oLEConfig.SSOALEControl)
        Dim sCarrierNumber As String = ""
        If Not lCompConfigs Is Nothing AndAlso lCompConfigs.Count() > 0 Then
            If (lCompConfigs.Any(Function(x) x.SSOACName = "CarrierNumber")) Then sCarrierNumber = lCompConfigs.Where(Function(x) x.SSOACName = "CarrierNumber").Select(Function(v) v.SSOACValue).FirstOrDefault()
        End If
        Dim oMapLEConfig As Map.SSOALEConfig = New Map.SSOALEConfig(oLEConfig.SSOALEClientID, oLEConfig.SSOALEClientSecret, oLEConfig.SSOALELoginURL, oLEConfig.SSOALEAuthCode, oLEConfig.SSOALEDataURL)
        Dim sRetMsg As String = ""
        Dim lMapSSOAConfigs As List(Of Map.SSOAConfig) = (From e In lCompConfigs Select Map.SSOAConfig.selectMapData(e, sRetMsg)).ToList()
        Select Case oLEConfig.SSOALESSOAControl
            Case 10 'CHR API
                Dim oAPI As New NGL.FM.CHRAPI.CHRAPI(bUseTLS12)
                Dim oAPIResponses As Map.OrderResponse = oAPI.ProcessOrderRequest(oMapDispatch, oMapLEConfig, lMapSSOAConfigs)

                'If Not oAPIResponses Is Nothing AndAlso oAPIResponses.Count() > 0 Then
                '    For Each oAPIResp As Map.QuoteResponse In oAPIResponses
                '        If oAPIResp.success Then
                '            ' we now insert a quote with error messages even if one is not available 
                '            ' using the postMessagesOnly flag with a zero cost.  This logic will help
                '            ' users track issues with API rating
                '            InsertAPIRateQuoteBids(LoadTenderControl, oAPIResp, sCarrierNumber, oResults)
                '        End If
                '    Next
                'End If

                    'Imports CHR = NGL.FM.CHRAPI
                    'Imports UPS = NGL.FM.UPSAPI
                    'Imports JTS = NGL.FM.JTSAPI
                    'Imports Map = NGL.API.Mapping
            Case 11  'UPS API
            Case 12  'YRC API
            Case 13  'JTS API
            Case 14  'FedX API
            Case 15  'Engage Lane API
            Case 16  'HMBay API
                Dim oAPI As New DTMS.API.HMBay.HMBayAPI()
                Dim lSpecialFees As New List(Of Map.RateRequest.SpecialRequirement)
                Dim oAPIResponses As New List(Of Map.QuoteResponse)
                'If BookControl <> 0 Then
                '    oAPIResponses = oAPI.GetBid(oRateRequest, LoadTenderControl, oMapLEConfig, lMapSSOAConfigs, strMsg, iDefaultFreightClass)
                'Else
                '    oAPIResponses = oAPI.ProcessRateRequest(oMapRateRequestOrder, LoadTenderControl, oMapLEConfig, lMapSSOAConfigs, lSpecialFees, strMsg, iDefaultFreightClass)
                'End If
                'If Not oAPIResponses Is Nothing AndAlso oAPIResponses.Count() > 0 Then
                '    For Each oAPIResp As Map.QuoteResponse In oAPIResponses
                '        If oAPIResp.success Then
                '            ' we now insert a quote with error messages even if one is not available 
                '            ' using the postMessagesOnly flag with a zero cost.  This logic will help
                '            ' users track issues with API rating
                '            InsertAPIRateQuoteBids(LoadTenderControl, oAPIResp, sCarrierNumber, oResults)
                '        End If
                '    Next
                'End If
            Case 17  'FFE API
                Dim oAPI As New DTMS.API.FFE.FFEAPI()
                Dim lSpecialFees As New List(Of Map.RateRequest.SpecialRequirement)
                Dim oAPIResponses As New List(Of Map.QuoteResponse)
                'If BookControl <> 0 Then
                '    oAPIResponses = oAPI.GetBid(oRateRequest, LoadTenderControl, oMapLEConfig, lMapSSOAConfigs, strMsg, iDefaultFreightClass)
                'Else
                '    oAPIResponses = oAPI.ProcessRateRequest(oMapRateRequestOrder, LoadTenderControl, oMapLEConfig, lMapSSOAConfigs, lSpecialFees, strMsg, iDefaultFreightClass)
                'End If
                'If Not oAPIResponses Is Nothing AndAlso oAPIResponses.Count() > 0 Then
                '    For Each oAPIResp As Map.QuoteResponse In oAPIResponses
                '        If oAPIResp.success Then
                '            ' we now insert a quote with error messages even if one is not available 
                '            ' using the postMessagesOnly flag with a zero cost.  This logic will help
                '            ' users track issues with API rating
                '            InsertAPIRateQuoteBids(LoadTenderControl, oAPIResp, sCarrierNumber, oResults)
                '        End If
                '    Next
                'End If
            Case 18  'EVANSTS API
            Case 19  'FROZENLOG API
            Case 20  'HUDSON API
            Case 21  'JBPARTNERS API
            Case 22  'LANTER API
            Case 23  'TQL API
            Case 24  'GTZ API
                Dim oAPI As New DTMS.API.GTZ.GTZAPI()
                Dim lSpecialFees As New List(Of Map.RateRequest.SpecialRequirement)
                Dim oAPIResponses As New List(Of Map.QuoteResponse)
                'If BookControl <> 0 Then
                '    oAPIResponses = oAPI.GetBid(oRateRequest, LoadTenderControl, oMapLEConfig, lMapSSOAConfigs, strMsg, iDefaultFreightClass)
                'Else
                '    oAPIResponses = oAPI.ProcessRateRequest(oMapRateRequestOrder, LoadTenderControl, oMapLEConfig, lMapSSOAConfigs, lSpecialFees, strMsg, iDefaultFreightClass)
                'End If
                'If Not oAPIResponses Is Nothing AndAlso oAPIResponses.Count() > 0 Then
                '    For Each oAPIResp As Map.QuoteResponse In oAPIResponses
                '        If oAPIResp.success Then
                '            ' we now insert a quote with error messages even if one is not available 
                '            ' using the postMessagesOnly flag with a zero cost.  This logic will help
                '            ' users track issues with API rating
                '            InsertAPIRateQuoteBids(LoadTenderControl, oAPIResp, sCarrierNumber, oResults)
                '        End If
                '    Next
                'End If
            Case Else
                'the two lines of code need to be replace they are just to get past compile errors


        End Select



        If oRet.Success = False Then Return oRet
        Dim oP44Dispatch = New NGL.FM.P44.Dispatch()
        ' oRet = CopyDALDispatchToP44Dispatch(oP44Dispatch, d, strMsg)
        If oRet.Success = False Then Return oRet
        'dispatch the load to P44
        Dim oP44Dispatching = New NGL.FM.P44.P44Dispatch()
        Dim strSI As String = ""
        'Dim oResults As NGL.FM.P44.Message = oP44Dispatching.Dispatch(P44WebServiceUrl, P44WebServiceLogin, P44WebServicePassword, P44AccountGroup, oP44Dispatch, strSI)
        'If Not String.IsNullOrWhiteSpace(oResults.Diagnostic) AndAlso oResults.Diagnostic <> "Success!" Then
        '    'we have a problem so log the issue and return false
        '    oRet.Success = False
        '    oRet.AddMessage(DTO.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.E_UnExpected_Error, {oResults.message})
        '    Return oRet
        'Else
        '    'convert the P44 Dispatch to Model Dispatch
        '    oRet = CopyP44DispatchToDALDispatch(d, oP44Dispatch, strMsg)
        '    If oRet.Success = False Then Return oRet
        '    oRet.Data = d
        '    If Not String.IsNullOrWhiteSpace(oResults.message) Then
        '        Select Case oResults.Severity
        '            Case NGL.FM.P44.SeverityEnum.ERROR
        '                oRet.AddMessage(DTO.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.E_UnExpected_Error, {oResults.message})
        '            Case NGL.FM.P44.SeverityEnum.WARNING
        '                oRet.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.E_UnExpected_Error, {oResults.message})
        '        End Select
        '    End If
        '    'return success.
        '    oRet.Success = True
        'End If

        Return oRet
    End Function



#End Region

End Class
