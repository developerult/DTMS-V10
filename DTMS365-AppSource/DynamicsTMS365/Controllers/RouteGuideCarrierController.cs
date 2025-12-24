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

    public class RouteGuideCarrierController : NGLControllerBase
    {
        #region " Constructors "

        public RouteGuideCarrierController() : base(Utilities.PageEnum.RouteGuideCarrier)
        {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.RouteGuideCarrierController";
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
                    f.filterName = "StaticRouteCarrControl";
                    f.filterValue = id.ToString();
                }

                DAL.NGLtblStaticRouteCarrData oDAL = new DAL.NGLtblStaticRouteCarrData(Parameters);
                Models.tblStaticRouteCarr[] records = new Models.tblStaticRouteCarr[] { };
                LTS.tblStaticRouteCarr[] oData = new LTS.tblStaticRouteCarr[] { };
                oData = oDAL.GettblStaticRouteCarrs(f, ref RecordCount);
                //if (oData != null && oData.Count() > 0)
                //{
                count = oData.Count();
                records = (from e in oData select Models.tblStaticRouteCarr.selectModelData(e)).ToArray();
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
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                DAL.NGLtblStaticRouteCarrData oDAL = new DAL.NGLtblStaticRouteCarrData(Parameters);
                Models.tblStaticRouteCarr[] records = new Models.tblStaticRouteCarr[] { };
                LTS.tblStaticRouteCarr[] oData = new LTS.tblStaticRouteCarr[] { };
                oData = oDAL.GettblStaticRouteCarrs(f, ref RecordCount);
                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                    records = (from e in oData select Models.tblStaticRouteCarr.selectModelData(e)).ToArray();
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
        public Models.Response Post([System.Web.Http.FromBody] Models.tblStaticRouteCarr data)
        {
            //create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLtblStaticRouteCarrData oDAL = new DAL.NGLtblStaticRouteCarrData(Parameters);
                LTS.tblStaticRouteCarr oChanges = Models.tblStaticRouteCarr.selectLTSData(data);
                bool blnRet = oDAL.SavetblStaticRouteCarr(oChanges);
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
                DAL.NGLtblStaticRouteCarrData oDAL = new DAL.NGLtblStaticRouteCarrData(Parameters);
                bool blnRet = oDAL.DeletetblStaticRouteCarr(id);

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

    }
}