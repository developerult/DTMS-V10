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
using NGL.UTC.Library;

namespace DynamicsTMS365.Controllers
{
    public class UsersController : NGLControllerBase
    {

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.UsersController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"


        /// <summary>
        /// map LTS data to Model
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>d
        /// <remarks>
        /// Modified by RHR for v-8.5.0.002 on 12/2/2021 fixed bug  UserWorkExt mapping to UserPhoneWorkExt
        /// </remarks>
        private Models.User selectModelData(LTS.tblUserSecurity d)
        {
            Models.User oRecord = new Models.User();
            List<string> skipObjs = new List<string> { "LEAControl", "UseMicrosoftAccount", "AllowNGLAPI", "AccountGroup","UserPhoneWorkExt","UserWorkExt" };
            string sMsg = "";
            oRecord = (Models.User)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(oRecord, d, skipObjs, ref sMsg);
            oRecord.UserPhoneWorkExt = d.UserWorkExt;
            return oRecord;
        }

        /// <summary>
        ///  map DTO data to Model
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>d
        /// <remarks>
        /// Modified by RHR for v-8.5.0.002 on 12/2/2021 fixed bug  UserWorkExt mapping to UserPhoneWorkExt
        /// </remarks>
        private Models.User selectModelData(DTO.tblUserSecurity d)
        {
            Models.User oRecord = new Models.User();
            List<string> skipObjs = new List<string> { "LEAControl", "UseMicrosoftAccount", "AllowNGLAPI", "AccountGroup", "UserPhoneWorkExt", "UserWorkExt" };
            string sMsg = "";
            oRecord = (Models.User)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(oRecord, d, skipObjs, ref sMsg);
            oRecord.UserPhoneWorkExt = d.UserWorkExt;
            return oRecord;
        }

        /// <summary>
        ///  map Model data to DTO
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>d
        /// <remarks>
        /// Modified by RHR for v-8.5.0.002 on 12/2/2021 fixed bug  UserWorkExt mapping to UserPhoneWorkExt
        /// </remarks>
        private DTO.tblUserSecurity selectDTOData(Models.User d)
        {
            DTO.tblUserSecurity oRecord = new DTO.tblUserSecurity();
            List<string> skipObjs = new List<string> { "LEAControl", "UseMicrosoftAccount", "AllowNGLAPI", "AccountGroup", "UserPhoneWorkExt", "UserWorkExt" };
            string sMsg = "";
            oRecord = (DTO.tblUserSecurity)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(oRecord, d, skipObjs, ref sMsg);
            oRecord.UserWorkExt  = d.UserPhoneWorkExt;
            return oRecord;
        }

        #endregion

        #region " REST Services"

        /// <summary>
        /// Pass in id = 0 to get all users in the Free Trial User Group
        /// Pass in id = 1 to get all users in the Free Trial Pending User Group
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, ActionName("GetFreeTrialUsers")]
        public Models.Response GetFreeTrialUsers(int id)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            try
            {
                LTS.tblUserSecurity[] users = new LTS.tblUserSecurity[] { };
                Models.User[] retVals = new Models.User[] { };
                DAL.NGLSecurityDataProvider oSec = new DAL.NGLSecurityDataProvider(Parameters);
                int RecordCount = 0;
                int count = 0;
                // get the take and skip parameters int skip = request["skip"] == null ? 0 :
                int skip = request["skip"] == null ? 0 : int.Parse(request["skip"]);
                int take = request["take"] == null ? 500 : int.Parse(request["take"]);
                string sortExpression = "";
                string filterWhere = request["filter[filters][0][value]"];
                int CatControl = 5;
                int pageSize = 0;

                if (id == 0)
                {
                    CatControl = 5;
                    sortExpression = "UserName Asc";
                    pageSize = 0; //**  TODO LVV ** This is because right now this method is only being called in 2 places - one by a dropdown list and one by a grid. I need to make this more general. This one is the dropdown list
                }
                if (id == 1)
                {
                    CatControl = 6;
                    sortExpression = "UserStartFreeTrial Desc"; 
                    pageSize = 500; //**  TODO LVV ** This is because right now this method is only being called in 2 places - one by a dropdown list and one by a grid. I need to make this more general. This one is the grid
                }

                users = oSec.GetFreeTrialUsers(ref RecordCount, CatControl, filterWhere, sortExpression, 1, pageSize, skip, take);
                if (users != null && users.Count() > 0)
                {
                    count = users.Length;
                    retVals = (from e in users
                                orderby e.UserName
                                select selectModelData(e)).ToArray();
                }

                response = new Models.Response(retVals, count);
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

        [HttpGet, ActionName("GetUserSettings")]
        public Models.Response GetUserSettings()
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            try
            {
                DTO.tblUserSecurity user = new DTO.tblUserSecurity();
                Models.User retVal = new Models.User();
                DAL.NGLSecurityDataProvider oSec = new DAL.NGLSecurityDataProvider(Parameters);
                int count = 0;

                user = oSec.GettblUserSecurity(Parameters.UserControl);
                if (user != null)
                {
                    count = 1;
                    retVal = selectModelData(user);
                    Array d = new Models.User[1] { retVal };
                    response = new Models.Response(d, count);
                }
                else
                {
                    response.StatusCode = HttpStatusCode.NoContent;
                    response.Errors = "No User Record found";
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

        [HttpGet, ActionName("GetCultureInfo")]
        public Models.Response GetCultureInfo()
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            try
            {
                var cultureInfoList = clsApplication.GenerateCultureInfoList();
                if (cultureInfoList != null && cultureInfoList.Count > 0)
                {
                    response.Data = cultureInfoList.ToArray();
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.StatusCode = HttpStatusCode.NoContent;
                    response.Errors = "No record found";
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
            return response;
        }

        [HttpGet, ActionName("GetTimeZoneInfo")]
        public Models.Response GetTimeZoneInfo()
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            try
            {
                var timeZoneInfoList = clsApplication.GetTimeZoneList();
                if (timeZoneInfoList != null && timeZoneInfoList.Count > 0)
                {
                    response.Data = timeZoneInfoList.ToArray();
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.StatusCode = HttpStatusCode.NoContent;
                    response.Errors = "No record found";
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
            return response;
        }

        [HttpPost, ActionName("SaveUserSettings")]
        public Models.Response SaveUserSettings([System.Web.Http.FromBody]Models.User user)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            try
            {
                Models.User retVal = new Models.User();
                int count = 0;
                DAL.NGLSecurityDataProvider oSec = new DAL.NGLSecurityDataProvider(Parameters);

                DTO.tblUserSecurity u = oSec.GettblUserSecurity(Parameters.UserControl);

                u.UserEmail = user.UserEmail;
                u.UserFriendlyName = user.UserFriendlyName;
                u.UserFirstName = user.UserFirstName;
                u.UserLastName = user.UserLastName;
                u.UserTitle = user.UserTitle;
                u.UserDepartment = user.UserDepartment;
                u.UserPhoneCell = user.UserPhoneCell;
                u.UserPhoneHome = user.UserPhoneHome;
                u.UserPhoneWork = user.UserPhoneWork;
                u.UserWorkExt = user.UserPhoneWorkExt;
                u.UserTheme365 = user.UserTheme365;
                u.UserCultureInfo = user.UserCultureInfo;
                u.UserTimeZone = user.UserTimeZone;

                DTO.tblUserSecurity dtoUS = oSec.UpdatetblUserSecurity(u);

                if (dtoUS != null)
                {
                    count = 1;
                    retVal = selectModelData(dtoUS);

                    if (Utilities.GlobalSSOResultsByUser.ContainsKey(Parameters.UserControl))
                    {
                        Utilities.GlobalSSOResultsByUser[Parameters.UserControl].UserTheme365 = retVal.UserTheme365;
                    }

                    Array d = new Models.User[1] { retVal };
                    response = new Models.Response(d, count);
                }
                else
                {
                    response.StatusCode = HttpStatusCode.NoContent;
                    response.Errors = "There was a problem saving the User Settings";
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
            return response;
        }


        [HttpPost, ActionName("UpdateUserTheme365")]
        public Models.Response UpdateUserTheme365([System.Web.Http.FromBody] Models.User user)
        {
            // create a response message to send back  tblUserSecurityChangeUserTheme365
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            try
            {
                bool blnRet = UsersController.ChangeUserTheme365(Parameters, Parameters.UserControl, user.UserTheme365);
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

        [HttpPost, ActionName("AcceptLegacyFTRequest")]
        public Models.Response AcceptLegacyFTRequest([System.Web.Http.FromBody]Models.User user)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            try
            {
                DAL.NGLSecurityDataProvider oSec = new DAL.NGLSecurityDataProvider(Parameters);
                string strMsg = "";

                bool blnRet = oSec.AcceptLegacyFTRequest(ref strMsg, user.UserSecurityControl);

                if (blnRet)
                {
                    Array d = new bool[1] { blnRet };
                    response = new Models.Response(d, 1);


                    string sFrom = System.Configuration.ConfigurationManager.AppSettings["SmtpFromAddress"];
                    NGLEmailData.GenerateEmail(sFrom, user.UserEmail, "", "Free Trial Request Accepted", "Your request for a free trial account has been accepted. You may now log in to use the software.", "", "", "", "");
                }
                else
                {
                    //** TODO LVV ** Localize these messages. Also, figure out better way to do this
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Errors = "Unexpected error";
                    if (!string.IsNullOrWhiteSpace(strMsg))
                    {
                        response.StatusCode = HttpStatusCode.Forbidden;
                        response.Errors = strMsg;
                    }                  
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
            return response;
        }

        [HttpPost, ActionName("ExtendFreeTrial")]
        public Models.Response ExtendFreeTrial([System.Web.Http.FromBody]Models.User user)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            try
            {
                Models.User retVal = new Models.User();
                DAL.NGLSecurityDataProvider oSec = new DAL.NGLSecurityDataProvider(Parameters);

                //Get the record for the user selected
                DTO.tblUserSecurity dtoUser = oSec.GettblUserSecurity(user.UserSecurityControl);

                //Get the date from the UI but use the time from the database
                DateTime dt = user.UserEndFreeTrial.Value.Date;
                TimeSpan ts = dtoUser.UserEndFreeTrial.Value.TimeOfDay;
                DateTime newDate = dt + ts;

                dtoUser.UserEndFreeTrial = newDate;
                
                DTO.tblUserSecurity dtoUS = oSec.UpdatetblUserSecurity(dtoUser);

                //** TODO LVV ** HERE WE HAVE TO CHECK IF THE RATESHOPPAGE FOR USER BECUASE MAYBE FT HAD EXPIRED
                //Need to make sure has access to RS page in role center

                if (dtoUS != null)
                {
                    Array d = new bool[1] { true };
                    response = new Models.Response(d, 1);
                }
                else
                {
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Errors = "There was a problem extending the Free Trial for this user.";
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
            return response;
        }



        #endregion

        #region "Static Methods"

        /// <summary>
        /// Updates the user theme for the user
        /// </summary>
        /// <param name="Parameters"></param>
        /// <param name="iUserControl"></param>
        /// <param name="sTheme"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.5.4.001 05/24/2034 
        /// updates the users theme for the provided user control
        /// </remarks>
        public static bool ChangeUserTheme365(DAL.WCFParameters Parameters, int iUserControl, string sTheme)
        {
            DAL.NGLSecurityDataProvider dalSecData = new DAL.NGLSecurityDataProvider(Parameters);

            return dalSecData.tblUserSecurityChangeUserTheme365(iUserControl, sTheme);
        }
       

        #endregion
    }
}