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
    /// EDIDocumentTypeController for EDIDocuments Rest API Controls
    /// </summary>
    /// <seealso cref="DynamicsTMS365.Controllers.NGLControllerBase" />
    /// <Remarks>
    /// Created By SN on 2/19/2018
    /// </Remarks>
    public class EDIDocumentTypeController: NGLControllerBase
    {
        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.EDIDocumentTypeController";
        /// <summary>
        /// SourceClass Property for logging and error tracking
        /// </summary>
        /// <value>The source class.</value>
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }
        /// <summary>
        /// The request
        /// </summary>
        HttpRequest request = HttpContext.Current.Request;// Added by SN on 02/21/18
        #endregion

        #region " Data Translation"

        /// <summary>
        /// Selecting a EDIDocumentType Model data by passing table records
        /// </summary>
        /// <param name="d">The table data.</param>
        /// <returns>Returns EDIDocument Type</returns>
        /// Modified by SN on 2/21/18
        private Models.EDIDocumentType selectModelData(LTS.tblEDIType d)
        {
            Models.EDIDocumentType modelEDIDocumentType = new Models.EDIDocumentType();
            List<string> skipEDIDocumentType = new List<string> { "EDITUpdated" };
            string sMsg = "";
            modelEDIDocumentType = (Models.EDIDocumentType)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelEDIDocumentType, d, skipEDIDocumentType, ref sMsg);
            if (modelEDIDocumentType != null) { modelEDIDocumentType.setUpdated(d.EDITUpdated.ToArray()); }
            return modelEDIDocumentType;
        }

        /// <summary>
        /// select table data by passing Model
        /// </summary>
        /// <param name="d">DocumentType Model</param>
        /// <returns>returns tblEDIDocumentType table data</returns>
        /// Modified by SN on 2/21/18
        private LTS.tblEDIType selectLTSData(Models.EDIDocumentType d)
        {
            LTS.tblEDIType ltsEDIDocumentType = new LTS.tblEDIType();
            if (d != null)
            {
                List<string> skipEDIDocumentType = new List<string> { "EDITUpdated" };
                string sMsg = "";
                ltsEDIDocumentType = (LTS.tblEDIType)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsEDIDocumentType, d, skipEDIDocumentType, ref sMsg);
                if (ltsEDIDocumentType != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsEDIDocumentType.EDITUpdated = bupdated == null ? new byte[0] : bupdated;
                }
            }
            return ltsEDIDocumentType;
        }


        #endregion

        #region "Rest Services"
        /// <summary>
        /// This is used to return a collection of Model record of Document Type based on
        /// Doc Type of Model passed to the method.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>An enumerable list of records</returns>
        /// Created by SN on 2/21/18
        /// <remarks>The following data is also recognized: filters.</remarks>
        [HttpGet, ActionName("GetRecord")]
        public Models.Response GetRecord([System.Web.Http.FromBody]Models.EDIDocumentType data)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                Models.EDIDocumentType[] ediDocumenttypes = new Models.EDIDocumentType[] { };
                DAL.NGLEDIDocumentTypeData dalData = new DAL.NGLEDIDocumentTypeData(Parameters);
                LTS.tblEDIType oChanges = selectLTSData(data);
                LTS.tblEDIType[] ltsDocumenttypes = dalData.GetEDIDocument(oChanges.EDITControl);

                if (ltsDocumenttypes != null && ltsDocumenttypes.Count() > 0)
                {
                    count = ltsDocumenttypes.Count();
                    ediDocumenttypes = (from e in ltsDocumenttypes
                                          select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                //getting element records from tblEDIType
                response = new Models.Response(ediDocumenttypes, count);
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
        /// Created by SN on 2/21/18
        /// <summary>
        /// This is used to return a collection of Model record of Document Type based on
        /// Doc Type Name and Doc Type Description passed to the method.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>An enumerable list of records</returns>
        /// <remarks>The following data is also recognized: filters.</remarks>
        [HttpGet , ActionName("GetRecords")]
        public Models.Response GetRecords(string filter)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);

                Models.EDIDocumentType[] edidocuments = new Models.EDIDocumentType[] { };
                DAL.NGLEDIDocumentTypeData oAn = new DAL.NGLEDIDocumentTypeData(Parameters);
                LTS.tblEDIType[] ltsRet = oAn.GetEDIDocuments(ref RecordCount, f);
                if (ltsRet != null && ltsRet.Count() > 0)
                {
                    count = ltsRet.Count();
                    edidocuments = (from e in ltsRet
                                    select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                response = new Models.Response(edidocuments, count);
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
        /// Created by SN on 2/21/18
        /// <summary>
        /// This is used to return a Model record of Document Type which has updated
        /// </summary>
        /// <param name="data">The Model.</param>
        /// <returns>Updated Single record</returns>
        /// <remarks>The following data is also recognized: Model.</remarks>
        [HttpPost , ActionName("PostSave")]
        public Models.Response PostSave([System.Web.Http.FromBody]Models.EDIDocumentType data)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLEDIDocumentTypeData dalData = new DAL.NGLEDIDocumentTypeData(Parameters);
                LTS.tblEDIType oChanges = selectLTSData(data);
                LTS.tblEDIType oData = dalData.UpdateEDIDocumentType(oChanges);
                Models.EDIDocumentType[] oRecords = new Models.EDIDocumentType[1];
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
        /// This is used to return a Model record of Document Type which has Inserted
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns>Inserted Single record</returns>
        /// <remarks>The following data is also recognized: Model.</remarks>
        [HttpPost , ActionName("SaveEDIDocument")]
        public Models.Response SaveEDIDocument([System.Web.Http.FromBody]Models.EDIDocumentType dt)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLEDIDocumentTypeData dalData = new DAL.NGLEDIDocumentTypeData(Parameters);
               
                LTS.tblEDIType oChanges = selectLTSData(dt);
                LTS.tblEDIType oData = dalData.InsertEDIDocumentType(oChanges);
                Models.EDIDocumentType[] oRecords = new Models.EDIDocumentType[1];
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
        /// Created by SN on 2/21/18
        /// <summary>
        /// This is used to return true or false record of Document Type which has deleted or not
        /// </summary>
        /// <param name="data">The Model.</param>
        /// <returns>deleted Single record</returns>
        /// <remarks>The following data is also recognized: Model.</remarks>
        [HttpDelete, ActionName("DeleteRecord")]
        public bool DeleteRecord([System.Web.Http.FromBody]Models.EDIDocumentType data)
        {
            bool response = new bool();
            try
            {
                Models.EDIDocumentType[] ediEDIDocumentTypes = new Models.EDIDocumentType[] { };
                DAL.NGLEDIDocumentTypeData dalData = new DAL.NGLEDIDocumentTypeData(Parameters);
                LTS.tblEDIType oChanges = selectLTSData(data);
                bool result = dalData.DeleteEDIDocumentType(oChanges);

                //getting element records from tblEDIType
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