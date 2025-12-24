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
    //delete edi loop records by passing this as parameter
    public class testMstructelements
    {
        public string MDSElementControl { get; set; }
    }
    /// <summary>
    /// EDIMasterDocStructElementController for EDI Master Doc Structure Element Rest API Controls
    /// </summary>
    /// <Remarks>
    /// Created By SRP on 2/28/2018
    /// </Remarks>
    public class EDIMasterDocStructElementController : NGLControllerBase
    {
        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.EDIMasterDocStructElementController";
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
        /// Selecting a EDIMasterDocStructElement Model data by passing table records
        /// </summary>
        /// <param name="d">tblEDIMasterDocStructElement Model</param>
        /// <returns>returns EDIMasterDocStructElement table data</returns>
        /// Modified by SRP on 3/2/2018
        private Models.EDIMasterDocStructElement selectModelData(LTS.tblEDIMasterDocStructElement d)
        {
            Models.EDIMasterDocStructElement modelEDIMasterDocStructElement = new Models.EDIMasterDocStructElement();
            //skipping values for reference to foreign keys added by SRP 28/2/2018
            List<string> skipEDIMasterDocStructElement = new List<string> { "MDSElementUpdated", "tblEDIMasterDocStructSegments", "tblEDIMasterDocStructSegment", "tblEDIMasterDocStructElement", "tblEDIMasterDocStructElements", "tblEDIDataType" };
            string sMsg = "";
            modelEDIMasterDocStructElement = (Models.EDIMasterDocStructElement)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelEDIMasterDocStructElement, d, skipEDIMasterDocStructElement, ref sMsg);
            if (modelEDIMasterDocStructElement != null) { modelEDIMasterDocStructElement.setUpdated(d.MDSElementUpdated.ToArray()); }
            return modelEDIMasterDocStructElement;
        }

        /// <summary>
        /// select table data by passing EDIMasterDocStructElement Model
        /// </summary>
        /// <param name="d">EDIMasterDocStructElement Model</param>
        /// <returns>returns tblEDIMasterDocStructElement table data</returns>
        /// Modified by SRP on 3/7/18
        private LTS.tblEDIMasterDocStructElement selectLTSData(Models.EDIMasterDocStructElement d)
        {
            LTS.tblEDIMasterDocStructElement ltsEDIMasterDocStructElement = new LTS.tblEDIMasterDocStructElement();
            if (d != null)
            {
                List<string> skipEDIMasterDocStructElement = new List<string> { "MDSElementUpdated" };
                string sMsg = "";
                ltsEDIMasterDocStructElement = (LTS.tblEDIMasterDocStructElement)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsEDIMasterDocStructElement, d, skipEDIMasterDocStructElement, ref sMsg);
                if (ltsEDIMasterDocStructElement != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsEDIMasterDocStructElement.MDSElementUpdated = bupdated == null ? new byte[0] : bupdated;
                }
            }
            return ltsEDIMasterDocStructElement;
        }
        #endregion


        #region "Rest Services"

        /// <summary>
        /// This is used to return a collection of Model record of EDIMasterDocStructElement based on
        /// EDIMasterDocStructElement of Model passed to the method.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>An enumerable list of records</returns>
        /// Created by SRP on 3/7/18
        /// <remarks>The following data is also recognized: filters.</remarks>
        [HttpGet, ActionName("GetRecord")]
        public Models.Response GetRecord([System.Web.Http.FromBody]Models.EDIMasterDocStructElement data)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                Models.EDIMasterDocStructElement[] ediEDIMasterDocStructElement = new Models.EDIMasterDocStructElement[] { };
                DAL.NGLEDIMDocStructelement dalData = new DAL.NGLEDIMDocStructelement(Parameters);
                LTS.tblEDIMasterDocStructElement oChanges = selectLTSData(data);
                LTS.tblEDIMasterDocStructElement[] ltsEDIMasterDocStructElement = dalData.GetEDIMasterDocStructElement(oChanges.MDSElementControl);

                if (ltsEDIMasterDocStructElement != null && ltsEDIMasterDocStructElement.Count() > 0)
                {
                    count = ltsEDIMasterDocStructElement.Count();
                    ediEDIMasterDocStructElement = (from e in ltsEDIMasterDocStructElement
                                                           select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                //getting element records from tblEDISegment
                response = new Models.Response(ediEDIMasterDocStructElement, count);
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
        /// This is used to return a collection of Model record of EDIMasterDocStructElement 
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
                Models.EDIMasterDocStructElement[] edimdocstructelement = new Models.EDIMasterDocStructElement[] { };
                DAL.NGLEDIMDocStructelement oAn = new DAL.NGLEDIMDocStructelement(Parameters);
                LTS.tblEDIMasterDocStructElement[] ltsMDocStructelements = oAn.GetMDocSegmentElements(ref RecordCount, f);
                if (ltsMDocStructelements != null && ltsMDocStructelements.Count() > 0)
                {
                    count = ltsMDocStructelements.Count();
                    edimdocstructelement = (from e in ltsMDocStructelements
                                          select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                //getting element records from tblEDIMasterDocStructElements
                response = new Models.Response(edimdocstructelement, count);
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
        /// This is used to return a Model record of EDIMasterDocStructElement which has updated
        /// </summary>
        /// <param name="data">The Model.</param>
        /// <returns>Updated Single record</returns>
        /// <remarks>The following data is also recognized: Model.</remarks>
        [HttpPost, ActionName("UpdateMDocSegmentElement")]
        public Models.Response UpdateMDocSegmentElement([System.Web.Http.FromBody]Models.EDIMasterDocStructElement data)
        {
            // create a response message to send back
            var response = new Models.Response();//new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLEDIMDocStructelement dalData = new DAL.NGLEDIMDocStructelement(Parameters);
                LTS.tblEDIMasterDocStructElement oChanges = selectLTSData(data);
                //updates the edi mdocstructelement
                LTS.tblEDIMasterDocStructElement oData = dalData.UpdateMDocSegmentElement(oChanges);
                Models.EDIMasterDocStructElement[] oRecords = new Models.EDIMasterDocStructElement[1];
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
        /// This is used to return a Model record of EDIMasterDocStructElement which has Inserted
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns>Inserted Single record</returns>
        /// <remarks>The following data is also recognized: Model.</remarks>
        [HttpPost, ActionName("SaveEDIMDocStructelement")]
        public Models.Response SaveEDIMDocStructelement([System.Web.Http.FromBody]Models.EDIMasterDocStructElement dt)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLEDIMDocStructelement dalData = new DAL.NGLEDIMDocStructelement(Parameters);
                LTS.tblEDIMasterDocStructElement oChanges = selectLTSData(dt);
                //insert the new edi MDocStructLoopSegment
                LTS.tblEDIMasterDocStructElement oData = dalData.InsertMDocSegmentElement(oChanges);
                Models.EDIMasterDocStructElement[] oRecords = new Models.EDIMasterDocStructElement[1];
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
        /// This is used to return true or false record of EDIMasterDocStructElement which has deleted or not
        /// by passing testMstructelements having parameter
        /// </summary>
        /// <param name="testMstructelements">The testMstructelements class.</param>
        /// <returns>deleted Single record</returns>
        /// <remarks>The following data is also recognized: Model.</remarks>
        [HttpPost, ActionName("DeleteRecord")]
        public bool DeleteRecord(testMstructelements d1)
        {
            bool response = new bool();
            try
            {
                Models.EDIMasterDocStructElement[] ediMDocStructElements = new Models.EDIMasterDocStructElement[] { };
                DAL.NGLEDIMDocStructelement dalData = new DAL.NGLEDIMDocStructelement(Parameters);
                bool result = dalData.DeleteEDIMasterDocStructElements(Convert.ToInt32(d1.MDSElementControl));
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