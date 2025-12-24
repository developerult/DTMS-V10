using DynamicsTMS365.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web;
using System.Web.Http;
using BLL = NGL.FM.BLL;
using DAL = Ngl.FreightMaster.Data;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using DTran = Ngl.Core.Utility.DataTransformation;
using LTS = Ngl.FreightMaster.Data.LTS;

namespace DynamicsTMS365.Controllers
{
    public class SSOAController : NGLControllerBase
    {
        //Rakib: 16/10/2025
        // In-memory OTP storage with timestamp
        private static readonly Dictionary<string, (string Otp, DateTime Expiry)> OtpStorage = new Dictionary<string, (string, DateTime)>();



        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.SSOAController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion


        #region " Data Translation"


        //private DAL.Models.SSOResults selectModelData(LTS.cmLocalizeKeyValuePair d)
        //{
        //    Models.cmLocalizeKeyValuePair modelRecord = new Models.cmLocalizeKeyValuePair();
        //    if (d != null)
        //    {
        //        List<string> skipObjs = new List<string> { "cmLocalUpdated" };
        //        string sMsg = "";
        //        modelRecord = (Models.cmLocalizeKeyValuePair)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
        //        if (modelRecord != null) { modelRecord.setUpdated(d.cmLocalUpdated.ToArray()); }
        //    }

        //    return modelRecord;
        //}


        //private LTS.cmLocalizeKeyValuePair selectLTSData(Models.cmLocalizeKeyValuePair d)
        //{
        //    LTS.cmLocalizeKeyValuePair ltsRecord = new LTS.cmLocalizeKeyValuePair();
        //    if (d != null)
        //    {
        //        List<string> skipObjs = new List<string> { "cmLocalUpdated" };
        //        string sMsg = "";
        //        ltsRecord = (LTS.cmLocalizeKeyValuePair)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
        //        if (ltsRecord != null)
        //        {
        //            byte[] bupdated = d.getUpdated();
        //            ltsRecord.cmLocalUpdated = bupdated == null ? new byte[0] : bupdated;

        //        }
        //    }

        //    return ltsRecord;
        //}




        #endregion


        #region " REST Services"

        //public Models.Response Get()
        //{
        //    // create a response message to send back
        //    var response = new Models.Response(); //new HttpResponseMessage();
        //    try
        //    {
        //        Models.cmLocalizeKeyValuePair[] oData = new Models.cmLocalizeKeyValuePair[] { };
        //        //count will contains the nunber of records in the database that matches the filters before paging
        //        int count = 0; // _context.Carriers.Count();

        //        // get the take and skip parameters int skip = request["skip"] == null ? 0 :
        //        int skip = request["skip"] == null ? 0 : int.Parse(request["skip"]);
        //        int take = request["take"] == null ? 10 : int.Parse(request["take"]);
        //        DAL.NGLcmLocalizeKeyValuePairData dalData = new DAL.NGLcmLocalizeKeyValuePairData(Parameters);
        //        //List<LTS.cmLocalizeKeyValuePair> oRecords = new List<LTS.cmLocalizeKeyValuePair>();
        //        List<LTS.cmLocalizeKeyValuePair> oRecords = dalData.GetPage(skip, take, ref count);
        //        if (oRecords != null && oRecords.Count() > 0)
        //        {

        //            oData = (from e in oRecords
        //                     orderby e.cmLocalValue descending
        //                     select selectModelData(e)).ToArray();
        //        }


        //        response = new Models.Response(oData, count);
        //    }
        //    catch (Exception ex)
        //    {
        //        // something went wrong - possibly a database error. return a
        //        // 500 server error and send the details of the exception.
        //        response.StatusCode = HttpStatusCode.InternalServerError;
        //        response.Errors = string.Format("The database read failed: {0}", ex.Message);
        //        //response.
        //    }

        //    // return the HTTP Response.
        //    return response;

        //}

        /// <summary>
        /// get the current single sign on data for the current ueser
        /// on some systems this can create a free trial account
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.5.3.007 on 06/19/2023 added logic to return an 
        /// InvalidLogin error if authentication fails
        /// when calling Get365Account
        /// </remarks>
        [HttpPost, ActionName("PostSSOResults")]
        public Models.Response PostSSOResults([System.Web.Http.FromBody] DAL.Models.SSOResults result)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            DAL.NGLSecurityDataProvider oSecData = new DAL.NGLSecurityDataProvider(Parameters);
            BLL.NGLTMS365BLL bll = new BLL.NGLTMS365BLL(Parameters);
            try
            {
                int errorCode = 0;
                DateTime expires;

                //If SSOAControl = 1 we use this logic to create a Free Trial (legacy)
                if (result.SSOAControl == 1)
                {
                    if (string.IsNullOrWhiteSpace(result.SSOAClientSecret))
                    {
                        FaultExceptionEventArgs fault = new FaultExceptionEventArgs();
                        response.StatusCode = HttpStatusCode.Unauthorized;
                        response.Errors = Utilities.getLocalizedMsg("PasswordNotBlank");
                        return response;
                    }

                    //tokens expire after 30 days there are 3600 seconds in an hours 
                    int intExpMinutes = 60 * 720;
                    expires = DateTime.Now.AddSeconds(intExpMinutes);
                    DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    result.SSOAExpires = Convert.ToInt32(Math.Floor((expires.ToUniversalTime() - origin).TotalSeconds));
                    result.USATToken = Guid.NewGuid().ToString();
                    result.SSOAClientSecret = DTran.Encrypt(result.SSOAClientSecret, "NGL");
                }
                else
                {
                    expires = result.getTokenExpirationDate();
                }

                //Modified By LVV on 4/4/18 for v-8.1
                //Added optional parameters for P44WebServiceLogin and P44WebServicePassword because these always need to come from the WebConfig. 
                string P44WebServiceLogin = System.Configuration.ConfigurationManager.AppSettings["P44WebServiceLogin"];
                string P44WebServicePassword = System.Configuration.ConfigurationManager.AppSettings["P44WebServicePassword"];

                DAL.Models.SSOAccount acct = oSecData.Get365Account(result.USATUserID, expires, result.UserSecurityControl, result.USATToken, result.USATUserID, result.SSOAUserEmail, result.UserFirstName, result.UserLastName, result.SSOAAuthCode, result.SSOAControl, result.SSOAClientSecret, result.UserFriendlyName, result.UserWorkPhone, result.UserWorkPhoneExt, P44WebServiceLogin, P44WebServicePassword);
                // Modified by RHR for v-8.5.3.007 on 06/19/2023 added logic to return an error if authentication fails
                if (acct == null || acct.SSOAControl == 0)
                {
                    string sMoreInfo = "";
                    if (acct != null && !string.IsNullOrWhiteSpace(acct.AuthenticationErrorMessage))
                    {
                        sMoreInfo = acct.AuthenticationErrorMessage;
                    }
                    FaultExceptionEventArgs fault = new FaultExceptionEventArgs();
                    response.StatusCode = HttpStatusCode.Unauthorized;
                    response.Errors = Utilities.getLocalizedMsg("InvalidLogin") + " " + sMoreInfo;
                    return response;
                }
                //update the result data with the account information
                result.SSOAControl = acct.SSOAControl;
                result.UserSecurityControl = acct.SSOAUserSecurityControl;
                result.UserName = acct.SSOAUserName;
                result.UserFriendlyName = acct.UserFriendlyName;
                result.IsUserCarrier = acct.IsUserCarrier;
                result.UserCarrierControl = acct.UserCarrierControl;
                result.UserCarrierContControl = acct.UserCarrierContControl;
                result.UserLEControl = acct.UserLEControl;
                result.UserTheme365 = acct.UserTheme365;
                result.CatControl = acct.CatControl;

                //save the results
                if (Utilities.GlobalSSOResultsByUser.ContainsKey(result.UserSecurityControl))
                {
                    Utilities.GlobalSSOResultsByUser[result.UserSecurityControl] = result;
                }
                else
                {
                    Utilities.GlobalSSOResultsByUser.Add(result.UserSecurityControl, result);
                }
                response.Data = new DAL.Models.SSOResults[] { result };
                if (errorCode == 0)
                {
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    //process the correct message for errorCode:
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Errors = string.Format("Validation Failed Because {0}", "Example Message:  Validation Failed");
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


        /// <summary>
        /// Works just like PostSSOResults except it sends NGL an email that a new user has signed up for a free trial
        /// is used for NGL Free Trial sign op not for App Source Free Trial Sign up
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        [HttpPost, ActionName("PostNGLFreeTrialSignup")]
        public Models.Response PostNGLFreeTrialSignup([System.Web.Http.FromBody] DAL.Models.SSOResults result)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            DAL.NGLSecurityDataProvider oSecData = new DAL.NGLSecurityDataProvider(Parameters);
            BLL.NGLTMS365BLL bll = new BLL.NGLTMS365BLL(Parameters);

            try
            {
                int errorCode = 0;
                DateTime expires;

                //If SSOAControl = 1 we use this logic to create a Free Trial (legacy)
                if (result.SSOAControl == 1)
                {
                    if (string.IsNullOrWhiteSpace(result.SSOAClientSecret))
                    {
                        FaultExceptionEventArgs fault = new FaultExceptionEventArgs();
                        response.StatusCode = HttpStatusCode.Unauthorized;
                        response.Errors = Utilities.getLocalizedMsg("PasswordNotBlank");
                        return response;
                    }

                    //tokens expire after 30 days there are 3600 seconds in an hours 
                    int intExpMinutes = 3600 * 720;
                    expires = DateTime.Now.AddSeconds(intExpMinutes);
                    DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    result.SSOAExpires = Convert.ToInt32(Math.Floor((expires.ToUniversalTime() - origin).TotalSeconds));
                    result.USATToken = Guid.NewGuid().ToString();
                    result.SSOAClientSecret = DTran.Encrypt(result.SSOAClientSecret, "NGL");
                }
                else
                {
                    expires = result.getTokenExpirationDate();
                }

                //Modified By LVV on 4/4/18 for v-8.1
                //Added optional parameters for P44WebServiceLogin and P44WebServicePassword because these always need to come from the WebConfig. 
                string P44WebServiceLogin = System.Configuration.ConfigurationManager.AppSettings["P44WebServiceLogin"];
                string P44WebServicePassword = System.Configuration.ConfigurationManager.AppSettings["P44WebServicePassword"];

                DAL.Models.SSOAccount acct = oSecData.Get365Account(result.USATUserID, expires, result.UserSecurityControl, result.USATToken, result.USATUserID, result.SSOAUserEmail, result.UserFirstName, result.UserLastName, result.SSOAAuthCode, result.SSOAControl, result.SSOAClientSecret, result.UserFriendlyName, result.UserWorkPhone, result.UserWorkPhoneExt, P44WebServiceLogin, P44WebServicePassword);
                //update the result data with the account information
                result.SSOAControl = acct.SSOAControl;
                result.UserSecurityControl = acct.SSOAUserSecurityControl;
                result.UserName = acct.SSOAUserName;
                result.UserFriendlyName = acct.UserFriendlyName;
                result.IsUserCarrier = acct.IsUserCarrier;
                result.UserCarrierControl = acct.UserCarrierControl;
                result.UserCarrierContControl = acct.UserCarrierContControl;
                result.UserLEControl = acct.UserLEControl;
                result.UserTheme365 = acct.UserTheme365;
                result.CatControl = acct.CatControl;

                //save the results
                if (DynamicsTMS365.Utilities.GlobalSSOResultsByUser.ContainsKey(result.UserSecurityControl))
                {
                    DynamicsTMS365.Utilities.GlobalSSOResultsByUser[result.UserSecurityControl] = result;
                }
                else
                {
                    DynamicsTMS365.Utilities.GlobalSSOResultsByUser.Add(result.UserSecurityControl, result);
                }

                response.Data = new DAL.Models.SSOResults[] { result };


                if (errorCode == 0)
                {
                    response.StatusCode = HttpStatusCode.OK;
                    //Modified by RHR for v-8.2 on 07/18/2018 added email logic for NGL Free Trial Signups 
                    System.Text.StringBuilder sbEmailNGL = new System.Text.StringBuilder();
                    System.Text.StringBuilder sbEmailUser = new System.Text.StringBuilder();
                    DAL.NGLEmailData oMail = new DAL.NGLEmailData(Parameters);
                    string nglSignUpEmailTo = System.Configuration.ConfigurationManager.AppSettings["NGLSignUpEmailTo"];
                    if (string.IsNullOrWhiteSpace(nglSignUpEmailTo)) { nglSignUpEmailTo = "developer@nextgeneration.com"; }
                    string sFrom = System.Configuration.ConfigurationManager.AppSettings["SmtpFromAddress"];
                    string newLine = System.Environment.NewLine + "<br />";

                    //Send an email to the NGL Admin person who handles the sign ups
                    sbEmailNGL.Append("New Free Trial Sign Up Request");
                    sbEmailNGL.Append(newLine);
                    sbEmailUser.Append(newLine);
                    sbEmailNGL.Append("A user has submitted a Free Trial requested with the following information: ");
                    sbEmailNGL.Append(newLine);
                    sbEmailNGL.Append("USER");
                    sbEmailNGL.Append(newLine);
                    sbEmailNGL.Append(string.Format("User Control Nbr: {0}, Friendly Name: {1}, User Email: {2}", result.UserSecurityControl, result.UserFriendlyName, result.SSOAUserEmail));
                    sbEmailNGL.Append(newLine);

                    try
                    {
                        oMail.GenerateEmail(sFrom, nglSignUpEmailTo, "", "New Free Trial Request", sbEmailNGL.ToString(), "", "", "", "");
                    }
                    catch (Exception ex)
                    {
                        FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                        string msg = string.Format("Error generating New Free Trial Request email to NGL for User Friendly Name: {0}  User Email: {1}. Err Msg: {2} ", result.UserFriendlyName, result.SSOAUserEmail, fault.formatMessage());
                        SaveAppError(msg);
                    }

                    //Send an email to the user who requested the sign up
                    sbEmailUser.Append("Thank you for your request for a Dynamics TMS 365 Free Trial!");
                    sbEmailUser.Append(newLine);
                    sbEmailUser.Append(newLine);
                    sbEmailUser.Append(string.Format("The following information has been sent to the administrator with your request. Once your request is verified someone will contact you with more information and your Free Trial will begin. If you would like to change any of the information or if you have any questions, please send an email to {0}.", nglSignUpEmailTo));
                    sbEmailUser.Append(newLine);
                    sbEmailUser.Append(newLine);
                    sbEmailUser.Append("User Information");
                    sbEmailUser.Append(newLine);
                    sbEmailUser.Append(string.Format("First Name: {0} {6} Last Name: {1} {6} Friendly Name: {2} {6} Phone: {3}  Ext: {4} {6} Email: {5} {6} {6}", result.UserFirstName, result.UserLastName, result.UserFriendlyName, result.UserWorkPhone, result.UserWorkPhoneExt, result.SSOAUserEmail, newLine));

                    try
                    {
                        oMail.GenerateEmail(sFrom, result.SSOAUserEmail, "", "Dynamics TMS 365 Free Trial Request Pending", sbEmailUser.ToString(), "", "", "", "");
                    }
                    catch (Exception ex)
                    {
                        FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                        string msg = string.Format("Error generating New Free Trial Request email to User: {0}, Email: {1}. Err Msg: {2} ", result.SSOAUserEmail, result.UserFriendlyName, fault.formatMessage());
                        SaveAppError(msg);
                    }
                }
                else
                {
                    //process the correct message for errorCode:
                    // note this may never happend because errorCode is not used
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Errors = string.Format("Validation Failed Because {0}", "Unexpected Error Contact Technical Support");
                }
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
        /// NOTE: I ADDED THE PARAMETER int id AS A WORKAROUND TO DISTINGUISH IT FROM
        /// THE OTHER GET METHOD GetNGLSSOAToken BECAUSE THE ROUTE MAPPING WAS NOT
        /// CALLING THE CORRECT METHOD
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, ActionName("GetFreeTrialInfo")]
        public Models.Response GetFreeTrialInfo(int id)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            Models.GenericResult gr = new Models.GenericResult();

            try
            {
                DAL.NGLSecurityDataProvider dalSecData = new DAL.NGLSecurityDataProvider(Parameters);
                DTO.tblUserSecurity usec = dalSecData.GettblUserSecurity(UserControl);

                gr.blnField = usec.UserFreeTrialActive;
                gr.strField = usec.UserFriendlyName;
                gr.strField2 = usec.UserEmail;
                gr.strField3 = usec.UserEndFreeTrial.ToString();
                gr.dtField = usec.UserEndFreeTrial;

            }
            catch (Exception ex)
            {
                //Error handler
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }

            int count = 1;
            Array d = new Models.GenericResult[1] { gr };
            response = new Models.Response(d, count);
            return response;
        }


        /// <summary>
        /// Place Holder for Future if needed
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        [HttpPost, ActionName("PostNGLLegacyResults")]
        public Models.Response PostNGLLegacyResults(string res)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            //BLL.NGLTMS365BLL bll = new BLL.NGLTMS365BLL(Parameters);

            //try
            //{
            //    int errorCode = 0;
            //    int intUserSecurityControl = res.UserSecurityControl;
            //    if (intUserSecurityControl == 0)
            //    {
            //        //TODO: add code to call DAL methods used to create a new trial user
            //        // or to link this user sign in with a new user account
            //        //manage errors  and populate errorCode
            //    }
            //    else
            //    {
            //        //TODO: add code to save user info to database (asynchronously)
            //        //manage errors and populate errorCode
            //        bll.Save365USATokenAsync(res);
            //    }
            //    //save the results
            //    if (DynamicsTMS365.Utilities.GlobalSSOResultsByUser.ContainsKey(intUserSecurityControl))
            //    {
            //        DynamicsTMS365.Utilities.GlobalSSOResultsByUser[intUserSecurityControl] = res;
            //    }
            //    else
            //    {
            //        DynamicsTMS365.Utilities.GlobalSSOResultsByUser.Add(intUserSecurityControl, res);
            //    }



            //    if (errorCode == 0)
            //    {
            //        response.StatusCode = HttpStatusCode.OK;
            //    }
            //    else
            //    {
            //        //process the correct message for errorCode:
            //        response.StatusCode = HttpStatusCode.InternalServerError;
            //        response.Errors = string.Format("Validation Failed Because {0}", "Example Message:  Validation Failed");

            //    }
            //}
            //catch (Exception ex)
            //{
            //    // something went wrong - possibly a database error. return a
            //    // 500 server error and send the details of the exception.
            //    response.StatusCode = HttpStatusCode.InternalServerError;
            //    response.Errors = string.Format("The database updated failed: {0}", ex.Message);
            //    //response.
            //}

            // return the HTTP Response.
            return response;
        }

        /// <summary>
        /// Place Holder for future if needed
        /// </summary>
        /// <param name="nglr"></param>
        /// <returns></returns>
        [HttpPost, ActionName("PostNGLRResults")]
        public Models.Response PostNGLRResults(string nglr)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            //BLL.NGLTMS365BLL bll = new BLL.NGLTMS365BLL(Parameters);

            //try
            //{
            //    int errorCode = 0;
            //    int intUserSecurityControl = nglr.UserSecurityControl;
            //    if (intUserSecurityControl == 0)
            //    {
            //        //TODO: add code to call DAL methods used to create a new trial user
            //        // or to link this user sign in with a new user account
            //        //manage errors  and populate errorCode
            //    }
            //    else
            //    {
            //        //TODO: add code to save user info to database (asynchronously)
            //        //manage errors and populate errorCode
            //        bll.Save365USATokenAsync(nglr);
            //    }
            //    //save the results
            //    if (DynamicsTMS365.Utilities.GlobalSSOResultsByUser.ContainsKey(intUserSecurityControl))
            //    {
            //        DynamicsTMS365.Utilities.GlobalSSOResultsByUser[intUserSecurityControl] = nglr;
            //    }
            //    else
            //    {
            //        DynamicsTMS365.Utilities.GlobalSSOResultsByUser.Add(intUserSecurityControl, nglr);
            //    }



            //    if (errorCode == 0)
            //    {
            //        response.StatusCode = HttpStatusCode.OK;
            //    }
            //    else
            //    {
            //        //process the correct message for errorCode:
            //        response.StatusCode = HttpStatusCode.InternalServerError;
            //        response.Errors = string.Format("Validation Failed Because {0}", "Example Message:  Validation Failed");

            //    }
            //}
            //catch (Exception ex)
            //{
            //    // something went wrong - possibly a database error. return a
            //    // 500 server error and send the details of the exception.
            //    response.StatusCode = HttpStatusCode.InternalServerError;
            //    response.Errors = string.Format("The database updated failed: {0}", ex.Message);
            //    //response.
            //}

            // return the HTTP Response.
            return response;
        }

        /// <summary>
        /// Legacy NGL User sign in Controller 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.5.4.005 on 01/29/2024 moved security information fro filter into header
        ///     filter is no longer used.
        /// </remarks>
        /// 
        /// Block on 16/10/2025 by Rakib for 2 factor authentication implementation
        //[HttpGet, ActionName("GetNGLSSOAToken")]
        //public Models.Response GetNGLSSOAToken(string filter)
        //{
        //    // create a response message to send back
        //    var response = new Models.Response(); //new HttpResponseMessage();
        //    try
        //    {

        //        string sNGLClass14 = HttpContext.Current.Request.Headers["NGLClass14"];
        //        DAL.Models.NGLClass14 oNGLClass14 = new DAL.Models.NGLClass14();
        //        if(!string.IsNullOrWhiteSpace(sNGLClass14))
        //        {
        //            try {
        //                oNGLClass14 = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.NGLClass14>(sNGLClass14);
        //            } catch (Exception ex) { 
        //                //do nothing
        //            }
        //        }
        //        //if (!string.IsNullOrWhiteSpace(sNGLClass14))
        //        //{
        //        //    try
        //        //    {
        //        //        oNGLClass14 = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.NGLClass14>(sNGLClass14);

        //        //    } catch(Exception ex) { //do nothing  }

        //        //}

        //        //DAL.Models.NGLClass14 ofilter = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.NGLClass14>(filter);
        //        //DAL.Models.NGLClass14[] orders = new DAL.Models.NGLClass14[] { };
        //        int count = 0;

        //        DAL.NGLSecurityDataProvider dalSecData = new DAL.NGLSecurityDataProvider(Parameters);
        //        //var ssoRes = dalSecData.NGLLegacySignIn(ofilter);

        //            var ssoRes = dalSecData.NGLLegacySignIn(oNGLClass14);
        //        if (ssoRes != null)
        //        {
        //            //save the results
        //            if (DynamicsTMS365.Utilities.GlobalSSOResultsByUser.ContainsKey(ssoRes.UserSecurityControl))
        //            {
        //                DynamicsTMS365.Utilities.GlobalSSOResultsByUser[ssoRes.UserSecurityControl] = ssoRes;
        //            }
        //            else
        //            {
        //                DynamicsTMS365.Utilities.GlobalSSOResultsByUser.Add(ssoRes.UserSecurityControl, ssoRes);
        //            }

        //            DAL.Models.SSOResults[] resArray = new DAL.Models.SSOResults[] { ssoRes };
        //            count = resArray.Count();
        //            response = new Models.Response(resArray, count);
        //        }
        //        else
        //        {
        //            response.Errors = "";
        //            response.StatusCode = HttpStatusCode.Forbidden;
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        //Error handler
        //        FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
        //        response.StatusCode = fault.StatusCode;
        //        response.Errors = fault.formatMessage();
        //        return response;
        //    }
        //    //catch (FaultException<DAL.SqlFaultInfo> ex)
        //    //{
        //    //    response.StatusCode = HttpStatusCode.BadRequest;
        //    //    //Note: this error handler is english only.  We need to add code to 
        //    //    // support localization text
        //    //    string sMsg = ex.Detail.ToString(ex.Reason.ToString());
        //    //    response.Errors = string.Format("Get Orders Filtered Failed: {0}", sMsg);
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    // something went wrong - possibly a database error. return a
        //    //    // 500 server error and send the details of the exception.
        //    //    response.StatusCode = HttpStatusCode.InternalServerError;
        //    //    response.Errors = string.Format("The database read failed: {0}", ex.Message);
        //    //    //response.
        //    //}

        //    // return the HTTP Response.
        //    return response;

        //}

        //Rakib: 16/10/2025
        [HttpGet, ActionName("GetNGLSSOAToken")]
        public Models.Response GetNGLSSOAToken(string filter)
        {
            var response = new Models.Response();

            string sNGLClass14 = HttpContext.Current.Request.Headers["NGLClass14"];
            DAL.Models.NGLClass14 oNGLClass14 = new DAL.Models.NGLClass14();

            try
            {
                if (!string.IsNullOrWhiteSpace(sNGLClass14))
                {
                    try
                    {
                        oNGLClass14 = new System.Web.Script.Serialization.JavaScriptSerializer()
                            .Deserialize<DAL.Models.NGLClass14>(sNGLClass14);
                    }
                    catch (Exception)
                    {
                        // Do nothing
                    }
                }

                string isEmailVerificationEnable = System.Configuration.ConfigurationManager.AppSettings["IsEmailVerificationEnable"];

                if (isEmailVerificationEnable == "True")
                {
                    if (filter != "NaN")
                    {
                        // Validate OTP
                        string username = oNGLClass14?.NGLvar1455; // Assuming NGLClass14 has UserName property
                        if (string.IsNullOrEmpty(username) || !OtpStorage.ContainsKey(username))
                        {
                            response.Errors = "Invalid OTP";
                            response.StatusCode = HttpStatusCode.BadRequest;
                            // Remove OTP if exists
                            if (!string.IsNullOrEmpty(username) && OtpStorage.ContainsKey(username))
                            {
                                OtpStorage.Remove(username);
                            }
                            return response;
                        }

                        var (storedOtp, expiry) = OtpStorage[username];
                        if (expiry < DateTime.UtcNow || storedOtp != filter)
                        {
                            response.Errors = "Invalid or expired OTP";
                            response.StatusCode = HttpStatusCode.BadRequest;
                            OtpStorage.Remove(username);
                            return response;
                        }

                        // OTP is valid, remove it from storage
                        OtpStorage.Remove(username);
                    }
                }

                int count = 0;
                DAL.NGLSecurityDataProvider dalSecData = new DAL.NGLSecurityDataProvider(Parameters);
                var ssoRes = dalSecData.NGLLegacySignIn(oNGLClass14);
                if (ssoRes != null)
                {
                    // Save the results
                    if (DynamicsTMS365.Utilities.GlobalSSOResultsByUser.ContainsKey(ssoRes.UserSecurityControl))
                    {
                        DynamicsTMS365.Utilities.GlobalSSOResultsByUser[ssoRes.UserSecurityControl] = ssoRes;
                    }
                    else
                    {
                        DynamicsTMS365.Utilities.GlobalSSOResultsByUser.Add(ssoRes.UserSecurityControl, ssoRes);
                    }

                    DAL.Models.SSOResults[] resArray = new DAL.Models.SSOResults[] { ssoRes };
                    count = resArray.Count();
                    response = new Models.Response(resArray, count);
                }
                else
                {
                    response.Errors = "";
                    response.StatusCode = HttpStatusCode.Forbidden;
                }
            }
            catch (Exception ex)
            {
                // Error handler
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                // Remove OTP if exists
                if (!string.IsNullOrEmpty(oNGLClass14?.NGLvar1455) && OtpStorage.ContainsKey(oNGLClass14.NGLvar1455))
                {
                    OtpStorage.Remove(oNGLClass14.NGLvar1455);
                }
            }
            return response;
        }


        //Rakib: 16/10/2025
        [HttpPost, ActionName("SendMailToAuthenticateUser")]
        public Models.Response SendMailToAuthenticateUser(string filter)
        {
            var response = new Models.Response();

            string sNGLClass14 = HttpContext.Current.Request.Headers["NGLClass14"];
            DAL.Models.NGLClass14 oNGLClass14 = new DAL.Models.NGLClass14();

            try
            {
                if (!string.IsNullOrWhiteSpace(sNGLClass14))
                {
                    try
                    {
                        oNGLClass14 = new System.Web.Script.Serialization.JavaScriptSerializer()
                            .Deserialize<DAL.Models.NGLClass14>(sNGLClass14);
                    }
                    catch (Exception)
                    {
                        // Do nothing
                    }
                }

                if (!string.IsNullOrEmpty(oNGLClass14?.NGLvar1455) && OtpStorage.ContainsKey(oNGLClass14.NGLvar1455))
                {
                    OtpStorage.Remove(oNGLClass14.NGLvar1455);
                }

                DAL.NGLSecurityDataProvider dalSecData = new DAL.NGLSecurityDataProvider(Parameters);
                var ssoRes = dalSecData.NGLLegacySignIn(oNGLClass14);

                var email = ssoRes.SSOAUserEmail;
                var username = ssoRes.UserName;

                // Generate OTP
                Random random = new Random();
                var otpLength = 6;
                const string chars = "0123456789";
                var generatedOTP = new string(Enumerable.Repeat(chars, otpLength)
                    .Select(s => s[random.Next(s.Length)]).ToArray());

                //var generatedOTP = "1234";

                #region WebMail SMTP
                string smtp_FEmail = System.Configuration.ConfigurationManager.AppSettings["SMTP_Email"];
                string smtp_Password = System.Configuration.ConfigurationManager.AppSettings["SMTP_Password"];
                string smtp_Domain = System.Configuration.ConfigurationManager.AppSettings["SMTP_Domain"];
                string smtp_Port = System.Configuration.ConfigurationManager.AppSettings["SMTP_Port"];

                //System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(smtp_Domain, Convert.ToInt32(smtp_Port));
                //client.EnableSsl = true;
                //client.UseDefaultCredentials = false;
                //client.Credentials = new NetworkCredential(smtp_Email, smtp_Password);

                // Create email message
                //MailMessage mailMessage = new MailMessage();
                //mailMessage.From = new MailAddress(smtp_Email);
                //mailMessage.To.Add(email);
                //mailMessage.Subject = "Email verification code";
                //mailMessage.IsBodyHtml = true;
                //mailMessage.Body = 

                string sSubject = "TMS Verification Code";
                string sBody = "<!DOCTYPE html>" +
                                  "<html lang='en'>" +
                                  "<head>" +
                                      "<meta charset='UTF-8'>" +
                                      "<meta name='viewport' content='width=device-width, initial-scale=1.0'>" +
                                      "<title>Email Verification</title>" +
                                  "</head>" +
                                  "<body style='margin: 0; padding: 0; font-family: Arial, sans-serif; background-color: #f4f4f4;'>" +
                                      "<table role='presentation' cellspacing='0' cellpadding='0' border='0' width='100%' style='background-color: #f4f4f4;'>" +
                                          "<tr>" +
                                              "<td style='padding: 20px 0;'>" +
                                                  "<table role='presentation' cellspacing='0' cellpadding='0' border='0' align='center' width='600' style='background-color: #ffffff; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,0.1);'>" +
                                                      "<tr>" +
                                                          "<td style='padding: 40px 30px; text-align: left; color: #333;'>" +
                                                              "<p style='font-size: 16px; line-height: 1.5; margin: 0 0 20px;'>" +
                                                                  "Hello " + username + ",<br><br>" +
                                                                  "Thank You for choosing NGL!<br><br>" +
                                                                  "To verify your email, please enter the following 6-digit code:<br><br>" +
                                                              "</p>" +
                                                              "<div style='text-align: center; background-color: #f8f9fa; border: 1px solid #e9ecef; border-radius: 8px; padding: 20px; margin: 0 0 20px;'>" +
                                                                  "<span style='font-size: 36px; font-weight: bold; color: #874696; letter-spacing: 8px;'>" + generatedOTP + "</span>" +
                                                              "</div>" +
                                                              "<p style='font-size: 16px; line-height: 1.5; margin: 0 0 20px;'>" +
                                                                  "This code will expire in 5 minutes for security reasons.<br><br>" +
                                                                  "Best wishes on your journey with us.<br><br>" +
                                                                  "In the meantime, if you have any questions, feel free to contact with us and we will get back to you." +
                                                              "</p>" +
                                                          "</td>" +
                                                      "</tr>" +
                                                  "</table>" +
                                              "</td>" +
                                          "</tr>" +
                                      "</table>" +
                                  "</body>" +
                                  "</html>";

                string emailBody = "<!DOCTYPE html>" +
                                "<html lang='en'>" +
                                "<head>" +
                                "<meta charset='UTF-8'>" +
                                "<meta name='viewport' content='width=device-width, initial-scale=1.0'>" +
                                "<title>TMS One-Time Password</title>" +
                                "</head>" +
                                "<body style='margin:0; padding:0; background-color:#f6f7fb; font-family:Arial, sans-serif;'>" +
                                "<table role='presentation' cellspacing='0' cellpadding='0' border='0' width='100%' style='background-color:#f6f7fb;'>" +
                                "<tr>" +
                                    "<td align='center' style='padding:24px 12px;'>" +
                                        "<table role='presentation' cellspacing='0' cellpadding='0' border='0' width='600' style='max-width:600px; width:100%; background-color:#ffffff; border-radius:6px; overflow:hidden; box-shadow:0 2px 6px rgba(0,0,0,0.1);'>" +
                                            "<tr>" +
                                                "<td style='background:#0b3a6b; text-align:center; padding:26px 20px;'>" +
                                                    "<h1 style='margin:0; color:#ffffff; font-size:26px; font-weight:700;'>Your TMS One-Time Password</h1>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td style='padding:28px 36px 20px 36px; text-align:center;'>" +
                                                    "<p style='margin:0 0 18px; font-size:16px; font-weight:600; color:#111827;'>To continue with your login, please enter the following verification code</p>" +
                                                    "<div style='display:inline-block; background:#f4f6f8; border-radius:6px; padding:22px 36px; min-width:230px; margin-top:20px;'>" +
                                                        "<span style='font-size:40px; font-weight:700; color:#7b2d7b; letter-spacing:6px;'>" + generatedOTP + "</span>" +
                                                    "</div>" +
                                                    "<p style='margin:22px 0 0; font-size:15px; font-weight:700; color:#111827;'>If you have trouble accessing your account, please contact your admin.</p>" +
                                                "</td>" +
                                            "</tr>" +
                                        "</table>" +
                                    "</td>" +
                                "</tr>" +
                                "</table>" +
                                "</body>" +
                                "</html>";


                //string otp = generatedOTP;  // dynamically generated OTP
                //string finalEmailBody = string.Format(emailBody, otp);


                // Send email
                //client.Send(mailMessage);
                //DAL.NGLEmailData oMail = new DAL.NGLEmailData(Parameters);
                //oMail.GenerateEmail(smtp_FEmail, email, "", sSubject, emailBody, "", "", "", "");
                
                SendEmailUsingSMTP(smtp_FEmail, email, sSubject, emailBody);

                // Store OTP with 2-minute expiry after email is sent
                OtpStorage[username] = (generatedOTP, DateTime.UtcNow.AddMinutes(5));
                #endregion

                response.Messages = email;
                response.Status = true;
                return response;
            }
            catch (Exception ex)
            {
                // Error handler
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Status = false;
                response.Errors = fault.formatMessage();
                // Remove OTP if exists
                if (!string.IsNullOrEmpty(oNGLClass14?.NGLvar1455) && OtpStorage.ContainsKey(oNGLClass14.NGLvar1455))
                {
                    OtpStorage.Remove(oNGLClass14.NGLvar1455);
                }
                return response;
            }
        }




        public void SendEmailUsingSMTP(string From_email, string To_Email, string Subject, string emailBody)
        {
            //try
            //{
            //    var smtp = new SmtpClient("smtp.office365.com")
            //    {
            //        Port = 587,
            //        Credentials = new NetworkCredential("smtp@nextgeneration.com", "NextGen#1"),
            //        EnableSsl = true,  // STARTTLS
            //        UseDefaultCredentials = false,
            //    };

            //    MailMessage mail = new MailMessage();
            //    mail.From = new MailAddress(From_email);
            //    mail.To.Add(To_Email);
            //    mail.Subject = Subject;
            //    mail.Body = emailBody;

            //    smtp.Send(mail);
            //} catch(Exception ex)
            //{
            //    FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
            //}


            try
            {
                //Force TLS 1.2+ (very important for modern mail servers)
                System.Net.ServicePointManager.SecurityProtocol =
                    SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;

                //Optional: trust all certificates (for self-hosted mail servers)
                ServicePointManager.ServerCertificateValidationCallback =
                    (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true;

                SmtpClient smtp = new SmtpClient
                {
                    Host = "smtp.office365.com",       // e.g., "smtp.office365.com" or "smtp.gmail.com"
                    Port = 587,                      // Try 587 (STARTTLS) first
                    EnableSsl = true,                // STARTTLS
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("smtp@nextgeneration.com", "NextGen#1"),
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };

                MailMessage mail = new MailMessage
                {
                    From = new MailAddress(From_email),
                    Subject = Subject,
                    Body = emailBody,
                    IsBodyHtml = true
                };

                mail.To.Add(To_Email);

                smtp.Send(mail);
                Console.WriteLine("Email sent successfully!");
            }
            catch (SmtpException ex)
            {
                Console.WriteLine("SMTP ERROR: " + ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine("INNER: " + ex.InnerException.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GENERAL ERROR: " + ex.Message);
            }
        }



        /// <summary>
        /// Validate the users token for the NGL Legacy accounts
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.5.4.005 on 01/29/2024 moved security information fro filter into header
        ///     filter is no longer used.
        /// </remarks>
        [HttpGet, ActionName("NGLLegacyValidateToken")]
        public Models.Response NGLLegacyValidateToken(string filter)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            try
            {

                string sNGLClass14 = HttpContext.Current.Request.Headers["NGLClass14"];
                DAL.Models.NGLClass14 oNGLClass14 = new DAL.Models.NGLClass14();
                if (!string.IsNullOrWhiteSpace(sNGLClass14))
                {
                    try
                    {
                        oNGLClass14 = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.NGLClass14>(sNGLClass14);
                    }
                    catch (Exception ex)
                    {
                        //do nothing
                    }
                }
                // DAL.Models.NGLClass14 ofilter = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.NGLClass14>(filter);
                //DAL.Models.NGLClass14[] orders = new DAL.Models.NGLClass14[] { };


                DAL.NGLSecurityDataProvider dalSecData = new DAL.NGLSecurityDataProvider(Parameters);
                //bool blnRet = dalSecData.NGLLegacyValidateToken(ofilter);
                bool blnRet = dalSecData.NGLLegacyValidateToken(oNGLClass14);


                bool[] oRecords = new bool[1];

                oRecords[0] = blnRet;

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



        /// <summary>
        /// Send an email with an auto generated password
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost, ActionName("PostNGLSendPassword")]
        public Models.Response PostNGLSendPassword(string filter)
        {
            //filter = Utilities.decodeNGLReservedCharacters(filter);

            string sNGLUserName = HttpContext.Current.Request.Headers["NGLUserName"];
            if (!string.IsNullOrWhiteSpace(sNGLUserName))
            {
                try
                {
                    sNGLUserName = Utilities.decodeNGLReservedCharacters(sNGLUserName);
                }
                catch (Exception ex)
                {
                    //do nothing
                }
            }
            //string asciichar = (Convert.ToChar(200)).ToString();
            //filter = filter.Replace(asciichar, "\\");

            //asciichar = (Convert.ToChar(201)).ToString();
            //filter = filter.Replace(asciichar, "/");

            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            try
            {
                DTO.tblUserSecurity usec;
                //step 1 validate user
                DAL.NGLSecurityDataProvider dalSecData = new DAL.NGLSecurityDataProvider(Parameters);
                try
                {
                    usec = dalSecData.GettblUserSecurityByUserName(sNGLUserName);
                    if (usec == null || usec.UserSecurityControl == 0)
                    {
                        FaultExceptionEventArgs fault = new FaultExceptionEventArgs();
                        response.StatusCode = HttpStatusCode.Unauthorized;
                        List<string> DetailsList = new List<string>();
                        DetailsList.Add(filter);
                        response.Errors = fault.formatMessage("", "E_NoUserSecurityForUser", DetailsList);
                        return response;

                    }
                    else if (string.IsNullOrWhiteSpace(usec.UserEmail))
                    {
                        FaultExceptionEventArgs fault = new FaultExceptionEventArgs();
                        response.StatusCode = HttpStatusCode.Unauthorized;
                        List<string> DetailsList = new List<string>();
                        DetailsList.Add(filter);
                        response.Errors = fault.formatMessage("", "E_NoUserEmailForUser", DetailsList);
                        return response;
                    }
                }
                catch (Exception ex)
                {
                    FaultExceptionEventArgs fault = new FaultExceptionEventArgs();
                    response.StatusCode = HttpStatusCode.Unauthorized;
                    List<string> DetailsList = new List<string>();
                    DetailsList.Add(filter);
                    response.Errors = fault.formatMessage("", "E_NoUserSecurityForUser", DetailsList);
                    return response;
                }

                /******************************************
                * step 2 generate a new password
                * step 3 save the password to the database
                * step 4 send the email
                ******************************************/
                generateNewPasswordAndSendEmail(ref response, usec.UserSecurityControl, usec.UserEmail, filter, Parameters);

                //****** MOVED TO NEW METHOD generateNewPasswordAndSendEmail() *****************************************************************************
                //////set 2 generate a new password
                ////string sPassword = Utilities.GenerateRandomPassword();
                //////step 3 save the password to the database
                ////if( dalSecData.tblUserSecuritySaveNewPassword(usec.UserSecurityControl, sPassword))
                ////{
                ////    //step 4 send the email
                ////    DAL.NGLEmailData oMail = new DAL.NGLEmailData(Parameters);

                ////    try
                ////    {
                ////        //TODO: read new configuration settings for email from, ResetPasswordEmailSubject, ResetPasswordEmailBody
                ////        string sFrom = System.Configuration.ConfigurationManager.AppSettings["SmtpFromAddress"];
                ////        string sSubject = System.Configuration.ConfigurationManager.AppSettings["ResetPasswordEmailSubject"];
                ////        string sBody = System.Configuration.ConfigurationManager.AppSettings["ResetPasswordEmailBody"].Replace(Utilities.newLine, Environment.NewLine);
                ////        sBody += sPassword;
                ////        oMail.GenerateEmail(sFrom, usec.UserEmail, "", sSubject, sBody, "", "", "", "");
                ////    }
                ////    catch (Exception ex)
                ////    {
                ////        FaultExceptionEventArgs fault = new FaultExceptionEventArgs();
                ////        if (ex.GetType() == typeof(FaultException<DAL.SqlFaultInfo>))
                ////        {
                ////            string sMsg = "";
                ////            string sReason = "";
                ////            if (((FaultException<DAL.SqlFaultInfo>)ex).Detail != null)
                ////            {
                ////                sMsg = ((FaultException<DAL.SqlFaultInfo>)ex).Detail.Message;
                ////                sReason = ((FaultException<DAL.SqlFaultInfo>)ex).Reason.ToString();
                ////            }
                ////            if ((!string.IsNullOrWhiteSpace(sMsg) && sMsg == "E_SQLExceptionMSG") || (!string.IsNullOrWhiteSpace(sReason) && sReason == "E_SQLExceptionMSG"))
                ////            {
                ////                //cannot generate email please try again
                ////                //SendEmailError                            
                ////                response.StatusCode = HttpStatusCode.InternalServerError;
                ////                response.Errors = fault.formatMessage("", "SendEmailError", null);
                ////                return response;
                ////            }
                ////        }
                ////        fault = Utilities.ManageExceptions(ref ex);
                ////        response.StatusCode = fault.StatusCode;
                ////        response.Errors = fault.formatMessage();
                ////        return response;
                ////    }
                ////} else
                ////{
                ////    FaultExceptionEventArgs fault = new FaultExceptionEventArgs();
                ////    response.StatusCode = HttpStatusCode.Unauthorized;
                ////    List<string> DetailsList = new List<string>();
                ////    DetailsList.Add(filter);
                ////    response.Errors = fault.formatMessage("", "E_NoUserSecurityForUser", DetailsList);
                ////    return response;
                ////}
                //******************************************************************************************************************************************

            }

            catch (Exception ex)
            {

                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }


            //BLL.NGLTMS365BLL bll = new BLL.NGLTMS365BLL(Parameters);

            //try
            //{
            //    int errorCode = 0;
            //    int intUserSecurityControl = nglr.UserSecurityControl;
            //    if (intUserSecurityControl == 0)
            //    {
            //        //TODO: add code to call DAL methods used to create a new trial user
            //        // or to link this user sign in with a new user account
            //        //manage errors  and populate errorCode
            //    }
            //    else
            //    {
            //        //TODO: add code to save user info to database (asynchronously)
            //        //manage errors and populate errorCode
            //        bll.Save365USATokenAsync(nglr);
            //    }
            //    //save the results
            //    if (DynamicsTMS365.Utilities.GlobalSSOResultsByUser.ContainsKey(intUserSecurityControl))
            //    {
            //        DynamicsTMS365.Utilities.GlobalSSOResultsByUser[intUserSecurityControl] = nglr;
            //    }
            //    else
            //    {
            //        DynamicsTMS365.Utilities.GlobalSSOResultsByUser.Add(intUserSecurityControl, nglr);
            //    }



            //    if (errorCode == 0)
            //    {
            //        response.StatusCode = HttpStatusCode.OK;
            //    }
            //    else
            //    {
            //        //process the correct message for errorCode:
            //        response.StatusCode = HttpStatusCode.InternalServerError;
            //        response.Errors = string.Format("Validation Failed Because {0}", "Example Message:  Validation Failed");

            //    }
            //}
            //catch (Exception ex)
            //{
            //    // something went wrong - possibly a database error. return a
            //    // 500 server error and send the details of the exception.
            //    response.StatusCode = HttpStatusCode.InternalServerError;
            //    response.Errors = string.Format("The database updated failed: {0}", ex.Message);
            //    //response.
            //}

            // return the HTTP Response.
            return response;
        }


        /// <summary>
        /// Update the user account with a new password,  user validation is required so the user must be logged in already
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.0 on 09/11/2017
        ///   string filter contains the new password,  user control passed in as header information from page
        /// </remarks>
        [HttpPost, ActionName("PostNGLPassword")]
        public Models.Response PostNGLPassword([System.Web.Http.FromBody] DynamicsTMS365.Models.userNewPassword newP)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.OK;
            if (!authenticateController(ref response)) { return response; }

            try
            {

                if (newP != null)
                {

                }
                DAL.NGLSecurityDataProvider dalSecData = new DAL.NGLSecurityDataProvider(Parameters);
                string sMsg = "";
                bool bisPasswordStrong = dalSecData.validatePasswordStrength(newP.newPassword, ref sMsg);
                if (!bisPasswordStrong)
                {
                    response.StatusCode = HttpStatusCode.Unauthorized;
                    response.Errors = Utilities.getLocalizedMsg(sMsg);
                    return response;
                }

                //if (string.IsNullOrWhiteSpace(newP.newPassword) || newP.newPassword.Length < 6 || newP.newPassword.Length > 20)
                //{
                //    response.StatusCode = HttpStatusCode.Unauthorized;
                //    response.Errors = Utilities.getLocalizedMsg("E_InvalidNewPassword");
                //    return response;
                //}

                bool blnSuccess = dalSecData.tblUserSecuritySaveNewPassword(UserControl, newP.newPassword, newP.currentPassword);
                if (!blnSuccess)
                {
                    response.StatusCode = HttpStatusCode.NotModified;
                    response.Errors = Utilities.getLocalizedMsg("E_UpdatePasswordFailed");
                    return response;
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



        /// <summary>
        /// Generate a new password
        /// Save the password to the database
        /// Send the email
        /// Returns false if response.Errors was populated, else true.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="UserSecurityControl"></param>
        /// <param name="UserEmail"></param>
        /// <param name="UserName"></param>
        /// <param name="Parameters"></param>
        /// <remarks>
        /// Created By LVV on 4/4/18 for v-8.1 TMS 365
        /// Moved this logic from PostNGLSendPassword so that it could be used in multiple places
        /// Also called in LEUserController.CreateUser if the new user account is NGL Legacy
        /// </remarks>
        public static bool generateNewPasswordAndSendEmail(ref Models.Response response, int UserSecurityControl, string UserEmail, string UserName, DAL.WCFParameters Parameters)
        {
            DAL.NGLSecurityDataProvider oSec = new DAL.NGLSecurityDataProvider(Parameters);

            string sPassword = Utilities.GenerateRandomPassword(); //generate a new password

            if (oSec.tblUserSecuritySaveNewPassword(UserSecurityControl, sPassword))//save the password to the database
            {
                return sendPasswordResetEmail(ref response, UserEmail, sPassword, Parameters); //send the email
            }
            else
            {
                FaultExceptionEventArgs fault = new FaultExceptionEventArgs();
                response.StatusCode = HttpStatusCode.Unauthorized;
                List<string> DetailsList = new List<string>();
                DetailsList.Add(UserName);
                response.Errors = fault.formatMessage("", "E_NoUserSecurityForUser", DetailsList);
                return false;
            }
            //return true;
        }

        /// <summary>
        /// Save the password to the database
        /// Send the email if required
        /// Returns false if response.Errors was populated, else true.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="UserSecurityControl"></param>
        /// <param name="UserEmail"></param>
        /// <param name="UserName"></param>
        /// <param name="Parameters"></param>
        /// <remarks>
        /// Created By LVV on 12/17/18
        /// </remarks>
        public static bool savePasswordOptionalSendEmail(ref Models.Response response, bool blnSendEmail, int UserSecurityControl, string UserEmail, string UserName, string sPassword, DAL.WCFParameters Parameters)
        {
            DAL.NGLSecurityDataProvider oSec = new DAL.NGLSecurityDataProvider(Parameters);

            //save the password to the database
            if (oSec.tblUserSecuritySaveNewPassword(UserSecurityControl, sPassword))
            {
                //send the email if required
                if (blnSendEmail) { return sendPasswordResetEmail(ref response, UserEmail, sPassword, Parameters); }
            }
            else
            {
                FaultExceptionEventArgs fault = new FaultExceptionEventArgs();
                response.StatusCode = HttpStatusCode.Unauthorized;
                List<string> DetailsList = new List<string>();
                DetailsList.Add(UserName);
                response.Errors = fault.formatMessage("", "E_NoUserSecurityForUser", DetailsList);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Reads configuration settings for SmtpFromAddress, ResetPasswordEmailSubject, ResetPasswordEmailBody
        /// Sends the email
        /// Returns false if response.Errors was populated, else true.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="UserEmail"></param>
        /// <param name="sPassword"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public static bool sendPasswordResetEmail(ref Models.Response response, string UserEmail, string sPassword, DAL.WCFParameters Parameters)
        {
            DAL.NGLEmailData oMail = new DAL.NGLEmailData(Parameters);
            try
            {
                //read configuration settings for email from, ResetPasswordEmailSubject, ResetPasswordEmailBody
                string sFrom = System.Configuration.ConfigurationManager.AppSettings["ResetPasswordEmailAddress"];
                string sSubject = System.Configuration.ConfigurationManager.AppSettings["ResetPasswordEmailSubject"];
                string sBody = System.Configuration.ConfigurationManager.AppSettings["ResetPasswordEmailBody"].Replace(Utilities.newLine, Environment.NewLine);
                sBody += sPassword;
                oMail.GenerateEmail(sFrom, UserEmail, "", sSubject, sBody, "", "", "", "");
            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = new FaultExceptionEventArgs();
                if (ex.GetType() == typeof(FaultException<DAL.SqlFaultInfo>))
                {
                    string sMsg = "";
                    string sReason = "";
                    if (((FaultException<DAL.SqlFaultInfo>)ex).Detail != null)
                    {
                        sMsg = ((FaultException<DAL.SqlFaultInfo>)ex).Detail.Message;
                        sReason = ((FaultException<DAL.SqlFaultInfo>)ex).Reason.ToString();
                    }
                    if ((!string.IsNullOrWhiteSpace(sMsg) && sMsg == "E_SQLExceptionMSG") || (!string.IsNullOrWhiteSpace(sReason) && sReason == "E_SQLExceptionMSG"))
                    {
                        //cannot generate email please try again
                        //SendEmailError                            
                        response.StatusCode = HttpStatusCode.InternalServerError;
                        response.Errors = fault.formatMessage("", "SendEmailError", null);
                        return false;
                    }
                }
                fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return false;
            }
            return true;
        }


        public static DAL.Models.SSOResults GetNGLLegacySSOAForUser(string AuthUN, string AuthT)
        {
            try
            {
                //use default parameters
                DAL.NGLSecurityDataProvider dalSecData = new DAL.NGLSecurityDataProvider(Utilities.DALWCFParameters);
                return dalSecData.GetNGLLegacySSOAForUser(AuthUN, AuthT);
            }
            catch (Exception ex)
            {
                //do nothing 
                return null; //user not valid
            }
        }

        //NetGetSSOAccount

        public static DAL.Models.SSOResults NetGetSSOAccount(int UserControl)
        {
            try
            {
                //use default parameters
                DAL.NGLSecurityDataProvider dalSecData = new DAL.NGLSecurityDataProvider(Utilities.DALWCFParameters);
                return dalSecData.NetGetSSOAccount(UserControl);
            }
            catch (Exception ex)
            {
                //do nothing 
                return null; //user not valid
            }
        }


        /// <summary>
        /// Determines if the users must be re-directed to RateShoppingQ page where cost restrinctions are applied
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="dblCarrierCostUpchargeLimitVisibility"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.5.4.001 on 07/18/2023
        /// </remarks>
        public static bool isUserRateShopRestricted(string UserName, double dblCarrierCostUpchargeLimitVisibility)
        {
            DAL.NGLSecurityDataProvider dalSecData = new DAL.NGLSecurityDataProvider(Utilities.DALWCFParameters);

            return dalSecData.isUserRateShopRestricted(UserName, dblCarrierCostUpchargeLimitVisibility);
        }

        /// <summary>
        /// Determines if the user has permission to access the screen
        /// </summary>
        /// <param name="PageControl"></param>
        /// <param name="USC"></param>
        /// <param name="blnThrowException"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created By LVV on 6/1/20 for v-8.2.1.008 - Task #202005191648 Fix Procedure Level Security Deployment Issues
        /// </remarks>
        public static bool CanUserAccessScreen(int PageControl, int USC, bool blnThrowException = true)
        {
            DAL.NGLSecurityDataProvider dalSecData = new DAL.NGLSecurityDataProvider(Utilities.DALWCFParameters);

            return dalSecData.CanUserAccessScreen(PageControl, USC, blnThrowException);
        }
        /// <summary>
        /// Validate that the current license is valid
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.5.3.006 added Check Auth Code to validate Auth License
        /// </remarks>
        public static bool CheckAuthCode()
        {
            DAL.NGLSystemDataProvider dalSysData = new DAL.NGLSystemDataProvider(Utilities.DALWCFParameters);

            return dalSysData.CheckAuthCode();
        }

        //public static bool CanUserAccessScreen(int PageControl, int USC, bool blnThrowException = true)
        //{
        //    DAL.NGLSecurityDataProvider dalSecData = new DAL.NGLSecurityDataProvider(Utilities.DALWCFParameters);
        //    return dalSecData.CanUserAccessScreen(PageControl, USC, blnThrowException);
        //}

        #endregion

    }
}