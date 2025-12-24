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
    /// EDIDocStructElementController for EDI Master Doc Structure element Rest API Controls
    /// </summary>
    /// <Remarks>
    /// Created By SRP on 3/2/2018
    /// </Remarks>
    public class teststructelements
    {
        public string DSElementControl { get; set; }
    }
    public class EDIDocStructElementController : NGLControllerBase
    {
        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.EDIDocStructElementController";
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
        /// Selecting a EDIDocStructElement Model data by passing table records
        /// </summary>
        /// <param name="d">tblEDIDocStructElement Model</param>
        /// <returns>returns EDIDocStructElement table data</returns>
        /// Modified by SRP on 3/2/2018
        private Models.EDIDocStructElement selectModelData(LTS.tblEDIDocStructElement d)
        {
            Models.EDIDocStructElement modelEDIDocStructElement = new Models.EDIDocStructElement();
            //skipping values for reference to foreign keys added by SRP 28/2/2018
            List<string> skipEDIDocStructElement = new List<string> { "DSElementUpdated", "tblEDIDataType", "tblEDIDocStructSegment" };
            string sMsg = "";
            modelEDIDocStructElement = (Models.EDIDocStructElement)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelEDIDocStructElement, d, skipEDIDocStructElement, ref sMsg);
            if (modelEDIDocStructElement != null) { modelEDIDocStructElement.setUpdated(d.DSElementUpdated.ToArray()); }
            return modelEDIDocStructElement;
        }

        /// <summary>
        /// select table data by passing EDIDocStructElement Model
        /// </summary>
        /// <param name="d">EDIDocStructElement Model</param>
        /// <returns>returns tblEDIDocStructElement table data</returns>
        /// Modified by SRP on 3/7/18
        private LTS.tblEDIDocStructElement selectLTSData(Models.EDIDocStructElement d)
        {
            LTS.tblEDIDocStructElement ltsEDIDocStructElement = new LTS.tblEDIDocStructElement();
            if (d != null)
            {
                List<string> skipEDIDocStructElement = new List<string> { "DSElementUpdated" };
                string sMsg = "";
                ltsEDIDocStructElement = (LTS.tblEDIDocStructElement)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsEDIDocStructElement, d, skipEDIDocStructElement, ref sMsg);
                if (ltsEDIDocStructElement != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsEDIDocStructElement.DSElementUpdated = bupdated == null ? new byte[0] : bupdated;
                }
            }
            return ltsEDIDocStructElement;
        }
        #endregion


        #region "Rest Services"

        /// <summary>
        /// This is used to return a collection of Model record of EDIDocStructElement based on
        /// StructElement of Model passed to the method.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>An enumerable list of records</returns>
        /// Created by SRP on 3/7/18
        /// <remarks>The following data is also recognized: filters.</remarks>
        [HttpGet, ActionName("GetRecord")]
        public Models.Response GetRecord([System.Web.Http.FromBody]Models.EDIDocStructElement data)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                Models.EDIDocStructElement[] ediEDIDocStructElement = new Models.EDIDocStructElement[] { };
                DAL.NGLEDIDocStructElement dalData = new DAL.NGLEDIDocStructElement(Parameters);
                LTS.tblEDIDocStructElement oChanges = selectLTSData(data);
                LTS.tblEDIDocStructElement[] ltsEDIDocStructElement = dalData.GetEDIDocStructElement(oChanges);

                if (ltsEDIDocStructElement != null && ltsEDIDocStructElement.Count() > 0)
                {
                    count = ltsEDIDocStructElement.Count();
                    ediEDIDocStructElement = (from e in ltsEDIDocStructElement
                                           select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                //getting element records from tblEDISegment
                response = new Models.Response(ediEDIDocStructElement, count);
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
        /// This is used to return a collection of Model record of EDIDocStructElement 
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
                Models.EDIDocStructElement[] ediEDIDocStructElement = new Models.EDIDocStructElement[] { };
                DAL.NGLEDIDocStructElement oAn = new DAL.NGLEDIDocStructElement(Parameters);
                LTS.tblEDIDocStructElement[] ltsEDIDocStructElement = oAn.GetEDIDocStructElements(ref RecordCount, f);
                if (ltsEDIDocStructElement != null && ltsEDIDocStructElement.Count() > 0)
                {
                    count = ltsEDIDocStructElement.Count();
                    ediEDIDocStructElement = (from e in ltsEDIDocStructElement
                                              select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                //getting element records from tblEDIMasterDocStructElements
                response = new Models.Response(ediEDIDocStructElement, count);
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
        /// This is used to return a Model record of EDIDocStructElement which has updated
        /// </summary>
        /// <param name="data">The Model.</param>
        /// <returns>Updated Single record</returns>
        /// <remarks>The following data is also recognized: Model.</remarks>
        [HttpPost, ActionName("UpdateEDIDocStructElement")]
        public Models.Response UpdateEDIDocStructElement([System.Web.Http.FromBody]Models.EDIDocStructElement data)
        {
            // create a response message to send back
            var response = new Models.Response();//new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLEDIDocStructElement dalData = new DAL.NGLEDIDocStructElement(Parameters);
                LTS.tblEDIDocStructElement oChanges = selectLTSData(data);
                //updates the edi EDIDocStructElement
                LTS.tblEDIDocStructElement oData = dalData.UpdateEDIDocStructElement(oChanges);
                Models.EDIDocStructElement[] oRecords = new Models.EDIDocStructElement[1];
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
        /// This is used to return a Model record of EDIDocStructElement which has Inserted
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns>Inserted Single record</returns>
        /// <remarks>The following data is also recognized: Model.</remarks>
        [HttpPost, ActionName("SaveEDIDocStructElement")]
        public Models.Response SaveEDIDocStructElement([System.Web.Http.FromBody]Models.EDIDocStructElement dt)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLEDIDocStructElement dalData = new DAL.NGLEDIDocStructElement(Parameters);
                LTS.tblEDIDocStructElement oChanges = selectLTSData(dt);
                //insert the new edi MDocStructLoopSegment
                LTS.tblEDIDocStructElement oData = dalData.InsertEDIDocStructElement(oChanges);
                Models.EDIDocStructElement[] oRecords = new Models.EDIDocStructElement[1];
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
        /// This is used to return true or false record of EDIDocStructElement which has deleted or not
        /// based on DSElementControl by passing teststructelements class
        /// </summary>
        /// <param name="data">The Model.</param>
        /// <returns>deleted Single record</returns>
        /// <remarks>The following data is also recognized: Model.</remarks>
        [HttpPost, ActionName("DeleteRecord")]
        public bool DeleteRecord(teststructelements d1)
        {
            bool response = new bool();
            try
            {
                Models.EDIDocStructElement[] ediMDocStructElements = new Models.EDIDocStructElement[] { };
                DAL.NGLEDIDocStructElement dalData = new DAL.NGLEDIDocStructElement(Parameters);
                bool result = dalData.DeleteEDIDocStructElements(Convert.ToInt32(d1.DSElementControl));
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