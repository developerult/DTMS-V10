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
using DModel = Ngl.FreightMaster.Data.Models;
using NGL.FM.BLL;
using Ngl.FreightMaster.Data.DataTransferObjects;
using System.Runtime.Remoting;
using System.Web.Http.Results;


namespace DynamicsTMS365.Controllers
{
    public class LoadBoardLaneTransLoadsController : NGLControllerBase
    {
        #region " Constructors "

        /// <summary>Initializes the Page property by calling the base class constructor</summary>
        public LoadBoardLaneTransLoadsController() : base(Utilities.PageEnum.LoadBoard) { }

        #endregion

        #region " Properties"

        /// <summary>This property is used for logging and error tracking</summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.LoadBoardLaneTransLoadsController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"


        private Models.LaneTransLoadXref selectModelData(LTS.vLaneTransLoadXref d)
        {
            Models.LaneTransLoadXref modelRecord = new Models.LaneTransLoadXref();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "LaneTranXUpdated", "vLaneTransLoadXrefDets" };
                string sMsg = "";
                modelRecord = (Models.LaneTransLoadXref)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                //When using Updated field with the DTO objects the data has already been converted to an array
                //so replace the LTL logic 
                //if (modelRecord != null) { modelRecord.setUpdated(d.LaneTranXUpdated.ToArray()); }
                // with the code below (just remove the ToArray
                if (modelRecord != null) { modelRecord.setUpdated(d.LaneTranXUpdated.ToArray()); }
            }

            return modelRecord;
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
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                int RecordCount = 0;
                int count = 0;
                DAL.Models.AllFilters f = new DAL.Models.AllFilters(); 
                f.ParentControl = id;
                string sRetMsg = "";
                savePageSetting(id.ToString(), ref sRetMsg, "LoadBoardLaneTransLoadLaneControl");
                
                DAL.NGLLaneTransLoadXrefData oDAL = new DAL.NGLLaneTransLoadXrefData(Parameters);
                Models.LaneTransLoadXref[] records = new Models.LaneTransLoadXref[] { };

                LTS.vLaneTransLoadXref[] oData = new LTS.vLaneTransLoadXref[] { };
                oData = oDAL.GetLaneTransLoadXrefs365(f, ref RecordCount);

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
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);

                //save the page filter for the next time the page loads
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter); }
                LTS.tblUserPageSetting[] pSettings = readPageSettings("LoadBoardLaneTransLoadLaneControl");
                string sUserPSMetaData = "0";
                if (pSettings != null && pSettings.Length > 0) {
                    sUserPSMetaData = pSettings[0].UserPSMetaData; 
                }
                int iParent = 0;
                int.TryParse(sUserPSMetaData, out iParent);
                f.ParentControl = iParent;

                DAL.NGLLaneTransLoadXrefData oDAL = new DAL.NGLLaneTransLoadXrefData(Parameters);

                Models.LaneTransLoadXref[] records = new Models.LaneTransLoadXref[] { };
                LTS.vLaneTransLoadXref[] oData = new LTS.vLaneTransLoadXref[] { };
                oData = oDAL.GetLaneTransLoadXrefs365(f, ref RecordCount);

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



        #endregion
    }
}