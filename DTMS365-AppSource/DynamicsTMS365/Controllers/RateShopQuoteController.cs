using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using DAL = Ngl.FreightMaster.Data;
using LTS = Ngl.FreightMaster.Data.LTS;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using BLL = NGL.FM.BLL;
using DTran = Ngl.Core.Utility.DataTransformation;
using DModel = Ngl.FreightMaster.Data.Models;
using Ngl.FreightMaster.Data;
using Ngl.FreightMaster.Data.LTS;

namespace DynamicsTMS365.Controllers
{
    public class RateShopQuoteController : NGLControllerBase
    {
        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.2 on 05/15/2019 initializes the Page property by calling the base class constructor
        /// </summary>
        public RateShopQuoteController()
                : base(Utilities.PageEnum.RateShopping)
	     {

        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.RateShopQuoteController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"




        #endregion

        #region " REST Services"


        /// POST 	/API/objectcontroller{data}  : Create a new object or Update a the current object if the control number exists
        /// GET 	/API/objectcontroller/{id} : Get the object information where the control number = "id"
        /// PUT 	/API/objectcontroller/{data} : Update the object information stored in data
        /// DELETE	/API/objectcontroller/{id} : Delete object where the control number = "id"


        [HttpGet, ActionName("GetRecords")]
        public Models.Response GetRecords()
        {
            return GetRecords("");
        }


        [HttpGet, ActionName("GetRecords")]
        public Models.Response GetRecords(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateFilter(ref response, filter)) { return response; } //Verfiy that the filters object is not null
            return GetAllRecords(filter);

        }

        [HttpGet, ActionName("GetAllRecords")]
        public Models.Response GetAllRecords(string filter)
        {

            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            try
            {
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter, "RateShopQuotesFilter"); }
            }
            catch (Exception ex)
            {
                //do nothing on error here
            }
            var oP44 = new P44RateQuoteController();
            return oP44.GetRecords(filter);
        }

        /// <summary>
        /// posts one record from the spreadsheet and generates the list of quotes 
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        /// <remarks>
        /// created by RHR for v-8.5.4.002 on 07/24/2023
        ///   The caller will post this API asychronous
        ///   The restult will be success or failure
        ///   The Call back method must return the ID with success or failure
        ///   The Call back method must update the status on the spreadsheet to Red or Green
        ///   The caller must assign a unique index as the Order.ID to the load (typically the row number in the spreadsheet)
        ///   The call back method will check if all rows have been processed before showing process complete
        /// </remarks>
        [HttpPost, ActionName("PostSpreadSheet")]
        public Models.Response PostSpreadSheet([System.Web.Http.FromBody] DAL.Models.RateRequestOrder order)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            if (order == null) { response.populateDefaultInvalidFilterResponseMessage(); return response; }
            try
            {
                BLL.NGLBookRevenueBLL oBookRev = new BLL.NGLBookRevenueBLL(Parameters);

                DTO.tblLoadTender.LoadTenderTypeEnum[] TenderTypes = { DTO.tblLoadTender.LoadTenderTypeEnum.RateShopping };
                DTO.tblLoadTender.BidTypeEnum[] bidTypes = { DTO.tblLoadTender.BidTypeEnum.P44, DTO.tblLoadTender.BidTypeEnum.NGLTariff };
                DateTime dtLoadDate = DateTime.Now;
                DateTime.TryParse(order.ShipDate, out dtLoadDate);
                order.DeliveryDate = dtLoadDate.AddDays(5).ToShortDateString();
                order.Pickup.RequiredDate = order.DeliveryDate;
                if (string.IsNullOrWhiteSpace(order.Pickup.CompState) && !string.IsNullOrWhiteSpace(order.Pickup.CompPostalCode))
                {
                    string sZip = order.Pickup.CompPostalCode;
                    using (var db = new NGLMASLookupDataContext(this.ConnectionString))
                    {
                        try
                        {
                            string sState = db.tblZipCodes.Where(t => t.ZipCode == sZip).Select(t => t.State).FirstOrDefault();
                            order.Pickup.CompState = sState;
                        }
                        catch { }
                    }
                }
                // foreach 
                foreach (DAL.Models.RateRequestStop s in order.Stops)
                {
                    s.RequiredDate = order.DeliveryDate;
                    if (string.IsNullOrWhiteSpace( s.CompState) && ! string.IsNullOrWhiteSpace(s.CompPostalCode))
                    {
                        string sZip = s.CompPostalCode;
                        using (var db = new NGLMASLookupDataContext(this.ConnectionString))
                        {
                            try { 
                                string sState = db.tblZipCodes.Where(t => t.ZipCode == sZip).Select(t => t.State).FirstOrDefault();
                                s.CompState = sState;                                
                            }
                            catch { }
                        }
                    }


                    
                }
                //use default for standard routing prefered = false, noLateDelivery = False, validated = True, optimizeByCapacity = True --  modeTypeControl and  tempType  are looked up later
                //Modified by RHR for v-8.5.4.006 on 05/24/2024 to use the AllowAsync and AllowP44Async properties
                DTO.GetCarriersByCostParameters tariffOptions = new DTO.GetCarriersByCostParameters(false, true, true, 0, 0);
                //use default for standard routing prefered = false, noLateDelivery = False, validated = True, optimizeByCapacity = True --  modeTypeControl and  tempType  are looked up later
                // update the async options
                tariffOptions.AllowAsync = true;
                tariffOptions.AllowP44Async = true;
                DTO.WCFResults result = oBookRev.GenerateQuote(order, TenderTypes, bidTypes, 0,tariffOptions);
                int iLoadTenderControl = 0;
                // Modified by RHR for v-8.2 01/01/2019 to simplify reading of WCFResults keys 
                result.TryParseKeyInt("LoadTenderControl", ref iLoadTenderControl);
                response.Data = new int[] { order.ID };
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

        [HttpPost, ActionName("DeleteAllQuotesForUser")]
        public Models.Response DeleteAllQuotesForUser(int id)
        {
            //create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {

                DAL.NGLLoadTenderData oDAL = new DAL.NGLLoadTenderData(Parameters);
                bool blnRet = oDAL.DeleteRateShoppingQuotes();
                bool[] oRecords = new bool[1];
                oRecords[0] = blnRet;
                response = new Models.Response(oRecords, 1);
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

        #endregion
    }

}