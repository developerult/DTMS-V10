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

namespace DynamicsTMS365.Controllers
{
    public class NSOpsPendingLoadController : NGLControllerBase
    {
        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.NSOpsPendingLoadController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        private Models.NextStopLoad selectModelData(LTS.vNSAvailablePendingLoad d)
        {
            Models.NextStopLoad modelRecord = new Models.NextStopLoad();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { };
                string sMsg = "";
                modelRecord = (Models.NextStopLoad)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                //if (modelRecord != null) { modelRecord.setUpdated(d.LTUpdated.ToArray()); }
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
            if (!authenticateFilter(ref response, filter)) { return response; } //Verfiy that the filters object is not null
            return GetvNSOpsPendingLoads(filter);
        }

        ////[HttpPost, ActionName("InsertOrUpdateLane")]
        ////public Models.Response InsertOrUpdateLane([System.Web.Http.FromBody]Models.vLELane365 l)
        ////{
        ////    var response = new Models.Response();
        ////    if (!authenticateController(ref response)) { return response; }
        ////    try
        ////    {
        ////        LTS.vLELane365 ltsLane = selectLTSData(l);

        ////        var blnRet = NGLLaneData.InsertOrUpdateLane365(ltsLane, l.LTTransType);

        ////        Array d = new bool[1] { blnRet };
        ////        response = new Models.Response(d, 1);
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
        ////        response.StatusCode = fault.StatusCode;
        ////        response.Errors = fault.formatMessage();
        ////        return response;
        ////    }
        ////    return response;
        ////}

        ////[HttpDelete, ActionName("DeleteLane")]
        ////public Models.Response DeleteLane(int id)
        ////{
        ////    var response = new Models.Response();
        ////    if (!authenticateController(ref response)) { return response; }
        ////    try
        ////    {
        ////        NGLLaneData.DeleteLane365(id);
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
        ////        response.StatusCode = fault.StatusCode;
        ////        response.Errors = fault.formatMessage();
        ////        return response;
        ////    }
        ////    return response;
        ////}

        #endregion

        #region " public methods"


        /// <summary>
        /// Returns an array of LTS.vNSAvailablePendingLoad data objects representing active 
        /// Next Stop Posted Loads that have not yet received any bids from any Carrier
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Models.Response GetvNSOpsPendingLoads(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                DAL.NGLLoadTenderData dalLTData = new DAL.NGLLoadTenderData(Parameters);
                Models.NextStopLoad[] retLanes = new Models.NextStopLoad[] { };
                int RecordCount = 0;
                int count = 0;
                //if the user did not do any sorting then apply the default sort
                if (f.SortValues?.Length < 1 || string.IsNullOrWhiteSpace(f.sortDirection))
                {
                    var sv1 = new DAL.Models.SortDetails { sortName = "LTTenderedDate", sortDirection = "Desc" };
                    var sv2 = new DAL.Models.SortDetails { sortName = "LoadTenderControl", sortDirection = "Asc" };
                    f.SortValues = new DAL.Models.SortDetails[] { sv1, sv2 };
                }
                LTS.vNSAvailablePendingLoad[] ltsView = dalLTData.GetvNSOpsPendingLoads(ref RecordCount, f);
                if (ltsView != null && ltsView.Count() > 0)
                {                   
                    count = RecordCount; //RecordCount contains the number of records in the database that matches the filters
                    retLanes = (from e in ltsView select selectModelData(e)).ToArray();
                }
                response = new Models.Response(retLanes, count);
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