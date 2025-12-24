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
    public class UserSecurityCarrierController : NGLControllerBase
    {

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.UserSecurityCarrierController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        private Models.vUserSecurityCarrier selectModelData(LTS.vUserSecurityCarrier d)
        {
            Models.vUserSecurityCarrier modelRecord = new Models.vUserSecurityCarrier();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "USCUpdated" };
                string sMsg = "";
                modelRecord = (Models.vUserSecurityCarrier)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.USCUpdated.ToArray()); }
            }
            return modelRecord;
        }

        private LTS.tblUserSecurityCarrier selectLTSData(Models.vUserSecurityCarrier d)
        {
            LTS.tblUserSecurityCarrier ltsRecord = new LTS.tblUserSecurityCarrier();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "USCUpdated" };
                string sMsg = "";
                ltsRecord = (LTS.tblUserSecurityCarrier)DTran.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.USCUpdated = bupdated == null ? new byte[0] : bupdated;
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
                Models.vUserSecurityCarrier[] records = new Models.vUserSecurityCarrier[]{};
                int count = 0;
                LTS.vUserSecurityCarrier oData = NGLUserSecurityCarrierData.GetvUserSecurityCarrier(id);
                if (oData != null)
                {
                    Models.vUserSecurityCarrier record = selectModelData(oData);
                    records[0] = record;
                    count = 1;
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
                //save the page filter for the next time the page loads
                //if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter); }
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                return GetvUserSecurityCarriers(f);                
            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }
        }

        /// <summary>
        /// Gets All the Child vLECompCar Records filtered by LECompAccessorialCompControl passed in as id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, ActionName("GetByParent")]
        public Models.Response GetByParent(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            DAL.Models.AllFilters f = new DAL.Models.AllFilters { ParentControl = id };
            return GetvUserSecurityCarriers(f);
        }

        [HttpPost, ActionName("Post")]
        public Models.Response Post([System.Web.Http.FromBody]Models.vUserSecurityCarrier data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                LTS.tblUserSecurityCarrier oChanges = selectLTSData(data);
                string strRet = NGLUserSecurityCarrierData.InsertOrUpdateUserSecurityCarrier(oChanges);
                if (!string.IsNullOrWhiteSpace(strRet))
                {
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Errors = strRet;
                    return response;
                }
                bool[] oRecords = new bool[1] { true };
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
                string strMsg = "";
                bool blnRet = NGLUserSecurityCarrierData.Delete(id, ref strMsg);
                if (blnRet == false && !string.IsNullOrWhiteSpace(strMsg))
                {
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Errors = strMsg;
                }
                else
                {
                    bool[] oRecords = new bool[1] { blnRet };
                    response = new Models.Response(oRecords, 1);
                }         
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
        /// GenericResult.Control = UserSecurityControl
        /// GenericResult.blnField = IsEdit
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet, ActionName("GetUserAssociatedCarriersList")]
        public Models.Response GetUserAssociatedCarriersList(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                Models.GenericResult gr = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Models.GenericResult>(filter);
                int usc = gr.Control;
                bool IsEdit = gr.blnField;
                int count = 0;
                DTO.vLookupList[] records = NGLUserSecurityCarrierData.GetUserAssociatedCarriersList(usc, IsEdit);
                if (records?.Count() > 0) { count = records.Count(); } else { records = new DTO.vLookupList[] { }; }
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

        #region "Private Methods"

        private Models.Response GetvUserSecurityCarriers(DAL.Models.AllFilters f)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            int RecordCount = 0;
            int count = 0;
            Models.vUserSecurityCarrier[] records = new Models.vUserSecurityCarrier[] { };
            LTS.vUserSecurityCarrier[] oData = NGLUserSecurityCarrierData.GetvUserSecurityCarriers(ref RecordCount, f);
            if (oData?.Count() > 0)
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