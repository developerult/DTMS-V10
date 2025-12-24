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
using LTS = Ngl.FreightMaster.Data.LTS;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;

namespace DynamicsTMS365.Controllers
{
    public class AlertController : NGLControllerBase
    {

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.AlertController";
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

        [HttpGet, ActionName("Get365AlertMessagesForUser")]
        public Models.Response Get365AlertMessagesForUser()
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLSecurityDataProvider oSec = new DAL.NGLSecurityDataProvider(Parameters);
                DAL.Models.Alert[] retVals = new DAL.Models.Alert[] { };

                retVals = oSec.Get365AlertMessagesForUser();
                if (retVals == null) { retVals = new DAL.Models.Alert[] { }; }

                response = new Models.Response(retVals, retVals.Length);
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