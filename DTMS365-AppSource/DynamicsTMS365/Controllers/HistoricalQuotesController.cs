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
    public class HistoricalQuotesController : NGLControllerBase
    {
        #region " Constructors "

        /// <summary>
        /// Created by RHR for v-8.2 on 05/15/2019 initializes the Page property by calling the base class constructor
        /// </summary>
        public HistoricalQuotesController()
                : base(Utilities.PageEnum.RateShopping)
	    {

        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.HistoricalQuotesController";
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

        [HttpGet, ActionName("GetRecords")]
        public Models.Response GetRecords(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateFilter(ref response, filter)) { return response; } //Verfiy that the filters object is not null
            return GetAllItems(filter);
        }

        [HttpGet, ActionName("GetAllItems")]
        public Models.Response GetAllItems(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();            
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                //save the page filter for the next time the page loads
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter, "HistoricalQuotesFltr"); }            

                LTS.vHistoricalQuote[] records = new LTS.vHistoricalQuote[] { };
                int count = 0;
                int RecordCount = 0;
                if (string.IsNullOrWhiteSpace(f.sortName) )
                {
                    f.sortName = "LoadTenderControl";
                    f.sortDirection = "DESC";
                }               
                records = NGLLoadTenderData.GetHistoricalQuotes(f, ref RecordCount);
                if (records?.Count() > 0)
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


        [HttpGet, ActionName("GetExportBids")]
        public Models.Response GetExportBids(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();            
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                //save the page filter for the next time the page loads
                //if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter, "HistoricalQuotesFltr"); }

                LTS.vExportBid[] records = new LTS.vExportBid[] { };
                int count = 0;
                int RecordCount = 0;
                if (f == null) { f = new DAL.Models.AllFilters(); }
                f.take = 1000; //we are limited to 1000 bids
                records = NGLLoadTenderData.GetExportBids(f, ref RecordCount);
                if (records?.Count() > 0)
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

        /// <summary>
        /// This logic is not complete
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet, ActionName("GenerateBidForAllActiveFilteredLoadsForCurrentUser")]
        public Models.Response GenerateBidsForAllActiveFilteredLoadsForCurrentUser(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();            
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                //save the page filter for the next time the page loads
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter, "HistoricalQuotesFltr"); }
                int RecordCount = 0;
                bool[] oRecords = new bool[1];
                oRecords[0] = NGLLoadTenderData.UpdateAllBidSelectedForExportForAllHistoricalQuotes(f, ref RecordCount, true);
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

        /// <summary>
        ///  Update All the BidSelectedForExport flags to true for All the filtered Historical Quotes
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.5.4.001 on o7/15/2023
        /// </remarks>
        [HttpGet, ActionName("SelectAllBidForExportForAllHistoricalQuotes")]
        public Models.Response SelectAllBidForExportForAllHistoricalQuotes(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();            
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                //save the page filter for the next time the page loads
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter, "HistoricalQuotesFltr"); }
                int RecordCount = 0;
                bool[] oRecords = new bool[1];
                oRecords[0] = NGLLoadTenderData.UpdateAllBidSelectedForExportForAllHistoricalQuotes(f, ref RecordCount,true);
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

        /// <summary>
        /// Update All the BidSelectedForExport flags to false for All the filtered Historical Quotes
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.5.4.001 on o7/15/2023
        /// </remarks>
        [HttpGet, ActionName("UnSelectAllBidForExportForAllHistoricalQuotes")]
        public Models.Response UnSelectAllBidForExportForAllHistoricalQuotes(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();            
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                //save the page filter for the next time the page loads
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter, "HistoricalQuotesFltr"); }
                int RecordCount = 0;
                bool[] oRecords = new bool[1];
                oRecords[0] = NGLLoadTenderData.UpdateAllBidSelectedForExportForAllHistoricalQuotes(f, ref RecordCount, false);
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



        /// <summary>
        /// Update All the BidSelectedForExport flags to true for the historical Quote 
        /// </summary>
        /// <param name="sLoadTenderControl"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.5.4.001 on o7/15/2023
        /// </remarks>
        [HttpPost, ActionName("SelectAllBidForExportForHistoricalQuote")]
        public Models.Response SelectAllBidForExportForHistoricalQuote([System.Web.Http.FromBody] string sLoadTenderControl)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLBidData oDAL = new DAL.NGLBidData(Parameters);
                int iLoadTenderControl = 0;
                int.TryParse(sLoadTenderControl, out iLoadTenderControl);
                if (iLoadTenderControl > 0)
                {
                    bool[] oRecords = new bool[1];
                    oRecords[0] = NGLLoadTenderData.UpdateAllBidSelectedForExportForHistoricalQuote(iLoadTenderControl, true);
                    response = new Models.Response(oRecords, 1);
                }
                else
                {
                    List<string> sDetailList = new List<string> { "Select All Bids for Export", "Quote Primary Key.  Please Select a Quote and try again." };
                    // Cannot save changes to {0}.  The following key fields are required: {1}.
                    FaultExceptionEventArgs fault = Utilities.BuildException("E_CannotSaveKeyFieldsRequired", sDetailList);
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Errors = fault.formatMessage();
                    return response;
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

        /// <summary>
        /// Update All the BidSelectedForExport flags to false for the Historical Quote
        /// </summary>
        /// <param name="sLoadTenderControl"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.5.4.001 on o7/15/2023
        /// </remarks>
        [HttpPost, ActionName("UnSelectAllBidForExportForHistoricalQuote")]
        public Models.Response UnSelectAllBidForExportForHistoricalQuote([System.Web.Http.FromBody] string sLoadTenderControl)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLBidData oDAL = new DAL.NGLBidData(Parameters);
                int iLoadTenderControl = 0;
                int.TryParse(sLoadTenderControl, out iLoadTenderControl);
                if (iLoadTenderControl > 0)
                {
                    bool[] oRecords = new bool[1];
                    oRecords[0] = NGLLoadTenderData.UpdateAllBidSelectedForExportForHistoricalQuote(iLoadTenderControl, false);
                    response = new Models.Response(oRecords, 1);
                }
                else
                {
                    List<string> sDetailList = new List<string> { "Unselect All Bids for Export", "Quote Primary Key.  Please Select a Quote and try again." };
                    // Cannot save changes to {0}.  The following key fields are required: {1}.
                    FaultExceptionEventArgs fault = Utilities.BuildException("E_CannotSaveKeyFieldsRequired", sDetailList);
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Errors = fault.formatMessage();
                    return response;
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

        /// <summary>
        /// Processes Accept or Reject information and returns success or fail with a message using GenericResult model
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost, ActionName("PostSave")]
        public Models.Response PostSave([System.Web.Http.FromBody]Models.AcceptorReject data)
        {
            var response = new Models.Response();
            if (data == null || data.BookControl == 0)
            {
                //todo: add error invalid request message
                return response;
            }
            if (!authenticateController(ref response)) { return response; }
            try
            {

                BLL.NGLBookBLL bllData = new BLL.NGLBookBLL(Parameters);
                BLL.NGLBookBLL.AcceptRejectEnum enmAR = BLL.NGLBookBLL.AcceptRejectEnum.Accepted;
                if (data.AcceptRejectCode == 1) { enmAR = BLL.NGLBookBLL.AcceptRejectEnum.Rejected; }
                if (data.AcceptRejectCode == 2) { enmAR = BLL.NGLBookBLL.AcceptRejectEnum.Expired; }
                DTO.WCFResults oResults = bllData.CarrierAcceptOrRejectLoad365(data.BookControl, enmAR, Parameters.UserCarrierContControl, Parameters.UserCarrierControl, data.BookTrackComment, Parameters.UserName);
                if (oResults != null)
                {
                    //TODO: add logic to localize any messages, warnings or errors
                    string strMsg = "";
                    if (oResults.Warnings != null && oResults.Warnings.Count() > 0)
                    {
                        strMsg = oResults.concatWarnings() + " ";
                    }
                    if (oResults.Errors != null && oResults.Errors.Count() > 0)
                    {
                        strMsg += oResults.concatErrors();
                    }
                    if (!string.IsNullOrWhiteSpace(strMsg))
                    {
                        response.Errors = strMsg;
                    }
                    else
                    {
                        if (oResults.Success == false)
                        {
                            response.Errors = "Unexpected Accept or Reject Failure.  Please try again.";
                        }
                    }
                }



            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }

            // return the HTTP Response.
            return response;
        }



        #endregion
    }
}