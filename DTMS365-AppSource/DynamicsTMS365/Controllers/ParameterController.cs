using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using DAL = Ngl.FreightMaster.Data;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using LTS = Ngl.FreightMaster.Data.LTS;

namespace DynamicsTMS365.Controllers
{
    public class ParameterController : NGLControllerBase
    {

    #region " Properties"
    /// <summary>
    /// This property is used for logging and error tracking.
    /// </summary>
    private string _SourceClass = "DynamicsTMS365.Controllers.ParameterController";
    public override string SourceClass
    {
        get { return _SourceClass; }
        set { _SourceClass = value; }
    }

    HttpRequest request = HttpContext.Current.Request;

    #endregion


    #region " Data Translation"


    private Models.Parameter selectModelData(LTS.Parameter d)
    {
        Models.Parameter modelRecord = new Models.Parameter();
        if (d != null)
        {
            List<string> skipObjs = new List<string> { "ParUpdated", "msrepl_tran_version", "rowguid","tblParCategory","ParOLE","ParValue" };
            string sMsg = "";
            modelRecord = (Models.Parameter)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
            if (modelRecord != null) {
                    modelRecord.setUpdated(d.ParUpdated.ToArray());
                    if (d.ParValue.HasValue)
                    {
                        modelRecord.ParValue = d.ParValue.Value;
                    } else
                    {
                        modelRecord.ParValue = 0;
                    }
                }
        }

        return modelRecord;
    }

      

        private LTS.Parameter selectLTSData(Models.Parameter d)
    {
        LTS.Parameter ltsRecord = new LTS.Parameter();
        if (d != null)
        {
            List<string> skipObjs = new List<string> { "ParUpdated", "msrepl_tran_version", "rowguid", "tblParCategory", "ParOLE" };
            string sMsg = "";
            ltsRecord = (LTS.Parameter)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
            if (ltsRecord != null)
            {
                byte[] bupdated = d.getUpdated();
                ltsRecord.ParUpdated = bupdated == null ? new byte[0] : bupdated;

            }
        }

        return ltsRecord;
    }




        #endregion


        #region " REST Services"  

        //[HttpGet, ActionName("Get")]
        //public Models.Response Get()
        //{
        //    return GetRecords("");
        //    //// create a response message to send back
        //    //var response = new Models.Response(); //new HttpResponseMessage();
        //    //if (!authenticateController(ref response)) { return response; }
        //    //try
        //    //{
        //    //    Models.Parameter[] records = new Models.Parameter[] { };
        //    //    int count = 0;
        //    //    int RecordCount = 0;
        //    //    int skip = request["skip"] == null ? 0 : int.Parse(request["skip"]);
        //    //    int take = request["take"] == null ? 50 : int.Parse(request["take"]);
        //    //    DAL.NGLParameterData dalData = new DAL.NGLParameterData(Parameters);
        //    //    List<LTS.Parameter> oData = new List<LTS.Parameter>();
                
        //    //    LTS.Parameter[] oPars = dalData.GetParameters(ref RecordCount,skip,take);
        //    //    if (oPars != null) { oData = oPars.ToList(); }
                
                
        //    //    if (oData != null && oData.Count() > 0)
        //    //    {                    
        //    //        count = oData.Count();
        //    //        records = (from e in oData
        //    //                   select selectModelData(e)).ToArray();
        //    //    }

        //    //    response = new Models.Response(records, count);
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
        //    //    response.StatusCode = fault.StatusCode;
        //    //    response.Errors = fault.formatMessage();
        //    //    return response;
        //    //}

        //    //// return the HTTP Response.
        //    //return response;
        //}

        [HttpGet, ActionName("GetRecords")]
        public Models.Response GetRecords(string filter)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                Models.Parameter[] records = new Models.Parameter[] { };
                int count = 0;
                int RecordCount = 0;
                int skip = request["skip"] == null ? 0 : int.Parse(request["skip"]);
                int take = request["take"] == null ? 50 : int.Parse(request["take"]);
                DAL.NGLParameterData dalData = new DAL.NGLParameterData(Parameters);
                List<LTS.Parameter> oData = new List<LTS.Parameter>();
                if (string.IsNullOrWhiteSpace(filter))
                {
                    LTS.Parameter[] oPars = dalData.GetParameters(ref RecordCount, skip, take);
                    if (oPars != null) { oData = oPars.ToList(); }
                }
                else
                {
                    DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                    LTS.Parameter[] oPars = dalData.GetParameters(f, ref RecordCount);
                    //LTS.Parameter oPar = dalData.GetParameterfilter);
                    //if (oPar != null) { oData.Add(oPar); }
                    if (oPars != null) { oData = oPars.ToList(); }
                }

                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                    records = (from e in oData
                               select selectModelData(e)).ToArray();
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

            // return the HTTP Response.
            return response;
        }

        //[HttpGet, ActionName("Get")]
        //public Models.Response Get(string filter)
        //{
        //    // create a response message to send back
        //    var response = new Models.Response(); //new HttpResponseMessage();
        //    if (!authenticateController(ref response)) { return response; }
        //    try
        //    {
        //        Models.Parameter[] records = new Models.Parameter[] { };
        //        int count = 0;
        //        int RecordCount = 0;
        //        int skip = request["skip"] == null ? 0 : int.Parse(request["skip"]);
        //        int take = request["take"] == null ? 50 : int.Parse(request["take"]);
        //        DAL.NGLParameterData dalData = new DAL.NGLParameterData(Parameters);
        //        List<LTS.Parameter> oData = new List<LTS.Parameter>();
        //        if (string.IsNullOrWhiteSpace(filter))
        //        {
        //            LTS.Parameter[] oPars = dalData.GetParameters(ref RecordCount, skip, take);
        //            if (oPars != null) { oData = oPars.ToList(); }
        //        }
        //        else
        //        {
        //            LTS.Parameter oPar = dalData.GetParameter(filter);
        //            if (oPar != null) { oData.Add(oPar); }
        //        }

        //        if (oData != null && oData.Count() > 0)
        //        {
        //            count = oData.Count();
        //            records = (from e in oData
        //                       select selectModelData(e)).ToArray();
        //            if (RecordCount > count) { count = RecordCount; }
        //        }

        //        response = new Models.Response(records, count);
        //    }
        //    catch (Exception ex)
        //    {
        //        FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
        //        response.StatusCode = fault.StatusCode;
        //        response.Errors = fault.formatMessage();
        //        return response;
        //    }

        //    // return the HTTP Response.
        //    return response;
        //}

        [HttpPost, ActionName("PostSave")]
        public Models.Response PostSave([System.Web.Http.FromBody]Models.Parameter data)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                LTS.Parameter oChanges = selectLTSData(data);

                DAL.NGLParameterData dalData = new DAL.NGLParameterData(Parameters);
                LTS.Parameter oData = dalData.SaveParameter(oChanges);

                Models.Parameter[] oRecords = new Models.Parameter[1];

                oRecords[0] = selectModelData(oData);

                response = new Models.Response(oRecords, 1);

            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }

            // return the HTTP Response.
            return response;
        }


        [HttpGet, ActionName("GetGlobalParText")]
        public Models.Response GetGlobalParText(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLParameterData dalData = new DAL.NGLParameterData(Parameters);
                string strParKey = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<string>(filter);
                string parText = "";
                if (!string.IsNullOrWhiteSpace(strParKey))
                {
                    LTS.Parameter oPars = dalData.GetParameter(strParKey);
                    if (oPars != null) { parText = oPars.ParText; }
                }
                string[] records = new string[1] { parText };
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

        [HttpGet, ActionName("GetGlobalParValue")]
        public Models.Response GetGlobalParValue(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLParameterData dalData = new DAL.NGLParameterData(Parameters);
                string strParKey = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<string>(filter);
                double parVal = 0;
                if (!string.IsNullOrWhiteSpace(strParKey))
                {
                    LTS.Parameter oPars = dalData.GetParameter(strParKey);
                    if (oPars != null) { if (oPars.ParValue.HasValue) { parVal = oPars.ParValue.Value; } }
                }
                double[] records = new double[1] { parVal };
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

        #endregion
    }
}