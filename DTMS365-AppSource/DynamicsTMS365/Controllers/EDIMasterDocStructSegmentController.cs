using DynamicsTMS365.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using DAL = Ngl.FreightMaster.Data;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using LTS = Ngl.FreightMaster.Data.LTS;

namespace DynamicsTMS365.Controllers
{

    //delete EDIMasterDocStructSegment record by passing this class as parameter 
    public class testMstructsegment
    {
        public string MDSSegControl { get; set; }
    }
    //save EDIMasterDocStructSegment record by passing this class as parameter 
    public class test
    {
        public string SegmentControl { get; set; }
        public string segmentlength { get; set; }
        public string MDSSegMDSLoopControl { get; set; }

    }
    /// <summary>
    /// EDIMasterDocStructSegmentController for EDI Master Doc Structure Segment Rest API Controls
    /// </summary>
    /// <Remarks>
    /// Created By SRP on 2/28/2018
    /// </Remarks>
    public class EDIMasterDocStructSegmentController : NGLControllerBase
    {
        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.EDIMasterDocStructSegmentController";
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
        /// Selecting a EDIMasterDocStructSegment Model data by passing table records
        /// </summary>
        /// <param name="d">tblEDIMasterDocStructSegment</param>
        /// <returns>returns EDIMasterDocStructSegment table data</returns>
        /// Modified by SRP on 3/7/2018
        private Models.EDIMasterDocStructSegment selectModelData(LTS.tblEDIMasterDocStructSegment d)
        {
            Models.EDIMasterDocStructSegment modelmdocstructsegment = new Models.EDIMasterDocStructSegment();
            //skipping values for reference to foreign keys added by SRP 28/2/2018
            List<string> skipEDIMasterDocStructSegments = new List<string> { "MDSSegUpdated", "tblEDIMasterDocStructSegment", "tblEDIMasterDocStructLoops", "tblEDISegment" };
            string sMsg = "";
            modelmdocstructsegment = (Models.EDIMasterDocStructSegment)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelmdocstructsegment, d, skipEDIMasterDocStructSegments, ref sMsg);
            if (modelmdocstructsegment != null) { modelmdocstructsegment.setUpdated(d.MDSSegUpdated.ToArray()); }
            return modelmdocstructsegment;
        }
        /// <summary>
        /// Selecting a EDIDocSegmentElement Model data by passing table records
        /// </summary>
        /// <param name="d">spDocSegmentElementsListResult</param>
        /// <returns>returns EDIDocSegmentElement table data</returns>
        /// Modified by SRP on 3/7/2018
        private Models.EDIDocSegmentElement selectModelDatanew(LTS.spDocSegmentElementsListResult d)
        {
            Models.EDIDocSegmentElement modelEDIDocSegmentElements = new Models.EDIDocSegmentElement();
            List<string> skipEDIDocSegmentElement = new List<string> { };
            string sMsg = "";
            modelEDIDocSegmentElements = (Models.EDIDocSegmentElement)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelEDIDocSegmentElements, d, skipEDIDocSegmentElement, ref sMsg);

            return modelEDIDocSegmentElements;
        }
        /// <summary>
        /// Selecting a EDIDocSegmentElement Model data by passing table records
        /// </summary>
        /// <param name="d">spPopulateEDIMasterLoopSegmentElementsResult</param>
        /// <returns>returns EDIDocSegmentElement table data</returns>
        /// Modified by SRP on 3/7/2018
        private Models.EDIDocSegmentElement selectModelDataElement(LTS.spPopulateEDIMasterLoopSegmentElementsResult d)
        {
            Models.EDIDocSegmentElement modelEDIDocSegmentElements = new Models.EDIDocSegmentElement();
            List<string> skipEDIDocSegmentElement = new List<string> { };
            string sMsg = "";
            modelEDIDocSegmentElements = (Models.EDIDocSegmentElement)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelEDIDocSegmentElements, d, skipEDIDocSegmentElement, ref sMsg);

            return modelEDIDocSegmentElements;
        }
        /// <summary>
        /// Selecting a EDIMasterDocStructElement Model data by passing table records
        /// </summary>
        /// <param name="d">tblEDIMasterDocStructElement</param>
        /// <returns>returns EDIMasterDocStructElement table data</returns>
        /// Modified by SRP on 3/7/2018
        private Models.EDIMasterDocStructElement selectModelDataElementNew(LTS.tblEDIMasterDocStructElement d)
        {
            Models.EDIMasterDocStructElement modelEDIDocSegmentElements = new Models.EDIMasterDocStructElement();
            List<string> skipEDIDocSegmentElement = new List<string> { "MDSElementUpdated", "tblEDIDataType", "tblEDIElmnt", "tblEDIMasterDocStructSegments", "_tblEDIMasterDocStructSegment", "tblEDIMasterDocStructSegment", "tblEDIMasterDocStructElements" };
            string sMsg = "";
            modelEDIDocSegmentElements = (Models.EDIMasterDocStructElement)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelEDIDocSegmentElements, d, skipEDIDocSegmentElement, ref sMsg);

            return modelEDIDocSegmentElements;
        }
        /// <summary>
        /// Selecting a EDIMasterDocStructElement Model data by passing table records
        /// </summary>
        /// <param name="d">vwGetElementsBySegmentLoop</param>
        /// <returns>returns EDIMasterDocStructElement table data</returns>
        /// Modified by SRP on 3/7/2018
        private Models.EDIMasterDocStructElement selectModelDataElementLoop(LTS.vwGetElementsBySegmentLoop d)
        {
            Models.EDIMasterDocStructElement modelEDIDocSegmentElements = new Models.EDIMasterDocStructElement();
            List<string> skipEDIDocSegmentElement = new List<string> { "MDSElementUpdated", "tblEDIDataType", "tblEDIElmnt", "tblEDIMasterDocStructSegments", "_tblEDIMasterDocStructSegment", "tblEDIMasterDocStructSegment", "tblEDIMasterDocStructElements" };
            string sMsg = "";
            modelEDIDocSegmentElements = (Models.EDIMasterDocStructElement)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelEDIDocSegmentElements, d, skipEDIDocSegmentElement, ref sMsg);

            return modelEDIDocSegmentElements;
        }
        /// <summary>
        /// Selecting a EDIMasterDocStructSegment Model data by passing table records
        /// </summary>
        /// <param name="d">vwGetMDocSegmentsByLoop</param>
        /// <returns>returns EDIMasterDocStructSegment table data</returns>
        /// Modified by SRP on 3/7/2018
        private Models.EDIMasterDocStructSegment selectModelDataGetMDocSegmentsByLoop(LTS.vwGetMDocSegmentsByLoop d)
        {
            Models.EDIMasterDocStructSegment modelEDIDocSegmentElements = new Models.EDIMasterDocStructSegment();
            List<string> skipEDIDocSegmentElement = new List<string> { };// { "MDSElementUpdated", "tblEDIDataType", "tblEDIElmnt", "tblEDIMasterDocStructSegments", "_tblEDIMasterDocStructSegment", "tblEDIMasterDocStructSegment", "tblEDIMasterDocStructElements" };
            string sMsg = "";
            modelEDIDocSegmentElements = (Models.EDIMasterDocStructSegment)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelEDIDocSegmentElements, d, skipEDIDocSegmentElement, ref sMsg);

            return modelEDIDocSegmentElements;
        }

        /// <summary>
        /// select table data by passing EDIMasterDocStructSegment Model
        /// </summary>
        /// <param name="d">EDIMasterDocStructSegment Model</param>
        /// <returns>returns tblEDIMasterDocStructSegment table data</returns>
        /// Modified by SRP on 3/7/18
        private LTS.tblEDIMasterDocStructSegment selectLTSData(Models.EDIMasterDocStructSegment d)
        {
            LTS.tblEDIMasterDocStructSegment ltsmdocstructsegment = new LTS.tblEDIMasterDocStructSegment();
            if (d != null)
            {
                List<string> skipEDIMasterDocStructSegments = new List<string> { "MDSSegUpdated" };
                string sMsg = "";
                ltsmdocstructsegment = (LTS.tblEDIMasterDocStructSegment)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsmdocstructsegment, d, skipEDIMasterDocStructSegments, ref sMsg);
                if (ltsmdocstructsegment != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsmdocstructsegment.MDSSegUpdated = bupdated == null ? new byte[0] : bupdated;
                }
            }
            return ltsmdocstructsegment;
        }
        #endregion


        #region "Rest Services"


        /// <summary>
        /// This is used to return a collection of Model record of EDIMasterDocStructSegment based on
        /// EDIMasterDocStructSegment of Model passed to the method.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>An enumerable list of records</returns>
        /// Created by SRP on 3/7/18
        /// <remarks>The following data is also recognized: filters.</remarks>
        [HttpGet, ActionName("GetRecord")]
        public Models.Response GetRecord([System.Web.Http.FromBody]Models.EDIMasterDocStructSegment data)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                Models.EDIMasterDocStructSegment[] ediEDIMasterDocStructSegment = new Models.EDIMasterDocStructSegment[] { };
                DAL.NGLEDIMDocStructLoopSegment dalData = new DAL.NGLEDIMDocStructLoopSegment(Parameters);
                LTS.tblEDIMasterDocStructSegment oChanges = selectLTSData(data);
                LTS.tblEDIMasterDocStructSegment[] ltsEDIMasterDocStructSegment = dalData.GetEDIMasterDocStructSegment(oChanges.MDSSegControl);

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

        /// Created by SRP on 2/3/18
        /// <summary>
        /// This is used to return a collection of Model record of EDIMasterDocStructSegment based on
        /// Segment and Loop
        /// </summary>
        /// <param name="Segment">The Segment.</param>
        /// <param name="Loop">The Loop.</param>
        /// <returns>An enumerable list of records</returns>
        [HttpGet, ActionName("GetRecordbySegmentLoop")]
        public Models.Response GetRecordbySegmentLoop(string Segment,string Loop)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                Models.EDIMasterDocStructSegment[] ediEDIMasterDocStructSegment = new Models.EDIMasterDocStructSegment[] { };
                DAL.NGLEDIMDocStructLoopSegment dalData = new DAL.NGLEDIMDocStructLoopSegment(Parameters);
                //LTS.tblEDIMasterDocStructSegment oChanges = selectLTSData(data);
                LTS.tblEDIMasterDocStructSegment[] ltsEDIMasterDocStructSegment = null;
                ltsEDIMasterDocStructSegment = dalData.GetEDIMasterDocStructLoopSegment().Where(model => model.MDSSegSegmentControl.Equals(Convert.ToInt32(Segment)) && model.MDSSegMDSLoopControl.Equals(Convert.ToInt32(Loop))).ToArray();

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

        /// Created by SRP on 3/2/18
        /// <summary>
        /// This is used to return a collection of Model record of EDIMasterDocStructSegment 
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
                Models.EDIMasterDocStructSegment[] EDIMDocStructLoops = new Models.EDIMasterDocStructSegment[] { };
                DAL.NGLEDIMDocStructLoopSegment oAn = new DAL.NGLEDIMDocStructLoopSegment(Parameters);
                LTS.tblEDIMasterDocStructSegment[] ltsMDocStructsegments = oAn.GetMDocStructLoopSegments(ref RecordCount, f);
                if (ltsMDocStructsegments != null && ltsMDocStructsegments.Count() > 0)
                {
                    count = ltsMDocStructsegments.Count();
                    EDIMDocStructLoops = (from e in ltsMDocStructsegments
                                          select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                //getting element records from tblEDIMasterDocStructSegments
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
        /// This is used to return a Model record of EDIMasterDocStructSegment which has updated
        /// </summary>
        /// <param name="data">The Model.</param>
        /// <returns>Updated Single record</returns>
        /// <remarks>The following data is also recognized: Model.</remarks>
        [HttpPost, ActionName("UpdateMDocStructLoopSegment")]
        public Models.Response UpdateMDocStructLoopSegment([System.Web.Http.FromBody]Models.EDIMasterDocStructSegment data)
        {
            // create a response message to send back
            var response = new Models.Response();//new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLEDIMDocStructLoopSegment dalData = new DAL.NGLEDIMDocStructLoopSegment(Parameters);
                LTS.tblEDIMasterDocStructSegment oChanges = selectLTSData(data);
                //updates the edi MDocStructLoopSegment
                LTS.tblEDIMasterDocStructSegment oData = dalData.UpdateMDocStructLoopSegment(oChanges);
                Models.EDIMasterDocStructSegment[] oRecords = new Models.EDIMasterDocStructSegment[1];
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
        /// This is used to return a Model record of EDIMasterDocStructSegment which has Inserted
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns>Inserted Single record</returns>
        /// <remarks>The following data is also recognized: Model.</remarks>
        [HttpPost, ActionName("SaveMDocStructLoopSegment")]
      public Models.Response SaveMDocStructLoopSegment([System.Web.Http.FromBody]Models.EDIMasterDocStructSegment dt)       
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLEDIMDocStructLoopSegment dalData = new DAL.NGLEDIMDocStructLoopSegment(Parameters);
                LTS.tblEDIMasterDocStructSegment oChanges = selectLTSData(dt);
                //insert the new edi MDocStructLoopSegment
                LTS.tblEDIMasterDocStructSegment oData = dalData.InsertMDocStructLoopSegment(oChanges);

                Models.EDIMasterDocStructSegment[] oRecords = new Models.EDIMasterDocStructSegment[1];
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

        /// Created by SRP on 3/30/18
        /// <summary>
        /// This is used to return a Model record of EDIMasterDocStructSegment which has Inserted 
        /// using sp name spPopulateEDIMasterLoopSegmentElements
        /// </summary>
        /// <param name="test">The test class having parameters of .</param>
        /// /// <param name="MDSSegMDSLoopControl">The test class having parameters of MDSSegMDSLoopControl.</param>
        /// /// <param name="segmentlength">The test class having parameters of segmentlength.</param>
        /// /// <param name="SegmentControl">The test class having parameters of SegmentControl.</param>
        /// <returns>Inserted Single record</returns>
        /// <remarks>The following data is also recognized: Model.</remarks>
        [HttpPost, ActionName("SaveMDocStructLoopSegmentElement")]
        public Models.Response SaveMDocStructLoopSegmentElement(test dt1)
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
                DAL.NGLEDIMDocStructLoopSegment dalData = new DAL.NGLEDIMDocStructLoopSegment(Parameters);
                LTS.spPopulateEDIMasterLoopSegmentElementsResult[] segList = null;
                segList = dalData.PopulateEDIMasterLoopSegmentElements(SegmentControl, segmentlength, MDSSegMDSLoopControl).ToArray();
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
        /// Created by SRP on 20/3/18
        /// <summary>
        /// This is used to return a collection of Model record of EDIMasterDocStructElement based on
        /// ParentLoopId
        /// </summary>
        /// <param name="segment">The segment.</param>
        /// <returns>An enumerable list of records</returns>
        [HttpGet, ActionName("GetRecordSegmentElements")]
        public Models.Response GetRecordSegmentElements(string segment)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                DAL.NGLEDIMDocStructLoopSegment dalData = new DAL.NGLEDIMDocStructLoopSegment(Parameters);
                Models.EDIMasterDocStructElement[] EDIMDocStructLoops = new Models.EDIMasterDocStructElement[] { };
                LTS.tblEDIMasterDocStructElement[] ltsMDocStructsegments = dalData.GetDocSegmentElementListbySegment(Convert.ToInt32(segment));
                if (ltsMDocStructsegments != null && ltsMDocStructsegments.Count() > 0)
                {
                    count = ltsMDocStructsegments.Count();
                    EDIMDocStructLoops = (from e in ltsMDocStructsegments
                                          select selectModelDataElementNew(e)).ToArray();
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
        /// Created by SRP on 20/3/18
        /// <summary>
        /// This is used to return a collection of Model record of EDIMasterDocStructElement based on
        /// ParentLoopId
        /// </summary>
        /// <param name="segment">The segment.</param>
        /// /// <param name="loop">The loop.</param>
        /// <returns>An enumerable list of records</returns>
        [HttpGet, ActionName("GetRecordSegmentLoopElements")]
        public Models.Response GetRecordSegmentLoopElements(string segment, string loop)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                DAL.NGLEDIMDocStructLoopSegment dalData = new DAL.NGLEDIMDocStructLoopSegment(Parameters);
                Models.EDIMasterDocStructElement[] EDIMDocStructLoops = new Models.EDIMasterDocStructElement[] { };
                LTS.vwGetElementsBySegmentLoop[] ltsMDocStructsegments = null;
                    ltsMDocStructsegments = dalData.GetDocSegmentElementListbySegmentLoop().Where(model => model.MDSSegSegmentControl.Equals(Convert.ToInt32(segment)) && model.MDSSegMDSLoopControl.Equals(Convert.ToInt32(loop))).ToArray();
                if (ltsMDocStructsegments != null && ltsMDocStructsegments.Count() > 0)
                {
                    count = ltsMDocStructsegments.Count();
                    EDIMDocStructLoops = (from e in ltsMDocStructsegments
                                          select selectModelDataElementLoop(e)).ToArray();
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
        /// Created by SRP on 20/3/18
        /// <summary>
        /// This is used to return a collection of Model record of EDIMasterDocStructSegment based on
        /// loopControl
        /// </summary>
        /// <param name="segment">The segment.</param>
        /// <returns>An enumerable list of records</returns>
        [HttpGet, ActionName("GetRecordSegmentByMaster")]
        public Models.Response GetRecordSegmentByMaster(string loopControl)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                DAL.NGLEDIMDocStructLoopSegment dalData = new DAL.NGLEDIMDocStructLoopSegment(Parameters);
                Models.EDIMasterDocStructSegment[] EDIMDocStructLoops = new Models.EDIMasterDocStructSegment[] { };
                LTS.vwGetMDocSegmentsByLoop[] ltsMDocStructsegments = null;
                ltsMDocStructsegments = dalData.GetMDocSegmentsByLoops().Where(model => model.MDSSegMDSLoopControl.Equals(Convert.ToInt32(loopControl))).ToArray();
                if (ltsMDocStructsegments != null && ltsMDocStructsegments.Count() > 0)
                {
                    count = ltsMDocStructsegments.Count();
                    EDIMDocStructLoops = (from e in ltsMDocStructsegments
                                          select selectModelDataGetMDocSegmentsByLoop(e)).ToArray();
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
        /// Created by SRP on 3/7/18
        /// <summary>
        /// This is used to return true or false record of EDIMasterDocStructSegment which has deleted or not
        /// based on MDSSegControl using teststructloop class
        /// </summary>
        /// <param name="testMstructsegment">The testMstructsegment.</param>
        /// <returns>deleted Single record</returns>
        [HttpPost, ActionName("DeleteRecord")]
        public bool DeleteRecord(testMstructsegment d1)
        {
            bool response = new bool();
            try
            {
                Models.EDIMasterDocStructSegment[] ediMDocStructLoops = new Models.EDIMasterDocStructSegment[] { };
                DAL.NGLEDIMDocStructLoopSegment dalData = new DAL.NGLEDIMDocStructLoopSegment(Parameters);
                bool result = dalData.DeleteEDIMasterDocStructSegments(Convert.ToInt32(d1.MDSSegControl));
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