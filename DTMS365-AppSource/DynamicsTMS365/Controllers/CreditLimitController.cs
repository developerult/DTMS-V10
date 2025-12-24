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

namespace DynamicsTMS365.Controllers
{
    public class CreditLimitController : NGLControllerBase
    {
        //#region " Constructors "

        ///// <summary>Initializes the Page property by calling the base class constructor</summary>
        //public CreditLimitController() : base(Utilities.PageEnum.CompanyFinancial) { }

        //#endregion

        #region " Properties"

        /// <summary>This property is used for logging and error tracking</summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.CreditLimitController";
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
        /// GET 	/API/objectcontroller/{id}   : Get the object information where the control number = "id"
        /// PUT 	/API/objectcontroller/{data} : Update the object information stored in data
        /// DELETE	/API/objectcontroller/{id}   : Delete object where the control number = "id"

        //Not Currently Used
        [HttpGet, ActionName("Get")]
        public Models.Response Get(int id)
        {
            //Note: The id must always match a CompControl
            //The system looks up the last saved CompFinancial pk for this user and return the first Comp record found
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                //if (id == 0) { id = readPagePrimaryKey(Parameters, Utilities.PageEnum.CompanyFinancial); } //get the parent control
                //LTS.vCompFin[] records = new LTS.vCompFin[] { };
                //int count = 0;
                //DTO.CompCredit oData = NGLCompCreditData.GetCompCreditFiltered(Control: id);
                //if (oData != null)
                //{
                //    LTS.vCompFin fin = selectLTSData(oData);
                //    records = new LTS.vCompFin[] { fin };
                //    count = 1;
                //}
                //response = new Models.Response(records, count);
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
                //if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter, "CreditLimitFltr"); } //save the page filter for the next time the page loads
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                DTO.CompCredit[] records = new DTO.CompCredit[] { };
                int RecordCount = 0;
                int count = 0;
                LTS.vCompFin[] oData = NGLCompCreditData.GetLECompsFins(ref RecordCount, f);
                if (oData?.Count() > 0)
                {
                    count = RecordCount;
                    records = (from e in oData orderby e.CompName ascending select selectDTOData(e)).ToArray();
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
        public Models.Response Post([FromBody]Models.GenericResult data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                bool blnRet = false; //default
                int compControl = data.Control;
                int creditLimit = data.intField1;
                DAL.Models.ResultObject res = NGLCompCreditData.SaveCreditLimit(compControl, creditLimit);
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


        [HttpPost, ActionName("SaveCreditLimit")]
        public Models.Response SaveCreditLimit([FromBody]Models.GenericResult[] data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                string strErrs = "";
                string strMsg = "";
                string sSep = "";
                foreach (Models.GenericResult g in data)
                {
                    int compControl = g.Control;
                    int creditLimit = g.intField1;
                    DAL.Models.ResultObject res = NGLCompCreditData.SaveCreditLimit(compControl, creditLimit);
                    if (res != null)
                    {
                        //The success message is the same for everything so we only need to show it 1 time
                        //The error message is specific to the record so we need to concat those
                        if (res.Success) { strMsg = res.SuccessMsg; } else { strErrs = sSep + res.ErrMsg; sSep = " "; }
                    }
                }
                if (!string.IsNullOrWhiteSpace(strMsg)) { response.Messages = strMsg; }
                if (!string.IsNullOrWhiteSpace(strErrs)) { response.Errors = strErrs; }                    
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
        /// If the provided value is 0 then we use the user's LE. 
        /// This is because it is easier to get the user's LE from the Controller than the JavaScript.
        ///   There are pages where SuperUsers can view data from other Legal Entities so this is why I
        ///   provided a way to send in a LEControl that is not that of the logged in user. 
        ///   This might change though based on new Business Rules Rob needs to determine because the
        ///   filter on the grid in the Credit Limit Assign Window includes a filter for only Companies that
        ///   are not restricted for the user. We added code so when a user is assigned/switches Legal Entity
        ///   that it automatically updates the Restricted Company list based on the Legal Entity. So even if a 
        ///   SuperUser can open the window and switch for example the Company page to view Companies from Legal
        ///   Entities they do not belong to, when they open the Credit Limit Assign window they will not be able to see those
        ///   because they are restricted comapnies for the SuperUser. So we have to figure out how to reconcile this.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost, ActionName("UpdateCreditRoutine365LE")]
        public Models.Response UpdateCreditRoutine365LE([FromBody]Models.GenericResult data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int leaControl = data.Control;
                if (leaControl == 0) { leaControl = Parameters.UserLEControl; } //If the provided value is 0 then we use the user's LE
                LTS.spUpdateCreditRoutine365Result res = NGLCompCreditData.UpdateCreditRoutine365LE(leaControl);
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