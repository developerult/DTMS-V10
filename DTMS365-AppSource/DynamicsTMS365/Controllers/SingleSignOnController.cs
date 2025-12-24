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
    public class SingleSignOnController : NGLControllerBase
    {

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.SingleSignOnController";
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

        [HttpGet, ActionName("GetSingleSignOnForUser")]
        public Models.Response GetSingleSignOnForUser(int id)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            try
            {
                DAL.NGLtblSingleSignOnAccountData oUSLE = new DAL.NGLtblSingleSignOnAccountData(Parameters);
                DAL.Models.SingleSignOn[] res = new DAL.Models.SingleSignOn[] { };
                int count = 0;

                res = oUSLE.GetSSOAListForUser365(id);

                if (res?.Length > 0) { count = res.Length; }

                response = new Models.Response(res, count);
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


        [HttpDelete, ActionName("DeleteSingleSignOnForUser")]
        public Models.Response DeleteSingleSignOnForUser(int id)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            bool blnSuccess = false;

            try
            {
                DAL.NGLtblSingleSignOnAccountData oUSLE = new DAL.NGLtblSingleSignOnAccountData(Parameters);

                if (id == 0)
                {
                    response.StatusCode = HttpStatusCode.OK;
                    // return the HTTP Response.
                    return response;
                }

                blnSuccess = oUSLE.DeleteSSOAXref(id);

                if (blnSuccess)
                {
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Errors = string.Format("Cannot delete the tblSSOASecurityXref record with Control {0}", id.ToString());

                }

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


        [HttpPost, ActionName("SaveSingleSignOnForUser")]
        public Models.Response SaveSingleSignOnForUser([System.Web.Http.FromBody]DAL.Models.SingleSignOn sso)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            try
            {
                DAL.NGLtblSingleSignOnAccountData oSSO = new DAL.NGLtblSingleSignOnAccountData(Parameters);

                //Check if the passwords match
                if (sso.SSOAXControl == 0) {
                    //Insert - always check if passwords match
                    if (!string.Equals(sso.SSOAXPass, sso.NewPass)) {
                        response.StatusCode = HttpStatusCode.InternalServerError;
                        response.Errors = "Passwords do not match";
                        return response;
                    }
                }
                else
                {
                    //Update - only check if passwords match if we are updating the passwords
                    if (sso.UpdateP) {
                        if (!string.Equals(sso.SSOAXPass, sso.NewPass)) {
                            response.StatusCode = HttpStatusCode.InternalServerError;
                            response.Errors = "Passwords do not match";
                            return response;
                        }
                    }
                }

                oSSO.InsertOrUpdateSSOASecurityXref365(sso);

                Array d = new int[1] { 1 };
                response = new Models.Response(d, 1);         
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

        [HttpPost, ActionName("MassUpdateSingleSignOn")]
        public Models.Response MassUpdateSingleSignOn([System.Web.Http.FromBody]DAL.Models.MassUpdateSingleSignOn data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                string strMsg = NGLtblSingleSignOnAccountData.MassUpdateSSOASecurityXref365(data);
                if (!string.IsNullOrWhiteSpace(strMsg))
                {
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Errors = strMsg;
                }
                else
                {
                    Array d = new bool[1] { true };
                    response = new Models.Response(d, 1);
                }       
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