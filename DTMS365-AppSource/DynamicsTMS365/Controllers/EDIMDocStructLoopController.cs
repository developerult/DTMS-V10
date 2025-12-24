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
    //Delete struct loop by passing this class as parameter
    public class teststructloop
    {
        public string MDSLoopControl { get; set; }
    }
    /// <summary>
    /// EDIMDocStructLoopController for EDI Master Doc Structure Loop Rest API Controls
    /// </summary>
    /// <Remarks>
    /// Created By SRP on 2/28/2018
    /// </Remarks>
    public class EDIMDocStructLoopController : NGLControllerBase
    {
        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.EDIMDocStructLoopController";
        /// <summary>
        /// SourceClass Property for logging and error tracking
        /// </summary>
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;// Added by SRP on 02/28/18
        #endregion

        #region " Data Translation"
        /// <summary>
        /// Selecting a EDIMDocStructLoop Model data by passing table records
        /// </summary>
        /// <param name="d">tblEDIMasterDocStructLoops</param>
        /// <returns>returns EDIMDocStructLoop table data</returns>
        /// Modified by SRP on 3/2/2018
        private Models.EDIMDocStructLoop selectModelData(LTS.tblEDIMasterDocStructLoops d)
        {
            Models.EDIMDocStructLoop modelmdocstructloop = new Models.EDIMDocStructLoop();
            //skipping values for reference to foreign keys added by SRP 28/2/2018
            List<string> skipEDIMDocStructLoop = new List<string> { "MDSLoopUpdated","tblEDIMasterDocStructLoop", "tblEDILoop", "tblEDIMasterDocument" };
            string sMsg = "";
            modelmdocstructloop = (Models.EDIMDocStructLoop)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelmdocstructloop, d, skipEDIMDocStructLoop, ref sMsg);
            if (modelmdocstructloop != null) { modelmdocstructloop.setUpdated(d.MDSLoopUpdated.ToArray()); }
            return modelmdocstructloop;
        }

        /// <summary>
        /// select table data by passing EDIMDocStructLoop Model
        /// </summary>
        /// <param name="d">EDIMDocStructLoop Model</param>
        /// <returns>returns tblEDIMasterDocStructLoops table data</returns>
        /// Modified by SRP on 3/7/18
        private LTS.tblEDIMasterDocStructLoops selectLTSData(Models.EDIMDocStructLoop d)
        {
            LTS.tblEDIMasterDocStructLoops ltsmdocstructloop = new LTS.tblEDIMasterDocStructLoops();
            if (d != null)
            {
                List<string> skipEDIMDocStructLoops = new List<string> { "MDSLoopUpdated" };
                string sMsg = "";
                ltsmdocstructloop = (LTS.tblEDIMasterDocStructLoops)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsmdocstructloop, d, skipEDIMDocStructLoops, ref sMsg);
                if (ltsmdocstructloop != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsmdocstructloop.MDSLoopUpdated = bupdated == null ? new byte[0] : bupdated;
                }
            }
            return ltsmdocstructloop;
        }
        #endregion


        #region "Rest Services"

        /// <summary>
        /// This is used to return a collection of Model record of EDIMDocStructLoop based on
        /// EDIMDocStructLoop of Model passed to the method.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>An enumerable list of records</returns>
        /// Created by SRP on 3/7/18
        /// <remarks>The following data is also recognized: filters.</remarks>
        [HttpGet, ActionName("GetRecord")]
        public Models.Response GetRecord([System.Web.Http.FromBody]Models.EDIMDocStructLoop data)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                Models.EDIMDocStructLoop[] ediMasterDocStructLoop = new Models.EDIMDocStructLoop[] { };
                DAL.NGLEDIMDocStructLoop dalData = new DAL.NGLEDIMDocStructLoop(Parameters);
                LTS.tblEDIMasterDocStructLoops oChanges = selectLTSData(data);
                LTS.tblEDIMasterDocStructLoops[] ltsEDIMasterDocStructLoop = dalData.GetEDIMasterDocStructLoop(oChanges.MDSLoopControl);
                if (ltsEDIMasterDocStructLoop != null && ltsEDIMasterDocStructLoop.Count() > 0)
                {
                    count = ltsEDIMasterDocStructLoop.Count();
                    ediMasterDocStructLoop = (from e in ltsEDIMasterDocStructLoop
                                             select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                response = new Models.Response(ediMasterDocStructLoop, count);
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

        /// Created by SRP on 3/7/18
        /// <summary>
        /// This is used to return a collection of Model record of EDIMDocStructLoop 
        /// </summary>
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
                Models.EDIMDocStructLoop[] EDIMDocStructLoops = new Models.EDIMDocStructLoop[] { };
                DAL.NGLEDIMDocStructLoop oAn = new DAL.NGLEDIMDocStructLoop(Parameters);
                LTS.tblEDIMasterDocStructLoops[] ltsMDocStructLoops = oAn.GetMDocStructLoops(ref RecordCount, f);
                if (ltsMDocStructLoops != null && ltsMDocStructLoops.Count() > 0)
                {
                    count = ltsMDocStructLoops.Count();
                    EDIMDocStructLoops = (from e in ltsMDocStructLoops
                                   select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                response = new Models.Response(EDIMDocStructLoops, count);
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
        /// Created by SRP on 2/3/18
        /// <summary>
        /// This is used to return a collection of Model record of EDIMDocStructLoop based on
        /// ParentLoopId
        /// </summary>
        /// <param name="ParentLoopId">The ParentLoopId.</param>
        /// <returns>An enumerable list of records</returns>
        [HttpGet, ActionName("GetRecordsbyLoop")]
        public Models.Response GetRecordsbyLoop(string ParentId)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                Models.EDIMDocStructLoop[] EDIMDocStructLoops = new Models.EDIMDocStructLoop[] { };
                DAL.NGLEDIMDocStructLoop oAn = new DAL.NGLEDIMDocStructLoop(Parameters);
                LTS.tblEDIMasterDocStructLoops[] ltsMDocStructLoops = oAn.GetEDIMasterDocStructLoopbyParent(Convert.ToInt32(ParentId));
                if (ltsMDocStructLoops != null && ltsMDocStructLoops.Count() > 0)
                {
                    count = ltsMDocStructLoops.Count();
                    EDIMDocStructLoops = (from e in ltsMDocStructLoops
                                          select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                response = new Models.Response(EDIMDocStructLoops, count);
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
        /// Created by SRP on 3/9/18
        /// <summary>
        /// This is used to return a Model record of EDIMDocStructLoop which has updated
        /// </summary>
        /// <param name="data">The Model.</param>
        /// <returns>Updated Single record</returns>
        /// <remarks>The following data is also recognized: Model.</remarks>
        [HttpPost, ActionName("UpdateMDocStructLoop")]
        public Models.Response UpdateMDocStructLoop([System.Web.Http.FromBody]Models.EDIMDocStructLoop data)
        {
            // create a response message to send back
            var response = new Models.Response();//new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLEDIMDocStructLoop dalData = new DAL.NGLEDIMDocStructLoop(Parameters);
                LTS.tblEDIMasterDocStructLoops oChanges = selectLTSData(data);
                //updates the edi segments
                LTS.tblEDIMasterDocStructLoops oData = dalData.UpdateMDocStructLoop(oChanges);
                Models.EDIMDocStructLoop[] oRecords = new Models.EDIMDocStructLoop[1];
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
        /// Created by SRP on 3/9/18
        /// <summary>
        /// This is used to return a Model record of EDIMDocStructLoop which has Inserted
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns>Inserted Single record</returns>
        /// <remarks>The following data is also recognized: Model.</remarks>
        [HttpPost, ActionName("SaveMDocStructLoop")]
        public Models.Response SaveMDocStructLoop([System.Web.Http.FromBody]Models.EDIMDocStructLoop dt)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLEDIMDocStructLoop dalData = new DAL.NGLEDIMDocStructLoop(Parameters);
                LTS.tblEDIMasterDocStructLoops oChanges = selectLTSData(dt);
                //insert the new edi segments
                LTS.tblEDIMasterDocStructLoops oData = dalData.InsertMDocStructLoop(oChanges);
                Models.EDIMDocStructLoop[] oRecords = new Models.EDIMDocStructLoop[1];
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
        /// Created by SRP on 3/7/18
        /// <summary>
        /// This is used to return true or false record of EDIDocStructLoop which has deleted or not
        /// based on MDSLoopControl using teststructloop class
        /// </summary>
        /// <param name="teststructloop">The teststructloop.</param>
        /// <returns>deleted Single record</returns>
        [HttpPost, ActionName("DeleteRecord")]
        public bool DeleteRecord(teststructloop d1)
        {
            bool response = new bool();
            /*if (!authenticateController(bool response)) { return response; }*/
            try
            {
                Models.EDIMDocStructLoop[] ediMDocStructLoops = new Models.EDIMDocStructLoop[] { };
                DAL.NGLEDIMDocStructLoop dalData = new DAL.NGLEDIMDocStructLoop(Parameters);
                //LTS.tblEDIMasterDocStructLoops oChanges = selectLTSData(data);
                bool result = dalData.DeleteEDIMasterDocStructLoops(Convert.ToInt32(d1.MDSLoopControl));
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
        /// Created by SRP on 3/16/18
        /// <summary>
        /// This is used to return true or false record of EDIMDocStructLoop which has deleted or not
        /// based on Parent Loop
        /// </summary>
        /// <param name="data">The Model.</param>
        /// <returns>deleted Single record</returns>
        /// <remarks>The following data is also recognized: Model.</remarks>
        [HttpDelete, ActionName("DeleteMParentLoopRecord")]
        public bool DeleteMParentLoopRecord([System.Web.Http.FromBody]Models.EDIMDocStructLoop data)
        {
            bool response = new bool();
            try
            {
                Models.EDIMDocStructLoop[] EDIMasterDocuments = new Models.EDIMDocStructLoop[] { };
                DAL.NGLEDIMDocStructLoop dalData = new DAL.NGLEDIMDocStructLoop(Parameters);
                bool result = dalData.DeleteEDIMasterParentLoop(Convert.ToInt32(data.MDSLoopControl));
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
        /// Created by SRP on 4/26/18
        /// <summary>
        /// This is used to check record of EDITPDocument is exist for EDIMasterDocument
        /// </summary>
        /// <param name="MasterDocEDITControl">The MasterDocEDITControl.</param>
        /// /// <param name="MasterDocInbound">The MasterDocInbound.</param>
        /// <returns>An enumerable list of records</returns>
        [HttpGet, ActionName("CheckAvailableLoop")]
        public Models.Response CheckAvailableLoop(string MasterDocEDITControl)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                Models.EDIMDocStructLoop[] ediMDocStructLoops = new Models.EDIMDocStructLoop[] { };
                DAL.NGLEDIMDocStructLoop dalData = new DAL.NGLEDIMDocStructLoop(Parameters);
                LTS.tblEDIMasterDocStructLoops[] ltsEDIMDocStructLoop = dalData.GetEDIStructLoopbyLoopIdParentId(Convert.ToInt32(MasterDocEDITControl));
                if (ltsEDIMDocStructLoop != null && ltsEDIMDocStructLoop.Count() > 0)
                {
                    count = ltsEDIMDocStructLoop.Count();
                    ediMDocStructLoops = (from e in ltsEDIMDocStructLoop
                                         select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                response = new Models.Response(ediMDocStructLoops, count);
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