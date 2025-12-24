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
    //
    public class cmLocalizeKeyValuePairController : NGLControllerBase
    {

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.cmLocalizeKeyValuePairController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion


        #region " Data Translation"


        private Models.cmLocalizeKeyValuePair selectModelData(LTS.cmLocalizeKeyValuePair d)
        {
            Models.cmLocalizeKeyValuePair modelRecord = new Models.cmLocalizeKeyValuePair();
            if (d != null)
            { 
                List<string> skipObjs = new List<string> {"cmLocalUpdated" };
                string sMsg = "";
                modelRecord = (Models.cmLocalizeKeyValuePair)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.cmLocalUpdated.ToArray()); }
            }

            return modelRecord;
        }


        private  LTS.cmLocalizeKeyValuePair selectLTSData(  Models.cmLocalizeKeyValuePair d)
        {
            LTS.cmLocalizeKeyValuePair ltsRecord = new LTS.cmLocalizeKeyValuePair();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "cmLocalUpdated" };
                string sMsg = "";
                ltsRecord = (LTS.cmLocalizeKeyValuePair)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null) 
                { 
                     byte[] bupdated = d.getUpdated();
                     ltsRecord.cmLocalUpdated = bupdated == null ? new byte[0] : bupdated;

                }
            }

            return ltsRecord;
        }




        #endregion


        #region " REST Services"

        public Models.Response Get()
        {
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            try
            {
                Models.cmLocalizeKeyValuePair[] oData = new Models.cmLocalizeKeyValuePair[] { };
                //count will contains the nunber of records in the database that matches the filters before paging
                int count = 0; // _context.Carriers.Count();

                // get the take and skip parameters int skip = request["skip"] == null ? 0 :
                int skip = request["skip"] == null ? 0 : int.Parse(request["skip"]);
                int take = request["take"] == null ? 100 : int.Parse(request["take"]);
                DAL.NGLcmLocalizeKeyValuePairData dalData = new DAL.NGLcmLocalizeKeyValuePairData(Parameters);
                //List<LTS.cmLocalizeKeyValuePair> oRecords = new List<LTS.cmLocalizeKeyValuePair>();
                List<LTS.cmLocalizeKeyValuePair> oRecords = dalData.GetPage(skip, take, ref count);
                if (oRecords != null && oRecords.Count() > 0)
                {

                    oData = (from e in oRecords
                              orderby e.cmLocalValue descending
                              select selectModelData(e)).ToArray();
                }


                response = new Models.Response(oData, count);
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
                DAL.NGLcmLocalizeKeyValuePairData dalData = new DAL.NGLcmLocalizeKeyValuePairData(Parameters);

                Models.cmLocalizeKeyValuePair oModel = new Models.cmLocalizeKeyValuePair();
                oModel.fillFromRequest(request);
                LTS.cmLocalizeKeyValuePair ltsRecord = selectLTSData(oModel);
                if (id == 0)
                {
                    LTS.cmLocalizeKeyValuePair ltsResults = dalData.Insert(ltsRecord);
                }
                else
                {
                    LTS.cmLocalizeKeyValuePair ltsResults = dalData.Save(ltsRecord);
                }
                
                if (ltsRecord != null && ltsRecord.cmLocalControl != 0)
                {
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Errors = string.Format("The changes to the Localized Key Value Pair with control number {0} could not be saved", id.ToString());

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
            // create a response message to send back
            var response = new Models.Response(); //new HttpResponseMessage();
            bool blnSuccess = false;
            try
            {
                if (id == 0)
                {
                    response.StatusCode = HttpStatusCode.OK;
                    // return the HTTP Response.
                    return response;
                }
                DAL.NGLcmLocalizeKeyValuePairData dalData = new DAL.NGLcmLocalizeKeyValuePairData(Parameters);

                Models.cmLocalizeKeyValuePair oModel = new Models.cmLocalizeKeyValuePair();
                oModel.fillFromRequest(request);
                if (oModel.cmLocalControl == 0)
                {
                    //try to delete using the id
                    //no support for optimistic concurrency, just delete it
                    blnSuccess = dalData.Delete(id);
                }
                else
                {
                    LTS.cmLocalizeKeyValuePair ltsRecord = selectLTSData(oModel);
                    //provides support for optimistic concurrency
                    blnSuccess = dalData.Delete(ltsRecord);
                }
                if (blnSuccess)
                {
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Errors = string.Format("Cannot delete the Localized Key Value Pair with control number {0}", id.ToString());
                    
                }
               
            }
            catch (Exception ex)
            {
                // something went wrong. set the errors field of
                return new Models.Response(string.Format("There was an error deleting the  Localized Key Value Pair with control number {0}: {1}", id.ToString(), ex.Message));
            }
            // return the HTTP Response.
            return response;
        }

        #endregion

    }
}