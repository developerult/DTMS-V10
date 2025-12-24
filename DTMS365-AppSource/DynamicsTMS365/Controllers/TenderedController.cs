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
    public class TenderedController : NGLControllerBase
    {

        #region " Constructors "

        /// <summary>Created by RHR for v-8.2 on 08/29/2018 initializes the Page property by calling the base class constructor</summary>
        public TenderedController() : base(Utilities.PageEnum.Tendered) { }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.TenderedController";
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
        public Models.Response GetRecords(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateFilter(ref response, filter)) { return response; } //Verfiy that the filters object is not null
            return GetAllItems(filter);
        }

        [HttpGet, ActionName("GetAllItems")]
        public Models.Response GetAllItems(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            //if (string.IsNullOrWhiteSpace(filter)) { response.populateDefaultInvalidFilterResponseMessage(); return response; }
            if (!authenticateController(ref response)) { return response; }
            try
            {
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter, "TenderedFltr"); } //save the page filter for the next time the page loads
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                LTS.vTenderedOrder[] records = new LTS.vTenderedOrder[] { };
                int count = 0;
                int RecordCount = 0;
                //If BookCarrierContControl is populated send it over as AllFilters.ContactControl, else send BookCarrierControl over as AllFilters.CarrierControlFrom
                if (Parameters.UserCarrierContControl > 0) { f.ContactControl = Parameters.UserCarrierContControl; } else { f.CarrierControlFrom = Parameters.UserCarrierControl; }               
                LTS.vTenderedOrder[] oData = NGLBookData.GetTenderedOrders365(f, ref RecordCount);                
                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                    records = oData;
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
        /// Processes Accept or Reject information and returns success or fail with a message using GenericResult model
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost, ActionName("PostSave")]
        public Models.Response PostSave([System.Web.Http.FromBody]Models.AcceptorReject data)
        {
            var response = new Models.Response();
            if (data == null || data.BookControl == 0)
            {
                //todo: add error invalid request message
                return response;
            }
            if (!authenticateController(ref response)) { return response; }
            try
            {
                BLL.NGLBookBLL bllData = new BLL.NGLBookBLL(Parameters);
                BLL.NGLBookBLL.AcceptRejectEnum enmAR = BLL.NGLBookBLL.AcceptRejectEnum.Accepted;
                if (data.AcceptRejectCode == 1) { enmAR = BLL.NGLBookBLL.AcceptRejectEnum.Rejected; }
                if (data.AcceptRejectCode == 2) { enmAR = BLL.NGLBookBLL.AcceptRejectEnum.Expired; }
                int carrierControl = 0;
                int carrierContControl = 0;
                //if ther user is not a carrier get the carrier information from the record
                if (Parameters.IsUserCarrier == true)
                {
                    carrierControl = Parameters.UserCarrierControl;
                    carrierContControl = Parameters.UserCarrierContControl;
                }
                else
                {
                    carrierControl = data.CarrierControl;
                    carrierContControl = data.CarrierContControl;
                }
                DTO.WCFResults oResults = bllData.CarrierAcceptOrRejectLoad365(data.BookControl, enmAR, carrierContControl, carrierControl, data.BookTrackComment, Parameters.UserName);
                if (oResults != null)
                {
                    //TODO: add logic to localize any messages, warnings or errors
                    string strMsg = "";
                    if (oResults.Warnings != null && oResults.Warnings.Count() > 0) { strMsg = oResults.concatWarnings() + " "; }
                    if (oResults.Errors != null && oResults.Errors.Count() > 0) { strMsg += oResults.concatErrors(); }
                    if (!string.IsNullOrWhiteSpace(strMsg))
                    {                        
                        response.Errors = strMsg;
                    } else
                    {
                        if (oResults.Success == false) { response.Errors = "Unexpected Accept or Reject Failure. Please try again."; }
                    }
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



        #endregion
    }
}