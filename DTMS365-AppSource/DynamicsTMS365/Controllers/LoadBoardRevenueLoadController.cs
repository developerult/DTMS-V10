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
    public class LoadBoardRevenueLoadController : NGLControllerBase
    {

        #region " Constructors "

        ///<summary> Initializes the Page property by calling the base class constructor</summary>
        public LoadBoardRevenueLoadController() : base(Utilities.PageEnum.LoadBoardRevenueLoad) { }

        #endregion

        #region " Properties"

        ///<summary> This property is used for logging and error tracking.</summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.LoadBoardRevenueLoadController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion


        #region " Data Translation"

        private Models.vBookRevenue selectModelData(LTS.vBookRevenue d)
        {
            Models.vBookRevenue modelRecord = new Models.vBookRevenue();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "BookUpdated", "rowguid", "Book", "BookLoad" };
                string sMsg = "";
                modelRecord = (Models.vBookRevenue)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.BookUpdated.ToArray()); }
            }
            return modelRecord;
        }

        public static LTS.vBookRevenue selectLTSData(Models.vBookRevenue d)
        {
            LTS.vBookRevenue ltsRecord = new LTS.vBookRevenue();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "BookUpdated", "rowguid", "Book", "BookLoad" };
                string sMsg = "";
                ltsRecord = (LTS.vBookRevenue)DTran.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.BookUpdated = bupdated == null ? new byte[0] : bupdated;
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

        //Not Currently Used
        [HttpGet, ActionName("Get")]
        public Models.Response Get(int id)
        {
            //Note: CRUD not currently supported in this controller
            var response = new Models.Response(); //new HttpResponseMessage();
            response.populateDefaultInvalidFilterResponseMessage();
            return response;
            ////Note: The id must always match a BookControl
            ////The system looks up the last saved BookControl pk for this user
            ////An invalid parent key Error is returned if the data does not match
            //var response = new Models.Response(); //new HttpResponseMessage();
            //if (!authenticateController(ref response)) { return response; }
            //try
            //{
            //    int count = 0;
            //    if (id == 0) { id = readPagePrimaryKey(Parameters, Utilities.PageEnum.LoadBoard); } //get the parent control
            //    DAL.NGLBookRevenueData oDAL = new DAL.NGLBookRevenueData(Parameters);
            //    LTS.vCMLoadBoardRevLoadSumTemplate oData = new LTS.vCMLoadBoardRevLoadSumTemplate();
            //    LTS.vCMLoadBoardRevLoadSumTemplate[] records = new LTS.vCMLoadBoardRevLoadSumTemplate[1];
            //    oData = NGLBookRevenueData.GetLoadBoardRevLoadSummaryData(id);
            //    if (oData != null) { records[0] = oData; count = 1; }
            //    response = new Models.Response(records, count);
            //}
            //catch (Exception ex)
            //{
            //    FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
            //    response.StatusCode = fault.StatusCode;
            //    response.Errors = fault.formatMessage();
            //    return response;
            //}
            //return response;
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

        //Not Currently Used
        [HttpGet, ActionName("GetAllRecords")]
        public Models.Response GetAllRecords(string filter)
        {
            ////Note: CRUD not currently supported in this controller
            //var response = new Models.Response(); //new HttpResponseMessage();
            //response.populateDefaultInvalidFilterResponseMessage();
            //return response;

            //Note: The id must always match a BookControl
            //The system looks up the last saved BookControl pk for this user
            //An invalid parent key Error is returned if the data does not match
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int count = 0;

                //Modified by RHR for v-8.5.4.004 on 12/06/2023 new BookControl setting 
                //int id = readPagePrimaryKey(Parameters, Utilities.PageEnum.LoadBoard); //get the parent control
                int id = 0;
                LTS.vCMLoadBoardRevLoadSumTemplate[] records = new LTS.vCMLoadBoardRevLoadSumTemplate[] { };
                id = readBookControlPageSetting(id);
                if (id == 0)
                {
                    return new Models.Response(records, 0);
                }

                LTS.vCMLoadBoardRevLoadSumTemplate oData = NGLBookRevenueData.GetLoadBoardRevLoadSummaryData(id);
               
                if (oData != null) { records = new LTS.vCMLoadBoardRevLoadSumTemplate[1] { oData }; count = 1; }
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
        public Models.Response Post([System.Web.Http.FromBody]LTS.vLoadBoardRev data)
        {
            //Note: CRUD not currently supported in this controller
            var response = new Models.Response(); //new HttpResponseMessage();
            response.populateDefaultInvalidFilterResponseMessage();
            return response;
            //var response = new Models.Response(); //new HttpResponseMessage();
            //if (!authenticateController(ref response)) { return response; }
            //try
            //{
            //    ////DAL.NGLBookRevenueData oDAL = new DAL.NGLBookRevenueData(Parameters);
            //    ////int id = data.BookControl;
            //    ////bool blnRet = oDAL.SavevLoadBoardRevChanges(data);
            //    ////if (blnRet)
            //    ////{
            //    ////    BLL.NGLBookRevenueBLL oBLL = new BLL.NGLBookRevenueBLL(Parameters);
            //    ////    oBLL.RecalculateBookRevenueFreightCostsNoReturn(id);
            //    ////}
            //    ////LTS.vLoadBoardRev oData = new LTS.vLoadBoardRev();
            //    ////bool[] results = new bool[1];
            //    ////results[0] = blnRet;
            //    ////response = new Models.Response(results, 1);
            //}
            //catch (Exception ex)
            //{
            //    FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
            //    response.StatusCode = fault.StatusCode;
            //    response.Errors = fault.formatMessage();
            //    return response;
            //}
            //return response;
        }


        [HttpGet, ActionName("GetLoadBoardRevLoadSummary")]
        public Models.Response GetLoadBoardRevLoadSummary(int id)
        {
            //Note: The id must always match a BookControl
            //The system looks up the last saved BookControl pk for this user
            //An invalid parent key Error is returned if the data does not match
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                LTS.vCMLoadBoardRevLoadSumTemplate[] records = new LTS.vCMLoadBoardRevLoadSumTemplate[] { };
                int count = 0;
                if (id == 0) {
                    return new Models.Response(records, count);
                    //id = readPagePrimaryKey(Parameters, Utilities.PageEnum.LoadBoard); 
                }
                LTS.vCMLoadBoardRevLoadSumTemplate oData = NGLBookRevenueData.GetLoadBoardRevLoadSummaryData(id);
                if (oData != null) { records = new LTS.vCMLoadBoardRevLoadSumTemplate[1] { oData }; count = 1; }
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