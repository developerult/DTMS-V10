using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using DAL = Ngl.FreightMaster.Data;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using LTS = Ngl.FreightMaster.Data.LTS;
using BLL = NGL.FM.BLL;


namespace DynamicsTMS365.Controllers
{
    public class P44RateQuoteController : NGLControllerBase
    {

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.CarriersController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " REST Services"

        public Models.Response Get(int id)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            try
            {
                DAL.NGLBookData dalBookData = new DAL.NGLBookData(Parameters);
                //Not sure if this code is being use,  it uses old logic that is not supported
                //for now throw an error message.
                dalBookData.throwDepreciatedException("The P44RateQuote Get By ID is not currently supported, please check with your administrator for more information.");
                //Note: if we do use this logic later we need to validate the user authorization
                //List<Models.RateQuote> oQuotes = new List<Models.RateQuote>();
                //if (id == 0) { return new Models.Response(oQuotes.ToArray(), 0); }
                
                //DTO.Book oBook = dalBookData.GetBookFiltered(id);
                //if ((oBook == null) || (oBook.BookControl == 0)) { return new Models.Response(oQuotes.ToArray(), 0); }
                //List<DTO.BookItem> oItems = new List<DTO.BookItem>();
                //string P44WebServiceUrl = System.Configuration.ConfigurationManager.AppSettings["P44WebServiceUrl"];
                //string P44WebServiceLogin = System.Configuration.ConfigurationManager.AppSettings["P44WebServiceLogin"];
                //string P44WebServicePassword = System.Configuration.ConfigurationManager.AppSettings["P44WebServicePassword"];

                //P44.P44Proxy oP44Proxy = new P44.P44Proxy(P44WebServiceUrl, P44WebServiceLogin, P44WebServicePassword);
                //foreach (DTO.BookLoad oBookLoad in oBook.BookLoads)
                //{
                //    if (oBookLoad.BookItems != null && oBookLoad.BookItems.Count() > 0)
                //    {
                //        oItems.AddRange(oBookLoad.BookItems);
                //    }
                //}



                //var lineItemsInfo = new List<P44.rateQuoteLineImpl>();
                //lineItemsInfo.Add(new P44.rateQuoteLineImpl()
                //{
                //    freightClass = "70",
                //    weight = "1000"
                //});



                //P44.RateRequest oRequest = new P44.RateRequest()
                //{
                //    timeout = 20,
                //    shipDate = oBook.BookDateLoad.HasValue ? oBook.BookDateLoad.Value.ToShortDateString() : DateTime.Now.ToShortDateString(),
                //    deliveryDate = oBook.BookDateRequired.HasValue ? oBook.BookDateRequired.Value.ToShortDateString() : DateTime.Now.ToShortDateString(),
                //    returnMultiple = true,
                //    destination = new P44.addressInfo()
                //    {
                //        address1 = oBook.BookDestAddress1,
                //        address2 = oBook.BookDestAddress2,
                //        address3 = oBook.BookDestAddress3,
                //        city = oBook.BookDestCity,
                //        stateName = oBook.BookDestState,
                //        country = oBook.BookDestCountry,
                //        postalCode = oBook.BookDestZip,
                //        companyName = oBook.BookDestName
                //    },
                //    origin = new P44.addressInfo()
                //    {
                //        address1 = oBook.BookOrigAddress1,
                //        address2 = oBook.BookOrigAddress2,
                //        address3 = oBook.BookOrigAddress3,
                //        city = oBook.BookOrigCity,
                //        stateName = oBook.BookOrigState,
                //        country = oBook.BookOrigCountry,
                //        postalCode = oBook.BookOrigZip,
                //        companyName = oBook.BookOrigName
                //    },
                //    lineItems = (from i in oItems select new P44.rateQuoteLineImpl() { weight = i.BookItemWeight.ToString(), weightUnit = "lbs", freightClass = "70", palletCount = (int)i.BookItemPallets, numPieces = i.BookItemQtyOrdered, description = i.BookItemDescription }).ToArray()
                //};

                //List<P44.rateQuoteResponse> oResponse = oP44Proxy.GetRateQuotes(oRequest);
                //if (oResponse != null && oResponse.Count > 0)
                //{

                //    foreach (P44.rateQuoteResponse q in oResponse)
                //    {
                //        Models.RateQuote oQuote = new Models.RateQuote()
                //        {
                //            BookControl = id,
                //            Mode = oP44Proxy.translateModeToString(q.mode),
                //            SCAC = string.IsNullOrWhiteSpace(q.scac) ? q.vendor : q.scac,
                //            Vendor = string.IsNullOrWhiteSpace(q.vendor) ? q.scac : q.vendor,
                //            InterLine = q.interLine,
                //            QuoteNumber = q.quoteNumber,
                //            TotalCost = q.rateDetail.total.ToString(),
                //            TransitTime = q.transitTime.ToString(),
                //            DeliveryDate = q.deliveryDate,
                //            QuoteDate = q.quoteDate,
                //            TotalWeight = q.totalWeight.ToString(),
                //            DetailTotal = q.rateDetail.total.ToString(),
                //            DetailTransitTime = q.rateDetail.transitTime.ToString()
                //        };

                //        var sb = new System.Text.StringBuilder();
                //        string sSpacer = "";
                //        //Default for testing
                //        //oQuote.errorCode1 = "TSTCode";
                //        //oQuote.errorMessage1 = "TST MSG";
                //        //oQuote.eMessage1 = "TST Details";
                //        //oQuote.errorfieldName1 = "TST Field Name";
                //        //oQuote.ErrorCount = 1;
                //        if (q.errors != null && q.errors.Length > 0)
                //        {
                //            oQuote.ErrorCount = q.errors.Length;
                //            int errsProcessed = 0;
                //            foreach (P44.ServiceError se in q.errors)
                //            {
                //                errsProcessed++;
                //                if (errsProcessed > 5)
                //                {
                //                    sb.Append(sSpacer);
                //                    sb.Append(se.message);
                //                    sSpacer = "; AND, ";
                //                }
                //                else
                //                {
                //                    switch (errsProcessed)
                //                    {
                //                        case 1:
                //                            oQuote.errorCode1 = se.errorCode;
                //                            oQuote.errorMessage1 = se.errorMessage;
                //                            oQuote.eMessage1 = se.message;
                //                            oQuote.errorfieldName1 = se.fieldName;
                //                            break;
                //                        case 2:
                //                            oQuote.errorCode2 = se.errorCode;
                //                            oQuote.errorMessage2 = se.errorMessage;
                //                            oQuote.eMessage2 = se.message;
                //                            oQuote.errorfieldName2 = se.fieldName;
                //                            break;
                //                        case 3:
                //                            oQuote.errorCode3 = se.errorCode;
                //                            oQuote.errorMessage3 = se.errorMessage;
                //                            oQuote.eMessage3 = se.message;
                //                            oQuote.errorfieldName3 = se.fieldName;
                //                            break;
                //                        case 4:
                //                            oQuote.errorCode4 = se.errorCode;
                //                            oQuote.errorMessage4 = se.errorMessage;
                //                            oQuote.eMessage4 = se.message;
                //                            oQuote.errorfieldName4 = se.fieldName;
                //                            break;
                //                        case 5:
                //                            oQuote.errorCode5 = se.errorCode;
                //                            oQuote.errorMessage5 = se.errorMessage;
                //                            oQuote.eMessage5 = se.message;
                //                            oQuote.errorfieldName5 = se.fieldName;
                //                            break;
                //                    }
                //                }
                //            }
                //        }
                //        oQuote.Errors = sb.ToString();
                //        //sb = new  System.Text.StringBuilder();
                //        //sSpacer = "";
                //        //if(q. != null && q.warnings.Length > 0 ){
                //        //    foreach (P44.serviceWarning sw in q.warnings) {
                //        //        sb.Append(sSpacer);
                //        //         sb.Append(sw.warningMessage);
                //        //        sSpacer = "; AND, ";
                //        //    }
                //        //}

                //        string sWarnings = "";
                //        //sb = new  System.Text.StringBuilder();
                //        //sSpacer = ""; 
                //        //if(q.infos != null && q.infos.Length > 0 ){
                //        //    foreach (P44.serviceInfo si in q.infos) {
                //        //        sb.Append(sSpacer);
                //        //         sb.Append(si.infoMessage);
                //        //        sSpacer = "; AND, ";
                //        //    }
                //        //}
                //        string sInfos = "";
                //        sb = new System.Text.StringBuilder();
                //        sSpacer = "";
                //        if (q.rateDetail.rateAdjustments != null && q.rateDetail.rateAdjustments.Length > 0)
                //        {
                //            oQuote.AdjustmentCount = q.rateDetail.rateAdjustments.Length;
                //            int adjsProcessed = 0;
                //            foreach (P44.rateAdjustment a in q.rateDetail.rateAdjustments)
                //            {
                //                adjsProcessed++;
                //                if (adjsProcessed > 5)
                //                {
                //                    sb.Append(sSpacer);
                //                    sb.Append("Class: ");
                //                    sb.Append(a.freightClass);
                //                    sb.Append(" Weight: ");
                //                    sb.Append(a.weight.ToString());
                //                    sb.Append(" Description: ");
                //                    sb.Append(a.description);
                //                    sb.Append(" Code: ");
                //                    sb.Append(a.descriptionCode);
                //                    sb.Append(" Amount: ");
                //                    sb.Append(a.amount.ToString());
                //                    sb.Append(" Rate: ");
                //                    sb.Append(a.rate.ToString());
                //                    sSpacer = "; AND, ";
                //                }
                //                else
                //                {
                //                    switch (adjsProcessed)
                //                    {
                //                        case 1:
                //                            oQuote.AdjfreightClass1 = a.freightClass;
                //                            oQuote.Adjweight1 = a.weight.ToString();
                //                            oQuote.Adjdescription1 = a.description;
                //                            oQuote.AdjdescriptionCode1 = a.descriptionCode;
                //                            oQuote.Adjamount1 = a.amount.ToString();
                //                            oQuote.Adjrate1 = a.rate.ToString();
                //                            break;
                //                        case 2:
                //                            oQuote.AdjfreightClass2 = a.freightClass;
                //                            oQuote.Adjweight2 = a.weight.ToString();
                //                            oQuote.Adjdescription2 = a.description;
                //                            oQuote.AdjdescriptionCode2 = a.descriptionCode;
                //                            oQuote.Adjamount2 = a.amount.ToString();
                //                            oQuote.Adjrate2 = a.rate.ToString();
                //                            break;
                //                        case 3:
                //                            oQuote.AdjfreightClass3 = a.freightClass;
                //                            oQuote.Adjweight3 = a.weight.ToString();
                //                            oQuote.Adjdescription3 = a.description;
                //                            oQuote.AdjdescriptionCode3 = a.descriptionCode;
                //                            oQuote.Adjamount3 = a.amount.ToString();
                //                            oQuote.Adjrate3 = a.rate.ToString();
                //                            break;
                //                        case 4:
                //                            oQuote.AdjfreightClass4 = a.freightClass;
                //                            oQuote.Adjweight4 = a.weight.ToString();
                //                            oQuote.Adjdescription4 = a.description;
                //                            oQuote.AdjdescriptionCode4 = a.descriptionCode;
                //                            oQuote.Adjamount4 = a.amount.ToString();
                //                            oQuote.Adjrate4 = a.rate.ToString();
                //                            break;
                //                        case 5:
                //                            oQuote.AdjfreightClass5 = a.freightClass;
                //                            oQuote.Adjweight5 = a.weight.ToString();
                //                            oQuote.Adjdescription5 = a.description;
                //                            oQuote.AdjdescriptionCode5 = a.descriptionCode;
                //                            oQuote.Adjamount5 = a.amount.ToString();
                //                            oQuote.Adjrate5 = a.rate.ToString();
                //                            break;
                //                    }
                //                }

                //            }
                //        }
                //        oQuote.Adjustments = sb.ToString();

                //        oQuotes.Add(oQuote);
                //    }
                //}

                //int count = oQuotes.Count;

                //response = new Models.Response(oQuotes.ToArray(), count);
            }
            catch (Exception ex)
            {
                // something went wrong - possibly a database error. return a
                // 500 server error and send the details of the exception.
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Errors = string.Format("The database read failed: {0}", ex.Message);
                //response.
            }

            // return the HTTP Response.
            return response;

        }

        public Models.Response Get(string filter)
        {
            DAL.NGLBookData dalBookData = new DAL.NGLBookData(Parameters);
            //Not sure if this code is being use,  it uses old logic that is not supported
            //for now throw an error message.
            dalBookData.throwDepreciatedException("The P44RateQuote Get By Filter is not currently supported, please check with your administrator for more information.");
            //Note: if we do use this logic later we need to validate the user authorization
            return new Models.Response();
            //// create a response message to send back
            //var response = new Models.Response(); //new HttpResponseMessage();

            //DAL.Models.RateRequestOrder orders = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.RateRequestOrder>(filter);
            //return GetRateRequestOrderQuote(orders);
        }

        /// <summary>
        /// Generate a quote base on the order information provided in filter.Data; returns no records if order informaiton is missing
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.1 on 02/23/2018
        /// </remarks>
        [HttpGet, ActionName("GetRecords")]
        public Models.Response GetRecords(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (string.IsNullOrWhiteSpace(filter)){ response.populateDefaultInvalidFilterResponseMessage();return response;  }
            if (!authenticateController(ref response)) { return response; }
            try
            {
                //save the page filter for the next time the page loads
               // if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter, "RateItQuotesFilter"); }

                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                if (f == null) { response.populateDefaultInvalidFilterResponseMessage(); return response; }               
                Models.tblBid[] records = new Models.tblBid[] { };
                DAL.NGLBidData dalBidData = new DAL.NGLBidData(Parameters);
                int count = 0;
                int RecordCount = 0;
                LTS.tblBid[] oData = dalBidData.GetBids(f, ref RecordCount);
                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                    records = (from e in oData
                               orderby e.BidTotalCost ascending
                               select tblBidController.selectModelData(e)).ToArray(); 
                    if (RecordCount > count) { count = RecordCount; }
                }
                response = new Models.Response(records, count);
            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }
            return response;
        }

        [HttpGet, ActionName("GetRateRequestItems")]
        public Models.Response GetRateRequestItems(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (string.IsNullOrWhiteSpace(filter)) { response.populateDefaultInvalidFilterResponseMessage(); return response; }
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                if (f == null) { response.populateDefaultInvalidFilterResponseMessage(); return response; }                
                LTS.vRateRequestItem[] records = new LTS.vRateRequestItem[] { };
                DAL.NGLLoadTenderData dalData = new DAL.NGLLoadTenderData(Parameters);
                int count = 0;
                int RecordCount = 0;
                records = dalData.GetRateRequestItems(f, ref RecordCount);
                if (records != null && records.Count() > 0)
                {
                    count = records.Count();                    
                    if (RecordCount > count) { count = RecordCount; }
                }
                response = new Models.Response(records, count);
            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }
            return response;
        }

        [HttpPost, ActionName("PostSave")]
        public Models.Response PostSave([System.Web.Http.FromBody]DAL.Models.AllFilters filter)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            if (filter == null) { response.populateDefaultInvalidFilterResponseMessage(); return response; }
            try
            {
                string sData = filter.Data;
                //if no order info return no quotes
                if (string.IsNullOrWhiteSpace(sData)) { return response; }
                DAL.Models.RateRequestOrder order = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.RateRequestOrder>(sData);
                //if no order info return no quotes
                if (order == null) { return response; }
                return SaveQuote(order);              
            }
            catch (Exception ex)           
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }
        }

        /// <summary>
        /// processes order data and returns the LoadTenderControl for the bid/quote as the first item in the results array.  
        /// Caller must use the non-zero value to load the results in the grid by setting the Load
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified By RHR for v-8.1 on 02/23/2018
        ///   this rest service was changed to return the LoadTenderControl in an array of integers 
        ///   this generally called directly by an ajax method which will call the read method of the
        ///   Grid Data assigning the LoadTenderControl to the filter object for the tblBid table (AKA Quote results)
        ///   generated by the PostRateRequest procedure via
        /// </remarks>
        [HttpPost, ActionName("PostRateRequest")]
        public Models.Response PostRateRequest([System.Web.Http.FromBody]DAL.Models.RateRequestOrder order)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            if (order == null) { response.populateDefaultInvalidFilterResponseMessage(); return response; }
            try
            {                
                return SaveQuote(order);
            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }
        }

        #endregion

        #region " Private Methods"

        //private bool createP44Proxy(ref Models.Response response, ref P44.P44Proxy oP44Proxy, ref string P44AccountGroup)
        //{
            
        //    string P44WebServiceUrl = "";
        //    string P44WebServiceLogin = "";
        //    string P44WebServicePassword = "";
        //    if (!readSSOASettings(ref response, ref P44WebServiceUrl, ref P44WebServiceLogin, ref P44WebServicePassword, ref P44AccountGroup)) { return false; }
        //    oP44Proxy = new P44.P44Proxy(P44WebServiceUrl, P44WebServiceLogin, P44WebServicePassword);
        //    if (oP44Proxy == null)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}

        // private bool readSSOASettings(ref Models.Response response, ref string P44WebServiceUrl, ref string P44WebServiceLogin,ref string P44WebServicePassword, ref string P44AccountGroup)
        //{
        //    bool blnRet = false;
        //    DAL.NGLtblSingleSignOnAccountData oSec = new DAL.NGLtblSingleSignOnAccountData(Parameters);
           
        //    DTO.WCFResults[] SSOA = oSec.GetSingleSignOnAccountByUser(Parameters.UserControl, DAL.Utilities.SSOAAccount.P44);
        //    if (SSOA?.Length > 0)
        //    {
        //        if (SSOA[0].Warnings?.Count > 0)
        //        {
        //            FaultExceptionEventArgs fault = new FaultExceptionEventArgs();
        //            response.Errors = fault.formatMessage("E_NotAuthProcedure", SSOA[0].concatWarnings(),null);
        //            response.StatusCode = HttpStatusCode.Unauthorized;
        //            return false;
        //        }
        //        if (SSOA[0].KeyFields?.Count > 0)
        //        {
        //            P44WebServiceUrl = SSOA[0].KeyFields["SSOALoginURL"];
        //            P44WebServiceLogin = SSOA[0].KeyFields["Username"];
        //            P44WebServicePassword = SSOA[0].KeyFields["Pass"];
        //            P44AccountGroup = SSOA[0].KeyFields["RefID"];
        //            return true;
        //        }
        //    } else
        //    {
        //        response.populateDefaultUnathorizedResponseMessage();                
        //        return false;
        //    }
        //    return blnRet;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v8.5.4.005 on 02/06/2024
        ///   for backward compatibility and compatibility with
        ///   Booking Order processing we must convert Tariff Temp codes back
        ///   to standard temp codes so the DoSpotRate logic works
        /// </remarks>
        private Models.Response SaveQuote(DAL.Models.RateRequestOrder order)
        {
            var response = new Models.Response();                   
            try
            {
                BLL.NGLBookRevenueBLL oBookRev = new BLL.NGLBookRevenueBLL(Parameters);
                
                DTO.tblLoadTender.LoadTenderTypeEnum[] TenderTypes = { DTO.tblLoadTender.LoadTenderTypeEnum.RateShopping };
                DTO.tblLoadTender.BidTypeEnum[] bidTypes = { DTO.tblLoadTender.BidTypeEnum.P44, DTO.tblLoadTender.BidTypeEnum.NGLTariff };
                //switch (order.TariffTempType)
                //{
                   
                //    case 1:
                //        order.TariffTempType = 3; // dry
                //        break;
                //    case 2:
                //        order.TariffTempType = 1; // frozen
                //        break;
                //    case 3:
                //        order.TariffTempType = 2; // reefer
                //        break;
                //    default:
                //        order.TariffTempType = 0; // any
                //        break;
                //}
                DTO.WCFResults result = oBookRev.GenerateQuote(order, TenderTypes, bidTypes, 0);
                int iLoadTenderControl = 0;
                // Modified by RHR for v-8.2 01/01/2019 to simplify reading of WCFResults keys 
                result.TryParseKeyInt("LoadTenderControl", ref iLoadTenderControl);
                response.Data = new int[] { iLoadTenderControl };
                response.Count = 1;
               
                //Deprecated in v-8.3.0.002 we now use addWCFMessagesToResponse and the NGLErrWarnMsgLogCtrl widget
                //check for errors messages and warning.
                //response.Errors = Utilities.formatWCFResultErrors(result);
                // response.Warnings = Utilities.formatWCFResultWarnings(result);
                // response.Messages = Utilities.formatWCFResultMessages(result);
                response.StatusCode = HttpStatusCode.OK;

                if (result != null)
                {
                    response.AsyncMessagesPossible = result.isAsyncMsgPossible();
                    response.AsyncMessageKey = result.getAsyncMessageKey();
                    response.AsyncTypeKey = result.getAsyncTypeKey();
                    Utilities.addWCFMessagesToResponse(ref response, ref result, "Save Quote");
                }

            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }
            return response;
        }

        //private void fillRateRequestItems(DAL.Models.RateRequestOrder order, ref List<DAL.Models.RateRequestItem> oItems)
        //{
        //    if (order == null || order.Stops == null || order.Stops.Count() < 1) { return; }
        //    if (oItems == null) { oItems = new List<DAL.Models.RateRequestItem>(); }
        //    foreach (DAL.Models.RateRequestStop oStop in order.Stops)
        //    {
        //        if (oStop.Items != null && oStop.Items.Count() > 0)
        //        {
        //            oItems.AddRange(oStop.Items);
        //        }
        //    }
        //    //populate defaults 
        //    if (oItems == null || oItems.Count() < 1 || oItems[0] == null)
        //    {
        //        oItems = new List<DAL.Models.RateRequestItem>();
        //        DAL.Models.RateRequestItem item = new DAL.Models.RateRequestItem()
        //        {
        //            FreightClass = "100",
        //            ItemNumber = "NA",
        //            Weight = 500,
        //            PalletCount = 1,
        //            Quantity = "1"
        //        };
        //        oItems.Add(item);

        //    }
        //}

        /// <summary>
        /// Generates a P44.RateRequest object used to produce the xml document
        /// </summary>
        /// <param name="order"></param>
        /// <param name="oItems"></param>
        /// <returns></returns>
        /// <remarks>
        /// NOTE!!!:   changes made in this function to the oRequest object may not work as expected
        ///     See the P44Proxy.buildXMLOrderWStream function for how the RateRequest is converted to XML
        ///     many values like fetchAllGuaranteed and returnMultiple are hard coded in buildXMLOrderWStream
        /// </remarks>
        //private P44.RateRequest getRateRequest(DAL.Models.RateRequestOrder order, ref List<Models.RateRequestItem> oItems, ref string P44AccountGroup)
        //{
        //    DateTime dtShipDate = DateTime.Now;
        //    DateTime.TryParse(order.ShipDate, out dtShipDate);
        //    DateTime dtdeliveryDate = DateTime.Now.AddDays(1);
        //    DateTime.TryParse(order.DeliveryDate, out dtdeliveryDate);

        //    P44.RateRequest oRequest = new P44.RateRequest()
        //    {
        //        fetchAllGuaranteed = true,  //not used here
        //        timeout = 10,
        //        shipDate = dtShipDate.ToShortDateString(),
        //        deliveryDate = dtdeliveryDate.ToShortDateString(),
        //        returnMultiple = true,  //not used here
        //        loginGroupKey = P44AccountGroup,
        //        destination = new P44.addressInfo()
        //        {
        //            address1 = order.Stops[0].CompAddress1,
        //            address2 = order.Stops[0].CompAddress2,
        //            address3 = order.Stops[0].CompAddress3,
        //            city = order.Stops[0].CompCity,
        //            stateName = order.Stops[0].CompState,
        //            country = order.Stops[0].CompCountry,
        //            postalCode = order.Stops[0].CompPostalCode,
        //            companyName = order.Stops[0].CompName
        //        },
        //        origin = new P44.addressInfo()
        //        {
        //            address1 = order.Pickup.CompAddress1,
        //            address2 = order.Pickup.CompAddress2,
        //            address3 = order.Pickup.CompAddress3,
        //            city = order.Pickup.CompCity,
        //            stateName = order.Pickup.CompState,
        //            country = order.Pickup.CompCountry,
        //            postalCode = order.Pickup.CompPostalCode,
        //            companyName = order.Pickup.CompName
        //        },
        //        accessorials = order.Accessorials, //LVV ADD
        //        lineItems = (from i in oItems select new P44.rateQuoteLineImpl() { weight = i.Weight.ToString(), weightUnit = i.WeightUnit, freightClass = i.FreightClass, palletCount = i.PalletCount, numPieces = i.NumPieces, description = i.Description, length = i.Length, width = i.Width, height = i.Height, packageType = i.PackageType, nmfcItem = i.NMFCItem, nmfcSub = i.NMFCSub, stackable = i.Stackable }).ToArray()
        //    };

        //    return oRequest;
        //}

        //private void fillBids(ref Models.tblBid[] nsBids, ref List<P44.rateQuoteResponse> oResponse, ref P44.RateRequest oRequest)
        //{
        //    if (nsBids == null) { nsBids = new Models.tblBid[] { }; }
        //    if (oResponse != null && oResponse.Count > 0)
        //    {
        //        DAL.NGLLoadTenderData dalLTData = new DAL.NGLLoadTenderData(Parameters);
        //        //TODO:  map to correct or default SHID, SO, and CNS numbers
        //        LTS.tblBid[] ltsBids = dalLTData.insertLoadBoardQuote(oRequest, oResponse, "SHID", "SO123", 0, "CNS");
        //        if (ltsBids != null && ltsBids.Count() > 0)
        //        {
        //            nsBids = (from e in ltsBids
        //                      orderby e.BidTotalCost ascending
        //                      select tblBidController.selectModelData(e)).ToArray();
        //        }


        //    }
        //}

        private Models.Response GetRateRequestOrderQuote(DAL.Models.RateRequestOrder order)
        {

            var response = new Models.Response();
            DAL.NGLBookData dalBookData = new DAL.NGLBookData(Parameters);
            //Not sure if this code is being use,  it uses old logic that is not supported
            //for now throw an error message.
            dalBookData.throwDepreciatedException("The P44RateQuote GGetRateRequestOrderQuote is not currently supported, please check with your administrator for more information.");
            return response;
            ////DAL.Models.RateRequestOrder rorder = (DAL.Models.RateRequestOrder)request["order"];
            //string origName = string.IsNullOrEmpty(request["CompName"]) ? "Empty" : request["CompName"];
            //string name = order.CompName;
            //try
            //{
            //    List<Models.RateQuote> oQuotes = new List<Models.RateQuote>();                
            //    if (string.IsNullOrWhiteSpace(origName)) { return new Models.Response(oQuotes.ToArray(), 0); }
            //    List<DAL.Models.RateRequestItem> oItems = new List<DAL.Models.RateRequestItem>();
            //    string P44WebServiceUrl = System.Configuration.ConfigurationManager.AppSettings["P44WebServiceUrl"];
            //    string P44WebServiceLogin = System.Configuration.ConfigurationManager.AppSettings["P44WebServiceLogin"];
            //    string P44WebServicePassword = System.Configuration.ConfigurationManager.AppSettings["P44WebServicePassword"];

            //    P44.P44Proxy oP44Proxy = new P44.P44Proxy(P44WebServiceUrl, P44WebServiceLogin, P44WebServicePassword);
            //    foreach (DAL.Models.RateRequestStop oStop in order.Stops)
            //    {
            //        if (oStop.Items != null && oStop.Items.Count() > 0)
            //        {
            //            oItems.AddRange(oStop.Items);
            //        }
            //    }


            //    var lineItemsInfo = new List<P44.rateQuoteLineImpl>();
            //    lineItemsInfo.Add(new P44.rateQuoteLineImpl()
            //    {
            //        freightClass = "70",
            //        weight = "1000"
            //    });


            //    DateTime dtShipDate = DateTime.Now;
            //    DateTime.TryParse(order.ShipDate, out dtShipDate);
            //    DateTime dtdeliveryDate = DateTime.Now.AddDays(1);
            //    DateTime.TryParse(order.DeliveryDate, out dtdeliveryDate);
            //    P44.RateRequest oRequest = new P44.RateRequest()
            //    {
            //        timeout = 20,
            //        shipDate = dtShipDate.ToShortDateString(),
            //        deliveryDate = dtdeliveryDate.ToShortDateString(),
            //        returnMultiple = true,
            //        destination = new P44.addressInfo()
            //        {
            //            address1 = order.Stops[0].CompAddress1,
            //            address2 = order.Stops[0].CompAddress2,
            //            address3 = order.Stops[0].CompAddress3,
            //            city = order.Stops[0].CompCity,
            //            stateName = order.Stops[0].CompState,
            //            country = order.Stops[0].CompCountry,
            //            postalCode = order.Stops[0].CompPostalCode,
            //            companyName = order.Stops[0].CompName
            //        },
            //        origin = new P44.addressInfo()
            //        {
            //            address1 = order.Pickup.CompAddress1,
            //            address2 = order.Pickup.CompAddress2,
            //            address3 = order.Pickup.CompAddress3,
            //            city = order.Pickup.CompCity,
            //            stateName = order.Pickup.CompState,
            //            country = order.Pickup.CompCountry,
            //            postalCode = order.Pickup.CompPostalCode,
            //            companyName = order.Pickup.CompName
            //        },
            //        lineItems = (from i in oItems select new P44.rateQuoteLineImpl() { weight = i.Weight.ToString(), weightUnit = i.WeightUnit, freightClass = i.FreightClass, palletCount = i.PalletCount, numPieces = i.NumPieces, description = i.Description, length = i.Length, width = i.Width, height = i.Height, packageType = i.PackageType, nmfcItem = i.NMFCItem, nmfcSub = i.NMFCSub, stackable = i.Stackable }).ToArray()
            //    };

            //    List<P44.rateQuoteResponse> oResponse = oP44Proxy.GetRateQuotes(oRequest);
            //    if (oResponse != null && oResponse.Count > 0)
            //    {
            //        foreach (P44.rateQuoteResponse q in oResponse)
            //        {
            //            Models.RateQuote oQuote = new Models.RateQuote()
            //            {
            //                BookControl = order.ID,
            //                Mode = oP44Proxy.translateModeToString(q.mode),
            //                SCAC = string.IsNullOrWhiteSpace(q.scac) ? q.vendor : q.scac,
            //                Vendor = string.IsNullOrWhiteSpace(q.vendor) ? q.scac : q.vendor,
            //                InterLine = q.interLine,
            //                QuoteNumber = q.quoteNumber,
            //                TotalCost = q.rateDetail.total.ToString(),
            //                TransitTime = q.transitTime.ToString(),
            //                DeliveryDate = q.deliveryDate,
            //                QuoteDate = q.quoteDate,
            //                TotalWeight = q.totalWeight.ToString(),
            //                DetailTotal = q.rateDetail.total.ToString(),
            //                DetailTransitTime = q.rateDetail.transitTime.ToString()
            //            };

            //            var sb = new System.Text.StringBuilder();
            //            string sSpacer = "";
            //            //Default for testing
            //            //oQuote.errorCode1 = "TSTCode";
            //            //oQuote.errorMessage1 = "TST MSG";
            //            //oQuote.eMessage1 = "TST Details";
            //            //oQuote.errorfieldName1 = "TST Field Name";
            //            //oQuote.ErrorCount = 1;
            //            if (q.errors != null && q.errors.Length > 0)
            //            {
            //                oQuote.ErrorCount = q.errors.Length;
            //                int errsProcessed = 0;
            //                foreach (P44.ServiceError se in q.errors)
            //                {
            //                    errsProcessed++;
            //                    if (errsProcessed > 5)
            //                    {
            //                        sb.Append(sSpacer);
            //                        sb.Append(se.message);
            //                        sSpacer = "; AND, ";
            //                    }
            //                    else
            //                    {
            //                        switch (errsProcessed)
            //                        {
            //                            case 1:
            //                                oQuote.errorCode1 = se.errorCode;
            //                                oQuote.errorMessage1 = se.errorMessage;
            //                                oQuote.eMessage1 = se.message;
            //                                oQuote.errorfieldName1 = se.fieldName;
            //                                break;
            //                            case 2:
            //                                oQuote.errorCode2 = se.errorCode;
            //                                oQuote.errorMessage2 = se.errorMessage;
            //                                oQuote.eMessage2 = se.message;
            //                                oQuote.errorfieldName2 = se.fieldName;
            //                                break;
            //                            case 3:
            //                                oQuote.errorCode3 = se.errorCode;
            //                                oQuote.errorMessage3 = se.errorMessage;
            //                                oQuote.eMessage3 = se.message;
            //                                oQuote.errorfieldName3 = se.fieldName;
            //                                break;
            //                            case 4:
            //                                oQuote.errorCode4 = se.errorCode;
            //                                oQuote.errorMessage4 = se.errorMessage;
            //                                oQuote.eMessage4 = se.message;
            //                                oQuote.errorfieldName4 = se.fieldName;
            //                                break;
            //                            case 5:
            //                                oQuote.errorCode5 = se.errorCode;
            //                                oQuote.errorMessage5 = se.errorMessage;
            //                                oQuote.eMessage5 = se.message;
            //                                oQuote.errorfieldName5 = se.fieldName;
            //                                break;
            //                        }
            //                    }
            //                }
            //            }
            //            oQuote.Errors = sb.ToString();
            //            //sb = new  System.Text.StringBuilder();
            //            //sSpacer = "";
            //            //if(q. != null && q.warnings.Length > 0 ){
            //            //    foreach (P44.serviceWarning sw in q.warnings) {
            //            //        sb.Append(sSpacer);
            //            //         sb.Append(sw.warningMessage);
            //            //        sSpacer = "; AND, ";
            //            //    }
            //            //}

            //            string sWarnings = "";
            //            //sb = new  System.Text.StringBuilder();
            //            //sSpacer = ""; 
            //            //if(q.infos != null && q.infos.Length > 0 ){
            //            //    foreach (P44.serviceInfo si in q.infos) {
            //            //        sb.Append(sSpacer);
            //            //         sb.Append(si.infoMessage);
            //            //        sSpacer = "; AND, ";
            //            //    }
            //            //}
            //            string sInfos = "";
            //            sb = new System.Text.StringBuilder();
            //            sSpacer = "";
            //            if (q.rateDetail.rateAdjustments != null && q.rateDetail.rateAdjustments.Length > 0)
            //            {
            //                oQuote.AdjustmentCount = q.rateDetail.rateAdjustments.Length;
            //                int adjsProcessed = 0;
            //                foreach (P44.rateAdjustment a in q.rateDetail.rateAdjustments)
            //                {
            //                    adjsProcessed++;
            //                    if (adjsProcessed > 5)
            //                    {
            //                        sb.Append(sSpacer);
            //                        sb.Append("Class: ");
            //                        sb.Append(a.freightClass);
            //                        sb.Append(" Weight: ");
            //                        sb.Append(a.weight.ToString());
            //                        sb.Append(" Description: ");
            //                        sb.Append(a.description);
            //                        sb.Append(" Code: ");
            //                        sb.Append(a.descriptionCode);
            //                        sb.Append(" Amount: ");
            //                        sb.Append(a.amount.ToString());
            //                        sb.Append(" Rate: ");
            //                        sb.Append(a.rate.ToString());
            //                        sSpacer = "; AND, ";
            //                    }
            //                    else
            //                    {
            //                        switch (adjsProcessed)
            //                        {
            //                            case 1:
            //                                oQuote.AdjfreightClass1 = a.freightClass;
            //                                oQuote.Adjweight1 = a.weight.ToString();
            //                                oQuote.Adjdescription1 = a.description;
            //                                oQuote.AdjdescriptionCode1 = a.descriptionCode;
            //                                oQuote.Adjamount1 = a.amount.ToString();
            //                                oQuote.Adjrate1 = a.rate.ToString();
            //                                break;
            //                            case 2:
            //                                oQuote.AdjfreightClass2 = a.freightClass;
            //                                oQuote.Adjweight2 = a.weight.ToString();
            //                                oQuote.Adjdescription2 = a.description;
            //                                oQuote.AdjdescriptionCode2 = a.descriptionCode;
            //                                oQuote.Adjamount2 = a.amount.ToString();
            //                                oQuote.Adjrate2 = a.rate.ToString();
            //                                break;
            //                            case 3:
            //                                oQuote.AdjfreightClass3 = a.freightClass;
            //                                oQuote.Adjweight3 = a.weight.ToString();
            //                                oQuote.Adjdescription3 = a.description;
            //                                oQuote.AdjdescriptionCode3 = a.descriptionCode;
            //                                oQuote.Adjamount3 = a.amount.ToString();
            //                                oQuote.Adjrate3 = a.rate.ToString();
            //                                break;
            //                            case 4:
            //                                oQuote.AdjfreightClass4 = a.freightClass;
            //                                oQuote.Adjweight4 = a.weight.ToString();
            //                                oQuote.Adjdescription4 = a.description;
            //                                oQuote.AdjdescriptionCode4 = a.descriptionCode;
            //                                oQuote.Adjamount4 = a.amount.ToString();
            //                                oQuote.Adjrate4 = a.rate.ToString();
            //                                break;
            //                            case 5:
            //                                oQuote.AdjfreightClass5 = a.freightClass;
            //                                oQuote.Adjweight5 = a.weight.ToString();
            //                                oQuote.Adjdescription5 = a.description;
            //                                oQuote.AdjdescriptionCode5 = a.descriptionCode;
            //                                oQuote.Adjamount5 = a.amount.ToString();
            //                                oQuote.Adjrate5 = a.rate.ToString();
            //                                break;
            //                        }
            //                    }

            //                }
            //            }
            //            oQuote.Adjustments = sb.ToString();

            //            oQuotes.Add(oQuote);
            //        }
            //    }

            //    int count = oQuotes.Count;

            //    response = new Models.Response(oQuotes.ToArray(), count);
            //}
            //catch (Exception ex)
            //{
            //    // something went wrong - possibly a database error. return a
            //    // 500 server error and send the details of the exception.
            //    response.StatusCode = HttpStatusCode.InternalServerError;
            //    response.Errors = string.Format("The database read failed: {0}", ex.Message);
            //    //response.
            //}

            //// return the HTTP Response.
            //return response;
        }

        //public Models.Response Post(int id)
        //{
        //    // create a response message to send back
        //    var response = new Models.Response(); //new HttpResponseMessage();
        //    try
        //    {
        //        DTO.Carrier dtocarrier = NGLCarrierData.GetCarrier(id);


        //        // if there was an carrier returned from the database
        //        if (dtocarrier != null)
        //        {

        //            // update the carrier object handling null values or empty strings
        //            dtocarrier.CarrierName = string.IsNullOrEmpty(request["Name"]) ? dtocarrier.CarrierName : request["Name"];
        //            dtocarrier.CarrierNumber = string.IsNullOrEmpty(request["Number"]) ? dtocarrier.CarrierNumber : int.Parse(request["Number"]);
        //            dtocarrier.CarrierSCAC = string.IsNullOrEmpty(request["CarrierSCAC"]) ? dtocarrier.CarrierSCAC : request["CarrierSCAC"];
        //            dtocarrier.CarrierModDate = string.IsNullOrEmpty(request["CarrierModDate"]) ? dtocarrier.CarrierModDate : Convert.ToDateTime(request["CarrierModDate"]);
        //            NGLCarrierData.UpdateRecordNoReturn(dtocarrier);
        //            // set the server response to OK
        //            response.StatusCode = HttpStatusCode.OK;
        //        }
        //        else
        //        {
        //            // we couldn't find the carrier with the passed in id
        //            // set the response status to error and return a message
        //            // with some more info.
        //            response.StatusCode = HttpStatusCode.InternalServerError;
        //            response.Errors = string.Format("The carrier with control number {0} was not found in the database", id.ToString());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // something went wrong - possibly a database error. return a
        //        // 500 server error and send the details of the exception.
        //        response.StatusCode = HttpStatusCode.InternalServerError;
        //        response.Errors = string.Format("The database updated failed: {0}", ex.Message);
        //        //response.
        //    }

        //    // return the HTTP Response.
        //    return response;
        //}

        public Models.Response Delete(int id)
        {
            try
            {

                DTO.Carrier carrierToDelete = NGLCarrierData.GetCarrier(id);

                int? Number = carrierToDelete.CarrierNumber;
                // delete the employee from the context
                //_context.Employees.DeleteOnSubmit(employeeToDelete);

                // submit the changes
                //_context.SubmitChanges();

                // if a valid employee object was found by id
                if (carrierToDelete != null)
                {
                    return new Models.Response(string.Format("You are not authorized to delete carrier data", id.ToString()));
                    //// mark the object for deletion
                    //_context.Carriers.DeleteOnSubmit(carrierToDelete);
                    //// delete the object from the database
                    //_context.SubmitChanges();

                    //// return an empty Models.Response object (this returns a 200 OK)
                    //return new Models.Response();
                }
                else
                {
                    // otherwise set the error field of a response object and return it.
                    return new Models.Response(string.Format("The carrier with control number {0} was not found in the database", id.ToString()));
                }
            }
            catch (Exception ex)
            {
                // something went wrong. set the errors field of
                return new Models.Response(string.Format("There was an error updating carrier with control number {0}: {1}", id.ToString(), ex.Message));
            }

        }

        //private P44M.Address.CountryEnum getCountryEnum(string s)
        //{
        //    var strCountry = s.Trim().ToUpper();

        //    //Set return value to US by default
        //    P44M.Address.CountryEnum retVal = P44M.Address.CountryEnum.US;
           
        //    //If country is null return US by default
        //    if (string.IsNullOrWhiteSpace(strCountry)) { return retVal; }

        //    if (strCountry == "US" || strCountry == "USA") { retVal = P44M.Address.CountryEnum.US; }

        //    if (strCountry == "CA" || strCountry == "CAN") { retVal = P44M.Address.CountryEnum.CA; }

        //    if (strCountry == "MX" || strCountry == "MEX") { retVal = P44M.Address.CountryEnum.MX; }

        //    return retVal;
        //}

        //private P44M.LineItem.FreightClassEnum getFreightClassEnum(string s)
        //{
        //    //Set return value to 50 by default
        //    P44M.LineItem.FreightClassEnum retVal = P44M.LineItem.FreightClassEnum._50;

        //    switch (s)
        //    {
        //        case "50":
        //            retVal = P44M.LineItem.FreightClassEnum._50;
        //            break;
        //        case "55":
        //            retVal = P44M.LineItem.FreightClassEnum._55;
        //            break;
        //        case "60":
        //            retVal = P44M.LineItem.FreightClassEnum._60;
        //            break;
        //        case "65":
        //            retVal = P44M.LineItem.FreightClassEnum._65;
        //            break;
        //        case "70":
        //            retVal = P44M.LineItem.FreightClassEnum._70;
        //            break;
        //        case "77.5":
        //            retVal = P44M.LineItem.FreightClassEnum._775;
        //            break;
        //        case "85":
        //            retVal = P44M.LineItem.FreightClassEnum._85;
        //            break;
        //        case "92.5":
        //            retVal = P44M.LineItem.FreightClassEnum._925;
        //            break;
        //        case "100":
        //            retVal = P44M.LineItem.FreightClassEnum._100;
        //            break;
        //        case "110":
        //            retVal = P44M.LineItem.FreightClassEnum._110;
        //            break;
        //        case "125":
        //            retVal = P44M.LineItem.FreightClassEnum._125;
        //            break;
        //        case "150":
        //            retVal = P44M.LineItem.FreightClassEnum._150;
        //            break;
        //        case "175":
        //            retVal = P44M.LineItem.FreightClassEnum._175;
        //            break;
        //        case "200":
        //            retVal = P44M.LineItem.FreightClassEnum._200;
        //            break;
        //        case "250":
        //            retVal = P44M.LineItem.FreightClassEnum._250;
        //            break;
        //        case "300":
        //            retVal = P44M.LineItem.FreightClassEnum._300;
        //            break;
        //        case "400":
        //            retVal = P44M.LineItem.FreightClassEnum._400;
        //            break;
        //        case "500":
        //            retVal = P44M.LineItem.FreightClassEnum._500;
        //            break;
        //    }

        //    return retVal;
        //}

        //private P44M.LineItem.PackageTypeEnum getPackageTypeEnum(string s)
        //{
        //    //Set return value to PLT by default
        //    P44M.LineItem.PackageTypeEnum retVal = P44M.LineItem.PackageTypeEnum.PLT;

        //    var strPackage = s.Trim().ToUpper();

        //    switch (strPackage)
        //    {
        //        case "PLT":
        //            retVal = P44M.LineItem.PackageTypeEnum.PLT;
        //            break;
        //        case "BAG":
        //            retVal = P44M.LineItem.PackageTypeEnum.BAG;
        //            break;
        //        case "BALE":
        //            retVal = P44M.LineItem.PackageTypeEnum.BALE;
        //            break;
        //        case "BOX":
        //            retVal = P44M.LineItem.PackageTypeEnum.BOX;
        //            break;
        //        case "BUCKET":
        //            retVal = P44M.LineItem.PackageTypeEnum.BUCKET;
        //            break;
        //        case "PAIL":
        //            retVal = P44M.LineItem.PackageTypeEnum.PAIL;
        //            break;
        //        case "BUNDLE":
        //            retVal = P44M.LineItem.PackageTypeEnum.BUNDLE;
        //            break;
        //        case "CAN":
        //            retVal = P44M.LineItem.PackageTypeEnum.CAN;
        //            break;
        //        case "CARTON":
        //            retVal = P44M.LineItem.PackageTypeEnum.CARTON;
        //            break;
        //        case "CASE":
        //            retVal = P44M.LineItem.PackageTypeEnum.CASE;
        //            break;
        //        case "COIL":
        //            retVal = P44M.LineItem.PackageTypeEnum.COIL;
        //            break;
        //        case "CRATE":
        //            retVal = P44M.LineItem.PackageTypeEnum.CRATE;
        //            break;
        //        case "CYLINDER":
        //            retVal = P44M.LineItem.PackageTypeEnum.CYLINDER;
        //            break;
        //        case "DRUM":
        //            retVal = P44M.LineItem.PackageTypeEnum.DRUM;
        //            break;
        //        case "PIECES":
        //            retVal = P44M.LineItem.PackageTypeEnum.PIECES;
        //            break;
        //        case "REEL":
        //            retVal = P44M.LineItem.PackageTypeEnum.REEL;
        //            break;
        //        case "ROLL":
        //            retVal = P44M.LineItem.PackageTypeEnum.ROLL;
        //            break;
        //        case "SKID":
        //            retVal = P44M.LineItem.PackageTypeEnum.SKID;
        //            break;
        //        case "TUBE":
        //            retVal = P44M.LineItem.PackageTypeEnum.TUBE;
        //            break;
        //    }

        //    return retVal;           
        //}

        //private P44M.LineItemHazmatDetail.PackingGroupEnum getPackingGroupEnum(string s)
        //{
        //    //Set return value to I by default
        //    P44M.LineItemHazmatDetail.PackingGroupEnum retVal = P44M.LineItemHazmatDetail.PackingGroupEnum.I;

        //    var strPacking = s.Trim().ToUpper();

        //    switch (strPacking)
        //    {
        //        case "I":
        //            retVal = P44M.LineItemHazmatDetail.PackingGroupEnum.I;
        //            break;
        //        case "II":
        //            retVal = P44M.LineItemHazmatDetail.PackingGroupEnum.II;
        //            break;
        //        case "III":
        //            retVal = P44M.LineItemHazmatDetail.PackingGroupEnum.III;
        //            break;
        //    }

        //    return retVal;
        //}

        //private P44M.Shipment.PaymentTermsOverrideEnum getPaymentTermsOverrideEnum(string s)
        //{
        //    //Set return value to PREPAID by default
        //    P44M.Shipment.PaymentTermsOverrideEnum retVal = P44M.Shipment.PaymentTermsOverrideEnum.PREPAID;

        //    var strPayment = s.Trim().ToUpper();

        //    switch (strPayment)
        //    {
        //        case "PREPAID":
        //            retVal = P44M.Shipment.PaymentTermsOverrideEnum.PREPAID;
        //            break;
        //        case "COLLECT":
        //            retVal = P44M.Shipment.PaymentTermsOverrideEnum.COLLECT;
        //            break;
        //        case "THIRDPARTY":
        //            retVal = P44M.Shipment.PaymentTermsOverrideEnum.THIRDPARTY;
        //            break;
        //    }

        //    return retVal;
        //}

        //private P44M.Shipment.DirectionOverrideEnum getDirectionOverrideEnum(string s)
        //{
        //    //Set return value to THIRDPARTY by default
        //    P44M.Shipment.DirectionOverrideEnum retVal = P44M.Shipment.DirectionOverrideEnum.THIRDPARTY;

        //    var strDirection = s.Trim().ToUpper();

        //    switch (strDirection)
        //    {
        //        case "THIRDPARTY":
        //            retVal = P44M.Shipment.DirectionOverrideEnum.THIRDPARTY;
        //            break;
        //        case "SHIPPER":
        //            retVal = P44M.Shipment.DirectionOverrideEnum.SHIPPER;
        //            break;
        //        case "CONSIGNEE":
        //            retVal = P44M.Shipment.DirectionOverrideEnum.CONSIGNEE;
        //            break;
        //    }

        //    return retVal;
        //}
      
        //private void Dispatch(DAL.Models.RateRequestOrder order)
        //{



        //    var apiInstance = new LTLDispatchApi();
        //    apiInstance.Configuration.Username = "username";
        //    apiInstance.Configuration.Password = "password";

        //    //   P44 PhoneNumber:
        //    //     Only North American phone numbers are accepted. 
        //    //     In requests, only digits and an optional 'X' (or lowercase 'x')
        //    //     marking the start of an extension will be used. Any other characters included
        //    //     for formatting will be stripped - - project44 will provide the phone number to
        //    //     the capacity provider in the format they accept. There must be at least ten digits
        //    //     before the optional 'X' (for the area code, central office code, and station
        //    //     code), and no more than thirteen, with the first digits exceeding ten being interpreted
        //    //     as the country code. The number of digits after the optional 'X' must be less
        //    //     than seven. Examples of acceptable phone numbers: '+1 123-456-7890 x 32', '(123)456-7890
        //    //     ext 30', '1234567890', '11234567890', '55-123-456-7890 ext. 312412', '+1 123-456-7890,
        //    //     ext. 1234'. In responses, phone numbers will be returned in the following format:
        //    //     '[+123 ]123-456-7890[, ext. 123456]' excluding the brackets and with the characters
        //    //     in brackets being optionally returned..

        //    #region "CAPACITY PROVIDER ACCOUNT GROUP"

        //    //** CAPACITY PROVIDER ACCOUNT GROUP **
        //    //Capacity provider account group, containing the account to be used for authentication
        //    //with the capacity provider's shipment API. (required).

        //    #region "Account Group"

        //    //(Code)
        //    //The code for the capacity provider account group that contains all accounts against
        //    //which an operation is to be performed. 
        //    //Capacity provider account groups are set up through the project44 Self-Service Portal. 
        //    //If no key is specified, the 'Default' account group will be used.

        //    var accountGroup = "";

        //    #endregion

        //    #region "Account Codes"

        //    //(Accounts)
        //    //Capacity provider accounts used for authentication with the capacity providers' APIs. 
        //    //For quoting, defaults to all accounts within the account group. 
        //    //For shipment, shipment status, and image, one and only one account is required.

        //    List<P44M.CapacityProviderAccount> accounts = new List<P44M.CapacityProviderAccount>();

        //    var accountCode = new P44M.CapacityProviderAccount("code");
        //    accounts.Add(accountCode);

        //    #endregion

        //    var capacityProviderAccountGroup = new P44M.CapacityProviderAccountGroup()
        //    {
        //        Code = accountGroup,
        //        Accounts = accounts
        //    };

        //    #endregion

        //    #region "ORIGIN"

        //    //** ORIGIN **
        //    //The origin address and contact for the shipment to be picked up. 
        //    //The origin contact will default to the requester, if not provided. (required).
        //    var origAddress = new P44M.Address()
        //    {
        //        PostalCode = order.Pickup.CompPostalCode,
        //        AddressLines = new List<string> { order.Pickup.CompAddress1, order.Pickup.CompAddress2, order.Pickup.CompAddress3 },
        //        City = order.Pickup.CompCity,
        //        State = order.Pickup.CompState,
        //        Country = getCountryEnum(order.Pickup.CompCountry)
        //    };
        //    var origContact = new P44M.Contact()
        //    {
        //        CompanyName = order.Pickup.CompName,
        //        ContactName = "OrigContactName",
        //        PhoneNumber = "OrigContactPhoneNumber",
        //        PhoneNumber2 = "OrigContactPhoneNumber2",
        //        Email = "OrigContactEmail",
        //        FaxNumber = "OrigContactFaxNumber"
        //    };
        //    var origin = new P44M.Location(origAddress, origContact); // NOTE: Location.Id not used for now but can be included for possible future use
           
        //    #endregion

        //    #region "DESTINATION"

        //    //** DESTINATION **
        //    //The destination address and contact for the requested shipment. (required).
        //    var destAddress = new P44M.Address()
        //    {
        //        PostalCode = order.Stops[0].CompPostalCode,
        //        AddressLines = new List<string> { order.Stops[0].CompAddress1, order.Stops[0].CompAddress2, order.Stops[0].CompAddress3 },
        //        City = order.Stops[0].CompCity,
        //        State = order.Stops[0].CompState,
        //        Country = getCountryEnum(order.Stops[0].CompCountry)
        //    };
        //    var destContact = new P44M.Contact()
        //    {
        //        CompanyName = order.Stops[0].CompName,
        //        ContactName = "DestContactName",
        //        PhoneNumber = "DestContactPhoneNumber",
        //        PhoneNumber2 = "DestContactPhoneNumber2",
        //        Email = "DestContactEmail",
        //        FaxNumber = "DestContactFaxNumber"
        //    };
        //    var destination = new P44M.Location(destAddress, destContact); // NOTE: Location.Id not used for now but can be included for possible future use

        //    #endregion

        //    #region "REQUESTOR LOCATION"

        //    //** REQUESTOR LOCATION **
        //    //The address and contact of the agent or freight coordinator who is responsible for the order. 
        //    //Contact name, phone number, and email are required. (required).
        //    //** NOTE ** For now I am defaulting this to be the same as origin
        //    var requestorAddress = new P44M.Address()
        //    {
        //        PostalCode = order.Pickup.CompPostalCode,
        //        AddressLines = new List<string> { order.Pickup.CompAddress1, order.Pickup.CompAddress2, order.Pickup.CompAddress3 },
        //        City = order.Pickup.CompCity,
        //        State = order.Pickup.CompState,
        //        Country = getCountryEnum(order.Pickup.CompCountry)
        //    };
        //    var requestorContact = new P44M.Contact()
        //    {
        //        CompanyName = order.Pickup.CompName,
        //        ContactName = "OrigContactName",
        //        PhoneNumber = "OrigContactPhoneNumber",
        //        PhoneNumber2 = "OrigContactPhoneNumber2",
        //        Email = "OrigContactEmail",
        //        FaxNumber = "OrigContactFaxNumber"
        //    };
        //    var requestor = new P44M.Location(requestorAddress, requestorContact); // NOTE: Location.Id not used for now but can be included for possible future use

        //    #endregion

        //    #region "LINE ITEMS"

        //    //** LINE ITEMS **
        //    //The line items to be shipped.
        //    //A line item consists of one or more packages, all of the same package type 
        //    //and with the same dimensions, freight class, and NMFC code. 
        //    //Each package, however, may have a different number of pieces and a different weight. 
        //    //Note that each capacity provider has a different maximum number of 
        //    //line items that they can accept. (required).
        //    List<P44M.LineItem> lineItems = new List<P44M.LineItem>();

        //    foreach (DAL.Models.RateRequestStop oStop in order.Stops)
        //    {
        //        if (oStop.Items != null && oStop.Items.Count() > 0)
        //        {
        //            foreach (DAL.Models.RateRequestItem item in oStop.Items)
        //            {
        //                var li = new P44M.LineItem();

        //                //(TotalWeight) Total weight of all packages composing this line item. (required).
        //                li.TotalWeight = item.Weight;

        //                //(PackageDimensions) Dimensions of each package in this line item. (required).
        //                var dimensions = new P44M.CubicDimension()
        //                {
        //                    Length = item.Length,
        //                    Width = item.Width,
        //                    Height = item.Height
        //                };                     
        //                li.PackageDimensions = dimensions;

        //                //(FreightClass) Freight class of all packages composing this item. Required for LTL quotes and shipments only
        //                li.FreightClass = getFreightClassEnum(item.FreightClass);

        //                //(PackageType) Type of packages composing this line item. (default: 'PLT').
        //                li.PackageType = getPackageTypeEnum(item.PackageType);
                        
        //                //(TotalPackages) The number of packages composing this line item. (default: '1').
        //                li.TotalPackages = 0;

        //                //(TotalPieces) The total number of pieces across all packages composing this line item. (default:'1').
        //                li.TotalPieces = 0;

        //                //(Description) Readable description of this line item
        //                li.Description = item.Description;

        //                //(Stackable) Whether the packages composing this line item are stackable. (default: 'false').
        //                li.Stackable = item.Stackable;

        //                //(NmfcItemCode) NMFC prefix code for all packages composing this line item.
        //                li.NmfcItemCode = item.NMFCItem;

        //                //(NmfcSubCode) NMFC suffix code for all packages composing this line item.
        //                li.NmfcSubCode = item.NMFCSub;

        //                #region "Hazmat"

        //                if (item.IsHazmat)
        //                {
        //                    //(HazmatDetail)
        //                    //Not available in rating (send the hazmat accessorial instead). 
        //                    //Required for shipment if this line item contains hazardous materials. 
        //                    //Provides important information about the hazardous materials to be transported, 
        //                    //as required by the US Department of Transportation (DOT).
        //                    var hazmatDetail = new P44M.LineItemHazmatDetail()
        //                    {
        //                        //(IdentificationNumber) The United Nations (UN) or North America (NA) number identifying the hazmat item. (required).                      
        //                        IdentificationNumber = item.HazmatId,
        //                        //(ProperShippingName) The proper shipping name of the hazmat item. (required).
        //                        ProperShippingName = "",
        //                        //(HazardClass)
        //                        //The hazard class number, according to the classification system outlined by the
        //                        //Federal Motor Carrier Safety Administration (FMCSA). 
        //                        //This is a one digit number or a two digit number separated by a decimal. (required).
        //                        HazardClass = item.HazmatClass,
        //                        //(PackingGroup) The hazmat packing group for a line item, indicating the degree of danger. (required).
        //                        PackingGroup = getPackingGroupEnum("")
        //                    };
        //                    li.HazmatDetail = hazmatDetail;
        //                }

        //                #endregion

        //                //Add the line item to the list
        //                lineItems.Add(li);
        //            }
        //        }
        //    }

        //    #endregion

        //    #region "PICKUP WINDOW"

        //    //** PICKUP WINDOW **
        //    //The pickup date and time range in the timezone of the shipment's origin location. (required).
        //    var pickupWindow = new P44M.LocalDateTimeWindow()
        //    {
        //        //(Date) Date for this time window in the timezone of the applicable location.(default: current date, format: yyyy-MM-dd).
        //        Date = "",
        //        //(StartTime) Start time of this window in the timezone of the applicable location. (format: HH:mm).
        //        StartTime = "",
        //        //(EndTime) End time of this window in the timezone of the applicable location. (format: HH:mm).
        //        EndTime = ""
        //    };

        //    #endregion

        //    #region "DELIVERY WINDOW"

        //    //** DELIVERY WINDOW **
        //    //The delivery date and time range in the timezone of the shipment's destination location. 
        //    //Required by some capacity providers when requesting guaranteed or expedited services.
        //    var deliveryWindow = new P44M.LocalDateTimeWindow()
        //    {
        //        //(Date) Date for this time window in the timezone of the applicable location.(default: current date, format: yyyy-MM-dd).
        //        Date = "",
        //        //(StartTime) Start time of this window in the timezone of the applicable location. (format: HH:mm).
        //        StartTime = "",
        //        //(EndTime) End time of this window in the timezone of the applicable location. (format: HH:mm).
        //        EndTime = ""
        //    };

        //    #endregion

        //    #region "CARRIER CODE"

        //    //** CARRIER CODE **
        //    //SCAC of the carrier that is to pick up this shipment. 
        //    //Required only for capacity providers that support multiple SCACs.

        //    var carrierCode = "";

        //    #endregion

        //    #region "SHIPMENT IDENTIFIERS"

        //    //Notes from Rob
        //    //---------------------------------------------------------------
        //    //External = readonly (default this value to CNS) label it SHID 
        //    //PRO = readonly BookProNumber
        //    //BOL = editable default to SHID/CNS
        //    //PO = editable default to order number
        //    //cust ref = readonly order number (label order no on screen)
        //    //---------------------------------------------------------------

        //    //** SHIPMENT IDENTIFIERS **
        //    //A list of identifiers or reference numbers for this shipment. 
        //    //Most capacity providers accept only identifiers of types 
        //    //'BILL_OF_LADING', 'PURCHASE_ORDER', and 'CUSTOMER_REFERENCE'.
        //    //Only one identifier of each type may be provided. 
        //    //An identifier of type 'SYSTEM_GENERATED' may not be provided. 
        //    //An identifier of type 'EXTERNAL' may be provided and subsequently
        //    //tracked with through project44 - - this identifier will not 
        //    //be communicated to the capacity provider.

        //    List<P44M.ShipmentIdentifier> shipmentIdentifiers = new List<P44M.ShipmentIdentifier>();

        //    //Bill of lading number, originated by the user (required)
        //    var BOL = new P44M.ShipmentIdentifier()
        //    {
        //        Type = P44M.ShipmentIdentifier.TypeEnum.BILLOFLADING,
        //        Value = ""
        //    };

        //    //Purchase order number, originated by the user
        //    var PO = new P44M.ShipmentIdentifier()
        //    {
        //        Type = P44M.ShipmentIdentifier.TypeEnum.PURCHASEORDER,
        //        Value = ""
        //    };

        //    //Other customer reference number, originated by the user
        //    var CUSTREF = new P44M.ShipmentIdentifier()
        //    {
        //        Type = P44M.ShipmentIdentifier.TypeEnum.CUSTOMERREFERENCE,
        //        Value = ""
        //    };

        //    //PRO Number, originated by the capacity provider
        //    // Can we send this?? I think we can but I can't find my notes on it
        //    var PRO = new P44M.ShipmentIdentifier()
        //    {
        //        Type = P44M.ShipmentIdentifier.TypeEnum.PRO,
        //        Value = ""
        //    };

        //    //External shipment identifier, originated by the user
        //    //(our internal id - can get passed back for tracking)
        //    var EXT = new P44M.ShipmentIdentifier()
        //    {
        //        Type = P44M.ShipmentIdentifier.TypeEnum.EXTERNAL,
        //        Value = ""
        //    };

        //    shipmentIdentifiers = new List<P44M.ShipmentIdentifier>(){ BOL, PO, CUSTREF, PRO, EXT };

        //    #endregion

        //    #region "ACCESSORIAL SERVICES"

        //    //** ACCESSORIAL SERVICES **
        //    //List of accessorial services to be requested for this shipment. 
        //    //Some capacity providers support accessorial services without 
        //    //providing a way of requesting them through their API. 
        //    //To handle this, project44 sends these accessorial services
        //    //through the capacity provider's pickup note API field, 
        //    //according to the shipment note configuration.

        //    List<P44M.AccessorialService> accessorialServices = new List<P44M.AccessorialService>();

        //    foreach (string a in order.Accessorials)
        //    {
        //        //(Code) The code for the requested accessorial service. 
        //        //A list of accessorial service codes supported by project44 is provided in the API reference data section. (required).
        //        accessorialServices.Add(new P44M.AccessorialService(a));
        //    }

        //    #endregion

        //    #region "PICKUP NOTE"

        //    //** PICKUP NOTE **
        //    //Note that applies to the pickup of this shipment. 
        //    //The shipment note configuration determines the final pickup note 
        //    //that is sent through the capacity provider's API and whether or 
        //    //not part of it may be cut off.

        //    var pickupNote = "";

        //    #endregion

        //    #region "DELIVERY NOTE"

        //    //** DELIVERY NOTE **
        //    //Note that applies to the delivery of this shipment. 
        //    //Currently, since nearly all capacity provider APIs have only 
        //    //a pickup note field and not a delivery note field, 
        //    //this delivery note will be inserted into the capacity provider's 
        //    //pickup note API field, according to the shipment note configuration.

        //    var deliveryNote = "";

        //    #endregion

        //    #region "EMERGENCY CONTACT"

        //    //** EMERGENCY CONTACT **
        //    //Emergency contact name and phone number are required when the shipment contains
        //    //items marked as hazardous materials.

        //    //If this is not provided just duplicate the Requestor Contact Mapping 
        //    var emergencyContact = requestorContact;

        //    #endregion

        //    #region "CAPACITY PROVIDER QUOTE NUMBER"

        //    //** CAPACITY PROVIDER QUOTE NUMBER **
        //    //The quote number for this shipment assigned by the capacity provider. 
        //    //Only a few LTL capacity providers accept a quote number when placing a shipment for pickup. 
        //    //Most volume LTL capacity providers, however, require a quote number.

        //    var capacityProviderQuoteNumber = "";

        //    #endregion

        //    #region "TOTAL LINEAR FEET (Not Used - Only for volume LTL shipments)"

        //    //** TOTAL LINEAR FEET **
        //    //The total linear feet that the shipment being quoted will take up in a trailer.
        //    //!***** ONLY FOR VOLUME LTL SHIPMENTS. *****!

        //    var totalLinearFeet = 0;

        //    #endregion

        //    #region "WEIGHT UNIT"

        //    //** WEIGHT UNIT **
        //    //Weight measurement unit for all weight values in this shipment request. (default: 'LB').

        //    var weightUnit = P44M.Shipment.WeightUnitEnum.LB;
        //    //var weightUnit = P44M.Shipment.WeightUnitEnum.KG;

        //    #endregion

        //    #region "LENGTH UNIT"

        //    //** LENGTH UNIT **
        //    //Length measurement unit for all length values in this shipment request. (default: 'IN').

        //    var lengthUnit = P44M.Shipment.LengthUnitEnum.IN;
        //    //var lengthUnit = P44M.Shipment.LengthUnitEnum.CM;

        //    #endregion

        //    #region "PAYMENT TERMS OVERRIDE"

        //    //** PAYMENT TERMS OVERRIDE **
        //    //An override of payment terms for the capacity provider account used by this request,
        //    //if it has 'Enable API override of payment terms' set as 'true' in the project44 Self-Service Portal. 
        //    //This functionality is typically used in situations where both inbound and outbound shipments 
        //    //are common for a given capacity provider and account number.

        //    var paymentTermsOverride = getPaymentTermsOverrideEnum("");

        //    #endregion

        //    #region "DIRECTION OVERRIDE"

        //    //** DIRECTION OVERRIDE **
        //    //An override of direction for the capacity provider account used by this request,
        //    //if it has 'Enable API override of direction' set as 'true' in the project44 Self-Service Portal.
        //    //This functionality is typically used in situations where both inbound and outbound shipments 
        //    //are common for a given capacity provider and account number.

        //    var directionOverride = getDirectionOverrideEnum("");

        //    #endregion

        //    #region "API CONFIGURATION"

        //    //** API CONFIGURATION **
        //    //Fields for configuring the behavior of this API.

        //    #region "Note Configuration"
        //    //** Note Configuration **
        //    //Configuration of the pickup note that will be constructed by project44. 
        //    //Pickup note construction is used to send some requested accessorial services to 
        //    //capacity providers through their API when no other API field is available for these services.
        //    //It is also used to send the delivery note through the capacity provider's pickup note API field. 
        //    //If no note configuration is provided, the default note configuration will be used, which will 
        //    //send the following note sections in order and will not enable truncation: 
        //    //pickup note, priority accessorials, pickup accessorials, dimensions, delivery note, 
        //    //delivery accessorials, and other accessorials.


        //    //(EnableTruncation)
        //    //If set to 'true', project44 will truncate the final pickup note it constructs
        //    //if it exceeds the maximum allowable length for the capacity provider. 
        //    //When 'false', an error will be returned and the shipment will be not placed 
        //    //for pickup with the capacity provider if the constructed pickup note exceeds
        //    //the maximum allowable length for the capacity provider. (default: 'false').

        //    var enableTruncation = false;

        //    //(NoteSections)
        //    //A list of sections, in order, to send through the capacity provider's pickup note API field. 
        //    //Brief descriptions of accessorial services will be used. 
        //    //Item dimensions are added in the format 00x00x00. 
        //    //If not provided, the default note sections, in order, are as follows: 
        //    //pickup note, priority accessorials, pickup accessorials, dimensions, 
        //    //delivery note, delivery accessorials, and other accessorials.

        //    List<P44M.ShipmentNoteSection> noteSections = new List<P44M.ShipmentNoteSection>()
        //    {
        //        new P44M.ShipmentNoteSection(P44M.ShipmentNoteSection.NameEnum.PICKUPNOTE),
        //        new P44M.ShipmentNoteSection(P44M.ShipmentNoteSection.NameEnum.DELIVERYNOTE),
        //        new P44M.ShipmentNoteSection(P44M.ShipmentNoteSection.NameEnum.PRIORITYACCESSORIALS),
        //        new P44M.ShipmentNoteSection(P44M.ShipmentNoteSection.NameEnum.PICKUPACCESSORIALS),
        //        new P44M.ShipmentNoteSection(P44M.ShipmentNoteSection.NameEnum.DELIVERYACCESSORIALS),
        //        new P44M.ShipmentNoteSection(P44M.ShipmentNoteSection.NameEnum.OTHERACCESSORIALS),
        //        new P44M.ShipmentNoteSection(P44M.ShipmentNoteSection.NameEnum.DIMENSIONS)
        //    };

        //    var noteConfiguration = new P44M.ShipmentNoteConfiguration()
        //    {
        //        EnableTruncation = enableTruncation,
        //        NoteSections = noteSections
        //    };

        //    #endregion

        //    #region "Allow Unsupported Accessorials"
        //    //** Allow Unsupported Accessorials **
        //    //If set to 'true', accessorial services that are not known to be supported by
        //    //the capacity provider will be allowed and will be sent through the capacity provider's
        //    //pickup note API field, according to the shipment note configuration. 
        //    //This is useful when the customer knows that a capacity provider supports an accessorial
        //    //service that they have not documented, or when the customer has a special agreement
        //    //with the capacity provider. (default: 'false').

        //    var allowUnsupportedAccessorials = true;

        //    #endregion

        //    #region "Enable Unit Conversion"
        //    //** Enable Unit Conversion **
        //    //If set to 'true', weight and length values in this shipment request will 
        //    //be converted when necessary to the capacity provider's supported units. 
        //    //When 'false', an error will be returned and the shipment will not be placed 
        //    //with the capacity provider if the capacity provider does not support the 
        //    //provided weight and length units. (default: 'false').

        //    var enableUnitConversion = true;

        //    #endregion

        //    #region "Fall Back To Default Account Group"
        //    //** Fall Back To Default Account Group **
        //    //If set to 'true' and the provided capacity provider account group 
        //    //code is invalid, the default capacity provider account group will be used. 
        //    //When 'false', an error will be returned if the provided capacity provider 
        //    //account group code is invalid. (default: 'false').

        //    var fallBackToDefaultAccountGroup = false;

        //    #endregion

        //    #region "Pre Scheduled Pickup"
        //    //** Pre Scheduled Pickup **
        //    //If set to 'true', will identify the pickup for this shipment as being already
        //    //scheduled, and will only transmit BOL information to the carrier.

        //    var preScheduledPickup = false;

        //    #endregion


        //    var apiConfiguration = new P44M.ShipmentApiConfiguration()
        //    {
        //        NoteConfiguration = noteConfiguration,
        //        AllowUnsupportedAccessorials = allowUnsupportedAccessorials,
        //        EnableUnitConversion = enableUnitConversion,
        //        FallBackToDefaultAccountGroup = fallBackToDefaultAccountGroup,
        //        PreScheduledPickup = preScheduledPickup
        //    };

        //    #endregion

            
        //    var shipment = new P44M.Shipment()
        //    {
        //        CapacityProviderAccountGroup = capacityProviderAccountGroup,
        //        OriginLocation = origin,
        //        DestinationLocation = destination,
        //        RequesterLocation = requestor,
        //        LineItems = lineItems,
        //        PickupWindow = pickupWindow,
        //        DeliveryWindow = deliveryWindow,
        //        CarrierCode = carrierCode,
        //        ShipmentIdentifiers = shipmentIdentifiers,
        //        AccessorialServices = accessorialServices,
        //        PickupNote = pickupNote,
        //        DeliveryNote = deliveryNote,
        //        EmergencyContact = emergencyContact,
        //        CapacityProviderQuoteNumber = capacityProviderQuoteNumber,
        //        TotalLinearFeet = totalLinearFeet,
        //        WeightUnit = weightUnit,
        //        LengthUnit = lengthUnit,
        //        PaymentTermsOverride = paymentTermsOverride,
        //        DirectionOverride = directionOverride,
        //        ApiConfiguration = apiConfiguration
        //    };

        //    try
        //    {
        //        // POST: Create a shipment.
        //        var result = apiInstance.CreateShipment(shipment);
        //        //P44M.ShipmentConfirmation result = apiInstance.CreateShipment(shipment);
        //        //P44M.ApiError errorResult = apiInstance.CreateShipment(shipment);

        //        //NOTE ** The result can either be a P44M.ShipmentConfirmation or a P44M.ApiError **


        //        //public ShipmentConfirmation(List<ShipmentIdentifier> ShipmentIdentifiers = null, string CapacityProviderBolUrl = null, string PickupNote = null, DateTime? PickupDateTime = default(DateTime?), List<Message> InfoMessages = null);

        //        //
        //        // Summary:
        //        //     /// Initializes a new instance of the P44SDK.V4.Model.ShipmentConfirmation class.
        //        //     ///
        //        //
        //        // Parameters:
        //        //   ShipmentIdentifiers:
        //        //     A list of identifiers for the confirmed shipment. Nearly all capacity providers
        //        //     provide a pickup confirmation number, which will appear in this list with type
        //        //     'PICKUP'. A few capacity providers also provide a PRO number when a shipment
        //        //     is confirmed. Shipment identifiers provided by the customer will show up here,
        //        //     as well..
        //        //
        //        //   CapacityProviderBolUrl:
        //        //     URL pointing to a PDF document of the capacity provider's Bill of Lading, if
        //        //     available..
        //        //
        //        //   PickupNote:
        //        //     The final note that was sent through the capacity provider's pickup note API
        //        //     field, as constructed by project44 according to the requested shipment note configuration..
        //        //
        //        //   PickupDateTime:
        //        //     The pickup date and time as provided by the capacity provider in the timezone
        //        //     of origin location of the shipment. (format: yyyy-MM-dd'T'HH:mm:ss).
        //        //
        //        //   InfoMessages:
        //        //     System messages and messages from the capacity provider with severity 'INFO'
        //        //     or 'WARNING'. No messages with severity 'ERROR' will be returned here..

        //                    //
        //                    // Summary:
        //                    //     /// Initializes a new instance of the P44SDK.V4.Model.Message class. ///
        //                    //
        //                    // Parameters:
        //                    //   Severity:
        //                    //     The severity of this message..
        //                    //
        //                    //   _Message:
        //                    //     Message informational text..
        //                    //
        //                    //   Diagnostic:
        //                    //     Diagnostic information, often originating from the capacity provider..
        //                    //
        //                    //   Source:
        //                    //     The originator of this message - - either project44 (the system) or the capacity
        //                    //     provider..
        //                    //public Message(SeverityEnum? Severity = default(SeverityEnum?), string _Message = null, string Diagnostic = null, SourceEnum? Source = default(SourceEnum?));


        //        //
        //        // Summary:
        //        //     /// Initializes a new instance of the P44SDK.V4.Model.ApiError class. ///
        //        //
        //        // Parameters:
        //        //   HttpStatusCode:
        //        //     The value of the HTTP status code..
        //        //
        //        //   HttpMessage:
        //        //     Description of the HTTP status code..
        //        //
        //        //   ErrorMessage:
        //        //     Description of the error..
        //        //
        //        //   Errors:
        //        //     (Optional) Collection of Message objects which provide further information as
        //        //     to the cause of this error..
        //        //
        //        //   SupportReferenceId:
        //        //     A reference identifier used by project44 support to assist with certain error
        //        //     messages..
        //        //public ApiError(int? HttpStatusCode = default(int?), string HttpMessage = null, string ErrorMessage = null, List<Message> Errors = null, string SupportReferenceId = null);



        //    }
        //    catch (Exception e)
        //    {
        //        //Debug.Print("Exception when calling LTLDispatchApi.CreateShipment: " + e.Message);
        //    }
        //}

        #endregion

    }
}