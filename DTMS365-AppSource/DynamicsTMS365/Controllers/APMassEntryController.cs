using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using DAL = Ngl.FreightMaster.Data;
using LTS = Ngl.FreightMaster.Data.LTS;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using BLL = NGL.FM.BLL;
using DTran = Ngl.Core.Utility.DataTransformation;
using DModel = Ngl.FreightMaster.Data.Models;

namespace DynamicsTMS365.Controllers
{
    public class APMassEntryController : NGLControllerBase
    {
        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.2 on 08/29/2018 initializes the Page property by calling the base class constructor
        /// </summary>
        public APMassEntryController()
                : base(Utilities.PageEnum.APMassEntryMaint)
        {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.APMassEntryController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        private Models.APMassEntry selectModelData(LTS.APMassEntry d)
        {
            Models.APMassEntry modelRecord = new Models.APMassEntry();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "APUpdated", "_APMassEntryMsg", "APMassEntryMsg", "_APMassEntryFees", "APMassEntryFees", "_APMassEntryHistory", "APMassEntryHistory", "_APMassEntryHistoryFees", "APMassEntryHistoryFees" };
                string sMsg = "";
                modelRecord = (Models.APMassEntry)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.APUpdated.ToArray()); }
            }
            return modelRecord;
        }

        private LTS.APMassEntry selectLTSData(Models.APMassEntry d)
        {
            LTS.APMassEntry ltsRecord = new LTS.APMassEntry();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "APUpdated", "_APMassEntryMsg", "APMassEntryMsg", "_APMassEntryFees", "APMassEntryFees", "_APMassEntryHistory", "APMassEntryHistory", "_APMassEntryHistoryFees", "APMassEntryHistoryFees" };
                string sMsg = "";
                ltsRecord = (LTS.APMassEntry)DTran.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.APUpdated = bupdated == null ? new byte[0] : bupdated;
                }
            }
            return ltsRecord;
        }

        #endregion

        #region " REST Services"

        /// POST 	/API/objectcontroller{data}  : Create a new object or Update a the current object if the control number exists
        /// GET 	/API/objectcontroller/{id} : Get the object information where the control number = "id"
        /// PUT 	/API/objectcontroller/{data} : Update the object information stored in data
        /// DELETE	/API/objectcontroller/{id} : Delete object where the control number = "id"

        [HttpGet, ActionName("Get")]
        public Models.Response Get(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {              
                Models.APMassEntry[] records = new Models.APMassEntry[] { };
                int count = 0;
                LTS.APMassEntry lts = NGLAPMassEntryData.GetAPMassEntry(id);
                if (lts != null)
                {
                    count = 1;
                    Models.APMassEntry ap = selectModelData(lts);
                    records = new Models.APMassEntry[1] { ap };
                }
                response = new Models.Response(records, count);
            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }
            return response;
        }

        //Returns a vAPMESummaryField object not an APMassEntry object
        [HttpGet, ActionName("GetByParent")]
        public Models.Response GetByParent(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                LTS.vAPMESummaryField[] records = new LTS.vAPMESummaryField[] { };
                int count = 0;
                LTS.vAPMESummaryField lts = NGLAPMassEntryData.GetvAPMESummaryFields(id);
                if (lts != null)
                {
                    count = 1;
                    records = new LTS.vAPMESummaryField[1] { lts };
                }
                response = new Models.Response(records, count);
            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }
            return response;
        }

        [HttpGet, ActionName("GetRecords")]
        public Models.Response GetRecords()
        {
            return GetRecords("");
        }

        [HttpGet, ActionName("GetRecords")]
        public Models.Response GetRecords(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateFilter(ref response, filter)) { return response; } //Verfiy that the filters object is not null
            return GetAllRecords(filter);
        }

        [HttpGet, ActionName("GetAllRecords")]
        public Models.Response GetAllRecords(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter, "APAuditAprvlFltr"); } //save the page filter for the next time the page loads
                DModel.AllFilters f = new JavaScriptSerializer().Deserialize<DModel.AllFilters>(filter);
                Models.AuditFilters d = null;
                if (f != null && !string.IsNullOrWhiteSpace(f.Data)) { d = new JavaScriptSerializer().Deserialize<Models.AuditFilters>(f.Data); }
                if (d == null) { response.populateDefaultInvalidFilterResponseMessage(); return response; }                                          
                Models.APMassEntry[] records = new Models.APMassEntry[] { };
                int RecordCount = 0;
                int count = 0;
                int intAPCarrierNumber = 0;
                int.TryParse(d.CarrierDDLValue, out intAPCarrierNumber);
                DateTime APReceivedDateFrom = (d.APReceivedDateFrom.HasValue ? d.APReceivedDateFrom.Value : DateTime.Now); //DateTime.Now.AddYears(-1);
                DateTime APReceivedDateTo = (d.APReceivedDateTo.HasValue ? d.APReceivedDateTo.Value : DateTime.Now); //DateTime.Now;
                bool ShowMatched = false;
                bool APApprovedFlag = false;
                bool APElectronicFlag = false;
                bool ShowAllErrors = false;
                bool ShowPA = false;
                switch (d.APAuditFltrsDDLValue)
                {
                    case 0: //Normal
                        break;
                    case 1: //Matched
                        ShowMatched = true;
                        break;
                    case 2: //Approved
                        APApprovedFlag = true;
                        break;
                    case 3: //Electronic
                        APElectronicFlag = true;
                        break;
                    case 4: //AllErrors
                        ShowAllErrors = true;
                        break;
                    case 5: //PA
                        ShowPA = true;
                        break;
                    default:
                        break;
                }
                LTS.APMassEntry[] oData = NGLAPMassEntryData.GetAPMassEntrysFiltered365(ref RecordCount, f, intAPCarrierNumber, APReceivedDateFrom, APReceivedDateTo, APApprovedFlag, APElectronicFlag, ShowMatched, ShowAllErrors, ShowPA);
                if (oData?.Count() > 0)
                {
                    count = oData.Count();
                    records = (from e in oData select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                response = new Models.Response(records, count);
            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }
            return response;
        }

        [HttpPost, ActionName("Post")]
        public Models.Response Post([System.Web.Http.FromBody]Models.APMassEntry data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                LTS.APMassEntry ltsAP = new LTS.APMassEntry();
                ltsAP = selectLTSData(data);
                bool blnRet = NGLAPMassEntryData.InsertOrUpdateAPMassEntry(ltsAP);
                bool[] records = new bool[1] { blnRet };
                response = new Models.Response(records, 1);
            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }
            return response;
        }

        [HttpDelete, ActionName("DELETE")]
        public Models.Response DELETE(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                bool blnRet = NGLAPMassEntryData.DeleteAPMassEntry365(id);
                bool[] oRecords = new bool[1] { blnRet };
                response = new Models.Response(oRecords, 1);
            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }
            return response;
        }

        [HttpPost, ActionName("RunAudit")]
        public Models.Response RunAudit([System.Web.Http.FromBody]Models.APMassEntry[] data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                BLL.NGLTMS365BLL bll = new BLL.NGLTMS365BLL(Parameters);
                Dictionary<int, string> dictAP = new Dictionary<int, string>();
                foreach (Models.APMassEntry d in data)
                {
                    dictAP.Add(d.APControl, d.APBillNumber);
                }
                DModel.ResultObject oResults = bll.RunAudit365(dictAP);
                DModel.ResultObject[] oRecords = new DModel.ResultObject[1] { oResults };
                response = new Models.Response(oRecords, 1);
            }
            catch (Exception ex)
            {
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