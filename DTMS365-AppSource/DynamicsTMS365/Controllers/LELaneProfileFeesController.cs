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
    public class LELaneProfileFeesController : NGLControllerBase
    {
        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.2 on 08/29/2018 initializes the Page property by calling the base class constructor
        /// </summary>
        public LELaneProfileFeesController()
               : base(Utilities.PageEnum.LELaneProfileFees)
        {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.LELaneProfileFeesController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"
        //  LTS.vLaneProfileSettings

        private Models.LELaneProfileFees selectModelData(DTO.LaneProfileSettings d)
        {
            Models.LELaneProfileFees modelRecord = new Models.LELaneProfileFees();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "rowguid" };
                string sMsg = "";
                modelRecord = (Models.LELaneProfileFees)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                //When using Updated field with the DTO objects the data has already been converted to an array
                //so replace the LTL logic 
                //if (modelRecord != null) { modelRecord.setUpdated(d.BookItemUpdated.ToArray()); }
                // with the code below (just remove the ToArray
               // if (modelRecord != null) { modelRecord.setUpdated(d.LaneProfileUpdated); }
            }

            return modelRecord;
        }
     

        public static LTS.vLaneProfileSetting selectLTSData(Models.LELaneProfileFees d)
        {
            LTS.vLaneProfileSetting ltsRecord = new LTS.vLaneProfileSetting();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "rowguid" };
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


        public static LTS.vLaneProfileSetting selectLTSDTOData(DTO.LaneProfileSettings d)
        {
            LTS.vLaneProfileSetting ltsRecord = new LTS.vLaneProfileSetting();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "_intPage",
                    "Page",
"PageSize",
"Pages",
"Parameters",
"RecordCount",
"TrackingState","_LaneProfileSettingsAccessorialCode","_LaneProfileSettingsAccessorialName","_LaneProfileSettingsLaneControl","_LaneProfileSettingsLaneName",
"_LaneProfileSettingsLaneNumber","_LaneProfileSettingsSelected","LaneProfileSettingsSelectedText"

                };
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
                f.ParentControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.LELaneProfileFees);
                f.LaneControl= readPagePrimaryKey(Parameters, Utilities.PageEnum.LaneMaint);
                DAL.NGLLaneData oDAL = new DAL.NGLLaneData(Parameters);

                Models.LELaneProfileFees[] records = new Models.LELaneProfileFees[] { };
                DTO.LaneProfileSettings[] oData = new DTO.LaneProfileSettings[] { };
                oData = oDAL.GetLaneProfileSettings(f.LaneControl, null);               
                
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
                DAL.NGLLaneData oDAL = new DAL.NGLLaneData(Parameters);
                int iLaneControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.LaneMaint);
             
                DTO.LaneProfileSettings oData = new DTO.LaneProfileSettings { };
                oData = oDAL.GetLaneProfileSettings(iLaneControl, null).Where(x=>x.LaneProfileSettingsAccessorialCode==id).FirstOrDefault();
                Models.LELaneProfileFees[] records = new Models.LELaneProfileFees[1];
                response = new Models.Response(records, count);               
               
                if (oData != null)
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

        [HttpPost, ActionName("Post")]
        public Models.Response Post([System.Web.Http.FromBody]Models.LELaneProfileFees data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int iLaneControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.LaneMaint);

                LTS.vLaneProfileSetting record = selectLTSData(data);
                DAL.NGLLaneData oDAL = new DAL.NGLLaneData(Parameters);              
                oDAL.UpdateLaneProfileXref(iLaneControl,record.LaneProfileSettingsAccessorialCode,record.LaneProfileSettingsSelected,Parameters.UserName);
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
                DAL.NGLLaneData oDAL = new DAL.NGLLaneData(Parameters);
                int iLaneControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.LaneMaint);
                DTO.LaneProfileSettings oData = new DTO.LaneProfileSettings { };
                oData = oDAL.GetLaneProfileSettings(iLaneControl, null).Where(x => x.LaneProfileSettingsAccessorialCode == id).FirstOrDefault();
               
                oDAL.UpdateLaneProfileXref(iLaneControl, oData.LaneProfileSettingsAccessorialCode, false, Parameters.UserName);
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


        #endregion
    }

}