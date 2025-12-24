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
    /// <summary>
    /// Creates an Reads User configuration settings for the carrier selection option 
    /// when adding a new fuel addendum to a tariff from an existing carrier's fuel
    /// addendum.  this controller actually save the data as a JSON string to the
    /// tblUserPageSettings.UserPSMetaData field.
    /// 
    /// Use this controller as a template for saving option control settings when a special 
    /// table is not required.  design the NGLWorkflowSectionCtrl widget to bind
    /// data fields to the vGenericPageSettingTemplate and map data to one or more of the tempalte
    /// columns.  In the controller determine how to read or save the data, typically this data
    /// is converted to a JSON string and stored in tblUserPageSettings.UserPSMetaData.
    /// NOTE:  this is only used by the Tariff Fuel pages
    /// </summary>
    public class FuelAdTemplOptionsController : NGLControllerBase
    {
        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.2 on 08/29/2018 initializes the Page property by calling the base class constructor
        /// </summary>
        public FuelAdTemplOptionsController()
                : base(Utilities.PageEnum.TariffFuel)
	     {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.FuelAdTemplOptionsController";
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

        [HttpGet, ActionName("Get")]
        public Models.Response Get(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {               
                //id is not used we always return the same record
                response = getFuelAdTemplOptions();
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
        /// Gets All the Child BookSpotRateData Records filtered by BookControl passed in as  id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.2 on 10/17/2018   
        /// </remarks>
        [HttpGet, ActionName("GetByParent")]
        public Models.Response GetByParent(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                //id is not used we always return the same record
                response = getFuelAdTemplOptions();
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
                //filter is not used we always return the same record
                response = getFuelAdTemplOptions();
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
        public Models.Response Post([System.Web.Http.FromBody]Models.vGenericPageSettingTemplate data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                
                bool[] oRecords = new bool[1] { false };
                string sSetting = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(data);
                string retMsg = "";
                oRecords[0] = savePageSetting(sSetting, ref retMsg, "FuelAdTemplOptions");            

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
                bool[] oRecords = new bool[1] { false };            
                string retMsg = "";
                oRecords[0] = savePageSetting("", ref retMsg, "FuelAdTemplOptions");

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


        #region "Private Functions"
        private Models.Response getFuelAdTemplOptions()
        {
            Models.vGenericPageSettingTemplate oData = new Models.vGenericPageSettingTemplate();
   
            int count = 1;
            Models.vGenericPageSettingTemplate[] oRet = new Models.vGenericPageSettingTemplate[] { new Models.vGenericPageSettingTemplate()  };
            DAL.NGLUserPageSettingData oUPS = new DAL.NGLUserPageSettingData(Parameters);
            LTS.tblUserPageSetting[] wdgtUPS = oUPS.GetPageSettingsForCurrentUser((int)Page, "FuelAdTemplOptions");
            if (wdgtUPS != null && wdgtUPS.Count() > 0)
            {
                oData = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Models.vGenericPageSettingTemplate>(wdgtUPS[0].UserPSMetaData);
            }
          
            
            if (oData != null) { oRet[0] = oData; }
            return new Models.Response(oRet, count);

        }


        #endregion


    }
}