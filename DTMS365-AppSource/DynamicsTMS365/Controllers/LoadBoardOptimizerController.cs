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
    public class LoadBoardOptimizerController : NGLControllerBase
    {
        #region " Constructors "

        /// <summary>Initializes the Page property by calling the base class constructor</summary>
        public LoadBoardOptimizerController() : base(Utilities.PageEnum.LoadBoard) { }

        #endregion

        #region " Properties"

        /// <summary>This property is used for logging and error tracking</summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.LoadBoardOptimizerController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        //private Models.Comp selectModelData(LTS.vLEComp365 d)
        //{
        //    Models.Comp modelRecord = new Models.Comp();
        //    if (d != null)
        //    {
        //        List<string> skipObjs = new List<string> { "CompUpdated" };
        //        string sMsg = "";
        //        modelRecord = (Models.Comp)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
        //        if (modelRecord != null) { modelRecord.setUpdated(d.CompUpdated.ToArray()); }
        //    }
        //    return modelRecord;
        //}

        //public static LTS.Comp selectLTSData(Models.Comp d)
        //{
        //    LTS.Comp ltsRecord = new LTS.Comp();
        //    if (d != null)
        //    {
        //        List<string> skipObjs = new List<string> { "CompUpdated", "CompCals", "_CompCals", "CompConts", "_CompConts", "CompEDIs", "_CompEDIs", "CompGoals", "_CompGoals", "CompTracks", "_CompTracks", "CompParameters", "_CompParameters", "CompAMSApptTrackingSettings", "_CompAMSApptTrackingSettings", "CompAMSColorCodeSettings", "_CompAMSColorCodeSettings", "CompAMSUserFieldSettings", "_CompAMSUserFieldSettings", "CompDockdoors", "_CompDockdoors", "tblSubscriptionRequestPendings", "_tblSubscriptionRequestPendings" };
        //        string sMsg = "";
        //        ltsRecord = (LTS.Comp)DTran.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
        //        if (ltsRecord != null)
        //        {
        //            byte[] bupdated = d.getUpdated();
        //            ltsRecord.CompUpdated = bupdated == null ? new byte[0] : bupdated;
        //        }
        //    }
        //    return ltsRecord;
        //}

        #endregion

        #region " REST Services"
        /// POST 	/API/objectcontroller{data}  : Create a new object or Update a the current object if the control number exists
        /// GET 	/API/objectcontroller/{id}   : Get the object information where the control number = "id"
        /// PUT 	/API/objectcontroller/{data} : Update the object information stored in data
        /// DELETE	/API/objectcontroller/{id}   : Delete object where the control number = "id"

        [HttpGet, ActionName("Get")]
        public Models.Response Get(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.Models.AllFilters f = new DAL.Models.AllFilters();
                LTS.tblUserPageSetting[] ups = readPageSettings("AllRecordsFilter", Parameters, Utilities.PageEnum.LoadBoard);
                if (ups?.Count() > 0) { f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(ups[0].UserPSMetaData); }
                f.page = 1;
                f.take = 1000;
                BLL.OptimizerBLL oBLL = new BLL.OptimizerBLL(Parameters);
                oBLL.RunOptimizer365(f);
                Array d = new bool[1] { true };
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

        [HttpPost, ActionName("Post")]
        public Models.Response Post([FromBody]Models.Comp data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.Models.AllFilters f = new DAL.Models.AllFilters();
                LTS.tblUserPageSetting[] ups = readPageSettings("AllRecordsFilter", Parameters, Utilities.PageEnum.LoadBoard);
                if (ups?.Count() > 0) { f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(ups[0].UserPSMetaData); }
                f.page = 1;
                f.take = 1000;

                DAL.NGLBookLoadBoard oDAL = new DAL.NGLBookLoadBoard(Parameters);
                int RecordCount = 0;
                LTS.vBookLoadBoard[] oData = oDAL.GetBookLoadBoards(f, ref RecordCount);
                if (oData?.Count() > 0)
                {
                
                }

                Array d = new bool[1] { true };
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



        [HttpGet, ActionName("RunOptimizer")]
        public Models.Response RunOptimizer(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.Models.AllFilters f = new DAL.Models.AllFilters();
                LTS.tblUserPageSetting[] ups = readPageSettings("AllRecordsFilter", Parameters, Utilities.PageEnum.LoadBoard);
                if (ups?.Count() > 0) { f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(ups[0].UserPSMetaData); }
                f.page = 1;
                f.take = 1000; //return all on one page, don't want paging here
                BLL.OptimizerBLL oBLL = new BLL.OptimizerBLL(Parameters);
                oBLL.RunOptimizer365(f);
                Array d = new bool[1] { true };
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

        [HttpGet, ActionName("GetOptimizerMessages")]
        public Models.Response GetOptimizerMessages(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLOptMsgData oOpt = new DAL.NGLOptMsgData(Parameters);
                DAL.Models.OptMsg[] records = new DAL.Models.OptMsg[] { };
                DAL.Models.OptMsg oData = oOpt.GetOptimizerMessages(Parameters.UserControl);
                if (oData != null) { records = new DAL.Models.OptMsg[1] { oData }; }
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

        /// <summary>
        /// Only gets the last record entered in the table
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, ActionName("GetLatestOptimizerMessage")]
        public Models.Response GetLatestOptimizerMessage(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLOptMsgData oOpt = new DAL.NGLOptMsgData(Parameters);
                DAL.Models.OptMsg[] records = new DAL.Models.OptMsg[] { };
                DAL.Models.OptMsg oData = oOpt.GetLatestOptimizerMessage(Parameters.UserControl);
                if (oData != null) { records = new DAL.Models.OptMsg[1] { oData }; }
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

        [HttpGet, ActionName("GetOptMsgs")]
        public Models.Response GetOptMsgs(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLOptMsgData oOpt = new DAL.NGLOptMsgData(Parameters);
                //if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter, "OptMsgFltr"); } //save the page filter for the next time the page loads
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                LTS.tmpOptMsg[] records = new LTS.tmpOptMsg[] { };
                int RecordCount = 0;
                int count = 0;
                if (isSortEmpty(f)) { applyDefaultSort(ref f, "OptMsgControl", false); }//addToSort(ref f, "OptMsgType", true); }
                records = oOpt.GetOptMsgs(ref RecordCount, f);
                if (records?.Count() > 0)
                {
                    count = records.Count();
                    //records = (from e in oData select selectModelData(e)).ToArray();
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

        [HttpDelete, ActionName("DeleteOptMsgByUser")]
        public Models.Response DeleteOptMsgByUser(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLOptMsgData oOpt = new DAL.NGLOptMsgData(Parameters);
                bool blnRet = oOpt.DeleteOptMsgByUser(Parameters.UserControl);
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


    }
}