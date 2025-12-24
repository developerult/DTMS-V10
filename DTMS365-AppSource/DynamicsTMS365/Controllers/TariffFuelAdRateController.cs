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
    public class CarrierFuelAdRateData
    {
        public string CarrFuelAdRateControl { get; set; }
    }
    public class TariffFuelAdRateController : NGLControllerBase
    {
        public int[] newRecords;
        LTS.CarrierFuelAdRate[] CompleteData = new LTS.CarrierFuelAdRate[] { };

        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.2 on 08/29/2018 initializes the Page property by calling the base class constructor
        /// </summary>
        public TariffFuelAdRateController()
                : base(Utilities.PageEnum.TariffFuel)
	     {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.TariffFuelAdRateController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        private Models.CarrierFuelAdRate selectModelData(LTS.CarrierFuelAdRate d)
        {
            Models.CarrierFuelAdRate modelRecord = new Models.CarrierFuelAdRate();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "CarrFuelAdRatesUpdated", "CarrierFuelAddendum" };
                string sMsg = "";
                modelRecord = (Models.CarrierFuelAdRate)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.CarrFuelAdRatesUpdated.ToArray()); }
            }

            return modelRecord;
        }


        private LTS.CarrierFuelAdRate selectLTSData(Models.CarrierFuelAdRate d)
        {
            LTS.CarrierFuelAdRate ltsRecord = new LTS.CarrierFuelAdRate();
            if (d != null)
            {
                //List<string> skipObjs = new List<string> { "CarrTarUpdated", "CarrierTariffBreakPoints", "CarrierTariffMatrixes", "CarrierTariffDiscounts", "CarrierTariffFees", "CarrierTariffInterlines", "CarrierTariffMinCharges", "CarrierTariffNonServices", "CarrierTariffEquipments", "CarrierTariffMatrixBPs", "CarrierTariffClassXrefs", "CarrierTariffNoDriveDays", "CarrierTariffMinWeights", "CompRefCarrier", "Carrier" };
                List<string> skipObjs = new List<string> { "CarrFuelAdRatesUpdated", "CarrierFuelAddendum" };
                string sMsg = "";
                ltsRecord = (LTS.CarrierFuelAdRate)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.CarrFuelAdRatesUpdated = bupdated == null ? new byte[0] : bupdated;

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
            //Note: The id must always match a CarrFuelAdRatesControl associated with the select tariff 
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
                    f.filterName = "CarrFuelAdRatesControl";
                    f.filterValue = id.ToString();
                }
                //get the tariff control and set it as the parent control
                f.ParentControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.Tariff);
                DAL.NGLCarrierFuelAdRateData oDAL = new DAL.NGLCarrierFuelAdRateData(Parameters);
                Models.CarrierFuelAdRate[] records = new Models.CarrierFuelAdRate[] { };
                LTS.CarrierFuelAdRate[] oData = new LTS.CarrierFuelAdRate[] { };
                oData = oDAL.GetCarrierFuelAdRatesByTariff(f, ref RecordCount);
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
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);

                //save the page filter for the next time the page loads
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter, "CarrierFuelAdRtFilte"); }
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter); }
                f.ParentControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.Tariff);
                DAL.NGLCarrierFuelAdRateData oDAL = new DAL.NGLCarrierFuelAdRateData(Parameters);
                Models.CarrierFuelAdRate[] records = new Models.CarrierFuelAdRate[] { };
                LTS.CarrierFuelAdRate[] oData = new LTS.CarrierFuelAdRate[] { };
                oData = oDAL.GetCarrierFuelAdRatesByTariff(f, ref RecordCount);
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
        public Models.Response Post([System.Web.Http.FromBody]Models.CarrierFuelAdRate data)
        {
            //create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLCarrierFuelAdRateData oDAL = new DAL.NGLCarrierFuelAdRateData(Parameters);
                LTS.CarrierFuelAdRate oChanges = selectLTSData(data);
                bool blnRet = oDAL.SaveCarrierFuelAdRateByTariff(oChanges, readPagePrimaryKey(Parameters, Utilities.PageEnum.Tariff));
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
                DAL.NGLCarrierFuelAdRateData oDAL = new DAL.NGLCarrierFuelAdRateData(Parameters);
                Models.CarrierFuelAdRate[] records = new Models.CarrierFuelAdRate[] { };
                //LTS.CarrierFuelAdRate[] oData = new LTS.CarrierFuelAdRate[] { };
                CompleteData = oDAL.GetCarrierFuelAdRatesByTariff(f, ref RecordCount);
                if (CompleteData != null && CompleteData.Count() > 0)
                {
                    count = CompleteData.Count();
                    records = (from e in CompleteData select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }

                    
                }
                response = new Models.Response(records, count);
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

        [HttpPost, ActionName("PostSpreadSheet")]
        public Models.Response PostSpreadSheet(Models.CarrierFuelAdRate data)
        {
            //create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
          
            if (!authenticateController(ref response)) { return response; }
            try
            {
                if (data.CarrFuelAdRatesEffDateNum.ToString() != "0")
                {
                    DateTime dtEffDate = DateTime.FromOADate(Double.Parse(data.CarrFuelAdRatesEffDateNum.ToString()));
                    data.CarrFuelAdRatesEffDate = dtEffDate;
                }
                DAL.NGLCarrierFuelAdRateData oDAL = new DAL.NGLCarrierFuelAdRateData(Parameters);
                LTS.CarrierFuelAdRate oChanges = selectLTSData(data);
                bool blnRet = oDAL.SaveCarrierFuelAdRateByTariff(oChanges, readPagePrimaryKey(Parameters, Utilities.PageEnum.Tariff));
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
        public Models.Response DELETE(CarrierFuelAdRateData AdRate)
        {
            //create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLCarrierFuelAdRateData oDAL = new DAL.NGLCarrierFuelAdRateData(Parameters);
                bool blnRet = oDAL.DeleteCarrierFuelAdRate(Int32.Parse(AdRate.CarrFuelAdRateControl));
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

        [HttpDelete, ActionName("DeleteFuelRateAdExRecords")]
        public Models.Response DeleteFuelRateAdExRecords(CarrierFuelAdRateData AdRate)
        {
            //create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLCarrierFuelAdRateData oDAL = new DAL.NGLCarrierFuelAdRateData(Parameters);
                bool blnRet = oDAL.DeleteCarrierFuelAdRateRecords(AdRate.CarrFuelAdRateControl);
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

        #endregion


        #region " public methods"


        #endregion
    }
}