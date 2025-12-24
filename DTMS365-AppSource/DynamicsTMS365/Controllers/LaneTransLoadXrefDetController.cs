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
    public class LaneTransLoadXrefDetController : NGLControllerBase
    {

        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.4 on 08/29/2018 initializes the Page property by calling the base class constructor
        /// </summary>
        public LaneTransLoadXrefDetController()
               : base(Utilities.PageEnum.LELaneTransLoad)
        {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.LaneTransLoadXrefDetController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        /// <summary>
        /// Perform a deep copy clone of vLaneTransLoadXrefDet data to LaneTransLoadXrefDet model data using Reflection
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.4.0.002 on 03/03/2021
        ///     we convert the vLaneTransLoadXrefDet data and store it in the   local LaneTransLoadXrefDet model
        ///     so it can be converted to a JSON object
        /// </remarks>
        private Models.LaneTransLoadXrefDet selectModelData(LTS.vLaneTransLoadXrefDet d)
        {
            Models.LaneTransLoadXrefDet modelRecord = new Models.LaneTransLoadXrefDet();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "LaneTranXDetUpdated", "vLaneTransLoadXref" };
                string sMsg = "";
                modelRecord = (Models.LaneTransLoadXrefDet)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                //When using Updated field with the DTO objects the data has already been converted to an array
                //so replace the LTL logic 
                //if (modelRecord != null) { modelRecord.setUpdated(d.LaneTranXDetUpdated.ToArray()); }
                // with the code below (just remove the ToArray
                if (modelRecord != null) { modelRecord.setUpdated(d.LaneTranXDetUpdated.ToArray()); }
            }

            return modelRecord;
        }


        /// <summary>
        /// Perform a deep copy clone of LaneTransLoadXrefDet model data to LaneTransLoadXrefDet table data using Reflection
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.4.0.002 on 03/03/2021
        ///     we convert the vLaneTransLoadXrefDet data stored in the  local LaneTransLoadXrefDet model
        ///     back to LTS.LaneTransLoadXrefDet table data so we can perform inserts or updates
        /// </remarks>
        public static LTS.LaneTransLoadXrefDet selectLTSData(Models.LaneTransLoadXrefDet d)
        {
            LTS.LaneTransLoadXrefDet ltsRecord = new LTS.LaneTransLoadXrefDet();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "LaneTranXDetUpdated", "Lane", "LaneTranRefLane", "LaneTransLoadXref", "tblModeTypeRefLane" };
                string sMsg = "";
                ltsRecord = (LTS.LaneTransLoadXrefDet)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.LaneTranXDetUpdated = bupdated == null ? new byte[0] : bupdated;

                }
            }

            return ltsRecord;
        }

        public static LTS.vLaneTransLoadXrefDet selectDTOData(Models.LaneTransLoadXrefDet d)
        {
            LTS.vLaneTransLoadXrefDet ltsRecord = new LTS.vLaneTransLoadXrefDet();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "LaneTranXDetUpdated","vLaneTransLoadXref" };
                string sMsg = "";
                ltsRecord = (LTS.vLaneTransLoadXrefDet)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.LaneTranXDetUpdated = bupdated == null ? new byte[0] : bupdated;

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
            //Note: The id must always match a LaneTranXDetControl associated with the selected record 
            //the system looks up the last saved Book Control pk for this user 
            //an invalid parent key Error is returned if the data does not match
            //If id is zero and no records are found the client should configure the page to add a new record.
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {

                int count = 0;
                DAL.NGLLaneTransLoadXrefDetData oDAL = new DAL.NGLLaneTransLoadXrefDetData(Parameters);
                LTS.vLaneTransLoadXrefDet oData = oDAL.GetLaneTransLoadXrefDet365(id);
                Models.LaneTransLoadXrefDet[] records = new Models.LaneTransLoadXrefDet[1];
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

        /// <summary>
        /// Gets All the Child vLECompCar Records filtered by LECompCarCompControl passed in as id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.2 on 10/09/2018
        ///   new name for Get method renamed to support Edit Widget
        /// </remarks>
        [HttpGet, ActionName("GetByParent")]
        public Models.Response GetByParent(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            DAL.Models.AllFilters f = new DAL.Models.AllFilters { filterName = "LaneTranXDetLaneTranXControl", filterValue = id.ToString() };
            f.ParentControl = id;
            return GetLaneTransLoadXrefDets(f);
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
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);

                //save the page filter for the next time the page loads
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter, "LaneTransLoadXrefDets"); }
                f.ParentControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.LELaneTransLoad);//New Prop
                return GetLaneTransLoadXrefDets(f);
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
        public Models.Response Post([System.Web.Http.FromBody]Models.LaneTransLoadXrefDet data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {                
                int iLaneTranXControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.LELaneTransLoad);

                DAL.NGLLaneTransLoadXrefDetData oDAL = new DAL.NGLLaneTransLoadXrefDetData(Parameters);
                LTS.LaneTransLoadXrefDet record = selectLTSData(data);                              
                bool blnRet = oDAL.SaveOrCreateLaneTransLoadXrefDetItem(record, iLaneTranXControl);
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
                DAL.NGLLaneTransLoadXrefDetData oDAL = new DAL.NGLLaneTransLoadXrefDetData(Parameters);
                bool blnRet = oDAL.DeleteLaneTransLoadXrefDetItem(id);
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

        #region " Private "
        public Models.Response GetLaneTransLoadXrefDets(DAL.Models.AllFilters f)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            try
            {
                int RecordCount = 0;
                int count = 0;
                
                DAL.NGLLaneTransLoadXrefDetData oDAL = new DAL.NGLLaneTransLoadXrefDetData(Parameters);
                Models.LaneTransLoadXrefDet[] records = new Models.LaneTransLoadXrefDet[] { };
                LTS.vLaneTransLoadXrefDet[] oData = new LTS.vLaneTransLoadXrefDet[] { };
                oData = oDAL.GetLaneTransLoadXrefDets365(f, ref RecordCount);

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


        #endregion

    }
}