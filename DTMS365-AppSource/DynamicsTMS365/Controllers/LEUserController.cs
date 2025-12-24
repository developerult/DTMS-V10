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

namespace DynamicsTMS365.Controllers
{
    public class LEUserController : NGLControllerBase
    {

        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.2 on 08/29/2018 initializes the Page property by calling the base class constructor
        /// </summary>
        public LEUserController()
                : base(Utilities.PageEnum.LEUsers)
	     {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.LEUserController";
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
            return GetLEUsers365(filter);
        }

        [HttpGet, ActionName("GetLEUsers365")]
        public Models.Response GetLEUsers365(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);

                LTS.vLEUsers365[] uLEs = new LTS.vLEUsers365[] { };
                DAL.NGLUserSecurityLegalEntityData oUSLE = new DAL.NGLUserSecurityLegalEntityData(Parameters);
                int RecordCount = 0;

                uLEs = oUSLE.GetLEUsers365(ref RecordCount, f);
                if (uLEs?.Length < 1)
                {
                    uLEs = new LTS.vLEUsers365[] { };
                    RecordCount = 0;
                }

                response = new Models.Response(uLEs, RecordCount);
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
        /// GenericResult.Control = UserSecurityControl
        /// GenericResult.intField1 = LEAdminControl
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        [HttpPost, ActionName("SetUserLE")]
        public Models.Response SetUserLE([System.Web.Http.FromBody]Models.GenericResult g)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLUserSecurityLegalEntityData oUSLE = new DAL.NGLUserSecurityLegalEntityData(Parameters);
                DAL.NGLSecurityDataProvider oSec = new DAL.NGLSecurityDataProvider(Parameters);
                int usc = g.Control;
                int LEControl = g.intField1;
                var spRes = oUSLE.InsertOrUpdateUserSecurityLE(usc, LEControl);
                if (spRes?.ErrNumber == 0)
                {
                    //Move the user to the LE Admin Group for the new LE
                    int newGroupControl = 0;
                    newGroupControl = oSec.GetEquivalentUserGroupForNewLE(usc, LEControl);
                    if (newGroupControl != 0) 
                    {
                        if (newGroupControl != 9){ oSec.ReplaceUserSecurityWithGroup(newGroupControl, usc); } //If the user is sup they don't switch                 
                        Array d = new bool[1] { true };
                        response = new Models.Response(d, 1);
                    }
                    else
                    {
                        response.StatusCode = HttpStatusCode.InternalServerError;
                        response.Errors = "Warning: Could not find the equivalent group for this Legal Entity. The user could not be moved from the old Legal Entity User Group and it will have to be done manually in the Role Center.";
                    }
                }
                else
                {
                    string msg = "There was a problem with the procedure that sets the users Legal Entity";
                    if(spRes != null) { msg = spRes.RetMsg; }
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Errors = msg;
                }
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


        [HttpDelete, ActionName("DeleteUser")]
        public Models.Response DeleteUser(int id)
        {           
            var response = new Models.Response(); // create a new HttpResponseMessage() to send back
            if (!authenticateController(ref response)) { return response; }
            try
            {
                NGLSecurityData.DeleteUserSecurity365(id);
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


        //Make sure AccountGroup is not null if AllowNGLAPI is true. If it is return an error message and do not continue.
        //Insert the UserSecurity record. If this fails do not continue.
        //Create the permissions for the User Group
        //If NGL Legacy Account then we need to assign a random password. If this fails write to the response.Errors message and continue
        //Insert the UserSecurityLE record. If this fails write to the response.Errors message and continue
        //If AllowNGLAPI is true then we have to create those credentials. The only way this will fail is if it returns an exception which will be caught below.
        [HttpPost, ActionName("CreateUser")]
        public Models.Response CreateUser([System.Web.Http.FromBody]Models.User u)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int UGControl = 0;
                bool bNextrackOnly = false;
                if (u.blnIsCarrierUser == true){
                    bNextrackOnly = true;
                    if (u.AssociatedCarriers[0].USCCarrierAccounting.ToUpper() == "N") { UGControl = 7; } else { UGControl = 8; } //7 is the default group Carriers that is not associated with a LE (8 = Carrier Accountants)
                }
                else
                {
                    if (u.AllowNGLAPI && string.IsNullOrWhiteSpace(u.AccountGroup))
                    {
                        response.StatusCode = HttpStatusCode.InternalServerError;
                        response.Errors = "Account Group cannot be null if Allow NGL API is checked.";
                        return response;
                    }
                    UGControl = u.UserUserGroupsControl;                    
                }

                /******************************************************
                * Insert the UserSecurity record
                ******************************************************/
                LTS.tblUserSecurity ltsUS = new LTS.tblUserSecurity
                {
                    UserName = u.UseMicrosoftAccount ? u.UserEmail : u.UserName, //if UseMicrosoftAccount is true then set UserName = UserEmail else UserName = UserName
                    UserEmail = u.UserEmail,
                    UserFirstName = u.UserFirstName,
                    UserLastName = u.UserLastName,
                    UserFriendlyName = string.IsNullOrWhiteSpace(u.UserFriendlyName) ? u.UserFirstName : u.UserFriendlyName,
                    UserPhoneWork = u.UserPhoneWork,
                    UserWorkExt = u.UserPhoneWorkExt,
                    UserUserGroupsControl = UGControl,
                    UserSSOAControl = u.UseMicrosoftAccount ? 5 : 1, //if UseMicrosoftAccount is true then set SSOAControl = 5 (Microsoft) else SSOAControl = 1 (NGL Legacy)
                    NEXTrackOnly = bNextrackOnly,
                    UserCultureInfo = u.UserCultureInfo,
                    UserTimeZone = u.UserTimeZone,

                };

                var usc = NGLSecurityData.CreatetblUserSecurity365(ltsUS); //If this returns nothing or 0 or throws an exception then it failed (it should never really return 0 or nothing -- only fail if there is an exception which will get caught below)
                if (usc == 0)
                {
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Errors = "There was a problem inserting the new user record.";
                    return response;
                }

                /****************************************************************
                * Create the permissions for the User Group
                ****************************************************************/
                NGLSecurityData.ReplaceUserSecurityWithGroup(UGControl, usc);

                /****************************************************************
                * If NGL Legacy Account then we need to handle the password
                ****************************************************************/
                bool blnHasErrors = false;
                if (!u.UseMicrosoftAccount)
                {
                    if (!u.AutoGeneratePwd && !string.IsNullOrWhiteSpace(u.Pwd))
                    {
                        //If AutoGeneratePwd is false and field Pwd is not null then we need to save the provided password
                        if (!SSOAController.savePasswordOptionalSendEmail(ref response, u.SendUserPwd, usc, u.UserEmail, u.UserName, u.Pwd, Parameters)) { blnHasErrors = true; }
                    }
                    else
                    {
                        //we need to assign a random password
                        if (!SSOAController.generateNewPasswordAndSendEmail(ref response, usc, u.UserEmail, u.UserName, Parameters)) { blnHasErrors = true; }
                    }
                }

                if (u.blnIsCarrierUser == false)
                {
                    //******* NON-CARRIER USER ******* 

                    /******************************************************
                    * Insert the UserSecurityLE record
                    ******************************************************/
                    var spRes = NGLUserSecurityLegalEntityData.InsertOrUpdateUserSecurityLE(usc, u.LEAControl);
                    if (spRes != null && spRes.ErrNumber != 0)
                    {
                        if (!blnHasErrors) { response.StatusCode = HttpStatusCode.InternalServerError; }
                        response.Errors += "There was a problem trying to automatically associate this user with a Legal Entity - you might have to do it manually. Error Message: " + spRes.RetMsg;
                        blnHasErrors = true;
                    }

                    /******************************************************
                    * Insert any Associated Carriers to UserSecurityCarrier
                    ******************************************************/
                    if (u.AssociatedCarriers?.Length > 0) {
                        foreach (Models.vUserSecurityCarrier ac in u.AssociatedCarriers)
                        {
                            try
                            {
                                LTS.tblUserSecurityCarrier ltsUSCar = new LTS.tblUserSecurityCarrier
                                {
                                    USCControl = 0, //Need to clear out the temporary index used by Add New Grid (these records will always be new since the user is new)
                                    USCUserSecurityControl = usc,
                                    USCCarrierControl = ac.USCCarrierControl,
                                    USCCarrierNumber = ac.USCCarrierNumber,
                                    USCCarrierContControl = ac.USCCarrierContControl,
                                    USCCarrierAccounting = ac.USCCarrierAccounting,
                                    USCModDate = DateTime.Now,
                                    USCModUser = Parameters.UserName
                                };
                                string strRet = NGLUserSecurityCarrierData.InsertOrUpdateUserSecurityCarrier(ltsUSCar);
                                if (!string.IsNullOrWhiteSpace(strRet))
                                {
                                    if (!blnHasErrors) { response.StatusCode = HttpStatusCode.InternalServerError; }
                                    response.Errors += strRet;
                                    blnHasErrors = true;
                                }                              
                            }
                            catch (Exception ex)
                            {
                                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                                response.StatusCode = fault.StatusCode;
                                response.Errors += fault.formatMessage();
                                blnHasErrors = true;
                                //return response; //Don't return we want to continue execution
                            }
                        }
                    }
                    

                    /********************************************************************
                    * If want to use the NGLAPI then we have to create those credentials
                    ********************************************************************/
                    if (u.AllowNGLAPI)
                    {
                        try
                        {
                            string P44WebServiceLogin = System.Configuration.ConfigurationManager.AppSettings["P44WebServiceLogin"];
                            string P44WebServicePassword = System.Configuration.ConfigurationManager.AppSettings["P44WebServicePassword"];
                            DAL.Models.SingleSignOn sso = new DAL.Models.SingleSignOn
                            {
                                SSOAXControl = 0,
                                SSOAControl = 4,
                                USC = usc,
                                SSOAXUN = P44WebServiceLogin,
                                SSOAXPass = P44WebServicePassword,
                                SSOAXRefID = u.AccountGroup,
                                UpdateP = true
                            };
                            NGLtblSingleSignOnAccountData.InsertOrUpdateSSOASecurityXref365(sso);
                        }
                        catch (Exception ex)
                        {
                            FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                            response.StatusCode = fault.StatusCode;
                            response.Errors += fault.formatMessage();
                            blnHasErrors = true;
                            return response;
                        }
                    }
                }
                else
                {
                    //******* CARRIER USER ******* 
                    //NOTE: Carriers do not have a LE associated because they should be able to see everything. I can't remember if an associated LE is a requirement for 365

                    /******************************************************
                    * Insert the UserSecurityCarrier record
                    ******************************************************/
                    LTS.tblUserSecurityCarrier ltsUSCar = new LTS.tblUserSecurityCarrier
                    {
                        USCUserSecurityControl = usc,
                        USCCarrierControl = u.AssociatedCarriers[0].USCCarrierControl,
                        USCCarrierNumber = u.AssociatedCarriers[0].USCCarrierNumber,
                        USCCarrierContControl = u.AssociatedCarriers[0].USCCarrierContControl,
                        USCCarrierAccounting = u.AssociatedCarriers[0].USCCarrierAccounting,
                        USCModDate = DateTime.Now,
                        USCModUser = Parameters.UserName
                    };
                    string strRet = NGLUserSecurityCarrierData.InsertOrUpdateUserSecurityCarrier(ltsUSCar);
                    if (!string.IsNullOrWhiteSpace(strRet))
                    {
                        if (!blnHasErrors) { response.StatusCode = HttpStatusCode.InternalServerError; }
                        response.Errors += strRet;
                        blnHasErrors = true;
                    }                  
                }

                //If there are no error messages just return a random array so we know it didn't fail
                if (!blnHasErrors)
                {
                    Array d = new bool[1] { true };
                    response = new Models.Response(d, 1);
                }
            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors += fault.formatMessage();
                return response;
            }
            return response;
        }



        [HttpGet, ActionName("GetActiveLEUsers365")]
        public Models.Response GetActiveLEUsers365(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            if (!authenticateFilter(ref response, filter)) { return response; } //Verfiy that the filters object is not null
            try
            {
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                int RecordCount = 0;
                int count = 0;
                LTS.vLEUsers365[] records = NGLUserSecurityLegalEntityData.GetActiveLEUsers365(ref RecordCount, f);
                if (records != null && records.Count() > 0)
                {
                    count = records.Count();
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

        #endregion
    }
}