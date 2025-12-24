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
    public class cmPageController : NGLControllerBase
    {

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.cmPageController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion


        #region " Data Translation"


        private Models.cmPage selectModelData(LTS.cmPage d)
        {
            Models.cmPage modelRecord = new Models.cmPage();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "PageUpdated","tblFormList" ,"cmPageDetails","cmPageMenus","cmPageTemplateXrefs"};
                string sMsg = "";
                modelRecord = (Models.cmPage)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.PageUpdated.ToArray()); }
            }

            return modelRecord;
        }


        private LTS.cmPage selectLTSData(Models.cmPage d)
        {
            LTS.cmPage ltsRecord = new LTS.cmPage();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "PageUpdated", "tblFormList", "cmPageDetails", "cmPageMenus", "cmPageTemplateXrefs" };
                string sMsg = "";
                ltsRecord = (LTS.cmPage)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.PageUpdated = bupdated == null ? new byte[0] : bupdated;

                }
            }

            return ltsRecord;
        }




        #endregion


        #region " REST Services"
        [HttpGet, ActionName("GetPage")]
        public Models.Response GetPage(int id)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();

            if (!authenticateController(ref response)) { return response; }
                       
            try
            {
                
                LTS.cmPage oData = new LTS.cmPage();
               
                // get the take and skip parameters int skip = request["skip"] == null ? 0 :
                //int skip = request["skip"] == null ? 0 : int.Parse(request["skip"]);
                //int take = request["take"] == null ? 10 : int.Parse(request["take"]);
                DAL.NGLcmPageData dalData = new DAL.NGLcmPageData(Parameters);
                oData = dalData.GetRecord(id);
                //List<Models.cmPage> oRecords = new List<Models.cmPage>();

                Models.cmPage[] oRecords = new Models.cmPage[1];
                
                oRecords[0] = selectModelData(oData);
                
                response = new Models.Response(oRecords, 1);
            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
                // something went wrong - possibly a database error. return a
                // 500 server error and send the details of the exception.
                //response.StatusCode = HttpStatusCode.InternalServerError;
                //response.Errors = string.Format("The database read failed: {0}", ex.Message);
                //response.
            }

            // return the HTTP Response.
            return response;

        }

        //[HttpGet, ActionName("GetPage")]
        //public Models.Response GetPage(int id,int usercontrol)
        //{
        //    // create a response message to send back
        //    var response = new Models.Response(); //new HttpResponseMessage();
        //    try
        //    {
        //        //TODO: 
        //        //  1. add code to validate the user authentication token with the usercontrol number

        //        LTS.cmPage oData = new LTS.cmPage();
        //        //count will contains the nunber of records in the database that matches the filters before paging
        //        int count = 0; // _context.Carriers.Count();

        //        // get the take and skip parameters int skip = request["skip"] == null ? 0 :
        //        //int skip = request["skip"] == null ? 0 : int.Parse(request["skip"]);
        //        //int take = request["take"] == null ? 10 : int.Parse(request["take"]);
        //        DAL.NGLcmPageData dalData = new DAL.NGLcmPageData(Parameters);
        //        oData = dalData.GetRecord(id);
        //        Models.cmPage[] oRecords = new Models.cmPage[1];

        //        oRecords[0] = selectModelData(oData);

        //        response = new Models.Response(oRecords, 1);
        //    }
        //    catch (Exception ex)
        //    {
        //        FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
        //        response.StatusCode = fault.StatusCode;
        //        response.Errors = fault.formatMessage();
        //        return response;
        //        // something went wrong - possibly a database error. return a
        //        // 500 server error and send the details of the exception.
        //        //response.StatusCode = HttpStatusCode.InternalServerError;
        //        //response.Errors = string.Format("The database read failed: {0}", ex.Message);
        //        //response.
        //    }

        //    // return the HTTP Response.
        //    return response;

        //}

        [HttpPost, ActionName("PostNewPage")]
        public Models.Response PostNewPage([System.Web.Http.FromBody]Models.cmPage data)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }            

            try
            {

                DAL.NGLcmPageData dalData = new DAL.NGLcmPageData(Parameters);
                LTS.cmPage oData = dalData.createPage(data.PageControl, data.PageName, data.PageDesc, data.PageCaption, data.PageCaptionLocal, data.PageDataSource, data.PageSortable, data.PagePageable, data.PageGroupable, data.PageEditable, data.PageDataElmtControl, data.PageElmtFieldControl, data.PageAutoRefreshSec, data.PageFormName, data.PageFormDesc, data.PageFormControl);

                Models.cmPage[] oRecords = new Models.cmPage[1];

                oRecords[0] = selectModelData(oData);

                response = new Models.Response(oRecords, 1);

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

        [HttpPost, ActionName("PostCreatePageDataElement")]
        public Models.Response PostCreatePageDataElement(string name)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }

            try
            {

                DAL.NGLcmPageData dalData = new DAL.NGLcmPageData(Parameters);
                List<LTS.cmElementField> oFields = dalData.createDataElement(name);

                response.StatusCode = HttpStatusCode.OK;           

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

        [HttpPost, ActionName("PostcmPage")]
        public Models.Response PostcmPage([System.Web.Http.FromBody]LTS.cmPage data, int usercontrol)
        {

            var response = new Models.Response();
            //Models.RateRequestOrder rorder = (Models.RateRequestOrder)request["order"];
            //string origName = string.IsNullOrEmpty(request["CompName"]) ? "Empty" : request["CompName"];
            //string name = order.CompName;
            try
            {
                //TODO: 
                //  1. add code to validate the user authentication token with the usercontrol number

                //LastUpdatedTimeStamp is passed from the get response to the ajax method and must be stored on the client in a 
                //request field like a hidden text field so it can be read back in when the data is posted.
                //this is an alternate way to pass the TimeStamp byte array from server to client and back again when we do not 
                //use a custom model objectformatMessage
                response.fillLastUpdatedTimeStampFromRequest(request);
                data.PageUpdated = response.getUpdated();
                DAL.NGLcmPageData dalData = new DAL.NGLcmPageData(Parameters);
                LTS.cmPage oData = dalData.Save(data);
                LTS.cmPage[] oRecords = new LTS.cmPage[] { oData };

                response = new Models.Response(oRecords, 1);
                
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

        [HttpPost, ActionName("PostCreateDataElementNoReturn")]
        public Models.Response PostCreateDataElementNoReturn(string filter)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLcmPageData dalData = new DAL.NGLcmPageData(Parameters);
                bool blnRet = dalData.createDataElementNoReturn(filter);
                if (blnRet)
                {
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.StatusCode = HttpStatusCode.Conflict;
                    response.Errors = "CreateDataElement " + Utilities.getLocalizedMsg("Failed");
                    return response;
                }

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
        ///Rest service to save the Pane Settings used by all pages
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.2 on 08/29/2018
        /// </remarks>
        [HttpPost, ActionName("PostPaneSetting")]
        public virtual Models.Response PostPaneSetting([FromBody] string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                LTS.tblUserPageSetting fSettings = new LTS.tblUserPageSetting { UserPSName = "PaneSettings", UserPSPageControl = (int)Utilities.PageEnum.Home, UserPSUserSecurityControl = Parameters.UserControl, UserPSMetaData = filter };
                DAL.NGLUserPageSettingData sDaL = new DAL.NGLUserPageSettingData(Parameters);
                bool blnRet = sDaL.SaveCurrentUserPageSetting(fSettings);
                bool[] oRecords = new bool[1];
                oRecords[0] = blnRet;
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


        public Models.Response Delete(int id, int usercontrol)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            bool blnSuccess = false;
            try
            {
                //TODO: 
                //  1. add code to validate the user authentication token with the usercontrol number
                //  2. add code to perform optimistic concurrency checks,  the updated field does not 
                //      pass through json serializaton
                
                if (id == 0)
                {
                    response.StatusCode = HttpStatusCode.OK;
                    // return the HTTP Response.
                    return response;
                }
                DAL.NGLcmPageData dalData = new DAL.NGLcmPageData(Parameters);
                blnSuccess = dalData.Delete(id);
               
                if (blnSuccess)
                {
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Errors = string.Format("Cannot delete the Localized Key Value Pair with control number {0}", id.ToString());

                }

            }
            catch (Exception ex)
            {
                // something went wrong. set the errors field of
                return new Models.Response(string.Format("There was an error deleting the content management page for control number {0}: {1}", id.ToString(), ex.Message));
            }
            // return the HTTP Response.
            return response;
        }

        #endregion
    }
}