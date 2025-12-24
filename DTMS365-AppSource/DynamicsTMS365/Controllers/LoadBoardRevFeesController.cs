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
    public class LoadBoardRevFeesController : NGLControllerBase
    {

        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.2 on 08/29/2018 initializes the Page property by calling the base class constructor
        /// </summary>
        public LoadBoardRevFeesController()
                : base(Utilities.PageEnum.LoadBoardRevenueFees)
	     {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.LoadBoardRevFeesController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"
        // not needed
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
            //Note: CRUD not currently supported in this controller
            var response = new Models.Response(); //new HttpResponseMessage();
           response.populateDefaultInvalidFilterResponseMessage();
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
            // filters are currently ignored for fees
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int RecordCount = 0;
                int count = 0;
                // DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);


                //save the page filter for the next time the page loads
                //if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter, "LBRevFeesFilter"); }
                //int iBookControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.LoadBoard);
                //Modified by RHR for v-8.5.4.004 on 12/06/2023 new BookControl setting
                DAL.Models.vCMLoadBoardRevTemplate[] records = new DAL.Models.vCMLoadBoardRevTemplate[] { };
                int iBookControl = 0;
                iBookControl = readBookControlPageSetting(iBookControl);
                if (iBookControl == 0)
                {
                    return new Models.Response(records, 0);
                }

                DAL.NGLBookRevenueData oDAL = new DAL.NGLBookRevenueData(Parameters);
                DAL.Models.vCMLoadBoardRevTemplate[] oData = new DAL.Models.vCMLoadBoardRevTemplate[] { };
                oData = oDAL.GetLoadBoardRevFees(iBookControl);
                count = oData.Count();
                if (RecordCount > count) { count = RecordCount; }
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
        public Models.Response Post([System.Web.Http.FromBody]LTS.vCarrierTariffMinCharge data)
        {
            //Note: CRUD not currently supported in this controller
            var response = new Models.Response(); //new HttpResponseMessage();
            response.populateDefaultInvalidFilterResponseMessage();
            return response;
        }


        [HttpDelete, ActionName("DELETE")]
        public Models.Response DELETE(int id)
        {
            //Note: CRUD not currently supported in this controller
            var response = new Models.Response(); //new HttpResponseMessage();
            response.populateDefaultInvalidFilterResponseMessage();
            return response;
        }


        #endregion


        #region " public methods"


        #endregion
    }
}