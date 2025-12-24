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
    public class PalletTypeController : NGLControllerBase
    {
        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.2 on 08/29/2018 initializes the Page property by calling the base class constructor
        /// </summary>
        public PalletTypeController()
                : base(Utilities.PageEnum.StaticListMaint)
	     {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.PalletTypeController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region "DataTranslation"        
        private Models.vPalletType selectModelData(LTS.PalletType d)
        {
            Models.vPalletType modelRecord = new Models.vPalletType();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "PalletTypeUpdated", "rowguid" };
                string sMsg = "";
                modelRecord = (Models.vPalletType)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                //When using Updated field with the DTO objects the data has already been converted to an array
                //so replace the LTL logic 
                //if (modelRecord != null) { modelRecord.setUpdated(d.LaneFeesUpdated.ToArray()); }
                // with the code below (just remove the ToArray
                if (modelRecord != null) { modelRecord.setUpdated(d.PalletTypeUpdated.ToArray()); }
            }

            return modelRecord;
        }
        #endregion

        #region " REST Services"

        [HttpGet, ActionName("Get")]
        public Models.Response Get(int id)
        {
            //Note: The id must always match a CarrTarControl associated with the select tariff 
            //the system looks up the last saved tariff pk for this user 
            //an invalid parent key Error is returned if the data does not match
            //If id is zero and no records are found the client should configure the page to add a new record.

            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
               
                int count = 0;

                DAL.NGLPalletTypeData oDAL = new DAL.NGLPalletTypeData(Parameters);
                LTS.PalletType oData = new LTS.PalletType { };
                oData = oDAL.GetPalletTypebyFiltered(id);
                Models.vPalletType[] records = new Models.vPalletType[1];
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

                DAL.NGLPalletTypeData oDAL = new DAL.NGLPalletTypeData(Parameters);
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);

                //save the page filter for the next time the page loads
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter); }
                f.ParentControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.StaticListMaint);
                Models.vPalletType[] records = new Models.vPalletType[] { };
                LTS.PalletType[] oData = new LTS.PalletType[] { };
                oData = oDAL. GetPalletTypeFiltered(f, ref RecordCount);
                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                    records = (from e in oData select selectModelData(e)).ToArray();
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

        [HttpPost, ActionName("Post")]
        public Models.Response Post([System.Web.Http.FromBody]LTS.PalletType data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLPalletTypeData oDAL = new DAL.NGLPalletTypeData(Parameters);
                bool blnRet = oDAL.SaveOrCreatePalletType(data);
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
                DAL.NGLPalletTypeData oDAL = new DAL.NGLPalletTypeData(Parameters);
                bool blnRet = oDAL.DeletePalletType(id);
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

        #region " public methods"


        #endregion
    }
}