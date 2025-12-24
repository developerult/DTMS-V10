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
    //delete edi struct element attribute records by passing this as parameter
    public class DocstructelementAttributeDel
    {
        public string DSAttrControl { get; set; }
    }
    /// <summary>
    /// EDIDocStructElmntAttributeController for EDI Master Doc Structure element attributes Rest API Controls
    /// </summary>
    /// <Remarks>
    /// Created By SRP on 3/2/2018
    /// </Remarks>
    public class EDIDocStructElmntAttributeController : NGLControllerBase
    {
        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.EDIDocStructElmntAttributeController";
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
        /// Selecting a EDIDocStructElmntAttribute Model data by passing table records
        /// </summary>
        /// <param name="d">tblEDIDocStructElmntAttribute Model</param>
        /// <returns>returns EDIDocStructElmntAttribute table data</returns>
        /// Modified by SRP on 3/7/2018
        private Models.EDIDocStructElmntAttribute selectModelData(LTS.tblEDIDocStructElmntAttribute d)
        {
            Models.EDIDocStructElmntAttribute modelEDIDocStructElement = new Models.EDIDocStructElmntAttribute();
            //skipping values for reference to foreign keys added by SRP 28/2/2018
            List<string> skipEDIDocStructElement = new List<string> { "DSAttrModUpdated", "tblEDIFormattingFunction", "tblEDIDocStructElement", "tblEDITransformationType", "tblEDIValidationType" };
            string sMsg = "";
            modelEDIDocStructElement = (Models.EDIDocStructElmntAttribute)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelEDIDocStructElement, d, skipEDIDocStructElement, ref sMsg);
            if (modelEDIDocStructElement != null) { modelEDIDocStructElement.setUpdated(d.DSAttrModUpdated.ToArray()); }
            return modelEDIDocStructElement;
        }
        /// <summary>
        /// Selecting a EDIDocStructElmntAttribute Model data by passing table records
        /// </summary>
        /// <param name="d">vw_tblEDIDocStructElmntAttribute </param>
        /// <returns>returns EDIDocStructElmntAttribute table data</returns>
        /// Modified by SRP on 3/7/2018
        private Models.EDIDocStructElmntAttribute selectModelDataGetEDIDocStructElmntAttribute(LTS.vw_tblEDIDocStructElmntAttribute d)
        {
            Models.EDIDocStructElmntAttribute modelEDIDocSegmentElements = new Models.EDIDocStructElmntAttribute();
            List<string> skipEDIDocSegmentElement = new List<string> { "DSAttrModUpdated" };// { "MDSElementUpdated", "tblEDIDataType", "tblEDIElmnt", "tblEDIMasterDocStructSegments", "_tblEDIMasterDocStructSegment", "tblEDIMasterDocStructSegment", "tblEDIMasterDocStructElements" };
            string sMsg = "";
            modelEDIDocSegmentElements = (Models.EDIDocStructElmntAttribute)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelEDIDocSegmentElements, d, skipEDIDocSegmentElement, ref sMsg);

            //if (modelEDIDocSegmentElements != null) { modelEDIDocSegmentElements.setUpdated(d.DSEUpdated.ToArray()); }

            return modelEDIDocSegmentElements;
        }

        /// <summary>
        /// select table data by passing EDIDocStructElmntAttribute Model
        /// </summary>
        /// <param name="d">EDIDocStructElmntAttribute Model</param>
        /// <returns>returns tblEDIDocStructElmntAttribute table data</returns>
        /// Modified by SRP on 3/7/18
        private LTS.tblEDIDocStructElmntAttribute selectLTSData(Models.EDIDocStructElmntAttribute d)
        {
            d.DSAttrDataMapFieldControl = d.DSAttrDataMapFieldControl;
            LTS.tblEDIDocStructElmntAttribute ltsEDIDocStructElement = new LTS.tblEDIDocStructElmntAttribute();
            if (d != null)
            {
                List<string> skipEDIDocStructElement = new List<string> { "DSAttrModUpdated" };
                string sMsg = "";
                ltsEDIDocStructElement = (LTS.tblEDIDocStructElmntAttribute)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsEDIDocStructElement, d, skipEDIDocStructElement, ref sMsg);
                if (ltsEDIDocStructElement != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsEDIDocStructElement.DSAttrModUpdated = bupdated == null ? new byte[0] : bupdated;
                }
            }
            return ltsEDIDocStructElement;
        }
        #endregion


        #region "Rest Services"

        /// <summary>
        /// This is used to return a collection of Model record of EDIDocStructElmntAttribute based on
        /// EDIDocStructElmntAttribute of Model passed to the method.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>An enumerable list of records</returns>
        /// Created by SRP on 3/7/18
        /// <remarks>The following data is also recognized: filters.</remarks>
        [HttpGet, ActionName("GetRecord")]
        public Models.Response GetRecord([System.Web.Http.FromBody]Models.EDIDocStructElmntAttribute data)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                Models.EDIDocStructElmntAttribute[] ediDocStructElmntAttribute = new Models.EDIDocStructElmntAttribute[] { };
                DAL.NGLEDIDocStructElmntAttribute dalData = new DAL.NGLEDIDocStructElmntAttribute(Parameters);
                LTS.tblEDIDocStructElmntAttribute oChanges = selectLTSData(data);
                LTS.tblEDIDocStructElmntAttribute[] ltsEDIDocStructElement = dalData.GetEDIDocStructElmntAttribute(oChanges);

                if (ltsEDIDocStructElement != null && ltsEDIDocStructElement.Count() > 0)
                {
                    count = ltsEDIDocStructElement.Count();
                    ediDocStructElmntAttribute = (from e in ltsEDIDocStructElement
                                              select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                //getting element records from tblEDISegment
                response = new Models.Response(ediDocStructElmntAttribute, count);
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
        /// This is used to return a collection of Model record of EDIDocStructElmntAttribute 
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
                Models.EDIDocStructElmntAttribute[] ediEDIDocStructElmntAttribute = new Models.EDIDocStructElmntAttribute[] { };
                DAL.NGLEDIDocStructElmntAttribute oAn = new DAL.NGLEDIDocStructElmntAttribute(Parameters);
                LTS.tblEDIDocStructElmntAttribute[] ltsEDIDocStructElmntAttribute = oAn.GetEDIDocStructElmntAttributes(ref RecordCount, f);
                if (ltsEDIDocStructElmntAttribute != null && ltsEDIDocStructElmntAttribute.Count() > 0)
                {
                    count = ltsEDIDocStructElmntAttribute.Count();
                    ediEDIDocStructElmntAttribute = (from e in ltsEDIDocStructElmntAttribute
                                              select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                //getting element records from tblEDIMasterDocStructElements
                response = new Models.Response(ediEDIDocStructElmntAttribute, count);
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
        /// This is used to return a Model record of EDIDocStructElmntAttribute which has updated
        /// </summary>
        /// <param name="data">The Model.</param>
        /// <returns>Updated Single record</returns>
        /// <remarks>The following data is also recognized: Model.</remarks>
        [HttpPost, ActionName("UpdateEDIDocStructElmntAttribute")]
        public Models.Response UpdateEDIDocStructElmntAttribute([System.Web.Http.FromBody]Models.EDIDocStructElmntAttribute data)
        {
            // create a response message to send back
            var response = new Models.Response();//new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLEDIDocStructElmntAttribute dalData = new DAL.NGLEDIDocStructElmntAttribute(Parameters);
                LTS.tblEDIDocStructElmntAttribute oChanges = selectLTSData(data);
                //updates the edi EDIDocStructElement
                LTS.tblEDIDocStructElmntAttribute oData = dalData.UpdateEDIDocStructElmntAttribute(oChanges);
                Models.EDIDocStructElmntAttribute[] oRecords = new Models.EDIDocStructElmntAttribute[1];
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
        /// This is used to return a Model record of EDIDocStructElmntAttribute which has Inserted
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns>Inserted Single record</returns>
        /// <remarks>The following data is also recognized: Model.</remarks>
        [HttpPost, ActionName("SaveEDIDocStructElmntAttribute")]
        public Models.Response SaveEDIDocStructElmntAttribute([System.Web.Http.FromBody]Models.EDIDocStructElmntAttribute dt)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLEDIDocStructElmntAttribute dalData = new DAL.NGLEDIDocStructElmntAttribute(Parameters);
                LTS.tblEDIDocStructElmntAttribute oChanges = selectLTSData(dt);
                //insert the new edi MDocStructLoopSegment
                LTS.tblEDIDocStructElmntAttribute oData = dalData.InsertEDIDocStructElmntAttribute(oChanges);
                Models.EDIDocStructElmntAttribute[] oRecords = new Models.EDIDocStructElmntAttribute[1];
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
        /// This is used to return a collection of Model record of EDIDocStructElmntAttribute based on
        /// ElementControl
        /// </summary>
        /// <param name="ElementControl">The ElementControl.</param>
        /// <returns>An enumerable list of records</returns>
        [HttpGet, ActionName("GetElementRecordsbyElementId")]
        public Models.Response GetElementRecordsbyElementId(string ElementControl)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                Models.EDIDocStructElmntAttribute[] ediEDIMasterDocStructElmntAttribute = new Models.EDIDocStructElmntAttribute[] { };
                DAL.NGLEDIDocStructElmntAttribute dalData = new DAL.NGLEDIDocStructElmntAttribute(Parameters);
                LTS.tblEDIDocStructElmntAttribute[] ltsEDIMasterDocStructElmntAttribute = null;
                ltsEDIMasterDocStructElmntAttribute = dalData.GetEDIDocStructElmntAttribute(Convert.ToInt32(ElementControl)).ToArray();

                if (ltsEDIMasterDocStructElmntAttribute != null && ltsEDIMasterDocStructElmntAttribute.Count() > 0)
                {
                    count = ltsEDIMasterDocStructElmntAttribute.Count();
                    ediEDIMasterDocStructElmntAttribute = (from e in ltsEDIMasterDocStructElmntAttribute
                                                           select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                //getting element records from tblEDIMasterDocStructElmntAttribute
                response = new Models.Response(ediEDIMasterDocStructElmntAttribute, count);


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
        /// This is used to return a collection of Model record of EDIDocStructElmntAttribute based on
        /// ElementControl
        /// </summary>
        /// <param name="ElementControl">The ElementControl.</param>
        /// <returns>An enumerable list of records</returns>
        [HttpGet, ActionName("GetRecordsbyElementId")]
        public Models.Response GetRecordsbyElementId(string ElementControl)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                DAL.NGLEDIDocStructElmntAttribute dalData = new DAL.NGLEDIDocStructElmntAttribute(Parameters);
                Models.EDIDocStructElmntAttribute[] EDIStructLoops = new Models.EDIDocStructElmntAttribute[] { };
                LTS.vw_tblEDIDocStructElmntAttribute[] ltsStructsegments = null;
                ltsStructsegments = dalData.GetDocStructElmntAttributes().Where(model => model.DSAttrDSElementControl.Equals(Convert.ToInt32(ElementControl))).ToArray();
                if (ltsStructsegments != null && ltsStructsegments.Count() > 0)
                {
                    count = ltsStructsegments.Count();
                    EDIStructLoops = (from e in ltsStructsegments
                                      select selectModelDataGetEDIDocStructElmntAttribute(e)).ToArray();
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

        /// Created by PKN on 5/07/18
		/// <summary>
		/// This is used to return true or false record of EDITPDocStructElementAttribute indicating record deletion
		/// by passing TPDocstructelementAttributeDel having parameter
		/// </summary>
		/// <param name="TPDocstructelementAttrib">The TPDocstructelementAttribute class.</param>
		/// <returns>deleted Single record</returns>
		/// <remarks>The following data is also recognized: Model.</remarks>
		[HttpPost, ActionName("DeleteRecord")]
        public bool DeleteRecord(DocstructelementAttributeDel TPDocstructelementAttrib)
        {
            bool response = new bool();
            try
            {
                Models.EDIMasterDocStructElmntAttribute[] ediMDocStructElementAttribute = new Models.EDIMasterDocStructElmntAttribute[] { };
                DAL.NGLEDIDocStructElmntAttribute dalData = new DAL.NGLEDIDocStructElmntAttribute(Parameters);
                bool result = dalData.DeleteEDITPDocStructElementAttribute(Convert.ToInt32(TPDocstructelementAttrib.DSAttrControl));
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