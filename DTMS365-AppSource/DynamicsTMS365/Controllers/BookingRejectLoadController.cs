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
using DModel = Ngl.FreightMaster.Data.Models;

namespace DynamicsTMS365.Controllers
{
    public class BookingRejectLoadController : NGLControllerBase
    {
        #region " Constructors "
        /// <summary>
        /// Created by RHR for v-8.2 on 08/29/2018 initializes the Page property by calling the base class constructor
        /// </summary>
        public BookingRejectLoadController()
                : base(Utilities.PageEnum.LoadBoard)
        {
        }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.BookingRejectLoadController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"



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
                ////int RecordCount = 0;
                int count = 0;
                ////DAL.Models.AllFilters f = new DAL.Models.AllFilters();
                ////f.filterName = "BookControl";
                ////f.filterValue = id.ToString();
                ////DAL.NGLBookLoadBoard oDAL = new DAL.NGLBookLoadBoard(Parameters);

                LTS.vBookLoadBoard[] oData = new LTS.vBookLoadBoard[] { };
                ////oData = oDAL.GetBookLoadBoards(f, ref RecordCount);
                ////if (oData != null && oData.Count() > 0)
                ////{
                ////    count = oData.Count();
                ////    if (RecordCount > count) { count = RecordCount; }
                ////}
                response = new Models.Response(oData, count);
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

        [HttpGet, ActionName("GetByParent")]
        public Models.Response GetByParent(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                Models.BookTrack[] records = new Models.BookTrack[] { };
                int count = 0;
                DModel.BookingMenuInfo b = NGLBookData.GetBookingMenuInfo(id);
                if (b != null)
                {
                    count = 1;
                    Models.BookTrack bt = new Models.BookTrack { BookTrackBookControl = b.BookControl, CarrierNumber = b.CarrierNumber, BookProNumber = b.BookProNumber, BookTrackDate = DateTime.Now, BookTrackContact = Parameters.UserName };
                    records = new Models.BookTrack[] { bt };
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
                ////int RecordCount = 0;
                int count = 0;
                //////save the page filter for the next time the page loads
                ////if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter); }
                ////DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
                ////DAL.NGLBookLoadBoard oDAL = new DAL.NGLBookLoadBoard(Parameters);
                LTS.vBookLoadBoard[] oData = new LTS.vBookLoadBoard[] { };
                ////oData = oDAL.GetBookLoadBoards(f, ref RecordCount);
                ////if (oData != null && oData.Count() > 0)
                ////{
                ////    count = oData.Count();
                ////    if (RecordCount > count) { count = RecordCount; }
                ////}
                response = new Models.Response(oData, count);
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
        public Models.Response Post([System.Web.Http.FromBody]Models.BookTrack data)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                bool[] oRecords = new bool[1] { false };

                if (data.BookTrackBookControl == 0)
                {
                    //nothing to do.  there is a bug in the client caller
                    response = new Models.Response(oRecords, 1);
                    return response;
                }
                DTO.BookTrack bt = new DTO.BookTrack
                {
                    BookTrackBookControl = data.BookTrackBookControl,
                    BookTrackDate = data.BookTrackDate,
                    BookTrackContact = data.BookTrackContact,
                    BookTrackComment = data.BookTrackComment,
                    BookTrackStatus = data.BookTrackStatus,
                    BookTrackCommentLocalized = data.BookTrackCommentLocalized,
                    BookTrackCommentKeys = data.BookTrackCommentKeys,
                    BookTrackModUser = Parameters.UserName,
                    BookTrackModDate = DateTime.Now
                };
                var oData = NGLBookTrackData.CreateRecord(bt);

                if (oData != null)
                {
                    oRecords = new bool[1] { true };
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

        [HttpDelete, ActionName("DELETE")]
        public Models.Response DELETE(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                ////DAL.NGLBookLoadBoard oDAL = new DAL.NGLBookLoadBoard(Parameters);
                ////bool blnRet = oDAL.DeleteBookLoadBoard(id);
                bool[] oRecords = new bool[1];

                ////oRecords[0] = blnRet;

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