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
using System.Web.Http.Routing.Constraints;

namespace DynamicsTMS365.Controllers
{
    public class cmPageDetailController : NGLControllerBase
    {

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.cmPageDetail";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        private Models.cmPageDetail selectModelData(LTS.cmPageDetail d)
        {
            Models.cmPageDetail modelRecord = new Models.cmPageDetail();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "PageDetUpdated","cmGroupSubType","cmGroupType","cmPage" };
                string sMsg = "";
                modelRecord = (Models.cmPageDetail)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) {
                    modelRecord.setUpdated(d.PageDetUpdated.ToArray());
                    if (d.PageDetParentID == 0) { modelRecord.parentId = null; } else { modelRecord.parentId = d.PageDetParentID; }                    
                }
            }
            return modelRecord;
        }

        private LTS.cmPageDetail selectLTSData(Models.cmPageDetail d)
        {
            LTS.cmPageDetail ltsRecord = new LTS.cmPageDetail();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "PageDetailUpdated" };
                string sMsg = "";
                ltsRecord = (LTS.cmPageDetail)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.PageDetUpdated = bupdated == null ? new byte[0] : bupdated;
                }
            }
            return ltsRecord;
        }

        #endregion

        #region " REST Services"       

        [HttpGet, ActionName("Get")]
        public Models.Response Get(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                Models.cmPageDetail[] pgDetails = new Models.cmPageDetail[] { };
                int count = 0;
                DAL.NGLcmPageData dalData = new DAL.NGLcmPageData(Parameters);
                LTS.cmPageDetail[] ltspgDetails = dalData.getPageDetailRecords(id);
                if (ltspgDetails != null && ltspgDetails.Count() > 0)
                {                  
                    count = ltspgDetails.Count(); //RecordCount contains the nunber of records in the database that matches the filters
                    pgDetails = (from e in ltspgDetails select selectModelData(e)).ToArray();
                }
                response = new Models.Response(pgDetails, count);
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

        [HttpPost, ActionName("PostPageDetUserVisibility")]
        public Models.Response PostPageDetUserVisibility([System.Web.Http.FromBody]Models.PageDetUserColumnSetting data)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                bool bVislible = false;
                bool blnRet = false;
                if (bool.TryParse(data.PageDetColumnValue, out bVislible))
                {
                    DAL.NGLcmPageData dalData = new DAL.NGLcmPageData(Parameters);
                    blnRet = dalData.savePageDetUserVisibility(data.PageDetPageControl, data.PageDetControl, bVislible);
                }
                bool[] oRecords = new bool[1] { blnRet };
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

        [HttpPost, ActionName("PostPageDetUserColumnWidth")]
        public Models.Response PostPageDetUserColumnWidth([System.Web.Http.FromBody]Models.PageDetUserColumnSetting data)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {           
                int iWidth = 0;
                bool blnRet = false;
                double dblTmp = 0;
                if (double.TryParse(data.PageDetColumnValue.ToString(),out dblTmp))
                {   
                    iWidth = (int)dblTmp; 
                    DAL.NGLcmPageData dalData = new DAL.NGLcmPageData(Parameters);
                    blnRet = dalData.savePageDetUserColumnWidth(data.PageDetPageControl, data.PageDetControl, iWidth);
                }               
                bool[] oRecords = new bool[1] { blnRet };
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

        [HttpPost, ActionName("PostPageDetUserColumnSequence")]
        public Models.Response PostPageDetUserColumnSequence([System.Web.Http.FromBody]Models.PageDetUserColumnSetting data)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int iSequenceNo = 0;
                bool blnRet = false;
                if (int.TryParse(data.PageDetColumnValue, out iSequenceNo))
                {
                    DAL.NGLcmPageData dalData = new DAL.NGLcmPageData(Parameters);
                    blnRet = dalData.savePageDetUserColumnSequence(data.PageDetPageControl, data.PageDetControl, iSequenceNo);
                }
                bool[] oRecords = new bool[1] { blnRet };
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

        /// <summary>
        /// May be called for the cmPage.aspx page in the future,  not currently used.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.2 on 04/30/2019
        ///     pass in the PageDetPageControl and the PageDetName to update all the content management
        ///     template settings for this pageDetField to use the phone number template
        /// </remarks>
        [HttpPost, ActionName("addPageDetailPhoneTemplate")]
        public Models.Response addPageDetailPhoneTemplate([System.Web.Http.FromBody]Models.PageDetKeys data)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                bool blnRet = false;              
                DAL.NGLcmPageData dalData = new DAL.NGLcmPageData(Parameters);
                blnRet = dalData.addPageDetailPhoneTemplate(data.PageDetPageControl, data.PageDetName);             
                bool[] oRecords = new bool[1] { blnRet };
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


        [HttpPost, ActionName("PostPageDetail")]
        public Models.Response PostPageDetail([System.Web.Http.FromBody]Models.cmPageDetail data)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                LTS.cmPageDetail oChanges = selectLTSData(data);
                DAL.NGLcmPageData dalData = new DAL.NGLcmPageData(Parameters);
                LTS.cmPageDetail oData = dalData.savePageDetailRecord(oChanges);              
                Models.cmPageDetail[] oRecords = new Models.cmPageDetail[1];
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
            return response;
        }

        [HttpPost, ActionName("PostAddDataElements")]
        public Models.Response PostAddDataElements([System.Web.Http.FromBody]Models.cmPageDetail data)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {              
                DAL.NGLcmPageData dalData = new DAL.NGLcmPageData(Parameters);
                bool blnRet = dalData.createAllPageDetailsForDataElement(data.PageDetPageControl, data.PageDetControl, data.PageDetDataElmtControl);
                bool[] rslts = new bool[1] { blnRet };
                response = new Models.Response(rslts, 1);
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

        [HttpPost, ActionName("PostCreatePageDetailFromField")]
        public Models.Response PostCreatePageDetailFromField([System.Web.Http.FromBody]Models.createPageDetailFromFieldFilters data)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {               
                DAL.NGLcmPageData dalData = new DAL.NGLcmPageData(Parameters);
                LTS.cmPageDetail oData = dalData.createPageDetailFromField(data.PageDetPageControl,data.PageDetParentID, data.PageDetElmtFieldControl);
                Models.cmPageDetail[] oRecords = new Models.cmPageDetail[1];
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
            return response;
        }

        [HttpPost, ActionName("PostUpdateDataElementFieldsNoReturn")]
        public Models.Response PostUpdateDataElementFieldsNoReturn(string filter)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {                
                DAL.NGLcmPageData dalData = new DAL.NGLcmPageData(Parameters);
                bool blnRet = dalData.updateDataElementFieldsNoReturn(filter);
                if (blnRet)
                {
                    response.StatusCode = HttpStatusCode.OK;
                } else
                {
                    response.StatusCode = HttpStatusCode.Conflict;
                    response.Errors = "UpdateDataElementFields " +  Utilities.getLocalizedMsg("Failed");
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
            return response;
        }



        [HttpPost, ActionName("PostHideAllControls")]
        public Models.Response PostHideAllControls([System.Web.Http.FromBody]string filter)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLcmPageData dalData = new DAL.NGLcmPageData(Parameters);
                int iPageControl = 0;
                int.TryParse(filter, out iPageControl);
                bool blnRet = false;
                if (iPageControl > 0)
                {
                    blnRet = dalData.PageMaintHideAllControls(iPageControl);
                }

                bool[] rslts = new bool[1] { blnRet };
                response = new Models.Response(rslts, 1);
                
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


        [HttpPost, ActionName("PostDelete")]
        public Models.Response PostDelete([System.Web.Http.FromBody]Models.cmPageDetail data)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                LTS.cmPageDetail oChanges = selectLTSData(data);
                DAL.NGLcmPageData dalData = new DAL.NGLcmPageData(Parameters);
                bool blnRet = dalData.DeleteDetail(oChanges);
                bool[] rslts = new bool[1] { blnRet };
                response = new Models.Response(rslts, 1);
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

        [HttpDelete, ActionName("ResetCurrentUserConfig")]
        public Models.Response ResetCurrentUserConfig(int id)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLcmPageData dalData = new DAL.NGLcmPageData(Parameters);
                bool blnRet = dalData.DeleteDetail(Parameters.UserControl, id);
                bool[] rslts = new bool[1] { blnRet };
                response = new Models.Response(rslts, 1);
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