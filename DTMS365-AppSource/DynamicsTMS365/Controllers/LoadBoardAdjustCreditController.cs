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
using DModel = Ngl.FreightMaster.Data.Models;

namespace DynamicsTMS365.Controllers
{
    public class LoadBoardAdjustCreditController : NGLControllerBase
    {
        #region " Constructors "

        /// <summary>Initializes the Page property by calling the base class constructor</summary>
        public LoadBoardAdjustCreditController() : base(Utilities.PageEnum.LoadBoard){ }

        #endregion

        #region " Properties"
        
        /// <summary>This property is used for logging and error tracking</summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.LoadBoardAdjustCreditController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        private DTO.CompCredit selectDTOData(LTS.vCompFin d)
        {
            DTO.CompCredit dtoRecord = new DTO.CompCredit();
            if (d != null)
            {
                dtoRecord = new DTO.CompCredit()
                {
                    CompControl = d.CompFinCompControl,
                    CompNumber = d.CompNumber ?? 0,
                    CompName = d.CompName,
                    CompCreditAssigned = d.CompFinCreditLimit ?? 0,
                    CompCreditAvailable = d.CompFinCreditAvail ?? 0,
                    CompCreditUsed = d.CompFinCreditUsed ?? 0
                };
            }
            return dtoRecord;
        }

        public static LTS.vCompFin selectLTSData(DTO.CompCredit d)
        {
            LTS.vCompFin ltsRecord = new LTS.vCompFin();
            if (d != null)
            {
                ltsRecord = new LTS.vCompFin()
                {
                    CompFinCompControl = d.CompControl,
                    CompNumber = d.CompNumber,
                    CompName = d.CompName,
                    CompFinCreditLimit = d.CompCreditAssigned,
                    CompFinCreditAvail = d.CompCreditAvailable,
                    CompFinCreditUsed = d.CompCreditUsed
                };
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
                LTS.vLoadBoardAdjustCredit[] oRecords = new LTS.vLoadBoardAdjustCredit[1];
                oRecords[0] = NGLCompCreditData.GetvLoadBoardAdjustCredit(id);
                response = new Models.Response(oRecords, 1);

                //DTO.CompCredit[] records = new DTO.CompCredit[1];
                //int RecordCount = 0;
                //DModel.AllFilters f = new DModel.AllFilters();
                //addToFilters(ref f, "CompFinCompControl", id.ToString());
                //LTS.vCompFin[] oData = NGLCompCreditData.GetLECompsFins(ref RecordCount, f);
                //if (oData?.Count() > 0) {
                //    records[0] = selectDTOData(oData[0]);
                //}
                //response = new Models.Response(records, 1);
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
                LTS.vLoadBoardAdjustCredit[] oRecords = new LTS.vLoadBoardAdjustCredit[1];
                if (id == 0) { id = readPagePrimaryKey(Parameters, Utilities.PageEnum.LoadBoard); }
                DTO.Book bk = NGLBookData.GetBookFilteredNoChildren(id);
                if(bk != null)
                {
                    oRecords[0] = NGLCompCreditData.GetvLoadBoardAdjustCredit(bk.BookCustCompControl);
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
                LTS.vLoadBoardAdjustCredit[] oRecords = new LTS.vLoadBoardAdjustCredit[1];
                int id = 0;
                int.TryParse(filter, out id);
                if (id == 0) { id = readPagePrimaryKey(Parameters, Utilities.PageEnum.LoadBoard); }         
                DTO.Book bk = NGLBookData.GetBookFilteredNoChildren(id);
                if (bk != null)
                {
                    oRecords[0] = NGLCompCreditData.GetvLoadBoardAdjustCredit(bk.BookCustCompControl);
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
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.2 on 12/17/2018
        ///  we now call the correct BookRevBLL.GenerateQuote based on the selected switch
        ///  added logic to process wcfResponse Message, Warning, and Error dictionary messages
        /// </remarks>
        [HttpPost, ActionName("Post")]
        public Models.Response Post([FromBody]LTS.vLoadBoardAdjustCredit data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                //if there is no data we cannot save
                Models.GenericResult oGResult = new Models.GenericResult() { Control = 0, strField = "undefined" };
                if (data == null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Errors = Utilities.getLocalizedMsg("E_CannotSaveNoData");
                    response.Data = new Models.GenericResult[] { oGResult }; //what is the deal with this? Why is this necessary?
                    response.Count = 1;
                    return response;
                }               
                //make sure the nullable field has a value - don't want to set it to 0 if it is null because we don't want to overwrite any existing values in the table with bad data - do not set the limit to 0 unless the user actually type it
                if (!data.CompCreditAssigned.HasValue) {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Errors = "Credit Assigned cannot be null";
                    response.Data = new Models.GenericResult[] { oGResult };
                    response.Count = 1;
                    return response;
                }
                int creditLimit = data.CompCreditAssigned.Value;
                int compControl = data.CompControl;
                DModel.ResultObject oRes = NGLCompCreditData.SaveCreditLimit(compControl, creditLimit);
                if (oRes != null)
                {
                    //if the save is not successful then we stop execution and return an error message to the user
                    if (!oRes.Success) {
                        response.StatusCode = HttpStatusCode.BadRequest;
                        response.Errors = oRes.ErrMsg;
                        return response;
                    }
                    //if the save is successful then we continue and update the credit information for the company
                    LTS.spUpdateCreditRoutine365Result res = NGLCompCreditData.UpdateCreditRoutine365Single(compControl);
                    if (res != null)
                    {
                        if (res.ErrNumber != 0)
                        {
                            response.StatusCode = HttpStatusCode.InternalServerError;
                            response.Errors = res.RetMsg;
                        }
                        else {
                            response.Messages = res.RetMsg;
                            bool[] oRecords = new bool[1] { true };
                            response = new Models.Response(oRecords, 1);
                        }
                    }
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