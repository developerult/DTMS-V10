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
    public class BookItemController : NGLControllerBase
    {

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.BookItemController";
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


        [HttpGet, ActionName("Get")]
        public Models.Response Get(int id)
        {
            var response = new Models.Response(); //create a new HttpResponseMessage() to send back
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DTO.BookItem[] bookItems = NGLBookItemData.GetBookItemsFiltered(id);
                int count = 0;
                if (bookItems != null && bookItems.Count() > 0)
                {
                    count = bookItems.Count();
                    //foreach (DTO.BookItem i in bookItems)
                    //{
                    //    if (string.IsNullOrWhiteSpace(i.BookItemOrderNumber))
                    //    {
                    //        i.BookItemOrderNumber = "Test";
                    //    }
                    //}
                }
                response = new Models.Response(bookItems, count);
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



        //[HttpGet, ActionName("GetBookItemsByBookControl")]
        //public Models.Response GetBookItemsByBookControl(int id)
        //{
        //    var response = new Models.Response(); //create a new HttpResponseMessage() to send back
        //    if (!authenticateController(ref response)) { return response; }
        //    try
        //    {
        //        DTO.BookItem[] bookItems = NGLBookItemData.GetBookItemsFiltered(id);
        //        int count = 0;
        //        if (bookItems != null && bookItems.Count() > 0)
        //        {
        //            count = bookItems.Count();
        //            //foreach (DTO.BookItem i in bookItems)
        //            //{
        //            //    if (string.IsNullOrWhiteSpace(i.BookItemOrderNumber))
        //            //    {
        //            //        i.BookItemOrderNumber = "Test";
        //            //    }
        //            //}
        //        }
        //        response = new Models.Response(bookItems, count);
        //    }
        //    catch (Exception ex)
        //    {
        //        FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
        //        response.StatusCode = fault.StatusCode;
        //        response.Errors = fault.formatMessage();
        //        return response;
        //    }
        //    return response;
        //}

        #endregion
    }
}