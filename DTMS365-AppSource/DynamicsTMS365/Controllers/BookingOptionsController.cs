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
    public class BookingOptionsController : NGLControllerBase
    {
        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.2 on 08/29/2018 initializes the Page property by calling the base class constructor
        /// </summary>
        public BookingOptionsController()
                : base(Utilities.PageEnum.LoadBoard)
	     {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.BookingOptionsController";
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

        // POST 	/API/objectcontroller{data}  : Create a new object or Update a the current object if the control number exists
        // GET 	/API/objectcontroller/{id} : Get the object information where the control number = "id"
        // PUT 	/API/objectcontroller/{data} : Update the object information stored in data
        // DELETE	/API/objectcontroller/{id} : Delete object where the control number = "id"


        /// <summary>
        /// Returns a blank Load Board Data Record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// //<remarks>
        /// Modified by RHR for v-8.4 on 06/22/2021 added model for vBookLoadBoard to support time only fields
        ///   BookDestStartHrs,BookDestStopHrs,BookOriginStartHrs,BookOriginStopHrs
        ///   NOTE:  this code does not read from the db,  somone removed the code but 
        ///   did not enter any comments as to why? 
        ///   This code may not be used?
        /// </remarks>
        [HttpGet, ActionName("Get")]
        public Models.Response Get(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                ////int RecordCount = 0;
                int count = 0;
                ////DAL.Models.AllFilters f = new DAL.Models.AllFilters();
                ////f.filterName = "BookControl";
                ////f.filterValue = id.ToString();
                ////DAL.NGLBookLoadBoard oDAL = new DAL.NGLBookLoadBoard(Parameters);

                Models.vBookLoadBoard[] oData = new Models.vBookLoadBoard[] { };
                ////oData = oDAL.GetBookLoadBoards(f, ref RecordCount);
                ////if (oData != null && oData.Count() > 0)
                ////{
                ////    count = oData.Count();
                ////    if (RecordCount > count) { count = RecordCount; }
                ////}
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

        /// <summary>
        /// Gets All the Child BookSpotRateData Records filtered by BookControl passed in as  id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.2 on 10/17/2018   
        /// </remarks>
        [HttpGet, ActionName("GetByParent")]
        public Models.Response GetByParent(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                ////int RecordCount = 0;
                int count = 0;
                ////DAL.Models.AllFilters f = new DAL.Models.AllFilters { ParentControl = id };
                ////DAL.NGLBookSpotRateData oDAL = new DAL.NGLBookSpotRateData(Parameters);
                Models.BookSpotRateData[] records = new Models.BookSpotRateData[] { };
                ////LTS.vBookSpotRateData[] oData = new LTS.vBookSpotRateData[] { };
                ////oData = oDAL.GetBookSpotRateData(f, ref RecordCount);
                ////if (oData != null && oData.Count() > 0)
                ////{
                ////    count = oData.Count();
                ////    records = (from e in oData select selectModelData(e)).ToArray();
                ////    if (RecordCount > count) { count = RecordCount; }
                ////}
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
        /// Returns a blank Load Board Record
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Returns a blank Load Board Record
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// //<remarks>
        /// Modified by RHR for v-8.4 on 06/22/2021 added model for vBookLoadBoard to support time only fields
        ///   BookDestStartHrs,BookDestStopHrs,BookOriginStartHrs,BookOriginStopHrs
        ///   NOTE:  this code does not read from the db,  somone removed the code but 
        ///   did not enter any comments as to why? 
        ///   This code may not be used?
        /// </remarks>
        [HttpGet, ActionName("GetAllRecords")]
        public Models.Response GetAllRecords(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                //Note: this code does not return any data.  it is not clear why?

                ////int RecordCount = 0;
                int count = 0;
                //////save the page filter for the next time the page loads
                ////if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter); }
                ////DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                ////DAL.NGLBookLoadBoard oDAL = new DAL.NGLBookLoadBoard(Parameters);
                Models.vBookLoadBoard[] oData = new Models.vBookLoadBoard[] { };
                ////oData = oDAL.GetBookLoadBoards(f, ref RecordCount);
                ////if (oData != null && oData.Count() > 0)
                ////{
                ////    count = oData.Count();
                ////    if (RecordCount > count) { count = RecordCount; }
                ////}
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

        /// <summary>
        /// This method expects a vBookLoadBoard data objec but it does not use this. 
        /// it updates the booking record based on the action selected on the BookingTenderWorkFlow widget
        /// and references the selected booking record for the user.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// //<remarks>
        /// Modified by RHR for v-8.4 on 06/22/2021 added model for vBookLoadBoard to support time only fields
        ///   BookDestStartHrs,BookDestStopHrs,BookOriginStartHrs,BookOriginStopHrs
        ///   The data object is never used.
        /// </remarks>
        [HttpPost, ActionName("Post")]
        public Models.Response Post([System.Web.Http.FromBody]Models.vBookLoadBoard data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLUserPageSettingData oUPS = new DAL.NGLUserPageSettingData(Parameters);
                BLL.NGLTMS365BLL oBLL = new BLL.NGLTMS365BLL(Parameters);
                int pageControl = (int)Utilities.PageEnum.LoadBoard;
                bool[] oRecords = new bool[1] { false };
                int pk = 0;

                //Steps
                //read the pk from UserPageSettings for this user for this page (in this case is BookControl) UPSNAme = pk
                LTS.tblUserPageSetting[] pkUPS = oUPS.GetPageSettingsForCurrentUser(pageControl, "pk");
                int.TryParse(pkUPS[0].UserPSMetaData, out pk);
                if(pk == 0) { response = new Models.Response(oRecords, 1); return response; } //return error if pk is nothing


                //read in optioncontrol settings - get metadata and deserialze JSON for wdgtBookingTenderWorkFlowOptionCtrlEdit (model already exists)
                LTS.tblUserPageSetting[] wdgtUPS = oUPS.GetPageSettingsForCurrentUser(pageControl, "wdgtBookingTenderWorkFlowOptionCtrlEdit");
                Models.WorkFlowSetting[] wfs = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Models.WorkFlowSetting[]>(wdgtUPS[0].UserPSMetaData);

                //find out which thing is checked - perform the action based on that
                var strAction = wfs.Where(x => x.fieldName.StartsWith("yn") && x.fieldVisible == true).Select(y => y.fieldName).FirstOrDefault();


                BLL.NGLTMS365BLL.BookingActions action = getBookingAction(strAction);

                var result = oBLL.ExecBookingMenu365(pk, action);

                // Modified by RHR for v-8.3.0.002 on 12/11/2020 added logic to return ErrWarnMsgLog data to client
                // Client must implement NGLErrWarnMsgLogCtrl logic
                //      * result must be a Models.ResultObject
                //      * DAL or BLL logic must call a function like NGLTMS365BLL.addWCFMessagesToResultObj
                //      * add widget to page in content management
                //      * add the following code to the saveSuccessCallback for the Controller
                //            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {           
                //              if (typeof(wdgtNGLErrWarnMsgLogCtrlDialog) !== 'undefined' && wdgtNGLErrWarnMsgLogCtrlDialog != null)
                //              {
                //                  wdgtNGLErrWarnMsgLogCtrlDialog.show(data);
                //              }
                //             }
                if (result != null)
                {
                    response.Err = result.Err.ToArray();
                    response.ErrTitle = result.ErrTitle;                
                    response.Warn = result.Warn.ToArray();
                    response.WarningTitle = result.WarningTitle;
                    response.Message = result.Message.ToArray();
                    response.MsgTitle = result.MsgTitle;
                    response.Log = result.Log.ToArray();
                    response.LogTitle = result.LogTitle;
                    if(result.Success == false && !string.IsNullOrWhiteSpace(result.ErrMsg)) {
                        response.StatusCode = HttpStatusCode.InternalServerError;
                        if (response.Err != null && response.Err.Count() > 0)
                        {
                            response.Errors = result.ErrMsg + "<br \\>" + response.Err[0].Message;
                        } else
                        {
                            response.Errors = result.ErrMsg;
                        }
                        
                        return response;
                    }
                }

                ////DAL.NGLBookLoadBoard oDAL = new DAL.NGLBookLoadBoard(Parameters);
                ////bool blnRet = oDAL.SaveBookLoadBoard(data);

                oRecords = new bool[1] { true };

                ////oRecords[0] = blnRet;

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
                ////DAL.NGLBookLoadBoard oDAL = new DAL.NGLBookLoadBoard(Parameters);
                ////bool blnRet = oDAL.DeleteBookLoadBoard(id);
                bool[] oRecords = new bool[1];

                ////oRecords[0] = blnRet;

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


        #region "Private Functions"

        public BLL.NGLTMS365BLL.BookingActions getBookingAction(string strAction)
        {
            BLL.NGLTMS365BLL.BookingActions action = BLL.NGLTMS365BLL.BookingActions.None;

            switch (strAction)
            {
                case "ynUnassignProvider":
                    action = BLL.NGLTMS365BLL.BookingActions.UnassignCarrier;
                    break;
                case "ynReject":
                    action = BLL.NGLTMS365BLL.BookingActions.Reject;
                    break;
                case "ynDrop":
                    action = BLL.NGLTMS365BLL.BookingActions.DropLoad;
                    break;
                case "ynRemoveOrder":
                    action = BLL.NGLTMS365BLL.BookingActions.RemoveOrder;
                    break;
                case "ynModify":
                    action = BLL.NGLTMS365BLL.BookingActions.Modify;
                    break;
                case "ynTender":
                    action = BLL.NGLTMS365BLL.BookingActions.Tender;
                    break;
                case "ynAccept":
                    action = BLL.NGLTMS365BLL.BookingActions.AcceptFinalize;
                    break;
                case "ynAcceptAll":
                    Console.WriteLine("ynAcceptAll");
                    action = BLL.NGLTMS365BLL.BookingActions.AcceptAll;
                    break;
                case "ynInvoice":
                    action = BLL.NGLTMS365BLL.BookingActions.Invoice;
                    break;
                case "ynInvoiceComplete":
                    action = BLL.NGLTMS365BLL.BookingActions.InvoiceComplete;
                    break;
                case "ynInvoiceSingle":
                    action = BLL.NGLTMS365BLL.BookingActions.InvoiceSingle;
                    break;
                case "ynInvoiceCompleteSingle":
                    action = BLL.NGLTMS365BLL.BookingActions.InvoiceCompleteSingle;
                    break;
                default:
                    action = BLL.NGLTMS365BLL.BookingActions.None;
                    break;
            }
            return action;
        }


        #endregion


    }
}