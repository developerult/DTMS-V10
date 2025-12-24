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
using BLL = NGL.FM.BLL;
using DTran = Ngl.Core.Utility.DataTransformation;

namespace DynamicsTMS365.Controllers
{
    public class UserGroupController : NGLControllerBase
    {

        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.2 on 08/29/2018 initializes the Page property by calling the base class constructor
        /// </summary>
        public UserGroupController()
                : base(Utilities.PageEnum.LEUsers)
	     {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.UserGroupController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        private Models.vUserGroup selectModelData(LTS.vUserGroup d)
        {
            Models.vUserGroup modelRecord = new Models.vUserGroup();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "UserGroupsUpdated" };
                string sMsg = "";
                modelRecord = (Models.vUserGroup)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.UserGroupsUpdated.ToArray()); }
            }
            return modelRecord;
        }

        private LTS.tblUserGroup selectLTSData(Models.vUserGroup d)
        {
            LTS.tblUserGroup ltsRecord = new LTS.tblUserGroup();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "UserGroupsUpdated" };
                string sMsg = "";
                ltsRecord = (LTS.tblUserGroup)DTran.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.UserGroupsUpdated = bupdated == null ? new byte[0] : bupdated;
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
                DAL.Models.AllFilters f = new DAL.Models.AllFilters { filterName = "UserGroupsControl", filterValue = id.ToString() };
                response = GetUserGroups(f);
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

        /// <summary>
        /// If called by a SuperUser return all UserGroups, else return only UserGroups associated with the Legal Entity aka AllFilters.ParentControl
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet, ActionName("GetAllRecords")]
        public Models.Response GetAllRecords(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                //save the page filter for the next time the page loads
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter,"UserGroupFilter"); }
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);

                //if the user did not do any sorting then apply the default sort
                if (f.SortValues?.Length < 1 || string.IsNullOrWhiteSpace(f.sortDirection))
                {
                    var sv1 = new DAL.Models.SortDetails { sortName = "LEAdminLegalEntity", sortDirection = "Asc" };
                    f.SortValues = new DAL.Models.SortDetails[] { sv1 };
                }
                response = GetUserGroups(f);
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
        public Models.Response Post([System.Web.Http.FromBody]Models.vUserGroup data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                LTS.tblUserGroup oChanges = new LTS.tblUserGroup();
                oChanges = selectLTSData(data);
                bool blnRet = NGLUserGroupData.InsertOrUpdateUserGroup(oChanges);
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

        [HttpDelete, ActionName("DELETE")]
        public Models.Response DELETE(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                bool blnRet = NGLUserGroupData.DeleteUserGroup(id);
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


        [HttpGet, ActionName("GetUserGroupsForLE")]
        public Models.Response GetUserGroupsForLE(int id)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            try
            {
                DAL.NGLSecurityDataProvider oSec = new DAL.NGLSecurityDataProvider(Parameters);
                Models.vLookupList[] results = new Models.vLookupList[] { };
                int count = 0;

                var groups = oSec.GetUserGroupsForLE(id);

                if (groups?.Length > 0)
                {
                    results = (from e in groups
                               orderby e.UserGroupsName ascending
                               select new Models.vLookupList { Control = e.UserGroupsControl, Name = e.UserGroupsName, Description = e.UserGroupsName }
                               ).ToArray();
                    count = results.Length;
                }

                response = new Models.Response(results, count);
            }
            catch (Exception ex)
            {
                //Error handler
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }

            // return the HTTP Response.
            return response;
        }


        /// <summary>
        /// GenericResult.Control = UserSecurityControl
        /// GenericResult.intField1 = GroupControl
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        [HttpPost, ActionName("ReplaceUserSecurityWithGroup")]
        public Models.Response ReplaceUserSecurityWithGroup([System.Web.Http.FromBody]Models.GenericResult g)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                //Make sure the group still exists
                DTO.tblUserGroup u = NGLSecurityData.GettblUserGroup(g.intField1);
                if(u == null || u.UserGroupsControl == 0)
                {
                    //NOTE: I don't think this will ever get hit because if the control is not valid the method thows an exception which is then caught below
                    //we can probably update this entire thing to use new methods but for now this is fine
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Errors = "This Role does not exist. Please refresh the data and try again.";
                    return response;
                }

                NGLSecurityData.ReplaceUserSecurityWithGroup(g.intField1, g.Control);

                //Always return true. If it fails it will throw an exception
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


        /// <summary>
        /// Updates the role center security group for carrier user accounts
        /// (carrier or carrier accountant). Also updates tblUserSecurityCarrier.USCCarrierAccounting
        /// for the first carrier associated with the usc
        /// (Note: Only works for carrier users because now standard users can be associated with multiple carriers, but carrier users can only have one associated carrier)
        /// GenericResult.Control = UserSecurityControl
        /// GenericResult.strField = USCCarrierAccounting
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        [HttpPost, ActionName("UpdateCarrierUserUserSecurityGroup")]
        public Models.Response UpdateCarrierUserUserSecurityGroup([System.Web.Http.FromBody]Models.GenericResult g)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int UGControl = 0;
                int usc = g.Control;
                string carrierAccounting = g.strField;

                if (g.strField.ToUpper() == "N") { UGControl = 7; } else { UGControl = 8; } //7 is the default group Carriers that is not associated with a LE (8 = Carrier Accountants)

                NGLSecurityData.ReplaceUserSecurityWithGroup(UGControl, usc);

                //gets the first record in tblUserSecurityCarrier associated with the USC (Note: Only works for carrier users because now standard users can be associated with multiple carriers, but carrier users can only have one associated carrier)
                LTS.tblUserSecurityCarrier ltsCar = NGLUserSecurityCarrierData.GetFirstRecordByUser(usc);
                ltsCar.USCCarrierAccounting = carrierAccounting;
                NGLUserSecurityCarrierData.InsertOrUpdateUserSecurityCarrier(ltsCar);

                //Always return true. If it fails it will throw an exception
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


        #region "Private Methods"

        /// <summary>
        /// If called by a SuperUser return all UserGroups, else return only UserGroups associated with the Legal Entity aka AllFilters.ParentControl
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        private Models.Response GetUserGroups(DAL.Models.AllFilters f)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            DAL.NGLUserGroupData oUSG = new DAL.NGLUserGroupData(Parameters);
            Models.vUserGroup[] records = new Models.vUserGroup[] { };
            int RecordCount = 0;
            int count = 0;
            LTS.vUserGroup[] oData = oUSG.GetUserGroups(ref RecordCount, f);
            if (oData?.Count() > 0)
            {
                count = oData.Count();
                records = (from e in oData select selectModelData(e)).ToArray();
                if (RecordCount > count) { count = RecordCount; }
            }
            response = new Models.Response(records, count);
            return response;
        }

        #endregion

    }
}