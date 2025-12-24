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
using DTran = Ngl.Core.Utility.DataTransformation;

namespace DynamicsTMS365.Controllers
{
    public class NSOpsHistoricalLoadController : NGLControllerBase
    {
        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.NSOpsHistoricalLoadController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        private Models.NextStopLoad selectModelData(LTS.tblLoadTender d)
        {
            Models.NextStopLoad modelRecord = new Models.NextStopLoad();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "LTUpdated", "tblLoadTenderType", "_tblLoadTenderType" };
                string sMsg = "";
                modelRecord = (Models.NextStopLoad)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.LTUpdated.ToArray()); }
            }
            return modelRecord;
        }

        private Models.tblBid selectModelData(LTS.tblBid d)
        {
            Models.tblBid modelRecord = new Models.tblBid();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "BidUpdated", "tblLoadTender", "_tblLoadTender", "tblBidStatusCode", "_tblBidStatusCode", "tblBidType", "_tblBidType" };
                string sMsg = "";
                modelRecord = (Models.tblBid)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.BidUpdated.ToArray()); }
            }
            return modelRecord;
        }

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
            if (!authenticateFilter(ref response, filter)) { return response; } //Verfiy that the filters object is not null
            try
            {
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                Models.NextStopLoad[] records = new Models.NextStopLoad[] { };
                int RecordCount = 0;
                int count = 0;              
                applyDefaultSort(ref f, "LoadTenderControl", false); //if the user did not do any sorting then apply the default sort
                LTS.vNSHisoricalLoad[] oData = NGLLoadTenderData.GetNSHisoricalLoads(ref RecordCount, f);
                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                    if (RecordCount > count) { count = RecordCount; }
                }
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

        /// <summary>
        /// This method returns all bids with BidLoadTenderControl = id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, ActionName("GetByParent")]
        public Models.Response GetByParent(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            DAL.Models.AllFilters f = new DAL.Models.AllFilters { filterName = "BidLoadTenderControl", filterValue = id.ToString() };
            try
            {
                Models.tblBid[] records = new Models.tblBid[] { };
                int RecordCount = 0;
                int count = 0;
                addToFilters(ref f, "BidTotalCost", "1", "999999999"); //filter out 0 cost bids
                applyDefaultSort(ref f, "BidTotalCost", true); //if the user did not do any sorting then apply the default sort
                LTS.vBid[] oData = NGLBidData.GetvBids(f, ref RecordCount);
                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                    if (RecordCount > count) { count = RecordCount; }
                }
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