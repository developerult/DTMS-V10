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
    public class tblBidSvcErrController : NGLControllerBase
    {
        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.tblBidSvcErrController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        ////private Models.tblBidSvcErr selectModelData(LTS.tblBidSvcErr d)
        ////{
        ////    Models.tblBidSvcErr nsBid = new Models.tblBidSvcErr();
        ////    List<string> skipObjs = new List<string> { "BidSvcErrUpdated", "tblBid", "_tblBid" };
        ////    string sMsg = "";
        ////    nsBid = (Models.tblBidSvcErr)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(nsBid, d, skipObjs, ref sMsg);
        ////    return nsBid;
        ////}

        private Models.tblBidSvcErr selectModelData(LTS.tblBidSvcErr d)
        {
            Models.tblBidSvcErr modelRecord = new Models.tblBidSvcErr();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "BidSvcErrUpdated", "tblBid", "_tblBid" };
                string sMsg = "";
                modelRecord = (Models.tblBidSvcErr)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.BidSvcErrUpdated.ToArray()); }
            }
            return modelRecord;
        }

        private LTS.tblBidSvcErr selectLTSData(Models.tblBidSvcErr d)
        {
            LTS.tblBidSvcErr ltsRecord = new LTS.tblBidSvcErr();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "BidSvcErrUpdated", "tblBid", "_tblBid" };
                string sMsg = "";
                ltsRecord = (LTS.tblBidSvcErr)DTran.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.BidSvcErrUpdated = bupdated == null ? new byte[0] : bupdated;
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
                f.filterName = "BidSvcErrBidControl";
                f.filterValue = id.ToString();
                f.sortName = "BidSvcErrBidControl";
                f.sortDirection = "ASC";
                f.skip = request["skip"] == null ? 0 : int.Parse(request["skip"]); ;
                f.take = request["take"] == null ? 100 : int.Parse(request["take"]); ;
                return ReadSvcErrs(f);
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
            if (!authenticateFilter(ref response, filter)) { return response; } //Verfiy that the filters object is not null
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                return ReadSvcErrs(f);
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
            DAL.Models.AllFilters f = new DAL.Models.AllFilters { filterName = "BidSvcErrBidControl", filterValue = id.ToString() };
            try
            {
                Models.tblBidSvcErr[] records = new Models.tblBidSvcErr[] { };
                int RecordCount = 0;
                int count = 0;
                LTS.tblBidSvcErr[] oData = NGLBidData.GetBidServiceErrors(f, ref RecordCount);
                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                    if (RecordCount > count) { count = RecordCount; }
                    records = (from e in oData orderby e.BidSvcErrControl ascending select selectModelData(e)).ToArray();
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


        private Models.Response ReadSvcErrs(DAL.Models.AllFilters f)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (f == null) { response.populateDefaultInvalidFilterResponseMessage(); return response; }
            try
            {
                Models.tblBidSvcErr[] records = new Models.tblBidSvcErr[] { };
                int count = 0;
                int RecordCount = 0;               
                LTS.tblBidSvcErr[] oData = NGLBidData.GetBidServiceErrors(f, ref RecordCount);                
                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                    if (RecordCount > count) { count = RecordCount; }
                    records = (from e in oData orderby e.BidSvcErrControl ascending select selectModelData(e)).ToArray();
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