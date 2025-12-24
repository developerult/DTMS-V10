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

namespace DynamicsTMS365.Controllers
{
    public class tblSingleSignOnController : NGLControllerBase
    {
        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.tblSingleSignOnController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        private Models.tblSingleSignOnAccount selectModelData(LTS.tblSingleSignOnAccount d)
        {
            Models.tblSingleSignOnAccount modelRecord = new Models.tblSingleSignOnAccount();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "SSOAUpdated" };
                string sMsg = "";
                modelRecord = (Models.tblSingleSignOnAccount)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.SSOAUpdated.ToArray()); }
            }
            return modelRecord;
        }

        public static LTS.tblSingleSignOnAccount selectLTSData(Models.tblSingleSignOnAccount d)
        {
            LTS.tblSingleSignOnAccount ltsRecord = new LTS.tblSingleSignOnAccount();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "SSOAUpdated" };
                string sMsg = "";
                ltsRecord = (LTS.tblSingleSignOnAccount)DTran.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.SSOAUpdated = bupdated == null ? new byte[0] : bupdated;
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
                DAL.Models.AllFilters f = new DAL.Models.AllFilters { filterName = "SSOAControl", filterValue = id.ToString() };
                response = GettblSingleSignOnAccounts365(f);
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
        /// Gets All the Child Records filtered by SSOAControl passed in as id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, ActionName("GetByParent")]
        public Models.Response GetByParent(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.Models.AllFilters f = new DAL.Models.AllFilters { filterName = "SSOAControl", filterValue = id.ToString() };
                response = GettblSingleSignOnAccounts365(f);
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
                //save the page filter for the next time the page loads
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter); }
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);

                Models.tblSingleSignOnAccount[] records = new Models.tblSingleSignOnAccount[] { };
                int RecordCount = 0;
                int count = 0;
                LTS.tblSingleSignOnAccount[] oData = NGLtblSingleSignOnAccountData.GettblSingleSignOnAccounts365(ref RecordCount, f);
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

        [HttpPost, ActionName("Post")]
        public Models.Response Post([System.Web.Http.FromBody]Models.tblSingleSignOnAccount data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                LTS.tblSingleSignOnAccount oChanges = new LTS.tblSingleSignOnAccount();
                oChanges = selectLTSData(data);
                bool blnRet = NGLtblSingleSignOnAccountData.InsertOrUpdateSingleSignOnAccount(oChanges);
                bool[] oRecords = new bool[1];
                oRecords[0] = blnRet;
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
                NGLtblSingleSignOnAccountData.DeleteSingleSignOnAccount(id);
                bool[] oRecords = new bool[1];
                oRecords[0] = true;
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

        #endregion

        #region "Private Methods"

        private Models.Response GettblSingleSignOnAccounts365(DAL.Models.AllFilters f)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            Models.tblSingleSignOnAccount[] records = new Models.tblSingleSignOnAccount[] { };
            int RecordCount = 0;
            int count = 0;
            LTS.tblSingleSignOnAccount[] oData = NGLtblSingleSignOnAccountData.GettblSingleSignOnAccounts365(ref RecordCount, f);
            if (oData != null && oData.Count() > 0)
            {
                count = oData.Count();
                records = (from e in oData select selectModelData(e)).ToArray();
                if (RecordCount > count) { count = RecordCount; }
            }
            response = new Models.Response(records, count);
            return response;
        }

        #endregion

    }
}