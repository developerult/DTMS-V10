using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL = Ngl.FreightMaster.Data;
using LTS = Ngl.FreightMaster.Data.LTS;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using BLL = NGL.FM.BLL;
using DTran = Ngl.Core.Utility.DataTransformation;


namespace DynamicsTMS365.Controllers
{
    /// <summary>
    ///First Child Display for Workflow Setup page links to tblParProcess
    /// </summary>
    /// <remarks>
    /// Created by RHR for v-8.5.3.006 on 10/20/2022  
    /// </remarks>
    public class WorkflowParProcessController : NGLControllerBase
    {
        #region " Constructors "

        public WorkflowParProcessController()
            : base(Utilities.PageEnum.WorkflowSetup)
        {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.WorkflowParProcessController";
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
        //    POST 	/API/objectcontroller{data
        //}  : Create a new object or Update a the current object if the control number exists
        /// 
        /// GET 	/API/objectcontroller/{id} : Get the object information where the control number = "id"
        /// PUT 	/API/objectcontroller/{data} : Update the object information stored in data
        /// DELETE	/API/objectcontroller/{id} : Delete object where the control number = "id"
        /// 

        [HttpGet, ActionName("Get")]
        public Models.Response Get(int id)
        {
            //Note: The id must always match a CarrTarEquipControl associated with the select tariff using CarrTarEquipCarrTarControl
            //the system looks up the last saved tariff pk for this user and return the first Service record found
            //an invalid parent key Error is returned if the data does not match
            //If id is zero and no records are found the client should configure the page to add a new service record.

            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            try
            {
                int RecordCount = 0;
                int count = 0;
                DAL.NGLtblParProcessData oDAL = new DAL.NGLtblParProcessData(Parameters);
                if (id == 0)
                {
                    
                    LTS.tblUserPageSetting[] aParProcControl = this.readPageSettings("ParProcControl");
                    string skey = aParProcControl[0].UserPSMetaData;
                    if (!int.TryParse(skey,out id))
                    {
                        oDAL.throwNoDataFaultException("Please select a row from the grid and try again.");
                    }
                    
                }
                DAL.Models.AllFilters f = new DAL.Models.AllFilters();
                if (id != 0)
                {
                    f.filterName = "ParProcControl";
                    f.filterValue = id.ToString();
                }

                
                Models.tblParProcess[] records = new Models.tblParProcess[] { };
                LTS.tblParProcess[] oData = new LTS.tblParProcess[] { };
                oData = oDAL.GettblParProcesses(f, ref RecordCount);
                //if (oData != null && oData.Count() > 0)
                //{
                count = oData.Count();
                records = (from e in oData select Models.tblParProcess.selectModelData(e)).ToArray();
                if (RecordCount > count) { count = RecordCount; }
                //  }
                response = new Models.Response(records, count);
            }
            catch (Exception ex)
            {
                //Error handler
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }

            // return the HTTP Response.
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
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            try
            {
                //ParProcCategoryControl
                int RecordCount = 0;
                int count = 0;
                //save the page filter for the next time the page loads
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter); }
                
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                //uset workflowsetup selected primary key as the ParProcCategoryControl saved to f.ParentControl
                f.ParentControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.WorkflowSetup); 
                DAL.NGLtblParProcessData oDAL = new DAL.NGLtblParProcessData(Parameters);
                Models.tblParProcess[] records = new Models.tblParProcess[] { };
                LTS.tblParProcess[] oData = new LTS.tblParProcess[] { };
                oData = oDAL.GettblParProcesses(f, ref RecordCount);
                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                    records = (from e in oData select Models.tblParProcess.selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                response = new Models.Response(records, count);
            }
            catch (Exception ex)
            {
                //Error handler
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }

            // return the HTTP Response.
            return response;
        }


        [HttpGet, ActionName("GetByParent")]
        public Models.Response GetByParent(int id)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            try
            {
                //ParProcCategoryControl
                int RecordCount = 0;
                int count = 0;
             

                DAL.Models.AllFilters f = new DAL.Models.AllFilters();
                if (id == 0)
                {
                   id = readPagePrimaryKey(Parameters, Utilities.PageEnum.WorkflowSetup);
                }

                f.ParentControl = id;
                DAL.NGLtblParProcessData oDAL = new DAL.NGLtblParProcessData(Parameters);
                Models.tblParProcess[] records = new Models.tblParProcess[] { };
                LTS.tblParProcess[] oData = new LTS.tblParProcess[] { };
                oData = oDAL.GettblParProcesses(f, ref RecordCount);
                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                    records = (from e in oData select Models.tblParProcess.selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                response = new Models.Response(records, count);
            }
            catch (Exception ex)
            {
                //Error handler
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }

            // return the HTTP Response.
            return response;
        }

        [HttpPost, ActionName("Post")]
        public Models.Response Post([System.Web.Http.FromBody] Models.tblParProcess data)
        {
            //create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLtblParProcessData oDAL = new DAL.NGLtblParProcessData(Parameters);
                LTS.tblParProcess oChanges = Models.tblParProcess.selectLTSData(data);
                bool blnRet = oDAL.SavetblParProcess(oChanges);
                bool[] oRecords = new bool[1];
                oRecords[0] = blnRet;
                response = new Models.Response(oRecords, 1);
            }
            catch (Exception ex)
            {
                //Error handler
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
            //create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLtblParProcessData oDAL = new DAL.NGLtblParProcessData(Parameters);
                bool blnRet = oDAL.DeletetblParProcess(id);

                bool[] oRecords = new bool[1];

                oRecords[0] = blnRet;

                response = new Models.Response(oRecords, 1);

            }
            catch (Exception ex)
            {
                //Error handler
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }

            return response;
        }


        #endregion


        #region " public methods"


        #endregion

    }
}