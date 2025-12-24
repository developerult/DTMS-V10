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
using DModel = Ngl.FreightMaster.Data.Models;
using LoadTenderTypeEnum = Ngl.FreightMaster.Data.DataTransferObjects.tblLoadTender.LoadTenderTypeEnum;
using BidTypeEnum = Ngl.FreightMaster.Data.DataTransferObjects.tblLoadTender.BidTypeEnum;


namespace DynamicsTMS365.Controllers
{
    public class DispatchingController : NGLControllerBase
    {

        #region " Properties"

        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.DispatchingController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Private Mehtods"
       
         private string formatDispatchTimeStringForAPI(string sTime,string sDefault)
        {
            string sRet = sDefault;
            DateTime dtParsed = DateTime.Now;
            string sToParse = string.Format("{0} {1}", "2018-01-01", sTime);
            if (DateTime.TryParse(sToParse,out dtParsed))
            {
                sRet = dtParsed.ToString("HH:mm");
            }          
            return sRet;
        }

        //private P44M.Shipment.LengthUnitEnum getLengthUnitEnum(string s)
        //{
        //    var strLengthUnitEnum = s.Trim().ToUpper();

        //    //Set default
        //    P44M.Shipment.LengthUnitEnum retVal = P44M.Shipment.LengthUnitEnum.IN;

           
        //    if (string.IsNullOrWhiteSpace(strLengthUnitEnum)) { return retVal; }

        //    if (strLengthUnitEnum == "CM") { retVal = P44M.Shipment.LengthUnitEnum.CM; }

        //    return retVal;
        //}

        //private P44M.Shipment.WeightUnitEnum getWeightUnitEnum(string s)
        //{
        //    var strWeightUnitEnum = s.Trim().ToUpper();

        //    //Set return value to US by default
        //    P44M.Shipment.WeightUnitEnum retVal = P44M.Shipment.WeightUnitEnum.LB;

        //    //If country is null return US by default
        //    if (string.IsNullOrWhiteSpace(strWeightUnitEnum)) { return retVal; }

        //    if (strWeightUnitEnum == "KG" ) { retVal = P44M.Shipment.WeightUnitEnum.KG; }

        //    return retVal;
        //}

        //private P44M.Address.CountryEnum getCountryEnum(string s)
        //{
        //    var strCountry = s.Trim().ToUpper();

        //    //Set return value to US by default
        //    P44M.Address.CountryEnum retVal = P44M.Address.CountryEnum.US;

        //    //If country is null return US by default
        //    if (string.IsNullOrWhiteSpace(strCountry)) { return retVal; }

        //    if (strCountry == "US" || strCountry == "USA") { retVal = P44M.Address.CountryEnum.US; }

        //    if (strCountry == "CA" || strCountry == "CAN") { retVal = P44M.Address.CountryEnum.CA; }

        //    if (strCountry == "MX" || strCountry == "MEX") { retVal = P44M.Address.CountryEnum.MX; }

        //    return retVal;
        //}

        //private P44M.LineItem.FreightClassEnum getFreightClassEnum(string s)
        //{
        //    //Set return value to 50 by default
        //    P44M.LineItem.FreightClassEnum retVal = P44M.LineItem.FreightClassEnum._50;

        //    switch (s)
        //    {
        //        case "50":
        //            retVal = P44M.LineItem.FreightClassEnum._50;
        //            break;
        //        case "55":
        //            retVal = P44M.LineItem.FreightClassEnum._55;
        //            break;
        //        case "60":
        //            retVal = P44M.LineItem.FreightClassEnum._60;
        //            break;
        //        case "65":
        //            retVal = P44M.LineItem.FreightClassEnum._65;
        //            break;
        //        case "70":
        //            retVal = P44M.LineItem.FreightClassEnum._70;
        //            break;
        //        case "77.5":
        //            retVal = P44M.LineItem.FreightClassEnum._775;
        //            break;
        //        case "85":
        //            retVal = P44M.LineItem.FreightClassEnum._85;
        //            break;
        //        case "92.5":
        //            retVal = P44M.LineItem.FreightClassEnum._925;
        //            break;
        //        case "100":
        //            retVal = P44M.LineItem.FreightClassEnum._100;
        //            break;
        //        case "110":
        //            retVal = P44M.LineItem.FreightClassEnum._110;
        //            break;
        //        case "125":
        //            retVal = P44M.LineItem.FreightClassEnum._125;
        //            break;
        //        case "150":
        //            retVal = P44M.LineItem.FreightClassEnum._150;
        //            break;
        //        case "175":
        //            retVal = P44M.LineItem.FreightClassEnum._175;
        //            break;
        //        case "200":
        //            retVal = P44M.LineItem.FreightClassEnum._200;
        //            break;
        //        case "250":
        //            retVal = P44M.LineItem.FreightClassEnum._250;
        //            break;
        //        case "300":
        //            retVal = P44M.LineItem.FreightClassEnum._300;
        //            break;
        //        case "400":
        //            retVal = P44M.LineItem.FreightClassEnum._400;
        //            break;
        //        case "500":
        //            retVal = P44M.LineItem.FreightClassEnum._500;
        //            break;
        //    }

        //    return retVal;
        //}

        //private P44M.LineItem.PackageTypeEnum getPackageTypeEnum(string s)
        //{
        //    //Set return value to PLT by default
        //    P44M.LineItem.PackageTypeEnum retVal = P44M.LineItem.PackageTypeEnum.PLT;

        //    var strPackage = s.Trim().ToUpper();

        //    switch (strPackage)
        //    {
        //        case "PLT":
        //            retVal = P44M.LineItem.PackageTypeEnum.PLT;
        //            break;
        //        case "BAG":
        //            retVal = P44M.LineItem.PackageTypeEnum.BAG;
        //            break;
        //        case "BALE":
        //            retVal = P44M.LineItem.PackageTypeEnum.BALE;
        //            break;
        //        case "BOX":
        //            retVal = P44M.LineItem.PackageTypeEnum.BOX;
        //            break;
        //        case "BUCKET":
        //            retVal = P44M.LineItem.PackageTypeEnum.BUCKET;
        //            break;
        //        case "PAIL":
        //            retVal = P44M.LineItem.PackageTypeEnum.PAIL;
        //            break;
        //        case "BUNDLE":
        //            retVal = P44M.LineItem.PackageTypeEnum.BUNDLE;
        //            break;
        //        case "CAN":
        //            retVal = P44M.LineItem.PackageTypeEnum.CAN;
        //            break;
        //        case "CARTON":
        //            retVal = P44M.LineItem.PackageTypeEnum.CARTON;
        //            break;
        //        case "CASE":
        //            retVal = P44M.LineItem.PackageTypeEnum.CASE;
        //            break;
        //        case "COIL":
        //            retVal = P44M.LineItem.PackageTypeEnum.COIL;
        //            break;
        //        case "CRATE":
        //            retVal = P44M.LineItem.PackageTypeEnum.CRATE;
        //            break;
        //        case "CYLINDER":
        //            retVal = P44M.LineItem.PackageTypeEnum.CYLINDER;
        //            break;
        //        case "DRUM":
        //            retVal = P44M.LineItem.PackageTypeEnum.DRUM;
        //            break;
        //        case "PIECES":
        //            retVal = P44M.LineItem.PackageTypeEnum.PIECES;
        //            break;
        //        case "REEL":
        //            retVal = P44M.LineItem.PackageTypeEnum.REEL;
        //            break;
        //        case "ROLL":
        //            retVal = P44M.LineItem.PackageTypeEnum.ROLL;
        //            break;
        //        case "SKID":
        //            retVal = P44M.LineItem.PackageTypeEnum.SKID;
        //            break;
        //        case "TUBE":
        //            retVal = P44M.LineItem.PackageTypeEnum.TUBE;
        //            break;
        //    }

        //    return retVal;
        //}

        //private P44M.LineItemHazmatDetail.PackingGroupEnum getPackingGroupEnum(string s)
        //{
        //    //Set return value to I by default
        //    P44M.LineItemHazmatDetail.PackingGroupEnum retVal = P44M.LineItemHazmatDetail.PackingGroupEnum.I;

        //    var strPacking = s.Trim().ToUpper();

        //    switch (strPacking)
        //    {
        //        case "I":
        //            retVal = P44M.LineItemHazmatDetail.PackingGroupEnum.I;
        //            break;
        //        case "II":
        //            retVal = P44M.LineItemHazmatDetail.PackingGroupEnum.II;
        //            break;
        //        case "III":
        //            retVal = P44M.LineItemHazmatDetail.PackingGroupEnum.III;
        //            break;
        //    }

        //    return retVal;
        //}

        //private P44M.Shipment.PaymentTermsOverrideEnum getPaymentTermsOverrideEnum(string s)
        //{
        //    //Set return value to PREPAID by default
        //    P44M.Shipment.PaymentTermsOverrideEnum retVal = P44M.Shipment.PaymentTermsOverrideEnum.PREPAID;

        //    var strPayment = s.Trim().ToUpper();

        //    switch (strPayment)
        //    {
        //        case "PREPAID":
        //            retVal = P44M.Shipment.PaymentTermsOverrideEnum.PREPAID;
        //            break;
        //        case "COLLECT":
        //            retVal = P44M.Shipment.PaymentTermsOverrideEnum.COLLECT;
        //            break;
        //        case "THIRDPARTY":
        //            retVal = P44M.Shipment.PaymentTermsOverrideEnum.THIRDPARTY;
        //            break;
        //    }

        //    return retVal;
        //}

        //private P44M.Shipment.DirectionOverrideEnum getDirectionOverrideEnum(string s)
        //{
        //    //Set return value to THIRDPARTY by default
        //    P44M.Shipment.DirectionOverrideEnum retVal = P44M.Shipment.DirectionOverrideEnum.THIRDPARTY;

        //    var strDirection = s.Trim().ToUpper();

        //    switch (strDirection)
        //    {
        //        case "THIRDPARTY":
        //            retVal = P44M.Shipment.DirectionOverrideEnum.THIRDPARTY;
        //            break;
        //        case "SHIPPER":
        //            retVal = P44M.Shipment.DirectionOverrideEnum.SHIPPER;
        //            break;
        //        case "CONSIGNEE":
        //            retVal = P44M.Shipment.DirectionOverrideEnum.CONSIGNEE;
        //            break;
        //    }

        //    return retVal;
        //}

        #endregion

        #region " REST Services"

        [HttpGet, ActionName("GetNextCNSNbr")]
        public Models.Response GetNextCNSNbr(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();            
            if (!authenticateController(ref response)) { return response; }
            try
            {            
                DAL.NGLLoadTenderData dalData = new DAL.NGLLoadTenderData(Parameters);
                string strResult = dalData.GetNextCNSNumberByLE(Parameters.UserLEControl);
                Array b = new string[1] { strResult };
                response = new Models.Response(b, 1);
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
        /// Reads the bid from the tblBid table using id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.2 on 12/22/2018
        ///     we now use the BidType and the LoadTenderType stored in the 
        ///     tblBid and tblLoadTender tables to dispatch the selected load
        /// </remarks>
        [HttpGet, ActionName("GetBidToDispatch")]
        public Models.Response GetBidToDispatch(int id)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            List<DModel.Dispatch> oDispatched = new List<DModel.Dispatch>();
            try
            {
                DModel.Dispatch oDispatch = NGLLoadTenderData.getBidToDispatch(id);
                oDispatched.Add(oDispatch);
                response = new Models.Response(oDispatched.ToArray(), 1);
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

        [HttpGet, ActionName("AssignBid")]
        public Models.Response AssignBid(int id)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            List<DModel.Dispatch> oDispatched = new List<DModel.Dispatch>();
            try
            {
                DModel.Dispatch oDispatch = NGLLoadTenderData.getBidToDispatch(id);
                BLL.NGLBookRevenueBLL oBookRevBLL = new BLL.NGLBookRevenueBLL(Parameters);
                DTO.WCFResults oRet = oBookRevBLL.AssignBid(ref oDispatch);

                response.Errors = Utilities.formatWCFResultErrors(oRet);
                response.Warnings = Utilities.formatWCFResultWarnings(oRet);
                response.Messages = Utilities.formatWCFResultMessages(oRet);
                response.StatusCode = HttpStatusCode.OK;
                bool[] oRecords = new bool[1] { false };
                if (oRet.Success)
                {
                    oRecords[0] = true;
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

        /// <summary>
        /// Sends the Dispatch data to the BLL Dispatch method.  
        /// the BLL will process the data correctly based on the loadTender Type and Bid type of the selected dispatched load
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.2 on 12/22/2018
        ///     we now use the BidType and the LoadTenderType from the Dispatch Model
        ///     and call the same BLL method for all dispatching logic
        ///  Modified by RHR for v-8.2.0.119 on 09/24/19    
        ///  we now call GetDispatchAndBOLReportData for the return data
        /// </remarks>
        [HttpPost, ActionName("Dispatch")]
        public Models.Response Dispatch(DModel.Dispatch d)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            DModel.Dispatch[] oDispatched = new DModel.Dispatch[1];
            try
            {
                BLL.NGLBookRevenueBLL oBookRevBLL = new BLL.NGLBookRevenueBLL(Parameters);
                //get the tender type and bid type from the dispatch model setting defaults if the data is invalid.
                //note: the conversion from int to DTO.tblLoadTender.LoadTenderTypeEnum expects DTO.tblLoadTender.LoadTenderTypeEnum to be an int
                //      if the DTO.tblLoadTender.LoadTenderTypeEnum is not defined as an int an unexpected exception will be thrown to the caller
                //      do not use this code if the int is actually a nullable int.
                if (d.BookControl == 0) { d.DispatchLoadTenderType = (int)LoadTenderTypeEnum.RateShopping; }

                LoadTenderTypeEnum eTenderType = LoadTenderTypeEnum.LoadBoard;
                if (Enum.IsDefined(typeof(LoadTenderTypeEnum), d.DispatchLoadTenderType)) { eTenderType = (LoadTenderTypeEnum)d.DispatchLoadTenderType; }

                BidTypeEnum eBidType = BidTypeEnum.NGLTariff;
                if (Enum.IsDefined(typeof(BidTypeEnum), d.DispatchBidType)) { eBidType = (BidTypeEnum)d.DispatchBidType; }

                DTO.WCFResults oRet = oBookRevBLL.DispatchBid(ref d, eTenderType, eBidType);
                /* Possible Error Codes:
                ''' 0 of Blank  = Success (No Problems)
                ''' 1 = Connection Failure (Database Connection Problems)
                ''' 2 = Data Validation Failure (There Is a problem with the data being transmitted)
                ''' 3 = Failed (Total failure Check the Error Messages Returned)
                ''' 4 = Had Errors (Some failures were reported validate that your data is correct)
                ''' 400 =  invalid request; 
                ''' 401 = invalid or missing credentials; 
                ''' 403 = User not authorized to perform this operation
                * */
                
                if (oRet.Errors?.Count > 0) {
                    d.ErrorMessage = Utilities.formatWCFResultErrors(oRet);
                    if (String.IsNullOrWhiteSpace(d.ErrorCode) || d.ErrorCode == "0")
                    {   
                        d.ErrorCode = "4"; //Had Errors (Some failures were reported validate that your data is correct)
                    }
                        
                } else
                {
                    if (oRet.Success == false && (String.IsNullOrWhiteSpace(d.ErrorCode) || d.ErrorCode == "0"))
                    {
                        d.ErrorCode = "3"; //Failed (Total failure Check the Error Messages Returned)
                        string[] p = new[] { d.BidControl.ToString(), "Please  correct the problem and try again." };
                        oRet.AddMessage(DTO.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.E_DispatchFailure, p);
                    }
                }
                
              
                int id = d.BookControl;
                if (id > 0 && (String.IsNullOrWhiteSpace(d.ErrorCode) || d.ErrorCode == "0"))
                {                 
                    oDispatched = NGLLoadTenderData.GetDispatchAndBOLReportData(id, false);
                } else
                {                  
                    oDispatched[0] = d;
                }
                
                //oDispatched.Add(oDispatch);
                int iCount = 0;
                if (oDispatched != null) { iCount = oDispatched.Count(); }
                response = new Models.Response(oDispatched, iCount);
                if ((String.IsNullOrWhiteSpace(d.ErrorCode) || d.ErrorCode == "0"))
                {
                    response.Errors = Utilities.formatWCFResultErrors(oRet);
                    response.Warnings = Utilities.formatWCFResultWarnings(oRet);
                    response.Messages = Utilities.formatWCFResultMessages(oRet);
                }
                else
                {
                    response.Errors = d.ErrorMessage;
                }
            }    
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                if (fault != null) { response.StatusCode = fault.StatusCode; response.Errors = fault.formatMessage(); }
                else { response.StatusCode = HttpStatusCode.ServiceUnavailable; response.Errors = ex.Message; }
                return response;
            }
            return response;
        }


        [HttpGet, ActionName("GetDispatchReportData")]
        public Models.Response GetDispatchReportData(int id)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            //List<DModel.Dispatch> oDispatched = new List<DModel.Dispatch>();
            DModel.Dispatch[] oDispatched;
            try
            {
                //DModel.Dispatch oDispatch = NGLLoadTenderData.getDispatchDataToPrintByBookControl(id);
                oDispatched = NGLLoadTenderData.GetDispatchAndBOLReportData(id, false);
                //oDispatched.Add(oDispatch);
                int iCount = 0;
                if (oDispatched != null) { iCount = oDispatched.Count(); }
                response = new Models.Response(oDispatched, iCount);
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