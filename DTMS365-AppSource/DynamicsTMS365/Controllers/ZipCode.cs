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

namespace DynamicsTMS365.Controllers
{
    public class ZipCodeController : NGLControllerBase
    {

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.ZipCodeController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion


        #region " Data Translation"

        private Models.ZipCodes selectModelData(LTS.tblZipCode d)
        {
            Models.ZipCodes modelRecord = new Models.ZipCodes();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "ZipCodeUpdated" };
                string sMsg = "";
                modelRecord = (Models.ZipCodes)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                //if (modelRecord != null) { modelRecord.setUpdated(d.cmLocalUpdated.ToArray()); }
            }
            return modelRecord;
        }

        #endregion


        #region " REST Services"

        [HttpGet, ActionName("GetZips")]
        public Models.Response GetZips(int id)
        {
            // create a response message to send back
           var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            try
            {
                DAL.Models.ZipCode[] retZips = new DAL.Models.ZipCode[] { };
                int RecordCount = 0;
                int count = 0;

                // get the take and skip parameters int skip = request["skip"] == null ? 0 :
                int skip = request["skip"] == null ? 0 : int.Parse(request["skip"]);
                int take = request["take"] == null ? 500 : int.Parse(request["take"]);
                string sortExpression = "";

                string ofilter = request["filter[filters][0][value]"];
                //string oOp = request["filter[filters][0][operator]"];

                string filterWhere = ofilter;
                if (String.IsNullOrWhiteSpace(filterWhere))
                {
                    response = new Models.Response(retZips, count);
                    return response;
                }           

                DAL.NGLLookupDataProvider dalZipData = new DAL.NGLLookupDataProvider(Parameters);

                if (id == 1)
                {
                    sortExpression = "ZipCode Asc";
                    retZips = dalZipData.GetZips(ref RecordCount, filterWhere, sortExpression, 1, 0, skip, take);
                    if (retZips != null && retZips.Count() > 0)
                    {
                        count = retZips.Length;
                    }
                }
                if (id == 2)
                {
                    sortExpression = "City Asc";
                    retZips = dalZipData.GetZipsByCity(ref RecordCount, filterWhere, sortExpression, 1, 0, skip, take);
                    if (retZips != null && retZips.Count() > 0)
                    {
                        count = retZips.Length;
                    }
                }
                if (id == 3)
                {
                    sortExpression = "State Asc";
                    retZips = dalZipData.GetZipsByState(ref RecordCount, filterWhere, sortExpression, 1, 0, skip, take);
                    if (retZips != null && retZips.Count() > 0)
                    {
                        count = retZips.Length;
                    }
                }
                         
                response = new Models.Response(retZips, count);
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

        [HttpGet, ActionName("GetZipsTest")]
        public Models.Response GetZipsTest(int id)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            //if (!authenticateController(ref response)) { return response; }

            try
            {
                DAL.Models.ZipCode[] retZips = new DAL.Models.ZipCode[] { };
                int RecordCount = 0;
                int count = 0;

                // get the take and skip parameters int skip = request["skip"] == null ? 0 :
                int skip = request["skip"] == null ? 0 : int.Parse(request["skip"]);
                int take = request["take"] == null ? 500 : int.Parse(request["take"]);
                string sortExpression = "";

                string ofilter = request["filter[filters][0][value]"];
                //string oOp = request["filter[filters][0][operator]"];

                string filterWhere = ofilter;
                if (String.IsNullOrWhiteSpace(filterWhere))
                {
                    response = new Models.Response(retZips, count);
                    return response;
                }

                DAL.WCFParameters oWCFParameters = new DAL.WCFParameters
                {
                    Database = System.Configuration.ConfigurationManager.AppSettings["Database"],
                    DBServer = System.Configuration.ConfigurationManager.AppSettings["DBServer"],
                    ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["NGLMASPROD"].ConnectionString,
                    WCFAuthCode = "NGLSystem",
                    UserName = "",
                    ValidateAccess = false
                };

                DAL.NGLLookupDataProvider dalZipData = new DAL.NGLLookupDataProvider(oWCFParameters);

                if (id == 1)
                {
                    sortExpression = "ZipCode Asc";
                    retZips = dalZipData.GetZips(ref RecordCount, filterWhere, sortExpression, 1, 0, skip, take);
                    if (retZips != null && retZips.Count() > 0)
                    {
                        count = retZips.Length;
                    }
                }
                if (id == 2)
                {
                    sortExpression = "City Asc";
                    retZips = dalZipData.GetZipsByCity(ref RecordCount, filterWhere, sortExpression, 1, 0, skip, take);
                    if (retZips != null && retZips.Count() > 0)
                    {
                        count = retZips.Length;
                    }
                }
                if (id == 3)
                {
                    sortExpression = "State Asc";
                    retZips = dalZipData.GetZipsByState(ref RecordCount, filterWhere, sortExpression, 1, 0, skip, take);
                    if (retZips != null && retZips.Count() > 0)
                    {
                        count = retZips.Length;
                    }
                }

                response = new Models.Response(retZips, count);
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


        //[HttpGet, ActionName("GetAllZips")]
        //public Models.Response GetAllZips(int id)
        //{
        //    create a response message to send back
        //   var response = new Models.Response(); //new HttpResponseMessage();
        //    if (!authenticateController(ref response)) { return response; }

        //    try
        //    {
        //        DAL.Models.ZipCode[] retZips = new DAL.Models.ZipCode[] { };
        //        int RecordCount = 0;
        //        int count = 0;

        //        get the take and skip parameters int skip = request["skip"] == null ? 0 :
        //       int skip = request["skip"] == null ? 0 : int.Parse(request["skip"]);
        //        int take = request["take"] == null ? 500 : int.Parse(request["take"]);
        //        string sortExpression = "";

        //        string ofilter = request["filter[filters][0][value]"];
        //        //string oOp = request["filter[filters][0][operator]"];

        //        string filterWhere = ofilter;
        //        if (String.IsNullOrWhiteSpace(filterWhere))
        //        {
        //            response = new Models.Response(retZips, count);
        //            return response;
        //        }

        //        DAL.NGLLookupDataProvider dalZipData = new DAL.NGLLookupDataProvider(Parameters);

        //        if (id == 1)
        //        {
        //            sortExpression = "ZipCode Asc";
        //            retZips = dalZipData.GetZips(ref RecordCount, filterWhere, sortExpression, 1, 0, skip, take);
        //            if (retZips != null && retZips.Count() > 0)
        //            {
        //                count = retZips.Length;
        //            }
        //        }
        //        if (id == 2)
        //        {
        //            sortExpression = "City Asc";
        //            retZips = dalZipData.GetZipsByCity(ref RecordCount, filterWhere, sortExpression, 1, 0, skip, take);
        //            if (retZips != null && retZips.Count() > 0)
        //            {
        //                count = retZips.Length;
        //            }
        //        }
        //        if (id == 3)
        //        {
        //            sortExpression = "State Asc";
        //            retZips = dalZipData.GetZipsByState(ref RecordCount, filterWhere, sortExpression, 1, 0, skip, take);
        //            if (retZips != null && retZips.Count() > 0)
        //            {
        //                count = retZips.Length;
        //            }
        //        }

        //        response = new Models.Response(retZips, count);
        //    }
        //    catch (Exception ex)
        //    {
        //        Error handler
        //        FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
        //        response.StatusCode = fault.StatusCode;
        //        response.Errors = fault.formatMessage();
        //        return response;
        //    }

        //    return the HTTP Response.
        //    return response;
        //}

        [HttpGet, ActionName("GetZipsForCityState")]
        public Models.Response GetZipsForCityState(string filter)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            try
            {
                DAL.Models.ZipCode z = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.ZipCode>(filter);
                DAL.Models.ZipCode[] retZips = new DAL.Models.ZipCode[] { };
                int count = 0;

                DAL.NGLLookupDataProvider dalZipData = new DAL.NGLLookupDataProvider(Parameters);

                retZips = dalZipData.GetZipsForCityState(z.City, z.State);
                if (retZips != null && retZips.Count() > 0)
                {
                    count = retZips.Length;
                }

                response = new Models.Response(retZips, count);
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


        #endregion
    }
}