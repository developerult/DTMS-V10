using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
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
    public class SettlementController : NGLControllerBase
    {

        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.2 on 08/29/2018 initializes the Page property by calling the base class constructor
        /// </summary>
        public SettlementController()
                : base(Utilities.PageEnum.Settlement)
	     {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.SettlementController";
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
            return GetSettlementItems(filter);
        }

        [HttpGet, ActionName("GetSettlementItems")]
        public Models.Response GetSettlementItems(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter, "SettlementFltr"); } //save the page filter for the next time the page loads
                DAL.Models.AllFilters f = new JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                DTO.SettlementItem[] setRes = new DTO.SettlementItem[] { };
                BLL.NGLTMS365BLL oBLL = new BLL.NGLTMS365BLL(Parameters);
                int count = 0;
                int RecordCount = 0;
                setRes = oBLL.GetSettlementItems365(ref RecordCount, f);
                if (setRes?.Count() > 0) { count = setRes.Length; }
                if (RecordCount > count) { count = RecordCount; }
                response = new Models.Response(setRes, count);
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


        [HttpPost, ActionName("SettlementQuickSave")]
        public Models.Response SettlementQuickSave([System.Web.Http.FromBody]DTO.SettlementItem[] settlements)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                BLL.NGLBookBLL oBLL = new BLL.NGLBookBLL(Parameters);

                List<string> savedCnsNumbers = new List<string>(); //note: we can only save once for each cns number so keep track             
                List<string> savedOrderNumbers = new List<string>(); //keep track of which order numbers are saved so we can show in success message               
                int savedRecordCount = 0; //set up variable to hold the number of records that are saved.               
                List<string> validationMessages = new List<string>(); //set up collection to hold problem items             
                List<string> errorMessages = new List<string>(); //set up collection to hold errors items

                //iterate claims and check payment and reconciled and then update
                foreach (DTO.SettlementItem item in settlements)
                {
                    int control = 0;
                    string orderNumber = "";
                    string cnsNumber = "";
                    string invoiceNumber = "";
                    decimal invoiceAmount = 0;

                    control = item.Control;
                    orderNumber = item.OrderNumber?.Trim();
                    cnsNumber = item.CnsNumber?.Trim();
                    invoiceNumber = item.InvoiceNumber?.Trim();
                    invoiceAmount = item.InvoiceAmount;

                    //Items can only be saved if the Status is null or if it is "N"
                    if (!string.IsNullOrWhiteSpace(item.Status) && item.Status.ToUpper() != "N") { continue; }
                    
                    //see if we saved it already
                    if (savedCnsNumbers.Contains(cnsNumber)) { continue; }

                    //validate - Invoice Amount > 0 AndAlso Invoice Number is not Nothing
                    if (invoiceAmount == 0)
                    {                      
                        validationMessages.Add("The invoice amount for Order #" + orderNumber + " must be greater than 0. This order has not been saved."); //add record to messages                
                        continue; //continue on without saving
                    }
                    if (string.IsNullOrEmpty(invoiceNumber))
                    {                       
                        validationMessages.Add("Please enter an invoice number for Order #" + orderNumber + ". This order has not been saved."); //add record to messages                      
                        continue; //continue on without saving
                    }
                    //save
                    try
                    {
                        int carrierControl = 0;
                        if(Parameters.UserCarrierControl > 0) { carrierControl = Parameters.UserCarrierControl; } else { carrierControl = item.BookCarrierControl; }
                        bool result = oBLL.InsertFreightBillWeb50(control, invoiceNumber, invoiceAmount, carrierControl);                        
                        if (result) //check result
                        {                           
                            if (cnsNumber.Trim().Length > 0){ if (!savedCnsNumbers.Contains(cnsNumber)){ savedCnsNumbers.Add(cnsNumber); } } //store in saved cns numbers if necessary                                  
                            savedOrderNumbers.Add("Order #" + orderNumber + " has been saved successfully."); //add success validation                           
                            savedRecordCount++; //increment record count
                        }
                        else { validationMessages.Add("Error saving item with Invoice Number #" + invoiceNumber + "."); } //add to problem items
                    }
                    catch (InvalidOperationException ex)
                    {
                        string ExceptionMessage = "Order #" + orderNumber;
                        if (cnsNumber.Trim().Length > 0) { ExceptionMessage += " with CNS number " + cnsNumber; }
                        ExceptionMessage += " has not been saved successfully. ";
                        if (ex.Message.ToUpper().Contains("E_DataValidationFailure".ToUpper()))
                        {
                            ExceptionMessage += " The freight bill number, " + invoiceNumber + ", has already been used and cannot be used again.";
                        }
                        else { ExceptionMessage += " " + ex.Message + "."; }
                        errorMessages.Add(ExceptionMessage);
                        continue;
                    }
                    catch (Exception ex)
                    {
                        FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                        string details = fault.formatMessage();
                        string ExceptionMessage = "Order #" + orderNumber;
                        if (cnsNumber.Trim().Length > 0) { ExceptionMessage += " with CNS number " + cnsNumber; }
                        //ExceptionMessage += " has not been saved successfully. Exception: " + ex.Message + ". " + details;
                        ExceptionMessage += " has not been saved successfully." + fault.Detail + ". ";
                        errorMessages.Add(ExceptionMessage);
                        continue;
                    }
                }
                Models.GenericResult gr = new Models.GenericResult();

                string strErrors = ""; //show error items
                if (errorMessages?.Count > 0)
                {                   
                    foreach (string e in errorMessages) { strErrors += e; }
                    gr.strField3 = strErrors;
                }
                string strInfos = ""; //show problem items
                if (validationMessages?.Count > 0)
                {                   
                    foreach (string v in validationMessages) { strInfos += v; }
                    gr.strField2 = strInfos;
                }
                string strSuccess = ""; //show success items
                if (savedRecordCount > 0)
                {
                    string str = savedRecordCount.ToString() + " item" + ((savedRecordCount == 1) ? " was" : "s were") + " saved successfully.";
                    if (savedOrderNumbers?.Count > 0)
                    {                       
                        foreach (string s in savedOrderNumbers) { strSuccess += s; }                      
                        gr.strField = strSuccess;
                    }                       
                }
                Array d = new Models.GenericResult[1] { gr };
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
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        ///<remarks>
        /// Modified by RHR for v-8.2.1.004 on 01/02/2020
        ///     added new logic to mark all fees as billed and not missing
        ///     this is to support now logic  created to support the AP Mass Entry screen
        ///     and the Quick Edit Freight bill logic
        /// Modified by RHR for v-8.5.3.006 on 02/17/2023 added Invoice Date
        ///     Allow updates to Carrier Cost
        ///</remarks>
        [HttpPost, ActionName("SettlementSave")]
        public Models.Response SettlementSave([System.Web.Http.FromBody]DAL.Models.SettlementSave s)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                BLL.NGLTMS365BLL bll = new BLL.NGLTMS365BLL(Parameters);

                //Modified By LVV on 2/16/2018 for v-8.1
                //Moved previous controller code into below BLL method so the exact same code can be called in the EDI (210In)
                //Modified by RHR for v-8.2.1.004 on 01/02/2020 
                //  we convert to a list, add fuel if available update the missing and billed properties 
                //  and convert back to array
                List<DAL.Models.SettlementFee> lFees = new List<DAL.Models.SettlementFee>();
                if (s.Fees != null ) { lFees = s.Fees.ToList(); }

                if (s.TotalFuel > 0 &&  !lFees.Any(y => (y.AccessorialCode == 2 || y.AccessorialCode == 9 || y.AccessorialCode == 15))){
                    DAL.Models.SettlementFee oFuelFee = new DAL.Models.SettlementFee
                    {
                        Control = 0,
                        AccessorialCode = 15,
                        Cost = s.TotalFuel,
                        Minimum = s.TotalFuel,
                        EDICode = "FUE",
                        Caption = "Flat Rate Fuel",
                        BilledFee = true,
                        MissingFee = false,
                        Pending = false
                    };
                    lFees.Add(oFuelFee);
                    s.TotalFuel = 0; // we must set to zero because the Settlement Save procedure will add fuel fees to this value                
                }  
                if (lFees != null  && lFees.Count() > 0)
                {
                    foreach (DAL.Models.SettlementFee f in lFees)
                    {
                        f.MissingFee = false;
                        f.BilledFee = true;
                    }
                }
                s.Fees = lFees.ToArray();
                DAL.Models.ResultObject oResults = bll.SettlementSave(s, false);
                //string strErrMsg = "";
                //strErrMsg = bll.SettlementSave(s, false);
                DAL.Models.ResultObject[] oRecords = new DAL.Models.ResultObject[1];
                oRecords[0] = oResults;
                response = new Models.Response(oRecords, 1);

                //if (!string.IsNullOrWhiteSpace(strErrMsg) && !strErrMsg.Contains("Success"))
                //{
                //    response.StatusCode = HttpStatusCode.InternalServerError;
                //    response.Errors = strErrMsg;
                //}
                //else
                //{
                //    Array d = new bool[1] { true };
                //    response = new Models.Response(d, 1);
                //}
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

        [HttpGet, ActionName("GetFBSHIDGrid365")]
        public Models.Response GetFBSHIDGrid365(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLBookData oBook = new DAL.NGLBookData(Parameters);
                int count = 0;
                LTS.vFBSHIDGrid365[] res = oBook.GetFBSHIDGrid365(filter);
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

        /// <summary>
        /// GenericResult.Control = CompControl
        /// GenericResult.intField1 = CarrierControl
        /// GenericResult.strField = SHID
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>
        /// GenericResult.blnField1 = ShowAuditFailReason
        /// GenericResult.blnField2 = ShowPendingFeeFailReason
        /// GenericResult.strField = APMessage
        /// </returns>
        [HttpGet, ActionName("GetAuditMessageVisibility")]
        public Models.Response GetAuditMessageVisibility(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                Models.GenericResult g = new JavaScriptSerializer().Deserialize<Models.GenericResult>(filter);
                Models.GenericResult[] ret = new Models.GenericResult[] { };
                DAL.NGLLookupDataProvider oLook = new DAL.NGLLookupDataProvider(Parameters);

                LTS.spCheckSettlementAuditMessageVisibilityResult spRes = oLook.CheckSettlementAuditMessageVisibility(g.Control, g.intField1, g.strField);

                if (spRes != null)
                {
                    Models.GenericResult gret = new Models.GenericResult();
                    bool ShowAuditFailReason = false;
                    bool ShowPendingFeeFailReason = false;
                    if (spRes.ShowAuditFailReason.HasValue) { ShowAuditFailReason = spRes.ShowAuditFailReason.Value; }
                    if (spRes.ShowPendingFeeFailReason.HasValue) { ShowPendingFeeFailReason = spRes.ShowPendingFeeFailReason.Value; }

                    gret.blnField1 = ShowAuditFailReason;
                    gret.blnField2 = ShowPendingFeeFailReason;
                    gret.strField = spRes.APMessage;
                    ret = new Models.GenericResult[1] { gret };
                }
                response = new Models.Response(ret, 1);
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

        [HttpGet, ActionName("GetAPCarrierCost")]
        public Models.Response GetAPCarrierCost(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (string.IsNullOrWhiteSpace(filter) ) { response.populateDefaultInvalidFilterResponseMessage(); return response; }
            if (!authenticateController(ref response)) { return response; }
            try
            {               
                DAL.NGLAPMassEntryData oDAL = new DAL.NGLAPMassEntryData(Parameters);
                DAL.Models.AllFilters f = new JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                if (string.IsNullOrWhiteSpace(f.filterValue)) { response.populateDefaultInvalidFilterResponseMessage(); return response; }
                int iAPControl = 0;
                decimal dRet = oDAL.GetAPCarrierCost(f.filterValue, ref iAPControl);
                Array ret = new Decimal[2] { dRet, Convert.ToDecimal( iAPControl )};               
                response = new Models.Response(ret, 2);
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

        
        [HttpGet, ActionName("GetBookFinAPActWgtBySHID")]
        public Models.Response GetBookFinAPActWgtBySHID(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (string.IsNullOrWhiteSpace(filter)) { response.populateDefaultInvalidFilterResponseMessage(); return response; }
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLAPMassEntryData oDAL = new DAL.NGLAPMassEntryData(Parameters);
                DAL.Models.AllFilters f = new JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                if (string.IsNullOrWhiteSpace(f.filterValue)) { response.populateDefaultInvalidFilterResponseMessage(); return response; }
                double dRet = oDAL.GetBookFinAPActWgtBySHID(f.filterValue);
                Array ret = new double[1] { dRet };
                response = new Models.Response(ret, 1);
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
        //GetSettlementBookTotalMilesBySHID
            [HttpGet, ActionName("GetSettlementBookTotalMilesBySHID")]
        public Models.Response GetSettlementBookTotalMilesBySHID(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (string.IsNullOrWhiteSpace(filter)) { response.populateDefaultInvalidFilterResponseMessage(); return response; }
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLAPMassEntryData oDAL = new DAL.NGLAPMassEntryData(Parameters);
                DAL.Models.AllFilters f = new JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                if (string.IsNullOrWhiteSpace(f.filterValue)) { response.populateDefaultInvalidFilterResponseMessage(); return response; }
                double dRet = oDAL.GetSettlementBookTotalMilesBySHID(f.filterValue);
                Array ret = new double[1] { dRet };
                response = new Models.Response(ret, 1);
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

        [HttpGet, ActionName("GetSettlementFuelForSHID")]
        public Models.Response GetSettlementFuelForSHID(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (string.IsNullOrWhiteSpace(filter)) { response.populateDefaultInvalidFilterResponseMessage(); return response; }
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLAPMassEntryData oDAL = new DAL.NGLAPMassEntryData(Parameters);
                DAL.Models.AllFilters f = new JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                if (string.IsNullOrWhiteSpace(f.filterValue)) { response.populateDefaultInvalidFilterResponseMessage(); return response; }
                decimal dRet = oDAL.GetSettlementFuelForSHID(f.filterValue,false);
                Array ret = new Decimal[1] { dRet };
                response = new Models.Response(ret, 1);
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




        [HttpGet, ActionName("GetSettlementQuickEdit")]
        public Models.Response GetSettlementQuickEdit(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                //Get the filters from the Settlement Grid
                var ups = readPageSettings("SettlementFltr", Parameters, Utilities.PageEnum.Settlement);
                DAL.Models.AllFilters f = new DAL.Models.AllFilters();
                if (ups?.Count() > 0)
                {
                    if(ups[0] != null && !string.IsNullOrWhiteSpace(ups[0].UserPSMetaData)) { f = new JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(ups[0].UserPSMetaData); }
                }              
                DTO.SettlementItem[] records = new DTO.SettlementItem[] { };
                BLL.NGLTMS365BLL oBLL = new BLL.NGLTMS365BLL(Parameters);
                int count = 0;
                int RecordCount = 0;
                DTO.SettlementItem[] setRes = oBLL.GetSettlementItems365(ref RecordCount, f);
                if (setRes?.Count() > 0)
                {
                    records = setRes.Where(n => n.Status.ToUpper() != "FAILED" && n.Status.ToUpper() != "PA" && n.Status.ToUpper() != "M").ToArray(); //Modified By LVV on 6/25/20 for v-8.2.1.008 Task #20200609164051 - Remove Failed Audit records from the Quick Freight Bill data entry dialog
                    if (records?.Count() > 0) { count = records.Length; }
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



        //Added By LVV 3/15/20 - For new Detailed Freight Bill Entry screen
        [HttpGet, ActionName("GetSettlementFBDEData")]
        public Models.Response GetSettlementFBDEData(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLBookFeePendingData oBook = new DAL.NGLBookFeePendingData(Parameters);
                DAL.Models.SettlementFBDEData[] records = new DAL.Models.SettlementFBDEData[] { };
                int count = 0;
                DAL.Models.SettlementFBDEData res = oBook.GetSettlementFBDEData(filter,false);
                if (res != null)
                {
                    records = new DAL.Models.SettlementFBDEData[1] { res };
                    count = 1;
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
        /// Called by the REST service to populate the Fees ddls on the Settlement Detailed Freight Bill Entry Screen.
        /// Gets the Fees based on the Legal Entity Carrier Accessorial Configuration as well as the Accessorial Allocation Type.
        /// Does not include Fuel Accessorials.
        /// Only returns Accessorials where AllowCarrierUpdates is true.
        /// Required fields in AllFilters: LEAdminControl, CarrierControlFrom, ParentControl (AllocationType).
        ///   These fields are used to filter the initial IQueryable and come from the backend. The filters in 
        ///   FilterValues are from the user and can be applied in addition on top of the initial query results.
        /// </summary>
        /// <param name="filter">
        /// Required fields in AllFilters: LEAdminControl, CarrierControlFrom, ParentControl (AllocationType).
        /// These fields are used to filter the initial IQueryable and come from the backend.
        /// The filters in FilterValues are from the user and can be applied in addition on top of the initial query results.
        /// </param>
        /// <returns>Models.Response</returns>
        /// <remarks>Created By LVV on 3/19/20 for v-8.2.1.006</remarks>
        [HttpGet, ActionName("GetLECarFeesByAllocationType")]
        public Models.Response GetLECarFeesByAllocationType(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.Models.AllFilters f = new JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                LTS.vLECarrierAccessorial[] records = new LTS.vLECarrierAccessorial[] { };
                int RecordCount = 0;
                int count = 0;
                applyDefaultSort(ref f, "Caption", true);
                records = NGLLECarrierAccessorialData.GetLECarFeesByAllocationType(ref RecordCount, f);
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



        [HttpGet, ActionName("GetUniqueOrdersBySHID")]
        public Models.Response GetUniqueOrdersBySHID(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLBookData oBook = new DAL.NGLBookData(Parameters);
                LTS.vFBSHIDGrid365[] records = new LTS.vFBSHIDGrid365[] { };
                int count = 0;
                records = oBook.GetUniqueOrdersBySHID(filter);
                if (records?.Count() < 1) { count = records.Count(); }
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

        [HttpGet, ActionName("GetUniqueOrigBySHID")]
        public Models.Response GetUniqueOrigBySHID(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLBookData oBook = new DAL.NGLBookData(Parameters);
                LTS.vFBSHIDGrid365[] records = new LTS.vFBSHIDGrid365[] { };
                int count = 0;
                records = oBook.GetUniqueOrigBySHID(filter);
                if (records?.Count() < 1) { count = records.Count(); }
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

        [HttpGet, ActionName("GetUniqueDestBySHID")]
        public Models.Response GetUniqueDestBySHID(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLBookData oBook = new DAL.NGLBookData(Parameters);
                LTS.vFBSHIDGrid365[] records = new LTS.vFBSHIDGrid365[] { };
                int count = 0;
                records = oBook.GetUniqueDestBySHID(filter);
                if (records?.Count() < 1) { count = records.Count(); }
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


        #endregion
    }
}