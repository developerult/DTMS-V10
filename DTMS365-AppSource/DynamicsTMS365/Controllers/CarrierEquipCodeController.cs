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
    public class CarrierEquipCodeController : NGLControllerBase
    {
        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.2 on 09/23/2018 initializes the Page property by calling the base class constructor
        /// </summary>
        public CarrierEquipCodeController()
                : base(Utilities.PageEnum.CarrierEquipCode)
	     {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.CarrierEquipCodeController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        private Models.CarrierEquipmentCode selectModelData(DTO.CarrierEquipCode d)
        {
            Models.CarrierEquipmentCode modelRecord = new Models.CarrierEquipmentCode();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "CarrierEquipCodesUpdated", "rowguid" };
                string sMsg = "";
                modelRecord = (Models.CarrierEquipmentCode)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.CarrierEquipCodesUpdated.ToArray()); }
            }

            return modelRecord;
        }

        private Models.CarrierEquipmentCode selectModelData(LTS.CarrierEquipCode d)
        {
            Models.CarrierEquipmentCode modelRecord = new Models.CarrierEquipmentCode();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "CarrierEquipCodesUpdated", "rowguid" };
                string sMsg = "";
                modelRecord = (Models.CarrierEquipmentCode)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.CarrierEquipCodesUpdated.ToArray()); }
            }

            return modelRecord;
        }


        private LTS.CarrierEquipCode selectLTSData(Models.CarrierEquipmentCode d)
        {
            LTS.CarrierEquipCode oRecord = new LTS.CarrierEquipCode();
            if (d != null)
            {                
                List<string> skipObjs = new List<string> { "CarrierEquipCodesUpdated","rowguid" };
                string sMsg = "";
                oRecord = (LTS.CarrierEquipCode)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(oRecord, d, skipObjs, ref sMsg);
                if (oRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    oRecord.CarrierEquipCodesUpdated = bupdated == null ? new byte[0] : bupdated;

                }
            }

            return oRecord;
        }



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
                DAL.NGLCarrierEquipCodeData oDAL = new DAL.NGLCarrierEquipCodeData(Parameters);
                Models.CarrierEquipmentCode[] records = new Models.CarrierEquipmentCode[1];
                DTO.CarrierEquipCode oData = new DTO.CarrierEquipCode();
                oData = oDAL.GetCarrierEquipCodeFiltered(id);
                if (oData != null )
                {
                    count = 1;
                    records[0] = selectModelData(oData);
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
                DAL.NGLCarrierEquipCodeData oDAL = new DAL.NGLCarrierEquipCodeData(Parameters);
                Models.CarrierEquipmentCode[] records = new Models.CarrierEquipmentCode[] { };
                LTS.CarrierEquipCode[] oData = new LTS.CarrierEquipCode[] { };
                oData = oDAL.GetCarrierEquipCodes(f, ref RecordCount);
                if (oData != null && oData.Count() > 0)
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
        public Models.Response Post([System.Web.Http.FromBody]Models.CarrierEquipmentCode data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;
                DAL.NGLCarrierEquipCodeData oDAL = new DAL.NGLCarrierEquipCodeData(Parameters);
                Models.CarrierEquipmentCode[] records = new Models.CarrierEquipmentCode[1];
                LTS.CarrierEquipCode oChanges = selectLTSData(data);
                bool blnRet = oDAL.SaveOrCreateCarrierEquipCodes(oChanges);

                bool[] oRecords = new bool[1] { blnRet };
                response = new Models.Response(oRecords, 1);
                //DTO.CarrierEquipCode oUpdated = new DTO.CarrierEquipCode();
                //if (oChanges.CarrierEquipControl == 0)
                //{
                //    oUpdated = (DTO.CarrierEquipCode)oDAL.CreateRecord(oChanges);
                //} else
                //{
                //    oUpdated = (DTO.CarrierEquipCode)oDAL.UpdateRecord(oChanges);
                //}
                //if (oUpdated != null)
                //{
                //    count = 1;
                //    records[0] = selectModelData(oUpdated);
                //}
                //response = new Models.Response(records, count);
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
                DAL.NGLCarrierEquipCodeData oDAL = new DAL.NGLCarrierEquipCodeData(Parameters);
                bool blnRet = oDAL.DeleteCarrierEquipCode(id);
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