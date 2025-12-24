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
using TAR = NGL.FM.CarTar;

namespace DynamicsTMS365.Controllers
{
    public class EstimateCarrierCostController : NGLControllerBase
    {

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.EstimateCarrierCostController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " REST Services"

        public Models.Response Get(int BookControl)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            try
            {
                Models.Carrier[] carriers = new Models.Carrier[] { };
                int count = 0; // _context.Carriers.Count();

                // get the take and skip parameters int skip = request["skip"] == null ? 0 :
                int skip = request["skip"] == null ? 0 : int.Parse(request["skip"]);
                int take = request["take"] == null ? 100 : int.Parse(request["take"]);
                //DTO.Carrier dtoCarriers()

                TAR.BookRev tarBookRev = new TAR.BookRev(Parameters);
                DTO.CarrierCostResults oResults = tarBookRev.estimatedCarriersByCost(BookControl);
                if (oResults != null && oResults.CarriersByCost != null && oResults.CarriersByCost.Count() > 0)
                {
                    response = new Models.Response(oResults.CarriersByCost.ToArray(), count);
                }
                
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

        public Models.Response CNSShop(Models.LoadStop[] stops)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            try
            {


                Models.Carrier[] carriers = new Models.Carrier[] { };
                int count = 0; // _context.Carriers.Count();

                // get the take and skip parameters int skip = request["skip"] == null ? 0 :
                int skip = request["skip"] == null ? 0 : int.Parse(request["skip"]);
                int take = request["take"] == null ? 100 : int.Parse(request["take"]);
                //DTO.Carrier dtoCarriers()

                //DTO.Carrier[] dtocarriers = NGLCarrierData.GetCarriersFiltered(1, null, true, 1, 0, skip, take);
                DAL.NGLCarrierData dalCarData = new DAL.NGLCarrierData(Parameters);
                DTO.Carrier[] dtocarriers = dalCarData.GetCarriersFiltered(0, null, true, 1, 0, skip, take);

                if (dtocarriers != null && dtocarriers.Count() > 0)
                {
                    //RecordCount contains the nunber of records in the database that matches the filters
                    count = dtocarriers[0].RecordCount;
                    carriers = (from e in dtocarriers
                                orderby e.CarrierName
                                select new Models.Carrier
                                {
                                    Control = e.CarrierControl,
                                    Name = e.CarrierName,
                                    Number = e.CarrierNumber,
                                    CarrierSCAC = e.CarrierSCAC,
                                    CarrierModDate = e.CarrierModDate,
                                    CarrierModUser = e.CarrierModUser
                                }).ToArray();
                }

                response = new Models.Response(carriers, count);
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

        public Models.Response Post(int id)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            try
            {
                DTO.Carrier dtocarrier = NGLCarrierData.GetCarrier(id);


                // if there was an carrier returned from the database
                if (dtocarrier != null)
                {

                    // update the carrier object handling null values or empty strings
                    dtocarrier.CarrierName = string.IsNullOrEmpty(request["Name"]) ? dtocarrier.CarrierName : request["Name"];
                    dtocarrier.CarrierNumber = string.IsNullOrEmpty(request["Number"]) ? dtocarrier.CarrierNumber : int.Parse(request["Number"]);
                    dtocarrier.CarrierSCAC = string.IsNullOrEmpty(request["CarrierSCAC"]) ? dtocarrier.CarrierSCAC : request["CarrierSCAC"];
                    dtocarrier.CarrierModDate = string.IsNullOrEmpty(request["CarrierModDate"]) ? dtocarrier.CarrierModDate : Convert.ToDateTime(request["CarrierModDate"]);
                    NGLCarrierData.UpdateRecordNoReturn(dtocarrier);
                    // set the server response to OK
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    // we couldn't find the carrier with the passed in id
                    // set the response status to error and return a message
                    // with some more info.
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Errors = string.Format("The carrier with control number {0} was not found in the database", id.ToString());
                }
            }
            catch (Exception ex)
            {
                // something went wrong - possibly a database error. return a
                // 500 server error and send the details of the exception.
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Errors = string.Format("The database updated failed: {0}", ex.Message);
                //response.
            }

            // return the HTTP Response.
            return response;
        }

        public Models.Response Delete(int id)
        {
            try
            {

                DTO.Carrier carrierToDelete = NGLCarrierData.GetCarrier(id);

                int? Number = carrierToDelete.CarrierNumber;
                // delete the employee from the context
                //_context.Employees.DeleteOnSubmit(employeeToDelete);

                // submit the changes
                //_context.SubmitChanges();

                // if a valid employee object was found by id
                if (carrierToDelete != null)
                {
                    return new Models.Response(string.Format("You are not authorized to delete carrier data", id.ToString()));
                    //// mark the object for deletion
                    //_context.Carriers.DeleteOnSubmit(carrierToDelete);
                    //// delete the object from the database
                    //_context.SubmitChanges();

                    //// return an empty Models.Response object (this returns a 200 OK)
                    //return new Models.Response();
                }
                else
                {
                    // otherwise set the error field of a response object and return it.
                    return new Models.Response(string.Format("The carrier with control number {0} was not found in the database", id.ToString()));
                }
            }
            catch (Exception ex)
            {
                // something went wrong. set the errors field of
                return new Models.Response(string.Format("There was an error updating carrier with control number {0}: {1}", id.ToString(), ex.Message));
            }

        }

        #endregion

    }
}