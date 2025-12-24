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
using DTran = Ngl.Core.Utility.DataTransformation;
using DModel = Ngl.FreightMaster.Data.Models;
using BLL = NGL.FM.BLL;
using System.Text;

//Added By LVV on 8/19/20 for v-8.3.0.001 - Task#20200817144456 - Modify the License File Integration Command Line Utility

namespace DynamicsTMS365.Controllers
{
    public class PageFooterMsgController : NGLControllerBase
    {

        #region " Properties"

        /// <summary> This property is used for logging and error tracking. </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.PageFooterMsgController";
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
                List<string> skipObjs = new List<string> { "PageUpdated", "tblFormList", "cmPageDetails", "cmPageMenus", "cmPageTemplateXrefs" };
                string sMsg = "";
                modelRecord = (Models.cmPage)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
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
                ltsRecord = (LTS.cmPage)DTran.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
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

        /// POST 	/API/objectcontroller{data}  : Create a new object or Update a the current object if the control number exists
        /// GET 	/API/objectcontroller/{id} : Get the object information where the control number = "id"
        /// PUT 	/API/objectcontroller/{data} : Update the object information stored in data
        /// DELETE	/API/objectcontroller/{id} : Delete object where the control number = "id"

        [HttpGet, ActionName("Get")]
        public string Get(int id)
        {
            string strRet = "";
            if (!authenticateCaller(ref strRet)) { return strRet; } //if there is an error put a number and special character at the beginning of the string and then we can look at it and parse it out later
            try
            {
                DModel.AllFilters f = new DModel.AllFilters();
                f.filterName = "PageControl";
                f.filterValue = id.ToString(); //Note: The id must always match a PageControl
                Models.cmPage[] records = new Models.cmPage[] { };
                DAL.NGLcmPageData oPg = new DAL.NGLcmPageData(Parameters);
                int RecordCount = 0;
                LTS.cmPage[] oData = oPg.GetPages(ref RecordCount, f);
                if (oData?.Count() > 0) { strRet = oData[0].PageFooterMsg; if (string.IsNullOrWhiteSpace(strRet)) { strRet = ""; } } //we don't want to return null so set it to ""
            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                strRet = "!Error!" + fault.formatMessage() + " (PageFooterMsgController)"; //if there is an error put a number and special character at the beginning of the string and then we can look at it and parse it out later
                return strRet;
            }
            return strRet;
        }


        private bool authenticateCaller(ref string strRet)
        {
            HttpContext httpContext = HttpContext.Current;
            bool blnSuccess = false;
            string authHeader = httpContext.Request.Headers["Authorization"];
            string Username = "";
            string Password = "";
            if (authHeader != null && authHeader.StartsWith("Basic"))
            {
                string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
                Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));
                int seperatorIndex = usernamePassword.IndexOf(':');
                Username = usernamePassword.Substring(0, seperatorIndex);
                Password = usernamePassword.Substring(seperatorIndex + 1);                
            }
            if (string.Equals(Username, "99999999") && string.Equals(Password, "NGLSystem")) { blnSuccess = true; }
            else
            {
                strRet = "!Error!" + Utilities.getLocalizedMsg("E_AccessDenied") + " (PageFooterMsgController)"; //if there is an error put a number and special character at the beginning of the string and then we can look at it and parse it out later
                blnSuccess = false;
            }
            return blnSuccess;
        }

        #endregion
    }
}