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
    public class BookingImagesController : NGLControllerBase
    {
        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.5.4.003 on 10/24/2023 initializes the Page property by calling the base class constructor
        /// </summary>
        public BookingImagesController()
                : base(Utilities.PageEnum.BookingImages)
        {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.BookingImagesController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

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
            //Note: The id must always match a BookItemControl associated with the select Book Item Record 
            //the system looks up the last saved Book Control pk for this user 
            //an invalid parent key Error is returned if the data does not match
            //If id is zero and no records are found the client should configure the page to add a new record.
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {

                int count = 0;
                DAL.NGLBookImageData oDAL = new DAL.NGLBookImageData(Parameters);
                LTS.BookImage oData = oDAL.GetBookImage(id);
                LTS.BookImage[] records = new LTS.BookImage[1];
                if (oData != null)
                {
                    count = 1;
                    records[0] = oData;
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
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                int id = 0;
                LTS.tblUserPageSetting[] osettings =  readPageSettings("ParentControl");
                string sParentControl = null;
                if (osettings != null && osettings.Length > 0) { sParentControl = osettings[0].UserPSMetaData; }
                int.TryParse(sParentControl, out id);
                f.ParentControl = id;
                //if (id == 0)
                ////{
                ////    //get the parent control
                ////    id = readPagePrimaryKey(Parameters, Utilities.PageEnum.LoadBoard);
                ////}
                //save the page filter for the next time the page loads
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter); }

                DAL.NGLBookImageData oDAL = new DAL.NGLBookImageData(Parameters);

                LTS.vBookImageSummary[] records = new LTS.vBookImageSummary[] { };
                records = oDAL.GetBookImageSummaries(f, ref RecordCount);

                if (records != null && records.Count() > 0)
                {
                    count = records.Count();
                }else
                {
                    oDAL.throwNoDataFaultException();
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

        //not supporte
        //[HttpPost, ActionName("Post")]
        //public Models.Response Post([System.Web.Http.FromBody] Models.BookItem data)
        //{
        //    var response = new Models.Response(); //new HttpResponseMessage();
        //    if (!authenticateController(ref response)) { return response; }
        //    try
        //    {
        //     
        //    }
        //    catch (Exception ex)
        //    {
        //        FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
        //        response.StatusCode = fault.StatusCode;
        //        response.Errors = fault.formatMessage();
        //        return response;
        //    }
        //    return response;
        //}


        [HttpDelete, ActionName("DELETE")]
        public Models.Response DELETE(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLBookImageData oDAL = new DAL.NGLBookImageData(Parameters);
                bool blnRet = oDAL.DeleteBookImage(id);
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


        #endregion


        #region " public methods"


        #endregion
    }
}