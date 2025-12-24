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
    public class LELaneFeeController : NGLControllerBase
    {
        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.2 on 08/29/2018 initializes the Page property by calling the base class constructor
        /// </summary>
        public LELaneFeeController()
               : base(Utilities.PageEnum.LELaneFee)
        {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.LELaneFeeController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"
        //  LTS.vLELaneFee

        private Models.LELaneFee selectModelData(LTS.vLELaneFee d)
        {
            Models.LELaneFee modelRecord = new Models.LELaneFee();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "LaneFeesUpdated", "rowguid" };
                string sMsg = "";
                modelRecord = (Models.LELaneFee)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                //When using Updated field with the DTO objects the data has already been converted to an array
                //so replace the LTL logic 
                //if (modelRecord != null) { modelRecord.setUpdated(d.LaneFeesUpdated.ToArray()); }
                // with the code below (just remove the ToArray
                if (modelRecord != null) { modelRecord.setUpdated(d.LaneFeesUpdated.ToArray()); }
            }

            return modelRecord;
        }

       

        public static LTS.LaneFee selectLTSData(Models.LELaneFee d)
        {
            LTS.LaneFee ltsRecord = new LTS.LaneFee();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "LaneFeesUpdated", "rowguid" ,"Lane"};
                string sMsg = "";
                ltsRecord = (LTS.LaneFee)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.LaneFeesUpdated = bupdated == null ? new byte[0] : bupdated;

                }
            }

            return ltsRecord;
        }

        public static LTS.vLELaneFee selectDTOData(Models.LELaneFee d)
        {
            LTS.vLELaneFee ltsRecord = new LTS.vLELaneFee();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "LaneFeesUpdated", "rowguid" };
                string sMsg = "";
                ltsRecord = (LTS.vLELaneFee)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.LaneFeesUpdated = bupdated == null ? new byte[0] : bupdated;

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

        [HttpGet, ActionName("Get")]
        public Models.Response Get(int id)
        {
            //Note: The id must always match a LELaneFeeControl associated with the select Book Item Record 
            //the system looks up the last saved Book Control pk for this user 
            //an invalid parent key Error is returned if the data does not match
            //If id is zero and no records are found the client should configure the page to add a new record.
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                
                int count = 0;
                DAL.NGLLaneFeeData oDAL = new DAL.NGLLaneFeeData(Parameters);
                LTS.vLELaneFee oData = oDAL.GetLaneFee365(id);
                Models.LELaneFee[] records = new Models.LELaneFee[1];
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
                f.ParentControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.LaneMaint);//New Prop

                DAL.NGLLaneFeeData oDAL = new DAL.NGLLaneFeeData(Parameters);

                Models.LELaneFee[] records = new Models.LELaneFee[] { };
                LTS.vLELaneFee[] oData = new LTS.vLELaneFee[] { };
                oData = oDAL.GetLaneFees365(f, ref RecordCount);

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
        public Models.Response Post([System.Web.Http.FromBody]Models.LELaneFee data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int iLaneFeeControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.LELaneFee);
                int iLaneFeelaneControl= readPagePrimaryKey(Parameters, Utilities.PageEnum.LaneMaint);

                DAL.NGLLaneFeeData oDAL = new DAL.NGLLaneFeeData(Parameters);
                LTS.LaneFee record = selectLTSData(data);
                record.LaneFeesLaneControl = iLaneFeelaneControl;
                bool blnRet = oDAL.SaveOrCreateLaneFeeItem(record,data.LaneFeesControl);
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
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLLaneFeeData oDAL = new DAL.NGLLaneFeeData(Parameters);
                bool blnRet = oDAL.DeleteLaneFeeItem(id);
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