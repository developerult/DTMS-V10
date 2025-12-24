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
    public class UserPageSettingController : NGLControllerBase
    {

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.UserPageSettingController";
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

        /// <summary>
        /// The parameter id is the PageControl
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, ActionName("GetPageSettingsForCurrentUser")]
        public Models.Response GetPageSettingsForCurrentUser(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLUserPageSettingData oUPS = new DAL.NGLUserPageSettingData(Parameters);
                LTS.tblUserPageSetting[] retVals = new LTS.tblUserPageSetting[] { };
                List<LTS.tblUserPageSetting> resVals = new List<LTS.tblUserPageSetting>();
                int count = 0;

                retVals = oUPS.GetPageSettingsForCurrentUser(id);
                if (retVals != null && retVals.Count() > 0)
                {
                    LTS.tblUserPageSetting userPS = new LTS.tblUserPageSetting();
                    userPS.UserPSControl = retVals[0].UserPSControl;
                    userPS.UserPSPageControl = retVals[0].UserPSPageControl;
                    userPS.UserPSUserSecurityControl = retVals[0].UserPSUserSecurityControl;
                    userPS.UserPSName = retVals[0].UserPSName;
                    userPS.UserPSMetaData = retVals[0].UserPSMetaData;
                    resVals.Add(userPS);
                    count = 1;
                }
                else
                {
                    LTS.tblUserPageSetting userPS = new LTS.tblUserPageSetting();
                    userPS.UserPSControl = 0;
                    resVals.Add(userPS);
                    count = 0;
                }

                response = new Models.Response(resVals.ToArray(), count);
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
        /// Uses the LTS object directly to insert or update a record in tblUserPageSetting
        /// We can do this because this table does not have an Updated field
        /// </summary>
        /// <param name="ups"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.2 on 09/13/2018
        ///   fixed bug where retVals LTS.tblUserPageSetting[1]; was set to an empty array causing an index out of range error
        ///   when setting retVals[0]  = oRecord
        /// </remarks>
        [HttpPost, ActionName("InsertOrUpdateCurrentUserPageSetting")]
        public Models.Response InsertOrUpdateCurrentUserPageSetting([System.Web.Http.FromBody]LTS.tblUserPageSetting ups)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLUserPageSettingData oUPS = new DAL.NGLUserPageSettingData(Parameters);
                //Modified by RHR for v-8.2 on 09/13/2018
                LTS.tblUserPageSetting[] retVals = new LTS.tblUserPageSetting[1];
                int count = 0;
               
                LTS.tblUserPageSetting oRecord = oUPS.InsertOrUpdateCurrentUserPageSetting(ups);

                if (oRecord != null)
                {
                    LTS.tblUserPageSetting userPS = new LTS.tblUserPageSetting();
                    userPS.UserPSControl = oRecord.UserPSControl;
                    userPS.UserPSPageControl = oRecord.UserPSPageControl;
                    userPS.UserPSUserSecurityControl = oRecord.UserPSUserSecurityControl;
                    userPS.UserPSName = oRecord.UserPSName;
                    userPS.UserPSMetaData = oRecord.UserPSMetaData;
                    
                    count = 1;
                    retVals[0] = userPS;
                }
                response = new Models.Response(retVals, count);
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
        /// Parameter id is the UserPSControl
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete, ActionName("DeleteUserPageSetting")]
        public Models.Response DeleteUserPageSetting(int id)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLUserPageSettingData oUPS = new DAL.NGLUserPageSettingData(Parameters);
                oUPS.DeleteUserPageSetting(id);
                //The only way the sub will fail is if an exception is thrown which will be handled below, so just return a default of true
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


        [HttpPost, ActionName("ResetSpecificUserPageSetting")]
        public Models.Response ResetSpecificUserPageSetting([FromBody]LTS.tblUserPageSetting ups)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLUserPageSettingData oUPS = new DAL.NGLUserPageSettingData(Parameters);
                oUPS.DeleteUserPageSettingFiltered(Parameters.UserControl, ups.UserPSPageControl, ups.UserPSName);
                bool[] records = new bool[] { true };
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

        #endregion

        #region " Public Methods"  

        public LTS.tblUserPageSetting[] GetPageSettingsForCurrentUserByName(int id, string name)
        {
            LTS.tblUserPageSetting[] retVals = new LTS.tblUserPageSetting[] { };
            try
            {
                DAL.NGLUserPageSettingData oUPS = new DAL.NGLUserPageSettingData(Parameters);   
                retVals = oUPS.GetPageSettingsForCurrentUser(id, name);               
            }
            catch (Exception ex)
            {
                //for now we just rethrow the exception,  
                //additional logic may be added to handle specific types of exeptions
                throw;
            }
            return retVals;
        }

        public bool SaveCurrentUserPageSetting(LTS.tblUserPageSetting ups)
        {
            
            try
            {
                DAL.NGLUserPageSettingData oUPS = new DAL.NGLUserPageSettingData(Parameters);
                return oUPS.SaveCurrentUserPageSetting(ups);
               
            }
            catch (Exception ex)
            {
                //for now we just rethrow the exception,  
                //additional logic may be added to handle specific types of exeptions
                throw;
            }
           
        }


        #endregion
    }
}