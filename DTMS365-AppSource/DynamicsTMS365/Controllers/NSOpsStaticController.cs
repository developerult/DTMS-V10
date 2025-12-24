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
    public class NSOpsStaticController : NGLControllerBase
    {
        #region " Constructors "

        /// <summary>
        /// Initializes the Page property by calling the base class constructor
        /// </summary>
        public NSOpsStaticController() : base(Utilities.PageEnum.NSOpsStatic) { }

        #endregion

        #region " Properties"

        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.NSOpsStaticController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        public static LTS.tblBid selectLTSData(Models.tblBid d)
        {
            LTS.tblBid ltsRecord = new LTS.tblBid();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "BidUpdated", "tblLoadTender", "_tblLoadTender", "tblBidStatusCode", "_tblBidStatusCode", "tblBidType", "_tblBidType" };
                string sMsg = "";
                ltsRecord = (LTS.tblBid)DTran.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.BidUpdated = bupdated == null ? new byte[0] : bupdated;
                }
            }
            return ltsRecord;
        }

        #endregion

        #region " REST Services"

        /// <summary>
        /// Inserts the Carrier's NEXTStop bid as a new record in the Bid Table
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.2 on 12/19/2018
        ///     Added code to normalize dispatching
        /// TODO:  When accepting any bid, especially a P44 bid we should show the users
        ///     a Dispatch dialog where final details can be enterd or the dispatch may fail
        ///     the system will try to lookup any missing data but may return errors if a bid
        ///     is not valid
        /// </remarks>
        [HttpPost, ActionName("AcceptNSBid")]
        public Models.Response AcceptNSBid([System.Web.Http.FromBody]Models.NSAcceptBid b)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }           
            try
            {
                BLL.NGLBookRevenueBLL oBookRevBLL = new BLL.NGLBookRevenueBLL(Parameters);
                DTO.WCFResults wcf = new DTO.WCFResults();
                if (b.LTBookControl != 0 && b.BidLoadTenderControl != 0 && b.BidControl != 0)
                {
                    // Make sure data has not changed since posting
                    var h = NGLLoadTenderData.HasLoadTenderChanged(b.BidLoadTenderControl);
                    if (h.HasChanged == true) { response.StatusCode = HttpStatusCode.Forbidden; response.Errors = h.RetMsg; return response; }
                    //Modified by RHR for v-8.2 on 12/19/2018
                    //convert the NSAcceptBid to a Dispatch model
                    //Note: for now the BLL will populate all the defaults
                    //      needed for dispatching like P44 requirements
                    //      later we should be able to pass DAL.Models.Dispatch data
                    //      directly to this method
                    DAL.Models.Dispatch oDispatch = new DAL.Models.Dispatch {
                        BidControl = b.BidControl,
                        BookControl = b.LTBookControl,
                        LoadTenderControl = b.BidLoadTenderControl,
                        DispatchBidType = b.BidBidTypeControl,
                        CarrierControl = b.BidCarrierControl,
                        LineHaul = b.BidLineHaul,
                        Fuel = b.BidFuelTotal,
                        FuelVariable = b.BidFuelVariable,
                        FuelUOM = b.BidFuelUOM,
                        TotalCost = String.Format("{0:C2}", b.BidTotalCost) ,
                        CarrTarEquipMatControl = b.BidBookCarrTarEquipMatControl,
                        CarrTarEquipControl = b.BidBookCarrTarEquipControl,
                        ModeTypeControl = b.BidBookModeTypeControl
                    } ;
                    //Accept the Bid based on BidType -- we use NextStop as default
                    DTO.tblLoadTender.BidTypeEnum bidType = DTO.tblLoadTender.BidTypeEnum.NextStop;
                    if (b.BidBidTypeControl == (int)DTO.tblLoadTender.BidTypeEnum.NGLTariff) { bidType = DTO.tblLoadTender.BidTypeEnum.NGLTariff; }
                    else if(b.BidBidTypeControl == (int)DTO.tblLoadTender.BidTypeEnum.P44) { bidType = DTO.tblLoadTender.BidTypeEnum.P44; }

                    wcf = oBookRevBLL.DispatchBid(ref oDispatch, DTO.tblLoadTender.LoadTenderTypeEnum.NextStop, bidType);
                }
                else
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Errors = string.Format("Accept Bid failed because BookControl {0}, BidLoadTenderControl {1}, and BidControl {2} all must be greater than 0. Source: NSOpsStaticController/AcceptNSBid", b.LTBookControl.ToString(), b.BidLoadTenderControl.ToString(), b.BidControl.ToString());
                    return response;
                }
                //Check for messages
                if (wcf != null && wcf.Warnings.Count > 0)
                {                  
                    if (wcf.Warnings.ContainsKey("W_ManualAutoAcceptNoTenderEmail")) { wcf.Warnings.Remove("W_ManualAutoAcceptNoTenderEmail"); } //We don't need to show this message to the user so if it is there remove it
                    //If we still have warnings then show them to the user
                    if (wcf.Warnings.Count > 0)
                    {
                        response.StatusCode = HttpStatusCode.InternalServerError;
                        string strWarnings = wcf.concatWarnings();
                        //string strErrors = wcf.concatErrors();
                        //string strMessages = wcf.concatMessage();
                        response.Errors = strWarnings;
                        return response;
                    }
                }
                bool[] oRecords = new bool[1] { true };
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

        /// <summary>
        /// Deletes the Posting to NEXTStop using the BookControl
        /// Used by Ops to delete a NEXTStop Posting
        /// </summary>
        /// <returns></returns>
        [HttpDelete, ActionName("DeleteNSLoad")]
        public Models.Response DeleteNSLoad(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                BLL.NGLDATBLL oBLL = new BLL.NGLDATBLL(Parameters);
                DTO.WCFResults wcfRes = new DTO.WCFResults();
                bool blnSuccess = false;
                wcfRes = oBLL.RunNEXTStopDeleteMethod(id);
                blnSuccess = wcfRes.Success;
                //Process the results
                if (blnSuccess)
                {
                    bool[] oRecords = new bool[1] { blnSuccess };
                    response = new Models.Response(oRecords, 1);
                }
                else
                {
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    if (wcfRes != null)
                    {
                        string strMsg = "";
                        if (wcfRes.Warnings != null && wcfRes.Warnings.Count() > 0) { strMsg = wcfRes.concatWarnings() + " "; }
                        if (wcfRes.Errors != null && wcfRes.Errors.Count() > 0) { strMsg += wcfRes.concatErrors(); }
                        if (!string.IsNullOrWhiteSpace(strMsg)) { response.Errors = strMsg; }
                    }
                    else { response.Errors = "Could not delete NEXTStop Posting."; }
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
        /// Deletes the Posting to NEXTStop using the LoadTenderControl
        /// SuperUsers can use this to delete records that have errors - aka the users 
        /// cannot delete the records, generally because the BookRevLoadTenderControl etc has 
        /// been wiped but the posting not deleted for whatever reason.
        /// Those conditions should not happens but this is in case
        /// Calls delete directly vs going through LoadBoardDeleteMethod
        /// </summary>
        /// <returns></returns>
        [HttpDelete, ActionName("SuperDeleteNSLoad")]
        public Models.Response SuperDeleteNSLoad(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DTO.DATResults wcfRes = new DTO.DATResults();
                int ltsc = (int)DTO.tblLoadTender.LoadTenderStatusCodeEnum.NStopDeleted;
                int bsc = (int)DTO.tblLoadTender.BidStatusCodeEnum.OpsDeletePost;
                wcfRes = NGLLoadTenderData.DeleteNextStopLoad(id, ltsc, bsc);
                if (wcfRes.Success)
                {
                    bool[] oRecords = new bool[1] { wcfRes.Success };
                    response = new Models.Response(oRecords, 1);
                }
                else
                {
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    if (wcfRes != null)
                    {
                        string strMsg = "";
                        if (wcfRes.Warnings != null && wcfRes.Warnings.Count() > 0) { strMsg = wcfRes.concatWarnings() + " "; }
                        if (wcfRes.Errors != null && wcfRes.Errors.Count() > 0) { strMsg += wcfRes.concatErrors(); }
                        if (!string.IsNullOrWhiteSpace(strMsg)) { response.Errors = strMsg; }
                    }
                    else { response.Errors = "Could not delete NEXTStop Posting."; }
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