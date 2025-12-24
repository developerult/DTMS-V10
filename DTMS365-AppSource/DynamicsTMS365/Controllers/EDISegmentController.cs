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
    /// EDISegmentController for EDISegments Rest API Controls
    /// </summary>
    /// <Remarks>
    /// Created By SRP on 2/26/2018
    /// </Remarks>
    public class EDISegmentController : NGLControllerBase
    {
        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.EDISegmentController";
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
        /// Selecting a EDISegment Model data by passing table records
        /// </summary>
        /// <param name="d">The table data.</param>
        /// <returns>Returns EDISegment Type</returns>
        /// Modified by SRP on 2/26/18
        private Models.EDISegment selectModelData(LTS.tblEDISegment d)
        {
            Models.EDISegment modelsegment = new Models.EDISegment();
            //skipping values for reference to foreign keys added by SRP 26/2/2018
            List<string> skipEDISegments = new List<string> { "SegmentUpdated" };
            string sMsg = "";
            modelsegment = (Models.EDISegment)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelsegment, d, skipEDISegments, ref sMsg);
            if (modelsegment != null) { modelsegment.setUpdated(d.SegmentUpdated.ToArray()); }
            return modelsegment;
        }

        /// <summary>
        /// select table data by passing EDISegment Model
        /// </summary>
        /// <param name="d">EDISegment Model</param>
        /// <returns>returns tblEDISegment table data</returns>
        /// Modified by SRP on 2/26/18
        private LTS.tblEDISegment selectLTSData(Models.EDISegment d)
        {
            LTS.tblEDISegment ltsSegment = new LTS.tblEDISegment();
            if (d != null)
            {
                List<string> skipEDISegments = new List<string> { "SegmentUpdated" };
                string sMsg = "";
                ltsSegment = (LTS.tblEDISegment)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsSegment, d, skipEDISegments, ref sMsg);
                if (ltsSegment != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsSegment.SegmentUpdated = bupdated == null ? new byte[0] : bupdated;
                }
            }
            return ltsSegment;
        }
        #endregion


        #region "Rest Services"
        /// <summary>
        /// This is used to return a collection of Model record of EDISegment based on
        /// Segment of Model passed to the method.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>An enumerable list of records</returns>
        /// Created by SRP on 2/26/18
        /// <remarks>The following data is also recognized: filters.</remarks>
        [HttpGet, ActionName("GetRecord")]
        public Models.Response GetRecord([System.Web.Http.FromBody]Models.EDISegment data)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                Models.EDISegment[] edisegments = new Models.EDISegment[] { };
                DAL.NGLEDISegment dalData = new DAL.NGLEDISegment(Parameters);
                LTS.tblEDISegment oChanges = selectLTSData(data);
                LTS.tblEDISegment[] ltsSegment = dalData.GetEDISegment(oChanges.SegmentControl);
                if (ltsSegment != null && ltsSegment.Count() > 0)
                {
                    count = ltsSegment.Count();
                    edisegments = (from e in ltsSegment
                                   select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                //getting element records from tblEDISegment
                response = new Models.Response(edisegments, count);
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
        /// This is used to return a collection of Model record of EDISegment based on
        /// Segment Name and Segment Description passed to the method.
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
                Models.EDISegment[] edisegments = new Models.EDISegment[] { };
                DAL.NGLEDISegment oAn = new DAL.NGLEDISegment(Parameters);
                LTS.tblEDISegment[] ltsSegment = oAn.GetEDISegments(ref RecordCount, f);
                if (ltsSegment != null && ltsSegment.Count() > 0)
                {
                    count = ltsSegment.Count();
                    edisegments = (from e in ltsSegment
                                    select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                //getting element records from tblEDISegment
                response = new Models.Response(edisegments, count);
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
        /// This is used to return a Model record of EDISegment which has updated
        /// </summary>
        /// <param name="data">The Model.</param>
        /// <returns>Updated Single record</returns>
        /// <remarks>The following data is also recognized: Model.</remarks>
        
        [HttpPost, ActionName("PostSave")]
        public Models.Response PostSave([System.Web.Http.FromBody]Models.EDISegment data)
        {
            // create a response message to send back
            var response = new Models.Response();//new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLEDISegment dalData = new DAL.NGLEDISegment(Parameters);
                LTS.tblEDISegment oChanges = selectLTSData(data);
                //updates the edi segments
                LTS.tblEDISegment oData = dalData.UpdateEDISegment(oChanges);
                Models.EDISegment[] oRecords = new Models.EDISegment[1];
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
        /// Created by SN on 2/21/18
        /// <summary>
        /// This is used to return a Model record of EDISegment which has Inserted
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns>Inserted Single record</returns>
        /// <remarks>The following data is also recognized: Model.</remarks>
        [HttpPost, ActionName("SaveEDISegment")]
        public Models.Response SaveEDISegment([System.Web.Http.FromBody]Models.EDISegment dt)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLEDISegment dalData = new DAL.NGLEDISegment(Parameters);
                LTS.tblEDISegment oChanges = selectLTSData(dt);
                //insert the new edi segments
                LTS.tblEDISegment oData = dalData.InsertEDISegment(oChanges);
                Models.EDISegment[] oRecords = new Models.EDISegment[1];
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
        /// This is used to return true or false record of EDISegment which has deleted or not
        /// </summary>
        /// <param name="data">The Model.</param>
        /// <returns>deleted Single record</returns>
        /// <remarks>The following data is also recognized: Model.</remarks>
        [HttpDelete, ActionName("DeleteRecord")]
        public bool DeleteRecord([System.Web.Http.FromBody]Models.EDISegment data)
        {
            bool response = new bool();
            /*if (!authenticateController(bool response)) { return response; }*/
            try
            {
               
                Models.EDISegment[] ediSegments = new Models.EDISegment[] { };
                DAL.NGLEDISegment dalData = new DAL.NGLEDISegment(Parameters);
                LTS.tblEDISegment oChanges = selectLTSData(data);
                bool result = dalData.DeleteEDISegment(oChanges);
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