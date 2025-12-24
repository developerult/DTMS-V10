using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL = Ngl.FreightMaster.Data;
using LTS = Ngl.FreightMaster.Data.LTS;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using BLL = NGL.FM.BLL;
using DTran = Ngl.Core.Utility.DataTransformation;

namespace DynamicsTMS365.Controllers
{
    public class ERPSettingsController : NGLControllerBase
    {
        #region " Constructors "

        public ERPSettingsController()
            : base(Utilities.PageEnum.ERPSettings)
        {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.ERPSettingsController";
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
        //    POST 	/API/objectcontroller{data
        //}  : Create a new object or Update a the current object if the control number exists
        /// 
        /// GET 	/API/objectcontroller/{id} : Get the object information where the control number = "id"
        /// PUT 	/API/objectcontroller/{data} : Update the object information stored in data
        /// DELETE	/API/objectcontroller/{id} : Delete object where the control number = "id"
        /// 

        [HttpGet, ActionName("Get")]
        public Models.Response Get(int id)
        {
            //Note: The id must always match a CarrTarEquipControl associated with the select tariff using CarrTarEquipCarrTarControl
            //the system looks up the last saved tariff pk for this user and return the first Service record found
            //an invalid parent key Error is returned if the data does not match
            //If id is zero and no records are found the client should configure the page to add a new service record.

            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            try
            {
              
                if (id == 0) { id = this.readPagePrimaryKey(); }
               
                DAL.NGLERPSettingData oDAL = new DAL.NGLERPSettingData(Parameters);
                Models.ERPSetting[] records = new Models.ERPSetting[] { };
                LTS.vERPSetting oData = new LTS.vERPSetting();
                oData = oDAL.GetvERPSettingFiltered(id);
                records = new Models.ERPSetting[1] { Models.ERPSetting.selectModelData(oData) }; 
                response = new Models.Response(records, 1);
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
            // create a response message to send back
            // Note: Filteres are not supported so they are ignored for ERPSettings
            //       Users are restricted by Legal Entity 
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            try
            {
                int RecordCount = 0;
                int count = 0;
                int iLEControl = this.Parameters.UserLEControl;

                DAL.NGLERPSettingData oDAL = new DAL.NGLERPSettingData(Parameters);
                Models.ERPSetting[] records = new Models.ERPSetting[] { };
                LTS.vERPSetting[] oData = oDAL.GetvERPSettingsFiltered(iLEControl);

                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                    records = (from e in oData select Models.ERPSetting.selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                response = new Models.Response(records, count);
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

        [HttpPost, ActionName("Post")]
        public Models.Response Post([System.Web.Http.FromBody] Models.ERPSetting data)
        {
            //create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLERPSettingData oDAL = new DAL.NGLERPSettingData(Parameters);
                DTO.ERPSetting oChanges = Models.ERPSetting.selectDTOData(data);
                bool blnRet = false;
                if (oChanges.ERPSettingControl == 0) {
                    DTO.ERPSetting oData = (DTO.ERPSetting)oDAL.CreateRecord(oChanges);
                    if (oData != null && oData.ERPSettingControl != 0)
                    {
                        blnRet = true;
                    }
                }
                else
                {
                    DTO.ERPSetting oData = (DTO.ERPSetting)oDAL.UpdateRecord(oChanges);
                    if (oData != null && oData.ERPSettingControl != 0)
                    {
                        blnRet = true;
                    }
                }
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


        [HttpDelete, ActionName("DELETE")]
        public Models.Response DELETE(int id)
        {
            //create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLERPSettingData oDAL = new DAL.NGLERPSettingData(Parameters);
                bool blnRet = oDAL.DeleteERPSetting(id);            
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


        #endregion


        #region " public methods"


        #endregion

    }
}