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

namespace DynamicsTMS365.Controllers
{
    public class NSCarrHistoricalBidController : NGLControllerBase
    {
        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.NSCarrHistoricalBidController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        private Models.tblBid selectModelData(LTS.tblBid d)
        {
            Models.tblBid modelRecord = new Models.tblBid();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "BidUpdated", "tblLoadTender", "_tblLoadTender", "tblBidStatusCode", "_tblBidStatusCode", "tblBidType", "_tblBidType" };
                string sMsg = "";
                modelRecord = (Models.tblBid)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.BidUpdated.ToArray()); }
            }
            return modelRecord;
        }

        ////public static LTS.vLELane365 selectLTSData(Models.vLELane365 d)
        ////{
        ////    LTS.vLELane365 ltsRecord = new LTS.vLELane365();
        ////    if (d != null)
        ////    {
        ////        List<string> skipObjs = new List<string> { "LaneUpdated", "rowguid", "_Lane", "Lane", "_tblRouteType", "tblRouteType" };
        ////        string sMsg = "";
        ////        ltsRecord = (LTS.vLELane365)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
        ////        if (ltsRecord != null)
        ////        {
        ////            byte[] bupdated = d.getUpdated();
        ////            ltsRecord.LaneUpdated = bupdated == null ? new byte[0] : bupdated;
        ////        }
        ////    }
        ////    return ltsRecord;
        ////}

        #endregion


        #region " REST Services"

        [HttpGet, ActionName("GetRecords")]
        public Models.Response GetRecords()
        {
            return GetRecords("");
        }

        [HttpGet, ActionName("GetRecords")]
        public Models.Response GetRecords(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            if (!authenticateCarrier(ref response)) { return response; } //Only Carriers can access this service
            if (!authenticateFilter(ref response, filter)) { return response; } //Verfiy that the filters object is not null
            try
            {
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                int RecordCount = 0;
                int count = 0;

                //if the user did not do any sorting then apply the default sort
                if (f.SortValues?.Length < 1 || string.IsNullOrWhiteSpace(f.sortDirection))
                {
                    var sv2 = new DAL.Models.SortDetails { sortName = "BidModDate", sortDirection = "Desc" };
                    f.SortValues = new DAL.Models.SortDetails[] { sv2 };
                }

                LTS.vBid[] oData = NGLBidData.GetNSCarHisoricalBids(ref RecordCount, Parameters.UserCarrierControl, f);

                if (oData != null && oData.Count() > 0)
                {
                    //RecordCount contains the number of records in the database that matches the filters
                    count = RecordCount;
                    //nsBids = (from e in ltsView
                    //          select selectModelData(e)).ToArray();
                }
                //response = new Models.Response(nsBids, count);
                response = new Models.Response(oData, count);
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