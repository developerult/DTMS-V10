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
    public class CarrierEquipmentController : NGLControllerBase
    {
        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.2 on 08/29/2018 initializes the Page property by calling the base class constructor
        /// </summary>
        public CarrierEquipmentController()
               : base(Utilities.PageEnum.CarrierEquipment)
        {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.CarrierEquipmentController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"
        //  LTS.vLaneProfileSettings

        private Models.CarrierTruck selectModelData(LTS.vMasterTruckList d)
        {
            Models.CarrierTruck modelRecord = new Models.CarrierTruck();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "rowguid" ,"CarrierTruckUpdated"};
                string sMsg = "";
                modelRecord = (Models.CarrierTruck)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);                
                if (modelRecord != null) { modelRecord.setUpdated(d.CarrierTruckUpdated.ToArray()); }
            }

            return modelRecord;
        }
     

        public static LTS.vLaneProfileSetting selectLTSData(Models.CarrierTruck d)
        {
            LTS.vLaneProfileSetting ltsRecord = new LTS.vLaneProfileSetting();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "rowguid", "CarrierTruckUpdated" };
                string sMsg = "";
                ltsRecord = (LTS.vLaneProfileSetting)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                //if (ltsRecord != null)
                //{
                //    byte[] bupdated = d.getUpdated();
                //    ltsRecord.BookItemUpdated = bupdated == null ? new byte[0] : bupdated;

                //}
            }

            return ltsRecord;
        }

        public static LTS.vLaneProfileSetting selectDToData(DTO.CarrierTruck d)
        {
            LTS.vLaneProfileSetting ltsRecord = new LTS.vLaneProfileSetting();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "rowguid", "CarrierTruckUpdated" };
                string sMsg = "";
                ltsRecord = (LTS.vLaneProfileSetting)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                //if (ltsRecord != null)
                //{
                //    byte[] bupdated = d.getUpdated();
                //    ltsRecord.BookItemUpdated = bupdated == null ? new byte[0] : bupdated;

                //}
            }

            return ltsRecord;
        }
        public static LTS.CarrierTruck selectLTSData1(Models.CarrierTruck d)
        {
            LTS.CarrierTruck ltsRecord = new LTS.CarrierTruck();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "rowguid" ,"CarrierTruckUpdated", "Page", "Pages", "RecordCount", "PageSize"

,"_CarrierTruckControl"
,"_CarrierTruckCarrierControl"
,"_CarrierTruckDescription"
,"_CarrierTruckWgtFrom"
,"_CarrierTruckWgtTo"
,"_CarrierTruckRateStarts"
,"_CarrierTruckRateExpires"
,"_CarrierTruckTL"
,"_CarrierTruckLTL"
,"_CarrierTruckEquipment"
,"_CarrierTruckMileRate"
,"_CarrierTruckCwtRate"
,"_CarrierTruckCaseRate"
,"_CarrierTruckFlatRate"
,"_CarrierTruckPltRate"
,"_CarrierTruckCubeRate"
,"_CarrierTruckTLT"
,"_CarrierTruckTMode"
,"_CarrierTruckFAK"
,"_CarrierTruckDisc"
,"_CarrierTruckPUMon"
,"_CarrierTruckPUTue"
,"_CarrierTruckPUWed"
,"_CarrierTruckPUThu"
,"_CarrierTruckPUFri"
,"_CarrierTruckPUSat"
,"_CarrierTruckPUSun"
,"_CarrierTruckDLMon"
,"_CarrierTruckDLTue"
,"_CarrierTruckDLWed"
,"_CarrierTruckDLThu"
,"_CarrierTruckDLFri"
,"_CarrierTruckDLSat"
,"_CarrierTruckDLSun"
,"_CarrierTruckPayTolPerLo"
,"_CarrierTruckPayTolPerHi"
,"_CarrierTruckPayTolCurLo"
,"_CarrierTruckPayTolCurHi"
,"_CarrierTruckCurType"
,"_CarrierTruckModUser"
,"_CarrierTruckModDate"
,"_CarrierTruckRoute"
,"_CarrierTruckMiles"
,"rowguid"
,"_CarrierTruckBkhlCostPerc"
,"_CarrierTruckPalletCostPer"
,"_CarrierTruckFuelSurChargePerc"
,"_CarrierTruckStopCharge"
,"_CarrierTruckDropCost"
,"_CarrierTruckUnloadDiff"
,"_CarrierTruckCasesAvailable"
,"_CarrierTruckCasesOpen"
,"_CarrierTruckCasesCommitted"
,"_CarrierTruckWgtAvailable"
,"_CarrierTruckWgtOpen"
,"_CarrierTruckWgtCommitted"
,"_CarrierTruckCubesAvailable"
,"_CarrierTruckCubesOpen"
,"_CarrierTruckCubesCommitted"
,"_CarrierTruckPltsAvailable"
,"_CarrierTruckPltsOpen"
,"_CarrierTruckPltsCommitted"
,"_CarrierTruckTrucksAvailable"
,"_CarrierTruckMaxLoadsByWeek"
,"_CarrierTruckMaxLoadsByMonth"
,"_CarrierTruckTotalLoadsForWeek"
,"_CarrierTruckTotalLoadsForMonth"
,"_CarrierTruckWeekDate"
,"_CarrierTruckMonthDate"
,"_CarrierTruckUpdated"
,"_CarrierTruckTempType"
,"_CarrierTruckHazmat"
};
                string sMsg = "";
                ltsRecord = (LTS.CarrierTruck)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.CarrierTruckUpdated = bupdated == null ? new byte[0] : bupdated;

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
                f.ParentControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.LegalEntityCarrierMaint);               
                DAL.NGLCarrierTruckData oDAL = new DAL.NGLCarrierTruckData(Parameters);
                Models.CarrierTruck[] records = new Models.CarrierTruck[] { };
                LTS.vMasterTruckList[] oData = new LTS.vMasterTruckList[] { };
                oData = oDAL.GetCarrierTrucks365(f,ref RecordCount);               
                
                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                    records = (from e in oData select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                response = new Models.Response(records.ToArray(), count);


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

        [HttpGet, ActionName("Get")]
        public Models.Response Get(int id)
        {
            
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
               
                int count = 0;
                int RecordCount = 0;
                DAL.Models.AllFilters f = new DAL.Models.AllFilters();
               DAL.NGLCarrierTruckData oDAL = new DAL.NGLCarrierTruckData(Parameters);
                int iCarrierTruckControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.LegalEntityCarrierMaint);
                f.ParentControl = iCarrierTruckControl;
                Models.CarrierTruck[] records = new Models.CarrierTruck[] { };
                LTS.vMasterTruckList[] oData = new LTS.vMasterTruckList[] { };
                oData = oDAL.GetCarrierTrucks365(f, ref RecordCount).Where(x => x.CarrierTruckControl == id).ToArray();

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
        public Models.Response Post([System.Web.Http.FromBody]Models.CarrierTruck data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
               
               // Models.CarrierTruck mdl = selectModelData(data);
                LTS.CarrierTruck record = selectLTSData1(data);
                int iCarrierControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.LegalEntityCarrierMaint);
               // record.CarrierTruckControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.CarrierEquipment);
                record.CarrierTruckCarrierControl = iCarrierControl;
                DAL.NGLCarrierTruckData oDAL = new DAL.NGLCarrierTruckData(Parameters);


                oDAL.SaveOrCreateCarrierTruckItem(record);
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
                DAL.NGLCarrierTruckData oDAL = new DAL.NGLCarrierTruckData(Parameters);
                int iCarrierControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.CarrierEquipment);
                DTO.CarrierTruck oData = new DTO.CarrierTruck { };
                bool blnRet = oDAL.DeleteCarrierTruckItem(id);
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