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

namespace DynamicsTMS365.Controllers
{
    public class EmailController : NGLControllerBase
    {
        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.EmailController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion



        #region " REST Services"

        [HttpPost, ActionName("GenerateEmail")]
        public Models.Response GenerateEmail([System.Web.Http.FromBody]Models.Email em)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            DAL.NGLEmailData oMail = new DAL.NGLEmailData(Parameters);
            try
            {   
                    oMail.GenerateEmail(em.emailFrom, em.emailTo, em.emailCc, em.emailSubject, em.emailBody, "", "", "", "");             
            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }
            response.Errors = "";
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }


        [HttpPost, ActionName("GenerateSMTPEmail")]
        public Models.Response GenerateSMTPEmail([FromBody]Models.Email em)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                string footerTag = string.Format("<br /><br />User: {2} User Email: {3}<br /><br />Server: {0} Database: {1}", Parameters.DBServer, Parameters.Database, Parameters.UserName, Parameters.UserEmail);
                NGLEmailData.GenerateEmail(em.emailFrom, em.emailTo, em.emailCc, em.emailSubject, em.emailBody + footerTag, "", "", "", "");
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