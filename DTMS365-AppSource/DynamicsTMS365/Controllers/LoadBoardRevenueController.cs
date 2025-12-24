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
    public class LoadBoardRevenueController : NGLControllerBase
    {

        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.2 on 08/29/2018 initializes the Page property by calling the base class constructor
        /// </summary>
        public LoadBoardRevenueController()
                : base(Utilities.PageEnum.LoadBoardRevenue)
	     {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.LoadBoardRevenueController";
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
                modelRecord = (Models.vBookRevenue)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
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
                ltsRecord = (LTS.vBookRevenue)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
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
        //    POST 	/API/objectcontroller{data
        //}  : Create a new object or Update a the current object if the control number exists
        /// 
        /// GET 	/API/objectcontroller/{id} : Get the object information where the control number = "id"
        /// PUT 	/API/objectcontroller/{data} : Update the object information stored in data
        /// DELETE	/API/objectcontroller/{id} : Delete object where the control number = "id"
        /// 

        [HttpGet, ActionName("Get")]
        public Models.Response Get(int id)
        {
            //Note: The id must always match a CarrTarEquipControl associated with the select tariff using CarrTarEquipCarrTarControl
            //the system looks up the last saved tariff pk for this user and return the first Service record found
            //an invalid parent key Error is returned if the data does not match
            //If id is zero and no records are found the client should configure the page to add a new service record.
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            { 
               

                if (id == 0)
                {
                    //get the parent control
                    id = readPagePrimaryKey(Parameters, Utilities.PageEnum.LoadBoard);
                }
               
                DAL.NGLBookRevenueData oDAL = new DAL.NGLBookRevenueData(Parameters);
                LTS.vLoadBoardRev oData = new LTS.vLoadBoardRev();
                LTS.vLoadBoardRev[] records = new LTS.vLoadBoardRev[1]; 
                oData = oDAL.GetvLoadBoardRev(id);
                if (oData != null)
                {
                    records[0] = oData;
                }

                response = new Models.Response(records, 1);
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
                //save the page filter for the next time the page loads
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter); }
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);

                //Modified by RHR for v-8.5.4.004 on 12/06/2023 new BookControl setting 
                LTS.vLoadBoardRev[] records = new LTS.vLoadBoardRev[1];
                int id = readBookControlPageSetting(f.ParentControl);
                if (id == 0)
                {
                    return new Models.Response(records, 0);
                }

                DAL.NGLBookRevenueData oDAL = new DAL.NGLBookRevenueData(Parameters);
                LTS.vLoadBoardRev oData = new LTS.vLoadBoardRev();
               
                oData = oDAL.GetvLoadBoardRev(id);
                if (oData != null)
                {                   
                    records[0] = oData;
                }

                response = new Models.Response(records, 1);
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
        public Models.Response Post([System.Web.Http.FromBody]LTS.vLoadBoardRev data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLBookRevenueData oDAL = new DAL.NGLBookRevenueData(Parameters);
                int id = data.BookControl;
                bool blnRet = oDAL.SavevLoadBoardRevChanges(data);    
                if (blnRet)
                {
                    BLL.NGLBookRevenueBLL oBLL = new BLL.NGLBookRevenueBLL(Parameters);
                    oBLL.RecalculateBookRevenueFreightCostsNoReturn(id);

                }
                LTS.vLoadBoardRev oData = new LTS.vLoadBoardRev();
                bool[] results = new bool[1];
                results[0] = blnRet;               
                response = new Models.Response(results, 1);
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


        #region " public methods"


        #endregion

    }
}