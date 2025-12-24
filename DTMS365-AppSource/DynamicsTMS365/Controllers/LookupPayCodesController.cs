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
    public class LookupPayCodesController : NGLControllerBase
    {
        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.2 on 08/29/2018 initializes the Page property by calling the base class constructor
        /// </summary>
        public LookupPayCodesController()
                : base(Utilities.PageEnum.StaticListMaint)
	     {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.LookupPayCodeController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region "DataTranslation"        
        private Models.Codes selectModelData(LTS.Code d)
        {
            Models.Codes modelRecord = new Models.Codes();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "CodeUpdated", "ID" };
                string sMsg = "";
                modelRecord = (Models.Codes)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);               
                if (modelRecord != null) { modelRecord.setUpdated(d.CodeUpdated.ToArray()); }
            }

            return modelRecord;
        }

        private Models.Codes selectLTSData(LTS.vLookUpCode d)
        {
            Models.Codes modelRecord = new Models.Codes();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "CodeUpdated" };
                string sMsg = "";
                modelRecord = (Models.Codes)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.CodeUpdated.ToArray()); }
            }

            return modelRecord;
        }
        #endregion

        #region " REST Services"

        [HttpGet, ActionName("Get")]
        public Models.Response Get(int id)
        {
            //Note: The id must always match a CarrTarControl associated with the select tariff 
            //the system looks up the last saved tariff pk for this user 
            //an invalid parent key Error is returned if the data does not match
            //If id is zero and no records are found the client should configure the page to add a new record.

            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int RecordCount = 0;
                int count = 0;
                string CodeType = "PAY";
                DAL.NGLCodeData oDAL = new DAL.NGLCodeData(Parameters);
                LTS.Code oData = new LTS.Code { };
                //LTS.Code[] oData1 = new LTS.Code[] { };
                LTS.vLookUpCode[] oData1 = new LTS.vLookUpCode[] { };
                DAL.Models.AllFilters f = new DAL.Models.AllFilters();
                oData1 = oDAL.GetCodesFiltered(f, ref RecordCount, CodeType).Where(x=>x.ID == id).ToArray();
                oData = oDAL.GetCodeFiltered365(oData1[0].Code, CodeType);
                Models.Codes[] records = new Models.Codes[1];
                if (oData != null)
                {
                    count = 1;
                    records[0] = selectModelData(oData);
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
                int RecordCount = 0;
                int count = 0;
                string CodeType = "PAY";
                DAL.NGLCodeData oDAL = new DAL.NGLCodeData(Parameters);
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);

                //save the page filter for the next time the page loads
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter); }
               // f.ParentControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.StaticListMaint);
                Models.Codes[] records = new Models.Codes[] { };
                LTS.vLookUpCode[] oData = new LTS.vLookUpCode[] { };
                oData = oDAL.GetCodesFiltered(f, ref RecordCount,CodeType);
                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                    records = (from e in oData select selectLTSData(e)).ToArray();
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

        [HttpPost, ActionName("Post")]
        public Models.Response Post([System.Web.Http.FromBody]LTS.Code data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
               
                bool[] oRecords = new bool[1];
                bool IsInserted = false;
                DAL.NGLCodeData oDAL = new DAL.NGLCodeData(Parameters);
                string s=new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<string>(data.CodeUpdated.ToString());
                IsInserted = (s == "") ? true : false;//CodeUpdate is null/Empty means inserting new record                
                var spRet = oDAL.SaveOrCreateCode365(data, IsInserted);              
                if (spRet.Length == 0)
                {                    
                    oRecords[0] = true;
                    response = new Models.Response(oRecords, 1);
                }
                else
                {
                    
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Errors = spRet[0].RetMsg;
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


        [HttpDelete, ActionName("DELETE")]
        public Models.Response DELETE(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int RecordCount = 0;
                string CodeType = "PAY";
                bool[] oRecords = new bool[1];
                DAL.NGLCodeData oDAL = new DAL.NGLCodeData(Parameters);
                LTS.vLookUpCode[] oData1 = new LTS.vLookUpCode[] { };
                DAL.Models.AllFilters f = new DAL.Models.AllFilters();
                oData1 = oDAL.GetCodesFiltered(f, ref RecordCount, CodeType).Where(x => x.ID == id).ToArray();
                var spRet = oDAL.DeleteCode365(oData1[0].rowguid);
                if (spRet.Length == 0)
                {
                    oRecords[0] = true;
                    response = new Models.Response(oRecords, 1);
                }
                else
                {

                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Errors = spRet[0].RetMsg;
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


        #endregion


       
    }
}