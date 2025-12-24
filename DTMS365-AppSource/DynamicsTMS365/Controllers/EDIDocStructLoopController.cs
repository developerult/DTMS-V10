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
    /// EDIMasterDocStructElementController for EDI Master Doc Structure Element Rest API Controls
    /// </summary>
    /// <Remarks>
    /// Created By SRP on 3/2/2018
    /// </Remarks>
    public class EDIDocStructLoopController : NGLControllerBase
    {
        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.EDIDocStructLoopController";
        /// <summary>
        /// SourceClass Property for logging and error tracking
        /// </summary>
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;// Added by SRP on 03/2/18
        #endregion

        #region " Data Translation"
        /// <summary>
        /// Selecting a EDIDocStructLoop Model data by passing table records
        /// </summary>
        /// <param name="d">tblEDIDocStructLoop</param>
        /// <returns>returns EDIMDocStructLoop table data</returns>
        /// Modified by SRP on 3/2/2018
        private Models.EDIDocStructLoop selectModelData(LTS.tblEDIDocStructLoop d)
        {
            Models.EDIDocStructLoop modelDSLoopUpdated = new Models.EDIDocStructLoop();
            //skipping values for reference to foreign keys added by SRP 28/2/2018
            List<string> skipDSLoopUpdateds = new List<string> { "DSLoopUpdated", "tblEDILoop", "tblEDITPDocument" };
            string sMsg = "";
            modelDSLoopUpdated = (Models.EDIDocStructLoop)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelDSLoopUpdated, d, skipDSLoopUpdateds, ref sMsg);
            if (modelDSLoopUpdated != null) { modelDSLoopUpdated.setUpdated(d.DSLoopUpdated.ToArray()); }
            return modelDSLoopUpdated;
        }
        /// <summary>
        /// Selecting a EDIDocStructLoop Model data by passing table records
        /// </summary>
        /// <param name="d">tblEDIMasterDocStructLoops Model</param>
        /// <returns>returns EDIMDocStructLoop table data</returns>
        /// Modified by SRP on 3/2/2018
        private Models.EDIMDocStructLoop selectMasterModelData(LTS.tblEDIMasterDocStructLoops d)
        {
            Models.EDIMDocStructLoop modelDSLoopUpdated = new Models.EDIMDocStructLoop();
            //skipping values for reference to foreign keys added by SRP 28/2/2018
            List<string> skipDSLoopUpdateds = new List<string> { "MDSLoopUpdated" };
            string sMsg = "";
            modelDSLoopUpdated = (Models.EDIMDocStructLoop)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelDSLoopUpdated, d, skipDSLoopUpdateds, ref sMsg);
            if (modelDSLoopUpdated != null) { modelDSLoopUpdated.setUpdated(d.MDSLoopUpdated.ToArray()); }
            return modelDSLoopUpdated;
        }
        /// <summary>
        /// Selecting a EDIDocStructLoop Model data by passing table records
        /// </summary>
        /// <param name="d">tblEDIMasterDocStructLoops Model</param>
        /// <returns>returns EDIDocStructLoop table data</returns>
        /// Modified by SRP on 3/2/2018
        private Models.EDIDocStructLoop CopyToEDIDocStructLoop(LTS.tblEDIMasterDocStructLoops d)
        {
            Models.EDIDocStructLoop CopyEDIDocStructLoop = new Models.EDIDocStructLoop();
            if (d != null)
            {
                CopyEDIDocStructLoop.DSLoopControl = d.MDSLoopControl ;
                CopyEDIDocStructLoop.DSLoopTPDocControl = d.MDSLoopMasterDocControl ;
                CopyEDIDocStructLoop.DSLoopLoopControl = d.MDSLoopLoopControl ;
                CopyEDIDocStructLoop.DSLoopParentLoopID = d.MDSLoopParentLoopID ;
                CopyEDIDocStructLoop.DSLoopUsage = d.MDSLoopUsage ;
                CopyEDIDocStructLoop.DSLoopMinCount = d.MDSLoopMinCount ;
                CopyEDIDocStructLoop.DSLoopMaxCount = d.MDSLoopMaxCount ;
                CopyEDIDocStructLoop.DSLoopSeqIndex = d.MDSLoopSeqIndex ;
                CopyEDIDocStructLoop.DSLoopDisabled = d.MDSLoopDisabled ;
                CopyEDIDocStructLoop.DSLoopCreateDate = d.MDSLoopCreateDate;
                CopyEDIDocStructLoop.DSLoopCreateUser = d.MDSLoopCreateUser != null ? d.MDSLoopCreateUser : null;
                CopyEDIDocStructLoop.DSLoopModDate = Convert.ToDateTime(d.MDSLoopModDate);
                CopyEDIDocStructLoop.DSLoopModUser = d.MDSLoopModUser != null ? d.MDSLoopModUser : null;
                CopyEDIDocStructLoop.DSLoopUpdated = null;
            }
            return CopyEDIDocStructLoop;
        }
        /// <summary>
        /// select table data by passing EDIDocStructLoop Model
        /// </summary>
        /// <param name="d">EDIDocStructLoop Model</param>
        /// <returns>returns tblEDIDocStructLoop table data</returns>
        /// Modified by SRP on 3/7/18
        private LTS.tblEDIDocStructLoop selectLTSData(Models.EDIDocStructLoop d)
        {
            LTS.tblEDIDocStructLoop ltsEDIDocStructLoop = new LTS.tblEDIDocStructLoop();
            if (d != null)
            {
                List<string> skipEDIDocStructLoop = new List<string> { "DSLoopUpdated" };
                string sMsg = "";
                ltsEDIDocStructLoop = (LTS.tblEDIDocStructLoop)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsEDIDocStructLoop, d, skipEDIDocStructLoop, ref sMsg);
                if (ltsEDIDocStructLoop != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsEDIDocStructLoop.DSLoopUpdated = bupdated == null ? new byte[0] : bupdated;
                }
            }
            return ltsEDIDocStructLoop;
        }
        #endregion


        #region "Rest Services"

        /// <summary>
        /// This is used to return a collection of Model record of EDIDocStructLoop based on
        /// EDIDocStructLoop of Model passed to the method.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>An enumerable list of records</returns>
        /// Created by SRP on 3/7/18
        /// <remarks>The following data is also recognized: filters.</remarks>
        [HttpGet, ActionName("GetRecord")]
        public Models.Response GetRecord([System.Web.Http.FromBody]Models.EDIDocStructLoop data)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                Models.EDIDocStructLoop[] ediEDIDocStructLoop = new Models.EDIDocStructLoop[] { };
                DAL.EDIDocStructLoop dalData = new DAL.EDIDocStructLoop(Parameters);
                LTS.tblEDIDocStructLoop oChanges = selectLTSData(data);
                LTS.tblEDIDocStructLoop[] ltsEDIDocStructLoop = dalData.GetEDIDocStructLoop(oChanges);

                if (ltsEDIDocStructLoop != null && ltsEDIDocStructLoop.Count() > 0)
                {
                    count = ltsEDIDocStructLoop.Count();
                    ediEDIDocStructLoop = (from e in ltsEDIDocStructLoop
                                           select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                //getting element records from tblEDISegment
                response = new Models.Response(ediEDIDocStructLoop, count);
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
        /// This is used to return a collection of Model record of EDITP Document based on
        /// TPControl
        /// </summary>
        /// <param name="TPDocControl">The TPDocControl.</param>
        /// <returns>An enumerable list of records</returns>
        [HttpGet, ActionName("GetRecordbyTpDoc")]
        public Models.Response GetRecordbyTpDoc(string TPDocControl)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                Models.EDIDocStructLoop[] ediEDIDocStructLoop = new Models.EDIDocStructLoop[] { };
                Models.EDIMDocStructLoop[] ediEDIMDocStructLoop = new Models.EDIMDocStructLoop[] { };
                DAL.EDIDocStructLoop dalData = new DAL.EDIDocStructLoop(Parameters);
                //LTS.tblEDIDocStructLoop oChanges = selectLTSData(data);
                LTS.tblEDIDocStructLoop[] ltsEDIDocStructLoop = dalData.GetEDIDocStructLoopbyTPDoc(Convert.ToInt32(TPDocControl));
                //LTS.tblEDIMasterDocStructLoops[] ltsEDIDocStructLoops = dalData.GetEDIDocStructLoopbyTPDocument(Convert.ToInt32(TPDocControl));
                if (ltsEDIDocStructLoop != null && ltsEDIDocStructLoop.Count() > 0)
                {
                    count = ltsEDIDocStructLoop.Count();
                    ediEDIDocStructLoop = (from e in ltsEDIDocStructLoop
                                           select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                    response = new Models.Response(ediEDIDocStructLoop, count);
                }
                //getting element records from tblEDISegment
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
        /// This is used to return a collection of Model record of EDIDocStructLoop 
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
                Models.EDIDocStructLoop[] ediDocStructLoops = new Models.EDIDocStructLoop[] { };
                DAL.EDIDocStructLoop oAn = new DAL.EDIDocStructLoop(Parameters);
                LTS.tblEDIDocStructLoop[] ltsEDIDocStructLoops = oAn.GetEDIDocStructLoops(ref RecordCount, f);
                if (ltsEDIDocStructLoops != null && ltsEDIDocStructLoops.Count() > 0)
                {
                    count = ltsEDIDocStructLoops.Count();
                    ediDocStructLoops = (from e in ltsEDIDocStructLoops
                                         select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                //getting element records from tblEDIDocStructLoops
                response = new Models.Response(ediDocStructLoops, count);
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
        /// This is used to return a Model record of EDIDocStructLoop which has updated
        /// </summary>
        /// <param name="data">The Model.</param>
        /// <returns>Updated Single record</returns>
        /// <remarks>The following data is also recognized: Model.</remarks>
        [HttpPost, ActionName("UpdateEDIDocStructLoop")]
        public Models.Response UpdateEDIDocStructLoop([System.Web.Http.FromBody]Models.EDIDocStructLoop data)
        {
            // create a response message to send back
            var response = new Models.Response();//new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.EDIDocStructLoop dalData = new DAL.EDIDocStructLoop(Parameters);
                LTS.tblEDIDocStructLoop oChanges = selectLTSData(data);
                //updates the edi mdocstructelement
                LTS.tblEDIDocStructLoop oData = dalData.UpdateEDIDocStructLoop(oChanges);
                Models.EDIDocStructLoop[] oRecords = new Models.EDIDocStructLoop[1];
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
        /// This is used to return a Model record of EDIDocStructLoop which has Inserted
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns>Inserted Single record</returns>
        /// <remarks>The following data is also recognized: Model.</remarks>
        [HttpPost, ActionName("SaveMDocSegmentElementMap")]
        public Models.Response SaveMDocSegmentElementMap([System.Web.Http.FromBody]Models.EDIDocStructLoop dt)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.EDIDocStructLoop dalData = new DAL.EDIDocStructLoop(Parameters);
                LTS.tblEDIDocStructLoop oChanges = selectLTSData(dt);
                //insert the new edi MDocStructLoopSegment
                LTS.tblEDIDocStructLoop oData = dalData.InsertEDIDocStructLoop(oChanges);
                Models.EDIDocStructLoop[] oRecords = new Models.EDIDocStructLoop[1];
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
        /// </summary>
        /// <param name="data">The Model.</param>
        /// <returns>deleted Single record</returns>
        /// <remarks>The following data is also recognized: Model.</remarks>
        [HttpDelete, ActionName("DeleteRecord")]
        public bool DeleteRecord([System.Web.Http.FromBody]Models.EDIDocStructLoop data)
        {
            bool response = new bool();
            /*if (!authenticateController(bool response)) { return response; }*/
            try
            {
                int count = 0;
                int RecordCount = 0;
                Models.EDIDocStructLoop[] ediMDocStructLoops = new Models.EDIDocStructLoop[] { };
                DAL.EDIDocStructLoop dalData = new DAL.EDIDocStructLoop(Parameters);
                LTS.tblEDIDocStructLoop oChanges = selectLTSData(data);
                bool result = dalData.DeleteEDIDocStructLoops(oChanges);
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
        /// This is used to return true or false record of EDIDocStructLoop which has deleted or not
        /// based on Parent Loop
        /// </summary>
        /// <param name="data">The Model.</param>
        /// <returns>deleted Single record</returns>
        /// <remarks>The following data is also recognized: Model.</remarks>
        [HttpDelete, ActionName("DeleteParentLoopRecord")]
        public bool DeleteParentLoopRecord([System.Web.Http.FromBody]Models.EDIDocStructLoop data)
        {

            bool response = new bool();
            /*if (!authenticateController(bool response)) { return response; }*/
            try
            {
                Models.EDIDocStructLoop[] EDIMasterDocuments = new Models.EDIDocStructLoop[] { };
                DAL.EDIDocStructLoop dalData = new DAL.EDIDocStructLoop(Parameters);
                //LTS.tblEDIMasterDocument oChanges = selectLTSData(data);
                bool result = dalData.DeleteEDIParentLoop(Convert.ToInt32(data.DSLoopControl));
                //bool result = dalData.DeleteEDIMasterDocument(oChanges);

                //getting element records from tblEDIElmnt
                response = result;
            }
            catch (Exception ex)
            {
                //Error handler
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                //response.StatusCode = fault.StatusCode;
                //response.Errors = fault.formatMessage();
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
        [HttpGet, ActionName("CheckAvailableLoops")]
        public Models.Response CheckAvailableLoops(string MasterDocEDITControl)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                Models.EDIDocStructLoop[] ediDocStructLoops = new Models.EDIDocStructLoop[] { };
                DAL.EDIDocStructLoop dalData = new DAL.EDIDocStructLoop(Parameters);
                LTS.tblEDIDocStructLoop[] ltsEDIDocStructLoop = dalData.GetEDIStructLoopbyLoopId(Convert.ToInt32(MasterDocEDITControl));
                if (ltsEDIDocStructLoop != null && ltsEDIDocStructLoop.Count() > 0)
                {
                    count = ltsEDIDocStructLoop.Count();
                    ediDocStructLoops = (from e in ltsEDIDocStructLoop
                                          select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                response = new Models.Response(ediDocStructLoops, count);
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