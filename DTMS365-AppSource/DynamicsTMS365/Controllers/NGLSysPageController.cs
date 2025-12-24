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
using BLL = NGL.FM.BLL;

//Added By LVV on 8/19/20 for v-8.3.0.001 - Task#20200817144456 - Modify the License File Integration Command Line Utility

namespace DynamicsTMS365.Controllers
{
    public class NGLSysPageController : NGLControllerBase
    {
        #region " Constructors "

        /// <summary> Initializes the Page property by calling the base class constructor </summary>
        public NGLSysPageController() : base(Utilities.PageEnum.NGLSystemMaint) { }

        #endregion

        #region " Properties"

        /// <summary> This property is used for logging and error tracking. </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.NGLSysPageController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        private Models.cmPage selectModelData(LTS.cmPage d)
        {
            Models.cmPage modelRecord = new Models.cmPage();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "PageUpdated", "tblFormList", "cmPageDetails", "cmPageMenus", "cmPageTemplateXrefs" };
                string sMsg = "";
                modelRecord = (Models.cmPage)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.PageUpdated.ToArray()); }
            }
            return modelRecord;
        }

        private LTS.cmPage selectLTSData(Models.cmPage d)
        {
            LTS.cmPage ltsRecord = new LTS.cmPage();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "PageUpdated", "tblFormList", "cmPageDetails", "cmPageMenus", "cmPageTemplateXrefs" };
                string sMsg = "";
                ltsRecord = (LTS.cmPage)DTran.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.PageUpdated = bupdated == null ? new byte[0] : bupdated;
                }
            }
            return ltsRecord;
        }


        #endregion

        #region " REST Services"

        /// POST 	/API/objectcontroller{data}  : Create a new object or Update a the current object if the control number exists
        /// GET 	/API/objectcontroller/{id} : Get the object information where the control number = "id"
        /// PUT 	/API/objectcontroller/{data} : Update the object information stored in data
        /// DELETE	/API/objectcontroller/{id} : Delete object where the control number = "id"

        [HttpGet, ActionName("Get")]
        public Models.Response Get(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DModel.AllFilters f = new DModel.AllFilters();
                f.filterName = "PageControl";
                f.filterValue = id.ToString(); //Note: The id must always match a PageControl
                Models.cmPage[] records = new Models.cmPage[] { };
                DAL.NGLcmPageData oPg = new DAL.NGLcmPageData(Parameters);
                int RecordCount = 0;
                int count = 0;
                LTS.cmPage[] oData = oPg.GetPages(ref RecordCount, f);
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
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter, "PgFooterGridFltr"); } //save the page filter for the next time the page loads
                DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DModel.AllFilters>(filter);
                DAL.NGLcmPageData oPg = new DAL.NGLcmPageData(Parameters);
                Models.cmPage[] records = new Models.cmPage[] { };
                int RecordCount = 0;
                int count = 0;
                LTS.cmPage[] oData = oPg.GetPages(ref RecordCount, f);
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
        public Models.Response Post([FromBody]Models.cmPage data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLcmPageData oPg = new DAL.NGLcmPageData(Parameters);
                bool blnRet = false;
                LTS.cmPage ltsPg = selectLTSData(data);
                LTS.cmPage oData = oPg.Save(ltsPg);
                if(oData != null && oData.PageControl > 0) { blnRet = true; }
                bool[] oRecords = new bool[] { blnRet };
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

        //Not Currently Used
        [HttpDelete, ActionName("DELETE")]
        public Models.Response DELETE(int id)
        {
            //Note: CRUD not currently supported in this controller
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                //DAL.NGLBookLoadBoard oDAL = new DAL.NGLBookLoadBoard(Parameters);
                //bool blnRet = oDAL.DeleteBookLoadBoard(id);
                //bool[] oRecords = new bool[1];
                //oRecords[0] = blnRet;
                //response = new Models.Response(oRecords, 1);
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