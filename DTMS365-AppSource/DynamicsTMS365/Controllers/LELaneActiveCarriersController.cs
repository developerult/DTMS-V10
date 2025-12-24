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
    public class LELaneActiveCarriersController : NGLControllerBase
    {
        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.2 on 08/29/2018 initializes the Page property by calling the base class constructor
        /// </summary>
        public LELaneActiveCarriersController()
                : base(Utilities.PageEnum.LELanePreferredCarriers)
	     {
        }

        #endregion
        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.LELaneActiveCarriersController";
        /// <summary>
        /// SourceClass Property for logging and error tracking
        /// </summary>
        /// <value>The source class.</value>
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }
        /// <summary>
        /// The request
        /// </summary>
        HttpRequest request = HttpContext.Current.Request;
        #endregion

        #region " Data Translation"  
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
                //int RecordCount = 0;
                //int count = 0;
                DAL.NGLLaneFeeData oDAL = new DAL.NGLLaneFeeData(Parameters);
                //LTS.vLanePreferredCarrier oData = oDAL.GetLaneFee365(id);
                //Models.LanePreferredCarrier[] records = new Models.LanePreferredCarrier[1];
                //if (oData != null)
                //{
                //    count = 1;
                //    records[0] = selectModelData(oData);
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
              //  Models.LanePreferredCarrier[] records = new Models.LanePreferredCarrier[] { };
                LTS.vLELaneActiveCarrier[] oData = new LTS.vLELaneActiveCarrier[] { };
                oData = oDAL.LookupLaneActiveCarriers365(f.ParentControl);

                if (oData != null && oData.Count() > 0)
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


         [HttpPost, ActionName("SaveLimitLaneToCarrier")]
        public Models.Response SaveLimitLaneToCarrier(Models.LanePreferredCarrier data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                bool blnRet = false; //default
                DAL.NGLLaneData oDAL = new DAL.NGLLaneData(Parameters);               
                LTS.vLELaneActiveCarrier[] oData = new LTS.vLELaneActiveCarrier[] { };
                LTS.LimitLaneToCarrier record = selectLTSData(data);
                blnRet = oDAL.AddSelectedCarriersToRestrictedList(record);
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
        #endregion
    }

}