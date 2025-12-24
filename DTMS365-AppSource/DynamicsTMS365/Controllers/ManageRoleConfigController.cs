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
using DModel = Ngl.FreightMaster.Data.Models;

namespace DynamicsTMS365.Controllers
{
    public class ManageRoleConfigController : NGLControllerBase
    {
        #region " Constructors "
        
        /// <summary> Initializes the Page property by calling the base class constructor </summary>
        public ManageRoleConfigController() : base(Utilities.PageEnum.ManageSecurity) { }

        #endregion

        #region " Properties"

        /// <summary> This property is used for logging and error tracking. </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.ManageRoleConfigController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"




        #endregion

        #region " REST Services"

        /// POST 	/API/objectcontroller{data}  : Create a new object or Update a the current object if the control number exists
        /// GET 	/API/objectcontroller/{id} : Get the object information where the control number = "id"
        /// PUT 	/API/objectcontroller/{data} : Update the object information stored in data
        /// DELETE	/API/objectcontroller/{id} : Delete object where the control number = "id"

        //Not Currently Used
        [HttpGet, ActionName("Get")]
        public Models.Response Get(int id)
        {
            //Note: CRUD not currently supported in this controller
            var response = new Models.Response(); //new HttpResponseMessage();
            response.populateDefaultInvalidFilterResponseMessage();
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
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter, "GroupSecGridFltr"); }
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter); } //save the page filter for the next time the page loads
                DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DModel.AllFilters>(filter);
                int RecordCount = 0;
                int count = 0;
                applyDefaultSort(ref f, "LEAdminLegalEntity", true); //if the user did not do any sorting then apply the default sort
                LTS.vUserGroupsSec365[] oData = new LTS.vUserGroupsSec365[] { };
                oData = NGLUserGroupData.GetUserGroupsSec365(ref RecordCount, f);
                if (oData?.Count() > 0)
                {
                    count = oData.Count();
                    if (RecordCount > count) { count = RecordCount; }
                }
                response = new Models.Response(oData, count);
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

        //Not Currently Used
        [HttpPost, ActionName("Post")]
        public Models.Response Post([FromBody]Models.vBookLoadBoard data)
        {
            //Note: CRUD not currently supported in this controller
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                //DAL.NGLBookLoadBoard oDAL = new DAL.NGLBookLoadBoard(Parameters);
                //data.BookDestEmergencyContactPhone = Utilities.removeNonNumericText(data.BookDestEmergencyContactPhone);
                //data.BookDestPhone = Utilities.removeNonNumericText(data.BookDestPhone);
                //data.BookCarrierContactPhone = Utilities.removeNonNumericText(data.BookCarrierContactPhone);
                //data.BookOrigEmergencyContactPhone = Utilities.removeNonNumericText(data.BookOrigEmergencyContactPhone);
                //data.BookOrigPhone = Utilities.removeNonNumericText(data.BookOrigPhone);
                //bool blnRet = oDAL.SaveBookLoadBoard(data);
                //bool[] oRecords = new bool[1];
                //oRecords[0] = blnRet;
                //response = new Models.Response(oRecords, 1);
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

        //Not Currently Used
        [HttpDelete, ActionName("DELETE")]
        public Models.Response DELETE(int id)
        {
            //Note: CRUD not currently supported in this controller
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                //DAL.NGLBookLoadBoard oDAL = new DAL.NGLBookLoadBoard(Parameters);
                //bool blnRet = oDAL.DeleteBookLoadBoard(id);
                //bool[] oRecords = new bool[1];
                //oRecords[0] = blnRet;
                //response = new Models.Response(oRecords, 1);
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

        [HttpGet, ActionName("GetConfigItems")]
        public Models.Response GetConfigItems(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int RecordCount = 0;
                int count = 0;
                //DModel.AllFilters f = new DModel.AllFilters();
                //f.filterName = "BookControl";
                //f.filterValue = id.ToString();
                //DAL.NGLBookLoadBoard oDAL = new DAL.NGLBookLoadBoard(Parameters);
                //Models.vLookupList[] oData = new Models.vLookupList[] { };
                DTO.vLookupList[] oData = new DTO.vLookupList[] { };
                oData = NGLSecurityData.GetConfigItems(id);
                if (oData?.Count() > 0)
                {
                    count = oData.Count();
                    if (RecordCount > count) { count = RecordCount; }
                }
                response = new Models.Response(oData.ToArray(), count);
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


        [HttpGet, ActionName("GetConfigSecTypes")]
        public Models.Response GetConfigSecTypes()
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int RecordCount = 0;
                int count = 0;
                DTO.vLookupList[] oData = new DTO.vLookupList[] { };
                oData = NGLUserGroupData.GetEnumGroupSecurityTypes();
                if (oData?.Count() > 0)
                {
                    count = oData.Count();
                    if (RecordCount > count) { count = RecordCount; }
                }
                response = new Models.Response(oData.ToArray(), count);
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


        [HttpGet, ActionName("GetConfigSecTypesForGroup")]
        public Models.Response GetConfigSecTypesForGroup(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int RecordCount = 0;
                int count = 0;
                DModel.SelectableGridItem[] oData = new DModel.SelectableGridItem[] { };
                oData = NGLUserGroupData.GetSecTypeConfigForGroup(id);
                if (oData?.Count() > 0)
                {
                    count = oData.Count();
                    if (RecordCount > count) { count = RecordCount; }
                }
                response = new Models.Response(oData, count);
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


        [HttpPost, ActionName("SaveSecTypesForGroup")]
        public Models.Response SaveSecTypesForGroup([FromBody]DModel.SelectableGridSave data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                bool blnRet = NGLUserGroupData.SaveSecTypesForGroup(data);
                Array d = new bool[1] { blnRet };
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


        [HttpPost, ActionName("SaveWizardConfiguration")]
        public Models.Response SaveWizardConfiguration([FromBody]DModel.MultiSelectBatchObjects data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                switch ((DAL.NGLSecurityDataProvider.ConfigAction)data.Action)
                {
                    case DAL.NGLSecurityDataProvider.ConfigAction.AllowAccess:
                        NGLSecurityData.ManageAccessForGroup(data, true);
                        break;
                    case DAL.NGLSecurityDataProvider.ConfigAction.RestrictAccess:
                        NGLSecurityData.ManageAccessForGroup(data, false);
                        break;
                    case DAL.NGLSecurityDataProvider.ConfigAction.Lock:
                        NGLSecurityData.ManageLocksForGroup(data, true);
                        break;
                    case DAL.NGLSecurityDataProvider.ConfigAction.Unlock:
                        NGLSecurityData.ManageLocksForGroup(data, false);
                        break;
                };            
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


        [HttpGet, ActionName("UpdateGroupBasedOnSecurityType")]
        public Models.Response UpdateGroupBasedOnSecurityType(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                NGLSecurityData.UpdateGroupBasedOnSecurityType(id);
                NGLSecurityData.UpdateAllUserPermissionsForGroup(id); //Modified by LVV 10/19 - Added call to update users
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

        [HttpGet, ActionName("UpdateAllGroupsBasedOnSecurityType")]
        public Models.Response UpdateAllGroupsBasedOnSecurityType()
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                NGLSecurityData.UpdateAllGroupsBasedOnSecurityType();
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