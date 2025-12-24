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
using DModel = Ngl.FreightMaster.Data.Models;

namespace DynamicsTMS365.Controllers
{
    public class tblSSOAConfigController : NGLControllerBase
    {
        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.5.4.002 on 08/07/2023 
        /// initializes the Page property by calling the base class constructor
        /// </summary>
        public tblSSOAConfigController()
                : base(Utilities.PageEnum.tblSSOAConfig)
        {
        }

        #endregion
        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.tblSSOAConfigController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"


        #endregion


        #region " REST Services"

        [HttpGet, ActionName("Get")]
        public Models.Response Get(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                Models.vtblSSOAConfig[] records = new Models.vtblSSOAConfig[] { };
                int count = 0;
                LTS.vtblSSOAConfig oLTSData = NGLtblSingleSignOnAccountData.GetvtblSSOAConfig(id);
                if (oLTSData != null)
                {
                    var tblSSOAConfig = Models.vtblSSOAConfig.selectModelData(oLTSData);
                    records = new Models.vtblSSOAConfig[] { tblSSOAConfig };
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
                int RecordCount = 0;
                int count = 0;
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);

                if (!string.IsNullOrWhiteSpace(filter) && (f != null && f.FilterValues.Length > 0))
                {
                    if (f.LEAdminControl == 0) { f.LEAdminControl = Utilities.GlobalSSOResultsByUser[this.UserControl].UserLEControl; }
                    bool blnOKToSave = true;

                    if (blnOKToSave)
                    {
                        savePageFilters(filter);
                    }
                }
                Models.vtblSSOAConfig[] records = new Models.vtblSSOAConfig[] { };
                LTS.vtblSSOAConfig[] oData = NGLtblSingleSignOnAccountData.GetvtblSSOAConfigs(f, ref RecordCount);
                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                    records = (from e in oData orderby e.SSOACName select Models.vtblSSOAConfig.selectModelData(e)).ToArray();
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


        /// <summary>
        /// Gets All the Child CarrierCont Records filtered by CarrierContLECarControl passed in as id
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
                DAL.Models.AllFilters f = new DAL.Models.AllFilters { ParentControl = id };
                Models.vtblSSOAConfig[] records = new Models.vtblSSOAConfig[] { };
                int RecordCount = 0;
                int count = 0;
                f.take = 100;
                LTS.vtblSSOAConfig[] oData = NGLtblSingleSignOnAccountData.GetvtblSSOAConfigs(f, ref RecordCount);
                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                    if (RecordCount > count) { count = RecordCount; }
                    records = (from e in oData select Models.vtblSSOAConfig.selectModelData(e)).ToArray();
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
        public Models.Response Post([FromBody] Models.vtblSSOAConfig data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                bool blnRet = true;
                LTS.tblSSOAConfig tblSSOAConfig = Models.vtblSSOAConfig.selectLTSData(data);
                LTS.tblSSOAConfig oResults = NGLtblSingleSignOnAccountData.InsertOrUpdatetblSSOAConfig(tblSSOAConfig);
                if (oResults == null || oResults.SSOACControl == 0) { blnRet = false; }
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

        [HttpDelete, ActionName("Delete")]
        public Models.Response Delete(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                NGLtblSingleSignOnAccountData.DeletetblSSOAConfig(id);
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

        #endregion

    }
}