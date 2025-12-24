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
    //Save EDITPDocument by Passing this class as Parameter
    public class testsavetpdocument
    {
        public string TPDocControl { get; set; }
        public string TPDocCCEDIControl { get; set; }
        public string TPDocEDITControl { get; set; }
        public string TPDocInbound { get; set; }
        public string Action { get; set; }
    }
    /// <summary>
    /// EDITPDocumentController for EDI TP Document Rest API Controls
    /// </summary>
    /// <Remarks>
    /// Created By SRP on 3/1/2018
    /// </Remarks>
    public class EDITPDocumentController : NGLControllerBase
    {
        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.EDITPDocumentController";
        /// <summary>
        /// SourceClass Property for logging and error tracking
        /// </summary>
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;// Added by SRP on 3/1/18
        #endregion

        #region " Data Translation"
        /// <summary>
        /// Selecting a EDITPDocument Model data by passing table records
        /// </summary>
        /// <param name="d">tblEDITPDocument Model</param>
        /// <returns>returns EDITPDocument table data</returns>
        /// Modified by SRP on 3/7/2018
        private Models.EDITPDocument selectModelData(LTS.tblEDITPDocument d)
        {
            Models.EDITPDocument modelEDITPDocument = new Models.EDITPDocument();
            //skipping values for reference to foreign keys added by SRP 3/1/2018
            List<string> skipEDITPDocument = new List<string> { "TPDocModUpdated", "tblEDIType", "tblEDITPDocuments" };
            string sMsg = "";
            modelEDITPDocument = (Models.EDITPDocument)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelEDITPDocument, d, skipEDITPDocument, ref sMsg);
            if (modelEDITPDocument != null) { modelEDITPDocument.setUpdated(d.TPDocModUpdated.ToArray()); }
            return modelEDITPDocument;
        }
        /// <summary>
        /// Selecting a EDIDocument Model data by passing table records
        /// </summary>
        /// <param name="d">spEDIDocumentList</param>
        /// <returns>returns EDIDocument model data</returns>
        /// Modified by SRP on 3/7/2018
        private Models.EDIDocument selectCompModelData(LTS.spEDIDocumentListResult d)
        {
            Models.EDIDocument modelEDITPDocument = new Models.EDIDocument();
            //skipping values for reference to foreign keys added by SRP 3/1/2018
            List<string> skipEDITPDocument = new List<string> { };
            string sMsg = "";
            modelEDITPDocument = (Models.EDIDocument)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelEDITPDocument, d, skipEDITPDocument, ref sMsg);
            return modelEDITPDocument;
        }
        /// <summary>
        /// Selecting a EDITPDocument Model data by passing table records
        /// </summary>
        /// <param name="d">sp_PreviewEDITPDocumentConfig</param>
        /// <returns>returns EDITPDocument model data</returns>
        /// Modified by SRP on 3/7/2018
        private Models.EDITPDocument selectModelDataPreview(LTS.sp_PreviewEDITPDocumentConfigResult d)
        {
            Models.EDITPDocument modelEDIDocSegmentElements = new Models.EDITPDocument();
            List<string> skipEDIDocSegmentElement = new List<string> { "TPDocModUpdated", "tblEDIType", "tblEDITPDocuments" };
            string sMsg = "";
            modelEDIDocSegmentElements = (Models.EDITPDocument)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelEDIDocSegmentElements, d, skipEDIDocSegmentElement, ref sMsg);
            return modelEDIDocSegmentElements;
        }
        /// <summary>
        /// Selecting a CompCarrierModel data by passing table records
        /// </summary>
        /// <param name="d">vwGetCompCarrier</param>
        /// <returns>returns CompCarrierModel data</returns>
        /// Modified by SRP on 3/7/2018
        private Models.CompCarrierModel selectCompCarrierModelData(LTS.vwGetCompCarrier d)
        {
            Models.CompCarrierModel modelEDITPDocument = new Models.CompCarrierModel();
            //skipping values for reference to foreign keys added by SRP 3/1/2018
            List<string> skipEDITPDocument = new List<string> { };
            string sMsg = "";
            modelEDITPDocument = (Models.CompCarrierModel)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelEDITPDocument, d, skipEDITPDocument, ref sMsg);
            return modelEDITPDocument;
        }
        /// <summary>
        /// select table data by passing EDITPDocument Model
        /// </summary>
        /// <param name="d">EDIMasterDocument Model</param>
        /// <returns>returns EDITPDocument table data</returns>
        /// Modified by SRP on 3/7/18
        private LTS.tblEDITPDocument selectLTSData(Models.EDITPDocument d)
        {
            LTS.tblEDITPDocument ltsEDITPDocument = new LTS.tblEDITPDocument();
            if (d != null)
            {
                List<string> skipEDITPDocument = new List<string> { "TPDocModUpdated" };
                string sMsg = "";
                ltsEDITPDocument = (LTS.tblEDITPDocument)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsEDITPDocument, d, skipEDITPDocument, ref sMsg);
                if (ltsEDITPDocument != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsEDITPDocument.TPDocModUpdated = bupdated == null ? new byte[0] : bupdated;
                }
            }
            return ltsEDITPDocument;
        }
        #endregion


        #region "Rest Services"
        /// <summary>
        /// This is used to return a collection of Model record of EDITPDocument based on
        /// EDIMasterDocStructElmntAttribute of Model passed to the method.
        /// </summary>
        /// <param name="Model data">The Model data.</param>
        /// <returns>An enumerable list of records</returns>
        /// Created by SRP on 3/7/18
        /// <remarks>The following data is also recognized: filters.</remarks>
        [HttpGet, ActionName("GetRecord")]
        public Models.Response GetRecord([System.Web.Http.FromBody]Models.EDITPDocument data)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                Models.EDITPDocument[] ediEDITPDocuments = new Models.EDITPDocument[] { };
                DAL.EDITPDocument dalData = new DAL.EDITPDocument(Parameters);
                LTS.tblEDITPDocument oChanges = selectLTSData(data);
                LTS.tblEDITPDocument[] ltsEDITPDocument = dalData.GetEDITPDocument(oChanges);

                if (ltsEDITPDocument != null && ltsEDITPDocument.Count() > 0)
                {
                    count = ltsEDITPDocument.Count();
                    ediEDITPDocuments = (from e in ltsEDITPDocument
                                          select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                //getting element records from tblEDITPDocument
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

        /// Created by SRP on 3/7/18
        /// <summary>
        /// This is used to return a collection of Model record of EDITPDocument 
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
                Models.EDITPDocument[] editpdoc = new Models.EDITPDocument[] { };
                DAL.EDITPDocument oAn = new DAL.EDITPDocument(Parameters);
                LTS.tblEDITPDocument[] ltstpdocs = oAn.GetEDITPDocuments(ref RecordCount, f);
                if (ltstpdocs != null && ltstpdocs.Count() > 0)
                {
                    count = ltstpdocs.Count();
                    editpdoc = (from e in ltstpdocs
                                            select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                //getting element records from tblEDITPDocuments
                response = new Models.Response(editpdoc, count);
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
        /// This is used to return a Model record of EDITPDocument which has updated
        /// </summary>
        /// <param name="data">The Model.</param>
        /// <returns>Updated Single record</returns>
        /// <remarks>The following data is also recognized: Model.</remarks>
        [HttpPost, ActionName("UpdateTPDocuments")]
        public Models.Response UpdateTPDocuments([System.Web.Http.FromBody]Models.EDITPDocument data)
        {
            // create a response message to send back
            var response = new Models.Response();//new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.EDITPDocument dalData = new DAL.EDITPDocument(Parameters);
                LTS.tblEDITPDocument oChanges = selectLTSData(data);
                //updates the edi tp document
                LTS.tblEDITPDocument oData = dalData.UpdateEDITPDocument(oChanges);
                Models.EDITPDocument[] oRecords = new Models.EDITPDocument[1];
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
        /// This is used to return a Model record of EDITPDocument which has Inserted
        /// </summary>
        /// <param name="testsavetpdocument">The testsavetpdocument class having parameters.</param>
        /// <returns>Inserted Single record</returns>
        /// <remarks>The following data is also recognized: Model.</remarks>
        [HttpPost, ActionName("SaveEDITPDocumentMap")]
        public Models.Response SaveEDITPDocumentMap(testsavetpdocument d1)
        {
            if (d1.TPDocInbound == "InBound"){ d1.TPDocInbound = "true";}
            else if (d1.TPDocInbound == "true"){d1.TPDocInbound = "true";}
            else {d1.TPDocInbound = "false";}
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.EDITPDocument dalData = new DAL.EDITPDocument(Parameters);
                Models.EDITPDocument data = new Models.EDITPDocument();
                data.TPDocControl = Convert.ToInt32(d1.TPDocControl);
                data.TPDocCCEDIControl = Convert.ToInt32(d1.TPDocCCEDIControl);
                data.TPDocEDITControl = Convert.ToInt32(d1.TPDocEDITControl);
                data.TPDocInbound = Convert.ToBoolean(d1.TPDocInbound);
                LTS.tblEDITPDocument oChanges = selectLTSData(data);
                //insert the new edi MDocStructLoopSegment
                LTS.tblEDITPDocument oData = dalData.InsertEDITPDocument(oChanges, d1.Action);
                //LTS.tblEDITPDocument oData = dalData.InsertEDITPDocument(Convert.ToInt32(d1.TPDocControl),Convert.ToInt32(d1.TPDocCCEDIControl), Convert.ToInt32(d1.TPDocEDITControl), Convert.ToBoolean(d1.TPDocInbound),d1.Action);
                Models.EDITPDocument[] otpdocuments = new Models.EDITPDocument[1];
                otpdocuments[0] = selectModelData(oData);
                response = new Models.Response(otpdocuments, 1);
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
        /// This is used to return a collection of Model record of EDITPDocument
        /// based on company as filter
        /// </summary>
        /// <returns>An enumerable list of records</returns>
        [HttpGet, ActionName("GetCompRecords")]
        public Models.Response GetCompRecords(string filter)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                Models.EDIDocument[] editpdoc = new Models.EDIDocument[] { };
                DAL.EDITPDocument oAn = new DAL.EDITPDocument(Parameters);
                LTS.spEDIDocumentListResult[] ltstpdocs = null;
                if (f.filterValue == "")
                {
                    ltstpdocs = oAn.GetEDITPDocumentList(ref RecordCount, f);
                }
                else
                {
                    if (f.filterName == "EDITName")
                        ltstpdocs = oAn.GetEDITPDocumentList(ref RecordCount, f).Where(model => model.EDITName.Contains(f.filterValue)).ToArray();
                    else if (f.filterName == "CarrierName")
                        ltstpdocs = oAn.GetEDITPDocumentList(ref RecordCount, f).Where(model => model.CarrierName.Contains(f.filterValue)).ToArray();
                    else if (f.filterName == "CompName")
                        ltstpdocs = oAn.GetEDITPDocumentList(ref RecordCount, f).Where(model => model.CompName.Contains(f.filterValue)).ToArray();
                    else if (f.filterName == "EDITControl")
                        ltstpdocs = oAn.GetEDITPDocumentList(ref RecordCount, f).Where(model => model.EDITControl.Equals(f.filterValue)).ToArray();
                }
                if (ltstpdocs != null && ltstpdocs.Count() > 0)
                {
                    count = ltstpdocs.Count();
                    editpdoc = (from e in ltstpdocs
                                select selectCompModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                //getting element records from tblEDITPDocuments
                response = new Models.Response(editpdoc, count);
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
        /// This is used to return a collection of Model record of EDITPDocument
        /// based on company as filter
        /// /// <param name="filter">The filter.</param>
        /// </summary>
        /// <returns>An enumerable list of records</returns>
        [HttpGet, ActionName("GetCompCarriers")]
        public Models.Response GetCompCarriers(string filter)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                Models.CompCarrierModel[] editpdoc = new Models.CompCarrierModel[] { };
                DAL.EDITPDocument oAn = new DAL.EDITPDocument(Parameters);
                LTS.vwGetCompCarrier[] ltstpdocs = null;
                ltstpdocs = oAn.GetCompCarrierList();
                if (ltstpdocs != null && ltstpdocs.Count() > 0)
                {
                    count = ltstpdocs.Count();
                    editpdoc = (from e in ltstpdocs
                                select selectCompCarrierModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                //getting element records from tblEDITPDocuments
                response = new Models.Response(editpdoc, count);
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
        /// This is used to return a preview record of EDITPDocument
        /// based on TPDocControl
        /// <param name="TPDocControl">The TPDocControl.</param>
        /// </summary>
        /// <returns>An enumerable list of records</returns>
        [HttpGet, ActionName("GetTPDocumentPreview")]
        public Models.Response GetTPDocumentPreview(string TPDocControl)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                DAL.EDITPDocument dalData = new DAL.EDITPDocument(Parameters);
                Models.EDITPDocument[] EDIMDocStructLoops = new Models.EDITPDocument[] { };
                LTS.sp_PreviewEDITPDocumentConfigResult[] ltsMDocStructsegments = dalData.GetTPDocumentPreview(Convert.ToInt32(TPDocControl));
                if (ltsMDocStructsegments != null)
                {
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
        /// This is used to copy from TP Doc to master Doc
        /// based on MasterDocControl and TPDocControl
        /// </summary>
        /// <param name="MasterDocControl">The MasterDocControl.</param>
        /// <param name="TPDocControl">The TPDocControl.</param>
        /// <returns>An enumerable list of records</returns>
        [HttpGet, ActionName("CopyTPDocConfigToMasterDoc")]
        public Models.Response CopyTPDocConfigToMasterDoc(string MasterDocControl,string TPDocControl)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                DAL.EDITPDocument dalData = new DAL.EDITPDocument(Parameters);
                Models.EDITPDocument[] EDIMDocStructLoops = new Models.EDITPDocument[] { };
                var ltsMDocStructsegments = dalData.CopyTPDocConfigToMasterDoc(Convert.ToInt32(MasterDocControl), Convert.ToInt32(TPDocControl));
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
        /// This is used to return a collection of Model record of EDITPDocument
        /// based on TPDocCCEDIControl and TPDocEDITControl and TPDocInbound
        /// </summary>
        /// <param name="TPDocCCEDIControl">The TPDocCCEDIControl.</param>
        /// <param name="TPDocEDITControl">The TPDocEDITControl.</param>
        /// <param name="TPDocInbound">The TPDocInbound.</param>
        /// <returns>An enumerable list of records</returns>
        [HttpGet, ActionName("GetRecordByCarrier")]
        public Models.Response GetRecordByCarrier(string TPDocCCEDIControl, string TPDocEDITControl,string TPDocInbound)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                if (TPDocInbound == "InBound"){TPDocInbound = "true";}
                else if (TPDocInbound == "true"){TPDocInbound = "true";}
                else {TPDocInbound = "false";}
                int count = 0;
                int RecordCount = 0;
                Models.EDITPDocument[] ediEDITPDocuments = new Models.EDITPDocument[] { };
                DAL.EDITPDocument dalData = new DAL.EDITPDocument(Parameters);
                LTS.tblEDITPDocument[] ltsEDITPDocument = dalData.GetEDITPDocumentByCarrier(Convert.ToInt32(TPDocCCEDIControl), Convert.ToInt32(TPDocEDITControl), Convert.ToBoolean(TPDocInbound));

                if (ltsEDITPDocument != null && ltsEDITPDocument.Count() > 0)
                {
                    count = ltsEDITPDocument.Count();
                    ediEDITPDocuments = (from e in ltsEDITPDocument
                                         select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                //getting element records from tblEDITPDocument
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

        /// Created by SRP on 3/7/18
        /// <summary>
        /// This is used to return a collection of Model record of EDITPDocument
        /// based on TPDocCCEDIControl and TPDocEDITControl and TPDocInbound
        /// </summary>
        /// <param name="TPDocCCEDIControl">The TPDocCCEDIControl.</param>
        /// <param name="TPDocEDITControl">The TPDocEDITControl.</param>
        /// <param name="TPDocInbound">The TPDocInbound.</param>
        /// <returns>An enumerable list of records</returns>
        [HttpGet, ActionName("GetRecordByCarrierID")]
        public Models.Response GetRecordByCarrierID(string TPDocCCEDIControl, string TPDocEDITControl, string TPDocInbound)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                if (TPDocInbound == "InBound") { TPDocInbound = "true"; }
                else if (TPDocInbound == "true") { TPDocInbound = "true"; }
                else { TPDocInbound = "false"; }
                int count = 0;
                int RecordCount = 0;
                Models.EDITPDocument[] ediEDITPDocuments = new Models.EDITPDocument[] { };
                DAL.EDITPDocument dalData = new DAL.EDITPDocument(Parameters);
                LTS.tblEDITPDocument[] ltsEDITPDocument = dalData.GetEDITPDocumentByCarrierID(Convert.ToInt32(TPDocCCEDIControl), Convert.ToInt32(TPDocEDITControl), Convert.ToBoolean(TPDocInbound));

                if (ltsEDITPDocument != null && ltsEDITPDocument.Count() > 0)
                {
                    count = ltsEDITPDocument.Count();
                    ediEDITPDocuments = (from e in ltsEDITPDocument
                                         select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                //getting element records from tblEDITPDocument
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


        /// Created by SRP on 2/26/18
        /// <summary>
        /// This is used to return true or false record of EDITPDocument which has deleted or not
        /// </summary>
        /// <param name="data">The Model.</param>
        /// <returns>deleted Single record</returns>
        /// <remarks>The following data is also recognized: Model.</remarks>
        [HttpDelete, ActionName("DeleteTPDocumentRecord")]
        public int DeleteTPDocumentRecord([System.Web.Http.FromBody]Models.EDITPDocument data)
        {
            int response = new int();
            try
            {
                Models.EDITPDocument[] EDIMasterDocuments = new Models.EDITPDocument[] { };
                DAL.EDITPDocument dalData = new DAL.EDITPDocument(Parameters);
                int result = dalData.DeleteEDITPDocumentFull(Convert.ToInt32(data.TPDocControl));
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
        /// This is used to copy from master Doc to TP Doc
        /// based on MasterDocControl and TPDocControl
        /// </summary>
        /// <param name="MasterDocControl">The MasterDocControl.</param>
        /// <param name="TPDocControl">The TPDocControl.</param>
        /// <returns>An enumerable list of records</returns>
        [HttpGet, ActionName("CopyMDocConfigtoTPEDIDoc")]
        public Models.Response CopyMDocConfigtoTPEDIDoc(string MasterDocControl, string TPDocControl)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                DAL.EDITPDocument dalData = new DAL.EDITPDocument(Parameters);
                Models.EDITPDocument[] EDIMDocStructLoops = new Models.EDITPDocument[] { };
                var ltsMDocStructsegments = dalData.CopyMDocConfigtoTPEDIDoc(Convert.ToInt32(MasterDocControl), Convert.ToInt32(TPDocControl));
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
        /// Created by SRP on 4/26/18
        /// <summary>
        /// This is used to check record of EDITPDocument is exist for EDIMasterDocument
        /// </summary>
        /// <param name="MasterDocEDITControl">The MasterDocEDITControl.</param>
        /// /// <param name="MasterDocInbound">The MasterDocInbound.</param>
        /// <returns>An enumerable list of records</returns>
        [HttpGet, ActionName("CheckAvailableTPDoc")]
        public Models.Response CheckAvailableTPDoc(string MasterDocEDITControl, string MasterDocInbound)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                Models.EDITPDocument[] ediEDITPDocuments = new Models.EDITPDocument[] { };
                DAL.EDITPDocument dalData = new DAL.EDITPDocument(Parameters);
                LTS.tblEDITPDocument[] ltsEDITPDocument = dalData.GetEDITPDocumentByEDITControl(Convert.ToInt32(MasterDocEDITControl), Convert.ToBoolean(MasterDocInbound));
                if (ltsEDITPDocument != null && ltsEDITPDocument.Count() > 0)
                {
                    count = ltsEDITPDocument.Count();
                    ediEDITPDocuments = (from e in ltsEDITPDocument
                                         select selectModelData(e)).ToArray();
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
        #endregion
    }
}