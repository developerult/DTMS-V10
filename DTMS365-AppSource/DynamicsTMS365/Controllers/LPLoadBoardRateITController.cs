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

using LoadTenderTypeEnum = Ngl.FreightMaster.Data.DataTransferObjects.tblLoadTender.LoadTenderTypeEnum;
using BidTypeEnum = Ngl.FreightMaster.Data.DataTransferObjects.tblLoadTender.BidTypeEnum;


namespace DynamicsTMS365.Controllers
{
    public class LPLoadBoardRateITController : NGLControllerBase
    {
        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.2 on 12/03/2018 initializes the Page property by calling the base class constructor
        /// </summary>
        public LPLoadBoardRateITController()
                : base(Utilities.PageEnum.LoadPlanning)
        {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.LPLoadBoardRateITController";
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
        /// Returns a blan Load Board Record with the id as the BookControl
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.4 on 06/22/2021 added model for vBookLoadBoard to support time only fields
        ///   BookDestStartHrs,BookDestStopHrs,BookOriginStartHrs,BookOriginStopHrs
        /// </remarks>
        [HttpGet, ActionName("Get")]
        public Models.Response Get(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {

                //The Load Board Rate It does not currently have any data to Readbeed to read
                // the load board data so for now we just return the parent id
                //in a vBookLoadBoard record
                //if more data is needed in the future we should read the records
                //from the database
                Models.vBookLoadBoard record = new Models.vBookLoadBoard();
                record.BookControl = id;
                Models.vBookLoadBoard[] oRecords = new Models.vBookLoadBoard[1];

                oRecords[0] = record;
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
        /// Gets All the Child Records filtered by parent id
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
                //The Load Board Rate It does not currently have any data to Read
                //The NGL Widget expects some data in an array
                //so just return an array with the first value of true
                bool[] oRecords = new bool[1];
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
                //The Load Board Rate It does not currently have any data to Read
                //The NGL Widget expects some data in an array
                //so just return an array with the first value of true
                bool[] oRecords = new bool[1];
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
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.2 on 12/17/2018
        ///  we now call the correct BookRevBLL.GenerateQuote based on the selected switch
        ///  added logic to process wcfResponse Message, Warning, and Error dictionary messages
        ///  Modified by RHR for v-8.4 on 06/22/2021 added model for vBookLoadBoard to support time only fields
        ///   BookDestStartHrs,BookDestStopHrs,BookOriginStartHrs,BookOriginStopHrs
        /// </remarks>
        [HttpPost, ActionName("Post")]
        public Models.Response Post([System.Web.Http.FromBody]Models.vBookLoadBoard data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DTO.WCFResults oRet = new DTO.WCFResults();
                Models.GenericResult oGResult = new Models.GenericResult() { Control = 0, strField = "undefined" };
                if (data == null) {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Errors = Utilities.getLocalizedMsg("E_CannotSaveNoData");
                    response.Data = new Models.GenericResult[] { oGResult };
                    response.Count = 1;
                    return response;
                }
                //The Load Board Rate It does not currently have any data except the BookControl
                //So all the other fields in data will be blank
                DAL.NGLUserPageSettingData oUPS = new DAL.NGLUserPageSettingData(Parameters);
                BLL.NGLTMS365BLL oBLL = new BLL.NGLTMS365BLL(Parameters);
                int pageControl = (int)Utilities.PageEnum.LoadPlanning;
                bool[] oRecords = new bool[1] { false };
                int pk = data.BookControl;

                //Steps
                //read the pk from UserPageSettings for this user for this page (in this case is BookControl) UPSNAme = pk
                if (pk == 0)
                {
                    LTS.tblUserPageSetting[] pkUPS = oUPS.GetPageSettingsForCurrentUser(pageControl, "pk");
                    int.TryParse(pkUPS[0].UserPSMetaData, out pk);
                    if (pk == 0)
                    {
                        //error messaged needs details 
                        //unformatted message:
                        //Cannot save changes to {0}.  The following key fields are required: {1}.
                        List<string> sDetailList = new List<string> { "Load Board Rates", "Booking Primary Key" };
                        FaultExceptionEventArgs fault = Utilities.BuildException("E_CannotSaveKeyFieldsRequired", sDetailList);
                        response.StatusCode = HttpStatusCode.BadRequest;
                        response.Errors = fault.formatMessage();
                        return response;
                    }
                }

                //read in optioncontrol settings - get metadata and deserialze JSON for wdgtBookingTenderWorkFlowOptionCtrlEdit (model already exists)
                LTS.tblUserPageSetting[] wdgtUPS = oUPS.GetPageSettingsForCurrentUser(pageControl, "wdgtRateITWorkFlowOptionEdit");
                Models.WorkFlowSetting[] wfs = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Models.WorkFlowSetting[]>(wdgtUPS[0].UserPSMetaData);
                //find out which thing is checked - perform the action based on that
                //Json OBJECT LOOKS LIKE '{"fieldID":"3273","fieldName":"RateITWorkFlowSwitchSpotRate","fieldDefaultValue":"true","fieldVisible":"true","fieldReadOnly":"false"}'
                //NGLWorkFlowGroups must be visible Switches must have a default value of "true" text
                if (wfs != null)
                {
                    var switchResponse = new Models.Response();
                    BLL.NGLBookRevenueBLL oBookRevBLL = new BLL.NGLBookRevenueBLL(this.Parameters);
                    foreach (Models.WorkFlowSetting stg in wfs)
                    {
                        switch (stg.fieldName)
                        {
                            case "RateITWorkFlowSwitchSpotRate":  //this is a switch so check the default value not the visible value
                                if (!string.IsNullOrWhiteSpace(stg.fieldDefaultValue) && stg.fieldDefaultValue.ToString().ToLower() == "true")
                                {
                                    LoadTenderTypeEnum[] TenderTypes = { LoadTenderTypeEnum.SpotRate };
                                    BidTypeEnum[] bidTypes = { BidTypeEnum.Spot };

                                    oRet = oBookRevBLL.GenerateQuote(null, TenderTypes, bidTypes, pk);

                                    int iBidControl = 0;
                                    // Modified by RHR for v-8.2 01/01/2019 to simplify reading of WCFResults keys 
                                    oRet.TryParseKeyInt("BidControl",ref iBidControl);
                                    oGResult.Control =  iBidControl;
                                    oGResult.strField = "BidControl";
                                    oGResult.strField2 = "RateITWorkFlowSwitchSpotRate";
                                    switchResponse.Data = new Models.GenericResult[] { oGResult };
                                    switchResponse.Count = 1;
                                    // Modified by RHR for v-8.5.3.007 on 02/20/2023
                                    //Deprecated in v-8.3.0.002 we now use addWCFMessagesToResponse and the NGLErrWarnMsgLogCtrl widget
                                    //check for errors messages and warning.
                                    //switchResponse.Errors = Utilities.formatWCFResultErrors(oRet);
                                    //switchResponse.Warnings = Utilities.formatWCFResultWarnings(oRet);
                                    //switchResponse.Messages = Utilities.formatWCFResultMessages(oRet);
                                    switchResponse.StatusCode = HttpStatusCode.OK;
                                }
                                break;
                            case "RateITRatingWorkFlowGroup":  // this is a group so just check the visible flag
                                if (stg.fieldVisible == true)
                                {
                                    bool blnGetP44Rate = false;
                                    bool blnGetNGLRate = false;
                                    foreach (Models.WorkFlowSetting subItems in wfs)
                                    {
                                        switch (subItems.fieldName)
                                        {
                                            case "RateITIncludeAPISwitch": //this is as switch so check the default value (true for checked)
                                                if (!string.IsNullOrWhiteSpace(subItems.fieldDefaultValue) && subItems.fieldDefaultValue.ToString().ToLower() == "true") { blnGetP44Rate = true; }
                                                break;
                                            case "RateITIncludeTariffSwitch":  //this is as switch so check the default value (true for checked)
                                                if (!string.IsNullOrWhiteSpace(subItems.fieldDefaultValue) && subItems.fieldDefaultValue.ToString().ToLower() == "true") { blnGetNGLRate = true; }
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                    if (blnGetNGLRate || blnGetP44Rate)
                                    {
                                        LoadTenderTypeEnum[] TenderTypes = { LoadTenderTypeEnum.LoadBoard };
                                        List<BidTypeEnum> bidTypes = new List<BidTypeEnum>();

                                        if (blnGetP44Rate) { bidTypes.Add(BidTypeEnum.P44); }
                                        if (blnGetNGLRate) { bidTypes.Add(BidTypeEnum.NGLTariff); }
                                       
                                       oRet = oBookRevBLL.GenerateQuote(null, TenderTypes, bidTypes.ToArray(), pk);

                                        int iLoadTenderControl = 0;
                                        // Modified by RHR for v-8.2 01/01/2019 to simplify reading of WCFResults keys 
                                        oRet.TryParseKeyInt("LoadTenderControl", ref iLoadTenderControl);                                     
                                        oGResult.Control = iLoadTenderControl;
                                        oGResult.strField = "LoadTenderControl";
                                        oGResult.strField2 = "RateITRatingWorkFlowGroup";
                                        oGResult.log = oRet.Log.ToArray();
                                        switchResponse.Data = new Models.GenericResult[] { oGResult };
                                        switchResponse.Count = 1;
                                        // Modified by RHR for v-8.5.3.007 on 02/20/2023
                                        // Deprecated in v - 8.3.0.002 we now use addWCFMessagesToResponse and the NGLErrWarnMsgLogCtrl widget
                                        //check for errors messages and warning.
                                        //switchResponse.Errors = Utilities.formatWCFResultErrors(oRet);
                                        //switchResponse.Warnings = Utilities.formatWCFResultWarnings(oRet);
                                        //switchResponse.Messages = Utilities.formatWCFResultMessages(oRet);
                                        switchResponse.StatusCode = HttpStatusCode.OK;
                                    }
                                }
                                break;
                            case "RateITPostWorkFlowGroup": // this is a group so just check the visible flag
                                if (stg.fieldVisible == true)
                                {
                                    bool blnPostToDAT = false;
                                    bool blnPostToNEXTStop = false;
                                    foreach (Models.WorkFlowSetting subItems in wfs)
                                    {
                                        switch (subItems.fieldName)
                                        {
                                            case "RateITPostToDATSwitch": //this is as switch so check the default value (true for checked)
                                                if (!string.IsNullOrWhiteSpace(subItems.fieldDefaultValue) && subItems.fieldDefaultValue.ToString().ToLower() == "true") { blnPostToDAT = true; }
                                                break;
                                            case "RateITPostToNGLSwitch": //this is as switch so check the default value (true for checked)
                                                if (!string.IsNullOrWhiteSpace(subItems.fieldDefaultValue) && subItems.fieldDefaultValue.ToString().ToLower() == "true") { blnPostToNEXTStop = true; }
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                    if (blnPostToNEXTStop || blnPostToDAT)
                                    {
                                        List <LoadTenderTypeEnum> TenderTypes = new List<LoadTenderTypeEnum>();
                                        BidTypeEnum[] bidTypes = { BidTypeEnum.NextStop };

                                        if (blnPostToDAT) { TenderTypes.Add(LoadTenderTypeEnum.DAT); }
                                        if (blnPostToNEXTStop) { TenderTypes.Add(LoadTenderTypeEnum.NextStop); }

                                        oRet = oBookRevBLL.GenerateQuote(null, TenderTypes.ToArray(), bidTypes, pk);

                                        int iBookControl = 0;
                                        // Modified by RHR for v-8.2 01/01/2019 to simplify reading of WCFResults keys 
                                        // Modified By LVV on 2/27/19 bug fix - If oGResult.Control is 0 then in the JS the close function never gets called to close the window and refresh the grid
                                        // The code in GenerateQuote() never populates KeyFields with BookControl - We already have BookControl as pk so just use that instead                                    
                                        iBookControl = pk; //oRet.TryParseKeyInt("BookControl", ref iBookControl); 
                                        oGResult.Control = iBookControl;
                                        oGResult.strField = "BookControl";
                                        oGResult.strField2 = "RateITPostWorkFlowGroup";
                                        switchResponse.Count = 1;
                                        // Deprecated in v - 8.3.0.002 we now use addWCFMessagesToResponse and the NGLErrWarnMsgLogCtrl widget
                                        //Deprecated in v-8.3.0.002 we now use addWCFMessagesToResponse and the NGLErrWarnMsgLogCtrl widget
                                        //check for errors messages and warning.
                                        //switchResponse.Errors = Utilities.formatWCFResultErrors(oRet);
                                        //switchResponse.Warnings = Utilities.formatWCFResultWarnings(oRet);
                                        //switchResponse.Messages = Utilities.formatWCFResultMessages(oRet);
                                        switchResponse.StatusCode = HttpStatusCode.OK;
                                        //Added By LVV on 2/27/29 for bug fix - forgot to add the below code to set Data
                                        switchResponse.Data = new Models.GenericResult[] { oGResult };
                                        switchResponse.Count = 1;
                                    }
                                }
                                break;
                            default:
                                break;
                        } 
                    }

                    //if (switchResponse != null && switchResponse.StatusCode == HttpStatusCode.OK) { return switchResponse; }
                    // Modified by RHR for v-8.5.3.007 on 02/20/2023
                    if (switchResponse != null)
                    {
                        if (oRet != null)
                        {
                            switchResponse.AsyncMessagesPossible = oRet.isAsyncMsgPossible();
                            switchResponse.AsyncMessageKey = oRet.getAsyncMessageKey();
                            switchResponse.AsyncTypeKey = oRet.getAsyncTypeKey();
                            Utilities.addWCFMessagesToResponse(ref switchResponse, ref oRet, "Rate It");
                        }
                        if (switchResponse.StatusCode == HttpStatusCode.OK) { return switchResponse; }
                    }
                }
               
                // this should not happen in production but we show the message to help debug problems 
                response = new Models.Response();
                response.Errors = Utilities.getLocalizedMsg("E_ReadWorkFlowFailure");         
                response.Data = new Models.GenericResult[] { oGResult };
                response.Count = 1;
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

        /// <summary>
        /// Calculates the spot rate details and creates the load board bid data. returns the bid control of the inserted record  and prepares a response object to return
        /// </summary>
        /// <param name="pk"></param>
        /// <param name="data"></param>
        /// <param name="iBidControl"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.2 on 12/04/2018
        /// </remarks>
        //private Models.Response executeDoSpotRate(int pk, LTS.vBookLoadBoard data, ref int iBidControl)
        //{
        //    var response = new Models.Response(); //new HttpResponseMessage();           
        //    try
        //    {
        //        BLL.NGLBookRevenueBLL oBLL = new BLL.NGLBookRevenueBLL(this.Parameters);
        //        iBidControl = oBLL.InsertLoadTenderSpotRate(pk);
                
        //        int count = 1;
        //        int[] aBidControls = new int[] { iBidControl };
        //        response = new Models.Response(aBidControls, count);
        //        response.StatusCode = HttpStatusCode.OK;

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
     

        #endregion


    }
}