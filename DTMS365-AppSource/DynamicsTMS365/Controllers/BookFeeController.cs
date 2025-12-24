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
    public class BookFeeController : NGLControllerBase
    {

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.BookFeeController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion


        #region " REST Services"

        [HttpGet, ActionName("GetBookFeesPending")]
        public Models.Response GetBookFeesPending(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                LTS.vBookFeesPending[] bfp = new LTS.vBookFeesPending[] { };
                DAL.NGLBookFeePendingData oBook = new DAL.NGLBookFeePendingData(Parameters);
                int RecordCount = 0;
                int count = 0;
                LTS.vBookFeesPending[] res = oBook.GetvBookFeesPending(ref RecordCount, f);
                if (res != null && res.Length > 0) {
                    count = RecordCount;
                    bfp = res;
                    response = new Models.Response(bfp, count);
                }
                else
                {
                    response = new Models.Response(bfp, count);
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

        [HttpPost, ActionName("SaveBookFeesPendingValue")]
        public Models.Response SaveBookFeesPendingValue([System.Web.Http.FromBody]Models.GenericResult g)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLBookFeePendingData oBook = new DAL.NGLBookFeePendingData(Parameters);
                bool retVal = oBook.SaveBookFeesPendingValue(g.Control, g.decField1);
                if (retVal)
                {
                    Array d = new bool[1] { retVal };
                    response = new Models.Response(d, 1);
                }
                else
                {
                    //This should never happen -- the only reason for SaveBookFeesPendingValue to return false is if the control number passed in is invalid (or if there is an exception)
                    //** TODO LVV ** Localize these messages. Also, figure out better way to do this
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Errors = "Could not find record with BookFeesPendingControl: " + g.Control.ToString();
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
        /// Note:
        /// GenericResult.Control = BookFeesPendingControl
        /// GenericResult.intField1 = BookFeesPendingBookControl
        /// GenericResult.blnField = blnUnlockBFC
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.2.0.117 on 8/20/19
        ///   moved processing logic to NGLTMS365BLL.SettlementApproveFee
        /// </remarks>
        [HttpPost, ActionName("ApproveBookFeePending")]
        public Models.Response ApproveBookFeePending([System.Web.Http.FromBody]Models.GenericResult g)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {                
                BLL.NGLTMS365BLL bll = new BLL.NGLTMS365BLL(Parameters);
                string strErrMsg = bll.SettlementApproveFee(g.Control, g.intField1, g.blnField);
                if (!string.IsNullOrWhiteSpace(strErrMsg) && !strErrMsg.Contains("Success"))
                {
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Errors = strErrMsg;
                }
                else
                {
                    Array d = new bool[1] { true };
                    response = new Models.Response(d, 1);
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


        [HttpDelete, ActionName("DeleteSettlementBFP")]
        public Models.Response DeleteSettlementBFP(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLBookFeePendingData oBFP = new DAL.NGLBookFeePendingData(Parameters);
                string strMsg = "";
                var blnRes = oBFP.DeleteSettlementBFP(id, ref strMsg);
                if (blnRes)
                {
                    Array d = new bool[1] { blnRes };
                    response = new Models.Response(d, 1);
                }
                else
                {
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Errors = string.Format("Cannot delete the BookFeesPending record with Control {0}", id.ToString());
                    if (!string.IsNullOrWhiteSpace(strMsg)) { response.Errors = strMsg; }               
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
        /// Parameter id = BookControl
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, ActionName("GetBookFeesByBookControl")]
        public Models.Response GetBookFeesByBookControl(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                LTS.vBookFee[] fees = new LTS.vBookFee[] { };
                DAL.NGLBookFeeData oBook = new DAL.NGLBookFeeData(Parameters);
                int RecordCount = 0;
                int count = 0;
                int skip = request["skip"] == null ? 0 : int.Parse(request["skip"]);
                int take = request["take"] == null ? 100 : int.Parse(request["take"]);
                fees = oBook.GetvBookFeesFiltered(ref RecordCount, id, 1, 0, skip, take);
                if (fees != null && fees.Length > 0)
                {
                    count = fees.Length;
                }
                response = new Models.Response(fees, count);             
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
        /// Used on CarrierAccessorialApproval page as a datasource to the
        /// Carrier dropdown.
        /// Gets a list of Carriers associated with records in the BookFeesPendingGrid
        /// </summary>
        /// <returns></returns>
        [HttpGet, ActionName("GetvBFPApprovalCarriers")]
        public Models.Response GetvBFPApprovalCarriers()
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DTO.vLookupList[] res = new DTO.vLookupList[] { };
                DAL.NGLLookupDataProvider oLook = new DAL.NGLLookupDataProvider(Parameters);
                int RecordCount = 0;
                int count = 0;
                int skip = request["skip"] == null ? 0 : int.Parse(request["skip"]);
                int take = request["take"] == null ? 100 : int.Parse(request["take"]);
                res = oLook.GetvBFPApprovalCarriers(ref RecordCount, 1, 0, skip, take);
                if (res?.Length > 0)
                {
                    count = RecordCount;             
                }
                response = new Models.Response(res, count);
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



        [HttpGet, ActionName("GetSettlementFeesForSHID")]
        public Models.Response GetSettlementFeesForSHID(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLBookFeePendingData oBook = new DAL.NGLBookFeePendingData(Parameters);
                int count = 0;
                DAL.Models.SettlementFee[] res = oBook.GetSettlementFeesForSHID(filter,false);
                if (res != null && res.Length > 0)
                {
                    count = res.Length;
                }
                response = new Models.Response(res, count);
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