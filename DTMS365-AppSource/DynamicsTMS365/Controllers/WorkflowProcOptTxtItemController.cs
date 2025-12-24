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
    /// Second Child Display for Workflow Setup page links to tblParProcOptTxtItem
    /// </summary>
    /// <remarks>
    /// Created by RHR for v-8.5.3.006 on 10/20/2022
    /// </remarks>
    public class WorkflowProcOptTxtItemController : NGLControllerBase
    {
        #region " Constructors "

        public WorkflowProcOptTxtItemController()
            : base(Utilities.PageEnum.WorkflowSetup)
        {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.WorkflowProcOptTxtItemController";
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
            //Note: The id must always match a ProcOptTxtItemControl associated with the selected tblParProcOptTxtItem
            //an invalid parent key Error is returned if the data does not match
            //If id is zero and no records are found the client should configure the page to add a new service record.

            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            try
            {
                int RecordCount = 0;
                int count = 0;
                DAL.NGLtblParProcOptTxtItemData oDAL = new DAL.NGLtblParProcOptTxtItemData(Parameters);
                if (id == 0)
                {

                    LTS.tblUserPageSetting[] aUserPageSettings = this.readPageSettings("ProcOptTxtItemControl");
                    string skey = aUserPageSettings[0].UserPSMetaData;
                    if (!int.TryParse(skey, out id))
                    {
                        oDAL.throwNoDataFaultException("Please select a row from the grid and try again.");
                    }

                }
                DAL.Models.AllFilters f = new DAL.Models.AllFilters();
                if (id != 0)
                {
                    f.filterName = "ProcOptTxtItemControl";
                    f.filterValue = id.ToString();
                }


                Models.tblParProcOptTxtItem[] records = new Models.tblParProcOptTxtItem[] { };
                LTS.tblParProcOptTxtItem[] oData = new LTS.tblParProcOptTxtItem[] { };
                oData = oDAL.GettblParProcOptTxtItems(f, ref RecordCount);
                //if (oData != null && oData.Count() > 0)
                //{
                count = oData.Count();
                records = (from e in oData select Models.tblParProcOptTxtItem.selectModelData(e)).ToArray();
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
                
                int RecordCount = 0;
                int count = 0;
                //save the page filter for the next time the page loads
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter); }

                DAL.NGLtblParProcOptTxtItemData oDAL = new DAL.NGLtblParProcOptTxtItemData(Parameters);
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                //use workflow setup selected tblParProcessOption key as the ProcOptTxtItemParProcOptControl saved to f.ParentControl
                LTS.tblUserPageSetting[] aUserPageSettings = this.readPageSettings("ParProcOptControl");
                string skey = aUserPageSettings[0].UserPSMetaData;
                int Parentid = 0;
                if (!int.TryParse(skey, out Parentid))
                {
                    oDAL.throwNoDataFaultException("Please select a row from the parent record and try again.");
                }
                f.ParentControl = Parentid;
                Models.tblParProcOptTxtItem[] records = new Models.tblParProcOptTxtItem[] { };
                LTS.tblParProcOptTxtItem[] oData = new LTS.tblParProcOptTxtItem[] { };
                oData = oDAL.GettblParProcOptTxtItems(f, ref RecordCount);
                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                    records = (from e in oData select Models.tblParProcOptTxtItem.selectModelData(e)).ToArray();
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
                
                int RecordCount = 0;
                int count = 0;
                DAL.NGLtblParProcOptTxtItemData oDAL = new DAL.NGLtblParProcOptTxtItemData(Parameters);
                DAL.Models.AllFilters f = new DAL.Models.AllFilters();
                if (id == 0)
                {
                    LTS.tblUserPageSetting[] aUserPageSettings = this.readPageSettings("ParProcOptControl");
                    string skey = aUserPageSettings[0].UserPSMetaData;
                    if (!int.TryParse(skey, out id))
                    {
                        oDAL.throwNoDataFaultException("Please select a row from the parent record and try again.");
                    }
                }
                f.ParentControl = id;
                Models.tblParProcOptTxtItem[] records = new Models.tblParProcOptTxtItem[] { };
                LTS.tblParProcOptTxtItem[] oData = new LTS.tblParProcOptTxtItem[] { };
                oData = oDAL.GettblParProcOptTxtItems(f, ref RecordCount);
                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                    records = (from e in oData select Models.tblParProcOptTxtItem.selectModelData(e)).ToArray();
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
        public Models.Response Post([System.Web.Http.FromBody] Models.tblParProcOptTxtItem data)
        {
            //create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLtblParProcOptTxtItemData oDAL = new DAL.NGLtblParProcOptTxtItemData(Parameters);
                if (data.ParProcOptTIParProcOptControl == 0)
                {
                    //use workflow setup selected tblParProcessOption key as the FK ProcOptTxtItemParProcOptControl if zero
                    LTS.tblUserPageSetting[] aUserPageSettings = this.readPageSettings("ParProcOptControl");
                    string skey = aUserPageSettings[0].UserPSMetaData;
                    int Parentid = 0;
                    if (!int.TryParse(skey, out Parentid))
                    {
                        oDAL.throwNoDataFaultException("Please select a row from the parent record and try again.");
                    }
                    data.ParProcOptTIParProcOptControl = Parentid;
                }
                
                LTS.tblParProcOptTxtItem oChanges = Models.tblParProcOptTxtItem.selectLTSData(data);
                bool blnRet = oDAL.SavetblParProcOptTxtItem(oChanges);
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
                DAL.NGLtblParProcOptTxtItemData oDAL = new DAL.NGLtblParProcOptTxtItemData(Parameters);
                bool blnRet = oDAL.DeletetblParProcOptTxtItem(id);

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