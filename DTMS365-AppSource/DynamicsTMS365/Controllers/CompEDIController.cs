using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL = Ngl.FreightMaster.Data;
using LTS = Ngl.FreightMaster.Data.LTS;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using BLL = NGL.FM.BLL;
using DTran = Ngl.Core.Utility.DataTransformation;

namespace DynamicsTMS365.Controllers
{
    public class CompEDIController : NGLControllerBase
    {
        #region " Constructors "
        /// <summary>
        /// Created by ManoRama for LELanePreferredCarriers 30/jul/2020 initializes the Page property by calling the base class constructor
        /// </summary>
        public CompEDIController()
            : base(Utilities.PageEnum.CompEDI)
        {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.CompEDIController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion
        #region " Data Translation"

        private Models.CompEDI selectModelData(LTS.vCompEDI d)
        {
            Models.CompEDI modelRecord = new Models.CompEDI();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "CompEDIUpdated" ,"CompEDICompControl","Comp"};
                string sMsg = "";
                modelRecord = (Models.CompEDI)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.CompEDIUpdated.ToArray()); }
            }

            return modelRecord;
        }


        private LTS.CompEDI selectLTSData(Models.CompEDI d)
        {
            LTS.CompEDI ltsRecord = new LTS.CompEDI();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "CompEDIUpdated","Comp" };
                string sMsg = "";
                ltsRecord = (LTS.CompEDI)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.CompEDIUpdated = bupdated == null ? new byte[0] : bupdated;

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

            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            try
            {
                int RecordCount = 0;
                int count = 0;
                DAL.Models.AllFilters f = new DAL.Models.AllFilters();
                if (id != 0)
                {
                    f.filterName = "CompEDIControl";
                    f.filterValue = id.ToString();
                }
                //get the parent control
                f.ParentControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.CompanyMaint);
                DAL.NGLCompEDIData  oDAL = new DAL.NGLCompEDIData (Parameters);
                Models.CompEDI[] records = new Models.CompEDI[] { };
                LTS.vCompEDI[] oData = new LTS.vCompEDI[] { };
                oData = oDAL.GetCompEDIs(f, ref RecordCount).Where(x => x.CompEDIControl == id).ToArray();
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
                //Error handler
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }

            // return the HTTP Response.
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
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            try
            {
                int RecordCount = 0;
                int count = 0;
                //save the page filter for the next time the page loads
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter); }
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                f.ParentControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.CompanyMaint);
                DAL.NGLCompEDIData  oDAL = new DAL.NGLCompEDIData (Parameters);
                Models.CompEDI[] records = new Models.CompEDI[] { };
                LTS.vCompEDI[] oData = new LTS.vCompEDI[] { };
                oData = oDAL.GetCompEDIs(f, ref RecordCount);
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
                //Error handler
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }

            // return the HTTP Response.
            return response;
        }

        [HttpPost, ActionName("Post")]
        public Models.Response Post([System.Web.Http.FromBody]Models.CompEDI data)
        {
            //create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                if (data.CompEDICompControl == 0) { data.CompEDICompControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.CompanyMaint); }
                DAL.NGLCompEDIData  oDAL = new DAL.NGLCompEDIData (Parameters);
                 LTS.CompEDI oChanges = selectLTSData(data);

                bool blnRet = oDAL.SaveCompanyEDI(oChanges);

                bool[] oRecords = new bool[1];

                oRecords[0] = blnRet;

                response = new Models.Response(oRecords, 1);


            }
            catch (Exception ex)
            {
                //Error handler
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
            //create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLCompEDIData  oDAL = new DAL.NGLCompEDIData (Parameters);
                bool blnRet = oDAL.DeleteCompanyEDI(id);
                bool[] oRecords = new bool[1];

                oRecords[0] = blnRet;

                response = new Models.Response(oRecords, 1);

            }
            catch (Exception ex)
            {
                //Error handler
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