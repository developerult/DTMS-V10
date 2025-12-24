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
    public class CarriersController : NGLControllerBase
    {

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.CarriersController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " REST Services"

        //I don't think this is being used anywhere
        //From tblLegalEntityCarrier
        [HttpGet, ActionName("GetCarriersForLegalEntity")]
        public Models.Response GetCarriersForLegalEntity(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                LTS.vLECarCarrier[] retVals = new LTS.vLECarCarrier[] { };
                DAL.NGLLegalEntityCarrierData oLook = new DAL.NGLLegalEntityCarrierData(Parameters);
                int RecordCount = 0;
                int count = 0;          
                // get the take and skip parameters int skip = request["skip"] == null ? 0 :
                int skip = request["skip"] == null ? 0 : int.Parse(request["skip"]);
                int take = request["take"] == null ? 500 : int.Parse(request["take"]);
                string sortExpression = "CarrierName Asc";
                string filterWhere = request["filter[filters][0][value]"];
                int LEControl = id; //Note: This will always be 0 for non-SuperUsers
                if (LEControl == 0)
                {
                    LEControl = Parameters.UserLEControl; //if LEControl param is 0 set it to the User's LEControl
                }
                retVals = oLook.GetCarriersForLegalEntity(ref RecordCount, LEControl, filterWhere, sortExpression, 1, 0, skip, take);
                if (retVals != null && retVals.Count() > 0)
                {
                    count = retVals.Length;
                }
                response = new Models.Response(retVals, count);
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


        [HttpGet, ActionName("GetCarriersWAccessorialMaintConfigsToCopy")]
        public Models.Response GetCarriersWAccessorialMaintConfigsToCopy(int id)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                List<Models.Carrier> carriers = new List<Models.Carrier>();
                DAL.NGLLookupDataProvider oLook = new DAL.NGLLookupDataProvider(Parameters);
                int LEControl = id; //Note: This will always be 0 for non-SuperUsers/admins
                if (LEControl == 0)
                {
                    LEControl = Parameters.UserLEControl; //if LEControl param is 0 set it to the User's LEControl
                }
                var spRet = oLook.GetCarriersWAccessorialMaintConfigsToCopy(LEControl);
                if (spRet != null && spRet.Count() > 0)
                {
                    if (spRet[0].ErrNumber != 0)
                    {
                        response.StatusCode = HttpStatusCode.InternalServerError;
                        response.Errors = spRet[0].RetMsg;
                    }
                    else
                    {
                        foreach (LTS.spGetCarriersWAccessorialMaintConfigsToCopyResult r in spRet)
                        {                    
                            if (r.CarrierControl.HasValue && r.CarrierControl.Value != 0)
                            {
                                Models.Carrier c = new Models.Carrier();
                                c.Control = r.CarrierControl.Value;
                                c.Name = r.CarrierName;
                                carriers.Add(c);
                            }                          
                        }
                        response = new Models.Response(carriers.ToArray(), carriers.Count);
                    }
                }
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


        [HttpPost, ActionName("CopyCarrierAccessorialMaintConfig")]
        public Models.Response CopyCarrierAccessorialMaintConfig([System.Web.Http.FromBody]Models.GenericResult g)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLLookupDataProvider oLook = new DAL.NGLLookupDataProvider(Parameters);
                int LEControl = g.Control; //Note: This will always be 0 for non-SuperUsers
                if (LEControl == 0)
                {
                    LEControl = Parameters.UserLEControl; //if LEControl param is 0 set it to the User's LEControl
                }
                int CopyToCarrierControl = g.intField1;
                int CopyFromCarrierControl = g.intField2;
                LTS.spCopyCarrierAccessorialMaintConfigResult spRet = oLook.CopyCarrierAccessorialMaintConfig(CopyToCarrierControl, CopyFromCarrierControl, LEControl);
                if (spRet.ErrNumber == 0)
                {
                    Array d = new bool[1] { true };
                    response = new Models.Response(d, 1);
                }
                else
                {
                    //** TODO LVV ** Localize these messages.
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Errors = spRet.RetMsg;
                }
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