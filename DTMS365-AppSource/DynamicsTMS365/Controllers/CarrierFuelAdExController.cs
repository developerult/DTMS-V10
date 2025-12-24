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
    public class CarrierFuelAdExController : NGLControllerBase
    {
        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.2 on 08/29/2018 initializes the Page property by calling the base class constructor
        /// </summary>
        public CarrierFuelAdExController()
                : base(Utilities.PageEnum.CarrierFuel)
	     {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.CarrierFuelAdExController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        private Models.CarrierFuelAdEx selectModelData(LTS.CarrierFuelAdEx d)
        {
            Models.CarrierFuelAdEx modelRecord = new Models.CarrierFuelAdEx();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "CarrFuelAdExUpdated", "CarrierFuelAddendum" };
                string sMsg = "";
                modelRecord = (Models.CarrierFuelAdEx)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.CarrFuelAdExUpdated.ToArray()); }
            }

            return modelRecord;
        }


        private LTS.CarrierFuelAdEx selectLTSData(Models.CarrierFuelAdEx d)
        {
            LTS.CarrierFuelAdEx ltsRecord = new LTS.CarrierFuelAdEx();
            if (d != null)
            {
                //List<string> skipObjs = new List<string> { "CarrTarUpdated", "CarrierTariffBreakPoints", "CarrierTariffMatrixes", "CarrierTariffDiscounts", "CarrierTariffFees", "CarrierTariffInterlines", "CarrierTariffMinCharges", "CarrierTariffNonServices", "CarrierTariffEquipments", "CarrierTariffMatrixBPs", "CarrierTariffClassXrefs", "CarrierTariffNoDriveDays", "CarrierTariffMinWeights", "CompRefCarrier", "Carrier" };
                List<string> skipObjs = new List<string> { "CarrFuelAdExUpdated", "CarrierFuelAddendum" };
                string sMsg = "";
                ltsRecord = (LTS.CarrierFuelAdEx)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.CarrFuelAdExUpdated = bupdated == null ? new byte[0] : bupdated;

                }
            }

            return ltsRecord;
        }


        private LTS.CarrierFuelAdEx selectLTSData(Models.vCarrierFuelAdEx d)
        {
            LTS.CarrierFuelAdEx ltsRecord = new LTS.CarrierFuelAdEx();
            if (d != null)
            {
                //List<string> skipObjs = new List<string> { "CarrTarUpdated", "CarrierTariffBreakPoints", "CarrierTariffMatrixes", "CarrierTariffDiscounts", "CarrierTariffFees", "CarrierTariffInterlines", "CarrierTariffMinCharges", "CarrierTariffNonServices", "CarrierTariffEquipments", "CarrierTariffMatrixBPs", "CarrierTariffClassXrefs", "CarrierTariffNoDriveDays", "CarrierTariffMinWeights", "CompRefCarrier", "Carrier" };
                List<string> skipObjs = new List<string> { 
                    "CarrFuelAdExUpdated", 
                    "CarrierFuelAddendum",  
                    "CalcNatAverageLabel",
                    "CalcNatAverageValue",
                    "CalcAvgWithZone1Label",
                    "CalcAvgWithZone1Value",
                    "CalcAvgWithZone2Label",
                    "CalcAvgWithZone2Value",
                    "CalcAvgWithZone3Label",
                    "CalcAvgWithZone3Value",
                    "CalcAvgWithZone4Label",
                    "CalcAvgWithZone4Value",
                    "CalcAvgWithZone5Label",
                    "CalcAvgWithZone5Value",
                    "CalcAvgWithZone6Label",
                    "CalcAvgWithZone6Value",
                    "CalcAvgWithZone7Label",
                    "CalcAvgWithZone7Value",
                    "CalcAvgWithZone8Label",
                    "CalcAvgWithZone8Value",
                    "CalcAvgWithZone9Label",
                    "CalcAvgWithZone9Value",
                    "CarrFuelAdExModDate",
                    "CarrFuelAdExEffDate"};

                string sMsg = "";
                ltsRecord = (LTS.CarrierFuelAdEx)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.CarrFuelAdExUpdated = bupdated == null ? new byte[0] : bupdated;
                    ltsRecord.CarrFuelAdExModDate = Utilities.convertStringToDateTime(d.CarrFuelAdExModDate);
                    ltsRecord.CarrFuelAdExEffDate = Utilities.convertStringToDateTime(d.CarrFuelAdExEffDate);


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
            //Note: The id must always match a CarrFuelAdExControl associated with the select tariff 
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
                    f.filterName = "CarrFuelAdExControl";
                    f.filterValue = id.ToString();
                }
                //get the tariff control and set it as the parent control
                f.ParentControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.CarrierFuel);
                DAL.NGLCarrierFuelAdExData oDAL = new DAL.NGLCarrierFuelAdExData(Parameters);
                Models.vCarrierFuelAdEx[] records = new Models.vCarrierFuelAdEx[] { };
                LTS.vCarrierFuelAdEx[] oData = new LTS.vCarrierFuelAdEx[] { };
                if (f.ParentControl == 0)
                {
                    response = new Models.Response(records, 0);
                    return response;
                }
                oData = oDAL.GetvCarrierFuelAdEx(f, ref RecordCount);
                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                    records = (from e in oData select Models.vCarrierFuelAdEx.selectModelData(e)).ToArray();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.5.4.006  added logic to get the Legal Entity Carrier Data 
        ///     before reading the Rate because this method is called before the CarrierFuelAdRtFilte is populated
        /// </remarks>
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



                Models.vCarrierFuelAdEx[] records = new Models.vCarrierFuelAdEx[] { };

                //get the  selected legal entity  carrier control
                int iLECarControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.LegalEntityCarrierMaint);
                DAL.NGLCarrierFuelAddendumData oParentDAL = new DAL.NGLCarrierFuelAddendumData(Parameters);
                LTS.CarrierFuelAddendum[] oParentData = new LTS.CarrierFuelAddendum[] { };
                oParentData = oParentDAL.GetCarrierFuelAddendumByLECarrierControl(iLECarControl); //GetCarrierTariffFuelAddendum
                if (oParentData != null && oParentData.Count() > 0)
                {
                    // get the CarrFuelAdRatesCarrFuelAdControl only the first record returned is used
                    // only one record should be returned
                    int iCarrFuelAdRatesCarrFuelAdControl = oParentData[0].CarrFuelAdControl;

                    //save the page filter for the next time the page loads
                    if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter, "CarrierFuelAdExFilte"); }
                    if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter); }
                    f.ParentControl = iCarrFuelAdRatesCarrFuelAdControl;
                    DAL.NGLCarrierFuelAdExData oDAL = new DAL.NGLCarrierFuelAdExData(Parameters);
                    if (f.ParentControl == 0)
                    {
                        //this normally a new record so just return no data
                        response = new Models.Response(records, 0);
                        return response;
                    }
                    LTS.vCarrierFuelAdEx[] oData = new LTS.vCarrierFuelAdEx[] { };
                    oData = oDAL.GetvCarrierFuelAdEx(f, ref RecordCount); // GetCarrierFuelAdExByTariff
                    if (oData != null && oData.Count() > 0)
                    {
                        count = oData.Count();
                        records = (from e in oData select Models.vCarrierFuelAdEx.selectModelData(e)).ToArray();
                        if (RecordCount > count) { count = RecordCount; }
                    }
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
        public Models.Response Post([System.Web.Http.FromBody]Models.vCarrierFuelAdEx data)
        {
            //create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLCarrierFuelAdExData oDAL = new DAL.NGLCarrierFuelAdExData(Parameters);
                LTS.CarrierFuelAdEx oChanges = selectLTSData(data);
                int carrFuelAdControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.CarrierFuel);
                bool[] oRecords = new bool[1];
                if (carrFuelAdControl == 0)
                {                   
                    oRecords[0] = false;
                    response = new Models.Response(oRecords, 0);
                    return response;
                }
                bool blnRet = oDAL.SaveCarrierFuelAdEx(oChanges, carrFuelAdControl); //SaveCarrierFuelAdExByTariff , , readPagePrimaryKey(Parameters, Utilities.PageEnum.CarrierFuel)
               
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
                DAL.NGLCarrierFuelAdExData oDAL = new DAL.NGLCarrierFuelAdExData(Parameters);
                bool blnRet = oDAL.DeleteCarrierFuelAdEx(id);
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