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

//Added By LVV on 3/1/17 for v-8.0 Next Stop

namespace DynamicsTMS365.Controllers
{
    public class tblBidCostAdjController : NGLControllerBase
    {
        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.tblBidCostAdjController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        //private Models.tblBidCostAdj selectModelData(LTS.tblBidCostAdj d)
        //{
        //    Models.tblBidCostAdj nsBid = new Models.tblBidCostAdj();
        //    List<string> skipObjs = new List<string> { "BidCostAdjUpdated", "tblBid", "_tblBid" };
        //    string sMsg = "";
        //    nsBid = (Models.tblBidCostAdj)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(nsBid, d, skipObjs, ref sMsg);
        //    return nsBid;
        //}

        private Models.tblBidCostAdj selectModelData(LTS.tblBidCostAdj d)
        {
            Models.tblBidCostAdj modelRecord = new Models.tblBidCostAdj();
            if (d != null)
            {
                // modified by RHR for v-8.5.3.005 on 09/22/2022 BidCostAdjWeight is nullable in LTS but not in Model so CopyMatchingFields does not map
                // also it was found that modelRecord.BidCostAdjWeight is a float
                List<string> skipObjs = new List<string> { "BidCostAdjUpdated", "tblBid", "_tblBid", "BidCostAdjWeight" };
                string sMsg = "";
                modelRecord = (Models.tblBidCostAdj)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) {
                    modelRecord.setUpdated(d.BidCostAdjUpdated.ToArray());
                    modelRecord.BidCostAdjWeight = (float)(d.BidCostAdjWeight ?? 0);
                }
            }
            return modelRecord;
        }

        private LTS.tblBidCostAdj selectLTSData(Models.tblBidCostAdj d)
        {
            LTS.tblBidCostAdj ltsRecord = new LTS.tblBidCostAdj();
            if (d != null)
            {
                // modified by RHR for v-8.5.3.005 on 09/22/2022 BidCostAdjWeight is nullable in LTS but not in Model so CopyMatchingFields does not map
                // also it was found that modelRecord.BidCostAdjWeight is a float
                List<string> skipObjs = new List<string> { "BidCostAdjUpdated", "tblBid", "_tblBid", "BidCostAdjWeight" };
                string sMsg = "";
                ltsRecord = (LTS.tblBidCostAdj)DTran.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.BidCostAdjUpdated = bupdated == null ? new byte[0] : bupdated;
                    ltsRecord.BidCostAdjWeight = (double)d.BidCostAdjWeight;
                }
            }

            return ltsRecord;
        }

        #endregion

        #region " REST Services"

        [HttpGet, ActionName("Get")]
        public Models.Response Get(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                //modified by RHR for v-8.1 on 02/20/2018 we now call ReadCostAdjustments for 
                //single code base in support of content management design patterns using AllFilters object
                DAL.Models.AllFilters f = new DAL.Models.AllFilters();
                f.filterName = "BidCostAdjBidControl";
                f.filterValue = id.ToString();
                f.sortName = "BidCostAdjControl";
                f.sortDirection = "ASC";
                f.skip = request["skip"] == null ? 0 : int.Parse(request["skip"]); ;
                f.take = request["take"] == null ? 0 : int.Parse(request["take"]); ;
                return ReadCostAdjustments(f);
            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }
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
                return ReadCostAdjustments(f);
            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }
        }

        [HttpGet, ActionName("GetByParent")]
        public Models.Response GetByParent(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            DAL.Models.AllFilters f = new DAL.Models.AllFilters { filterName = "BidCostAdjBidControl", filterValue = id.ToString() };
            try
            {
                Models.tblBidCostAdj[] records = new Models.tblBidCostAdj[] { };
                int RecordCount = 0;
                int count = 0;
                LTS.tblBidCostAdj[] oData = NGLBidData.GetBidCostAdjustments(f, ref RecordCount);
                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                    if (RecordCount > count) { count = RecordCount; }
                    records = (from e in oData orderby e.BidCostAdjControl ascending select selectModelData(e)).ToArray();
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

        #endregion

        private Models.Response ReadCostAdjustments(DAL.Models.AllFilters f)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (f  == null) { response.populateDefaultInvalidFilterResponseMessage(); return response; }
            try
            {                
                Models.tblBidCostAdj[] records = new Models.tblBidCostAdj[] { };
                int count = 0;
                int RecordCount = 0;             
                LTS.tblBidCostAdj[] oData = NGLBidData.GetBidCostAdjustments(f,ref RecordCount);
                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                    if (RecordCount > count) { count = RecordCount; }
                    records = (from e in oData orderby e.BidCostAdjControl ascending select selectModelData(e)).ToArray();
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

    }
}