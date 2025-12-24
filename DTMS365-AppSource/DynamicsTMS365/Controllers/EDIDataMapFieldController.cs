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
    /// EDIDataMapFieldController for DataMap Rest API Controls
    /// </summary>
    /// <Remarks>
    /// Created By SRP on 2/27/2018
    /// </Remarks>
    public class EDIDataMapFieldController : NGLControllerBase
    {
        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.EDIDataMapFieldController";
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
        /// Selecting a EDIDataMapField Model data by passing table records
        /// </summary>
        /// <param name="d">tblEDIDataMapField</param>
        /// <returns>returns EDIDataMapField table data</returns>
        /// Modified by SRP on 3/7/2018
        private Models.EDIDataMapField selectModelData(LTS.tblEDIDataMapField d)
        {
            Models.EDIDataMapField modeledidatamapfield = new Models.EDIDataMapField();

            //skipping values for reference to foreign keys added by SRP 27/2/2018
            List<string> skipObjs = new List<string> { "DataMapFieldModUpdated", "tblEDIType", "tblEDIDataType"};
            string sMsg = "";
            modeledidatamapfield = (Models.EDIDataMapField)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modeledidatamapfield, d, skipObjs, ref sMsg);
            if (modeledidatamapfield != null) { modeledidatamapfield.setUpdated(d.DataMapFieldModUpdated.ToArray()); }
            return modeledidatamapfield;
        }
        /// <summary>
        /// Selecting a EDIDataMapField Model data by passing table records
        /// </summary>
        /// <param name="d">vw_tblEDIDataMapField</param>
        /// <returns>returns EDIDataMapField table data</returns>
        /// Modified by SRP on 3/7/2018
        private Models.EDIDataMapField selectModelDataGetEDIDocStructElmntAttribute(LTS.vw_tblEDIDataMapField d)
        {
            Models.EDIDataMapField modelEDIDataMapFields = new Models.EDIDataMapField();
            List<string> skipEDIDataMapField = new List<string> { "DataMapFieldModUpdated" };
            string sMsg = "";
            modelEDIDataMapFields = (Models.EDIDataMapField)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelEDIDataMapFields, d, skipEDIDataMapField, ref sMsg);

            return modelEDIDataMapFields;
        }
        /// <summary>
        /// select table data by passing EDIDataMapField Model
        /// </summary>
        /// <param name="d">EDIMasterDocument Model</param>
        /// <returns>returns tblEDIDataMapField table data</returns>
        /// Modified by SRP on 3/7/18
        private LTS.tblEDIDataMapField selectLTSData(Models.EDIDataMapField d)
        {
            LTS.tblEDIDataMapField ltsedidatamapfield = new LTS.tblEDIDataMapField();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "DataMapFieldModUpdated" };
                string sMsg = "";
                ltsedidatamapfield = (LTS.tblEDIDataMapField)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsedidatamapfield, d, skipObjs, ref sMsg);
                if (ltsedidatamapfield != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsedidatamapfield.DataMapFieldModUpdated = bupdated == null ? new byte[0] : bupdated;
                }
            }
            return ltsedidatamapfield;
        }
        #endregion

        #region "Rest Services"

        /// <summary>
        /// This is used to return a collection of Model record of EDIDataMapField based on
        /// EDIDataMapField of Model passed to the method.
        /// </summary>
        /// <param name="Model data">The Model data.</param>
        /// <returns>An enumerable list of records</returns>
        /// Created by SRP on 3/7/18
        /// <remarks>The following data is also recognized: filters.</remarks>
        [HttpGet, ActionName("GetRecord")]
        public Models.Response GetRecord([System.Web.Http.FromBody]Models.EDIDataMapField data)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                Models.EDIDataMapField[] ediEDIDataMapFields = new Models.EDIDataMapField[] { };
                DAL.NGLEDIDataMapField dalData = new DAL.NGLEDIDataMapField(Parameters);
                LTS.tblEDIDataMapField oChanges = selectLTSData(data);
                LTS.tblEDIDataMapField[] ltsEDIDataMapField = dalData.GetEDIDataMapField(oChanges.DataMapFieldControl);

                if (ltsEDIDataMapField != null && ltsEDIDataMapField.Count() > 0)
                {
                    count = ltsEDIDataMapField.Count();
                    ediEDIDataMapFields = (from e in ltsEDIDataMapField
                                   select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                //getting element records from tblEDISegment
                response = new Models.Response(ediEDIDataMapFields, count);
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
        /// This is used to return a collection of Model record of EDIDataMapField 
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
                Models.EDIDataMapField[] edidatamapfields = new Models.EDIDataMapField[] { };
                DAL.NGLEDIDataMapField oAn = new DAL.NGLEDIDataMapField(Parameters);
                //getting element records from tblEDIDataMapField
                LTS.tblEDIDataMapField[] ltsRet = oAn.GetEDIDataMapFields(ref RecordCount, f);
                if (ltsRet != null && ltsRet.Count() > 0)
                {
                    count = ltsRet.Count();
                    edidatamapfields = (from e in ltsRet
                                    select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                response = new Models.Response(edidatamapfields, count);
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
        /// This is used to return a Model record of EDIDataMapField which has updated
        /// </summary>
        /// <param name="data">The Model.</param>
        /// <returns>Updated Single record</returns>
        /// <remarks>The following data is also recognized: Model.</remarks>
        [HttpPost, ActionName("PostSave")]
        public Models.Response PostSave([System.Web.Http.FromBody]Models.EDIDataMapField data)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLEDIDataMapField dalData = new DAL.NGLEDIDataMapField(Parameters);
                LTS.tblEDIDataMapField oChanges = selectLTSData(data);
                //updates the edi datamapfields
                LTS.tblEDIDataMapField oData = dalData.UpdateEDIDataMapField(oChanges);
                Models.EDIDataMapField[] oRecords = new Models.EDIDataMapField[1];
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
        /// This is used to return a Model record of EDIDataMapField which has Inserted
        /// </summary>
        /// <param name="dt">The EDIDataMapField model.</param>
        /// <returns>Inserted Single record</returns>
        /// <remarks>The following data is also recognized: Model.</remarks>
        [HttpPost, ActionName("SaveEDIDataMapField")]
        public Models.Response SaveEDIDataMapField([System.Web.Http.FromBody]Models.EDIDataMapField dt)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLEDIDataMapField dalData = new DAL.NGLEDIDataMapField(Parameters);
                LTS.tblEDIDataMapField oChanges = selectLTSData(dt);
                //insert the new edi datamapfield
                LTS.tblEDIDataMapField oData = dalData.InsertEDIDataMapField(oChanges);
                Models.EDIDataMapField[] oRecords = new Models.EDIDataMapField[1];
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
        /// This is used to return a collection of Model record of EDIDataMapField
        /// based on TableId
        /// </summary>
        /// <param name="filter">The TableId.</param>
        /// <returns>An enumerable list of records</returns>
        [HttpGet, ActionName("GetRecordsByTableId")]
        public Models.Response GetRecordsByTableId(string filter)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                Models.EDIDataMapField[] edidatamapfields = new Models.EDIDataMapField[] { };
                DAL.NGLEDIDataMapField oAn = new DAL.NGLEDIDataMapField(Parameters);
                //getting element records from tblEDIDataMapField
                LTS.tblEDIDataMapField[] ltsRet = oAn.GetEDIDataMapFieldByTableId(filter);
                if (ltsRet != null && ltsRet.Count() > 0)
                {
                    count = ltsRet.Count();
                    edidatamapfields = (from e in ltsRet
                                        select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                response = new Models.Response(edidatamapfields, count);
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
        /// Get Distinct Map Table Records 
        /// </summary>
        /// <returns>An enumerable list of records</returns>

        [HttpGet, ActionName("GetTableRecords")]
        public Models.Response GetTableRecords()
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                int RecordCount = 0;
                DAL.NGLEDIDataMapField dalData = new DAL.NGLEDIDataMapField(Parameters);
                Models.EDIDataMapField[] EDIStructLoops = new Models.EDIDataMapField[] { };
                LTS.vw_tblEDIDataMapField[] ltsStructsegments = null;
                
                ltsStructsegments = dalData.GetTableRecords();
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
        #endregion
    }
}