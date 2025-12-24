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
    
    //For Saving records to EDIMasterDocStructSegment by passing this class  as parameter
    public class segmentsave
    {
        //public Models.EDIMasterDocStructSegment dt { get; set; }
        public string SegmentControl { get; set; }
        public string segmentlength { get; set; }
        public string MDSSegMDSLoopControl { get; set; }
    }
    //For deleting records from EDIMasterDocStructSegment by passing this class  as parameter
    public class teststructsegment
    {
        public string DSSegControl { get; set; }
    }
    /// <summary>
    /// EDIDocStructSegmentController for EDI Master Doc Structure Segment Rest API Controls
    /// </summary>
    /// <Remarks>
    /// Created By SRP on 3/2/2018
    /// </Remarks>
    public class EDIDocStructSegmentController : NGLControllerBase
    {
        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.EDIDocStructSegmentController";
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
        /// Selecting a EDIDocStructSegment Model data by passing table records
        /// </summary>
        /// <param name="d">tblEDIDocStructSegment</param>
        /// <returns>returns EDIDocStructSegment table data</returns>
        /// Modified by SRP on 3/7/2018
        private Models.EDIDocStructSegment selectModelData(LTS.tblEDIDocStructSegment d)
        {
            Models.EDIDocStructSegment modelEDIDocStructSegment = new Models.EDIDocStructSegment();
            //skipping values for reference to foreign keys added by SRP 28/2/2018
            List<string> skipEDIDocStructElement = new List<string> { "DSSegUpdated", "tblEDIDocStructLoop", "tblEDISegment" };
            string sMsg = "";
            modelEDIDocStructSegment = (Models.EDIDocStructSegment)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelEDIDocStructSegment, d, skipEDIDocStructElement, ref sMsg);
            if (modelEDIDocStructSegment != null) { modelEDIDocStructSegment.setUpdated(d.DSSegUpdated.ToArray()); }
            return modelEDIDocStructSegment;
        }
        /// <summary>
        /// Selecting a EDIDocStructSegment Model data by passing table records
        /// </summary>
        /// <param name="d">vw_GetSegmentByLoop</param>
        /// <returns>returns EDIDocStructSegment table data</returns>
        /// Modified by SRP on 3/7/2018
        private Models.EDIDocStructSegment selectModelDataGetMDocSegmentsByLoop(LTS.vw_GetSegmentByLoop d)
        {
            Models.EDIDocStructSegment modelEDIDocSegmentElements = new Models.EDIDocStructSegment();
            List<string> skipEDIDocSegmentElement = new List<string> { };// { "MDSElementUpdated", "tblEDIDataType", "tblEDIElmnt", "tblEDIMasterDocStructSegments", "_tblEDIMasterDocStructSegment", "tblEDIMasterDocStructSegment", "tblEDIMasterDocStructElements" };
            string sMsg = "";
            modelEDIDocSegmentElements = (Models.EDIDocStructSegment)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelEDIDocSegmentElements, d, skipEDIDocSegmentElement, ref sMsg);

            //if (modelEDIDocSegmentElements != null) { modelEDIDocSegmentElements.setUpdated(d.DSEUpdated.ToArray()); }

            return modelEDIDocSegmentElements;
        }
        /// <summary>
        /// Selecting a EDIDocStructElement Model data by passing table records
        /// </summary>
        /// <param name="d">vwGetSegmentElementsBySegmentLoop</param>
        /// <returns>returns EDIDocStructElement table data</returns>
        /// Modified by SRP on 3/7/2018
        private Models.EDIDocStructElement selectModelDataElementLoop(LTS.vwGetSegmentElementsBySegmentLoop d)
        {
            Models.EDIDocStructElement modelEDIDocSegmentElements = new Models.EDIDocStructElement();
            List<string> skipEDIDocSegmentElement = new List<string> { "DSElementUpdated", "tblEDIDataType", "tblEDIElmnt", "tblEDIMasterDocStructSegments", "_tblEDIMasterDocStructSegment", "tblEDIMasterDocStructSegment", "tblEDIMasterDocStructElements" };
            string sMsg = "";
            modelEDIDocSegmentElements = (Models.EDIDocStructElement)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelEDIDocSegmentElements, d, skipEDIDocSegmentElement, ref sMsg);

            return modelEDIDocSegmentElements;
        }
        /// <summary>
        /// Selecting a EDIDocStructElement Model data by passing table records
        /// </summary>
        /// <param name="d">spPopulateEDITPLoopSegmentElementsResult Model</param>
        /// <returns>returns EDIDocStructElement table data</returns>
        /// Modified by SRP on 3/7/2018
        private Models.EDIDocSegmentElement selectModelDataElement(LTS.spPopulateEDITPLoopSegmentElementsResult d)
        {
            Models.EDIDocSegmentElement modelEDIDocSegmentElements = new Models.EDIDocSegmentElement();
            List<string> skipEDIDocSegmentElement = new List<string> { };
            string sMsg = "";
            modelEDIDocSegmentElements = (Models.EDIDocSegmentElement)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelEDIDocSegmentElements, d, skipEDIDocSegmentElement, ref sMsg);

            //if (modelEDIDocSegmentElements != null) { modelEDIDocSegmentElements.setUpdated(d.DSEUpdated.ToArray()); }

            return modelEDIDocSegmentElements;
        }
        /// <summary>
        /// select table data by passing EDIDocStructSegment Model
        /// </summary>
        /// <param name="d">EDIDocStructSegment Model</param>
        /// <returns>returns tblEDIDocStructSegment table data</returns>
        /// Modified by SRP on 3/7/18
        private LTS.tblEDIDocStructSegment selectLTSData(Models.EDIDocStructSegment d)
        {
            LTS.tblEDIDocStructSegment ltsEDIDocStructSegment = new LTS.tblEDIDocStructSegment();
            if (d != null)
            {
                List<string> skipEDIDocStructSegment = new List<string> { "DSSegUpdated" };
                string sMsg = "";
                ltsEDIDocStructSegment = (LTS.tblEDIDocStructSegment)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsEDIDocStructSegment, d, skipEDIDocStructSegment, ref sMsg);
                if (ltsEDIDocStructSegment != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsEDIDocStructSegment.DSSegUpdated = bupdated == null ? new byte[0] : bupdated;
                }
            }
            return ltsEDIDocStructSegment;
        }
        #endregion

        #region "Rest Services"

        /// <summary>
        /// This is used to return a collection of Model record of EDIDocStructSegment based on
        /// EDIDocStructSegment of Model passed to the method.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>An enumerable list of records</returns>
        /// Created by SRP on 3/7/18
        /// <remarks>The following data is also recognized: filters.</remarks>
        [HttpGet, ActionName("GetRecord")]
        public Models.Response GetRecord([System.Web.Http.FromBody]Models.EDIDocStructSegment data)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                Models.EDIDocStructSegment[] ediDocStructSegment = new Models.EDIDocStructSegment[] { };
                DAL.EDIDocStructSegment dalData = new DAL.EDIDocStructSegment(Parameters);
                LTS.tblEDIDocStructSegment oChanges = selectLTSData(data);
                LTS.tblEDIDocStructSegment[] ltsDocStructSegment = dalData.GetEDIDocStructSegment(oChanges);
                if (ltsDocStructSegment != null && ltsDocStructSegment.Count() > 0)
                {
                    count = ltsDocStructSegment.Count();
                    ediDocStructSegment = (from e in ltsDocStructSegment
                                              select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                //getting element records from tblEDISegment
                response = new Models.Response(ediDocStructSegment, count);
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

        /// Created by SRP on 3/2/18
        /// <summary>
        /// This is used to return a collection of Model record of EDIDocStructSegment 
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
                Models.EDIDocStructSegment[] ediEDIDocStructSegment = new Models.EDIDocStructSegment[] { };
                DAL.EDIDocStructSegment oAn = new DAL.EDIDocStructSegment(Parameters);
                LTS.tblEDIDocStructSegment[] ltsEDIDocStructSegment = oAn.GetEDIDocStructSegments(ref RecordCount, f);
                if (ltsEDIDocStructSegment != null && ltsEDIDocStructSegment.Count() > 0)
                {
                    count = ltsEDIDocStructSegment.Count();
                    ediEDIDocStructSegment = (from e in ltsEDIDocStructSegment
                                            select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                //getting element records from tblEDIDocStructSegment
                response = new Models.Response(ediEDIDocStructSegment, count);
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
        /// Created by SRP on 3/2/18
        /// <summary>
        /// This is used to return a Model record of EDIDocStructSegment which has updated
        /// </summary>
        /// <param name="data">The Model.</param>
        /// <returns>Updated Single record</returns>
        /// <remarks>The following data is also recognized: Model.</remarks>
        [HttpPost, ActionName("UpdateEDIDocStructSegment")]
        public Models.Response UpdateEDIDocStructSegment([System.Web.Http.FromBody]Models.EDIDocStructSegment data)
        {
            // create a response message to send back
            var response = new Models.Response();//new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.EDIDocStructSegment dalData = new DAL.EDIDocStructSegment(Parameters);
                LTS.tblEDIDocStructSegment oChanges = selectLTSData(data);
                //updates the edi DocStructSegment
                LTS.tblEDIDocStructSegment oData = dalData.UpdateEDIDocStructSegment(oChanges);
                Models.EDIDocStructSegment[] oRecords = new Models.EDIDocStructSegment[1];
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
        /// This is used to return a Model record of EDIDocStructSegment which has Inserted
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns>Inserted Single record</returns>
        /// <remarks>The following data is also recognized: Model.</remarks>
        [HttpPost, ActionName("SaveEDIDocStructSegment")]
        public Models.Response SaveEDIDocStructSegment([System.Web.Http.FromBody]Models.EDIDocStructSegment dt)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.EDIDocStructSegment dalData = new DAL.EDIDocStructSegment(Parameters);
                LTS.tblEDIDocStructSegment oChanges = selectLTSData(dt);
                //insert the new edi DocStructSegment
                LTS.tblEDIDocStructSegment oData = dalData.InsertEDIDocStructSegment(oChanges);
                Models.EDIDocStructSegment[] oRecords = new Models.EDIDocStructSegment[1];
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
        /// Created by SRP on 3/2/18
        /// <summary>
        /// This is used to return a collection of Model record of EDIDocStructSegment based on
        /// loopControl
        /// </summary>
        /// <param name="loopControl">The loopControl.</param>
        /// <returns>An enumerable list of records</returns>
        [HttpGet, ActionName("GetRecordSegmentByLoop")]
        public Models.Response GetRecordSegmentByLoop(string loopControl)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                DAL.EDIDocStructSegment dalData = new DAL.EDIDocStructSegment(Parameters);
                Models.EDIDocStructSegment[] EDIStructLoops = new Models.EDIDocStructSegment[] { };
                LTS.vw_GetSegmentByLoop[] ltsStructsegments = null;
                //ltsStructsegments = dalData.GetSegmentsByLoops().Where(model => model.DSSegDSLoopControl.Equals(Convert.ToInt32(loopControl))).ToArray();
                ltsStructsegments = dalData.GetSegmentsByLoops().Where(model => model.DSSegDSLoopControl.Equals(Convert.ToInt32(loopControl))).ToArray();
                if (ltsStructsegments != null && ltsStructsegments.Count() > 0)
                {
                    count = ltsStructsegments.Count();
                    EDIStructLoops = (from e in ltsStructsegments
                                          select selectModelDataGetMDocSegmentsByLoop(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                response = new Models.Response(EDIStructLoops, count);
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
        /// Created by SRP on 3/2/18
        /// <summary>
        /// This is used to return a collection of Model record of EDIDocStructSegment based on
        /// SegmentControl and loopControl
        /// </summary>
        /// <param name="segment">The loop.</param>
        /// <param name="loop">The loop.</param>
        /// <returns>An enumerable list of records</returns>
        [HttpGet, ActionName("GetRecordSegmentLoopElement")]
        public Models.Response GetRecordSegmentLoopElement(string segment, string loop)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                DAL.EDIDocStructSegment dalData = new DAL.EDIDocStructSegment(Parameters);
                Models.EDIDocStructElement[] EDIMasterDocStructElementLoops = new Models.EDIDocStructElement[] { };
                LTS.vwGetSegmentElementsBySegmentLoop[] ltsEDIMasterDocStructElementsegments = null;
                ltsEDIMasterDocStructElementsegments = dalData.GetSegmentElementbySegmentLoop().Where(model => model.DSSegSegmentControl.Equals(Convert.ToInt32(segment)) && model.DSSegDSLoopControl.Equals(Convert.ToInt32(loop))).ToArray();
                if (ltsEDIMasterDocStructElementsegments != null && ltsEDIMasterDocStructElementsegments.Count() > 0)
                {
                    count = ltsEDIMasterDocStructElementsegments.Count();
                    EDIMasterDocStructElementLoops = (from e in ltsEDIMasterDocStructElementsegments
                                          select selectModelDataElementLoop(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                response = new Models.Response(EDIMasterDocStructElementLoops, count);
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
        /// Created by SRP on 3/2/18
        /// <summary>
        /// This is used to return a collection of Model record of EDIDocStructSegment based on
        /// SegmentControl and loopControl
        /// </summary>
        /// <param name="segment">The loop.</param>
        /// <param name="loop">The loop.</param>
        /// <returns>An enumerable list of records</returns>
        [HttpGet, ActionName("GetSegmentRecordsbySegmentLoop")]
        public Models.Response GetSegmentRecordsbySegmentLoop(string Segment, string Loop)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                Models.EDIDocStructSegment[] ediEDIMasterDocStructSegment = new Models.EDIDocStructSegment[] { };
                DAL.EDIDocStructSegment dalData = new DAL.EDIDocStructSegment(Parameters);
                //LTS.tblEDIMasterDocStructSegment oChanges = selectLTSData(data);
                LTS.tblEDIDocStructSegment[] ltsEDIMasterDocStructSegment = null;
                ltsEDIMasterDocStructSegment = dalData.GetStructSegmentByLoopSegment().Where(model => model.DSSegSegmentControl.Equals(Convert.ToInt32(Segment)) && model.DSSegDSLoopControl.Equals(Convert.ToInt32(Loop))).ToArray();

                if (ltsEDIMasterDocStructSegment != null && ltsEDIMasterDocStructSegment.Count() > 0)
                {
                    count = ltsEDIMasterDocStructSegment.Count();
                    ediEDIMasterDocStructSegment = (from e in ltsEDIMasterDocStructSegment
                                                    select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                //getting element records from tblEDISegment
                response = new Models.Response(ediEDIMasterDocStructSegment, count);
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
        /// This is used to return a Model record of EDIDocStructSegment which has Inserted
        /// </summary>
        /// <param name="segmentsave">The segmentsave class.</param>
        /// <returns>populate data using sp is spPopulateEDITPLoopSegmentElements</returns>
        /// <remarks>The following dt1 is also recognized: Model using this class passing parameter and 
        /// P.</remarks>
        [HttpPost, ActionName("SaveDocStructLoopSegmentElement")]
        public Models.Response SaveDocStructLoopSegmentElement(segmentsave dt1)
        {
            int count = 0;
            string MDSSegMDSLoopControl = dt1.MDSSegMDSLoopControl;
            string segmentlength = dt1.segmentlength;
            string SegmentControl = dt1.SegmentControl;
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.EDIDocStructSegment dalData = new DAL.EDIDocStructSegment(Parameters);
                LTS.spPopulateEDITPLoopSegmentElementsResult[] segList = null;
                segList = dalData.PopulateEDILoopSegmentElements(SegmentControl, segmentlength, MDSSegMDSLoopControl).ToArray();
                //segList = dalData.GetDocSegmentElementList().ToArray();
                count = segList.Count();
                var segArray = (from x in segList select selectModelDataElement(x)).ToArray();
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
        /// Created by SRP on 3/7/18
        /// <summary>
        /// This is used to return true or false record of EDIDocStructSegment which has deleted or not
        /// </summary>
        /// <param name="teststructsegment">The teststructsegment class.</param>
        /// <returns>true or false</returns>
        [HttpPost, ActionName("DeleteRecord")]
        public bool DeleteRecord(teststructsegment d1)
        {
            bool response = new bool();
            /*if (!authenticateController(bool response)) { return response; }*/
            try
            {
                Models.EDIDocStructSegment[] ediMDocStructLoops = new Models.EDIDocStructSegment[] { };
                DAL.EDIDocStructSegment dalData = new DAL.EDIDocStructSegment(Parameters);
                bool result = dalData.DeleteEDIDocStructSegments(Convert.ToInt32(d1.DSSegControl));
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