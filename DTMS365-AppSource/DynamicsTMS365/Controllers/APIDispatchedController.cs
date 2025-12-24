using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using DAL = Ngl.FreightMaster.Data;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using LTS = Ngl.FreightMaster.Data.LTS;
using BLL = NGL.FM.BLL;

namespace DynamicsTMS365.Controllers
{
    public class APIDispatchedController : NGLControllerBase
    {

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.APIDispatchedController";
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


        [HttpGet, ActionName("GetRecords")]
        public Models.Response GetRecords(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateFilter(ref response, filter)) { return response; } //Verfiy that the filters object is not null
            return GetAllItems(filter);
        }

        [HttpGet, ActionName("GetAllItems")]
        public Models.Response GetAllItems(string filter)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();            
            if (!authenticateController(ref response)) { return response; }

            try
            {
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                LTS.vAPIDispatchedLoad[] records = new LTS.vAPIDispatchedLoad[] { };
                int count = 0;
                int RecordCount = 0;
                if (string.IsNullOrWhiteSpace(f.sortName))
                {
                    f.sortName = "LTBookDateLoad";
                    f.sortDirection = "DESC";
                }
                DAL.NGLLoadTenderData dalData = new DAL.NGLLoadTenderData(Parameters);
                records = dalData.GetAPIDispatchedLoads(f, ref RecordCount);
                if (records != null && records.Count() > 0)
                {
                    count = records.Count();
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

            // return the HTTP Response.
            return response;
        }
        /// <summary>
        /// Processes Posted changes not finished.  AcceptOrReject Data needs to be modified to support dispatched data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost, ActionName("PostSave")]
        public Models.Response PostSave([System.Web.Http.FromBody]Models.AcceptorReject data)
        {
            var response = new Models.Response();
           
            try
            {

                return response;



            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();                
            }

            // return the HTTP Response.
            return response;
        }



        #endregion

    }
}