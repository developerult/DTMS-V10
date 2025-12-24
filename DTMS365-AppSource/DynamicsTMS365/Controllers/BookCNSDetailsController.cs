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
    public class BookCNSDetailsController : NGLControllerBase
    {

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.BookCNSDetailsController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"


        private Models.Order selectModelData(LTS.vBookOrder d)
        {
            Models.Order order = new Models.Order();
            List<string> skipObjs = new List<string> {"ShipKey" };
            string sMsg = "";
            order = (Models.Order)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(order, d, skipObjs, ref sMsg);
            if (string.IsNullOrWhiteSpace(order.BookSHID))
            {
                order.ShipKey = order.BookConsPrefix;
            }
            else
            {
                order.ShipKey = order.BookSHID;
            }
            return order;
        }




        #endregion

        #region " REST Services"

        public Models.Response Get(string id)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            try
            {
                Models.Order[] orders = new Models.Order[] { };
                int count = 0;
                //Filtered Parameters
                int BookControl = 0;
                
               // not used? -- DAL.Utilities.NGLDateFilterType datefilterType = DAL.Utilities.NGLDateFilterType.None;               
                var ltsOrders = new List<LTS.vBookOrder>();
                DAL.NGLBookData dalBookData = new DAL.NGLBookData(Parameters);
                if (int.TryParse(id, out BookControl))
                {
                    LTS.vBookOrder ltsOrder = dalBookData.GetLoadOrder(BookControl);
                    if (ltsOrder != null && ltsOrder.BookControl != 0){
                        ltsOrders.Add(ltsOrder);
                    }

                }
                else
                {
                    LTS.vBookOrder[] ltsArrayofOrders = dalBookData.GetLoadOrders(id);
                    if (ltsArrayofOrders != null && ltsArrayofOrders.Count() > 0)
                    {
                        ltsOrders = ltsArrayofOrders.ToList();
                    }
                }
                


                if (ltsOrders != null && ltsOrders.Count() > 0)
                {                                       
                    orders = (from e in ltsOrders
                              orderby e.BookDateLoad descending
                              select selectModelData(e)).ToArray();
                }
                count = orders.Count();
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

        //Read only view add logic for post somewhere else
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