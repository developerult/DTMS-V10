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
using DynamicsTMS365.Models;

namespace DynamicsTMS365.Controllers
{
    public class LoadBoardLoadStatusController : NGLControllerBase
    {
        #region " Constructors "

        /// <summary> Initializes the Page property by calling the base class constructor </summary>
        public LoadBoardLoadStatusController() : base(Utilities.PageEnum.LoadBoardLoadStatus) { }

        #endregion

        #region " Properties"

        /// <summary> This property is used for logging and error tracking. </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.LoadBoardLoadStatusController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        ////private Models.BookItem selectModelData(DTO.BookItem d)
        ////{
        ////    Models.BookItem modelRecord = new Models.BookItem();
        ////    if (d != null)
        ////    {
        ////        List<string> skipObjs = new List<string> { "BookItemUpdated", "rowguid", "Book", "BookLoad" };
        ////        string sMsg = "";
        ////        modelRecord = (Models.BookItem)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
        ////        //When using Updated field with the DTO objects the data has already been converted to an array
        ////        //so replace the LTL logic 
        ////        //if (modelRecord != null) { modelRecord.setUpdated(d.BookItemUpdated.ToArray()); }
        ////        // with the code below (just remove the ToArray
        ////        if (modelRecord != null) { modelRecord.setUpdated(d.BookItemUpdated); }
        ////    }

        ////    return modelRecord;
        ////}

        ////private Models.BookItem selectModelData(LTS.BookItem d)
        ////{
        ////    Models.BookItem modelRecord = new Models.BookItem();
        ////    if (d != null)
        ////    {
        ////        List<string> skipObjs = new List<string> { "BookItemUpdated", "rowguid", "Book", "BookLoad" };
        ////        string sMsg = "";
        ////        modelRecord = (Models.BookItem)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
        ////        if (modelRecord != null) { modelRecord.setUpdated(d.BookItemUpdated.ToArray()); }
        ////    }

        ////    return modelRecord;
        ////}

        ////public static LTS.BookItem selectLTSData(Models.BookItem d)
        ////{
        ////    LTS.BookItem ltsRecord = new LTS.BookItem();
        ////    if (d != null)
        ////    {
        ////        List<string> skipObjs = new List<string> { "BookItemUpdated", "rowguid", "Book", "BookLoad" };
        ////        string sMsg = "";
        ////        ltsRecord = (LTS.BookItem)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
        ////        if (ltsRecord != null)
        ////        {
        ////            byte[] bupdated = d.getUpdated();
        ////            ltsRecord.BookItemUpdated = bupdated == null ? new byte[0] : bupdated;

        ////        }
        ////    }

        ////    return ltsRecord;
        ////}

        ////public static DTO.BookItem selectDTOData(Models.BookItem d)
        ////{
        ////    DTO.BookItem ltsRecord = new DTO.BookItem();
        ////    if (d != null)
        ////    {
        ////        List<string> skipObjs = new List<string> { "BookItemUpdated", "rowguid", "Book", "BookLoad" };
        ////        string sMsg = "";
        ////        ltsRecord = (DTO.BookItem)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
        ////        if (ltsRecord != null)
        ////        {
        ////            byte[] bupdated = d.getUpdated();
        ////            ltsRecord.BookItemUpdated = bupdated == null ? new byte[0] : bupdated;

        ////        }
        ////    }

        ////    return ltsRecord;
        ////}

        #endregion

        #region " REST Services"

        /// POST 	/API/objectcontroller{data}  : Create a new object or Update a the current object if the control number exists
        /// GET 	/API/objectcontroller/{id} : Get the object information where the control number = "id"
        /// PUT 	/API/objectcontroller/{data} : Update the object information stored in data
        /// DELETE	/API/objectcontroller/{id} : Delete object where the control number = "id"

        //Not Currently Used
        [HttpGet, ActionName("Get")]
        public Models.Response Get(int id)
        {
            //Note: CRUD not currently supported in this controller
            var response = new Models.Response(); //new HttpResponseMessage();
            response.populateDefaultInvalidFilterResponseMessage();
            return response;
            ////Note: The id must always match a BookTrackControl associated with the select Book Track Record 
            ////The system looks up the last saved Book Control pk for this user 
            ////An invalid parent key Error is returned if the data does not match
            ////If id is zero and no records are found the client should configure the page to add a new record.
            //var response = new Models.Response(); //new HttpResponseMessage();
            //if (!authenticateController(ref response)) { return response; }
            //try
            //{
            //    int RecordCount = 0;
            //    int count = 0;
            //    DAL.NGLBookItemData oDAL = new DAL.NGLBookItemData(Parameters);
            //    DTO.BookItem oData = oDAL.GetBookItemFiltered(id);
            //    Models.BookItem[] records = new Models.BookItem[1];
            //    if (oData != null)
            //    {
            //        count = 1;
            //        records[0] = selectModelData(oData);
            //    }
            //    response = new Models.Response(records, count);
            //}
            //catch (Exception ex)
            //{
            //    FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
            //    response.StatusCode = fault.StatusCode;
            //    response.Errors = fault.formatMessage();
            //    return response;
            //}
            //return response;
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
               
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter); } //save the page filter for the next time the page loads
                //Modified by RHR for v-8.5.4.004 on 12/06/2023 new BookControl setting 
                //if.BookControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.LoadBoard); //Get the BookControl
                LTS.vLBLoadStatus365[] oLTSData = new LTS.vLBLoadStatus365[] { };
                Models.vLBLoadStatus365[] records = new Models.vLBLoadStatus365[] { };

                f.BookControl = readBookControlPageSetting(f.BookControl);
                if (f.BookControl == 0)
                {
                    return new Models.Response(records, 0);
                }

                bool blnEntireLoad = false;
                if(f.Data == "true") { blnEntireLoad = true; }
                oLTSData = NGLBookTrackData.GetLoadBoardLoadStatus(ref RecordCount, f, blnEntireLoad);
                if (oLTSData?.Count() > 0)
                {
                    count = oLTSData.Count();
                    //records = (from e in oData select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                    records = (from e in oLTSData select Models.vLBLoadStatus365.selectModelData(e)).ToArray();
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

        [HttpPost, ActionName("Post")]
        public Models.Response Post([FromBody]Models.vLBLoadStatus365 data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                //Modified by RHR for v-8.5.4.004 on 12/06/2023 new BookControl setting 
                //int iBookControl = readPagePrimaryKey(Parameters, Utilities.PageEnum.LoadBoard);
                int iBookControl = data.BookTrackBookControl;
                if (iBookControl == 0)
                {
                    iBookControl = readBookControlPageSetting(iBookControl);
                    
                }
                if (iBookControl == 0)
                {
                    List<string> lDetails = new List<string>() { "Missing Booking Record Reference", " was not found. Please return to the parent page and select the data that " };
                    NGLBookTrackData.throwInvalidKeyParentRequiredException(lDetails);
                }

                DTO.BookTrack oData = new DTO.BookTrack
                {
                    BookTrackBookControl = iBookControl,
                    BookTrackComment = data.BookTrackComment,
                    BookTrackContact = data.BookTrackContact,
                    BookTrackDate = Utilities.convertStringToDateTime(data.BookTrackDate),
                    BookTrackStatus = data.LoadStatusControl.GetValueOrDefault(),
                };
                bool blnRet = false;
                DTO.BookTrack record = (DTO.BookTrack)NGLBookTrackData.CreateRecord(oData);
                if (record != null && record.BookTrackControl > 0) { blnRet = true; }
                bool[] oRecords = new bool[1] { blnRet };
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

        //Not Currently Used
        [HttpDelete, ActionName("DELETE")]
        public Models.Response DELETE(int id)
        {
            //Note: CRUD not currently supported in this controller
            var response = new Models.Response(); //new HttpResponseMessage();
            response.populateDefaultInvalidFilterResponseMessage();
            return response;
            ////var response = new Models.Response(); //new HttpResponseMessage();
            ////if (!authenticateController(ref response)) { return response; }
            ////try
            ////{
            ////    DAL.NGLBookItemData oDAL = new DAL.NGLBookItemData(Parameters);
            ////    bool blnRet = oDAL.DeleteBookItem(id);
            ////    bool[] oRecords = new bool[1];
            ////    oRecords[0] = blnRet;
            ////    response = new Models.Response(oRecords, 1);
            ////}
            ////catch (Exception ex)
            ////{
            ////    FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
            ////    response.StatusCode = fault.StatusCode;
            ////    response.Errors = fault.formatMessage();
            ////    return response;
            ////}
            ////return response;
        }


        #endregion

    }
}