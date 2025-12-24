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
    public class UnsubscribedAlertController : NGLControllerBase
    {
        #region " Constructors "
        public UnsubscribedAlertController() : base(Utilities.PageEnum.UnSubscribedAlert)
        {
        }
        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.UnsubscribedAlertController";
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

            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            try
            {
                int RecordCount = 0;
                int count = 0;
                if (id == 0) { id = this.readPagePrimaryKey(); }
                DAL.Models.AllFilters f = new DAL.Models.AllFilters();
                if (id != 0)
                {
                    f.filterName = "ProcedureControl";
                    f.filterValue = id.ToString();
                }

                DAL.NGLSecurityDataProvider oDAL = new DAL.NGLSecurityDataProvider(Parameters);
                Models.tblProcedureList[] records = new Models.tblProcedureList[] { };
                DTO.tblProcedureList[] oData = new DTO.tblProcedureList[1];
                oData[0] = oDAL.GetUserUnSubscribedSystemAlert(id, Parameters.UserControl); // Rob will provide this method

                count = oData.Count();
                records = (from e in oData select Models.tblProcedureList.selectModelData(e)).ToArray();
                if (RecordCount > count) { count = RecordCount; }

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
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter); }
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);

                DAL.NGLSecurityDataProvider oDAL = new DAL.NGLSecurityDataProvider(Parameters);
                Models.tblProcedureList[] records = new Models.tblProcedureList[] { };
                DTO.tblProcedureList[] oData = new DTO.tblProcedureList[] { };
                oData = oDAL.GetUnSubscribedSystemAlertsByUser(Parameters.UserControl);

                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                    records = (from e in oData select Models.tblProcedureList.selectModelData(e)).ToArray();
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
        public Models.Response Post([System.Web.Http.FromBody] Models.tblProcedureList data)
        {
            //create a response message to send back
            int RecordCount = 0;
            int count = 0;
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLSecurityDataProvider oDAL = new DAL.NGLSecurityDataProvider(Parameters);
                Models.tblProcedureList[] records = new Models.tblProcedureList[] { };
                DTO.tblProcedureList[] oData = new DTO.tblProcedureList[] { };

                oData = oDAL.UnSubscribeSystemAlertByUser(data.ProcedureControl, Parameters.UserControl);
                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                    records = (from e in oData select Models.tblProcedureList.selectModelData(e)).ToArray();

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

            return response;
        }



        [HttpDelete, ActionName("DELETE")]
        public Models.Response DELETE(int ProcdeureControl)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            int RecordCount = 0;
            int count = 0;
            if (!authenticateController(ref response)) { return response; }
            try
            {

                DAL.NGLSecurityDataProvider oDAL = new DAL.NGLSecurityDataProvider(Parameters);
                Models.tblProcedureList[] records = new Models.tblProcedureList[] { };
                DTO.tblProcedureList[] oData = new DTO.tblProcedureList[] { }; //data.ProcAlertUserXrefShowPopup, data.ProcAlertUserXrefSendEmail
                oData = oDAL.UnSubscribeSystemAlertByUser(ProcdeureControl, Parameters.UserControl);
                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                    records = (from e in oData select Models.tblProcedureList.selectModelData(e)).ToArray();

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



        [HttpPost, ActionName("UnsubScribeAll")]
        public Models.Response UnsubScribeAll([System.Web.Http.FromBody] Models.tblProcedureList data)
        {
            //create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            int RecordCount = 0;
            int count = 0;
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLSecurityDataProvider oDAL = new DAL.NGLSecurityDataProvider(Parameters);
                Models.tblProcedureList[] records = new Models.tblProcedureList[] { };
                DTO.tblProcedureList[] oData = new DTO.tblProcedureList[] { };
                oData = oDAL.UnSubscribeAllSystemAlertsByUser(Parameters.UserControl);
                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                    records = (from e in oData select Models.tblProcedureList.selectModelData(e)).ToArray();

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

            return response;
        }


        #endregion


    }
}