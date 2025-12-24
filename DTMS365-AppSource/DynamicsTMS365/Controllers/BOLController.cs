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
using DModel = Ngl.FreightMaster.Data.Models;

namespace DynamicsTMS365.Controllers
{
    public class BOLController : NGLControllerBase
    {
        #region " Properties"

        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.BOLController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Private Mehtods"

        #endregion

        #region " REST Services"

        //NOTE: THIS IS STILL CALLED FROM BOLREPORTCTRL FROM RATESHOPPING PAGE BECAUSE THERE IS NO BOOKCONTROL IN THAT GRID
        /// <summary>
        /// Looks up the BOL Additional Service string,
        /// a comma seperated list of accessorial (descriptions) excluding charges like fuel
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.1.2 RC2 on 05/09/2018
        /// Modified by RHR we now expect the BookControl not the load Tender Control 
        /// because the load tender control is not required may be zero
        /// </remarks>
        [HttpGet, ActionName("GetBOLAdditionalServices")]
        public Models.Response GetBOLAdditionalServices(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();            
            if (!authenticateController(ref response)) { return response; }
            try
            {
                string strResult = NGLLoadTenderData.GetBOLAccessorialString(id);
                if (string.IsNullOrWhiteSpace(strResult)) { strResult = "None"; } //Added By LVV for BOL Report Changes
                Array b = new string[1] { strResult };
                response = new Models.Response(b, 1);
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
        /// Gets the BOL Model using the BookControl number PK user GetBOL to read the records using the LoadTenderControl
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, ActionName("Get")]
        public Models.Response Get(int id)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            DModel.Dispatch[] oDispatched;
            try
            {
                oDispatched = NGLLoadTenderData.GetDispatchAndBOLReportData(id, true);
                int iCount = 0;
                if (oDispatched != null) { iCount = oDispatched.Count(); }
                response = new Models.Response(oDispatched, iCount);
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

        //NOTE: THIS IS STILL CALLED FROM BOLREPORTCTRL FROM RATESHOPPING PAGE BECAUSE THERE IS NO BOOKCONTROL IN THAT GRID
        /// <summary>
        /// Get the Dispatch data using the tblLoadTender Control of a Dispatched Load
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.1.2 RC2 on 05/09/2018
        /// this method is currently identical to the Dispatching Controller class 
        /// but is duplicated here to support future changes if needed.
        /// </remarks>
        [HttpGet, ActionName("GetBOL")]
        public Models.Response GetBOL(int id)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            List<DAL.Models.Dispatch> oDispatched = new List<DAL.Models.Dispatch>();
            try
            {
                DAL.NGLLoadTenderData dalLTData = new DAL.NGLLoadTenderData(Parameters);
                DAL.Models.Dispatch oDispatch = dalLTData.getBOLToPrint(id);
                oDispatched.Add(oDispatch);
                response = new Models.Response(oDispatched.ToArray(), 1);
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



        [HttpPost, ActionName("EmailBOL")]
        public Models.Response EmailBOL([System.Web.Http.FromBody]Models.GenericResult data)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Errors = "Feature coming soon";

                //string[] oRecords = new string[1] {"Email sent to {insert email here}" };
                //response = new Models.Response(oRecords, 1);
            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                if (fault != null) { response.StatusCode = fault.StatusCode; response.Errors = fault.formatMessage(); }
                else { response.StatusCode = HttpStatusCode.ServiceUnavailable; response.Errors = ex.Message; }
                return response;
            }
            return response;
        }



        #endregion
    }
}