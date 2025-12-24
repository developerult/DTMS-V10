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

//Added By LVV on 1/4/17 for v-8.0 Next Stop

namespace DynamicsTMS365.Controllers
{
    public class tblBidController : NGLControllerBase
    {
        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.tblBidController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        public static Models.tblBid selectModelData(LTS.tblBid d)
        {
            Models.tblBid nsBid = new Models.tblBid();
            List<string> skipObjs = new List<string> { "BidUpdated", "tblLoadTender", "_tblLoadTender", "tblBidStatusCode", "_tblBidStatusCode", "tblBidType", "_tblBidType" };
            string sMsg = "";
            nsBid = (Models.tblBid)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(nsBid, d, skipObjs, ref sMsg);
            return nsBid;
        }

        public static LTS.tblBid selectLTSData(Models.tblBid d)
        {
            LTS.tblBid ltsRecord = new LTS.tblBid();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "BidUpdated", "tblLoadTender", "_tblLoadTender", "tblBidStatusCode", "_tblBidStatusCode", "tblBidType", "_tblBidType" };
                string sMsg = "";
                ltsRecord = (LTS.tblBid)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
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
        /// This method only returns Active NEXTStop bids for a specific carrier (the carrier is the logged in user)
        /// BidStatusCode = Active AND BidCarrierControl = CarrierControl  AND BidBidTypeControl = NEXTStop
        /// Used on the NextStopCarrier page to display the Carrier's Active Bids that are pending acceptance by Ops
        /// </summary>
        /// <returns></returns>
        [ActionName("GetBids")]
        public Models.Response GetBids()
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            try
            {
                Models.tblBid[] nsBids = new Models.tblBid[] { };

 
                int skip = request["skip"] == null ? 0 : int.Parse(request["skip"]);
                int take = request["take"] == null ? 100 : int.Parse(request["take"]);
                int count = 0;
                int RecordCount = 0;
                int CarrierControl = 0;
                string strMsg = "";
                string filterWhere = "";
                string sortExpression = "";
                int bsc = (int)DTO.tblLoadTender.BidStatusCodeEnum.Active;
                int btype = (int)DTO.tblLoadTender.BidTypeEnum.NextStop;

                //Get the CarrierControl associated with the User account
                CarrierControl = Parameters.UserCarrierControl;               
                if (CarrierControl != 0)
                {
                    //We only want the carrier to see the bids they made on NEXTStop aka BidTypeControl 1 
                    filterWhere = "(BidStatusCode = " + bsc + ") AND (BidCarrierControl = " + CarrierControl + ")" + " AND (BidBidTypeControl = " + btype + ")";
                    sortExpression = "BidControl Asc";

                    LTS.tblBid[] ltsBids = NGLBidData.GetBids(ref RecordCount,
                                                                       filterWhere,
                                                                       sortExpression,
                                                                       1,
                                                                       0,
                                                                       skip,
                                                                       take);

                    if (ltsBids != null && ltsBids.Count() > 0)
                    {
                        //RecordCount contains the nunber of records in the database that matches the filters
                        count = RecordCount;
                        nsBids = (from e in ltsBids
                                  orderby e.BidControl ascending
                                  select selectModelData(e)).ToArray();
                    }

                    response = new Models.Response(nsBids, count);
                }
                else
                {
                    //Error - No Carrier Associated with user
                    response.StatusCode = HttpStatusCode.Unauthorized;
                    response.Errors = "No Carrier found for user account.";
                    if (!string.IsNullOrWhiteSpace(strMsg))
                    {
                        response.Errors = strMsg;
                    }
                }              
               
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

        /// <summary>
        /// Returns an array of LTS.tblBid data objects representing  
        /// Next Stop Bids that are Archived and that do not have BidStatusCode Active
        /// Additional filters may be applied
        /// </summary>
        /// <returns></returns>
        [ActionName("GetHistoricalBids")]
        public Models.Response GetHistoricalBids(int id)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            try
            {
                Models.tblBid[] nsBids = new Models.tblBid[] { };
                int count = 0;
                int RecordCount = 0;
                int CarrierControl = 0;
                string strMsg = "";
                string filterWhere = "";

                DAL.NGLSecurityDataProvider oSec = new DAL.NGLSecurityDataProvider(Parameters);
                DAL.NGLBidData dalBidData = new DAL.NGLBidData(Parameters);

                // get the take and skip parameters int skip = request["skip"] == null ? 0 :
                int skip = request["skip"] == null ? 0 : int.Parse(request["skip"]);
                int take = request["take"] == null ? 100 : int.Parse(request["take"]);               

                
                if (id != 0)
                {
                    //CARRIER 
                    //Get the CarrierControl associated with the User account
                    CarrierControl = Parameters.UserCarrierControl;                  
                    if (CarrierControl != 0)
                    {
                        //we want to see Bids from this CarrierControl with BidType NEXTStop
                        filterWhere = "(BidCarrierControl = " + CarrierControl + ")" + " AND (BidBidTypeControl = 1)";
                    }
                    else
                    {
                        //Error - No Carrier Associated with user
                        response.StatusCode = HttpStatusCode.Unauthorized;
                        response.Errors = "No Carrier found for user account.";
                        if (!string.IsNullOrWhiteSpace(strMsg))
                        {                        
                            response.Errors = strMsg;
                        }
                        return response;
                    }                  
                }
                else
                {
                    //OPERATIONS
                    filterWhere = "";
                }

                string sortExpression = "BidControl Asc";
            
                LTS.tblBid[] ltsBids = dalBidData.GetNSHisoricalBids(ref RecordCount,
                                                                        filterWhere,
                                                                        sortExpression,
                                                                        1,
                                                                        0,
                                                                        skip,
                                                                        take);


                if (ltsBids != null && ltsBids.Count() > 0)
                {
                    //RecordCount contains the nunber of records in the database that matches the filters
                    count = RecordCount;
                    nsBids = (from e in ltsBids
                              orderby e.BidControl ascending
                              select selectModelData(e)).ToArray();
                }

                response = new Models.Response(nsBids, count);
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

        /// <summary>
        /// This method returns a bid with BidStatusCode = Active and BidLoadTenderControl = id
        /// if it exists. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ActionName("GetActiveBidById")]
        public Models.Response GetActiveBidById(string id)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            try
            {
                Models.tblBid[] nsBids = new Models.tblBid[] { };
                int count = 0;

                // get the take and skip parameters int skip = request["skip"] == null ? 0 :
                int skip = request["skip"] == null ? 0 : int.Parse(request["skip"]);
                int take = request["take"] == null ? 100 : int.Parse(request["take"]);

                //Filtered Parameters  
                int RecordCount = 0;

                int bsc = (int)DTO.tblLoadTender.BidStatusCodeEnum.Active;

                string filterWhere = "(BidStatusCode = " + bsc + ") AND (BidLoadTenderControl = " + id + ")";
                string sortExpression = "BidTotalCost Asc";

                DAL.NGLBidData dalBidData = new DAL.NGLBidData(Parameters);               
                LTS.tblBid[] ltsBids = dalBidData.GetBids(ref RecordCount,
                                                                        filterWhere,
                                                                        sortExpression,
                                                                        1,
                                                                        0,
                                                                        skip,
                                                                        take);


                if (ltsBids != null && ltsBids.Count() > 0)
                {
                    //RecordCount contains the nunber of records in the database that matches the filters
                    count = RecordCount;
                    nsBids = (from e in ltsBids
                              orderby e.BidTotalCost ascending
                              select selectModelData(e)).ToArray();
                }

                response = new Models.Response(nsBids, count);
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

        /// <summary>
        /// This method returns all bids with BidLoadTenderControl = id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ActionName("GetBidsById")]
        public Models.Response GetBidsById(string id)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            try
            {
                Models.tblBid[] nsBids = new Models.tblBid[] { };
                int count = 0;

                // get the take and skip parameters int skip = request["skip"] == null ? 0 :
                int skip = request["skip"] == null ? 0 : int.Parse(request["skip"]);
                int take = request["take"] == null ? 100 : int.Parse(request["take"]);

                //Filtered Parameters  
                int RecordCount = 0;


                string filterWhere = "(BidLoadTenderControl = " + id + ")";
                string sortExpression = "BidTotalCost Asc";

                DAL.NGLBidData dalBidData = new DAL.NGLBidData(Parameters);               
                LTS.tblBid[] ltsBids = dalBidData.GetBids(ref RecordCount,
                                                                        filterWhere,
                                                                        sortExpression,
                                                                        1,
                                                                        0,
                                                                        skip,
                                                                        take);


                if (ltsBids != null && ltsBids.Count() > 0)
                {
                    //RecordCount contains the nunber of records in the database that matches the filters
                    count = RecordCount;
                    nsBids = (from e in ltsBids
                              orderby e.BidTotalCost ascending
                              select selectModelData(e)).ToArray();
                }

                response = new Models.Response(nsBids, count);
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

        [ActionName("GetNonZeroBidsById")]
        public Models.Response GetNonZeroBidsById(string id)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            try
            {
                Models.tblBid[] nsBids = new Models.tblBid[] { };
                int count = 0;

                DAL.Models.AllFilters oFilters = new DAL.Models.AllFilters();

                // get the take and skip parameters int skip = request["skip"] == null ? 0 :
                oFilters.skip = request["skip"] == null ? 0 : int.Parse(request["skip"]);
                oFilters.take = request["take"] == null ? 100 : int.Parse(request["take"]);
                oFilters.filterName = "BidLoadTenderControl";
                oFilters.filterValue = id.ToString();
                oFilters.sortName = "BidTotalCost";
                oFilters.sortDirection = "Asc";
                //Filtered Parameters  
                int RecordCount = 0;                

                DAL.NGLBidData dalBidData = new DAL.NGLBidData(Parameters);
                LTS.tblBid[] ltsBids = dalBidData.GetNonZeroBids(oFilters,ref RecordCount);


                if (ltsBids != null && ltsBids.Count() > 0)
                {
                    //RecordCount contains the nunber of records in the database that matches the filters
                    count = RecordCount;
                    nsBids = (from e in ltsBids
                              orderby e.BidTotalCost ascending
                              select selectModelData(e)).ToArray();
                }

                response = new Models.Response(nsBids, count);
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

        /////// <summary>
        /////// Inserts the Carrier's NEXTStop bid as a new record in the Bid Table
        /////// </summary>
        /////// <returns></returns>
        ////[ActionName("PostBid")]
        ////public Models.Response PostBid()
        ////{
        ////    // create a response message to send back
        ////    var response = new Models.Response(); //new HttpResponseMessage();
        ////    if (!authenticateController(ref response)) { return response; }
        ////    int bidControl;

        ////    try
        ////    {
        ////        //Parameters.UserControl = UserControl;
        ////        //Parameters.ValidateAccess = true;
        ////        DAL.NGLBidData dalData = new DAL.NGLBidData(Parameters);
        ////        Parameters = dalData.Parameters;
        ////        Parameters.ValidateAccess = false;
        ////        DAL.NGLSecurityDataProvider oSec = new DAL.NGLSecurityDataProvider(Parameters);
        ////        Models.tblBid oModel = new Models.tblBid();
        ////        int CarrierControl = 0;
        ////        string strMsg = "";

        ////        //Get the CarrierControl associated with the User account
        ////        CarrierControl = Parameters.UserCarrierControl;
        ////        if (CarrierControl != 0)
        ////        {
        ////            oModel.fillFromRequestInsert(request);
        ////            LTS.tblBid ltsRecord = selectLTSData(oModel);
        ////            bidControl = oModel.BidControl;
        ////            ltsRecord.BidCarrierControl = CarrierControl; //Set the CarrierControl

        ////            if (bidControl == 0)
        ////            {
        ////                LTS.tblBid ltsResults = dalData.InsertNEXTStopBid(ltsRecord);
        ////            }
        ////            else
        ////            {
        ////                //LTS.tblBid ltsResults = dalData.Save(ltsRecord);
        ////            }

        ////            if (ltsRecord != null && ltsRecord.BidControl != 0)
        ////            {
        ////                response.StatusCode = HttpStatusCode.OK;

        ////                var nsBids = (selectModelData(ltsRecord));
        ////                response.Data = new Models.tblBid[] { nsBids };
        ////            }
        ////            else
        ////            {
        ////                response.StatusCode = HttpStatusCode.InternalServerError;
        ////                response.Errors = string.Format("The changes to the Bid with control number {0} could not be saved", bidControl.ToString());

        ////            }
        ////        }
        ////        else
        ////        {
        ////            //Error - No Carrier Associated with user
        ////            response.StatusCode = HttpStatusCode.Unauthorized;
        ////            response.Errors = "No Carrier found for user account.";
        ////            if (!string.IsNullOrWhiteSpace(strMsg))
        ////            {
        ////                response.Errors = strMsg;
        ////            }
        ////        }

        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        //Error handler
        ////        FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
        ////        response.StatusCode = fault.StatusCode;
        ////        response.Errors = fault.formatMessage();
        ////        return response;
        ////    }

        ////    // return the HTTP Response.
        ////    return response;
        ////}

        /// <summary>
        /// NOTE - TODO - SEE IF THIS IS USED ANYWHERE. I THINK IT SHOULD BE DEPRECIATED.
        /// NOT USING THE LATEST VERSION OF NSACCEPT CODE - WE ARE MOVING TO A DISPATCH MODEL LIKE
        /// IN NSOpsStaticController.AcceptNSBid()
        /// 
        /// The method used by Operations to accept a Carrier's NEXTStop bid. Calls a different method
        /// based on the Bid Type.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ActionName("AcceptBid")]
        public Models.Response PostAcceptBid(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                //Parameters.UserControl = UserControl;
                //Parameters.ValidateAccess = true;
                BLL.NGLDATBLL oBLL = new BLL.NGLDATBLL(Parameters);
                Parameters = oBLL.Parameters;
                int CarrierContControl = 0;     

                Models.tblBid m = new Models.tblBid();
                m.fillFromRequest(request);
                DTO.WCFResults wcf = new DTO.WCFResults();

                if (id != 0 && m.BidLoadTenderControl !=0 && m.BidControl != 0)
                {
                    // Make sure data has not changed since posting
                    DAL.NGLLoadTenderData oLT = new DAL.NGLLoadTenderData(Parameters);
                    var h = oLT.HasLoadTenderChanged(m.BidLoadTenderControl);
                    if (h.HasChanged == true)
                    {
                        response.StatusCode = HttpStatusCode.Forbidden;
                        response.Errors = h.RetMsg;
                        return response;
                    }
                    switch (m.BidBidTypeControl)
                    {
                        case (int)DTO.tblLoadTender.BidTypeEnum.NextStop:
                            wcf = oBLL.NSAccept(m.BidLoadTenderControl, id, m.BidControl, m.BidCarrierControl, m.BidLineHaul, m.BidFuelUOM, m.BidFuelVariable, CarrierContControl);
                            break;
                        case (int)DTO.tblLoadTender.BidTypeEnum.NGLTariff:
                            var SelectedCarrier = new DTO.CarriersByCost();
                            SelectedCarrier.CarrierControl = m.BidCarrierControl;
                            SelectedCarrier.BookCarrTarEquipMatControl = m.BidBookCarrTarEquipMatControl;
                            SelectedCarrier.BookCarrTarEquipControl = m.BidBookCarrTarEquipControl;
                            SelectedCarrier.BookModeTypeControl = m.BidBookModeTypeControl;                            
                            wcf = oBLL.NSNGLTariffAccept(m.BidLoadTenderControl, id, m.BidControl, SelectedCarrier, CarrierContControl);
                            break;
                        case (int)DTO.tblLoadTender.BidTypeEnum.P44:
                            wcf = oBLL.NSP44Accept(m.BidLoadTenderControl, id, m.BidControl, m.BidCarrierControl, m.BidLineHaul, m.BidFuelUOM, m.BidFuelVariable, m.BidFuelTotal, m.BidTotalCost, CarrierContControl);
                            break;
                        default:
                            break;
                    }       
                }
                else
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Errors = string.Format("Accept Bid failed because BookControl {0}, BidLoadTenderControl {1}, and BidControl {2} all must be greater than 0. Source: tblBidController/PostAcceptBid", id.ToString(), m.BidLoadTenderControl.ToString(), m.BidControl.ToString());
                }

                //Check for messages
                if (wcf != null && wcf.Warnings.Count > 0)
                {
                    //We don't need to show this message to the user so if it is there remove it
                    if (wcf.Warnings.ContainsKey("W_ManualAutoAcceptNoTenderEmail")) { wcf.Warnings.Remove("W_ManualAutoAcceptNoTenderEmail"); }

                    //If we still have warnings then show them to the user
                    if (wcf.Warnings.Count > 0)
                    {
                        response.StatusCode = HttpStatusCode.InternalServerError;
                        string strWarnings = wcf.concatWarnings();
                        //string strErrors = wcf.concatErrors();
                        //string strMessages = wcf.concatMessage();
                        response.Errors = strWarnings;
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

        /// <summary>
        /// Update the BidSelectedForExport flag to true
        /// </summary>
        /// <param name="sBidControl"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.5.4.001 on o7/15/2023
        /// </remarks>
        [HttpPost, ActionName("SelectBidForExport")]
        public Models.Response SelectBidForExport([System.Web.Http.FromBody] string sBidControl)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLBidData oDAL = new DAL.NGLBidData(Parameters);
                int iBidControl = 0;
                int.TryParse(sBidControl, out iBidControl);
                if (iBidControl > 0)
                {
                    bool[] oRecords = new bool[1];
                    oRecords[0] = oDAL.UpdateBidSelectedForExport(iBidControl, true);
                    response = new Models.Response(oRecords, 1);
                }
                else
                {
                    List<string> sDetailList = new List<string> { "Select Bid for Export", "Bid Primary Key.  Please select a record and try again." };
                    // Cannot save changes to {0}.  The following key fields are required: {1}.
                    FaultExceptionEventArgs fault = Utilities.BuildException("E_CannotSaveKeyFieldsRequired", sDetailList);
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Errors = fault.formatMessage();
                    return response;
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
        /// Update the BidSelectedForExport flag to false
        /// </summary>
        /// <param name="sBidControl"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.5.4.001 on o7/15/2023
        /// </remarks>
        [HttpPost, ActionName("UnSelectBidForExport")]
        public Models.Response UnSelectBidForExport([System.Web.Http.FromBody] string sBidControl)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLBidData oDAL = new DAL.NGLBidData(Parameters);
                int iBidControl = 0;
                int.TryParse(sBidControl, out iBidControl);
                if (iBidControl > 0)
                {
                    bool[] oRecords = new bool[1];
                    oRecords[0] = oDAL.UpdateBidSelectedForExport(iBidControl, false);
                    response = new Models.Response(oRecords, 1);
                }
                else
                {
                    List<string> sDetailList = new List<string> { "Unselect Bid for Export", "Bid Primary Key.  Please select a record and try again." };
                    // Cannot save changes to {0}.  The following key fields are required: {1}.
                    FaultExceptionEventArgs fault = Utilities.BuildException("E_CannotSaveKeyFieldsRequired", sDetailList);
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Errors = fault.formatMessage();
                    return response;
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
        /// Update All the BidSelectedForExport flags to true for the Load
        /// </summary>
        /// <param name="sLoadTenderControl"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.5.4.001 on o7/15/2023
        /// </remarks>
        [HttpPost, ActionName("SelectAllBidForExport")]
        public Models.Response SelectAllBidForExport([System.Web.Http.FromBody] string sLoadTenderControl)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLBidData oDAL = new DAL.NGLBidData(Parameters);
                int iLoadTenderControl = 0;
                int.TryParse(sLoadTenderControl, out iLoadTenderControl);
                if (iLoadTenderControl > 0)
                {
                    bool[] oRecords = new bool[1];
                    oRecords[0] = oDAL.UpdateAllBidSelectedForExport(iLoadTenderControl, true);
                    response = new Models.Response(oRecords, 1);
                }
                else
                {
                    List<string> sDetailList = new List<string> { "Select All Bids for Export", "Quote Primary Key.  Please Get Rates and try again." };
                    // Cannot save changes to {0}.  The following key fields are required: {1}.
                    FaultExceptionEventArgs fault = Utilities.BuildException("E_CannotSaveKeyFieldsRequired", sDetailList);
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Errors = fault.formatMessage();
                    return response;
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
        /// Update All the BidSelectedForExport flags to false for the Load
        /// </summary>
        /// <param name="sLoadTenderControl"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.5.4.001 on o7/15/2023
        /// </remarks>
        [HttpPost, ActionName("UnSelectAllBidForExport")]
        public Models.Response UnSelectAllBidForExport([System.Web.Http.FromBody] string sLoadTenderControl)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLBidData oDAL = new DAL.NGLBidData(Parameters);
                int iLoadTenderControl = 0;
                int.TryParse(sLoadTenderControl, out iLoadTenderControl);
                if (iLoadTenderControl > 0)
                {
                    bool[] oRecords = new bool[1];
                    oRecords[0] = oDAL.UpdateAllBidSelectedForExport(iLoadTenderControl, false);
                    response = new Models.Response(oRecords, 1);
                }
                else
                {
                    List<string> sDetailList = new List<string> { "Unselect All Bids for Export", "Quote Primary Key.  Please Get Rates and try again." };
                    // Cannot save changes to {0}.  The following key fields are required: {1}.
                    FaultExceptionEventArgs fault = Utilities.BuildException("E_CannotSaveKeyFieldsRequired", sDetailList);
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Errors = fault.formatMessage();
                    return response;
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
        /// Requires a LoadTenderControl to be passed in as filter,  not n AllFilter object
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet, ActionName("GetExportBids")]
        public Models.Response GetExportBids(string filter)
        {
            
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int iLoadTenderControl = 0;
                int.TryParse(filter, out iLoadTenderControl);
                
                if (iLoadTenderControl == 0)
                {                   
                    List<string> sDetailList = new List<string> { "Get Quotes to Export", "Quote Primary Key.  Please Get Rates and try again." };
                    // Cannot save changes to {0}.  The following key fields are required: {1}.
                    FaultExceptionEventArgs fault = Utilities.BuildException("E_CannotSaveKeyFieldsRequired", sDetailList);
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Errors = fault.formatMessage();
                    return response;
                }
                LTS.vExportBid[] records = new LTS.vExportBid[] { };
                int count = 0;
                int RecordCount = 0;               
                records = NGLLoadTenderData.GetExportQuoteBids(iLoadTenderControl);
                if (records?.Count() > 0)
                {
                    count = records.Count();
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

        /////// <summary>
        /////// Deletes a Bid using the Bid Control by setting Archived to True
        /////// and BidStatusCode to CarrDeleted
        /////// Used by the NEXTStop Carrier to "Withdraw" or Delete a bid
        /////// </summary>
        /////// <returns></returns>
        ////[ActionName("DeleteBid")]
        ////public Models.Response DeleteBid()
        ////{
        ////    // create a response message to send back
        ////    var response = new Models.Response(); //new HttpResponseMessage();
        ////    if (!authenticateController(ref response)) { return response; }

        ////    try
        ////    {
        ////        //Parameters.UserControl = UserControl;
        ////        //Parameters.ValidateAccess = true;
        ////        DAL.NGLBidData dalData = new DAL.NGLBidData(Parameters);
        ////        Parameters = dalData.Parameters;

        ////        Models.tblBid oModel = new Models.tblBid();

        ////        oModel.fillFromRequest(request);
        ////        if (oModel.BidControl != 0)
        ////        {
        ////            dalData.CarrierDeleteBid(oModel.BidControl, Parameters.UserName);
        ////        }
        ////        else
        ////        {
        ////            response.Errors = "Cannot delete a Bid with BidControl = 0";
        ////            response.StatusCode = HttpStatusCode.BadRequest;
        ////            return response;
        ////        }   
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        //** TODO ** Error handler
        ////        FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
        ////        response.StatusCode = fault.StatusCode;
        ////        response.Errors = string.Format("There was an error deleting the NEXTStop Bid: {0}", fault.formatMessage());
        ////        return response;
        ////    }
        ////    // return the HTTP Response.
        ////    return response;
        ////}


        #endregion
    }
}