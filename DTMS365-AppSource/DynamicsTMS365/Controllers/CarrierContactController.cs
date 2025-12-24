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
    public class CarrierContactController : NGLControllerBase
    {

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.CarrierContactController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        private Models.CarrierContact selectModelData(LTS.CarrierCont d)
        {
            Models.CarrierContact modelRecord = new Models.CarrierContact();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "CarrierContUpdated", "Carrier", "_Carrier" };
                string sMsg = "";
                modelRecord = (Models.CarrierContact)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.CarrierContUpdated.ToArray()); }
            }
            return modelRecord;
        }

        public static LTS.CarrierCont selectLTSData(Models.CarrierContact d)
        {
            LTS.CarrierCont ltsRecord = new LTS.CarrierCont();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "CarrierContUpdated", "Carrier", "_Carrier" };
                string sMsg = "";
                ltsRecord = (LTS.CarrierCont)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.CarrierContUpdated = bupdated == null ? new byte[0] : bupdated;
                }
            }
            return ltsRecord;
        }

        #endregion

        #region " REST Services"

        /// <summary>
        /// Gets one CarrierCont record using the CarrierContControl passed in as id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, ActionName("Get")]
        public Models.Response Get(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.Models.AllFilters f = new DAL.Models.AllFilters { filterName = "CarrierContControl", filterValue = id.ToString() };
                int count = 0;

                Models.CarrierContact[] records = new Models.CarrierContact[] { };
                LTS.CarrierCont oData = NGLCarrierContData.GetCarrierContact(id);
                if (oData != null)
                {
                    Models.CarrierContact r = selectModelData(oData);
                    records = new Models.CarrierContact[] { r };
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
        /// Gets All the Child CarrierCont Records filtered by CarrierContLECarControl passed in as id
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
                DAL.Models.AllFilters f = new DAL.Models.AllFilters { ParentControl = id };
                Models.CarrierContact[] records = new Models.CarrierContact[] { };
                int RecordCount = 0;
                int count = 0;
                LTS.CarrierCont[] oData = NGLCarrierContData.GetCarContsByLECarrier(ref RecordCount, f);
                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                    if (RecordCount > count) { count = RecordCount; }
                    records = (from e in oData
                               orderby e.CarrierContName ascending
                               select selectModelData(e)).ToArray();
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
        public Models.Response Post([System.Web.Http.FromBody]Models.CarrierContact data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                LTS.CarrierCont ltsCont = selectLTSData(data);
                ltsCont.CarrierContact800 = Utilities.removeNonNumericText(ltsCont.CarrierContact800);
                ltsCont.CarrierContactFax = Utilities.removeNonNumericText(ltsCont.CarrierContactFax);
                ltsCont.CarrierContactPhone = Utilities.removeNonNumericText(ltsCont.CarrierContactPhone);

                NGLCarrierContData.InsertOrUpdateCarrierContByLECarrier(ltsCont);
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

        [HttpDelete, ActionName("Delete")]
        public Models.Response Delete(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                bool blnRet = NGLCarrierContData.DeleteCarrierContact(id);
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

        //I don't think this is used anywhere (10/24/19)
        [HttpGet, ActionName("GetCarContsByLECarrier")]
        public Models.Response GetCarContsByLECarrier(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();            
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);

                Models.CarrierContact[] records = new Models.CarrierContact[] { };
                int RecordCount = 0;
                int count = 0;

                LTS.CarrierCont[] conts = NGLCarrierContData.GetCarContsByLECarrier(ref RecordCount, f);

                if (conts != null && conts.Count() > 0)
                {
                    count = conts.Count();
                    if (RecordCount > count) { count = RecordCount; }

                    records = (from e in conts
                               orderby e.CarrierContName ascending
                               select selectModelData(e)).ToArray();
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

        //I don't think this is used anywhere (10/24/19)
        [HttpPost, ActionName("SaveCarrierContByLECarrier")]
        public Models.Response SaveCarrierContByLECarrier([System.Web.Http.FromBody]Models.CarrierContact cont)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                LTS.CarrierCont ltsCont = selectLTSData(cont);

                NGLCarrierContData.InsertOrUpdateCarrierContByLECarrier(ltsCont);

                Array d = new int[1] { 1 };
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

        //I don't think this is used anywhere (10/24/19)
        [HttpDelete, ActionName("DeleteCarrierContact")]
        public Models.Response DeleteCarrierContact(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            bool blnSuccess = false;
            try
            {

                if (id == 0)
                {
                    response.StatusCode = HttpStatusCode.OK;
                    // return the HTTP Response.
                    return response;
                }

                blnSuccess = NGLCarrierContData.DeleteCarrierContact(id);

                if (blnSuccess)
                {
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Errors = string.Format("Cannot delete the CarrierContact record with Control {0}", id.ToString());
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