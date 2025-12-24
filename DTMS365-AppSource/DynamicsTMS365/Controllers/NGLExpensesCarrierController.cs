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
using DTran = Ngl.Core.Utility.DataTransformation;

namespace DynamicsTMS365.Controllers
{
    public class NGLExpensesCarrierController : NGLControllerBase
    {
        #region " Constructors "

        /// <summary>Initializes the Page property by calling the base class constructor</summary>
        public NGLExpensesCarrierController() : base(Utilities.PageEnum.NGLExpenses) { }

        #endregion

        #region " Properties"

        /// <summary>This property is used for logging and error tracking</summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.NGLExpensesCarrierController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        ////private Models.vLELane365 selectModelData(LTS.vLELane365 d)
        ////{
        ////    Models.vLELane365 modelRecord = new Models.vLELane365();
        ////    if (d != null)
        ////    {
        ////        List<string> skipObjs = new List<string> { "LaneUpdated" };
        ////        string sMsg = "";
        ////        modelRecord = (Models.vLELane365)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
        ////        if (modelRecord != null) { modelRecord.setUpdated(d.LaneUpdated.ToArray()); }
        ////    }
        ////    return modelRecord;
        ////}

        ////private Models.Comp selectModelData(LTS.vLEComp365 d)
        ////{
        ////    Models.Comp modelRecord = new Models.Comp();
        ////    if (d != null)
        ////    {
        ////        List<string> skipObjs = new List<string> { "CompUpdated" };
        ////        string sMsg = "";
        ////        modelRecord = (Models.Comp)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
        ////        if (modelRecord != null) { modelRecord.setUpdated(d.CompUpdated.ToArray()); }
        ////    }
        ////    return modelRecord;
        ////}

        ////public static LTS.vLELane365 selectLTSData(Models.vLELane365 d)
        ////{
        ////    LTS.vLELane365 ltsRecord = new LTS.vLELane365();
        ////    if (d != null)
        ////    {
        ////        List<string> skipObjs = new List<string> { "LaneUpdated", "rowguid", "_Lane", "Lane", "_tblRouteType", "tblRouteType" };
        ////        string sMsg = "";
        ////        ltsRecord = (LTS.vLELane365)DTran.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
        ////        if (ltsRecord != null)
        ////        {
        ////            byte[] bupdated = d.getUpdated();
        ////            ltsRecord.LaneUpdated = bupdated == null ? new byte[0] : bupdated;
        ////        }
        ////    }
        ////    return ltsRecord;
        ////}

        #endregion

        #region " REST Services"
        /// POST 	/API/objectcontroller{data}  : Create a new object or Update a the current object if the control number exists
        /// GET 	/API/objectcontroller/{id}   : Get the object information where the control number = "id"
        /// PUT 	/API/objectcontroller/{data} : Update the object information stored in data
        /// DELETE	/API/objectcontroller/{id}   : Delete object where the control number = "id"

        //Not Currently Used
        [HttpGet, ActionName("Get")]
        public Models.Response Get(int id)
        {
            //Note: The id must always match a BookControl
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                //////if (id == 0) { id = readPagePrimaryKey(Parameters, Utilities.PageEnum.NGLExpenses); } //get the parent control
                ////DAL.Models.AllFilters f = new DAL.Models.AllFilters();
                ////f.filterName = "BookControl";
                ////f.filterValue = id.ToString();
                ////int RecordCount = 0;
                ////int count = 0;
                ////LTS.vNGLAccounting[] oData = NGLBookData.GetvNGLAccounting(ref RecordCount, f);
                ////if (oData != null && oData.Count() > 0)
                ////{
                ////    count = RecordCount;
                ////    //records = (from e in oData orderby e.CompName ascending select selectModelData(e)).ToArray();
                ////}
                ////response = new Models.Response(oData, 1);
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

        //Not Currently Used
        [HttpGet, ActionName("GetRecords")]
        public Models.Response GetRecords()
        {
            return GetRecords("");
        }

        //Not Currently Used
        [HttpGet, ActionName("GetRecords")]
        public Models.Response GetRecords(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateFilter(ref response, filter)) { return response; } //Verfiy that the filters object is not null
            return GetAllRecords(filter);
        }

        //Not Currently Used
        [HttpGet, ActionName("GetAllRecords")]
        public Models.Response GetAllRecords(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                ////if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter, "expensesGridFilter"); } //save the page filter for the next time the page loads
                ////DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                ////Models.AuditFilters d = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Models.AuditFilters>(f.Data);
                ////if (d == null) { response.populateDefaultInvalidFilterResponseMessage(); return response; }

                ////addToFilters(ref f, "BookCarrierControl", d.CarrierDDLValue);
                ////applyDefaultSort(ref f, "BookControl", false);

                ////LTS.vNGLAccounting[] oData = new LTS.vNGLAccounting[] { };
                ////int RecordCount = 0;
                ////int count = 0;
                ////oData = NGLBookData.GetvNGLAccounting(ref RecordCount, f);
                ////if (oData?.Count() > 0)
                ////{
                ////    count = oData.Count();
                ////    if (RecordCount > count) { count = RecordCount; }
                ////}
                ////response = new Models.Response(oData, count);
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
        /// Creates a New Expense Carrier record
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Post")]
        public Models.Response Post([FromBody]LTS.vCMAddCarrier data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                if (data == null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Errors = Utilities.getLocalizedMsg("E_CannotSaveNoData");
                    return response;
                }
                bool blnRet = false;
                if (!data.CarrierNumber.HasValue || data.CarrierNumber.Value == 0) { data.CarrierNumber = NGLCarrierData.GetNextCarrierNumber(); }
                DTO.Carrier carrier = new DTO.Carrier() {
                    CarrierName = data.CarrierName,
                    CarrierNumber = data.CarrierNumber ?? 0,
                    CarrierTypeCode = "X",
                    CarrierActive = true,
                    CarrierIgnoreTariff = true,
                    CarrierAllowWebTender = false,
                    CarrierQualInsuranceDate = DateTime.Parse("12/31/2099"),
                    CarrierQualQualified = true,
                    //CarrierSCAC,
                    //CarrierAlphaCode,
                    //CarrierLegalEntity,
                    CarrierStreetAddress1 = data.CarrierStreetAddress1,
                    CarrierStreetAddress2 = data.CarrierStreetAddress2,
                    CarrierStreetCity = data.CarrierStreetCity,
                    CarrierStreetState = data.CarrierStreetState,
                    CarrierStreetCountry = data.CarrierStreetCountry,
                    CarrierStreetZip = data.CarrierStreetZip,
                    CarrierMailAddress1 = data.CarrierMailAddress1,
                    CarrierMailAddress2 = data.CarrierMailAddress2,
                    CarrierMailAddress3 = data.CarrierMailAddress3,
                    CarrierMailCity = data.CarrierMailCity,
                    CarrierMailState = data.CarrierMailState,
                    CarrierMailCountry = data.CarrierMailCountry,
                    CarrierMailZip = data.CarrierMailZip
                    //Cast(0 as bit) as blnCopyStreetToMail,
                };
                var d = NGLCarrierData.CreateRecord(carrier);
                DTO.Carrier result = (DTO.Carrier)d;
                if (result != null)
                {              
                    if (result.CarrierControl != 0) {
                        blnRet = true;
                        if (!string.IsNullOrWhiteSpace(data.CarrierContName)) {
                            DTO.CarrierCont cont = new DTO.CarrierCont()
                            {
                                CarrierContCarrierControl = result.CarrierControl,
                                CarrierContName = data.CarrierContName,
                                CarrierContTitle = data.CarrierContTitle,
                                CarrierContactPhone = Utilities.removeNonNumericText(data.CarrierContactPhone),
                                CarrierContPhoneExt = data.CarrierContPhoneExt,
                                CarrierContactFax = Utilities.removeNonNumericText(data.CarrierContactFax),
                                CarrierContact800 = Utilities.removeNonNumericText(data.CarrierContact800),
                                CarrierContactEMail = data.CarrierContactEMail,
                                //CarrierContactDefault = data.CarrierContactDefault ?? true,
                                //CarrierContLECarControl = data.CarrierContLECarControl ?? 0,
                                //CarrierContSchedContact = data.CarrierContSchedContact ?? false
                            };
                            var c = NGLCarrierContData.CreateRecord(cont);
                        }
                    }
                }
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


        #endregion

    }
}