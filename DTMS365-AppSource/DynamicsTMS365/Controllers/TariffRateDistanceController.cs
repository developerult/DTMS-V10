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
    public class TariffRateDistanceController : NGLControllerBase
    {
        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.2 on 09/06/2018 initializes the Page property by calling the base class constructor
        /// </summary>
        public TariffRateDistanceController()
                : base(Utilities.PageEnum.TariffRatesDistance)
	     {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.TariffRateController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        // no models using LTS direct
        private int GetLaneNumber(string laneNumber)
        {
            if (string.IsNullOrWhiteSpace(laneNumber) || laneNumber.ToLower() == "all")
                return 0;

            DAL.NGLLaneData oLane = new DAL.NGLLaneData(Parameters);
            int lane = oLane.GetLaneControlIfExist(laneNumber);
            if (lane < 1)
                lane = 0;

            return lane;
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
            //Note: The id must always match a CarrTarControl associated with the select tariff 
            //the system looks up the last saved tariff pk for this user 
            //an invalid parent key Error is returned if the data does not match
            //If id is zero and no records are found the client should configure the page to add a new record.

            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            try
            {
                int RecordCount = 0;
                int count = 0;
                DAL.Models.AllFilters f = new DAL.Models.AllFilters();
                if (id != 0)
                {
                    f.filterName = "CarrTarEquipMatControl";
                    f.filterValue = id.ToString();
                }
                //get the parent control
                f.ParentControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.Tariff);
                DAL.NGLCarrTarEquipMatData oDAL = new DAL.NGLCarrTarEquipMatData(Parameters);
               
                LTS.vCarrTarEquipMatPivotTruckLoad[] oData = new LTS.vCarrTarEquipMatPivotTruckLoad[] { };
                oData = oDAL.GetCarrTarEquipMatDistanceRates(f, ref RecordCount);
                if (oData == null )
                {
                    oData = new LTS.vCarrTarEquipMatPivotTruckLoad[] { };
                    count = 0;
                } else
                {
                    count = oData.Count();
                    if (RecordCount > count) { count = RecordCount; }
                }
               
                response = new Models.Response(oData, count);                
            }
            catch (Exception ex)
            {
                //Error handler
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }

            // return the HTTP Response.
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
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            try
            {
                int RecordCount = 0;
                int count = 0;
                //save the page filter for the next time the page loads
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter); }
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                f.ParentControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.Tariff);
                DAL.NGLCarrTarEquipMatData oDAL = new DAL.NGLCarrTarEquipMatData(Parameters);                
                LTS.vCarrTarEquipMatPivotTruckLoad[] oData = new LTS.vCarrTarEquipMatPivotTruckLoad[] { };
                oData = oDAL.GetCarrTarEquipMatDistanceRates(f, ref RecordCount);
                if (oData == null)
                {
                    oData = new LTS.vCarrTarEquipMatPivotTruckLoad[] { };
                    count = 0;
                }
                else
                {
                    count = oData.Count();                   
                    if (RecordCount > count) { count = RecordCount; }
                } 
                response = new Models.Response(oData, count);
            }
            catch (Exception ex)
            {
                //Error handler
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }

            // return the HTTP Response.
            return response;
        }

        [HttpPost, ActionName("Post")]
        public Models.Response Post([System.Web.Http.FromBody]LTS.vCarrTarEquipMatPivotTruckLoad data)
        {
            //create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLCarrTarEquipMatData oDAL = new DAL.NGLCarrTarEquipMatData(Parameters);
                bool blnRet = oDAL.SaveCarrTarEquipMatDistanceRate(data);                
                bool[] oRecords = new bool[1];
                oRecords[0] = blnRet; 
                response = new Models.Response(oRecords, 1);
            }
            catch (Exception ex)
            {
                //Error handler
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
            //create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLCarrTarEquipMatData oDAL = new DAL.NGLCarrTarEquipMatData(Parameters);                
                oDAL.DeletCarrTarEquipMatByControl(id);
                bool[] oRecords = new bool[1];
                oRecords[0] = true;               
                response = new Models.Response(oRecords, 1);
            }
            catch (Exception ex)
            {
                //Error handler
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }

            return response;
        }

        [HttpPost, ActionName("PostSpreadSheet")]
        public Models.Response PostSpreadSheet(LTS.vCarrTarEquipMatPivotTruckLoad[] data, int CarrTarEquipMatCarrTarControl, int CarrTarEquipMatCarrTarMatBPControl, string CarrTarEquipMatName, DateTime EffectiveDate, DateTime? EffectiveTo = null)
        {
            //create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                BLL.NGLTariffImportToolBLL oBLL = new BLL.NGLTariffImportToolBLL(Parameters);
                var importHeader = new DTO.RateImportHeader
                {
                    EffectiveDate = EffectiveDate,
                    EffectiveTo = EffectiveTo,
                    CarrTarControl = CarrTarEquipMatCarrTarControl,
                    CarrTarMatBPControl = CarrTarEquipMatCarrTarMatBPControl,
                    CarrTarEquipMatName = CarrTarEquipMatName
                };
                //Modified by RHR for v-8.5.0.002 on 01/05/2022 added where filter for empty or null country. (do not import empty rows country is required)
                //Modified by RHR for v-8.5.4.001 on 06/14/2023 new Origin Rate shop logic
                var importData = data.Select(x => new DTO.RateImportDetailData
                {
                    ClassTypeControl = x.CarrTarEquipMatClassTypeControl,
                    State = string.IsNullOrWhiteSpace(x.CarrTarEquipMatState) ? null : x.CarrTarEquipMatState,
                    Country = string.IsNullOrWhiteSpace(x.CarrTarEquipMatCountry) ? null : x.CarrTarEquipMatCountry,
                    City = string.IsNullOrWhiteSpace(x.CarrTarEquipMatCity) ? null : x.CarrTarEquipMatCity,
                    FromZip = x.CarrTarEquipMatFromZip,
                    ToZip = x.CarrTarEquipMatToZip,
                    Min = x.CarrTarEquipMatMin ?? default,
                    MaxDays = x.CarrTarEquipMatMaxDays,
                    Lane = GetLaneNumber(x.LaneNumber),
                    Val1 = x.Rate ?? 0,
                    CarrTarEquipMatOrigZip = x.CarrTarEquipMatOrigZip                    
                }).Where(f =>  !string.IsNullOrEmpty(f.Country)).ToList();
                var oRet = oBLL.ImportNewRates(importHeader, ref importData);

                if (oRet.Errors?.Count > 0)
                    throw new InvalidOperationException("Failed to import tariff rates");

                bool blnSavePK = savePagePrimaryKey(Parameters, Utilities.PageEnum.Tariff, oRet.Key);               
                bool[] oRecords = new bool[1] { true };
                response = new Models.Response(oRecords, 1);
            }
            catch (Exception ex)
            {
                //Error handler
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }

            return response;
        }

        [HttpGet, ActionName("GetCompleteRecord")]
        public Models.Response GetCompleteRecord()
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            try
            {
                int RecordCount = 0;
                int count = 0;

                DAL.Models.AllFilters f = new DAL.Models.AllFilters();
                f.ParentControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.Tariff);
                DAL.NGLCarrTarEquipMatData oDAL = new DAL.NGLCarrTarEquipMatData(Parameters);
                LTS.vCarrTarEquipMatPivotTruckLoad[] oData = new LTS.vCarrTarEquipMatPivotTruckLoad[] { };
                //Modified by RHR for v-8.5.0.002 on 01/05/2022 added skip and tak for up to 50,000 rows 
                f.skip = 0;
                f.take = 50000;
                oData = oDAL.GetCarrTarEquipMatDistanceRates(f, ref RecordCount);
                if (oData == null)
                {
                    oData = new LTS.vCarrTarEquipMatPivotTruckLoad[] { };
                    count = 0;
                }
                else
                {
                    count = oData.Count();
                    if (RecordCount > count) { count = RecordCount; }
                }
                response = new Models.Response(oData, count);
            }
            catch (Exception ex)
            {
                //Error handler
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }

            // return the HTTP Response.
            return response;
        }

        //Modified by RHR for v-8.5.0.002 on 01/05/2022 removed does not work as expected
        //use DeleteCarrTarEquipMatRates instead 

        //[HttpPost, ActionName("DeleteCarrTarEquipMatRecords")]
        //public Models.Response DeleteCarrTarEquipMatRecords([System.Web.Http.FromBody] string filter)
        //{
        //    //create a response message to send back
        //    var response = new Models.Response(); //new HttpResponseMessage();
        //    if (!authenticateController(ref response)) { return response; }
        //    try
        //    {
        //        string carrTarEquipMatControl = filter.ToString();
        //        DAL.NGLCarrTarEquipMatData oDAL = new DAL.NGLCarrTarEquipMatData(Parameters);
        //        bool blnRet = oDAL.DeleteCarrTarEquipMatRecords(carrTarEquipMatControl);
        //        bool[] oRecords = new bool[1];
        //        oRecords[0] = blnRet;
        //        response = new Models.Response(oRecords, 1);
        //    }
        //    catch (Exception ex)
        //    {
        //        FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
        //        response.StatusCode = fault.StatusCode;
        //        response.Errors = fault.formatMessage();
        //        return response;
        //    }

        //    return response;
        //}

        //Created by RHR for v-8.5.0.002 on 01/05/2022 pass CarrTarEquipControl (id) to new stored procedure
        [HttpDelete, ActionName("DeleteCarrTarEquipMatRates")]
        public Models.Response DeleteCarrTarEquipMatRates(int id)
        {
            //create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                
                DAL.NGLCarrTarEquipMatData oDAL = new DAL.NGLCarrTarEquipMatData(Parameters);
                bool blnRet = oDAL.DeleteCarrTarEquipMatRates(id);
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

        //Created by RHR for v-8.5.0.002 on 01/05/2022 pass CarrTarEquipControl (id) to new stored procedure
        [HttpDelete, ActionName("DeleteAllCarrTarEquipMatRates")]
        public Models.Response DeleteAllCarrTarEquipMatRates(int id)
        {
            //create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {

                DAL.NGLCarrTarEquipMatData oDAL = new DAL.NGLCarrTarEquipMatData(Parameters);
                bool blnRet = oDAL.DeleteCarrTarEquipMatRates(id,true);
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

        #region "public methods"

        public int GetPageControlForActiveTariff()
        {
            int iRet = (int)Utilities.PageEnum.TariffRatesDistance;

            if (Parameters.UserControl == 0)
            {
                Parameters = null; //reset the parameter
            }
            //get the rate type for this tariff
            DAL.NGLCarrTarEquipMatData oDAL = new DAL.NGLCarrTarEquipMatData(Parameters);
            // Modified by RHR for v-8.5.4.001 on 06/14/2023 new Origin Rate shop logic new pages 
            //  we now check both the rate type and the origin rating flag to determine the page to display
            LTS.vCarrTarEquipMatPivotTruckLoad oCarTarData = oDAL.GetCarrTarEquipMatTarRateTypeForActiveTariff();
            if (oCarTarData != null)
            {
                bool bOrigRating = oCarTarData.CarrTarEquipMultiOrigRating ?? false;
                int iRateType = oCarTarData.CarrTarEquipMatTarRateTypeControl;
                switch (iRateType)
                {
                    case 3:
                        if (bOrigRating) {                             
                            iRet = (int)Utilities.PageEnum.TariffOrginRatesClass;
                        }
                        else
                        {
                            iRet = (int)Utilities.PageEnum.TariffRatesClass;
                        }
                        break;
                    case 4:
                        if (bOrigRating)
                        {
                            iRet = (int)Utilities.PageEnum.TariffOrginRatesFlat;
                        }
                        else
                        {
                            iRet = (int)Utilities.PageEnum.TariffRatesFlat;
                        }
                        break;
                    case 5:
                        if(bOrigRating)
                        {
                            iRet = (int)Utilities.PageEnum.TariffOrginRatesUOM;
                        }
                        else
                        {
                            iRet = (int)Utilities.PageEnum.TariffRatesUOM;
                        }
                        break;
                    default:
                        if (bOrigRating)
                        {
                            iRet = (int)Utilities.PageEnum.TariffOrginRatesDistance;
                        }
                        else
                        {
                            iRet = (int)Utilities.PageEnum.TariffRatesDistance;
                        }
                        break;
                }

            }

            return iRet;
        }

        #endregion
    }
}