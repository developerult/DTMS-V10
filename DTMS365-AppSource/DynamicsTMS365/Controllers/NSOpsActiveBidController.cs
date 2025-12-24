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

namespace DynamicsTMS365.Controllers
{
    public class NSOpsActiveBidController : NGLControllerBase
    {
        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.NSOpsActiveBidController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        private Models.NextStopLoad selectModelData(LTS.vNSLoadsWActiveBid d)
        {
            Models.NextStopLoad modelRecord = new Models.NextStopLoad();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { };
                string sMsg = "";
                modelRecord = (Models.NextStopLoad)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                //if (modelRecord != null) { modelRecord.setUpdated(d.CarrTarUpdated.ToArray()); }
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

        /// <summary>
        /// This method returns a bid with BidStatusCode = Active and BidLoadTenderControl = id if it exists. 
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
                applyDefaultSort(ref f, "BidTotalCost", true); //if the user did not do any sorting then apply the default sort
                LTS.tblBid[] oData = NGLBidData.GetActiveBidsById(ref RecordCount, f);
                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                    records = (from e in oData select selectModelData(e)).ToArray();
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
            if (!authenticateFilter(ref response, filter)) { return response; } //Verfiy that the filters object is not null
            try
            {
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                Models.NextStopLoad[] records = new Models.NextStopLoad[] { };
                int RecordCount = 0;
                int count = 0;
                
                applyDefaultSort(ref f, "LTTenderedDate", false); //if the user did not do any sorting then apply the default sort
                addToSort(ref f, "LoadTenderControl", true); //add another sort value
                LTS.vNSLoadsWActiveBid[] oData = NGLLoadTenderData.GetvNSLoadsWActiveBid(ref RecordCount, f);
                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                    records = (from e in oData select selectModelData(e)).ToArray();
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

        /////// <summary>
        /////// Deletes the Posting to NEXTStop using the BookControl
        /////// Used by Ops to delete a NEXTStop Posting
        /////// </summary>
        /////// <returns></returns>
        ////[HttpDelete, ActionName("DELETE")]
        ////public Models.Response DELETE(int id)
        ////{
        ////    var response = new Models.Response(); //new HttpResponseMessage();
        ////    if (!authenticateController(ref response)) { return response; }
        ////    try
        ////    {
        ////        BLL.NGLDATBLL oBLL = new BLL.NGLDATBLL(Parameters);
        ////        DTO.WCFResults wcfRes = new DTO.WCFResults();
        ////        bool blnSuccess = false;

        ////        wcfRes = oBLL.RunNEXTStopDeleteMethod(id);
        ////        blnSuccess = wcfRes.Success;

        ////        //Process the results
        ////        if (blnSuccess)
        ////        {
        ////            bool[] oRecords = new bool[1] { blnSuccess };
        ////            response = new Models.Response(oRecords, 1);
        ////        }
        ////        else
        ////        {
        ////            response.StatusCode = HttpStatusCode.InternalServerError;
        ////            if (wcfRes != null)
        ////            {
        ////                string strMsg = "";
        ////                if (wcfRes.Warnings != null && wcfRes.Warnings.Count() > 0) { strMsg = wcfRes.concatWarnings() + " "; }
        ////                if (wcfRes.Errors != null && wcfRes.Errors.Count() > 0) { strMsg += wcfRes.concatErrors(); }
        ////                if (!string.IsNullOrWhiteSpace(strMsg)) { response.Errors = strMsg; }
        ////            }
        ////            else { response.Errors = "Could not delete NEXTStop Posting."; }
        ////        }
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



    }
}