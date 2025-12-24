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
using DTran = Ngl.Core.Utility.DataTransformation;
using DModel = Ngl.FreightMaster.Data.Models;

namespace DynamicsTMS365.Controllers
{
    public class POHdrController : NGLControllerBase
    {
        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.2 on 08/29/2018 initializes the Page property by calling the base class constructor
        /// </summary>
        public POHdrController()
                : base(Utilities.PageEnum.OrderPreview)
        {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.POHdrController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        private Models.vPOHdr selectModelData(LTS.vPOHdr d)
        {
            Models.vPOHdr modelRecord = new Models.vPOHdr();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "POHdrUpdated", "_POItems", "POItems" };
                string sMsg = "";
                modelRecord = (Models.vPOHdr)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.POHdrUpdated.ToArray()); }
                //modelRecord.POHdrControl = d.POHdrControl.ToString();
            }
            return modelRecord;
        }

        private LTS.POHdr selectLTSData(Models.vPOHdr d)
        {
            LTS.POHdr ltsRecord = new LTS.POHdr();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "POHdrUpdated", "_POItems", "POItems" };
                string sMsg = "";
                ltsRecord = (LTS.POHdr)DTran.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.POHdrUpdated = bupdated == null ? new byte[0] : bupdated;
                    //long temp = 0;
                    //long.TryParse(d.POHdrControl, out temp);
                    //ltsRecord.POHdrControl = temp;
                }
            }
            return ltsRecord;
        }


        #endregion

        #region " REST Services"

        /// POST 	/API/objectcontroller{data}  : Create a new object or Update a the current object if the control number exists
        /// GET 	/API/objectcontroller/{id} : Get the object information where the control number = "id"
        /// PUT 	/API/objectcontroller/{data} : Update the object information stored in data
        /// DELETE	/API/objectcontroller/{id} : Delete object where the control number = "id"

        ////Not Currently Used
        //[HttpGet, ActionName("Get")]
        //public Models.Response Get(int id)
        //{
        //    var response = new Models.Response(); //new HttpResponseMessage();
        //    if (!authenticateController(ref response)) { return response; }
        //    try
        //    {
        //        ////int RecordCount = 0;
        //        int count = 0;
        //        ////DAL.Models.AllFilters f = new DAL.Models.AllFilters();
        //        ////f.filterName = "BookControl";
        //        ////f.filterValue = id.ToString();
        //        ////DAL.NGLBookLoadBoard oDAL = new DAL.NGLBookLoadBoard(Parameters);

        //        LTS.vPOHdr[] oData = new LTS.vPOHdr[] { };
        //        ////oData = oDAL.GetBookLoadBoards(f, ref RecordCount);
        //        ////if (oData != null && oData.Count() > 0)
        //        ////{
        //        ////    count = oData.Count();
        //        ////    if (RecordCount > count) { count = RecordCount; }
        //        ////}
        //        response = new Models.Response(oData, count);
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

        public Models.Response Get(long id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                Models.vPOHdr[] records = new Models.vPOHdr[1];
                int count = 0;
                string sPohdrControl = id.ToString();
                LTS.vPOHdr oData = NGLPOHdrData.GetPOHdrFiltered365(sPohdrControl);
                if (oData != null)
                {
                    count = 1;
                    records[0] = selectModelData(oData);
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

        //[HttpGet, ActionName("Get")]
        //public Models.Response Get(string filter)
        //{
        //    var response = new Models.Response(); //new HttpResponseMessage();
        //    if (!authenticateController(ref response)) { return response; }
        //    try
        //    {
        //        Models.vPOHdr[] records = new Models.vPOHdr[1];
        //        int count = 0;
        //        LTS.vPOHdr oData = NGLPOHdrData.GetPOHdrFiltered365(filter);
        //        if (oData != null)
        //        {
        //            count = 1;
        //            records[0] = selectModelData(oData);
        //        }
        //        response = new Models.Response(records, count);
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

        //Not Currently Used
        [HttpGet, ActionName("GetByParent")]
        public Models.Response GetByParent(long id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                Models.vPOHdr[] records = new Models.vPOHdr[] { };
                int RecordCount = 0;
                int count = 0;
                ////DModel.BookingMenuInfo b = NGLBookData.GetBookingMenuInfo(id);
                ////if (b != null)
                ////{
                ////    count = 1;
                ////    Models.CarrierDropLoad cdl = new Models.CarrierDropLoad { CarrierDropNumber = b.CarrierNumber, CarrierDropProNumber = b.BookProNumber, CarrierDropDate = DateTime.Now, CarrierDropTime = DateTime.Now };
                ////    records = new Models.CarrierDropLoad[] { cdl };
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
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter, "OrderPrevFltr"); } //save the page filter for the next time the page loads
                DModel.AllFilters f = new JavaScriptSerializer().Deserialize<DModel.AllFilters>(filter);
                Models.OrderPreviewFilters d = null;
                if (f != null && !string.IsNullOrWhiteSpace(f.Data)) { d = new JavaScriptSerializer().Deserialize<Models.OrderPreviewFilters>(f.Data); }
                if (d == null) { response.populateDefaultInvalidFilterResponseMessage(); return response; }
                Models.vPOHdr[] records = new Models.vPOHdr[] { };
                int RecordCount = 0;
                int count = 0;
                int intNatAcctNbr = 0;
                int intCompNumber = 0;
                int intFrtTyp = 0;
                int.TryParse(d.NatAcctDDLValue, out intNatAcctNbr);
                int.TryParse(d.CompDDLValue, out intCompNumber);
                intFrtTyp = d.FrtTypDDLValue;
                LTS.vPOHdr[] oData = NGLPOHdrData.GetPOHdrsFiltered365(ref RecordCount, f, intCompNumber, intNatAcctNbr, intFrtTyp);
                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                    records = (from e in oData select selectModelData(e)).ToArray();
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

        //Not Currently Used
        [HttpPost, ActionName("Post")]
        public Models.Response Post([System.Web.Http.FromBody]Models.vPOHdr data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {

                data.POHDROrigContactFax = Utilities.removeNonNumericText(data.POHDROrigContactFax);
                data.POHDROrigContactPhone = Utilities.removeNonNumericText(data.POHDROrigContactPhone);
                data.POHDRDestContactFax = Utilities.removeNonNumericText(data.POHDRDestContactFax);
                data.POHDRDestContactPhone = Utilities.removeNonNumericText(data.POHDRDestContactPhone);
                
                ////DAL.NGLCarrierDropLoadData oDAL = new DAL.NGLCarrierDropLoadData(Parameters);
                bool[] oRecords = new bool[1] { false };

                ////DTO.CarrierDropLoad cdl = new DTO.CarrierDropLoad
                ////{
                ////    CarrierDropNumber = data.CarrierDropNumber,
                ////    CarrierDropContact = data.CarrierDropContact,
                ////    CarrierDropProNumber = data.CarrierDropProNumber,
                ////    CarrierDropReason = data.CarrierDropReason,
                ////    CarrierDropDate = data.CarrierDropDate,
                ////    CarrierDropTime = data.CarrierDropTime,
                ////    CarrierDropReasonLocalized = data.CarrierDropReasonLocalized,
                ////    CarrierDropReasonKeys = data.CarrierDropReasonKeys
                ////};
                ////var oData = oDAL.CreateRecord(cdl);

                ////if (oData != null)
                ////{
                ////    oRecords = new bool[1] { true };
                ////}

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



        [HttpPost, ActionName("DeletePOHdr")]
        public Models.Response DeletePOHdr([System.Web.Http.FromBody]Models.vPOHdr data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                long control = 0;
                long.TryParse(data.POHdrControl, out control);
                bool blnRet = NGLPOHdrData.DeletePOHdr(control);
                bool[] oRecords = new bool[1] { blnRet };
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
        /// GenericResult.strField = strPOHDRModVerify
        /// GenericResult.strField2 = strOrderNumber
        /// GenericResult.strField3 = strVendorNumber
        /// GenericResult.strField4 = strBookProNumber
        /// GenericResult.intField1 = intOrderSequence
        /// GenericResult.intField2 = intDefCompNumber
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost, ActionName("ProcessSingle")]
        public Models.Response ProcessSingle([System.Web.Http.FromBody]Models.GenericResult gr)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                BLL.NGLOrderImportBLL oBLL = new BLL.NGLOrderImportBLL(Parameters);
                string strPOHDRModVerify = gr.strField;
                string strOrderNumber = gr.strField2;
                string strVendorNumber = gr.strField3;
                string strBookProNumber = gr.strField4;
                int intOrderSequence = gr.intField1;
                int intDefCompNumber = gr.intField2;
                var blnSuccess = oBLL.ImportPOHdrRecord(strPOHDRModVerify, strOrderNumber, intOrderSequence, intDefCompNumber, strVendorNumber, strBookProNumber, true);
                bool[] oRecords = new bool[1] { blnSuccess };
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

        [HttpPost, ActionName("ProcessAll")]
        public Models.Response ProcessAll()
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                var ups = readPageSettings("OrderPrevFltr", Parameters, Utilities.PageEnum.OrderPreview);
                DModel.AllFilters f = new DModel.AllFilters();
                Models.OrderPreviewFilters d = null;
                if (ups?.Count() > 0)
                {
                    if (ups[0] != null && !string.IsNullOrWhiteSpace(ups[0].UserPSMetaData))
                    {
                        f = new JavaScriptSerializer().Deserialize<DModel.AllFilters>(ups[0].UserPSMetaData);
                        if (f != null && !string.IsNullOrWhiteSpace(f.Data)) { d = new JavaScriptSerializer().Deserialize<Models.OrderPreviewFilters>(f.Data); }
                    }
                }
                if (d == null) { response.populateDefaultInvalidFilterResponseMessage(); return response; }
                int RecordCount = 0;
                int intNatAcctNbr = 0;
                int intCompNumber = 0;
                int intFrtTyp = 0;
                int.TryParse(d.NatAcctDDLValue, out intNatAcctNbr);
                int.TryParse(d.CompDDLValue, out intCompNumber);
                intFrtTyp = d.FrtTypDDLValue;
                List<string> strExceptions = new List<string>();
                LTS.vPOHdr[] orders = NGLPOHdrData.GetPOHdrsFiltered365(ref RecordCount, f, intCompNumber, intNatAcctNbr, intFrtTyp);
                try
                {
                    if (orders?.Count() < 1) { return response; }
                    int intTotal = orders.Count();
                    int intRecord = 0;
                    int intPercentComplete = 0;
                    foreach (LTS.vPOHdr o in orders)
                    {
                        intRecord += 1;
                        intPercentComplete = 100 / (intTotal / intRecord);
                        if (!o.POHDRHoldLoad)
                        {
                            string strBookProNumber = o.POHDRPRONumber;
                            string strOrderNumber = o.POHDROrderNumber;
                            int intOrderSequence = o.POHDROrderSequence;
                            int intDefCompNumber = 0;
                            if (o.POHDRDefaultCustomer.HasValue) { intDefCompNumber = o.POHDRDefaultCustomer.Value; }
                            string strVendorNumber = o.POHDRvendor;
                            string strPOHDRModVerify = o.POHDRModVerify.ToUpper();
                            //Check if we are allowed to process this record (we do not process Split Orders)
                            if (strPOHDRModVerify != "FINALIZED"
                                 && strPOHDRModVerify != "DELETE-F"
                                 && strPOHDRModVerify != "NO LANE"
                                 && strPOHDRModVerify != "NEW TRAN-F"
                                 && strPOHDRModVerify != "NEW TRAN"
                                 && strPOHDRModVerify != "SPLIT NEW COMP"
                                 && strPOHDRModVerify != "SPLIT NEW TRAN"
                                 && strPOHDRModVerify != "SPLIT ORDER"
                                 && strPOHDRModVerify != "SPLIT DELETED")
                            {
                                try
                                {
                                    //Import the data
                                    BLL.NGLOrderImportBLL oBLL = new BLL.NGLOrderImportBLL(Parameters);
                                    var blnSuccess = oBLL.ImportPOHdrRecord(strPOHDRModVerify, strOrderNumber, intOrderSequence, intDefCompNumber, strVendorNumber, strBookProNumber, true);
                                }
                                catch (InvalidOperationException ex)
                                {
                                    //if we get this message we cannot continue
                                    string strOrderNumberLabel = Utilities.getLocalizedMsg("BookCarrOrderNumber");
                                    string E_InvalidOperationException = Utilities.getLocalizedMsg("E_InvalidOperationException");
                                    strExceptions.Add(string.Concat(strOrderNumberLabel, " -- ", strOrderNumber, ": ", E_InvalidOperationException, System.Environment.NewLine, ex.Message));
                                    break;
                                }
                                catch (TimeoutException timeoutEx)
                                {
                                    //if we get this message we cannot continue
                                    string strOrderNumberLabel = Utilities.getLocalizedMsg("BookCarrOrderNumber");
                                    string strTimedout = Utilities.getLocalizedMsg("Timedout");
                                    string strOperationTimedout = Utilities.getLocalizedMsg("OperationTimedout");                                 
                                    strExceptions.Add(string.Concat(strOrderNumberLabel, " -- ", strOrderNumber, ": ", strTimedout, System.Environment.NewLine, strOperationTimedout));
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    //for an unexpected error we save the error, skip this order and move on to the next
                                    //strExceptions.Add(PaneSettings.MainInterface.FMMessage.GetFMError(ex, "OrderPreview.ImportPOHrs"))
                                }
                            }
                        }
                        //Me.OrderPreviewWorker.ReportProgress(intPercentComplete, intRecord & " of " & intTotal & "  " & PaneSettings.MainInterface.LocalizeString("OrdersProcessed"))
                    }
                }
                catch (Exception ex)
                {
                    //strExceptions.Add(PaneSettings.MainInterface.FMMessage.GetFMError(ex, "OrderPreview.ImportPOHrs"))
                }
                if (strExceptions?.Count() > 0) {
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Errors = String.Join("", strExceptions);
                }
                else
                {
                    bool[] oRecords = new bool[1] { true };
                    response = new Models.Response(oRecords, 1);
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

        [HttpPost, ActionName("ToggleHoldStatus")]
        public Models.Response ToggleHoldStatus([System.Web.Http.FromBody]Models.vPOHdr data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                long temp = 0;
                long.TryParse(data.POHdrControl, out temp);
                NGLPOHdrData.ToggleHoldStatus(temp);
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
        /// GenericResult.strField = orderNumber
        /// GenericResult.intField1 = sequenceNumber
        /// GenericResult.intField2 = customerNumber
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet, ActionName("ViewImportChanges")]
        public Models.Response ViewImportChanges(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            if (!authenticateFilter(ref response, filter)) { return response; } //Verfiy that the filters object is not null
            try
            {
                Models.GenericResult gr = new JavaScriptSerializer().Deserialize<Models.GenericResult>(filter);
                BLL.NGLOrderImportBLL oBLL = new BLL.NGLOrderImportBLL(Parameters);
                int count = 0;
                string orderNumber = gr.strField;
                int sequenceNumber = gr.intField1;
                int customerNumber = gr.intField2;
                DModel.CompareChanges[] oData = oBLL.GetImportChanges365(orderNumber, sequenceNumber, customerNumber);
                if (oData?.Count() > 0) { count = oData.Count(); } else { oData = new DModel.CompareChanges[] { }; }
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


        #endregion
    }
}