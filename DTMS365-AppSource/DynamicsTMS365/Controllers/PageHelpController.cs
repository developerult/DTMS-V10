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

//Added By LVV on 5/15/17 for v-8.0 Help Pages

namespace DynamicsTMS365.Controllers
{
    public class PageHelpController : NGLControllerBase
    {

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.PageHelpController";
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

        [HttpGet, ActionName("GetPageHelpInfo")]
        public Models.Response GetPageHelpInfo(int id)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            DAL.Models.HelpInfo helpInfo = new DAL.Models.HelpInfo();
            
            try
            {
                DAL.NGLcmPageData dalPGData = new DAL.NGLcmPageData(Parameters);
                helpInfo = dalPGData.GetPageHelpInfo(id, UserControl);
            }
            catch (Exception ex)
            {
                //Error handler
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }
          
            int count = 1;
            Array d = new DAL.Models.HelpInfo[1] { helpInfo };
            response = new Models.Response(d, count);
            return response;
        }

        [HttpPost, ActionName("PostSaveHelpInfo")]
        public Models.Response PostSave([System.Web.Http.FromBody]DAL.Models.HelpInfo h)
        {
            //create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            Models.GenericResult gr = new Models.GenericResult();

            try
            {
                DAL.NGLcmPageData dalPGData = new DAL.NGLcmPageData(Parameters);
                h.USec = Parameters.UserControl;
                var res = dalPGData.InsertOrUpdatePageHelpNotes(h);

                if (res != null)
                {
                    if (res.ErrNumber != 0)
                    { 
                        response.StatusCode = HttpStatusCode.InternalServerError;
                        response.Errors = res.RetMsg;
                    }
                    else
                    {
                        gr.Control = res.PageHelpControl ?? 0;

                        Array d = new Models.GenericResult[1] { gr };
                        response = new Models.Response(d, 1);
                    }
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

            return response;
        }


        #endregion

    }
}