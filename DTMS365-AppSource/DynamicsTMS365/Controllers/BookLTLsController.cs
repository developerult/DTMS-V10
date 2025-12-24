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
    public class BookLTLsController : NGLControllerBase
    {
        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.BookLTLsController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        
    private Models.Order  selectModelData(LTS.vBookLTL d){
        Models.Order order = new Models.Order();        
        List<string> skipObjs = new List<string> {};
        string sMsg = "";
        order = (Models.Order)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(order,d,skipObjs,ref sMsg);
        return order;
    }

       
        private Models.Order selectModelData(LTS.spGetUniqueBookingAddressListResult d)
        {
            Models.Order order = new Models.Order();
            List<string> skipObjs = new List<string> { };
            string sMsg = "";
            order = (Models.Order)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(order, d, skipObjs, ref sMsg);
            return order;
        }
        private Models.Order selectModelData(LTS.vBookOrder d)
        {
            Models.Order order = new Models.Order();
            List<string> skipObjs = new List<string> { };
            string sMsg = "";
            order = (Models.Order)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(order, d, skipObjs, ref sMsg);
            return order;
        }




        #endregion

        #region " REST Services"

        public Models.Response Get()
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            try
            {
                Models.Order[] orders = new Models.Order[] { };
                int count = 0; 

                // get the take and skip parameters int skip = request["skip"] == null ? 0 :
                int skip = request["skip"] == null ? 0 : int.Parse(request["skip"]);
                int take = request["take"] == null ? 100 : int.Parse(request["take"]);

                //Filtered Parameters
               int BookCustCompControl = 0;
               int BookCarrierControl = 0;
               DAL.Utilities.NGLDateFilterType datefilterType   = DAL.Utilities.NGLDateFilterType.None;
               DateTime? StartDate = null;
               DateTime? EndDate = null;
               Boolean LaneOriginAddressUse  = false; 
               string sortExpression  = "BookDateLoad Desc";
               int RecordCount = 0;

                DAL.NGLBookData dalBookData = new DAL.NGLBookData(Parameters);
                LTS.vBookLTL[] ltsLTLs = dalBookData.GetBookLTLFiltered( ref RecordCount,
                                                                        BookCustCompControl, 
                                                                        BookCarrierControl, 
                                                                        datefilterType,
                                                                        StartDate,
                                                                        EndDate,
                                                                        LaneOriginAddressUse,
                                                                        sortExpression, 
                                                                        1, 
                                                                        0, 
                                                                        skip, 
                                                                        take);


                if (ltsLTLs != null && ltsLTLs.Count() > 0)
                {
                    //RecordCount contains the nunber of records in the database that matches the filters
                    count = RecordCount;
                    orders = (from e in ltsLTLs
                                orderby e.BookDateLoad descending
                                select selectModelData(e)).ToArray();
                }

                response = new Models.Response(orders, count);
            }
            catch (Exception ex)
            {
                // something went wrong - possibly a database error. return a
                // 500 server error and send the details of the exception.
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Errors = string.Format("The database read failed: {0}", ex.Message);
                //response.
            }

            // return the HTTP Response.
            return response;

        }


        public Models.Response Get(string filter)
        {
            

            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            try
            {

                DAL.Models.OrderFilter ofilter = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.OrderFilter>(filter);
                Models.Order[] orders = new Models.Order[] { };
                int count = 0;

                // get the take and skip parameters int skip = request["skip"] == null ? 0 :
                int skip = request["skip"] == null ? 0 : int.Parse(request["skip"]);
                int take = request["take"] == null ? 100 : int.Parse(request["take"]);

                //Filtered Parameters
                //int BookCustCompControl = 0;
                //int BookCarrierControl = 0;
                //DAL.Utilities.NGLDateFilterType datefilterType = DAL.Utilities.NGLDateFilterType.None;
                //DateTime? StartDate = null;
                //DateTime? EndDate = null;
                //Boolean LaneOriginAddressUse = false;
                string sortExpression = "BookDateLoad Desc";
                int RecordCount = 0;

                DAL.NGLBookData dalBookData = new DAL.NGLBookData(Parameters);
                LTS.spGetUniqueBookingAddressListResult[] orderAddresses = dalBookData.GetUniqueBookingAddressListFiltered(ref RecordCount,
                                                                        ofilter,
                                                                        sortExpression,
                                                                        skip,
                                                                        take);
                //LTS.vBookOrder[] ltsOrders = dalBookData.GetOrdersFiltered(ref RecordCount,
                //                                                        ofilter,
                //                                                        sortExpression,
                //                                                        skip,
                //                                                        take);


                if (orderAddresses != null && orderAddresses.Count() > 0)
                {
                    //RecordCount contains the nunber of records in the database that matches the filters
                    count = RecordCount;
                    orders = (from e in orderAddresses
                              select selectModelData(e)).ToArray();
                }

                response = new Models.Response(orders, count);
            }
            catch (FaultException<DAL.SqlFaultInfo> ex) { 
                response.StatusCode = HttpStatusCode.BadRequest;
                //Note: this error handler is english only.  We need to add code to 
                // support localization text
                string sMsg = ex.Detail.ToString(ex.Reason.ToString());
                response.Errors = string.Format("Get Orders Filtered Failed: {0}", sMsg);
            }
            catch (Exception ex)
            {
                // something went wrong - possibly a database error. return a
                // 500 server error and send the details of the exception.
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Errors = string.Format("The database read failed: {0}", ex.Message);
                //response.
            }

            // return the HTTP Response.
            return response;

        }

        //[HttpGet, ActionName("GetLTLOrdersFiltered")]
        //public Models.Response GetLTLOrdersFiltered([System.Web.Http.FromUri]Models.OrderFilter filter)

        //{
        //    // create a response message to send back
        //    var response = new Models.Response(); //new HttpResponseMessage();
        //    try
        //    {
        //        Models.Order[] orders = new Models.Order[] { };
        //        int count = 0;

        //        // get the take and skip parameters int skip = request["skip"] == null ? 0 :
        //        int skip = request["skip"] == null ? 0 : int.Parse(request["skip"]);
        //        int take = request["take"] == null ? 10 : int.Parse(request["take"]);
        //        if (filter == null) { filter = new Models.OrderFilter(); filter.FromDate = DateTime.Now; filter.ToDate = DateTime.Now.AddDays(5); }
        //        //Filtered Parameters
        //        int BookCustCompControl = filter.BookCustCompControl;
        //        int BookCarrierControl = filter.BookCarrierControl;
        //        DAL.Utilities.NGLDateFilterType datefilterType = DAL.Utilities.NGLDateFilterType.None;
        //        DateTime? StartDate = filter.FromDate;
        //        DateTime? EndDate = filter.ToDate;
        //        Boolean LaneOriginAddressUse = false; // filter.LaneOriginAddressUse;
        //        string sortExpression = "BookDateLoad Desc";
        //        int RecordCount = 0;

        //        DAL.NGLBookData dalBookData = new DAL.NGLBookData(Parameters);
        //        LTS.vBookLTL[] ltsLTLs = dalBookData.GetBookLTLFiltered(ref RecordCount,
        //                                                                BookCustCompControl,
        //                                                                BookCarrierControl,
        //                                                                datefilterType,
        //                                                                StartDate,
        //                                                                EndDate,
        //                                                                LaneOriginAddressUse,
        //                                                                sortExpression,
        //                                                                1,
        //                                                                0,
        //                                                                skip,
        //                                                                take);


        //        if (ltsLTLs != null && ltsLTLs.Count() > 0)
        //        {
        //            //RecordCount contains the nunber of records in the database that matches the filters
        //            count = RecordCount;
        //            orders = (from e in ltsLTLs
        //                      orderby e.BookDateLoad descending
        //                      select selectModelData(e)).ToArray();
        //        }

        //        response = new Models.Response(orders, count);
        //    }
        //    catch (Exception ex)
        //    {
        //        // something went wrong - possibly a database error. return a
        //        // 500 server error and send the details of the exception.
        //        response.StatusCode = HttpStatusCode.InternalServerError;
        //        response.Errors = string.Format("The database read failed: {0}", ex.Message);
        //        //response.
        //    }

        //    // return the HTTP Response.
        //    return response;

        //}
        ////Read only view add logic for post somewhere else

        public Models.Response Post(int id)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
        //    try
        //    {
        //        DTO.Carrier dtocarrier = NGLCarrierData.GetCarrier(id);


        //        // if there was an carrier returned from the database
        //        if (dtocarrier != null)
        //        {

        //            // update the carrier object handling null values or empty strings
        //            dtocarrier.CarrierName = string.IsNullOrEmpty(request["Name"]) ? dtocarrier.CarrierName : request["Name"];
        //            dtocarrier.CarrierNumber = string.IsNullOrEmpty(request["Number"]) ? dtocarrier.CarrierNumber : int.Parse(request["Number"]);
        //            dtocarrier.CarrierSCAC = string.IsNullOrEmpty(request["CarrierSCAC"]) ? dtocarrier.CarrierSCAC : request["CarrierSCAC"];
        //            dtocarrier.CarrierModDate = string.IsNullOrEmpty(request["CarrierModDate"]) ? dtocarrier.CarrierModDate : Convert.ToDateTime(request["CarrierModDate"]);
        //            NGLCarrierData.UpdateRecordNoReturn(dtocarrier);
        //            // set the server response to OK
        //            response.StatusCode = HttpStatusCode.OK;
        //        }
        //        else
        //        {
        //            // we couldn't find the carrier with the passed in id
        //            // set the response status to error and return a message
        //            // with some more info.
        //            response.StatusCode = HttpStatusCode.InternalServerError;
        //            response.Errors = string.Format("The carrier with control number {0} was not found in the database", id.ToString());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // something went wrong - possibly a database error. return a
        //        // 500 server error and send the details of the exception.
        //        response.StatusCode = HttpStatusCode.InternalServerError;
        //        response.Errors = string.Format("The database updated failed: {0}", ex.Message);
        //        //response.
        //    }

        //    // return the HTTP Response.
            return response;
        }

        public Models.Response Delete(int id)
        {
            return null;
        }

        //Read only view add logic for Delete somewhere else
        //public Models.Response Delete(int id)
        //{
        //    try
        //    {

        //        DTO.Carrier carrierToDelete = NGLCarrierData.GetCarrier(id);

        //        int? Number = carrierToDelete.CarrierNumber;
        //        // delete the employee from the context
        //        //_context.Employees.DeleteOnSubmit(employeeToDelete);

        //        // submit the changes
        //        //_context.SubmitChanges();

        //        // if a valid employee object was found by id
        //        if (carrierToDelete != null)
        //        {
        //            return new Models.Response(string.Format("You are not authorized to delete carrier data", id.ToString()));
        //            //// mark the object for deletion
        //            //_context.Carriers.DeleteOnSubmit(carrierToDelete);
        //            //// delete the object from the database
        //            //_context.SubmitChanges();

        //            //// return an empty Models.Response object (this returns a 200 OK)
        //            //return new Models.Response();
        //        }
        //        else
        //        {
        //            // otherwise set the error field of a response object and return it.
        //            return new Models.Response(string.Format("The carrier with control number {0} was not found in the database", id.ToString()));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // something went wrong. set the errors field of
        //        return new Models.Response(string.Format("There was an error updating carrier with control number {0}: {1}", id.ToString(), ex.Message));
        //    }

        //}

        #endregion
    }
}