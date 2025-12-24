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
using DModel = Ngl.FreightMaster.Data.Models;

//NOTE: Part of special process so Bill can pay Paul (Company #5000 PPR Freight Services, Inc.)

namespace DynamicsTMS365.Controllers
{
    public class NGLAccountingConsSumController : NGLControllerBase
    {
        #region " Constructors "

        /// <summary>Initializes the Page property by calling the base class constructor</summary>
        public NGLAccountingConsSumController() : base(Utilities.PageEnum.NGLAccounting) { }

        #endregion

        #region " Properties"

        /// <summary>This property is used for logging and error tracking</summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.NGLAccountingConsSumController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        ////private Models.vLELane365 selectModelData(LTS.vLELane365 d)
        ////{
        ////    Models.vLELane365 modelRecord = new Models.vLELane365();
        ////    if (d != null)
        ////    {
        ////        List<string> skipObjs = new List<string> { "LaneUpdated" };
        ////        string sMsg = "";
        ////        modelRecord = (Models.vLELane365)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
        ////        if (modelRecord != null) { modelRecord.setUpdated(d.LaneUpdated.ToArray()); }
        ////    }
        ////    return modelRecord;
        ////}

        ////private Models.Comp selectModelData(LTS.vLEComp365 d)
        ////{
        ////    Models.Comp modelRecord = new Models.Comp();
        ////    if (d != null)
        ////    {
        ////        List<string> skipObjs = new List<string> { "CompUpdated" };
        ////        string sMsg = "";
        ////        modelRecord = (Models.Comp)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
        ////        if (modelRecord != null) { modelRecord.setUpdated(d.CompUpdated.ToArray()); }
        ////    }
        ////    return modelRecord;
        ////}

        ////public static LTS.vLELane365 selectLTSData(Models.vLELane365 d)
        ////{
        ////    LTS.vLELane365 ltsRecord = new LTS.vLELane365();
        ////    if (d != null)
        ////    {
        ////        List<string> skipObjs = new List<string> { "LaneUpdated", "rowguid", "_Lane", "Lane", "_tblRouteType", "tblRouteType" };
        ////        string sMsg = "";
        ////        ltsRecord = (LTS.vLELane365)DTran.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
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
        /// POST 	/API/objectcontroller{data}  : Create a new object or Update a the current object if the control number exists
        /// GET 	/API/objectcontroller/{id}   : Get the object information where the control number = "id"
        /// PUT 	/API/objectcontroller/{data} : Update the object information stored in data
        /// DELETE	/API/objectcontroller/{id}   : Delete object where the control number = "id"

        //Called from Popup Window and Not Grid
        [HttpGet, ActionName("Get")]
        public Models.Response Get(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DModel.AllFilters f = new DModel.AllFilters();
                f.filterName = "BookControl";
                f.filterValue = id.ToString(); //Note: The id must always match a BookControl
                int RecordCount = 0;
                int count = 0;
                LTS.vNGLAccounting[] oData = NGLBookData.GetvNGLAccounting(ref RecordCount, f);
                if (oData?.Count() > 0)
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

        //Called from Grid
        [HttpGet, ActionName("GetAllRecords")]
        public Models.Response GetAllRecords(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                LTS.vNGLAccountingConsSum[] oData = new LTS.vNGLAccountingConsSum[] { };
                int RecordCount = 0;
                int count = 0;

                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter, "consSummaryGridFilter"); } //save the page filter for the next time the page loads
                DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DModel.AllFilters>(filter);
                if (!string.IsNullOrWhiteSpace(f.Data))
                {
                    addToFilters(ref f, "BookConsPrefix", f.Data);
                    oData = NGLBookData.GetvNGLAccountingConsSum(ref RecordCount, f);
                    if (oData?.Count() > 0)
                    {
                        count = oData.Count();
                        if (RecordCount > count) { count = RecordCount; }
                    }
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
        /// Updates specifically the non 'PPR' records on the CNS 
        /// ** NOTE: THIS POST METHOD IS CALLED FROM THE POPUP EDIT WINDOW AND NOT FROM THE GRID **
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Post")]
        public Models.Response Post([FromBody]LTS.vNGLAccounting data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                if (data == null) { response.StatusCode = HttpStatusCode.BadRequest; response.Errors = Utilities.getLocalizedMsg("E_CannotSaveNoData"); return response; }
                bool blnRet = false;
                DModel.ResultObject oData = null;
                oData = NGLBookData.UpdateNGLAccountingConsSum(data);
                if (oData != null)
                {
                    blnRet = oData.Success;
                    if (!oData.Success) { response.StatusCode = HttpStatusCode.InternalServerError; response.Errors = oData.ErrMsg; return response; }
                }
                bool[] results = new bool[1] { blnRet };
                response = new Models.Response(results, 1);
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

        //Called by Summary Widget
        [HttpGet, ActionName("GetTotals")]
        public Models.Response GetTotals(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                LTS.vNGLAccountingConsSum[] records = new LTS.vNGLAccountingConsSum[] { };
                int count = 0;
                //Get the CNS from the page settings for the grid
                var pgSet = readPageSettings("consSummaryGridFilter");
                if (pgSet?.Count() > 0)
                {
                    DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DModel.AllFilters>(pgSet[0].UserPSMetaData);
                    if (!string.IsNullOrWhiteSpace(f.Data))
                    {
                        LTS.vNGLAccountingConsSum oData = NGLBookData.GetNGLAcctConsSumTotals(f.Data);
                        if (oData != null)
                        {
                            records = new LTS.vNGLAccountingConsSum[1] { oData };
                            count = 1;
                        }
                    }                   
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

    }
}