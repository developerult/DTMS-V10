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
using DTran = Ngl.Core.Utility.DataTransformation;

namespace DynamicsTMS365.Controllers
{
    public class BookTrackController : NGLControllerBase
    {
        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.BookTrackController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion


        #region " Data Translation"

        private Models.BookTrack selectModelData(LTS.vBookTrack d)
        {
            Models.BookTrack modelRecord = new Models.BookTrack();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "BookTrackUpdated", "_BookTrackDetails", "BookTrackDetails", "_Book", "Book" };
                string sMsg = "";
                modelRecord = (Models.BookTrack)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.BookTrackUpdated.ToArray()); }
            }
            return modelRecord;
        }

        #endregion


        #region " REST Services"

        [HttpGet, ActionName("GetAllCommentsHoverOverData")]
        public Models.Response GetAllCommentsHoverOverData(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                string formattedComments = "No comments for this item.";
                LTS.spGetAllCommentsHoverOverDataResult[] bookTracks = NGLBookTrackData.GetAllCommentsHoverOverData(Parameters.UserControl, id);
                if (bookTracks?.Count() > 0)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    foreach (LTS.spGetAllCommentsHoverOverDataResult bt in bookTracks)
                    {
                        string strDates = "";
                        if (bt.BookTrackDate != null) { strDates = string.Format(" on {0} at {1}", bt.BookTrackDate.Value.ToString("MM/dd/yyyy"), bt.BookTrackDate.Value.ToShortTimeString()); }
                        string[] p = new string[] { bt.BookTrackComment, bt.BookTrackContact, strDates };
                        sb.Append(string.Format("<div style='border-bottom:solid 1px #ccc;padding:5px;'>{0}<br /><span style='color:#666;font-size:95%;'>{1}{2}</span></div>", p));
                    }
                    formattedComments = sb.ToString();
                }
                string[] records = new string[1] { formattedComments };
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

        /// POST 	/API/objectcontroller{data}  : Create a new object or Update a the current object if the control number exists
        /// GET 	/API/objectcontroller/{id}   : Get the object information where the control number = "id"
        /// PUT 	/API/objectcontroller/{data} : Update the object information stored in data
        /// DELETE	/API/objectcontroller/{id}   : Delete object where the control number = "id"


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
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter, "BookTrackGridFilter"); } //save the page filter for the next time the page loads
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                Models.BookTrack[] records = new Models.BookTrack[] { };
                int RecordCount = 0;
                int count = 0;
                LTS.vBookTrack[] oData = NGLBookTrackData.GetBookTracksBySHID(ref RecordCount, f);
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