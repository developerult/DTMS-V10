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
using DTran = Ngl.Core.Utility.DataTransformation;
using DModel = Ngl.FreightMaster.Data.Models;

namespace DynamicsTMS365.Controllers
{
    public class CompFinancialController : NGLControllerBase
    {
        #region " Constructors "

        /// <summary>Initializes the Page property by calling the base class constructor</summary>
        public CompFinancialController() : base(Utilities.PageEnum.CompanyFinancial) { }

        #endregion

        #region " Properties"

        /// <summary>This property is used for logging and error tracking</summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.CompFinancialController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        ////private DTO.CompCredit selectDTOData(LTS.vCompFin d)
        ////{
        ////    DTO.CompCredit dtoRecord = new DTO.CompCredit();
        ////    if (d != null)
        ////    {
        ////        dtoRecord = new DTO.CompCredit()
        ////        {
        ////            CompControl = d.CompFinCompControl,
        ////            CompNumber = d.CompNumber ?? 0,
        ////            CompName = d.CompName,
        ////            CompCreditAssigned = d.CompFinCreditLimit ?? 0,
        ////            CompCreditAvailable = d.CompFinCreditAvail ?? 0,
        ////            CompCreditUsed = d.CompFinCreditUsed ?? 0
        ////        };
        ////    }
        ////    return dtoRecord;
        ////}

        ////public static LTS.vCompFin selectLTSData(DTO.CompCredit d)
        ////{
        ////    LTS.vCompFin ltsRecord = new LTS.vCompFin();
        ////    if (d != null)
        ////    {
        ////        ltsRecord = new LTS.vCompFin()
        ////        {
        ////            CompFinCompControl = d.CompControl,
        ////            CompNumber = d.CompNumber,
        ////            CompName = d.CompName,
        ////            CompFinCreditLimit = d.CompCreditAssigned,
        ////            CompFinCreditAvail = d.CompCreditAvailable,
        ////            CompFinCreditUsed = d.CompCreditUsed
        ////        };
        ////    }
        ////    return ltsRecord;
        ////}

        #endregion

        #region " REST Services"
        /// POST 	/API/objectcontroller{data}  : Create a new object or Update a the current object if the control number exists
        /// GET 	/API/objectcontroller/{id}   : Get the object information where the control number = "id"
        /// PUT 	/API/objectcontroller/{data} : Update the object information stored in data
        /// DELETE	/API/objectcontroller/{id}   : Delete object where the control number = "id"

        [HttpGet, ActionName("Get")]
        public Models.Response Get(int id)
        {
            //Note: The id must always match a CompControl
            //The system looks up the last saved CompFinancial pk for this user and return the first Comp record found
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                if (id == 0) { id = readPagePrimaryKey(Parameters, Utilities.PageEnum.CompanyFinancial); } //get the parent control
                LTS.vCompFin[] records = new LTS.vCompFin[] { };
                DModel.AllFilters f = new DModel.AllFilters();
                addToFilters(ref f, "CompFinCompControl", id.ToString());
                int RecordCount = 0;
                LTS.vCompFin[] oData = NGLCompCreditData.GetLECompsFins(ref RecordCount, f);
                if (oData?.Count() > 0) {
                    records = new LTS.vCompFin[] { oData[0] };
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

        //Not Currently Used
        [HttpGet, ActionName("GetAllRecords")]
        public Models.Response GetAllRecords(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                //////save the page filter for the next time the page loads
                ////if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter); }
                ////DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);

                ////int id = f.ParentControl;
                ////if (id == 0)
                ////{
                ////    //get the parent control
                ////    id = readPagePrimaryKey(Parameters, Utilities.PageEnum.LoadBoard);
                ////}

                ////DAL.NGLBookRevenueData oDAL = new DAL.NGLBookRevenueData(Parameters);
                ////LTS.vLoadBoardRev oData = new LTS.vLoadBoardRev();
                ////LTS.vLoadBoardRev[] records = new LTS.vLoadBoardRev[1];
                ////oData = oDAL.GetvLoadBoardRev(id);
                ////if (oData != null)
                ////{
                ////    records[0] = oData;
                ////}

                ////response = new Models.Response(records, 1);
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

        //Modified By LVV on 7/14/20 for v-8.3.0.001 Task #20200609155542 - Credit Hold
        [HttpPost, ActionName("Post")]
        public Models.Response Post([FromBody]LTS.vCompFin data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                bool blnRet = false; //default
                int creditLimit = data.CompFinCreditLimit ?? 0;
                DAL.Models.ResultObject res = NGLCompCreditData.SavevCompFin(data);
                if (res != null)
                {
                    //if (res.Success) { response.Messages = res.SuccessMsg; } else { response.Errors = res.ErrMsg; }
                    blnRet = res.Success;
                }
                Array d = new bool[1] { blnRet };
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


        //Created By LVV on 7/14/20 for v-8.3.0.001 Task #20200609155542 - Credit Hold
        [HttpGet, ActionName("UpdateCreditNowSingle")]
        public Models.Response UpdateCreditNowSingle()
        {     
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int id = readPagePrimaryKey(Parameters, Utilities.PageEnum.CompanyFinancial); //The system looks up the last saved CompFinancial pk for this user and return the first Comp record found
                /* NOTE
                 * What happens when a Super or Admin is on the Comp page and changes the LE? 
                 * They won’t be able to see any companies because all companies for Legal Entities the user does not belong to are restricted for the user. 
                 * We added logic to automatically update the restricted company list when the user is assign / switches LE.
                 * Pretty sure that was a thing we did.
                 * Tested it and I see no records in the popupwindow, whereas before I implemented the fix of adding oSecureComp logic 
                 * when I switched LE on the page I could see the records in the window.
                 * I need a Business Rule for what to do in this situation 
                 */
                LTS.spUpdateCreditRoutine365Result res = NGLCompCreditData.UpdateCreditRoutine365Single(id);
                if (res != null)
                {
                    if (res.ErrNumber != 0)
                    {
                        response.StatusCode = HttpStatusCode.InternalServerError;
                        response.Errors = res.RetMsg;
                    }
                    else { response.Messages = res.RetMsg; }
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