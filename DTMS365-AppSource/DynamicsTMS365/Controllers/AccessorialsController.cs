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
    public class AccessorialsController : NGLControllerBase
    {

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.AccessorialsController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion


        #region " REST Services"

        /// <summary>
        /// Added by LVV on 6/15/18 for v-8.2 VSTS #337
        /// Replaces GetAccessorialsByCarrByLE()
        /// </summary>
        /// <returns></returns>
        [HttpGet, ActionName("GetAccessorialsByLegalEntityCarrier")]
        public Models.Response GetAccessorialsByLegalEntityCarrier()
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.Models.NGLAPIAccessorial[] retFees = new DAL.Models.NGLAPIAccessorial[] { };
                DAL.NGLLegalEntityCarrierData oLECar = new DAL.NGLLegalEntityCarrierData(Parameters);
                int RecordCount = 0;
                int count = 0;

                int skip = request["skip"] == null ? 0 : int.Parse(request["skip"]);
                int take = request["take"] == null ? 500 : int.Parse(request["take"]);
                string sortExpression = "Code Asc";
                string filterWhere = request["filter[filters][0][value]"];
                //string oOp = request["filter[filters][0][operator]"];

                retFees = oLECar.GetAccessorialsByLegalEntityCarrier(ref RecordCount, filterWhere, sortExpression, 1, 0, skip, take);

                if (retFees != null && retFees.Count() > 0)
                {
                    count = retFees.Length;
                }
                response = new Models.Response(retFees, count);
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
        /// Returns a list of accessorials used to generate the bid filtered by LoadTenderControl
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// returns an array of NGLAPIAccessorial model record
        /// </returns>
        /// <remarks>
        /// Created by RHR for v-8.1 on 4/11/2018
        ///     reads the tblLoadTenderBookAPIFee table records by using the LoadTenderConrol 
        ///     to lookup the associated LoadTenderBooking controls 
        /// </remarks>
        [HttpGet, ActionName("GetAccessorialsByLoadTender")]
        public Models.Response GetAccessorialsByLoadTender(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (id == 0) { response.populateDefaultInvalidFilterResponseMessage(); return response; }
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.Models.NGLAPIAccessorial[] retVal = new DAL.Models.NGLAPIAccessorial[] { };
                retVal = NGLLoadTenderData.GetAccessorialsByLoadTender(id);
                int count = 0;
                if (retVal != null && retVal.Count() > 0) { count = retVal.Length; }
                response = new Models.Response(retVal, count);
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

        //GettblNGLAPICode

        /// <summary>
        /// Returns a list of accessorials used to generate the bid filtered by LoadTenderControl
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// returns an array of NGLAPIAccessorial model record
        /// </returns>
        /// <remarks>
        /// Created by RHR for v-8.1 on 4/11/2018
        ///     reads the tblLoadTenderBookAPIFee table records by using the LoadTenderConrol 
        ///     to lookup the associated LoadTenderBooking controls 
        ///     NOTE:  this service is used for Lookup information 
        ///             and should not be used for CRUD operations
        /// </remarks>
        [HttpGet, ActionName("GetNGLAPICode")]
        public Models.Response GetNGLAPICode(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (id == 0) { response.populateDefaultInvalidFilterResponseMessage(); return response; }
            if (!authenticateController(ref response)) { return response; }
            try
            {
                LTS.tblNGLAPICode retVal = new LTS.tblNGLAPICode();
                Models.NGLAPICode[] records = new Models.NGLAPICode[1];               
                retVal = NGLLoadTenderData.GettblNGLAPICode(id);
                int count = 0;
                if (retVal != null && retVal.NACControl > 0) {
                    count = 1;
                    records[0] = new Models.NGLAPICode
                    {
                        NACCode = retVal.NACCode,
                        NACControl = retVal.NACControl,
                        NACDesc = retVal.NACDesc,
                        NACID = retVal.NACID,
                        NACName = retVal.NACName
                    };
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


        [HttpGet, ActionName("GetAccessorials")]
        public Models.Response GetAccessorials()
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DTO.tblAccessorial[] retFees = new DTO.tblAccessorial[] { };
                int count = 0;
                retFees = NGLtblAccessorialData.GettblAccessorialsFiltered();
                if (retFees?.Count() > 0) { count = retFees.Length; }
                response = new Models.Response(retFees, count);
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