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
using DModel = Ngl.FreightMaster.Data.Models;

namespace DynamicsTMS365.Controllers
{
    public class MasterCarriersController : NGLControllerBase
    {
        #region " Constructors "

        /// <summary>Initializes the Page property by calling the base class constructor</summary>
        public MasterCarriersController() : base(Utilities.PageEnum.MasterCarrier) { }

        #endregion

        #region " Properties"

        /// <summary>This property is used for logging and error tracking</summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.MasterCarriersController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        private Models.CarrierModel selectModelData(LTS.Carrier d)
        {
            Models.CarrierModel modelRecord = new Models.CarrierModel();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "CarrierUpdated" };
                string sMsg = "";
                modelRecord = (Models.CarrierModel)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.CarrierUpdated.ToArray()); }
            }
            return modelRecord;
        }

        public static DTO.Carrier selectDTOData(Models.CarrierModel d)
        {
            DTO.Carrier dtoRecord = new DTO.Carrier();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "CarrierUpdated" };
                string sMsg = "";
                dtoRecord = (DTO.Carrier)DTran.CopyMatchingFields(dtoRecord, d, skipObjs, ref sMsg);
                if (dtoRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    dtoRecord.CarrierUpdated = bupdated == null ? new byte[0] : bupdated;
                }
            }
            return dtoRecord;
        }

        public static LTS.Carrier selectLTSData(Models.CarrierModel d)
        {
            LTS.Carrier ltsRecord = new LTS.Carrier();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "CarrierUpdated", "CarrierCont", "_CarrierCont" };
                string sMsg = "";
                ltsRecord = (LTS.Carrier)DTran.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.CarrierUpdated = bupdated == null ? new byte[0] : bupdated;
                }
            }
            return ltsRecord;
        }

        #endregion

        #region " REST Services"
        /// POST 	/API/objectcontroller{data}  : Create a new object or Update a the current object if the control number exists
        /// GET 	/API/objectcontroller/{id}   : Get the object information where the control number = "id"
        /// PUT 	/API/objectcontroller/{data} : Update the object information stored in data
        /// DELETE	/API/objectcontroller/{id}   : Delete object where the control number = "id"

        [HttpGet, ActionName("Get")]
        public Models.Response Get(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DModel.AllFilters f = new DModel.AllFilters();
                addToFilters(ref f, "CarrierControl", id.ToString());
                int RecordCount = 0;
                int count = 0;
                Models.CarrierModel[] records = new Models.CarrierModel[] { };
                LTS.Carrier[] oData = NGLCarrierData.GetMasterCarriers365(ref RecordCount, f);
                if (oData?.Count() > 0)
                {                 
                    Models.CarrierModel record = selectModelData(oData[0]);
                    records = new Models.CarrierModel[] { record };
                    count = 1;
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
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter, "MstrCarrGridFltr"); } //save the page filter for the next time the page loads
                DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DModel.AllFilters>(filter);
                int RecordCount = 0;
                int count = 0;
                Models.CarrierModel[] records = new Models.CarrierModel[] { };
                LTS.Carrier[] oData = NGLCarrierData.GetMasterCarriers365(ref RecordCount, f);
                if (oData?.Count() > 0)
                {
                    count = oData.Count();
                    records = (from e in oData select selectModelData(e)).ToArray();
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
        public Models.Response Post([FromBody]Models.CarrierModel data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                if (data == null) { return getNullDataMsgResponse(); } //If the data is null return an error message
                bool blnRet = false;
                LTS.Carrier oChanges = selectLTSData(data);
                LTS.vLegalEntityAdmin lea = NGLLegalEntityAdminData.GetLegalEntityAdmin(Parameters.UserLEControl);
                if (lea != null) { oChanges.CarrierLegalEntity = lea.LegalEntity; }
                var arr = new int?[] { oChanges.CarrierGradReliability, oChanges.CarrierGradBillingAccuracy, oChanges.CarrierGradFinancialStrength, oChanges.CarrierGradEquipmentCondition, oChanges.CarrierGradContactAttitude, oChanges.CarrierGradDriverAttitude, oChanges.CarrierGradClaimFrequency, oChanges.CarrierGradClaimPayment, oChanges.CarrierGradGeographicCoverage, oChanges.CarrierGradPriceChangeNotification, oChanges.CarrierGradPriceChangeFrequency, oChanges.CarrierGradPriceAggressiveness };
                oChanges.CarrierGradAverage = Queryable.Average(arr.AsQueryable());
                DModel.ResultObject res = NGLCarrierData.InsertOrUpdateCarrier365(oChanges);
                if (res != null)
                {
                    if (!res.Success) {
                        response.StatusCode = HttpStatusCode.InternalServerError;
                        response.Errors = res.ErrMsg;
                        return response;
                    } else { blnRet = true; }
                }         
                bool[] oRecords = new bool[1] { blnRet };
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
                bool blnRet = false;
                DTO.Carrier dto = NGLCarrierData.GetCarrier(id);
                if (dto != null) { NGLCarrierData.DeleteRecord(dto); blnRet = true; }               
                bool[] oRecords = new bool[1] { blnRet };
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