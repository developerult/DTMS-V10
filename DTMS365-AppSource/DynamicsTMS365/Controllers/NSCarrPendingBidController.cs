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
    public class NSCarrPendingBidController : NGLControllerBase
    {
        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.NSCarrPendingBidController";
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

        private Models.NextStopLoad selectModelData(LTS.vNSLoadsWActiveBid d)
        {
            Models.NextStopLoad nsLoad = new Models.NextStopLoad();
            List<string> skipObjs = new List<string> { };
            string sMsg = "";
            nsLoad = (Models.NextStopLoad)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(nsLoad, d, skipObjs, ref sMsg);
            return nsLoad;
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

        [HttpGet, ActionName("GetByParent")]
        public Models.Response GetByParent(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            DAL.Models.AllFilters f = new DAL.Models.AllFilters { filterName = "LoadTenderControl", filterValue = id.ToString() };
            try
            {
                Models.NextStopLoad[] records = new Models.NextStopLoad[] { };
                int RecordCount = 0;
                int count = 0;

                LTS.vNSLoadsWActiveBid[] oData = NGLLoadTenderData.GetvNSLoadsWActiveBid(ref RecordCount, f);

                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                    records = (from e in oData
                               orderby e.LTTenderedDate descending, e.LoadTenderControl ascending
                               select selectModelData(e)).ToArray();
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
                Models.tblBid[] nsBids = new Models.tblBid[] { };
                int RecordCount = 0;
                int count = 0;

                //if the user did not do any sorting then apply the default sort
                if (f.SortValues?.Length < 1 || string.IsNullOrWhiteSpace(f.sortDirection))
                {
                    var sv2 = new DAL.Models.SortDetails { sortName = "BidControl", sortDirection = "Asc" };
                    f.SortValues = new DAL.Models.SortDetails[] { sv2 };
                }

                LTS.tblBid[] oData = NGLBidData.GetNSCarrPendingBids(f, ref RecordCount);

                if (oData != null && oData.Count() > 0)
                {
                    count = RecordCount;
                    nsBids = (from e in oData
                              orderby e.BidControl ascending
                              select selectModelData(e)).ToArray();
                }
                response = new Models.Response(nsBids, count);
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