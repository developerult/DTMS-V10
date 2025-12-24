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
using DModel = Ngl.FreightMaster.Data.Models;

namespace DynamicsTMS365.Controllers
{
    public class POItemController : NGLControllerBase
    {
        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.2 on 08/29/2018 initializes the Page property by calling the base class constructor
        /// </summary>
        public POItemController()
                : base(Utilities.PageEnum.OrderPreview)
        {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.POItemController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        private Models.vPOItem selectModelData(LTS.vPOItem d)
        {
            Models.vPOItem modelRecord = new Models.vPOItem();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "POItemUpdated" };
                string sMsg = "";
                modelRecord = (Models.vPOItem)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.POItemUpdated.ToArray()); }
                //modelRecord.POHdrControl = d.POHdrControl.ToString();
            }
            return modelRecord;
        }

        private LTS.vPOItem selectLTSData(Models.vPOItem d)
        {
            LTS.vPOItem ltsRecord = new LTS.vPOItem();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "POItemUpdated" };
                string sMsg = "";
                ltsRecord = (LTS.vPOItem)DTran.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.POItemUpdated = bupdated == null ? new byte[0] : bupdated;
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

                LTS.vPOHdr[] oData = new LTS.vPOHdr[] { };
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

        [HttpGet, ActionName("GetByParent")]
        public Models.Response GetByParent(long id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {

                int count = 0;
                Models.vPOItem[] records = new Models.vPOItem[] { };
                LTS.vPOItem[] oData = NGLPOItemData.GetPOItemsByParent(id);
                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
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
        ///  AllFilters.Data = e.POHDROrderNumber;
        ///  AllFilters.ParentControl = e.POHDROrderSequence;
        ///  AllFilters.CompNumberFrom = e.POHDRDefaultCustomer;
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet, ActionName("GetAllRecords")]
        public Models.Response GetAllRecords(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DModel.AllFilters>(filter);
                int RecordCount = 0;
                int count = 0;
                Models.vPOItem[] records = new Models.vPOItem[] { };
                LTS.vPOItem[] oData = NGLPOItemData.GetPOItemsFiltered365(ref RecordCount, f);
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

        [HttpPost, ActionName("Post")]
        public Models.Response Post([System.Web.Http.FromBody]Models.vPOHdr data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
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
    }
}