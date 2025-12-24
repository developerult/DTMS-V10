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
    public class BookItemDetailsController : NGLControllerBase
    {

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.BookItemDetails";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"


        private Models.OrderDetail selectModelData(LTS.vBookOrderDetail d)
        {
            Models.OrderDetail oRecord = new Models.OrderDetail();
            List<string> skipObjs = new List<string> { "ShipKey" };
            string sMsg = "";
            oRecord = (Models.OrderDetail)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(oRecord, d, skipObjs, ref sMsg);
            
            return oRecord;
        }




        #endregion

        #region " REST Services"

        public Models.Response Get(string id)
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            try
            {
                Models.OrderDetail[] oRecords = new Models.OrderDetail[] { };
                int count = 0;
                //Filtered Parameters
                int BookControl = 0;

                // not used? -- DAL.Utilities.NGLDateFilterType datefilterType = DAL.Utilities.NGLDateFilterType.None;
                LTS.vBookOrderDetail[] ltsRecords = new LTS.vBookOrderDetail[] {};
                DAL.NGLBookData dalBookData = new DAL.NGLBookData(Parameters);
                if (int.TryParse(id, out BookControl))
                {
                   ltsRecords = dalBookData.GetOrderDetails(BookControl);                    

                }
                
                if (ltsRecords != null && ltsRecords.Count() > 0)
                {
                    oRecords = (from e in ltsRecords
                                orderby e.BookLoadPONumber, e.BookItemItemNumber 
                              select selectModelData(e)).ToArray();
                }
                count = oRecords.Count();
                response = new Models.Response(oRecords, count);
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

        

        #endregion

    }
}