// added manually
//using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Dynamics.DataEntities; //Resources
//using Microsoft.Identity.Client; //Nuget Package: IConfidentialClientApplication, ConfidentialClientApplicationBuilder, AuthenticationResult
using Microsoft.OData.Client; //DataServiceQueryException, DataServiceClientException
using System;
using System.Linq;
using System.Web; //Manually browsed and added reference to System.Web: HttpUtility
//using Test_OData_ConsoleApp;
//using Newtonsoft.Json.Linq;
using TMS = Ngl.FreightMaster.Integration;

namespace NGL.FM.CMDLine.D365Integration
{
    class Program
    {

        public static string ODataEntityPath = ""; //= ClientConfiguration.Default.UriString + "data";
        public static string sLegalEntity = "usmf";
        static void Main(string[] args)
        {
            try
            {

                sLegalEntity = NGL.FM.CMDLine.D365Integration.Properties.Settings.Default.LegalEntities;
                ClientConfiguration.UriString = NGL.FM.CMDLine.D365Integration.Properties.Settings.Default.D365_URL; // "https://ngl-dev015e64cfd23d55673cdevaos.axcloud.dynamics.com/",
                ClientConfiguration.ActiveDirectoryResource = NGL.FM.CMDLine.D365Integration.Properties.Settings.Default.ActiveDirectoryResource; //"https://ngl-dev015e64cfd23d55673cdevaos.axcloud.dynamics.com",
                ClientConfiguration.ActiveDirectoryTenant = NGL.FM.CMDLine.D365Integration.Properties.Settings.Default.ActiveDirectoryTenant; //"https://sts.windows.net/2518be7e-c933-4905-af64-24ad0157202f",
                ClientConfiguration.ActiveDirectoryClientAppId = NGL.FM.CMDLine.D365Integration.Properties.Settings.Default.ActiveDirectoryClientAppId; //"f065bd6b-2455-4963-a00b-d205c095c461",
                ClientConfiguration.ActiveDirectoryClientAppSecret = NGL.FM.CMDLine.D365Integration.Properties.Settings.Default.ActiveDirectoryClientAppSecret; //"0bF7Q~mPdDfMJfC9FkJf4N.fM5NTUrzVJaz45",
                ODataEntityPath = ClientConfiguration.UriString + "data";
                //string aadTenant = NGL.FM.CMDLine.D365Integration.Properties.Settings.Default.ActiveDirectoryTenant;
                //return;


                Uri oDataUri = new Uri(ODataEntityPath, UriKind.Absolute);
                var context = new Resources(oDataUri);

                context.SendingRequest2 += new EventHandler<SendingRequest2EventArgs>(delegate (object sender, SendingRequest2EventArgs e)
                {
                    var authenticationHeader = OAuthHelper.GetAuthenticationHeader(useWebAppAuthentication: true);
                    e.RequestMessage.SetHeader(OAuthHelper.OAuthHeader, authenticationHeader);
                });

                context.BuildingRequest += (sender, e) =>
                {
                    var uriBuilder = new UriBuilder(e.RequestUri);
                    // Requires a reference to System.Web
                    var paramValues = HttpUtility.ParseQueryString(uriBuilder.Query);
                    paramValues.Add("cross-company", "true");
                    uriBuilder.Query = paramValues.ToString();
                    e.RequestUri = uriBuilder.Uri;
                };

                bool blnProcessPick = ProcessPickListData(context);

                bool blnProcessAP = ProcessAPExportData(context);

                if (System.Diagnostics.Debugger.IsAttached)
                {
                    Console.WriteLine("Tap Enter to Continue");
                    string skey = Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Log(ex.Message + ex.InnerException);
            }

        }


        public static Ngl.Core.Communication.clsLog oLog { get; set; }

        public static void Log(string sMsg)
        {

            System.IO.StreamWriter mioLog = null;
            try
            {
                if (oLog == null)
                {
                    oLog = new Ngl.Core.Communication.clsLog();
                    oLog.Debug = true;
                }

                mioLog = oLog.Open("C:\\Data\\TMSLogs\\TestPickListLog.txt", 7, false);
                System.Diagnostics.Debug.WriteLine(sMsg);
                oLog.Write(sMsg, ref mioLog);
                Console.WriteLine(sMsg);
            }
            catch
            {

            }
            finally
            {
                if (oLog != null && mioLog != null)
                {
                    int intReturn = 0;
                    oLog.closeLog(intReturn, ref mioLog);
                }
            }

        }

        public static int RequiredInteger(string Sval, int iDefault, bool bAllowZero)
        {
            int iTmp = 0;
            int iRet = 0;
            iRet = int.TryParse(Sval, out iTmp) == true ? iTmp : 0;
            return RequiredInteger(iRet, iDefault, bAllowZero);

        }
        public static int RequiredInteger(int iVal, int iDefault, bool bAllowZero)
        {

            int iRet = iVal;
            if (iRet == 0 && bAllowZero == false)
            {
                if (iDefault == 0 && bAllowZero == false)
                {
                    iDefault = 1;
                }
                iRet = iDefault;
            }
            return iRet;
        }
        public static decimal RequiredDecimal(decimal dVal, decimal dDefault, bool bAllowZero)
        {


            if (dVal == 0 && bAllowZero == false)
            {
                if (dDefault == 0 && bAllowZero == false)
                {
                    dDefault = 1;
                }
                dVal = dDefault;
            }
            return dVal;
        }
        public static decimal RequiredDecimal(string sVal, decimal dDefault, bool bAllowZero)
        {
            decimal dTmp = 0;
            decimal dRet = 0;
            dRet = decimal.TryParse(sVal, out dTmp) == true ? dTmp : 0;
            return RequiredDecimal(dRet, dDefault, bAllowZero);

        }

        public static decimal RequiredDecimal(double dVal, decimal dDefault, bool bAllowZero)
        {

            decimal dRet = (decimal)dVal;
            return RequiredDecimal(dRet, dDefault, bAllowZero);
        }

        public static decimal RequiredDecimal(int iVal, decimal dDefault, bool bAllowZero)
        {

            decimal dRet = iVal;
            return RequiredDecimal(dRet, dDefault, bAllowZero);
        }

        public static string RequiredString(string Sval, string sDefault, bool bAllowEmpty)
        {

            string sRet = Sval;

            if (string.IsNullOrWhiteSpace(sRet) && bAllowEmpty == false)
            {
                if (string.IsNullOrWhiteSpace(sDefault) && bAllowEmpty == false)
                {
                    sDefault = "1";
                }
                sRet = sDefault;
            }
            return sRet;
        }

        /// <summary>
        /// Converts a Date String to the correct format needed by D365
        /// dtDefault is set to Now if nothing is provided
        /// Will use dtDefault if Sval is not a valid date unless bAllowEmpty is true 
        /// If bAllowEmpty is true and Sval is not a valid date an empty string is returned
        /// </summary>
        /// <param name="Sval"></param>
        /// <param name="dtDefault"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.5.1.002 on 04/10/2022 added default date format for Dynamics 365
        /// </remarks>
        public static string DTMSDateString(string Sval, DateTime? dtDefault = null, bool bAllowEmpty = false)
        {
            DateTime dtVAl = DateTime.Now;
            if (!dtDefault.HasValue)
            {
                dtDefault = DateTime.Now;
            }
            string sRet = dtDefault.Value.ToString("MM/dd/yyyy HH:mm:ss");

            if (DateTime.TryParse(Sval, out dtVAl))
            {
                sRet = dtVAl.ToString("MM/dd/yyyy HH:mm:ss");
            } else
            {
                if (bAllowEmpty == true)
                {
                    sRet = "";
                }
            }

            return sRet;
        }

        public static bool ProcessPickListData(Resources context)
        {
            bool blnRet = false;
            try
            {
                TMSIntegrationServices.clsPickListObject85[] Headers = new TMSIntegrationServices.clsPickListObject85[1];
                TMSIntegrationServices.clsPickDetailObject85[] Details = new TMSIntegrationServices.clsPickDetailObject85[1];
                TMSIntegrationServices.clsPickListFeeObject85[] Fees = new TMSIntegrationServices.clsPickListFeeObject85[1];
                bool bRet = readTMSPicklistData(ref Headers, ref Details, ref Fees);
                if (bRet)
                {
                    bRet = createPickListRequired(context, sLegalEntity, ref Headers, ref Details, ref Fees);
                    if (bRet)
                    {
                        updatePickListOptionalFields(context, sLegalEntity, ref Headers, ref Details, ref Fees);

                        if (Headers != null && Headers.Count() > 0)
                        {
                            foreach (TMSIntegrationServices.clsPickListObject85 h in Headers)
                            {
                                Log("Confirming Picklist Export for PLControl: " + h.PLControl.ToString());
                                confirmTMSPicklistData(h.PLControl);
                            }
                        }
                    }
                }
                blnRet = true;
            }
            catch (Exception ex)
            {
                Log(string.Format("Process Picklist Data Failure: exception {0}  inner {1}", ex.Message, ex.InnerException));
            }
            return blnRet;
        }

        public static bool ProcessAPExportData(Resources context)
        {
            bool blnRet = false;
            try
            {
                TMSIntegrationServices.clsAPExportObject85[] Headers = new TMSIntegrationServices.clsAPExportObject85[1];
                TMSIntegrationServices.clsAPExportDetailObject85[] Details = new TMSIntegrationServices.clsAPExportDetailObject85[1];
                TMSIntegrationServices.clsAPExportFeeObject85[] Fees = new TMSIntegrationServices.clsAPExportFeeObject85[1];
                bool bRet = readTMSAPExportData(ref Headers, ref Details, ref Fees);
                if (bRet)
                {
                    bRet = createAPExportRequired(context, sLegalEntity, ref Headers, ref Details, ref Fees);
                    if (bRet)
                    {
                        updateAPExportOptionalFields(context, sLegalEntity, ref Headers, ref Details, ref Fees);

                        if (Headers != null && Headers.Count() > 0)
                        {
                            foreach (TMSIntegrationServices.clsAPExportObject85 h in Headers)
                            {
                                Log("Confirming AP Export for APControl: " + h.APControl.ToString());
                                confirmTMSAPExportData(h.APControl);
                            }
                        }
                    }
                }
                blnRet = true;
            }
            catch (Exception ex)
            {
                Log(string.Format("Process AP Data Failure: exception {0}  inner {1}", ex.Message, ex.InnerException));
            }
            return blnRet;
        }

        public static bool confirmTMSPicklistData(long PLControl)
        {
            bool blnRet = false;
            try
            {
                Log("Begin Confirm Picklist Data ");

                TMSIntegrationServices.DTMSERPIntegration oDTMSService = new TMSIntegrationServices.DTMSERPIntegration();
                var RetVal = TMS.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
                string sAuth = NGL.FM.CMDLine.D365Integration.Properties.Settings.Default.DTMSAuthCode; //  "WSPROD";
                oDTMSService.Url = NGL.FM.CMDLine.D365Integration.Properties.Settings.Default.NGL_FM_CMDLine_D365Integration_TMSIntegrationServices_DTMSERPIntegration;

                int intMaxRetry = 1;
                int intRetryMinutes = 30;
                int intMaxRowsReturned = 100;
                bool blnAutoConfirmation = false;
                int intRetVal = 0;
                string sRetMessage = "";

                blnRet = oDTMSService.ConfirmPickListExport70(sAuth, PLControl, ref sRetMessage);
                if (blnRet)
                {
                    Log(string.Format("Confirm Picklist for PL Control {0} is Complete", PLControl));
                } else
                {
                    Log(string.Format("Confirm Picklist for PL Control {0} Failed", PLControl));
                }

            }
            catch (Exception ex)
            {
                Log("Unexpected Confirm Picklist Integration Error! Could not update Picklist export flag:  " + ex.ToString());
            }

            return blnRet;
        }


        public static bool confirmTMSAPExportData(int APControl)
        {
            bool blnRet = false;
            try
            {
                Log("Begin Confirm AP Export Data ");

                TMSIntegrationServices.DTMSERPIntegration oDTMSService = new TMSIntegrationServices.DTMSERPIntegration();
                var RetVal = TMS.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
                string sAuth = NGL.FM.CMDLine.D365Integration.Properties.Settings.Default.DTMSAuthCode; //  "WSPROD";
                oDTMSService.Url = NGL.FM.CMDLine.D365Integration.Properties.Settings.Default.NGL_FM_CMDLine_D365Integration_TMSIntegrationServices_DTMSERPIntegration;

                int intMaxRetry = 1;
                int intRetryMinutes = 30;
                int intMaxRowsReturned = 100;
                bool blnAutoConfirmation = false;
                int intRetVal = 0;
                string sRetMessage = "";

                blnRet = oDTMSService.ConfirmAPExport70(sAuth, APControl, ref sRetMessage);
                if (blnRet)
                {
                    Log(string.Format("Confirm AP Export for AP Control {0} is Complete", APControl));
                }
                else
                {
                    Log(string.Format("Confirm AP Export for AP Control {0} Failed", APControl));
                }

            }
            catch (Exception ex)
            {
                Log("Unexpected Confirm AP Export Integration Error! Could not update AP Export flag:  " + ex.ToString());
            }

            return blnRet;
        }

        public static bool readTMSPicklistData(ref TMSIntegrationServices.clsPickListObject85[] Headers, ref TMSIntegrationServices.clsPickDetailObject85[] Details, ref TMSIntegrationServices.clsPickListFeeObject85[] Fees)
        {
            bool blnRet = false;
            try
            {
                Log("Begin Process Picklist Data ");

                TMSIntegrationServices.DTMSERPIntegration oDTMSService = new TMSIntegrationServices.DTMSERPIntegration();

                var RetVal = TMS.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
                string sAuth = NGL.FM.CMDLine.D365Integration.Properties.Settings.Default.DTMSAuthCode; //  "WSPROD";
                oDTMSService.Url = NGL.FM.CMDLine.D365Integration.Properties.Settings.Default.NGL_FM_CMDLine_D365Integration_TMSIntegrationServices_DTMSERPIntegration;
                int intMaxRetry = 1;
                int intRetryMinutes = 30;
                int intMaxRowsReturned = 100;
                bool blnAutoConfirmation = false;
                int intRetVal = 0;
                string sRetMessage = "";

                string strCriteria = string.Format(" MaxRetry = {0} RetryMinutes = {1} MaxRowsReturned = {2} AutoConfirmation = {3} LegalEntity {4}", intMaxRetry, intRetryMinutes, intMaxRowsReturned, blnAutoConfirmation, sLegalEntity);
                Log("Criteria: " + strCriteria);
                TMSIntegrationServices.clsPickListData85 oPickListData = oDTMSService.GetPickListData85(sAuth, intMaxRetry, intRetryMinutes, sLegalEntity, intMaxRowsReturned, blnAutoConfirmation, ref intRetVal, ref sRetMessage);
                RetVal = (TMS.Configuration.ProcessDataReturnValues)intRetVal;
                if (RetVal != TMS.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete)
                {
                    switch (RetVal)
                    {
                        case TMS.Configuration.ProcessDataReturnValues.nglDataConnectionFailure:
                            Log("Error Data Connection Failure! could not export Picklist information:  " + sRetMessage);
                            break;
                        case TMS.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure:
                            Log("Picklist Integration Failure! could not export Picklist information:  " + sRetMessage);
                            break;
                        case TMS.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors:
                            Log("Picklist Integration Had Errors! could not export some Picklist information:  " + sRetMessage);
                            break;
                        case TMS.Configuration.ProcessDataReturnValues.nglDataValidationFailure:
                            Log("Picklist Integration Data Validation Failure! could not export Picklist information:  " + sRetMessage);
                            break;
                        default:
                            blnRet = true;
                            break;
                    }
                }
                if (oPickListData != null)
                {
                    Headers = oPickListData.Headers;
                    Details = oPickListData.Details;
                    Fees = oPickListData.Fees;
                }
                if (Headers != null && Headers.Count() > 0 && Headers[0] != null)
                {
                    blnRet = true;
                    Log("Picklist Headers Found: " + Headers.Count().ToString());
                    foreach (var h in Headers)
                    {
                        if (h != null)
                        {
                            Log("PL Control: " + h.PLControl.ToString());

                        } else
                        {
                            Log("PL Control is missing");
                        }

                    }
                    if (Details != null && Details.Count() > 0 && Details[0] != null)
                    {
                        Log("Total Details Found: " + Details.Count().ToString());
                    }
                    else
                    {
                        Log("No Details Found");
                        blnRet = false;
                    }

                }
                else
                {
                    Log("No Headers Found");
                    blnRet = false;
                }


                Log("Read Picklist Data Complete");
            }
            catch (Exception ex)
            {
                Log("Unexpected Picklist Integration Error! Could not import Picklist information:  " + ex.ToString());
            }

            return blnRet;
        }

        public static bool readTMSAPExportData(ref TMSIntegrationServices.clsAPExportObject85[] Headers, ref TMSIntegrationServices.clsAPExportDetailObject85[] Details, ref TMSIntegrationServices.clsAPExportFeeObject85[] Fees)
        {
            bool blnRet = false;
            try
            {
                Log("Begin Process APExport Data ");

                TMSIntegrationServices.DTMSERPIntegration oDTMSService = new TMSIntegrationServices.DTMSERPIntegration();

                var RetVal = TMS.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
                string sAuth = NGL.FM.CMDLine.D365Integration.Properties.Settings.Default.DTMSAuthCode; //  "WSPROD";
                oDTMSService.Url = NGL.FM.CMDLine.D365Integration.Properties.Settings.Default.NGL_FM_CMDLine_D365Integration_TMSIntegrationServices_DTMSERPIntegration;

                int intMaxRetry = 1;
                int intRetryMinutes = 30;
                int intMaxRowsReturned = 100;
                bool blnAutoConfirmation = false;
                int intRetVal = 0;
                string sRetMessage = "";

                string strCriteria = string.Format(" MaxRetry = {0} RetryMinutes = {1} MaxRowsReturned = {2} AutoConfirmation = {3} LegalEntity {4}", intMaxRetry, intRetryMinutes, intMaxRowsReturned, blnAutoConfirmation, sLegalEntity);
                Log("Criteria: " + strCriteria);
                TMSIntegrationServices.clsAPExportData85 oAPExportData = oDTMSService.GetAPData85(sAuth, intMaxRetry, intRetryMinutes, sLegalEntity, intMaxRowsReturned, blnAutoConfirmation, ref intRetVal, ref sRetMessage);
                RetVal = (TMS.Configuration.ProcessDataReturnValues)intRetVal;
                if (RetVal != TMS.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete)
                {
                    switch (RetVal)
                    {
                        case TMS.Configuration.ProcessDataReturnValues.nglDataConnectionFailure:
                            Log("Error Data Connection Failure! could not export AP information:  " + sRetMessage);
                            break;
                        case TMS.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure:
                            Log("APExport Integration Failure! could not export AP information:  " + sRetMessage);
                            break;
                        case TMS.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors:
                            Log("APExport Integration Had Errors! could not export some AP information:  " + sRetMessage);
                            break;
                        case TMS.Configuration.ProcessDataReturnValues.nglDataValidationFailure:
                            Log("APExport Integration Data Validation Failure! could not export AP information:  " + sRetMessage);
                            break;
                        default:
                            blnRet = true;
                            break;
                    }
                }
                if (oAPExportData != null)
                {
                    Headers = oAPExportData.Headers;
                    Details = oAPExportData.Details;
                    Fees = oAPExportData.Fees;
                }
                if (Headers != null && Headers.Count() > 0 && Headers[0] != null)
                {
                    blnRet = true;
                    Log("Headers Found: " + Headers.Count().ToString());
                    foreach (var h in Headers)
                    {
                        if (h != null)
                        {
                            Log("AP Control: " + h.APControl.ToString());

                        }
                        else
                        {
                            Log("AP Control is missing");
                        }

                    }
                    if (Details != null && Details.Count() > 0 && Details[0] != null)
                    {
                        Log("Total Details Found: " + Details.Count().ToString());
                    }
                    else
                    {
                        Log("No Details Found");
                        blnRet = false;
                    }

                }
                else
                {
                    Log("No AP Headers Found");
                    blnRet = false;
                }


                Log("Read APExport Data Complete");
            }
            catch (Exception ex)
            {
                Log("Unexpected APExport Integration Error! Could not import APExport information:  " + ex.ToString());
            }

            return blnRet;
        }

        /// <summary>  
        /// Create pick list required record
        /// </summary> 
        public static bool createPickListRequired(Resources context, string sLegalEntity, ref TMSIntegrationServices.clsPickListObject85[] Headers, ref TMSIntegrationServices.clsPickDetailObject85[] Details, ref TMSIntegrationServices.clsPickListFeeObject85[] Fees)
        {
            bool blnRet = false;
            try
            {
                // Create pick list required record - line 1
                decimal dTmp = 0;
                int iTmp = 0;
                if (Headers != null && Headers.Count() > 0)
                {
                    foreach (TMSIntegrationServices.clsPickListObject85 h in Headers)
                    {
                        // get the items for this header
                        TMSIntegrationServices.clsPickDetailObject85[] hDetails = Details.Where(x => x.PLControl == h.PLControl).ToArray();
                        foreach (TMSIntegrationServices.clsPickDetailObject85 i in hDetails)
                        {
                            Log("Adding Required Item to Pick Table: " + i.ItemNumber);
                            // Create pick list required record - line 
                            PickListRequired oPickRequired = new PickListRequired();
                            oPickRequired.DataAreaId = sLegalEntity;
                            oPickRequired.PLControlHeader = h.PLControl;
                            oPickRequired.BookCarrOrderNumberDetail = h.BookCarrOrderNumber;
                            oPickRequired.OrderSequenceDetail = h.BookOrderSequence;
                            oPickRequired.ItemNumberDetail = RequiredString(i.ItemNumber, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.LotNumberDetail = i.LotNumber;
                            oPickRequired.BookDoNotInvoiceHeader = h.BookDoNotInvoice == true ? NoYes.Yes : NoYes.No;
                            oPickRequired.FuelSurChargeHeader = RequiredDecimal(h.FuelSurCharge, 0, true);
                            oPickRequired.FreightCostDetail = RequiredDecimal(i.FreightCost, 0, true);
                            oPickRequired.BookItemOrderNumberDetail = RequiredString(i.BookItemOrderNumber, h.BookCarrOrderNumber, true);
                            oPickRequired.BookTransTypeHeader = h.BookTransType;
                            oPickRequired.BookRevNetCostHeader = RequiredDecimal(h.BookRevNetCost, 0, true);
                            oPickRequired.BookWhseAuthorizationNoHeader = h.BookWhseAuthorizationNo;
                            oPickRequired.CarrierLegalEntityHeader = h.CarrierLegalEntity;
                            oPickRequired.CarrierNameHeader = h.CarrierName;
                            oPickRequired.CompAlphaCodeDetail = h.CompAlphaCode;
                            oPickRequired.BookShipCarrierNumberHeader = h.BookShipCarrierNumber;
                            oPickRequired.BookProNumberDetail = h.BookProNumber;
                            oPickRequired.BookShipCarrierDetailsHeader = h.BookShipCarrierDetails;
                            oPickRequired.PackDetail = RequiredInteger(i.Pack, 0, true);
                            oPickRequired.StackableDetail = i.Stackable == true ? NoYes.Yes : NoYes.No;
                            oPickRequired.BookTotalPLHeader = RequiredDecimal(h.BookTotalPL, 0, true);
                            oPickRequired.FuelCostDetail = i.FuelCost;
                            oPickRequired.BookRevNonTaxableHeader = RequiredDecimal(h.BookRevNonTaxable, 0, true);
                            oPickRequired.BookLoadComHeader = h.BookLoadCom;
                            oPickRequired.BookDateRequiredHeader = DTMSDateString(h.BookDateRequired, null, true);
                            oPickRequired.BookRevCarrierCostHeader = RequiredDecimal(h.BookRevCarrierCost, 0, true);
                            oPickRequired.BookShipCarrierNameHeader = h.BookShipCarrierName;
                            oPickRequired.BookOrigCityHeader = h.BookOrigCity;
                            oPickRequired.PalletsDetail = RequiredDecimal(i.Pallets, 0, true);
                            oPickRequired.BookDateOrderedHeader = DTMSDateString(h.BookDateOrdered, null, true);
                            oPickRequired.LineHaulCostDetail = i.LineHaulCost;
                            oPickRequired.CompAlphaCodeHeader = RequiredString(h.CompAlphaCode, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.ItemCostDetail = i.ItemCost;
                            oPickRequired.BookPickupStopNumberHeader = h.BookPickupStopNumber;
                            oPickRequired.BookOrigCountryHeader = h.BookOrigCountry;
                            oPickRequired.BookStopNoHeader = RequiredInteger(h.BookStopNo, 1, false);
                            oPickRequired.BookOrigStateHeader = h.BookOrigState;
                            oPickRequired.BookDestCityHeader = h.BookDestCity;
                            oPickRequired.BookTotalBFCHeader = RequiredDecimal(h.BookTotalBFC, 0, true);
                            oPickRequired.BookSHIDHeader = h.BookSHID;
                            oPickRequired.BookOrigNameHeader = h.BookOrigName;
                            oPickRequired.PalletTypeDetail = i.PalletType;
                            oPickRequired.BFCDetail = i.BFC;
                            oPickRequired.BookOrigZipHeader = h.BookOrigZip;
                            oPickRequired.BookDestAddress1Header = h.BookDestAddress1;
                            oPickRequired.WeightDetail = RequiredDecimal(i.Weight, 0, true);
                            oPickRequired.BookOrderSequenceHeader = h.BookOrderSequence;
                            oPickRequired.QtyOrderedDetail = RequiredInteger(i.QtyOrdered, 1, false);
                            oPickRequired.BookRouteFinalDateHeader = DTMSDateString(h.BookRouteFinalDate);
                            oPickRequired.BookShipCarrierProNumberHeader = h.BookShipCarrierProNumber;
                            oPickRequired.CarrierAlphaCodeHeader = h.CarrierAlphaCode;
                            oPickRequired.BookRevLoadSavingsHeader = RequiredDecimal(h.BookRevLoadSavings, 0, true);
                            oPickRequired.BookLoadPONumberHeader = h.BookLoadPONumber;
                            oPickRequired.BookFinServiceFeeHeader = RequiredDecimal(h.BookFinServiceFee, 0, true);
                            oPickRequired.BookDestZipHeader = h.BookDestZip;
                            oPickRequired.BookDateLoadHeader = DTMSDateString(h.BookDateLoad, null, true);
                            oPickRequired.BookDestNameHeader = h.BookDestName;
                            oPickRequired.BookRouteFinalCodeHeader = h.BookRouteFinalCode;
                            oPickRequired.BookRevTotalCostHeader = RequiredDecimal(h.BookRevTotalCost, 0, true);
                            oPickRequired.CustomerPONumberDetail = i.CustomerPONumber;
                            oPickRequired.BookCarrOrderNumberHeader = h.BookCarrOrderNumber;
                            oPickRequired.BookPickNumberHeader = RequiredInteger(h.BookPickNumber, 1, false);
                            oPickRequired.BookMilesFromHeader = h.BookMilesFrom;
                            oPickRequired.CarrierNumberHeader = RequiredInteger(h.CarrierNumber, 0, true);
                            oPickRequired.CompNameHeader = h.CompName;
                            oPickRequired.BookOrigAddress1Header = h.BookOrigAddress1;
                            oPickRequired.BookDestCountryHeader = h.BookDestCountry;
                            oPickRequired.BookTotalWgtHeader = RequiredDecimal(h.BookTotalWgt, 1, false);
                            oPickRequired.BookRevOtherCostHeader = RequiredDecimal(h.BookRevOtherCost, 0, true);
                            oPickRequired.BookFinAPGLNumberHeader = h.BookFinAPGLNumber;
                            oPickRequired.BookRevFreightTaxHeader = RequiredDecimal(h.BookRevFreightTax, 0, true);
                            oPickRequired.BookTotalCubeHeader = RequiredInteger(h.BookTotalCube, 1, false);
                            oPickRequired.LoadOrderHeader = RequiredInteger(h.LoadOrder, 0, true);
                            oPickRequired.BookDestStateHeader = h.BookDestState;
                            oPickRequired.CubeDetail = RequiredInteger(i.Cube, 0, true);
                            oPickRequired.BookTypeCodeHeader = h.BookTypeCode;
                            oPickRequired.TotalNonFuelFeesHeader = RequiredDecimal(h.TotalNonFuelFees, 0, true);
                            oPickRequired.BookConsPrefixHeader = h.BookConsPrefix;
                            oPickRequired.CommCodeDescriptionHeader = h.CommCodeDescription;
                            oPickRequired.BookTotalCasesHeader = RequiredInteger(h.BookTotalCases, 1, false);
                            oPickRequired.FeesCostDetail = i.FeesCost;
                            oPickRequired.BookProNumberHeader = h.BookProNumber;
                            //Modified by RHR for v-8.5.3.003 on 07/12/2022
                            oPickRequired.CarrierEquipmentCodesHeader = h.CarrierEquipmentCodes; // maps to transportation mode;
                            oPickRequired.BookCarrierTypeCodeHeader = h.BookCarrierTypeCode; // "BookCarrierTypeCode-updated";
                            context.AddToPickListRequireds(oPickRequired);
                        }

                    }
                }
                context.SaveChanges();
                blnRet = true;
                Log("Changes Saved to D365 Pick Table");
                Console.WriteLine("entity has been created ok !");
            }
            catch (Exception ex)
            {
                Log(ex.Message + ex.InnerException);
            }
            return blnRet;
        }

        public static bool createAPExportRequired(Resources context, string sLegalEntity, ref TMSIntegrationServices.clsAPExportObject85[] Headers, ref TMSIntegrationServices.clsAPExportDetailObject85[] Details, ref TMSIntegrationServices.clsAPExportFeeObject85[] Fees)
        {
            bool blnRet = false;
            try
            {
                // Create AP Export required record - line 1
                decimal dTmp = 0;
                int iTmp = 0;
                if (Headers != null && Headers.Count() > 0)
                {
                    foreach (TMSIntegrationServices.clsAPExportObject85 h in Headers)
                    {
                        // get the items for this header
                        TMSIntegrationServices.clsAPExportDetailObject85[] hDetails = Details.Where(x => x.APControl == h.APControl).ToArray();
                        foreach (TMSIntegrationServices.clsAPExportDetailObject85 i in hDetails)
                        {
                            Log("Adding Required Item to AP Export Table: " + i.ItemNumber);
                            // Create AP Export required record - line 
                            ImportRequired oAPR = new ImportRequired();
                            oAPR.DataAreaId = sLegalEntity;
                            oAPR.APControl = h.APControl;
                            oAPR.CarrierNumber = h.CarrierNumber;
                            oAPR.BookFinAPBillNumber = h.BookFinAPBillNumber;
                            oAPR.BookFinAPBillInvDate = DTMSDateString(h.BookFinAPBillInvDate, null, true);
                            oAPR.BookCarrOrderNumber = h.BookCarrOrderNumber;
                            oAPR.BookFinAPActCost = RequiredDecimal(h.BookFinAPACtCost, 0, true);
                            oAPR.BookFinAPActWgt = h.BookFinAPActWgt;
                            oAPR.BookFinAPBillNoDate = DTMSDateString(h.BookFinAPBillNoDate, null, true);
                            oAPR.BookProNumber = h.BookProNumber;
                            oAPR.CompanyNumber = h.CompanyNumber.ToString();
                            oAPR.BookOrderSequence = h.BookOrderSequence;
                            oAPR.APFee1 = RequiredDecimal(h.APFee1, 0, true);
                            oAPR.APFee2 = RequiredDecimal(h.APFee2, 0, true);
                            oAPR.APFee3 = RequiredDecimal(h.APFee3, 0, true);
                            oAPR.APFee4 = RequiredDecimal(h.APFee4, 0, true);
                            oAPR.APFee5 = RequiredDecimal(h.APFee5, 0, true);
                            oAPR.APFee6 = RequiredDecimal(h.APFee6, 0, true);
                            oAPR.OtherCosts = RequiredDecimal(h.OtherCosts, 0, true);
                            oAPR.BookShipCarrierProNumber = h.BookShipCarrierProNumber;
                            oAPR.BookShipCarrierNumber = h.BookShipCarrierNumber;
                            oAPR.BookSHID = h.BookSHID;
                            oAPR.CarrierAlphaCode = h.CarrierAlphaCode;
                            oAPR.CarrierLegalEntity = h.CarrierLegalEntity;
                            oAPR.CompLegalEntity = h.CompLegalEntity;
                            oAPR.CompAlphaCode = h.CompAlphaCode;
                            oAPR.BookConsPrefix = h.BookConsPrefix;
                            oAPR.BookRouteConsFlag = h.BookRouteConsFlag == true ? NoYes.Yes : NoYes.No;
                            oAPR.BookShipCarrierName = h.BookShipCarrierName;
                            oAPR.BookShipCarrierDetails = h.BookShipCarrierDetails;
                            oAPR.BookFinAPStdCost = RequiredDecimal(h.BookFinAPStdCost, 0, true);
                            oAPR.BookFinAPGLNumber = h.BookFinAPGLNumber;
                            oAPR.APReduction = RequiredDecimal(h.APReduction, 0, true);
                            oAPR.APReductionAdjustedCost = RequiredDecimal(h.APReductionAdjustedCost, 0, true);
                            oAPR.OrderNumber = i.OrderNumber;
                            oAPR.ItemNumber = RequiredString(i.ItemNumber, "S-" + h.APControl.ToString(), false);
                            oAPR.LotNumber = i.LotNumber;
                            oAPR.QtyOrdered = RequiredDecimal(i.QtyOrdered, 1, true);
                            oAPR.FreightCost = i.FreightCost;                            
                            oAPR.LineHaulCost = i.LineHaulCost;
                            oAPR.FuelCost = i.FuelCost;
                            oAPR.FeesCost = i.FeesCost;
                            oAPR.ItemCost = i.ItemCost;
                            oAPR.Weight = RequiredDecimal(i.Weight,0,true);
                            //oAPR.Cube = i.Cube;
                            oAPR.Size = i.Size;
                            oAPR.CustItemNumber = i.CustItemNumber;
                            oAPR.CustomerNumber = i.CustomerNumber;
                            oAPR.OrderSequence = i.OrderSequence;
                            oAPR.CustomerPONumber = i.CustomerPONumber;
                            oAPR.BookProNumber = i.BookProNumber;
                            oAPR.CompLegalEntity = i.CompLegalEntity;
                            oAPR.CompAlphaCode = i.CompAlphaCode;
                            oAPR.Pallets = RequiredDecimal(i.Pallets, 0, true);
                            oAPR.Stackable = i.Stackable == true ? NoYes.Yes : NoYes.No;
                            oAPR.BookItemOrderNumber = RequiredString(i.BookItemOrderNumber, h.BookCarrOrderNumber, true);
                            context.AddToImportRequireds(oAPR);
                        }

                    }
                }
                context.SaveChanges();
                blnRet = true;
                Log("Changes Saved to D365 Pick Table");
                Console.WriteLine("entity has been created ok !");
            }
            catch (Exception ex)
            {
                Log(ex.Message + ex.InnerException);
            }
            return blnRet;
        }


        /// <summary>  
        /// Update pick list optional record
        /// </summary> 
        public static void updatePickListOptionalFields(Resources context, string sLegalEntity, ref TMSIntegrationServices.clsPickListObject85[] Headers, ref TMSIntegrationServices.clsPickDetailObject85[] Details, ref TMSIntegrationServices.clsPickListFeeObject85[] Fees)
        {
            try
            {
                //PickListOptional pickList = context.PickListOptionals.AddQueryOption("cross-company", "true").Where(x => x.DataAreaId == "USMF" && x.PLControlHeader == 14).First();
                ////PickListOptional pickList = context.PickListOptionals.AddQueryOption("cross-company", "true").AddQueryOption("$filter", "PLControlHeader eq 14 and dataAreaId eq 'USMF'").First();
                //  Where(x => x.DataAreaId == "USMF" && x.PLControlHeader == 14).First();

                //Console.WriteLine("*** DISPLAY REQUIRED ENTITY FIELDS ***");
                //Console.WriteLine(JsonConvert.SerializeObject(pickList));
                //Console.WriteLine("*** UPDATING OPTIONAL FIELDS ***");

                decimal dTmp = 0;
                int iTmp = 0;
                if (Headers != null && Headers.Count() > 0)
                {
                    foreach (TMSIntegrationServices.clsPickListObject85 h in Headers)
                    {
                        // get the items for this header
                        TMSIntegrationServices.clsPickDetailObject85[] hDetails = Details.Where(x => x.PLControl == h.PLControl).ToArray();
                        foreach (TMSIntegrationServices.clsPickDetailObject85 i in hDetails)
                        {
                            Log("Adding Optional Item to Pick Table: " + i.ItemNumber);
                            PickListOptional pickList = context.PickListOptionals.Where(x => x.DataAreaId == sLegalEntity && x.PLControlHeader == h.PLControl).FirstOrDefault();
                            // Create pick list required record - line                             
                            pickList.BookCarrOrderNumberDetail = h.BookCarrOrderNumber;
                            pickList.OrderSequenceDetail = h.BookOrderSequence;
                            pickList.ItemNumberDetail = i.ItemNumber;
                            pickList.LotNumberDetail = i.LotNumber;
                            pickList.HazmatDetail = i.Hazmat;
                            //pickList.BookCarrActLoadCompleteDateHeader = new DateTime(2021, 12, 31);
                            pickList.BookItemRatedMarineCodeDetail = i.BookItemRatedMarineCode;
                            pickList.CostCenterDetail = i.CostCenter;
                            pickList.LaneNumberHeader = h.LaneNumber;
                            pickList.BookCarrTripNoHeader = h.BookCarrTripNo; // "BookCarrTripNoHeader-updated-g4";
                            pickList.BookItemDiscountDetail = RequiredDecimal(i.BookItemDiscount, 0, true);
                            //pickList.BookCarrScheduleDateHeader = new DateTime(2021, 12, 31);
                            pickList.BookItemRatedIATACodeDetail = i.BookItemRatedIATACode;
                            pickList.Hazmat49CFRCodeDetail = i.Hazmat49CFRCode;
                            pickList.BookOrigAddress3Header = h.BookOrigAddress3;
                            pickList.BookRevCommCostHeader = RequiredDecimal(h.BookRevCommCost, 0, true);
                            pickList.PLExportedHeader = NoYes.Yes;
                            //pickList.BookCarrStartUnloadingDateHeader = new DateTime(2021, 12, 31);
                            pickList.DOTCodeDetail = i.DOTCode; // "DOTCode-updated";
                            pickList.LevelOfDensityDetail = i.LevelOfDensity;// 11;
                            //pickList.BookCarrScheduleTimeHeader = 22;
                            //pickList.BookCarrFinishUnloadingDateHeader = new DateTime(2021, 12, 31);
                            pickList.PLExportRetryHeader = 0;
                            pickList.SizeDetail = i.Size; // "Size-updated";
                            pickList.BookItemRated49CFRCodeDetail = i.BookItemRated49CFRCode; // "BookItemRated49CFRCode-updated";
                            pickList.BookRevGrossRevenueHeader = RequiredDecimal(h.BookRevGrossRevenue, 0, true);
                            pickList.BookDestAddress2Header = h.BookDestAddress2;
                            //pickList.BookCarrStartLoadingDateHeader = new DateTime(2021, 12, 31);
                            pickList.LaneCommentsHeader = h.LaneComments; // "LaneComments-updated";
                            //pickList.BookCarrActualTimeHeader = 30;
                            pickList.CompLegalEntityHeader = h.CompLegalEntity;
                            //pickList.BookCarrFinishUnloadingTimeHeader = 0;
                            pickList.CountryOfOrginDetail = ""; // not available
                            pickList.BookAlternateAddressLaneNumberHeader = h.BookAlternateAddressLaneNumber; // "BookAlternateAddressLaneNumber-updated";
                            pickList.CompLegalEntityDetail = h.CompLegalEntity; // "CompLegalEntity-updated";
                            pickList.DescriptionDetail = i.Description; // "Description-updated";
                            pickList.LotExpirationDateDetail = DTMSDateString(i.LotExpirationDate, null, true);
                            pickList.BookRouteConsFlagDetail = h.BookRouteConsFlag == true ? NoYes.Yes : NoYes.No;
                            pickList.BookDestAddress3Header = h.BookDestAddress3; // "BookDestAddress3-updated";
                            pickList.CompNatNumberHeader = h.CompNatNumber; // 11;
                            pickList.LaneLegalEntityHeader = h.LaneLegalEntity; // "";
                            pickList.BookCarrSealNoHeader = h.BookCarrSealNo; // "BookCarrSealNo-updated";
                            pickList.BookCommCompControlHeader = h.BookCommCompControl; // 0;
                            pickList.CustItemNumberDetail = i.CustItemNumber; // "CustItemNumber-updated";
                            pickList.HighsDetail = RequiredDecimal(i.Highs, 0, true); // 20;
                            pickList.QtyHeightDetail = RequiredDecimal(i.QtyHeight, 0, true);
                            pickList.BookOriginalLaneNumberHeader = h.BookOriginalLaneNumber;// "BookOriginalLaneNumber-updated";
                            //pickList.BookCarrApptTimeHeader = 40;
                            pickList.CompNatNumberDetail = i.CompNatNumber;
                            pickList.BookAlternateAddressLaneNumberDetail = h.BookAlternateAddressLaneNumber; //"BookAlternateAddressLaneNumber-updated";
                            pickList.BookItemWeightBreakDetail = RequiredDecimal(i.BookItemWeightBreak, 0, true);
                            pickList.BookItemLineHaulDetail = RequiredDecimal(i.BookItemLineHaul, 0, true);
                            pickList.QtyWidthDetail = RequiredDecimal(i.QtyWidth, 0, true);
                            //pickList.BookcarrStartUnloadingTimeHeader = 240;
                            pickList.BookCarrDriverNameHeader = h.BookCarrDriverName; // "BookCarrDriverName-updated";
                            pickList.HSTDetail = RequiredDecimal(i.HST, 0, true);
                            pickList.BookOriginalLaneLegalEntityHeader = h.BookOriginalLaneLegalEntity; // "BookOriginalLaneLegalEntity-updated";
                            pickList.BookItemRatedDOTCodeDetail = i.BookItemRatedDOTCode;// "BookItemRatedDOTCode-updated";
                            pickList.BrandDetail = i.Brand; // "Brand-updated";
                            pickList.BookItemRatedFAKClassDetail = i.BookItemRatedFAKClass; // "BookItemRatedFAKClass-updated";
                            //pickList.BookCarrStartLoadingTimeHeader = 10;
                            //pickList.BookCarrFinishLoadingTimeHeader = 40;
                            pickList.QtyLengthDetail = RequiredDecimal(i.QtyLength, 0, true);
                            pickList.BookFinCommStdHeader = RequiredDecimal(h.BookFinCommStd, 0, true);
                            pickList.BookItemRatedNMFCSubClassDetail = i.BookItemRatedNMFCSubClass; // "BookItemRatedNMFCSubClassD-updated";
                            pickList.BookItemTaxesDetail = RequiredDecimal(i.BookItemTaxes, 0, true);
                            pickList.BookItemNonTaxableFeesDetail = RequiredDecimal(i.BookItemNonTaxableFees, 0, true);
                            pickList.FAKClassDetail = i.FAKClass; // "FAKClass-updated";
                            pickList.BookWarehouseNumberHeader = h.BookWarehouseNumber; // "BookWarehouseNumber-updated";
                            pickList.BookItemTaxableFeesDetail = RequiredDecimal(i.BookItemTaxableFees, 0, true);
                            pickList.CustomerNumberDetail = i.CustomerNumber; // "CustomerNumber-updated";
                            pickList.MarineCodeDetail = i.MarineCode; // "MarineCode-updated";
                            //pickList.BookCarrFinishLoadingDateHeader = new DateTime(2021, 12, 31);
                            //pickList.BookCarrActUnloadCompDateHeader = new DateTime(2021, 12, 31);
                            //pickList.BookCarrActTimeHeader = 0;
                            pickList.IATACodeDetail = i.IATACode; // "IATACodeD-updated";
                            pickList.TiesDetail = RequiredDecimal(i.Ties, 0, true);
                            pickList.NMFCClassDetail = i.BookItemRatedNMFCClass; // "NMFCClass-upddated";
                            //pickList.BookCarrActualDateHeader = new DateTime(2021, 12, 31);
                            pickList.CompNumberHeader = h.CompNumber.ToString(); // "CompNumber-updated";
                                                                                 // pickList.BookCarrActDateHeader = new DateTime(2021, 12, 31);
                            pickList.QtyPalletPercentageDetail = RequiredDecimal(i.QtyPalletPercentage, 0, true);
                            //pickList.BookCarrApptDateHeader = new DateTime(2021, 12, 31);
                            //pickList.BookCarrActUnloadCompTimeHeader = 0;
                            pickList.LimitedQtyFlagDetail = i.LimitedQtyFlag ? NoYes.Yes : NoYes.No;
                            pickList.BookCarrRouteNoHeader = h.BookCarrRouteNo; //  "BookCarrRouteNo-updated";
                            //pickList.BookCarrActLoadCompleteTimeHeader = 0;
                            pickList.BookItemRatedNMFCClassDetail = i.BookItemRatedNMFCClass; // "BookItemRatedNMFCClass-uupdated";
                            pickList.PLExportDateHeader = DateTime.Now;
                            pickList.GTINDetail = i.GTIN; // "GTIND-updated";
                            pickList.BookCarrDriverNoHeader = h.BookCarrDriverNo; // "BookCarrDriverNo-updated";
                            pickList.BookOrigAddress2Header = h.BookOrigAddress2; // "BookOrigAddress2-updated";
                            pickList.BookCarrTrailerNoHeader = h.BookCarrTrailerNo; // "BookCarrTrailerNo-updated";
                            pickList.HazmatTypeCodeDetail = i.HazmatTypeCode; // "HazmatTypeCode-updated";
                            pickList.DescriptionDetail = i.Description; // "DescDetail-updated";
                            pickList.CustomerNumberDetail = i.CustomerNumber; // "0001-updated";
                            context.UpdateObject(pickList);
                        };
                        context.SaveChanges();
                        Log("Save Optional Item");
                    }


                }
                Log("entity has been updated ok !");
            }
            catch (Exception ex)
            {
                Log(ex.Message + ex.InnerException);
            }
        }

        /// <summary>  
        /// Update pick list optional record
        /// </summary> 
        public static void updateAPExportOptionalFields(Resources context, string sLegalEntity, ref TMSIntegrationServices.clsAPExportObject85[] Headers, ref TMSIntegrationServices.clsAPExportDetailObject85[] Details, ref TMSIntegrationServices.clsAPExportFeeObject85[] Fees)
        {
            try
            {
                decimal dTmp = 0;
                int iTmp = 0;
                if (Headers != null && Headers.Count() > 0)
                {
                    foreach (TMSIntegrationServices.clsAPExportObject85 h in Headers)
                    {
                        // get the items for this header
                        TMSIntegrationServices.clsAPExportDetailObject85[] hDetails = Details.Where(x => x.APControl == h.APControl).ToArray();
                        foreach (TMSIntegrationServices.clsAPExportDetailObject85 i in hDetails)
                        {
                            Log("Adding Optional Item to Pick Table: " + i.ItemNumber);
                            ImportOptional APImport = context.ImportOptionals.Where(x => x.DataAreaId == sLegalEntity && x.APControl == h.APControl).FirstOrDefault();
                            // Update AP Optional data for header and  line                             
                            APImport.LaneNumber = h.LaneNumber;
                            APImport.BookItemCostCenterNumber = h.BookItemCostCenterNumber;
                            APImport.BookCarrBLNumber = h.BookCarrBLNumber;
                            APImport.BookFinAPActTax = RequiredDecimal(h.BookFinAPActTax, 0, true);
                            APImport.BookFinAPExportRetry = h.BookFinAPExportRetry;
                            APImport.BookFinAPExportDate = DTMSDateString(h.BookFinAPExportDate);
                            APImport.PrevSentDate = DTMSDateString(h.PrevSentDate);
                            APImport.CarrierEquipmentCodes = h.CarrierEquipmentCodes;
                            APImport.BookCarrierTypeCode = h.BookCarrierTypeCode; // "BookCarrTripNoHeader-updated-g4";
                            APImport.BookWarehouseNumber = h.BookWarehouseNumber;
                            APImport.BookMilesFrom = RequiredDecimal(h.BookMilesFrom, 0, true);
                            APImport.CompNatNumber = h.CompNatNumber;
                            APImport.BookReasonCode = h.BookReasonCode;
                            APImport.BookTransType = RequiredInteger(h.BookTransType, 0, true);
                            APImport.APTaxDetail1 = RequiredDecimal(h.APTaxDetail1, 0, true);
                            APImport.APTaxDetail2 = RequiredDecimal(h.APTaxDetail2, 0, true);
                            APImport.APTaxDetail3 = RequiredDecimal(h.APTaxDetail3, 0, true);
                            APImport.APTaxDetail4 = RequiredDecimal(h.APTaxDetail4, 0, true);
                            APImport.APTaxDetail5 = RequiredDecimal(h.APTaxDetail5, 0, true);
                            APImport.LaneLegalEntity = h.LaneLegalEntity; 
                            APImport.BookFinAPTotalTaxableFees = RequiredDecimal(h.BookFinAPTotalTaxableFees, 0, true);
                            APImport.BookFinAPTotalTaxes = RequiredDecimal(h.BookFinAPTotalTaxes, 0, true);
                            APImport.BookFinAPNonTaxableFees = RequiredDecimal(h.BookFinAPNonTaxableFees, 0, true);
                            APImport.BookWhseAuthorizationNo = h.BookWhseAuthorizationNo;
                            APImport.APReductionReason = h.APReductionReason;
                            APImport.Pack = (int)i.Pack;
                            APImport.Description = i.Description;
                            APImport.Hazmat = i.Hazmat;
                            APImport.Brand = i.Brand;
                            APImport.CostCenter = i.CostCenter;
                            APImport.LotExpirationDate = DTMSDateString(i.LotExpirationDate);
                            APImport.GTIN = i.GTIN;
                            APImport.BFC = RequiredDecimal(i.BFC, 0, true);
                            APImport.LotExpirationDate = DTMSDateString(i.LotExpirationDate, null, true);
                            APImport.CountryOfOrgin = ""; // i.CountryOfOrgin; is missing
                            APImport.HST = i.HST;
                            APImport.PalletType = i.PalletType;
                            APImport.CompNatNumber = i.CompNatNumber;
                            APImport.BookItemDiscount = RequiredDecimal(i.BookItemDiscount, 0, true);
                            APImport.BookItemLineHaul = RequiredDecimal(i.BookItemLineHaul, 0, true);
                            APImport.BookItemTaxableFees = RequiredDecimal(i.BookItemTaxableFees, 0, true);
                            APImport.BookItemTaxes = RequiredDecimal(i.BookItemTaxes, 0, true); // 20;
                            APImport.BookItemNonTaxableFees = RequiredDecimal(i.BookItemNonTaxableFees, 0, true);
                            APImport.BookItemWeightBreak = RequiredDecimal(i.BookItemWeightBreak, 0, true);
                            APImport.BookItemRated49CFRCode = i.BookItemRated49CFRCode;
                            APImport.BookItemRatedIATACode = i.BookItemRatedIATACode;
                            APImport.BookItemRatedDOTCode = i.BookItemRatedDOTCode;
                            APImport.BookItemRatedMarineCode = i.BookItemRatedMarineCode;
                            APImport.BookItemRatedNMFCClass = i.BookItemRatedNMFCClass;
                            APImport.BookItemRatedNMFCSubClass = i.BookItemRatedNMFCSubClass;
                            APImport.BookItemRatedFAKClass = i.BookItemRatedFAKClass;
                            APImport.HazmatTypeCode = i.HazmatTypeCode;
                            APImport.Hazmat49CFRCode = i.Hazmat49CFRCode;
                            APImport.IATACode = i.IATACode;
                            APImport.DOTCode = i.DOTCode; 
                            APImport.MarineCode = i.MarineCode;
                            APImport.NMFCClass = i.NMFCClass;
                            APImport.FAKClass = i.FAKClass;
                            APImport.LimitedQtyFlag = i.LimitedQtyFlag == true ? NoYes.Yes : NoYes.No;
                            APImport.Ties = RequiredDecimal(i.Ties, 0, true);
                            APImport.Highs = RequiredDecimal(i.Highs, 0, true);
                            APImport.QtyPalletPercentage = RequiredDecimal(i.QtyPalletPercentage, 0, true);
                            APImport.QtyLength = RequiredDecimal(i.QtyLength, 0, true);
                            APImport.QtyWidth = RequiredDecimal(i.QtyWidth, 0, true);
                            APImport.QtyHeight = RequiredDecimal(i.QtyHeight, 0, true);
                            APImport.LevelOfDensity = i.LevelOfDensity;                            
                            context.UpdateObject(APImport);
                        };
                        context.SaveChanges();
                        Log("Save Optional Item");
                    }


                }
                Log("entity has been updated ok !");
            }
            catch (Exception ex)
            {
                Log(ex.Message + ex.InnerException);
            }
        }
    }
}
