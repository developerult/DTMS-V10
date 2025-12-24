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
using DTran = Ngl.Core.Utility.DataTransformation;
using DModel = Ngl.FreightMaster.Data.Models;
//Added By RHR on 06/29/2022 for v-8.5.3.003 - Manage Serice Tokens use for carrier reports and other Utiliities

namespace DynamicsTMS365.Controllers
{
    public class ServiceTokenController : NGLControllerBase
    {
        #region " Constructors "

        /// <summary> Initializes the Page property by calling the base class constructor </summary>
        public ServiceTokenController() : base(Utilities.PageEnum.NGLSystemMaint) { }

        #endregion

        #region " Properties"

        /// <summary> This property is used for logging and error tracking. </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.ServiceTokenController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        private DAL.NGLtblServiceTokenData _TokenDAL;
        private DAL.NGLtblServiceTokenData oTokenDAL
        {
            get { 
                if (_TokenDAL == null) 
                { 
                    _TokenDAL = new DAL.NGLtblServiceTokenData(Utilities.DALWCFParameters);  
                }
                return _TokenDAL;
            }
            set { _TokenDAL = value; }
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
                int count = 0;
                
                DTO.tblServiceToken oDTOToken = oTokenDAL.GettblServiceTokenFiltered(id);
                Models.tblServiceToken[] oRecords = new Models.tblServiceToken[] { Models.tblServiceToken.selectModelData(oDTOToken) };
               
                if (oRecords?.Count() > 0) { count = oRecords.Count(); }
                response = new Models.Response(oRecords, count);
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
        [HttpGet, ActionName("GetRecords")]
        public Models.Response GetRecords()
        {
            return GetRecords("");
        }

        //Not Currently Used
        [HttpGet, ActionName("GetRecords")]
        public Models.Response GetRecords(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateFilter(ref response, filter)) { return response; } //Verfiy that the filters object is not null
            return GetAllRecords(filter);
        }

        //Not Currently Used
        [HttpGet, ActionName("GetAllRecords")]
        public Models.Response GetAllRecords(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                ////if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter, "FormConfigGridFltr"); } //save the page filter for the next time the page loads
                ////DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DModel.AllFilters>(filter);
                ////int RecordCount = 0;
                ////int count = 0;
                ////DModel.SecurityTypeConfig[] oData = new DModel.SecurityTypeConfig[] { };
                ////oData = NGLSecurityData.GetFormSecTypeXTabData(ref RecordCount, f);
                ////if (oData?.Count() > 0)
                ////{
                ////    count = oData.Count();
                ////    if (RecordCount > count) { count = RecordCount; }
                ////}
                ////response = new Models.Response(oData, count);
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
        public Models.Response Post([FromBody] Models.tblServiceToken data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                LTS.tblServiceToken oLTSToken = oTokenDAL.InsertOrUpdatetblServiceToken(Models.tblServiceToken.selectLTSData(data));
                Models.tblServiceToken oData = Models.tblServiceToken.selectModelData(oLTSToken);
                Models.tblServiceToken[] oRecords = new Models.tblServiceToken[1];
                oRecords[0] = oData;
                int iCount = oRecords.Count();
                response = new Models.Response(oRecords, iCount);
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
                bool[] oRecords = new bool[] { oTokenDAL.DeleteToken(id) };
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