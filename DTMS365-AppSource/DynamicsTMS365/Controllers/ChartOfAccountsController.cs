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
    public class ChartOfAccountController : NGLControllerBase
    {
        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.5.2.007 on 04/18/2023 initializes the Page property by calling the base class constructor
        /// </summary>
        public ChartOfAccountController()
                : base(Utilities.PageEnum.ChartOfAccounts)
        {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.ChartOfAccountController";
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
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                DAL.NGLChartOfAccountData oDAL = new DAL.NGLChartOfAccountData(Parameters);
                Models.ChartOfAccount[] records = new Models.ChartOfAccount[1];
                DTO.ChartOfAccount oData = new DTO.ChartOfAccount();
                oData = oDAL.GetChartOfAccountFiltered(id);
                if (oData != null)
                {
                    count = 1;
                    records[0] = Models.ChartOfAccount.selectModelData(oData);
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
                int RecordCount = 0;
                int count = 0;
                //save the page filter for the next time the page loads
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter); }
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                DAL.NGLChartOfAccountData oDAL = new DAL.NGLChartOfAccountData(Parameters);
                Models.ChartOfAccount[] records = new Models.ChartOfAccount[] { };
                DTO.ChartOfAccount[] oData = new DTO.ChartOfAccount[] { };
                oData = oDAL.GetChartOfAccounts(f, ref RecordCount);
                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                    records = (from e in oData select Models.ChartOfAccount.selectModelData(e)).ToArray();
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



        [HttpPost, ActionName("Post")]
        public Models.Response Post([System.Web.Http.FromBody] Models.ChartOfAccount data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                DAL.NGLChartOfAccountData oDAL = new DAL.NGLChartOfAccountData(Parameters);
                DTO.ChartOfAccount oChanges = Models.ChartOfAccount.selectDTOData(data);
                DTO.ChartOfAccount oUpdated = new DTO.ChartOfAccount();
                if (oChanges.ID == 0)
                {
                    oUpdated = (DTO.ChartOfAccount)oDAL.CreateRecord(oChanges);
                }
                else
                {
                    oUpdated = (DTO.ChartOfAccount)oDAL.UpdateRecord(oChanges);
                }
                bool[] oRecords = new bool[1];
                if (oUpdated != null)
                {
                    count = 1;
                    oRecords[0] = true;
                } else
                {
                    oRecords[0] = false;
                }
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




        [HttpDelete, ActionName("DELETE")]
        public Models.Response DELETE(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLChartOfAccountData oDAL = new DAL.NGLChartOfAccountData(Parameters);
                bool blnRet = oDAL.DeleteChartOfAccount(id);
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