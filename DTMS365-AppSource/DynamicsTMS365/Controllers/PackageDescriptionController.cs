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
    public class PackageDescriptionController : NGLControllerBase
    {

        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.2 on 08/29/2018 initializes the Page property by calling the base class constructor
        /// </summary>
        public PackageDescriptionController()
                : base(Utilities.PageEnum.PackageDescriptions)
	     {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.PackageDescriptionController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        private Models.PackageDescription selectModelData(LTS.tblPackageDescription d)
        {
            Models.PackageDescription modelRecord = new Models.PackageDescription();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "PkgDescUpdated" };
                string sMsg = "";
                modelRecord = (Models.PackageDescription)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.PkgDescUpdated.ToArray()); }
            }

            return modelRecord;
        }


        private LTS.tblPackageDescription selectLTSData(Models.PackageDescription d)
        {
            LTS.tblPackageDescription ltsRecord = new LTS.tblPackageDescription();
            if (d != null)
            {
                //List<string> skipObjs = new List<string> { "PkgDescUpdated", "CarrierPackageDescriptionBreakPoints", "CarrierPackageDescriptionMatrixes", "CarrierPackageDescriptionDiscounts", "CarrierPackageDescriptionFees", "CarrierPackageDescriptionInterlines", "CarrierPackageDescriptionMinCharges", "CarrierPackageDescriptionNonServices", "CarrierPackageDescriptionEquipments", "CarrierPackageDescriptionMatrixBPs", "CarrierPackageDescriptionClassXrefs", "CarrierPackageDescriptionNoDriveDays", "CarrierPackageDescriptionMinWeights", "CompRefCarrier", "Carrier" };
                List<string> skipObjs = new List<string> { "PkgDescUpdated" };
                string sMsg = "";
                ltsRecord = (LTS.tblPackageDescription)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.PkgDescUpdated = bupdated == null ? new byte[0] : bupdated;

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
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            try
            {
                int RecordCount = 0;
                int count = 0;
                DAL.Models.AllFilters f = new DAL.Models.AllFilters();
                f.filterName = "PkgDescControl";
                f.filterValue = id.ToString();
                DAL.NGLtblPackageDescriptionData oDAL = new DAL.NGLtblPackageDescriptionData(Parameters);
                Models.PackageDescription[] records = new Models.PackageDescription[] { };
                LTS.tblPackageDescription[] oData = new LTS.tblPackageDescription[] { };
                oData = oDAL.GettblPackageDescriptions(f, ref RecordCount);
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
                DAL.NGLtblPackageDescriptionData oDAL = new DAL.NGLtblPackageDescriptionData(Parameters);
                Models.PackageDescription[] records = new Models.PackageDescription[] { };
                LTS.tblPackageDescription[] oData = new LTS.tblPackageDescription[] { };
                oData = oDAL.GettblPackageDescriptions(f, ref RecordCount);
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
        public Models.Response Post([System.Web.Http.FromBody]Models.PackageDescription data)
        {
            //create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLtblPackageDescriptionData oDAL = new DAL.NGLtblPackageDescriptionData(Parameters);
                LTS.tblPackageDescription oChanges = selectLTSData(data);
                bool blnRet = oDAL.InsertOrUpdatetblPackageDescription(oChanges);


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
                DAL.NGLtblPackageDescriptionData oDAL = new DAL.NGLtblPackageDescriptionData(Parameters);
                bool blnRet = oDAL.DeletetblPackageDescription(id);
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
    }
}