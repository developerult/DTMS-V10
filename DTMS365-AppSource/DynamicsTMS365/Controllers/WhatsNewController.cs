using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
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

namespace DynamicsTMS365.Controllers
{
    public class WhatsNewController : NGLControllerBase
    {
        #region " Constructors "

        /// <summary>Initializes the Page property by calling the base class constructor</summary>
        public WhatsNewController() : base(Utilities.PageEnum.WhatsNew) { }

        #endregion

        #region " Properties"

        /// <summary>This property is used for logging and error tracking</summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.WhatsNewController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        private Models.WhatsNew selectModelData(LTS.vWhatsNew d)
        {
            Models.WhatsNew modelRecord = new Models.WhatsNew();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "WhatsNewUpdated" };
                string sMsg = "";
                modelRecord = (Models.WhatsNew)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.WhatsNewUpdated.ToArray()); }
            }
            return modelRecord;
        }

        public static LTS.tblWhatsNew selectLTSData(Models.WhatsNew d)
        {
            LTS.tblWhatsNew ltsRecord = new LTS.tblWhatsNew();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "Version", "_Version", "tblFeatureType", "_tblFeatureType", "WhatsNewUpdated", "_WhatsNewUpdated" };
                string sMsg = "";
                ltsRecord = (LTS.tblWhatsNew)DTran.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.WhatsNewUpdated = bupdated == null ? new byte[0] : bupdated;
                }
            }
            return ltsRecord;
        }

        #endregion

        #region " REST Services"
        /// POST 	/API/objectcontroller{data}  : Create a new object or Update a the current object if the control number exists
        /// GET 	/API/objectcontroller/{id}   : Get the object information where the control number = "id"
        /// PUT 	/API/objectcontroller/{data} : Update the object information stored in data
        /// DELETE	/API/objectcontroller/{id}   : Delete object where the control number = "id"

        [HttpGet, ActionName("Get")]
        public Models.Response Get(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DModel.AllFilters f = new DModel.AllFilters();
                f.filterName = "WhatsNewControl";
                f.filterValue = id.ToString(); //Note: The id must always match a WhatsNewControl
                Models.WhatsNew[] records = new Models.WhatsNew[] { };
                int RecordCount = 0;
                int count = 0;
                LTS.vWhatsNew[] oData = NGLWhatsNewData.GetvWhatsNew(ref RecordCount, f);
                if (oData?.Count() > 0)
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
            if (!authenticateFilter(ref response, filter)) { return response; } //Verfiy that the filters object is not null
            return GetAllRecords(filter);
        }

        [HttpGet, ActionName("GetAllRecords")]
        public Models.Response GetAllRecords(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter, "WhatsNewFltr"); } //save the page filter for the next time the page loads
                DModel.AllFilters f = new JavaScriptSerializer().Deserialize<DModel.AllFilters>(filter);
                Models.WhatsNew[] records = new Models.WhatsNew[] { };
                int RecordCount = 0;
                int count = 0;

                if (isSortEmpty(f)) { applyDefaultSort(ref f, "WhatsNewVersion", false); addToSort(ref f, "WhatsNewFeatureTypeControl", true); }

                LTS.vWhatsNew[] oData = NGLWhatsNewData.GetvWhatsNewHdrGrid(ref RecordCount, f);
                if (oData?.Count() > 0)
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

        [HttpGet, ActionName("GetByParent")]
        public Models.Response GetByParent(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DModel.AllFilters f = new DModel.AllFilters { ParentControl = id, sortName = "WhatsNewSeqNo", sortDirection = "Asc" };
                Models.WhatsNew[] records = new Models.WhatsNew[] { };
                int RecordCount = 0;
                int count = 0;
                LTS.vWhatsNew[] oData = NGLWhatsNewData.GetvWhatsNewDetailGrid(ref RecordCount, f);
                if (oData?.Count() > 0)
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

        [HttpPost, ActionName("Post")]
        public Models.Response Post([FromBody]Models.WhatsNew data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                LTS.tblWhatsNew oChanges = selectLTSData(data);
                bool blnRet = NGLWhatsNewData.SaveWhatsNew(oChanges);
                bool[] oRecords = new bool[1] { blnRet };
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

        [HttpDelete, ActionName("DELETE")]
        public Models.Response DELETE(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                bool blnRet = NGLWhatsNewData.DeleteWhatsNew(id);
                bool[] oRecords = new bool[1] { blnRet };
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


        //LVV - I hate how I wrote this but if I am experimenting and if I choose this I will fix it later
        [HttpPost, ActionName("AddWhatsNew")]
        public Models.Response AddWhatsNew([FromBody]Models.WhatsNewItem data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                //save the first record as the header and the rest as notes
                bool blnRet = false;
                LTS.tblWhatsNew wnHeader = new LTS.tblWhatsNew() { WhatsNewVersion = data.Version, WhatsNewFeatureTypeControl = data.FeatureType, WhatsNewSeqNo = data.SequenceNo, WhatsNewTitle = data.Title, WhatsNewNote = "" };
                int ctrl = NGLWhatsNewData.InsertWhatsNew(wnHeader);
                if (ctrl > 0)
                {
                    blnRet = true;
                    List<LTS.tblWhatsNew> batch = new List<LTS.tblWhatsNew>();
                    foreach (Models.NoteItem item in data.Notes)
                    {
                        LTS.tblWhatsNew wnNote = new LTS.tblWhatsNew() {
                            WhatsNewVersion = data.Version,
                            WhatsNewFeatureTypeControl = data.FeatureType,
                            WhatsNewTitle = "",
                            WhatsNewNote = item.Note,
                            WhatsNewSeqNo = item.SequenceNo,
                            WhatsNewParentID = ctrl
                        };
                        batch.Add(wnNote);
                    }
                    blnRet = NGLWhatsNewData.InsertWhatsNew(batch);
                }
                bool[] oRecords = new bool[1] { blnRet };
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

        [HttpGet, ActionName("GetWhatsNewReportHTML")]
        public Models.Response GetWhatsNewReportHTML(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                //if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter, "WhatsNewFltr"); } //save the page filter for the next time the page loads
                //DModel.AllFilters f = new JavaScriptSerializer().Deserialize<DModel.AllFilters>(filter);
                string html = NGLWhatsNewData.GetWhatsNewReportHTML(filter);
                string[] records = new string[1] { html };
                response = new Models.Response(records, 1);
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