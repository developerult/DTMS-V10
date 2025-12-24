using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using DAL = Ngl.FreightMaster.Data;
using LTS = Ngl.FreightMaster.Data.LTS;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using DTran = Ngl.Core.Utility.DataTransformation;

namespace DynamicsTMS365.Controllers
{
    public class APMassEntryMsgController : NGLControllerBase
    {

        #region " Properties"

        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.APMassEntryMsgController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        private Models.APMassEntryMsg selectModelData(LTS.vAPMassEntryMsg d)
        {
            Models.APMassEntryMsg modelRecord = new Models.APMassEntryMsg();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "_APMassEntry", "APMassEntry", "_APMassEntryMsg", "APMassEntryMsg", "_APMassEntryFees", "APMassEntryFees", "_APMassEntryHistory", "APMassEntryHistory", "_APMassEntryHistoryFees", "APMassEntryHistoryFees" };
                string sMsg = "";
                modelRecord = (Models.APMassEntryMsg)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                //if (modelRecord != null) { modelRecord.setUpdated(d.APMFeesUpdated.ToArray()); }
            }
            return modelRecord;
        }

        public static LTS.APMassEntryMsg selectLTSData(Models.APMassEntryMsg d)
        {
            LTS.APMassEntryMsg ltsRecord = new LTS.APMassEntryMsg();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "_APMassEntry", "APMassEntry", "_APMassEntryMsg", "APMassEntryMsg", "_APMassEntryFees", "APMassEntryFees", "_APMassEntryHistory", "APMassEntryHistory", "_APMassEntryHistoryFees", "APMassEntryHistoryFees" };
                string sMsg = "";
                ltsRecord = (LTS.APMassEntryMsg)DTran.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                //if (ltsRecord != null)
                //{
                //    byte[] bupdated = d.getUpdated();
                //    ltsRecord.LECompAccessorialUpdated = bupdated == null ? new byte[0] : bupdated;
                //}
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
                int count = 0;
                Models.APMassEntryMsg[] records = new Models.APMassEntryMsg[] { };
                LTS.vAPMassEntryMsg data = NGLAPMassEntryMsg.GetAPMassEntryMsg(id);
                if (data != null)
                {
                    Models.APMassEntryMsg r = selectModelData(data);
                    records = new Models.APMassEntryMsg[] { r };
                    count = 1;
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

        /// <summary>
        /// Gets All the Child Records filtered by APControl passed in as id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, ActionName("GetByParent")]
        public Models.Response GetByParent(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                Models.APMassEntryMsg[] records = new Models.APMassEntryMsg[] { };
                int RecordCount = 0;
                int count = 0;
                DAL.Models.AllFilters f = new DAL.Models.AllFilters { ParentControl = id };
                LTS.vAPMassEntryMsg[] oData = NGLAPMassEntryMsg.GetAPMassEntryMsgs(ref RecordCount, f);
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

        //Not currently used
        [HttpGet, ActionName("GetRecords")]
        public Models.Response GetRecords()
        {
            return GetRecords("");
        }

        //Not currently used
        [HttpGet, ActionName("GetRecords")]
        public Models.Response GetRecords(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateFilter(ref response, filter)) { return response; } //Verfiy that the filters object is not null
            return GetAllRecords(filter);
        }

        //Not currently used
        [HttpGet, ActionName("GetAllRecords")]
        public Models.Response GetAllRecords(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                //////save the page filter for the next time the page loads
                ////if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter); }
                ////DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);

                //////if the user did not do any sorting then apply the default sort
                ////if (f.SortValues?.Length < 1 || string.IsNullOrWhiteSpace(f.sortDirection))
                ////{
                ////    var sv1 = new DAL.Models.SortDetails { sortName = "LEAdminLegalEntity", sortDirection = "Asc" };
                ////    f.SortValues = new DAL.Models.SortDetails[] { sv1 };
                ////}
                ////response = GetUserGroups(f);
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
        public Models.Response Post([System.Web.Http.FromBody]Models.APMassEntryMsg data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                //LTS.APMassEntryMsg oChanges = new LTS.APMassEntryMsg();
                //oChanges = selectLTSData(data);
                //bool blnRet = NGLAPMassEntryMsg.InsertOrUpdateAPMassEntryMsg(oChanges);
                bool blnRet = NGLAPMassEntryMsg.UpdateResolvedFlag(data.APMMsgControl, data.APMMsgResolved);
                bool[] oRecords = new bool[1];
                oRecords[0] = blnRet;
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

        //Not currently used
        [HttpDelete, ActionName("DELETE")]
        public Models.Response DELETE(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                //NOTE: Should not get called. Instead use method RemoveFormRestrictionFromGroup
                ////bool blnRet = NGLUserGroupData.DeleteUserGroup(id);
                ////bool[] oRecords = new bool[1] { blnRet };
                ////response = new Models.Response(oRecords, 1);
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