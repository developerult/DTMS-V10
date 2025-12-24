using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using DAL = Ngl.FreightMaster.Data;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using LTS = Ngl.FreightMaster.Data.LTS;
using BLL = NGL.FM.BLL;

//Added By LVV on 12/23/16 for v-8.0 Next Stop
//TODO I THINK WE CAN DELETE THIS

namespace DynamicsTMS365.Controllers
{
    public class NextStopLoadController : NGLControllerBase
    {
        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.NextStopLoadsController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        private Models.NextStopLoad selectModelData(LTS.vNSLoadsWActiveBid d)
        {
            Models.NextStopLoad nsLoad = new Models.NextStopLoad();
            List<string> skipObjs = new List<string> { };
            string sMsg = "";
            nsLoad = (Models.NextStopLoad)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(nsLoad, d, skipObjs, ref sMsg);
            return nsLoad;
        }

        private Models.NextStopLoad selectModelData(LTS.vNSAvailablePendingLoad d)
        {
            Models.NextStopLoad nsLoad = new Models.NextStopLoad();
            List<string> skipObjs = new List<string> { };
            string sMsg = "";
            nsLoad = (Models.NextStopLoad)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(nsLoad, d, skipObjs, ref sMsg);
            return nsLoad;
        }

        private Models.NextStopLoad selectModelData(LTS.tblLoadTender d)
        {
            Models.NextStopLoad nsBid = new Models.NextStopLoad();
            List<string> skipObjs = new List<string> { "LTUpdated", "tblLoadTenderType", "_tblLoadTenderType" };
            string sMsg = "";
            nsBid = (Models.NextStopLoad)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(nsBid, d, skipObjs, ref sMsg);
            return nsBid;
        }

        private LTS.tblLoadTender selectLTSDataLT(Models.NextStopLoad d)
        {
            LTS.tblLoadTender ltsRecord = new LTS.tblLoadTender();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "LTUpdated", "tblLoadTenderType", "_tblLoadTenderType" };
                string sMsg = "";
                ltsRecord = (LTS.tblLoadTender)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] ltupdated = d.getUpdated();
                    ltsRecord.LTUpdated = ltupdated == null ? new byte[0] : ltupdated;

                }
            }

            return ltsRecord;
        }



        #endregion

        #region " REST Services"

        [HttpGet, ActionName("GetNSHisoricalLoads")]
        public Models.Response GetNSHisoricalLoads(int id)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            try
            {
                Models.NextStopLoad[] nsLoads = new Models.NextStopLoad[] { };
                int count = 0;

                // get the take and skip parameters int skip = request["skip"] == null ? 0 :
                int skip = request["skip"] == null ? 0 : int.Parse(request["skip"]);
                int take = request["take"] == null ? 100 : int.Parse(request["take"]);

                //Filtered Parameters               
                string sortExpression = "LoadTenderControl Desc";
                string filterWhere = "";
                int RecordCount = 0;

                DAL.NGLLoadTenderData dalLTData = new DAL.NGLLoadTenderData(Parameters);
                LTS.tblLoadTender[] ltsLoads = dalLTData.GetNSHisoricalLoads(ref RecordCount,
                                                                        filterWhere,
                                                                        sortExpression,
                                                                        1,
                                                                        0,
                                                                        skip,
                                                                        take);


                if (ltsLoads != null && ltsLoads.Count() > 0)
                {
                    //RecordCount contains the nunber of records in the database that matches the filters
                    count = RecordCount;
                    nsLoads = (from e in ltsLoads
                               select selectModelData(e)).ToArray();
                }

                response = new Models.Response(nsLoads, count);
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

        [HttpGet, ActionName("GetLoadsWActiveBid")]
        public Models.Response GetLoadsWActiveBid(int id)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            try
            {
                Models.NextStopLoad[] nsLoads = new Models.NextStopLoad[] { };
                int count = 0;

                // get the take and skip parameters int skip = request["skip"] == null ? 0 :
                int skip = request["skip"] == null ? 0 : int.Parse(request["skip"]);
                int take = request["take"] == null ? 100 : int.Parse(request["take"]);

                //Filtered Parameters               
                string sortExpression = "LTTenderedDate Desc, LoadTenderControl Asc";
                string filterWhere = "";
                int RecordCount = 0;

                DAL.NGLLoadTenderData dalLTData = new DAL.NGLLoadTenderData(Parameters);
                LTS.vNSLoadsWActiveBid[] ltsLoads = dalLTData.GetvNSLoadsWActiveBid(ref RecordCount,
                                                                        filterWhere,
                                                                        sortExpression,
                                                                        1,
                                                                        0,
                                                                        skip,
                                                                        take);

                if (ltsLoads != null && ltsLoads.Count() > 0)
                {
                    //RecordCount contains the nunber of records in the database that matches the filters
                    count = RecordCount;
                    nsLoads = (from e in ltsLoads
                               orderby e.LTTenderedDate descending, e.LoadTenderControl ascending
                               select selectModelData(e)).ToArray();
                }

                response = new Models.Response(nsLoads, count);
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
        /// Deletes the Posting to NEXTStop using the BookControl
        /// Used by Ops to delete a NEXTStop Posting
        /// </summary>
        /// <returns></returns>
        [ActionName("DeleteLoadWActiveBid")]
        public Models.Response DeleteLoadWActiveBid(int id)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            bool blnSuccess = false;
            DTO.WCFResults wcfRes = new DTO.WCFResults();
            
            try
            {
                //Parameters.UserControl = UserControl;
                //Parameters.ValidateAccess = true;
                BLL.NGLDATBLL oBLL = new BLL.NGLDATBLL(Parameters);
                Parameters = oBLL.Parameters;
                Models.NextStopLoad oModel = new Models.NextStopLoad();

                oModel.fillFromRequestDelete(request);
                if (oModel.LTBookControl != 0)
                {
                    wcfRes = oBLL.RunNEXTStopDeleteMethod(oModel.LTBookControl);
                    blnSuccess = wcfRes.Success;
                }
                else
                {
                    response.StatusCode = HttpStatusCode.OK;
                    return response;
                }

                //Process the results
                if (blnSuccess)
                {
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    if (wcfRes != null)
                    {
                        string strMsg = "";
                        if (wcfRes.Warnings != null && wcfRes.Warnings.Count() > 0)
                        {
                            strMsg = wcfRes.concatWarnings() + " ";
                        }
                        if (wcfRes.Errors != null && wcfRes.Errors.Count() > 0)
                        {
                            strMsg += wcfRes.concatErrors();
                        }
                        if (!string.IsNullOrWhiteSpace(strMsg))
                        {
                            response.Errors = strMsg;
                        }
                    }
                    else
                    {
                        response.Errors = string.Format("Cannot delete the NEXTStop Posting with control number {0} and BookControl {1}", oModel.LoadTenderControl.ToString(), oModel.LTBookControl.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                //** TODO ** Error handler
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = string.Format("There was an error deleting the NEXTStop Posting: {0}. ", fault.formatMessage());
                return response;
            }
            // return the HTTP Response.
            return response;
        }

        //I put the CarrierControl as int param here so we can tell if the call is
        //coming from a Carrier page (1) or Ops page (0)
        ////[HttpGet, ActionName("GetAvailablePendingLoads")]
        ////public Models.Response GetAvailablePendingLoads(int id)
        ////{
        ////    // create a response message to send back
        ////    var response = new Models.Response(); //new HttpResponseMessage();
        ////    if (!authenticateController(ref response)) { return response; }

        ////    try
        ////    {
        ////        Models.NextStopLoad[] nsLoads = new Models.NextStopLoad[] { };
        ////        int count = 0;

        ////        // get the take and skip parameters
        ////        int skip = request["skip"] == null ? 0 : int.Parse(request["skip"]);
        ////        int take = request["take"] == null ? 100 : int.Parse(request["take"]);

        ////        //Filtered Parameters 

        ////        string filterWhere = "";//"(BidStatusCode = 1) AND (BidCarrierControl = " + CarrierControl + ")";
        ////        string sortExpression = "LTTenderedDate Desc, LoadTenderControl Asc";
        ////        int RecordCount = 0;
        ////        string strMsg = "";

        ////        DAL.NGLLoadTenderData dalLTData = new DAL.NGLLoadTenderData(Parameters);
        ////        BLL.NGLTMS365BLL bll = new BLL.NGLTMS365BLL(Parameters);
        ////        LTS.vNSAvailablePendingLoad[] ltsLoads;

        ////        if (id != 0)
        ////        {
        ////            //USES CM NOW - NSCarrAvailLoadController
        ////            //If this is a Carrier
        ////            ltsLoads = new LTS.vNSAvailablePendingLoad[] { };
        ////        }
        ////        else
        ////        {
        ////            //If this is Ops
        ////            ltsLoads = dalLTData.GetvNSOpsPendingLoads(ref RecordCount,
        ////                                                                filterWhere,
        ////                                                                sortExpression,
        ////                                                                1,
        ////                                                                0,
        ////                                                                skip,
        ////                                                                take);
        ////        }


        ////        if (ltsLoads != null && ltsLoads.Count() > 0)
        ////        {
        ////            //RecordCount contains the nunber of records in the database that matches the filters
        ////            count = RecordCount;
        ////            nsLoads = (from e in ltsLoads
        ////                       orderby e.LTTenderedDate descending, e.LoadTenderControl ascending
        ////                       select selectModelData(e)).ToArray();
        ////            response = new Models.Response(nsLoads, count);
        ////        }
        ////        else
        ////        {
        ////            if (!string.IsNullOrWhiteSpace(strMsg))
        ////            {
        ////                response.StatusCode = HttpStatusCode.Unauthorized;
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



        #endregion
    }
}