using System;
using System.Linq;
using System.Threading.Tasks;

// added manually
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Dynamics.DataEntities; //Resources
using Microsoft.Identity.Client; //Nuget Package: IConfidentialClientApplication, ConfidentialClientApplicationBuilder, AuthenticationResult
using Microsoft.OData.Client; //DataServiceQueryException, DataServiceClientException
using System.Web; //Manually browsed and added reference to System.Web: HttpUtility
using Newtonsoft.Json;

using Test_OData_ConsoleApp;
using System.Collections.Generic;

using System.Text;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;


using TMS = Ngl.FreightMaster.Integration;
using DAL = Ngl.FreightMaster.Data;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;

namespace Test_OData_ConsoleApp
{
    class Class1
    {

        public static string sConnectionString = "Server=nglsql03p;User ID=nglweb;Password=5529;Database=NGLMASD365";
        public static string ODataEntityPath = ClientConfiguration.Default.UriString + "data";

        static void Main(string[] args)
        {
            //Retrieves an authentication header for the Web API call - pass authentication
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
            string sLegalEntity = "usmf";
            TMS.clsPickListObject80[] Headers = new TMS.clsPickListObject80[1];
            TMS.clsPickDetailObject80[] Details = new TMS.clsPickDetailObject80[1];
            TMS.clsPickListFeeObject80[] Fees = new TMS.clsPickListFeeObject80[1];
            bool bRet = readTMSPicklistData(ref Headers, ref Details, ref Fees);


            //Create pick list required record
            createPickListRequired(context, sLegalEntity, ref Headers, ref Details, ref Fees);

            //Retrieves pick list required record
            //readPickList(context); 

            //Update pick list optional record
            //updatePickListOptionalFields(context);

            //Update pick list optional record V2
            //updatePickListOptinalFieldsV2(context);

            //updateCustGroup(context);
            //updateCustGroup2(context);

            Console.ReadLine();
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


        public static bool readTMSPicklistData(ref TMS.clsPickListObject80[] Headers, ref TMS.clsPickDetailObject80[] Details, ref TMS.clsPickListFeeObject80[] Fees)
        {
            bool blnRet = false;
            try
            {
                Log("Begin Process Picklist Data ");
                TMS.clsPickList picklist = new TMS.clsPickList();
                string strMsg = "";
                // set the default value to false
                var RetVal = TMS.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
                int intMaxRetry = 1;
                int intRetryMinutes = 30;
                // If Unit Test Keys are provided and we have a Legal Entity then we are running a unit test
                picklist.MaxRowsReturned = 100;
                picklist.AutoConfirmation = false;

                string strCriteria = string.Format(" MaxRetry = {0} RetryMinutes = {1} MaxRowsReturned = {2} AutoConfirmation = {3}", intMaxRetry, intRetryMinutes, picklist.MaxRowsReturned, picklist.AutoConfirmation);
                //string sConnectionString = Properties.Settings.Default.strConnection;
                string sLegalEntity = "USMF";
                Log("Connection: " + sConnectionString);
                RetVal = picklist.readObjectData80(ref Headers, sConnectionString, intMaxRetry, intRetryMinutes, sLegalEntity, ref Fees, ref Details);
                string LastError = picklist.LastError;
                if (RetVal != TMS.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete)
                {
                    switch (RetVal)
                    {
                        case TMS.Configuration.ProcessDataReturnValues.nglDataConnectionFailure:
                            Log("Error Data Connection Failure! could not export Picklist information:  " + LastError);
                            break;
                        case TMS.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure:
                            Log("Picklist Integration Failure! could not export Picklist information:  " + LastError);
                            break;
                        case TMS.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors:
                            Log("Picklist Integration Had Errors! could not export some Picklist information:  " + LastError);
                            break;
                        case TMS.Configuration.ProcessDataReturnValues.nglDataValidationFailure:
                            Log("Picklist Integration Data Validation Failure! could not export Picklist information:  " + LastError);
                            break;
                        default:
                            blnRet = true;
                            break;
                    }
                }
                if (Headers != null && Headers.Count() > 0)
                {
                    Log("Headers Found: " + Headers.Count().ToString());
                    foreach (var h in Headers)
                    {
                        Log("PL Control: " + h.PLControl.ToString());
                    }
                    if (Details != null && Details.Count() > 0)
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
                //if (Headers != null && Headers.Count() > 0)
                //{
                //    foreach (var p in Headers)
                //    {
                //        try
                //        {
                //            picklist.confirmExport(sConnectionString, p.PLControl);
                //        }
                //        catch (Exception ex)
                //        {
                //            Log(" Update Picklist Confirmation Error for Order Number: " + p.BookCarrOrderNumber + " using PL Control Number: " + p.PLControl);
                //        }
                //    }
                //}
                //else
                //    Log("No Pick List Status Updates to Process");
                blnRet = true;
                Log("Read Picklist Data Complete");
            }
            catch (Exception ex)
            {
                Log("Unexpected Picklist Integration Error! Could not import Picklist information:  " + ex.ToString());
            }

            return blnRet;
        }



        /// <summary>  
        /// Retrieves pick list required record
        /// </summary> 
        public static void readPickList(Resources context)
        {
            var pickList = context.PickListRequireds.AddQueryOption("cross-company", "true").Where(x => x.DataAreaId == "USMF").First();
            Console.WriteLine(JsonConvert.SerializeObject(pickList));

        }

        /// <summary>  
        /// Create pick list required record
        /// </summary> 
        public static void createPickListRequired(Resources context, string sLegalEntity, ref TMS.clsPickListObject80[] Headers, ref TMS.clsPickDetailObject80[] Details, ref TMS.clsPickListFeeObject80[] Fees)
        {

            try
            {
                // Create pick list required record - line 1
                decimal dTmp = 0;
                int iTmp = 0;
                if (Headers != null && Headers.Count() > 0)
                {
                    foreach (TMS.clsPickListObject80 h in Headers)
                    {
                        // get the items for this header
                        TMS.clsPickDetailObject80[] hDetails = Details.Where(x => x.PLControl == h.PLControl).ToArray();
                        foreach (TMS.clsPickDetailObject80 i in hDetails)
                        {
                            Log("Adding Required Item to Pick Table: " + i.ItemNumber);
                            // Create pick list required record - line 
                            PickListRequired oPickRequired = new PickListRequired();
                            oPickRequired.DataAreaId = sLegalEntity;
                            oPickRequired.PLControlHeader = h.PLControl;
                            oPickRequired.BookCarrOrderNumberDetail = RequiredString(h.BookCarrOrderNumber, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.OrderSequenceDetail = RequiredInteger(h.BookOrderSequence, 1, false);
                            oPickRequired.ItemNumberDetail = RequiredString(i.ItemNumber, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.LotNumberDetail = RequiredString(i.LotNumber, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.BookDoNotInvoiceHeader = h.BookDoNotInvoice == true ? NoYes.Yes : NoYes.No;
                            oPickRequired.FuelSurChargeHeader = RequiredDecimal(h.FuelSurCharge, 1, false);
                            oPickRequired.FreightCostDetail = RequiredDecimal(i.FreightCost, 1, false);
                            oPickRequired.BookItemOrderNumberDetail = RequiredString(i.BookItemOrderNumber, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.BookTransTypeHeader = RequiredString(h.BookTransType, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.BookRevNetCostHeader = RequiredDecimal(h.BookRevNetCost, 1, false);
                            oPickRequired.BookWhseAuthorizationNoHeader = RequiredString(h.BookWhseAuthorizationNo, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.CarrierLegalEntityHeader = RequiredString(h.CarrierLegalEntity, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.CarrierNameHeader = RequiredString(h.CarrierName, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.CompAlphaCodeDetail = RequiredString(h.CompAlphaCode, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.BookShipCarrierNumberHeader = RequiredString(h.BookShipCarrierNumber, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.BookProNumberDetail = RequiredString(h.BookProNumber, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.BookShipCarrierDetailsHeader = RequiredString(h.BookShipCarrierDetails, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.PackDetail = RequiredInteger(i.Pack, 1, false);
                            oPickRequired.StackableDetail = i.Stackable == true ? NoYes.Yes : NoYes.No;
                            oPickRequired.BookTotalPLHeader = RequiredDecimal(h.BookTotalPL, 1, false);
                            oPickRequired.FuelCostDetail = 1;
                            oPickRequired.BookRevNonTaxableHeader = RequiredDecimal(h.BookRevNonTaxable, 1, false);
                            oPickRequired.BookLoadComHeader = RequiredString(h.BookLoadCom, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.BookDateRequiredHeader = RequiredString(h.BookDateRequired, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.BookRevCarrierCostHeader = RequiredDecimal(h.BookRevCarrierCost, 1, false);
                            oPickRequired.BookShipCarrierNameHeader = RequiredString(h.BookShipCarrierName, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.BookOrigCityHeader = RequiredString(h.BookOrigCity, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.PalletsDetail = RequiredDecimal(i.Pallets, 1, false);
                            oPickRequired.BookDateOrderedHeader = RequiredString(h.BookDateOrdered, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.LineHaulCostDetail = RequiredDecimal(i.BookItemLineHaul, 1, false);
                            oPickRequired.CompAlphaCodeHeader = RequiredString(h.CompAlphaCode, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.ItemCostDetail = RequiredDecimal(i.ItemCost, 1, false);
                            oPickRequired.BookPickupStopNumberHeader = RequiredInteger(h.BookPickupStopNumber, 1, false);
                            oPickRequired.BookOrigCountryHeader = RequiredString(h.BookOrigCountry, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.BookStopNoHeader = RequiredInteger(h.BookStopNo, 1, false);
                            oPickRequired.BookOrigStateHeader = RequiredString(h.BookOrigState, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.BookDestCityHeader = RequiredString(h.BookDestCity, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.BookTotalBFCHeader = RequiredDecimal(h.BookTotalBFC, 1, false);
                            oPickRequired.BookSHIDHeader = RequiredString(h.BookSHID, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.BookOrigNameHeader = RequiredString(h.BookOrigName, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.PalletTypeDetail = RequiredString(i.PalletType, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.BFCDetail = RequiredDecimal(i.BFC, 1, false);
                            oPickRequired.BookOrigZipHeader = RequiredString(h.BookOrigZip, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.BookDestAddress1Header = RequiredString(h.BookDestAddress1, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.WeightDetail = RequiredDecimal(i.Weight, 1, false);
                            oPickRequired.BookOrderSequenceHeader = RequiredInteger(h.BookOrderSequence, 1, false);
                            oPickRequired.QtyOrderedDetail = RequiredInteger(i.QtyOrdered, 1, false);
                            oPickRequired.BookRouteFinalDateHeader = RequiredString(h.BookRouteFinalDate, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.BookShipCarrierProNumberHeader = RequiredString(h.BookShipCarrierProNumber, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.CarrierAlphaCodeHeader = RequiredString(h.CarrierAlphaCode, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.BookRevLoadSavingsHeader = RequiredDecimal(h.BookRevLoadSavings, 1, false);
                            oPickRequired.BookLoadPONumberHeader = RequiredString(h.BookLoadPONumber, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.BookFinServiceFeeHeader = RequiredDecimal(h.BookFinServiceFee, 1, false);
                            oPickRequired.BookDestZipHeader = RequiredString(h.BookDestZip, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.BookDateLoadHeader = RequiredString(h.BookDateLoad, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.BookDestNameHeader = RequiredString(h.BookDestName, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.BookRouteFinalCodeHeader = RequiredString(h.BookRouteFinalCode, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.BookRevTotalCostHeader = RequiredDecimal(h.BookRevTotalCost, 1, false);
                            oPickRequired.CustomerPONumberDetail = RequiredString(i.CustomerPONumber, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.BookCarrOrderNumberHeader = RequiredString(h.BookCarrOrderNumber, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.BookPickNumberHeader = RequiredInteger(h.BookPickNumber, 1, false);
                            oPickRequired.BookMilesFromHeader = RequiredString(h.BookMilesFrom, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.CarrierNumberHeader = RequiredInteger(h.CarrierNumber, 1, false);
                            oPickRequired.CompNameHeader = RequiredString(h.CompName, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.BookOrigAddress1Header = RequiredString(h.BookOrigAddress1, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.BookDestCountryHeader = RequiredString(h.BookDestCountry, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.BookTotalWgtHeader = RequiredDecimal(h.BookTotalWgt, 1, false);
                            oPickRequired.BookRevOtherCostHeader = RequiredDecimal(h.BookRevOtherCost, 1, false);
                            oPickRequired.BookFinAPGLNumberHeader = RequiredString(h.BookFinAPGLNumber, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.BookRevFreightTaxHeader = RequiredDecimal(h.BookRevFreightTax, 1, false);
                            oPickRequired.BookTotalCubeHeader = RequiredInteger(h.BookTotalCube, 1, false);
                            oPickRequired.LoadOrderHeader = RequiredInteger(h.LoadOrder, 1, false);
                            oPickRequired.BookDestStateHeader = RequiredString(h.BookDestState, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.CubeDetail = RequiredInteger(i.Cube, 1, false);
                            oPickRequired.BookTypeCodeHeader = RequiredString(h.BookTypeCode, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.TotalNonFuelFeesHeader = RequiredDecimal(h.TotalNonFuelFees, 1, false);
                            oPickRequired.BookConsPrefixHeader = RequiredString(h.BookConsPrefix, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.CommCodeDescriptionHeader = RequiredString(h.CommCodeDescription, "S-" + h.PLControl.ToString(), false);
                            oPickRequired.BookTotalCasesHeader = RequiredInteger(h.BookTotalCases, 1, false);
                            oPickRequired.FeesCostDetail = 1; //TODO:  fix mapping in Web Services
                            oPickRequired.BookProNumberHeader = RequiredString(h.BookProNumber, "S-" + h.PLControl.ToString(), false);
                            context.AddToPickListRequireds(oPickRequired);
                        }

                    }
                }
                context.SaveChanges();

                Log("Changes Saved to D365 Pick Table");
                Console.WriteLine("entity has been created ok !");
            }
            catch (Exception ex)
            {
                Log(ex.Message + ex.InnerException);
            }
        }

        public static void createPickListRequiredBackup(Resources context)
        {

            try
            {
                // Create pick list required record - line 1
                context.AddToPickListRequireds(new PickListRequired
                {
                    DataAreaId = "usmf",
                    PLControlHeader = 22,
                    BookCarrOrderNumberDetail = "BookCarrOrderNumberDetail-1",
                    OrderSequenceDetail = 1,
                    ItemNumberDetail = "Itemabcd19",
                    LotNumberDetail = "Lot1",
                    BookDoNotInvoiceHeader = NoYes.Yes,
                    FuelSurChargeHeader = 20,
                    FreightCostDetail = 100,
                    BookItemOrderNumberDetail = "BookItemOrderNumberDetail-1",
                    BookTransTypeHeader = "BookTransTypeHeader-1",
                    BookRevNetCostHeader = 1,
                    BookWhseAuthorizationNoHeader = "BookWhseAuthorizationNoHeader-1",
                    CarrierLegalEntityHeader = "CarrierLegalEntityHeader-1",
                    CarrierNameHeader = "DHL",
                    CompAlphaCodeDetail = "CompAlphaCodeDetail-1",
                    BookShipCarrierNumberHeader = "BookShipCarrierNumberHeader-1",
                    BookProNumberDetail = "BookProNumberDetail-1",
                    BookShipCarrierDetailsHeader = "BookShipCarrierDetailsHeader-1",
                    PackDetail = 10,
                    StackableDetail = NoYes.Yes,
                    BookTotalPLHeader = 1,
                    FuelCostDetail = 1,
                    BookRevNonTaxableHeader = 2,
                    BookLoadComHeader = "BookLoadComHeader-Test",
                    BookDateRequiredHeader = "1/1/2022",
                    BookRevCarrierCostHeader = 3,
                    BookShipCarrierNameHeader = "DHL",
                    BookOrigCityHeader = "City-Test",
                    PalletsDetail = 6,
                    BookDateOrderedHeader = "1/2/2022",
                    LineHaulCostDetail = 20,
                    CompAlphaCodeHeader = "CompAlphaCodeHeader-Test",
                    ItemCostDetail = 20,
                    BookPickupStopNumberHeader = 1,
                    BookOrigCountryHeader = "US",
                    BookStopNoHeader = 1,
                    BookOrigStateHeader = "CA",
                    BookDestCityHeader = "LA",
                    BookTotalBFCHeader = 1,
                    BookSHIDHeader = "BookSHIDHeader-Test",
                    BookOrigNameHeader = "BookOrigNameHeader-Test",
                    PalletTypeDetail = "PalletTypeDetail-Test",
                    BFCDetail = 1,
                    BookOrigZipHeader = "14222",
                    BookDestAddress1Header = "US",
                    WeightDetail = 300,
                    BookOrderSequenceHeader = 1,
                    QtyOrderedDetail = 15,
                    BookRouteFinalDateHeader = "1/1/2022",
                    BookShipCarrierProNumberHeader = "0001",
                    CarrierAlphaCodeHeader = "Alpha",
                    BookRevLoadSavingsHeader = 20,
                    BookLoadPONumberHeader = "PO00001",
                    BookFinServiceFeeHeader = 20,
                    BookDestZipHeader = "111",
                    BookDateLoadHeader = "1/2/2022",
                    BookDestNameHeader = "BookDestNameHeader-Test",
                    BookRouteFinalCodeHeader = "TT",
                    BookRevTotalCostHeader = 20,
                    CustomerPONumberDetail = "123",
                    BookCarrOrderNumberHeader = "1",
                    BookPickNumberHeader = 1,
                    BookMilesFromHeader = "AB",
                    CarrierNumberHeader = 1,
                    CompNameHeader = "GGG",
                    BookOrigAddress1Header = "TT",
                    BookDestCountryHeader = "US",
                    BookTotalWgtHeader = 20,
                    BookRevOtherCostHeader = 20,
                    BookFinAPGLNumberHeader = "-Test",
                    BookRevFreightTaxHeader = 10,
                    BookTotalCubeHeader = 10,
                    LoadOrderHeader = 1,
                    BookDestStateHeader = "CA",
                    CubeDetail = 10,
                    BookTypeCodeHeader = "99",
                    TotalNonFuelFeesHeader = 10,
                    BookConsPrefixHeader = "A",
                    CommCodeDescriptionHeader = "CommCodeDescriptionHeader-Desc",
                    BookTotalCasesHeader = 10,
                    FeesCostDetail = 20,
                    BookProNumberHeader = "1"


                });

                // Create pick list required record - line 2
                context.AddToPickListRequireds(new PickListRequired
                {
                    DataAreaId = "usmf",
                    PLControlHeader = 22,
                    BookCarrOrderNumberDetail = "BookCarrOrderNumberDetail-2",
                    OrderSequenceDetail = 1,
                    ItemNumberDetail = "Item1234AA19",
                    LotNumberDetail = "Lot33",
                    BookDoNotInvoiceHeader = NoYes.No,
                    FuelSurChargeHeader = 20,
                    FreightCostDetail = 100,
                    BookItemOrderNumberDetail = "BookItemOrderNumberDetail-1",
                    BookTransTypeHeader = "BookTransTypeHeader-1",
                    BookRevNetCostHeader = 1,
                    BookWhseAuthorizationNoHeader = "BookWhseAuthorizationNoHeader-1",
                    CarrierLegalEntityHeader = "CarrierLegalEntityHeader-1",
                    CarrierNameHeader = "DHL",
                    CompAlphaCodeDetail = "CompAlphaCodeDetail-1",
                    BookShipCarrierNumberHeader = "BookShipCarrierNumberHeader-1",
                    BookProNumberDetail = "BookProNumberDetail-1",
                    BookShipCarrierDetailsHeader = "BookShipCarrierDetailsHeader-1",
                    PackDetail = 10,
                    StackableDetail = NoYes.No,
                    BookTotalPLHeader = 1,
                    FuelCostDetail = 1,
                    BookRevNonTaxableHeader = 2,
                    BookLoadComHeader = "BookLoadComHeader-Test",
                    BookDateRequiredHeader = "1/1/2022",
                    BookRevCarrierCostHeader = 3,
                    BookShipCarrierNameHeader = "DHL",
                    BookOrigCityHeader = "City-Test",
                    PalletsDetail = 6,
                    BookDateOrderedHeader = "1/2/2022",
                    LineHaulCostDetail = 20,
                    CompAlphaCodeHeader = "CompAlphaCodeHeader-Test",
                    ItemCostDetail = 20,
                    BookPickupStopNumberHeader = 1,
                    BookOrigCountryHeader = "US",
                    BookStopNoHeader = 1,
                    BookOrigStateHeader = "CA",
                    BookDestCityHeader = "LA",
                    BookTotalBFCHeader = 1,
                    BookSHIDHeader = "BookSHIDHeader-Test",
                    BookOrigNameHeader = "BookOrigNameHeader-Test",
                    PalletTypeDetail = "PalletTypeDetail-Test",
                    BFCDetail = 1,
                    BookOrigZipHeader = "14222",
                    BookDestAddress1Header = "US",
                    WeightDetail = 300,
                    BookOrderSequenceHeader = 1,
                    QtyOrderedDetail = 15,
                    BookRouteFinalDateHeader = "1/1/2022",
                    BookShipCarrierProNumberHeader = "0001",
                    CarrierAlphaCodeHeader = "Alpha",
                    BookRevLoadSavingsHeader = 20,
                    BookLoadPONumberHeader = "PO00001",
                    BookFinServiceFeeHeader = 20,
                    BookDestZipHeader = "111",
                    BookDateLoadHeader = "1/2/2022",
                    BookDestNameHeader = "BookDestNameHeader-Test",
                    BookRouteFinalCodeHeader = "TT",
                    BookRevTotalCostHeader = 20,
                    CustomerPONumberDetail = "123",
                    BookCarrOrderNumberHeader = "1",
                    BookPickNumberHeader = 1,
                    BookMilesFromHeader = "AB",
                    CarrierNumberHeader = 1,
                    CompNameHeader = "GGG",
                    BookOrigAddress1Header = "TT",
                    BookDestCountryHeader = "US",
                    BookTotalWgtHeader = 20,
                    BookRevOtherCostHeader = 20,
                    BookFinAPGLNumberHeader = "-Test",
                    BookRevFreightTaxHeader = 10,
                    BookTotalCubeHeader = 10,
                    LoadOrderHeader = 1,
                    BookDestStateHeader = "CA",
                    CubeDetail = 10,
                    BookTypeCodeHeader = "99",
                    TotalNonFuelFeesHeader = 10,
                    BookConsPrefixHeader = "A",
                    CommCodeDescriptionHeader = "CommCodeDescriptionHeader-Desc",
                    BookTotalCasesHeader = 10,
                    FeesCostDetail = 20,
                    BookProNumberHeader = "1"

                });
                context.SaveChanges();

                Console.WriteLine("entity has been created ok !");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.InnerException);
            }
        }

        /// <summary>  
        /// Update pick list optional record
        /// </summary> 
        public static void updatePickListOptionalFields(Resources context)
        {

            try
            {
                //PickListOptional pickList = context.PickListOptionals.AddQueryOption("cross-company", "true").Where(x => x.DataAreaId == "USMF" && x.PLControlHeader == 14).First();
                ////PickListOptional pickList = context.PickListOptionals.AddQueryOption("cross-company", "true").AddQueryOption("$filter", "PLControlHeader eq 14 and dataAreaId eq 'USMF'").First();
                //  Where(x => x.DataAreaId == "USMF" && x.PLControlHeader == 14).First();
                PickListOptional pickList = context.PickListOptionals.Where(x => x.DataAreaId == "USMF" && x.PLControlHeader == 19).First();

                Console.WriteLine("*** DISPLAY REQUIRED ENTITY FIELDS ***");
                Console.WriteLine(JsonConvert.SerializeObject(pickList));
                Console.WriteLine("*** UPDATING OPTIONAL FIELDS ***");

                pickList.BookCarrOrderNumberDetail = "BookCarrOrderNumberD";
                pickList.OrderSequenceDetail = 1;
                pickList.ItemNumberDetail = pickList.ItemNumberDetail + "-updated";
                pickList.LotNumberDetail = "Lot1-updated00222";
                pickList.HazmatDetail = "updated000222-g1";
                pickList.BookCarrActLoadCompleteDateHeader = new DateTime(2021, 12, 31);
                pickList.BookItemRatedMarineCodeDetail = "";
                pickList.CostCenterDetail = "CostCenterDetail-updated-g2";
                pickList.LaneNumberHeader = "LaneNumberHeader-updated-g3";
                pickList.BookCarrTripNoHeader = "BookCarrTripNoHeader-updated-g4";
                pickList.BookItemDiscountDetail = 0;
                pickList.BookCarrScheduleDateHeader = new DateTime(2021, 12, 31);
                pickList.BookItemRatedIATACodeDetail = "BookItemRatedIATACodeDetail-updated-g6";
                pickList.Hazmat49CFRCodeDetail = "";
                pickList.BookOrigAddress3Header = "";
                pickList.BookRevCommCostHeader = 0;
                pickList.PLExportedHeader = NoYes.No;
                pickList.BookCarrStartUnloadingDateHeader = new DateTime(2021, 12, 31);
                pickList.DOTCodeDetail = "DOTCode-updated";
                pickList.LevelOfDensityDetail = 11;
                pickList.BookCarrScheduleTimeHeader = 22;
                pickList.BookCarrFinishUnloadingDateHeader = new DateTime(2021, 12, 31);
                pickList.PLExportRetryHeader = 0;
                pickList.SizeDetail = "Size-updated";
                pickList.BookItemRated49CFRCodeDetail = "BookItemRated49CFRCode-updated";
                pickList.BookRevGrossRevenueHeader = 350;
                pickList.BookDestAddress2Header = "BookDestAddress2-updated";
                pickList.BookCarrStartLoadingDateHeader = new DateTime(2021, 12, 31);
                pickList.LaneCommentsHeader = "LaneComments-updated";
                pickList.BookCarrActualTimeHeader = 30;
                pickList.CompLegalEntityHeader = "";
                pickList.CarrierEquipmentCodesHeader = "CarrierEquipmentCodes-updated";
                pickList.BookCarrFinishUnloadingTimeHeader = 0;
                pickList.CountryOfOrginDetail = "CountryOfOrgin-updated";
                pickList.BookAlternateAddressLaneNumberHeader = "BookAlternateAddressLaneNumber-updated";
                pickList.CompLegalEntityDetail = "CompLegalEntity-updated";
                pickList.DescriptionDetail = "Description-updated";
                pickList.LotExpirationDateDetail = "";
                pickList.BookRouteConsFlagDetail = NoYes.No;
                pickList.BookDestAddress3Header = "BookDestAddress3-updated";
                pickList.CompNatNumberHeader = 11;
                pickList.LaneLegalEntityHeader = "";
                pickList.BookCarrSealNoHeader = "BookCarrSealNo-updated";
                pickList.BookCommCompControlHeader = 0;
                pickList.CustItemNumberDetail = "CustItemNumber-updated";
                pickList.HighsDetail = 20;
                pickList.QtyHeightDetail = 30;
                pickList.BookOriginalLaneNumberHeader = "BookOriginalLaneNumber-updated";
                pickList.BookCarrApptTimeHeader = 40;
                pickList.CompNatNumberDetail = 50;
                pickList.BookAlternateAddressLaneNumberDetail = "BookAlternateAddressLaneNumber-updated";
                pickList.BookItemWeightBreakDetail = 12;
                pickList.BookItemLineHaulDetail = 2;
                pickList.QtyWidthDetail = 200;
                pickList.BookCarrierTypeCodeHeader = "BookCarrierTypeCode-updated";
                pickList.BookcarrStartUnloadingTimeHeader = 240;
                pickList.BookCarrDriverNameHeader = "BookCarrDriverName-updated";
                pickList.HSTDetail = 23;
                pickList.BookOriginalLaneLegalEntityHeader = "BookOriginalLaneLegalEntity-updated";
                pickList.BookItemRatedDOTCodeDetail = "BookItemRatedDOTCode-updated";
                pickList.BrandDetail = "Brand-updated";
                pickList.BookItemRatedFAKClassDetail = "BookItemRatedFAKClass-updated";
                pickList.BookCarrStartLoadingTimeHeader = 10;
                pickList.BookCarrFinishLoadingTimeHeader = 40;
                pickList.QtyLengthDetail = 10;
                pickList.BookFinCommStdHeader = 0;
                pickList.BookItemRatedNMFCSubClassDetail = "BookItemRatedNMFCSubClassD-updated";
                pickList.BookItemTaxesDetail = 100;
                pickList.BookItemNonTaxableFeesDetail = 100;
                pickList.FAKClassDetail = "FAKClass-updated";
                pickList.BookWarehouseNumberHeader = "BookWarehouseNumber-updated";
                pickList.BookItemTaxableFeesDetail = 100;
                pickList.CustomerNumberDetail = "CustomerNumber-updated";
                pickList.MarineCodeDetail = "MarineCode-updated";
                pickList.BookCarrFinishLoadingDateHeader = new DateTime(2021, 12, 31);
                pickList.BookCarrActUnloadCompDateHeader = new DateTime(2021, 12, 31);
                pickList.BookCarrActTimeHeader = 0;
                pickList.IATACodeDetail = "IATACodeD-updated";
                pickList.TiesDetail = 230;
                pickList.NMFCClassDetail = "NMFCClass-upddated";
                pickList.BookCarrActualDateHeader = new DateTime(2021, 12, 31);
                pickList.CompNumberHeader = "CompNumber-updated";
                pickList.BookCarrActDateHeader = new DateTime(2021, 12, 31);
                pickList.QtyPalletPercentageDetail = 5;
                pickList.BookCarrApptDateHeader = new DateTime(2021, 12, 31);
                pickList.BookCarrActUnloadCompTimeHeader = 0;
                pickList.LimitedQtyFlagDetail = NoYes.No;
                pickList.BookCarrRouteNoHeader = "BookCarrRouteNo-updated";
                pickList.BookCarrActLoadCompleteTimeHeader = 0;
                pickList.BookItemRatedNMFCClassDetail = "BookItemRatedNMFCClass-uupdated";
                pickList.PLExportDateHeader = new DateTime(2021, 12, 31);
                pickList.GTINDetail = "GTIND-updated";
                pickList.BookCarrDriverNoHeader = "BookCarrDriverNo-updated";
                pickList.BookOrigAddress2Header = "BookOrigAddress2-updated";
                pickList.BookCarrTrailerNoHeader = "BookCarrTrailerNo-updated";
                pickList.HazmatTypeCodeDetail = "HazmatTypeCode-updated";
                pickList.DescriptionDetail = "DescDetail-updated";
                pickList.CustomerNumberDetail = "0001-updated";
                context.UpdateObject(pickList);
                context.SaveChanges();
                Console.WriteLine("entity has been updated ok !");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.InnerException);
            }
        }
        /*
        /// <summary>  
        /// Update pick list optional record V2
        /// </summary> 
        private static void updatePickListOptinalFieldsV2(Resources context)
        {
            PickListOptional pickList = context.PickListOptionals.AddQueryOption("cross-company", "true").Where(x => x.DataAreaId == "USMF" && x.PLControlHeader == 2).First();
            Console.WriteLine("*** DISPLAY REQUIRED ENTITY FIELDS ***");
            Console.WriteLine(JsonConvert.SerializeObject(pickList));
            string token = OAuthHelper.GetAccessToken();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://tmsdemof305af20d6bacc62devaos.axcloud.dynamics.com");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var method = new HttpMethod("PATCH");
            var payload = new PickListOptional()
            {
                DescriptionDetail = "Update-V2"
            };

            var stringPayload = JsonConvert.SerializeObject(payload);
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(method, "/data/PickListOptionals?filter=dataAreaId eq 'usmf'")
            {
                Content = new StringContent(stringPayload, Encoding.UTF8, "application/json")
            };
            HttpResponseMessage response = client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode) //204
            {
                Console.WriteLine("entity has been updated ok !");
            }
            else
            {
                Console.WriteLine(response.StatusCode.ToString());
            }

            //var result = client.("/data/PurchaseOrderHeadersV2(dataAreaId='001',PurchaseOrderNumber='001-000234')", httpContent).Result;

        }
        */


        public static void updateCustGroup(Resources context)
        {

            try
            {
                string company = "USMF";
                CustomerGroup custGroup = context.CustomerGroups.AddQueryOption("cross-company", "true").Where(x => x.DataAreaId == "USMF" && x.CustomerGroupId == "30").First();
                Console.WriteLine("*** DISPLAY ENTITY ***");
                Console.WriteLine(JsonConvert.SerializeObject(custGroup));
                custGroup.Description = "desc-changed";

                context.UpdateObject(custGroup);
                context.SaveChanges();
                Console.WriteLine("updated ok");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.InnerException);
            }


        }

        public static void updateCustGroup2(Resources context)
        {

            try
            {
                string company = "USMF";
                CustomerGroup custGroup = context.CustomerGroups.AddQueryOption("cross-company", "true").Where(x => x.DataAreaId == "USMF" && x.CustomerGroupId == "30").First();
                Console.WriteLine("*** DISPLAY ENTITY ***");
                Console.WriteLine(JsonConvert.SerializeObject(custGroup));

                Console.WriteLine("update ok");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.InnerException);
            }


        }
    }
}
