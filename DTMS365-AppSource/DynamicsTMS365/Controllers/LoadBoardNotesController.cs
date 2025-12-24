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
    public class LoadBoardNotesController : NGLControllerBase
    {

        #region " Constructors "

        /// <summary>Initializes the Page property by calling the base class constructor</summary>
        public LoadBoardNotesController() : base(Utilities.PageEnum.LoadBoardNotes) { }

        #endregion

        #region " Properties"
        
        /// <summary>This property is used for logging and error tracking.</summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.LoadBoardNotesController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion


        #region " Data Translation"

        private Models.BookNote selectModelData(LTS.vBookNote d)
        {
            Models.BookNote modelRecord = new Models.BookNote();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "BookNotesUpdated", "rowguid", "BookNotes", "Book", "_BookNotes", "_Book" };
                string sMsg = "";
                modelRecord = (Models.BookNote)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.BookNotesUpdated.ToArray()); }
            }
            return modelRecord;
        }

        public static LTS.BookNote selectLTSData(Models.BookNote d)
        {
            LTS.BookNote ltsRecord = new LTS.BookNote();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "BookNotesUpdated", "rowguid", "BookNotes", "Book", "_BookNotes", "_Book" };
                string sMsg = "";
                ltsRecord = (LTS.BookNote)DTran.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.BookNotesUpdated = bupdated == null ? new byte[0] : bupdated;
                }
            }
            return ltsRecord;
        }

        #endregion

        #region " REST Services"
        /// POST 	/API/objectcontroller{data}  : Create a new object or Update a the current object if the control number exists
        /// GET 	/API/objectcontroller/{id} : Get the object information where the control number = "id"
        /// PUT 	/API/objectcontroller/{data} : Update the object information stored in data
        /// DELETE	/API/objectcontroller/{id} : Delete object where the control number = "id"

        [HttpGet, ActionName("Get")]
        public Models.Response Get(int id)
        {
            //Note: The id must always match a BookControl associated with the select Load
            //The system looks up the last saved Book pk for this user and return the first Notes found
            //An invalid parent key Error is returned if the data does not match
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {

                //Modified by RHR for v-8.5.4.004 on 12/06/2023 new BookControl setting 
                //if (id == 0) { id = readPagePrimaryKey(Parameters, Utilities.PageEnum.LoadBoard); } //get the parent control
                Models.BookNote[] records = new Models.BookNote[1];
                id = readBookControlPageSetting(id);
                if (id == 0)
                {
                    return new Models.Response(records, 0);
                }

                LTS.vBookNote[] oData = NGLBookNoteData.GetvBookNotes(id);
                if (oData != null) { records[0] = selectModelData(oData[0]); }
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

        //Not Currently Used
        [HttpGet, ActionName("GetAllRecords")]
        public Models.Response GetAllRecords(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {                
                //if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter); } //save the page filter for the next time the page loads
                //DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                //int id = f.ParentControl;
                //if (id == 0) { id = readPagePrimaryKey(Parameters, Utilities.PageEnum.LoadBoard); } //get the parent control
                //DAL.NGLBookRevenueData oDAL = new DAL.NGLBookRevenueData(Parameters);
                //LTS.vLoadBoardRev oData = new LTS.vLoadBoardRev();
                //LTS.vLoadBoardRev[] records = new LTS.vLoadBoardRev[1];
                //oData = oDAL.GetvLoadBoardRev(id);
                //if (oData != null) { records[0] = oData; }
                //response = new Models.Response(records, 1);
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
        public Models.Response Post([FromBody]Models.BookNote data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                LTS.BookNote lts = selectLTSData(data);
                bool blnRet = NGLBookNoteData.InsertOrUpdateBookNote(lts);
                bool[] results = new bool[1] { blnRet };
                response = new Models.Response(results, 1);
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