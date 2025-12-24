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
using UTC = NGL.UTC.Library;
using NGL.UTC.Library;

namespace DynamicsTMS365.Controllers
{
    /// <summary>
    /// Rest Service for Lane Data
    /// </summary>
    /// <remarks>
    /// Modified by RHR for v-8.4 on 06/24/2021 changed location of selectLTSData and selectModelData for vLELane365
    ///     we now call staic method  Models.vLELane365.selectLTSData and Models.vLELane365.selectModelData
    /// </remarks>
    public class LaneController : NGLControllerBase
    {
        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.2 on 08/29/2018 initializes the Page property by calling the base class constructor
        /// </summary>
        public LaneController()
                : base(Utilities.PageEnum.LaneMaint)//Added for Page Name(Enum No) On 14-JUL-2020 by ManoRama with help of Rob
	     {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.LaneController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

       

       

        #endregion


        #region " REST Services"

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
            return GetLELanesFiltered(filter);
        }

        [HttpGet, ActionName("GetLELanesFiltered")]
        public Models.Response GetLELanesFiltered(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                //Modified by RHR for v-8.3.0.001 on 07/30/2020 
                // added logic to save the page filters.
                // Modified by RHR on 08/12/2020 for v-8.3.0.001 
                // when saving the page filter data. This changes was implemented
                // to support saving sorting and grouping settings when a filter is not 
                // selected. 
                // old code removed // if (!string.IsNullOrWhiteSpace(filter) && (f != null && f.FilterValues.Length > 0))
                if (!string.IsNullOrWhiteSpace(filter)){ savePageFilters(filter); }
                
                Models.vLELane365[] retLanes = new Models.vLELane365[] { };
                LTS.vLELane365[] ltsView = new LTS.vLELane365[] { };
                int RecordCount = 0;
                int count = 0;
                ltsView = NGLLaneData.GetLELanesFiltered(ref RecordCount, f);
                if (ltsView != null && ltsView.Count() > 0)
                {
                    //RecordCount contains the number of records in the database that matches the filters
                    count = RecordCount;
                    // Modified by RHR removed default order by Lane Name logic,  does not apply.  Users choose the sort order
                    retLanes = (from e in ltsView   select Models.vLELane365.selectModelData(e)).ToArray();
                }
                response = new Models.Response(retLanes, count);
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

        [HttpGet, ActionName("GetLELaneSummary")]
        public Models.Response GetLELaneSummary(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                if (id == 0) { id = this.readPagePrimaryKey(); }
                int count = 0;

                DAL.NGLLaneData oDAL = new DAL.NGLLaneData(Parameters);
                LTS.vLaneLESummary oData = oDAL.GetLELaneSummary(id);
                LTS.vLaneLESummary[] records = new LTS.vLaneLESummary[] { };
                if (oData != null)
                {
                    records = new LTS.vLaneLESummary[1] { oData };
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


        //GETS A LOOKUPLIST OF THE GetTimeZones
        [HttpGet, ActionName("GetTimeZones")]
        public Models.Response GetTimeZones(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                List <UTC.TimeZoneInfoDto> lTimeZones = UTC.clsApplication.GetTimeZoneList();
                response = new Models.Response(lTimeZones.ToArray(), lTimeZones.Count());               
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

        [HttpPost, ActionName("InsertOrUpdateLane")]
        public Models.Response InsertOrUpdateLane([System.Web.Http.FromBody]Models.vLELane365 l)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                BLL.NGLTMS365BLL bll = new BLL.NGLTMS365BLL(Parameters);
                DAL.Models.ResultObject result = new DAL.Models.ResultObject();
                LTS.vLELane365 ltsLane = Models.vLELane365.selectLTSData(l);
                if (l.LaneControl != 0)
                {
                    ltsLane.ReferenceLaneNumber = l.ReferenceLaneNumber;
                }
                else
                {
                    ltsLane.ReferenceLaneNumber = l.ReferenceLaneNumber;
                }
                ltsLane.LaneDestEmergencyContactPhone = Utilities.removeNonNumericText(ltsLane.LaneDestEmergencyContactPhone);
                ltsLane.LaneDestContactPhone = Utilities.removeNonNumericText(ltsLane.LaneDestContactPhone);
                ltsLane.LaneOrigEmergencyContactPhone = Utilities.removeNonNumericText(ltsLane.LaneOrigEmergencyContactPhone);
                ltsLane.LaneOrigEmergencyContactPhone = Utilities.removeNonNumericText(ltsLane.LaneOrigEmergencyContactPhone);
                ltsLane.LaneCarrierBookApptviaTokenFailPhone = Utilities.removeNonNumericText(ltsLane.LaneCarrierBookApptviaTokenFailPhone);

                ltsLane.LaneCarrierBookApptviaTokenFailPhone = Utilities.removeNonNumericText(ltsLane.LaneCarrierBookApptviaTokenFailPhone);
                bool blnInsert = false;
                if(ltsLane.LaneControl == 0) { blnInsert = true; }
                result = NGLLaneData.InsertOrUpdateLane365(ltsLane, l.LTTransType);
                if(result != null && result.Success == true)
                {
                    int laneControl = result.Control;
                    if (blnInsert && laneControl != 0) { result = new DAL.Models.ResultObject(); result = bll.CalculateAndSaveLaneMilesLatLong(laneControl); }
                }
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

        
        [HttpGet, ActionName("RecalculateLatLongMiles")]
        public Models.Response RecalculateLatLongMiles(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
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

        [HttpDelete, ActionName("DeleteLane")]
        public Models.Response DeleteLane(int id)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                NGLLaneData.DeleteLane365(id);
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