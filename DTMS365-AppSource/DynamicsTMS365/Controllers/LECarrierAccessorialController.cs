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
using DTran = Ngl.Core.Utility.DataTransformation;
using DModel = Ngl.FreightMaster.Data.Models;

namespace DynamicsTMS365.Controllers
{
    public class LECarrierAccessorialController : NGLControllerBase
    {
        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.LECarrierAccessorialController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        private static LTS.tblLECarrierAccessorial selectLTSData(Models.LECarrierAccessorial d)
        {
            LTS.tblLECarrierAccessorial ltsRecord = new LTS.tblLECarrierAccessorial();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "LECAUpdated", "tblLegalEntityCarrier", "_tblLegalEntityCarrier", "Accessorial", "_Accessorial" };
                string sMsg = "";
                ltsRecord = (LTS.tblLECarrierAccessorial)DTran.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.LECAUpdated = bupdated == null ? new byte[0] : bupdated;
                }
            }
            return ltsRecord;
        }

        private Models.vLECarrierAccessorial selectModelData(LTS.vLECarrierAccessorial d)
        {
            Models.vLECarrierAccessorial modelRecord = new Models.vLECarrierAccessorial();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "LECAUpdated", "tblLegalEntityCarrier", "_tblLegalEntityCarrier", "Accessorial", "_Accessorial" };
                string sMsg = "";
                modelRecord = (Models.vLECarrierAccessorial)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.LECAUpdated.ToArray()); }
            }
            return modelRecord;
        }

        private static LTS.tblLECarrierAccessorial selectLTSData(Models.CarrierLegalAccessorialXref d)
        {
            LTS.tblLECarrierAccessorial ltsRecord = new LTS.tblLECarrierAccessorial();
            if (d != null)
            {
                ltsRecord.LECAAccessorialCode = d.AccessorialCode;
                ltsRecord.LECACaption = d.Caption;
                ltsRecord.LECAEDICode = d.EDICode;
                ltsRecord.LECAAutoApprove = d.AutoApprove;
                ltsRecord.LECAAllowCarrierUpdates = d.AllowCarrierUpdates;
                ltsRecord.LECAAccessorialVisible = d.AccessorialVisible;
                ltsRecord.LECAApproveToleranceLow = d.ApproveToleranceLow;
                ltsRecord.LECAApproveToleranceHigh = d.ApproveToleranceHigh;
                ltsRecord.LECAApproveTolerancePerLow = d.ApproveTolerancePerLow;
                ltsRecord.LECAApproveTolerancePerHigh = d.ApproveTolerancePerHigh;
                ltsRecord.LECAAverageValue = d.AverageValue;
                ltsRecord.LECADynamicAverageValue = d.DynamicAverageValue;
            }
            return ltsRecord;
        }

        #endregion


        #region " REST Services"

        /// <summary>
        /// Gets All the Child vLECarrierAccessorial Records filtered by LECAControl passed in as id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, ActionName("GetByParent")]
        public Models.Response GetByParent(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.Models.AllFilters f = new DAL.Models.AllFilters { ParentControl = id, sortName = "LECAControl", sortDirection = "Asc" };
                Models.vLECarrierAccessorial[] records = new Models.vLECarrierAccessorial[] { };
                int RecordCount = 0;
                int count = 0;
                LTS.vLECarrierAccessorial[] oData = NGLLECarrierAccessorialData.GetLECarrierAccessorials(ref RecordCount, f);
                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                    if (RecordCount > count) { count = RecordCount; }
                    records = (from e in oData select selectModelData(e)).ToArray();
                }
                ////response = new Models.Response(oData, count);
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

        //Added By LVV on 6/18/20
        [HttpGet, ActionName("GetRecords")]
        public Models.Response GetRecords()
        {
            return GetRecords("");
        }

        //Added By LVV on 6/18/20
        [HttpGet, ActionName("GetRecords")]
        public Models.Response GetRecords(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateFilter(ref response, filter)) { return response; } //Verfiy that the filters object is not null
            return GetAllRecords(filter);
        }

        //Added By LVV on 6/18/20
        [HttpGet, ActionName("GetAllRecords")]
        public Models.Response GetAllRecords(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter, "LECarAssrlGridFltr"); } //save the page filter for the next time the page loads
                DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DModel.AllFilters>(filter);

                applyDefaultSort(ref f, "LECAControl", true);

                Models.vLECarrierAccessorial[] records = new Models.vLECarrierAccessorial[] { };
                int RecordCount = 0;
                int count = 0;
                LTS.vLECarrierAccessorial[] oData = NGLLECarrierAccessorialData.GetLECarrierAccessorials(ref RecordCount, f);
                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                    if (RecordCount > count) { count = RecordCount; }
                    records = (from e in oData select selectModelData(e)).ToArray();
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

        [HttpPost, ActionName("Post")]
        public Models.Response Post([System.Web.Http.FromBody]Models.LECarrierAccessorial data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                LTS.tblLECarrierAccessorial ltsCont = selectLTSData(data); //Note: There is something stupid going on in the caller because the view name does not match the lts table names because it is old. I can't change the field names yet because of the Settlement page method GetLECAForSettlement and I dont feel like messing with that today
                DModel.ResultObject res = NGLLECarrierAccessorialData.InsertOrUpdateLECarrierAccessorial(ltsCont);
                DModel.ResultObject[] oRecords = new DModel.ResultObject[1] { res };
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

        [HttpDelete, ActionName("Delete")]
        public Models.Response Delete(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                bool blnRet = NGLLECarrierAccessorialData.DeleteLECarrierAccessorials(id);
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
        /// GenericResult.intField1 = LEAControl
        /// GenericResult.intField2 = CarrierControl
        /// Note:
        /// Security on this method says if user not
        /// SuperUser they can only look up for their
        /// own LE
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet, ActionName("GetLECarrierAccessorial")]
        public Models.Response GetLECarrierAccessorial(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLLECarrierAccessorialData oLECAData = new DAL.NGLLECarrierAccessorialData(Parameters);
                LTS.vLECarrierAccessorial[] retVal = new LTS.vLECarrierAccessorial[] { };
                int RecordCount = 0;
                int count = 0;
                // get the take and skip parameters int skip = request["skip"] == null ? 0 :
                int skip = request["skip"] == null ? 0 : int.Parse(request["skip"]);
                int take = request["take"] == null ? 500 : int.Parse(request["take"]);
                string sortExpression = "LECAControl Asc";
                string filterWhere = request["filter[filters][0][value]"];
                //string oOp = request["filter[filters][0][operator]"];

                Models.GenericResult ofilter = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Models.GenericResult>(filter);
                if (ofilter.intField1 == 0) { ofilter.intField1 = Parameters.UserLEControl; }

                retVal = oLECAData.GetLECarrierAccessorial(ref RecordCount, ofilter.intField1, ofilter.intField2, filterWhere, sortExpression, 1, 0, skip, take);
                if (retVal != null && retVal.Count() > 0)
                {
                    count = retVal.Length;
                }

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

        [HttpDelete, ActionName("DeleteLECarrierAccessorials")]
        public Models.Response DeleteLECarrierAccessorials(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            bool blnSuccess = false;
            try
            {
                DAL.NGLLECarrierAccessorialData oLECAData = new DAL.NGLLECarrierAccessorialData(Parameters);

                if (id == 0)
                {
                    response.StatusCode = HttpStatusCode.OK;
                    // return the HTTP Response.
                    return response;
                }

                blnSuccess = oLECAData.DeleteLECarrierAccessorials(id);

                if (blnSuccess)
                {
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Errors = string.Format("Cannot delete the tblLECarrierAccessorial record with Control {0}", id.ToString());

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
        /// Called from CarrierAccessorialApproval.aspx
        /// </summary>
        /// <param name="clax"></param>
        /// <returns></returns>
        [HttpPost, ActionName("SaveLECarrierAccessorial")]
        public Models.Response SaveLECarrierAccessorial([System.Web.Http.FromBody]Models.CarrierLegalAccessorialXref clax)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {               
                if (clax.LEAdminControl == 0) { clax.LEAdminControl = Parameters.UserLEControl; } //If LEAdminControl = 0 then use the Users LEControl
                LTS.tblLECarrierAccessorial ltsCont = selectLTSData(clax);
                DModel.ResultObject res = NGLLECarrierAccessorialData.InsertOrUpdateLECarrierAccessorial(ltsCont, clax.LEAdminControl, clax.CarrierControl);
                DModel.ResultObject[] oRecords = new DModel.ResultObject[1] { res };
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
        /// GenericResult.intField1 = CopyFromCompControl
        /// GenericResult.intArray = CopyToCompControls
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost, ActionName("CopyLECarrierAccessorialConfig")]
        public Models.Response CopyLECarrierAccessorialConfig([System.Web.Http.FromBody]Models.GenericResult data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int copyFromLECarControl = data.intField1;
                int[] copyToLeCarControls = data.intArray;

                string strRet = NGLLECarrierAccessorialData.CopyLECarrierAccessorialConfig(copyFromLECarControl, copyToLeCarControls);
                if (String.IsNullOrWhiteSpace(strRet))
                {
                    bool[] oRecords = new bool[1] { true };
                    response = new Models.Response(oRecords, 1);
                }
                else
                {
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Errors = strRet;
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
        /// Copies all visible accessorial codes from tblAccessorial to tblLECarrierAccessorial using the default values from tblAccessorial.
        /// Parameter data is a GenericResult object where GenericResult.intArray is an array of LECarControls
        /// </summary>
        /// <param name="data">GenericResult.intArray = copyToLeCarControls (array of LECarControls)</param>
        /// <returns></returns>
        [HttpPost, ActionName("CopyDefaultAccessorialsToLECarriers")]
        public Models.Response CopyDefaultAccessorialsToLECarriers([FromBody]Models.GenericResult data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int[] copyToLeCarControls = data.intArray;
                string strRet = NGLLECarrierAccessorialData.CopyDefaultAccessorialsToLECarriers(copyToLeCarControls);
                if (string.IsNullOrWhiteSpace(strRet))
                {
                    bool[] oRecords = new bool[1] { true };
                    response = new Models.Response(oRecords, 1);
                }
                else
                {
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Errors = strRet;
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
        /// DEPRECIATED - NO LONGER USED - LVV 3/19/20 v-8.2.1.006
        /// REPLACED BY SettlementController.GetLECarFeesByAllocationType
        /// GenericResult.strField = CompLegalEntity
        /// GenericResult.intField1 = CompControl
        /// GenericResult.intField2 = CarrierControl
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet, ActionName("GetLECAForSettlement")]
        public Models.Response GetLECAForSettlement(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Errors = "This method has been Depreciated. Please contact technical support.";
                return response;

                ////Models.GenericResult ofilter = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Models.GenericResult>(filter);
                ////DAL.NGLLECarrierAccessorialData oLECAData = new DAL.NGLLECarrierAccessorialData(Parameters);
                ////LTS.vLECarrierAccessorial[] retVal = new LTS.vLECarrierAccessorial[] { };
                ////int RecordCount = 0;
                ////int count = 0;
                ////int skip = request["skip"] == null ? 0 : int.Parse(request["skip"]);
                ////int take = request["take"] == null ? 500 : int.Parse(request["take"]);
                ////string filterWhere = "(AccessorialCode <> 2 AND AccessorialCode <> 9 AND AccessorialCode <> 15)";
                ////string sortExpression = "LECAControl Asc";
                ////if (string.IsNullOrWhiteSpace(ofilter.strField))
                ////{
                ////    response = new Models.Response(retVal, count);
                ////    return response;
                ////}
                ////retVal = oLECAData.GetLECAForSettlement(ref RecordCount, ofilter.strField, ofilter.intField2, filterWhere, sortExpression, 1, 0, skip, take);
                ////if (retVal != null && retVal.Count() > 0)
                ////{
                ////    count = retVal.Length;
                ////}
                ////response = new Models.Response(retVal, count);
            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }
            //return response;
        }



        #endregion

    }
}