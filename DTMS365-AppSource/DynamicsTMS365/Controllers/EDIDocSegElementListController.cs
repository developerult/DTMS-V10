using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using DAL = Ngl.FreightMaster.Data;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using LTS = Ngl.FreightMaster.Data.LTS;

namespace DynamicsTMS365.Controllers
{
    public class EDIDocSegElementListController : NGLControllerBase
    {
        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.EDIDocSegElementListController";
        /// <summary>
        /// SourceClass Property for logging and error tracking
        /// </summary>
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;// Added by SRP on 02/27/18
        #endregion

        #region " Data Translation"
        /// <summary>
        /// Selecting a EDIDocSegmentElements Model
        /// </summary>
        /// <param name="d"></param>
        /// <returns>Returns EDIDocSegmentElements</returns>
        /// <remarks>
        /// Modified by SRP on 2/27/18
        /// </remarks>
        private Models.EDIDocSegmentElement selectModelData(LTS.tblEDIDocSegmentElement d)
        {
            Models.EDIDocSegmentElement modelEDIDocSegmentElements = new Models.EDIDocSegmentElement();

            //skipping values for reference to foreign keys added by SRP 27/2/2018
            List<string> skipEDIDocSegmentElement = new List<string> { "DSEUpdated", "tblEDIType", "tblEDIElmnt", "tblEDISegment", "tblEDIFormattingFunction" };
            string sMsg = "";
            modelEDIDocSegmentElements = (Models.EDIDocSegmentElement)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelEDIDocSegmentElements, d, skipEDIDocSegmentElement, ref sMsg);
            if (modelEDIDocSegmentElements != null) { modelEDIDocSegmentElements.setUpdated(d.DSEUpdated.ToArray()); }
            return modelEDIDocSegmentElements;
        }

        private Models.EDIDocSegmentElement selectModelDatanew(LTS.spDocSegmentElementsListResult d)
        {
            Models.EDIDocSegmentElement modelEDIDocSegmentElements = new Models.EDIDocSegmentElement();
            List<string> skipEDIDocSegmentElement = new List<string> { };
            string sMsg = "";
            modelEDIDocSegmentElements = (Models.EDIDocSegmentElement)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelEDIDocSegmentElements, d, skipEDIDocSegmentElement, ref sMsg);

            //if (modelEDIDocSegmentElements != null) { modelEDIDocSegmentElements.setUpdated(d.DSEUpdated.ToArray()); }

            return modelEDIDocSegmentElements;
        }
        /// <summary>
        /// select LTSEDIDocSegmentElement
        /// </summary>
        /// <param name="d">EDIDocSegmentElement parameter</param>
        /// <returns>returns tblEDIDocSegmentElement</returns>
        /// <remarks>
        /// Modified by SRP on 2/27/18
        /// </remarks>
        private LTS.tblEDIDocSegmentElement selectLTSData(Models.EDIDocSegmentElement d)
        {
            LTS.tblEDIDocSegmentElement ltsEDIDocSegmentElements = new LTS.tblEDIDocSegmentElement();
            if (d != null)
            {
                List<string> skipEDIDocSegmentElement = new List<string> { "DSEUpdated" };
                string sMsg = "";
                ltsEDIDocSegmentElements = (LTS.tblEDIDocSegmentElement)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsEDIDocSegmentElements, d, skipEDIDocSegmentElement, ref sMsg);
                if (ltsEDIDocSegmentElements != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsEDIDocSegmentElements.DSEUpdated = bupdated == null ? new byte[0] : bupdated;
                }
            }
            return ltsEDIDocSegmentElements;
        }
        #endregion

        #region "Rest Services"
        /// <summary>
        /// Get EDIDocSegmentElement Records
        /// </summary>
        /// <returns>Returns Model Response</returns>
        /// <remarks>
        /// Modified by SRP on 2/27/18
        /// </remarks>
        [HttpGet, ActionName("GetRecords")]
        public Models.Response GetRecords(string filter)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                Models.EDIDocSegmentElement[] ediEDIDocSegmentElements = new Models.EDIDocSegmentElement[] { };
                DAL.NGLEDIDocSegmentElement dalData = new DAL.NGLEDIDocSegmentElement(Parameters);
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                LTS.spDocSegmentElementsListResult[] segList = null;
                count = segList.Count();
                var segArray = (from x in segList select selectModelDatanew(x)).ToArray();
                response = new Models.Response(segArray, count);
               
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