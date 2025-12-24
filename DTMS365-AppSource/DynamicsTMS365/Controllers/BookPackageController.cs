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
    public class BookPackageController : NGLControllerBase
    {
        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.2 on 09/06/2018 initializes the Page property by calling the base class constructor
        /// </summary>
        public BookPackageController()
                : base(Utilities.PageEnum.BookPackage)
	     {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.BookPackageController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        private Models.BookPackage selectModelData(LTS.vBookPackage d)
        {
            Models.BookPackage modelRecord = new Models.BookPackage();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "BookPkgUpdated" };
                string sMsg = "";
                modelRecord = (Models.BookPackage)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.BookPkgUpdated.ToArray()); }
            }

            return modelRecord;
        }

        private Models.BookPackage selectModelData(LTS.BookPackage d)
        {
            Models.BookPackage modelRecord = new Models.BookPackage();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "BookPkgUpdated",  "Book" };
                string sMsg = "";
                modelRecord = (Models.BookPackage)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.BookPkgUpdated.ToArray()); }
            }

            return modelRecord;
        }

        public static LTS.BookPackage selectLTSData(Models.BookPackage d)
        {
            LTS.BookPackage ltsRecord = new LTS.BookPackage();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "BookPkgUpdated", "Book" };
                string sMsg = "";
                ltsRecord = (LTS.BookPackage)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.BookPkgUpdated = bupdated == null ? new byte[0] : bupdated;

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
            //Note: The id must always match a BookPkgControl associated with the select tariff 
            //the system looks up the last saved tariff pk for this user 
            //an invalid parent key Error is returned if the data does not match
            //If id is zero and no records are found the client should configure the page to add a new record.
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
                    f.filterName = "BookPkgControl";
                    f.filterValue = id.ToString();
                }
                DAL.NGLBookPackage oDAL = new DAL.NGLBookPackage(Parameters);
                Models.BookPackage[] records = new Models.BookPackage[] { };
                LTS.vBookPackage[] oData = new LTS.vBookPackage[] { };
                oData = oDAL.GetBookPackages(f, ref RecordCount);
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

        /// <summary>
        /// Gets All the Child BookPackage Records filtered by BookControl passed in as  id
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
                int RecordCount = 0;
                int count = 0;
                DAL.Models.AllFilters f = new DAL.Models.AllFilters { ParentControl = id };
                DAL.NGLBookPackage oDAL = new DAL.NGLBookPackage(Parameters);
                Models.BookPackage[] records = new Models.BookPackage[] { };
                LTS.vBookPackage[] oData = new LTS.vBookPackage[] { };
                oData = oDAL.GetBookPackages(f, ref RecordCount);
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
                int RecordCount = 0;
                int count = 0;
                //save the page filter for the next time the page loads
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter); }
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                f.ParentControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.LoadBoard);
                DAL.NGLBookPackage oDAL = new DAL.NGLBookPackage(Parameters);
                Models.BookPackage[] records = new Models.BookPackage[] { };
                LTS.vBookPackage[] oData = new LTS.vBookPackage[] { };
                oData = oDAL.GetBookPackages(f, ref RecordCount);
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
        public Models.Response Post([System.Web.Http.FromBody]Models.BookPackage data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLBookPackage oDAL = new DAL.NGLBookPackage(Parameters);
                LTS.BookPackage oChanges = new LTS.BookPackage();
                oChanges = selectLTSData(data);
                bool blnRet = oDAL.SaveBookPackage(oChanges);
                bool[] oRecords = new bool[1];
                oRecords[0] = blnRet;
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
                DAL.NGLBookPackage oDAL = new DAL.NGLBookPackage(Parameters);
                oDAL.DeleteBookPackage(id);
                bool[] oRecords = new bool[1];
                oRecords[0] = true;
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

        #region "public methods"

       
        #endregion
    }
}