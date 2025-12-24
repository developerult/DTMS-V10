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
    /// Rest Service for Lane Detail Data
    /// </summary>
    /// <remarks>
    /// Modified by RHR for v-8.4 on 06/24/2021 changed location of selectLTSData and selectModelData for vLELane365
    ///     we now call staic method  Models.vLELane365.selectLTSData and Models.vLELane365.selectModelData
    /// </remarks>
    public class LaneDetailController : NGLControllerBase
    {
        #region " Constructors "

        /// <summary>Initializes the Page property by calling the base class constructor</summary>
        public LaneDetailController() : base(Utilities.PageEnum.LaneDetail) { }

        #endregion

        #region " Properties"

        /// <summary>This property is used for logging and error tracking</summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.LaneDetailController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        

        private Models.Comp selectModelData(LTS.vLEComp365 d)
        {
            Models.Comp modelRecord = new Models.Comp();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "CompUpdated" };
                string sMsg = "";
                modelRecord = (Models.Comp)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.CompUpdated.ToArray()); }
            }
            return modelRecord;
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
            //Note: The id must always match a LaneControl
            //The system looks up the last saved LaneDetail pk for this user and return the first Lane record found
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                if (id == 0) { id = readPagePrimaryKey(Parameters, Utilities.PageEnum.LaneDetail); } //get the parent control
                DAL.Models.AllFilters f = new DAL.Models.AllFilters();
                f.filterName = "LaneControl";
                f.filterValue = id.ToString();
                f.LEAdminControl = Parameters.UserLEControl;
                Models.vLELane365[] records = new Models.vLELane365[] { };
                int RecordCount = 0;
                int count = 0;
                //LTS.vLELane365[] oData = NGLLaneData.GetLELane365(ref RecordCount, f);
                LTS.vLELane365[] oData = NGLLaneData.GetLELanesFiltered(ref RecordCount, f);
                if (oData != null && oData.Count() > 0)
                {
                    count = RecordCount;
                    records = (from e in oData select Models.vLELane365.selectModelData(e)).ToArray();
                }
                response = new Models.Response(records, 1);
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
                //////save the page filter for the next time the page loads
                ////if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter); }
                ////DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);

                ////int id = f.ParentControl;
                ////if (id == 0)
                ////{
                ////    //get the parent control
                ////    id = readPagePrimaryKey(Parameters, Utilities.PageEnum.LoadBoard);
                ////}

                ////DAL.NGLBookRevenueData oDAL = new DAL.NGLBookRevenueData(Parameters);
                ////LTS.vLoadBoardRev oData = new LTS.vLoadBoardRev();
                ////LTS.vLoadBoardRev[] records = new LTS.vLoadBoardRev[1];
                ////oData = oDAL.GetvLoadBoardRev(id);
                ////if (oData != null)
                ////{
                ////    records[0] = oData;
                ////}

                ////response = new Models.Response(records, 1);
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
        public Models.Response Post([FromBody]Models.vLELane365 data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                LTS.vLELane365 ltsLane = Models.vLELane365.selectLTSData(data);
                ltsLane.LaneDestEmergencyContactPhone = Utilities.removeNonNumericText(ltsLane.LaneDestEmergencyContactPhone);
                ltsLane.LaneDestContactPhone = Utilities.removeNonNumericText(ltsLane.LaneDestContactPhone);
                ltsLane.LaneOrigEmergencyContactPhone = Utilities.removeNonNumericText(ltsLane.LaneOrigEmergencyContactPhone);
                ltsLane.LaneOrigEmergencyContactPhone = Utilities.removeNonNumericText(ltsLane.LaneOrigEmergencyContactPhone);
                DAL.Models.ResultObject result = NGLLaneData.InsertOrUpdateLane365(ltsLane, data.LTTransType);
                var laneControl = 0;
                if (result != null) { laneControl = result.Control; }
                bool blnRet = false;
                if(laneControl != 0) { blnRet = true; }
                bool[] results = new bool[1] { blnRet };
                response = new Models.Response(results, 1);
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

        [HttpGet, ActionName("GetCompany")]
        public Models.Response GetCompany(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                //if (id == 0) { id = readPagePrimaryKey(Parameters, Utilities.PageEnum.CompanyDetail); } //get the parent control
                DAL.Models.AllFilters f = new DAL.Models.AllFilters();
                f.filterName = "CompControl";
                f.filterValue = id.ToString();
                Models.Comp[] records = new Models.Comp[] { };
                int RecordCount = 0;
                int count = 0;
                LTS.vLEComp365[] oData = NGLCompData.GetLEComp365(ref RecordCount, f);
                if (oData != null && oData.Count() > 0)
                {
                    count = RecordCount;
                    records = (from e in oData orderby e.CompName ascending select selectModelData(e)).ToArray();
                }
                response = new Models.Response(records, 1);
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



        [HttpGet, ActionName("RecalculateLatLongMiles")]
        public Models.Response RecalculateLatLongMiles(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {

                if (id == 0) { id = readPagePrimaryKey(Parameters, Utilities.PageEnum.LaneDetail); } //get the parent control
                BLL.NGLTMS365BLL bll = new BLL.NGLTMS365BLL(Parameters);
                DAL.Models.ResultObject result = bll.CalculateAndSaveLaneMilesLatLong(id);
                //DAL.Models.ResultObject result = bll.CalculateAndSaveMilesLatLong(id,1);
                if (result == null) { result = new DAL.Models.ResultObject(); }
                Array d = new DAL.Models.ResultObject[1] { result };
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