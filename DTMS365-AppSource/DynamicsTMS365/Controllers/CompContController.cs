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
    public class CompContController : NGLControllerBase
    {
        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.2 on 10/09/2018 
        /// initializes the Page property by calling the base class constructor
        /// Not sure if Utilities.PageEnum.CompanyMaint for the child data 
        /// Comp Contact will cause any trouble.  Some testing may be required
        /// </summary>
        public CompContController()
                : base(Utilities.PageEnum.CompanyMaint)
	     {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.CompContController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        private Models.CompContact selectModelData(LTS.CompCont d)
        {
            Models.CompContact modelRecord = new Models.CompContact();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "CompContUpdated", "rowguid", "_Comp", "Comp" };
                string sMsg = "";
                modelRecord = (Models.CompContact)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.CompContUpdated.ToArray()); }
            }

            return modelRecord;
        }

        public static LTS.CompCont selectLTSData(Models.CompContact d)
        {
            LTS.CompCont ltsRecord = new LTS.CompCont();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "CompContUpdated", "rowguid", "_Comp", "Comp" };
                string sMsg = "";
                ltsRecord = (LTS.CompCont)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.CompContUpdated = bupdated == null ? new byte[0] : bupdated;

                }
            }

            return ltsRecord;
        }

        #endregion


        #region " REST Services"

        /// <summary>
        ///  Gets one CompContact record using the CompContControl passed in as id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.2 on 10/09/2018
        ///   previous code to get the first contact record filterd by the CompContCompControl
        ///   this method was modified to use the CompContControl for id 
        ///   to return theselected record
        /// </remarks>
        [HttpGet, ActionName("Get")]
        public Models.Response Get(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int RecordCount = 0;
                int count = 0;
                DAL.Models.AllFilters f = new DAL.Models.AllFilters { filterName = "CompContControl", filterValue = id.ToString() };      
                DAL.NGLCompContData oDAL = new DAL.NGLCompContData(Parameters);
                Models.CompContact[] records = new Models.CompContact[] { };
                LTS.CompCont[] oData = new LTS.CompCont[] { };
                oData = oDAL.GetCompContsFiltered365(ref RecordCount, f);
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

        //
        /// <summary>
        /// Gets All the Child CompContact Records filtered by CompContCompControl passed in as  id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.2 on 10/09/2018
        ///   new name for Get method renamed to support Edit Widget
        /// </remarks>
        [HttpGet, ActionName("GetByParent")]
        public Models.Response GetByParent(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {                       
                DAL.Models.AllFilters f = new DAL.Models.AllFilters { filterName = "CompContCompControl", filterValue = id.ToString() };         
                DAL.NGLCompContData oCont = new DAL.NGLCompContData(Parameters);
                Models.CompContact[] retCompConts = new Models.CompContact[] { };
                LTS.CompCont[] conts = new LTS.CompCont[] { };
                int RecordCount = 0;
                int count = 0;
                conts = oCont.GetCompContsFiltered365(ref RecordCount, f);
                if (conts != null && conts.Count() > 0)
                {
                    //RecordCount contains the number of records in the database that matches the filters
                    count = RecordCount;
                    retCompConts = (from e in conts  select selectModelData(e)).ToArray();
                }
                response = new Models.Response(retCompConts, count);
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
            if (!authenticateController(ref response)) { return response; }
            if (!authenticateFilter(ref response, filter)) { return response; } //Verfiy that the filters object is not null
            try
            {
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);

                DAL.NGLCompContData oCont = new DAL.NGLCompContData(Parameters);
                LTS.CompCont[] conts = new LTS.CompCont[] { };
                int RecordCount = 0;

                conts = oCont.GetCompContsFiltered365(ref RecordCount, f);

                if (conts?.Length < 1)
                {
                    conts = new LTS.CompCont[] { };
                    RecordCount = 0;
                }

                response = new Models.Response(conts, RecordCount);
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


        ////[HttpGet, ActionName("GetCompContsFiltered")]
        ////public Models.Response GetCompContsFiltered(string filter)
        ////{
        ////    var response = new Models.Response(); //new HttpResponseMessage();
        ////    if (!authenticateController(ref response)) { return response; }
        ////    try
        ////    {
        ////        DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
        ////        DAL.NGLCompContData oCont = new DAL.NGLCompContData(Parameters);
        ////        LTS.CompCont[] conts = new LTS.CompCont[] { };
        ////        int RecordCount = 0;
        ////        conts = oCont.GetCompContsFiltered365(ref RecordCount, f);
        ////        if (conts?.Length < 1)
        ////        {
        ////            conts = new LTS.CompCont[] { };
        ////            RecordCount = 0;
        ////        }
        ////        response = new Models.Response(conts, RecordCount);
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
        ////        response.StatusCode = fault.StatusCode;
        ////        response.Errors = fault.formatMessage();
        ////        return response;
        ////    }
        ////    return response;
        ////}

        [HttpPost, ActionName("Post")]
        public Models.Response Post([System.Web.Http.FromBody]Models.CompContact data)
        {
            //create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }

            return PostCompCont(data);            
        }


        ////[HttpPost, ActionName("InsertOrUpdateCompCont")]
        ////public Models.Response InsertOrUpdateCompCont([System.Web.Http.FromBody]Models.CompContact cont)
        ////{
        ////    // create a response message to send back
        ////    var response = new Models.Response(); //new HttpResponseMessage();
        ////    if (!authenticateController(ref response)) { return response; }
        ////    return PostCompCont(cont);
        ////}

       

        [HttpDelete, ActionName("DELETE")]
        public Models.Response DELETE(int id)
        {
            //create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            return DeleteContact(id);           
        }

        ////[HttpDelete, ActionName("DeleteCompContact")]
        ////public Models.Response DeleteCompContact(int id)
        ////{
        ////    // create a response message to send back
        ////    var response = new Models.Response(); //new HttpResponseMessage();
        ////    if (!authenticateController(ref response)) { return response; }
        ////    return DeleteContact(id);

        ////}



        #endregion


        #region "Private Methods"

        private Models.Response PostCompCont(Models.CompContact data)
        {
            var response = new Models.Response();
            try
            {
                DAL.NGLCompContData oCompCont = new DAL.NGLCompContData(Parameters);
                LTS.CompCont ltsCont = selectLTSData(data);                
                ltsCont.CompContPhone = Utilities.removeNonNumericText(ltsCont.CompContPhone);
                ltsCont.CompContFax = Utilities.removeNonNumericText(ltsCont.CompContFax);
                ltsCont.CompCont800 = Utilities.removeNonNumericText(ltsCont.CompCont800);
                oCompCont.InsertOrUpdateCompCont365(ltsCont);
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

        private Models.Response DeleteContact(int id)
        {
            var response = new Models.Response();
            try
            {
                bool blnRet = true;
                if (id == 0)
                {
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    DAL.NGLCompContData oDAL = new DAL.NGLCompContData(Parameters);
                    blnRet = oDAL.DeleteCompContact(id);
                }
                bool[] oRecords = new bool[1];
                oRecords[0] = blnRet;
                if (blnRet)
                {
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Errors = string.Format("Cannot delete the Company Contact record with Control {0}", id.ToString());

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

        #endregion
    }
}