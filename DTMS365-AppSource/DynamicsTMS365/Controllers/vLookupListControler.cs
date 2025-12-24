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

using StaticLists = Ngl.FreightMaster.Data.NGLLookupDataProvider.StaticLists;
using GlobalDynamicLists = Ngl.FreightMaster.Data.NGLLookupDataProvider.GlobalDynamicLists;
using UserDynamicLists = Ngl.FreightMaster.Data.NGLLookupDataProvider.UserDynamicLists;
using SortType = Ngl.FreightMaster.Data.NGLLookupDataProvider.ListSortType;
using UTC = NGL.UTC.Library;

namespace DynamicsTMS365.Controllers
{
    /// <summary>
    /// (Read Only) Generic vLookupListControler there are 3 types of lookup lists
    /// 1. Static lists for all users stored in global space via the Utilities class
    ///     Application State objects are stored in memory and are only updated with the data is null
    /// 2. User Dynamic List,  user specific lists stored seperately for each user
    ///     these lists are stored in Session State Variables on the Server and must 
    ///     be refreshed if the session state is opened.
    /// 3. Global Dynamic Lists stored in global space via the Utilities class
    ///     Applicaiton State objects stored in memory but updated based on ModDate using call to server DB
    ///     to check for changes
    /// </summary>
    /// <remarks>
    /// Created by RHR for v-8.0 on 2/21/2017
    /// </remarks>

    ///RequirementsMode

    [System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Required)]
    public class vLookupListController : NGLControllerBase
    {
        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.vLookupListControler";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " REST Services"


        //public Models.Response Get()
        //{
        //    Models.vLookupList[] oLookup = new Models.vLookupList[2];
        //    Models.vLookupList l1 = new Models.vLookupList { Control = 1, Name = "test", Description = "First" };
        //    oLookup[0] = l1;
        //    Models.vLookupList l2 = new Models.vLookupList { Control = 2, Name = "test 2", Description = "Second" };
        //    oLookup[1] = l2;
        //    return new Models.Response(oLookup, 2);
        //   // return Get(56);
        //}

        /*****************STATIC LISTS**********************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.5.3.007 on 03/09/2023
        ///   adde logic to filter Legal Entity List by user's Legal Entity Control (and Carrier -- 0) if not a super user
        /// </remarks>
        [ActionName("GetStaticList")]
        public Models.Response GetStaticList(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DTO.vLookupList[] oLookup = new DTO.vLookupList[] { };
                int count = 0;
               

                StaticLists ListType = (StaticLists)id;
                SortType SortType = SortType.Name; //Default SortType is Name

                if (ListType == StaticLists.LoadStatusCodes) { SortType = SortType.SeqNo; } //If id = LoadStatusCodes then we sort by SeqNo
                oLookup = Utilities.globalvLookup.getStaticvLookupList(ListType, SortType, null, Parameters);
                if (oLookup != null && id == (int)DAL.NGLLookupDataProvider.StaticLists.LegalEntities && Parameters.CatControl != 4)
                {
                    // this is a Legal Entity list and the user is not super so restrict access
                    List<DTO.vLookupList> list = new List<DTO.vLookupList>();
                    list.Add(oLookup.Where(f => f.Control == Parameters.UserLEControl).FirstOrDefault());
                    list.Add(oLookup.Where(f => f.Control == 0).FirstOrDefault());
                    oLookup = list.ToArray();

                }
                if (oLookup != null && oLookup.Count() > 0) { count = oLookup.Count(); }
                response = new Models.Response(oLookup, count);
            }
            catch (Exception ex)
            {
                // something went wrong - possibly a database error. return a 500 server error and send the details of the exception.
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Errors = string.Format("The database read failed: {0}", ex.Message);
            }
            return response;
        }

        /// <summary>
        /// Converts Control number to string and inserts Name from vLookup into control returns Model.vLookupText
        /// Used to support drop down lists that save the text value instead of a control number for a lookup table
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks> 
        /// Created by RHR for v-8.2 on 10/16/2018
        /// Overload for GetStaticList used when a numeric control number is not wanted
        /// </remarks>
        [ActionName("GetStaticListText")]
        public Models.Response GetStaticListText(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DTO.vLookupList[] oLookup = new DTO.vLookupList[] { };
                Models.vLookupText[] oLookupText = new Models.vLookupText[] { };
                int count = 0;
                oLookup = Utilities.globalvLookup.getStaticvLookupList((StaticLists)id, SortType.Name, null, Parameters);
                if (oLookup != null && oLookup.Count() > 0) {
                    count = oLookup.Count();
                    oLookupText = (from t in oLookup
                                   select new Models.vLookupText()
                                   {
                                       Control = t.Name,
                                       Name = t.Name,
                                       Description = t.Description
                                   }).ToArray();
                }
                response = new Models.Response(oLookupText, count);
            }
            catch (Exception ex)
            {
                // something went wrong - possibly a database error. return a 500 server error and send the details of the exception.
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Errors = string.Format("The database read failed: {0}", ex.Message);
            }
            return response;
        }

        /// <summary>
        /// Converts Control number to string and inserts Description for all fields
        /// from vLookup into control returns Model.vLookupText
        /// Used to support drop down lists that save the description value 
        /// instead of a control number for a lookup table
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks> 
        /// Created by RHR for v-8.2 on 10/16/2018
        /// Overload for GetStaticList used when a numeric control number is not wanted
        /// </remarks>
        [ActionName("GetStaticListDESC")]
        public Models.Response GetStaticListDESC(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DTO.vLookupList[] oLookup = new DTO.vLookupList[] { };
                Models.vLookupText[] oLookupText = new Models.vLookupText[] { };
                int count = 0;
                oLookup = Utilities.globalvLookup.getStaticvLookupList((StaticLists)id, SortType.Name, null, Parameters);
                if (oLookup != null && oLookup.Count() > 0)
                {
                    count = oLookup.Count();
                    oLookupText = (from t in oLookup
                                   select new Models.vLookupText()
                                   {
                                       Control = t.Description,
                                       Name = t.Description,
                                       Description = t.Description
                                   }).ToArray();
                }
                response = new Models.Response(oLookupText, count);
            }
            catch (Exception ex)
            {
                // something went wrong - possibly a database error. return a 500 server error and send the details of the exception.
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Errors = string.Format("The database read failed: {0}", ex.Message);
            }
            return response;
        }


        /// <summary>
        /// Used to support drop down lists that display the "Description" field instead of the "Name" field
        /// The value saved is still the Control field
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by LVV for v-8.2.1.004 on 12/18/19
        /// Overload for GetStaticList used when a numeric control number is wanted but the display should be "Description" field
        /// </remarks>
        [ActionName("GetStaticListCTRLDESC")]
        public Models.Response GetStaticListCTRLDESC(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DTO.vLookupList[] oLookup = new DTO.vLookupList[] { };
                DTO.vLookupList[] oLookupText = new DTO.vLookupList[] { };
                int count = 0;
                oLookup = Utilities.globalvLookup.getStaticvLookupList((StaticLists)id, SortType.Name, null, Parameters);
                if (oLookup != null && oLookup.Count() > 0)
                {
                    count = oLookup.Count();
                    oLookupText = (from t in oLookup
                                   select new DTO.vLookupList()
                                   {
                                       Control = t.Control,
                                       Name = t.Description,
                                       Description = t.Name
                                   }).ToArray();
                }
                response = new Models.Response(oLookupText, count);
            }
            catch (Exception ex)
            {
                // something went wrong - possibly a database error. return a 500 server error and send the details of the exception.
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Errors = string.Format("The database read failed: {0}", ex.Message);
            }
            return response;
        }

        /// <summary>
        /// Converts Control number to string and inserts Description for Control field in vLookup
        /// Used to support drop down lists that save the description value instead of a control number for a lookup table
        /// Still display Name field in ddl
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by LVV for v-8.2.1.004 on 12/18/19
        /// Overload for GetStaticList used when a numeric control number is not wanted
        /// </remarks>
        [ActionName("GetStaticListDESCName")]
        public Models.Response GetStaticListDESCName(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DTO.vLookupList[] oLookup = new DTO.vLookupList[] { };
                Models.vLookupText[] oLookupText = new Models.vLookupText[] { };
                int count = 0;
                oLookup = Utilities.globalvLookup.getStaticvLookupList((StaticLists)id, SortType.Name, null, Parameters);
                if (oLookup != null && oLookup.Count() > 0)
                {
                    count = oLookup.Count();
                    oLookupText = (from t in oLookup
                                   select new Models.vLookupText()
                                   {
                                       Control = t.Description,
                                       Name = t.Name,
                                       Description = t.Name
                                   }).ToArray();
                }
                response = new Models.Response(oLookupText, count);
            }
            catch (Exception ex)
            {
                // something went wrong - possibly a database error. return a 500 server error and send the details of the exception.
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Errors = string.Format("The database read failed: {0}", ex.Message);
            }
            return response;
        }


        /// <summary>
        /// Used to support drop down lists that display a template of "Name - Description" instead of the "Name" field
        /// ddl Text = Name - Description
        /// ddl Value = The value saved is still the Control field
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by LVV for v-8.2.1.006 on 4/6/20
        /// Overload for GetStaticList used when a numeric control number is wanted but the display should be "Name - Description"
        /// </remarks>
        [ActionName("GetStaticListVcTnd")]
        public Models.Response GetStaticListVcTnd(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DTO.vLookupList[] oLookup = new DTO.vLookupList[] { };
                DTO.vLookupList[] oLookupText = new DTO.vLookupList[] { };
                int count = 0;
                oLookup = Utilities.globalvLookup.getStaticvLookupList((StaticLists)id, SortType.Name, null, Parameters);
                if (oLookup != null && oLookup.Count() > 0)
                {
                    count = oLookup.Count();
                    oLookupText = (from t in oLookup
                                   select new DTO.vLookupList()
                                   {
                                       Control = t.Control,
                                       Name = t.Name + " " + t.Description,
                                       Description = t.Name
                                   }).ToArray();
                }
                response = new Models.Response(oLookupText, count);
            }
            catch (Exception ex)
            {
                // something went wrong - possibly a database error. return a 500 server error and send the details of the exception.
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Errors = string.Format("The database read failed: {0}", ex.Message);
            }
            return response;
        }

        /// <summary>
        /// We don't have a way in CM currently to add the options aka blank null record to the ddl
        /// so I wrote this as a workaround for now
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ActionName("GetStaticListWEmptyDefault")]
        public Models.Response GetStaticListWEmptyDefault(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DTO.vLookupList[] oRecords = new DTO.vLookupList[] { };
                List<DTO.vLookupList> list = new List<DTO.vLookupList>();
                list.Add(new DTO.vLookupList { Control = 0, Name = "", Description = "" });
                int count = 0;

                StaticLists ListType = (StaticLists)id;
                SortType SortType = SortType.Name; //Default SortType is Name
                if (ListType == StaticLists.LoadStatusCodes) { SortType = SortType.SeqNo; } //If id = LoadStatusCodes then we sort by SeqNo

                DTO.vLookupList[] oLookup = Utilities.globalvLookup.getStaticvLookupList(ListType, SortType, null, Parameters);
                if (oLookup?.Count() > 0) { list.AddRange(oLookup); }
                oRecords = list.ToArray();
                count = oRecords.Count();
                response = new Models.Response(oRecords, count);
            }
            catch (Exception ex)
            {
                // something went wrong - possibly a database error. return a 500 server error and send the details of the exception.
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Errors = string.Format("The database read failed: {0}", ex.Message);
            }
            return response;
        }


        /*****************GLOBAL DYNAMIC LISTS**********************/
        [ActionName("GetGlobalDynamicList")]
        public Models.Response GetGlobalDynamicList(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DTO.vLookupList[] oLookup = new DTO.vLookupList[] { };
                int count = 0;
                GlobalDynamicLists ListType = (GlobalDynamicLists)id;
                int iCriteria = 0;
                int iLEAdminControl = Parameters.UserLEControl;
                if (ListType == GlobalDynamicLists.LECarrier)
                {
                    iCriteria = iLEAdminControl;
                    oLookup = Utilities.globalvLookup.getGlobalDynamicvLookupList(ListType,SortType.Name, iCriteria, Parameters);
                } else if (ListType == GlobalDynamicLists.LECarrierOrAny)
                {
                    iCriteria = iLEAdminControl;
                    oLookup = Utilities.globalvLookup.getGlobalDynamicvLookupList(ListType, SortType.Name, iCriteria, Parameters);
                } else if (ListType == GlobalDynamicLists.TariffAvailableHDM)
                {
                    iCriteria = readPagePrimaryKey(Parameters, Utilities.PageEnum.Tariff); ;
                    oLookup = Utilities.globalvLookup.getGlobalDynamicvLookupList(ListType, SortType.Name, iCriteria, Parameters);
                }
                else
                {
                    oLookup = Utilities.globalvLookup.getGlobalDynamicvLookupList((GlobalDynamicLists)id);
                }
                   
                if (oLookup != null && oLookup.Count() > 0) { count = oLookup.Count(); }
                response = new Models.Response(oLookup, count);
            }
            catch (Exception ex)
            {
                // something went wrong - possibly a database error. return a 500 server error and send the details of the exception.
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Errors = string.Format("The database read failed: {0}", ex.Message);
            }
            return response;
        }


        /*****************GLOBAL DYNAMIC LISTS**********************/
        [ActionName("GetLiveGlobalDynamicList")]
        public Models.Response GetLiveGlobalDynamicList(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DTO.vLookupList[] oLookup = new DTO.vLookupList[] { };
                int count = 0;               
                oLookup = Utilities.globalvLookup.getGlobalDynamicvLookupList((GlobalDynamicLists)id, DAL.NGLLookupDataProvider.ListSortType.Name, Parameters);
                if (oLookup != null && oLookup.Count() > 0) { count = oLookup.Count(); }
                response = new Models.Response(oLookup, count);
            }
            catch (Exception ex)
            {
                // something went wrong - possibly a database error. return a 500 server error and send the details of the exception.
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Errors = string.Format("The database read failed: {0}", ex.Message);
            }
            return response;
        }

        [HttpGet, ActionName("GetGlobalDynamicListFiltered")]
        public Models.Response GetGlobalDynamicListFiltered(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                Models.vLookupListCriteria ofilter = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Models.vLookupListCriteria>(filter);
                if (ofilter == null )
                {
                    FaultExceptionEventArgs fault = new FaultExceptionEventArgs();
                    response.StatusCode = HttpStatusCode.BadRequest;
                    List<string> DetailsList = new List<string> { "GlobalDynamicLists", " the filter is missing"  }; // maps to E_InvalidParameterNameValue -- "Invalid Parameter: No record exists in the database for {0}: {1}."
                    response.Errors = fault.formatMessage("E_InvalidRequest", "E_InvalidParameterNameValue", DetailsList);
                    return response;
                }
                if (Enum.IsDefined(typeof(GlobalDynamicLists), ofilter.id) == false)
                {
                    FaultExceptionEventArgs fault = new FaultExceptionEventArgs();
                    response.StatusCode = HttpStatusCode.BadRequest;
                    List<string> DetailsList = new List<string> { "GlobalDynamicLists", ofilter.id.ToString() }; // maps to E_InvalidParameterNameValue -- "Invalid Parameter: No record exists in the database for { 0}: { 1}."
                    response.Errors = fault.formatMessage("E_InvalidRequest", "E_InvalidParameterNameValue", DetailsList);
                    return response;
                }
                if (Enum.IsDefined(typeof(SortType), ofilter.sortKey) == false) { ofilter.sortKey = 0; } //on sort key error just use 0 to sort by control number
                DTO.vLookupList[] oLookup = new DTO.vLookupList[] { };
                int count = 0;
                oLookup = Utilities.globalvLookup.getGlobalDynamicvLookupList((GlobalDynamicLists)ofilter.id, (SortType)ofilter.sortKey,ofilter.criteria,Parameters);
                if (oLookup != null && oLookup.Count() > 0) { count = oLookup.Count(); }
                response = new Models.Response(oLookup, count);
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

        /*****************USER DYNAMIC LISTS**********************/
        [ActionName("GetUserDynamicList")]
        public Models.Response GetUserDynamicList(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }          
            try
            {
                DTO.vLookupList[] oLookup = new DTO.vLookupList[] { };
                int count = 0;
                UserDynamicLists ListType = (UserDynamicLists)id;
                int iCriteria = 0;
                
                //DynamicsTMS365.TMSApp.clsUserLists uLists = Utilities.UserLists.getUserTMSLookup(userToken)
                //DynamicsTMS365.TMSApp.clsUserTMSLookup UserLookups = (DynamicsTMS365.TMSApp.clsUserTMSLookup)(HttpContext.Current.Session["UserLookups"]);
                DynamicsTMS365.TMSApp.clsUserTMSLookup UserLookups = Utilities.UserLists.getUserTMSLookup(this.UserControl);
                if (UserLookups == null) { UserLookups = new DynamicsTMS365.TMSApp.clsUserTMSLookup(); }
                if (ListType == UserDynamicLists.LaneTariff)
                {
                    iCriteria = readPagePrimaryKey(Parameters, Utilities.PageEnum.Tariff);
                    oLookup = UserLookups.getUserDynamicvLookupList(ListType, SortType.Name, iCriteria,null,Parameters);
                }else if (ListType == UserDynamicLists.CarrierTariffProName) {
                    iCriteria = readPagePrimaryKey(Parameters, Utilities.PageEnum.Tariff);
                    DTO.vLookupList[] tLookup = UserLookups.getUserDynamicvLookupList(ListType, SortType.Name, iCriteria, null, Parameters);
                    List<Models.vLookupText> newLookup = new List<Models.vLookupText>();
                    if (tLookup == null || tLookup.Count() < 1)
                    {
                        newLookup.Add(new Models.vLookupText { Control = "None", Name = "None - No Carrier Pro Fromula", Description = "No Carrier Pro Fromula" });
                    } else
                    {
                        foreach (DTO.vLookupList l in tLookup)
                        {
                            newLookup.Add(new Models.vLookupText { Control = l.Name, Name = l.Name + " - " + l.Description, Description = l.Description });
                        }
                        response = new Models.Response(newLookup.ToArray(), newLookup.Count);
                        return response;
                    }
                }else if(ListType == UserDynamicLists.LEAcssCodes) {
                    //Modified By LVV on 4/9/20 - bug fix null reference exception
                    DAL.Models.SSOResults ssoRes = Utilities.GlobalSSOResultsByUser[UserControl];
                    if (ssoRes != null) { iCriteria = ssoRes.UserLEControl; }
                    //iCriteria = Utilities.GlobalSSOResultsByUser[UserControl].UserLEControl;
                    oLookup = UserLookups.getUserDynamicvLookupList(ListType, SortType.Name, iCriteria, null, Parameters);
                }else {
                    oLookup = UserLookups.getUserDynamicvLookupList(ListType, SortType.Name, null, null, Parameters);
                }
                
                if (oLookup != null && oLookup.Count() > 0) { count = oLookup.Count(); }
                response = new Models.Response(oLookup, count);
            }
            catch (Exception ex)
            {
                // something went wrong - possibly a database error. return a 500 server error and send the details of the exception.
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Errors = string.Format("The database read failed: {0}", ex.Message);
            }
            return response;
        }

        [HttpGet, ActionName("GetUserDynamicListFiltered")]
        public Models.Response GetUserDynamicListFiltered(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                Models.vLookupListCriteria ofilter = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Models.vLookupListCriteria>(filter);
                if (ofilter == null)
                {
                    FaultExceptionEventArgs fault = new FaultExceptionEventArgs();
                    response.StatusCode = HttpStatusCode.BadRequest;
                    List<string> DetailsList = new List<string> { "UserDynamicLists", " the filter is missing" }; // maps to E_InvalidParameterNameValue -- "Invalid Parameter: No record exists in the database for {0}: {1}."
                    response.Errors = fault.formatMessage("E_InvalidRequest", "E_InvalidParameterNameValue", DetailsList);
                    return response;
                }
                if (Enum.IsDefined(typeof(UserDynamicLists), ofilter.id) == false)
                {
                    FaultExceptionEventArgs fault = new FaultExceptionEventArgs();
                    response.StatusCode = HttpStatusCode.BadRequest;
                    List<string> DetailsList = new List<string> { "UserDynamicLists", ofilter.id.ToString() }; // maps to E_InvalidParameterNameValue -- "Invalid Parameter: No record exists in the database for { 0}: { 1}."
                    response.Errors = fault.formatMessage("E_InvalidRequest", "E_InvalidParameterNameValue", DetailsList);
                    return response;
                }
                if (Enum.IsDefined(typeof(SortType), ofilter.sortKey) == false) { ofilter.sortKey = 0; } //on sort key error just use 0 to sort by control number
                DTO.vLookupList[] oLookup = new DTO.vLookupList[] { };
                int count = 0;

                DynamicsTMS365.TMSApp.clsUserTMSLookup UserLookups = Utilities.UserLists.getUserTMSLookup(this.UserControl);
                if (UserLookups == null) { UserLookups = new DynamicsTMS365.TMSApp.clsUserTMSLookup(); }
                oLookup = UserLookups.getUserDynamicvLookupList((UserDynamicLists)ofilter.id, (SortType)ofilter.sortKey, ofilter.criteria, null, Parameters);
                if (oLookup != null && oLookup.Count() > 0) { count = oLookup.Count(); }
                response = new Models.Response(oLookup, count);
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
        /// Used to support drop down lists that display the "Description" field instead of the "Name" field
        /// The value saved is still the Control field
        /// DDL Value = DTO.vLookupList.Control (saved)
        /// DDL Text  = DTO.vLookupList.Description (displayed)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by LVV for v-8.2.1.004 on 12/19/19
        /// Overload for GetUserDynamicList used when a numeric control number is wanted but the display should be "Description" field
        /// </remarks>
        [ActionName("GetUserDynamicListVcTd")]
        public Models.Response GetUserDynamicListVcTd(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DTO.vLookupList[] oLookup = new DTO.vLookupList[] { };
                DTO.vLookupList[] oLookupText = new DTO.vLookupList[] { };
                int count = 0;
                bool blnReturnResponse = false;
                oLookup = GetUserDynamicListMain(ref response, ref blnReturnResponse, id);
                if (blnReturnResponse) { return response; }
                if (oLookup?.Count() > 0)
                {
                    count = oLookup.Count();
                    oLookupText = (from t in oLookup
                                   select new DTO.vLookupList()
                                   {
                                       Control = t.Control,
                                       Name = t.Description,
                                       Description = t.Name
                                   }).ToArray();
                }
                response = new Models.Response(oLookupText, count);
            }
            catch (Exception ex)
            {
                // something went wrong - possibly a database error. return a 500 server error and send the details of the exception.
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Errors = string.Format("The database read failed: {0}", ex.Message);
            }
            return response;
        }

        /// <summary>
        /// Used to support drop down lists that save the description value instead of a control number for a lookup table
        /// DDL Value = DTO.vLookupList.Description (saved)
        /// DDL Text  = DTO.vLookupList.Description (displayed)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by LVV for v-8.2.1.004 on 12/19/19
        /// Overload for GetUserDynamicList used when a numeric control number is not wanted
        /// Examples: ChartOfAcounts (A/P G/L No.)
        /// </remarks>
        [ActionName("GetUserDynamicListVdTd")]
        public Models.Response GetUserDynamicListVdTd(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DTO.vLookupList[] oLookup = new DTO.vLookupList[] { };
                Models.vLookupText[] oLookupText = new Models.vLookupText[] { };
                int count = 0;
                bool blnReturnResponse = false;
                oLookup = GetUserDynamicListMain(ref response, ref blnReturnResponse, id);
                if (blnReturnResponse) { return response; }
                if (oLookup?.Count() > 0)
                {
                    count = oLookup.Count();
                    oLookupText = (from t in oLookup
                                   select new Models.vLookupText()
                                   {
                                       Control = t.Description,
                                       Name = t.Description,
                                       Description = t.Name
                                   }).ToArray();
                }
                response = new Models.Response(oLookupText, count);
            }
            catch (Exception ex)
            {
                // something went wrong - possibly a database error. return a 500 server error and send the details of the exception.
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Errors = string.Format("The database read failed: {0}", ex.Message);
            }
            return response;
        }

        /// <summary>
        /// Used to support drop down lists that save the description value instead of a control number for a lookup table
        /// DDL Value = DTO.vLookupList.Description (saved)
        /// DDL Text  = DTO.vLookupList.Name (displayed)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by LVV for v-8.2.1.004 on 12/19/19
        /// Overload for GetUserDynamicList used when a numeric control number is not wanted
        /// Examples: ChartOfAcounts (A/P G/L Desc)
        /// </remarks>
        [ActionName("GetUserDynamicListVdTn")]
        public Models.Response GetUserDynamicListVdTn(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DTO.vLookupList[] oLookup = new DTO.vLookupList[] { };
                Models.vLookupText[] oLookupText = new Models.vLookupText[] { };
                int count = 0;
                bool blnReturnResponse = false;
                oLookup = GetUserDynamicListMain(ref response, ref blnReturnResponse, id);
                if (blnReturnResponse) { return response; }
                if (oLookup?.Count() > 0)
                {
                    count = oLookup.Count();
                    oLookupText = (from t in oLookup
                                   select new Models.vLookupText()
                                   {
                                       Control = t.Description,
                                       Name = t.Name,
                                       Description = t.Control.ToString()
                                   }).ToArray();
                }
                response = new Models.Response(oLookupText, count);
            }
            catch (Exception ex)
            {
                // something went wrong - possibly a database error. return a 500 server error and send the details of the exception.
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Errors = string.Format("The database read failed: {0}", ex.Message);
            }
            return response;
        }


        /// <summary>
        /// Used to support drop down lists that save the description value instead of a control number for a lookup table
        /// DDL Value = DTO.vLookupList.Description (saved)
        /// DDL Text  = DTO.vLookupList.Name - DTO.vLookupList.Description (displayed)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by LVV for v-8.2.1.004 on 12/31/19
        /// Overload for GetUserDynamicList used when a numeric control number is not wanted
        /// Examples: ChartOfAcounts (A/P G/L No.)
        /// </remarks>
        [ActionName("GetUserDynamicListVdTdn")]
        public Models.Response GetUserDynamicListVdTdn(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DTO.vLookupList[] oLookup = new DTO.vLookupList[] { };
                Models.vLookupText[] oLookupText = new Models.vLookupText[] { };
                int count = 0;
                bool blnReturnResponse = false;
                oLookup = GetUserDynamicListMain(ref response, ref blnReturnResponse, id);
                if (blnReturnResponse) { return response; }
                if (oLookup?.Count() > 0)
                {
                    count = oLookup.Count();
                    oLookupText = (from t in oLookup
                                   select new Models.vLookupText()
                                   {
                                       Control = t.Description,
                                       Name = t.Description + " " + t.Name,
                                       Description = t.Name
                                   }).ToArray();
                }
                response = new Models.Response(oLookupText, count);
            }
            catch (Exception ex)
            {
                // something went wrong - possibly a database error. return a 500 server error and send the details of the exception.
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Errors = string.Format("The database read failed: {0}", ex.Message);
            }
            return response;
        }


        /// <summary>
        /// Used to support drop down lists that save the description value instead of a control number for a lookup table
        /// DDL Value = DTO.vLookupList.Description (saved)
        /// DDL Text  = DTO.vLookupList.Name (displayed)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.5.4.005 on 04/04/2024 New Time Zone settings
        /// id is zero 
        /// </remarks>
        [ActionName("GetTimeZoneList")]
        public Models.Response GetTimeZoneList(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                Models.vLookupText[] oLookupText = new Models.vLookupText[] { };                              
                 List<UTC.TimeZoneInfoDto> lTimeZones = UTC.clsApplication.GetTimeZoneList();

                int count = 0;
                bool blnReturnResponse = false;
                if (lTimeZones?.Count() > 0)
                {
                    count = lTimeZones.Count();
                    oLookupText = (from t in lTimeZones
                                   select new Models.vLookupText()
                                   {
                                       Control = t.value,
                                       Name = t.value,
                                       Description = t.text
                                   }).ToArray();
                }
                response = new Models.Response(oLookupText, count);
            }
            catch (Exception ex)
            {
                // something went wrong - possibly a database error. return a 500 server error and send the details of the exception.
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Errors = string.Format("The database read failed: {0}", ex.Message);
            }
            return response;
        }

        private DTO.vLookupList[] GetUserDynamicListMain(ref Models.Response response, ref bool blnReturnResponse, int id)
        {
            DTO.vLookupList[] oLookup = new DTO.vLookupList[] { };
            UserDynamicLists ListType = (UserDynamicLists)id;
            int iCriteria = 0;
            blnReturnResponse = false;

            DynamicsTMS365.TMSApp.clsUserTMSLookup UserLookups = Utilities.UserLists.getUserTMSLookup(this.UserControl);
            if (UserLookups == null) { UserLookups = new DynamicsTMS365.TMSApp.clsUserTMSLookup(); }
            if (ListType == UserDynamicLists.LaneTariff)
            {
                iCriteria = readPagePrimaryKey(Parameters, Utilities.PageEnum.Tariff);
                oLookup = UserLookups.getUserDynamicvLookupList(ListType, SortType.Name, iCriteria, null, Parameters);
            }
            else if (ListType == UserDynamicLists.CarrierTariffProName)
            {
                iCriteria = readPagePrimaryKey(Parameters, Utilities.PageEnum.Tariff);
                DTO.vLookupList[] tLookup = UserLookups.getUserDynamicvLookupList(ListType, SortType.Name, iCriteria, null, Parameters);
                List<Models.vLookupText> newLookup = new List<Models.vLookupText>();
                if (tLookup == null || tLookup.Count() < 1)
                {
                    newLookup.Add(new Models.vLookupText { Control = "None", Name = "None - No Carrier Pro Fromula", Description = "No Carrier Pro Fromula" });
                }
                else
                {
                    foreach (DTO.vLookupList l in tLookup)
                    {
                        newLookup.Add(new Models.vLookupText { Control = l.Name, Name = l.Name + " - " + l.Description, Description = l.Description });
                    }
                    response = new Models.Response(newLookup.ToArray(), newLookup.Count);
                    blnReturnResponse = true;
                    return oLookup;
                }
            }
            else if (ListType == UserDynamicLists.LEAcssCodes)
            {
                //Modified By LVV on 4/9/20 - bug fix null reference exception
                DAL.Models.SSOResults ssoRes = Utilities.GlobalSSOResultsByUser[UserControl];
                if (ssoRes != null) { iCriteria = ssoRes.UserLEControl; }
                //iCriteria = Utilities.GlobalSSOResultsByUser[UserControl].UserLEControl;
                oLookup = UserLookups.getUserDynamicvLookupList(ListType, SortType.Name, iCriteria, null, Parameters);
            }
            else if (ListType == UserDynamicLists.ChartOfAcounts)
            {
                oLookup = UserLookups.getUserDynamicvLookupList(ListType, SortType.Name, null, null, Parameters);
                var list = oLookup.ToList();
                list.Insert(0, new DTO.vLookupList { Control = 0, Name = "", Description = "" });
                oLookup = list.ToArray();
            }
            else
            {
                oLookup = UserLookups.getUserDynamicvLookupList(ListType, SortType.Name, null, null, Parameters);
            }
            return oLookup;
        }

        #endregion

    }
}