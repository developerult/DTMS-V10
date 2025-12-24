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
    public class MDocstructelementAttributeDel
    {
        public string MDSAttrControl { get; set; }
    }
    /// <summary>
    /// EDIMasterDocStructElmntAttributeController for EDI Master Doc Structure element attributes Rest API Controls
    /// </summary>
    /// <Remarks>
    /// Created By SRP on 2/28/2018
    /// </Remarks>
    public class EDIMasterDocStructElmntAttributeController : NGLControllerBase
    {
        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.EDIMasterDocStructElmntAttributeController";
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
        /// Selecting a EDIMasterDocStructElmntAttribute Model data by passing table records
        /// </summary>
        /// <param name="d">tblEDIMasterDocStructElmntAttribute</param>
        /// <returns>returns EDIMasterDocStructElmntAttribute table data</returns>
        /// Modified by SRP on 3/7/2018
        private Models.EDIMasterDocStructElmntAttribute selectModelData(LTS.tblEDIMasterDocStructElmntAttribute d)
        {
            Models.EDIMasterDocStructElmntAttribute modelmdocstructelementattribute = new Models.EDIMasterDocStructElmntAttribute();
            //skipping values for reference to foreign keys added by SRP 28/2/2018
            List<string> skipEDIMasterDocStructElmntAttributes = new List<string> { "MDSAttrModUpdated", "tblEDIElmnt", "tblEDIMasterDocStructElement", "tblEDIMasterDocStructElmntAttribute", "tblEDITransformationType", "tblEDIValidationType", "tblEDIFormattingFunction" };
            string sMsg = "";
            modelmdocstructelementattribute = (Models.EDIMasterDocStructElmntAttribute)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelmdocstructelementattribute, d, skipEDIMasterDocStructElmntAttributes, ref sMsg);
            if (modelmdocstructelementattribute != null) { modelmdocstructelementattribute.setUpdated(d.MDSAttrModUpdated.ToArray()); }
            return modelmdocstructelementattribute;
        }
        /// <summary>
        /// Selecting a EDIMasterDocStructElmntAttribute Model data by passing view records
        /// </summary>
        /// <param name="d">vw_tblEDIMasterDocStructElmntAttribute </param>
        /// <returns>returns EDIMasterDocStructElmntAttribute table data</returns>
        /// Modified by SRP on 3/7/2018
        private Models.EDIMasterDocStructElmntAttribute selectModelDataGetEDIDocStructElmntAttribute(LTS.vw_tblEDIMasterDocStructElmntAttribute d)
        {
            Models.EDIMasterDocStructElmntAttribute modelEDIDocSegmentElements = new Models.EDIMasterDocStructElmntAttribute();
            List<string> skipEDIDocSegmentElement = new List<string> { "MDSAttrModUpdated" };// { "MDSElementUpdated", "tblEDIDataType", "tblEDIElmnt", "tblEDIMasterDocStructSegments", "_tblEDIMasterDocStructSegment", "tblEDIMasterDocStructSegment", "tblEDIMasterDocStructElements" };
            string sMsg = "";
            modelEDIDocSegmentElements = (Models.EDIMasterDocStructElmntAttribute)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelEDIDocSegmentElements, d, skipEDIDocSegmentElement, ref sMsg);
            return modelEDIDocSegmentElements;
        }
        /// <summary>
        /// select table data by passing EDIMasterDocStructElmntAttribute Model
        /// </summary>
        /// <param name="d">EDIMasterDocStructElmntAttribute Model</param>
        /// <returns>returns tblEDIMasterDocStructElmntAttribute table data</returns>
        /// Modified by SRP on 3/7/18
        private LTS.tblEDIMasterDocStructElmntAttribute selectLTSData(Models.EDIMasterDocStructElmntAttribute d)
        {
            d.MDSAttrDataMapFieldControl = d.MDSAttrDataMapFieldControl;
            LTS.tblEDIMasterDocStructElmntAttribute ltsmdocstructelement = new LTS.tblEDIMasterDocStructElmntAttribute();
            if (d != null)
            {
                List<string> skipEDIMasterDocStructElmntAttributes = new List<string> { "MDSAttrModUpdated" };
                string sMsg = "";
                ltsmdocstructelement = (LTS.tblEDIMasterDocStructElmntAttribute)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsmdocstructelement, d, skipEDIMasterDocStructElmntAttributes, ref sMsg);
                if (ltsmdocstructelement != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsmdocstructelement.MDSAttrModUpdated = bupdated == null ? new byte[0] : bupdated;
                }
            }
            return ltsmdocstructelement;
        }
        #endregion


        #region "Rest Services"

        /// <summary>
        /// This is used to return a collection of Model record of EDIMasterDocStructElmntAttribute based on
        /// EDIMasterDocStructElmntAttribute of Model passed to the method.
        /// </summary>
        /// <param name="Model data">The Model data.</param>
        /// <returns>An enumerable list of records</returns>
        /// Created by SRP on 3/7/18
        /// <remarks>The following data is also recognized: filters.</remarks>
        [HttpGet, ActionName("GetRecord")]
        public Models.Response GetRecord([System.Web.Http.FromBody]Models.EDIMasterDocStructElmntAttribute data)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                Models.EDIMasterDocStructElmntAttribute[] ediEDIMasterDocStructElmntAttribute = new Models.EDIMasterDocStructElmntAttribute[] { };
                DAL.NGLEDIMDocStructelementattribute dalData = new DAL.NGLEDIMDocStructelementattribute(Parameters);
                LTS.tblEDIMasterDocStructElmntAttribute oChanges = selectLTSData(data);
                LTS.tblEDIMasterDocStructElmntAttribute[] ltsEDIMasterDocStructElmntAttribute = dalData.GetEDIMasterDocStructElmntAttribute(oChanges.MDSAttrControl);

                if (ltsEDIMasterDocStructElmntAttribute != null && ltsEDIMasterDocStructElmntAttribute.Count() > 0)
                {
                    count = ltsEDIMasterDocStructElmntAttribute.Count();
                    ediEDIMasterDocStructElmntAttribute = (from e in ltsEDIMasterDocStructElmntAttribute
                                                    select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                //getting element records from tblEDISegment
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
        /// Created by SRP on 3/7/18
        /// <summary>
        /// This is used to return a collection of Model record of EDIMasterDocStructElmntAttribute
        /// based on ElementControl
        /// </summary>
        /// <returns>An enumerable list of records</returns>
        [HttpGet, ActionName("GetElementRecordbyElementId")]
        public Models.Response GetElementRecordbyElementId(string ElementControl)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                Models.EDIMasterDocStructElmntAttribute[] ediEDIMasterDocStructElmntAttribute = new Models.EDIMasterDocStructElmntAttribute[] { };
                DAL.NGLEDIMDocStructelementattribute dalData = new DAL.NGLEDIMDocStructelementattribute(Parameters);
                LTS.tblEDIMasterDocStructElmntAttribute[] ltsEDIMasterDocStructElmntAttribute = null;
                ltsEDIMasterDocStructElmntAttribute = dalData.GetEDIMasterDocStructElmntAttribute(Convert.ToInt32(ElementControl)).ToArray();

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
        /// Created by SRP on 3/7/18
        /// <summary>
        /// This is used to return a collection of Model record of EDIMasterDocStructElmntAttribute
        /// based on ElementControl
        /// </summary>
        /// <returns>An enumerable list of records</returns>
        [HttpGet, ActionName("GetRecordbyElementId")]
        public Models.Response GetRecordbyElementId(string ElementControl)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                DAL.NGLEDIMDocStructelementattribute dalData = new DAL.NGLEDIMDocStructelementattribute(Parameters);
                Models.EDIMasterDocStructElmntAttribute[] EDIStructLoops = new Models.EDIMasterDocStructElmntAttribute[] { };
                LTS.vw_tblEDIMasterDocStructElmntAttribute[] ltsStructsegments = null;
                ltsStructsegments = dalData.GetMasterDocStructElmntAttributes().Where(model => model.MDSAttrMDSElementControl.Equals(Convert.ToInt32(ElementControl))).ToArray();
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
        /// Created by SRP on 3/7/18
        /// <summary>
        /// This is used to return a collection of Model record of EDIMasterDocStructElmntAttribute 
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
                Models.EDIMasterDocStructElmntAttribute[] EDIMDocStructelementattr = new Models.EDIMasterDocStructElmntAttribute[] { };
                DAL.NGLEDIMDocStructelementattribute oAn = new DAL.NGLEDIMDocStructelementattribute(Parameters);
                LTS.tblEDIMasterDocStructElmntAttribute[] ltsMDocStructelementattr = oAn.GetMDocSegElementAttributes(ref RecordCount, f);
                if (ltsMDocStructelementattr != null && ltsMDocStructelementattr.Count() > 0)
                {
                    count = ltsMDocStructelementattr.Count();
                    EDIMDocStructelementattr = (from e in ltsMDocStructelementattr
                                          select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                //getting element records from tblEDIMasterDocStructElmntAttribute
                response = new Models.Response(EDIMDocStructelementattr, count);
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
        [HttpPost, ActionName("UpdateMDocSegElementAttributes")]
        public Models.Response UpdateMDocSegElementAttributes([System.Web.Http.FromBody]Models.EDIMasterDocStructElmntAttribute data)
        {
            // create a response message to send back
            var response = new Models.Response();//new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLEDIMDocStructelementattribute dalData = new DAL.NGLEDIMDocStructelementattribute(Parameters);
                LTS.tblEDIMasterDocStructElmntAttribute oChanges = selectLTSData(data);

                //updates the edi MDocStructLoopSegment
                LTS.tblEDIMasterDocStructElmntAttribute oData = dalData.UpdateMDocSegmentElementAtribute(oChanges);
                Models.EDIMasterDocStructElmntAttribute[] oRecords = new Models.EDIMasterDocStructElmntAttribute[1];
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
        /// This is used to return a Model record of EDIMasterDocStructElmntAttribute which has Inserted
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns>Inserted Single record</returns>
        /// <remarks>The following data is also recognized: Model.</remarks>
        [HttpPost, ActionName("SaveMDocSegElementAttributes")]
        public Models.Response SaveMDocSegElementAttributes([System.Web.Http.FromBody]Models.EDIMasterDocStructElmntAttribute data)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLEDIMDocStructelementattribute dalData = new DAL.NGLEDIMDocStructelementattribute(Parameters);
                LTS.tblEDIMasterDocStructElmntAttribute oChanges = selectLTSData(data);
                //insert the new edi MDocStructLoopSegment
                LTS.tblEDIMasterDocStructElmntAttribute oData = dalData.InsertMDocSegElementAttributes(oChanges);
                Models.EDIMasterDocStructElmntAttribute[] oRecords = new Models.EDIMasterDocStructElmntAttribute[1];
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

        /// Created by PKN on 5/07/18
        /// <summary>
        /// This is used to return true or false record of EDIMasterDocStructElementAttribute indicating record deletion
        /// by passing MDocstructelementAttributeDel having parameter
        /// </summary>
        /// <param name="MDocstructelementAttrib">The MDocstructelementAttribute class.</param>
        /// <returns>deleted Single record</returns>
        /// <remarks>The following data is also recognized: Model.</remarks>
        [HttpPost, ActionName("DeleteRecord")]
        public bool DeleteRecord(MDocstructelementAttributeDel MDocstructelementAttrib)
        {
            bool response = new bool();
            try
            {
                Models.EDIMasterDocStructElmntAttribute[] ediMDocStructElementAttribute = new Models.EDIMasterDocStructElmntAttribute[] { };
                DAL.NGLEDIMDocStructelementattribute dalData = new DAL.NGLEDIMDocStructelementattribute(Parameters);
                bool result = dalData.DeleteEDIMasterDocStructElementAttribute(Convert.ToInt32(MDocstructelementAttrib.MDSAttrControl));
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