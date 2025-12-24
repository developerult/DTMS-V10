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
using DTran = Ngl.Core.Utility.DataTransformation;

namespace DynamicsTMS365.Controllers
{
    public class NextStopCarrierController : NGLControllerBase
    {
        #region " Constructors "

        /// <summary>
        /// Initializes the Page property by calling the base class constructor
        /// </summary>
        public NextStopCarrierController() : base(Utilities.PageEnum.NextStopCarrier) { }

        #endregion

        #region " Properties"

        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.NextStopCarrierController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        public static LTS.tblBid selectLTSData(Models.tblBid d)
        {
            LTS.tblBid ltsRecord = new LTS.tblBid();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "BidUpdated", "tblLoadTender", "_tblLoadTender", "tblBidStatusCode", "_tblBidStatusCode", "tblBidType", "_tblBidType" };
                string sMsg = "";
                ltsRecord = (LTS.tblBid)DTran.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.BidUpdated = bupdated == null ? new byte[0] : bupdated;
                }
            }
            return ltsRecord;
        }

        #endregion

        #region " REST Services"

        /// <summary>
        /// Inserts the Carrier's NEXTStop bid as a new record in the Bid Table
        /// </summary>
        /// <returns></returns>
        [HttpPost, ActionName("PostNSBid")]
        public Models.Response PostNSBid([System.Web.Http.FromBody]Models.tblBid bid)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            if (!authenticateCarrier(ref response)) { return response; } //Only Carriers can access this service
            try
            {
                bool blnSuccess = false;
                //Parameters = dalData.Parameters;
                //Parameters.ValidateAccess = false;
                LTS.tblBid ltsRecord = selectLTSData(bid);
                ltsRecord.BidCarrierControl = Parameters.UserCarrierControl; //Set the CarrierControl 

                LTS.tblBid ltsResults = NGLBidData.InsertNEXTStopBid(ltsRecord);

                if (ltsRecord != null && ltsRecord.BidControl != 0) { blnSuccess = true; }

                bool[] oRecords = new bool[1] { blnSuccess };
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
        /// Deletes a Bid using the Bid Control by setting Archived to True and BidStatusCode to CarrDeleted
        /// Used by the NEXTStop Carrier to "Withdraw" or Delete a bid
        /// </summary>
        /// <returns></returns>
        [HttpDelete, ActionName("DeleteNSBid")]
        public Models.Response DeleteNSBid(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            if (!authenticateCarrier(ref response)) { return response; } //Only Carriers can access this service
            try
            {
                NGLBidData.CarrierDeleteBid(id, Parameters.UserName);
                bool[] oRecords = new bool[1] { true };
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