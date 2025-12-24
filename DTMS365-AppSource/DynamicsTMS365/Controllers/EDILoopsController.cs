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
    /// <summary>
    /// EDILoopsController for EDILoops Rest API Controls
    /// </summary>
    /// <Remarks>
    /// Created By SRP on 2/26/2018
    /// </Remarks>
    public class EDILoopsController : NGLControllerBase
    {
        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.EDILoopsController";
        /// <summary>
        /// SourceClass Property for logging and error tracking
        /// </summary>
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;// Added by SRP on 02/26/18
        #endregion

        #region " Data Translation"
        /// <summary>
        /// Selecting a EDILoop Model data by passing table records
        /// </summary>
        /// <param name="d">The table data.</param>
        /// <returns>Returns EDILoop Type</returns>
        /// Modified by SRP on 2/26/18
        private Models.EDILoop selectModelData(LTS.tblEDILoop d)
        {
            Models.EDILoop modelloops = new Models.EDILoop();
            //skipping values for reference to foreign keys added by SRP 26/2/2018
            List<string> skipEDILoops = new List<string> { "LoopUpdated" };
            string sMsg = "";
            modelloops = (Models.EDILoop)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelloops, d, skipEDILoops, ref sMsg);
            if (modelloops != null) { modelloops.setUpdated(d.LoopUpdated.ToArray()); }
            return modelloops;
        }
        /// <summary>
        /// select table data by passing EDILoop Model
        /// </summary>
        /// <param name="d">EDILoop Model</param>
        /// <returns>returns tblEDILoop table data</returns>
        /// Modified by SRP on 2/26/18
        private LTS.tblEDILoop selectLTSData(Models.EDILoop d)
        {
            LTS.tblEDILoop ltsloops = new LTS.tblEDILoop();
            if (d != null)
            {
                List<string> skipEDILoops = new List<string> { "LoopUpdated" };
                string sMsg = "";
                ltsloops = (LTS.tblEDILoop)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsloops, d, skipEDILoops, ref sMsg);
                if (ltsloops != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsloops.LoopUpdated = bupdated == null ? new byte[0] : bupdated;
                }
            }
            return ltsloops;
        }
        #endregion

        #region "Rest Services"
        /// Created by SRP on 2/26/18
        /// <summary>
        /// This is used to return a collection of Model record of EDILoop based on
        /// Loop Name and Loop Description passed to the method.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>An enumerable list of records</returns>
        /// <remarks>The following data is also recognized: filters.</remarks>s>

        [HttpGet, ActionName("GetRecords")]
        public Models.Response GetRecords(string filter)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                Models.EDILoop[] ediloops = new Models.EDILoop[] { };
                DAL.NGLEDILoop oAn = new DAL.NGLEDILoop(Parameters);
                LTS.tblEDILoop[] ltsRet = oAn.GetEDILoops(ref RecordCount, f);
                if (ltsRet != null && ltsRet.Count() > 0)
                {
                    count = ltsRet.Count();
                    ediloops = (from e in ltsRet
                                    select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                //getting element records from tblEDILoop
                response = new Models.Response(ediloops, count);
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

        /// Created by SRP on 2/26/18
        /// <summary>
        /// This is used to return a Model record of EDILoop which has updated
        /// </summary>
        /// <param name="data">The Model.</param>
        /// <returns>Updated Single record</returns>
        /// <remarks>The following data is also recognized: Model.</remarks>
        [HttpPost, ActionName("PostSave")]
        public Models.Response PostSave([System.Web.Http.FromBody]Models.EDILoop data)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLEDILoop dalData = new DAL.NGLEDILoop(Parameters);
                LTS.tblEDILoop oChanges = selectLTSData(data);
                //updates the edi loops
                LTS.tblEDILoop oData = dalData.UpdateEDILoop(oChanges);
                Models.EDILoop[] oRecords = new Models.EDILoop[1];
                oRecords[0] = selectModelData(oData);
                response = new Models.Response(oRecords, 1);
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

        /// Created by SN on 2/21/18
        /// <summary>
        /// This is used to return a Model record of EDILoop which has Inserted
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns>Inserted Single record</returns>
        /// <remarks>The following data is also recognized: Model.</remarks>
        [HttpPost, ActionName("SaveEDILoop")]
        public Models.Response SaveEDILoop([System.Web.Http.FromBody]Models.EDILoop dt)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLEDILoop dalData = new DAL.NGLEDILoop(Parameters);
                LTS.tblEDILoop oChanges = selectLTSData(dt);
                //insert the new edi loop
                LTS.tblEDILoop oData = dalData.InsertEDILoop(oChanges);
                Models.EDILoop[] oRecords = new Models.EDILoop[1];
                oRecords[0] = selectModelData(oData);
                response = new Models.Response(oRecords, 1);
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
        /// Created by SRP on 2/26/18
        /// <summary>
        /// This is used to return true or false record of EDILoop which has deleted or not
        /// </summary>
        /// <param name="data">The Model.</param>
        /// <returns>deleted Single record</returns>
        /// <remarks>The following data is also recognized: Model.</remarks>
        [HttpDelete, ActionName("DeleteRecord")]
        public bool DeleteRecord([System.Web.Http.FromBody]Models.EDILoop data)
        {
            bool response = new bool();
            /*if (!authenticateController(bool response)) { return response; }*/
            try
            {
                Models.EDILoop[] ediEDIDocumentTypes = new Models.EDILoop[] { };
                DAL.NGLEDILoop dalData = new DAL.NGLEDILoop(Parameters);
                LTS.tblEDILoop oChanges = selectLTSData(data);
                bool result = dalData.DeleteEDILoop(oChanges);
                response = result;
            }
            catch (Exception ex)
            {
                //Error handler
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                return response;
            }
            return response;
        }

        #endregion
    }
}