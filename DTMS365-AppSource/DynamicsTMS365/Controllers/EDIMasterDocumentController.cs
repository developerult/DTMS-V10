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
    /// EDIMasterDocumentController for Documents Rest API Controls
    /// </summary>
    /// <Remarks>
    /// Created By SRP on 2/27/2018
    /// </Remarks>
    public class EDIMasterDocumentController : NGLControllerBase
    {
        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.EDIMasterDocumentController";
        /// <summary>
        /// SourceClass Property for logging and error tracking
        /// </summary>
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;// Added by SRP on 02/27/18
        #endregion

        #region " Data Translation"

        /// <summary>
        /// Selecting a EDIMasterDocument Model data by passing table records
        /// </summary>
        /// <param name="d">tblEDIMasterDocument Model</param>
        /// <returns>returns EDIMasterDocument table data</returns>
        /// Modified by SRP on 3/7/2018
        private Models.EDIMasterDocument selectModelData(LTS.tblEDIMasterDocument d)
        {
            Models.EDIMasterDocument modeledimasterdocuments = new Models.EDIMasterDocument();
            //skipping values for reference to foreign keys added by SRP 27/2/2018
            List<string> skipEDIMasterDocuments = new List<string> { "MasterDocModUpdated", "tblEDIType" };
            string sMsg = "";
            modeledimasterdocuments = (Models.EDIMasterDocument)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modeledimasterdocuments, d, skipEDIMasterDocuments, ref sMsg);
            if (modeledimasterdocuments != null) { modeledimasterdocuments.setUpdated(d.MasterDocModUpdated.ToArray()); }
            return modeledimasterdocuments;
        }
        /// <summary>
        /// Selecting a EDIMasterDocument Model data by passing table records
        /// </summary>
        /// <param name="d">tblEDIMasterDocument Model</param>
        /// <returns>returns EDIMasterDocument table data</returns>
        /// Modified by SRP on 3/7/2018
        private Models.EDIMasterDocument selectModelDataExist(LTS.tblEDIMasterDocument d)
        {
            Models.EDIMasterDocument modelEDIMasterDocument = new Models.EDIMasterDocument();
            //skipping values for reference to foreign keys added by SRP 3/1/2018
            List<string> skipEDIMasterDocument = new List<string> { "MasterDocModUpdated", "tblEDIType" };
            string sMsg = "";
            modelEDIMasterDocument = (Models.EDIMasterDocument)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelEDIMasterDocument, d, skipEDIMasterDocument, ref sMsg);
            if (modelEDIMasterDocument != null) { modelEDIMasterDocument.setUpdated(d.MasterDocModUpdated.ToArray()); }
            return modelEDIMasterDocument;
        }
        /// <summary>
        /// Selecting a EDIMasterDocument Model data by passing view records
        /// </summary>
        /// <param name="d">vw_GetMasterDocument</param>
        /// <returns>returns EDIMasterDocument table data</returns>
        /// Modified by SRP on 3/7/2018
        private Models.EDIMasterDocument selectModelData(LTS.vw_GetMasterDocument d)
        {
            Models.EDIMasterDocument modeledimasterdocuments = new Models.EDIMasterDocument();
            //skipping values for reference to foreign keys added by SRP 27/2/2018
            List<string> skipEDIMasterDocuments = new List<string> { "MasterDocModUpdated", "tblEDIType" };
            string sMsg = "";
            modeledimasterdocuments = (Models.EDIMasterDocument)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modeledimasterdocuments, d, skipEDIMasterDocuments, ref sMsg);
            //if (modeledimasterdocuments != null) { modeledimasterdocuments.setUpdated(d.MasterDocModUpdated.ToArray()); }
            return modeledimasterdocuments;
        }
        /// <summary>
        /// Selecting a EDIMasterDocument Model data by passing sp records
        /// </summary>
        /// <param name="d">sp_PreviewEDIMasterDocumentConfig</param>
        /// <returns>returns EDIMasterDocument table data</returns>
        /// Modified by SRP on 3/7/2018
        private Models.EDIMasterDocument selectModelDataPreview(LTS.sp_PreviewEDIMasterDocumentConfigResult d)
        {
            Models.EDIMasterDocument modelEDIDocSegmentElements = new Models.EDIMasterDocument();
            List<string> skipEDIDocSegmentElement = new List<string> { "MasterDocModUpdated", "tblEDIType"};
            string sMsg = "";
            modelEDIDocSegmentElements = (Models.EDIMasterDocument)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelEDIDocSegmentElements, d, skipEDIDocSegmentElement, ref sMsg);

            return modelEDIDocSegmentElements;
        }
        /// <summary>
        /// select table data by passing EDIMasterDocument Model
        /// </summary>
        /// <param name="d">EDIMasterDocument Model</param>
        /// <returns>returns tblEDIMasterDocument table data</returns>
        /// Modified by SRP on 3/7/18
        private LTS.tblEDIMasterDocument selectLTSData(Models.EDIMasterDocument d)
        {
            LTS.tblEDIMasterDocument ltsedimasterdocuments = new LTS.tblEDIMasterDocument();
            if (d != null)
            {
                List<string> skipEDIMasterDocuments = new List<string> { "MasterDocModUpdated" };
                string sMsg = "";
                ltsedimasterdocuments = (LTS.tblEDIMasterDocument)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsedimasterdocuments, d, skipEDIMasterDocuments, ref sMsg);
                if (ltsedimasterdocuments != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsedimasterdocuments.MasterDocModUpdated = bupdated == null ? new byte[0] : bupdated;
                }
            }
            return ltsedimasterdocuments;
        }
        #endregion

        #region "Rest Services"

        /// <summary>
        /// This is used to return a collection of Model record of EDIMasterDocument based on
        /// MasterDocControl of Model passed to the method.
        /// </summary>
        /// <param name="Model data">The Model data.</param>
        /// <returns>An enumerable list of records</returns>
        /// Created by SRP on 3/7/18
        /// <remarks>The following data is also recognized: filters.</remarks>
        [HttpGet, ActionName("GetRecord")]
        public Models.Response GetRecord(string MasterDocControl)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                Models.EDIMasterDocument[] ediMasterDocuments = new Models.EDIMasterDocument[] { };
                DAL.NGLEDIMasterDocument dalData = new DAL.NGLEDIMasterDocument(Parameters);
                //LTS.tblEDIMasterDocument oChanges = selectLTSData(data);
                LTS.tblEDIMasterDocument[] ltsEDIMasterDocument = dalData.GetEDIMasterDocument(Convert.ToInt32(MasterDocControl));

                if (ltsEDIMasterDocument != null && ltsEDIMasterDocument.Count() > 0)
                {
                    count = ltsEDIMasterDocument.Count();
                    ediMasterDocuments = (from e in ltsEDIMasterDocument
                                           select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                //getting element records from tblEDISegment
                response = new Models.Response(ediMasterDocuments, count);
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
                Models.EDIMasterDocument[] edimasterdocuments = new Models.EDIMasterDocument[] { };
                DAL.NGLEDIMasterDocument oAn = new DAL.NGLEDIMasterDocument(Parameters);
                //getting element records from tblEDIMasterDocuments_
                LTS.tblEDIMasterDocument[] ltsRet = oAn.GetEDIMasterDocuments(ref RecordCount, f);
                if (ltsRet != null && ltsRet.Count() > 0)
                {
                    count = ltsRet.Count();
                    edimasterdocuments = (from e in ltsRet
                                    select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                response = new Models.Response(edimasterdocuments, count);
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
        /// This is used to return a Model record of EDIMasterDocument which has updated
        /// </summary>
        /// <param name="data">The Model.</param>
        /// <returns>Updated Single record</returns>
        /// <remarks>The following data is also recognized: Model.</remarks>
        [HttpPost, ActionName("PostSave")]
        public Models.Response PostSave([System.Web.Http.FromBody]Models.EDIMasterDocument data)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLEDIMasterDocument dalData = new DAL.NGLEDIMasterDocument(Parameters);
                LTS.tblEDIMasterDocument oChanges = selectLTSData(data);
                //updates the edi materdocuments
                LTS.tblEDIMasterDocument oData = dalData.UpdateEDIMasterDocument(oChanges);
                Models.EDIMasterDocument[] oRecords = new Models.EDIMasterDocument[1];
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
        [HttpPost, ActionName("SaveEDIMasterDocument")]
        public Models.Response SaveEDIMasterDocument([System.Web.Http.FromBody]Models.EDIMasterDocument dt)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLEDIMasterDocument dalData = new DAL.NGLEDIMasterDocument(Parameters);
                LTS.tblEDIMasterDocument oChanges = selectLTSData(dt);
                //insert the new edi masterdocument
                LTS.tblEDIMasterDocument oData = dalData.InsertEDIMasterDocument(oChanges);
                Models.EDIMasterDocument[] oRecords = new Models.EDIMasterDocument[1];
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
        /// This is used to return a preview record of EDIMasterDocument
        /// based on MasterDocControl
        /// </summary>
        /// <returns>An enumerable list of records</returns>
        [HttpGet, ActionName("GetMasterDocPreview")]
        public Models.Response GetMasterDocPreview(string MasterDocControl)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                DAL.NGLEDIMasterDocument dalData = new DAL.NGLEDIMasterDocument(Parameters);
                Models.EDIMasterDocument[] EDIMDocStructLoops = new Models.EDIMasterDocument[] { };
                LTS.sp_PreviewEDIMasterDocumentConfigResult[] ltsMDocStructsegments = dalData.GetMDocLoopPreview(Convert.ToInt32(MasterDocControl));
                if (ltsMDocStructsegments != null)
                {
                    // count = ltsMDocStructsegments.Count();
                    EDIMDocStructLoops = (from e in ltsMDocStructsegments
                                          select selectModelDataPreview(e)).ToArray();
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
        /// This is used to return true or false record of EDIMasterDocument which has deleted or not
        /// </summary>
        /// <param name="data">The Model.</param>
        /// <returns>deleted Single record</returns>
        /// <remarks>The following data is also recognized: Model.</remarks>
        [HttpDelete, ActionName("DeleteRecord")]
        public bool DeleteRecord([System.Web.Http.FromBody]Models.EDIMasterDocument data)
        {
            bool response = new bool();
            /*if (!authenticateController(bool response)) { return response; }*/
            try
            {
                Models.EDIMasterDocument[] EDIMasterDocuments = new Models.EDIMasterDocument[] { };
                DAL.NGLEDIMasterDocument dalData = new DAL.NGLEDIMasterDocument(Parameters);
                LTS.tblEDIMasterDocument oChanges = selectLTSData(data);
                bool result = dalData.DeleteEDIMasterDocument(oChanges);

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
        /// Created by SRP on 3/7/18
        /// <summary>
        /// This is used to return a record of GetMasterEDIDocRecords
        /// </summary>
        /// <returns>An enumerable list of records</returns>
        [HttpGet, ActionName("GetMasterEDIDocRecords")]
        public Models.Response GetMasterEDIDocRecords()
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                DAL.NGLEDIMasterDocument dalData = new DAL.NGLEDIMasterDocument(Parameters);
                Models.EDIMasterDocument[] EDIMDocStructLoops = new Models.EDIMasterDocument[] { };
                LTS.vw_GetMasterDocument[] ltsMDocStructsegments = null;
                ltsMDocStructsegments = dalData.GetEDIMasterDoc();
                if (ltsMDocStructsegments != null && ltsMDocStructsegments.Count() > 0)
                {
                    count = ltsMDocStructsegments.Count();
                    EDIMDocStructLoops = (from e in ltsMDocStructsegments
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
        /// This is used to return true or false record of EDIMasterDocument which has deleted or not
        /// </summary>
        /// <param name="data">The Model.</param>
        /// <returns>deleted Single record</returns>
        /// <remarks>The following data is also recognized: Model.</remarks>
        [HttpDelete, ActionName("DeleteMasterRecord")]
        public int DeleteMasterRecord([System.Web.Http.FromBody]Models.EDIMasterDocument data)
        {

            int response = new int();
            try
            {
                Models.EDIMasterDocument[] EDIMasterDocuments = new Models.EDIMasterDocument[] { };
                DAL.NGLEDIMasterDocument dalData = new DAL.NGLEDIMasterDocument(Parameters);
                //LTS.tblEDIMasterDocument oChanges = selectLTSData(data);
                int result = dalData.DeleteEDIMasterDocumentFull(Convert.ToInt32(data.MasterDocControl));
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
        /// Created by SRP on 3/2/18
        /// <summary>
        /// This is used to check record of EDIMasterDocument is exist or not based on
        /// ElementControl
        /// </summary>
        /// <param name="MasterDocEDITControl">The MasterDocEDITControl.</param>
        /// /// <param name="MasterDocInbound">The MasterDocInbound.</param>
        /// <returns>An enumerable list of records</returns>
        [HttpGet, ActionName("CheckExist")]
        public Models.Response CheckExist(string MasterDocEDITControl, string MasterDocInbound)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                Models.EDIMasterDocument[] ediEDITPDocuments = new Models.EDIMasterDocument[] { };
                DAL.NGLEDIMasterDocument dalData = new DAL.NGLEDIMasterDocument(Parameters);
                LTS.tblEDIMasterDocument[] ltsEDITPDocument = dalData.GetEDIMsDocumentByEDITControl(Convert.ToInt32(MasterDocEDITControl), Convert.ToBoolean(MasterDocInbound));
                if (ltsEDITPDocument != null && ltsEDITPDocument.Count() > 0)
                {
                    count = ltsEDITPDocument.Count();
                    ediEDITPDocuments = (from e in ltsEDITPDocument
                                         select selectModelDataExist(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                response = new Models.Response(ediEDITPDocuments, count);
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
        /// This is used to return a collection of Model record of EDIMasterDocument based on
        /// filter
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>An enumerable list of records</returns>
        [HttpGet, ActionName("GetMasterEDIDocRecordsByMasterID")]
        public Models.Response GetMasterEDIDocRecordsByMasterID(string filter)
        {
            
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                DAL.NGLEDIMasterDocument dalData = new DAL.NGLEDIMasterDocument(Parameters);
                Models.EDIMasterDocument[] EDIMDocStructLoops = new Models.EDIMasterDocument[] { };
                LTS.vw_GetMasterDocument[] ltsMDocStructsegments = null;
                if (f.filterName == "EDITName")
                {
                    ltsMDocStructsegments = dalData.GetEDIMasterDoc().Where(r => r.EDITName == f.filterValue).ToArray();
                }
                else
                {
                    if (f.filterValue == "InBound") { f.filterValue = "true"; } else { f.filterValue = "false"; }
                    ltsMDocStructsegments = dalData.GetEDIMasterDoc().Where(r => r.MasterDocInbound == Convert.ToBoolean(f.filterValue)).ToArray();
                }
                if (ltsMDocStructsegments != null && ltsMDocStructsegments.Count() > 0)
                {
                    count = ltsMDocStructsegments.Count();
                    EDIMDocStructLoops = (from e in ltsMDocStructsegments
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

        #endregion
    }
}