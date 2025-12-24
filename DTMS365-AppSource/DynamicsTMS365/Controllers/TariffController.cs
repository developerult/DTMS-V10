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
    public class TariffController : NGLControllerBase
    {
        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.2 on 08/29/2018 initializes the Page property by calling the base class constructor
        /// </summary>
        public TariffController()
                : base(Utilities.PageEnum.Tariff)
	     { 
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.TariffController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        private Models.vCarrierContract selectModelData(LTS.vCarrierContract d)
        {
            Models.vCarrierContract modelRecord = new Models.vCarrierContract();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "CarrTarUpdated" };
                string sMsg = "";
                modelRecord = (Models.vCarrierContract)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.CarrTarUpdated.ToArray()); }
            }

            return modelRecord;
        }


        private LTS.vCarrierContract selectLTSData(Models.vCarrierContract d)
        {
            LTS.vCarrierContract ltsRecord = new LTS.vCarrierContract();
            if (d != null)
            {
                //List<string> skipObjs = new List<string> { "CarrTarUpdated", "CarrierTariffBreakPoints", "CarrierTariffMatrixes", "CarrierTariffDiscounts", "CarrierTariffFees", "CarrierTariffInterlines", "CarrierTariffMinCharges", "CarrierTariffNonServices", "CarrierTariffEquipments", "CarrierTariffMatrixBPs", "CarrierTariffClassXrefs", "CarrierTariffNoDriveDays", "CarrierTariffMinWeights", "CompRefCarrier", "Carrier" };
                List<string> skipObjs = new List<string> { "CarrTarUpdated" };
                string sMsg = "";
                ltsRecord = (LTS.vCarrierContract)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.CarrTarUpdated = bupdated == null ? new byte[0] : bupdated;

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
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int RecordCount = 0;
                int count = 0;
                DAL.Models.AllFilters f = new DAL.Models.AllFilters();
                f.filterName = "CarrTarControl";
                f.filterValue = id.ToString();
                DAL.NGLCarrTarContractData oDAL = new DAL.NGLCarrTarContractData(Parameters);
                Models.vCarrierContract[] records = new Models.vCarrierContract[] { };
                LTS.vCarrierContract[] oData = new LTS.vCarrierContract[] { };
                oData = oDAL.GetCarrTarContracts(f, ref RecordCount);
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

        [HttpGet, ActionName("GetCarrierTariffSummary")]
        public Models.Response GetCarrierTariffSummary(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                if (id == 0) { id = this.readPagePrimaryKey(); }
                int count = 0;

                DAL.NGLCarrTarContractData oDAL = new DAL.NGLCarrTarContractData(Parameters);
                LTS.vCarrierTariffSummary oData = oDAL.GetCarrTarContractSummary(id);
                LTS.vCarrierTariffSummary[] records = new LTS.vCarrierTariffSummary[] { };
                if (oData != null)
                {
                    records = new LTS.vCarrierTariffSummary[1] { oData };
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
                //Modified by RHR for v-8.2 on 6/5/2019 added logic to prevent saving 
                //user filters when one of the filter values is CarrTarControl
                //Generally this is due to a link from another page or from the desktop app
                //save the page filter for the next time the page loads
                //if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter); }               
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                //Modified by RHR for v-8.2 on 6/5/2019 added logic to prevent saving 
                if (!string.IsNullOrWhiteSpace(filter)  && (f != null && f.FilterValues.Length > 0))
                {
                    bool blnOKToSave = true;
                    foreach (DAL.Models.FilterDetails fv in f.FilterValues)
                    {
                        if (fv.filterName == "CarrTarControl")
                        {
                            blnOKToSave = false;
                            break;
                        }
                    }
                    if (blnOKToSave)
                    {
                        savePageFilters(filter);
                    }
                }
                DAL.NGLCarrTarContractData oDAL = new DAL.NGLCarrTarContractData(Parameters);
                Models.vCarrierContract[] records = new Models.vCarrierContract[] { };
                LTS.vCarrierContract[] oData = new LTS.vCarrierContract[] { };
                oData = oDAL.GetCarrTarContracts(f, ref RecordCount);
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
        public Models.Response Post([System.Web.Http.FromBody]Models.vCarrierContract data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLCarrTarContractData oDAL = new DAL.NGLCarrTarContractData(Parameters);
                LTS.vCarrierContract oChanges = selectLTSData(data);
                bool blnRet = oDAL.SaveCarrTarContract(oChanges);


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
                DAL.NGLCarrTarContractData oDAL = new DAL.NGLCarrTarContractData(Parameters);
                bool blnRet = oDAL.DeleteCarrTarContract(id);
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