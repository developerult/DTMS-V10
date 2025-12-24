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
    public class LELanePreferredCarriersController : NGLControllerBase
    {
        #region " Constructors "
        /// <summary>
        /// Created by ManoRama for LELanePreferredCarriers 30/jul/2020 initializes the Page property by calling the base class constructor
        /// </summary>
        public LELanePreferredCarriersController()
            : base(Utilities.PageEnum.LELanePreferredCarriers)
        {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.LELanePreferredCarriersController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"        

        private Models.LanePreferredCarrier selectModelData(DTO.LimitLaneToCarrier d)
        {
            Models.LanePreferredCarrier modelRecord = new Models.LanePreferredCarrier();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "LimitLaneToCarrierUpdated", "rowguid" ,"Lane"};
                string sMsg = "";
               modelRecord = (Models.LanePreferredCarrier)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.LimitLaneToCarrierUpdated.ToArray()); }
            }

            return modelRecord;
        }

        private LTS.LimitLaneToCarrier selectLTSDTOData(DTO.LimitLaneToCarrier d)
        {
            LTS.LimitLaneToCarrier modelRecord = new LTS.LimitLaneToCarrier();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "LimitLaneToCarrierUpdated", "rowguid", "Lane" };
                string sMsg = "";
                modelRecord = (LTS.LimitLaneToCarrier)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
               // if (modelRecord != null) { modelRecord.setUpdated(d.LimitLaneToCarrierUpdated.ToArray()); }
            }

            return modelRecord;
        }
        public static LTS.LimitLaneToCarrier selectLTSData(Models.LanePreferredCarrier d)
        {
            LTS.LimitLaneToCarrier ltsRecord = new LTS.LimitLaneToCarrier();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "LimitLaneToCarrierUpdated", "rowguid", "Lane" };
                string sMsg = "";
                ltsRecord = (LTS.LimitLaneToCarrier)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.LimitLaneToCarrierUpdated = bupdated == null ? new byte[0] : bupdated;

                }
            }

            return ltsRecord;
        }      
        public static DTO.LimitLaneToCarrier selectDTOData(LTS.LimitLaneToCarrier d)
        {
            DTO.LimitLaneToCarrier ltsRecord = new DTO.LimitLaneToCarrier();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "LimitLaneToCarrierUpdated", "rowguid", "Lane" };
                string sMsg = "";
                ltsRecord = (DTO.LimitLaneToCarrier)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                //if (ltsRecord != null)
                //{
                //    byte[] bupdated = d.getUpdated();
                //    ltsRecord.LimitLaneToCarrierUpdated = bupdated == null ? new byte[0] : bupdated;

                //}
            }

            return ltsRecord;
        }

        private Models.LanePreferredCarrierDetails selectDTOModelData(DTO.LimitLaneToCarrierDetails d)
        {
            Models.LanePreferredCarrierDetails modelRecord = new Models.LanePreferredCarrierDetails();
            

            if (d != null)
            {
                List<string> skipObjs = new List<string> { "LimitLaneToCarrierUpdated", "rowguid", "Lane" };
                string sMsg = "";
                modelRecord = (Models.LanePreferredCarrierDetails)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                //if (modelRecord != null) { modelRecord.setUpdated(d.LimitLaneToCarrierUpdated.ToArray()); }
            }

            return modelRecord;
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
            //Note: The id must always match a LanePreferredCarrierControl associated with the select Book Item Record 
            //the system looks up the last saved Book Control pk for this user 
            //an invalid parent key Error is returned if the data does not match
            //If id is zero and no records are found the client should configure the page to add a new record.
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int RecordCount = 0;
                int count = 0;                
              
                DAL.NGLLaneData oDAL = new DAL.NGLLaneData(Parameters);
                DTO.LimitLaneToCarrier[] OData = new DTO.LimitLaneToCarrier[] { };
                Models.LanePreferredCarrier[] records = new Models.LanePreferredCarrier[] { };
                OData = oDAL.GetLanePreferredCarriersByID365(id, ref RecordCount);

                if (OData != null && OData.Count() > 0)
                {
                    count = OData.Count();
                    records = (from e in OData select selectModelData(e)).ToArray();
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
                f.ParentControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.LaneMaint);
                DAL.NGLLaneData oDAL = new DAL.NGLLaneData(Parameters);
                DTO.LimitLaneToCarrier[] OData = new DTO.LimitLaneToCarrier[] { };
                Models.LanePreferredCarrier[] records = new Models.LanePreferredCarrier[] { };
                OData = oDAL.GetLanePreferredCarriers365(f.ParentControl, ref RecordCount);

                if (OData != null && OData.Count() > 0)
                {
                    count = OData.Count();
                    records = (from e in OData select selectModelData(e)).ToArray();
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

       
        [HttpGet, ActionName("GetByParent")]
        public Models.Response GetByParent(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
         //   DAL.Models.AllFilters f = new DAL.Models.AllFilters { filterName = "LLTCControl", filterValue = id.ToString() };
            return GetLEPrefCarsFiltered(id);
        }

        private Models.Response GetLEPrefCarsFiltered(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int RecordCount = 0;
                int count = 0;
                //if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter); }
                int ParentControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.LaneMaint);
                DAL.NGLLaneData oDAL = new DAL.NGLLaneData(Parameters);
                DTO.LimitLaneToCarrier[] OData = new DTO.LimitLaneToCarrier[] { };
                Models.LanePreferredCarrier[] records = new Models.LanePreferredCarrier[] { };
                Models.LanePreferredCarrierDetails[] rec = new Models.LanePreferredCarrierDetails[] { };
                DTO.LimitLaneToCarrierDetails[] records1 = new DTO.LimitLaneToCarrierDetails[] { };
                OData = oDAL.GetLanePreferredCarriers365(ParentControl, ref RecordCount);

                if (OData != null && OData.Count() > 0)
                {
                    count = OData.Count();
                    records = (from e in OData select selectModelData(e)).Where(x => x.LLTCControl == id).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }
                if (records != null && records.Count() > 0)
                {

                    records1 = records[0].Details;
                        
                    
                    rec = (from e in records1 select selectDTOModelData(e)).ToArray();
                }
                response = new Models.Response(rec, count);
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
        public Models.Response Post([System.Web.Http.FromBody]Models.LanePreferredCarrier data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                // modified by  RHR for v - 8.5.1.002 on 04 / 18 / 2022  renamed iLanePrefCarrierControl to iLLTCControl
                //      CarrierControl is not the primary key in the table LLTCControl is the key
                int iLLTCControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.LELanePreferredCarriers);
                int iLaneFeelaneControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.LaneMaint);

                DAL.NGLLaneData oDAL = new DAL.NGLLaneData(Parameters);
                LTS.LimitLaneToCarrier record = selectLTSData(data);
                //DTO.LimitLaneToCarrier dtoCarrier = selectDTOData(record);
                //removed by RHR for v-8.5.1.002 on 04/18/2022  iLanePrefCarrierControl was the primary key not the carrier control (see renamed above)
                //          Also the parameter is not being set correcly when the selected record is changed in the UI
                //record.LLTCCarrierControl = iLanePrefCarrierControl;
                bool blnRet = oDAL.UpdateLanePreferredCarrier365(record);
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

        [HttpPost, ActionName("SavePreferredCarrierContacts")]
        public Models.Response SavePreferredCarrierContacts(Models.LanePreferredCarrier data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int iLanePrefCarrierControl = data.LLTCControl;
                int RecordCount = 0;                            
                bool blnRet = false; //default

                DAL.NGLLaneData oDAL = new DAL.NGLLaneData(Parameters);
                DTO.LimitLaneToCarrier[] OData = new DTO.LimitLaneToCarrier[] { };                            
                OData = oDAL.GetLanePreferredCarriersByID365(iLanePrefCarrierControl, ref RecordCount);
                if (OData != null && OData.Count() > 0)
                {
                    DTO.LimitLaneToCarrier dtoprefcar = OData.FirstOrDefault();
                    //LTS.LimitLaneToCarrier prefcar = selectLTSDTOData(dtoprefcar);
                    dtoprefcar.LLTCCarrierContControl = data.LLTCCarrierContControl;                    
                    blnRet = oDAL.UpdateLanePreferredCarrier365DTO(dtoprefcar);
                }


                blnRet = true;

                Array d = new bool[1] { blnRet };
                response = new Models.Response(d, 1);
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


        [HttpGet, ActionName("GetContacts")]
        public Models.Response GetContacts(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int RecordCount = 0;
                int count = 0;
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                DAL.Models.Contact[] oData = new DAL.Models.Contact[] { };
                f.LaneControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.LaneMaint);
                //f.LEAdminControl=
                DAL.NGLCarrierContData oDAL = new DAL.NGLCarrierContData(Parameters);              
                Models.LanePreferredCarrier[] records = new Models.LanePreferredCarrier[] { };                
                oData = oDAL.GetUserLEPreferredCarrierContacts( ref RecordCount,f);
                if (oData?.Count() > 0)
                {
                    count = oData.Count();
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
        [HttpDelete, ActionName("DELETE")]
        public Models.Response DELETE(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLLaneData oDAL = new DAL.NGLLaneData(Parameters);
                oDAL.RemoveLaneCarrierRestriction(id);
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