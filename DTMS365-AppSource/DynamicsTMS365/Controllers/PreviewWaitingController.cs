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
    public class PreviewWaitingController : NGLControllerBase
    {
        #region " Constructors "

        public PreviewWaitingController()
            : base(Utilities.PageEnum.PreviewWaiting)
        {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.PreviewWaitingController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion
        #region " Data Translation"


        #endregion
        //BookCarrOrderNumber


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
                if (id == 0) { id = this.readPagePrimaryKey(); }
                DAL.Models.AllFilters f = new DAL.Models.AllFilters();
                if (id != 0)
                {
                    f.filterName = "BookControl";
                    f.filterValue = id.ToString();
                }
                //GetCarrierContractExpiredAlerts365
                DAL.NGLSystemAlertData oDAL = new DAL.NGLSystemAlertData(Parameters);
                LTS.vCMAlert[] records = new LTS.vCMAlert[] { };
                records = oDAL.GetPOsWaitingAlerts365(f, ref RecordCount);
                response = new Models.Response(records, 1);
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
                DAL.NGLSystemAlertData oDAL = new DAL.NGLSystemAlertData(Parameters);
                LTS.vCMAlert[] records = new LTS.vCMAlert[] { };
                records = oDAL.GetPOsWaitingAlerts365(f, ref RecordCount);
                if (records != null)
                {
                    count = records.Count();
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

        //[HttpPost, ActionName("Post")]
        //public Models.Response Post([System.Web.Http.FromBody] LTS.vCMAlert data)
        //{
        //    //create a response message to send back
        //    var response = new Models.Response(); //new HttpResponseMessage();

        //    return response;
        //}


        //[HttpDelete, ActionName("DELETE")]
        //public Models.Response DELETE(int id)
        //{
        //    //create a response message to send back
        //    var response = new Models.Response(); //new HttpResponseMessage();
        //    if (!authenticateController(ref response)) { return response; }


        //    return response;
        //}



        #endregion


        #region " public methods"


        #endregion

    }
}