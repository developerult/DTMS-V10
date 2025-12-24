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
    public class LECompCarrierController : NGLControllerBase
    {
        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.2 on 10/09/2018 
        /// initializes the Page property by calling the base class constructor
        /// Not sure if Utilities.PageEnum.CompanyMaint for the child data 
        /// Comp Contact will cause any trouble.  Some testing may be required
        /// </summary>
        public LECompCarrierController()
                : base(Utilities.PageEnum.CompanyMaint)
	     {
        }

        #endregion
        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.LECompCarrierController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        private Models.LECompCar selectModelData(LTS.vLECompCar d)
        {
            Models.LECompCar modelRecord = new Models.LECompCar();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "LECompCarUpdated"};
                string sMsg = "";
                modelRecord = (Models.LECompCar)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.LECompCarUpdated.ToArray()); }
            }

            return modelRecord;
        }

        public static LTS.vLECompCar selectLTSData(Models.LECompCar d)
        {
            LTS.vLECompCar ltsRecord = new LTS.vLECompCar();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "LECompCarUpdated" };
                string sMsg = "";
                ltsRecord = (LTS.vLECompCar)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.LECompCarUpdated = bupdated == null ? new byte[0] : bupdated;

                }
            }

            return ltsRecord;
        }

        public static LTS.tblLegalEntityCompCarrier selecttblLegalEntityCompCarrierData(Models.LECompCar d)
        {
            LTS.tblLegalEntityCompCarrier ltsRecord = new LTS.tblLegalEntityCompCarrier();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "LECompCarUpdated" };
                string sMsg = "";
                ltsRecord = (LTS.tblLegalEntityCompCarrier)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.LECompCarUpdated = bupdated == null ? new byte[0] : bupdated;

                }
            }

            return ltsRecord;
        }

        #endregion


        #region " REST Services"

        /// <summary>
        /// Gets one vLECompCar record using the LECompCarControl passed in as id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.2 on 10/09/2018
        ///   previous code to get the first carrier record filterd by the LECompCarCompControl
        ///   this method was modified to use the LECompCarControl for id  
        ///   to return theselected record
        /// </remarks>
        [HttpGet, ActionName("Get")]
        public Models.Response Get(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int RecordCount = 0;
                int count = 0;
                DAL.Models.AllFilters f = new DAL.Models.AllFilters { filterName = "LECompCarControl", filterValue = id.ToString() };

                DAL.NGLLECompCarData oDAL = new DAL.NGLLECompCarData(Parameters);
                Models.LECompCar[] records = new Models.LECompCar[] { };
                LTS.vLECompCar[] oData = new LTS.vLECompCar[] { };
                oData = oDAL.GetLECompCarsFiltered(ref RecordCount, f);
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
            DAL.Models.AllFilters f = new DAL.Models.AllFilters { filterName = "LECompCarCompControl", filterValue = id.ToString() };
            return GetLECompCarsFiltered(f);
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
            return GetLECompCarsFiltered(filter);
        }

        [HttpGet, ActionName("GetLECompCarsFiltered")]
        public Models.Response GetLECompCarsFiltered(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
             
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                return GetLECompCarsFiltered(f);               

            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
            }
            return response;
        }

        private Models.Response GetLECompCarsFiltered(DAL.Models.AllFilters f)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int RecordCount = 0;
                int count = 0;
                DAL.NGLLECompCarData oDAL = new DAL.NGLLECompCarData(Parameters);
                Models.LECompCar[] records = new Models.LECompCar[] { };
                LTS.vLECompCar[] oData = new LTS.vLECompCar[] { };

                oData = oDAL.GetLECompCarsFiltered(ref RecordCount, f);

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

        /// <summary>
        /// Insert or update vLECompCar data into tblLegalEntityCompCarrier table
        /// </summary>
        /// <param name="compcar"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.2 on 10/09/2018
        ///     we no longer support passing in LTS.tblLegalEntityCompCarrier when the source data 
        ///     is LTS.vLECompCar.  The post method copies the key data into a new
        ///     LTS.tblLegalEntityCompCarrier  validation on required fields must be performed
        ///     by the DAL.NGLLECompCarData.InsertOrUpdateLECompCar method
        /// </remarks>
        [HttpPost, ActionName("Post")]
        public Models.Response Post([System.Web.Http.FromBody]Models.LECompCar data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLLECompCarData oDAL = new DAL.NGLLECompCarData(Parameters);
                LTS.tblLegalEntityCompCarrier compcar = new LTS.tblLegalEntityCompCarrier();
                compcar = selecttblLegalEntityCompCarrierData(data);
                oDAL.InsertOrUpdateLECompCar(compcar);
                bool[] oRecords = new bool[1];

                oRecords[0] = true;

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

        //old version removed,  we no longer support passing in LTS.tblLegalEntityCompCarrier when the source data 
        // is LTS.vLECompCar
        //public Models.Response Post([System.Web.Http.FromBody]LTS.tblLegalEntityCompCarrier compcar)
        //{
        //    var response = new Models.Response(); //new HttpResponseMessage();
        //    if (!authenticateController(ref response)) { return response; }
        //    try
        //    {
        //        DAL.NGLLECompCarData oCompCar = new DAL.NGLLECompCarData(Parameters);

        //        oCompCar.InsertOrUpdateLECompCar(compcar);

        //        Array d = new bool[1] { true };
        //        response = new Models.Response(d, 1);
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


        [HttpDelete, ActionName("Delete")]
        public Models.Response Delete(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {               
                bool blnRet = true;
                if (id == 0)
                {
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    DAL.NGLLECompCarData oDAL = new DAL.NGLLECompCarData(Parameters);
                    blnRet = oDAL.DeleteLECompCar(id);
                }
                bool[] oRecords = new bool[1];
                oRecords[0] = blnRet;
                if (blnRet)
                {
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Errors = string.Format("Cannot delete the Company Carrier record with Control {0}", id.ToString());

                }
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