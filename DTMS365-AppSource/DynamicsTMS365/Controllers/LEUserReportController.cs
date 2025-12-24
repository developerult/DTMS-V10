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
    public class LEUserReportController : NGLControllerBase
    {
        #region " Constructors "

        /// <summary>
        /// Created by LVV on 5/17/19 - NOTE: This class can only be used by the LEUsers.aspx page
        /// </summary>
        public LEUserReportController()
                : base(Utilities.PageEnum.LEUsers)
        {
        }

        #endregion

        #region " Properties"

        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.LEUserReportController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        private Models.Report selectModelData(LTS.vRestrictedReportsByGroup d)
        {
            Models.Report modelRecord = new Models.Report();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "" };
                string sMsg = "";
                modelRecord = (Models.Report)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                //if (modelRecord != null) { modelRecord.setUpdated(d.UserGroupsUpdated.ToArray()); }
            }
            return modelRecord;
        }

        private Models.Form selectModelData(DTO.tblFormList d)
        {
            Models.Form modelRecord = new Models.Form();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "" };
                string sMsg = "";
                modelRecord = (Models.Form)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                //if (modelRecord != null) { modelRecord.setUpdated(d.UserGroupsUpdated.ToArray()); }
            }
            return modelRecord;
        }

        #endregion

        #region " REST Services"

        /// POST 	/API/objectcontroller{data}  : Create a new object or Update a the current object if the control number exists
        /// GET 	/API/objectcontroller/{id} : Get the object information where the control number = "id"
        /// PUT 	/API/objectcontroller/{data} : Update the object information stored in data
        /// DELETE	/API/objectcontroller/{id} : Delete object where the control number = "id"

        //Not currently used
        [HttpGet, ActionName("Get")]
        public Models.Response Get(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                ////DAL.Models.AllFilters f = new DAL.Models.AllFilters { filterName = "UserGroupsControl", filterValue = id.ToString() };
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

        /// <summary>
        /// Gets All the Child Records filtered by UserGroupsControl passed in as id
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
                //save the GroupControl so it can be used by the method RemoveReportRestrictionFromGroup()
                string msg = "";
                savePageSetting(id.ToString(), ref msg, "ReportParentRoleControl");

                Models.Report[] records = new Models.Report[] { };
                int RecordCount = 0;
                int count = 0;
                DAL.Models.AllFilters f = new DAL.Models.AllFilters { ParentControl = id };
                LTS.vRestrictedReportsByGroup[] oData = NGLUserGroupData.GetRestrictedReportsByGroup(ref RecordCount, f);
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

        [HttpGet, ActionName("GetUnRestrictedReportsByGroup")]
        public Models.Response GetUnRestrictedReportsByGroup()
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                var sSetting = readPageSettings("ReportParentRoleControl", Parameters, Utilities.PageEnum.LEUsers);
                int groupControl = 0;
                int.TryParse(sSetting[0].UserPSMetaData, out groupControl);

                DTO.tblReportList[] res = NGLSecurityData.GetUnRestrictedReportsByGroup(groupControl);
                Models.vLookupList[] newLookup = new Models.vLookupList[] { new Models.vLookupList { Control = 0, Name = "None", Description = "No Reports Available" } };
                if (res?.Count() > 0)
                {
                    if (res.Count() > 1 || res[0].ReportControl != 0) //if an empty object was returned we want to ignore it and return our above default vlookup object
                    {
                        newLookup = (from t in res
                                     orderby t.ReportName
                                     select new Models.vLookupList { Control = t.ReportControl, Name = t.ReportName, Description = t.ReportDescription }
                                     ).ToArray();
                }
            }
                response = new Models.Response(newLookup, newLookup.Count());
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
        /// Creates the report restriction and updates all the dependent users
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Post")]
        public Models.Response Post([System.Web.Http.FromBody]Models.Report data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                if(data.ReportControl > 0)
                {
                    int[] reportControls = new int[1] { data.ReportControl };
                    NGLUserGroupData.CreateRestrictedReportByGroup(reportControls, data.UserGroupsControl);
                }              
                bool[] oRecords = new bool[1] { true };
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
        //NOTE: Should not get called. Instead use method RemoveFormRestrictionFromGroup
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


        /// <summary>
        /// GenericResult.Control = ReportControl
        /// GenericResult.intField1 = UserGroupsControl
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost, ActionName("RemoveReportRestrictionFromGroup")]
        public Models.Response RemoveReportRestrictionFromGroup([System.Web.Http.FromBody]Models.GenericResult data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int reportControl = data.Control;
                int groupControl = data.intField1;
                NGLSecurityData.RemoveRestrictedReportByGroup(reportControl, groupControl);
                Array d = new bool[1] { true };
                response = new Models.Response(d, 1);
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