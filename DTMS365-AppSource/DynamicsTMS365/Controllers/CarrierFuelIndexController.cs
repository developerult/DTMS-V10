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
using System;

namespace DynamicsTMS365.Controllers
{

    public class CarrierFuelIndexController : NGLControllerBase
    {
        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.2 on 08/29/2018 initializes the Page property by calling the base class constructor
        /// </summary>
        public CarrierFuelIndexController()
                : base(Utilities.PageEnum.CarrierFuelIndexMaint)
	     {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.CarrierFuelIndexController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"
        private Models.vNatFuelCrossTab selectModelDataFromDTO(DTO.NatFuelCrossTab d)
        {
            Models.vNatFuelCrossTab modelRecord = new Models.vNatFuelCrossTab();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "CarrierFuelUpdated" };
                string sMsg = "";
                modelRecord = (Models.vNatFuelCrossTab)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                //if (modelRecord != null) { modelRecord.setUpdated(d.CarrierFuelUpdated.ToArray()); }
            }

            return modelRecord;
        }
        private Models.vNatFuelCrossTab selectModelData(LTS.vNatFuelCrossTab d)
        {
            Models.vNatFuelCrossTab modelRecord = new Models.vNatFuelCrossTab();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "CarrierFuelUpdated" };
                string sMsg = "";
                modelRecord = (Models.vNatFuelCrossTab)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                //if (modelRecord != null) { modelRecord.setUpdated(d.CarrierFuelUpdated.ToArray()); }
            }

            return modelRecord;
        }

        public static LTS.vNatFuelCrossTab selectLTSData(Models.vNatFuelCrossTab d)
        {
            LTS.vNatFuelCrossTab ltsRecord = new LTS.vNatFuelCrossTab();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "CarrierFuelUpdated" };
                string sMsg = "";
                ltsRecord = (LTS.vNatFuelCrossTab)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                   // ltsRecord.CarrierFuelUpdated = bupdated == null ? new byte[0] : bupdated;
                }
            }
            return ltsRecord;
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
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);

                //save the page filter for the next time the page loads
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter); }
                
                DAL.NGLCarrierData oDAL = new DAL.NGLCarrierData(Parameters);

                Models.vNatFuelCrossTab[] records = new Models.vNatFuelCrossTab[] { };
                LTS.vNatFuelCrossTab[] oData1 = new LTS.vNatFuelCrossTab[] { };
                DTO.NatFuelCrossTab[] oData = new DTO.NatFuelCrossTab[] { };
                oData1 = oDAL.GetNatFuelCrossTabData365(ref RecordCount,f);

                if (oData1 != null && oData1.Count() > 0)
                {
                    count = oData1.Count();
                    records = (from e in oData1 select selectModelData(e)).ToArray();
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
        public Models.Response Post([System.Web.Http.FromBody]Models.vNatFuelCrossTab data)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {

                DAL.NGLBatchProcessDataProvider oDAL = new DAL.NGLBatchProcessDataProvider(Parameters);
                LTS.vNatFuelCrossTab record = selectLTSData(data);               
                 oDAL.InsertNatFuelIndex(Convert.ToDateTime(record.NatFuelDate),Convert.ToDouble(record.NatAverage), Convert.ToDouble(record.ZoneFuelCost1), Convert.ToDouble(record.ZoneFuelCost2), Convert.ToDouble(record.ZoneFuelCost3),
                    Convert.ToDouble(record.ZoneFuelCost4), Convert.ToDouble(record.ZoneFuelCost5), Convert.ToDouble(record.ZoneFuelCost6), Convert.ToDouble(record.ZoneFuelCost7),
                    Convert.ToDouble(record.ZoneFuelCost8), Convert.ToDouble(record.ZoneFuelCost9));
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

        [HttpDelete, ActionName("DELETE")]
        public Models.Response DELETE(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLCarrierData oDAL = new DAL.NGLCarrierData(Parameters);
                 oDAL.DeleteNatFuelCrossTabData(id);
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


        [HttpGet, ActionName("UpdateAllFuelFees")]
        public Models.Response UpdateAllFuelFees()
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {

                BLL.NGLCarrierBLL oBLL = new BLL.NGLCarrierBLL(Parameters);                
                bool[] oRecords = new bool[1];
                oRecords[0] = oBLL.UpdateAllCarrierFuelFees();
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