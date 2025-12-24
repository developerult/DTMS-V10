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

namespace DynamicsTMS365.Controllers
{
    public class LegalEntityCarrierController : NGLControllerBase
    {
        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.2 on 10/09/2018 
        /// initializes the Page property by calling the base class constructor
        /// Not sure if Utilities.PageEnum.CompanyMaint for the child data 
        /// Comp Contact will cause any trouble.  Some testing may be required
        /// </summary>
        public LegalEntityCarrierController()
                : base(Utilities.PageEnum.LegalEntityCarrierMaint)
	     {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.LegalEntityCarrierController";
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
            return GetAllRecords(filter);
        }

        [HttpGet, ActionName("GetAllRecords")]
        public Models.Response GetAllRecords(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                LTS.vLegalEntityCarrierByLE[] oData = new LTS.vLegalEntityCarrierByLE[] { };
                int RecordCount = 0;
                int count = 0;

                //if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter); }
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);                
                f.BookControl = Convert.ToInt32(f.FilterValues.Where(x => x.filterName == "CarrierControl").Select(y => y.filterValueFrom).FirstOrDefault());
                if (f.BookControl != 0)
                {                    
                    oData = NGLLegalEntityCarrierData.GetLegalEntityCarriersByCarrier(ref RecordCount, f).Where(x => x.CarrierControl == f.BookControl).ToArray();
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter, "LECarSetFilter"); } //save the page filter for the next time the page loads
                    oData = NGLLegalEntityCarrierData.GetLegalEntityCarriersByLE(ref RecordCount, f);
                }
                if (oData?.Count () > 0)
                {
                    count = oData.Count();
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



        [HttpGet, ActionName("GetLegalEntityCarriersByLE")]
        public Models.Response GetLegalEntityCarriersByLE(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.Models.AllFilters f = new DAL.Models.AllFilters() { LEAdminControl = id };
                LTS.vLegalEntityCarrierByLE[] ltsView = new LTS.vLegalEntityCarrierByLE[] { };
                int RecordCount = 0;
                ltsView = NGLLegalEntityCarrierData.GetLegalEntityCarriersByLE(ref RecordCount, f);
                if (ltsView == null) { ltsView = new LTS.vLegalEntityCarrierByLE[] { }; }
                response = new Models.Response(ltsView, RecordCount);
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
        /// 
        /// </summary>
        /// <param name="cds"></param>
        /// <returns></returns>
        /// <remarks>
        ///  Modified by RHR for v-8.4 on 4/22/2021
        /// </remarks>
        [HttpPost, ActionName("SaveLegalEntityCarrier")]
        public Models.Response SaveLegalEntityCarrier([System.Web.Http.FromBody]Models.CarrierDispatchSettings cds)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                //**TODO LVV** ADD LOGIC TO CHECK IF THE USER HAS ROLE CENTER PERMISSIONS TO RUN THIS PROCEDURE
                if (cds.LEAdminControl == 0) { cds.LEAdminControl = Parameters.UserLEControl; }
                //Modified by RHR for v - 8.4.0.002 on 04 / 27 / 2021 Added Fields for Token Accept Reject      
                    var spRet = NGLLegalEntityCarrierData.InsertOrUpdateLegalEntityCarrier(cds.LEAdminControl, cds.CarrierControl, cds.DispatchTypeControl, cds.RateShopOnly, cds.APIDispatching, 
                    cds.APIStatusUpdates, cds.ShowAuditFailReason, cds.ShowPendingFeeFailReason, cds.BillToCompControl, cds.CarrierAccountRef, 
                    cds.LECarUseDefault, cds.LECarExpiredLoadsTo, cds.LECarExpiredLoadsCc, cds.LECarCarrierAcceptLoadMins,
                    cds.LECarBillingAddress1, cds.LECarBillingAddress2, cds.LECarBillingAddress3, cds.LECarBillingCity, cds.LECarBillingState, 
                    cds.LECarBillingZip, cds.LECarBillingCountry, cds.LECarAllowLTLConsolidation, cds.LECarAllowCarrierAcceptRejectByEmail, 
                    cds.LECarCarrierAuthCarrierAcceptRejectByEmail, cds.LECarCarrierAuthCarrierAcceptRejectExpMin,
                    cds.LECarWillDriveSunday, cds.LECarWillDriveSaturday, cds.LECarUpliftUseCarrierSpecific, cds.LECarCarrierSpecificUpliftPerc);


                if (spRet.ErrNumber == 0)
                {
                    int lecControl = 0;
                    if (spRet.LECarControl.HasValue) { lecControl = spRet.LECarControl.Value; }
                    Array d = new int[1] { lecControl };
                    response = new Models.Response(d, 1);
                }
                else
                {
                    //** TODO LVV ** Localize these messages. Also, figure out better way to do this
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Errors = spRet.RetMsg;
                }
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

        [HttpDelete, ActionName("DeleteLegalEntityCarrier")]
        public Models.Response DeleteLegalEntityCarrier(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            //bool blnSuccess = false;
            try
            {
                //if (id == 0) { response.StatusCode = HttpStatusCode.OK; return response; }
                //**TODO LVV** ADD LOGIC TO CHECK IF THE USER HAS ROLE CENTER PERMISSIONS TO RUN THIS PROCEDURE
                bool blnSuccess = NGLLegalEntityCarrierData.DeleteLegalEntityCarrier(id);

                if (blnSuccess)
                {
                    //response.StatusCode = HttpStatusCode.OK;
                    bool[] oRecords = new bool[1] { blnSuccess };
                    response = new Models.Response(oRecords, 1);
                }
                else
                {
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Errors = string.Format("Cannot delete the tblLegalEntityCarrier record with Control {0}", id.ToString());
                }
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
        ///  GenericResult.Control = LECarControl
        ///  GenericResult.intField1 = BillToCompControl;
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        [HttpPost, ActionName("SetLECarrierBillToComp")]
        public Models.Response SetLECarrierBillToComp([System.Web.Http.FromBody]Models.GenericResult g)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                var blnRet = NGLLegalEntityCarrierData.SetLECarrierBillToComp(g.Control, g.intField1);
                //This method only ever returns false if an exception occurs, and this would be captured by the error handler
                //so really the returned value in response does not matter to the ajax code
                Array d = new bool[1] { blnRet };
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
        /// <summary>
        /// Method to Get Carrier Summary 
        /// </summary>
        /// <param name="id">Carrier PK</param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.5.4.003 on 10/06/2023
        ///     fixed bug where wrong LECarControl filter was not being applied correctly
        /// </remarks>
        [HttpGet, ActionName("GetCarrierSummary")]
        public Models.Response GetCarrierSummary(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.Models.AllFilters f = new DAL.Models.AllFilters() { LEAdminControl = Parameters.UserLEControl };
                //Modified by RHR for v - 8.5.4.003 on 10 / 06 / 2023
                if (id == 0)
                {
                    id = readPagePrimaryKey(Parameters, Utilities.PageEnum.LegalEntityCarrierMaint);
                }
                
                f.filterName = "LECarControl";
                f.filterValue = id.ToString();
                LTS.vLegalEntityCarrierByLE[] ltsView = new LTS.vLegalEntityCarrierByLE[] { };
                int RecordCount = 1;
                ltsView = NGLLegalEntityCarrierData.GetLegalEntityCarriersByLE(ref RecordCount, f); 
                if (ltsView == null) { ltsView = new LTS.vLegalEntityCarrierByLE[] { }; }
                response = new Models.Response(ltsView, RecordCount);
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