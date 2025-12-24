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
    /// <summary>
    /// API REST Service for Load Board Financial Data
    /// </summary>
    /// <remarks>
    /// created by RHR for v-8.5.2.006 on 12/22/2022 added booking financial data
    /// </remarks>
    public class LoadBoardFinancialController : NGLControllerBase
    {

        #region " Constructors "

        /// <summary>Initializes the Page property by calling the base class constructor</summary>
        public LoadBoardFinancialController() : base(Utilities.PageEnum.LoadBoardFinancial) { }

        #endregion

        #region " Properties"

        /// <summary>This property is used for logging and error tracking.</summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.LoadBoardFinancialController";
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
        /// POST 	/API/objectcontroller{data}  : Create a new object or Update a the current object if the control number exists
        /// GET 	/API/objectcontroller/{id} : Get the object information where the control number = "id"
        /// PUT 	/API/objectcontroller/{data} : Update the object information stored in data
        /// DELETE	/API/objectcontroller/{id} : Delete object where the control number = "id"

        [HttpGet, ActionName("Get")]
        public Models.Response Get(int id)
        {
            //Note: The id must always match a BookControl associated with the select Load
            //The system looks up the last saved Book pk for this user and return the financial data found
            //An invalid parent key Error is returned if the data does not match
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                //Modified by RHR for v-8.5.4.004 on 12/06/2023 new BookControl setting 
                //if (id == 0) { id = readPagePrimaryKey(Parameters, Utilities.PageEnum.LoadBoard); } //get the parent control
                Models.vBookFinancial[] records = new Models.vBookFinancial[1];
                id = readBookControlPageSetting(id);
                if (id == 0)
                {
                    return new Models.Response(records, 0);
                }
                LTS.vBookFinancial oData = NGLBookFinancialData.GetvBookFinancial(id);
                DateTime currentTime = DateTime.Now;
                if (oData != null) { records[0] = Models.vBookFinancial.selectModelData( oData); }
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

        //Not Currently Used
        [HttpGet, ActionName("GetAllRecords")]
        public Models.Response GetAllRecords(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
               //do nothing not supported
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
        public Models.Response Post([FromBody] Models.vBookFinancial data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                LTS.vBookFinancial record = Models.vBookFinancial.selectLTSData(data);
                record.BookFinAPExportDate = Utilities.updateTimeSpan(record.BookFinAPExportTimeString, record.BookFinAPExportDate);
                record.BookDateLoad = Utilities.updateTimeSpan(record.BookDateLoadTimeString, record.BookDateLoad);
                record.BookFinCheckClearedDate = Utilities.updateTimeSpan(record.BookFinCheckClearedTimeString, record.BookFinCheckClearedDate);
                record.BookFinCommCreditPayDate = Utilities.updateTimeSpan(record.BookFinCommCreditPayTimeString, record.BookFinCommCreditPayDate);
                record.BookFinCommPayDate = Utilities.updateTimeSpan(record.BookFinCommPayTimeString, record.BookFinCommPayDate);
                record.BookFinAPPayDate = Utilities.updateTimeSpan(record.BookFinAPPayTimeString, record.BookFinAPPayDate);
                record.BookFinAPBillInvDate = Utilities.updateTimeSpan(record.BookFinAPBillInvTimeString, record.BookFinAPBillInvDate);
                record.BookFinAPBillNoDate = Utilities.updateTimeSpan(record.BookFinAPBillNoTimeString, record.BookFinAPBillNoDate);
                record.BookFinARPayDate = Utilities.updateTimeSpan(record.BookFinARPayTimeString, record.BookFinARPayDate);
                record.BookFinARInvoiceDate = Utilities.updateTimeSpan(record.BookFinARInvoiceTimeString, record.BookFinARInvoiceDate);
                
                bool blnRet = NGLBookFinancialData.UpdatevBookFinancial(record);
                bool[] results = new bool[1] { blnRet };
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