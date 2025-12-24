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
    /// Replaces TempTypeController
    /// </summary>
    /// <remarks>
    /// Created by RHR for v-8.5.2.007 on 04/24/2023
    /// used to fix bug where vTempType was never deployed and 
    /// source code was lost
    /// </remarks>
    public class TempType365Controller : NGLControllerBase
    {
        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.5.2.007 on 04/24/2023 initializes the Page property by calling the base class constructor
        /// </summary>
        public TempType365Controller()
                : base(Utilities.PageEnum.StaticListMaint)
        {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.TempType365Controller";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"
        // moved to Model class

        #endregion

        #region " REST Services"
        //    POST 	/API/objectcontroller{data
        //}  : Create a new object or Update a the current object if the control number exists
        /// 
        /// GET 	/API/objectcontroller/{id} : Get the object information where the control number = "id"
        /// GetRecord: /API/objectcontroller/{data} used to read using a string primary key
        /// PUT 	/API/objectcontroller/{data} : Update the object information stored in data
        /// DELETE	/API/objectcontroller/{id} : Delete object where the control number = "id"
        /// DeleteRecord: /API/objectcontroller/{data} used to delete using a string primary key

        [HttpGet, ActionName("Get")]
        public Models.Response Get(int id)
        {
            //string Value = null;
            //Note: The id must always match a CarrTarControl associated with the select tariff 
            //the system looks up the last saved tariff pk for this user 
            //an invalid parent key Error is returned if the data does not match
            //If id is zero and no records are found the client should configure the page to add a new record.

            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                //int RecordCount = 0;
                int count = 0;

                //DAL.NGLTempTypeData oDAL = new DAL.NGLTempTypeData(Parameters);
                //LTS.vTempType365 oData = new LTS.vTempType365 { };
                //oData = oDAL.GetTempTypebyFiltered(id);
                //Models.vTempType365[] records = new Models.vTempType365[1];
                //if (oData != null)
                //{
                //    count = 1;
                //    records[0] = Models.vTempType365.selectModelData(oData);
                //}
                response = new Models.Response(new Models.vTempType365[1], count);
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

        [HttpGet, ActionName("GetRecord")]
        public Models.Response GetRecord(string filter)
        {
            //string Value = null;
            //Note: The id must always match a CarrTarControl associated with the select tariff 
            //the system looks up the last saved tariff pk for this user 
            //an invalid parent key Error is returned if the data does not match
            //If id is zero and no records are found the client should configure the page to add a new record.

            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                //int RecordCount = 0;
                int count = 0;

                DAL.NGLTempTypeData oDAL = new DAL.NGLTempTypeData(Parameters);
                LTS.vTempType365 oData = new LTS.vTempType365 { };
                string sKey = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<string>(filter);
                oData = oDAL.GetTempTypebyFiltered(sKey);
                Models.vTempType365[] records = new Models.vTempType365[1];
                if (oData != null)
                {
                    count = 1;
                    records[0] = Models.vTempType365.selectModelData(oData);
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

                DAL.NGLTempTypeData oDAL = new DAL.NGLTempTypeData(Parameters);
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);

                //save the page filter for the next time the page loads
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter); }
                //f.ParentControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.StaticListMaint);
                Models.vTempType365[] records = new Models.vTempType365[] { };
                LTS.vTempType365[] oData = new LTS.vTempType365[] { };
                oData = oDAL.GetTempTypeFilter365(f, ref RecordCount);
                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                    records = (from e in oData select Models.vTempType365.selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                response = new Models.Response(oData, count);
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
        public Models.Response Post([System.Web.Http.FromBody] Models.vTempType365 data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLTempTypeData oDAL = new DAL.NGLTempTypeData(Parameters);
                LTS.TempType oTempType = Models.vTempType365.selectLTSTableData(data);
                bool blnRet = oDAL.SaveOrCreateTempType(oTempType);
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


        [HttpDelete, ActionName("DELETE")]
        public Models.Response DELETE(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            response.ErrTitle = "Not Supported";
            response.Errors = "Cannot use this method to delete Temp Type records. Contact NGL Support.";
            return response;           
        }


        [HttpGet, ActionName("DeleteRecord")]
        public Models.Response DeleteRecord(string filter)
        {
       
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLTempTypeData oDAL = new DAL.NGLTempTypeData(Parameters);
                string sKey = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<string>(filter);
                bool blnRet = oDAL.DeleteTempType365(sKey);
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


        #region " public methods"


        #endregion
    }
}