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
using DModel = Ngl.FreightMaster.Data.Models;

namespace DynamicsTMS365.Controllers
{
    public class CompToleranceController : NGLControllerBase
    {
        #region " Constructors "

        public CompToleranceController() : base(Utilities.PageEnum.CompanyMaint) { }

        #endregion

        #region " Properties"

        /// <summary>This property is used for logging and error tracking.</summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.CompToleranceController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        public static Models.Comp selectModelData(LTS.vLEComp365 d)
        {
            Models.Comp nsComp = new Models.Comp();
            List<string> skipObjs = new List<string> { "CompUpdated" };
            string sMsg = "";
            nsComp = (Models.Comp)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(nsComp, d, skipObjs, ref sMsg);
            return nsComp;
        }

        public static LTS.Comp selectLTSData(Models.Comp d)
        {
            LTS.Comp ltsRecord = new LTS.Comp();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "CompUpdated", "CompCals", "_CompCals", "CompConts", "_CompConts", "CompEDIs", "_CompEDIs", "CompGoals", "_CompGoals", "CompTracks", "_CompTracks", "CompParameters", "_CompParameters", "CompAMSApptTrackingSettings", "_CompAMSApptTrackingSettings", "CompAMSColorCodeSettings", "_CompAMSColorCodeSettings", "CompAMSUserFieldSettings", "_CompAMSUserFieldSettings", "CompDockdoors", "_CompDockdoors", "tblSubscriptionRequestPendings", "_tblSubscriptionRequestPendings" };
                string sMsg = "";
                ltsRecord = (LTS.Comp)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.CompUpdated = bupdated == null ? new byte[0] : bupdated;
                }
            }
            return ltsRecord;
        }

        #endregion

        #region " REST Services"

        /// <summary>
        /// Method to Get display the popup window values for Tolerance
        /// </summary>
        /// <param name="id">0</param>
        /// <returns></returns>
        [HttpGet, ActionName("Get")]
        public Models.Response Get(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DModel.AllFilters f = new DModel.AllFilters();
                f.ParentControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.CompanyMaint);
                Models.Comp[] retComps = new Models.Comp[] { };
                LTS.vLEComp365[] vcomps = new LTS.vLEComp365[1];
                int RecordCount = 0;
                int count = 0;
                vcomps = NGLCompData.ReadCompTolerance(ref RecordCount, f).ToArray();
                if (vcomps != null && vcomps.Count() > 0)
                {
                    //RecordCount contains the number of records in the database that matches the filters
                    count = RecordCount;
                    retComps = (from e in vcomps
                                orderby e.CompName ascending
                                select selectModelData(e)).ToArray();
                }
                response = new Models.Response(retComps, count);
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

        //Not currently used
        [HttpGet, ActionName("GetRecords")]
        public Models.Response GetRecords()
        {
            return GetRecords("");
        }

        //Not currently used
        [HttpGet, ActionName("GetRecords")]
        public Models.Response GetRecords(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                //DModel.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DModel.AllFilters>(filter);
                //Models.Comp[] retComps = new Models.Comp[] { };
                //LTS.vLEComp365[] vcomps = new LTS.vLEComp365[] { };
                //int RecordCount = 0;
                //int count = 0;
                //vcomps = NGLCompData.GetLECompsFiltered(ref RecordCount, f);
                //if (vcomps != null && vcomps.Count() > 0)
                //{
                //    //RecordCount contains the number of records in the database that matches the filters
                //    count = RecordCount;
                //    retComps = (from e in vcomps
                //                orderby e.CompName ascending
                //                select selectModelData(e)).ToArray();
                //}
                //response = new Models.Response(retComps, count);
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
        /// Method for saving tolerance values into database for the company
        /// </summary>
        /// <param name="data">4text box values</param>
        /// <remarks>
        /// Added for Company data Migrations---ManoRama
        /// Modified By LVV on 10/28/20 for v-8.3.0.002 - Task #20201028134331 - Add Fields to Company Maint
        ///  Moved this method from CompController to new CompToleranceController (refactor)
        /// </remarks>
        /// <returns></returns>
        [HttpPost, ActionName("Post")]
        public Models.Response Post([System.Web.Http.FromBody]Models.Comp data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DModel.AllFilters f = new DModel.AllFilters();
                f.ParentControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.CompanyMaint);
                Models.Comp[] retComps = new Models.Comp[1];
                LTS.vLEComp365[] vcomps = new LTS.vLEComp365[] { };
                LTS.Comp compObj = new LTS.Comp();
                int RecordCount = 0;
                int count = 0;
                vcomps = NGLCompData.ReadCompTolerance(ref RecordCount, f);
                if (vcomps != null && vcomps.Count() > 0)
                {
                    //RecordCount contains the number of records in the database that matches the filters
                    count = RecordCount;
                    retComps = (from e in vcomps
                                orderby e.CompName ascending
                                select selectModelData(e)).ToArray();

                    compObj = (from e in retComps
                               orderby e.CompName ascending
                               select selectLTSData(e)).FirstOrDefault();
                }
                compObj.CompPayTolPerLo = data.CompPayTolPerLo;
                compObj.CompPayTolPerHi = data.CompPayTolPerHi;
                compObj.CompPayTolCurLo = data.CompPayTolCurLo;
                compObj.CompPayTolCurHi = data.CompPayTolCurHi;
                NGLCompData.SaveCompTolerances(compObj);
                response = new Models.Response(retComps, count);
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

        //I don't this this is used anywhere
        /*
        [HttpPost, ActionName("SavePayableTolerances")]
        public Models.Response SavePayableTolerances(Models.Comp toleranceData)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.Models.AllFilters f = new DAL.Models.AllFilters();
                f.ParentControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.CompanyMaint);
                f.LEAdminControl = 2;
                Models.Comp[] retComps = new Models.Comp[1];
                LTS.vLEComp365[] vcomps = new LTS.vLEComp365[] { };
                LTS.Comp compObj = new LTS.Comp();
                int RecordCount = 0;
                int count = 0;
                vcomps = NGLCompData.GetLECompsFiltered(ref RecordCount, f).Where(x => x.CompControl == f.ParentControl).ToArray();
                if (vcomps != null && vcomps.Count() > 0)
                {
                    //RecordCount contains the number of records in the database that matches the filters
                    count = RecordCount;
                    retComps = (from e in vcomps
                                orderby e.CompName ascending
                                select selectModelData(e)).ToArray();

                    compObj = (from e in retComps
                               orderby e.CompName ascending
                               select selectLTSData(e)).FirstOrDefault();
                }
                compObj.CompPayTolPerLo = toleranceData.CompPayTolPerLo;
                compObj.CompPayTolPerHi = toleranceData.CompPayTolPerHi;
                compObj.CompPayTolCurLo = toleranceData.CompPayTolCurLo;
                compObj.CompPayTolCurHi = toleranceData.CompPayTolCurHi;
                NGLCompData.SaveCompTolerances(compObj);
                response = new Models.Response(retComps, count);
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
        */

        #endregion
    }
}