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

namespace DynamicsTMS365.Controllers
{
    public class EditorController : NGLControllerBase
    {

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.Editor";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " REST Services"       


        [HttpGet, ActionName("GetEditorContentNoAuth")]
        public Models.Response GetEditorContentNoAuth(string filter)
        {
            DAL.Models.EditorContent ofilter = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.EditorContent>(filter);

            // create a response message to send back
            var response = new Models.Response();
            DAL.Models.EditorContent edit = new DAL.Models.EditorContent();

            try
            {
                DAL.NGLcmPageData dalPGData = new DAL.NGLcmPageData(Parameters);
                edit = dalPGData.GetEditorContent(ofilter);
            }
            catch (Exception ex)
            {

                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }

            int count = 1;
            Array d = new DAL.Models.EditorContent[1] { edit };
            response = new Models.Response(d, count);
            return response;
        }


        [HttpPost, ActionName("SaveEditor")]
        public Models.Response SaveEditor([System.Web.Http.FromBody]DAL.Models.EditorContent h)
        {
            var response = new Models.Response();
            bool blnRes = false;
            int uc = 0;

            var token = System.Web.HttpContext.Current.Request.Headers["Authorization"];
            var susc = System.Web.HttpContext.Current.Request.Headers["USC"];
            if (!int.TryParse(susc, out uc)) { uc = h.USec; }

            if (Utilities.GlobalSSOResultsByUser.ContainsKey(uc))
            {
                DAL.Models.SSOResults res = Utilities.GlobalSSOResultsByUser[uc];
                //modified by RHR for v-8.5.3.007 on 6/14/2023 to fix issue with token expiration
                //if (res == null || res.willTokenExpire(120, token))
                //{
                //    response.StatusCode = HttpStatusCode.NonAuthoritativeInformation;
                //    response.Errors = "Your authorization is about to expire. Please log in again to continue.";
                //    return response;
                //}
            }
            else
            {
                response.StatusCode = HttpStatusCode.Unauthorized;
                response.Errors = "You are not authorized to access this data. Please log in to continue.";
                return response;
            }
            try
            {
                DAL.NGLcmPageData dalPGData = new DAL.NGLcmPageData(Parameters);
                blnRes = dalPGData.SaveEditorContent(h);
            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }
            int count = 1;
            Array d = new bool[1] { blnRes };
            response = new Models.Response(d, count);
            return response;
        }


        #endregion

    }
}