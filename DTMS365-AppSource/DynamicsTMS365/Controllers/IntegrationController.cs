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
    public class IntegrationController : NGLControllerBase
    {
        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.5.4.004 on 11/13/2023
        /// initializes the Page property by calling the base class constructor
        /// </summary>
        public IntegrationController()
                : base(Utilities.PageEnum.ERPIntegration)
        {
        }

        #endregion
        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.IntegrationController";
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
        /// Gets one Integration record using the IntegrationControl passed in as id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.5.4.004 on 11/13/2023
        /// </remarks>
        [HttpGet, ActionName("Get")]
        public Models.Response Get(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                if(id == 0) { id = this.readPagePrimaryKey(); }
                DAL.NGLIntegrationData oDAL = new DAL.NGLIntegrationData(Parameters);
                Models.Integration[] records = new Models.Integration[] { };
                LTS.vIntegration oData =  oDAL.GetvIntegrationFiltered(id);
                records = new Models.Integration[1] { Models.Integration.selectModelData(oData) };
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

        /// <summary>
        /// Gets All the Child Integration Records filtered by ERPSettingControl passed in as id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.5.4.004 on 11/13/2023
        /// </remarks>
        [HttpGet, ActionName("GetByParent")]
        public Models.Response GetByParent(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int RecordCount = 0;
                int count = 0;
                if (id == 0) { id = readPagePrimaryKey(Parameters, Utilities.PageEnum.ERPSettings); }

                DAL.NGLIntegrationData oDAL = new DAL.NGLIntegrationData(Parameters);
                Models.Integration[] records = new Models.Integration[] { };
                LTS.vIntegration[] oData = oDAL.GetvIntegrationsFiltered(id);
                

                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                    records = (from e in oData select Models.Integration.selectModelData(e)).ToArray();
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


        /// <summary>
        /// Insert or update Integration data into tblLegalEntityCompCarrier table
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.5.4.004 on 11/13/2023
        /// </remarks>
        [HttpPost, ActionName("Post")]
        public Models.Response Post([System.Web.Http.FromBody] Models.Integration data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLIntegrationData oDAL = new DAL.NGLIntegrationData(Parameters);
                DTO.Integration oChanges = Models.Integration.selectDTOData(data);
                bool blnRet = false;
                if (oChanges.IntegrationControl == 0)
                {
                    if (oChanges.ERPSettingControl == 0)
                    {
                        oChanges.ERPSettingControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.ERPSettings); 
                    }
                    DTO.Integration oData = (DTO.Integration)oDAL.CreateRecord(oChanges);
                    if (oData != null && oData.IntegrationControl != 0)
                    {
                        blnRet = true;
                    }
                }
                else
                {
                    DTO.Integration oData = (DTO.Integration)oDAL.UpdateRecord(oChanges);
                    if (oData != null && oData.IntegrationControl != 0)
                    {
                        blnRet = true;
                    }
                }
                bool[] oRecords = new bool[1];

                oRecords[0] = true;

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

        [HttpDelete, ActionName("Delete")]
        public Models.Response Delete(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
               
                DAL.NGLIntegrationData oDAL = new DAL.NGLIntegrationData(Parameters);
                bool blnRet = oDAL.DeleteIntegration(id);
                bool[] oRecords = new bool[1];
                oRecords[0] = blnRet;
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
    }
}